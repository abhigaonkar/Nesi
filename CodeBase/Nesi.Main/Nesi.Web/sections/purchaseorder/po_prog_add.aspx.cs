using DevExpress.Web;
using DevExpress.Xpo;
using MySql.Data.MySqlClient;
using nesi.core;
using NESI.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class sections_purchaseorder_po_prog_add : Page
{
	private NeMember currentUser;
	MySqlConnection conn;
	string poprogid = "";
	protected void Page_Init(object _sender, EventArgs _e)
	{
		currentUser = Toolbox.do_handle_authentication(OpsPage.Purchases);
		nesi_cut_po.Visible = currentUser.business_unit.is_backoffice;
		conn = Toolbox.connect();
	}
	protected void Page_Unload(object _sender, EventArgs _e)
	{
		if (conn == null || conn.State != ConnectionState.Open) return;
		conn.Close();
		conn.Dispose();
	}

	private void DataBindForVendor(string poprogid, string bu, int vendorID)
	{
		var queryWithMapping = @"
SELECT DISTINCT
  a.Vendor_ID,
  vendor_number,
  vendor_name,
  NAME vendor_branch,
  address_city
FROM
  vendor a,
  business_unit b,
  address c,
  vendor_business_unit d
WHERE b.ID = d.business_unit_id
  AND d.Vendor_ID = a.Vendor_ID
  AND a.Vendor_active = 1
  AND a.Vendor_Hold = 'F'
  AND c.Address_Table_ID = a.Vendor_ID
  AND c.Address_Table = 'Vendor'
  AND b.enable_timesheet = 1
  AND d.business_unit_id = @v0
  AND c.Active = 1
	AND a.business_unit_id_link IS NULL 
ORDER BY a.Vendor_CreatedDateTime DESC,
  a.vendor_name";

		var queryWithPrevious = @"
SELECT DISTINCT
  a.vendor_id,
  a.vendor_number,
  a.vendor_name,
  a.vendor_branch,
  address_city
FROM
  (SELECT DISTINCT
    vendor_id,
    vendor_number,
    vendor_name,
    NAME vendor_branch,
    address_city,
    Vendor_CreatedDateTime
  FROM
    vendor a,
    business_unit b,
    address c
  WHERE b.ID = a.business_unit_id
    AND a.Vendor_active = 1
    AND a.Vendor_Hold = 'F'
    AND c.Address_Table_ID = Vendor_ID
    AND c.Address_Table = 'Vendor'
    AND b.enable_timesheet = 1
    AND a.vendor_id = @v0 
	AND a.business_unit_id_link IS NULL 
  UNION
  SELECT DISTINCT
    a.Vendor_ID,
    vendor_number,
    vendor_name,
    NAME vendor_branch,
    address_city,
    Vendor_CreatedDateTime
  FROM
    vendor a,
    business_unit b,
    address c,
    vendor_business_unit d
  WHERE b.ID = a.business_unit_id
    AND d.Vendor_ID = a.Vendor_ID
    AND a.Vendor_active = 1
    AND a.Vendor_Hold = 'F'
    AND c.Address_Table_ID = a.Vendor_ID
    AND c.Address_Table = 'Vendor'
    AND b.enable_timesheet = 1
    AND d.business_unit_id = @v1 
	AND a.business_unit_id_link IS NULL ) a
  ORDER BY a.Vendor_CreatedDateTime DESC,
    a.vendor_name
";

		if (string.IsNullOrWhiteSpace(poprogid))
		{
			// When creating a new PO....
			ddl_vendor.DataSource = Toolbox.doSQL_dt(conn, queryWithMapping, new object[] { bu });
			ddl_vendor.DataBind();
		}
		else
		{
			ddl_vendor.DataSource = Toolbox.doSQL_dt(conn, queryWithPrevious, new object[] { vendorID, bu });
			ddl_vendor.DataBind();
		}
	}

protected void Page_Load(object _sender, EventArgs _e)
	{

		divSide.InnerHtml = shared.PrintSidePanelHTML(currentUser);
		var q = Request.QueryString;
		poprogid = q["poprogid"] ?? "0";
		var from_bvpo = poprogid.StartsWith("00");
		poprogid = from_bvpo ? Toolbox.doSQL_string(conn, @"SELECT poprog_id FROM poprog_header WHERE poprog_bvpo = @v0  LIMIT 1", new object[] { poprogid }) : poprogid;
		var action = q["action"] ?? "";
		if (from_bvpo) { Response.Redirect("./po_prog_add.aspx?action=show&poprogid=" + poprogid, true); }
		hidCompanyID.Value = q["business_unit_id"] ?? hidCompanyID.Value;
		hidAction.Value = action;
		hidPOProgID.Value = poprogid;
		DDLCurrency.DataBind();
		var vendorID = 0;


		//lblAddContact.InnerHtml = @"<a href='javascript:boing(""/#/opens/11/vendors/" + vendorID + @""", ""Vendor"", 950, 1024);'>" + "Add Contact" + "</a>";

		/* Check if PO exists in BV, if not then add */
		if (poprogid != "" && poprogid != "0")
		{
			var mspo = new NePOProg(int.Parse(poprogid));
			fill_integ_history(hidPOProgID.Value);
		}
		else
		{
			for (var i = 1; i < tabs.TabPages.Count; i++)
			{
				if (i == 6) continue; // Don't hide the Vendor tab.
				tabs.TabPages[i].Visible = false;
			}
			tr_po_id.Visible = false;
			tr_po_number.Visible = false;
			ImgBtnCancelPO2.Visible = false;
			ImageButtonPrintPreview.Visible = false;
			btnOpenScanWindow.Visible = false;
			tr_last4_credit_card.Visible = false;
		}
		tabs.TabPages.FindByText("Vendor").Visible = false;
		// Added Min Date for dates
		combo_expectedorderdate.MinDate = DateTime.Today.AddDays(-7);
		combo_required_date.MinDate = DateTime.Today.AddDays(-7);
		combo_expected_receive_date.MinDate = DateTime.Today.AddDays(-7);
		if (!string.IsNullOrEmpty(q["lineitems"]))
		{
			tabs.ActiveTabIndex = 3;
			if_picklist.Attributes.Add("src", "/sections/member/picklist/pikclist.aspx?id=" + poprogid + "&rev=0&origin=purchaseorder&iframe=yes");
		}
		if (q["a"] != null && (q["poprog_id"] != null || q["po_id"] != null))
		{
			Response.Clear();
			switch (q["a"])
			{
				case "get_ts":
					if (!string.IsNullOrEmpty(q["poprog_id"]))
					{
						var ts = Convert.ToDateTime(Toolbox.doSQL_string(conn, @"SELECT IFNULL(poprog_ts, '2000-01-01') FROM poprog_header  WHERE poprog_id =@v0", new object[] { q["poprog_id"] }));
						Response.Write(ts.Year == 2000 ? "0" : ts.Ticks + "|" + Toolbox.MySQL_longdt(ts));
					}
					else
					{
						Response.Write("0");
					}
					break;
				#region json_update_header
				case "json_update_header":
					Toolbox.do_set_plain_header(Response);
					// You need three things, po total, order qty & received qty
					// For this example i'll use a JSON string
					// Grab the totals and the status
					var dr = Toolbox.doSQL_dt(conn, @" SELECT IFNULL(SUM(po_details_qty_ordered * po_details_cost),0) po_total, IFNULL(SUM(po_details_qty_ordered),0) order_qty, IFNULL(SUM(po_details_qty_received),0) recd_qty FROM po_details_current WHERE po_details_poprog_id=@v0 ", new object[] { q["po_id"] }).Rows[0];
					var po_total = Convert.ToDouble(dr["po_total"]);
					var order_qty = Convert.ToDouble(dr["order_qty"]);
					var recd_qty = Convert.ToDouble(dr["recd_qty"]);
					var po_status = "unknown";
					if (!string.IsNullOrEmpty(q["po_id"]))
					{
						try
						{
							po_status = Toolbox.doSQL_string(conn, @"SELECT poprog_status.status_type as _status FROM poprog_status INNER JOIN poprog_header ON poprog_status.POProg_Status_ID = poprog_header.poprog_status  where poprog_header.poprog_id =@v0", new object[] { q["po_id"] });
						}
						catch
						{
							po_status = "unknown";
						}
						// Create the feed
						var feed = string.Format(@"
	{{
	""po_total""	: ""{0:C2}"",
	""order_qty""	: ""{1}"",
	""recd_qty""	: ""{2}"",
    ""po_status""   : ""{3}""
	}}", po_total, order_qty, recd_qty, po_status);
						Response.Write(feed);
					}
					break;
				#endregion json_update_header
				#region BM_approval
				case "BM_approval":
					if (q["po_id"] != null && q["type"] != null)
					{
						var po_id = Convert.ToInt32(q["po_id"]);
						var po = new NePOProg(po_id);
						var po_value = po.poprog_total_cost;
						// Figure out who should be approving this based on the value
						var n_limits = Toolbox.doSQL_int(conn, @"SELECT COUNT(id) from business_unit_po_dist WHERE amount_to >= @v1  and member_id = @v2  ", new[] { ddl_company.Value, po_value, currentUser.id });
						if (n_limits == 0)
						{
							Response.Write("<b style='color:#f00'>You are not authorized to approve purchase orders at this value</a>");
							Response.End();
						}

						var msg = new NeEMail
						{
							To = new NeMember(Convert.ToInt32(po.poprog_cutby_member_id)).NEEmail,
							From = currentUser.NEEmail,
							isHTML = true
						};
						var body = new StringBuilder();
						if (po.poprog_status != OpsPOStatus.WaitingBMApproval)
						{
							Response.Write(string.Format("ERROR: Order is no longer in the waiting for approval status.<br/>Please check with your purchaser.<br/><a href='https://{0}'>NESI Home Page</a>", HttpContext.Current.Request.Url.Host));
						}
						else
						{
							switch (q["type"])
							{
								case "approve":
									order_approved(po_id, currentUser);
									msg.Subject = string.Format("PO#:{0} has been approved", po.business_unit_id.ToString().PadLeft(3, '0') + "-" + po.poprog_bvpo.TrimStart('0'));
									if (po.poprog_cutby_member_id != currentUser.id)
									{
										body = body.AppendFormat(@"
<div style='font-family:arial;font-size:12px'>
	<b>Purchase Order: </b><a href='{4}/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}' target = 'blank'>{0}</a><br/>
	<b>Vendor: </b>{1}<br/>
    <b>Description: </b>{3}<br/>
	<b>Approved By: </b>{2}<br/></div>",
						   po.poprog_id,
						  new NEVendor(po.poprog_vendor_id).Vendor_Name,
						   currentUser.FullName, po.poprog_order_description, Toolbox.app_setting("Domain"));
										msg.Body = body.ToString();
										msg.Send();


									}
									Response.Write(string.Format("Order has been approved.<br/><a href='https://{0}'>NESI Home Page</a>", HttpContext.Current.Request.Url.Host));
									break;
								case "deny":
									msg.Subject = string.Format("PO#:{0} has been denied", po.business_unit_id.ToString().PadLeft(3, '0') + "-" + po.poprog_bvpo.TrimStart('0'));
									if (po.poprog_cutby_member_id != currentUser.id)
									{
										body = body.AppendFormat(@"
<div style='font-family:arial;font-size:12px'>
	<b>Purchase Order: </b><a href='{4}/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}' target = 'blank'>{0}</a><br/>
	<b>Vendor: </b>{1}<br/>
<b>Description: </b>{3}<br/>
	<b>Denied By: </b>{2}<br/></div>",
						   po.poprog_id,
						  new NEVendor(po.poprog_vendor_id).Vendor_Name,
						   currentUser.FullName, po.poprog_order_description, Toolbox.app_setting("Domain")
					   );
										msg.Body = body.ToString();
										msg.Send();
									}
									Response.Write(string.Format("Order has been denied.<br/><a href='https://{0}'>NESI Home Page</a>", HttpContext.Current.Request.Url.Host));
									break;
							}
						}
					}
					else
					{
						Response.Write("<b style='color:#f00'>Invalid Attempt</a>");
					}
					break;
				#endregion BM_approval                    #region get_ts

				case "update_ts":
					Toolbox.do_set_plain_header(Response);
					if (!string.IsNullOrEmpty(q["poprog_id"]))
					{
						int.TryParse(q["poprog_id"], out var id);
						var po = new NePOProg(id);
						if (!Toolbox.Contains(po.poprog_status, new[] { OpsPOStatus.WaitingToBeClosed, OpsPOStatus.Closed, OpsPOStatus.Cancelled }))
						{
							Toolbox.doSQL_void(conn, @"SET @disable_triggers = 1;UPDATE poprog_header SET poprog_last_modified=NOW(), poprog_ts = NOW() WHERE poprog_id = @v0;SET @disable_triggers = NULL;", new object[] { id });
						}
						Response.Write("1");
					}
					else
					{
						Response.Write("Invalid Request");
					}
					break;
			}
			try
			{
				Title = poprogid == "0" || poprogid == "" ? "New PO" : Server.HtmlEncode("PO " + Toolbox.doSQL_string(conn, @"SELECT CONCAT(lpad(poprog_header.business_unit_id,3,'0'),'-', trim(LEADING '0' from poprog_bvpo),' - ', VENDOR_NAME(poprog_vendor_id), ' - ', URLDECODE(poprog_order_description)) from poprog_header  where poprog_id =@v0", new object[] { poprogid }));
			}
			catch
			{
				Title = "New PO";
			}
			var lbltemp = (Label)Page.Master.FindControl("lblHeading");
			lbltemp.Text = Title;
			Response.End();
		}
		else
		{
			ASPxComboBoxShipVia.DataSource = Toolbox.doSQL_dt(conn, @"SELECT id, shipping_method FROM poprog_shipping", null);
			ASPxComboBoxShipVia.DataBind();
			ASPxComboBoxPayment.DataSource = Toolbox.doSQL_dt(conn, @"SELECT term_id AS id, CAST(CONCAT(term_code,' - ', term_desc) AS CHAR) as payment_method FROM term where Active=1 order by term_id", null);
			ASPxComboBoxPayment.DataBind();
			try
			{
				Title = poprogid == "0" || poprogid == ""
							? "New PO"
							: Server.HtmlEncode("PO " + Toolbox.doSQL_string(conn, @"SELECT CONCAT(lpad(poprog_header.business_unit_id,3,'0'),'-', trim(LEADING '0' from poprog_bvpo),' - ', VENDOR_NAME(poprog_vendor_id), ' - ', URLDECODE(poprog_order_description)) from poprog_header  where poprog_id =@v0", new object[] { poprogid }));
			}
			catch
			{
				Title = "New PO";
			}
			var lbltemp = (Label)Page.Master.FindControl("lblHeading");
			lbltemp.Text = Title;
			btnOpenScanWindow.ImageUrl = Toolbox.doSQL_int(conn, @"Select ifnull(count(poprog_scans_id),0) from poprog_scans where poprog_scans_poprog_id = @v0 ", new object[] { poprogid }) <= 0
				? "~/images/IconsButtons/32px-Crystal_Clear_app_scanner-disabled.png"
				: "~/images/IconsButtons/32px-Crystal_Clear_app_scanner.png";
			btnOpenScanWindow.OnClientClick = "javascript:boing('/sections/purchaseorder/po_prog_scans.aspx?poprogid=" + poprogid + "','Scans',900,800);";
			ImageButtonPrintPreview.OnClientClick = "javascript:boing('/sections/purchaseorder/POOrderSlip.aspx?pp=true&PO=" + poprogid + "','Purchase_Order_Preview',900,800);";
			var h = Toolbox.doSQL_int(conn, @"Select ifnull(count(po_details_current.po_details_id),0) from po_details_current where po_details_current.po_details_poprog_id = @v0 ", new object[] { poprogid });
			h = 200 + h * 50;
			if (h < 750)
				h = 750;
			if (poprogid == "")
			{
				ButtonStartPO.Visible = currentUser.AuthenticatedForPrivilege(86);
			}
			if_picklist.Attributes.Add("height", h.ToString());
			ddlPartList.DataSource = Toolbox.doSQL_dt(@"SELECT 0 id, 'Select part /rec# to filter' t, 0 order_by, 0 rec_no  UNION ALL SELECT po_details_id id, CONCAT(po_details_part_no, ' - Rec: ', po_details_rec_no) t, 1 order_by, po_details_rec_no rec_no  FROM po_details_current WHERE po_details_poprog_id = @v0 ORDER BY order_by, rec_no", new object[] { poprogid });
			ddlPartList.DataBind();
			if (!IsPostBack)
			{
				Session["po_branch"] = null;

				if (action == "show")
				{
					if (!NePOProg.POExists(Convert.ToInt32(poprogid)))
					{
						Response.Write("PO Doesn't Exist.<br/><a href='javascript:history.go(-1);'>back</a>");
						Response.End();
					}
					ButtonStartPO.Visible = false;
					ImageButton2.Visible = false;
					Button1a.Visible = false;
					var poprogress = new NePOProg(Convert.ToInt32(poprogid));
					vendorID = poprogress.poprog_vendor_id;
					var cutbymember = new NeMember(Convert.ToInt32(poprogress.poprog_cutby_member_id));
					var po_branch = new NeBusinessUnit(poprogress.business_unit_id);
					chk_nesi_po.Checked = poprogress.nesi_cut_po;
					hidCompanyID.Value = poprogress.business_unit_id.ToString();
					if (poprogress.poprog_status == OpsPOStatus.ApprovedtoOrder || poprogress.poprog_status == OpsPOStatus.NotIssued)
					{
						ImageButtonPrintPO2.ToolTip = "Issue this Purchase Order";
					}
					else
					{
						ImageButtonPrintPO2.ToolTip = "Print A Copy of the Purchase Order";
						UpdateButton2.Visible = false;
						ImageButtonSaveVendor.Visible = false;
						UpdateButton4.Visible = false;
						chkAllowEdit.Enabled = false;
					}
					//  lbTopPoNum.Visible = true; commented out by andy
					lbTopPoNum.Text = "PO Number: " + po_branch.id.ToString().PadLeft(3, '0') + "-" + poprogress.poprog_bvpo.TrimStart('0');
					lblTopRightPO.Text = po_branch.id.ToString().PadLeft(3, '0') + "-" + poprogress.poprog_bvpo.TrimStart('0');
					hidStatus.Value = poprogress.poprog_status.ToString();
					ddl_company.Value = poprogress.business_unit_id.ToString();
					ImageButtonAPProblems.Visible = currentUser.business_unit.is_backoffice && poprogress.poprog_status != OpsPOStatus.APProblems && poprogress.poprog_status != OpsPOStatus.Closed;
					var can_see_ap_button = currentUser.AuthenticatedForPrivilege(OpsPrivilege.ImageButtonApProblemsFixedVisible);
					var same_branch_has_privilege = new Current_User().visible_business_units.Split(',').Contains(po_branch.id.ToString()) && can_see_ap_button;
					var nesi_user_has_privilege = currentUser.business_unit.is_backoffice && can_see_ap_button;
					var has_ap_problems = poprogress.poprog_status == OpsPOStatus.APProblems;
					ImageButtonAPProblemsFixed.Visible = has_ap_problems && (same_branch_has_privilege || nesi_user_has_privilege);
					bind_locations();
					combo_location.SelectedIndex = poprogress.location_master_id == 0 ? 0 : combo_location.Items.FindByValue(poprogress.location_master_id).Index;
					ddl_vendor.Value = poprogress.poprog_vendor_id;
					hidVendorID.Value = poprogress.poprog_vendor_id.ToString();
					ddlContacts.DataSource = Toolbox.doSQL_dt(conn, @"SELECT contact_id,contact_name as contact FROM contact  WHERE contact_cust_id =@v0 AND contact_type = 'Vendor'", new object[] { hidVendorID.Value });
					ddlContacts.DataBind();
					ddlContacts.Value = poprogress.poprog_contact_id;
					hidCompanyID.Value = poprogress.business_unit_id.ToString();
					get_open_work_orders(hidCompanyID.Value);
					ASPxcbwo.Value = poprogress.poprog_woprog_id.ToString();
					ASPxComboBoxShipVia.Value = poprogress.poprog_shipping_method.ToString();
					ASPxComboBoxPayment.Value = poprogress.poprog_poprog_payment_method_id;
					update_po_totals();
					var wos = poprogress.get_workorders(poprogress.poprog_id);
					foreach (DataRow dr in wos.Rows)
					{
						if (dr["woprog_status"].Equals("Invoiced") || dr["woprog_status"].Equals(OpsWOStatus.WaitingToBeInvoiced))
						{
						}
					}

					try
					{
						if (poprogress.poprog_contact_id != 0)
						{
							ddlContacts.Value = poprogress.poprog_contact_id;
						}
					}
					catch (Exception exce) { Toolbox.do_errorLog_errorStack(exce); }
					ImageButtonAddScan2.Visible = true;
					get_totals();

					var country = NeBusinessUnit.GetbuCountry(poprogress.business_unit_id.ToString());
					if (country == "CDN")
					{
						//txchk1.Checked = true;
					}
					try
					{
						DDLCurrency.Value = Convert.ToInt32(poprogress.poprog_country_code_currency);
					}
					catch (Exception exce) { Toolbox.do_errorLog_errorStack(exce); }
					display_po_information(poprogid);
					if_vendor.Attributes.Add("height", "800");
					ddl_company.ClientEnabled = poprogress.poprog_id == 0;
					ddl_vendor.ClientEnabled = poprogress.poprog_status == OpsPOStatus.NotIssued;
					Labelcutby.Text = cutbymember.FullName;
					lblLinkButtonPickList.Visible = false; // changed to false by Andy March 5 2011
					lblLinkButtonPickList.Text = @"<a href='javascript:boing(""/sections/member/picklist/pikclist.aspx?id=" + poprogid + @"&rev=0&origin=purchaseorder"", ""PurchaseOrderPickList"", 950, 800);'>Part List - BETA</a>";
					var company = new NeBusinessUnit(Convert.ToInt32(hidCompanyID.Value));
					var defaddress = company.name + "\n";
					defaddress += company.address + "\n";
					defaddress += company.city + "\n";
					defaddress += company.Prov + ", ";
					defaddress += company.postal + "\n";
					if (company.country == "CDN")
					{
						defaddress += "Canada";
					}
					else
					{
						defaddress += "USA";
					}
					hidDefaultAddress.Value = defaddress;
					//ASPxcbVendor.SelectedIndex	= !string.IsNullOrEmpty(_q["vendor_id"]) ? ASPxcbVendor.Items.FindByValue(_q["vendor_id"]).Index : -1;
					//ASPxcbVendor_SelectedIndexChanged(ASPxcbVendor, new EventArgs());
					//}
				}
				else if (action == "add")
				{

					ddl_vendor.Focus();
					if (hidCompanyID.Value == "")
					{
						hidCompanyID.Value = currentUser.business_unit_id.ToString();
					}
					get_open_work_orders(hidCompanyID.Value);
					try
					{
						ddl_company.Value = hidCompanyID.Value;
					}
					catch (Exception exce) { Toolbox.do_errorLog_errorStack(exce); }
					ASPxComboBoxShipVia.Value = "1";
					ASPxComboBoxPayment.Value = 17;
					UpdateButton2.Visible = false;
					ImageButtonSaveVendor.Visible = false;
					UpdateButton3.Visible = false;
					UpdateButton4.Visible = false;
					//ASPxRoundPanelAddingLines.Visible = false;
					ImageButton7.Visible = false;
					//ImageButton7.Enabled = false;
					combo_expectedorderdate.Date = DateTime.Now;
					lblStatusDisp.Text = "Not Issued";
					Labelcutby.Text = currentUser.FullName;
					var country = NeBusinessUnit.GetbuCountry(hidCompanyID.Value);
					var company = new NeBusinessUnit(Convert.ToInt32(hidCompanyID.Value));
					DDLCurrency.Value = company.default_currency;
					lblShippingData.Text = company.name + "\n";
					lblShippingData.Text += company.address + "\n";
					lblShippingData.Text += company.city + "\n";
					lblShippingData.Text += company.Prov + ", ";
					lblShippingData.Text += company.postal + "\n";

					if (company.country == "CDN")
					{
						lblShippingData.Text += "Canada";
					}
					else
					{
						lblShippingData.Text += "USA";
					}
					hidShipToAddressID.Value = "0";
					hidManualAddress.Value = lblShippingData.Text;
					txtShippingCity.Text = company.city;
					txtShippingPostCode.Text = company.postal;
					txtShippingAddress1.Text = company.address;
					txtShippingAddress2.Text = " ";
					txtAddressBox.Text = company.address + "," + company.city + "\n";
					txtAddressBox.Text += company.Prov + ", ";
					if (company.country == "CDN")
					{
						txtAddressBox.Text += "Canada\n";
						txtShippingCountry.Text = "Canada\n";
						txtShippingProv.Text = company.Prov;
					}
					else
					{
						txtAddressBox.Text += "USA\n";
						txtShippingCountry.Text = "USA\n";
						txtShippingProv.Text = company.State;
					}

					txtAddressBox.Text += company.postal;
					bind_locations();
					combo_location.SelectedIndex = 0;
				}
				display_link_buttons();
			}
			else
			{
				bind_locations();
				if (hidAction.Value == "show")
				{
					var poprogress = new NePOProg(Convert.ToInt32(poprogid));
					if (poprogress.poprog_status != OpsPOStatus.NotIssued)
					{
						ddl_vendor.Value = poprogress.poprog_vendor_id;
						ddl_vendor.Enabled = false;
					}

					vendorID = poprogress.poprog_vendor_id;
				}
				get_open_work_orders(hidCompanyID.Value);
			}
		}

		// DDLCurrency.ReadOnly = true;
		DDLCurrency.Enabled = false;
		DDLCurrency.Value = Toolbox.doSQL_int(@"SELECT ifnull(default_currency,1) FROM neintranet.business_unit where ID=@v0", new object[] { ddl_company.Value });

		DataBindForVendor(poprogid, hidCompanyID.Value, vendorID);
	}
	private void bind_locations()
	{
		var bu = new NeBusinessUnit(ddl_company.Value);
		using (var uow = new UnitOfWork())
		{
			combo_location.DataSource = from x in new XPQuery<ne_xpo.cs.inventory_location_master>(uow)
										where x.business_unit_id == uow.GetObjectByKey<ne_xpo.cs.business_unit>(bu.warehouse_bu_id)
										select x;
			combo_location.DataBind();
		}
		var li = new ListEditItem("Please Select A Location", 0);
		combo_location.Items.Insert(0, li);
	}
	private void display_po_information(string _poprogid)
	{
		var str_branch_name = "";
		var poprogress = new NePOProg(Convert.ToInt32(_poprogid));
		var woprogress = new NeWOProg(); // (Convert.ToInt32(poprogress.poprog_woprog_id.ToString()));
		try
		{
			if (poprogress.poprog_woprog_id > 200000)
			{
				woprogress.Load(poprogress.poprog_woprog_id);
			}
		}
		catch (Exception ee)
		{
			Toolbox.do_errorLog_errorStack(ee);
		}
		var company = new NeBusinessUnit(poprogress.business_unit_id);

		//Numbers And Branch
		txtDescription.Text = poprogress.poprog_order_description;
		lblStatusDisp.Text = Toolbox.doSQL_string(conn, @"SELECT status_type FROM poprog_status  WHERE poprog_status_id =@v0", new object[] { poprogress.poprog_status });
		//lblCustNameDisp.Text = woprogress.CustomerName.ToString();
		lblPONumDisp.Text = $"{poprogress.business_unit_id.ToString().PadLeft(3, '0')}-{poprogress.poprog_bvpo.TrimStart('0')}";
		lblPOProgIDDisp.Text = poprogress.poprog_id.ToString();
		str_branch_name = company.name;
		chkAllowEdit.Checked = poprogress.edit_after_issue == 1;
		hidStatus.Value = poprogress.poprog_status.ToString();
		DDLCurrency.Value = Convert.ToInt32(poprogress.poprog_country_code_currency);
		set_work_order_details(poprogress.poprog_woprog_id.ToString());
		//Address And Name Details

		var vendaddresstable = Toolbox.doSQL_dt(conn, @"
SELECT 
	vendor_number, 
	vendor_name, 
	address_addr1,
	vendor_credittype, 
	vendor_creditlimit, 
	address_city, 
	address_prov, 
	address_postal, 
	address_country, 
	address_phonearea, 
	address_phonefirst, 
	address_phonelast, 
	address_email 
FROM 
	vendor, address 
WHERE 
	vendor_id = @v0 AND 
	address_table = 'Vendor' AND
	address_table_id = vendor_id ", new object[] { poprogress.poprog_vendor_id.ToString() });
		foreach (DataRow vendorrowinfo in vendaddresstable.Rows)
		{
			lblVendorNoDisp.InnerHtml = $@"<a href='javascript:boing(""/#/opens/11/vendors/{poprogress.poprog_vendor_id}"", ""Vendor"", 950, 1024);'>{vendorrowinfo["Vendor_number"]}</a>";
			lblCreditLimitDisp.Text = vendorrowinfo["Vendor_CreditLimit"].ToString();
			lblCreditTypeDisp.Text = vendorrowinfo["Vendor_CreditType"].ToString();
			lblVendDetails.Text = $@"
			{vendorrowinfo["vendor_name"]} <br />
			{vendorrowinfo["Address_Addr1"]} <br />
			{vendorrowinfo["Address_City"]} {vendorrowinfo["Address_Prov"]}<br />
			{vendorrowinfo["Address_Country"]} {vendorrowinfo["Address_Postal"]}<br />
			{vendorrowinfo["Address_PhoneArea"]}-{vendorrowinfo["Address_PhoneFirst"]}-{vendorrowinfo["Address_PhoneLast"]}<br />
			{vendorrowinfo["Address_Email"]}
			";
		}
		//Dates
		LabelOrderDateDisp.Text = poprogress.poprog_order_placed_date;
		combo_expectedorderdate.Text = poprogress.poprog_expected_order_date;
		combo_required_date.Text = poprogress.poprog_date_required;
		combo_expected_receive_date.Text = poprogress.poprog_expected_received_date;
		combo_last4.Value = poprogress.cc_lastfour_id;
		combo_last4.Enabled = false;
		//ASPxDateEdit4.Text = poprogress.poprog_date_required;
		lblReceivedDateDisp.Text = poprogress.poprog_received_date;
		chkAckOfPORec.Checked = Convert.ToBoolean(poprogress.poprog_ack_req);
		chkPORec.Checked = Convert.ToBoolean(poprogress.poprog_ack_req_rec); ;
		chkShipNoticeReq.Checked = Convert.ToBoolean(poprogress.poprog_ship_note_req);
		chkShipNotice.Checked = Convert.ToBoolean(poprogress.poprog_ship_note_req_rec);
		update_top_right_box(_poprogid);
		var address = NePOProg.GetShippingAddress(poprogress.poprog_id);

		lblShippingData.Text = address; 

		if (poprogress.poprog_shipping_method != 2)
		{
			txtShippingAddress1.Text = poprogress.Shipping_Addr1;
			txtShippingAddress2.Text = poprogress.Shipping_Addr2;
			txtShippingCity.Text = poprogress.Shipping_City;
			txtShippingCountry.Text = poprogress.Shipping_Country.Trim().ToLower() == "cdn" ? "Canada" : "USA";
			txtShippingPostCode.Text = poprogress.Shipping_Postal;
			txtShippingProv.Text = poprogress.Shipping_Province;
		}
		else
		{
			// when shipping method is 2 (Pick up order), we still need to do this.
			txtShippingAddress1.Text = poprogress.Shipping_Addr1;
			txtShippingAddress2.Text = poprogress.Shipping_Addr2;
			txtShippingCity.Text = poprogress.Shipping_City;
			txtShippingCountry.Text = poprogress.Shipping_Country.Trim().ToLower() == "cdn" ? "Canada" : "USA";
			txtShippingPostCode.Text = poprogress.Shipping_Postal;
			txtShippingProv.Text = poprogress.Shipping_Province;
		}

		hidManualAddress.Value = address;
		//}
		txtPOHistory.Text = poprogress.eventtext;
		var sb = new StringBuilder();
		using (var uow = new UnitOfWork())
		{
			var x = (from y in new XPQuery<ne_xpo.cs.poprogstatus>(uow)
					 where y.poprogstatus_poprog_id == (int)poprogress.poprog_id
					 select new
					 {
						 who = y.poprogstatus_member_id.member_fullname,
						 when = y.poprogstatus_datetime,
						 what = y.poprogstatus_status
					 }).OrderByDescending(_z => _z.when);
			foreach (var y in x)
			{
				sb.AppendFormat("{1:yyyy-MM-dd HH:mm:ss} - Status set to '{2}' by {0}\n", y.who, y.when, y.what);
			}
		}
		txtStatusHistory.Text = sb.ToString();
		build_chat_history();
		insert_grid_information(hidPOProgID.Value);

		//}
	}
	private void update_top_right_box(string _poprogid)
	{
		var poprogress = new NePOProg(Convert.ToInt32(_poprogid));
		var totalordered = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_ordered),0) FROM po_details_current  WHERE po_details_poprog_id=@v0", new object[] { _poprogid });
		var totalreceived = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_received),0) FROM po_details_current  WHERE po_details_poprog_id=@v0", new object[] { _poprogid });
		var lastModified = Toolbox.doSQL_string(conn, @"SELECT IFNULL(poprog_ts, 'N/A') FROM poprog_header WHERE poprog_id=@v0", new object[] { _poprogid });
		lbllinesTotal.Text = totalordered.ToString();
		lbl_lastmodified.Text = lastModified == "N/A" ? "N/A" : Toolbox.MySQL_longdt(DateTime.Parse(lastModified));
		img_lastmodified.Visible = _poprogid != "0" && (currentUser.business_unit.is_backoffice || Toolbox.Contains(currentUser.MemberTypeID, new[] { 117, 78 })) && !Toolbox.Contains(poprogress.poprog_status, new[] { 6, 7, 8 });
		//		lblPOTotal.Text = totalcost.ToString("C2");
		lblPOTotal.Text = new NePOProg(Convert.ToInt32(_poprogid)).poprog_total_cost.ToString("C2");
		lblRecdTotal.Text = totalreceived.ToString();
		lbl_headerStatus.Text = lblStatusDisp.Text;
	}
	private void move_status(object _status_id, object _po_id)
	{
		var id = 0;
		int.TryParse(_po_id.ToString(), out id);
		var s_id = 0;
		int.TryParse(_status_id.ToString(), out s_id);
		//check_for_changes();
		if (id > 0 && s_id > 0)
		{
			var status = Toolbox.doSQL_string(conn, @"SELECT status_type FROM poprog_status WHERE poprog_status_id = @v0  LIMIT 1", new object[] { s_id });
			NePOProg.UpdateStatus(Convert.ToInt32(hidPOProgID.Value), s_id);
			NePOProg.status_log.add(id, currentUser.id, s_id);
		}
	}

	

    private bool checkAdvancedState()
    {
        //This checks if attached work orders are in an advanced state, and hides the Unissue button if so
        var isAdvancedState = false;

        var check = Toolbox.doSQL_dt(conn, @"SELECT * FROM po_details_current LEFT JOIN woprog ON po_details_woprog_id = woprog_id WHERE po_details_poprog_id = @v0 AND po_details_woprog_id BETWEEN 1000000 AND 2000000 ", new object[] { hidPOProgID.Value });

        foreach (DataRow row in check.Rows)
        {
			var woStatus = Toolbox.ReturnBlankIfNull_string(row["woprog_status"]);
            if (woStatus != "" && woStatus.Contains("Invoiced"))
            {
                Unissue2.Visible = false;
                isAdvancedState = true;
                
            }
            
        }
        return isAdvancedState;
        
    }

	private void checkForUnissue(int status = 0)
	{
		if (!currentUser.AuthenticatedForPrivilege(OpsPrivilege.AlterPosAfterIssue) &&
			!currentUser.AuthenticatedForPrivilege(OpsPrivilege.UnissuePOPurchaser))
		{
			return;
		}

		if (currentUser.AuthenticatedForPrivilege(OpsPrivilege.AlterPosAfterIssue) &&
			(NePOProg.LineCountActive(Convert.ToInt32(hidPOProgID.Value)) == 0 || currentUser.business_unit.is_backoffice))
		{
			Unissue2.Visible = true;
		}
		else if (currentUser.AuthenticatedForPrivilege(OpsPrivilege.UnissuePOPurchaser) && status != OpsPOStatus.ReceivedWaitingforInvoice)
		{
			Unissue2.Visible = true;
		}
	}
	private void display_link_buttons()
	{

        checkAdvancedState();

        var link_buttons = hidAction.Value == "add"
			? $"<a href='po_prog_edit.aspx?business_unit_id={hidCompanyID.Value}'><img src='/images/IconsButtons/32px-Crystal_Clear_action_back.png' title='Go back to purchase order progress page'></a>"
			: $"<a href='po_prog_edit.aspx?business_unit_id={hidCompanyID.Value}'><img src='/images/IconsButtons/32px-Crystal_Clear_action_back.png' title='Go back to purchase order progress page.'></a>";
		linkbuttons2.Text = link_buttons;
		int.TryParse(hidStatus.Value, out var intStatus);
		if (intStatus > 0)
		{
			if (NePOProg.LineCount(Convert.ToInt32(hidPOProgID.Value)) == 0)
			{
				ImgBtnCancelPO2.ImageUrl = "/images/iconsbuttons/32px-Crystal_Clear_action_button_cancel.png";
			}
			else
			{
				ImgBtnCancelPO2.Enabled = false;
				ImgBtnCancelPO2.ImageUrl = "/images/iconsbuttons/32px-Crystal_Clear_action_button_cancel_grey.png";
				ImgBtnCancelPO2.ToolTip = "There must be no line items on the PO before you can cancel it";
			}
			ImgBtnCancelPO2.Visible = true;
		}
		switch(intStatus)
			{
			case OpsPOStatus.NotIssued:
				ImageButtonOkayToOrder2.Visible = currentUser.AuthenticatedForPrivilege(OpsPrivilege.CreatePos);
				ImageButtonQuestions2.Visible   = true;
				chkAckOfPORec.Enabled = true;
				chkShipNoticeReq.Enabled = true;
			break;
			case OpsPOStatus.WaitingBMApproval:
				var value    = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_ordered * po_details_cost), 0) FROM po_details_current WHERE po_details_poprog_id = @v0 ", new object[] { hidPOProgID.Value });
			
				if (NePOProg.HasPermissionToApprove(Convert.ToInt32(ddl_company.Value), value, currentUser.id))
					{
						ImageButton_BMApp.Visible = true;
					}
					

				ImageButtonQuestions2.Visible = true;
				chkAckOfPORec.Enabled         = true;
				//     chkPORec.Enabled = false;
				chkShipNoticeReq.Enabled = true;
				//     chkShipNotice.Enabled = false;
			break;
			case OpsPOStatus.ApprovedtoOrder:
				//Approved can be Issued
				ImageButtonPrintPO2.Visible   = true;
				ImageButtonAddScan2.Visible   = true;
				ImageButtonQuestions2.Visible = true;
				chkAckOfPORec.Enabled         = true;
				//       chkPORec.Enabled = false;
				chkShipNoticeReq.Enabled = true;

				if (!checkAdvancedState())
					{
					checkForUnissue();
					}
       
				//       chkShipNotice.Enabled = false;
			break;
			case OpsPOStatus.IssuedWaitingforPackingSlip:
				//Waiting for Packing Slip. Scan can be added
				//It can be printed again
				//Member with privilege can unissue.
				ImageButtonPrintPO2.Visible     = true;
				ImageButtonPrintPreview.Visible = false;
				//ImgBtnCancelPO.Visible = true;
				ImageButtonAddScan2.Visible = true;

				if (!checkAdvancedState())
					{
					checkForUnissue();
					}

				LinkButton1.Visible = false;
				// Changing this to not show the button when it shouldn't be clicked...
				lbAllPacking.ClientVisible = NePOProg.LineCountActive(Convert.ToInt32(hidPOProgID.Value)) == 0;
				ASPxButtonRecAll.Visible   = true;
				//      chkAckOfPORec.Enabled = false;
				chkPORec.Enabled = true;
				//      chkShipNoticeReq.Enabled = false;
				chkShipNotice.Enabled = true;
			break;
			case OpsPOStatus.ReceivedWaitingforInvoice:
				//Waiting for Invoice, can be printed again, scan 
				//can be added, and it can be sent to waiting to be closed.
				ImageButtonPrintPO2.Visible     = true;
				ImageButtonAddScan2.Visible     = true;
				ImageButtonPrintPreview.Visible = false;
				//ImageButtonWaitClose.Visible = true;

				if (currentUser.AuthenticatedForPrivilege(OpsPrivilege.UnissuePOPurchaser))
				{
					Unissue2.Visible = false;
				}
				
				if(currentUser.AuthenticatedForPrivilege(OpsPrivilege.AlterPosAfterIssue))
				{
					if (!checkAdvancedState())
					{
						checkForUnissue(OpsPOStatus.ReceivedWaitingforInvoice);
					}
				}

				LinkButton1.Visible      = false;
				chkAckOfPORec.Enabled    = false;
				chkPORec.Enabled         = false;
				chkShipNoticeReq.Enabled = false;
				chkShipNotice.Enabled    = false;
			break;
			case OpsPOStatus.WaitingToBeClosed:
				LinkButton1.Visible         = false;
				ImageButtonPrintPO2.Visible = true;
				chkAckOfPORec.Enabled       = false;
				chkPORec.Enabled            = false;
				chkShipNoticeReq.Enabled    = false;
				chkShipNotice.Enabled       = false;
			break;
			case OpsPOStatus.Closed:
				ImageButtonPrintPO2.Visible = true;
			break;
			case OpsPOStatus.Questions:
				ImageButtonAnswer2.Visible  = true;
				ImageButtonPrintPO2.Visible = true;
				chkAckOfPORec.Enabled       = true;
				//          chkPORec.Enabled = false;
				chkShipNoticeReq.Enabled = true;
				//           chkShipNotice.Enabled = false;
			break;
			case OpsPOStatus.Cancelled:
				ImgBtnCancelPO2.Visible = false;	
			break;
			}
		if (Toolbox.Contains(intStatus, new []{	OpsPOStatus.NotIssued,
												OpsPOStatus.IssuedWaitingforPackingSlip,
												OpsPOStatus.ApprovedtoOrder,
												OpsPOStatus.Questions
												}))
			{
			ImgBtnCancelPO2.Visible = NePOProg.LineCount(Convert.ToInt32(hidPOProgID.Value), false) == 0;
			}
		if (Toolbox.Contains(intStatus, new []{	OpsPOStatus.IssuedWaitingforVendorConfirmation,
												OpsPOStatus.IssuedWaitingforCompleteDelivery
												}));
		{
			ImageButtonPrintPreview.Visible = false;
		}
		if (NePOProg.LineCount(Convert.ToInt32(hidPOProgID.Value)) == 0)
		{
			ImageButtonPrintPO2.OnClientClick = "alert('You can not print a purchase order with NO lines on it');event.preventDefault();";
		}
	}
	private void fill_integ_history(string _poprogid)
	{
		if (!string.IsNullOrEmpty(_poprogid))
		{

			DataTable dt = Toolbox.doSQL_dt(conn, @"CALL ds_integration_history(@v0,@v1);", new object[] { _poprogid, 2 });
			gv_integ_history.DataSource = dt;
			gv_integ_history.DataBind();
		}
	}
	protected void gv_integ_history_HtmlRowPrepared(object _sender, ASPxGridViewTableRowEventArgs _e)
	{
		Color last_color = Color.White;
		if (_e.RowType == GridViewRowType.Preview)
		{
			_e.Row.BackColor = last_color;
		}
		else
		{
			last_color = _e.Row.BackColor == Color.Empty ? Color.White : _e.Row.BackColor;
		}
	}
	private void insert_grid_information(string _poprogid)
	{
		if (!IsPostBack)
		{
			ddlPartList.Value = 0;
		}
		txtPartHistory.InnerHtml = HistoryDetails(_poprogid);

	}

	private string HistoryDetails(string poid)
	{
		var partnotes = new StringBuilder("<div>");
		var dateexp = "";
		var idFilter = ddlPartList.Value == null || ddlPartList.Value.ToString() == "0"
						? ""
						: " AND (reference_no = " + ddlPartList.Value + " OR reference_no = 1 AND part_no = (SELECT po_details_part_no FROM po_details_current WHERE po_details_id = " + ddlPartList.Value + "))";
		var parthistory = Toolbox.doSQL_dt(string.Format(@" SELECT a.part_no, a.vendor_part_no, a.description,a.diff_qty_ordered, a.qty_ordered, a.diff_qty_received, a.qty_received, a.cost, a.dateofchange, a.part_added, b.member_fullname, a.notes, a.reference_no, a.dateexpectedchange, c.woprog_bvwo, a.is_gl_account FROM poprog_part_history a LEFT JOIN member b ON a.member_id = b.member_id LEFT JOIN woprog c ON a.woprog_id = c.woprog_id WHERE a.poprog_id = @v0 {0}  ORDER BY a.dateofchange desc, a.part_no, a.reference_no", idFilter), new object[] { poid });
		var lastDate = new DateTime();
		foreach (DataRow row in parthistory.Rows)
		{
			var thisDate = DateTime.Parse(row["dateofchange"].ToString());
			var qty_ordered = Convert.ToDouble(row["qty_ordered"]);
			var diff_qty_ordered = Convert.ToDouble(row["diff_qty_ordered"]);
			var qty_received = Convert.ToDouble(row["qty_received"]);
			var diff_qty_received = Convert.ToDouble(row["diff_qty_received"]);
			if (thisDate.Date != lastDate.Date)
			{
				lastDate = thisDate.Date;
				partnotes.AppendFormat("</div><div style='font-size:12pt;font-weight:500;'>{0:yyyy-MM-dd}</div><div style='padding-left:25px;' title='{0:yyyy-MM-dd}'>", thisDate);
			}
			partnotes.AppendFormat(row["part_added"].ToString() == "1"
				? "{0} was added by <b>{1}</b> @ <b>{2:HH:mm:ss}</b><br/>\n"
				: "{0} was modified by <b>{1}</b> @ <b>{2:HH:mm:ss}</b><br/>\n", row["part_no"], row["member_fullname"], thisDate);
			partnotes.AppendFormat("<b>Vendor Part:</b> {0}<br/>\n", row["vendor_part_no"]);
			partnotes.AppendFormat("<b>Description:</b> {0}<br/>\n", row["description"]);
			partnotes.AppendFormat("<b>WO:</b> {0}<br/>\n", row["woprog_bvwo"]);
			if (diff_qty_ordered != 0)
			{
				partnotes.AppendFormat("<b>Ordered:</b> {0}<br/>\n", diff_qty_ordered > 0 ? "+" + diff_qty_ordered : diff_qty_ordered.ToString(CultureInfo.InvariantCulture));
			}
			if (diff_qty_received != 0)
			{
				partnotes.AppendFormat("<b>Received:</b> {0} (Total: {1}/{2})<br/>\n", diff_qty_received > 0 ? "+" + diff_qty_received : diff_qty_received.ToString(CultureInfo.InvariantCulture), qty_received, qty_ordered);
			}
			partnotes.AppendFormat("<b>Date Expected:</b> {0}<br/>\n", row["dateexpectedchange"] is DBNull ? "" : Convert.ToDateTime(row["dateexpectedchange"]).ToString("yyyy-MM-dd"));
			partnotes.AppendFormat("<b>Cost at time of edit:</b> {0}<br/>\n", row["cost"]);
			if (row["notes"] != "")
			{
				partnotes.AppendFormat("<b>Notes:</b> {0}<br/>\n", row["notes"]);
			}
			partnotes.Append("<br/>\n");
		}

		return partnotes.ToString();
	}

	protected void ASPxcbVendor_SelectedIndexChanged(object _sender, EventArgs _e)
	{
		var vendorid = Convert.ToInt32(ddl_vendor.Value.ToString());

		if (vendorid != 0)
		{
			hidVendorID.Value = vendorid.ToString();
			var vendid = ddl_vendor.Value.ToString();
			var vendor = new NEVendor(vendorid);
			lblVendorNoDisp.InnerHtml = @"<a href='javascript:boing(""/#/opens/11/vendors/" + vendor.ID + @""", ""Vendor"", 950, 1024);'>" + vendor.vendor_number + "</a>";

			lblCreditLimitDisp.Text = vendor.Vendor_Credit_Limit.ToString();
			lblCreditTypeDisp.Text = vendor.Vendor_Credit_Type.ToString();
			if (vendor.Vendor_Term_ID != 0)
			{
				ASPxComboBoxPayment.Value = vendor.Vendor_Term_ID;
			}
			else
			{
				ASPxComboBoxPayment.Value = 17;
			}
			ddlContacts.DataSource = Toolbox.doSQL_dt(conn, @"Select contact_id,contact_name as contact from contact  where contact_cust_id =@v0 and contact_type = 'Vendor'", new object[] { hidVendorID.Value });
			ddlContacts.DataBind();
			var str_nameand_address = "SELECT vendor_number, vendor_name, Address_Addr1, ";
			str_nameand_address += "Address_City, Address_Prov, Address_Postal, ";
			str_nameand_address += "Address_Country, Address_PhoneArea, ";
			str_nameand_address += "Address_PhoneFirst, Address_PhoneLast, Address_Email ";
			str_nameand_address += "FROM vendor, address ";
			str_nameand_address += "WHERE Vendor_ID =@v0 AND address_table_id = vendor_id AND address_table = 'Vendor' AND active = 1";
			var vendaddresstable = Toolbox.doSQL_dt(conn, str_nameand_address, new object[] { vendid });
			//vendaddresstable = _tools.recode_datatable(vendaddresstable, true);
			foreach (DataRow vendorrowinfo in vendaddresstable.Rows)
			{
				lblVendDetails.Text = vendorrowinfo["vendor_name"] + "<br/>";
				lblVendDetails.Text += vendorrowinfo["Address_Addr1"] + "<br/>";
				lblVendDetails.Text += vendorrowinfo["Address_City"] + " ";
				lblVendDetails.Text += vendorrowinfo["Address_Prov"] + "<br/>";
				lblVendDetails.Text += vendorrowinfo["Address_Country"] + " ";
				lblVendDetails.Text += vendorrowinfo["Address_Postal"] + "<br/>";
				lblVendDetails.Text += vendorrowinfo["Address_PhoneArea"] + "-";
				lblVendDetails.Text += vendorrowinfo["Address_PhoneFirst"] + "-";
				lblVendDetails.Text += vendorrowinfo["Address_PhoneLast"] + "<br/>";
				lblVendDetails.Text += vendorrowinfo["Address_Email"] + "<br/>";
			}
			errorlabel.Visible = false;
		}
	}
	protected void ASPxcbwo_SelectedIndexChanged(object _sender, EventArgs _e)
	{
		var woprogid = 0;
		lblWOInfo.Text = "";
		hidWOProgID.Value = "0";
		try
		{
			woprogid = Convert.ToInt32(ASPxcbwo.Value.ToString());
		}
		catch (Exception exce) { Toolbox.do_errorLog_errorStack(exce); }
		if (woprogid != 0)
		{
			set_work_order_details(woprogid.ToString());
			hidWOProgID.Value = woprogid.ToString();
		}
	}
	protected void ASPxCCBCompany_SelectedIndexChanged(object _sender, EventArgs _e)
	{
		var companyid = 0;
		int.TryParse(ddl_company.Value.ToString(), out companyid);
		hidCompanyID.Value = companyid.ToString();
		get_open_work_orders(companyid.ToString());
		var company = new NeBusinessUnit(companyid);
		lblShippingData.Text = company.name + "\n";
		lblShippingData.Text += company.address + "\n";
		lblShippingData.Text += company.city + "\n";
		lblShippingData.Text += company.Prov + ", ";
		lblShippingData.Text += company.postal + "\n";
		if (company.country == "CDN")
		{
			lblShippingData.Text += "Canada";
		}
		else
		{
			lblShippingData.Text += "USA";
		}
		hidManualAddress.Value = lblShippingData.Text;

		txtShippingPostCode.Text = company.postal;
		txtShippingAddress1.Text = company.address;
		txtShippingAddress2.Text = " ";
		txtShippingCity.Text = company.city;

		txtAddressBox.Text = company.address + "," + company.city + "\n";

		txtAddressBox.Text += company.Prov + ", ";
		if (company.country == "CDN")
		{
			txtAddressBox.Text += "Canada\n";
			txtShippingCountry.Text = "Canada\n";
			txtShippingProv.Text = company.Prov;

		}
		else
		{
			txtAddressBox.Text += "USA\n";
			txtShippingCountry.Text = "USA\n";
			txtShippingProv.Text = company.State;

		}
		txtAddressBox.Text += company.postal;
	}
	protected void LinkButton1_Click(object _sender, EventArgs _e)
	{
		ASPxpucAddressSel.ShowOnPageLoad = true;
	}

	protected void lblAddContact_Click(object _sender, EventArgs _e)
	{
		if (this.ddl_vendor.Value != null)
		{
			popAddContact.ShowOnPageLoad = true;
		}
		else
		{
			//ClientScript.RegisterStartupScript(this.GetType(), "Info", "alert( 'Please select a valid Vendor!');", true);
			ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Please select a valid vendor!')", "Server"), true);
			popAddContact.ShowOnPageLoad = false;
		}
	}

	protected void popAddContactSave_Click(object _sender, EventArgs _e)
	{

		if (!string.IsNullOrWhiteSpace(this.txt_ContactName.Text))
		{
			var this_contact = new NEContact
			{
				Contact_Cust_ID = Convert.ToInt32(this.ddl_vendor.Value),
				Contact_Type = "Vendor",
				Contact_Status = "Active",
				Contact_Name = this.txt_ContactName.Text,
				address_id = 0,
				customer_id = Convert.ToInt32(this.ddl_vendor.Value),
				status = "Active",
				status_id = 1,
				name = this.txt_ContactName.Text,
				type = "Vendor"
			};

			var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM contact WHERE contact_name = @v0  AND contact_cust_id = @v1  AND contact_type = 'Vendor'", new object[] { this.txt_ContactName.Text, this_contact.Contact_Cust_ID });
			if (c > 0)
			{
				ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('This contact is existing in the database.')", "Server"), true);
				return;//return new DataExtra("Duplicate Contact Detected");
			}

			// this_contact.Contact_Title = txt_ContactTitle.Text;
			this_contact.Contact_Email = txt_ContactEmail.Text;
			this_contact.Contact_CellPhone = txt_ContactCellphone.Text;
			try
			{
				//	var temp_address_id =
				//		Toolbox.doSQL_int(
				//			@"SELECT IFNULL(MAX(address_id),0) FROM address where address_table = 'Customer' AND address_table_id = @v0 ",
				//			new object[] { model.Customer_id });
				//this_contact.address_id = temp_address_id;
				this_contact.AddNEContact(this_contact);
				var d = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM contact WHERE contact_name = @v0  AND contact_cust_id = @v1  AND contact_type = 'Vendor'", new object[] { this.txt_ContactName.Text, this_contact.Contact_Cust_ID });
				if (d > 0)
				{
					ddlContacts.DataSource = Toolbox.doSQL_dt(conn, @"Select contact_id,contact_name as contact from contact  where contact_cust_id =@v0 and contact_type = 'Vendor'", new object[] { hidVendorID.Value });
					ddlContacts.DataBind();
					popAddContact.ShowOnPageLoad = false;
				}

			}
			catch
			{
				// return new DataExtra("There was an error saving this contact.");
			}

			this.txt_ContactName.Text = "";
			// this.txt_ContactTitle.Text = "";
			this.txt_ContactEmail.Text = "";
			// this.txt_ContactExt.Text = "";
			this.txt_ContactCellphone.Text = "";
		}
		else
		{
			// ClientScript.RegisterStartupScript(this.GetType(), "Info", "alert('Please use a valid Contact Name!');", true);
			ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, string.Format("alert('Please use a valid contact name!')", "Server"), true);
		}





	}

	protected void ImageButton4_Click(object _sender, ImageClickEventArgs _e)
	{
		//check_for_changes();
		var addr1 = txtShippingAddress1.Text;
		var addr2 = txtShippingAddress2.Text;
		var city = txtShippingCity.Text;
		var prov = txtShippingProv.Text;
		var country = txtShippingProv.Text;
		var postcode = txtShippingPostCode.Text;

		if (hidPOProgID.Value != "0")
		{
			Toolbox.doSQL_void(conn, @"UPDATE poprog_header SET Shipping_Addr1 = @v0 ,Shipping_Addr2=@v1,Shipping_City=@v2, Shipping_Province=@v3,Shipping_Country=@v4,Shipping_Postal=@v5 WHERE poprog_id = @v6 ", new object[] { addr1, addr2, city, prov, country, postcode, hidPOProgID.Value });

		}

		var builtaddress = "";
		builtaddress += addr1 + "\n" + addr2 + "\n" + city + " " + prov + "\n" + country + " " + postcode + "\n";
		lblShippingData.Text = builtaddress;
		hidManualAddress.Value = builtaddress;


		ASPxpucAddressSel.ShowOnPageLoad = false;
	}
	protected void ImageButton7_Click(object _sender, ImageClickEventArgs _e)
	{
		Toolbox.doSQL_void(conn, @"Insert into poprogcomment (poprogcomment_poprog_id,poprogcomment_text,poprogcomment_datetime,poprogcomment_member_id)  values (@v0,@v1,@v2,@v3)", new object[] { hidPOProgID.Value, txtPONotes.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), currentUser.id });
		txtPONotes.Text = "";
		build_chat_history();
	}

	private void build_chat_history()
	{
		var str = "<table style='font-family: Arial;'>";
		var color_ = "";
		var str_posql = "SELECT a.poprogcomment_text note, fun_time(a.poprogcomment_datetime) dt, member_name(b.member_id) as name ";
		str_posql += "FROM poprogcomment AS a, member AS b where a.poprogcomment_poprog_id = @v0";
		str_posql += " AND a.poprogcomment_member_id = b.member_id order by poprogcomment_id desc";
		var notes = Toolbox.doSQL_dt(conn, str_posql, new object[] { hidPOProgID.Value });
		foreach (DataRow dr in notes.Rows)
		{
			if (currentUser.FullName == dr[2].ToString())
				color_ = "blue";
			else
				color_ = "red";
			str += "<tr ><td style='vertical-align:top;'> " + dr[2] + " </td><td style='vertical-align:top;'>(" + dr[1] + ") - </td><td style='vertical-align:top; font-family: color: " + color_ + "; Arial; font-style: italic;' >" + dr[0].ToString().Replace("\n", "<br/>") + " </td></tr><tr><td colspan='3' style=' border-bottom:solid; border-bottom-color: lightgray;'></td></tr>";
		}
		str += "</table>";
		div_comments.InnerHtml = str;
	}
	protected void btnCancel_Click(object _sender, EventArgs _e)
	{
		ASPxpcScanDisplay.ShowOnPageLoad = false;
	}
	protected void ImageButtonWaitPackingSlip_Click(object _sender, ImageClickEventArgs _e)
	{
		//check_for_changes();
		NePOProg.UpdateStatus(Convert.ToInt32(hidPOProgID.Value), OpsPOStatus.IssuedWaitingforPackingSlip);
		lblStatusDisp.Text = "ISSUED - WAITING FOR PACKING SLIP";
		NePOProg.status_log.add(Convert.ToInt32(hidPOProgID.Value), currentUser.id, OpsPOStatus.IssuedWaitingforPackingSlip);
		NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, "Status Changed: Issued - Waiting for packing slip");
		response_helper.redirect("~/sections/purchaseorder/po_prog_edit.aspx?business_unit_id=" + hidCompanyID.Value, "_top", "");
	}
	protected void ImageButtonInvoiceSlip_Click(object _sender, ImageClickEventArgs _e)
	{

		var waiting_lines = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM po_details_current WHERE po_details_poprog_id = @v0  AND po_details_qty_ordered != po_details_qty_received", new object[] { hidPOProgID.Value });
		if (waiting_lines > 0)
		{
			throw new Exception("You cannot change the status of this PO to 'Received' if all items havent't been received");
		}
		var po = new NePOProg();
		po.poprog_status = OpsPOStatus.ReceivedWaitingforInvoice;
		po.poprog_update();
		NEPO_Details_Current.BalanceQuantities(Convert.ToInt32(hidPOProgID.Value));
		lblStatusDisp.Text = "RECEIVED - WAITING FOR INVOICE SLIP";
		NePOProg.status_log.add(Convert.ToInt32(hidPOProgID.Value), currentUser.id, OpsPOStatus.ReceivedWaitingforInvoice);
		NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, "Status Changed: Received - Waiting for Invoice");
		response_helper.redirect("~/sections/purchaseorder/po_prog_edit.aspx?business_unit_id=" + hidCompanyID.Value, "_top", "");
	}
	protected void ImageButton_PP_Click(object _sender, ImageClickEventArgs _e)
	{
	}
	protected void ImageButtonOkayToOrder_Click(object _sender, ImageClickEventArgs _e)
	{
		if (hidPOProgID.Value == "") { set_error("PO is not defined."); return; }
		var po_id = Convert.ToInt32(hidPOProgID.Value);
		var po = new NePOProg(po_id);
		var branch = new NeBusinessUnit(po.business_unit_id);
		//Check Status
		if (po.poprog_status == OpsPOStatus.Cancelled)
		{
			set_error("This PO has been cancelled by a user and can no longer be issued or approved.");
			return;
		}
		update_po();
		var value = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_ordered * po_details_cost), 0) FROM po_details_current WHERE po_details_poprog_id = @v0 ", new object[] { hidPOProgID.Value });
		var n_limits = Toolbox.doSQL_dt(conn, @"SELECT * from business_unit_po_dist WHERE business_unit_id = @v0 AND amount_to >= @v1 and member_id = @v2   ", new[] { ddl_company.Value, value, currentUser.id });
		if (n_limits.Rows.Count > 0)// if this person is authorized for this level, push it right thorugh.. 
		{
			#region Line count check
			var lineCount = NePOProg.LineCount(po_id);
			if (lineCount == 0)
			{
				set_error("PO's without line items cannot be approved.");
				return;
			}
			#endregion Line count check
			var pennyLineCount = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM po_details_current WHERE po_details_poprog_id = @v0 AND po_details_cost = 0.01 AND po_details_part_no != 0", new object[] { po_id });
			if (pennyLineCount > 0)
			{
				set_error("PO's with line items that have a cost of a penny cannot be approved.");
				return;
			}
			var commentsWithCost = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM po_details_current WHERE po_details_poprog_id = @v0 AND po_details_cost > 0 AND po_details_part_no = 0", new object[] { po_id });
			if (commentsWithCost > 0)
				{
				set_error("PO's with comments that have costs cannot be approved.");
				return;
				}
			lblStatusDisp.Text = "Approved To Order";
			order_approved(po_id, currentUser);
			var msg = new NeEMail
					{
					To     = new NeMember(po.poprog_cutby_member_id).NEEmail,
					From   = currentUser.NEEmail,
					isHTML = false,
					Subject = $"PO#:{po.business_unit_id.ToString().PadLeft(3, '0') + "-" + po.poprog_bvpo.TrimStart('0')} has been approved"
					};
		if (po.poprog_cutby_member_id != currentUser.id)
			{
				var body = new StringBuilder();
				body = body.AppendFormat(@"
<div style='font-family:arial;font-size:12px'>
	<b>Purchase Order: </b><a href='{4}/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}' target = 'blank'>{0}</a><br/>
	<b>Vendor: </b>{1}<br/>
<b>Description: </b>{3}<br/>
	<b>Approved By: </b>{2}<br/></div>",
							po.poprog_id,
						   new NEVendor(po.poprog_vendor_id).Vendor_Name,
							currentUser.FullName,
							po.poprog_order_description,
							Toolbox.app_setting("Domain")
						);
				msg.Body = body.ToString();
				msg.Send();
			}
			Toolbox.doSQL_void(conn, @"UPDATE passport SET active = false, valid = true WHERE url_yes LIKE CONCAT('%po_id=',@v0 ,'&%')", new object[] { po.poprog_id });
			response_helper.redirect($"~/sections/purchaseorder/po_prog_edit.aspx?business_unit_id={hidCompanyID.Value}", "_top", "");
			return;
		}

	var member_id = 0;
	//first send it to the person who has approval for this business unit and this level

	n_limits = Toolbox.doSQL_dt(conn, @" 
SELECT
	a.member_id,
	a.business_unit_id,
	a.amount_from,
	a.amount_to
FROM
	business_unit_po_dist a
LEFT JOIN member b ON a.member_id = b.member_id 	
WHERE
	a.business_unit_id = @v0 AND
	@v1 BETWEEN a.amount_from AND a.amount_to
	AND b.member_status = 'Active'
ORDER BY 
	a.amount_to ", new[] { ddl_company.Value, Math.Abs(value) });
	foreach (DataRow dr in n_limits.Rows) // who is this users supervisor who has permission?
		{
		if (!VacationInterface.is_on_vacation(Convert.ToInt32(dr["member_id"]), DateTime.Now))
			{
			member_id = Convert.ToInt32(dr["member_id"]);
			break;
			}
		}
	if (member_id == 0) // if there is no one on record!
		{
		if (!VacationInterface.is_on_vacation(branch.branch_manager.id, DateTime.Now))
			{
			member_id = branch.branch_manager.id;
			}
		else
			{
			member_id = !VacationInterface.is_on_vacation(new NeMember(branch.branch_manager.reports_to).id, DateTime.Now)
				? new NeMember(branch.branch_manager.reports_to).id
				: new NeMember(branch.branch_manager.reports_to).reports_to;
			}
		}

	// Check make sure the person with this member_id have the permission to see the page because the email will send to him/her.
	// If no permission we will add one automatically for him/her.
	var thisPerson = new NeMember(member_id);
	if (!thisPerson.AuthenticatedForPrivilege(OpsPrivilege.AllowPoApprovalOverAPresetLimit))
		{
		var upa = new NEUserPage();
		if(!thisPerson.AuthenticatedForPage(OpsPage.Purchases))
			{
			upa = new NEUserPage
					{
					admin = new NeMember(1),
					user = thisPerson,
					user_id = thisPerson.id,
					page_id = OpsPage.Purchases,
					type_id = 1
					};
			upa.save();
			}
		else
			{
			upa	= new NEUserPage(thisPerson.id, OpsPage.Purchases, 1);
			}
		var upr = new NEUserPrivilege
					{
					admin   = currentUser,
					user    = thisPerson,
					user_id = member_id,
					typepage_id = upa.id,
					privilege_id = OpsPrivilege.AllowPoApprovalOverAPresetLimit,
					type_id = 1
					};
		upr.save();
		}

	var    line_items = new StringBuilder();
	double total_po   = 0;
	var dt = Toolbox.doSQL_dt(conn, @" 
SELECT 
	a.po_details_id AS id, 
	a.po_details_part_no AS part_no, 
	a.po_details_description AS description, 
	a.po_details_qty_ordered AS qty, 
	a.po_details_cost AS cost, 
	a.po_details_qty_ordered*a.po_details_cost AS total_cost, 
	IF(a.expense_category_id > 0, c.expense_category_id, IFNULL(b.woprog_bvwo,'Empty WO?')) wo, 
	IF(a.expense_category_id > 0, 'Expense Category', IFNULL(b.woprog_customername,'Empty Customer?')) customer, 
	IF(a.expense_category_id > 0, c.name, IFNULL(b.woprog_description,'Empty Description?')) wo_description 
FROM 
	po_details_current a 
LEFT JOIN 
	woprog b ON a.po_details_woprog_id = b.WOProg_ID and a.is_gl_account = 0
LEFT JOIN 
	expense_category c ON a.expense_category_id = c.expense_category_id  
WHERE 
	a.po_details_poprog_id =@v0", new object[] { po.poprog_id });
	foreach (DataRow dr in dt.Rows)
		{
		line_items.Append($@"
<tr>
	<td>{dr["part_no"]}</td>
	<td>{dr["description"]}</td>
	<td align='center'>{dr["qty"]}</td>
	<td align='right'>{Convert.ToDouble(dr["cost"]):C2}</td>
	<td align='right'>{Convert.ToDouble(dr["total_cost"]):C2}</td>
	<td align='center'>{dr["wo"]}</td>
	<td align='left' nowrap='nowrap'>{dr["customer"]}</td>
	<td align='left'>{dr["wo_description"]}</td>
</tr>");
		total_po   += Convert.ToDouble(dr["total_cost"]);
		}
	var bm = new NeMember(Convert.ToInt32(member_id));
	var m = new NeEMail
				{
				To   = bm.NEEmail,
				From = currentUser.NEEmail,
				Body = $@"
<p><span style='font-family: arial; font-size:14px'>
{bm.FirstName},<br/>
{currentUser.FullName} has requested approval to proceed with PO#:{po.business_unit_id.ToString().PadLeft(3, '0') + "-" + po.poprog_bvpo.TrimStart('0')}.<br/><br/>
<table style='font-family: arial; font-size:14px' cellspacing='2' cellpadding='2'>
<tr>
		<td><b>Branch:</b></td>
		<td>{new NeBusinessUnit(po.business_unit_id).name}</td>
	</tr>	
<tr>
		<td><b>Vendor:</b></td>
		<td>{new NEVendor(po.poprog_vendor_id).Name}</td>
	</tr>
	<tr>
		<td><b>PO Description:</b></td>
		<td>{po.poprog_order_description}</td>
	</tr>
	<tr>
		<td><b>PO Amount:</b></td>
		<td>{value:c2}</td>
	</tr>
</table><br/><br/>
<b><u>Line Items</u><b><br/>
<table cellspacing='2' cellpadding='2' text-align='center' style='font-family: arial; font-size:11px'>
	<tr>
		<td align='center'><b>Part No</b></td>
		<td align='center'><b>Description</b></td>
		<td align='center'><b>Qty</b></td>
		<td align='center'><b>Cost Each</b></td>
		<td align='center'><b>Total Cost</b></td>
		<td align='center'><b>WO/GL</b></td>
		<td align='center'><b>Cust/Acct</b></td>
		<td align='center' nowrap='nowrap'><b>Description</b></td>
	</tr>
{line_items}
<tr><td></td><td></td><td></td><td></td><td align='right'>{total_po:c2}</td></tr>
</table><br/><br/>
<div>Choosing approve below will automatically approve this purchase order.</div>
<div>Choosing deny will just take you to the PO screen.</div></span></p>
",
				URLyes       = $"/sections/purchaseorder/po_prog_add.aspx?a=BM_approval&po_id={po_id}&type=approve",
				URLno        = $"/sections/purchaseorder/po_prog_add.aspx?a=BM_approval&po_id={po_id}&type=deny",
				isHTML       = true,
				Subject      = "PO needs approval",
				to_member_id = bm.id
				};
	m.file_passport();
	//Change Status
	wait_order_approved(po_id, currentUser);
	set_error("A request-for-approval email has been sent to " + bm.FullName);
	}
	private void set_error(string _msg)
	{
		ScriptManager.RegisterStartupScript(this, GetType(), "Error", $@"alert(""{_msg}"");", true);
		return;
	}
	protected void ImageButton_BMApp_Click(object _sender, ImageClickEventArgs _e)
	{
		if (hidPOProgID.Value == "" || hidPOProgID.Value == "0") { set_error("PO is not defined.  Please refresh the screen (F5)"); return; }
		var po_id = Convert.ToInt32(hidPOProgID.Value);
		var value = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_ordered * po_details_cost), 0) FROM po_details_current WHERE po_details_poprog_id = @v0 ", new object[] { hidPOProgID.Value });
		if (!NePOProg.HasPermissionToApprove(Convert.ToInt32(ddl_company.Value), value, currentUser.id))
			{
				set_error("Sorry, you are not authorized to approve this PO");
				return;
			}
		else
		{
			lblStatusDisp.Text = "Approved To Order";
			order_approved(po_id, currentUser);
			response_helper.redirect(string.Format("~/sections/purchaseorder/po_prog_edit.aspx?business_unit_id={0}", hidCompanyID.Value), "_top", "");
		}
	}
	private void wait_order_approved(int _po_id, NeMember _member)
	{
		NePOProg.UpdateStatus(_po_id, OpsPOStatus.WaitingBMApproval);
		NePOProg.status_log.add(_po_id, _member.id, OpsPOStatus.WaitingBMApproval);
		NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, "Status Changed: Waiting For Approval");
	}
	private void order_approved(int _po_id, NeMember _member)
	{
		update_po_totals();
		var thisPO = new NePOProg(_po_id);
		NePOProg.UpdateStatus(_po_id, OpsPOStatus.ApprovedtoOrder);
		NePOProg.status_log.add(_po_id, currentUser.id, OpsPOStatus.ApprovedtoOrder, thisPO.poprog_total_cost);
		NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, "Status Changed: Approved to order");
	}
	protected void Unissue_Click(object _sender, ImageClickEventArgs _e)
	{
		NePOProg.UpdateStatus(Convert.ToInt32(hidPOProgID.Value), OpsPOStatus.NotIssued);
		lblStatusDisp.Text = "Not Issued";
		NePOProg.status_log.add(Convert.ToInt32(hidPOProgID.Value), currentUser.id, OpsPOStatus.NotIssued);
		NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, "Status Changed: Unissued");
		response_helper.redirect("~/sections/purchaseorder/po_prog_edit.aspx?business_unit_id=" + hidCompanyID.Value, "_top", "");
	}
	protected void ImgBtnCancelPO_Click(object _sender, ImageClickEventArgs _e)
	{
		ASPxPopupCancel.HeaderText = "Cancel PO";
		lblReason.ForeColor = Color.Black;
		lblReason.Font.Bold = false;
		lblReason.Text = "Please provide the reason you are cancelling this PO?";
		CancelNotesSave.Visible = true;
		ASPxPopupCancel.ShowOnPageLoad = true;
	}

	protected void Vendor_Contact_Cancel(object _sender, EventArgs _e)
	{
		popAddContact.ShowOnPageLoad = false;
	}

	protected void CancelNotesSave_Click(object _sender, ImageClickEventArgs _e)
	{
		if (txtReason.Text == null || txtReason.Text.Length < 10)
		{
			ScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "alert('Please enter a decent description explaining why you are closing this PO');", true);
			return;
		}
		var prognotes = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
		//if (hid_ts.Value != prognotes.poprog_ts.Ticks.ToString())
		//{
		//	throw new Exception("This purchase order has been edited by someone else since you loaded it. Please refresh the page");
		//}
		prognotes.eventtext = "PO Cancelled : " + txtReason.Text;
		prognotes.notes_poprogid = Convert.ToInt32(hidPOProgID.Value);
		prognotes.notes_memberid = currentUser.id;
		prognotes.SaveNotes();
		//txtPONotes.Text = "";
		txtPOHistory.Text = prognotes.GetNotesData(hidPOProgID.Value);
		var shouldRedirect = true;
		cancel_the_po(ref shouldRedirect);
		if (shouldRedirect)
		{
			ASPxPopupCancel.ShowOnPageLoad = false;
			Response.Redirect("/sections/purchaseorder/po_prog_edit.aspx?business_unit_id=" + prognotes.business_unit_id);
		}
	}
	private void cancel_the_po(ref bool shouldRedirect)
	{
		var company = new NeBusinessUnit(hidCompanyID.Value);
		var dsn = company.DSN;
		var c = NePOProg.LineCountActive(Convert.ToInt32(hidPOProgID.Value));
		if (c > 0)
		{
			ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('Before canceling this PO you must manually remove or deactivate all of the lines.');", true);
			shouldRedirect = false;
			return;
		}
		var progress = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
		var woupdate = "";
		double woqty = 0;

		//If the PO has been Issued Remove the items from the Work Order by adding a negative quantity
		var detaillist = Toolbox.doSQL_dt(conn, @"SELECT * FROM po_details_current  WHERE po_details_poprog_id =@v0 and is_gl_account=false", new object[] { progress.poprog_id.ToString() });
		foreach (DataRow detailrow in detaillist.Rows)
		{
			woqty = Convert.ToDouble(detailrow["po_details_qty_ordered"].ToString()) *
					Convert.ToDouble(detailrow["po_details_vendor_qty_per"].ToString());
			Toolbox.doSQL_void(conn, @"
UPDATE 
	wo_detail_current
SET 
	wo_detail_current_qty_committed = wo_detail_current_qty_committed - @v0, 
	wo_detail_current_qty_invoiced = wo_detail_current_qty_invoiced - @v1, 
	wo_detail_current_origin = CONCAT(wo_detail_current_origin, ' PO ',@v2,'cancelled:',@v3,' items') 
WHERE 
	wo_detail_current_woprog_id = @v4 AND 
	wo_detail_current_master_id =@v5  AND 
	wo_detail_current_origin LIKE CONCAT('%',@v6,'%')",
				new[]
					{
							woqty,
							woqty,
							progress.poprog_bvpo,
							woqty,
							detailrow["po_details_woprog_id"],
							detailrow["po_details_part_no"],
							progress.poprog_bvpo
					}
			);
		}
		NePOProg.UpdateStatus(Convert.ToInt32(hidPOProgID.Value), OpsPOStatus.Cancelled);
		lblStatusDisp.Text = "Cancelled";
		NePOProg.status_log.add(Convert.ToInt32(hidPOProgID.Value), currentUser.id, OpsPOStatus.Cancelled);
		NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, "Status Changed: Cancelled");
		lblStatusDisp.Text = "CANCELLED!";
		
	}
	protected void ImageButtonQuestions_Click(object _sender, ImageClickEventArgs _e)
	{

		NePOProg.UpdateStatus(Convert.ToInt32(hidPOProgID.Value), OpsPOStatus.Questions);
		lblStatusDisp.Text = "Questions";
		NePOProg.status_log.add(Convert.ToInt32(hidPOProgID.Value), currentUser.id, OpsPOStatus.Questions);
		NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, "Status Changed: Sent to Questions bucket");
		response_helper.redirect("~/sections/purchaseorder/po_prog_edit.aspx?business_unit_id=" + hidCompanyID.Value, "_top", "");
	}
	protected void ImageButtonAnswer_Click(object _sender, ImageClickEventArgs _e)
	{
		NePOProg.UpdateStatus(Convert.ToInt32(hidPOProgID.Value), OpsPOStatus.NotIssued);
		lblStatusDisp.Text = "Answered";
		NePOProg.status_log.add(Convert.ToInt32(hidPOProgID.Value), currentUser.id, OpsPOStatus.NotIssued);
		NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, "Status Changed: Answered - Back to Not Issued");
		response_helper.redirect("~/sections/purchaseorder/po_prog_edit.aspx?business_unit_id=" + hidCompanyID.Value, "_top", "");
	}
	protected void lbAllPacking_Click(object _sender, EventArgs _e)
	{
		//check_for_changes();
		var rowsincomplete = 0;
		var linecounter = 0;
		var received = false;
		var poprog = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
		try
		{
			var details = Toolbox.doSQL_dt(conn, @"SELECT po_details_line_active FROM po_details_current  WHERE po_details_part_no != 0 and po_details_poprog_id=@v0", new object[] { poprog.poprog_id });
			foreach (DataRow row in details.Rows)
			{
				if (row["po_details_line_active"].ToString() == "1")
				{
					rowsincomplete++;
				}
				linecounter++;
			}
			if (rowsincomplete == 0)
			{
				received = true;
			}
			var tempcompany = new NeBusinessUnit(poprog.business_unit_id);
		}
		catch
		{
			ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('Error pulling up PO Details to check if lines are complete.');", true);
			return;
		}
		if (!received)
		{
			ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('Not all lines are marked as complete.');", true);
			return;
		}
		NePOProg.UpdateStatus(Convert.ToInt32(hidPOProgID.Value), OpsPOStatus.ReceivedWaitingforInvoice);
		NEPO_Details_Current.BalanceQuantities(Convert.ToInt32(hidPOProgID.Value));
		NePOProg.status_log.add(Convert.ToInt32(hidPOProgID.Value), currentUser.id, OpsPOStatus.ReceivedWaitingforInvoice);
		NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, "Status Changed: Received-Waiting for Invoice");
		response_helper.redirect("~/sections/purchaseorder/po_prog_edit.aspx?business_unit_id=" + hidCompanyID.Value, "_top", "");
	}
	protected void receive_all(object _sender, EventArgs _e)
	{
		var po_dt = Toolbox.doSQL_dt(conn, @" SELECT a.*, b.poprog_vendor_id, b.business_unit_id FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE (a.po_details_qty_ordered > a.po_details_qty_received OR a.po_details_part_no = 0) AND a.po_details_poprog_id = @v0 ", new object[] { hidPOProgID.Value });
		var sb_error = new StringBuilder();
		var should_process = true;
		var WorkingBusinessUnit = new NeBusinessUnit(hidCompanyID.Value);
		var WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
		foreach (DataRow dr in po_dt.Rows)
		{
			var rec_no = Toolbox.ReturnZeroIfNull_int(dr["po_details_rec_no"]);
			var part_no = Toolbox.ReturnZeroIfNull_int(dr["po_details_part_no"]);
			var vendor_code = Toolbox.ReturnBlankIfNull_string(dr["po_details_vendor_part_no"]);
			var vendor_id = Toolbox.ReturnZeroIfNull_int(dr["poprog_vendor_id"]);
			var qty_per = Toolbox.ReturnZeroIfNull_double(dr["po_details_vendor_qty_per"]);
			var cost = Toolbox.ReturnZeroIfNull_double(dr["po_details_cost"]);
			var business_unit_id = Toolbox.ReturnZeroIfNull_int(dr["business_unit_id"]);
			/**
             * Ricky Patel 01/06/2017
             * Warehouse business unit ID
             */
			WorkingBusinessUnit = new NeBusinessUnit(business_unit_id);
			WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
			// Check for zero sells
			if (cost / qty_per <= 0.0005 && part_no > 0)
			{
				sb_error.AppendFormat(@"\n\nRec #{0} - Part #{1} has a vendor qty per of {2} and a cost of ${3}, this will result in a zero sell price.. Please fix.", rec_no, part_no, qty_per, cost);
				should_process = false;
			}
			// Check for vendor code duplication
			var existing_vendor_codes = Toolbox.doSQL_int(conn, @"SELECT COUNT(a.id) FROM inventory_price a LEFT JOIN inventory_item_master b ON a.master_id = b.master_id WHERE a.vendor_code = @v0  AND a.vendor_id = @v1  AND a.master_id != @v2  AND b.active = true", new object[] { vendor_code, vendor_id, part_no });
			if (existing_vendor_codes > 0)
			{
				var existing_parts = Toolbox.doSQL_string(conn, @"SELECT CAST(GROUP_CONCAT(DISTINCT(a.master_id) ORDER BY a.master_id) AS CHAR) _parts FROM inventory_price a LEFT JOIN inventory_item_master b ON a.master_id = b.master_id WHERE a.vendor_code = @v0  AND a.vendor_id = @v1  AND a.id != @v2 ", new object[] { vendor_code, vendor_id, part_no, WarehouseBusinessUnit.id });
				sb_error.AppendFormat(@"\n\nRec #{0} - Part #{1} has a vendor code <b>{2}</b> that exists on other part number(s). Please fix.", rec_no, part_no, vendor_code);
				should_process = false;
			}
		}
		if (!should_process)
		{
			ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "alert", "alert(\"" + sb_error + "\");", true);
			return;
		}
		double qty_rec = 0;
		double qty_previous_rec = 0;
		double totqty = 0;
		var commit_wo = (int)rb_receiveallcommit.Value == 1;
		var poprogress = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
		/**
             * Ricky Patel 01/06/2017
             * Warehouse business unit ID
             */
		WorkingBusinessUnit = new NeBusinessUnit(hidCompanyID.Value);
		WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
		var company = new NeBusinessUnit(Convert.ToInt32(WarehouseBusinessUnit.id));
		var details = new NEPO_Details_Current();

	try
		{
			foreach (DataRow poline in po_dt.Rows)
			{
				qty_rec = 0;
				try
				{
					qty_rec = Convert.ToDouble(poline["po_details_qty_ordered"].ToString());
				}
				catch (Exception ee)
				{
					Toolbox.do_errorLog_errorStack(ee);
				}
				qty_previous_rec = 0;
				try
				{
					qty_previous_rec = Convert.ToDouble(poline["po_details_qty_received"].ToString());
				}
				catch (Exception ee)
				{
					Toolbox.do_errorLog_errorStack(ee);
				}
				// grab the details for the line
				details.po_details_current_line(Convert.ToInt32(poline["po_details_id"].ToString()));
				var is_exclude = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(b.is_exclude), 1) FROM inventory_item_master a LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id WHERE a.master_id = @v0 ", new[] { poline["po_details_part_no"] }) == 1;

                // Reload it in each looping.
			    commit_wo = (int)rb_receiveallcommit.Value == 1;

			    try
			    {
			        var dt = Toolbox.doSQL_dt(
                        conn,
			            @"SELECT a.tag_id, b.is_rental FROM inventory_item_master a LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id WHERE a.master_id = @v0",
			            new object[] { details.po_details_part_no}
			            );
			        if (dt != null && dt.Rows.Count == 1)
			        {
			            var is_rental = Convert.ToBoolean(dt.Rows[0]["is_rental"]);
			            if (is_rental)
			            {
			                // Rental items must be committed to wo.
			                commit_wo = true;  // commit to wo
			                is_exclude = true; // not touch stock
			            }
			        }
			    }
			    catch (Exception ex)
			    {
                }

			    if (details.po_details_line_active == 1)
				{
					totqty = qty_rec;
					details.po_details_id = Convert.ToInt32(poline["po_details_id"].ToString());
					//get the status of the work order FIRST!
					#region if this is a real work order, not a shop work order & also isn't a gl account
					if (!new List<int>(new[] { 9999999, 9999998, 9999997 }).Contains(details.po_details_woprog_id) && !details.is_gl_account)
					{
						var statusofwo = Toolbox.doSQL_string(conn, @"SELECT WOProg_Status FROM woprog  WHERE woprog_id =@v0", new object[] { details.po_details_woprog_id });
						var wobvwoofwo = Toolbox.doSQL_string(conn, @"SELECT WOProg_BVWO FROM woprog  WHERE woprog_id =@v0", new object[] { details.po_details_woprog_id });
						if (new List<string>(new[]{  "Invoiced",
															OpsWOStatus.WaitingToBeInvoiced,
															"Waiting For PO",
															"Waiting BM Approval",
															"Waiting PM Approval",
															"Waiting Approval"}).Contains(statusofwo))
						{
							throw new Exception("You Cannot Receive parts for a Work Order: " + wobvwoofwo + " because it has a of Status: " + statusofwo);
						}
					}
					#endregion if this is a real work order, not a shop work order & also isn't a gl account
					if (details.po_details_part_no == 0)
					{
						details.po_details_line_active = 0;
						details.PO_Details_Update2();
					}
					else
					{
						details.po_details_line_active = 0;
						details.po_details_qty_received = qty_rec;
						try
						{
							if (qty_rec <= details.po_details_qty_orderd)
							{
								details.PO_Details_Update2();
							}
						}
						catch
						{
							throw new Exception("There was an issue updating the PO line " + details.po_details_rec_no + " in MYSQ:");
						}
						// try moving parts to the work order in mysql
						try
						{
							#region if is a shop/reg workorder
							var line_is_stock = false;
							if (new List<int>(new[] { 9999999, 9999998, 9999997 }).Contains(details.po_details_woprog_id) && details.po_details_part_no != 0 && !is_exclude)
							//if (details.po_details_part_no != 0)
							{
								line_is_stock = true;
							}
							#endregion if is a shop/reg workorder
							#region if is as normal workorder
							else if ((commit_wo || is_exclude) && details.woprog_id > 200000 && !details.is_gl_account && !new List<int>(new[] { 9999999, 9999998, 9999997 }).Contains(details.po_details_woprog_id))
							{
								details.MoveToWOProgDetails(poprogress.poprog_bvpo,
																details.po_details_poprog_id.ToString(),
																details.po_details_id.ToString(),
																currentUser.id.ToString(),
																qty_rec - qty_previous_rec,
																commit_wo || is_exclude
																);
							}
							#endregion if is as normal workorder
							// Making it so the receiving of po items ALWAYS updates inventory. (Except inventory excludes)
							var updateqtyordered = Convert.ToDouble(poline["po_details_qty_ordered"].ToString());
							var updateqtyrecieved = Convert.ToDouble(poline["po_details_qty_received"].ToString());
							var updateqtyforstock = updateqtyordered - updateqtyrecieved;
							if ((!commit_wo || line_is_stock) && !details.is_gl_account && !is_exclude)
							{
								#region Update Stock
								var fromstock = new Nestock_transfer();
								fromstock.type_id = 2; // Purchase Order to Inventory
								fromstock.master_id = details.po_details_part_no;
								if (updateqtyforstock > 0)
								{
									fromstock.from_id = Convert.ToInt32(hidPOProgID.Value);
								}
								else
								{
									fromstock.to_id = Convert.ToInt32(hidPOProgID.Value);
								}
								// Check if this part has a valid location.. if not add a link.
								var c_loc = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM inventory_location WHERE master_id = @v0  AND business_unit_id = @v1 ", new object[] { details.po_details_part_no, company.id });
								if (c_loc == 0) // Doesn't exist... make one.
								{
									var loc_master_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(id),0) FROM inventory_location_master WHERE business_unit_id = @v0  AND name = '0.0.0'", new object[] { company.id });
									if (loc_master_id == 0) // Doesn't have a loc_master of 0.0.0??? Make it.
									{
										var ilm = new location_master
										{
											business_unit_id = company.id,
											type_id = 1,
											name = "0.0.0"
										};
										ilm.save();
										loc_master_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(id),0) FROM inventory_location_master WHERE business_unit_id = @v0  AND name = '0.0.0'", new object[] { company.id });
										if (loc_master_id == 0)
										{
											throw new Exception("There was an issue creating the default 0.0.0 location for your branch, aborting.");
										}
									}

									var il = new location
									{
										master_id = Convert.ToInt32(details.part_no),
										location_master_id = loc_master_id,
										business_unit_id = company.id,
										min = 0,
										max = 0,
										qty = 0
									};
									il.save();
									fromstock.to_location_id = Convert.ToInt32(il.id);
								}
								fromstock.quantity = updateqtyforstock * details.po_details_vendor_qty_per;
								fromstock.description = details.po_details_description;
								fromstock.member_id = currentUser.id;
								fromstock.note = "PO Quantity Updated";
								if (poprogress.location_master_id != 0)
								{
									fromstock.to_location_id = poprogress.location_master_id;
								}
								fromstock.business_unit_id = Convert.ToInt32(WarehouseBusinessUnit.id);
								fromstock.cost = details.po_details_cost / details.po_details_vendor_qty_per;
								fromstock.po_line_id = details.po_details_id;
								try
								{
									fromstock.Nestock_transfer_save();
								}
								catch (Exception eei)
								{
									details.po_details_line_active = 1;
									details.po_details_qty_received = details.po_details_qty_received - qty_rec;
									details.PO_Details_Update2();
									Toolbox.do_errorLog_errorStack(eei);
									throw eei;
								}
								#endregion Update Stock
							}
						}
						catch (Exception ex2)
						{
							details.po_details_line_active = 1;
							details.po_details_qty_received = qty_previous_rec;
							details.PO_Details_Update2();
							throw new Exception(string.Format("There was an issue trying to update rec {0} the work order: {1}", details.po_details_rec_no, ex2));
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			errorlabel.Text = ex.Message;
		}
		update_po_totals(true);
		update_top_right_box(poprogress.poprog_id.ToString());
		NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, $"Purchase Order {hidPOProgID.Value}  had receive-all-items applied to it");
	}
	private void update_po_totals(bool updateReceivedDate = false)
	{
		//check_for_changes();
		try
		{
			var po_total = Toolbox.doSQL_double(conn, @"Select ifnull((SELECT SUM(po_details_qty_ordered * po_details_cost) FROM po_details_current  WHERE po_details_poprog_id =@v0),0) ", new object[] { hidPOProgID.Value });
			po_total = Math.Round(po_total, 2);
			var p_orec_total = Toolbox.doSQL_double(conn, @"Select ifnull((SELECT SUM(IFNULL(po_details_qty_received,0) * IFNULL(po_details_cost,0)) FROM po_details_current  WHERE po_details_poprog_id =@v0),0) ", new object[] { hidPOProgID.Value });
			p_orec_total = Math.Round(p_orec_total, 2);

			if (updateReceivedDate)
			{
				Toolbox.doSQL_void(conn,
					@"UPDATE poprog_header SET poprog_total_cost = @v0 , poprog_total_recCost = @v1 , poprog_received_date = NOW(), poprog_ts=poprog_ts WHERE poprog_id = @v2  LIMIT 1",
					new object[] { po_total, p_orec_total, hidPOProgID.Value });
			}
			else
			{
				Toolbox.doSQL_void(conn,
					@"UPDATE poprog_header SET poprog_total_cost = @v0 , poprog_total_recCost = @v1 , poprog_ts=poprog_ts WHERE poprog_id = @v2  LIMIT 1",
					new object[] { po_total, p_orec_total, hidPOProgID.Value });
			}

			//			NePOProg poprogress2 = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
			//			hid_ts.Value = poprogress2.poprog_ts.Ticks.ToString();
		}
		catch
		{
			errorlabel.Text = "Error updating PO Header Totals";
		}
	}

	private void set_work_order_details(string _woprogid)
	{
		hidWOProgID.Value = _woprogid;
		var sql = @"SELECT WOProg_ID, 
            WOProg_BVWO, WOPRog_CustomerName WOProg_CustomerName, WOProg_Description, 
            name, Member_FirstName, Member_LastName
            FROM woprog, business_unit, member 
            WHERE woprog.business_unit_id = member.business_unit_id
            AND WOProg_ID = " + _woprogid + @" 
            AND member.Member_ID = WOProg_PM_MemberID";
		var wot_ab = Toolbox.doSQL_dt(conn, @"SELECT WOProg_ID, WOProg_BVWO, WOPRog_CustomerName WOProg_CustomerName, WOProg_Description, name, Member_FirstName, Member_LastName FROM woprog, business_unit, member  WHERE woprog.business_unit_id = member.business_unit_id AND WOProg_ID =@v0 AND member.Member_ID = WOProg_PM_MemberID", new object[] { _woprogid });
		foreach (DataRow row in wot_ab.Rows)
		{
			lblWOInfo.Text = row[1] + " - " + row[2] + "\n";
			lblWOInfo.Text += row[3] + "\n";
			lblWOInfo.Text += "Company: " + row[4] + "\n";
			lblWOInfo.Text += "Project Manager: " + row[5] + " " + row[6] + "\n";
		}
	}
	protected void ButtonStartPO_Click(object _sender, EventArgs _e)
	{
		var po_order_number = "";
		var errorMessage = new StringBuilder();
		if (hidCompanyID.Value == "0")
			{
			errorMessage.Append("<li>Please select a branch");
			}
		if (hidVendorID.Value == "0" || hidVendorID.Value == "")
			{
			errorMessage.Append("<li>Please select a vendor");
			}
		if(string.IsNullOrWhiteSpace(txtDescription.Text))
			{
			errorMessage.Append("<li>Please enter a description");
			}
		if (string.IsNullOrWhiteSpace(txtShippingAddress1.Text))
			{
			errorMessage.Append("<li>Please enter a value for Address line 1");
			}
		if (string.IsNullOrWhiteSpace(txtShippingCity.Text))
			{
			errorMessage.Append("<li>Please enter a city");
			}
		if (string.IsNullOrWhiteSpace(txtShippingProv.Text))
			{
			errorMessage.Append("<li>Please enter a province/state");
			}
		if (string.IsNullOrWhiteSpace(txtShippingCountry.Text))
			{
			errorMessage.Append("<li>Please enter a country");
			}
		if (string.IsNullOrWhiteSpace(txtShippingPostCode.Text))
			{
			errorMessage.Append("<li>Please enter a postal/zip code");
			}
		if (string.IsNullOrWhiteSpace(combo_expectedorderdate.Text))
			{
			errorMessage.Append("<li>Please enter an expected order date");
			}
		if (string.IsNullOrWhiteSpace(combo_required_date.Text))
			{
			errorMessage.Append("<li>Please enter an expected required date");
			}
		if (string.IsNullOrWhiteSpace(combo_expected_receive_date.Text))
			{
			errorMessage.Append("<li>Please enter an expected receive date");
			}
		if (Convert.ToInt32(ASPxComboBoxShipVia.Value.ToString()) == 0)
			{
			errorMessage.Append("<li>Please enter a shipping method");
			}
		if (Convert.ToInt32(ASPxComboBoxPayment.Value.ToString()) == 0)
			{
			errorMessage.Append("<li>Please select a vendor payment term.");
			}
		if (errorMessage.Length > 0)
			{
			errorlabel.Visible = true;
			errorlabel.Text    = "<ul>"+errorMessage+"</ul>";
			return;
			}

		var company = new NeBusinessUnit(Convert.ToInt32(hidCompanyID.Value));
		var woprog = new NeWOProg();
		var dsn = company.DSN;

		var poid = "";
		try
			{
			var poprog = new NePOProg
				{
				business_unit_id = Convert.ToInt32(hidCompanyID.Value),
				poprog_customer_id = 0,
				poprog_cutby_member_id = currentUser.id,
				poprog_cutdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
				poprog_date_required = combo_required_date.Text != ""
								? Convert.ToDateTime(combo_required_date.Text).ToString("yyyy-MM-dd HH:mm:ss")
								: "",
				poprog_expected_order_date = combo_expectedorderdate.Text != ""
								? Convert.ToDateTime(combo_expectedorderdate.Text).ToString("yyyy-MM-dd HH:mm:ss")
								: "",
				poprog_expected_received_date = combo_expected_receive_date.Text != ""
								? Convert.ToDateTime(combo_expected_receive_date.Text).ToString("yyyy-MM-dd HH:mm:ss")
								: "",
				poprog_last_modified = DateTime.Now,
				poprog_ts = DateTime.Now,
				poprog_invoice_number = "",
				poprog_invoice_slip_scan_date = "",
				poprog_order_description = txtDescription.Text.Replace("'", ""),
				poprog_order_placed_date = "",
				poprog_packing_slip_scan_date = "",
				poprog_received_date = "",
				poprog_check_sent_date = "",
				nesi_cut_po = chk_nesi_po.Checked,
				location_master_id = combo_location.Value != null
								? Convert.ToInt32(combo_location.Value)
								: 0,
				poprog_shipping_method = Convert.ToInt32(ASPxComboBoxShipVia.Value.ToString()),
				poprog_poprog_payment_method_id = Convert.ToInt32(ASPxComboBoxPayment.Value.ToString()),
				poprog_shipping_address_id = 0
				};
			poprog.poprog_contact_id = Convert.ToInt32(ddlContacts.Value);
			poprog.poprog_status = OpsPOStatus.NotIssued;
			poprog.poprog_total_cost = 0;
			poprog.cc_lastfour_id = combo_last4.Value == null ? 0 : (int)combo_last4.Value;
			poprog.poprog_total_recCost = 0;
			poprog.poprog_vendor_id = Convert.ToInt32(ddl_vendor.Value.ToString());
			poprog.poprog_woprog_id = woprog.woprog_id;
			hidManualAddress.Value = txtAddressBox.Text;
			var delimiters = new[] { '\n' };
			var addresshold = txtAddressBox.Text;
			var address = addresshold.Split(delimiters);
			var builtaddress = "";
			foreach (var addressline in address)
				{
				builtaddress += addressline + "\n";
				}
			DDLCurrency.DataBind();
			poprog.poprog_country_code_currency = DDLCurrency.Value == null ? company.default_currency.ToString() : DDLCurrency.Value.ToString();
			poprog.poprog_ack_req = chkAckOfPORec.Checked ? 1 : 0;
			poprog.poprog_ack_req_rec = chkPORec.Checked ? 1 : 0;
			poprog.poprog_ship_note_req = chkShipNoticeReq.Checked ? 1 : 0;
			poprog.poprog_ship_note_req_rec = chkShipNotice.Checked ? 1 : 0;
			poprog.edit_after_issue = chkAllowEdit.Checked ? 1 : 0;
			poprog.Shipping_Addr1 = txtShippingAddress1.Text;
			poprog.Shipping_Addr2 = txtShippingAddress2.Text;
			poprog.Shipping_City = txtShippingCity.Text;
			poprog.Shipping_Country = txtShippingCountry.Text.Trim().ToLower() == "canada" ? "CDN" : "USA";
			poprog.Shipping_Postal = txtShippingPostCode.Text;
			poprog.Shipping_Province = txtShippingProv.Text;
			poprog.POProg_Save();
			poid = poprog.poprog_id.ToString();
			poprog.poprog_bvpo = poprog.poprog_id.ToString().PadLeft(10, '0');
			poprog.poprog_update();
			}
		catch (Exception ex2)
			{
			var message = "Error Inserting into the MYSql Table: " + ex2;
			throw new Exception(message);
			//return;
			}
		hidPOProgID.Value = poid;
		Response.Redirect("po_prog_add.aspx?poprogid=" + poid + "&action=show");
	}
	protected void ImageButtonSaveVendor_Click1(object _sender, EventArgs _e)
	{
		// Verify they can update the vendor.
		var po = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
		var bu = new NeBusinessUnit(po.business_unit_id);

		if (po.location_master_id > 0)
		{
			errorlabel.Text = "Lines are already allocated to this PO, you must first remove the lines before changing the vendor.";
			errorlabel.Visible = true;
			return;
		}

		update_po();
	}
	protected void UpdateButton_Click(object _sender, ImageClickEventArgs _e)
	{
		if (string.IsNullOrWhiteSpace(txtShippingAddress1.Text))
		{
			errorlabel.Visible = true;
			errorlabel.Text = "Address line 1 is required.";
			return;
		}
		if (string.IsNullOrWhiteSpace(txtShippingCity.Text))
		{
			errorlabel.Visible = true;
			errorlabel.Text = "City is required.";
			return;
		}
		if (string.IsNullOrWhiteSpace(txtShippingProv.Text))
		{
			errorlabel.Visible = true;
			errorlabel.Text = "Province/State is required.";
			return;
		}
		if (string.IsNullOrWhiteSpace(txtShippingCountry.Text))
		{
			errorlabel.Visible = true;
			errorlabel.Text = "Country is required.";
			return;
		}
		if (string.IsNullOrWhiteSpace(txtShippingPostCode.Text))
		{
			errorlabel.Visible = true;
			errorlabel.Text = "Postal/Zip Code is required.";
			return;
		}
		if (combo_expectedorderdate.Text == "")
		{
			errorlabel.Visible = true;
			errorlabel.Text = "Please make sure you have selected an expected order date.";
			return;
		}
		if (combo_required_date.Text == "")
		{
			errorlabel.Visible = true;
			errorlabel.Text = "Please make sure you have selected an expected required date.";
			return;
		}
		if (combo_expected_receive_date.Text == "")
		{
			errorlabel.Visible = true;
			errorlabel.Text = "Please make sure you have selected an expected receive date.";
			return;
		}

		update_po();
	}
	private void update_po()
	{
		var woprog = new NeWOProg();
		var poprog = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
		if (poprog.poprog_woprog_id > 0)
		{
			woprog.Load(poprog.poprog_woprog_id);
		}
		//if (poprog.poprog_ts.Ticks.ToString() != hid_ts.Value)
		//{
		//	ScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "confirm('Someone else has changed parts of the PO while you were editing. This page will auto refresh...'); location.href= location.href;", true);
		//	//			errorlabel.Text		= "Please refresh this PO before saving, the data has changed since you last loaded it.";
		//	//			errorlabel.Visible	= true;
		//	return;
		//}
		var oldvendorid = poprog.poprog_vendor_id;
		var poBusinessUnit = new NeBusinessUnit(poprog.business_unit_id);
		var dsnnew = poBusinessUnit.DSN;
		if (poprog.poprog_order_description != txtDescription.Text.Replace("'", "") ||
			poprog.poprog_date_required != Toolbox.MySQL_shortdt(combo_required_date.Date) ||
			poprog.poprog_expected_order_date != Toolbox.MySQL_shortdt(combo_expectedorderdate.Date) ||
			poprog.cc_lastfour_id != (combo_last4.Value == null ? 0 : (int)combo_last4.Value) ||
			poprog.location_master_id != (combo_location.Value != null ? Convert.ToInt32(combo_location.Value) : 0) ||
			poprog.poprog_shipping_method != Convert.ToInt32(ASPxComboBoxShipVia.Value.ToString()) ||
			poprog.poprog_poprog_payment_method_id != Convert.ToInt32(ASPxComboBoxPayment.Value.ToString()) ||
			poprog.poprog_country_code_currency != DDLCurrency.Value.ToString() ||
			poprog.poprog_contact_id != (int)ddlContacts.Value ||
			poprog.nesi_cut_po != (nesi_cut_po.Visible ? chk_nesi_po.Checked : poprog.nesi_cut_po) ||
			poprog.poprog_vendor_id != Convert.ToInt32(ddl_vendor.Value.ToString()) ||
			hidManualAddress.Value != txtAddressBox.Text
			)
		{
			poprog.poprog_last_modified = DateTime.Now;
			poprog.poprog_ts = DateTime.Now;
		}
		poprog.poprog_cutby_member_id = currentUser.id;
		poprog.poprog_cutdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
		poprog.poprog_date_required = Toolbox.MySQL_shortdt(combo_required_date.Date);
		poprog.poprog_expected_order_date = Toolbox.MySQL_shortdt(combo_expectedorderdate.Date);
		poprog.poprog_expected_received_date = Toolbox.MySQL_shortdt(combo_expected_receive_date.Date);
		poprog.poprog_invoice_number = "";
		poprog.poprog_invoice_slip_scan_date = "";
		poprog.poprog_order_description = txtDescription.Text.Replace("'", "");
		poprog.poprog_order_placed_date = "";
		poprog.poprog_packing_slip_scan_date = "";
		poprog.poprog_received_date = "";
		poprog.cc_lastfour_id = combo_last4.Value == null ? 0 : (int)combo_last4.Value;
		poprog.nesi_cut_po = nesi_cut_po.Visible ? chk_nesi_po.Checked : poprog.nesi_cut_po;
		poprog.poprog_check_sent_date = "";
		poprog.location_master_id = combo_location.Value != null ? Convert.ToInt32(combo_location.Value) : 0;
		poprog.poprog_shipping_method = Convert.ToInt32(ASPxComboBoxShipVia.Value.ToString());
		poprog.poprog_poprog_payment_method_id = Convert.ToInt32(ASPxComboBoxPayment.Value.ToString());
		poprog.poprog_shipping_address_id = 0;
		poprog.poprog_country_code_currency = DDLCurrency.Value.ToString();
		poprog.poprog_contact_id = (int)ddlContacts.Value;
		poprog.poprog_status = OpsPOStatus.NotIssued;
		poprog.poprog_total_cost = 0;
		poprog.poprog_total_recCost = 0;
		poprog.poprog_vendor_id = Convert.ToInt32(ddl_vendor.Value.ToString());
		//poprog.poprog_manual_shipaddress = txtAddressBox.Text.Replace("'", "");
		poprog.notes_memberid = currentUser.id;
		poprog.poprog_ack_req = Convert.ToInt32(chkAckOfPORec.Checked);
		poprog.poprog_ack_req_rec = Convert.ToInt32(chkPORec.Checked);
		poprog.poprog_ship_note_req = Convert.ToInt32(chkShipNoticeReq.Checked);
		poprog.poprog_ship_note_req_rec = Convert.ToInt32(chkShipNotice.Checked);
		poprog.edit_after_issue = chkAllowEdit.Checked ? 1 : 0;
		hidManualAddress.Value = txtAddressBox.Text;
		poprog.Shipping_Addr1 = txtShippingAddress1.Text;
		poprog.Shipping_Addr2 = txtShippingAddress2.Text;
		poprog.Shipping_City = txtShippingCity.Text;
		poprog.Shipping_Country = txtShippingCountry.Text.Trim().ToLower() == "canada" ? "CDN" : "USA";
		poprog.Shipping_Postal = txtShippingPostCode.Text;
		poprog.Shipping_Province = txtShippingProv.Text;

		poprog.poprog_update();
		poprog = new NePOProg(poprog.poprog_id);
		//hid_ts.Value = poprog.poprog_ts.Ticks.ToString();
		if (oldvendorid != poprog.poprog_vendor_id)
		{
			//The Vendor has Been Changed.
			//Check all items on this PO
			//And update them if they exist for the new vendor
			//other wise delet delet them.
			var po_dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM po_details_current  WHERE po_details_qty_ordered > po_details_qty_received AND po_details_part_no != 0 AND po_details_poprog_id =@v0", new object[] { hidPOProgID.Value });
			var details = new NEPO_Details_Current();
			var part_check_sql = "";
			var delstr = "";
			var vendorinfotable = new DataTable();
			try
			{
				foreach (DataRow poline in po_dt.Rows)
				{
					details.po_details_current_line(Convert.ToInt32(poline["po_details_id"]));
					vendorinfotable = Toolbox.doSQL_dt(conn, @"SELECT * FROM inventory_price  WHERE master_id =@v0 AND business_unit_id =@v1  AND vendor_id =@v2  AND vendor_code =@v3 ",
						new object[] { details.po_details_part_no, poprog.business_unit_id, poprog.poprog_vendor_id, details.po_details_vendor_part_no });
					foreach (DataRow vendorinforow in vendorinfotable.Rows)
					{
						details.po_details_vendor_part_no = vendorinforow["vendor_code"].ToString();
						details.po_details_vendor_qty_per = Convert.ToDouble(vendorinforow["qty"].ToString());
						details.po_details_cost = Convert.ToDouble(vendorinforow["cost"].ToString());
					}
					if (vendorinfotable.Rows.Count > 0)
					{
						details.po_details_id = Convert.ToInt32(poline["po_details_id"].ToString());
						details.PO_Details_Update2();
					}
					else
					{
						Toolbox.doSQL_void(conn, @"DELETE FROM po_details_current  WHERE po_details_id =@v0 limit 1 ", new object[] { poline["po_details_id"].ToString() });
					}
				}
			}
			catch (Exception ex)
			{
				errorlabel.Text = ex.ToString();
			}
			Toolbox.doSQL_void(conn, @" UPDATE poprog_header SET poprog_total_cost = (SELECT IFNULL(SUM(po_details_qty_ordered * po_details_cost),0) FROM po_details_current WHERE po_details_poprog_id = @v0 ) WHERE poprog_id = @v0  LIMIT 1", new object[] { hidPOProgID.Value });
		
			NePOProg.AddNote(Convert.ToInt32(hidPOProgID.Value), currentUser.id, "Purchase Order had its Vendor Changed");
		}
		display_po_information(hidPOProgID.Value);
	}
	private void get_open_work_orders(string _companyid)
	{
		if (!string.IsNullOrEmpty(_companyid))
		{
			ASPxcbwo.DataSource = Toolbox.doSQL_dt(conn, @" SELECT woprog_id, CONCAT(woprog_bvwo,' - ', woprog_customername, ' ', LEFT(woprog_description,40), ' - ', name) workorder FROM woprog inner join business_unit on woprog.business_unit_id = business_unit.id inner join customer on woprog_customer_id = customer.customer_id WHERE woprog.business_unit_id = @v0  AND (woprog_status = 'Open' or woprog_status = 'Initial Prep' or woprog_status = 'Just Scanned' or woprog_status = 'Questions for PM' or woprog_status='Open Vendor POs') and customer_qc_member_id IS NOT NULL and woprog_bvwo != 'Not Entered' ORDER BY woprog.business_unit_id, woprog_customername, woprog_bvwo", new object[] { _companyid });
			ASPxcbwo.DataBind();
		}
	}
	protected void lbAddScan_Click(object _sender, EventArgs _e)
	{
		//DataTable table = new DataTable();
		// table.Columns.Add("filename", typeof(string));
		//table.Columns.Add("scantime", typeof(string));
		lsbScannedItems.Items.Clear();
		ASPxpcScanList.ShowOnPageLoad = true;
		DirectoryInfo dir_just = null;
		var popath = Toolbox.doSQL_string(@"SELECT POPath from business_unit  WHERE id =@v0", new object[] { hidCompanyID.Value });
		//dirJust = new DirectoryInfo("f:\\ProjectFiles\\REGIS\\sections\\purchaseorder\\scanned_items\\");
		dir_just = new DirectoryInfo(popath);
		var arr_files = dir_just.GetFiles();
		Array.Sort(arr_files, delegate (FileInfo _f1, FileInfo _f2)
		{
			if (_f1 == null)
			{
				if (_f2 == null)
				{
					return 0;
				}

				return -1;
			}
			else
			{
				return _f2 == null
					? 1
					: _f2.CreationTime.CompareTo(_f1.CreationTime);
			}
		});
		for (var i = arr_files.Length - 1; i >= 0; i--)
		{
			var filename = Server.UrlEncode(arr_files[i].Name);
			var str_created = arr_files[i].CreationTime.ToString("MM-dd HH:mm:ss");
			str_created = filename + " - " + str_created;
			var li = new ListItem(str_created, filename);
			li.Attributes.Add("title", filename + " " + str_created);
			lsbScannedItems.Items.Add(li);
			//table.Rows.Add(filename, strCreated);
		}
		//ASPxGridView1.DataSource = table;
		//ASPxGridView1.DataBind();
	}
	protected void lsbScannedItems_SelectedIndexChanged(object _sender, EventArgs _e)
	{
		var httpFile = lsbScannedItems.SelectedValue;
		var filename = Server.UrlDecode(lsbScannedItems.SelectedValue);
		hidFileName.Value = filename;
		var popath = Toolbox.doSQL_string(@"SELECT POPath from business_unit  WHERE ID =@v0", new object[] { hidCompanyID.Value });
		// File.Move(popath + filename, popath + newfilename);
		// File.Copy(popath + "\\" + filename, @"C:\Inetpub\neintranet\sections\purchaseorder\scanned_items\" + filename, true);
		// PO business Unit ID
		var fileServer = NeTaxEntity.BaseFolder(hidCompanyID.Value, false);
		var fileServer_ext = NeTaxEntity.BaseFolder(hidCompanyID.Value, true);
		File.Copy(popath + "\\" + filename, fileServer + @"\POs\scanned_items\" + filename, true);
		// File.Copy(popath + "\\" + filename, @"f:\ProjectFiles\REGIS\sections\purchaseorder\scanned_items\" + filename, true);
		var pdfscan = "<embed src=\"" + fileServer_ext + @"\\POs\\scanned_items\\" + httpFile + "\" width=\"100%\" height=\"700\">\n";
		lblScanShow.Text = pdfscan;
		ASPxpcScanDisplay.ShowOnPageLoad = true;
	}
	protected void btnUseScan_Click(object _sender, EventArgs _e)
	{
		//check_for_changes();
		try
		{
			NePOProg.use_scan(Convert.ToInt32(hidPOProgID.Value), hidCompanyID.Value, hidFileName.Value, Convert.ToInt32(rbl_slip_type.SelectedValue), currentUser, mem_notes.Text, "");
			ASPxpcScanDisplay.ShowOnPageLoad = false;
			ASPxpcScanList.ShowOnPageLoad = false;
		}
		catch (Exception scan_move_ex)
		{
			throw new Exception("There Was an Error Attempting to Rename This Scan: " + scan_move_ex);
		}
		finally
		{
			var script = "<script type=\"text/javascript\"> framefresh(); </script>";
			ScriptManager.RegisterStartupScript(this, GetType(), "refresh", script, false);
		}
	}
	private void get_totals()
	{
		double display = 0;
		var sel = "SELECT IFNULL(SUM(po_details_qty_ordered * po_details_cost),0) AS ordersum FROM po_details_current WHERE po_details_poprog_id = " + hidPOProgID.Value;
		try
		{
			display = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_ordered * po_details_cost),0) AS ordersum FROM po_details_current  WHERE po_details_poprog_id =@v0", new object[] { hidPOProgID.Value });
		}
		catch (Exception exce) { Toolbox.do_errorLog_errorStack(exce); }
		//info1disp.Text = display.ToString("C2");
		display = 0;
		sel = "SELECT IFNULL(SUM(po_details_qty_received * po_details_cost),0) AS ordersum FROM po_details_current WHERE po_details_poprog_id = " + hidPOProgID.Value;
		try
		{
			display = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_received * po_details_cost),0) AS ordersum FROM po_details_current  WHERE po_details_poprog_id =@v0", new object[] { hidPOProgID.Value });
		}
		catch (Exception exce) { Toolbox.do_errorLog_errorStack(exce); }
		//info2disp.Text = display.ToString("C2");
	}
	private static class response_helper
	{
		public static void redirect(string _url, string _target, string _window_features)
		{
			var context = HttpContext.Current;
			if (_target == "_top")
			{
				_target = "_self";
			}
			if ((string.IsNullOrEmpty(_target) ||
				 _target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
				string.IsNullOrEmpty(_window_features))
			{
				context.Response.Redirect(_url);
			}
			else
			{
				var page = (Page)context.Handler;
				if (page == null)
				{
					throw new InvalidOperationException(
						"Cannot redirect to new window outside Page context.");
				}
				_url = page.ResolveClientUrl(_url);
				string script;
				script = !string.IsNullOrEmpty(_window_features)
					? @"window.open(""{0}"", ""{1}"", ""{2}"");"
					: @"window.open(""{0}"", ""{1}"");";
				script = string.Format(script, _url, _target, _window_features);
				ScriptManager.RegisterStartupScript(page,
													typeof(Page),
													"Redirect",
													script,
													true);
			}
		}
	}
	[WebMethod]
	public static string IsPartID_TextChanged(string _part)
	{
		var success = "true";
		return success;
	}
	//protected void b_bv_sync_Click(object _sender, EventArgs _e)
	//	{
	//	var podc = new NEPO_Details_Current();
	//	podc.MoveToBVPORecord(hidPOProgID.Value, _my_member.id.ToString());
	//	ScriptManager.RegisterStartupScript(this, GetType(), "refresh", "location.href = location.href;", true);
	//	}
	protected void ScriptManager1_AsyncPostBackError(object _sender, AsyncPostBackErrorEventArgs _e)
	{
	}
	protected void tb_save_cc_Click(object _sender, EventArgs _e)
	{
		var b = (ASPxButton)_sender;
		Toolbox.doSQL_void(conn, @"INSERT INTO poprog_cc (business_unit_id, type, last_four,name) VALUES (@v0 , @v1 , @v2 ,@v3 )", new[] { ddl_company.Value, combo_cctype.Value, tb_lastfour_cc.Text, tb_ccname.Text });
		tb_lastfour_cc.Text = "";
		tb_ccname.Text = "";
		combo_cctype.SelectedIndex = -1;
		combo_last4.DataBind();
		b.Enabled = true;
		b.ClientEnabled = true;
	}
	protected void ImageButtonAPProblems_Click(object _sender, ImageClickEventArgs _e)
	{
	//	check_for_changes();
		try
		{
			NePOProg.send_to_ap_problems(hidPOProgID.Value, currentUser, "");
			//    move_status(10, hidPOProgID.Value);
		}
		catch (Exception ee)
		{
			errorlabel.Text = ee.Message;
		}
		ScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "alert('An email has been sent to the purchaser.');", true);
		display_po_information(hidPOProgID.Value);
		update_top_right_box(hidPOProgID.Value);
	}
	protected void ImageButtonAPProblemsFixed_Click(object _sender, ImageClickEventArgs _e)
	{
		var po = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
		var previous_status = Toolbox.doSQL_int(conn, @"Select ifnull((SELECT poprogstatus.POProgStatus_Status_ID FROM poprogstatus WHERE poprogstatus_poprog_id = @v0  AND poprogstatus.POProgStatus_Status_ID != 10 ORDER BY poprogstatus_datetime DESC LIMIT 1),4)", new object[] { po.poprog_id });
		var previous_status_st = Toolbox.doSQL_string(conn, @"SELECT status_type FROM poprog_status WHERE poprog_status_id = @v0 ", new object[] { previous_status });
		var previous_member_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(a.poprogstatus_member_id),0) FROM poprogstatus a LEFT JOIN poprog_status b ON a.poprogstatus_status = b.status_type WHERE a.poprogstatus_poprog_id = @v0  AND b.poprog_status_id = 10 ORDER BY poprogstatus_datetime DESC LIMIT 1", new object[] { po.poprog_id });
		var ap_emails = Toolbox.doSQL_string(conn, @"Select ifnull((SELECT
	    group_concat(member.member_neemail SEPARATOR ';')
	    FROM
	        member
	    INNER JOIN membertype ON member.member_membertype_id = membertype.membertype_id AND membertype.membertype_id = 48 and member.member_status = 'Active'
	    INNER JOIN business_unit ON member.business_unit_id = business_unit.ID AND business_unit.is_backoffice = 1),'')", new object[] { });
		try
		{
			move_status(previous_status, hidPOProgID.Value);
		}
		catch (Exception ee)
		{
			errorlabel.Text = ee.Message;
		}
		//em.To="aketelaars@newelectric.com";
		var em = new NeEMail();
		if (previous_member_id > 0)
		{
			em.To = new NeMember((int)previous_member_id).NEEmail;
			if (em.To == "")
			{
				em.To = ap_emails != "" ? ap_emails : "ap@" + Toolbox.app_setting("DomainForEmail");
			}
		}
		else
		{
			em.To = ap_emails != "" ? ap_emails : "ap@" + Toolbox.app_setting("DomainForEmail");
		}
		if (currentUser.business_unit.purchaser != null && currentUser.business_unit.purchaser.id != currentUser.id)
		{
			em.CC = currentUser.business_unit.purchaser.NEEmail;
		}
		em.From = currentUser.NEEmail;
		em.Subject = string.Format("PO {0}'s AP Problems have been fixed", po.business_unit_id.ToString().PadLeft(3, '0') + "-" + po.poprog_bvpo.TrimStart('0'));
		em.Body = string.Format(@"
<div style='font-family: Arial'>
	This is a notification that the AP Problems have been fixed by {4}. <br />
	PO#: <a href='{5}/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}'>{1}</a><br /> 
	Business Unit: {2}<br />
	Vendor: {3}
</div>",
	po.poprog_id,
	po.business_unit_id.ToString().PadLeft(3, '0') + "-" + po.poprog_bvpo,
	new NeBusinessUnit(po.business_unit_id).name,
	new NEVendor(po.poprog_vendor_id).Vendor_Name,
	currentUser.FullName,
	 Toolbox.app_setting("Domain")
	);
		em.isHTML = true;
		em.Send();
		ScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "alert('An email has been sent to Accounts Payable.');", true);
		display_po_information(hidPOProgID.Value);
		update_top_right_box(hidPOProgID.Value);
	}
	protected void cb_ack_Callback(object _source, CallbackEventArgs _e)
	{
		//check_for_changes();
		if (_e.Parameter != "" && hidPOProgID.Value != null && hidPOProgID.Value != "" && hidPOProgID.Value != "0")
		{
			var control = _e.Parameter.Split('|').GetValue(0).ToString();
			var value = _e.Parameter.Split('|').GetValue(1).ToString();
			switch (control)
			{
				case "ack_req":
					Toolbox.doSQL_void(conn, @"UPDATE poprog_header  SET poprog_ack_req =@v0, poprog_last_modified=now(), poprog_ts=now()   WHERE poprog_id =@v1", new object[] { value == "true" ? 1 : 0, hidPOProgID.Value });
					break;
				case "ack_rec":
					var po = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
					var vendor = new NEVendor(Convert.ToInt32(po.poprog_vendor_id));
					Toolbox.doSQL_void(conn, @"UPDATE poprog_header  SET poprog_ack_req_rec =@v0, poprog_last_modified=now(),poprog_ts = now()   WHERE poprog_id =@v1", new object[] { value == "true" ? 1 : 0, hidPOProgID.Value });
					if (value.Equals("true"))
					{
						NePOProg.send_email_to_trackers(hidPOProgID.Value, vendor.Name + " has confirmed the receipt of PO " + po.poprog_bvpo, currentUser, "");
					}
					else
					{
						NePOProg.send_email_to_trackers(hidPOProgID.Value, "Sorry, my mistake.. " + vendor.Name + " has NOT confirmed the receipt of PO " + po.poprog_bvpo, currentUser, "");
					}
					break;
				case "ship_req":
					Toolbox.doSQL_void(conn, @"UPDATE poprog_header  SET poprog_ship_note_req =@v0, poprog_last_modified=now(), poprog_ts = now()   WHERE poprog_id =@v1", new object[] { value == "true" ? 1 : 0, hidPOProgID.Value });
					break;
				case "ship_rec":
					var po2 = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
					var vendor2 = new NEVendor(Convert.ToInt32(po2.poprog_vendor_id));
					Toolbox.doSQL_void(conn, @"UPDATE poprog_header  SET poprog_ship_note_req_rec =@v0, poprog_last_modified=now(), poprog_ts=now()    WHERE poprog_id =@v1", new object[] { value == "true" ? 1 : 0, hidPOProgID.Value });
					if (value.Equals("true"))
					{
						NePOProg.send_email_to_trackers(hidPOProgID.Value, vendor2.Name + " has confirmed the shipping of PO " + po2.poprog_bvpo, currentUser, "");
					}
					else
					{
						NePOProg.send_email_to_trackers(hidPOProgID.Value, "Sorry, my mistake.. " + vendor2.Name + " has NOT confirmed the shipping of PO " + po2.poprog_bvpo, currentUser, "");
					}
					break;
			}
		}
	}
	//private void check_for_changes()
	//{
	//	if (hidPOProgID.Value != "" && hidPOProgID.Value != "0")
	//	{
			//var po = new NePOProg(Convert.ToInt32(hidPOProgID.Value));
			//if (hid_ts.Value != po.poprog_ts.Ticks.ToString() && hid_ts.Value != "" && hid_ts.Value != null && po.poprog_ts != null)
			//{
			//	ScriptManager.RegisterStartupScript(this, GetType(), "Refresh", "confirm('Someone else has changed parts of the PO while you were editing. This page will auto refresh...'); location.href= location.href;", true);
			//	return;
			//}
	//	}
	//}

	protected void ddlPartList_OnSelectedIndexChanged(object _sender, EventArgs _e)
	{
		txtPartHistory.InnerHtml = HistoryDetails(poprogid);

	}
}
