# Database Configuration Guide

## Overview

The Nesi Timesheet Application uses **SQL Server** as its database provider. This guide covers all supported SQL Server configurations, from local development to cloud deployment.

## Supported SQL Server Options

1. **LocalDB** - Development on Windows (default)
2. **SQL Server Express** - Local server on Windows/Linux
3. **SQL Server in Docker** - Cross-platform development
4. **Azure SQL Database** - Cloud deployment

---

## 1. LocalDB (Windows Development - Default)

### What is LocalDB?

LocalDB is a lightweight version of SQL Server Express designed for development. It's automatically installed with:
- Visual Studio 2019+
- .NET SDK 6.0+

### Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NesiDb;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

### Setup Steps

1. **Verify LocalDB is installed:**
   ```bash
   sqllocaldb info
   ```
   You should see `mssqllocaldb` in the list.

2. **Run migrations:**
   ```bash
   cd backend/src/Nesi.Infrastructure
   dotnet ef database update --startup-project ../Nesi.Api
   ```

3. **Start the application:**
   ```bash
   cd ../Nesi.Api
   dotnet run
   ```

### LocalDB Management

**View databases:**
```bash
sqllocaldb info
```

**Stop LocalDB instance:**
```bash
sqllocaldb stop mssqllocaldb
```

**Start LocalDB instance:**
```bash
sqllocaldb start mssqllocaldb
```

**Delete database:**
```bash
sqllocaldb delete mssqllocaldb
sqllocaldb create mssqllocaldb
```

---

## 2. SQL Server Express (Windows/Linux)

### Installation

**Windows:**
1. Download [SQL Server 2022 Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Run installer and select "Basic" installation
3. Note the instance name (default: `SQLEXPRESS`)

**Linux (Ubuntu/Debian):**
```bash
# Add Microsoft repository
wget -qO- https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
sudo add-apt-repository "$(wget -qO- https://packages.microsoft.com/config/ubuntu/20.04/mssql-server-2022.list)"

# Install SQL Server
sudo apt-get update
sudo apt-get install -y mssql-server

# Configure SQL Server
sudo /opt/mssql/bin/mssql-conf setup

# Start SQL Server
sudo systemctl status mssql-server
```

### Connection String

**Windows with Windows Authentication:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=NesiDb;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

**Linux or SQL Authentication:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NesiDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

### Setup Steps

1. **Update appsettings.json** with appropriate connection string

2. **Create database (optional - migrations will create it):**
   ```sql
   CREATE DATABASE NesiDb;
   ```

3. **Run migrations:**
   ```bash
   cd backend/src/Nesi.Infrastructure
   dotnet ef database update --startup-project ../Nesi.Api
   ```

---

## 3. SQL Server in Docker (Mac/Linux/Windows)

### Why Docker?

- Cross-platform development
- Isolated database instance
- Easy setup and teardown
- Version control

### Setup Steps

1. **Install Docker Desktop** from [docker.com](https://www.docker.com/products/docker-desktop)

2. **Run SQL Server container:**
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
      -p 1433:1433 --name nesi-sqlserver \
      -d mcr.microsoft.com/mssql/server:2022-latest
   ```

3. **Verify container is running:**
   ```bash
   docker ps
   ```

4. **Update connection string** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost,1433;Database=NesiDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;MultipleActiveResultSets=true"
     }
   }
   ```

5. **Run migrations:**
   ```bash
   cd backend/src/Nesi.Infrastructure
   dotnet ef database update --startup-project ../Nesi.Api
   ```

### Docker Management

**Stop container:**
```bash
docker stop nesi-sqlserver
```

**Start container:**
```bash
docker start nesi-sqlserver
```

**Remove container:**
```bash
docker rm -f nesi-sqlserver
```

**View logs:**
```bash
docker logs nesi-sqlserver
```

**Connect with Azure Data Studio:**
- Server: `localhost,1433`
- Authentication: SQL Login
- User: `sa`
- Password: `YourStrong@Passw0rd`

---

## 4. Azure SQL Database (Production)

### Setup in Azure Portal

1. **Create Azure SQL Database:**
   - Go to [Azure Portal](https://portal.azure.com)
   - Create new "Azure SQL Database"
   - Configure server and database settings
   - Note the server name, database name, and credentials

2. **Configure Firewall:**
   - Add your IP address to firewall rules
   - Or enable "Allow Azure services"

### Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:YOUR_SERVER.database.windows.net,1433;Initial Catalog=NesiDb;Persist Security Info=False;User ID=YOUR_USER;Password=YOUR_PASSWORD;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

### Best Practices for Azure SQL

1. **Use Azure Key Vault for secrets:**
   ```csharp
   builder.Configuration.AddAzureKeyVault(
       new Uri($"https://{keyVaultName}.vault.azure.net/"),
       new DefaultAzureCredential());
   ```

2. **Use Managed Identity** (when deploying to Azure App Service):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=tcp:YOUR_SERVER.database.windows.net,1433;Initial Catalog=NesiDb;Authentication=Active Directory Managed Identity;MultipleActiveResultSets=True;"
     }
   }
   ```

3. **Enable Connection Resiliency:**
   ```csharp
   builder.Services.AddDbContext<NesiDbContext>(options =>
       options.UseSqlServer(
           connectionString,
           sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
               maxRetryCount: 5,
               maxRetryDelay: TimeSpan.FromSeconds(30),
               errorNumbersToAdd: null)));
   ```

### Run Migrations

**From local machine:**
```bash
cd backend/src/Nesi.Infrastructure
dotnet ef database update --startup-project ../Nesi.Api
```

**From Azure DevOps Pipeline:**
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Run EF Migrations'
  inputs:
    command: custom
    custom: ef
    arguments: 'database update --startup-project ../Nesi.Api'
    workingDirectory: 'backend/src/Nesi.Infrastructure'
```

---

## Connection String Parameters Explained

| Parameter | Description | Example |
|-----------|-------------|---------|
| `Server` | SQL Server instance | `localhost\\SQLEXPRESS` |
| `Database` | Database name | `NesiDb` |
| `Trusted_Connection` | Use Windows Auth | `true` |
| `User Id` | SQL Auth username | `sa` |
| `Password` | SQL Auth password | `YourPassword` |
| `TrustServerCertificate` | Skip cert validation (dev only) | `true` |
| `MultipleActiveResultSets` | Allow multiple result sets | `true` |
| `Encrypt` | Encrypt connection | `true` (Azure SQL) |
| `Connection Timeout` | Connection timeout (seconds) | `30` |

---

## Environment-Specific Configuration

### Development (appsettings.Development.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NesiDb;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

### Staging (appsettings.Staging.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:nesi-staging.database.windows.net,1433;Initial Catalog=NesiDb-Staging;..."
  }
}
```

### Production (appsettings.Production.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:nesi-prod.database.windows.net,1433;Initial Catalog=NesiDb;..."
  }
}
```

**Security Note:** Never commit production credentials. Use:
- Environment variables
- Azure Key Vault
- User Secrets (for local development)

---

## Troubleshooting

### Issue: Cannot connect to LocalDB

**Solution:**
```bash
# Check LocalDB instances
sqllocaldb info

# Delete and recreate
sqllocaldb delete mssqllocaldb
sqllocaldb create mssqllocaldb
sqllocaldb start mssqllocaldb
```

### Issue: Login failed for user 'sa'

**Solution:**
- Verify password meets SQL Server requirements (uppercase, lowercase, numbers, symbols)
- Check SQL Server authentication mode is set to "Mixed Mode"

### Issue: A network-related or instance-specific error

**Solution:**
- Verify SQL Server is running: `services.msc` (Windows)
- Check firewall allows port 1433
- Verify server name and instance name are correct

### Issue: Azure SQL connection fails

**Solution:**
- Check firewall rules in Azure Portal
- Verify credentials are correct
- Ensure "Allow Azure services" is enabled
- Check connection string format

### Issue: Migrations fail

**Solution:**
```bash
# Clear and rebuild
dotnet clean
dotnet build

# Remove last migration (if needed)
dotnet ef migrations remove --startup-project ../Nesi.Api

# Add new migration
dotnet ef migrations add NewMigration --startup-project ../Nesi.Api

# Update database
dotnet ef database update --startup-project ../Nesi.Api
```

---

## Migration from MySQL to SQL Server

If you're migrating from the old MySQL setup:

1. **Backup MySQL data** (if needed)
2. **Update packages** (already done)
3. **Remove old migrations:**
   ```bash
   rm -rf Data/Migrations/*.cs
   ```
4. **Create new SQL Server migration:**
   ```bash
   dotnet ef migrations add InitialCreateSqlServer --startup-project ../Nesi.Api
   ```
5. **Update database:**
   ```bash
   dotnet ef database update --startup-project ../Nesi.Api
   ```

---

## Best Practices

1. ✅ **Use LocalDB for local development** - Fast and simple
2. ✅ **Use Docker for team consistency** - Same version across team
3. ✅ **Use Azure SQL for production** - Managed, scalable, secure
4. ✅ **Never commit connection strings** with production credentials
5. ✅ **Use migrations** for all schema changes
6. ✅ **Enable connection resiliency** for cloud databases
7. ✅ **Use environment-specific** configuration files
8. ✅ **Implement retry policies** for transient failures

---

## Additional Resources

- [SQL Server Documentation](https://docs.microsoft.com/en-us/sql/sql-server/)
- [Azure SQL Documentation](https://docs.microsoft.com/en-us/azure/azure-sql/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Connection Strings Reference](https://www.connectionstrings.com/sql-server/)

---

**Version:** 1.0.0  
**Last Updated:** April 15, 2026  
**Database Provider:** SQL Server (LocalDB, Express, Docker, Azure SQL)
