using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("MasterPurchasesGrid")]
    public class MasterPurchasesGrid : ModelBase<MasterPurchasesGrid>
    {
        public string business_unit { get; set; }
        public string status { get; set; }
        public string date_cut { get; set; }
        public string poprog_bvpo { get; set; }
        public string woprog_bvwo { get; set; }
        public string name_vendor { get; set; }
        public double total_cost { get; set; }
        public string date_required { get; set; }
        public string date_ordered { get; set; }
        public string date_received { get; set; }

        public string name_cut_by { get; set; }
        public double qty_ordered { get; set; }
        public double qty_received { get; set; }
        public string name_requested_by { get; set; }
        public long poprog_apstatus { get; set; }
        public string poprog_apstatus_name { get; set; }

        public string poprog_hasproblem_notes { get; set; }
        public string poprog_description { get; set; }
        public string pm_name { get; set; }
        public long poprog_id { get; set; }
        public long woprog_id { get; set; }
        public long vendor_id { get; set; }
    }
}
