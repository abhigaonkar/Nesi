using System;
using nesi.core;
using NESI.BLL.Common.Shared;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using dto = NESI.DTO.ViewModels.Page.Employees.EmployeeIT;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeIT : EmployeeEdit
	{
		public EmployeeIT(Employee current_user, int member_id) : base(current_user, member_id)
		{
			can_access = tab_enabled[1];
		}

		public override object Profile()
		{
			var user = new NeMember(member_id);
			var entity = (dto)MapperFrom(new dto(), user);
			var n2user = new Employee(member_id);
			var cellphoneList = GetCellPhoneList(member_id);
			var simList = GetSIMList(member_id);

			return new
			{
				is_backoffice,
				entity,
				cellphoneList,
				simList
			};
		}

		public string syncLdapPassword(string password)
		{
			return n2_member.SyncLdap(password);
		}

		public string ResetPassword(string pass, DateTime curDateTime)
		{
            //expire the password
            var result = n2_member.SetPassword(pass, curDateTime.AddDays(-181));
            return result.IsSuccess ?
                "Password changed successfully" :
                result.JoinErrors();
        }

		public DataExtra Save(dto model)
		{

			if (model.gets_phone && (model.cellphone_id == 0 || model.cellphone_number_id == 0))
			{
				throw new Exception("This user must have a cell phone set to them, and a SIM card.");
			}
			if (model.gets_neemail && string.IsNullOrEmpty(model.NEEmail))
			{
				throw new Exception("If the person is supposed to have email, they need email address.");
			}
			if (model.gets_phoneext && string.IsNullOrEmpty(model.PhoneExtension))
			{
				throw new Exception("If the person is supposed to have extension, they need extension.");
			}
			var user = new NeMember(member_id);
			user = (NeMember)MapperFrom(user, model);
			var n2user = new Employee(member_id);

			if (user.cellphone_id != 0)
			{
				var c = new NECellphone(user.cellphone_id) { last_update_member_id = Convert.ToInt32(UserId) };
				if (c.activedate.Year < 2000)
				{
					c.activedate = DateTime.Now;
				}
				c.save();
			}
			if (user.cellphone_number_id != 0)
			{
				var c = new NECellphone_Number(user.cellphone_number_id) { last_update_member_id = Convert.ToInt32(UserId) };
				c.save();
			}

			user.save();
			return MemberSave(user, model, "IT information");
		}


		public LabelValueInt[] GetCellPhoneList(int mid)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT 
	0 value,
	'No Cellphone' label
UNION 
SELECT
	a.id value,
	CONCAT(a.imei,' - ',b.name) label
FROM
	cellphone a,
	cellphone_status b
WHERE
	a.cellphone_status_id = b.id and 
	ifnull((select MAX(member_id) from member where cellphone_id = a.id limit 1),0) = 0
UNION
SELECT
	a.id value,
	CONCAT(imei,' - ',name) label
FROM
	cellphone a,
	cellphone_status b,
	member c
where 
	a.cellphone_status_id = b.id and 
	a.id = c.cellphone_id and 
	c.member_id = @p0", mid);
		}

		public LabelValueInt[] GetSIMList(int mid)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT 
	0 value,
	'No Number' label
UNION 
SELECT
	a.id value,
	concat(a.number,' - ',a.status) label
FROM
	cellphone_number a
WHERE
	a.status='Active' and 
	ifnull((select MAX(member_id) from member where cellphone_number_id = a.id limit 1),0) = 0
UNION
SELECT
	id value,
	concat(a.number,' - ',a.status) label
FROM
	cellphone_number a
where 
	ifnull((select member_id from member where cellphone_number_id = a.id limit 1),0) = 0
union
select a.id value,
Concat(a.number,' - ',a.status) label
from cellphone_number a,member b 
where a.id = b.cellphone_number_id
and b.member_id = @p0",
				mid);
		}
	}
}