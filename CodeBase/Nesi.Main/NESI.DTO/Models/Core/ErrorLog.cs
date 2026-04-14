using System;
using AutoMapper.Attributes;
using NESI.Data.Entities;

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(error_log))]
	[MapsFrom(typeof(error_log))]
	public class ErrorLog
	{
		public int id { get; set; }
		public long Member_ID { get; set; }
		public System.DateTime dt { get; set; }
		public string error_desc { get; set; }
		public string error_short { get; set; }
		public sbyte? level { get; set; }
		public string host_url { get; set; }
		public string full_stacktrace { get; set; }
		public string user_ip { get; set; }
		public string origin { get; set; }
		public bool? is_global { get; set; }
		public string error_on_page { get; set; }
		public string error_on_line { get; set; }
	}
}