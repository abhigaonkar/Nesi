
using System.ComponentModel.DataAnnotations;
namespace NESI.DTO.ViewModels.Page.TimeSheet
{
    public class UpdateTimeSheetProject
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string name { get; set; }

    }
}
