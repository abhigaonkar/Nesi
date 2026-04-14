using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Timesheet.BreakTime
{
    public class BreakTimeRecordResult: BreakTimeRecord
    {
        public bool success { get; set; }
        public string reason { get; set; }
        public OperationType operationType { get; set; }
    }

    public enum OperationType
    {
        Add = 0,
        Edit = 1
    }
}
