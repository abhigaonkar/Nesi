using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorBase : BLLBase
	{
		public int vendor_id { get; set; }
		public bool can_save_vendor { get; set; }
		public bool can_edit_vendor_contacts { get; set; }

		public bool is_back_office { get; set; }

		public string current_prov { get; set; }
		public string current_country { get; set; }
		

		public VendorBase(Employee user) : base(user)
		{
			can_save_vendor = CurrentUser.AuthenticatedForPrivilege(12);
			can_edit_vendor_contacts = CurrentUser.AuthenticatedForPrivilege(108);
			is_back_office = CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault();
			current_prov = CurrentUser.EmployeeProfile.Member_Prov;
			current_country = CurrentUser.EmployeeProfile.member_country;
			current_country = current_country == "CAN" ? "CDN" : current_country;
		}

		public VendorBase Profile()
		{
			return this;
		}

		protected LabelValueInt[] GetTermList()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"
SELECT term_id value, CAST(CONCAT(term_code,' - ', term_desc) AS CHAR) label FROM term
			");
		}

		protected LabelValueString[] IdTypeList()
		{
			return new[]
			{
				new LabelValueString("Business","B"), 
				new LabelValueString("S.I.N (Social)","S"),
			};
		}

		protected LabelValueString[] CreditTypeList()
		{
			return new[]
			{
				new LabelValueString("Unlimited","1"),
				new LabelValueString("No Credit","0"),
				new LabelValueString("Limit","2"),
			};
		}

		protected LabelValueInt[] TaxList()
		{
		return	bllToolbox.doSQL_Array<LabelValueInt>(@"
				Select 0 value, 'none' label union Select tax_id value, tax_name label from tax where is_active = 1
				");
		}
	}
}