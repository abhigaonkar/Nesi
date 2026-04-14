using System;
using NESI.Common;
using NESI.DTO.ViewModels.Page.Customers;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	[ModelDefination("VendorEmailsGrid")]
	public class VendorEmailsGrid : ModelBase<VendorEmailsGrid>
	{
		public long emaillog_id { get; set; }
		public DateTime date { get; set; }
		public string from_address { get; set; }
		public string to_address { get; set; }
		public string subject { get; set; }
	}
}