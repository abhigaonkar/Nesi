using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
	[RoutePrefix("api/Page/Customers/New")]
	public class CustomerNewController : CustomerControllerBase
	{
		[HttpGet]
		[Route("Profile")]
		public IHttpActionResult GetProfile()
		{
			var o = new BLL.Pages.Customers.CustomerNew(CurrentUser);
			return Ok(o.Profile());
		}

		[HttpGet]
		[Route("businessUnitPorifle/{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetProjectManagerList(int buid)
		{
			var o = new BLL.Pages.Customers.CustomerNew(CurrentUser);
			return Ok(o.getBusinessUnitDefaultProfile(buid));
		}

		[HttpPost]
		[Route("CheckName")]
		public IHttpActionResult CheckCustomerName([FromBody] DataString model)
		{
			var o = new BLL.Pages.Customers.CustomerNew(CurrentUser);
			return Ok(o.GetCustomerByName(model.Data));
		}

		[HttpPost]
		[Route("CheckPhone")]
		public IHttpActionResult CheckPhone([FromBody] DataString model)
		{
			var o = new BLL.Pages.Customers.CustomerNew(CurrentUser);
			return Ok(o.GetCustomerByPhoneNumber(model.Data));
		}

		[HttpPost]
		[Route("Save")]
		public IHttpActionResult Save([FromBody] DTO.ViewModels.Page.Customers.CustomerNew model)
		{
			var o = new BLL.Pages.Customers.CustomerNew(CurrentUser);
			return Ok(o.Save(model));
		}
	}
}
