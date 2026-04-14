using System.Security.Claims;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Page.BusinessUnit
{
    public class PageBusinessUnitController : EmployeeController
    {
  	    [Route("api/Page/BusinessUnit")]
	    public IHttpActionResult Get()
	      {
	          var stuff = User.Identity as ClaimsIdentity;

	        return Ok(new BLL.Pages.BusinessUnit.BusinessUnit().GetList(CurrentUser));
	    }

	

	}
}
