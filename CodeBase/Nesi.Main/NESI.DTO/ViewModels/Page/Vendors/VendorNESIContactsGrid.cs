using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	[ModelDefination("VendorNESIContactsGrid")]
	public class VendorNESIContactsGrid : ModelBase<VendorNESIContactsGrid>
	{
		public int vendor_id { get; set; }
		public int? contact_id { get; set; }
		public string contact_name { get; set; }
		public string contact_cellphone { get; set; }
		public string contact_directline { get; set; }
		public string contact_extension { get; set; }
		public string contact_email { get; set; }
		public string contact_status { get; set; }
		public string contact_password { get; set; }
		public bool login_enable { get; set; }
	}
}