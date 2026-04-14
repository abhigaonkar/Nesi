using System;
using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorStartNew : VendorBase
	{
		public LabelValueString[] countryList { get; set; }
		public LabelValueString[] provList { get; set; }
		public LabelValueInt[] termList { get; set; }


		public VendorStartNew(Employee user) : base(user)
		{
			countryList = GetCountryList();
			provList = GetProvList();
			termList = GetTermList();
		}

		public DataExtra Save(DTO.ViewModels.Page.Vendors.VendorStartNew model)
		{
			var current_user = new NeMember(UserId);
			var vendobj = new NEVendor
			{
				Name = model.name,
				Hold = false,
				business_unit_id = CurrentUser.BusinessUnitId,
				Term_ID = model.term_id,
				PO_Exempt = false,
				CPRS = false,
				InitMember_ID = CurrentUser.Id,
				IDType = "B",
				IDNumber = "",
				Credit_Type = 1,
				Credit_Limit = 0,
				Account = "",
				Buyer = "",
				Notes = ""
			};

			var returned_id = vendobj.Save();
			vendobj.Number = returned_id.ToString();
			vendobj.Number_Int = returned_id;
			vendobj.id = returned_id;
			bllToolbox.doSQL_void(@"UPDATE vendor SET vendor_number = vendor_id, vendor_number_int = vendor_id  WHERE vendor_id =@v0", returned_id);
			vendobj.Address.Table_ID = returned_id;
			vendobj.Address.Table = "Vendor";
			vendobj.Address.Type = "B";
			vendobj.Address.Desc = "";
			vendobj.Address.Addr1 = model.addr1.Replace("'", "-");
			vendobj.Address.Addr2 = model.addr2.Replace("'", "-");
			vendobj.Address.Addr3 = model.addr3.Replace("'", "-");
			vendobj.Address.Addr4 = model.addr4.Replace("'", "-");
			vendobj.Address.City = model.city.Replace("'", "-");
			vendobj.Address.Postal = model.postal;
			vendobj.Address.Prov = model.prov;
			vendobj.Address.Country = model.country;
			vendobj.Address.PhoneArea = model.phonearea;
			vendobj.Address.Phonefirst = model.phonefirst;
			vendobj.Address.PhoneLast = model.phonelast;
			vendobj.Address.PhoneExt = model.phoneext;
			vendobj.Address.FaxArea = model.faxarea;
			vendobj.Address.FaxFirst = model.faxfirst;
			vendobj.Address.FaxLast = model.faxlast;
			vendobj.Address.Email = model.email;
			vendobj.Address.Web = model.web;
			vendobj.Address.RVAccount = "50110";
			vendobj.Address.RVAccount_Consol = "51000";
			vendobj.Address.Save();
			//try
			//{
			//	
			//	vendobj.sync_bvs(current_user);
			//}
			//catch (Exception)
			//{
			//	// ignored
			//}
			vendor_id = vendobj.id;
			return new DataExtra("New vendor has been created successfully.", this.vendor_id);
		}


		public LabelValueInt[] NameCheck(string name)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT vendor_id value, vendor_name label FROM vendor WHERE vendor_name LIKE  CONCAT('%',@v0,'%')  ORDER BY CAST(vendor_number AS UNSIGNED)", name.Trim());
		}

		public LabelValueInt[] PhoneCheck(string phone)
		{
			if (phone.Length == 10)
			{
				var phonearea = phone.Substring(0, 3);
				var phonefirst = phone.Substring(3, 3);
				var phonelast = phone.Substring(6, 4);

				return bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT b.vendor_id value, b.vendor_name label FROM address a
LEFT JOIN vendor b ON b.vendor_id = a.address_table_id  WHERE address_table = 'Vendor' and a.active=true
AND a.address_phonearea = @v0 AND a.address_phonefirst = @v1 
AND a.address_phonelast = @v2  ORDER BY CAST(b.vendor_number AS UNSIGNED)", phonearea, phonefirst, phonelast);
			}
			else
			{
				return null;
			}
		}
	}
}