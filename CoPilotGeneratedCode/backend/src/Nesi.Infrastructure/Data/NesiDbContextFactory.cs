using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nesi.Infrastructure.Data;

/// <summary>
/// Design-time factory for creating DbContext instances during migrations
/// </summary>
public class NesiDbContextFactory : IDesignTimeDbContextFactory<NesiDbContext>
{
    public NesiDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NesiDbContext>();
        
        // Use a connection string for design-time (migrations)
        // This won't actually connect, just used for generating migration scripts
        var connectionString = "Server=localhost;Database=NesiDb;User=root;Password=root;";
        
        optionsBuilder.UseMySql(
            connectionString,
            new MySqlServerVersion(new Version(8, 0, 21)));

        return new NesiDbContext(optionsBuilder.Options);
    }
}
