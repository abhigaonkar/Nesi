using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
	[RoutePrefix("api/Page/Customers/Sales")]
	public class CustomerSalesController : CustomerControllerBase
	{
		[HttpGet]
		[Route("Profile/{cust_id}/{address_id}")]
		public IHttpActionResult GetProfile(int cust_id, int address_id)
		{
			var o = new BLL.Pages.Customers.CustomerSales(CurrentUser, cust_id,address_id);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("{cust_id}/{address_id}")]
		public IHttpActionResult Save([FromBody] DTO.ViewModels.Page.Customers.CustomerSalesProperties model, int cust_id, int address_id)
		{
			var o = new BLL.Pages.Customers.CustomerSales(CurrentUser, cust_id, address_id);
			return Ok(o.Save(model));
		}

		[HttpPost]
		[Route("History/{cust_id}/{address_id}")]
		public IHttpActionResult SaveHistory([FromBody] DTO.ViewModels.Page.Customers.CustomerHistory model, int cust_id, int address_id)
		{
			var o = new BLL.Pages.Customers.CustomerSales(CurrentUser, cust_id, address_id);
			return Ok(o.SaveHistory(model));
		}
	}
}