using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Customers
{
	[ModelDefination("CustomerArEmailsGrid")]
	public class CustomerArEmailsGrid : ModelBase<CustomerArEmailsGrid>
	{
		public long emaillog_id { get; set; }
		public DateTime emaillog_timestamp { get; set; }
		public string emaillog_from { get; set; }
		public string emaillog_to { get; set; }
		public string emaillog_body { get; set; }
		public string emaillog_subject { get; set; }
	}
}