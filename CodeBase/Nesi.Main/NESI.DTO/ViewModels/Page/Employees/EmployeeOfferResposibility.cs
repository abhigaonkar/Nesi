namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeOfferResposibility : EmployeeOfferBase
	{
		public string name { get; set; }
		public string description { get; set; }
		public string core_responsibility { get; set; }
		public string daily { get; set; }
		public string weekly { get; set; }
		public string monthly { get; set; }
		public string quarterly { get; set; }
		public string annually { get; set; }
		public string asneeded { get; set; }
		public int? mt_cr_priority { get; set; }
		public string is_default { get; set; }
		public bool is_checked { get; set; }
		public int? memberoffer_cr_id { get; set; }
	}
}