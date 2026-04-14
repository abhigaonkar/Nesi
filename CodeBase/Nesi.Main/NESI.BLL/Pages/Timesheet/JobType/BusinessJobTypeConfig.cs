using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Timesheet.JobType
{
    public class BusinessJobTypeConfig
    {
        public int business_unit_id { get; set; }
        public bool allow_jobtype_selection { get; set; }
        public bool okay { get; set; }
    }
}
