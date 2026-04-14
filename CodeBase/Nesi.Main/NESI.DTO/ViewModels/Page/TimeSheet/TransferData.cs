using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
    public class TimesheetTransferData
    {
        public Timesheet originalTimesheetInfo { get; set; }
        public Timesheet targetTimesheetInfo { get; set; }
        public TimesheetValue timesheetValue { get; set; }

        public InsertTimeSheetWorkOrder ToBeAddedPositiveTimesheetRecord { get; set; }
        public InsertTimeSheetWorkOrder ToBeAddedNegativeTimesheetRecord { get; set; }
        public InsertTimeSheetShop ToBeAddedPositveShopTimeRecord { get; set; }

        public TimesheetTransferData()
        {
            this.originalTimesheetInfo = new Timesheet();
            this.targetTimesheetInfo = new Timesheet();
            this.timesheetValue = new TimesheetValue();
        }
    }

    public class TimesheetTransferResult
    {
        public bool okay { get; set; }
        public string summary { get; set; }
        public List<TransferErrorRecord> errors { get; set; }

        public TimesheetTransferResult()
        {
            this.okay = false;
            this.summary = "";
            this.errors = new List<TransferErrorRecord> { };
        }
    }

    public class TransferErrorRecord
    {
        public string field { get; set; }
        public string info { get; set; }
    }

    public class TimesheetValue
    {
        public int businessUnitId { get; set; }
        public int userId { get; set; }
        public DateTime selectedDate { get; set; }
        public bool allowUnlinkedTimesheet { get; set; }
        public bool allow_jobtype_selection { get; set; }
    }
}
