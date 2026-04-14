using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("MasterQuoteGrid")]
    public class MasterQuoteGrid : ModelBase<MasterQuoteGrid>
    {
        public long quote { get; set; }
        public long rev { get; set; }
        public string business_unit { get; set; }
        public string customer { get; set; }
        public string contact { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public string status { get; set; }
        public DateTime open_date { get; set; }
        public string pm { get; set; }
        public string notes { get; set; }
        public string note { get; set; }
        public DateTime? completion { get; set; }
        public DateTime? date_due { get; set; }

        public string cust_notes { get; set; }
        public long pct_chance { get; set; }
        public string pct_chance_reason { get; set; }
        public string pct_chance_reason_str { get; set; }
        
        public double hours_spent { get; set; }
        public DateTime verified_date { get; set; }
        public DateTime last_fax_date { get; set; }
        public DateTime? last_sent_date { get; set; }
        public DateTime last_print_date { get; set; }
        public string country { get; set; }
        public double pipeline { get; set; }
        public DateTime? exp_podate { get; set; }
        public double dollar_margin { get; set; }
        public string margin { get; set; }
        public string acct_manager { get; set; }
        public string address_city { get; set; }
        public long contact_id { get; set; }
        public long expected_value { get; set; }
        public DateTime? qO_startdate { get; set; }
        public DateTime killed_date { get; set; }
        public bool parallel_bid { get; set; }
        public bool follow_up { get; set; }
        public string address_prov { get; set; }
        public string ram { get; set; }
        public DateTime? startdate { get; set; }
        public long status_id { get; set; }
        public long customer_id { get; set; }
        public long address_id { get; set; }

        public double totalquotedhours { get; set; }
		public string isr { get; set; }
        public long total_labour_qty { get; set; }
        public bool is_tm { get; set; }
        public string controls_person { get; set; }
    }
}
