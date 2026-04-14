using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Applicant
{
	[ModelDefination("ApplicantHomeGrid")]
	public class ApplicantHomeGrid : ModelBase<ApplicantHomeGrid>
	{
		public int id { get; set; }
		public int addedbymemberid { get; set; }
		public string name { get; set; }
		public string addedbyname { get; set; }
		public DateTime dateentered { get; set; }
		public string status { get; set; }
		public int membertypeid { get; set; }
		public string membertype_name { get; set; }
		public int business_unit_id { get; set; }
		public string business_unit_name { get; set; }
		public string cellphone { get; set; }
		public string email { get; set; }
		public string offer_status { get; set; }
		public string notes { get; set; }
        public string becomes_memberid { get; set; }
	}
}