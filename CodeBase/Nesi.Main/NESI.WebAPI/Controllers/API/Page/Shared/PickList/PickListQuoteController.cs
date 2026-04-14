using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Pages.Shared.PickList.Quote;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Shared.PickList;
using NESI.DTO.ViewModels.Shared.PickList.Quote;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Shared.PickList
{
	[RoutePrefix("api/Page/Shared/PickList/Quote")]
	public class PickListQuoteController : EmployeeController
	{
		[HttpDelete]
		[QuoteFilter()]
		[Route("{quoteId}/{revision}/{id}")]
		public IHttpActionResult DeleteWorksheet(int quoteId, int revision, int id)
		{
			return OkD((new PickListQuoteLine(CurrentUser, quoteId, revision)).DeleteWorkSheet(id));
		}
		[HttpPost]
		[QuoteFilter()]
		[Route("ReOrder/{quoteId}/{revision}")]
		public IHttpActionResult ReOrder([FromBody] ReOrderItem[] models, int quoteId, int revision)
		{
			return Ok(new PickListQuoteLine(CurrentUser, quoteId, revision).ReOrder(models));
		}
		[HttpPost]
		[QuoteFilter()]
		[Route("Labor/{quoteId}/{revision}")]
		public IHttpActionResult InsertLabor([FromBody] DTO.ViewModels.Shared.PickList.Quote.PickListQuoteWorksheetLabor[] models, int quoteId, int revision)
		{
			return OkD(new PickListQuoteLine(CurrentUser, quoteId, revision).AddQuoteLabor(models));
		}

		[HttpPost]
		[Route("CopyMoveLines")]
		public IHttpActionResult CopyMove([FromBody] DTO.ViewModels.Shared.PickList.Quote.PickListQuoteCopyMoveLine model)
		{
			var o = new PickListQuoteLine(CurrentUser, model.quote_id, model.revision);
			return Ok(model.type == "Copy" ? o.CopyLines(model) : o.MoveLines(model));
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("ApplyDiscount/{quoteId}/{revision}")]
		public IHttpActionResult ApplyDiscount([FromBody] DTO.ViewModels.Core.DataBool model, int quoteId, int revision)
		{
			var o = new PickListQuoteLine(CurrentUser, quoteId, revision);
			return Ok(o.applyDiscount(model.data));
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("LineIsChecked/{quoteId}/{revision}")]
		public IHttpActionResult LineIsChecked([FromBody] DTO.ViewModels.Core.DataIdBool model, int quoteId, int revision)
		{
			var o = new PickListQuoteLine(CurrentUser, quoteId, revision);
			return OkD(o.UpdateQuoteLineChecked(model.id, model.value));
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("Material/{quoteId}/{revision}")]
		public IHttpActionResult InsertMaterial([FromBody] DTO.ViewModels.Shared.PickList.Quote.PickListQuotePartDetailInsert model, int quoteId, int revision)
		{
			return OkD(new PickListQuoteLine(CurrentUser, quoteId, revision).AddNewQuoteMaterialLine(model));
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("Materials/{quoteId}/{revision}")]
		public IHttpActionResult InsertMaterials([FromBody] DTO.ViewModels.Shared.PickList.Quote.PickListQuotePartDetailInsert[] models, int quoteId, int revision)
		{
			return OkD(new PickListQuoteLine(CurrentUser, quoteId, revision).AddNewQuoteMaterialLines(models));
		}

		[Route("Inventory/{quoteId}/{revision}/{masterId}/{qty}/{lineId}")]
		[QuoteFilter()]
		public IHttpActionResult GetInventoryDetail(int quoteId, int revision, int masterId, double qty, int lineId)
		{
			return Ok(new PickListQuoteLine(CurrentUser, quoteId, revision).GetInventoryByMasterId(masterId, qty, lineId));
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("Inventories/{quoteId}/{revision}")]
		public IHttpActionResult GetInventoryDetails([FromBody] int[] master_id_list, int quoteId, int revision)
		{
			return Ok(new PickListQuoteLine(CurrentUser, quoteId, revision).GetInventoryByMasterIdList(master_id_list));
		}

		//		[HttpPost]
		//		[Route("QueryPart/{quoteId}/{revision}")]
		//		public IHttpActionResult GetPartsByQuery([FromBody] DTO.ViewModels.Core.DataString model, int quoteId, int revision)
		//		{
		//			return Ok((new PickListQuote(CurrentUser, quoteId, revision)).GetPartsBySearchNumber(model.Data));
		//		}

		[HttpPost]
		[QuoteFilter()]
		[Route("Part/{quoteId}/{revision}")]
		public IHttpActionResult GetPartsByMasterId([FromBody] DTO.ViewModels.Core.DataInt model, int quoteId, int revision)
		{
			return Ok((new PickListQuote(CurrentUser, quoteId, revision)).GetPartsByMasterId(model.Data));
		}


		[HttpPost]
		[QuoteFilter()]
		[Route("SearchInventory/{quoteId}/{revision}")]
		public IHttpActionResult GetSearchInventory([FromBody] DTO.ViewModels.Core.DataStringType model, int quoteId, int revision)
		{
			return Ok((new InventorySearch(CurrentUser, quoteId, revision)).Search(model));
		}

		[Route("Sections/{quoteId}/{revision}")]
		[QuoteFilter()]
		public IHttpActionResult GetSections(int quoteId, int revision)
		{
			return Ok(new PickListQuote(CurrentUser).GetFullSectionList(quoteId, revision));
		}

		[Route("SectionList/{quoteId}/{revision}")]
		[QuoteFilter()]
		public IHttpActionResult GetSectionList(int quoteId, int revision)
		{
			return Ok(new PickListQuote(CurrentUser).GetSectionList(quoteId, revision));
		}

		#region Edit Sections in worksheet.
		[HttpGet]
		[QuoteFilter()]
		[Route("EditSections/{quoteId}/{revision}")]
		public IHttpActionResult GetEditSections(int quoteId, int revision)
		{
			return Ok(new PickListQuoteLine(CurrentUser, quoteId, revision).LoadSections());
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("EditSections/{quoteId}/{revision}")]
		public IHttpActionResult NewSections([FromBody] DTO.ViewModels.Core.DataString model, int quoteId, int revision)
		{
			return OkD(new PickListQuoteLine(CurrentUser, quoteId, revision).AddSection(model.Data));
		}

		[HttpPatch]
		[Route("EditSections/{quoteId}/{revision}")]
		[QuoteFilter()]
		public IHttpActionResult EditSections([FromBody] DTO.ViewModels.Core.LabelValueInt model, int quoteId, int revision)
		{
			return OkD(new PickListQuoteLine(CurrentUser, quoteId, revision).EditSection(model.Value, model.Label));
		}

		[HttpDelete]
		[Route("EditSections/{quoteId}/{revision}/{id}")]
		[QuoteFilter()]
		public IHttpActionResult DeleteSection(int quoteId, int revision, int id)
		{
			return OkD(new PickListQuoteLine(CurrentUser, quoteId, revision).DeleteSection(id));
		}
		#endregion

		[HttpGet]
		[QuoteFilter()]
		[Route("CostHandler/{quoteId}/{revision}/{qty}/{cost}")]
		public IHttpActionResult CostHandler(int quoteId, int revision, string qty, string cost)
		{
			var dqty = Convert.ToDouble(qty.Replace("_", "."));
			var dcost = Convert.ToDouble(cost.Replace("_", "."));
			return Ok(new PickListQuoteLine(CurrentUser, quoteId, revision).CostHandler(dqty, dcost));
		}

		[Route("{quoteId}/{revision}")]
		[QuoteFilter()]
		public IHttpActionResult Get(int quoteId, int revision)
		{
            if (revision == 0)
            {
                string quote = quoteId.ToString();
                string id = quote.Substring(0, 6);
                string rev = quote.Substring(6, quote.Length - 6);
                quoteId = int.Parse(id);
                revision = int.Parse(rev);
            }

            return Ok(new PickListQuote(CurrentUser, quoteId, revision));
		}

		[HttpPost]
		[Route("Note")]
		public IHttpActionResult UpdateNote([FromBody] PickListNote model)
		{
			return OkD((new PickListQuoteLine(CurrentUser)).UpdateNote(model));
		}

		[HttpPatch]
		[Route("Line/{field}")]
		public IHttpActionResult UpdateLine([FromBody] PickListQuotePartDetailEdit model, string field)
		{
			return Ok((new PickListQuoteLine(CurrentUser, model.quote_id, model.revision)).UpdateLine(model, field));
		}


		[Route("RecentWos/{quoteId}/{revision}")]
		[QuoteFilter()]
		public IHttpActionResult GetRecentWos(int quoteId, int revision)
		{
			return Ok((new InventorySearch(CurrentUser, quoteId, revision)).GetRecentWos());
		}

		[Route("RecentQuotes/{quoteId}/{revision}")]
		[QuoteFilter()]
		public IHttpActionResult GetRecentQuotes(int quoteId, int revision)
		{
			return Ok((new InventorySearch(CurrentUser, quoteId, revision)).GetRecentQuotes());
		}



	}
}
