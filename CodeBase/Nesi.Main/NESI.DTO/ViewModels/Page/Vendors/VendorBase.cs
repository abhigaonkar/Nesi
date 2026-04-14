using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	public class VendorBase
	{
		[Required]
		public int vendor_id { get; set; }
		
	}
}