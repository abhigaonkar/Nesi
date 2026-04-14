using MediatR;
using Nesi.Domain.Interfaces;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Application.Commands.Timesheet;

public class SubmitTimesheetCommandHandler : IRequestHandler<SubmitTimesheetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public SubmitTimesheetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SubmitTimesheetCommand request, CancellationToken cancellationToken)
    {
        var timesheet = await _unitOfWork.Repository<TimesheetEntry>().GetByIdAsync(request.Id);
        if (timesheet == null)
        {
            throw new InvalidOperationException("Timesheet not found");
        }

        // Verify user owns this timesheet
        if (timesheet.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("You can only submit your own timesheets");
        }

        // Can only submit if in Draft status
        if (timesheet.Status != TimesheetStatus.Draft)
        {
            throw new InvalidOperationException("Can only submit timesheets in Draft status");
        }

        timesheet.Submit();
        _unitOfWork.Repository<TimesheetEntry>().Update(timesheet);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
