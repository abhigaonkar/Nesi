using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Script.Serialization;
using nesi.core;

public partial class Emailed_Account_Statements_index : Page
	{


	private NeMember myMember;
	private const int _page_id = 199; // from Page table in DB
	private const string _page_name = "emailed_account_statements";
	Toolbox _tools;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	static string default_filter = "";
	NameValueCollection _q;

	protected void Page_Init(object sender, EventArgs e)
		{
		_q = Request.QueryString;
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		layout.__page_name = _page_name;
		layout.used_gv = gv;
		if (Session["working_business_unit_id"] != null)
			{
			Session.Remove("working_business_unit_id");
			}
	//	default_filter = string.Format("page1|sort1|a3|filter[Status] Not Like '%Invoiced%' And [PM] = '{0}' And [Branch] = '{1}'  |conditions3|8|8|14|4|15|5|visible30|t0|t16|t15|t1|t2|t3|t11|t13|t4|t5|t7|t17|t18|t6|t10|t22|t12|t19|t14|t20|t9|t21|t23|t8|t24|t25|t26|t27|t28|t29|width25|32px|59px|59px|72px|162px|259px|59px|59px|78px|69px|59px|59px|59px|59px|91px|59px|59px|59px|59px|59px|174px|59px|59px|59px|50px|25px|25px|25px|25px|25px", myMember.FirstName, myMember.business_unit_name);
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;

		h.Set("gridview_id", "gv");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
		_tools.dont_cache_page();
		if (Cache["ds_depend"] == null)
			{
			Cache["ds_depend"] = DateTime.Now;
			}
	
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var _q = Request.QueryString;
		
	

		if (!IsPostBack && !IsCallback)
			{

			}
		

		if (!IsCallback && !IsPostBack)
			{

			var gl = new NeGridLayouts(myMember.id, _page_name);
			
			if ((gl.GridLayoutID != 0 )&&(_q["customer_id"] == null))
				{
				gv.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				dde_filter.Text = gl.GridLayout_Name;
				}
			
			else if (_q["cust_id"] == null)
				{
				gv.LoadClientLayout(default_filter);
				gl.GridLayout_Layout = default_filter;
				gl.member_id = myMember.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				dde_filter.Text = gl.GridLayout_Name;
				}
			
			}

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




}
