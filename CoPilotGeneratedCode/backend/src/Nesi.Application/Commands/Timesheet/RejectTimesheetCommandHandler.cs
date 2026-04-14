using MediatR;
using Nesi.Domain.Interfaces;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Application.Commands.Timesheet;

public class RejectTimesheetCommandHandler : IRequestHandler<RejectTimesheetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RejectTimesheetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RejectTimesheetCommand request, CancellationToken cancellationToken)
    {
        var timesheet = await _unitOfWork.Repository<TimesheetEntry>().GetByIdAsync(request.Id);
        if (timesheet == null)
        {
            throw new InvalidOperationException("Timesheet not found");
        }

        // Can only reject if in Submitted status
        if (timesheet.Status != TimesheetStatus.Submitted)
        {
            throw new InvalidOperationException("Can only reject timesheets in Submitted status");
        }

        timesheet.Reject();
        _unitOfWork.Repository<TimesheetEntry>().Update(timesheet);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
