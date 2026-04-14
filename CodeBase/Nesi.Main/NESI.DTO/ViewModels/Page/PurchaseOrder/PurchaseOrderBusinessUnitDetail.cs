using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.PurchaseOrder
{
	[ModelDefination("PurchaseOrderBusinessUnitDetail")]
	public class PurchaseOrderBusinessUnitDetail : ModelBase<PurchaseOrderBusinessUnitDetail>
	{
		public int poprog_id { get; set; }
		public string branch { get; set; }
		public string po { get; set; }
		public string vendor { get; set; }
		public DateTime cut { get; set; }
		public DateTime issued { get; set; }
		public DateTime required { get; set; }
		public string description { get; set; }
		public string status { get; set; }
		public string shipping { get; set; }
		public double cost { get; set; }
		public string purchaser { get; set; }
		public string vendor_id { get; set; }
		public string nesi_cut_po { get; set; }
		public int n_lines { get; set; }
		public string apnotes { get; set; }
		public int poprog_apstatus { get; set; }
		public int business_unit_id { get; set; }
		public double rec_cost { get; set; }
		}
}