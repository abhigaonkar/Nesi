using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;

namespace NESI.WebAPI.Controllers.API.Page.Vendors
{
	[RoutePrefix("api/Page/Vendors/StartNew")]
	public class VendorStartNewController : VendorControllerBase
	{
		[HttpGet]
		[Route("")]
		public IHttpActionResult GetProfile()
		{
			var o = new BLL.Pages.Vendors.VendorStartNew(CurrentUser);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("")]
		public IHttpActionResult Save([FromBody] DTO.ViewModels.Page.Vendors.VendorStartNew model)
		{
			var o = new BLL.Pages.Vendors.VendorStartNew(CurrentUser);
			return Ok(o.Save(model));
		}

		[HttpPost]
		[Route("CheckPhone")]
		public IHttpActionResult PhoneCheck([FromBody] DataString model)
		{
			var o = new BLL.Pages.Vendors.VendorStartNew(CurrentUser);
			return Ok(o.PhoneCheck(model.Data));
		}

		[HttpPost]
		[Route("CheckName")]
		public IHttpActionResult NameCheck([FromBody] DataString model)
		{
			var o = new BLL.Pages.Vendors.VendorStartNew(CurrentUser);
			return Ok(o.NameCheck(model.Data));
		}
	}
}
