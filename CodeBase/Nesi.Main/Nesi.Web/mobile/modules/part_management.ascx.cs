using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Text;
using NESI.Common.Models;
using nesi.core;

public partial class mobile_modules_part_management : System.Web.UI.UserControl
{
	protected NeMember current_user;
	bool is_admin = false;
	bool is_purchaser = false;
	bool can_commit = false;
	Toolbox _tools;
	private NeBusinessUnit WorkingBusinessUnit {get; set;}
	private NeBusinessUnit WarehouseBusinessUnit {get; set;}
	protected prepped_vars10 global_pv = new prepped_vars10();
	branch_options bo_obj;
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		_tools.dont_cache_page();
		try
		{
			current_user = Toolbox.do_handle_authentication(185);
		}
		catch
		{
			Response.Redirect("/mobile/index.aspx?a=logoff");
		}
		var is_limited_release		= shared.properties.exists("part_management_branches");
		if(is_limited_release)
			{
			var limited_release_deny_list	= new shared.properties("part_management_branches").value.Split(',').Select(int.Parse).ToList();
			if (!limited_release_deny_list.Contains(current_user.business_unit.id))
//	if (!(current_user.business_unit.country=="USA"))
				{
				Server.Transfer("/mobile/index.aspx?a=part_management2");
				}
			}
		else
			{
			Server.Transfer("/mobile/index.aspx?a=part_management2");
			}
		WorkingBusinessUnit = new NeBusinessUnit(current_user.business_unit_id);
		WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
		bo_obj = new branch_options(WarehouseBusinessUnit.id);
		is_admin = current_user.AuthenticatedForPage("49") && bo_obj.stk_adj || current_user.id == 8;
		is_purchaser = (current_user.AuthenticatedForPrivilege(71) || current_user.id == 8) && bo_obj.stk_adj;
		can_commit = current_user.AuthenticatedForPrivilege(190);
		//	lbl_user.Text					= "Logged in as: <b>"+current_user.FullName+"</b>";
		Session["memberid"] = current_user.id.ToString();
		//		lbbvwo.Visible					= mv.ActiveViewIndex != 2;
		//		label_bvwo.Visible				= mv.ActiveViewIndex != 2;
		#region If user has all location privilege or not
		populate_datasources();
		#endregion If user has all location privilege or not
		xfer_btn_Cmt.Visible = can_commit;


		//Thread.Sleep(1000);
		tb_annual_qty.Attributes["type"] = "number";
		fill_annual_history();
	}
	private void populate_datasources()
	{
		#region From location datasource
		if (Session["working_master_id"] != null && Session["working_master_id"].ToString() != "0")
		{
			ds_xfer_from_location.SelectCommand = @"
SELECT 0 id, '' name, 0 qty, 0 type_id, 0 max, 0 order_n 
UNION
SELECT
	b.id,
	CONCAT(IF(b.type_id = 1, 'INT - ', 'EXT - '), b.name,' (',IFNULL(a.qty,0),')')  name,
	IFNULL(a.qty,0) qty,
	b.type_id,
	IFNULL(a.max, 0) max,
	IF(a.qty > 0 AND b.type_id = 1, 1, IF(a.qty > 0 AND b.type_id = 2, 2, 3)) order_n
FROM
	inventory_location_master b
LEFT JOIN	
	inventory_location a  ON
		a.location_master_id = b.id AND a.master_id = @master_id
WHERE
	b.business_unit_ID = @business_unit_id
ORDER BY order_n ASC, qty DESC, type_id ASC, name ASC";
		}
		#endregion From location datasource
		if (!current_user.AuthenticatedForPrivilege(117))
		{
			#region To location datasource - Joining based on inventory_location.... only mapped locations
			ds_xfer_to_location.SelectCommand = @"
SELECT 0 id, '' name, 0 qty, 0 type_id, 0 max, 0 order_n
UNION 
SELECT
	a.id,
	CONCAT(IF(a.type_id = 1, 'INT - ', 'EXT - '), a.name,' (',IFNULL(b.qty,0),')')  name,
	IFNULL(b.qty, 0) qty,
	a.type_id,
	IFNULL(b.max, 0) max,
	IF(b.qty > 0 AND a.type_id = 1, 1, IF(b.qty > 0 AND a.type_id = 2, 2, 3)) order_n
FROM
	inventory_location b
LEFT JOIN 
	inventory_location_master a
		ON a.id = b.location_master_id AND b.master_id = @master_id AND b.business_unit_id = @business_unit_id
WHERE
	a.business_unit_id = @business_unit_id AND a.type_id = 1
UNION 
SELECT
	a.id,
	CONCAT(IF(a.type_id = 1, 'INT - ', 'EXT - '), a.name,' (',IFNULL(b.qty,0),')')  name,
	IFNULL(b.qty, 0) qty,
	a.type_id,
	IFNULL(b.max, 0) max,
	IF(b.qty > 0 AND a.type_id = 1, 1, IF(b.qty > 0 AND a.type_id = 2, 2, 3)) order_n
FROM
	inventory_location_master a
LEFT JOIN 
	inventory_location b
		ON a.id = b.location_master_id AND b.master_id = @master_id AND b.business_unit_id = @business_unit_id
WHERE
	a.business_unit_id = @business_unit_id AND a.type_id = 2
ORDER BY order_n ASC, qty DESC, type_id ASC, name ASC";
			#endregion To location datasource
		}
		else
		{
			#region To location datasource - Joining based on inventory_master_location.... all locations.
			ds_xfer_to_location.SelectCommand = @"
SELECT 0 id, '' name, 0 qty, 0 type_id, 0 max, 0 order_n
UNION 
SELECT
	a.id,
	CONCAT(IF(a.type_id = 1, 'INT - ', 'EXT - '), a.name,' (',IFNULL(b.qty,0),')')  name,
	IFNULL(b.qty, 0) qty,
	a.type_id,
	IFNULL(b.max, 0) max,
	IF(b.qty > 0 AND a.type_id = 1, 1, IF(b.qty > 0 AND a.type_id = 2, 2, 3)) order_n
FROM
	inventory_location_master a
LEFT JOIN
	inventory_location b ON a.id = b.location_master_id AND b.master_id = @master_id AND b.business_unit_id = @business_unit_id
WHERE
	a.business_unit_id = @business_unit_id
ORDER BY order_n ASC, qty DESC, type_id ASC, name ASC";
			#endregion To location datasource
		}
		xfer_from_location.DataBind();
		xfer_to_location.DataBind();
		ddl_commit_from_location.DataBind();
		ddl_return_to_location.DataBind();
		gv_history.DataBind();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		//		div_error.InnerHtml				= "";

		prep();
		scanbox.Focus();
		if (!IsPostBack)
		{
			xfer_lb_warning.Text = "";
			Session["working_master_id"] = "";
			Session["working_tab_index"] = is_admin ? 4 : 5;
			mv.ActiveViewIndex = is_admin ? 4 : 5;
			dteRequired.Text = System.DateTime.Today.AddDays(14).ToString("yyyy-MM-dd");
			btn_reset_Click(btn_reset, new EventArgs());
			pop_invalid_qty.JSProperties["cp_final_call"] = "";

		}
		else
		{
			var sb = new StringBuilder();
			sb.AppendFormat("USER => {0}\n", current_user.id);
			foreach (string key in Request.Form.Keys)
			{
				if (Request.Form[key].Length < 100)
				{
					sb.AppendFormat("{0} => {1}\n", key, Request.Form[key]);
				}
			}
			//_tools.debug_note("BARCODE_DEBUG::\n"+sb);
		}
		xfer_b_return.Text = "Remove from WO to the [To] Location";
	}
	protected void new_scan(object sender, EventArgs e)
	{
		var master_id = 0;
		var location_master_id = 0;
		var location_id = 0;
		var ilm = new location_master();
		var il = new location();
		double qty;
		try
		{
			var _reg = new Regex(@"(\d+)-\w+");
			var isError = false;
			double _qty = 0;
			if (!_reg.IsMatch(scanbox.Text))  // is the text box not a bar code scan?
			{
				//	scanbox.Text = "88-" + scanbox.Text;
			}

			if (_reg.IsMatch(scanbox.Text))
			{
				var _split = scanbox.Text.Replace("{", "").Replace("}", "").Split('-');
				var _type = Convert.ToInt32(_split[0]);
				if (mv.ActiveViewIndex == 2 && _type == 2)
				{
					po_lb_warning.Text = "You can only scan a work order on the transfer tab";
					scanbox.Text = "";
					return;
				}
				else if (mv.ActiveViewIndex == 0 && _type == 2)
				{
					admin_lb_warning.Text = "You can only scan a work order on the transfer tab";
					scanbox.Text = "";
					return;
				}

				else if (_type == 4 || _type == 6)
				{
					mv.ActiveViewIndex = 2;
				}
				else
				{
					po_lb_warning.Text = "";
					admin_lb_warning.Text = "";
				}
				switch (_type)
				{
					#region Inventory - 001
					case 1:  // if it was a master id
						// int _itemcount 		= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_item_master WHERE master_id = @v0  AND active = 1", new object[] {  _split[1] } );
						var _i = new inventory();
						_i.Load(_split[1], WarehouseBusinessUnit.id);
						master_id = Convert.ToInt32(_split[1]);

						if (!_i.active)
						{

							xfer_lb_warning.Text = "Not a valid part #";
							//		ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Invalid Part')", "Server"), true);
							isError = true;
						}
						else if ((_i.is_exclude || !_i.allowed_to_stock) && _i.master_id != "777")
						{
							xfer_lb_warning.Text = "This part is not allowed to be entered from a mobile device";
							//		ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Invalid Part')", "Server"), true);
							isError = true;
						}
						else if (!_i.allowed_to_stock && _i.master_id != "777")
						{
							xfer_lb_warning.Text = "This part is not a stocked part";
							//		ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Invalid Part')", "Server"), true);
							isError = true;
						}
						else if (_i.master_id == "777")
						{
							xfer_lb_warning.Text = "";
							lb_part_no.Text = _i.master_id;
							lb_part_description.Text = "";
							tb_777_description.Visible = true;
							tb_777_description.Focus();
						}
						else
						{
							tb_777_description.Visible = false;
							xfer_lb_warning.Text = "";
							if (get_master_id() != "" && get_master_id() != _split[1])
							{
								xfer_txtQty.Text = "";
							}
							lb_part_description.Text = _i.description_full;
							lb_part_no.Text = _i.master_id;
							if (xfer_txtQty.Text == "")
							{
								xfer_txtQty.Text = "";
							}
							else if (double.TryParse(xfer_txtQty.Text, out _qty))
							{
								xfer_txtQty.Text = Convert.ToString(_qty);
							}
							else
							{
								xfer_txtQty.Text = "";
							}							
							if (WarehouseBusinessUnit.country == "USA")
							{

								lbl_units.Text = _i.sold_as_usa_name;
							}
							else
							{
								lbl_units.Text = _i.sold_as_canadian_name;
							}

							Session["working_master_id"] = _i.master_id;
							Session["gv_search"] = null;
							update_qty_nos_in_masterid_lbl();
							populate_datasources();
							if (mv.ActiveViewIndex == 0)
							{
								#region Admin
								admin_int_location.DataBind();
								admin_ext_location.DataBind();
								#endregion Admin
							}
							else if (mv.ActiveViewIndex == 1) // if you're on the transfer tab
							{
								#region Transfer
								xfer_from_location.DataBind();
								xfer_to_location.DataBind();
								if (current_user.member_default_location != 0)
								{
									var str_loc_id = current_user.member_default_location.ToString();

									if (xfer_to_location.Items.FindByValue(str_loc_id) != null)
									{
										xfer_to_location.ClearSelection();
										xfer_to_location.Items.FindByValue(str_loc_id).Selected = true;
									}

									if (Session["working_int_loc_id"] != null && xfer_from_location.Items.FindByValue(Session["working_int_loc_id"].ToString()) != null)
									{
										xfer_from_location.ClearSelection();
										xfer_from_location.Items.FindByValue(Session["working_int_loc_id"].ToString()).Selected = true;
									}
								}
								#endregion Transfer
							}
							else if (mv.ActiveViewIndex == 2)
							{
								#region PO tab
								po_lineitem.DataBind();
								po_int_location.DataBind();
								po_ext_location.DataBind();
								try
								{

									//				double po_line_qty = string.IsNullOrEmpty(po_lineitem.SelectedValue) ? 0 : Toolbox.doSQL_double(@"SELECT IFNULL(po_details_qty_ordered - po_details_qty_received, 0) FROM po_details_current WHERE po_details_id = @v0 ", new object[] {  po_lineitem.SelectedValue } );
								}
								catch
								{
									ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('This part is not on this PO')", "Server"), true);
									return;
								}
								po_lineitem_binder();
								//po_lb_po_qty.Text			= "<b>"+po_line_qty.ToString("N")+"</b><br/><span style='font-size:11px;'>Unreceived on this line</span>";
								//po_lb_ext_qty.Text			= "<b>"+po_ext_qty.ToString("N")+"</b><br/><span style='font-size:11px;'>In stock at this location</span>";
								#endregion
							}
							else if (mv.ActiveViewIndex == 3)
							{
								if (hid_location_id.Value != "")
								{
									location_master_id = Convert.ToInt32(hid_location_id.Value);
								}
								if (location_master_id > 0)
								{
									ilm = new location_master(location_master_id);
								}
								if (master_id > 0)
								{
									il = new location(location_master_id, WarehouseBusinessUnit.id, master_id);
								}
								update_annual_fields(ilm, il);
								if (location_master_id > 0 && master_id > 0)
								{
									tb_annual_qty.Focus();
								}
							}
						}
						if (!isError)
						{
							update_quantity_labels(true);
						}
						scanbox.Text = "";
						break;
					#endregion inventory - 001
					#region WO - 002
					case 2:  // if it was a wo
						try
						{
							lb_wo_no.Text = _split[1];
							NeWOProg wo2;
							try
							{
								wo2 = new NeWOProg(Convert.ToInt32(lb_wo_no.Text));
							}
							catch
							{
								wo2 = new NeWOProg(WorkingBusinessUnit.id, lb_wo_no.Text.PadLeft(10, '0'));
							}

							isError = wo_entered(wo2);
							if (!isError)
							{
								try
								{
									if (dteRequired.Text == "")
									{
										dteRequired.Text = DateTime.Today.AddDays(14).ToString();
									}
									lb_wo_no.Text = wo2.woprog_id.ToString();

								}
								catch { throw; }
							}



						}
						catch (Exception ee)
						{
							lbbvwo.Text = "";
							lb_wo_no.Text = "";

							//	ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('" + ee.Message + "')", "Server"), true);
							xfer_lb_warning.Text = ee.Message;
							isError = true;
						}
						/*		if(current_user.member_default_location != 0 && mv.ActiveViewIndex == 1)
									{
									string str_loc_id		= current_user.member_default_location.ToString();
									if(xfer_int_location.Items.FindByValue(str_loc_id) != null)
										{
										xfer_int_location.ClearSelection();
										xfer_int_location.Items.FindByValue(str_loc_id).Selected	= true;
										}
									if(xfer_ext_location.Items.FindByValue(str_loc_id) != null)
										{
										xfer_ext_location.ClearSelection();
										xfer_ext_location.Items.FindByValue(str_loc_id).Selected	= true;
										}
									}
						*/

						//		update_quantity_labels(true);
						break;
					#endregion WO - 002
					#region Employee Login - 003
					case 3:  // if the scan is an employee login
						var _memberid = Convert.ToInt32(_split[1]);
						current_user = new NeMember(Convert.ToInt32(_memberid));
						current_user = new NeMember(current_user.Username, current_user.Password);
						if (current_user.Authenticated)
						{
							Session.Add("session", current_user.SessionID);
							//			lbl_user.Text = current_user.Username;
						}
						else
						{
							isError = true;
							ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Invalid Credentials')", "Server"), true);
							xfer_lb_warning.Text = "Invalid Credentials";
						}
						break;
					#endregion Employee Login - 003
					#region PO - 004
					case 4:
						var poprog_id = 0;
						if (int.TryParse(_split[1], out poprog_id))
						{
							var po_exists = Toolbox.doSQL_int("SELECT COUNT(*) FROM poprog_header WHERE poprog_id = @v0", poprog_id) == 1;
							if (po_exists)
							{
								var po = new NePOProg(poprog_id);
								if (po.poprog_status == 3)
								{
									label_bvwo.Visible = true;
									label_bvwo.InnerText = "BV PO#";
									lb_wo_no.Text = _split[1];
									lbbvwo.Text = po.poprog_bvpo;
									lbbvwo.Visible = true;
									Session["working_poprog_id"] = po.poprog_id;
									po_lineitem_binder();
									isError = po_entered(po);
								}
								else
								{
									ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Cant use this po in this status')", "Server"), true);
								}
							}
							else
							{
								ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('PO does not exist')", "Server"), true);
							}
						}
						else
						{
							ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Not a valid PO bard code')", "Server"), true);
						}

						break;
					#endregion PO - 004
					#region Location - 005
					case 5:
						location_master_id = Convert.ToInt32(_split[1]);
						master_id = Convert.ToInt32(Session["working_master_id"]);
						if (mv.ActiveViewIndex == 0)  // admin tab
						{
							if (admin_int_location.Items.FindByValue(location_master_id.ToString()) != null)
							{
								admin_int_location.ClearSelection();
								admin_int_location.Items.FindByValue(location_master_id.ToString()).Selected = true;
								ilm = new location_master(location_master_id);
								qty = Toolbox.doSQL_double(@"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2 ", new object[] {  location_master_id, WarehouseBusinessUnit.id, master_id } );
								admin_lb_int_qty.Text = string.Format("<b>{0:N}</b><br/><span style='font-size:11px;'>At this location</span>", qty);

								if (ilm.type_id == 2)
								{
									Session["admin_working_int_loc_id"] = location_master_id.ToString();
								}
								else
								{
									Session.Remove("admin_working_int_loc_id");
								}
							}
						}
						else if (mv.ActiveViewIndex == 1)  // xfer tab
						{
							if (xfer_from_location.Items.FindByValue(location_master_id.ToString()) != null)
							{
								xfer_from_location.ClearSelection();
								xfer_from_location.Items.FindByValue(location_master_id.ToString()).Selected = true;
								qty = Toolbox.doSQL_double(@"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2 ", new object[] {  location_master_id, WarehouseBusinessUnit.id, master_id } );
								xfer_lb_int_qty.Text = string.Format("<b>{0:N}</b><br/><span style='font-size:11px;'>At this location</span>", qty);
								xfer_lb_int_qty0.Text = xfer_lb_int_qty.Text;
								ilm = new location_master(location_master_id);
								if (ilm.type_id == 2)
								{
									Session["working_int_loc_id"] = location_master_id.ToString();
								}
								else
								{
									Session.Remove("working_int_loc_id");
								}
							}
						}
						else if (mv.ActiveViewIndex == 2)  // po tab
						{
							if (po_int_location.Items.FindByValue(location_master_id.ToString()) != null)
							{
								po_int_location.ClearSelection();
								po_int_location.Items.FindByValue(location_master_id.ToString()).Selected = true;
								qty = Toolbox.doSQL_double(@"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2 ", new object[] {  location_master_id, WarehouseBusinessUnit.id, master_id } );
								po_lb_int_qty.Text = string.Format("<b>{0:N}</b><br/><span style='font-size:11px;'>At this location</span>", qty);

								ilm = new location_master(location_master_id);
								if (ilm.type_id == 2)
								{
									Session["po_working_int_loc_id"] = location_master_id.ToString();
								}
								else
								{
									Session.Remove("po_working_int_loc_id");
								}
							}
						}
						else if (mv.ActiveViewIndex == 3) // annual updates
						{
							hid_location_id.Value = location_master_id.ToString();
							if (location_master_id > 0)
							{
								ilm = new location_master(location_master_id);
							}
							if (master_id > 0)
							{
								il = new location(location_master_id, WarehouseBusinessUnit.id, master_id);
							}
							update_annual_fields(ilm, il);
							if (location_master_id > 0 && master_id > 0)
							{
								tb_annual_qty.Focus();
							}
						}


						break;
					#endregion
					#region po line item 006
					case 6:
						if (mv.ActiveViewIndex == 2)
						{

						}
						break;

					#endregion
					#region quote 007

					#endregion
					#region location/master combo 009

					#endregion
					#region Typed-WO or PO - 088
					case 88:
						if (mv.ActiveViewIndex == 1)// if we're on the transfer tab, assume it's a work order
						{
							try
							{
								lb_wo_no.Text = _split[1];
								NeWOProg wo2;
								try
								{
									wo2 = new NeWOProg(WorkingBusinessUnit.id, lb_wo_no.Text.PadLeft(10, '0'));
								}
								catch
								{
									wo2 = new NeWOProg(Convert.ToInt32(lb_wo_no.Text));
								}
								isError = wo_entered(wo2);
								lb_wo_no.Text = wo2.woprog_id.ToString();


							}
							catch
							{
								lb_wo_no.Text = "";
								ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Invalid WO')", "Server"), true);
								isError = true;
							}
							if (current_user.member_default_location != 0 && mv.ActiveViewIndex == 1)
							{
								var str_loc_id = current_user.member_default_location.ToString();
								if (xfer_from_location.Items.FindByValue(str_loc_id) != null)
								{
									xfer_from_location.ClearSelection();
									xfer_from_location.Items.FindByValue(str_loc_id).Selected = true;
								}
								if (xfer_to_location.Items.FindByValue(str_loc_id) != null)
								{
									xfer_to_location.ClearSelection();
									xfer_to_location.Items.FindByValue(str_loc_id).Selected = true;
								}
							}
							update_quantity_labels(true);
						}
						else if (mv.ActiveViewIndex == 2)  // if we're on the PO tab, assume it's a PO
						{
							lb_wo_no.Text = _split[1];
							if (int.TryParse(_split[1], out poprog_id))
							{
								var po_exists = false;
								try
								{
									poprog_id = _tools.getSQL_int(@"Select poprog_id from poprog_header  where poprog_bvpo =@v0 and business_unit_id =@v1 ", new object[] { lb_wo_no.Text.PadLeft(10, '0'),WorkingBusinessUnit.id });
									po_exists = true;
								}
								catch
								{
									po_exists = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM poprog_header WHERE poprog_id =@v0 ", poprog_id) == 1;
								}

								if (po_exists)
								{
									var po = new NePOProg(poprog_id);
									if (po.poprog_status == 3)
									{
										label_bvwo.Visible = true;
										label_bvwo.InnerText = "BV PO#";
										lb_wo_no.Text = poprog_id.ToString();
										lbbvwo.Text = po.poprog_bvpo;
										lbbvwo.Visible = true;
										Session["working_poprog_id"] = po.poprog_id;
										po_lineitem_binder();
										isError = po_entered(po);
									}
									else
									{
										ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('You cant use this PO because of its status')", "Server"), true);
									}
									po_entered(po);
								}
								else
								{
									ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('PO does not exist')", "Server"), true);
								}

							}
							else
							{
								ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Not a valid PO bar code')", "Server"), true);
							}

						}
						break;
					#endregion WO - 088
					default:
						break;
				}
			}
			else
			{
				if (scanbox.Text != "")
				{
					scanbox.Text = scanbox.Text.Replace("{", "").Replace("}", "");
					var _itemcount = Toolbox.doSQL_dt(@"SELECT inventory_barcode.master_id FROM inventory_barcode WHERE barcode_no = @v0  AND is_active = 1 GROUP BY inventory_barcode.barcode_no ", new object[] {  scanbox.Text } );
					if (_itemcount.Rows.Count == 0)
					{
						xfer_lb_warning.Text = "Not a valid part #";
						isError = true;
					}
					else if (_itemcount.Rows.Count > 1)
					{
						xfer_lb_warning.Text = "Duplicate bar code";
						isError = true;
					}
					else
					{
						var _masterid = Toolbox.doSQL_int(@"Select master_id FROM inventory_barcode WHERE barcode_no = @v0 AND is_active = 1", scanbox.Text);
						lb_part_description.Text = Toolbox.doSQL_string(@"SELECT part_description(@v0, true, @v1)", new object[] { _masterid, current_user.business_unit.country});
						lb_part_no.Text = _masterid.ToString();
						if (xfer_txtQty.Text == "")
						{
							xfer_txtQty.Text = "";
						}
						else if (double.TryParse(xfer_txtQty.Text, out _qty))
						{
							xfer_txtQty.Text = Convert.ToString(_qty);
						}
						else
						{
							xfer_txtQty.Text = "";
						}
					}
				}
			}
			scanbox.Text = "";
			//		xfer_lb_notice.Text = notice;
			xfer_lb_warning.Visible = isError;
			if (!isError)
			{
				xfer_lb_warning.Text = "";

			}
			else
			{
				xfer_lb_warning.Visible = true;

			}

			#region Added by MH 9/14 per email from Iain.
			if (mv.ActiveViewIndex == 0 && current_user.member_default_location != 0 && !is_admin && !is_purchaser)
			{
				admin_int_location.ClearSelection();
				var li = admin_int_location.Items.FindByValue(current_user.member_default_location.ToString());
				if (li != null)
				{
					li.Selected = true;
				}
			}
			#endregion Added by MH 9/14 per email from Iain.
		}
		catch (Exception ee)
		{
			xfer_lb_warning.Text = ee.Message;
		}
		scanbox.Focus();
	}
	private void update_error(string msg)
	{
		div_error.InnerHtml = "<div style='width:75%;text-align:center;color:#fff;background-color:#c00;border-radius:5px;margin:2px;padding:5px;'>" + msg + "</div>";
		scanbox.Text = "";
	}
	protected void gv_annual_history_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
	{
		var dr = gv_annual_history.GetDataRow(Toolbox.ReturnZeroIfNull_int(e.CellValue));
		var previous_qty = Convert.ToDouble(dr["previous_qty"]);
		var qty = Convert.ToDouble(dr["qty"]);
		if (e.DataColumn.VisibleIndex == 0)
		{
			var image_src = previous_qty > qty ? "/image/icon[minus].gif" : previous_qty == qty ? "/image/pixel.gif" : "/image/icon[plus].gif";
			var image_alt = previous_qty > qty ? "minus" : previous_qty == qty ? "nochange" : "plus";
			var image_bgcolor = previous_qty > qty ? "#f00" : previous_qty == qty ? "#ccc" : "#0f0";
			e.Cell.Text = string.Format("<img src='{0}' width='12' height='12' style='border:solid 1px #{1};background-color:{1}' alt='{2}' />", image_src, image_bgcolor, image_alt);
		}
		else if (e.DataColumn.VisibleIndex == 1)
		{
			var cells = e.CellValue.ToString().Contains(" ") ? e.CellValue.ToString().Split(' ') : new string[] { "", "", "" };
			e.Cell.Text = string.Format("{0}<br />{1} {2}", cells[0], cells[1], cells[2]);
		}
	}
	private void fill_annual_history()
	{
		//StringBuilder sb			= new StringBuilder();
		//sb.Append(@"
		//		<table cellspacing='0' cellpadding='2'>");
		gv_annual_history.DataSource = Toolbox.doSQL_dt(@"SELECT a.date_transferred, a.masterid,b.name location,a.previous_qty,a.qty FROM inventory_annual_counts_history a LEFT JOIN inventory_location_master b ON a.locationid = b.id WHERE a.memberid = @v0  ORDER BY a.date_transferred DESC", new object[] {  current_user.id } );
		gv_annual_history.DataBind();
		/*
		foreach(DataRow dr in dt.Rows)
			{
			double previous_qty			= Convert.ToDouble(dr["previous_qty"]);
			double qty					= Convert.ToDouble(dr["qty"]);
			string image_src			= previous_qty > qty ? "/image/icon[minus].gif" : previous_qty == qty ? "/image/pixel.gif" : "/image/icon[plus].gif";
			string image_alt			= previous_qty > qty ? "minus" : previous_qty == qty ? "nochange" : "plus";
			string image_bgcolor		= previous_qty > qty ? "#f00" : previous_qty == qty ? "#ccc" : "#0f0";
			string date					= dr["date_transferred"].ToString();
			string part_no				= dr["masterid"].ToString();
			string location				= dr["location"].ToString();
			sb.AppendFormat(@"
						<tr style='padding-top:2px;border-bottom:solid 1px #bbb;height:15px;'>
							<td style='width:16px;text-align:center;'></td>
							<td style='width:30%;text-align:center;'>{3}</td>
							<td style='width:15%;border-left:solid 1px #bbb;text-align:center;'>{4}</td>
							<td style='width:40%;border-left:solid 1px #bbb;'>&nbsp;{5}</td>
							<td style='width:10%;text-align:center;border-left:solid 1px #bbb;'>{6}</td>
						</tr>",
								image_src,
								image_bgcolor,
								image_alt,
								date,
								part_no,
								location,
								qty
							   );
			}
		sb.Append(@"
				</table>");
		annual_history.InnerHtml			= sb.ToString();
		 */
	}
	private void update_annual_fields(location_master ilm, location il)
	{
		button_annual_qty.Enabled = ilm.id > 0 && il.id > 0;
		tb_annual_qty.Text = "";
		annual_table.Visible = true;
		annual_lb_location_name.Text = ilm.id > 0 ? ilm.name : "--";
		#region Percent Done -- Removed for now
		//int n_parts_total					= Toolbox.doSQL_int(@"SELECT COUNT(a.id) FROM inventory_location a WHERE a.location_master_id = @v0 ", new object[] {  ilm.id } );
		//int n_parts_done					= Toolbox.doSQL_int(@"SELECT COUNT(DISTINCT a.master_id) FROM inventory_location a RIGHT JOIN inventory_annual_counts_history b ON a.business_unit_id = b.business_unit_id AND a.master_id = b.masterid AND a.location_master_id = b.locationid WHERE b.locationid = @v0  AND date_transferred > DATE_SUB(CURDATE(), INTERVAL 14 DAY)", new object[] {  ilm.id } );
		//double pct_done						= (double) n_parts_done/n_parts_total;
		//annual_lb_location_qty.Text			= n_parts_done == 0 ? "0.00%" : pct_done.ToString("P2");
		#endregion Percent Done -- Removed for now
		#region Last Updated
		if (ilm.id > 0 && il.id > 0)
		{
			var last_updated_c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM inventory_annual_counts_history WHERE business_unit_id=@v0 AND masterid = @v1 AND locationid = @v2", new object[] { il.business_unit_id, il.master_id, il.location_master_id});
			var last_updated = "-- Never --";
			var is_recent = false;
			if (last_updated_c > 0)
			{
				last_updated = Toolbox.doSQL_string(@"SELECT MAX(date_transferred) FROM inventory_annual_counts_history WHERE business_unit_id=@v0 AND masterid = @v1 AND locationid = @v2", new object[] { il.business_unit_id, il.master_id, il.location_master_id});
				var date = new DateTime();
				DateTime.TryParse(last_updated, out date);
				if (date.Year > 1970)
				{
					last_updated = Toolbox.MySQL_longdt(date);
					var ts = DateTime.Now.Subtract(date);
					is_recent = ts.TotalDays < 14;
				}
			}
			annual_last_updated.Attributes["style"] = is_recent
													? "width:100%;text-align:center;background-color:#c00;color:#fff;font-size:1.75em;"
													: "width:100%;text-align:center;background-color:#060;color:#fff;font-size:1.75em;";
			annual_last_updated.InnerHtml = is_recent
													? "<div>Last Annual Update for this Part</div>" + last_updated
													: "<div>Last Annual Update for this Part</div>" + last_updated;
		}
		#endregion Last Updated
	}
	protected void button_annual_qty_Click(object sender, EventArgs e)
	{
		var master_id = 0;
		var location_master_id = 0;
		var location_id = 0;
		var ilm = new location_master();
		var il = new location();
		location_master_id = Convert.ToInt32(hid_location_id.Value);
		master_id = Convert.ToInt32(Session["working_master_id"]);
		if (location_master_id > 0)
		{
			ilm = new location_master(location_master_id);
		}
		if (master_id > 0)
		{
			il = new location(location_master_id, WarehouseBusinessUnit.id, master_id);
		}
		double qty = 0;
		var can_convert = double.TryParse(tb_annual_qty.Text, out qty);
		if (!can_convert)
		{
			xfer_lb_warning.Text = "Invalid Quantity Supplied.";
			return;
		}
		else if (qty < 0)
		{
			xfer_lb_warning.Text = "Negative Quantity Supplied.";
			return;
		}
		if (il.id > 0 && ilm.id > 0 && master_id > 0 && qty >= 0)
		{
			var ib = new branch(il.master_id, il.business_unit_id);
			var prev_qty = il.qty;
			var this_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  il.master_id, il.business_unit_id } );
			il.log_is_manual = true;
			il.member_id = current_user.id;
			il.alert_worthy = true;
			var diff = il.qty - qty;
			il.qty = qty;
			il.section_id = 1;
			var this_type = prev_qty > qty ? 3 : 2;
			var pre_ib = new branch(il.master_id, il.business_unit_id);
			il.save();
			il.update_branch(this_type, ib.dollar_balance, diff, this_cost, ib);
			var post_ib = new branch(il.master_id, il.business_unit_id);

			Toolbox.doSQL_void(@"
INSERT INTO inventory_annual_counts_history
	(
	business_unit_id,
	masterid,
	previous_qty,
	qty,
	locationid,
	memberid,
	date_transferred,
	origin,

	cost,
	prev_dollar_balance,
	post_dollar_balance,
	pre_onhand_qty,
	post_onhand_qty
	)
VALUES 
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5,
	NOW(),
	'barcode',
	
	@v6,
	@v7,
	@v8,
	@v9,
	@v10
	
	)
", 
				new object[] { 
	il.business_unit_id,			// {0}
	il.master_id,			// {1}
	il.qty,					// {2}
	tb_annual_qty.Text,		// {3}
	ilm.id,					// {4}
	current_user.id,		// {5}

	this_cost,				// {6}
	pre_ib.dollar_balance,	// {7}
	post_ib.dollar_balance,	// {8}
	pre_ib.onhand_qty,		// {9}
	post_ib.onhand_qty		// {10}
 });
			il = new location(location_master_id, WarehouseBusinessUnit.id, master_id);
		}
		update_annual_fields(ilm, il);
		fill_annual_history();
	}
	protected bool wo_entered(NeWOProg wo2)
	{
		var isError = false;
		if (current_user.business_unit_id != wo2.business_unit_id)
		{
			xfer_lb_warning.Visible = true;
			xfer_lb_warning.Text = "Sorry, you can only open work orders from your branch";

			//		ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('" + xfer_lb_warning.Text + "')", "Server"), true);
			Session["woid"] = null;
			update_qty_nos_in_masterid_lbl();
			return true;
		}
		lbbvwo.Text = wo2.OrderNumber.TrimStart('0') + " - " + (wo2.CustomerName.Length > 10 ? wo2.CustomerName.Substring(0, 10) : wo2.CustomerName);
		Session["working_poprog_id"] = "";
		//	xfer_ext_location.DataBind();
		//	xfer_int_location.DataBind();
		//	update_quantity_labels(true);
		var int_val_ex = xfer_from_location.SelectedValue;
		var ext_val_ex = xfer_to_location.SelectedValue;
		xfer_from_location.DataBind();
		xfer_to_location.DataBind();
		if (xfer_from_location.Items.FindByValue(int_val_ex) != null)
		{
			xfer_from_location.Items.FindByValue(int_val_ex).Selected = true;
		}
		if (xfer_to_location.Items.FindByValue(ext_val_ex) != null)
		{
			xfer_to_location.Items.FindByValue(ext_val_ex).Selected = true;
		}

		if (wo2.Status != OpsWOStatus.Open)
		{
			isError = true;
			lbbvwo.Text = "";
			lb_wo_no.Text = "";

			xfer_lb_warning.Text = "Sorry WO Status: " + wo2.Status;

			ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('" + xfer_lb_warning.Text + "')", "Server"), true);

			Session["woid"] = null;
			update_qty_nos_in_masterid_lbl();
		}
		else
		{
			var wotext = "WO: " + wo2.OrderNumber + @"\n";
			wotext += wo2.CustomerName + @"\n";
			wotext += wo2.Description;
			//		ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('" + wotext + "');</script>");
			Session["woid"] = wo2.woprog_id;
			update_qty_nos_in_masterid_lbl();

		}
		return isError;
	}
	protected void update_qty_nos_in_masterid_lbl()
	{
		if (Session["woid"] != null && get_master_id() != "" && get_master_id() != "777")
		{
			var dt = _tools.getSQL_datatable(@"Select ifnull(sum(wo_detail_current_qty_ordered),0) as req, ifnull(sum(wo_detail_current_qty_committed),0) as com from wo_detail_current  where wo_detail_current_master_id =@v0 and wo_detail_current_woprog_id =@v1 ", new object[] { get_master_id(),Session["woid"] });
			if (dt.Rows.Count > 0)
			{
				var required = Math.Round(Convert.ToDouble(dt.Rows[0][0]), 2);
				var committed = Math.Round(Convert.ToDouble(dt.Rows[0][1]), 2);
				//				lb_part_no.Text = Session["working_master_id"] + "    - Req: " + required + "   Com: " + committed;
				lbl_cmt_qty1.Text = lbl_cmt_qty0.Text = lbl_cmt_qty.Text = "Committed: " + committed;
				lbl_req_qty1.Text = lbl_req_qty0.Text = lbl_req_qty.Text = "Requested: " + required;
				lbl_req_onwo1.Visible = lbl_req_onwo0.Visible = lbl_req_onwo.Visible = true;
			}
		}
	}
	protected bool po_entered(NePOProg po2)
	{
		var isError = false;
		var vendorname = new NEVendor(po2.poprog_vendor_id).Name;
		lbbvwo.Text = po2.poprog_bvpo + " - " + (vendorname.Length > 10 ? vendorname.Substring(0, 10) : vendorname);
		Session["working_woprog_id"] = "";

		ds_po_int_location.DataBind();
		ds_po_ext_location.DataBind();
		update_quantity_labels(true);

		if (po2.poprog_status == 7 || po2.poprog_status == 8)
		{
			isError = true;
			lbbvwo.Text = "";
			lb_wo_no.Text = "";
			xfer_lb_warning.Text = "Sorry PO Status: " + po2.poprog_status;
			ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('" + xfer_lb_warning.Text + "')", "Server"), true);
		}
		else
		{
			var wotext = "PO: " + po2.poprog_bvpo + @"\n";
			wotext += vendorname + @"\n";
			wotext += po2.poprog_order_description;
			//		ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('" + wotext + "');</script>");
		}
		return isError;
	}
	protected string get_master_id()
	{
		var x = "";
		if (lb_part_no.Text.Contains("Req"))
		{
			x = lb_part_no.Text.Split('-').GetValue(0).ToString().TrimEnd(' ');
		}
		else
		{
			x = lb_part_no.Text;
		}

		return x;
	}
	protected void po_lineitem_binder()
	{
		var po_id = "";
		if (lb_wo_no.Text != "")
		{
			po_id = Convert.ToInt32(lb_wo_no.Text).ToString();
		}


		var m_id = get_master_id();
		var value = po_lineitem.SelectedValue;
		var _dt = new DataTable();
			if (po_id != "" && m_id != "")
			{
				_dt = Toolbox.doSQL_dt(@" SELECT a.po_details_id id, CONCAT(b.poprog_bvpo, ' - ', VENDOR_NAME(b.poprog_vendor_id),' - (', a.po_details_qty_ordered - a.po_details_qty_received, ' Remaining)') name FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE b.poprog_status = 3 AND b.poprog_id = @v0  AND a.po_details_part_no = @v1  AND b.business_unit_id = @v2  AND IS_EXCLUDE(po_details_part_no) = false AND po_details_line_active = 1", new object[] {  po_id, m_id, WorkingBusinessUnit.id } );
			}
			else if (po_id != "")
			{
				_dt = Toolbox.doSQL_dt(@" SELECT a.po_details_id id, CONCAT(b.poprog_bvpo, ' - (',a.po_details_part_no , ') - ', VENDOR_NAME(b.poprog_vendor_id),' - (', a.po_details_qty_ordered - a.po_details_qty_received, ' Remaining)') name FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE b.poprog_status = 3 AND b.poprog_id = @v0  AND b.business_unit_id = @v1  AND IS_EXCLUDE(po_details_part_no) = false AND po_details_line_active = 1", new object[] {  po_id, WorkingBusinessUnit.id  } );
			}
			else if (m_id != "")
			{
				_dt = Toolbox.doSQL_dt(@" SELECT a.po_details_id id, CONCAT(b.poprog_bvpo, ' - ', VENDOR_NAME(b.poprog_vendor_id),' - (', a.po_details_qty_ordered - a.po_details_qty_received, ' Remaining)') name FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE b.poprog_status = 3 AND a.po_details_part_no = @v0  AND b.business_unit_id = @v1  AND IS_EXCLUDE(po_details_part_no) = false AND po_details_line_active = 1", new object[] {  m_id, WorkingBusinessUnit.id  } );
			}
			else
			{
				_dt = Toolbox.doSQL_dt(@" SELECT a.po_details_id id, CONCAT(b.poprog_bvpo, ' - (',a.po_details_part_no , ') - ', VENDOR_NAME(b.poprog_vendor_id),' - (', a.po_details_qty_ordered - a.po_details_qty_received, ' Remaining)') name FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE b.poprog_status = 3 AND b.business_unit_id = @v0  AND IS_EXCLUDE(po_details_part_no) = false AND po_details_line_active = 1", new object[] {  WorkingBusinessUnit.id  } );
			}
			po_lineitem.DataSource = _dt;
			po_lineitem.DataBind();
			if (_dt.Rows.Count > 1)
			{
				po_lineitem.Items.Insert(0, new ListItem("Choose Line Item", "0"));
				if (lb_wo_no.Text == "" && get_master_id() == "")
				{
					po_lineitem.SelectedIndex = 0;
					value = "";
					update_quantity_labels(true);
				}
			}
			else
			{
				if (lb_wo_no.Text != "")
				{
					po_lineitem.SelectedIndex = 0;
					Session["working_master_id"] = Toolbox.doSQL_string(@"SELECT po_details_part_no FROM po_details_current WHERE po_details_id = @v0", po_lineitem.SelectedValue);
					update_quantity_labels(true);
				}
			}
			if (po_lineitem.Items.FindByValue(value) != null)
			{
				po_lineitem.Items.FindByValue(value).Selected = true;
			}
	}
	protected void btn_reset_Click(object sender, EventArgs e)
	{
		// General
		Session.Remove("working_master_id");
		Session.Remove("working_poprog_id");
		Session.Remove("working_woprog_id");
		Session.Remove("working_int_loc_id");
		Session["woid"] = null;

		lb_part_description.Text = "";
		lb_part_no.Text = "";
		lbbvwo.Text = "";
		lb_wo_no.Text = "";
		div_added.InnerHtml = "";
		
		// Admin
		admin_lb_warning.Text = "";
		admin_lb_ext_qty.Text = "";
		admin_lb_int_qty.Text = "";
		admin_txtnewcode.Text = "";
		admin_txtQty.Text = "";
		admin_lb_notice.Text = "";
		ds_admin_int_location.DataBind();
		ds_admin_ext_location.DataBind();

		// Transfer
		xfer_lb_warning.Text = "";
		xfer_lb_notice.Text = "";
		lbl_error_return.Text = "";
		lbl_error_commit.Text = "";


		xfer_lb_ext_qty.Text = "";
		xfer_lb_ext_qty0.Text = "";
		xfer_lb_int_qty.Text = "";
		xfer_lb_int_qty0.Text = "";
		xfer_txtQty.Text = "";
		lbl_units.Text = "";
		scanbox.Text = "";
		lbl_req_qty.Text = "";
		lbl_req_onwo.Visible = false;
		lbl_cmt_qty.Text = "";
		lbl_error_request.Text = "";
		txt_request_qty.Text = "";
		tb_777_description.Text = "";
		lbl_req_qty1.Text = "";
		lbl_req_qty0.Text = "";
		lbl_req_onwo0.Visible = false;
		lbl_req_onwo1.Visible = false;
		lbl_cmt_qty0.Text = "";
		lbl_cmt_qty1.Text = "";

		// Purchase Order
		po_lb_warning.Text = "";
		po_lb_ext_qty.Text = "";
		po_lb_po_qty.Text = "";
		po_lb_int_qty.Text = "";
		po_lb_notice.Text = "";
		po_txtQty.Text = "";
		po_lineitem.ClearSelection();
		update_quantity_labels(true);
		po_lineitem_binder();
		scanbox.Focus();
	}
	protected void prep()
	{
		var tab_index = Convert.ToInt32(Session["working_tab_index"]);
		tab_request.CssClass = tab_index != 4 ? "tab active" : "tab inactive";
		tab_commit.CssClass = tab_index != 5 ? "tab active" : "tab inactive";
		tab_return.CssClass = tab_index != 6 ? "tab active" : "tab inactive";
		tab_history.CssClass = tab_index != 7 ? "tab active" : "tab inactive";
		tab_shopping.CssClass = tab_index != 8 ? "tab active" : "tab inactive";

		if (is_admin)
		{
			tab_admin.Visible = tab_index != 0;
			tab_admin.CssClass = tab_index != 0 ? "tab active" : "tab inactive";
			tab_incorrect.CssClass = tab_index != 9 ? "tab active" : "tab inactive";
		}
		else
		{
			tab_admin.Visible = false;
			tab_admin.CssClass = "tab disabled";
			tab_incorrect.CssClass = "tab disabled";
		}
		if (is_purchaser || current_user.id == 8)
		{
			tab_po.Enabled = true;//tab_index != 2;
			tab_po.CssClass = tab_index != 2 ? "tab active" : "tab inactive";
		}
		else
		{
			tab_po.Enabled = false;
			tab_po.CssClass = "tab disabled";
		}
		tab_transfer.Enabled = can_commit && tab_index != 1;
		tab_commit.Enabled = can_commit;
		tab_return.Enabled = can_commit;

		tab_annual.Visible = false;
		tab_transfer.CssClass = tab_index != 1 ? "tab active" : "tab inactive";
		var annual_allowed = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(allow_mobile_annual), 0) FROM inventory_branch_options WHERE business_unit_id = @v0", new object[] { WarehouseBusinessUnit.id}) == 1;
		if (!IsPostBack)
		{
			if (annual_allowed && current_user.AuthenticatedForPrivilege(171))
			{
				tab_annual.Visible = true;
				tab_annual.Enabled = tab_index != 3;
				tab_annual.CssClass = tab_index != 3 ? "tab active" : "tab inactive";
			}
			else if (can_commit)
			{
				if (tab_index == 3)
				{
					mv.ActiveViewIndex = 1;
				}
			}
			else
			{
				mv.ActiveViewIndex = 4;
			}
		}

		po_b_rec_stock.Visible = is_admin;
		po_int_location.Visible = is_admin;
		po_lb_int_qty.Visible = is_admin;
		//xfer_b_move.Visible					= is_admin;
		lbbvwo.Visible = mv.ActiveViewIndex == 1 || mv.ActiveViewIndex == 2 || mv.ActiveViewIndex == 4 || mv.ActiveViewIndex == 5 || mv.ActiveViewIndex == 6 || mv.ActiveViewIndex == 8;
		label_bvwo.Visible = mv.ActiveViewIndex == 1 || mv.ActiveViewIndex == 2 || mv.ActiveViewIndex == 4 || mv.ActiveViewIndex == 5 || mv.ActiveViewIndex == 6 || mv.ActiveViewIndex == 8;
		label_bvwo.InnerText = mv.ActiveViewIndex == 1 || mv.ActiveViewIndex == 4 || mv.ActiveViewIndex == 5 || mv.ActiveViewIndex == 6 || mv.ActiveViewIndex == 8 ? "BV WO#:" : mv.ActiveViewIndex == 2 ? "BV PO#:" : "";
	}
	protected void add_line_to_wo(int masterid, int woid, double qty, string req_or_committed, DateTime d)
	{
		var starttime = DateTime.Now.TimeOfDay.TotalSeconds;
		var recnumber = 0;
		var so = new NeSalesOrder();
		var wo = new NeWOProg(Convert.ToInt32(woid));
		var inv = new inventory();
		inv.Load(masterid, WarehouseBusinessUnit.id);
		double dblComQty = 0;
		var strComQty = "select ifnull((SELECT wo_detail_current_qty_committed FROM wo_detail_current WHERE wo_detail_current_woprog_id = '" + wo.woprog_id + "' AND wo_detail_current_master_id = '" + masterid + "'),0)";
		try
		{
			dblComQty = Toolbox.doSQL_double(@"select ifnull((SELECT wo_detail_current_qty_committed FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_master_id =@v1 ),0) ", new object[] { wo.woprog_id,masterid });
		}
		catch
		{
			dblComQty = 0;
		}
		if (wo.Status == "Invoiced" || wo.Status == OpsWOStatus.WaitingToBeInvoiced)
		{
			xfer_lb_warning.Text = "You can not add or alter an item on this work order because it has been invoiced/is waiting to be invoiced.";
			return;
		}
		var detail = new NeWODetailCurrent();
		var line_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(wo_detail_current_id), 0) FROM wo_detail_current WHERE wo_detail_current_woprog_id = '" + wo.woprog_id + "' AND wo_detail_current_master_id = '" + masterid + "' LIMIT 1");
		if (line_id != 0 && masterid != 777)
		{
			detail = new NeWODetailCurrent(line_id);
		}
		so.working_line_id = masterid == 777 ? 0 : line_id;
		var cust = new NECustomer(Convert.ToInt32(wo.WOProg_Customer_ID));
		so.forcenewpartline = false;
		so.PartNo = global_pv.master_id.ToString();
		var bd = new bingo_data();
		bd.before_cost = detail.cost;
		bd.before_qty = detail.qty_committed;
		bd.before_sell = detail.sell;
		bd.detail_id = detail.id;
		bd.master_id = detail.master_id;
		bd.woprog_id = detail.woprog_id;
		bd.ca_qty = global_pv.qty;
		var do_bd_save = true;
		so.Desc = string.IsNullOrEmpty(detail.description) ? inv.description_int : detail.description;
		if (req_or_committed == "req") // Cost or sell should not be changed 
		{
			so.OrderedQuantity = global_pv.qty;
			so.ManualChange = true;
			so.DateRequired = d.ToString("yyyy-MM-dd");
			so.SellOverride = masterid == 777 ? 0 : detail.sell;
			so.CostOverRide = masterid == 777 ? 0 : detail.cost;
			so.forcenewpartline = masterid == 777;
			do_bd_save = false;
		}
		else if (req_or_committed == "com")
		{
			so.Quantity = global_pv.qty;
			double temp_cost = 0;
			double temp_qty = 0;
			if (!inv.allowed_to_stock && !inv.is_exclude)
			{
				temp_qty = dblComQty + so.Quantity <= 0 ? 1 : dblComQty + so.Quantity;
				temp_cost = Math.Round(Toolbox.doSQL_double(@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, @v2 )",  new object[] {  detail.master_id, WarehouseBusinessUnit.id, temp_qty }),3 );
				if (detail.qty_committed > 0)
				{
					temp_cost = Math.Round(Toolbox.doSQL_double(@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )",  new object[] {detail.woprog_id, detail.master_id, so.Quantity, temp_cost }),3 );
				}
				bd.ca_cost = temp_cost;
			}
			else
			{
				temp_qty = dblComQty + so.Quantity;
				if (so.Quantity > 0 && detail.qty_committed > 0 && detail.cost != inv.cost_price_branch)
				{
					temp_cost = Math.Round(Toolbox.doSQL_double(@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] {   detail.woprog_id, detail.master_id, so.Quantity, inv.cost_price_branch }),3 );
					bd.ca_cost = temp_cost;
				}
				else
				{
					temp_cost = Math.Round(inv.cost_price_branch, 2, MidpointRounding.AwayFromZero);
					bd.ca_cost = temp_cost;
				}
			}
			so.CostOverRide = temp_cost;
			so.SellOverride = wo.use_fixed_material_markup ? wo.fixed_material_markup * so.CostOverRide : shared.GetSellPrice(so.CostOverRide, 0, inv.is_qty, temp_qty, detail.business_unit_id);
		}
		else if (req_or_committed == "ret")
		{
			so.OrderedQuantity = Math.Abs(global_pv.qty) * -1;
			so.Quantity = Math.Abs(global_pv.qty) * -1;
			so.ActualQuantity = Math.Abs(global_pv.qty);
			double temp_cost = 0;
			double temp_qty = 0;
			if (!inv.allowed_to_stock && !inv.is_exclude)
			{
				temp_qty = dblComQty + so.Quantity <= 0 ? 1 : dblComQty + so.Quantity;
				temp_cost = Math.Round(Toolbox.doSQL_double(@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, @v2 )",  new object[] {  detail.master_id, WarehouseBusinessUnit.id, temp_qty }),3 );
				if (detail.qty_committed > 0)
				{
					temp_cost = Math.Round(Toolbox.doSQL_double(@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )",  new object[] {  detail.woprog_id, detail.master_id, so.Quantity, temp_cost }),3 );
				}
			}
			else
			{
				temp_qty = dblComQty + so.Quantity;
				temp_cost = detail.cost;
			}
			so.CostOverRide = temp_cost;
			bd.ca_cost = temp_cost;
			bd.ca_qty = temp_qty;
			so.SellOverride = wo.use_fixed_material_markup ? wo.fixed_material_markup * so.CostOverRide : shared.GetSellPrice(so.CostOverRide, 0, inv.is_qty, temp_qty, detail.business_unit_id);
		}
		if (do_bd_save)
		{
			bd.ca_sell = so.SellOverride;
			bd.after_cost = so.CostOverRide;
			bd.after_sell = so.SellOverride;
			bd.after_qty = dblComQty + so.Quantity;
			bd.save();
		}
		so.BillingTypeID = wo.QuoteID != "0" ? 1 : 0;
		if (masterid != 777)
		{
			so.Desc = detail.description == null ? inv.description : detail.description;
		}
		else
		{
			so.Desc = tb_777_description.Text;
		}
		so.CustomerDiscount = wo.woprog_apply_discount == 0 ? 0 : Convert.ToDouble(cust.Discount);
		var ilm = req_or_committed == "com"
									? new location_master(global_pv.int_location_id)
									: req_or_committed == "ret"
										? new location_master(global_pv.ext_location_id)
										: new location_master();
		if (masterid != 777)
		{
			if (req_or_committed == "com")
			{
				so.committing_from_location = ilm.name;
			}
			else if (req_or_committed == "ret")
			{
				so.uncommitting_to_location = ilm.name;
			}
		}
		try
		{
			recnumber = so.SavePart(wo.woprog_id, "FALSE", WorkingBusinessUnit.DSN, current_user.FullName, current_user.id, line_id == 0 || masterid == 777, "");
			if (masterid != 777)
			{

				var woc = new NeWOProgChanges();
				woc.WoProgChanges_WOProg_ID = (int)wo.woprog_id;
				woc.WoProgChanges_BVWO = wo.OrderNumber.TrimStart('0');
				woc.WoProgChanges_BVWORec = recnumber.ToString();
				woc.WoProgChanges_DateTime = Toolbox.MySQLNow_long();
				woc.WoProgChanges_Modified_Member_ID = current_user.id;
				woc.WoProgChanges_WOProgComment_ID = 0;
				woc.WoProgChanges_WasPartNo = detail.master_id.ToString();
				woc.WoProgChanges_WasPrice = detail.sell.ToString();
				woc.WoProgChanges_WasQty = detail.qty_committed.ToString();
				woc.WoProgChanges_IsPartNo = so.PartNo;
				woc.WoProgChanges_IsPrice = so.SellOverride.ToString();
				woc.WoProgChanges_IsQty = (detail.qty_committed + so.Quantity).ToString();
				woc.WoProgChanges_DeleteFlag = "false";
				woc.WoProgChanges_WasDesc = detail.description;
				woc.WoProgChanges_IsDesc = inv.description_int;
				woc.business_unit_id = WorkingBusinessUnit.id;
				woc.WOProgChanges_WasBillingType = detail.billtypeid;
				woc.WoProgChanges_IsBillingType = so.BillingTypeID;
				woc.WOProgChanges_OrderedQty = (detail.qty_ordered + so.OrderedQuantity).ToString();
				woc.WOProgChanges_ManualPriceChange = 0;
				woc.was_req_qty = detail.qty_ordered;
				if (req_or_committed == "com")
				{
					ilm = new location_master(global_pv.int_location_id);
					woc.FullWOComment = "Committed from Location: " + ilm.name;
				}
				else if (req_or_committed == "ret")
				{
					ilm = new location_master(global_pv.ext_location_id);
					woc.FullWOComment = "Returned to Location: " + ilm.name;
				}
				woc.AddtoWOProgChanges();
				NeWOProg.update_header_totals(wo.woprog_id.ToString(), WorkingBusinessUnit.id, wo.OrderNumber);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		if (Convert.ToInt32(masterid) < 990000 && !inv.is_exclude && inv.allowed_to_stock)
		{
			//	inv.Load(global_pv.master_id, current_user.business_unit_id);
			inv.Load(global_pv.master_id, WarehouseBusinessUnit.id);

			var fromstock = new Nestock_transfer();
			fromstock.type_id = 1;
			fromstock.master_id = global_pv.master_id;
			fromstock.description = inv.description;
			fromstock.business_unit_id = WarehouseBusinessUnit.id;
			fromstock.member_id = current_user.id;

			if (req_or_committed == "ret")
			{
				fromstock.to_id = 0;
				fromstock.from_id = wo.woprog_id;
				fromstock.to_id = global_pv.ext_location_id; // int_ is the FROM ddl
				fromstock.to_location_id = global_pv.ext_location_id;
				fromstock.type_id = 4;
				fromstock.quantity = global_pv.qty;
				fromstock.note = "Returned to stock via bar code scanner";
				fromstock.cost = detail.cost > 0 ? detail.cost : inv.cost_price_branch;
			}
			else if (req_or_committed == "com")
			{
				fromstock.to_id = wo.woprog_id;
				fromstock.from_id = global_pv.int_location_id; // int_ is the FROM ddl
				fromstock.from_location_id = global_pv.int_location_id;
				fromstock.quantity = global_pv.qty * -1;
				fromstock.note = "committed via bar code scanner";
				fromstock.cost = inv.cost_price_branch;
			}
			if (req_or_committed != "req")
			{
				fromstock.Nestock_transfer_save();
			}
		}
	}
	protected void txtQty_TextChanged(object sender, EventArgs e)
	{
	}
	protected void validate_entries(string req_or_committed)
	{
		if (lbl_masterid.Text == "777" && req_or_committed != "req")
		{
			xfer_lb_warning.Text = "Error";
			ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('777 can only be required on a WO')", "Server"), true);

			return;
		}
		else if (get_master_id() != "" && lb_wo_no.Text != "")
		{
			NeWOProg wo;
			try
			{
				wo = new NeWOProg(Convert.ToInt32(lb_wo_no.Text));
			}
			catch
			{
				try
				{
					wo = new NeWOProg(WorkingBusinessUnit.id, lb_wo_no.Text);
				}
				catch
				{
					xfer_lb_warning.Text = "Invalid wo";
					ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Invalid WO')", "Server"), true);

					return;
				}
			}

			xfer_lb_warning.Text = "";
			try
			{
				var d = System.DateTime.Today.AddDays(14);
				try
				{
					if (!string.IsNullOrEmpty(Request.Form["dteRequired"]))
					{
						d = Convert.ToDateTime(Request.Form["dteRequired"]);
					}
					else
					{
						d = Convert.ToDateTime(dteRequired.Text);
					}
				}
				catch { }
				add_line_to_wo(global_pv.master_id, wo.woprog_id, global_pv.qty, req_or_committed, d);
				status_update(string.Format("{0} ({1}) - {2} on {3}<br/>", global_pv.qty, req_or_committed, global_pv.master_id, wo.OrderNumber));
				//	xfer_txtQty.Text = "";  removed by andy dec 7 - to allow for easy committing and requiring
				xfer_lb_warning.Text = "";
			}
			catch (Exception ee)
			{
				xfer_lb_notice.Text = string.Format("crash while adding part to wo" + ee);
				return;
			}
			update_qty_nos_in_masterid_lbl();
		}
	}
	protected void btn_addman_Click(object sender, EventArgs e)
	{
		// Saving Manufacturer's Barcodes
		save_barcode("M");
	}
	protected void btn_addvend_Click(object sender, EventArgs e)
	{
		// Saving Vendor's Barcodes
		save_barcode("V");
	}
	private void save_barcode(string type)
	{
		var barcode = admin_txtnewcode.Text;
		var type_ = type == "V" ? "Vendor" : "Manufacturer ";
		if (barcode != "" && get_master_id() != "")
		{
			var count = Toolbox.doSQL_int(@"SELECT COUNT(master_id) FROM inventory_barcode where barcode_no = @v0 and table_type = @v1 and is_active = 1", new object[] { barcode, type});
			if (count > 0)
			{
				Toolbox.doSQL_void(@"Update inventory_barcode set master_id = @v0 where barcode_no = @v1 and table_type = @v2 and is_active = 1", new object[] { get_master_id(), barcode, type});
				status_update(type_ + " BC added to " + get_master_id() + "<br/>" + div_added.InnerHtml);
			}
			else
			{
				Toolbox.doSQL_void(@"INSERT INTO inventory_barcode (barcode_no,master_id,member_id_added,table_type,is_active) 
VALUES (@v0,@v1,@v2,@v3,1)", new object[] { barcode, get_master_id(), current_user.id, type});
				status_update(type_ + " BC added to " + get_master_id() + "<br/>" + div_added.InnerHtml);
			}
		}
		admin_txtnewcode.Text = "";
	}
	protected void status_update(string n)
	{
		n = "- " + n;
		var l = new Label();
		switch (mv.ActiveViewIndex)
		{
			case 0:
				l = admin_lb_notice;
				break;
			case 1:
				l = xfer_lb_notice;
				break;
			case 2:
				l = po_lb_notice;
				break;
		}
		// Does the label contain <br/>?
		var is_splittable = l.Text.Contains("<br/>");
		if (is_splittable)
		{
			// Split the existing notices by the line break tag, assuming there is one notice per line, 
			// but because you can't split by character(s) just replace the line breaks with pipes
			var notices = l.Text.Substring(0, l.Text.Length - 5).Replace("<br/>", "|").Split('|');
			// Only want to process if the count is greater than one
			if (notices.Length > 1)
			{
				// Check if there are more than 5 notices
				if (notices.Length == 5)
				{
					// Need to chop off the first one to make room for the new row.
					l.Text = "";
					//	notices			= notices.Where((val, idx) => idx != 0).ToArray();
					var ns = new List<string>(notices);
					ns.Add(n);
					for (var i = 0; i < ns.Count; i++)
					{
						l.Text += ns[i] != "" ? ns[i] + "<br/>" : "";
					}
				}
				else
				{
					l.Text += n + "<br/>";
				}
			}
			// Otherwise, just append.
			else
			{
				l.Text += n + "<br/>";
			}
		}
		// Otherwise just put the status message into the label.
		else
		{
			l.Text = n + "<br/>";
		}
	}
	protected void btn_printBC_Click(object sender, EventArgs e)
	{
		if (get_master_id() != "")
		{
			var inv = new inventory();
			if (lb_wo_no.Text != "")
			{
				inv.Load(get_master_id(), WarehouseBusinessUnit.id);
			}
			else
			{
				inv.Load(get_master_id(), WarehouseBusinessUnit.id);
			}
			inv.print_barcode_label(1);
		}
	}
	protected void tab_admin_Click(object sender, EventArgs e)
	{
		mv.ActiveViewIndex = 0;
		Session["working_tab_index"] = mv.ActiveViewIndex;
		tab_transfer.Enabled = can_commit;
		tab_admin.Enabled = false;
		tab_po.Enabled = is_purchaser ? true : false;//is_purchaser ? true : false;
		tbl_new.Visible = true;
	}
	protected void tab_incorrect_Click(object sender, EventArgs e)
	{
		tab_admin.Visible = is_admin ? true : false;
		mv.ActiveViewIndex = 9;
		Session["working_tab_index"] = mv.ActiveViewIndex;
		tab_transfer.Enabled = can_commit;
		tab_incorrect.Enabled = false;
		tab_po.Enabled = is_purchaser ? true : false;//is_purchaser ? true : false;
		tbl_new.Visible = false;		
		if (gv_incorrect_qtys.Rows.Count == 0)
		{
			gv_incorrect_qtys.Visible = false;
			admin_lb_warning0.Text = "There are no locations to correct";
			admin_lb_warning0.Visible = true;
		}
		else
		{
			admin_lb_warning0.Text = "";
			admin_lb_warning0.Visible = false;
		}
	}
	protected void tab_transfer_Click(object sender, EventArgs e)
	{
		mv.ActiveViewIndex = 1;
		Session["working_tab_index"] = mv.ActiveViewIndex;
		tab_admin.Visible = is_admin ? true : false;
		tab_transfer.Visible = can_commit;
		tab_po.Visible = is_purchaser ? true : false;//is_purchaser ? true : false;
		tbl_new.Visible = true;
	}
	protected void tab_po_Click(object sender, EventArgs e)
	{
		mv.ActiveViewIndex = 2;
		Session["working_tab_index"] = mv.ActiveViewIndex;
		tab_admin.Visible = is_admin ? true : false;
		tab_transfer.Visible = can_commit;
		tab_po.Visible = false;//is_purchaser ? true : false;
		tbl_new.Visible = true;
	}
	protected void tab_annual_Click(object sender, EventArgs e)
	{
		mv.ActiveViewIndex = 3;
		Session["working_tab_index"] = mv.ActiveViewIndex;
		tbl_new.Visible = true;
		//tab_admin.Enabled					= is_admin ? true : false;
		tab_transfer.Visible = can_commit;
		tab_po.Visible = is_purchaser ? true : false;//is_purchaser ? true : false;
	}
	protected void tab_request_Click(object sender, EventArgs e)
	{
		tab_admin.Visible = is_admin ? true : false;
		mv.ActiveViewIndex = 4;
		Session["working_tab_index"] = mv.ActiveViewIndex;
		dteRequired.Text = System.DateTime.Today.AddDays(14).ToString("yyyy-MM-dd");
		label_bvwo.Visible = true;
		tbl_new.Visible = true;
		tab_transfer.Enabled = can_commit;
		tab_po.Enabled = is_purchaser ? true : false;//is_purchaser ? true : false;
	}
	protected void tab_commmit_Click(object sender, EventArgs e)
	{
		tab_admin.Visible = is_admin ? true : false;
		mv.ActiveViewIndex = 5;
		Session["working_tab_index"] = mv.ActiveViewIndex;
		label_bvwo.Visible = true;
		tbl_new.Visible = true;
		tab_transfer.Enabled = can_commit;
		tab_po.Enabled = is_purchaser ? true : false;//is_purchaser ? true : false;
	}
	protected void tab_return_Click(object sender, EventArgs e)
	{
		mv.ActiveViewIndex = 6;
		Session["working_tab_index"] = mv.ActiveViewIndex;
		label_bvwo.Visible = true;
		tbl_new.Visible = true;
		tab_transfer.Enabled = can_commit;
		tab_po.Enabled = is_purchaser ? true : false;//is_purchaser ? true : false;
	}
	protected void tab_history_Click(object sender, EventArgs e)
	{
		tab_admin.Visible = is_admin ? true : false;
		mv.ActiveViewIndex = 7;
		Session["working_tab_index"] = mv.ActiveViewIndex;
		tbl_new.Visible = true;
		tab_transfer.Enabled = can_commit;
		tab_po.Enabled = is_purchaser ? true : false;//is_purchaser ? true : false;
	}
	protected void tab_shopping_Click(object sender, EventArgs e)
	{
		tab_admin.Visible = is_admin ? true : false;
		mv.ActiveViewIndex = 8;
		Session["working_tab_index"] = mv.ActiveViewIndex;
		label_bvwo.Visible = true;
		tbl_new.Visible = true;
		lbl_masterid.Visible = true;
		tab_transfer.Enabled = can_commit;
		tab_po.Enabled = is_purchaser ? true : false;//is_purchaser ? true : false;
	}
	protected void xfer_b_move_Click(object sender, EventArgs e)
	{
		// Errored 
		var is_valid = true;
		xfer_lb_warning.Text = "";
		xfer_lb_warning.Style.Add("Padding", "5px");
		var pv = prep_vars();
		if (get_master_id() == "")
		{
			xfer_lb_warning.Text += "- You need to first define the part you are using.<br/>";
			return;
		}
		if (pv.int_qty == 0)
		{
			is_valid = false;
			xfer_lb_warning.Text += "- Your FROM location doesn't have stock to move.<br/>";
		}
		if (pv.qty == 0)
		{
			is_valid = false;
			xfer_lb_warning.Text += "- Please define a quantity to transfer.<br/>";
		}
		if (pv.qty > pv.int_qty)
		{
			is_valid = false;
			xfer_lb_warning.Text += "- You are trying to move more stock than your FROM location has.<br/>";
		}
		if (pv.qty < 0)
		{
			is_valid = false;
			xfer_lb_warning.Text += "- You can't transfer a negative quantity.<br/>";
		}
		//if(pv.ext_location_id == pv.ext2_location_id)
		//	{
		//	is_valid				= false;
		//	xfer_lb_warning.Text	+= "- Please choose different locations.<br/>";
		//	}
		if (pv.int_location_id == 0)
		{
			is_valid = false;
			xfer_lb_warning.Text += "- Please select a location to transfer FROM.<br/>";
		}
		if (pv.ext_location_id == 0)
		{
			is_valid = false;
			xfer_lb_warning.Text += "- Please select a location to transfer TO.<br/>";
		}
		if (pv.ext_location_id == pv.int_location_id && pv.int_location_id != 0 && pv.ext_location_id != 0)
		{
			is_valid = false;
			xfer_lb_warning.Text += "- You can't transfer TO and FROM the same location.<br/>";
		}
		if (pv.int_location_id != 0 && pv.ext_location_id != 0)
		{
			var from_loc = new location_master(pv.int_location_id);
			var to_loc = new location_master(pv.ext_location_id);
			if (from_loc.type_id == 1 && to_loc.type_id == 1) // both are internal
			{
				//is_valid				= false;
				//xfer_lb_warning.Text	+= "- You can't use internal locations for both the TO and FROM.<br/>";
			}
		}
		//if(pv.ext2_location_id == 0)
		//	{
		//	is_valid				= false;
		//	xfer_lb_warning.Text	+= "- Please select a location to transfer to.<br/>";
		//	}
		if (!is_valid)
		{
			return;
		}
		else
		{
			try
			{
				global_pv = pv;
				var location_from = new location(pv.int_location_id, WarehouseBusinessUnit.id, pv.master_id);
				var location_to = new location(pv.ext_location_id, WarehouseBusinessUnit.id, pv.master_id);
				var il = new location();
				il.section_id = 12;
				il.xfer((int)location_from.id, (int)location_to.id, pv.qty, current_user);

				xfer_txtQty.Text = "";
				status_update(pv.qty + " " + pv.master_id + " moved from " + location_from.this_master.name + " to " + location_to.this_master.name);
				update_quantity_labels(true);
			}
			catch
			{
				throw;
			}
		}
	}
	protected void xfer_b_tostock_Click(object sender, EventArgs e)
	{
		var is_valid = true;

		lbl_error_return.Text = "";
		lbl_error_return.Style.Add("Padding", "5px");
		var pv = prep_vars();
		if (lb_wo_no.Text == "")
		{
			lbl_error_return.Text += "- You need to first define the work order to return stock from.<br/>";
			return;
		}
		var wo = new NeWOProg(Convert.ToInt32(lb_wo_no.Text));
		if (get_master_id() == "")
		{
			lbl_error_return.Text += "- You need to first define the part you are using.<br/>";
			return;
		}
		var on_wo = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(wo_detail_current_qty_committed),0) FROM wo_detail_current  WHERE wo_detail_current_master_id =@v0 AND wo_detail_current_woprog_id =@v1 ", new object[] { get_master_id(),lb_wo_no.Text });
		if (wo.Status != OpsWOStatus.Open)
		{
			lbl_error_return.Text += "- This is not an <i>Open</i> work order<br/>";
			return;
		}
		if (pv.qty == 0)
		{
			is_valid = false;
			lbl_error_return.Text += "- Please define a quantity to commit.<br/>";
		}
		if (pv.qty < 0)
		{
			is_valid = false;
			lbl_error_return.Text += "- You can't transfer a negative quantity.<br/>";
		}
		if (pv.qty > on_wo)
		{
			is_valid = false;
			lbl_error_return.Text += "- You can't transfer a quantity greater than what is on the work order.<br/>";
		}
		if (pv.ext_location_id == 0)
		{
			is_valid = false;
			lbl_error_return.Text += "- Please select a location to return stock to.<br/>";
		}
		var ext_loc = new location_master(pv.ext_location_id);
		//if(ext_loc.type_id != 2)
		//	{
		//	is_valid				= false;
		//	xfer_lb_warning.Text	+= "- Please select an <b>external</b> location to return stock to.<br/>";
		//	}
		if (!is_valid)
		{
			return;
		}
		else
		{
			try
			{
				global_pv = pv;
				validate_entries("ret");
				update_quantity_labels(true);
				update_qty_nos_in_masterid_lbl();
				populate_datasources();
				txt_return_qty.Text = "";

				ddl_return_to_location.SelectedValue = pv.ext_location_id.ToString();
				gv_shopping.DataBind();
			}
			catch
			{
				throw;
			}
		}
	}
	protected void admin_int_location_SelectedIndexChanged(object sender, EventArgs e)
	{
		var ddl = (DropDownList)sender;
		admin_lb_int_qty.Text = "<b>" + Toolbox.doSQL_double(@"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2 ", new object[] {  ddl.SelectedValue, WarehouseBusinessUnit.id, Session["working_master_id"] } ).ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>";
	}
	protected void admin_ext_location_SelectedIndexChanged(object sender, EventArgs e)
	{
		var ddl = (DropDownList)sender;
		admin_lb_ext_qty.Text = "<b>" + Toolbox.doSQL_double(@"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2 ", new object[] {  ddl.SelectedValue, WarehouseBusinessUnit.id, Session["working_master_id"] } ).ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>";
	}
	protected void xfer_int_location_SelectedIndexChanged(object sender, EventArgs e)
	{
		var ddl = (DropDownList)sender;
		if (ddl.SelectedValue != "0")
		{
			xfer_lb_int_qty.Text = "<b>" + Toolbox.doSQL_double(@"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2 ", new object[] {  ddl.SelectedValue, WarehouseBusinessUnit.id, Session["working_master_id"] } ).ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>";
			xfer_lb_int_qty0.Text = xfer_lb_int_qty.Text;
			var inv = new location_master(Convert.ToInt32(ddl.SelectedValue));
			if (inv.type_id == 2)
			{
				Session["working_int_loc_id"] = ddl.SelectedValue;
			}
			else
			{
				Session.Remove("working_int_loc_id");
			}
		}
		else
		{
			Session["working_int_loc_id"] = "0";
		}
	}
	protected void xfer_ext_location_SelectedIndexChanged(object sender, EventArgs e)
	{
		var ddl = (DropDownList)sender;
		xfer_lb_ext_qty0.Text = xfer_lb_ext_qty.Text = "<b>" + Toolbox.doSQL_double(@"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2 ", new object[] {  ddl.SelectedValue, WarehouseBusinessUnit.id, Session["working_master_id"] } ).ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>";
	}
	protected void po_ext_location_SelectedIndexChanged(object sender, EventArgs e)
	{
		var ddl = (DropDownList)sender;
		po_lb_ext_qty.Text = "<b>" + Toolbox.doSQL_double(@"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2 ", new object[] {  ddl.SelectedValue, WarehouseBusinessUnit.id, Session["working_master_id"] } ).ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>";
	}
	protected void po_int_location_SelectedIndexChanged(object sender, EventArgs e)
	{
		var ddl = (DropDownList)sender;
		po_lb_int_qty.Text = "<b>" + Toolbox.doSQL_double(@"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND business_unit_id = @v1  AND master_id = @v2 ", new object[] {  ddl.SelectedValue, WarehouseBusinessUnit.id, Session["working_master_id"] } ).ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>";
	}
	protected void po_lineitem_SelectedIndexChanged(object sender, EventArgs e)
	{
		var ddl = (DropDownList)sender;
		po_lb_po_qty.Text = "";
		po_txtQty.Text = "";
		if (lbbvwo.Text != "" && ddl.SelectedValue != "" && ddl.SelectedValue != "0")
		{
			var temp_qty = Toolbox.doSQL_double(@"SELECT IFNULL(po_details_qty_ordered - po_details_qty_received, 0) FROM po_details_current WHERE po_details_id = @v0 ", new object[] {  ddl.SelectedValue } );
			po_lb_po_qty.Text = ddl.SelectedValue != "" && ddl.SelectedValue != "0" ? "<b>" + temp_qty.ToString("N") + "</b><br/><span style='font-size:11px;'>Unreceived on this line</span>" : "";

			var poprog_id = Toolbox.doSQL_int(@"SELECT po_details_poprog_id FROM po_details_current WHERE po_details_id = @v0", ddl.SelectedValue);
			var part_no = Toolbox.doSQL_string(@"SELECT po_details_part_no FROM po_details_current WHERE po_details_id = @v0", ddl.SelectedValue);
			var po = new NePOProg(poprog_id);
			lbbvwo.Text = po.poprog_bvpo;
			lb_wo_no.Text = poprog_id.ToString();
			Session["working_poprog_id"] = poprog_id;
			var inv = new inventory();
			inv.Load(part_no, WarehouseBusinessUnit.id);
			Session["working_master_id"] = part_no;
			lb_part_description.Text = inv.description_full;
			lb_part_no.Text = get_master_id() == "" ? part_no : get_master_id();
			po_txtQty.Text = temp_qty.ToString(); ;
			po_lineitem_binder();
			update_quantity_labels(true);
		}
	}
	protected void mv_ActiveViewChanged(object sender, EventArgs e)
	{
		if (current_user != null)
		{
			Session["working_tab_index"] = mv.ActiveViewIndex;
			if (lb_wo_no.Text != "" || lbbvwo.Text != "")
			{
				//		lb_wo_no.Text = lbbvwo.Text		= "";
			}
			if (mv.ActiveViewIndex == 2)
			{
				po_lineitem.Items.Clear();
				po_lineitem.SelectedIndex = -1;
				po_lineitem_binder();
			}
			update_quantity_labels(false);
			prep();
		}
	}
	protected void po_b_rec_stock_Click(object sender, EventArgs e)
	{
		var is_valid = true;
		xfer_lb_warning.Text = "";
		xfer_lb_warning.Style.Add("Padding", "5px");
		var pv = prep_vars();
		if (get_master_id() == "")
		{
			po_lb_warning.Text += "- You need to first define the part you are using.<br/>";
			return;
		}
		if (pv.qty == 0)
		{
			is_valid = false;
			po_lb_warning.Text += "- Please define a quantity to transfer.<br/>";
		}
		if (pv.qty < 0)
		{
			is_valid = false;
			po_lb_warning.Text += "- You can't transfer a negative quantity.<br/>";
		}
		if (pv.int_location_id == 0)
		{
			is_valid = false;
			po_lb_warning.Text += "- Please select a location to transfer to.<br/>";
		}
		if (!is_valid)
		{
			return;
		}
		else
		{
			try
			{
				var podc = new NEPO_Details_Current();
				podc.po_details_current_line(pv.po_lineitem_id);
				podc.po_details_qty_received += pv.qty;

				podc.PO_Details_Update();
				var inv = new inventory();
				inv.Load(pv.master_id, WarehouseBusinessUnit.id);

				var st = new Nestock_transfer();
				st.type_id = 2;
				st.master_id = pv.master_id;
				st.description = inv.description;
				st.business_unit_id = WarehouseBusinessUnit.id;
				st.member_id = current_user.id;
				st.to_id = pv.int_location_id;
				st.to_location_id = pv.int_location_id;
				st.from_id = podc.po_details_poprog_id;
				st.quantity = pv.qty;
				st.note = "Received via bar code scanner";
				st.cost = inv.cost_price_branch;
				st.Nestock_transfer_save();

				xfer_txtQty.Text = "";
				status_update("Received " + pv.qty + " " + pv.master_id + " to " + new location_master(pv.int_location_id).name);
				lb_part_no.Text = "";
				lb_part_description.Text = "";
				Session["working_master_id"] = null;
				update_quantity_labels(true);
			}
			catch
			{
				throw;
			}
		}
	}
	protected void po_b_rec_location_Click(object sender, EventArgs e)
	{
		var is_valid = true;
		xfer_lb_warning.Text = "";
		xfer_lb_warning.Style.Add("Padding", "5px");
		var pv = prep_vars();
		if (get_master_id() == "")
		{
			po_lb_warning.Text += "- You need to first define the part you are using.<br/>";
			return;
		}
		if (pv.qty == 0)
		{
			is_valid = false;
			po_lb_warning.Text += "- Please define a quantity to transfer.<br/>";
		}
		if (pv.qty < 0)
		{
			is_valid = false;
			po_lb_warning.Text += "- You can't transfer a negative quantity.<br/>";
		}
		if (pv.ext_location_id == 0)
		{
			is_valid = false;
			po_lb_warning.Text += "- Please select a valid external location to transfer to.<br/>";
		}
		if (!is_valid)
		{
			return;
		}
		else
		{
			try
			{
				var podc = new NEPO_Details_Current();
				podc.po_details_current_line(pv.po_lineitem_id);
				podc.po_details_qty_received += pv.qty;
				podc.PO_Details_Update();
				//		location_to.qty					+= pv.qty;
				//		location_to.save();
				var inv = new inventory();
					inv.Load(pv.master_id, WarehouseBusinessUnit.id);

				var st = new Nestock_transfer();
				st.type_id = 2;
				st.master_id = pv.master_id;
				st.description = inv.description;
				st.business_unit_id = WarehouseBusinessUnit.id;
				st.member_id = current_user.id;
				st.to_id = pv.ext_location_id;
				st.to_location_id = pv.ext_location_id;
				st.from_id = podc.po_details_poprog_id;
				st.quantity = pv.qty;
				st.note = "Received via bar code scanner";
				st.cost = inv.cost_price_branch;
				st.Nestock_transfer_save();

				xfer_txtQty.Text = "";
				status_update("Received " + pv.qty + " " + pv.master_id + " to " + new location_master(pv.ext_location_id).name);

				lb_part_no.Text = "";
				lb_part_description.Text = "";
				Session["working_master_id"] = null;
				update_quantity_labels(true);
			}
			catch
			{
				throw;
			}
		}
	}
	protected void update_quantity_labels(bool force)
	{
		var tab_index = Convert.ToInt32(Session["working_tab_index"]);
		Session["working_master_id"] = Session["working_master_id"] == null || string.IsNullOrEmpty(Session["working_master_id"].ToString()) ? "0" : Session["working_master_id"];
		var mast = Session["working_master_id"].ToString();
		if (tab_index == 0)
		{
			#region Admin
			var admin_int_qty = !force || admin_int_location.SelectedIndex == -1 ? Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = @v0  AND a.master_id = @v1  ORDER BY qty DESC", 
				new object[] {  WarehouseBusinessUnit.id, Session["working_master_id"] })
				: 
				Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = @v0 AND a.master_id = @v1 AND a.location_master_id = @v2 ORDER BY qty DESC", 
				new object[] { WarehouseBusinessUnit.id, Session["working_master_id"], admin_int_location.SelectedValue });
			admin_lb_int_qty.Text = admin_lb_int_qty.Text == "" || force ? "<b>" + admin_int_qty.ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>" : admin_lb_int_qty.Text;
			var admin_ext_qty = !force || admin_ext_location.SelectedIndex == -1 ?
				Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location_master b LEFT JOIN inventory_location a ON b.id = a.location_master_id AND a.master_id = @v1  WHERE b.business_unit_id = @v0  ORDER BY qty DESC", new object[] {  WarehouseBusinessUnit.id, Session["working_master_id"]})
				: 
				Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location_master b LEFT JOIN inventory_location a ON b.id = a.location_master_id AND a.master_id = @v1 WHERE b.business_unit_id = @v0 AND b.id = @v2 ORDER BY qty DESC", new object[] { WarehouseBusinessUnit.id, Session["working_master_id"], admin_ext_location.SelectedValue });
			admin_lb_ext_qty.Text = admin_lb_ext_qty.Text == "" || force ? "<b>" + admin_ext_qty.ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>" : admin_lb_ext_qty.Text;
			admin_lb_warning.Text = "";
			admin_ext_location.DataBind();
			admin_int_location.DataBind();
			#endregion Admin
		}
		else if (tab_index == 1 || tab_index == 5 || tab_index == 6)
		{
			#region Transfer
			if (tab_index == 5 && ddl_commit_from_location.SelectedValue != "")
			{
				xfer_from_location.SelectedValue = ddl_commit_from_location.SelectedValue;
			}
			else if (tab_index == 6 && ddl_return_to_location.SelectedValue != "")
			{
				xfer_to_location.SelectedValue = ddl_return_to_location.SelectedValue;
			}

			var xfer_int_qty = !force ? Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = @v0  AND a.master_id = @v1  ORDER BY qty DESC", new object[] {  WarehouseBusinessUnit.id, Session["working_master_id"] })
				: Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location_master b LEFT JOIN inventory_location a ON b.id = a.location_master_id AND a.master_id = @v1 WHERE b.business_unit_id = @v0 AND b.id = @v2 ORDER BY qty DESC", new object[] { WarehouseBusinessUnit.id, Session["working_master_id"], xfer_from_location.SelectedValue });
			xfer_lb_int_qty.Text = xfer_lb_int_qty.Text == "" || force ? "<b>" + xfer_int_qty.ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>" : xfer_lb_int_qty.Text;
			xfer_lb_int_qty0.Text = xfer_lb_int_qty.Text;
			var xfer_ext_qty = !force ? Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = @v0  AND a.master_id = @v1  ORDER BY qty DESC", new object[] {  WarehouseBusinessUnit.id, Session["working_master_id"] })
				: Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location_master b LEFT JOIN inventory_location a ON b.id = a.location_master_id AND a.master_id = @v1 WHERE b.business_unit_id = @v0 AND b.id = @v2 ORDER BY qty DESC", new object[] { WarehouseBusinessUnit.id, Session["working_master_id"], xfer_to_location.SelectedValue });
			xfer_lb_ext_qty.Text = xfer_lb_ext_qty.Text == "" || force ? "<b>" + xfer_ext_qty.ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>" : xfer_lb_ext_qty.Text;
			xfer_lb_ext_qty0.Text = xfer_lb_ext_qty.Text;
			xfer_lb_warning.Text = "";
			var int_val_ex = xfer_from_location.SelectedValue;
			var ext_val_ex = xfer_to_location.SelectedValue;
			xfer_from_location.DataBind();
			xfer_to_location.DataBind();
			if (xfer_from_location.Items.FindByValue(int_val_ex) != null)
			{
				xfer_from_location.Items.FindByValue(int_val_ex).Selected = true;
			}
			if (xfer_to_location.Items.FindByValue(ext_val_ex) != null)
			{
				xfer_to_location.Items.FindByValue(ext_val_ex).Selected = true;
			}
			//xfer_ext_location.DataBind();
			//xfer_int_location.DataBind();
			//if(force)
			//    {
			//    if(global_pv.int_location_id != 0)
			//        {
			//        xfer_int_location.Items.FindByValue(global_pv.int_location_id.ToString()).Selected		= true;
			//        }
			//    if(global_pv.ext_location_id != 0)
			//        {
			//        xfer_ext_location.Items.FindByValue(global_pv.ext_location_id.ToString()).Selected		= true;
			//        }
			//    }
			update_qty_nos_in_masterid_lbl();
			#endregion Transfer
		}
		else if (tab_index == 2)
		{
			#region Purchase Order
			po_txtQty.Text = "";
			var po_int_qty = !force ? Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = @v0  AND a.master_id = @v1  AND b.type_id = 1 ORDER BY qty DESC",new object[] {  WarehouseBusinessUnit.id, Session["working_master_id"]})
				: Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location_master b LEFT JOIN inventory_location a ON b.id = a.location_master_id AND a.master_id = @v1 WHERE b.business_unit_id = @v0 AND b.id = @v2 AND b.type_id = 1 ORDER BY qty DESC", new object[] { WarehouseBusinessUnit.id, Session["working_master_id"], po_int_location.SelectedValue });
			po_lb_int_qty.Text = po_lb_int_qty.Text == "" || force ? "<b>" + po_int_qty.ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>" : po_lb_int_qty.Text;
			var po_ext_qty = !force ? Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = @v0  AND a.master_id = @v1  AND b.type_id = 2 ORDER BY qty DESC",new object[] {  WarehouseBusinessUnit.id, Session["working_master_id"] }) : Toolbox.doSQL_double(@" SELECT IFNULL(MAX(a.qty),0) qty FROM inventory_location_master b LEFT JOIN inventory_location a ON b.id = a.location_master_id AND a.master_id = @v1 WHERE b.business_unit_id = @v0 AND b.id = @v2 AND b.type_id = 2 ORDER BY qty DESC", new object[] { WarehouseBusinessUnit.id, Session["working_master_id"], po_ext_location.SelectedValue });
			po_lb_ext_qty.Text = po_lb_ext_qty.Text == "" || force ? "<b>" + po_ext_qty.ToString("N") + "</b><br/><span style='font-size:11px;'>At this location</span>" : po_lb_ext_qty.Text;
			if (!string.IsNullOrEmpty(po_lineitem.SelectedValue) && po_lineitem.SelectedValue != "0")
			{
				var qty_req = Toolbox.doSQL_double(@"SELECT po_details_qty_ordered - po_details_qty_received FROM po_details_current WHERE po_details_id = @v0 ", new object[] {  po_lineitem.SelectedValue } );
				po_txtQty.Text = qty_req.ToString();
			}
			var po_line_qty = string.IsNullOrEmpty(po_lineitem.SelectedValue) || po_lineitem.SelectedValue == "0" ? 0 : Toolbox.doSQL_double(@"SELECT IFNULL(po_details_qty_ordered - po_details_qty_received, 0) FROM po_details_current WHERE po_details_id = @v0 ", new object[] {  po_lineitem.SelectedValue } );
			po_lb_po_qty.Text = po_lineitem.SelectedValue != "0" ? "<b>" + po_line_qty.ToString("N") + "</b><br/><span style='font-size:11px;'>Unreceived on this line</span>" : "";
			po_lb_warning.Text = "";
			var int_val = po_int_location.SelectedValue;
			var ext_val = po_ext_location.SelectedValue;
			po_int_location.DataBind();
			po_ext_location.DataBind();
			if (po_int_location.Items.FindByValue(int_val) != null)
			{
				po_int_location.Items.FindByValue(int_val).Selected = true;
			}
			if (po_ext_location.Items.FindByValue(int_val) != null)
			{
				po_ext_location.Items.FindByValue(int_val).Selected = true;
			}
			#endregion Purchase Order
		}
		else if (tab_index == 4) // request
		{

		}
		else if (tab_index == 5) // commit
		{

		}
	}
	protected void admin_b_transfer_Click(object sender, EventArgs e)
	{
		// Run through checks
		var is_valid = true;
		admin_lb_warning.Text = "";
		admin_lb_warning.Style.Add("Padding", "5px");
		var pv = prep_vars();
		if (get_master_id() == "")
		{
			is_valid = false;
			admin_lb_warning.Text += "- You need to first define the part you are using.<br/>";
			return;
		}
		if (pv.int_location_id == 0)
		{
			is_valid = false;
			admin_lb_warning.Text += "- Please select a location to transfer from.<br/>";
		}
		if (pv.ext_location_id == 0)
		{
			is_valid = false;
			admin_lb_warning.Text += "- Please select a location to transfer to.<br/>";
		}
		if (pv.qty == 0)
		{
			is_valid = false;
			admin_lb_warning.Text += "- Please define a quantity to transfer.<br/>";
		}
		if (pv.int_location_id == pv.ext_location_id)
		{
			is_valid = false;
			admin_lb_warning.Text += "- You can't choose the same location as the location FROM / TO.<br/>";
		}
		if (pv.qty > pv.int_qty)
		{
			is_valid = false;
			admin_lb_warning.Text += "- You can't transfer a quantity greater than what exists.<br/>";
		}
		if (pv.qty < 0)
		{
			is_valid = false;
			admin_lb_warning.Text += "- You can't transfer a negative quantity.<br/>";
		}
		if (!is_valid)
		{
			return;
		}
		else
		{
			var il = new location();
			var location_from = new location(pv.int_location_id, WarehouseBusinessUnit.id, pv.master_id);
			var location_to = new location(pv.ext_location_id, WarehouseBusinessUnit.id, pv.master_id);
			il.section_id = 12;
			il.xfer((int)location_from.id, (int)location_to.id, pv.qty, current_user);
			update_quantity_labels(true);
			admin_int_location.Items.FindByValue(pv.int_location_id.ToString()).Selected = true;
			admin_ext_location.Items.FindByValue(pv.ext_location_id.ToString()).Selected = true;
			admin_txtQty.Text = "";

			status_update(pv.qty + " " + pv.master_id + " moved from " + location_from.this_master.name + " to " + location_to.this_master.name);

		}
	}
	protected void b_updatecb1_Click(object sender, EventArgs e)
	{
		var is_valid = true;
		admin_lb_warning.Text = "";
		admin_lb_warning.Style.Add("Padding", "5px");
		var pv = prep_vars();
		if (get_master_id() == "")
		{
			is_valid = false;
			admin_lb_warning.Text += "- You need to first define the part you are using.<br/>";
			return;
		}
		if (pv.qty == 0)
		{
			//		is_valid				= false;
			//		admin_lb_warning.Text	+= "- Please define a quantity to transfer.<br/>";
		}
		if (pv.int_location_id == 0)
		{
			is_valid = false;
			admin_lb_warning.Text += "- Please select a location to update.<br/>";
		}
		if (pv.qty < 0)
		{
			is_valid = false;
			admin_lb_warning.Text += "- You set to a negative quantity.<br/>";
		}
		if (!is_valid)
		{
			return;
		}
		else
		{
			var this_current_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  pv.master_id, WarehouseBusinessUnit.id } );

			var ib = new branch(pv.master_id, WarehouseBusinessUnit.id);
			var location_from = new location(pv.int_location_id, WarehouseBusinessUnit.id, pv.master_id);
			var was_qty = location_from.qty;
			var diff = location_from.qty - pv.qty;
			var used_qty = ib.onhand_qty == 0 ? pv.qty : ib.onhand_qty - was_qty + pv.qty;
			location_from.qty = pv.qty;
			location_from.member_id = current_user.id;
			location_from.alert_worthy = true;
			location_from.section_id = 11;
			ib.member_id = current_user.id;
			location_from.save();
			var this_type = was_qty > used_qty ? 3 : 2;
			location_from.update_branch(this_type, ib.dollar_balance, Math.Abs(diff), this_current_cost, ib);

			update_quantity_labels(true);
			if (admin_int_location.Items.Count > 0)
			{
				try
				{
					admin_int_location.Items.FindByValue(pv.int_location_id.ToString()).Selected = true;
				}
				catch { }
			}
			admin_ext_location.Items.FindByValue(pv.ext_location_id.ToString()).Selected = true;
			admin_txtQty.Text = "";
			status_update(pv.master_id + " updated qty from " + was_qty + " to " + pv.qty + " at " + location_from.this_master.name);
		}
	}
	protected void b_updatecb2_Click(object sender, EventArgs e)
	{
		var is_valid = true;
		admin_lb_warning.Text = "";
		admin_lb_warning.Style.Add("Padding", "5px");
		var pv = prep_vars();
		if (get_master_id() == "")
		{
			is_valid = false;
			admin_lb_warning.Text += "- You need to first define the part you are using.<br/>";
			return;
		}
		if (pv.qty == 0)
		{
			//		is_valid				= false;
			//		admin_lb_warning.Text	+= "- Please define a quantity to transfer.<br/>";
		}
		if (pv.ext_location_id == 0)
		{
			is_valid = false;
			admin_lb_warning.Text += "- Please select a location to update.<br/>";
		}
		if (pv.qty < 0)
		{
			is_valid = false;
			admin_lb_warning.Text += "- You set to a negative quantity.<br/>";
		}
		if (!is_valid)
		{
			return;
		}
		else
		{
			var this_current_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  pv.master_id, WarehouseBusinessUnit.id } );

			var ib = new branch(pv.master_id, WarehouseBusinessUnit.id);
			var location_to = new location(pv.ext_location_id, WarehouseBusinessUnit.id, pv.master_id);
			var was_qty = location_to.qty;
			var diff = location_to.qty - pv.qty;
			var used_qty = ib.onhand_qty == 0 ? pv.qty : ib.onhand_qty - was_qty + pv.qty;
			ib.member_id = current_user.id;
			location_to.qty = pv.qty;
			location_to.section_id = 11;
			location_to.alert_worthy = true;
			location_to.member_id = current_user.id;
			location_to.save();
			var this_type = was_qty > used_qty ? 3 : 2;
			location_to.update_branch(this_type, ib.dollar_balance, Math.Abs(diff), this_current_cost, ib);

			update_quantity_labels(true);
			if (admin_int_location.Items.Count > 0)
			{
				try
				{
					admin_int_location.Items.FindByValue(pv.int_location_id.ToString()).Selected = true;
				}
				catch { }
			}
			admin_ext_location.Items.FindByValue(pv.ext_location_id.ToString()).Selected = true;
			admin_txtQty.Text = "";
			status_update(pv.master_id + " updated qty from " + was_qty + " to " + pv.qty + " at " + location_to.this_master.name);
		}
	}
	public prepped_vars10 prep_vars()
	{
		var pv = new prepped_vars10();
		var master_id = 0;
		var int_location_id = 0;
		var po_lineitem_id = 0;
		var ext_location_id = 0;
		double int_qty = 0;
		double ext_qty = 0;
		double qty = 0;
		if (get_master_id() != "")
		{
			int.TryParse(get_master_id(), out master_id);
		}
		pv.master_id = master_id;
		switch (mv.ActiveViewIndex)
		{
			#region 0 - Admin
			case 0:
				if (admin_int_location.Text != "")
				{
					int.TryParse(admin_int_location.SelectedValue, out int_location_id);
				}
				pv.int_location_id = int_location_id;
				if (admin_ext_location.Text != "")
				{
					int.TryParse(admin_ext_location.SelectedValue, out ext_location_id);
				}
				pv.ext_location_id = ext_location_id;
				pv.str_int_qty = admin_lb_int_qty.Text.Contains("<b>") ? admin_lb_int_qty.Text.Substring(3, admin_lb_int_qty.Text.IndexOf("</b>") - 3).Replace(",", "") : "";
				if (pv.str_int_qty != "")
				{
					double.TryParse(pv.str_int_qty, out int_qty);
				}
				pv.int_qty = int_qty;
				pv.str_ext_qty = admin_lb_ext_qty.Text.Contains("<b>") ? admin_lb_ext_qty.Text.Substring(3, admin_lb_ext_qty.Text.IndexOf("</b>") - 3).Replace(",", "") : "";
				if (pv.str_ext_qty != "")
				{
					double.TryParse(pv.str_ext_qty, out ext_qty);
				}
				pv.ext_qty = ext_qty;
				pv.str_qty = admin_txtQty.Text;
				if (pv.str_qty != "")
				{
					double.TryParse(pv.str_qty, out qty);
				}
				pv.qty = qty;
				break;
			#endregion
			#region 1 - Transfer
			case 1:
				if (xfer_from_location.Text != "")
				{
					int.TryParse(xfer_from_location.SelectedValue, out int_location_id);
				}
				pv.int_location_id = int_location_id;
				if (xfer_to_location.Text != "")
				{
					int.TryParse(xfer_to_location.SelectedValue, out ext_location_id);
				}
				pv.ext_location_id = ext_location_id;
				pv.str_int_qty = xfer_lb_int_qty.Text.Contains("<b>") ? xfer_lb_int_qty.Text.Substring(3, xfer_lb_int_qty.Text.IndexOf("</b>") - 3).Replace(",", "") : "";
				if (pv.str_int_qty != "")
				{
					double.TryParse(pv.str_int_qty, out int_qty);
				}
				pv.int_qty = int_qty;
				pv.str_ext_qty = xfer_lb_ext_qty.Text.Contains("<b>") ? xfer_lb_ext_qty.Text.Substring(3, xfer_lb_ext_qty.Text.IndexOf("</b>") - 3).Replace(",", "") : "";
				if (pv.str_ext_qty != "")
				{
					double.TryParse(pv.str_ext_qty, out ext_qty);
				}
				pv.ext_qty = ext_qty;
				pv.str_qty = xfer_txtQty.Text;
				if (pv.str_qty != "")
				{
					double.TryParse(pv.str_qty, out qty);
				}
				pv.qty = qty;
				break;
			#endregion
			#region 2 - Purchase Order
			case 2:
				if (po_lineitem.Text != "")
				{
					int.TryParse(po_lineitem.SelectedValue, out po_lineitem_id);
				}
				pv.po_lineitem_id = po_lineitem_id;
				if (po_int_location.Text != "")
				{
					int.TryParse(po_int_location.SelectedValue, out int_location_id);
				}
				pv.int_location_id = int_location_id;
				if (po_ext_location.Text != "")
				{
					int.TryParse(po_ext_location.SelectedValue, out ext_location_id);
				}
				pv.ext_location_id = ext_location_id;
				pv.str_int_qty = po_lb_int_qty.Text.Contains("<b>") ? po_lb_int_qty.Text.Substring(3, po_lb_int_qty.Text.IndexOf("</b>") - 3).Replace(",", "") : "";
				if (pv.str_int_qty != "")
				{
					double.TryParse(pv.str_int_qty, out int_qty);
				}
				pv.int_qty = int_qty;
				pv.str_ext_qty = po_lb_ext_qty.Text.Contains("<b>") ? po_lb_ext_qty.Text.Substring(3, po_lb_ext_qty.Text.IndexOf("</b>") - 3).Replace(",", "") : "";
				if (pv.str_ext_qty != "")
				{
					double.TryParse(pv.str_ext_qty, out ext_qty);
				}
				pv.ext_qty = ext_qty;
				pv.str_qty = po_txtQty.Text;
				if (pv.str_qty != "")
				{
					double.TryParse(pv.str_qty, out qty);
				}
				pv.qty = qty;
				break;
			#endregion
			#region 4 - Request
			case 4:
				pv.str_qty = txt_request_qty.Text;
				if (pv.str_qty != "")
				{
					double.TryParse(pv.str_qty, out qty);
				}
				pv.qty = qty;
				break;
			#endregion
			#region 5 - Commit
			case 5:

				if (ddl_commit_from_location.Text != "")
				{
					int.TryParse(ddl_commit_from_location.SelectedValue, out int_location_id);
				}
				pv.int_location_id = int_location_id;
				pv.str_int_qty = xfer_lb_int_qty0.Text.Contains("<b>") ? xfer_lb_int_qty0.Text.Substring(3, xfer_lb_int_qty0.Text.IndexOf("</b>") - 3).Replace(",", "") : "";
				if (pv.str_int_qty != "")
				{
					double.TryParse(pv.str_int_qty, out int_qty);
				}
				pv.int_qty = int_qty;

				pv.str_qty = txt_commit_qty.Text;
				if (pv.str_qty != "")
				{
					double.TryParse(pv.str_qty, out qty);
				}
				pv.qty = qty;
				break;
			#endregion
			#region 6 - Return
			case 6:
				if (ddl_return_to_location.Text != "")
				{
					int.TryParse(ddl_return_to_location.SelectedValue, out ext_location_id);
				}
				pv.ext_location_id = ext_location_id;
				pv.str_ext_qty = xfer_lb_ext_qty0.Text.Contains("<b>") ? xfer_lb_ext_qty0.Text.Substring(3, xfer_lb_ext_qty0.Text.IndexOf("</b>") - 3).Replace(",", "") : "";
				if (pv.str_ext_qty != "")
				{
					double.TryParse(pv.str_ext_qty, out ext_qty);
				}
				pv.ext_qty = ext_qty;
				pv.str_qty = txt_return_qty.Text;
				if (pv.str_qty != "")
				{
					double.TryParse(pv.str_qty, out qty);
				}
				pv.qty = qty;

				break;
			#endregion

		}
		return pv;
	}
	protected void btnRequire_Click(object sender, EventArgs e)
	{
		var is_valid = true;
		lbl_error_request.Text = "";
		lbl_error_request.Style.Add("Padding", "5px");
		var pv = prep_vars();
		if (lb_wo_no.Text == "")
		{
			lbl_error_request.Text += "- You need to first define the work order.<br/>";
			return;
		}
		if (get_master_id() == "")
		{
			lbl_error_request.Text += "- You need to first define the part you are using.<br/>";
			return;
		}
		if (pv.qty == 0)
		{
			is_valid = false;
			lbl_error_request.Text += "- Please define a quantity.<br/>";
		}
		if (!is_valid)
		{
			return;
		}
		else
		{
			try
			{
				global_pv = pv;
				validate_entries("req");
				update_qty_nos_in_masterid_lbl();
				//			update_quantity_labels(true);
				txt_request_qty.Text = "";
				tb_777_description.Text = "";
				gv_shopping.DataBind();
			}
			catch
			{
				throw;
			}
		}
	}
	protected void btnCmt_Click(object sender, EventArgs e)
	{
		var is_valid = true;
		lbl_error_commit.Text = "";
		lbl_error_commit.Style.Add("Padding", "5px");
		var pv = prep_vars();
		if (lb_wo_no.Text == "")
		{
			lbl_error_commit.Text += "- You need to first define the work order to commit to.<br/>";
			return;
		}
		var wo = new NeWOProg(Convert.ToInt32(lb_wo_no.Text));
		if (get_master_id() == "")
		{
			lbl_error_commit.Text += "- You need to first define the part you are using.<br/>";
			return;
		}
		if (wo.Status != OpsWOStatus.Open)
		{
			lbl_error_commit.Text += "- This is not an <i>Open</i> work order<br/>";
			return;
		}
		//if (pv.int_qty <= 0) // int_qty is the FROM qty
		//{
		//	is_valid = false;
		//	xfer_lb_warning.Text += "- The location you're trying to commit from, doesn't have stock.<br/>";
		//}
		if (pv.qty == 0)
		{
			is_valid = false;
			lbl_error_commit.Text += "- Please define a quantity to commit.<br/>";
		}
		if (pv.qty < 0)
		{
			is_valid = false;
			lbl_error_commit.Text += "- You can't commit a negative quantity.<br/>";
		}
		if (pv.int_location_id == 0)
		{
			is_valid = false;
			lbl_error_commit.Text += "- Please select a location to commit from.<br/>";
		}
		var inv = new inventory();
		inv.Load(get_master_id(), WarehouseBusinessUnit.id);
		if (inv.cost_price_branch == 0)
		{
			is_valid = false;
			lbl_error_commit.Text += "- The selected part doesn't have a cost, please contact your purchaser.<br/>";
		}
		//if (ext_loc.type_id != 2)
		//{
		//	is_valid = false;
		//	xfer_lb_warning.Text += "- Please select an external location to commit from.<br/>";
		//}
		if (!is_valid)
		{
			xfer_from_location.DataBind();
			xfer_to_location.DataBind();
			return;
		}
		else
		{
			try
			{
				var ext_loc = new location_master(pv.int_location_id);
				global_pv = pv;
				validate_entries("com");
				update_quantity_labels(true);
				update_qty_nos_in_masterid_lbl();
				populate_datasources();
				txt_commit_qty.Text = "";
				ddl_commit_from_location.SelectedValue = pv.int_location_id.ToString();
				gv_shopping.DataBind();
			}
			catch
			{
				throw;
			}
		}
	}
	//protected void xfer_b_emptytobin_Click(object sender, EventArgs e)
	protected void xfer_b_emptytobin_Click()
	{
		emptybin_emailer(false);
	}
	//protected void xfer_b_emptyfrombin_Click(object sender, EventArgs e)
	protected void xfer_b_emptyfrombin_Click()
	{
		emptybin_emailer(true);
	}
	private void emptybin_emailer(bool is_from)
	{
		var location_id = 0;
		if (is_from)
		{
			int.TryParse(xfer_from_location.SelectedValue, out location_id);
		}
		else
		{
			int.TryParse(xfer_to_location.SelectedValue, out location_id);
		}
		var master_id = 0;
		int.TryParse(get_master_id(), out master_id);
		if (location_id > 0 && master_id > 0)
		{
			var il = new location(location_id, WarehouseBusinessUnit.id, master_id);
			var ilm = new location_master(il.location_master_id);
			var em = new NeEMail();
			em.From = "administrator@" + Toolbox.app_setting("DomainForEmail");

			var temp_name = "";
			var purchaser_id = (int)Toolbox.doSQL_int(@"SELECT IFNULL(MIN(member_id), 0) FROM member WHERE member_membertype_id = 9 AND member_status = 'active' AND business_unit_id =@v0 " , WorkingBusinessUnit.id);
			var shipper_id = (int)Toolbox.doSQL_int(@"SELECT IFNULL(MIN(member_id), 0) FROM member WHERE member_membertype_id = 28 AND member_status = 'active'AND business_unit_id = @v0" , WorkingBusinessUnit.id);
			var service_coord_id = (int)Toolbox.doSQL_int(@"SELECT IFNULL(MIN(member_id), 0) FROM memberWHERE member_membertype_id = 34 AND member_status = 'active' AND business_unit_id =@v0 ", WorkingBusinessUnit.id);


			if (shipper_id != 0)
			{
				var shipper = new NeMember(shipper_id);
				if (shipper.NEEmail != "" && !shipper.NEEmail.Contains("nomail"))
				{
					em.To = shipper.NEEmail;
					temp_name = shipper.FullName;
					if (purchaser_id != 0)
					{
						var purchaser = new NeMember(purchaser_id);
						em.CC = purchaser.NEEmail;
					}
					else if (service_coord_id != 0)
					{
						var sc = new NeMember(service_coord_id);
						em.CC = sc.NEEmail;
					}
				}
				else
				{
					if (purchaser_id != 0)
					{
						var purchaser = new NeMember(purchaser_id);
						em.To = purchaser.NEEmail;
						temp_name = purchaser.FullName;
					}
				}
			}
			else if (purchaser_id != 0)
			{
				var purchaser = new NeMember(purchaser_id);
				em.To = purchaser.NEEmail;
				temp_name = purchaser.FullName;
			}
			else if (service_coord_id != 0)
			{
				var sc = new NeMember(service_coord_id);
				em.CC = sc.NEEmail == "" || sc.NEEmail.Contains("nomail") ? current_user.business_unit.branch_manager.NEEmail : sc.NEEmail;
			}
			else
			{
				em.To = current_user.business_unit.branch_manager.NEEmail;
				temp_name = current_user.business_unit.branch_manager.FullName;
			}




			em.Subject = "Bin with Invalid Quantity - Master ID: " + master_id + " in Location: " + ilm.name;
			em.isHTML = true;
			var wo_n = lbbvwo.Text == "" ? "N/A" : lbbvwo.Text;
			em.Body = string.Format(@"
<table cellpadding='3' cellspacing='0' style='font-family:arial;font-size:12px;'>
	<tr>
		<td width='150'><b>Reportee: </b></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td><b>Master ID: </b></td>
		<td>{1}</td>
	</tr>
	<tr>
		<td><b>Location: </b></td>
		<td>{2}</td>
	</tr>
	<tr>
		<td><b>Work Order (if applicable): </b></td>
		<td>{4}</td>
	</tr>
	<tr>
		<td><b>Current Recorded Qty: </b></td>
		<td>{3}</td>
	</tr>
</table>
", current_user.FullName, master_id, ilm.name, il.qty, wo_n);

			em.Send();
			status_update("Invalid Quantity notification sent to " + temp_name);
		}
		else
		{
			if (master_id == 0)
			{
				xfer_lb_warning.Text += "- Please enter a part #.<br/>";
			}
			if (location_id == 0)
			{
				xfer_lb_warning.Text += "- Please select a location.<br/>";
			}
		}
	}
	protected void bt_signout_Click(object sender, EventArgs e)
	{
		Response.Redirect("/mobile/index.aspx?a=logoff");
	}


	protected void pop_search_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		if (mv.ActiveViewIndex == 8)
		{
			rdo_search.Items[1].Enabled = false;
			rdo_search.SelectedIndex = 0;
		}
		if (pc_search.ActiveTabIndex == 0)
		{
			//	txt_search.ClientVisible = false;
			var dt = _tools.getSQL_datatable(@"SELECT woprog.woprog_id id, TRIM(LEADING '0' FROM woprog_bvwo) num, concat( name, '-', woprog_customername, '-', woprog_description ) _name FROM woprog INNER JOIN business_unit ON woprog_company_id = business_unit.id INNER JOIN appointments on woprog.WOProg_Address_ID = appointments.Location and appointments.member_id =@v0 and date(appointments.StartDate) = CURDATE()   WHERE woprog_status = 'open' and woprog.WOProg_Hold = 0 and woprog.WOProg_Associate_WOProg_ID = 0 order by id desc", new object[] { current_user.id});
			Session["gv_search"] = dt;
			gv_search0.DataSource = Session["gv_search"];
			gv_search0.DataBind();
		}
		else
		{
			if (rdo_search.SelectedIndex < 0)
			{
				rdo_search.SelectedIndex = 0;
			}
			txt_search.Focus();
			txt_search.ClientVisible = true;
		}
	}
	protected void txt_search_TextChanged(object sender, EventArgs e)
	{
		var dt = new DataTable();
		if (rdo_search.SelectedValue == "wo")
		{
			dt = Toolbox.doSQL_dt(@" SELECT woprog_id id, TRIM(LEADING '0' from woprog_bvwo) num, CONCAT(name,'-',woprog_customername,'-',woprog_description) _name FROM woprog a INNER JOIN business_unit b on a.business_unit_id = b.id WHERE a.woprog_status = 'open' AND ( a.woprog_bvwo LIKE CONCAT'%',@v1,'%') or a.woprog_customername LIKE CONCAT'%',@v1,'%') or a.woprog_description LIKE CONCAT'%',@v1,'%')) order by a.woprog_customername, a.woprog_description",
				new object[] {  WorkingBusinessUnit.id, txt_search.Text } );
		}
		else if (rdo_search.SelectedValue == "mjt")
		{
			dt = Toolbox.doSQL_dt(@" SELECT a.woprog_id id, TRIM(LEADING '0' FROM a.woprog_bvwo) num, CONCAT(b.name,'-',a.woprog_customername,'-',a.woprog_description) _name 
FROM woprog a INNER JOIN business_unit b ON a.business_unit_id = b.id INNER JOIN appointments c on a.woprog_address_id = c.location AND c.member_id = @v0  AND DATE(c.startdate) = CURDATE()
WHERE a.woprog_status = 'open' and a.woprog_hold = 0 and a.woprog_associate_woprog_id = 0 ORDER BY id DESC", new object[] {  current_user.id } );
		}
		else
		{
			var where = "";
			var search_ = txt_search.Text.Split(' ');
			if (mv.ActiveViewIndex != 8)
			{
				foreach (var s in search_)
				{
					if (s != "")
					{
						where += " CONCAT(' ',IF(c.country = 'USA', b.desc_full_usa, b.desc_full_cdn)) like '% " + Toolbox.AddSlashes(s) + "%' and ";
					}
				}
				if (where.EndsWith(" and "))
				{
					where = " a.master_id = '" + Toolbox.AddSlashes(txt_search.Text) + "' or " + where.Substring(0, where.Length - 4);
				}
			}
			else
			{
				foreach (var s in search_)
				{
					if (s != "")
					{
						where += " CONCAT(' ', FULL_PART_DESCRIPTION(a.wo_detail_current_master_id, true, d.country)) LIKE '% " + s + "%' and ";
					}
				}
				if (where.EndsWith(" and "))
				{
					where = " wo_detail_current_master_id = '" + Toolbox.AddSlashes(txt_search.Text) + "' OR " + where.Substring(0, where.Length - 4);
				}
			}

			if (mv.ActiveViewIndex != 8 && where != "")
			{
				dt = Toolbox.doSQL_dt(@" SELECT a.master_id id, a.master_id num, IF(c.country = 'USA', b.desc_full_usa, b.desc_full_cdn) _name 
FROM inventory_price a INNER JOIN inventory_description b on b.master_id = a.master_id INNER JOIN business_unit c on a.business_unit_id = c.id 
where a.vendor_code LIKE @v1 AND a.business_unit_id = @v0  UNION ( SELECT a.master_id id, a.master_id num, IF(c.country = 'USA', " +
				                      "b.desc_full_usa, b.desc_full_cdn) _name FROM inventory_item_master a LEFT JOIN inventory_description b ON a.master_id = b.master_id LEFT JOIN business_unit c" +
				                      " ON c.id = @v0  WHERE " + where + " ORDER BY ( SELECT count(log.id) FROM log " +
				                      "WHERE log.associated_alt_table_id = a.master_id AND log.dt > DATE_SUB(CURDATE(), INTERVAL 1 YEAR) AND log.action_id = 3 ) DESC ) ", 
									  new object[] {  WarehouseBusinessUnit.id , "%" + txt_search.Text + "%" } );
			}
			else if (where != "")
			{
				dt = Toolbox.doSQL_dt(string.Format(@" SELECT a.wo_detail_current_master_id id, a.wo_detail_current_master_id num,
CONCAT(IF(d.country = 'USA', b.desc_full_usa, b.desc_full_cdn), IF(LENGTH(TRIM(URLDECODE(a.wo_detail_current_notes))) > 0,
CONCAT(' \n\n****************\nNOTES: ',URLDECODE(a.wo_detail_current_notes),'\n****************'), '')) _name 
FROM wo_detail_current a LEFT JOIN inventory_description b ON a.wo_detail_master_id = b.master_id
LEFT JOIN inventory_location f ON a.wo_detail_current_master_id = f.master_id AND f.business_unit_id = @v1
LEFT JOIN inventory_location_master c ON f.location_master_id = c.id LEFT JOIN business_unit d ON d.id = @v1
LEFT JOIN inventory_item_master e ON a.wo_detail_current_master_id = e.master_id  WHERE wo_detail_current_master_id IS NOT NULL AND {0} 
AND a.wo_detail_current_woprog_id =@v0  AND a.wo_detail_current_master_id < '99000' AND a.wo_detail_current_billtypeid NOT IN (3, 5, 6) 
AND a.wo_detail_current_master_id != '2139' AND c.type_id = 1 GROUP BY a.wo_detail_current_rec_no ORDER BY c.name,a.wo_detail_current_rec_no", where), new object[] { lb_wo_no.Text, WarehouseBusinessUnit.id });
			}
		}

		//	lst_search.Rows = dt.Rows.Count;
		//lst_search.DataBind();
		Session["gv_search"] = dt;
		gv_search.DataSource = dt;
		gv_search.DataBind();

	}


	protected void gv_search_SelectedIndexChanged(object sender, EventArgs e)
	{
		var gv = (GridView)sender;
		if (gv.SelectedIndex >= 0)
		{
			gv.DataSource = Session["gv_search"];
			gv.DataBind();
			switch (gv.ID)
			{
				case "gv_search":
					if (rdo_search.SelectedValue == "wo")
					{

						scanbox.Text = "002-" + gv.SelectedPersistedDataKey.Value;
						new_scan(scanbox, new EventArgs());
					}
					else
					{
						scanbox.Text = "001-" + gv.SelectedPersistedDataKey.Value;
						new_scan(scanbox, new EventArgs());
					}
					txt_search.Text = "";
					//		gv_search.DataSource = "";
					//		gv_search.DataBind();

					pop_search.ShowOnPageLoad = false;
					break;
				case "gv_history":
					scanbox.Text = "001-" + gv.SelectedPersistedDataKey.Value;
					new_scan(scanbox, new EventArgs());

					break;
				case "gv_shopping":
					scanbox.Text = "001-" + gv.SelectedPersistedDataKey.Value;
					new_scan(scanbox, new EventArgs());

					break;
				case "gv_search0":
					scanbox.Text = "002-" + gv.SelectedPersistedDataKey.Value;
					new_scan(scanbox, new EventArgs());
					pop_search.ShowOnPageLoad = false;
					break;

			}

		}
	}
	//protected void xfer_b_emptytobin_Click_commit(object sender, EventArgs e)
	protected void xfer_b_emptytobin_Click_commit()
	{

		var location_id = 0;

		int.TryParse(ddl_commit_from_location.SelectedValue, out location_id);

		var master_id = 0;
		int.TryParse(get_master_id(), out master_id);
		if (location_id > 0 && master_id > 0)
		{
			var il = new location(location_id, current_user.business_unit_id, master_id);
			var ilm = new location_master(il.location_master_id);
			var em = new NeEMail();
			em.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
			var purchaser_id = (int)Toolbox.doSQL_int(@"SELECT IFNULL(MIN(member_id), 0) FROM member 
WHERE member_membertype_id = 9 AND member_status = 'active' AND business_unit_id = @v0" , WorkingBusinessUnit.id);
			var purchaser = new NeMember(purchaser_id);
			em.To = purchaser.NEEmail == "" ? "Mhyde@" + Toolbox.app_setting("DomainForEmail") : purchaser.NEEmail;
			if (current_user.business_unit_id == 1)
			{
				em.CC = "oakshipper@" + Toolbox.app_setting("DomainForEmail");
			}
			em.Subject = "Bin with Invalid Quantity - Master ID: " + master_id + " in Location: " + ilm.name;
			em.isHTML = true;
			var wo_n = lbbvwo.Text == "" ? "N/A" : lbbvwo.Text;
			em.Body = string.Format(@"
<table cellpadding='3' cellspacing='0' style='font-family:arial;font-size:12px;'>
	<tr>
		<td width='150'><b>Reportee: </b></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td><b>Master ID: </b></td>
		<td>{1}</td>
	</tr>
	<tr>
		<td><b>Location: </b></td>
		<td>{2}</td>
	</tr>
	<tr>
		<td><b>Work Order (if applicable): </b></td>
		<td>{4}</td>
	</tr>
	<tr>
		<td><b>Current Recorded Qty: </b></td>
		<td>{3}</td>
	</tr>
</table>
", current_user.FullName, master_id, ilm.name, il.qty, wo_n);
			//		em.Bcc = "aketelaars@newelectric.com";
			em.Send();
			status_update("Invalid Quantity notification sent to " + purchaser.FullName);
		}
		else
		{
			if (master_id == 0)
			{
				xfer_lb_warning.Text += "- Please enter a part #.<br/>";
			}
			if (location_id == 0)
			{
				xfer_lb_warning.Text += "- Please select a location.<br/>";
			}
		}


	}
	//protected void xfer_b_emptytobin_Click_return(object sender, EventArgs e)
	protected void xfer_b_emptytobin_Click_return()
	{
		var location_id = 0;

		int.TryParse(ddl_return_to_location.SelectedValue, out location_id);

		var master_id = 0;
		int.TryParse(get_master_id(), out master_id);
		if (location_id > 0 && master_id > 0)
		{
			var il = new location(location_id, WarehouseBusinessUnit.id, master_id);
			var ilm = new location_master(il.location_master_id);
			var em = new NeEMail();
			em.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
			var purchaser_id = (int)Toolbox.doSQL_int(@"SELECT IFNULL(MIN(member_id), 0) FROM member WHERE member_membertype_id = 9 AND member_status = 'active' AND business_unit_id = @v0", WorkingBusinessUnit.id);
			var purchaser = new NeMember(purchaser_id);
			em.To = purchaser.NEEmail == "" ? "Mhyde@newelectric.com" : purchaser.NEEmail;
			if (current_user.business_unit_id == 1)
			{
				em.CC = "oakshipper@" + Toolbox.app_setting("DomainForEmail");
			}
			em.Subject = "Bin with Invalid Quantity - Master ID: " + master_id + " in Location: " + ilm.name;
			em.isHTML = true;
			var wo_n = lbbvwo.Text == "" ? "N/A" : lbbvwo.Text;
			em.Body = string.Format(@"
<table cellpadding='3' cellspacing='0' style='font-family:arial;font-size:12px;'>
	<tr>
		<td width='150'><b>Reportee: </b></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td><b>Master ID: </b></td>
		<td>{1}</td>
	</tr>
	<tr>
		<td><b>Location: </b></td>
		<td>{2}</td>
	</tr>
	<tr>
		<td><b>Work Order (if applicable): </b></td>
		<td>{4}</td>
	</tr>
	<tr>
		<td><b>Current Recorded Qty: </b></td>
		<td>{3}</td>
	</tr>
</table>
", current_user.FullName, master_id, ilm.name, il.qty, wo_n);
			//		em.Bcc = "aketelaars@newelectric.com";
			em.Send();
			status_update("Invalid Quantity notification sent to " + purchaser.FullName);
		}
		else
		{
			if (master_id == 0)
			{
				xfer_lb_warning.Text += "- Please enter a part #.<br/>";
			}
			if (location_id == 0)
			{
				xfer_lb_warning.Text += "- Please select a location.<br/>";
			}
		}
	}

	protected void gv_incorrect_qtys_SelectedIndexChanged(object sender, EventArgs e)
	{
		var is_valid = true;
		
		var id = gv_incorrect_qtys.SelectedPersistedDataKey.Value;
		var iil = new incorrect_levels();
		if (id != null)
		{
			iil = new incorrect_levels(Convert.ToInt32(id));
		}
		var tb = (TextBox)gv_incorrect_qtys.Rows[gv_incorrect_qtys.SelectedRow.RowIndex].FindControl("tb_loc_fix");

		var master_id = iil.master_id;
		var location_master_id = iil.location_id;
		var i = new inventory();
		i.Load(iil.master_id, WarehouseBusinessUnit.id);
		var ilm = new location_master();
		var il = new location();

		if (location_master_id > 0)
		{
			ilm = new location_master(location_master_id);
		}
		if (master_id > 0)
		{
			il = new location(location_master_id,WarehouseBusinessUnit.id, master_id);
		}
		double qty = 0;
		var can_convert = double.TryParse(tb.Text, out qty);
		if (!can_convert)
		{
			xfer_lb_warning.Text = "Invalid Quantity Supplied.";
			return;
		}
		else if (qty < 0)
		{
			xfer_lb_warning.Text = "Negative Quantity Supplied.";
			return;
		}
		if (il.id > 0 && ilm.id > 0 && master_id > 0 && qty >= 0)
		{
			var ib = new branch(il.master_id, il.business_unit_id);
			var prev_qty = il.qty;
			var this_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  il.master_id, il.business_unit_id } );
			il.log_is_manual = true;
			il.member_id = current_user.id;
			il.alert_worthy = true;
			var diff = il.qty - qty;
			il.qty = qty;
			il.section_id = 1;
			var this_type = prev_qty > qty ? 3 : 2;
			var pre_ib = new branch(il.master_id, il.business_unit_id);
			il.save();
			il.update_branch(this_type, ib.dollar_balance, diff, this_cost, ib);
			var post_ib = new branch(il.master_id, il.business_unit_id);

			il = new location(location_master_id, WarehouseBusinessUnit.id, master_id);

			iil.approve(iil.id, current_user.id, qty);
			gv_incorrect_qtys.DataBind();

			if (current_user.id != il.member_id)
			{
				var email = new NeEMail();
				email.To = new NeMember(Convert.ToInt32(iil.member_id)).NEEmail;
				email.Subject = "Location Qty Update";
				email.Body = "<div style='font-family: arial;'>Part: " + iil.master_id + " " + i.description_full + " in location: " + il.this_master.name + " had its qty updated from " + prev_qty + " to " + qty + ".  You are being notified because your name is on record for reporting the incorrect qty for this part at this location.</div>";
				email.isHTML = true;
				email.Send();
			}

		}



	}
	protected void btn_send_invalid_qty_Click(object sender, EventArgs e)
	{
var master_id = 0;
var location_id = 0;
double qty = 0;
		try
		{
			
			int.TryParse(get_master_id(), out master_id);
			double.TryParse(txt_pop_qty.Text, out qty);
			switch (txt_final_call.Text)
			{
				case "from_return":
					int.TryParse(ddl_return_to_location.SelectedValue, out location_id);
					break;
				case "from_commit":
					int.TryParse(ddl_commit_from_location.SelectedValue, out location_id);
					break;
				case "from_to_bin":
					int.TryParse(xfer_to_location.SelectedValue, out location_id);
					break;
				case "from_from_bin":
					int.TryParse(xfer_from_location.SelectedValue, out location_id);
					break;
			}
		}
		catch
		{
		
			pop_invalid_qty.ShowOnPageLoad = false;
			return;
		}
		if (master_id != 0 && location_id != 0 && txt_pop_qty.Text != "")
		{
			var il = new location(location_id, WarehouseBusinessUnit.id, master_id);
			var ilm = new location_master(il.location_master_id);
			var em = new NeEMail();
			em.From = "administrator@" + Toolbox.app_setting("DomainForEmail");


			em.To = WorkingBusinessUnit.purchaser.NEEmail == "" ? WorkingBusinessUnit.branch_manager.NEEmail : WorkingBusinessUnit.purchaser.NEEmail;
			if (WorkingBusinessUnit.id == 1)
			{
				em.CC = "oakshipper@" + Toolbox.app_setting("DomainForEmail");
			}
			em.Subject = "Bin with Invalid Quantity - Master ID: " + master_id + " in Location: " + ilm.name;
			em.isHTML = true;
			var wo_n = lbbvwo.Text == "" ? "N/A" : lbbvwo.Text;
			em.Body = string.Format(@"
<table cellpadding='3' cellspacing='0' style='font-family:arial;font-size:12px;'>
	<tr>
		<td width='150'><b>Reportee: </b></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td><b>Master ID: </b></td>
		<td>{1}</td>
	</tr>
	<tr>
		<td><b>Location: </b></td>
		<td>{2}</td>
	</tr>
	<tr>
		<td><b>Work Order (if applicable): </b></td>
		<td>{4}</td>
	</tr>
	<tr>
		<td><b>Current System Qty: </b></td>
		<td>{3}</td>
	</tr>
<tr>
		<td><b>Reported Qty: </b></td>
		<td>{5}</td>
	</tr>
</table>
", current_user.FullName, master_id, ilm.name, il.qty, wo_n, txt_pop_qty.Text);
			//		em.Bcc = "aketelaars@newelectric.com";
					em.Send();
			status_update("Invalid Quantity notification sent to " + WorkingBusinessUnit.purchaser.FullName);
			var iil = new incorrect_levels();
			iil.master_id = master_id;
			iil.location_id = location_id;
			iil.incorrect_qty = il.qty;
			iil.reported_qty = Convert.ToDouble(txt_pop_qty.Text);
			iil.member_id = current_user.id;
			iil.date = System.DateTime.Today;
			iil.id = 0;
			iil.save();



		}
		else
		{
			
			

		}
pop_invalid_qty.ShowOnPageLoad = false;
			
	}
}
public class prepped_vars10
	{
	private int _po_lineitem_id;
	private int _int_location_id;
	private int _ext_location_id;
	private double _int_qty;
	private string _str_int_qty;
	private string _str_ext_qty;
	private double _ext_qty;
	private string _str_qty;
	private double _qty;
	private int _master_id;
	private int _ID;

	public int po_lineitem_id { get {return _ID;} set{_ID = value;} }
	public int int_location_id { get {return _int_location_id;} set{_int_location_id = value;} }
	public int ext_location_id { get {return _ext_location_id;} set{_ext_location_id = value;} }
	public double int_qty { get {return _int_qty;} set{_int_qty = value;} }
	public string str_int_qty { get {return _str_int_qty;} set{_str_int_qty = value;} }
	public string str_ext_qty { get {return _str_ext_qty;} set{_str_ext_qty = value;} }
	public double ext_qty { get {return _ext_qty;} set{_ext_qty = value;} }
	public string str_qty { get {return _str_qty;} set{_str_qty = value;} }
	public double qty { get {return _qty;} set{_qty = value;} }
	public int master_id { get {return _master_id;} set{_master_id = value;} }
	}
