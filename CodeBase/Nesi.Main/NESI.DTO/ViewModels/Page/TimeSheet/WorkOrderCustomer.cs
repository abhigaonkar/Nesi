// ReSharper disable InconsistentNaming
namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class WorkOrderCustomer
	{
		public int woprog_customer_id { get; set; }
		public int customer_id { get; set; }
		public int customer_number { get; set; }
		public string customer_name { get; set; }
		public int woprog_id { get; set; }
		public int parent_woprog_id { get; set; }

		public int Value => customer_id;
		public string Label => customer_name;
	}
}