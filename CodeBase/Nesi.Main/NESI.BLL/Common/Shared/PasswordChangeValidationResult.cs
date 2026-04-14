using System.ComponentModel;

namespace NESI.BLL.Common.Shared
{
    public enum PasswordChangeValidationResult
    {
        Unknown = 0,
        Success,
        
        [Description("UserName is incorrect.")]
        UserNameNotCorrect,
        [Description("Current password is incorrect.")]
        CurrentPasswordNotCorrect,
        [Description("Confirmation password does not match new password.")]
        ConfirmationPasswordNotMatched,
        [Description("New password cannot be the same as the old one.")]
        CurrentPasswordSameAsOld,
        [Description("New password does not fulfill complexity requirements.")]
        NewPasswordNotComplex
    }
}