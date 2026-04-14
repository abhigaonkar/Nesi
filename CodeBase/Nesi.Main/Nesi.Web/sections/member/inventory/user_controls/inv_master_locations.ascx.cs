using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Text;
using NESI.Common.Models;
using nesi.core;

public partial class sections_member_inventory_master_locations : System.Web.UI.UserControl
	{
	public NeMember current_user;
	private const int _page_id = 1; // from Page table in DB
	public string selected_master;
	private const string _page_name = "InventoryCopyLocations";
	Toolbox _tools;
	public NeBusinessUnit WorkingBusinessUnit  { get; set; }
	public NeBusinessUnit WarehouseBusinessUnit  { get; set; }
	bool can_copy_locations			= false;
	bool can_merge_locations			= false;
	bool is_purchaser				= false;
	bool is_admin					= false;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		var _q = Request.QueryString;
		can_copy_locations		= current_user.AuthenticatedForPrivilege(123);
		can_merge_locations		= current_user.AuthenticatedForPrivilege(137);
		is_purchaser			= current_user.AuthenticatedForPrivilege(71);
		is_admin				= current_user.AuthenticatedForPage(49);
		if(!IsPostBack)
			{
			Session.Remove("inv_master_location_country");
			Session.Remove("inv_master_location_id");
			}
		if(!is_purchaser)
			{
			gv_master_locations.Settings.ShowTitlePanel		= false;
			}
		if(!is_admin)
			{
			bt_delete_loc.ClientVisible			= false;
			}
		}
	protected void gv_master_locations_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		if (e.Parameters == "print_bc")
			{
			var selectItems = gv.GetSelectedFieldValues("id");
			foreach (var selectItemId in selectItems)
				{
				var lm = new location_master(Convert.ToInt32(selectItemId));
				var l = new location();
				l.business_unit_id = lm.business_unit_id;
				l.this_master = lm;
				l.id = lm.id;
				l.print_location_barcode_label(1);
				}
			}
		}

	protected void gv_master_locations_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		GridViewDataColumn gvc;
		if(e.DataColumn.Caption == "Action")
			{
			gvc									= (GridViewDataColumn) gv.Columns["Action"];
			var name							= gv.GetRowValues(e.VisibleIndex, "barename").ToString();
			var n_parts							= Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "n_parts"));
			var n_users							= Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "n_users"));
			var n_pos							= Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "n_pos"));
			var copy_button				= (HtmlButton) gv.FindRowCellTemplateControl(e.VisibleIndex, gvc, "copy_button");
			var delete_button			= (HtmlButton) gv.FindRowCellTemplateControl(e.VisibleIndex, gvc, "delete_button");
			var merge_button				= (HtmlButton) gv.FindRowCellTemplateControl(e.VisibleIndex, gvc, "merge_button");
			var view_button				= (HtmlButton) gv.FindRowCellTemplateControl(e.VisibleIndex, gvc, "view_button");
			if(copy_button != null)
				{
				copy_button.Visible				= name != "0.0.0" && can_copy_locations && n_parts > 0;
				}
			if(merge_button != null)
				{
				merge_button.Visible			= can_merge_locations && n_parts > 0;
				}
			if(delete_button != null && (n_parts > 0 | n_users > 0 | n_pos > 0))
				{
				delete_button.Disabled				= true;
				if(n_parts > 0)
					{
					delete_button.Attributes["title"]	= delete_button.Disabled ? "This location has parts linked to it, and cannot be deleted." : "";
					}
				else if(n_users > 0)
					{
					delete_button.Attributes["title"]	= delete_button.Disabled ? "This location has users linked to it, and cannot be deleted." : "";
					}
				else if(n_pos > 0)
					{
					delete_button.Attributes["title"]	= delete_button.Disabled ? "This location has active purchase orders linked to it, and cannot be deleted." : "";
					}
				}
			if(view_button != null)
				{
				view_button.Visible					= n_parts > 0;
				}
			}
		}
	protected void pop_copy_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
		{
		var master_location_id						= 0;
		int.TryParse(e.Parameter, out master_location_id);
		var ilm						= new location_master(master_location_id);
		l_which_location.Text						= ilm.type_id == 1 ? "Int - " + ilm.name : "Ext - " + ilm.name;
		Session["inv_master_location_id"]			= master_location_id;
		ddl_destinationlocation.DataBind();
		gv_current_mappings.DataBind();
		}
	protected void pop_merge_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
		{
		var master_location_id						= 0;
		int.TryParse(e.Parameter, out master_location_id);
		var ilm						= new location_master(master_location_id);
		merge_l_which_location.Text					= ilm.type_id == 1 ? "Int - " + ilm.name : "Ext - " + ilm.name;
		merge_ddl_destinationlocation.SelectedIndex	= -1;
		merge_ddl_destinationlocation.Value			= null;
		Session["inv_master_location_id"]			= master_location_id;
		merge_ddl_destinationlocation.DataBind();
		gv_merge_current_mappings.DataBind();
		}
	protected void pop_view_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
		{
		var master_location_id						= 0;
		int.TryParse(e.Parameter, out master_location_id);
		var ilm						= new location_master(master_location_id);
		l_view_which_location.Text					= ilm.name;
		Session["inv_master_location_id"]			= master_location_id;
		gv_current_mappings.DataBind();
		}
	protected void cb_merge_location_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var origin_id						= Convert.ToInt32(Session["inv_master_location_id"]);
		var destination_id					= Convert.ToInt32(e.Parameter);
		// Get a list of all current mappings for the origin_id
		var _origin					= Toolbox.doSQL_dt(@"SELECT * FROM inventory_location WHERE location_master_id = @v0  ORDER BY master_id", new object[] {  origin_id } );
		var _destination				= Toolbox.doSQL_dt(@"SELECT * FROM inventory_location WHERE location_master_id = @v0 ", new object[] {  destination_id } );
		// Loop through list
		if(_origin.Rows.Count > 0)
			{
			foreach(DataRow dr in _origin.Rows)
				{
				var id				= Convert.ToInt32(dr["id"]);
				var master_id		= Convert.ToInt32(dr["master_id"]);
				var business_unit_id		= Convert.ToInt32(dr["business_unit_id"]);
				var qty			= Convert.ToDouble(dr["qty"]);
				var min			= Convert.ToDouble(dr["min"]);
				var max			= Convert.ToDouble(dr["max"]);
				// Check to see if the mapping exists in the destination AND the origin
				if(_destination.Select("master_id = "+master_id).Count() > 0 && qty > 0)
					{
					// If so, just append the quantity... make sure to denote this in the log table.
					var dest_qty		= Convert.ToDouble(_destination.Select("master_id = "+master_id)[0]["qty"]) + qty;
					var dest_id			= Convert.ToInt32(_destination.Select("master_id = "+master_id)[0]["id"]);
					var il		= new location(id);
					// Update the location table
					il.xfer(id, dest_id, qty, current_user);
					il					= new location(id);
					il.delete();
					}
				else
					{ 
					var dest_id				= 0;
					var il			= new location();
					if(_destination.Select("master_id = "+master_id).Count() == 0)
						{
						il.location_master_id	= destination_id;
						il.master_id			= master_id;
						il.member_id			= current_user.id;
						il.min					= min;
						il.max					= max;
						il.qty					= 0;
						il.alert_worthy			= false;
						il.business_unit_id			= business_unit_id;
						il.save(); 
						dest_id				= (int) il.id;
						}
					else
						{
						dest_id				= Convert.ToInt32(_destination.Select("master_id = "+master_id)[0]["id"]);
						}
					il						= new location(id);
					if(qty > 0)
						{
						il.xfer(id, dest_id, qty, current_user);
						il					= new location(id);
						}
					il.delete();
					}
				}
			e.Result		= "SUCCESS";
			}
		else
			{
			e.Result		= "Nothing to merge";
			}
		}
	protected void cb_delete_location_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var template_id					= Session["inv_master_location_id"].ToString();
		var template					= Toolbox.doSQL_dt(@" SELECT a.id, a.master_id FROM inventory_location a WHERE a.location_master_id = @v0  AND a.qty = 0 ORDER BY a.id", new object[] {  template_id } );
		var sb					= new StringBuilder();
		foreach(DataRow dr in template.Rows)
			{
			var il			= new location(Convert.ToInt32(dr["id"]));
			try
				{
				il.delete();
				}
			catch (Exception ee)
				{
				sb.AppendFormat("Cannot delete link to {0} - {1}\n", il.master_id, ee.Message);
				}
			}
		e.Result				= template.Rows.Count > 0 && sb.Length == 0 ? "SUCCESS" : sb.Length > 0 ? sb.ToString() : "Nothing to delete";
		}
	protected void cb_copy_location_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var template_id					= Session["inv_master_location_id"].ToString();
		var destination_id				= e.Parameter;
		var template					= Toolbox.doSQL_dt(@" SELECT a.id FROM inventory_location a WHERE a.location_master_id = @v0  AND a.master_id NOT IN (SELECT master_id FROM inventory_location WHERE location_master_id = @v1 ) ORDER BY a.id", new object[] {  template_id, destination_id } );
		foreach(DataRow dr in template.Rows)
			{
			var id					= Convert.ToInt32(dr["id"]);
			var il			= new location(id);
			il.id					= null;
			il.location_master_id	= Convert.ToInt32(destination_id);
			il.save();
			var l					= new NELog();
			l.section_id			= OpsLog.Section.LocationMaster;
			l.business_unit_id		= WarehouseBusinessUnit.id;
			l.member_id				= current_user.id;
			l.is_manual				= false;
			l.table					= OpsLog.Table.InventoryLocation;
			l.table_id				= il.id;
			l.alt_table_id			= il.master_id;
			// Save Min
			l.action_id				= OpsLog.Action.AdjustedMinimumQuantity;
			l.value_old				= 0;
			l.value_new				= il.min;
			l.save();
			// Save Max
			l.action_id				= OpsLog.Action.AdjustedMaximumQuantity;
			l.value_old				= 0;
			l.value_new				= il.max;
			l.save();
			// Save qty
			l.action_id				= OpsLog.Action.AdjustedLocationQuantity;
			l.value_old				= 0;
			l.value_new				= il.qty;
			l.save();
			}
		e.Result				= template.Rows.Count > 0 ? "SUCCESS" : "Nothing to copy";
		}
	protected void bt_gv_export_Click(object sender, EventArgs e)
		{
		b_gv_export.WriteXlsxToResponse(true);
		}
	protected void gv_view_current_mappings_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		if(e.Parameters.Contains('|'))
			{
			var gv			= (ASPxGridView) sender;
			var p				= e.Parameters.Split('|');
			var id					= Convert.ToInt32(p[1]);
			var il			= new location(id);
			il.delete();
			gv.DataBind();
			}
		}
	protected void bt_delete_Init(object sender, EventArgs e)
		{
		var b							= (ASPxButton) sender;
		var tc	= (GridViewDataItemTemplateContainer) b.NamingContainer;
		var dr								= tc.Grid.GetDataRow(tc.VisibleIndex);
		if(dr != null)
			{
			var qty								= Convert.ToDouble(dr["qty"]);
			if(qty == 0 && is_purchaser)
				{
				b.ClientSideEvents.Click			= string.Format("function(s,e){{gv_view_current_mappings.PerformCallback('d|{0}')}}", dr["id"]);
				}
			else
				{
				b.ToolTip							= is_purchaser ? "Location has an existing quantity; Can't delete" : "";
				b.Enabled							= false;
				b.Visible							= is_purchaser;
				}
			}
		}
	protected void bt_export_Click(object sender, EventArgs e)
		{
		var c											= new NeBusinessUnit(Session["working_business_unit_id"]);
		gv_master_locations.Columns["Action"].Visible		= false;
		gve_master_locations.FileName	= "master_locations_list_for_"+c.name.Replace(" ", "_");
		gve_master_locations.WriteXlsxToResponse();
		gv_master_locations.Columns["Action"].Visible		= true;
		}
}