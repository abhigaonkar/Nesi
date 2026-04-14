using System;
using System.Web.Http.Controllers;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class NesiEmployeeNoCheckFilterAttribute : NesiFilterAttributeBase
	{
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{

			var user = base.GetCurrentUser(actionContext);
			var ouser = base.GetCurrentUser(actionContext);
			return user != null && ouser != null
				   && !user.IsContact
				   && user.ExpiredTime > DateTime.Now;

		}
	}
}