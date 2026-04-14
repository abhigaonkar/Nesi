using NESI.BLL.Pages.AssetRate;
using NESI.BLL.Pages.Timesheet.BreakTime;
using NESI.BLL.Pages.Timesheet.JobType;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.AssetRate
{
    [PageAuthorizationFilter(10)]
    [RoutePrefix("api/Page/AssetRate")]
    public class AssetCustomerRateController : EmployeeController
    {
        private readonly IAssetCustomerRate _service = new AssetCustomerRateService();

        [Route("List")]
        [HttpGet]
        public IHttpActionResult Get([FromUri] AssetCustomerRateQueryParameter input)
        {
            var res = this._service.Get(input);
            return Ok(res);
        }

        [HttpPost]
        [Route("Save")]
        public IHttpActionResult Update([FromBody] AssetCustomerRateUpdateParameter model)
        {
            var result = this._service.Update(model);
            return OkD(result);
        }

        [Route("BusinessUnit")]
        [HttpGet]
        public IHttpActionResult GetBusinessUnits([FromUri] GetVisiableBUParameter input)
        {
            var list = this._service.getAvailableBusinessUnits(input);
            return Ok(list);
        }

        [HttpPost]
        [Route("Delete")]
        public IHttpActionResult DeleteCustomizedAssetRate([FromBody] AssetCustomerRateDeleteParameter model)
        {
            var result = this._service.Delete(model);
            return OkD(result);
        }
    }

}
