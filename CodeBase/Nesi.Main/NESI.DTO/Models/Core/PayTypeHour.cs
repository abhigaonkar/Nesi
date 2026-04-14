using System;
using AutoMapper.Attributes;

// ReSharper disable InconsistentNaming

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(NESI.Data.Entities.paytypehours))]
	[MapsFrom(typeof(NESI.Data.Entities.paytypehours))]
	public class PayTypeHour
	{
		public int PayTypeHours_ID { get; set; }
		public string Description { get; set; }
		public string abbreviation { get; set; }
		public double? multiplier { get; set; }
	}
}