using System.Collections.Generic;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeBase : BLLBase
	{

		public bool can_see_wage { get; set; }
		public bool ispayroll { get; set; }
		public bool is_branch_hr { get; set; }
		public bool is_depart_hr { get; set; }
		public bool issupervisor { get; set; }
		public bool can_edit_info { get; set; }
		public bool can_see_priv { get; set; }
		public bool can_edit_other_branches { get; set; }
		public bool is_branch_daysoff { get; set; }
		public bool show_action_column { get; set; }

		public EmployeeBase(Employee current_user) : base(current_user)
		{
			show_action_column= current_user.AuthenticatedForPrivilege(5);
			can_edit_info = current_user.AuthenticatedForPrivilege(32);
			is_branch_hr = current_user.AuthenticatedForPrivilege(33);
			is_depart_hr = current_user.AuthenticatedForPrivilege(35);
			can_see_priv = current_user.AuthenticatedForPrivilege(36);
			can_edit_other_branches = current_user.AuthenticatedForPrivilege(6);
			is_branch_daysoff = current_user.AuthenticatedForPrivilege(166);
			can_see_wage = current_user.AuthenticatedForPrivilege(101);  

			ispayroll = current_user.AuthenticatedForPrivilege(37)
				|| current_user.EmployeeProfile.is_CAN_boardmember.GetValueOrDefault() == 1
				|| current_user.EmployeeProfile.is_US_boardmember.GetValueOrDefault() == 1;

		}


		public virtual object Profile()
		{
			return this;
		}


	}
}