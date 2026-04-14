using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using NESI.BLL.Base;
using NESI.BLL.Common.Cache;
using NESI.BLL.Common.Shared;
using NESI.BLL.Core.Employee;
using NESI.BLL.Core.Member;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.CurrentUser;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.CurrentUser.SignIn
{
	[RoutePrefix("api/CurrentUser")]
	public class CurrentUserController : ApiControllerBase
	{
		[Route("")]
		public IHttpActionResult Get()
		{
			return Ok(CurrentUser);
		}

		[HttpDelete]
		[Route("")]
		public IHttpActionResult Delete()
		{
			bool result;
			try
			{
				Global.OnlineUser.Delete(UId.ToString());
				Global.OriginalUser.Delete(UId.ToString());
				HttpContext.Current.GetOwinContext().Authentication.SignOut();
				result = true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				result = false;
			}
			return Ok(new { data = result });
		}

		[Route("Active")]
		public IHttpActionResult GetActive()
		{
			if (CurrentUser == null)
			{
				return NotFound();
			}
			try
			{
				if ((DateTime.Now - CurrentUser.PingTime).TotalSeconds >
					60 * BLL.Common.Shared.Configuration.TimeOutOfPingTime)
				{
					return NotFound();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return NotFound();
			}
			return Ok(
				new
				{
					user = new ActiveUser(CurrentUser),
					visible_business_unit_list = CurrentUser.VisibleBusinessUnitLabelValueList
				});
		}

		[HttpPost]
		[Route("Ping")]
		public IHttpActionResult Ping()
		{
			CurrentUser.PingTime = DateTime.Now;
			return OkD(1);
		}


		[HttpPost]
		[Route("MobileLog")]
		public IHttpActionResult MobileLog([FromBody] DataInt model)
		{
			var o = new ProfileBase(new Employee(CurrentUserId));
			return OkD(o.SetPropertyValue("last_mobile_login", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
		}

		[Route("Privilege/{id}")]
		[HttpGet]
		public IHttpActionResult GetPrivilege(int id)
		{
			return Ok(CovertoJsonData(CurrentUser.AuthenticatedForPrivilege(id)));
		}

		[Route("Page/{id}")]
		[HttpGet]
		public IHttpActionResult GetPage(int id)
		{
			return Ok(CovertoJsonData(CurrentUser.AuthorizePage(id)));
		}

		[Route("PageUrl/{id}")]
		[HttpGet]
		public IHttpActionResult GetPageUrl(int id, [FromUri] bool isMobile = false)
		{

			var url = CurrentUser.GetMenuRouter(id, isMobile);
			if (url != null)
			{
				return Ok(url);
			}
			return NotFound();
		}

	}
}
