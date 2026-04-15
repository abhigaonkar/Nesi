using MediatR;
using Nesi.Application.DTOs.WorkOrder;
using Nesi.Domain.Interfaces;
using Nesi.Domain.Entities;

namespace Nesi.Application.Queries.WorkOrder;

public class GetWorkOrderByIdQueryHandler : IRequestHandler<GetWorkOrderByIdQuery, WorkOrderDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWorkOrderByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<WorkOrderDto?> Handle(GetWorkOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var workOrder = await _unitOfWork.Repository<Domain.Entities.WorkOrder>()
            .GetByIdAsync(request.Id);

        if (workOrder == null)
        {
            return null;
        }

        var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(workOrder.CustomerId);

        return new WorkOrderDto
        {
            Id = workOrder.Id,
            WorkOrderNumber = workOrder.WorkOrderNumber,
            CustomerId = workOrder.CustomerId,
            CustomerName = customer?.Name ?? "Unknown",
            Description = workOrder.Description,
            StartDate = workOrder.StartDate,
            EndDate = workOrder.EndDate,
            IsActive = workOrder.IsActive,
            CreatedAt = workOrder.CreatedAt
        };
    }
}
