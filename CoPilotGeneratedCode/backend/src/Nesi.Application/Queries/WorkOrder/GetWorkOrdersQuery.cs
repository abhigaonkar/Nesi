using MediatR;
using Nesi.Application.DTOs.WorkOrder;

namespace Nesi.Application.Queries.WorkOrder;

public record GetWorkOrdersQuery : IRequest<List<WorkOrderDto>>;
