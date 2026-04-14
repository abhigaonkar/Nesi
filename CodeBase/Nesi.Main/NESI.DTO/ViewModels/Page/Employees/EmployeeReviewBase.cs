namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeReviewBase
	{
		public int id { get; set; }
		public int review_id => id;
		public int memberid { get; set; }
	}
}