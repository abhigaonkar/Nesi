using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class advance_pay_reports_index : System.Web.UI.Page
    {
    NeMember myMember;
    static int _page_id = 219;
    static string _page_name = "Advance Pay Report";
    ASPxHiddenField h;
    SqlDataSource ds_templates;
    ASPxDropDownEdit dde_filter;
    Panel panel_export;
    ASPxButton btn_export;
    ASPxButton btn_excel;

    protected void Page_Init(object sender, EventArgs e)
        {
        var _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(_page_id);
        layout.__page_name = _page_name;
        h = (ASPxHiddenField) layout.FindControl("h");
        ds_templates = (SqlDataSource) layout.FindControl("ds_templates");
        dde_filter = (ASPxDropDownEdit) layout.FindControl("dde_filter");
        panel_export = (Panel) layout.FindControl("panel_export");
        panel_export.Visible = true;
        btn_export = (ASPxButton) layout.FindControl("btn_pdf");
        btn_export.Visible = false;
        btn_excel = (ASPxButton) layout.FindControl("btn_excel");
        btn_excel.Visible = false;
        layout.used_gv = gv;
        h.Set("gridview_id", "gv");
        ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
        ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
        }

    protected void Page_Load(object sender, EventArgs e)
        {
        var _tools = new Toolbox();
        gv.LoadClientLayout( "[Invoice Date] Is greater than '2017-09-05'");
        

        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
        if (!IsCallback && !IsPostBack)
            {
            var gl = new NeGridLayouts(myMember.id, _page_name);
            if (gl.GridLayoutID == 0)
                {
                DateTime todayMinusSizMonths = DateTime.Now.AddMonths(-6);
                gv.LoadClientLayout("page1|filter36|[WOProg_InvoiceDate] >= #" + todayMinusSizMonths.ToString("yyyy-MM-dd") + "#|conditions1|26|9|hierarchy29|0|-1|1|-1|2|-1|3|-1|4|-1|5|-1|6|-1|7|-1|8|-1|9|-1|10|-1|11|-1|12|-1|13|-1|14|-1|15|-1|16|-1|17|-1|18|-1|19|-1|20|-1|21|-1|22|-1|23|-1|24|-1|25|-1|26|-1|27|-1|28|-1|visible29|t0|t2|t3|t5|f6|t7|t8|t9|t12|t13|t14|t15|t16|t17|t18|t19|t20|t21|t22|t1|t10|t23|t24|t25|t26|t27|t28|t29|t30|width29|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e|e");
                gl.GridLayout_Layout = gv.SaveClientLayout();
                gl.member_id = myMember.id;
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
    }

