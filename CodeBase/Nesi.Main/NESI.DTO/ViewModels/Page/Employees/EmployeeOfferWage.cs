using System;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeOfferWage : EmployeeOfferBase
	{
		public double wage { get; set; }
		public bool is_CDN { get; set; }
		public bool part_time { get; set; }
		public int paytype_id { get; set; }
		public int vendor_id { get; set; }
		public double previous_wage { get; set; }
		public double vacation_amount_1 { get; set; }
		public double vacation_amount_2 { get; set; }
		public double vacation_amount_3 { get; set; }
		public int vacation_interval_1 { get; set; }
		public int vacation_interval_2 { get; set; }
		public int vacation_interval_3 { get; set; }
		public double minwage_1 { get; set; }
		public double maxwage_1 { get; set; }
		public double avgwage_1 { get; set; }
		public double minwage_2 { get; set; }
		public double maxwage_2 { get; set; }
		public double avgwage_2 { get; set; }
		public int bonus_type { get; set; }
		public double bonus_amount { get; set; }
		public double bonus_margin_threshold { get; set; }
		public double bonus_netincome_threshold { get; set; }
		public double bonus_netincome_highwater { get; set; }
		public double bonus_revenue_threshold { get; set; }
		public string comp_details { get; set; }
		public double bonus_amount_minValue { get; set; }
		public double bonus_amount_maxValue { get; set; }
		public string bonus_amount_tootip { get; set; }
		public DateTime benefits_startdate { get; set; }
		public EmployeeOfferWageNotes wage_notes { get; set; }
		public DateTime startdate { get; set; }
		public double start_wage { get; set; }
		public double current_wage { get; set; }
		public string contract_details { get; set; }
	}
}