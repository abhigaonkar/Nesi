using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/UserInfo")]
	public class EmployeeUserInfoController : EmployeesControllerBase
	{
		[HttpGet]
		[Route("Profile/{memberid}")]
		public IHttpActionResult GetProfile(int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeUserInfo(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("Profile/{memberid}")]
		public IHttpActionResult Save([FromBody] DTO.ViewModels.Page.Employees.EmployeeUserInfo model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeUserInfo(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.Save(model));
		}

		[HttpGet]
		[VisibleBusinessUnitFilter]
		[Route("ChargeOut/{buid}/{mtid}")]
		public IHttpActionResult GetChargeOutRate(int buid, int mtid)
		{
			var o = new BLL.Pages.Employees.EmployeeUserInfo(CurrentUser);
			return OkD(o.GetChargeout(buid, mtid));
		}

		[HttpPost]
		[Route("PrintBarCode/{memberid}")]
		public async Task<DataExtra> PrintBarCode([FromBody] DTO.ViewModels.Core.DataInt model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeUserInfo(CurrentUser, memberid);
			return await o.PrintBarCodeAsync(model.Data);
		}

		[HttpPost]
		[Route("DuplicateSIN/{memberid}")]
		public IHttpActionResult CheckDuplicateSIN([FromBody] DataString model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeUserInfo(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.CheckDuplicateSIN(model.Data));
		}


		[HttpPost]
		[Route("Reset_todo/{memberid}")]
		public IHttpActionResult ResetTodo([FromBody] DataString model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeUserInfo(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.Reset_Todo(model.Data));
		}


		[HttpPost]
		[Route("CheckReportsTo/{memberid}")]
		public IHttpActionResult CheckReportsTo([FromBody] DataString model, int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeUserInfo(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.CheckReportsTo(model.Data));
		}
	}
}
