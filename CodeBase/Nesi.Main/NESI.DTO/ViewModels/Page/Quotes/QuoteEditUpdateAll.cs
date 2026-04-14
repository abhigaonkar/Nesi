using System;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteEditUpdateAll
	{
		public int quote_id { get; set; }
		public int revision { get; set; }
		public int customer_id { get; set; }
		public int address_id { get; set; }
		public string customer_name { get; set; }
		public int customer_contact { get; set; }
		public DateTime date_due { get; set; }
		public DateTime? date_expected_start { get; set; }
		public DateTime? exp_podate { get; set; }
		public int chance_winning { get; set; }
		public int  chance_winning_reason { get; set; }
		public string chance_winning_note { get; set; }
		public DateTime completion_date { get; set; }
		public string job_description { get; set; }
		public bool follow_up { get; set; }
        public bool is_tm { get; set; }
		public string cust_spec_doc { get; set;}
		public int quoted_by { get; set; }
		public int quoted_business_unit_id { get; set; }

        public int revenue_line_id { get; set; }

        public  int pricetype_id { get; set; }
		public decimal quoted_price { get; set; }
		public decimal price_to { get; set; }
		public double expected_value { get; set; }
		public bool us_currency { get; set; }
		public bool inflation_term { get; set; }
		public int percent_down { get; set; }
		public int net_due { get; set; }
		public string customer_term { get; set; }
		public long ts_ticks { get; set; }
		public bool quoter_locked_bool { get; set; }
		public int period_valid { get; set; }
		public int? opportunity_internal_id { get; set; }
		public bool parallel_bid { get; set; }
		public int? bdm { get; set; }
		public int? acting_bdm { get; set; }
	}
}