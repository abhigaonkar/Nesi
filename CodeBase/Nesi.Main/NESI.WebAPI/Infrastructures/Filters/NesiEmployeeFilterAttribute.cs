using System;
using System.Web.Http.Controllers;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.BLL.Core.User;
using NESI.Data.Entities;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class NesiEmployeeFilterAttribute : NesiFilterAttributeBase
	{
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{

			var user = base.GetCurrentUser(actionContext);
			var ouser = base.GetOriginalUser(actionContext);
			return user != null && ouser != null
				   && !user.IsContact
				   && Is_Active(ouser)
				   && user.FvrPassed
				   && !ouser.IsContact
				   && ouser.FvrPassed
				   && ouser.ExpiredTime > DateTime.Now;

		}


		public bool Is_Active(User user)
		{
			var tool = new BLLBase.BLLToolbox(new NESIMySQL());
			var o = tool.doSQL_string(@"SELECT member_status from member where member_id=@v0", user.Id).ToLower() == "active";
			if (!o)
			{
				BLL.Common.Cache.Global.OriginalUser.Delete(user.Guid);
				BLL.Common.Cache.Global.OnlineUser.Delete(user.Guid);
			}
			return o;
		}
	}
}