using MediatR;
using Nesi.Application.DTOs.Timesheet;
using Nesi.Domain.Interfaces;
using Nesi.Domain.Entities;

namespace Nesi.Application.Queries.Timesheet;

public class GetTimesheetByIdQueryHandler : IRequestHandler<GetTimesheetByIdQuery, TimesheetDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTimesheetByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TimesheetDto?> Handle(GetTimesheetByIdQuery request, CancellationToken cancellationToken)
    {
        var timesheet = await _unitOfWork.Repository<TimesheetEntry>().GetByIdAsync(request.Id);
        if (timesheet == null)
        {
            return null;
        }

        var user = await _unitOfWork.Repository<User>().GetByIdAsync(timesheet.UserId);
        var payType = await _unitOfWork.Repository<PayType>().GetByIdAsync(timesheet.PayTypeId);
        
        Domain.Entities.WorkOrder? workOrder = null;
        if (timesheet.WorkOrderId.HasValue)
        {
            workOrder = await _unitOfWork.Repository<Domain.Entities.WorkOrder>().GetByIdAsync(timesheet.WorkOrderId.Value);
        }

        JobType? jobType = null;
        if (timesheet.JobTypeId.HasValue)
        {
            jobType = await _unitOfWork.Repository<JobType>().GetByIdAsync(timesheet.JobTypeId.Value);
        }

        return new TimesheetDto
        {
            Id = timesheet.Id,
            UserId = timesheet.UserId,
            UserName = user?.Username ?? "Unknown",
            Date = timesheet.Date,
            Hours = timesheet.Hours,
            PayTypeId = timesheet.PayTypeId,
            PayTypeName = payType?.Name ?? "Unknown",
            WorkOrderId = timesheet.WorkOrderId,
            WorkOrderNumber = workOrder?.WorkOrderNumber,
            WorkOrderDescription = workOrder?.Description,
            JobTypeId = timesheet.JobTypeId,
            JobTypeName = jobType?.Name,
            Notes = timesheet.Notes,
            Status = timesheet.Status,
            CreatedAt = timesheet.CreatedAt,
            UpdatedAt = timesheet.UpdatedAt
        };
    }
}
