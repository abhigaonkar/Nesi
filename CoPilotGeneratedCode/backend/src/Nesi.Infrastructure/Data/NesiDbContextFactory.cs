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
        // Default to LocalDB for development
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=NesiDb;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true";
        
        optionsBuilder.UseSqlServer(connectionString);

        return new NesiDbContext(optionsBuilder.Options);
    }
}
