using log4net;
using Microsoft.Owin.Security.DataProtection;
using NESI.BLL.Core;
using NESI.BLL.Core.Member;
using NESI.BLL.Core.User;
using NESI.BLL.EmbeddedFiles;
using NESI.BLL.EmbeddedResources;
using NESI.Common.Exceptions;
using NESI.Common.Extensions;
using NESI.Common.Models;
using NESI.Common.Password;
using NESI.Common.Password.Configuration;
using NESI.Common.Templates;
using NESI.Data.Entities;
using NESI.DTO.Models.Users;
using NESI.DTO.ViewModels.CurrentUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using NESI.BLL.Core.Employee;
using nesi.core;

namespace NESI.BLL.Common.Shared
{
    /// <summary>
    /// General user related operations, externalized
    /// </summary>
    public static class UserManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Create a data protector, will be used for encryption purposes
        /// </summary>
        private static readonly IDataProtector DataProtector = new DpapiDataProtectionProvider("NESI").Create();

        /// <summary>
        /// Returns true iff the given user is configured for LDAP
        /// </summary>
        /// <param name="employeeProfile"></param>
        /// <returns></returns>
        public static bool IsLdapUser(Member employeeProfile) =>
            !string.IsNullOrEmpty(employeeProfile.member_ldap_user);

        /// <summary>
        /// Validates that the password is valid according to password strength requirements
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PasswordValidationResult ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            var options = ConfigurationSettingsExtractor.GetConfiguredPasswordOptions();
            var validationErrors = PasswordValidator.Validate(password, options);

            return validationErrors == PasswordValidationError.None
                ? PasswordValidationResult.PasswordValidationSucceeded()
                : PasswordValidationResult.PasswordValidationFailed(validationErrors);
        }

        /// <summary>
        /// Ensures that the password provided matches that stored for the user
        /// Assumes that the stored password is securely hashed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
	    public static bool IsCorrectPasswordForUser(User user, string password)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (password == null) throw new ArgumentNullException(nameof(password));

            if (user.IsContact)
            {
                Logger.Warn($"Attempting to verify password for non employee user {user.UserName}");
                return user.Password == password;
            }
            try
            {
                return PasswordHasher.VerifyHashedPassword(user.Password, password) == PasswordVerificationResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error($"Unable to verify user password for user {user.Id}", e);
                return false;
            }

        }

        #region Change Password

        /// <summary>
        /// Validate the password change request in the user context
        /// </summary>
        /// <param name="user"></param>
        /// <param name="changePasswordRequest"></param>
        /// <returns></returns>
        public static PasswordChangeValidationResult ValidatePasswordChangeForUser(User user,
            ChangePasswordRequestModel changePasswordRequest)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (changePasswordRequest == null) throw new ArgumentNullException(nameof(changePasswordRequest));

            //
            //  Request should be valid
            //  User name should match current user
            //  Old password should match the current
            var result = ValidatePasswordChange(changePasswordRequest);
            if (result != PasswordChangeValidationResult.Success) return result;

            if (!string.Equals(changePasswordRequest.Username, user.UserName, StringComparison.OrdinalIgnoreCase))
            {
                return PasswordChangeValidationResult.UserNameNotCorrect;
            }

            if (!IsCorrectPasswordForUser(user, changePasswordRequest.OldPassword))
            {
                return PasswordChangeValidationResult.CurrentPasswordNotCorrect;
            }

            return PasswordChangeValidationResult.Success;
        }

        #endregion

        #region Forgot/Reset Password

        /// <summary>
        /// Have the embedded purpose in the token be environment specific
        /// Prevent users from hijacking link to change creds in other environments
        /// </summary>
        private static string EnvSpecificForgotPasswordPurpose =>
            $"{UserTokenGenerator.PurposeForgotPassword}_{Configuration.Environment.ToLower()}";

        /// <summary>
        /// Process a forgot password request
        /// </summary>
        /// <param name="forgotPassword"></param>
        /// <returns></returns>
        public static NesiOperationResult ProcessForgotPassword(ForgotPasswordModel forgotPassword)
        {
            if (forgotPassword == null) throw new ArgumentNullException(nameof(forgotPassword));
            if (string.IsNullOrEmpty(forgotPassword.Username)) return NesiOperationResult.Fail("Please enter a valid username.");

            try
            {

               // var user = new UserFactory().GetUser(forgotPassword.Username);
                var user = new Employee(forgotPassword.Username);

                if (user == null || user.Id == 0 || !user.IsActive)
                    return NesiOperationResult.Fail($"User '{forgotPassword.Username}' not found");
                if (user.IsContact)
                    return NesiOperationResult.Fail(AssemblyFileLoader.LoadFile<EmbeddedBLLResourceMarker, EmbeddedBLLResourceMarker>("CustomerLoginDisabled.html"));

                var token = HttpUtility.UrlEncode(GetPasswordResetToken(user));
                var body = AssemblyFileLoader.LoadFile<User, EmbeddedBLLResourceMarker>("ForgotPassword.html");

                var dict = new Dictionary<string, object>
                {
                    {"username", user.UserName},
                    {"token", token},
                    {"hostname", Configuration.HostName},
                };

                body = TemplateParser.Parse(body, dict);

                //check email is not nomail, and use personal if cant find Neemail
                var userEmail = user.EmployeeProfile.member_neemail;
                if (userEmail == "" || userEmail.Contains("nomail@"))
                {
                    userEmail = user.EmployeeProfile.Member_Email;
                    if (userEmail == "" || userEmail.Contains("nomail@"))
                    {
                        //no email on file for the user. 
                        var emailIT = NesiEmailBuilder.CutTicketToIT(user.ReportToManager.member_neemail,
                            $" {user.FullName} ({user.UserName}) Was unable to reset their " + Toolbox.app_setting("Domain") + " password, because they dont have a valid email address on file.");
                        emailIT.Send();
                        return NesiOperationResult.Fail($"No email on file, IT has been alerted");
                    }
                }

                var email = NesiEmailBuilder.BuildForgotPasswordEmail(userEmail, body);
                email.Send();

                return NesiOperationResult.Success($"Please check {userEmail} for further instructions");
            }
            catch (NesiException nesiException)
            {
                Logger.Error($"Failed to get token for {forgotPassword.Username}", nesiException);
                return NesiOperationResult.Fail(nesiException.Message);
            }
            catch (Exception exception)
            {
                Logger.Error($"Failed to get token for {forgotPassword.Username}", exception);
                return NesiOperationResult.Fail("Error while generating token");
            }

        }

        /// <summary>
        /// Check that the given token is valid for the user
        /// </summary>
        /// <param name="token"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static NesiOperationResult ValidateResetToken(string token, string username)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (username == null) throw new ArgumentNullException(nameof(username));

            var user = new UserFactory().GetUser(username);
            if (user == null || user.Id == 0) return NesiOperationResult.Fail($"User {username} not found");
            if (user.IsContact) return NesiOperationResult.Fail("Feature not available for contacts");

            var result = new UserTokenGenerator(DataProtector).Validate(
                EnvSpecificForgotPasswordPurpose,
                token,
                new UserIdentity(user));

            return !result.IsSuccess ? result : NesiOperationResult.Success();
        }

        /// <summary>
        /// Uses the reset token to reset the user password
        /// </summary>
        /// <param name="model"></param>
        /// <param name="curDateTime"></param>
        /// <returns></returns>
	    public static NesiOperationResult ResetEmployeePassword(ResetPassword model, DateTime curDateTime)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            try
            {
                /*
                 * Independently validate the token. Very similar to change password, but doesn't require
                 * validation of pre-existing password
                 */
                var validationResult = ValidatePasswordReset(model);
                if (validationResult != PasswordChangeValidationResult.Success) return NesiOperationResult.Fail(validationResult.GetEnumDescription());

                /*
                 * Re-validate the token and user name; don't count on it being called beofore this
                 * Plus, the token could've expired in the meantime
                 */
                var tokenValidationResult = ValidateResetToken(model.Token, model.Username);
                if (!tokenValidationResult.IsSuccess) return tokenValidationResult;

                var user = new UserFactory().GetUser(model.Username);
                if (user == null || user.Id == 0) return NesiOperationResult.Fail($"User {model.Username} not found");
                if (user.IsContact) return NesiOperationResult.Fail("Feature not available for contacts");

                /*
                 * Ensure that the flag is set for the database record
                 * This wasy, we'll protect against multiple clicks with the same token
                 * Flag will be reset when the password is saved
                 */
                bool resetFlagSet = CheckMember(user.Id,
                    member => member.request_password_reset_flag.HasValue &&
                              member.request_password_reset_flag == true);
                if (!resetFlagSet)
                {
                    return NesiOperationResult.Fail("Link already used");
                }

                return user.SetPassword(model.Password, curDateTime);
            }
            catch (NesiException nesiException)
            {
                Logger.Error($"Failed to reset password for  {model.Username ?? "NO USER"}", nesiException);
                return NesiOperationResult.Fail(nesiException.Message);
            }
            catch (Exception exception)
            {
                Logger.Error($"Failed to reset password for  {model.Username ?? "NO USER"}", exception);
                return NesiOperationResult.Fail("Error while resetting password");
            }
        }

        /// <summary>
        /// Get the reset token for the given user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tokenExpiryDate"></param>
        /// <returns></returns>
        public static string GetPasswordResetToken(User user, DateTime? tokenExpiryDate = null)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.IsContact) throw new NesiException("Feature not available for contacts");

            var token = new UserTokenGenerator(DataProtector).Generate(
                EnvSpecificForgotPasswordPurpose,
                new UserIdentity(user),
                tokenExpiryDate);
            SetPasswordFlag(user.Id, true);
            return token;
        }

        #endregion

        #region Request Account

        /// <summary>
        /// Process a request for a new account
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
	    public static NesiOperationResult RequestPassword(string emailAddress)
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            if (!Email.CheckEmail(emailAddress)) return NesiOperationResult.Fail("Please enter a valid email address.");

            if (Email.IsSPAddress(emailAddress))
            { return NesiOperationResult.Fail("This is not for existing employees, please contact IT or click the forgot password link."); }
            var email = NesiEmailBuilder.GetNewLoginRequestEmail(emailAddress);
            email.Send();
            return NesiOperationResult.Success();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Validate password reset model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static PasswordChangeValidationResult ValidatePasswordReset(ResetPassword model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (!model.RePassword.Equals(model.Password, StringComparison.InvariantCulture))
            {
                return PasswordChangeValidationResult.ConfirmationPasswordNotMatched;
            }

            if (!ValidatePassword(model.Password).IsSuccess)
            {
                return PasswordChangeValidationResult.NewPasswordNotComplex;
            }

            return PasswordChangeValidationResult.Success;
        }

        /// <summary>
        /// Validates a password change mode syntactically
        /// </summary>
        /// <param name="changePasswordRequest"></param>
        /// <returns></returns>
        private static PasswordChangeValidationResult ValidatePasswordChange(
            ChangePasswordRequestModel changePasswordRequest)
        {
            if (changePasswordRequest == null) throw new ArgumentNullException(nameof(changePasswordRequest));
            //
            //  Confirmation password should match new one
            //  New password should not match old one
            //  New password should be complex enough
            //
            if (!changePasswordRequest.ConfirmPassword.Equals(changePasswordRequest.NewPassword, StringComparison.InvariantCulture))
            {
                return PasswordChangeValidationResult.ConfirmationPasswordNotMatched;
            }

            if (changePasswordRequest.NewPassword.Equals(changePasswordRequest.OldPassword, StringComparison.InvariantCulture))
            {
                return PasswordChangeValidationResult.CurrentPasswordSameAsOld;
            }

            if (!ValidatePassword(changePasswordRequest.NewPassword).IsSuccess)
            {
                return PasswordChangeValidationResult.NewPasswordNotComplex;
            }

            return PasswordChangeValidationResult.Success;

        }


        /// <summary>
        /// Perform an action on the member with the given id
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="configAction"></param>
        private static void UpdateMember(int memberId, Action<member> configAction)
        {
            if (configAction == null) throw new ArgumentNullException(nameof(configAction));
            using (var db = new NESIMySQL())
            {
                var member = (from m in db.member
                              where (m.Member_ID == memberId && m.Member_Status == "Active")
                              select m).SingleOrDefault();

                if (member == null) throw new NesiException($"Memeber with id {memberId} not found");
                configAction(member);
                db.SaveChanges();
            }
        }

        private static bool CheckMember(int memberId, Func<member, bool> checkFunc)
        {
            if (checkFunc == null) throw new ArgumentNullException(nameof(checkFunc));
            using (var db = new NESIMySQL())
            {
                var member = (from m in db.member
                              where (m.Member_ID == memberId && m.Member_Status == "Active")
                              select m).SingleOrDefault();

                if (member == null) throw new NesiException($"Memeber with id {memberId} not found");
                return checkFunc(member);
            }
        }



        /// <summary>
        /// Set password flag for a member
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="flagValue"></param>
        private static void SetPasswordFlag(int memberId, bool flagValue) =>
            UpdateMember(memberId, member => { member.request_password_reset_flag = flagValue; });


        #endregion

    }
}