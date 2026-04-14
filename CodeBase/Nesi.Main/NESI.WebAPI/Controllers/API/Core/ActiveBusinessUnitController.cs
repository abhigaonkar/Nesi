using System.Linq;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Core
{
	[RoutePrefix("api/Core/ActiveBusinessUnit")]
	public class ActiveBusinessUnitController : EmployeeController
    {
	    [Route("")]
	    public IHttpActionResult Get()
	    {
		    var list = new BLL.Core.BusinessUnit(CurrentUser)
			    .GetActiveBusinessUnitDropDownLists()
				.Select(x => new {Label=x.ddl_name, Value=x.Id})
				.OrderBy(x => x.Value);
				 

		    return Ok(list);
	    }
	}
}
