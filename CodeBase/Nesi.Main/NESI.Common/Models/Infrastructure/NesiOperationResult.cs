using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common.Models
{
    /// <summary>
    /// Generic result of an operation class
    /// Used in place of returning plain strings as results
    /// </summary>
    public class NesiOperationResult
    {
        /// <summary>
        /// Require static construction
        /// </summary>
        protected NesiOperationResult()
        {}

        /// <summary>
        /// True iff operation was successful
        /// </summary>
        public bool IsSuccess { get; protected set; }

        /// <summary>
        /// Collection of errors, filled only when returning failure
        /// </summary>
        public IEnumerable<string> Errors { get; protected set; } = new List<string>();

        /// <summary>
        /// General success message, used only when <see cref="IsSuccess"/> is true
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Join all errors in a separated string
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string JoinErrors(string separator = ",") => string.Join(separator, Errors);

        /// <summary>
        /// Construct success result
        /// </summary>
        public static NesiOperationResult Success(string message = null) => new NesiOperationResult {IsSuccess = true, Message = message};

        /// <summary>
        /// Create failure result from one or more errors
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static NesiOperationResult Fail(params string[] errors) =>
            new NesiOperationResult {IsSuccess = false, Errors = errors};

        /// <summary>
        /// Create failure result from 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static NesiOperationResult Fail(Exception exception) =>
            new NesiOperationResult {IsSuccess = false, Errors = new []{exception.Message}};
    }
}
