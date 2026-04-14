using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Export;
using NESI.Common;
using NESI.Common.Interface;
using NESI.DataProcessor;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Reports;
using NESI.WebAPI.Controllers.Base;


namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    public class MasterWorkOrderController : EmployeeController, IReportViewer<DTO.ViewModels.Page.Reports.MasterWorkOrder>
    {
        [HttpPost]
        [Route("api/Page/MasterWorkOrder/Search/")]
        public async Task<Response<DTO.ViewModels.Page.Reports.MasterWorkOrder>> Search([FromBody] BodyParams param)
        {
            return await SearchResults(param);
        }

        [HttpGet]
        [Route("api/Page/MasterWorkOrder/")]
        public Report GetSchema()
        {
            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterWorkOrder>();
            var rep = objT.GetSchema();

            if (rep != null)
            {
                if (rep.isPrivate)
                {
                    if (!this.CurrentUser.AuthenticatedForPrivilege(90001))
                        rep.exportToExcelEndPoint = string.Empty;

                    if (!this.CurrentUser.AuthenticatedForPrivilege(90002))
                        rep.exportToPdfEndPoint = string.Empty;
                }
            }

            return rep as Report;
        }

        [Route("api/Page/MasterWorkOrder/GetDistinct")]
        [HttpPost]
        public async Task<List<string>> GetDistinct([FromBody] BodyParams param)
        {
            NESI.BLL.Pages.Reports.MasterWorkOrder wo = new BLL.Pages.Reports.MasterWorkOrder(this.CurrentUser, param);

			// var data = wo.GetDistinct(param);
			var data = DataProcesserEngine
				.GetDistinct<DTO.ViewModels.Page.Reports.MasterWorkOrder,
					NESI.BLL.Pages.Reports.MasterWorkOrder>(wo, param);

			return data;
        }

        [Route("api/Page/MasterWorkOrder/ExporttoExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ExporttoExcel([FromBody] BodyParams param)
        {
            param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
            param.page_count = 1;
            var grouparray = (string[])param.column_groupBy.Clone();
            param.column_groupBy = new string[] { };
            var res = await this.SearchResults(param);

            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterWorkOrder>();
            var rep = objT.GetSchema();

            List<Schema> schemList = new List<Schema>();
            param.columns.ForEach(s =>
            {
                var pp = rep.columns.Where(x => x.name == s).FirstOrDefault();
                schemList.Add(new Schema { name = s, header = pp.header });
            });

            return ExportHelper.ExportToExcelInit(res, param, grouparray, "excelfile", "MasterWorkOrder", schemList.ToArray());
        }

        [Route("api/Page/MasterWorkOrder/ExporttoPdf")]
        [HttpPost]
        public async Task<HttpResponseMessage> ExporttoPdf([FromBody] BodyParams param)
        {
            param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
            param.page_count = 1;

            var masterWorkOrder1 = new BLL.Pages.Reports.MasterWorkOrder(this.CurrentUser, param);
            var res = masterWorkOrder1.GetDataFromStore();
            var grouparray = (string[])param.column_groupBy.Clone();

            param.column_groupBy = new string[] { };
            //var res = await this.SearchResults(param);

            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterWorkOrder>();
            var rep = objT.GetSchema();

            List<Schema> schemList = new List<Schema>();
            param.columns.ForEach(s =>
            {
                var pp = rep.columns.Where(x => x.name == s).FirstOrDefault();
                schemList.Add(new Schema { name = s, header = pp.header });
            });

            return ExportHelper.ExportToPDFInit(res, param, grouparray, "pdffile", "MasterWorkOrder", schemList.ToArray());
        }

        [Route("api/Page/MasterWorkOrder/Edit")]
        [HttpPost]
        public async Task<IHttpActionResult> EditModel(DTO.ViewModels.Page.Reports.MasterWorkOrder model)
        {
            NESI.BLL.Pages.Reports.MasterWorkOrder woprog = new BLL.Pages.Reports.MasterWorkOrder(this.CurrentUser, null);

            var data = await woprog.EditModel2(model);

            return Ok(data);
        }

        /// <summary>
        /// Bulk edit Assets based on array of model posted
        /// </summary>
        /// <param name="cvms">array of model</param>
        /// <returns></returns>
        [Route("api/Page/MasterWorkOrder/BulkEditAsset")]
        [HttpPost]
        public async Task<List<KeyValuePair<string, bool>>> BulkEdit(DTO.ViewModels.Page.Reports.MasterWorkOrder[] cvms)
        {
            List<KeyValuePair<string, bool>> list = new List<KeyValuePair<string, bool>>();
            NESI.BLL.Pages.Reports.MasterWorkOrder CC = new NESI.BLL.Pages.Reports.MasterWorkOrder(this.CurrentUser, null);
            string s2 = string.Empty;
            string status1 = string.Empty;
            foreach (var cvm in cvms)
            {
                var r = await CC.EditModel2(cvm);
                var data = r.Extra as DTO.ViewModels.Page.Reports.MasterWorkOrder;
                Boolean status = data.Okay;
                list.Add(new KeyValuePair<string, bool>(Convert.ToString(data.woprog_id), status));
            }
            return list;
        }

        private async Task<Response<DTO.ViewModels.Page.Reports.MasterWorkOrder>> SearchResults(BodyParams param = null)
        {
            NESI.BLL.Pages.Reports.MasterWorkOrder wobl = new BLL.Pages.Reports.MasterWorkOrder(this.CurrentUser, param) { SetColumnList = param.columns };
            
            var data = DataProcesserEngine
                .GetProcessedDataFromDatatable<DTO.ViewModels.Page.Reports.MasterWorkOrder,
                    NESI.BLL.Pages.Reports.MasterWorkOrder>(param, wobl);

            bool canSee = this.CurrentUser.AuthenticatedForPrivilege(81);
            if (!canSee)
            {
                var d = data.ColumnSummary;
                d["grossmargin"] = "0";
            }

            data.extra = new { wobl.GroupSummary };
           
            return data;
        }

        [HttpGet]
        [Route("api/Page/MasterWorkOrder/Ram")]
        public IHttpActionResult Geram()
        {
            NESI.BLL.Pages.Reports.MasterWorkOrder wobl = new BLL.Pages.Reports.MasterWorkOrder(this.CurrentUser, null);
            return Ok(wobl.getAllRams());
        }


        [HttpPost]
        [Route("api/Page/MasterWorkOrder/UpdateRam")]
        public IHttpActionResult UpdateStatus([FromBody] DTO.ViewModels.Core.DataIdInt model)
        {
            NESI.BLL.Pages.Reports.MasterWorkOrder wobl = new BLL.Pages.Reports.MasterWorkOrder(this.CurrentUser, null);
            var r = wobl.UpdateRamStatus(model.id, model.value);
            return Ok(r);
        }

        Task<Report> IReportViewer<MasterWorkOrder>.GetSchema()
        {
            throw new NotImplementedException();
        }

        public Task<Response<MasterWorkOrder>> Search(List<string> listParams)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetDistinct()
        {
            throw new NotImplementedException();
        }

        public Task<Response<MasterWorkOrder>> Search()
        {
            throw new NotImplementedException();
        }
    }
}
