using System;
using System.ComponentModel;

namespace NESI.Common.Password
{
    /// <summary>
    /// Status of a syntactic password verification. All applicable flags are returned
    /// </summary>
    [Flags]
    public enum PasswordValidationError
    {
        None = 0,
        [Description("Password is too short - Passwords must be at least 8 characters long.")]
        PasswordTooShort = 1,
        [Description("No non-letter or digit")]
        NoNonletterOrDigit = 2,
        [Description("No lowercase")]
        NoLowercase = 4,
        [Description("No uppercase")]
        NoUppercase = 8,
        [Description("No digit")]
        NoDigit = 16
    }
}