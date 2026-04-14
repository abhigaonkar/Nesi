using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("InventoryUsage")]
    public class InventoryUsage : ModelBase<InventoryUsage>
    {
        public string dt { get; set; }
        public string branch { get; set; }
        public long master_id { get; set; }

        public string location { get; set; }
        public string member_name { get; set; }
        public string is_manual { get; set; }
        public string section { get; set; }
        public string description { get; set; }

        public long qty_before { get; set; }
        public long qty_after { get; set; }
        public long qty_diff { get; set; }

        public double db_before { get; set; }
        public double db_after { get; set; }
        public double db_diff { get; set; }
        public double cost_per { get; set; }
    }
}
