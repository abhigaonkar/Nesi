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
using bllT = NESI.BLL.Pages.Reports.MasterCustomersGrid;
using dtoT = NESI.DTO.ViewModels.Page.Reports.MasterCustomersGrid;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    [PageAuthorizationFilter(83)]
    [RoutePrefix("api/Page/MasterCustomersGrid")]
    public class MasterCustomersGridController :  EmployeeGridControllerBase<dtoT, bllT>
    {
        public MasterCustomersGridController()
        {
            key = "ID";
            moduleName = "MasterCustomersGridController";
        }

        protected override bllT GetObject(BodyParams param, params object[] extra_params)
        {
            param.keyColumn = key;
            DateTime used_from_dt = DateTime.Now;
            DateTime used_to_dt = DateTime.Now;

            if (param.queryparam.Length == 2)
            {
                used_from_dt = Convert.ToDateTime(param.queryparam[0].value);
                used_to_dt = Convert.ToDateTime(param.queryparam[1].value);
            }

            used_from_dt = DateTime.Now;
            used_to_dt = DateTime.Now;

            return new bllT(this.CurrentEmployee, param, used_from_dt, used_to_dt) { SetColumnList = param.columns };
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
