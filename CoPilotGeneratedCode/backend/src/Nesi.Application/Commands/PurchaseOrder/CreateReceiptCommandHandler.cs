using MediatR;
using Nesi.Domain.Entities;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.PurchaseOrder;

public class CreateReceiptCommandHandler : IRequestHandler<CreateReceiptCommand, int>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReceiptCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
    {
        // Get PO with line items
        var purchaseOrder = await _purchaseOrderRepository.GetWithDetailsAsync(
            request.PurchaseOrderId, 
            cancellationToken);
        
        if (purchaseOrder == null)
            throw new InvalidOperationException($"Purchase order {request.PurchaseOrderId} not found");

        // TODO: Get current user ID from authentication context
        int currentUserId = 1;

        // Generate receipt number
        var receiptNumber = await GenerateReceiptNumberAsync(request.PurchaseOrderId, cancellationToken);

        // Create receipt
        var receipt = new PurchaseOrderReceipt(
            request.PurchaseOrderId,
            receiptNumber,
            request.ReceivedDate,
            currentUserId,
            request.PackingSlipNumber,
            request.Notes);

        // Add receipt items and update line item quantities
        foreach (var itemInput in request.Items)
        {
            var lineItem = purchaseOrder.LineItems?
                .FirstOrDefault(li => li.Id == itemInput.PurchaseOrderLineItemId);
            
            if (lineItem == null)
                throw new InvalidOperationException($"Line item {itemInput.PurchaseOrderLineItemId} not found");

            // Create receipt item
            var receiptItem = new PurchaseOrderReceiptItem(
                receipt.Id,
                itemInput.PurchaseOrderLineItemId,
                itemInput.QuantityReceived,
                itemInput.Condition,
                itemInput.Notes,
                itemInput.HasDiscrepancy,
                itemInput.DiscrepancyReason);

            // Update line item received quantity
            lineItem.RecordReceivedQuantity(itemInput.QuantityReceived);
        }

        // Check if PO is fully received and update status
        if (purchaseOrder.LineItems?.All(li => li.IsFullyReceived) == true)
        {
            purchaseOrder.MarkAsReceived();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return receipt.Id;
    }

    private async Task<string> GenerateReceiptNumberAsync(int purchaseOrderId, CancellationToken cancellationToken)
    {
        // Get count of existing receipts for this PO
        var purchaseOrder = await _purchaseOrderRepository.GetWithDetailsAsync(purchaseOrderId, cancellationToken);
        var receiptCount = purchaseOrder?.Receipts?.Count ?? 0;
        
        // Generate receipt number: PO number + receipt sequence
        return $"{purchaseOrder?.PurchaseOrderNumber}-R{(receiptCount + 1):D3}";
    }
}
