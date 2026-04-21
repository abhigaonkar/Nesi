using Microsoft.EntityFrameworkCore;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;
using Nesi.Domain.Interfaces;
using Nesi.Infrastructure.Data;

namespace Nesi.Infrastructure.Repositories;

public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
{
    public PurchaseOrderRepository(NesiDbContext context) : base(context)
    {
    }

    public async Task<PurchaseOrder?> GetByPurchaseOrderNumberAsync(string poNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(po => po.PurchaseOrderNumber == poNumber && !po.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetByVendorIdAsync(int vendorId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(po => po.Vendor)
            .Include(po => po.LineItems)
            .Where(po => po.VendorId == vendorId && !po.IsDeleted)
            .OrderByDescending(po => po.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetByWorkOrderIdAsync(int workOrderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(po => po.Vendor)
            .Include(po => po.LineItems)
            .Where(po => po.WorkOrderId == workOrderId && !po.IsDeleted)
            .OrderByDescending(po => po.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(PurchaseOrderStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(po => po.Vendor)
            .Where(po => po.Status == status && !po.IsDeleted)
            .OrderByDescending(po => po.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetByRequesterIdAsync(int requesterId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(po => po.Vendor)
            .Include(po => po.LineItems)
            .Where(po => po.RequestedBy == requesterId && !po.IsDeleted)
            .OrderByDescending(po => po.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetPendingApprovalsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(po => po.Vendor)
            .Include(po => po.Requester)
            .Include(po => po.LineItems)
            .Where(po => po.Status == PurchaseOrderStatus.Submitted && !po.IsDeleted)
            .OrderBy(po => po.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetActiveOrdersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(po => po.Vendor)
            .Include(po => po.LineItems)
            .Where(po => po.IsActive && !po.IsDeleted)
            .OrderByDescending(po => po.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<PurchaseOrder?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(po => po.Vendor)
            .Include(po => po.WorkOrder)
            .Include(po => po.Requester)
            .Include(po => po.Approver)
            .Include(po => po.LineItems)
            .Include(po => po.Receipts)
                .ThenInclude(r => r.ReceiptItems)
            .FirstOrDefaultAsync(po => po.Id == id && !po.IsDeleted, cancellationToken);
    }

    public async Task<string> GeneratePurchaseOrderNumberAsync(CancellationToken cancellationToken = default)
    {
        var currentYear = DateTime.UtcNow.Year;
        var prefix = $"PO-{currentYear}-";

        var lastPO = await _dbSet
            .Where(po => po.PurchaseOrderNumber.StartsWith(prefix))
            .OrderByDescending(po => po.PurchaseOrderNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastPO == null)
        {
            return $"{prefix}00001";
        }

        var lastNumber = int.Parse(lastPO.PurchaseOrderNumber.Substring(prefix.Length));
        var nextNumber = lastNumber + 1;
        return $"{prefix}{nextNumber:D5}";
    }
}
