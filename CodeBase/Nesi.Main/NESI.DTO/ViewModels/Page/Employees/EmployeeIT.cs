using AutoMapper.Attributes;

namespace NESI.DTO.ViewModels.Page.Employees
{
	[MapsFrom(typeof(nesi.core.NeMember))]
	public class EmployeeIT
	{
		public int id { get; set; }
		public string emplogon { get; set; }
		public string Username { get; set; }
		public string NEEmail { get; set; }
		public string LDAP_user { get; set; }
		public string PhoneExtension { get; set; }
		public int business_unit_id { get; set; }
		public int cellphone_id { get; set; }
		public int cellphone_number_id { get; set; }
		public bool gets_barcodescanner { get; set; }
		public bool gets_phone { get; set; }
		public bool gets_neemail { get; set; }
		public bool gets_phoneext { get; set; }
		public bool gets_laptop { get; set; }
		public bool include_in_mobile_contactlist { get; set; }
		public bool is_LDAP { get; set; }

		public string last_mobile_login { get; set; }
	}
}