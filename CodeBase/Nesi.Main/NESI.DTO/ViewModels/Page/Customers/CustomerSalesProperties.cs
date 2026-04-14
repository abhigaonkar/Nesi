using System;
using AutoMapper.Attributes;

namespace NESI.DTO.ViewModels.Page.Customers
{
	[MapsTo(typeof(nesi.core.customer_sales_properties))]
	[MapsFrom(typeof(nesi.core.customer_sales_properties))]
	public class CustomerSalesProperties : CustomerBase
	{
		public int industry { get; set; }
		public string sector { get; set; }
		public string naics_code { get; set; }
		public int employee_size { get; set; }
		public string do_at_location { get; set; }
		public string affiliated_companies { get; set; }
		public string known_suppliers { get; set; }
		public string known_competitors { get; set; }
		public string why_choose { get; set; }
		public int origin { get; set; }
		public int member_id { get; set; }
		public int status_id { get; set; }
		public int call_cycle { get; set; }
		public int year_end { get; set; }
		public int project_mgr_member_id { get; set; }
		public int controls_mgr_member_id { get; set; }
		public int discount_pct { get; set; }
		public int decision_maker { get; set; }
		public string account_code { get; set; }
		public DateTime? next_followup_date { get; set; }
		public string next_followup_notes { get; set; }
		public double job_budget_threshold { get; set; }
		public bool po_required { get; set; }
		public bool confirmed_po_required { get; set; }
		public bool confirmed_tax_exempt { get; set; }

		public int isr_member_id { get; set; }
		public int osr_member_id { get; set; }
		public int ram_member_id { get; set; }
		public int mam_member_id { get; set; }
		public int cisr_member_id { get; set; }
		public int am_member_id { get; set; }

		public string notes_public { get; set; }
		public string notes_sales { get; set; }

		public bool found { get; set; }
		public string last_invoice { get; set; }
		public string last_fiscal { get; set; }
		public string current_fiscal { get; set; }
		public string ytd { get; set; }
		public string lytd { get; set; }
	}
}