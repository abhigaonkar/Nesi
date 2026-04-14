using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_customer_credit_limit_admin : System.Web.UI.Page
	{
	NeMember current_user		= new NeMember();
	private const string _page_name = "CreditLimitAdmin";
	private const int _page_id = 136;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;

	Toolbox _tools;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		current_user						= Toolbox.do_handle_authentication(_page_id);
		layout.__page_name					= _page_name;
		layout.used_gv						= gv_credit_limit;
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;

		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Credit Limit Admin";

		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= current_user.id.ToString();

		if (!IsPostBack)
			{
			var gl	= new NeGridLayouts(current_user.id, _page_name);
			if(gl.GridLayoutID == 0)
				{
				gl.GridLayout_Layout		= gv_credit_limit.SaveClientLayout();
				gl.member_id	= current_user.id;
				gl.GridLayout_Name			= "Default";
				gl.GridLayout_Gridid		= _page_name;
				gl.SaveGridLayout();
				
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_credit_limit.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text					= gl.GridLayout_Name;
			}
		}
	protected void gv_credit_limit_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void gv_credit_limit_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
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
}