using System;
using System.Data;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using nesi.core;

public partial class sections_customer_modules_notes : System.Web.UI.UserControl
	{
	NeMember current_user;
	public NECustomer this_customer  { get; set; }
	public NEAddress this_address  { get; set; }
	customer_sales_properties this_csp;
	bool AdminAuthenticated = false;
	bool EditPastNotes = false;
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
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
		AdminAuthenticated = current_user.AuthenticatedForPrivilege(115);
		EditPastNotes		= current_user.AuthenticatedForPrivilege(160);
		if(!IsPostBack)
			{
			Session["cust_ar_emails"] = null;
			Session["ar_report_invoice_notes_all"] = null;
			}
		}
	protected void gv_emails_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var gv_emails  = (ASPxGridView)sender;
		var gvdc = (GridViewDataColumn)gv_emails.Columns["emaillog_body"];
		var _html    = (ASPxHtmlEditor)gv_emails.FindEditRowCellTemplateControl(gvdc, "aremails");
		var id               = gv_emails.GetRowValues(gv_emails.EditingRowVisibleIndex, "emaillog_id").ToString();
		_html.Html              = Toolbox.doSQL_string(@"SELECT emaillog_body FROM emaillog WHERE emaillog_id = @v0" , id);
		}

	public void init()
		{
		this_customer	= this_customer == null ? new NECustomer((int) customer_id) : this_customer;
		this_address	= new NEAddress(address_id);
		this_csp		= new customer_sales_properties((int) address_id);
		pc.TabPages[0].Visible		= this_address.Type == "B" && AdminAuthenticated;
		pc.TabPages[1].Visible		= this_address.Type == "B" && AdminAuthenticated;
		pc.TabPages[4].Visible		= this_address.Type == "B" && current_user.business_unit_id == 11;
		pc.TabPages[5].Visible		= this_address.Type == "B" && current_user.business_unit_id == 11;
		customer_memo.Text			= this_customer.Memo;
		customer_arnotes.Text		= this_customer.customer_arnotes;
		customer_salesnotes.Text	= this_csp.notes_sales;
		customer_salesnotes.ReadOnly	= !EditPastNotes;
		bt_savenotes.Visible		= EditPastNotes;
		mem_customer_notes.Text		= this_csp.notes_public;

		if (Session["cust_ar_emails"] == null)
			{
			Session["cust_ar_emails"] = Toolbox.doSQL_dt(@"CALL CUSTCOLLECTIONEMAILS(@v0)",new object[] { customer_id } );
			}

		gv_emails.DataSource = Session["cust_ar_emails"];
		gv_emails.DataBind();
		
		if (pc.TabPages[5].Visible) // only show the invoice notes if you're nesi
			{
			if ((Session["ar_report_invoice_notes_all"] == null) || (Session["ar_report_cust"] == null))
				{
				var dt = Toolbox.doSQL_dt(@" SELECT a.ar_notes_ts as `Date`, a.ar_notes_note as `Note`, c.member_fullname as `By`, b.woprog_invoiceno as Invoice, b.woprog_bvwo as `wo` FROM ar_notes a INNER JOIN woprog b ON a.ar_notes_woprogid = b.woprog_id LEFT JOIN member c ON a.ar_notes_memberid = c.member_id  WHERE b.woprog_customer_id =@v0 ORDER BY a.ar_notes_ts DESC", new object[] { customer_id });
				gv_invoice_notes_all.DataSource = dt;
				gv_invoice_notes_all.DataBind();
				Session["ar_report_invoice_notes_all"] = dt;
				}
			else
				{
				gv_invoice_notes_all.DataSource = Session["ar_report_invoice_notes_all"];
				gv_invoice_notes_all.DataBind();
				}
			}
		}

	private void cb_Callback(string e)
		{
		this_customer	= new NECustomer((int) customer_id);
		this_address	= new NEAddress(address_id);
		this_csp		= new customer_sales_properties((int) address_id);
		switch(e)
			{
			case "save_ar_notes":
				this_customer.customer_arnotes			= customer_arnotes.Text;
				this_customer.Save();
			break;
			case "save_invoicing_instructions":
				this_customer.Memo						= customer_memo.Text;
				this_customer.Save();
			break;
			#region Location Specific
			case "add_public_note":
			this_csp.notes_public = "[" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + current_user.FullName + " - " + memaddpublicnotes.Text + "\n" + this_csp.notes_public + "\n";
				this_csp.save();
				memaddpublicnotes.Text = "";
			break;
			case "add_sales_note":
				this_csp.notes_sales					= "[" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] "+current_user.FullName+" - "+memaddsalesnote.Text+"\n"+this_csp.notes_sales+"\n";
				this_csp.save();
				memaddsalesnote.Text = "";
			break;
			#endregion Location Specific
			}
		init();
		}
	protected void btnaddpublicnotes_Click(object sender, EventArgs e)
		{
		cb_Callback("add_public_note");
		}
	protected void btn_specialinvoicing_Click(object sender, EventArgs e)
		{
		cb_Callback("save_invoicing_instructions");
		}
	protected void btn_ar_notes_Click(object sender, EventArgs e)
		{
		cb_Callback("save_ar_notes");
		}
	protected void btnaddsalesnote_Click(object sender, EventArgs e)
		{
		cb_Callback("add_sales_note");
		}
	protected void bt_savenotes_Click(object sender, EventArgs e)
		{
		this_csp					= new customer_sales_properties((int) address_id);
		this_csp.notes_sales		= customer_salesnotes.Text;
		this_csp.save();

		}
}