using System;
using System.IO;
using System.Web;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Core.FileManager
{
	public class EmployeeSignBackFileStore : FileStore
	{
		public int offer_id { get; set; }
		public EmployeeSignBackFileStore(Employee.Employee user, int offerid) : base(user)
		{
			this.offer_id = offerid;
		}

		public DataExtra Save(string filename)
		{
			filename = Path.Combine(new BLL.Core.FileManager.TempFile(CurrentUser).BasePath, CurrentUser.Guid, filename);
			var f = new file_store.fileObj
			{
				page_id = 127,
				folder_id = 3,
				sub_folder_id = offer_id,
			};
			return base.Save(f, filename);
		}
	}
}