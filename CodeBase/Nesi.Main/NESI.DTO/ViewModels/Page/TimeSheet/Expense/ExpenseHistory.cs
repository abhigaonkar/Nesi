using System;

namespace NESI.DTO.ViewModels.Page.TimeSheet.Expense
{
	public class ExpenseHistory
	{
		public int id_expense { get; set; }
		public string approved { get; set; }
		public int approved_int { get; set; }
		public string pay_period { get; set; }
		public string item_text { get; set; }
		public int id_payperiod { get; set; }
		public string date_requested { get; set; }
		public string seller { get; set; }
		public double amount { get; set; }
		public string tooltip { get; set; }
		public string approved_by { get; set; }
		public string gl_name { get; set; }
		public DateTime date_purchased { get; set; }
	}
}