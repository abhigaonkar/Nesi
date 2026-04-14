using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nesi.Infrastructure.Data;

namespace Nesi.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    /// <summary>
    /// Migrates the database and seeds initial data
    /// </summary>
    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<NesiDbContext>();
            
            // Run migrations
            await context.Database.MigrateAsync();
            
            // Seed data
            await DatabaseSeeder.SeedAsync(context);
        }
        catch (Exception ex)
        {
            // Log the error (in production, use proper logging)
            Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
            throw;
        }

        return host;
    }
}
