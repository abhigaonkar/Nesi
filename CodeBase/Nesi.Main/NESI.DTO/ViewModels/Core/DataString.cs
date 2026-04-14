using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Core
{
	public class DataString
	{
		[Required]
		public string Data;
		public string Data2;
        public string Data3;
        public string Data4;
        public string Data5;
    }

	public class DataStringArray
	{
		[Required]
		public string[] Data;
	}
}