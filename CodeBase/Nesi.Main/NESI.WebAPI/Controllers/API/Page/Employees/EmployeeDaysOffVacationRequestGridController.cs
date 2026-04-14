using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NESI.Common;
using NESI.WebAPI.Infrastructures.Filters;
using bllT = NESI.BLL.Pages.Employees.EmployeeDaysOffVacationRequestGrid;
using dtoT = NESI.DTO.ViewModels.Page.Employees.EmployeeDaysOffVacationRequestGrid;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[PageAuthorizationFilter(127)]
	[RoutePrefix("api/Page/EmployeeDaysOffVacationRequestGrid")]
	public class EmployeeDaysOffVacationRequestGridController : EmployeeGridControllerBase<dtoT, bllT>
	{

		public EmployeeDaysOffVacationRequestGridController()
		{
			key = "vacation_id";
			moduleName = "EmployeeDaysOffVacationRequestGrid";
		}

		protected override bllT GetObject(BodyParams param, params object[] extra_params)
		{
			param.keyColumn = key;
			var memberid = 0;
			if (extra_params != null && extra_params.Length == 1)
			{
				memberid = Convert.ToInt32(extra_params[0]);
			}
			else if (param.queryparam.Length == 1)
			{
				memberid = Convert.ToInt32(param.queryparam[0].value);
			}

			return new bllT(this.CurrentEmployee, param, memberid) { SetColumnList = param.columns };
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
