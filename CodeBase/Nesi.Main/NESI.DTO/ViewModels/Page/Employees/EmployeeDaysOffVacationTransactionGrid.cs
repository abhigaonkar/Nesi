using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Employees
{
	[ModelDefination("EmployeeDaysOffVacationTransactionGrid")]
	public class EmployeeDaysOffVacationTransactionGrid : ModelBase<EmployeeDaysOffVacationTransactionGrid>
	{
		public int id { get; set; }
		public DateTime dt { get; set; }
		public double value_old { get; set; }
		public double value_new { get; set; }
		public double value_delta { get; set; }
		public string changed_by { get; set; }
		public string action { get; set; }
	}
}