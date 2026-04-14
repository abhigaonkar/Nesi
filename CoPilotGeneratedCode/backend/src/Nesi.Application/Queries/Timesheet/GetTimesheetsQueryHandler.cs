using MediatR;
using Nesi.Application.DTOs.Timesheet;
using Nesi.Application.Common;
using Nesi.Domain.Interfaces;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Application.Queries.Timesheet;

public class GetTimesheetsQueryHandler : IRequestHandler<GetTimesheetsQuery, PagedResult<TimesheetDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTimesheetsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<TimesheetDto>> Handle(GetTimesheetsQuery request, CancellationToken cancellationToken)
    {
        var timesheets = await _unitOfWork.Repository<TimesheetEntry>().GetAllAsync();
        var users = await _unitOfWork.Repository<User>().GetAllAsync();
        var payTypes = await _unitOfWork.Repository<PayType>().GetAllAsync();
        var jobTypes = await _unitOfWork.Repository<JobType>().GetAllAsync();
        var workOrders = await _unitOfWork.Repository<Domain.Entities.WorkOrder>().GetAllAsync();

        // Apply filters
        var query = timesheets.AsQueryable();

        if (request.UserId.HasValue)
        {
            query = query.Where(t => t.UserId == request.UserId.Value);
        }

        if (request.StartDate.HasValue)
        {
            query = query.Where(t => t.Date >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(t => t.Date <= request.EndDate.Value);
        }

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<TimesheetStatus>(request.Status, out var status))
        {
            query = query.Where(t => t.Status == status);
        }

        // Get total count
        var totalCount = query.Count();

        // Apply paging and convert to list
        var items = query
            .OrderByDescending(t => t.Date)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList()
            .Select(t => new TimesheetDto
            {
                Id = t.Id,
                UserId = t.UserId,
                UserName = users.FirstOrDefault(u => u.Id == t.UserId)?.Username ?? "Unknown",
                Date = t.Date,
                Hours = t.Hours,
                PayTypeId = t.PayTypeId,
                PayTypeName = payTypes.FirstOrDefault(p => p.Id == t.PayTypeId)?.Name ?? "Unknown",
                WorkOrderId = t.WorkOrderId,
                WorkOrderNumber = t.WorkOrderId.HasValue 
                    ? workOrders.FirstOrDefault(w => w.Id == t.WorkOrderId)?.WorkOrderNumber 
                    : null,
                WorkOrderDescription = t.WorkOrderId.HasValue 
                    ? workOrders.FirstOrDefault(w => w.Id == t.WorkOrderId)?.Description 
                    : null,
                JobTypeId = t.JobTypeId,
                JobTypeName = t.JobTypeId.HasValue 
                    ? jobTypes.FirstOrDefault(j => j.Id == t.JobTypeId)?.Name 
                    : null,
                Notes = t.Notes,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToList();

        return new PagedResult<TimesheetDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
