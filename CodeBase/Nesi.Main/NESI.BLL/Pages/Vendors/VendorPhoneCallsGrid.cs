using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorPhoneCallsGrid : BLLGridBase<DTO.ViewModels.Page.Vendors.VendorPhoneCallsGrid>
	{
		public VendorPhoneCallsGrid()
		{

		}

		public VendorPhoneCallsGrid(Employee user) : base(user)
		{

		}

		public VendorPhoneCallsGrid(Employee user, BodyParams param, int vendor_id) : base(user, param)
		{
			this.query_params = new object[] { vendor_id };
			this.query = $@"CALL vendor_phone_log_copy(@p0)";
		}
	}
}