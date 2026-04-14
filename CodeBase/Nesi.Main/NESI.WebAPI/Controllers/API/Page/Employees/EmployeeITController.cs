using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/IT")]
    public class EmployeeITController : EmployeesControllerBase
    {
	    [HttpGet]
	    [Route("Profile/{memberid}")]
	    public IHttpActionResult GetProfile(int memberid)
	    {
		    var o = new BLL.Pages.Employees.EmployeeIT(CurrentUser, memberid);
		    return !o.can_access ? (IHttpActionResult) NotFound() : Ok(o.Profile());
	    }

        [HttpPost]
        [Route("Profile/{memberid}")]
        public IHttpActionResult Save([FromBody] DTO.ViewModels.Page.Employees.EmployeeIT model, int memberid)
        {
	        var o = new BLL.Pages.Employees.EmployeeIT(CurrentUser, memberid);
	        return !o.can_access ? (IHttpActionResult) NotFound() : Ok(o.Save(model));
        }


	    [HttpPost]
	    [Route("ResetPassword/{memberid}")]
	    public IHttpActionResult ResetPassword([FromBody] DataString model, int memberid)
	    {
		    var o = new BLL.Pages.Employees.EmployeeIT(CurrentUser, memberid);
		    return !o.can_access ? NotFound() : OkD(o.ResetPassword(model.Data, DateTime.Now));
	    }

		[HttpGet]
        [Route("SyncLdap/{memberid}")]
        public IHttpActionResult SyncLdap(int memberid, [FromBody] DataString model)
        {
            var o = new BLL.Pages.Employees.EmployeeIT(CurrentUser, memberid);
			return !o.can_access ? NotFound() : OkD(o.syncLdapPassword(model.Data));
        }

    }
}
