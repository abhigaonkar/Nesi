using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.DTO.ViewModels.Core;

namespace NESI.WebAPI.Controllers.API.Page.Applicant
{
	[RoutePrefix("api/Page/Applicant/StartNew")]
    public class ApplicantStartNewController : ApplicantControllerBase
	{
		[HttpPost]
		[Route("Existing")]
		public IHttpActionResult GetExistingApplicants([FromBody] LabelValueString model)
		{
			var o = new BLL.Pages.Applicant.ApplicantStartNew(CurrentUser);
			return Ok(o.GetExistingApplicants(model));
		}

	}
}
