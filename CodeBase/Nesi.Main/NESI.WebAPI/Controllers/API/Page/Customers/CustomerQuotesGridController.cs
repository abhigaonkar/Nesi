using System;
using System.Threading.Tasks;
using System.Web.Http;
using NESI.Common;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using bllT = NESI.BLL.Pages.Customers.CustomerQuotesGrid;
using dtoT = NESI.DTO.ViewModels.Page.Customers.CustomerQuotesGrid;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
	[PageAuthorizationFilter(10)]
	[RoutePrefix("api/Page/CustomerQuotesGrid")]
	public class CustomerQuotesGridController : EmployeeGridControllerBase<dtoT, bllT>
	{
		protected int _customer_id;
		protected int _address_id;


		public CustomerQuotesGridController()
		{
			key = "quote_id";
		}

		protected override bllT GetObject(BodyParams param, params object[] extra_param)
		{
			param.keyColumn = key;
			if (extra_param != null && extra_param.Length == 2)
			{
				_customer_id = Convert.ToInt32(extra_param[0]);
				_address_id = Convert.ToInt32(extra_param[1]);
			}
			else
			{
				GetParams(param);
			}
			return new bllT(this.CurrentEmployee, param, _customer_id, _address_id) { SetColumnList = param.columns };
		}

		[Route("")]
		[HttpGet]
		public new Task<Report> GetSchema()
		{
			return base.GetSchema();
		}

		//[HttpPost]
		//[Route("Search/{query}")]
		//public Response<dtoT> Search([FromBody] BodyParams param, string query)
		//{
		//	return SearchResults(GetObject(param, query), param);
		//}

		public void GetParams(BodyParams param)
		{
			if (param.queryparam.Length != 2) return;
			_customer_id = Convert.ToInt32(param.queryparam[0].value);
			_address_id = Convert.ToInt32(param.queryparam[1].value);
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

	}
}
