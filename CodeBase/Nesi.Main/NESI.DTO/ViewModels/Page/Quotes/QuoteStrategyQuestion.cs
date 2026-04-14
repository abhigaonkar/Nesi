namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteStrategyQuestion
	{
		public int id { get; set; }
		public string question { get; set; }
		public bool is_checked { get; set; }
		public string notes { get; set; }
		public string type { get; set; }
		public double ifyes { get; set; }
		public double ifno { get; set; }
	}
}