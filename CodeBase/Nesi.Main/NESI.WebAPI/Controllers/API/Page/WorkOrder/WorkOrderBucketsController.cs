using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.WorkOrder
{
	[RoutePrefix("api/Page/WorkOrder/Buckets")]
	public class WorkOrderBucketsController : WorkOrderControllerBase
	{


		[Route("")]
		public IHttpActionResult GetProfileEmpty()
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderBuckets(CurrentUser, 0, 0, "WOProg_OpenDateTime");
			return Ok(o.Profile());
		}

		[Route("{buid}")]
		[VisibleBusinessUnitFilter]
		public IHttpActionResult GetProfileBuId(int buid)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderBuckets(CurrentUser, buid, 0, "WOProg_OpenDateTime");
			return Ok(o.Profile());
		}

		[Route("{buid}/{pmid}/{orderby}")]
		[VisibleBusinessUnitFilter]
		public IHttpActionResult GetProfile(int buid, int pmid, string orderby)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderBuckets(CurrentUser, buid, pmid, orderby);
			return Ok(o.Profile());
		}
	}
}
