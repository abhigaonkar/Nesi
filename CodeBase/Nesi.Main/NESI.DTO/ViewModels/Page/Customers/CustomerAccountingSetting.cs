using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Customers
{
	public class CustomerAccountingSetting : CustomerBase
	{
		public string sell_level { get; set; }
		public int credit_type { get; set; }
		public bool tax_prompt { get; set; }
		public double credit_limit { get; set; }
		public int customer_creditdays { get; set; }
		public string statement_type { get; set; }
		public string invoice_type { get; set; }
		public bool apply_finance_charges { get; set; }
		public bool customer_autostatements { get; set; }
		public string customer_autostatement_address { get; set; }
		public string customer_autostatement_ccaddress { get; set; }
		public string customer_invoice_address { get; set; }
		public string customer_invoice_ccaddress { get; set; }
		public int? default_invoicetype { get; set; }
		public bool customer_auto_invoice { get; set; }
		public bool requires_wo_copy { get; set; }
		public string overallmargin { get; set; }
        public int? customer_term_id { get; set; }
	}
}
