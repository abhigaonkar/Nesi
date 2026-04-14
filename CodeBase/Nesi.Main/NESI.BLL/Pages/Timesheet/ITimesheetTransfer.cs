using NESI.DTO.ViewModels.Page.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Timesheet
{
    public interface ITimesheetTransfer
    {
        TimesheetTransferResult Transfer(TimesheetTransferData data);
        bool IsTransferable(DTO.ViewModels.Page.TimeSheet.Timesheet timesheet);
    }
}
    