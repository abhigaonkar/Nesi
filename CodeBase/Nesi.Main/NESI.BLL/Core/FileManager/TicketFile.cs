using System.IO;

namespace NESI.BLL.Core.FileManager
{
	public class TicketFile : NeFileBase
	{

		public override string BaseFolder => base.FileServer + $@"\global";
		public override string BasePath => Path.Combine(BaseFolder, $@"ticket_attachments");

		public TicketFile(Employee.Employee user) : base(user)
		{
			base.Validate_folder_contents();
		}

		public string GetUserTempPath()
		{
			var path = Path.Combine(BasePath, base.CurrentUser.Id.ToString());

			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}
			return path;
		}
	}
}