using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using nesi.core;

public partial class customer_notes : Page
	{
    NeMember current_user;
	NECustomer cust;
	protected NameValueCollection _q;
	private string _cust_id = "0";
	private string _origin = "";
	  Toolbox _tools = new Toolbox();
		bool AdminAuthenticated		= false;

	  protected void Page_PreInit(object sender, EventArgs e)
	  {
		  Page.Theme = "";
	  }

    protected void Page_Load(object sender, EventArgs e)
		{

			current_user = new NeMember(Session["session"].ToString());
			if (!current_user.AuthenticatedForPrivilege(10))
			{
				lblerror.Text = "Sorry you are not authorized to see customer notes";
				lblerror.ClientVisible = true;
				return;
			}
        _tools.dont_cache_page();
		_q = Request.QueryString;
		AdminAuthenticated				= current_user.AuthenticatedForPrivilege(115);
		if (!string.IsNullOrEmpty(_q["customer_id"]))
		{
			_cust_id = _q["customer_id"];
			cust = new NECustomer(Convert.ToInt32(_cust_id));
			if (!string.IsNullOrEmpty(_q["origin"]))
			{
				_origin = _q["origin"];
			}

			if (!IsPostBack)
			{
				Session["cust_ar_emails"] = null;
				Session["ar_report_invoice_notes_all"] = null;
			}
				fill_page();
			
				
		}
		
		
        }

	protected void fill_page()
	{
		cust = new NECustomer(Convert.ToInt32(_cust_id));
		if(!IsCallback)
			{
		customer_memo.Text = cust.Memo;
		customer_arnotes.Text = cust.customer_arnotes;
		customer_salesnotes.Text = cust.salesnotes;
		mem_customer_notes.Text = cust.Notes;
			}

		
			

		try
		{
			if (Session["cust_ar_emails"]==null)
			{
				var dt2 = _tools.getSQL_datatable(@"call custcollectionemails(@v0)",new object[] { _cust_id } );
				Session["cust_ar_emails"] = dt2;
			}
		}
		catch
		{ }

		gv_emails.DataSource = Session["cust_ar_emails"];
		gv_emails.DataBind();

		if (((current_user.business_unit_id == 11)||(_origin=="ar_report"))&&(!current_user.isContact)) // only show the invoice notes if you're nesi
		{
			if ((Session["ar_report_invoice_notes_all"] == null) || (Session["ar_report_cust"] == null))
			{
				try
				{
					var dt = _tools.getSQL_datatable(@"SELECT ar_notes.ar_notes_ts as `Date`, ar_notes.ar_notes_note as `Note`, get_name(ar_notes_memberid) as `By`, woprog.WOProg_InvoiceNo as Invoice, woprog.WOProg_BVWO as `wo` FROM ar_notes INNER JOIN woprog ON ar_notes.ar_notes_woprogid = woprog.WOProg_ID  WHERE woprog.woprog_customer_id =@v0 Order by ar_notes.ar_notes_ts desc", new object[] { _cust_id });
					gv_invoice_notes_all.DataSource = dt;
					gv_invoice_notes_all.DataBind();
					Session["ar_report_invoice_notes_all"] = dt;
				}
				catch
				{ }
			}
			else
			{
				gv_invoice_notes_all.DataSource = Session["ar_report_invoice_notes_all"];
				gv_invoice_notes_all.DataBind();
			}
		}
		else
		{
			pc.TabPages[5].ClientVisible = false;
		}
		pc.TabPages[0].ClientVisible	= AdminAuthenticated;
		pc.TabPages[1].ClientVisible	= AdminAuthenticated;

	}

	
	
	protected void cb_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter == "add_sales_note")
		{
			_tools.getSQL_void(@"update customer set customer_salesnotes =@v0 where customer_id = @v1",
				new object[]
					{
					System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + current_user.FullName2 + "-" +memaddsalesnote.Text + System.Environment.NewLine + cust.salesnotes + System.Environment.NewLine ,
						cust.Customer_ID
					});
			
		memaddsalesnote.Text = "";

		}
		else if (e.Parameter == "add_public_note")
		{
			_tools.getSQL_void(@"update customer set customer_notes = @v0 where customer_id = @v1",
				new object[]
					{
					System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + current_user.FullName2 + "-" + memaddpublicnotes.Text + System.Environment.NewLine + cust.Notes + System.Environment.NewLine ,
						cust.Customer_ID
					});
			memaddpublicnotes.Text = "";
		}
		else if (e.Parameter == "save_invoicing_instructions")
		{
			_tools.getSQL_void(@"update customer set customer_memo = @v0 where customer_id =@v1 " , new object[] { customer_memo.Text, cust.Customer_ID});
		}
		else if (e.Parameter == "save_ar_notes")
		{
			_tools.getSQL_void(@"update customer set customer_arnotes = @v0 where customer_id =@v1 ", new object[] { customer_arnotes.Text, cust.Customer_ID});
		
		}
		
		fill_page();
	}





	
	
	protected void gv_emails_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var gv_emails = (ASPxGridView)sender;
		var gvdc = (GridViewDataColumn)gv_emails.Columns["emaillog_body"];
		var _html = (ASPxHtmlEditor)gv_emails.FindEditRowCellTemplateControl(gvdc,"aremails");
		var id = gv_emails.GetRowValues(gv_emails.EditingRowVisibleIndex, "emaillog_id").ToString();
		
		_html.Html = _tools.getSQL_string(@"select emaillog_body from emaillog  where emaillog_id =@v0", new object[] { id });
	}
}

