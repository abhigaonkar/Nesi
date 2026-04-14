using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Timesheet.JobType
{
    public class JobTypeRecord
    {
        public int membertype_id { get; set; }
        public string membertype_name {get;set;}
        public int reports_to { get; set; }
        public string paytypeAllowed { get; set; }
        public string[] extraPaytypes { get; set; }
    }
}
