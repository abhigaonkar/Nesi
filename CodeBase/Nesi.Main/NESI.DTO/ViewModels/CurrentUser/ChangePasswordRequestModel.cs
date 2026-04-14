using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.CurrentUser
{
    /// <summary>
    /// Model used to change an existing password
    /// </summary>
	public class ChangePasswordRequestModel
	{
	    [Required]
		[MinLength(3)]
		public string Username { get; set; }

		[Required]
		public string OldPassword { get; set; }

	    [Required]
		[MinLength(6)]
		public string NewPassword { get; set; }

	    [Required]
		[MinLength(6)]
		[Compare("NewPassword")]
		public string ConfirmPassword { get; set; }

	}
}