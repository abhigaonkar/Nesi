using System;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeReviewListItem :EmployeeReviewBase
	{
		public DateTime date { get; set; }
		public bool locked { get; set; }
		public string reviewedby { get; set; }
		public string status { get; set; }
		public bool was_printed { get; set; }
		public string avg { get; set; }
		public string offerid { get; set; }
	}
}