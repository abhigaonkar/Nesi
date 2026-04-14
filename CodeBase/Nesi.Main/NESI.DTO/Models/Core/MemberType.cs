using AutoMapper.Attributes;
using NESI.Data.Entities;

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(membertype))]
	[MapsFrom(typeof(membertype))]
	public class MemberType
	{
		public int membertype_id { get; set; }
		public string membertype_name { get; set; }
		public bool? active { get; set; }
		public bool? is_elevated { get; set; }
		public bool show_on_ratesheet { get; set; }
		public int reports_to { get; set; }
		public string objective { get; set; }
		public string benefitplan { get; set; }
		public int? ratesheet_order { get; set; }
		public bool? considered_pm { get; set; }
		public int mt_id_us { get; set; }
		public bool is_scheduled { get; set; }
		public bool is_team_leader { get; set; }
		public bool exec_severance_package { get; set; }
		public bool is_oncall { get; set; }
		public int org_chart_level { get; set; }
		public int ticket_vote_weight { get; set; }
		public int? membertype_classification { get; set; }
		public bool? can_see_holcos { get; set; }
		public bool? see_all_units { get; set; }
	}
}