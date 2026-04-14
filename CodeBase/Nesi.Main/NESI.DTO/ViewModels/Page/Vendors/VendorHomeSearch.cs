using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	[ModelDefination("VendorHomeSearch")]
	public class VendorHomeSearch : ModelBase<VendorHomeSearch>
	{
		public int vendor_id { get; set; }
		public string vendor_number { get; set; }
		public string vendor_name { get; set; }
		public string phone { get; set; }
		public int business_unit_id { get; set; }
		public string business_unit_name { get; set; }
		public string vendor_address { get; set; }
		public string website { get; set; }
	}

}