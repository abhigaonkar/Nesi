using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Domain.Interfaces;

/// <summary>
/// Repository interface for WorkOrder entity with specific query methods
/// </summary>
public interface IWorkOrderRepository : IRepository<WorkOrder>
{
    Task<IEnumerable<WorkOrder>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken cancellationToken = default);
    Task<WorkOrder?> GetByWorkOrderNumberAsync(string workOrderNumber, CancellationToken cancellationToken = default);
    Task<WorkOrder?> GetByQuoteIdAsync(int quoteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkOrder>> GetByProjectManagerIdAsync(int projectManagerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkOrder>> GetActiveWorkOrdersAsync(CancellationToken cancellationToken = default);
    Task<string> GenerateWorkOrderNumberAsync(CancellationToken cancellationToken = default);
}
