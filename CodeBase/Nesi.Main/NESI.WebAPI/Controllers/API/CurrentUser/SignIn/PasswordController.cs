using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using log4net;
using NESI.BLL.Common.Shared;
using NESI.Common.Extensions;
using NESI.Common.Models;
using NESI.Common.Password;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.CurrentUser;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Models;

namespace NESI.WebAPI.Controllers.API.CurrentUser.SignIn
{
	[RoutePrefix("api/Password")]
	public class PasswordController : ApiController
	{
	    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Convert an operation result to our Api response model
        /// </summary>
        /// <param name="operationResult"></param>
        /// <param name="successMessage"></param>
        /// <returns></returns>
	    private IHttpActionResult ApiResultFromOperationResult(NesiOperationResult operationResult,  string successMessage = null)
	    {
	        if (operationResult == null) throw new ArgumentNullException(nameof(operationResult));
	        var apiResult = operationResult.IsSuccess ?
	            ApiResult.SuccessResult(successMessage ?? operationResult.Message ?? "Success") :
	            ApiResult.FailureResult(operationResult.JoinErrors());
	        return Ok(apiResult);
	    }



        /// <summary>
        /// Called when user clicks on forgot password. This generates an email for the user
        /// with further instructions to reset
        /// </summary>
        /// <param name="forgotPassword"></param>
        /// <returns></returns>
        [HttpPost]
		[Route("Forgot")]
		public IHttpActionResult ForgotPassword([FromBody] ForgotPasswordModel forgotPassword)
        {
            var result = UserManager.ProcessForgotPassword(forgotPassword);
            return ApiResultFromOperationResult(result);
		}


        /// <summary>
        /// Called when user requests a login
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
		[HttpPost]
		[Route("Request")]
		public IHttpActionResult RequestPassword([FromBody] EmailAddress email)
		{
		    var result = UserManager.RequestPassword(email.Email);
		    return ApiResultFromOperationResult(result);
		}

        /// <summary>
        /// Validates the given password for complexity requirements
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		[HttpPost]
		[Route("Validate")]
		public IHttpActionResult ValidatePassword([FromBody] DataString obj)
        {
            var result = UserManager.ValidatePassword(obj.Data);
            return ApiResultFromOperationResult(result);
		}

        /// <summary>
        /// Called initially when the user has clicked on the reset token in email
        /// We verify that the token is valid and the usernames match
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		[HttpPost]
		[Route("ValidateResetToken")]
		public IHttpActionResult ValidateResetToken([FromBody] DataString obj)
		{
            var result = UserManager.ValidateResetToken(obj.Data, obj.Data2);
		    return ApiResultFromOperationResult(result);
		}

	    /// <summary>
	    /// Called when user resets password after <see cref="ValidateResetToken"/>
	    /// </summary>
	    /// <param name="model"></param>
	    /// <returns></returns>
	    [HttpPost]
	    [Route("Reset")]
	    public IHttpActionResult ResetPassword([FromBody] ResetPassword model)
	    {
	        var result = UserManager.ResetEmployeePassword(model, DateTime.Now);
	        return ApiResultFromOperationResult(result);
	    }
	}
}
;