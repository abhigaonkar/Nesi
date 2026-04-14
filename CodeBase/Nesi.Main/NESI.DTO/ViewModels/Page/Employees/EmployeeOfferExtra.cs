namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeOfferExtra :EmployeeOfferBase
	{
		public bool gets_phone { get; set; }
		public bool gets_laptop { get; set; }
		public bool gets_vehicle { get; set; }
		public bool gets_neemail { get; set; }
		public bool gets_barcodescanner { get; set; }
		public bool gets_businesscards { get; set; }
		public bool gets_directdeposit { get; set; }
		public bool gets_phoneext { get; set; }
		public string notes { get; set; }
	}
}