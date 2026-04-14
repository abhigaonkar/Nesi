using NESI.WebAPI.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Core.Employee;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
	[RoutePrefix("api/Layout/ChangePassword")]
	public class ChangePasswordController : EmployeeController
	{ 
        /// <summary>
        /// Called when the user initiates profile password change
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		[HttpPost]
		[Route("2")]
		public IHttpActionResult ChangePassword([FromBody] DTO.ViewModels.CurrentUser.Layout.ChangePassword model)
		{

			var passwordChangeResult = new BLL.Layout.Banner.ChangePassword(CurrentUser)
			        .ChangeUserPassword(model.password, model.current_password, DateTime.Now);

			if (passwordChangeResult.IsSuccess && model.sync_pass.GetValueOrDefault(false))
			{
				var u = new Employee(CurrentUserId);
                u.SyncLdap(model.password);
			}
			return OkD(
			    passwordChangeResult.IsSuccess? 
			        "Password changed successfully": 
			        passwordChangeResult.JoinErrors());
		}

		[Route("getusername")]
		public IHttpActionResult get_name()
		{
			return OkD(CurrentUser.UserName);
		}



	}
}
