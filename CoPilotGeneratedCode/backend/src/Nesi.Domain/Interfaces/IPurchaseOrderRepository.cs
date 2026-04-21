using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Domain.Interfaces;

/// <summary>
/// Repository interface for PurchaseOrder entity with specific query methods
/// </summary>
public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
{
    Task<PurchaseOrder?> GetByPurchaseOrderNumberAsync(string poNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetByVendorIdAsync(int vendorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetByWorkOrderIdAsync(int workOrderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(PurchaseOrderStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetByRequesterIdAsync(int requesterId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetPendingApprovalsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetActiveOrdersAsync(CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<string> GeneratePurchaseOrderNumberAsync(CancellationToken cancellationToken = default);
}
