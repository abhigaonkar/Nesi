using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class master_cr : Page
{
	private NeMember myMember;
	private const int _page_id = 150; // from Page table in DB
	private const string _page_name = "MasterResponsibilities";
	static string default_filter = "page1|visible7|f6|t0|t1|t2|t3|t4|t5|width7|e|81px|694px|130px|146px|150px|150px";
	Toolbox _tools;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;



	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		layout.used_gv = gv_cr;
		h.Set("gridview_id", "gv_cr");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Master Responsibilties Report";
		if (!IsPostBack)
		{
			var gl = new NeGridLayouts(myMember.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				default_filter = "page1|visible7|f6|t0|t1|t2|t3|t4|t5|width7|e|81px|694px|130px|146px|150px|150px";

				gv_cr.LoadClientLayout(default_filter);
				gl.GridLayout_Layout = "page1|visible7|f6|t0|t1|t2|t3|t4|t5|width7|e|81px|694px|130px|146px|150px|150px";
				gl.member_id = myMember.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_cr.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
			Session["master_cr"] = null;



		}
		fill_grid();
	}




	protected void gv_cr_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_cr_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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




	protected void gv_cr_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{

	}
	protected void fill_grid()
	{
		if (Session["master_cr"] == null)
		{
			Session["master_cr"] = _tools.getSQL_datatable(string.Format(@"SELECT
membertype_responsibilities.core_responsibility_id crid,
business_unit.ddl_name name,
core_responsibilities.core_responsibility,
membertype.membertype_name title,
get_name(member.Member_id) should_be,
ifnull((select membertype.membertype_name from memberoffer_cr, member_offers, membertype where member_offers.membertypeid = membertype.membertype_id and member_offers.`status`= 'Accepted' and member_offers.isapplicant = 0 and memberoffer_cr.memberoffer_moid = member_offers.id and member_offers.id = member.Member_ID and memberoffer_cr.memberoffer_crid = core_responsibilities.id),null) AS offered_title,
ifnull((select get_name(member_offers.id) from memberoffer_cr, member_offers where member_offers.`status`= 'Accepted' and member_offers.isapplicant = 0 and memberoffer_cr.memberoffer_moid = member_offers.id and member_offers.id = member.Member_ID and memberoffer_cr.memberoffer_crid = core_responsibilities.id),null) AS offered_cr
FROM
core_responsibilities
LEFT JOIN membertype_responsibilities ON core_responsibilities.id = membertype_responsibilities.core_responsibility_id
INNER JOIN membertype ON membertype_responsibilities.membertype_id = membertype.membertype_id
INNER JOIN member ON member.Member_MemberType_ID = membertype.membertype_id and member.Member_Status = 'Active'
INNER join business_unit ON member.business_unit_id = business_unit.id
WHERE
core_responsibilities.`status` = 'Active' AND
business_unit.id in ({0})
union 

SELECT
core_responsibilities.id crid,
business_unit.ddl_name name,
core_responsibilities.core_responsibility,
null title,
null should_be,
membertype.membertype_name as offered_title,
get_name(member.Member_ID) as offered_cr
FROM
memberoffer_cr
INNER JOIN member_offers ON memberoffer_cr.memberoffer_moid = member_offers.id
INNER JOIN core_responsibilities ON memberoffer_cr.memberoffer_crid = core_responsibilities.id
INNER JOIN member on member_offers.id = member.Member_ID and member.Member_Status = 'Active'
inner join business_unit on member.business_unit_id = business_unit.id 
inner join membertype on member_offers.membertypeid = membertype.membertype_id
WHERE
member_offers.isapplicant = 0 AND
member_offers.`status` = 'Accepted' AND
memberoffer_cr.memberoffer_crid NOT IN ((
SELECT GROUP_CONCAT(DISTINCT membertype_responsibilities.core_responsibility_id) stuff 
FROM
          membertype_responsibilities 
where membertype_responsibilities.membertype_id = member.Member_MemberType_ID 
group by membertype_responsibilities.membertype_id
)) AND
business_unit.id in ({0})

ORDER BY
crid ASC,name", new Current_User().visible_business_units), null);


		}
		gv_cr.DataSource = Session["master_cr"];
		gv_cr.DataBind();

	}

	protected void cb_chkprivate_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{

	}
}
