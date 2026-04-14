// ReSharper disable InconsistentNaming

using System;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class TimeSheetDay
	{
		public int MemberTime_ID { get; set; }
		public int business_unit_id { get; set; }
		public DateTime Date { get; set; }
		public string workorder_id { get; set; }
		public string MemberTime_Cust_No { get; set; }
		public string customer_name { get; set; }
		public int Hours { get; set; }
		public int Miles { get; set; }
		public int MemberTime_WoComment_ID { get; set; }
		public int MemberTime_PayTypeHours_ID { get; set; }
		public DateTime Created_Date { get; set; }
		public string HourType { get; set; }
		public string MTBy { get; set; }
		public string MemberTime_Premium { get; set; }
		public string Comments { get; set; }



	}
}