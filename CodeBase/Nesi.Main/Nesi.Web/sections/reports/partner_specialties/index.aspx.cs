using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_reports_partner_specialties_index : Page
    {
    private const int _page_id = 196; // from Page table in DB
    private const string _page_name = "Partner Specialties";
    private NameValueCollection _q;
    private Toolbox _tools = new Toolbox();
    private bool can_edit_reps;
    private bool can_view_dollar_totals;
    private NeMember current_user;
    private ASPxDropDownEdit dde_filter;
    public int ddl_selected;
    private SqlDataSource ds_templates;
    private ASPxHiddenField h;
    private Panel panel_export;

    protected void Page_Init(object sender, EventArgs e)
        {
        _tools = new Toolbox();
        //	myMember = Toolbox.do_handle_authentication(_page_id);
        current_user = Toolbox.do_handle_authentication(196);
        _q = Request.QueryString;
        layout.__gv_id = "gv";
        layout.__page_name = _page_name;
        h = (ASPxHiddenField) layout.FindControl("h");
        ds_templates = (SqlDataSource) layout.FindControl("ds_templates");
        dde_filter = (ASPxDropDownEdit) layout.FindControl("dde_filter");
        panel_export = (Panel) layout.FindControl("panel_export");
        panel_export.Visible = true;
        layout.used_gv = gv;
        can_view_dollar_totals = current_user.AuthenticatedForPrivilege(60);
        can_edit_reps = current_user.AuthenticatedForPrivilege(157);
        h.Set("gridview_id", "gv");
        ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
        ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
        var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
        var lbltemp = (Label) Page.Master.FindControl("lblHeading");
        lbltemp.Text = "Partner Specialties";
        if (!IsPostBack)
            {
            var gl = new NeGridLayouts(current_user.id, _page_name);
            if (gl.GridLayoutID == 0)
                {
                gl.GridLayout_Layout = "";
                gv.LoadClientLayout(gl.GridLayout_Layout);
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
        }

    protected void Page_Load(object sender, EventArgs e)
        {
        var sql = @"SELECT
partner_skills.id,
partner_skills.`table` Type,
partner_skills.skill Skill,
date(partner_skills.date_added) `Date Added`,
member.member_fullname `Added By`,
business_unit.ddl_name business_unit,
ifnull(customer.customer_name,
vendor.Vendor_Name) AS Name,
ifnull(customer.is_partner,vendor.is_partner) is_partner,
ifnull(Concat(address.Address_PhoneArea,' ',address.Address_PhoneFirst,' ', address.Address_PhoneLast) ,
Concat(address1.Address_PhoneArea,' ',address1.Address_PhoneFirst,' ', address1.Address_PhoneLast)) Phone

FROM
partner_skills
INNER JOIN member ON partner_skills.added_by = member.Member_ID
INNER join business_unit ON member.business_unit_id = business_unit.id
LEFT JOIN customer ON partner_skills.table_id = customer.customer_id AND partner_skills.`table` = 'Customer'
LEFT JOIN vendor ON partner_skills.table_id = vendor.Vendor_ID AND partner_skills.`table` = 'Vendor'
LEFT JOIN address on address.Address_Table_ID = customer.customer_id and address.address_table = 'Customer' and address.Address_Type = 'B'
LEFT JOIN address address1 on address1.Address_Table_ID = vendor.Vendor_ID and address1.address_table = 'Vendor' and address1.Address_Type = 'B'";
        gv.DataSource = Toolbox.doSQL_dt(sql,null);
        gv.DataBind();
        }

    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
                if (gv.Columns[i] is GridViewDataColumn)
                    {
                    var col = (GridViewDataColumn) gv.Columns[i];
                    if (col.GroupIndex > -1)
                        gv.UnGroup(col);
                    col.Visible = true;
                    }
            }
        }

    protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
        var gv = (ASPxGridView) sender;
        e.Properties["cpExp"] = gv.SaveClientLayout();
        }
    }