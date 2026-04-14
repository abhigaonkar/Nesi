using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using nesi.core;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Employees;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/Privilege")]
	public class EmployeePrivilegesController : EmployeesControllerBase
	{
		[HttpGet]
		[Route("SwitchUser/{memberid}")]
		public IHttpActionResult GetSwitchUserProfile(int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeePrivileges(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetSwitchUserProfile());
		}

		[HttpPost]
		[Route("SwitchUser/{memberid}")]
		public IHttpActionResult SaveSwitchUser([FromBody] DataIntArray model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeePrivileges(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveSwitchUser(model.data));
		}

		[HttpGet]
		[Route("List/{memberid}/{buid}/{memberType}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetTreeList(int memberid, int buid, int memberType)
		{
			var o = new BLL.Pages.Employees.EmployeePrivileges(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetTreeList(buid, memberType == 1));
		}

		[HttpPost]
		[Route("List/{memberid}/{buid}/{memberType}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult SaveTreeList([FromBody] PagePrivilegeTreeNode[] model, int memberid, int buid, int memberType)
		{
			var o = new BLL.Pages.Employees.EmployeePrivileges(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveTree(model, buid, memberType == 1));
		}

		[HttpPost]
		[Route("Global/{memberid}")]
		public IHttpActionResult SaveGlobal([FromBody] PrivilegeTreeNode[] model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeePrivileges(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveGlobal(model));
		}
		[HttpPost]
		[Route("Report/{memberid}")]
		public IHttpActionResult SaveReport([FromBody] PrivilegeTreeNode[] model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeePrivileges(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveReport(model));
		}
	}
}
