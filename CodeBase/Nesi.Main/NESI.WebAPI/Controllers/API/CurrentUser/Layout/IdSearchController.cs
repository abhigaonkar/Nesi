using System.Web.Http;
using NESI.BLL.Layout.Banner;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[RoutePrefix("api/Layout/IdSearch")]
    public class IdSearchController : EmployeeController
    {
        
        [HttpPost]
        [Route("")]
        public IHttpActionResult Search([FromBody] DTO.ViewModels.Core.DataString model)
        {
	        return Ok(new BLL.Layout.Banner.IdSearch(CurrentUser).GetSearch(model.Data));
	    }
    }
}
