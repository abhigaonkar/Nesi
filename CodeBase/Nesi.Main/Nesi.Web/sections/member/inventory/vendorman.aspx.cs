using System;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_member_inventory_vendorman : System.Web.UI.Page
	{
		public NeMember current_user;
		Toolbox _tools;
		JavaScriptSerializer jSON = new JavaScriptSerializer();
		private const int _page_id = 198; // from Page table in DB
		public const string _page_description = "Vendor / Manufacturer Cross Reference";
		public string selected_master;
		public const string _page_name = "vendorman";
		ASPxHiddenField h;
		SqlDataSource ds_templates;
		ASPxDropDownEdit dde_filter;
		Panel panel_export;
		NeBusinessUnit this_company;


		protected void Page_Init(object sender, EventArgs e)
		{
			_tools = new Toolbox();
			current_user = Toolbox.do_handle_authentication(_page_id);
			layout.__page_name = _page_name;
			_tools.dont_cache_page();
			if (Session["working_business_unit_id"] == null || !current_user.AuthenticatedForPrivilege(4))
			{
				Session.Add("working_business_unit_id", current_user.business_unit_id.ToString());
			}
			var _q = Request.QueryString;
			h = (ASPxHiddenField)layout.FindControl("h");
			ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
			dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
			panel_export = (Panel)layout.FindControl("panel_export");
			this_company = new NeBusinessUnit(Session["working_business_unit_id"]);
			panel_export.Visible = true;
			layout.used_gv = gv;
		}
	protected void Page_Load(object sender, EventArgs e)
		{
			_tools.add_css("/css/inventory_branch.css");
			var _q = Request.QueryString;
			//gv_orders.Columns["Save"].Visible	= button_manageprices.Visible = is_purchaser;
			var _f = Request.Form;
			var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
			var lbltemp = (Label)Page.Master.FindControl("lblHeading");
			lbltemp.Text = _page_description;
			var gv_id = "gv";
			NeGridLayouts gl;
			h.Set("gridview_id", gv_id);
			ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name + "_" + gv_id + "_" + this_company.DSN;
			ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
			gl = new NeGridLayouts(current_user.id, _page_name + "_" + gv_id + "_" + this_company.DSN);

			if (gl.GridLayout_Layout != "")
			{
					gv.LoadClientLayout(gl.GridLayout_Layout);
			}
			else
			{
				gl = new NeGridLayouts();
				gl.GridLayout_Layout = gv.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name + "_" + gv_id + "_" + this_company.DSN;
				gl.SaveGridLayout();
			}
			h.Set("ID", gl.GridLayoutID);
			h.Set("NAME", gl.GridLayout_Name);
			dde_filter.Text = gl.GridLayout_Name;
			panel_export.Visible = true;

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
	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
}
