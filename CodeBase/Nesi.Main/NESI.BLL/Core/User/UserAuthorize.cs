using System;
using System.Reflection;
using log4net;
using NESI.Common.Password;

namespace NESI.BLL.Core.User
{
	public partial class User
	{
	    protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Check if user has the privilege to access the page by page id
		/// </summary>
		/// <param name="pageId"></param>
		/// <returns></returns>
		public bool AuthorizePage(int pageId)
		{
			return Pages?.FindAll(m =>
					   m.page_id == pageId && (m.menu_enabled == true || m.menu_mobile_enabled == true) &&
					   (
						   this.IsEmployee() ||
						   (m.menu_customer_enabled == true && this.IsCustomer()) ||
						   (m.menu_vendor_enabled == true && this.IsVendor())
					   )
				   ).Count > 0;

		}

		/// <summary>
		/// Check if user has the privilege by privilegeId
		/// </summary>
		/// <param name="privilegeId"></param>
		/// <returns></returns>
		public bool AuthenticatedForPrivilege(int privilegeId)
		{
			return Privileges?.FindAll(m =>
					   m.Privilege_ID == privilegeId && m.Privilege_Enabled == 1 &&
					   (
						   this.IsEmployee() ||
						   (m.Privilege_CustEnabled == 1 && this.IsCustomer()) ||
						   (m.Privilege_VendorEnabled == 1 && this.IsVendor())
					   )
				   ).Count > 0;
		}
	}
}