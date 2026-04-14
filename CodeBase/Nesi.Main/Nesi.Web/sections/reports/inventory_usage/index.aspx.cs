using System;
using System.Web.UI;
using DevExpress.Web;
using System.Web.UI.WebControls;
using System.Data;
using nesi.core;

public partial class sections_reports_inventory_by_vendor_index : Page
{
	NeMember _current_user;
	private int _page_id = 97;
	static string _page_name = "InventoryUsage";
	ASPxHiddenField _h;
	SqlDataSource _ds_templates;
	ASPxDropDownEdit _dde_filter;
	Panel _panel_export;
	protected void Page_Init(object _sender, EventArgs _e)
	{
		_current_user = Toolbox.do_handle_authentication(_page_id);
		layout.__page_name = _page_name;
		_ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		_dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		_panel_export = (Panel)layout.FindControl("panel_export");
		_panel_export.Visible = true;
		_h = (ASPxHiddenField)layout.FindControl("h");
		layout.used_gv = gv_history;
		_h.Set("gridview_id", "gv_history");
		_ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		_ds_templates.SelectParameters["@member_id"].DefaultValue = _current_user.id.ToString();
	}
	protected void Page_Load(object _sender, EventArgs _e)
	{

		ddl_company.DataSource = Toolbox.doSQL_dt(string.Format(@"SELECT id, ddl_name FROM business_unit  WHERE id in ({0}) ORDER BY name", new Current_User().visible_business_units), null);
		if (!IsCallback && !IsPostBack)
		{
			var menu = new NeMenu(_current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			var lbltemp = (Label)Page.Master.FindControl("lblHeading");
			lbltemp.Text = "Inventory Usage Report";
			divSide.InnerHtml = shared.PrintSidePanelHTML(_current_user);

			var gl = new NeGridLayouts(_current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gv_history.FilterExpression = "";
				gl.GridLayout_Layout = gv_history.SaveClientLayout();
				gl.member_id = _current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				_h.Set("ID", gl.GridLayoutID);
				_h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_history.LoadClientLayout(gl.GridLayout_Layout);
				_h.Set("ID", gl.GridLayoutID);
				_h.Set("NAME", gl.GridLayout_Name);
			}
			_dde_filter.Text = gl.GridLayout_Name;
			Session["dt_inventory_usage"] = null;
			ddl_company.DataBind();
			ddl_company.Value = _current_user.business_unit_id;
		}
		load_grid();
	}

	protected void gv_counts_CustomJSProperties(object _sender, ASPxGridViewClientJSPropertiesEventArgs _e)
	{
		var gv = (ASPxGridView)_sender;
		_e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_history_CustomCallback(object _sender, ASPxGridViewCustomCallbackEventArgs _e)
	{
		var gv = (ASPxGridView)_sender;
		if (_e.Parameters != "")
		{
			gv.LoadClientLayout(_e.Parameters);
		}
		else
		{
			gv.FilterExpression = "";
			for (var i = 0; i < gv.Columns.Count; i++)
			{
				var c = gv.Columns[i] as GridViewDataColumn;
				if (c != null)
				{
					var col = c;
					if (col.GroupIndex > -1)
					{
						gv.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}
	}

	private void load_grid()
	{
		var hist = Toolbox.doSQL_dt(@" SELECT a.dt, a.location, a.member_name, a.qty_before, a.qty_after, a.qty_diff, 
(a.cost_per*a.qty_before) db_before, 
(a.cost_per*a.qty_after) db_after, 
((a.cost_per*a.qty_after) - (a.cost_per*a.qty_before)) db_diff,
a.is_manual, a.section, b.ddl_name branch, a.cost_per, a.master_id, a.description 
FROM inventory_usage a 
LEFT JOIN business_unit b ON a.`business_unit_id` = b.id  WHERE a.business_unit_id=@v0 ORDER BY a.id DESC", new object[] { ddl_company.Value });
		gv_history.DataSource = hist;
		gv_history.DataBind();
	}

	protected void bt_excel_Click(object _sender, EventArgs _e)
	{
		ex_inv.WriteXlsToResponse();
	}
	protected void ASPxComboBox1_SelectedIndexChanged(object _sender, EventArgs _e)
	{
		Session["dt_inventory_usage"] = null;
		load_grid();
	}
}
