using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.WorkOrder
{
	[RoutePrefix("api/Page/WorkOrder/ScannedFile")]
	public class WorkOrderScannedFileController : WorkOrderControllerBase
	{


		[Route("{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetProfile(int buid, [FromUri] string filename)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderScannedFile(CurrentUser, buid, filename + ".pdf");
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult RenameFile([FromBody] DataInt model, int buid, [FromUri] string filename)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderScannedFile(CurrentUser, buid, filename + ".pdf");
			return Ok(o.Rename(model.Data));
		}

		[HttpDelete]
		[Route("{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult DeleteFile(int buid, [FromUri] string filename)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderScannedFile(CurrentUser, buid, filename + ".pdf");
			return Ok(o.Delete());
		}

		[HttpGet]
		[Route("FilePath")]
		public IHttpActionResult GetFilePath()
		{
			var o = new BLL.Core.FileManager.TempFile(CurrentUser);
			return OkD(o.BasePath);
		}

		[HttpPost]
		[Route("Uploaded/{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult Uploaded([FromBody] DataFiles model, int buid)
		{
			var o2 = new BLL.Core.FileManager.TempFile(CurrentUser);
			if (!(model.fullpath.StartsWith(o2.BasePath) && model.fullpath.Contains(CurrentUser.Guid)))
			{
				return NotFound();
			}
			var o = new BLL.Pages.WorkOrder.WorkOrderBuckets(CurrentUser, buid);
			return Ok(o.UploadFiles(model));
		}
	}
}
