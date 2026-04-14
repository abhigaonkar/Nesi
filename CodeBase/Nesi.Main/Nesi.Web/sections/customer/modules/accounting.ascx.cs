using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class sections_customer_modules_accounting : System.Web.UI.UserControl
	{
	NeMember current_user;
	bool AdminAuthenticated = false;
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
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		/* MH (2017-06-06): We don't need this right now, everything flows from the TE... can't be overridden in the Customer.
		cb_gl_account.DataBind();
		*/
		cb_default_invoicetype.DataBind();
		}
	public void init()
		{
		this_customer					= this_customer == null ? new NECustomer((int) customer_id) : this_customer;
		// TODO: These have to be based off of a gl_type_mapping - type = "C"
		//cb_gl_account.Value				= this_customer.GL_ID != "" ? Convert.ToInt32(this_customer.GL_ID) : 8;
		// cb_sell_level.Value				= this_customer.Address.SellPrice == null || this_customer.Address.SellPrice == "" ? "01" : this_customer.Address.SellPrice.PadLeft(2,'0');
		chk_prompt_tax.Checked			= this_customer.TaxPrompt == "T";
		cb_credit_type.Value			= this_customer.Credit_Type;
		tb_credit_limit.Text			= this_customer.CreditLimit.ToString();
		spin_dayscredit.Value			= this_customer.customer_creditdays;
		cb_statements.Value				= this_customer.StatementType;
		cb_invoices.Value				= this_customer.InvoiceType;
		chk_finance_charges.Checked		= this_customer.ApplyFinanceCharges == "T";
		chk_auto_statement.Checked		= this_customer.customer_autostatements == 1;
		tb_statement_email.Text			= this_customer.customer_autostatement_address;
		tb_cc_statement_email.Text		= this_customer.customer_autostatement_ccaddress;
		tb_invoice_email.Text			= this_customer.customer_invoice_address;
		tb_invoice_cc_email.Text		= this_customer.customer_invoice_ccaddress;
		cb_default_invoicetype.Value	= this_customer.default_invoicetype;
		chk_auto_invoicing.Checked		= this_customer.customer_auto_invoice == 1;
		chk_req_wo.Checked				= this_customer.requires_wo_copy;
		mem_ar_notes.Text = this_customer.customer_arnotes;
		mem_si.Text = this_customer.Memo;
		lb_overallmargin.Text			= !current_user.AuthenticatedForPrivilege(81) && !current_user.isContact 
											? "--" 
											: Toolbox.doSQL_double(@"SELECT customer_margin(@v0 )", new object[] {  customer_id } ).ToString("P2");
		lb_error.Text		= "";
		AdminAuthenticated = current_user.AuthenticatedForPrivilege(115);

		rates_iframe.Attributes["src"] = !AdminAuthenticated ? "about:blank" : "/sections/customer/rates.aspx?customer_id=" + customer_id;
		rates_iframe.Attributes["data-aa"] = !AdminAuthenticated ? "t" : "f";

	

		}
	private void set_ddl(DropDownList ddl, object _value)
	{
		var value = _value == null ? "" : _value.ToString().ToUpper();
		ddl.SelectedValue = value;
	}

	protected void bt_save_Click(object sender, EventArgs e)
		{
		// Preliminary error checking
			lb_error.Text = "";
		var errors			= new List<string>();
		var is_error				= false;
		/* MH (2017-06-06): We don't need this right now, everything flows from the TE... can't be overridden in the Customer.
		if(cb_gl_account.Value == null)
			{
			is_error				= true;
			errors.Add("Please select a GL Account");
			}
		*/
		if(cb_sell_level.Value == null)
			{
			is_error				= true;
			errors.Add("Please select a Selling Price Level");
			}
		if(cb_credit_type.Value == null)
			{
			is_error				= true;
			errors.Add("Please select a Credit Type");
			}
		if(cb_credit_type.Value != null && (int) cb_credit_type.Value == 2 && tb_credit_limit.Text == "")
			{
			is_error				= true;
			errors.Add("Please provide a credit limit");
			}
		if(!is_error)
			{
			try
				{
				this_customer										= new NECustomer((int)customer_id);
				this_customer.Member_ID								= current_user.id;
				// TODO: Move this to the consolidated function
				//this_customer.GL_ID									= cb_gl_account.Value == null ? this_customer.GL_ID : cb_gl_account.Value.ToString();
				this_customer.Address.SellPrice						= cb_sell_level.Value.ToString();
				this_customer.TaxPrompt								= chk_prompt_tax.Checked ? "T" : "F";
				this_customer.Credit_Type							= (int) cb_credit_type.Value;
				this_customer.CreditLimit							= Convert.ToDouble(tb_credit_limit.Text);
				this_customer.customer_creditdays					= (int) spin_dayscredit.Number;
				this_customer.StatementType							= (string) cb_statements.Value;
				this_customer.InvoiceType							= (string) cb_invoices.Value;
				this_customer.ApplyFinanceCharges					= chk_finance_charges.Checked ? "T" : "F";
				this_customer.customer_autostatements				= chk_auto_statement.Checked ? 1 : 0;
				this_customer.customer_autostatement_address		= tb_statement_email.Text;
				this_customer.customer_autostatement_ccaddress		= tb_cc_statement_email.Text;
				this_customer.customer_invoice_address				= tb_invoice_email.Text;
				this_customer.customer_invoice_ccaddress			= tb_invoice_cc_email.Text;
				this_customer.default_invoicetype					= (int) cb_default_invoicetype.Value;
				this_customer.customer_auto_invoice					= chk_auto_invoicing.Checked ? 1 : 0;
				this_customer.requires_wo_copy						= chk_req_wo.Checked;

				this_customer.Save();
				this_customer.Address.Save();
				lb_error.Text		= "";
				}
			catch (Exception ee)
				{
				lb_error.Text		= ee.Message;
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
		rates_iframe.Attributes["src"] = !AdminAuthenticated ? "about:blank" : "./rates.aspx?customer_id=" + customer_id;
		//rates_iframe.Attributes["src"] = "about:blank";//AdminAuthenticated ? "./rates.aspx?customer_id="+customer_id : "";
		rates_iframe.Attributes["data-aa"] = !AdminAuthenticated ? "t" : "f";

		}
	
	protected void cb_note_save_Callback(object source, CallbackEventArgs e)
	{
		this_customer.customer_arnotes = mem_ar_notes.Text;
		this_customer.Memo = mem_si.Text;
		this_customer.Save();
		e.Result = "Customer Notes Saved";
	}
}