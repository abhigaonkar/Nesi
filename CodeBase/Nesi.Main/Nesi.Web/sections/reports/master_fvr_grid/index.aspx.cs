using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class master_fvr_grid : System.Web.UI.Page
	{
	private NeMember myMember;
	private const int _page_id		= 188; // from Page table in DB
    private const string _page_name = "master_fvr_grid";
    static string default_filter = "";
	Toolbox _tools;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	
    protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(_page_id);
		layout.__page_name					= _page_name;
		h									= (ASPxHiddenField) layout.FindControl("h");
		layout.used_gv			= gv;
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		h.Set("gridview_id", "gv");
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
				Session["master_fvr_grid"] = null;
			var gl	= new NeGridLayouts(myMember.id, _page_name);
			if(gl.GridLayoutID == 0)
				{
					//default_filter = "page1|filter[Branch] = '" + myMember.business_unit_name + "'";
				gv.LoadClientLayout(default_filter);
//				gl.GridLayout_Layout = "page1|filter[Branch] = '" + myMember.business_unit_name + "'";
				gl.member_id	= myMember.id;
				gl.GridLayout_Name			= "Default";
				gl.GridLayout_Gridid		= _page_name;
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
			dde_filter.Text					= gl.GridLayout_Name;
			}
		fill_grid();
		}

	protected void fill_grid()
	{
		if (Session["master_fvr_grid"] == null)
		{
			Session["master_fvr_grid"] = _tools.getSQL_datatable(@"SELECT 
	a.id ID,
	IF(b.active = 1, 'True', 'False') Active,
	b.dt_insert `Date`,
	a.type `Type`,
	d.ddl_name business_unit, 
	k.member_fullname Cut_By,
	c.member_fullname Employee, 
	CONCAT(g.name,'.',g.ext) filename,
	IF(IFNULL(e.confirmed, 0) = 1, 'True', 'False') Confirmed,
	IF(IFNULL(b.upload_required, 0) = 1, 'True', 'False') Upload_Required,
	CONCAT(h.name,'.',h.ext) Uploaded_Filename,
	i.status HR_Status,
	j.name FVR_Status
FROM 
	member_fvr_dtl b 
LEFT JOIN	member c
	ON b.member_id = c.member_id
LEFT JOIN member_fvr_hdr a 
	ON a.id = b.member_fvr_hdr_id  
LEFT join business_unit d 
	ON c.business_unit_id = d.id 
LEFT JOIN member_fvr_history e 
	ON b.id = e.member_fvr_dtl_id 
LEFT JOIN member_fvr_tab f
	ON b.tab_index = f.id and a.type = f.type
LEFT JOIN filestore.files g 
	ON b.file_id = g.id
LEFT JOIN filestore.files h 
	ON b.uploaded_file_id = h.id
LEFT JOIN member_hrstatus i
	on c.member_hrstatus_id = i.id
LEFT JOIN member_fvr_status j
	ON b.status_id = j.id
LEFT JOIN member k
	ON a.req_member_id = k.member_id 
WHERE 
	c.member_status = 'Active' and
	(is_supervisor(c.member_id," + myMember.id + @") or (" + myMember.business_unit_id + @"=11 ))
GROUP BY 
	b.id 
ORDER BY
	a.ts DESC,
	d.id,
	b.member_id,
	b.tab_index",null);

		}

		gv.DataSource = Session["master_fvr_grid"];
		gv.DataBind();

	}

	
    protected void gv_er_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
    protected void gv_er_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
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
   
}
