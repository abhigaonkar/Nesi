using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class UpdateTimeSheetQuote : UpdateTimeSheetBase
	{
		[Required]
		public int MemberTime_ID { get; set; }
		[Range(minimum: 0, maximum: 5)]
		public int Rating { get; set; }
		[Required]
		public double NumberOfHours { get; set; }
		public double PercentComplete { get; set; }

	}
}