using System.ComponentModel.DataAnnotations;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Demo
{

    public class CheckUserNamePassword
    {
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        [SQLInjection()]
       public string username { get; set;}
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        [SQLInjection()]
        public string password { get; set;}
    }
}