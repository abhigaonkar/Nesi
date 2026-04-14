using NESI.WebAPI.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    public class MemberController : EmployeeController
    {
        [Route("api/Page/Member/Get")]
        public IHttpActionResult Get()
        {
            return Ok(new BLL.Pages.Reports.Member().GetList());
        }

    }
}