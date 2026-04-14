using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Timesheet.JobType
{
    public interface IJobTypeService
    {
        BusinessJobTypeConfig GetBusinessJobTypeConfig(BusinessJobTypeConfigQueryParameter businessJobTypeConfigQueryParameter);
        List<JobTypeRecord> GetAvailableJobTypesForGivenEmployee(JobTypeRecordQueryParameter jobTypeQueryParameter);
        JobTypeInfo GetJobTypeInfo(JobTypeRecordQueryParameter jobTypeRecordQueryParameter);
    }
}
