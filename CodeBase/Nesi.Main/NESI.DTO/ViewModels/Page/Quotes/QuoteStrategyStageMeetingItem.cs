using System;
using NESI.DTO.ViewModels.Core;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteStrategyStageMeetingItem : QuoteStrategyCheckItem
	{
		public LabelValueInt[] check_list { get; set; }
		public bool[] check_value { get; set; }
		public bool[] check_enabled { get; set; }
		public bool[] check_visible { get; set; }
		public string[] check_text { get; set; }
	}
}