using System;

namespace NESI.BLL.Pages.Timesheet.BreakTime
{
    public class BreakTimeQueryParameter
    {
        public int member_id { get; set; }
        public int business_unit_id { get; set; }

        public DateTime date { get; set; }
    }
}