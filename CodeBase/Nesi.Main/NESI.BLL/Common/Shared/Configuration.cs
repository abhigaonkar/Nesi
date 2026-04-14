using System;
using System.Web.Configuration;
using System.Web;

namespace NESI.BLL.Common.Shared
{
	/// <summary>
	/// get app setting's value and public with readonly property
	/// setting at web.config app.settings
	/// </summary>
	public static class Configuration
	{
		static string _hostname;

		public static string HostName
		{
			get
			{
				if (string.IsNullOrEmpty(_hostname) && HttpContext.Current?.Request.UrlReferrer != null)
				{
					_hostname = HttpContext.Current.Request.UrlReferrer.ToString();
				}
				return _hostname;
			}
		}

		public static string Environment { get; } = GetConfigSetting("Environment", "Unknown");
		
        /// <summary>
        /// Token Expired Time
        /// </summary>
        public static int TokenExpiredTime { get; } = int.Parse(GetConfigSetting("tokenExpiredTime", "30"));


		/// <summary>
		/// nesi 1 auth key of mobile
		/// </summary>
		public static string MobileAuthKey { get; } = GetConfigSetting("mobile_auth_key", "$!18$uv58SjET9QaIH5");

		public static int TimesheetPastDayLength { get; } = int.Parse(GetConfigSetting("pastday_length", "180"));
		public static int SearchHistoryDisplay { get; } = int.Parse(GetConfigSetting("search_history_display", "5"));

		/// <summary>
		/// nesi 1 encrypt password
		/// </summary>
		/// 
		public static string EncryptionPass { get; } = GetRequiredConfigSetting("encryption_pass");

		public static bool DebugRedirect { get; } = GetConfigSetting("debug_redirect", "1") == "1";
		public static string DebugRedirectEmail { get; } = GetConfigSetting("debug_redirect_email", "lukelu@newelectric.com");

		public static string MxAddress { get; } = GetConfigSetting("mx_address", "smtp.newelectric.com");
		public static int ForcedChangePasswordDays { get; } = int.Parse(GetConfigSetting("ForcedChangePasswordDays", "98"));
		public static string LdapPath { get; } = GetConfigSetting("LDAP_Path", "LDAP://192.168.0.197/dc=NewElectric,dc=local");
		public static string LdapUser { get; } = GetConfigSetting("ldap_user", "administrator");
		public static string LdapPass { get; } = GetConfigSetting("ldap_pass", "20!0pp");
		public static string UNCBasePath { get; } = GetRequiredConfigSetting("UNC_base_path");

		public static string EmailFrom { get; } = GetConfigSetting("Email_From", "administrator@newelectric.com");
		public static string EmailIt { get; } = GetConfigSetting("Email_IT", "it@newelectric.com");
		public static string RequestPasswordTo { get; } = GetConfigSetting("RequestPassword_To", "insidesales@newelectric.com");
		public static string RequestPasswordBcc { get; } = GetConfigSetting("RequestPassword_BCC", "mhyde@newelectric.com");
		public static string ForgetPasswordTo { get; } = GetConfigSetting("ForgetPassword_To", "it@newelectric.com");
		public static string ForgetPasswordTo2 { get; } = GetConfigSetting("ForgetPassword_To2", "gbrennan@newelectric.com");
		public static string ForgetPasswordCc { get; } = GetConfigSetting("ForgetPassword_CC", "vgude@newelectric.com");
		public static string XpoConnection { get; } = GetRequiredConnectionString("MySQLXPO");
		public static int TimeOutOfPingTime { get; } = int.Parse(GetConfigSetting("TimeOutOfPingTime", "60"));
		public static string PageSizeForExcel { get; } = GetConfigSetting("PageSizeForExcel");
		public static string PageSizeForPdf { get; } = GetConfigSetting("PageSizeForPdf");
		public static string GridConfigFileName { get; } = GetConfigSetting("GridConfigFileName");
		public static string MemCacheAbsoluteTimeout { get; } = GetConfigSetting("MemCacheAbsoluteTimeout");
		public static string SvnUser { get; } = GetConfigSetting("svn_user", "lukelu");
		public static string SvnPassword { get; } = GetConfigSetting("svn_password", "luke123");
		public static string SvnServer { get; } = GetConfigSetting("svn_server", "https://na-devapp01:8443/svn/NESI-Intranet/trunk");

        public static string BillingEmail { get; }= GetConfigSetting("Billing_Email", "billing@sparkpowercorp.com");

        private static string GetConfigSetting(string key, string defaultValue = "")
		{
			return WebConfigurationManager.AppSettings[key] ?? defaultValue;
		}

		private static string GetRequiredConfigSetting(string settingKey)
		{
			if (string.IsNullOrEmpty(settingKey)) throw new ArgumentNullException(nameof(settingKey));
			return WebConfigurationManager.AppSettings[settingKey] ??
				   throw new InvalidOperationException($"Required application setting '{settingKey}' not found");
		}

		private static string GetRequiredConnectionString(string connectionName)
		{
			if (string.IsNullOrEmpty(connectionName)) throw new ArgumentNullException(nameof(connectionName));
			return WebConfigurationManager.ConnectionStrings[connectionName]?.ConnectionString ??
				   throw new InvalidOperationException($"Required connection string '{connectionName}' not found");
		}

        public  static string GetRevenueLineTM { get; } = GetConfigSetting("revenue_line_tm");
	    public static string GetRevenueLineQuoted { get; } = GetConfigSetting("revenue_line_quoted");

	    public static string GetNetSuiteRealm { get; } = GetConfigSetting("Realm");
        public static string GetNetSuiteConsumerKey { get; } = GetConfigSetting("ConsumerKey");
	    public static string GetNetSuiteConsumerSecret { get; } = GetConfigSetting("ConsumerSecret");
	    public static string GetNetSuiteToken { get; } = GetConfigSetting("Token");
	    public static string GetNetSuiteTokenSecret { get; } = GetConfigSetting("TokenSecret");
	    public static string GetNetSuiteIncomeStatementUrl { get; } = GetConfigSetting("IncomeStatementUrl");
    }
}