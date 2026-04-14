using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Vendors
{
	[RoutePrefix("api/Page/Vendors/Market")]
    public class VendorMarketController : VendorControllerBase
	{
		[HttpGet]
		[Route("{vendor_id}")]
		public IHttpActionResult GetPhoneProfile(int vendor_id)
		{
			var o = new BLL.Pages.Vendors.VendorMarket(CurrentUser, vendor_id);
			return Ok(o.Profile());
		}
	}
}
