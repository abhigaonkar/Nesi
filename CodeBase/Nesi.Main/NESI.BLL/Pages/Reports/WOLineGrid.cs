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
    public class WOLineGrid : BLLGridBase<DTO.ViewModels.Page.Reports.WOLineGrid>
    {
        public WOLineGrid()
        {
        }

        public WOLineGrid(Employee user) : base(user)
        {
        }

        public WOLineGrid(Employee user, BodyParams param) : base(user, param, new object[] { })
        {
            this.query = "CALL report_wo_line_grid_n2(@p0,'{bu_ids}')";
            this.query_params = new object[] { user.Id };
        }
    }
}
