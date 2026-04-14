using System;
using AutoMapper.Attributes;

// ReSharper disable InconsistentNaming
// ReSharper disable InconsistentNaming

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(NESI.Data.Entities.passport))]
	[MapsFrom(typeof(NESI.Data.Entities.passport))]
	public class Passport
	{
			public int id { get; set; }
			public DateTime? dt { get; set; }
			public string to { get; set; }
			public string from { get; set; }
			public bool? active { get; set; }
			public string reqhash { get; set; }
			public bool? valid { get; set; }
			public string url_yes { get; set; }
			public string url_no { get; set; }
			public long? to_member_id { get; set; }
			public string expiry_date { get; set; }
			public string yes_text { get; set; }
			public string no_text { get; set; }
	}
}
