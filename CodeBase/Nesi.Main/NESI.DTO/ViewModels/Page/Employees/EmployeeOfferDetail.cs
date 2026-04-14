using System;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeOfferDetail : EmployeeOfferBase
	{
		public string fullname { get; set; }
		public string status { get; set; }
		public DateTime? startdate { get; set; }
		public DateTime? enddate { get; set; }
		public int enteredby { get; set; }
		public string author { get; set; }
		public int reports_to { get; set; }
		public DateTime? start_minDate { get; set; }
		public DateTime? benefits_startdate { get; set; }
		public string notes { get; set; }
	}
}