using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Quotes
{
	[RoutePrefix("api/Page/Quotes/Strategy")]
	public class QuoteStrategyPageController : QuoteControllerBase
	{
		[HttpGet]
		[Route("ProcessQuestionHistory/{quoteId}/{type}")]
		public IHttpActionResult ProcessQuestionHistory(int quoteId, string type)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategyUpdate(CurrentUser, quoteId).GetQuoteProcessQuestionHistory(type);
			return Ok(o);
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("ProcessQuestionHistory/{quoteId}/{type}")]
		public IHttpActionResult SaveProcessQuestionHistory([FromBody] DTO.ViewModels.Page.Quotes.QuoteStrategyQuestion[] models, int quoteId, string type)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategyUpdate(CurrentUser, quoteId).SaveQuoteProcessQuestionHistory(type, models);
			return OkD(o);
		}

		[HttpDelete]
		[QuoteFilter()]
		[Route("ProcessQuestionHistory/{quoteId}/{type}")]
		public IHttpActionResult DeleteProcessQuestionHistory(int quoteId, string type)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategyUpdate(CurrentUser, quoteId).ClearQuoteQuestionHistory(type);
			return OkD(o);
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("ProcessQuestionHistoryUpdate/{quoteId}")]
		public IHttpActionResult ProcessQuestionHistoryUpdate([FromBody] DTO.ViewModels.Core.NameFieldValueId model, int quoteId)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategyUpdate(CurrentUser, quoteId).ProcessQuestionHistoryUpdate(model);
			return Ok(o);
		}

		[HttpGet]
		[QuoteFilter()]
		[Route("ReconScore/{quoteId}/{type}")]
		public IHttpActionResult ReconScore(int quoteId, string type)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategyUpdate(CurrentUser, quoteId).GetQuoteProcessQuestionHistory(type);
			return Ok(o);
		}



		[Route("Init/{quoteId}")]
		[QuoteFilter()]
		public IHttpActionResult GetInit(int quoteId)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategy(CurrentUser, quoteId);
			return Ok(o);
		}

		[Route("Stage1/{quoteId}")]
		public IHttpActionResult GetStage1(int quoteId)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategyStage1(CurrentUser, quoteId);
			return Ok(o);
		}

	

		[HttpPost]
		[QuoteFilter()]
		[Route("Stage1/{quoteId}")]
		public IHttpActionResult ApproveStage1([FromBody] DTO.ViewModels.Page.Quotes.QuoteStrategyStage1 model, int quoteId)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategyStage1(CurrentUser, quoteId).ApproveStage1(model);
			return OkD(o);
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("UpdateScheduleItem/{quoteId}")]
		public IHttpActionResult UpdateScheduleItem([FromBody] DTO.ViewModels.Core.NameFieldValue model, int quoteId)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategyUpdate(CurrentUser, quoteId).UpdateScheduleItem(model);
			return Ok(o);
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("KillStage1/{quoteId}")]
		public IHttpActionResult KillStage1([FromBody] DTO.ViewModels.Core.DataString model, int quoteId)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategyStage1(CurrentUser, quoteId);
			return OkD(o.CloseStage1(model.Data));
		}

		[HttpPost]
		[QuoteFilter()]
		[Route("KillQuote/{quoteId}")]
		public IHttpActionResult KillQuote([FromBody] DTO.ViewModels.Core.DataString model, int quoteId)
		{
			var o = new BLL.Pages.Quotes.QuoteStrategyUpdate(CurrentUser, quoteId);
			return OkD(o.KillQuote(model.Data));
		}

	}
}
