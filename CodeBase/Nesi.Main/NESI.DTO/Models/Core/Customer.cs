using System;
using AutoMapper.Attributes;
using NESI.Data.Entities;

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(customer))]
	[MapsFrom(typeof(customer))]
	public class Customer
	{
		public int customer_id { get; set; }
		public string Customer_Number { get; set; }
		public int? customer_number_int { get; set; }
		public string customer_name { get; set; }
		public int? Customer_Company_ID { get; set; }
		public short Customer_CreditType { get; set; }
		public double Customer_CreditLimit { get; set; }
		public double Customer_Discount { get; set; }
		public string customer_notes { get; set; }
		public short? Customer_StatementCode { get; set; }
		public string Customer_ServiceChargeCode { get; set; }
		public string Customer_TaxPrompt { get; set; }
		public string Customer_Hold { get; set; }
		public string customer_pricecode { get; set; }
		public string Customer_StatementType { get; set; }
		public string customer_invoicetype { get; set; }
		public string Customer_PORequired { get; set; }
		public string customer_apply_finance { get; set; }
		public string customer_GL_ID { get; set; }
		public System.DateTime Customer_CreatedDateTime { get; set; }
		public int Customer_InitMember_ID { get; set; }
		public System.DateTime Customer_LastDateTime { get; set; }
		public int Customer_Member_ID { get; set; }
		public string Customer_Error { get; set; }
		public int? Customer_QC_Member_ID { get; set; }
		public DateTime? Customer_QC_DateTime { get; set; }
		public int? Customer_Status { get; set; }
		public string customer_memo { get; set; }
		public int? Customer_Account_Manager { get; set; }
		public int? Customer_YearEnd { get; set; }
		public string Customer_NextDate { get; set; }
		public string Customer_SalesNotes { get; set; }
		public string customer_considerations { get; set; }
		public int Customer_Frequency_days { get; set; }
		public int? Customer_ControlsGuy { get; set; }
		public int? customer_qc2_member_id { get; set; }
		public DateTime? customer_qc2_datetime { get; set; }
		public int customer_autostatements { get; set; }
		public string customer_invoice_address { get; set; }
		public string customer_autostatement_address { get; set; }
		public string customer_invoice_ccaddress { get; set; }
		public string customer_autostatement_ccaddress { get; set; }
		public int? customer_creditdays { get; set; }
		public int? customer_collection_status { get; set; }
		public string customer_arnotes { get; set; }
		public string customer_whyhold { get; set; }
		public int? customer_whohold { get; set; }
		public string customer_name_raw { get; set; }
		public double? customer_budget_threshold { get; set; }
		public int? customer_decision_maker { get; set; }
		public string customer_followupnotes { get; set; }
		public System.DateTime customer_ts { get; set; }
		public int? customer_default_invoicetype { get; set; }
		public int customer_auto_invoice { get; set; }
		public bool? confirmed_po { get; set; }
		public bool? confirmed_taxexempt { get; set; }
		public int? customer_origin { get; set; }
		public string customer_accountcode { get; set; }
		public int? isr { get; set; }
		public int? osr { get; set; }
		public int? ram { get; set; }
		public int? mam { get; set; }
		public int? cisr { get; set; }
		public int? customer_lastorigin { get; set; }
		public double? margin { get; set; }
		public string facebook { get; set; }
		public string twitter { get; set; }
		public string linkedin { get; set; }
		public int? requires_wo_copy { get; set; }
		public bool? requires_quote_split { get; set; }
		public bool? is_partner { get; set; }
		public string customer_gl_id_consol { get; set; }
        public int? business_unit_id { get; set; }
	}
}