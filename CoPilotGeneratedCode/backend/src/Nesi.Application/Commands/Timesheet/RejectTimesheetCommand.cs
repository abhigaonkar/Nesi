using MediatR;

namespace Nesi.Application.Commands.Timesheet;

public record RejectTimesheetCommand(int Id) : IRequest<bool>;
