using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorPhoneNumber : VendorEditBase
	{
		public VendorPhoneNumber(Employee user, int id) : base(user, id)
		{

		}

		public new object Profile()
		{

			return new
			{
				address_id = vendor.Address.id,
				phoneList = GetPhoneNumberList()
			};
		}

		public DataExtra Save(DTO.ViewModels.Page.Vendors.VendorPhoneNumber model)
		{
			var p = new NePhoneNumbers(model.phone_numbers_id);
			p = (NePhoneNumbers)MapperFrom(p, model);
			p.phone_numbers_default = model.is_default;
			p.phone_numbers_active = model.is_active;
			p.type = "Address";
			p.phone_numbers_type = "Address";
			p.Save();

			return new DataExtra()
			{
				Data = "Phone number has been saved successfully.",
				Extra = GetPhoneNumberList()
			};
		}

		public DataExtra Delete(int phoneId)
		{
			bllToolbox.doSQL_void(@"delete from phone_numbers where phone_numbers_id=@p0", phoneId);
			return new DataExtra()
			{
				Data = "Phone number has been deleted successfully.",
				Extra = GetPhoneNumberList()
			};
		}

		public DataTable GetPhoneNumberList()
		{
			return bllToolbox.doSQL_dt(@"
				SELECT 
					phone_numbers_comm_type comm_type, 
					phone_numbers_number number, 
					phone_numbers_id,
					phone_numbers_default is_default,
					phone_numbers_active is_active,
					phone_numbers_type type,
					phone_numbers_table_id
				FROM 
					phone_numbers 
				WHERE 
					phone_numbers_type = 'Address' AND 
					phone_numbers_table_id = @p0", vendor.Address.id);
		}

	}
}