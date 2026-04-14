using System.Data;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerHomeSearch : BLLGridBase<DTO.ViewModels.Page.Customers.CustomerHomeSearch>
	{
		public CustomerHomeSearch()
		{

		}

		public CustomerHomeSearch(Employee user): base(user)
		{

		}

		public CustomerHomeSearch(Employee user, BodyParams param, string criteria) : base(user, param, new object[] { criteria })
		{

			this.query= @"CALL CRM_SEARCH_V2(@p0, 1, 0, 0)";
		}

	}
}