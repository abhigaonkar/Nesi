namespace NESI.DTO.ViewModels.Page.PurchaseOrder
{
	public class CountValue
	{
		public string name { get; set; }
		public int count { get; set; }
		public double value { get; set; }
	}

	public class BusinessUnitSummary
	{
		public int tax_entity_id { get; set; }
		public int businessUnit_id { get; set; }
		public string businessUnit_name { get; set; }
		public CountValue total { get; set; }
		public CountValue just_cut { get; set; }
		public CountValue waiting_approval { get; set; }
		public CountValue be_issued { get; set; }
		public CountValue waiting_confirmation { get; set; }
		public CountValue waiting_parts { get; set; }
		public CountValue questions { get; set; }
		public CountValue ap_problems { get; set; }

	}
}