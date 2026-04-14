using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Timesheet.JobType
{
    public class JobTypeInfo
    {
        //
        // Three info the client side (browser) required.
        //
        public bool showJobType { get; set; }
        public List<JobTypeRecord> jobTypes { get; set; }
        public JobTypeRecord defaultValue { get; set; }

        //
        // Below are middle results, not care client.
        //
        public BusinessJobTypeConfig businessJobTypeConfig { get; set; }
        public JobTypeRecordQueryParameter jobTypeRecordQueryParameter { get; set; }

        public JobTypeInfo(JobTypeRecordQueryParameter parameter)
        {
            // By default, not show.
            this.jobTypes = new List<JobTypeRecord> { };
            this.showJobType = false;
            this.defaultValue = new JobTypeRecord { membertype_id = 0, membertype_name = "" };

            this.businessJobTypeConfig = new BusinessJobTypeConfig {  allow_jobtype_selection = false, business_unit_id = 0, okay = false};
            if (parameter == null)
            {
                this.jobTypeRecordQueryParameter = new JobTypeRecordQueryParameter { defaultValue = new JobTypeRecord { } };
            }
            else
            {
                this.jobTypeRecordQueryParameter = parameter;
                this.jobTypeRecordQueryParameter.defaultValue = new JobTypeRecord { };
            }
        }
    }
}
