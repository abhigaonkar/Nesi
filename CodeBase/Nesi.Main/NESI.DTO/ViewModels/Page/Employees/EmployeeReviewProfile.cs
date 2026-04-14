using System;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeReviewProfile: EmployeeReviewBase
	{
		public string fullName { get; set; }
		public DateTime date { get; set; }
		public bool locked { get; set; }
		public int reviewed_by_id { get; set; }
		public string status { get; set; }
		public int offerid { get; set; }
		public bool was_printed { get; set; }
		public bool is_supervisor { get; set; }
		public string membertype_name { get; set; }
	}
}