namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteDetail
	{
		public string line_text { get; set; }
		public string row_id { get; set; }
		public double current_section_total { get; set; }
		public bool is_referenced { get; set; }
		public int count { get; set; }
		public int section_id { get; set; }
		public bool has_name { get; set; }
		public bool is_checked { get; set; }
		public int line_number { get; set; }
		public int order_number { get; set; }

	}
}