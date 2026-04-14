using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;

namespace NESI.WebAPI.Controllers.API.Page.Vendors
{
	[RoutePrefix("api/Page/Vendors/BvContact")]
	public class VendorBvContactController : VendorControllerBase
	{
		[HttpGet]
		[Route("{vendor_id}/{index}")]
		public IHttpActionResult GetProfile(int vendor_id, int index)
		{
			var o = new BLL.Pages.Vendors.VendorBvContact(CurrentUser, vendor_id,index);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("{vendor_id}/{index}")]
		public IHttpActionResult Save([FromBody] DTO.ViewModels.Page.Vendors.VendorBvContact model, int vendor_id, int index)
		{
			var o = new BLL.Pages.Vendors.VendorBvContact(CurrentUser, vendor_id, index);
			return Ok(o.Save(model));
		}
	}
}
