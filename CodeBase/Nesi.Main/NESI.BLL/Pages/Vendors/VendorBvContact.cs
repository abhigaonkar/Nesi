using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorBvContact : VendorEditBase
	{
		protected int Index { get; set; }
		public VendorBvContact(Employee user, int id, int index) : base(user, id)
		{
			Index = index;
		}

		public new object Profile()
		{
			var entity = new DTO.ViewModels.Page.Vendors.VendorBvContact();
			var c = new BVContact(vendor.Address.id, Index, true);
			entity = (DTO.ViewModels.Page.Vendors.VendorBvContact)MapperFrom(entity, c);
			return new
			{
				entity,
			};
		}

		public DataExtra Save(DTO.ViewModels.Page.Vendors.VendorBvContact model)
		{
			var c = new BVContact(vendor.Address.id, Index);
			c = (BVContact) MapperFrom(c, model);
			c.Save();
			return new DataExtra("BV Contact has been saved successfully.");
		}
	}
}