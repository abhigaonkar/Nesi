using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class UpdateTimeSheetWorkOrder : UpdateTimeSheetBase
	{
		[Required]
		public int MemberTime_ID { get; set; }
		[Range(minimum: 0, maximum: 5)]
		public int Rating { get; set; }
		[Range(minimum: 0, maximum: 100)]
		public double PercentComplete { get; set; }
		[Required]
		public double NumberOfHours { get; set; }
		[Required]
		public int MemberTime_WoComment_ID { get; set; }
		[Required]
		public string MemberTime_WoComment { get; set; }
		public int SelectedWorkOrderId { get; set; }

        public int selectedJobType { get; set; }
        public bool allow_jobtype_selection { get; set; }
        public int? scope_id { get; set; }
        public int prov_id { get; set; }

        public decimal? mileage_value { get; set; }

        public string mileage_unit { get; set; }

        public bool is_prevailing_wage { get; set; }
    }
}