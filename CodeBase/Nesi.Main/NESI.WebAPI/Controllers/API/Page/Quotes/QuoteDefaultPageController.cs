using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Quotes
{
	[RoutePrefix("api/Page/Quotes")]

	public class QuoteDefaultPageController : QuoteControllerBase
	{
		[Route("Profile")]
		public IHttpActionResult GetProfile()
		{
			return Ok(new BLL.Pages.Quotes.QuoteProfile(CurrentUser));
		}

		[QuoteFilter()]
		[Route("ActiveRevision/{quoteId}")]
		public IHttpActionResult GetActiveRevision(int quoteId)
		{
			return OkD(new BLL.Pages.Quotes.QuoteProfile().GetActiveRevision(quoteId.ToString()));
		}

		[HttpPost]
		[Route("ProfileFilter")]
		public IHttpActionResult GetProfileFilter([FromBody] int[] selected_users)
		{
			return Ok(new BLL.Pages.Quotes.QuoteProfile().GetQuoteSummaryByMemberList(selected_users));
		}

		[HttpPost]
		[Route("Search")]
		public IHttpActionResult Search([FromBody] DTO.ViewModels.Page.Quotes.QuoteSearch model)
		{
			return Ok(new BLL.Pages.Quotes.QuoteSearch(CurrentUser).doSearch(model));
		}
		[Route("Search/Init")]
		public IHttpActionResult GetSearchInit()
		{
			return Ok(new BLL.Pages.Quotes.QuoteSearch(CurrentUser));
		}

		[Route("Search/UserList/{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetSearchGetUserList(int buid)
		{
			return Ok(new BLL.Pages.Quotes.QuoteSearch(CurrentUser).GetUserListByBusinessUnit(buid));
		}

        [HttpGet]
        [Route("Exist/{quoteId}")]
        public IHttpActionResult Exist(string quoteId)
        {
            return OkD(new BLL.Pages.Quotes.QuoteSearch(CurrentUser).DoesQuoteExist(quoteId));
        }
    }
}
