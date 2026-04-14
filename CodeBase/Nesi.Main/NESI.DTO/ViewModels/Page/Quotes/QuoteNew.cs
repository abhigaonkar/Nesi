using System;
using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteNew
	{
		[Required]
		public int Customer_id { get; set; }
		[Required]
		public int Customer_address { get; set; }
		[Required]
		public int Customer_contact { get; set; }
		[Required]
		[Range(0.01, double.MaxValue)]
		public double Expected_value { get; set; }
		[Required]
		[Range(30,90)]
		public int Chance_winning { get; set; }
		[Required]
	    public DateTime Date_due { get; set; }
	    [Required]
	    public DateTime Date_expected_start { get; set; }
	    [Required]
        public DateTime Date_invoiced { get; set; }
		[Required]
		public string Job_description { get; set; }
		[Required]
		public int Quote_businessUnit { get; set; }
        [Required]
        public int revenueLine { get; set; }
        [Required]
		public int Managed_by { get; set; }
		[Required]
		public int bdm { get; set; }
		public int[] Qualifiers { get; set; }
		public string Business_unit_name { get; set; }
		public string Customer_name { get; set; }
		public string Customer_address_name { get; set; }
		public string Customer_contact_name { get; set; }
		public string Managed_by_name { get; set; }
		public bool quoter_locked { get; set; }

	}
}