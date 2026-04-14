using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace NESI.WebAPI.Infrastructures.Filters
{
	
	public class ValidateModelAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			// Only way to be sure, that null comment be allowed only to one page without breaking
			// rest of the pages
			if (actionContext.Request.RequestUri.AbsolutePath.Equals("/api/Page/Timesheet/WorkOrder") 
				&& (actionContext.ModelState.IsValid == false))
            {
				if (actionContext.ModelState.ContainsKey("model.MemberTime_WoComment"))
					actionContext.ModelState.Remove("model.MemberTime_WoComment");
            }
			if (actionContext.ModelState.IsValid == false)
			{
				actionContext.Response = actionContext.Request.CreateErrorResponse(
					HttpStatusCode.BadRequest, actionContext.ModelState);
			}
		}
	}

}