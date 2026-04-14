using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Employees;
using NESI.Common.Models;

namespace NESI.BLL.Pages.Employees
{
	public class emp_trm
	{
		public int id { get; set; }
		public int mgr_member_id { get; set; }
		public int trm_member_id { get; set; }
		public DateTime dt_added { get; set; }
		public DateTime dt_modified { get; set; }
		public DateTime term_date { get; set; }
		public string term_time { get; set; }
		public string reason_actual { get; set; }
		public string reason_roe { get; set; }
		public bool is_returning { get; set; }
		public DateTime return_date { get; set; }
		public bool is_vac_bank { get; set; }
		public string vac_bank_detail { get; set; }
		public bool is_returned_property { get; set; }
		public double non_returned_value { get; set; }
		public DateTime last_day_worked { get; set; }

		public emp_trm() { }
		public emp_trm(int _id)
		{
			load(_id);
		}
		public void load_most_recent(int _member_id, int _mgr_id)
		{
			var dt = Toolbox.doSQL_dt(@"SELECT id FROM emp_trm WHERE trm_member_id = @v0  ORDER BY dt_added DESC", new object[] { _member_id });
			if (dt.Rows.Count == 0)
			{
				// One hasn't been started, so start one.
				start_trm(_member_id, _mgr_id);
				if (id != 0)
				{
					// Safe Recursive
					load_most_recent(_member_id, _mgr_id);
				}
			}
			else
			{
				var _id = Convert.ToInt32(dt.Rows[0]["id"]);
				load(_id);
			}
		}
		private void start_trm(int _member_id, int _mgr_id)
		{
			trm_member_id = _member_id;
			mgr_member_id = _mgr_id;
			id = Toolbox.doSQL_return_id(@"INSERT INTO emp_trm 
(mgr_member_id, trm_member_id, is_returned_property) VALUES (@v0, @v1, 1)", new object[] { _mgr_id, _member_id });
		}
		private bool exists(int _id)
		{
			var e = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM emp_trm WHERE id = @v0", _id);
			return e > 0;
		}
		private void load(int _id)
		{
			if (exists(_id))
			{
				var dr = Toolbox.doSQL_dt(@" SELECT id, mgr_member_id, trm_member_id, dt_added, dt_modified, term_date, term_time, reason_actual, reason_roe, is_returning, return_date, is_vac_bank, vac_bank_detail, is_returned_property, non_returned_value, last_day_worked FROM emp_trm WHERE id = @v0  LIMIT 1", new object[] { _id }).Rows[0];
				id = _id;
				mgr_member_id = (int)dr["mgr_member_id"];
				trm_member_id = (int)dr["trm_member_id"];
				dt_added = dr["dt_added"] == DBNull.Value ? DateTime.Now : (DateTime)dr["dt_added"];
				dt_modified = dr["dt_modified"] == DBNull.Value ? DateTime.Now : (DateTime)dr["dt_modified"];
				term_date = dr["term_date"] == DBNull.Value ? DateTime.Now : (DateTime)dr["term_date"];
				term_time = dr["term_time"] == DBNull.Value ? DateTime.Now.ToString("HH:mm") : (string)dr["term_time"];
				reason_actual = dr["reason_actual"] == DBNull.Value ? "" : (string)dr["reason_actual"];
				reason_roe = dr["reason_roe"] == DBNull.Value ? "" : (string)dr["reason_roe"];
				is_returning = dr["is_returning"] != DBNull.Value && (bool)dr["is_returning"];
				return_date = dr["return_date"] == DBNull.Value ? DateTime.Now : (DateTime)dr["return_date"];
				is_vac_bank = dr["is_vac_bank"] != DBNull.Value && (bool)dr["is_vac_bank"];
				vac_bank_detail = dr["vac_bank_detail"] == DBNull.Value ? "" : (string)dr["vac_bank_detail"];
				is_returned_property = dr["is_returned_property"] != DBNull.Value && (bool)dr["is_returned_property"];
				non_returned_value = (double)dr["non_returned_value"];
				last_day_worked = dr["last_day_worked"] == DBNull.Value ? term_date : (DateTime)dr["last_day_worked"];
			}
			else
			{
				throw new Exception("Entry doesn't exist.");
			}
		}
		public void save()
		{
			if (id != 0)
			{
				// Update
				Toolbox.doSQL_void(@"
UPDATE
	emp_trm
SET
	term_date				= @v0,
	term_time				= @v1,
	reason_actual			= @v2,
	reason_roe				= @v3,
	is_returning			= @v4,
	return_date				= @v5,
	is_vac_bank				= @v6,
	vac_bank_detail			= @v7,
	is_returned_property	= @v8,
	non_returned_value		= @v9,
last_day_worked = @v11
WHERE
	id = @v10
LIMIT 1
", new object[] {
  Toolbox.MySQL_shortdt(term_date),		// {0}
  term_time,							// {1}
  reason_actual,	// {2}
  reason_roe,		// {3}
  is_returning,							// {4}
  Toolbox.MySQL_shortdt(return_date),	// {5}
  is_vac_bank,							// {6}
  vac_bank_detail,	// {7}
  is_returned_property,					// {8}
  non_returned_value,					// {9}
  id,									// {10}
  last_day_worked // {11}
  });
			}
		}
	}


	public class EmployeeTermination : EmployeeEdit
	{
		protected NeMember current_user;
		protected emp_trm et = new emp_trm();
		protected readonly Toolbox _tools;

		public EmployeeTermination(Employee loginuser, int mid) : base(loginuser, mid)
		{
			can_access = tab_enabled[7];
			current_user = new NeMember(UserId);
			et.load_most_recent(member_id, UserId);
#pragma warning disable 618
			_tools = new Toolbox();
#pragma warning restore 618


		}

		public DataExtra StartTerminate(EmployeeStartTerminate model)
		{
			if (!(can_term_all || NeMember.is_supervisor(member_id, UserId)))
			{
				return new DataExtra("You're not supposed to be here.", null);
			}
			send_emails(true);
			deactivate_NESI_account();
			clear_auto_reports();
			clear_term_chklist();
			create_term_chklist();
			/*if (n1_member.LDAP_user != "")
			{
				deactivate_AD_account();
			}*/

			et.reason_roe = model.roe;
			et.reason_actual = model.act;
			et.last_day_worked = model.ldw;
			et.save();
			return et.id > 0 ? new DataExtra($"Successfullly terminated {n1_member.FullName}", new { et.id }) : new DataExtra("failed.", null);
		}

		private LabelValueString[] GetRoeList()
		{
			return new[]
			{
				new LabelValueString("Shortage of work (layoff)"),
				new LabelValueString("Return to school"),
				new LabelValueString("Resigned or Quit"),
				new LabelValueString("Retirement"),
				new LabelValueString("Maternity"),
				new LabelValueString("Work Sharing"),
				new LabelValueString("Dismissal"),
				new LabelValueString("Apprentice Training"),
				new LabelValueString("Leave of Absence"),
				new LabelValueString("Parental"),
				new LabelValueString("Compassionate Care"),
			};
		}

		public override object Profile()
		{
			var t = new EmployeeTerminationProfile();
			t = (EmployeeTerminationProfile)MapperFrom(t, et);
			var roeList = GetRoeList();
			if (et?.reason_roe != null && roeList.FirstOrDefault(x => x.Value == et.reason_roe) == null)
			{
				var list = roeList.ToList();
				list.Add(new LabelValueString(et.reason_roe));
				roeList = list.ToArray();
			}
			return new
			{
				can_terminate,
				status = n1_member.Status == "Active" ? "Active" : "Inactive",
				//ad_status = n1_member.LDAP_user == "" ? "N/A" : AD_status() == "Active" ? "Active" : "Inactive",
				hr_status = bllToolbox.doSQL_string(@"Select status from member_hrstatus  where id =@v0", n1_member.hrstatus_id),
				fullname = n1_member.FullName,
				business_unit_name = n1_member.business_unit.name,
				membertype_name = n1_member.membertype.name,
				checklist = GetCheckList(),
				roeList,
				entity = t,
			};
		}

		public EmployeeTerminationCheckListItem[] GetCheckListData(string type)
		{

			var sql = @"
SELECT 
			ifnull(a.id,0) id,
			b.field field,
			ifnull(a.is_checked,0)=1 is_checked,
			a.response response,
			ifnull(a.chk_by,0) chkby,
			m.member_fullname chkby_name,
			ifnull(a.chk_by,0)>0 is_completed,
			ifnull(b.response_required,0)=1 resp_req,
			a.comment comment,
			b.type type,
			if(ifnull(a.is_checked,0)=1,fun_time(a.dt_modified),'') dt
		FROM 
			emp_trm_chklist_lnk a
		LEFT JOIN
			emp_trm_chklist_opt b 
				ON a.opt_id = b.id
		left join member m on a.chk_by = m.member_id
		WHERE 
			a.trm_id =@p0";
			if (!string.IsNullOrEmpty(type) && type != "All")
			{
				sql += " and b.type=@p1";
			}

			return bllToolbox.doSQL_Array<EmployeeTerminationCheckListItem>(sql, et.id, type);
		}


		public object GetCheckList(string type = "")
		{
			var dt = bllToolbox.doSQL_dt(
				@"SELECT ifnull((Select count(a.id) 
from emp_trm_chklist_lnk a INNER JOIN emp_trm_chklist_opt c on c.id = a.opt_id 
where c.type = b.type and a.is_checked = 0 and a.trm_id = @v0  GROUP BY c.type) ,0)id,
b.type _type 
FROM emp_trm_chklist_opt 
b GROUP BY b.type",
				et.id);
			var dirs = new Dictionary<string, int>();
			foreach (DataRow row in dt.Rows)
			{
				dirs.Add(row["_type"].ToString(), Convert.ToInt32(row["id"]));
			}
			var checklistTypeList = new[]
		 {
				new LabelValueString()
				{
					Label = $"Showing Direct Supervisor Checklist ({dirs["Direct"]})",
					Value = "Direct"
				},
				new LabelValueString()
				{
					Label = $"Showing Payroll Admin Checklist ({dirs["Payroll"]})",
					Value = "Payroll"
				},
				new LabelValueString()
				{
					Label = $"Showing System Administrator Checklist ({dirs["SysAdmin"]})",
					Value = "SysAdmin"
				}
			};

			var checklist_type = "";
			var checklist_enabled = false;
			var checklist_enabled_Direct = false;
			var checklist_enabled_Payroll = false;
			var checklist_enabled_SysAdmin = false;
			var is_super = NeMember.is_supervisor(n1_member.id, current_user.id);
			if (n1_member.hrstatus_id == 5)
			{
				checklist_type = "All";

			}
			if ((current_user.id == n1_member.reports_to) || (is_super))
			{
				checklist_type = "Direct";
				checklist_enabled_Direct = true;

			}
			if (current_user.AuthenticatedForPrivilege(37) && (current_user.business_unit_id == 11))
			{
				checklist_type = "Payroll";
				checklist_enabled_Payroll = true;
			}
			if (CurrentUser.isDeveloper)
			{
				checklist_type = "SysAdmin";
				checklist_enabled_SysAdmin = true;
			}

			if (type == "All" && checklist_type == "All")
			{
				checklistTypeList = new[]
				{
					new LabelValueString()
					{
						Label = "Showing Complete CheckList",
						Value = "All"
					}
				};
			}
			else if (type == "All" && checklist_type != "All")
			{
				type = checklist_type;
			}

			switch (type)
			{
				case "Direct":
					checklist_enabled = checklist_enabled_Direct;
					break;
				case "Payroll":
					checklist_enabled = checklist_enabled_Payroll;
					break;
				case "SysAdmin":
					checklist_enabled = checklist_enabled_SysAdmin;
					break;
			}

			return new
			{
				checklistTypeList,
				checklist_enabled,
				checklist_type = type,
				data = GetCheckListData(type)
			};
		}

		private void send_emails(bool do_force)
		{
			if (n1_member.Status == "Active" || do_force)
			{
				deactivate_NESI_account();
			    
				var actingManager = n1_member.business_unit.acting_right_hand > 0 ? new NeMember(n1_member.business_unit.acting_right_hand) : new NeMember();
				var terminatedUserisBM = n1_member.business_unit.branch_manager.id32 == n1_member.id32;
				var terminatedUserisAM = actingManager.id32 == n1_member.id32;
				var te = new NeTaxEntity(n1_member.business_unit.tax_entity_id);
				//Body of email
				
				var body = $@"<div style='font-size:11px;font-family:arial;'>
<div><b>Employee Name: </b><u>{n1_member.FullName}</u></div>
<div><b>Business Unit: </b><u>{n1_member.business_unit_name}</u></div>
<div><b>Tax Entity: </b><u>{te.public_name}</u></div>
<div><b>Position: </b><u>{n1_member.membertype.name}</u></div>
<div><b>Termination Date: </b><u>{Toolbox.MySQL_shortdt(et.term_date)}</u></div>
<div><b>Termination Time: </b><u>{et.term_time}</u></div>
<div><b>Last Date Worked: </b><u>{et.last_day_worked:yyyy-MM-dd}</u></div>
<div><b>Termination Reason (Actual): </b><u>{et.reason_actual}</u></div>
<div><b>Termination Reason (ROE): </b><u>{et.reason_roe}</u></div>
<div><b>Will the employee be returning?: </b><u>{Toolbox.Bool_to_UserFriendlyResponse(et.is_returning)}</u></div>
<div><b>If the employee is returning please provide return date: </b><u>{Toolbox.MySQL_shortdt(et.return_date)}</u></div>
<div><b>If the employee is returning, are they requesting their vacation pay or banked hours?: </b><u>{Toolbox.Bool_to_UserFriendlyResponse(et.is_vac_bank)}</u></div>
<div><b>If yes, please provide details: </b><u>{et.vac_bank_detail}</u></div>
<div><b>Has employee returned all company property?: </b><u>{Toolbox.Bool_to_UserFriendlyResponse(et.is_returned_property)}</u></div>
<div><b>Please estimate value of any company property not returned: </b><u>{et.non_returned_value:C2}</u></div>
";

				//Initializing vars -- keeping help notice separated form hr/payroll as it might end up creating new tickets if any one in hr/payroll respond to the chain.
				//Per SPID-25743, help desk notification has been removed,as it is no longer required
				var hr_payroll_notice = new NeEMail
				{
					To = EmailID.Payroll + Toolbox.app_setting("DomainForEmail") + ";" + EmailID.HR + Toolbox.app_setting("DomainForEmail"),
					From = EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),
					Subject = n1_member.FullName + " has been terminated by " + current_user.FullName,
					isHTML =true,
					Body = body
				};
			    //cc - CC-ing Manager only on hr/payroll notification, for the same reason that one response will create a new ticket in Jira
				hr_payroll_notice.CC = terminatedUserisBM 
										? "" 
										: n1_member.business_unit.branch_manager.NEEmail;
				if (actingManager.id > 0 && !terminatedUserisAM)
				{
					hr_payroll_notice.CC += hr_payroll_notice.CC == ""
										? actingManager.NEEmail
										: ";" + actingManager.NEEmail;
				}
				// Sending emails out
			
				hr_payroll_notice.Send();
			}
		}
		private void deactivate_NESI_account()
		{
			//		shared.alert_payroll(n1_member.FullName + " has been terminated", "Their sessions and accounts in both nesi.ca and active directory have been disabled");
			//		shared.alert_it(n1_member.FullName + " has been terminated", "Their sessions and accounts in both nesi.ca and active directory have been disabled");
			ne_session.clear_active_sessions(n1_member.id);
			if (n1_member.hrstatus_id != 4 && n1_member.hrstatus_id != 5)
			{
				n1_member.TerminateDate = Toolbox.MySQL_shortdt(DateTime.Now);
			}
			n1_member.hrstatus_id = 4;
			n1_member.Status = "Not Active";
			n1_member.save();
			var payperiod_id = Toolbox.doSQL_int("CALL _payperiod()");
			if (Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE payroll_handler = @v0", new object[] { n1_member.id }) > 0 && n1_member.reports_to > 0)
			{
				Toolbox.doSQL_void(@"UPDATE member SET payroll_handler = @v1 WHERE payroll_handler = @v0", new object[] { n1_member.id, n1_member.reports_to });
			}
			Toolbox.doSQL_void(@"UPDATE member_offers SET status = 'Closed' WHERE memberid = @v0 AND startdate >= CURDATE()", new object[] { n1_member.id });
			Toolbox.doSQL_void(@"UPDATE vacation_master SET status = 4 WHERE member_id = @v0 AND payperiod_id > @v1", new object[] { n1_member.id, payperiod_id });
		}
		/*private string AD_status()
		{
			var directoryEntry = new DirectoryEntry(Toolbox.app_setting("ldap_path"), Toolbox.app_setting("ldap_user"), Toolbox.app_setting("ldap_pass"), AuthenticationTypes.Secure);
			var directorySearcher =
				new DirectorySearcher(directoryEntry) { Filter = $"(&(SAMAccountName={n1_member.LDAP_user}))" };
			var user = directorySearcher.FindOne();
			if (user != null)
			{
				var u = user.GetDirectoryEntry();
				var status = Convert.ToInt32(u.Properties["userAccountControl"].Value);
				return status == 512 ? "Active" : status == 514 ? "Inactive" : status.ToString();
			}
			else
			{
				return "N/A";
			}
		}
		private void deactivate_AD_account()
		{
			if (AD_status() == "Active")
			{
				var directoryEntry = new DirectoryEntry(Toolbox.app_setting("ldap_path"), Toolbox.app_setting("ldap_user"), Toolbox.app_setting("ldap_pass"), AuthenticationTypes.Secure);
				var directorySearcher =
					new DirectorySearcher(directoryEntry) { Filter = $"(&(SAMAccountName={n1_member.LDAP_user}))" };
				var user = directorySearcher.FindOne();
				if (user != null)
				{
					var u = user.GetDirectoryEntry();
					u.Properties["userAccountControl"].Value = 0x0002;
					u.CommitChanges();
				}
			}
		}*/
		protected void create_term_chklist()
		{
			if (n1_member.hrstatus_id != 4) return;
			var dt = _tools.getSQL_datatable(@"Select * from emp_trm_chklist_opt", null);
			foreach (DataRow dr in dt.Rows)
			{
				if (_tools.getSQL_int(@"Select count(id) from emp_trm_chklist_lnk  where trm_id =@v0 and opt_id =@v1 ", new object[] { et.id, dr["id"] }) == 0)
				{
					bllToolbox.doSQL_void(@"Insert into emp_trm_chklist_lnk (trm_id,opt_id,dt_added)  values (@v0,@v1,now())", et.id, dr["id"]);
				}
			}
		}
		protected void clear_term_chklist()
		{
			bllToolbox.doSQL_void(@"Delete from emp_trm_chklist_lnk where trm_id = @v0", et.id);

		}

		protected void clear_auto_reports()
		{
			bllToolbox.doSQL_void(@"Delete from auto_reports_schedule where member_id = @v0", et.trm_member_id);
		}

		public DataExtra SaveProfile(EmployeeTerminationProfile model)
		{
			et = (emp_trm)MapperFrom(et, model);
			et.save();
			var priv = new NEMemberTypePrivilege();
			priv.ClearPrivileges(member_id);
			try
			{
				bllToolbox.doSQL_void(@"Delete from emp_review_history where member_id =@v0  and emp_review_history.date > curdate()", member_id);
				bllToolbox.doSQL_void(@"update member_offers set status = 'Closed' where memberid = @v0 and member_offers.startdate > curdate()", member_id);
				bllToolbox.doSQL_void(@"Delete from ticket_memberview where member_id = @v0", member_id);
				bllToolbox.doSQL_void(@"Update vacation_master set status = 4 where vacation_master.member_id = @v0 and date_start > curdate()", member_id);
				bllToolbox.doSQL_void(@"delete from training_history where member_id = @v0 and date > curdate()", member_id);
				bllToolbox.doSQL_void(@"update passport set active=1 where `from` = @v0 and Active = 1", n1_member.NEEmail);
				bllToolbox.doSQL_void(@"delete from auto_reports_schedule where member_id = @v0", member_id);
				bllToolbox.doSQL_void(@"delete from shopping_cart where member_id = @v0", member_id);
				bllToolbox.doSQL_void(@"delete from shopping_cart_header where member_id = @v0", member_id);
				bllToolbox.doSQL_void(@"Delete from emp_review where member_id = @v0 and emp_review.date > curdate()", member_id);
				bllToolbox.doSQL_void(@"update emp_review set status = 'Closed' where member_id = @v0 and status != 'Delivered'", member_id);
				bllToolbox.doSQL_void(@"update member_todo_list_helper set member_id = @v1 where member_id = @v0", member_id, n1_member.reports_to);
				bllToolbox.doSQL_void(@"Delete from oncall_schedule where member_id =@v0", member_id);
			}
			catch (Exception)
			{
				// ignored

			}
			send_emails(true);


			return new DataExtra("Information has been saved successfully.", Profile());
		}

		public DataExtra SaveCheckListItem(EmployeeTerminationCheckListItem model, string type)
		{
			var sql = @"Update emp_trm_chklist_lnk 
set is_checked=@p0,chk_by=@p1,comment=@p2,dt_modified=now() where id=@p3";

			bllToolbox.doSQL_void(sql, model.is_completed, model.is_completed ? UserId : 0, model.comment, model.id);

			if (model.resp_req)
			{
				bllToolbox.doSQL_void(@"Update emp_trm_chklist_lnk set response=@p0 where id=@p1", model.response, model.id);
				if ((model.type != "SysAdmin") && (model.response != ""))
				{
					var email = new NeEMail
					{
						To = "it@" + Toolbox.app_setting("DomainForEmail"),
						Subject = n1_member.FullName + " Checklist - updated",
						Body = n1_member.FullName + " Checklist" + System.Environment.NewLine
					};
					email.Body += current_user.FullName + " just set this in the termination checklist" + System.Environment.NewLine;
					email.Body += "Notes:" + model.comment;
					email.From = "it@" + Toolbox.app_setting("DomainForEmail");
					email.Send();
				}
			}

			return new DataExtra("Checklist has been updated successfully.", GetCheckListData(type));
		}

		public DataExtra SaveCheckList(EmployeeTerminationCheckListItem[] model, string type)
		{
			foreach (var item in model)
			{
				var is_already_selected = bllToolbox.doSQL_int(@"Select is_checked from emp_trm_chklist_lnk  where id =@v0 limit 1 ", item.id);
				if (item.is_checked != Convert.ToBoolean(is_already_selected))
				{
					bllToolbox.doSQL_void(@"update emp_trm_chklist_lnk 
set is_checked = @v0,chk_by=@v1 where id = @v2 limit 1", (item.is_checked ? 1 : 0), (item.is_checked ? UserId : 0), item.id);
				}
			}
			return new DataExtra("Checklists have been saved successfullly.", GetCheckList(type));
		}

		public object StartTerminateProfile()
		{
			return new
			{
				roeList = GetRoeList()
			};
		}
	}
}