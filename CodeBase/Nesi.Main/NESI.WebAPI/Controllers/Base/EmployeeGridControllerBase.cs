using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Export;
using nesi.core;
using NESI.BLL.Pages.WorkOrder;
using NESI.Common;
using NESI.Common.Interface;
using NESI.DataProcessor;
using NESI.DTO.ViewModels.Page.WorkOrder;

namespace NESI.WebAPI.Controllers.Base
{


	public abstract class EmployeeGridControllerBase<T, BLLT> : ApiControllerBase, IReportViewer<T>, IReportEditor<T>, IReportExporter where T : class, IModelBase, new() where BLLT : IModelGenerator<T>, new()
	{
		protected string key = "id";
		protected string moduleName = "";

		protected abstract BLLT GetObject(BodyParams param, params object[] extra_params);

		public async Task<HttpResponseMessage> ExporToExcel([FromBody] BodyParams param)
		{
			param.keyColumn = key;
			param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
			param.page_count = 1;
			var grouparray = (string[])param.column_groupBy.Clone();
			param.column_groupBy = new string[] { };

			var res = SearchResults(GetObject(param), param);

			var objT = Activator.CreateInstance<T>();
			var rep = objT.GetSchema();

			List<Schema> schemList = new List<Schema>();
			param.columns.ForEach(s =>
			{
				var pp = rep.columns.FirstOrDefault(x => x.name == s);
				if (pp != null)
					schemList.Add(new Schema { name = s, header = pp.header });
			});

			return ExportHelper.ExportToExcelInit(res, param, grouparray, "excelfile", moduleName, schemList.ToArray());
		}

		public async Task<HttpResponseMessage> ExportToPdf(BodyParams param)
		{
			var res = GetObject(param).GetDataFromStore();

			param.keyColumn = key;
			param.page_size = Convert.ToInt32(NESI.BLL.Common.Shared.Configuration.PageSizeForPdf);
			param.page_count = 1;
			var grouparray = (string[])param.column_groupBy.Clone();
			param.column_groupBy = new string[] { };

			var objT = Activator.CreateInstance<T>();
			var rep = objT.GetSchema();

			List<Schema> schemList = new List<Schema>();
			param.columns.ForEach(s =>
			{
				var pp = rep.columns.FirstOrDefault(x => x.name == s);
				if (pp != null)
					schemList.Add(new Schema { name = s, header = pp.header });
			});

			return ExportHelper.ExportToPDFInit(res, param, grouparray, "pdffile", moduleName, schemList.ToArray());
		}

		public async Task<Report> GetSchema()
		{
			var objT = Activator.CreateInstance<T>();
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
			if (!string.IsNullOrEmpty(CurrentEmployee.ExtraType) && r != null)
			{
				r.editable = false;
				r.editEndPoint = string.Empty;
				r.createEndPoint = string.Empty;
				r.deleteEndPoint = string.Empty;
			}
			return r;
		}

		public Response<T> SearchResults(BLLT obj, BodyParams param)
		{
            try
            {
                var o = DataProcesserEngine.GetProcessedDataFromDatatable<T, BLLT>(param, obj);
                o.extra = obj.GetProcessExtra();
                return o;
            }
            catch (Exception ex)
            {
                Toolbox.do_errorLog_errorStack(ex);
                throw ex.InnerException;
            }
        }

		public List<string> GetDistinct(BLLT obj, BodyParams _param)
		{
			//	return obj.GetDistinct(_param).Take(1000).ToList();
			return DataProcesserEngine.GetDistinct<T, BLLT>(obj, _param);
		}


		

		public Task<List<string>> GetDistinct()
		{
			throw new NotImplementedException();
		}
		public Task<Response<T>> Search()
		{
			throw new NotImplementedException();
		}

		public Task<Response<T>> Search(List<string> listParams)
		{
			throw new NotImplementedException();
		}

		public Task<int> CreateModel(T model)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteModel(string id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> EditModel(T model)
		{
			throw new NotImplementedException();
		}

		public Task<List<KeyValuePair<string, bool>>> BulkEdit(T[] cvms)
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

		public Task<HttpResponseMessage> ExporttoPdf(string fileName, List<string> columnList)
		{
			throw new NotImplementedException();
		}

		public Task<HttpResponseMessage> ExporttoExcel(string fileName, List<string> columnList)
		{
			throw new NotImplementedException();
		}

		Task<string> IReportEditor<T>.DeleteModel(string id)
		{
			throw new NotImplementedException();
		}
	}
}
