using Export;
using NESI.BLL.Pages.Reports;
using NESI.Common;
using NESI.Common.Interface;
using NESI.DataProcessor;
using NESI.DTO.ViewModels.Page.Reports;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    [PageAuthorizationFilter(219)]
    public class AdvancePayReportController : EmployeeController, IReportViewer<DTO.ViewModels.Page.Reports.AdvancePayReportGrid>
    {
        [Route("api/Page/AdvancePayReport/GetDistinct")]
        [HttpPost]
        public async Task<List<string>> GetDistinct([FromBody] BodyParams param)
        {
            NESI.BLL.Pages.Reports.AdvancePayReport wov2 = new BLL.Pages.Reports.AdvancePayReport(this.CurrentUser, param);
            var data = wov2.GetDistinct(param);

            return data;
        }

        [HttpGet]
        [Route("api/Page/AdvancePayReport/")]
        public Report GetSchema()
        {
            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.AdvancePayReportGrid>();
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
        [Route("api/Page/AdvancePayReport/Search")]
        public async Task<Response<DTO.ViewModels.Page.Reports.AdvancePayReportGrid>> Search([FromBody] BodyParams param)
        {
            return await SearchResults(param);
        }

        [Route("api/Page/AdvancePayReport/ExporttoExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ExporttoExcel([FromBody] BodyParams param)
        {
            param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
            param.page_count = 1;
            var grouparray = (string[])param.column_groupBy.Clone();
            param.column_groupBy = new string[] { };
            var res = await this.SearchResults(param);

            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.AdvancePayReportGrid>();
            var rep = objT.GetSchema();

            List<Schema> schemList = new List<Schema>();
            param.columns.ForEach(s =>
            {
                var pp = rep.columns.Where(x => x.name == s).FirstOrDefault();
                schemList.Add(new Schema { name = s, header = pp.header });
            });

            return ExportHelper.ExportToExcelInit(res, param, grouparray, "excelfile", "AdvancePayReport", schemList.ToArray());
        }


        public Task<Response<AdvancePayReportGrid>> Search()
        {
            throw new NotImplementedException();
        }

        public Task<Response<AdvancePayReportGrid>> Search(List<string> listParams)
        {
            throw new NotImplementedException();
        }

        Task<Report> IReportViewer<AdvancePayReportGrid>.GetSchema()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetDistinct()
        {
            throw new NotImplementedException();
        }

        private async Task<Response<DTO.ViewModels.Page.Reports.AdvancePayReportGrid>> SearchResults(BodyParams param = null)
        {
            NESI.BLL.Pages.Reports.AdvancePayReport wobl = new BLL.Pages.Reports.AdvancePayReport(this.CurrentUser, param) { SetColumnList = param.columns };

            var data = DataProcesserEngine
                .GetProcessedDataFromDatatable<DTO.ViewModels.Page.Reports.AdvancePayReportGrid,
                    NESI.BLL.Pages.Reports.AdvancePayReport>(param, wobl);

            return data;
        }

    }
}
