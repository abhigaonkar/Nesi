using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Timesheet.BreakTime
{
    public class BreakTimeRecord
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public int business_unit_id { get; set; }
        public int member_id { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan morning_break_start { get; set; }
        public int morning_break_duration { get; set; }
        public TimeSpan lunch_break_start { get; set; }
        public int lunch_break_duration { get; set; }
        public TimeSpan afternoon_break_start { get; set; }
        public int afternoon_break_duration { get; set; }
        public int entered_by_member_id { get; set; }

        public bool locked { get; set; }

        public BreakTimeRecord()
        {
            this.locked = false;
        }
    }
}
