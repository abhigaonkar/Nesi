using System;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeTerminationProfile 
	{
		public int member_id { get; set; }
		public int id { get; set; }
		public int mgr_member_id { get; set; }
		public int trm_member_id { get; set; }
		public DateTime dt_added { get; set; }
		public DateTime dt_modified { get; set; }
		public DateTime term_date { get; set; }
		public string term_time { get; set; }
		public string reason_actual { get; set; }
		public string reason_roe { get; set; }
		public bool is_returning { get; set; }
		public DateTime return_date { get; set; }
		public bool is_vac_bank { get; set; }
		public string vac_bank_detail { get; set; }
		public bool is_returned_property { get; set; }
		public double non_returned_value { get; set; }
		public DateTime last_day_worked { get; set; }
	}
}