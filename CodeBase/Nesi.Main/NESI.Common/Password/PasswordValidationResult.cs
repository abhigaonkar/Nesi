using System.Linq;
using NESI.Common.Extensions;
using NESI.Common.Models;

namespace NESI.Common.Password
{
    /// <summary>
    /// Extend result to provide additional information
    /// </summary>
    public class PasswordValidationResult : NesiOperationResult
    {
        /// <summary>
        /// Reason why validation failed
        /// </summary>
        public PasswordValidationError Reason { get; private set; }

        /// <summary>
        /// Password validation was successful
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static PasswordValidationResult PasswordValidationSucceeded(string message = null) =>
            new PasswordValidationResult { IsSuccess = true, Reason = PasswordValidationError.None, Message = message };

        /// <summary>
        /// Indicate a password validation error
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static PasswordValidationResult PasswordValidationFailed(PasswordValidationError reason) =>
            new PasswordValidationResult { IsSuccess = false, Reason = reason, Errors = reason.GetSetFlagsDescriptions(PasswordValidationError.None).ToArray()};
    }
}