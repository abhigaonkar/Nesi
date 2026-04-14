using System;
using AutoMapper.Attributes;

// ReSharper disable InconsistentNaming

namespace NESI.DTO.Models.Users
{
	[MapsTo(typeof(NESI.Data.Entities.vendor))]
	[MapsFrom(typeof(NESI.Data.Entities.vendor))]
	public class Vendor
	{
		public int Vendor_ID { get; set; }
		public string vendor_number { get; set; }
		public int Vendor_Number_Int { get; set; }
		public string Vendor_Name { get; set; }
		public int? Vendor_Company_ID { get; set; }
		public int Vendor_GL_ID { get; set; }
		public int Vendor_CreditType { get; set; }
		public double Vendor_CreditLimit { get; set; }
		public string Vendor_Hold { get; set; }
		public string Vendor_CPRS { get; set; }
		public string Vendor_IDType { get; set; }
		public string Vendor_IDNumber { get; set; }
		public string Vendor_Account { get; set; }
		public string vendor_notes { get; set; }
		public string Vendor_Buyer { get; set; }
		public int Vendor_Term_ID { get; set; }
		public System.DateTime Vendor_CreatedDateTime { get; set; }
		public int Vendor_InitMember_ID { get; set; }
		public System.DateTime Vendor_LastDateTime { get; set; }
		public int Vendor_Member_ID { get; set; }
		public int? Vendor_QC_Member_ID { get; set; }
		public DateTime? Vendor_QC_DateTime { get; set; }
		public int? Vendor_PO_Exempt { get; set; }
		public int Vendor_Active { get; set; }
		public int? vendor_company_id_link { get; set; }
		public string vendor_website { get; set; }
		public System.DateTime vendor_ts { get; set; }
		public bool? is_partner { get; set; }
		public int? Vendor_GL_ID_Consol { get; set; }
		public bool? is_inspector { get; set; }
		public int business_unit_id { get; set; }
		public int? business_unit_id_link { get; set; }
	}
}