using System;
using AutoMapper.Attributes;

namespace NESI.DTO.Models.TimeSheet
{
	[MapsTo(typeof(NESI.Data.Entities.membertime))]
	[MapsFrom(typeof(NESI.Data.Entities.membertime))]
	public class MemberTime
	{
			public int MemberTime_ID { get; set; }
			public long membertime_memberid { get; set; }
			public DateTime Date { get; set; }
			public string MemberTime_Cust_No { get; set; }
			public long MemberTime_Customer_ID { get; set; }
			public string membertime_workorder_id { get; set; }
			public double? NumberOfHours { get; set; }
			public int? MemberTime_MemberTypeHours_ID { get; set; }
			public int? MemberTime_PayTypeHours_ID { get; set; }
			public int? MemberTime_WoComment_ID { get; set; }
			public string MemberTime_Mileage { get; set; }
			public string MemberTime_SRED { get; set; }
			public string MemberTime_Warranty { get; set; }
			public int? Member_ID_Create { get; set; }
			public int Member_ID_Audit { get; set; }
			public DateTime? Created_Date { get; set; }
			public DateTime? Modified_Date { get; set; }
			public string membertime_customer_name { get; set; }
			public string MemberTime_Premium { get; set; }
			public string WOType { get; set; }
			public int? child_shoptime { get; set; }
			public double? wo_percent_complete { get; set; }
			public string ProductCode { get; set; }
			public sbyte? SRED { get; set; }
			public double? Mileage { get; set; }
            
             public decimal? mileage_value { get; set; }
            public string mileage_unit { get; set; }
        public sbyte? Warrenty { get; set; }
			public string Memo { get; set; }
			public string EmpLogon { get; set; }
			public long? MemberTime_CompanyID { get; set; }
			public int? MemberTime_Child_CompanyID { get; set; }
			public string MemberTime_Child_WorkOrder_ID { get; set; }
			public string MemberTime_Child_Cust_No { get; set; }
			public string membertime_child_customer_name { get; set; }
			public int? MemberTime_WoComment_Child_ID { get; set; }
			public double MemberTime_Tax1 { get; set; }
			public double MemberTime_Tax2 { get; set; }
			public double MemberTime_Tax3 { get; set; }
			public double MemberTime_Tax4 { get; set; }
			public double MemberTime_CostPrice { get; set; }
			public double MemberTime_SellPrice { get; set; }
			public int? MemberTime_WOProg_id { get; set; }
			public int? MemberTime_Child_WOProg_id { get; set; }
			public int? membertime_shop_type_id { get; set; }
			public int? quote_section_id { get; set; }
			public DateTime ts { get; set; }
			public int? membertype_id { get; set; }
			public long? membertype_chargeout_id { get; set; }
			public int? rating { get; set; }
			public int? division_id { get; set; }
			public int? business_unit_id { get; set; }
			public int? child_business_unit_id { get; set; }
			public int? child_division_id { get; set; }
	}
}