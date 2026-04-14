using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class InsertTimeSheetBase
	{
	

		[Required]
		public DateTime Date { get; set; }

		public DateTime LocalDate => Date.ToLocalTime();

		[Required]
		public int SelectedUserId { get; set; }
		[Required]
		public int SelectedBusinessUnitId { get; set; }
	
		[Required]
		public double NumberOfHours { get; set; }
		[Range(minimum: 0, maximum: 5)]
		public int? Rating { get; set; }
		[Range(minimum: 0, maximum: 100)]
		public double? PercentComplete { get; set; }
		[Required]
		public int MemberTime_WoComment_ID { get; set; }
		[Required]
		public string MemberTime_WoComment { get; set; }
		[Required]
		public int PayTypeId { get; set; }

		public virtual int CustId { get; set; }
		public virtual string CustNo { get; set; }
		public virtual string CustName { get; set; }
		public virtual string WoProg_Bvwo { get; set; }

        public int selectedJobType { get; set; }
        public string selectedJobTypeName { get; set; }
        public bool allow_jobtype_selection { get; set; }

        public string entry_type { get; set; }
        public List<TimesheetHourTypeRecord> timesheetHourTypeList { get; set; }
        public int? scope_id { get; set; }
        public int? prov_id { get; set; }

        public decimal? mileage_value { get; set; }

        public string mileage_unit { get; set; }

        // For transfer used only.
        public bool IsTransfer { get; set; }
        public string transferInfo { get; set; }
        public int originalChargeoutId { get; set; }
        public double OriginalPrice { get; set; }
        public double OriginalCost { get; set; }
        public int originalMemberTypeid { get; set; }
        public int IdForTransfer { get; set; }
        public int orginalChargeoutIdForParentWorkOrderLaborLIine { get; set; }
        public bool orginal_branch_can_see_jobtype { get; set; }
    }

    public class TimesheetHourTypeRecord
    {
        public int hourTypeId { get; set; }
        public string hourType { get; set; }
        public double hours { get; set; }
    }
}