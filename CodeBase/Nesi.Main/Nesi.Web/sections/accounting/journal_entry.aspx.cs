using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class journal_entry : System.Web.UI.Page
{
    NeMember current_user;
	Toolbox _tools;
    private const int _page_id = 187; // from Page table in DB
    static string default_filter = "";
    ASPxHiddenField h;
    SqlDataSource ds_templates;
    ASPxDropDownEdit dde_filter;
    Panel panel_export;
    ASPxButton btn_export;
    ASPxButton btn_excel;
    private const string _page_name = "JournalEntries";


    protected void Page_Init(object sender, EventArgs e)
        {
     
        _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(_page_id);
        var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
        layout.__page_name = _page_name;
        layout.used_gv = gv;
        h = (ASPxHiddenField)layout.FindControl("h");
        ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
        dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
        panel_export = (Panel)layout.FindControl("panel_export");
        panel_export.Visible = true;
        btn_export = (ASPxButton)layout.FindControl("btn_pdf");
        btn_export.Visible = false;
        btn_excel = (ASPxButton)layout.FindControl("btn_excel");
        btn_excel.Visible = false;
        h.Set("gridview_id", "gv");
        ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
        ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();


    }

    protected void Page_Load(object sender, EventArgs e)
        {

        Sql_te.SelectCommand = "Select id, ddl_name from tax_entity where id in (" + new Current_User().visible_tax_entities +") order by ddl_name";
    var _q = Request.QueryString;
    if (!IsPostBack)
        {
          
        ddl_te.Value = current_user.business_unit.tax_entity_id;
        var gl = new NeGridLayouts(Convert.ToInt32(current_user.id), _page_name);
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

    
        }

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


    protected void ddl_te_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
