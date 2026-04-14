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
    public class InventoryUsage : BLLGridBase<DTO.ViewModels.Page.Reports.InventoryUsage>
    {
        public InventoryUsage()
        {
        }

        public InventoryUsage(Employee user) : base(user)
        {
        }

        public InventoryUsage(Employee user, BodyParams param,  int business_unit) : base(user, param, new object[] { })
        {
            //if (business_unit == 0)
            //{
            //    business_unit = user.BusinessUnitId;
            //}

            //this.query_params = new object[] { business_unit };

            query = @" SELECT a.dt, a.location, a.member_name, a.qty_before, a.qty_after, a.qty_diff, 
(a.cost_per*a.qty_before) db_before, 
(a.cost_per*a.qty_after) db_after, 
((a.cost_per*a.qty_after) - (a.cost_per*a.qty_before)) db_diff,
a.is_manual, a.section, b.ddl_name branch, a.cost_per, a.master_id, a.description 
FROM inventory_usage a 
LEFT JOIN business_unit b ON a.`business_unit_id` = b.id  WHERE find_in_set(a.business_unit_id,'{bu_ids}') ORDER BY a.id DESC
";
        }
    }
}
