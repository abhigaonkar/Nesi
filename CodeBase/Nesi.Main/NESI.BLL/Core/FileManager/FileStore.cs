using System.IO;
using System.Web;
using NESI.BLL.Base;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Core.FileManager
{
	public class FileStore : BLLBase
	{
		public FileStore(Employee.Employee user) : base(user)
		{

		}

		public virtual DataExtra Delete(int fileid)
		{
			bllToolbox.doSQL_void(@"Delete from filestore.files where id = @v0 limit 1", fileid);
			return new DataExtra("File has been deleted successfully.", null);
		}

		public virtual DataExtra Save(file_store.fileObj f, string filename)
		{
			bllToolbox.doSQL_void(@"Delete from filestore.files where sub_folder_id = @p0 and page_id = @p1 and folder_id = @p2",
				f.sub_folder_id, f.page_id, f.folder_id);
			var stream = new FileStream(filename, FileMode.Open);

			var rawdata = new byte[stream.Length];
			stream.Read(rawdata, 0, (int)stream.Length);
			stream.Close();

			f.ext = System.IO.Path.GetExtension(filename);
			f.name = System.IO.Path.GetFileNameWithoutExtension(filename);
			switch (f.ext)
			{
				case "xls":
					f.mime = "application/vnd.ms-excel";
					break;
				case "xlsx":
					f.mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
					break;
				case "doc":
					f.mime = "application/vnd.ms-word";
					break;
				case "rtf":
					f.mime = "application/vnd.ms-word";
					break;
				case "txt":
					f.mime = "application/vnd.ms-word";
					break;
				case "docx":
					f.mime = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
					break;
				case "pdf":
					f.mime = "application/pdf";
					break;
				case "bmp":
					f.mime = "image/bmp";
					break;
				case "gif":
					f.mime = "image/gif";
					break;
				case "jpg":
				case "jpeg":
					f.mime = "image/jpeg";
					break;
				case "png":
					f.mime = "image/png";
					break;
				case "f4v":
				case "flv":
					f.mime = "video/x-flv";
					break;
				default:
					f.mime = "application/octet-stream";
					break;
			}
			f.content = rawdata;
			f.save();
			return new DataExtra()
			{
				Data = "Uploaded file successfully.",
				Extra = f.id
			};
		}
	}
}