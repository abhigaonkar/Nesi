using MediatR;
using Nesi.Domain.Interfaces;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Application.Commands.Timesheet;

public class ApproveTimesheetCommandHandler : IRequestHandler<ApproveTimesheetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApproveTimesheetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ApproveTimesheetCommand request, CancellationToken cancellationToken)
    {
        var timesheet = await _unitOfWork.Repository<TimesheetEntry>().GetByIdAsync(request.Id);
        if (timesheet == null)
        {
            throw new InvalidOperationException("Timesheet not found");
        }

        // Can only approve if in Submitted status
        if (timesheet.Status != TimesheetStatus.Submitted)
        {
            throw new InvalidOperationException("Can only approve timesheets in Submitted status");
        }

        timesheet.Approve();
        _unitOfWork.Repository<TimesheetEntry>().Update(timesheet);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
