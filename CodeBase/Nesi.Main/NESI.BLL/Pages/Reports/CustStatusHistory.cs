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
    public class CustStatusHistory : BLLGridBase<DTO.ViewModels.Page.Reports.CustStatusHistory>
    {
        public CustStatusHistory()
        {
        }

        public CustStatusHistory(Employee user) : base(user)
        {
        }

        public CustStatusHistory(Employee user, BodyParams param) : base(user, param, new object[] { })
        {
            this.query = "CALL ds_master_customer_status_n2('{bu_ids}')";
        }
    }
}
