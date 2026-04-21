using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.PurchaseOrder;

public class ApprovePurchaseOrderCommandHandler : IRequestHandler<ApprovePurchaseOrderCommand, bool>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApprovePurchaseOrderCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ApprovePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(
            request.PurchaseOrderId, 
            cancellationToken);
        
        if (purchaseOrder == null)
            return false;

        // Approve the PO
        purchaseOrder.Approve(request.ApprovedBy);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
