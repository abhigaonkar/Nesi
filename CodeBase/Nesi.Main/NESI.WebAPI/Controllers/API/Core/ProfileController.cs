using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Base;
using NESI.DTO.ViewModels.Core;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Core
{
	[RoutePrefix("api/Core/Profile")]
	public class ProfileController : EmployeeController
	{
		[Route("{name}")]
		public IHttpActionResult GetValue(string name)
		{
			var o = new ProfileBase(CurrentUser);
			return OkD(o.GetValueByPropertyName(name));
		}

		[HttpPost]
		[Route("{name}")]
		public IHttpActionResult SaveValue([FromBody] DataString model, string name)
		{
			var o = new ProfileBase(CurrentUser);
			return OkD(o.SetPropertyValue(name, model.Data));
		}
	}
}
