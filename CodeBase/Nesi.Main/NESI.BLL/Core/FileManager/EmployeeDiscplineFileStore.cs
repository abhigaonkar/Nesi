using System;
using System.IO;
using System.Web;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Core.FileManager
{
	public class EmployeeDiscplineFileStore : FileStore
	{
		public int disc_id { get; set; }
		public EmployeeDiscplineFileStore(Employee.Employee user, int disc_id) : base(user)
		{
			this.disc_id = disc_id;
		}

		public DataExtra Save(string filename)
		{
			filename = Path.Combine(new BLL.Core.FileManager.TempFile(CurrentUser).BasePath, CurrentUser.Guid, filename);
			var f = new file_store.fileObj
			{
				page_id = 127,
				folder_id = 6,
				sub_folder_id = disc_id,
			};
			return base.Save(f, filename);
		}
	}
}