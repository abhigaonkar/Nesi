using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper.Attributes;

namespace NESI.DTO.Models.TimeSheet
{
	[MapsTo(typeof(NESI.Data.Entities.expense_reimbursement))]
	[MapsFrom(typeof(NESI.Data.Entities.expense_reimbursement))]
	public class ExpenseReimbursement
	{
		public int id_expense { get; set; }
		[Required]
		public long id_member { get; set; }
		public int approved { get; set; }
		public string receipt_number { get; set; }
		public DateTime date_requested { get; set; }
		public DateTime date_purchased { get; set; }
		public DateTime? date_start { get; set; }
		public DateTime? date_end { get; set; }
		public int id_payperiod { get; set; }
		[Required]
		public int id_seller { get; set; }
		public int? cust_number { get; set; }
		public int? wo_number { get; set; }
		public string item_text { get; set; }
		public decimal? amount { get; set; }
		public string currency { get; set; }
		public int? woprog_id { get; set; }
		public int? customer_id { get; set; }
		public int? master_id { get; set; }
		public bool? has_file { get; set; }
		public string file_ext { get; set; }
		public string file_mime { get; set; }
		public double? distance { get; set; }
		public string unit_distance { get; set; }
		public string attendees { get; set; }
		public bool? type { get; set; }
		public int? approved_by { get; set; }
	}
}