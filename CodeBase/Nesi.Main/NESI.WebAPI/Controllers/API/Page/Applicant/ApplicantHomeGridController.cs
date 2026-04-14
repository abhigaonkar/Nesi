using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NESI.Common;
using NESI.DTO.ViewModels.Core;
using NESI.WebAPI.Infrastructures.Filters;
using NESI.WebAPI.Controllers.Base;
using bllT = NESI.BLL.Pages.Applicant.ApplicantHomeGrid;
using dtoT = NESI.DTO.ViewModels.Page.Applicant.ApplicantHomeGrid;

namespace NESI.WebAPI.Controllers.API.Page.Applicant
{
	[PageAuthorizationFilter(138)]
	[RoutePrefix("api/Page/ApplicantHomeGrid")]
	public class ApplicantHomeGridController : EmployeeGridControllerBase<dtoT, bllT>
	{

		public ApplicantHomeGridController()
		{
			key = "id";
			moduleName = "ApplicantHomeGrid";
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
		[Route("Delete/{id}")]
		public IHttpActionResult Delete(int id)
		{
			var o = new bllT(CurrentEmployee);
			return OkD(o.Delete(id));
		}

		[HttpPost]
		[Route("Update/{id}")]
		public IHttpActionResult Delete([FromBody] LabelValueString model, int id)
		{
			var o = new bllT(CurrentEmployee);
			return OkD(o.Update(model.Label, model.Value, id));
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
