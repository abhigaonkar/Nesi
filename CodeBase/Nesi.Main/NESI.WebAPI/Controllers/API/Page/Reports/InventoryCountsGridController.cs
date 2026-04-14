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
using bllT = NESI.BLL.Pages.Reports.InventoryCountsGrid;
using dtoT = NESI.DTO.ViewModels.Page.Reports.InventoryCountsGrid;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    [PageAuthorizationFilter(103)]
    [RoutePrefix("api/Page/InventoryCountsGrid")]
    public class InventoryCountsGridController : EmployeeGridControllerBase<dtoT, bllT>
    {
        public InventoryCountsGridController()
        {
            key = "master_id";
            moduleName = "InventoryCountsGrid";
        }

        protected override bllT GetObject(BodyParams param, params object[] extra_params)
        {
            param.keyColumn = key;

            int business_unit = 0;
            int with_location = 0;

            if (extra_params != null && extra_params.Length == 2)
            {
                business_unit = Convert.ToInt32(extra_params[0]);
                with_location = Convert.ToInt32(extra_params[1]);
            }
            else if (param.queryparam.Length == 2)
            {
                business_unit = Convert.ToInt32(param.queryparam[0].value);
                with_location = Convert.ToInt32(param.queryparam[1].value);
            }

            var r =  new bllT(this.CurrentEmployee, param, business_unit, with_location) { SetColumnList = param.columns };
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

            bool canSee = this.CurrentUser.AuthenticatedForPrivilege(58);
            if (!canSee)
            {
                foreach (var item in o.data)
                {
                    item.extdcost = 0;
                    item.cost = 0;
                }

                if (o.ColumnSummary != null) {
                    o.ColumnSummary["extdcost"] = "0";
                    o.ColumnSummary["mincost"] = "0";
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
