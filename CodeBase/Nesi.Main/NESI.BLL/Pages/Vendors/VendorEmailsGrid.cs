using System.Collections.Generic;
using System.Data;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorEmailsGrid : BLLGridBase<DTO.ViewModels.Page.Vendors.VendorEmailsGrid>
	{
		public VendorEmailsGrid()
		{

		}

		public VendorEmailsGrid(Employee user) : base(user)
		{

		}

		public VendorEmailsGrid(Employee user, BodyParams param, int vendor_id) : base(user, param)
		{
			var _emails = bllToolbox.doSQL_dt(@"SELECT contact_email FROM contact 
WHERE contact_type = 'vendor' AND contact_cust_id =@v0 
AND contact_status = 'active' AND contact_email NOT IN ('', 'needed', '@') AND contact_email LIKE '%@%.%'", vendor_id);
			var emails = new List<string>();
			if (_emails.Rows.Count > 0)
			{
				foreach (DataRow _address in _emails.Rows)
				{
					var email = "'\"" + _address["contact_email"] + "\"'";
					if (!emails.Contains(email))
					{
						emails.Add(email);
					}
				}
			}
			if (emails.Count == 0)
			{
				emails.Add("'_#_#_#_#_'");
			}
			this.query_params = new object[] { string.Join(" ", emails.ToArray()) };
			this.query = $@"SELECT 
	emaillog_timestamp `date`,
	emaillog_from `from_address`,
	emaillog_to `to_address`,
	emaillog_subject SUBJECT,
	emaillog_id
FROM 
	emaillog 
WHERE 
	MATCH(emaillog_to, emaillog_from) AGAINST (" + query_params[0] + @" IN BOOLEAN MODE)
ORDER BY DATE DESC";
		}
	}
}