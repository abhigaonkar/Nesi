using System.ComponentModel.DataAnnotations;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteEditFieldUpdate
	{

		public int Line_id;
		[Required]
		public int QuoteId { get; set; }
		[Required]
		public int Revision { get; set; }
		[Required]
		public string FieldName { get; set; }
		public string FieldValue { get; set; }
		[Required]
		public long Ticks { get; set; }
	}
}