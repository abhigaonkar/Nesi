using System.Collections.Generic;

namespace NESI.DTO.ViewModels.Page.WorkOrder
{
	public class WorkOrderMainHeader : WorkOrderBase
	{
		public string Description { get; set; }
		public double woprog_StillToBeBilled { get; set; }
		public double taxes { get; set; }
		public double taxes_adj { get; set; }
		public int prev_woprog_id { get; set; }
		public int next_woprog_id { get; set; }
		public double woprog_grossmargin { get; set; }
		public double margin_whole { get; set; }
		public string day_since_scan { get; set; }
		public string woprog_ERID { get; set; }
		public bool comment_visible { get; set; }
		public long ts_ticks { get; set; }
		public string[] cant_delete_reason { get; set; }
		public int woprog_creditcard_payment { get; set; }
	}

	public class WorkOrderButtonStyle
	{
		public string id { get; set; }
		public bool visible { get; set; }
		public bool enabled { get; set; }
		public string tooltip { get; set; }
		public string err_msg { get; set; }
		public string icon { get; set; }
		public string label { get; set; }
		public int order { get; set; }
		public string parent { get; set; }
	}
}