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
    public class InvoicingTime : BLLGridBase<DTO.ViewModels.Page.Reports.InvoicingTime>
    {
        public InvoicingTime()
        {
        }

        public InvoicingTime(Employee user) : base(user)
        {
        }

        public InvoicingTime(Employee user, BodyParams param) : base(user, param, new object[] { })
        {
           this.query = @"CALL REPORT_INVOICELAG({bu_ids})";

        }
    }
}
