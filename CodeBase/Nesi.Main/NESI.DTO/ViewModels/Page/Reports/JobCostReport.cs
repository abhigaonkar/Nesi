using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("JobCostReport")]
    public class JobCostReport : ModelBase<JobCostReport>
    {
        public string pm { get; set; }
        public string wo { get; set; }
        public string status { get; set; }
        public string active { get; set; }
        public long quoteid { get; set; }
        public string customer_name { get; set; }
        public string descript { get; set; }
        public double quoted_amount { get; set; }

        public double matl_used { get; set; }
        public double bench_sell { get; set; }
        public double to_be_billed { get; set; }
        public string prog_billed { get; set; }
        public double total_cost { get; set; }
        public string quoted_amount_used { get; set; }
        public string overall_margin { get; set; }

        public double quoted_labor_hours { get; set; }
        public double actual_labor_hours { get; set; }
        public double labor_cost_tot { get; set; }
        public double labor_cost_per { get; set; }
        public string labor_used { get; set; }
        public double material_quote_price { get; set; }
        public string material_used { get; set; }

        public double labor_hours_remaining { get; set; }
        public double material_actual_price { get; set; }
        public string service_addr { get; set; }

        public DateTime cut_date { get; set; }
        public DateTime invoiced_date { get; set; }

        public double projected_labor_cost { get; set; }
        public double projected_material_cost { get; set; }
        public double projected_total_cost { get; set; }
        public string projected_margin { get; set; }
        public string quote_margin_p { get; set; }
        public double quote_margin_dollars { get; set; }

        public long woprog_id { get; set; }
    }
}
