using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("InventoryCountsGrid")]
    public class InventoryCountsGrid : ModelBase<InventoryCountsGrid>
    {
        public string tag { get; set; }
        public long master_id { get; set; }

        public string description { get; set; }
        public double cost { get; set; }
        public long qty_onhand { get; set; }
        public double extdcost { get; set; }
        public double mincost { get; set; }
        public string loc { get; set; }
    }
}
