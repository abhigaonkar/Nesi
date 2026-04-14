using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerEmailsGrid : BLLGridBase<DTO.ViewModels.Page.Customers.CustomerEmailsGrid>
	{
		public CustomerEmailsGrid()
		{

		}

		public CustomerEmailsGrid(Employee user) : base(user)
		{

		}

		public CustomerEmailsGrid(Employee user, BodyParams param, int customer_id) : base(user, param)
		{
			this.query_params = new object[] { customer_id };
			this.query = $@"CALL CUSTEMAILS(@p0)";
		}
	}
}