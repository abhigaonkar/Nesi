using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_member_core_responsibilities : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 137;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	Toolbox _tools;
	string _page_name = "cap_coreresponsibilities";
	protected void Page_Init()
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		LayoutControl1.__page_name = _page_name;
		LayoutControl1.used_gv = gvcore;
		h = (ASPxHiddenField)LayoutControl1.FindControl("h");
		ds_templates = (SqlDataSource)LayoutControl1.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)LayoutControl1.FindControl("dde_filter");
		panel_export = (Panel)LayoutControl1.FindControl("panel_export");
		panel_export.Visible = true;
		h.Set("gridview_id", "gvcore");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	    Session["visibleBU"] = new Current_User().visible_business_units;

        gvcore.DataBind();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));
		var _q = Request.QueryString;
		var cb = (ASPxComboBox)gvcore.FindTitleTemplateControl("ddlbranch");
		if (!IsPostBack)
		{
			cb.Text = current_user.business_unit_name;
			cb.Value = current_user.business_unit_id;
			cb.DataBind();
			gvcore.FilterExpression = "status='Active'";
			Session["gvcore"] = null;

			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gl.GridLayout_Layout = gvcore.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gvcore.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
		}
hdnCompany["value"] = cb.Value.ToString();
fillgrid();

	}
	protected void cb_chkprivate_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var p = e.Parameter.Split('|');
		_tools.getSQL_void("update cr_skills_link set cr_skills_priority_id =@v0  where id = @v1" , new object[] {  p[1],p[0]});
	}

	protected void gvtypes_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var ASPxPageControl1 = (ASPxPageControl) gvcore.FindEditFormTemplateControl("ASPxPageControl1");
		var ASPxRoundPanel1 = (ASPxRoundPanel) ASPxPageControl1.FindControl("ASPxRoundPanel1");
		var core = (ASPxTextBox) ASPxRoundPanel1.FindControl("txtcore");
		var ddlstatus = (ASPxComboBox) ASPxRoundPanel1.FindControl("ddlstatus");
		var ddlgroup = (ASPxComboBox) ASPxRoundPanel1.FindControl("ddlgroup");
		var lbid = (ASPxLabel) ASPxRoundPanel1.FindControl("lbid");
		var mem = (ASPxMemo) ASPxRoundPanel1.FindControl("mem");
		var txtdaily = (ASPxTextBox) ASPxRoundPanel1.FindControl("txtdaily");
		var txtweekly = (ASPxTextBox) ASPxRoundPanel1.FindControl("txtweekly");
		var txtmonthly = (ASPxTextBox) ASPxRoundPanel1.FindControl("txtmonthly");
		var txtquarterly = (ASPxTextBox) ASPxRoundPanel1.FindControl("txtquarterly");
		var txtannually = (ASPxTextBox) ASPxRoundPanel1.FindControl("txtannually");
		var txtas_required = (ASPxTextBox) ASPxRoundPanel1.FindControl("txtas_required");


		if (core.Text == "")
			{
			throw new Exception("You must enter a core responsibility");
			}
		_tools.getSQL_void(@"update core_responsibilities set core_responsibility =@v0,member_id =@v1,
status=@v2,description=@v3,weekly=@v4,daily=@v5,monthly=@v6,quarterly=@v7,annually=@v8,
as_required=@v9,cr_group_id=@v10 where id =@v11 ", 
new object[]
			{
			core.Text,current_user.id,ddlstatus.Text,mem.Text,txtweekly.Text,txtdaily.Text,
				txtmonthly.Text,txtquarterly.Text,txtannually.Text,txtas_required.Text,ddlgroup.Value,
			lbid.Text
			
		});

	gvcore.CancelEdit();
		gvcore.DataBind();
		e.Cancel = true;
		Session["gvcore"] = null;
		fillgrid();
		
	}
	protected void gvtypes_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		var ASPxPageControl1 = (ASPxPageControl)gvcore.FindEditFormTemplateControl("ASPxPageControl1");
		var ASPxRoundPanel1 = (ASPxRoundPanel)ASPxPageControl1.FindControl("ASPxRoundPanel1");
		var core = (ASPxTextBox)ASPxRoundPanel1.FindControl("txtcore");
		var ddlstatus = (ASPxComboBox)ASPxRoundPanel1.FindControl("ddlstatus");
		var ddlgroup = (ASPxComboBox)ASPxRoundPanel1.FindControl("ddlgroup");
		var lbid = (ASPxLabel)ASPxRoundPanel1.FindControl("lbid");
		var mem = (ASPxMemo)ASPxRoundPanel1.FindControl("mem");
		var txtdaily = (ASPxTextBox)ASPxRoundPanel1.FindControl("txtdaily");
		var txtweekly = (ASPxTextBox)ASPxRoundPanel1.FindControl("txtweekly");
		var txtmonthly = (ASPxTextBox)ASPxRoundPanel1.FindControl("txtmonthly");
		var txtquarterly = (ASPxTextBox)ASPxRoundPanel1.FindControl("txtquarterly");
		var txtannually = (ASPxTextBox)ASPxRoundPanel1.FindControl("txtannually");
		var txtas_required = (ASPxTextBox)ASPxRoundPanel1.FindControl("txtas_required");

		if (core.Text == "")
		{
			throw new Exception("You must enter a core responsibility");
		}

		_tools.getSQL_void(@"Insert into core_responsibilities 
(core_responsibility,member_id,date_added,status,description,daily,weekly,monthly,quarterly,annually,as_required,cr_group_id) 
													values(@v0,@v1,curdate(),@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10)",
			new object[] { 	core.Text,current_user.id, ddlstatus.Text,mem.Text,txtdaily.Text,txtweekly.Text,txtmonthly.Text,
				txtquarterly.Text,txtannually.Text,
			txtas_required.Text,ddlgroup.Value });
	
		gvcore.CancelEdit();
		gvcore.DataBind();
		e.Cancel = true;
		Session["gvcore"] = null;
		fillgrid();

		var newid = _tools.getSQL_int(@"Select id from core_responsibilities order by id desc limit 1"  , null);
		gvcore.StartEdit(gvcore.FindVisibleIndexByKeyValue(newid));
		
	}


	protected void gvcore_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (e.DataColumn.FieldName == "core_responsibility")
			{
				e.Cell.ToolTip = gvcore.GetRowValues(e.VisibleIndex, "description").ToString();
			}
		}
	}


	protected void gv_review_items_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
	{

	}
	protected void gv_review_items_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (e.DataColumn.FieldName == "item")
			{
				var gv = (ASPxGridView)sender;
				e.Cell.ToolTip = gv.GetRowValues(e.VisibleIndex, "description").ToString();
			}
		}
	}


	protected void ddlbranch_Init(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			var c = (ASPxComboBox)sender;
			c.Value = current_user.business_unit_id;
			c.DataBind();
		}
	}
	protected void gvcore_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (gvcore.GetRowValues(e.VisibleIndex, "a").ToString() == "0")
			{
				e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			}
		}
	}


	protected void gvcore_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
	
		var editedrow = gvcore.EditingRowVisibleIndex;
		if (e.Parameters == "next")
		{
			if (editedrow == gvcore.VisibleRowCount)
			{

				gvcore.AddNewRow();
			}
			else
			{
				if (editedrow < 0)
				{
					gvcore.AddNewRow();
				}
				else
				{
					gvcore.StartEdit(editedrow + 1);
				}
				}
			}
		else if (e.Parameters == "prev")
		{
			gvcore.StartEdit(editedrow - 1);
			
		}
		if (e.Parameters.Split('|').Length > 0)
		{
			if (e.Parameters.Split('|').GetValue(0).ToString() == "filter")
			{
				Session["gvcore"] = null;
				hdnCompany["value"] = e.Parameters.Split('|').GetValue(1);
				fillgrid();

			}
		}
		
		if (e.Parameters[0].ToString() == "x")
		{
			var search = e.Parameters.Split('|').GetValue(1).ToString();
			if (search == "")
			{
				Session["gvcore"] = null;
				fillgrid();
			}
			else
			{
				Session["gvcore"] = _tools.getSQL_datatable(@"SELECT
core_responsibilities.id,
core_responsibilities.core_responsibility AS core_responsibility,
core_responsibilities.cr_group_id,
core_responsibilities.member_id,
core_responsibilities.date_added,
core_responsibilities.last_modified_date,
core_responsibilities.`status`,
core_responsibilities.default_target,
core_responsibilities.description AS description,
daily AS daily,
weekly AS weekly,
monthly AS monthly,
quarterly AS quarterly,
annually AS annually,
as_required AS as_required,
membertype_responsibilities.membertype_id,
sum(ifnull((Select count(member_id) from member where member.Member_MemberType_ID=membertype_responsibilities.membertype_id and member.business_unit_id " + (hdnCompany["value"].ToString() == "999" ? ">0" : "=" + hdnCompany["value"]) + @" and member_status = 'Active'),0)) as a
FROM
core_responsibilities
LEFT JOIN membertype_responsibilities ON membertype_responsibilities.core_responsibility_id = core_responsibilities.id
where (core_responsibility like '%" + search + @"%' or daily like '%" + search + @"%' or weekly like '%" + search + @"%' or monthly like '%" + search + @"%' or quarterly like '%" + search + @"%' or annually like '%" + search + @"%' or as_required like '%" + search + @"%' or description like '%" + search + @"%') 
group by core_responsibilities.id 
order by core_responsibilities.core_responsibility",null);
				fillgrid();
			}
		}

		if (e.Parameters == "")
		{
			gvcore.FilterExpression = "";
			for (var i = 0; i < gvcore.Columns.Count; i++)
			{
				if (gvcore.Columns[i] is GridViewDataColumn)
				{
					var col = (GridViewDataColumn)gvcore.Columns[i];
					if (col.GroupIndex > -1)
					{
						gvcore.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}
	}
	protected void fillgrid ()
	{
		var tb = (ASPxButtonEdit)gvcore.FindTitleTemplateControl("ASPxButtonEdit1");
		
		var search = tb.Text;
		if (Session["gvcore"] == null)
		{
			if (search != "")
			{
				Session["gvcore"] = _tools.getSQL_datatable(@"SELECT
core_responsibilities.id,
core_responsibilities.core_responsibility AS core_responsibility,
core_responsibilities.cr_group_id,
core_responsibilities.member_id,
core_responsibilities.date_added,
core_responsibilities.last_modified_date,
core_responsibilities.`status`,
core_responsibilities.default_target,
core_responsibilities.description AS description,
daily AS daily,
weekly AS weekly,
monthly AS monthly,
quarterly AS quarterly,
annually AS annually,
as_required AS as_required,
membertype_responsibilities.membertype_id,
sum(ifnull((Select count(member_id) from member where member.Member_MemberType_ID=membertype_responsibilities.membertype_id and member.business_unit_id " + (hdnCompany["value"].ToString() == "999" ? ">0" : "=" + hdnCompany["value"]) + @" and member_status = 'Active'),0)) as a
FROM
core_responsibilities
LEFT JOIN membertype_responsibilities ON membertype_responsibilities.core_responsibility_id = core_responsibilities.id
where (core_responsibility like '%" + search + @"%' or daily like '%" + search + @"%' or weekly like '%" + search + @"%' or monthly like '%" + search + @"%' or quarterly like '%" + search + @"%' or annually like '%" + search + @"%' or as_required like '%" + search + @"%' or description like '%" + search + @"%') 
group by core_responsibilities.id 
order by core_responsibilities.core_responsibility",null);
			}
		}
		if (Session["gvcore"] == null)
		{
		Session["gvcore"] = _tools.getSQL_datatable(@"SELECT
core_responsibilities.id,
core_responsibilities.core_responsibility AS core_responsibility,
core_responsibilities.cr_group_id,
core_responsibilities.member_id,
core_responsibilities.date_added,
core_responsibilities.last_modified_date,
core_responsibilities.`status`,
core_responsibilities.default_target,
core_responsibilities.description AS description,
daily AS daily,
weekly AS weekly,
monthly AS monthly,
quarterly AS quarterly,
annually AS annually,
as_required AS as_required,
membertype_responsibilities.membertype_id,
sum(ifnull((Select count(member_id) from member where member.Member_MemberType_ID=membertype_responsibilities.membertype_id and member.business_unit_id " + (hdnCompany["value"].ToString() == "999" ? ">0" : "=" + hdnCompany["value"]) + @" and member_status = 'Active'),0)) as a
FROM
core_responsibilities
LEFT JOIN membertype_responsibilities ON membertype_responsibilities.core_responsibility_id = core_responsibilities.id
group by core_responsibilities.id
order by core_responsibilities.core_responsibility",null);

		}
		gvcore.DataSource = Session["gvcore"];
		gvcore.DataBind();
	}
	protected void gvcore_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		gvcore.SettingsText.PopupEditFormCaption = gvcore.GetRowValuesByKeyValue(e.EditingKeyValue, "core_responsibility").ToString();
	}
	protected void gvcore_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var frame = (HtmlContainerControl)gvcore.FindEditFormTemplateControl("iframe_cr");
		if (!gvcore.IsNewRowEditing)  // are we editting here?
		{
			var rowIndex = gvcore.EditingRowVisibleIndex;
			var value1 = gvcore.GetRowValues(rowIndex, new string[] { "id" });
			frame.Attributes.Add("src", "cr_detail.aspx?id=" + value1);
		}
		else
		{

			frame.Attributes.Add("src", "cr_detail.aspx?id=0");
		}
	}
	protected void gvcore_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		Session["gvcore"] = null;
		fillgrid();
	}
	protected void gvcore_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		_tools.getSQL_void(@"Delete from core_responsibilities where id = @v0 limit 1", new object[] { e.Keys[0]});

		gvcore.CancelEdit();
		e.Cancel = true;
		Session["gvcore"] = null;
		fillgrid();
	}
	protected void gvcore_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
}
	

	