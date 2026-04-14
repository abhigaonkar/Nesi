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
using bllT = NESI.BLL.Pages.Reports.MasterQuoteGrid;
using dtoT = NESI.DTO.ViewModels.Page.Reports.MasterQuoteGrid;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    [PageAuthorizationFilter(78)]
    [RoutePrefix("api/Page/MasterQuoteGrid")]
    public class MasterQuoteGridController : EmployeeGridControllerBase<dtoT, bllT>
    {
        public MasterQuoteGridController()
        {
            key = "quote";
            moduleName = "MasterQuoteGrid";
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

        [Route("ExporttoExcel")]
        [HttpPost]
        public Task<HttpResponseMessage> ExporttoExcel([FromBody] BodyParams param)
        {
            return ExporToExcel(param);
        }

        [Route("ExporttoPdf")]
        [HttpPost]
        public Task<HttpResponseMessage> ExporttoPdf([FromBody] BodyParams param)
        {
            return ExportToPdf(param);
        }


        [HttpGet]
        [Route("Chance")]
        public IHttpActionResult GetChances()
        {
            var bllt =  new bllT(this.CurrentEmployee);
            var r = bllt.GetChances();
            return Ok(r);
        }

        [Route("BulkEditAsset")]
        [HttpPost]
        public async Task<List<KeyValuePair<string, bool>>> BulkEdit(DTO.ViewModels.Page.Reports.MasterQuoteGrid[] quotes)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var list = bllt.UpdateMultipleQuotes(quotes);
            return list;
        }

        [Route("Edit")]
        [HttpPost]
        public async Task<IHttpActionResult> EditModel(DTO.ViewModels.Page.Reports.MasterQuoteGrid model)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var result = bllt.UpdateSingleQuote(model);
            return Ok(result);
        }

        [Route("Note")]
        [HttpPost]
        public async Task<IHttpActionResult> AddNote(DTO.ViewModels.Page.Reports.MasterQuoteGrid model)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var result = bllt.AddNote(model);
            return Ok(result);
        }

        [Route("updateChance")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateChance(DTO.ViewModels.Page.Reports.MasterQuoteGrid model)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var result = bllt.UpdateChance(model);
            return Ok(result);
        }
    }
}
