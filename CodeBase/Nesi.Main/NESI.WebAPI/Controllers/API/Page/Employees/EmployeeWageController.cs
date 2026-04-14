using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dto = NESI.DTO.ViewModels.Page.Employees.EmployeeWageVacation;
using dtoWage = NESI.DTO.ViewModels.Page.Employees.EmployeeWage;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/Wage")]
    public class EmployeeWageController : EmployeesControllerBase
	{

		[HttpGet]
		[Route("Vacation/{memberid}")]
		public IHttpActionResult GetProfile(int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeWage(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("Vacation/{memberid}")]
		public IHttpActionResult SaveVacation([FromBody] dto model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeWage(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveVacation(model));
		}


		[HttpPost]
		[Route("Wage/{memberid}")]
		public IHttpActionResult SaveWage([FromBody] dtoWage model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeWage(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveWage(model));
		}

	}
}
