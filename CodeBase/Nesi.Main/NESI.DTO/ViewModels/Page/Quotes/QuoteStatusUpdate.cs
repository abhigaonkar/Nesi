using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteStatusUpdate
	{
		[Required]
		public int quote_id { get; set; }
		[Required]
		public int revision { get; set; }
		public string who_competitor { get; set; }
		public string why_lose { get; set; }
		public string what_price { get; set; }
		[Required]
		public string status_id { get; set; }
		public bool is_revive { get; set; }
	}
}