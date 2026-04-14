using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_reports_master_vendor_index : System.Web.UI.Page
{
	Toolbox _tools;
	NeMember current_user;
	static int _page_id = 211;
	static string _page_name = "MasterVendors";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;

	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
	
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;

		layout.used_gv = gv_vendors;
		h.Set("gridview_id", "gv_vendors");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Master Vendor Report";
		if (!IsCallback && !IsPostBack)
		{
			var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gv_vendors.FilterExpression = string.Format("[Vendor_Active] = 1");
				gl.GridLayout_Layout = gv_vendors.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_vendors.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
			Session["master_vendor_grid"] = null;
fill_grid();
		}
	//	
	}
	protected void fill_grid()
	{
		
		gv_vendors.DataBind();

	}


	protected void gv_vendors_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_vendors_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.Parameters != "")
		{
			gv.LoadClientLayout(e.Parameters);
		}
		else
		{
			gv.FilterExpression = "";
			for (var i = 0; i < gv.Columns.Count; i++)
			{
				if (gv.Columns[i] is GridViewDataColumn)
				{
					var col = (GridViewDataColumn)gv.Columns[i];
					if (col.GroupIndex > -1)
					{
						gv.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}
	}

	protected void hl_web_Init(object sender, EventArgs e)
	{
		try
		{
			var container = ((ASPxHyperLink)sender).NamingContainer as GridViewDataItemTemplateContainer;
			if (container.Text != "&nbsp;")
			{
				((ASPxHyperLink)sender).ClientSideEvents.Click = "function(s,e){ window.open('" + container.Text.Replace("www.", "http://") + "','','scrollbars=no,menubar=no,height=600,width=800,resizable=yes,toolbar=no,location=no,status=no');}";
			}
		}
		catch { }

	}

	protected void hl_wo_Init(object sender, EventArgs e)
	{
		var link = (ASPxHyperLink)sender;
		var container = (GridViewDataRowTemplateContainer)link.NamingContainer;
		//		if(gv_purchases.GetDataRow(container.VisibleIndex)["woprog_id"].ToString().Contains("For") || string.IsNullOrEmpty(gv_purchases.GetDataRow(container.VisibleIndex)["woprog_id"].ToString()))
		//			{
		//			link.Enabled							= false;
		//			}
	}
	protected void cb_Callback(object sender, CallbackEventArgsBase e)
	{
		try
		{
			var sql = "";
			var p = e.Parameter.Split('|');

		}
		catch
		{
			throw new Exception("Couldn't Update PO Header Table");
		}
	}
	protected void ASPxMemo1_Init(object sender, EventArgs e)
	{
		var dtecomp = sender as ASPxMemo;
		var container = dtecomp.NamingContainer as GridViewDataItemTemplateContainer;
		//dtecomp.Text = _tools.value_from(container.KeyValue);
		dtecomp.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb.PerformCallback('{0}|' + s.GetText() + '|n'); }}", container.KeyValue);

	}



	protected void ASPxComboBox2_Init(object sender, EventArgs e)
	{

		var dtecomp = sender as ASPxComboBox;
		var container = dtecomp.NamingContainer as GridViewDataItemTemplateContainer;
		dtecomp.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ cb.PerformCallback('{0}|' + s.GetValue() + '|p'); }}", container.KeyValue);
	}
}