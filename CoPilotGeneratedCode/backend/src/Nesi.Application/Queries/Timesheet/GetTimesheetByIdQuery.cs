using MediatR;
using Nesi.Application.DTOs.Timesheet;

namespace Nesi.Application.Queries.Timesheet;

public record GetTimesheetByIdQuery(int Id) : IRequest<TimesheetDto?>;
