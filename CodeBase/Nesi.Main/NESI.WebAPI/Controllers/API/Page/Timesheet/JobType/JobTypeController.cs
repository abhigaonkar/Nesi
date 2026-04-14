using NESI.BLL.Pages.Timesheet.BreakTime;
using NESI.BLL.Pages.Timesheet.JobType;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet.JobType
{
    [PageAuthorizationFilter(28)]
    [RoutePrefix("api/Page/Timesheet/Jobtype")]
    public class JobTypeController : EmployeeController
    {
        private readonly IJobTypeService _iJobTypeService = new JobTypeService();

        [Route("Jobtype")]
        [HttpGet]
        public IHttpActionResult Get([FromUri] JobTypeRecordQueryParameter input)
        {
            var info = this._iJobTypeService.GetJobTypeInfo(input);
            return Ok(info);
        }
    }
}
