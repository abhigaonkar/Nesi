using System;

namespace NESI.DTO.ViewModels.Page.WorkOrder
{
	public class WorOrderBucketScanned
	{
		public int business_unit_id { get; set; }
		public string file_name { get; set; }
		public string created_time { get; set; }
		public double size { get; set; }
		public string last_access_time { get; set; }
	}
}