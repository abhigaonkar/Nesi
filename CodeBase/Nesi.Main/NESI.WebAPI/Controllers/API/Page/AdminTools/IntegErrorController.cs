using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;
namespace NESI.WebAPI.Controllers.API.Page.AdminTools
{
    [PageAuthorizationFilter(260)]
    [RoutePrefix("api/Page/AdminTools/IntegError")]
    public class IntegErrorController : AdminToolsControllerBase
    {
        [Route("Months")]
        [HttpGet]
        public IHttpActionResult GetAvailableMonths()
        {
            var o = new BLL.Pages.AdminTools.IntegErrorBase();
            return Ok(o.GetErrorMonths());
        }

        [Route("Log/{duration}")]
        [HttpGet]
        public IHttpActionResult GetErrorLogs(string duration)
        {

            var o = new BLL.Pages.AdminTools.IntegErrorBase();
            var v= o.GetIntegrationErrors(duration);
            return Ok(v);
        }
    }
}