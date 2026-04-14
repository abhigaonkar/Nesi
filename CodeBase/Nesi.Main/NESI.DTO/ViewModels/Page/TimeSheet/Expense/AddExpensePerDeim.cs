using System;
using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TimeSheet.Expense
{
	public class AddExpensePerDiem
	{
		[Required]
		public int[] Id_member { get; set; }
		[Required]
		public DateTime Date_start { get; set; }
		[Required]
		public DateTime Date_end { get; set; }
		public double Rate { get; set; }
		public string Wo_number { get; set; }
		public bool IsShop { get; set; }
		[Required]
		public double Amount { get; set; }
		

	}
}