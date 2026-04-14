using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerPhoneNumber : CustomerBase
	{
		public int address_id { get; set; }

		public CustomerPhoneNumber(Employee user) : base(user)
		{
		}
		public CustomerPhoneNumber(Employee user, int cust_id, int add_id) : base(user)
		{
			this.customer_id = cust_id;
			this.address_id = add_id;

		}

		public new object Profile()
		{
			return new
			{
				phoneList = GetPhoneNumberList()
			};
		}

		public DataExtra Save(DTO.ViewModels.Page.Customers.CustomerPhoneNumber model)
		{
			// ReSharper disable once RedundantAssignment
			var p = new NePhoneNumbers(model.phone_numbers_id);

			// p = AutoMapper.Mapper.Map<NePhoneNumbers>(model);
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


		public DataExtra Delete(int id)
		{
			bllToolbox.doSQL_void(@"delete from phone_numbers where phone_numbers_id=@p0", id);
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
					phone_numbers_table_id = @p0", address_id);
		}

	}
}