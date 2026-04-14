using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Customers
{
	public class CustomerBase
	{
		[Required]
		public int address_id { get; set; }
		[Required]
		public int customer_id { get; set; }
		
	}
}