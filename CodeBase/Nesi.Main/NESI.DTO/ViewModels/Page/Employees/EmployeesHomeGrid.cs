using System;
using NESI.Common;
using NESI.Common.Interface;

namespace NESI.DTO.ViewModels.Page.Employees
{
	[ModelDefination("EmployeesHomeGrid")]
	public class EmployeesHomeGrid : ModelBase<EmployeesHomeGrid>
	{
		public int memberid { get; set; }
		public string edit { get; set; }
		public int bu_id { get; set; }
		public string bu_name { get; set; }
		public string loginstatus { get; set; }
		public string member_payroll_id { get; set; }
		public string _name { get; set; }
		public string contact { get; set; }
		public string neemail { get; set; }
		public DateTime startdate { get; set; }
		public string cell { get; set; }
		public int hrstatus { get; set; }
		public string hrstatus_text { get; set; }
		public int membertypeid { get; set; }
		public string title { get; set; }
		public string ext { get; set; }
		public string user { get; set; }
		public string status { get; set; }
		public string day_off { get; set; }
		public string v { get; set; }
		public int mo_id { get; set; }
		public string signed { get; set; }
		public string mo_status { get; set; }
		public DateTime last_mobile_login { get; set; }
		public bool _in_mobile_contact_list { get; set; }
		public double years { get; set; }
		public DateTime emp_review_date { get; set; }
		public string emp_review_status { get; set; }
		public int emp_review_id { get; set; }
		public DateTime last_review_date { get; set; }
		public DateTime last_agreement { get; set; }
		public double gen_score { get; set; }
		public double cr_score { get; set; }
		public int morale { get; set; }
		public string tax_entity { get; set; }
		public string supervisor { get; set; }
		public string address { get; set; }
		public int te_id { get; set; }
		public double chargeout_rate { get; set; }

	}
}