using NESI.BLL.Common.Shared;
using NESI.Common.Extensions;
using NESI.Common.Models;
using NESI.Common.Password;
using NESI.DTO.ViewModels.CurrentUser;
using System;
using System.Web;

namespace NESI.BLL.Core.User
{
    public abstract partial class User
    {
        public bool ForceChangePassword { get; set; }

        /// <summary>
        /// password verification at server side
        /// we need do verify at client side as well
        /// </summary>
        /// <param name="changePassword"></param>
        /// <param name="passwordChangeTimestamp"></param>
        /// <returns> Result of password change</returns>
        public virtual NesiOperationResult ChangePassword(ChangePasswordRequestModel changePassword, DateTime passwordChangeTimestamp)
        {
            if (changePassword == null) throw new ArgumentNullException(nameof(changePassword));
            var validationResult = UserManager.ValidatePasswordChangeForUser(this, changePassword);

            if (validationResult != PasswordChangeValidationResult.Success)
            {
                return NesiOperationResult.Fail(validationResult.GetEnumDescription());
            }

            return SetPassword(changePassword.NewPassword, passwordChangeTimestamp);
        }

        /// <summary>
        /// Set the user password.
        /// </summary>
        /// <param name="newPassword">Password, must match complexity requirements</param>
        /// <param name="curDateTime"></param>
        /// <returns></returns>
        public virtual NesiOperationResult SetPassword(string newPassword, DateTime curDateTime)
        {
            var result = UserManager.ValidatePassword(newPassword);

            if (!result.IsSuccess)
            {
                return result;
            }

            try
            {
                var hashedPassword = PasswordHasher.HashPassword(newPassword);
                SaveHashedPassword(hashedPassword, curDateTime);
                ForceChangePassword = false;
                PasswordDateChange = curDateTime;
                return NesiOperationResult.Success();
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to set password for user {Id}", e);
                return NesiOperationResult.Fail(e);
            }
        }

        protected abstract void SaveHashedPassword(string password, DateTime curDateTime);

    }
}