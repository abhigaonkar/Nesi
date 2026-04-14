using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerArEmailsGrid : BLLGridBase<DTO.ViewModels.Page.Customers.CustomerArEmailsGrid>
	{
		public CustomerArEmailsGrid()
		{

		}

		public CustomerArEmailsGrid(Employee user) : base(user)
		{

		}

		public CustomerArEmailsGrid(Employee user, BodyParams param, int customer_id) : base(user, param)
		{
			this.query_params = new object[] { customer_id };
			this.query = $@"CALL CUSTCOLLECTIONEMAILS(@p0)";
		}
	}
}