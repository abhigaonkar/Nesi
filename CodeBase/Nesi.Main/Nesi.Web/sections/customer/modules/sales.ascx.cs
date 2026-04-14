using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.UI;
using DevExpress.Web;
using System.Text;
using nesi.core;

public partial class sections_customer_modules_sales : System.Web.UI.UserControl
	{
	public int address_id  
		{ 
		get {var _address_id = 0; if(ViewState["address_id"] == null){return _address_id;}else{int.TryParse(ViewState["address_id"].ToString(), out _address_id); return _address_id; }} 
		set {ViewState["address_id"] = value.ToString();}
		}
	public int customer_id  
		{ 
		get {var _customer_id = 0; if(ViewState["customer_id"] == null){return _customer_id;}else{int.TryParse(ViewState["customer_id"].ToString(), out _customer_id); return _customer_id; }} 
		set {ViewState["customer_id"] = value.ToString();}
		}
    protected override void OnInit(EventArgs e)
        {
        base.OnInit(e);
        this.DataBinding += new EventHandler(sales_databind);
		}
	public NECustomer this_customer  { get; set; }
	public bool is_initialized	{get;set;}
	public NEAddress this_address  { get; set; }
	private const int _page_id			= 10;
	NeMember current_user;
	Toolbox _tools				= new Toolbox();
	bool can_manage_sales_reps	= false;
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(10);
		can_manage_sales_reps	= current_user.AuthenticatedForPrivilege(157);
		
		//sales_tab.TabPages.FindByName("protected").Visible							= can_manage_sales_reps;
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		// init(); // Not needed to fix the issue of not retaining postback data.
		}
	public void sales_databind(object sender, EventArgs e)
		{
		this.init();
		}
	public void init()
		{
		if(address_id != 0 && customer_id != 0)
			{
			Toolbox.do_debug("Sales UC Init Start");
			var csp												= new customer_sales_properties(address_id);
			var _company															= new NeBusinessUnit(current_user.business_unit_id);
			spin_callcycle.Number														= csp.call_cycle;
			cb_yearend.Items.FindByValue(csp.year_end).Selected							= true;
			cb_projectmanager.Value														= csp.project_mgr_member_id;
			cb_controlsmanager.Value													= csp.controls_mgr_member_id;
			spin_discount.Number														= csp.discount_pct;
			cb_decisionmaker.Value														= csp.decision_maker;
			cb_accountcode.Value														= csp.account_code == null ? "0" : csp.account_code;
			date_next_followup.Date														= csp.next_followup_date;
			memo_next_followup.Text														= csp.next_followup_notes;
			tb_job_budget.Text															= csp.job_budget_threshold.ToString();
			chk_porequired.Checked														= csp.po_required;
			rbl_poconfirmed.Value														= csp.confirmed_po_required ? 1 : 0;
			rbl_taxexempt.Value															= csp.confirmed_tax_exempt ? 1 : 0;
			cb_industry.Value															= csp.industry;
			cb_found.Value																= csp.origin;
			cb_empsize.Value															= csp.employee_size;
			tb_sector.Text																= csp.sector;
			tb_naics.Text																= csp.naics_code;
			tb_do_at_location.Text														= csp.do_at_location;
			memo_whychoose.Text															= csp.why_choose;
			memo_affiliated.Text														= csp.affiliated_companies;
			memo_suppliers.Text															= csp.known_suppliers;
			memo_competitors.Text														= csp.known_competitors;
			cb_insidesales.Value														= csp.isr_member_id;
			cb_outsidesales.Value														= csp.osr_member_id;
			cb_regacctmgr.Value															= csp.ram_member_id;
			cb_am.Value = csp.am_member_id;
			hdn_customer_id.Value = customer_id.ToString();
			hdn_address_id.Value = address_id.ToString();
			bt_found.Visible															= current_user.AuthenticatedForPrivilege(132);
			//;
			bt_ratesheet.ClientSideEvents.Click											= string.Format(@"
function(s,e)
	{{
	var _id = rate_sheet_branch.GetValue(); 
	if(_id == null)
		{{
		alert('Please select a branch');
		}}
	else
		{{
		boing('/sections/reports/rates_sheet/index.aspx?business_unit_id='+_id+'&customerid={0}','rates',950,800);
		}}
}}", customer_id);
            cb_ratesheet_branch.DataSource = Toolbox.doSQL_dt("Select id, ddl_name name from business_unit where find_in_set(id,'" + new Current_User().visible_business_units + "')", null);
		cb_ratesheet_branch.DataBind();

		var sql		= @"
SELECT 
	customer_or_contact_status_id id, 
	customer_or_contact_status status
FROM 
	customer_or_contact_status
WHERE
	customer_or_contact_status_id IN ("; 
		switch(csp.status_id)
			{
			case 0:
				sql			+= "0,3,5,6,7,8,9";
			break;
			case 1:
				sql			+= "1,2,3,5,6,8,9";
			break;
			case 2:
				sql			+= "2,3,5,6,9";
			break;
			case 3:
				sql			+= "0,3,5,6,7,8";
			break;
			case 4:
				sql			+= "4";
			break;
			case 5:
				sql			+= "5";
			break;
			case 6:
				sql			+= "1,6";
			break;
			case 7:
				sql			+= "0,3,5,6,7,8";
			break;
			case 8:
				sql			+= "3,5,6,7,8";
			break;
			case 9:
				sql			+= "0,3,5,6,7,8,9";
			break;
			}
		sql			+= @")
ORDER BY 
	customer_or_contact_status_order, id";
		cb_status.DataSource		= Toolbox.doSQL_dt(sql,null);
		cb_status.DataBind();
		cb_status.Items.FindByValue(csp.status_id).Selected		= true;

		
		lb_last_invoice.InnerText = Toolbox.doSQL_string(@"SELECT GET_DAYSSINCELASTINVOICE(@v0, @v1)", new object[] { customer_id, address_id});
		lb_last_invoice.InnerText = lb_last_invoice.InnerText == "999" ? "N/A" : lb_last_invoice.InnerText;
		lb_last_fiscal.InnerText = Toolbox.doSQL_double(@" SELECT IFNULL(SUM(woprog_invoicednettotal), 0) FROM woprog WHERE woprog_customer_id = @v0  AND woprog_address_id = @v4  AND business_unit_id = @v1  AND woprog_invoicedate BETWEEN @v2  AND @v3 ", new object[] {  customer_id, _company.id, _company.fiscal_start_previous.ToString("yyyy-MM-dd"), _company.fiscal_end_previous.ToString("yyyy-MM-dd"), address_id  } ).ToString("C2");
		lb_current_fiscal.InnerText = Toolbox.doSQL_double(@" SELECT IFNULL(SUM(woprog_invoicednettotal), 0) FROM woprog WHERE woprog_customer_id = @v0  AND woprog_address_id = @v4  AND business_unit_id = @v1  AND woprog_invoicedate BETWEEN @v2  AND @v3 ", new object[] {  customer_id, _company.id, _company.fiscal_start_current.ToString("yyyy-MM-dd"), _company.fiscal_end_current.ToString("yyyy-MM-dd"), address_id  } ).ToString("C2");
		var branch_fiscals = shared.GetBV7DSNs();
		double ytd = 0;
		double lytd = 0;
		foreach (DataRow branch_fiscal in branch_fiscals.Rows)
			{
			if (branch_fiscal["id"].ToString() != "8")
				{
				var b = new NeBusinessUnit(branch_fiscal["id"]);
				ytd += _tools.getSQL_double(@" SELECT IFNULL(SUM(woprog_invoicednettotal), 0) FROM woprog WHERE woprog_customer_id = @v0  AND woprog_address_id = @v3  AND business_unit_id = @v1  AND woprog_invoicedate BETWEEN @v2  AND CURDATE()", new object[] {  customer_id, b.id, Toolbox.MySQL_shortdt(b.fiscal_start_current), address_id  } );
				lytd += _tools.getSQL_double(@" SELECT IFNULL(SUM(woprog_invoicednettotal), 0) FROM woprog WHERE woprog_customer_id = @v0  AND woprog_address_id = @v3  AND business_unit_id = @v1  AND woprog_invoicedate BETWEEN @v2  AND @v3 ", new object[] {  customer_id, b.id, Toolbox.MySQL_shortdt(b.fiscal_start_previous), Toolbox.MySQL_shortdt(b.fiscal_end_previous), address_id  } );
				}
			}
		lb_ytd_companywide.InnerText = ytd.ToString("C2");
		lb_lytd_companywide.InnerText = lytd.ToString("C2");
			gv_history.DataBind();
			is_initialized		= true;
			}
			Toolbox.do_debug("Sales UC Init End");
		}
	protected void bt_savesales_Click(object sender, EventArgs e)
		{
		try
			{
			this_customer								= this_customer == null ? new NECustomer((int) customer_id) : this_customer;
			this_address								= this_address == null ? new NEAddress(address_id) : this_address;
			var csp				= new customer_sales_properties(address_id);
			#region Tab 1 -- Properties 1
			csp.status_id								= Convert.ToInt32(cb_status.Value);
			csp.call_cycle								= spin_callcycle.Number > 0 ? Convert.ToInt32(spin_callcycle.Number) : 0;
			csp.year_end								= Convert.ToInt32(cb_yearend.Value);
			csp.project_mgr_member_id					= Convert.ToInt32(cb_projectmanager.Value);
			csp.controls_mgr_member_id					= Convert.ToInt32(cb_controlsmanager.Value);
			var temp_discount							= csp.discount_pct;
			csp.discount_pct							= spin_discount.Number > 0 ? Convert.ToInt32(spin_discount.Number) : 0;
			if (csp.discount_pct != 0 && temp_discount < csp.discount_pct)
				{
				var email = new NeEMail();
				email.Subject = this_customer.Customer_Name + " has had their discount raised to " + csp.discount_pct + "% by " + current_user.FullName;
				var bms = Toolbox.doSQL_dt(@"SELECT member_neemail e FROM member  WHERE member_membertype_id IN (5,29) AND member_status = 'Active'" , null);
				var addresses					= new List<string>();
				foreach (DataRow dr in bms.Rows)
					{
					addresses.Add(dr["e"].ToString());
					}
				email.To = addresses.Aggregate((x,y) => x + ";" + y);
				email.From = "CustomerDiscounts@thatsnew.com";
				email.Send();
				NECustomer.add_to_customer_history(customer_id, current_user.id, System.DateTime.Now, string.Format("Customer discount changed from {0}% to {1}%, for location: {2}", temp_discount, csp.discount_pct, this_address.Addr1), 6, 1, address_id);
				}
			csp.decision_maker							= Convert.ToInt32(cb_decisionmaker.Value);
			csp.account_code							= cb_accountcode.Value == null ? "" : cb_accountcode.Value.ToString();
			csp.next_followup_date						= date_next_followup.Date == null ? new DateTime() : date_next_followup.Date;
			csp.next_followup_notes						= memo_next_followup.Text;
			csp.job_budget_threshold					= Convert.ToDouble(tb_job_budget.Text);
			csp.po_required								= chk_porequired.Checked;
			csp.confirmed_po_required					= (int) rbl_poconfirmed.Value == 1;
			csp.confirmed_tax_exempt					= (int) rbl_taxexempt.Value == 1;
			#endregion Tab 1
			#region Tab 2 -- Properties 2
			csp.industry								= (int) cb_industry.Value;
			csp.origin									= (int) cb_found.Value;
			csp.sector									= tb_sector.Text;
			csp.naics_code								= tb_naics.Text;
			csp.employee_size							= (int) cb_empsize.Value;
			csp.do_at_location							= tb_do_at_location.Text;
			csp.why_choose								= memo_whychoose.Text;
			csp.affiliated_companies					= memo_affiliated.Text;
			csp.known_suppliers							= memo_suppliers.Text;
			csp.known_competitors						= memo_competitors.Text;
			#endregion Tab 2
			#region Tab 3
			csp.isr_member_id							= (int) cb_insidesales.Value;
			csp.osr_member_id							= (int) cb_outsidesales.Value;
			csp.ram_member_id							= (int) cb_regacctmgr.Value;
			csp.am_member_id							= (int) cb_am.Value;
			#endregion Tab 3
			csp.save();
			init();
			lb_notify.Text								= "<div id='lb_notify'><b style='color:#090;'>Saved</b></div>";
			ScriptManager.RegisterStartupScript(this, this.GetType(), "remove", @"setTimeout(""$('#lb_notify').fadeOut()"", 5000);", true);
			cb_am.DataBind();
		}
		catch (Exception ee)
			{
			lb_notify.Text								= "<div><b style='color:#c00'>"+ee+"</b></div>";
			}
		}
	protected void bt_addaction_Click(object sender, EventArgs e)
		{
		var date_action        = (ASPxDateEdit) gv_history.FindTitleTemplateControl("date_action");
		var cb_action			= (ASPxComboBox) gv_history.FindTitleTemplateControl("cb_action");
		var cb_action_member	= (ASPxComboBox) gv_history.FindTitleTemplateControl("cb_action_member");
		var cb_origin			= (ASPxComboBox) gv_history.FindTitleTemplateControl("cb_origin");
		var lb_error				= (ASPxLabel)	 gv_history.FindTitleTemplateControl("lb_error");
		var memo_note              = (ASPxMemo)	 gv_history.FindTitleTemplateControl("memo_note");
		var is_error					= false;
		var errors			= new StringBuilder();

		if (date_action.Date.Year < 1900)
			{
			errors.Append("- Missing Date<br/>");
			is_error			= true;
			}
		if (cb_action.Value == null)
			{
			errors.Append("- Missing Action<br/>");
			is_error			= true;
			}
		if (cb_action_member.Value == null)
			{
			errors.Append("- Missing Member<br/>");
			is_error			= true;
			}
		if (cb_origin.Value == null)
			{
			errors.Append("- Missing Origin<br/>");
			is_error			= true;
			}

		if(is_error)
			{
			lb_error.Text		= errors.ToString();
			}
		else
			{
			try
				{
				var ch		= new customer_history();
				ch.member_id			= (int) cb_action_member.Value;
				ch.action_id			= (int) cb_action.Value;
				ch.date					= date_action.Date;
				ch.customer_id			= customer_id;
				ch.notes				= memo_note.Text;
				ch.origin				= (int) cb_origin.Value;
				ch.address_id			= address_id;
				ch.save();
				gv_history.DataBind();
				date_action.Date		= DateTime.Today;
				cb_action.Value			= null;
				cb_action_member.Value	= null;
				cb_origin.Value			= null;
				memo_note.Text			= "";
				lb_error.Text			= "<div id='lb_error'><b style='color:#090;'>Saved</b></div>";
				ScriptManager.RegisterStartupScript(this, this.GetType(), "remove", @"setTimeout(""$('#lb_error').fadeOut()"", 5000);", true);
				}
			catch (Exception ee)
				{
				lb_error.Text			= "<div id='lb_error'><b style='color:#c00;'>"+ee+"</b></div>";
				}
			}

		}
	protected void cb_saveorigin_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var origin_name		= e.Parameter.Trim();
		if(origin_name == "")
			{
			e.Result			= "Error - Blank Name";
			return;
			}
		var exists				= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM customer_origin WHERE name = @v0", origin_name);
		if(exists > 0)
			{
			e.Result			= "Error - Name already exists";
			return;
			}
		Toolbox.doSQL_void(@"INSERT INTO customer_origin (name) VALUES (PROPER(@v0))",origin_name);
		e.Result				= "SUCCESS";
		}
	protected void combo_lastorigin_Callback(object sender, CallbackEventArgsBase e)
		{
		((ASPxComboBox) sender).DataBind();
		}
}