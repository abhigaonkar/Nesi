using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using System;
using NESI.Common.Models;

namespace NESI.BLL.Layout.Banner
{
    public class ChangePassword : BLLBase
    {
        public ChangePassword(Employee user) :base(user)
        {

        }
        public NesiOperationResult ChangeUserPassword( string password, string currentPassword, DateTime curDateTime)
        {
	        var changePasswordRequest =
		        new DTO.ViewModels.CurrentUser.ChangePasswordRequestModel
		        {
			        Username = CurrentUser.UserName,
			        NewPassword = password,
			        OldPassword = currentPassword,
			        ConfirmPassword = password
		        };
            var result = CurrentUser.ChangePassword(changePasswordRequest, curDateTime);
            return result;
        }
    }
}
