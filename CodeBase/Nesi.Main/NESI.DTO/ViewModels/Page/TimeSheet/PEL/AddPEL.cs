using System;

namespace NESI.DTO.ViewModels.Page.TimeSheet.PEL
{
	public class AddPEL
	{
		public DateTime Date_start { get; set; }
		public DateTime Date_end { get; set; }
		public string Note { get; set; }
		public double Hours { get; set; }
	}
}