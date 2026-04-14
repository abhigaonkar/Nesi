using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Vendors
{
	[PageAuthorizationFilter(11)]
	public class VendorControllerBase : EmployeeController
    {
    }
}
