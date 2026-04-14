using NESI.Common.Extensions;
using NESI.Common.Models;

namespace NESI.Common.Password
{
    /// <summary>
    /// Extend result to provide additional information
    /// </summary>
    public class TokenValidationResult : NesiOperationResult
    {
        /// <summary>
        /// Reason why validation failed
        /// </summary>
        public TokenValidationErrorReason Reason { get; private set; }

        /// <summary>
        /// Token validation was successful
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static TokenValidationResult TokenValidationSuccess(string message =null) => 
            new TokenValidationResult { IsSuccess = true, Reason = TokenValidationErrorReason.Unknown, Message = message };

        /// <summary>
        /// Indicate a token validation error
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static TokenValidationResult TokenValidationError(TokenValidationErrorReason reason) =>
            new TokenValidationResult { IsSuccess = false, Reason = reason, Errors = new []{ reason.GetEnumDescription()}};
    }
}