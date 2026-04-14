using System;
using System.Web.Http;
using nesi.core;
using NESI.BLL.Pages.Timesheet.Vacation;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.TimeSheet.Vacation;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet.Vacation
{
	[RoutePrefix("api/Page/Timesheet/Vacation")]
	public class PageTimeSheetVacationController : TimeSheetControllerBase
	{

		[Route("PayTypeId")]
		public IHttpActionResult GetPayTypeId()
		{
			return OkD(new BLL.Pages.Timesheet.Vacation.Vacation(CurrentUser).paytype_id == 1);
		}

		[Route("History")]
		public IHttpActionResult GetVacationHistory()
		{
			var list = new BLL.Pages.Timesheet.Vacation.Vacation(CurrentUser).View_Past();
			return Ok(list);
		}

		[Route("Available/Dollars")]
		public IHttpActionResult GetAvailableHours()
		{
			var res = new VacationBLL(CurrentUser).GetCurrentAvailable(1);
			return OkD(res);
		}
		[Route("Available/Hours")]
		public IHttpActionResult GetAvailableDollars()
		{
			var res = new VacationBLL(CurrentUser).GetCurrentAvailable(2);
			return OkD(res);
		}

		[Route("Available/AtDate")]
		public IHttpActionResult GetAvailableCrurrent()
		{
			var res = new VacationBLL(CurrentUser);
			return Ok(new
			{
				hours = res.GetCurrentAvailable(1),
				dollars = res.GetCurrentAvailable(2)
			});
		}

		[Route("Available/AtDate/{_date}")]
		public IHttpActionResult GetAvailableAtDate(DateTime _date)
		{
			var res = new VacationBLL(CurrentUser);
			return Ok(new
			{
				hours = res.GetAvailableAtDate(1, _date),
				dollars = res.GetAvailableAtDate(2, _date)
			});
		}

		[Route("Available/HoursAtDate/{_date}")]
		public IHttpActionResult GetAvailableHoursAtDate(DateTime _date)
		{
			var res = new VacationBLL(CurrentUser).GetAvailableAtDate(1, _date);
			return OkD(res);
		}
		[Route("Available/DollarsAtDate/{_date}")]
		public IHttpActionResult GetAvailableDollarsAtDate(DateTime _date)
		{
			var res = new VacationBLL(CurrentUser).GetAvailableAtDate(2, _date);
			return OkD(res);
		}
		[Route("Notes/{vacationId}")]
		public IHttpActionResult GetVacationNotes(int vacationId)
		{
			var list = new BLL.Pages.Timesheet.Vacation.Vacation(CurrentUser).Past_Notes(vacationId);
			return Ok(list);
		}
		[HttpPost]
		[Route("Notes/{vacationId}")]
		public IHttpActionResult AddVacationNote(int vacationId, [FromBody] DataString note)
		{
			var res = new BLL.Pages.Timesheet.Vacation.Vacation(CurrentUser).Request_Note_Add(vacationId, note.Data);
			return OkD(res);
		}
		[HttpDelete]
		[Route("Delete/{vacationId}")]
		public IHttpActionResult CancelVacation(int vacationId)
		{
			var res = new BLL.Pages.Timesheet.Vacation.Vacation(CurrentUser).Request_Cancel(vacationId);
			return OkD(res);
		}

		[HttpPost]
		[Route("Save")]
		public IHttpActionResult AddVacation([FromBody] AddVacation model)
		{
			var res = new VacationBLL(CurrentUser).AddVacation(model);
			return OkD(res);
		}

		[HttpPost]
		[Route("Review")]
		public IHttpActionResult ReviewVacation([FromBody] AddVacation model)
		{
			var res = new VacationBLL(CurrentUser).GetVacationReview(model);
			return Ok(res);
		}
	}
}
