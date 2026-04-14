using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace NESI.DTO.Validation
{
	public class SQLInjectionAttribute : ValidationAttribute
	{
		protected string _message;
		public SQLInjectionAttribute(string message = ": Input value is invalid")
		{
			this._message = message;
		}
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var stringValue = value?.ToString() ?? string.Empty;
			var exp = new Regex(@"['"";]", RegexOptions.IgnoreCase);
			if (exp.IsMatch(stringValue))
			{
				throw new ValidationException(validationContext.MemberName + _message);
			}
			else
			{
				return ValidationResult.Success;

			}

		}
	}
}