using System;

namespace NESI.DTO.ViewModels.Page.TimeSheet.Vacation
{
	public class ReviewVacation
	{
		public string date_return { get; set; }
		public string date_start { get; set; }
		public string date_end { get; set; }
		public string type_of_payment { get; set; }
		public string[] affected_payrolls { get; set; }
		public string[] holidays { get; set; }
		public double hours { get; set; }
		public double available_hours { get; set; }
		public double available_dollars { get; set; }
		public double hours_atdate { get; set; }
		public double dollars_atdate { get; set; }
		public double dollars { get; set; }
		public bool isAlloutstanding { get; set; }

	}
}