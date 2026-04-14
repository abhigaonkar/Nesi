using NESI.Common;
using NESI.DataProcessor;
using NESI.WebAPI.Controllers.Base;
using System.Collections.Generic;
using System.Web.Http;
using NESI.DTO.ViewModels.Page;
using System.Threading.Tasks;
using System.Net.Http;
using NESI.Common.Interface;
using System;
using NESI.DTO.ViewModels.Page.Reports;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    public class LocationController : EmployeeController, IReportViewer<DTO.ViewModels.Page.Reports.Address>
    {
        /// <summary>
        /// Search Customer based on quary parameters
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Page/Location/Search")]
        public async Task<Response<DTO.ViewModels.Page.Reports.Address>> Search([FromBody] BodyParams param)
        {
            return await SearchResults(param);
        }

        /// <summary>
        /// Common Method to get search Results both for serach and exports
        /// </summary>
        /// <param name="bodyParam">Query paarmeter object</param>
        /// <returns></returns>
        public async Task<Response<DTO.ViewModels.Page.Reports.Address>> SearchResults([FromBody] BodyParams bodyParam, QueryParam param = null)
        {
            if (param == null)
            {
                param = new QueryParam(Request.GetQueryNameValuePairs(), "address_id");
                bodyParam.column_filter = param.column_filter;
            }

            bodyParam.keyColumn = "address_id";

            var addressBl = new BLL.Pages.Reports.Address() { SetColumnList = bodyParam.columns };
            var data = DataProcesserEngine.GetProcessedData<DTO.ViewModels.Page.Reports.Address, BLL.Pages.Reports.Address>(bodyParam, addressBl);

            return data;

        }
        
        /// <summary>
        /// Get default schema and Report definition
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Page/Location")]
        public async Task<Report> GetSchema()
        {
            var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.Address>();
            var rep = objT.GetSchema();

            return rep as Report;
        }

        public Task<List<string>> GetDistinct()
        {
            throw new NotImplementedException();
        }

        public Task<Response<Address>> Search()
        {
            throw new NotImplementedException();
        }

        public Task<Response<Address>> Search(List<string> listParams)
        {
            throw new NotImplementedException();
        }
    }
}
