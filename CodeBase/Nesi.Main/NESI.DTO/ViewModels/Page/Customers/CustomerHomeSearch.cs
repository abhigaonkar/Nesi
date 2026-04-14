using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Customers
{
	[ModelDefination("CustomerHomeSearch")]
	public class CustomerHomeSearch : ModelBase<CustomerHomeSearch>
	{
		public int customer_id { get; set; }
		public string customer_name { get; set; }
		public string customer_number { get; set; }
		public int hits { get; set; }
		public string matches { get; set; }
		public string city { get; set; }
		public string provstate { get; set; }
		public string phone { get; set; }
		public string address_ { get; set; }
		public int qc1 { get; set; }
		public int qc2 { get; set; }
		public DateTime dateadded { get; set; }
		public string hold { get; set; }
		public string membername { get; set; }
		public string statuss { get; set; }
		public string cust_status { get; set; }
		public string address_table { get; set; }
         
        public string active { get; set; }
		}
}