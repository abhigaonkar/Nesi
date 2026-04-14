using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.XtraGrid;
using nesi.core;

public partial class modules_phoneComments : System.Web.UI.UserControl
{


   

    public int _page_id = 203;
    static string _page_name = "PhoneComments";
    Toolbox _tools;
    NeMember current_user;
    ASPxHiddenField h;
    ASPxDropDownEdit dde_filter;
    Panel panel_export;
    public bool showLayout = true;
    public bool forTimeSheet = true;
    public string timeSheetDate = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        
        _tools = new Toolbox();
        if (forTimeSheet)
        {
            _page_id = 28;
            gv_phoneComments.SettingsPager.PageSize = 25;

        }
        current_user = Toolbox.do_handle_authentication(_page_id);
        
        if (showLayout)
        {
            layout.__page_name = _page_name;
            layout.used_gv = gv_phoneComments;
        
            h = (ASPxHiddenField) layout.FindControl("h");
            dde_filter = (ASPxDropDownEdit) layout.FindControl("dde_filter");
            panel_export = (Panel) layout.FindControl("panel_export");
            panel_export.Visible = true;
            gv_phoneComments.SettingsPager.PageSize = 75;
            SqlDataSource1.UpdateCommand = "";
        }
        else
        {
            layout.Visible = false;
        }
  
    }


    protected void Page_Load(object sender, EventArgs e)
    {
     //   Toolbox _tools = new Toolbox();
       // current_user = Toolbox.do_handle_authentication(1);
        //	_tools.dont_cache_page();


       


        if (!IsPostBack)
        {


            if (showLayout)
            {
                var gl = new NeGridLayouts(current_user.id, _page_name);
                h.Set("gridview_id", "gv_phoneComments");
                if (gl.GridLayoutID == 0)
                {
                    gl.GridLayout_Layout = gv_phoneComments.SaveClientLayout();
                    gl.member_id = current_user.id;
                    gl.GridLayout_Name = "Default";
                    gl.GridLayout_Gridid = _page_name;
                    gl.SaveGridLayout();

                    h.Set("ID", gl.GridLayoutID);
                    h.Set("NAME", gl.GridLayout_Name);
                }
                else
                {
                    gv_phoneComments.LoadClientLayout(gl.GridLayout_Layout);
                    h.Set("ID", gl.GridLayoutID);
                    h.Set("NAME", gl.GridLayout_Name);
                }
                dde_filter.Text = gl.GridLayout_Name;
            }
        }
        if (forTimeSheet)
        {
            populate_grid();
        }

    }

    public void save_grid()
    {
        gv_phoneComments.UpdateEdit();
    }

    public void populate_grid()
    {
        if (this.Visible)
        {
            var ext = current_user.PhoneExtension;

            string sql;
                if (timeSheetDate.Equals(string.Empty))
                {
                    sql = string.Format(@"
SELECT * FROM neintranet.phone_log
where (phone_log_from_number = {0} OR phone_log_to_number = {0} )AND phone_log_date >= curdate() - interval 1 day AND( is_internal = 0 OR is_internal is null)
", ext);
                }
                else
                {
                    sql = string.Format(@"
SELECT * FROM neintranet.phone_log
where (phone_log_from_number = {0} OR phone_log_to_number = {0} ) AND phone_log_date = ""{1}"" AND( is_internal = 0 OR is_internal is null)", ext, timeSheetDate);
                }
                if (ext.Equals(string.Empty))
                {
                    sql = "SELECT * FROM neintranet.phone_log limit 0";
                }
         //   gv_phoneComments.DataSourceID = null;
            SqlDataSource1.SelectCommand = sql;
            //gv_phoneComments.DataSource = dt;
            gv_phoneComments.DataBind();
            gv_phoneComments.Columns["phone_log_date"].Visible = false;
            gv_phoneComments.Settings.ShowFilterBar = 0;
            gv_phoneComments.Settings.ShowFilterRow = false;
            gv_phoneComments.Settings.ShowGroupPanel = false;
           
            
            Page.ClientScript.RegisterStartupScript(GetType(), "Test", @"
$(document).ready(function () {
		        $("".statusBar"").children().children().children().children().first().css(""display"", ""none"");
		    });

", true);
        }
    }

    protected void gv_phoneComments_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        if (showLayout)
        {
            var gv = (ASPxGridView) sender;
            e.Properties["cpExp"] = gv.SaveClientLayout();
        }
    }

    protected void gv_phoneComments_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (showLayout)
        {
            var gv = (ASPxGridView) sender;
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



    protected void gv_phoneComments_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (showLayout)
        {
            e.Editor.Visible = false;
        }
    }
}