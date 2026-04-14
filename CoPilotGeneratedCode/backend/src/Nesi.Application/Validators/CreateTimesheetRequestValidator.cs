using FluentValidation;
using Nesi.Application.DTOs.Timesheet;

namespace Nesi.Application.Validators;

public class CreateTimesheetRequestValidator : AbstractValidator<CreateTimesheetRequest>
{
    public CreateTimesheetRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Date cannot be in the future");

        RuleFor(x => x.Hours)
            .GreaterThan(0).WithMessage("Hours must be greater than 0")
            .LessThanOrEqualTo(24).WithMessage("Hours cannot exceed 24 per day");

        RuleFor(x => x.PayTypeId)
            .GreaterThan(0).WithMessage("Pay type is required");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes must not exceed 500 characters");
    }
}
