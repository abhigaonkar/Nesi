namespace NESI.DTO.ViewModels.Page.WorkOrder
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
		public string just_scanned { get; set; }
		public string businessUnit_name { get; set; }
		public CountValue total { get; set; }
		public CountValue open { get; set; }
		public CountValue on_hold { get; set; }
		public CountValue being_processed { get; set; }
		public CountValue init_prep { get; set; }
		public CountValue open_vendor_pos { get; set; }
		public CountValue open_pos { get; set; }
		public CountValue rework { get; set; }
		public CountValue questions { get; set; }
		public CountValue pm_approval { get; set; }
		public CountValue bm_approval { get; set; }
		public CountValue to_be_invoiced { get; set; }
		public CountValue waiting_cut_po { get; set; }

	}
}