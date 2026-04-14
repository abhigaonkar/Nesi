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
	[RoutePrefix("api/Page/WorkOrder/RenameFile")]
	public class WorkOrderRenameFileController : WorkOrderControllerBase
	{


		[Route("{buid}/{woprog_id}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetProfile(int buid,  int woprog_id)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderRenameFile(CurrentUser, buid, woprog_id);
			return Ok(o.Profile());
		}

		//[HttpPost]
		//[Route("{buid}/{woprog_id}")]
		//public IHttpActionResult RenameFile([FromBody] DataInt model, int buid, int woprog_id)
		//{
		//	if (!CurrentUser.IsVisibleBusinessUnitId(buid))
		//	{
		//		return NotFound();
		//	}
		//	var o = new BLL.Pages.WorkOrder.WorkOrderRenameFile(CurrentUser, buid, woprog_id);
		//	return Ok(o.Rename(model.Data));
		//}

		[HttpDelete]
		[Route("{buid}/{woprog_id}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult DeleteFile(int buid, int woprog_id)
		{
			if (!CurrentUser.IsVisibleBusinessUnitId(buid))
			{
				return NotFound();
			}
			var o = new BLL.Pages.WorkOrder.WorkOrderRenameFile(CurrentUser, buid, woprog_id);
			return Ok(o.Delete());
		}

		//[HttpGet]
		//[Route("FilePath")]
		//public IHttpActionResult GetFilePath()
		//{
		//	var o = new BLL.Core.FileManager.TempFile(CurrentUser);
		//	return OkD(o.BasePath);
		//}

		//[HttpPost]
		//[Route("Uploaded/{buid}")]
		//public IHttpActionResult Uploaded([FromBody] DataFiles model, int buid)
		//{
		//	if (!CurrentUser.IsVisibleBusinessUnitId(buid))
		//	{
		//		return NotFound();
		//	}
		//	var o2 = new BLL.Core.FileManager.TempFile(CurrentUser);
		//	if (!(model.fullpath.StartsWith(o2.BasePath) && model.fullpath.Contains(CurrentUser.Guid)))
		//	{
		//		return NotFound();
		//	}
		//	var o = new BLL.Pages.WorkOrder.WorkOrderBuckets(CurrentUser, buid);
		//	return Ok(o.UploadFiles(model));
		//}
	}
}
