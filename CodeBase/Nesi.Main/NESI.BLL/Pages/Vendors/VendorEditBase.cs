using nesi.core;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorEditBase : VendorBase
	{
		protected readonly NEVendor vendor;


		public VendorEditBase(Employee user, int id) : base(user)
		{
			this.vendor_id = id;
			vendor = new NEVendor(id);
		}

	}
}