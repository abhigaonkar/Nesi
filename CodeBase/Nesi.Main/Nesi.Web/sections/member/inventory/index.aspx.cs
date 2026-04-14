using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using DevExpress.Web;
using DevExpress.Web.Data;
using System.Text;
using System.Threading;
using DevExpress.Xpo;
using NESI.Common.Models;
using nesi.core;
//using nesi.bv;

public partial class branch_inventory : Page
	{
	public NeMember current_user;
	JavaScriptSerializer JSON				= new JavaScriptSerializer();
	private const int page_id			= 43; // from Page table in DB
	public const string page_description	= "Inventory / Branch Access";
    public string selected_master;
    public const string _page_name			= "BranchInventory";
	protected bool is_purchaser				= false;
	protected bool is_admin				= false;
	protected bool can_qty_tab				= false;
	protected bool can_adjust_stock			= false;
	ASPxHiddenField _h;
	SqlDataSource _ds_templates;
	ASPxDropDownEdit _dde_filter;
	Panel _panel_export;
	NeBusinessUnit workingBusinessUnit;
	NeBusinessUnit warehouseBusinessUnit;

	location _loc;
	location_master _loc_m;
	branch_options _bo_obj; 
    protected void Page_Init(object _sender, EventArgs _e)
		{
		current_user						= Toolbox.do_handle_authentication(page_id);
		can_adjust_stock					= current_user.AuthenticatedForPrivilege(104);
		layout.__page_name					= _page_name;
		if(Session["working_business_unit_id"] == null)
			{
			Session.Add("working_business_unit_id", current_user.business_unit_id);
			}
		if(Session["working_warehouse_bu_id"] == null)
			{
			Session.Add("working_warehouse_bu_id", current_user.business_unit.warehouse_bu_id);
			}
		
		var q								= Request.QueryString;
		_h									= (ASPxHiddenField) layout.FindControl("h");
		_ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		_dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		_panel_export						= (Panel) layout.FindControl("panel_export");
		workingBusinessUnit						= new NeBusinessUnit(Session["working_business_unit_id"]);
		warehouseBusinessUnit					= new NeBusinessUnit(Session["working_warehouse_bu_id"]);

		uc_home_tab.current_user					= current_user;
		uc_home_tab.WarehouseBusinessUnit			= warehouseBusinessUnit;
		uc_home_tab.WorkingBusinessUnit				= workingBusinessUnit;

		uc_part_transfer.current_user				= current_user;
		uc_part_transfer.WarehouseBusinessUnit		= warehouseBusinessUnit;
		uc_part_transfer.WorkingBusinessUnit		= workingBusinessUnit;

		uc_vdi.current_user							= current_user;
		uc_vdi.WarehouseBusinessUnit				= warehouseBusinessUnit;
		uc_vdi.WorkingBusinessUnit					= workingBusinessUnit;

		uc_orders_tab.current_user					= current_user;
		uc_orders_tab.WarehouseBusinessUnit			= warehouseBusinessUnit;
		uc_orders_tab.WorkingBusinessUnit			= workingBusinessUnit;

		// Check if the stock adjustment period has elapsed
		_bo_obj									= new branch_options(workingBusinessUnit.id);
		if(_bo_obj.stk_adj_enddt.Year > 1965 && _bo_obj.stk_adj_enddt < DateTime.Now)
			{
			_bo_obj.stop_adjustment_period(1);
			}
		///
		if(!IsPostBack && !IsCallback && q["a"] == "orders")
			{
			Session["orders_customer_id"] = null;
			Session["orders_dept_id"] = null;
			Session["orders_rfq_id"] = null;
			}
		if(q["a"] == "orders" || q["a"] == "qty")
			{
			layout.used_gv												= q["a"] == "orders" ? uc_orders_tab.gvorders : gv_minmax;
			layout.__page_name											= string.Format("{0}_gv_{1}", _page_name, (q["a"] == "orders" ? "orders" : "minmax"));
			page_name													= layout.__page_name;
			_ds_templates.SelectParameters["@page_name"].DefaultValue	= layout.__page_name;
			_ds_templates.SelectParameters["@member_id"].DefaultValue	= current_user.id.ToString();
			}
		}
	string page_name						= "";
    protected void Page_Load(object _sender, EventArgs _e)
		{
		using(var conn = Toolbox.connect())
			{
		#region Variable Declaration
		var q								= Request.QueryString;
		is_admin							= current_user.AuthenticatedForPage(49);
		is_purchaser						= current_user.AuthenticatedForPrivilege(71);
		can_qty_tab							= current_user.AuthenticatedForPrivilege(162);
		//gv_orders.Columns["Save"].Visible	= button_manageprices.Visible = is_purchaser;
		var f				= Request.Form;
		var menu							= new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(current_user);
		var lbltemp						= (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text						= page_description;
		var ico					= new consignment();
		var _dt						= new DataTable();
		#region groups postback
		if(IsPostBack && q["a"] != null &&  q["a"] == "groups")
			{
			group_hdrs.Visible				= true;
			temp_master_id.Value			= group_part_search.Text;
			if(temp_master_id.Value == "")
				{
				_dt		= Toolbox.doSQL_dt(conn,@" SELECT id, URLDECODE(name) name, get_name(member_id) created_by, edited_dt, (SELECT COUNT(master_id) FROM inventory_group_dtl b  WHERE b.group_id = a.id) n_parts, URLDECODE(notes) Notes FROM inventory_group_hdr a" , null);
				}
			else
				{
				_dt		= Toolbox.doSQL_dt(conn,@"SELECT id, URLDECODE(name) name, get_name(member_id) created_by, edited_dt, (SELECT COUNT(master_id) FROM inventory_group_dtl b WHERE b.group_id = a.id) n_parts, notes Notes FROM inventory_group_hdr a WHERE id IN (SELECT DISTINCT(group_id) FROM inventory_group_dtl WHERE master_id = @v0 )", new object[] {  temp_master_id.Value } );
				}
			if(_dt.Rows.Count > 0)
				{
				group_hdrs.DataSource			= _dt;			
				group_hdrs.DataBind();
				}
			}
		else if(!IsPostBack && q["a"] != null &&  q["a"] == "groups")
			{
			group_hdrs.Visible				= true;
			_dt		= Toolbox.doSQL_dt(conn,@"SELECT id, URLDECODE(name) name, get_name(member_id) created_by, edited_dt, (SELECT COUNT(master_id) FROM inventory_group_dtl b  WHERE b.group_id = a.id) n_parts, notes Notes FROM inventory_group_hdr a" , null);
			if(_dt.Rows.Count > 0)
				{
				group_hdrs.DataSource			= _dt;			
				group_hdrs.DataBind();
				}
			}
		#endregion groups postback
		#region kitted postback
        var strsql = "SELECT " +
	                    "inventory_kit_hdr_id id, "+
	                    "inventory_kit_hdr_name name, "+
	                    "b.member_fullname created_by, "+
	                    "inventory_kit_hdr_edited_dt edited_dt, "+
	                    "(SELECT COUNT(inventory_kit_dtl_master_id) FROM inventory_kit_dtl b WHERE b.inventory_kit_dtl_hdr_id = a.inventory_kit_hdr_id) n_parts, "+
	                  //  "(SELECT GetKittedSell(1," + WorkingBusinessUnit.id + ",a.inventory_kit_hdr_id)) sell,"+
	                    "inventory_kit_hdr_note Notes ";

		if(IsPostBack && q["a"] != null &&  q["a"] == "kitted")
			{
			kitted_hdrs.Visible				= true;
			temp_master_id.Value			= kitted_part_search.Text;
			if(temp_master_id.Value == "")
				{
					_dt = Toolbox.doSQL_dt(conn, strsql + "FROM inventory_kit_hdr a LEFT JOIN member b ON a.inventory_kit_hdr_created_by = b.member_id",null);
				}
				else
				{
					_dt = Toolbox.doSQL_dt(conn, string.Format("{0}FROM inventory_kit_hdr a LEFT JOIN member b ON a.inventory_kit_hdr_created_by = b.member_id WHERE a.inventory_kit_hdr_id IN (SELECT DISTINCT(inventory_kit_dtl_hdr_id) FROM inventory_kit_dtl WHERE inventory_kit_dtl_master_id = '{1}')", strsql, temp_master_id.Value),null);
				}
				kitted_hdrs.DataSource			= _dt;
			kitted_hdrs.DataBind();
			}
		else if(!IsPostBack && q["a"] != null &&  q["a"] == "kitted")
			{
			kitted_hdrs.Visible				= true;
				_dt = Toolbox.doSQL_dt(conn, strsql + "FROM inventory_kit_hdr a LEFT JOIN member b ON a.inventory_kit_hdr_created_by = b.member_id",null);
			if (_dt.Rows.Count > 0)
				{
				kitted_hdrs.DataSource			= _dt;
				kitted_hdrs.DataBind();
				}
			}
		#endregion kitted postback
    	string output						= null;
		string aid							= null;
		string value_id;
		string value_name					= null;
		string code							= null;
		string master_id					= "0";
		string action						= null;
		string attribute_id					= null;
		string attribute_name				= null;
		var member_id						= current_user.id;
		string TagID						= null;
		string tag_name						= null;
		var inv_class					= new functions(current_user);
		var show_cost						= is_purchaser || current_user.AuthenticatedForPrivilege(58);
		inv_class.WorkingBusinessUnit		= workingBusinessUnit;
		inv_class.WarehouseBusinessUnit		= warehouseBusinessUnit;
		var inventory					= new inventory();
		var dsn							= workingBusinessUnit.DSN;		
		var returned_string				= "";
		var br_obj				= new branch();
		DataTable subdt					= null;
		button_location.Visible							= is_purchaser;
		button_manageprices.Visible						= is_purchaser;
		button_newitem.Visible							= is_purchaser;
		button_orders.Visible							= is_purchaser;
		button_move_parts.Visible						= current_user.AuthenticatedForPrivilege(177);
		button_rfqs.Visible								= can_qty_tab;
		if(Session["inv_branch_include_external"] == null)
			{
			Session.Add("inv_branch_include_external", 0);
			}
		else if(!IsPostBack && q["a"] == "orders")
			{
			Session["inv_branch_include_external"]		= 0;
			}

		NeGridLayouts gl;
		if(gv_minmax.Visible)
			{
			fill_gv_minmax();
			}
		if(orders_panel.Visible)
			{
			uc_orders_tab.fill_gv_orders();
			}
		if(pnl_home.Visible)
			{
			uc_home_tab.fill_gv_merge();
			uc_home_tab.fill_gv_tocreate();
			}

		#endregion Variable Declaration

			if (Request.HttpMethod == "POST")
				{
				#region POST

				if (f["a"] != null)
					{
					action = f["a"];
					}
				if (action == "newpart")
					{
					
					Toolbox.do_set_plain_header(Response);
					var pl = f["payload"];
					var payload = JSON.Deserialize<new_part_obj>(pl);

					if (payload.tag_id > 0 && payload.att_vals.Length > 0)
						{
						var n_prices = payload.prices.Length;
						if(n_prices == 0)
							{
							throw new Exception("Cannot create a part without pricing");
							}
						master_id = "0";
						master_id = inventory.NewPart(payload.tag_id, string.Join(",", payload.att_vals), current_user.id, false);
						var is_number = new Regex(@"^\d+$");
						var success = is_number.Match(master_id).Success;
						if (success)
							{
							var should_be_qced = false;
							try
								{
								should_be_qced = Convert.ToBoolean(Toolbox.doSQL_int(conn, @"
SELECT 
	COUNT(*)
FROM inventory_item_master 
WHERE 
	master_id IN  (
				  SELECT
					master_id 
				  FROM inventory_item_detail a 
				  LEFT JOIN inventory_attribute_value b ON a.attribute_value_id = b.attribute_value_id 
				  WHERE 
					b.approved = false
				  ) AND 
master_id = @v0", new object[] { master_id}));
								}
							catch (Exception ee)
								{
								throw ee;
								}

							#region bypass qc

							if (payload.tag_id == OpsSpecialTag.NonStockItem)
								{
								should_be_qced = false;
								}
							if (!should_be_qced)
								{
								Toolbox.doSQL_void(conn, @"UPDATE inventory_item_master SET approved = 1, approved_by = 1 WHERE master_id = @v0 LIMIT 1", new object[] { master_id});
								}

							#endregion bypass qc
							
							if (payload.prices.Length > 0)
								{
								foreach(var vp in payload.prices)
									{
									var vpr = new vendor_price_row();
									vpr.cost = vp.CostPrice;
									vpr.total = vp.CostTotal;
									vpr.master_id = master_id;
									vpr.vendor_id = vp.VendorId;
									vpr.vendor_code = vp.VendorPartNo;
									vpr.qty = vp.Quantity;
									vpr.member_id = current_user.id;
									vpr.business_unit_id = warehouseBusinessUnit.id;
									try
										{
										vpr.save();
										}
									catch (Exception ee)
										{
										Response.Write(ee.Message);
										Response.End();
										}
									}
								}
							}
						Response.Write(master_id);
						}
					else
						{
						Response.Write("INVALID REQUEST");
						}
					Response.End();
					}
				else if (action == "update_picture")
					{
					Response.Clear();
					inv_class.picture_save();
					}
				else if (action == "mass_po_cut")
					{
					
					var parts = JSON.Deserialize<IList<po>>(f["parts"]);
					try
						{
						var errored_lines = new List<string>();
						if(parts[0].data[0].bu_id != warehouseBusinessUnit.id)
							{
							Response.Write("Your working business unit has changed since you loaded this page.. please either refresh the page, or change the working business unit in a different tab.");
							Response.End();
							return;
							}
						for (var i = 0; i < parts.Count; i++)
							{
							var p = parts[i];
							for (var j = 0; j < parts[i].data.Length; j++)
								{
								// Need to do a dupe check before even trying to cut a PO
								var li = parts[i].data[j];
								var existing_vendor_codes = Toolbox.doSQL_string(conn, @"SELECT IFNULL(GROUP_CONCAT(a.master_id),'') 
FROM inventory_price a LEFT JOIN inventory_item_master b 
ON a.master_id = b.master_id WHERE a.vendor_code = @v0 AND a.vendor_id = @v1 AND a.master_id != @v2 
AND b.active = true", new object[] { li.vendor_code, p.id, li.master_id});
								if (existing_vendor_codes != "")
								    {
								    string inv_admin = shared.get_nesi_member_based_on_function("INVENTORY_ADMINISTRATOR").FullName + " at " +
								                       shared.get_nesi_member_based_on_function("INVENTORY_ADMINISTRATOR").NEEmail;

                                    errored_lines.Add(string.Format("The vendor code for part #{0} already exists, for the same vendor in the NESI part database, on these part(s): {1}.  Please contact {2}", li.master_id, existing_vendor_codes, inv_admin));
									}
								}
							}
						if (errored_lines.Count > 0)
							{
							Response.Write(string.Join("\n", errored_lines.ToArray()));
							Response.End();
							return;
							}
						var po_email_body = "";
						var vendors_used = new Dictionary<int, int>();
                        var open_windows_script = "";
						for (var i = 0; i < parts.Count; i++)
							{
							var p = parts[i];
							var po_number = "0";
							var vendor_obj = new NEVendor(p.id);
							var current_pos = Toolbox.doSQL_dt(conn,@"SELECT a.poprog_id FROM poprog_header a LEFT JOIN po_details_current b ON b.po_details_poprog_id = a.poprog_id WHERE a.business_unit_id = @v1  AND a.poprog_status = 1 AND a.poprog_vendor_id = @v0  LIMIT 1", new object[] {  p.id, workingBusinessUnit.id } );
							var poprog = new NePOProg();
							if (current_pos.Rows.Count == 0 || (p.cut_new && !vendors_used.ContainsKey(p.id)))
								{
								try
									{
									// Cut New One.

									#region SAVE NESI RECORD

									poprog.poprog_status = 1;
									poprog.poprog_order_placed_date = Toolbox.MySQLNow_long();
									poprog.poprog_total_cost = 0;
									poprog.poprog_total_recCost = 0;
									poprog.poprog_vendor_id = p.id;
									poprog.poprog_cutby_member_id = current_user.id;
									poprog.business_unit_id =  workingBusinessUnit.id;
									poprog.poprog_customer_id = 0;
									poprog.poprog_cutdate = Toolbox.MySQLNow_long();
									poprog.poprog_ack_req = 0;
									poprog.poprog_ack_req_rec = 0;
									poprog.poprog_shipping_method = 1;
									poprog.location_master_id = 0;
									poprog.poprog_shipping_address_id = 0;

                             

                                    var this_country = workingBusinessUnit.country == "CDN" ? "Canada" : "USA";

									var branch_address = string.Format(
										@"{0}
{1}
{2}
{3}, {4}
{5}
",
										workingBusinessUnit.name, // {0}
										workingBusinessUnit.address, // {1}
										workingBusinessUnit.city, // {2}
										workingBusinessUnit.Prov, // {3}
										workingBusinessUnit.postal, // {4}
										this_country // {5}
										);
                                   
                                    poprog.Shipping_Addr1 = workingBusinessUnit.address;
                                    poprog.Shipping_Addr2 = "";
                                    poprog.Shipping_City = workingBusinessUnit.city;
                                    poprog.Shipping_Country = this_country== "Canada"? "CDN":"USA";
                                    poprog.Shipping_Postal = workingBusinessUnit.postal;
                                    poprog.Shipping_Province = workingBusinessUnit.Prov;

                                    poprog.poprog_manual_shipaddress = NePOProg.GetShippingAddress(poprog.poprog_id);
									poprog.poprog_ship_note_req = 0;
									poprog.poprog_ship_note_req_rec = 0;
									poprog.poprog_invoice_number = "";
									poprog.poprog_packing_slip_scan_date = "";
									poprog.poprog_order_description = "";
									var two_days = DateTime.Now.AddDays(2);
									if (two_days.DayOfWeek == DayOfWeek.Saturday || two_days.DayOfWeek == DayOfWeek.Sunday)
										{
										two_days = two_days.AddDays(2);
										}
									poprog.poprog_date_required = Toolbox.MySQL_longdt(two_days);
									poprog.poprog_expected_received_date = Toolbox.MySQL_longdt(two_days);
									poprog.poprog_country_code_currency = workingBusinessUnit.default_currency.ToString();
									poprog.poprog_order_description = "Order for Stock";
									poprog.edit_after_issue = 1;
									poprog.POProg_Save();
									if (p.cut_new)
										{
										vendors_used.Add(p.id, poprog.poprog_id);
										}

									#endregion SAVE NESI RECORD

									//#region SAVE BV RECORD

									//var obj_po = new PurchaseOrder();
									//obj_po.VendorNumber = vendor_obj.Number;
									//obj_po.VendorOrderNumber = "0000000000";
									//var obj_poi = new PurchaseOrderItem();
									//obj_poi.Warehouse = "";
									//obj_poi.PartNumber = "";
									//obj_poi.ProductDescription = poprog.poprog_order_description;
									//obj_po.LineItems.Add(obj_poi);
									//obj_po.RequiredDate = DateTime.Now;
									//obj_po.PODate = DateTime.Now;
									//obj_po.Status = "O";
									//obj_po.ReferenceNumber = poprog.poprog_id.ToString();
									//obj_po.Discount = 0;
									//obj_po.TermsCode = "/";
									//obj_po.TermsDescription = "";
									//obj_po.ShippingMethodCode = "/";
									//obj_po.ShippingMethod = "";
									//obj_po.Tax1Exempt = "";
									//obj_po.Tax2Exempt = "/";
									//obj_po.Freight = 0;
									//try
									//	{
									//	obj_po.PONumber = poprog.poprog_id.ToString().PadLeft(10, '0');
									//	obj_po.Add_New(workingBusinessUnit.DSN, current_user.Initials);
									//	}
									//catch (Exception ee)
									//	{
									//	throw ee;
									//	}

									//#endregion SAVE BV RECORD
									
									#region UPDATE NESI WITH BV PO

									Toolbox.doSQL_void(conn, @"UPDATE poprog_header SET poprog_bvpo = @v0 WHERE poprog_id = @v1 LIMIT 1", new object[] {poprog.poprog_id.ToString().PadLeft(10, '0'), poprog.poprog_id});

									#endregion UPDATE NESI WITH BV PO

									poprog.UpdatePOTotals();
									poprog = new NePOProg(poprog.poprog_id);
									po_number = poprog.poprog_bvpo;
//									po_email_body += string.Format("PO #{0} Cut,", po_number);
                                    po_email_body += string.Format("PO #{0} Cut,", "<a href='/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=" + po_number + "' target='_blank' title='" + new NEVendor(poprog.poprog_vendor_id).Vendor_Name + "'>" + po_number + "</a>");

                                    //if(poprog.poprog_total_cost < 2000 && poprog.poprog_status != 5)
                                    //	{
                                    //	Toolbox.doSQL_void(conn,@"UPDATE poprog_header SET poprog_status = '5' WHERE poprog_id = @v0  LIMIT 1", new object[] {  poprog.poprog_id } );
                                    //	}
                                }
                                catch (Exception ee)
									{
									
									throw;
									}
								}
							else if (p.cut_new && vendors_used.ContainsKey(p.id))
								{
								poprog = new NePOProg(vendors_used[p.id]);
								po_number = poprog.poprog_bvpo;
								po_email_body += string.Format("Using PO #{0} -  didn't cut a new one,", "<a href='/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=" + po_number + "' target='_blank' title='" + new NEVendor(poprog.poprog_vendor_id).Vendor_Name + "'>" + po_number + "</a>");

                                }
							else
								{
								var current_po = current_pos.Rows[0];
								poprog = new NePOProg(Convert.ToInt32(current_po["poprog_id"]));
								po_number = poprog.poprog_bvpo;
                                po_email_body += string.Format("Using PO #{0} - didn't cut a new one,", "<a href='/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=" + po_number + "' target='_blank' title='" + new NEVendor(poprog.poprog_vendor_id).Vendor_Name + "'>" + po_number + "</a>");
                            }
                            for (var j = 0; j < parts[i].data.Length; j++)
								{
								var li = parts[i].data[j];
								inventory.Load(li.master_id, warehouseBusinessUnit.id);
								var po_dc = new NEPO_Details_Current();
								// Does a line item already exist on this PO for this part?
								var po_lines = Toolbox.doSQL_dt(conn,@"SELECT po_details_id FROM po_details_current WHERE po_details_part_no = @v0  AND po_details_poprog_id = @v1 ", new object[] {  li.master_id, poprog.poprog_id } );
								if (po_lines.Rows.Count == 0)
									{
									#region SAVE NEW PO LINE ITEM
									po_dc.po_details_part_no = li.master_id;
									po_dc.po_details_vendor_part_no = li.vendor_code;
									po_dc.po_details_description = inventory.description;
									po_dc.po_details_notes = "";
									po_dc.po_details_qty_received = 0;
									po_dc.po_details_sell_price = 0;
									po_dc.po_details_date_expected = Toolbox.MySQLNow_long();
									po_dc.po_details_cost = li.cost;
									po_dc.po_details_tax1 = workingBusinessUnit.Tax1 > 0 ? 1 : 0;
									po_dc.po_details_tax2 = workingBusinessUnit.Tax2 > 0 ? 1 : 0;
									po_dc.po_details_tax3 = workingBusinessUnit.Tax3 > 0 ? 1 : 0;
									po_dc.po_details_tax4 = workingBusinessUnit.Tax4 > 0 ? 1 : 0;
									po_dc.po_details_poprog_id = poprog.poprog_id;
									// Check if any work orders need this part... there can only be one.
									var c = Toolbox.doSQL_int(conn, @"
SELECT 
	COUNT(*) 
FROM 
	wo_detail_current a 
LEFT JOIN 
	woprog b 
		ON a.wo_detail_current_woprog_id = b.woprog_id 
WHERE 
	a.business_unit_id = @v0 AND 
	(a.wo_detail_current_qty_ordered - a.wo_detail_current_qty_committed) > 0 AND 
	b.woprog_status = 'Open' AND
	b.woprog_hold != TRUE AND 
	a.wo_detail_current_master_id = @v1", new object[] { workingBusinessUnit.id, li.master_id});
									var woprog_id = 9999999;
									if (c == 1)
									    {
										woprog_id = Toolbox.doSQL_int(conn, @"
SELECT 
	wo_detail_current_woprog_id 
FROM 
	wo_detail_current a 
LEFT JOIN 
	woprog b 
		ON a.wo_detail_current_woprog_id = b.woprog_id 
WHERE 
	a.business_unit_id = @v0 AND 
	(a.wo_detail_current_qty_ordered - a.wo_detail_current_qty_committed) > 0 AND 
	b.woprog_status = 'Open' AND
	b.woprog_hold != TRUE AND 
	a.wo_detail_current_master_id = @v1 
LIMIT 1", new object[] { workingBusinessUnit.id, li.master_id});
										}
									po_dc.is_gl_account = false;
									po_dc.po_details_woprog_id = woprog_id;
									po_dc.po_details_date_added = Toolbox.MySQLNow_long();
									po_dc.po_details_date_modified = Toolbox.MySQLNow_long();
									po_dc.po_details_add_member_id = (int) current_user.id;
									po_dc.po_details_audit_member_id = (int) current_user.id;
									po_dc.po_details_qty_orderd = li.to_order;
									po_dc.po_details_vendor_qty_per = li.qty;
									    po_dc.business_unit_id = workingBusinessUnit.id;

                                    po_dc.PO_Details_Save();
									//po_email_body							+= string.Format("Quantity of {2} for part {0} added to PO#:{1},", li.master_id, po_number, po_dc.po_details_qty_orderd);

									#endregion SAVE NEW PO LINE ITEM
									}
								else if (po_lines.Rows.Count == 1)
									{
									var po_row = po_lines.Rows[0];
									po_dc.po_details_current_line(Convert.ToInt32(po_row["po_details_id"]));
									po_dc.po_details_id = Convert.ToInt32(po_row["po_details_id"]);
									var prev_qty = po_dc.po_details_qty_orderd;
									po_dc.po_details_qty_orderd += li.to_order;
									po_dc.po_details_date_modified = Toolbox.MySQLNow_long();
									po_dc.PO_Details_Update();
									//po_email_body							+= string.Format("Quantity of {2} for part {0} added to existing quantity of {3}, for PO#: {1}<br/>", li.master_id, po_number, li.to_order, prev_qty);
									}
								}
                           
                           


                            }
                      
                        Response.Write("SUCCESS|" + po_email_body);

						}
					catch (Exception ee)
						{
						if (!(ee is ThreadAbortException))
							{
							
							Response.Write(ee.ToString());
							}
						}
					Session["inv_gv_orders"] = null;
					uc_orders_tab.fill_gv_orders();
                  
                    Response.End();
					}
				else if (action == "mass_save_mmo")
					{
					var mmos = JSON.Deserialize<IList<minmaxobj>>(f["mmos"]);
					try
						{
						for (var i = 0; i < mmos.Count; i++)
							{
							//_tools.debug_note(i);
							var ib = new branch(mmos[i].master_id, mmos[i].business_unit_id);
							var il = new location(mmos[i].location_master_id, mmos[i].business_unit_id, mmos[i].master_id);
							var prev_qty = il.qty;
							var this_cost = Toolbox.doSQL_double(conn,@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  mmos[i].master_id, mmos[i].business_unit_id } );
							il.log_is_manual = true;
							il.max = mmos[i].max;
							il.min = mmos[i].min;
							il.member_id = current_user.id;
							il.alert_worthy = true;
							var diff = il.qty - mmos[i].qty;
							il.qty = mmos[i].qty;
							il.section_id = 1;
							var this_type = prev_qty > mmos[i].qty ? 3 : 2;
							//_tools.debug_note(this_type+", "+ib.dollar_balance+", "+diff+", "+this_cost+", "+mmos[i].master_id+", "+mmos[i].location_master_id+", "+mmos[i].qty);
							//2, 1244.88, -100, 0, 808, 4112, 9676
							il.save();
							il.update_branch(this_type, ib.dollar_balance, diff, this_cost, ib);
							}
						Response.Write("SUCCESS");
						}
					catch (Exception ee)
						{
						Response.Write(ee.ToString());
						}
					Session["inv_gv_minmax"] = null;
					fill_gv_minmax();
					Response.End();
					}
				else if (action == "mass_delete_locations")
					{
					var delos = JSON.Deserialize<IList<dellocobj>>(f["mmos"]);
					var this_i = 0;
					try
						{
						for (var i = 0; i < delos.Count; i++)
							{
							this_i = i;
							var il = new location(delos[i].lm_id, warehouseBusinessUnit.id, delos[i].m_id);
							il.delete();
							}
						Response.Write("SUCCESS");
						}
					catch (Exception ee)
						{
						var il = new location(delos[this_i].lm_id, warehouseBusinessUnit.id, delos[this_i].m_id);
						var ilm = new location_master(il.location_master_id);
						if (ee.ToString().Contains("last location"))
							{
							Response.Write(string.Format("You cannot delete the last location ({0}) that is referenced by a part ({1})", ilm.name, il.master_id));
							}
						else
							{
							
							Response.Write(string.Format(ee + " : ({0})  from part: ({1}), the IT department has been emailed.", ilm.name, il.master_id));
							}
						}
					Session["inv_gv_minmax"] = null;
					fill_gv_minmax();
					Response.End();
					}
				else if (action == "rfq_dump")
					{
					Response.Clear();
					var ro = new rfq_obj();
					ro.rfq_id = Convert.ToInt32(f["r_id"]);
					ro.is_new = ro.rfq_id == 0;
					var warning = "";
// Take care of incoming vendors
					ro.vendor_id = new ArrayList();
					ro.vendor_id.AddRange(f["v"].Split(new char[] {','}));
// Take care of incoming parts
					var rop = JSON.Deserialize<IList<rfq_obj_line>>(f["p"]);
// Take care of RFQ header info
					var my_rfq = new VendorRFQ();
					if (ro.rfq_id == 0)
						{
						// New One
						my_rfq.name = current_user.FirstName + " - Name TBD";
						my_rfq.member_id = current_user.id;
						my_rfq.business_unit_id = warehouseBusinessUnit.id;
						my_rfq.date_open = DateTime.Now;
						my_rfq.date_close = DateTime.Now.AddDays(14);
						my_rfq.notes = "Added from Orders Tab by " + current_user.FullName;
						my_rfq.save();
						}
					else
						{
						my_rfq = new VendorRFQ(ro.rfq_id);
						}
// Create / Verify that vendors exist
					if (ro.is_new)
						{
						// Is New RFQ - Basic Vendor Dump
						for (var vi = 0; vi < ro.vendor_id.Count; vi++)
							{
							var this_v_id = Convert.ToInt32(ro.vendor_id[vi]);
							var vv = new NEVendor(this_v_id);
							var my_rv = new rfq_vendor();
							my_rv.rfq_header_id = my_rfq.id;
							my_rv.vendor_id = this_v_id;
							var code2 = Toolbox.do_RandomString(20);
							while (my_rv.key_code_exists(code2))
								{
								code2 = Toolbox.do_RandomString(20);
								}
							my_rv.keycode = code2;
							// As only the vendor id has been submitted, we need to supply a contact
							var n_contacts = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM contact
WHERE contact_cust_id =@v0  AND contact_type = 'Vendor' AND contact_status_id IN (1,8) AND contact_status = 'Active'", new object[] { this_v_id});
							if (n_contacts > 1)
								{
								warning += "There were " + n_contacts + " available contacts for " + vv.Name + " - used the first active contact.|";
								}
							my_rv.contact_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MIN(contact_id), 0) 
FROM contact WHERE contact_cust_id = @v0 AND contact_type = 'Vendor' AND contact_status_id IN (1,8) AND contact_status = 'Active'", new object[] { this_v_id });
							if (my_rv.contact_id == 0)
								{
								var c = new NEContact();
								c.Contact_Type = "Vendor";
								c.Contact_Cust_ID = this_v_id;
								c.Contact_Email = "fakeemail@fake.com";
								c.Contact_Name = "Default Contact";
								c.Contact_Status = "Active";
								c.Contact_Status_ID = 8;
								c.Save(c);
								my_rv.contact_id = c.Contact_ID;
								warning += "Default contact created for " + vv.Name + ".|";
								}
							my_rv.save();
							}
						}
					else
						{
						// Existing - Need to check if vendor already exists.
						for (var vi = 0; vi < ro.vendor_id.Count; vi++)
							{
							var this_v_id = Convert.ToInt32(ro.vendor_id[vi]);
							var my_rv = new rfq_vendor(this_v_id, ro.rfq_id);
							var vv = new NEVendor(this_v_id);
							if (!my_rv.does_exist)
								{
								my_rv.rfq_header_id = my_rfq.id;
								my_rv.vendor_id = this_v_id;
								var code2 = Toolbox.do_RandomString(20);
								while (my_rv.key_code_exists(code))
									{
									code2 = Toolbox.do_RandomString(20);
									}
								my_rv.keycode = code2;
								// As only the vendor id has been submitted, we need to supply a contact
								var n_contacts = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM contact
WHERE contact_cust_id = @v0 AND contact_type = 'Vendor' AND contact_status_id IN (1,8) AND contact_status = 'Active'", new object[] { this_v_id });
								if (n_contacts > 1)
									{
									warning += "There were " + n_contacts + " available contacts for " + vv.Name + " - used the first active contact.|";
									}
								my_rv.contact_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MIN(contact_id), 0) 
FROM contact WHERE contact_cust_id = @v0 AND contact_type = 'Vendor' AND contact_status_id IN (1,8) AND contact_status = 'Active'", new object[] { this_v_id });
								if (my_rv.contact_id == 0)
									{
									var c = new NEContact();
									c.Contact_Type = "Vendor";
									c.Contact_Cust_ID = this_v_id;
									c.Contact_Email = "fakeemail@fake.com";
									c.Contact_Name = "Default Contact";
									c.Contact_Status = "Active";
									c.Contact_Status_ID = 8;
									c.Save(c);
									my_rv.contact_id = c.Contact_ID;
									warning += "Default contact created for " + vv.Name + ".|";
									}
								my_rv.save();
								}
							}
						}
					foreach (var rol in rop)
						{
						var my_pl = new rfq_part_list();
						my_pl.rfq_header_id = my_rfq.id;
						my_pl.master_id = rol.id;
						if (my_pl.exists())
							{
							my_pl = new rfq_part_list(my_rfq.id, rol.id);
							}
						my_pl.qty = rol.q;
						my_pl.note = "";
						my_pl.save();
						}
					Response.Write("SUCCESS," + my_rfq.id + "," + warning);
					Response.End();
					}

					#endregion POST
				}
			else if (Request.HttpMethod == "GET")
				{

				if (q["id"] != null)
					{
					if (Session["working_master_id"] == null)
						{
						Session.Add("working_master_id", q["id"]);
						}
					else
						{
						Session["working_master_id"] = q["id"];
						}
					}

				#region action & disabler

				var disabler = "";
				if (current_user.AuthenticatedForPrivilege(153))
					{
					var bo_obj = new branch_options(warehouseBusinessUnit.id);
					var used_color = !bo_obj.stk_adj
						? "#19C637"
						: "#E31937";
					var used_words = !bo_obj.stk_adj
						? "Allow Stock Changes for 24hr<div style='font-size:9px;'>(Not currently active)</div>"
						: "Close Stock Changes<div style='font-size:9px;'>Expires " + bo_obj.stk_adj_enddt + "</div>";
					toggle_stock_adj.InnerHtml += "<button type='button' data-is_active='" + bo_obj.stk_adj + "' onclick='toggle_stock_changes(this)' style='font-size:11px;font-family:arial;color:#fff;font-weight:bold;padding:3px;background-color:" + used_color + ";height:auto;border:outset 2px #fff;width:auto'>" + used_words + "</button>";
					toggle_stock_adj.InnerHtml += @"
<script>
	function toggle_stock_changes(obj)
		{
		if(confirm('Are you sure?'))
			{
			$(obj).attr('disabled', true);
			var is_active			= $(obj).attr('data-is_active');
			var set_state			= is_active == 'False';
			please_wait('begin', 'Toggling Stock Adjustments');
			var vars			=	{
									a:				'toggle_stock_changes',
									state:			set_state
									}
			$.get('./index.aspx', vars,
					function(returned)
						{
						if(returned == 'SUCCESS')
							{
							location.href = location.href;
							}
						else
							{
							alert(returned);
							$(obj).attr('disabled', false);
							please_wait('stop');
							}
						});
			}
		}
</script>";
					}
				else if (is_purchaser)
					{
					var bo_obj = new branch_options(warehouseBusinessUnit.id);
					if (bo_obj.stk_adj)
						{
						toggle_stock_adj.InnerHtml += string.Format("<div style='font-size:10px;'><b>Stock adjustments open till:</b> {0}</div>", bo_obj.stk_adj_enddt);
						}
					}
				if (is_purchaser && current_user.AuthenticatedForPrivilege(4))
					{
					branch_box.InnerHtml = string.Format(@"<select id='branch_selector' onchange='working_branch(this)'>{0}</select>", branches_selector());
					}
				else if (is_purchaser)
					{
					branch_box.InnerHtml = string.Format("<input type='hidden' id='branch_selector' value='{0}'/>", warehouseBusinessUnit.id);
					}
				else
					{
					branch_box.InnerHtml = string.Format("<input type='hidden' id='branch_selector' value='{0}'/>", warehouseBusinessUnit.id);
					button_newitem.Disabled = true;
					disabler = " disabled";
					}
				action = q["a"] ?? "";

				#endregion action

				#region set_XML_header

				if (action.Contains("xml"))
					{
					//Toolbox.do_set_plain_header(Response);
					Toolbox.do_set_XML_header(Response);
					
					}

				#endregion set_XML_header

				if (action == "xml_attributes")
					{
					#region xml_attributes
					_dt = inv_class.attributes();
					if (_dt.Rows.Count > 0)
						{
						Response.Write(@"
		<attributes>");
						foreach (DataRow dr in _dt.Rows)
							{
							var xml_attribute_id = dr["attribute_id"].ToString();
							var xml_attribute = dr["attribute"].ToString();
							Response.Write(string.Format(@"
			<attribute id='{0}' name=""{1}"" />
		", xml_attribute_id, xml_attribute));
							}
						Response.Write(@"
		</attributes>");
						}
					Response.End(); 
					#endregion
					}
				else if (action == "xml_barcodes")
					{
					#region xml_barcodes
					Response.Clear();
					output = "<response>";
					master_id = q["master_id"] ?? "";
					if (master_id != string.Empty)
						{
						_dt = Toolbox.doSQL_dt(conn, @" SELECT a.id, a.master_id, IFNULL(a.barcode_no, '') barcode_no, IFNULL(a.table_type, '') table_type, a.table_id, IF(a.table_type = 'V', (SELECT b.vendor_name FROM vendor b WHERE b.vendor_id = a.table_id), (SELECT c.value FROM inventory_attribute_value c WHERE c.attribute_value_id = a.table_id)) ref_name, a.dt_created, a.member_id_added, MEMBER_NAME(a.member_id_added) `member_name`, a.is_active FROM inventory_barcode a WHERE master_id = @v0  ORDER BY dt_created DESC", new object[] { master_id });
						if (_dt.Rows.Count > 0)
							{
							foreach (DataRow dr in _dt.Rows)
								{
								var bc = new barcode
									{
									id = Convert.ToInt32(dr["id"]),
									master_id = Convert.ToInt32(dr["master_id"]),
									barcode_no = (string)dr["barcode_no"],
									table_type = (string)dr["table_type"],
									table_id = Convert.ToInt32(dr["table_id"]),
									ref_name = dr["ref_name"] == DBNull.Value ? "" : dr["ref_name"].ToString(),
									dt_created = dr["dt_created"].ToString(),
									member_id_added = Convert.ToInt32(dr["member_id_added"]),
									member_name = dr["member_name"].ToString(),
									is_active = Convert.ToBoolean(dr["is_active"])
									};
								output += string.Format(@"
	<row>
		<id>{0}</id>
		<master_id>{1}</master_id>
		<barcode_no>{2}</barcode_no>
		<table_type>{3}</table_type>
		<table_id>{4}</table_id>
		<ref_name>{5}</ref_name>
		<dt_created>{6}</dt_created>
		<member_id_added>{7}</member_id_added>
		<member_name>{8}</member_name>
		<is_active>{9}</is_active>
	</row>",
									bc.id, // {0}
									bc.master_id, // {1}
									bc.barcode_no, // {2}
									bc.table_type, // {3}
									bc.table_id, // {4}
									bc.ref_name, // {5}
									bc.dt_created, // {6}
									bc.member_id_added, // {7}
									bc.member_name, // {8}
									bc.is_active // {9}
									);
								}
							}
						else
							{
							output += "<error>No barcodes yet scanned for this part.</error>";
							}
						}
					else
						{
						output += "<error>Master ID not provided</error>";
						}
					output += "</response>";
					Response.Write(output);
					Response.End(); 
					#endregion
					}
				else if (action == "xml_tags")
					{
					#region xml_tags
					_dt = inv_class.approved_tags();
					if (_dt.Rows.Count > 0)
						{
						Response.Write(@"
<tags>");
						foreach (DataRow dr in _dt.Rows)
							{
							TagID = dr["tag_id"].ToString();
							tag_name = dr["tag"].ToString();
							Response.Write(string.Format(@"
	<tag id='{0}' name=""{1}"" />
", TagID, tag_name));
							}
						Response.Write(@"
</tags>");
						}
					Response.End(); 
					#endregion
					}
				else if (action == "xml_get_rfqs")
					{
					#region xml_get_rfqs
					var sb = new StringBuilder();
					_dt = Toolbox.doSQL_dt(conn, @"SELECT id,name FROM rfq_header  WHERE status IN ('OPEN', 'BUILDING') AND business_unit_id =@v0", new object[] { warehouseBusinessUnit.id });
					if (_dt.Rows.Count > 0)
						{
						sb.Append("\n<rfqs>");
						foreach (DataRow dr in _dt.Rows)
							{
							var rfq_id = dr["id"].ToString();
							var rfq_name = dr["name"].ToString();
							sb.AppendFormat(@"<rfq><id>{0}</id><name>{1}</name></rfq>", rfq_id, rfq_name);
							}
						sb.Append("\n</rfqs>");
						}
					Response.Write(sb.ToString());
					Response.End(); 
					#endregion
					}
				else if (action == "xml_values")
					{
					#region xml_values
					aid = q["aid"];
					_dt = inv_class.values(aid);
					if (_dt.Rows.Count > 0)
						{
						Response.Write(@"
		<values>");
						foreach (DataRow dr in _dt.Rows)
							{
							var xml_value_id = dr["attribute_value_id"].ToString();
							var xml_value = dr["value"].ToString();
							Response.Write(string.Format(@"
			<value id='{0}' name=""{1}"" />
		", xml_value_id, xml_value));
							}
						Response.Write(@"
		</values>");
						}
					Response.End(); 
					#endregion
					}
				else if (action == "selected_xml_values")
					{
					#region selected_xml_values
					aid = q["aid"];
					TagID = q["tag_id"];
					_dt = inv_class.selected_values(TagID, aid);
					if (_dt.Rows.Count > 0)
						{
						Response.Write(@"
					<values>");
						foreach (DataRow dr in _dt.Rows)
							{
							var xml_value_id = dr["attribute_value_id"].ToString();
							var xml_value = dr["value"].ToString();
							Response.Write(string.Format(@"
						<value id='{0}' name=""{1}"" />", xml_value_id, xml_value));
							}
						Response.Write(@"
					</values>");
						}
					else
						{
						Response.Write(@"
					<values>
					
					</values>
		");
						}
					Response.End(); 
					#endregion
					}
				else if (action == "available_xml_values")
					{
					#region available_xml_values
					aid = q["aid"];
					TagID = q["tag_id"];
					_dt = inv_class.available_values(TagID, aid);
					if (_dt.Rows.Count > 0)
						{
						Response.Write(@"
					<values>");
						foreach (DataRow dr in _dt.Rows)
							{
							var xml_value_id = dr["attribute_value_id"].ToString();
							var xml_value = dr["value"].ToString();
							Response.Write(string.Format(@"
						<value id='{0}' name=""{1}"" />
					", xml_value_id, xml_value));
							}
						Response.Write(@"
					</values>");
						}
					Response.End(); 
					#endregion
					}
				else if (action == "xml_browse_parts")
					{
					#region xml_browse_parts
					var sb = new StringBuilder();
					try
						{
						var tag_id = q["tag_id"];
						var attval_pattern = q["attvals"];
						var vendor_id = q["vendor_id"];
						var type = Convert.ToInt32(q["type"]);
						_dt = type == 999999 ? Toolbox.doSQL_dt(conn, @"SELECT b.master_id item, '' _pattern, b.active FROM inventory_price a LEFT JOIN inventory_item_master b ON a.master_id = b.master_id WHERE a.vendor_id = @v0  AND a.business_unit_id = @v1 ", new object[] { vendor_id, warehouseBusinessUnit.id})
							: inv_class.Match_Parts(tag_id, attval_pattern);
						sb.Append("<matches>");
						foreach (DataRow dr in _dt.Rows)
							{
							master_id = dr["item"].ToString();
							var attval = dr["_pattern"].ToString();
							var active = Convert.ToBoolean(dr["active"]);
							var description = Toolbox.doSQL_string(conn, @"SELECT full_part_description_wo_tag(@v0, false, @v1)", new object[] { master_id, warehouseBusinessUnit.id });
							tag_name = "";
							var vpr = new vendor_price_row(master_id, warehouseBusinessUnit.id, vendor_id);
							var vendor_code = "";
							double vendor_price = 0;
							double vendor_qty = 0;
							var lead_time = 7;
							var last_updated = "";
							var price_id = 0;
							if (vpr.exists)
								{
								vendor_code = vpr.vendor_code.ToString();
								vendor_price = Convert.ToDouble(vpr.cost);
								lead_time = Convert.ToInt32(vpr.lead_time);
								vendor_qty = Convert.ToDouble(vpr.qty);
								last_updated = vpr.last_edited.ToString("M/d/yyyy");
								price_id = Convert.ToInt32(vpr.price_id);
								}
							if (active)
								{
								sb.AppendFormat(@"
<part>
	<tag_id>{0}</tag_id>
	<tag_name>{1}</tag_name>
	<master_id>{2}</master_id>
	<description>{3}</description>
	<price_id>{4}</price_id>
	<vendor_code>{5}</vendor_code>
	<qty>{6}</qty>
	<cost>{7}</cost>
	<lead_time>{8}</lead_time>
	<last_updated>{9}</last_updated>
</part>",
									tag_id, // {0}
									tag_name, // {1}
									master_id, // {2}
									description, // {3}
									price_id, // {4}
									vendor_code, // {5}
									vendor_qty, // {6}
									vendor_price, // {7}
									lead_time, // {8}
									last_updated // {9}
									);
								}
							}
						sb.Append(@"</matches>");
						}
					catch (Exception ee)
						{
						sb.AppendFormat(@"<matches><part number='error'>{0}</part></matches>", ee);
						}
					Response.Write((string)sb.ToString());
					Response.End(); 
					#endregion
					}
				else if (action == "xml_tag_att_val")
					{
					#region xml_tag_att_val
					TagID = q["tag_id"];
					_dt = inv_class.attributes(TagID);
					subdt = null;
					if (_dt.Rows.Count > 0)
						{
						Response.Write("<attvals>");
						foreach (DataRow dr in _dt.Rows)
							{
							attribute_id = dr["attribute_id"].ToString();
							attribute_name = dr["attribute"].ToString();
							Response.Write(string.Format(@"
<attribute id=""{0}"" name=""{1}"">", attribute_id, HttpUtility.HtmlEncode(attribute_name)));
							subdt = inv_class.tag_attribute_selected_values(TagID, attribute_id);
							foreach (DataRow subdr in subdt.Rows)
								{
								value_id = subdr["attribute_value_id"].ToString();
								value_name = HttpUtility.HtmlEncode(subdr["value"]);
								Response.Write(string.Format(@"
	<value id='{0}' name=""{1}"" />", value_id, value_name));
								}
							Response.Write(@"
</attribute>");
							}
						Response.Write("\n</attvals>");
						}
					else
						{
						Response.Write(@"
<attvals>
<attribute id='0' name='Invalid Tag ID'>
<value id='0' name='No values available'/>
</attribute>
</attvals>");
						}
					Response.End(); 
					#endregion
					}
				else if (action == "xml_match_parts")
					{
					#region xml_match_parts
						var sb_match_parts	= new StringBuilder();
					try
						{
						var tag_id = q["tag_id"];
						var attval_pattern = q["attvals"];
							_dt = inv_class.Match_Parts(tag_id, attval_pattern);
						sb_match_parts.Append(@"<matches>");
						foreach (DataRow dr in _dt.Rows)
							{
							master_id = dr["item"].ToString();
							var attval = dr["_pattern"].ToString();
							var active = Convert.ToBoolean(dr["active"]);
							var description = Toolbox.do_value_from(dr["_description"]);
							tag_name = Toolbox.do_value_from(dr["_description"]);
							var vendor_code = "";
							try
								{
								vendor_code = Toolbox.doSQL_string(conn, @"SELECT IFNULL(MAX(vendor_code), '--') vendor_code 
FROM inventory_price WHERE master_id = @v0 AND business_unit_id = @v1 ORDER BY cost LIMIT 1", new object[] { master_id, warehouseBusinessUnit.id });
								}
							catch
								{
								vendor_code = "--";
								}
							if (active)
								{
								sb_match_parts.AppendFormat(@"
	<part master_id=""{2}"" tag_id=""{0}"" tag_name=""{1}"" description=""{3}"" active=""{4}"" attval=""{5}"" vendor_code=""{6}""/>",
									tag_id,
									tag_name,
									master_id,
									description,
									active,
									attval,
									HttpUtility.HtmlEncode(vendor_code)
									);
								}
							}
						sb_match_parts.Append(@"</matches>");
						}
					catch (Exception ee)
						{
						sb_match_parts.AppendFormat(@"<matches><part number='error'>{0}</part></matches>", ee);
						}
					Response.Write(sb_match_parts.ToString());
					Response.End(); 
					#endregion
					}
				else if (action == "prepop_xml_tag_att_val")
					{
					#region prepop_xml_tag_att_val
					TagID = q["tag_id"];
					_dt = inv_class.Browse_Attributes(TagID);
					if (_dt.Rows.Count > 0)
						{
						Response.Write("<attvals>");
						foreach (DataRow dr in _dt.Rows)
							{
							attribute_id = dr["attribute_id"].ToString();
							attribute_name = Toolbox.do_value_from(dr["attribute"]);
							Response.Write(string.Format(@"
			<attribute id=""{0}"" name=""{1}"">", attribute_id, attribute_name));
							subdt = inv_class.Browse_Values(TagID, attribute_id);
							foreach (DataRow subdr in subdt.Rows)
								{
								value_id = subdr["value_id"].ToString();
								value_name = Toolbox.do_value_from(subdr["value"].ToString());
								Response.Write(string.Format(@"
			<value id='{0}' name=""{1}"" />", value_id, value_name));
								}
							Response.Write(@"
			</attribute>");
							}
						Response.Write("\n</attvals>");
						}
					else
						{
						Response.Write(@"
		<attvals>
			<attribute id='0' name='Invalid Tag ID'>
				<value id='0' name='No values available'/>
			</attribute>
		</attvals>");
						}
					Response.End(); 
					#endregion
					}
				else if (action == "xml_lookup_part")
					{
		//			#region xml_lookup_part
		//			if (q["Q"] != "" && q["DSN"] != "")
		//				{
		//				dsn = (string)q["DSN"];
		//				var qu = (string)q["Q"];
		//				_dt = inv_class.part_lookup(qu, dsn);
		//				code = "";
		//				var description = "";
		//				if (_dt.Rows.Count > 0)
		//					{
		//					Response.Write(@"
		//<results>");
		//					foreach (DataRow dr in _dt.Rows)
		//						{
		//						code = dr["CODE"].ToString();
		//						code = code.Replace("\"", "");
		//						description = dr["DESCRIPTION"].ToString();
		//						description = description.Replace("\"", " inch");
		//						Response.Write(string.Format(@"
		//	<match code=""{0}"" description=""{1}"" />", code, description));
		//						}
		//					Response.Write(@"
		//</results>");
		//					}
		//				else { }
		//				}
		//			Response.End(); 
		//			#endregion
					}
				else if (action == "xml_part_att_val")
					{
					#region xml_part_att_val
					master_id = q["master_id"];
					_dt = inv_class.Part_Att_Vals(master_id);
					if (_dt.Rows.Count > 0)
						{
						Response.Write("<attvals>");
						foreach (DataRow dr in _dt.Rows)
							{
							attribute_name = dr["attribute"].ToString();
							value_name = dr["value"].ToString();
							Response.Write(string.Format(@"
			<attribute name=""{0}"" value=""{1}"" />", attribute_name, value_name));
							}
						Response.Write("\n</attvals>");
						}
					else
						{
						Response.Write(@"
		<attvals>
			<attribute id='0' name='Invalid Tag ID'>
				<value id='0' name='No values available'/>
			</attribute>
		</attvals>");
						}
					Response.End(); 
					#endregion
					}
				else if (action == "xml_part_history")
					{
					#region xml_part_history
					if (q["CODE"] != "")
						{
						code = (string)q["CODE"];
						Response.Write(@"
		<branches>");
			//			if (false)
			//				{
			//				var header_info = inv_class.header_info(code, dsn);
			//				var cost_price = (string)header_info["COSTPRICE"];
			//				var vendor_code = (string)header_info["VENDORCODE"];
			//				var last_sold_date = "";
			//				var last_bought_date = "";
			//				if (inv_class.exists_in_purchase_order_detail(code, dsn))
			//					{
			//					last_bought_date = inv_class.last_bought_date(code, dsn, "PURCHASE_ORDER_DTAIL");
			//					if (last_bought_date.Length == 8)
			//						{
			//						last_bought_date = last_bought_date.Substring(4, 2) + "/" + last_bought_date.Substring(6, 2) + "/" + last_bought_date.Substring(0, 4);
			//						}
			//					}
			//				else
			//					{
			//					last_bought_date = inv_class.last_bought_date(code, dsn, "PURCHASE_HISTORY_DTL");
			//					if (last_bought_date.Length == 8)
			//						{
			//						last_bought_date = last_bought_date.Substring(4, 2) + "/" + last_bought_date.Substring(6, 2) + "/" + last_bought_date.Substring(0, 4);
			//						}
			//					}
			//				if (inv_class.exists_in_sales_order_detail(code, dsn))
			//					{
			//					last_sold_date = inv_class.last_sold_date(code, dsn, "SALES_ORDER_DETAIL");
			//					if (last_sold_date.Length == 8)
			//						{
			//						last_sold_date = last_sold_date.Substring(4, 2) + "/" + last_sold_date.Substring(6, 2) + "/" + last_sold_date.Substring(0, 4);
			//						}
			//					}
			//				else
			//					{
			//					last_sold_date = inv_class.last_sold_date(code, dsn, "SALES_HISTORY_DETAIL");
			//					if (last_sold_date.Length == 8)
			//						{
			//						last_sold_date = last_sold_date.Substring(4, 2) + "/" + last_sold_date.Substring(6, 2) + "/" + last_sold_date.Substring(0, 4);
			//						}
			//					}
			//				Response.Write(string.Format(@"
			//<branch id='{0}' vendor_code=""{2}"" cost=""{1}"" last_sold=""{4}"" last_bought=""{3}"" />
			//", dsn, cost_price, vendor_code, last_bought_date, last_sold_date));
			//				}
			//			else
			//				{
			//				Response.Write(@"
			//<branch id='" + dsn + "' vendor_code='N/A' cost='' last_sold='' last_bought='' />");
			//				}
						}
					Response.Write(@"
		</branches>");
					Response.End(); 
					#endregion
					}
				else if (action == "xmlchk_reference")
					{
					#region xmlchk_reference
					code = q["code"];
					master_id = inv_class.is_reference_unique(code);
					bool is_unique = master_id != "";
					Response.Write(string.Format(@"<response><exists>{0}</exists><master_id>{1}</master_id></response>", is_unique, master_id));
					Response.End(); 
					#endregion
					}
				else if (action == "xml_vendor_contacts")
					{
					#region xml_vendor_contacts
					var _vendor_id = /*Toolbox.do_string(conn,"SELECT vendor_id FROM vendor WHERE vendor_number_int = "+*/ q["vendor_id"]; //);
					var contacts = Toolbox.doSQL_dt(conn, @"SELECT contact_id, URLDECODE(contact_name) contact_name FROM contact  WHERE contact_type = 'Vendor' AND contact_status = 'Active' AND contact_cust_id =@v0", new object[] { _vendor_id });
					Response.Write("<contacts>");
					if (contacts.Rows.Count > 0)
						{
						foreach (DataRow dr in contacts.Rows)
							{
							var name = dr["contact_name"].ToString();
							var id = dr["contact_id"].ToString();
							Response.Write("<contact><name>" + name + "</name><id>" + id + "</id></contact>");
							}
						}
					else
						{
						Response.Write("<contact><name>No available contacts</name><id>0</id></contact>");
						}
					Response.Write("</contacts>");
					Response.End(); 
					#endregion
					}
				else if (action == "xml_part_locations")
					{
					#region xml_part_locations
					var partlocations = Toolbox.doSQL_dt(conn, @" SELECT a.id, a.location_master_id, a.qty, b.name, b.type_id, IFNULL(a.min, 0) min, IFNULL(a.max, 0) max FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.master_id = @v0  AND a.business_unit_id = @v1 ORDER BY b.type_id ASC, b.name ASC", new object[] { q["id"], warehouseBusinessUnit.id });
					Response.Write("<locations>");
					if (partlocations.Rows.Count > 0)
						{
						foreach (DataRow dr in partlocations.Rows)
							{
							var location_master_id = dr["location_master_id"].ToString();
							var id = dr["id"].ToString();
							var type_id = dr["type_id"].ToString();
							var name = type_id == "1" ? "INT - " : "EXT - ";
							name += dr["name"].ToString();
							var qty = Convert.ToDouble(dr["qty"]);
							var min = Convert.ToDouble(dr["min"]);
							var max = Convert.ToDouble(dr["max"]);
							Response.Write(string.Format(@"
	<location>
		<id>{0}</id>
		<location_master_id>{1}</location_master_id>
		<qty>{2}</qty>
		<min>{3}</min>
		<max>{4}</max>
		<name>{5}</name>
	</location>",
								id,
								location_master_id,
								qty,
								min,
								max,
								HttpUtility.HtmlEncode(name)
								));
							}
						}
					Response.Write("</locations>");
					Response.End(); 
					#endregion
					}
				else if (action == "xml_locations")
					{
					#region xml_locations
					var locations = Toolbox.doSQL_dt(conn, @"SELECT a.id,a.name,a.type_id, (SELECT COUNT(*) FROM inventory_location b WHERE b.location_master_id = a.id) c FROM inventory_location_master a WHERE a.business_unit_id = @v0  ORDER BY a.type_id ASC, name ASC", new object[] { warehouseBusinessUnit.id });
					Response.Write("<locations>");
					if (locations.Rows.Count > 0)
						{
						foreach (DataRow dr in locations.Rows)
							{
							var id = dr["id"].ToString();
							var type_id = dr["type_id"].ToString();
							var name = type_id == "1" ? "INT - " : "EXT - ";
							name += dr["name"].ToString();
							var c = dr["c"].ToString();
							Response.Write(string.Format(@"
	<location>
		<id>{0}</id>
		<name>{1}</name>
		<type_id>{2}</type_id>
		<c>{3}</c>
	</location>",
								id,
								name,
								type_id,
								c
								));
							}
						}
					Response.Write("</locations>");
					Response.End(); 
					#endregion
					}
				else if (action == "xml_price_info")
					{
					inv_class.price_info(false);
					}
				else if (action == "xml_price_history")
					{
					inv_class.price_info(true);
					}
				else if (action == "add_value")
					{
					inv_class.value_add();
					}
				else if (action == "adjust_minmax")
					{
					Response.Clear();
					try
						{
						var temp_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  q["master_id"], warehouseBusinessUnit.id } );
						var onhand = q["onhand"] != null ? Convert.ToDouble(q["onhand"]) : Toolbox.doSQL_double(@"SELECT IFNULL(MAX(onhand_qty),0) FROM inventory_branch WHERE master_id = @v0  AND business_unit_id = @v1 ", new object[] {  q["master_id"], warehouseBusinessUnit.id } );
						//if(_q["onhand"] != null)
						//	{
						//Check Exists
						if (Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM inventory_branch WHERE business_unit_id = @v0 AND master_id = @v1", new object[] { warehouseBusinessUnit.id, q["master_id"]}) == 1)
							{
							br_obj.load(Convert.ToInt32(q["master_id"]), warehouseBusinessUnit.id);
							}
						br_obj.business_unit_id = warehouseBusinessUnit.id;
						br_obj.master_id = Convert.ToInt32(q["master_id"]);
						br_obj.member_id = current_user.id;
						br_obj.min_qty = Convert.ToDouble(q["min"]);
						br_obj.max_qty = Convert.ToDouble(q["max"]);
						br_obj.dollar_balance = temp_cost*onhand;
						br_obj.onhand_qty = onhand;
						br_obj.save();
						if (!string.IsNullOrEmpty(q["poprog_id"]) && !string.IsNullOrEmpty(q["delivery_date"]))
							{
							Toolbox.doSQL_void(conn, @"UPDATE poprog_header SET poprog_expected_received_date = @v0 WHERE poprog_id = @v1 LIMIT 1", new object[] { q["delivery_date"], q["poprog_id"]});
							}
						Response.Write("SUCCESS");
						}
					catch (Exception ee)
						{
						Response.Write(ee.Message);
						}
					Response.End();
					}
				else if (action == "working_branch")
					{
					Response.Clear();
					try
						{
						var requested_id = q["requested_id"];
						var bu = new NeBusinessUnit(requested_id);
						if (Session["working_business_unit_id"] == null)
							{
							Session.Add("working_business_unit_id", bu.id);
							}
						else
							{
							Session["working_business_unit_id"] = bu.id;
							}
						if (Session["working_warehouse_bu_id"] == null)
							{
							Session.Add("working_warehouse_bu_id", bu.warehouse_bu_id);
							}
						else
							{
							Session["working_warehouse_bu_id"] = bu.warehouse_bu_id;
							}
						Response.Write("Success");
						}
					catch (Exception ee)
						{
						Response.Write(ee.Message);
						}
					Response.End();
					}
				else if (action == "groups")
					{
					lbltemp.Text = "Inventory / Branch Access / Groups";
					button_groups.Style["background-color"] = "#35a";
					button_groups.Disabled = true;
					group_panel.Visible = true;
					group_hdrs.Visible = true;
					}
				else if (action == "kitted")
					{
					lbltemp.Text = "Inventory / Branch Access / Kitted Parts";
					button_kitted.Style["background-color"] = "#35a";
					button_kitted.Disabled = true;
					kitted_hdrs.Visible = true;
					kitted_panel.Visible = true;
					}
				else if (action == "rfqs")
					{
					lbltemp.Text = "Inventory / Branch Access / RFQ's";
					button_rfqs.Style["background-color"] = "#35a";
					button_rfqs.Disabled = true;
					rfq_tab.Visible = true;;

//memo_email.Text						= email_message_text;
					}
				else if (action == "orders_customer")
					{
					Response.Clear();
					try
						{
						var requested_id = q["requested_id"];
						Session["orders_customer_id"] = requested_id;
						Session["inv_gv_orders"] = null;
						Response.Write("Success");
						}
					catch (Exception ee)
						{
						Response.Write(ee.Message);
						}
					Response.End();
					}
				else if (action == "orders_extloc")
					{
					Response.Clear();
					try
						{
						var requested_id = q["requested_id"];
						Session["orders_ext_loc_id"] = requested_id;
						if (requested_id != "0")
							{
							Session["inv_branch_include_external"] = 1;
							}
						Session["inv_gv_orders"] = null;
						Response.Write("Success");
						}
					catch (Exception ee)
						{
						Response.Write(ee.Message);
						}
					Response.End();
					}
				else if (action == "orders_rfq")
					{
					var _rfq_id = q["requested_id"];
					Session["orders_rfq_id"] = _rfq_id;
					Session["inv_gv_orders"] = null;
					Toolbox.QuickReponse(Response, "Success");
					}
				else if (action == "locations")
					{
					lbltemp.Text = "Inventory / Branch Access / Locations";
					button_location.Style["background-color"] = "#35a";
					button_location.Disabled = true;
					uc_master_locations.Visible = true;
					}
				else if (action == "vdi")
					{
					lbltemp.Text = "Inventory / Branch Access / Vendor Data Import";
					button_manageprices.Style["background-color"] = "#35a";
					button_manageprices.Disabled = true;
					uc_vdi.Visible = true;
					}
				else if (action == "orders")
					{
					if (!IsPostBack)
						{
						Session["inv_gv_orders"] = null;
						}
					lbltemp.Text = "Inventory / Branch Access / Orders";
					button_orders.Style["background-color"] = "#35a";
					button_orders.Disabled = true;
					orders_panel.Visible = true;
					layout.used_gv = uc_orders_tab.gvorders;
					_h.Set("gridview_id", "gv_orders");
					_panel_export.Visible = true;
					gl = new NeGridLayouts(current_user.id, page_name);
					if (gl.GridLayout_Layout != "")
						{
						uc_orders_tab.gvorders.LoadClientLayout(gl.GridLayout_Layout);
						}
					else
						{
						gl = new NeGridLayouts();
						gl.GridLayout_Layout = uc_orders_tab.gvorders.SaveClientLayout();
						gl.member_id = current_user.id;
						gl.GridLayout_Name = "Default";
						gl.GridLayout_Gridid = page_name;
						gl.SaveGridLayout();
						}
					uc_orders_tab.hid_exp.Value = uc_orders_tab.gvorders.SaveClientLayout();
					_h.Set("ID", gl.GridLayoutID);
					_h.Set("NAME", gl.GridLayout_Name);
					_dde_filter.Text = gl.GridLayout_Name;
					uc_orders_tab.fill_gv_orders();
					}
				else if (action == "qty")
					{
					if (!IsPostBack)
						{
						Session["inv_gv_minmax"] = null;
						}
					lbltemp.Text = "Inventory / Branch Access / QTY Management";
					button_qty.Style["background-color"] = "#35a";
					button_qty.Disabled = true;
					gv_minmax.Visible = true;
					_panel_export.Visible = true;
					page_name = string.Format("{0}_gv_minmax", _page_name);
					layout.used_gv = gv_minmax;
					_h.Set("gridview_id", "gv_minmax");
					gl = new NeGridLayouts(current_user.id, page_name);
					if (!IsCallback)
						{
						if (gl.GridLayout_Layout != "")
							{
							gv_minmax.LoadClientLayout(gl.GridLayout_Layout);
							}
						else
							{
							gl = new NeGridLayouts();
							gl.GridLayout_Layout = gv_minmax.SaveClientLayout();
							gl.member_id = current_user.id;
							gl.GridLayout_Name = "Default";
							gl.GridLayout_Gridid = page_name;
							gl.SaveGridLayout();
							}
						_dde_filter.Text = gl.GridLayout_Name;
						_h.Set("ID", gl.GridLayoutID);
						_h.Set("NAME", gl.GridLayout_Name);
						}
					fill_gv_minmax();
					}
				else if (action == "move_parts")
					{
					if (!IsPostBack)
						{
						uc_part_transfer.fill_gv_transfer();
						}
					uc_part_transfer.Visible = true;
					lbltemp.Text = "Inventory / Branch Access / Move Parts";
					button_move_parts.Style["background-color"] = "#35a";
					button_move_parts.Disabled = true;
					}
				else if (action == "toggle_stock_changes")
					{
					if (current_user.MemberTypeID != 5 && !current_user.AuthenticatedForPrivilege(153))
						{
						Toolbox.QuickReponse(Response, "You cannot set this");
						}
					else
						{
						_bo_obj = new branch_options(warehouseBusinessUnit.id);
						var state = Convert.ToBoolean(q["state"]);
						if (state)
							{
							_bo_obj.start_adjustment_period(current_user.id);
							}
						else
							{
							_bo_obj.stop_adjustment_period(current_user.id);
							}
						Toolbox.QuickReponse(Response, "SUCCESS");
						}
					}
				else if (action == "delete_location")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					if (q["id"] != null)
						{
						_loc = new location(Convert.ToInt32(q["id"]));
						try
							{
							br_obj = new branch(q["id"], warehouseBusinessUnit.id);
							_loc.update_branch(3, br_obj.dollar_balance, _loc.qty, 0, br_obj);
							_loc.delete();
							Response.Write("SUCCESS");
							}
						catch (Exception ee)
							{
							if (ee.ToString().Contains("last location"))
								{
								Response.Write("You cannot delete the last location that is referenced on a part");
								}
							else
								{
								Response.Write("An error occured trying to delete this location, the IT department has been emailed.");
								}
							}
						}
					Response.End();
					}
				else if (action == "delete_location_master")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					if (q["id"] != null)
						{
						_loc_m = new location_master(Convert.ToInt32(q["id"]));
						if (_loc_m.children > 0)
							{
							var locs = Toolbox.doSQL_string(conn, @"SELECT GROUP_CONCAT(master_id) FROM inventory_location WHERE location_master_id =@v0 limit 50 ", new object[] {
							_loc_m.id });
							Response.Write("Can't delete. \nThese parts have locations set to this master location:\n" + locs);
							}
						else
							{
							try
								{
								_loc_m.delete();
								Response.Write("SUCCESS");
								}
							catch (Exception ee)
								{
								Response.Write(ee.Message);
								}
							}
						}
					Response.End();
					}
				else if (action == "save_location")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					if (!string.IsNullOrEmpty(q["master_id"]))
						{
						_loc = string.IsNullOrEmpty(q["id"]) ? new location() : new location(Convert.ToInt32(q["id"]));
						_loc.business_unit_id = _loc.business_unit_id == 0 ? warehouseBusinessUnit.id : _loc.business_unit_id;
						_loc.master_id = _loc.master_id == 0 ? Convert.ToInt32(q["master_id"]) : _loc.master_id;
						if (string.IsNullOrEmpty(q["id"]))
							{
							var loc_c = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM inventory_location 
WHERE location_master_id =@v0 AND business_unit_id =@v1  AND master_id =@v2",
								new object[] { q["location_master_id"], _loc.business_unit_id, _loc.master_id});
							if (loc_c > 0)
								{
								Response.Write("This location already exists.");
								Response.End();
								}
							}
						else
							{
							var loc_c_sql = @"SELECT COUNT(*) FROM inventory_location " +
							                "WHERE location_master_id = @v0" +
							                " AND business_unit_id = @v1" +
							                " AND master_id = @v2" +
							                " AND id != @v3";
							var loc_c = Toolbox.doSQL_int(conn, loc_c_sql,
									new object[] { q["location_master_id"], _loc.business_unit_id, _loc.master_id, q["id"] });
							
							if (loc_c > 0)
								{
								Response.Write("This location already exists.");
								Response.End();
								}
							}
						var prev_qty = _loc.qty;
						_loc.location_master_id = Convert.ToInt32(q["location_master_id"]);
						var diff = _loc.qty == 0 ? Convert.ToDouble(q["qty"]) : _loc.qty - Convert.ToDouble(q["qty"]);
						_loc.qty = Convert.ToDouble(q["qty"]);
						_loc.min = Convert.ToDouble(q["min"]);
						_loc.max = Convert.ToDouble(q["max"]);
						_loc.alert_worthy = true;
						_loc.log_is_manual = true;
						_loc.member_id = current_user.id;
						_loc.section_id = 2;
						try
							{
							br_obj = new branch(_loc.master_id, _loc.business_unit_id);
							var this_cost = Toolbox.doSQL_double(conn,@"SELECT GET_CURRENT_COST(@v0 ,@v1 )", new object[] {  _loc.master_id, _loc.business_unit_id } );
							_loc.save();
							var this_type = prev_qty > Convert.ToDouble(q["qty"]) ? 3 : 2;
							//_tools.debug_note(this_type + ", " + br_obj.dollar_balance + ", " + diff + ", " + this_cost + ", " + q["qty"] + ", " + prev_qty);
							// 3, 1.5, 1, 1.5, 0, 1
							_loc.update_branch(this_type, br_obj.dollar_balance, diff, this_cost, br_obj);
							Response.Write("SUCCESS");
							}
						catch (Exception ee)
							{
							Response.Write(ee.Message);
							}
						}
					Response.End();
					}
				else if (action == "save_master_location")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					_loc_m = string.IsNullOrEmpty(q["id"])
						? new location_master()
						: new location_master(Convert.ToInt32(q["id"]));
					_loc_m.business_unit_id = _loc_m.business_unit_id == 0 ? warehouseBusinessUnit.id : _loc_m.business_unit_id;
					if (_loc_m.id != null)
						{
						var location_log = new NELog();
						location_log.business_unit_id = warehouseBusinessUnit.id;
						location_log.member_id = current_user.id;
						location_log.is_manual = true;
						location_log.table = OpsLog.Table.InventoryLocationMaster;
						location_log.table_id = _loc_m.id;
						if (_loc_m.type_id != Convert.ToInt32(q["type_id"]))
							{
							location_log.section_id = OpsLog.Section.LocationMaster;
							location_log.action_id = OpsLog.Action.ChangedLocationType;
							location_log.value_old = _loc_m.type_id;
							location_log.value_new = q["type_id"];
							location_log.save();
							}
						if (_loc_m.name != q["name"])
							{
							location_log.section_id = OpsLog.Section.LocationMaster;
							location_log.action_id = OpsLog.Action.ChangedLocationName;
							location_log.value_old = _loc_m.name;
							location_log.value_new = q["name"];
							location_log.save();
							}
						}
					_loc_m.name = q["name"];
					if (_loc_m.id == null)
						{
						
						var loc_sql = "SELECT COUNT(*) FROM inventory_location_master WHERE TRIM(name) =@v0 AND business_unit_id = @v1";
						var c_loc_m = Toolbox.doSQL_int(conn, loc_sql, new object[] { _loc_m.name.Trim(), _loc_m.business_unit_id});
						if (c_loc_m > 0)
							{
							Response.Write("Duplicate location, not saving");
							Response.End();
							}
						}
					else
						{
						var loc_sql = "SELECT COUNT(*) FROM inventory_location_master WHERE TRIM(name) =@v0 AND business_unit_id = @v1 AND id !=@v2 ";
						var c_loc_m = Toolbox.doSQL_int(conn, loc_sql, new object[] { _loc_m.name.Trim(), _loc_m.business_unit_id , _loc_m.id });
						if (c_loc_m > 0)
							{
							Response.Write("Duplicate location, not saving");
							Response.End();
							}
						}
					_loc_m.type_id = Convert.ToInt32(q["type_id"]);
					try
						{
						_loc_m.save();
						Response.Write("SUCCESS");
						}
					catch (Exception ee)
						{
						Response.Write(ee.Message);
						}
					Response.End();
					}
				else if (action == "handle_note")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					if (q["id"] != null && q["note"] != null)
						{
						if (q["type"] == "consignment")
							{
							try
								{
								ico = new consignment(Convert.ToInt32(q["id"]));
								ico.note = q["note"];
								ico.save();
								Response.Write("SUCCESS");
								}
							catch (Exception ee)
								{
								Response.Write(ee.ToString());
								}
							}
						else if (q["type"] == "pricing")
							{
							try
								{
								var vpr = new vendor_price_row(q["id"]);
								vpr.note = q["note"];
								vpr.save(true);
								Response.Write("SUCCESS");
								}
							catch (Exception ee)
								{
								Response.Write(ee.ToString());
								}
							}
						}
					else
						{
						Response.Write("Server Error");
						}
					Response.End();
					}
				else if (action == "manage_prices")
					{
					lbltemp.Text = "Inventory / Branch Access / Manage Pricing";
					button_manageprices.Style["background-color"] = "#35a";
					button_manageprices.Disabled = true;
					output += string.Format(@"
       						<script>
       							$(document).ready(function()
       												{{
       												attach_ac($('#tag_id'), 'inventory_tag');
       												attach_ac($('#vendor_id'), 'vendor');
       												$('#vendor_id').attr('data-click', 'populate_browser()');
       												$('#tag_id').attr('data-click', ""if($('#vendor_id').attr('data-id') != ''){{populate_browser()}}else{{$('#vendor_id').focus()}}"").focus();
       												}});
       						</script>
							<div align='left' style='padding:5px;'>
								<button type='button' onclick=""location.href='./index.aspx?a=vdi';"">Vendor Data Import</button>
							</div>
       						<table cellpadding='2' cellspacing='0' style='width:100%'>
       							<tr>
       								<td class='tag_space' colspan='2'>
       									<table cellpadding='2' cellspacing='0' style='width:100%'>
       										<tr>
       											<td width='5%'><b>Tag:</b></td>
       											<td><input id='tag_id' class='idbox' data-id='' value='' data-isac='false' data-master_id='' data-business_unit_id='{0}' data-click='' style='width:100%'></td>
       										</tr>
       										<tr>
       											<td><b>Vendor:</b></td>
       											<td><input id='vendor_id' class='idbox' data-id='' value='' data-isac='false' data-master_id='' data-business_unit_id='{0}' data-click='' style='width:100%'></td>
       										</tr>
       									</table>
       								</td>
       							</tr>
       							<tr>
       								<td id='results' valign='top' width='200'>&nbsp;</td>
       								<td id='parts' valign='top'>&nbsp;</td>
       							</tr>
       						</table>",
						warehouseBusinessUnit.id
						);
					}
				else if (action == "chk_uniqueness")
					{
					Toolbox.do_set_XML_header(Response);
					if (q["tag"] != null && q["tag_pattern"] != null)
						{
						TagID = (string) q["tag"];
						var tag_pattern = (string) q["tag_pattern"];
						if (string.IsNullOrWhiteSpace(TagID))
							{
							throw new Exception("Tag not selected");
							}
						if (q["master_id"] != null)
							{
							master_id = (string) q["master_id"];
							if (inv_class.is_item_unique(TagID, tag_pattern, master_id))
								{
								Response.Write(@"
<response>
	<unique>true</unique>
	<master_id>null</master_id>
	<active>null</active>
</response>");
								}
							else
								{
								master_id = inv_class.find_tagpattern(tag_pattern, TagID);
								inventory.Load(master_id, warehouseBusinessUnit.id);
								Response.Write(string.Format(@"
<response>
	<unique>false</unique>
	<master_id>{0}</master_id>
	<active>{1}</active>
</response>", master_id, inventory.active));
								}
							}
						else
							{
							if (inv_class.is_item_unique(TagID, tag_pattern))
								{
								Response.Write(@"
<response>
	<unique>true</unique>
	<master_id>null</master_id>
	<active>null</active>
</response>");
								}
							else
								{
								master_id = inv_class.find_tagpattern(tag_pattern, TagID);
								inventory.Load(master_id, warehouseBusinessUnit.id);
								Response.Write(string.Format(@"
<response>
	<unique>false</unique>
	<master_id>{0}</master_id>
	<active>{1}</active>
</response>", master_id, inventory.active));
								}
							}
						Response.End();
						}
					else
						{
						Response.Write("FAILED");
						Response.End();
						}
					}
				else if (action == "vendorquery")
					{
					if (q["q"] != null)
						{
						var this_query = q["q"];
						var this_type_of = "";
						output = "";
						Response.Clear();
						Response.ContentType = "text/plain";
						if (Regex.IsMatch(this_query, "^[0-9]+$"))
							{
							this_type_of = "id";
							}
						else
							{
							this_type_of = "name";
							}
						_dt = inv_class.Query_Vendors(this_query, this_type_of);
						if (_dt.Rows.Count > 0)
							{
							foreach (DataRow dr in _dt.Rows)
								{
								var text = dr["vendor_name"].ToString();
								var value = dr["vendor_number"].ToString();
								output += string.Format("{1} - {0}|{1}\n", text, value);
								}
							}
						Response.Write(output);
						Response.End();
						}
					else
						{
						Response.Redirect("/sections/member/inventory/");
						}
					}
				else if (action == "location_qty_xfer")
					{
					Toolbox.do_set_plain_header(Response);
					if (!string.IsNullOrEmpty(q["to_id"]) &&
						!string.IsNullOrEmpty(q["from_id"]) &&
						!string.IsNullOrEmpty(q["qty"]))
						{
						try
							{
							var from = new location(Convert.ToInt32(q["from_id"]));
							var to = new location(Convert.ToInt32(q["to_id"]));
							var qty = Convert.ToDouble(q["qty"]);
							if (@from.qty < qty)
								{
								output = "You cannot transfer more items than currently exist in the (" + @from.this_master.name + ") location. \n There are only:" + @from.qty + " Available";
								Response.Write(output);
								Response.End();
								}
							_loc = new location();
							_loc.log_is_manual = true;
							_loc.section_id = 2;
							br_obj = new branch(_loc.master_id, _loc.business_unit_id);
							_loc.update_branch(1, br_obj.dollar_balance, 0, 0, br_obj);
							_loc.xfer((int) @from.id, (int) to.id, qty, current_user);
							output = "SUCCESS";
							}
						catch (Exception ee)
							{
							output = ee.ToString();
							}
						}
					else
						{
						output = "Invalid Request";
						}
					Response.Write(output);
					Response.End();
					}
				else if (action == "start_part")
					{
					lbltemp.Text = "Inventory / Branch Access / New Part";
					button_newitem.Style["background-color"] = "#35a";
					var forced_script = "";
					tag_name = "Type tag name here";
					if (!string.IsNullOrEmpty(q["tag_id"]) && (!string.IsNullOrEmpty(q["master_id"]) || !string.IsNullOrEmpty(q["pattern"])))
						{
						TagID = q["tag_id"];
						tag_name = inventory.TagName(TagID);
						if(!string.IsNullOrEmpty(q["master_id"]))
							{
							master_id = q["master_id"];
							forced_script = string.Format("<script>build_att_val_boxes({0}, {1})</script>", TagID, master_id);
							}
						else if(!string.IsNullOrEmpty(q["pattern"]))
							{
							forced_script = string.Format("<script>build_att_val_boxes({0}, '{1}')</script>", TagID, Toolbox.do_value_from(q["pattern"],true));
							}
						}
					button_newitem.Disabled = true;
					output += string.Format(@"
                                                <input type='hidden' id='hidPartCreated' value='0' />
                           						<input type='hidden' id='type_of' value='NEW' /><input type='hidden' id='n_attributes' value='0'><input type='hidden' id='branch_dsn' value='{0}'>
                           						<table class='new_part' cellspacing='0'>
                           							<tr>
                           								<td class='tag_space' colspan='2' valign='middle'>
                           									<input type='hidden' id='country' value='{1}' />
                           									<table cellpadding='0' cellspacing='0' width='100%'>
                           										<tr>
                           											<td width='125' align='center'>
                           												<button id='SAVE_BUTTON' type='button' disabled><img src='/images/icon/icon[save].gif' align='absmiddle' />Save</button>
                           											</td>
                           											<td width='95%'>
                           												<input id='TAG_ID' type='text' style='width:98%;font-weight:bold' onfocus=""attach_ac(this, 'inventory_tag')"" data-id='{4}' value='{3}' data-isac='false' data-master_id='{2}' data-business_unit_id='{2}'
                                                                               onblur='onBlurEvent()' data-saved-id='{7}' data-saved-name='{6}' />
                           											</td>
                           										</tr>
                           									</table>
                           									<div id='uniquestatus' align='left'></div>
                           								</td>
                           							</tr>
                           							<tr>
                           								<td width='10%' valign='top' align='left'>
                           									<div id='att_vals'>
                           									</div>
                           								</td>
                           								<td class='pricing' valign='top'>
                           									<table cellpadding='0' cellspacing='0' class='info'>
                           										<tr>
                           											<td id='new_pricing'>
                           												<table id='new_pricing_table' cellspacing='0' cellpadding='2'>
                           													<thead id='new_pricing_head'>
                           														<td class='cb'>&nbsp;</td>
                           														<th>Vendor</th>
                           														<th width='150'>Ven. Part #</th>
                           														<th width='55'>Total</th>
                           														<th width='65'>Qty<br/><i>Per Part #</i></th>
                           														<th width='60'>Cost<br/><i>Base Pr.</i></th>
                           														<td class='del'><button type='button' onclick='add_price_row(this)'>+</button></td>
                           													</thead>
                           													<tbody id='new_pricing_body'>
                           													</tbody>
                           												</table>
                           											</td>
                           										</tr>
                           									</table>
                           									<div id='matched_parts'>&nbsp;</div>
                           								</td>
                           							</tr>
                           						</table>
                           						<div id='picture_box' style='display:none;'>
                           							<iframe src='' style='width:650px;border:0;height:420px;' id='picture_box_iframe'></iframe>
													<button type='button' id='picture_box_print'><img src='/images/icon/icon[print_barcode].GIF' align='absmiddle' />Print Barcode</button>
                           						</div>
                           						<script>
                           							function reset_picture_box()
                           								{{
                           								$('#picture_box').dialog('close');
                           								}}
                           						</script>
                           						{5}
                           						",
						dsn,
						warehouseBusinessUnit.country,
						warehouseBusinessUnit.id,
						tag_name,
						TagID,
						forced_script,
                        tag_name,
                        TagID
                        );
					}
				else if (action == "get")
					{
					if (q["tab"] != null && q["id"] != null)
						{
						try
							{
							inventory.Load(q["id"], warehouseBusinessUnit.id);
							}
						catch (Exception ee)
							{
							if (ee.Message.Contains("Does Not Exist"))
								{
								Toolbox.FriendlyException(Response, "Part does not exist", "history.go(-1);");
								}
							}
						Page.Header.Title = "Inventory - (#" + inventory.master_id + ")";
						if (!inventory.active)
							{
							Toolbox.FriendlyException(Response, "This is an inactive part", "history.go(-1);");
							}
						else
							{
							#region Javascript Tab Control

							output = @"
							<script type='text/javascript'>
								function tab(obj)
									{
									var _tab		= $(obj).attr('data-tab');
									var _id			= $('#MASTER_ID').val();
									var _url		= './index.aspx?a=get&tab='+_tab+'&id='+_id;
									if($(obj).attr('class') == 'active')
										{
										location.href	= _url;
										}
									else
										{
										return false;
										}
									}
								function attacher(obj)
									{
									var _linktype	= $(obj).parents('tr:first').find('select').val();
									switch(_linktype)
										{
										case 'M':
											attach_ac(obj, 'manufacturer');
										break;
										case 'V':
											attach_ac(obj, 'vendor');
										break;
										}
									}
								function catch_enter(obj)
									{
									var key = window.event.which||window.event.keyCode;     
									if(key == 13)
										{
										$(obj).parents('tr:first').find('button').click();
										$(obj).blur();
										return false;
										}
									}
							</script>
							<div class='view_part'>";
							var disabled = new NameValueCollection();
							disabled["G"] = "active";
							disabled["P"] = "active";
							disabled["H"] = "active";
							disabled["V"] = "active";
							disabled["A"] = "active";
							disabled["VH"] = "active";
							disabled["S"] = "active";
							disabled["SH"] = "active";
							disabled["BC"] = "active";
							disabled["PL"] = "active";
							disabled["U"] = "active";
							disabled["QH"] = "active";
							disabled["CO"] = "active";
							disabled["NO"] = "active";
							if (string.IsNullOrEmpty(q["stockdetail"]) || q["stockdetail"] != "true")
								{
								disabled[q["tab"]] = "disabled";
								}
							var hide_tabs = !show_cost && !is_purchaser ? " style='display:none;'" : "";
							var hide_location = inventory.is_exclude ? "style='display:none' " : "";
							output += string.Format(@"
								<div class='submenu' align='right'>
									<table cellpadding='0' cellspacing='0'>
										<tr>
											<td class='{0}'		data-tab='G'	onclick='tab(this)' {0}>General</td>
											<td class='{17}'	data-tab='NO'	onclick='tab(this)' {17}>Notes</td>
											<td class='{15}'	data-tab='PL'	onclick='tab(this)' {16} {0}>Part Locations</td>
											<td class='{13}'	data-tab='QH'	onclick='tab(this)' {16} {13}>Quantity Adjustments</td>
											<td class='{10}'	data-tab='BC'	onclick='tab(this)' {10}>Barcodes</td>
											<td class='{9}'		data-tab='A'	onclick='tab(this)' {9}>Alternates</td>
											<td class='{12}'	data-tab='U'	onclick='tab(this)' {12}>Usage</td>
											<td class='{14}'	data-tab='CO'	onclick='tab(this)' {14}>Repairs</td>
											<td class='{1}'		data-tab='P'	onclick='tab(this)' {1}>Vendor<br/>Prices</td>
											<td class='{2}'		data-tab='H'	onclick='tab(this)' {11} {2}>Vendor Price History</td>
										</tr>
									</table>
								</div>
								<div class='partheader'>{8}<input type='hidden' value='{7}' id='MASTER_ID'/></div>",
								disabled["G"], // 0 
								disabled["P"], // 1 
								disabled["H"], // 2 
								disabled["V"], // 3 
								disabled["VH"], // 4 
								disabled["S"], // 5 
								disabled["SH"], // 6 
								inventory.master_id, // 7 
								inv_class.part_description(inventory.master_id), // 8 
								disabled["A"], // 9 
								disabled["BC"], // 10 
								hide_tabs, // 11
								disabled["U"], // 12
								disabled["QH"], // 13
								disabled["CO"], // 14
								disabled["PL"], // 15
								hide_location, // 16
								disabled["NO"] // 17
								);

							#endregion Javascript Tab Control

							string canadian_sold_as;
							string usa_sold_as;
							if (q["tab"] == "G")
								{
								var sb_general = new StringBuilder();
								lbltemp.Text = "Inventory / Branch Access / Part Viewer / Stock Detail";

								#region General Part

								var replicate_cell = is_purchaser ? string.Format(@"
														<button type='button' onclick=""location.href='./index.aspx?a=start_part&tag_id={1}&master_id={0}'"" title=""This will take you to the new item page, with all of the attributes filled out to match this part"" style='font-size:11px;font-weight:bold;padding:5px;'><img src='/images/icon/icon[replicate].gif' valign='middle' /> Replicate Part</button>
													", inventory.master_id, inv_class.get_TagIDfromMaster(inventory.master_id)) : "";
								var file_button = @"<button type='button' onclick='load_file_popup()' style='font-size:11px;font-weight:bold;padding:5px;'><img src='/images/icon/icon[file].gif' valign='middle' /> Files</button>";
								divMenu.InnerHtml = "</form>" + divMenu.InnerHtml;
								br_obj = new branch(inventory.master_id, warehouseBusinessUnit.id);
								var cost_price_branch = show_cost || is_purchaser ? Toolbox.do_Monetize(inventory.cost_price_branch) : "--";
								var sell_price = shared.GetSellPrice(inventory.cost_price_branch, 0, true, 1, warehouseBusinessUnit.id);
								var sellprice_cell = is_purchaser || show_cost ? string.Format("<table cellspacing='0' cellpadding='0'><tr data-business_unit_id='{0}'><td><b>{1:C2}</b></td><td></td></tr></table>", warehouseBusinessUnit.id, sell_price) : sell_price.ToString("c2");
								sb_general.AppendFormat(@"
										<div id='new_pic' style='display:none;'>
											<div>
												<form method='post' action='index.aspx' id='picture_submit'>
													<b style='color:#f00;font-size:11px;'>Only use .jpg files when adding pictures</b><br/>
													<table cellpadding='2' cellspacing='2' width='95%'>
														<tr>
															<td valign='top'>
																<div style='width:300px;height:300px;margin-bottom:15px;' >
																	<img height='300' width='300' id='part_image_pop' src='/_tools/inventory_picture/index.aspx?id={4}' style='border:solid 2px #000;' />
																</div>
															</td>
															<td valign='top'>
																<div id='paste_box' style=""border: 2px dashed #999;background-image: url('/images/ticket/bg[paste-sm].png');background-color: #fff;background-position: center top;background-repeat: no-repeat;width:300px;height:300px;""></div>
															</td>
														</tr>
														<tr>
															<td>&nbsp;</td>
															<td>
																<div id='new_pic' style='width:320px;text-align:center'>
																	<center>
																	<input type='hidden' name='a' value='update_picture'/>
																	<input type='hidden' class='inventory_file_blob' name='inventory_file_blob' value=''/>
																	<input type='hidden' id='master_id' name='master_id' value='{4}'/>
																	<input type='hidden' name='close' value='false'/>
																	<input name='picture' id='_picture' type='file'/>
																	<table>
																		<tr>
																			<td><input type='submit' value='Submit'/></td>
																			<td><button type='button' disabled='true' class='inventory_cancelpasted' onclick='branch_inventory.paste_image.remove()'>Cancel Pasted Image</button> </td>
																		</tr>
																	</table>
																	</center>
																</div>
															</td>
														</tr>
													</table>
												</form>
											</div>
										</div>
										<table cellpadding='0' cellspacing='0' class='general'>
											<tr>
												<td class='pricing_td' valign='top' align='center'>
													<div style='width:150px;float:left;clear:right;margin:5px;'><img id='part_image' title='Click me to change photo' height='150' width='150' style='display:none;cursor:pointer;padding:1px;border:solid 1px #000;' /></div>
   													<div class='_panel' {6}><div class='_title'>Branch Cost</div><div onmouseover='legend(this, true)' onmouseout='legend(this, false)'  class='legend L{8}'>L{8}</div><b style='padding:10px;display:block;text-align:center;'>{1}</b></div>
   													<div class='_panel' {6}><div class='_title'>Country-Wide Cost</div><div class='legend'>&nbsp;&nbsp;&nbsp;</div>
													<div class='_panel' {6}><div class='_title'>Branch Sell</div><div class='legend'>&nbsp;&nbsp;&nbsp;</div><b style='padding:10px;display:block;'>{5}</b></div>
   											   		{7}{0}
												</td>
												<td class='properties' valign='top' align='left' width='350'>
														",
									"", // {0}
									cost_price_branch, // {1}
									"", // {2}
									disabler, // {3}
									inventory.master_id, // {4}
									sellprice_cell, // {5}
									hide_location, // {6}
									file_button, // {7}
									inventory.cost_price_branch_level // {8}
									);

								#region Tag Properties

								sb_general.Append(@"
													<div class='_panel'>
														<div class='_title'>Tag Properties</div>
														<table cellpadding='2' cellspacing='0'>");
								sb_general.AppendFormat(@"
															<tr>
																<td class='l' width='150'>Force Cost to 0.01?</td>
																<td class='r' colspan='2'>{0}</td>
															</tr>
															<tr>
																<td class='l' width='150'>Is Allowed to Stock?</td>
																<td class='r' colspan='2'>{1}</td>
															</tr>
															<tr>
																<td class='l' width='150'>Canadian Sold As Unit:</td>
																<td class='r' colspan='2'>{2}</td>
															</tr>
															<tr>
																<td class='l' width='150'>USA Sold As Unit:</td>
																<td class='r' colspan='2'>{3}</td>
															</tr>
											",
									inventory.is_exclude,
									inventory.allowed_to_stock,
									inventory.sold_as_canadian_name,
									inventory.sold_as_usa_name
									);
								if (is_admin)
									{
									sb_general.AppendFormat(@"
															<script type='text/javascript'>
															branch_inventory.toggle_consumable = function(obj){{please_wait('Start');var _master_id	= $('#MASTER_ID').val();$.get('./index.aspx', {{'a': 'toggle_consumable', master_id : _master_id, chked:$(obj).is(':checked')}}, function(r){{please_wait('Stop');if(r == 'SUCCESS'){{alert('Saved');}}}});}};
															</script>
															<tr>
																<td class='l' width='150'>Is Consumable?:</td>
																<td class='r' colspan='2'><input type='checkbox' onchange='branch_inventory.toggle_consumable(this);' id='is_consumable' {0} /></td>
															</tr>", inventory.branch_obj.is_consumable ? "CHECKED" : "");
									}
								else
									{
									sb_general.AppendFormat(@"
															<tr>
																<td class='l' width='150'>Is Consumable?:</td>
																<td class='r' colspan='2'>{0}</td>
															</tr>", inventory.branch_obj.is_consumable);
									}

                                var subcontract = inventory.subcontract
                                    ? "CHECKED"
                                    : "";
                                var shipping = inventory.shipping
                                    ? "CHECKED"
                                    : "";
                                var others = inventory.others
                                    ? "CHECKED"
                                    : "";

                                sb_general.AppendFormat(@"
														<tr>
															<td class='l' width='150'>Sub-Contract?:</td>
															<td class='r' colspan='2'><input type='checkbox' {0} disabled='disabled' /> </td>
                                                        </tr>
														<tr>
															<td class='l' width='150'>Shipping?:</td>
															<td class='r' colspan='2'><input type='checkbox' {1} disabled='disabled' /></td>
														</tr>
														<tr>
															<td class='l' width='150'>Others?:</td>
															<td class='r' colspan='2'><input type='checkbox' {2} disabled='disabled' /></td>
														</tr>
										",
                                    subcontract,
                                    shipping,
                                    others
                                );

                                sb_general.Append(@"
														</table>
													</div>");

								#endregion Tag Properties

								#region Attributes / Values

								var att_vals = "";
								_dt = Toolbox.doSQL_dt(conn, string.Format(@"
	SELECT 
		GET_ATTRIBUTE(c.attribute_id) attribute, 
		c.value 
	FROM 
		inventory_item_master a 
	LEFT JOIN inventory_item_detail b on a.master_id = b.master_id 
	LEFT JOIN inventory_attribute_value c on b.attribute_value_id = c.attribute_value_id 
	LEFT JOIN inventory_tag_link d on a.tag_id = d.tag_id AND d.attribute_id = c.attribute_id
	WHERE a.master_id = {0}  AND c.approved = true
	ORDER BY d.order_id;", inventory.master_id),null);
								foreach (DataRow dr in _dt.Rows)
									{
									attribute_name = Toolbox.do_value_from(dr["attribute"]);
									value_name = Toolbox.do_value_from(dr["value"]);
									att_vals += string.Format(@"
															<tr>
																<td class='l' width='150'>{0}:</td>
																<td class='r' colspan='2'>{1}</td>
															</tr>", attribute_name, value_name);
									}
								sb_general.AppendFormat(@"
													<div class='_panel'>
														<div class='_title'>Attributes & Values</div>
														<table cellpadding='2' cellspacing='0' width='100%'>
															{0}
														</table>
													</div>", att_vals);

								#endregion Attributes / Values

								if (is_purchaser)
									{
									#region Analysis

									var a_fig = new analysis_figures();
									var fis_last_start = Toolbox.MySQL_shortdt(warehouseBusinessUnit.fiscal_start_previous);
									var fis_last_end = Toolbox.MySQL_shortdt(warehouseBusinessUnit.fiscal_end_previous);
									var fis_this_start = Toolbox.MySQL_shortdt(warehouseBusinessUnit.fiscal_start_current);
									var fis_this_end = Toolbox.MySQL_shortdt(warehouseBusinessUnit.fiscal_end_current);

									#region Define QTY Sold

									a_fig.qtysold_last_fiscal = Toolbox.doSQL_double(conn,@"SELECT IFNULL(SUM(a.wo_detail_history_qty_committed),0) FROM wo_detail_history a LEFT JOIN woprog b ON a.wo_detail_history_woprog_id = b.woprog_id WHERE a.business_unit_id = @v0  AND a.wo_detail_history_master_id = @v1  AND b.woprog_invoicedate BETWEEN @v2  AND @v3  ", new object[] {  inventory.business_unit_id, inventory.master_id, fis_last_start, fis_last_end } );
									a_fig.qtysold_this_fiscal = Toolbox.doSQL_double(conn,@"SELECT IFNULL(SUM(a.wo_detail_history_qty_committed),0) FROM wo_detail_history a LEFT JOIN woprog b ON a.wo_detail_history_woprog_id = b.woprog_id WHERE a.business_unit_id = @v0  AND a.wo_detail_history_master_id = @v1  AND b.woprog_invoicedate BETWEEN @v2  AND @v3  ", new object[] {  inventory.business_unit_id, inventory.master_id, fis_this_start, fis_this_end } );

									#endregion Define QTY Sold

									#region Define $ Sold

									a_fig.dolsold_last_fiscal = Toolbox.doSQL_double(conn,@"SELECT IFNULL(SUM(a.wo_detail_history_qty_committed*a.wo_detail_history_price_sell),0) FROM wo_detail_history a LEFT JOIN woprog b ON a.wo_detail_history_woprog_id = b.woprog_id WHERE a.business_unit_id = @v0  AND a.wo_detail_history_master_id = @v1  AND b.woprog_invoicedate BETWEEN @v2  AND @v3  ", new object[] {  inventory.business_unit_id, inventory.master_id, fis_last_start, fis_last_end } );
									a_fig.dolsold_this_fiscal = Toolbox.doSQL_double(conn,@"SELECT IFNULL(SUM(a.wo_detail_history_qty_committed*a.wo_detail_history_price_sell),0) FROM wo_detail_history a LEFT JOIN woprog b ON a.wo_detail_history_woprog_id = b.woprog_id WHERE a.business_unit_id = @v0  AND a.wo_detail_history_master_id = @v1  AND b.woprog_invoicedate BETWEEN @v2  AND @v3  ", new object[] {  inventory.business_unit_id, inventory.master_id, fis_this_start, fis_this_end } );

									#endregion Define $ Sold

									#region Define % Margin

									var this_fisc_cost = Toolbox.doSQL_double(conn,@"SELECT IFNULL(SUM(a.wo_detail_history_price_cost),0) FROM wo_detail_history a LEFT JOIN woprog b ON a.wo_detail_history_woprog_id = b.woprog_id WHERE a.business_unit_id = @v0  AND a.wo_detail_history_master_id = @v1  AND b.woprog_invoicedate BETWEEN @v2  AND @v3  ", new object[] {  inventory.business_unit_id, inventory.master_id, fis_this_start, fis_this_end } );
									var last_fisc_cost = Toolbox.doSQL_double(conn,@"SELECT IFNULL(SUM(a.wo_detail_history_price_cost),0) FROM wo_detail_history a LEFT JOIN woprog b ON a.wo_detail_history_woprog_id = b.woprog_id WHERE a.business_unit_id = @v0  AND a.wo_detail_history_master_id = @v1  AND b.woprog_invoicedate BETWEEN @v2  AND @v3  ", new object[] {  inventory.business_unit_id, inventory.master_id, fis_last_start, fis_last_end } );
									var this_fisc_sell = Toolbox.doSQL_double(conn,@"SELECT IFNULL(SUM(a.wo_detail_history_price_sell),0) FROM wo_detail_history a LEFT JOIN woprog b ON a.wo_detail_history_woprog_id = b.woprog_id WHERE a.business_unit_id = @v0  AND a.wo_detail_history_master_id = @v1  AND b.woprog_invoicedate BETWEEN @v2  AND @v3  ", new object[] {  inventory.business_unit_id, inventory.master_id, fis_this_start, fis_this_end } );
									var last_fisc_sell = Toolbox.doSQL_double(conn,@"SELECT IFNULL(SUM(a.wo_detail_history_price_sell),0) FROM wo_detail_history a LEFT JOIN woprog b ON a.wo_detail_history_woprog_id = b.woprog_id WHERE a.business_unit_id = @v0  AND a.wo_detail_history_master_id = @v1  AND b.woprog_invoicedate BETWEEN @v2  AND @v3  ", new object[] {  inventory.business_unit_id, inventory.master_id, fis_last_start, fis_last_end } );
									a_fig.permarg_last_fiscal = last_fisc_sell == 0 ? 0 : (last_fisc_sell - last_fisc_cost)/last_fisc_sell;
									a_fig.permarg_this_fiscal = this_fisc_sell == 0 ? 0 : (this_fisc_sell - this_fisc_cost)/this_fisc_sell;

									#endregion Define % Margin

									sb_general.AppendFormat(@"
														<div class='_panel'>
															<div class='_title'>Analysis</div>
															<table cellpadding='2' cellspacing='0' width='100%'>
																<tr>
																	<td class='l'>QTY Sold this fiscal:</td>
																	<td class='r'>{0:N0}</td>
																</tr>
																<tr>
																	<td class='l'>QTY Sold last fiscal</td>
																	<td class='r'>{1:N0}</td>
																</tr>
																<tr>
																	<td class='l'>$ Sold This Fiscal</td>
																	<td class='r'>{2:C2}</td>
																</tr>
																<tr>
																	<td class='l'>$ Sold Last Fiscal</td>
																	<td class='r'>{3:C2}</td>
																</tr>
																<tr>
																	<td class='l'>% Margin This Fiscal</td>
																	<td class='r'>{4:P2}</td>
																</tr>
																<tr>
																	<td class='l'>% Margin Last Fiscal</td>
																	<td class='r'>{5:P2}</td>
																</tr>
															</table>
														</div>",
										a_fig.qtysold_this_fiscal,
										a_fig.qtysold_last_fiscal,
										a_fig.dolsold_this_fiscal,
										a_fig.dolsold_last_fiscal,
										a_fig.permarg_this_fiscal,
										a_fig.permarg_last_fiscal
										);

									#endregion Analysis
									}

								#region Add a list of vendors that sell this manufacturer.

								var man = "";
								try
									{
									man = Toolbox.doSQL_string(conn, @"Select ifnull(MAX(inventory_attribute_value.`value`),'') from 
																		inventory_attribute_value INNER JOIN inventory_item_detail 
ON inventory_item_detail.attribute_value_id = inventory_attribute_value.attribute_value_id where
																		inventory_item_detail.master_id =@v0  and inventory_attribute_value.attribute_id = 16",
										new object[] { inventory.master_id});
									}
								catch {}
								if (man != "")
									{
									var sql = @"SELECT DISTINCT
															inventory_attribute_value.`value`,
															vendor.Vendor_Name,
															Concat('(',address.address_phonearea,')',address.address_phonefirst,'-',address.address_phonelast) phone,
																vendor.vendor_id
															FROM
															inventory_attribute_value
															INNER JOIN inventory_item_detail ON inventory_item_detail.attribute_value_id = inventory_attribute_value.attribute_value_id
															LEFT JOIN inventory_price ON inventory_item_detail.master_id = inventory_price.master_id
															INNER JOIN vendor ON inventory_price.vendor_id = vendor.Vendor_ID AND vendor.Vendor_Name is not null
															INNER JOIN address ON vendor.vendor_id = address.address_table_id and address.address_table = 'Vendor' 
															where inventory_attribute_value.attribute_id = 16 and inventory_attribute_value.`value` = '" + man + "'";
									try
										{
										var dt = Toolbox.doSQL_dt(conn,sql  , null);
										if (dt.Rows.Count > 0)
											{
											sb_general.AppendFormat(@"
													<div class='_panel'>
														<div class='_title'><img src='/images/icon/icon[bluesquare].gif' align='absmiddle'/> Vendors Selling This Manufacturer</div>
														<table cellpadding='2' cellspacing='0'>
															<tr>
																<td class='l' width='125'>Vendor</td>
																<td class='l' width='50'>Phone Number</td>
															</tr>");
											foreach (DataRow dr in dt.Rows)
												{
												var stuff = string.Format(@"<a href=""javascript:boing('../../vendor/index.aspx?vendor_id={1}&first_tab=true','Vendor',950,800);"">{0}</a><br/>", Toolbox.do_value_from(dr[1]), dr[3]);
												sb_general.AppendFormat(@"
													<tr>
																<td class='r'>{0}</td>
																<td class='r'>{1}</td>
															</tr>",
													stuff,
													dr["phone"]);
												}
											sb_general.Append("</table></div>");
											}
										}
									catch {}
									}

								#endregion  Add a list of vendors that sell this manufacturer.

								sb_general.Append(@"</td>
												<td class='properties' valign='top'>");

								#region Purchaser Stock

								if (is_purchaser)
									{
									#region Business Unit
									var bu_qty_in_stock = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(a.qty), 0) FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = @v0 AND a.master_id = @v1  and b.type_id = 1", new object[] {  warehouseBusinessUnit.id, inventory.master_id } );
									var bu_qty_external = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(a.qty), 0) FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = @v0 AND a.master_id = @v1  and b.type_id = 2", new object[] {  warehouseBusinessUnit.id, inventory.master_id } );
									var bu_qty_on_hand = Toolbox.doSQL_double(@"SELECT IFNULL(MAX(onhand_qty), 0) FROM inventory_branch WHERE business_unit_id = @v0  AND master_id = @v1 ", new object[] {  warehouseBusinessUnit.id, inventory.master_id } );
									DataRow bu_minmax = null;
									var bu_min = 0;
									var bu_max = 0;
									double bu_dollar_b = 0;
									try
										{
										bu_minmax = Toolbox.doSQL_dt(conn,@"SELECT min_qty,max_qty,dollar_balance FROM inventory_branch WHERE business_unit_id = @v0  AND master_id = @v1 ", new object[] {  warehouseBusinessUnit.id, inventory.master_id } ).Rows[0];
										}
									catch {}
									if (bu_minmax != null)
										{
										bu_min = (int) bu_minmax["min_qty"];
										bu_max = (int) bu_minmax["max_qty"];
										bu_dollar_b = bu_minmax["dollar_balance"] != DBNull.Value ? Convert.ToDouble(bu_minmax["dollar_balance"]) : 0;
										}
									var bu_warning = bu_qty_in_stock > bu_max || bu_qty_in_stock < bu_min || bu_qty_in_stock == 0 ? "color:red;" : "color:#000;";
									bu_warning += bu_qty_in_stock > bu_max ? "font-family:arial black;" : "";
									bu_warning += bu_qty_in_stock < bu_min ? "font-family:arial;font-style:oblique;" : "";
									bu_warning += bu_qty_in_stock == 0 ? "font-family:arial narrow;font-style:oblique;" : "";
									var bu_qty_totals = Toolbox.doSQL_dt(conn,@" SELECT IF(po_details_line_active = 1, IFNULL(SUM((po_details_qty_ordered - po_details_qty_received)*po_details_vendor_qty_per), 0), 0) on_order, IFNULL(SUM(po_details_qty_ordered)*po_details_vendor_qty_per, 0) committed, (SELECT IFNULL(SUM(wo_detail_current_qty_committed), 0) FROM wo_detail_current WHERE wo_detail_current_master_id = @v0  AND business_unit_id = @v1 ) on_workorders, (SELECT IFNULL(SUM(wo_detail_current_qty_ordered - wo_detail_current_qty_committed), 0) FROM wo_detail_current WHERE wo_detail_current_master_id = @v0  AND business_unit_id = @v1 ) qty_req FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_part_no = @v0  AND a.po_details_line_active = 1 AND b.business_unit_id = @v1", new object[] {  inventory.master_id, workingBusinessUnit.id  } ).Rows[0];
									var bu_qty_on_order = Convert.ToDouble(bu_qty_totals["on_order"]);
									var bu_qty_committed = Convert.ToDouble(bu_qty_totals["committed"]);
									var bu_qty_on_workorders = Convert.ToDouble(bu_qty_totals["on_workorders"]);
									var bu_qty_req = Convert.ToDouble(bu_qty_totals["qty_req"]);
									var bu_qty_to_order = bu_qty_req - bu_qty_on_order - bu_qty_in_stock + bu_min;
									bu_qty_to_order = bu_qty_to_order <= 0 ? 0 : bu_qty_to_order;
									var bu_row_id = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM inventory_location WHERE master_id = @v0 AND business_unit_id = @v1", new object[] { inventory.master_id,warehouseBusinessUnit.id}) > 0 ? Toolbox.doSQL_string(conn, @"SELECT MAX(id) FROM inventory_location WHERE master_id = @v0 AND business_unit_id = @v1", new object[] { inventory.master_id, workingBusinessUnit.id}) : "";
									var bu_location = bu_row_id != "" ? Toolbox.doSQL_string(conn, @"SELECT name FROM inventory_location WHERE id = @v0", new object[] { bu_row_id}) : "";
									#endregion Business Unit

									sb_general.AppendFormat(@"
														<div class='_panel_report'>
														<div class='_title'>Stock - Warehouse</div>
															<table cellpadding='2' cellspacing='0'>
																<tr>
																	<td class='l'>On Order:</td>
																	<td class='r'>{4:N0}</td>
																</tr>
																<tr>
																	<td class='l'>Committed:</td>
																	<td class='r'>{6:N0}</td>
																</tr>
																<tr>
																	<td class='l'>To Order:</td>
																	<td class='r'>{7:N0}</td>
																</tr>
																<tr>
																	<td class='l'>Dollar Balance:</td>
																	<td class='r' title='Total dollar balance for the on hand quantity.'>{10:C2}</td>
																</tr>
																<tr>
																	<td class='l'>Internal QTY:</td>
																	<td class='r' title='The sum total of all internal locations'><a href='./index.aspx?a=get&tab=PL&id={11}' style='color:#00d'>{0:N0}</a></td>
																</tr>
																<tr>
																	<td class='l'>External QTY:</td>
																	<td class='r' title='The sum total of all external locations'><a href='./index.aspx?a=get&tab=PL&id={11}' style='color:#00d'>{13:N0}</a></td>
																</tr>
																<tr>
																	<td class='l'>Total On Hand QTY:</td>
																	<td class='r' title='The sum total from every location'><a href='./index.aspx?a=get&tab=PL&id={11}' style='color:#00d'>{12:N0}</a></td>
																</tr>
															</table>
														</div>",
										bu_qty_in_stock, // {0}
										bu_min, // {1}
										bu_max, // {2}
										bu_warning, // {3}
										bu_qty_on_order, // {4}
										bu_qty_committed, // {5}
										bu_qty_on_workorders, // {6}
										bu_qty_to_order, // {7}
										bu_row_id, // {8}
										bu_location, // {9}
										bu_dollar_b, // {10}
										inventory.master_id, // {11}
										bu_qty_on_hand, // {12}
										bu_qty_external // {13}
										);
									
									}

								#endregion Purchaser Stock

								#region Cross Company Stock Quantities 
								sb_general.Append(@"
														</div>
														<div class='_panel_report'>");
								var branch_stocks = Toolbox.doSQL_dt(conn, @"SELECT onhand_qty,name
    FROM (
        SELECT 
            a.onhand_qty,
            b.ddl_name AS name 
        FROM 
            inventory_branch a 
        LEFT JOIN 
            business_unit b ON a.business_unit_id = b.id 
        WHERE 
            a.master_id = @v0 
            AND b.active = 'T' 
            AND b.id NOT IN (8, 11, @v1) 
            AND b.has_inventory = 1 

        UNION ALL 

        SELECT 
            SUM(a.onhand_qty) AS onhand_qty, 
            'Total Company cross stock' AS name 
        FROM 
            inventory_branch a 
        LEFT JOIN 
            business_unit b ON a.business_unit_id = b.id 
        WHERE 
            a.master_id = @v0 
            AND b.active = 'T' 
            AND b.id NOT IN (8, 11, @v1) 
            AND b.has_inventory = 1
    ) AS subquery
    ORDER BY 
        CASE 
            WHEN name = 'Total Company cross stock' THEN 1 
            ELSE 0 
        END, 
        name;", new object[] { inventory.master_id, warehouseBusinessUnit.id });

								if (branch_stocks.Rows.Count > 0)
								{
									sb_general.Append(@"
															<div class='_title'><img src='/images/icon/icon[greysquare].gif' align='absmiddle'/> Cross Company Stock Quantities</div>
															<table cellpadding='2' cellspacing='0'>");
									foreach (DataRow dr in branch_stocks.Rows)
									{
										var name = dr["name"].ToString();
										var qty = dr["onhand_qty"] != DBNull.Value ? Convert.ToDouble(dr["onhand_qty"]) : 0;
										sb_general.AppendFormat(@"
														<tr>
															<td class='l'>{0}:</td>
															<td class='r'>{1:N0}</td>
														</tr>",
											name,
											qty
											);
									}
									sb_general.Append(@"</table>
									</div>");
								}
								#endregion Cross Company Stock Quantities

								var sb_alts = new StringBuilder();
								_dt = Toolbox.doSQL_dt(conn,@"SELECT Inventory_Alternate_Alternate_Master_ID master_id, full_part_description(Inventory_Alternate_Alternate_Master_ID, false, '') descr FROM inventory_alternate  WHERE inventory_alternate_origin_master_id =@v0", new object[] { inventory.master_id });
								if (_dt.Rows.Count > 0)
									{
									foreach (DataRow dr in _dt.Rows)
										{
										var alt_master_id = dr["master_id"].ToString();
										var alt_description = dr["descr"].ToString();
										sb_alts.AppendFormat("<div style='padding:5px' align='left'><a href='./index.aspx?a=get&tab=G&id={0}' style='color:#000;'><img style='float:left;margin:5px;' src='/_tools/inventory_picture/index.aspx?id={0}' width='35' height='35' align='absmiddle' /><b>{0}</b> - {1}</a></div>", alt_master_id, alt_description);
										}
									}
								else
									{
									sb_alts.Append("<div>No alternates available</div>");
									}
								sb_general.AppendFormat(@"</div>
													<div class='_panel_report'>
														<div class='_title'>Alternate Parts</div>
														<table cellpadding='2' cellspacing='0' width='100%'>
															<tr>
																<td>
																	{0}
																</td>
															</tr>
														</table>
													</div>", sb_alts);

								#region Part of Groups

								var sb_groups = new StringBuilder();
								_dt = Toolbox.doSQL_dt(conn,@"SELECT b.id, b.name FROM inventory_group_dtl a left join inventory_group_hdr b on a.group_id = b.id  where a.master_id =@v0", new object[] { inventory.master_id });
								if (_dt.Rows.Count > 0)
									{
									foreach (DataRow dr in _dt.Rows)
										{
										var group_id = dr["id"].ToString();
										var group_name = Toolbox.do_value_from(dr["name"], true);
										sb_groups.AppendFormat(@"<div style='padding:5px' align='left'><a href=""javascript:boing('/sections/member/picklist/pikclist.aspx?origin=groupings&id={0}', 'groupings',950, 800);"">{1}</a></div>", group_id, group_name);
										}
									}
								else
									{
									sb_groups.Append("<div>Not in a group</div>");
									}
								sb_general.AppendFormat(@"</div>
													<div class='_panel_report' style='overflow-y:scroll;max-height:300px;'>
														<div class='_title'>Groups containing this part</div>
														<table cellpadding='2' cellspacing='0' width='100%'>
															<tr>
																<td>
																	{0}
																</td>
															</tr>
														</table>
													</div>", sb_groups);

								#endregion Part of Groups

								#region Part of Kits

								var sb_kits = new StringBuilder();
								_dt = Toolbox.doSQL_dt(conn,@"SELECT inventory_kit_hdr_id id, inventory_kit_hdr_name name FROM inventory_kit_dtl a left join inventory_kit_hdr b on a.inventory_kit_dtl_hdr_id = b.inventory_kit_hdr_id  where a.inventory_kit_dtl_master_id =@v0", new object[] { inventory.master_id });
								if (_dt.Rows.Count > 0)
									{
									foreach (DataRow dr in _dt.Rows)
										{
										var kit_id = dr["id"].ToString();
										var kit_name = dr["name"].ToString();
										sb_kits.AppendFormat(@"<div style='padding:5px' align='left'><a href=""javascript:boing('/sections/member/picklist/pikclist.aspx?origin=kitted&id={0}', 'kittings',950, 800);"">{1}</a></div>", kit_id, kit_name);
										}
									}
								else
									{
									sb_kits.Append("<div>Not in a kit</div>");
									}
								sb_general.AppendFormat(@"</div>
													<div class='_panel_report' style='overflow-y:scroll;max-height:300px;'>
														<div class='_title'>Kits containing this part</div>
														<table cellpadding='2' cellspacing='0' width='100%'>
															<tr>
																<td>
																	{0}
																</td>
															</tr>
														</table>
													</div>", sb_kits);

								#endregion Part of Kits

								sb_general.Append(@"
													</td>
											</tr>
										</table>
									");
								output += sb_general.ToString();

								#endregion General Part
								}
							else if (q["tab"] == "P")
								{
								lbltemp.Text = "Inventory / Branch Access / Part Viewer / Pricing Tab";
								TagID = inv_class.get_parts_tag(q["id"]);
								canadian_sold_as = Toolbox.doSQL_string(conn, @"SELECT canadian_sold_as FROM inventory_tag WHERE tag_id = @v0", new object[] { TagID});
								usa_sold_as = Toolbox.doSQL_string(conn, @"SELECT usa_sold_as FROM inventory_tag WHERE tag_id =@v0 " , new object[] { TagID});
								output += string.Format(@"
									<input type='hidden' value='{3}' id='country' />
									<input type='hidden' value='{1}' id='canadian_sold_as' />
									<input type='hidden' value='{2}' id='usa_sold_as' />
								<div class='branch_head' align='left'>{0} Pricing</div>
								<div id='this_branch'>&nbsp;</div>
								<div class='branch_head' align='left'>Company Wide Pricing</div>
								<div id='not_this_branch'>&nbsp;</div>
								",
									warehouseBusinessUnit.name, // {0}
									canadian_sold_as, // {1}
									usa_sold_as, // {2}
									warehouseBusinessUnit.country // {3}
									);
								}
							else if (q["tab"] == "H")
								{
								if (show_cost || is_purchaser)
									{
									lbltemp.Text = "Inventory / Branch Access / Part Viewer / History Tab";
									TagID = inv_class.get_parts_tag(q["id"]);
									canadian_sold_as = Toolbox.doSQL_string(conn, @"SELECT canadian_sold_as FROM inventory_tag WHERE tag_id =@v0 ", new object[] { TagID });
									usa_sold_as = Toolbox.doSQL_string(conn, "SELECT usa_sold_as FROM inventory_tag WHERE tag_id =@v0 ", new object[] { TagID });
									output += string.Format(@"
									<input type='hidden' value='{3}' id='country' />
									<input type='hidden' value='{1}' id='canadian_sold_as' />
									<input type='hidden' value='{2}' id='usa_sold_as' />
								<div class='branch_head' align='left'>{0} Pricing History</div>
								<div id='this_branch'>&nbsp;</div>
								<div class='branch_head' align='left'>Company Wide Pricing History</div>
								<div id='not_this_branch'>&nbsp;</div>",
										warehouseBusinessUnit.name, // {0}
										canadian_sold_as, // {1}
										usa_sold_as, // {2}
										warehouseBusinessUnit.country // {3}
										);
									}
								else
									{
									Response.Redirect("./index.aspx?a=get&tab=G&id=" + q["id"], true);
									}
								}
							else if (q["tab"] == "S")
								{
								_dt = Toolbox.doSQL_dt(conn,string.Format(@" SELECT b.name Branch,
CAST(CONCAT('$',FORMAT(IFNULL( CASE (b.country) WHEN 'CDN' THEN IF(d.usa_sold_as = 1 AND d.canadian_sold_as = 2
AND {0}  = 'USA', a.sellprice / 3.2808399, a.sellprice) 
WHEN 'USA' THEN IF(d.canadian_sold_as = 2 AND d.usa_sold_as = 1
AND {0} = 'CDN', a.sellprice * 3.2808399, a.sellprice) END, 0),3)) AS CHAR) 'Sell Price',
a.date_modified 'Date Modified' FROM inventory_sellprice a LEFT join business_unit b ON a.business_unit_id = b.id
LEFT JOIN inventory_item_master c ON a.master_id = c.master_id LEFT JOIN inventory_tag d ON c.tag_id = d.tag_id
WHERE a.master_id = @v0  AND b.id != 8 ORDER BY Branch", warehouseBusinessUnit.country),new object[] {  q["id"]  } );
								output += inv_class.report_writer(_dt);
								}
							else if (q["tab"] == "NO")
								{
                                /**
                                 * This is intentionally passing business_unit_id instead warehouse business_unit_id. One BU might have different notes than another. 
                                 */

								output += string.Format("<iframe src='./inv_notes.aspx?id={0}' style='width:100%;height:750px;' frameborder='0' />", q["id"]);
								}
							else if (q["tab"] == "SH")
								{
								_dt = Toolbox.doSQL_dt(conn,@"SELECT business_unit_name(business_unit_id) Branch, sellprice 'Sell Price', dt 'Date Modified', get_name(member_id) 'Changed By' FROM inventory_sellprice_history WHERE master_id = @v0  AND business_unit_id = @v1  ORDER BY dt DESC", new object[] {  q["id"], warehouseBusinessUnit.id } );
								output += Toolbox.do_ReportWriter(_dt);
								}
							else if (q["tab"] == "A")
								{
								if (q["id"] != null)
									{
									master_id = q["id"];
									}
								output += string.Format("<iframe width='100%' height='800' src='/sections/member/picklist/pikclist.aspx?origin=alternates&id={0}' frameborder='0'></iframe>", master_id);
								}
							else if (q["tab"] == "BC")
								{
								output += string.Format(@"
       									<table width='100%' cellspacing='0' cellpadding='0' class='barcodes'>
       										<thead>
       											<tr>
       												<th>Type</th>
       												<th>Associated Vendor/Manufacturer</th>
       												<th>Barcode</th>
       												<th>Active</th>
       												<th>&nbsp;</th>
       											</tr>
       											<tr>
       												<td align='center'>
       													<select id='link_type' onchange='clear_associated(this)'>
       														<option value='V'>Vendor</option>
       														<option value='M'>Manufacturer</option>
       													</select>
       												</td>
       												<td align='center'><input type='text' class='linked_id' data-master_id='{0}' onfocus='attacher(this);'/></td>
       												<td align='center'><input type='text' class='barcode' onkeydown='catch_enter(this)'/></td>
													<td align='center' width='20'><input type='checkbox' checked='checked' disabled/></td>
       												<td align='center' width='35'><button id='save_link' type='button' onclick='save_barcode(this)'><img src='/images/icon/icon[save].gif' align='absmiddle' /></button></td>
       											</tr>
       										</thead>
       										<tbody>
       										</tbody>
       									</table>
       									", q["id"]);
								}
							else if (q["tab"] == "U")
								{
								#region PO

								output += "<br/><div align='left'><b>Purchases</b></div><iframe width='100%' id='if_po' height='345' frameborder='0' src='./usage.aspx?type=po&master_id=" + q["id"] + "&business_unit_id=" + workingBusinessUnit.id + "'></iframe>";

								#endregion PO

								#region WO

								output += "<div align='left'><b>Work Orders</b></div><iframe width='100%' id='if_wo' height='345' frameborder='0' src='./usage.aspx?type=wo&master_id=" + q["id"] + "&business_unit_id=" + workingBusinessUnit.id + "'></iframe>";

								#endregion WO
								}
							else if (q["tab"] == "QH")
								{
								output += "<br/><br/><iframe width='100%' id='if_qh' height='2000px' frameborder='0' src='./history.aspx?master_id=" + q["id"] + "&business_unit_id=" + warehouseBusinessUnit.id + "'></iframe>";
								}
							else if (q["tab"] == "CO")
								{
								output += string.Format(@"
							<div align='left'>
							<table cellpadding='5' cellspacing='0' id='consignment_new' width='600' ;margin:5px;'>
									<tr>
									<td>Customer</td><td align='left'><input type='text' class='t_customer' style='text-align:center;width:250px;font-size:11px;font-family:arial;' id='t_customer' data-click=""$(this).val(item[2]);"" onfocus=""attach_ac(this, 'customer')""/></td>
									</tr><tr>
									<td>Serial #</td><td align='left'><input type='text' class='t_serial' id='t_serial' style='text-align:center;width:250px;;font-size:11px;font-family:arial;' /></td>
									</tr><tr>
									<td></td><td align='left'><button type='button' onclick='consignment_save(this)' style='font-size:11px;width:65px;font-weight:bold'><img src='/images/icon/icon[save].gif' align='absmiddle' /> Add</button></td>
									</tr>
							</table>
							<script>
								$(document).ready(function()
									{{
									$('#add_table').find('.t_customer').focus();
									}});
							</script></div>",
									Convert.ToDouble(inventory.cost_price_branch*.5).ToString("N2")
									);
								up_consignment.Visible = true;
								}
							else if (q["tab"] == "PL")
								{
								gv_partlocations.Visible = true;
								gv_partlocations.Enabled = is_purchaser;
								gv_partlocations.Settings.ShowTitlePanel = is_purchaser;
								gv_partlocations.Columns["Actions"].Visible = is_purchaser;
								output += is_purchaser ? @"
<script>
	$('document').ready(function()
		{
		populate_locations(true);
		try {
		$('#ctl00_cphMasterBody_gv_partlocations_Title_add_name')[0].selectedIndex = -1;
		}catch(e){}
		$('#transfer_window').dialog(	{
										autoOpen: false,
										title: 'Transfer location stock',
										modal:		true,
										width:		350,
										resizable: false,
										close:		function()
														{
														$('.stock_detail .tr').find('.td').each(function()
															{
															$(this).css({'background-color':''});
															});
														}
										});
		});
</script>
<div id='transfer_window'>
	<table width='100%'>
		<tr>
			<td>Transferring <b>FROM</b>:</td>
			<td><select style='width:175px' id='location_from'></select></td>
		</tr>
		<tr>
			<td>Transferring <b>TO</b>:</td>
			<td><select style='width:175px' id='location_to'></select></td>
		</tr>
		<tr>
			<td>Quantity:</td>
			<td><input id='transfer_qty' type='text' onkeydown='only_numeric(event, 5)' size='5' /> - Max of <b id='transfer_maxqty'></b></td>
		</tr>
		<tr>
			<td colspan='2' align='center'><button type='button' onclick='location_xfer(this)'>Transfer</button><button type='button' onclick=""$('#transfer_window').dialog('close')"">Cancel</button></td>
		</tr>
	</table>
</div>" : "";
								}

							#region Image Upload Jscript

							output += @"
							</div>
							<script> 
								var options =	{
												beforeSubmit:	function(formData, jqForm, options)
																	{
																	var picture_name		= $('#_picture').val();
																	var picture_blob		= $('.inventory_file_blob').val();
																	var isMatch				= picture_name.match(/\.jpe?g/gi);
																	if(picture_blob == '' && !isMatch)
																		{
																		alert('Please supply a picture');
																		return false;
																		}
																	},
												success:		function(responseText, statusText)
																	{
																	$('#part_image_pop').fadeOut().attr('src', '/_tools/inventory_picture/index.aspx?id=" + q["id"] + @"&ts='+Math.random()*11.33).fadeIn();
																	$('#part_image').fadeOut().attr('src', '/_tools/inventory_picture/index.aspx?id=" + q["id"] + @"&ts='+Math.floor(Math.random()*11.33)).fadeIn();
																	branch_inventory.paste_image.remove();
																	}
												};
								$(document).ready(function()
										{
										$('#new_pic').dialog(
														{
														resizable: false, 
														autoOpen: false, 
														height: 450, 
														modal:false,
														width: 700,
														title: 'Update Image',
														closeOnEscape: true, 
														close: function()
																{
																
																},
														open: function()
																{
																$('#picture_submit').attr('ENCTYPE', 'multipart/form-data').ajaxForm(options);
																branch_inventory.paste_image.init();
																}
														});
										$('#part_image').click(function()
																{
																$('#new_pic').dialog('open');
																}).attr('src', '/_tools/inventory_picture/index.aspx?id=" + q["id"] + @"').fadeIn();";

							#endregion Image Upload Jscript

							#region populate pricing for P & H (Pricing/Historic Pricing)

							if (q["tab"] == "P" || q["tab"] == "H")
								{
								var see_cost = show_cost || is_purchaser;
								var can_edit = is_purchaser;
								var is_historic = q["tab"] == "H";
								var _disabler = is_historic ? "disabled" : disabler;
								output += string.Format(@"
											populate_pricing(1, '{0}', {1}, {2}, {3});
											populate_pricing(0, '{0}', {1}, {2}, {3});
											",
									_disabler,
									is_historic.ToString().ToLower(),
									can_edit.ToString().ToLower(),
									see_cost.ToString().ToLower()
									);
								}
								#endregion populate pricing for P & H (Pricing/Historic Pricing)

							else if (q["tab"] == "BC")
								{
								output += @"
											populate_barcodes();
											";
								}
							output += @"
											});	
								</script>";
							}
						}
					}
				else if (action == "link_picture")
					{
					Response.Clear();
					if (q["id"] != null && inventory.part_exists(q["id"]))
						{
						output = string.Format(@"
<html>
	<head>
		<title>Image Submission</title>
		<script type='text/javascript' src='/js/jquery-1.3.2.min.js'></script>
		<script type='text/javascript' src='/js/jquery.paste.js'></script>
		<script type='text/javascript' src='/js/inventory/_branch.js'></script>
		<script>
			function do_submit()
				{{
				var picture_name		= $('#_picture').val();
				var picture_blob		= $('.inventory_file_blob').val();
				var isMatch				= picture_name.match(/\.jpe?g/gi);
				if(picture_blob == '' && !isMatch)
					{{
					alert('Please supply a picture');
					return false;
					}}
				$('#aspnetForm').submit();
				}}
			$('document').ready(
				function()
					{{
					branch_inventory.paste_image.init();
					}});
		</script>
		<style type='text/css'>
			* {{ font-family: Arial; font-size:12px; }}
		</style>
	</head>
	<body>
		<form name='aspnetForm' method='post' action='index.aspx' id='aspnetForm' enctype='multipart/form-data'>
			<b style='color:#f00;font-size:11px;'>Only use .jpg files when adding pictures</b><br/>
			<table cellpadding='2' cellspacing='2' width='95%'>
				<tr>
					<td valign='top'>
						<div style='width:300px;height:300px;margin-bottom:15px;' >
							<img height='300' width='300' id='part_image_pop' src='/_tools/inventory_picture/index.aspx?id={0}' style='border:solid 2px #000;' />
						</div>
					</td>
					<td>
						<div id='paste_box' style=""border: 2px dashed #999;background-image: url('/images/ticket/bg[paste-sm].png');background-color: #fff;background-position: center top;background-repeat: no-repeat;position: relative;width:300px;height:300px;"">
						</div>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
					<td>
						<div id='new_pic' style='width:320px;text-align:center'>
							<center>
							<input type='hidden' name='a' value='update_picture'/>
							<input type='hidden' class='inventory_file_blob' name='inventory_file_blob' value=''/>
							<input type='hidden' id='master_id' name='master_id' value='{0}'/>
							<input type='hidden' name='close' value='false'/>
							<input name='picture' id='_picture' type='file'/>
							<table>
								<tr>
									<td><input type='submit' value='Submit' onmousedown='do_submit()'/></td>
									<td><button type='button' disabled='true' class='inventory_cancelpasted' onclick='branch_inventory.paste_image.remove()'>Cancel Pasted Image</button> </td>
								</tr>
							</table>
							</center>
						</div>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>", q["id"]);
						}
					else
						{
						output = "Invalid ID";
						}
					Response.Write(output);
					Response.End();
					}
				else if (action == "get_sold_as")
					{
					inv_class.get_sold_as(current_user);
					}
				else if (action == "push_to_tag")
					{
					if (q["tag_id"] != null && q["attribute_value_id"] != null && q["attribute_id"] != null)
						{
						TagID = q["tag_id"];
						var attribute_value_id = q["attribute_value_id"];
						attribute_id = q["attribute_id"];
						try
							{
							Toolbox.doSQL_void(conn, @"INSERT INTO inventory_tag_preset (tag_id, attribute_id, value_id) 
VALUES (@v0, @v1, @v2)", new object[] { TagID, attribute_id, attribute_value_id});
							Response.Write("SUCCESS");
							}
						catch (Exception ee)
							{
							throw ee;
							}
						}
					else
						{
						Response.Write("FAILED");
						}
					Response.End();
					}
				else if (action == "copy_price")
					{
					inv_class.copy_price();
					}
				else if (action == "wo_switch_part")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					if (!string.IsNullOrEmpty(q["master_id"]) && !string.IsNullOrEmpty(q["row_id"]) && !string.IsNullOrEmpty(q["business_unit_id"]))
						{
						inventory = new inventory();
						inventory.Load(q["master_id"], q["business_unit_id"]);
						if(inventory.cost_price_branch == 0)
							{
							Response.Write("The cost price on this part is zero, please add a vendor price before applying this part.");
							Response.End();
							}
						var wodc = new NeWODetailCurrent(Convert.ToInt32(q["row_id"]));
						var this_wo = new NeWOProg(wodc.woprog_id);
						var this_c = Toolbox.doSQL_int(conn,@"SELECT COUNT(*)
FROM wo_detail_current WHERE wo_detail_current_master_id = @v0 AND wo_detail_current_woprog_id = @v1",
							new object[] { inventory.master_id, wodc.woprog_id});
						if (this_c > 0)
							{
							Response.Write("This part already exists on the work order");
							Response.End();
							}
						var was_desc = wodc.description;
						wodc.id = Convert.ToInt32(q["row_id"]);
						var prev_description = wodc.description;
						wodc.master_id = Convert.ToInt32(inventory.master_id);
						wodc.description = inventory.description;
						wodc.cost = inventory.cost_price_branch;
						wodc.sell = inventory.sell_price;
						wodc.unit = inventory.sell_price;
						wodc.qty_ordered = 0;
						wodc.qty_invoiced = 0;
						wodc.qty_committed = 0;
						wodc.notes = " Was part 777, changed to " + wodc.master_id + " by " + current_user.FullName + ",  Orig. Desc: " + prev_description;
						wodc.save(current_user, "/sections/member/expense/if_newexpense.aspx.cs - b_save_Click #2", false);

						#region Save Change Record

						var change = new NeWOProgChanges();
						change.WoProgChanges_WOProg_ID = (int) wodc.woprog_id;
						change.WoProgChanges_WasQty = "0";
						change.WoProgChanges_IsQty = "0";
						change.WoProgChanges_WasPrice = "0";
						change.WoProgChanges_IsPrice = wodc.sell.ToString();
						change.WoProgChanges_WasPartNo = "777";
						change.WoProgChanges_IsPartNo = wodc.master_id.ToString();
						change.WoProgChanges_WasDesc = was_desc;
						change.WOProgChanges_WasBillingType = 0;
						change.WoProgChanges_IsBillingType = 0;
						change.WOProgChanges_ManualPriceChange = 0;
						change.business_unit_id = wodc.business_unit_id;
						change.WoProgChanges_Modified_Member_ID = current_user.id;
						change.WoProgChanges_BVWO = wodc.bvwo.ToString();
						change.WoProgChanges_BVWORec = wodc.rec_no.ToString();
						change.WoProgChanges_DeleteFlag = "false";
						change.WoProgChanges_DateTime = Toolbox.MySQLNow_long();
						change.ModifiedByFullName = current_user.FullName;
						change.was_req_qty = 0;
						change.AddtoWOProgChanges();

						#endregion Save Change Record

						#region Send email to PM

						var this_pm = new NeMember(Convert.ToInt32(this_wo.intProjectManager));
						if (this_pm.NEEmail != "")
							{
							var pm_notify = new NeEMail();
							pm_notify.From = "administrator@thatsnew.com";
							pm_notify.To = this_pm.NEEmail;
							pm_notify.Subject = string.Format("{1} has changed part # 777 on WO:{0} to part #{2}", this_wo.OrderNumber, current_user.FullName, wodc.master_id);
							pm_notify.Body = "";
							pm_notify.CC = current_user.NEEmail;
							pm_notify.Send();
							}

						#endregion Send email to PM

						returned_string = "SUCCESS";
						}
					else
						{
							returned_string = "FAILED";
						}
					Response.Write(returned_string);
					Response.End();
					}
				else if (action == "new_consignment")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					try
						{
						master_id = q["master_id"];
						ico = new consignment();
						ico.business_unit_id = workingBusinessUnit.id;
						ico.customer_id = Convert.ToInt32(q["customer_id"]);
						ico.serial = q["serial"];
						ico.status = consignment.StatusType.WaitingToBeQuoted;
						ico.master_id = Convert.ToInt32(master_id);
						ico.save();
						//	ico.repair_price			= Convert.ToDouble(_q["repair_price"]);
						returned_string = "SUCCESS";
						}
					catch (Exception ee)
						{
						returned_string = ee.ToString();
						}
					Response.Write(returned_string);
					Response.End();
					}
				else if (action == "edit_consignment")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					try
						{
						master_id = q["master_id"];
						var row_id = Convert.ToInt32(q["id"]);
						ico = new consignment(row_id);
						ico.business_unit_id = workingBusinessUnit.id;
						ico.customer_id = Convert.ToInt32(q["customer_id"]);
						ico.serial = q["serial"];
						//			ico.repair_price			= Convert.ToDouble(_q["repair_price"]);
						ico.status = ico.Convert_Status(q["status"]);
						ico.master_id = Convert.ToInt32(master_id);
						ico.save();
						returned_string = "SUCCESS";
						}
					catch (Exception ee)
						{
						returned_string = ee.ToString();
						}
					Response.Write(returned_string);
					Response.End();
					}
				else if (action == "delete_price")
					{
					inv_class.delete_price();
					}
				else if (action == "edit_price" || action == "new_price")
					{
					inv_class.save_price();
					}
				else if (action == "save_barcode")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					try
						{
						var bc = q["id"] == string.Empty ? new barcode() : new barcode(Convert.ToInt32(q["id"]));
						bc.master_id = Convert.ToInt32(q["master_id"]);
						bc.barcode_no = q["barcode_no"];
						bc.table_type = q["table_type"];
						bc.table_id = /* bc.table_type == "V" ? Toolbox.doSQL_int(conn,@"SELECT vendor_id FROM vendor WHERE vendor_number_int = @v0  LIMIT 1", new object[] {  _q["table_id"] } ) :*/ Convert.ToInt32(q["table_id"]);
						bc.is_active = Convert.ToBoolean(q["is_active"]);
						bc.member_id_added = current_user.id;
						bc.Save();
						returned_string = "SUCCESS";
						}
					catch (Exception ee)
						{
						returned_string = ee.ToString();
						}
					Response.Write(returned_string);
					Response.End();
					}
				else if (action == "json_vend_info")
					{
					Response.Clear();
					master_id = q["master_id"];
					var vendor_number = q["vendor_number"];
					var vend_info = Toolbox.doSQL_dt(conn,@" SELECT CAST(IFNULL(MIN(vendor_code), '') AS CHAR(100)) vendor_code, CAST(IFNULL(MIN(cost),0) AS DECIMAL(15,5)) cost, CAST(IFNULL(MIN(qty),1) AS DECIMAL(15,2)) qty , CAST(IFNULL(MIN(lead_time),14) AS UNSIGNED) lead FROM inventory_price  WHERE master_id =@v0 AND vendor_id =@v1  AND business_unit_id =@v2 ", new object[] { master_id,vendor_number,warehouseBusinessUnit.id }).Rows[0];
					Response.Write(@"
{
""vendor_code"":""" + vend_info["vendor_code"] + @""",
""cost"":""" + vend_info["cost"] + @""",
""qty"":""" + vend_info["qty"] + @""",
""lead"":""" + vend_info["lead"] + @"""
}
");
					Response.End();
					}
				else if (action == "save_rfq_vendor")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					try
						{
						var s_vendor_id = /*Toolbox.do_int(conn,"SELECT vendor_id FROM vendor WHERE vendor_number_int = "+*/ Convert.ToInt32(q["vendor_id"]);
						var s_contact_id = Convert.ToInt32(q["contact_id"]);
						if (s_vendor_id == 0 && s_contact_id == 0)
							{
							throw new Exception("Neither vendor nor contact are supplied.");
							}
						else if (s_vendor_id == 0)
							{
							throw new Exception("Cannot save a blank vendor.");
							}
						else if (s_contact_id == 0)
							{
							throw new Exception("Cannot save a blank contact, you may need to add a contact to this vendor.");
							}
						else
							{
							var rv = new rfq_vendor();
							rv.vendor_id = s_vendor_id;
							rv.contact_id = s_contact_id;
							rv.rfq_header_id = Convert.ToInt32(Session["working_rfq_id"]);
							var code2 = Toolbox.do_RandomString(20);
							while (rv.key_code_exists(code))
								{
								code2 = Toolbox.do_RandomString(20);
								}
							rv.keycode = code2;
							rv.save();
							}
						returned_string = "SUCCESS";
						}
					catch (Exception ee)
						{
						returned_string = ee.ToString();
						}
					Response.Write(returned_string);
					Response.End();
					}
				else if (action == "toggle_consumable")
					{
					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					inventory.Load(q["master_id"], warehouseBusinessUnit.id);
					inventory.branch_obj.is_consumable = q["chked"].ToLower() == "true";
					inventory.branch_obj.save(conn);
					Response.Write("SUCCESS");
					Response.End();
					}
				else if (action == "toggle_include_external")
					{

					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					Session["inv_branch_include_external"] = q["include"];
					Session["inv_gv_orders"] = null;
					Response.Write("SUCCESS");
					Response.End();
					}
				else if (action == "reset_minmax_levels")
					{
					#region reset_minmax_levels

					Response.Clear();
					Toolbox.do_set_plain_header(Response);
					if (is_purchaser)
						{
						try
							{
							var dt_minmax = Toolbox.doSQL_dt(conn,@"SELECT id FROM inventory_location WHERE business_unit_id = @v0  AND (min != 0 OR max != 0)", new object[] {  warehouseBusinessUnit.id } );
							var inv_l = new location();
							var inv_al = new ArrayList();
							foreach (DataRow dr_m in dt_minmax.Rows)
								{
								var loc_id = Convert.ToInt32(dr_m["id"]);
								inv_al.Add(loc_id);
								}
							inv_l = new location(inv_al);
							foreach (location il in inv_l.items)
								{
								il.min = 0;
								il.max = 0;
								}
							inv_l.member_id = current_user.id;
							inv_l.mass_save();
							Response.Write("SUCCESS");
							}
						catch (Exception ee)
							{
							Response.Write(ee.ToString());
							}
						}
					else
						{
						Response.Write("Only purchasers can perform this function");
						}
					Response.End();
					}
				else
					{
					lbltemp.Text = "Inventory / Branch Access / Home";
					button_home.Disabled = true;
					button_home.Style["background-color"] = "#35a";
					pnl_home.Visible = true;
					uc_home_tab.fill_gv_merge();
					uc_home_tab.fill_gv_tocreate();
					if (is_purchaser)
						{
						uc_home_tab.gvtocreate.Columns["New Part #"].Visible = true;
						}
					gl = new NeGridLayouts((int) current_user.id, "gv_merge");
					if (string.IsNullOrEmpty(gl.GridLayout_Gridid) || gl.GridLayout_Gridid == "0")
						{
						gl.GridLayout_Name = "Default";
						gl.GridLayout_Layout = uc_home_tab.gvmerge.FilterExpression;
						gl.member_id = current_user.id;
						gl.GridLayout_Gridid = "gv_merge";
						gl.SaveGridLayout();
						}
					else
						{
						uc_home_tab.gvmerge.FilterExpression = gl.GridLayout_Layout;
						}
					}
				detail.InnerHtml = output;

				#endregion GET
				}
			}

		} // PAGE LOAD
	#region protected methods
	private struct analysis_figures
		{
		public double qtysold_this_fiscal { get; set; }
		public double qtysold_last_fiscal { get; set; }
		public double dolsold_this_fiscal { get; set; }
		public double dolsold_last_fiscal { get; set; }
		public double permarg_this_fiscal { get; set; }
		public double permarg_last_fiscal { get; set; }
		}
	protected void fill_gv_minmax()
		{
		if (Session["inv_gv_minmax"] == null || Request.Form.Count == 0)
			{
			Session["inv_gv_minmax"] = Toolbox.doSQL_dt(@"CALL inventory_branch_qty_tab(@v0, @v1)",new object[] { workingBusinessUnit.id, warehouseBusinessUnit.id } );
			gv_minmax.ClearSort();
			gv_minmax.SortBy((GridViewColumn)gv_minmax.Columns["master_id"], DevExpress.Data.ColumnSortOrder.Ascending);
			}
		gv_minmax.DataSource = Session["inv_gv_minmax"];

		gv_minmax.DataBind();

		}
	protected string branches_selector()
		{
		var sb						= new StringBuilder();
		var business_unit_id		= workingBusinessUnit.id;
		var visibleTaxEntities		= new Current_User().visible_tax_entities.Split(',').Select(int.Parse).ToList();
		var visibleBusinessUnits	= new Current_User().visible_business_units.Split(',').Select(int.Parse).ToList();
		using (var uow = new UnitOfWork())
			{
			var tax_entities	= from t in new XPQuery<ne_xpo.cs.tax_entity>(uow)
								  where t.is_active && visibleTaxEntities.Contains(t.id)
								  select t;
			foreach(var te in tax_entities)
				{
				var business_units = from b in new XPQuery<ne_xpo.cs.business_unit>(uow)
									 where b.tax_entity_id == te.id && visibleBusinessUnits.Contains(b.id)
									 select new
										 {
										 id = b.id,
										 name = b.ddl_name,
										 has_inventory = b.has_inventory == 1,
										 old_company_id = b.old_company_id
										 };
				var optgroup_added		= 0;
				foreach(var bu in business_units)
					{
					if(!bu.has_inventory) continue;
					if(optgroup_added == 0)
						{
						sb.AppendFormat("<optgroup label='{0}'>", te.ddl_name);
						optgroup_added++;
						}
					var selected		= bu.id == business_unit_id ? "selected" : "";
					sb.AppendFormat(@"
   	<option value='{0}'{2}>{1}</option>
   	", bu.id, bu.name, selected);
					}
				sb.Append("</optgroup>");
				}
			}
		return sb.ToString();
		}

	public class new_part_obj
		{
		public int tag_id {get;set;}
		public int[] att_vals {get;set;}
		public new_price_obj[] prices {get;set;}
		}
	public class new_price_obj
		{
		public int VendorId {get; set;}
		public string VendorPartNo{get; set;}
		public double CostTotal {get; set;}
		public double Quantity {get; set;}
		public double CostPrice {get;set;}
		}
	protected void groups_search_Click(object _sender, EventArgs _e)
		{
		temp_master_id.Value							= group_part_search.Text;
		}
	protected void group_hdrs_CustomButtonCallback(object _sender, ASPxDataDeletingEventArgs _e)
		{
		var tools			= new Toolbox();
		tools.getSQL_void(@"DELETE FROM inventory_group_dtl WHERE group_id =@v0 ", new object[] { _e.Keys[0]});
		tools.getSQL_void("DELETE FROM inventory_group_hdr WHERE id =@v0 ", new object[] { _e.Keys[0] });
		_e.Cancel				= true;
		temp_master_id.Value	= group_part_search.Text;
		DataTable dt;
		if(temp_master_id.Value == "")
			{
			dt					= tools.getSQL_datatable(@"SELECT id, name name, get_name(member_id) created_by, edited_dt, (SELECT COUNT(master_id) FROM inventory_group_dtl b  WHERE b.group_id = a.id) n_parts, notes Notes FROM inventory_group_hdr a" , null);
			}
		else
			{
			dt					= tools.getSQL_datatable(@"SELECT id, name name, get_name(member_id) created_by, edited_dt, (SELECT COUNT(master_id) FROM inventory_group_dtl b WHERE b.group_id = a.id) n_parts, notes Notes FROM inventory_group_hdr a WHERE id IN (SELECT DISTINCT(group_id) FROM inventory_group_dtl WHERE master_id = @v0 )", new object[] {  temp_master_id.Value } );
			}
		group_hdrs.DataSource	= dt;			
		group_hdrs.DataBind();
		}
	protected void kitted_search_Click(object _sender, EventArgs _e)
		{
		temp_master_id.Value							= kitted_part_search.Text;
		}
	protected void kitted_hdrs_CustomButtonCallback(object _sender, ASPxDataDeletingEventArgs _e)
		{
		var tools			= new Toolbox();
		tools.getSQL_void(@"DELETE FROM inventory_kit_dtl WHERE inventory_kit_dtl_hdr_id=@v0 ", new object[] { _e.Keys[0] });
		tools.getSQL_void("DELETE FROM inventory_kit_hdr WHERE inventory_kit_hdr_id=@v0 ", new object[] { _e.Keys[0] });
		_e.Cancel				= true;
		temp_master_id.Value	= group_part_search.Text;
		DataTable dt;
		if(temp_master_id.Value == "")
				{
				dt		= tools.getSQL_datatable(@" SELECT a.inventory_kit_hdr_id id, a.inventory_kit_hdr_name name, MEMBER_NAME(a.inventory_kit_hdr_created_by) created_by, a.inventory_kit_hdr_edited_dt edited_dt, (SELECT COUNT(b.inventory_kit_dtl_master_id) FROM inventory_kit_dtl b WHERE b.inventory_kit_dtl_hdr_id = a.inventory_kit_hdr_id) n_parts, GET_KITTED_SELL(1, @v0 , a.inventory_kit_hdr_id) sell, a.inventory_kit_hdr_note Notes FROM inventory_kit_hdr a", new object[] {  warehouseBusinessUnit.id } );
				}
			else
				{
				dt		= tools.getSQL_datatable(@" SELECT a.inventory_kit_hdr_id id, a.inventory_kit_hdr_name name, MEMBER_NAME(a.inventory_kit_hdr_created_by) created_by, a.inventory_kit_hdr_edited_dt edited_dt, (SELECT COUNT(b.inventory_kit_dtl_master_id) FROM inventory_kit_dtl b WHERE b.inventory_kit_dtl_hdr_id = a.inventory_kit_hdr_id) n_parts, GET_KITTED_SELL(1, @v0 , a.inventory_kit_hdr_id) sell, a.inventory_kit_hdr_note Notes FROM inventory_kit_hdr a WHERE a.inventory_kit_hdr_id IN (SELECT DISTINCT(c.inventory_kit_dtl_hdr_id) FROM inventory_kit_dtl c WHERE c.inventory_kit_dtl_master_id = @v0 )", new object[] {  temp_master_id.Value } );
				}
		kitted_hdrs.DataSource			= dt;
		kitted_hdrs.DataBind();
		}
	protected void gv_minmax_CustomButtonCallback(object _sender, ASPxGridViewCustomButtonCallbackEventArgs _e)
		{
		var temp_gv			= _sender as ASPxGridView;
		var master_id				= temp_gv.GetRowValues(_e.VisibleIndex, new string[] {"master_id"}).ToString();
        if (_e.ButtonID == "Print_BarCode")
			{
            try
				{
				}
            catch 
				{}
			}
		}
	protected void gv_minmax_CustomJSProperties(object _sender, ASPxGridViewClientJSPropertiesEventArgs _e)
		{
		var gv				= (ASPxGridView) _sender;
        var ds				= new object[gv.VisibleRowCount];
        for(var i = 0; i < gv.VisibleRowCount; i++)
        	{
			ds[i]					= gv.GetRowValues(i, "master_id");
        	}
		_e.Properties["cpid"]		= ds;
		_e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void bt_save_CustomJSProperties(object _sender, CustomJSPropertiesEventArgs _e)
		{
		var bt								= (ASPxButton) _sender;
		var container	= bt.NamingContainer as GridViewDataItemTemplateContainer;
		_e.Properties["cpIndex"]						= container.VisibleIndex;
		}
	protected void cb_saveminmax_Callback(object _source, CallbackEventArgs _e)
		{
		minmaxobj mmo;
		try
			{
			mmo = JSON.Deserialize<minmaxobj>(_e.Parameter);
			}
		catch
			{
			throw new Exception("I think you're trying to save an invalid number.");
			}
		var tools = new Toolbox();
		if (mmo.min > mmo.max)
			{
			throw new Exception("The submitted minimum quantity is greater than the submitted maximum value. \n Not Saved.");
			}
		else
			{
			var ib			= new branch(mmo.master_id, mmo.business_unit_id);
			var il				= new location(mmo.location_master_id, mmo.business_unit_id, mmo.master_id);
			var prev_qty				= il.qty;
			var this_cost			= Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  mmo.master_id, mmo.business_unit_id } );
			il.log_is_manual			= true;
			il.max						= mmo.max;
			il.min						= mmo.min;
			il.member_id = current_user.id;
			var diff					= il.qty - mmo.qty;
			il.qty						= mmo.qty;
			il.section_id				= 1;
			var this_type				= prev_qty > mmo.qty ? 3 : 2;
			il.save();
			il.update_branch(this_type, ib.dollar_balance, diff, this_cost, ib);
			Session["inv_gv_minmax"] = null;
			fill_gv_minmax();
			}

		}
	protected class rfq_obj
		{
		private int _rfq_id;
		public int rfq_id  { get { return _rfq_id; } set { _rfq_id = Convert.ToInt32(value); }}
		public bool is_new { get; set; }
		private ArrayList _vendor_id;
		public ArrayList vendor_id  { get { return _vendor_id; } set { _vendor_id = value; }}
		}
	protected class rfq_obj_line
		{
		private int _id;
		public int id  { get { return _id; } set { _id = Convert.ToInt32(value); }}
		private double _q;
		public double q  { get { return _q; } set { _q = Convert.ToInt32(value); }}
		}
	protected class minmaxobj
		{
		public int master_id  { get; set; }
		public double min { get; set; }
		public double max { get; set; }
		public double qty { get; set; }
		public int location_master_id { get; set; }
		public int business_unit_id { get; set; }
		}
	protected class dellocobj
		{
		private int _m_id;
		private int _lm_id;
		
		public int m_id  { get { return _m_id; } set { _m_id = Convert.ToInt32(value); }}
		public int lm_id { get { return _lm_id; } set { _lm_id = Convert.ToInt32(value); } }

		}
	protected class po
		{
		private int _id;
		private bool _cut_new;
		public int id  { get { return _id; } set { _id = Convert.ToInt32(value); }}
		public bool cut_new {get { return _cut_new; } set { _cut_new = Convert.ToBoolean(value); }}
		private po_line[] _data;
		public po_line[] data { get { return _data; } set { _data = value; }}
		}
	protected class po_line
		{
		private int _master_id;
		private string _vendor_code;
		private double _cost;
		private double _qty;
		private double _to_order;
		private int _lead;
		private int _bu_id;
		
		public int master_id  { get { return _master_id; } set { _master_id = Convert.ToInt32(value); }}
		public string vendor_code  { get { return _vendor_code; } set { _vendor_code = value; }}
		public double cost { get { return _cost; } set { _cost = Convert.ToDouble(value); }}
		public double qty { get { return _qty; } set { _qty = Convert.ToDouble(value); }}
		public double to_order { get { return _to_order; } set { _to_order = Convert.ToDouble(value); }}
		public int bu_id { get { return _bu_id;} set {_bu_id = Convert.ToInt32(value); } }
		public int lead {get { return _lead; } set { _lead = Convert.ToInt32(value); }}
		}
	protected void gv_consignment_HtmlDataCellPrepared(object _sender, ASPxGridViewTableDataCellEventArgs _e)
		{
		if (_e.DataColumn.Caption == "Work Order")
			{
			var woprogid = Toolbox.doSQL_int(@"Select IFNULL(MAX(wo_detail_current_woprog_id), 0) from wo_detail_current where wo_detail_current_consignment_id = @v0",
			new object[] { _e.KeyValue});
			if(woprogid == 0)
				{
				woprogid = Toolbox.doSQL_int(@"Select IFNULL(MAX(wo_detail_history_woprog_id), 0) from wo_detail_history where wo_detail_history_consignment_id = @v0",
					new object[] { _e.KeyValue });
			}

			if (woprogid != 0)
				{
				_e.Cell.Text = string.Format(@"<a href=""javascript:boing('/sections/workorder/index.aspx?woprog_id={0}', 'wo');"">{1}</a>", woprogid,new NeWOProg(woprogid).OrderNumber);
				}
			}
		else if (_e.DataColumn.Caption == "Quote")
			{
			var quoteid = Toolbox.doSQL_int(@"Select IFNULL(MAX(quote_id),0) from quote_worksheet where consignment_id = @v0",
				new object[] { _e.KeyValue });
			var revision = Toolbox.doSQL_int(@"Select IFNULL(MAX(revision),0) from quote_worksheet where consignment_id = @v0",
				new object[] { _e.KeyValue });
			if (quoteid != 0)
				{
				_e.Cell.Text = string.Format(@"<a href='javascript:boing(""/sections/member/quote/index.aspx?a=g&quote_id={0}&revision={1}"",""quote"",1000,750);'>{0}</a>", quoteid, revision);
				}
			}
		else if (_e.DataColumn.Caption == "Quoted Price")
			{
				var repairprice = Toolbox.doSQL_double(@"Select IFNULL(MAX(extended_per),0) from quote_worksheet  where consignment_id =@v0", new object[] { _e.KeyValue });
				_e.Cell.Text = repairprice.ToString("C2");

			}
		}
	protected void Load_Layout(object _sender, ASPxGridViewCustomCallbackEventArgs _e)
		{
		var gv				= (ASPxGridView) _sender;
		if(_e.Parameters != "")
			{
			gv.LoadClientLayout(_e.Parameters);
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
	protected void cb_print_barcode_Callback(object _source, CallbackEventArgs _e)
		{
		var master_id		= _e.Parameter;
        var inv			= new inventory();
		inv.Load(master_id, workingBusinessUnit.id);
        inv.print_barcode_label(1);
		}
	protected void gv_partlocations_hdr_qty_init(object _sender, EventArgs _e)
		{
		var qty									= (HtmlInputControl) (_sender);
		qty.Disabled								= !_bo_obj.stk_adj;
		qty.Value									= !_bo_obj.stk_adj ? "0" : "";
		}
	protected void gv_partlocations_HtmlDataCellPrepared(object _sender, ASPxGridViewTableDataCellEventArgs _e)
			{		
			var gv			= (ASPxGridView) _sender;
			if (_e.DataColumn.FieldName == "qty")
				{
				if(!_bo_obj.stk_adj || !is_purchaser)
					{
					var tb		= (TextBox) gv.FindRowCellTemplateControl(_e.VisibleIndex, _e.DataColumn, "TextBox1");;
					if(tb != null)
						{
						tb.Enabled		= false;
						}
					}
				}
			if (_e.DataColumn.FieldName == "qty" || _e.DataColumn.Caption == "Actions")
				{
				if (!can_adjust_stock)
					{
					_e.Cell.Enabled = false;
					}
				}
			}
	protected void cbp_save_rfq_Callback(object _sender, CallbackEventArgsBase _e)
		{
		}
	protected void gv_vendors_RowDeleting(object _sender, ASPxDataDeletingEventArgs _e)
		{
		var gv				= (ASPxGridView) _sender;
		var rv				= new rfq_vendor(Convert.ToInt32(_e.Keys[0]));
		rv.delete();
		gv.DataBind();
		_e.Cancel					= true;
		}
	protected void b_cancel_Click(object _sender, EventArgs _e)
		{
		if(gv_consignment.Visible)
			{
			gv_consignment.CancelEdit();
			gv_consignment.DataBind();
			}
		}
	protected void b_save_vendor_Click(object _sender, EventArgs _e)
		{
		var b					= (ASPxButton) _sender;
		}
	protected void call_save_Callback(object _source, CallbackEventArgs _e)
		{
		}
	protected void cbox_available_contacts_Callback(object _sender, CallbackEventArgsBase _e)
		{
		var cb					= (DropDownList) _sender;
		cb.DataBind();
		}
	protected void callback_set_vendor_Callback(object _source, CallbackEventArgs _e)
		{
		Session["rfq_vendor_id"]			= _e.Parameter;
		}
	protected string note_handler(object _container)
		{
		var q					= Request.QueryString;
		var c		= _container as GridViewDataItemTemplateContainer;
		var icon							= string.IsNullOrEmpty(gv_consignment.GetDataRow(c.ItemIndex)["note"].ToString()) ? "icon[note_blank].gif" : "icon[note].gif";
		var note							= gv_consignment.GetDataRow(c.ItemIndex)["note"].ToString();
		return "<img src='/images/icon/"+icon+"' class='note' onclick='note_show(this)' data-type='consignment' data-note=\""+note+"\" data-id='"+gv_consignment.GetDataRow(c.ItemIndex)["id"]+"' style='cursor:pointer' data-tooltip=\""+note+"\" />";
		}
	protected void gv_consignment_HtmlEditFormCreated(object _sender, ASPxGridViewEditFormEventArgs _e)
		{
		var gv							= (ASPxGridView) _sender;
		var customer						= (TextBox) gv.FindEditFormTemplateControl("t_customer");
		var save							= (ASPxButton) gv.FindEditFormTemplateControl("b_save");
		var delete						= (ASPxButton) gv.FindEditFormTemplateControl("b_delete");
		var id							= gv.GetDataRow(gv.EditingRowVisibleIndex)["id"];
		var woprogid = Toolbox.doSQL_int(@"Select IFNULL(MAX(wo_detail_current_woprog_id), 0) from wo_detail_current where wo_detail_current_consignment_id =@v0 ", new object[] { id });
		if(woprogid == 0)
			{
			woprogid = Toolbox.doSQL_int(@"Select IFNULL(MAX(wo_detail_history_woprog_id),0) from wo_detail_history where wo_detail_history_consignment_id =@v0 ",new object[] { id });
			}

		if (woprogid != 0)
			{
			delete.Enabled					= false;
			}
		customer.Attributes.Add("onfocus", "attach_ac(this, 'customer')");
		customer.Attributes.Add("data-id", gv.GetDataRow(gv.EditingRowVisibleIndex)["customer_id"].ToString());
		customer.Attributes.Add("data-isac","false");
		save.Attributes.Add("data-id",  gv.GetDataRow(gv.EditingRowVisibleIndex)["id"].ToString());
		save.Attributes.Add("class", "save_button");
		}
	protected void rfq_panel_Init(object _sender, EventArgs _e)
		{
		
		}
	#endregion protected
	private string add_ordinal_suffix(int _num)
		{
		var last2_digits = Math.Abs(_num % 100);
		var last_digit = last2_digits % 10;
		return _num + "thstndrd".Substring((last2_digits > 10 && last2_digits < 14) || last_digit > 3 ? 0 : last_digit * 2, 2);
		}
	protected void cb_merge_Callback(object _source, CallbackEventArgs _e)
		{
		var json_text			= _e.Parameter;
		var rm	= JSON.Deserialize<IList<rfq_merge_item>>(json_text);
		var returned				= "";
		var i						= 1;
		var rfq_id					= 0;
		var business_unit_id				= 0;
		var vrfq				= new VendorRFQ();
		var rfq				= new rfq_vendor();
		if(Session["working_rfq_id"] != null)
			{
			rfq_id					= Convert.ToInt32(Session["working_rfq_id"]);
			vrfq					= new VendorRFQ(rfq_id);
			business_unit_id				= vrfq.business_unit_id;
			}
		var vendor_id				= 0; 
		if(Session["rfq_vendor_id"] != null)
			{
			vendor_id				= Convert.ToInt32(Session["rfq_vendor_id"]);
			}
		//int vendor_number_int		= Toolbox.doSQL_int(@"SELECT vendor_number_int FROM vendor WHERE vendor_id = @v0 ", new object[] {  vendor_id } );
		// Level 1 - Check for syntaxual errors
		foreach(var r in rm)
			{
			if(r.vendor_code.Trim() == "")
				{
				returned							+= "- The vendor code on the "+add_ordinal_suffix(i)+" selected line is blank\n";
				}
			if(r.cost == 0)
				{
				returned							+= "- The cost on the "+add_ordinal_suffix(i)+" selected line is zero\n";
				}
			if(r.cost < 0)
				{
				returned							+= "- The cost on the "+add_ordinal_suffix(i)+" selected line is less than zero\n";
				}
			if(r.qty_per == 0)
				{
				returned							+= "- The qty per on the "+add_ordinal_suffix(i)+" selected line is zero\n";
				}
			if(r.qty_per < 0)
				{
				returned							+= "- The qty per on the "+add_ordinal_suffix(i)+" selected line is less than zero\n";
				}
			i++;
			}

		// Level 2 - Passed Level 1, now check for possible conflicts in existing data
		if(returned == "" && i > 0)
			{
			foreach(var r in rm)
				{
				// Check for existing parts with the same vendor code in the current branch / vendor
				var dr						= Toolbox.doSQL_dt(@"SELECT COUNT(*) c, IFNULL(GROUP_CONCAT(DISTINCT master_id), '') m FROM inventory_price WHERE master_id != @v0  AND vendor_code = @v1  AND vendor_id = @v2 ", new object[] {  r.master_id, r.vendor_code.Trim(), vendor_id } ).Rows[0];
				var existing_rows				= Convert.ToInt32(dr["c"]);
				var master_ids				= dr["m"].ToString();
				if(existing_rows > 0)
					{
					returned					+= "- The vendor part # for "+r.master_id+" on the "+add_ordinal_suffix(i)+" selected line already exists on id(s): "+master_ids+"\n";
					}
				}
			if(returned == "")
				{
				// Level 3 - Merge & Update
				var vpr		= new vendor_price_row();
				foreach(var r in rm)
					{
					var line	= new rfq_lineitem(rfq_id, r.master_id, vendor_id);
					vpr					= new vendor_price_row(r.master_id, business_unit_id, vendor_id, r.vendor_code);
					if(!vpr.exists)
						{
						vpr.master_id		= r.master_id;
						vpr.vendor_id		= vendor_id;//vendor_number_int;
						vpr.is_benchmark	= false;
						vpr.is_preferred	= false;
						vpr.business_unit_id		= business_unit_id;
						}
					vpr.vendor_code		= r.vendor_code;
					vpr.lead_time		= line.lead_time;
					vpr.member_id		= current_user.id;
					vpr.qty				= r.qty_per;
					vpr.cost			= r.cost / r.qty_per;
					vpr.total			= r.cost;
					vpr.origin			= "RFQ Merge - "+Toolbox.MySQLNow_short();
					vpr.save(true);

					line.qty			= r.qty_per;
					line.vendor_code	= r.vendor_code;
					line.vendor_price	= r.cost;
					line.save();
					line.set_merged();
					}
				returned = "SUCCESS";
				}
			}
		else if(i == 0)
			{
			returned = "No lines were sent...";
			}
		_e.Result	= returned;
		}
	protected class rfq_merge_item
		{
		public int master_id { get; set;}
		public string vendor_code { get; set; }
		public double cost {  get; set; }
		public double qty_per {get; set;}
		}
	protected void gv_minmax_FooterCellPrepared(object _sender, ASPxGridViewTableFooterCellEventArgs _e)
		{

		}
	protected void gv_minmax_HtmlDataCellPrepared(object _sender, ASPxGridViewTableDataCellEventArgs _e)
		{
		var gv			= (ASPxGridView) _sender;
		if(_e.DataColumn.Caption == "#")
			{
			_e.Cell.Visible = false;
			}
		if (_e.DataColumn.FieldName == "qty")
			{
			_e.Cell.Enabled	= is_purchaser && _bo_obj.stk_adj;
			}
		if(_e.DataColumn.FieldName == "min_qty" || _e.DataColumn.FieldName == "max_qty")
			{
			_e.Cell.Enabled	= is_purchaser;
			}
		var save_col		= gv.Columns["save"];
		if(save_col.Visible)
			{
			var b		= (ASPxButton) gv.FindFooterCellTemplateControl(gv.Columns["save"], "ASPxButton1");
			b.Visible			= is_purchaser;
			b.Enabled			= is_purchaser;
			}
		if (_e.DataColumn.Name == "save")
			{
			_e.Cell.Visible	= is_purchaser;
			}
		}
	protected void qty_to_print_ButtonClick(object _source, ButtonEditClickEventArgs _e)
	{
		var masterid = Convert.ToInt32(hidbcmastertid["id"]);
		var be = (ASPxButtonEdit)_source;
		var pu = (ASPxPopupControl)popbcprint;
		pu.ShowOnPageLoad = false;
		if (be.Text != null)
			{
			var inv = new inventory();
			inv.Load(masterid,workingBusinessUnit.id);
            inv.print_barcode_label(Convert.ToInt16(be.Text));
        }
	}

	protected void b_delete_Click(object _sender, EventArgs _e)
		{
		var b			= (ASPxButton) _sender;
		Toolbox.doSQL_void(@"DELETE FROM inventory_consignment WHERE id = @v0 LIMIT 1", new object[] { gv_consignment.GetDataRow(gv_consignment.EditingRowVisibleIndex)["id"]});
		gv_consignment.CancelEdit();
		gv_consignment.DataBind();
		}
	protected void pop_files_ClientLayout(object _sender, ASPxClientLayoutArgs _e)
	{
		if (Session["working_master_id"] != null)
		{
		}

	}
	protected void gv_minmax_HtmlCommandCellPrepared(object _sender, ASPxGridViewTableCommandCellEventArgs _e)
		{}
	protected void gv_minmax_CommandButtonInitialize(object _sender, ASPxGridViewCommandButtonEventArgs _e)
		{
		var grid		= (ASPxGridView) _sender;
		var master_id			= Convert.ToInt32(grid.GetRowValues(_e.VisibleIndex, "master_id"));
		var qty				= Convert.ToDouble(grid.GetRowValues(_e.VisibleIndex, "qty"));
		if(_e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
			{
			_e.Visible			= is_purchaser;
			}
		}
} // Inventory_start

public partial class functions
		{
		#region variable declaration
		string _sql, _this_string							= "";
		int _this_int									= 0;
		HttpRequest _req								= HttpContext.Current.Request;
		NameValueCollection _form						= HttpContext.Current.Request.Form;
		HttpResponse _resp								= HttpContext.Current.Response;
	
		private NeMember _member;
		public NeBusinessUnit WorkingBusinessUnit {get; set;}
		public NeBusinessUnit WarehouseBusinessUnit {get; set;}
		Toolbox _tools;
		private inventory _inventory;
		public functions(NeMember _user)
			{
			_tools										= new Toolbox();
			_member										= _user;
			_tools.current_user							= _user;
			_tools.page_author							= new NeMember(711);
			_inventory									= new inventory();
			}
		#endregion variable declaration
		#region string
		public string get_DSNs()
			{
			return Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(company_dsnbv7) from business_unit  WHERE has_inventory = 1");
			}
		public string get_parts_tag(object _master_id)
			{
			return Toolbox.doSQL_string(@"SELECT tag_id FROM inventory_item_master WHERE master_id =@v0 ", new object[] { _master_id});
			}	
		public string find_tagpattern(string _pattern, string _tag_id)
			{
			var dr	 = Toolbox.doSQL_dt(@"CALL match_parts(@v2 , @v0 , @v1 ,0, false)", new object[] {  _pattern, _member.business_unit.id, _tag_id } ).Rows[0];
			return dr["item"].ToString();
			}
		public string insert_master_part(string _tag_id)
			{
			try
				{
				_this_string		= _tools.returnSQL_id(@"INSERT INTO inventory_item_master (tag_id, created_by, create_date) VALUES (@v0, @v1, now())",
					new object[] { _tag_id, _member.id});
				}
			catch (Exception ee)
				{
				
				_this_string		= "0";
				}
			return _this_string;
			}
		public string part_description(string _master_id)
			{
			var tag_id			= Toolbox.doSQL_string(@"SELECT tag_id FROM inventory_item_master WHERE master_id =@v0 ",_master_id);
			var tag_name		= Toolbox.doSQL_string(@"SELECT tag FROM inventory_tag WHERE tag_id = @v0",tag_id);
			var description		= Toolbox.doSQL_string(@"SELECT full_part_description(@v0, true, @v1)", new object[] { _master_id, WarehouseBusinessUnit.country});
			var button_not_qced	= string.Format(@"<img src='/images/inventory/decal/decal[notqced].png' title=""Part has not been qc'ed, please contact the inventory administrator ({0})."" align='absmiddle' style='float:left;'/>", shared.get_nesi_member_based_on_function("INVENTORY_ADMINISTRATOR").FullName + " at " + shared.get_nesi_member_based_on_function("INVENTORY_ADMINISTRATOR").NEEmail);
			var button_is_qced	= string.Format(@"<img src='/images/inventory/decal/decal[qced].png' title=""Part has been qc'ed"" align='absmiddle' style='float:left;'/>", "");
			var button_print		= string.Format(@"<button type='button' style='width:59px;height:48px;float:left;margin-right:2px;background-color:#4682B4;border:solid 1px #fff;border-radius:5px;font-size:8px;color:#fff;font-weight:bold;' onclick='bc({0})' title=""Print this part number's barcode""><img src='/images/icon/icon[print_barcode].gif' /><br/>BARCODE</button>", _master_id);
			var button_replicate	= string.Format(@"<button type='button' style='width:59px;height:48px;float:left;margin-right:10px;background-color:#4682B4;border:solid 1px #fff;border-radius:5px;font-size:8px;color:#fff;font-weight:bold;' onclick=""location.href='./index.aspx?a=start_part&tag_id={1}&master_id={0}'""><img src='/images/icon/icon[replicate].gif' /><br/>REPLICATE</button>", _master_id, tag_id);
			var used_description	= string.Format(@"<b style='font-size:1.2em;'>{0}</b><div style='font-weight:bold;font-size:.65em;'>{1}</div>", _master_id, description);
			var returned			= is_q_ced(_master_id, "part") 
										? string.Format(@"{0}{1}{2}{3}", button_is_qced, button_print, button_replicate, used_description)
										: string.Format(@"{0}{1}{2}{3}", button_not_qced, button_print, button_replicate, used_description);
			return returned;
			}	
		public string is_reference_unique(string _code)
			{
			string temp;
			try
				{
				temp		= Toolbox.doSQL_string(@"SELECT CAST(IFNULL(MAX(master_id), '') AS CHAR) master_id FROM inventory_price WHERE TRIM(vendor_code) = @v0 limit 1", _code);
				}
			catch
				{
				temp		= "";
				}
			return temp;
			}
		public string get_TagIDfromMaster(object _master_id)
			{
			return Toolbox.doSQL_string(@"SELECT tag_id FROM inventory_item_master WHERE master_id = @v0", new object[] { _master_id});
			}
		public string last_sold_date(string _part_number, string _dsn, string _table)
			{
			var _string	= "";
			//using(var bv_conn = BVDB.connect(_dsn))
			//	{
			//	var column			= "";
			//	switch(_table)
			//		{
			//		case "SALES_ORDER_DETAIL":		column	= "BVRVADDDATE";
			//		break;
			//		case "SALES_HISTORY_DETAIL":	column	= "INVOICE_DATE";
			//		break;
			//		}
			//	_string = BVDB.getSQL_string(bv_conn,string.Format(@"SELECT MAX({0}) FROM {1}  WHERE CODE = ? ", _table, column ), new object[] {  _part_number} );
			//	}
			return _string;
			}
		public string last_bought_date(string _part_number, string _dsn, string _table)
			{
			var _string	= "";
			//using(var bv_conn = BVDB.connect(_dsn))
			//	{
			//	_string = BVDB.getSQL_string(bv_conn,String.Format(@"SELECT MAX(LAST_RCVE_DATE) FROM {0}  WHERE CODE = ? ", _table), new object[] {  _part_number  } );
			//	}
			return _string;
			}
		public string report_writer(DataTable _dt)
			{
			var output				= "";
			if(_dt.Rows.Count > 0)
				{
				var gv					= new GridView();
				gv.DataSource				= _dt;
				gv.DataBind();
				gv.UseAccessibleHeader		= true;
				gv.HeaderRow.TableSection	= TableRowSection.TableHeader;
				gv.FooterRow.TableSection	= TableRowSection.TableFooter;
				gv.Width					= Unit.Percentage(100);
				gv.CssClass					= "tablesorter";
				gv.Style.Add("font-family", "arial");
				gv.Style.Add("font-size", "11px");
				gv.Style.Add("text-align", "center");
				gv.Style.Add("display", "none");
				for(var r = 0; r < gv.Rows.Count; r++)
					{
					for(var c = 0; c < gv.Rows[r].Cells.Count; c++)
						{
						var cell			= gv.Rows[r].Cells[c];
						cell.Width				= Unit.Percentage(100/gv.Rows[r].Cells.Count);
						cell.Style.Add("border-right", "solid 1px #ddd");
						cell.Style.Add("border-bottom", "solid 1px #ddd");
						}
					}
				gv.BorderWidth				= 0;
				gv.GridLines				= 0;
				var sw				= new StringWriter();
				var hw			= new HtmlTextWriter(sw);
				gv.RenderControl(hw);
				var pager				= "";
				if(gv.Rows.Count > 10)
					{
					pager					= "$('.tablesorter').tablesorter().tablesorterPager({positionFixed:false, size:10,container: $('#pages_')}).show();$('#pages_').slideDown();";
					}
				else
					{
					pager					= "$('.tablesorter').tablesorter().show();";
					}
				output						+= sw.ToString();
				output						+= string.Format(@"
				<div id='pages_' align='center' style='display:none;'>
					<form>
						<button type='button' class='first'>&lt;&lt;</button>
						<button type='button' class='prev'>&lt;</button>
						<input type='text' size='5' class='pagedisplay'/>
						<button type='button' class='next'>&gt;</button>
						<button type='button' class='last'>&gt;&gt;</button>
						<select class='pagesize'>
							<option selected='selected'  value='10'>10</option>
							<option value='20'>20</option>
							<option value='30'>30</option>
							<option  value='40'>40</option>
						</select>
					</form>
				</div>
				<script>
					$('body').ready(function()
										{{
										{0}
										}});
				</script>", pager);
				}
			else
				{
				output			= "No available data";
				}
			return output;
			}	
		#endregion string
		#region void
		public void get_sold_as(NeMember _user)
			{
			_resp.Clear();
			var temp_company	= new NeBusinessUnit(_user.business_unit_id);
			var q	= _req.QueryString;
			var tag_id			= q["tag_id"];
			var sold_as			= "";
			if(temp_company.country == "CDN")
				{
				sold_as				= Toolbox.doSQL_string(@"SELECT canadian_sold_as FROM inventory_tag WHERE tag_id =@v0 ",tag_id);
				}
			else
				{
				sold_as				= Toolbox.doSQL_string(@"SELECT usa_sold_as FROM inventory_tag WHERE tag_id =@v0 ",tag_id);
				}
			_resp.Write(sold_as);
			_resp.End();			
			}
		public void save_price()
			{
			_resp.Clear();
			
			Toolbox.do_set_plain_header(_resp);
			var q	= _req.QueryString;
			var price_id			= 0;
			int.TryParse(q["price_id"], out price_id);
			var master_id		= q["master_id"];
			var vendor_id		= q["vendor_id"];
			var part_number		= Toolbox.do_value_from(q["part_number"], false);
			var qty				= Convert.ToDouble(q["qty"]);
			var unit_cost		= Convert.ToDouble(q["costprice"]);
			var cost_total		= Convert.ToDouble(q["costtotal"]);
			var benchmark			= Convert.ToBoolean(q["benchmark"]);
			var is_preferred		= Convert.ToBoolean(q["is_preferred"]);
			var lead_time			= q["lead_time"] != null ? Convert.ToInt32(q["lead_time"]) : 0;
			try
				{
				var vpr					= price_id > 0 ? new vendor_price_row(price_id) : new vendor_price_row();
				vpr.cost				= unit_cost;
				vpr.total				= cost_total;
				vpr.master_id			= master_id;
				vpr.vendor_id			= vendor_id;
				if(lead_time != 0)
					{
					vpr.lead_time		= lead_time;
					}
				vpr.is_benchmark		= benchmark;
				vpr.is_preferred		= is_preferred;
				vpr.vendor_code			= part_number;
				vpr.qty					= qty;
				vpr.member_id			= _member.id;
				if(price_id == 0)
					{
					vpr.business_unit_id			= WarehouseBusinessUnit.id;
					}
				vpr.save();
				Toolbox.doSQL_void(@"UPDATE inventory_cost SET cost=GET_CURRENT_COST(master_id, business_unit_id) WHERE business_unit_id = @v1 and master_id = @v0", new object[] { master_id, WarehouseBusinessUnit.id});
				_resp.Write("Success");
				}
			catch(Exception ee)
				{
				_resp.Write(ee.Message);
				_resp.End();
				}
			_resp.End();
			}
		public void copy_price()
			{
			_resp.Clear();
			
			Toolbox.do_set_plain_header(_resp);
			var q	= _req.QueryString;
			var price_id			= "";
			object master_id		= "";
			var fromBusinessUnitID	= 0;
			object unit_cost		= "0";
			object qty				= "1";
			object part_number		= "";
			object vendor_id		= "";
			object unit_total		= "";
			#region grab line info
			try
				{
				price_id				= q["price_id"];
				var dt			= Toolbox.doSQL_dt(@"SELECT * FROM inventory_price WHERE id = @v0  LIMIT 1", new object[] {  price_id } );
				foreach(DataRow dr in dt.Rows)
					{
					master_id				= dr["master_id"];
					unit_cost				= dr["cost"];
					fromBusinessUnitID		= (int) dr["business_unit_id"];
					var from_company		= new NeBusinessUnit(fromBusinessUnitID);
					var _inventory	= new inventory();
					_inventory.Load(master_id, fromBusinessUnitID);
					if(_inventory.sold_as_canadian == "2" && _inventory.sold_as_usa == "1" && WarehouseBusinessUnit.country == "USA" && from_company.country == "CDN")
						{
						unit_cost		= Convert.ToDouble(unit_cost) / 3.2808399;
						qty				= (double) 1;
						}
					else if(_inventory.sold_as_canadian == "2" && _inventory.sold_as_usa == "1" && WarehouseBusinessUnit.country == "CDN" && from_company.country == "USA")
						{
						unit_cost		= Convert.ToDouble(unit_cost) * 3.2808399;
						qty				= (double) 1;
						}
					else
						{
						qty				= Convert.ToDouble(dr["qty"]);
						}
					unit_total			= unit_cost;
					part_number			= dr["vendor_code"];
					vendor_id			= dr["vendor_id"];
					}
				}
			catch (Exception ee)
				{
				_resp.Write(ee.ToString());
				_resp.End();
				}
			#endregion grab line info
			#region insert new line
			try 
				{
				var vpr	= new vendor_price_row();
				vpr.cost				= unit_cost;
				vpr.total				= unit_total;
				vpr.master_id			= master_id;
				vpr.vendor_id			= vendor_id;
				vpr.vendor_code			= part_number;
				vpr.qty					= qty;
				vpr.member_id			= _member.id;
				vpr.business_unit_id			= WorkingBusinessUnit.id;
				vpr.origin				= "Copied From ID:"+price_id;
				vpr.save();
				Toolbox.doSQL_void(@"UPDATE inventory_cost SET cost=GET_CURRENT_COST(master_id, business_unit_id)
WHERE business_unit_id = @v1 and master_id = @v0", new object[] { master_id, WorkingBusinessUnit.id});
				}
			catch (Exception ee)
				{
				_resp.Write(ee.ToString());
				_resp.End();
				}
			#endregion insert new line
			_resp.Write("SUCCESS");
			_resp.End();
			}	
		public void delete_price()
			{
			_resp.Clear();
			
			Toolbox.do_set_plain_header(_resp);
			var temp_company		= new NeBusinessUnit(WorkingBusinessUnit.id);
			var q	= _req.QueryString;
			string master_id;
			if(q["price_id"] != null)
				{
				try
					{
					var i						= Toolbox.doSQL_dt(@"SELECT master_id, vendor_id, benchmark FROM inventory_price WHERE id = @v0  LIMIT 1", new object[] {  q["price_id"] } ).Rows[0];
					master_id					= i["master_id"].ToString();
					var vid						= i["vendor_id"].ToString();
					var benchmark					= Convert.ToBoolean(i["benchmark"]);
					if(benchmark)
						{
						// Need to find the most recently purchased price
						// Get latest vendor number for part outside of deleting row. it will return 0 if there isn't one.
						var exists			= false;
						var vendor_id		= Toolbox.doSQL_int(@"
SELECT 
	IFNULL(MIN(c.vendor_id), 0) 
FROM 
	po_details_current a 
LEFT JOIN 
	poprog_header b 
		ON a.po_details_poprog_id = b.poprog_id 
LEFT JOIN 
	vendor c 
		ON b.poprog_vendor_id = c.vendor_id 
WHERE 
	a.po_details_part_no = @v0 AND 
	b.business_unit_id = @v1 AND 
	c.vendor_id != @v2
ORDER BY 
	a.po_details_date_modified 
LIMIT 1", new object[] { master_id, WorkingBusinessUnit.id, vid});
						if(vendor_id != 0)
							{
							// Set bench to latest (check to see if it exists though)
							exists		= Toolbox.doSQL_int(@"
SELECT 
	COUNT(*) 
FROM 
	inventory_price 
WHERE 
	master_id = @v0 AND 
	business_unit_id = @v1 AND 
	vendor_id = @v2", new object[] {  master_id, 
							WorkingBusinessUnit.id, 
							vendor_id}) > 0;
							}
						var temp_id	= 0;
						if(exists)
							{
							temp_id	= Toolbox.doSQL_int(@"
SELECT
	a.id
FROM
	inventory_price a
LEFT JOIN
	vendor b ON a.vendor_id = b.vendor_id
LEFT JOIN
	po_details_current c
		ON a.master_id = c.po_details_part_no AND
		c.po_details_poprog_id IN (SELECT poprog_id FROM poprog_header WHERE business_unit_id = a.business_unit_id)
LEFT JOIN
	poprog_header d
		ON c.po_details_poprog_id = d.poprog_id AND
		d.poprog_vendor_id = b.vendor_id
WHERE 
	a.master_id = @v0 AND 
	a.business_unit_id = @v1 AND 
	a.vendor_id = @v2
ORDER BY 
	edited_dt DESC
LIMIT 1", new object[] {      master_id, 
							WorkingBusinessUnit.id, 
							vendor_id});
							}
						else if(vendor_id != 0) 
							{
							// Set benchmark to highest	cost, get that id from inventory_price						
							temp_id	= Toolbox.doSQL_int(@"
SELECT
	id
FROM
	inventory_price 
WHERE 
	master_id = @v0 AND 
	business_unit_id = @v1
ORDER BY 
	cost DESC
LIMIT 1", new object[] {      master_id, 
							WorkingBusinessUnit.id, 
							vendor_id});
							}
						// Save price if there was an alternate to jump to.
						if(vendor_id != 0)
							{ 
							var vpr			= new vendor_price_row(temp_id);
							vpr.is_benchmark				= true;
							vpr.origin						= "Made benchmark price from vendor line deletion";
							vpr.save(true);
							}
						}
					if(master_id != "" && vid != "")
						{
						Toolbox.doSQL_void(@"DELETE FROM inventory_price WHERE id = @v0 LIMIT 1", new object[] { q["price_id"]});
						//Toolbox.doSQL_void(@"DELETE FROM SPECIAL_PRICING WHERE bvspecpricepartno = @v0  and bvspecpricecode = @v1 ", temp_company , new object[] {  master_id, vendor_number } );
						_resp.Write("SUCCESS");
						}
					else
						{
						_resp.Write("FAILED");
						}
					}
				catch (Exception ee)
					{
					throw(ee);
					}
				Toolbox.doSQL_void(@"UPDATE inventory_cost 
SET cost=GET_CURRENT_COST(master_id, business_unit_id) WHERE business_unit_id = @v1 and master_id = @v0", new object[] { master_id, WorkingBusinessUnit.id});
				}
			else
				{
				_resp.Write("FAILED");
				}
			_resp.End();
			}	
		public void picture_save()
			{
			var master_id			= _form["master_id"];
			var file		= _req.Files.Count == 1 ? _req.Files[0] : null;
			var pasted				= _form["inventory_file_blob"];
			byte[] buffer;
			string filename, ext		= "";
			if(file != null)
				{
				filename					= Path.GetFileName(file.FileName);
				ext						= Path.GetExtension(filename);
				var stream		= new BufferedStream(file.InputStream);
				buffer						= new byte[file.ContentLength];
				stream.Read(buffer,0, buffer.Length);
				}
			else
				{
				filename				= master_id;
				ext					= ".jpg";
				buffer					= Convert.FromBase64String(pasted.Split(',')[1]);
				}
			var image				= Convert.ToBase64String(buffer);
			var picture_id			= _tools.returnSQL_id(@"INSERT INTO inventory_picture (master_id, insert_dt, picture, name, ext, active, added_by) VALUES (@v0, now(), COMPRESS(@v1), @v2, @v3, true, @v4)", new object[] { master_id, image, filename, ext, _member.id});
			add_history(master_id, string.Format("Picture ID#{0} ({2}) added by {1}.", picture_id, _member.FullName, filename), _member.id, "inventory_picture", picture_id);
			_resp.Clear();
			if(_form["close"] == "true")
				{
				_resp.Write(string.Format("<div style='padding:10px;font-weight:bold;font-size:13px;font-family:arial;color:#060;'>Successfully saved image to part # {0}.<br/><i style='font-size:10px;'>Note: The submitted image won't show up on the part until it has been QC'ed.</i></div><div align='center'><button type='button' onclick='parent.reset_picture_box()'>Press to close</button></div>", master_id));
				}
			else
				{
				_resp.Write("Successfully added picture to part <a target='_parent' href='./index.aspx?a=get&tab=G&id="+master_id+"'>"+master_id+"</a>");
				}
			_resp.End();
			}
		public void value_add()
			{
			var q	= _req.QueryString;
			if(q["att"] != null && q["val"] != null && q["tag_id"] != null)
				{
				var att				= Convert.ToInt32(q["att"]);
				var val			= Toolbox.remove_duplicate_spaces(q["val"].Trim());
				var tag_id		= q["tag_id"];
				// Run check on value first
				var first_check				= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_attribute_value WHERE attribute_id = @v0 AND value = @v1", new object[] { att, val});
				var second_check			= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_attribute_value WHERE attribute_id = @v0 AND value = @v1", new object[] { att, val.Replace(" ", "").Replace("-", "")});
				if(first_check == 0 && second_check == 0)
					{
					var auto_qc				= att == 17 ? "1" : WorkingBusinessUnit.id.ToString();
					var auto_approve		= att == 17 ? "1" : "0";
					var auto_approve_by		= att == 17 ? "1" : "NULL";
					var auto_approve_dt		= att == 17 ? "NOW()" : "NULL";
					var new_val_id			= _tools.returnSQL_id(@"
INSERT INTO inventory_attribute_value 
	(
	attribute_id, 
	value, 
	create_date, 
	created_by, 
	qced,
	approved, 
	approved_by, 
	approved_date 
	) 
VALUES 
	(
	@v0, 
	@v1, 
	NOW(), 
	@v2, 
	@v3,
	@v4,
	@v5,
	"+ auto_approve_dt + @"
	)", new object[] {
	att, 					 // {0}
	val, // {1}
	_member.id,				 // {2}
	auto_qc,				 // {3}
	auto_approve,			 // {4}
	auto_approve_by		 // {5}
	// auto_approve_dt			 // {6}
	});
					try
						{
						Toolbox.doSQL_void(@"INSERT INTO inventory_tag_preset (tag_id, attribute_id, value_id, qced) VALUES (@v0, @v1, @v2, @v3);", new object[] { tag_id, att, new_val_id, WorkingBusinessUnit.id});
						Toolbox.QuickReponse(_resp, new_val_id);
						}
					catch(ThreadAbortException ee)
						{
						// Need something for managing the quick response Thread abort.
						}
					catch(Exception ee)
						{
						Toolbox.doSQL_void(@"DELETE FROM inventory_attribute_value WHERE attribute_value_id =@v0 ",new_val_id);
						Toolbox.QuickReponse(_resp, "Rollback");
						}
					}
				else
					{
					if(att == 17)
						{
						var warning		= new NeEMail();
						warning.From		= "noreply@" + Toolbox.app_setting("DomainForEmail");
						warning.Subject		= "Duplicate manufacturer part #";
						warning.To			= "mkettenbach@" + Toolbox.app_setting("DomainForEmail");
						warning.isHTML		= true;
						warning.Body		= string.Format("<b>{0}</b> from <b>{1}</b> just tried creating a manufacturer part # of <b>{2}</b>... one does already exists either directly, without spaces or without dashes.", _member.FullName, _member.business_unit.name, val);
						warning.Send();
						}
					Toolbox.QuickReponse(_resp, "Exists");
					}
				}
			else
				{
				Toolbox.QuickReponse(_resp, "Failed");
				}
			}
		public void add_history(object _master_id, object _what_happened, object _member_id, object _table, object _table_id)
			{
			Toolbox.doSQL_void(@"INSERT INTO inventory_history (master_id, member_id, datetime, event, origin_table, origin_id)
VALUES (@v0, @v2, now(), @v1, @v3, @v4)", new object[] { _master_id, _what_happened, _member_id, _table, _table_id});
			}
		public void price_info(bool _historic)
			{
			_resp.Clear();
			DataTable dt;
			var q				= _req.QueryString;
			var table						= "";
			var order						= "";
			var dtcolumn					= "";
			if(!_historic)
				{
				table							= "inventory_price";
				order							= "base_cost ASC";
				dtcolumn						= "a.edited_dt";
				}
			else
				{
				table							= "inventory_price_history";
				order							= "a.insert_dt DESC";
				dtcolumn						= "a.insert_dt";
				}
			if(q["master_id"] != null && q["use"] != null)
				{
				var use						= Convert.ToBoolean(Convert.ToInt32(q["use"]));
				var inc						= use ? "=" : "!=";;
				var master_id				= q["master_id"];
				dt								= Toolbox.doSQL_dt(@"CALL INVENTORY_VENDOR_PRICES(@v0, @v1, @v2, @v3)", new object[] {_historic, use, WarehouseBusinessUnit.id, master_id});
				dt.TableName					= "price";
				dt								= _tools.HTMLize_datatable(dt);
				dt.WriteXml(_resp.OutputStream);
				}
			_resp.End();
			}
		#endregion void
		#region boolean
		//public bool part_exists_in_bv(string _part_number, string _dsn)
		//	{
		//	using(var bv_conn = BVDB.connect(_dsn))
		//		{
		//		_this_int				= BVDB.getSQL_int(bv_conn,@"SELECT COUNT(*) FROM INVENTORY WHERE CODE = ? ", new object[] {  _part_number } );
		//		}
		//	return _this_int > 0;
		//	}
		public bool exists_in_purchase_order_detail(string _part_number, string _dsn)
			{
			//using(var bv_conn = BVDB.connect(_dsn))
			//	{
			//	_this_int				= BVDB.getSQL_int(bv_conn,@"SELECT COUNT(*) FROM PURCHASE_ORDER_DTAIL WHERE CODE = ? ", new object[] {  _part_number } );
			//	}
			return false;
			}

	public bool exists_in_sales_order_detail(string _part_number, string _dsn)
		{
		_this_int					= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM SALES_ORDER_DETAIL WHERE CODE =?", _part_number);
		return _this_int > 0;
		}

	public bool is_item_unique(string _tag, string _tagpattern, string _master_id)
			{
			var dt	 = Toolbox.doSQL_dt(@"CALL match_parts(@v2, @v0 , @v1 ,0, false)", new object[] {  _tagpattern, _member.business_unit.id, _tag } );
			if(dt.Rows.Count == 0)
				{
				return true;
				}
			else if(dt.Rows.Count > 0)
				{
				var isunique = false;
				foreach(DataRow dr in dt.Rows)
					{
					if(dr["item"].ToString() == _master_id)
						{
						isunique = true;
						}
					}
				return isunique;
				}
			else
				{
				return false;
				}
			}
		public bool is_item_unique(string _tag, string _tagpattern)
			{
			var dt	 = Toolbox.doSQL_dt(@"CALL match_parts(@v2 , @v0 , @v1 , 0, false)", new object[] {  _tagpattern, _member.business_unit.id, _tag } );
			if(dt.Rows.Count == 0)
				{
				return true;
				}
			else
				{
				return false;
				}
			}
		public bool is_qty(object _tag_id)
			{
			return Convert.ToBoolean(Toolbox.doSQL_int(@"SELECT is_qty FROM inventory_tag WHERE tag_id =@v0 ", new object[] { _tag_id}));
			}
		public bool is_q_ced(object _id, string _type)
			{
			int qced;
			switch(_type)
				{
				case "part":
					qced			= Toolbox.doSQL_int(@"SELECT IFNULL(approved, 0) FROM inventory_item_master WHERE master_id =@v0 ", new object[] { _id});
				break;
				case "tag":
					qced			= Toolbox.doSQL_int(@"SELECT IFNULL(approved, 0) FROM inventory_tag WHERE tag_id=@v0 ", new object[] { _id });
				break;
				case "value":
					qced			= Toolbox.doSQL_int(@"SELECT IFNULL(approved, 0) FROM inventory_attribute_value WHERE attribute_value_id = @v0 AND approved = true", new object[] { _id });
				break;
				default:
					qced			= 0;
				break;
				}
			return Convert.ToBoolean(qced);
			}
		#endregion boolean
		#region integer
		public int business_unit_id_from_dsn(object _dsn)
			{
			return Toolbox.doSQL_int(@"SELECT id business_unit_id from business_unit  WHERE Company_DSNBV7 = @v0", new object[] { _dsn});
			}
		#endregion integer
		#region DataTable
		public DataTable Browse_Attributes(string _tag_id)
			{
			if(_tag_id == "")
				{
				_tag_id				= "true";
				}
			_sql						= string.Format(@"
SELECT 
	d.attribute,
	d.attribute_id
FROM 
	inventory_item_detail a 
LEFT JOIN 
	inventory_attribute_value b
		ON a.attribute_value_id = b.attribute_value_id 
LEFT JOIN
	inventory_item_master c
		ON a.master_id = c.master_id
LEFT JOIN
	inventory_attribute d
		on b.attribute_id = d.attribute_id
WHERE c.tag_id = @v0 AND b.active = true
GROUP BY d.attribute_id, d.attribute
ORDER BY d.attribute_id");
			return Toolbox.doSQL_dt(_sql,new object[] { _tag_id});
			}
		public DataTable Browse_Values(string _tag_id, string _att_id)
			{
			_sql						= string.Format(@"
SELECT 
	b.value,
	b.attribute_value_id value_id
FROM 
	inventory_item_detail a 
LEFT JOIN 
	inventory_attribute_value b
		ON a.attribute_value_id = b.attribute_value_id 
LEFT JOIN
	inventory_item_master c
		ON a.master_id = c.master_id
LEFT JOIN
	inventory_attribute d
		ON b.attribute_id = d.attribute_id
WHERE c.tag_id = @v0 and d.attribute_id = @v1 AND b.active = true
GROUP BY value_id" );
			return Toolbox.doSQL_dt(_sql, new object[] { _tag_id, _att_id});
			}
		public DataTable Query_Vendors(string _query, string _type_of)
			{
			if(_type_of == "id")
				{
				_sql					= string.Format(@"SELECT vendor_number, vendor_name FROM vendor WHERE vendor_number like ""%{0}%"" AND vendor_active = 1 ORDER BY CAST(vendor_number AS DEC(10))", _query);
				}
			else
				{
				_sql					= @"SELECT vendor_number, vendor_name FROM vendor WHERE vendor_name like ""%"+_query+@"%"" AND vendor_active = 1 ORDER BY CAST(vendor_number AS DEC(10))";
				}
			return Toolbox.doSQL_dt(_sql,null);
			}
		public DataTable attributes()
			{
			return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute ORDER BY attribute"  , null);
			}
		public DataTable attributes(string _tag_id)
			{
			return Toolbox.doSQL_dt(@" SELECT a.attribute_id, b.attribute FROM inventory_tag_link a LEFT JOIN inventory_attribute b ON a.attribute_id = b.attribute_id  WHERE tag_id =@v0 ORDER BY a.order_id, b.attribute", new object[] { _tag_id });
			}
		public DataTable approved_tags()
			{
			return Toolbox.doSQL_dt("SELECT * FROM inventory_tag WHERE approved = 1 ORDER BY tag",null);
			}
		public DataTable Match_Parts(string _tag_id, string _attval_pattern)
			{
			_attval_pattern		= _attval_pattern.TrimEnd(',');
			_attval_pattern		= _attval_pattern.TrimStart(',');
			_sql					= string.Format("CALL match_parts({0}, '{1}', {2}, 0, false);", _tag_id, _attval_pattern, _member.business_unit.id);
			return Toolbox.doSQL_dt(@"CALL match_parts(@v0 , @v1 , @v2 , 0, false)", new object[] {  _tag_id, _attval_pattern, _member.business_unit.id} );
			}
		public DataTable tags(string _seed, string _searchterm, string _per_page)
			{
			_searchterm			= _searchterm.Replace("\"", "&#34;").Replace("'", "&#39;");
			switch (_searchterm)
				{
				case "APPROVED":
				case "approved": _sql = string.Format("SELECT * FROM inventory_tag WHERE approved = 1 ORDER BY tag LIMIT {0},{1}", _seed, _per_page);
				break;
				case "UNAPPROVED":
				case "unapproved":	_sql				= "SELECT * FROM inventory_tag WHERE approved = 0 ORDER BY tag LIMIT "+_seed+","+_per_page;
				break;
				default:			_sql				= "SELECT * FROM inventory_tag WHERE tag LIKE '%"+_searchterm+"%' OR tag_id LIKE \"%"+_searchterm+"%\" ORDER BY tag LIMIT "+_seed+","+_per_page;
				break;
				}
			return Toolbox.doSQL_dt(_sql  , null);
			}
		public DataTable tags(int _exclude)
			{
			return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_id = @v0  and approved = true ORDER BY value", new object[] {  _exclude } );
			}
		public DataTable tag_attribute_selected_values(string _tag_id, string _attribute_id)
			{
			if(_attribute_id == "16")// || attribute_id == "17")
				{
				return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_id = @v0  AND approved = true ORDER BY value", new object[] {  _attribute_id, WorkingBusinessUnit.id } );
				}
			else if(_attribute_id == "17")
				{
				return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_value_id IN (SELECT value_id FROM inventory_tag_preset WHERE tag_id = @v0  AND attribute_id = @v1 ) AND attribute_value_id NOT IN (SELECT attribute_value_id FROM inventory_item_detail) AND approved = true", new object[] {  _tag_id, _attribute_id } );
				}
			else
				{
				return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_value_id in (SELECT value_id FROM inventory_tag_preset WHERE tag_id = @v0  AND attribute_id = @v1  AND (qced is NULL OR qced = @v2  OR approved = 1)) AND approved = true ORDER BY value", new object[] {  _tag_id, _attribute_id, WorkingBusinessUnit.id } );
				}
			}
		public DataTable values(string _attribute_id)
			{
			return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_id = @v0  AND approved = true ORDER BY value", new object[] {  _attribute_id } );
			}
		public DataTable available_values(string _tag_id, string _attribute_id)
			{
			return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_id = @v1  AND approved = true AND attribute_value_id NOT IN (SELECT value_id FROM inventory_tag_preset WHERE tag_id = @v0  AND attribute_id = @v1 )", new object[] {  _tag_id, _attribute_id } );
			}
		public DataTable selected_values(string _tag_id, string _attribute_id)
			{
				return Toolbox.doSQL_dt(@"SELECT * FROM inventory_attribute_value WHERE attribute_value_id IN (SELECT value_id FROM inventory_tag_preset WHERE tag_id = @v0  AND attribute_id = @v1 ) AND approved = true", new object[] {  _tag_id, _attribute_id } );
			}
		public DataTable ds_ns()
			{
			return Toolbox.doSQL_dt(@"SELECT COMPANY_DSNBV7 DSN from business_unit  WHERE HAS_INVENTORY = 1" , null);
			}
			//public Hashtable header_info(string _part_number, string _dsn)
		//	{
		//	var dt			= Toolbox.doSQL_dt(@"SELECT TOP 1 MAX(SELLING_PRICE) PRICE, VENDOR_CODE FROM SPECIAL_PRICING WHERE BVSPECPRICEPARTNO = ?  GROUP BY VENDOR_CODE ORDER BY PRICE DESC", _dsn , new object[] {  _part_number } );
		//	var header		= new Hashtable();
		//	foreach(DataRow dr in dt.Rows)
		//		{
		//		header["COSTPRICE"]		= dr["PRICE"].ToString();
		//		header["VENDORCODE"]	= dr["VENDOR_CODE"].ToString();
		//		}
		//	return header;
		//	}
		//public DataTable part_lookup(string _q, string _dsn)
		//	{
		//	return Toolbox.doSQL_dt(@" SELECT RTRIM(CODE) CODE, RTRIM(INV_DESCRIPTION) DESCRIPTION FROM INVENTORY WHERE CODE LIKE ? OR INV_DESCRIPTION LIKE ?", _dsn , new object[] { "%"+ _q+"%", "%" + _q + "%" } );
		//	}
		public DataTable Part_Att_Vals(string _master_id)
			{
			return Toolbox.doSQL_dt(@"SELECT * FROM inventory  WHERE master_id =@v0", new object[] { _master_id });
			}
		#endregion DataTable
		}
