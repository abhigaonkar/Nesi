using System;
using System.Web.UI;
using System.Data;
using System.Collections;
using DevExpress.Web;
using nesi.core;

public partial class sections_customer_modules_new : System.Web.UI.UserControl
	{
	NeMember current_user;
	NECustomer this_customer;
	NEAddress this_address;
	bool AdminAuthenticated = false;
	public int business_unit_id
		{
		get {
			if(Session["business_unit_id"] == null)
				{return 0;}
			else
				{return Convert.ToInt32(Session["business_unit_id"]);}
			} 
		set	{
			Session["business_unit_id"]		= value;
			}
		}
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
		AdminAuthenticated = current_user.AuthenticatedForPrivilege(115);
		_tools.dont_cache_page();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		if((!Page.IsPostBack)&&(!Page.IsCallback))
			{
			init(true);
			}
		}
	public void init(bool force)
		{
		var c									= new NeBusinessUnit(newcustomer_com.Value);
		if(newcustomer_country.Items.Count == 0)
			{
			var countrylist						= NeCountry.get_list();
			for (var x = 0; x < countrylist.Count; x++)
				{
				var _country_name					= (countrylist[x]).name;
				var _country_code					= (countrylist[x]).code;
				newcustomer_country.Items.Add(new ListEditItem(_country_name, _country_code));
				}
			}
		if(force)
			{
			newcustomer_country.SelectedItem			= newcustomer_country.Items.FindByValue(c.country);
			}

		if(newcustomer_provstate.Items.Count == 0)
			{
			var _provs						= Toolbox.doSQL_dt(@"SELECT prov_desc,prov_abbv FROM prov"  , null);
			newcustomer_provstate.DataSource		= _provs;
			newcustomer_provstate.TextField			= "prov_desc";
			newcustomer_provstate.ValueField		= "prov_abbv";
			newcustomer_provstate.DataBind();
			}
		
		if(force)
			{
			newcustomer_provstate.SelectedItem			= newcustomer_provstate.Items.FindByValue(c.provstate);
			}

		if(newcustomer_com.Value == null)
			{
			newcustomer_com.Value					= current_user.business_unit.id;
			}
		if(newcustomer_projmgr.Items.Count == 0)
			{
			newcustomer_projmgr.DataBind();
			}

		var i	= 0;
		if(newcustomer_projmgr.SelectedIndex >= 0)
			{
			i = (int) newcustomer_projmgr.Items[newcustomer_projmgr.SelectedIndex].Value;
			}
		if(newcustomer_projmgr.Value == null || (i != 0 && newcustomer_projmgr.Items.FindByValue(i) == null))
			{
				newcustomer_projmgr.DataBind();
			newcustomer_projmgr.SelectedIndex			= -1;
			}
		if(!current_user.AuthenticatedForPrivilege(135))
			{
			newcustomer_acctmgr.Value					= 0;
			newcustomer_acctmgr.ClientEnabled			= false;
			}
		if(newcustomer_email.Value == null)
			{
			newcustomer_email.Value						= "call4APEmail@" + Toolbox.app_setting("DomainForEmail");
			}
		var bu = new NeBusinessUnit(Convert.ToInt32(newcustomer_com.Value));
        // TODO : GL Account Change in customer 
		var _gl_accounts = Toolbox.doSQL_dt(@"SELECT b.id,a.account_no,a.gl_chart_name FROM GL_te a,GL_Group_te b  WHERE a.gl_group_id = b.id AND b.number ='120' AND a.tax_entity_id = @v0" , new object[] {bu.tax_entity_id});
		if(_gl_accounts.Rows.Count > 0 && newcustomer_gl_receivables.Items.Count == 0)
			{
			foreach(DataRow _gl_account in _gl_accounts.Rows)
				{
				var _text						= string.Format("{0}-{1}", _gl_account["account_no"], _gl_account["gl_chart_name"]);
				var _value						= _gl_account["id"].ToString();
				var _gl					= new ListEditItem(_text, _value);
				newcustomer_gl_receivables.Items.Add(_gl);
				}
			if(newcustomer_gl_receivables.Value == null)
				{
				newcustomer_gl_receivables.SelectedIndex		= 0;
				}
			}
		var _taxes = Toolbox.doSQL_dt(@"SELECT 0 id, 'Not Set' name UNION SELECT tax_id id, CONCAT(tax_id,' ',tax_name,' - ', tax_percentage, '%') name FROM tax WHERE is_active = 1 ORDER BY id" , null);
		
		if (newcustomer_tax1.Items.Count == 0)
		{
			newcustomer_tax1.DataSource = _taxes;
			newcustomer_tax1.TextField = "name";
			newcustomer_tax1.ValueField = "id";
			newcustomer_tax1.DataBind();
		}
		if (newcustomer_tax2.Items.Count == 0)
		{
			newcustomer_tax2.DataSource = _taxes;
			newcustomer_tax2.TextField = "name";
			newcustomer_tax2.ValueField = "id";
			newcustomer_tax2.DataBind();
		}
		if (newcustomer_tax3.Items.Count == 0)
		{
			newcustomer_tax3.DataSource = _taxes;
			newcustomer_tax3.TextField = "name";
			newcustomer_tax3.ValueField = "id";
			newcustomer_tax3.DataBind();
		}
		if (newcustomer_tax4.Items.Count == 0)
		{
			newcustomer_tax4.DataSource = _taxes;
			newcustomer_tax4.TextField = "name";
			newcustomer_tax4.ValueField = "id";
			newcustomer_tax4.DataBind();
		}
	
			newcustomer_tax1.ClientEnabled = AdminAuthenticated;
			newcustomer_tax2.ClientEnabled = AdminAuthenticated;
			newcustomer_tax3.ClientEnabled = AdminAuthenticated;
			newcustomer_tax4.ClientEnabled = AdminAuthenticated;
			newcustomer_tax1.ValidationSettings.CausesValidation = AdminAuthenticated;
			newcustomer_tax2.ValidationSettings.CausesValidation = AdminAuthenticated;
			newcustomer_tax3.ValidationSettings.CausesValidation = AdminAuthenticated;
			newcustomer_tax4.ValidationSettings.CausesValidation = AdminAuthenticated;

	

		}
	protected void nc_name_check_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var _dr			= Toolbox.doSQL_dt(@"SELECT COUNT(*) c, customer_id FROM customer WHERE customer_name = @v0  LIMIT 1", new object[] {  e.Parameter.Trim() } ).Rows[0];
		e.Result			= string.Format("{0},{1}", _dr["c"], _dr["customer_id"]);
		}
	protected void nc_phone_check_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var full_phone	= string.Format("{0}", e.Parameter);
		e.Result			= phone_check(full_phone);
		}
	protected string phone_check(string pn)
		{
		var _dr			= Toolbox.doSQL_dt(@" SELECT COUNT(*) c, b.address_table_id id FROM phone_numbers a LEFT JOIN address b ON a.phone_numbers_table_id = b.address_id WHERE phone_numbers_number = @v0  AND phone_numbers_type = 'address' AND phone_numbers_table_id IN ( SELECT d.address_id FROM customer c LEFT JOIN address d ON d.address_table_id = c.customer_id WHERE c.customer_status != 4 ) LIMIT 1", new object[] {  pn } ).Rows[0];
		var custname		= "";
		if(_dr["id"] != DBNull.Value)
			{
			custname		= Toolbox.doSQL_string(@"SELECT customer_name FROM customer a WHERE customer_id = @v0 LIMIT 1", new object[] { _dr["id"]});
			}
		return string.Format("{0},{1},{2}", _dr["c"], custname, _dr["id"]);
		}
	protected void newcustomer_company_SelectedIndexChanged(object sender, EventArgs e)
		{		
		Session["business_unit_id"]		= (int) newcustomer_com.Value;
		newcustomer_projmgr.SelectedIndex	= -1;
		newcustomer_projmgr.DataBind();
		init(true);
		}
	protected void btn_save_new_customer_Click(object sender, EventArgs e)
		{
		// Check if customer name already exists, and the customer_ts > curdate() -- Curdate returns the date, not the time, so it's anything above (for example) "2013-12-24".
		// If there are, this indicates a duplicate.
	
		var c_exists										= Toolbox.doSQL_int(@"SELECT COUNT(customer_id) 
FROM customer WHERE customer_name = @v0 AND customer_ts > CURDATE()", newcustomer_name.Text);
		if(c_exists > 0)
			{
			lb_newerror.Text	= "This customer already exists, the save process has been stopped to prevent duplication. <br/> Please refresh your browser by pressing the F5 key.";
			return;
			}
		else if(newcustomer_name.Text.Length > 60)
			{
			lb_newerror.Text	= "The customer name is too long, please shorten it. The max number of characters for a name is 60 and this name is "+newcustomer_name.Text.Length+" characters";
			return;
			}
		else if ((newcustomer_gl_receivables.SelectedIndex < 0))
		{
			lb_newerror.Text = "You must select a valid GL Account for both the legacy BV setup and the consolidated BV setup.";
			return;
		}
		else
			{
			lb_newerror.Text	= "";
			}
		this_customer									= new NECustomer();
		this_address									= new NEAddress();

		this_customer.Customer_Name						= newcustomer_name.Text;
		this_customer.business_unit_id					= (int) newcustomer_com.Value;
		this_customer.Credit_Type						= 1;
		this_customer.TaxPrompt							= "F";
		this_customer.Hold								= "T";
		this_customer.customer_whyhold					= "New Customer";
		this_customer.StatementType						= "F";
		this_customer.InvoiceType						= "F";
		this_customer.InitMember_ID						= current_user.id;
		this_customer.Member_ID							= current_user.id;
		this_customer.customer_lastorigin				= (int) new_combo_origin.Value;
		this_customer.Notes								= "";
		
		this_customer.ApplyFinanceCharges				= "F";
		this_customer.Account_Manager					= (int) newcustomer_acctmgr.Value;

		this_address.Type								= "B";
		this_address.Desc								= "";
		this_address.Addr1								= newcustomer_addr1.Text;
		this_address.Addr2								= newcustomer_addr2.Text;
		this_address.Addr3								= newcustomer_addr3.Text;
		this_address.Addr4								= newcustomer_addr4.Text;
		this_address.City								= newcustomer_ci1.Text;
		this_address.Prov								= newcustomer_provstate.Value.ToString();
		this_address.Postal								= newcustomer_postal.Text;
		this_address.Country							= newcustomer_country.Value.ToString();
		this_address.PhoneArea							= newcustomer_phone_area.Text;
		this_address.Phonefirst							= newcustomer_phone_prefix.Text;
		this_address.PhoneLast							= newcustomer_phone_suffix.Text;
		this_address.PhoneExt							= newcustomer_phone_ext.Text;
		this_address.Email								= newcustomer_email.Text;
		this_customer.customer_autostatement_address	= newcustomer_email.Text;
		this_address.FaxArea							= newcustomer_fax_area.Text;
		this_address.FaxFirst							= newcustomer_fax_prefix.Text;
		this_address.FaxLast							= newcustomer_fax_suffix.Text;
		
        // Customer RVAccount # 00000
		this_address.RVAccount							= "00000";
		this_address.RVAccount_Consol 					= "41000";

		this_address.Tax1 = Convert.ToInt32(newcustomer_tax1.Value);
		this_address.Tax2 = Convert.ToInt32(newcustomer_tax2.Value);
		this_address.Tax3 = Convert.ToInt32(newcustomer_tax3.Value);
		this_address.Tax4 = Convert.ToInt32(newcustomer_tax4.Value);
		
		this_address.SellPrice							= "01";
		this_address.Table								= "Customer";
		var _cust_id									= this_customer.Save();
		this_address.Table_ID							= _cust_id;
		this_address.id									= (int) _cust_id;
		var _addr_id									= this_address.Save();
		var sp					= new customer_sales_properties(_addr_id);
		sp.address_id									= _addr_id;
		sp.customer_id									= _cust_id;
		sp.status_id									= 1;
		sp.sector										= "";
		sp.po_required									= false;
		sp.project_mgr_member_id						= (int) newcustomer_projmgr.Value;
		sp.next_followup_date							= DateTime.Now.AddMonths(1);
		sp.origin										= this_customer.customer_lastorigin;
		// VP of Sales check for setting default sales reps (Member Type ID: 37)
		if(current_user.membertype.id == 37)
			{
			if((int) newcustomer_isr.Value != 0)
				{
				sp.isr_member_id	= (int) newcustomer_isr.Value;
				}
			
			if((int) newcustomer_osr.Value != 0)
				{
				sp.ram_member_id	= (int) newcustomer_osr.Value;
				}
			if((int) newcustomer_ram.Value != 0)
				{
				sp.osr_member_id	= (int) newcustomer_ram.Value;
				}
			}
		sp.save();
	//	var NSI = new NetSuite_Integration.Customer();
	//	NSI.SyncNetSuite(this_customer, current_user.business_unit.tax_entity_id);
	//	this_customer.sync_bvs(current_user);
	//		ScriptManager.RegisterStartupScript(this, this.GetType(), "redir", "please_wait('stop');alert('New Customer Added, Click ok to Open');location.href = './index.aspx?customer_id=106902';", true);

			ScriptManager.RegisterStartupScript(this, this.GetType(), "redir", "please_wait('stop');alert('New Customer Added, Click ok to Open');location.href = './index.aspx?customer_id=" + _cust_id + "';", true);
		
		//Response.Redirect(string.Format("./index.aspx?customer_id={0}", _cust_id));
		}
	protected void newcustomer_projmgr_SelectedIndexChanged(object sender, EventArgs e)
		{
		}
}