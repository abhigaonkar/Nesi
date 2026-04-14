using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_offer_gridview : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 169;
	static string _page_name = "Employement_Agreement_Gridviews";
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
		lc.used_gv = gv_mo;
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
			Session["offer_gridview"] = null;
			var gl = new NeGridLayouts(current_user.id, _page_name);
			h.Set("gridview_id", "gv_mo");
			if (gl.GridLayoutID == 0)
			{
				gv_mo.FilterExpression = string.Format("");
				gl.GridLayoutID = 0;
				gl.GridLayout_Layout = gv_mo.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				gl = new NeGridLayouts(current_user.id, _page_name);
				gv_mo.FilterExpression = string.Format("[mo_days] < 14 And [mo_status] <> 'Accepted'");
				gl.GridLayoutID = 0;
				gl.GridLayout_Layout = gv_mo.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Employment Agreements Due within 2 weeks";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				gl = new NeGridLayouts(current_user.id, _page_name);
				gv_mo.FilterExpression = string.Format("[review_days] < 14 And [r_status] <> 'Not Set'");
				gl.GridLayout_Layout = gv_mo.SaveClientLayout();
				gl.GridLayoutID = 0;
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Reviews Due within 2 weeks";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_mo.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;


		}
		fill_grid();

	}

	protected void fill_grid()
	{
		if (Session["offer_gridview"] == null)
		{
			Session["offer_gridview"] = _tools.getSQL_datatable(@"SELECT
member.Member_ID,
member.member_fullname,
get_name(member.reports_to) reports_to,
business_unit.ddl_name business_unit,
datediff(ifnull(member_offers.enddate,member.member_startdate),curdate()) AS mo_days,
member_offers.id AS active_offer_id,
ifnull((Select ifnull(id,'No Agreement') from member_offers mo where mo.memberid = member.member_id order by mo.id desc limit 1),'No Agreement') AS mo_id,
ifnull((Select ifnull(status,'No Agreement') from member_offers mo where mo.memberid = member.member_id order by mo.id desc limit 1),'No Agreement') AS mo_status,
ifnull((Select emp_review.date from emp_review where emp_review.member_id = member.member_id and emp_review.status !='Delivered' order by emp_review.id desc limit 1),'Not Set') nr_date,
ifnull((Select emp_review.`status` from emp_review where emp_review.member_id = member.member_id and emp_review.status !='Delivered' order by emp_review.id desc limit 1),'Not Set') r_status,
(Select datediff(emp_review.date,curdate()) from emp_review where emp_review.member_id = member.member_id and emp_review.status !='Delivered' order by emp_review.id desc limit 1) review_days,
ifnull((Select emp_review.id from emp_review where emp_review.member_id = member.member_id and emp_review.status !='Delivered' order by emp_review.id desc limit 1),'Not Set') r_id,
member_hrstatus.status hr_status,
ifnull((Select ifnull(reports_to,0) from member_offers mox where mox.memberid = member.member_id order by mox.id desc limit 1),0) mo_reports_to
FROM
member 
inner join business_unit on member.business_unit_id = business_unit.id
inner join member_hrstatus on member.member_hrstatus_id = member_hrstatus.id
left join member_offers on member.Member_ID = member_offers.memberid and member_offers.status = 'Accepted' 
where 
member_status='Active' 
and business_unit.id in ("+ new Current_User().visible_business_units +@")
and (is_supervisor(member.member_id," + current_user.id + @") 
or ifnull((Select ifnull(reports_to,0) from member_offers mox where mox.memberid = member.member_id order by mox.id desc limit 1),0) = " + current_user.id + @"

or (is_owner(" + current_user.id + @",business_unit.tax_entity_id))
            or (member.member_id = " + current_user.id + @") 
            or (" + current_user.AuthenticatedForPrivilege(155) + ")) order by member.member_fullname",null);
			
		}
		gv_mo.DataSource = Session["offer_gridview"];
		gv_mo.DataBind();

	}


	protected void gv_mo_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	protected void gv_mo_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
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
			var moid = Convert.ToInt32(gv_mo.GetRowValues(gv_mo.FindVisibleIndexByKeyValue(container.KeyValue), "active_offer_id"));
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
			var moid = Convert.ToInt32(gv_mo.GetRowValues(gv_mo.FindVisibleIndexByKeyValue(container.KeyValue), "r_id"));
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
                var mo_id = gv_mo.GetRowValuesByKeyValue(c.KeyValue, "mo_id").ToString();
                var mo_reports_to = gv_mo.GetRowValuesByKeyValue(c.KeyValue, "mo_reports_to").ToString();
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
}

	