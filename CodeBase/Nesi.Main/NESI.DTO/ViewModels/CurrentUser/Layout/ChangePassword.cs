using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.CurrentUser.Layout
{
   public class ChangePassword
    {
      
        [Required]
        public string password { get; set; }
        [Required]
        public string current_password { get; set; }
		public bool? sync_pass { get; set; }

	}
}
