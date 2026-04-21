using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.PurchaseOrder;

public class SubmitPurchaseOrderCommandHandler : IRequestHandler<SubmitPurchaseOrderCommand, bool>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitPurchaseOrderCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SubmitPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetWithDetailsAsync(
            request.PurchaseOrderId, 
            cancellationToken);
        
        if (purchaseOrder == null)
            return false;

        // Submit the PO (this validates it has line items)
        purchaseOrder.Submit();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
