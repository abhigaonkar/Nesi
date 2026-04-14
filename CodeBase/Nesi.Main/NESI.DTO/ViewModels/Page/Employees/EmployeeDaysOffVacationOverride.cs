using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeDaysOffVacationOverride
	{
		[Required]
		public int member_id { get; set; }
		[Required, Range(0.01, 9999999)]
		public double amount { get; set; }
		[Required]
		public string memo { get; set; }
		[Required]
		public bool auto_vac_payout { get; set; }
	}
}