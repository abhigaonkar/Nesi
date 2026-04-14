using System.Threading.Tasks;
using System.Web.Http;
using NESI.Common;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using bllT = NESI.BLL.Pages.Customers.CustomerHomeSearch;
using dtoT = NESI.DTO.ViewModels.Page.Customers.CustomerHomeSearch;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
	[PageAuthorizationFilter(10)]
	[RoutePrefix("api/Page/CustomerHomeSearch")]
	public class CustomerHomeSearchController : EmployeeGridControllerBase<dtoT, bllT>
	{

		public CustomerHomeSearchController()
		{
			key = "customer_id";
		}

		protected override bllT GetObject(BodyParams param, params object[] extra_param)
		{
			param.keyColumn = key;

			return new bllT(this.CurrentEmployee, param, extra_param[0].ToString()) { SetColumnList = param.columns };
		}

		[Route("")]
		[HttpGet]
		public new Task<Report> GetSchema()
		{
			return base.GetSchema();
		}

		[HttpPost]
		[Route("Search/{query}")]
		public Response<dtoT> Search([FromBody] BodyParams param, string query)
		{
			return SearchResults(GetObject(param, query), param);
		}

		[HttpPost]
		[Route("Search")]
		public Response<dtoT> SearchAll([FromBody] BodyParams param)
		{
			return SearchResults(GetObject(param, ""), param);
		}

		[Route("GetDistinct")]
		[HttpPost]
		public IHttpActionResult GetDistinct([FromBody] BodyParams param)
		{
			return Ok(base.GetDistinct(GetObject(param, ""), param));
		}

	}
}
