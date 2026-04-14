using System.Linq;
using System.Web.Http;
using NESI.BLL.Common.Cache;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Cache
{
	[RoutePrefix("api/Cache/BusinessUnit")]
	public class CacheBusinessUnitController : EmployeeController
    {
  	    [Route("")]
	    public IHttpActionResult Get()
	    {
			return Ok(Global.BusinessUnit.GetList().OrderBy(m => m.ID));
		}

	    [Route("{id}")]
	    public IHttpActionResult Get(int id)
	    {
		    return Ok(Global.BusinessUnit.GetValue(id.ToString()));
	    }

	}
}
