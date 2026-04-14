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
    public class CustomerLoginHistoryGrid : BLLGridBase<DTO.ViewModels.Page.Reports.CustomerLoginHistoryGrid>
    {
        public CustomerLoginHistoryGrid()
        {
        }

        public CustomerLoginHistoryGrid(Employee user) : base(user)
        {
        }

        public CustomerLoginHistoryGrid(Employee user, BodyParams param, int customer_id) : base(user, param, new object[] { customer_id })
        {
            this.query = @"
SELECT
    log_page.dt date,
    contact.Contact_Name contact,
    customer.Customer_Name cust,
    customer.customer_id customer_id,
    contact.Contact_ID contact_id
FROM
log_page
INNER JOIN contact ON log_page.member_id = (contact.Contact_ID + 100000)
INNER JOIN customer ON contact.Contact_Cust_ID = customer.Customer_ID
";

            if (customer_id > 0)
            {
                this.query = this.query + @" where customer.Customer_ID = @p0 ";
            }

            this.query = this.query + " group by date(log_page.dt),contact.Contact_Name order by log_page.dt desc";
        }
    }
}
