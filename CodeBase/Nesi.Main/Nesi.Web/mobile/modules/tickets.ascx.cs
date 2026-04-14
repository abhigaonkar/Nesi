using System;
using DevExpress.Web;
using System.Data;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class mobile_modules_tickets : System.Web.UI.UserControl
	{
	NeMember current_user;
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user	= Toolbox.do_handle_authentication(170);
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		// Populate waiting on me (1)
			populate(1);
		if(Session["search_results"] == null || !this.Page.IsCallback)
			{
			if(!this.Page.IsCallback)
				{
				Session["search_results"]	= null;
				}
			populate(2);
			}
		else
			{
			gv_mytickets.DataSource		= (DataTable) Session["search_results"];
			gv_mytickets.DataBind();
			}
		if(Toolbox.doSQL_int(@"SELECT COUNT(*) FROM ticket_group WHERE ticket_group_administrator = @v0", current_user.id) > 0)
			{
			populate(3);
			}
		else
			{
			newtickets_title.Visible	= false;
			gv_newtickets.Visible		= false;
			}

		if(Toolbox.doSQL_int(@"SELECT COUNT(*) FROM ticket_group WHERE ticket_group_administrator = @v0", current_user.id) > 0)
			{
			populate(4);
			}
		else
			{
			yourgroup_title.Visible		= false;
			gv_yourgroup.Visible		= false;
			}
		}
	private void populate(int _type)
		{
		var _tickets		= new DataTable();
		var gv			= new ASPxGridView();
		switch(_type)
			{
			case 1: // Waiting on you

		var sql = "SELECT ticketheader_id id,ticketheader_id ordered_id, a.ticketheader_issue AS issue,a.ticketheader_modified_date, a.ticketheader_created_date AS DateCreated, b.ticketpage_name AS PageName, c.ticketstatus_status AS Status, d.tickettype_name AS TypeOfTicket, f.member_fullname AS assignedto, get_name(ticketheader_createdby_member_id) AS RaisedBy, e.ticketpriority_name AS Priority, h.ticket_group_name as groupname ,a.ticket_header_expected_completion as exp_fin, ";
		sql += " a.ticketheader_priority_id priority_id, a.ticketheader_status_id status_id, b.ticketpage_ticket_group_id group_id ";
		sql += @"FROM 
			ticketheader a 
		LEFT JOIN ticketpage b ON 
			a.ticketheader_module_id = b.ticketpage_id
		LEFT JOIN ticketstatus c ON 
			a.ticketheader_status_id = c.ticketstatus_id
		LEFT JOIN tickettype d  ON 
			a.ticketheader_issuetype = d.tickettype_id
		LEFT JOIN ticketpriority e ON 
			a.ticketheader_priority_id = e.ticketpriority_id
		LEFT JOIN member f ON 
			a.ticketheader_member_assigned_id = f.member_id 
		LEFT JOIN member g ON 
			a.ticketheader_createdby_member_id = g.member_id 
		LEFT JOIN ticket_group h ON 
			b.ticketpage_ticket_group_id = h.ticket_group_id ";
		sql += "WHERE ticketheader_module_id = ticketpage_id ";
		sql += " ";
		sql += "AND ((ticketheader_createdby_member_id=" + current_user.id + " AND ticketstatus_id in (3,7)) OR (ticketstatus_id=9 AND (ticketheader_member_assigned_id='' or ticketheader_member_assigned_id is null) AND h.ticket_group_administrator = " + current_user.id + ")  or (ticketstatus_id = 1 and h.ticket_group_administrator = " + current_user.id + ") or (ticketstatus_id in (8,9) and ifnull(ticketheader_member_assigned_id,0)= " + current_user.id + ")) ";
		sql += " ";
//		if (Convert.ToBoolean(Session["showall"]) == false)
//		{
//			sql += "AND a.ticket_header_expected_completion<='" + Convert.ToDateTime(Session["ticket_duedate"]).ToString("yyyy-MM_dd") + "' and a.ticket_header_expected_completion !='0001-01-01 00:00:00' ";
//		}
//		if (Convert.ToInt32(Session["ticket_pages"]) != 0)
//		{
//			sql += " AND h.ticket_group_id=" + Convert.ToInt32(Session["ticket_pages"]);
//		}
//		if (Convert.ToInt32(Session["ticket_type"]) != 0)
//		{
//			sql += " AND tickettype_id=" + Convert.ToInt32(Session["ticket_type"]);
//		}
//		if (!myMember.AuthenticatedForPrivilege(3, 131))
//		{
//			sql += " AND h.ticket_group_id!=4";
//		}
		// Not applicable
//		if (current_user.id != Convert.ToInt32(Session["ticket_user"]))
//		{
//			sql += " AND (a.ticketheader_private=0 ||(ticketheader_createdby_member_id=" + current_user.id + ")||(h.ticket_group_administrator=" + current_user.id + "))";
//		}
		sql += "  ";
	//	sql += "AND ticketheader_createdby_member_id = g.member_id ";
		sql += " ORDER BY ticketheader_id desc";
		var HeaderTable = Toolbox.doSQL_dt(sql,null);

				_tickets	= HeaderTable;//Toolbox.doSQL_dt(@"CALL GETTICKETS(@v0 , 'GetMyCourt')", new object[] {  current_user.id } );
				gv			= gv_waiting;
			break;
			case 2: // Your Tickets
				_tickets	= Toolbox.doSQL_dt(@"CALL GETTICKETS(@v0 , 'GetMyTickets')", new object[] {  current_user.id } );
				gv			= gv_mytickets;
			break;
			case 3: // New Tickets
				_tickets	= Toolbox.doSQL_dt(@"CALL GETTICKETS(@v0 , 'GetUnassignedTickets')", new object[] {  current_user.id } );
				gv			= gv_newtickets;
			break;
			case 4: // Tickets in your Group
				_tickets	= Toolbox.doSQL_dt(@"CALL GETTICKETS(@v0 , 'GetAllOpenTicketsfromGroup')", new object[] {  current_user.id } );
				gv			= gv_yourgroup;
			break;
			}
		if(gv != null)
			{
			gv.DataSource		= _tickets;
			gv.DataBind();
			}

		}
	protected void gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if(e.VisibleIndex > -1)
			{
			var gv			= (ASPxGridView) sender;
			var dr				= gv.GetDataRow(e.VisibleIndex);
			switch(e.DataColumn.FieldName)
				{
				case "ordered_id":
					var b		= (HtmlContainerControl) gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "bt");
					var priority_id			= dr["priority_id"] == DBNull.Value ? 0 : (int) dr["priority_id"];
					switch(priority_id)
						{
						case 0: // Null
						case 1: // Not assigned
							e.Cell.Style["background-color"]	= "#eee";
							b.Style["background-color"]			= "#eee";
						break;
						case 2: // Critical
							e.Cell.Style["background-color"]	= "#f00";
							b.Style["background-color"]			= "#f00";
							b.Style["color"]					= "#fff";
						break;
						case 3: // Urgent
							e.Cell.Style["background-color"]	= "#f90";
							b.Style["background-color"]			= "#f90 ";
						break;
						case 4: // Important
							e.Cell.Style["background-color"]	= "#ffcc00";
							b.Style["background-color"]			= "#ffcc00";
						break;
						case 5: // Minimum
							e.Cell.Style["background-color"]	= "#ffff00";
							b.Style["background-color"]			= "#ffff00";
						break;
						case 6: // Nice to have
							e.Cell.Style["background-color"]	= "#99cc99";
							b.Style["background-color"]			= "#99cc99";
						break;
						}
				break;
				}
			}
		}
	protected void ticket_cbp_Callback(object sender, CallbackEventArgsBase e)
		{
		if(e.Parameter.Contains("|"))
			{
			var paras			= e.Parameter.Split('|');
			var action			= paras[0];
			var ticket_id			= 0;
			var ticket		= new NETickets();
			switch(action)
				{
				case "cancelsearch":
					gv_newtickets.ClientVisible		= true;
					gv_waiting.ClientVisible		= true;
					gv_yourgroup.ClientVisible		= true;
					newtickets_title.Visible		= true;
					yourgroup_title.Visible			= true;
					waiting_title.Visible			= true;
					text_search.Text				= "";
					mytickets_title.InnerText		= "Tickets I Started";
					bt_cancel.ClientVisible			= false;
					Session["search_results"]		= null;
					populate(1);
					populate(2);
					populate(3);
					populate(4);
				break;
				case "search":
					var dt					= NETickets.GetCreatedissues(text_search.Text, (int) current_user.id);
					gv_newtickets.ClientVisible		= false;
					gv_waiting.ClientVisible		= false;
					gv_yourgroup.ClientVisible		= false;
					newtickets_title.Visible		= false;
					yourgroup_title.Visible			= false;
					waiting_title.Visible			= false;
					bt_cancel.ClientVisible			= true;
					mytickets_title.InnerText		= "Search Results";
					Session["search_results"]		= dt;
					gv_mytickets.DataSource			= (DataTable) Session["search_results"];
					gv_mytickets.DataBind();
				break;
				}
			}
		}
	
}
