using System;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class Timesheet : DTO.Models.TimeSheet.MemberTime
	{
		public int Id { get; set; }
		public int BusinessUnitId { get; set; }
		public string WorkorderId { get; set; }
		public string bvwo { get; set; }
		public string CustNo { get; set; }
		public long CustId { get; set; }
		public string CustName { get; set; }
		public double Hours { get; set; }
		public string ChildWo { get; set; }
		public double Miles { get; set; }
		public int? WoCommentId { get; set; }
		public int? PayTypeId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LocalDate => Date.ToLocalTime();
        public int? internal_project_id { get; set; }
        public string HourType { get; set; }
		public int HourTypeId { get; set; }
		public string MemberUser { get; set; }
		public int MemberId { get; set; }
		public string MemberPremium { get; set; }
		public string Comments { get; set; }
		public bool ButtonVisible { get; set; }
		// hide these fields for client side.
		public new double MemberTime_CostPrice => 0;
		public new double MemberTime_SellPrice => 0;
		public int TsLitePaytypeId { get; set; }
        public string membertype_name { get; set; }
        public int? scope_id { get; set; }
        public string scope_name { get; set; }
        public int? prov_id { get; set; }

        //public string mileage_unit { get; set; }
        public int WoTypeId
		{
			get
			{
				if (string.IsNullOrEmpty(WOType)) return 0;
				switch (WOType.ToLower())
				{
					case "wo":
						return 0;
					case "quote":
						return 1;
					case "telem":
						return 2;
					case "shop":
						return 3;
					default:
						return 0;
				}
			}
		}

        public int? payperiod_id { get; set; }
        public string transfer_type { get; set; }
        public double sell { get; set; }
        public double cost { get; set; }
        public bool can_be_transferred { get; set; }
        public int? OrigMemberTime_ID { get; set; }
        public bool can_delete_transfered_item { get; set; }
        public bool can_edit_transfered_item { get; set; }

    }
}