using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Applicant
{
	[RoutePrefix("api/Page/Applicant/Edit")]
	public class ApplicantEditController : ApplicantControllerBase
	{
		[HttpGet]
		[Route("{applicantid}")]
		public IHttpActionResult GetProfile(int applicantid)
		{
			var o = new BLL.Pages.Applicant.ApplicantEdit(CurrentUser, applicantid);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("{applicantid}")]
		public IHttpActionResult SaveProfile([FromBody] DTO.ViewModels.Page.Applicant.ApplicantEdit model, int applicantid)
		{
			var o = new BLL.Pages.Applicant.ApplicantEdit(CurrentUser, applicantid);
			return Ok(o.Save(model));
		}
	}
}
