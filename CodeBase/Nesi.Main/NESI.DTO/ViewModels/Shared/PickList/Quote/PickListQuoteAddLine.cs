namespace NESI.DTO.ViewModels.Shared.PickList.Quote
{
	public class PickListQuoteAddLine
	{
		public string workorder_text { get; set; }
		public string sectiontype { get; set; }
		public int revision { get; set; }
		public int qty { get; set; }
		public string partNo { get; set; }
		public int section_id { get; set; }
		public int quote_id { get; set; }
		public int workorder_id { get; set; }
		public string description { get; set; }
		public string charge_out_type_name { get; set; }
		public string membertype_name { get; set; }
		public int kittedpart_id { get; set; }
		public string kittedpart_name { get; set; }
		public string cost { get; set; }
		public string sell { get; set; }
		public string extd { get; set; }
		public string extd2 { get; set; }
		public int billtype { get; set; }
		public double discont { get; set; }
	}
}