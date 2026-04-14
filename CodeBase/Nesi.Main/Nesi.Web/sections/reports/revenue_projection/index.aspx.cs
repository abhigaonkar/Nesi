using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_reports_revenue_projection_index : System.Web.UI.Page
{
	Toolbox _tools;
	NeMember current_user;
	static int _page_id = 214;
	static string _page_name = "Revenue_Projection";
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

		layout.used_gv = gv;
		h.Set("gridview_id", "gv");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Revenue Projection Report";
        if (!IsCallback && !IsPostBack)
        {
            dte_start_month.Date = System.DateTime.Today;
            dte_end_month.Date = System.DateTime.Today;
            var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
            divMenu.InnerHtml = menu.MenuHTML;
            var gl = new NeGridLayouts(current_user.id, _page_name);
            if (gl.GridLayoutID == 0)
            {
                gv.FilterExpression = "";
                gl.GridLayout_Layout = gv.SaveClientLayout();
                gl.member_id = current_user.id;
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
            Session["rpt_Revenue_Projection"] = null;
        }
        fill_grid();
	}
	protected void fill_grid()
	{
    
	//TODO JA 5/31/2017 : should ask what TE you want. not hard coded 2. 
        if (Session["rpt_Revenue_Projection"]==null)
        {
            Session["rpt_Revenue_Projection"] = _tools.getSQL_datatable(@"call get_projected_revenue(@v0,@v1,2)",new object[] { dte_start_month.Date.ToString("yyyy-MM-dd") ,dte_end_month.Date.ToString("yyyy-MM-dd") });
            
        }
        gv.DataSource = Session["rpt_Revenue_Projection"];
        gv.DataBind();
        gv.DataBind();
	}


	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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





    protected void ASPxDateEdit1_DateChanged(object sender, EventArgs e)
    {
        Session["rpt_Revenue_Projection"] = null;
        fill_grid();
    }
}