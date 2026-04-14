using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_member_skills_responsibility_link : System.Web.UI.Page
{
	NeMember current_user;
	private const int page_id = 159; // from Page table in DB
	private const string _page_name = "skillscr";
	Toolbox _tools;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	private bool can_edit = false;
	protected void Page_Init()
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		can_edit = current_user.AuthenticatedForPrivilege(155);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));
		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		layout.__page_name = _page_name;
		layout.used_gv = gvcore;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;

		gvcore.DataBind();
		h.Set("gridview_id", "gvcore");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var _q = Request.QueryString;
		
		if (!IsPostBack)
		{
			if (!IsPostBack)
			{
				var gl = new NeGridLayouts(current_user.id, _page_name);
				if (gl.GridLayoutID == 0)
				{
					gl.GridLayout_Layout = gvcore.SaveClientLayout();
					gl.member_id = current_user.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();

					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				else
				{
					gvcore.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				dde_filter.Text = gl.GridLayout_Name;
				
			}
			Session["gvcoreskills"] = null;
		}

fillgrid();
	}

	protected void fillgrid()
	{
		if (Session["gvcoreskills"] == null)
		{
			Session["gvcoreskills"] = _tools.getSQL_datatable(@"SELECT cr_skills_link.cr_skills_priority_id, core_responsibilities.core_responsibility AS cr, core_responsibilities.description AS description, core_responsibilities.`status` AS `status`, master_skills.`name`, master_skills.description AS sd, master_skills.`status` AS skill_status, master_skills.skilltype AS st, master_skills.can_be_trained AS cbt, cr_group.`name` AS gn FROM cr_skills_link RIGHT JOIN core_responsibilities ON core_responsibilities.id = cr_skills_link.cr_id LEFT JOIN master_skills ON cr_skills_link.skills_id = master_skills.id INNER JOIN cr_group ON core_responsibilities.cr_group_id = cr_group.id"  , null);

		}
		gvcore.DataSource = Session["gvcoreskills"];
		gvcore.DataBind();
	}

	protected void gvcore_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;

		if (e.Parameters[0].ToString() == "x")
		{
			var search = e.Parameters.Split('|').GetValue(1).ToString();
			if (search == "")
			{
				Session["gvcoreskills"] = null;
				fillgrid();
			}
			else
			{
				Session["gvcoreskills"] = _tools.getSQL_datatable(@"SELECT
cr_skills_link.cr_skills_priority_id,
core_responsibilities.core_responsibility AS cr,
core_responsibilities.description AS description,
core_responsibilities.`status` AS `status`,
master_skills.`name`,
master_skills.description AS sd,
master_skills.`status` AS skill_status,
master_skills.skilltype AS st,
master_skills.can_be_trained AS cbt,
cr_group.`name` AS gn
FROM
cr_skills_link
RIGHT JOIN core_responsibilities ON core_responsibilities.id = cr_skills_link.cr_id
LEFT JOIN master_skills ON cr_skills_link.skills_id = master_skills.id
INNER JOIN cr_group ON core_responsibilities.cr_group_id = cr_group.id 
where core_responsibilities.core_responsibility like '%" + search + "%' or master_skills.`name` like '%" 
+ search + "%' or core_responsibilities.description like '%" + search + "%' or master_skills.description like '%" + search + "%'", null);
				fillgrid();
			}

		}
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
	protected void gvcore_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
}
	

	