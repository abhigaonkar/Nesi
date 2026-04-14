using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
    public class JobCostReport : BLLGridBase<DTO.ViewModels.Page.Reports.JobCostReport>
    {
        public JobCostReport()
        {
        }

        public JobCostReport(Employee user) : base(user)
        {
        }

        public JobCostReport(Employee user, BodyParams param, int business_unit) : base(user, param, new object[] { })
        {
            if (business_unit == 0)
            {
                business_unit = user.BusinessUnitId;
            }

            this.query_params = new object[] { business_unit };
            this.donotcache = true;
            this.query = "CALL report_jobcost('{bu_ids}')";
        }
    }
}
