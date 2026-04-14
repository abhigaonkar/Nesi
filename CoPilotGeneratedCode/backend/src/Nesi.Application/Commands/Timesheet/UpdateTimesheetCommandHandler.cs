using MediatR;
using Nesi.Domain.Interfaces;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Application.Commands.Timesheet;

public class UpdateTimesheetCommandHandler : IRequestHandler<UpdateTimesheetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTimesheetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateTimesheetCommand request, CancellationToken cancellationToken)
    {
        var timesheet = await _unitOfWork.Repository<TimesheetEntry>().GetByIdAsync(request.Id);
        if (timesheet == null)
        {
            throw new InvalidOperationException("Timesheet not found");
        }

        // Only allow updates if timesheet is in Draft status
        if (timesheet.Status != TimesheetStatus.Draft)
        {
            throw new InvalidOperationException("Can only update timesheets in Draft status");
        }

        // Verify user owns this timesheet
        if (timesheet.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("You can only update your own timesheets");
        }

        // Update fields
        timesheet.UpdateDetails(
            request.Date,
            request.Hours,
            request.PayTypeId,
            request.WorkOrderId,
            request.JobTypeId,
            request.Notes);

        _unitOfWork.Repository<TimesheetEntry>().Update(timesheet);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
