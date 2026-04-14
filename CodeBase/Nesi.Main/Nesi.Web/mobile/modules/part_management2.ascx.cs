using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Web;
using System.Linq;
using NESI.Common.Models;
//using nesi.bv;
using nesi.core;

public partial class mobile_modules_part_management2 : UserControl
	{
	private NeMember current_user;

	NameValueCollection _q;
	private NeBusinessUnit WorkingBusinessUnit { get; set; }
	private NeBusinessUnit WarehouseBusinessUnit { get; set; }

	branch_options branchOptionsObject;
	
	protected override void OnInit(EventArgs e)
		{
		InitializeComponent();
		base.OnInit(e);
		}
   
	private void InitializeComponent()
		{
		try
			{
			current_user = Toolbox.do_handle_authentication(OpsPage.PartManagement);
			WorkingBusinessUnit                   = new NeBusinessUnit(current_user.business_unit_id);
			WarehouseBusinessUnit                 = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
			Session["working_business_unit_id"]   = WorkingBusinessUnit.id;
			Session["warehouse_business_unit_id"] = WarehouseBusinessUnit.id;
			}
		catch
			{
			Response.Redirect("/#/signout");
			}
		this.PreRender += Load;
		}
	private void Load(object s, EventArgs e)
		{
		Toolbox.do_dont_cache_page(Response);
		_q = Request.QueryString;


		Session["memberid"] = current_user.id.ToString();
		hdn_member_id.Value = current_user.id.ToString();

		branchOptionsObject = new branch_options(WarehouseBusinessUnit.id);

		//       is_purchaser = true;
		btn_fix_qty_save.Visible = current_user.AuthenticatedForPrivilege(OpsPrivilege.PurchaserAccess) && branchOptionsObject.stk_adj;
		tab_incorrect.Visible    = current_user.AuthenticatedForPrivilege(OpsPrivilege.PurchaserAccess) && branchOptionsObject.stk_adj;
		tab_annual.Visible       = false;

		xfer_lb_notice.Text = "";
		xfer_lb_warning.Text = "";
		var click_wo = false;
		if (!IsPostBack)
			{
			//		uc_workorder.Visible = false;
			Session["fix_master_id"] = null;
			Session["fix_location_id"] = null;
			ddl_other_trucks.ClientVisible = false;
			ddl_other_wo.ClientVisible = false;
			ddl_company.DataBind();
			ddl_company.Value = WorkingBusinessUnit.id;

			branchOptionsObject = new branch_options(WarehouseBusinessUnit.id);

			rpt_annual.Visible = branchOptionsObject.annual_count_on;


			Session["mobile_rpt_wo_progid"] = null;


			if (!string.IsNullOrEmpty(_q["woprog_id"]))
				{
				int woprog_id;
				int.TryParse(_q["woprog_id"], out woprog_id);
				Session["working_woprog_id"] = woprog_id.ToString();
				ddl_wo.Value = woprog_id;
				click_wo = true;
				}
			}
		else
			{

			if (mv.GetActiveView() == vw_wo)
				{
				load_workorder();
				}

			}
		set_selectors();
		branchOptionsObject = new branch_options(WarehouseBusinessUnit.id);
		ScriptManager.RegisterStartupScript(up, up.GetType(), "set_tabs", "set_tabs();", true);
		if (click_wo)
			{
			ScriptManager.RegisterStartupScript(up, up.GetType(), "click_wo", "$('#" + tab_wo.ClientID + "').click();", true);
			}
		ddl_wo.DataBind();
		ddl_trucks.DataBind();
		}
	private void handleTabs()
		{
		tab_wo.Enabled = ddl_wo.Value != null && mv.GetActiveView() != vw_wo;
		tab_status.Enabled = ddl_wo.Value != null &&  mv.GetActiveView() != vw_status;
		tab_truck.Enabled = ddl_wo.Value != null &&  mv.GetActiveView() != vw_truck;
		tab_pull.Enabled = ddl_wo.Value != null &&  mv.GetActiveView() != vw_pull;
		tab_return.Enabled = ddl_wo.Value != null &&  mv.GetActiveView() != vw_return;
		tab_history.Enabled = ddl_wo.Value != null &&  mv.GetActiveView() != vw_history;
		tab_search.Enabled = ddl_wo.Value != null &&  mv.GetActiveView() != vw_search;
		 
		}



	protected void mv_ActiveViewChanged(object sender, EventArgs e)
		{

		hdn_view_id.Value = mv.ActiveViewIndex.ToString();
		ddl_trucks.DataBind();
		}

	private void load_workorder()
		{

			if (Session["working_woprog_id"] != null && Session["working_woprog_id"].ToString() != "" && Session["working_woprog_id"].ToString() != "0")
                {
				ddl_other_wo.DataBind();
                }
        }

	protected void tab_wo_Click(object sender, EventArgs e)
		{
		mv.ActiveViewIndex = 0;
		if_shoppingcart.Visible = false;
		load_wo_repeater("", true);
		rpt_wo.Visible = true;
		load_workorder();
		set_selectors();
		}
	protected void tab_truck_Click(object sender, EventArgs e)
		{
		mv.ActiveViewIndex = 1;
		load_truck_repeater("", true);
		rpt_truck.Visible = true;
		//	uc_workorder.Visible		 = false;
		set_selectors();
		}
	protected void tab_return_Click(object sender, EventArgs e)
		{
		mv.ActiveViewIndex = 2;
		load_return_repeater("", true);
		//	uc_workorder.Visible		 = false;
		set_selectors();
		}

	protected void tab_pull_Click(object sender, EventArgs e)
		{
		mv.ActiveViewIndex = 3;
		load_pull_repeater("", true);
		//	uc_workorder.Visible		 = false;
		set_selectors();
		}
	protected void tab_incorrect_Click(object sender, EventArgs e)
		{
		mv.ActiveViewIndex = 4;
		load_correct_repeater("", true);
		//	uc_workorder.Visible		 = false;
		set_selectors();
		}

	protected void tab_history_Click(object sender, EventArgs e)
		{
		mv.ActiveViewIndex = 5;
		load_history_repeater("", true);
		//	uc_workorder.Visible		 = false;
		set_selectors();

		}
	protected void tab_po_Click(object sender, EventArgs e)
		{
		mv.ActiveViewIndex = 6;
		load_po_repeater("", true);
		//	uc_workorder.Visible		 = false;
		set_selectors();
		}
	protected void tab_annual_Click(object sender, EventArgs e)
		{
		mv.ActiveViewIndex = 7;
		if_shoppingcart_annual.Visible = false;
		rpt_annual.Visible = true;

		branchOptionsObject = new branch_options(WarehouseBusinessUnit.id);
		rpt_annual.Visible = branchOptionsObject.annual_count_on;


		load_annual_repeater("", true);



		//	uc_workorder.Visible		 = false;
		set_selectors();
		}
	protected void tab_search_Click(object sender, EventArgs e)
		{
		mv.ActiveViewIndex = 8;
		load_search_repeater("", true);
		//	uc_workorder.Visible		 = false;
		set_selectors();
		}

	#region general functions



	protected void set_selectors()
		{
		ddl_wo.ClientVisible = true;
		ddl_trucks.ClientVisible = true;

		ddl_company.ClientVisible = false;
		//       ddl_trucks.NullText = "Select Truck";

		switch (mv.ActiveViewIndex)
			{
			case 0:
				ddl_other_wo.ClientVisible = ddl_wo.Value != null;
				break;
			case 1:
				ddl_other_trucks.ClientVisible = true;
				ddl_other_wo.ClientVisible = false;
				break;
			case 2:
				ddl_other_trucks.ClientVisible = false;
				ddl_other_wo.ClientVisible = false;
				break;
			case 3:
				ddl_other_trucks.ClientVisible = false;
				ddl_other_wo.ClientVisible = false;
				break;
			case 4:
				/*			ddl_wo.SelectedIndex = -1;
							ddl_trucks.SelectedIndex = -1;
							ddl_other_trucks.SelectedIndex = -1;
							ddl_wo.ClientVisible = false;
							ddl_trucks.ClientVisible = false;
				 */
				ddl_other_trucks.SelectedIndex = -1;
				ddl_other_trucks.ClientVisible = false;
				ddl_other_wo.ClientVisible = false;
				break;
			case 5:
				ddl_other_trucks.ClientVisible = false;
				ddl_other_wo.ClientVisible = false;
				break;
			case 6:
				ddl_other_trucks.ClientVisible = false;
				ddl_other_wo.ClientVisible = false;
				break;
			case 7:
				ddl_other_trucks.ClientVisible = false;
				ddl_other_wo.ClientVisible = false;
				ddl_wo.ClientVisible = false;
				ddl_trucks.ClientVisible = true;
				ddl_company.ClientVisible = true;
				tr_qty_in_stock.Visible = false;
				tr_qty_on_order.Visible = false;
				tr_qty_on_truck.Visible = false;
				tr_qty_on_wo.Visible = false;
				break;
			case 8:
				ddl_other_trucks.ClientVisible = false;
				ddl_other_wo.ClientVisible = false;
				ScriptManager.RegisterStartupScript(this, GetType(), ClientID, string.Format("scroll_menu_left();", "Server"), true);

				break;
			}
		//    ddl_trucks.DataBind();
		handleTabs();
		ScriptManager.RegisterStartupScript(this, GetType(), ClientID, string.Format("set_tabs();", "Server"), true);
		}

	protected void ddl_trucks_SelectedIndexChanged(object sender, EventArgs e)
		{
		if (ddl_trucks.Value != null)
			{
			Session["mobile_ddl_truck"] = ddl_trucks.Value;
			}
		if (ddl_wo.Value != null)
			{
			Session["working_woprog_id"] = ddl_wo.Value;
			}
		load_page();
		}

	protected void ddl_company_SelectedIndexChanged(object sender, EventArgs e)
		{
		if (ddl_company.Value != null)
			{
			WorkingBusinessUnit = new NeBusinessUnit(ddl_company.Value);
			WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
			Session["working_business_unit_id"] = WorkingBusinessUnit.id;
			Session["warehouse_business_unit_id"] = WarehouseBusinessUnit.id;
			ddl_trucks.Value = null;
			Session["mobile_ddl_truck"] = null;
			ddl_trucks.DataBind();
			branchOptionsObject = new branch_options(WarehouseBusinessUnit.id);
			rpt_annual.Visible = branchOptionsObject.annual_count_on;
			load_annual_repeater("", false);
			}
		load_page();
		}

	protected void cb_part_Callback(object sender, CallbackEventArgsBase e)
		{
		if (e.Parameter != "")
			{


			if (!e.Parameter.Contains("|"))
				{
				var i = new inventory();
				i.Load(e.Parameter, WarehouseBusinessUnit.id);
				var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(i.master_id)));
				lbl_pop_part.Text = i.master_id;
				lbl_pop_description.Text = i.description_full;
				lbl_pop_qty_onhand.Text = il.qty + " (*)";
				lbl_pop_qty_external.Text = i.ext_onhand_qty.ToString();
				lbl_pop_qty_onwos.Text = i.wo_usage.ToString();
				lbl_pop_qty_onorder.Text = i.po_usage.ToString();
				lbl_pop_location.Text = i.location_name.Replace("'", "");
				img_pop_part.ImageUrl = "~/_tools/inventory_picture/index.aspx?id=" + e.Parameter;
				img_pop_part.DataBind();
				}
			else
				{
				var i = new inventory();
				i.Load(e.Parameter.Split('|').GetValue(1), WarehouseBusinessUnit.id);
				var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(i.master_id)));

				i.print_barcode_label(1);
				lbl_pop_part.Text = i.master_id;
				lbl_pop_description.Text = i.description_full;
				lbl_pop_qty_onhand.Text = il.qty + " (*)";
				lbl_pop_qty_external.Text = i.ext_onhand_qty.ToString();
				lbl_pop_qty_onwos.Text = i.wo_usage.ToString();
				lbl_pop_qty_onorder.Text = i.po_usage.ToString();
				lbl_pop_location.Text = i.location_name.Replace("'", "");
				img_pop_part.ImageUrl = "~/_tools/inventory_picture/index.aspx?id=" + e.Parameter;
				img_pop_part.DataBind();
				}
			}
		}




	protected void cb_fix_qty_Callback(object sender, CallbackEventArgsBase e)
		{
		if (e.Parameter != "save")
			{
			var i = new inventory();
			i.Load(e.Parameter, WarehouseBusinessUnit.id);
			var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(i.master_id)));

			//hdn_fix_qty_internal_location_id.Value
			lbl_fix_part.Text = i.master_id;
			lbl_fix_description.Text = i.description_full;
			lbl_fix_qty_insystem.Text = il.qty.ToString();
			hdn_fix_qty_internal_location_id.Value = il.location_master_id.ToString();

			lbl_fix_location.Text = i.location_name.Replace("'", "");
			Session["fix_master_id"] = i.master_id;
			Session["fix_location_id"] = hdn_fix_qty_internal_location_id.Value;

			}
		else
			{
			double new_qty = 0;
			var master_id = Convert.ToInt32(Session["fix_master_id"]);
			double.TryParse(txt_fix_actual_qty.Text, out new_qty);
			var location_id = Convert.ToInt32(Session["fix_location_id"]);
			if (master_id != 0 && location_id != 0 && txt_fix_actual_qty.Text != "")
				{
				var il = new location(location_id, WarehouseBusinessUnit.id, master_id);
				var ilm = new location_master(il.location_master_id);
				var em = new NeEMail();
				em.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
				em.To = WarehouseBusinessUnit.purchaser.NEEmail == "" ? WorkingBusinessUnit.branch_manager.NEEmail : WarehouseBusinessUnit.purchaser.NEEmail;
				if (WarehouseBusinessUnit.id == 1)
					{
					em.CC = "oakshipper@" + Toolbox.app_setting("DomainForEmail");
					}
				em.Subject = "Bin with Invalid Quantity - Master ID: " + master_id + " in Location: " + ilm.name;
				em.isHTML = true;
				var wo_n = ddl_wo.Value == null ? "N/A" : ddl_wo.Value.ToString();
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
", current_user.FullName, master_id, ilm.name, il.qty, wo_n, txt_fix_actual_qty.Text);
				//		em.Bcc = "aketelaars@newelectric.com";
				em.Send();

				var iil = new incorrect_levels();
				iil.master_id = master_id;
				iil.location_id = location_id;
				iil.incorrect_qty = il.qty;
				iil.reported_qty = Convert.ToDouble(txt_fix_actual_qty.Text);
				iil.member_id = current_user.id;
				iil.date = DateTime.Today;
				iil.id = 0;
				iil.save();
				cb_fix_qty.JSProperties["cp_close"] = "1";


				}
			else
				{



				}



			}
		}

	protected void load_page()
		{
		Load(this, null);
		Control ctrl;
		TextBox tb_search;
		if (ddl_wo.Value == null && ddl_trucks.Value == null && mv.ActiveViewIndex == 7)
			{

			mv.ActiveViewIndex = -1;
			//ddl_other_wo.ClientVisible = mv.ActiveViewIndex == 0;
			//		load_history_repeater("");
			rpt_wo.DataSource = "";
			rpt_wo.DataBind();

			}
		switch (mv.ActiveViewIndex)
			{
			case 0:
				if (rpt_wo.Controls.Count > 0)
					{
					if (rpt_wo.Visible)
						{
						ctrl = rpt_wo.Controls[0].Controls[0].FindControl("tb_wo_search");
						tb_search = (TextBox)ctrl;
						load_wo_repeater(tb_search.Text, false);
						}
					else
						{
						if_shoppingcart.Attributes["src"] = "/mobile/shopping_cart.aspx?is_mobile=1&woprog_id=" + Session["working_woprog_id"] + "&location_id=" + ddl_trucks.Value;
						//	if_shoppingcart.Attributes.Add("onload", "resizeIframe(this);");
						}
					}
				break;

			case 1:
				if (rpt_truck.Controls.Count > 0)
					{
					if (rpt_truck.Visible)
						{
						ctrl = rpt_truck.Controls[0].Controls[0].FindControl("tb_truck_search");
						tb_search = (TextBox)ctrl;
						load_truck_repeater(tb_search.Text, false);
						}
					else
						{
						if_shoppingcart_truck.Attributes["src"] = "/mobile/shopping_cart.aspx?is_mobile=1&woprog_id=0&location_id=" + ddl_trucks.Value;
						//		if_shoppingcart_truck.Attributes.Add("onload", "resizeIframe(this);");
						}
					}

				break;
			case 2:
				if (rpt_return.Controls.Count > 0)
					{
					ctrl = rpt_return.Controls[0].Controls[0].FindControl("tb_return_search");
					tb_search = (TextBox)ctrl;
					load_return_repeater(tb_search.Text, false);
					}
				break;
			case 3:
				if (rpt_pull.Controls.Count > 0)
					{
					ctrl = rpt_pull.Controls[0].Controls[0].FindControl("tb_pull_search");
					tb_search = (TextBox)ctrl;
					load_pull_repeater(tb_search.Text, false);
					}
				break;
			case 4:
				if (rpt_correct.Controls.Count > 0)
					{
					ctrl = rpt_correct.Controls[0].Controls[0].FindControl("tb_correct_search");
					tb_search = (TextBox)ctrl;
					load_correct_repeater(tb_search.Text, false);
					}
				break;
			case 5:
				if (rpt_history.Controls.Count > 0)
					{
					ctrl = rpt_history.Controls[0].Controls[0].FindControl("tb_history_search");

					tb_search = (TextBox)ctrl;
					load_history_repeater(tb_search.Text, false);

					}
				else
					{
					load_history_repeater("", false);
					}
				break;
			case 6:
				if (rpt_po.Controls.Count > 0)
					{
					ctrl = rpt_po.Controls[0].Controls[0].FindControl("tb_po_search");
					tb_search = (TextBox)ctrl;
					load_po_repeater(tb_search.Text, false);
					}
				break;
			case 7:
				if (rpt_annual.Visible)
					{

					ctrl = rpt_annual.Controls[0].Controls[0].FindControl("tb_annual_search");
					tb_search = (TextBox)ctrl;
					load_annual_repeater(tb_search.Text, false);
					}
				break;
			case 8:
				if_shoppingcart_search.Attributes["src"] = "/mobile/shopping_cart.aspx?is_mobile=1&woprog_id=" + Session["working_woprog_id"] + "&location_id=" + ddl_trucks.Value;

				break;
			}

		}

	protected void btn_search_Click(object sender, EventArgs e)
		{
		load_page();

		}

	#endregion

	#region wo stuff

	protected void load_wo_repeater(string search, bool initialLoad)
		{
		if(initialLoad)
			{ 
			load_page();
			}
		if (ddl_wo.Value != null && (int)ddl_wo.Value >= 0)
			{
			var sql = @"Select wo_detail_current.wo_detail_current_master_id master_id, wo_detail_current.wo_detail_current_description description,  
(TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM wo_detail_current.wo_detail_current_qty_committed)AS char)))) wo_qty, 
(TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM (wo_detail_current.wo_detail_current_qty_ordered-wo_detail_current.wo_detail_current_qty_committed))AS char)))) still_needed_qty ,
ifnull((Select qty from inventory_location il_default inner join inventory_location_master ilm on ilm.id=il_default.location_master_id where il_default.master_id = wo_detail_current.wo_detail_current_master_id and il_default.business_unit_id=" + WarehouseBusinessUnit.id + @" and ilm.type_id = 1 order by qty desc,min_qty desc, max_qty desc limit 1),0) internal_qty,
ifnull((Select il.qty from inventory_location il where il.location_master_id = " + (ddl_trucks.Value == null ? 0 : ddl_trucks.Value) + @"  and il.master_id=wo_detail_current.wo_detail_current_master_id),0) truck_qty,
'" + (ddl_trucks.Text == "" ? "No truck Selected" : "Take from " + ddl_trucks.Text) + @"' truck_lbl,
wo_detail_current.wo_detail_current_id line_id,
if(it.is_exclude=0 and it.allowed_to_stock=1,1,0) lck,
'" + (ddl_other_wo.Text == "" ? "No Other WO Selected" : "Move to " + ddl_other_wo.Value) + @"' o_wo_lbl
from wo_detail_current 
LEFT JOIN inventory_description ON inventory_description.master_id = wo_detail_current.wo_detail_current_master_id
LEFT join inventory_branch on inventory_branch.master_id =inventory_description.master_id and inventory_branch.business_unit_id = " + WarehouseBusinessUnit.id + @"  
LEFT join inventory_item_master iim on wo_detail_current.wo_detail_current_master_id = iim.master_id 
LEFT join inventory_tag it on iim.tag_id = it.tag_id 
 where wo_detail_current.wo_detail_current_woprog_id = " + ddl_wo.Value + @"  and (( it.active = 1 and it.is_exclude=0 ) or wo_detail_current.wo_detail_current_master_id = 777) 
                          order by wo_detail_current.wo_detail_current_qty_committed  desc";
			// where wo_detail_current.wo_detail_current_woprog_id = " + ddl_wo.Value + @"  and ((it.is_exclude=0 and it.allowed_to_stock = 1 and it.active = 1 and iim.approved=1) or wo_detail_current.wo_detail_current_master_id = 777) 
			Session["mobile_rpt_wo_progid"] = ddl_wo.Value.ToString();
			if (search != "")
				{
				sql = @"Select wo_detail_current.wo_detail_current_master_id master_id, wo_detail_current.wo_detail_current_description description,  (TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM wo_detail_current.wo_detail_current_qty_committed)AS char)))) wo_qty, (TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM (wo_detail_current.wo_detail_current_qty_ordered-wo_detail_current.wo_detail_current_qty_committed))AS char)))) still_needed_qty ,
ifnull((Select qty from inventory_location il_default inner join inventory_location_master ilm on ilm.id=il_default.location_master_id where il_default.master_id = wo_detail_current.wo_detail_current_master_id and il_default.business_unit_id=" + WarehouseBusinessUnit.id + @" and ilm.type_id = 1 order by qty desc,min_qty desc, max_qty desc limit 1),0) internal_qty,
ifnull((Select il.qty from inventory_location il where il.location_master_id = " + (ddl_trucks.Value == null ? 0 : ddl_trucks.Value) + @"  and il.master_id=wo_detail_current.wo_detail_current_master_id),0) truck_qty,
'" + (ddl_trucks.Text == "" ? "No truck Selected" : "Take from " + ddl_trucks.Text) + @"' truck_lbl,
wo_detail_current.wo_detail_current_id line_id,
if(it.is_exclude=0 and it.allowed_to_stock=1,1,0) lck,
'" + (ddl_other_wo.Text == "" ? "No Other WO Selected" : "Move to " + ddl_other_wo.Value) + @"' o_wo_lbl
from wo_detail_current 
LEFT JOIN inventory_description ON inventory_description.master_id = wo_detail_current.wo_detail_current_master_id
LEFT join inventory_branch on inventory_branch.master_id = wo_detail_current.wo_detail_current_master_id and inventory_branch.business_unit_id = " + WarehouseBusinessUnit.id + @" 
LEFT join inventory_item_master iim on wo_detail_current.wo_detail_current_master_id = iim.master_id  
LEFT join inventory_tag it on iim.tag_id = it.tag_id 
where wo_detail_current.wo_detail_current_woprog_id = " + ddl_wo.Value + @"  
						and (inventory_description.master_id like'%" + search + "%' or inventory_description.description like '%" + search + @"%')
and (( it.active = 1 and it.is_exclude=0 ) or wo_detail_current.wo_detail_current_master_id = 777) 
				order by wo_detail_current.wo_detail_current_qty_committed  desc";
				}

			rpt_wo.DataSource = Toolbox.doSQL_dt(sql, null);
			rpt_wo.DataBind();

			if (search != "")
				{
				var ctrl = rpt_wo.Controls[0].Controls[0].FindControl("tb_wo_search");
				var tb_search = (TextBox)ctrl;
				tb_search.Text = search;
				}
			}
		else
			{
			rpt_wo.DataBind();
			}
		}

	protected void btn_add_part_location_Click(object sender, EventArgs e)
		{
		rpt_annual.Visible = false;
		var ctrl = rpt_annual.Controls[0].Controls[0].FindControl("tb_annual_search");
		var tb_search = (TextBox)ctrl;
		if_shoppingcart_annual.Visible = true;
		if_shoppingcart_annual.Attributes["src"] = "/mobile/shopping_cart.aspx?is_mobile=1&woprog_id=0&location_id=" + ddl_trucks.Value + "&is_annual=1&search_string=" + Server.UrlEncode(tb_search.Text);
		//		if_shoppingcart.Attributes.Add("onload", "resizeIframe(this);");
		}

	protected void btn_add_part_Click(object sender, EventArgs e)
		{
		rpt_wo.Visible = false;
		ddl_other_wo.ClientVisible = false;
		var ctrl = rpt_wo.Controls[0].Controls[0].FindControl("tb_wo_search");
		var tb_search = (TextBox)ctrl;
		if_shoppingcart.Visible = true;
		if_shoppingcart.Attributes["src"] = "/mobile/shopping_cart.aspx?is_mobile=1&woprog_id=" + Session["mobile_rpt_wo_progid"] + "&location_id=" + ddl_trucks.Value + "&search_string=" + Server.UrlEncode(tb_search.Text);
		//		if_shoppingcart.Attributes.Add("onload", "resizeIframe(this);");
		}
	protected void btn_add_part_truck_Click(object sender, EventArgs e)
		{
		var ctrl = rpt_truck.Controls[0].Controls[0].FindControl("tb_truck_search");
		var tb_search = (TextBox)ctrl;
		rpt_truck.Visible = false;
		if_shoppingcart_truck.Visible = true;
		if_shoppingcart_truck.Attributes["src"] = "/mobile/shopping_cart.aspx?is_mobile=1&woprog_id=0&location_id=" + ddl_trucks.Value + "&search_string=" + tb_search.Text;
		//		if_shoppingcart_truck.Attributes.Add("onload", "resizeIframe(this);");
		}

	protected void btn_wo_save_Click(object sender, EventArgs e)
		{
		var woprog_id = ddl_wo.Value;
		var i = new inventory();
		NeWOProg wo;
		Session["mobile_rpt_wo_progid"] = null;
		Session["mobile_rpt_other_woprog_id"] = null;

		if (woprog_id != null && woprog_id != "")
			{
			wo = new NeWOProg(Convert.ToInt32(ddl_wo.Value));
			Session["mobile_rpt_wo_progid"] = wo;
			Session["mobile_rpt_other_woprog_id"] = new NeWOProg(Convert.ToInt32(ddl_other_wo.Value));
			foreach (RepeaterItem item in rpt_wo.Items)
				{
				if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
					{
					var master_id = (HtmlGenericControl)item.FindControl("rpt_wo_master_id");
					var tk_req = (TextBox)item.FindControl("TextBox5");
					var tr_stock = (HtmlInputGenericControl)item.FindControl("TextBox6");
					var tr_truck = (HtmlInputGenericControl)item.FindControl("TextBox7");
					var tr_transfer = (TextBox)item.FindControl("TextBox11");

					if (tk_req.Text != "")  // req some more
						{
						if (ddl_wo.Value != null && ddl_wo.Value.ToString() != "")
							{
							var line_id = tk_req.ToolTip;
							//					switch (master_id.InnerText)
							//					{
							//						case "777":
							update_req_line(Convert.ToInt32(line_id), Convert.ToDouble(tk_req.Text));
							//							break;
							//						default:
							//							add_part_on_wo(wo.woprog_id, Convert.ToInt32(master_id.InnerText), Convert.ToDouble(tk_req.Text), "req", current_user.id, 0);
							//							break;
							//					}
							}
						}
					double.TryParse(tr_stock.Value, out var qtyStock);
					if (qtyStock != 0) // MH: Disabling negative movement for post FSI, to get this working ==> Yan: Enable negative movement.
						{
						if (ddl_wo.Value != null && ddl_wo.Value != "")
							{

							var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(master_id.InnerText)));
							add_part_on_wo(wo.woprog_id, Convert.ToInt32(master_id.InnerText), qtyStock, "com", current_user.id, il.location_master_id);

							//					add_part_on_wo(wo.woprog_id, Convert.ToInt32(master_id.InnerText), Convert.ToDouble(tr_stock.Text), "com", current_user.id, Convert.ToInt32(ddl_trucks.Value));
							}
						}
					double.TryParse(tr_truck.Value, out var qtyTruck);
					if (qtyTruck != 0) // MH: Disabling negative movement for post FSI, to get this working ==> Yan: Enable negative movement.
						{
						if (ddl_wo.Value != null && ddl_wo.Value != "")
							{
							add_part_on_wo(wo.woprog_id, Convert.ToInt32(master_id.InnerText), qtyTruck, "com", current_user.id, Convert.ToInt32(ddl_trucks.Value));
							}
						}
					// MH: Disabling transfer logic for phase 1 release of this page. ==> Yan: Enable transfer movement.
					if (tr_transfer.Text != "" && tr_transfer.Text.Trim() != "0")
						{
						if (ddl_wo.Value != null && ddl_wo.Value != "")
							{
							if (ddl_other_wo.Value != null && ddl_other_wo.Value != "")
								{
								var line_id = tk_req.ToolTip;
								PartTransfer(line_id, Convert.ToInt32(ddl_other_wo.Value), Convert.ToDouble(tr_transfer.Text), wo.OrderNumber);
								}
							}
						}




					}
				}
			if (rpt_wo.Controls.Count > 0)
				{
				var ctrl = rpt_wo.Controls[0].Controls[0].FindControl("tb_wo_search");
				var tb_search = (TextBox)ctrl;

				load_wo_repeater(tb_search.Text, true);
				}
			else
				{
				load_wo_repeater("", true);
				}
			}
		}
	#endregion





	#region truck tab stuff
	protected void load_truck_repeater(string search, bool initialLoad)
		{
		if(initialLoad)
			{ 
			load_page();
			}
		var desc_country = current_user.Country == "USA" ? "usa" : "cdn";
		if (ddl_trucks.Value != null)
			{
			var sql = @"Select inventory_location.master_id master_id,qty truck_qty,inventory_description.desc_full_" + desc_country + @" description, 
ifnull((Select qty from inventory_location il_default inner join inventory_location_master ilm on ilm.id=il_default.location_master_id where il_default.master_id = inventory_location.master_id and il_default.business_unit_id=" + WarehouseBusinessUnit.id + @" and ilm.type_id = 1 order by qty desc,min_qty desc, max_qty desc limit 1),0) internal_qty,
ifnull((select sum(wo_detail_current.wo_detail_current_qty_committed) from wo_detail_current where wo_detail_current_woprog_id = " + (ddl_wo.Value == null ? 0 : ddl_wo.Value) + @" and wo_detail_current_master_id=inventory_location.master_id),0) wo_qty,
'" + (ddl_trucks.Text == "" ? "No truck Selected" : ddl_trucks.Text + " to WO") + @"' truck_lbl,
'" + (ddl_other_trucks.Text == "" ? "No Other truck Selected" : ddl_trucks.Text + " to " + ddl_other_trucks.Text) + @"' other_truck_lbl,
Concat('" + ddl_trucks.Text + @": ',qty) truck_qty_lbl
from inventory_location 
LEFT JOIN inventory_description ON inventory_location.master_id = inventory_description.master_id
LEFT join inventory_branch on inventory_branch.master_id = inventory_location.master_id and inventory_branch.business_unit_id = inventory_location.business_unit_id  
LEFT join inventory_item_master iim on inventory_branch.master_id = iim.master_id  
LEFT join inventory_tag it on iim.tag_id = it.tag_id and it.is_exclude=0 and it.allowed_to_stock = 1 and it.active = 1
 where location_master_id = " + ddl_trucks.Value + @" 
                          order by qty desc";
			if (search != "")
				{
				sql = @"Select inventory_location.master_id master_id,qty truck_qty,inventory_description.desc_full_" + desc_country + @" description, 
ifnull((Select qty from inventory_location il_default inner join inventory_location_master ilm on ilm.id=il_default.location_master_id where il_default.master_id = inventory_location.master_id and il_default.business_unit_id=" + WarehouseBusinessUnit.id + @" and ilm.type_id = 1 order by qty desc,min_qty desc, max_qty desc limit 1),0) internal_qty,
ifnull((select sum(wo_detail_current.wo_detail_current_qty_committed) from wo_detail_current where wo_detail_current_woprog_id = " + (ddl_wo.Value == null ? 0 : ddl_wo.Value) + @" and wo_detail_current_master_id=inventory_location.master_id),0) wo_qty,
'" + (ddl_trucks.Text == "" ? "No truck Selected" : ddl_trucks.Text + " to WO") + @"' truck_lbl,
'" + (ddl_other_trucks.Text == "" ? "No Other truck Selected" : ddl_trucks.Text + " to " + ddl_other_trucks.Text) + @"' other_truck_lbl,
Concat('" + ddl_trucks.Text + @": ',qty) truck_qty_lbl
from inventory_location 
LEFT JOIN inventory_description ON inventory_location.master_id = inventory_description.master_id
LEFT join inventory_branch on inventory_branch.master_id = inventory_location.master_id and inventory_branch.business_unit_id = inventory_location.business_unit_id  
LEFT join inventory_item_master iim on inventory_branch.master_id = iim.master_id  
LEFT join inventory_tag it on iim.tag_id = it.tag_id and it.is_exclude=0 and it.allowed_to_stock = 1 and it.active = 1
where location_master_id = " + ddl_trucks.Value + @" 
						and (inventory_description.master_id like'%" + search + "%' or inventory_description.description like '%" + search + "%') order by qty desc";
				}


			rpt_truck.DataSource = Toolbox.doSQL_dt(sql, null);
			rpt_truck.DataBind();

			if (search != "")
				{
				var ctrl = rpt_truck.Controls[0].Controls[0].FindControl("tb_truck_search");
				var tb_search = (TextBox)ctrl;
				tb_search.Text = search;
				}
			}
		}

	protected void btn_truck_save_Click(object sender, EventArgs e)
		{
		var truck_id = ddl_trucks.Value;
		var i = new inventory();
		NeWOProg wo;
		Session["mobile_rpt_wo_progid"] = null;
		if (truck_id != null && truck_id != "")
			{
			foreach (RepeaterItem item in rpt_truck.Items)
				{
				if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
					{
					var master_id = (HtmlGenericControl)item.FindControl("rpt_truck_master_id");
					var tk_stock = (TextBox)item.FindControl("TextBox1");
					var tr_wo = (TextBox)item.FindControl("TextBox2");
					var tr_truck = (TextBox)item.FindControl("TextBox3");

					if (tk_stock.Text != "" && tk_stock.Text != "0")  // take from stock
						{
						if (ddl_trucks.Value != null && ddl_trucks.Value.ToString() != "")
							{
							var il = new location();
							var _default_internal_location_id = branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(master_id.InnerText));
							var location_from = new location(_default_internal_location_id, WarehouseBusinessUnit.id, Convert.ToInt32(master_id.InnerText));
							var location_to = new location(Convert.ToInt32(ddl_trucks.Value), WarehouseBusinessUnit.id, Convert.ToInt32(master_id.InnerText));
							il.section_id = 19;
							il.xfer((int)location_from.id, (int)location_to.id, Convert.ToDouble(tk_stock.Text), current_user);
							tk_stock.Text = "";
							}
						}

					if (tr_wo.Text != "" && tr_wo.Text != "0")
						{
						if (ddl_wo.Value != null && ddl_wo.Value != "")
							{
							wo = new NeWOProg(Convert.ToInt32(ddl_wo.Value));
							add_part_on_wo(wo.woprog_id, Convert.ToInt32(master_id.InnerText), Convert.ToDouble(tr_wo.Text), "com", current_user.id, Convert.ToInt32(ddl_trucks.Value));
							}
						}

					if (tr_truck.Text != "" && tr_truck.Text != "0") // move between trucks
						{
						if (ddl_other_trucks.Value != null && ddl_other_trucks.Value.ToString() != "")
							{
							var il = new location();
							var location_from = new location(Convert.ToInt32(ddl_trucks.Value), WarehouseBusinessUnit.id, Convert.ToInt32(master_id.InnerText));
							var location_to = new location(Convert.ToInt32(ddl_other_trucks.Value), WarehouseBusinessUnit.id, Convert.ToInt32(master_id.InnerText));
							il.section_id = 19;
							il.xfer((int)location_from.id, (int)location_to.id, Convert.ToDouble(tr_truck.Text), current_user);
							tr_truck.Text = "";
							}
						}

					}
				}
			var ctrl = rpt_truck.Controls[0].Controls[0].FindControl("tb_truck_search");
			var tb_search = (TextBox)ctrl;

			load_truck_repeater(tb_search.Text, false);
			}
		}
	#endregion

	#region return tab stuff
	protected void load_return_repeater(string search, bool initialLoad)
		{
		if(initialLoad)
			{ 
			load_page();
			}

		if (ddl_wo.Value != null)
			{
			var sql = @"Select a.wo_detail_current_master_id master_id, 
a.wo_detail_current_description description,
0 truck_qty, 
ifnull((Select qty from inventory_location il_default inner join inventory_location_master ilm on ilm.id=il_default.location_master_id where il_default.master_id = a.wo_detail_current_master_id and il_default.business_unit_id=" + WarehouseBusinessUnit.id + @" and ilm.type_id = 1 order by qty desc,min_qty desc, max_qty desc limit 1),0) internal_qty, 
(TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM a.wo_detail_current_qty_committed)AS char))))  wo_qty
from wo_detail_current a 
inner join inventory_branch on inventory_branch.business_unit_id = " + WarehouseBusinessUnit.id + @" and inventory_branch.master_id =  a.wo_detail_current_master_id
inner join inventory_description ON inventory_branch.master_id = inventory_description.master_id
inner join inventory_item_master iim on inventory_branch.master_id = iim.master_id  
inner join inventory_tag it on iim.tag_id = it.tag_id and it.is_exclude=0 and it.allowed_to_stock = 1 and it.active = 1
where wo_detail_current_woprog_id = " + (ddl_wo.Value == null ? 0 : ddl_wo.Value) + @"  order by a.wo_detail_current_qty_committed desc";


			if (search != "")
				{
				sql = @"Select a.wo_detail_current_master_id master_id, 
a.wo_detail_current_description description,
0 truck_qty, 
ifnull((Select qty from inventory_location il_default inner join inventory_location_master ilm on ilm.id=il_default.location_master_id where il_default.master_id = a.wo_detail_current_master_id and il_default.business_unit_id=" + WarehouseBusinessUnit.id + @" and ilm.type_id = 1 order by qty desc,min_qty desc, max_qty desc limit 1),0) internal_qty,  
(TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM a.wo_detail_current_qty_committed)AS char))))  wo_qty
from wo_detail_current a 
LEFT join inventory_branch on inventory_branch.business_unit_id = " + WarehouseBusinessUnit.id + @" and inventory_branch.master_id =  a.wo_detail_current_master_id
LEFT join inventory_description ON inventory_branch.master_id = inventory_description.master_id
LEFT join inventory_item_master iim on inventory_branch.master_id = iim.master_id  
LEFT join inventory_tag it on iim.tag_id = it.tag_id and it.is_exclude=0 and it.allowed_to_stock = 1 and it.active = 1
where wo_detail_current_woprog_id = " + (ddl_wo.Value == null ? 0 : ddl_wo.Value) + @"  
						and (inventory_description.master_id like'%" + search + "%' or inventory_description.description like '%" + search + "%')  order by a.wo_detail_current_qty_committed desc";
				}

			rpt_return.DataSource = Toolbox.doSQL_dt(sql, null);
			rpt_return.DataBind();

			if (search != "")
				{
				var ctrl = rpt_return.Controls[0].Controls[0].FindControl("tb_return_search");
				var tb_search = (TextBox)ctrl;
				tb_search.Text = search;
				}
			}

		}

	protected void btn_return_save_Click(object sender, EventArgs e)
		{
		var truck_id = ddl_trucks.Value;
		var i = new inventory();
		NeWOProg wo;
		Session["mobile_rpt_wo_progid"] = null;

		if (ddl_wo.Value != null)
			{
			wo = new NeWOProg(Convert.ToInt32(ddl_wo.Value));
			Session["mobile_rpt_wo_progid"] = wo;
			foreach (RepeaterItem item in rpt_return.Items)
				{
				if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
					{
					var master_id = (HtmlGenericControl)item.FindControl("rpt_return_master_id");
					var return_to_stock = (TextBox)item.FindControl("TextBox2");
					var return_to_truck = (TextBox)item.FindControl("TextBox3");
					if (return_to_stock.Text != "" && return_to_stock.Text != "0")
						{

						var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(master_id.InnerText)));
						add_part_on_wo(wo.woprog_id, Convert.ToInt32(master_id.InnerText), Convert.ToDouble(return_to_stock.Text), "ret", current_user.id, il.location_master_id);

						}

					if (return_to_truck.Text != "" && return_to_truck.Text != "0" && ddl_trucks.Value != null)
						{

						add_part_on_wo(wo.woprog_id, Convert.ToInt32(master_id.InnerText), Convert.ToDouble(return_to_truck.Text), "ret", current_user.id, Convert.ToInt32(ddl_trucks.Value));

						}

					}
				}
			var ctrl = rpt_return.Controls[0].Controls[0].FindControl("tb_return_search");
			var tb_search = (TextBox)ctrl;

			load_return_repeater(tb_search.Text, true);
			}
		}
	#endregion

	#region pull tab stuff
	protected void load_pull_repeater(string search, bool initialLoad)
		{
		if(initialLoad)
			{ 
			load_page();
			}

		if (ddl_wo.Value != null)
			{
			var sql = @"Select a.wo_detail_current_master_id master_id, 
a.wo_detail_current_description description,
ifnull((Select il.qty from inventory_location il where il.location_master_id = " + (ddl_trucks.Value == null ? 0 : ddl_trucks.Value) + @" and il.business_unit_id = " + WarehouseBusinessUnit.id + @" and il.master_id=a.wo_detail_current_master_id),0) truck_qty, 
ifnull((Select il_default.qty from inventory_location il_default inner join inventory_location_master ilm on ilm.id=il_default.location_master_id where il_default.master_id = a.wo_detail_current_master_id and il_default.business_unit_id=" + WarehouseBusinessUnit.id + @" and ilm.type_id = 1 order by il_default.qty desc,il_default.min desc, il_default.max desc limit 1),0) internal_qty, 
(TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM (a.wo_detail_current_qty_ordered-a.wo_detail_current_qty_committed))AS char))))  wo_qty
from wo_detail_current a 
LEFT join inventory_branch on inventory_branch.business_unit_id = " + WarehouseBusinessUnit.id + @" and inventory_branch.master_id =  a.wo_detail_current_master_id
LEFT join inventory_description ON inventory_branch.master_id = inventory_description.master_id
LEFT join inventory_item_master iim on inventory_branch.master_id = iim.master_id  
LEFT join inventory_tag it on iim.tag_id = it.tag_id and it.is_exclude=0 and it.allowed_to_stock = 1 and it.is_exclude=0  and it.active = 1
where wo_detail_current_woprog_id = " + (ddl_wo.Value == null ? 0 : ddl_wo.Value) + @"  
and (a.wo_detail_current_qty_ordered-a.wo_detail_current_qty_committed)<>0
order by (a.wo_detail_current_qty_ordered-a.wo_detail_current_qty_committed) desc";
			if (search != "")
				{
				sql = @"Select a.wo_detail_current_master_id master_id, 
a.wo_detail_current_description description,
ifnull((Select il.qty from inventory_location il where il.location_master_id = " + (ddl_trucks.Value == null ? 0 : ddl_trucks.Value) + @" and il.business_unit_id = " + WarehouseBusinessUnit.id + @"  and il.master_id=a.wo_detail_current_master_id),0) truck_qty, 
ifnull((Select il_default.qty from inventory_location il_default inner join inventory_location_master ilm on ilm.id=il_default.location_master_id where il_default.master_id = a.wo_detail_current_master_id and il_default.business_unit_id=" + WarehouseBusinessUnit.id + @" and ilm.type_id = 1 order by il_default.qty desc,il_default.min desc, il_default.max desc limit 1),0) internal_qty, 
(TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM (a.wo_detail_current_qty_ordered-a.wo_detail_current_qty_committed))AS char))))  wo_qty
from wo_detail_current a 
LEFT join inventory_branch on inventory_branch.business_unit_id = " + WarehouseBusinessUnit.id + @" and inventory_branch.master_id =  a.wo_detail_current_master_id
LEFT join inventory_description ON inventory_branch.master_id = inventory_description.master_id
LEFT join inventory_item_master iim on inventory_branch.master_id = iim.master_id  
LEFT join inventory_tag it on iim.tag_id = it.tag_id and it.is_exclude=0 and it.allowed_to_stock = 1 and it.is_exclude=0  and it.active = 1
where wo_detail_current_woprog_id = " + (ddl_wo.Value == null ? 0 : ddl_wo.Value) + @" 
and (a.wo_detail_current_qty_ordered-a.wo_detail_current_qty_committed)<>0
						and (inventory_description.master_id like'%" + search + "%' or inventory_description.description like '%" + search + "%') order by (a.wo_detail_current_qty_ordered-a.wo_detail_current_qty_committed) desc";
				}

			rpt_pull.DataSource = Toolbox.doSQL_dt(sql, null);
			rpt_pull.DataBind();

			if (search != "")
				{
				var ctrl = rpt_pull.Controls[0].Controls[0].FindControl("tb_pull_search");
				var tb_search = (TextBox)ctrl;
				tb_search.Text = search;
				}
			}
		}

	protected void btn_pull_clear_Click(object sender, EventArgs e)
		{
		if ((int)ddl_wo.Value > 0)
			{
			Toolbox.doSQL_void(@"update wo_detail_current a set a.wo_detail_current_qty_ordered = a.wo_detail_current_qty_committed where a.wo_detail_current_woprog_id =@v0 ", new object[] { ddl_wo.Value });

			}

		load_pull_repeater("", false);

		}

	protected void btn_pull_save_Click(object sender, EventArgs e)
		{
		var truck_id = ddl_trucks.Value;
		var i = new inventory();
		NeWOProg wo;
		Session["mobile_rpt_wo_progid"] = null;

		if ((int)ddl_wo.Value >= 0)
			{
			wo = new NeWOProg(Convert.ToInt32(ddl_wo.Value));
			Session["mobile_rpt_wo_progid"] = wo;
			foreach (RepeaterItem item in rpt_pull.Items)
				{
				if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
					{
					var master_id = (HtmlGenericControl)item.FindControl("rpt_pull_master_id");
					var tk_stock = (HtmlInputGenericControl)item.FindControl("TextBox2");
					var tk_truck = (HtmlInputGenericControl)item.FindControl("TextBox4");

					double.TryParse(tk_stock.Value, out var qtyStock);
					if (qtyStock > 0)  // pull from stock
						{

						var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(master_id.InnerText)));
						add_part_on_wo(wo.woprog_id, Convert.ToInt32(master_id.InnerText), qtyStock, "com", current_user.id, il.location_master_id);

						}
					double.TryParse(tk_truck.Value, out var qtyTruck);
					if (qtyTruck > 0 && ddl_trucks.Value != null)  // pull from truck
						{

						add_part_on_wo(wo.woprog_id, Convert.ToInt32(master_id.InnerText), qtyTruck, "com", current_user.id, Convert.ToInt32(ddl_trucks.Value));
						}
					}
				}
			}
		var ctrl = rpt_pull.Controls[0].Controls[0].FindControl("tb_pull_search");
		var tb_search = (TextBox)ctrl;

		load_pull_repeater(tb_search.Text, false);

		}
	#endregion


	#region Correct tab stuff
	protected void load_correct_repeater(string search, bool initialLoad)
		{
		if(initialLoad)
			{ 
			load_page();
			}

		var sql = @"SELECT  
inventory_incorrect_levels.id,
inventory_incorrect_levels.master_id,
Location.`name` AS location_name,
if (business_unit.country='CDN',inventory_description.desc_full_cdn,inventory_description.desc_full_usa) AS description,
inventory_location.qty internal_qty,
Location.id location_id
FROM
inventory_incorrect_levels
LEFT JOIN inventory_location_master AS Location ON inventory_incorrect_levels.location_id = Location.id
LEFT JOIN inventory_description ON inventory_incorrect_levels.master_id = inventory_description.master_id
LEFT JOIN inventory_location ON Location.id = inventory_location.location_master_id AND inventory_incorrect_levels.master_id = inventory_location.master_id 
LEFT JOIN business_unit ON business_unit.id = inventory_location.business_unit_id
where Location.business_unit_id=" + WarehouseBusinessUnit.id + @" and (inventory_incorrect_levels.cleared_date is null)  

UNION
SELECT
0 AS id,
inventory_location.master_id,
inventory_location_master.`name` AS location_name,
if (business_unit.country='CDN',inventory_description.desc_full_cdn,inventory_description.desc_full_usa) AS description,
inventory_location.qty internal_qty,
inventory_location_master.id location_id
FROM
inventory_location_master
LEFT JOIN inventory_location ON inventory_location_master.id = inventory_location.location_master_id
LEFT JOIN inventory_description ON inventory_location.master_id = inventory_description.master_id
LEFT JOIN business_unit ON business_unit.id = inventory_location.business_unit_id
WHERE
inventory_location.qty < 0 AND
inventory_location_master.business_unit_id = " + WarehouseBusinessUnit.id + @"

order by location_name";
		if (search != "")
			{
			sql = @"
SELECT  
inventory_incorrect_levels.id,
inventory_incorrect_levels.master_id,
Location.`name` AS location_name,
if (business_unit.country='CDN',inventory_description.desc_full_cdn,inventory_description.desc_full_usa) AS description,
inventory_location.qty internal_qty,
Location.id location_id
FROM
inventory_incorrect_levels
LEFT JOIN inventory_location_master AS Location ON inventory_incorrect_levels.location_id = Location.id
LEFT JOIN inventory_description ON inventory_incorrect_levels.master_id = inventory_description.master_id
LEFT JOIN inventory_location ON Location.id = inventory_location.location_master_id AND inventory_incorrect_levels.master_id = inventory_location.master_id 
LEFT JOIN business_unit ON business_unit.id = inventory_location.business_unit_id
where Location.business_unit_id=" + WarehouseBusinessUnit.id + @" and (inventory_incorrect_levels.cleared_date is null) and (inventory_description.master_id like'%" + search + "%' or inventory_description.description like '%" + search + @"%') 

UNION
SELECT
0 AS id,
inventory_location.master_id,
inventory_location_master.`name` AS location_name,
if (business_unit.country='CDN',inventory_description.desc_full_cdn,inventory_description.desc_full_usa) AS description,
inventory_location.qty internal_qty,
inventory_location_master.id location_id
FROM
inventory_location_master
LEFT JOIN inventory_location ON inventory_location_master.id = inventory_location.location_master_id
LEFT JOIN inventory_description ON inventory_location.master_id = inventory_description.master_id
LEFT JOIN business_unit ON business_unit.id = inventory_location.business_unit_id
WHERE
inventory_location.qty < 0 AND
inventory_location_master.business_unit_id = " + WarehouseBusinessUnit.id + @" and (inventory_description.master_id like'%" + search + "%' or inventory_description.description like '%" + search + @"%') 

order by location_name";
			}


		rpt_correct.DataSource = Toolbox.doSQL_dt(sql, null);
		rpt_correct.DataBind();

		if (search != "")
			{
			var ctrl = rpt_correct.Controls[0].Controls[0].FindControl("tb_correct_search");
			var tb_search = (TextBox)ctrl;
			tb_search.Text = search;
			}

		}



	protected void btn_correct_save_Click(object sender, EventArgs e)
		{

		foreach (RepeaterItem item in rpt_correct.Items)
			{
			if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
				{
				var hdn_ic_location_id = (HiddenField)item.FindControl("hdn_ic_location_id");
				var hdn_master_id = (HiddenField)item.FindControl("hdn_master_id");
				var hdn_master_location_id = (HiddenField)item.FindControl("hdn_master_location_id");

				var ic_location_id = hdn_ic_location_id.Value;
				var master_id = hdn_master_id.Value;
				var master_location_id = hdn_master_location_id.Value;

				var tk_stock = (TextBox)item.FindControl("TextBox2");

				if (tk_stock.Text != "")
					{
					double qty = 0;
					var can_convert = double.TryParse(tk_stock.Text, out qty);
					if (!can_convert)
						{
						xfer_lb_warning.Text = "Invalid Quantity Supplied.";
						return;
						}
					if (qty < 0)
						{
						xfer_lb_warning.Text = "Negative Quantity Supplied.";
						return;
						}
					var ilm = new location_master();
					var il = new location();
					if (master_location_id != "0")
						{
						ilm = new location_master(Convert.ToInt32(master_location_id));
						}
					if (master_id != "0")
						{
						il = new location(Convert.ToInt32(master_location_id), WarehouseBusinessUnit.id, Convert.ToInt32(master_id));
						}

					var i = new inventory();
					i.Load(master_id, WarehouseBusinessUnit.id);


					if (il.id > 0 && ilm.id > 0 && master_id != "0")
						{
						var ib = new branch(il.master_id, WarehouseBusinessUnit.id);
						var prev_qty = il.qty;
						var this_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] { il.master_id, il.business_unit_id });
						il.log_is_manual = true;
						il.member_id = current_user.id;
						il.alert_worthy = true;
						var diff = qty - il.qty;
						il.qty = qty;
						il.section_id = 19;
						var this_type = diff < 0 ? 3 : 2;
						il.save();
						il.update_branch(this_type, ib.dollar_balance, diff, this_cost, ib);
						//			inventory_branch post_ib = new inventory_branch(il.master_id, (int)il.business_unit_id);

						if (ic_location_id != "0")
							{
							var iil = new incorrect_levels();
							if (ic_location_id != null)
								{
								iil = new incorrect_levels(Convert.ToInt32(ic_location_id));
								master_id = iil.master_id.ToString();
								master_location_id = iil.location_id.ToString();
								iil.approve(iil.id, current_user.id, qty);
								if (current_user.id != il.member_id)
									{
									var email = new NeEMail();
									email.To = new NeMember(Convert.ToInt32(iil.member_id)).NEEmail;
									email.Subject = "Location Qty Update";
									email.Body = "<div style='font-family: arial;'>Part: " + master_id + " " + i.description_full + " in location: " + il.this_master.name + " had its qty updated from " + prev_qty + " to " + qty + ".  You are being notified because your name is on record for reporting the incorrect qty for this part at this location.</div>";
									email.isHTML = true;
									email.Send();
									}
								}

							}






						}


					}
				}
			}
		load_correct_repeater("", false);

		}
	#endregion

	#region annual counts stuff

	protected void load_annual_repeater(string search, bool initialLoad)
		{
		if(initialLoad)
			{ 
			load_page();
			}
		//(SELECT MAX(dt) FROM log USE INDEX (business_unit_id_4) where business_unit_id = c_id AND action_id = 3 AND associated_table = "inventory_location" AND associated_alt_table_id = a.master_id)
		if (branchOptionsObject.annual_count_on)
			{
			//      rpt_annual.Controls[0].Controls[0].FindControl("lbl_top_annual
			DataTable dt_annual;


			if (Session["mobile_ddl_truck"] == null || Session["mobile_ddl_truck"].ToString() == "0" || Session["mobile_ddl_truck"].ToString() == "" || ddl_trucks.Value == null)
				{
				dt_annual = Toolbox.doSQL_dt(@"call get_inventory_locations_with_qtys(@v0,@v1,@v2)", new object[] { WarehouseBusinessUnit.id, branchOptionsObject.annual_count_start_date.ToString("yyyy-MM-dd"), branchOptionsObject.annual_count_end_date.ToString("yyyy-MM-dd") });
				}
			else
				{
				dt_annual = Toolbox.doSQL_dt(@"call get_inventory_qtys_at_location(@v0,@v1,@v2,@v3)", new object[] { WarehouseBusinessUnit.id, branchOptionsObject.annual_count_start_date.ToString("yyyy-MM-dd"), branchOptionsObject.annual_count_end_date.ToString("yyyy-MM-dd"), ddl_trucks.Value });
				}

			var empty_search = false;
			if (search != "")
				{
				if (dt_annual.Select("convert(master_id,'System.String') like '%" + search.ToUpper() + "%' or description like '%" + search.ToUpper() + "%' or location like '%" + search.ToUpper() + "%'").Length > 0)
					{
					dt_annual = dt_annual.Select("convert(master_id,'System.String') like '%" + search.ToUpper() + "%' or description like '%" + search.ToUpper() + "%' or location like '%" + search.ToUpper() + "%'").CopyToDataTable();
					}
				else
					{
					empty_search = true;
					}


				}
			if (!empty_search)
				{
				var dtn = dt_annual.Clone();
				var i = 0;
				var n_results = (Label)vw_annual.FindControl("n_results");
				if (n_results != null)
					{
					n_results.Text = string.Format("{0:N0} result(s) returned<div style='font-size:12px;'>{1}</div>", dt_annual.Rows.Count, dt_annual.Rows.Count > 500 ? "Showing the first 500 results" : "");
					}
				foreach (DataRow row in dt_annual.Rows)
					{
					if (i < 500)
						{
						dtn.ImportRow(row);
						i++;
						}
					if (i > 500)
						break;
					}
				rpt_annual.DataSource = dtn;

				rpt_annual.DataBind();
				}

			if (search != "")
				{
				var ctrl = rpt_annual.Controls[0].Controls[0].FindControl("tb_annual_search");
				var tb_search = (TextBox)ctrl;
				tb_search.Text = search;
				if (empty_search)
					{
					n_results.Text = string.Format(@"No matches for ""{0}""", search);
					}
				}
			}
		}


	struct error_helper
		{
		public int master_id { get; set; }
		public string qty { get; set; }
		public string error { get; set; }
		}
	protected string ShowMergedInfo(object _old_id)
		{
		if (Toolbox.ReturnZeroIfNull_int(_old_id) == 0)
			{
			return "";
			}
		else
			{
			return " - was MID: #" + _old_id;
			}
		}
	protected void btn_annual_save_Click(object sender, EventArgs e)
		{
		// Build up list of parts in repeater
		var list_parts = new List<string>();
		var dict_parts = new Dictionary<Tuple<int, int>, DateTime>();
		foreach (RepeaterItem item in rpt_annual.Items)
			{
			if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
				{
				var tk_stock = (TextBox)item.FindControl("TextBox2");
				if (tk_stock.Text != "" && ddl_company.Value != null && ddl_company.Value != "")
					{
					var hdn_master_id = (HiddenField)item.FindControl("hdn_master_id");
					var master_id = hdn_master_id.Value;
					list_parts.Add(master_id);
					}
				}
			}
		var dt_lastedits = Toolbox.doSQL_dt(string.Format(@"SELECT masterid,MAX(ts)ts,locationid FROM inventory_annual_counts_history 
WHERE business_unit_id = @v0  AND masterid IN ({0}) AND DATE(ts) BETWEEN @v1 AND @v2 
GROUP BY masterid,locationid ", string.Join(",", list_parts)), new object[] { ddl_company.Value, branchOptionsObject.annual_count_start_date.ToString("yyyy-MM-dd"), branchOptionsObject.annual_count_end_date.ToString("yyyy-MM-dd") });
		foreach (DataRow dr in dt_lastedits.Rows)
			{
			var m = Convert.ToInt32(dr["masterid"]);
			var l = Convert.ToInt32(dr["locationid"]);
			var d = Toolbox.ReturnBlankDateTimeIfNull(dr["ts"]);
			dict_parts.Add(new Tuple<int, int>(m, l), d);
			}
		var list_errors = new List<error_helper>();
		var lines_processed = 0;
		foreach (RepeaterItem item in rpt_annual.Items)
			{
			if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
				{
				var tk_stock = (TextBox)item.FindControl("TextBox2");
				if (tk_stock.Text != "" && ddl_company.Value != null && ddl_company.Value != "")
					{

					//        var rpt_annual_last_inv  = (HtmlGenericControl)item.FindControl("rpt_annual_last_inv");
					var hdn_annual_last_inv = (HiddenField)item.FindControl("hdn_annual_last_inv");

					// force is_replace logic to look at live data and not the stale gridview data

					var hdn_master_id = (HiddenField)item.FindControl("hdn_master_id");
					var master_id = hdn_master_id.Value;
					var int_master_id = Convert.ToInt32(master_id);
					var location_master_id = 0;
					var location_id = 0;
					var ilm = new location_master();
					var il = new location();
					var td = (HiddenField)item.FindControl("hdn_location_id");
					var hdn_qty = (HiddenField)item.FindControl("hdn_annual_qty");
					var hdn_db = (HiddenField)item.FindControl("hdn_annual_db");
					var old_qty = hdn_qty.Value == "" ? 0 : Convert.ToDouble(hdn_qty.Value);
					var dollar_balance = hdn_db.Value == "" ? 0 : Convert.ToDouble(hdn_db.Value);


					if (td.Value != "" && td.Value != "0")
						{
						location_master_id = Convert.ToInt32(td.Value);
						ilm = new location_master(Convert.ToInt32(td.Value));
						}
					if (master_id != "" && master_id != "0")
						{
						il = new location(location_master_id, Convert.ToInt32(ddl_company.Value), int_master_id);
						}
					var key = new Tuple<int, int>(int_master_id, location_master_id);
					var last_edit_db = dict_parts.ContainsKey(key) ? dict_parts[key] : new DateTime();
					var last_edit_dt = hdn_annual_last_inv.Value == "" ? new DateTime() : Convert.ToDateTime(hdn_annual_last_inv.Value);
					var is_replace = hdn_annual_last_inv.Value == "";
					if (last_edit_db != last_edit_dt)
						{
						list_errors.Add(new error_helper { master_id = int_master_id, qty = tk_stock.Text, error = "Updated since page last loaded" });
						continue;
						}


					if (il.id > 0 && ilm.id > 0 && Convert.ToInt32(master_id) > 0 && tk_stock.Text != "")
						{
						double qty = 0;
						var can_convert = double.TryParse(tk_stock.Text, out qty);

						if (!can_convert)
							{
							list_errors.Add(new error_helper { master_id = int_master_id, qty = tk_stock.Text, error = "Invalid Quantity Supplied" });
							continue;
							}

						var ib = new branch(il.master_id, il.business_unit_id);
						var prev_qty = il.qty;
						var this_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] { il.master_id, il.business_unit_id });
						il.log_is_manual = true;
						il.member_id = current_user.id;
						il.alert_worthy = false;
						il.qty = !is_replace ? il.qty + qty : qty;
						il.section_id = 10;

						il.save();
						var diff = is_replace ? qty - prev_qty : qty;
						var this_type = diff < 0 ? 3 : 2;
						il.update_branch(this_type, ib.dollar_balance, diff, this_cost, ib);
						var post_ib = new branch(il.master_id, il.business_unit_id);


						var list_hist = new List<history_vars>();
						var h = new history_vars
							{
							business_unit_id = il.business_unit_id,
							cost = Convert.ToDouble(this_cost),
							location_master_id = Convert.ToInt32(location_master_id),
							master_id = Convert.ToInt32(master_id),
							member_id = current_user.id,
							origin = "mobile",
							post_dollar_balance = is_replace ? qty * this_cost : (qty + old_qty) * this_cost,
							post_onhandqty = is_replace ? qty : qty + old_qty,
							pre_dollar_balance = dollar_balance,
							pre_onhandqty = Convert.ToDouble(old_qty),
							qty = qty,
							when_ts = DateTime.Now
							};

						list_hist.Add(h);
						add_history_line(list_hist);
						tk_stock.Text = "";
						lines_processed++;
						}
					}
				}

			}
		if (lines_processed > 0)
			{
			xfer_lb_notice.Text = string.Format(@"
<div style='margin:5px;padding:10px;background-color:#090;color:#fff;border-radius:5px;'>
	<div style='font-size:1.5em;'>{0} Entry/Entries Processed Successfully</div>
</div>", lines_processed);
			}
		if (list_errors.Count > 0)
			{
			var error_sb = new StringBuilder();
			error_sb.Append(@"
<div style='margin:5px;padding:10px;background-color:#f00;color:#fff;border-radius:5px;'>
<div style='font-size:1.5em;'>Some Error(s) Occured While Processing.</div><br/>
<table cellspacing='0' cellpadding='2' style='background-color:#fff;border:solid 1px #f00;color:#000;'>
	<thead>
		<tr style='background-color:#ddd;color:#000;'>
			<th>Master ID</th>
			<th>QTY</th>
			<th>Error</th>
		</tr>
	</thead>
	<tbody>");
			foreach (var err in list_errors)
				{
				error_sb.AppendFormat(@"
		<tr>
			<td align='center'>{0}</td>
			<td align='center'>{1}</td>
			<td align='center' style='color:#f00;'>{2}</td>
		</tr>", err.master_id, err.qty, err.error);
				}
			error_sb.Append(@"
	</tbody>
</table>
</div>
");
			xfer_lb_warning.Text = error_sb.ToString();
			}
		var ctrl = rpt_annual.Controls[0].Controls[0].FindControl("tb_annual_search");
		var tb_search = (TextBox)ctrl;
		load_annual_repeater(tb_search.Text, false);

		}

	private void add_history_line(
			int business_unit_id,
			int master_id,
			double qty,
			int location_master_id,
			int member_id,
			string origin,

			double previous_cost,
			double prev_dollar_balance,
			double post_dollar_balance,
			double pre_onhand_qty,
			double post_onhand_qty
			)
		{
		Toolbox.doSQL_void(@"
INSERT INTO inventory_annual_counts_history
	(
	business_unit_id,
	masterid,
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
	NOW(),
	@v5,
	@v6,
	@v7,
	@v8,
	@v9,
	@v10
	)
", new object[] {
	business_unit_id,				// {0}
    master_id,				// {1}
    qty,					// {2}
    location_master_id,		// {3}
    member_id,				// {4}
    origin,					// {5}
    previous_cost,			// {6}
    prev_dollar_balance,	// {7}
    post_dollar_balance,	// {8}
    pre_onhand_qty,			// {9}
    post_onhand_qty			// {10}
 });
		}
	private struct history_vars
		{
		public int business_unit_id { get; set; }
		public int master_id { get; set; }
		public double qty { get; set; }
		public int location_master_id { get; set; }
		public int member_id { get; set; }
		public string origin { get; set; }
		public double cost { get; set; }
		public double pre_dollar_balance { get; set; }
		public double post_dollar_balance { get; set; }
		public double pre_onhandqty { get; set; }
		public double post_onhandqty { get; set; }
		public DateTime when_ts { get; set; }
		}
	private void add_history_line(List<history_vars> list_vars)
		{
		var sb = new StringBuilder();
		sb.Append(@"
INSERT INTO inventory_annual_counts_history
	(
	business_unit_id,
	masterid,
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
");
		for (var i = 0; i < list_vars.Count(); i++)
			{
			var hv = list_vars[i];
			sb.AppendFormat(@"	
	(
	'{0}',
	'{1}',
	'{2}',
	'{3}',
	'{4}',
	'{11}',
	'{5}',
	'{6}',
	'{7}',
	'{8}',
	'{9}',
	'{10}'
	)",
	hv.business_unit_id,                        // {0}
	hv.master_id,                       // {1}
	hv.qty,                             // {2}
	hv.location_master_id,              // {3}
	hv.member_id,                       // {4}
	hv.origin,                          // {5}
	hv.cost,                            // {6}
	hv.pre_dollar_balance,              // {7}
	hv.post_dollar_balance,             // {8}
	hv.pre_onhandqty,                   // {9}
	hv.post_onhandqty,                  // {10}
	Toolbox.MySQL_longdt(hv.when_ts)    // {11}
			);
			if (i < list_vars.Count() - 1)
				{
				sb.Append(",");
				}
			}
		Toolbox.doSQL_void(sb.ToString());
		}


	#endregion


	#region search tab stuff

	protected void load_search_repeater(string search, bool initialLoad)
		{
		if(initialLoad)
			{ 
			load_page();
			}

		if_shoppingcart_search.Attributes["src"] = "/mobile/shopping_cart.aspx?is_mobile=1&woprog_id=" + Session["working_woprog_id"] + "&location_id=" + ddl_trucks.Value;


		}

	protected void btn_search_save_Click(object sender, EventArgs e)
		{




		}
	#endregion

	#region history tab stuff
	protected void load_history_repeater(string search, bool initialLoad)
		{
		if(initialLoad)
			{ 
			load_page();
			}
		var where = "";
		if (ddl_wo.Value != null)
			{
			where = @"ss.stock_transfer_type in(1,3,4,6,7) and 
(ss.stock_transfer_from_id = " + ddl_wo.Value + @" OR
ss.stock_transfer_to_id = " + ddl_wo.Value + @") AND ";
			}
		if (ddl_trucks.Value != null)
			{
			where += @" 
(ss.stock_transfer_from_id = " + ddl_trucks.Value + @" OR
ss.stock_transfer_to_id = " + ddl_trucks.Value + @") AND ";
			}
		if (where.Length > 0)
			{
			where = where.Substring(0, where.Length - 4);
			}
		else
			{
			where = " ss.business_unit_id =" + WarehouseBusinessUnit.id;
			}

		var sql_annual_counts = @"
(Select l.associated_alt_table_id master_id,
ide.desc_full_cdn description,
l.dt _date,
concat(mm.member_fullname, ' changed qty at ',ilm_from.`name`,' from ', l.value_old, ' to ', l.value_new) note


from `log` l
left join inventory_description ide on ide.master_id= l.associated_alt_table_id
left join inventory_location il on il.id = l.associated_table_id
left join inventory_location_master ilm_from on ilm_from.id= il.location_master_id
left join member mm on mm.member_id = l.member_id

where l.section_id=10 and l.action_id = 3 and l.business_unit_id = " + WarehouseBusinessUnit.id + @"  and l.dt >= '" + branchOptionsObject.annual_count_start_date.ToString("yyyy-MM-dd") + @"' order by l.dt desc limit 100)

union";

		var sql = @"

(Select ss.stock_transfer_master_id master_id,
ss.stock_transfer_description description,
ss.stock_transfer_date_added _date,
if(ss.stock_transfer_type = 6,Concat(member.member_fullname,if(ss.stock_transfer_quantity>0,' received ','returned'),abs(ss.stock_transfer_quantity ),if(ss.stock_transfer_quantity>0,' from PO: ', ' back to'),ph.poprog_bvpo,if(ss.stock_transfer_quantity>0,' to WO: ',' from WO:'),wo_to.woprog_bvwo),
Concat(member.member_fullname,' moved ',abs(ss.stock_transfer_quantity) ,' from (',if(ss.stock_transfer_quantity<0,ifnull(ilm_from.`name`,concat('wo-',wo_from.woprog_bvwo)),ifnull(ilm_to.`name`,concat('wo-',wo_to.woprog_bvwo))),') to (',if(ss.stock_transfer_quantity<0,ifnull(ilm_to.`name`,concat('wo-',wo_to.woprog_bvwo)),ifnull(ilm_from.`name`,concat('wo-',wo_from.woprog_bvwo))),')')) note

FROM
stock_transfer AS ss
INNER JOIN stock_transfer_type ON ss.stock_transfer_type = stock_transfer_type.id
INNER JOIN member on member.Member_ID = ss.stock_transfer_member_id
left join po_details_current podc on podc.po_details_id = stock_transfer_from_id
left join poprog_header ph on podc.po_details_poprog_id = ph.poprog_id
left join inventory_location_master ilm_from on ilm_from.id= stock_transfer_from_id
left join inventory_location_master ilm_to on ilm_to.id= stock_transfer_to_id
left join woprog wo_from on wo_from.WOProg_ID = stock_transfer_from_id
left join woprog wo_to on wo_to.WOProg_ID = stock_transfer_to_id


WHERE
 " + where + @" 
ORDER BY
ss.stock_transfer_date_added DESC
limit 100)";
		if (search != "")
			{

			sql_annual_counts = @"

(Select l.associated_alt_table_id master_id,
ide.desc_full_cdn description,
l.dt _date,
concat(mm.member_fullname, ' changed qty at ',ilm_from.`name`,' from ', l.value_old, ' to ', l.value_new) note


from `log` l
left join inventory_description ide on ide.master_id= l.associated_alt_table_id
left join inventory_location il on il.id = l.associated_table_id
left join inventory_location_master ilm_from on ilm_from.id= il.location_master_id
left join member mm on mm.member_id = l.member_id

where l.section_id=10 and l.action_id = 3 and l.business_unit_id = " + WarehouseBusinessUnit.id + @"  and l.dt > '" + branchOptionsObject.annual_count_start_date.ToString("yyyy-MM-dd") + @"' 
AND
(l.associated_alt_table_id like'%" + search + "%' or ide.desc_full_cdn description like '%" + search + @"%' or
mm.member_fullname like '%" + search + @"%' or l.dt like '%" + search + @"%')

order by l.dt desc limit 100)

union";


			sql = @"

(Select ss.stock_transfer_master_id master_id,
ss.stock_transfer_description description,
ss.stock_transfer_date_added _date,
if(ss.stock_transfer_type = 6,Concat(member.member_fullname,if(ss.stock_transfer_quantity>0,' received ','returned'),abs(ss.stock_transfer_quantity ),if(ss.stock_transfer_quantity>0,' from PO: ', ' back to'),ph.poprog_bvpo,if(ss.stock_transfer_quantity>0,' to WO: ',' from WO:'),wo_to.woprog_bvwo),
Concat(member.member_fullname,' moved ',abs(ss.stock_transfer_quantity) ,' from (',if(ss.stock_transfer_quantity>0,ifnull(ilm_from.`name`,concat('wo-',wo_from.woprog_bvwo)),ifnull(ilm_to.`name`,concat('wo-',wo_to.woprog_bvwo))),') to (',if(ss.stock_transfer_quantity>0,ifnull(ilm_to.`name`,concat('wo-',wo_to.woprog_bvwo)),ifnull(ilm_from.`name`,concat('wo-',wo_from.woprog_bvwo))),')')) note

FROM
stock_transfer AS ss
INNER JOIN stock_transfer_type ON ss.stock_transfer_type = stock_transfer_type.id
INNER JOIN member on member.Member_ID = ss.stock_transfer_member_id
left join po_details_current podc on podc.po_details_id = stock_transfer_from_id
left join poprog_header ph on podc.po_details_poprog_id = ph.poprog_id
left join inventory_location_master ilm_from on ilm_from.id= stock_transfer_from_id
left join inventory_location_master ilm_to on ilm_to.id= stock_transfer_to_id
left join woprog wo_from on wo_from.WOProg_ID = stock_transfer_from_id
left join woprog wo_to on wo_to.WOProg_ID = stock_transfer_to_id

WHERE
 " + where + @" 
AND
(ss.stock_transfer_master_id like'%" + search + "%' or stock_transfer_description like '%" + search + @"%' or wo_from.woprog_bvwo like '%" + search + @"%' 
or wo_from.woprog_customername like '%" + search + @"%'  or wo_to.woprog_bvwo like '%" + search + @"%' or wo_to.woprog_customername like '%" + search + @"%' or
member.member_fullname like '%" + search + @"%' or ss.stock_transfer_date_added like '%" + search + @"%')
ORDER BY
ss.stock_transfer_date_added DESC
limit 100)";
			}


		rpt_history.DataSource = branchOptionsObject.annual_count_on ? Toolbox.doSQL_dt(sql_annual_counts + sql, null) : Toolbox.doSQL_dt(sql, null);
		rpt_history.DataBind();

		if (search != "")
			{
			var ctrl = rpt_history.Controls[0].Controls[0].FindControl("tb_history_search");
			var tb_search = (TextBox)ctrl;
			tb_search.Text = search;
			}

		}

	#endregion

	#region po tab stuff

	protected void load_po_repeater(string search, bool initialLoad)
		{
		if(initialLoad)
			{ 
			load_page();
			}
		if (ddl_wo.Value != null)
			{
			var sql = @"
SELECT 
	a.po_details_part_no master_id, 
	a.po_details_description description,  
	(TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM a.po_details_qty_ordered*a.po_details_vendor_qty_per)AS char)))) po_qty, 
	(TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM ((a.po_details_qty_ordered*a.po_details_vendor_qty_per)-(a.po_details_qty_received*a.po_details_vendor_qty_per)))AS char)))) still_needed_qty ,
	IFNULL((SELECT qty FROM inventory_location il_default INNER JOIN inventory_location_master ilm ON ilm.id = il_default.location_master_id WHERE il_default.master_id = a.po_details_part_no and il_default.business_unit_id=b.business_unit_id and ilm.type_id = 1 order by qty desc,min_qty desc, max_qty desc limit 1),0) internal_qty,
	IFNULL((SELECT il.qty FROM inventory_location il WHERE il.location_master_id = " + (ddl_trucks.Value ?? 0) + @"  and il.master_id=a.po_details_part_no),0) truck_qty,
	a.po_details_id line_id,
	if(it.is_exclude=0 and it.allowed_to_stock=1 and a.po_details_line_active=1,true,false) lck,
	if(a.po_details_line_active=1,true,false) wo_lck,
	Concat('PO:',po_details_poprog_id,'-',vendor.vendor_name) po_desc
FROM 
	po_details_current a
LEFT JOIN 
	poprog_header b on b.poprog_id = a.po_details_poprog_id
LEFT JOIN 
	inventory_description c ON c.master_id = a.po_details_part_no
LEFT JOIN 
	inventory_branch d on d.master_id = c.master_id and d.business_unit_id = " + WarehouseBusinessUnit.id + @"  
LEFT JOIN 
	inventory_item_master iim on a.po_details_part_no = iim.master_id 
LEFT JOIN 
	inventory_tag it on iim.tag_id = it.tag_id 
LEFT JOIN 
	vendor e on e.vendor_id = b.poprog_vendor_id
WHERE 
	b.poprog_status in (3,4,6,12) and 
	a.po_details_woprog_id = " + ddl_wo.Value + @" and 
	a.is_gl_account = false  and (( it.active = 1 ) ) 
ORDER BY 
	a.po_details_qty_ordered  desc";
			// where wo_detail_current.wo_detail_current_woprog_id = " + ddl_wo.Value + @"  and ((it.is_exclude=0 and it.allowed_to_stock = 1 and it.active = 1 and iim.approved=1) or wo_detail_current.wo_detail_current_master_id = 777) 

			if (search != "")
				{
				sql = @"Select po_details_current.po_details_part_no master_id, po_details_current.po_details_description description,  
(TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM po_details_current.po_details_qty_ordered*po_details_current.po_details_vendor_qty_per)AS char)))) po_qty, 
(TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM ((po_details_current.po_details_qty_ordered*po_details_current.po_details_vendor_qty_per)-(po_details_current.po_details_qty_received*po_details_current.po_details_vendor_qty_per)))AS char)))) still_needed_qty ,
ifnull((Select qty from inventory_location il_default inner join inventory_location_master ilm on ilm.id=il_default.location_master_id where il_default.master_id = po_details_current.po_details_part_no and il_default.business_unit_id=poprog_header.business_unit_id and ilm.type_id = 1 order by qty desc,min_qty desc, max_qty desc limit 1),0) internal_qty,
ifnull((Select il.qty from inventory_location il where il.location_master_id = " + (ddl_trucks.Value ?? 0) + @"  and il.master_id=po_details_current.po_details_part_no),0) truck_qty,
po_details_current.po_details_id line_id,
if(it.is_exclude=0 and it.allowed_to_stock=1 and po_details_line_active=1,true,false) lck,
if(po_details_line_active=1,true,false) wo_lck,
Concat('PO:',po_details_poprog_id,'-',vendor.vendor_name) po_desc
from po_details_current 
LEFT join poprog_header on poprog_header.poprog_id = po_details_current.po_details_poprog_id
LEFT JOIN inventory_description ON inventory_description.master_id = po_details_current.po_details_part_no
LEFT join inventory_branch on inventory_branch.master_id =inventory_description.master_id and inventory_branch.business_unit_id = " + WarehouseBusinessUnit.id + @"  
LEFT join inventory_item_master iim on po_details_current.po_details_part_no = iim.master_id 
LEFT join inventory_tag it on iim.tag_id = it.tag_id 
LEFT join vendor on vendor.vendor_id = poprog_header.poprog_vendor_id
 where  poprog_header.poprog_status in (3,4,6,12) 
and po_details_current.po_details_woprog_id = " + ddl_wo.Value + @" and po_details_current.is_gl_account = false  
						and (inventory_description.master_id like'%" + search + "%' or po_details_vendor_part_no like '%" + search + @"%' or inventory_description.description like '%" + search + @"%')
and (( it.active = 1 )) 
				order by po_details_current.po_details_qty_ordered  desc";
				//and ((it.is_exclude=0 and it.allowed_to_stock = 1 and it.active = 1 and iim.approved=1) or wo_detail_current.wo_detail_current_master_id = 777) 
				}
			rpt_po.DataSource = Toolbox.doSQL_dt(sql, null);
			rpt_po.DataBind();

			if (search != "")
				{
				var ctrl = rpt_po.Controls[0].Controls[0].FindControl("tb_po_search");
				var tb_search = (TextBox)ctrl;
				tb_search.Text = search;
				}
			}
		}
		
	protected void load_status_repeater(string sort, bool initialLoad)
		{
		if(initialLoad)
			{ 
			load_page();
			}
		var orderBy = "";
		switch(sort)
			{
			case "1":
				orderBy = "rec_no";
			break;
			case "2":
				orderBy = "master_id";
			break;
			case "3":
				orderBy = "description";
			break;
			case "4":
				orderBy = "qty_committed / qty_ordered";
			break;
			}
		rpt_status.DataSource	= Toolbox.doSQL_dt($@"SELECT rec_no, master_id, description, ROUND(qty_committed,2) qty_committed, ROUND(qty_ordered, 2) qty_ordered, ROUND(IF((qty_committed / qty_ordered) * 100 > 100, 100, (qty_committed / qty_ordered) * 100), 2) width FROM wo_detail WHERE woprog_id = @v0 AND type = 'M' ORDER BY {orderBy} {(chkAscending.Checked ? "ASC" : "DESC")}", new []{ddl_wo.Value});
		rpt_status.DataBind();

		}

	protected void btn_po_save_Click(object sender, EventArgs e)
		{
		var woprog_id = ddl_wo.Value;
		var i = new inventory();
		NeWOProg wo;
		var should_process = true;
		Session["mobile_rpt_wo_progid"] = null;

		if (ddl_wo.Value != null)
			{
			wo = new NeWOProg(Convert.ToInt32(ddl_wo.Value));
			Session["mobile_rpt_wo_progid"] = wo;
			foreach (RepeaterItem item in rpt_po.Items)
				{
				var did_lock = false;
				should_process = true;
				if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
					{
					var master_id = (HtmlGenericControl)item.FindControl("rpt_po_master_id");
					var tk_to_wo = (TextBox)item.FindControl("TextBox8");
					var tk_to_stock = (TextBox)item.FindControl("TextBox9");
					var tk_to_truck = (TextBox)item.FindControl("TextBox10");
					double qty_rec = 0;
					if (tk_to_stock.Text != "" && tk_to_stock.Text != "0")  // receive to stock
						{
						// find default internal location

						double.TryParse(tk_to_stock.Text, out qty_rec);
						var newinv = new inventory();
						var il = new location(branch.get_default_internal_location(WarehouseBusinessUnit.id, Convert.ToInt32(master_id.InnerText)));
						//	var column_location = il.id;
						var details = new NEPO_Details_Current();

						var line_id = tk_to_wo.ToolTip;
						details.po_details_current_line(Convert.ToInt32(line_id));
						var poprogress = new NePOProg(details.po_details_poprog_id);
						receive_po_item(Convert.ToInt32(line_id), il, qty_rec, "stock", current_user);
						tk_to_stock.Text = "";
						}
					if (tk_to_truck.Text != "" && tk_to_truck.Text != "0" && ddl_trucks.Value != null)  // receive to truck
						{
						double.TryParse(tk_to_truck.Text, out qty_rec);
						var newinv = new inventory();
						var il = new location(Convert.ToInt32(ddl_trucks.Value), WarehouseBusinessUnit.id, Convert.ToInt32(master_id.InnerText));
						//	var column_location = il.id;
						var details = new NEPO_Details_Current();

						var line_id = tk_to_wo.ToolTip;
						details.po_details_current_line(Convert.ToInt32(line_id));
						var poprogress = new NePOProg(details.po_details_poprog_id);
						receive_po_item(Convert.ToInt32(line_id), il, qty_rec, "truck", current_user);
						tk_to_truck.Text = "";
						}
					if (tk_to_wo.Text != "" && tk_to_wo.Text != "0")  // receive to wo
						{
						double.TryParse(tk_to_wo.Text, out qty_rec);
						var newinv = new inventory();
						var details = new NEPO_Details_Current();

						var line_id = tk_to_wo.ToolTip;
						details.po_details_current_line(Convert.ToInt32(line_id));
						var poprogress = new NePOProg(details.po_details_poprog_id);
						receive_po_item(Convert.ToInt32(line_id), wo, qty_rec, "wo", current_user);
						tk_to_wo.Text = "";
						}
					}
				}
			}
		var ctrl = rpt_po.Controls[0].Controls[0].FindControl("tb_po_search");
		var tb_search = (TextBox)ctrl;

		load_po_repeater(tb_search.Text, false);
		}



	protected void receive_po_item(int po_line_id, object to, double qty, string to_type, NeMember myMember)
		{
		#region validate
		if (po_line_id <= 0 || to == null || qty == 0 || to_type == "")
			{
			return;
			}
		NEPO_Details_Current podc;
		location il = null;
		inventory i;
		NeWOProg wo = null;
		NePOProg poprogress;

		string bvpo;
		var recordschanged = 0;

		var dsn = "";
		try
			{
			podc = new NEPO_Details_Current();
			podc.po_details_current_line(po_line_id);
			podc.po_details_id = po_line_id;
			if (podc.po_details_line_active == 0)
				{
				return;
				}
			switch (to_type)
				{
				case "wo":
					wo = (NeWOProg)to;
					break;
				case "truck":
					il = (location)to;
					break;
				case "stock":
					il = (location)to;
					break;
				}
			poprogress = new NePOProg(podc.po_details_poprog_id);
			i = new inventory();
			i.Load(podc.po_details_part_no, WarehouseBusinessUnit.id);
			dsn = WorkingBusinessUnit.DSN;
			}
		catch
			{
			return;
			}
		#endregion

		//var lockinfo = PurchaseOrder.CheckPOLock(dsn, poprogress.poprog_bvpo);
		//if (lockinfo != "" && !lockinfo.Contains("MH") && lockinfo!=current_user.Initials)
		//		{
		//			throw new Exception("This PO is locked by user " + lockinfo + " and cannot be changed at this moment");
		//		}
		//		PurchaseOrder.UnlockPO(dsn, poprogress.poprog_bvpo, myMember.Initials);
		//		PurchaseOrder.LockOrder(dsn, poprogress.poprog_bvpo, myMember.Initials);
		bvpo = poprogress.poprog_bvpo;
		recordschanged++;

		var GLAccName = podc.is_gl_account ? Toolbox.doSQL_string(@"SELECT IFNULL(MAX(gl_te.account_no),'') FROM gl_te WHERE gl_te.id = @v0", podc.po_details_woprog_id) : "";
		qty = qty / podc.po_details_vendor_qty_per;

		var qty_rec = qty;
		var old_qty_received = podc.po_details_qty_received;
		var totqty = podc.po_details_qty_received + qty;
		var newworkorderid = podc.po_details_woprog_id.ToString();
		var newpartnumber = podc.po_details_part_no.ToString();

		var newdescription = i.description_full;
		var new_qty_received = qty;
		var newqty = podc.po_details_qty_orderd;


		qty_rec = podc.po_details_qty_received + qty_rec;
		var qtyRecHold = podc.po_details_qty_received;
		podc.po_details_qty_received = qty_rec;

		#region add note if the quantity received is greater than the quantity ordered

		if (qty_rec > podc.po_details_qty_orderd)
			{
			//Create Note
			var eventtext = string.Format("{0} received {2} of {1} when the PO had {2} on order.", myMember.FullName, podc.po_details_part_no,
				qty_rec, podc.po_details_qty_orderd);
			try
				{
				Toolbox.doSQL_void(@"
INSERT INTO poprog_notes 
	(
	poprog_id, 
	eventtext, 
	date, 
	member_id
	)
VALUES
	(
	@v0, 
	@v1, 
	now(), 
	@v2
	)", new object[] {
						poprogress.poprog_id,
						eventtext,
						myMember.id});
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				throw;
				}
			}

		#endregion add note if the quantity received is greater than the quantity ordered

		podc.po_details_line_active = podc.po_details_qty_orderd == podc.po_details_qty_received ? 0 : 1;

		#region save to purchase order
		try
			{
			podc.PO_Details_Update();
			}
		catch (Exception ee)
			{
			Toolbox.do_errorLog_errorStack(ee);
			throw;
			}
		#endregion save to purchase order

		try
			{
			if (to_type == "wo" && podc.woprog_id > 20000 && !podc.is_gl_account && qty > 0)
				{
				#region save & commit to work order

				var addnew = Toolbox.doSQL_int(@"SELECT count(wo_detail_current_id) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1  and wo_detail_current_master_id != 0", new object[] { wo.woprog_id, podc.po_details_part_no }) == 0;
				var wodetails = new NeWODetailCurrent();
				if (!addnew)
					{
					wodetails = new NeWODetailCurrent(Toolbox.doSQL_int(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1  and wo_detail_current_master_id != 0", new object[] { wo.woprog_id, podc.po_details_part_no }));
					}
				podc.MoveToWOProgDetails(poprogress.poprog_bvpo, podc.po_details_poprog_id.ToString(), podc.po_details_id.ToString(), myMember.id.ToString(), new_qty_received * podc.po_details_vendor_qty_per, true);
				var recnumber = "";
				try
					{
					recnumber =
						Toolbox.doSQL_string(
							@"SELECT wo_detail_current_rec_no FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0 and wo_detail_current_master_id = @v1",
							new object[] { podc.woprog_id, podc.po_details_part_no });
					}
				catch (Exception ee)
					{
					throw ee;
					}

				#endregion save & commit to work order

				#region save audit trail

				try
					{
					var changes = new NeWOProgChanges();
					changes.WoProgChanges_BVWO = wo.OrderNumber;
					changes.WoProgChanges_BVWORec = recnumber;
					changes.WoProgChanges_WOProg_ID = Convert.ToInt32(wo.woprog_id);
					changes.WoProgChanges_Modified_Member_ID = current_user.id;
					changes.WOProgChanges_ManualPriceChange = 0;

					var _checksell = wo.use_fixed_material_markup ? wo.fixed_material_markup * (podc.po_details_cost / podc.po_details_vendor_qty_per) : shared.GetSellPrice(podc.po_details_cost / podc.po_details_vendor_qty_per, 0, true, qty + (addnew ? 0 : wodetails.qty_committed), wodetails.business_unit_id);

					if (!addnew)
						{
						changes.WoProgChanges_WasPartNo = podc.po_details_part_no.ToString();
						changes.WoProgChanges_WasPrice = wodetails.sell.ToString();
						}
					else
						{
						changes.WoProgChanges_WasPrice = _checksell.ToString();
						}
					changes.WoProgChanges_WasPartNo = podc.po_details_part_no.ToString();
					changes.WoProgChanges_WasPrice = wodetails.sell.ToString();
					changes.WoProgChanges_WasQty = addnew ? "0" : wodetails.qty_committed.ToString();
					changes.WoProgChanges_WasDesc = wodetails.description;
					changes.WOProgChanges_WasBillingType = wodetails.billtypeid;

					changes.WoProgChanges_IsPrice = _checksell.ToString();
					changes.WoProgChanges_IsPartNo = newpartnumber;
					changes.WoProgChanges_IsQty = (wodetails.qty_committed + new_qty_received * podc.po_details_vendor_qty_per).ToString();
					changes.WoProgChanges_DeleteFlag = "false";
					changes.WoProgChanges_IsDesc = wodetails.description;
					changes.WoProgChanges_IsBillingType = wodetails.billtypeid;

					changes.FullWOComment = "Received " + new_qty_received * podc.po_details_vendor_qty_per + " from PO " + poprogress.poprog_bvpo;
					changes.WOProgChanges_OrderedQty = wodetails.qty_ordered.ToString();
					changes.was_req_qty = wodetails.qty_ordered;
					var _isqty = Convert.ToDouble(changes.WoProgChanges_IsQty);
					var _wasqty = Convert.ToDouble(changes.WoProgChanges_WasQty);
					var _diff = _wasqty - _isqty;


					changes.business_unit_id = WorkingBusinessUnit.id;
					try
						{
						changes.AddtoWOProgChanges();
						}
					catch (Exception Ex)
						{
						throw new Exception(Ex.ToString());
						}


					}
				catch (Exception ee)
					{
					Toolbox.do_errorLog_errorStack(ee);
					throw;
					}
				//PurchaseOrder.UnlockPO(dsn, poprogress.poprog_bvpo, myMember.Initials);
				#endregion save audit trail

				#region Save Notes

				var to_stock = new Nestock_transfer();
				to_stock.master_id = podc.po_details_part_no;
				if (new_qty_received > 0) // receiving in goes to the wo
					{
					to_stock.from_id = po_line_id;
					to_stock.to_id = wo.woprog_id;
					to_stock.to_location_id = wo.woprog_id;
					to_stock.type_id = 6; // From PO TO wo
					}
				else // receiving back, REMOVES it from the wo, goes back to the PO
					{
					to_stock.from_id = il.location_master_id;
					to_stock.from_location_id = to_stock.from_id;
					to_stock.to_id = po_line_id;
					to_stock.type_id = 7; // From wo to PO
					}
				to_stock.quantity = new_qty_received * podc.po_details_vendor_qty_per; //quantity that is received into stock
				to_stock.description = wodetails.description;
				to_stock.member_id = myMember.id;
				to_stock.note = "PO Received to WO";
				to_stock.business_unit_id = poprogress.business_unit_id;
				to_stock.cost = podc.po_details_cost / podc.po_details_vendor_qty_per;
				to_stock.po_line_id = po_line_id;
				if (to_stock.quantity != 0)
					{
					to_stock.Nestock_transfer_save();
					}

				if (podc.po_details_woprog_id != 0 && to_type == "wo")
					{
					try
						{
						//var _notes = new bv_notes();
						//_notes.prog = poprogress.poprog_bvpo;
						//_notes.item = "PORD";
						//_notes.n_user = myMember.Initials;
						//_notes.subject = string.Format("{0}: {1} received", podc.po_details_part_no, qty);
						//_notes.detail = string.Format("{0}: {1} received against WO: {2}", podc.po_details_part_no, qty, wo.OrderNumber);
						//_notes.Save(dsn);

						var prognotes = new NePOProg();
						prognotes.eventtext = string.Format("{0}: {1} received against WO: {2}", podc.po_details_part_no, qty,
							wo.OrderNumber);
						prognotes.notes_poprogid = poprogress.poprog_id;
						prognotes.poprog_bvpo = poprogress.poprog_bvpo;
						prognotes.notes_memberid = myMember.id;
						prognotes.business_unit_id = poprogress.business_unit_id;
						prognotes.SaveNotes();
						}
					catch (Exception ee)
						{
						Toolbox.do_errorLog_errorStack(ee);
						}
					}

				#endregion Save Notes
				}

			if (!(to_type == "wo") && !podc.is_gl_account)
				{
				#region if we aren't committing directly to the workorder, update stock info
				var to_stock = new Nestock_transfer();
				to_stock.master_id = podc.po_details_part_no;
				if (new_qty_received > 0) // receiving in goes INTO inventory
					{
					to_stock.from_id = po_line_id;
					to_stock.to_id = il.location_master_id;
					to_stock.to_location_id = to_stock.to_id;
					to_stock.type_id = 2; // From PO TO inventory
					}
				else // receiving back, REMOVES it from inventory, goes into the ether
					{
					to_stock.from_id = il.location_master_id;
					to_stock.from_location_id = to_stock.from_id;
					to_stock.to_id = po_line_id;
					to_stock.type_id = 5; // From Inventory TO ether
					}
				to_stock.quantity = new_qty_received * podc.po_details_vendor_qty_per; //quantity that is received into stock
				to_stock.description = newdescription;
				to_stock.member_id = myMember.id;
				to_stock.note = "PO Quantity Updated";
				to_stock.business_unit_id = poprogress.business_unit_id;
				to_stock.cost = podc.po_details_cost / podc.po_details_vendor_qty_per;
				to_stock.po_line_id = po_line_id;
				if (to_stock.quantity != 0)
					{
					to_stock.Nestock_transfer_save();
					}
				#endregion
				}

			//PurchaseOrder.UnlockPO(dsn, poprogress.poprog_bvpo, myMember.Initials);

			}
		catch
			{
			#region throw errors

			podc.po_details_qty_received = qtyRecHold;
			podc.po_details_line_active = 1;
			try
				{
				podc.PO_Details_Update();
				}
			catch (Exception ee)
				{
				//PurchaseOrder.UnlockPO(dsn, poprogress.poprog_bvpo, myMember.Initials);
				Toolbox.do_errorLog_errorStack(ee);
				throw;
				}
			//PurchaseOrder.UnlockPO(dsn, poprogress.poprog_bvpo, myMember.Initials);
			throw;

			#endregion throw errors

			}
		// end if (qtyRec != 0 && _complete == "1")

		}


	#endregion





	protected void update_req_line(int line_id, double qty)
		{
		Toolbox.doSQL_void(@"update wo_detail_current a 
set a.wo_detail_current_qty_ordered=(wo_detail_current_qty_committed + " + qty + ") where a.wo_detail_current_id = " + line_id + " limit 1");

		}

	protected void add_part_on_wo(int woprog_id, int master_id, double add_qty, string req_or_committed, int member_id, int master_location_id)
		{
		// if this is a transfer, the master_location_id is the detail line i
		NeWOProg wo;
		NeWOProg old_wo;
		if (req_or_committed == "tra")
			{
			try
				{
				// load destination wo
				if (Session["mobile_rpt_other_woprog_id"] != null)
					wo = (NeWOProg)Session["mobile_rpt_other_woprog_id"];
				else
					{
					wo = new NeWOProg(woprog_id);
					Session["mobile_rpt_other_woprog_id"] = wo;
					}

				//loAD origin work order
				if (Session["mobile_rpt_wo_progid"] != null)
					old_wo = (NeWOProg)Session["mobile_rpt_wo_progid"];
				else
					{
					var x = Toolbox.doSQL_int(@"Select wo_detail_current_woprog_id from wo_detail_current  where wo_detail_current_id=@v0 limit 1 ", new object[] { master_location_id });
					old_wo = new NeWOProg(x);
					Session["mobile_rpt_wo_progid"] = old_wo;
					}

				if (old_wo.Status == "Invoiced" || old_wo.Status == OpsWOStatus.WaitingToBeInvoiced || old_wo.Status == "Waiting For PO")
					{
					xfer_lb_warning.Text = "You can not add or alter an item on " + old_wo.OrderNumber + " because it has been invoiced/is waiting to be invoiced.";
					return;
					}
				}
			catch
				{
				xfer_lb_warning.Text = "Invalid wo";
				ScriptManager.RegisterStartupScript(this, GetType(), ClientID, string.Format("alert('Invalid WO')", "Server"), true);
				return;
				}
			}
		else
			{
			old_wo = null;
			try
				{
				if (Session["mobile_rpt_wo_progid"] != null)
					wo = (NeWOProg)Session["mobile_rpt_wo_progid"];
				else
					{
					wo = new NeWOProg(woprog_id);
					Session["mobile_rpt_wo_progid"] = wo;
					}
				}
			catch
				{
				xfer_lb_warning.Text = "Invalid wo";
				ScriptManager.RegisterStartupScript(this, GetType(), ClientID, string.Format("alert('Invalid WO')", "Server"), true);
				return;
				}
			}
		#region validation

		if (wo.Status == "Invoiced" || wo.Status == OpsWOStatus.WaitingToBeInvoiced || wo.Status == "Waiting For PO")
			{
			xfer_lb_warning.Text = "You can not add or alter an item on this work order because it has been invoiced/is waiting to be invoiced.";
			return;
			}
		if (master_id == OpsSpecialPart.MakeThisPart && add_qty != 0)
			{
			//		xfer_lb_warning.Text = "Error";
			//		ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('777 can only be added to the required amounts on a WO')", "Server"), true);
			//		return;
			}

		#endregion

		#region prep_variables

		var inv = new inventory();
		inv.Load(master_id, WarehouseBusinessUnit.id);
		var dbl_com_qty_on_wo = Toolbox.doSQL_double(@"select ifnull((SELECT wo_detail_current_qty_committed FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_master_id =@v1  limit 1),0) ", new object[] { wo.woprog_id, master_id });
		var dbl_required_on_wo = Toolbox.doSQL_double(@"select ifnull((SELECT wo_detail_current_qty_ordered FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_master_id =@v1  limit 1),0) ", new object[] { wo.woprog_id, master_id });

		if (req_or_committed == "req")
			{
			if (dbl_required_on_wo - dbl_com_qty_on_wo == add_qty)
				{
				inv = null;

				return;
				}
			}
		// wo is the destination work oreer.. in the case of a transfer
		var detail = new NeWODetailCurrent();
		var old_detail = new NeWODetailCurrent();
		var so = new NeSalesOrder();
		var cust = new NECustomer(Convert.ToInt32(wo.WOProg_Customer_ID));

		if (req_or_committed == "tra")
			{
			old_detail = new NeWODetailCurrent(master_location_id);
			}

		var line_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(wo_detail_current_id), 0) FROM wo_detail_current 
WHERE wo_detail_current_woprog_id = '" + wo.woprog_id + "' AND wo_detail_current_master_id = '" + master_id + "' LIMIT 1");
		if (line_id != 0 && master_id != OpsSpecialPart.MakeThisPart)
			{
			detail = new NeWODetailCurrent(line_id);

			}

		#endregion

		#region write data to wo

		so.working_line_id = master_id == OpsSpecialPart.MakeThisPart ? 0 : line_id;
		so.forcenewpartline = false;
		so.PartNo = master_id.ToString();

		var bd = new bingo_data();
		bd.before_cost = detail.cost;
		bd.before_qty = detail.qty_committed;
		bd.before_sell = detail.sell;
		bd.detail_id = detail.id;
		bd.master_id = detail.master_id;
		bd.woprog_id = detail.woprog_id;
		bd.ca_qty = add_qty;
		var do_bd_save = true;
		so.Desc = string.IsNullOrEmpty(detail.description) ? inv.description_int : detail.description;
		if (req_or_committed == "req") // Cost or sell should not be changed 
			{
			so.OrderedQuantity = add_qty;
			so.ManualChange = true;
			//	so.DateRequired = d.ToString("yyyy-MM-dd");   ** matt to fill in 
			so.SellOverride = master_id == OpsSpecialPart.MakeThisPart ? 0 : detail.sell;
			so.CostOverRide = master_id == OpsSpecialPart.MakeThisPart ? 0 : detail.cost;
			so.forcenewpartline = master_id == OpsSpecialPart.MakeThisPart;
			do_bd_save = false;
			}
		else if (req_or_committed == "tra")
			{
			so.Quantity = add_qty;
			double temp_cost = 0;
			double temp_qty = 0;

			temp_qty = dbl_com_qty_on_wo + so.Quantity <= 0 ? 1 : dbl_com_qty_on_wo + so.Quantity;
			temp_cost = old_detail.cost; // get cost from existing work order line
			bd.ca_cost = temp_cost;
			if (dbl_required_on_wo < temp_qty && so.Quantity > 0)
				{
				so.OrderedQuantity = temp_qty - dbl_required_on_wo;
				}
			else
				{
				so.OrderedQuantity = 0;
				}
			so.CostOverRide = temp_cost;

			so.SellPrice = 0;
			so.SellOverride = 0;
			so.ManualChange = false;
			//	so.SellOverride = shared.GetSellPrice(so.CostOverRide, 0, inv.is_qty, temp_qty, detail.business_unit_id);
			so.transfer_to_line_id = line_id;

			}
		else if (req_or_committed == "com")
			{
			so.Quantity = add_qty;
			double temp_cost = 0;
			double temp_qty = 0;
			if (!inv.allowed_to_stock && !inv.is_exclude)
				{
				temp_qty = dbl_com_qty_on_wo + so.Quantity <= 0 ? 1 : dbl_com_qty_on_wo + so.Quantity;
				temp_cost = Math.Round(Toolbox.doSQL_double(@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, @v2 )", new object[] { detail.master_id, WarehouseBusinessUnit.id, temp_qty }), 3);
				if (detail.qty_committed > 0)
					{
					temp_cost = Math.Round(Toolbox.doSQL_double(@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { 3, detail.woprog_id, detail.master_id, so.Quantity, temp_cost }), 3);
					}
				bd.ca_cost = temp_cost;
				}
			else
				{
				temp_qty = dbl_com_qty_on_wo + so.Quantity;
				if (so.Quantity > 0 && detail.qty_committed > 0 && detail.cost != inv.cost_price_branch)
					{
					temp_cost = Math.Round(Toolbox.doSQL_double(@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { detail.woprog_id, detail.master_id, so.Quantity, inv.cost_price_branch }), 3);
					bd.ca_cost = temp_cost;
					}
				else
					{
					temp_cost = Math.Round(inv.cost_price_branch, 2, MidpointRounding.AwayFromZero);
					bd.ca_cost = temp_cost;
					}
				}
			if (dbl_required_on_wo < temp_qty && so.Quantity > 0)
				{
				so.OrderedQuantity = temp_qty - dbl_required_on_wo;
				}
			else
				{
				so.OrderedQuantity = 0;
				}
			so.CostOverRide = temp_cost;
			so.SellOverride = wo.use_fixed_material_markup ? wo.fixed_material_markup * so.CostOverRide : shared.GetSellPrice(so.CostOverRide, 0, inv.is_qty, temp_qty, detail.business_unit_id);
			so.SellPrice = so.SellOverride;
			//so.RetailCost = shared.GetSellPrice(so.CostOverRide, 0, inv.is_qty, temp_qty, detail.business_unit_id);
			}
		else if (req_or_committed == "ret")
			{
			so.OrderedQuantity = Math.Abs(add_qty) * -1;
			so.Quantity = Math.Abs(add_qty) * -1;
			so.ActualQuantity = Math.Abs(add_qty);
			double temp_cost = 0;
			double temp_qty = 0;
			if (!inv.allowed_to_stock && !inv.is_exclude)
				{
				temp_qty = dbl_com_qty_on_wo + so.Quantity <= 0 ? 1 : dbl_com_qty_on_wo + so.Quantity;
				temp_cost = Math.Round(Toolbox.doSQL_double(@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, @v2 )", new object[] { detail.master_id, WarehouseBusinessUnit.id, temp_qty }), 3);
				if (detail.qty_committed > 0)
					{
					temp_cost = Math.Round(Toolbox.doSQL_double(@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { detail.woprog_id, detail.master_id, so.Quantity, temp_cost }), 3);
					}
				}
			else
				{
				temp_qty = dbl_com_qty_on_wo + so.Quantity;
				temp_cost = detail.cost;
				}
			so.CostOverRide = temp_cost;
			bd.ca_cost = temp_cost;
			bd.ca_qty = temp_qty;
			so.SellOverride = wo.use_fixed_material_markup ? wo.fixed_material_markup * so.CostOverRide : shared.GetSellPrice(so.CostOverRide, 0, inv.is_qty, temp_qty, detail.business_unit_id);
			so.SellPrice = wo.use_fixed_material_markup ? wo.fixed_material_markup * so.CostOverRide : shared.GetSellPrice(so.CostOverRide, 0, inv.is_qty, temp_qty, detail.business_unit_id);
			}
		if (do_bd_save)
			{
			bd.ca_sell = so.SellOverride;
			bd.after_cost = so.CostOverRide;
			bd.after_sell = so.SellOverride;
			bd.after_qty = dbl_com_qty_on_wo + so.Quantity;
			bd.save();
			}
		so.BillingTypeID = wo.QuoteID != "0" ? 1 : 0;
		so.CustomerDiscount = wo.woprog_apply_discount == 0 ? 0 : Convert.ToDouble(cust.Discount);
		var ilm = new location_master();
		if (master_id != OpsSpecialPart.MakeThisPart)
			{
			so.Desc = detail.description == null ? inv.description : detail.description;
			switch (req_or_committed)
				{
				case "com":
					ilm = new location_master(master_location_id);
					so.committing_from_location = ilm.name;
					break;
				case "ret":
					ilm = new location_master(master_location_id);
					so.uncommitting_to_location = ilm.name;
					break;
				case "tra":
					so.Desc = old_detail.description;
					so.committing_from_location = "WO: " + old_detail.woprog_id;
					so.OriginWorkOrder = old_wo.OrderNumber;

					break;
				}
			}
		else
			{
			//	so.Desc = tb_777_description.Text;  * matt to figure out
			}
		var recnumber = 0;

		try
			{

			recnumber = so.SavePart(wo.woprog_id, req_or_committed == "tra" ? "TRUE" : "FALSE", WorkingBusinessUnit.DSN, current_user.FullName, current_user.id, line_id == 0 || master_id == OpsSpecialPart.MakeThisPart, "");

			if (master_id != OpsSpecialPart.MakeThisPart)
				{
				var woc = new NeWOProgChanges
					{
					WoProgChanges_WOProg_ID = wo.woprog_id,
					WoProgChanges_BVWO = wo.OrderNumber,
					WoProgChanges_BVWORec = recnumber.ToString(),
					WoProgChanges_DateTime = Toolbox.MySQLNow_long(),
					WoProgChanges_Modified_Member_ID = current_user.id,
					WoProgChanges_WOProgComment_ID = 0,
					WoProgChanges_WasPartNo = detail.master_id.ToString(),
					WoProgChanges_WasPrice = detail.sell.ToString(),
					WoProgChanges_WasQty = detail.qty_committed.ToString(),
					WoProgChanges_IsPartNo = so.PartNo,
					WoProgChanges_IsPrice = so.SellOverride.ToString(),
					WoProgChanges_IsQty = (detail.qty_committed + so.Quantity).ToString(),
					WoProgChanges_DeleteFlag = "false",
					WoProgChanges_WasDesc = detail.description,
					WoProgChanges_IsDesc = inv.description_int,
					business_unit_id = WorkingBusinessUnit.id,
					WOProgChanges_WasBillingType = detail.billtypeid,
					WoProgChanges_IsBillingType = so.BillingTypeID,
					WOProgChanges_OrderedQty = so.OrderedQuantity.ToString(),
					WOProgChanges_ManualPriceChange = 0,
					was_req_qty = detail.qty_ordered
					};
				if (req_or_committed == "com" || req_or_committed == "ret")
					{
					woc.FullWOComment = req_or_committed == "com" ? "Committed from Location: " + ilm.name : "Returned to Location: " + ilm.name;
					}
				else if (req_or_committed == "tra")
					{
					woc.FullWOComment = " - Transferred " + so.Quantity + " from work order: " + old_wo.OrderNumber;
					}
				woc.AddtoWOProgChanges();
				NeWOProg.update_header_totals(wo.woprog_id.ToString(), WorkingBusinessUnit.id, wo.OrderNumber);
				if (req_or_committed == "tra")
					{
					NeWOProg.update_header_totals(old_wo.woprog_id.ToString(), WorkingBusinessUnit.id, old_wo.OrderNumber);
					}
				}
			}
		catch (Exception ex)
			{
			throw ex;
			}
		if (Convert.ToInt32(master_id) < OpsSpecialPart.LaborThreshold && !inv.is_exclude && inv.allowed_to_stock)
			{

			var fromstock = new Nestock_transfer();
			fromstock.type_id = 1;
			fromstock.master_id = master_id;
			fromstock.description = inv.description;
			fromstock.business_unit_id = WarehouseBusinessUnit.id;
			fromstock.member_id = current_user.id;
			fromstock.stock_transfer_log_section_id = 19;

			if (req_or_committed == "ret")
				{
				fromstock.to_id = 0;
				fromstock.from_id = wo.woprog_id;
				fromstock.to_id = master_location_id; // int_ is the FROM ddl
				fromstock.to_location_id = master_location_id;

				fromstock.type_id = 4;
				fromstock.quantity = add_qty;
				fromstock.note = "Returned to stock via mobile";
				fromstock.cost = detail.cost > 0 ? detail.cost : inv.cost_price_branch;
				}

			else if (req_or_committed == "com")
				{
				fromstock.to_id = wo.woprog_id;
				fromstock.from_id = master_location_id; // int_ is the FROM ddl
				fromstock.from_location_id = master_location_id;
				fromstock.quantity = add_qty * -1;
				fromstock.note = "committed via mobile";
				fromstock.cost = inv.cost_price_branch;

				}
			if (req_or_committed != "req" && req_or_committed != "tra")
				{
				fromstock.Nestock_transfer_save();
				}
			}





		#endregion

		}

	protected void ddl_wo_Callback(object sender, CallbackEventArgsBase e)
		{
		if(ddl_wo.Value != null)
			{
			load_wo_repeater("", true);
			}
		}
	protected void cb_general_Callback(object source, CallbackEventArgs e)
		{
		if (e.Parameter == "")
			{
			Session["working_woprog_id"] = null;
			Session["mobile_rpt_wo_progid"] = null;
			}
		else if (e.Parameter != "")
			{
			Session["working_woprog_id"] = e.Parameter;
			}
		load_page();
		}


	protected void PartTransfer(string wo_line_id, int to_wo, double qty, string origin_bvwo)
		{
		NeWOProg wo;
		try
			{
			if (Session["mobile_rpt_other_woprog_id"] != null)
				wo = (NeWOProg)Session["mobile_rpt_other_woprog_id"];
			else
				{
				wo = new NeWOProg(to_wo);
				Session["mobile_rpt_other_woprog_id"] = wo;
				}
			}
		catch
			{
			xfer_lb_warning.Text = "Invalid wo";
			ScriptManager.RegisterStartupScript(this, GetType(), ClientID, string.Format("alert('Invalid TO WO')", "Server"), true);
			return;
			}

		var details = new NeWODetailCurrent(Convert.ToInt32(wo_line_id));
		var inv = new inventory();
		details.GetLineDetails(wo_line_id);
		if (details.type == OpsWOLineType.Material) // if its not a labour part
			{
			inv.Load(details.master_id, WarehouseBusinessUnit.id);
			}
		else
			{
			return;
			}
		var from_cost = details.cost;
		var from_qty = details.qty_committed;
		try
			{
			add_part_on_wo(to_wo, details.master_id, qty, "tra", current_user.id, Convert.ToInt32(wo_line_id));
			}
		catch
			{
			ScriptManager.RegisterStartupScript(this, GetType(), ClientID, string.Format("alert('Could not transfer to other WO')", "Server"), true);
			}

		// this is the original work order

		details.qty_committed = Math.Abs(qty) * -1;
		details.qty_invoiced = details.qty_committed;
		details.qty_ordered = Math.Abs(qty) * -1;
		details.sell = shared.GetSellPrice(details.cost, 0, inv.is_qty, from_qty + details.qty_committed, details.business_unit_id);
		details.unit = details.sell;
		details.transferring_from_bvwo = origin_bvwo;
		details.transferring_to_bvwo = wo.OrderNumber;
		details.is_transferring_from = false;
		details.is_transferring_to = true;
		details.added_by_module = "part_management2";

		details.save(current_user, "mobile part management", false);



		}


	protected void tab_status_OnClick(object _sender, EventArgs _e)
		{
		mv.SetActiveView(vw_status);
		set_selectors();
		load_status_repeater("1", false);
		}

	protected void ddl_status_sort_OnSelectedIndexChanged(object _sender, EventArgs _e)
		{
		load_status_repeater(ddl_status_sort.SelectedValue, false);
		}

	protected void chkAscending_OnCheckedChanged(object _sender, EventArgs _e)
		{
		load_status_repeater(ddl_status_sort.SelectedValue, false);
		}

	protected void ddl_wo_OnSelectedIndexChanged(object _sender, EventArgs _e)
		{
		if (ddl_trucks.Value != null)
			{
			Session["mobile_ddl_truck"] = ddl_trucks.Value;
			}
		if (ddl_wo.Value != null)
			{
			Session["working_woprog_id"] = ddl_wo.Value;
			load_wo_repeater("", false);
			}
		mv.SetActiveView(vw_wo);
		load_page();
		}
	}



