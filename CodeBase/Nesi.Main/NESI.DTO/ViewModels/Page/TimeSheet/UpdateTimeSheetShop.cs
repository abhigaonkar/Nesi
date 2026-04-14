using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class UpdateTimeSheetShop : UpdateTimeSheetBase
	{
		[Required]
		public int MemberTime_ID { get; set; }
		[Range(minimum: 0, maximum: 5)]
		public int Rating { get; set; }
		[Required]
		public double NumberOfHours { get; set; }
		[Required]
		public int MemberTime_WoComment_ID { get; set; }
		[Required]
		public string MemberTime_WoComment { get; set; }
        public int? Internal_Project_Id { get; set; }

    }
}