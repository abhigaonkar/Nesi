using MediatR;
using Nesi.Domain.Interfaces;
using Nesi.Domain.Entities;

namespace Nesi.Application.Commands.Timesheet;

public class CreateTimesheetCommandHandler : IRequestHandler<CreateTimesheetCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTimesheetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateTimesheetCommand request, CancellationToken cancellationToken)
    {
        // Validate user exists
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        // Validate pay type exists
        var payType = await _unitOfWork.Repository<PayType>().GetByIdAsync(request.PayTypeId);
        if (payType == null)
        {
            throw new InvalidOperationException("Pay type not found");
        }

        // Create timesheet entry
        var timesheet = new TimesheetEntry(
            request.UserId,
            request.Date,
            request.Hours,
            request.PayTypeId,
            request.WorkOrderId,
            request.JobTypeId,
            request.Notes ?? string.Empty);

        await _unitOfWork.Repository<TimesheetEntry>().AddAsync(timesheet);
        await _unitOfWork.SaveChangesAsync();

        return timesheet.Id;
    }
}
