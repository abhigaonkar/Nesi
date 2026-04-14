using System;

namespace NESI.DTO.ViewModels.Page.Applicant
{
	public class ApplicantEdit
	{
		public int id { get; set; }
		public string firstname { get; set; }
		public string lastname { get; set; }
		public int business_unit_id { get; set; }
		public int membertypeid { get; set; }
		public string notes { get; set; }
		public string status { get; set; }
		public int becomes_memberid { get; set; }
		public string address { get; set; }
		public string city { get; set; }
		public string province { get; set; }
		public string country { get; set; }
		public string postal { get; set; }
		public string cellphone { get; set; }
		public string email { get; set; }
		public string homephone { get; set; }
		public string apt { get; set; }
		public int addedbymemberid { get; set; }
		public DateTime dateentered { get; set; }
	}
}