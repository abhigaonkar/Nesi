using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_reports_inventory_counts_index : Page
{
	Toolbox _tools;
	NeMember current_user;
	static int _page_id = 103;
	static string _page_name = "InventoryCounts";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	bool can_see_cost;

	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		can_see_cost = current_user.AuthenticatedForPrivilege(58);
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		layout.used_gv = gv_counts;
		panel_export.Visible = true;
		h.Set("gridview_id", "gv_counts");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Inventory Counts Report";
		if (!IsCallback && !IsPostBack)
		{
			ddlCompany.DataSource = _tools.getSQL_datatable(string.Format(@"SELECT id, ddl_name name FROM business_unit  WHERE id in ({0}) ORDER BY name",
				new Current_User().visible_business_units), null);
			ddlCompany.DataBind();
			ddlCompany.Value = current_user.business_unit_id;
			var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gv_counts.DataBind();
				var c = Toolbox.doSQL_int(@"SELECT COUNT(id) FROM inventory_location_master WHERE id = @v0 ", new object[] { current_user.member_default_location });
				gv_counts.FilterExpression = c > 0 ? "[loc]='" + new location_master(current_user.member_default_location).name + "'" : "";
				gv_counts.SortBy(gv_counts.Columns["qty_onhand"], DevExpress.Data.ColumnSortOrder.Descending);

				gl.GridLayout_Layout = gv_counts.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_counts.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
			Session["inventory_counts_gv"] = null;
		}
		if (Session["inventory_counts_gv"] == null)
		{
			load_grid();
		}
		else
		{
			gv_counts.DataSource = Session["inventory_counts_gv"];
			gv_counts.DataBind();
		}

	}

	protected void gv_counts_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_counts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.Parameters != "")
		{
			gv.LoadClientLayout(e.Parameters);
		}
		else
		{
			gv_counts.FilterExpression = "[loc]='" + new location_master(current_user.member_default_location).name + "'";
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

	protected void load_grid()
	{
		if (chkbox_show_locations.Checked)
		{
			var dt = _tools.getSQL_datatable(@" SELECT master_id, qty_onhand, tag, description, IFNULL(cost, 0) cost, IFNULL(cost, 0) * IFNULL(qty_onhand, 0) extdcost, name loc, IFNULL(min,0) * IFNULL(cost,0) mincost FROM ( SELECT a.master_id as master_id, f.qty as qty_onhand, f.min, c.tag as tag, d.description as description, e.cost as cost, g.`name` FROM inventory_item_master a LEFT JOIN inventory_branch b ON a.master_id = b.master_id LEFT JOIN inventory_tag c ON a.tag_id = c.tag_id LEFT JOIN inventory_description d ON d.master_id = a.master_id LEFT JOIN inventory_location f ON a.master_id = f.master_id and f.business_unit_id = b.business_unit_id LEFT JOIN inventory_location_master g ON g.id = f.location_master_id LEFT JOIN inventory_cost e ON e.master_id = a.master_id AND e.business_unit_id = b.business_unit_id WHERE a.active = true AND c.is_exclude = false AND c.active = true AND b.business_unit_id = @v0  ) parts ORDER BY master_id ASC", new object[] { ddlCompany.Value });
			dt = _tools.recode_datatable(dt, false);
			gv_counts.DataSource = dt;
			gv_counts.DataBind();
			Session["inventory_counts_gv"] = dt;
		}
		else
		{

			var dt = _tools.getSQL_datatable(@" SELECT master_id, qty_onhand, tag, description, IFNULL(cost, 0) cost, IFNULL(cost, 0) * IFNULL(qty_onhand, 0) extdcost, '' loc , 0 mincost FROM ( SELECT a.master_id as master_id, b.onhand_qty as qty_onhand, c.tag as tag, d.description as description, e.cost as cost FROM inventory_item_master a LEFT JOIN inventory_branch b ON a.master_id = b.master_id LEFT JOIN inventory_tag c ON a.tag_id = c.tag_id LEFT JOIN inventory_description d ON d.master_id = a.master_id LEFT JOIN inventory_cost e ON e.master_id = a.master_id AND e.business_unit_id = b.business_unit_id WHERE a.active = true AND c.is_exclude = false AND c.active = true AND b.business_unit_id = @v0  ) parts ORDER BY master_id ASC", new object[] { ddlCompany.Value });
			dt = _tools.recode_datatable(dt, false);
			gv_counts.DataSource = dt;
			gv_counts.DataBind();
			Session["inventory_counts_gv"] = dt;


		}


	}

	protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
	{
		load_grid();
	}
	protected void gv_counts_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.FieldName == "description")
		{
			e.Cell.Text = _tools.value_from(e.CellValue);
		}

	}

	protected void ASPxCheckBox1_CheckedChanged(object sender, EventArgs e)
	{
		Session["inventory_counts_gv"] = null;
		load_grid();
	}

	protected void gv_counts_DataBound(object sender, EventArgs e)
	{
		if (!can_see_cost)
		{
			if(gv_counts.Columns["extdcost"] != null)
				{
				gv_counts.Columns.Remove(gv_counts.Columns["extdcost"]);
				}
			if(gv_counts.Columns["cost"] != null)
				{
				gv_counts.Columns.Remove(gv_counts.Columns["cost"]);
				}


		}
	}
}
