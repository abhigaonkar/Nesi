using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	public class VendorAddress : VendorBase
	{
		[Required]
		public string addr1 { get; set; }
		public string addr2 { get; set; }
		public string addr3 { get; set; }
		public string addr4 { get; set; }
		public string city { get; set; }
		public string postal { get; set; }
		[Required]
		public string prov { get; set; }
		[Required]
		public string country { get; set; }
		public string phonearea { get; set; }
		public string phonefirst { get; set; }
		public string phonelast { get; set; }
		public string phoneext { get; set; }
		public string faxarea { get; set; }
		public string faxfirst { get; set; }
		public string faxlast { get; set; }
		public string email { get; set; }
		public string web { get; set; }
	}
}