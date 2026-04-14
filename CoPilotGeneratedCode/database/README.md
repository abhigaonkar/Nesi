# Database Setup Guide

This folder contains database initialization scripts and documentation for the NESI application.

## Prerequisites

- MySQL Server 8.0 or higher
- .NET 8 SDK (for EF Core migrations)

## Quick Start

### Option 1: Automatic Setup (Recommended for Development)

The application will automatically create and seed the database on first run:

```bash
cd ../backend/src/Nesi.Api
dotnet run
```

The application will:
1. Create the database if it doesn't exist
2. Run all pending migrations
3. Seed initial data (users, customers, work orders, timesheets)

### Option 2: Manual Setup

If you prefer to create the database manually:

1. **Create the database:**
   ```bash
   mysql -u root -p < create-database.sql
   ```

2. **Run migrations:**
   ```bash
   cd ../backend
   dotnet ef database update --project src/Nesi.Infrastructure --startup-project src/Nesi.Api
   ```

3. **Run the application to seed data:**
   ```bash
   cd src/Nesi.Api
   dotnet run
   ```

## Database Schema

The database includes the following tables:

- **Users** - Application users (employees, managers, admins)
- **Customers** - Client organizations
- **WorkOrders** - Projects/tasks for customers
- **Timesheets** - Employee time entries
- **PayTypes** - Pay rate types (Regular, Overtime, Double Time)
- **JobTypes** - Job categories (Development, QA, Design, PM)

## Demo Users

After seeding, the following demo users are available:

| Username   | Password       | Role     | Email                  |
|------------|----------------|----------|------------------------|
| admin      | Admin@123      | Admin    | admin@nesi.com         |
| manager1   | Manager@123    | Manager  | manager1@nesi.com      |
| employee1  | Employee@123   | Employee | employee1@nesi.com     |
| employee2  | Employee@123   | Employee | employee2@nesi.com     |
| employee3  | Employee@123   | Employee | employee3@nesi.com     |

## Connection String

Default connection string (configured in `appsettings.json`):

```
Server=localhost;Database=NesiDb;User=root;Password=root;AllowPublicKeyRetrieval=true;
```

**⚠️ IMPORTANT:** Change the password before deploying to production!

## Migrations Management

### Create a new migration:
```bash
cd backend
dotnet ef migrations add <MigrationName> --project src/Nesi.Infrastructure --startup-project src/Nesi.Api
```

### Apply migrations:
```bash
dotnet ef database update --project src/Nesi.Infrastructure --startup-project src/Nesi.Api
```

### Remove last migration:
```bash
dotnet ef migrations remove --project src/Nesi.Infrastructure --startup-project src/Nesi.Api
```

### Generate SQL script:
```bash
dotnet ef migrations script --project src/Nesi.Infrastructure --startup-project src/Nesi.Api --output migration.sql
```

## Sample Data

The seeder creates:
- 5 users (1 admin, 1 manager, 3 employees)
- 4 customers
- 5 work orders
- ~60-80 timesheet entries (past 30 days, excluding weekends)

Timesheet entries include:
- Mix of draft, submitted, and approved statuses
- Various pay types and job types
- Realistic hour distributions (6-10 hours per day)

## Troubleshooting

### Connection Issues

If you get connection errors:
1. Verify MySQL is running: `mysql --version`
2. Check credentials in `appsettings.json`
3. Ensure MySQL is accessible on localhost:3306

### Migration Issues

If migrations fail:
1. Ensure MySQL user has CREATE/ALTER permissions
2. Check EF Core tools are installed: `dotnet ef --version`
3. Verify the DbContext can be instantiated

### Seeding Issues

If data doesn't appear:
1. Check application logs
2. Verify migrations ran successfully
3. Ensure the seeder isn't being skipped (check for existing data)

## Production Deployment

Before deploying to production:

1. **Update connection string** with production credentials
2. **Change default passwords** for all demo users
3. **Use proper password hashing** (implement ASP.NET Core Identity)
4. **Backup the database** before running migrations
5. **Test migrations** in a staging environment first
6. **Review security settings** on the MySQL server

## Additional Resources

- [EF Core Migrations Documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [MySQL Documentation](https://dev.mysql.com/doc/)
- [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
