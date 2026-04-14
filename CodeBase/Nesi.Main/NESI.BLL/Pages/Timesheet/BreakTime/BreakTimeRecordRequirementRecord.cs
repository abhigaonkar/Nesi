using System;

namespace NESI.BLL.Pages.Timesheet.BreakTime
{
    public class BreakTimeRecordRequirementRecord
    {
        public bool monitor_breaktime { get; set; }

        public TimeSpan def_start_time { get; set; }
        public TimeSpan def_morning_start { get; set; }
        public int def_morning_dur { get; set; }
        public TimeSpan def_lunch_start { get; set; }
        public int def_lunch_dur { get; set; }
        public TimeSpan def_afternoon_start { get; set; }
        public int def_afternoon_dur { get; set; }
    }
}