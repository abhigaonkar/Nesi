using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using DevExpress.Web;
using System.Data;
using DevExpress.Xpo;
using nesi.core;

public partial class sections_member_huddle_index : Page
	{
	NeMember current_user;
	private const int _page_id			= 181; // from Page table in DB
	Toolbox _tools;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools						= new Toolbox();
		current_user				= Toolbox.do_handle_authentication(_page_id);
	//	uc_new.current_user			= current_user;
		load_huddle_selector();
		if(!IsCallback && !IsPostBack)
			{
				hdnid.Value = "0";
			Session["cbp_id_bag"]	= new Dictionary<int, bool>();
			}
		//gv_users_from.DataSource	= get_available_users_dt();
		//gv_users_to.DataSource		= get_selected_users_dt();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var menu							= new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml					= menu.MenuHTML;

	    if(!IsPostBack && !IsCallback)
			{
				view_huddles.DataBind();

                   var currentHuddle = find_current_huddle();
                   if (currentHuddle != 0)
                    {
                        view_huddles.SelectedIndex = view_huddles.Items.FindByValue(currentHuddle).Index;
                    }


			    load_main_overview();
				load_teammates();
				Session["huddle_source_list"] = null;
				load_source_tabs();
				ASPxTabControl1.ActiveTabIndex = 1;
				hdn_pop_task.Clear();
				hdn_pop_task.Add("key", "-1");
			
			}
		
		set_source_list();
        //gv_users_from.DataBind();
        //gv_users_to.DataBind();
		}


	private int find_current_huddle()
	{


		using (var uow = new UnitOfWork())
		{
			var m = uow.GetObjectByKey<ne_xpo.cs.member>((int)current_user.id);
			var huds = (from x in new XPQuery<ne_xpo.cs.huddle>(uow)
						join y in new XPQuery<ne_xpo.cs.huddle_user>(uow) on x.id equals y.huddle.id
						where
							(x.created_by == m || x.captain == m || y.member == m) && x.name != "" && x.name != null && x.date_of_huddle.Date<=DateTime.Today.Date

						select new
						{
							id = x.id,
							date = x.date_of_huddle
						}
						

										).Distinct().OrderByDescending(x => x.date).ToList();
			if (huds.Count > 0)
			{
				return huds[0].id;
			}



			return 0;

		}
	}

	private void load_source_tabs()
	{
		var dt = _tools.getSQL_datatable(@"Select id, name from huddle_source  where type = 1" , null);
		foreach (DataRow dr in dt.Rows)
		{
			var tp = new Tab(dr[1].ToString(), dr[0].ToString());
			ASPxTabControl1.Tabs.Add(tp);
		}
	}

	private void load_teammates()
	{
		if (view_huddles.SelectedIndex >= 0)
		{
			pc.Tabs.Clear();
			cp_left_panel.ClientVisible = true;
			var dt = _tools.getSQL_datatable(@"Select b.member_id,b.member_fullname,a.member from huddle_user a inner join member b on b.member_id = a.member  where a.huddle =@v0", new object[] { view_huddles.Value });
			ddl_custom_member.DataSource = dt;
			ddl_custom_member.DataBind();
			foreach (DataRow dr in dt.Rows)
			{

				var tp = new Tab(new NeMember(Convert.ToInt32(dr["member"])).FullName, dr["member"].ToString());
				tp.Name = dr["member_id"].ToString();
				tp.Text = dr["member_fullname"].ToString();
				pc.Tabs.Add(tp);
//				ListEditItem li = new ListEditItem();
//				li.Text = dr["member_fullname"].ToString();
//				li.Value = Convert.ToInt32(dr["member_id"]);
				tb_users.Tokens.Add(dr["member_fullname"].ToString());
			}
		
		
		}
		else
		{
			pc.Tabs.Clear();
		}
		
	}

	private void fix_order_n()
	{
		for (var x = 0; x < gv.VisibleRowCount; x++)
		{
			if (Convert.ToInt32(gv.GetRowValues(x, "order_n")) != x)
			{
				UpdateSortIndex(Convert.ToInt32(gv.GetRowValues(x, "id")), x);
			}
		}
	}

	private void load_huddle_selector()
		{
		//SELECT a.id, a.name FROM huddle a LEFT JOIN huddle_user b ON b.huddle_id = a.id WHERE a.created_by = @member OR a.captain = @member OR b.member = @member GROUP BY a.id
		using(var uow = new UnitOfWork())
			{
			var m	= uow.GetObjectByKey<ne_xpo.cs.member>((int) current_user.id);
			var huds			= (from x in new XPQuery<ne_xpo.cs.huddle>(uow) 
									join y in new XPQuery<ne_xpo.cs.huddle_user>(uow) on x.id equals y.huddle.id
									where 
										(x.created_by == m || x.captain == m || y.member == m) && x.name != "" && x.name != null
					
									 select new 
										{
										id = x.id,
										name = (x.date_of_huddle.Year + "-" + x.date_of_huddle.Month.ToString().PadLeft(2, '0') + "-" + x.date_of_huddle.Day.ToString().PadLeft(2, '0') + " " + x.date_of_huddle.Hour.ToString().PadLeft(2, '0') + ":" + x.date_of_huddle.Minute.ToString().PadLeft(2, '0') + " - " + x.name),
							date = x.date_of_huddle
						   }
						
										).Distinct().OrderByDescending(x => x.date).ToList();
			view_huddles.DataSource	= huds;
			view_huddles.TextField	= "name";
			view_huddles.ValueField	= "id";
			view_huddles.DataBind();
			}

		}

    private void load_main_overview()
		{
/*		Control obj_main_overview										= LoadControl("modules/main_overview.ascx");
		obj_main_overview.ID											= "uc_main_overview";
		cbp_body.Controls.Add(obj_main_overview);
		sections_member_huddle_modules_main_overview uc_main_overview	= (sections_member_huddle_modules_main_overview) obj_main_overview;
		uc_main_overview.current_user									= current_user;
		view_huddles.SelectedIndex										= -1;
 */
		}

    /*
	public DataTable get_available_users_dt()
		{
		return Toolbox.doSQL_dt(@"select a.member_id id, CONCAT('(',b.name, ') - ', a.member_fullname) name from member a LEFT join business_unit b ON a.business_unit_id = b.id WHERE a.member_status = 'Active' AND a.member_id NOT IN (SELECT member_id FROM huddle_user WHERE huddle_id = @v0 ) ORDER BY a.business_unit_id, a.member_lastname, a.member_nickname", new object[] {  hid_id.Value } );
		}
	public DataTable get_selected_users_dt()
		{
		return Toolbox.doSQL_dt(@"select a.member_id id, CONCAT('(',c.name, ') - ', b.member_fullname) name from huddle_user a LEFT JOIN member b ON a.member_id = b.member_id LEFT join business_unit c ON b.business_unit_id = c.id WHERE a.huddle_id = @v0  AND b.member_status = 'Active' ORDER BY b.business_unit_id, b.member_lastname, b.member_nickname", new object[] {  hid_id.Value } );
		}
    protected void cb_pnl_Callback(object sender, CallbackEventArgsBase e)
		{
		var paras			= e.Parameter.Split('|');
		int id				= 0;
		int member_id		= 0;
		int huddle_id		= 0;
		int.TryParse(hid_id.Value, out huddle_id);
		huddle t;
		huddle.user tm;
		StringBuilder sb	= new StringBuilder();
		switch(paras[0])
			{
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
			case "new":
				pnl_new_huddle.ClientVisible	= true;
				tb_title.Text		= "";
				t					= new huddle();
				t.created_by		= (int) current_user.id;
				t.save();
				int hud_id			= t.id;
				hid_id.Value		= hud_id.ToString();
			break;
			case "drag":
				id					= Convert.ToInt32(paras[1]);
				bool leftToRight    = Convert.ToBoolean(paras[2]);
				tm					= new huddle.user(huddle_id, (int) id);
				if(leftToRight)
					{
					tm.save();
					}
				else
					{
					tm.delete();
					}
				pnl_new_huddle.ClientVisible	= true;
			break;
			case "save":
				pnl_new_huddle.ClientVisible	= true;
				t					= new huddle(huddle_id);
				if(tb_title.Text.Trim() == "")
					{
					sb.Append("Please supply a name for this huddle!");
					}
				if(sb.Length > 0)
					{
					tb_title.Focus();
					lb_result.ForeColor	= System.Drawing.Color.Red;
					lb_result.Text		= sb.ToString();
					return;
					}
				else
					{
					t.name				= tb_title.Text;
					try
						{
						t.save();
						lb_result.ForeColor		= System.Drawing.Color.DarkGreen;
						lb_result.Text			= "Saved";
						}
					catch (Exception ee)
						{
						_tools.catch_error(ee);
						lb_result.ForeColor		= System.Drawing.Color.Red;
						lb_result.Text			= "Error saving huddle";
						}
					}
			break;
			}
		gv_users_from.DataSource	= get_available_users_dt();
		gv_users_to.DataSource		= get_selected_users_dt();
		gv_users_from.DataBind();
		gv_users_to.DataBind();
		gv_past_huddles.DataBind();
		}
		 */
	protected void cbp_body_Callback(object sender, CallbackEventArgsBase e)
		{
/*		int huddle_id			= view_huddles.Value == null ? 0 : Convert.ToInt32(view_huddles.Value);
		string action			= e.Parameter;
		switch(action)
			{
			case "overview":
				load_main_overview();
			break;
			case "load":
				load_detail(huddle_id);
			break;
			}
 */
		}
	protected void view_huddles_Callback(object sender, CallbackEventArgsBase e)
		{
		load_huddle_selector();
		var x			= (ASPxComboBox) sender;
		if(e.Parameter != "0")
			{
			x.Value					= Convert.ToInt32(e.Parameter);
			}
		}

	private void UpdateSortIndex(int rowKey, int sortIndex)
	{
		SqlDataSource1.UpdateParameters["id"].DefaultValue = rowKey.ToString();
		SqlDataSource1.UpdateParameters["order_n"].DefaultValue = sortIndex.ToString();
		SqlDataSource1.Update();
	}

	int GetGridViewKeyByVisibleIndex(ASPxGridView gridView, int visibleIndex)
	{
		return (int)gridView.GetRowValues(visibleIndex, gridView.KeyFieldName);
	}

	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gridView = sender as ASPxGridView;
		if (e.Parameters != null)
		{
			var parameters = e.Parameters.Split('|');
			if ((parameters[0] == "DRAGROW") && parameters[1] != null)
			{
				var draggingIndex = int.Parse(parameters[1]);
				var targetIndex = int.Parse(parameters[2]);
				var draggingRowKey = Convert.ToInt32(gv.GetRowValues(draggingIndex, "id"));
				var targetRowKey = Convert.ToInt32(gv.GetRowValues(targetIndex, "id"));

				var draggingDirection = (targetIndex < draggingIndex) ? 1 : -1;
				for (var rowIndex = 0; rowIndex < gridView.VisibleRowCount; rowIndex++)
				{
					var rowKey = GetGridViewKeyByVisibleIndex(gridView, rowIndex);  // get the id of the row.
					var index = (int)gridView.GetRowValuesByKeyValue(rowKey, "order_n");  // get the order number of the row.
					if ((index > Math.Min(targetIndex, draggingIndex)) && (index < Math.Max(targetIndex, draggingIndex)))
					{
						UpdateSortIndex(rowKey, index + draggingDirection);
					}
				}
				UpdateSortIndex(draggingRowKey, targetIndex);
				UpdateSortIndex(targetRowKey, targetIndex + draggingDirection);
			}
			else if (parameters[0] == "ADD_SOURCE")
			{
				var draggingIndex = int.Parse(parameters[1]);
				var mid = Convert.ToInt32(pc.ActiveTab.Name);
				var source_id = Convert.ToInt32(ASPxTabControl1.ActiveTab.Name);
				var h = new huddle(Convert.ToInt32(view_huddles.Value));
				var ht = new huddle.task();

				ht.huddle_source = Convert.ToInt32(gv_source.GetRowValuesByKeyValue(draggingIndex, "source"));
				ht.assigned_to = _tools.getSQL_int(@"Select ifnull((Select id from huddle_user  where member =@v0 and huddle =@v1  limit 1),0) ", new object[] { mid,h.id });
				ht.huddle = h.id;
				ht.linetext = gv_source.GetRowValuesByKeyValue(draggingIndex, "name").ToString();
				ht.link = gv_source.GetRowValuesByKeyValue(draggingIndex, "link").ToString();
				ht.source_id = draggingIndex;
				ht.status = 1;
				if (source_id == 3)
				{
					ht.description = new huddle.task(Convert.ToInt32(gv_source.GetRowValuesByKeyValue(draggingIndex, "id"))).description;
					ht.linetext = gv_source.GetRowValuesByKeyValue(draggingIndex, "name2").ToString();
					ht.source_id = new huddle.task(Convert.ToInt32(gv_source.GetRowValuesByKeyValue(draggingIndex, "id"))).source_id;
				}
				else
				{
					ht.description = "";
				}
				ht.date_due = dte.Date.AddDays(7);
				ht.created_by = Convert.ToInt32(current_user.id);
				ht.order_n = gv.VisibleRowCount;
				ht.save();
			}
			else if (parameters[0] == "DELETE")
			{
				var key = gv.GetRowValues(Convert.ToInt32(parameters[1]), "id").ToString();
				var h = new huddle.task(Convert.ToInt32(key));
				h.delete();
			}
			gv.DataBind();
		}
	}
	protected void gv_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.RowType == GridViewRowType.Data)
		{
			var rowOrder = e.GetValue("order_n");
			if (rowOrder != null)
				e.Row.Attributes.Add("sortOrder", rowOrder.ToString());
		}
	}

	
	protected void pc_ActiveTabChanged(object source, TabControlEventArgs e)
	{
			SqlDataSource1.DataBind();
			SqlDataSource1.SelectParameters["@mid"].DefaultValue = e.Tab.Name;
			gv.DataBind();
			fix_order_n();
			gv.ClientVisible = true;

			ScriptManager.RegisterStartupScript(this, GetType(), "Refresh1", "callback();", true);
	
	}
	protected void view_huddles_SelectedIndexChanged(object sender, EventArgs e)
	{
		if(view_huddles.SelectedIndex>=0)
		{
			cp_left_panel.ClientVisible = true;
			tb_users.Tokens.Clear();
			load_teammates();
			pc.DataBind();
			pc.ActiveTabIndex = pc.Tabs.FindByName(current_user.id.ToString()) != null ? pc.Tabs.FindByName(current_user.id.ToString()).Index : 0;

			SqlDataSource1.DataBind();
			SqlDataSource1.SelectParameters["@mid"].DefaultValue = current_user.id.ToString();
			gv.DataBind();
			fix_order_n();


		if (pc.ActiveTabIndex >= 0)
		{
			gv.ClientVisible = true;
			SqlDataSource1.DataBind();
			SqlDataSource1.SelectParameters["@mid"].DefaultValue = pc.ActiveTab.Name;
			gv.DataBind();
			fix_order_n();
		}

		lbltitle.Text = "Agenda for " + view_huddles.Text;
		var h = new huddle(Convert.ToInt32(view_huddles.Value));
		name.Text = h.name;
		description.Text = h.description;
		captain.Value = Convert.ToInt32(h.captain);
		active.Checked = h.active;
		dte.Text = h.date_of_huddle.ToString("yyyy-MM-dd HH:mm");
		createddate.Text = "Created: " + h.dt_created.ToString("yyyy-MM-dd HH:mm");
		delete_huddle.ClientVisible = (h.captain == Convert.ToInt32(current_user.id));
		set_source_list();
		}
		
	}
	protected void save_huddle_Click(object sender, EventArgs e)
	{
		huddle h;
		if (view_huddles.SelectedIndex >= 0)
		{
			h = new huddle(Convert.ToInt32(view_huddles.Value));
			h.name = name.Text;
			h.description = description.Text;
			h.captain = Convert.ToInt32(captain.Value);
			h.active = active.Checked;
			h.date_of_huddle = Convert.ToDateTime(dte.Text);
			h.created_by = (int)current_user.id;
			h.save();
			var i = tb_users.Value.ToString().Split(',');

			foreach (var user in i)
			{
				var tm = new huddle.user(h.id, (int)Convert.ToInt32(user));
				tm.save();
			}

			var dt = _tools.getSQL_datatable(@"Select b.member_id, a.id from huddle_user a inner join member b on b.member_id = a.member  where a.huddle =@v0", new object[] { view_huddles.Value });
			foreach (DataRow dr in dt.Rows)
			{
				if (!i.Contains(dr[0].ToString()))
				{
					//DataTable dt1 = _tools.getSQL_datatable(@"Select a.id from huddle_task a  where a.huddle =@v0 and a.assigned_to =@v1 ", new object[] { view_huddles.Value,dr[1] });
					//foreach (DataRow dr1 in dt1.Rows)
					//{
						var ht = new huddle.task(Convert.ToInt32(dr[0]));
						ht.delete();
					//}
					var tm = new huddle.user(h.id, (int)Convert.ToInt32(dr[0]));
					tm.delete();
				}
			}
			ScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "alert('Huddle Details Saved');", true);

		}
		else
		{
			h = new huddle();
			h.name = name.Text;
			h.description = description.Text;
			h.captain = Convert.ToInt32(captain.Value);
			h.active = true;
			h.date_of_huddle = Convert.ToDateTime(dte.Text);
			h.created_by = (int)current_user.id;
			h.save();

			var i = tb_users.Value.ToString().Split(',');

			foreach (var user in i)
			{
				var tm = new huddle.user(h.id, (int)Convert.ToInt32(user));
				tm.save();
			}
			

			ScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "alert('New Huddle Created');", true);
			
			load_huddle_selector();
			view_huddles.Value = h.id;
			lbltitle.Text = "Agenda for " + view_huddles.Text;
			hdnid.Value = h.id.ToString();
			
		}
	
		load_teammates();
		pc.DataBind();
		pc.ActiveTabIndex = pc.Tabs.FindByName(current_user.id.ToString()) != null ? pc.Tabs.FindByName(current_user.id.ToString()).Index : 0;
		if (pc.ActiveTabIndex >= 0)
		{
			gv.ClientVisible = true;
			SqlDataSource1.DataBind();
			SqlDataSource1.SelectParameters["@mid"].DefaultValue = pc.ActiveTab.Name;
			gv.DataBind();
		}
		dte.Text = h.date_of_huddle.ToString("yyyy-MM-dd HH:mm");
		

	}

	protected void set_source_list()
	{
		
		if (Session["huddle_source_list"] == null)
		{
			gv_source.FilterExpression = "";
			gv_source.Columns["Date"].Visible = false;
			gv_source.Columns["Group"].Visible = false;
			switch (ASPxTabControl1.ActiveTab.Text)
			{
				#region last huddle only
				case "Previous Huddle": Session["huddle_source_list"] = _tools.getSQL_datatable(@"SELECT a.id id, a.id visible_id, concat(c.member_nickname,' - ',IF(a.huddle_source = 1, CONCAT(d.ticketheader_id,'-',d.ticketheader_issue, ' - (', f.ticketstatus_status, ')'), a.linetext)) name, concat(IF(a.huddle_source = 1, CONCAT(d.ticketheader_id,'-',d.ticketheader_issue, ' - (', f.ticketstatus_status, ')'), a.linetext)) name2, if(a.status=0,'Not Started',if(a.status=1,'Started',if(a.status=3,'Completed','Pushed'))) status, a.link link, date(huddle.date_of_huddle) date, a.huddle_source source FROM huddle_task a inner join huddle on a.huddle = huddle.id LEFT JOIN huddle_user b on a.assigned_to = b.id LEFT JOIN member c on b.member = c.member_id LEFT JOIN ticketheader d ON a.source_id = d.ticketheader_id LEFT JOIN huddle_source e ON a.huddle_source = e.id LEFT JOIN ticketstatus f ON d.ticketheader_status_id = f.ticketstatus_id  WHERE (b.member =@v0 or huddle.captain=@v1  ) and huddle.id = ifnull((SELECT huddle.id FROM huddle INNER JOIN huddle_user ON huddle.id = huddle_user.huddle WHERE huddle_user.member =@v2  AND date(huddle.date_of_huddle) <@v3  ORDER BY huddle.date_of_huddle DESC LIMIT 1),0) ORDER BY huddle.date_of_huddle desc, c.member_id,a.order_n", new object[] { current_user.id,current_user.id,current_user.id,new huddle(Convert.ToInt32(view_huddles.Value)).date_of_huddle.Date.ToString("yyyy-MM-dd") });
					gv_source.Columns["Date"].Visible = true;
					gv_source.FilterExpression = "[status] == 'Started' or [status] == 'Not Started' or [status] == 'Pushed'  ";
					
					break;
				#endregion

				#region all huddles
				case "All Huddles": Session["huddle_source_list"] = _tools.getSQL_datatable(@"SELECT a.id id, a.id visible_id, concat(c.member_nickname,' - ',IF(a.huddle_source = 1, CONCAT(d.ticketheader_id,'-',d.ticketheader_issue, ' - (', f.ticketstatus_status, ')'), a.linetext)) name, concat(IF(a.huddle_source = 1, CONCAT(d.ticketheader_id,'-',d.ticketheader_issue, ' - (', f.ticketstatus_status, ')'), a.linetext)) name2, if(a.status=0,'Not Started',if(a.status=1,'Started',if(a.status=3,'Completed','Pushed'))) status, a.link link, date(huddle.date_of_huddle) date, a.huddle_source source FROM huddle_task a inner join huddle on a.huddle = huddle.id LEFT JOIN huddle_user b on a.assigned_to = b.id LEFT JOIN member c on b.member = c.member_id LEFT JOIN ticketheader d ON a.source_id = d.ticketheader_id LEFT JOIN huddle_source e ON a.huddle_source = e.id LEFT JOIN ticketstatus f ON d.ticketheader_status_id = f.ticketstatus_id  WHERE (b.member =@v0 or huddle.captain=@v1  ) ORDER BY huddle.date_of_huddle desc, c.member_id,a.order_n", new object[] { current_user.id,current_user.id });
						gv_source.Columns["Date"].Visible = true;
						break;
                #endregion

					#region tickets
					case "Tickets": Session["huddle_source_list"] = _tools.getSQL_datatable(@"SELECT ticketheader.ticketheader_id id, ticketheader.ticketheader_id visible_id, concat(ifnull(c.member_nickname,'Not Assigned'),' - ',ticketheader.ticketheader_id,'-',ticketheader.ticketheader_issue, ' - (', f.ticketstatus_status, ')') name, f.ticketstatus_status status, Concat('/sections/member/tickets/ticketpage.aspx?issue=',ticketheader.ticketheader_id) link, a.ticket_group_name `group`, 1 source, '' name2 FROM ticketheader LEFT JOIN ticketstatus f on ticketheader.ticketheader_status_id = f.ticketstatus_id inner join ticketpage b on ticketheader.ticketheader_module_id = b.ticketpage_id inner join ticket_group a on b.ticketpage_ticket_group_id = a.ticket_group_id left join member c on ticketheader.ticketheader_member_assigned_id = c.member_id  WHERE f.ticketstatus_id !=5 ORDER BY ticketheader.ticketheader_id desc" , null);
						gv_source.Columns["Group"].Visible = true;
						break;
					#endregion

					#region Employees
					case "Employees": Session["huddle_source_list"] = _tools.getSQL_datatable(@"SELECT member.member_id id, member.member_id visible_id, concat(c.name, ' - ', member_fullname) name, member_status status, '' link, '' `group`, 6 source, '' name2 FROM member LEFT join business_unit c on business_unit_id = c.id ORDER BY member_status,c.name,member_fullname", null);
						
						break;
					#endregion

					#region Quotes boing("/sections/member/quote/index.aspx?a=g&quote_id=" + id + "&revision=" + rev, "quote", 1035, 800);
					case "Quotes": Session["huddle_source_list"] = _tools.getSQL_datatable(@" SELECT `a`.`quote_id` id, `a`.`quote_id` visible_id, Concat(`a`.`quote_id`,'-',`f`.`ddl_name`,' - ', `d`.`customer_name`, ' - ' ,`a`.`job_description`, ' - ', `a`.`quoted_price`) name, Concat('/sections/member/quote/index.aspx?a=g&quote_id=',`a`.`quote_id`,'&revision=',`a`.`revision`) link, `e`.`status` AS status, 5 source, '' name2 FROM ( ( ( ( ( ( ( ( ( ( ( ( `quote_master` `a` LEFT JOIN `contact` `c` ON ( ( `a`.`Contact_ID` = `c`.`Contact_ID` ) ) ) LEFT JOIN `customer` `d` ON ( ( `a`.`customer_id` = `d`.`customer_id` ) ) ) LEFT JOIN `quote_status` `e` ON ((`a`.`status_id` = `e`.`id`)) ) LEFT JOIN `business_unit` `f` ON ( ( `a`.`business_unit_id` = `f`.`ID` ) ) ) LEFT JOIN `member` `h` ON ( ( `a`.`quoted_by` = `h`.`Member_ID` ) ) ) LEFT JOIN `quote_cust_notes` `i` ON ( ( `i`.`quote_cust_notes_quoteid` = `a`.`quote_id` ) ) ) ) LEFT JOIN `customer_sales_properties` `k` ON ( ( `a`.`address_id` = `k`.`address_id` ) ) ) LEFT JOIN `member` `kk` ON ( ( `k`.`account_manager` = `kk`.`Member_ID` ) ) ) LEFT JOIN `address` `m` ON ( ( `a`.`address_id` = `m`.`Address_ID` ) ) ) LEFT JOIN `member` `ram` ON ( ( `k`.`ram_member_id` = `ram`.`Member_ID` ) ) ) LEFT JOIN `woprog` `wo` ON ( ( ( `wo`.`woprog_bvwo` = lpad(`a`.`wo`, 10, 0) ) AND ( `wo`.`business_unit_id` = `a`.`business_unit_id` ) ) ) )  WHERE ((`a`.`open_date`>curdate()-interval 2 year) and (`a`.`active_revision` = 1) AND (`a`.`status_id` NOT IN(6,9, 7)) ) order by `f`.`name`, `d`.`customer_name`", null);
						
						break;
					#endregion

					#region Work Orders
					case "Work Orders": Session["huddle_source_list"] = _tools.getSQL_datatable(@"SELECT woprog_id id, woprog_bvwo visible_id, Concat(woprog_bvwo,'-',business_unit.name,' - ',woprog_customername,' - ',woprog_description) name, woprog_status status, Concat('/wo_prog_frame.aspx?action=show&woprog_id=',woprog_id,'&business_unit_id=',business_unit_id) link, 4 source, '' name2 FROM woprog inner join business_unit on business_unit.id = business_unit_id  WHERE woprog_status!='Closed' and woprog_status!='Invoiced' and woprog_status!='Deleted' and business_unit_id !=8 ORDER BY business_unit_id Asc, woprog_id desc" , null);
						break;
					#endregion

					#region Customer
					case "Customers": Session["huddle_source_list"] = _tools.getSQL_datatable(@"SELECT customer_id id, customer_id visible_id, concat(customer_id,'-',customer_name,'-',address_city) name, customer_or_contact_status status, Concat('/sections/customer/index.aspx?customer_id=',customer_id) link, 8 source, '' name2 FROM customer join customer_or_contact_status on customer_or_contact_status.customer_or_contact_status_id = customer_status inner join address on address.Address_Table_ID = customer_id and address_table = 'Customer' and Address_Type = 'B'  WHERE customer_status!=4 and customer_status!=5 ORDER BY customer_name " , null);
						break;
					#endregion

					#region branches
					case "Business Units": Session["huddle_source_list"] = _tools.getSQL_datatable(string.Format(@"SELECT id, id visible_id, name name, active status,
Concat('/business_units_edit.aspx?c_id=',id) link, 7 source, '' name2 FROM business_unit  where id in ({0}) ORDER BY name ",  new Current_User().visible_business_units ),null);
						break;
					#endregion
					#region Purchase Orders
					case "Purchase Orders": Session["huddle_source_list"] = _tools.getSQL_datatable(@"SELECT poprog_header.poprog_id id, poprog_bvpo visible_id, Concat(name,'-',poprog_bvpo,'-',vendor_name) name, active status, Concat('../../../sections/purchaseorder/po_prog_add.aspx?poprogid=',poprog_id) link, 10 source, '' name2 FROM poprog_header inner join business_unit on poprog_header.business_unit_id = business_unit.id inner join vendor on poprog_header.poprog_vendor_id = vendor.Vendor_ID ORDER BY poprog_header.business_unit_id,poprog_bvpo", null);
						break;
					#endregion
				}
		}

		gv_source.DataSource = Session["huddle_source_list"];
		

		gv_source.DataBind();

	}

	protected void gv_source_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		Session["huddle_source_list"] = null;
		set_source_list();
	}
	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var clientData = new Dictionary<int, object>();
		var grid = sender as ASPxGridView;
		for (var i = grid.VisibleStartIndex; i < grid.VisibleRowCount; i++)
		{
			var rowValues = grid.GetRowValues(i,new string[] {"id","link"}) as object[];
//
			if (rowValues[0] != null)
			{
			var key = Convert.ToInt32(rowValues[0]);
				var link = rowValues[1].ToString();

				if (link == "")
					clientData.Add(key, "link");
			}
		}
		if (clientData.Count != 0)
		{
			e.Properties["cp_cellsToDisable"] = clientData;
		}

	}


    protected void cp_left_panel_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter == "new")
		{
	//		view_huddles.ClientVisible = false;
			captain.Value = current_user.id;
			active.Checked = true;
			tb_users.Value = "";
			description.Text = "";
			name.Text = "";
			dte.Text = "";
			delete_huddle.ClientVisible = false;
			tb_users.Tokens.Add(current_user.FullName);

		}
		
	}
	protected void Delete_huddle_Click(object sender, EventArgs e)
	{
		huddle h;
		if (view_huddles.SelectedIndex >= 0)
		{
			_tools.getSQL_void(@"Delete from huddle_task  where huddle =@v0", new object[] { view_huddles.Value });
			
			_tools.getSQL_void(@"Delete from huddle_user  where huddle =@v0", new object[] { view_huddles.Value });
			_tools.getSQL_void(@"Delete from huddle  where id =@v0", new object[] { view_huddles.Value });

			ScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "alert('Huddle Deleted');", true);
			pc.Tabs.Clear();
			pc.ClientVisible = false;
			gv.ClientVisible = false;
			description.Text = "";
			name.Text = "";
			tb_users.Tokens.Clear();
			dte.Text = "";
			load_huddle_selector();
			view_huddles.SelectedIndex = -1;
			active.Checked = false;
			delete_huddle.ClientVisible = false;
			Session["huddle_source_list"] = null;
			hdnid.Value = "0";
			load_huddle_selector();
			load_main_overview();
			load_teammates();
			lbltitle.Text = "No Huddle Selected";
			cp_left_panel.ClientVisible = false;
		}

	}
	
	protected void gv_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
	{
		
		var key = gv.GetRowValues(e.VisibleIndex,"id").ToString();
		var h = new huddle.task(Convert.ToInt32(key));
		h.delete();

		gv.DataBind();
	}
	protected void btn_add_custom_Click(object sender, EventArgs e)
	{
		if (ddl_custom_member.Value == null || ddl_custom_member.SelectedIndex == -1)
		{
			ScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "alert('You must assign this to a team member');", true);
			return;
		}
		if (dte_custom_date.Date < DateTime.Today)
		{
			ScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "alert('You must select a date in the future');", true);
			return;
		}

		var mid = Convert.ToInt32(ddl_custom_member.Value);
		
		var h = new huddle(Convert.ToInt32(view_huddles.Value));
		var ht = new huddle.task();

		ht.huddle_source = 2;
		ht.assigned_to = _tools.getSQL_int(@"Select ifnull((Select id from huddle_user  where member =@v0 and huddle =@v1  limit 1),0) ", new object[] { mid,h.id });
		ht.huddle = h.id;
		ht.linetext = txt_custom_item.Text;
		ht.link = "";
		ht.source_id = 0;
		ht.status = 1;
		ht.description = "";
		ht.date_due = dte_custom_date.Date;
		ht.created_by = Convert.ToInt32(current_user.id);
		ht.order_n = gv.VisibleRowCount;
		ht.save();
		gv.DataBind();

	}
	protected void gv_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
	{
		foreach (var args in e.UpdateValues)
		{
			var ht = new huddle.task(Convert.ToInt32(args.Keys["id"]));
			ht.date_due = Convert.ToDateTime(args.NewValues["date_due"]);
			if (args.NewValues["task_description"] == null)
			{
				args.NewValues["task_description"] = "";
			}
			if (args.NewValues["to_be_done"] == null)
			{
				args.NewValues["to_be_done"] = "";
			}

			ht.description = args.NewValues["task_description"].ToString();
			if (args.NewValues["status"].ToString() != args.OldValues["status"].ToString())
			{
				if (args.NewValues["status"].ToString() == "1")
				{
					ht.status = 1;
				}
				else if (args.NewValues["status"].ToString() == "2")
				{
					ht.status = 2;
				}
				else
				{
					ht.status = 3;
				}
			}
			if (args.NewValues["to_be_done"].ToString() != args.OldValues["to_be_done"].ToString())
			{
				ht.to_be_done = args.NewValues["to_be_done"].ToString();
			}
			ht.save();



			
		}


		gv.DataBind();
		//e.UpdateValues[0].OldValues["task_description"].ToString()
	}

    protected void pc_PreRender(object sender, EventArgs e)
	{
	if ((!IsPostBack) && (!IsCallback))
		{
			if (pc.Tabs.Count > 0)
			{
				pc.DataBind();
				pc.ActiveTabIndex = pc.Tabs.FindByName(current_user.id.ToString()) != null ? pc.Tabs.FindByName(current_user.id.ToString()).Index : 0;
			
				SqlDataSource1.DataBind();
				SqlDataSource1.SelectParameters["@mid"].DefaultValue = current_user.id.ToString();
				gv.DataBind();
				fix_order_n();
				gv.ClientVisible = true;

				lbltitle.Text = "Agenda for " + view_huddles.Text;
				var h = new huddle(Convert.ToInt32(view_huddles.Value));
				name.Text = h.name;
				description.Text = h.description;
				captain.Value = Convert.ToInt32(h.captain);
				active.Checked = h.active;
				dte.Text = h.date_of_huddle.ToString("yyyy-MM-dd HH:mm");
				createddate.Text = "Created: " + h.dt_created.ToString("yyyy-MM-dd HH:mm");
				delete_huddle.ClientVisible = (h.captain == Convert.ToInt32(current_user.id));

			}
		}
	}
	protected void pop_push_WindowCallback(object source, PopupWindowCallbackArgs e)
	{
		if (e.Parameter!="")
		{
			if (e.Parameter.Equals("push"))
			{
				var h = new huddle(Convert.ToInt32(ddl_pop_huddle.Value));
				var ht_old = new huddle.task(Convert.ToInt32(gv.GetRowValues(Convert.ToInt32(hdn_pop_task["key"]), "id")));


				var h2 = new huddle(Convert.ToInt32(view_huddles.Value));
				var ht = new huddle.task();

				ht.huddle_source = ht_old.huddle_source;
				var a = _tools.getSQL_int(@"Select ifnull((Select huddle_user.member from huddle_user  where huddle_user.id =@v0 and huddle_user.huddle =@v1 ),0) ", new object[] { ht_old.assigned_to,h2.id });
				ht.assigned_to = _tools.getSQL_int(@"Select ifnull((Select id from huddle_user  where member =@v0 and huddle =@v1  limit 1),0) ", new object[] { a,h.id });
				ht.huddle = h.id;
				ht.linetext = ht_old.linetext;
				ht.link = ht_old.link;
				ht.source_id = ht_old.source_id;
				ht.status = 1;
				ht.description = ht_old.description;
	//			ht.date_due = dte_custom_date.Date;
				ht.created_by = ht_old.created_by;
				ht.order_n = ht_old.order_n;
				ht.to_be_done = ASPxMemo1.Text;
				ht.save();

				ht_old.status = 4;
				ht_old.save();

				pop_push.JSProperties["cp_alert"] = "Task Pushed to huddle: " + h.description + " on " + h.date_of_huddle;
				return;
			}


			hdn_pop_task["key"] = e.Parameter;
			lbl_pop_task.Text = gv.GetRowValues(Convert.ToInt32(hdn_pop_task["key"]), "task").ToString();
			ASPxMemo1.Text = gv.GetRowValues(Convert.ToInt32(hdn_pop_task["key"]), "to_be_done").ToString();

			using (var uow = new UnitOfWork())
			{
				var m = uow.GetObjectByKey<ne_xpo.cs.member>((int)current_user.id);
				var huds = (from x in new XPQuery<ne_xpo.cs.huddle>(uow)
							join y in new XPQuery<ne_xpo.cs.huddle_user>(uow) on x.id equals y.huddle.id
							where
								(x.created_by == m || x.captain == m || y.member == m) && x.name != "" && x.name != null && x.date_of_huddle > DateTime.Today

							select new
							{
								id = x.id,
								name = (x.date_of_huddle.Year + "-" + x.date_of_huddle.Month.ToString().PadLeft(2, '0') + "-" + x.date_of_huddle.Day.ToString().PadLeft(2, '0') + " " + x.date_of_huddle.Hour.ToString().PadLeft(2, '0') + ":" + x.date_of_huddle.Minute.ToString().PadLeft(2, '0') + " - " + x.name),
								date = x.date_of_huddle
							}

											).Distinct().OrderByDescending(x => x.date).ToList();
				ddl_pop_huddle.DataSource = huds;
				ddl_pop_huddle.TextField = "name";
				ddl_pop_huddle.ValueField = "id";
				ddl_pop_huddle.DataBind();
				ddl_pop_huddle.SelectedIndex = ddl_pop_huddle.Items.Count > 0 ? 0 : -1;

			}

		}
	}

	protected void gv_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (e.ButtonID.Equals("push"))
			{
				if ((gv.GetRowValues(e.VisibleIndex, "status").ToString().Equals("Pushed")) || (gv.GetRowValues(e.VisibleIndex, "status").ToString().Equals("Completed")))
				{
					e.Visible = DevExpress.Utils.DefaultBoolean.False;
				}
			}
		}
	}
	protected void gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if(e.VisibleIndex > -1)
			{
			if(e.DataColumn.FieldName == "status")
				{
				switch(Toolbox.ReturnBlankIfNull_string(e.CellValue).ToString())
					{
					case "Completed":
						e.Cell.BackColor		= System.Drawing.Color.LimeGreen;
					break;
					case "Not Started":
						e.Cell.BackColor		= System.Drawing.Color.White;
					break;
					case "Started":
						e.Cell.BackColor		= System.Drawing.Color.LightGreen;
					break;
					case "Pushed":
						e.Cell.BackColor		= System.Drawing.Color.LightGray;
					break;
					}
				}
			}
		}
}