using System;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeReviewMileStone
	{
		public int id { get; set; }
		public string milestone { get; set; }
		public DateTime due { get; set; }
		public int ticketid { get; set; }
		public string notes { get; set; }
		public bool completed { get; set; }

	}
}