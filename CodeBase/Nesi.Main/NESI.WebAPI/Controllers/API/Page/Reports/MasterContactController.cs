using System.Web.Http;
using NESI.WebAPI.Controllers.Base;
using System.Collections.Generic;
using NESI.DataProcessor;
using System.Net.Http;
using System;
using NESI.Common;
using System.Threading.Tasks;
using NESI.Common.Interface;
using NESI.DTO.ViewModels.Page;
using Export;
using NESI.DTO.ViewModels.Page.Reports;
using System.Linq;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    public class PageMasterContactController : EmployeeController, IReportViewer<DTO.ViewModels.Page.Reports.MasterContact>, IReportExporter,IReportEditor<DTO.ViewModels.Page.Reports.MasterContact>
    {
        /// <summary>
        /// Search Master Contact based on quary parameters
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Page/MasterContact/Search")]
        public async Task<Response<DTO.ViewModels.Page.Reports.MasterContact>> Search([FromBody] BodyParams param)
        {
            return await SearchResults(param);
        }

        /// <summary>
        /// Get default schema and Report definition
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Page/MasterContact/")]
        public async Task<Report> GetSchema()
        {
            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterContact>();
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

        [Route("api/Page/MasterContact/GetDistinct")]
        [HttpPost]
        public async Task<List<string>> GetDistinct([FromBody] BodyParams param)
        {
            NESI.BLL.Pages.Reports.MasterContact wo = new BLL.Pages.Reports.MasterContact(this.CurrentUser, param);

            var data = DataProcesserEngine
                .GetDistinct<DTO.ViewModels.Page.Reports.MasterContact,
                    NESI.BLL.Pages.Reports.MasterContact>(wo, param);

            return data;
        }

        [Route("api/Page/MasterContact/ExporttoExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ExporttoExcel([FromBody] BodyParams param)
        {
//            QueryParam param = new QueryParam(Request.GetQueryNameValuePairs(), null);
            param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
            param.page_count = 1;
            const string moduleName = "MasterContact";
            var grouparray = (string[])param.column_groupBy.Clone();
            param.column_groupBy = new string[] { };
            var res = await this.SearchResults(param);

            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterContact>();
            var rep = objT.GetSchema();

            List<Schema> schemList = new List<Schema>();
            param.columns.ForEach(s =>
            {
                var pp = rep.columns.Where(x => x.name == s).FirstOrDefault();
                schemList.Add(new Schema { name = s, header = pp.header });
            });

            return ExportHelper.ExportToExcelInit(res, param, grouparray, "excelfile", moduleName, schemList.ToArray());
        }

        [Route("api/Page/MasterContact/ExporttoPdf")]
        [HttpPost]
        public async Task<HttpResponseMessage> ExporttoPdf([FromBody] BodyParams param)
        {
            //            QueryParam param = new QueryParam(Request.GetQueryNameValuePairs(), null);
            var masterContactObj = new BLL.Pages.Reports.MasterContact(this.CurrentUser, param);
            var res = masterContactObj.GetDataFromStore();

            param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
            param.page_count = 1;
            var grouparray = (string[])param.column_groupBy.Clone();
            param.column_groupBy = new string[] { };
            //var res = await this.SearchResults(param);

            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterContact>();
            var rep = objT.GetSchema();

            List<Schema> schemList = new List<Schema>();
            param.columns.ForEach(s =>
            {
                var pp = rep.columns.Where(x => x.name == s).FirstOrDefault();
                schemList.Add(new Schema { name = s, header = pp.header });
            });
            return ExportHelper.ExportToPDFInit(res, param, grouparray, "pdffile", "MasterContact", schemList.ToArray());
        }

        /// <summary>
        /// Edits Asset based on model posted
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/Page/MasterContact/EditContact")]
        [HttpPost]
        public async Task<bool> EditModel(DTO.ViewModels.Page.Reports.MasterContact model)
        {
            NESI.BLL.Pages.Reports.MasterContact cc = new NESI.BLL.Pages.Reports.MasterContact(this.CurrentUser, null);

            int id = await cc.EditModel(model);

            return id > 0;
        }


        /// <summary>
        /// Bulk edit Assets based on array of model posted
        /// </summary>
        /// <param name="cvms">array of model</param>
        /// <returns></returns>
        [Route("api/Page/MasterContact/BulkEditContact")]
        [HttpPost]
        public async Task<List<KeyValuePair<string, bool>>> BulkEdit(DTO.ViewModels.Page.Reports.MasterContact[] cvms)
        {
            List<KeyValuePair<string, bool>> list = new List<KeyValuePair<string, bool>>();

            NESI.BLL.Pages.Reports.MasterContact cc = new NESI.BLL.Pages.Reports.MasterContact(this.CurrentUser, null);
            foreach (var cvm in cvms)
            {
                int id = await cc.EditModel(cvm);
                bool status = false;
                if (id != 0)
                    status = true;  //success
                else
                    status = false; //failure

                list.Add(new KeyValuePair<string, bool>(Convert.ToString(id), status));
            }

            return list;
        }

        /// <summary>
        /// Common Method to get search Results both for serach and exports
        /// </summary>
        /// <param name="param">Query paarmeter object</param>
        /// <returns></returns>
        public async Task<Response<DTO.ViewModels.Page.Reports.MasterContact>> SearchResults(BodyParams param)
        {
            //if (param == null)
            //    param = new QueryParam(Request.GetQueryNameValuePairs(), null);

            NESI.BLL.Pages.Reports.MasterContact wobl = new BLL.Pages.Reports.MasterContact(this.CurrentUser, param) {SetColumnList = param.columns};

            var data = DataProcesserEngine
                .GetProcessedDataFromDatatable<DTO.ViewModels.Page.Reports.MasterContact,
                    NESI.BLL.Pages.Reports.MasterContact>(param, wobl);

            return data;

        }

        public Task<int> CreateModel(DTO.ViewModels.Page.Reports.MasterContact model)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteModel(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<MasterContact>> Search()
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ExporttoPdf(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ExporttoExcel(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<Response<MasterContact>> Search(List<string> listParams)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetDistinct()
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ExporttoExcel(string fileName, List<string> columnList)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ExporttoPdf(string fileName, List<string> columnList)
        {
            throw new NotImplementedException();
        }
    }
}
