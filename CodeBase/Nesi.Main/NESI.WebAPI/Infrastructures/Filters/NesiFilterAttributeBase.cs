using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.User;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public abstract class NesiFilterAttributeBase : AuthorizeAttribute
	{
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
			user.Guid = guid;
			user.ResetLastActiveTime(actionContext);
			return  user;
		}
		protected User GetOriginalUser(HttpActionContext actionContext)
		{
			var guid = GetGuid(actionContext);
			if (guid == null) return null;
			var user = Global.OriginalUser.GetValue(GetGuid(actionContext));
			if (user == null) return null;
			user.Guid = guid;
			user.ResetLastActiveTime(actionContext);
			return user;
		}
	}
}