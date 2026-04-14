namespace NESI.Common.Password
{
	public class PasswordOptions
	{
		/// <summary>Minimum required length</summary>
		public int RequiredLength { get; set; }

		/// <summary>Require a non letter or digit character</summary>
		public bool RequireNonLetterOrDigit { get; set; }

		/// <summary>Require a lower case letter ('a' - 'z')</summary>
		public bool RequireLowercase { get; set; }

		/// <summary>Require an upper case letter ('A' - 'Z')</summary>
		public bool RequireUppercase { get; set; }

		/// <summary>Require a digit ('0' - '9')</summary>
		public bool RequireDigit { get; set; }

		/// <summary>
		/// Convenience builder for complex requirements
		/// </summary>
		public static PasswordOptions ComplexPassword => new PasswordOptions
		{
			RequireDigit = true,
			RequireLowercase = true,
			RequireNonLetterOrDigit = true,
			RequireUppercase = true,
			RequiredLength = 8
		};
	}
}