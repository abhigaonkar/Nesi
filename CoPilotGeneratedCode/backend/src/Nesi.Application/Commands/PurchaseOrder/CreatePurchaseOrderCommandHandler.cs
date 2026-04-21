using MediatR;
using Nesi.Domain.Entities;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.PurchaseOrder;

public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, int>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePurchaseOrderCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        // Generate PO number
        var poNumber = await _purchaseOrderRepository.GeneratePurchaseOrderNumberAsync(cancellationToken);

        // TODO: Get current user ID from authentication context
        // For now using a placeholder
        int currentUserId = 1; // This should come from HttpContext

        // Create PO entity
        var purchaseOrder = new Domain.Entities.PurchaseOrder(
            poNumber,
            request.VendorId,
            currentUserId,
            request.OrderDate,
            request.Description,
            request.WorkOrderId);

        // Set required by date if provided
        if (request.RequiredByDate.HasValue)
        {
            purchaseOrder.UpdateRequiredByDate(request.RequiredByDate.Value);
        }

        // Update shipping address if provided
        if (!string.IsNullOrWhiteSpace(request.ShippingAddress))
        {
            purchaseOrder.UpdateShippingAddress(
                request.ShippingAddress,
                request.ShippingCity ?? string.Empty,
                request.ShippingState ?? string.Empty,
                request.ShippingZipCode ?? string.Empty);
        }

        // Add to repository (line items will be added separately after PO is created)
        await _purchaseOrderRepository.AddAsync(purchaseOrder, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Now add line items
        decimal subTotal = 0;
        foreach (var lineItemInput in request.LineItems)
        {
            var lineItem = new PurchaseOrderLineItem(
                purchaseOrder.Id,
                lineItemInput.LineNumber,
                lineItemInput.Description,
                lineItemInput.Quantity,
                lineItemInput.UnitPrice,
                lineItemInput.PartNumber,
                lineItemInput.UnitOfMeasure);
            
            subTotal += lineItem.TotalPrice;
        }

        // Update financial totals (assuming no tax/shipping for now)
        purchaseOrder.UpdateFinancials(subTotal, 0, 0);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return purchaseOrder.Id;
    }
}
