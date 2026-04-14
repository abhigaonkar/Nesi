using System;
using System.Collections.Generic;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Web.Services;
using System.Text;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class sections_hr_i_index : System.Web.UI.Page
	{
	Toolbox _tools;
	NeMember current_user;
	emp_checks chks							= new emp_checks();
	emp_checks chks_tmp						= new emp_checks();
	private const int _page_id			= 1; // from Page table in DB
	int this_user_id						= 0;
	NeMember this_user						= new NeMember();
	private const string _page_description	= "FVR";
	member_fvr_hdr hdr;
	member_fvr_dtl dtl;
	bool is_manager							= false;
	bool is_manager_set						= false;
	NameValueCollection _q;
	DataTable dt_steps						= Toolbox.doSQL_dt(@"SELECT type, tab_index, IF(name LIKE '%(%' AND tab_index = 4, 'New Hire Acknowledgement', name) name FROM member_fvr_tab"  , null);
	int active_step							= 0;
	bool is_mobile							= false;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools				= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(_page_id);
		if(!is_manager_set)
			{
			is_manager		= current_user.AuthenticatedForPrivilege(138);
			is_manager_set	= true;
			}
		_q					= Request.QueryString;
		is_mobile			= !string.IsNullOrEmpty(_q["is_mobile"]) && _q["is_mobile"] == "1";
		if(is_mobile)
			{
			mobile_header_tags.Visible = true;
			}
		mobile_css.Visible	= is_mobile;
		chks_tmp.populate_session(Page, ref chks_tmp);
		if(string.IsNullOrEmpty(_q["id"]))
			{
			Toolbox.FriendlyException(Response, "Invalid attempt", "javascript:window.close()");
			}
		else
			{
			var id					= Convert.ToInt32(_q["id"]);
			hdr						= new member_fvr_hdr(id);
			Session["fvr_hdr"]		= hdr;
			}
		if(string.IsNullOrEmpty(_q["member_id"]))
			{
			this_user				= current_user;
			this_user_id			= current_user.id;
			}
		else if(is_manager)
			{
			this_user_id			= Convert.ToInt32(_q["member_id"]);
			this_user				= new NeMember(this_user_id);
			lb_as_user.InnerHtml	= string.Format("<b>Viewing an FVR for: {0}</b>", this_user.FullName);
			}
		var hist		= new member_fvr_history();
		var dt				= hist.history(hdr.id, this_user.id);
		if(hdr.type	== member_fvr_hdr.types.NEWHIRE)
			{
			#region NEWHIRE
			foreach (DataRow dr in dt.Rows)
				{
				var tab_index			= Convert.ToInt32(dr["tab_index"]);
				var chked				= Convert.ToBoolean(dr["confirmed"]);
				switch(tab_index)
					{
					case 0:
						chks.new_welcome				= chked;
					break;
					case 1:
						chks.new_information			= chked;
					break;
					case 2:
						chks.new_handbook				= chked;
					break;
					case 3:
						chks.new_president				= chked;
					break;
					case 4:
						chks.new_hire_acknowledgement	= chked;
					break;
					case 5:
						chks.new_truck					= chked;
					break;
					case 6:
						chks.new_contact_list			= chked;
					break;
					case 7:
						chks.new_confidentiality		= chked;
					break;
					case 8:
						chks.new_qualifications			= chked;
					break;
					case 9:
						chks.new_training				= chked;
					break;
					case 10:
						chks.new_finish					= chked;
					break;
					}
				}
			#endregion NEWHIRE
			}
		else if(hdr.type == member_fvr_hdr.types.RENEW)
			{
			#region RENEW
			foreach(DataRow dr in dt.Rows)
				{
				var tab_index			= Convert.ToInt32(dr["tab_index"]);
				var chked				= Convert.ToBoolean(dr["confirmed"]);
				switch(tab_index)
					{
					case 0:
						chks.renew_welcome			= chked;
					break;
					case 1:
						chks.renew_information			= chked;
					break;
					case 2:
						chks.renew_handbook				= chked;
					break;
					case 3:
						chks.renew_survey				= chked;
					break;
					case 4:
						chks.renew_president				= chked;
					break;
					case 5:
						chks.renew_truck					= chked;
					break;
					case 6:
						chks.renew_contact_list			= chked;
					break;
					case 7:
						chks.renew_confidentiality		= chked;
					break;
					case 8:
						chks.renew_qualifications			= chked;
					break;
					case 9:
						chks.renew_trademarks				= chked;
					break;
					case 10:
						chks.renew_code				= chked;
					break;
					case 11:
						chks.renew_safety				= chked;
					break;
					case 12:
						chks.renew_training				= chked;
					break;
					case 13:
						chks.renew_finish				= chked;
					break;
					}
				}
			#endregion RENEW
			}
			if(!string.IsNullOrEmpty(_q["s"]) && int.TryParse(_q["s"], out active_step))
				{
 				active_step					= Convert.ToInt32(_q["s"]);
				}
		if(!IsPostBack)
			{
			chks.active_step	= active_step;
			}
		Session["emp_checks"]			= chks;
		#region commented
		//string csv_tabs					= "";
		//if(get_tabs.Rows.Count == 0)
		//	{
		//	Toolbox.FriendlyException(Response, "This is not an active FVR", "/default.aspx");
		//	}
		//foreach(DataRow dr in get_tabs.Rows)
		//	{
		//	int t		= (int) dr["tab_index"];
		//	csv_tabs += t + ",";
		//	}
		//csv_tabs		= csv_tabs.TrimEnd(',');
		//List<int> shown_tabs			= Array.ConvertAll(csv_tabs.Split(','), s => int.Parse(s)).ToList();
		//if(!IsPostBack)
		//	{
		//	if(active_step != 0 && active_step != 99)
		//		{
		//		chks.active_step		= shown_tabs.IndexOf(active_step);
		//		}
		//	else
		//		{
		//		chks.active_step		= chks.active_step == 0 ? 0 : shown_tabs.Count - 1;
		//		}
		//	}
		//else
		//	{
		////	chks.active_step		= 0;
		//	}
		//csv_tabs					= "";
		//for(int i = 0;i < steps.Count; i++)
		//	{
		//	csv_tabs			+= i+",";
		//	}
		//csv_tabs					= csv_tabs.TrimEnd(',');
		//List<int> available_tabs	= Array.ConvertAll(csv_tabs.Split(','), int.Parse).ToList();
		//List<int> missing			= available_tabs.Except(shown_tabs).ToList();
		//missing.Reverse();
		//Wizard wiz					= new Wizard();
		//switch(hdr.type)
		//	{
		//	//case "NEWHIRE":
		//	//	wiz					= wiz_new;
		//	//break;
		//	//case "RENEW":
		//	//	wiz					= wiz_renew;
		//	//break;
		//	//case "EXIT":
		//	//	wiz					= wiz_exit;
		//	//break;
		//	}
		//foreach(int tab in missing)
		//	{
		//	wiz.WizardSteps.RemoveAt(tab);
		//	}
		if(!IsPostBack)
			{
			//if(wiz_exit.Visible)
			//	{
			//	wiz_exit.ActiveStepIndex	= chks.active_step;
			//	wiz_exit_ActiveStepChanged(wiz_exit, null);
			//	}
			//else if(wiz_new.Visible)
			//	{
			//	wiz_new.ActiveStepIndex		= chks.active_step;
			//	wiz_new_ActiveStepChanged(wiz_new, null);
			//	}
			//else if(wiz_renew.Visible)
			//	{
			//	wiz_renew.ActiveStepIndex		= chks.active_step;
			//	wiz_renew_ActiveStepChanged(wiz_renew, null);
			//	}
			}
		#endregion commented
		if(hdr.type == "NEWHIRE" && string.IsNullOrEmpty(_q["v"]))
			{
				populate_categories();
				wiz_new.Visible = true;


			}
		else
			{
			populate_categories();
			switch(hdr.type)
				{
				case "NEWHIRE":
					wiz_new.Visible			= true;
				break;
				case "RENEW":
					wiz_renew.Visible		= true;
				break;
				}
			}
		}
	protected void Page_Load()
		{
		chks.populate_session(Page, ref chks);
		}
	private void populate_categories()
		{
		var dt				= member_fvr_hdr.get_fvrs(this_user.id, hdr.id);
		var cats			= new StringBuilder();
		var t_index					= 0;
		var index_set				= false;
		var hist		= new member_fvr_history();
		if(dt.Rows.Count > 0)
			{
		foreach(DataRow dr in dt.Rows)
			{
			var tab_index			= Convert.ToInt32(dr["tab_index_pseudo"]);
			var css_class		= tab_index == active_step || (active_step == 0 && t_index == 0 && !index_set) 
										? "sidestep selected" 
										: "sidestep available" ;
			if(active_step == 0 && !index_set)
				{
				active_step = tab_index;
				index_set = true;
				}
			var id					= Convert.ToInt32(dr["detail_id"]);
			var confirmed			= Convert.ToInt32(dr["confirmed"]);
			var upload_required		= Convert.ToInt32(dr["upload_required"]);
			t_index					= tab_index;
			var name				= dr["name"].ToString();
			if(css_class == "sidestep selected")
				{
				dtl					= new member_fvr_dtl(id);
				hist				= new member_fvr_history(id);
				populate_step(tab_index, name);
				}
			if(dtl == null)
				{
				//Toolbox.FriendlyException(Response, "This FVR has already been verified, or does not exist.", "/home.aspx");
				}
			var used_image		= confirmed == 0
										? "/images/pixel.gif" 
										: "/images/icon/icon[ok].gif";
			var upload_div		= upload_required == 0 ? "" : "<div class='up_req'><img src='/images/icon/icon[save].gif' width='10' height='10' align='absmiddle' /> Upload Required</div>";
			var used_url			= tab_index == active_step ? "" : string.Format(@"onclick=""location.href='./index.aspx?v=1&id={0}&s={1}&is_mobile={2}';""", hdr.id, tab_index, is_mobile?1:0);
			cats.AppendFormat(@"
	<div class='{2}' {3}>
		<table cellspacing='0' cellpadding='0' width='100%'>
			<tr>
				<td align='center' rowspan='2' valign='middle'><img src='{1}' width='16' height='16' align='absmiddle' /></td>
				<td align='left'>{0}</td>
			</tr>
			<tr>
				<td align='left'>{4}</td>
			</tr>
		</table>
	</div>", name, used_image, css_class, used_url, upload_div);
			}
				var show_uploadpanel						= dtl.upload_required == 1 && hist.file_uploaded == 0;
				var show_filepanel							= dtl.upload_required == 1 && hist.file_uploaded == 1;
				var	f										= dtl.uploaded_file_id > 0 ? new file_store.fileObj(dtl.uploaded_file_id) : new file_store.fileObj();
		switch(hdr.type)
			{
			case "NEWHIRE":
				wiz_new_categories.InnerHtml	= cats.ToString();
				var new_html					= string.Format(@"
<b>Uploaded file</b> <br/>
<button onclick=""boing('/_tools/get_file/index.aspx?file_id={0}', 'uploadedfile', 640,480);"" class='whitetext aligncenter' type='button'><img src='/images/icon/icon[popup].gif' align='absmiddle' /> <b>{1}.{2}</b></button><br/><br/>
<button type='button' data-id='{3}' onclick='fvr.client.remove_file.run(this)' class='whitetext aligncenter'><img src='/images/icon/icon[delete].gif' align='absmiddle' />Delete</button>", dtl.uploaded_file_id, f.name, f.ext, dtl.id);
				if(is_mobile)
					{
					new_if_upload_mobile.Attributes["src"]				= "./modules/upload.aspx?dtl_id="+dtl.id;
					new_pnl_upload_mobile.Visible						= show_uploadpanel;
					new_pnl_showfile_mobile.Visible					= show_filepanel;
					if(show_filepanel)
						{
						new_pnl_showfile_html_mobile.InnerHtml	= new_html;
						}
					}
				else
					{
					new_if_upload_desktop.Attributes["src"]				= "./modules/upload.aspx?dtl_id="+dtl.id;
					new_pnl_upload_desktop.Visible						= show_uploadpanel;
					new_pnl_showfile_desktop.Visible					= show_filepanel;
					if(show_filepanel)
						{
						new_pnl_showfile_html_desktop.InnerHtml	= new_html;
						}
					}
			break;
			case "RENEW":
				var renew_html								= string.Format(@"
<b>Uploaded file</b> <br/>
<button onclick=""boing('/_tools/get_file/index.aspx?file_id={0}', 'uploadedfile', 640,480);"" class='whitetext aligncenter' type='button'><img src='/images/icon/icon[popup].gif' align='absmiddle' /> <b>{1}.{2}</b></button><br/><br/>
<button type='button' data-id='{3}' onclick='fvr.client.remove_file.run(this)' class='whitetext aligncenter'><img src='/images/icon/icon[delete].gif' align='absmiddle' />Delete</button>", dtl.uploaded_file_id, f.name, f.ext, dtl.id);
				wiz_renew_categories.InnerHtml	= cats.ToString();
				if (is_mobile)
					{
					renew_if_upload_mobile.Attributes["src"]			= "./modules/upload.aspx?dtl_id="+dtl.id;
					renew_pnl_upload_mobile.Visible						= show_uploadpanel;
					renew_pnl_showfile_mobile.Visible					= show_filepanel;
					if(show_filepanel)
						{
						renew_pnl_showfile_html_mobile.InnerHtml	= renew_html;
						}
					}
				else
					{
					renew_if_upload_desktop.Attributes["src"]			= "./modules/upload.aspx?dtl_id="+dtl.id;
					renew_pnl_upload_desktop.Visible					= show_uploadpanel;
					renew_pnl_showfile_desktop.Visible					= show_filepanel;
					if(show_filepanel)
						{
						renew_pnl_showfile_html_desktop.InnerHtml	= renew_html;
						}
					}
			break;
			}
			}
		else
			{
			Toolbox.FriendlyException(Response, "Unable to access FVR information.", "window.close();");
			}
		}
	private void populate_step(int actual_index, string name)
		{
		var t				= new sections_hr_i_modules_template();
		var ei	= new sections_hr_i_modules_employee_information();
		switch(hdr.type)
			{
			case "NEWHIRE":
				NEWHIRE_header.InnerText				= name;
				switch(actual_index)
					{
					case 0: // Welcome
						t		= NEWHIRE_welcome;
					break;
					case 1: // Employee Information
						ei		= NEWHIRE_information;
					break;
					case 2: // Employee Handbook
						t		= NEWHIRE_handbook;
					break;
					case 3: // Message From President
						t		= NEWHIRE_president;
					break;
					case 4:		// New Hire Acknowledgement (US-NC)
								// New Hire Acknowledgement (US-MI)
								// New Hire Acknowledgement (CAN-ON)
						t		= NEWHIRE_hire_acknowledgement;
					break;
					case 5: // Truck
						t		= NEWHIRE_truck;
					break;
					case 6: // Contact List
						t		= NEWHIRE_contacts;
					break;
					case 7: // Confidentiality Agreement
						t		= NEWHIRE_confidentiality;
					break;
					case 8: // Statement of Qualifications
						t		= NEWHIRE_qualifications;
					break;
					case 9: // Training
						t		= NEWHIRE_training;
					break;
					case 10: // Finish
						t		= NEWHIRE_finish;
					break;
					case 11: // Marketing
					break;
					}
			break;
			case "RENEW":
				switch(actual_index)
					{
					case 0: //Welcome
						t		= RENEW_welcome;
					break;
					case 1: //Employee Information
						ei		= RENEW_information;
					break;
					case 2: //Employee Handbook
						t		= RENEW_emp_handbook;
					break;
					case 3: //Survey
						t		= RENEW_survey;
					break;
					case 4: //Message from President
						t		= RENEW_message_from_president;
					break;
					case 5: //Benefits
						t		= RENEW_truck;
					break;
					case 6: //Contact List
						t		= RENEW_contact_list;
					break;
					case 7: //Confidentiality Agreement
						t		= RENEW_confidentiality_agreement;
					break;
					case 8: //Statement of Qualifications
						t		= RENEW_qualifications_stmt;
					break;
					case 9: //Trademarks
						t		= RENEW_trademarks;
					break;
					case 10: //Codes of Conduct
						t		= RENEW_code;
					break;
					case 11: //Safety Manual
						t		= RENEW_safety;
					break;
					case 12: //Training
						t		= RENEW_training;
					break;
					case 13: //Finish
						t		= RENEW_finish;
					break;
					}
			break;
			}
		if(!string.IsNullOrEmpty(ei.ID))
			{
			ei.Visible		= true;
			ei.dtl			= dtl;
			ei.current_user	= this_user;
			ei.populate(dtl.id);
			}
		else if(!string.IsNullOrEmpty(t.ID))
			{
			t.Visible		= true;
			t.dtl			= dtl;
			t.current_user	= this_user;
			t.populate(dtl.id);
			}
		}

    protected void bt_refresh_click(object sender, EventArgs e)
		{
		populate_categories();
		handle_next_step();
		}
	protected void handle_next_step()
		{
		chks						= (emp_checks) HttpContext.Current.Session["emp_checks"];
		var str_steps			= Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(b.tab_index)
FROM member_fvr_dtl a LEFT JOIN member_fvr_tab b ON a.tab_index = b.id WHERE a.active = 1 AND a.member_id = @v0 AND a.member_fvr_hdr_id = @v1 ORDER BY b.tab_index",
			new object[] { this_user.id, hdr.id}).Split(',');
		var int_steps			= new List<int>();
		foreach(var s in str_steps)
			{
			int_steps.Add(Convert.ToInt32(s));
			}
		var step_index				= int_steps.IndexOf(active_step);
		// Should it goto the next step or finish?
		var all_steps_completed	= Toolbox.doSQL_int(@"SELECT COUNT(b.tab_index) 
FROM member_fvr_dtl a LEFT JOIN member_fvr_tab b ON a.tab_index = b.id LEFT JOIN member_fvr_history c ON a.id = c.member_fvr_dtl_id
WHERE a.active = 1 AND a.member_id = @v0 AND a.member_fvr_hdr_id = @v1 AND IFNULL(c.confirmed,0) = 0", new object[] { this_user.id, hdr.id}) == 0;
		// Was the latest change an unchecking of an acknowledgement?
		var last_confirmation		= Toolbox.doSQL_int(@"SELECT IFNULL(c.confirmed, 0) FROM member_fvr_dtl a LEFT JOIN member_fvr_tab b ON a.tab_index = b.id LEFT
JOIN member_fvr_history c ON a.id = c.member_fvr_dtl_id WHERE a.active = 1 AND
a.member_id = @v0 AND a.member_fvr_hdr_id = @v1 ORDER BY c.ts DESC LIMIT 1", new object[] { this_user.id, hdr.id});
		if(int_steps.Count > 1 && step_index < int_steps.Count - 1 && last_confirmation == 1 && !all_steps_completed)
			{
			// Next
			var next_s				= int_steps[step_index + 1];
			Response.Redirect(string.Format("./index.aspx?v=1&id={0}&s={1}&is_mobile={2}", hdr.id, next_s, is_mobile?1:0));
			}
		else if(int_steps.Count > 1 && step_index > 0 && last_confirmation == 1 && !all_steps_completed)
			{
			// Prev -- Treat this as if the user had manually clicked the last step, approved it, it should now go to the prior step
			var next_s				= int_steps[step_index - 1];
			Response.Redirect(string.Format("./index.aspx?v=1&id={0}&s={1}&is_mobile={2}", hdr.id, next_s, is_mobile?1:0));
			}
		else if(all_steps_completed)
			{
			// Finish
			}
		}
		#region commented
	//protected void wiz_new_ActiveStepChanged(object sender, EventArgs e)
	//	{
	//	if(hdr == null && Session["fvr_hdr"] != null)
	//		{
	//		hdr				= (member_fvr_hdr) Session["fvr_hdr"];
	//		_tools				= new Toolbox();
	//		current_user		= Toolbox.do_handle_authentication(_page_id);
	//		}
	//	if(hdr != null)
	//		{
	//		chks.populate_session(Page, ref chks);
	//		chks.active_step				= wiz_new.ActiveStepIndex;
	//		Session["emp_checks"]			= chks;
	//		Button b						= new Button();
	//		if(wiz_new.FindControl("FinishNavigationTemplateContainerID") != null)
	//			{
	//			if(wiz_new.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton") != null)
	//				{
	//				b	= (Button) wiz_new.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton");
	//				}
	//			}
	//
	//		switch (wiz_new.ActiveStep.ID)
	//			{
	//			case "NEWHIRE_welcome_step":
	//				// 0
	//				wiz_new.HeaderText		= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 0")[0]["name"];
	//				dtl						= new member_fvr_dtl(hdr.id, 0, this_user.id);
	//				NEWHIRE_welcome.dtl				= dtl;
	//				NEWHIRE_welcome.current_user	= this_user;
	//				NEWHIRE_welcome.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			case "NEWHIRE_information_step":
	//				// 1
	//				wiz_new.HeaderText		= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 1")[0]["name"];
	//				dtl						= new member_fvr_dtl(hdr.id, 1, this_user.id);
	//				NEWHIRE_information.dtl			= dtl;
	//				NEWHIRE_information.current_user	= this_user;
	//				NEWHIRE_information.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			case "NEWHIRE_handbook_step":
	//				// 2
	//				wiz_new.HeaderText			= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 2")[0]["name"];
	//				dtl							= new member_fvr_dtl(hdr.id, 2, this_user.id);
	//				NEWHIRE_handbook.dtl			= dtl;
	//				NEWHIRE_handbook.current_user	= this_user;
	//				NEWHIRE_handbook.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			case "NEWHIRE_president_step":
	//				// 3
	//				wiz_new.HeaderText					= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 3")[0]["name"];
	//				dtl									= new member_fvr_dtl(hdr.id, 3, this_user.id);
	//				NEWHIRE_president.dtl			= dtl;
	//				NEWHIRE_president.current_user	= this_user;
	//				NEWHIRE_president.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			case "NEWHIRE_hire_acknowledgement_step":
	//				// 4
	//				wiz_new.HeaderText			= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 4")[0]["name"];
	//				dtl							= new member_fvr_dtl(hdr.id, 4, this_user.id);
	//				NEWHIRE_hire_acknowledgement.dtl			= dtl;
	//				NEWHIRE_hire_acknowledgement.current_user	= this_user;
	//				NEWHIRE_hire_acknowledgement.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			case "NEWHIRE_truck_step":
	//				// 5
	//				wiz_new.HeaderText		= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 5")[0]["name"];
	//				dtl						= new member_fvr_dtl(hdr.id, 5, this_user.id);
	//				NEWHIRE_truck.dtl			= dtl;
	//				NEWHIRE_truck.current_user	= this_user;
	//				NEWHIRE_truck.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			case "NEWHIRE_contacts_step":
	//				// 6
	//				wiz_new.HeaderText		= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 6")[0]["name"];
	//				dtl						= new member_fvr_dtl(hdr.id, 6, this_user.id);
	//				NEWHIRE_contacts.dtl			= dtl;
	//				NEWHIRE_contacts.current_user	= this_user;
	//				NEWHIRE_contacts.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			case "NEWHIRE_confidentiality_step":
	//				// 7
	//				wiz_new.HeaderText						= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 7")[0]["name"];
	//				dtl										= new member_fvr_dtl(hdr.id, 7, this_user.id);
	//				NEWHIRE_confidentiality.dtl			= dtl;
	//				NEWHIRE_confidentiality.current_user	= this_user;
	//				NEWHIRE_confidentiality.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			case "NEWHIRE_qualifications_step":
	//				// 8
	//				wiz_new.HeaderText					= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 8")[0]["name"];
	//				dtl									= new member_fvr_dtl(hdr.id, 8, this_user.id);
	//				NEWHIRE_qualifications.dtl				= dtl;
	//				NEWHIRE_qualifications.current_user	= this_user;
	//				NEWHIRE_qualifications.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			case "NEWHIRE_training_step":
	//				// 9
	//				wiz_new.HeaderText		= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 9")[0]["name"];
	//				dtl						= new member_fvr_dtl(hdr.id, 9, this_user.id);
	//				NEWHIRE_training.dtl				= dtl;
	//				NEWHIRE_training.current_user		= this_user;
	//				NEWHIRE_training.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			case "NEWHIRE_finish_step":
	//				// 10
	//				wiz_new.HeaderText		= (string) dt_steps.Select("type = 'NEWHIRE' AND tab_index = 10")[0]["name"];
	//				dtl						= new member_fvr_dtl(hdr.id, 10, this_user.id);
	//				NEWHIRE_finish.dtl				= dtl;
	//				NEWHIRE_finish.current_user		= this_user;
	//				NEWHIRE_finish.populate(dtl.id);
	//				if(!dtl.is_complete())
	//					{
	//					b.Style.Add("display", "none");
	//					}
	//				else
	//					{
	//					b.Style.Remove("display");
	//					}
	//			break;
	//			default:
	//			break;
	//			}
	//		}
	//	}
	//protected void wiz_renew_ActiveStepChanged(object sender, EventArgs e)
	//    {
	//    if(hdr == null && Session["fvr_hdr"] != null)
	//        {
	//        hdr				= (member_fvr_hdr) Session["fvr_hdr"];
	//        _tools				= new Toolbox();
	//        current_user		= Toolbox.do_handle_authentication(_page_id);
	//        }
	//    if(hdr != null)
	//        {
	//        chks.populate_session(Page, ref chks);
	//        chks.active_step				= wiz_renew.ActiveStepIndex;
	//        Session["emp_checks"]			= chks;
	//        Button b						= new Button();
	//        if(wiz_renew.FindControl("FinishNavigationTemplateContainerID") != null)
	//            {
	//            if(wiz_renew.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton") != null)
	//                {
	//                b	= (Button) wiz_renew.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton");
	//                }
	//            }
	//        switch (wiz_renew.ActiveStep.ID)
	//            {
	//            case "RENEW_welcome_step":
	//                wiz_renew.HeaderText		= (string) dt_steps.Select("type = 'RENEW' AND tab_index = 0")[0]["name"];
	//                dtl						= new member_fvr_dtl(hdr.id, 0, this_user.id);
	//                RENEW_welcome.dtl				= dtl;
	//                RENEW_welcome.current_user	= this_user;
	//                RENEW_welcome.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_information_step":
	//                wiz_renew.HeaderText			= (string) dt_steps.Select("type = 'RENEW' AND tab_index = 1")[0]["name"];
	//                dtl								= new member_fvr_dtl(hdr.id, 1, this_user.id);
	//                RENEW_information.dtl			= dtl;
	//                RENEW_information.current_user	= this_user;
	//                RENEW_information.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_handbook_step":
	//                wiz_renew.HeaderText			= (string) dt_steps.Select("type = 'RENEW' AND tab_index = 2")[0]["name"];
	//                dtl							= new member_fvr_dtl(hdr.id, 2, this_user.id);
	//                RENEW_emp_handbook.dtl			= dtl;
	//                RENEW_emp_handbook.current_user	= this_user;
	//                RENEW_emp_handbook.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_survey_step":
	//                wiz_renew.HeaderText			= (string) dt_steps.Select("type = 'RENEW' AND tab_index = 3")[0]["name"];
	//                dtl								= new member_fvr_dtl(hdr.id, 3, this_user.id);
	//                RENEW_survey.dtl			= dtl;
	//                RENEW_survey.current_user	= this_user;
	//                RENEW_survey.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_president_step":
	//                wiz_renew.HeaderText					= (string) dt_steps.Select("type = 'RENEW' AND tab_index = 4")[0]["name"];
	//                dtl									= new member_fvr_dtl(hdr.id, 4, this_user.id);
	//                RENEW_message_from_president.dtl			= dtl;
	//                RENEW_message_from_president.detail_id			= dtl.id;
	//                RENEW_message_from_president.current_user	= this_user;
	//                RENEW_message_from_president.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_truck_step":
	//                wiz_renew.HeaderText		=  (string) dt_steps.Select("type = 'RENEW' AND tab_index = 5")[0]["name"];
	//                dtl						= new member_fvr_dtl(hdr.id, 5, this_user.id);
	//                RENEW_truck.dtl			= dtl;
	//                RENEW_truck.current_user	= this_user;
	//                RENEW_truck.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_contacts_step":
	//                wiz_renew.HeaderText		=  (string) dt_steps.Select("type = 'RENEW' AND tab_index = 6")[0]["name"];
	//                dtl						= new member_fvr_dtl(hdr.id, 6, this_user.id);
	//                RENEW_contact_list.dtl			= dtl;
	//                RENEW_contact_list.current_user	= this_user;
	//                RENEW_contact_list.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_confidentiality_step":
	//                wiz_renew.HeaderText						=  (string) dt_steps.Select("type = 'RENEW' AND tab_index = 7")[0]["name"];
	//                dtl										= new member_fvr_dtl(hdr.id, 7, this_user.id);
	//                RENEW_confidentiality_agreement.dtl			= dtl;
	//                RENEW_confidentiality_agreement.current_user	= this_user;
	//                RENEW_confidentiality_agreement.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_qualifications_step":
	//                wiz_renew.HeaderText					=  (string) dt_steps.Select("type = 'RENEW' AND tab_index = 8")[0]["name"];
	//                dtl									= new member_fvr_dtl(hdr.id, 8, this_user.id);
	//                RENEW_qualifications_stmt.dtl				= dtl;
	//                RENEW_qualifications_stmt.current_user	= this_user;
	//                RENEW_qualifications_stmt.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_trademarks_step":
	//                wiz_renew.HeaderText					=  (string) dt_steps.Select("type = 'RENEW' AND tab_index = 9")[0]["name"];
	//                dtl									= new member_fvr_dtl(hdr.id, 9, this_user.id);
	//                RENEW_trademarks.dtl				= dtl;
	//                RENEW_trademarks.current_user	= this_user;
	//                RENEW_trademarks.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_code_step":
	//                wiz_renew.HeaderText					=  (string) dt_steps.Select("type = 'RENEW' AND tab_index = 10")[0]["name"];
	//                dtl									= new member_fvr_dtl(hdr.id, 10, this_user.id);
	//                RENEW_code.dtl				= dtl;
	//                RENEW_code.current_user	= this_user;
	//                RENEW_code.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_safety_step":
	//                wiz_renew.HeaderText					=  (string) dt_steps.Select("type = 'RENEW' AND tab_index = 11")[0]["name"];
	//                dtl									= new member_fvr_dtl(hdr.id, 11, this_user.id);
	//                RENEW_safety.dtl				= dtl;
	//                RENEW_safety.current_user	= this_user;
	//                RENEW_safety.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_training_step":
	//                wiz_renew.HeaderText			= (string) dt_steps.Select("type = 'RENEW' AND tab_index = 12")[0]["name"];
	//                dtl								= new member_fvr_dtl(hdr.id, 12, this_user.id);
	//                RENEW_training.dtl				= dtl;
	//                RENEW_training.detail_id		= dtl.id;
	//                RENEW_training.current_user		= this_user;
	//                RENEW_training.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "RENEW_finish_step":
	//                wiz_renew.HeaderText			= (string) dt_steps.Select("type = 'RENEW' AND tab_index = 13")[0]["name"];
	//                dtl								= new member_fvr_dtl(hdr.id, 13, this_user.id);
	//                RENEW_finish.dtl				= dtl;
	//                RENEW_finish.detail_id			= dtl.id;
	//                RENEW_finish.current_user		= this_user;
	//                RENEW_finish.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            default:
	//            break;
	//            }
	//        }
	//    }
	//protected void wiz_exit_ActiveStepChanged(object sender, EventArgs e)
	//    {
	//    if(hdr == null && Session["fvr_hdr"] != null)
	//        {
	//        hdr					= (member_fvr_hdr) Session["fvr_hdr"];
	//        _tools				= new Toolbox();
	//        current_user		= Toolbox.do_handle_authentication(_page_id);
	//        }
	//    Button b						= (Button)wiz_exit.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton");
	//    if(hdr != null)
	//        {
	//        chks.populate_session(Page, ref chks);
	//        chks.active_step				= ((Wizard) sender).ActiveStepIndex;
	//        Session["emp_checks"]			= chks;
	//        switch (wiz_exit.ActiveStep.ID)
	//            {
	//            case "EXIT_interview_step":
	//                // 0
	//                wiz_exit.HeaderText				=  Toolbox.doSQL_string(@"SELECT name FROM member_fvr_tab  WHERE type = 'EXIT' AND tab_index = 0" , null);
	//                dtl								= new member_fvr_dtl(hdr.id, 0, this_user.id);
	//                EXIT_interview.dtl				= dtl;
	//                EXIT_interview.current_user		= this_user;
	//                EXIT_interview.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "EXIT_checklist_step":
	//                // 1
	//                wiz_exit.HeaderText				= Toolbox.doSQL_string(@"SELECT name FROM member_fvr_tab  WHERE type = 'EXIT' AND tab_index = 1" , null);
	//                dtl								= new member_fvr_dtl(hdr.id, 1, this_user.id);
	//                EXIT_checklist.dtl				= dtl;
	//                EXIT_checklist.current_user		= this_user;
	//                EXIT_checklist.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "EXIT_forwarding_step":
	//                // 2
	//                wiz_exit.HeaderText					=  Toolbox.doSQL_string(@"SELECT name FROM member_fvr_tab  WHERE type = 'EXIT' AND tab_index = 2" , null);
	//                dtl									= new member_fvr_dtl(hdr.id, 2, this_user.id);
	//                EXIT_forwarding.dtl					= dtl;
	//                EXIT_forwarding.current_user		= this_user;
	//                EXIT_forwarding.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "EXIT_notification_step":
	//                // 3
	//                wiz_exit.HeaderText				=  Toolbox.doSQL_string(@"SELECT name FROM member_fvr_tab  WHERE type = 'EXIT' AND tab_index = 3" , null);
	//                dtl								= new member_fvr_dtl(hdr.id, 3, this_user.id);
	//                EXIT_notification.dtl				= dtl;
	//                EXIT_notification.current_user	= this_user;
	//                EXIT_notification.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            case "EXIT_finalreview_step":
	//                // 4
	//                wiz_exit.HeaderText					= Toolbox.doSQL_string(@"SELECT name FROM member_fvr_tab  WHERE type = 'EXIT' AND tab_index = 4" , null);
	//                dtl									= new member_fvr_dtl(hdr.id, 4, this_user.id);
	//                EXIT_finalreview.dtl				= dtl;
	//                EXIT_finalreview.current_user		= this_user;
	//                EXIT_finalreview.populate(dtl.id);
	//                if(!dtl.is_complete())
	//                    {
	//                    b.Style.Add("display", "none");
	//                    }
	//                else
	//                    {
	//                    b.Style.Remove("display");
	//                    }
	//            break;
	//            default:
	//            break;
	//            }
	//        }
	//    }
	//protected void wiz_new_SideBarButtonClick(object sender, WizardNavigationEventArgs e)
	//	{
	//	Wizard w			= (Wizard) sender;
	//	}
	//protected void wiz_renew_SideBarButtonClick(object sender, WizardNavigationEventArgs e)
	//	{
	//	Wizard w			= (Wizard) sender;
	//	}
	//protected void wiz_exit_SideBarButtonClick(object sender, WizardNavigationEventArgs e)
	//	{
	//	Wizard w			= (Wizard) sender;
	//	}
	//protected void SideBarList_SelectedIndexChanged(object sender, EventArgs e)
	//	{
	//	}
	//protected void new_SideBarList_ItemDataBound(object sender, DataListItemEventArgs e)
	//	{
	//	DataList dl			= (DataList) sender;
	//	chks.populate_session(Page, ref chks);
	//	Image i				= (Image) e.Item.FindControl("SideBarIcon");
	//	WizardStepBase wb	= (WizardStepBase) e.Item.DataItem;
	//	switch(wb.ID)
	//		{
	//		case "NEWHIRE_welcome_step":
	//			i.ImageUrl		= chks.new_welcome ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "NEWHIRE_information_step":
	//			i.ImageUrl		= chks.new_information ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "NEWHIRE_handbook_step":
	//			i.ImageUrl		= chks.new_handbook ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "NEWHIRE_president_step":
	//			i.ImageUrl		= chks.new_president ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "NEWHIRE_hire_acknowledgement_step":
	//			i.ImageUrl		= chks.new_hire_acknowledgement ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "NEWHIRE_truck_step":
	//			i.ImageUrl		= chks.new_truck ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "NEWHIRE_contacts_step":
	//			i.ImageUrl		= chks.new_contact_list ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "NEWHIRE_confidentiality_step":
	//			i.ImageUrl		= chks.new_confidentiality ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "NEWHIRE_qualifications_step":
	//			i.ImageUrl		= chks.new_qualifications ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "NEWHIRE_training_step":
	//			i.ImageUrl		= chks.new_training ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "NEWHIRE_finish_step":
	//			i.ImageUrl		= chks.new_finish ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		}
	//	}
	//protected void renew_SideBarList_ItemDataBound(object sender, DataListItemEventArgs e)
	//	{
	//	DataList dl			= (DataList) sender;
	//	chks.populate_session(Page, ref chks);
	//	Image i				= (Image) e.Item.FindControl("SideBarIcon");
	//	WizardStepBase wb	= (WizardStepBase) e.Item.DataItem;
	//	switch(wb.ID)
	//		{
	//		case "RENEW_welcome_step":
	//			i.ImageUrl		= chks.renew_welcome ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_information_step":
	//			i.ImageUrl		= chks.renew_information ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_handbook_step":
	//			i.ImageUrl		= chks.renew_handbook ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_president_step":
	//			i.ImageUrl		= chks.renew_president ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_truck_step":
	//			i.ImageUrl		= chks.renew_truck ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_contacts_step":
	//			i.ImageUrl		= chks.renew_contact_list ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_confidentiality_step":
	//			i.ImageUrl		= chks.renew_confidentiality ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_qualifications_step":
	//			i.ImageUrl		= chks.renew_qualifications ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_trademarks_step":
	//			i.ImageUrl		= chks.renew_trademarks ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_code_step":
	//			i.ImageUrl		= chks.renew_code ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_safety_step":
	//			i.ImageUrl		= chks.renew_safety ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_survey_step":
	//			i.ImageUrl		= chks.renew_survey ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_training_step":
	//			i.ImageUrl		= chks.renew_training ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "RENEW_finish_step":
	//			i.ImageUrl		= chks.renew_finish ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		}
	//	}
	//protected void ex_SideBarList_ItemDataBound(object sender, DataListItemEventArgs e)
	//	{
	//	DataList dl			= (DataList) sender;
	//	chks.populate_session(Page, ref chks);
	//	Image i				= (Image) e.Item.FindControl("SideBarIcon");
	//	WizardStepBase wb	= (WizardStepBase) e.Item.DataItem;
	//	switch(wb.ID)
	//		{
	//		case "EXIT_interview_step":
	//			i.ImageUrl		= chks.exit_interview ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "EXIT_checklist_step":
	//			i.ImageUrl		= chks.exit_checklist ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "EXIT_forwarding_step":
	//			i.ImageUrl		= chks.exit_forwarding ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "EXIT_notification_step":
	//			i.ImageUrl		= chks.exit_notification ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		case "EXIT_finalreview_step":
	//			i.ImageUrl		= chks.exit_finalreview ? "~/images/icon/icon[ok].gif" : i.ImageUrl;
	//		break;
	//		}
	//	}

		#endregion commented
	public static void send_notice_email(member_fvr_hdr hdr, NeMember this_user)
		{
            var _Tools = new Toolbox();
		var dtl			= new member_fvr_dtl();
		dtl.member_fvr_hdr_id		= hdr.id;
		dtl.member_id				= this_user.id;
		if(!dtl.is_complete())
			{
			throw new Exception("FVR Has errors and can not be finished.");
			}
		else
			{
			dtl.do_complete();
			}
		// Send email
		var created_user		= new NeMember(hdr.req_member_id);
		try
			{
			var em				= new NeEMail();
			em.Subject				= "FVR #"+hdr.id+" has been completed by "+this_user.FullName;
			em.To					= created_user.NEEmail;
			em.From					= "administrator@" + Toolbox.app_setting("DomainForEmail");
			em.Body					= "Please login to NESI and review their submission.<br/> Steps/Files included in this FVR:<br/>";
			var fvr_info		= Toolbox.doSQL_dt(@" SELECT c.name tab, CONCAT(d.name, '.', d.ext) filename FROM member_fvr_dtl a LEFT JOIN member_fvr_hdr b ON a.member_fvr_hdr_id = b.id LEFT JOIN member_fvr_tab c ON b.type = c.type AND a.tab_index = c.id LEFT JOIN filestore.files d ON a.file_id = d.id WHERE a.member_id = @v0  AND a.member_fvr_hdr_id = @v1  ORDER BY a.tab_index", new object[] {  this_user.id, hdr.id } );
			if(fvr_info.Rows.Count > 0)
				{
				var sb	= new StringBuilder();
				sb.Append("<ul>");
				foreach(DataRow fi in fvr_info.Rows)
					{
					var tab		= fi["tab"].ToString();
					var filename	= fi["filename"] == DBNull.Value ? "N/A" : fi["filename"].ToString();
					sb.AppendFormat("<li>Step: {0}, Filename: {1}</li>", tab, filename);
					}
				sb.Append("</ul>");
				em.Body				+= sb.ToString();
				em.CC				= "fvr@" + Toolbox.app_setting("DomainForEmail");
				}
			else
				{
				em.Body				+= "There were errors retrieving the steps/files for this FVR - Matt has been included in this email and will look into why this happened.";
				em.CC				= "fvr@" + Toolbox.app_setting("DomainForEmail") + ";" + "mhyde@" + Toolbox.app_setting("DomainForEmail");
				}
			em.isHTML				= true;
			em.Send();
			}
		catch (Exception ee)
			{
                _Tools.catch_error(ee);

			}
		}
		#region commented
	//protected void wiz_new_FinishButtonClick(object sender, WizardNavigationEventArgs e)
	//	{
	//	wiz_new.Visible				= false;
	//	send_notice_email();
	//	}
	//protected void wiz_renew_FinishButtonClick(object sender, WizardNavigationEventArgs e)
	//	{
	//	wiz_renew.Visible				= false;
	//	send_notice_email();
	//	}
	//protected void wiz_exit_FinishButtonClick(object sender, WizardNavigationEventArgs e)
	//	{
	//	wiz_exit.Visible			= false;
	//	ScriptManager.RegisterStartupScript(this, GetType(), "closeme", "opener.location.reload();window.close()", true);
	//	}
	//protected void bt_refresh_click(object sender, EventArgs e)
	//	{
	//	chks				= (emp_checks) Session["emp_checks"];
	//	if(wiz_exit.Visible)
	//		{
	//		wiz_exit.ActiveStepIndex	= chks.active_step;
	//		wiz_exit_ActiveStepChanged(wiz_exit, null);
	//		}
	//	//else if(wiz_new.Visible)
	//	//	{
	//	//	wiz_new.ActiveStepIndex		= chks.active_step;
	//	//	wiz_new_ActiveStepChanged(wiz_new, null);
	//	//	}
	//	else if(wiz_renew.Visible)
	//		{
	//		wiz_renew.ActiveStepIndex		= chks.active_step;
	//		wiz_renew_ActiveStepChanged(wiz_renew, null);
	//		}
	//	}
	//protected void wiz_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
	//	{
	//	Wizard wiz		= (Wizard) sender;
	//	wiz.ActiveStepIndex	= wiz.ActiveStepIndex - 1;
	//	}
	//protected void wiz_NextButtonClick(object sender, WizardNavigationEventArgs e)
	//	{
	//	Wizard wiz		= (Wizard) sender;
	//	wiz.ActiveStepIndex	= wiz.ActiveStepIndex + 1;
	//	}
		#endregion commented
	[WebMethod]
	public static string do_remove_file(int detail_id)
		{
		var dtl				= new member_fvr_dtl(detail_id);
		dtl.uploaded_file_id			= 0;
		dtl.save();
		var hist			= new member_fvr_history(detail_id);
		hist.file_uploaded				= 0;
		hist.confirmed					= 0;
		hist.save();
		return "Saved";
		}
	[WebMethod]
	public static string do_acknowledge(int dtl_id, bool is_checked, string type, string subtype)
		{
		var chks					= new emp_checks();
		chks							= (emp_checks) HttpContext.Current.Session["emp_checks"];
		var dtl				= new member_fvr_dtl(dtl_id);
		var hdr				= new member_fvr_hdr(dtl.member_fvr_hdr_id);
		var hist			= new member_fvr_history(dtl.id);
		var this_user				= new NeMember(HttpContext.Current.Session["session"].ToString());
		var error					= "";
		var returned					= "";

		if(dtl.upload_required == 1 && dtl.uploaded_file_id == 0)
			{
			error				= "This step requires that you upload a file before you can proceed.";
			}
		else if(dtl.tab_index_pseudo == 1)
			{
			var emp_page		= new sections_hr_i_modules_employee_information();
			emp_page.dtl											= dtl;
			emp_page.current_user									= this_user;
			emp_page.populate(Convert.ToInt32(dtl.member_id));
			if(!emp_page.validate(true))
				{
				error										= "You must fill out all information in this FVR, that is denoted with an asterisk, before proceeding.";
				chks.new_information						= is_checked;
				HttpContext.Current.Session["emp_checks"]	= chks;
				}
			else
				{
				hist.member_fvr_dtl_id			= dtl.id;
				hist.confirmed					= is_checked ? 1 : 0;
				HttpContext.Current.Session["emp_checks"]			= chks;
				hist.save();
				if(Toolbox.doSQL_int(@"SELECT COUNT(a.id) FROM member_fvr_dtl a LEFT JOIN member_fvr_history b ON a.id = b.member_fvr_dtl_id
WHERE a.member_id = @v0 AND  a.member_fvr_hdr_id = @v1 AND IFNULL(b.confirmed, 0) = 0", new object[] { dtl.member_id, dtl.member_fvr_hdr_id}) == 0)
					{
					send_notice_email(hdr, this_user);
                    if (hdr.type == "NEWHIRE" && this_user.hrstatus_id == 1)
                    {
                        var typepriv = new NEMemberTypePrivilege();
                        typepriv.ClearPrivileges(this_user.id);
                        typepriv.SetMemberPrivileges(this_user.id);
                        this_user.hrstatus_id = 6;
                        this_user.save();
                    }
                    return "CLOSEME";
					}
				returned		= "Saved";
				}
			}
		else
			{
			hist.member_fvr_dtl_id			= dtl.id;
			hist.confirmed					= is_checked ? 1 : 0;
			HttpContext.Current.Session["emp_checks"]			= chks;
			hist.save();
			if(Toolbox.doSQL_int(@"SELECT COUNT(a.id) FROM member_fvr_dtl a LEFT JOIN member_fvr_history b ON a.id = b.member_fvr_dtl_id
WHERE a.member_id = @v0 AND a.member_fvr_hdr_id = @v1 AND IFNULL(b.confirmed, 0) = 0", new object[] { dtl.member_id, dtl.member_fvr_hdr_id}) == 0)
				{
				dtl.active	= 0;
				dtl.save();
				send_notice_email(hdr, this_user);
				if(hdr.type == "NEWHIRE" && this_user.hrstatus_id == 1)
					{
					var typepriv = new NEMemberTypePrivilege();
					typepriv.ClearPrivileges(this_user.id);
					typepriv.SetMemberPrivileges(this_user.id);
					this_user.hrstatus_id = 6;
					this_user.save();
					}
				return "CLOSEME";
				}
			returned		= "Saved";
			}
		if(error != "")
			{
			returned		= error;
			}
		return returned;
		}
}