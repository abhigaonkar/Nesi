using nesi.core;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeContact : EmployeeEdit
	{
		public EmployeeContact(Employee current_user) : base(current_user)
		{
		}

		public EmployeeContact(Employee current_user, int mid) : base(current_user, mid)
		{

		}

		public new object Profile()
		{

			var entity = new DTO.ViewModels.Page.Employees.EmployeeContact();
			entity = (DTO.ViewModels.Page.Employees.EmployeeContact)MapperFrom(entity, n1_member);
			entity.Membertype_name = n1_member.membertype.name;
			entity.Business_unit_name = n1_member.business_unit.name;
			return new
			{
				entity
			};

		}
	}
}