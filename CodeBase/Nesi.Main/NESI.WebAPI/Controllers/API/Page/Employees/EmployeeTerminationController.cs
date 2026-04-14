using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using nesi.core;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Employees;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/Termination")]
	public class EmployeeTerminationController : EmployeesControllerBase
	{

		[HttpGet]
		[Route("Start/{memberid}")]
		public IHttpActionResult GetStartTerminate(int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeTermination(CurrentUser, memberid);
			if (!o.can_terminate) return  NotFound();
			return Ok(o.StartTerminateProfile());
		}

		[HttpPost]
		[Route("Start/{memberid}")]
		public IHttpActionResult PostStartTerminate([FromBody] EmployeeStartTerminate model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeTermination(CurrentUser, memberid);
			if (!o.can_terminate) return  NotFound();
			return Ok(o.StartTerminate(model));
		}

		[HttpGet]
		[Route("Profile/{memberid}")]
		public IHttpActionResult GetProfile(int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeTermination(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("Profile/{memberid}")]
		public IHttpActionResult SaveProfile([FromBody] EmployeeTerminationProfile model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeTermination(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveProfile(model));
		}

		[HttpGet]
		[Route("CheckList/{memberid}/{type}")]
		public IHttpActionResult GetCheckList(int memberid, string type)
		{
			var o = new BLL.Pages.Employees.EmployeeTermination(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetCheckList(type));
		}

		[HttpPost]
		[Route("CheckList/{memberid}/{type}")]
		public IHttpActionResult SaveCheckList([FromBody] EmployeeTerminationCheckListItem[] model, int memberid, string type)
		{
			var o = new BLL.Pages.Employees.EmployeeTermination(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveCheckList(model,type));
		}


		[HttpPost]
		[Route("CheckList/Item/{memberid}/{type}")]
		public IHttpActionResult SaveCheckListItem([FromBody] EmployeeTerminationCheckListItem model, int memberid, string type)
		{
			var o = new BLL.Pages.Employees.EmployeeTermination(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveCheckListItem(model, type));
		}
	}
}
