using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorAccounting : VendorEditBase
	{
		public VendorAccounting(Employee user, int id) : base(user, id)
		{

		}

		public new object Profile()
		{
			var entity = new DTO.ViewModels.Page.Vendors.VendorAccounting();
			entity = (DTO.ViewModels.Page.Vendors.VendorAccounting)MapperFrom(entity, vendor);
			return new
			{
				countryList = GetCountryList(),
				provList = GetProvList(),
				termList = GetTermList(),
				taxList = TaxList(),
				creditTypeList = CreditTypeList(),
				idTypeList = IdTypeList(),
				entity
			};
		}

		public DataExtra Save(DTO.ViewModels.Page.Vendors.VendorAccounting model)
		{
			var v2 = (NEVendor)MapperFrom(vendor, model);
			v2.Vendor_CPRS = model.cprs ? "T" : "F";
			v2.Vendor_PO_Exempt = model.po_exempt ? 1 : 0;
			v2.Address.Tax1 = model.tax1;
			v2.Address.Tax2 = model.tax2;
			v2.Address.Tax3 = model.tax3;
			v2.Address.Tax4 = model.tax4;
			v2.Address.Save();
			v2.Save();
			return new DataExtra("Vendor accounting has been saved successfully.");
		}
	}
}