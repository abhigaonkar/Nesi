using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Timesheet.JobType
{
    public class JobTypeRecordQueryParameter
    {
        public int business_uint_id { get; set; }
        public int customer_id { get; set; }
        public int member_id { get; set; }
        public int member_type_id { get; set; }

        public bool allow_jobtype_selection { get; set; }

        public bool okayOnGetJobTypes { get; set; }

        public JobTypeRecord defaultValue { get; set; }

        public JobTypeRecordQueryParameter()
        {
            this.okayOnGetJobTypes = false;
        }
    }
}
