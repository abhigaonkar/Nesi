using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Customers
{
	public class CustomerAccounting : CustomerBase
	{
		[Required] public string gl_account { get; set; }
		[Required] public int tax_1 { get; set; }
		[Required] public int tax_2 { get; set; }
		[Required] public int tax_3 { get; set; }
		[Required] public int tax_4 { get; set; }

		public string taxex_1 { get; set; }
		public string taxex_2 { get; set; }
		public string taxex_3 { get; set; }
		public string taxex_4 { get; set; }

	}
}
