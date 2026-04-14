using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.WorkOrder;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.WorkOrder
{
	[RoutePrefix("api/Page/WorkOrder/Edit/WoComments")]
	public class WorkOrderWoCommentsController : WorkOrderControllerBase
	{

		[Route("{buid}/{str_wo_id}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetProfile(int buid, string str_wo_id)
		{
			var o = new BLL.Pages.WorkOrder.WorkorderWoComments(CurrentUser, buid, str_wo_id);
			if (!CurrentUser.IsVisibleBusinessUnitId(buid) || !o.can_access)

			{
				return NotFound();
			}
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("{buid}/{str_wo_id}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult SaveComment([FromBody] DataString model, int buid, string str_wo_id)
		{
			var o = new BLL.Pages.WorkOrder.WorkorderWoComments(CurrentUser, buid, str_wo_id);
			if (!CurrentUser.IsVisibleBusinessUnitId(buid) || !o.can_access)

			{
				return NotFound();
			}
			return Ok(o.AddComments(model.Data));
		}

		[HttpPatch]
		[Route("{buid}/{str_wo_id}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult SaveTimesheetComment([FromBody] WorkOrderWoComment model, int buid, string str_wo_id)
		{
			var o = new BLL.Pages.WorkOrder.WorkorderWoComments(CurrentUser, buid, str_wo_id);
			if (!CurrentUser.IsVisibleBusinessUnitId(buid) || !o.can_access)

			{
				return NotFound();
			}
			return Ok(o.SaveTimesheetComments(model));
		}
	}
}
