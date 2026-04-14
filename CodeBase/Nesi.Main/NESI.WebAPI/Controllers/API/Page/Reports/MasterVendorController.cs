using System.Web.Http;
using NESI.WebAPI.Controllers.Base;
using System.Collections.Generic;
using NESI.DataProcessor;
using System.Net.Http;
using System;
using NESI.Common;
using NESI.DTO.ViewModels.Page;
using System.Threading.Tasks;
using NESI.Common.Interface;
using Export;
using NESI.DTO.ViewModels.Page.Reports;
using System.Linq;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    public class PageMasterVendorController : EmployeeController, IReportViewer<DTO.ViewModels.Page.Reports.MasterVendor>, IReportEditor<DTO.ViewModels.Page.Reports.MasterVendor>, IReportExporter
    {
        readonly string key = "vendor_ID";

        /// <summary>
        /// Search Master Vendor based on quary parameters
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Page/MasterVendor/Search")]
        public async Task<Response<DTO.ViewModels.Page.Reports.MasterVendor>> Search([FromBody] BodyParams param)
        {
            return await SearchResults(param);
        }

        /// <summary>
        /// Get default schema and Report definition
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Page/MasterVendor/")]
        public async Task<Report> GetSchema()
        {
            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterVendor>();
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

        /// <summary>
        /// Get Distinct records for specified column in query param
        /// </summary>
        /// <returns></returns>
        ///
        [HttpPost]
        [Route("api/Page/MasterVendor/GetDistinct")]
        public async Task<List<string>> GetDistinct([FromBody] BodyParams param)
        {
            param.keyColumn = key;
            NESI.BLL.Pages.Reports.MasterVendor custbl = new BLL.Pages.Reports.MasterVendor(this.CurrentUser, param);

            var data = DataProcesserEngine
                .GetDistinct<DTO.ViewModels.Page.Reports.MasterVendor,
                    NESI.BLL.Pages.Reports.MasterVendor>(custbl, param);

            return data;
        }

        /// <summary>
        /// Exports Search results in Excel file
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route("api/Page/MasterVendor/ExporttoExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ExporttoExcel([FromBody] BodyParams param)
        {
            param.keyColumn = key;
            //            QueryParam param = new QueryParam(Request.GetQueryNameValuePairs(), key);
            param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
            param.page_count = 1;
            const string moduleName = "MasterVendor";
            var grouparray = (string[])param.column_groupBy.Clone();
            param.column_groupBy = new string[] { };
            var res = await this.SearchResults(param);

            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterVendor>();
            var rep = objT.GetSchema();

            List<Schema> schemList = new List<Schema>();
            param.columns.ForEach(s =>
            {
                var pp = rep.columns.Where(x => x.name == s).FirstOrDefault();
                schemList.Add(new Schema { name = s, header = pp.header });
            });

            return ExportHelper.ExportToExcelInit(res, param, grouparray, "excelfile", moduleName, schemList.ToArray());
        }

        /// <summary>
        /// Exports serach result in pdf file
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route("api/Page/MasterVendor/ExporttoPdf")]
        [HttpPost]
        [AllowAnonymous] // Will find a way to send external URL with token from frontend and remove this attribute.
        public async Task<HttpResponseMessage> ExporttoPdf([FromBody] BodyParams param)
        {
            param.keyColumn = key;
            //            QueryParam param = new QueryParam(Request.GetQueryNameValuePairs(), key);
            var masterVendorObj = new BLL.Pages.Reports.MasterVendor(this.CurrentUser, param);
            var res = masterVendorObj.GetDataFromStore();

            param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
            param.page_count = 1;
            var grouparray = (string[])param.column_groupBy.Clone();
            param.column_groupBy = new string[] { };
            //var res = await this.SearchResults(param);

            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.MasterVendor>();
            var rep = objT.GetSchema();

            List<Schema> schemList = new List<Schema>();
            param.columns.ForEach(s =>
            {
                var pp = rep.columns.Where(x => x.name == s).FirstOrDefault();
                schemList.Add(new Schema { name = s, header = pp.header });
            });

            return ExportHelper.ExportToPDFInit(res, param, grouparray, "pdffile", "MasterVendor", schemList.ToArray());
        }

        /// <summary>
        /// Edits Vendor based on model posted
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/Page/MasterVendor/EditVendor")]
        [HttpPost]
        public async Task<bool> EditModel(DTO.ViewModels.Page.Reports.MasterVendor model)
        {
            var CC = new NESI.BLL.Pages.Reports.MasterVendor(this.CurrentUser, null);

            int id = await CC.EditModel(model);

            return id > 0;
        }

        /// <summary>
        /// Bulk edit vendor records based on array of model posted
        /// </summary>
        /// <param name="cvms">array of model</param>
        /// <returns></returns>

        [Route("api/Page/MasterVendor/BulkEditVendor")]
        [HttpPost]
        public async Task<List<KeyValuePair<string, bool>>> BulkEdit(DTO.ViewModels.Page.Reports.MasterVendor[] cvms)
        {
            List<KeyValuePair<string, bool>> list = new List<KeyValuePair<string, bool>>();
            NESI.BLL.Pages.Reports.MasterVendor CC = new NESI.BLL.Pages.Reports.MasterVendor(this.CurrentUser, null);
            string s2 = string.Empty;
            string status1 = string.Empty;
            foreach (var cvm in cvms)
            {
                int id = await CC.EditModel(cvm);
                bool status = false;
                if (id != 0)
                {
                    status = false;
                }
                else
                {
                    status = true;

                }
                list.Add(new KeyValuePair<string, bool>(Convert.ToString(id), status));
            }
            return list;
        }

        /// <summary>
        /// Common Method to get search Results both for serach and exports
        /// </summary>
        /// <param name="param">Query paarmeter object</param>
        /// <returns></returns>
        public async Task<Response<DTO.ViewModels.Page.Reports.MasterVendor>> SearchResults([FromBody] BodyParams param)
        {
            NESI.BLL.Pages.Reports.MasterVendor custbl = new BLL.Pages.Reports.MasterVendor(this.CurrentUser, param) { SetColumnList = param.columns };
            param.keyColumn = key;
            //if (param == null)
            //    param = new QueryParam(Request.GetQueryNameValuePairs(), key);
            
            var data = DataProcesserEngine
                .GetProcessedDataFromDatatable<DTO.ViewModels.Page.Reports.MasterVendor,
                    NESI.BLL.Pages.Reports.MasterVendor>(param, custbl);

            return data;

        }

        public Task<int> CreateModel(DTO.ViewModels.Page.Reports.MasterVendor model)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteModel(string id)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ExporttoExcel(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ExporttoPdf(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<Response<MasterVendor>> Search()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetDistinct()
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ExporttoPdf(string fileName, List<string> columnList)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ExporttoExcel(string fileName, List<string> columnList)
        {
            throw new NotImplementedException();
        }

        public Task<Response<MasterVendor>> Search(List<string> listParams)
        {
            throw new NotImplementedException();
        }
    }
}
