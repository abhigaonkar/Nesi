namespace NESI.DTO.ViewModels.Shared.PickList.Inventory
{
	public class InventoryPartDetail
	{
		public int master_id { get; set; }
		public string description { get; set; }
		public double sell_price { get; set; }
		public double cost_price_branch { get; set; }
		public string tag_id { get; set; }
		public string tag_name { get; set; }
		public string sold_as { get; set; }
		public double ttl_ms { get; set; }
		public string bv_part_number { get; set; }
		public int business_unit_id { get; set; }
		public bool is_qty { get; set; }
		public string vendor_code { get; set; }
		public double vendor_qty { get; set; }
		public double vendor_sell { get; set; }
		public string shown_vendor_date { get; set; }
		public double onhand_qty { get; set; }
		public bool show_sell { get; set; }
		public bool show_cost { get; set; }
		public bool has_pic { get; set; }
		public bool has_minmax { get; set; }
		public bool is_stocked { get; set; }
		public double int_onhand_qty { get; set; }
		public double ext_onhand_qty { get; set; }
		public double wo_usage { get; set; }
		public double po_usage { get; set; }
		public double min_qty { get; set; }
		public bool is_exclude { get; set; }
	}
}