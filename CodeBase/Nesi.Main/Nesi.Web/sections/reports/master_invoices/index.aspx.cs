using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Collections.Specialized;
using System.Data;
using nesi.core;

public partial class sections_reports_master_invoices_index : Page
	{
	Toolbox _tools;
	NeMember current_user;
	static int _page_id			= 113;
	static string _page_name		= "MasterInvoices";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;
	ASPxDateEdit dte;
	public string woprogid_notes;
	protected NameValueCollection _q;
	string cust_id;
	string te_id;
	string origin=null;


	protected void Page_PreInit(object sender, EventArgs e)
	{
		_q = Request.QueryString;
		if (!string.IsNullOrEmpty(_q["origin"]))
		{
			origin = _q["origin"];
			cust_id = _q["customer_id"];
			te_id = _q["te_id"];
			Page.Theme = "";
			
		}
		
	}

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools			= new Toolbox();
		current_user	= Toolbox.do_handle_authentication(_page_id);
		layout.__page_name					= _page_name;
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		layout.used_gv			= gv_invoices;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		h.Set("gridview_id", "gv_invoices");
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= current_user.id.ToString();
		_tools.dont_cache_page();
		if (Cache["ds_depend"] == null)
			{
			Cache["ds_depend"]			= DateTime.Now;
			}
		

		}
	protected void Page_Load(object sender, EventArgs e)
		{
		if(!IsCallback && !IsPostBack)
			{
				if (origin == null)
				{
					var gl = new NeGridLayouts(current_user.id, _page_name);
					if (gl.GridLayoutID == 0)
					{
						//		gv_invoices.FilterExpression	= string.Format("[name_branch] = '{0}'", current_user.business_unit.name);
						gl.GridLayout_Layout = gv_invoices.SaveClientLayout();
						gl.member_id = current_user.id;
						gl.GridLayout_Name = "Default";
						gl.GridLayout_Gridid = _page_name;
						gl.SaveGridLayout();

						h.Set("ID", gl.GridLayoutID);
						h.Set("NAME", gl.GridLayout_Name);
					}
					else
					{
						gv_invoices.LoadClientLayout(gl.GridLayout_Layout);
						h.Set("ID", gl.GridLayoutID);
						h.Set("NAME", gl.GridLayout_Name);
					}
					dde_filter.Text = gl.GridLayout_Name;
				}
			Session["master_invoices_gv"] = null;
			}
		dte = (ASPxDateEdit)gv_invoices.FindTitleTemplateControl("dte_multiconfirm");
		if (origin == "ar_report")
		{
			panel_export.Visible = false;
			gv_invoices.Columns["cust_no"].Visible = false;
			gv_invoices.Columns["WOProg_CustomerName"].Visible = false;
			gv_invoices.Columns["business_unit"].Visible = true;
			gv_invoices.Columns["balance"].Width = Unit.Pixel(125);
		}
        gv_invoices.Columns[0].Visible = false;
        fill_grid();


		}

	protected void fill_grid()
	{
		var adder = "";
		if (!current_user.AuthenticatedForPrivilege(110))
		{
			adder = " and tax_entity_id = " + current_user.business_unit.tax_entity_id;
		}
		if ((origin == "ar_report")&&(cust_id!= null)&&(cust_id!="")&&(te_id!=null)&&(te_id!=""))
		{
		//	gv_invoices.Width = Unit.Pixel(870);

			adder += " and woprog_customer_id = " + cust_id + " and tax_entity_id = " + te_id;
			
		}
        
			var dt = _tools.getSQL_datatable(@"
SELECT
	customer_collection_status.ID,
	a.WOProg_ID,
	a.WOProg_BVWO,
	a.WOProg_CustomerName,
	c.contact_name,
	c.contact_email,
	customer_collection_status,
	a.WOProg_CustPO WOProg_CustPO,
	business_unit.ddl_name business_unit,
	a.WOProg_InvoicedNetTotal,
	a.woprog_ExpectedCheckRun,
	IFNULL(a.woprog_invoice_collection_status, 0) woprog_invoice_collection_status,
	a.WOProg_InvoiceNo,
	a.WOProg_InvoiceDate,
	a.woprog_invoice_paid_date,
	(SELECT IFNULL(DATEDIFF(curdate(), (SELECT MAX(Date) FROM membertime WHERE membertime_woprog_id = a.woprog_id)), 0) FROM woprog WHERE woprog_id = a.woprog_id) invoice_lag,
	date_add(a.WOProg_InvoiceDate, interval a.woprog_dayscredit DAY)  as exp_pay,
	a.woprog_customer_id, a.woprog_invoicebalance as balance,
	projman.member_fullname pm,
	a.business_unit_id, datediff(curdate(),WOProg_InvoiceDate) as dayssince,
	(Select ar_notes_note from ar_notes where ar_notes_woprogid = a.woprog_id order by ar_notes_id desc limit 1) as ar_notes_note,
	(Select ar_notes_ts from ar_notes where ar_notes_woprogid = a.woprog_id order by ar_notes_id desc limit 1) as note_date,
	address_phonefull as phone,
	concat(customer_invoice_address,',',customer_invoice_ccaddress,',',customer_autostatement_address,',',customer_autostatement_ccaddress) as customer_emails,
	customer.customer_number as cust_no, 
	csp_m.member_fullname acct_manager ,
	(SELECT IFNULL(DATEDIFF(woprog_opendatetime, (SELECT MAX(Date) FROM membertime WHERE membertime_woprog_id = a.woprog_id)), 0) FROM woprog WHERE woprog_id = a.woprog_id) scan_time,
	(SELECT IFNULL(DATEDIFF(woprog_invoicedate, woprog_opendatetime), 0) FROM woprog WHERE woprog_id = a.woprog_id) processing_time,
  a.woprog_dayscredit terms,
if(" + !current_user.AuthenticatedForPrivilege(58) + @",0,woprog_laborcost) lab_cost, 
	if(" + !current_user.AuthenticatedForPrivilege(58) + @",0,woprog_materialcost) mat_cost,
customer.customer_creditdays cust_terms,
business_unit.tax_entity_id te_id
FROM
	woprog a 
INNER JOIN 
	customer ON 
		 a.woprog_customer_id = customer.Customer_ID
LEFT JOIN 
	customer_collection_status ON  
		customer.customer_collection_status = customer_collection_status.ID
LEFT JOIN  
	invoice_collection_status ON  
		a.woprog_invoice_collection_status = invoice_collection_status.invoice_collection_status_id
INNER join  
	business_unit ON  
		a.business_unit_id = business_unit.id
INNER JOIN  
	address on  
		customer.customer_id=address.address_table_id and address_table = 'Customer' and address_type = 'B' 
LEFT JOIN  
	customer_sales_properties csp ON  
		address.address_id = csp.address_id 
LEFT JOIN  
	member csp_m ON  
		csp.account_manager = csp_m.member_id  
LEFT JOIN
	member projman ON
		a.woprog_pm_memberid = projman.member_id
LEFT JOIN
	contact c ON a.woprog_contact_id = c.contact_id
WHERE
	a.woprog_invoice_paid_date is null AND
	a.WOProg_Status = 'Invoiced' AND
a.WOProg_InvoicedNetTotal > 0 AND
	a.business_unit_id NOT IN (8,36,26) AND
    a.business_unit_id in (" + new Current_User().visible_business_units + ")" + adder,null);
		gv_invoices.DataSource= dt;
		gv_invoices.DataBind();
			//ne_session.snap_collection(_page_id, "master_invoices_gv", ne_session.obj_size(dt), dt.Rows.Count, dt.Columns.Count);
		}




		



	protected void gv_invoices_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void gv_invoices_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		if(e.Parameters != "")
			{
			gv.LoadClientLayout(e.Parameters);
			}
		else
			{
			gv.FilterExpression		= "";
			for(var i = 0; i < gv.Columns.Count; i++)
				{
				if (gv.Columns[i] is GridViewDataColumn)
					{
					var col = (GridViewDataColumn) gv.Columns[i];
					if (col.GroupIndex > -1)
						{
						gv.UnGroup(col);
						}
					col.Visible = true;
					}
				}
			}
		}


	protected void gv_invoicenotes_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		var gv = (ASPxGridView)sender;

		if (e.NewValues["note"] != e.OldValues["note"])
		{
			_tools.getSQL_void(@"Update ar_notes  set ar_notes_memberid =@v0, ar_notes_note =@v1 ,ar_notes_ts =@v2   where ar_notes_id =@v3", new object[] { current_user.id,e.NewValues["note"],System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),e.Keys[0] });
		}
		Session["master_invoices_gv"] = null;
		e.Cancel = true;
		gv.CancelEdit();
	}


	protected void gv_invoicenotes_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		var gv = (ASPxGridView)sender;

		if (e.NewValues["note"] != null)
		{
			_tools.getSQL_void(@"Insert into ar_notes (ar_notes_memberid,ar_notes_note,ar_notes_woprogid,ar_notes_ts)  values (@v0,@v1,@v2,@v3)",new object[] { current_user.id,e.NewValues["note"].ToString(),woprogid_notes,System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") } );
		}
		Session["master_invoices_gv"] = null;
		e.Cancel = true;
		gv.CancelEdit();
	}


	protected void sql_invnotes_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		e.Command.Parameters["@woid"].Value = woprogid_notes;
	}


	protected void gv_invoices_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		try
			{
			woprogid_notes = gv.GetRowValues(gv.EditingRowVisibleIndex, "WOProg_ID").ToString();
			}
		catch
			{
			}
		e.EditForm.Visible = true;


	}


	protected void gv_invoices_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		try
		{

			var status = e.NewValues["customer_collection_status"] == null ? 0 : Convert.ToInt32(e.NewValues["customer_collection_status"]);
			var inv_status = e.NewValues["woprog_invoice_collection_status"] == null ? 0 : Convert.ToInt32(e.NewValues["woprog_invoice_collection_status"]);
			var confirmed = e.NewValues["woprog_ExpectedCheckRun"]==null? "" : Convert.ToDateTime(e.NewValues["woprog_ExpectedCheckRun"]).ToString("yyyy-MM-dd") ;
			var custpo = e.NewValues["WOProg_CustPO"] == null ? "" : e.NewValues["WOProg_CustPO"].ToString();
			
			var wo = new NeWOProg(Convert.ToInt32(e.Keys[0]));
			if(e.NewValues["WOProg_CustPO"] == null)
				{
				custpo				= wo.PONumber;
				}
			var cust = new NECustomer((int) wo.WOProg_Customer_ID)
							{
								customer_collection_status = status,
								Member_ID = current_user.id
							};
			cust.Save();
			var exp="";
			if (confirmed != "")
			{
				exp = ",woprog_ExpectedCheckRun='" + confirmed + "'";
			}
			else
			{

				exp = ",woprog_ExpectedCheckRun=null";
			}

				_tools.getSQL_void(@"update woprog  set woprog_invoice_collection_status=@v0 "+ exp +", WOProg_CustPO=@v1   where woprog_id =@v2", new object[] { inv_status,custpo,e.Keys[0] });
				try
				{
					if (wo.woprog_invoice_collection_status != inv_status)
					{
						var invoice_status = _tools.getSQL_string(@"Select invoice_collection_status_name from invoice_collection_status  where invoice_collection_status_id =@v0", new object[] { inv_status });
						if (invoice_status.Trim() == "")
						{
							_tools.getSQL_void(@"Insert into ar_notes (ar_notes_memberid,ar_notes_note,ar_notes_woprogid,ar_notes_ts)  values (@v0,'Invoice Status Cleared',@v1,@v2)",new object[] { current_user.id,wo.woprog_id,System.DateTime.Now.ToString("yyyy-MM-dd") } );
						}
						else
						{
							_tools.getSQL_void(@"Insert into ar_notes (ar_notes_memberid,ar_notes_note,ar_notes_woprogid,ar_notes_ts)  values (@v0,'Invoice Status changed to@v1,@v2,@v3)",new object[] { current_user.id,invoice_status,wo.woprog_id,System.DateTime.Now.ToString("yyyy-MM-dd") } );
						}
					}
				}
				catch { }

			
			

			Session["master_invoices_gv"] = null;
			fill_grid();
		}
		catch (Exception ee)
		{
			throw (ee);
		}





		e.Cancel = true;
		gv_invoices.CancelEdit();
	}
	protected void gv_invoices_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.VisibleIndex >= 0)
		{
			if (gv.GetRowValues(e.VisibleIndex, "woprog_invoice_collection_status").ToString() == "2")
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFCCCC");
			}
			else if (gv.GetRowValues(e.VisibleIndex, "woprog_invoice_collection_status").ToString() == "5")
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#99CCFF");
			}
			else if (gv.GetRowValues(e.VisibleIndex, "woprog_invoice_collection_status").ToString() == "7")
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
			}
			else if (gv.GetRowValues(e.VisibleIndex, "woprog_invoice_collection_status").ToString() == "3")
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#99FF99");
			}
		}
	}
	protected void btn_apply_selected_Click(object sender, EventArgs e)
	{
	

		
		var selectItems = gv_invoices.GetSelectedFieldValues("WOProg_ID");
		if ((selectItems.Count > 0) && (dte.Date.ToString("yyyy-MM-dd") != "0001-01-01"))
		{
			foreach (var selectItemId in selectItems)
			{
				try
				{
					_tools.getSQL_void(@"update woprog  set woprog_ExpectedCheckRun =@v0  where woprog_id =@v1 limit 1 ", new object[] { dte.Date.ToString("yyyy-MM-dd"),selectItemId });
				
				}
				catch
				{

				}

			}
			dte.Date = new DateTime(1, 1, 1);
			dte.Value = null;
			gv_invoices.Selection.UnselectAll();
			Session["master_invoices_gv"] = null;
			fill_grid();
		}
	}

	
}
