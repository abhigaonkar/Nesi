using System;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeDaysOffVacationOverride : EmployeeEdit
	{
		public EmployeeDaysOffVacationOverride(Employee current_user, int mid) : base(current_user, mid)
		{
		}

		public override object Profile()
		{
			var user = new NeMember(member_id);
			return new
			{
				is_backoffice,
				entity = new DTO.ViewModels.Page.Employees.EmployeeDaysOffVacationOverride()
				{
					member_id = member_id,
					amount = Math.Round(user.Holiday,2),
					memo = "",
					auto_vac_payout = bllToolbox.doSQL_bool(@"SELECT auto_vacation_payout FROM member WHERE member_id = @v0", member_id)
				},
			};
		}

		public DataExtra Save(DTO.ViewModels.Page.Employees.EmployeeDaysOffVacationOverride model)
		{
			var user = new NeMember(model.member_id);
			var this_user = user;
			var admin = new NeMember(UserId);
			// var holiday_delta =Math.Round(model.amount - user.Holiday,2);
			payroll.vacation.add_adjustment(Toolbox.connect(), ref this_user, admin, model.memo.Trim(), model.amount);

			if (model.auto_vac_payout != user.ReceivesAutoVacationPayout)
			{
				bllToolbox.doSQL_void(@"UPDATE member SET auto_vacation_payout = @v0 WHERE member_id =@v1", new object[] { model.auto_vac_payout , model.member_id});
			}
			new NESI.BLL.Pages.Employees.EmployeeDaysOffVacationTransactionGrid(CurrentUser).ClearCache(new object[] {model.member_id});
			return new DataExtra()
			{
				Data = "Override has been saved successfully.",
				Extra = Profile()
			};
		}
	}
}