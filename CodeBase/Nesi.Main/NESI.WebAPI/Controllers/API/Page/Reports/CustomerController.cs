using NESI.Common;
using NESI.DataProcessor;
using NESI.WebAPI.Controllers.Base;
using System.Collections.Generic;
using System.Web.Http;
using NESI.DTO.ViewModels.Page;
using System.Threading.Tasks;
using System.Net.Http;
using NESI.Common.Interface;
using NESI.DTO.ViewModels.Page.Reports;

namespace NESI.WebAPI.Reports
{
    public class CustomerController : EmployeeController, IReportViewer<DTO.ViewModels.Page.Reports.Customer>
    {


        
        public Task<List<string>> GetDistinct()
        {
            throw new System.NotImplementedException();
        }

      
        public Task<Report> GetSchema()
        {
            throw new System.NotImplementedException();
        }

        
        /// <summary>
        /// Search Customer based on quary parameters
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Page/Customer/Search")]
        public async Task<Response<DTO.ViewModels.Page.Reports.Customer>> Search([FromBody] BodyParams param)
        {
            return await SearchResults(param.globalfilter);
        }

        public Task<Response<Customer>> Search(List<string> listParams)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<Customer>> Search()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Common Method to get search Results both for serach and exports
        /// </summary>
        /// <param name="param">Query paarmeter object</param>
        /// <returns></returns>
        public async Task<Response<DTO.ViewModels.Page.Reports.Customer>> SearchResults(string filter)
        {
            NESI.BLL.Pages.Reports.Customer custbl = new NESI.BLL.Pages.Reports.Customer();

            var response = new Response<DTO.ViewModels.Page.Reports.Customer>();
            var lst = custbl.GetCustomers(filter);
            response.data = lst;
            return response;
        }
    }

}

    
