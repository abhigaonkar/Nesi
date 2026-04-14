using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Attributes;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	public class VendorPhoneNumber : VendorBase
	{
		public int phone_numbers_id { get; set; }
		public string number { get; set; }
		public int phone_numbers_table_id { get; set; }
		public bool is_default { get; set; }
		public bool is_active { get; set; }
		public string comm_type { get; set; }
		public string type { get; set; }
	}
}
