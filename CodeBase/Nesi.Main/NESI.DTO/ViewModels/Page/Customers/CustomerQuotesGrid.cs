using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Customers
{
	[ModelDefination("CustomerQuotesGrid")]
	public class CustomerQuotesGrid : ModelBase<CustomerQuotesGrid>
	{
		public int id { get; set; }
		public int quote_id { get; set; }
		public int revision { get; set; }
		public string quoted_by { get; set; }
		public DateTime open_date { get; set; }
		public string job_description { get; set; }
		public string status { get; set; }
	}
}