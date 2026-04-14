using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Quotes;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Quotes
{
	[RoutePrefix("api/Page/Quotes/Edit")]
	public class QuoteEditPageController : QuoteControllerBase
	{

		[QuoteFilter()]
		[Route("Init/{quoteId}/{revision}")]
		public IHttpActionResult GetInit(int quoteId, int revision)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser).InitilizeQuote(quoteId, revision);
			return Ok(o);
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("ActiveRevision/{quoteId}/{revision}")]
		public IHttpActionResult ActiveRevision(int quoteId, int revision)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser, quoteId, revision);
			return OkD(o.set_active_revision());
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("UpdateLastPrintDate/{quoteId}/{revision}")]
		public IHttpActionResult UpdateLastPrintDate(int quoteId, int revision)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser, quoteId, revision);
			return Ok(o.Update_last_printed());
		}

		[HttpPost]
		[Route("UpdateStatus")]
		public IHttpActionResult UpdateStatus([FromBody] QuoteStatusUpdate model)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser, model.quote_id, model.revision);
			return OkD(o.status_update(model));
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("ReOrder/{quoteId}/{revision}/{type}")]
		public IHttpActionResult ReOrder([FromBody] ReOrderItem[] models, int quoteId, int revision, int type)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser, quoteId, revision);
			return Ok(o.re_order(models, type));
		}

		[Route("FollowUpHistory/{quoteId}/{revision}")]
		[QuoteFilter()]
		public IHttpActionResult GetHistory(int quoteId, int revision)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser, quoteId, revision);
			return Ok(o.GetFollowHistory());
		}

		[HttpPost]
		[Route("DoneFollowUp/{followUpId}")]
		public IHttpActionResult DoneFollowUp(int followUpId)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser);
			return OkD(o.DoneFollowUp(followUpId));
		}

		[HttpPost]
		[Route("ScheduleFollowUp")]
		public IHttpActionResult ScheduleFollowUp([FromBody] QuoteFollowUp model)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser, model.quote_id, model.revision);
			return OkD(o.AddFollowUp(model));
		}

		[Route("Address/{customerId}/{addressId}")]
		public IHttpActionResult GetAddressInfo(int customerId, int addressId)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser).GetCustomerAddressInfo(customerId, addressId);
			return Ok(o);
		}
		[Route("Contact/{contactId}")]
		public IHttpActionResult GetContactInfo(int contactId)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser).GetContactInfo(contactId);
			return Ok(o);
		}
		[HttpPost]
		[Route("Update")]
		public IHttpActionResult UpdateField(DTO.ViewModels.Page.Quotes.QuoteEditFieldUpdate model)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser).UpdateField(model);
			return Ok(o);
		}

		[HttpPost]
		[Route("Duplicate/{quoteId}/{revision}")]
		[QuoteFilter()]
		public IHttpActionResult Duplcate(int quoteId, int revision)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser, quoteId, revision).duplicate_quote();

			return Ok(o);
		}

		[HttpPost]
		[Route("UpdateAll")]
		public IHttpActionResult UpdateAll(DTO.ViewModels.Page.Quotes.QuoteEditUpdateAll model)
		{
			var o = new BLL.Pages.Quotes.QuoteEdit(CurrentUser).UpdateAll(model);
			return Ok(o);
		}

		[Route("Detail/NewId/{quoteId}/{revision}/{typeId}")]
		[QuoteFilter()]
		public IHttpActionResult GetNewId(int quoteId, int revision, int typeId)
		{
			return Ok(new BLL.Pages.Quotes.QuoteDetail(CurrentUser, quoteId, revision).GetNewDetailId(typeId));
		}

		[Route("Detail/NewSection/{quoteId}/{revision}/{lineId}")]
		[QuoteFilter()]
		public IHttpActionResult GetNewSection(int quoteId, int revision, int lineId)
		{
			return Ok(new BLL.Pages.Quotes.QuoteDetail(CurrentUser, quoteId, revision).CreateSectionForNoteAdder(lineId));
		}

		[HttpPost]
		[Route("Detail/NewIds/{quoteId}/{revision}/{typeId}")]
		[QuoteFilter()]
		public IHttpActionResult PostNewIds([FromBody] DTO.ViewModels.Core.LabelValueInt[] models, int quoteId, int revision, int typeId)
		{
			return Ok(new BLL.Pages.Quotes.QuoteEdit(CurrentUser, quoteId, revision).SaveNewDetails(typeId, models));
		}

		[Route("Detail/Div/{quoteId}/{revision}/{typeId}")]
		[QuoteFilter()]
		public IHttpActionResult GetDiv(int quoteId, int revision, int typeId)
		{
			return Ok(new BLL.Pages.Quotes.QuoteEdit(CurrentUser).GetQuoteDiv(quoteId.ToString(), revision.ToString(), typeId));
		}

		[HttpPost]
		[Route("Detail/Delete/{quoteId}/{revision}/{typeId}")]
		[QuoteFilter()]
		public IHttpActionResult DeleteDetail([FromBody] DataIdInt model, int quoteId, int revision, int typeId)
		{
			return Ok(new BLL.Pages.Quotes.QuoteDetail(CurrentUser, quoteId, revision).del_extratext_detail(typeId, model.id, model.value == 1));
		}


        [HttpGet]
        [QuoteFilter()]
		[Route("Opportunity/{customerId}")]
        public IHttpActionResult GetOpportunityListByCustomerId(int customerId)
        {
            return Ok(new BLL.Pages.Quotes.QuoteEdit(CurrentUser).GetOpportunities(customerId));
        }
	}
}
