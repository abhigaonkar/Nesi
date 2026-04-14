using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Core
{
	[RoutePrefix("api/core/PayTypeHours")]
    public class PayTypeHoursController : EmployeeController
    {
	    [Route("List/{buid}")]
	    public IHttpActionResult GetList(int buid)
	    {
		    return Ok(new BLL.Core.Ne2PayTypeHours().GetList(buid));
	    }
   
    }
}
