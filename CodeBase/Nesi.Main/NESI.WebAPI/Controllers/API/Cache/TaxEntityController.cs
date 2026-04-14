using System.Linq;
using System.Web.Http;
using NESI.BLL.Common.Cache;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Authentication;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Cache
{
	[RoutePrefix("api/Cache/TaxEntity")]
	public class TaxEntityController : ApiControllerBase
    {
	  
	    [Route("")]
	    public IHttpActionResult Get()
	    {
		    return Ok(Global.TaxEntity.GetList().OrderBy(m=>m.id));
	    }

	    [Route("{id}")]
	    public IHttpActionResult Get(int id)
	    {
		    return Ok(Global.TaxEntity.GetValue(id.ToString()));
	    }
	}
}
