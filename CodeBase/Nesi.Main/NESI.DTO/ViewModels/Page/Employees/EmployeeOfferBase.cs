namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeOfferBase
	{
		public int id { get; set; }
		public int offer_id => id;
		public int memberid { get; set; }
		public int applicantid { get; set; }
		public int business_unit_id { get; set; }
		public int membertypeid { get; set; }
		public bool isapplicant { get; set; }

	}
}