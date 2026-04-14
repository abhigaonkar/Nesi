using NESI.DTO.ViewModels.CurrentUser;
using NESI.WebAPI.Controllers.Base;
using System;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.CurrentUser.SignIn
{
    [RoutePrefix("api/Password")]
	public class PasswordChangeController : ApiControllerBase
    {
        /// <summary>
        /// Change the logged in user password
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
	    [HttpPost]
	    [Route("Change")]
	    public IHttpActionResult ChangePassword([FromBody] ChangePasswordRequestModel changePassword)
        {
            var passwordChangeResult = CurrentUser.ChangePassword(changePassword, DateTime.Now);

            return OkD(
	            passwordChangeResult.IsSuccess ?
	                "Password changed successfully" :
	                passwordChangeResult.JoinErrors());
	    }

        /// <summary>
        /// Returns if the current user is required to change their password
        /// </summary>
        /// <returns></returns>
	    [Infrastructures.Filters.NesiEmployeeFilter]
	    [HttpGet]
	    [Route("Change")]
	    public IHttpActionResult Get()
	    {
		    return Ok(CovertoJsonData(
			    DateTime.Now.Subtract(CurrentUser.PasswordDateChange).Days > BLL.Common.Shared.Configuration.ForcedChangePasswordDays)
		    );
	    }
	}
}
