using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("WOLineGrid")]
    public class WOLineGrid : ModelBase<WOLineGrid>
    {
        public string wo { get; set; }
        public string customer { get; set; }

        public string pM { get; set; }
        public string wo_status { get; set; }
        public long master_id { get; set; }

        public string description { get; set; }
        public double qty_required { get; set; }
        public double qty_committed { get; set; }
        public double qty_unfulfilled { get; set; }
        public double int_onhand_qty { get; set; }
        public double not_received { get; set; }

        public string notes { get; set; }
        public string date_required { get; set; }
        public string date_received { get; set; }
        public string po { get; set; }
        public string po_status { get; set; }
        public string date_expected { get; set; }
        public double stillNotOrdered { get; set; }
        public long daysLeft { get; set; }
        public double ext_onhand_qty { get; set; }
        public string wo_lineitem_billtype_name { get; set; }
        public double cost { get; set; }
        public double sell { get; set; }
        public double total_sell { get; set; }
        public double total_cost { get; set; }
        public string business_unit { get; set; }
        public string job_type { get; set; }
        public long is_consumable { get; set; }

        public long poprog_id { get; set; }
        public long customer_id { get; set; }
        public long billtype_id { get; set; }
        public long quote_id { get; set; }
        public long wo_detail_current_woprog_id { get; set; }
    }
}
