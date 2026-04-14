using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;

namespace NESI.WebAPI.Controllers.API.Page.ReleaseSystem
{
	[RoutePrefix("api/Page/PasswordEnhance")]
	public class PasswordEnhanceController : ApiController
	{
		[Route("")]
		[HttpGet]
		public async Task<IHttpActionResult> Get()
		{
			var o = new BLL.Pages.ReleaseSystem.PasswordEnhance();
			return await Task.FromResult(Ok(o.GenerateMemberScripts()));
		}
	}
}
