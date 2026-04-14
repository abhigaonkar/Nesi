using System;
using System.Data;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class sections_member_huddle_modules_detail_tasks : System.Web.UI.UserControl
	{
	public int huddle_id  { get; set; }
	private huddle h;
	private huddle.user hu;
	private huddle.task ht					= new huddle.task();
	Toolbox _tools;
	public NeMember current_user;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools		= new Toolbox();

		}
    protected override void OnInit(EventArgs e)
        {
        base.OnInit(e);
        this.DataBinding += new EventHandler(this_databind);
		}
	protected void this_databind(object sender, EventArgs e)
		{
		h									= new huddle(huddle_id);
		hu									= new huddle.user(huddle_id, (int) current_user.id);
		add_task_assigned_to.ClientEnabled		= (int) current_user.id == h.captain;
		add_task_source.DataSource			= fill_sources();
		add_task_source.DataBind();
		add_task_assigned_to.DataSource		= fill_assigned_to();
		add_task_assigned_to.DataBind();
		if(add_task_assigned_to.SelectedIndex == -1)
			{
			add_task_assigned_to.Value		= hu.id;
			}
		if(add_task_date_due.Date.Year < 2000)
			{
			add_task_date_due.Date			= Toolbox.GetNextWeekday(DateTime.Today.AddDays(1), DayOfWeek.Friday);
			}
		if(add_task_source.SelectedIndex == -1)
			{
			add_task_source.SelectedIndex	= 0;
			div_ticket.Visible				= true;
			add_task_ticket.DataSource		= fill_tickets((int) add_task_assigned_to.Value, true);
			add_task_ticket.DataBind();
			add_task_link.ClientEnabled		= false;
			}
		add_task_assigned_to.Enabled	= h.active;
		add_task_source.Enabled			= h.active;
		add_task_date_due.Enabled		= h.active;
		add_task_description.Enabled	= h.active;
		add_task_link.Enabled			= h.active;
		add_task_status.Enabled			= h.active;
		add_task_text.Enabled			= h.active;
		add_task_ticket.Enabled			= h.active;
		add_task_save.Enabled			= h.active;
		fill_tasks();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		}
	private void fill_tasks()
		{
		var dt					= Toolbox.doSQL_dt(@" SELECT a.id, a.source_id, IF(a.huddle_source = 1, CONCAT(d.ticketheader_issue, ' - (', f.ticketstatus_status, ')'), a.linetext) task, e.name type, e.id huddle_source, a.date_due, a.status, a.link, c.member_id assigned_to_id, IFNULL(d.ticketheader_status_id, 0) ticket_status, c.member_fullname assigned_to, a.description task_description, b.color FROM huddle_task a LEFT JOIN huddle_user b on a.assigned_to = b.id LEFT JOIN member c on b.member = c.member_id LEFT JOIN ticketheader d ON a.source_id = d.ticketheader_id LEFT JOIN huddle_source e ON a.huddle_source = e.id LEFT JOIN ticketstatus f ON d.ticketheader_status_id = f.ticketstatus_id WHERE a.huddle = @v0  ORDER BY c.member_id, a.order_n", new object[] {  huddle_id } );
		var sources								= fill_sources();
		var assigned_to							= fill_assigned_to();
		var ul = new HtmlGenericControl("ul");
 		ul.Attributes["class"]							= "task_li";
		foreach(DataRow dr in dt.Rows)
			{
				var li = new HtmlGenericControl("li");
			var c		= (sections_member_huddle_modules_task) LoadControl("./task.ascx");
			c.task_id									= (int) dr["id"];
			c.ID										= "TASK_"+c.task_id;
			c.is_active									= h.active;
			c.task										= (string) dr["task"];
			c.description								= (string) dr["task_description"];
			c.description								= c.description.Trim() == "" ? "<i>No description given</i>" : c.description;
			var link									= (string) dr["link"];
			c.link										= link == "" ? "" : string.Format(@"javascript:boing(""{0}"", ""huddle_{1}"", 1100, 750);", link, c.ID);
			c.status_index								= (int) dr["status"];
			c.huddle_source								= dr["type"].ToString();
			c.assigned_to								= dr["assigned_to"].ToString();
			c.assigned_to_id							= Convert.ToInt32(dr["assigned_to_id"]);
			c.show_delete								= h.captain == (int) current_user.id;
			c.h											= h;
			c.color										= dr["color"].ToString();
			c.ht										= ht;
			c.current_user								= current_user;
			var huddle_source_id						= (int) dr["huddle_source"];
			var source_id								= (int) dr["source_id"];
			c.date_due									= Toolbox.MySQL_shortdt((DateTime) dr["date_due"]);
			c.ticket_status_id							= Convert.ToInt32(dr["ticket_status"]);
			c.label_ticket_value						= huddle_source_id == 1 ? source_id+" - "+c.task : c.task;
			li.Controls.Add(c);
			ul.Controls.Add(li);
			}
		cbp_tasks.Controls.Add(ul);
		ds_assignedto.SelectCommand				= string.Format(@"
SELECT a.id, b.member_fullname name, b.member_lastname, b.member_nickname, c.business_unit_id FROM huddle_user a LEFT JOIN member b ON a.member = b.member_id LEFT join business_unit c ON b.business_unit_id = c.id WHERE a.huddle = {0} 
ORDER BY 
	member_lastname, member_nickname", huddle_id);
		}
	private DataTable fill_sources() 
		{
		return Toolbox.doSQL_dt(@"SELECT id, name, type FROM huddle_source ORDER BY id"  , null);
		}
	private DataTable fill_assigned_to()
		{
		return Toolbox.doSQL_dt(@" SELECT a.id, b.member_fullname name, b.member_lastname, b.member_nickname, c.business_unit_id FROM huddle_user a LEFT JOIN member b ON a.member = b.member_id LEFT join business_unit c ON b.business_unit_id = c.id WHERE a.huddle = @v0  ORDER BY member_lastname, member_nickname", new object[] {  huddle_id } );
		}
	private DataTable fill_tickets(int _assigned_id, bool exclude_referenced_tickets)
		{
		hu							= new huddle.user(_assigned_id);
		var referenced			= exclude_referenced_tickets ? string.Format("AND a.ticketheader_id NOT IN (SELECT source_id FROM huddle_task WHERE huddle = {0} AND assigned_to = {1})", huddle_id, _assigned_id) : "";
		return Toolbox.doSQL_dt(string.Format(@"
SELECT 
	a.ticketheader_id id, 
	a.ticketheader_issue name, 
	b.ticketpriority_name priority, 
	c.ticketstatus_status status,
	a.ticket_header_expected_completion committed
FROM 
	ticketheader a 
LEFT JOIN 
	ticketpriority b ON a.ticketheader_priority_id = b.ticketpriority_id
LEFT JOIN
	ticketstatus c ON a.ticketheader_status_id = c.ticketstatus_id
WHERE 
	a.ticketheader_status_id NOT IN (5,7,10,11,12) AND
	a.ticketheader_member_assigned_id = {0} {1}
ORDER BY 
	a.ticket_header_expected_completion DESC,
	b.ticketpriority_name ASC,
	a.ticketheader_id ASC
	", hu.member, referenced),null);
		}
	private void clear()
		{
		add_task_ticket.DataSource		= fill_tickets((int) add_task_assigned_to.Value, true);
		add_task_ticket.DataBind();
		add_task_ticket.SelectedIndex	= -1;
		add_task_link.Text				= "";
		add_task_status.SelectedIndex	= 0;
		add_task_text.Text				= "";
		add_task_description.Text		= "";
		div_text.Visible				= (int) add_task_source.Value == 2;
		div_ticket.Visible				= (int) add_task_source.Value == 1;
		add_task_link.ClientEnabled		= (int) add_task_source.Value == 2;
		add_task_date_due.Date			= Toolbox.GetNextWeekday(DateTime.Today.AddDays(1), DayOfWeek.Friday);
		}
	protected void cbp_addtask_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		if(add_task_source.Value != null)
			{
			switch(e.Parameter)
				{
				case "refresh":
				case "handle_ticket":
				case "handle_assigned_to":
				case "handle_source":
					switch((int) add_task_source.Value)
						{
						case 1:
							// Ticket
							div_ticket.Visible				= true;
							add_task_ticket.DataSource		= fill_tickets((int) add_task_assigned_to.Value, true);
							add_task_ticket.DataBind();
							add_task_link.ClientEnabled		= false;
							add_task_link.Attributes["onclick"]		= "if(add_task_link.GetText() != ''){boing(add_task_link.GetText(), 'ticket', 1100, 750);}";
							if(add_task_ticket.Value != null)
								{
								var hostname				= Request.Url.Port != 80 ? string.Format("{0}:{1}", Request.Url.Host, Request.Url.Port) : Request.Url.Host;
								add_task_link.Text			= "/sections/member/tickets/ticketpage.aspx?issue="+add_task_ticket.Value;
								}
							add_task_ticket.Focus();
						break;
						case 2:
							// Custom
							div_ticket.Visible				= false;
							div_text.Visible				= true;
							add_task_link.ClientEnabled		= true;
							add_task_link.Attributes["onclick"]		= "";
							add_task_text.Focus();
						break;
						}
					cbp_tasks.DataBind();
				break;
				case "save":
				//	try
				//		{
					var ht		= new huddle.task();
					ht.assigned_to		= (int) add_task_assigned_to.Value;
					ht.huddle			= huddle_id;
					ht.huddle_source	= (int) add_task_source.Value;
					if(ht.status == 0 && (int) add_task_status.Value == 1 && (int) add_task_source.Value == 1)
						{
						// Need to update the status of the ticket to in progress.
						}
					ht.status			= (int) add_task_status.Value;
					ht.description		= add_task_description.Text;
					ht.link				= add_task_link.Text;
					ht.created_by		= (int) current_user.id;
					ht.order_n			= Toolbox.doSQL_int(@"SELECT IFNULL(MAX(order_n), 0) + 1 FROM huddle_task WHERE huddle = @v0 AND assigned_to = @v1", new object[] {
					huddle_id, ht.assigned_to});
					switch((int) add_task_source.Value)
						{
						case 1:
							// Needs to have ticket & date due
							if(add_task_ticket.Value == null || (int) add_task_ticket.Value == 0)
								{
								throw new Exception("Please select a ticket");
								}
							else if(add_task_date_due.Date.Year < 2000)
								{
								throw new Exception("Please select a date due");
								}
							else
								{
								ht.source_id				= (int) add_task_ticket.Value;
								ht.link						= "/sections/member/tickets/ticketpage.aspx?issue="+ht.source_id;
								ht.date_due					= add_task_date_due.Date;
								var ticket			= new NETickets(ht.source_id);
								ticket.expected_completion	= ht.date_due;
								ticket.save();
								}
						break;
						case 2:
							// Needs to have text & date due
							if(add_task_text.Text.Trim() == "")
								{
								throw new Exception("Please provide some text for this task");
								}
							else if(add_task_date_due.Date.Year < 2000)
								{
								throw new Exception("Please select a date due");
								}
							else
								{
								ht.linetext			= add_task_text.Text;
								ht.date_due			= add_task_date_due.Date;
								}
						break;
						}
					ht.save();
					clear();
					add_task_ticket.Focus();
				//		}
				//catch(Exception ee)
				//		{
				//	_tools.debug_note(ee);
				//	}
				break;
				}
			}
		}
	protected void cbp_tasks_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		}
	protected void cb_task_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		if(e.Parameter.Contains("|"))
			{
			var paras			= e.Parameter.Split('|');
			var task_id				= 0;
			switch(paras[0])
				{
				case "delete":
					int.TryParse(paras[1], out task_id);
					if(task_id > 0)
						{
						if(ht.exists(task_id))
							{
							ht				= new huddle.task(task_id);
							ht.delete();
							fill_tasks();
							}
						else
							{
							throw new Exception("Task doesn't exist");
							}
						}
					else
						{
						throw new Exception("Invalid Task ID");
						}
				break;
				case "update_status":
					int.TryParse(paras[1], out task_id);
					if(task_id > 0)
						{
						if(ht.exists(task_id))
							{
							var status_id	= 0;
							int.TryParse(paras[2], out status_id);
							ht				= new huddle.task(task_id);
							ht.status		= status_id;
							ht.save();
							fill_tasks();
							}
						else
							{
							throw new Exception("Task doesn't exist");
							}
						}
					else
						{
						throw new Exception("Invalid Task ID");
						}
				break;
				case "update_order":
					var to_update = paras[1].Split(',');
					foreach(var s in to_update)
						{
						var pair	= s.Split(':');
						int.TryParse(pair[0], out task_id);
						var order_n		= 0;
						int.TryParse(pair[1], out order_n);
						ht				= new huddle.task(task_id);
						ht.order_n		= order_n;
						ht.save();
						}
					fill_tasks();
				break;
				}
			}
		}
}