using MediatR;

namespace Nesi.Application.Commands.Timesheet;

public record ApproveTimesheetCommand(int Id) : IRequest<bool>;
