using MediatR;

namespace Nesi.Application.Commands.Timesheet;

public record SubmitTimesheetCommand(int Id, int UserId) : IRequest<bool>;
