using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Core.Employee;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet
{
	[RoutePrefix("api/Page/Timesheet/Telem")]
	public class TimeSheetTelemController : TimeSheetControllerBase
	{
		/// <summary>
		/// update Telem
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPatch]
		[Route("")]
		public IHttpActionResult UpdateTelemTelem([FromBody] DTO.ViewModels.Page.TimeSheet.UpdateTimeSheetTelem model)
		{
			if (!CanSeeUser(model.SelectedBusinessUnitId, model.SelectedUserId)) return NotFound();

			return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).UpdateTelem(CurrentUser, model)));
		}


		/// <summary>
		/// insert telem
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("")]
		public IHttpActionResult InsertTelem([FromBody] DTO.ViewModels.Page.TimeSheet.InsertTimeSheetTelem model)
		{

			var selectedUser = new Employee(model.SelectedUserId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).SaveTelem(selectedUser, model)));
		}
	}
}
