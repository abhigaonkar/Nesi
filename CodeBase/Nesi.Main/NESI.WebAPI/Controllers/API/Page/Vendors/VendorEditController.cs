using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;

namespace NESI.WebAPI.Controllers.API.Page.Vendors
{
	[RoutePrefix("api/Page/Vendors/Edit")]
	public class VendorEditController : VendorControllerBase
	{
		[HttpGet]
		[Route("{vendor_id}")]
		public IHttpActionResult GetProfile(int vendor_id)
		{
			var o = new BLL.Pages.Vendors.VendorEdit(CurrentUser, vendor_id);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("{vendor_id}")]
		public IHttpActionResult Save([FromBody] DTO.ViewModels.Page.Vendors.VendorEdit model, int vendor_id)
		{
			var o = new BLL.Pages.Vendors.VendorEdit(CurrentUser, vendor_id);
			return Ok(o.Save(model));
		}
	}
}
