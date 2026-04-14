using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NESI.WebAPI.Models
{
    /// <summary>
    /// Used to return more information to the front end
    /// </summary>
    public class ApiResult
    {
        public const string DefaultSuccessMessage = "Success";
        public const string DefaultFailureMessage = "Unknown error";

        /// <summary>
        /// Indicates that the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Either information or error message depending on the <see cref="Success"/> value
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Static constructor for ease of use
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult SuccessResult(string message = DefaultSuccessMessage) =>
            new ApiResult {Success = true, Message = message};

        /// <summary>
        ///  Static constructor for ease of use
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult FailureResult(string message = DefaultFailureMessage) =>
            new ApiResult {Success = false, Message = message};
    }
}