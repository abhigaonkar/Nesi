using System;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeWage
	{
		public int id { get; set; }
		public int member_id { get; set; }
		public DateTime date { get; set; }
		public double current_wage { get; set; }
		public DateTime? date_next_raise { get; set; }
		public string comment { get; set; }
		public int member_id_audit { get; set; }
		public string audit_username { get; set; }
		public int member_id_added_by { get; set; }
		public int business_unit_id { get; set; }
		public int membertype_id { get; set; }
		public int bonus_type { get; set; }
		public string bonus_type_name { get; set; }
		public double bonus_amount { get; set; }
        public bool active { get; set; }
    }
}