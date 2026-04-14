using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;
using NESI.WebAPI.Infrastructures.Filters;
using bllT = NESI.BLL.Pages.Employees.EmployeesHomeGrid;
using dtoT = NESI.DTO.ViewModels.Page.Employees.EmployeesHomeGrid;
using NESI.WebAPI.Controllers.Base;
 
namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[PageAuthorizationFilter(127)]
	[RoutePrefix("api/Page/EmployeesHomeGrid")]
	public class EmployeesHomeGridController : EmployeeGridControllerBase<dtoT, bllT>
	{

		public EmployeesHomeGridController()
		{
			key = "memberid";
			moduleName = "EmployeesHomeGrid";
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

			return SearchResults(GetObject(param), param);
		}

		[HttpPost]
		[Route("AdvanceSearch")]
		public IHttpActionResult AdvanceSearch([FromBody] DataString param)
		{
			return Ok(new bllT((Employee) CurrentUser).AdvanceSearch(param.Data));
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
