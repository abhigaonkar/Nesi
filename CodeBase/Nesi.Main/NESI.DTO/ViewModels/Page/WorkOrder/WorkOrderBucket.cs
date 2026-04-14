namespace NESI.DTO.ViewModels.Page.WorkOrder
{
	public class WorkOrderBucket
	{
		public int woprog_id { get; set; }
		public string ddl_name { get; set; }
		public int woprog_associate_woprog_id { get; set; }
		public string pm_name { get; set; }
		public int pm_id { get; set; }
		public string woprog_status { get; set; }
		public string woprog_bvwo { get; set; }
		public string woprog_custpo { get; set; }
		public string woprog_customername { get; set; }
		public string woprog_description { get; set; }
		public double woprog_stilltobebilled { get; set; }
		public double woprog_totaltandm { get; set; }
		public bool woprog_hold { get; set; }
		public bool woprog_creditcard_payment { get; set; }
		public string woprog_quoteid { get; set; }
		public int days_since { get; set; }
		public int parent_woprog_id { get; set; }
		public int business_unit_id { get; set; }
		public string last_updated_by { get; set; }
		public string last_updated_dt { get; set; }
		public WorkOrderPo[] po_list { get; set; }
	}

	public class WorkOrderPo
	{
		public int id { get; set; }
		public string bvpo { get; set; }
		public string vendor_name { get; set; }
	}
}