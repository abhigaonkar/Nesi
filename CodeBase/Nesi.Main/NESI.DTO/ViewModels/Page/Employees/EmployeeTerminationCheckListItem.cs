using System;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeTerminationCheckListItem
	{
		public int id { get; set; }
		public string field { get; set; }
		public bool is_checked { get; set; }
		public bool is_completed { get; set; }
		public string response { get; set; }
		public string chkby_name { get; set; }
		public int chkby { get; set; }
		public bool resp_req { get; set; }
		public string comment { get; set; }
		public string type { get; set; }
		public string dt { get;set; }
	}
}