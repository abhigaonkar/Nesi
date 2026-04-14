using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper.Attributes;

namespace NESI.DTO.ViewModels.Page.Customers
{
	[MapsTo(typeof(nesi.core.customer_history))]
	[MapsFrom(typeof(nesi.core.customer_history))]
	public class CustomerHistory : CustomerBase
	{
		[Required]
		public DateTime date { get; set; }
		[Required]
		public int action_id { get; set; }
		[Required]
		public int member_id { get; set; }
		[Required]
		public int origin { get; set; }
		[Required]
		public string notes { get; set; }
	}
}