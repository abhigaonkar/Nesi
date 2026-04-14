using System;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeEdit : EmployeeBase
	{
		public int member_id { get; set; }
		protected NeMember n1_member { get; set; }
		protected Employee n2_member { get; set; }
		public bool can_access { get; set; }
		public string error_message { get; set; }
		public bool[] tab_enabled { get; set; }
		public bool can_terminate { get; set; }
		public bool vacation_visible { get; set; }
		public bool can_see_it { get; set; }
		public bool can_see_userswitch { get; set; }
		public bool can_edit_user { get; set; }
		public bool can_review { get; set; }
		public bool can_view_all_reviews { get; set; }
		public bool can_term_all { get; set; }
		public bool can_view_employment_agreement_tab { get; set; }
		public bool is_backoffice { get; set; }
		public bool isowner { get; set; }

		public EmployeeEdit(Employee user) : base(user)
		{

		}
		public EmployeeEdit(Employee current_user, int mid) : base(current_user)
		{
			// var currentuser = new NeMember(current_user.Id);

			member_id = mid;
			can_access = true;
			error_message = "";
			vacation_visible = current_user.AuthenticatedForPrivilege(165);
			can_see_it = current_user.AuthenticatedForPrivilege(126);
			can_see_userswitch = current_user.AuthenticatedForPrivilege(170);
			can_term_all = current_user.AuthenticatedForPrivilege(156);
			can_review = current_user.AuthenticatedForPrivilege(128);
			can_view_all_reviews = current_user.AuthenticatedForPrivilege(144);
			can_view_employment_agreement_tab = current_user.AuthenticatedForPrivilege(197);

			if (member_id > 0)
			{
				n1_member = new NeMember(member_id);
				n2_member = new Employee(member_id);
				is_backoffice = n1_member.is_backoffice;
				issupervisor = NeMember.is_supervisor(member_id, current_user.Id);
				isowner = NeMember.is_owner(current_user.Id, n2_member.TaxEntityId);
				if (n2_member.BusinessUnitId != current_user.BusinessUnitId && !isowner && n2_member.BusinessUnitId != 0 && !can_edit_other_branches && !issupervisor)
				{
					can_access = false;
					error_message = "You can only access users from your branch";
				}
				if (!isowner && !is_branch_hr && !is_depart_hr && !issupervisor && !can_edit_other_branches || n1_member.isContact)
				{
					can_access = false;
					error_message = "You are not authorised to see this page";
				}
				can_terminate = n2_member.Status == "Active" && (n1_member.reports_to == current_user.Id || can_term_all);
				can_edit_user = issupervisor || ispayroll || can_edit_info;
			}

			tab_enabled = new bool[11];
			//User information
			tab_enabled[0] = true;
			//IT
			tab_enabled[1] = can_see_it;
			// wage
			tab_enabled[2] = member_id > 0 && (can_see_wage && (issupervisor ||
											  n1_member.reports_to == 0 && isowner ||
											  can_edit_other_branches && (n1_member.is_backoffice ||
																		  is_branch_hr || ispayroll)));

			// day off
			tab_enabled[3] = isowner || issupervisor || ispayroll;
			// disc
			tab_enabled[4] = isowner || issupervisor || ispayroll;
			// priv
			tab_enabled[5] = can_see_priv || can_see_userswitch;
			// files
			tab_enabled[6] = isowner || issupervisor || ispayroll;
			// term
			tab_enabled[7] = member_id > 0 && (n1_member.Status == "Not Active" && (issupervisor || can_term_all || current_user.AuthorizePage(44) && n1_member.is_backoffice) // from n1 code line 1204
							 && (isowner || issupervisor || ispayroll || // from N1 code line 248
								 can_term_all ||
								 current_user.AuthorizePage(44) &&
								 n1_member.is_backoffice ||
								 CurrentUser.BusinessUnitId == n2_member.BusinessUnitId && current_user.MemberType.membertype_id == 5));

			//employment aggreemnt
			tab_enabled[8] = member_id > 0 && (issupervisor ||
							 n1_member.reports_to == 0 && isowner ||
							 can_edit_other_branches || can_view_employment_agreement_tab);
			//review
			tab_enabled[9] = isowner || issupervisor || ispayroll || can_view_all_reviews;
			//footprints
			tab_enabled[10] = isowner || issupervisor || can_edit_info || tab_enabled[7];


		}

		protected DataExtra MemberSave(NeMember user, object model, string title = "Information")
		{
			var msg = $"{title} has been saved successfully.";
			try
			{
				user.save();
			}
			catch (Exception e)
			{
				msg = e.Message;
			}
			return new DataExtra()
			{
				Data = msg,
				Extra = model
			};
		}
	}
}