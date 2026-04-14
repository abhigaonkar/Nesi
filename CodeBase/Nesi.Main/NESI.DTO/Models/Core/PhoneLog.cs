using System;
using AutoMapper.Attributes;

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(NESI.Data.Entities.phone_log))]
	[MapsFrom(typeof(NESI.Data.Entities.phone_log))]
	public class PhoneLog
	{
		public int phone_log_id { get; set; }
		public long? phone_log_altigen_session_id { get; set; }
		public DateTime? phone_log_date { get; set; }
		public string phone_log_from_number { get; set; }
		public string phone_log_to_number { get; set; }
		public string phone_log_from_name { get; set; }
		public string phone_log_to_name { get; set; }
		public int phone_log_duration { get; set; }
		public long phone_log_time { get; set; }
		public int phone_log_direction { get; set; }
		public string imei_ds { get; set; }
		public long calltime_nb { get; set; }
		public string notes { get; set; }
		public sbyte calltype_fg { get; set; }
		public sbyte numbertype_fg { get; set; }
		public bool? is_internal { get; set; }
		public bool updated_to_customer_history { get; set; }
	}
}