using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	public class VendorEdit :VendorBase
	{
		[Required]
		public string name { get; set; }
		[Required]
		public int business_unit_id { get; set; }
		public bool vendor_hold { get; set; }
		public bool is_partner { get; set; }
	}
}