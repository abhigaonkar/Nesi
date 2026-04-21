using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.PurchaseOrder;

public class ConfirmReceiptCommandHandler : IRequestHandler<ConfirmReceiptCommand, bool>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmReceiptCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ConfirmReceiptCommand request, CancellationToken cancellationToken)
    {
        // Find the receipt
        var allPurchaseOrders = await _purchaseOrderRepository.GetAllAsync(cancellationToken);
        
        Domain.Entities.PurchaseOrderReceipt? receipt = null;
        foreach (var po in allPurchaseOrders)
        {
            var poWithDetails = await _purchaseOrderRepository.GetWithDetailsAsync(po.Id, cancellationToken);
            receipt = poWithDetails?.Receipts?.FirstOrDefault(r => r.Id == request.ReceiptId);
            if (receipt != null)
                break;
        }

        if (receipt == null)
            return false;

        // Confirm the receipt
        receipt.Confirm();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
