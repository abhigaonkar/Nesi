using NESI.DTO.ViewModels.Core;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteEditCustomerAddressInfo
	{
		public LabelValueInt[] address_options { get; set; }
		public string address_addr1 { get; set; }
		public string address_addr2 { get; set; }
		public string address_addr3 { get; set; }
		public string address_addr4 { get; set; }
		public string address_city { get; set; }
		public string address_prov { get; set; }
		public string address_postal { get; set; }
		public string address_country { get; set; }
		public string address_phoneNumber { get; set; }
		public string address_faxNumber { get; set; }
        public bool address_is_active { get; set; }
	}

	public class QuoteEditCustomerContactInfo
	{
		public string contact_extension { get; set; }
		public string contact_cellPhone { get; set; }
		public string contact_email { get; set; }
	}
}