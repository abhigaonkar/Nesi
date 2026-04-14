using System;
using System.Linq;
using System.Web.Http;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Page.TimeSheet;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet
{
	[RoutePrefix("api/Page/Timesheet")]
	public class PageTimeSheetController : TimeSheetControllerBase
	{

		[Route("List/{userId}/{date}")]
		public IHttpActionResult GetList(int userId, DateTime date)
		{
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			return Ok(new BLL.Pages.Timesheet.Timesheet(CurrentUser).GetList(userId, date));
		}

		[Route("EditPermission/{userId}/{date}")]
		public IHttpActionResult GetEditPermisssion(int userId, DateTime date)
		{
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			return OkD(new BLL.Pages.Timesheet.Timesheet(CurrentUser).GetEditPermission(userId, date));
		}

		[Route("VisibleBusinessUnit")]
		public IHttpActionResult GetVisibleBusinessUnitList()
		{
			return Ok(new BLL.Pages.Timesheet.Timesheet(CurrentUser).FillBusinessUnits());
		}

		[Route("Profile")]
		public IHttpActionResult GetProfile()
		{
			return Ok(new BLL.Pages.Timesheet.Timesheet(CurrentUser));
		}

		[Route("User/{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetUserList(int buid)
		{
			return Ok(new BLL.Pages.Timesheet.Timesheet(CurrentUser).FillUsers(buid));
		}

		[Route("User")]
		public IHttpActionResult GetUserList()
		{
			return Ok(new BLL.Pages.Timesheet.Timesheet(CurrentUser).FillUsers(0));
		}

		[Route("PastDays/{userId}")]
		public IHttpActionResult GetPastDays(int userId)
		{
			var selectedUser = new Employee(userId);
            if (!CanSeeUser(selectedUser)) return NotFound();
            
            var ts = new BLL.Pages.Timesheet.Timesheet(selectedUser);
			return Ok(
			 ControllerHelper.ConvertoJsonData(ts.GetPastDays(selectedUser.EmployeeProfile.member_startdate).Select(x => x.Date.ToString("yyyy-MM-dd")))
				);
		}


        [Route("Country/{buid}")]
        public IHttpActionResult GetCountry(int Buid)
        {
            return Ok(ControllerHelper.ConvertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).GetCountry(Buid)));
        }

        [Route("StartDate/{userId}")]
        public IHttpActionResult GetStartDate(int userId)
        {
            var selectedUser = new Employee(userId);
            if (!CanSeeUser(selectedUser)) return NotFound();
            var ts = new BLL.Pages.Timesheet.Timesheet(selectedUser);
            return Ok(
            ControllerHelper.ConvertoJsonData(ts.GetStarEndtDate(selectedUser))
            );
        }

        [Route("ProvinceList/{userId}")]
        public IHttpActionResult GetProvinceList(int userId)
        {
            var selectedUser = new Employee(userId);
            if (!CanSeeUser(selectedUser)) return NotFound();

            var ts = new BLL.Pages.Timesheet.Timesheet(selectedUser);
            return Ok(ControllerHelper.ConvertoJsonData(ts.GetProvinceList()));
        }

        [Route("DefaultProvince/{buid}")]
        public IHttpActionResult GetDefaultProvince(int Buid)
        {
            return Ok(ControllerHelper.ConvertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).GetDefaultProvince(Buid)));
        }

        [Route("PayperiodTotalHours/{userId}")]
		public IHttpActionResult getPayperiodTotalHours(int userId)
		{
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			var ts = new BLL.Pages.Timesheet.Timesheet(selectedUser);
			return Ok(ts.getPayperiodTotalHours(selectedUser));
		}

		[Route("ValueFromScheduler/{userId}/{date}")]
		public IHttpActionResult GetValueFromScheduler(int userId, DateTime date)
		{
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			var ts = new BLL.Pages.Timesheet.Timesheet(selectedUser);
			return Ok(
				ts.GetDefaultValueFromScheduler(date)
			);
		}

		[HttpDelete]
		[Route("Delete/{userId}/{id}")]
		public IHttpActionResult Delete(int userId, int id)
		{
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

            var ts = new BLL.Pages.Timesheet.Timesheet(CurrentUser);
            var info = ts.IsDeleteStartFromATransferedRecord(id, userId);
            if (info.DeleteFromTransferItem)
            {
                var r = ts.DeleteFromTransferredRecord(id, userId, info);
                return OkD(r);
            }

            // Below is the delete from normal timesheet record.

            // Limit daily cumulative time entry to 24 hours
            var error = ExceedMaxHoursOnOneDay_DeleteCase(id, CurrentUser);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return OkD(error);
            }

            return OkD(new BLL.Pages.Timesheet.Timesheet(CurrentUser).Delete(id,userId));
		}

        private string ExceedMaxHoursOnOneDay_DeleteCase(int id, Employee current)
        {
            BLL.Pages.Timesheet.Timesheet ts = new BLL.Pages.Timesheet.Timesheet(current);
            BLL.Pages.Timesheet.CumulativeParameter parameter = new BLL.Pages.Timesheet.CumulativeParameter();
            var record = ts.GetbyId(id);
            parameter.OnWhichDate = record.Date;
            parameter.ForWho = (int)record.membertime_memberid;
            parameter.action = BLL.Pages.Timesheet.TimesheetAction.Delete;
            parameter.newValue = record.NumberOfHours.Value;
            parameter.oldValue = 0;

            var result = ts.GetCumulativeInformation(parameter);
            if (result.DoesItExceedMaxValue)
            {
                return result.Errors[0].error;
            }
            else
            {
                // Not Exceeds
                return "";
            }
        }

    }
}
