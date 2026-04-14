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
    public class InventoryCountsGrid : BLLGridBase<DTO.ViewModels.Page.Reports.InventoryCountsGrid>
    {
        public InventoryCountsGrid()
        {
        }

        public InventoryCountsGrid(Employee user) : base(user)
        {
        }

        public InventoryCountsGrid(Employee user, BodyParams param, int business_unit, int with_location) : base(user, param, new object[] { })
        {
            if (business_unit == 0)
            {
                business_unit = user.BusinessUnitId;
            }

            this.donotcache = true;
            this.query_params = new object[] { business_unit };

            string sql_withLocation = @"
SELECT master_id, qty_onhand, tag, description, IFNULL(cost, 0) cost, IFNULL(cost, 0) * IFNULL(qty_onhand, 0) extdcost, name loc, IFNULL(min, 0) *IFNULL(cost, 0) mincost 
FROM (SELECT a.master_id as master_id, f.qty as qty_onhand, f.min, c.tag as tag, d.description as description, e.cost as cost, g.`name` FROM inventory_item_master a LEFT JOIN inventory_branch b ON a.master_id = b.master_id LEFT JOIN inventory_tag c ON a.tag_id = c.tag_id LEFT JOIN inventory_description d ON d.master_id = a.master_id LEFT JOIN inventory_location f ON a.master_id = f.master_id and f.business_unit_id = b.business_unit_id LEFT JOIN inventory_location_master g ON g.id = f.location_master_id LEFT JOIN inventory_cost e ON e.master_id = a.master_id AND e.business_unit_id = b.business_unit_id WHERE a.active = true AND c.is_exclude = false 
AND c.active = true AND b.business_unit_id = @p0  ) parts ORDER BY master_id ASC";

            string sql_withoutLocation = @" 
SELECT master_id, qty_onhand, tag, description, IFNULL(cost, 0) cost, IFNULL(cost, 0) * IFNULL(qty_onhand, 0) extdcost, '' loc , 0 mincost 
FROM ( SELECT a.master_id as master_id, b.onhand_qty as qty_onhand, c.tag as tag, d.description as description, e.cost as cost FROM inventory_item_master a LEFT JOIN inventory_branch b ON a.master_id = b.master_id LEFT JOIN inventory_tag c ON a.tag_id = c.tag_id LEFT JOIN inventory_description d ON d.master_id = a.master_id LEFT JOIN inventory_cost e ON e.master_id = a.master_id AND e.business_unit_id = b.business_unit_id WHERE a.active = true AND c.is_exclude = false AND c.active = true
AND b.business_unit_id = @p0  ) parts ORDER BY master_id ASC";

            if (with_location == 1)
            {
                this.query = sql_withLocation;
            }
            else
            {
                this.query = sql_withoutLocation;
            }
        }
    }
}
