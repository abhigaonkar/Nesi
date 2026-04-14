using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using nesi.core;

public partial class sections_member_huddle_modules_task : System.Web.UI.UserControl
	{
	public NeMember current_user  { get; set; }
	public huddle h  { get; set; }
	public huddle.task ht  { get; set; }
	public string task  { get; set; }
	public int status_index  { get; set; }
	public string huddle_source  { get; set; }
	public string assigned_to  { get; set; }
	public string color  { get; set; }
	public int assigned_to_id  { get; set; }
	public int ticket_status_id  { get; set; }
	public string date_due  { get; set; }
	public string description  { get; set; }
	public string label_ticket_value  { get; set; }
	public string link  { get; set; }
	public int task_id  { get; set; }
	public bool show_delete  { get; set; }
	public bool is_active { get; set; }
	protected void Page_Init(object sender, EventArgs e)
		{
		task_cbp_handler.ClientInstanceName			= "task_cbp_handler_"+task_id;
		monitor.ClientInstanceName					= "monitor_"+task_id;
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		task_table.Style["border-color"]			= color;
		draghandle.Style["background-color"]		= color;
		task_table.Attributes["data-id"]			= task_id.ToString();
		task_table.Attributes["data-cpb_id"]		= "task_cbp_handler_"+task_id;
		task_table.Attributes["data-monitor_id"]	= "monitor_"+task_id;
		
		draghandle.Style["opacity"]					= status_index == 2 ? "0.5" : "1";
		link_box.InnerHtml							= link == "" ? task : string.Format("<a href='{0}' style='color:#000'>{1}</a>", link, task);
		if((int)current_user.id != h.captain && current_user.id != assigned_to_id)
			{
			status.ClientEnabled		= false;
			}
		var cbp_bag			= (Dictionary<int, bool>) Session["cbp_id_bag"];
		if(cbp_bag == null)
			{
			Session["cbp_id_bag"]	= new Dictionary<int, bool>();
			cbp_bag					= (Dictionary<int, bool>) Session["cbp_id_bag"];
			}
		if(cbp_bag.ContainsKey(task_id))
			{
			comment_div.Visible					= (bool) cbp_bag[task_id];
			comments_div.Visible				= (bool) cbp_bag[task_id];
			description_div.Visible				= (bool) cbp_bag[task_id];
			}
		load_comments();
		
		combo_copy.ClientVisible = (int)current_user.id == h.captain || current_user.id == assigned_to_id;
		if (combo_copy.ClientVisible)
		{
			combo_copy.DataSource = Toolbox.doSQL_dt(@"SELECT id, Concat(date_of_huddle,' - ',name) name FROM huddle WHERE date_of_huddle > @v0  AND active = TRUE", new object[] {  System.DateTime.Today.ToString("yyyy-MM-dd") } );
			combo_copy.DataBind();
		}

		if(!is_active)
			{
			status.Enabled						= false;
	//		icon_refresh.Visible				= false;
	//		icon_delete.Visible					= false;
			comment_box.Disabled				= true;
			button_new_comment.Enabled			= false;
			
			}
		}
	private void load_comments()
		{
		var _comments					= Toolbox.doSQL_dt(@"SELECT a.id,a.created_dt, a.comment, b.member_fullname name FROM huddle_task_comments a LEFT JOIN member b ON a.created_by = b.member_id WHERE a.huddle_task = @v0 ", new object[] {  task_id } );
		var sb					= new StringBuilder();
		foreach(DataRow dr in _comments.Rows)
			{
			var comment		= dr["comment"].ToString().Replace("\n", "<br/>\n");
			sb.AppendFormat("<div class='comment' data-id='{0}'><u><b>{2} at {3}</b></u><br/>{1}</div>", dr["id"], comment, dr["name"], Toolbox.MySQL_longdt((DateTime) dr["created_dt"]));
			}
		comments_div.InnerHtml				= sb.ToString();
		}
	protected void task_cbp_handler_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		if(e.Parameter.Contains("|"))
			{
			var paras			= e.Parameter.Split('|');
			switch(paras[0])
				{
				case "expand":
					var cbp_bag			= (Dictionary<int, bool>) Session["cbp_id_bag"];
					var is_v								= !cbp_bag.ContainsKey(task_id) ? false : (bool) cbp_bag[task_id];
					comment_div.Visible						= !is_v;
					comments_div.Visible					= !is_v;
					description_div.Visible					= !is_v;
					var deg									= is_v ? 0 : 180;
					icon_expand.Style["-ms-transform"]		= "rotate("+deg+"deg)";
					icon_expand.Style["-webkit-transform"]	= "rotate("+deg+"deg)";
					icon_expand.Style["transform"]			= "rotate("+deg+"deg)";
					cbp_bag[task_id]						= comment_div.Visible;
				break;
				case "refresh":
					load_comments();
				break;
				case "new_comment":
					var htc					= new huddle.task.comment();
					htc.huddle_task							= task_id;
					htc.created_by							= (int) current_user.id;
					htc.created_dt							= DateTime.Now;
					htc.text								= paras[1];
					htc.save();
					comment_box.InnerText					= "";
					load_comments();
				break;
				case "copytask":
					var other_huddle_id		= 0;
					int.TryParse(paras[1], out other_huddle_id);
					var ht			= new huddle.task(task_id);
					var new_ht		= new huddle.task();
					new_ht.assigned_to		= ht.assigned_to;
					new_ht.created_by		= ht.created_by;
					new_ht.date_due			= ht.date_due;
					new_ht.description		= ht.description;
					new_ht.huddle			= other_huddle_id;
					new_ht.huddle_source	= ht.huddle_source;
					new_ht.linetext			= ht.linetext;
					new_ht.link				= ht.link;
					new_ht.source_id		= ht.source_id;
					new_ht.status			= ht.status;
					new_ht.save();
					// Copy old chat
					Toolbox.doSQL_void(string.Format(@"INSERT INTO huddle_task_comments (huddle_task, created_by, created_dt, comment) SELECT '{0}', created_by, created_dt, comment FROM huddle_task_comments WHERE huddle_task = '{1}'", new_ht.id, ht.id));
					load_comments();
				break;
				}
			}
		}
	protected void monitor_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{

		}
	protected void ASPxLabel1_Init(object sender, EventArgs e)
	{

	}
}