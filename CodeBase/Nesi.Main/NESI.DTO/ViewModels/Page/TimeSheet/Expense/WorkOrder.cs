namespace NESI.DTO.ViewModels.Page.TimeSheet.Expense
{
	public class WorkOrder
	{
		public int Woprog_id { get; set; }
		public string Customer_name { get; set; }
		public string Woprog_bvwo { get; set; }
		public string Description { get; set; }
		public int Value => Woprog_id;
		public string Label => Description;
	}
}