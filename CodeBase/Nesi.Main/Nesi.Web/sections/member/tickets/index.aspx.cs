using System;
using System.Data;
using DevExpress.Web;
using DevExpress.Xpo;
using nesi.core;

public partial class sections_member_tickets_index : System.Web.UI.Page
{
    Toolbox _Tools = new Toolbox();
    NeMember current_user;
    int _PageAccess = 170;   //Page From DB
    protected void Page_Init(object sender, EventArgs e)
        {
        _Tools		= new Toolbox();
        current_user	= Toolbox.do_handle_authentication(_PageAccess);
        wiki_help.Visible = true;
        wiki_help.Attributes.Add("onclick", string.Format("page_obj.pop_wiki_help({0});", _PageAccess));
        var tickets = new NETickets();

        if (current_user.id != 8)
        {
            ASPxGridView2.DataSource = _Tools.getSQL_datatable(@"SELECT ticketmanager.ticketmanager_id, ticketmanager.ticketmanager_member_id, ticketmanager.ticketmanager_group_id FROM ticket_group,ticketmanager INNER JOIN member ON ticketmanager.ticketmanager_member_id = member.Member_ID  WHERE member.Member_Status = 'Active' and ticketmanager.ticketmanager_group_id=ticket_group_id and ticket_group_administrator =@v0", new object[] { current_user.id });
            SqlDataSource4.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group where ticket_group_administrator = " + current_user.id;
        }
        else
        {
            ASPxGridView2.DataSource = _Tools.getSQL_datatable(@"SELECT ticketmanager.ticketmanager_id, ticketmanager.ticketmanager_member_id, ticketmanager.ticketmanager_group_id FROM ticketmanager INNER JOIN member ON ticketmanager.ticketmanager_member_id = member.Member_ID  WHERE member.Member_Status = 'Active'" , null);
            SqlDataSource4.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group";
        }
        SqlDataSource4.DataBind();
        ASPxGridView2.DataBind();
        if ((current_user.member_isticket_admin > 0) || (_Tools.getSQL_int(@"Select count(ticketmanager_id) from ticketmanager  where ticketmanager_member_id =@v0", new object[] { current_user.id }) > 0))
        {

            if (current_user.member_isticket_admin > 0)
            {
                ASPxLabel1.ClientVisible = true;
            }
        }
        if (current_user.member_isticket_admin > 0)
        {
            ddluser.ClientEnabled = true;
        }
        populate_groups();
        if (!IsPostBack)
        {
            if (Session["ticket_user"] == null)
            {
                Session["ticket_user"] = current_user.id.ToString();
            }
            if (Session["ticket_duedate"] == null)
            {
            Session["ticket_duedate"] = System.DateTime.Today.AddDays(60);
            Session["showall"] = true;
            }
            if (Session["showall"] == null)
            {
                Session["showall"] = true;
            }
            if (Session["ticket_pages"] == null)
            {
                Session["ticket_pages"] = 0;
            }
            if (Session["ticket_type"] == null)
            {
                    Session["ticket_type"] = 0;
            }

            ddluser.Value = Convert.ToInt32(Session["ticket_user"]);
            dteduebefore.Date = Convert.ToDateTime(Session["ticket_duedate"]);
            hdnmemid.Value = ddluser.Value.ToString();
            chkshowall.Checked = Convert.ToBoolean(Session["showall"]);
            ddlgroup.Value = Convert.ToInt32(Session["ticket_pages"]);
            ddltype.Value = Convert.ToInt32(Session["ticket_type"]);
            Session["ticket_search"] = null;
        }
            load_buckets();
        }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["ticket_search"] = null;
            Session["ticket_search_string"] = null;

        }
        gv_search.DataSource = Session["ticket_search"];
        gv_search.DataBind();
    }
    protected void populate_groups()
    {
        if (current_user.AuthenticatedForPrivilege(131))  // if the people can see plan 17 tickets... 
        {

            ddlgroup.DataSource = _Tools.getSQL_datatable(@"SELECT
  0 ticket_group_id,
  'All Groups' ticket_group_name,
  0 ticket_group_administrator,
  0 order_n,
  0 active
UNION
SELECT
  ticket_group_id,
   ticket_group_name,
   ticket_group_administrator,
   order_n,
   active
FROM
  ticket_group
ORDER BY order_n", null);
            ddltype.DataSource = _Tools.getSQL_datatable(@"Select 0 tickettype_id,'All Types' tickettype_name UNION Select * from tickettype order by tickettype_id"  , null);
        }
        else
        {
            ddlgroup.DataSource = _Tools.getSQL_datatable(@"SELECT
  0 ticket_group_id,
  'All Groups' ticket_group_name,
  0 ticket_group_administrator,
  0 order_n,
  0 active
UNION
SELECT
  ticket_group_id,
 ticket_group_name,
   ticket_group_administrator,
   order_n,
   active
FROM
  ticket_group
WHERE ticket_group_id != 4
ORDER BY order_n", null);
            ddltype.DataSource = _Tools.getSQL_datatable(@"Select 0 tickettype_id,'All Types' tickettype_name UNION Select * from tickettype  where tickettype_id != 6 order by tickettype_id" , null);
        }
        ddltype.DataBind();
        ddlgroup.DataBind();


    }
    protected void load_buckets()
    {
        #region stuff waiting for me

        var sql = string.Format(@"
		SELECT 
			ticketheader_id, 
			wa.member_fullname waiting_name, 
			a.ticketheader_issue ticket,
			a.ticketheader_modified_date, 
			a.ticketheader_created_date DateCreated, 
			b.ticketpage_name PageName, 
			c.ticketstatus_status Status, 
			d.tickettype_name TypeOfTicket, 
			f.member_fullname assignedto, 
			g.member_fullname RaisedBy, 
			e.ticketpriority_name Priority, 
			h.ticket_group_name groupname ,
			a.ticket_header_expected_completion exp_fin, 
			a.ticketheader_priority_id priority_id, 
			a.ticketheader_status_id status_id, 
			b.ticketpage_ticket_group_id group_id 
		FROM 
            ticketheader a 
        LEFT JOIN 
			ticketpage b ON a.ticketheader_module_id = b.ticketpage_id
        LEFT JOIN 
			ticketstatus c ON a.ticketheader_status_id = c.ticketstatus_id
        LEFT JOIN 
			tickettype d ON a.ticketheader_issuetype = d.tickettype_id
        LEFT JOIN 
			ticketpriority e ON a.ticketheader_priority_id = e.ticketpriority_id
        LEFT JOIN 
			member f ON a.ticketheader_member_assigned_id = f.member_id 
        LEFT JOIN 
			member g ON a.ticketheader_createdby_member_id = g.member_id 
        LEFT JOIN 
			ticket_group h ON b.ticketpage_ticket_group_id = h.ticket_group_id  
		LEFT JOIN 
			member wa on a.ticketheader_waitingon_member_id = wa.member_id
		WHERE 
			(
			(ticketheader_createdby_member_id={0} AND ticketstatus_id = 7) OR 
			(ticketstatus_id=9 AND (ticketheader_member_assigned_id='' or ticketheader_member_assigned_id is null) AND h.ticket_group_administrator = {0})  or 
			(ticketstatus_id = 1 and h.ticket_group_administrator = {0}) or 
			(ticketstatus_id in (8,9) and IFNULL(ticketheader_member_assigned_id,0)= {0}) OR
			(ticketstatus_id = 3 AND ticketheader_waitingon_member_id = {0})
			) ", Session["ticket_user"]);
        if (current_user.id != Convert.ToInt32(Session["ticket_user"]))
        {
            sql += " AND (a.ticketheader_private=0  OR ticketheader_createdby_member_id=" + current_user.id + " OR h.ticket_group_administrator=" + current_user.id + ")";
        }
        Toolbox.do_debug("gvinmycourt start");
        sql += " ORDER BY ticketheader_id desc";
        var HeaderTable = _Tools.getSQL_datatable(sql,null);
        Toolbox.do_debug("gvinmycourt stop");

        gvinmycourt.DataSource = HeaderTable;
        gvinmycourt.DataBind();

		rp_waitingonme.HeaderText = "Waiting for me  (" + gvinmycourt.VisibleRowCount + ")";
		

        #endregion

        #region stuff im waiting for

        var sqlSelect = "SELECT ticketheader_id, wa.member_fullname waiting_name, a.ticketheader_issue AS ticket,a.ticketheader_modified_date, a.ticketheader_created_date AS DateCreated, b.ticketpage_name AS PageName, c.ticketstatus_status AS Status, d.tickettype_name AS TypeOfTicket, f.member_fullname AS assignedto, g.member_fullname AS RaisedBy, e.ticketpriority_name AS Priority, h.ticket_group_name as groupname ,a.ticket_header_expected_completion as exp_fin, ";
        sqlSelect += " a.ticketheader_priority_id priority_id, a.ticketheader_status_id status_id, b.ticketpage_ticket_group_id group_id ";
        sqlSelect += "FROM ticketheader AS a LEFT JOIN member AS f ON a.ticketheader_member_assigned_id = f.member_id left join ticketpriority e on a.ticketheader_priority_id = e.ticketpriority_id LEFT JOIN member wa on a.ticketheader_waitingon_member_id = wa.member_id, ticketpage AS b, ticketstatus AS c, tickettype AS d, member AS g, ticket_group as h ";
        
        sqlSelect += "WHERE ticketheader_module_id = ticketpage_id ";
        sqlSelect += "AND ticketheader_status_id = ticketstatus_id and ticketstatus_id!=5 ";
        sqlSelect += "AND ticketheader_issuetype = tickettype_id ";
        if (Convert.ToBoolean(Session["showall"]) == false)
        {
            sqlSelect += "AND a.ticket_header_expected_completion<='" + Convert.ToDateTime(Session["ticket_duedate"]).ToString("yyyy-MM_dd") + "' and a.ticket_header_expected_completion !='0001-01-01 00:00:00' ";
        }
        if (Convert.ToInt32(Session["ticket_pages"]) != 0)
        {
            sqlSelect += " AND h.ticket_group_id=" + Convert.ToInt32(Session["ticket_pages"]);
        }
        if (!current_user.AuthenticatedForPrivilege(131))
        {
            sqlSelect += " AND h.ticket_group_id!=4";
        }
        if (Convert.ToInt32(Session["ticket_type"]) != 0)
        {
            sqlSelect += " AND tickettype_id=" + Convert.ToInt32(Session["ticket_type"]);
        }
        if (current_user.id != Convert.ToInt32(Session["ticket_user"]))
        {
            sqlSelect += " AND (a.ticketheader_private=0 ||(ticketheader_createdby_member_id=" + current_user.id + ")||(h.ticket_group_administrator=" + current_user.id + "))";
        }
        sqlSelect += " AND (((ticketheader_createdby_member_id=" + Session["ticket_user"] + ") AND(ifnull(ticketheader_member_assigned_id,0)!= " + Session["ticket_user"] + ")) or ((ifnull(ticketheader_member_assigned_id,0)= " + Session["ticket_user"] + ") AND (ticketheader_createdby_member_id!=" + Session["ticket_user"] + ") AND (ticketstatus_id in (3,7))))";
    
        sqlSelect += " AND b.ticketpage_ticket_group_id = h.ticket_group_id ";
        sqlSelect += "AND ticketheader_createdby_member_id = g.member_id ";
        sqlSelect += " ORDER BY ticketheader_id desc";
        Toolbox.do_debug("gvistarted start");
        gvistarted.DataSource = _Tools.getSQL_datatable(sqlSelect,null);
        Toolbox.do_debug("gvistarted stop");
        gvistarted.DataBind();
		rp_waiting.HeaderText = "Stuff Im Waiting For  (" + gvistarted.VisibleRowCount  +")";

        #endregion
        #region cadence
        sqlSelect = "SELECT ticketheader_id, wa.member_fullname waiting_name, a.ticketheader_issue AS ticket,a.ticketheader_modified_date, a.ticketheader_created_date AS DateCreated, b.ticketpage_name AS PageName, c.ticketstatus_status AS Status, d.tickettype_name AS TypeOfTicket, f.member_fullname AS assignedto, g.member_fullname AS RaisedBy, e.ticketpriority_name AS Priority, h.ticket_group_name as groupname ,a.ticket_header_expected_completion as exp_fin, ";
        sqlSelect += " a.ticketheader_priority_id priority_id, a.ticketheader_status_id status_id, b.ticketpage_ticket_group_id group_id ";
        sqlSelect += "FROM ticketheader AS a LEFT JOIN member AS f ON a.ticketheader_member_assigned_id = f.member_id LEFT JOIN member wa on a.ticketheader_waitingon_member_id = wa.member_id, ticketpage AS b, ticketstatus AS c, tickettype AS d, ticketpriority AS e, member AS g, ticket_group as h ";
        sqlSelect += "WHERE ticketheader_module_id = ticketpage_id ";
        sqlSelect += "AND ticketheader_status_id = ticketstatus_id and ticketstatus_id!=5 ";
        sqlSelect += "AND ticketheader_issuetype = tickettype_id ";
        if (Convert.ToBoolean(Session["showall"]) == false)
        {
            sqlSelect += "AND a.ticket_header_expected_completion<='" + Convert.ToDateTime(Session["ticket_duedate"]).ToString("yyyy-MM_dd") + "' and a.ticket_header_expected_completion !='0001-01-01 00:00:00' ";
        }
        if (Convert.ToInt32(Session["ticket_pages"]) != 0)
        {
            sqlSelect += " AND h.ticket_group_id=" + Convert.ToInt32(Session["ticket_pages"]);
        }
        if (!current_user.AuthenticatedForPrivilege(131)) //
        {
            sqlSelect += " AND h.ticket_group_id!=4";
        }
        if (current_user.id != Convert.ToInt32(Session["ticket_user"]))
        {
            sqlSelect += " AND (a.ticketheader_private=0 ||(ticketheader_createdby_member_id=" + current_user.id + ")||(h.ticket_group_administrator=" + current_user.id + "))";
        }
        if (Convert.ToInt32(Session["ticket_type"]) != 0)
        {
            sqlSelect += " AND tickettype_id=" + Convert.ToInt32(Session["ticket_type"]);
        }
        if (ddltype.Value != null && ddltype.Value.ToString() != "6")  // if a type is selected that is not the LT objectives
        {
//			sqlSelect += " AND ((h.ticket_group_administrator = " + Session["ticket_user"] + ") OR (ticketheader_member_assigned_id= " + Session["ticket_user"] + "))";
            sqlSelect += " AND ((h.ticket_group_administrator = " + Session["ticket_user"] + "))";

        }
        else
        {
            sqlSelect += " AND (a.ticketheader_objective_owner = " + Session["ticket_user"] + ") ";
        }
        sqlSelect += " AND ticketheader_priority_id = ticketpriority_id and b.ticketpage_ticket_group_id = h.ticket_group_id ";
        sqlSelect += "AND ticketheader_createdby_member_id = g.member_id ";
        sqlSelect += " ORDER BY ticketheader_id desc";
        Toolbox.do_debug("gvcadence start");
        HeaderTable = _Tools.getSQL_datatable(sqlSelect,null);
        Toolbox.do_debug("gvcadence stop");
        gvcadence.DataSource = HeaderTable;
        gvcadence.DataBind();
        if (ddltype.Value != null && ddltype.Value.ToString() == "6")
        {
			rp_myresponsiblity.HeaderText = "LT Objective List  (" + gvcadence.VisibleRowCount + ")";
        }
        else
        {

			rp_myresponsiblity.HeaderText = "Stuff in My Group(s)  (" + gvcadence.VisibleRowCount + ")";
        }

#endregion
        #region my recent closed tickets
        sqlSelect = "SELECT ticketheader_id, wa.member_fullname waiting_name, a.ticketheader_issue AS ticket,a.ticketheader_modified_date, a.ticketheader_created_date AS DateCreated, b.ticketpage_name AS PageName, c.ticketstatus_status AS Status, d.tickettype_name AS TypeOfTicket, f.member_fullname AS assignedto, g.member_fullname AS RaisedBy, e.ticketpriority_name AS Priority, h.ticket_group_name as groupname ,a.ticket_header_expected_completion as exp_fin, ";
        sqlSelect += "a.ticketheader_priority_id priority_id, a.ticketheader_status_id status_id, b.ticketpage_ticket_group_id group_id ";
        sqlSelect += "FROM ticketheader AS a LEFT JOIN member AS f ON a.ticketheader_member_assigned_id = f.member_id LEFT JOIN member wa on a.ticketheader_waitingon_member_id = wa.member_id, ticketpage AS b, ticketstatus AS c, tickettype AS d, ticketpriority AS e, member AS g, ticket_group as h ";
        sqlSelect += "WHERE ticketheader_module_id = ticketpage_id ";
        sqlSelect += "AND ticketheader_status_id = ticketstatus_id ";
        sqlSelect += "AND ticketheader_issuetype = tickettype_id ";
        sqlSelect += "AND ticketheader_status_id = 5 "; //5 is the status of a Close Ticket
        
        if (Convert.ToInt32(Session["ticket_pages"]) != 0)
        {
            sqlSelect += " AND h.ticket_group_id=" + Convert.ToInt32(Session["ticket_pages"]);
        }
        if (!current_user.AuthenticatedForPrivilege(131))
        {
            sqlSelect += " AND h.ticket_group_id!=4";
        }
        if (Convert.ToInt32(Session["ticket_type"]) != 0)
        {
            sqlSelect += " AND tickettype_id=" + Convert.ToInt32(Session["ticket_type"]);
        }
        sqlSelect += " AND (ticketheader_createdby_member_id=" + Session["ticket_user"] + " OR h.ticket_group_administrator=" + Session["ticket_user"] + " OR a.ticketheader_member_assigned_id = "+Session["ticket_user"]+")";
        sqlSelect += " AND ticketheader_closed_date>curdate()-interval 7 day";
        sqlSelect += " AND ticketheader_priority_id = ticketpriority_id ";
        sqlSelect += "AND ticketheader_createdby_member_id = g.member_id and b.ticketpage_ticket_group_id = h.ticket_group_id ";
        sqlSelect += " ORDER BY a.ticket_header_expected_completion DESC, ticketheader_modified_date DESC";
        Toolbox.do_debug("gv_closed0 start");
        HeaderTable = _Tools.getSQL_datatable(sqlSelect,null);
        Toolbox.do_debug("gv_closed0 stop");
        gv_closed0.DataSource = HeaderTable;
        gv_closed0.DataBind();
		rp_closedtickets.HeaderText = "My Tickets Closed Last Week  (" + gv_closed0.VisibleRowCount + ")";

#endregion
        #region in progress
        sqlSelect = "SELECT ticketheader_id, wa.member_fullname waiting_name, a.ticketheader_issue AS ticket,a.ticketheader_modified_date, a.ticketheader_created_date AS DateCreated, b.ticketpage_name AS PageName, c.ticketstatus_status AS Status, d.tickettype_name AS TypeOfTicket, f.member_fullname AS assignedto, g.member_fullname AS RaisedBy, e.ticketpriority_name AS Priority, h.ticket_group_name as groupname ,a.ticket_header_expected_completion as exp_fin, ";
        sqlSelect += " a.ticketheader_priority_id priority_id, a.ticketheader_status_id status_id, b.ticketpage_ticket_group_id group_id ";
        sqlSelect += "FROM ticketheader AS a LEFT JOIN member AS f ON a.ticketheader_member_assigned_id = f.member_id LEFT JOIN member wa on a.ticketheader_waitingon_member_id = wa.member_id, ticketpage AS b, ticketstatus AS c, tickettype AS d, ticketpriority AS e, member AS g, ticket_group as h ";
        sqlSelect += "WHERE ticketheader_module_id = ticketpage_id ";
        sqlSelect += "AND ticketheader_status_id = ticketstatus_id and ticketstatus_id!=5 AND ticketheader_status_id = ticketstatus_id and ticketstatus_id!=3 AND ticketheader_status_id = ticketstatus_id and (ticketstatus_id<7 || ticketstatus_id>=10) ";
        sqlSelect += "AND ticketheader_issuetype = tickettype_id ";
        if (Convert.ToBoolean(Session["showall"]) == false)
        {
            sqlSelect += "AND a.ticket_header_expected_completion<='" + Convert.ToDateTime(Session["ticket_duedate"]).ToString("yyyy-MM_dd") + "' and a.ticket_header_expected_completion !='0001-01-01 00:00:00' ";
        }
        if (Convert.ToInt32(Session["ticket_pages"]) != 0)
        {
            sqlSelect += " AND h.ticket_group_id=" + Convert.ToInt32(Session["ticket_pages"]);
        }
        if (!current_user.AuthenticatedForPrivilege(131))
        {
            sqlSelect += " AND h.ticket_group_id!=4";
        }
        if (current_user.id != Convert.ToInt32(Session["ticket_user"]))
        {
            sqlSelect += " AND a.ticketheader_private=0";
        }
        if (Convert.ToInt32(Session["ticket_type"]) != 0)
        {
            sqlSelect += " AND tickettype_id=" + Convert.ToInt32(Session["ticket_type"]);
        }
        sqlSelect += " AND ((ticketheader_member_assigned_id= " + Session["ticket_user"] + "))";
        sqlSelect += " AND ticketheader_priority_id = ticketpriority_id and b.ticketpage_ticket_group_id = h.ticket_group_id ";
        sqlSelect += "AND ticketheader_createdby_member_id = g.member_id ";
        sqlSelect += " ORDER BY a.ticket_header_expected_completion DESC, e.ticketpriority_name,ticketheader_id ";
        Toolbox.do_debug("gv_progres start");
        HeaderTable = _Tools.getSQL_datatable(sqlSelect,null);
        Toolbox.do_debug("gv_progres stop");
        gv_progres.DataSource = HeaderTable;
        gv_progres.DataBind();
        rp_workingon.HeaderText = "Stuff Im Working On  (" + gv_progres.VisibleRowCount + ")";
		 
        #endregion
		#region watching
        sqlSelect = "SELECT ticketheader_id, wa.member_fullname waiting_name, a.ticketheader_issue AS ticket,a.ticketheader_modified_date, a.ticketheader_created_date AS DateCreated, b.ticketpage_name AS PageName, c.ticketstatus_status AS Status, d.tickettype_name AS TypeOfTicket, f.member_fullname AS assignedto, g.member_fullname AS RaisedBy, e.ticketpriority_name AS Priority, h.ticket_group_name as groupname ,a.ticket_header_expected_completion as exp_fin, ";
        sqlSelect += " a.ticketheader_priority_id priority_id, a.ticketheader_status_id status_id, b.ticketpage_ticket_group_id group_id ";
        sqlSelect += "FROM ticketheader AS a LEFT JOIN member AS f ON a.ticketheader_member_assigned_id = f.member_id LEFT JOIN member wa on a.ticketheader_waitingon_member_id = wa.member_id, ticketpage AS b, ticketstatus AS c, tickettype AS d, ticketpriority AS e, member AS g, ticket_group as h, ticket_memberview as i ";
        sqlSelect += "WHERE ticketheader_module_id = ticketpage_id ";
        sqlSelect += "AND ticketheader_status_id = ticketstatus_id and ticketstatus_id!=5  ";
        sqlSelect += "AND ticketheader_issuetype = tickettype_id ";
        sqlSelect += "AND ticketheader_id = i.ticket_id ";
        sqlSelect += "AND (i.member_id = " + Session["ticket_user"] + ")";
        if (Convert.ToBoolean(Session["showall"]) == false)
        {
            sqlSelect += "AND a.ticket_header_expected_completion<='" + Convert.ToDateTime(Session["ticket_duedate"]).ToString("yyyy-MM_dd") + "' and a.ticket_header_expected_completion !='0001-01-01 00:00:00' ";
        }
        if (Convert.ToInt32(Session["ticket_pages"]) != 0)
        {
            sqlSelect += " AND h.ticket_group_id=" + Convert.ToInt32(Session["ticket_pages"]);
        }
        if (!current_user.AuthenticatedForPrivilege(131))
        {
            sqlSelect += " AND h.ticket_group_id!=4";
        }
        if (Convert.ToInt32(Session["ticket_type"]) != 0)
        {
            sqlSelect += " AND tickettype_id=" + Convert.ToInt32(Session["ticket_type"]);
        }
        if (current_user.id != Convert.ToInt32(Session["ticket_user"]))
        {
            sqlSelect += " AND (a.ticketheader_private=0 ||(ticketheader_createdby_member_id=" + current_user.id + ")||(h.ticket_group_administrator=" + current_user.id + "))";
        }
        sqlSelect += " AND ticketheader_priority_id = ticketpriority_id and b.ticketpage_ticket_group_id = h.ticket_group_id ";
        sqlSelect += "AND ticketheader_createdby_member_id = g.member_id ";
        sqlSelect += " ORDER BY a.ticket_header_expected_completion DESC, ticketheader_priority_id,ticketheader_id ";
        Toolbox.do_debug("gv_watching start");
        HeaderTable = _Tools.getSQL_datatable(sqlSelect,null);
        Toolbox.do_debug("gv_watching stop");
        gv_watching.DataSource = HeaderTable;
        gv_watching.DataBind();
		#endregion watching
		
        rp_watching.HeaderText = "Other Stuff Im Watching  (" + gv_watching.VisibleRowCount + ")";

        gvcadence.FocusedRowIndex = -1;
        gv_watching.FocusedRowIndex = -1;
        gvistarted.FocusedRowIndex = -1;
        gvinmycourt.FocusedRowIndex = -1;
        gv_progres.FocusedRowIndex = -1;
        gv_watching.FocusedRowIndex = -1;
        gv_closed0.FocusedRowIndex = -1;
    }
    protected void lbvct0_Click(object sender, EventArgs e)
    {

    }
    protected void ASPxGridView2_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {


        var memid = Convert.ToInt32(e.NewValues["ticketmanager_member_id"]);
        var groupid = Convert.ToInt32(e.NewValues["ticketmanager_group_id"]);
        if (_Tools.getSQL_int(@"select count(ticketmanager_id) from ticketmanager  where ticketmanager_member_id =@v0 and ticketmanager_group_id =@v1 ", new object[] { memid,groupid }) >= 1)
        {
            e.Cancel = true;
            ASPxGridView2.CancelEdit();
            return;
        }
        var tm		= new NETickets.ticketmanager();
        tm.member_id					= memid;
        tm.group_id						= groupid;
        tm.save();

        e.Cancel = true;

        ASPxGridView2.CancelEdit();
        if (current_user.id != 8)
        {
            ASPxGridView2.DataSource = _Tools.getSQL_datatable(@"SELECT ticketmanager.ticketmanager_id, ticketmanager.ticketmanager_member_id, ticketmanager.ticketmanager_group_id FROM ticket_group,ticketmanager INNER JOIN member ON ticketmanager.ticketmanager_member_id = member.Member_ID  WHERE member.Member_Status = 'Active' and ticketmanager.ticketmanager_group_id=ticket_group_id and ticket_group_administrator =@v0", new object[] { current_user.id });
            SqlDataSource4.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group where ticket_group_administrator = " + current_user.id;
        }
        else
        {
            ASPxGridView2.DataSource = _Tools.getSQL_datatable(@"SELECT ticketmanager.ticketmanager_id, ticketmanager.ticketmanager_member_id, ticketmanager.ticketmanager_group_id FROM ticketmanager INNER JOIN member ON ticketmanager.ticketmanager_member_id = member.Member_ID  WHERE member.Member_Status = 'Active'" , null);
            SqlDataSource4.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group";
        }
    
        rp_waitingonme.ClientVisible = false;
    }
    protected void ASPxGridView2_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        NETickets.ticketmanager.delete(Convert.ToInt32(e.Keys[0]));

        if (current_user.id != 8)
        {
            ASPxGridView2.DataSource = _Tools.getSQL_datatable(@"SELECT ticketmanager.ticketmanager_id, ticketmanager.ticketmanager_member_id, ticketmanager.ticketmanager_group_id FROM ticket_group,ticketmanager INNER JOIN member ON ticketmanager.ticketmanager_member_id = member.Member_ID  WHERE member.Member_Status = 'Active' and ticketmanager.ticketmanager_group_id=ticket_group_id and ticket_group_administrator =@v0", new object[] { current_user.id });
            SqlDataSource4.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group where ticket_group_administrator = " + current_user.id;
        }
        else
        {
            ASPxGridView2.DataSource = _Tools.getSQL_datatable(@"SELECT ticketmanager.ticketmanager_id, ticketmanager.ticketmanager_member_id, ticketmanager.ticketmanager_group_id FROM ticketmanager INNER JOIN member ON ticketmanager.ticketmanager_member_id = member.Member_ID  WHERE member.Member_Status = 'Active'" , null);
            SqlDataSource4.SelectCommand = "Select ticket_group_id,ticket_group_name from ticket_group";
        }
        
        rp_waitingonme.ClientVisible = false;

        e.Cancel = true;
        ASPxGridView2.CancelEdit();
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
    }
    private System.Drawing.Color html_color(string html_color)
        {
        return System.Drawing.ColorTranslator.FromHtml(html_color);
        }
    protected void gvinmycourt_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        var gv = (ASPxGridView)sender;
        #region priority colors
        var ds					= gv.DataSource;
        var gv_id				= gv.ID;
        var _priority_id			= gv.GetDataRow(e.VisibleIndex) != null ? gv.GetDataRow(e.VisibleIndex)["priority_id"].ToString() : "";
        var _status_id			= gv.GetDataRow(e.VisibleIndex) != null ? gv.GetDataRow(e.VisibleIndex)["status_id"].ToString() : "";
        var created_name			= gv.GetDataRow(e.VisibleIndex) != null ? gv.GetDataRow(e.VisibleIndex)["RaisedBy"].ToString() : "";
        var waiting_name			= gv.GetDataRow(e.VisibleIndex) != null && gv.GetDataRow(e.VisibleIndex)["waiting_name"] != null ? gv.GetDataRow(e.VisibleIndex)["waiting_name"].ToString() : "";
        var _ticket_id				= gv.GetDataRow(e.VisibleIndex) != null ? (int) gv.GetDataRow(e.VisibleIndex)["ticketheader_id"] : 0;

			var _fullname		= waiting_name != "" ? waiting_name : created_name;
        if (e.DataColumn.FieldName == "priority_id")
            {
            var priority_id		= 0;
            int.TryParse(_priority_id, out priority_id);
            switch(priority_id)
                {
                case 1:
                    e.Cell.Text = "NR";
                    e.Cell.ToolTip = "Not Ranked";
                break;
                case 2:
                    e.Cell.BackColor = html_color("#f00");
                    e.Cell.ForeColor = html_color("#fff");
                    e.Cell.Text = "C";
                    e.Cell.ToolTip = "Critical";
                break;
                case 3:
                    e.Cell.BackColor = html_color("#fa0");
                    e.Cell.Text = "U";
                    e.Cell.ToolTip = "Urgent";
                break;
                case 4:
                    e.Cell.BackColor = html_color("#ffcc00");
                    e.Cell.Text = "I";
                    e.Cell.ToolTip = "Important";
                break;
                case 5:
                    e.Cell.BackColor = html_color("#99cc99");
                    e.Cell.Text = "N";
                    e.Cell.ToolTip = "Nice to Have";
                break;
                case 6:
                e.Cell.BackColor = html_color("#ffff00");
                    e.Cell.Text = "M";
                    e.Cell.ToolTip = "Minimum";
                break;
                }
            }
        #endregion
        #region status colors

        else if (e.DataColumn.FieldName == "status_id" && e.CellValue != null)
            {
            var status_id		= 0;
            int.TryParse(_status_id, out status_id);
            switch(status_id)
                {
                case 1:
                    e.Cell.BackColor = html_color("#ccc");
                    e.Cell.Text = "NA";
                    e.Cell.ToolTip = "Not Assigned";
                break;
                case 2:
                    e.Cell.BackColor = html_color("#0cf");
                    e.Cell.Text = "IP";
                    e.Cell.ToolTip = "In Progress";
                break;
                case 3:
                    e.Cell.BackColor = html_color("#00f");
                    e.Cell.ForeColor = html_color("#fff");
                    e.Cell.Text = "WU";
                    e.Cell.ToolTip = "Waiting for Response from "+_fullname;
                break;
                case 5:
                    e.Cell.BackColor = html_color("#333");
                    e.Cell.ForeColor = html_color("#fff");
                    e.Cell.Text = "C";
                    e.Cell.ToolTip = "Closed";
                break;
                case 6:
                    e.Cell.BackColor = html_color("#9c6");
                    e.Cell.Text = "R";
                    e.Cell.ToolTip = "Read";
                break;
                case 7:
                    e.Cell.BackColor = html_color("#f3f");
                    e.Cell.ForeColor = html_color("#fff");
                    e.Cell.Text = "OC";
                    e.Cell.ToolTip = "Waiting for Opener to Close";
                break;
                case 8:
                    e.Cell.BackColor = html_color("#ffc");
                    e.Cell.Text = "A";
                    e.Cell.ToolTip = "Assigned";
                break;
                case 9:
                    e.Cell.BackColor = html_color("#0f0");
                    e.Cell.Text = "AU";
                    e.Cell.ToolTip = "Answered by "+_fullname;
                break;
                    case 10:
                        e.Cell.BackColor = html_color("#ee8e4a");
                        e.Cell.ForeColor = html_color("#FFF");
                        e.Cell.Text = "BR";
                        e.Cell.ToolTip = "Waiting for beta release";
                        break;
                    case 12:
                        e.Cell.BackColor = html_color("#808");
                        e.Cell.ForeColor = html_color("#fff");
                        e.Cell.Text = "FR";
                        e.Cell.ToolTip = "Waiting for final release";
                        break;
            }
            }
        #endregion
        #region status colors

        else if (e.DataColumn.FieldName == "ticket")
        {
            try
            {
                if ((gv.GetRowValuesByKeyValue(e.KeyValue, "exp_fin") != DBNull.Value)&&(Convert.ToDateTime(gv.GetRowValuesByKeyValue(e.KeyValue, "exp_fin")) > new System.DateTime(2005,1,1)) && (gv.GetRowValuesByKeyValue(e.KeyValue, "exp_fin").ToString() != ""))
                {
                    if ((Convert.ToDateTime(gv.GetRowValuesByKeyValue(e.KeyValue, "exp_fin")) > System.DateTime.Today) && (Convert.ToDateTime(gv.GetRowValuesByKeyValue(e.KeyValue, "exp_fin")) < System.DateTime.Today.AddDays(7)))
                    {
                        e.Cell.BackColor = html_color("#fa0");
                        //e.Cell.ToolTip = s.GetRowValuesByKeyValue(e.KeyValue, "exp_fin").ToString();
                    }
                    else if (Convert.ToDateTime(gv.GetRowValuesByKeyValue(e.KeyValue, "exp_fin")) < System.DateTime.Today.AddDays(1))
                    {
                        e.Cell.BackColor = html_color("#f00");
                        e.Cell.ForeColor = html_color("#fff");
                        //e.Cell.ToolTip = s.GetRowValuesByKeyValue(e.KeyValue, "exp_fin").ToString();
                    }
                }
            }
            catch { }
            
        }
        #endregion
    }
    protected void lblfilter_click(object sender, EventArgs e)
    {
        if (ddluser.SelectedIndex > -1)
        {
            Session["ticket_user"] = ddluser.Value.ToString();
            Session["ticket_duedate"] = dteduebefore.Date;
            Session["showall"] = chkshowall.Checked;
            Session["ticket_pages"] = ddlgroup.Value;
            Session["ticket_type"] = ddltype.Value;
            hdnmemid.Value = ddluser.Value.ToString();

            load_buckets();
        }
        
    }
    protected void ASPxLabel2_DataBound(object sender, EventArgs e)
    {
            var l                             = (ASPxLabel)sender;
            var tc	= (GridViewDataItemTemplateContainer) l.NamingContainer;
            var dr								= tc.Grid.GetDataRow(tc.VisibleIndex);
            var gv                         = tc.Grid;

            var id                                  = Convert.ToInt32(dr["ticketheader_id"]);

			var created_name		= dr["RaisedBy"].ToString();
			var status_id		= dr["status_id"].ToString();
			var waiting_name		= Toolbox.ReturnBlankIfNull_string(dr["waiting_name"]);

			var _fullname		= waiting_name != "" ? waiting_name : created_name;

            var subject                          = dr["ticket"].ToString();
            var createdby                        = dr["raisedby"].ToString();
            var groupname                        = dr["groupname"].ToString();
            var subgroup                         = dr["pagename"].ToString();
            var status                           = dr["status"].ToString();
			switch(status_id)
				{
				case "3":
					status							= "Waiting for reply from "+_fullname;
				break;
				case "9":
					status							= "Answered by "+_fullname;
				break;
				}
            var priority                         = dr["priority"].ToString();
            var expectedfinish                   = dr["exp_fin"].ToString();
            var assignedto                        = dr["assignedto"].ToString();
            var _type                            = dr["typeofticket"].ToString();
            var datecut                          = Convert.ToDateTime(dr["datecreated"].ToString()).ToString("yyyy-MM-dd"); 
            var temp_expectedfinish		    = new DateTime();
            DateTime.TryParse(expectedfinish, out temp_expectedfinish);
            expectedfinish						= temp_expectedfinish.Year < 2000 ? "" : temp_expectedfinish.ToString("yyyy-MM-dd");


            var html = string.Format(@"<table border='0' cellpadding='1' cellspacing='1' style='width: 500px;'>
            <tbody>
                <tr>
                    <td nowrap='nowrap' width='100'>Created By:</td>
                    <td>{2}</td>
                </tr>
                <tr>
                    <td>Group:</td>
                    <td>{3}</td>
                </tr>
                <tr>
                    <td nowrap='nowrap'>Sub Group:</td>
                    <td>{4}</td>
                </tr>
                <tr>
                    <td>Status:</td>
                    <td>{5}</td>
                </tr>
                <tr>
                    <td>Type:</td>
                    <td>{9}</td>
                </tr>
                <tr>
                    <td>Priority:</td>
                    <td>{6}</td>
                </tr>
                <tr>
                    <td nowrap='nowrap'>Assigned To:</td>
                    <td>{8}</td>
                </tr>
                <tr>
                    <td nowrap='nowrap'>Expected Finish:</td>
                    <td>{7}</td>
                </tr>
                <tr>
                    <td nowrap='nowrap'>Date Cut:</td>
                    <td>{10}</td>
                </tr>
            </tbody>
        </table>", id, subject, createdby, groupname, subgroup, status, priority, expectedfinish, assignedto,_type,datecut);

            l.Text = id + " - " + subject; 


            l.Attributes.Add("data-tooltip", html);
            l.Attributes.Add("data-title", l.Text);
            l.CssClass = "ttip";
    }
    protected void gv_search_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters != (Convert.ToString(Session["ticket_search_string"])))
        {
            
                var tickets = new NETickets();
                var possible_ticket_id		= 0;
                int.TryParse(e.Parameters, out possible_ticket_id);
                var do_search				= true;
                if(possible_ticket_id > 0)
                    {
                    // It is numeric, now check if there is a ticket out there....
                    var c			= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM ticketheader WHERE ticketheader_id = @v0", possible_ticket_id);
                    if(c > 0)
                        {
                        // Valid ticket, do redirect.
                        do_search			= false;
                        gv_search.JSProperties["cpredir"]		= e.Parameters;
                        }
                    else
                        {
                        // It's numeric, but be searched for.
                        gv_search.JSProperties["cpredir"]		= "";
                        do_search			= true;
                        }
                    }
                else
                    {
                    gv_search.JSProperties["cpredir"]		= "";
                    }
            if(do_search)
                {
                var issues = NETickets.GetCreatedissues(e.Parameters, (int) current_user.id);
                Session["ticket_search"] = issues;
                Session["ticket_search_string"] = e.Parameters;
                }
            
        }

        gv_search.DataSource = Session["ticket_search"];
        gv_search.DataBind();
        

    }
    protected void gv_search_PageIndexChanged(object sender, EventArgs e)
    {
        gv_search.DataSource = Session["ticket_search"];
        gv_search.DataBind();
        
    }
    protected void gv_search_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
    {
        gv_search.DataSource = Session["ticket_search"];
        gv_search.DataBind();
        
    }
    protected void gv_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
        if(e.VisibleIndex >= 0)
            {
            var gv					= (ASPxGridView) sender;
            var dr						= gv.GetDataRow(e.VisibleIndex);
            if(dr != null)
                {
                var ticket_id					= Convert.ToInt32(dr["ticketheader_id"]);
                e.Row.Attributes["onclick"]		= string.Format("javascript:boing('/sections/member/tickets/ticketpage.aspx?issue={0}', 'tickets{0}',1095,775)", ticket_id);
                }
            }
        }
}
