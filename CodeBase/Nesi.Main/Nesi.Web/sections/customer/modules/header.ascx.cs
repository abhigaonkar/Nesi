using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Data;
using System.Text;
using nesi.core;

public partial class sections_customer_modules_header : System.Web.UI.UserControl
	{
	NeMember current_user;
	public NECustomer this_customer  { get; set; }
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
        this.DataBinding += new EventHandler(this_databind);
		}
	protected void this_databind(object sender, EventArgs e)
		{
		this.init();
		}
	public bool is_initialized	{get;set;}
	public void init()
		{
		Toolbox.do_debug("Header UC Init Start");
		this_customer						= this_customer == null ? new NECustomer((int) customer_id) : this_customer;
		var AdminAuthenticated				= current_user.AuthenticatedForPrivilege(115);
		if(hdn_customer_id.Value != customer_id.ToString())
			{
			tb_name.Value						= this_customer.Customer_Name;
	
			memo_whyhold.Value					= this_customer.customer_whyhold;
			if (this_customer.Hold == "T" && (this_customer.customer_whohold != 0))
				{
				lbl_whohold.Text = "<b>Put on hold by: </b>"+new NeMember(Convert.ToInt32(this_customer.customer_whohold)).FullName;
				}
			cb_cust_status.Value				= this_customer.Customer_Status;
			ck_partner.Checked					= this_customer.is_partner == null ? false : this_customer.is_partner;
			chk_on_hold.Checked					= this_customer.Hold == "T";
			hdn_customer_id.Value				= customer_id.ToString();
			}
		bt_save.Enabled = true;
		memo_whyhold.Enabled				= AdminAuthenticated;
		chk_on_hold.Enabled					= AdminAuthenticated;
		tb_name.Enabled						= AdminAuthenticated;
		if(this_customer.Address.csp != null)
			{
			h_lblam.Text						= Toolbox.doSQL_string(@"SELECT IFNULL(MAX(member_fullname), 'Not Set')
FROM member WHERE member_id = @v0 LIMIT 1", this_customer.Address.csp.am_member_id);
			h_lblpm.Text						= Toolbox.doSQL_string(@"SELECT IFNULL(MAX(member_fullname), 'Not Set')
FROM member WHERE member_id = @v0 LIMIT 1", this_customer.Address.csp.project_mgr_member_id);
			h_lblbranch.Text					= Toolbox.doSQL_string(@"SELECT IFNULL(MAX(b.description), 'Not Set')
FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE a.member_id = @v0 LIMIT 1", this_customer.Address.csp.project_mgr_member_id);
			}
		is_initialized						= true;

		lb_error.Text						= "";
		Toolbox.do_debug("Header UC Init End");
		}
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(10);
		
		cb_cust_status.DataBind();
		}
	protected void detail_customer_name_check_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var _dr			= Toolbox.doSQL_dt(@"SELECT COUNT(*) c, IFNULL(MAX(customer_id), 0) customer_id FROM customer WHERE customer_name = @v0  AND customer_id != @v1  LIMIT 1", new object[] {  e.Parameter.Trim(), customer_id } ).Rows[0];
		var id				= Convert.ToInt32(_dr["customer_id"]);
		var c				= Convert.ToInt32(_dr["c"]);
		e.Result			= string.Format("{0}|{1}", c, id);
		}
	protected void bt_save_Click(object sender, EventArgs e)
		{
		if(this_customer == null)
			{
			this_customer					= new NECustomer((int) customer_id);
			}
		var prev_hold_status				= this_customer.Hold == "T";
		var prev_name					= this_customer.Customer_Name;
		var errors					= new List<string>();
		var is_error						= false;
		this_customer.Customer_Name			= tb_name.Text;
		if(address_id == 0)
			{
			address_id						= Toolbox.doSQL_int(@"SELECT address_id FROM address 
WHERE address_table = 'Customer' AND address_table_id = @v0 AND address_type = 'B' LIMIT 1", customer_id);
			}
		if(prev_hold_status && !chk_on_hold.Checked && memo_whyhold.Text == this_customer.customer_whyhold)
			{
			errors.Add("You must fill out a (different) reason why you're taking this customer off hold.");
			is_error					= true;
			}
		else if(!prev_hold_status && chk_on_hold.Checked && memo_whyhold.Text == this_customer.customer_whyhold)
			{
			errors.Add("You must fill out a (different) reason why you're putting this customer on hold.");
			is_error					= true;
			}
		else if(!prev_hold_status && chk_on_hold.Checked && memo_whyhold.Text.Trim() == "")
			{
			errors.Add("You must fill out a reason why you're putting this customer on hold.");
			is_error					= true;
			}
	
		if(!is_error)
			{
			
		
			var prev_hold						= this_customer.Hold == "T";
			var this_hold						= chk_on_hold.Checked;
			this_customer.Member_ID				= current_user.id;
			this_customer.customer_whyhold		= memo_whyhold.Text;
			if(current_user.AuthenticatedForPrivilege(135))
				{
				//this_customer.Account_Manager	= (int) cb_acct_mgr.Value;
				}
			this_customer.Customer_Status = Convert.ToInt32(cb_cust_status.Value);
			if(this_hold != prev_hold)
				{
				this_customer.customer_whohold = current_user.id;
				NECustomer.add_to_customer_history(customer_id, current_user.id, System.DateTime.Now, this_customer.customer_whyhold, (prev_hold ? 20 : 19), 1, address_id);
				}
			this_customer.Hold					= chk_on_hold.Checked ? "T" : "F";
			this_customer.is_partner = ck_partner.Checked;
			try
				{
				this_customer.Save();
				if(prev_name != tb_name.Text)
					{
					shared.alert_ar(string.Format("FYI: Customer Name Change - #{0}", this_customer.Customer_Number), string.Format("{1} has changed customer #{0}'s name from <b>{2}</b> to <b>{3}</b>", this_customer.Customer_Number, current_user.FullName, prev_name, tb_name.Text));
					}
		//		var NSI = new NetSuite_Integration.Customer();
		//		NSI.SyncNetSuite(this_customer, current_user.business_unit.tax_entity_id);
				hdn_customer_id.Value = "0";
				init();
				lb_error.Text				= "<div id='lb_notify'><b style='color:#090;'>Saved</b></div>";
				ScriptManager.RegisterStartupScript(this, this.GetType(), "remove", @"setTimeout(""$('#lb_notify').fadeOut()"", 5000);", true);
				}
			catch (Exception ee)
				{
				lb_error.Text				= ee.Message;
				}
			
			}
		else
			{
		
			var sb			= new StringBuilder();
			sb.Append("<div class='title'>There are errors with your submission</div>");
			foreach(var err in errors)
				{
				sb.AppendFormat("<div class='err'>&bullet; {0}</div>",err);
				}
			lb_error.Text		= sb.ToString();
			}
		if(is_error)
			{
			lb_error.ForeColor		= System.Drawing.Color.Red;
			}
		else
			{
			lb_error.ForeColor		= System.Drawing.Color.Green;
			}
		}
	protected void cb_cust_status_Init(object sender, EventArgs e)
	{
		var dt = Toolbox.doSQL_dt(@"Select customer_or_contact_status s, description d from customer_or_contact_status order by customer_or_contact_status_id"  , null);
		var tooltip = "";
		foreach (DataRow dr in dt.Rows)
		{
			tooltip += dr["s"] + ": " + dr["d"] + System.Environment.NewLine;
		}
		cb_cust_status.ToolTip = tooltip;
	}
}