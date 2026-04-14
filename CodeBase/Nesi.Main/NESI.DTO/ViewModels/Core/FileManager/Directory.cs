using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Core.FileManager
{
	public class Directory
	{
	
		[Required]
		public string Name { get; set; }
		[Required]
		public string FullName { get; set; }
		public List<Directory> SubDirectories { get; set; }
		public List<File> Files { get; set; }
	}
}