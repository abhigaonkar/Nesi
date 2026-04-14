
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
    public class MasterCustomersGrid : BLLGridBase<DTO.ViewModels.Page.Reports.MasterCustomersGrid>
    {
        public MasterCustomersGrid()
        {
        }

        public MasterCustomersGrid(Employee user) : base(user)
        {
        }

        public MasterCustomersGrid(Employee user, BodyParams param, DateTime used_from_dt, DateTime used_to_dt) : base(user, param, new object[] { })
        {
            var can_view_dollar_totals = user.AuthenticatedForPrivilege(60);

            this.query_params = new object[] { used_from_dt, used_to_dt, can_view_dollar_totals };
            this.query = "CALL report_master_customer(@p0, @p1, @p2)";
        }
    }
}
