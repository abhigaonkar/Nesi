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
    public class CustomerRatesGrid : BLLGridBase<DTO.ViewModels.Page.Reports.CustomerRatesGrid>
    {
        public CustomerRatesGrid()
        {
        }

        public CustomerRatesGrid(Employee user) : base(user)
        {
        }

        public CustomerRatesGrid(Employee user, BodyParams param) : base(user, param, new object[] {})
        {
            query = @"
SELECT customer.customer_id, 
d.ddl_name as business_unit, customer.customer_name as customer_name, membertype_chargeout.membertype_id, m.membertype_name as membertype_name ,
membertype_chargeout.business_unit_id, customer_rate.chargeout as chargeout, membertype_chargeout.Chargeout AS normal, 
customer_rate.last_updated as last_updated, customer_rate.from_date, customer_rate.to_date FROM customer_rate 
INNER JOIN customer ON customer_rate.customer_id = customer.Customer_ID 
INNER JOIN membertype_chargeout ON customer_rate.base_chargeout_id = membertype_chargeout.id 
LEFT JOIN business_unit d ON d.id = membertype_chargeout.business_unit_id
LEFT JOIN membertype m ON m.membertype_id = membertype_chargeout.membertype_id
WHERE membertype_chargeout.paytype_id = 1 AND membertype_chargeout.membertype_id NOT IN (10,11,14,15,16,3) AND find_in_set(d.id,'{bu_ids}') 
ORDER BY customer_name, chargeout
";
        }
    }
}
