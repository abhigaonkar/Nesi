using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Export;
using NESI.Common;
using NESI.DataProcessor;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using bllT = NESI.BLL.Pages.WorkOrder.WorkOrderBusinessUnitDetail;
using dtoT = NESI.DTO.ViewModels.Page.WorkOrder.WorkOrderBusinessUnitDetail;

namespace NESI.WebAPI.Controllers.API.Page.WorkOrder
{
	[PageAuthorizationFilter(12)]
	[RoutePrefix("api/Page/WorkOrderBusinessUnitDetail")]
	public class WorkOrderDetailGridController : EmployeeGridControllerBase<dtoT, bllT>
	{
		public WorkOrderDetailGridController() : base()
		{
			key = "woprog_id";
			moduleName = "WorkOrderBusinessUnitDetail";
		}

		protected override bllT GetObject(BodyParams param, params object[] extra_param)
		{
			param.keyColumn = key;
			var buid = CurrentUser.BusinessUnitId;

			if (extra_param != null && extra_param.Length == 1)
			{
				buid = Convert.ToInt32(extra_param[0]);
			}
			else if (param.queryparam.Length == 1)
			{
				buid = Convert.ToInt32(param.queryparam[0].value);
			}
			return new bllT(this.CurrentEmployee, param, buid) { SetColumnList = param.columns };
		}

		[Route("")]
		[HttpGet]
		public new Task<Report> GetSchema()
		{
			return base.GetSchema();
		}

		[HttpPost]
		[Route("Search/{buid}")]
		[VisibleBusinessUnitFilter()]
		public Response<dtoT> Search([FromBody] BodyParams param, int buid)
		{
			return SearchResults(GetObject(param, buid), param);
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
