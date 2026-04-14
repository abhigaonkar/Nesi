using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Customers
{
	[ModelDefination("CustomerInvoicingInstructionsGrid")]
	public class CustomerInvoicingInstructionsGrid : ModelBase<CustomerInvoicingInstructionsGrid>
	{
		public long id { get; set; }
		public DateTime date { get; set; }
		public string note { get; set; }
		public string by { get; set; }
		public string invoice { get; set; }
		public string wo { get; set; }
	}
}