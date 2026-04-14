using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class master_inventory : System.Web.UI.Page
{
    NeMember myMember;
    Toolbox _tools = new Toolbox();
    private const int _page_id = 90; // from Page table in DB
    private const string _page_name = "MasterInventory";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;

    protected void Page_Init(object sender, EventArgs e)
    {
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();
		layout.used_gv			= gv_MasterInventory;
		layout.__page_name					= _page_name;
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		h.Set("gridview_id", "gv_MasterInventory");
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= myMember.id.ToString();
    }



    protected void Page_Load(object sender, EventArgs e)
    {

        
        
        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
        if (!IsPostBack)
        {
			var gl	= new NeGridLayouts(myMember.id, _page_name);
			if(gl.GridLayoutID == 0)
				{
				gl.GridLayout_Layout		= gv_MasterInventory.SaveClientLayout();
				gl.member_id	= myMember.id;
				gl.GridLayout_Name			= "Default";
				gl.GridLayout_Gridid		= _page_name;
				gl.SaveGridLayout();
				
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_MasterInventory.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text					= gl.GridLayout_Name;
        }
      

        
       
        
    }
	protected void gv_MasterInventory_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		_tools.debug_note(e.Parameters);
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
	protected void gv_MasterInventory_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
}
