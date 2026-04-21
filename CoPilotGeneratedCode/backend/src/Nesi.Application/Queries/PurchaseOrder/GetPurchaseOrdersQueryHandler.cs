using MediatR;
using Nesi.Application.DTOs.PurchaseOrder;
using Nesi.Domain.Enums;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.PurchaseOrder;

public class GetPurchaseOrdersQueryHandler : IRequestHandler<GetPurchaseOrdersQuery, GetPurchaseOrdersQueryResult>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;

    public GetPurchaseOrdersQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<GetPurchaseOrdersQueryResult> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.PurchaseOrder> purchaseOrders;

        // Apply filters
        if (request.VendorId.HasValue)
        {
            purchaseOrders = await _purchaseOrderRepository.GetByVendorIdAsync(request.VendorId.Value, cancellationToken);
        }
        else if (request.WorkOrderId.HasValue)
        {
            purchaseOrders = await _purchaseOrderRepository.GetByWorkOrderIdAsync(request.WorkOrderId.Value, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<PurchaseOrderStatus>(request.Status, true, out var status))
        {
            purchaseOrders = await _purchaseOrderRepository.GetByStatusAsync(status, cancellationToken);
        }
        else
        {
            purchaseOrders = await _purchaseOrderRepository.GetAllAsync(cancellationToken);
        }

        var purchaseOrderList = purchaseOrders.ToList();
        
        // Apply pagination
        var paginatedPOs = purchaseOrderList
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(po => new PurchaseOrderDto
            {
                Id = po.Id,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                VendorId = po.VendorId,
                VendorName = po.Vendor?.CompanyName ?? "",
                WorkOrderId = po.WorkOrderId,
                WorkOrderNumber = po.WorkOrder?.WorkOrderNumber,
                RequestedBy = po.RequestedBy,
                RequesterName = "", // TODO: Load from user repository
                ApprovedBy = po.ApprovedBy,
                ApproverName = po.ApprovedBy.HasValue ? "" : null, // TODO: Load from user repository
                OrderDate = po.OrderDate,
                RequiredByDate = po.RequiredByDate,
                ApprovedAt = po.ApprovedAt,
                Status = po.Status.ToString(),
                Description = po.Description,
                Notes = po.Notes,
                SubTotal = po.SubTotal,
                TaxAmount = po.TaxAmount,
                ShippingCost = po.ShippingCost,
                TotalAmount = po.TotalAmount,
                ShippingAddress = po.ShippingAddress,
                ShippingCity = po.ShippingCity,
                ShippingState = po.ShippingState,
                ShippingZipCode = po.ShippingZipCode,
                IsActive = po.IsActive,
                CreatedAt = po.CreatedAt,
                UpdatedAt = po.UpdatedAt
            })
            .ToList();

        return new GetPurchaseOrdersQueryResult
        {
            PurchaseOrders = paginatedPOs,
            TotalCount = purchaseOrderList.Count,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
