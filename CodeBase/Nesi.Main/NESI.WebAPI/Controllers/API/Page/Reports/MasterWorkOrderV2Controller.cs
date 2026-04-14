using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Export;
using NESI.Common;
using NESI.Common.Interface;
using NESI.DataProcessor;
using NESI.DTO.ViewModels.Page.Reports;
using NESI.WebAPI.Controllers.Base;
namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    public class MasterWorkOrderV2Controller : EmployeeController, IReportViewer<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>
    {
        [HttpGet]
        [Route("api/Page/MasterWorkOrderV2/")]
        public Report GetSchema()
        {
            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>();
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

        [HttpPost]
        [Route("api/Page/MasterWorkOrderV2/Search/")]
        public async Task<Response<MasterWorkOrderV2>> Search([FromBody] BodyParams param)
        {
            return await SearchResults(param);
        }

        [Route("api/Page/MasterWorkOrderV2/GetDistinct")]
        [HttpPost]
        public async Task<List<string>> GetDistinct([FromBody] BodyParams param)
        {
            //            QueryParam param = new QueryParam(Request.GetQueryNameValuePairs(), null);
            NESI.BLL.Pages.Reports.MasterWorkOrderV2 wov2 = new BLL.Pages.Reports.MasterWorkOrderV2(this.CurrentUser, param);
			//	var data = wov2.GetDistinct(param);
			var data = DataProcesserEngine
				.GetDistinct<DTO.ViewModels.Page.Reports.MasterWorkOrderV2,
					NESI.BLL.Pages.Reports.MasterWorkOrderV2>(wov2, param);
			return data;
        }

        //[Route("api/Page/MasterWorkOrderV2/Edit")]
        //[HttpPost]
        //public async Task<bool> EditModel(DTO.ViewModels.Page.Reports.MasterWorkOrderV2 model)
        //{
        //   //  QueryParam param = new QueryParam(Request.GetQueryNameValuePairs(), null);
        //    NESI.BLL.Pages.Reports.MasterWorkOrderV2 woprog = new BLL.Pages.Reports.MasterWorkOrderV2(this.CurrentUser, null);

        //    int id = await woprog.EditModel(model);

        //    return id > 0;
        //}

        [Route("api/Page/MasterWorkOrderV2/Edit")]
        [HttpPost]
        public async Task<IHttpActionResult> EditModel2(DTO.ViewModels.Page.Reports.MasterWorkOrderV2 model)
        {
            NESI.BLL.Pages.Reports.MasterWorkOrderV2 woprog = new BLL.Pages.Reports.MasterWorkOrderV2(this.CurrentUser, null);

            var data = await woprog.EditModel2(model);

            return Ok(data);
        }

        /// <summary>
        /// Bulk edit Assets based on array of model posted
        /// </summary>
        /// <param name="cvms">array of model</param>
        /// <returns></returns>
        [Route("api/Page/MasterWorkOrderV2/BulkEditAsset")]
        [HttpPost]
        public async Task<List<KeyValuePair<string, bool>>> BulkEdit(DTO.ViewModels.Page.Reports.MasterWorkOrderV2[] cvms)
        {
            List<KeyValuePair<string, bool>> list = new List<KeyValuePair<string, bool>>();
            NESI.BLL.Pages.Reports.MasterWorkOrderV2 CC = new NESI.BLL.Pages.Reports.MasterWorkOrderV2(this.CurrentUser, null);
            string s2 = string.Empty;
            string status1 = string.Empty;
            foreach (var cvm in cvms)
            {
                var r = await CC.EditModel2(cvm);
                var data = r.Extra as DTO.ViewModels.Page.Reports.MasterWorkOrderV2;
                Boolean status = data.Okay;
                list.Add(new KeyValuePair<string, bool>(Convert.ToString(data.woprog_id), status));
            }
            return list;
        }
        

        [Route("api/Page/MasterWorkOrderV2/ExporttoExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ExporttoExcel([FromBody] BodyParams param)
        {
            //            QueryParam param = new QueryParam(Request.GetQueryNameValuePairs(), null);
            param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
            param.page_count = 1;
            const string moduleName = "MasterWorkOrderV2";
            var grouparray = (string[])param.column_groupBy.Clone();
            param.column_groupBy = new string[] { };
            var res = await this.SearchResults(param);

            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>();
            var rep = objT.GetSchema();

            return ExportHelper.ExportToExcelInit(res, param, grouparray, "excelfile", "MasterWorkOrderV2", rep.columns);
        }

        [Route("api/Page/MasterWorkOrderV2/ExporttoPdf/{fileName}")]
        [HttpGet]
        public async Task<HttpResponseMessage> ExporttoPdf(string fileName, [FromBody] BodyParams param)
        {
            //            QueryParam param = new QueryParam(Request.GetQueryNameValuePairs(), null);
            param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
            param.page_count = 1;
            var grouparray = (string[])param.column_groupBy.Clone();
            param.column_groupBy = new string[] { };
            var res = await this.SearchResults(param);

            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>();
            var rep = objT.GetSchema();

            return ExportHelper.ExportToPDFInit(res, param, grouparray, fileName, "MasterWorkOrderV2", rep.columns);
        }

        private async Task<Response<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>> SearchResults(BodyParams param = null)
        {
            //            if (param == null)
            //                param = new QueryParam(Request.GetQueryNameValuePairs(), null);

            NESI.BLL.Pages.Reports.MasterWorkOrderV2 wobl = new BLL.Pages.Reports.MasterWorkOrderV2(this.CurrentUser, param) { SetColumnList = param.columns};

            var data = DataProcesserEngine
                .GetProcessedDataFromDatatable<DTO.ViewModels.Page.Reports.MasterWorkOrderV2,
                    NESI.BLL.Pages.Reports.MasterWorkOrderV2>(param, wobl);

            data.extra = new { wobl.GroupSummary };
            return data;
        }

        Task<Report> IReportViewer<MasterWorkOrderV2>.GetSchema()
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateModel(MasterWorkOrderV2 model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteModel(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetDistinct()
        {
            throw new NotImplementedException();
        }

        public Task<Response<MasterWorkOrderV2>> Search()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("api/Page/MasterWorkOrderV2/Ram")]
        public IHttpActionResult Geram()
        {
            NESI.BLL.Pages.Reports.MasterWorkOrder wobl = new BLL.Pages.Reports.MasterWorkOrder(this.CurrentUser, null);
            return Ok(wobl.getAllRams());
        }

        [HttpPost]
        [Route("api/Page/MasterWorkOrderV2/UpdateRam")]
        public IHttpActionResult UpdateStatus([FromBody] DTO.ViewModels.Core.DataIdInt model)
        {
            NESI.BLL.Pages.Reports.MasterWorkOrder wobl = new BLL.Pages.Reports.MasterWorkOrder(this.CurrentUser, null);
            var r = wobl.UpdateRamStatus(model.id, model.value);
            return Ok(r);
        }


        public Task<Response<MasterWorkOrderV2>> Search(List<string> listParams)
        {
            throw new NotImplementedException();
        }
    }
}
