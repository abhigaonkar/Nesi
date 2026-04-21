using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.WorkOrder;

public class UpdateProgressCommandHandler : IRequestHandler<UpdateProgressCommand, bool>
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProgressCommandHandler(
        IWorkOrderRepository workOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateProgressCommand request, CancellationToken cancellationToken)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(request.WorkOrderId, cancellationToken);
        
        if (workOrder == null)
            return false;

        // Update milestones
        workOrder.UpdateMilestones(request.Milestones);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
