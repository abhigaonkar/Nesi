using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet
{
	[PageAuthorizationFilter(28)]
	public class TimeSheetControllerBase : EmployeeController
	{
		
	}
}