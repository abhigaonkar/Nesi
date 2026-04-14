using NESI.DTO.ViewModels.Core;

namespace NESI.DTO.ViewModels.Page.TimeSheet.Expense
{
	public class ExpensePerDiemProfile
	{
		public double PerdiemRate { get; set; }
		public string BusinessUnitCountry { get; set; }
		public LabelValueInt[] ForEmployees { get; set; }
		public WorkOrder[] WorkOrders { get; set; }
		public bool allow_unlinked_timesheet { get; set; }
	}
}