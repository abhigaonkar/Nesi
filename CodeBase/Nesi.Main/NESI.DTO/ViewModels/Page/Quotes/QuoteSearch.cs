using System;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteSearch
	{
		[SQLInjection()]
		public string Quote_id { get; set; }
		[SQLInjection()]
		public string Quote_by { get; set; }
		[SQLInjection()]
		public string Quote_businessUnit { get; set; }
		[SQLInjection()]
		public string Date_before { get; set; }
		[SQLInjection()]
		public string Date_after { get; set; }
		[SQLInjection()]
		public string Job_description { get; set; }
		[SQLInjection()]
		public string Customer_name { get; set; }
		[SQLInjection()]
		public string Status { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
	}
}