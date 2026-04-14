using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using nesi.core;

public partial class taskmanagerlist : System.Web.UI.Page
{
    NeMember myMember;
    private const int _page_id = 35; // from Page table in DB
	static string _page_name = "TaskManagerlist";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	Toolbox _tools;
	NameValueCollection _q;

	protected void Page_Init(object sender, EventArgs e)
	{
		
 _tools = new Toolbox();
layout.__page_name = _page_name;
myMember = Toolbox.do_handle_authentication(_page_id);
		layout.used_gv			= gv;
h = (ASPxHiddenField)layout.FindControl("h");
ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
panel_export = (Panel)layout.FindControl("panel_export");
panel_export.Visible = true;
h.Set("gridview_id", "gv");
ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
	_q = Request.QueryString;
	}

    protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack && !IsCallback)
			{
			Session["visible_business_unit_ids"] = new Current_User().visible_business_units;
			if (_q["parent_id"] == null)
			{
				var gl = new NeGridLayouts(myMember.id, _page_name);
				if (gl.GridLayoutID == 0)
				{
					gv.DataBind();
					gv.FilterExpression = "[owner]=" + myMember.id;
					gv.SortBy(gv.Columns["id"], DevExpress.Data.ColumnSortOrder.Ascending);
					gl.GridLayout_Layout = gv.SaveClientLayout();
					gl.member_id = myMember.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();

					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				else
				{
					gv.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				dde_filter.Text = gl.GridLayout_Name;
			}
		}
		if (!IsPostBack)
		{
			Session["gv_task"] = null;
		}
		if (_q["parent_id"] != null)  // if this task is part of a parent task
		{
			layout.Visible = false;
			
			gv.Settings.ShowFooter = false;
			gv.Settings.ShowGroupPanel = false;
			gv.Settings.ShowFilterBar = GridViewStatusBarMode.Hidden;
            gv.SettingsPopup.EditForm.Height = Unit.Pixel(650);
			gv.SettingsPopup.EditForm.Width = Unit.Percentage(100);
			gv.StylesPopup.EditForm.Header.BackColor = System.Drawing.Color.Green;
			gv.Settings.ShowFilterRow = false;
			gv.Settings.ShowFilterRowMenu = false;
			gv.Columns["date_started"].Visible = false;
			gv.Columns["date_completed"].Visible = false;
			gv.Columns["cutby"].Visible = false;
			gv.Columns["parent_task"].Visible = false;
			gv.Columns["recurring"].Visible = false;
			gv.Columns["recurring_frequency"].Visible = false;
			gv.Columns["notify_warning_days"].Visible = false;
			gv.Columns["messageboard"].Visible = false;
			gv.Columns["recurring_resolution"].Visible = false;
			gv.Columns["dept"].Visible = false;
			gv.FindTitleTemplateControl("rdoowners").Visible = false;
			gv.FindTitleTemplateControl("rdodue").Visible = false;
			gv.FindTitleTemplateControl("chklst_showold").Visible = false;
			
		}
		else
		{
			gv.SettingsPopup.EditForm.Height = Unit.Pixel(700);
		
		}
		fill_grid();
    }

	protected void fill_grid()
	{
	//	if (Session["gv_task"] == null)
	//	{
			var chk = (ASPxCheckBox)gv.FindTitleTemplateControl("chklst_showold");
			var rdoowners = (ASPxRadioButtonList)gv.FindTitleTemplateControl("rdoowners");
			var rdodue = (ASPxRadioButtonList)gv.FindTitleTemplateControl("rdodue");


			var parent_filter = "";
			var old_filter = "";
			var user_filter = "";
			var date_filter = "";

			if (_q["parent_id"] == null)
			{
				parent_filter = "parent_task = 0";
				if (chk.Value.ToString() == "0")  // are we showing old ones?
				{ old_filter = " and status !=5"; }
				if (rdoowners.SelectedItem.Value.ToString() == "1")
				{
					user_filter = " and owner=" + myMember.id;
				}
				else if (rdoowners.SelectedItem.Value.ToString() == "2")
				{
					user_filter = " and (task_member.task_member_member_id = " + myMember.id + " or task.`owner` = " + myMember.id + ")";
				}
				if (rdodue.SelectedItem.Value.ToString() == "1")
				{
					date_filter = " and date_due<'" + System.DateTime.Today.AddDays(7).ToString("yyyy-MM-dd") + "'";
				}
				if (rdodue.SelectedItem.Value.ToString() == "2")
				{
					date_filter = " and '" + System.DateTime.Today.ToString("yyyy-MM-dd") + "' > date_due - Interval (notify_warning_days) day";
				}
			}
			else
			{
				parent_filter = "parent_task = " + _q["parent_id"];
			}
		//	Session["gv_task"] = _tools.getSQL_datatable(@"Select distinct task.*,urldecode(name) as name1 FROM task LEFT JOIN task_member ON task.id = task_member.task_member_task_id  where@v0", new object[] { parent_filter,old_filter,user_filter,date_filter });
		//}
		//gv.DataSource = Session["gv_task"];
			gv.DataSource = _tools.getSQL_datatable("Select distinct task.*,urldecode(name) as name1, date_format(date_due,'%Y-%m-%d') date_due1 FROM task LEFT JOIN task_member ON task.id = task_member.task_member_task_id where " + parent_filter + old_filter + user_filter + date_filter,null);
		gv.DataBind();


	}

	protected void btnAdd_Click(object sender, EventArgs e)
	{
		gv.AddNewRow();

	}
	protected void btnclose_Click(object sender, EventArgs e)
	{
		gv.CancelEdit();

		gv.DataBind();


	}
	protected void gv_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
	{
//		
		var sub_text = "";
		if (_q["parent_id"] != null)
		{
			sub_text = "&parent_id=" + _q["parent_id"];
		}
		
		if (gv.EditingRowVisibleIndex>=0)
		{
			var x = gv.EditingRowVisibleIndex;
			var frame = (HtmlContainerControl)gv.FindEditFormTemplateControl("I1");
			frame.Attributes.Add("src", "index.aspx?task_id=" + gv.GetRowValues(x, "id") + sub_text);
			frame.Attributes.Add("onload", "resizeIframe(this);");
		}
		else
		{
			var x = gv.EditingRowVisibleIndex;
			var frame = (HtmlContainerControl)gv.FindEditFormTemplateControl("I1");
			frame.Attributes.Add("src", "index.aspx?task_id=0" + sub_text);
			frame.Attributes.Add("onload", "resizeIframe(this);");
			
		}
	}
	protected void btnAdd_Click1(object sender, EventArgs e)
	{

		gv.AddNewRow();
	}
	protected void gv_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		gv.DataBind();
	}
	protected void cb_chkprivate_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		try
			{

			if (string.IsNullOrEmpty(e.Parameter)) return;

			var p = e.Parameter.Split('|');
			if (p[2] == "p")
			{
				_tools.getSQL_void(@"Update task  set private=@v0  where id=@v1", new object[] { Convert.ToInt16(Convert.ToBoolean(p[1])),p[0] });
			}
			else if (p[2] == "m")
			{
				_tools.getSQL_void(@"Update task  set messageboard=@v0  where id=@v1", new object[] { Convert.ToInt16(Convert.ToBoolean(p[1])),p[0] });
			}
			else if (p[2] == "e")
			{
				_tools.getSQL_void(@"Update task  set emails=@v0  where id=@v1", new object[] { Convert.ToInt16(Convert.ToBoolean(p[1])),p[0] });
			}
			else if (p[2] == "n")
			{
				_tools.getSQL_void(@"Update task  set name=@v0  where id=@v1", new object[] { _tools.value_to(p[1]),p[0] });
			}
			else if (p[2] == "o")
			{
				_tools.getSQL_void(@"Update task  set owner=@v0  where id=@v1", new object[] { p[1],p[0] });
			}
			else if (p[2] == "s")
			{
				_tools.getSQL_void(@"Update task  set status=@v0  where id=@v1", new object[] { p[1],p[0] });
			}
			else if (p[2] == "pr")
			{
				_tools.getSQL_void(@"Update task  set priority=@v0  where id=@v1", new object[] { p[1],p[0] });
			}
			else if (p[2] == "dd")
			{
				_tools.getSQL_void(@"Update task  set date_due=@v0  where id=@v1", new object[] { Convert.ToDateTime(p[1]).ToString("yyyy-MM-dd"),p[0] });
			}
			else if (p[2] == "de")
			{
				_tools.getSQL_void(@"Update task  set dept=@v0  where id=@v1", new object[] { Convert.ToInt32(p[1]),p[0] });
			}
		}
		catch
		{
			throw new Exception("Couldn't Update Task");
		}
	}
	protected void dtl_chkprivate_Init(object sender, EventArgs e)
	{
        var chk = sender as ASPxCheckBox;
        var container = chk.NamingContainer as GridViewDataItemTemplateContainer;
		chk.ClientSideEvents.CheckedChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('{0}|' + s.GetChecked()+ '|p'); }}", container.KeyValue);
    }
	protected void dtl_chkmessagboard_Init(object sender, EventArgs e)
	{
		var chk = sender as ASPxCheckBox;
		var container = chk.NamingContainer as GridViewDataItemTemplateContainer;
		chk.ClientSideEvents.CheckedChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('{0}|' + s.GetChecked()+ '|m'); }}", container.KeyValue);
	}
	protected void dtl_chkemails(object sender, EventArgs e)
	{
		var chk = sender as ASPxCheckBox;
		var container = chk.NamingContainer as GridViewDataItemTemplateContainer;
		chk.ClientSideEvents.CheckedChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('{0}|' + s.GetChecked()+ '|e'); }}", container.KeyValue);
	}

	protected void gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.FieldName == "notes")
		{
			e.Cell.Text = _tools.value_from(e.CellValue.ToString());
			e.Cell.ToolTip = e.Cell.Text;
		}
		
	}
	protected void txtname_Init(object sender, EventArgs e)
	{
		var txt = sender as ASPxTextBox;
		var container = txt.NamingContainer as GridViewDataItemTemplateContainer;
		txt.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('{0}|' + s.GetText()+ '|n'); }}", container.KeyValue);
		

	}
	protected void ddlowner_Init(object sender, EventArgs e)
	{
		var owner = sender as ASPxComboBox;
		var container = owner.NamingContainer as GridViewDataItemTemplateContainer;
		owner.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('{0}|' + s.GetValue()+ '|o'); }}", container.KeyValue);
	}
	protected void ddlstatus_Init(object sender, EventArgs e)
	{
		var status = sender as ASPxComboBox;
		var container = status.NamingContainer as GridViewDataItemTemplateContainer;
		status.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('{0}|' + s.GetValue()+ '|s'); }}", container.KeyValue);
	}
	protected void ddlpriority_Init(object sender, EventArgs e)
	{
		var priority = sender as ASPxComboBox;
		var container = priority.NamingContainer as GridViewDataItemTemplateContainer;
		priority.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('{0}|' + s.GetValue()+ '|pr'); }}", container.KeyValue);
		
	}
	protected void ddldept_Init(object sender, EventArgs e)
	{
		var dept = sender as ASPxComboBox;
		var container = dept.NamingContainer as GridViewDataItemTemplateContainer;
		dept.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('{0}|' + s.GetValue()+ '|de'); }}", container.KeyValue);
	}
	protected void dtedue_Init(object sender, EventArgs e)
	{
		var due = sender as ASPxDateEdit;
		var container = due.NamingContainer as GridViewDataItemTemplateContainer;
		due.ClientSideEvents.DateChanged = string.Format("function (s, e) {{lblid.SetText({0}); dte_dd.SetDate(s.GetValue()); pop_dd.Show(); }}", container.KeyValue);
	}
	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
	
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		
	}
	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		
		var gv				= (ASPxGridView) sender;
		if(e.Parameters != "")
			{
			gv.LoadClientLayout(e.Parameters);
			}
		else
			{
				gv.FilterExpression = "[owner]='" + myMember.id;
			for(var i = 0; i < gv.Columns.Count; i++)
				{
				if (gv.Columns[i] is GridViewDataColumn)
					{
					var col = (GridViewDataColumn) gv.Columns[i];
					if (col.GroupIndex > -1)
						{
						gv.UnGroup(col);
						}
					col.Visible = true;
					}
				}
			}
	
	}
	protected void gv_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		if (gv.IsNewRowEditing)
		{
			gv.SettingsText.PopupEditFormCaption = "Start New Task";
		}
		else
		{
			if (_q["parent_id"] == null)
			{
				gv.SettingsText.PopupEditFormCaption = "Task: " + e.EditingKeyValue + " - " + gv.GetRowValuesByKeyValue(e.EditingKeyValue,"name1").ToString().PadRight(75,' ').Substring(0,75);
			}
			else
			{
				gv.SettingsText.PopupEditFormCaption = "Sub Task: " + e.EditingKeyValue + " - " + gv.GetRowValuesByKeyValue(e.EditingKeyValue, "name1").ToString().PadRight(75,' ').Substring(0, 75);
			}
		}
	}
	protected void gv_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
	{
		if (_q["parent_id"] == null)
		{
			gv.SettingsText.PopupEditFormCaption = "Start New Task";
		}
		else
		{
			gv.SettingsText.PopupEditFormCaption = "Start New Sub Task";
		}
	}

	protected void ASPxButton1_Init(object sender, EventArgs e)
	{
		var due = sender as ASPxButton;
		var container = due.NamingContainer as GridViewDataItemTemplateContainer;
		var d = Convert.ToDateTime(container.Text).ToString("yyyy,M,d");
		due.ClientSideEvents.Click = string.Format("function (s, e) {{var d = new Date({1}); hdd.Set('yep','{0}'); dte_dd.SetDate(d); pop_dd.Show(); }}", container.KeyValue, d);
	
	}
	protected void btnsavedue_Click(object sender, EventArgs e)
	{
		if (Convert.ToString(hdd["yep"]) != "")
		{
			if (dte_dd.Date != null)
			{
				if (txtwhychange.Text != "")
				{
					var t = new NeTask(Convert.ToInt32(hdd["yep"]));
					
					_tools.getSQL_void(@"Insert into task_history (task_history_date,task_history_memberid,task_history_note,task_history_taskid)
values (@v0,@v1,CONCAT('Due Date Changed from ',@v2,' to ',@v3,' - ',@v4),@v5)",
						new object[] { System.DateTime.Today.ToString("yyyy-MM-dd"),myMember.id,t.date_due.ToString("yyyy-MM-dd"),dte_dd.Date.ToString("yyyy-MM-dd"),txtwhychange.Text,t.id } );
					t.date_due = dte_dd.Date;
					t.save();

					pop_dd.ShowOnPageLoad = false;
				}
				else
				{
					lbl_dd_changeError.Text = "Why did we change the date?";
					return;
				}
			}
			else
			{
				lbl_dd_changeError.Text = "You must select a valid new due date";
				return;
			}
		}
	}
}
