using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/Employment")]
	public class EmployeeEmploymentAgreementsController : EmployeesControllerBase
	{
		[HttpGet]
		[Route("List/{memberid}")]
		public IHttpActionResult GetList(int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeEmploymentAgreements(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetList());
		}

		[HttpGet]
		[Route("List/Applicant/{applicant_id}")]
		public IHttpActionResult GetApplicantList(int applicant_id)
		{
			var o = new BLL.Pages.Employees.EmployeeEmploymentAgreements(CurrentUser, true, applicant_id);
			if (!o.can_access) return NotFound();
			return Ok(o.GetList());
		}

		[HttpDelete]
		[Route("List/{memberid}/{id}")]
		public IHttpActionResult DeleteOffer(int memberid, int id)
		{
			var o = new BLL.Pages.Employees.EmployeeEmploymentAgreements(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.Delete(id));
		}

		[HttpDelete]
		[Route("List/Applicant/{applicant_id}/{id}")]
		public IHttpActionResult DeleteApplicantOffer(int applicant_id, int id)
		{
			var o = new BLL.Pages.Employees.EmployeeEmploymentAgreements(CurrentUser, true, applicant_id);
			if (!o.can_access) return NotFound();
			return Ok(o.Delete(id));
		}
	}
}
