using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.CurrentUser.Layout;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[RoutePrefix("api/Layout/DashMessage")]
	public class DashMessageController : EmployeeController
    {
		[Route("{id}")]
	    public IHttpActionResult Get(int id)
	    {
		    return Ok(new BLL.Layout.Menu.DashMessage(CurrentUser).GetDashMessageListByBusinessUnitId(id));
	    }

		[Route("")]
	    public IHttpActionResult Post([FromBody] DashMessage msg)
	    {
		    return Ok(CovertoJsonData(new BLL.Layout.Menu.DashMessage(CurrentUser).InsertDashMessage(msg)));

	    }
	}
}
