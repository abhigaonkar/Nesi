using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee")]
    public class EmployeeListController : EmployeesControllerBase
    {
	    [HttpGet]
	    [Route("Profile")]
	    public IHttpActionResult GetProfile()
	    {
		    var o = new BLL.Pages.Employees.EmployeeList(CurrentUser);
		    return Ok(o.Profile());
	    }
	}
}
