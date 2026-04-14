using nesi.core;
using NESI.BLL.Core;

// ReSharper disable All

namespace NESI.BLL.Common.Shared
{

	public static class Alert
	{
		/// <summary>
		/// copy from app_code\shared.cs
		/// </summary>
		/// <param name="status_subject"></param>
		/// <param name="status_message"></param>
		public static void Alert_it(string status_subject, string status_message)
		{
			var e = new NeEMail
			{
				To = Configuration.EmailIt,
				From = Configuration.EmailFrom,
				Subject = status_subject,
				Body = status_message,
				isHTML = true
			};
			e.Send();
		}

	}
}