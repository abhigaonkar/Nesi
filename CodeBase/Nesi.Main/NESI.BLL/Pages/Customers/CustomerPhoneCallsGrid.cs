using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerPhoneCallsGrid : BLLGridBase<DTO.ViewModels.Page.Customers.CustomerPhoneCallsGrid>
	{
		public CustomerPhoneCallsGrid()
		{

		}

		public CustomerPhoneCallsGrid(Employee user) : base(user)
		{

		}

		public CustomerPhoneCallsGrid(Employee user, BodyParams param, int customer_id) : base(user, param)
		{
			this.query_params = new object[] { customer_id };
			this.query = $@"CALL customer_phone_log(@p0)";
		}
	}
}