using System;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Internal;
using nesi.core;
using System.Collections.Generic;
using System.Data;

public partial class sections_reports_wo_part_history_index : Page
	{
	Toolbox _tools;
	NeMember current_user;
	JavaScriptSerializer jSON		= new JavaScriptSerializer();
	static int _page_id			= 220;
	static string _page_name		= "WOPartHistory";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools			= new Toolbox();
		current_user	= Toolbox.do_handle_authentication(_page_id);
		layout.__page_name					= _page_name;
		if(Session["working_business_unit_id"] != null)
			{
			Session.Remove("working_business_unit_id");
			}
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		layout.used_gv			= gv_wo_part_history_grid;
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= current_user.id.ToString();
		
		}
	protected void Page_Load(object sender, EventArgs e)
		{
			var lbltemp = (Label)Page.Master.FindControl("lblHeading");
			lbltemp.Text = "WO Part History";
		if(!IsCallback && !IsPostBack)
			{
			var menu			= new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml	= menu.MenuHTML;
			var gl	= new NeGridLayouts(Convert.ToInt32(current_user.id), _page_name);
			h.Set("gridview_id", "gv_wo_part_history_grid");
			if(gl.GridLayoutID == 0)
				{
                gv_wo_part_history_grid.FilterExpression = "";
				gl.GridLayout_Layout				= gv_wo_part_history_grid.SaveClientLayout();
				gl.member_id			= current_user.id;
				gl.GridLayout_Name					= "Default";
				gl.GridLayout_Gridid				= _page_name;
				gl.SaveGridLayout();
				
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
                gv_wo_part_history_grid.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text					= gl.GridLayout_Name;
			}

        if (!IsPostBack)
        {
            Session["workorder_part_history_grid"] = null;
        }
        fill_parthistory();

        }

    private void fill_parthistory()
    {
        

        if (Session["workorder_part_history_grid"] == null)
        {
            
            DataTable _part_history_dt;

            _part_history_dt = Toolbox.doSQL_dt(@"CALL report_wo_commits('2017-01-01', @v0)",new object[] { System.DateTime.Today.ToString("yyyy-MM-dd") });
           
           
            Session["workorder_part_history_grid"] = _part_history_dt.DefaultView.ToTable();
        }
        gv_wo_part_history_grid.DataSource = Session["workorder_part_history_grid"];
        gv_wo_part_history_grid.DataBind();
    }

    protected void gv_wo_part_history_grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		if(e.Parameters != "")
			{
			gv.LoadClientLayout(e.Parameters);
			}
		else
			{
			gv.FilterExpression		= "";
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
	protected void gv_wo_part_history_grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void gv_wo_part_history_grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
	}
	protected void header_Init(object sender, EventArgs e)
		{
		var lb		= (ASPxLabel) sender;
		lb.Attributes.Add("data-title", lb.Text.Replace("\"", ""));
		lb.CssClass			= "ttip";
		}
}
