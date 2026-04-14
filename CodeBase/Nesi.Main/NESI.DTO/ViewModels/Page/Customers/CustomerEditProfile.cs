using System;
using System.Data;

namespace NESI.DTO.ViewModels.Page.Customers
{
	public class CustomerEditProfile
	{
		public int customer_id { get; set; }
		public string customer_name { get; set; }
		public string customer_number { get; set; }
		public int status_id { get; set; }
		public bool is_partner { get; set; }
		public bool is_on_hold { get; set; }
		public string why_hold { get; set; }
		public string who_hold { get; set; }
		public int account_manager_id { get; set; }
		public int project_manager_id { get; set; }
		public int business_unit_id { get; set; }

		public string added_by { get; set; }
		public DateTime date_added { get; set; }
		public string qc_dt1 { get; set; }
		public string qc_dt2 { get; set; }
		public string qc_dt1_text { get; set; }
		public string qc_dt2_text { get; set; }
		public bool qc1approve_visible { get; set; }
		public bool qc2approve_visible { get; set; }
		public bool qc1_visible { get; set; }
		public bool qc2_visible { get; set; }
		public string[] disabled_reasons { get; set; }
		public DataTable addresses { get; set; }

		public double total_all { get; set; }
		public double total_1_year { get; set; }
		public double total_2_year { get; set; }
		public int reg_account_manager_id { get; set; }
		public int isr { get; set; }
		public int osr { get; set; }
        public bool active { get; set; }
		public bool? key_account { get; set; }

	}
}