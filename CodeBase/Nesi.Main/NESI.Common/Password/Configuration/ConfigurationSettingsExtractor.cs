using System;
using System.Configuration;

namespace NESI.Common.Password.Configuration
{
	/// <summary>
	/// Simple Configuration value extractor
	/// </summary>
	public static class ConfigurationSettingsExtractor
	{
		private static readonly string passwordConfigSection =
			"securitySettings/passwordRequirements";

		/// <summary>
		/// Load configuration settings from config file and return the settings
		/// </summary>
		/// <returns></returns>
		public static PasswordOptions GetConfiguredPasswordOptions()
		{
			var configValue = ConfigurationManager.GetSection(passwordConfigSection)
				as PasswordOptionsConfigurationSection;
			if (configValue == null)
				throw new InvalidOperationException("No configured password settings");

			return new PasswordOptions
			{
				RequiredLength = configValue.RequiredLength,
				RequireLowercase = configValue.RequireLowercase,
				RequireNonLetterOrDigit = configValue.RequireNonLetterOrDigit,
				RequireDigit = configValue.RequireDigit,
				RequireUppercase = configValue.RequireUppercase
			};
		}
	}
}