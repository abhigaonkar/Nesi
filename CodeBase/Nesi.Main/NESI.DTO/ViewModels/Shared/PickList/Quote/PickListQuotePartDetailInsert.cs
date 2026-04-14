using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Shared.PickList.Quote
{
	public class PickListQuotePartDetailInsert
	{
		[Required]
		public int section_id { get; set; }
		public string description { get; set; }
		[Required]
		public double cost { get; set; }
		[Required]
		public double sell { get; set; }
		[Required]
		public double extd { get; set; }
		[Required]
		public double extd2 { get; set; }
		[Required]
		public double discount { get; set; }
		public string master_id { get; set; }
		[Required]
		public double qty { get; set; }
	}
}