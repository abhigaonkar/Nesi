using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using NESI.DTO.ViewModels.Core.FileManager;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using File = NESI.DTO.ViewModels.Core.FileManager.File;

namespace NESI.WebAPI.Controllers.API.Core
{
	[RoutePrefix("api/Core/FileManager")]
	public class FileManagerController : EmployeeController
	{
		//[Route("Test/{dir}")]
		//public IHttpActionResult GetTestE(string dir)
		//{
		//	return Ok(new BLL.Core.FileManager.TestFile().GetDirectoryInfo(@"e:\" + dir));
		//}
		[HttpPost]
		[Route("Files")]
		[FileFitler()]
		public IHttpActionResult GetFiles([FromBody] URL dir)
		{
			// TODO: Check user has right to get file from this folder.

			var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
			var filesInfo = new DirectoryInfo(dir.Data);
			if (!o.CheckFile(dir.Data) || !filesInfo.Exists)
			{
				return NotFound();
			}
			return Ok(new BLL.Core.FileManager.TestFile().GetFiles(dir.Data));
		}

		[PageAuthorizationFilter(28)]
		[Route("Safety/TreeNode/{Id}")]
		public IHttpActionResult GetWorkSafetyFile(int Id)
		{

			return Ok(new BLL.Core.FileManager.SafetyFile(CurrentUser, Id).GetTreeNode());
		}

		[PageAuthorizationFilter(10)]
		[Route("Customer/TreeNode/{custId}/{addressId}")]
		public IHttpActionResult GetCustomerFile(int custId, int addressId)
		{

			return Ok(new BLL.Core.FileManager.CustomerFile(CurrentUser, custId, addressId).GetTreeNode());
		}

		[PageAuthorizationFilter(11)]
		[Route("Vendor/TreeNode/{vendorId}")]
		public IHttpActionResult GetVendorFile(int vendorId)
		{

			return Ok(new BLL.Core.FileManager.VendorFile(CurrentUser, vendorId).GetTreeNode());
		}


		[Route("WorkOrder/TreeNode/{woId}")]
		[WorkOrderFitler("woId")]
		public IHttpActionResult GetWorkOrderTreeNode(int woId)
		{
			//TODO: check is the user has access right for wo

			return Ok(new BLL.Core.FileManager.WorkOrderFile(woId).GetTreeNode());
		}

		[Route("CustomerAsset/TreeNode/{caId}")]
		public IHttpActionResult GetCustomerAssetTreeNode(int caId)
		{
			//TODO: check is the user has access right for wo
			var treeNodes = new BLL.Core.FileManager.CustomerAssetFile(caId, this.CurrentUser.TaxEntityId).GetTreeNode();
			return Ok(treeNodes);
		}

		[Route("Quote/TreeNode/{quoteId}/{revision}")]
		[QuoteFilter()]
		public IHttpActionResult GetQuoteTreeNode(int quoteId, int revision)
		{
			//TODO: check is the user has access right for Quote

			return Ok(new BLL.Core.FileManager.QuoteFile(quoteId, revision).GetTreeNode());
		}

		[Route("Applicant/TreeNode/{applicantid}")]
		public IHttpActionResult GetApplicantTreeNode(int applicantid)
		{
			//TODO: check is the user has access right for Applicant

			return Ok(new BLL.Core.FileManager.ApplicantFile(applicantid).GetTreeNode());
		}

		[Route("Employee/TreeNode/{memberid}")]
		public IHttpActionResult GetEmployeeTreeNode(int memberid)
		{
			//TODO: check is the user has access right for Employee

			return Ok(new BLL.Core.FileManager.MemberFile(memberid).GetTreeNode());
		}

		[HttpPost]
		[Route("Download")]
		public IHttpActionResult DownLoadFile([FromBody] File file)
		{
			//TODO: Check user can download the file?
			var path = file.FullName;

			if (!System.IO.File.Exists(path)) return NotFound();
			var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
			if (!o.CheckFile(file.FullName))
			{
				return NotFound();
			}
			var result = new HttpResponseMessage(HttpStatusCode.OK);
			var stream = new FileStream(path, FileMode.Open);
			result.Content = new StreamContent(stream);
			result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
			result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = file.Name };
			return ResponseMessage(result);
		}


		[HttpPost]
		[Route("File/Upload")]
		public IHttpActionResult UpLoadFile(string savedUrl)
		{
			var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
			if (!o.CheckFile(savedUrl))
			{
				return NotFound();
			}
			try
			{
				var httpRequest = HttpContext.Current.Request;

				if (httpRequest.Files.Count < 1)
				{
					return BadRequest();
				}
				var path = HttpUtility.UrlDecode(httpRequest.QueryString.ToString().Substring("savedUrl=".Length));

				// TODO: Check user has right to save file to this folder.



				for (var i = 0; i < httpRequest.Files.Count; i++)
				{
					var postedFile = httpRequest.Files[i];
					// if (postedFile == null) return BadRequest();
					if (!System.IO.Directory.Exists(path))
					{
						System.IO.Directory.CreateDirectory(path);
					}
					var filePath = System.IO.Path.Combine(path, postedFile.FileName);
					if (!System.IO.File.Exists(filePath))
					{
						postedFile.SaveAs(filePath);
					}

				}
				return OkD("File has been uploaded successfully.");

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return OkD("Uploading file fail: " + e.Message);
			}

		}

		[HttpPost]
		[Route("File/Delete")]
		public IHttpActionResult DeleteFiles([FromBody] DTO.ViewModels.Core.FileManager.File[] files)
		{
			// TODO: Check user has right to delete file.

			try
			{
				foreach (var file in files)
				{
					var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
					if (o.CheckFile(file.FullName))
					{
						System.IO.File.Delete(file.FullName);
					}
				}
				return OkD("Files have been deleted successfully.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return BadRequest();
			}

		}

		[HttpPost]
		[Route("File/Rename")]
		public IHttpActionResult RenameFile([FromBody] DTO.ViewModels.Core.FileManager.File file)
		{
			// TODO: Check user has right to rename file.

			try
			{
				var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
				if (!o.CheckFile(file.FullName))
				{
					return NotFound();
				}
				var path = file.FullName.Substring(0, file.FullName.LastIndexOf('\\'));
				var newPath = System.IO.Path.Combine(path, file.Name);
				System.IO.File.Move(file.FullName, newPath);
				return OkD("Files have been renamed successfully.");

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return BadRequest();
			}
		}
		[HttpPost]
		[Route("File/Move")]
		public IHttpActionResult MoveFile([FromBody] DTO.ViewModels.Core.FileManager.MoveOrCopyFile[] files)
		{
			// TODO: Check user has right to move file.

			try
			{

				foreach (var file in files)
				{
					var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
					if (o.CheckFile(file.From))
					{
						System.IO.File.Move(file.From, file.To);
					}

				}
				return OkD("Files were moved successfully.");

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return BadRequest();
			}
		}
		[HttpPost]
		[Route("File/Copy")]
		public IHttpActionResult CopyFile([FromBody] DTO.ViewModels.Core.FileManager.MoveOrCopyFile[] files)
		{
			// TODO: Check user has right to copy file.

			try
			{
				foreach (var file in files)
				{
					var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
					if (o.CheckFile(file.From))
					{
						System.IO.File.Copy(file.From, file.To);
					}

				}
				return OkD("Files were moved successfully.");

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return BadRequest();
			}
		}
		[HttpPost]
		[Route("Directory/List")]
		public IHttpActionResult ListDirectory([FromBody] DTO.ViewModels.Core.FileManager.Directory dir)
		{
			// TODO: Check user has right to list directory.
			var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
			if (!o.CheckFile(dir.FullName))
			{
				return NotFound();
			}
			try
			{
				// System.IO.Directory.CreateDirectory(dir.FullName);
				return Ok(new BLL.Core.FileManager.TestFile().GetDirectoryTreeNode(dir.FullName));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return BadRequest();
			}
		}
		[HttpPost]
		[Route("Directory/Create")]
		public IHttpActionResult CreateDirectory([FromBody] DTO.ViewModels.Core.FileManager.Directory dir)
		{
			// TODO: Check user has right to create directory.

			var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
			if (!o.CheckFile(dir.FullName))
			{
				return NotFound();
			}
			try
			{
				System.IO.Directory.CreateDirectory(dir.FullName);
				return OkD("Directory has been created successfully.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return BadRequest();
			}
		}
		[HttpPost]
		[Route("Directory/Delete")]
		public IHttpActionResult DeleteDirectory([FromBody] DTO.ViewModels.Core.FileManager.Directory dir)
		{
			// TODO: Check user has right to delete directory.
			var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
			if (!o.CheckFile(dir.FullName))
			{
				return NotFound();
			}
			try
			{
				System.IO.Directory.Delete(dir.FullName, true);
				return OkD("Directory has been deleted successfully.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return BadRequest();
			}
		}
		[HttpPost]
		[Route("Directory/Rename")]
		public IHttpActionResult RenameDirectory([FromBody] DTO.ViewModels.Core.FileManager.Directory[] dirs)
		{
			// TODO: Check user has right to rename directory.

			var o = new BLL.Core.Privileges.NeCheckPrivilege(CurrentUser);
			if (!o.CheckFile(dirs[0].FullName))
			{
				return NotFound();
			}
			try
			{
				System.IO.Directory.Move(dirs[0].FullName, dirs[1].FullName);
				return OkD("Directory has been deleted successfully.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return BadRequest();
			}
		}
	}
}
