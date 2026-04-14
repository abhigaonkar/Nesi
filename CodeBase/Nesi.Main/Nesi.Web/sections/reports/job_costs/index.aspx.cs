using System;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI;
using System.Text.RegularExpressions;
using DevExpress.Web;
using nesi.core;

public partial class sections_reports_job_costs_index : Page
	{
	public NeMember current_user;
	public double dblWoTotal = 0.00;
	public int intWoTotal = 0;
	public string DSN;

	private const int _page_id = 59; // from Page table in DB
	private const string _page_description = "Job Cost Report";
	static string _page_name = "JobCostReport";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;

	protected void Page_Init(object sender, EventArgs e)
		{
		current_user = Toolbox.do_handle_authentication(59);
		var cList2 = new NeBusinessUnit();
		    ddlCompany.DataSource = Toolbox.doSQL_dt(string.Format(@"select id, ddl_name from business_unit  where id in ({0}) ", new Current_User().visible_business_units ),null);
		ddlCompany.DataBind();
		if (!IsCallback)
			{
			ddlCompany.Value = current_user.business_unit_id.ToString();
			}
		layout.__page_name = _page_name;
		layout.used_gv = gv_jobcost;
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		h = (ASPxHiddenField)layout.FindControl("h");
		h.Set("gridview_id", "gv_jobcost");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		if (!IsPostBack)
			{
			if (current_user.AuthenticatedForPrivilege(50))
				{
				ddlCompany.Enabled = true;
				}
			}
		if (!IsCallback)
			{
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
				{
				gv_jobcost.FilterExpression = "";
				gl.GridLayout_Layout = gv_jobcost.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_jobcost.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text = gl.GridLayout_Name;
			}
		}
	protected void Button1_Click(object sender, EventArgs e)
		{
		}
	protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
		{
		//lblCurrentView.Text = "";
		}
	protected void gv_jobcost_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		}
	protected void gv_jobcost_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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

	protected void gv_jobcost_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
		// General catches
		var r = new Regex("[A-Za-z]+");
		double try_num = 0;
		double.TryParse(e.CellValue.ToString(), out try_num);
		var try_num_ext = try_num * 100;
		var woprog_id = 0;
		if (try_num < 0)
			{
			e.Cell.BackColor = Color.Red;
			e.Cell.ForeColor = Color.White;
			}
		else if (try_num == 0 && !r.Match(Toolbox.ReturnBlankIfNull_string(e.CellValue).ToString()).Success)
			{
			e.Cell.ForeColor = Color.LightGray;
			}
		else
			{
			switch (e.DataColumn.FieldName)
				{
				case "actual_labor_hours":
					var quoted_labor_hours = Convert.ToDouble(gv_jobcost.GetDataRow(e.VisibleIndex)["quoted_labor_hours"]);
					if (try_num > quoted_labor_hours)
						{
						e.Cell.Font.Bold = true;
						e.Cell.ForeColor = Color.Red;
						}
				break;
				case "material_actual_price":
					var quoted_material = Convert.ToDouble(gv_jobcost.GetDataRow(e.VisibleIndex)["material_quote_price"]);
					if (try_num > quoted_material)
						{
						e.Cell.Font.Bold = true;
						e.Cell.ForeColor = Color.Red;
						}
				break;
				case "quoted_amount_used":
				case "labor_complete":
				case "material_complete":
					if (try_num_ext > 100)
						{
						e.Cell.Font.Bold = true;
						e.Cell.ForeColor = Color.Red;
						}
					else if (try_num == 100)
						{
						e.Cell.Font.Bold = true;
						}
					else if (try_num_ext > 75)
						{
						e.Cell.Font.Bold = true;
						e.Cell.ForeColor = Color.Orange;
						}
				break;
				case "wo":
					woprog_id = Convert.ToInt32(gv_jobcost.GetDataRow(e.VisibleIndex)["woprog_id"]);
					e.Cell.Text = string.Format(@"<a href=""javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={1}&business_unit_id={2}', 'wo{1}',1280, 960);"">{0}</a>", Toolbox.ReturnBlankIfNull_string(e.CellValue), woprog_id, ddlCompany.Value);
				break;
				case "quoteid":
					if (Toolbox.ReturnBlankIfNull_string(e.CellValue).Length >= 7)
						{
						var quote_n = Toolbox.ReturnBlankIfNull_string(e.CellValue);
						var quote_id = quote_n.Substring(0, 6);
						var rev = quote_n.Substring(5, quote_n.Length - 6);
						e.Cell.Text = string.Format(@"<a href=""javascript:boing('/sections/member/quote/index.aspx?a=g&quote_id={0}&revision={1}', 'quote{0}',1035, 800);"">{0} V{1}</a>", quote_id, rev);
						}
				break;
				}
			}
		}
}
