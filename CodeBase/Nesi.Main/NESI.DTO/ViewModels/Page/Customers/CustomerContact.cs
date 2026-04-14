namespace NESI.DTO.ViewModels.Page.Customers
{
	public class CustomerContact: CustomerBase
	{
		public string contact_name { get; set; }
		public string name_first { get; set; }
		public string name_last { get; set; }
		public int contact_id { get; set; }
		public string contact_cellphone { get; set; }
		public string contact_title { get; set; }
		public string contact_directline { get; set; }
		public string contact_extension { get; set; }
		public string contact_email { get; set; }
		public string contact_password { get; set; }
		public string contact_status { get; set; }
		public int contact_status_id { get; set; }
		public string contact_status_label { get; set; }
		public string facebook { get; set; }
		public string twitter { get; set; }
		public string linkedin { get; set; }
		public bool login_enabled { get; set; }
		public bool stopsurveys { get; set; }
		public int wos { get; set; }
		public int quotes { get; set; }

	}
}