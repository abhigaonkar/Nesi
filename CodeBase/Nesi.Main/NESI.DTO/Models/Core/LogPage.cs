using System;
using AutoMapper.Attributes;
using NESI.Data.Entities;

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(log_page))]
	[MapsFrom(typeof(log_page))]
	public class LogPage
	{
		public int id { get; set; }
		public System.DateTime dt { get; set; }
		public int member_id { get; set; }
		public string ip_address { get; set; }
		public string url { get; set; }
		public string query_string { get; set; }
		public DateTime? request_start { get; set; }
		public DateTime? request_end { get; set; }
		public DateTime? render_end { get; set; }
		public int? elements { get; set; }
		public int? cl_process { get; set; }
		public string host { get; set; }
	}
}