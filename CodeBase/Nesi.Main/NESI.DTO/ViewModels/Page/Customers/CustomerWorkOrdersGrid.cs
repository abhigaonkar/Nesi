using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Customers
{
	[ModelDefination("CustomerWorkOrdersGrid")]
	public class CustomerWorkOrdersGrid : ModelBase<CustomerWorkOrdersGrid>
	{
		public int woprog_id { get; set; }
		public DateTime cut_dt { get; set; }
		public DateTime close_dt { get; set; }
		public string wo_number { get; set; }
		public string contact_name { get; set; }
		public string wo_description { get; set; }
		public string status { get; set; }
		public string quote_id { get; set; }
		public string rev { get; set; }
		public int business_unit_id { get; set; }
		public string woprog_bvwo { get; set; }
		public string addy { get; set; }
	}
}