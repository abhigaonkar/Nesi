namespace NESI.DTO.ViewModels.Page.TimeSheet.PEL
{
	public class PELSummary
	{
		public double total_days { get; set; }
		public double total_hours { get; set; }
		public double total_paid_days { get; set; }
		public double total_paid_hours { get; set; }
		public double total_unpaid_days { get; set; }
		public double total_unpaid_hours { get; set; }
		public double used_hours { get; set; }
		public double used_days { get; set; }
		public double used_paid_days { get; set; }
		public double used_paid_hours { get; set; }
		public double used_unpaid_days { get; set; }
		public double used_unpaid_hours { get; set; }
	}
}