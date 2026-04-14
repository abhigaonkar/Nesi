using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Page.TimeSheet.PEL;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet.PEL
{
	[RoutePrefix("api/Page/Timesheet/PEL")]
	public class TimesheetPELController : TimeSheetControllerBase
	{
		[Route("")]
		public IHttpActionResult GetProfile()
		{
			var o = new BLL.Pages.Timesheet.PEL.PELSchedule(CurrentUser);
			return Ok(o.Profile());
		}

		[Route("Review")]
		public IHttpActionResult Reveiw([FromBody] AddPEL model)
		{
			var o = new BLL.Pages.Timesheet.PEL.PELSchedule(CurrentUser);
			return Ok(o.Review(model));
		}

		[HttpPost]
		[Route("Save")]
		public IHttpActionResult AddVacation([FromBody] AddPEL model)
		{
			var o = new BLL.Pages.Timesheet.PEL.PELSchedule(CurrentUser);
			return Ok(o.Save(model));
		}

		[HttpDelete]
		[Route("Delete/{vacationId}")]
		public IHttpActionResult CancelVacation(int vacationId)
		{
			var o = new BLL.Pages.Timesheet.PEL.PELSchedule(CurrentUser);
			return Ok(o.Cancel(vacationId));
		}
	}
}
