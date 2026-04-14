using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;


namespace NESI.WebAPI.Infrastructures.Filters
{
	public class NesiAutheticationFilterAttribute : NesiFilterAttributeBase
	{
		protected int GetIntParamvalue(HttpActionContext actionContext, string paramName)
		{

			var o = GetStringParamvalue(actionContext, paramName);
			return string.IsNullOrEmpty(o) ? -1 : Convert.ToInt32(o);
		}

		protected string GetStringParamvalue(HttpActionContext actionContext, string paramName)
		{
			var ps = actionContext.ControllerContext.RouteData.Values;
			var ret = string.Empty;
			if (ps != null && Convert.ToBoolean(ps.ContainsKey(paramName)))
			{
				ret = ps[paramName].ToString();
			}
			return ret;
		}

		protected string GetCookieGuid(HttpActionContext actionContext)
		{
			var cookies = actionContext.Request.GetOwinContext().Request.Cookies;
			var cookieGuid = "";
			if (cookies?["nesi2"] != null)
			{
				cookieGuid = cookies["nesi2"];
			}
			return cookieGuid;
		}

		protected override bool IsAuthorized(HttpActionContext actionContext)
		{

			var user = base.GetCurrentUser(actionContext);
			//var cookieGuid = GetCookieGuid(actionContext);
			//var guid = base.GetGuid(actionContext);
			//if (!BLL.Common.Shared.Configuration.DebugRedirect 
			//	&& (string.IsNullOrEmpty(cookieGuid) || cookieGuid != guid)) return false;
			return user != null && user.ExpiredTime > DateTime.Now;
		}
	}
}