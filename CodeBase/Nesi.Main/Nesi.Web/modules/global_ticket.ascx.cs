using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using DevExpress.Xpo;
using nesi.core;

public partial class modules_global_ticket : System.Web.UI.UserControl
	{
	public NeMember current_user  { get; set; }
	private int _page_id	= 0;
	protected void Page_Init(object sender, EventArgs e)
		{
		_page_id			= detect_current_page();
		}
	protected void Page_Load(object sender, EventArgs e) // 100-200ms
		{
		if (current_user != null && current_user.id != 1316)
			{
			if(!Page.IsPostBack && !Page.IsCallback)
				{
				// default the fields
				ddl_group.Value			= 1;
				ddl_type.Value			= 3;
				populate_ddls();
				ddl_pertaining.Value	= related_ticketpage_id((int) _page_id);
				if((int) ddl_pertaining.Value == 0)
					{
					// Ticket page doesn't exist, make one.
					if(_page_id != 0)
						{
						var p					= new NePage(_page_id);
						var tp		= new NETickets.ticketpage();
						tp.name						= p.Name;
						tp.group_id					= 1;
						tp.page_id					= _page_id;
						tp.save();
						populate_ddls();
						ddl_pertaining.Value		= related_ticketpage_id((int) _page_id);
						}
					if((int) ddl_pertaining.Value == 0)
						{
						// Shouldn't happen, if it does shoot Matt an email.
						Page.Master.FindControl("root_flyout").Visible		= false;
						this.Visible										= false;
						}
					}
				}
			if((Page.IsCallback || Page.IsPostBack) && ddl_pertaining.Value != null)
				{
				populate_related_tickets(related_page_id((int) ddl_pertaining.Value));
				}
			else
				{
				populate_related_tickets(_page_id);
				}
			}
		}
	private int detect_current_page()
		{
		using (var conn = Toolbox.connect())
			{
			var has_page_id = Request.RawUrl.Contains("page_id=");
			var page_id					= 0;
			if(!has_page_id)
				{
				// Get it from the page table
				var page_entries	= Toolbox.doSQL_dt(conn, @"SELECT page_id FROM page WHERE page_scriptpath LIKE CONCAT(@v0,'%')", new object[] {  Request.Url.LocalPath } );
				if(page_entries.Rows.Count >= 1)
					{
					page_id				= Convert.ToInt32(page_entries.Rows[0]["page_id"]);
					}
				else // Nothing exists, check page_related
					{
					var path				= Request.Url.LocalPath;
					var c_related			= Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM page_related WHERE path = @v0 AND related_page_id != 0", new object[] { path });
					if(c_related == 0)
						{
						c_related			= Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM page_related WHERE path = @v0", new object[] { path });
						if(c_related == 1) // Entry does exist, but doesn't have a page_id associated with it yet.
							{
							}
						else // Make a blank related page entry.
							{
							Toolbox.doSQL_void(conn, @"INSERT INTO page_related (path, related_page_id) VALUES (@v0, 0)", new object[] { path });
							}
						}
					else if(c_related == 1)
						{
						// Use the related page_id for this page.
						page_id			= Toolbox.doSQL_int(conn, @"SELECT related_page_id FROM page_related WHERE path = @v0", new object[] { path });	
						}
					}
				}
			else
				{
				page_id			= Convert.ToInt32(Request.QueryString["page_id"]);
				}
			return page_id;
			}
		}
	private int related_ticketpage_id(int _id)
		{
		using(var uow = new UnitOfWork())
			{
			var pertaining	= (from x in new XPQuery<ne_xpo.cs.ticketpage>(uow)
									where x.page_id.page_id == _page_id && x.ticketpage_ticket_group_id.ticket_group_id == (int) ddl_group.Value
										select new 
										{
										id = x.ticketpage_id
										}).SingleOrDefault();
			return pertaining == null ? 0 : pertaining.id;
			}
		}
	private int related_page_id(int _id)
		{
		using(var uow = new UnitOfWork())
			{
			var page	= (from x in new XPQuery<ne_xpo.cs.ticketpage>(uow)
									where x.ticketpage_id == _id
										select new 
										{
										id = x.page_id.page_id
										}).SingleOrDefault();
			return page == null || page.id == null ? 0 : page.id;
			}
		}
	private void populate_ddls()
		{
		using(var uow = new UnitOfWork())
			{
			ddl_group.DataSource		= (from x in new XPQuery<ne_xpo.cs.ticket_group>(uow)
										select new 
											{
											id = x.ticket_group_id,
											name = x.ticket_group_name
											}).ToList();
			ddl_group.DataBind();
			var types					= (from x in new XPQuery<ne_xpo.cs.ticket_group_type_link>(uow) 
											join y in new XPQuery<ne_xpo.cs.tickettype>(uow) on x.ticket_type_id.tickettype_id equals y.tickettype_id
											where x.ticket_group_id.ticket_group_id == (int) ddl_group.Value
										select new 
											{
											id = x.ticket_type_id.tickettype_id,
											name = y.tickettype_name
											}).ToList();
			ddl_type.DataSource			= types;
			ddl_type.DataBind();
			if(ddl_type.Items.FindByValue(ddl_type.Value) == null)
				{
				ddl_type.SelectedIndex	= -1;
				}
			var perts					= (from x in new XPQuery<ne_xpo.cs.ticketpage>(uow)
										where x.ticketpage_ticket_group_id.ticket_group_id == (int) ddl_group.Value 
										select new 
											{
											id = x.ticketpage_id,
											name = x.ticketpage_name
											}).ToList();
			ddl_pertaining.DataSource	= perts;
			ddl_pertaining.DataBind();
			if(ddl_pertaining.Items.FindByValue(ddl_pertaining.Value) == null)
				{
				ddl_pertaining.SelectedIndex	= -1;
				}
			}

		}
	private void populate_related_tickets(int page_id)
		{
		// Dont forget about private ticket viewability
		if(page_id > 0)
			{
			var dt		= Toolbox.doSQL_dt(@" SELECT a.ticketheader_id id, 1 initial_order, IFNULL(a.ticketheader_createdby_member_id, 0) created_by, IFNULL(a.ticketheader_member_assigned_id, 0) assigned_to, IFNULL(e.ticket_group_administrator, 0) admin_id, IFNULL(a.ticketheader_private, 0) is_private, CAST(IFNULL(GROUP_CONCAT(f.member_id), '') AS CHAR(1000)) watchers, a.ticketheader_issue name, c.ticketstatus_status status, c.ticketstatus_id status_id, a.ticketheader_created_date dt_created, a.ticketheader_issuetype type, if(a.ticketheader_issuetype = 3 AND a.ticketheader_status_id NOT IN (5,7,10,11,12),total_votes(a.ticketheader_id), -1) v, (SELECT COUNT(*) FROM ticket_vote WHERE ticketheader_id = a.ticketheader_id AND member_id = @v1 ) voted FROM ticketheader a LEFT JOIN ticketpage b on a.ticketheader_module_id = b.ticketpage_id LEFT JOIN ticketstatus c ON a.ticketheader_status_id = c.ticketstatus_id LEFT JOIN ticketpage d ON a.ticketheader_module_id = d.ticketpage_id LEFT JOIN ticket_group e ON d.ticketpage_ticket_group_id = e.ticket_group_id LEFT JOIN ticket_memberview f ON a.ticketheader_id = f.ticket_id WHERE b.page_id = @v0  AND a.ticketheader_private = 0 and a.ticketheader_status_id NOT IN (5,7) AND a.ticketheader_issue NOT LIKE '%milestone%' GROUP BY a.ticketheader_id UNION SELECT a.ticketheader_id id, 2 initial_order, IFNULL(a.ticketheader_createdby_member_id, 0) created_by, IFNULL(a.ticketheader_member_assigned_id, 0) assigned_to, IFNULL(e.ticket_group_administrator, 0) admin_id, IFNULL(a.ticketheader_private, 0) is_private, CAST(IFNULL(GROUP_CONCAT(f.member_id), '') AS CHAR(1000)) watchers, a.ticketheader_issue name, c.ticketstatus_status status, c.ticketstatus_id status_id, a.ticketheader_created_date dt_created, a.ticketheader_issuetype type, if(a.ticketheader_issuetype = 3 AND a.ticketheader_status_id NOT IN (5,7,10,11,12),total_votes(a.ticketheader_id), -1) v, (SELECT count(*) FROM ticket_vote WHERE ticketheader_id = a.ticketheader_id AND member_id = @v1 ) voted FROM ticketheader a LEFT JOIN ticketpage b on a.ticketheader_module_id = b.ticketpage_id LEFT JOIN ticketstatus c ON a.ticketheader_status_id = c.ticketstatus_id LEFT JOIN ticketpage d ON a.ticketheader_module_id = d.ticketpage_id LEFT JOIN ticket_group e ON d.ticketpage_ticket_group_id = e.ticket_group_id LEFT JOIN ticket_memberview f ON a.ticketheader_id = f.ticket_id WHERE b.page_id = @v0  AND a.ticketheader_status_id IN (5,7) AND a.ticketheader_issue NOT LIKE '%milestone%' GROUP BY a.ticketheader_id", new object[] {  page_id, current_user.id } );
			var temp_dt	= dt.Copy();
			temp_dt.Clear();
			foreach(DataRow dr in dt.Rows)
				{
				var is_private		= Convert.ToBoolean(dr["is_private"]);
				var do_add			= true;
				if(is_private)
					{
					var admin_id			= Convert.ToInt32(dr["admin_id"]);
					var assigned_to			= Convert.ToInt32(dr["assigned_to"]);
					var created_by			= Convert.ToInt32(dr["created_by"]);
					var watcher_list	= new List<int>();
					var watchers			= dr["watchers"].ToString();
					if(watchers != "")
						{
						watcher_list		= watchers.Split(',').Select(int.Parse).ToList();
						}
					do_add					= Toolbox.Contains(current_user.id, new int[]{created_by, assigned_to, admin_id}) || watcher_list.Contains(current_user.id);
					}
				if(do_add)
					{
					temp_dt.ImportRow(dr);
					}
				}
			temp_dt.DefaultView.Sort			= "initial_order ASC, dt_created DESC";
			gv_existing_tickets.DataSource		= temp_dt.DefaultView.ToTable();
			gv_existing_tickets.DataBind();
			}
		else
			{
			gv_existing_tickets.DataSource = null;
			gv_existing_tickets.DataBind();
			}
		}
	protected void gv_existing_tickets_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		if(e.VisibleIndex >= 0 && gv.Visible)
			{
			var dr				= gv.GetDataRow(e.VisibleIndex);
			if(dr != null)
				{
				var id					= (int) dr["id"];
				var status_id			= (int) dr["status_id"];
				var type				= dr["type"].ToString();

				if(e.DataColumn.Name.Equals("watch")&&dr!=null)
					{
					var created_by			=  Convert.ToInt32(dr["created_by"]);
					var assigned_to			=  Convert.ToInt32(dr["assigned_to"]);
					var admin_id			=  Convert.ToInt32(dr["admin_id"]);
					var watcher_list	= new List<int>();
					var watchers			= dr["watchers"].ToString();
					if(watchers != "")
						{
						watcher_list		= watchers.Split(',').Select(int.Parse).ToList();
						}
					var div		= (HtmlContainerControl) gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "icon_holder");
					if(Toolbox.Contains(current_user.id, new int[]{created_by, assigned_to, admin_id}) || Toolbox.Contains(status_id, new int[]{5,7,10,11,12}))
						{
						div.Visible							= false;
						}
					else
						{
						var is_watching					= watcher_list.Contains(current_user.id);
						div.Attributes["class"]				= is_watching
																? "iconw watching" 
																: "iconw notwatching";
						div.Style.Add("cursor", "pointer");
						div.Attributes["title"]				= is_watching
																? "Stop watching this ticket"
																: "Watch this ticket";
						div.Attributes["onclick"]			= is_watching
																? string.Format("gv_existing_tickets.PerformCallback('w|{0}|0');", id)
																: string.Format("gv_existing_tickets.PerformCallback('w|{0}|1');", id);
						}
					}
				else if(e.DataColumn.Name == "type")
					{
					var div		= (HtmlContainerControl) gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "icon_holder");
					if(div != null)
						{
						switch(type)
							{
							case "1":
								div.Attributes["class"]		= "icont bug";
								div.Attributes["title"]		= "Bug";
							break;
							case "2":
								div.Attributes["class"]		= "icont question";
								div.Attributes["title"]		= "Question";
							break;
							case "3":
								div.Attributes["class"]		= "icont rfc";
								div.Attributes["title"]		= "Request for Change";
							break;
							default:
								div.Attributes["class"]		= "icont empty";
								div.Attributes["title"]		= "";
							break;
							}
						}
					}
				else if(e.DataColumn.Name == "vote")
					{
					var div_wrapper		= (HtmlContainerControl) gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "icon_holder_wrapper");
					var v								= Convert.ToInt32(dr["v"]);
					var voted							= Convert.ToInt32(dr["voted"]);
					div_wrapper.Visible					= false;
					if(type == "3" && !Toolbox.Contains(status_id, new int[]{5,7,10,11,12}))
						{
						div_wrapper.Visible					= true;
						div_wrapper.InnerHtml				= string.Format("<b>{0}</b>", v);
						div_wrapper.Attributes["onclick"]	= string.Format("page_obj.global_ticket.vote({0},{1});", id, 1-voted);
						div_wrapper.Attributes["title"]		= voted == 1 
																? "Click to remove your vote from this ticket" 
																: "Click to add your vote to this ticket!\nThe higher the rank, the sooner it will be done.";
						if(voted == 1)
							{
							div_wrapper.Attributes["class"]		+= " voted";
							}
						}
					}
				}
			}
		}
	protected void gv_existing_tickets_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		if(e.Parameters.Contains("|"))
			{
			var paras			= e.Parameters.Split('|');
			var ticket_id			= 0;
			switch(paras[0])
				{
				case "w":
					int.TryParse(paras[1], out ticket_id);
					if(ticket_id > 0)
						{
						var should_watch			= paras[2] == "1";
						if(should_watch) // Add watch
							{
							var tmv			= new NETickets.ticket_memberview();
							tmv.ticket_id							= ticket_id;
							tmv.member_id							= (int) current_user.id;
							tmv.save();
							}
						else // Remove watch
							{
							var tmv			= new NETickets.ticket_memberview(ticket_id, current_user.id);
							tmv.delete();
							}
						}
				break;
				case "v":
					int.TryParse(paras[1], out ticket_id);
					if(ticket_id > 0)
						{
						var should_vote			= paras[2] == "1";
						if(should_vote) // Add watch
							{
							var tv				= new NETickets.ticket_vote();
							tv.ticketheader_id						= ticket_id;
							tv.member_id							= current_user.id;
							tv.save();
							}
						else // Remove vote
							{
							var tv				= new NETickets.ticket_vote(ticket_id, current_user.id);
							tv.delete();
							}
						}
				break;
				}
			}
		populate_related_tickets(related_page_id((int) ddl_pertaining.Value));
		}
	protected void cbp_global_ticket_Callback(object sender, CallbackEventArgsBase e)
		{
		switch(e.Parameter)
			{
			case "refresh":
				populate_ddls();
				if(!Toolbox.Contains(((int) ddl_group.Value), new int[]{1,30,31}))
					{
					ddl_pertaining.Value				= null;
					ddl_type.Value						= null;
					gv_existing_tickets.ClientVisible	= false;
					cbp_global_ticket.JSProperties["cpCollapse"]		= true;
					}
				else
					{
					gv_existing_tickets.ClientVisible	= true;
					if(ddl_pertaining.Value == null)
						{
						ddl_pertaining.Value			= related_ticketpage_id(_page_id);
						}
					populate_related_tickets(related_page_id((int) ddl_pertaining.Value));
					cbp_global_ticket.JSProperties["cpCollapse"]		= false;
					}
			break;
			case "save":
				cbp_global_ticket.JSProperties["cpCollapse"]		= (int) ddl_group.Value > 1;
				// Validation
				var has_blanks			= subject.Text.Trim() == "" || body.Text.Trim() == ""; // First level.. blank values
				if(has_blanks)
					{
					throw new Exception("Neither the subject not the body of the ticket can be blank");
					}
				// Check if another ticket has same subject
				var cExists			= Toolbox.doSQL_int(@"SELECT IFNULL(MAX(ticketheader_id),0) FROM ticketheader WHERE ticketheader_issue = @v0", subject.Text);
				if(cExists > 0)
					{
					throw new Exception(string.Format(@"This same subject exists on <a href=""javascript:boing('/sections/member/tickets/ticketpage.aspx?issue={0}', 'tickets{0}',1100,900);"">ticket #{0}</a>. You may want to add yourself as a viewer to that ticket, or if it is closed, have the ticket group administrator re-open it.", cExists));
					}
					var t = new NETickets
						{
						issue = subject.Text,
						createdby_member_id = current_user.id,
						issuetype = (int) ddl_type.Value,
						module_id = (int) ddl_pertaining.Value,
						is_private = is_private.Checked
						};
					t.save();
				var groupId			= (int) ddl_group.Value;
				if(t.id > 0)
					{
					var ti = new NETickets.ticketissue
						{
						ticketheader_id = t.id,
						message = body.Text == "" ? "(Message body not provided)" : body.Text,
						created_member_id = current_user.id
						};
					ti.save();
					var fileSaved = false;
					if(file_blob.Value.Length > 100)
						{
						var image = Convert.FromBase64String(file_blob.Value.Split(',')[1]);
						NETickets.ticketissue.add_attachment(ti.id, image, image.Length, ti.id.ToString(), "png");
						fileSaved = true;
						}
					if(fileSaved)
						{
						ti.uploaded_file = ti.id + ".png";
						ti.save();
						}
					NETickets.send_new_ticket_alert(t.id, current_user, t.issue, ti.message);
					subject.Text		= "";
					body.Text			= "";
					is_private.Checked	= false;
					if(groupId == 1 || groupId == 30 || groupId == 31)
						{
						populate_related_tickets(related_page_id((int) ddl_pertaining.Value));
						}
					subject.Focus();
					}
			break;
			}
		}
}