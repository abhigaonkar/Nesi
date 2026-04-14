using System;
using System.Threading.Tasks;
using System.Web.Http;
using NESI.Common;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using bllT = NESI.BLL.Pages.Customers.CustomerArEmailsGrid;
using dtoT = NESI.DTO.ViewModels.Page.Customers.CustomerArEmailsGrid;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
	[PageAuthorizationFilter(10)]
	[RoutePrefix("api/Page/CustomerArEmailsGrid")]
	public class CustomerArEmailsGridController : EmployeeGridControllerBase<dtoT, bllT>
	{
		protected int _customer_id;


		public CustomerArEmailsGridController()
		{
			key = "emaillog_id";
		}
		protected override bllT GetObject(BodyParams param, params object[] extra_param)
		{
			param.keyColumn = key;
			if (extra_param != null && extra_param.Length == 1)
			{
				_customer_id = Convert.ToInt32(extra_param[0]);
			}
			else if (param.queryparam.Length == 1)
			{
				_customer_id = Convert.ToInt32(param.queryparam[0].value);
			}
			return new bllT(this.CurrentEmployee, param, _customer_id) { SetColumnList = param.columns };
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

	}
}
