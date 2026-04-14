using NESI.BLL.Pages.Timesheet.BreakTime;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet.Breaktime
{
    [PageAuthorizationFilter(28)]
    [RoutePrefix("api/Page/Timesheet/Breaktime")]
    public class BreaktimeController : EmployeeController
    {
        private readonly IBreakTimeService _iBreakTimeService = new BreakTimeService();

        [Route("BreakTimeRequirement")]
        [HttpGet]
        public IHttpActionResult GetBreakTimeRequirementForBusineness([FromUri] BranchBreakTimeRequirementInputParameter input)
        {
            var record = this._iBreakTimeService.GetBranchBreakTimeRequirement(input);
            return Ok(record);
        }

        [Route("BreakTimeRecordRequired")]
        [HttpGet]
        public IHttpActionResult GetBreakTimeRequirementOnSpecificDate([FromUri] BreakTimeQueryParameter input)
        {
            var record = this._iBreakTimeService.GetBreakTimeRequirementOnSpecificDate(input);
            return Ok(record);
        }

        [Route("AddBreakTimeRecord")]
        [HttpPost]
        public IHttpActionResult AddBreakTimeRecord([FromBody] BreakTimeRecord input)
        {
            var record = this._iBreakTimeService.AddBreakTimeRecord(input);
            return Ok(record);
        }

        [Route("UpdateBreakTimeRecord")]
        [HttpPost]
        public IHttpActionResult UpdateBreakTimeRecord([FromBody] BreakTimeRecord input)
        {
            // Get old value
            var queryInput = new BreakTimeQueryParameter { member_id = input.member_id, business_unit_id = input.business_unit_id, date = input.date };
            var list = this._iBreakTimeService.GetBreakTimeRecord(queryInput);
            var data = new BreakTimeRecordChangeNotification
            {
                PrviousRecord = list[0]
            };

            // Update
            var record = this._iBreakTimeService.UpdateBreakTimeRecord(input);
            data.PostRecord = record;

            if (record.success)
            {
                data.CurrentLoginUserId = CurrentUser.Id;
                data.CurrentLoginUserName = CurrentUser.FullName;
                data.memberIDInRRecord = record.member_id;
                
                // Sending email when record is updated.
                BreakTimeEmailSender.Send(data);

                // Log the change
                var logData = new BreakTimeRecordLog() { PrviousRecord =  list[0], PostRecord = record, Alt_Table_Id = CurrentUser.Id };
                BreakTimeRecordLogUpdater.Log(logData);
            }

            return Ok(record);
        }

        [Route("BreakTimeRecord")]
        [HttpGet]
        public IHttpActionResult GetBreakTimeRecords([FromUri] BreakTimeQueryParameter input)
        {
            var record = this._iBreakTimeService.GetBreakTimeRecord(input);
            return Ok(record);
        }
    }
}
