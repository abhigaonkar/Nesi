using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Core.FileManager
{
	public class MoveOrCopyFile
	{
		[Required]
		public string From { get; set; }
		[Required]
		public string To { get; set; }
	}
}