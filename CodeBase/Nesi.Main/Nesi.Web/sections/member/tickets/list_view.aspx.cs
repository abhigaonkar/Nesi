using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Xpo;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using nesi.core;

public partial class sections_member_tickets_Copy_of_index : System.Web.UI.Page
{
	NeMember current_user;
	static string _page_name	= "Tickets";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;

	NameValueCollection _q;
	protected void Page_Init(object sender, EventArgs e)
		{
		_q						= Request.QueryString;
		Toolbox.do_debug("Page_init start");
		current_user			= Toolbox.do_handle_authentication(170);

		layout.__page_name      = _page_name;
		layout.used_gv			= gv_tickets;
		ds_templates            = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter              = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export            = (Panel)layout.FindControl("panel_export");
		panel_export.Visible    = true;
		h						= (ASPxHiddenField) layout.FindControl("h");
		h.Set("gridview_id", "gv_tickets");
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= current_user.id.ToString();
		gv_bind();
		Toolbox.do_debug("Page_init stop");
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		Toolbox.do_debug("Page_load start");
			if (!IsCallback && !IsPostBack)
			{
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
				{
				gv_tickets.FilterExpression = "";
				gl.GridLayout_Layout = gv_tickets.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_tickets.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text = gl.GridLayout_Name;
				}
		var tickets = new NETickets();
		if (current_user.AuthenticatedForPrivilege(131))  // if user can see plan 17 
		{
			pnl_plan17.Visible = true;
		}
		if (current_user.member_isticket_admin > 0 || (Toolbox.doSQL_int(@"Select count(ticketmanager_id) from ticketmanager where ticketmanager_member_id = @v0" , current_user.id) > 0))
		{
			ASPxRoundPanel3.Visible = true;
			pnl_mygroup.Visible = current_user.member_isticket_admin > 0;
		}

		#region manager settings stuff
		if (current_user.id != 8)
		{
			gv_assignees.DataSource = Toolbox.doSQL_dt(@"SELECT b.ticketmanager_id, b.ticketmanager_member_id, b.ticketmanager_group_id FROM ticket_group a, ticketmanager b INNER JOIN member c ON b.ticketmanager_member_id = c.member_id  WHERE c.member_status = 'Active' and b.ticketmanager_group_id = a.ticket_group_id and a.ticket_group_administrator =@v0", new object[] { current_user.id });
			sds_ticketgroup.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group where ticket_group_administrator = " + current_user.id;
		}
		else
		{
			gv_assignees.DataSource = Toolbox.doSQL_dt(@"SELECT b.ticketmanager_id, b.ticketmanager_member_id, b.ticketmanager_group_id FROM ticketmanager b INNER JOIN member c ON b.ticketmanager_member_id = c.member_id  WHERE c.member_status = 'Active'" , null);
			sds_ticketgroup.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group";
		}

		#endregion


		//sept3		string perpetual_layout			= Session["clientlayout_tickets"] != null ? Session["clientlayout_tickets"].ToString() : "";
		#region tickets in my court label
		var x = Toolbox.doSQL_dt(@"CALL GETTICKETS(@v0 , 'GetMyCourt')", new object[] {  current_user.id } ).Rows.Count;
		if (x>0)
		{
		mycourt_link.InnerText = "Tickets in my court ( " + x + " )";
		mycourt_link.Style.Add("color", "#f00");
		//blMyCourt.ForeColor = System.Drawing.Color.Red;
		}
		#endregion
		
		sds_ticketgroup.DataBind();
		gv_assignees.DataBind();
		hl_ev_ticketreleases.Visible = Toolbox.doSQL_int(@"SELECT COUNT(ticketheader_id) FROM ticketheader
WHERE release_id IN (SELECT id FROM releases WHERE final_released = 0) AND ticketheader_status_id != 5") > 0;

		Toolbox.do_debug("Page_load stop");
	}
	private void gv_bind()
		{
		var action			= string.IsNullOrEmpty(_q["a"]) ? "" : _q["a"];
		var titles	= new Dictionary<string,string>()
			{
			{"GetMyTickets", "Tickets That I Started"},
			{"GetTicketReleases", "Tickets In Current Release"},
			{"GetMyAssignedTicketsthisweek", "Tickets Due This Week"},
			{"GetAllOpenTickets", "All Open Tickets"},
			{"GetAllClosedTickets", "All Closed Tickets"},
			{"GetAllOpenTicketsfromGroupthisweek", "Tickets Due This Week From My Group"},
			{"GetAllTickets", "All Tickets"},
			{"GetAllLTTickets", "All LT Tickets"},
			{"GetAllOpenTicketsfromGroup", "Open Tickets From My Group"},
			{"GetMyCourt", "Tickets In My Court"},
			{"GetMyAssignedTickets", "Tickets Assigned to Me"},
			{"GetUnassignedTickets", "Unassigned Tickets From My Group"}
			};
		var _tickets	= new DataTable();
		if(action == "Search")
			{
			var possible_ticket_id		= 0;
			int.TryParse(_q["q"], out possible_ticket_id);
			if (possible_ticket_id > 0)
				{
				if (Toolbox.doSQL_int(@"Select count(ticketheader_id) from ticketheader where ticketheader_id = @v0" , possible_ticket_id) > 0)
					{
					var script = "boing('/sections/member/tickets/ticketpage.aspx?issue=" + possible_ticket_id + "','tickets1',1095,740)";
					ClientScript.RegisterStartupScript(GetType(), "javascript", script, true);
					}
				}
			txtSearchBox.Value				= _q["q"];
			_tickets						= NETickets.GetCreatedissues(_q["q"], (int) current_user.id);
			gv_tickets.SettingsText.Title	= "Search Results for: "+_q["q"];
			}
		else if(titles.ContainsKey(action) || action == "")
			{
			if(action == "")
				{
				action = "GetMyCourt";
				}
			_tickets						= Toolbox.doSQL_dt(@"CALL GETTICKETS(@v0 , @v1 )", new object[] {  current_user.id, action } );
			gv_tickets.SettingsText.Title	= titles[action];
			}
		else
			{
			Toolbox.FriendlyException(Response, "Unsupported ticket search type.", "");
			}
		gv_tickets.DataSource			= _tickets;
		gv_tickets.DataBind();
		}
	protected void gv_tickets_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
		Session["clientlayout_tickets"] = gv_tickets.SaveClientLayout();
		}
	#region manager settings
	protected void ASPxGridView2_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		var memid = Convert.ToInt32(e.NewValues["ticketmanager_member_id"]);
		var groupid = Convert.ToInt32(e.NewValues["ticketmanager_group_id"]);
		if (Toolbox.doSQL_int(@"select count(ticketmanager_id) from ticketmanager where ticketmanager_member_id =@v0   and ticketmanager_group_id =@v1 ",
			new object[] {memid , groupid}) >= 1)
		{
			e.Cancel = true;
			gv_assignees.CancelEdit();
			return;
		}
		Toolbox.doSQL_void(@"Insert into ticketmanager (ticketmanager_member_id,ticketmanager_group_id) values (@v0,@v1)" , new object[] { memid, groupid });
		e.Cancel = true;
		gv_assignees.CancelEdit();
		if (current_user.id != 8)
		{
			gv_assignees.DataSource = Toolbox.doSQL_dt(@"SELECT ticketmanager.ticketmanager_id, ticketmanager.ticketmanager_member_id, ticketmanager.ticketmanager_group_id FROM ticket_group,ticketmanager INNER JOIN member ON ticketmanager.ticketmanager_member_id = member.Member_ID  WHERE member.Member_Status = 'Active' and ticketmanager.ticketmanager_group_id=ticket_group_id and ticket_group_administrator =@v0", new object[] { current_user.id });
			sds_ticketgroup.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group where ticket_group_administrator = " + current_user.id;
		}
		else
		{
			gv_assignees.DataSource = Toolbox.doSQL_dt(@"SELECT ticketmanager.ticketmanager_id, ticketmanager.ticketmanager_member_id, ticketmanager.ticketmanager_group_id FROM ticketmanager INNER JOIN member ON ticketmanager.ticketmanager_member_id = member.Member_ID  WHERE member.Member_Status = 'Active'" , null);
			sds_ticketgroup.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group";
		}
	}
	protected void ASPxGridView2_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		Toolbox.doSQL_void(@"Delete from ticketmanager where ticketmanager_id  = @v0", new object[] { e.Keys[0] });
		if (current_user.id != 8)
		{
			gv_assignees.DataSource = Toolbox.doSQL_dt(@"SELECT ticketmanager.ticketmanager_id, ticketmanager.ticketmanager_member_id, ticketmanager.ticketmanager_group_id FROM ticket_group,ticketmanager INNER JOIN member ON ticketmanager.ticketmanager_member_id = member.Member_ID  WHERE member.Member_Status = 'Active' and ticketmanager.ticketmanager_group_id=ticket_group_id and ticket_group_administrator =@v0", new object[] { current_user.id });
			sds_ticketgroup.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group where ticket_group_administrator = " + current_user.id;
		}
		else
		{
			gv_assignees.DataSource = Toolbox.doSQL_dt(@"SELECT ticketmanager.ticketmanager_id, ticketmanager.ticketmanager_member_id, ticketmanager.ticketmanager_group_id FROM ticketmanager INNER JOIN member ON ticketmanager.ticketmanager_member_id = member.Member_ID  WHERE member.Member_Status = 'Active'" , null);
			sds_ticketgroup.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group";
		}

		e.Cancel = true;
		gv_assignees.CancelEdit();
	}
#endregion
	List<int> voted_list;
	protected void gv_tickets_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		if(voted_list == null)
			{
			using(var uow = new UnitOfWork())
				{
				voted_list			= (from y in new XPQuery<ne_xpo.cs.ticket_vote>(uow)
									where y.member_id.member_id == (int) current_user.id 
										select y.ticketheader_id.ticketheader_id).ToList();
				}
			}
		if (e.VisibleIndex >= 0)
			{
			var dr				= gv.GetDataRow(e.VisibleIndex);
			var ticket_id			= (int) dr["ticketheader_id"];
			var status_id			= (int) dr["status_id"];
			var type				= dr["TypeOfTicket"].ToString();
			switch(e.DataColumn.FieldName)
				{
				case "ticketheader_id":
					switch (gv.GetRowValues(e.VisibleIndex, "Priority").ToString())
						{
						case "1 Critical":
							e.Cell.Style["background-color"] = "#f00";
							e.Cell.Style["color"] = "#fff";
							break;
						case "2 Urgent":
							e.Cell.Style["background-color"] = "#f90";
							break;
						case "3 Important":
							e.Cell.Style["background-color"] = "#ffcc00";
							break;
						case "4 Minimum":
							e.Cell.Style["background-color"] = "#ffff00";
							break;
						case "5 Nice To Have":
							e.Cell.Style["background-color"] = "#99cc99";
							break;
						case "6 Not Ranked":
							e.Cell.Style["background-color"] = "#ccc";
							break;
						}
					break;
				case "Priority":
					e.Cell.ToolTip = Toolbox.ReturnBlankIfNull_string(e.CellValue);
					switch (Toolbox.ReturnBlankIfNull_string(e.CellValue))
						{
						case "1 Critical":
							e.Cell.Style["background-color"] = "#f00";
							break;
						case "2 Urgent":
							e.Cell.Style["background-color"] = "#f90";
							break;
						case "3 Important":
							e.Cell.Style["background-color"] = "#ffcc00";
							break;
						case "4 Minimum":
							e.Cell.Style["background-color"] = "#ffff00";
							break;
						case "5 Nice To Have":
							e.Cell.Style["background-color"] = "#99cc99";
							break;
						case "6 Not Ranked":
							e.Cell.Style["background-color"] = "#ccc";
							break;
						}
					break;
				case "Status":
					e.Cell.ToolTip = Toolbox.ReturnBlankIfNull_string(e.CellValue);
                    switch (status_id)
						{
						case 1:
							e.Cell.Style["background-color"] = "#ccc";
							break;
						case 2:
							e.Cell.Style["background-color"] = "#3ff";
							break;
						case 3:
							e.Cell.Style["background-color"] = "#00f";
							e.Cell.Style["color"] = "#fff";
							break;
						case 5:
							e.Cell.Style["background-color"] = "#333";
							e.Cell.Style["color"] = "#fff";
						break;
						case 6:
							e.Cell.Style["background-color"] = "#9c6";
							break;
						case 7:
							e.Cell.Style["background-color"] = "#f3f";
							e.Cell.Style["color"] = "#fff";
							break;
						case 8:
							e.Cell.Style["background-color"] = "#ffc";
							break;
						case 9:
							e.Cell.Style["background-color"] = "#0f0";
							break;
						}
					break;
				case "exp_fin":
					 if (Toolbox.ReturnNullDateTime(e.CellValue) != null)
						{
						if (Convert.ToDateTime(e.CellValue) > System.DateTime.MinValue)
							{
							if (Convert.ToDateTime(e.CellValue) < System.DateTime.Today)
								{
								e.Cell.Style["background-color"] = "#f00";
								e.Cell.Style["color"] = "#fff";
								}
							else if (Convert.ToDateTime(e.CellValue) < System.DateTime.Today.AddDays(7))
								{
								e.Cell.Style["background-color"] = "#f90";
								}
							}
						}
					break;
				case "AsignedTo":
					if (e.CellValue.ToString() == current_user.FullName2)
						{
						e.Cell.Font.Bold = true;
						}
					break;
				case "RaisedIssue":
					using (var uow = new UnitOfWork())
						{
						ne_xpo.cs.ticketheader x;
						int current_user_id, created_by, assigned_to, group_admin = 0;
						var hl = (ASPxHyperLink)gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "hl");
						if (hl != null)
							{
							x = uow.GetObjectByKey<ne_xpo.cs.ticketheader>(ticket_id);
							current_user_id = (int)current_user.id;
							created_by = x.ticketheader_createdby_member_id == null ? 0 : x.ticketheader_createdby_member_id.member_id;
							assigned_to = x.ticketheader_member_assigned_id == null ? 0 : x.ticketheader_member_assigned_id.member_id;
							group_admin = x.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_id;
							var width = current_user_id != created_by && current_user_id != assigned_to && current_user_id != group_admin ? 740 : 1095;
							var height = 740;
							hl.NavigateUrl = string.Format("javascript:boing('/sections/member/tickets/ticketpage.aspx?issue={0}', 'tickets{0}',{1},{2})", ticket_id, width, height);
							hl.Text = e.CellValue.ToString();
							}
						}
					break;
				case "v":
					var div_wrapper		= (HtmlContainerControl) gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "icon_holder_wrapper");
					var v								= Convert.ToInt32(dr["v"]);
					var voted							= voted_list.Contains(ticket_id) ? 1 : 0;
					div_wrapper.Visible					= false;
					if(type == "3" && !Toolbox.Contains(status_id, new int[]{5,7,10,11,12}))
						{
						div_wrapper.Visible					= true;
						div_wrapper.InnerHtml				= string.Format("<b style='position:relative;right:10%;'>{0}</b>", v);
						div_wrapper.Attributes["onclick"]	= string.Format("local_ticket.vote({0},{1});", ticket_id, 1-voted);
						div_wrapper.Attributes["title"]		= voted == 1 
																? "Click to remove your vote from this ticket" 
																: "Click to add your vote to this ticket!\nThe higher the rank, the sooner it will be done.";
						if(voted == 1)
							{
							div_wrapper.Attributes["class"]		+= " voted";
							}
						}
					break;
				}
			}
		}
	protected void blthisquarter_Click(object sender, EventArgs e)
	{
		Viewplanthisquarter();
	}
	protected void Viewplanthisquarter()
	{
		try
		{
			Session["TicketSearchString"] = "";
			var strMemberID = current_user.id.ToString();
			var tickets = new NETickets();
			Session["gv_tickets"] = GetPlanThisQuarter(strMemberID, current_user);
			
			Session["gv_tickets_title"] = "Plan 17 This Quarter - Stuff Im Taking Care Of";
			if (!current_user.AuthenticatedForPrivilege(131))
			{
				gv_tickets.DataSource = GetPlanThisQuarterOther(strMemberID).Select("TypeOfTicket <> 'LT Quarterly Objective'").Length == 0 ? GetPlanThisQuarterOther("9999") : GetPlanThisQuarterOther(strMemberID).Select("TypeOfTicket <> 'LT Quarterly Objective'").CopyToDataTable();
			}
			else
			{
				gv_tickets.DataSource = GetPlanThisQuarterOther(strMemberID);
			}
			gv_tickets.DataBind();
			gv_tickets.SettingsText.Title = "Plan 17 This Quarter - Greener Grass";
		}
		catch { }

	}
	public DataTable GetPlanThisQuarterOther(string memberid)
		{
		var quarter_end = "03";
		if (System.DateTime.Today.Month > 3)
			quarter_end = "06";
		else if (System.DateTime.Today.Month > 6)
			quarter_end = "09";
		else quarter_end = "12";

		quarter_end = System.DateTime.Today.Year + "-" + quarter_end + "-31";


		var sqlSelect = "SELECT ticketheader_id, URLDECODE(a.ticketheader_issue) AS RaisedIssue,a.ticketheader_modified_date,a.ticketheader_private, a.ticketheader_created_date AS DateCreated, b.ticketpage_name AS PageName, c.ticketstatus_status AS Status, d.tickettype_name AS TypeOfTicket, CONCAT(f.Member_nickname, ' ', f.Member_LastName) AS AsignedTo, a.release_id, CONCAT(g.Member_nickname, ' ', g.Member_LastName) AS RaisedBy, e.ticketpriority_name AS Priority, h.ticket_group_name as groupname ,a.ticket_header_expected_completion as exp_fin,h.ticket_group_administrator as g_admin, a.release_id ";
		sqlSelect += "FROM ticketheader AS a, ticketpage AS b, ticketstatus AS c, tickettype AS d, ticketpriority AS e, member AS f, member AS g, ticket_group as h ";
		sqlSelect += "WHERE ticketheader_module_id = ticketpage_id ";
		sqlSelect += "AND ticketheader_status_id = ticketstatus_id ";
		sqlSelect += "AND ticketheader_issuetype = tickettype_id ";
		sqlSelect += "AND (ticketheader_member_assigned_id != @v0 OR ticketheader_member_assigned_id is null) ";
		sqlSelect += " AND ticketheader_priority_id = ticketpriority_id ";
		sqlSelect += "AND ticketheader_createdby_member_id = g.member_id and b.ticketpage_ticket_group_id = h.ticket_group_id and b.ticketpage_ticket_group_id = 4 ";
		sqlSelect += "AND ticketheader_member_assigned_id = f.member_id ";
		sqlSelect += "AND (ticket_header_expected_completion is null or ticket_header_expected_completion < @v1) ";
		sqlSelect += "AND ticketheader_status_id != 5 "; //5 is the status of a Close Ticket
		sqlSelect += " ORDER BY ticketheader_priority_id,ticketheader_id ";
		var HeaderTable = Toolbox.doSQL_dt(sqlSelect,new object[] { memberid, quarter_end} );
		return HeaderTable;
		}
	public string GetPlanThisQuarter(string memberid, NeMember myMember)
		{
		var quarter_end = "03";
		if (System.DateTime.Today.Month > 3)
			quarter_end = "06";
		else if (System.DateTime.Today.Month > 6)
			quarter_end = "09";
		else quarter_end = "12";

		quarter_end = System.DateTime.Today.Year + "-" + quarter_end + "-31";


		var sqlSelect = "SELECT ticketheader_id, URLDECODE(a.ticketheader_issue) AS RaisedIssue,a.ticketheader_modified_date,a.ticketheader_private, a.ticketheader_created_date AS DateCreated, b.ticketpage_name AS PageName,a.release_id,  c.ticketstatus_status AS Status, d.tickettype_name AS TypeOfTicket, CONCAT(f.Member_nickname, ' ', f.Member_LastName) AS AsignedTo, CONCAT(g.Member_nickname, ' ', g.Member_LastName) AS RaisedBy, e.ticketpriority_name AS Priority, h.ticket_group_name as groupname ,a.ticket_header_expected_completion as exp_fin,h.ticket_group_administrator as g_admin ";
		sqlSelect += "FROM ticketheader AS a, ticketpage AS b, ticketstatus AS c, tickettype AS d, ticketpriority AS e, member AS f, member AS g, ticket_group as h ";
		sqlSelect += "WHERE ticketheader_module_id = ticketpage_id ";
		sqlSelect += "AND ticketheader_status_id = ticketstatus_id ";
		sqlSelect += "AND ticketheader_issuetype = tickettype_id ";
		sqlSelect += "AND ticketheader_member_assigned_id = " + memberid;
		sqlSelect += " AND ticketheader_priority_id = ticketpriority_id ";
		sqlSelect += "AND ticketheader_createdby_member_id = g.member_id and b.ticketpage_ticket_group_id = h.ticket_group_id and b.ticketpage_ticket_group_id = 4 ";
		sqlSelect += "AND ticketheader_member_assigned_id = f.member_id ";
		sqlSelect += "AND ((ticketheader_private=0) or (CONCAT(g.Member_nickname, ' ', g.Member_LastName)='" + myMember.FullName + "') or (h.ticket_group_administrator=" + myMember.id + ") or  (CONCAT(f.Member_nickname, ' ', f.Member_LastName)='" + myMember.FullName + "')) ";
		sqlSelect += "AND (ticket_header_expected_completion is null or ticket_header_expected_completion < '" + quarter_end + "') ";
		sqlSelect += "AND ticketheader_status_id != 5 "; //5 is the status of a Close Ticket
		sqlSelect += " ORDER BY ticketheader_priority_id,ticketheader_id ";
		//	DataTable HeaderTable = _Tools.getSQL_datatable(@sqlSelect  , null);
		return sqlSelect;
		}
	protected void blAllplan17_Click(object sender, EventArgs e)
	{
		
	}
	protected void lblt_Click(object sender, EventArgs e)
	{
		ViewLTIssues();
	}
	protected void ViewLTIssues()
	{
		Session["TicketSearchString"] = "";
		var strMemberID = current_user.id.ToString();
		var tickets = new NETickets();
		if (current_user.AuthenticatedForPrivilege(131))
		{
			Session["gv_tickets"] = Toolbox.doSQL_dt(@"CALL GETTICKETS(0, 'GetAllLTTickets')"  , null);
			Session["gv_tickets_title"] = "All LT Tickets ";
			gv_tickets.SortBy(gv_tickets.Columns["exp_fin"], DevExpress.Data.ColumnSortOrder.Ascending);
			gv_tickets.DataBind();
		}
		else
		{
			Response.End();
		}

		
	}
	protected void gv_tickets_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		}
	protected void gv_tickets_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		if(e.Parameters.StartsWith("v"))
			{
			var paras			= e.Parameters.Split('|');
			var ticket_id			= 0;
			int.TryParse(paras[1], out ticket_id);
			if(ticket_id > 0)
				{
				var should_vote			= paras[2] == "1";
				if(should_vote) // Add watch
					{
					var tv				= new NETickets.ticket_vote();
					tv.ticketheader_id						= ticket_id;
					tv.member_id							= (int) current_user.id;
					tv.save();
					}
				else // Remove vote
					{
					var tv				= new NETickets.ticket_vote(ticket_id, (int) current_user.id);
					tv.delete();
					}
				gv_bind();
				}

			}
		else
			{ 
			if (e.Parameters != "")
				{
				gv.LoadClientLayout(e.Parameters);
				}
			else
				{
				gv.FilterExpression = "";
				for (var i = 0; i < gv.Columns.Count; i++)
					{
					if (gv.Columns[i] is GridViewDataColumn)
						{
						var col = (GridViewDataColumn)gv.Columns[i];
						if (col.GroupIndex > -1)
							{
							gv.UnGroup(col);
							}
						col.Visible = true;
						}
					}
				}
			}
		}
}
