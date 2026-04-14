using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Employees
{
	[ModelDefination("EmployeeDaysOffVacationRequestGrid")]
	public class EmployeeDaysOffVacationRequestGrid : ModelBase<EmployeeDaysOffVacationRequestGrid>
	{
		public int vacation_id { get; set; }
		public int payperiod_id { get; set; }
		public DateTime date_insert { get; set; }
		public DateTime date_start { get; set; }
		public DateTime date_end { get; set; }
		public DateTime date_return { get; set; }
		public string status { get; set; }
		public string method { get; set; }
		public double amount { get; set; }
		public string comments { get; set; }
		public string payperiod { get; set; }
		public string requested_by { get; set; }
	}
}