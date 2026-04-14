using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using DevExpress.Xpo;
using nesi.core;

public partial class sections_member_tickets_CreateIssue : System.Web.UI.Page
	{
	NeMember current_user;
	NameValueCollection _q;
	bool is_leadship_team		= false;
	bool is_mobile				= false;
	protected void Page_Init(object sender, EventArgs e)
		{
		_q = Request.QueryString;
		is_mobile = !string.IsNullOrEmpty(_q["is_mobile"]);
		current_user = Toolbox.do_handle_authentication(170);
		is_leadship_team = current_user.AuthenticatedForPrivilege(131);
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		populate_groups();
		var actiontype = Request.QueryString["action"];
		if (IsPostBack)
			{
			var moduleid = 0;
			if(ddl_page.Value != null)
				{
				int.TryParse(ddl_page.Value.ToString(), out moduleid);
				}
			if (!txtIssue.Visible)
				{
				if (tb_search.Text != "") // && moduleid != 0)
					{
					if(!is_mobile)
						{
						gv_matches.Visible = true;
						var tickets = new NETickets();
						tickets.module_id = moduleid;
						var issues = NETickets.GetCreatedissues(tb_search.Text, (int) current_user.id);
						gv_matches.Columns["AsignedTo"].Visible = false;
						gv_matches.Columns["Priority"].Visible = false;
						gv_matches.DataSource = issues;
						gv_matches.DataBind();
							
						lbCreateNewTicket.Visible = true;
						}
					}
				}
			}
		else  // if !ispostback
			{
			if ((Request.QueryString["from"] == "milestone"))
				{
				if ((Request.QueryString["ms_id"] != "") && (Request.QueryString["ms_id"] != null))
					{
					var milestone = Toolbox.doSQL_string("Select ifnull((select milestone from memberoffer_milestones where id =@v0),'')" ,Request.QueryString["ms_id"]);
					var milestone_dt = Toolbox.doSQL_string("Select ifnull((select due from memberoffer_milestones where id =@v0),'')", Request.QueryString["ms_id"]);

					var milestone_memberid = Toolbox.doSQL_int(@"Select member_offers.memberid from memberoffer_milestones 
inner join member_offers on memberoffer_milestones.offerid = member_offers.id where memberoffer_milestones.id =@v0 limit 1" , Request.QueryString["ms_id"]);
					tb_search.Text = milestone;
					var groupid = Toolbox.doSQL_int(@"Select ifnull((SELECT
ticket_group.ticket_group_id 
FROM
ticket_group
INNER JOIN ticketpage ON ticketpage.ticketpage_ticket_group_id = ticket_group.ticket_group_id
INNER JOIN ticketheader ON ticketheader.ticketheader_module_id = ticketpage.ticketpage_id
WHERE
		  ticket_group_administrator =@v0
ORDER BY
ticketheader.ticketheader_created_date DESC
LIMIT 1
),0)", milestone_memberid);
					if (groupid != 0)
						{
						ddl_group.Value = groupid;
						ddlUser.DataSource = new NETickets().GetTicketManagerList((int) ddl_group.Value);
						ddlUser.DataValueField = "ID";
						ddlUser.DataTextField = "NAME";
						ddlUser.DataBind();

						ddlUser.SelectedValue = milestone_memberid.ToString();
						}
					dteexpected.Date = Convert.ToDateTime(milestone_dt);

					}
				}
			}
		#region Mobile redux
		if(is_mobile)
			{
			ticket_details.Visible							= true;
			ddl_group.Native								= true;
			ddl_page.Native									= true;
			ddl_page.ClientSideEvents.SelectedIndexChanged	= "";
			ddl_group.AutoPostBack							= true;
			ddl_page.AutoPostBack							= true;
			ddl_type.AutoPostBack							= true;
			ddl_page.ClientEnabled							= true;
			ddl_type.ClientEnabled							= true;
			bt_reset.ClientSideEvents.Click					= "function(s,e){if(confirm('Are you sure you want to reset the form?')){location.href = location.href;}}";
			tb_search.ClientEnabled							= true;
			gv_matches.Visible								= false;
			ddl_type.Native									= true;
			Label1.Visible									= true;
			btn_search.Visible								= false;
			filMyFile.Visible								= true;
			details_headings.Visible						= false;
			pastbox_td.Visible								= false;
			txtIssue.Visible								= true;
			txtIssue.StylesToolbars.Toolbar.CssClass		= "hideme";
			txtIssue.StylesToolbars.BarDockControl.CssClass = "hideme";
			txtIssue.StylesToolbars.ToolbarItem.CssClass	= "hideme";
			txtIssue.Settings.AllowHtmlView					= false;
			txtIssue.Settings.AllowPreview					= false;
			chkPrivate.Visible								= false;
			txtIssue.Height									= Unit.Pixel(200);
			ddlSeverity.Visible								= false;
			ddloo.Visible									= false;
			ddlUser.Visible									= false;
			dteexpected.Visible								= false;
			txtIssue.Width									= Unit.Percentage(99);
			ddl_group.CaptionSettings.Position				= DevExpress.Web.EditorCaptionPosition.Top;
			ddl_group.Width									= Unit.Percentage(99);
			ddl_page.CaptionSettings.Position				= DevExpress.Web.EditorCaptionPosition.Top;
			ddl_page.Width									= Unit.Percentage(99);
			ddl_type.CaptionSettings.Position		= DevExpress.Web.EditorCaptionPosition.Top;
			ddl_type.Width							= Unit.Percentage(99);
			tb_search.CaptionSettings.Position			= DevExpress.Web.EditorCaptionPosition.Top;
			tb_search.Width								= Unit.Percentage(99);
			btnSubmit.Visible								= true;
			btnSubmit.Width									= Unit.Percentage(99);
			lbl_selectass.Visible							= false;
			lbl_selectoo.Visible							= false;
			lbl_selectpri0.Visible							= false;
			lbl_selectpri.Visible							= false;
			Label1.Text										= "Include File:";
			lbl_error.Width									= Unit.Pixel(350);
			btnSubmit.Font.Size								= FontUnit.Point(18);
			var meta_vp								= new HtmlMeta();
			meta_vp.Name									= "viewport";
			meta_vp.Content									= "width=device-width";
			var meta_vp2								= new HtmlMeta();
			meta_vp2.Name									= "viewport";
			meta_vp2.Content								= "width=device-width, initial-scale=1, maximum-scale=1, user-scalable=0";
			var meta_hh								= new HtmlMeta();
			meta_hh.Name									= "HandheldFriendly";
			meta_hh.Content									= "true";
			bt_mobile_back.Visible							= true;
			ph_meta.Controls.Add(meta_vp);
			ph_meta.Controls.Add(meta_vp2);
			ph_meta.Controls.Add(meta_hh);
			}
		#endregion Mobile redux
		}
	protected void populate_groups()
		{
		ddl_group.DataSource				= Toolbox.doSQL_dt(@"SELECT a.ticket_group_id, CONCAT(a.ticket_group_name,' (',b.member_fullname,')') ticket_group_name FROM ticket_group a LEFT JOIN member b ON a.ticket_group_administrator = b.member_id WHERE a.active  ORDER BY a.order_n", null );
		ddl_group.DataBind();
		populate_types();
		populate_pages();
		}
	private void populate_types()
		{
		if(ddl_group.Value != null)
			{
			ddl_type.DataSource					= Toolbox.doSQL_dt(@" SELECT a.tickettype_id, a.tickettype_name FROM tickettype a INNER JOIN ticket_group_type_link b ON a.tickettype_id = b.ticket_type_id INNER JOIN ticketpage c ON c.ticketpage_ticket_group_id = b.ticket_group_id WHERE c.ticketpage_ticket_group_id = @v0  GROUP BY a.tickettype_id", new object[] {  ddl_group.Value } );
		ddl_type.DataBind();
			}
		}
	private void populate_pages()
		{
		if(ddl_group.Value != null)
			{
			ddl_page.DataSource					= Toolbox.doSQL_dt(@"SELECT ticketpage_id, ticketpage_name FROM ticketpage where ticketpage_ticket_group_id = @v0  order by ticketpage_name", new object[] {  ddl_group.Value } );
			ddl_page.DataBind();
			}
		}
	protected void SearchForIssues()
		{
		pnl_recent_tickets.Visible			= false;
		ddl_page.ClientEnabled				= true;
		ddl_type.ClientEnabled				= true;
		tb_search.ClientEnabled			= true;
		txtIssue.Visible					= false;
		Label1.Visible						= false;
		bt_reset.ClientSideEvents.Click		= "function(s,e){if(confirm('Are you sure you want to reset this form?')){location.href = location.href;}}";
		filMyFile.Visible					= false;
		chkPrivate.Visible					= false;
		btnSubmit.Visible					= false;
		pastebox.Visible					= false;
		lbl_error.Visible					= false;
		ticket_details.Visible				= false;
		lbl_selectass.Visible				= false;
		ddlUser.Visible						= false;
		lbl_selectoo.Visible				= false;
		ddloo.Visible						= false;
		lbl_selectpri.Visible				= false;
		ddlSeverity.Visible					= false;
		lbl_error.Text						= "";
		lbl_error.Style.Clear();
		var errors					= new List<string>();
		var is_error						= false;

		lbl_error.Text = "";
		var strSearchBox = tb_search.Text;
		if (strSearchBox.Trim() == "")
			{
			is_error		= true;
			errors.Add("Please provide something to search for.");
			}
		var moduleid = 0;
		if(ddl_page.Value != null)
			{
			int.TryParse(ddl_page.Value.ToString(), out moduleid);
			}
		if (moduleid == 0)
			{
			is_error		= true;
			errors.Add("Please select the page/section this ticket pertains to.");
			}
		
		if(is_error)
			{
			lbl_error.Style.Add("padding", "10px");
			lbl_error.Text += "<div style='font-size:14px;'>Please address the following errors: </div>";
			foreach(var s in errors)
				{
				lbl_error.Text += string.Format("<div>&bullet; {0}</div>", s); 
				}
			lbl_error.Visible = true;
			return;
			}

		Session["CreateIssuesSearch"] = tb_search.Text;
		try
			{
			Session["TypeSearch"] = ddl_page.Value.ToString();
			}
		catch
			{
			Session["TypeSearch"] = "0";
			}
		var tickets = new NETickets();
		tickets.module_id = moduleid;
		var issues = NETickets.GetCreatedissues(strSearchBox, (int) current_user.id);
		if (issues.Rows.Count != 0)
			{
			if(!is_mobile)
				{
			gv_matches.Visible = true;
			gv_matches.Columns["AsignedTo"].Visible = false;
			gv_matches.Columns["Priority"].Visible = false;
			gv_matches.DataSource = issues;
			gv_matches.DataBind();
			lbCreateNewTicket.Visible = true;
				}
			}
		else
			{
			gv_matches.Visible = false;
			txtIssue.Visible = true;
			Label1.Visible = true;
			filMyFile.Visible = true;
			lbCreateNewTicket.Visible = false;
			chkPrivate.Visible = true;
			//lbCreateNewTicket.Visible = false;
			btnSubmit.Visible = true;
			pastebox.Visible = true;
			ticket_details.Visible = true;
			if (is_leadship_team)
				{
				dteexpected.ClientVisible = true;
				lbl_selectpri0.ClientVisible = true;
				}

			if (Toolbox.doSQL_int(@"Select ticket_group_administrator from ticket_group where ticket_group_id =@v0 " , 
				new object[] {		
				ddl_group.Value}) == current_user.id || current_user.id == 8)
				{
				ddlUser.Visible = true;
				ddlSeverity.Visible = true;
				lbl_selectass.Visible = true;
				lbl_selectpri.Visible = true;
				if (Request.QueryString["from"] != "milestone")
					{
					ddlUser.DataSource = tickets.GetTicketManagerList(Convert.ToInt32(ddl_group.Value));
					ddlUser.DataValueField = "ID";
					ddlUser.DataTextField = "NAME";
					ddlUser.DataBind();
					var lic3 = new ListItem("Select Assigned User", "0");
					ddlUser.Items.Insert(0, lic3);
					ddlUser.SelectedValue = "0";
					}

				ddlSeverity.DataSource = tickets.getticketpriotiylist();
				ddlSeverity.DataValueField = "ID";
				ddlSeverity.DataTextField = "NAME";
				ddlSeverity.DataBind();
				var lic4 = new ListItem("Select Severity", "0");
				ddlSeverity.Items.Insert(0, lic4);
				ddlSeverity.SelectedValue = "0";
				}
			if (ddl_type.Value.ToString() == "6")
				{
				ddloo.Visible = true;
				lbl_selectoo.ClientVisible = true;
				ddloo.DataSource = Toolbox.doSQL_dt(@"Select member_id id, get_name(member_id) name from member  where member_status = 'Active' order by member_fullname" , null);
				ddloo.DataValueField = "id";
				ddloo.DataTextField = "name";
				ddloo.DataBind();
				var lic5 = new ListItem("Select Objective Owner", "0");
				ddloo.Items.Insert(0, lic5);
				ddloo.SelectedValue = "0";
				}
			}

		}
	private void populate_related()
		{
		if(ddl_group.Value != null)
			{
			var page_filter		= ddl_page.Value == null ? "" : "a.ticketheader_module_id ="+ddl_page.Value+" AND ";
			var r				= Toolbox.doSQL_dt(string.Format(@" SELECT a.ticketheader_id t_id, a.ticketheader_issue t_subject, a.ticketheader_created_date t_when, c.ticketstatus_status t_status, b.ticketpage_name t_pertaining, (SELECT ticketissues_message FROM ticketissues WHERE ticketissues_ticketheader_id = a.ticketheader_id ORDER BY ticketissues_id LIMIT 1) t_body, (SELECT COUNT(*) FROM ticket_memberview WHERE ticket_id = a.ticketheader_id AND member_id = @v2 ) t_watching, IFNULL(a.ticketheader_member_assigned_id, 0) t_assigned, a.ticketheader_createdby_member_id t_created, d.ticket_group_administrator t_admin FROM ticketheader a LEFT JOIN ticketpage b ON a.ticketheader_module_id = b.ticketpage_id LEFT JOIN ticketstatus c ON a.ticketheader_status_id = c.ticketstatus_id LEFT JOIN ticket_group d ON b.ticketpage_ticket_group_id = d.ticket_group_id
WHERE b.ticketpage_ticket_group_id = @v0  AND {0}  a.ticketheader_status_id NOT IN (5,7) AND a.ticketheader_private = 0 ORDER BY a.ticketheader_created_date DESC", page_filter), new object[] {  ddl_group.Value,  current_user.id } );
			var output		= new DataTable("related");
			output.Columns.Add("t_id", typeof(int));
			output.Columns.Add("t_subject", typeof(string));
			output.Columns.Add("t_when",  typeof(string));
			output.Columns.Add("t_status",  typeof(string));
			output.Columns.Add("t_pertaining",  typeof(string));
			output.Columns.Add("t_body",  typeof(string));
			output.Columns.Add("t_watching", typeof(int));
			output.Columns.Add("t_assigned", typeof(int));
			output.Columns.Add("t_created", typeof(int));
			output.Columns.Add("t_admin", typeof(int));

			foreach(DataRow dr in r.Rows)
				{
				var t_id			= (int) dr["t_id"];
				var t_subject	= dr["t_subject"].ToString();
				var t_when		= Toolbox.fun_time((DateTime)dr["t_when"]);
				var t_status		= dr["t_status"].ToString();
				var t_pertaining	= dr["t_pertaining"].ToString();
				var t_body		= dr["t_body"].ToString();
				var t_watching		= Convert.ToInt32(dr["t_watching"]);
				var t_assigned		= Convert.ToInt32(dr["t_assigned"]);
				var t_created		= Convert.ToInt32(dr["t_created"]);
				var t_admin			= Convert.ToInt32(dr["t_admin"]);
			
				t_body			= t_body.Contains("<img src=\"data:image")
									? t_body.Replace("<img src=", "<br /><img onclick=\"boing($(this).attr('src'), 'embedded_ticket_attachment', $(window).width(), $(window).height())\" style='cursor:pointer;max-width:335px;height:auto;border:solid 1px #000;' src=")
									: t_body;
				t_body			= t_body.Replace("\n", "").Replace("<p><br/></p>", "").Replace("</p>", "<br/>").Replace("<p>", "");
				output.Rows.Add(t_id, t_subject, t_when, t_status,t_pertaining, t_body, t_watching, t_assigned, t_created, t_admin);
				}
			dv_recent.DataSource	= output;
			dv_recent.DataBind();
			}
		else
			{
			pnl_recent_tickets.Visible			= false;
			}
		}
	public string dv_button_link(int i, object di)
		{
		var row		= di as DataRowView;
		return "";//row["t_id"];

		}
	public bool dv_button_visible(int i, object di)
		{
		var row		= di as DataRowView;
		var is_watching		= (int) row["t_watching"];
		var t_assigned		= (int) row["t_assigned"];
		var t_created		= (int) row["t_created"];
		var t_admin			= (int) row["t_admin"];
		if(!Toolbox.Contains(current_user.id32, new int[]{t_admin, t_assigned, t_created}))
			{
			return i == is_watching;
			}
		else
			{
			return false;
			}
		}
	protected void btnSubmit_Click(object sender, EventArgs e)
		{
		Session["CreateIssuesSearch"] = null;
		Session["TypeSearch"] = null;
		Session["AdminAction"] = "0";
		Session["VMIAction"] = "1";
		Session["TicketSearchString"] = "";
		lbl_error.Text			= "";
		lbl_error.Style.Clear();
		populate_groups();
		var errors		= new List<string>();
		var is_error			= false;
		var ticket_type			= 0;
		if(ddl_type.Value != null)
			{
			int.TryParse(ddl_type.Value.ToString(), out ticket_type);
			}
		var ticket_group		= 0;
		if(ddl_group.Value != null)
			{
			int.TryParse(ddl_group.Value.ToString(), out ticket_group);
			}
		var ticket_owner		= 0;
		int.TryParse(ddloo.SelectedValue, out ticket_owner);
		var ticket_priority		= 1;
		int.TryParse(ddlSeverity.SelectedValue, out ticket_priority);
		var ticket_module		= 0;
		if(ddl_page.Value != null)
			{
			int.TryParse(ddl_page.Value.ToString(), out ticket_module);
			}
		var ticket_assigned		= 0;
		int.TryParse(ddlUser.SelectedValue, out ticket_assigned);
		if (ticket_type == 0)
			{
			is_error		= true;
			errors.Add("Please select a type of issue.");
			}
		if (ticket_module == 0)
			{
			is_error		= true;
			errors.Add("Please select a subgroup that this ticket would pertain to.");
			}
		if (ticket_group == 0)
			{
			is_error		= true;
			errors.Add("Please select a group that this ticket would fall under.");
			}
		if (tb_search.Text.Trim() == "")
			{
			is_error		= true;
			errors.Add("You must have a subject for the ticket.");
			}
		if (ticket_type == 6 && current_user.id != 8)
			{
			is_error		= true;
			errors.Add("You are not authorized to cut a leadership team objective... see Andy for details");
			}
		if (ticket_type == 6 && ticket_owner == 0)
			{
			is_error		= true;
			errors.Add("If this is a leadership team objective, you must select an objective owner");
			}
		if (ticket_group == 1 && ticket_type == 1 && string.IsNullOrEmpty(txtIssue.Html.Trim()))
			{
			is_error		= true;
			errors.Add("If you are submitting an error with the system, you must provide detail to the issue, and preferably how to replicate the problem.");
			}
		if (ticket_group == 1 && ticket_type == 3 && string.IsNullOrEmpty(txtIssue.Html.Trim()))
			{
			is_error		= true;
			errors.Add("If you are submitting an request for change, you must provide detail to the your request.");
			}
		if(is_error)
			{
			lbl_error.Style.Add("padding", "5px");
			lbl_error.Style.Add("width", "80%");
			var max_width		= is_mobile ? "max-width:200px;" : "";
			lbl_error.Text += "<div style='font-size:.75em;"+max_width+"'>Please address the following errors: <br/><ul>";
			foreach(var s in errors)
				{
				lbl_error.Text += string.Format("<li> {0}", s); 
				}
			lbl_error.Text += "</li></div>"; 
			lbl_error.Visible = true;
			filMyFile.Visible = true;
			ddl_page.ClientEnabled				= true;
			ddl_type.ClientEnabled		= true;
			tb_search.ClientEnabled			= true;
			return;
			}

		var tickets                   = new NETickets();
		tickets.issue						= tb_search.Text;
		tickets.issuetype					= ticket_type;
		tickets.createdby_member_id			= current_user.id;
		tickets.objective_owner				= (int) ticket_owner;
		tickets.priority_id					= ddlUser.Visible ? ticket_priority : 1;
		tickets.module_id					= ticket_module;
		tickets.is_private					= chkPrivate.Checked;
		tickets.member_assigned_id			= ddlUser.Visible ? (int) ticket_assigned : 0;
		if (dteexpected.Text != "")
			{
			tickets.expected_completion		= dteexpected.Date;
			}
		tickets.save();
		
		var	issue			= new NETickets.ticketissue();
		issue.ticketheader_id			= tickets.id;
		issue.message					= txtIssue.Html.Trim() == "" 
											? "(Message body not provided)"
											: txtIssue.Html;
		issue.created_member_id			= current_user.id;
		issue.created_date				= DateTime.Now;
        	issue.save();
        
		
		if (!string.IsNullOrEmpty(_q["ms_id"]))
			{
				Toolbox.doSQL_void(@"UPDATE memberoffer_milestones SET ticketid = @v0  WHERE id =@v1  LIMIT 1",
					new object[] { tickets.id, _q["ms_id"]} );
			}

		if (ticket_assigned > 0)
			{
			if (current_user.id != ticket_assigned)
				{
				#region Send The Assigned User An Email
				var Assignemail = Toolbox.doSQL_string(@"SELECT member_neemail FROM member WHERE member_id =@v0 ",new object[] {  ddlUser.SelectedValue});
				var mail = new NeEMail();
				mail.To = Assignemail;
				mail.Subject = "You have been assigned Ticket# " + tickets.id;
				mail.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
				mail.isHTML = true;
				mail.Body = string.Format("The Ticket titled {0} has been assigned to you. Please review this ticket and then get back to the user as soon as possible", tb_search.Text);
				mail.Send();
				#endregion
				}
			}

		try
			{
			if (hiddenshot.Value.Trim() != "")
				{
				var image				= Convert.FromBase64String(hiddenshot.Value.Split(',')[1]);
				NETickets.ticketissue.add_attachment(issue.id, image, image.Length, issue.id.ToString(), "png"); 
				issue.uploaded_file			= issue.id+".png";
				issue.save();
				}
			else if (filMyFile.PostedFile != null && filMyFile.PostedFile.ContentLength > 0)
				{
				var file			= filMyFile.PostedFile;
				var buffer				= new byte[file.ContentLength];
				file.InputStream.Read(buffer, 0, file.ContentLength);

                var name					= System.IO.Path.GetFileName(file.FileName);
				var extension			= System.IO.Path.GetExtension(file.FileName).ToLower();

                NETickets.ticketissue.add_attachment(issue.id, buffer, file.ContentLength, issue.id.ToString(), extension);

                issue.uploaded_file			= issue.id+extension;
				issue.save();
				}
			}
		catch (Exception Ex)
			{
			Toolbox.do_catch_error(Ex, 711);
			lbl_error.Visible = true;
			lbl_error.Text += "Failed to Upload File" + Ex;
			}
		NETickets.send_new_ticket_alert(tickets.id, current_user, tickets.issue, issue.message);

		if (!string.IsNullOrEmpty(_q["ms_id"]))
			{

			}
		else if(_q["from"] == "list")
			{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "Refresh", "window.parent.pop_new.Hide();window.parent.gv_tickets.Refresh();location.href='CreateIssue.aspx?from=list';alert('Ticket #"+tickets.id+" created');", true);
			}
		else if(is_mobile)
			{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "GoBack", "if(confirm('Ticket #"+tickets.id+" created. Would you like to create another?')){location.href = location.href;}else{location.href = '/mobile/index.aspx?a=tickets';}", true);
			}
		else
			{
			Response.Redirect("index.aspx");
			}
		}
	protected void lbCreateNewTicket_Click(object sender, EventArgs e)
		{
		try
			{
			ddl_page.Value = ddl_page.Value.ToString();
			}
		catch
			{
			lbl_error.Visible = true;
			lbl_error.Text = "Please Choose A Page Type to Submit";
			return;
			}
		gv_matches.Visible = is_mobile;
		txtIssue.Visible = true;
		pastebox.Visible = true;
		ticket_details.Visible = true;
		Label1.Visible = true;
		filMyFile.Visible = true;
		chkPrivate.Visible = true;
		btnSubmit.Visible = true;
		lbCreateNewTicket.Visible = false;
		if (is_leadship_team)
			{
			dteexpected.ClientVisible = true;
			lbl_selectpri0.ClientVisible = true;
			}
		var tickets = new NETickets();
		if (Toolbox.doSQL_int(@"Select ticket_group_administrator from ticket_group where ticket_group_id =@v0 " ,new object[] {  ddl_group.Value}) == current_user.id || current_user.id == 8)
			{
			ddlUser.Visible = true;
			ddlSeverity.Visible = true;
			lbl_selectass.Visible = true;
			lbl_selectpri.Visible = true;
			if (Request.QueryString["from"] != "milestone")
				{
				ddlUser.DataSource = tickets.GetTicketManagerList(Convert.ToInt32(ddl_group.Value));
				ddlUser.DataValueField = "ID";
				ddlUser.DataTextField = "NAME";
				ddlUser.DataBind();
				var lic3 = new ListItem("Select Assigned User", "0");
				ddlUser.Items.Insert(0, lic3);
				ddlUser.SelectedValue = "0";
				}


			ddlSeverity.DataSource = tickets.getticketpriotiylist();
			ddlSeverity.DataValueField = "ID";
			ddlSeverity.DataTextField = "NAME";
			ddlSeverity.DataBind();
			var lic4 = new ListItem("Select Severity", "0");
			ddlSeverity.Items.Insert(0, lic4);
			ddlSeverity.SelectedValue = "0";
			}
		if (ddl_type.Value.ToString() == "6")
			{
			ddloo.Visible = true;
			lbl_selectoo.ClientVisible = true;
			ddloo.DataSource = Toolbox.doSQL_dt(@"Select member_id id, get_name(member_id) name from member  where member_status = 'Active' order by member_fullname" , null);
			ddloo.DataValueField = "id";
			ddloo.DataTextField = "name";
			ddloo.DataBind();
			var lic5 = new ListItem("Select Objective Owner", "0");
			ddloo.Items.Insert(0, lic5);
			ddloo.SelectedValue = "0";
			}

		}
	protected void cb_main_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		if(!is_mobile)
			{
		if(e.Parameter == "group")
			{
			ddl_page.SelectedIndex				= -1;
			ddl_type.SelectedIndex			= -1;
			}
		if(e.Parameter == "reset")
			{
			populate_related();
			}
		else
			{
			var current_group					= Convert.ToInt32(ddl_group.Value);
			using(var uow = new UnitOfWork())
				{
				var x							= uow.GetObjectByKey<ne_xpo.cs.ticket_group>(current_group);
				var group_name				= x.ticket_group_name;
				recent_title.InnerHtml			= "<span style='color:#ddd;'>Recent tickets for: </span>"+group_name;
				}
			pnl_recent_tickets.Visible			= !is_mobile;
			ddl_page.ClientEnabled				= true;
			ddl_type.ClientEnabled		= true;
			tb_search.ClientEnabled			= true;
			populate_related();
			}
			}
		else
			{
			ddl_page.ClientEnabled				= true;
			ddl_type.ClientEnabled		= true;
			tb_search.ClientEnabled			= true;
			}
		}
	protected void btn_search_Click(object sender, EventArgs e)
		{
		SearchForIssues();
		}
	protected void dv_recent_CustomCallback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var paras				= e.Parameter.Split('|');
		var ticket_id				= 0;
		int.TryParse(paras[0], out ticket_id);
		var should_watch			= paras[1] == "1";
		if(should_watch) // Add watch
			{
			var tmv			= new NETickets.ticket_memberview();
			if(tmv.exists(ticket_id, (int) current_user.id) == 0)
				{
				tmv.ticket_id							= ticket_id;
				tmv.member_id							= (int) current_user.id;
				tmv.save();
				}
			}
		else // Remove watch
			{
			var tmv			= new NETickets.ticket_memberview(ticket_id, (int) current_user.id);
			tmv.delete();
			}
		populate_related();
		}
}
