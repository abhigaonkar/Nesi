using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using MySql.Data.MySqlClient;
using NESI.Common.Models;
//using nesi.bv;
using nesi.core;

public partial class sections_workorder_mobile_wo_index : Page
{
	Toolbox _tools;
	NeMember current_user;
	NeWOProg wo;
	NameValueCollection _q;
	public int this_woprog_id
	{
		get { var _this_woprog_id = 0; if (Session["mobile_woprog_id"] == null) { return _this_woprog_id; }
		int.TryParse(Session["mobile_woprog_id"].ToString(), out _this_woprog_id); return _this_woprog_id;
		}
		set { Session["mobile_woprog_id"] = value.ToString(); }
	}

	protected void Page_Init(object sender, EventArgs e)
	{
		current_user = Toolbox.do_handle_authentication(1);
		 _q = Request.QueryString;
		_tools = new Toolbox();
		wo = new NeWOProg();

		if (!IsPostBack)
		{
		hdn_company.Value = current_user.business_unit_id.ToString();
			if ((_q["id"]!=null)&&(_q["id"]=="0"))
			{
				Session["mobile_woprog_id"] = null;
			}
			else
				{
				btn_cancel.Visible = false;
				}
		    BusinessUnitDropDownList.DataSource = Toolbox.doSQL_dt(@"CALL get_visible_business_units(@v0 )", new object[] {  current_user.id } );
			BusinessUnitDropDownList.DataBind();
			BusinessUnitDropDownList.SelectedValue = current_user.business_unit_id.ToString();
			populate_contact();
		}


	}

	protected void Page_Load(object sender, EventArgs e)
	{

		tr_margin.Visible = current_user.AuthenticatedForPrivilege(81);
		tr_total.Visible = current_user.AuthenticatedForPrivilege(81);
		


		if (this_woprog_id==0)
		{
			this_woprog_id = Convert.ToInt32(_q["id"]);
		}
		hidWOProgID.Value = this_woprog_id.ToString();
		if (this_woprog_id != 0)
		{
			wo = new NeWOProg(this_woprog_id);
			
		//	populate_pm();



			var ts_temp = Convert.ToDateTime(Toolbox.doSQL_string(@"SELECT woprog_ts FROM woprog  where woprog_id = @v0", new object[] { hidWOProgID.Value }));
			hid_ts.Value = ts_temp.Ticks.ToString();
		}

		if (!IsPostBack)
		{

			fill_page();

		    if (this_woprog_id != 0)
		    {
		        // DataBindForCustomer(wo.woprog_id.ToString(), this.hdn_company.Value, wo.WOProg_Customer_ID);

		        var revenueId = this.SetDefaultRevenueLine(ddlquote.Value, wo.revenue_line_id);
		        DataBindForRevenueLine(this.hdn_company.Value, revenueId);
            }
		    else
		    {
		        DataBindForCustomer("0", this.hdn_company.Value, 0);

		        var revenueId = this.SetDefaultRevenueLine(ddlquote.Value, 0);
                DataBindForRevenueLine(this.hdn_company.Value, revenueId);
            }

		    this.defaultTM.Value = this.GetConfigSettingByQuery(NESI.BLL.Common.Shared.Configuration.GetRevenueLineTM);
		    this.defaultQuoted.Value = this.GetConfigSettingByQuery(NESI.BLL.Common.Shared.Configuration.GetRevenueLineQuoted);
        }

			ddlpm.SelectedIndex  = ddlpm.SelectedIndex;
		
	}
	public void fill_page()
	{
		#region new work order
		if (this_woprog_id == 0 || this_woprog_id == null)
		{
			tbl_header_info.Visible = false;
			tr_sales_value.Visible = true;
			tr_hours_value.Visible = true;
		}
		#endregion

		if (this_woprog_id != 0 && this_woprog_id != null)
		{
			#region edit work order
			tr_sales_value.Visible = current_user.AuthenticatedForPrivilege(81);
			tr_hours_value.Visible = current_user.AuthenticatedForPrivilege(81);
			wo = new NeWOProg(this_woprog_id);
			hdn_company.Value = wo.business_unit_id.ToString();
			// ddlcustomername.DataBind();
			// ddlcustomername.Value = wo.WOProg_Customer_ID;
		    DataBindForCustomer(wo.woprog_id.ToString(), this.hdn_company.Value, wo.WOProg_Customer_ID);

            expected_endDate.Text = wo.woprog_Expected_EndDate.ToString("yyyy-MM-dd");
			expected_startDate.Text = wo.woprog_Expected_StartDate.ToString("yyyy-MM-dd");
			txt_expect_sales.Text = wo.woprog_expected_sales_value.ToString();
			txt_expected_hrs.Text = wo.woprog_exp_labor.ToString();
			ddllocation.DataBind();
			ddllocation.SelectedValue = wo.woprog_Address_ID.ToString();
		
			
			ddlcontact.DataBind();
			ddlcontact.SelectedValue = wo.woprog_Contact_ID.ToString();
			ddlquote.DataBind();
			ddlquote.Value = wo.QuoteID;
			txt_po.Text = wo.PONumber;
			text_desc.Text = wo.Description;
			tb_inspection.Text = wo.inspection_link;
			chk_inspection.Checked = wo.inspection_required;
		    chk_labor_only.Checked = wo.labor_only;
			ddlpm.SelectedIndex = -1;
			ddlpm.DataBind();
			if (ddlpm.Items.FindByValue(wo.intProjectManager.ToString()) != null)
			{
				ddlpm.SelectedValue = wo.intProjectManager.ToString();
			}
			else
			{
				ddlpm.Items.Add(new ListItem(wo.intProjectManager.ToString(), wo.strProjectManager));
				ddlpm.SelectedValue = wo.intProjectManager.ToString();
			}
			//	expected_endDate.Text = wo.woprog_Expected_EndDate.ToString("")
			var addy = new NEAddress(wo.woprog_Address_ID);
			var service_address = addy.Addr1;
			service_address += addy.Addr2.Trim().Length > 0 ? "<br/>" + addy.Addr2 : "";
			service_address += addy.Addr3.Trim().Length > 0 ? "<br/>" + addy.Addr3 : "";
			service_address += addy.Addr4.Trim().Length > 0 ? "<br/>" + addy.Addr4 : "";
			lbl_address.Text = string.Format(@"<a target='_blank' href='//maps.google.com/?q={0} {1}, {2}, {3}'>{0}<br/>{1},{2},{3}</a>",
							service_address,
							addy.City,
							addy.Prov,
							addy.Postal
							);
			lbl_bvwo.InnerText = wo.OrderNumber;

			lbl_status.InnerText = wo.Status;
			lbl_margin.InnerText = wo.woprog_grossmargin.ToString("P0");
			lbl_margin.Visible = current_user.AuthenticatedForPrivilege(81);
			lbl_total.InnerText = wo.net_total.ToString("C2");
			lbl_total.Visible = current_user.AuthenticatedForPrivilege(81);


			#endregion
		}
        

    }

    protected void ddlcustomername_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlquote.DataBind();
		var li = new ListEditItem("", "0");
		li.Selected = true;
		ddlquote.Items.Add(li);
		ddllocation.DataBind();


	    ddlcontact.DataBind();

		populate_ca();

		//		ddlquote.DataBind();
	}
	protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
	{
		ddlca.DataBind();
	}
	protected void populate_ca()
	{
		ddlca.DataBind();
		var li = new ListItem("", "0");
		li.Selected = true;
		ddlca.Items.Add(li);

	}

	protected void populate_contact()
	{
		ddlcontact.DataBind();
	}
	protected void add_quote_lines_to_wo()
	{

		var quote = new quote(Convert.ToInt32(ddlquote.Value.ToString()));
		var dt = Toolbox.doSQL_dt(@"Select section from quote_section  where quote_id =@v0 and revision =@v1 ", new object[] { quote.QuoteID,quote.Revision });
		foreach (DataRow dr in dt.Rows)
		{
			var section = Toolbox.do_value_from(dr["section"], false).Replace(",", "").Trim();
			if (section != "")
			{
				Toolbox.doSQL_void(@" INSERT INTO wocomment ( comments, workorder_id, personal, printcomments, member_id_audit, wocomment_member_id, business_unit , created_date, modified_date,woprog_id ) VALUES ( @v0 , @v1 , 0, 0, @v2 , @v2 , @v3 , NOW(), NOW(), @v4  )", new object[] {  section, wo, current_user.id, hdn_company.Value, wo.woprog_id  } );
			}
		}
	}
	protected void validate_wo()
	{
		var ttt = 0;
		var dsn = "";
		var PSQLDSN = new NeBusinessUnit();
		var business_unit_id = hdn_company.Value;
		dsn = PSQLDSN.GetBUDSN(Convert.ToInt32(business_unit_id));

		lblError.Text = "";
		try
		{

			// validate quote is not already taken 
			try { ttt = Convert.ToInt32((ddlquote.Value.ToString())); }
			catch { }
			if (ttt > 100000)
			{
				var seed = ddlquote.Value.ToString().Substring(0, 6);
				var Count = Toolbox.doSQL_string(@"SELECT ifnull(MAX(woprog_BVWO),0) FROM WOPROG  where woprog_quoteid like CONCAT(@v0,'%')", new object[] { seed });
				if ((Count != "0") && (Count != Session["mobile_woprog_id"].ToString()))
				{
					lblError.Text = "This Quote or a version of this quote has already been assigned to work order: " + Count;
					return;
				}
			}
			try
			{

				if (ddllocation.SelectedValue == null || ddllocation.SelectedItem.Text == "")
				{
					lblError.Text = "Please select a valid address";

					return;
				}

				if (ddlcustomername.Value == null || (ddlcustomername.SelectedItem.Text == "Select Customer") || (ddlcustomername.Value != null && ddlcustomername.Value.ToString() == "0"))
				{
					lblError.Text = "Please select a customer";

					return;
				}
			var temp_customer = new NECustomer(Convert.ToInt32(ddlcustomername.Value));
			var temp_address = new NEAddress(Convert.ToInt32(ddllocation.SelectedItem.Value));
			if (temp_customer.PORequired.Equals("T"))
				{
				if (txt_po.Text == "")
					{
					lblError.Text = "This customer requires a PO at the time of cutting the work order.";
					return;
					}

				}

			if (temp_customer.IsNEcompany)
				{
				lblError.Text = "You are not allowed to cut intercompany work orders from the mobile.";
				return;
				}
			if (temp_customer.Hold == "T")
				{
				lblError.Text = "You cannot choose this customer because they are on hold";
				return;
				}
			}
			catch
			{
				lblError.Text = "Error trying to save work order, please check all required fields.";
				return;
			}

			try
			{
				if ((ddlpm.SelectedValue == "0" || ddlpm.SelectedItem.Text.Equals("Select PM")))
				{
					lblError.Text = "Please select a project manager";

					return;
				}
			}
			catch
			{
				lblError.Text = "Please select a project manager";

				return;
			}

			try
			{
				if ((BusinessUnitDropDownList.SelectedValue == "0" || BusinessUnitDropDownList.SelectedItem.Text.Equals("unknown")))
				{
					lblError.Text = "Invalid department selected";

					return;
				}

			}
			catch
			{
				lblError.Text = "Invalid department selected";

				return;
			}

			if (ddlcontact.SelectedValue != null)
			{
				if (ddlcontact.SelectedValue == "0")
				{
					lblError.Text = "Please Select a Contact";
					return;
				}
			}
			else
			{
				lblError.Text = "Please Select a Contact";

				return;
			}

		    if (ddlRevenueLines.Value != null)
		    {
		        if (int.Parse(ddlRevenueLines.Value.ToString()) == 0)
		        {
		            lblError.Text = "Please select revenue line";
		            return;
		        }
		    }
		    else
		    {
		        lblError.Text = "Please select revenue line";
		        return;
		    }

            if (text_desc.Text == "")
			{
				lblError.Text = "Work order description is missing";
				return;
			}
		if (text_desc.Text.Length > 999)
			{
			lblError.Text = "Work order description can't be longer than 999 characters.";

			return;
			}
		if ((txt_expect_sales.Text != "0")&&(current_user.AuthenticatedForPrivilege(81)))
			{
				try
				{
					Convert.ToDouble(txt_expect_sales.Text);
				}
				catch
				{
					lblError.Text = "Please enter a proper numeric value in the expected sales value field";

					return;
				}
			}
			double temp_exp_sales1 = 0;
			double.TryParse(txt_expect_sales.Text, out temp_exp_sales1);

			if (expected_startDate.Text == "")
			{
				lblError.Text = "Expected start date not supplied.";

				return;
			}
			if (expected_endDate.Text == "")
			{
				lblError.Text = "Expected end date not supplied.";

				return;
			}
			if ((txt_expected_hrs.Text == "") || (txt_expected_hrs.Text == "0"))
			{

				if ((hdn_company.Value != "11")&&(current_user.AuthenticatedForPrivilege(81)))
				{

					lblError.Text = "Please indicate the expected amount of hours required for this work.";

					return;
				}
			}


		}
		catch (Exception ex)
		{
			lblError.Text += ex.Message;
			_tools.current_user = current_user;
			_tools.page_author = new NeMember(711);
			_tools.catch_error(ex);
			return;
		}
		lblError.Text = "";
	}
	protected void CreateWorkOrder(bool redirect)
	{
		using (var conn = Toolbox.connect())
			{
			var branch = new NeBusinessUnit(hdn_company.Value);
			var strExpectedSalesValue = "";
			var quotedamount = 0.0;
			var currency = branch.country == "USA" ? 1 : 2;
			var hoursspentonqote = 0.0;
			var error_flag = false;
			var error_level = 0;
			var is_downpayment = false;
			var rd = 0;
			var cc = 0;
			var business_unit_id = hdn_company.Value;
			var new_woprog_id = 0;
	
			var dsn = branch.GetBUDSN(Convert.ToInt32(hdn_company.Value));
			Session["DSNName"] = dsn;
			strExpectedSalesValue = txt_expect_sales.Text.Replace(",", "").Replace("$", "");
			double sales_value = 0;
			double.TryParse(strExpectedSalesValue, out sales_value);
			quotedamount = (ddlquote.Value != null && ddlquote.Value.ToString() != "0" && quotedamount == 0) ? Convert.ToDouble(strExpectedSalesValue) : 0;
			var DiscountFlag = 0;
			var Tax1 = 0;
			var Tax2 = 0;
			var Tax3 = 0;
			var Tax4 = 0;
			var isrebill = 0;
			var iscredit = 0;
			var woprog_wocredit = 0;
			var assetid = ddlca.SelectedValue == null ? 0 : Convert.ToInt32(ddlca.SelectedValue);
			var wo_count = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM woprog  where woprog_customer_id = @v0 and woprog_address_id = @v1 ", new[] { ddlcustomername.Value,ddllocation.SelectedValue });
		
			var tempcustbvid = new NECustomer(Convert.ToInt32(ddlcustomername.Value));

		#region Set up wo header tax stuff
		var taxinfo = new NEAddress(Convert.ToInt32(ddllocation.SelectedValue));
		Tax1 = string.IsNullOrWhiteSpace(taxinfo.Tax1Exempt) ? taxinfo.Tax1 : 0;
		Tax2 = string.IsNullOrWhiteSpace(taxinfo.Tax2Exempt) ? taxinfo.Tax2 : 0;
		Tax3 = string.IsNullOrWhiteSpace(taxinfo.Tax3Exempt) ? taxinfo.Tax3 : 0;
		Tax4 = string.IsNullOrWhiteSpace(taxinfo.Tax4Exempt) ? taxinfo.Tax4 : 0;
		try
		{
	//		List<int> company_taxes = new List<int>() { branch.company_Tax1, branch.company_Tax2, branch.company_Tax3, branch.company_Tax4 };
			
					Tax1 = string.IsNullOrWhiteSpace(taxinfo.Tax1Exempt) ? taxinfo.Tax1_Consol : 0;
					Tax2 = string.IsNullOrWhiteSpace(taxinfo.Tax2Exempt) ? taxinfo.Tax2_Consol : 0;
					Tax3 = string.IsNullOrWhiteSpace(taxinfo.Tax3Exempt) ? taxinfo.Tax3_Consol : 0;
					Tax4 = string.IsNullOrWhiteSpace(taxinfo.Tax4Exempt) ? taxinfo.Tax4_Consol : 0;
			
			if (ddlquote.Value!=null && branch.TaxQuotedJobs == 0 && Convert.ToInt32(ddlquote.Value.ToString()) != 0)
			{
				// If the selected company doesn't tax quoted jobs, and there is a quote selected, zero out all taxes.
				Tax1 = Tax2 = Tax3 = Tax4 = 0;
			}
		}
		catch (Exception ee)
		{
			Toolbox.do_errorLog_errorStack(ee);
		}
		#endregion

		lblError.Text = "";
		error_flag = false;
		var newWOBussinessUnit = new NeBusinessUnit(BusinessUnitDropDownList.SelectedValue);

		#region create the base work order to grab the ID

		#region Actual MSQL insert query is here!
		var my_comm = new MySqlCommand();
		my_comm.Connection = conn;
		#region CommandText
		my_comm.CommandText = @"
INSERT INTO woprog 
	(
    business_unit_id,
    woprog_bvwo,
    woprog_customername,
    woprog_opendatetime,
    woprog_closedatetime,
    woprog_cutdatetime,
    woprog_description,
    woprog_quoteid,
    woprog_pm_memberid,
    woprog_custpo,
    woprog_invoiceno,
    woprog_invoicedate,
    woprog_customer_id,
    woprog_status,
    woprog_interbranch_woprog_id,
    woprog_uncertainty,
    woprog_advancement,
    woprog_rdflag,
    woprog_servicecall,
    woprog_cutby_memberid,
    woprog_department,
    woprog_contact_id,
    woprog_address_id,
    woprog_location_in_plant,
    woprog_specialinstructions,
    woprog_erid,
    woprog_labourtotalsell,
    woprog_materialtotalsell,
    woprog_jobcost_tracking,
    woprog_billed_as_quoted,
    woprog_quotedhourstotal,
    woprog_quotedamount,
    woprog_timesheet_percentage,
    woprog_invoicednettotal,
    woprog_localtax1,
    woprog_localtax2,
    woprog_localtax3,
    woprog_hoursspentquoting,
    woprog_expected_sales_value,
    woprog_laborcost,
    woprog_materialcost,
    woprog_associate_woprog_id,
    woprog_sp_memberid,
    woprog_quotedprice_type,
    woprog_tax1,
    woprog_tax2,
    woprog_tax3,
    woprog_tax4,
    woprog_progressbilled,
    woprog_stilltobebilled,
    woprog_totaltandm,
    woprog_grossmargin,
    `material sell estimated`,
    `labour sell estimated`,
    woprog_customer_consolidation,
    woprog_expected_startdate,
    woprog_expected_enddate,
    woprog_hold,
    woprog_makeid,
    woprog_modelid,
    woprog_erwarranty,
    woprog_erquotedprice,
    woprog_apply_discount,
    woprog_corecompetency,
    woprog_customersnotes,
    woprog_expectedcheckrun,
    woprog_creditcard_payment,
    woprog_collection_notes,
    woprog_last_email_date,
    woprog_last_faxed_date,
    woprog_invoice_paid_date,
    woprog_dayscredit,
    woprog_whyhold,
    woprog_onholdmemberid,
    woprog_vis_to_cust,
    woprog_invoice_tax,
    woprog_invoice_collection_status,
    woprog_iscredit,
    woprog_isrebill,
    woprog_whycredit,
    woprog_wocredit,
    woprog_invoicebalance,
    woprog_invoicecreditamt,
    woprog_servicereportdesc,
    woprog_custpo_member_id,
    woprog_custpo_dt,
    woprog_verbal_quote,
    woprog_exp_labor,
    woprog_assetid,
    woprog_default_invoicetype,
    woprog_auto_invoice,
    woprog_glposting_instructions,
    woprog_isdownpayment,
    schedule_status_id,
    woprog_notification_sent,
    woprog_survey_sent,
    warranty,
    survey_finished,
	parent_woprog_id,
	currency_id,
	default_labour_sell_gl,
	default_material_sell_gl,
	inspection_required,
	inspection_link,
	labor_only, 
    mat_only,
    sub_only,
    fixed_labour_rate ,
    fixed_material_markup ,
sustainability_project ,
use_fixed_labour_rate ,
use_fixed_material_markup ,
acting_ram,
woprog_jobtag_id,
revenue_line_id
	) 
VALUES
    (
	@business_unit_id,
	@bvwo,
	@customername,
	@opendatetime,
	@closedatetime,
	NOW(),
	@description,
	@quoteid,
	@pm_memberid,
	@custpo,
	@invoiceno,
	@invoicedate,
	@customer_id,
	@status,
	@interbranch_woprog_id,
	@uncertainty,
	@advancement,
	@rdflag,
	@servicecall,
	@cutby_memberid,
	@department,
	@contact_id,
	@address_id,
	@location_in_plant,
	@specialinstructions,
	@erid,
	@labourtotalsell,
	@materialtotalsell,
	@jobcost_tracking,
	@billed_as_quoted,
	@quotedhourstotal,
	@quotedamount,
	@timesheet_percentage,
	@invoicednettotal,
	@localtax1,
	@localtax2,
	@localtax3,
	@hoursspentquoting,
	@expected_sales_value,
	@laborcost,
	@materialcost,
	@associate_woprog_id,
	@sp_memberid,
	@quotedprice_type,
	@tax1,
	@tax2,
	@tax3,
	@tax4,
	@progressbilled,
	@stilltobebilled,
	@totaltandm,
	@grossmargin,
	@materialsellestimated,
	@laboursellestimated,
	@customer_consolidation,
	@expected_startdate,
	@expected_enddate,
	@hold,
	@makeid,
	@modelid,
	@erwarranty,
	@erquotedprice,
	@apply_discount,
	@corecompetency,
	@customersnotes,
	@expectedcheckrun,
	@creditcard_payment,
	@collection_notes,
	@last_email_date,
	@last_faxed_date,
	@invoice_paid_date,
	@dayscredit,
	@whyhold,
	@onholdmemberid,
	@vis_to_cust,
	@invoice_tax,
	@invoice_collection_status,
	@iscredit,
	@isrebill,
	@whycredit,
	@wocredit,
	@invoicebalance,
	@invoicecreditamt,
	@servicereportdesc,
	@custpo_member_id,
	@custpo_dt,
	@verbal_quote,
	@exp_labor,
	@assetid,
	@default_invoicetype,
	@auto_invoice,
	@glposting_instructions,
	@isdownpayment,
	@schedule_status_id,
	@notification_sent,
	@survey_sent,
	@warranty,
	@survey_finished,
	@parent_woprog_id,
	@currency,
	@default_labour_sell_gl,
	@default_material_sell_gl,
	@inspection_required,
	@inspection_link,
	@labor_only,
@mat_only,
 @sub_only,
    @fixed_labour_rate ,
    @fixed_material_markup ,
@sustainability_project ,
@use_fixed_labour_rate ,
@use_fixed_material_markup,
@acting_ram,0,
@revenue_line_id
	)";
		#endregion CommandText
		#region Parameters
		my_comm.Parameters.AddWithValue("@address_id", ddllocation.SelectedValue);
		my_comm.Parameters.AddWithValue("@advancement", "");
		my_comm.Parameters.AddWithValue("@apply_discount", DiscountFlag);
		my_comm.Parameters.AddWithValue("@assetid", assetid);
		my_comm.Parameters.AddWithValue("@associate_woprog_id", 0);
		my_comm.Parameters.AddWithValue("@auto_invoice", 0);
		my_comm.Parameters.AddWithValue("@billed_as_quoted", 0);
		my_comm.Parameters.AddWithValue("@closedatetime", null);
		my_comm.Parameters.AddWithValue("@collection_notes", null);
		my_comm.Parameters.AddWithValue("@business_unit_id", hdn_company.Value);
		my_comm.Parameters.AddWithValue("@contact_id", ddlcontact.SelectedValue);
		my_comm.Parameters.AddWithValue("@corecompetency", "");
		my_comm.Parameters.AddWithValue("@creditcard_payment", cc);
		my_comm.Parameters.AddWithValue("@customer_consolidation", null);
		my_comm.Parameters.AddWithValue("@customer_id", ddlcustomername.Value.ToString().Trim());
		my_comm.Parameters.AddWithValue("@customername", ddlcustomername.SelectedItem.Text.Replace("'", ""));
		my_comm.Parameters.AddWithValue("@customersnotes", "");
		my_comm.Parameters.AddWithValue("@custpo", txt_po.Text.Trim());
		my_comm.Parameters.AddWithValue("@custpo_dt", txt_po.Text.Trim() != "" ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : null);
		my_comm.Parameters.AddWithValue("@custpo_member_id", 0);
		my_comm.Parameters.AddWithValue("@cutby_memberid", current_user.id);
		my_comm.Parameters.AddWithValue("@cutdatetime", null);
		my_comm.Parameters.AddWithValue("@dayscredit", Convert.ToInt32(tempcustbvid.customer_creditdays));
		my_comm.Parameters.AddWithValue("@default_invoicetype", tempcustbvid.Credit_Type);
		my_comm.Parameters.AddWithValue("@department", BusinessUnitDropDownList.SelectedValue != "" ? BusinessUnitDropDownList.SelectedValue : "0");
		my_comm.Parameters.AddWithValue("@description", text_desc.Text.Replace("'", "").Replace("\"", "").Replace(",", ""));
		my_comm.Parameters.AddWithValue("@erid", "0");
		my_comm.Parameters.AddWithValue("@erquotedprice", 0);
		my_comm.Parameters.AddWithValue("@erwarranty", 0);
		my_comm.Parameters.AddWithValue("@exp_labor", txt_expected_hrs.Text != "" ? txt_expected_hrs.Text.Trim() : null);
		my_comm.Parameters.AddWithValue("@expected_enddate", Convert.ToDateTime(expected_endDate.Text).ToString("yyyy-MM-dd"));
		my_comm.Parameters.AddWithValue("@expected_sales_value", strExpectedSalesValue);
		my_comm.Parameters.AddWithValue("@expected_startdate", expected_startDate.Text != "" ? Convert.ToDateTime(expected_startDate.Text).ToString("yyyy-MM-dd") : null);
		my_comm.Parameters.AddWithValue("@expectedcheckrun", null);
		my_comm.Parameters.AddWithValue("@glposting_instructions", null);
		my_comm.Parameters.AddWithValue("@grossmargin", 0);
		my_comm.Parameters.AddWithValue("@hold", false);
		my_comm.Parameters.AddWithValue("@hoursspentquoting", hoursspentonqote);
		my_comm.Parameters.AddWithValue("@interbranch_woprog_id", null);
		my_comm.Parameters.AddWithValue("@invoice_collection_status", null);
		my_comm.Parameters.AddWithValue("@invoice_paid_date", null);
		my_comm.Parameters.AddWithValue("@invoice_tax", null);
		my_comm.Parameters.AddWithValue("@invoicebalance", 0);
		my_comm.Parameters.AddWithValue("@invoicecreditamt", 0);
		my_comm.Parameters.AddWithValue("@invoicedate", null);
		my_comm.Parameters.AddWithValue("@invoicednettotal", 0);
		my_comm.Parameters.AddWithValue("@invoiceno", null);
		my_comm.Parameters.AddWithValue("@iscredit", iscredit);
		my_comm.Parameters.AddWithValue("@isdownpayment", is_downpayment);
		my_comm.Parameters.AddWithValue("@isrebill", isrebill);
		my_comm.Parameters.AddWithValue("@jobcost_tracking", 0);
		my_comm.Parameters.AddWithValue("@laborcost", 0);
		my_comm.Parameters.AddWithValue("@laboursellestimated", 0);
		my_comm.Parameters.AddWithValue("@labourtotalsell", 0);
		my_comm.Parameters.AddWithValue("@last_email_date", null);
		my_comm.Parameters.AddWithValue("@last_faxed_date", null);
		my_comm.Parameters.AddWithValue("@localtax1", 0);
		my_comm.Parameters.AddWithValue("@localtax2", 0);
		my_comm.Parameters.AddWithValue("@localtax3", 0);
		my_comm.Parameters.AddWithValue("@location_in_plant", "");
		my_comm.Parameters.AddWithValue("@makeid", 0);
		my_comm.Parameters.AddWithValue("@materialcost", 0);
		my_comm.Parameters.AddWithValue("@materialsellestimated", 0);
		my_comm.Parameters.AddWithValue("@materialtotalsell", 0);
		my_comm.Parameters.AddWithValue("@modelid", 0);
		my_comm.Parameters.AddWithValue("@notification_sent", null);
		my_comm.Parameters.AddWithValue("@onholdmemberid", 0);
		my_comm.Parameters.AddWithValue("@opendatetime", null);
		my_comm.Parameters.AddWithValue("@parent_woprog_id", 0);
		my_comm.Parameters.AddWithValue("@pm_memberid", ddlpm.SelectedValue);
		my_comm.Parameters.AddWithValue("@progressbilled", 0);
		my_comm.Parameters.AddWithValue("@quotedamount", quotedamount);
		my_comm.Parameters.AddWithValue("@quotedhourstotal", 0);
		my_comm.Parameters.AddWithValue("@quotedprice_type", null);
		my_comm.Parameters.AddWithValue("@quoteid", ddlquote.Value);
		my_comm.Parameters.AddWithValue("@rdflag", rd);
		my_comm.Parameters.AddWithValue("@schedule_status_id", null);
		my_comm.Parameters.AddWithValue("@servicecall", false);
		my_comm.Parameters.AddWithValue("@servicereportdesc", null);
		my_comm.Parameters.AddWithValue("@sp_memberid", 0);
		my_comm.Parameters.AddWithValue("@specialinstructions", "");
		my_comm.Parameters.AddWithValue("@status", OpsWOStatus.Open);
		my_comm.Parameters.AddWithValue("@stilltobebilled", 0);
		my_comm.Parameters.AddWithValue("@survey_finished", null);
		my_comm.Parameters.AddWithValue("@survey_sent", null);
		my_comm.Parameters.AddWithValue("@tax1", Tax1);
		my_comm.Parameters.AddWithValue("@tax2", Tax2);
		my_comm.Parameters.AddWithValue("@tax3", Tax3);
		my_comm.Parameters.AddWithValue("@tax4", Tax4);
		my_comm.Parameters.AddWithValue("@timesheet_percentage", 0);
		my_comm.Parameters.AddWithValue("@totaltandm", 0);
		my_comm.Parameters.AddWithValue("@uncertainty", "");
		my_comm.Parameters.AddWithValue("@verbal_quote", false);
		my_comm.Parameters.AddWithValue("@vis_to_cust", true);
		my_comm.Parameters.AddWithValue("@warranty", false);
		my_comm.Parameters.AddWithValue("@whycredit", null);
		my_comm.Parameters.AddWithValue("@whyhold", "");
		my_comm.Parameters.AddWithValue("@wocredit", woprog_wocredit);
		my_comm.Parameters.AddWithValue("@currency", currency);
		my_comm.Parameters.AddWithValue("@default_labour_sell_gl", "");
		my_comm.Parameters.AddWithValue("@default_material_sell_gl", "");
		my_comm.Parameters.AddWithValue("@inspection_required", chk_inspection.Checked);
		my_comm.Parameters.AddWithValue("@inspection_link", tb_inspection.Text);
	    my_comm.Parameters.AddWithValue("@labor_only", chk_labor_only.Checked);
            my_comm.Parameters.AddWithValue("@mat_only", 0);
            my_comm.Parameters.AddWithValue("@sub_only", 0);
            my_comm.Parameters.AddWithValue("@fixed_labour_rate", 0);
            my_comm.Parameters.AddWithValue("@fixed_material_markup", 0);
            my_comm.Parameters.AddWithValue("@sustainability_project", 0);
            my_comm.Parameters.AddWithValue("@use_fixed_labour_rate", 0);
            my_comm.Parameters.AddWithValue("@use_fixed_material_markup", 0);
            my_comm.Parameters.AddWithValue("@acting_ram", taxinfo.csp.ram_member_id!=0? taxinfo.csp.ram_member_id: Convert.ToInt32(ddlpm.SelectedValue));
        my_comm.Parameters.AddWithValue("@revenue_line_id", ddlRevenueLines.Value);
            #endregion Parameters

            try
		{
			my_comm.ExecuteNonQuery();
			my_comm.CommandText = "SELECT LAST_INSERT_ID()";
			new_woprog_id = Convert.ToInt32(my_comm.ExecuteScalar().ToString());
		}
		catch (Exception ex)
		{
			Toolbox.do_errorLog_errorStack(ex);
			throw;
		}
		#endregion

		#endregion
		
		#region - now do the all the peripheral stuff like closing quotes and setting up stuff.

			if (error_flag) return;
				{
				try
					{

					var now = Toolbox.MySQLNow_long();
					var added_wo = new NeWOProg(new_woprog_id);
					added_wo.OrderNumber = new_woprog_id.ToString().PadLeft(10, '0');
					added_wo.Save();
					wo = added_wo;
                    //hid_ts.Value = added_wo.woprog_ts.Ticks.ToString();


                    try
                    {
                        Toolbox.doSQL_void("Delete from wo_checklist_history where woprog_id = @v0", new object[] { new_woprog_id });
                        DataTable dt_cl = Toolbox.doSQL_dt("Select * from wo_checklist where status = 1 order by checklist_order ", null);
                        foreach (DataRow dr_cl in dt_cl.Rows)
                        {
                            Toolbox.doSQL_void("Insert into wo_checklist_history (wo_checklist_id,woprog_id,dt,complete) values(@v0,@v1,now(),0)", new object[] { dr_cl["id"], new_woprog_id });
                        }
                    }
                    catch { }

                    #region Change customer status, if applicable.
                    if (added_wo.WOProg_Customer_ID != 0)
						{
						var csp = new customer_sales_properties(added_wo.woprog_Address_ID);
						if (wo_count == 0) // No work orders cut before, move this customer's status to "Onboarding"  
							{
							csp.status_id = 9;
							csp.save();
							NECustomer.add_to_customer_history(added_wo.WOProg_Customer_ID,current_user.id,DateTime.Now,"First Work Order Added - Changing status to 'Onboarding'",18,1,added_wo.woprog_Address_ID);
							}
						if (wo_count == 2) // 2 work orders cut before, move this customer's status to "Customer", or status_id 3 
							{
							csp.status_id = 3;
							csp.save();
							NECustomer.add_to_customer_history(added_wo.WOProg_Customer_ID, current_user.id, DateTime.Now, "Third Work Order Added - Changing status to 'Customer'",18,1,added_wo.woprog_Address_ID);
							}
						if (csp.status_id == 0)  // if this address is stale, reset it to customer
							{
							csp.status_id = 3;
							csp.save();
							NECustomer.add_to_customer_history(added_wo.WOProg_Customer_ID, current_user.id, DateTime.Now, "New Work Order Added - Changing status from 'stale' back to 'Customer'",18,1,added_wo.woprog_Address_ID);
							}
						}
					#endregion Change status, if applicable.

					#region If Linked To a Quote
					if (ddlquote.Value != null && ddlquote.Value.ToString() != "0")  // if it's a quoted job, save the quote line.
						{
						#region Insert Quote Line
						var x = 2;
						var tempquote1 = new quote(Convert.ToInt32(ddlquote.Value.ToString()));
						try
							{
							var tempquote = new quote().ReceiveNeQuotes(Convert.ToInt32(ddlquote.Value.ToString()), added_wo.woprog_id, "",current_user.id, Convert.ToInt32(ddlcontact.SelectedValue), dsn);
							}
						catch (Exception ex)
							{
							throw new Exception(ex + " Error opening the Quote");
							}

						quotedamount = tempquote1.Price == null || new NeBusinessUnit(tempquote1.business_unit_id).is_er ? 0 : Convert.ToDouble(tempquote1.Price);

						hoursspentonqote = Convert.ToDouble(tempquote1.HoursSpent);
						#region Update Quote History
						var pmmember = new NeMember(Convert.ToInt32(ddlpm.SelectedValue));
						
						try
							{
							Toolbox.doSQL_void(conn, @"INSERT INTO quote_history 
(create_datetime, created_by, quote_id, revision, event) 
VALUES(now(),@v0,@v1,@v2,@v3)", new object[] {  current_user.id.ToString() , tempquote1.QuoteID.ToString() , tempquote1.Revision.ToString(),
								"Received - WO:" + added_wo.OrderNumber + " PM:" + pmmember.FirstName + " " + pmmember.LastName + " StartDate:" + expected_startDate.Text + " EndDate:" + expected_endDate.Text });
							}
						catch (Exception ex)
							{
							throw new Exception(ex.Message);
							}
						#endregion Update Quote History
			
						if (newWOBussinessUnit.is_er)
							{
							#region Repair
							var _quote = new quote(Convert.ToInt32(ddlquote.Value.ToString()));
							var dt = Toolbox.doSQL_dt(conn, @"Select * from quote_worksheet  where quote_id =@v0 and revision =@v1  and consignment_id !=0", new object[] { _quote.QuoteID,_quote.Revision });
							if (dt.Rows.Count > 0)
								{
								#region Has Consignment
								foreach (DataRow dr in dt.Rows)
									{
									var wo_quote_line = new NeWODetailCurrent
										{
										tax1 = branch.TaxQuotedJobs == 1 ? Tax1 : 0,
										tax2 = branch.TaxQuotedJobs == 1 ? Tax2 : 0,
										tax3 = branch.TaxQuotedJobs == 1 ? Tax3 : 0,
										tax4 = branch.TaxQuotedJobs == 1 ? Tax4 : 0,
										description = dr["description"].ToString(),
										master_id = 2139,
										code = "QUOTE",
										business_unit_id = branch.id,
										qty_ordered = 1,
										qty_committed = 1,
										qty_invoiced = 1,
										cost = .01,
										sell = Convert.ToDouble(dr["extended_per"]),
										unit = Convert.ToDouble(dr["extended_per"]),
										woprog_id = new_woprog_id,
										bvwo = Convert.ToInt32(wo.OrderNumber),
										origin = "Automatically Added",
										billtypeid = 3,
										rec_no = x,
										type = "Q",
										consignment_id = Convert.ToInt32(dr["consignment_id"])
										};
									try
										{
										wo_quote_line.save(current_user, "/sections/workorder/index.aspx.cs - create_workorder #4", false);
										}
									catch (Exception ex)
										{
										_tools.catch_error(ex);
										throw new Exception(ex.Message);
										}
									var consign = new consignment(Convert.ToInt32(dr["consignment_id"]));
									consign.load();
									consign.status = consignment.StatusType.InProcess;
									consign.save();
									x++;
									}
								#endregion Has Consignment
								}
							else
								{
								#region Doesn't Have Consignment

								var wo_quote_line = new NeWODetailCurrent
									{
									tax1 = branch.TaxQuotedJobs == 1 ? Tax1 : 0,
									tax2 = branch.TaxQuotedJobs == 1 ? Tax2 : 0,
									tax3 = branch.TaxQuotedJobs == 1 ? Tax3 : 0,
									tax4 = branch.TaxQuotedJobs == 1 ? Tax4 : 0,
									description = "Price as Quoted Q: " + ddlquote.Value.ToString(),
									master_id = 2139,
									code = "QUOTE",
									business_unit_id = branch.id,
									qty_ordered = 1,
									qty_committed = 1,
									qty_invoiced = 1,
									cost = .01,
									sell = quotedamount,
									unit = quotedamount,
									woprog_id = new_woprog_id,
									bvwo = Convert.ToInt32(wo.OrderNumber),
									origin = "Automatically Added",
									billtypeid = 3,
									rec_no = 2,
									type = "Q"
									};
								try
									{
									wo_quote_line.save(current_user, "/sections/workorder/index.aspx.cs - create_workorder #5", false);
									}
								catch (Exception ex)
									{
									_tools.catch_error(ex);
									throw new Exception(ex.Message);
									}
								#endregion Doesn't Have Consignment
								}
							#endregion Repair
							}
						else
							{
							var wo_quote_line = new NeWODetailCurrent
								{
								tax1 = branch.TaxQuotedJobs == 1 ? Tax1 : 0,
								tax2 = branch.TaxQuotedJobs == 1 ? Tax2 : 0,
								tax3 = branch.TaxQuotedJobs == 1 ? Tax3 : 0,
								tax4 = branch.TaxQuotedJobs == 1 ? Tax4 : 0,
								description = "Price as Quoted Q: " + ddlquote.Value.ToString(),
								master_id = 2139,
								code = "QUOTE",
								business_unit_id = branch.id,
								qty_ordered = 1,
								qty_committed = 1,
								qty_invoiced = 1,
								cost = 0,
								sell = quotedamount,
								unit = quotedamount,
								woprog_id = new_woprog_id,
								bvwo = Convert.ToInt32(added_wo.OrderNumber),
								origin = "Automatically Added",
								billtypeid = 11,
								rec_no = 2,
								type = "Q"
								};
							try
								{
								wo_quote_line.save(current_user, "/sections/workorder/index.aspx.cs - create_workorder #6", false);
								}
							catch (Exception ex)
								{
								_tools.catch_error(ex);
								throw new Exception(ex.Message);
								}
							}

						var tempquote2 = new quote(Convert.ToInt32(ddlquote.Value.ToString()));
						#region build task list

						var quote_details = Toolbox.doSQL_dt(conn, @"Select linetext,line_number from quote_extratext  where quote_id =@v0 and revision =@v1  and type =1 order by line_number", new object[] { tempquote2.QuoteID,tempquote2.Revision });
						foreach (DataRow qd in quote_details.Rows)
							{
							Toolbox.doSQL_void(conn, @"Insert into woprog_tasks (woprog_id,task,complete,_order)  Values(@v0,@v1,0,@v2)",new[] { new_woprog_id,qd["linetext"],qd["line_number"] } );
							}
						#endregion

						#endregion Insert Quote Line
						}
					#endregion If Linked To a Quote


					//	Clear_all();
					#region update customer status
					try
						{
						Toolbox.doSQL_void(conn, @"Insert into customer_history 
(customer_history_memberID,customer_history_Action,customer_history_custID,customer_history_Date,customer_history_notes) 
Values (@v0,@v1,@v2,now(),@v3)", new[] {  current_user.id ,2,
							ddlcustomername.Value ,
							"WO Cut: " + added_wo.OrderNumber + " in " + current_user.business_unit_name + ": " + added_wo.Description });
						}
					catch (Exception ex)
						{
						throw new Exception(ex.ToString());
						}
					#endregion

					hidWOProgID.Value = new_woprog_id.ToString();
					if (redirect)
						{
						cb_wo_header.JSProperties["cp_alert"] = "Work Order Created";
						cb_wo_header.JSProperties["cp_newid"] = hidWOProgID.Value;
						Context.ApplicationInstance.CompleteRequest();
						}
					}
				catch (Exception ex4)
					{
					var _trace = new StackTrace(ex4, true);
					var error_message = string.Format("Error Saving Work Order into Intranet : {0} from {1}: {2}<br />", business_unit_id, dsn, ex4.Message, _trace.GetFrame(0).GetFileLineNumber());
					lblError.Text = error_message;

					error_flag = true;
					Toolbox.doSQL_void(conn, @"INSERT INTO Error (Error_Member_ID,Error_DSN,Error_Desc,Error_DateTime) VALUES (@v0 , @v1 , @v2 , NOW()) ", new object[] {  current_user.id, dsn, ex4 } );
					}
				}

			#endregion
			}
	}
	protected void save_click()
	{

		validate_wo();
		cb_wo_header.JSProperties["cp_close"] = "true";
		if (lblError.Text == "")
		{
			if (hidWOProgID.Value == "0")
			{
		//		if ((hdn_company.Value == "8")||(hdn_company.Value=="38"))
		//		{
					CreateWorkOrder(true);
		//		}
		//		else
		//		{
		//			create_workorder(true);
		//		}
			}
			else
			{
				save_workorder();
			}
		}
	}
	private void quote_unattach(MySqlConnection conn, string _was_quote, string _is_quote)
	{
		if (_was_quote == "0") return;
		int is_rev, is_quote_id, was_rev, was_quote_id;
		var quote_number = Convert.ToInt32(_was_quote);
		quote.splice(_was_quote, out was_quote_id, out was_rev);
		quote.splice(quote_number, out is_quote_id, out is_rev);
		quote.unattach(is_quote_id, is_rev);

		//Remove Quote Line If Exists
		var details = new NeWODetailCurrent();
		var quote_lines = Toolbox.doSQL_dt(conn, @"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = 2139 AND wo_detail_current_type = 'Q' AND wo_detail_current_billtypeid IN (3,11)", new object[] { wo.woprog_id });
		if (quote_lines.Rows.Count > 1)
		{
			throw new Exception("There are multiple quote lines on this work order, this should never happen.");
		}
		else
		{
			Toolbox.doSQL_void(conn, @"DELETE FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  and wo_detail_current_master_id = 2139 AND wo_detail_current_type = 'Q' AND wo_detail_current_billtypeid IN (3,11)", new object[] { wo.woprog_id });
		}
		// Move all other items that are job cost billtype to regular billtype
		var dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id != 2139", new object[] { wo.woprog_id });
		foreach (DataRow dr in dt.Rows)
		{
			var consignment_id = Convert.ToInt32(dr["wo_detail_current_consignment_id"]);
			var origin = dr["wo_detail_current_origin"].ToString();
			if (consignment_id != 0 && !origin.Contains("Expense Reimbursement") && !origin.Contains("Per Diem Expense") && !origin.Contains("Company Credit Card Expense"))
			{
				var consign = new consignment(consignment_id);
				consign.load();
				consign.status = consignment.StatusType.Quoted;
				consign.save();
			}
			Toolbox.doSQL_void(conn, @"UPDATE wo_detail_current SET wo_detail_current_billtypeid = '0' WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_billtypeid = '1'", new object[] { wo.woprog_id });
		}
		NeWODetailCurrent.reorder_lines(wo.woprog_id);

		//Update Quote History
		var pmmember = new NeMember(Convert.ToInt32(ddlpm.SelectedValue));
		quote.add_history(conn, current_user.id32, was_quote_id, was_rev, string.Format("Unreceived - WO:{0} PM:{1} StartDate: {2} EndDate: {3}", wo.OrderNumber, pmmember.FullName, Toolbox.MySQL_shortdt(wo.woprog_Expected_StartDate), Toolbox.MySQL_shortdt(wo.woprog_Expected_EndDate)));
	}
	protected void save_workorder()
	{
		using (var conn = Toolbox.connect())
			{
			var taxinfo = new NEAddress();

		#region Company Checking
		var tempcompany = new NeBusinessUnit();
		try
		{
			tempcompany = new NeBusinessUnit(Convert.ToInt32(hdn_company.Value));
		}
		catch
		{
			lblError.Text = "Company value not defined.";
			lblError.Visible = true;
			return;
		}
		#endregion Company Checking

		#region Tax Checking
		try
		{
			taxinfo = new NEAddress(Convert.ToInt32(ddllocation.SelectedValue));
			if (taxinfo.id == 0)
			{
				lblError.Text = "Something is wrong with the address on this work order";
				return;
			}
		}
		catch
		{

		}
		#endregion Tax Checking

		var workorder1 = wo;
		#region Checking if work order has been updated since last load.
		if (hid_ts.Value != workorder1.woprog_ts.Ticks.ToString())
		{
			lblError.Text = "The information in this work order has changed since last accessed. <br/> You may want to <a href='javascript:location.href = location.href;'>refresh</a> the page so as not to cause a conflict";

			return;
		}
		#endregion Checking if work order has been updated since last load.

		var Checkpoints = new List<string>();
		// Capture current quote_id
		var pre_save_quoteid = workorder1.QuoteID.Trim();
		var pre_quote_is_set = pre_save_quoteid != "" && pre_save_quoteid != "0";
		var int_pre_save_quoteid = pre_quote_is_set ? Convert.ToInt32(pre_save_quoteid) : 0;

		var quotedamount = 0.0;
		var hoursspentonqote = 0.0;
		var error_flag = false;

		var DSNNEW = "";
		DSNNEW = tempcompany.DSN;


		var ddl_quote_value = 0;
		try
		{
			if (ddlquote.Value.ToString().Contains("V"))
			{
				try
				{
					ddl_quote_value = Convert.ToInt32(ddlquote.Value.ToString().Substring(0, 8).Trim().Replace("V", ""));
				}
				catch (Exception ee)
				{
					ddl_quote_value = 0;
					_tools.catch_error(ee);
				}
			}
			else
			{
				ddl_quote_value = Convert.ToInt32(ddlquote.Value.ToString());
			}
		}
		catch
		{
			lblError.Text = "Quote value not defined.";
			ddlquote.Focus();
			return;
		}
		double prev_qty_billed = 0;
		double prev_sell_billed = 0;
		var c_prev_billed = Toolbox.doSQL_int(@"SELECT COUNT(wo_detail_current_id) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND (wo_detail_current_billtypeid = 3 or wo_detail_current_billtypeid = 11) AND wo_detail_current_master_id = 2139", new object[] {  workorder1.woprog_id } );
		var c_associated = Toolbox.doSQL_int(@"SELECT COUNT(woprog_id) FROM woprog WHERE woprog_associate_woprog_id = @v0 ", new object[] {  workorder1.woprog_id } );
		var c_associated_open = Toolbox.doSQL_int(@"SELECT COUNT(woprog_id) FROM woprog WHERE woprog_associate_woprog_id = @v0  AND woprog_status = 'Open'", new object[] {  workorder1.woprog_id } );
		if (c_prev_billed > 0)
		{
			var dt_prev_billed = Toolbox.doSQL_dt(@"SELECT wo_detail_current_qty_committed qty, wo_detail_current_price_sell sell FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND (wo_detail_current_billtypeid = 3 or wo_detail_current_billtypeid = 11) AND wo_detail_current_master_id = 2139", new object[] {  workorder1.woprog_id } );
			if (dt_prev_billed.Rows.Count > 0)
			{
				var dr_prev_billed = dt_prev_billed.Rows[0];
				prev_qty_billed = 1 - Convert.ToDouble(dr_prev_billed["qty"]);
				prev_sell_billed = Convert.ToDouble(dr_prev_billed["sell"]);
			}
		}
		if (pre_save_quoteid != "0" && (ddl_quote_value == 0 || (pre_quote_is_set && ddl_quote_value != int_pre_save_quoteid)))
		{
			var _dt = Toolbox.doSQL_dt(@"SELECT woprog_id, woprog_bvwo FROM woprog WHERE woprog_associate_woprog_id = @v0 ", new object[] {  workorder1.woprog_id } );
			foreach (DataRow _dr in _dt.Rows)
			{
				var woprog_id = Convert.ToInt32(_dr["woprog_id"]);
				#region Add history line for this progress billing to the jobcost.
				var progbill_statusline = Toolbox.doSQL_string(@" SELECT CAST( CONCAT ( 'Progress Billed WO: ', RIGHT(woprog_bvwo,6), ' Invoice: ', RIGHT(IFNULL(woprog_invoiceno,'N/A'),6), ' $', FORMAT(woprog_invoicednettotal,2), IF(woprog_quotedamount=0, '', CONCAT(' (',ROUND((woprog_invoicednettotal/woprog_quotedamount)*100,3),'%)') ) ) as CHAR) line_text FROM woprog WHERE woprog_id = @v0  ", new object[] {  woprog_id } );
				Toolbox.doSQL_void(@" INSERT INTO woprogstatus ( woprogstatus_woprog_id, woprogstatus_member_id, woprogstatus_status, woprogstatus_datetime, note ) VALUES ( @v0 , @v1 , 'Quote Unlinked - Removed Info', NOW(), @v2  )", new object[] {  workorder1.woprog_id, current_user.id, progbill_statusline  } );
				#endregion Add history line for this progress billing to the jobcost.
			}

		}
		if (c_prev_billed > 0 && ddl_quote_value != 0 && pre_quote_is_set && ddl_quote_value != int_pre_save_quoteid)
		{
			_tools.catch_error(new Exception(string.Format("c_prev_billed: {0}\nddl_quote_value: {1}\npre_quote_is_set: {2}\nint_pre_save_quoteid: {3}", c_prev_billed, ddl_quote_value, pre_quote_is_set, int_pre_save_quoteid)));
			ddlquote.DataBind();
			//lblError.Text = "You cannot remove a quote link when there have already been progress billings. Please contact a member of NESI to discuss your options.";
			var linked_wos = new List<string>();
			if (c_associated > 0 && c_associated_open == 0)
			{
				#region All are invoiced, just unlink them from woprog_associate_woprog_id and add them to wo_associated.
				var _dt = Toolbox.doSQL_dt(@"SELECT woprog_id, woprog_bvwo FROM woprog WHERE woprog_associate_woprog_id = @v0 ", new object[] {  workorder1.woprog_id } );
				foreach (DataRow _dr in _dt.Rows)
				{
					var woprog_id = Convert.ToInt32(_dr["woprog_id"]);
					var woprog_bvwo = _dr["woprog_bvwo"].ToString();
					var wo_assoc = new wo_associated();
					wo_assoc.woprog_id_a = workorder1.woprog_id;
					wo_assoc.woprog_id_b = woprog_id;
					wo_assoc.save();
					var wo_comment = string.Format(@"Quote #{0} was removed by {1}, the progress billing WO: #{2} has been removed", workorder1.QuoteID, current_user.FullName, woprog_bvwo);
					Toolbox.doSQL_void(@" INSERT INTO woprogcomment ( woprogcomment_woprog_id, woprogcomment_member_id, woprogcomment_text, woprogcomment_x, woprogcomment_y, woprogcomment_datetime ) VALUES ( @v0 , @v1 , @v2 , 0, 0, NOW() )", new object[] {  workorder1.woprog_id, current_user.id, wo_comment  } );
					linked_wos.Add(woprog_bvwo);
				}
				Toolbox.doSQL_void(@"UPDATE woprog SET woprog_associate_woprog_id = 0 WHERE woprog_associate_woprog_id = @v0 ", new object[] {  workorder1.woprog_id } );
				#endregion
			}
			else if (c_associated > 0 && c_associated_open > 0)
			{
				#region A little more work, we need to get all the invoiced work orders and run through the procedure above.
				var _dt = Toolbox.doSQL_dt(@"SELECT woprog_id, woprog_bvwo FROM woprog WHERE woprog_associate_woprog_id = @v0  AND woprog_status = 'Invoiced'", new object[] {  workorder1.woprog_id } );
				foreach (DataRow _dr in _dt.Rows)
				{
					var woprog_id = Convert.ToInt32(_dr["woprog_id"]);
					var woprog_bvwo = _dr["woprog_bvwo"].ToString();
					var wo_assoc = new wo_associated();
					wo_assoc.woprog_id_a = workorder1.woprog_id;
					wo_assoc.woprog_id_b = woprog_id;
					wo_assoc.save();
					var wo_comment = string.Format(@"Quote #{0} was removed by {1}, the progress billing WO #{2}'s link has been removed", workorder1.QuoteID, current_user.FullName, woprog_bvwo);
					Toolbox.doSQL_void(@" INSERT INTO woprogcomment ( woprogcomment_woprog_id, woprogcomment_member_id, woprogcomment_text, woprogcomment_x, woprogcomment_y, woprogcomment_datetime ) VALUES ( @v0 , @v1 , @v2 , 0, 0, NOW() )", new object[] {  workorder1.woprog_id, current_user.id, wo_comment  } );
					linked_wos.Add(woprog_bvwo);
				}
				// Then we need to run through all the open progress billings and delete them.
				_dt = Toolbox.doSQL_dt(@"SELECT woprog_id, woprog_bvwo FROM woprog WHERE woprog_associate_woprog_id = @v0  AND woprog_status != 'Invoiced'", new object[] {  workorder1.woprog_id } );
				foreach (DataRow _dr in _dt.Rows)
				{
					var woprog_id = Convert.ToInt32(_dr["woprog_id"]);
					var woprog_bvwo = _dr["woprog_bvwo"].ToString();
					var wo_assoc = new wo_associated();
					wo_assoc.woprog_id_a = workorder1.woprog_id;
					wo_assoc.woprog_id_b = woprog_id;
					wo_assoc.save();
					Toolbox.doSQL_void(@"UPDATE woprog SET woprog_status = 'Deleted' WHERE woprog_id = @v0 ", new object[] {  woprog_id } );
					var wo_comment = string.Format(@"Quote #{0} was removed by {1}, the progress billing WO #{2}'s link has been removed", workorder1.QuoteID, current_user.FullName, woprog_bvwo);
					Toolbox.doSQL_void(@" INSERT INTO woprogcomment ( woprogcomment_woprog_id, woprogcomment_member_id, woprogcomment_text, woprogcomment_x, woprogcomment_y, woprogcomment_datetime ) VALUES ( @v0 , @v1 , @v2 , 0, 0, NOW() )", new object[] {  workorder1.woprog_id, current_user.id, wo_comment  } );
					linked_wos.Add(woprog_bvwo);
				}
				Toolbox.doSQL_void(@"UPDATE woprog SET woprog_associate_woprog_id = 0 WHERE woprog_associate_woprog_id = @v0 ", new object[] {  workorder1.woprog_id } );
				#endregion
			}
			if (linked_wos.Count > 0)
			{
				shared.alert_invoicing("Quote was unlinked from job cost while having progress billings.", string.Format(@"
	<table>
		<tr>
			<td><b>Job Cost:</b></td>
			<td>{0}</td>
		</tr>
		<tr>
			<td><b>Previous Quote #:</b></td>
			<td>{1}</td>
		</tr>
		<tr>
			<td><b>Who unlinked?:</b></td>
			<td>{2}</td>
		</tr>
		<tr>
			<td><b>Affected progress billings:</b></td>
			<td>{3}</td>
		</tr>
		<tr>
			<td><b>Open Progress Billings Deleted?:</b></td>
			<td>{4}</td>
		</tr>
	</table>
All progress billings are linked on the 'Linked WOs' tab, inside the job cost work order.<br/>
Credits will, most likely, need to be added.
",
			workorder1.OrderNumber,
			workorder1.QuoteID,
			current_user.FullName,
			linked_wos.Aggregate((a, x) => a + "," + x),
			(c_associated_open > 0)
					));
			}
		}
		quote tempquote2;
		quote previous_quote;
		double this_qty_billed = 1;
		if (ddl_quote_value > 0)  // this job is linked to a quote
		{
			tempquote2 = new quote(ddl_quote_value);
			quotedamount = Convert.ToDouble(tempquote2.Price);
			// Check to see if this has any Progress Bills / Down Payments out there
			previous_quote = new quote(Convert.ToInt32(pre_save_quoteid));
			if (c_associated > 0)
			{
				// If so, we need to do a little footwork here to achieve a correct quantity, otherwise it defaults to 1....
				var previous_quotevalue = Convert.ToDouble(previous_quote.Price);
				if (quotedamount != prev_sell_billed)
				{
					this_qty_billed = 1 - Math.Round(prev_sell_billed / quotedamount, 2);
					if (this_qty_billed < 0)
					{
						ddlquote.DataBind();
						lblError.Text = "You cannot link a quote with a lower quoted value than the original quote, when there have already been progress billings.";
						return;
					}
				}
			}
		}




		workorder1.business_unit_id = Convert.ToInt32(hdn_company.Value);
		var tempcust = new NECustomer(Convert.ToInt32(ddlcustomername.Value));
		workorder1.CustomerName = tempcust.Customer_Name;
		workorder1.Description = text_desc.Text.Replace("'", "").Replace("\"", "").Replace(",", ".");
		if (quotedamount == 0 && ddl_quote_value > 0)
		{
			var temp_quote = new quote(ddl_quote_value);
			double temp_price = temp_quote.Price;
			if (temp_price == 0)
			{
				error_flag = true;
				Checkpoints.Add("Quoted price was zero");
				lblError.Text = "The selected quote doesn't have a quoted price set. Cannot proceed until the price is supplied on the quote.";
				return;
			}
		quotedamount = temp_price;
		}
		workorder1.QuoteID = ddl_quote_value.ToString();
		workorder1.intProjectManager = Convert.ToInt32(ddlpm.SelectedValue);
		workorder1.woprog_sp_memberid = Convert.ToInt16(ddlpm.SelectedValue);
		if (workorder1.PONumber != txt_po.Text)
		{
			workorder1.woprog_custpo_member_id = current_user.id;
			workorder1.woprog_custpo_dt = DateTime.Now;
		}
		workorder1.PONumber = txt_po.Text;
		workorder1.WOProg_Customer_ID = Convert.ToInt32(ddlcustomername.Value);


	
		workorder1.woprog_Contact_ID = Convert.ToInt32(ddlcontact.SelectedValue);
		workorder1.woprog_Address_ID = Convert.ToInt32(ddllocation.SelectedValue);
		workorder1.woprog_Expected_EndDate = Convert.ToDateTime(expected_endDate.Text);

		if (expected_startDate.Text != "")
		{
			workorder1.woprog_Expected_StartDate = Convert.ToDateTime(expected_startDate.Text);
		}
			#region Unreceive Quote
			try
			{
				if (pre_save_quoteid != "0" && pre_save_quoteid != ddl_quote_value.ToString()) // if the quote has changed.. we need to do some work.
				{
					quote_unattach(conn, pre_save_quoteid, ddl_quote_value.ToString());
					var tempquote1 = new quote(Convert.ToInt32(pre_save_quoteid));
					var pmmember = new NeMember(Convert.ToInt32(ddlpm.SelectedIndex));
					var quote_history_information = "UnReceived from WO:" + workorder1.OrderNumber + " - (" + pre_save_quoteid + ") was replaced by (" + ddl_quote_value + ")";
					var sql_quote_history = "INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event) VALUES(now(),"
							+ current_user.id + ", " + tempquote1.QuoteID + ", " + tempquote1.Revision + ", '" + quote_history_information + "')";
					try
					{
						_tools.getSQL_void(@"INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event)  VALUES(now(),@v0,@v1,@v2,@v3)", new object[] { current_user.id, tempquote1.QuoteID, tempquote1.Revision, quote_history_information });
					}
					catch (Exception ex)
					{
						_tools.catch_error(ex);
						throw new Exception(ex.ToString());
					}
					//_tools.getSQL_void(@"delete from woprog_tasks  where woprog_id =@v0", new object[] { workorder1.woprog_id });
				}
			}
			catch (Exception ex4)
			{
				error_flag = true;
			}
			#endregion Unreceive Quote
			#region Receive Quote
			var quote_was_received = false;
			try
			{
				// Get the current count for 2139 lines
				var n_lines_q = Toolbox.doSQL_int(conn, @"SELECT COUNT(wo_detail_current_id) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = 'Q' AND (wo_detail_current_billtypeid = 3 or wo_detail_current_billtypeid = 11) AND wo_detail_current_master_id = 2139", new object[] { workorder1.woprog_id });
				if (n_lines_q == 0)
				{
					if (ddl_quote_value > 0 && pre_save_quoteid != ddl_quote_value.ToString())
					{
						quote_was_received = true;
						var quotesuccess = new quote().ReceiveNeQuotes(ddl_quote_value,
						    workorder1.woprog_id,
																			txt_po.Text,
																			current_user.id,
																			Convert.ToInt32(ddlcontact.SelectedValue),
																			tempcompany.DSN
																			);
						tempquote2 = new quote(ddl_quote_value);
						quotedamount = Convert.ToDouble(tempquote2.Price);

						if (quotesuccess)
						{
							#region Add 2139 - (Price As Quoted) Line
							Toolbox.doSQL_void(conn, @" UPDATE wo_detail_current SET wo_detail_current_billtypeid = 1 WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_billtypeid = 0", new object[] { workorder1.woprog_id });
							var quote_line = new NeWODetailCurrent();
							var linecounts = quote_line.GetLineCount(workorder1.woprog_id);
							quote_line.tax1 = tempcompany.TaxQuotedJobs == 1 ? workorder1.woprog_tax1 : 0;
							quote_line.tax2 = tempcompany.TaxQuotedJobs == 1 ? workorder1.woprog_tax2 : 0;
							quote_line.tax3 = tempcompany.TaxQuotedJobs == 1 ? workorder1.woprog_tax3 : 0;
							quote_line.tax4 = tempcompany.TaxQuotedJobs == 1 ? workorder1.woprog_tax4 : 0;
							quote_line.description = "Price as Quoted Quote: " + ddl_quote_value;
							quote_line.master_id = 2139;
							quote_line.code = "QUOTE";
							quote_line.business_unit_id = workorder1.business_unit_id;
							quote_line.qty_committed = this_qty_billed;
							quote_line.qty_invoiced = this_qty_billed;
							quote_line.type = "Q";
							quote_line.cost = 0;// quotedamount - (quotedamount * tempquote2.margin);
							quote_line.sell = quotedamount;
							quote_line.unit = quotedamount;
							quote_line.woprog_id = workorder1.woprog_id;
							quote_line.bvwo = Convert.ToInt32(workorder1.OrderNumber);
							quote_line.origin = "Automatically Added";
							quote_line.billtypeid = 11;
							quote_line.rec_no = linecounts + 2;
							quote_line.added_by = current_user.id;
							quote_line.save(current_user, "/sections/workorder/index.aspx.cs - create_workorder #1", false);
							#endregion Add 2139 Line
							if (linecounts > 0)
							{
								#region 2139 line exists,
								var lineid = _tools.getSQL_int(@"SELECT wo_detail_current_id FROM wo_detail_current  WHERE wo_detail_current_woprog_id=@v0 AND wo_detail_current_rec_no =@v1  AND wo_detail_current_master_id = 2139", new object[] { workorder1.woprog_id, quote_line.rec_no });
								var lastlabor = 0;
								var firstpart = 0;
								var lastquote = 0;
								var newinv = new inventory();
								var linetable = _tools.getSQL_datatable(@"SELECT wo_detail_current_master_id, wo_detail_current_code, wo_detail_current_rec_no FROM wo_detail_current  WHERE wo_detail_current_woprog_id=@v0 ORDER BY wo_detail_current_rec_no", new object[] { workorder1.woprog_id });
								foreach (DataRow row in linetable.Rows)
								{
									var this_rec_no = Convert.ToInt32(row["wo_detail_current_rec_no"]);
									var this_master_id = row["wo_detail_current_master_id"].ToString();
									if (this_rec_no > 99000)
									{
										lastlabor = this_rec_no;
									}
									else if (this_master_id == "2139")
									{
										lastquote = this_rec_no;
									}
									else if (lastlabor != 0)
									{
										firstpart = this_rec_no;
										break;
									}
								}
								if (firstpart == 0)
								{
									//quote_line.SwitchRecNo(linecounts + 2, 2, workorder1.woprog_id, lineid);
								}
								else
								{
									//quote_line.SwitchRecNo(linecounts + 2, firstpart, workorder1.woprog_id, lineid);
									//quote_line.SwitchRecNo(firstpart, 2, workorder1.woprog_id, lineid);
								}
								#endregion 2139 line exists,
							}
						}
						#region build task list
						var quote_details = _tools.getSQL_datatable(@"Select linetext,line_number from quote_extratext  where quote_id =@v0 and revision =@v1  and type =1 order by line_number", new object[] { tempquote2.QuoteID, tempquote2.Revision });
						foreach (DataRow qd in quote_details.Rows)
						{
							_tools.getSQL_void(@"Insert into woprog_tasks (woprog_id,task,complete,_order)  Values(@v0,@v1,0,@v2)", new object[] { workorder1.woprog_id, qd["linetext"], qd["line_number"] });
						}
						#endregion

						#region Update Quote History
						var pmmember = new NeMember(Convert.ToInt32(ddlpm.SelectedValue));
						var quote_history_information = string.Format(@"Received - WO:{0}  PM:{1} Start Date:{2} End Date:{3}", workorder1.OrderNumber, pmmember.FullName, Toolbox.MySQL_shortdt(workorder1.woprog_Expected_StartDate), Toolbox.MySQL_shortdt(workorder1.woprog_Expected_EndDate));
						var sql_quote_history = string.Format(@"
INSERT INTO quote_history 
	(
	create_datetime, 
	created_by, 
	quote_id, 
	revision, 
	event
	) 
VALUES
	(
	NOW(),
	{0},
	{1},
	{2},
	""{3}""
	)",
						current_user.id,
						tempquote2.QuoteID,
						tempquote2.Revision,
						quote_history_information);
						try
						{
							_tools.getSQL_void(@" INSERT INTO quote_history ( create_datetime, created_by, quote_id, revision, event ) VALUES ( NOW(), @v0 , @v1 , @v2 , @v3  )", new object[] { current_user.id, tempquote2.QuoteID, tempquote2.Revision, quote_history_information });
						}
						catch (Exception ex)
						{
							error_flag = true;
							throw new Exception(ex.ToString());
						}
						#endregion Update Quote History
						#region Send Email
						if (tempcompany.id == 1)
						{
							var mail = new NeEMail();
							mail.To = EmailID.OakQuoteUpdate + Toolbox.app_setting("DomainForEmail");
							mail.Subject = "A Quote Has Been Added to a Work Order";
							mail.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
							var body = "Cut - WO:" + workorder1.OrderNumber + "\n";
							try
							{
								body += "Customer Name: " + Toolbox.doSQL_string(conn, @"SELECT customer_name FROM customer  where Customer_ID =@v0", new object[] { ddlcustomername.Value }) + "\n";
							}
							catch (Exception ex)
							{
								throw new Exception(ex.ToString());
							}
							body += " Description: " + tempquote2.txtJobDescription + "\n";
							body += " StartDate:" + Toolbox.MySQL_shortdt(workorder1.woprog_Expected_StartDate) + "\n";
							body += "\n";
							body += "QUOTE ID: " + tempquote2.QuoteID + "\n";
							body += "Revision: " + tempquote2.Revision + "\n";
							body += "Value of Quote: " + tempquote2.Price + "\n";
							body += "Price Type: " + tempquote2.price_type + "\n";
							body += "Work Sheet Total T+M: ";
							double worksheettandm = 0;
							try
							{
								worksheettandm = Toolbox.doSQL_double(conn, @"SELECT ifnull(SUM(original_sell * qty),0) FROM quote_worksheet  WHERE quote_id =@v0 AND revision =@v1 ", new object[] { tempquote2.QuoteID, tempquote2.Revision });
							}
							catch (Exception ex)
							{
								throw new Exception(ex.ToString());
							}
							body += worksheettandm.ToString("C2") + "\n";
							body += "Customer Phone: ";
							var customerphone = "";
							try
							{
								customerphone = Toolbox.doSQL_string(conn, @"SELECT CONCAT('(',Address_PhoneArea,') ',Address_PhoneFirst,'-',Address_PhoneLast,' EXT:',Address_PhoneExt) FROM address  WHERE Address_ID =@v0", new object[] { ddllocation.SelectedValue });
							}
							catch (Exception ex)
							{
								throw new Exception(ex.ToString());
							}
							body += customerphone + "\n";
							// body += "Customer On Hold: ";
							//if (tempcustbvid.Hold == "T")
							//{
							//    body += "Yes\n";
							//}
							//else
							//{
							//    body += "No\n";
							//}
							body += " Customer Contact: ";
							var contactname = "";
							try
							{
								contactname = Toolbox.doSQL_string(conn, @"SELECT Contact_Name FROM contact  WHERE Contact_ID =@v0", new object[] { tempquote2.contact_id });
							}
							catch (Exception ex)
							{
								throw new Exception(ex.ToString());
							}
							body += contactname + "\n";
							body += " PM:" + pmmember.FullName + "\n";
							body += " Quote Due Date:" + tempquote2.completion_date + "\n";
							body += " EndDate:" + Toolbox.MySQL_shortdt(workorder1.woprog_Expected_EndDate) + "\n";
							mail.Body = body;
							mail.Send();
						}
						#endregion Send Email
					}
					else if (ddl_quote_value > 0)
					{
						tempquote2 = new quote(ddl_quote_value);
						quotedamount = Convert.ToDouble(tempquote2.Price);
					}
				}
			}
			catch (Exception ex5)
			{
				error_flag = true;
				lblError.Text = "Failure to Receive Quote " + ex5.Message;
				return;
			}
			#endregion Receive Quote
		workorder1.QuotedPrice = quotedamount != 0 ? quotedamount.ToString() : "0.00";
		workorder1.woprog_hoursspentquoting = Convert.ToDouble(hoursspentonqote);
		workorder1.woprog_exp_labor = Toolbox.ReturnZeroIfNull_double(txt_expected_hrs.Text);
		workorder1.woprog_assetid = Toolbox.ReturnZeroIfNull_int(ddlca.SelectedValue);
		workorder1.inspection_required = chk_inspection.Checked;
		workorder1.inspection_link = tb_inspection.Text;

		double sales_value = 0;
		double.TryParse(txt_expect_sales.Text, out sales_value);
		workorder1.woprog_expected_sales_value = sales_value;
		
			
			if (tempcompany.is_er)
			{
				//			workorder1.woprog_ERID = Convert.ToInt32(ex_ddl(ddlERJobID));
			}
			else
			{
				workorder1.woprog_ERID = 0;
			}


		workorder1.revenue_line_id = int.Parse(ddlRevenueLines.Value.ToString());

        if (!error_flag)
		{
			workorder1.SaveWorkOrder();
			workorder1 = new NeWOProg(workorder1.woprog_id);
			hid_ts.Value = workorder1.woprog_ts.Ticks.ToString();
		}
	wo = workorder1;
		cb_wo_header.JSProperties["cp_alert"] = "Work Order Updated";
		cb_wo_header.JSProperties["cp_close"] = "";
	}
		}
	protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
	{
		ddlcustomername.DataBind();
		ddlquote.DataBind();
	    var buid = BusinessUnitDropDownList.SelectedValue;

	    hdn_company.Value = buid;
	    //ddlpm.DataSource = Toolbox.doSQL_dt("SELECT Member_id,NAME FROM vw_activepms WHERE business_unit_id=" + buid, null);
	     //ddlpm.DataBind();


	    }
	protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
	{
		save_click();
		fill_page();
	}
	protected void ddlpm_DataBound(object sender, EventArgs e)
		{
		var li = new ListItem("Select PM", "0");
		ddlpm.Items.Insert(0, li);
		    ddlpm.SelectedIndex = 0;
		    }


    private void DataBindForCustomer( string poprogid, string bu, int customerId)
    {
        var sp = "CALL ds_workorder_customers_withMapping(@v0, @v1, @v2)";
        ddlcustomername.DataSource = Toolbox.doSQL_dt(sp, new object[] { poprogid, bu, customerId });
        ddlcustomername.DataBind();
        if (customerId > 0)
        {
            ddlcustomername.Value = customerId;
            ddlcustomername.SelectedIndex = 
                customerId != 0 && ddlcustomername.Items.Count > 0 && ddlcustomername.Items.FindByValue(customerId) != null ? 
                ddlcustomername.Items.FindByValue(customerId).Index :
                -1;
        }
    }

    private void DataBindForRevenueLine(string bu, int revenueLineId)
    {
        var query = @"
SELECT DISTINCT
  rl.revenue_line_id id,
  rl.Name name
FROM
  revenue_line_business_unit map
  INNER JOIN revenue_line rl
    ON rl.revenue_line_id = map.revenue_line_id AND rl.Active = 1
WHERE map.business_unit_id = @v0;";

        DataTable dt = Toolbox.doSQL_dt(query, new object[] { bu });

        DataView dv = new DataView(dt);

        dv.RowFilter = "id=" + Toolbox.ReturnZeroIfNull_int(revenueLineId).ToString(); // to check revenueLineId exist in Database  or not

        ddlRevenueLines.DataSource = dt;

        ddlRevenueLines.DataBind();

        ddlRevenueLines.Value = dv.Count == 1 ? revenueLineId : 0;

    }

    private int SetDefaultRevenueLine(object quotedObject, int currentValue)
    {
        /*
         * If a quote is linked, the revenue line is set to Quoted
           If a quote is unlinked the revenue line is set to T&M
         */
        var tm = this.GetConfigSettingByQuery(NESI.BLL.Common.Shared.Configuration.GetRevenueLineTM);
        var quoted = this.GetConfigSettingByQuery(NESI.BLL.Common.Shared.Configuration.GetRevenueLineQuoted);
        var defaultTM = Convert.ToInt32(tm);
        var defaultQuoted = Convert.ToInt32(quoted);

        var newValue = 0;
        if (quotedObject == null || string.IsNullOrWhiteSpace(quotedObject.ToString()) || Convert.ToInt32(quotedObject) == 0 )
        {
            // no quoted things
            if (currentValue == 0 || currentValue != defaultQuoted)
            {
                newValue = defaultTM;
            }
            else
            {
                newValue = currentValue;
            }
        }
        else
        {
            // have a quoted wo
            newValue = defaultQuoted;
        }

        return newValue;
    }

    private string GetConfigSettingByQuery(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return "0";
        }

        var revenue_line_id = Toolbox.doSQL_int(@"SELECT revenue_line_id FROM revenue_line WHERE NAME='" + name + "' limit 1");
        // Toolbox.doSQL_int(@"Select customer_term_id from customer where customer_id=@v0 limit 1", new object[] { cboStatus.Value });
        //if ((revenue_line_id==null)
        //{
        //    return "0";
        //}
        return revenue_line_id.ToString();
    }

}