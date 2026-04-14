using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/Edit")]
	public class EmployeeEditController : EmployeesControllerBase
    {
	    [HttpGet]
	    [Route("Profile/{memberid}")]
	    public IHttpActionResult GetProfile(int memberid)
	    {
		    var o = new BLL.Pages.Employees.EmployeeEdit(CurrentUser, memberid);
		    return Ok(o.Profile());
	    }
	}
}
