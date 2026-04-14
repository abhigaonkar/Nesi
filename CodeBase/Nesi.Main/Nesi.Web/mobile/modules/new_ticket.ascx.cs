using DevExpress.Web;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using nesi.core;

public partial class mobile_modules_new_ticket : System.Web.UI.UserControl
	{
	bool auth_for_leadership_team		= false;
	NeMember current_user;
	NameValueCollection _q;
	public string ticket_id	{get;set;}
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user					= Toolbox.do_handle_authentication(1);
		_q								= Request.QueryString;
		if(!string.IsNullOrEmpty(_q["do"]))
			{
			switch(_q["do"])
				{
				case "new":
					ticket_created.Visible		= false;
					new_ticket.Visible			= true;
				break;
				case "after":
					ticket_created.Visible		= true;
					new_ticket.Visible			= false;
					ticket_id					= _q["ticket_id"];
				break;
				}
			}
		else
			{
			new_ticket.Visible					= true;
			}
		populate_groups();
		populate_subgroups();
		populate_types();
		if(!IsPostBack)
			{
			Session["force_start"]		= null;
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		ASPxWebControl.RegisterBaseScript(Page);
		}
	protected void Page_PreRender(object sender, EventArgs e)
		{
		populate_related();
		fill_recent_tickets();
		}
	private void populate_groups()
		{
		using(var uow = new UnitOfWork())
			{
			ddl_group.DataSource				= (from ad in new XPQuery<ne_xpo.cs.ticket_group>(uow)
													where (!auth_for_leadership_team ? ad.ticket_group_id != 4 : true)
														select new 
															{
															id	= ad.ticket_group_id,
															name = ad.ticket_group_name+" ("+ad.ticket_group_administrator.member_fullname+")",
															order = ad.order_n
															}).ToList().OrderBy(x => x.order);
			ddl_group.DataBind();
			}
		}
	public void populate_subgroups()
		{
		if(!string.IsNullOrEmpty(ddl_group.SelectedValue) && ddl_group.SelectedValue != "0")
			{
			ddl_subgroup.DataSource			= Toolbox.doSQL_dt(@"SELECT ticketpage_id id,ticketpage_name name FROM ticketpage WHERE ticketpage_ticket_group_id = @v0  ORDER BY ticketpage_name", new object[] {  ddl_group.SelectedValue } );
			ddl_subgroup.DataBind();
			}
		if(!IsPostBack) // Needed for adding placeholder
			{
			ddl_subgroup.DataBind();
			}
		}
	public void populate_types()
		{
		if(!string.IsNullOrEmpty(ddl_group.SelectedValue) && ddl_group.SelectedValue != "0")
			{
			ddl_type.DataSource			= Toolbox.doSQL_dt(@" SELECT DISTINCT a.tickettype_id id, a.tickettype_name name FROM tickettype a INNER JOIN ticket_group_type_link b ON a.tickettype_id = b.ticket_type_id INNER JOIN ticketpage c ON c.ticketpage_ticket_group_id = b.ticket_group_id WHERE c.ticketpage_ticket_group_id = @v0 ", new object[] {  ddl_group.SelectedValue } );
			ddl_type.DataBind();
			}
		if(!IsPostBack) // Needed for adding placeholder
			{
			ddl_type.DataBind();
			}
		}
	private void show_ticket_fields()
		{
		related_tickets.Visible			= false;
		recent_tickets.Visible			= false;
		fields.Style["display"]			= "block";
		}
	private void hide_ticket_fields()
		{
		recent_tickets.Visible			= true;
		related_tickets.Visible			= tb_topic.Text.Trim() != "";
		fields.Style["display"]			= "none";
		}
	private void populate_related()
		{
		var force_start		=  Session["force_start"] != null ? (string) Session["force_start"] : "";

		if(tb_topic.Text != "")
			{
			var dt			= NETickets.GetCreatedissues(tb_topic.Text, (int) current_user.id);
			var results		= dt.Clone();

			if(dt.Rows.Count > 0)
				{
				var this_group_id			= Convert.ToInt32(ddl_group.SelectedValue);
				var this_subgroup_id		= Convert.ToInt32(ddl_subgroup.SelectedValue);
				var this_type_id			= Convert.ToInt32(ddl_type.SelectedValue);
				foreach(DataRow dr in dt.Rows)
					{
					var ticket_id			= (int) dr["id"];
					var group_id			= (int) dr["groupid"];
					var subgroup_id			= (int) dr["pageid"];
					var status				= (int) dr["status_id"];
					var type_id				= (int) dr["typeid"];
					if(group_id == this_group_id && subgroup_id == this_subgroup_id && type_id == this_type_id && !Toolbox.Contains(status, new []{5,7}))
						{
						results.ImportRow(dr);
						}
					else if(this_group_id != 0 && this_group_id == group_id && this_subgroup_id == 0 && this_type_id == 0)
						{
						results.ImportRow(dr);
						}
					else if(this_subgroup_id != 0 && this_subgroup_id == subgroup_id && this_type_id == 0)
						{
						results.ImportRow(dr);
						}
					}
				if(results.Rows.Count > 0 && force_start != "1")
					{
					gv_related.DataSource	= results;
					gv_related.DataBind();
					hide_ticket_fields();
					recent_tickets.Visible		= false;
					bt_startnew.Visible			= true;
					}
				else
					{
					show_ticket_fields();
					}
				}
			else
			    {
			    show_ticket_fields();
			    }
        }
		else
			{
			hide_ticket_fields();
			}
		}
	protected void ddl_group_DataBound(object sender, EventArgs e)
		{
		if(ddl_group.Items.FindByValue("0") == null)
			{
			var li			= new ListItem();
			li.Value			= "0";
			li.Text				= "Group";
			ddl_group.Items.Insert(0, li);
			}
		}
	protected void ddl_subgroup_DataBound(object sender, EventArgs e)
		{
		if(ddl_subgroup.Items.FindByValue("0") == null)
			{
			var li			= new ListItem();
			li.Value			= "0";
			li.Text				= "Sub Group";
			ddl_subgroup.Items.Insert(0, li);
			}
		}
	protected void ddl_type_DataBound(object sender, EventArgs e)
		{
		if(ddl_type.Items.FindByValue("0") == null)
			{
			var li			= new ListItem();
			li.Value			= "0";
			li.Text				= "Type";
			ddl_type.Items.Insert(0, li);
			}
		}
	protected void ddl_group_SelectedIndexChanged(object sender, EventArgs e)
		{
		if(ddl_group.SelectedValue != "0")
			{
			ddl_subgroup.Enabled	= true;
			ddl_type.Enabled		= true;
			populate_subgroups();
			populate_types();
			populate_related();
			}
		else
			{
			ddl_subgroup.SelectedValue	= "0";
			ddl_type.SelectedValue		= "0";
			tb_topic.Text				= "";
			ddl_subgroup.Enabled		= false;
			ddl_type.Enabled			= false;
			}
		}
	protected void ddl_subgroup_SelectedIndexChanged(object sender, EventArgs e)
		{
		if(ddl_subgroup.SelectedValue != "0")
			{
			tb_topic.Enabled		= true;
			bt_check.Enabled		= true;
			}
		}
	protected void bt_check_Click(object sender, EventArgs e)
		{
		populate_related();
		}
	private void fill_recent_tickets()
		{
		using(var uow = new UnitOfWork())
			{
			var curr_user				= (from y in new XPQuery<ne_xpo.cs.member>(uow)
													where y.member_id == (int) current_user.id
														select y).FirstOrDefault();
			var dt_issues							= (from y in new XPQuery<ne_xpo.cs.ticketissues>(uow)
														where 
															!y.ticketissues_ticketheader_id.ticketheader_private || 
															(
															y.ticketissues_ticketheader_id.ticketheader_private && 
																(
																y.ticketissues_ticketheader_id.ticketheader_createdby_member_id == curr_user ||
																y.ticketissues_ticketheader_id.ticketheader_member_assigned_id == curr_user || 
																y.ticketissues_ticketheader_id.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator == curr_user
																)
															)
														select y.ticketissues_ticketheader_id.ticketheader_id).Distinct();
			var where_clause			= " WHERE a.ticketheader_status_id NOT IN (5,7,10,11,12) ";
			if(ddl_group.SelectedValue != "0" && ddl_group.SelectedValue != "")
				{
				where_clause			+= " AND h.ticket_group_id = "+ddl_group.SelectedValue;
				}
			if(ddl_subgroup.SelectedValue != "0" && ddl_subgroup.SelectedValue != "")
				{
				where_clause			+= " AND b.ticketpage_id = "+ddl_subgroup.SelectedValue;
				}
			var allowed_list			= string.Join(",", dt_issues);
			where_clause			+= " AND a.ticketheader_id in ("+allowed_list+") ";
		gv_recent.DataSource		= Toolbox.doSQL_dt(string.Format(@" SELECT a.ticketheader_id id, a.ticketheader_id, a.ticketheader_issue RaisedIssue, a.ticketheader_issue issue, a.ticketheader_modified_date, a.ticketheader_created_date DateCreated, b.ticketpage_id pageid, b.ticketpage_name PageName, c.ticketstatus_status Status, d.tickettype_id typeid, d.tickettype_name TypeOfTicket, f.member_fullname AsignedTo, g.member_fullname RaisedBy, e.ticketpriority_id priority_id, e.ticketpriority_name Priority, h.ticket_group_id groupid, h.ticket_group_name groupname, a.ticket_header_expected_completion exp_fin, a.release_id, a.ticketheader_status_id status_id, if(a.ticketheader_issuetype = 3 AND a.ticketheader_status_id NOT IN (5,7,10,11,12),total_votes(a.ticketheader_id), -1) v FROM ticketheader AS a LEFT JOIN member AS f ON a.ticketheader_member_assigned_id = f.member_id LEFT JOIN ticketpage AS b ON a.ticketheader_module_id = b.ticketpage_id LEFT JOIN ticketstatus AS c ON a.ticketheader_status_id = c.ticketstatus_id LEFT JOIN tickettype AS d ON a.ticketheader_issuetype = d.tickettype_id LEFT JOIN ticketpriority AS e ON a.ticketheader_priority_id = e.ticketpriority_id LEFT JOIN member AS g ON a.ticketheader_createdby_member_id = g.member_id LEFT JOIN ticket_group h ON b.ticketpage_ticket_group_id = h.ticket_group_id
{0}  ORDER BY a.ticketheader_created_date DESC LIMIT 20",  where_clause ),null );

		gv_recent.DataBind();
			}
		}
	protected void gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if(e.VisibleIndex > -1)
			{
			var gv			= (ASPxGridView) sender;
			var dr				= gv.GetDataRow(e.VisibleIndex);
			if(dr != null)
				{
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
								e.Cell.Style["background-color"]	= "#fc0";
								b.Style["background-color"]			= "#fc0";
							break;
							case 5: // Minimum
								e.Cell.Style["background-color"]	= "#ff0";
								b.Style["background-color"]			= "#ff0";
							break;
							case 6: // Nice to have
								e.Cell.Style["background-color"]	= "#9c9";
								b.Style["background-color"]			= "#9c9";
							break;
							}
					break;
					}
				}
			}
		}
	protected void bt_startnew_Click(object sender, EventArgs e)
		{
		Session["force_start"]	= "1";
		show_ticket_fields();
		}
	protected void bt_cutticket_Click(object sender, EventArgs e)
		{
		// Make ticket header
		var ticket			= new NETickets();
		ticket.module_id			= Convert.ToInt32(ddl_subgroup.SelectedValue);
		ticket.issuetype			= Convert.ToInt16(ddl_type.SelectedValue);
		ticket.issue				= tb_topic.Text;
		ticket.createdby_member_id	= current_user.id;
		ticket.is_private			= chk_private.Checked;
		ticket.priority_id			= 1;
		ticket.save();

		
		var	issue	= new NETickets.ticketissue(); 

        issue.ticketheader_id			= ticket.id;
		issue.message					= ticket_body.Text;
		issue.created_member_id			= current_user.id;
		issue.created_date				= DateTime.Now;
		issue.save();

		if(ticket_file.PostedFile != null)
			{
			var file				= ticket_file.PostedFile;
			var buffer					= new byte[file.ContentLength];
			file.InputStream.Read(buffer, 0, file.ContentLength);
			var name						= System.IO.Path.GetFileName(file.FileName);
			var extension				= System.IO.Path.GetExtension(file.FileName).ToLower();
			    if (extension != "")
			        {
			        NETickets.ticketissue.add_attachment(issue.id, buffer, file.ContentLength, issue.id.ToString(), extension);

			        issue.uploaded_file = issue.id + extension;
			        issue.save();
			        }
			    }

		NETickets.send_new_ticket_alert(ticket.id, current_user, ticket.issue, issue.message);
		Response.Redirect("/mobile/index.aspx?a=new_ticket&do=after&ticket_id="+ticket.id, false);
		}
}