using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	[ModelDefination("VendorPoGrid")]
	public class VendorPoGrid : ModelBase<VendorPoGrid>
	{
		public int poprog_id { get; set; }
		public DateTime? poprog_cutdate { get; set; }
		public DateTime? poprog_closedDate { get; set; }
		public string poprog_bvpo { get; set; }
		public string poprog_order_description { get; set; }
		public string status_type { get; set; }
		public int business_unit_id { get; set; }
		public string business_unit_name { get; set; }
	}
}