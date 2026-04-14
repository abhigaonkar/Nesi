using System;
using AutoMapper.Attributes;

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(NESI.Data.Entities.quote_master))]
	[MapsFrom(typeof(NESI.Data.Entities.quote_master))]
	public class Quote_Master
	{
		public long quote_id { get; set; }
		public int revision { get; set; }
		public int? active_revision { get; set; }
		public int? status_id { get; set; }
		public int? prev_status_id { get; set; }
		public long? customer_id { get; set; }
		public DateTime? open_date { get; set; }
		public DateTime? date_due { get; set; }
		public DateTime? completion_date { get; set; }
		public string job_description { get; set; }
		public string cust_spec_doc { get; set; }
		public long? quoted_by { get; set; }
		public int? quoter_locked { get; set; }
		public int? company_id { get; set; }
		public long division_id { get; set; }
		public long? killed_by { get; set; }
		public int? competitor_id { get; set; }
		public int? Contact_ID { get; set; }
		public int? include_title { get; set; }
		public DateTime? last_print_date { get; set; }
		public DateTime? last_fax_date { get; set; }
		public DateTime? verified_date { get; set; }
		public decimal quoted_price { get; set; }
		public decimal? price_to { get; set; }
		public decimal? take_off_price { get; set; }
		public string custom_term { get; set; }
		public decimal? tm_pricing { get; set; }
		public int net_due { get; set; }
		public int? percentage_down { get; set; }
		public int? us_currency { get; set; }
		public int? inflation_term { get; set; }
		public int? pricetype_id { get; set; }
		public string po { get; set; }
		public int? wo { get; set; }
		public long? duped_from_id { get; set; }
		public int? duped_from_rev { get; set; }
		public string updated_by_page { get; set; }
		public int? success_percent { get; set; }
		public byte? quote_master_apply_discount { get; set; }
		public int? address_id { get; set; }
		public string why_lose { get; set; }
		public string who_competitor { get; set; }
		public string what_price { get; set; }
		public string cust_reason_for_close { get; set; }
		public int? pct_chance { get; set; }
		public int? pct_chance_reason { get; set; }
		public string pct_chance_note { get; set; }
		public double hours_spent { get; set; }
		public bool? use_current_cost { get; set; }
		public double? margin { get; set; }
		public double? expected_value { get; set; }
		public bool? follow_up { get; set; }
        public bool? is_tm { get; set; }
		public int? allowed_to_quote { get; set; }
		public int? allowed_to_quote_2 { get; set; }
		public string killed_at_stage { get; set; }
		public DateTime? killed_date { get; set; }
		public string why_killed { get; set; }
		public DateTime? last_modified { get; set; }
		public int? currency { get; set; }
		public string why_revised { get; set; }
		public int? business_unit_id { get; set; }
		public DateTime? exp_podate { get; set; }
	}
}