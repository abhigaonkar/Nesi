using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Collections.Specialized;
using DevExpress.Xpo;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Net;
using nesi.core;

public partial class sections_member_tickets_ticketpage : Page
{
	Toolbox _tools;
	NeMember _current_user;
	NameValueCollection _q;
	NETickets _ticket;
	int _ticket_id = 0;
	int _group_admin_id = 0;
	bool _do_duplicate = false;
	bool _is_admin = false;
	bool _is_assignee = false;
	bool _is_creator = false;
	bool _user_tracking_ticket = false;
	bool _auth_for_leadership_team = false;
	bool _auth_for_release_system = false;
	bool _is_mobile = false;
	bool _is_masquerading = false;
	protected void Page_PreInit(object _sender, EventArgs _e)
	{
		_q = Request.QueryString;
		_is_mobile = !string.IsNullOrEmpty(_q["is_mobile"]) || Page.Request.Browser.IsMobileDevice;
	}
	protected void Page_Init(object _sender, EventArgs _e)
	{
		_tools = new Toolbox();
		_tools.dont_cache_page();
		_tools.page_author = new NeMember(711);
		_ticket = new NETickets();
		_current_user = Toolbox.do_handle_authentication(170);
		_tools.current_user = _current_user;
		_auth_for_leadership_team = _current_user.AuthenticatedForPrivilege(131);
		_auth_for_release_system = _current_user.AuthenticatedForPage(166);
		int.TryParse(_q["issue"], out _ticket_id);
		bool.TryParse(_q["duplicate"], out _do_duplicate);

	    wiki_help.Visible = true;
	    wiki_help.Attributes.Add("onclick", string.Format("page_obj.pop_wiki_help({0});", 170));

        var cookieIsMasq = "0";
		if (Request.Cookies["ismasq"]!=null)
		{
			cookieIsMasq = Request.Cookies["ismasq"].Value;
		}
		if (cookieIsMasq == "1" || (Session["masq"] != null && Session["orig_masq_user"] != null && _current_user.id != (int)Session["orig_masq_user"]))
		{
			btnaddchat.Enabled = false;
			btnaddchat.Text = "Button disabled while masquerading";
			_is_masquerading = true;
		}
		if (_ticket_id == 0 || !_ticket.exists(_ticket_id))
		{
			Toolbox.FriendlyException(Response, "Invalid Ticket Number", "window.close();");
		}
		if (_ticket_id > 0 && !_do_duplicate)
		{
			_ticket = new NETickets(_ticket_id);
		}
	}
	protected void Page_Load(object _sender, EventArgs _e)
	{
		if (_is_mobile)
		{
			viewport.Text = "<meta content='width=device-width, initial-scale=1, maximum-scale=1' name='viewport' />";
			bt_mobile_back.Visible = true;
			txtMessage.Visible = false;
			mobile_chat_message.Visible = true;
			paste_row.Visible = false;
			chat_pop.Style.Add("overflow", "auto");
			tb_pastebox.Visible = false;
			combo_assigned.Native = true;
			combo_created.Native = true;
			combo_group.Native = true;
			combo_priority.Native = true;
			combo_status.Native = true;
			combo_status_popup.Native = true;
			combo_availableusers.Native = true;
			ddl_page.Native = true;
			ddl_type.Native = true;
			popnewchat.Width = Unit.Percentage(100);
			popnewchat.Height = Unit.Percentage(100);
			popnewchat.AutoUpdatePosition = true;
			popnewchat.AllowDragging = false;
			chkPrivate.Native = true;
			btnSubmit.Style.Add("margin-top", "25px");
			js_handler.Style.Add("width", "100%");
			js_handler.Style.Add("height", "100px");
			js_handler.Style.Add("float", "left");
			//js_handler.InnerHtml	= "<script type='text/javascript'>$(document).ready(function(){window.parent.mobile_ticket.resize_frame();});</script>";
		}
		using (var uow = new UnitOfWork())
		{
			txtMessage.Style["resize"] = "auto";
			if (_ticket_id > 0 && !_do_duplicate)
			{
				populate_header_info();
			}
			if (!IsPostBack && !IsCallback && _ticket.id > 0 && !_do_duplicate)
			{
				#region populate ticket
				if (_ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_id > 1 && _current_user.AuthenticatedForPrivilege(164))
				{
					bt_duplicate.ClientSideEvents.Click = string.Format("function(s,e){{location.href='./ticketpage.aspx?issue={0}&duplicate=true';}}", _ticket_id);
					bt_duplicate.Visible = true;
				}
				else
				{
					row_duplicate.Visible = false;
				}

				#region Move from assigned to read when the assignee has first opened the ticket
				if (_ticket.status_id == 8 && _is_assignee)
				{
					_ticket.status_id = 6;
					_ticket.modified_date = DateTime.Now;
					_ticket.save();
				}
				#endregion Move from assigned to read when the assignee has first opened the ticket

				display_ticket_information();
				populate_ddl(combo_group, uow);
				populate_ddl(combo_created, uow);
				populate_ddl(combo_assigned, uow);
				populate_ddl(combo_objectiveowner, uow);
				populate_ddl(combo_priority, uow);
				populate_ddl(combo_status, uow);
				populate_ddl(combo_status_popup, uow);
				populate_ddl(ddl_page, uow);
				populate_ddl(combo_waitinguser, uow);
				populate_ddl(combo_waitingonuser_popup, uow);
				Page.Title = "Ticket " + _ticket_id + " " + _ticket.issue;

				//bt_ticketfiles.Visible = _auth_for_release_system;
				//if (bt_ticketfiles.Visible)
				//	{
				//	bt_ticketfiles.ClientSideEvents.Click = string.Format("function(s,e){{boing('./ticket_files.aspx?ticket_id={0}', 'ticketfiles_{0}', 340, 740);}}", _ticket_id);
				//	}

				#endregion populate ticket
			}
			else if (_do_duplicate && _ticket_id > 0 && !IsPostBack && !IsCallback)
			{
				#region Duplicate request
				// Dupe Header
				var dupe_ticket = new NETickets(_ticket_id);
				dupe_ticket.id = 0;
				dupe_ticket.save();
				var new_ticket_id = dupe_ticket.id;
				// Dupe first issue, if applicable
				var dupe_issue_c = Toolbox.doSQL_int(@"SELECT IFNULL(MIN(ticketissues_id),0) id FROM ticketissues 
WHERE ticketissues_ticketheader_id = @v0 AND ticketissues_created_member_id = @v1", new object[] { _ticket_id, dupe_ticket.createdby_member_id });
				if (dupe_issue_c > 0)
				{
					var issue = new NETickets.ticketissue(dupe_issue_c);
					issue.id = 0;
					issue.id = new_ticket_id;
					issue.save();
				}
				// Dupe Tasks, Set all tasks to false
				var dupe_tasks_c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM ticket_tasks WHERE ticket_id = @v0", new object[] { _ticket_id });
				if (dupe_tasks_c > 0)
				{
					Toolbox.doSQL_void(@"INSERT INTO ticket_tasks (ticket_id, task, complete, _order, cost) 
SELECT @v1, task, 0, _order, cost FROM ticket_tasks WHERE ticket_id = @v0", new object[] { _ticket_id, new_ticket_id });
				}
				Response.Redirect("./ticketpage.aspx?issue=" + new_ticket_id, true);
				#endregion Duplicate request
			}
		}
	}
	private void populate_header_info()
	{
		using (var uow = new UnitOfWork())
		{
			_group_admin_id = _ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_id;
			_is_admin = _current_user.id == _group_admin_id || _current_user.id == 8;
			_is_assignee = _current_user.id == _ticket.member_assigned_id;
			_is_creator = _current_user.id == _ticket.createdby_member_id;

			_user_tracking_ticket = (from ut in new XPQuery<ne_xpo.cs.ticket_memberview>(uow)
									 where
										 ut.ticket_id == uow.GetObjectByKey<ne_xpo.cs.ticketheader>(_ticket_id) &&
										 ut.member_id == uow.GetObjectByKey<ne_xpo.cs.member>(_current_user.id)
									 select new { ut.id }).Any();
			col_details.Visible = _is_admin || _is_assignee || _is_creator;
			combo_created.ClientEnabled = _is_admin || _is_assignee;
			col_tasks.Visible = _is_admin || _is_assignee || _is_creator || _user_tracking_ticket;// || user_tracking_ticket;
			client_cbp_left.Visible = !_is_admin && !_is_assignee && !_is_creator;
			row_vote.Visible = _ticket.issuetype == 3 && !Toolbox.Contains(_ticket.status_id, new int[] { 5, 7, 10, 11, 12 });
			admin_details.Visible = _is_admin || _is_assignee;
			col_trackers.Visible = _is_creator || _is_admin || _is_assignee;
			hl_closeticket.ClientVisible = _ticket.status_id != 5 && _ticket.createdby_member_id == _current_user.id;
			hl_requestreopen.ClientVisible = _ticket.status_id == 5;
			hl_requestreopen.Text = _is_creator ? "Reopen ticket" : "Request Reopen of this ticket";
			pop_reopen_ticket.HeaderText = _is_creator ? "Reason for reopening?" : "Why do you want this ticket reopened?";
			btn_reopenticket.Text = _is_creator ? "Reopen Ticket" : "Request Reopen";
			var x = from y in new XPQuery<ne_xpo.cs.ticket_group>(uow)
					where y.ticket_group_administrator.member_id == _current_user.id &&
					(y.ticket_group_id == 1 || y.ticket_group_id == 2 || y.ticket_group_id == 30 || y.ticket_group_id == 31)
					select y;
			var is_nesi_group_admin = false;
			if (x.Any())
			{
				foreach (var xx in x)
				{
					if (Toolbox.Contains(xx.ticket_group_id, new[] { 1, 2, 30, 31 }))
					{
						is_nesi_group_admin = true;
					}
				}
			}
			if (_user_tracking_ticket && !_is_admin && !_is_assignee && !_is_creator)
			{
				col_clientdetails.Style["height"] = "250px";
				client_details_body.Style["min-height"] = "0px";
			}
			if (_ticket.is_private && !_is_assignee && !_is_admin && !_is_creator && !_user_tracking_ticket)
			{
				Toolbox.FriendlyException(Response, "Restricted, this ticket is marked as private.", "./index.aspx");
			}

			if (!_is_admin && !_is_assignee && !_is_creator)
			{
				// Set up the client panel - Only header data, small interaction
				lbl_client_number.Text = _ticket.id.ToString();
				lbl_client_subject.Text = _ticket.issue;
				lbl_client_type.Text = _ticket.xpo_ref.ticketheader_issuetype.tickettype_name;
				lbl_client_assigned.Text = _ticket.xpo_ref.ticketheader_member_assigned_id == null
														? "- Not Set -"
														: _ticket.xpo_ref.ticketheader_member_assigned_id.member_fullname;
				lbl_client_created.Text = _ticket.xpo_ref.ticketheader_createdby_member_id.member_fullname;
				lbl_client_group.Text = _ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_name;
				lbl_client_pertaining.Text = _ticket.xpo_ref.ticketheader_module_id.ticketpage_name;
				var waitinguser = _ticket.xpo_ref.ticketheader_waitingon_member_id == null
														? _ticket.xpo_ref.ticketheader_createdby_member_id.member_fullname
														: _ticket.xpo_ref.ticketheader_waitingon_member_id.member_fullname;
				bt_assigntome.Visible = Toolbox.Contains(_ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_id, new[] { 1, 30, 31 }) && !_is_admin && is_nesi_group_admin && Toolbox.Contains(_ticket.status_id, new[] { 1, 2, 3, 6, 8, 9 });
				bt_movegroup.Visible = Toolbox.Contains(_ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_id, new[] { 1, 30, 31 }) && !_is_admin && is_nesi_group_admin && Toolbox.Contains(_ticket.status_id, new[] { 1, 2, 3, 6, 8, 9 });
				switch (_ticket.status_id)
				{
					case 3:
						lbl_client_status.Text = "Waiting for response from " + waitinguser;
						break;
					case 9:
						lbl_client_status.Text = "Answered by " + waitinguser;
						break;
					default:
						lbl_client_status.Text = _ticket.xpo_ref.ticketheader_status_id.ticketstatus_status;
						break;
				}
				chk_client_follow.ClientEnabled = _ticket.status_id != 5;
				hl_requestreopen_client.ClientVisible = _ticket.status_id == 5;
				hl_requestreopen_client.Text = _is_creator ? "Reopen ticket" : "Request Reopen of this ticket";
				chk_client_follow.Checked = _user_tracking_ticket;
				fill_votes(true);
			}
		}
	}
	private void fill_votes(bool _is_client)
	{
		if (_is_client)
		{
			if (client_row_vote.Visible)
			{
				var v = Toolbox.doSQL_int(@"SELECT total_votes(@v0)", _ticket.id);
				client_icon_holder_wrapper.InnerHtml = string.Format("<b style='position:relative;right:10px;'>{0}</b>", v);
				var voted = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM ticket_vote WHERE ticketheader_id = @v0 AND member_id = @v1",
					new object[] { _ticket.id, _current_user.id });

				client_icon_holder_wrapper.Attributes["onclick"] = string.Format("local_ticket.vote({0},{1});", _ticket.id, 1 - voted);
				client_icon_holder_wrapper.Attributes["title"] = voted == 1
														? "Click to remove your vote from this ticket"
														: "Click to add your vote to this ticket!\nThe higher the rank, the sooner it will be done.";
				var current_class = client_icon_holder_wrapper.Attributes["class"];
				if (voted == 1 && !current_class.EndsWith("voted"))
				{
					client_icon_holder_wrapper.Attributes["class"] += " voted";
				}
				else if (current_class.EndsWith("voted"))
				{
					client_icon_holder_wrapper.Attributes["class"] = current_class.Substring(0, current_class.LastIndexOf("voted")); ;
				}
				current_class = client_icon_holder_wrapper.Attributes["class"];
			}
		}
		else
		{
			if (row_vote.Visible)
			{
				var v = Toolbox.doSQL_int(@"SELECT total_votes(@v0)", new object[] { _ticket.id });
				icon_holder_wrapper.InnerHtml = string.Format("<b style='position:relative;right:10px;'>{0}</b>", v);
				var voted = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM ticket_vote WHERE ticketheader_id = @v0 AND member_id = @v1",
					new object[] { _ticket.id, _current_user.id });

				icon_holder_wrapper.Attributes["onclick"] = string.Format("local_ticket.vote({0},{1});", _ticket.id, 1 - voted);
				icon_holder_wrapper.Attributes["title"] = voted == 1
														? "Click to remove your vote from this ticket"
														: "Click to add your vote to this ticket!\nThe higher the rank, the sooner it will be done.";
				var current_class = icon_holder_wrapper.Attributes["class"];
				if (voted == 1 && !current_class.EndsWith("voted"))
				{
					icon_holder_wrapper.Attributes["class"] += " voted";
				}
				else if (current_class.EndsWith("voted"))
				{
					icon_holder_wrapper.Attributes["class"] = current_class.Substring(0, current_class.LastIndexOf("voted")); ;
				}
			}
		}

	}
	private void populate_ddl(Control _c, UnitOfWork _uow)
	{
		var member_select = @"SELECT member_id id, member_fullname name FROM member WHERE member_status = 'Active' AND member_nickname != '' ORDER BY member_fullname";
		switch (_c.ID)
		{
			// TODO Move to XPO - Not currently doing so because of member_status being an ENUM datatype
			case "combo_created":
				combo_created.DataSource = Toolbox.doSQL_dt(member_select, null);
				combo_created.DataBind();
				break;
			case "combo_assigned":
				combo_assigned.DataSource = _ticket.GetTicketManagerList(_ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_id);
				combo_assigned.DataBind();
				break;
			case "combo_objectiveowner":
				combo_objectiveowner.DataSource = Toolbox.doSQL_dt(member_select, null);
				combo_objectiveowner.DataBind();
				break;
			case "combo_priority":
				combo_priority.DataSource = (from pr in new XPQuery<ne_xpo.cs.ticketpriority>(_uow)
											 select new
											 {
												 id = pr.ticketpriority_id,
												 name = pr.ticketpriority_name
											 }).ToList().OrderBy(_x => _x.name);
				combo_priority.DataBind();
				break;
			case "combo_status":
				var status_list = (from st in new XPQuery<ne_xpo.cs.ticketstatus>(_uow)
								   select new
								   {
									   id = st.ticketstatus_id,
									   name = st.ticketstatus_status
								   }).ToList();
				combo_status.Items.Clear();
				foreach (var x in status_list)
				{
					var group_id = (int)combo_group.Value;
					var it_statuses = new int[] { 10, 11, 12 };
					var it_groups = new int[] { 1, 30 };
					var is_it_group = Toolbox.Contains(group_id, it_groups);
					var is_it_status = Toolbox.Contains(x.id, it_statuses);

					if (is_it_status)
					{
						if (is_it_group)
						{
							combo_status.Items.Add(x.name, x.id);
						}
					}
					else
					{
						combo_status.Items.Add(x.name, x.id);
					}
				}
				combo_status.DataBind();
				break;
			case "ddl_page":
				ddl_page.DataSource = Toolbox.doSQL_dt(@"SELECT ticketpage_id,ticketpage_name FROM ticketpage WHERE ticketpage_ticket_group_id = @v0  ORDER BY ticketpage_name", new object[] { combo_group.Value });
				ddl_page.DataBind();
				break;
			case "combo_status_popup":
				var dt = (from st in new XPQuery<ne_xpo.cs.ticketstatus>(_uow)
						  where (st.ticketstatus_id != 9 || _ticket.status_id == 9)
						  select new
						  {
							  id = st.ticketstatus_id,
							  name = st.ticketstatus_status
						  }).ToList();
				combo_status_popup.DataSource = dt;
				combo_status_popup.DataBind();
				break;
			case "combo_group":
				combo_group.DataSource = (from ad in new XPQuery<ne_xpo.cs.ticket_group>(_uow)
										  where !_auth_for_leadership_team ? ad.ticket_group_id != 4 : true
										  select new
										  {
											  id = ad.ticket_group_id,
											  name = ad.ticket_group_name + " (" + ad.ticket_group_administrator.member_fullname + ")",
											  order = ad.order_n
										  }).ToList().OrderBy(_x => _x.order);
				combo_group.DataBind();
				break;
			case "combo_waitinguser":
				try
				{
					var users = new Dictionary<int, string>();
					// Group Admin
					users.Add(_ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_id,
						_ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_fullname);
					// Ticket Creator
					if (!users.ContainsKey(_ticket.createdby_member_id))
					{
						users.Add(_ticket.createdby_member_id,
								_ticket.xpo_ref.ticketheader_createdby_member_id.member_fullname);
					}
					// Ticket assignee
					if (_ticket.member_assigned_id != 0 && !users.ContainsKey(_ticket.member_assigned_id))
					{
						users.Add(_ticket.member_assigned_id, _ticket.xpo_ref.ticketheader_member_assigned_id.member_fullname);
					}
					// Watchers
					var watchers = from wa in new XPQuery<ne_xpo.cs.ticket_memberview>(_uow)
								   where wa.ticket_id == _uow.GetObjectByKey<ne_xpo.cs.ticketheader>(_ticket.id)
								   select new
								   {
									   id = wa.member_id.member_id,
									   name = wa.member_id.member_fullname
								   };
					foreach (var wa in watchers)
					{
						if (!users.ContainsKey(wa.id))
						{
							users.Add(wa.id, wa.name);
						}
					}
					combo_waitinguser.DataSource = users;
					combo_waitinguser.DataBind();
				}
				catch (Exception ee)
				{
					_tools.catch_error(ee);
				}
				break;
			case "combo_waitingonuser_popup":
				var users_popup = new Dictionary<int, string>();
				var chk = _ticket.xpo_ref;
				var chk2 = _ticket.xpo_ref.ticketheader_module_id;
				var chk3 = _ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id;
				var chk4 = _ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator;
				var group_admin = _ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator;

				// Group Admin
				if (group_admin != null)
				{
					users_popup.Add(group_admin.member_id, group_admin.member_fullname);
				}
				// Ticket Creator
				if (!users_popup.ContainsKey(_ticket.createdby_member_id))
				{
					users_popup.Add(_ticket.createdby_member_id,
							_ticket.xpo_ref.ticketheader_createdby_member_id.member_fullname);
				}
				// Ticket assignee
				if (_ticket.member_assigned_id != 0 && !users_popup.ContainsKey(_ticket.member_assigned_id))
				{
					users_popup.Add(_ticket.member_assigned_id, _ticket.xpo_ref.ticketheader_member_assigned_id.member_fullname);
				}
				// Watchers
				var watchers_popup = from wa in new XPQuery<ne_xpo.cs.ticket_memberview>(_uow)
									 where wa.ticket_id == _uow.GetObjectByKey<ne_xpo.cs.ticketheader>(_ticket.id)
									 select new
									 {
										 id = wa.member_id.member_id,
										 name = wa.member_id.member_fullname
									 };
				foreach (var wa in watchers_popup)
				{
					if (!users_popup.ContainsKey(wa.id))
					{
						users_popup.Add(wa.id, wa.name);
					}
				}
				combo_waitingonuser_popup.DataSource = users_popup;
				combo_waitingonuser_popup.DataBind();
				break;
		}
	}
	private void display_ticket_information()
	{
		#region Create Ticket Header 
		lblIssueIDDisp.Text = _ticket_id.ToString();
		combo_created.Value = _ticket.createdby_member_id;
		chkPrivate.Checked = _ticket.is_private;
		lblDateCreateDisp.Text = string.Format("<span title='{1}'>{0}</span>", Toolbox.MySQL_longdt(_ticket.created_date), Toolbox.MySQL_longdt(_ticket.created_date));
		lblDateModDisplay.Text = string.Format("<span title='{1}'>{0}</span>", Toolbox.MySQL_longdt(_ticket.modified_date), Toolbox.MySQL_longdt(_ticket.modified_date));
		row_assigned_date.Visible = _ticket.member_assigned_id > 0 && _ticket.assigned_date.Year > 2000;
		lblasndateDisp.Text = row_assigned_date.Visible ? string.Format("<span title='{1}'>{0}</span>", Toolbox.MySQL_longdt(_ticket.assigned_date), Toolbox.MySQL_longdt(_ticket.assigned_date)) : "";
		row_closed_date.Visible = _ticket.status_id == 5;
		lblCloseDisp.Text = row_closed_date.Visible ? string.Format("<span title='{1}'>{0}</span>", Toolbox.MySQL_longdt(_ticket.closed_date), Toolbox.MySQL_longdt(_ticket.closed_date)) : "";
		txtexphours.Text = _ticket.expected_hours.ToString();
		dtefinish.Date = _ticket.expected_completion.Year > 2000 ? _ticket.expected_completion : new DateTime();
		ddl_page.Value = _ticket.xpo_ref.ticketheader_module_id.ticketpage_id;
		combo_status.Value = _ticket.status_id;
		//row_release.Visible				= _ticket.release_id > 0;
		//lblrelease.Text					= row_release.Visible ? _ticket.release_id.ToString() : "";
		row_waiting.Style["display"] = _ticket.status_id == 3 || _ticket.status_id == 9 ? "block" : "none";
		combo_waitinguser.Value = _ticket.waiting_on_member_id;
		combo_status_popup.Value = _ticket.status_id;
		combo_status.Value = _ticket.status_id;
		combo_assigned.Value = _ticket.member_assigned_id;
		combo_objectiveowner.Value = _ticket.objective_owner;
		combo_created.Value = _ticket.createdby_member_id;
		combo_priority.Value = _ticket.priority_id;
		lblhoursdisp.Text = _ticket.hours_worked.ToString();
		memo_cause.Text = _ticket.cause;
		memo_roi.Text = _ticket.roi;
		memo_solution.Text = _ticket.solution;
		memo_symptom.Text = _ticket.symptom;
		memo_notes.Text = _ticket.notes;
		fill_votes(false);
		combo_group.Value = _ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_id;
		ddl_type.Value = _ticket.issuetype;
		row_close_ticket.Visible = _is_creator && _ticket.status_id != 5;
		memo_subject.Text = _ticket.issue;
		popup_row_manually_set_status.Visible = _is_admin || _is_assignee;
		popup_hours.Visible = _is_admin || _is_assignee;
		#endregion
		#region Format Message Information for Display
		var sb_messages = new StringBuilder();
		var issue_list = NETickets.get_ticket_issues(_ticket.id);
		using (var uow = new UnitOfWork())
		{
			foreach (var ti in issue_list)
			{
				var created_user = uow.GetObjectByKey<ne_xpo.cs.member>(ti.created_member_id);
				var user_company = created_user.business_unit_id;
				var bcolor = created_user.member_id == _ticket.member_assigned_id ? "FFFF99" : "99CCFF";
				var bbcolor = created_user.member_id == _ticket.member_assigned_id ? "FFFFC2" : "CCE6FF";
				var fcolor = "000";
				var start = "<td align='left'>";
				var end = "</td><td width=10></td>";
				if (created_user.member_id != _ticket.member_assigned_id)
				{
					start = "<td width=10></td><td align = 'right'>";
					end = "</td>";
				}

				sb_messages.AppendFormat(@"
<div class='message'>
	<div class='m_title noselect' style='background-color:#{3};padding:5px;color:#{4};cursor:pointer;' onclick='handle_collapse(this);'>
		<b>{1} {5} - {2}</b><br/>
		<div style='font-size:11px;' class='date' title='{0}'>{8}</div>
	</div>
	<div class='m_body' align='left'> ",
					Toolbox.MySQL_longdt(ti.created_date),                       // {0}
					created_user.member_fullname,           // {1}
					user_company.ddl_name,              // {2}
					bcolor,                                 // {3}
					"#000",                                 // {4}
					string.IsNullOrEmpty(created_user.member_phoneextension) ? "" : "(Ext: " + created_user.member_phoneextension + ")",    // {5}
					start,                              // {6}
					bbcolor,                                // {7}
					Toolbox.MySQL_longdt(ti.created_date)       // {8}
					);

				if (ti.uploaded_file != "")
				{
					if (ti.uploaded_file.Contains(".png") || ti.uploaded_file.Contains(".jpg") || ti.uploaded_file.Contains(".gif"))
					{
						sb_messages.AppendFormat(@"
<img src='./include.ashx?file={0}' onclick='local_ticket.handle_image(this, event);' data-id='{1}' data-file=""{0}"" class='attached_image' title='Shift+click for new window, click for fullscreen of image' /><br/>
	", ti.uploaded_file, ti.id);
					}
					else if (ti.uploaded_file.Contains(".webm") || ti.uploaded_file.Contains(".mp4") || ti.uploaded_file.Contains(".flv"))
					{
						sb_messages.AppendFormat(@"
<video style='max-width:300px;border:solid 1px #000;cursor:pointer' controls>
<source src='./include.ashx?file={0}' type='video/mp4'>
</video><br/>
	", ti.uploaded_file, ti.id);
					}
					else
					{
						sb_messages.AppendFormat(@"
<button type='button' onclick=""$('#downloader').attr('src','./include.ashx?file={0}');"">
	<img src='/images/icon/icon[attachment].gif' align='absmiddle' />
	<b style='font-size:10px'>Included File</b>
</button>
<br />", ti.uploaded_file);

					}
				}
				ti.message = ti.message.Contains("<br")
									? ti.message + "</div></div>"
									: ti.message.Replace("\n", "<br />") + "</div></div>";
				ti.message = ti.message.Contains("<img src=\"data:image")
									? ti.message.Replace("<img src=", "<br /><br /><img onclick='local_ticket.handle_image(this, event);' data-id='' data-file='' class='attached_image' title='Click for fullscreen of image' src=")
									: ti.message;
				sb_messages.Append(ti.message);
			}
			lblInformation.Text = sb_messages.ToString();
		}
		#endregion
		#region Display Page Controls Based on User and Privilege
		if (_ticket.status_id != 5)
		{
			btnaddchat.ClientEnabled = true;
		}
		if (!_is_mobile)
		{
			btn_files.Visible = true;
			pop_ticket_files.ClientSideEvents.PopUp = string.Format(@"
			function(s,e)
				{{
				var if_f = $('.if_files');
				var if_src = if_f.attr('src');
				if(if_src == '' || if_src === undefined)
					{{
					if_f.attr('src', '/FileManager.aspx?parent_page=ticket_attachments&id={0}');
					}}
				}}", _ticket.id);
		}

		if (_is_creator && _ticket.status_id != 5)  // if the user is the ticket creator...
		{
			combo_created.ClientEnabled = true;
			if (_ticket.status_id == 3)
			{
				combo_status_popup.Value = 3;
				combo_waitingonuser_popup.Value = _ticket.member_assigned_id != 0 ? _ticket.member_assigned_id : _ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_id;
			}
			else if (_ticket.status_id == 2 && _is_assignee)
			{
				combo_status_popup.Value = 3;
				combo_waitingonuser_popup.Value = _ticket.createdby_member_id;
			}
			else
			{
				combo_status_popup.Value = _ticket.status_id;
				combo_waitingonuser_popup.Value = _ticket.waiting_on_member_id;
			}
			combo_group.ClientEnabled = true;
			ddl_page.ClientEnabled = true;
			ddl_type.ClientEnabled = true;
		}

		// if it s the adminstrator/assignee/creator.. set the status options
		if ((_is_admin || _is_assignee || _is_creator) && _ticket.status_id != 5)  // if the ticket group admin is the user
		{
			combo_created.ClientEnabled = true;
			combo_status_popup.Enabled = true;
			bt_saveticket.Visible = true;
			combo_group.ClientEnabled = true;
			ddl_page.ClientEnabled = true;
			ddl_type.ClientEnabled = true;
			combo_assigned.ClientEnabled = true;
			txtexphours.ClientEnabled = true;
			combo_priority.ClientEnabled = true;
			memo_subject.ClientEnabled = true;
			chkPrivate.ClientEnabled = true;
			ddl_type.ClientEnabled = true;
			dtefinish.ClientEnabled = true;
			combo_availableusers.ClientEnabled = true;
			bt_savetracker.ClientEnabled = true;
			list_trackers.ClientEnabled = true;
			//bt_ticketfiles.ClientEnabled			= true;
			memo_new_task.ClientEnabled = true;
			bt_add_task.ClientEnabled = true;
		}
		pc_chat.TabPages[5].Visible = _is_assignee;
		switch (_ticket.issuetype)
		{
			case 1:
				pc_chat.TabPages[1].Visible = true;
				pc_chat.TabPages[2].Visible = true;
				pc_chat.TabPages[3].Visible = true;
				break;
			case 2:
				break;
			case 3:
				pc_chat.TabPages[4].Visible = true;
				break;
			case 4:
				pc_chat.TabPages[1].Visible = true;
				pc_chat.TabPages[2].Visible = true;
				pc_chat.TabPages[3].Visible = true;
				break;
			case 5:
				pc_chat.TabPages[1].Visible = true;
				pc_chat.TabPages[2].Visible = true;
				pc_chat.TabPages[3].Visible = true;
				pc_chat.TabPages[4].Visible = true;
				break;
			case 6:
				pc_chat.TabPages[1].Visible = true;
				pc_chat.TabPages[2].Visible = true;
				pc_chat.TabPages[3].Visible = true;
				pc_chat.TabPages[4].Visible = true;
				break;
		}
		if (!_is_admin && !_is_assignee)
		{
			memo_cause.ClientEnabled = false;
			memo_solution.ClientEnabled = false;
			memo_symptom.ClientEnabled = false;
			memo_roi.ClientEnabled = false;
			btn_cause.Visible = false;
			btn_roi.Visible = false;
			btn_solution.Visible = false;
			btn_symptom.Visible = false;
		}
		// if the user is the ticket assignee
		default_status(_ticket_id);
		var popup_status = (int)combo_status_popup.Value;
		if (Toolbox.Contains(popup_status, new int[] { 3, 9 }))
		{
			var should_display = popup_row_manually_set_status.Visible && Toolbox.Contains((int)combo_status_popup.Value, new int[] { 3, 9 });
			popup_row_waitingon_user.Style["display"] = should_display ? "block" : "none";
			if (should_display)
			{
				if (popup_status == 3)
				{
					combo_waitingonuser_popup.Value = _ticket.createdby_member_id;
				}
				else
				{
					combo_waitingonuser_popup.Value = _ticket.member_assigned_id == 0 ? _ticket.createdby_member_id : _ticket.member_assigned_id;
				}
			}
		}
		else
		{
			combo_waitingonuser_popup.Value = _ticket.createdby_member_id;
		}
		popup_row_waitingon_user.Style["display"] = popup_row_manually_set_status.Visible && Toolbox.Contains((int)combo_status_popup.Value, new int[] { 3, 9 }) ? "block" : "none";
		if ((_is_admin || _is_assignee || _is_creator) && _ticket.status_id != 5)
		{
			combo_status_popup.Enabled = true;
			combo_priority.Enabled = true;
			txtexphours.Enabled = true;
			combo_status_popup.DataBind();
			if (Toolbox.Contains((int)combo_status_popup.Value, new int[2] { 6, 8 }) || _ticket.expected_hours == 0)
			{
				txtexphours.ClientEnabled = true;
			}
			//combo_status_popup.Items.FindByValue("1").Enabled = false;
			if (!_is_admin)  // the assignee is also the admin.. show the closed button still.
			{
				//combo_status_popup.Items.FindByValue("5").Enabled = false; 
			}
			bt_saveticket.Visible = true;
		}

		#endregion
		#region display date changes
		var date_miss = Toolbox.doSQL_int(@"Select count(ticketissues_id) from ticketissues where ticketissues_ticketheader_id =@v0   and ticketissues_new_date !=''",
			new object[] { _ticket_id });
		//if (date_miss>0)
		//	{
		//	lblmiss.Text = "This tickets date has been changed " + date_miss + " time(s)";
		//	}
		#endregion
		var group_id = Convert.ToInt32(combo_group.Value);
		pastebox.Visible = Toolbox.Contains(group_id, new int[] { 1, 30, 31 });
		popnewchat.CssClass = pastebox.Visible ? "new_chat_wpastebox" : "new_chat_wopastebox";
		fill_tasks(_ticket.status_id == 5);
	}
	private void fill_tasks(bool _is_closed)
	{
		using (var uow = new UnitOfWork())
		{
			var tasks = (from t in new XPQuery<ne_xpo.cs.ticket_tasks>(uow)
						 where t.ticket_id.ticketheader_id == _ticket_id
						 select new
						 {
							 id = t.id,
							 body = t.task,
							 complete = t.complete,
							 assignee = t.assignee == null ? 0 : Convert.ToInt32(t.assignee.member_id)
						 }).ToList();

			var users = new Dictionary<int, string>();
			// Group Admin
			users.Add(_ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_id,
						_ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_fullname);
			// Ticket Creator
			if (!users.ContainsKey(_ticket.createdby_member_id))
			{
				users.Add(_ticket.createdby_member_id,
						_ticket.xpo_ref.ticketheader_createdby_member_id.member_fullname);
			}
			// Ticket assignee
			if (_ticket.member_assigned_id != 0 && !users.ContainsKey(_ticket.member_assigned_id))
			{
				users.Add(_ticket.member_assigned_id, _ticket.xpo_ref.ticketheader_member_assigned_id.member_fullname);
			}
			// Watchers
			var watchers = from wa in new XPQuery<ne_xpo.cs.ticket_memberview>(uow)
						   where wa.ticket_id == uow.GetObjectByKey<ne_xpo.cs.ticketheader>(_ticket.id)
						   select new
						   {
							   id = wa.member_id.member_id,
							   name = wa.member_id.member_fullname
						   };
			foreach (var wa in watchers)
			{
				if (!users.ContainsKey(wa.id))
				{
					users.Add(wa.id, wa.name);
				}
			}

			foreach (var task in tasks)
			{
				var c = (NETickets.ticket_task.ticket_task_uc)LoadControl("./modules/ticket_task.ascx");
				c.id = task.id;
				c.body = task.body;
				c.complete = task.complete;
				c.enabled = _ticket.status_id != 5 && (_is_admin || _is_assignee || _current_user.id == task.assignee);
				c.assignee = task.assignee == 0 ? Convert.ToInt32(_ticket.member_assigned_id) : task.assignee;
				c.users = users;
				task_list.Controls.Add(c);
			}
		}
	}
	protected void default_status(int _ticketid)
	{
		var tickets = new NETickets(_ticketid);
		//combo_status_popup.SelectedValue = ticket.status_id.ToString();

		if (_ticket.createdby_member_id == _current_user.id)
		{
			switch (_ticket.status_id)
			{
				case 3:
					combo_status_popup.Value = 3;
					combo_waitingonuser_popup.Value = _ticket.member_assigned_id;

					break;
			}
		}
		if (_ticket.member_assigned_id == _current_user.id)
		{
			switch (_ticket.status_id)
			{
				case 1:
					combo_status_popup.Value = 2;
					break;
				case 9:
					combo_status_popup.Value = 2;
					break;
				case 8:
					combo_status_popup.Value = 2;
					break;
				case 6:
					combo_status_popup.Value = 2;
					break;
				case 2:
					combo_status_popup.Value = 3;
					break;
			}
		}

		if (Convert.ToInt32(_ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_id) == _current_user.id) // if it's the admin for the group
		{
			if (_ticket.createdby_member_id == _current_user.id)
			{
				switch (_ticket.status_id)
				{
					case 3:
						combo_status_popup.Value = 3;
						popup_row_waitingon_user.Style["display"] = "block";
						combo_waitingonuser_popup.Value = _ticket.createdby_member_id;
						break;
				}
			}
			else  // if the member is the group admin but not the creator
			{
				switch (_ticket.status_id)
				{
					case 9:
						combo_status_popup.Value = 3;
						popup_row_waitingon_user.Style["display"] = "block";
						combo_waitingonuser_popup.Value = _ticket.createdby_member_id;
						break;
				}
			}
		}
	}
	protected void btnSubmit_Click(object _sender, EventArgs _e)
	{
		if (_is_masquerading)
		{
			throw new Exception("You cannot submit ticket replies while masquerading");
		}
		lblError.Text = "";
		lblError.Visible = false;
		var used_msg = _is_mobile ? mobile_chat_message.Text : txtMessage.Html;
		if (combo_status_popup.SelectedItem != null && _ticket.status_id != Convert.ToInt32(combo_status_popup.Value) || combo_waitingonuser_popup.SelectedItem != null && combo_waitingonuser_popup.Value.ToString() != _ticket.waiting_on_member_id.ToString())
		{
			var pre_status_text = _ticket.status_id == 3  // 3 = waiting for user 
												? (_ticket.xpo_ref.ticketheader_waitingon_member_id == null
													? "Waiting for reply from " + _ticket.xpo_ref.ticketheader_createdby_member_id.member_fullname
													: "Waiting for reply from " + _ticket.xpo_ref.ticketheader_waitingon_member_id.member_fullname)
												: _ticket.status_id == 9  // answered by user
													? (_ticket.xpo_ref.ticketheader_waitingon_member_id == null
															? "Answered by " + _ticket.xpo_ref.ticketheader_createdby_member_id.member_fullname
															: "Answered by " + _ticket.xpo_ref.ticketheader_waitingon_member_id.member_fullname)
													: _ticket.xpo_ref.ticketheader_status_id.ticketstatus_status;
			var new_status_text = combo_status_popup.SelectedItem.Value.ToString() == "3"
												? "Waiting for reply from " + new NeMember(Convert.ToInt32(combo_waitingonuser_popup.Value)).FullName
												: combo_status_popup.SelectedItem.Value.ToString() == "9"
													? "Answered by " + new NeMember(Convert.ToInt32(combo_waitingonuser_popup.Value)).FullName
													: combo_status_popup.SelectedItem.Text;
			used_msg += "\n<hr noshade /> Updated Status from " + pre_status_text + " to " + new_status_text;
			_ticket.waiting_on_member_id = combo_waitingonuser_popup.Value == null ? _current_user.id : (int)combo_waitingonuser_popup.Value;
		}
		else
		{
			_ticket.waiting_on_member_id = _current_user.id;
		}

		if (!lblError.Visible)
		{
			lblError.Visible = false;
			lblError.Text = "";
			var issue = new NETickets.ticketissue();
			issue.ticketheader_id = _ticket.id;
			issue.message = used_msg;
			issue.created_member_id = _current_user.id;

			issue.save();

			double hours_worked;
			double.TryParse(txtHours.Text, out hours_worked);
			#region file added
			try
			{
				if (hiddenshot.Value.Trim() != "")
				{
					var image = Convert.FromBase64String(hiddenshot.Value.Split(',')[1]);
					NETickets.ticketissue.add_attachment(issue.id, image, image.Length, issue.id.ToString(), "png");
					issue.uploaded_file = issue.id + ".png";
					issue.save();
				}
				else if (filMyFile.PostedFile.ContentLength > 0)
				{
					var file = filMyFile.PostedFile;
					var buffer = new byte[file.ContentLength];
					file.InputStream.Read(buffer, 0, file.ContentLength);
					var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
					NETickets.ticketissue.add_attachment(issue.id, buffer, file.ContentLength, issue.id.ToString(), extension);
					issue.uploaded_file = issue.id + extension;
					issue.save();
				}
			}
			catch (Exception ex)
			{
				_tools.catch_error(ex);
				lblError.Visible = true;
				lblError.Text = "Failed to Upload File" + ex;
			}
			#endregion

			_ticket.modified_date = DateTime.Now;
			_ticket.status_id = Convert.ToInt32(combo_status_popup.Value);
			var group_admin = _ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_id;
			if (!_is_assignee && _current_user.id != group_admin && !combo_status_popup.Visible) // No need to override when the assignee == 0 and it's the group admin setting the status.
			{
				_ticket.waiting_on_member_id = _ticket.member_assigned_id != 0 ? _ticket.member_assigned_id : group_admin;

				_ticket.status_id = 3; // Always throws it out of their buckets, back to the assignee/group admin to delegate what status it needs to be.

			}
			_ticket.closed_date = _ticket.status_id == 5 ? DateTime.Now : _ticket.closed_date;
			_ticket.modified_date = DateTime.Now;
			_ticket.hours_worked = hours_worked;


			_ticket.save();
			if (_ticket.member_assigned_id != _current_user.id && _ticket.createdby_member_id != _current_user.id && _current_user.id != group_admin)
			{
				NETickets.add_viewer_to_ticket(_current_user.id, _current_user, _ticket_id);
			}
			_ticket.SendMessage(_current_user.id);
			var csm = Page.ClientScript;
			csm.RegisterClientScriptBlock(GetType(), "", "<script type='text/javascript'>setTimeout('handle_ticket_updated(true, null, null)', 500);</script>");
		}
	}
	protected void btnClosingSubmit_Click(object _sender, EventArgs _e)
	{
		lblError.Text = "";

		var issue = new NETickets.ticketissue
		{
			ticketheader_id = _ticket_id,
			message = txtCloseComment.Text,
			created_member_id = _current_user.id
		};
		issue.save();

		var ti = new NETickets(_ticket_id)
		{
			status_id = 5,
			closed_date = DateTime.Now,
			modified_date = DateTime.Now
		};
		ti.save_with_history(_current_user);

		var csm = Page.ClientScript;
		csm.RegisterClientScriptBlock(GetType(), "", "<script type='text/javascript'>setTimeout('handle_ticket_updated(true, null, null)', 500);</script>");
	}
	protected void ddl_page_Callback(object _sender, CallbackEventArgsBase _e)
	{
		using (var uow = new UnitOfWork())
		{
			populate_ddl(ddl_page, uow);
		}

	}
	protected void combo_group_Callback1(object _sender, CallbackEventArgsBase _e)
	{
		if (_e.Parameter != "")
		{
			var id = Convert.ToInt32(_e.Parameter[0]);
		}

	}
	protected void ddl_type_Callback(object _sender, CallbackEventArgsBase _e)
	{
		ddl_type.DataBind();
		if (_e.Parameter != "")
		{
			var id = Convert.ToInt32(_e.Parameter[0]);
		}
	}
	protected void task_list_Callback(object _sender, CallbackEventArgsBase _e)
	{
		if (_e.Parameter.Contains("|"))
		{
			var paras = _e.Parameter.Split('|');
			NETickets.ticket_task tt;
			switch (paras[0])
			{
				case "new":
					var task = paras[1];
					if (task == "")
					{
						task_list.JSProperties["cpResult"] = "FAILED";
						throw new Exception("Task can not be blank");
					}
					else
					{
						tt = new NETickets.ticket_task
						{
							complete = false,
							task = task,
							ticket_id = _ticket_id,
							assignee = Convert.ToInt32(_ticket.member_assigned_id)
						};
						tt.save();
						fill_tasks(false);
						task_list.JSProperties["cpResult"] = "SUCCESS";
					}
					break;
				case "edittask":
					tt = new NETickets.ticket_task(Convert.ToInt32(paras[1])) { task = paras[2] };
					tt.save();
					fill_tasks(false);
					task_list.JSProperties["cpResult"] = "COMPLETESUCCESS";
					break;
				case "delete":
					NETickets.ticket_task.delete(Convert.ToInt32(paras[1]));
					fill_tasks(false);
					task_list.JSProperties["cpResult"] = "DELETESUCCESS";
					break;
				case "complete":
					tt = new NETickets.ticket_task(Convert.ToInt32(paras[1])) { complete = Convert.ToBoolean(paras[2]) };
					tt.save();
					fill_tasks(false);
					task_list.JSProperties["cpResult"] = "COMPLETESUCCESS";
					break;
				case "setassignee":
					tt = new NETickets.ticket_task(Convert.ToInt32(paras[1])) { assignee = Convert.ToInt32(paras[2]) };
					tt.save();
					fill_tasks(false);
					task_list.JSProperties["cpResult"] = "COMPLETESUCCESS";
					break;
			}
		}
	}
	protected void cb_Callback(object _sender, CallbackEventArgsBase _e)
	{
		if (_e.Parameter.Contains("|"))
		{
			if (_e.Parameter.StartsWith("v"))
			{
				var paras = _e.Parameter.Split('|');
				int ticket_id;
				int.TryParse(paras[1], out ticket_id);
				if (ticket_id > 0)
				{
					var should_vote = paras[2] == "1";
					if (should_vote) // Add watch
					{
						var tv = new NETickets.ticket_vote
						{
							ticketheader_id = ticket_id,
							member_id = _current_user.id
						};
						tv.save();
					}
					else // Remove vote
					{
						var tv = new NETickets.ticket_vote(ticket_id, _current_user.id);
						tv.delete();
					}
					display_ticket_information();
				}

			}
		}
		else
		{
			cb.JSProperties["cpAction"] = _e.Parameter;
			switch (_e.Parameter)
			{
				#region save
				case "save":
					lblError0.Visible = false;
					var this_ticket = _ticket;
					var prev_ticket = new NETickets(_ticket.id);
					var is_error = false;
					var error_action = "";
					var error_text = "";

					using (var uow = new UnitOfWork())
					{
						populate_ddl(combo_group, uow);
						populate_ddl(ddl_page, uow);
					}
					if (ddl_page.Value == null)
					{
						error_text = "Please supply the page this ticket pertains to";
						error_action = "ddl_page.ToggleDropDown();";
						is_error = true;
					}
					else
					{
						this_ticket.module_id = Convert.ToInt32(ddl_page.Value);
					}
					if (ddl_type.Value == null && !is_error)
					{
						error_text = "Please supply the type of ticket";
						error_action = "ddl_type.ToggleDropDown();";
						is_error = true;
					}
					else if (!is_error)
					{
						this_ticket.issuetype = Convert.ToInt32(ddl_type.Value);
					}
					if (!is_error && combo_assigned.Value != null)
					{
						this_ticket.member_assigned_id = (int)combo_assigned.Value;
					}
					if (combo_status.Value != null && !is_error)
					{
						this_ticket.status_id = (int)combo_status.Value;
						if ((this_ticket.status_id == 7 || this_ticket.status_id == 5) && this_ticket.member_assigned_id == 0)
						{
							this_ticket.member_assigned_id = _current_user.id;
						}

						if (this_ticket.status_id == 7 || this_ticket.status_id == 5)
						{
							// Check if there are any huddle items linked to this ticket that need closing.
							Toolbox.doSQL_void(@"UPDATE huddle_task SET status = 3 WHERE source_id = @v0", new object[] { this_ticket.id });
						}
						this_ticket.save();
						this_ticket = new NETickets(this_ticket.id);
					}
					if (combo_waitinguser.Value != null && !is_error)
					{
						this_ticket.waiting_on_member_id = (int)combo_waitinguser.Value;
					}
					/*
					if(this_ticket.status_id == 7 && prev_ticket.status_id != 7 && prev_ticket.status_id != 5)
						{
						if(this_ticket.issuetype == 1)
							{
							if(string.IsNullOrEmpty(this_ticket.symptom))
								{
								error_text			= "Before you can set this ticket to 'Waiting for opener to close', you must fill out the symptom";
								error_action		= "pc_chat.SetActiveTabIndex(1);";
								is_error			= true;
								}
							else if(string.IsNullOrEmpty(this_ticket.cause))
								{
								error_text			= "Before you can set this ticket to 'Waiting for opener to close', you must fill out the cause";
								error_action		= "pc_chat.SetActiveTabIndex(2);";
								is_error			= true;
								}
							else if(string.IsNullOrEmpty(this_ticket.solution))
								{
								error_text			= "Before you can set this ticket to 'Waiting for opener to close', you must fill out the solution";
								error_action		= "pc_chat.SetActiveTabIndex(3);";
								is_error			= true;
								}
							}
						else if(this_ticket.issuetype == 3)
							{
							if(string.IsNullOrEmpty(this_ticket.roi))
								{
								error_text			= "Before you can set this ticket to 'Waiting for opener to close', you must fill out the ROI";
								error_action		= "pc_chat.SetActiveTabIndex(4);";
								is_error			= true;
								}
							}
						}
					 */
					if (is_error)
					{
						cb.JSProperties["cpError"] = true;
						cb.JSProperties["cpErrorAction"] = error_action;
						lblError0.Text = error_text;
						lblError0.Visible = true;
						return;
					}
					this_ticket.priority_id = (int?)combo_priority.Value ?? 1;
					this_ticket.expected_completion = dtefinish.Date;
					this_ticket.createdby_member_id = (int)combo_created.Value;
					this_ticket.is_private = chkPrivate.Checked;
					this_ticket.issue = memo_subject.Text;
					this_ticket.modified_date = DateTime.Now;
					this_ticket.closed_date = this_ticket.status_id == 5 ? DateTime.Now : this_ticket.closed_date;
					this_ticket.assigned_date = prev_ticket.member_assigned_id != this_ticket.member_assigned_id ? DateTime.Now : this_ticket.assigned_date;
					double expected_hours_value;
					if (txtexphours.Value == null)
					{
						txtexphours.Value = 0;
					}
					double.TryParse(txtexphours.Value.ToString(), out expected_hours_value);
					this_ticket.expected_hours = expected_hours_value;
					/*
						if (prev_ticket.status_id != 5 && (this_ticket.status_id == 6 || this_ticket.status_id == 8) && this_ticket.member_assigned_id != 0 && this_ticket.member_assigned_id == current_user.id)
							{
							if (expected_hours_value == 0)
								{
								lblError0.Text = "You must enter an expected hours value - for status of Read or Assigned";
								lblError0.Visible = true;
								cb.JSProperties["cpError"]				= true;
								error_action							= "true";
								cb.JSProperties["cpErrorAction"]		= error_action;
								txtexphours.Focus();
								return;
								}
							}
					 */
					if (this_ticket.member_assigned_id != 0 && this_ticket.status_id == 1)
					{
						combo_assigned.SelectedIndex = 0;
					}
					try
					{
						this_ticket.save_with_history(_current_user);
					}
					catch (Exception ee)
					{
						throw;
					}
					cb.JSProperties["cpError"] = false;
					_ticket = new NETickets(this_ticket.id);
					populate_header_info();
					display_ticket_information();
					break;
				#endregion save
				#region group_change
				case "group_change":
					ddl_page.Value = null;
					ddl_type.Value = null;
					combo_assigned.Value = null;
					combo_priority.Value = null;
					combo_status.Value = null;
					using (var uow = new UnitOfWork())
					{
						populate_ddl(combo_status, uow);
						populate_ddl(combo_status_popup, uow);
						populate_ddl(combo_assigned, uow);
						populate_ddl(ddl_page, uow);
						populate_ddl(combo_priority, uow);
					}
					break;
				#endregion group_change
				#region refresh
				case "refresh":
					using (var uow = new UnitOfWork())
					{
						populate_header_info();
						populate_ddl(combo_group, uow);
						populate_ddl(combo_created, uow);
						populate_ddl(combo_assigned, uow);
						populate_ddl(combo_objectiveowner, uow);
						populate_ddl(combo_priority, uow);
						populate_ddl(combo_status, uow);
						populate_ddl(combo_status_popup, uow);
						populate_ddl(ddl_page, uow);
						populate_ddl(combo_waitinguser, uow);
					}
					break;
					#endregion refresh
			}
		}
	}
	protected void client_cbp_CallBack(object _sender, CallbackEventArgsBase _e)
	{
		if (_e.Parameter.StartsWith("v"))
		{
			var paras = _e.Parameter.Split('|');
			int ticket_id;
			int.TryParse(paras[1], out ticket_id);
			if (ticket_id > 0)
			{
				var should_vote = paras[2] == "1";
				if (should_vote) // Add watch
				{
					var tv = new NETickets.ticket_vote
					{
						ticketheader_id = ticket_id,
						member_id = _current_user.id
					};
					tv.save();
				}
				else // Remove vote
				{
					var tv = new NETickets.ticket_vote(ticket_id, _current_user.id);
					tv.delete();
				}
				fill_votes(true);
			}

		}
		else if (_e.Parameter == "movegroup")
		{
			using (var uow = new UnitOfWork())
			{
				var x = from y in new XPQuery<ne_xpo.cs.ticket_group>(uow)
						where y.ticket_group_administrator.member_id == _current_user.id &&
						(y.ticket_group_id == 1 || y.ticket_group_id == 30 || y.ticket_group_id == 31)
						select y;
				var group_admin_of = 0;
				if (x.Any())
				{
					foreach (var xx in x)
					{
						if (Toolbox.Contains(xx.ticket_group_id, new[] { 1, 30, 31 }))
						{
							group_admin_of = xx.ticket_group_id;
							break;
						}
					}
					if (group_admin_of > 0)
					{
						var module_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(ticketpage_id), 0) 
FROM ticketpage WHERE page_id = @v0 AND ticketpage_ticket_group_id = @v1", new object[] { _ticket.xpo_ref.ticketheader_module_id.page_id.page_id, group_admin_of });
						if (module_id > 0)
						{
							_ticket.module_id = module_id;
							_ticket.save();
							client_cbp_left.JSProperties["cpRefresh"] = true;
						}
					}
				}
			}
		}
		else if (_e.Parameter == "assigntome")
		{
			_ticket.member_assigned_id = _current_user.id;
			_ticket.save();
			client_cbp_left.JSProperties["cpRefresh"] = true;
		}
	}
	protected void cb_client_follow_Callback(object _sender, CallbackEventArgs _e)
	{
		var do_follow = Convert.ToBoolean(_e.Parameter);
		if (do_follow)
		{
			NETickets.add_viewer_to_ticket(_current_user.id, _current_user, _ticket_id);
		}
		else
		{
			var tmv = new NETickets.ticket_memberview(_ticket_id, _current_user.id);
			tmv.delete();
		}
	}
	protected void cb_request_reopen_Callback(object _sender, CallbackEventArgs _e)
	{
		var from_creator = _ticket.createdby_member_id == _current_user.id;
		if (from_creator)
		{
			_ticket.status_id = 9;
			_ticket.save();
			var ti = new NETickets.ticketissue
			{
				created_date = DateTime.Now,
				created_member_id = _current_user.id,
				message = "Ticket reopened by opener.<br/>Reason supplied: " + _e.Parameter,
				ticketheader_id = _ticket.id
			};
			ti.save();
		}
		var request = new NeEMail
		{
			To = _ticket.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_neemail,
			From = _current_user.NEEmail,
			isHTML = true,
			Subject = from_creator
												? string.Format("Ticket #{0} has been reopened by opener", _ticket_id)
												: string.Format("Request that Ticket #{0} be reopened", _ticket_id),
			Body =
									string.Format(
                                        "Ticket #: <a href='" + Toolbox.app_setting("Domain") + "/sections/member/tickets/ticketpage.aspx?issue={0}'>{0}</a><br/>Issue: {1}<br/>Reason Provided:{2}",
										_ticket.id, _ticket.issue, _e.Parameter),
			CC =
									_ticket.xpo_ref.ticketheader_member_assigned_id == null
										? ""
										: _ticket.xpo_ref.ticketheader_member_assigned_id.member_neemail
		};
		request.Send();
		_e.Result = "Sent";
	}
	protected void list_trackers_Callback(object _sender, CallbackEventArgsBase _e)
	{
	}
	protected void cb_task_Callback(object _sender, CallbackEventArgsBase _e)
	{
		if (_e.Parameter.Contains("|"))
		{
			var paras = _e.Parameter.Split('|');
			switch (paras[0])
			{
				case "addtracker":
					if (combo_availableusers.Value != null)
					{
						int new_member_id;
						int.TryParse(combo_availableusers.Value.ToString(), out new_member_id);
						NETickets.add_viewer_to_ticket(new_member_id, _current_user, _ticket_id);
						list_trackers.DataBind();
						combo_availableusers.DataBind();
						combo_availableusers.Value = null;
						list_trackers.UnselectAll();
					}
					else
					{
						throw new Exception("Please select a user");
					}
					break;
				case "removetracker":
					NETickets.delete_viewer_from_ticket(Convert.ToInt32(paras[1]));
					list_trackers.DataBind();
					combo_availableusers.DataBind();
					combo_availableusers.Value = null;
					list_trackers.UnselectAll();
					break;
			}
		}
	}
	protected void pc_chat_Callback(object _sender, CallbackEventArgsBase _e)
	{
		_ticket = new NETickets(_ticket_id);
		switch (_e.Parameter)
		{
			case "symptom":
				_ticket.symptom = memo_symptom.Text;
				_ticket.save();
				break;
			case "cause":
				_ticket.cause = memo_cause.Text;
				_ticket.save();
				break;
			case "solution":
				_ticket.solution = memo_solution.Text;
				_ticket.save();
				break;
			case "roi":
				_ticket.roi = memo_roi.Text;
				_ticket.save();
				break;
			case "notes":
				_ticket.notes = memo_notes.Text;
				_ticket.save();
				break;
		}
	}
	protected void combo_waitingonuser_popup_Callback(object _sender, CallbackEventArgsBase _e)
	{
		using (var uow = new UnitOfWork())
		{
			populate_ddl(combo_waitingonuser_popup, uow);
		}
	}
	protected void popnewchat_WindowCallback(object source, PopupWindowCallbackArgs e)
	{
		display_ticket_information();
	}
}
