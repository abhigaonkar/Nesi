using System.Web.Http;
using NESI.BLL.Layout.Banner;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[Route("api/Layout/ToDo")]
	public class ToDoController : EmployeeController
	{
		public IHttpActionResult Get()
        {
            return Ok(new ToDo(CurrentUser).GetToDo());
        }
    }
}
