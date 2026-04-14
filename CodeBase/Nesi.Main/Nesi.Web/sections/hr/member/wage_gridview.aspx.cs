using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_wage_gridview : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 226;
	static string _page_name = "Wage_Gridview";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	Toolbox _tools;
    bool auth_for_edit_all;
    bool auth_for_edit;

	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		lc.__page_name = _page_name;
		lc.used_gv = gv;
		ds_templates = (SqlDataSource)lc.FindControl("ds_templates");
		
		h = (ASPxHiddenField)lc.FindControl("h");
		dde_filter = (ASPxDropDownEdit)lc.FindControl("dde_filter");
		panel_export = (Panel)lc.FindControl("panel_export");
		panel_export.Visible = true;
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		
		var _q = Request.QueryString;
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));
		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
        auth_for_edit_all = current_user.AuthenticatedForPrivilege(6);
        auth_for_edit = current_user.AuthenticatedForPrivilege(32);
		if (!IsPostBack)
		{
			Session["wage_gridview"] = null;
			var gl = new NeGridLayouts(current_user.id, _page_name);
			h.Set("gridview_id", "gv");
			if (gl.GridLayoutID == 0)
			{
				gv.FilterExpression = string.Format("[member_termdate]>'" + System.DateTime.Today.AddDays(-365).ToString("yyyy-MM-dd") + "'");
				gl.GridLayoutID = 0;
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
		fill_grid();

	}

	protected void fill_grid()
	{
		if (Session["wage_gridview"] == null)
		{
			Session["wage_gridview"] = _tools.getSQL_datatable(@"Call get_wage_gridview_data(@v0)",new object[] { current_user.id32 });
			
		}
		gv.DataSource = Session["wage_gridview"];
		gv.DataBind();

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
	protected void ASPxHyperLink1_Init(object sender, EventArgs e)
	{
		var hl = (ASPxHyperLink)sender;
		var container = hl.NamingContainer as GridViewDataItemTemplateContainer;
		hl.ClientSideEvents.Click = string.Format("function (s, e) {{ boing('index.aspx?id={0}','employee',1200,900)}}", container.KeyValue);
	}
	protected void ASPxHyperLink2_Init(object sender, EventArgs e)
	{
		var hl = (ASPxHyperLink)sender;
		var container = hl.NamingContainer as GridViewDataItemTemplateContainer;
		try
		{
			var moid = Convert.ToInt32(gv.GetRowValues(gv.FindVisibleIndexByKeyValue(container.KeyValue), "active_offer_id"));
			hl.ClientSideEvents.Click = string.Format("function (s, e) {{ boing('member_offer.aspx?id={0}','employee_offer',1200,900)}}", moid);
		}
		catch { }
	}
	protected void ASPxHyperLink3_Init(object sender, EventArgs e)
	{

	var hl = (ASPxHyperLink)sender;
		var container = hl.NamingContainer as GridViewDataItemTemplateContainer;
		try
		{
			var moid = Convert.ToInt32(gv.GetRowValues(gv.FindVisibleIndexByKeyValue(container.KeyValue), "r_id"));
			hl.ClientSideEvents.Click = string.Format("function (s, e) {{ boing('review_list_for_member.aspx?memberid={0}&id={1}','employee',1200,900)}}", container.KeyValue,moid);
		}
		catch { }
		
		}
	protected void ASPxHyperLink4_Init(object sender, EventArgs e)
	{
        var hl = (ASPxHyperLink)sender;
        if (hl.Text.Equals("0"))
        {
            hl.NavigateUrl = "";
            hl.Text = "";
        }
        else if ((!auth_for_edit_all) && (!auth_for_edit))
        {
            if (hl.Text == "In Development")
            {
                var c = (GridViewDataItemTemplateContainer)hl.NamingContainer;
                var mo_id = gv.GetRowValuesByKeyValue(c.KeyValue, "mo_id").ToString();
                var mo_reports_to = gv.GetRowValuesByKeyValue(c.KeyValue, "mo_reports_to").ToString();
                if (mo_reports_to == current_user.id.ToString())
                {

                }
                else
                {
                    hl.NavigateUrl = "";
                    hl.Text = "";
                }
            }
            else
            {

                hl.NavigateUrl = "";
                hl.Text = "";
            }
        }
	}





    protected void gv_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        if (e.ButtonID == "print_er")
        {
            gv.JSProperties["cp_redirect1"] = gv.GetRowValues(e.VisibleIndex, "Member_ID");
        }
        else if (e.ButtonID == "print_ext")
        {
            gv.JSProperties["cp_redirect2"] = gv.GetRowValues(e.VisibleIndex, "Member_ID");
        }

    }
}

	