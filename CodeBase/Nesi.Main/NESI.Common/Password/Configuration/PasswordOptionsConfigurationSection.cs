using System.Configuration;

namespace NESI.Common.Password.Configuration
{
	/// <summary>
	/// Class that maps to a configuration section in the web.config file
	/// </summary>
	public class PasswordOptionsConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty(nameof(RequiredLength), DefaultValue = "6", IsRequired = false)]
		public int RequiredLength
		{
			get => (int)this[nameof(RequiredLength)];
			set => this[nameof(RequiredLength)] = value;
		}

		[ConfigurationProperty(nameof(RequireNonLetterOrDigit), DefaultValue = "false", IsRequired = false)]
		public bool RequireNonLetterOrDigit
		{
			get => (bool)this[nameof(RequireNonLetterOrDigit)];
			set => this[nameof(RequireNonLetterOrDigit)] = value;
		}

		[ConfigurationProperty(nameof(RequireLowercase), DefaultValue = "false", IsRequired = false)]
		public bool RequireLowercase
		{
			get => (bool)this[nameof(RequireLowercase)];
			set => this[nameof(RequireLowercase)] = value;
		}

		[ConfigurationProperty(nameof(RequireUppercase), DefaultValue = "false", IsRequired = false)]
		public bool RequireUppercase
		{
			get => (bool)this[nameof(RequireUppercase)];
			set => this[nameof(RequireUppercase)] = value;
		}

		[ConfigurationProperty(nameof(RequireDigit), DefaultValue = "false", IsRequired = false)]
		public bool RequireDigit
		{
			get => (bool)this[nameof(RequireDigit)];
			set => this[nameof(RequireDigit)] = value;
		}


	}
}