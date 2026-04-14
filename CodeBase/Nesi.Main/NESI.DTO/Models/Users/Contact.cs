using AutoMapper.Attributes;
// ReSharper disable InconsistentNaming

namespace NESI.DTO.Models.Users
{
	[MapsTo(typeof(NESI.Data.Entities.contact))]
	[MapsFrom(typeof(NESI.Data.Entities.contact))]
	public class Contact
	{
		public int Contact_ID { get; set; }
		public int Contact_Cust_ID { get; set; }
		public string contact_bvno { get; set; }
		public int contact_quotemdb_id { get; set; }
		public string Contact_Name { get; set; }
		public string Contact_Email { get; set; }
		public string Contact_Title { get; set; }
		public string Contact_CellPhone { get; set; }
		public string Contact_Status { get; set; }
		public string Contact_Birthday { get; set; }
		public string Contact_Extension { get; set; }
		public string Contact_Login { get; set; }
		public string Contact_Password { get; set; }
		public string contact_type { get; set; }
		public int contact_status_id { get; set; }
		public int Contact_NesiMemberID { get; set; }
		public int Contact_Login_Enabled { get; set; }
		public string contact_directline { get; set; }
		public int contact_page_id { get; set; }
		public bool contact_password_set { get; set; }
		public System.DateTime ts { get; set; }
		public string name_first { get; set; }
		public string name_last { get; set; }
		public int address_id { get; set; }
		public string facebook { get; set; }
		public string twitter { get; set; }
		public string linkedin { get; set; }
		public int stopsurveys { get; set; }
	}
}