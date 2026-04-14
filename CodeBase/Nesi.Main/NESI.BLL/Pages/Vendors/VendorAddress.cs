using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorAddress : VendorEditBase
	{
		public VendorAddress(Employee user, int id) : base(user, id)
		{

		}

		public new object Profile()
		{
			var entity = new DTO.ViewModels.Page.Vendors.VendorAddress();
			entity = (DTO.ViewModels.Page.Vendors.VendorAddress)MapperFrom(entity, vendor.Address);
			return new
			{
				countryList = GetCountryList(),
				provList = GetProvList(),
				entity
			};
		}

		public DataExtra Save(DTO.ViewModels.Page.Vendors.VendorAddress model)
		{
			vendor.Address = (NEAddress)MapperFrom(vendor.Address, model);
			vendor.Address.Save();
			return new DataExtra("Vendor address has been saved successfully.");
		}
	}
}