using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Page.HomePage
{
	[Route("api/Page/HomePage/Contact")]
	public class HomePageContactController : ApiControllerBase
	{
		public IHttpActionResult Get()
		{
			return Ok(new BLL.Pages.HomePage.HomePageContact(CurrentUser));
		}
	}
}
