using System;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[RoutePrefix("api/Layout/DefaultPage")]
	public class DefaultPageController : EmployeeController
	{
		[Route("Set/{id}")]
		public IHttpActionResult Get(int id)
		{
			return OkD(new BLL.Layout.Banner.DefaultPage(CurrentUser).SetDefaultPage(id));
		}

		[HttpGet]
		[Route("")]
		public IHttpActionResult GetDefaultPageId()
		{
			return OkD(new BLL.Layout.Banner.DefaultPage(CurrentUser).GetDefaultPageId());
		}

		[HttpGet]
		[Route("PropertyValue/{name}")]
		public IHttpActionResult GetProfileSetting(string name)
		{
			var o = new BLL.Base.ProfileBase(CurrentUser);
			return OkD(o.GetValueByPropertyName(name));
		}

		[HttpPost]
		[Route("PropertyValue/{name}")]
		public IHttpActionResult SaveProfileSetting([FromBody] LabelValueInt model, string name)
		{
			var o = new BLL.Base.ProfileBase(CurrentUser);
			return OkD(o.SetPropertyValue(model.Label, model.Value.ToString()));
		}
	}
}
