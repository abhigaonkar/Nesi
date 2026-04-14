using NESI.BLL.Pages.Reports;
using NESI.Common;
using NESI.DTO.ViewModels.Core;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using bllT = NESI.BLL.Pages.Reports.MasterOutstandingInvoicesGrid;
using dtoT = NESI.DTO.ViewModels.Page.Reports.MasterOutstandingInvoicesGrid;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    [PageAuthorizationFilter(113)]
    [RoutePrefix("api/Page/MasterOutstandingInvoicesGrid")]
    public class MasterOutstandingInvoicesGridController : EmployeeGridControllerBase<dtoT, bllT>
    {
        public MasterOutstandingInvoicesGridController()
        {
            key = "wOProg_ID";
            moduleName = "MasterOutstandingInvoicesGrid";
        }

        protected override bllT GetObject(BodyParams param, params object[] extra_params)
        {
            string origin = "";
            string cust_id = "";
            string te_id = "";

            param.keyColumn = key;

            if (param.queryparam.Length == 3)
            {
                origin = Convert.ToString(param.queryparam[0].value);
                cust_id = Convert.ToString(param.queryparam[1].value);
                te_id = Convert.ToString(param.queryparam[1].value);
            }

            return new bllT(this.CurrentEmployee, param, cust_id, te_id, origin) { SetColumnList = param.columns };
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

        [Route("confirmPaidDate")]
        [HttpPost]
        public async Task<IHttpActionResult> ConfirmPaidDate(ConfirmPaidList confirmedList)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var result = bllt.ConfirmPaidDate(confirmedList);
            return Ok(result);
        }

        [Route("invoiceNotes/{woID}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetInvoiceNotes(long woID)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var result = bllt.GetInvoiceNotes(woID);
            return Ok(result);
        }

        [Route("addOrUpdateNote")]
        [HttpPost]
        public async Task<IHttpActionResult> AddOrUpdateNote(EditNote model)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var result = bllt.AddOrUpdateNote(model);
            return Ok(result);
        }

        [Route("Edit")]
        [HttpPost]
        public async Task<IHttpActionResult> EditModel(DTO.ViewModels.Page.Reports.MasterOutstandingInvoicesGrid model)
        {
            var bllt = new bllT(this.CurrentEmployee);
            var result = bllt.UpdateSingleOutStandingInvoice(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("customerStatus")]
        public IHttpActionResult CustomerStatus()
        {
            var bllt = new bllT(this.CurrentEmployee);
            var r = bllt.GetCustomerStatus();
            return Ok(r);
        }

        [HttpGet]
        [Route("invoiceStatus")]
        public IHttpActionResult InvoiceStatus()
        {
            var bllt = new bllT(this.CurrentEmployee);
            var r = bllt.GetInvoiceStatus();
            return Ok(r);
        }

    }
}
