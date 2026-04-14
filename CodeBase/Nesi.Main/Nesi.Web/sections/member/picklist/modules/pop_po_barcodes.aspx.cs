using System;
using System.Data;
using DevExpress.Web;
using System.Collections.Specialized;
using nesi.core;

public partial class sections_member_picklist_modules_pop_po_barcodes : System.Web.UI.Page
	{
	NeMember current_user;

	    private int _page_id = 1;
	int business_unit_id				= 0;
	int poprog_id				= 0;
	int line_id					= 0;
	Toolbox _tools;

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools					= new Toolbox();
		var _q	= Request.QueryString;
		current_user			= Toolbox.do_handle_authentication(_page_id);
		if(string.IsNullOrEmpty(_q["business_unit_id"]) || string.IsNullOrEmpty(_q["poprog_id"]))
			{
			Toolbox.FriendlyException(Response, "Company ID and/or PO ID not supplied", "window.close();");
			}
		else
			{
			int.TryParse(_q["business_unit_id"], out business_unit_id);
			int.TryParse(_q["poprog_id"], out poprog_id);
			if(!string.IsNullOrEmpty(_q["line_id"]))
				{
				int.TryParse(_q["line_id"], out line_id);
				}
			if(business_unit_id == 0 || poprog_id == 0)
				{
				Toolbox.FriendlyException(Response, "Invalid Company ID and/or PO ID.", "window.close();");
				}
			else if(line_id != 0)
				{
                // Check that this line belongs to this PO
                var temp_id			= Toolbox.doSQL_int(@"SELECT po_details_poprog_id FROM po_details_current WHERE po_details_id = @v0", new object[] { line_id});
				if(poprog_id != temp_id)
					{
					Toolbox.FriendlyException(Response, "The supplied ID doesn't belong to the specified PO.", "window.close();");
					}
				}
			}
		gv_pop_bcqty_populate();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		}
	protected void gv_pop_bcqty_populate()
		{
		var src = new DataTable();
		var base_sql = @"
SELECT 
	b.po_details_id id,
	c.master_id,
	d.is_exclude,
	b.po_details_description description,
	b.po_details_woprog_id woprog_id,
	ROUND(IF(((d.is_exclude=1)or(d.usa_sold_as!=3) or (d.canadian_sold_as!=3)),1,(b.po_details_qty_received*b.po_details_vendor_qty_per)),0) as qty
FROM
	poprog_header a
INNER JOIN 
	po_details_current b 
		ON b.po_details_poprog_id = a.poprog_id
INNER JOIN 
	inventory_item_master c 
		ON b.po_details_part_no = c.master_id
INNER JOIN 
	inventory_tag d 
		ON c.tag_id = d.tag_id
WHERE
";
		if(line_id == 0)
			{
			base_sql	+= string.Format(@"
	po_details_poprog_id = '{0}' AND 
	po_details_part_no != 0 AND
	(d.is_exclude = false || ((b.po_details_woprog_id BETWEEN 30000 AND 9999996) and  b.is_gl_account = false))", poprog_id);
			}
		else
			{
			base_sql	+= string.Format(@"
	po_details_id = '{0}' AND
	(d.is_exclude = false || ((b.po_details_woprog_id BETWEEN 30000 AND 9999996) and b.is_gl_account = false))", line_id);
			}

		// Recode the datatable as using URLDECODE can cost cycles.
		src = Toolbox.doSQL_dt(base_sql,null);
		src = _tools.recode_datatable(src, false);
		gv_pop_bcqty.DataSource = src;
		gv_pop_bcqty.DataBind();
		}
	protected void btn_printqtybcs_Click(object sender, EventArgs e)
		{

		//ASPxGridView gv = (ASPxGridView)pop_bc_printqty.FindControl("gv_pop_bcqty");

		/*		string[] fields = new string[] { "master_id", "description", "qty" };

				List<object> keyvalues = Grid_GroupSelect.GetSelectedFieldValues(fields);
		*/
		}
	protected void btn_printqtybcs_Click1(object sender, EventArgs e)
		{
		//	hid_pop_bcqty_partslist["gv"]
		}
	protected void cbp_bc_qty_Callback(object sender, CallbackEventArgsBase e)
		{
		}
	protected void gv_pop_bcqty_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{
		var qty = Convert.ToInt32(e.GetValue("qty"));
		if (qty > 5) // Low printing
			{
			e.Row.BackColor = System.Drawing.Color.Yellow;
			}
		if (qty > 10) // Medium printing
			{
			e.Row.BackColor = System.Drawing.Color.Orange;
			}
		if (qty > 20) // High printing
			{
			e.Row.BackColor = System.Drawing.Color.Maroon;
			e.Row.ForeColor = System.Drawing.Color.White;
			}
		}
	protected void gv_pop_bcqty_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if(e.DataColumn.FieldName == "qty" || e.DataColumn.FieldName == "woprog_id")
			{
			var gv		= (ASPxGridView) sender;
			var master_id		= Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "master_id"));
			var woprog_id		= Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "woprog_id"));
			var is_exclude		= Convert.ToBoolean(gv.GetRowValues(e.VisibleIndex, "is_exclude"));
			if(is_exclude && e.DataColumn.FieldName == "qty")
 				{
				var tb	= (ASPxTextBox) gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "txtbcqty");
				if(tb != null)
					{
					tb.Enabled		= false;
					tb.Value		= 0;
					}
				}
			else if(is_exclude && (woprog_id < 10000 || woprog_id > 9999996) && e.DataColumn.FieldName == "woprog_id")
				{
				var bt	= (ASPxButton) gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "bt_printwo");
				if(bt != null)
					{
					bt.Visible	= false;
					}
				}
			}
		}
	protected void bt_printwo_Click(object sender, EventArgs e)
		{
		var b							= (ASPxButton) sender;
		var nc	= (GridViewDataItemTemplateContainer) b.NamingContainer;
		var gv							= nc.Grid;
		var woprog_id							= Convert.ToInt32(gv.GetRowValues(nc.VisibleIndex, "woprog_id"));
		var wo								= new NeWOProg(woprog_id);
		wo.print_barcode_label(1);
		}
	protected void btn_printqtybcs_Click2(object sender, EventArgs e)
		{
		var b			= (ASPxButton) sender;
		var cbp	= (ASPxCallbackPanel) b.NamingContainer;
		var gv			= (ASPxGridView)cbp.FindControl("gv_pop_bcqty");
		var start = gv.VisibleStartIndex;
		var end = (gv.PageIndex + 1) * gv.SettingsPager.PageSize;
		if (end > gv.VisibleRowCount)
			{
			end = gv.VisibleRowCount;
			}
		var inv = new inventory();
		for (var i = start; i < end; i++)
			{
			var master_id = gv.GetRowValues(i, new string[] { "master_id" });
			var gvdc = (GridViewDataColumn)gv.Columns[2];
			var box_qty = (ASPxTextBox)gv.FindRowCellTemplateControl(i, gvdc, "txtbcqty");
			var qty = 0;
			if (box_qty != null)
				{
				int.TryParse(box_qty.Text, out qty);
				}
			if (qty > 0 && master_id != null)
				{
				inv.Load(master_id, business_unit_id);
				inv.print_barcode_label(qty);
				}
			}
		}
}