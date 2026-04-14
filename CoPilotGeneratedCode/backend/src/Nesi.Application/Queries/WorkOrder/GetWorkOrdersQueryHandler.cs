using MediatR;
using Nesi.Application.DTOs.WorkOrder;
using Nesi.Domain.Interfaces;
using Nesi.Domain.Entities;

namespace Nesi.Application.Queries.WorkOrder;

public class GetWorkOrdersQueryHandler : IRequestHandler<GetWorkOrdersQuery, List<WorkOrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWorkOrdersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<WorkOrderDto>> Handle(GetWorkOrdersQuery request, CancellationToken cancellationToken)
    {
        var workOrders = await _unitOfWork.Repository<Domain.Entities.WorkOrder>().GetAllAsync();
        var customers = await _unitOfWork.Repository<Customer>().GetAllAsync();

        return workOrders
            .Where(w => w.IsActive)
            .OrderBy(w => w.WorkOrderNumber)
            .Select(w => new WorkOrderDto
            {
                Id = w.Id,
                WorkOrderNumber = w.WorkOrderNumber,
                CustomerId = w.CustomerId,
                CustomerName = customers.FirstOrDefault(c => c.Id == w.CustomerId)?.Name ?? "Unknown",
                Description = w.Description,
                StartDate = w.StartDate,
                EndDate = w.EndDate,
                IsActive = w.IsActive,
                CreatedAt = w.CreatedAt
            })
            .ToList();
    }
}
