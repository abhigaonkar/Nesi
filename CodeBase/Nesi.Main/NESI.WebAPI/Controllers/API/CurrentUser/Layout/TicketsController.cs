using System.Web.Http;
using NESI.BLL.Layout.Tickets;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[Route("api/Layout/Tickets")]
	public class TicketsController : EmployeeController
	{
		public IHttpActionResult Get()
		{
			return Ok(new Tickets(CurrentUser).GetTickets());
		}
	}
}
