using MediatR;
using Nesi.Application.DTOs.PurchaseOrder;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.PurchaseOrder;

public class GetReceiptsByPurchaseOrderQueryHandler : IRequestHandler<GetReceiptsByPurchaseOrderQuery, List<PurchaseOrderReceiptDto>>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;

    public GetReceiptsByPurchaseOrderQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<List<PurchaseOrderReceiptDto>> Handle(GetReceiptsByPurchaseOrderQuery request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetWithDetailsAsync(request.PurchaseOrderId, cancellationToken);
        
        if (purchaseOrder?.Receipts == null)
            return new List<PurchaseOrderReceiptDto>();

        return purchaseOrder.Receipts.Select(r => new PurchaseOrderReceiptDto
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
                ItemDescription = "", // Would need to join with line items
                QuantityReceived = ri.QuantityReceived,
                Condition = ri.Condition,
                Notes = ri.Notes,
                HasDiscrepancy = ri.HasDiscrepancy,
                DiscrepancyReason = ri.DiscrepancyReason
            }).ToList()
        }).ToList();
    }
}
