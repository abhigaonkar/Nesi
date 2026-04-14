using System;
using AutoMapper.Attributes;

namespace NESI.DTO.ViewModels.Page.Employees
{
	[MapsFrom(typeof(nesi.core.NeMember))]
	public class EmployeeUserInfo
	{
		public int id { get; set; }
		public string benefits_id { get; set; }
		public string life_insurance_id { get; set; }
		public string PayrollNo { get; set; }
		public string benefits_startdate { get; set; }
		public int business_unit_id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Nickname { get; set; }
		public string FullName { get; set; }
		public string MiddleInitial { get; set; }
		public string PhoneAreaCode { get; set; }
		public string PhoneFirst { get; set; }
		public string PhoneLast { get; set; }
		public string Email { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string Address { get; set; }
		public string NECellPhoneArea { get; set; }
		public string NECellPhoneFirst { get; set; }
		public string NECellPhoneLast { get; set; }
		public string NETruck { get; set; }
		public string SIN { get; set; }
		public string PhoneExtension { get; set; }
		public string DriversLicence { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string apprentice_contract { get; set; }
		public string ElectricalLicence { get; set; }
		public string BirthDate { get; set; }
		public string Prov { get; set; }
		public int MemberTypeID { get; set; }
		public int reports_to { get; set; }
		public bool is_CAN_boardmember { get; set; }
		public bool is_US_boardmember { get; set; }
		public bool tswatch { get; set; }
		public string Status { get; set; }
		public string HasDependants { get; set; }
		public int member_default_location { get; set; }
		public string Country { get; set; }
		public int paytype_id { get; set; }
		public int payroll_handler { get; set; }
		public int hrstatus_id { get; set; }
		public string NEEmail { get; set; }
		public string StartDate { get; set; }
		public string TerminateDate { get; set; }

		public string EmergencyFirstName1 { get; set; }
		public string EmergencyLastName1 { get; set; }
		public string EmergencyPhoneArea1 { get; set; }
		public string EmergencyPhoneFirst1 { get; set; }
		public string EmergencyPhoneLast1 { get; set; }
		public string pecell_area { get; set; }
		public string pecell_pref { get; set; }
		public string pecell_suff { get; set; }
		public string empnotes { get; set; }
		public bool part_time { get; set; }
		public double memberwage { get; set; }
	//	public DateTime Terminate_Date { get; set; }
		public DateTime Start_Date { get; set; }
	//	public DateTime Birth_Date { get; set; }

		public bool is_backoffice { get; set; }
		public string reporting_text { get; set; }
		public bool ReceivesAutoVacationPayout { get; set; }
	}
}