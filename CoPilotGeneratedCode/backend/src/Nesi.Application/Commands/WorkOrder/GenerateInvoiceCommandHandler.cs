using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.WorkOrder;

public class GenerateInvoiceCommandHandler : IRequestHandler<GenerateInvoiceCommand, Unit>
{
    private readonly IRepository<Domain.Entities.WorkOrder> _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateInvoiceCommandHandler(
        IRepository<Domain.Entities.WorkOrder> workOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(GenerateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(request.WorkOrderId, cancellationToken);
        if (workOrder == null)
            throw new InvalidOperationException($"Work order with ID {request.WorkOrderId} not found");

        workOrder.GenerateInvoice(request.InvoiceAmount);

        _workOrderRepository.Update(workOrder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
