using NESI.DTO.ViewModels.Core;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteStrategyCheckItem
	{
		public bool isStageMeeting { get; set; }
		public string name { get; set; }
		public string background_color { get; set; }
		public string button_label { get; set; }
		public string button_icon { get; set; }
		public bool button_enabled { get; set; }
		public string completed_type { get; set; }
		public bool is_checked { get; set; }
		public string milestone { get; set; }
		public string date { get; set; }
		public bool date_enabled { get; set; }
		public LabelValueInt[] options { get; set; }
		public bool assign_to_enabled { get; set; }
		public int assign_to { get; set; }
		public string notes { get; set; }
		public string dialog_type { get; set; }
		public string html { get; set; }
	}
}