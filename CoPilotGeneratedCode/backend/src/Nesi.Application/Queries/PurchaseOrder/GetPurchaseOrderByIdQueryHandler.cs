using MediatR;
using Nesi.Application.DTOs.PurchaseOrder;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.PurchaseOrder;

public class GetPurchaseOrderByIdQueryHandler : IRequestHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDto?>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;

    public GetPurchaseOrderByIdQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<PurchaseOrderDto?> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var purchaseOrder = request.IncludeLineItems || request.IncludeReceipts
            ? await _purchaseOrderRepository.GetWithDetailsAsync(request.Id, cancellationToken)
            : await _purchaseOrderRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (purchaseOrder == null)
            return null;

        var dto = new PurchaseOrderDto
        {
            Id = purchaseOrder.Id,
            PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
            VendorId = purchaseOrder.VendorId,
            VendorName = purchaseOrder.Vendor?.CompanyName ?? "",
            WorkOrderId = purchaseOrder.WorkOrderId,
            WorkOrderNumber = purchaseOrder.WorkOrder?.WorkOrderNumber,
            RequestedBy = purchaseOrder.RequestedBy,
            RequesterName = "", // TODO: Load from user repository
            ApprovedBy = purchaseOrder.ApprovedBy,
            ApproverName = purchaseOrder.ApprovedBy.HasValue ? "" : null, // TODO: Load from user repository
            OrderDate = purchaseOrder.OrderDate,
            RequiredByDate = purchaseOrder.RequiredByDate,
            ApprovedAt = purchaseOrder.ApprovedAt,
            Status = purchaseOrder.Status.ToString(),
            Description = purchaseOrder.Description,
            Notes = purchaseOrder.Notes,
            SubTotal = purchaseOrder.SubTotal,
            TaxAmount = purchaseOrder.TaxAmount,
            ShippingCost = purchaseOrder.ShippingCost,
            TotalAmount = purchaseOrder.TotalAmount,
            ShippingAddress = purchaseOrder.ShippingAddress,
            ShippingCity = purchaseOrder.ShippingCity,
            ShippingState = purchaseOrder.ShippingState,
            ShippingZipCode = purchaseOrder.ShippingZipCode,
            IsActive = purchaseOrder.IsActive,
            CreatedAt = purchaseOrder.CreatedAt,
            UpdatedAt = purchaseOrder.UpdatedAt
        };

        // Include line items if requested
        if (request.IncludeLineItems && purchaseOrder.LineItems != null)
        {
            dto.LineItems = purchaseOrder.LineItems.Select(li => new PurchaseOrderLineItemDto
            {
                Id = li.Id,
                PurchaseOrderId = li.PurchaseOrderId,
                LineNumber = li.LineNumber,
                Description = li.Description,
                PartNumber = li.PartNumber,
                Quantity = li.Quantity,
                UnitOfMeasure = li.UnitOfMeasure,
                UnitPrice = li.UnitPrice,
                TotalPrice = li.TotalPrice,
                QuantityReceived = li.QuantityReceived,
                RemainingQuantity = li.RemainingQuantity(),
                IsFullyReceived = li.IsFullyReceived(),
                Notes = li.Notes,
                CreatedAt = li.CreatedAt
            }).ToList();
        }

        // Include receipts if requested
        if (request.IncludeReceipts && purchaseOrder.Receipts != null)
        {
            dto.Receipts = purchaseOrder.Receipts.Select(r => new PurchaseOrderReceiptDto
            {
                Id = r.Id,
                PurchaseOrderId = r.PurchaseOrderId,
                ReceiptNumber = r.ReceiptNumber,
                ReceivedDate = r.ReceivedDate,
                ReceivedBy = r.ReceivedBy,
                ReceiverName = "", // TODO: Load from user repository
                Status = r.Status.ToString(),
                Notes = r.Notes,
                PackingSlipNumber = r.PackingSlipNumber,
                CreatedAt = r.CreatedAt,
                ReceiptItems = r.ReceiptItems?.Select(ri => new PurchaseOrderReceiptItemDto
                {
                    Id = ri.Id,
                    PurchaseOrderReceiptId = ri.PurchaseOrderReceiptId,
                    PurchaseOrderLineItemId = ri.PurchaseOrderLineItemId,
                    ItemDescription = "", // Would need to load from line item
                    QuantityReceived = ri.QuantityReceived,
                    Condition = ri.Condition,
                    Notes = ri.Notes,
                    HasDiscrepancy = ri.HasDiscrepancy,
                    DiscrepancyReason = ri.DiscrepancyReason
                }).ToList()
            }).ToList();
        }

        return dto;
    }
}
