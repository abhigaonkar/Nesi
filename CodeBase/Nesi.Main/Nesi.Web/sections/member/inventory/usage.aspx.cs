using System;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_member_inventory_usage : System.Web.UI.Page
	{
	public NeMember myMember;
	JavaScriptSerializer jSON				= new JavaScriptSerializer();
	private const int _page_id			= 1; // from Page table in DB
    public string selected_master;
    private const string _page_name			= "BranchInventoryUsage";
	protected bool is_purchaser				= false;
	NeBusinessUnit this_company;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	Toolbox _tools;
	
    protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(_page_id);
		var _q				= Request.QueryString;
		var type							= _q["type"] ?? "";
		NeGridLayouts gl;
		layout.__page_name					= _page_name;
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		gv_PO.Visible						= (type == "po");
		gv_WO.Visible						= (type == "wo");
		chk_wos.Visible						= gv_WO.Visible;
		this_company						= new NeBusinessUnit(_q["business_unit_id"]);
		var gv_id						= gv_PO.Visible ? "gv_PO" : "gv_WO";
		layout.used_gv						= gv_PO.Visible ? gv_PO : gv_WO;
		if(Session["working_business_unit_id"] == null)
			{
			Session.Add("working_business_unit_id", this_company.id.ToString());
			}
		h.Set("gridview_id", gv_id);
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name+"_"+gv_id+"_"+this_company.DSN;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= myMember.id.ToString();
		gl									= new NeGridLayouts(myMember.id, _page_name+"_"+gv_id+"_"+this_company.DSN);
		
		if(!IsPostBack)
			{
			if(gl.GridLayout_Layout != "")
				{
				if(gv_PO.Visible)
					{
					gv_PO.LoadClientLayout(gl.GridLayout_Layout);
					}
				else
					{
					gv_WO.LoadClientLayout(gl.GridLayout_Layout);
					}
				}
			else
				{
				gl							= new NeGridLayouts();
				gl.GridLayout_Layout		= gv_PO.Visible ? gv_PO.SaveClientLayout() : gv_WO.SaveClientLayout();
				gl.member_id	= myMember.id;
				gl.GridLayout_Name			= "Default";
				gl.GridLayout_Gridid		= _page_name+"_"+gv_id+"_"+this_company.DSN;
				gl.SaveGridLayout();
				}
			h.Set("ID", gl.GridLayoutID);
			h.Set("NAME", gl.GridLayout_Name);
			dde_filter.Text					= gl.GridLayout_Name;
			panel_export.Visible			= true;
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		if(!IsPostBack)
			{
			chk_wos.Checked			= true;
			}
		}
	protected void Load_Layout(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void ds_WO_Init(object sender, EventArgs e)
		{
		var ds		= (SqlDataSource) sender;
		var which			= "current";
		if(h.Contains("historicWO"))
			{
			which				= h.Get("historicWO").ToString() == "True" ? "current" : "history";
			}
		var sql			= string.Format(@"
SELECT 
	customer_name(b.woprog_customer_id) name, 
	DATE_FORMAT(b.WOProg_CutDateTime , '%m/%d/%Y') date_cut,
	a.wo_detail_{0}_qty_committed qty_com, 
	a.wo_detail_{0}_qty_ordered qty_req,
	a.wo_detail_{0}_price_sell price, 
	b.woprog_bvwo wo_n, 
	b.woprog_id, 
	b.business_unit_id,
	b.woprog_customer_id customer_id
FROM wo_detail_{0} a 
LEFT JOIN 
	woprog b ON a.wo_detail_{0}_woprog_id = b.woprog_id 
WHERE 
	a.wo_detail_{0}_master_id = @master_id AND 
	b.business_unit_id = @business_unit_id
ORDER BY wo_n DESC", which);
		ds.SelectCommand	= sql;
		gv_WO.DataBind();
		}
}
