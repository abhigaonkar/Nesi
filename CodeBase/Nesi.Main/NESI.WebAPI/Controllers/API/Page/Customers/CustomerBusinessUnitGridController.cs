using System;
using System.Threading.Tasks;
using System.Web.Http;
using NESI.Common;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using bllT = NESI.BLL.Pages.Customers.CustomerBusinessUnitGrid; // <-- Replace with actual class name
using dtoT = NESI.DTO.ViewModels.Page.Customers.CustomerBusinessUnitGrid;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
	[PageAuthorizationFilter(10)]
	[RoutePrefix("api/Page/CustomerBusinessUnitGrid")]
	public class CustomerBusinessUnitGridController : EmployeeGridControllerBase<dtoT, bllT>
	{
		protected int _customer_id;

		public CustomerBusinessUnitGridController()
		{
			key = "cust_id";
		}

		protected override bllT GetObject(BodyParams param, params object[] extra_param)
		{
			param.keyColumn = key;
			if (extra_param != null && extra_param.Length == 2)
			{
				_customer_id = Convert.ToInt32(extra_param[0]);
			}
			else
			{
				GetParams(param);
			}
			return new bllT(this.CurrentEmployee, param, _customer_id)
			{
				SetColumnList = param.columns
			};
		}

		private void GetParams(BodyParams param)
		{
			if (param.queryparam?.Length >= 1)
			{
				_customer_id = Convert.ToInt32(param.queryparam[0].value);
			}
		}

		[HttpPost]
		[Route("Search")]
		public Response<dtoT> SearchAll([FromBody] BodyParams param)
		{
			return SearchResults(GetObject(param), param);
		}

		[HttpPost]
		[Route("GetDistinct")]
		public IHttpActionResult GetDistinct([FromBody] BodyParams param)
		{
			return Ok(base.GetDistinct(GetObject(param), param));
		}

		[HttpGet]
		[Route("")]
		public new Task<Report> GetSchema()
		{
			return base.GetSchema();
		}
	}
}
