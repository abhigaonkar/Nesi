using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using nesi.core;

public partial class sections_member_inventory_user_controls_orders_tab : System.Web.UI.UserControl
{
	public NeMember current_user;
	private const int _page_id = 43; // from Page table in DB
	public const string _page_name = "BranchInventory";
	protected bool is_purchaser = false;
	protected bool can_qty_tab = false;
	protected bool can_adjust_stock = false;
	NameValueCollection _q;
	public NeBusinessUnit WorkingBusinessUnit {get; set; }
	public NeBusinessUnit WarehouseBusinessUnit {get; set; }
	private string page_name = "";
	public ASPxGridView gvorders { get { return gv_orders; } }
	public HtmlInputHidden hid_exp { get { return hidexp; } }
	protected void Page_Init(object sender, EventArgs e)
	{
		current_user = Toolbox.do_handle_authentication(_page_id);
		can_adjust_stock = current_user.AuthenticatedForPrivilege(104);
		is_purchaser = current_user.AuthenticatedForPrivilege(71);
		if (Session["working_business_unit_id"] == null && WorkingBusinessUnit == null)
		{
			Session["working_business_unit_id"] = current_user.business_unit_id.ToString();
		}
		if (Session["working_warehouse_bu_id"] == null && WarehouseBusinessUnit == null)
		{
			Session["working_warehouse_bu_id"] = current_user.business_unit.warehouse_bu_id;
		}
			WorkingBusinessUnit = new NeBusinessUnit(Session["working_business_unit_id"]);
			WarehouseBusinessUnit = new NeBusinessUnit(Session["working_warehouse_bu_id"]);
		_q = Request.QueryString;
		if (_q["a"] == "orders")
		{
			#region Work Order Filter
			var wo_c = Toolbox.doSQL_dt(@"
SELECT 
	b.woprog_id,
	b.woprog_bvwo,
	c.customer_name customer_name,
	c.customer_id,
    CONCAT(e.member_nickname, ' ', e.member_lastname) as pm 
FROM 
	wo_detail_current a
LEFT JOIN
	woprog b
		ON a.wo_detail_current_woprog_id = b.woprog_id
LEFT JOIN
	customer c
		ON b.woprog_customer_id = c.customer_id
LEFT JOIN
	member e
		ON b.woprog_pm_memberid = e.member_id
WHERE 
	a.business_unit_id = @v0 AND
	a.wo_detail_current_qty_ordered - a.wo_detail_current_qty_committed > 0 AND
	c.customer_id IS NOT NULL
GROUP BY woprog_id
ORDER BY woprog_bvwo", new object[] { Session["working_business_unit_id"] });
			var s = new StringBuilder();
			s.Append(@"
<div style='margin:2px;width:450px;border:solid 2px #999;border-radius:5px;'>
	<div style='background-color:#999;color:#fff;font-weight:bold;font-size:14px;padding:2px;'>Options</div>
	<div style='padding:2px;'>");
			s.Append(@"
		<b style='display:inline-block;width:150px'>WO Filter:</b>
		<select onchange='handle_wo_filter(this)' id='wo_filter' style='font-family:arial;font-size:11px;width:250px;font-weight:bold;'>
			<option value='0'>Select All</option>");
			foreach (DataRow r in wo_c.Rows)
			{
				var wo_id = r["woprog_id"].ToString();
				var wo_n = r["woprog_bvwo"].ToString();
				var cust = r["customer_name"].ToString();
				var pm = r["pm"].ToString();
				s.Append(string.Format("<option value='{0}'>{1} - {2} - {3}</option>", wo_id, wo_n, cust, pm));
			}
			s.Append("</select><br/>");
			s.Append("<input type='checkbox' style='margin-left:155px;' onclick='handle_wo_filter(null)' id='exclude_wo'><label for='exclude_wo'>Include everything <b>BUT</b> this work order</label></div>");
			s.Append("<div style='padding:2px;'><b style='display:inline-block;width:150px'>Customer Filter:</b> <select onchange='handle_cust_filter(this)' id='cust_filter' style='font-family:arial;font-size:11px;width:250px;font-weight:bold;'><option value='0'>Select All</option>");
			var selectable_customer_ids = new List<int>();
			var view = wo_c.DefaultView;
			view.Sort = "customer_name ASC";
			wo_c = view.ToTable();
			foreach (DataRow r in wo_c.Rows)
			{
				var cust_name = r["customer_name"].ToString();
				var cust_id = Convert.ToInt32(r["customer_id"]);
				if (!selectable_customer_ids.Contains(cust_id))
				{
					s.Append(string.Format("<option value='{0}'>{1}</option>", cust_id, cust_name));
					selectable_customer_ids.Add(cust_id);
				}
			}
			s.Append("</select>");
			s.Append("<input type='checkbox' style='margin-left:155px;' onclick='handle_cust_filter(null)' id='exclude_cust'><label for='exclude_cust'>Include everything <b>BUT</b> this customer</label></div>");
			s.Append("<div style='padding:2px;'><label for='include_external' onclick='toggle_include_external(this)' style='display:inline-block;width:150px;font-weight:bold;'>Include Ext. Locations:</label><input id='include_external' onclick='toggle_include_external(this)' type='checkbox'></div>");
			s.Append("<div style='padding:2px;'><b style='display:inline-block;width:150px'>Include Only:</b><select onchange='handle_extloc_filter(this)' id='extloc_filter' style='font-family:arial;font-size:11px;width:250px;font-weight:bold;'><option value='0'>Select All</option>");
			var _extlocs = Toolbox.doSQL_dt(@"SELECT id,name FROM inventory_location_master  WHERE business_unit_id =@v0 AND type_id = 2 ORDER BY name", new object[] { WarehouseBusinessUnit.id });
			foreach (DataRow r in _extlocs.Rows)
			{
				var _name = r["name"].ToString();
				var _id = Convert.ToInt32(r["id"]);
				s.Append(string.Format("<option value='{0}'>{1}</option>", _id, _name));
			}
			s.Append("</select></div>");

			var _rfqs = Toolbox.doSQL_dt(@"SELECT id,name FROM rfq_header  WHERE business_unit_id = @v0 AND date_close > curdate() ORDER BY name", new object[] { WarehouseBusinessUnit.id });
			if (_rfqs.Rows.Count > 0)
			{
				s.Append("<div style='padding:2px;'><b style='display:inline-block;width:150px'>Filter by RFQ:</b><select onchange='handle_rfq_filter(this)' id='rfq_filter' style='font-family:arial;font-size:11px;width:250px;font-weight:bold;'><option value='0'>Select RFQ</option>");
				foreach (DataRow r in _rfqs.Rows)
				{
					var _name = r["name"].ToString();
					var _id = Convert.ToInt32(r["id"]);
					s.Append(string.Format("<option value='{0}'>{1}</option>", _id, _name));
				}
				s.Append("</select></div>");
			}

			s.Append("<div style='padding:2px;'><label for='cut_new' style='display:inline-block;width:150px;font-weight:bold;'>Force new PO's:</label><input type='checkbox'  id='cut_new' /></div>");
			s.Append("<div style='padding:2px;'><span style='display:inline-block;width:150px;font-weight:bold;'>Fill Max QTY's:</span><button type='button' onclick='fill_max(this)'>Fill</button></div>");
			s.Append("</div>");
			wo_selector.InnerHtml = s.ToString();
			#endregion Work Order Filter
		}
	}
	protected void gv_orders_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		e.Properties["cpFil"] = gv.FilterExpression;
	}
	public void fill_gv_orders()
	{
		if (Session["inv_gv_orders"] == null || Request.Form.Count == 0)
		{
			var cust_id = Session["orders_customer_id"] ?? 0;
			var loc_id = Session["orders_ext_loc_id"] ?? 0;
			var include_external = Session["inv_branch_include_external"] ?? 0;
			var rfq_id = Session["orders_rfq_id"] ?? 0;

			var dt = Toolbox.doSQL_dt(@"CALL OrdersTab3(@v0 , @v1, @v2 , 0, @v3 , @v4)", new [] { WarehouseBusinessUnit.id, WorkingBusinessUnit.id, include_external, cust_id, loc_id});
			if (Convert.ToInt32(rfq_id) > 0)
			{
				var used_master_ids = Toolbox.doSQL_string(@"SELECT IFNULL(GROUP_CONCAT(master_id), '') FROM rfq_lineitem where rfq_header_id = @v0", new [] { rfq_id });
				if (used_master_ids != "")
				{
					var temp_dt = dt.Clone();
					var used_ids = used_master_ids.Split(',').Select(int.Parse).ToList();
					foreach (DataRow row in dt.Rows)
					{
						var master_id = (int)row["master_id"];
						if (used_ids.Contains(master_id))
						{
							temp_dt.ImportRow(row);
						}
					}
					dt.Clear();
					dt = temp_dt;
				}
			}
			Session["inv_gv_orders"] = dt;
			//_tools.debug_note(sql_);
		}
		gv_orders.DataSource = Session["inv_gv_orders"];
		gv_orders.DataBind();
	}
	DataTable _po_c = new DataTable("asdf");
	DataTable _wo_c = new DataTable("asdf");
	DataTable _po_count = new DataTable("asdf");
	DataTable _default_vendors = new DataTable("asdf");
	private void fill_gv_orders_workorders()
	{
		if (_po_c.TableName == "asdf")
		{
			_po_c.TableName = "fdsa";
			_wo_c.TableName = "fdsa";
			_default_vendors.TableName = "fdsa";
			var grid = gv_orders;
			var part_column = (GridViewDataColumn)grid.Columns["master_id"];
			var start = grid.VisibleStartIndex;
			var end = (grid.PageIndex + 1) * grid.SettingsPager.PageSize;
			var str_orders_customer_id = Session["orders_customer_id"] != null ? Session["orders_customer_id"].ToString() : "";
			var orders_customer_id = str_orders_customer_id != ""
					? str_orders_customer_id.Contains("_")
						? Convert.ToInt32(str_orders_customer_id.Split('_')[1])
						: Convert.ToInt32(str_orders_customer_id)
					: 0;
			var dept_id = Session["orders_dept_id"] == null ? 0 : Convert.ToInt32(Session["orders_dept_id"]);
			var exclude_customer = str_orders_customer_id.Contains("_");
			if (end > grid.VisibleRowCount)
			{
				end = grid.VisibleRowCount;
			}
			var part_numbers = new List<string>();
			for (var wi = start; wi < end; wi++)
			{
				var part_nu = grid.GetRowValues(wi, "master_id").ToString() == "" ? 0 : Convert.ToInt32(grid.GetRowValues(wi, "master_id"));
				if (part_nu < 900000 && part_nu != 0)
				{
					part_numbers.Add(grid.GetRowValues(wi, "master_id").ToString());
				}
			}
			var part_list = string.Join(",", part_numbers.Select(n => n.ToString()).ToArray());
			_po_c = Toolbox.doSQL_dt(@"
SELECT 
	a.po_details_part_no master_id,
	b.poprog_id,
	b.poprog_bvpo,
	a.po_details_qty_ordered - a.po_details_qty_received qty_still_to_be,
	c.vendor_name,
	CAST(IFNULL(DATE_FORMAT(b.poprog_expected_received_date, '%m/%d/%Y'), 'Not Set') AS CHAR(10)) expected_date
FROM 
	po_details_current a
LEFT JOIN
	poprog_header b ON
		a.po_details_poprog_id = b.poprog_id
LEFT JOIN
	vendor c ON
		b.poprog_vendor_id = c.vendor_id
WHERE 
	FIND_IN_SET(a.po_details_part_no, @v0) AND 
	b.business_unit_id = @v1 AND
	b.poprog_status != 6 AND
	(
	a.po_details_qty_ordered = 0 OR
	((a.po_details_qty_ordered != a.po_details_qty_received) AND (a.po_details_qty_ordered - a.po_details_qty_received) > 0) and a.po_details_line_active = 1
	)
", new object[] { part_list, WorkingBusinessUnit.id });
			if (orders_customer_id != 0)
			{
				var inc_exc_customer = exclude_customer ? "!=" : "=";
				_wo_c = Toolbox.doSQL_dt(string.Format(@"
SELECT 
	a.wo_detail_current_master_id master_id,
	b.woprog_id,
	b.woprog_bvwo,
	c.customer_name, 
	IFNULL(a.wo_detail_current_qty_ordered,0) - IFNULL(a.wo_detail_current_qty_committed,0) qty_needed,
	a.wo_detail_current_notes notes,
    member_name(b.woprog_pm_memberid) pm,
    CAST(IFNULL(wo_detail_current_date_required,'N/A') AS CHAR) reqdate 
FROM 
	wo_detail_current a
LEFT JOIN
	woprog b
		ON a.wo_detail_current_woprog_id = b.woprog_id
LEFT JOIN
	customer c
		ON b.woprog_customer_id = c.customer_id
WHERE 
	FIND_IN_SET(a.wo_detail_current_master_id, '{0}') AND 
	a.business_unit_id = '{1}' AND
	b.woprog_status = 'Open' AND
	b.woprog_hold != TRUE AND
	b.woprog_customer_id {3} {2} AND
	(IFNULL(a.wo_detail_current_qty_ordered,0) - IFNULL(a.wo_detail_current_qty_committed,0)) > 0 ",part_list, WorkingBusinessUnit.id, orders_customer_id, inc_exc_customer), null);
			}
			else
			{
				_wo_c = Toolbox.doSQL_dt(@"
SELECT 
	a.wo_detail_current_master_id master_id,
	b.woprog_id,
	b.woprog_bvwo,
	c.customer_name, 
	IFNULL(a.wo_detail_current_qty_ordered,0) - IFNULL(a.wo_detail_current_qty_committed,0) qty_needed,
	a.wo_detail_current_notes notes,
    member_name(b.woprog_pm_memberid) pm,
    CAST(IFNULL(wo_detail_current_date_required,'N/A') AS CHAR) reqdate 
FROM 
	wo_detail_current a
LEFT JOIN
	woprog b
		ON a.wo_detail_current_woprog_id = b.woprog_id
LEFT JOIN
	customer c
		ON b.woprog_customer_id = c.customer_id
WHERE 
	FIND_IN_SET(a.wo_detail_current_master_id, @v0) AND 
	a.business_unit_id = @v1 AND
	b.woprog_status = 'Open' AND
	b.woprog_hold != TRUE AND
	(IFNULL(a.wo_detail_current_qty_ordered,0) - IFNULL(a.wo_detail_current_qty_committed,0)) > 0", new object[] { part_list, WorkingBusinessUnit.id });
			}
			_po_count = Toolbox.doSQL_dt(@"
SELECT 
	po_details_part_no master_id, 
	COUNT(*) c 
FROM 
	po_details_current a 
LEFT JOIN 
	poprog_header b ON a.po_details_poprog_id = b.poprog_id 
WHERE 
	FIND_IN_SET(a.po_details_part_no, @v0) AND 
	b.business_unit_id = @v1 AND 
	(
	a.po_details_qty_ordered = 0 OR 
		(
		a.po_details_qty_ordered != a.po_details_qty_received
		)
	) and 
	a.po_details_line_active = 1 
GROUP BY master_id", new object[] { part_list, WorkingBusinessUnit.id });
			_default_vendors = Toolbox.doSQL_dt(@"SELECT a.master_id, b.vendor_id id, 
b.vendor_name name, a.qty, a.cost, a.lead_time, a.vendor_code, IFNULL(a.is_preferred, 0) is_preferred, 
a.edited_dt _date FROM inventory_price a LEFT JOIN vendor b ON a.vendor_id = b.vendor_id WHERE FIND_IN_SET(a.master_id, @v0) AND a.business_unit_id = @v1",
new object[] { part_list, WarehouseBusinessUnit.id});
		}
	}
	protected void gv_orders_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		fill_gv_orders_workorders();
		var index = e.VisibleIndex;
		var gv = sender as ASPxGridView;
		var master_id = gv.GetRowValues(index, "master_id").ToString();
		var vendor_id = 0;
		int.TryParse(gv.GetRowValues(index, "vendor_id").ToString(), out vendor_id);
		var cost_level = gv.GetRowValues(index, "cost_level") == DBNull.Value ? 0 : Convert.ToInt32(gv.GetRowValues(index, "cost_level"));
		var poprog_id = gv.GetRowValues(index, "poprog_id").ToString();
		var qty_required = gv.GetRowValues(index, "qty_required").ToString();
		var qty_on_order = Convert.ToDouble(gv.GetRowValues(index, "qty_on_order"));
		var qty_stock_inl = Convert.ToDouble(gv.GetRowValues(index, "qty_stock_int"));
		var qty_stock_exl = Convert.ToDouble(gv.GetRowValues(index, "qty_stock_ext"));
		var qty_min = Convert.ToDouble(gv.GetRowValues(index, "qty_min"));
		var qty_max = Convert.ToDouble(gv.GetRowValues(index, "qty_max"));

		var column = e.DataColumn.Caption;
		GridViewDataColumn gvc;

		var pos_wo_qty = column != "PO" || _po_count.Select("master_id = " + master_id).Length == 0 ? 0 : Convert.ToInt32(_po_count.Select("master_id = " + master_id)[0]["c"]);
		if (e.DataColumn.FieldName == "description")
		{
			//e.Cell.Text				= Toolbox.do_value_from(e.CellValue, false);
		}
		if (e.DataColumn.Caption == "Description")
		{
			switch (cost_level)
			{
				case 0:
					e.Cell.Style.Add("background-color", "#fff");
					e.Cell.Style.Add("color", "#000");
					break;
				case 1:
					e.Cell.Style.Add("background-color", "#009900");
					e.Cell.Style.Add("color", "#fff");
					break;
				case 2:
					e.Cell.Style.Add("background-color", "#1d7373");
					e.Cell.Style.Add("color", "#fff");
					break;
				case 3:
					e.Cell.Style.Add("background-color", "#009999");
					e.Cell.Style.Add("color", "#fff");
					break;
				case 4:
					e.Cell.Style.Add("background-color", "#5ccccc");
					e.Cell.Style.Add("color", "#fff");
					break;
				case 5:
					e.Cell.Style.Add("background-color", "#a66f00");
					e.Cell.Style.Add("color", "#fff");
					break;
				case 6:
					e.Cell.Style.Add("background-color", "#bf8f30");
					e.Cell.Style.Add("color", "#fff");
					break;
				case 7:
					e.Cell.Style.Add("background-color", "#ff9966");
					e.Cell.Style.Add("color", "#fff");
					break;
				case 8:
					e.Cell.Style.Add("background-color", "#bf3030");
					e.Cell.Style.Add("color", "#fff");
					break;
				case 9:
					e.Cell.Style.Add("background-color", "#a60000");
					e.Cell.Style.Add("color", "#fff");
					break;
				case 10:
					e.Cell.Style.Add("background-color", "#ff0000");
					e.Cell.Style.Add("color", "#fff");
					break;
			}
		}
		if (e.DataColumn.FieldName == "qty_to_order")
		{
			if (Convert.ToInt32(Session["inv_branch_include_external"]) == 0)
			{
				e.Cell.ToolTip = string.Format("{0} = Req({1}) - On Order({2}) - In Stock( Inl:{3}) + Min Stock Qty({4})", e.CellValue, qty_required, qty_on_order, qty_stock_inl, qty_min);
			}
			else
			{
				e.Cell.ToolTip = string.Format("{0} = Req({1}) - On Order({2}) - In Stock( Inl:{3}, exl: {4}) + Min Stock Qty({5})", e.CellValue, qty_required, qty_on_order, qty_stock_inl, qty_stock_exl, qty_min);
			}
		}
		if (e.DataColumn.FieldName == "qty_to_order_to_max")
		{
			if (Convert.ToInt32(Session["inv_branch_include_external"]) == 0)
			{
				e.Cell.ToolTip = string.Format("{0} = Req({1}) - On Order({2}) - In Stock( Inl:{3}) + Max Stock Qty({4})", e.CellValue, qty_required, qty_on_order, qty_stock_inl, qty_max);
			}
			else
			{
				e.Cell.ToolTip = string.Format("{0} = Req({1}) - On Order({2}) - In Stock( Inl:{3}, exl: {4}) + Max Stock Qty({5})", e.CellValue, qty_required, qty_on_order, qty_stock_inl, qty_stock_exl, qty_max);
			}
		}
		if (column == "PO" && qty_on_order == 0 && vendor_id != 0 && pos_wo_qty == 0)
		{
			e.Cell.Text = @"<img src='/images/icon/icon[shipping].gif' onclick=""location.href='/sections/purchaseorder/po_prog_add.aspx?action=add&poprogid=0&companyid=" + WorkingBusinessUnit.id + @"'""/>";
		}

		else if (column == "PO" && qty_on_order >= 0 && vendor_id != 0 && pos_wo_qty != 0)
		{
			var r = _po_c.Select("master_id = '" + master_id + "'");
			if (r.Length != 0)
			{
				// bvwo#, woprog_id, customer name, department
				var sb = new StringBuilder();
				for (var c = 0; c < r.Length; c++)
				{
					var po_id = r[c]["poprog_id"].ToString();
					var po_n = r[c]["poprog_bvpo"].ToString();
					var vend = Toolbox.do_value_from(r[c]["vendor_name"], false);
					var exp_date = r[c]["expected_date"].ToString();
					var qty = Convert.ToDouble(r[c]["qty_still_to_be"]) == 0 ? "None Ordered" : r[c]["qty_still_to_be"] + " Unreceived";
					sb.AppendFormat("<div class='wo_link' title='Expected Delivery Date: {2}'><a target='_blank' href='/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}&lineitems=true'><b>{1}</b> ({3} - {4}) </a></div>", po_id, qty, exp_date, po_n, vend);
					e.Cell.Text = sb.Length == 0 ? @"<img src='/images/icon/icon[shipping].gif' onclick=""location.href='/sections/purchaseorder/po_prog_add.aspx?action=add&poprogid=0&companyid=" + WorkingBusinessUnit.id + @"'""/>" : sb.ToString();
				}
			}
			else
			{
				e.Cell.Text = @"<img src='/images/icon/icon[shipping].gif' onclick=""location.href='/sections/purchaseorder/po_prog_add.aspx?action=add&poprogid=0&companyid=" + WorkingBusinessUnit.id + @"'""/>";
			}
		}
		else if (column == "Stock (int)" && !can_adjust_stock)
		{
			gvc = (GridViewDataColumn)gv.Columns["Stock (Int)"];
			var t_qty_stock_int = (HtmlInputText)gv.FindRowCellTemplateControl(index, gvc, "t_qty_stock_int");
			t_qty_stock_int.Disabled = true;
		}
		else if (column == "Stock (ext)" && !can_adjust_stock)
		{
			gvc = (GridViewDataColumn)gv.Columns["Stock (ext)"];
			var t_qty_stock_ext = (HtmlInputText)gv.FindRowCellTemplateControl(index, gvc, "t_qty_stock_ext");
			t_qty_stock_ext.Disabled = true;
		}
		else if (column == "Process" && master_id == "777")
		{
			gvc = (GridViewDataColumn)gv.Columns[14];
			var chk_process = (HtmlInputCheckBox)gv.FindRowCellTemplateControl(index, gvc, "cb_inc");
			if (chk_process != null)
			{
				chk_process.Visible = false;
			}
		}
		else if (column == "Min" && !is_purchaser)
		{
			gvc = (GridViewDataColumn)gv.Columns["Min"];
			var t_qty_min = (HtmlInputText)gv.FindRowCellTemplateControl(index, gvc, "t_qty_min");
			t_qty_min.Disabled = true;
		}
		else if (column == "Max" && !is_purchaser)
		{
			gvc = (GridViewDataColumn)gv.Columns["Max"];
			var t_qty_max = (HtmlInputText)gv.FindRowCellTemplateControl(index, gvc, "t_qty_max");
			t_qty_max.Disabled = true;
		}
		else if (e.DataColumn.Name == "wo_parts")
		{
			var res = new StringBuilder();
			var r = _wo_c.Select("master_id = '" + master_id + "'");
			if (r.Length != 0)
			{
				// bvwo#, woprog_id, customer name, department
				for (var c = 0; c < r.Length; c++)
				{
					var wo_id = r[c]["woprog_id"].ToString();
					var wo_n = r[c]["woprog_bvwo"].ToString();
					var cust = Toolbox.do_value_from(r[c]["customer_name"], false);
					var qty = Convert.ToDouble(r[c]["qty_needed"]);
					var notes = Toolbox.do_value_from(r[c]["notes"], true);
					var pm = r[c]["pm"].ToString();
					var reqdate = r[c]["reqdate"].ToString();
					var note_img = notes.Length > 0 ? @"<img src='/images/icon/icon[note].gif' class='wo_note' align='absmiddle' style='margin-right:2px;' data-tooltip=""" + notes + @""" />" : "";
					res.AppendFormat("<div class='wo_link'>{5}<a target='_blank' href='/sections/workorder/index.aspx?woprog_id={0}&business_unit_id={1}&lineitems=true'><b>Date Required: {7} </b> <b style='font-size:16px'> | </b> ({3}) {4}<b style='font-size:16px'> | </b><b>{2} Needed<b style='font-size:16px'> | </b>{6}</b></a></div>", wo_id, WorkingBusinessUnit.id, qty, wo_n, cust, note_img, pm, reqdate);
				}
				e.Cell.Text = res.ToString();
			}
			else
			{
				e.Cell.Text = "";
			}
		}
		else if (column == "Est. Delivery")
		{
			gvc = (GridViewDataColumn)gv.Columns["Ex. Deliv."];
			var deliv_date = (ASPxDateEdit)gv.FindRowCellTemplateControl(index, gvc, "delivery_date");
			deliv_date.Visible = (poprog_id != "");
			deliv_date.Enabled = is_purchaser;
		}
		else if (column == "Default Vendor")
		{
			var dv = new StringBuilder();
			var r = _default_vendors.Select("master_id = '" + master_id + "'", "is_preferred DESC, cost ASC");
			dv.Append("<select onchange='sw_def_ven(this)' style='min-width:100px;width:100%;font-size:11px;font-family:Tahoma'>");
			if (r.Length != 0)
			{
				for (var c = 0; c < r.Length; c++)
				{
					var is_preferred = r[c]["is_preferred"].ToString() == "1" ? "selected" : "";
					dv.AppendFormat(@"<option data-qty='{0}' data-cost='{1}' data-lead='{2}' data-code=""{3}"" value='{4}' {5} data-_date='{7}'>{6}",
						r[c]["qty"],          // {0}
						r[c]["cost"],         // {1}
						r[c]["lead_time"],    // {2}
						Toolbox.do_value_from(r[c]["vendor_code"]),  // {3}
						r[c]["id"],           // {4}
						is_preferred,         // {5}
						r[c]["name"],         // {6}
						r[c]["_date"]
						);
				}
			}
			dv.Append("<select>");
			e.Cell.Text = dv.ToString();
		}
	}
	protected void Load_Layout(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.Parameters != "" && e.Parameters != "reload" && gv.ID == "gv_orders")
		{
			gv.LoadClientLayout(e.Parameters);
		}
		else if (e.Parameters != "" && e.Parameters == "reload" && gv.ID == "gv_orders")
		{
			page_name = string.Format("{0}_gv_orders", _page_name);
			var gl = new NeGridLayouts(current_user.id, page_name);
			gv_orders.LoadClientLayout(gl.GridLayout_Layout);

		}
		else if (e.Parameters != "")
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
}