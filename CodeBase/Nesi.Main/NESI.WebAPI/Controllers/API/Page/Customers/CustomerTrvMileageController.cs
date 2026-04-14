using NESI.BLL.Pages.AssetRate;
using NESI.BLL.Pages.NewLaborRates;
using NESI.BLL.Pages.Timesheet.BreakTime;
using NESI.BLL.Pages.Timesheet.JobType;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
    [PageAuthorizationFilter(10)]
    [RoutePrefix("api/Page/mileage")]
    public class CustomerTrvMileageController : EmployeeController
    {
        private readonly INewLaborRate _service = new NewLaborRateService();

        [Route("List")]
        [HttpGet]
        public IHttpActionResult Get([FromUri] GetNewLaborRateListParameter input)
        {
            var res = this._service.GetNewLaborRateList(input);
            return Ok(res);
        }

        [HttpPost]
        [Route("Save")]
        public IHttpActionResult Update([FromBody] UpdateParameter model)
        {
            var result = this._service.CreateNewLaborRate(model);
            return OkD(result);
        }

        [HttpPost]
        [Route("Delete")]
        public IHttpActionResult DeleteTrvMileageChargeout([FromBody] DeleteParameter model)
        {
            var result = this._service.DeleteNewLaborRate(model);
            return OkD(result);
        }

    }
}