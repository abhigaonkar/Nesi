using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/Contact")]
    public class EmployeeContactController : EmployeesControllerBase
	{

		[HttpGet]
		[Route("{memberid}")]
		public IHttpActionResult GetProfile(int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeContact(CurrentUser, memberid);
		//	if (!o.can_access) return NotFound();
			return Ok(o.Profile());
		}
	}
}
