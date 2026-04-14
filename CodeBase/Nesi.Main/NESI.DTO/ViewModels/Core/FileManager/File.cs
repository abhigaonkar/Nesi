using System;
using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Core.FileManager
{
	public class File
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string FullName { get; set; }
		public long Size { get; set; }
		public DateTime LastModified { get; set; }
		public string MimeType { get; set; }
		public string Extension { get; set; }
	}
}