using Microsoft.EntityFrameworkCore;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Infrastructure.Data;

/// <summary>
/// Database seeder for populating initial/demo data
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(NesiDbContext context)
    {
        // Ensure database is created
        await context.Database.MigrateAsync();

        // Check if data already exists
        if (await context.Users.AnyAsync())
        {
            return; // Database already seeded
        }

        // Seed data (will be inserted in order due to foreign key relationships)
        await SeedUsersAsync(context);
        await SeedCustomersAsync(context);
        await SeedWorkOrdersAsync(context);
        await SeedTimesheetsAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(NesiDbContext context)
    {
        // Note: In production, use proper password hashing (e.g., BCrypt, ASP.NET Core Identity)
        // For demo purposes, using simple hashed passwords
        var users = new List<User>
        {
            new User("admin", "admin@nesi.com", "Admin", "User", 
                HashPassword("Admin@123"), UserRole.Admin),
            
            new User("manager1", "manager1@nesi.com", "John", "Manager", 
                HashPassword("Manager@123"), UserRole.Manager),
            
            new User("employee1", "employee1@nesi.com", "Jane", "Smith", 
                HashPassword("Employee@123"), UserRole.Employee),
            
            new User("employee2", "employee2@nesi.com", "Bob", "Johnson", 
                HashPassword("Employee@123"), UserRole.Employee),
            
            new User("employee3", "employee3@nesi.com", "Alice", "Williams", 
                HashPassword("Employee@123"), UserRole.Employee)
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync(); // Save to get IDs for relationships
    }

    private static async Task SeedCustomersAsync(NesiDbContext context)
    {
        var customers = new List<Customer>
        {
            new Customer("Acme Corporation", "John Doe", "john.doe@acme.com", "555-0101", "123 Main St, City, State"),
            new Customer("TechStart Inc.", "Jane Smith", "jane.smith@techstart.com", "555-0102", "456 Tech Ave, City, State"),
            new Customer("Global Industries", "Bob Wilson", "bob.wilson@global.com", "555-0103", "789 Industry Blvd, City, State"),
            new Customer("Local Services LLC", "Alice Brown", "alice.brown@localservices.com", "555-0104", "321 Service Rd, City, State")
        };

        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();
    }

    private static async Task SeedWorkOrdersAsync(NesiDbContext context)
    {
        var customers = await context.Customers.ToListAsync();
        
        var workOrders = new List<WorkOrder>
        {
            new WorkOrder("WO-2024-001", customers[0].Id, "Website Redesign Project", new DateTime(2024, 1, 1)),
            new WorkOrder("WO-2024-002", customers[0].Id, "Mobile App Development", new DateTime(2024, 2, 1)),
            new WorkOrder("WO-2024-003", customers[1].Id, "Database Migration", new DateTime(2024, 1, 15)),
            new WorkOrder("WO-2024-004", customers[2].Id, "System Integration", new DateTime(2024, 3, 1)),
            new WorkOrder("WO-2024-005", customers[3].Id, "Maintenance & Support", new DateTime(2024, 1, 1))
        };

        await context.WorkOrders.AddRangeAsync(workOrders);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTimesheetsAsync(NesiDbContext context)
    {
        var users = await context.Users.Where(u => u.Role == UserRole.Employee).ToListAsync();
        var workOrders = await context.WorkOrders.ToListAsync();
        var payTypes = await context.PayTypes.ToListAsync();
        var jobTypes = await context.JobTypes.ToListAsync();

        var timesheets = new List<TimesheetEntry>();
        var random = new Random(42); // Fixed seed for reproducibility

        // Create timesheets for the past 30 days
        var startDate = DateTime.Today.AddDays(-30);
        
        foreach (var user in users)
        {
            for (int i = 0; i < 30; i++)
            {
                var date = startDate.AddDays(i);
                
                // Skip weekends
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                // Randomly assign 70% of workdays
                if (random.Next(100) < 70)
                {
                    var hours = (decimal)(6 + random.NextDouble() * 4); // 6-10 hours
                    var workOrder = workOrders[random.Next(workOrders.Count)];
                    var payType = payTypes[random.Next(Math.Min(2, payTypes.Count))]; // Mostly regular or OT
                    var jobType = jobTypes[random.Next(jobTypes.Count)];

                    var timesheet = new TimesheetEntry(
                        user.Id,
                        date,
                        Math.Round(hours, 2),
                        payType.Id,
                        workOrder.Id,
                        jobType.Id,
                        $"Work on {workOrder.Description}"
                    );

                    // Submit recent timesheets (last 7 days)
                    if (date >= DateTime.Today.AddDays(-7))
                    {
                        timesheet.Submit();
                        
                        // Approve some of them (60%)
                        if (random.Next(100) < 60)
                        {
                            timesheet.Approve();
                        }
                    }
                    else
                    {
                        // Approve older timesheets (90%)
                        timesheet.Submit();
                        if (random.Next(100) < 90)
                        {
                            timesheet.Approve();
                        }
                    }

                    timesheets.Add(timesheet);
                }
            }
        }

        await context.Timesheets.AddRangeAsync(timesheets);
        await context.SaveChangesAsync();
    }

    // Simple password hashing for demo purposes
    // In production, use ASP.NET Core Identity or BCrypt
    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
