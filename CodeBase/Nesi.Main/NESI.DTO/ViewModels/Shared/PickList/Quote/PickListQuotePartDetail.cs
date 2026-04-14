namespace NESI.DTO.ViewModels.Shared.PickList.Quote
{
	public class PickListQuotePartDetail : PickListQuotePartDetailInsert
	{
		public double ext_onhand_qty { get; set; }
		public int revision { get; set; }
		public double sell_price { get; set; }
		public double onhand { get; set; }
		public string description_full { get; set; }
		public bool exclude_part { get; set; }
		public double cost_price { get; set; }
		public double used_cost_price { get; set; }
		public double used_sell_price { get; set; }
		public string tag_id { get; set; }
		public bool allowed_to_stock { get; set; }
		public int quote_id { get; set; }
		public int line_id { get; set; }
		public int cost_level { get; set; }
		public bool has_pic { get; set; }
		public bool has_minmax { get; set; }
		public bool is_stocked { get; set; }
		public double int_onhand_qty { get; set; }
		public double wo_usage { get; set; }
		public double po_usage { get; set; }
		public bool is_exclude { get; set; }
		public double ttl_ms { get; set; }
		public string last_purchased { get; set; }
	}
}