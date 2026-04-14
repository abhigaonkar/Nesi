using System;
using System.Linq;
using System.Threading.Tasks;
using NESI.Common.Extensions;

namespace NESI.Common.Password
{
	/// <summary>
	/// Validates a given password against specified criteria
	/// </summary>
	public static class PasswordValidator
	{
		/// <summary>
		/// Validate the given password
		/// </summary>
		/// <param name="password">Password to validate</param>
		/// <param name="options">Validation options</param>
		/// <returns>Validation result</returns>
		public static Task<PasswordValidationError> ValidateAsync(string password, PasswordOptions options) =>
			Task.FromResult(Validate(password, options));

		/// <summary>
		///     Ensures that the string is of the required length and meets the configured requirements
		/// </summary>
		/// <param name="password">Password to validate</param>
		/// <param name="options">Validation options</param>
		/// <returns>Validation result</returns>
		public static PasswordValidationError Validate(string password, PasswordOptions options)
		{
			if (password == null) throw new ArgumentNullException(nameof(password));
			if (options == null) throw new ArgumentNullException(nameof(options));

			var result = PasswordValidationError.None;

			if (string.IsNullOrWhiteSpace(password) || password.Length < options.RequiredLength)
			{
				result |= PasswordValidationError.PasswordTooShort;
			}

			if (options.RequireNonLetterOrDigit && password.All(c => c.IsLetterOrDigit()))
			{
				result |= PasswordValidationError.NoNonletterOrDigit;
			}

			if (options.RequireDigit && password.All(c => !c.IsDigit()))
			{
				result |= PasswordValidationError.NoDigit;
			}

			if (options.RequireLowercase && password.All(c => !c.IsLower()))
			{
				result |= PasswordValidationError.NoLowercase;
			}

			if (options.RequireUppercase && password.All(c => !c.IsUpper()))
			{
				result |= PasswordValidationError.NoUppercase;
			}

			return result;
		}
	}
}