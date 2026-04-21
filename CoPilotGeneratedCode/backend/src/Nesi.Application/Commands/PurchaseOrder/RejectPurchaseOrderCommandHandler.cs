using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.PurchaseOrder;

public class RejectPurchaseOrderCommandHandler : IRequestHandler<RejectPurchaseOrderCommand, bool>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectPurchaseOrderCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RejectPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(
            request.PurchaseOrderId, 
            cancellationToken);
        
        if (purchaseOrder == null)
            return false;

        // Reject the PO
        purchaseOrder.Reject(request.RejectedBy, request.Reason);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
