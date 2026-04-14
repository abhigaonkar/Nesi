using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_member_training_header : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 155;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	Toolbox _tools;
	string _page_name = "cap_training_header";
	protected void Page_Init()
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));
		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		LayoutControl1.__page_name = _page_name;
		LayoutControl1.used_gv = gvtraining;
		h = (ASPxHiddenField)LayoutControl1.FindControl("h");
		ds_templates = (SqlDataSource)LayoutControl1.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)LayoutControl1.FindControl("dde_filter");
		panel_export = (Panel)LayoutControl1.FindControl("panel_export");
		panel_export.Visible = true;
		h.Set("gridview_id", "gvtraining");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();

		gvtraining.DataBind();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var _q = Request.QueryString;
		if (!IsPostBack)
		{
			Session["gvtraining"] = null;
			Session["gv_training_searchstring"] = null;
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gl.GridLayout_Layout = gvtraining.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gvtraining.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
		}
	fillgrid();

	}
	
	protected void gvtraining_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			
		}
	}


	protected void gvtraining_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
	
		var editedrow = gvtraining.EditingRowVisibleIndex;
		
		if (e.Parameters[0] == 'x')
		{
			var x = e.Parameters.Split('|');
			Session["gv_training_searchstring"] = x[1];
			Session["gvtraining"] = null;
			fillgrid();
		}
		else if (e.Parameters == "next")
		{
			if (editedrow == gvtraining.VisibleRowCount)
			{

				gvtraining.AddNewRow();
			}
			else
			{
				if (editedrow < 0)
				{
					gvtraining.AddNewRow();
				}
				else
				{
					gvtraining.StartEdit(editedrow + 1);
				}
				}
			}
		else if (e.Parameters == "prev")
		{
			gvtraining.StartEdit(editedrow - 1);
			
		}
		else
		{
			gvtraining.FilterExpression = "";
			for (var i = 0; i < gvtraining.Columns.Count; i++)
			{
				if (gvtraining.Columns[i] is GridViewDataColumn)
				{
					var col = (GridViewDataColumn)gvtraining.Columns[i];
					if (col.GroupIndex > -1)
					{
						gvtraining.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}
		
	}
	protected void fillgrid ()
	{
		if (Session["gvtraining"] == null)
		{
			if (Session["gv_training_searchstring"] == null)
			{
				Session["gvtraining"] = _tools.getSQL_datatable(@"select * from training_header",null);
			}
			else
			{
				Session["gvtraining"] = _tools.getSQL_datatable(@"select * from training_header where name like '%" + Session["gv_training_searchstring"] + "%' or notes like '%" + Session["gv_training_searchstring"] + "%' or contact_info like '%" + Session["gv_training_searchstring"] + "%' or url like '%" + Session["gv_training_searchstring"] + "%'",null);
			}
		}
		gvtraining.DataSource = Session["gvtraining"];
		gvtraining.DataBind();
	}
	protected void gvtraining_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		gvtraining.SettingsText.PopupEditFormCaption = gvtraining.GetRowValuesByKeyValue(e.EditingKeyValue, "name").ToString();
	}
	protected void gvtraining_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var frame = (HtmlContainerControl)gvtraining.FindEditFormTemplateControl("iframe_cr");
		if (!gvtraining.IsNewRowEditing)  // are we editting here?
		{
			var rowIndex = gvtraining.EditingRowVisibleIndex;
			var value1 = gvtraining.GetRowValues(rowIndex, new string[] { "id" });
			Session["trainingid"] = null;
			frame.Attributes.Add("src", "training_detail.aspx?id=" + value1);
		}
		else
		{
			Session["trainingid"] = null;
			frame.Attributes.Add("src", "training_detail.aspx?id=0");
		}
	}

	protected void gvtraining_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
	
		gvtraining.CancelEdit();
		e.Cancel = true;
		Session["gvtraining"] = null;
		fillgrid();
		
	}
protected void  gvtraining_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
{
	gvtraining.CancelEdit();
	e.Cancel = true;
	Session["gvtraining"] = null;
	fillgrid();
	
}
protected void gvtraining_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
{
	Session["gvtraining"] = null;
	fillgrid();
}

protected void gvtraining_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
{

	_tools.getSQL_void(@"Delete from training_header where id =@v0 " , new object[] {
		e.Keys[0]});


	gvtraining.CancelEdit();
	e.Cancel = true;
	Session["gvtraining"] = null;
	fillgrid();
}
protected void gvtraining_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
{
	var gv = (ASPxGridView)sender;
	e.Properties["cpExp"] = gv.SaveClientLayout();
}
}
	

	