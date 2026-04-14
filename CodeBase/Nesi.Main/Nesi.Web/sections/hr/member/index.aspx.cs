using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;

using System.Text;
using System.Text.RegularExpressions;
using System.Data;

using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using nesi.core;

public partial class sections_hr_member_index : System.Web.UI.Page
	{
	NeMember current_user;
	private int page_id					= 127;
		bool can_see_wage;
		bool can_see_it;
		bool can_edit_info;
		bool can_see_priv;
		bool can_see_userswitch;
		bool can_view_all_reviews;
		bool is_depart_hr;
		bool is_branch_hr;
		bool can_term_all;
		bool can_edit_other_branches;
		bool can_review;
		bool issupervisor;
		bool ispayroll;
	    private bool isowner;
		int id;
		int mo_id;
		int comp_id;
		Toolbox _tools;
		NameValueCollection _q;
	NeMember user			= new NeMember();
	protected void Page_Init(object sender, EventArgs e)
		{
		Toolbox.do_debug(@"Start (member/index.aspx) - Page Init");
		sqlmember.DataBind();
		membertype1.DataBind();
		_tools								= new Toolbox();
		_tools.page_author					= new NeMember(8);
		current_user						= Toolbox.do_handle_authentication(page_id);
		_q									= Request.QueryString;
		id									= string.IsNullOrEmpty(_q["id"]) || _q["id"] == "0" ? 0 : Convert.ToInt32(_q["id"]);
		mo_id								= string.IsNullOrEmpty(_q["mo_id"]) || _q["mo_id"] == "0" ? 0 : Convert.ToInt32(_q["mo_id"]);
		if(id > 0)
			{
			user								= new NeMember(id);
			uc_days_off.active_member_id		= Convert.ToInt32(id);
			if(current_user.AuthenticatedForPrivilege(165))
				{
				uc_vacation.user					= user;
				uc_vacation.admin					= current_user;
				uc_vacation.DataBind();
				}
			else
				{
				uc_vacation.Visible				= false;
				}
			uc_member_usage1.employee_id = Convert.ToInt32(id);
			uc_member_usage1.DataBind();
			}
		else
			{
				uc_member_usage1.Visible = false;
			uc_days_off.Visible					= false;
			}
		Toolbox.do_debug(@"End (member/index.aspx) - Page Init");
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		lb_status.Text						= "";
		Title								= "Editing " + user.FullName;
		wage_tab._id						= user.id32;
		wage_tab.user						= user;
		it_tab._id							= user.id32;
		it_tab.user							= user;
		hdnmember.Value						= id.ToString();
		comp_id								= user.id == 0 ? current_user.business_unit.id32 : user.business_unit.id32;
		wage_tab._comp_id					= user.id == 0 ? current_user.business_unit.id32 : user.business_unit.id32;
        if(IsPostBack)
			{
			GeneratePrivileges();
			}
		issupervisor                        = NeMember.is_supervisor(user.id,current_user.id);
        isowner = NeMember.is_owner( current_user.id, user.business_unit.tax_entity_id);
		can_see_wage						= current_user.AuthenticatedForPrivilege(101) || issupervisor || isowner;  // 
		can_see_it							= current_user.AuthenticatedForPrivilege(126);
		can_edit_info						= current_user.AuthenticatedForPrivilege(32);
		can_see_priv						= current_user.AuthenticatedForPrivilege(36);
		can_see_userswitch					= current_user.AuthenticatedForPrivilege(170);
		can_edit_other_branches				= current_user.AuthenticatedForPrivilege(6);
		ispayroll                           = current_user.AuthenticatedForPrivilege(37) || current_user.id == 8 || current_user.id == 711 || current_user.is_CAN_boardmember || current_user.is_US_boardmember;
		can_term_all                        = current_user.AuthenticatedForPrivilege(156);
		is_branch_hr                        = current_user.AuthenticatedForPrivilege(33);
		is_depart_hr                        = current_user.AuthenticatedForPrivilege(35);
		can_review                          = current_user.AuthenticatedForPrivilege(128);
		can_view_all_reviews				= current_user.AuthenticatedForPrivilege(144);

        Toolbox.FriendlyException(Response, "This page is deactivated. Please use the employees page in the menu", "/#/opens/127/employees/"+id);

        if (!isowner&&!is_branch_hr && !is_depart_hr && !issupervisor && !can_edit_other_branches||user.isContact)
		{
			Toolbox.FriendlyException(Response, "You are not authorised to see this page", "./frame.aspx");
		}

		
		if (is_branch_hr && !can_edit_other_branches && !isowner)
		{
			if (user.business_unit_id != current_user.business_unit_id && user.business_unit_id != 0&&!issupervisor)
			{
				Toolbox.FriendlyException(Response, "You can only access users from your branch", "./frame.aspx");
			}
		}

		if (user.business_unit_id != current_user.business_unit_id && !isowner && user.business_unit_id != 0 && !can_edit_other_branches&&!issupervisor)
			{
			Toolbox.FriendlyException(Response, "You can only access users from your branch", "./frame.aspx");
			}
		#region Deactivate user JS
		js_protected.InnerHtml				= user.Status == "Active" ? @"
<script type='text/javascript'>
function deactivate_user()
	{
		var roe = txt_term_roe.GetText();
		var act = txt_term_act.GetText();
        var ldw = 	dte_last_day_worked.GetText();	
        var obj = {roe:txt_term_roe.GetText(),act:txt_term_act.GetText(),ldw:dte_last_day_worked.GetText()};
		var url		= './if_termination.aspx?id=" + user.id+@"&type=direct&a=k&' + $.param(obj);
		$.ajax(
				{
				type:		'GET',
				url:		url,
				dataType:	'text',
				beforeSend: function()
								{
								please_wait('start', 'Deactivating User');
								},
				success:	function(resp)
								{
								if(resp == 'SUCCESS')
									{
									confirm('User has been deactivated',5000);
									}
								else
									{
									confirm(resp);
									}
								},
				complete:	function()
								{
								please_wait('stop');
								pc.GetTab(7).SetVisible(true);
								pc.SetActiveTabIndex(7);
								location.hash = '#top';
								$('.if_termination').attr('src', $('.if_termination').attr('src'));
								},
				error:
					function()
						{
						please_wait('stop');
						alert('There was an error terminating this user');
						}
				}
				);
		
	}
</script>
" : "";
		bt_terminate.ClientSideEvents.Click	= user.Status == "Active" ?
			@"function(s, e) {
if (confirm('Are you sure you want to TERMINATE " + user.FullName  + @" ?'))
			{
pop_term.Show();

}
else
{

}
}" : 
	@"function(s, e) {
pc.GetTab(7).SetVisible(true);
pc.SetActiveTabIndex(7);
location.hash = '#top';
}";
		#endregion Deactivate user JS
		var include_elevated				= current_user.membertype.is_elevated ? "" : " AND b.is_elevated = FALSE";
		wage_info.Visible					= can_see_wage;
		row_payroll_handler.Visible			= issupervisor || ispayroll;
		tb_payroll_id.ClientEnabled			= ispayroll;
		if (pc.TabPages[0].Name == "0" && (issupervisor||ispayroll))  //user info
			{
			pc.TabPages[0].ClientEnabled = true;
			}
		else
			{
			bt_save.Visible				= false;
			}
		if (pc.TabPages[1].Name == "1" && can_see_it)  //it
			{
			pc.TabPages[1].ClientEnabled = true;
			}

		    if (pc.TabPages[2].Name == "2" &&
		        (   
		            issupervisor ||
		            current_user.reports_to == 0 && (isowner) ||
		            can_edit_other_branches && current_user.is_backoffice ||
                 ((is_branch_hr || ispayroll)  )
		        )
		    ) //wage
		        {
		        if (is_branch_hr || ispayroll || current_user.is_backoffice)
		            {
		            pc.TabPages[2].ClientEnabled = true;  // wage
		            }
		            pc.TabPages[8].ClientEnabled = true;  // employment agreement
		        }

		    if (pc.TabPages[3].Name == "3" && (isowner||issupervisor || ispayroll))  // days
			{
			pc.TabPages[3].ClientEnabled = true;
			}
		if (pc.TabPages[4].Name == "4" && (isowner||issupervisor || ispayroll))  // disc
			{
           
            pc.TabPages[4].ClientEnabled = true;
			}
		if (pc.TabPages[5].Name == "5" && (can_see_priv || can_see_userswitch))	// priv
			{
			pc.TabPages[5].ClientEnabled = true;
			}
		if(can_see_userswitch && !can_see_priv)
			{
			pnl_privileges.Visible		= false;
			}
		if (pc.TabPages[6].Name == "6" && (isowner||issupervisor || ispayroll))  // files
			{
			pc.TabPages[6].ClientEnabled = true;
			}
		if (pc.TabPages[7].Name == "7")   //term	
		{
            // Added the ability for Payroll to see Termination Tab when user is not active and they have the privileges - Ticket no. 4494
            if (isowner||issupervisor || ispayroll ||
					can_term_all || 
					user.Status == "Not Active" && 
					current_user.AuthenticatedForPage(44) && 
					current_user.is_backoffice ||
					user.business_unit_id == current_user.business_unit_id && current_user.MemberTypeID == 5
				)
			{
				pc.TabPages[7].ClientEnabled = true;
				bt_terminate.ClientVisible = true;
			}
			else
			{
				pc.TabPages[7].ClientEnabled = false;

			}
		}
		
		if (pc.TabPages[9].Name == "9" && (isowner||issupervisor || ispayroll||current_user.AuthenticatedForPrivilege(144)))   //review	
		{
          
            pc.TabPages[9].ClientEnabled = true;
		}
		if (pc.TabPages[10].Name == "10" && (isowner||issupervisor || can_edit_info))   //usage	
		{
			pc.TabPages[10].ClientEnabled = true;
		}

		if (id == 0)
			{
			pc.TabPages[1].ClientEnabled	=	pc.TabPages[2].ClientEnabled = 
												pc.TabPages[3].ClientEnabled = 
												pc.TabPages[4].ClientEnabled = 
												pc.TabPages[5].ClientEnabled = 
												pc.TabPages[6].ClientEnabled =
												pc.TabPages[8].ClientEnabled =
												pc.TabPages[9].ClientEnabled = 
												pc.TabPages[10].ClientEnabled=
												pc.TabPages[7].ClientEnabled = false;
			}
		
		if(!IsPostBack)
			{
			populate_user(user);
			}
		pc.Visible = true;
		
		}
	protected void ddl_title_SelectedIndexChanged(object sender, EventArgs e)
		{
		var mt				= new NeMemberType();
		var chargeout			= Toolbox.doSQL_double(@"SELECT IFNULL(MAX(chargeout), 0) FROM membertype_chargeout WHERE business_unit_id = @v0  AND membertype_id = @v1  AND paytype_id = 1", new object[] {  ddl_branch.SelectedValue, ddl_title.SelectedValue } );
		lb_chargout.Text			= chargeout == 0 ? "Not set" : chargeout.ToString("C2");
		}
	protected void bt_save_Click(object sender, EventArgs e)
	{

	
		/*
		StringBuilder f					= new StringBuilder();
		foreach(string key in Request.Form.Keys ) 
			{
			string v			= Request.Form[key];
			if(key != "__VIEWSTATE" && key != "__EVENTVALIDATION")
				{
				f.AppendFormat("{0}=>{1}\n", key, v);
				}
			}
		string ff					= f.ToString();
		NeEMail post_catcher		= new NeEMail();
		post_catcher.To				= "mhyde@newelectric.com";
		post_catcher.From			= "administrator@newelectric.com";
		post_catcher.Subject		= "User Submission";
		post_catcher.isHTML			= false;
		post_catcher.Body			= ff.ToString();
		post_catcher.Send();
		*/
		try
		{
			var _q = Request.QueryString;
			var id = string.IsNullOrEmpty(_q["id"]) || _q["id"] == "0" ? 0 : Convert.ToInt32(_q["id"]);

			var paytype_id = Convert.ToInt32(ddl_emptype.SelectedValue) == 0 ? 1 : Convert.ToInt32(ddl_emptype.SelectedValue);

			var o_info = new NeMember(id);
			var n_info = new NeMember(id);
			var mo = new NeMemberOffer();
			// Validation Checks
			var errors = new List<string>();
			var hrstatus_id = ddl_hrstatus.Value==null?1:Convert.ToInt32(ddl_hrstatus.Value);
			ddl_hrstatus.Value = hrstatus_id;
			#region validation
			if (id == 0)  // if it's a new user
			{
				// Does username exist in any other users?
				if (Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_user = @v0  AND member_id != @v1 ", new object[] {  tb_username.Text, id } ) > 0)
				{
					errors.Add("Username already exists, please choose another.");
				}

			}
			// SSN/SIN Check
			// Ignore this if they are a subcontractor.
			if (ddl_emptype.SelectedValue != "4" && hrstatus_id > 2)
			{
				if (id != 0)  // if it's an existing user
				{
					if (Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_sin = @v0  AND member_id != @v1 ", new object[] {  tb_social.Text, id } ) > 0)
					{
						var ssn_id = Toolbox.doSQL_int(@"SELECT member_id FROM member WHERE member_sin = @v0  LIMIT 1", new object[] {  tb_social.Text } );
						var ssn_user = new NeMember(ssn_id);
						errors.Add(string.Format("SSN/SIN Already exists on employee #{0}, {1}", ssn_user.id, ssn_user.FullName));
					}
				}
			}
			if (ddl_emptype.SelectedValue != "4")
			{
				if (mo_id != 0)
				{
					mo = new NeMemberOffer(mo_id);
					if (Toolbox.doSQL_int(@"Select count(id) from member_offers  where status ='Released' and applicantid =@v0 and business_unit_id =@v1  and isapplicant=1 and id =@v2 ", new object[] { mo.applicantid,mo.business_unit_id,mo.id }) == 0)
					{
						errors.Add(string.Format("The applicant offer is either the wrong status or something else is busted there.. check it out before creating the new employee"));
					}
				}
			}
			// Test password strength / validity
			if (row_password.Visible && row_username.Visible)
			{
				if (Regex.IsMatch(tb_password.Text, @"[^\w\.@-]"))
				{
					errors.Add("Password can't contain illegal characters... use only numbers and letters");
				}
				if (!Regex.IsMatch(tb_password.Text, @"\d") || Regex.IsMatch(tb_password.Text, @"^\d+$") || tb_password.Text.Length < 6)  // if all the characters are numbers or if there are no numbers or it's too short
				{
					errors.Add("Password must contain both letters and digits and be at least 6 characters int");
				}

			}

			// Test reports_to 
			if (id != 0)  // if it's an existing user
			{
				if (ddl_reportsto.Text == "" || NeMember.Check_for_circular_org_chart(Convert.ToInt32(id), Convert.ToInt32(ddl_reportsto.SelectedValue)) == true)
				{
					errors.Add("Reporting Lines are Circular.  Somewhere between this person and the CEO, the reporting lines become circular.");
				}
				if ((personal_details.Visible == true)&&(current_user.business_unit.is_backoffice))
				{
					if (tb_payroll_id.Text == "")
					{
						errors.Add("Missing Payroll ID");
					}
					if (tb_benefits_id.Text == "" && n_info.benefits_startdate <= System.DateTime.Now)
					{
						errors.Add("Missing Benefits ID");
					}
					if (tb_life_insurance_id.Text == "")
					{
				//		errors.Add("Missing Life Insurance ID");
					}
				}

			}

			// Add as many of these checks as needed... adding each error to the list "errors"
			if (string.IsNullOrEmpty(ddl_default_location.SelectedValue) && Convert.ToInt32(ddl_branch.SelectedValue) != 11 && Convert.ToInt32(ddl_branch.SelectedValue) != 48 && hrstatus_id > 1)
			{
				errors.Add("Please select a valid default location for this user.");
			}
			if (string.IsNullOrEmpty(ddl_reportsto.SelectedValue))
			{
				errors.Add("Please select a valid person for this new user to report to.");
			}

			if ((id != 0 && hrstatus_id > 2)&& current_user.business_unit.is_backoffice)  // if it's an existing user
			{
				if (ddl_hrstatus.Value.ToString() == "3" || ddl_hrstatus.Value.ToString() == "6")
				{
					if (string.IsNullOrEmpty(tb_emerg_fname1.Text.Trim()))
					{
						errors.Add("Please enter a valid emergency contact first name");
					}
					if (string.IsNullOrEmpty(tb_emerg_lname1.Text.Trim()))
					{
						errors.Add("Please enter a valid emergency contact last name");
					}
					if (string.IsNullOrEmpty(tb_emerg1_area.Text.Trim()))
					{
						errors.Add("Please enter a valid emergency contact area code");
					}
					if (string.IsNullOrEmpty(tb_emerg1_pref.Text.Trim()))
					{
						errors.Add("Please enter a valid emergency contact phone prefix");
					}
					if (string.IsNullOrEmpty(tb_emerg1_suff.Text.Trim()))
					{
						errors.Add("Please enter a valid emergency contact phone suffix");
					}
					if (string.IsNullOrEmpty(tb_social.Text.Trim()))
					{
						errors.Add("Please enter a valid Social Insurance Number");
					}

			
				}
				
			}
			if (date_birth.Text == "")
			{
	//			errors.Add("Please save a valid birthdate");
			}

			if (hrstatus_id != n_info.hrstatus_id && hrstatus_id == 5)
			{

				var tc = (ASPxTabControl)uc_member_usage1.FindControl("tc");
				if (tc != null)
				{
					if (tc.Visible)
					{
						errors.Add("You can't set this person to PAST if they are still active in the system somehow.  Go to the Footprints tab and reassign the open items.");
					}
				}

			}

			// Finalize Validation, throwing the list to the status update handler, and letting it parse it to the bulleted list of errors.
			if (errors.Count > 0)
			{
				status_update(errors, true);
				return;
			}
			///////////////////////////////////////////////////////
			if(mo.id == 0 && mo_id > 0)
				{
				mo	= new NeMemberOffer(mo_id);
				}
			#endregion



			n_info.FirstName = tb_firstname.Text;
			n_info.MiddleInitial = tb_middleinitial.Text;
			n_info.LastName = tb_lastname.Text;
			n_info.Nickname = tb_nickname.Text;
			n_info.Address = tb_address.Text;
			n_info.City = tb_city.Text;
			n_info.Prov = ddl_provstate.SelectedValue;
			n_info.Country = ddl_country.SelectedValue;
			n_info.PostalCode = tb_postal.Text;
			n_info.Email = tb_email.Text.ToLower();
			n_info.empnotes = tb_notes.Text;

			//		n_info.hrstatus_id					= id == 0 ? 1 : Convert.ToInt32(ddl_hrstatus.SelectedValue) == 3 && o_info.hrstatus_id != 3 ? 6 : Convert.ToInt32(ddl_hrstatus.SelectedValue);
			n_info.hrstatus_id = ddl_hrstatus.Value==null?1:Convert.ToInt32(ddl_hrstatus.Value);
			n_info.NEEmail = id == 0 ? "nomail@thatsnew.com" : n_info.NEEmail;
			n_info.timetostat = id == 0 ? (n_info.Country == "CAN" ? 0 : 90) : n_info.timetostat;
			n_info.vacation_amount_1 = id == 0 ? (decimal)(n_info.Country == "CAN" ? 0.04 : 40) : n_info.vacation_amount_1;
			n_info.vacation_amount_2 = id == 0 ? (decimal)(n_info.Country == "CAN" ? 0.06 : 60) : n_info.vacation_amount_2;
			n_info.vacation_amount_3 = id == 0 ? (decimal)(n_info.Country == "CAN" ? 0.08 : 120) : n_info.vacation_amount_3;
			n_info.vacation_interval_1 = id == 0 ? (n_info.Country == "CAN" ? 0 : 12) : n_info.vacation_interval_1;
			n_info.vacation_interval_2 = id == 0 ? (n_info.Country == "CAN" ? 60 : 36) : n_info.vacation_interval_2;
			n_info.vacation_interval_3 = id == 0 ? (n_info.Country == "CAN" ? 120 : 60) : n_info.vacation_interval_3;
			n_info.payroll_handler = Convert.ToInt32(ddl_payrollhandler.SelectedValue);
			n_info.PhoneAreaCode = tb_homephone_area.Text;
			n_info.PhoneFirst = tb_homephone_pref.Text;
			n_info.PhoneLast = tb_homephone_suff.Text;
			n_info.Username = tb_username.Text;
			n_info.apprentice_contract = tb_apprentice_contract.Text;
			n_info.Password = tb_password.Text;
			n_info.StartDate = Toolbox.MySQL_shortdt(date_start.Date);
			//	n_info.TerminateDate				= Toolbox.MySQL_shortdt(date_end.Date);
			n_info.TerminateDate = n_info.TerminateDate == "" ? "2099-12-31" : Toolbox.MySQL_shortdt(date_end.Date);
		//	n_info.BirthDate = date_birth.Date.Year <= 1900 ? "1970-01-01" : Toolbox.MySQL_shortdt(date_birth.Date);
			n_info.BirthDate = date_birth.Text != "" ? Toolbox.MySQL_shortdt(date_birth.Date) : "";
			n_info.NETruck = tb_truck_number.Text;
			n_info.PhoneExtension = lb_extension.Text;
			n_info.business_unit_id = Convert.ToInt32(ddl_branch.SelectedValue);
			n_info.member_default_location = n_info.business_unit_id == 11 || n_info.business_unit_id == 48  ? 0 : Convert.ToInt32(ddl_default_location.SelectedValue);
			
			n_info.Status = ddl_status.SelectedValue;
			n_info.EmergencyFirstName1 = tb_emerg_fname1.Text;
			n_info.EmergencyLastName1 = tb_emerg_lname1.Text;
			n_info.EmergencyPhoneArea1 = tb_emerg1_area.Text;
			n_info.EmergencyPhoneFirst1 = tb_emerg1_pref.Text;
			n_info.EmergencyPhoneLast1 = tb_emerg1_suff.Text;
		    n_info.EmergencyFirstName2 = ""; //tb_emerg_fname2.Text;
		    n_info.EmergencyLastName2 = "";//tb_emerg_lname2.Text;
		    n_info.EmergencyPhoneArea2 = ""; //tb_emerg2_area.Text;

		    n_info.EmergencyPhoneFirst2 = ""; // tb_emerg2_pref.Text;

		    n_info.EmergencyPhoneLast2 = "";// tb_emerg2_suff.Text;

            n_info.HasDependants = ddl_hasdependants.SelectedValue;
			n_info.reports_to = id == 0 || ddl_reportsto.Enabled ? Convert.ToInt32(ddl_reportsto.SelectedValue) : n_info.reports_to;
			n_info.MemberTypeID = id == 0 ? Convert.ToInt32(ddl_title.SelectedValue) : n_info.MemberTypeID;
			n_info.is_CAN_boardmember = chk_can_isboardmember.Checked;
			n_info.is_US_boardmember = chk_us_isboardmember.Checked;
			
			if (tb_social.Text.Length > 3)
			{
				n_info.SIN = tb_social.Text == null ? null : tb_social.Text.Trim();
				n_info.SIN = n_info.SIN != null && n_info.SIN.Length < 3 ? null : n_info.SIN;
			}
			n_info.DriversLicence = tb_drivers_license.Text;
			n_info.ElectricalLicence = tb_electrical_license.Text;
			n_info.paytype_id = paytype_id == 0 ? mo.paytype_id == 0 ? 1 : mo.paytype_id : paytype_id;
		    

			n_info.pecell_area = tb_pecell_area.Text;
			n_info.pecell_pref = tb_pecell_pref.Text;
			n_info.pecell_suff = tb_pecell_suff.Text;

			n_info.benefits_id = tb_benefits_id.Text;
			n_info.PayrollNo = tb_payroll_id.Text;
			n_info.life_insurance_id = tb_life_insurance_id.Text;

			n_info.benefits_startdate = mo.id > 0 ? (mo.benefits_startdate == null ? Convert.ToDateTime(n_info.StartDate).AddMonths(3) : mo.benefits_startdate) : (o_info.benefits_startdate ==null? Convert.ToDateTime(n_info.StartDate).AddMonths(3):o_info.benefits_startdate);
			n_info.part_time = chkpart_time.Checked;
		    n_info.tswatch = (chk_ts_watch.Checked==true);
			errors.Clear();

			#region new user creation
			if (id == 0)  // if it's a new user
			{
				var hard_error = false;
				try
				{
					n_info.save();
				}
				catch (Exception ee)
				{
					_tools.catch_error(ee);
					hard_error = true;
					errors.Add(string.Format("There was an error saving this employee, not proceeding. The error thrown was: <div class='dtl'>{0}</div>", ee));
					status_update(string.Format("There was an error saving this employee, not proceeding. The error thrown was: <div class='dtl'>{0}</div>", ee), true);
				}


				#region Kick off PreLive function
				mo.memberid		= n_info.id;
				mo.save();
			    NESI.BLL.Pages.Employees.NeMemberOffer.go_prelive(mo_id,current_user,"hired");

				#endregion

				Response.Redirect("./index.aspx?id=" + n_info.id, false);
				Context.ApplicationInstance.CompleteRequest();

			}
			#endregion new user
			#region existing user update
			else
			{
				// UPDATE
				// Hard errors are defined as ones that keep the user from being updated/inserted via the NeMember Class
				var hard_error = false;
				// Soft errors are defined as ones where the user has been inserted/updated, but the information afterward privileges/wage/etc.. wasn't updated... this error should be noted and sent to HR
				//bool soft_error						= false;

				try
				{
					n_info.save();
				}
				catch (Exception ee)
				{
					hard_error = true;
					status_update(string.Format("There was a problem saving this employee's information. The error thrown was: <div class='dtl'>{0}</div>", ee), true);
				}

				if (o_info.hrstatus_id == 1 && Convert.ToInt32(ddl_hrstatus.Value) != 1)
				{
					try
					{
						var status_name = Toolbox.doSQL_string(@"SELECT status FROM member_hrstatus WHERE id = @v0  LIMIT 1", new object[] {  ddl_hrstatus.Value } );
						var em = new NeEMail();
						em.Subject = "(" + current_user.FullName + ") has moved (" + n_info.FullName + ") to the HR status of '" + status_name + "'";
						em.To = n_info.business_unit.branch_manager.NEEmail;
						em.From = current_user.NEEmail;
					
						em.Send();
					}
					catch (Exception ee)
					{
						_tools.catch_error(ee);
						errors.Add(string.Format("There was an error sending a message to the branch manager of the HR status change: <div class='dtl'>{0}</div>", ee));
					}
				}
				if (o_info.hrstatus_id != n_info.hrstatus_id)
				{
					try
					{
						var typepriv = new NEMemberTypePrivilege();
						switch (n_info.hrstatus_id)
						{
							case 1:  // New
								typepriv.SetBaseLoginPrivilege(n_info.id);
								break;
							case 3:  // Approved
							case 6:  // Probation
								if(o_info.hrstatus_id != 6)
									{
									typepriv.SetMemberPrivileges(n_info.id);
									}
								break;
							case 4:  // Pending Closure 
							case 5:  // Past
								typepriv.ClearPrivileges(n_info.id);
								break;
						}
					}
					catch (Exception ee)
					{
						_tools.catch_error(ee);
						errors.Add(string.Format("There was a problem switching this user's hr status: <div class='dtl'>{0}</div>", ee));
						status_update(string.Format("There was a problem switching this user's hr status: <div class='dtl'>{0}</div>", ee), true);
					}
				}
				if (o_info.MemberTypeID != n_info.MemberTypeID || o_info.Status == "Not Active" && n_info.Status == "Active")
				{
					try
					{
						//NEMemberTypePrivilege createpriv = new NEMemberTypePrivilege();
						//createpriv.SetMemberPrivileges(n_info.id);
						// Doing this because it's using a method to clear privileges when setting the new member type privileges
						// This method was originally intended for use only when terminating a user, not when they were still active
						// So I am choosing to patch the issue for the time being versus doing a broader fix.
						//TODO:Fix the problem where user's status are cleared upon changing their member types.
						n_info.Status = "Active";
						n_info.save();
					}
					catch (Exception ee)
					{
						_tools.catch_error(ee);
						errors.Add(string.Format("There was a problem switching this user's member type: <div class='dtl'>{0}</div>", ee));
						status_update(string.Format("There was a problem switching this user's member type: <div class='dtl'>{0}</div>", ee), true);
					}
				}
				if (!hard_error && can_see_wage)
				{
					try
					{
						//		NeWage wage						= new NeWage(n_info.id);  // pull up the current wage info from the member
						//		wage.date						= Toolbox.MySQLNow_int();
						//		wage.current_wage				= Convert.ToDouble(tb_payrate.Text);
						//		wage.current_wage_type			= ddl_emptype.SelectedValue;
						//		wage.member_id_audit			= current_user.id;
						//		wage.member_id_added_by			= current_user.id;
						//		if ((Convert.ToDouble(tb_payrate.Text) != wage.current_wage)||(wage.current_wage_type!=ddl_emptype.SelectedValue))
						//		{
						//			wage.comment = "Wage or wage type was changed via the general page edit";
						//			wage.save();
						//		}
					}
					catch (Exception ee)
					{
						_tools.catch_error(ee);
						errors.Add(string.Format("There was a problem saving this employee's wage information. The error thrown was: <div class='dtl'>{0}</div>", ee));
						status_update(string.Format("There was a problem saving this employee's wage information. The error thrown was: <div class='dtl'>{0}</div>", ee), true);
					}
				}
				if (!hard_error)
				{
					try
					{
						var before = Toolbox.dict_create(o_info);
						var after = Toolbox.dict_create(n_info);
						var diff_from = before.Except(after).ToDictionary(k => k.Key, v => v.Value);
						var diff_to = after.Except(before).ToDictionary(k => k.Key, v => v.Value);

						if (diff_from.Count > 0)
						{
							var changes = Toolbox.dict_dump(diff_from) + "<b>Changed to</b> <br/>" + Toolbox.dict_dump(diff_to);
							var hr_notice = new NeEMail();
							hr_notice.To = current_user.id != 711 ? "payroll@" + Toolbox.app_setting("DomainForEmail") : "mhyde@thatsnew.com";
							hr_notice.From = "administrator@thatsnew.com";
						
							hr_notice.Subject = "User edit notice";
							hr_notice.isHTML = true;
							hr_notice.Body = string.Format("The user <b>{0}</b> was edited by <b>{1}</b> - Member ID ({2}).<br/><b>Here is a list of items changed:</b><br/>{3}", n_info.FullName, current_user.FullName, n_info.id, changes);
							hr_notice.Body += "<br/>HR Status Legend";
							hr_notice.Body += "<br/>1 - New";
							hr_notice.Body += "<br/>2 - Waiting for Initial Setup";
							hr_notice.Body += "<br/>3 - Approved";
							hr_notice.Body += "<br/>4 - Pending Closure";
							hr_notice.Body += "<br/>5 - Past";
							hr_notice.Body += "<br/>6 - Probation";


							if (errors.Count > 0)
							{
								foreach (var s in errors)
								{
									hr_notice.Body += s + "<br/>";
								}
							}
							hr_notice.Send();
						}
						status_update(string.Format("Successfully updated {0}'s info.", n_info.Nickname), false);
					}
					catch (Exception ee)
					{
						_tools.catch_error(ee);
						status_update(string.Format("There was a problem sending a notice to HR, all other information saved correctly.<br/> The error thrown was: <div class='dtl'>{0}</div>", ee), true);
					}
				}
			}
			#endregion existing user update
		}
		catch (Exception except)
		{
			_tools.catch_error(except);
			throw;
		}
	}

	private void fill_paytypes()
		{
		var dt		= new DataTable();
		dt.TableName		= "paytypes";
		dt.Columns.Add("value");
		dt.Columns.Add("text");
		if(id != 0)
			{
			dt.Rows.Add("1", "Hourly");
			dt.Rows.Add("2", "Salary w/ Timesheet");
			dt.Rows.Add("3", "Salary w/o Timesheet");
			dt.Rows.Add("4", "Subcontract");
			dt.Rows.Add("5", "1099");
			dt.Rows.Add("6", "Co-Op");
			dt.Rows.Add("7", "Owner/NA");
			}
		else
			{
			dt.Rows.Add("1", "Hourly");
			dt.Rows.Add("2", "Salary w/ Timesheet");
			dt.Rows.Add("4", "Subcontract");
			dt.Rows.Add("5", "1099");
			dt.Rows.Add("6", "Co-Op");
			dt.Rows.Add("7", "Owner/NA");
			}
		ddl_emptype.DataSource = _tools.getSQL_datatable(@"Select * from member_paytype"  , null);
		ddl_emptype.DataValueField = "id";
		ddl_emptype.DataTextField = "paytype";
		ddl_emptype.DataBind();
		}
	private void status_update(string text, bool is_error)
		{
//		lb_status.CssClass				= is_error ? "lb_status error" : "lb_status";
//		lb_status.EncodeHtml			= false;
//		lb_status.Text					= text;
//		ScriptManager.RegisterClientScriptBlock(this, GetType(),
//				"alertMessage", @"alert('" + text + "')", true);
			ScriptManager.RegisterStartupScript(this, this.GetType(), "alertmessage", "alert('" + text.Replace("'","") + "')", true);

		}
	private void status_update(List<string> _items, bool is_error)
		{
		lb_status.CssClass				= is_error ? "lb_status error" : "lb_status";
		lb_status.EncodeHtml			= false;
		var text				= new StringBuilder();
		
		foreach(var _item in _items)
			{
			text.AppendFormat("- {0}</br>",_item);
			}
		
		lb_status.Text					= text.ToString();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertmessage", "alert('" + text.Replace("'", "") + "')", true);

    }
	protected void populate_from_app()
	{
		var mo = new NeMemberOffer(mo_id);
		var app = new NeApplicant(mo.applicantid);
		var n_info = new NeMember(); 
		try
		{
			if(app.id > 0)
				{
			tb_firstname.Text = app.firstname;
			tb_lastname.Text = app.lastname;
			tb_nickname.Text = app.firstname;
			tb_middleinitial.Text = "";
			tb_benefits_id.Text = "";
			tb_life_insurance_id.Text = "";
			tb_payroll_id.Text = "";
			tb_city.Text = app.city;
			tb_postal.Text = app.postal;
			tb_email.Text = app.email.ToLower();
			tb_address.Text = app.address;
			lb_necell_area.Text = "";
			lb_necell_pref.Text = "";
			lb_necell_suff.Text = "";
			tb_truck_number.Text = "";
			tb_social.Text = "";
			lb_extension.Text = "";
			tb_drivers_license.Text = "";
			tb_username.Text = app.firstname.Substring(0, 1) + app.lastname.Replace(" ", "");
			tb_password.Text = tb_username.Text.Replace(" ","") + app.id;
			tb_electrical_license.Text = "";
			lb_neemail.Text = "";
			date_birth.Text = "";
			chkpart_time.Checked = false;
			set_ddl(ddl_provstate, app.province);
			set_ddl(ddl_payrollhandler, new NeBusinessUnit(mo.business_unit_id).branch_manager.id);
			set_ddl(ddl_country, app.country);
			set_ddl(ddl_reportsto, mo.reports_to.ToString());
			chk_ts_watch.Checked = true;
		
			member_photo.Style.Add("background-image", "/images/member/nophoto.png");
			member_photo.Attributes.Add("data-default_bg", "/images/member/nophoto.png");
			
			ddl_default_location.DataSource		= app.business_unit_id !=0 ? inventory.get_ext_locations(app.business_unit_id) : inventory.get_ext_locations(app.business_unit_id);
			ddl_default_location.DataBind();
			ddl_default_location.Items.Insert(0, new ListItem("Please Select Location", "0"));
			date_start.Text = mo.startdate.ToString("yyyy-MM-dd");
			date_end.Text = "2099-12-31";
			ddl_default_location.SelectedIndex = -1;
			chkpart_time.Checked = mo.part_time;
			tb_emerg_fname1.Text = "";
			tb_emerg_lname1.Text = "";
			tb_emerg1_area.Text = "";
			tb_emerg1_pref.Text = "";
			tb_emerg1_suff.Text = "";

                /*
            tb_emerg_fname2.Text = "";
			tb_emerg_lname2.Text = "";
			tb_emerg2_area.Text = "";
			tb_emerg2_pref.Text = "";
			tb_emerg2_suff.Text = "";
			*/

			ddl_status.SelectedIndex = 1;
			bt_terminate.Visible = false;
			ddl_hrstatus.Value = 1;
		//	set_ddl(ddl_hrstatus,1);
			set_ddl(ddl_branch, mo.business_unit_id);
			set_ddl(ddl_title, mo.membertypeid);
				set_ddl(ddl_emptype, mo.paytype_id==0?1:mo.paytype_id);
			
			if (app.homephone == "") { app.homephone = app.cellphone; }

			try
			{
				tb_homephone_area.Text = app.homephone.Replace("-","").Replace(" ","").Replace("(","").Replace(")","").PadRight(10,'x').Substring(0, 3);
				tb_homephone_pref.Text = app.homephone.Replace("-","").Replace(" ", "").Replace(")", "").PadRight(10, 'x').Substring(3, 3);
				tb_homephone_suff.Text = app.homephone.Replace("-","").Replace(" ", "").Replace("(", "").Replace(")", "").PadRight(10, 'x').Substring(6, 4);
			}
			catch { }

			try
			{
				tb_pecell_area.Text = app.cellphone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").PadRight(10, 'x').Substring(0, 3);
				tb_pecell_pref.Text = app.cellphone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").PadRight(10, 'x').Substring(3, 3);
				tb_pecell_suff.Text = app.cellphone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").PadRight(10, 'x').Substring(6, 4);
			}
			catch { }
				}
			else
				{
				Toolbox.FriendlyException(Response, "Applicant doesn't exist", "");
				}
			bt_save.Visible = true;

		}
		catch (Exception except)
		{
			_tools.catch_error(except);
			throw;
		}
	}
	private void populate_user(NeMember user)
		{
		using(var conn = Toolbox.connect())
			{
		ddl_provstate.DataSource			= shared.GetProvs();
		ddl_provstate.DataBind();
			    ddl_branch.DataSource = _tools.getSQL_datatable("Select id, name from business_unit order by name",null);
		ddl_branch.DataBind();
		var li							= user.id == 0 ? ddl_branch.Items.FindByValue(current_user.business_unit_id.ToString()) : ddl_branch.Items.FindByValue(user.business_unit_id.ToString());
		if(li != null)
			{
			li.Selected						= !IsPostBack || li.Selected;
			}
		ddl_branch.Enabled					= current_user.id == 711 || current_user.AuthenticatedForPrivilege(122);
		ddl_title.DataSource = new NeMemberType().LoadMemberList();
		ddl_title.DataBind();
		ddl_payrollhandler.DataSource		= Toolbox.doSQL_dt(conn, @" SELECT a.member_id id, CONCAT(b.name, ' - ',a.member_fullname) name FROM member a LEFT JOIN business_unit b on a.business_unit_id = b.id WHERE FIND_IN_SET(a.member_id, GET_POSSIBLE_PAYROLL_HANDLERS(@v0)) AND a.member_status = 'Active' AND a.member_id IN (SELECT memberpage_member_id FROM memberpage WHERE memberpage_page_id = 44) ORDER BY b.name, a.member_lastname, a.member_nickname", new object[] {  user.id } );
		ddl_payrollhandler.DataBind();
		ddl_reportsto.DataSource = Toolbox.doSQL_dt(conn, @" call get_possible_supervisors(" + user.id + ")", new object[] {  user.id } );
			    ddl_reportsto.DataBind();
            ListItem l = new ListItem("Board of Directors", "0");
            ddl_reportsto.Items.Add(l);
          
		
		li									= user.id == 0 ? ddl_payrollhandler.Items.FindByValue(current_user.business_unit_id.ToString()) : ddl_payrollhandler.Items.FindByValue(user.payroll_handler.ToString());
		var temp_selected					= li == null ? false : li.Selected;
		if(li != null)
			{
			li.Selected							= !IsPostBack || temp_selected;
			}
		fill_paytypes();
		ddl_default_location.DataSource		= user.business_unit_id != 0 ? inventory.get_ext_locations(user.business_unit.id) : inventory.get_ext_locations(current_user.business_unit.id);
		ddl_default_location.DataBind();
		ddl_default_location.Items.Insert(0, new ListItem("Please Select Location", "0"));
	
		bt_newhire.Visible					= user.id != 0 && current_user.AuthenticatedForPrivilege(120);
	//	bt_newhire.Visible = false;
		if(user.id != 0)
			{
			#region Existing User

				
				btn_payroll_verified.ClientEnabled = NeMember.to_do.exists(current_user.id, "payroll", user.id, conn);

				tb_benefits_id.Text = user.benefits_id;
				tb_life_insurance_id.Text = user.life_insurance_id;
				tb_payroll_id.Text = user.PayrollNo;
				lbl_benefits_startdate.Text = user.benefits_startdate == null ? "" : "Benefits Start Date: " + Convert.ToDateTime(user.benefits_startdate).ToString("yyyy-MM-dd");

			pc.TabPages[6].Enabled													= true;
			bt_print_barcode.Visible												= true;
		
			
			lb_id.Text																= user.id.ToString();
			tb_firstname.Text														= user.FirstName;
			tb_lastname.Text														= user.LastName;
			tb_nickname.Text														= user.Nickname;
			tb_middleinitial.Text													= user.MiddleInitial;

			tb_homephone_area.Text													= user.PhoneAreaCode;
			tb_homephone_pref.Text													= user.PhoneFirst;
			tb_homephone_suff.Text													= user.PhoneLast;
			
			tb_city.Text															= user.City;
			tb_postal.Text															= user.PostalCode;
			tb_email.Text = user.Email.ToLower();
			tb_address.Text															= user.Address;
			lb_necell_area.Text														= user.NECellPhoneArea;
			lb_necell_pref.Text														= user.NECellPhoneFirst;
			lb_necell_suff.Text														= user.NECellPhoneLast;
			tb_truck_number.Text													= user.NETruck;
			tb_social.Text															= user.SIN;
			lb_extension.Text														= user.PhoneExtension;
			tb_drivers_license.Text													= user.DriversLicence;
			tb_username.Text														= user.Username;
			tb_password.Text														= user.Password;
			tb_apprentice_contract.Text												= user.apprentice_contract;
			
			row_password.Visible													= false;
			row_username.Visible													= false;
			tb_electrical_license.Text												= user.ElectricalLicence;
			if (user.BirthDate != "")
			{
				date_birth.Date = Convert.ToDateTime(user.BirthDate);
			}
				member_photo.Style.Add("background-image", "/_tools/member_photo/index.aspx?member_id="+user.id);
			member_photo.Attributes.Add("data-default_bg", "/_tools/member_photo/index.aspx?member_id="+user.id);
			if_photo.Attributes["src"]												= "/_tools/member_photo/index.aspx?upload_form=true&member_id="+user.id;
			set_ddl(ddl_provstate, user.Prov);
			set_ddl(ddl_title, user.MemberTypeID.ToString());
			

			if (user.reports_to == 100000)
			{
				user.reports_to = Toolbox.doSQL_int(@"select ifnull((Select reports_to from member_offers  where memberid =@v0 order by enddate desc limit 1),100000)", new object[] { user.id });
			}
			if (user.reports_to == 100000)
			{
				user.reports_to = Convert.ToInt32(user.payroll_handler);
			}
			chk_can_isboardmember.Checked = user.is_CAN_boardmember;
			chk_us_isboardmember.Checked = user.is_US_boardmember;
			    chk_ts_watch.Checked = user.tswatch;
			set_ddl(ddl_reportsto, user.reports_to.ToString());
			ddl_reportsto.Enabled =		NeMember.is_supervisor(user.id,current_user.id) || 
										is_branch_hr && current_user.business_unit_id == user.business_unit_id || 
										is_depart_hr  || 
										ispayroll;
			lbl_reporting.Text = NeMember.get_reporting_line(user.id);
			ddl_title.Enabled														= current_user.MemberTypeID == 40;
			set_ddl(ddl_status, user.Status);
			if(user.Status == "Active")
				{
				ddl_status.Enabled			= false;
				}
			set_ddl(ddl_hasdependants, user.HasDependants);
			set_ddl(ddl_default_location, user.member_default_location.ToString());
			set_ddl(ddl_country, user.Country);
			
			set_ddl(ddl_emptype, user.paytype_id);

/*			    if (user.payroll_handler == 0 || (ddl_payrollhandler.Items.FindByValue(user.payroll_handler.ToString()) == null))
			        {
			        if (user.id != user.business_unit.branch_manager.id)
			            {
			            set_ddl(ddl_payrollhandler, user.business_unit.branch_manager.id);
			            }
			        else if ((ddl_payrollhandler.Items.FindByValue(user.reports_to.ToString()) != null) )
			            {
			            set_ddl(ddl_payrollhandler, user.reports_to);
			            }
			        else
			            {
			            set_ddl(ddl_payrollhandler, user.id);
			            }
			        }
			    else
			        {*/
			        set_ddl(ddl_payrollhandler, user.payroll_handler);
			       // }


			    ddl_hrstatus.Value = user.hrstatus_id;
		//	set_ddl(ddl_hrstatus, user.hrstatus_id);
			lb_neemail.Text															= user.NEEmail;
			date_start.Date															= Convert.ToDateTime(user.StartDate);

			if (user.TerminateDate == "")
			{
				date_end.Date = new DateTime(1900, 1, 1);
			}
			else
			{
				date_end.Date = Convert.ToDateTime(user.TerminateDate);
			}

		
			tb_emerg_fname1.Text													= user.EmergencyFirstName1;
			tb_emerg_lname1.Text													= user.EmergencyLastName1;
			tb_emerg1_area.Text														= user.EmergencyPhoneArea1;
			tb_emerg1_pref.Text														= user.EmergencyPhoneFirst1;
			tb_emerg1_suff.Text														= user.EmergencyPhoneLast1;
			/*tb_emerg_fname2.Text													= user.EmergencyFirstName2;
			tb_emerg_lname2.Text													= user.EmergencyLastName2;
			tb_emerg2_area.Text														= user.EmergencyPhoneArea2;
			tb_emerg2_pref.Text														= user.EmergencyPhoneFirst2;
			tb_emerg2_suff.Text														= user.EmergencyPhoneLast2;
            */
			tb_pecell_area.Text														= user.pecell_area;
			tb_pecell_pref.Text														= user.pecell_pref;
			tb_pecell_suff.Text														= user.pecell_suff;
			tb_notes.Text															= user.empnotes;
			chkpart_time.Checked = user.part_time;
			var memberwage_c														= Toolbox.doSQL_int(conn,@"SELECT COUNT(memberwage_id) FROM memberwage WHERE memberwage_memberid = @v0 ", new object[] {  user.id } );
			var memberwage														= user.id == 0 
																							? 0 
																							: memberwage_c > 0 
																								? Toolbox.doSQL_double(conn,@"SELECT currentwage FROM memberwage WHERE memberwage_memberid = @v0  ORDER BY Date DESC LIMIT 1", new object[] {  user.id } )
																								: 0;
			
			var if_type			= "";
			var mem_type	= new NeMemberType(current_user.MemberTypeID);

			if(issupervisor || current_user.AuthenticatedForPrivilege(156))
				{
				if_type				= "direct";
				}
			else if(current_user.id == 1295 || current_user.id == 8 || current_user.id == 711)
				{
				if_type				= "sysadmin";
				}
			else if(current_user.AuthenticatedForPage(44)&&current_user.business_unit_id == 11)
				{
				if_type				= "payroll";
				}
			if_termination.Attributes["src"]										= "./if_termination.aspx?id="+user.id+"&type="+if_type;


			personal_details.Visible = current_user.AuthenticatedForPrivilege(37);
		
			// set up file directory
			#region populate file tab
			hidMemberID.Value = user.id.ToString();
			div_files.InnerHtml = "Files for : " + user.FullName2 + System.Environment.NewLine;
			div_files.InnerHtml += "<iframe src='/filemanager.aspx?parent_page=member_files&id=" + hidMemberID.Value + "' frameborder='no' width='100%' height='500px' scrolling='auto'></iframe>";
			#endregion
			#region populate priv

			ddlcompany.DataSource = _tools.getSQL_datatable(string.Format(@"Select id, ddl_name name from business_unit  where id in ({0}) order by ddl_name", 
				new Current_User().visible_business_units),null);

			ddlcompany.DataTextField = "name";
			ddlcompany.DataValueField = "id";
			ddlcompany.DataBind();
			var li1 = new ListItem("All Business Units", "99");
			ddlcompany.Items.Add(new ListItem(li1.Text, li1.Value));
			ddlcompany.SelectedValue = user.business_unit_id.ToString();
			lblPrivileges.Text = user.FullName + " is a membertype: " + Toolbox.doSQL_string(conn,@"Select membertype_name from membertype  where membertype_id =@v0", new object[] { user.MemberTypeID }) + ". The highlighted rows indicate where " + user.Nickname + " deviates from their membertype default settings.";

			ddlview.Items.Clear();
			ddlview.ClearSelection();
			ddlview.Items.Add(new ListItem("View membertype templates",  "0"));
			ddlview.Items.Add(new ListItem("View " + user.FullName, "1"));
			ddlview.SelectedIndex = 1;
	//		GeneratePrivileges();

			#endregion
			#region wage tab
			if (can_see_wage)
			{
				wage_tab.user = user;
				wage_tab._id = user.id32;
                wage_tab.populate();	
			}
			else
			{
				pc.TabPages[2].ClientEnabled = false;
				pc.TabPages[2].ClientVisible = false;
			}
			#endregion
            #region it tab
            if (can_see_it)
            {
                it_tab.user = user;
				it_tab._id = user.id32;
                it_tab.populate();
            }
            else
            {
                pc.TabPages[1].ClientEnabled = false;
                pc.TabPages[1].ClientVisible = false;
            }
            #endregion
            #region days off
            //pnlView.Visible = false;
			hidMemberID.Value = user.id.ToString();
		//	hidCompanyID.Value = user.business_unit_id.ToString();
			//Bindgv_daysoff(0);
			#endregion
			#region usage tab
		
			uc_member_usage1.DataBind();
			#endregion
			BindDiscipGrid();
			hr_status.Visible = (current_user.business_unit_id == 109 || current_user.business_unit_id == 11 || current_user.id == 8);
			if(user.Status == "Active")
				{

				if (user.reports_to == current_user.id || can_term_all)
				{
					bt_terminate.Visible = true;
				}
				}
			else
				{
				bt_print_barcode.Visible											= false;
                // Added the ability for Payroll to see Termination Tab when user is not active and they have the privileges - Ticket no. 4494
                if (issupervisor || can_term_all || user.Status == "Not Active" && current_user.AuthenticatedForPage(44) && current_user.business_unit_id == 11)
				{
					pc.TabPages[7].ClientEnabled = true;
					pc.TabPages[7].ClientVisible = true;
			
				}
				bt_terminate.Visible			= false;
				}
			#endregion Existing User
			}
		else // if its a new user
			{
			pc.TabPages[6].Enabled				= false;
			// Set up test data
			if (mo_id == 0)
			{
				tb_benefits_id.Text = "";
				tb_life_insurance_id.Text = "";
				lbl_benefits_startdate.Text = "";
				tb_payroll_id.Text = "";
				tb_firstname.Text = "";
				tb_lastname.Text = "";
				tb_nickname.Text = "";
				tb_middleinitial.Text = "";
				tb_homephone_area.Text = "";
				tb_homephone_pref.Text = "";
				tb_homephone_suff.Text = "";
				tb_city.Text = "";
				tb_postal.Text = "";
				tb_email.Text = "";
				tb_address.Text = "";
				lb_necell_area.Text = "";
				lb_necell_pref.Text = "";
				lb_necell_suff.Text = "";
				tb_truck_number.Text = "";
				tb_social.Text = "";
				lb_extension.Text = "";
				tb_drivers_license.Text = "";
				tb_username.Text = "";
				tb_password.Text = "";
				tb_electrical_license.Text = "";
				lb_neemail.Text = "";
				date_birth.Text = "";
				ddl_provstate.Items.FindByValue(current_user.Prov).Selected = true;
				ddl_country.ClearSelection();
				set_ddl(ddl_payrollhandler, current_user.id);
				ddl_country.Items.FindByValue(current_user.Country).Selected = true;
				
				member_photo.Style.Add("background-image", "/images/member/nophoto.png");
				member_photo.Attributes.Add("data-default_bg", "/images/member/nophoto.png");
				
				date_start.Text = "";
				date_end.Text = "2099-12-31";
				ddl_default_location.SelectedIndex = -1;
				tb_emerg_fname1.Text = "";
				tb_emerg_lname1.Text = "";
				tb_emerg1_area.Text = "";
				tb_emerg1_pref.Text = "";
				tb_emerg1_suff.Text = "";
				/*tb_emerg_fname2.Text = "";
				tb_emerg_lname2.Text = "";
				tb_emerg2_area.Text = "";
				tb_emerg2_pref.Text = "";
				tb_emerg2_suff.Text = "";*/
				ddl_status.SelectedIndex = 1;
				bt_terminate.Visible = false;
				chk_ts_watch.Checked = false;
			}
			else
			{
				populate_from_app();
			}
			}
		ddl_title_SelectedIndexChanged(ddl_title, null);
			}
		}
	private void set_ddl(DropDownList ddl, object value)
		{
		if(value != null)
			{
			var _value		= value.ToString();
			ddl.ClearSelection();
			if(ddl.Items.FindByValue(_value) != null)
				{
				ddl.Items.FindByValue(_value).Selected		= true;
				}
			else if(ddl.Items.Count > 0)
				{
				    if (ddl.ID == "ddl_payrollhandler")
				        {
				        ddl.DataBind();
				        ListItem li = new ListItem(user.FullName, _value);
				        ddl.Items.Add(li);
				        ddl.Items.FindByValue(_value).Selected = true;
				        }
				    else
				        {
				        ddl.ClearSelection();
				        }
				    //		ddl.SelectedIndex							= -1;
                }
			}
		else
			{
			    ddl.ClearSelection();
            //	ddl.SelectedIndex							= -1;
        }
		}
	protected void button_new_Click(object sender, EventArgs e)
		{
		populate_user(new NeMember());
		}
	#region days off tab
	/*
	protected void drpLeaveType_SelectedIndexChanged(object sender, EventArgs e)
	{
		//pnlView.Visible = false;
		Bindgv_daysoff(Convert.ToInt16(drpLeaveType.SelectedValue));
	}
	protected void gv_daysoff_SelectedIndexChanged(object sender, EventArgs e)
		{
		ClearTextBoxes();
		if(gv_daysoff.SelectedIndex >= 0)
			{
			btnupdatedays.Visible = true;
			btnAddNeDayoff.Visible = false;
			var mdo_id = (gv_daysoff.Rows[gv_daysoff.SelectedIndex].Cells[0]).Text;
			NeMemberDaysOff mdo = new NeMemberDaysOff(mdo_id);
			ddl_dayofftype.SelectedValue = mdo.leavetype_id.ToString();
			ddl_dayofftype.Enabled = false;
			ddl_requesttype.SelectedValue = mdo.requesttype_id.ToString();
			ddl_requesttype.Enabled = false;
			if(mdo.date_requested_start == "")
				{
				mdo.date_requested_start	= Toolbox.MySQL_shortdt(Convert.ToDateTime(mdo.date_requested));
				}
			if(mdo.date_requested_end == "")
				{
				mdo.date_requested_end		= Toolbox.MySQL_shortdt(Convert.ToDateTime(mdo.date_requested));
				}
			txtStartDate.Date = Convert.ToDateTime(mdo.date_requested_start);
			txtEnddate.Date = Convert.ToDateTime(mdo.date_requested_end);
			tb_daysoff_comments.Text = mdo.comments;
			tb_daysoff_days.Text = mdo.days.ToString();
			tb_daysoff_hours.Text = mdo.hours.ToString();
			panel_update.Visible = true;
			}
		}

	protected void btnUpdatedays_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
		var mdo_id					= (gv_daysoff.Rows[gv_daysoff.SelectedIndex].Cells[0]).Text;
		var mdo						= new NeMemberDaysOff(mdo_id);

		mdo.date_requested_start	= txtStartDate.Date.ToString("yyyy-MM-dd");
		mdo.date_requested_end		= txtEnddate.Date.ToString("yyyy-MM-dd");
		mdo.hours					= Convert.ToDouble(tb_daysoff_hours.Text);
		mdo.days					= Convert.ToDouble(tb_daysoff_days.Text);
		mdo.comments				= tb_daysoff_comments.Text;
		try
			{
			mdo.save();
			gv_daysoff.SelectRow(-1);
			panel_update.Visible = false;
			}
		catch (Exception ex)
			{
			literror.Text = "The following Error occured  :- " + ex.Message;
			}
		finally
			{
			}
		Complete(1);
		}

	protected void btnCanceldays_Click(object sender, System.Web.UI.ImageClickEventArgs e)
	{
		//pnlView.Visible = false;
		gv_daysoff.SelectRow(-1);
		ddl_dayofftype.SelectedValue = "0";
		ddl_requesttype.SelectedValue = "0";
		ClearTextBoxes();
		Bindgv_daysoff(0);
			panel_update.Visible = false;
	}
	*/
	protected void btn_printbarcode_Click(object sender, EventArgs e)
		{
		try
			{
			NeMember _member;
			_member = new NeMember(Convert.ToInt32(hidMemberID.Value));
			_member.print_barcode_label(1);
			errorlabel.Text = "";
			}
		catch
			{

			errorlabel.Text = "Error printing bar code";
			}
		}
	#endregion
	#region privileges
	protected void load_membertype_ddl()
	{


		DataTable dt2;
		if (ddlcompany.SelectedValue != "99")
		{
			dt2 = _tools.getSQL_datatable(@"Select member_fullname, membertype_name, ddl_name from member,business_unit,membertype  where member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 and member_membertype_id =@v1  order by name, member_firstname", new object[] { ddlcompany.SelectedValue,ddlMemberType.SelectedValue });
		}
		else
		{
			dt2 = _tools.getSQL_datatable(@"Select member_fullname, membertype_name, ddl_name from member,business_unit,membertype  where member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and member_membertype_id =@v0 order by name, member_firstname", new object[] { ddlMemberType.SelectedValue });
		}
		var _list_of_members2 = "<table>";
		var company2 = "";
		foreach (DataRow dr2 in dt2.Rows)
		{
			if (dr2[2].ToString() != company2)
			{
				company2 = dr2[2].ToString();
				_list_of_members2 += "<tr><td>" + dr2[2] + "</td>";
			}
			else
			{
				_list_of_members2 += "<tr><td></td>";
			}
			_list_of_members2 += "<td>" + dr2[0] + "</td><td>" + dr2[1] + "</td></tr>";
		}
		_list_of_members2 += "</table>";



		divmembertype.InnerHtml = "<span width: '200px'; class='opt1' data-tooltip='" + _list_of_members2 + "'> Members of this Member Type (put mouse here) </span>";


	}
	protected void GeneratePrivileges()
		{
		var sb		= new StringBuilder();
		GenerateGlobalPrivileges(ref sb, true);
		GenerateReportEnginePrivileges(ref sb);
		sb.Append("<div style='font-weight:bold;font-size:20px;text-decoration:underline;'>Member Page Privileges</div>");
		var memEdit = new NeMember(Convert.ToInt32(id));
		var TypePriv = new NEMemberTypePrivilege();
		var strPageSQL = "";
		var strPrivSQL = "";


		TypePriv.TypePages(Convert.ToInt32(memEdit.MemberTypeID.ToString()));
		TypePriv.TypePrivileges(Convert.ToInt32(memEdit.MemberTypeID.ToString()));

		var dsPriv = new DataSet();
		var dtPage = Toolbox.doSQL_dt(@"SELECT page_id,page_parent_id,page_name,page_desc,page_scriptpath,page_order FROM page"  , null);
		dtPage.TableName = "Page";
		var dtPriv = Toolbox.doSQL_dt(@"SELECT privilege_id,privilege_page_id,privilege_name,privilege_desc FROM privilege"  , null);
		dtPriv.TableName = "Privilege";
		dsPriv.Tables.Add(dtPage);
		dsPriv.Tables.Add(dtPriv);
		//string strMap = "\r<ul>";
		GetChildren(0, ref sb, ref dsPriv, ref memEdit, ref TypePriv);
		divSiteMap.InnerHtml = sb.ToString();

		/*string strMemberSQL = "SELECT Member_FirstName,Member_LastName FROM Member"
							+ " WHERE  Member_ID =" + hidMemberID.Value;
		OdbcCommand comMember = new OdbcCommand(strMemberSQL, conn);
		OdbcDataReader drMember = comMember.ExecuteReader();
		drMember.Read();
		spanMemberName.InnerHtml = drMember["Member_FirstName"].ToString() + " " + drMember["Member_LastName"].ToString();*/
		

		}
	private void GenerateReportEnginePrivileges(ref StringBuilder sb)
		{
		sb.Append("<div style='font-weight:bold;font-size:20px;text-decoration:underline;'>Report Privileges</div><ul>");
		var links		= Toolbox.doSQL_dt(@"SELECT a.name category, b.name report_name, b.id, IFNULL(c.id,0) link_id, IF(c.id IS NULL, 0, 1) chked FROM r_engine_category a LEFT JOIN r_engine_report b ON b.r_engine_category = a.id LEFT JOIN r_engine_permission c on c.r_engine_report = b.id AND c.member = @v0  ORDER BY a.name,b.name", new object[] {  id } );
		var last_category		= "";
		foreach(DataRow dr in links.Rows)
			{
			var this_id			= dr["id"];
			var category		= (string) dr["category"];
			var report_name		= dr["report_name"];
			var this_link_id	= dr["link_id"];
			var this_chked		= (int) dr["chked"] == 1 ? "checked" : "";
			if(category != last_category)
				{
				if(last_category != "")
					{
					sb.Append("</ul>");
					}
				sb.AppendFormat("<li><b>{0}</b><ul>", category);
				last_category	= category;
				}
			sb.AppendFormat(@"
			<li class='reportenginepriv' style=""list-style-image: url('/images/pixel.gif'); display: list-item;"">
				<input type='checkbox' value='{0}' id='!reportenginepriv__{0}_{3}' name='!reportenginepriv_{0}_{3}' {2} />
				<label for='!reportenginepriv_{0}_{3}'><b>{1}</b></label>
			</li>", 
				this_id, 	   // {0}
				report_name,    // {1}
				this_chked,    // {2}
				this_link_id   // {3}
				);
			}
		sb.Append("</ul></ul>");
		}
	private void GenerateGlobalPrivileges(ref StringBuilder sb, bool is_member)
		{
		sb.Append("<div style='font-weight:bold;font-size:20px;text-decoration:underline;'>Global Privileges</div><ul>");
		var	link_type		= is_member ? 1 : 2;
		var links		= Toolbox.doSQL_dt(@"SELECT a.id, IFNULL(b.id,0) link_id, a.name, a.description, IF(user_id IS NULL, 0, 1) chked FROM privilege_global a LEFT JOIN privilege_global_link b on b.global_priv_id = a.id AND b.type_id = @v0  AND b.user_id = @v1 ", new object[] {  link_type, id } );
		foreach(DataRow dr in links.Rows)
			{
			var this_id			= dr["id"];
			var this_name		= dr["name"];
			var this_desc		= dr["description"];
			var this_link_id	= dr["link_id"];
			var this_chked	= Convert.ToInt16(dr["chked"]) == 1 ? "checked" : "";
			sb.AppendFormat(@"<li class='global_priv' style=""list-style-image: url('/images/pixel.gif'); display: list-item;""><input type='checkbox' value='{0}' id='!globalpriv_{0}_{4}' name='!globalpriv_{0}_{4}' {3} /><label for='!globalpriv_{0}_{4}'>({0}) <b>{1}</b> <i>{2}</i></label></li>", this_id, this_name, this_desc, this_chked, this_link_id);
			}
		sb.Append("</ul>");
		}
	private void GetChildren(int parent_id, ref StringBuilder sb, ref DataSet ds, ref NeMember memEdit, ref NEMemberTypePrivilege TypePriv)
		{
		sb.Append("<ul>");

		foreach (DataRow row in ds.Tables[0].Rows)
			{
			var page_id			= Convert.ToInt32(row["page_id"]);
			var page_name	= row["page_name"].ToString();
			var page_desc	= row["page_desc"].ToString();
			var page_parent_id	= Convert.ToInt32(row["page_parent_id"]);

			if (parent_id == page_parent_id)
				{
				var strChecked = "";

				//Nic: sep 1st, 2011
				var strStyle = "";

				// check to see if page is in mem object
				if (memEdit.AuthenticatedForPage(page_id))
					{
					strChecked = "CHECKED";
					}

				//Nic: sep 1st, 2011
				if (TypePriv.AuthenticatedForTypePage(page_id))
					{
					strStyle = strChecked != "CHECKED" ? "style='background-color:#F4FA58;'" : "";
					}
				else
					{
					strStyle = strChecked == "CHECKED" ? "style='background-color:#F4FA58;'" : "";
					}

				DataTable dt;

				if (ddlcompany.SelectedValue != "99")
					{
					dt = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, ddl_name from member,memberpage,business_unit,membertype  where member.member_id = memberpage_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 and memberpage_page_id =@v1  order by name, member_firstname", new object[] { ddlcompany.SelectedValue,page_id });
					}
				else
					{
					dt = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, ddl_name from member,memberpage,business_unit,membertype  where member.member_id = memberpage_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and memberpage_page_id =@v0 order by name, member_firstname", new object[] { page_id });
					}

				var _list_of_members = new StringBuilder();
				_list_of_members.Append(@"<table width='100%' style='align:left;'>");
				var name = "";
				foreach (DataRow dr in dt.Rows)
					{
					var member_name			= (string) dr["name"];
					var membertype_name		= (string) dr["membertype_name"];
					var this_name	= (string) dr["ddl_name"];
					if (this_name != name)
						{
						name = this_name;
						_list_of_members.AppendFormat(@"<tr><td colspan='2' align='left'><b>{0}</b></td></tr>", name);
						}
					_list_of_members.AppendFormat(@"<tr><td>{0}</td><td>{1}</td></tr>", member_name, membertype_name);
					}
				_list_of_members.Append(@"</table>");

				var chkname		= "!page_" + page_id + "_" + parent_id;
				var onclick		= ds.Tables["Privilege"].Select("privilege_page_id = "+page_id).Any() || ds.Tables["Page"].Select("page_parent_id = "+page_id).Any()
										? @"data-expanded='false' style=""list-style-image:url('/images/icon/icon[minus].gif');"" onclick='toggle_children(this, event)'"
										: @" style=""list-style-image:url('/images/pixel.gif');{0}"" ";
				sb.AppendFormat(@"
<li class='page' {6}>
		<span class='opt1' data-title='FALSE' {7} data-tooltip=""{0}""><input type='checkbox' id='{1}' name='{1}' value='1' {2} onclick='javascript:testChecked(this.id);'><label for='{1}'><b> ({3}) {4}:</b> <i style='font-size:11px;color:#777;'>{5}</i></label></span>", 
				_list_of_members, 
				chkname, 
				strChecked, 
				page_id, 
				page_name, 
				page_desc,
				onclick,
				strStyle
				);
				if(ds.Tables["Privilege"].Rows.Count > 0)
					{
					sb.Append("<ul>");
					}

				foreach (DataRow prow in ds.Tables["Privilege"].Rows)
					{
					#region privileges
					var privilege_id				= Convert.ToInt32(prow["privilege_id"]);
					var privilege_name			= prow["privilege_name"].ToString();
					var privilege_description	= prow["privilege_desc"].ToString();
					var privilege_page_id			= Convert.ToInt32(prow["privilege_page_id"]);
					if (privilege_page_id == page_id)
						{
						var strCheckedp = "";

						var strStylep = "";

						if (memEdit.AuthenticatedForPrivilege(privilege_id))
							{
							strCheckedp = "CHECKED";
							}

						if (TypePriv.AuthenticatedForTypePrivilege(privilege_id))
							{
							strStylep			= strCheckedp != "CHECKED" ? "background-color:#F4FA58;" : "";
							}
						else
							{
							strStylep			= strCheckedp == "CHECKED" ? "background-color:#F4FA58;" : "";
							}

						DataTable dt2;
						if (ddlcompany.SelectedValue != "99")
							{
							dt2 = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, ddl_name from member,memberpageprivilege,business_unit,membertype  where member.member_id = memberpageprivilege_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 and memberpageprivilege_privilege_id =@v1  order by name, member_firstname", new object[] { ddlcompany.SelectedValue,privilege_id });
							}
						else
							{
							dt2 = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, ddl_name from member,memberpageprivilege,business_unit,membertype  where member.member_id = memberpageprivilege_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and memberpageprivilege_privilege_id =@v0 order by name, member_firstname", new object[] { privilege_id });
							}	
						var _list_of_members2 = new StringBuilder();
						_list_of_members2.Append(@"<table width='100%' style='align:left;'>");
						var sub_name = "";
						foreach (DataRow dr2 in dt2.Rows)
							{
							var member_name			= (string) dr2["name"];
							var membertype_name		= (string) dr2["membertype_name"];
							var this_name	= (string) dr2["name"];
							if (this_name != sub_name)
								{
								sub_name = this_name;
								_list_of_members2.AppendFormat(@"<tr><td colspan='2' align='left'><b>{0}</b></td></tr>", sub_name);
								}
							_list_of_members2.AppendFormat(@"<tr><td>{0}</td><td>{1}</td></tr>", member_name, membertype_name);
							}
						_list_of_members2.Append(@"</table>");
						var chknamep = string.Format(@"!priv_{0}_{1}", privilege_id, page_id);
						
						sb.AppendFormat(@"
<li class='priv opt1' style=""list-style-image:url('/images/pixel.gif');{6}""  data-title='FALSE' data-tooltip=""{0}""><input type='checkbox' id='{1}' name='{1}' value='1' {2} onclick='javascript:testChecked(this.id);'><label for='{1}'><b>({5}) {3}</b> <i style='font-size:11px;color:#777;'>{4}</i></label>", 
						_list_of_members2, 
						chknamep, 
						strCheckedp, 
						privilege_name, 
						privilege_description,
						privilege_id,
						strStylep
						);
						}
					#endregion privileges
					}
				if(ds.Tables["Privilege"].Rows.Count > 0)
					{
					sb.Append("</ul>");
					}
				GetChildren(page_id, ref sb, ref ds, ref memEdit, ref TypePriv);
				sb.Append("</li>");
				}
			//  strConcat += "</ul>";
			}
		sb.Append("</ul>");
		}
	protected void rptPages_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		var item = e.Item;
		if (item.ItemType == ListItemType.Item ||
			item.ItemType == ListItemType.AlternatingItem)
		{
			var rptPrivileges = (Repeater)item.FindControl("rptPrivileges");

			var page = (NePage)item.DataItem;
			rptPrivileges.DataSource = page.Privileges;
			rptPrivileges.DataBind();
		}
	}
	protected void btnEditPriv_Click(object sender, EventArgs e)
	{
		var upa			= new NEUserPage();
		var upr		= new NEUserPrivilege();
		var user			= new NeMember(id);
		NEUserPrivilege.clear(current_user, user, 1);
		NEUserPage.clear(current_user, user, 1);
		NeGlobalPrivilegeLink.clear(user.id, 1);
		report_engine.permission.clear(user.id);
		// loop form data        
		foreach (string key in Request.Form.Keys)
			{
			if(key.StartsWith("!"))
				{
			//debug.InnerHtml += " key" + key;
			var arr = key.Split('_');
			var item_type		= arr[0];
			if(item_type == "!globalpriv")
				{
				var gp_priv_id				= Convert.ToInt32(arr[1]);
				var gp_link_id				= Convert.ToInt32(arr[2]);
				var gpl	= gp_link_id == 0 
												? new NeGlobalPrivilegeLink() 
												: new NeGlobalPrivilegeLink(gp_link_id);
				gpl.global_priv_id			= gp_priv_id;
				gpl.type_id					= 1;
				gpl.user_id					= Convert.ToInt32(id);
				gpl.save();
				}
			else if(item_type == "!reportenginepriv")
				{
				var rep_id			= Convert.ToInt32(arr[1]);
				if (rep_id <= 0)
					{
					continue;
					}
				var rep = new report_engine.permission
							{
								member = user.id,
								r_engine_report = rep_id
							};
				rep.save();
				}
			else if (item_type == "!page" && Request.Form[key] == "1")
				{
				var page_id		= Convert.ToInt32(arr[1]);
				upa = new NEUserPage
						{
							admin = current_user,
							user = user,
							user_id = user.id,
							page_id = page_id,
							type_id = 1
						};
				upa.save();

				//now get privileges for this page
				foreach (string keyp in Request.Form.Keys)
					{
					var arrp = keyp.Split('_');
					var sub_item_type	= arrp[0];
					if (sub_item_type == "!priv" && arrp[2] == arr[1] && Request.Form[keyp] == "1")
						{
						var priv_id			= Convert.ToInt32(arrp[1]);
						upr					= new NEUserPrivilege();
						upr.admin			= current_user;
						upr.user			= user;
						upr.user_id			= Convert.ToInt32(user.id);
						upr.privilege_id	= priv_id;
						upr.typepage_id		= Convert.ToInt32(upa.id);
						upr.type_id			= 1;
						upr.save();
						}
					}
				}
			}
			}
		lblError.Text = "Successfully edited Privileges";


		try
			{

			send_mail("Set");

			}
		catch { }

		GeneratePrivileges();
	}
	protected void lbPriv_Click(object sender, EventArgs e)
	{
		ddlMemberType.Visible = true;
		var type = new NeMemberType();
		var typelist = new ArrayList();

		typelist = type.LoadMemberList();
		ddlMemberType.DataSource = typelist;
		ddlMemberType.DataBind();

		load_membertype_ddl();
		btnEditPriv.Visible = false;
		btnReset.Visible = false;
		btnMbrTypPriv.Visible = true;
		chkCascadeChanges.Visible = true;
		lblPrivileges.Visible = false;


		GenerateTypePrivileges();
	}
	protected void ddlMemberType_SelectedIndexChanged(object sender, EventArgs e)
	{
		lblPrivileges.Visible = false;

		load_membertype_ddl();
		GenerateTypePrivileges();
	}
	protected void GenerateTypePrivileges()
		{
		var TypePriv = new NEMemberTypePrivilege();
		TypePriv.TypePages(Convert.ToInt32(ddlMemberType.SelectedValue));
		TypePriv.TypePrivileges(Convert.ToInt32(ddlMemberType.SelectedValue));

		var dsTypePriv = new DataSet();
		var dtPage = Toolbox.doSQL_dt(@"SELECT page_id,page_parent_id,page_name,page_desc,page_scriptpath,page_order FROM page"  , null);
		dtPage.TableName = "Page";
		var dtPriv = Toolbox.doSQL_dt(@"SELECT privilege_id,privilege_page_id,privilege_name,privilege_desc FROM privilege"  , null);
		dtPriv.TableName = "Privilege";
		dsTypePriv.Tables.Add(dtPage);
		dsTypePriv.Tables.Add(dtPriv);
		var sb		= new StringBuilder();
		GenerateGlobalPrivileges(ref sb, false);
		sb.Append("<div style='font-weight:bold;font-size:20px;text-decoration:underline;'>Membertype Page Privileges</div>");
		GetTypeChildren(0, ref sb, ref dsTypePriv, ref TypePriv);

		divSiteMap.InnerHtml = sb.ToString();

		}
	private void GetTypeChildren(int parent_id, ref StringBuilder sb, ref DataSet ds, ref NEMemberTypePrivilege TypePriv)
		{
		sb.Append("\n<ul>");
		foreach (DataRow row in ds.Tables[0].Rows)
			{
			var page_id			= Convert.ToInt32(row["page_id"]);
			var page_name	= row["page_name"].ToString();
			var page_desc	= row["page_desc"].ToString();
			var page_parent_id	= Convert.ToInt32(row["page_parent_id"]);
			if (parent_id == page_parent_id)
				{
				var strChecked = "";
				// check to see if page is in mem object
				if (TypePriv.AuthenticatedForTypePage(page_id))
					{
					strChecked = "CHECKED";
					}

				DataTable dt;
				if (ddlcompany.SelectedValue != "99")
					{
					dt = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, ddl_name from member,memberpage,business_unit,membertype  where member.member_id = memberpage_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 and memberpage_page_id =@v1  order by name, member_firstname", new object[] { ddlcompany.SelectedValue,page_id });
					}
				else
					{
					dt = _tools.getSQL_datatable(@"Select member_fullname name, membertype_name, ddl_name from member,memberpage,business_unit,membertype  where member.member_id = memberpage_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and memberpage_page_id =@v0 order by name, member_firstname", new object[] { page_id });
					} 
				var _list_of_members = new StringBuilder();
				_list_of_members.Append(@"<table width='100%'>");
				var name = "";
				foreach (DataRow dr in dt.Rows)
					{
					var member_name			= (string) dr["name"];
					var membertype_name		= (string) dr["membertype_name"];
					var this_name	= (string) dr["ddl_name"];
					if (this_name != name)
						{
						name = this_name;
						_list_of_members.AppendFormat(@"<tr><td colspan='2' align='left'><b>{0}</b></td></tr>", name);
						}
					_list_of_members.AppendFormat(@"<tr><td>{0}</td><td>{1}</td></tr>", member_name, membertype_name);
					}
				_list_of_members.Append(@"</table>");

				var chkname		= "!page_" + page_id + "_" + parent_id;
				var onclick		= ds.Tables["Privilege"].Select("privilege_page_id = "+page_id).Count() > 0 || ds.Tables["Page"].Select("page_parent_id = "+page_id).Count() > 0
										? @"data-expanded='false' style=""list-style-image:url('/images/icon/icon[minus].gif')"" onclick='toggle_children(this, event)'" 
										: @" style=""list-style-image:url('/images/pixel.gif')"" ";
				sb.AppendFormat(@"
<li class='page' {6}>
		<span class='opt1' data-title='FALSE' data-tooltip=""{0}""><input type='checkbox' id='{1}' name='{1}' value='1' {2} onclick='javascript:testChecked(this.id);'><label for='{1}'><b> ({3}) {4}:</b> <i style='font-size:11px;color:#777;'>{5}</i></label></span>", 
				_list_of_members, 
				chkname, 
				strChecked, 
				page_id, 
				page_name, 
				page_desc,
				onclick);
				if(ds.Tables["Privilege"].Rows.Count > 0)
					{
					sb.Append("<ul>");
					}
				foreach (DataRow prow in ds.Tables["Privilege"].Rows)
					{
					#region privileges
					var privilege_id				= Convert.ToInt32(prow["privilege_id"]);
					var privilege_name			= prow["privilege_name"].ToString();
					var privilege_description	= prow["privilege_desc"].ToString();
					var privilege_page_id			= Convert.ToInt32(prow["privilege_page_id"]);
					if (privilege_page_id == page_id)
						{

						DataTable dt2;
						if (ddlcompany.SelectedValue != "99")
							{
							dt2 = _tools.getSQL_datatable(@"SELECT member_fullname name, membertype_name, ddl_name from member,memberpageprivilege,business_unit,membertype  where member.member_id = memberpageprivilege_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 and memberpageprivilege_privilege_id =@v1  order by name, member_firstname", new object[] { ddlcompany.SelectedValue,prow["Privilege_ID"] });
							}
						else
							{
							dt2 = _tools.getSQL_datatable(@"SELECT member_fullname name, membertype_name, ddl_name from member,memberpageprivilege,business_unit,membertype  where member.member_id = memberpageprivilege_member_id and member.member_status = 'Active' and member_membertype_id = membertype_id and business_unit_id = business_unit.id and memberpageprivilege_privilege_id =@v0 order by name, member_firstname", new object[] { privilege_id });
							}
						var _list_of_members2 = new StringBuilder();
						_list_of_members2.Append(@"<table width='100%'>");
						var sub_name = "";
						foreach (DataRow dr2 in dt2.Rows)
							{
							var member_name			= (string) dr2["name"];
							var membertype_name		= (string) dr2["membertype_name"];
							var this_name	= (string) dr2["ddl_name"];
							if (this_name != sub_name)
								{
								sub_name = this_name;
								_list_of_members2.AppendFormat(@"<tr><td colspan='2' align='left'><b>{0}</b></td></tr>", sub_name);
								}
							_list_of_members2.AppendFormat(@"<tr><td>{0}</td><td>{1}</td></tr>", member_name, membertype_name);
							}
						_list_of_members2.Append(@"</table>");

						var strCheckedp = "";
						// check to see if page is in mem object
						if (TypePriv.AuthenticatedForTypePrivilege(privilege_id))
							{
							strCheckedp = "CHECKED";
							}
						var chknamep = string.Format(@"!priv_{0}_{1}", privilege_id, page_id);
						sb.AppendFormat(@"
<li class='priv opt1' style=""list-style-image:url('/images/pixel.gif')""  data-title='FALSE' data-tooltip=""{0}""><input type='checkbox' id='{1}' name='{1}' value='1' {2} onclick='javascript:testChecked(this.id);'><label for='{1}'><b>({5}) {3}</b> <i style='font-size:11px;color:#777;'>{4}</i></label>", 
						_list_of_members2, 
						chknamep, 
						strCheckedp, 
						privilege_name, 
						privilege_description,
						privilege_id
						);
						}
					#endregion privileges
					}
				if(ds.Tables["Privilege"].Rows.Count > 0)
					{
					sb.Append("</ul>");
					}
				GetTypeChildren(page_id, ref sb, ref ds, ref TypePriv);
				sb.Append("</li>");
				}
			}
		sb.Append("</ul>");
		}
	protected void btnMbrTypPriv_Click(object sender, EventArgs e)
		{
		var strMessage = "";
		var selected_type_id			= ddlMemberType.SelectedValue;
		var mt					= new NeMemberType(selected_type_id);
		try
			{
			// delete old data before inserts
			Toolbox.doSQL_void(@"DELETE FROM membertypepageprivilege WHERE membertypepageprivilege_type_id = @v0 ", new object[] {  selected_type_id } );
			Toolbox.doSQL_void(@"DELETE FROM membertypepage WHERE membertypepage_type_id = @v0 ", new object[] {  selected_type_id } );

			// loop form data        
			foreach (string key in Request.Form.Keys)
				{
				//debug.InnerHtml += " key" + key;
				var arr	= key.Split('_');
				var type		= arr[0];
				var page_id	= arr.Length > 1 ? arr[1] : "";
				if(type == "!globalpriv")
					{
					var gp_priv_id				= Convert.ToInt32(arr[1]);
					var gp_link_id				= Convert.ToInt32(arr[2]);
					var gpl	= gp_link_id == 0 
													? new NeGlobalPrivilegeLink() 
													: new NeGlobalPrivilegeLink(gp_link_id);
					gpl.global_priv_id			= gp_priv_id;
					gpl.type_id					= 2;
					gpl.save();
					}
				else if (type == "!page" && Request.Form[key] == "1")
					{
					var membertypepage_id		= Toolbox.doSQL_return_id(@" INSERT INTO membertypepage ( membertypepage_type_id, membertypepage_page_id ) VALUES ( @v0 , @v1  )", new object[] {  selected_type_id, page_id  } );

					//now get privileges for this page
					foreach (string keyp in Request.Form.Keys)
						{
						if(keyp.Contains("!"))
							{
							var arrp		= keyp.Split('_');
							var sub_type		= arrp[0];
							var privilege_id	= arrp.Length > 1 ? arrp[1] : ""; 
							var sub_page_id	= arrp.Length > 2 ? arrp[2] : "";
							if(sub_type == "!priv")
								{
								var a = arrp;
								}
							if (sub_type == "!priv" && sub_page_id == page_id && Request.Form[keyp] == "1")
								{
								Toolbox.doSQL_void(@" INSERT INTO membertypepageprivilege ( membertypepageprivilege_membertypepage_id, membertypepageprivilege_privilege_id, membertypepageprivilege_type_id ) VALUES ( @v0 , @v1 , @v2  )", new object[] {  membertypepage_id, privilege_id, selected_type_id  } );
								}
							}
						}
					}
				}
			shared.alert_it("Member Type Privilege Change", "Member Type ("+mt.name+") Privileges have been changed by "+current_user.FullName);
			}
		catch (Exception ee)
			{
			throw ee;
			}

		if (chkCascadeChanges.Checked)
			{
			var cascade = new NEMemberTypePrivilege();
			strMessage = cascade.CascadeMemberChanges(selected_type_id);
			}

		GenerateTypePrivileges();
		lblError.Text = "Successfully edited Type Privileges" + " " + strMessage;
		}
	protected void btnReset_Click(object sender, EventArgs e)
	{
		var TypePriv = new NEMemberTypePrivilege();
		TypePriv.SetMemberPrivileges(hidMemberID.Value);
		GeneratePrivileges();
		try
		{
			send_mail("Reset");
		}
		catch { }

	}
	protected void send_mail(string msg)
	{
		
		var _tools = new Toolbox();
		var mail = new NeEMail();
		mail.To = "igalbraith@" + Toolbox.app_setting("DomainForEmail");
		mail.Subject = "A Privilege Change Has been Made: Privilege=" + msg;
		mail.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
		var message = "Privileges were changed by ";
		try
		{
			message += current_user.FullName2;
		}
		catch { }
		try
		{
			message += " for the following member " + _tools.getSQL_string(@"SELECT member_fullname FROM member WHERE member_id = @v0  LIMIT 1", new object[] {  id } );
		}
		catch { }

		mail.Body = message;
		mail.Send();
	}
	protected void ddlcompany_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ddlview.SelectedValue == "1")
		{
			GeneratePrivileges();
		}
		else
		{
			GenerateTypePrivileges();
			load_membertype_ddl();
		}

	}
	protected void ddlview_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ddlview.SelectedValue == "0")
		{

			ddlMemberType.Visible = true;
			var type = new NeMemberType();
			var typelist = new ArrayList();

			typelist = type.LoadMemberList();
			ddlMemberType.DataSource = typelist;
			ddlMemberType.DataBind();

			load_membertype_ddl();

			btnEditPriv.Visible = false;
			btnReset.Visible = false;
			btnMbrTypPriv.Visible = true;
			chkCascadeChanges.Visible = true;
			lblPrivileges.Visible = false;
			GenerateTypePrivileges();
			divmembertype.Visible = true;

		}
		else
		{
			//pnlView.Visible = false;
			var editing_member   = new NeMember(Convert.ToInt32(hidMemberID.Value));
			var membertype_name	  = Toolbox.doSQL_string(@"SELECT membertype_name FROM membertype WHERE membertype_id = @v0 ", new object[] { editing_member.MemberTypeID } );
			lblPrivileges.Visible	  = true;
			lblPrivileges.Text        = string.Format("<b>{0}</b> is a membertype: <b>{1}</b>. The highlighted rows indicate where <b>{2}</b> deviates from their membertype default settings.", 
				editing_member.FullName, 	 // {0}
				membertype_name, 			 // {1}
				editing_member.Nickname		 // {2}
				);
			ddlMemberType.Visible     = false;
			divmembertype.Visible     = false;
			chkCascadeChanges.Visible = false;
			btnMbrTypPriv.Visible     = false;
			btnEditPriv.Visible       = true;
			btnReset.Visible          = true;
			GeneratePrivileges();
		}
	}
	#endregion
	#region discipline tab
	protected void BindDiscipGrid()
	{
		
		discipline_tab1._id = Convert.ToInt32(id);
		discipline_tab1._addedid = current_user.id32;
		discipline_tab1.populate();

	}


		#endregion
	protected void gv_offers_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
	{

		var frame = (HtmlContainerControl)gv_offers.FindEditFormTemplateControl("IFrame_Offer");
		if (!gv_offers.IsNewRowEditing)  // are we editting here?
		{
			var rowIndex = gv_offers.EditingRowVisibleIndex;
			var value1 = gv_offers.GetRowValues(rowIndex, new string[] { "id" });
			frame.Attributes.Add("src", "member_offer.aspx?id=" + value1);
		}
		else
		{
			Session["offerid"] = null;
			frame.Attributes.Add("src", "member_offer.aspx?memberid=" + hdnmember.Value);
		}
	//	frame.Attributes.Add("onload", "resizeIframe(this);");
	//	frame.Attributes.Add("onload", "resizeIframe(this);");
	}
	/*
	protected void Bindgv_daysoff(int ltype)
		{
		gv_daysoff.                                                                   DataSource = dto_grid(id, Convert.ToInt32(ltype.ToString()));
		gv_daysoff.DataBind();
		}
	*/
	protected void ddl_branch_SelectedIndexChanged(object sender, EventArgs e)
		{
		var ddl								= (DropDownList) sender;
		ddl_default_location.DataSource		= inventory.get_ext_locations(Convert.ToInt32(ddl.SelectedValue));
		ddl_default_location.DataBind();
		    ddl_default_location.Items.Insert(0, new ListItem("Please Select Location", "0"));
        // Auto change payroll handler
        var payroll_handler					= 0;
		
		using(var conn = Toolbox.connect())
			{
            // Check if there is a BM
			var bm							= Toolbox.doSQL_dt(conn,@"SELECT member_id FROM member WHERE business_unit_id = @v0  AND member_membertype_id = '5' AND member_status = 'Active'", new object[] {  ddl.SelectedValue } );
			if(bm.Rows.Count == 0)
				{
				// If not, use the "reports to" (R2) chain based on the R2's ability to view the payroll approval page
				var reports_to				= ddl_reportsto.SelectedValue;
				if(reports_to == null)
					{
					// Use the get_bm routine in MySQL, which will utilize the org chart
					payroll_handler			= Toolbox.doSQL_int(conn,@"SELECT GET_BM(@v0 )", new object[] {  ddl.SelectedValue } );
					}
				else
					{
					var reports_to_stack	= NeMember.get_supervisors(user.id);
					var users_with_approval	= Toolbox.doSQL_dt(conn,@"SELECT memberpage_member_id id FROM memberpage WHERE memberpage_page_id = 44" , null);
					foreach(DataRow dr in reports_to_stack.Rows)
						{
						var id			= Convert.ToInt32(dr["memberid"]);
						if(!users_with_approval.Select("id = "+id).Any()) continue;
						payroll_handler		= id;
						break;
						}
					}
				}
			else
				{
				payroll_handler				= Convert.ToInt32(bm.Rows[0]["member_id"]);
				}
			if(ddl_payrollhandler.Items.FindByValue(payroll_handler.ToString()) != null)
				{
				ddl_payrollhandler.SelectedValue	= payroll_handler.ToString();
				}
			else
				{
				throw new Exception("Could not automatically set payroll handler");
				}
			}
		}
	protected void gv_reviewitem_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		var gv = (ASPxGridView)sender;
	//	if (e.RowType != GridViewRowType.Data) return;
		if (e.VisibleIndex >= 0)
		{
			if (Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "groupid")) % 2 == 0)
			{
				e.Row.BackColor = System.Drawing.Color.LightCoral;
			}
			else
			{
				e.Row.BackColor = System.Drawing.Color.LightBlue;
			}
		}

	}
	protected void gv_offers_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		gv_offers.DataBind();
		Session["offerid"] = null;

	}
	protected void gv_offers_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		var status = new NeMemberOffer(Convert.ToInt32(e.Keys[0])).status;
		if (e.Values["is_signed"].ToString() == "1")
		{
			throw new Exception("You can not delete offers that have already been signed");
		}
		if (status != "In Development" && status != "Expired" && status != "Closed")
		{
			throw new Exception("You can not delete offers unless they are of the status 'In Development'");
		}
		_tools.getSQL_void(@"Delete from member_offers  where id =@v0", new object[] { e.Keys[0] });
		gv_offers.CancelEdit();
		e.Cancel = true;
		gv_offers.DataBind();
		Session["offerid"] = null;
		
	}
	protected void gv_review_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (e.DataColumn.Caption == "Score")
			{
				e.Cell.Text = _tools.getSQL_double(@"select ifnull((SELECT avg(emp_review_history.score) FROM emp_review_history  where ifnull(emp_review_history.score,0)>0 and emp_review_id=@v0),0) ", new object[] { gv_review.GetRowValues(e.VisibleIndex, "id") }).ToString("N1");
			}
			

		}
		
	}
	protected void gv_review_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		Session["reviewid"] = null;
		gv_review.DataBind();
	}
	protected void gv_review_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var frame = (HtmlContainerControl)gv_review.FindEditFormTemplateControl("IFrame_review");
		if (!gv_review.IsNewRowEditing)  // are we editting here?
		{
			var rowIndex = gv_review.EditingRowVisibleIndex;
			var value1 = gv_review.GetRowValues(rowIndex, new string[] { "id" });
			frame.Attributes.Add("src", "review_list_for_member.aspx?id=" + value1 + "&memberid=" + hdnmember.Value);
		}
		else
		{
			frame.Attributes.Add("src", "review_list_for_member.aspx?id=0&memberid=" + hdnmember.Value);
		}
	//	frame.Attributes.Add("onload", "resizeIframe(this);");
		gv_review.SettingsText.PopupEditFormCaption = "Review for " + new NeMember(Convert.ToInt32(hdnmember.Value)).FullName;
	}
	protected void gv_review_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		new NeEmpReview().delete(Convert.ToInt32(e.Keys[0]));
	

		e.Cancel = true;
		gv_review.CancelEdit();
		gv_review.DataBind();
	}
	protected void gv_review_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
	//	if (e.VisibleIndex >= 0)
	//	{
			if (gv_review.GetRowValues(e.VisibleIndex, "locked").ToString() == "1")
			{
				if (e.ButtonType == DevExpress.Web.ColumnCommandButtonType.Delete)
					e.Visible = false;
				
			}


	//	}
	}
	protected void gv_review_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		gv_review.SettingsText.PopupEditFormCaption = "Review for " + new NeMember(Convert.ToInt32(hdnmember.Value)).FullName;
	}
	protected void ASPxButton1_Click(object sender, EventArgs e)
	{
		var appid = _tools.getSQL_int(@"select ifnull((Select id from applicants  where becomes_memberid =@v0 limit 1),0) ", new object[] { hdnmember.Value });
		if (appid != 0)
		{
			var f = new NeFiles();
			f.copy_applicant_to_member(appid, Convert.ToInt32(hdnmember.Value));
		}
	}
	protected void gv_offers_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		if (gv_offers.GetRowValues(e.VisibleIndex, "status").ToString() == "Accepted" || gv_offers.GetRowValues(e.VisibleIndex, "status").ToString() == "Released" || gv_offers.GetRowValues(e.VisibleIndex, "status").ToString() == "Awaiting Start Date" || gv_offers.GetRowValues(e.VisibleIndex, "status").ToString() == "Previous")
		{
			if (e.ButtonType == ColumnCommandButtonType.Delete)
			{
				e.Visible = false;
			}
		}
	}
	protected void cb_user_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		if(e.Parameter.Contains("|"))
			{
			var paras			= e.Parameter.Split('|');
			var this_type		= paras[0];
			var this_id			= paras[1];
			switch(this_type)
				{
				case "a":
					Toolbox.doSQL_void(@"INSERT INTO user_switch (member_id, mapped_member_id) VALUES (@v0 , @v1 )", new object[] {  hidMemberID.Value, this_id } );
				break;
				case "r":
					Toolbox.doSQL_void(@"DELETE FROM user_switch WHERE member_id = @v0  AND mapped_member_id = @v1 ", new object[] {  hidMemberID.Value, this_id } );
				break;
				}
			}
		}
	protected void lb_userswitch_selected_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		lb_userswitch_selected.DataBind();
		}
	protected void lb_userswitch_available_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		lb_userswitch_available.DataBind();
		}
	protected void cb_reset_todo_Callback(object source, DevExpress.Web.CallbackEventArgs e)
	{
		_tools.getSQL_void(@"Delete from member_todo_list_helper  where type=@v0 and type_id =@v1 ", new object[] { e.Parameter,hdnmember.Value });
//		switch (e.Parameter)
//		{
//			case "payroll":
//				btn_payroll_verified.ClientEnabled = false;
//				break;
//
//		
//		}
			
	}
	protected void ddl_hrstatus_Load(object sender, EventArgs e)
	{

		ddl_hrstatus.JSProperties["cp_set"] = ddl_hrstatus.Value==null?"1":ddl_hrstatus.Value.ToString();
	}
	protected void pc_ActiveTabChanged(object source, TabControlEventArgs e)
	{
		if (e.Tab.Text.Equals("Privileges"))
		{
			GeneratePrivileges();

		}

	}
	protected void gv_offers_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		//if(e.Parameters.Contains("|"))
		//	{
		//	var paras			= e.Parameters.Split('|');
		//	var action			= paras[0];
		//	if(action == "switch_active")
		//		{
		//		int active_offer_id;
		//		int.TryParse(paras[1], out active_offer_id);
		//		if(active_offer_id > 0)
		//			{
		//			user.active_offer_id = active_offer_id;
		//			}
		//		}
		//	}
		//gv_offers.DataBind();
		}
}
	

	