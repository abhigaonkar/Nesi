// ReSharper disable All

using NESI.DTO.ViewModels.Core;

namespace NESI.DTO.ViewModels.Shared.PickList.Quote
{
	public class PickListQuoteCopyMoveLine
	{
		public int quote_id { get; set; }
		public int revision { get; set; }
		public PickListItem[] items { get; set; }
		public LabelValueInt[] destination { get; set; }
		public string type { get; set; }
	}
}