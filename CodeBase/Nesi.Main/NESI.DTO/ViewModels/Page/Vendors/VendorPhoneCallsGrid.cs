using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	[ModelDefination("VendorPhoneCallsGrid")]
	public class VendorPhoneCallsGrid : ModelBase<VendorPhoneCallsGrid>
	{
		public long phone_log_id { get; set; }
		public DateTime phone_log_date { get; set; }
		public string phone_log_from_number { get; set; }
		public string phone_log_to_number { get; set; }
		public string phone_log_from_name { get; set; }
		public string phone_log_to_name { get; set; }
		public int phone_log_duration { get; set; }
		public string notes { get; set; }
	}
}