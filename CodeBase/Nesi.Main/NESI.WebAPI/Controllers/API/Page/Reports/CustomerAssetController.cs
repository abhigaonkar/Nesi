using System.Web.Http;
using NESI.WebAPI.Controllers.Base;
using System.Collections.Generic;
using NESI.DataProcessor;
using System.Net.Http;
using System;
using Export;
using NESI.Common;
using System.Threading.Tasks;
using NESI.Common.Interface;
using NESI.DTO.ViewModels.Page.Reports;
using System.Linq;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
	public class PageCustomerAssetController : ApiControllerBase, IReportViewer<DTO.ViewModels.Page.Reports.CustomerAsset>, IReportEditor<DTO.ViewModels.Page.Reports.CustomerAsset>, IReportExporter
	{

		readonly string key = "id";
		/// <summary>
		/// Search Customer assets based on quary parameters
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("api/Page/CustomerAsset/Search")]
		public async Task<Response<DTO.ViewModels.Page.Reports.CustomerAsset>> Search([FromBody] BodyParams param)
		{
			return await SearchResults(param);
		}

		/// <summary>
		/// Get default schema and Report definition
		/// </summary>
		/// <returns></returns>
		[Route("api/Page/CustomerAsset/")]
		[HttpGet]
		public async Task<Report> GetSchema()
		{
			var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.CustomerAsset>();
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
			var r = rep as Report;
			if (CurrentEmployee.ExtraType == "customer" && r != null)
			{
				r.editable = false;
				r.editEndPoint = string.Empty;
				r.createEndPoint = string.Empty;
				r.deleteEndPoint = string.Empty;
			}
			return r;
		}

		/// <summary>
		/// Get Distinct records for specified column in query param
		/// </summary>
		/// <returns></returns>
		[Route("api/Page/CustomerAsset/GetDistinct")]
		[HttpPost]
		public async Task<List<string>> GetDistinct([FromBody] BodyParams param)
		{
			param.keyColumn = "id";
			NESI.BLL.Pages.Reports.CustomerAsset custbl = new BLL.Pages.Reports.CustomerAsset(CurrentEmployee, param);

			var data = DataProcesserEngine
				.GetDistinct<DTO.ViewModels.Page.Reports.CustomerAsset,
					NESI.BLL.Pages.Reports.CustomerAsset>(custbl, param);

			return data;
		}

		/// <summary>
		/// Exports Search results in Excel file
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		[Route("api/Page/CustomerAsset/ExporttoExcel")]
		[HttpPost]
		public async Task<HttpResponseMessage> ExporttoExcel([FromBody] BodyParams param)
		{
			param.keyColumn = "id";
			//            BodyParams param = new BodyParams(Request.GetQueryNameValuePairs(), "id");
			param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
			param.page_count = 1;
			const string moduleName = "CustomerAsset";
			var grouparray = (string[])param.column_groupBy.Clone();
			param.column_groupBy = new string[] { };
			var res = await this.SearchResults(param);

			var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.CustomerAsset>();
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
		[Route("api/Page/CustomerAsset/ExporttoPdf")]
		[HttpPost]
		public async Task<HttpResponseMessage> ExporttoPdf([FromBody] BodyParams param)
		{
			var custAssetObj = new BLL.Pages.Reports.CustomerAsset(CurrentEmployee, param);
			var res = custAssetObj.GetDataFromStore();

			param.keyColumn = "id";
			//            QueryParam param = new QueryParam(Request.GetQueryNameValuePairs(), "id");
			param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
			param.page_count = 1;
			var grouparray = (string[])param.column_groupBy.Clone();
			param.column_groupBy = new string[] { };
			//var res = await this.SearchResults(param);

			var objT = Activator.CreateInstance<DTO.ViewModels.Page.Reports.CustomerAsset>();
			var rep = objT.GetSchema();

			List<Schema> schemList = new List<Schema>();
			param.columns.ForEach(s =>
			{
				var pp = rep.columns.FirstOrDefault(x => x.name == s);
				schemList.Add(new Schema { name = s, header = pp.header });
			});

			return ExportHelper.ExportToPDFInit(res, param, grouparray, "pdffile", "CustomerAsset", schemList.ToArray());
		}

		/// <summary>
		/// Creates new asset as per Model posted
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Route("api/Page/CustomerAsset/CreateAsset")]
		[HttpPost]
		public async Task<int> CreateModel(DTO.ViewModels.Page.Reports.CustomerAsset model)
		{
			NESI.BLL.Pages.Reports.CustomerAsset CC = new NESI.BLL.Pages.Reports.CustomerAsset(this.CurrentEmployee, null);
			int id = await CC.CreateModel(model);

			return id;
		}

		/// <summary>
		/// Edits Asset based on model posted
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Route("api/Page/CustomerAsset/EditAsset")]
		[HttpPost]
		public async Task<bool> EditModel(DTO.ViewModels.Page.Reports.CustomerAsset model)
		{
			NESI.BLL.Pages.Reports.CustomerAsset CC = new NESI.BLL.Pages.Reports.CustomerAsset(this.CurrentEmployee, null);

			int id = await CC.EditModel(model);

			return id > 0;
		}

		/// <summary>
		/// Bulk edit Assets based on array of model posted
		/// </summary>
		/// <param name="cvms">array of model</param>
		/// <returns></returns>
		[Route("api/Page/CustomerAsset/BulkEditAsset")]
		[HttpPost]
		public async Task<List<KeyValuePair<string, bool>>> BulkEdit(DTO.ViewModels.Page.Reports.CustomerAsset[] cvms)
		{
			List<KeyValuePair<string, bool>> list = new List<KeyValuePair<string, bool>>();
			NESI.BLL.Pages.Reports.CustomerAsset CC = new NESI.BLL.Pages.Reports.CustomerAsset(this.CurrentEmployee, null);
			string s2 = string.Empty;
			string status1 = string.Empty;
			foreach (var cvm in cvms)
			{
				int id = await CC.EditModel(cvm);
				bool status = id == 0;

				list.Add(new KeyValuePair<string, bool>(Convert.ToString(id), status));
			}
			return list;
		}

		/// <summary>
		/// Deletes Asset based on id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Route("api/Page/CustomerAsset/DeleteAsset/{id}")]
		[HttpPost]
		public async Task<string> DeleteModel(string id)
		{
			NESI.BLL.Pages.Reports.CustomerAsset CC = new NESI.BLL.Pages.Reports.CustomerAsset(this.CurrentEmployee, null);
			string status = await CC.DeleteModel(Convert.ToInt32(id));
			return status;
		}

		/// <summary>
		/// Common Method to get search Results both for serach and exports
		/// </summary>
		/// <param name="param">Query paarmeter object</param>
		/// <returns></returns>
		public async Task<Response<DTO.ViewModels.Page.Reports.CustomerAsset>> SearchResults([FromBody] BodyParams param)
		{
			param.keyColumn = key;
			NESI.BLL.Pages.Reports.CustomerAsset custbl = new BLL.Pages.Reports.CustomerAsset(this.CurrentEmployee, param) { SetColumnList = param.columns };

			var data = DataProcesserEngine
				.GetProcessedDataFromDatatable<DTO.ViewModels.Page.Reports.CustomerAsset,
					NESI.BLL.Pages.Reports.CustomerAsset>(param, custbl);
			return data;
		}
		public Task<Response<CustomerAsset>> Search()
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

		public Task<Response<CustomerAsset>> Search(List<string> listParams)
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

		public Task<List<string>> GetDistinct()
		{
			throw new NotImplementedException();
		}
	}
}
