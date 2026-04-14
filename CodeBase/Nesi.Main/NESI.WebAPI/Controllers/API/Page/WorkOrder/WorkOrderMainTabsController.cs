using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.WorkOrder
{
	[RoutePrefix("api/Page/WorkOrder/Edit/Maintabs")]
	public class WorkOrderMainTabsController : WorkOrderControllerBase
	{

		[Route("{buid}/{str_wo_id}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetProfile(int buid, string str_wo_id)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderMainTabs(CurrentUser, buid, str_wo_id);
			if (!CurrentUser.IsVisibleBusinessUnitId(buid) || !o.can_access)
			{
				return NotFound();
			}
			return Ok(o.Profile());
		}

	}
}
