using NESI.Common;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using bllT = NESI.BLL.Pages.Reports.JobCostReport;
using dtoT = NESI.DTO.ViewModels.Page.Reports.JobCostReport;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    [PageAuthorizationFilter(59)]
    [RoutePrefix("api/Page/JobCostReport")]
    public class JobCostReportController : EmployeeGridControllerBase<dtoT, bllT>
    {
        public JobCostReportController()
        {
            key = "woprog_id";
            moduleName = "JobCostReport";
        }

        protected override bllT GetObject(BodyParams param, params object[] extra_params)
        {
            param.keyColumn = key;

            int business_unit = 0;

            if (extra_params != null && extra_params.Length == 1)
            {
                business_unit = Convert.ToInt32(extra_params[0]);
            }
            else if (param.queryparam.Length == 1)
            {
                business_unit = Convert.ToInt32(param.queryparam[0].value);
            }

            var r = new bllT(this.CurrentEmployee, param, business_unit) { SetColumnList = param.columns };
            return r;
        }

        [Route("")]
        [HttpGet]
        public new Task<Report> GetSchema()
        {
            return base.GetSchema();
        }

        [HttpPost]
        [Route("Search")]
        public Response<dtoT> SearchAll([FromBody] BodyParams param)
        {
            var o = SearchResults(GetObject(param), param);
            return o;
        }

        [Route("GetDistinct")]
        [HttpPost]
        public IHttpActionResult GetDistinct([FromBody] BodyParams param)
        {
            return Ok(base.GetDistinct(GetObject(param), param));
        }

        /// <summary>
        /// Exports Search results in Excel file
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route("ExporttoExcel")]
        [HttpPost]
        public Task<HttpResponseMessage> ExporttoExcel([FromBody] BodyParams param)
        {
            return ExporToExcel(param);
        }

        /// <summary>
        /// Exports serach result in pdf file
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route("ExporttoPdf")]
        [HttpPost]
        public Task<HttpResponseMessage> ExporttoPdf([FromBody] BodyParams param)
        {
            return ExportToPdf(param);
        }
    }
}
