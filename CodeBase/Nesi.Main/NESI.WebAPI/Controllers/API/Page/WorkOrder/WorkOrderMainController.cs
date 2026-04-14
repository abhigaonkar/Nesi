using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.WorkOrder
{
	[RoutePrefix("api/Page/WorkOrder/Edit/Main")]
	public class WorkOrderMainController : WorkOrderControllerBase
	{

		[Route("{bu_id}/{str_wo_id}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetProfile(int bu_id, string str_wo_id)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderMain(CurrentUser, bu_id, str_wo_id);
			return Ok(o.Profile());
		}

	}
}
