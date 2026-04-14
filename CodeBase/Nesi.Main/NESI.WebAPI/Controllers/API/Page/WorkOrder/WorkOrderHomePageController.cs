using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Page.Shared;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.WorkOrder
{
	[RoutePrefix("api/Page/WorkOrder")]
	public class WorkOrderHomePageController : WorkOrderControllerBase
	{
		[Route("Profile")]
		public IHttpActionResult GetProfile()
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderBase(CurrentUser);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("BusinessUnitSummary")]
		public IHttpActionResult GetBusinessUnitSummary([FromBody] int[] buids)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderBase(CurrentUser);
			return Ok(o.GetSummaryByBusinessUnit(buids));
		}

		[Route("SummaryDetail/{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetSummary(int buid)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderBase(CurrentUser);
			return Ok(o.GetBusinessUnitSummaryDetail(buid));
		}

		[HttpPost]
		[Route("SaveLayout")]
		public IHttpActionResult SaveLayOut([FromBody] OrderHomeLayout model)
		{
			var o = new BLL.Pages.WorkOrder.WorkOrderBase(CurrentUser);
			return OkD(o.SaveHomeLayout(model, "WorkOrder"));
		}
	}
}
