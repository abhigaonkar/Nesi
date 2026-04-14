using System;

namespace NESI.DTO.ViewModels.Core
{
	public class PayPeriod
	{
		public int PayPeriodId { get; set; }
		public string PayDate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Start_Date => StartDate.ToString("yyyy-MM-dd");
		public string End_Date => EndDate.ToString("yyyy-MM-dd");
	}
}