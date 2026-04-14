using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_member_certificates : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 154;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	Toolbox _tools;
	string _page_name = "cap_certificates";
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
		LayoutControl1.used_gv = gvcert;
		h = (ASPxHiddenField)LayoutControl1.FindControl("h");
		ds_templates = (SqlDataSource)LayoutControl1.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)LayoutControl1.FindControl("dde_filter");
		panel_export = (Panel)LayoutControl1.FindControl("panel_export");
		panel_export.Visible = true;
		h.Set("gridview_id", "gvcert");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	//	gvcert.DataBind();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var _q = Request.QueryString;
		if (!IsPostBack)
		{
			Session["gvcert"] = null;
			Session["gv_cert_searchstring"] = null;
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gl.GridLayout_Layout = gvcert.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gvcert.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
		}
		
	fillgrid();

	}
	
	protected void gvcert_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			
		}
	}


	protected void gvcert_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
	
		var editedrow = gvcert.EditingRowVisibleIndex;
		
		if (e.Parameters[0] == 'x')
		{
			var x = e.Parameters.Split('|');
			Session["gv_cert_searchstring"] = x[1];
			Session["gvcert"] = null;
			fillgrid();
		}
		else if (e.Parameters == "next")
		{
			if (editedrow == gvcert.VisibleRowCount)
			{

				gvcert.AddNewRow();
			}
			else
			{
				if (editedrow < 0)
				{
					gvcert.AddNewRow();
				}
				else
				{
					gvcert.StartEdit(editedrow + 1);
				}
				}
			}
		else if (e.Parameters == "prev")
		{
			gvcert.StartEdit(editedrow - 1);
			
		}
		else
		{
			gvcert.FilterExpression = "";
			for (var i = 0; i < gvcert.Columns.Count; i++)
			{
				if (gvcert.Columns[i] is GridViewDataColumn)
				{
					var col = (GridViewDataColumn)gvcert.Columns[i];
					if (col.GroupIndex > -1)
					{
						gvcert.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}
		
	}
	protected void fillgrid ()
	{
		if (Session["gvcert"] == null)
		{
			if (Session["gv_cert_searchstring"] == null)
			{
				Session["gvcert"] = _tools.getSQL_datatable(@"select * from certificates"  , null);
			}
			else
			{
				Session["gvcert"] = _tools.getSQL_datatable(@"select * from certificates  where certificate_name like CONCAT('%',@v0,'%') or notes like CONCAT('%',@v1,'%') ", new object[] { Session["gv_cert_searchstring"],Session["gv_cert_searchstring"] });
			}
		}
		gvcert.DataSource = Session["gvcert"];
		gvcert.DataBind();
	}
	protected void gvcert_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		gvcert.SettingsText.PopupEditFormCaption = "Certificate: " + gvcert.GetRowValuesByKeyValue(e.EditingKeyValue, "certificate_name");
	}
	protected void gvcert_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var frame = (HtmlContainerControl)gvcert.FindEditFormTemplateControl("iframe_cr");
		if (!gvcert.IsNewRowEditing)  // are we editting here?
		{
			var rowIndex = gvcert.EditingRowVisibleIndex;
			var value1 = gvcert.GetRowValues(rowIndex, new string[] { "id" });
			frame.Attributes.Add("src", "certificates_detail.aspx?id=" + value1);
		}
		else
		{

			frame.Attributes.Add("src", "certificates_detail.aspx?id=0");
		}
	}

	protected void gvcert_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
	
		gvcert.CancelEdit();
		e.Cancel = true;
		Session["gvcert"] = null;
		fillgrid();
		
	}
protected void  gvcert_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
{
	gvcert.CancelEdit();
	e.Cancel = true;
	Session["gvcert"] = null;
	fillgrid();
	
}
protected void gvcert_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
{
	Session["gvcert"] = null;
	fillgrid();
}
protected void btnAddType_Init(object sender, EventArgs e)
{
	var b = (ASPxButton)sender;
	b.ClientEnabled = current_user.AuthenticatedForPrivilege(155);
}
protected void gvcert_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
{
	var gv = (ASPxGridView)sender;
	e.Properties["cpExp"] = gv.SaveClientLayout();
}
}
	

	