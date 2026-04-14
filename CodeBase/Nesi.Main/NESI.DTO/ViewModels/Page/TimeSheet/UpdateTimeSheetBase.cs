using System;
using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class UpdateTimeSheetBase
	{

		public DateTime LocalDate => Date.ToLocalTime();
		[Required]
		public DateTime Date { get; set; }
		[Required]
		public int SelectedUserId { get; set; }
		[Required]
		public int SelectedBusinessUnitId { get; set; }
	}
}