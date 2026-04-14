using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace NESI.DTO.ViewModels.Shared.PickList.Quote
{
	public class PickListQuotePartDetailEdit
	{
		[Required]
		public int quote_id { get; set; }
		[Required]
		public int revision { get; set; }
		[Required]
		public int id { get; set; }
		[Required]
		public double cost { get; set; }
		[Required]
		public double extd { get; set; }
		[Required]
		public double extd2 { get; set; }
		public string master_id { get; set; }
		[Required]
		public double qty { get; set; }
		[Required]
		public string label { get; set; }
	}
}