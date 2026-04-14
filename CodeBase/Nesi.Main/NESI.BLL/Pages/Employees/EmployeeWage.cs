using System;
using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using dto = NESI.DTO.ViewModels.Page.Employees.EmployeeWageVacation;
using dtoWage = NESI.DTO.ViewModels.Page.Employees.EmployeeWage;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeWage : EmployeeEdit
	{
		public EmployeeWage(Employee current_user, int mid) : base(current_user, mid)
		{
			can_access = tab_enabled[2];
		}

		public override object Profile()
		{
			var user = new NeMember(member_id);
			var entity = (dto)MapperFrom(new dto(), user);
			entity.is_CDN_country = user.business_unit.country.Equals("CDN");
			entity.is_receive_stat_pay = user.receive_stat_pay == 0;
			if (entity.is_CDN_country)
			{
				entity.vacation_amount_1 = entity.vacation_amount_1 * 100;
				entity.vacation_amount_2 = entity.vacation_amount_2 * 100;
				entity.vacation_amount_3 = entity.vacation_amount_3 * 100;
			}
			return new
			{
				is_backoffice,
				can_edit = is_backoffice && CurrentUser.AuthenticatedForPrivilege(37),
				can_edit_wage = CurrentUser.AuthenticatedForPrivilege(101) && user.hrstatus_id < 4,
				bonusTypeList=bllToolbox.doSQL_Array<LabelValueInt>(@"
Select 0 value,'Not  Set' label
union 
SELECT bonus_type.id value, bonus_type.bonus_type label
FROM bonus_type 
INNER JOIN 
bonus_membertype_link 
ON bonus_membertype_link.bonus_type_id = bonus_type.id 
and bonus_membertype_link.membertype_id =@v0", member_id),

				entity,
				wageList = GetWageGrid(),
			};
		}


		public DataExtra SaveVacation(dto model)
		{
			if (model.is_CDN_country)
			{
				model.vacation_amount_1 = model.vacation_amount_1 / 100;
				model.vacation_amount_2 = model.vacation_amount_2 / 100;
				model.vacation_amount_3 = model.vacation_amount_3 / 100;
			}
			var user = new NeMember(member_id);
			user = (NeMember)MapperFrom(user, model);

			if (user.paytype_id > 3 && user.paytype_id != 7)
			{
				user.receive_stat_pay = 0;
				user.timetostat = 9999;
			}
			else
			{
				user.receive_stat_pay = model.is_receive_stat_pay ? 0 : 1;
			}

			return MemberSave(user, model, "Vacation");
		}

		public dtoWage[] GetWageGrid()
		{
			return bllToolbox.doSQL_Array<dtoWage>(@" 
SELECT mw.memberwage_id id, 
mw.memberwage_memberid member_id,
mw.date, 
mw.currentwage current_wage, 
mw.nextraise date_next_raise, 
IFNULL(mw.comment,'') comment, 
mw.member_id_audit,
mw.memberwage_added_by member_id_added_by,
m.member_fullname audit_username, 
mw.bonus_type_id bonus_type, 
mw.bonus_amount, 
mw.business_unit_id,
mw.active,
ifnull(mw.membertype_id,0) membertype_id,
bt.bonus_type bonus_type_name 
FROM memberwage mw 
INNER JOIN member m 
ON mw.member_id_audit = m.member_id 
left JOIN bonus_type bt 
ON mw.bonus_type_id = bt.id 
WHERE memberwage_memberid = @v0  
ORDER BY mw.date desc,mw.memberwage_id DESC", member_id);
		}

		public DataExtra SaveWage(dtoWage model)
		{
			var employee = new NeMember(member_id);
			var employee_reportsto = employee.reports_to > 0 ? new NeMember(employee.reports_to) : new NeMember();
			var isNew = model.id == 0;
			var oldwage =  isNew ? new NeWage() : new NeWage(model.member_id);
			var w = isNew ? new NeWage() : new NeWage(model.member_id);
			w = (NeWage)MapperFrom(w, model);
			w.date = model.date.ToString("yyyy-MM-dd");
			w.date_next_raise = model.date_next_raise.GetValueOrDefault(DateTime.Now.AddYears(1)).ToString("yyyy-MM-dd");
			// If its a new wage
			if (isNew)
			{
				w.member_id_audit = CurrentUser.Id;
				w.member_id_added_by = CurrentUser.Id;
			}

			w.save();
			const string subjectInsert = "Employee Wage Inserted";
			const string subjectUpdate = "Employee Wage Updated";
		    
			var body = $@"This is an automated message to let you know the following: <br/>
					{CurrentUser.FullName} has {(model.id == 0 ? "Inserted" : "Updated")} {employee.FullName}'s wage. <br/>
					<ul>
					<li>Date Added: {Toolbox.MySQLNow_short()}</li>
					<li>Effective Date: {w.date}</li>
					<li>Old Wage: {(model.id > 0 ? oldwage.current_wage: 0)}</li>
					<li>New Wage: {w.current_wage}</li>
					<li>Business Unit: {employee.business_unit_name}</li>
					<li>Manager: {(employee.reports_to > 0? employee_reportsto.FullName : "NA")}</li>
					<li>Notes: {w.comment}</li>
					</ul>";
			// If wage didn't exist before
			if(isNew) 
			{
			shared.alert_payroll(subjectInsert,body);
			shared.alert_hr(subjectInsert,body);
			}
			else
			{
			shared.alert_payroll(subjectUpdate,body);
			shared.alert_hr(subjectUpdate,body);
			}
			
			return new DataExtra()
			{
				Data = "Wage has been saved successfully",
				Extra = Profile()
			};
		}
	}


}