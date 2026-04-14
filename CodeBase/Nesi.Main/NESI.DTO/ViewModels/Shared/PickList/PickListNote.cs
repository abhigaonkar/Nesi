using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Shared.PickList
{
	public class PickListNote
	{
		[Required]
		public int id { get; set; }
		[Required]
		public string note { get; set; }
		[Required]
		public bool is_new { get; set; }
	}
}