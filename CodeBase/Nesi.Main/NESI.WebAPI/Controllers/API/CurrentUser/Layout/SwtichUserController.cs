using System.Web.Http;
using NESI.BLL.Common;
using NESI.BLL.Layout.Banner;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[RoutePrefix("api/Layout")]
	public class SwtichUserController : EmployeeController
	{
		[Route("SwitchUserList")]
		public IHttpActionResult Get()
		{
			return Ok(new SwtichUser(UId,OriginalUser).GetSwitchUserList());
		}

		[HttpPut]
		[Route("SwitchUser/{id}")]
		public IHttpActionResult SwitchToUser(int id)
		{
			var employee = OriginalUser;
			var switchUser = new SwtichUser(UId,employee);
			if (!switchUser.CanSwitchUser) return NotFound();
			switchUser.SwitchToUser(id);
			return Ok(CovertoJsonData(true));
		}

		[HttpDelete]
		[Route("SwitchUser")]
		public IHttpActionResult CancelSwitchToUser()
		{
			var employee = OriginalUser;
			var switchUser = new SwtichUser(UId, employee);
			if (!switchUser.CanSwitchUser) return NotFound();
			if (employee.Id == CurrentUserId) return NotFound();
			switchUser.CancelSwitchToUser();
			return Ok(CovertoJsonData(true));
		}
	}
}
