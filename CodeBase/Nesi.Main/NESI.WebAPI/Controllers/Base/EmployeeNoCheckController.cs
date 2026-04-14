using NESI.BLL.Core.Employee;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.Base
{
	[NesiEmployeeNoCheckFilter]
	public class EmployeeNoCheckController : ApiControllerBase
	{
		protected new Employee CurrentUser => (Employee)base.CurrentUser;
		protected new Employee OriginalUser => (Employee)base.OriginalUser;
	}
}
