using System.Net.Mail;
using System.Text.RegularExpressions;

namespace NESI.BLL.Common.Shared
{
	public static class Email
	{
		public static bool CheckEmail(string e)
		{
			return Regex.IsMatch(e.Trim(), "^([0-9a-zA-Z']([-.\\w']*[0-9a-zA-Z'])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
		}

		public static void AddAddress(string to, MailAddressCollection collection)
		{
			if (string.IsNullOrEmpty(to)) return;
			if (to.Contains(";"))
			{
				var addresses = to.Split(';');
				foreach (var t in addresses)
				{
					if (Email.CheckEmail(t))
					{
						collection.Add(new MailAddress(t));
					}
				}
			}
			else
			{
				if (to != "")
				{
					collection.Add(new MailAddress(to));
				}
				else
				{
					collection.Clear();
				}
			}
		}

	    public static bool IsSPAddress(string emailAddress)
	    {
	        if (!CheckEmail(emailAddress)) return false;
	        return emailAddress.Contains("thatsnew") || emailAddress.Contains("newelectric") ||
	               emailAddress.Contains("nesi");
	    }
	}
}