using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
	[PageAuthorizationFilter(10)]
	public class CustomerControllerBase : EmployeeController
    {
    }
}
