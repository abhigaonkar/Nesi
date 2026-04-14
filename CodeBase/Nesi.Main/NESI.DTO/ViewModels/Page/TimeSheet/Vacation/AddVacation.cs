using System;
using FluentValidation;
using FluentValidation.Attributes;

namespace NESI.DTO.ViewModels.Page.TimeSheet.Vacation
{
	[Validator(typeof(AddVacationValidator))]
	public class AddVacation
	{
		public DateTime Date_return { get; set; }
		public DateTime Date_start { get; set; }
		public DateTime Date_end { get; set; }
		public bool Alloutstanding { get; set; }
		public bool Unpaid { get; set; }
		public string Note { get; set; }
		public string O { get; set; }
		public double Hours { get; set; }
		public double Money { get; set; }
	}

	public class AddVacationValidator : AbstractValidator<AddVacation>
	{
		public AddVacationValidator()
		{
			RuleFor<DateTime>(x => x.Date_end).GreaterThanOrEqualTo(x => x.Date_start).WithMessage("End Date must be later than Start Date");
			RuleFor<DateTime>(x => x.Date_return).GreaterThanOrEqualTo(x => x.Date_start).WithMessage("Return Date must be later than Start Date");
		}
	}
}