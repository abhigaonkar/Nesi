using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Vendors
{
	public class VendorStartNew : VendorAddress
	{
		public int table_id { get; set; }
		public string table { get; set; }
		public string desc { get; set; }
		public string type { get; set; }
		public int number_int { get; set; }
		[Required]
		public string name { get; set; }
		public bool hold { get; set; }
		[Required]
		public int business_unit_id { get; set; }
		[Required]
		public int term_id { get; set; }
		public bool cprs { get; set; }
		public int InitMember_ID { get; set; }
		public string idtype { get; set; }
		public string idnumber { get; set; }
		public int credit_type { get; set; }
		public int credit_limit { get; set; }
		public string account { get; set; }
		public string buyer { get; set; }
		public string notes { get; set; }
		public string rvAccount { get; set; }
		public string rvAccount_Consol { get; set; }
	}
}