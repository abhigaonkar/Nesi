using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.CurrentUser
{
    /// <summary>
    /// Model we use for forgot password
    /// </summary>
    public class ForgotPasswordModel
    {
        /// <summary>
        /// Username of the user who forgot their password
        /// </summary>
        
        public string Username { get; set; }
    }
}