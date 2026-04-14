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
using bllT = NESI.BLL.Pages.Reports.MasterPurchasesGrid;
using dtoT = NESI.DTO.ViewModels.Page.Reports.MasterPurchasesGrid;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    [PageAuthorizationFilter(102)]
    [RoutePrefix("api/Page/MasterPurchasesGrid")]
    public class MasterPurchasesGridController : EmployeeGridControllerBase<dtoT, bllT>
    {
        public MasterPurchasesGridController()
        {
            key = "poprog_id";
            moduleName = "MasterPurchasesGrid";
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

        [HttpGet]
        [Route("apStatus")]
        public IHttpActionResult GetChances()
        {
            var bllt = new bllT(this.CurrentEmployee);
            var r = bllt.GetApStatus();
            return Ok(r);
        }

        [Route("updateApStatus")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateApStatus(DTO.ViewModels.Page.Reports.MasterPurchasesGrid model)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var result = bllt.UpdateApStatus(model);
            return Ok(result);
        }

        [Route("Edit")]
        [HttpPost]
        public async Task<IHttpActionResult> EditModel(DTO.ViewModels.Page.Reports.MasterPurchasesGrid model)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var result = bllt.UpdateSingleMasterPurchase(model);
            return Ok(result);
        }

        [Route("BulkEditAsset")]
        [HttpPost]
        public async Task<List<KeyValuePair<string, bool>>> BulkEdit(DTO.ViewModels.Page.Reports.MasterPurchasesGrid[] purchases)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var list = bllt.UpdateMultiplePurchases(purchases);
            return list;
        }
    }
}
