using AutoMapper.Attributes;

namespace NESI.DTO.Models.Quote
{
	[MapsTo(typeof(NESI.Data.Entities.quote_worksheet))]
	[MapsFrom(typeof(NESI.Data.Entities.quote_worksheet))]
	public class QuoteWorksheet
	{
		public int id { get; set; }
		public int quote_id { get; set; }
		public int revision { get; set; }
		public int section_id { get; set; }
		public string part_no { get; set; }
		public string code { get; set; }
		public string description { get; set; }
		public double? sell { get; set; }
		public double? cost { get; set; }
		public double? extended_per { get; set; }
		public double? original_sell { get; set; }
		public double? qty { get; set; }
		public System.DateTime ts { get; set; }
		public string notes { get; set; }
		public long quote_worksheet_part_requested { get; set; }
		public int? member_id { get; set; }
		public double quote_worksheet_discount { get; set; }
		public int? consignment_id { get; set; }
		public int? cost_level { get; set; }
		public bool? has_file { get; set; }
	}
}