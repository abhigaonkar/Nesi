using MediatR;
using Nesi.Application.DTOs.Timesheet;
using Nesi.Application.Common;

namespace Nesi.Application.Queries.Timesheet;

public record GetTimesheetsQuery(
    int? UserId,
    DateTime? StartDate,
    DateTime? EndDate,
    string? Status,
    int PageNumber,
    int PageSize) : IRequest<PagedResult<TimesheetDto>>;
