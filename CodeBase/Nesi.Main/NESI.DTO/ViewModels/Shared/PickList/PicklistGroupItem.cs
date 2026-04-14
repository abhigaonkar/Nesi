namespace NESI.DTO.ViewModels.Shared.PickList
{
	public class PicklistGroupItem
	{
		public int master_id { get; set; }
		public string description { get; set; }
		public double qty { get; set; }
		public bool is_checked { get; set; }
		public double cost { get; set; }
		public double sell { get; set; }
		public int cost_level { get; set; }
		public double onhand { get; set; }
		public double int_onhand_qty { get; set; }
		public double ext_onhand_qty { get; set; }
		public double extd { get; set; }
		public double extd2 { get; set; }
		public double discount { get; set; }
		public string section_id { get; set; }
		public string section_name { get; set; }
	}
}