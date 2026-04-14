using System.IO;

namespace NESI.BLL.Core.FileManager
{
	public class MessageAttachFile : NeFileBase
	{
		public override string BaseFolder => base.FileServer + $@"\TE\TE{CurrentUser.TaxEntityId}\message_attach_files\";
		public override string BasePath => Path.Combine(BaseFolder, $"{CurrentUser.Id}");


		public MessageAttachFile(Employee.Employee user): base(user)
		{
			base.Validate_folder_contents();
		}

	
	}
}