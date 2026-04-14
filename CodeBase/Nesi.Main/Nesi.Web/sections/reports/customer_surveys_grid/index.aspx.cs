using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class customer_surveys_grid : Page
{
    private NeMember myMember;
    private const int _page_id = 161; // from Page table in DB
    private const string _page_name = "CustView_surveys";
    Toolbox _tools;
    protected NameValueCollection _q;
    ASPxHiddenField h;
    SqlDataSource ds_templates;
    ASPxDropDownEdit dde_filter;
    Panel panel_export;

    protected void Page_Init(object sender, EventArgs e)
    {
        _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(_page_id);

        if (Session["working_business_unit_id"] != null)
        {
            Session.Remove("working_business_unit_id");
        }
        _tools.dont_cache_page();
        if (Cache["ds_depend"] == null)
        {
            Cache["ds_depend"] = DateTime.Now;
        }
        layout.__page_name = _page_name;
        layout.used_gv = ASPxGridView1;
        h = (ASPxHiddenField)layout.FindControl("h");
        ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
        dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
        panel_export = (Panel)layout.FindControl("panel_export");
        panel_export.Visible = true;

        h.Set("gridview_id", "ASPxGridView1");
        ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
        ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
        _tools.dont_cache_page();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
        var temp_contact = new NEContact(Convert.ToInt32(myMember.ContactID));
        _q = Request.QueryString;
        var lbltemp = (Label)Page.Master.FindControl("lblHeading");
        if (menu.PageDescription != null)
        {
            lbltemp.Text = menu.PageDescription;
        }
        if (!IsPostBack)
        {
            var gl = new NeGridLayouts(myMember.id, _page_name);
            if (gl.GridLayoutID == 0)
            {
                gl.GridLayout_Layout = "";
                gl.member_id = myMember.id;
                gl.GridLayout_Name = "Default";
                gl.GridLayout_Gridid = _page_name;
                gl.SaveGridLayout();

                h.Set("ID", gl.GridLayoutID);
                h.Set("NAME", gl.GridLayout_Name);
            }
            else
            {
                ASPxGridView1.LoadClientLayout(gl.GridLayout_Layout);
                h.Set("ID", gl.GridLayoutID);
                h.Set("NAME", gl.GridLayout_Name);
            }
            dde_filter.Text = gl.GridLayout_Name;
            Session["customer_survey_grid"] = null;
        }
        fill_grid();

    }
    protected void fill_grid()
    {
        if (Session["customer_survey_grid"] == null)
        {
            DataTable dt;
            if (myMember.isContact)
            {
                dt = _tools.getSQL_datatable(@"SELECT wo_survey.id id, wo_survey.contact_id, wo_survey.date date, woprog.WOProg_BVWO bvwo, 
woprog.woprog_description, woprog.woprog_customername cust_name, contact.Contact_Name name, wo_survey.rating rating, wo_survey.notes, 
if(wo_survey.clean=1,'Yes','No') clean, if(wo_survey.ontime=1,'Yes','No') ontime, if(wo_survey.callme=1,'Yes','No') callme ,woprog.WOProg_ID,woprog.business_unit_id, woprog.woprog_customer_id customer_id
FROM wo_survey 
INNER JOIN woprog ON wo_survey.woprog_id = woprog.WOProg_ID 
INNER JOIN contact ON wo_survey.contact_id = contact.Contact_ID 
where wo_survey.contact_id = " + myMember.ContactID, null);
            }
            else
            {
                dt = _tools.getSQL_datatable(
                    @"SELECT wo_survey.id id, wo_survey.contact_id, wo_survey.date date, woprog.WOProg_BVWO bvwo, 
woprog.woprog_description, woprog.woprog_customername cust_name, contact.Contact_Name name, wo_survey.rating rating, wo_survey.notes, 
if(wo_survey.clean=1,'Yes','No') clean, if(wo_survey.ontime=1,'Yes','No') ontime, if(wo_survey.callme=1,'Yes','No') callme ,woprog.WOProg_ID,woprog.business_unit_id, woprog.woprog_customer_id customer_id
FROM wo_survey 
INNER JOIN woprog ON wo_survey.woprog_id = woprog.WOProg_ID 
INNER JOIN contact ON wo_survey.contact_id = contact.Contact_ID where woprog.business_unit_id in(" +
                    new Current_User().visible_business_units + ")", null);
            }


            Session["customer_survey_grid"] = dt;
        }
        ASPxGridView1.DataSource = Session["customer_survey_grid"];
        ASPxGridView1.DataBind();
    }
    protected int GetRatingValue(object value)
    {
        return value != DBNull.Value ? Convert.ToInt32(value) : 0;
    }

    protected void ASPxGridView1_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        var gv = (ASPxGridView)sender;
        e.Properties["cpExp"] = gv.SaveClientLayout();
    }
    protected void ASPxGridView1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
