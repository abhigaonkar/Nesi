using Microsoft.EntityFrameworkCore;
using Nesi.Domain.Entities;

namespace Nesi.Application.Common;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<WorkOrder> WorkOrders { get; }
    DbSet<Quote> Quotes { get; }
    DbSet<TimesheetEntry> TimesheetEntries { get; }
    DbSet<Material> Materials { get; }
    DbSet<PurchaseOrder> PurchaseOrders { get; }
    DbSet<Vendor> Vendors { get; }
    DbSet<User> Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
