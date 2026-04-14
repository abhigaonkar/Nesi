using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Customers
{
	public class CustomerAddress : CustomerBase
	{
		public string table { get; set; }
		public string description { get; set; }
		[Required]
		public string address_line_1 { get; set; }
		public string address_line_2 { get; set; }
		public string address_line_3 { get; set; }
		public string address_line_4 { get; set; }
		[Required]
		public string address_city { get; set; }
		[Required]
		public string address_prov { get; set; }
		[Required]
		public string address_postalcode { get; set; }
		[Required]
		public string address_country { get; set; }
		[Required]
		[MaxLength(3)]
		public string phone_area { get; set; }
		[Required]
		[MaxLength(3), MinLength(3)]
		public string phone_prefix { get; set; }
		[Required]
		[MaxLength(4), MinLength(4)]
		public string phone_suffix { get; set; }
		[MaxLength(5)]
		public string phone_ext { get; set; }
		[MaxLength(3)]
		public string fax_area { get; set; }
		[MaxLength(3)]
		public string fax_prefix { get; set; }
		[MaxLength(4)]
		public string fax_suffix { get; set; }
		[MaxLength(50)]
		public string gps_coordinates { get; set; }
		[MaxLength(200)]
		public string website { get; set; }
		[MaxLength(200)]
		public string facebook { get; set; }
		[MaxLength(200)]
		public string twitter { get; set; }
		[MaxLength(200)]
		public string linkedin { get; set; }
        [Required]
        public bool Active { get; set; }
	}
}