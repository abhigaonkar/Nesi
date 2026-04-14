using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/Files")]
    public class EmployeeFilesController : EmployeesControllerBase
    {

	    [HttpPost]
	    [Route("CopyApplicant/{memberid}")]
	    public IHttpActionResult CopyApplicant(int memberid)
	    {
		    var o = new BLL.Core.FileManager.MemberFile(memberid);
			o.CopyFromApplicantFiles();
		    return OkD("Files have been copied successfully.");
	    }
	}
}
