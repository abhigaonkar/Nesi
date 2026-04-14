using System;
using System.Data;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_workorder_MasterScanList : System.Web.UI.Page
	{
	decimal totalquoted;
	NeMember myMember;
	private const int _page_id = 94; // from Page table in DB
	private const string _page_name = "MasterScanList";
	Toolbox _tools = new Toolbox();
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;

	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		layout.__gv_id = "gv_scanreport";
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		layout.used_gv = gv_scanreport;
		h.Set("gridview_id", "gv_scanreport");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
		if (!IsPostBack)
		{
			var gl = new NeGridLayouts(myMember.id, _page_name);
			if (gl.GridLayoutID == 0)
			{

				gv_scanreport.LoadClientLayout(gl.GridLayout_Layout);
				gl.member_id = myMember.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_scanreport.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
			

		}

	}
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
	
		_tools.dont_cache_page();

		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		if (!IsPostBack)
			{
			}
		gv_scanreport.DataBind();
		}
	protected void Button1_Click(object sender, EventArgs e)
		{
		gv_scanreport.CancelEdit();
		}
	protected void gv_scanreport_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
		{
		if (e.IsGroupSummary)
			{
			if (e.Item.FieldName == "QuotedAmount")
				{
				totalquoted = Convert.ToDecimal(e.Value.ToString());
				var newText = string.Format("Quoted: {0:C}", Convert.ToDecimal(e.Value.ToString()));
				e.Text = newText;
				}

			}
		}


	protected void gv_scanreport_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{
		var dr						= gv_scanreport.GetDataRow(e.VisibleIndex);
		var bh		= new shared.BusinessHours(new TimeSpan(8,0,0), new TimeSpan(17,0,0), new DayOfWeek[] {DayOfWeek.Saturday, DayOfWeek.Sunday});
		if(dr != null)
			{
			var woprog_id			= dr["WOID"];
			var status			= dr["Status"].ToString();
			if(!status.Contains("Open PO") && !status.Contains("Waiting For PO"))
				{
				var c_statuses			= Toolbox.doSQL_int(@"SELECT COUNT(woprogstatus_datetime) FROM woprogstatus WHERE woprogstatus_woprog_id = @v0  AND woprogstatus_status = @v1  ", new object[] {  woprog_id, status } );
				if(c_statuses > 0)
					{
					var status_date	= Toolbox.doSQL_datetime(@"SELECT MAX(woprogstatus_datetime) FROM woprogstatus WHERE woprogstatus_woprog_id = @v0  AND woprogstatus_status = @v1  GROUP BY woprogstatus_woprog_id", new object[] {  woprog_id, status } );
					var diff				= bh.Calculate(status_date, DateTime.Now);
					if(diff >= 3)
						{
						e.Row.ForeColor	= System.Drawing.ColorTranslator.FromHtml("#ff0000");
						}
					e.Row.CssClass						+= " opt1";
					e.Row.Attributes["data-tooltip"]	= string.Format("This work order has been in '{0}' for {1} hour(s)", status, Math.Round(diff, 2));
					e.Row.Attributes["data-title"]		= "Alert!";
					}
				}
			}
		}
	protected void gv_scanreport_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
	
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_scanreport_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if(e.Parameters != "" && e.Parameters != "new" && (e.Parameters.Length > 5 && e.Parameters.Substring(0, 4) == "page"))
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
			Session["master_customer_gv"]		= null;
			gv_scanreport.DataBind();
			}
	}
}
