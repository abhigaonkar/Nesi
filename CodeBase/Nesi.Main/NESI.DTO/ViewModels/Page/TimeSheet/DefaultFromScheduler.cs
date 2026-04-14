using System;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class DefaultFromScheduler
	{
		public int UserId { get; set; }
		public DateTime Date { get; set; }
		public int Business_Unit_Id { get; set; }
		public int Id { get; set; }
		public int Customer_Id { get; set; }
		public int Hours { get; set; }
		public int EntryType { get; set; }
		public int PayType { get; set; }
		public string Comment { get; set; }
	}
}