using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Core.Employee;
namespace NESI.WebAPI.Controllers.API.Page.Timesheet
{
    [RoutePrefix("api/Page/Timesheet/Projects")]
    public class TimeSheetProjectsController : TimeSheetControllerBase
    {
        [HttpGet]
        [Route("ProjectsList")]
        public IHttpActionResult GetInternalProjects()
        {
            var o = new BLL.Pages.Timesheet.Timesheet(CurrentUser).Get_InternalProjectList(CurrentUser)
                .Select(x => new { Label = x.name, Value = x.id });
            return Ok(o);
        }
        /// <summary>
        /// update project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult UpdateProject([FromBody] DTO.ViewModels.Page.TimeSheet.UpdateTimeSheetProject model)
        {
            return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).UpdateProject(CurrentUser, model)));
        }
        /// <summary>
		/// insert project
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult NewProject([FromBody] DTO.ViewModels.Page.TimeSheet.InsertTimeSheetProject model)
        {
            return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).New_Project(CurrentUser, model)));
        }

    }
}