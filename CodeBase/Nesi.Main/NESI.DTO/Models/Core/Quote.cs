using System;
using AutoMapper.Attributes;

namespace NESI.DTO.Models.Core
{

	[MapsTo(typeof(NESI.Data.Entities.quote))]
	[MapsFrom(typeof(NESI.Data.Entities.quote))]
	public class Quote
	{
		public string quote_n { get; set; }
		public long quote_id { get; set; }
		public int? business_unit_id { get; set; }
		public int revision { get; set; }
		public long? customer_id { get; set; }
		public DateTime? open_date { get; set; }
		public DateTime? date_due { get; set; }
		public DateTime? completion_date { get; set; }
		public string job_description { get; set; }
		public string cust_spec_doc { get; set; }
		public long? quoted_by { get; set; }
		public long? killed_by { get; set; }
		public int? status_id { get; set; }
		public int? prev_status_id { get; set; }
		public int? Contact_ID { get; set; }
		public DateTime? last_print_date { get; set; }
		public DateTime? last_fax_date { get; set; }
		public DateTime? verified_date { get; set; }
		public decimal quoted_price { get; set; }
		public decimal quote_price { get; set; }
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
		public int? allowed_to_quote { get; set; }
		public int? address_id { get; set; }
		public double? expected_value { get; set; }
		public int? allowed_to_quote_2 { get; set; }
		public string killed_at_stage { get; set; }
		public DateTime? killed_date { get; set; }
		public string why_killed { get; set; }
		public int? pct_chance { get; set; }
		public int? currency { get; set; }
	}
}