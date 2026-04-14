using System;
using System.ComponentModel.DataAnnotations;
using NESI.DTO.ViewModels.Core.Enums;

namespace NESI.DTO.ViewModels.Page.TimeSheet.Expense
{
	public class AddExpense
	{
		[Required]
		public int Id_member { get; set; }
		[Required]
		public DateTime Date_purchased { get; set; }
		[Required]
		public DateTime Date_start { get; set; }
		[Required]
		public DateTime Date_end { get; set; }
		public string Rate { get; set; }
		[Required]
		public string Id_seller { get; set; }
		public string Wo_number { get; set; }
		public bool IsShop { get; set; }
		[Required]
		public double Amount { get; set; }
        [Required]
        public double Pre_Tax_Amount { get; set; }
        public Currency Currency { get; set; }
		[Required]
		public int Master_id { get; set; }
		[Required]
		public string Master_name { get; set; }
		public string Receipt_number { get; set; }
		[Required]
		public bool Has_file { get; set; }
		public string File_name { get; set; }
		[Required]
		public string Item_text { get; set; }
		public string File_path { get; set; }
		public double Distance { get; set; }
		public string Distance_unit { get; set; }
		public string Attendees { get; set; }

	}
}