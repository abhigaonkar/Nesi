using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Owin;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.User;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class ActionFilterAttributeBase : ActionFilterAttribute
	{

		protected string GetClientIPAddress(HttpRequestMessage request)
		{
			if (request.Properties.ContainsKey("MS_HttpContext"))
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				return IPAddress.Parse(((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress).ToString();
			}
			if (request.Properties.ContainsKey("MS_OwinContext"))
			{
				return IPAddress.Parse(((OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress).ToString();
			}
			return string.Empty;
		}
		protected string GetGuid(HttpActionContext actionContext)
		{
			var incomingPrincipal = actionContext.RequestContext.Principal;
			return incomingPrincipal?.Identity?.Name;
		}

		protected User GetCurrentUser(HttpActionContext actionContext)
		{
			var guid = GetGuid(actionContext);
			if (guid == null) return null;
			var user = Global.OnlineUser.GetValue(GetGuid(actionContext));
			if (user == null) return null;
			user.ResetLastActiveTime(actionContext);
			return user;
		}
		protected User GetOriginalUser(HttpActionContext actionContext)
		{
			var guid = GetGuid(actionContext);
			if (guid == null) return null;
			var user = Global.OriginalUser.GetValue(GetGuid(actionContext));
			if (user == null) return null;
			user.ResetLastActiveTime(actionContext);
			return user;
		}


	}
}