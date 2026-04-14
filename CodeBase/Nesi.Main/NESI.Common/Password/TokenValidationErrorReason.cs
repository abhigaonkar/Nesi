using System.ComponentModel;

namespace NESI.Common.Password
{
    /// <summary>
    /// Indicates the reason token validation has failed
    /// </summary>
    public enum TokenValidationErrorReason
    {
        Unknown = 0,
        [Description("Parse Error")]
        ParseError,
        [Description("Expired Token")]
        ExpiredToken,
        [Description("Invalid Identity")]
        InvalidIdentity,
        [Description("Purpose Mismatch")]
        PurposeMismatch,
        [Description("Token Already Used")]
        TokenAlreadyUsed
    }
}