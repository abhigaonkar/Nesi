using System.Web.Http;
using NESI.BLL.Layout.Banner;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[Route("api/Layout/WhoDoIAsks")]
	public class WhoDoIAskController : EmployeeController
	{
		public IHttpActionResult Get()
		{
			if (CurrentUser.IsContact) return NotFound();
			return Ok(new WhoDoIAsk(CurrentUser).GetWhoDoIAsks());
		}
	}
}
