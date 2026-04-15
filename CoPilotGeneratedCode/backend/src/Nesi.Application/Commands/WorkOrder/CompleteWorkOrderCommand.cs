using MediatR;

namespace Nesi.Application.Commands.WorkOrder;

public record CompleteWorkOrderCommand(
    int WorkOrderId,
    int CompletedBy,
    string? CompletionNotes) : IRequest<Unit>;
