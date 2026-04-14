using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("InvoicingTime")]
    public class InvoicingTime : ModelBase<InvoicingTime>
    {
        public string bvwo { get; set; }
        public string business_unit { get; set; }
        public string pm { get; set; }
        public string customer { get; set; }

        public double to_scan { get; set; }
        public double time_prep { get; set; }
        public double questions { get; set; }
        public double rework { get; set; }
        public double pm_app { get; set; }

        public double bm_app { get; set; }
        public double bm_parent_app { get; set; }
        public double cust_po { get; set; }
        public double to_be_invoiced { get; set; }
        public double total_time { get; set; }

        public double total_nesi { get; set; }
        public double total_branch { get; set; }
        public DateTime invoice_date { get; set; }
        public DateTime last_hour { get; set; }
        public DateTime wo_cut { get; set; }

        public double rev { get; set; }
        public string wo_status { get; set; }
        public string woprog_description { get; set; }
        public long id { get; set; }
    }
}
