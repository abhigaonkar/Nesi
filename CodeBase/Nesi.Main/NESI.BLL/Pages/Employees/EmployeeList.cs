using System.Collections.Generic;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeList : EmployeeBase
	{
		public List<int> report_to_list { get; set; }
		public bool edit_button_enabled(int buid,int member_id)
		{
			return ((is_branch_hr || can_edit_info) && CurrentUser.BusinessUnitId == buid) ||
			       can_edit_other_branches || report_to_list.Contains(member_id);
		}

		public EmployeeList(Employee current_user) : base(current_user)
		{
			report_to_list = GetReportsToAllList(CurrentUser.Id);
		}


	}
}