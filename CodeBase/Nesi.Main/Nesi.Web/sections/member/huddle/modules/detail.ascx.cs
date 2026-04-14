using System;
using System.Data;
using DevExpress.Web;
using System.Text;
using nesi.core;

public partial class sections_member_huddle_modules_detail : System.Web.UI.UserControl
	{
	public NeMember current_user { get; set; }
	public int huddle_id  { get {return Convert.ToInt32(hid_id.Value);} set {hid_id.Value = value.ToString();} }
	private huddle h;
	public ASPxPageControl page_control { get { return pc; } }
	Toolbox _tools;
    public event EventHandler refresh_view_huddle;
    protected override void OnInit(EventArgs e)
        {
		_tools		= new Toolbox();
        base.OnInit(e);
        this.DataBinding += new EventHandler(this_databind);
		}
	protected void this_databind(object sender, EventArgs e)
		{
		if(huddle_id > 0)
			{
			h								= new huddle(huddle_id);
			name.Text						= h.name;
			description.Text				= h.description;
			captain.Value					= (int) h.captain;
			active.Checked					= h.active;
			dt_created.Text = h.dt_created.ToString();
			date_of_huddle.Date = h.date_of_huddle;
			gv_users_from.DataSource		= get_available_users_dt();
			gv_users_from.DataBind();
			gv_users_to.DataSource			= get_selected_users_dt();
			gv_users_to.DataBind();
			gv_users_from.Enabled			= h.active;
			gv_users_to.Enabled				= h.active;
			captain.DataSource				= get_captains_dt();
			captain.DataBind();
			uc_detail_tasks.huddle_id		= huddle_id;
			uc_detail_tasks.current_user	= current_user;
			//uc_detail_tasks.DataBind();
			if(((int) current_user.id != h.captain))
				{
				name.ClientEnabled			= false;
				description.ClientEnabled	= false;
				captain.ClientEnabled		= false;
				save_huddle.ClientEnabled	= false;
				active.ClientEnabled			= false;
				pc.TabPages[2].ClientEnabled	= false;
				}
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var x	= 2;
		
		}
	public DataTable get_captains_dt() // Switch to Linq/XPO
		{
		return Toolbox.doSQL_dt(@"select a.member_id id, CONCAT('(',b.name, ') - ', a.member_fullname) name from member a LEFT join business_unit b ON a.business_unit_id = b.id  WHERE a.member_status = 'Active' ORDER BY a.business_unit_id, a.member_lastname, a.member_nickname" , null);
		}
	public DataTable get_available_users_dt() // Switch to Linq/XPO
		{
		return Toolbox.doSQL_dt(@"select a.member_id id, CONCAT('(',b.name, ') - ', a.member_fullname) name from member a LEFT join business_unit b ON a.business_unit_id = b.id WHERE a.member_status = 'Active' AND a.member_id NOT IN (SELECT captain id FROM huddle WHERE id = @v0  UNION SELECT member id FROM huddle_user WHERE huddle = @v0 ) ORDER BY a.business_unit_id, a.member_lastname, a.member_nickname", new object[] {  huddle_id } );
		}
	public DataTable get_selected_users_dt() // Switch to Linq/XPO
		{
		return Toolbox.doSQL_dt(@"select a.member id, CONCAT('(',c.name, ') - ', b.member_fullname) name, a.color from huddle_user a LEFT JOIN member b ON a.member = b.member_id LEFT join business_unit c ON b.business_unit_id = c.id WHERE a.huddle = @v0  AND b.member_status = 'Active' ORDER BY b.business_unit_id, b.member_lastname, b.member_nickname", new object[] {  huddle_id } );
		}
	protected void cbp_people_Callback(object source, CallbackEventArgsBase e)
		{
		var paras			= e.Parameter.Split('|');
		var id				= 0;
		var member_id		= 0;
		var huddle_id		= 0;
		int.TryParse(hid_id.Value, out huddle_id);
		huddle t;
		huddle.user tm;
		NeMember m;
		var sb	= new StringBuilder();
		switch(paras[0])
			{
			/*
			case "cancel":
				if(tb_title.Text == "" && huddle_id > 0)
					{
					huddle.delete(huddle_id);
					}
				hid_id.Value					= "";
				pnl_new_huddle.ClientVisible	= false;
			break;
			case "edit":
				pnl_new_huddle.ClientVisible	= true;
				huddle_id						= Convert.ToInt32(paras[1]);
				t								= new huddle(huddle_id);
				tb_title.Text					= t.name;
				hid_id.Value					= huddle_id.ToString();
			break;
			*/
			case "color":
				t					= new huddle(huddle_id);
				id					= Convert.ToInt32(paras[1]);
				var color		= paras[2];
				tm					= new huddle.user(huddle_id, (int) id);
				tm.color			= color;
				tm.save();
				m					= new NeMember((int) id);
				m.color				= color;
				m.save();
			break;
			case "drag":
				t					= new huddle(huddle_id);
				id					= Convert.ToInt32(paras[1]);
				var leftToRight    = Convert.ToBoolean(paras[2]);
				tm					= new huddle.user(huddle_id, (int) id);
				m					= new NeMember((int) id);
				tm.color			= m.color;
				if(leftToRight)
					{
					tm.save();
					}
				else
					{
					if(tm.member == t.captain)
						{
						throw new Exception("You must first remove this users as the captain before removing them from the selected users");
						}
					else if(Toolbox.doSQL_int(@"SELECT COUNT(*) FROM huddle_task WHERE assigned_to = @v0", tm.id) > 0)
						{
						throw new Exception("You cannot remove a user that has tasks assigned.");
						}
					else
						{
						tm.delete();
						}
					}
			break;
			}
		gv_users_from.DataSource	= get_available_users_dt();
		gv_users_to.DataSource		= get_selected_users_dt();
		gv_users_from.DataBind();
		gv_users_to.DataBind();
		uc_detail_tasks.DataBind();
		}
	protected void cbp_detail_overview_Callback(object sender, CallbackEventArgsBase e)
		{
		var paras			= e.Parameter.Split('|');
		var id				= 0;
		var member_id		= 0;
		var huddle_id		= 0;
		int.TryParse(hid_id.Value, out huddle_id);
		huddle t;
		var tm		= new huddle.user();
		var sb	= new StringBuilder();
		switch(paras[0])
			{
			case "save":
				t					= new huddle(huddle_id);
				if(name.Text.Trim() == "")
					{
					sb.Append("<li>Please supply a name for this huddle.");
					}
				if(description.Text.Trim() == "")
					{
					sb.Append("<li>Please supply a description for this huddle.");
					}
				if((int) captain.Value == 0)
					{
					sb.Append("<li>Please supply a captain for this huddle.");
					}
				if(sb.Length > 0)
					{
					lb_result.ForeColor	= System.Drawing.Color.Red;
					lb_result.Text		= "<ul>"+sb+"</ul>";
					return;
					}
				else
					{
					var incoming_captain	= Convert.ToInt32(captain.Value);
					var existing_captain	= t.captain;
					// If you are replacing the captain, add the existing captain as a huddle_user
					if(existing_captain != 0 && incoming_captain != existing_captain && !tm.exists(huddle_id, incoming_captain))
						{
						// Add captain to huddle_users
						tm					= new huddle.user();
						tm.huddle			= huddle_id;
						tm.member			= incoming_captain;
						tm.save();
						}
					t.name				= name.Text;
					t.captain			= incoming_captain;
					t.description		= description.Text;
					t.active			= active.Checked;
					t.date_of_huddle = date_of_huddle.Date;
					try
						{
						t.save();
						lb_result.ForeColor		= System.Drawing.Color.DarkGreen;
						lb_result.Text			= "Last Saved @ "+Toolbox.MySQLNow_long();
						gv_users_from.DataBind();
						gv_users_to.DataBind();
						uc_detail_tasks.DataBind();
						}
					catch (Exception ee)
						{
						_tools.catch_error(ee);
						lb_result.ForeColor		= System.Drawing.Color.Red;
						lb_result.Text			= "Error saving huddle @ "+Toolbox.MySQLNow_long();
						}
					}
			break;
			}
		}
	
}