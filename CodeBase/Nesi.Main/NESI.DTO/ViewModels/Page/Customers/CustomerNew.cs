using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Customers
{
	public class CustomerNew
	{
		[Required]
		public int business_unit { get; set; }
		[Required]
		public int project_manager_id { get; set; }
		[Required]
		public int account_manager_id { get; set; }
		[Required]
		public int reg_account_manager_id { get; set; }
		[Required]
		public int isr { get; set; }
		[Required]
		public int osr { get; set; }
		[Required]
		public int origin_id { get; set; }
		[Required]
		[MaxLength(60)]
		public string customer_name { get; set; }
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
		[MaxLength(3),MinLength(3)]
		public string phone_prefix { get; set; }
		[Required]
		[MaxLength(4),MinLength(4)]
		public string phone_suffix { get; set; }
		[MaxLength(5)]
		public string phone_ext { get; set; }
		[MaxLength(3)]
		public string fax_area { get; set; }
		[MaxLength(3)]
		public string fax_prefix { get; set; }
		[MaxLength(4)]
		public string fax_suffix { get; set; }
		[Required]
		[MaxLength(200)]
		public string ap_email { get; set; }
		[Required]
		public string gl_receivalbes { get; set; }
		[Required]
		public int tax_1 { get; set; }
		[Required]
		public int tax_2 { get; set; }
		[Required]
		public int tax_3 { get; set; }
		[Required]
		public int tax_4 { get; set; }

	}
}