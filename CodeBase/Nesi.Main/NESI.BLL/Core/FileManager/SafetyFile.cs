using System.IO;

namespace NESI.BLL.Core.FileManager
{
	public class SafetyFile : NeFileBase
	{
		public override string BaseFolder=> base.FileServer + $@"\TE\TE{CurrentUser.TaxEntityId}\safety_files";
		public override string BasePath => Path.Combine(BaseFolder, $@"SA{Id}");

		public int Id { get; set; }

		public SafetyFile(Employee.Employee user, int id) : base(user)
		{
			this.Id = id;
			base.Validate_folder_contents();
		}

	}
}