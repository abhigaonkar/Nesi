using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.CurrentUser
{
    /// <summary>
    /// Model used to reset password
    /// </summary>
	public class ResetPassword
	{
		[Required]
		[MaxLength(1000)]
		public string Token { get; set; }
		[Required]
		[MaxLength(150)]
		public string Username { get; set; }
		[Required]
		[MaxLength(50)]
		public string Password { get; set; }
		[Required]
		[MaxLength(50)]
		public string RePassword { get; set; }
	}
}