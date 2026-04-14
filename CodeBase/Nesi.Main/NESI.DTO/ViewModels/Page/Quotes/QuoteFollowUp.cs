using System;
using System.ComponentModel.DataAnnotations;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteFollowUp
	{
		[Required]
		public int quote_id { get; set; }
		[Required]
		public int revision { get; set; }
		[Required]
		public DateTime schedule_date { get; set; }
		[Required]
		public string follow_up_note { get; set; }
	}
}