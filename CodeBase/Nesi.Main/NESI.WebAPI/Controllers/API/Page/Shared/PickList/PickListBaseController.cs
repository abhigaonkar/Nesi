using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Pages.Shared.PickList;
using NESI.BLL.Pages.Shared.PickList.Quote;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Shared.PickList
{
	[RoutePrefix("api/Page/Shared/PickList")]
	public class PickListBaseController : EmployeeController
	{
		[Route("MemberTypes/{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetMemberTypes(int buid)
		{
			return Ok((new PickListBase(CurrentUser)).GetMemberTypes(buid));
		}

		[Route("Kitteds")]
		public IHttpActionResult GetKitteds()
		{
			return Ok((new PickListBase(CurrentUser)).GetKittedDDLs());
		}
		[Route("Kitteds/{buid}/{id}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetKitted(int buid, int id)
		{
			return Ok((new PickListBase(CurrentUser)).GetPartsByKitted(buid, id));
		}

		[Route("Groups")]
		public IHttpActionResult GetGroups()
		{
			return Ok((new PickListBase(CurrentUser)).GetGroupDDL());
		}

		[Route("Groups/{buid}/{id}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetGroup(int buid, int id)
		{
			if (!CanSeeBusinessUnit(buid)) return NotFound();
			return Ok((new PickListBase(CurrentUser)).GetPartsByGroup(buid, id));
		}



		[Route("Customers/{buid}")]
		public IHttpActionResult GetCustomers(int buid)
		{
			if (!CanSeeBusinessUnit(buid)) return NotFound();
			return Ok((new PickListBase(CurrentUser)).GetCustomersByBuId(buid));
		}

		[Route("CustomersQuotes/{buid}/{custId}")]
		public IHttpActionResult GetCustomerQuotes(int buid, int custId)
		{
			if (!CanSeeBusinessUnit(buid)) return NotFound();
			return Ok((new PickListBase(CurrentUser)).GetCustomerQuotesByCustomerId(custId));
		}

		[HttpPost]
		[Route("QueryQuotes")]
		public IHttpActionResult GetQuotesByQuery([FromBody] DTO.ViewModels.Core.DataString model)
		{
			return Ok((new PickListBase(CurrentUser)).GetQuotesByQuery(model.Data));
		}

		[Route("QuoteParts/{buid}/{partNo}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetQuoteParts(int buid, string partNo)
		{
			return Ok((new PickListBase(CurrentUser)).GetQuoteParts(buid, partNo));
		}
		[Route("CustomersWos/{buid}/{custId}")]
		public IHttpActionResult GetCustomerWos(int buid, int custId)
		{
			if (!CanSeeBusinessUnit(buid)) return NotFound();
			return Ok((new PickListBase(CurrentUser)).GetCustomerWosByCustomerId(custId));
		}

		[HttpPost]
		[Route("QueryWos")]
		public IHttpActionResult GetWosByQuery([FromBody] DTO.ViewModels.Core.DataString model)
		{
			return Ok((new PickListBase(CurrentUser)).GetWosByQuery(model.Data));
		}


		[Route("WoParts/{buid}/{partNo}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetWoParts(int buid, string partNo)
		{
			return Ok((new PickListBase(CurrentUser)).GetWoParts(buid, partNo));
		}

		[Route("ChargeOut/{buid}/{mtId}/{quoteId}/{revision}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetChargeOut(int buid, int mtId, int quoteId, int revision)
		{
			if (!CanSeeBusinessUnit(buid)) return NotFound();
			return Ok((new PickListBase(CurrentUser)).GetMemberTypeHours(buid, mtId, quoteId, revision));
		}

		[Route("InventoryPicture/{masterId}")]
		public IHttpActionResult GetInventoryPicture(int masterId)
		{
			return OkD((new PickListBase(CurrentUser)).GetInventroyPicutre(masterId));
		}

		[HttpGet]
		[Route("Settings/MatchWholeWord")]
		public IHttpActionResult GetMatchWholeWordSetting()
		{
			var o = new BLL.Pages.Shared.PickList.Quote.InventorySearch(CurrentUser);
			return OkD(o.GetMatchWholeWordSetting());
		}

		[HttpPost]		
		[Route("Settings/MatchWholeWord")]
		public IHttpActionResult SaveMatchWholeWordSetting([FromBody] DTO.ViewModels.Core.DataString model)
		{
			var o = new BLL.Pages.Shared.PickList.Quote.InventorySearch(CurrentUser);
			return OkD(o.SaveMatchWholeWordSetting(model.Data));
		}

		[HttpPost]
		[Route("InventorySearchHistory")]
		public IHttpActionResult GetInventorySearchHistory([FromBody] DTO.ViewModels.Core.DataString model)
		{
			var history = new BLL.Common.ToolBox.SearchHistory.InventorySearchHistory(CurrentUser);
			return Ok(history.SearchValues(model.Data));
		}

		[HttpGet]
		[Route("SpecificSections/{buid}/{type}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetSpecificSections(int buid, int type)
		{
			var o = (new PickListBase(CurrentUser));
			return Ok(
				new
				{
					can_edit = o.Can_Edit_SpecificSection(buid),
					list = o.GetSpecificSections(buid, type)
				});
		}

		[HttpPost]
		[Route("SpecificSections/{buid}/{type}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult AddSpecificSections([FromBody] DTO.ViewModels.Core.DataString model, int buid, int type)
		{
			return OkD((new PickListBase(CurrentUser)).AddSpeicficSection(model.Data, buid, type));
		}

		[HttpPatch]
		[Route("SpecificSections/{id}")]
		public IHttpActionResult UpdateSpecificSections([FromBody] DTO.ViewModels.Core.DataString model, int id)
		{
			return OkD((new PickListBase(CurrentUser)).UpdateSpeicficSection(model.Data, id));
		}

		[HttpDelete]
		[Route("SpecificSections/{id}")]
		public IHttpActionResult DeleteSpecificSections(int id)
		{
			return OkD((new PickListBase(CurrentUser)).DeleteSpeicficSection(id));
		}
	}
}
