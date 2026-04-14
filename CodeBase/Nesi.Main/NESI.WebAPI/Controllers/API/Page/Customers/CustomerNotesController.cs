using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
	[RoutePrefix("api/Page/Customers/Notes")]
	public class CustomerNotesController : CustomerControllerBase
    {

	    [HttpGet]
	    [Route("Profile/{cust_id}/{address_id}")]
	    public IHttpActionResult GetProfile(int cust_id, int address_id)
	    {
		    var o = new BLL.Pages.Customers.CustomerNotes(CurrentUser, cust_id, address_id);
		    return Ok(o.Profile());
	    }

	    [HttpGet]
	    [Route("ArEmails/{cust_id}/{address_id}")]
	    public IHttpActionResult GetArEmails(int cust_id, int address_id)
	    {
		    var o = new BLL.Pages.Customers.CustomerNotes(CurrentUser, cust_id, address_id);
		    return Ok(o.GetArEmails());
	    }

	    [HttpGet]
	    [Route("InvoiceNotes/{cust_id}/{address_id}")]
	    public IHttpActionResult GetInvoiceNotes(int cust_id, int address_id)
	    {
		    var o = new BLL.Pages.Customers.CustomerNotes(CurrentUser, cust_id, address_id);
		    return Ok(o.GetInvoiceNotes());
	    }

		[HttpPost]
	    [Route("{cust_id}/{address_id}")]
	    public IHttpActionResult Save([FromBody] DTO.ViewModels.Core.DataIdString model, int cust_id, int address_id)
	    {
		    var o = new BLL.Pages.Customers.CustomerNotes(CurrentUser, cust_id, address_id);
		    return Ok(o.Save(model));
	    }

	}
}
