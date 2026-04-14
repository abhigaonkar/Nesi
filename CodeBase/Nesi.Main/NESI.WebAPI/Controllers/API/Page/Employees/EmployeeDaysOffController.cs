using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/DaysOff")]
    public class EmployeeDaysOffController : EmployeesControllerBase
	{
		[HttpGet]
		[Route("Override/{memberid}")]
		public IHttpActionResult GetOverride(int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeDaysOffVacationOverride(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("Override/{memberid}")]
		public IHttpActionResult SaveOverride([FromBody] DTO.ViewModels.Page.Employees.EmployeeDaysOffVacationOverride model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeDaysOffVacationOverride(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.Save(model));
		}

	}
}
