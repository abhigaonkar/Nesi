using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.BLL.Pages.Timesheet.Expense;
using NESI.DTO.ViewModels.Page.PurchaseOrder;
using NESI.DTO.ViewModels.Page.Shared;
using NESI.WebAPI.Infrastructures.Filters;
using NESI.DTO.ViewModels.Page.PurchaseOrder;


namespace NESI.WebAPI.Controllers.API.Page.PurchaseOrder
{
	[RoutePrefix("api/Page/PurchaseOrder")]
	public class PurchaseOrderHomePageController : PurchaseOrderControllerBase
	{

		[Route("Profile")]
		public IHttpActionResult GetProfile()
		{
			var o = new BLL.Pages.PurchaseOrder.PurchaseOrderBase(CurrentUser);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("SaveLayout")]
		public IHttpActionResult SaveLayOut([FromBody] OrderHomeLayout model)
		{
			var o = new BLL.Pages.PurchaseOrder.PurchaseOrderBase(CurrentUser);
			return OkD(o.SaveHomeLayout(model,"PurchaseOrder"));
		}

		[Route("SummaryDetail/{buid}")]
		[VisibleBusinessUnitFilter]
		public IHttpActionResult GetSummary(int buid)
		{
			var o = new BLL.Pages.PurchaseOrder.PurchaseOrderBase(CurrentUser);
			return Ok(o.GetBusinessUnitSummaryDetail(buid));
		}

		[HttpPost]
		[Route("BusinessUnitSummary")]
		public IHttpActionResult GetBusinessUnitSummary([FromBody] int[] buids)
		{
			var o = new BLL.Pages.PurchaseOrder.PurchaseOrderBase(CurrentUser);
			return Ok(o.GetSummaryByBusinessUnit(buids));
		}

		[HttpGet]
		[Route("APStatus")]
		public IHttpActionResult GeAPStatus()
		{
			var o = new BLL.Pages.PurchaseOrder.PurchaseOrderBase();
			return Ok(o.APStatusList());
		}

		[HttpPost]
		[Route("UpdateAPNotes")]
		public IHttpActionResult UpdateAPNotes([FromBody] DTO.ViewModels.Core.DataIdString model)
		{
			var o = new BLL.Pages.PurchaseOrder.PurchaseOrderBase();
			return Ok(o.UpdateAPNotes(model.id,model.value));
		}

		[HttpPost]
		[Route("UpdateStatus")]
		public IHttpActionResult UpdateStatus([FromBody] DTO.ViewModels.Core.DataIdInt model)
		{
			var o = new BLL.Pages.PurchaseOrder.PurchaseOrderBase();
			return Ok(o.UpdateStatus(model.id, model.value));
		}

        [HttpGet]
        [Route("CreditCard")]
        public IHttpActionResult CreditCard()
        {

            var o = new BLL.Pages.PurchaseOrder.PurchaseOrderBase(CurrentUser);
            return Ok(o.GetCreditCardList());
        }

        [HttpPost]
        [Route("CreditCardSave")]
        public IHttpActionResult CreditCardSave([FromBody] AddCreditCard model)
        {
            var selectedUser = new Employee(model.member_id);
            if (!CanSeeUser(selectedUser)) return NotFound();

            var o = new BLL.Pages.PurchaseOrder.PurchaseOrderBase(CurrentUser);
            return OkD(o.SaveCreditCard(0, selectedUser, model));
        }


    }
}

