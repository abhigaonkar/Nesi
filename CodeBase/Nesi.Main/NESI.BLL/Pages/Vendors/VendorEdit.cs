using System;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorEdit : VendorEditBase
	{
		public string name { get; set; }
		public bool vendor_hold { get; set; }
		public bool is_partner { get; set; }
		public int business_unit_id { get; set; }
		public string number { get; set; }
		public DateTime? qc_date { get; set; }
		public string date_added { get; set; }
		public string added_by { get; set; }

		public VendorEdit(Employee user, int id) : base(user, id)
		{


		}

		public new object Profile()
		{

			name = vendor.Name;
			business_unit_id = vendor.business_unit_id;
			vendor_hold = vendor.Vendor_Hold == "T";
			is_partner = vendor.is_partner;
			number = vendor.Number;
			if (vendor.QC_DateTime != new DateTime())
			{
				qc_date = vendor.QC_DateTime;
			}
			date_added = vendor.Vendor_CreatedDateTime.ToString("yyyy-MM-dd hh:mm:ss");
			added_by = new NeMember(vendor.InitMember_ID).FullName;
			var entity = new
			{
				vendor_id,
				name,
				business_unit_id,
				vendor_hold,
				is_partner,
				number,
				qc_date,
				date_added,
				added_by,
			};
			return new
			{
				businessUnitList = VisibleBusinessUnit(),
				entity
			};
		}

		public DataExtra Save(DTO.ViewModels.Page.Vendors.VendorEdit model)
		{
			vendor.Name = model.name;
			vendor.business_unit_id = model.business_unit_id;
			vendor.is_partner = model.is_partner;
			vendor.Vendor_Hold = model.vendor_hold ? "T" : "F";
			vendor.Save();
			return new DataExtra("Vendor has been saved successfully.");
		}
	}
}