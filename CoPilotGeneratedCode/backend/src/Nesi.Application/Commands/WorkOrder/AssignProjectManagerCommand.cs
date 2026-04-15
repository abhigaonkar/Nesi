using MediatR;

namespace Nesi.Application.Commands.WorkOrder;

public record AssignProjectManagerCommand(
    int WorkOrderId,
    int ProjectManagerId,
    DateTime? ScheduledStartDate,
    DateTime? ScheduledEndDate) : IRequest<Unit>;
