using NESI.BLL.Pages.Reports;
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
using bllT = NESI.BLL.Pages.Reports.CustomerLoginHistoryGrid;
using dtoT = NESI.DTO.ViewModels.Page.Reports.CustomerLoginHistoryGrid;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
	[PageAuthorizationFilter(157)]
	[RoutePrefix("api/Page/CustomerLoginHistoryGrid")]
	public class CustomerLoginHistoryGridController : EmployeeGridControllerBase<dtoT, bllT>
	{
		public CustomerLoginHistoryGridController()
		{
			key = "Customer_ID";
			moduleName = "CustomerLoginHistoryGrid";
		}

		protected override bllT GetObject(BodyParams param, params object[] extra_params)
		{
			param.keyColumn = key;
			var customer_id = -1;
			if (extra_params != null && extra_params.Length == 1)
			{
				customer_id = Convert.ToInt32(extra_params[0]);
			}
			else if (param.queryparam.Length == 1)
			{
				customer_id = Convert.ToInt32(param.queryparam[0].value);
			}

			return new bllT(this.CurrentEmployee, param, customer_id) { SetColumnList = param.columns };
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
	}
}
