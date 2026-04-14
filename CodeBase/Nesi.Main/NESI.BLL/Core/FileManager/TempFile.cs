using System.IO;

namespace NESI.BLL.Core.FileManager
{
	public class TempFile : NeFileBase
	{
		public override string BaseFolder => base.FileServer + $@"\TE\TE{CurrentUser.TaxEntityId}\Temp_Files\";
		public override string BasePath => Path.Combine(BaseFolder, $"{CurrentUser.Id}");


		public TempFile(Employee.Employee user): base(user)
		{
			base.Validate_folder_contents();
		}

	
	}
}