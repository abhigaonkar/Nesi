using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESI.Common;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("MasterWorkOrderV2")]
    public class MasterWorkOrderV2 : ModelBase<MasterWorkOrderV2>
    {
        public long? woprog_id { get; set; }

        [SQLInjection()]
        public string customer { get; set; }

        // [SQLInjection()]
        public string description { get; set; }

        [SQLInjection()]
        public string quote { get; set; }

        public double? jobcost { get; set; }

        public double? remainder { get; set; }

        [SQLInjection()]
        public string status { get; set; }

        public double? to_be_billed { get; set; }

        public double? grossmargin { get; set; }

        public double? completion { get; set; }

        [SQLInjection()]
        public string pm { get; set; }

        public double? invoiced_amount { get; set; }

        public decimal? expected_sales { get; set; }

        public double? quotedamount { get; set; }

        public System.DateTime? invoice_date { get; set; }

        [SQLInjection()]
        public string bvwo { get; set; }

        [SQLInjection()]
        public string business_unit { get; set; }

        public long? woid { get; set; }

        public System.DateTime? cut_date { get; set; }

        public double? progress { get; set; }

        // [SQLInjection()]
        public string notes { get; set; }

        [SQLInjection()]
        public string account_manager { get; set; }

        public System.DateTime? expected_enddate { get; set; }

        public double? progressbilling { get; set; }

        public System.DateTime? woprog_last_email_date { get; set; }

        public int? days_since_cut { get; set; }

        public int? days_since_inv { get; set; }

        public bool? sc { get; set; }

        [SQLInjection()]
        public string po { get; set; }

        public bool? rd { get; set; }

        [SQLInjection()]
        public string asset { get; set; }

        public bool? hold { get; set; }

        [SQLInjection()]
        public string city { get; set; }

        [SQLInjection()]
        public string province { get; set; }

        [SQLInjection()]
        public string service_addr { get; set; }

        public System.DateTime? scanned_date { get; set; }

        public int? days_late { get; set; }

        [SQLInjection()]
        public string ram { get; set; }

        public decimal? materialcost { get; set; }

        public decimal? laborcost { get; set; }

        [SQLInjection()]
        public string country { get; set; }

        [SQLInjection()]
        public string customer_req_po { get; set; }

        public int? days_in_bucket { get; set; }

        public int? days_till_start { get; set; }

        public int? days_till_close { get; set; }

        public double? exp_hours { get; set; }

        public double? gross_profit { get; set; }

        public double? hours_left { get; set; }

        [SQLInjection()]
        public string inspection_link { get; set; }

        public bool? inspection_required { get; set; }

        public double? invoice_lag { get; set; }

        public System.DateTime? last_date_worked { get; set; }

        [SQLInjection()]
        public string woprog_associate_woprog_id { get; set; }

        public double? total_hours_worked { get; set; }

        public int? warranty { get; set; }

        public double? wo_total { get; set; }

        [SQLInjection()]
        public string assoc_id { get; set; }

        public int? business_unit_id { get; set; }

        public double? tmmargin { get; set; }

        [SQLInjection()]
        public string woprog_invoiceno { get; set; }
        public string acting_ram { get; set; }
        public bool open_pos { get; set; }
        public double? quote_costs { get; set; }
        public int? sustainability_project { get; set; }
        public bool Okay { get; set; }

        public decimal? tm_sell { get; set; }

        public double? hours_spent { get; set; }
        public string isr { get; set; }
        public string jobtag { get; set; }
        public System.DateTime? expected_startdate { get; set; }
        public decimal? quotedhours { get; set; }

          [SQLInjection()]
        public string controls_person { get; set; }

    }
}
