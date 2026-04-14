using System.Linq;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Core
{
	[RoutePrefix("api/Core/VisibleBusinessUnit")]
	public class VisibleBusinessUnitController : EmployeeController
	{
		[Route("")]
	    public IHttpActionResult Get()
	    {
		    return Ok(CurrentUser.VisibleBusinessUnitList.OrderBy(x=> x.ddl_name));
	    }

	}
}
