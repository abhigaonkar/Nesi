using System.Web.Http;
using NESI.BLL.Layout.Banner;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[Route("api/Layout/Message")]
	public class MessageController : EmployeeController
	{

		public IHttpActionResult Get()
		{
			return Ok(new Message(CurrentUser).GetMessages());
		}
	}
}
