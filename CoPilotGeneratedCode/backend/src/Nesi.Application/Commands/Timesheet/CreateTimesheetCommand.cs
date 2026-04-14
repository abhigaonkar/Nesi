using MediatR;

namespace Nesi.Application.Commands.Timesheet;

public record CreateTimesheetCommand(
    int UserId,
    DateTime Date,
    decimal Hours,
    int PayTypeId,
    int? WorkOrderId,
    int? JobTypeId,
    string? Notes) : IRequest<int>;
