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
    public class MasterPhoneCallGrid : BLLGridBase<DTO.ViewModels.Page.Reports.MasterPhoneCallGrid>
    {
        public MasterPhoneCallGrid()
        {
        }

        public MasterPhoneCallGrid(Employee user) : base(user)
        {
        }

        public MasterPhoneCallGrid(Employee user, BodyParams param) : base(user, param, new object[] { })
        {
            this.query = "SELECT * FROM neintranet.phone_log where phone_log_date >= curdate() - interval 8 month  order by phone_log_date desc";
        }
    }
}
