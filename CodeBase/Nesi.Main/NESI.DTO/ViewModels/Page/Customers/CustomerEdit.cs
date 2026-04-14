namespace NESI.DTO.ViewModels.Page.Customers
{
	public class CustomerEdit : CustomerBase
	{
		public int project_manager_id { get; set; }
		public int account_manager_id { get; set; }
		public int status_id { get; set; }
		public bool is_partner { get; set; }
		public bool is_on_hold { get; set; }
		public string customer_name { get; set; }
		public string why_hold;
		public int reg_account_manager_id { get; set; }
		public int isr { get; set; }
		public int osr { get; set; }
	}
}