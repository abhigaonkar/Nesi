using System.Collections.Generic;
using System.Data;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteSummary
	{
		public string[] Header { get; set; }
		public List<QuoteListItem[]> Body { get; set; }
		public string[] Sum { get; set; }
	}
}