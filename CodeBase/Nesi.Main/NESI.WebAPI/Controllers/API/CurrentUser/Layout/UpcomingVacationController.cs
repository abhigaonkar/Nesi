using System.Web.Http;
using NESI.BLL.Layout.Banner;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[Route("api/Layout/UpcomingVacations")]
	public class UpcomingVacationController : EmployeeController
	{
		public IHttpActionResult Get()
        {
            return Ok(new UpcomingVacations(CurrentUser).GetUpcomingVacations());
        }
    }
}
