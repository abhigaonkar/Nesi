using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Customers
{
	[ModelDefination("CustomerEmailsGrid")]
	public class CustomerEmailsGrid : ModelBase<CustomerEmailsGrid>
	{
		public long emaillog_id { get; set; }
		public DateTime date { get; set; }
		public string from_address { get; set; }
		public string to_address { get; set; }
		public string subject { get; set; }
	}
}