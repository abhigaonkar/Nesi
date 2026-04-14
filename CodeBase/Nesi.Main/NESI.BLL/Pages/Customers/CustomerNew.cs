using System;
using System.Linq;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerNew : CustomerBase
	{

		public LabelValueInt[] BusinessUnitList { get; set; }
		public LabelValueInt[] AccountManagerList { get; set; }
		public LabelValueInt[] ProjectManagerList { get; set; }
		public LabelValueInt[] CustomerOriginList { get; set; }
		public LabelValueString[] CountryList { get; set; }
		public LabelValueString[] ProvList { get; set; }
		public LabelValueInt[] GlReceiveList { get; set; }
		public LabelValueInt[] TaxList { get; set; }
		public object BusinessUnitDefaultProfile { get; set; }
		public LabelValueInt[] RegAccountManagerList { get; set; }
		public CustomerNew(Employee user) : base(user)
		{
		}

		public new CustomerNew Profile()
		{
			BusinessUnitList = NESI.BLL.Common.Cache.Global.BusinessUnit.GetActiveList()
				.Select(x => new LabelValueInt { Label = x.ddl_Name, Value = x.ID }).ToArray();
			AccountManagerList = GetAccountManagerList();
			CustomerOriginList = GetCustomerOriginList();
			CountryList = GetCountryList();
			ProvList = GetProvList();
			TaxList = GetTaxList(true);
			BusinessUnitDefaultProfile = getBusinessUnitDefaultProfile(CurrentUser.BusinessUnitId);
			RegAccountManagerList = GetRegAccountManagerList();
			return this;
		}

		public object getBusinessUnitDefaultProfile(int buId)
		{
			var bu = new NeBusinessUnit(buId);
			return new
			{
				country = bu.country,
				provstate = bu.provstate,
				GlReceiveList = GetGlReceiveList(buId),
				ProjectManagerList = GetProjectManagerList(buId),
			};
		}

		public LabelValueInt[] GetCustomerOriginList()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"
			SELECT id value, name label FROM customer_origin
			");
		}

		public DataExtra Save(DTO.ViewModels.Page.Customers.CustomerNew model)
		{
			var this_customer = new NECustomer();
			var this_address = new NEAddress();
			if (model.account_manager_id == 999999) model.account_manager_id = 0;
			if (model.reg_account_manager_id == 999999) model.reg_account_manager_id = 0;
			if (model.isr == 999999) model.isr = 0;
			if (model.osr == 999999) model.osr = 0;
			if (model.tax_1 == 999999) model.tax_1 = 0;
			if (model.tax_2 == 999999) model.tax_2 = 0;
			if (model.tax_3 == 999999) model.tax_3 = 0;
			if (model.tax_4 == 999999) model.tax_4 = 0;

			this_customer.Customer_Name = model.customer_name;
			this_customer.business_unit_id = model.business_unit;
			this_customer.Credit_Type = 1;
			this_customer.TaxPrompt = "F";
			this_customer.Hold = "T";
			this_customer.customer_whyhold = "New Customer";
			this_customer.StatementType = "F";
			this_customer.InvoiceType = "F";
			this_customer.InitMember_ID = UserId;
			this_customer.Member_ID = UserId;
			this_customer.customer_lastorigin = model.origin_id;
			this_customer.Notes = "";

			this_customer.ApplyFinanceCharges = "F";
			this_customer.Account_Manager = model.account_manager_id;

			this_address.Type = "B";
			this_address.Desc = "";
			this_address.Addr1 = model.address_line_1;
			this_address.Addr2 = model.address_line_2;
			this_address.Addr3 = model.address_line_3;
			this_address.Addr4 = model.address_line_4;
			this_address.City = model.address_city;
			this_address.Prov = model.address_prov;
			this_address.Postal = model.address_postalcode;
			this_address.Country = model.address_country;
			this_address.PhoneArea = model.phone_area;
			this_address.Phonefirst = model.phone_prefix;
			this_address.PhoneLast = model.phone_suffix;
			this_address.PhoneExt = model.phone_ext;
			this_address.Email = model.ap_email;
			this_customer.customer_autostatement_address = model.ap_email;
			this_address.FaxArea = model.fax_area;
			this_address.FaxFirst = model.fax_prefix;
			this_address.FaxLast = model.fax_suffix;

			// Customer RVAccount # 00000
			this_address.RVAccount = "00000";
			this_address.RVAccount_Consol = "41000";

			this_address.Tax1 = model.tax_1;
			this_address.Tax2 = model.tax_2;
			this_address.Tax3 = model.tax_3;
			this_address.Tax4 = model.tax_4;

			this_address.SellPrice = "01";
			this_address.Table = "Customer";
			var _cust_id = this_customer.Save();
			this_address.Table_ID = _cust_id;
			this_address.id = (int)_cust_id;
			var _addr_id = this_address.Save();
			var sp = new customer_sales_properties(_addr_id)
			{
				address_id = _addr_id,
				customer_id = _cust_id,
				status_id = 1,
				sector = "",
				po_required = false,
				project_mgr_member_id = model.project_manager_id,
				next_followup_date = DateTime.Now.AddMonths(1),
				origin = this_customer.customer_lastorigin
			};
			// VP of Sales check for setting default sales reps(Member Type ID: 37)
			if (CurrentUser.MemberType.membertype_id == 37)
			{
				sp.isr_member_id = 0;
				sp.ram_member_id = 0;
				sp.osr_member_id = 0;
				//if ((int)newcustomer_isr.Value != 0)
				//{
				//	sp.isr_member_id = (int)newcustomer_isr.Value;
				//}

				//if ((int)newcustomer_osr.Value != 0)
				//{
				//	sp.ram_member_id = (int)newcustomer_osr.Value;
				//}
				//if ((int)newcustomer_ram.Value != 0)
				//{
				//	sp.osr_member_id = (int)newcustomer_ram.Value;
				//}
			}
			sp.save();
			new CustomerHomeSearch(CurrentUser).ClearCache();
			try
			{
				//this_customer.sync_bvs(new NeMember(UserId));
			}
			catch (Exception e)
			{
				Console.Write(e.ToString());
			}
			 
			return new DataExtra()
			{
				Data = "success.",
				Extra = _cust_id
			};
		}
	}
}