using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.Member;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[Route("api/Layout/AllActive")]
	public class AllActiveController : ApiControllerBase
	{
		[HttpGet]
		public IHttpActionResult GetAllActive()
		{
			return Ok(new BLL.Layout.Banner.AllActiveUser().GetAllActiveUsers());
		}

	}
}
