using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Core.Employee;
using NESI.BLL.Layout.Banner;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[RoutePrefix("api/Layout")]
	public class MenusController : ApiControllerBase
    {
	    [Route("Menus")]
	    [HttpGet]
	    public IHttpActionResult GetMenus()
	    {
//		    if (!CurrentUser.IsContact)
//		    {
//			    return Ok(new ToDo((Employee)CurrentUser).BadageMenus());
//		    }
//		    else
//		    {
			    return Ok(CurrentUser.GetMenus());
//		    }
	    }

	    [Route("MobileMenus")]
	    [HttpGet]
	    public IHttpActionResult GetMobileMenus()
	    {
		    return Ok(CurrentUser.GetMobileMenus());
	    }

        
    }
}
