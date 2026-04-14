namespace NESI.DTO.ViewModels.Page.Shared
{
	public class OrderCompanySummary
	{
		public int value { get; set; }
		public string name { get; set; }
		public string label { get; set; }
		public string tax_entity_name { get; set; }
		public int tax_entity_id { get; set; }
		public string dsn { get; set; }
		public string path { get; set; }
		public int total { get; set; }
		public double dollars { get; set; }
		public int orderby { get; set; }
	}
}