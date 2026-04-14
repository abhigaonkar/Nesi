using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
	[RoutePrefix("api/Page/Customers")]
	public class CustomerHomePageController : CustomerControllerBase
    {
	    [HttpPost]
		[Route("Search")]
	    public IHttpActionResult SearchByCriteria([FromBody] DTO.ViewModels.Page.Customers.SearchCriteriaPagination model)
	    {
			var o=new BLL.Pages.Customers.CustomerBase(CurrentUser);
		    return Ok(o.SearchCustomers(model));
	    }

	    [HttpGet]
	    [Route("Profile")]
		public IHttpActionResult GetProfile()
	    {
		    var o = new BLL.Pages.Customers.CustomerBase(CurrentUser);
		    return Ok(o.Profile());
		}

	    [HttpPost]
	    [Route("PostcodeSearch")]
	    public IHttpActionResult PostCodeSearch()
	    {
		    var o = new BLL.Pages.Customers.CustomerBase(CurrentUser);
		    return Ok(o.PostCodeSearch());
		}

    }
}
