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
using bllT = NESI.BLL.Pages.Reports.WOLineGrid;
using dtoT = NESI.DTO.ViewModels.Page.Reports.WOLineGrid;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    [PageAuthorizationFilter(105)]
    [RoutePrefix("api/Page/WOLineGrid")]
    public class WOLineGridController : EmployeeGridControllerBase<dtoT, bllT>
    {
        public WOLineGridController()
        {
            key = "wo_detail_current_woprog_id";
            moduleName = "WOLineGrid";
        }

        protected override bllT GetObject(BodyParams param, params object[] extra_params)
        {
            param.keyColumn = key;
            return new bllT(this.CurrentEmployee, param) { SetColumnList = param.columns };
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

            bool canSee = this.CurrentUser.AuthenticatedForPrivilege(93);
            if (!canSee)
            {
                if (o.ColumnSummary != null && o.ColumnSummary["total_cost"] != null)
                {
                    var d = o.ColumnSummary;
                    d["total_cost"] = "0";
                }

                if (o.data != null)
                {
                    foreach (var item in o.data)
                    {
                        item.cost = 0;
                        item.total_cost = 0;
                    }
                }
            }

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
