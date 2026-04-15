using Microsoft.EntityFrameworkCore;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;
using Nesi.Domain.Interfaces;
using Nesi.Infrastructure.Data;

namespace Nesi.Infrastructure.Repositories;

public class WorkOrderRepository : Repository<WorkOrder>, IWorkOrderRepository
{
    public WorkOrderRepository(NesiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<WorkOrder>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(wo => wo.CustomerId == customerId && !wo.IsDeleted)
            .Include(wo => wo.Customer)
            .Include(wo => wo.ProjectManager)
            .OrderByDescending(wo => wo.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(wo => wo.Status == status && !wo.IsDeleted)
            .Include(wo => wo.Customer)
            .Include(wo => wo.ProjectManager)
            .OrderByDescending(wo => wo.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkOrder?> GetByWorkOrderNumberAsync(string workOrderNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(wo => wo.Customer)
            .Include(wo => wo.ProjectManager)
            .Include(wo => wo.Quote)
            .FirstOrDefaultAsync(wo => wo.WorkOrderNumber == workOrderNumber && !wo.IsDeleted, cancellationToken);
    }

    public async Task<WorkOrder?> GetByQuoteIdAsync(int quoteId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(wo => wo.Customer)
            .Include(wo => wo.ProjectManager)
            .Include(wo => wo.Quote)
            .FirstOrDefaultAsync(wo => wo.QuoteId == quoteId && !wo.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<WorkOrder>> GetByProjectManagerIdAsync(int projectManagerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(wo => wo.ProjectManagerId == projectManagerId && !wo.IsDeleted)
            .Include(wo => wo.Customer)
            .Include(wo => wo.ProjectManager)
            .OrderByDescending(wo => wo.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkOrder>> GetActiveWorkOrdersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(wo => wo.IsActive && !wo.IsDeleted && 
                  wo.Status != WorkOrderStatus.Complete && 
                  wo.Status != WorkOrderStatus.Cancelled && 
                  wo.Status != WorkOrderStatus.Closed)
            .Include(wo => wo.Customer)
            .Include(wo => wo.ProjectManager)
            .OrderByDescending(wo => wo.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<string> GenerateWorkOrderNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"WO-{year}-";
        
        var lastWorkOrder = await _dbSet
            .Where(wo => wo.WorkOrderNumber.StartsWith(prefix))
            .OrderByDescending(wo => wo.WorkOrderNumber)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (lastWorkOrder == null)
        {
            return $"{prefix}0001";
        }
        
        var lastNumber = lastWorkOrder.WorkOrderNumber.Split('-').Last();
        if (int.TryParse(lastNumber, out var number))
        {
            return $"{prefix}{(number + 1):D4}";
        }
        
        return $"{prefix}0001";
    }
}
