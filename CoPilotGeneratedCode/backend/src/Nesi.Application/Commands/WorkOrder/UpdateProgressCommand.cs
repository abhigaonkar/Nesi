using MediatR;

namespace Nesi.Application.Commands.WorkOrder;

public record UpdateProgressCommand(
    int WorkOrderId,
    string Milestones,
    int? PercentComplete) : IRequest<bool>;
