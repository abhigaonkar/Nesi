using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Page.Employees;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/FootPrints")]
	public class EmployeeFootPrintsController : EmployeesControllerBase
    {
	    [HttpGet]
	    [Route("Profile/{memberid}")]
	    public IHttpActionResult GetOverride(int memberid)
	    {
		    var o = new BLL.Pages.Employees.EmployeeFootPrints(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
		    return Ok(o.Profile());
	    }

	    [HttpGet]
	    [Route("Options/{memberid}/{link}")]
	    public IHttpActionResult GetOptions(int memberid,string link)
	    {
		    var o = new BLL.Pages.Employees.EmployeeFootPrints(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
		    return Ok(o.GetOptions(link));
	    }

	    [HttpPost]
	    [Route("Reassign")]
	    public IHttpActionResult Reassign([FromBody] EmployeeFootPrintsReassign model)
	    {
		    var o = new BLL.Pages.Employees.EmployeeFootPrints(CurrentUser, model.from_member_id);
			if (!o.can_access) return NotFound();
		    return Ok(o.Reassign(model));
	    }
	}
}
