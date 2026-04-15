using MediatR;
using Nesi.Application.DTOs.WorkOrder;

namespace Nesi.Application.Queries.WorkOrder;

public record GetWorkOrderByIdQuery(int Id) : IRequest<WorkOrderDto?>;
