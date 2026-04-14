using System;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeWageVacation
	{
		public int id { get; set; }
		public bool is_CDN_country { get; set; }
		public decimal vacation_amount_1 { get; set; }
		public decimal vacation_amount_2 { get; set; }
		public decimal vacation_amount_3 { get; set; }
		public int vacation_interval_1 { get; set; }
		public int vacation_interval_2 { get; set; }
		public int vacation_interval_3 { get; set; }
		public int timetostat { get; set; }
		public bool is_receive_stat_pay { get; set; }
	}
}