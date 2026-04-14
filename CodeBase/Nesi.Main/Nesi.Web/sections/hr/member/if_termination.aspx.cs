using DevExpress.Web;
using nesi.core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Web.UI;

public partial class sections_hr_member_if_termination : System.Web.UI.Page
	{
	
	NeMember current_user;
	NeMember loaded_user;
	bool can_term_all;
	string chk_type					= "";
	emp_trm et						= new emp_trm();
	bool is_debug = false;
	string debug_email = "mhyde@" + Toolbox.app_setting("DomainForEmail");
	NameValueCollection _q;
	Toolbox	_tools;
	DataTable employee_list;
	bool _has_open_footprints;
	bool _enabled = false;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		_q									= Request.QueryString;
		current_user						= Toolbox.do_handle_authentication(47);
	
 		if(string.IsNullOrEmpty(_q["id"]))
			{
			Toolbox.FriendlyException(Response, "Member ID Not Defined", "./");
			}
 		if(string.IsNullOrEmpty(_q["type"]) && string.IsNullOrEmpty(_q["a"]))
			{
			Toolbox.FriendlyException(Response, "Type Not Defined", "./");
			}
		var employee_id				= Convert.ToInt32(_q["id"]);
		loaded_user						= new NeMember(employee_id);
		uc_member_usage1.employee_id = loaded_user.id;
		uc_member_usage1.current_user = current_user.id;
		if (!IsPostBack)
		{
			uc_member_usage1.DataBind();
		}
		chk_type						= _q["type"];
		et.load_most_recent(loaded_user.id32, current_user.id32); 
		employee_list			= Toolbox.doSQL_dt(@" SELECT a.member_id id, CONCAT('(', b.name,') ',member_fullname) text FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE a.member_status = 'Active' AND a.member_id != @v1  ORDER BY b.name, a.member_lastname, a.member_firstname", new object[] {  loaded_user.business_unit_id, loaded_user.id } );
		can_term_all = current_user.AuthenticatedForPrivilege(156);
	//	has_subordinates	= NeMember.get_allreports(current_user.id).Rows.Count > 0;
		// The populate routines need to be in the Init method in order to bind correctly.
	
				}
	protected void Page_Load(object sender, EventArgs e)
		{
		if(!string.IsNullOrEmpty(_q["a"]))
			{
			if(can_term_all || NeMember.is_supervisor(loaded_user.id,current_user.id))
				{
				var action			= _q["a"];
				switch(action)
					{
					case "k":
						send_emails(true);
						deactivate_NESI_account();
						clear_auto_reports();
						clear_term_chklist();
						create_term_chklist();						
						/*if(loaded_user.LDAP_user != "")
							{
							deactivate_AD_account();
							}*/
						if (_q["roe"] != null && _q["act"] != null && _q["ldw"] != null)
						{
							et.load_most_recent(loaded_user.id32, current_user.id32);
							et.reason_roe = _q["roe"];
							et.reason_actual = _q["act"];
						    et.last_day_worked = Convert.ToDateTime(_q["ldw"]);
						        							et.save();
						}
						Toolbox.QuickReponse(Response, "SUCCESS");
					break;
					}
				}
			else
				{
				Toolbox.FriendlyException(Response, "You're not supposed to be here", "/default.aspx");
				}
			}
		else
			{

				if (loaded_user.hrstatus_id == 5)
				{
					_enabled = false;
					var bb = (ASPxButton)gv_chklist1.FindFooterCellTemplateControl(gv_chklist1.Columns[0], "btnsave");
					bb.Enabled = false;
				}

			if(!IsPostBack)
				{
			
				create_term_chklist();
			
				
				if (loaded_user.hrstatus_id == 5)
				{
					Session["term_gv_filter"] = "All";
					button_save.ClientEnabled = false;
					_enabled = false;
					
				}
				else if ((current_user.id == loaded_user.reports_to)||(NeMember.is_supervisor(loaded_user.id,current_user.id)))
				{
					Session["term_gv_filter"] = "Direct";
				}
				else if (current_user.AuthenticatedForPrivilege(37) && (current_user.business_unit_id == 11))
				{
					Session["term_gv_filter"] = "Payroll";
				}
				else if (current_user.id == 711 || current_user.id == 8 || current_user.id == 1295)
				{
					Session["term_gv_filter"] = "SysAdmin";
				}
				
				b_nesistatus.InnerHtml			= loaded_user.Status == "Active" ? "<b class='active'>Active</b>" : "<b class='inactive'>Inactive</b>";
				//b_adstatus.InnerHtml			= loaded_user.LDAP_user == "" ? "N/A" : AD_status() == "Active" ? "<b class='active'>Active</b>" : "<b class='inactive'>Inactive</b>";
				b_hrstatus.InnerHtml = _tools.getSQL_string(@"Select status from member_hrstatus  where id =@v0", new object[] { loaded_user.hrstatus_id });
				et.load_most_recent(loaded_user.id32, current_user.id32); 
				lb_employee_name.Text			= loaded_user.FullName;
				lbl_branch.Text = loaded_user.business_unit.name;
				lb_position.Text				= loaded_user.membertype.name;
				date_termination.Date			= et.term_date;
				time_term.DateTime				= Convert.ToDateTime(et.term_time);
				date_termination.Date			= et.term_date;
				date_return.Date				= et.return_date;
				tb_reason_actual.Text			= et.reason_actual;
				    dte_last_date_worked.Date = et.last_day_worked;
				if (ddl_roe.Items.FindByTextWithTrim(et.reason_roe) == null)
				{
					ddl_roe.Items.Add(et.reason_roe, et.reason_roe);
				}
				ddl_roe.Text = et.reason_roe;

				rbl_returning.Items.FindByValue(et.is_returning ? 1 : 0).Selected	= true;
				if(et.is_returning)
					{
					tr_return_date.Visible				= true;
					tr_return_vacbank.Visible			= true;
					}
				rbl_vac_bank.Items.FindByValue(et.is_vac_bank ? 1 : 0).Selected	= true;
				if(et.is_vac_bank)
					{
					tr_return_vacbank_details.Visible	= true;
					memo_vac_bank.Text					= et.vac_bank_detail;
					}
				rbl_property.Items.FindByValue(et.is_returned_property ? 1 : 0).Selected	= true;
				if(!et.is_returned_property)
					{
					tr_property_estimation.Visible		= true;
					}
				}
	//		generate_chklist(1, chk_type);
			validate();
			fill_gv();
			set_tabs();
		}
		if (!gv_chklist1.IsCallback)
		{
			set_selected();
			set_tabs();
		}
	}

	protected void LoadComplete(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			fill_gv();
			set_tabs();
		}
	}

	protected void rbl_returning_SelectedIndexChanged(object sender, EventArgs e)
		{
		var rbl				= (ASPxRadioButtonList) sender;
		et.is_returning						= (int) rbl.Value == 1;
		et.save();
		tr_return_date.Visible				= (int) rbl.Value == 1;
		tr_return_vacbank.Visible			= (int) rbl.Value == 1;
		}
	protected void rbl_vac_bank_SelectedIndexChanged(object sender, EventArgs e)
		{
		var rbl				= (ASPxRadioButtonList) sender;
		et.is_vac_bank						= (int) rbl.Value == 1;
		et.save();
		tr_return_vacbank_details.Visible	= (int) rbl.Value == 1;
		}
	protected void rbl_property_SelectedIndexChanged(object sender, EventArgs e)
		{
		var rbl				= (ASPxRadioButtonList) sender;
		et.is_returned_property				= (int) rbl.Value == 1;
		et.save();
		tr_property_estimation.Visible		= (int) rbl.Value == 0;
		}
	private class emp_trm
		{
		public int id						{get;set;}
		public int mgr_member_id			{get;set;}
		public int trm_member_id			{get;set;}
		public DateTime dt_added			{get;set;}
		public DateTime dt_modified			{get;set;}
		public DateTime term_date			{get;set;}
		public string term_time				{get;set;}
		public string reason_actual			{get;set;}
		public string reason_roe			{get;set;}
		public bool is_returning			{get;set;}
		public DateTime return_date			{get;set;}	
		public bool is_vac_bank				{get;set;}
		public string vac_bank_detail		{get;set;}
		public bool is_returned_property	{get;set;}
		public double non_returned_value	{get;set;}
		    public DateTime last_day_worked { get; set; }
		    
		public emp_trm(){}
		public emp_trm(int _id)
			{
			load(_id);
			}
		public void load_most_recent(int _member_id, int _mgr_id)
			{
			var dt					= Toolbox.doSQL_dt(@"SELECT id FROM emp_trm WHERE trm_member_id = @v0  ORDER BY dt_added DESC", new object[] {  _member_id } );
			if(dt.Rows.Count == 0)
				{
				// One hasn't been started, so start one.
				start_trm(_member_id, _mgr_id);
				if(id != 0)
					{
					// Safe Recursive
					load_most_recent(_member_id, _mgr_id);
					}
				}
			else
				{
				var _id				= Convert.ToInt32(dt.Rows[0]["id"]);
				load(_id);
				}
			}
		private void start_trm(int _member_id, int _mgr_id)
			{
			trm_member_id				= _member_id;
			mgr_member_id				= _mgr_id;
			id							= Toolbox.doSQL_return_id(@"INSERT INTO emp_trm 
(mgr_member_id, trm_member_id, is_returned_property) VALUES (@v0, @v1, 1)", new object[] { _mgr_id, _member_id});
			}
		private bool exists(int _id)
			{
			var e						= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM emp_trm WHERE id = @v0", _id);
			return e > 0;
			}
		private void load(int _id)
			{
			if(exists(_id))
				{
				var dr					= Toolbox.doSQL_dt(@" SELECT id, mgr_member_id, trm_member_id, dt_added, dt_modified, term_date, term_time, reason_actual, reason_roe, is_returning, return_date, is_vac_bank, vac_bank_detail, is_returned_property, non_returned_value, last_day_worked FROM emp_trm WHERE id = @v0  LIMIT 1", new object[] {  _id } ).Rows[0];
				id							= _id;
				mgr_member_id				= (int) dr["mgr_member_id"];
				trm_member_id				= (int) dr["trm_member_id"];
				dt_added					= dr["dt_added"] == DBNull.Value ? DateTime.Now : (DateTime) dr["dt_added"];
				dt_modified					= dr["dt_modified"] == DBNull.Value ? DateTime.Now : (DateTime) dr["dt_modified"];
				term_date					= dr["term_date"] == DBNull.Value ? DateTime.Now : (DateTime) dr["term_date"];
				term_time					= dr["term_time"] == DBNull.Value ? DateTime.Now.ToString("HH:mm") : (string) dr["term_time"];
				reason_actual				= dr["reason_actual"] == DBNull.Value ? "" : (string) dr["reason_actual"];
				reason_roe					= dr["reason_roe"] == DBNull.Value ? "" : (string) dr["reason_roe"];
				is_returning				= dr["is_returning"] != DBNull.Value && (bool) dr["is_returning"];
				return_date					= dr["return_date"] == DBNull.Value ? DateTime.Now : (DateTime) dr["return_date"];
				is_vac_bank					= dr["is_vac_bank"] != DBNull.Value && (bool) dr["is_vac_bank"];
				vac_bank_detail				= dr["vac_bank_detail"] == DBNull.Value ? "" : (string) dr["vac_bank_detail"];
				is_returned_property		= dr["is_returned_property"] != DBNull.Value &&  (bool) dr["is_returned_property"];
				non_returned_value			= (double) dr["non_returned_value"];
                last_day_worked =  dr["last_day_worked"] == DBNull.Value ? term_date : (DateTime)dr["last_day_worked"];
            }
			else
				{
				throw new Exception("Entry doesn't exist.");
				}
			}
		public void save()
			{
			if(id != 0)
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
	protected void button_save_Click(object sender, EventArgs e)
	{
		var is_errored = false;
		var errors = new List<string>();
		if (string.IsNullOrWhiteSpace(tb_reason_actual.Text))
		{
			is_errored = true;
			errors.Add("Please provide the actual reason you are terminating this user");
		}
		if (string.IsNullOrWhiteSpace(ddl_roe.Text))
		{
			is_errored = true;
			errors.Add("Please provide the ROE reason for terminating this user");
		}
	    if (string.IsNullOrWhiteSpace(dte_last_date_worked.Text))
	        {
	        is_errored = true;
	        errors.Add("Please provide the last day this employee worked");
	        }
        if (is_errored)
		{
			lb_status.Style["color"] = "#C00";
			var _errors = "<div align='left'><ul>";
			foreach (var s in errors)
			{
				_errors += "<li>" + s;
			}
			_errors += "</ul></div>";
			lb_status.Text = _errors;
			return;
		}
		et.reason_actual = tb_reason_actual.Text;
		et.reason_roe = ddl_roe.Text;
		et.return_date = date_return.Date;
		et.term_time = time_term.DateTime.ToString("HH:mm");
		et.term_date = date_termination.Date;
		et.vac_bank_detail = memo_vac_bank.Text;
	    et.last_day_worked = dte_last_date_worked.Date;
		et.non_returned_value = tb_property.Text == "" ? 0 : Convert.ToDouble(tb_property.Text.Replace("$", ""));
		try
		{
			et.save();
			var priv = new NEMemberTypePrivilege();
			priv.ClearPrivileges(loaded_user.id);
			lb_status.Style["color"] = "#090";
			var mem_type = new NeMemberType(current_user.MemberTypeID);

			#region  delete from reviews, offers and ticket viewers

			try
			{
				_tools.getSQL_void(@"Delete from emp_review_history where member_id =@v0  and emp_review_history.date > curdate()", new object[] { loaded_user.id} );
			
                _tools.getSQL_void(@"update member_offers set status = 'Closed' where memberid = @v0 and member_offers.startdate > curdate()", new object[] { loaded_user.id });
				_tools.getSQL_void(@"Delete from ticket_memberview where member_id = @v0", new object[] { loaded_user.id});
				_tools.getSQL_void(@"Update vacation_master set status = 4 where vacation_master.member_id = @v0 and date_start > curdate()", new object[] { loaded_user.id });
				_tools.getSQL_void(@"delete from training_history where member_id = @v0 and date > curdate()", new object[] { loaded_user.id });
				_tools.getSQL_void(@"update passport set active=1 where `from` = @v0 and Active = 1", new object[] { loaded_user.NEEmail });
				_tools.getSQL_void(@"delete from auto_reports_schedule where member_id = @v0", new object[] { loaded_user.id });
				_tools.getSQL_void(@"delete from shopping_cart where member_id = @v0", new object[] { loaded_user.id });
				_tools.getSQL_void(@"delete from shopping_cart_header where member_id = @v0", new object[] { loaded_user.id });
                _tools.getSQL_void(@"Delete from emp_review where member_id = @v0 and emp_review.date > curdate()", new object[] { loaded_user.id });
                _tools.getSQL_void(@"update emp_review set status = 'Closed' where member_id = @v0 and status != 'Delivered'", new object[] { loaded_user.id });
                _tools.getSQL_void(@"update member_todo_list_helper set member_id = @v1 where member_id = @v0", new object[] { loaded_user.id, loaded_user.reports_to });
            }
			catch { }

			#endregion

			#region remove them from the oncall schedule

			_tools.getSQL_void(@"Delete from oncall_schedule where member_id =@v0",new object[] { loaded_user.id });
			#endregion
			send_emails(true);

		}
		catch (Exception ee)
		{
			lb_status.Style["color"] = "#C00";
			lb_status.Text = "Error Saving: " + ee;
		}
//		ScriptManager.RegisterStartupScript(this, typeof(string), "key", "window.parent.window.location.href = window.parent.window.location.href;", true);
	}
	private void send_emails(bool do_force)
		{
		if (loaded_user.Status == "Active" || do_force)
			{
				lb_status.Text = "Saved - User privileges & login permissions to NESI are completely revoked.";
				deactivate_NESI_account();
				/*if (loaded_user.LDAP_user != "")
				{
					deactivate_AD_account();
				}*/
				var actingManager		= loaded_user.business_unit.acting_right_hand > 0 ? new NeMember(loaded_user.business_unit.acting_right_hand) : new NeMember(); 
				var it_notice = new NeEMail();
				var hr_notice = new NeEMail();
				it_notice.To = is_debug ? debug_email : "it@newelectric.com;";
				it_notice.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
				it_notice.CC = is_debug ? "" : "payroll@newelectric.com;" + loaded_user.business_unit.branch_manager.NEEmail;
				if(actingManager.id > 0)
					{
					hr_notice.CC += hr_notice.CC == "" 
						? actingManager.NEEmail 
						: ";"+actingManager.NEEmail;
					}
				it_notice.Subject = loaded_user.FullName + " has been terminated by " + current_user.FullName;
				it_notice.isHTML = true;
				it_notice.Body = string.Format("Please login to NESI and fill out the System Administrator Checklist.");
				it_notice.Send();

				hr_notice.To = is_debug ? debug_email : "payroll@" + Toolbox.app_setting("DomainForEmail");
				hr_notice.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
				hr_notice.CC = is_debug ? "" : loaded_user.business_unit.branch_manager.NEEmail;
				if(actingManager.id > 0)
					{
					hr_notice.CC	+= hr_notice.CC == "" 
										? actingManager.NEEmail 
										: ";"+actingManager.NEEmail;
					}
				hr_notice.Subject = loaded_user.FullName + " has been terminated by " + current_user.FullName;
				hr_notice.isHTML = true;
				hr_notice.Body = string.Format("Please login and fill out the termination checklist for this employee");
				hr_notice.Send();



				hr_notice.To = is_debug ? debug_email : "payroll@" + Toolbox.app_setting("DomainForEmail");
				hr_notice.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
				hr_notice.CC = is_debug ? "" : loaded_user.business_unit.branch_manager.NEEmail + ";it@" + Toolbox.app_setting("DomainForEmail");
				if(actingManager.id > 0)
					{
					hr_notice.CC += hr_notice.CC == "" 
						? actingManager.NEEmail 
						: ";"+actingManager.NEEmail;
					}
				hr_notice.Subject = "Termination Checklist for " + loaded_user.FullName + " who has been terminated by " + current_user.FullName;
				hr_notice.isHTML = true;
				//   Need to create email.
				var sb = new StringBuilder();
				// Header information
				sb.AppendFormat(@"<div style='font-size:11px;font-family:arial;'>
<div><b>Employee Name: </b><u>{0}</u></div>
<div><b>Business Unit: </b><u>{1}</u></div>
<div><b>Position: </b><u>{2}</u></div>
<div><b>Termination Date: </b><u>{3}</u></div>
<div><b>Termination Time: </b><u>{4}</u></div>
<div><b>Last Date Worked: </b><u>{13}</u></div>
<div><b>Termination Reason (Actual): </b><u>{5}</u></div>
<div><b>Termination Reason (ROE): </b><u>{6}</u></div>
<div><b>Will the employee be returning?: </b><u>{7}</u></div>
<div><b>If the employee is returning please provide return date: </b><u>{8}</u></div>
<div><b>If the employee is returning, are they requesting their vacation pay or banked hours?: </b><u>{9}</u></div>
<div><b>If yes, please provide details: </b><u>{10}</u></div>
<div><b>Has employee returned all company property?: </b><u>{11}</u></div>
<div><b>Please estimate value of any company property not returned: </b><u>{12:C2}</u></div>
",
			loaded_user.FullName,
			current_user.business_unit_name,
			loaded_user.membertype.name,
			Toolbox.MySQL_shortdt(et.term_date),
			et.term_time,
			et.reason_actual,
			et.reason_roe,
			et.is_returning ? "Yes" : "No",
			Toolbox.MySQL_shortdt(et.return_date),
			et.is_vac_bank ? "Yes" : "No",
			et.vac_bank_detail,
			et.is_returned_property ? "Yes" : "No",
			et.non_returned_value,
            et.last_day_worked.ToString("yyyy-MM-dd")
			);
			
			}
		}
	private void deactivate_NESI_account()
		{
	//		shared.alert_payroll(loaded_user.FullName + " has been terminated", "Their sessions and accounts in both nesi.ca and active directory have been disabled");
	//		shared.alert_it(loaded_user.FullName + " has been terminated", "Their sessions and accounts in both nesi.ca and active directory have been disabled");
		ne_session.clear_active_sessions(loaded_user.id);
		if (loaded_user.hrstatus_id != 4 && loaded_user.hrstatus_id != 5)
		{
			loaded_user.TerminateDate = Toolbox.MySQL_shortdt(DateTime.Now);
		}
		loaded_user.hrstatus_id = 4;
		loaded_user.Status = "Not Active";
		loaded_user.sent_welcome_email = false;
		loaded_user.save();
		var payperiod_id = Toolbox.doSQL_int("CALL _payperiod()");
		if(Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE payroll_handler = @v0", new object[] { loaded_user.id}) > 0 && loaded_user.reports_to > 0)
			{
			Toolbox.doSQL_void(@"UPDATE member SET payroll_handler = @v1 WHERE payroll_handler = @v0", new object[] { loaded_user.id, loaded_user.reports_to});
			}
		Toolbox.doSQL_void(@"UPDATE member_offers SET status = 'Closed' WHERE memberid = @v0 AND startdate >= CURDATE()", new object[] { loaded_user.id } );
		Toolbox.doSQL_void(@"UPDATE vacation_master SET status = 4 WHERE member_id = @v0 AND payperiod_id > @v1", new object[] { loaded_user.id, payperiod_id});
		}
	/*private string AD_status()
		{
		var directoryEntry = new DirectoryEntry(Toolbox.app_setting("ldap_path"), Toolbox.app_setting("ldap_user"), Toolbox.app_setting("ldap_pass"), AuthenticationTypes.Secure);
		var directorySearcher = new DirectorySearcher(directoryEntry);
		if(directorySearcher == null)
			{
			_tools.page_author			= new NeMember(1295);
			_tools.catch_error(new Exception("User "+loaded_user.LDAP_user+" doesn't exist... please check the ldap account for "+loaded_user.FullName));
			return "";
			}
		directorySearcher.Filter = string.Format("(&(SAMAccountName={0}))", loaded_user.LDAP_user);
		var user = directorySearcher.FindOne();
		if(user != null)
			{
		var u	= user.GetDirectoryEntry();
		var status	= Convert.ToInt32(u.Properties["userAccountControl"].Value);
		return status == 512 ? "Active" : status == 514 ? "Inactive" : status.ToString();
			}
		else
			{
			return "N/A";
			}
		}
	private void deactivate_AD_account()
		{
		if(AD_status() == "Active")
			{
		var directoryEntry = new DirectoryEntry(Toolbox.app_setting("ldap_path"), Toolbox.app_setting("ldap_user"), Toolbox.app_setting("ldap_pass"), AuthenticationTypes.Secure);
		var directorySearcher = new DirectorySearcher(directoryEntry);
		if(directorySearcher == null)
			{
			_tools.page_author			= new NeMember(1295);
			_tools.catch_error(new Exception("User "+loaded_user.LDAP_user+" doesn't exist... please check the ldap account for "+loaded_user.FullName));
			return;
			}
		directorySearcher.Filter = string.Format("(&(SAMAccountName={0}))", loaded_user.LDAP_user);
		var user = directorySearcher.FindOne();
			if(user != null)
				{
				var u	= user.GetDirectoryEntry();
				u.Properties["userAccountControl"].Value	= 0x0002;
				u.CommitChanges();
				}
			}
		}
*/
	protected void fill_gv()
	{
		
		var tid = et.id.ToString();
		var _type = Session["term_gv_filter"] == null ? "" : Session["term_gv_filter"].ToString();
		var _real_type = _type;
		if (_type =="All")
		{
			_type ="";
		}
		else
		{
			_type = " and b.type='" + _type + "'";
		}
		


		gv_chklist1.DataSource = _tools.getSQL_datatable(string.Format(@"SELECT 
			a.id id,
			b.field field,
			a.is_checked chk,
			a.response response,
			m.member_fullname chkby,
			b.response_required resp_req,
			a.comment comment,
			b.type _type,
if(a.is_checked=1,fun_time(a.dt_modified),'') dt
		FROM 
			emp_trm_chklist_lnk a
		LEFT JOIN
			emp_trm_chklist_opt b 
				ON a.opt_id = b.id
		left join member m on a.chk_by = m.member_id
		WHERE 
			a.trm_id = {0} {1}", tid,_type),null);
		gv_chklist1.DataBind();
		set_tabs();
		

		var lbl = (ASPxLabel)gv_chklist1.FindTitleTemplateControl("lbltitle");
		switch (_real_type)
		{
			case "All":
				lbl.Text = "Showing Complete Checklist";
				break;
			case "SysAdmin":
				lbl.Text = "Showing System Administrator Checklist";
				if (current_user.id == 711 || current_user.id == 8 || current_user.id == 1295)
				{
					_enabled = true;
				}
				else
				{
					_enabled = false;
				}
				break;
			case "Payroll":
				if (current_user.AuthenticatedForPrivilege(37) && (current_user.business_unit_id == 11))
				{
					_enabled = true;
				}
				else
				{
					_enabled = false;
				}
				lbl.Text = "Showing Payroll Admin Checklist";
				break;
			case "Direct":
				if (NeMember.is_supervisor(loaded_user.id,current_user.id))
				{
					_enabled = true;
				}
				else
				{
					_enabled = false;
				}
				lbl.Text = "Showing Direct Supervisor Checklist";
				break;
		}
		if (loaded_user.hrstatus_id == 5)
		{
			_enabled = false;
			var bb = (ASPxButton)gv_chklist1.FindFooterCellTemplateControl(gv_chklist1.Columns[0], "btnsave");
			bb.Enabled = false;
		}
		
	}

	protected void set_tabs()
	{

		var stat = _tools.getSQL_datatable(@"SELECT ifnull((Select count(a.id) from emp_trm_chklist_lnk a INNER JOIN emp_trm_chklist_opt c on c.id= a.opt_id where c.type = b.type and a.is_checked=0 and a.trm_id = @v0  GROUP BY c.type ) ,0)id, b.type _type FROM emp_trm_chklist_opt b GROUP BY b.type", new object[] {  et.id } );

		if (stat.Rows.Count > 0)
		{
			var gv_chklist1 = (ASPxGridView)pc.FindControl("gv_chklist1");
			var lbldirect = (ASPxLabel)gv_chklist1.FindTitleTemplateControl("lbldirect");
			var lblpayroll = (ASPxLabel)gv_chklist1.FindTitleTemplateControl("lblpayroll");
			var lblsystems = (ASPxLabel)gv_chklist1.FindTitleTemplateControl("lblsystems");
		
			foreach (DataRow stat_row in stat.Rows)
			{
			
				if (stat_row["_type"].ToString() == "Direct")
				{
					
					lbldirect.Text = stat_row["id"].ToString();
					if (stat_row["id"].ToString() == "0")
					{
						lbldirect.ForeColor = System.Drawing.Color.Black;
					}
					else
					{
						lbldirect.ForeColor = System.Drawing.Color.Red;
					}
				}
				else if (stat_row["_type"].ToString() == "Payroll")
				{
				
					lblpayroll.Text = stat_row["id"].ToString();
					if (stat_row["id"].ToString() == "0")
					{
						lblpayroll.ForeColor = System.Drawing.Color.Black;
					}
					else
					{
						lblpayroll.ForeColor = System.Drawing.Color.Red;
					}
				}
				else if (stat_row["_type"].ToString() == "SysAdmin")
				{
				
					lblsystems.Text = stat_row["id"].ToString();
					if (stat_row["id"].ToString() == "0")
					{
						lblsystems.ForeColor = System.Drawing.Color.Black;
					}
					else
					{
						lblsystems.ForeColor = System.Drawing.Color.Red;
					}
				}
				
				
			}
	//		ASPxTabControl tc = (ASPxTabControl)uc_member_usage1.FindControl("tc");


		

			if ((lbldirect.ForeColor == System.Drawing.Color.Black) && (lblpayroll.ForeColor == System.Drawing.Color.Black) && (lblsystems.ForeColor == System.Drawing.Color.Black) && (loaded_user.hrstatus_id==4))
			{
			
		//		uc_member_usage1.rebuild_tabs();
				if ((Session["has_open_items" + loaded_user.id] == null) || (Session["has_open_items" + loaded_user.id].ToString() == "True"))
				{
				//	gv_chklist1.JSProperties["cp_refresh"] = "1";
					ScriptManager.RegisterStartupScript(this, typeof(string), "key1", "alert('This person still has open footprints, so they need to be re delegated BEFORE they will be officially set to PAST status');", true);
				}
				else
				{
					if ((IsPostBack)||(IsCallback))
					{
						if (Session["has_open_items" + loaded_user.id].ToString() == "False")
						{
							if (validate())
							{
								finish();
								gv_chklist1.JSProperties["cp_refresh"] = "1";
						//		ScriptManager.RegisterStartupScript(this, typeof(string), "key1", "alert('Good Job!  you made it through the checklist!  The persons HR status will be now set to PAST.');location.href=location.href;", true);
							}
						}
					}
				}
			}
		}
		if (loaded_user.hrstatus_id == 5)
		{
			_enabled = false;
			var bb = (ASPxButton)gv_chklist1.FindFooterCellTemplateControl(gv_chklist1.Columns[0], "btnsave");
			bb.Enabled = false;
		}
	}

	protected void mem_Init(object sender, EventArgs e)
	{
		
			var mem = sender as ASPxMemo;
			var container = mem.NamingContainer as GridViewDataItemTemplateContainer;
			mem.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_chklist1.PerformCallback('{0}|m|' + s.GetValue()); }}", container.KeyValue);
		
	}
	protected void chk_box_Init(object sender, EventArgs e)
	{
		
			var mem = sender as ASPxCheckBox;
			var container = mem.NamingContainer as GridViewDataItemTemplateContainer;
			var res_req = gv_chklist1.GetRowValuesByKeyValue(container.KeyValue, "resp_req").ToString() == "False" ? "0" : "1";
			mem.ClientSideEvents.CheckedChanged = string.Format(@"function (s, e) {{  ccc(s.mainElement, {0},{1},s.GetValue(),{2});}}", container.KeyValue, res_req, "ddl_response_" + container.KeyValue + ".GetText()");
		
	}
	protected void ddl_user_PreRender(object sender, EventArgs e)
	{
		var mem = sender as ASPxComboBox;
		var container = mem.NamingContainer as GridViewDataItemTemplateContainer;
		mem.ClientVisible = gv_chklist1.GetRowValues(container.VisibleIndex, "resp_req").ToString() == "True" ? true : false;
		mem.ClientEnabled = gv_chklist1.GetRowValues(container.VisibleIndex, "chk").ToString() == "1" ? false : true;
		var res_req = gv_chklist1.GetRowValuesByKeyValue(container.KeyValue, "resp_req").ToString() == "False" ? "0" : "1";

		mem.ClientInstanceName = "ddl_response_" + container.KeyValue;
		mem.ClientSideEvents.SelectedIndexChanged = string.Format(@"function (s, e) {{  gv_chklist1.PerformCallback('{0}|r|' + s.GetText());}}", container.KeyValue, res_req, "s.GetText()");


	}

	protected void gv_chklist_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters != null)
		{
			if (e.Parameters.Contains('|'))
			{
				var paras			= e.Parameters.Split('|');
				var key = paras[0];
				var val = paras[2];
				var res_req = gv_chklist1.GetRowValuesByKeyValue(key, "resp_req").ToString();
				var _type = gv_chklist1.GetRowValuesByKeyValue(key, "_type").ToString();
				var field = gv_chklist1.GetRowValuesByKeyValue(key, "field").ToString();
				var comments = gv_chklist1.GetRowValuesByKeyValue(key, "comment").ToString();
				if (paras[1] == "c")
				{
					_tools.getSQL_void(@"update emp_trm_chklist_lnk set is_checked = " + (val == "1" ? 1 : 0) + ",chk_by=" + (val == "1" ? current_user.id : 0) + " where id = " + key + " limit 1");
				}
				else if (paras[1] == "m")
				{
					_tools.getSQL_void(@"update emp_trm_chklist_lnk set comment =@v0 where id = @v1 limit 1",new object[] {
						val,key
						});
				}
				if (res_req == "True")
				{
					if (paras.Length > 3 || paras[1] == "r")
					{
						var val2 = paras[1] == "r" ? val : paras[3];
						var resp = paras[1] == "r" ? val2 :  (val == "1" ? val2 : "");
						_tools.getSQL_void(@"update emp_trm_chklist_lnk set response =@v0 where id =@v1  limit 1", new object[] {
					resp,key
						});
						if ((_type != "SysAdmin")&&(resp!=""))
						{
							var email = new NeEMail();
							email.To = "it@" + Toolbox.app_setting("DomainForEmail");
							email.Subject = loaded_user.FullName + " Checklist -" + field + " : " + val2;
							email.Body = loaded_user.FullName + " Checklist -" + field + " : " + val2 + System.Environment.NewLine;
							email.Body += current_user.FullName + " just set this in the termination checklist" + System.Environment.NewLine;
							email.Body += "Notes:" + comments;
							email.From = "it@" + Toolbox.app_setting("DomainForEmail");
							email.Send();
						}
					}
				}
			}
			else if (e.Parameters == "saveall")
			{
				for (var x = 0; x < gv_chklist1.VisibleRowCount; x++)
				{
					var key = gv_chklist1.GetRowValues(x, "id").ToString();
					var is_selected = gv_chklist1.Selection.IsRowSelected(x);
					var is_already_selected = _tools.getSQL_int(@"Select is_checked from emp_trm_chklist_lnk  where id =@v0 limit 1 ", new object[] { key });

					if (is_selected != Convert.ToBoolean(is_already_selected))
					{
						_tools.getSQL_void(@"update emp_trm_chklist_lnk 
set is_checked = @v0,chk_by=@v1 where id = @v2 limit 1", new object[] {
							(is_selected ? 1 : 0),(is_selected ? current_user.id : 0),key
											   
											   });
					}


				}

			}
			else
			{
				Session["term_gv_filter"] = e.Parameters;
				
			}
		}
	
		fill_gv();
		set_selected();
		set_tabs();
	}
	

	protected void create_term_chklist()
	{
		if (loaded_user.hrstatus_id == 4)
		{

			var dt = _tools.getSQL_datatable(@"Select * from emp_trm_chklist_opt"  , null);
			foreach (DataRow dr in dt.Rows)
			{
				if (_tools.getSQL_int(@"Select count(id) from emp_trm_chklist_lnk  where trm_id =@v0 and opt_id =@v1 ", new object[] { et.id,dr["id"] }) == 0)
				{
					_tools.getSQL_void(@"Insert into emp_trm_chklist_lnk (trm_id,opt_id,dt_added)  values (@v0,@v1,now())",new object[] { et.id,dr["id"] } );
				}
			}
		}
	}

	protected void clear_term_chklist()
	{
		_tools.getSQL_void(@"Delete from emp_trm_chklist_lnk where trm_id = @v0" , new object[] {
			et.id});

	}

	protected void clear_auto_reports()
	{
		_tools.getSQL_void(@"Delete from auto_reports_schedule where member_id = @v0" , new object[] {
			et.trm_member_id});
	}
	

	protected void gv_chklist1_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			e.Row.Enabled = _enabled;
			if (gv_chklist1.GetRowValuesByKeyValue(e.KeyValue, "chk").ToString() == "0")
			{
				e.Row.BackColor = System.Drawing.Color.LightSalmon;
				
			}
			else
			{
				
	//				gv_chklist1.Selection.SelectRowByKey(e.KeyValue);
			}
		}
	}

	protected void set_selected()
	{
		for (var x = 0; x < gv_chklist1.VisibleRowCount; x++)
		{
			if (gv_chklist1.GetRowValues(x, "chk").ToString() == "1")
			{
				gv_chklist1.Selection.SelectRow(x);
			}

		}

	}

	protected bool validate()
	{
		var is_errored = false;
		var errors = new List<string>();
		if (string.IsNullOrWhiteSpace(tb_reason_actual.Text))
		{
			is_errored = true;
			errors.Add("Please provide the actual reason you are terminating this user");
		}
		if (string.IsNullOrWhiteSpace(ddl_roe.Text))
		{
			is_errored = true;
			errors.Add("Please provide the ROE reason for terminating this user");
		}
		if (is_errored)
		{
			lb_status.Style["color"] = "#C00";
			var _errors = "<div align='left'><ul>";
			foreach (var s in errors)
			{
				_errors += "<li>" + s;
			}
			_errors += "</ul></div>";
			lb_status.Text = _errors;
			return false;
			
		}
		lb_status.Text = "";
		return true;
	}

	protected void finish()
	{
		var is_errored = validate();
		
		et.reason_actual = tb_reason_actual.Text;
		et.reason_roe = ddl_roe.Text;
		et.return_date = date_return.Date;
		et.term_time = time_term.DateTime.ToString("HH:mm");
		et.term_date = date_termination.Date;
		et.vac_bank_detail = memo_vac_bank.Text;
		et.non_returned_value = tb_property.Text == "" ? 0 : Convert.ToDouble(tb_property.Text.Replace("$", ""));
		try
		{
			et.save();
			var priv = new NEMemberTypePrivilege();
			priv.ClearPrivileges(loaded_user.id);
		
			var hr_notice = new NeEMail();
			if (loaded_user.Status == "Active")
			{
				lb_status.Text = "Saved - User privileges & login permissions to NESI are completely revoked.";
				deactivate_NESI_account();
				/*if (loaded_user.LDAP_user != "")
				{
					deactivate_AD_account();
				}*/
				var it_notice = new NeEMail();
				it_notice.To = is_debug ? "debug@" + Toolbox.app_setting("DomainForEmail") : "it@" + Toolbox.app_setting("DomainForEmail") + ";";
				it_notice.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
				it_notice.CC = is_debug ? "" : "payroll@" + Toolbox.app_setting("DomainForEmail") + ";" + loaded_user.business_unit.branch_manager.NEEmail;
				it_notice.Subject = current_user.FullName + " has completed termination procedure for " + loaded_user.FullName;
				it_notice.isHTML = true;
				it_notice.Body = "";
				it_notice.Send();

				hr_notice.To = is_debug ? debug_email : "payroll@" + Toolbox.app_setting("DomainForEmail");
				hr_notice.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
				hr_notice.CC = is_debug ? "" : loaded_user.business_unit.branch_manager.NEEmail;
				hr_notice.Subject = current_user.FullName + " has completed termination procedure for " + loaded_user.FullName;
				hr_notice.isHTML = true;
				hr_notice.Body = "";
				hr_notice.Send();

			}

			hr_notice.To = is_debug ? "debug@" + Toolbox.app_setting("DomainForEmail") : "payroll@" + Toolbox.app_setting("DomainForEmail");
			hr_notice.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
			hr_notice.CC = is_debug ? "" : loaded_user.business_unit.branch_manager.NEEmail + ";it@" + Toolbox.app_setting("DomainForEmail");
			hr_notice.Subject = "Termination Checklist for " + loaded_user.FullName;
			hr_notice.isHTML = true;

			//   Need to create email.
			var sb = new StringBuilder();
			// Header information
			sb.AppendFormat(@"<div style='font-size:11px;font-family:arial;'>
<div><b>Employee Name: </b><u>{0}</u></div>
<div><b>Business Unit: </b><u>{1}</u></div>
<div><b>Position: </b><u>{2}</u></div>
<div><b>Termination Date: </b><u>{3}</u></div>
<div><b>Termination Time: </b><u>{4}</u></div>
<div><b>Last Day Worked: </b><u>{13}</u></div>
<div><b>Termination Reason (Actual): </b><u>{5}</u></div>
<div><b>Termination Reason (ROE): </b><u>{6}</u></div>
<div><b>Will the employee be returning?: </b><u>{7}</u></div>
<div><b>If the employee is returning please provide return date: </b><u>{8}</u></div>
<div><b>If the employee is returning, are they requesting their vacation pay or banked hours?: </b><u>{9}</u></div>
<div><b>If yes, please provide details: </b><u>{10}</u></div>
<div><b>Has employee returned all company property?: </b><u>{11}</u></div>
<div><b>Please estimate value of any company property not returned: </b><u>{12:C2}</u></div>
",
		loaded_user.FullName,
		loaded_user.business_unit_name,
		loaded_user.membertype.name,
		Toolbox.MySQL_shortdt(et.term_date),
		et.term_time,
		et.reason_actual,
		et.reason_roe,
		et.is_returning ? "Yes" : "No",
		Toolbox.MySQL_shortdt(et.return_date),
		et.is_vac_bank ? "Yes" : "No",
		et.vac_bank_detail,
		et.is_returned_property ? "Yes" : "No",
		et.non_returned_value,
        et.last_day_worked
		);
			// Checklist
			var _types = new List<string>(new string[] { "Direct", "SysAdmin", "Payroll" });
			for (var t = 0; t < _types.Count; t++)
			{
				var dt = Toolbox.doSQL_dt(@"CALL emp_trm_chklist(@v0 , @v1 )", new object[] {  et.id, _types[t] } );
				if (dt.Rows.Count > 0)
				{
					sb.Append("<br/><b>" + _types[t] + " checklist</b><hr/>");
					foreach (DataRow dr in dt.Rows)
					{
						var field = dr["field"].ToString();
						var is_checked = dr["is_checked"].ToString() == "1";
						var response = dr["response"].ToString();
						var comment = dr["comment"].ToString();
						if (response != "")
						{
							field += " -- <u>" + response + "</u>";
						}
						if (comment != "")
						{
							field += " -- <u>" + comment + "</u>";
						}
						var chk = is_checked ? "&#9745;" : "&#9744;";
						sb.AppendFormat("<div><span style='width:50px;'>{0}</span><span>{1}</span></div>\n", chk, field);
					}
				}
				else
				{
					sb.Append("<i>The " + _types[t] + " checklist has not been filled out.</i><hr/><br/>\n");
				}

			}
		    hr_notice.Body = sb.ToString();
        	hr_notice.Send();


			if (loaded_user.hrstatus_id != 4 && loaded_user.hrstatus_id != 5)
			{
				loaded_user.TerminateDate = Toolbox.MySQL_shortdt(DateTime.Now);
			}

			loaded_user.hrstatus_id = 5;
			
			loaded_user.Status = "Not Active";
			loaded_user.save();

		}
		catch (Exception ee)
		{
			lb_status.Style["color"] = "#C00";
			lb_status.Text = "Error Saving: " + ee;
		}
//		ScriptManager.RegisterStartupScript(this, typeof(string), "key", "window.parent.window.location.href = window.parent.window.location.href;", true);
	


	}
	protected void pc_ActiveTabChanged(object source, TabControlEventArgs e)
	{

	}
}