using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using NESI.BLL.Core.Logger;

namespace NESI.WebAPI.Controllers.API.Core
{
	[RoutePrefix("api/Error")]
	public class ClientErrorHandlerController : ApiController
	{
		[HttpPost]
		[Route("")]
		public IHttpActionResult Post(DTO.Models.Core.ErrorLog model)
		{
			var httpRequest = HttpContext.Current.Request;
			model.user_ip = httpRequest.UserHostAddress;
			model.dt = DateTime.Now;

			new ClientErrorHandler().AddLog(model);
			return Ok();
		}
	}
}
