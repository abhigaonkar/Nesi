using NESI.BLL.Core.Employee;
using NESI.BLL.Pages.Timesheet;
using NESI.DTO.ViewModels.Page.TimeSheet;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet
{
    [RoutePrefix("api/Page/Timesheet/WorkOrder")]
    public class TimesheetTransferController : TimeSheetControllerBase
    {

        [HttpPost]
        [Route("Transfer")]
        public IHttpActionResult TransferWorkOrder([FromBody] TimesheetTransferData model)
        {
            var selectedUser = new Employee(model.timesheetValue.userId);
            ITimesheetTransfer _tst = new TimesheetTransferService(CurrentUser, selectedUser);
            
            if (!CanSeeUser(selectedUser))
            {
                return NotFound();
            }

            var result = _tst.Transfer(model);
            return Ok(result);
        }
    }
}