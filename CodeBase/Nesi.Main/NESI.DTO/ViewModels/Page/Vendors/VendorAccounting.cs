using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	public class VendorAccounting : VendorBase
	{
		[Required]
		public int vendor_term_id { get; set; }
		public bool po_exempt { get; set; }
		public bool cprs { get; set; }
		[Required]
		public string vendor_idtype { get; set; }
		public string vendor_idnumber { get; set; }
		[Required]
		public int vendor_credit_type { get; set; }
		public int vendor_credit_limit { get; set; }
		public string vendor_account { get; set; }
		public string vendor_buyer { get; set; }
		public int tax1 { get; set; }
		public int tax2 { get; set; }
		public int tax3 { get; set; }
		public int tax4 { get; set; }
	}
}