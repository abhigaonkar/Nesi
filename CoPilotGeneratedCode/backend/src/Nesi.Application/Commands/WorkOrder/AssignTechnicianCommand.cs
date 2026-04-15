using MediatR;

namespace Nesi.Application.Commands.WorkOrder;

public record AssignTechnicianCommand(
    int WorkOrderId,
    int TechnicianId,
    int AssignedBy,
    string? Role,
    string? Notes) : IRequest<int>;
