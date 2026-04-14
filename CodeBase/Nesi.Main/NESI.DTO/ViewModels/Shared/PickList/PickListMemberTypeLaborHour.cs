namespace NESI.DTO.ViewModels.Shared.PickList
{
	public class PickListMemberTypeLaborHour
	{
		public int master_id { get; set; }
		public int business_unit_id { get; set; }
		public int membertype_id { get; set; }
		public string membertype_name { get; set; }
		public int paytype_id { get; set; }
		public string paytype_name { get; set; }
		public double chargeout { get; set; }
		public double? qty { get; set; }
		public double multiplier { get; set; }
		public bool is_checked { get; set; }
	}
}