using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.WorkOrder
{
	[ModelDefination("WorkOrderBusinessUnitDetail")]
	public class WorkOrderBusinessUnitDetail : ModelBase<WorkOrderBusinessUnitDetail>
	{
		public int woprog_id { get; set; }
		public int business_unit_id { get; set; }
		public int customer_id { get; set; }
		public string woprog_bvwo { get; set; }
		public string woprog_customername { get; set; }
		public string woprog_description { get; set; }
		public string woprog_custpo { get; set; }
		public string status { get; set; }
		public DateTime last_modified { get; set; }
		public string cutby { get; set; }
		public double woprog_stilltobebilled { get; set; }
		public DateTime woprog_cutdatetime { get; set; }
		public string quote_id { get; set; }
		public string pm { get; set; }
		public string woprog_invoiceno { get; set; }
		public string contact_Name { get; set; }
		public DateTime open_dt { get; set; }
		public string hold { get; set; }
		public double margin { get; set; }
		public string workorder_schedule_date { get; set; }
	}
}