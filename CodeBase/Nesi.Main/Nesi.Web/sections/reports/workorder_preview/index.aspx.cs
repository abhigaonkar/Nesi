using System;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Collections.Specialized;
using DevExpress.XtraReports.Web;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Drawing;
using DevExpress.XtraPrinting;
using NESI.Common.Models;
using nesi.core;

public partial class sections_reports_workorder_preview_index : System.Web.UI.Page
{
	int wo_id = 0;
	bool is_signoff = false;
	NeMember currentUser;
	NeWOProg this_woprog;
	NeBusinessUnit WorkOrderBU;
	NameValueCollection _q;
	int memid = 0;
	int breakout = 0;
	bool is_html = false;
	bool is_pdf = false;
	
	protected void Page_Init(object sender, EventArgs e)
	{
		_q = Request.QueryString;
		is_html = !string.IsNullOrEmpty(_q["html"]) && _q["html"] == "1";
		is_pdf = !string.IsNullOrEmpty(_q["pdf"]) && _q["pdf"] == "1";
		is_signoff = !string.IsNullOrEmpty(_q["is_signoff"]) && _q["is_signoff"].ToLower() == "true";

		if (is_signoff && is_html || is_pdf)
		{
			currentUser = Toolbox.do_handle_authentication(12);
		}
		else
		{
			currentUser = Toolbox.do_handle_authentication(12);
			if (!currentUser.AuthenticatedForPrivilege(3))
			{
				Toolbox.FriendlyException(Response, "Access Denied, you need permission to Invoice Print Preview Button", "");
			}
		}
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		breakout = currentUser.isContact ? 0 : Toolbox.ReturnZeroIfNull_int(_q["breakout"]);
		wo_id = _q["id"] == "null" ? 0 : Toolbox.ReturnZeroIfNull_int(_q["id"]);

		if (wo_id == 0)
		{
			Toolbox.FriendlyException(Response, "No work order supplied", "/");
		}



		ReportToolbar1.ClientSideEvents.ItemValueChanged = "function(s,e){please_wait('start');location.href='index.aspx?id=" + wo_id + "&is_signoff=" + is_signoff + "&breakout='+s.GetValue();}";

		if (wo_id > 0)
		{
			//	report			= new InvoicePreview(wo_id);
			this_woprog = new NeWOProg(wo_id);
			WorkOrderBU = new NeBusinessUnit(this_woprog.business_unit_id);
			Title = "Work Order Preview for WO " + this_woprog.OrderNumber.TrimStart('0');
			SqlDataSource1.FilterExpression = string.Format("[woprog_id] = " + wo_id);
			/* If invoice number exists then Invoice or Packing slip 
            if (this_woprog.woprog_InvoiceNo == "")
			    {
			    is_signoff = true;
			    this.Title = "Packing Slip / Acceptance Form Preview for WO " + this_woprog.OrderNumber.TrimStart('0');
            }
            */
			if (currentUser.isContact && !IsPostBack)
			{
				object ob1 = ReportToolbar1.Items[17];
				object ob2 = ReportToolbar1.Items[18];
				object ob3 = ReportToolbar1.Items[19];
				ReportToolbar1.Items.Remove((ReportToolbarButton)ob3);
				ReportToolbar1.Items.Remove((ReportToolbarComboBox)ob2);
				ReportToolbar1.Items.Remove((ReportToolbarButton)ob1);
			}
			else
			{
				//if (!IsPostBack)
				//{
				var rtbi = ReportToolbar1.Items[18];
				if (rtbi.ItemKind == ReportToolbarItemKind.Custom)
				{
					var cb = (ReportToolbarComboBox)rtbi;
					var dt = Toolbox.doSQL_dt(@"Select * from customer_default_invoice_types", null);
					//	int xx=0;
					//	int xxx=0;
					foreach (DataRow dr in dt.Rows)
					{

						var l = new ListElement();
						l.Value = dr["id"].ToString();
						l.Text = dr["invoice_type"].ToString();
						if (l.Value == this_woprog.default_invoicetype.ToString())
						{
							//		xx=xxx;
						}
						//xxx++;
						//		cb.Elements.Add(l);
					}
					//	cb.Index = xx;
				}

				//}
				ReportToolbar1.DataBind();
			}
			var cust = new NECustomer(this_woprog.WOProg_Customer_ID);
			var company = new NeBusinessUnit(this_woprog.business_unit_id);
			if (!IsPostBack)
			{

				txtEmailAddress.Text = "";
			
					if (new NEContact(this_woprog.woprog_Contact_ID).Contact_Email != "")
					{
						txtEmailAddress.Text = new NEContact(this_woprog.woprog_Contact_ID).Contact_Email;
					}
				
				if (!currentUser.isContact)
				{
					txtmyemail.Text = currentUser.NEEmail;
				}
				if (txtEmailAddress.Text == "")
				{
					if (currentUser.isContact)
					{
						txtEmailAddress.Text = currentUser.Email;
						txtmyemail.Text = currentUser.Email;
						Session["emailfrom"] = currentUser.Email;
					}
					else
					{
						txtEmailAddress.Text = currentUser.NEEmail;
						txtmyemail.Text = currentUser.NEEmail;
						Session["emailfrom"] = currentUser.NEEmail;
					}
				}
				txtEmailAddress.Enabled = true;
				if (cust.customer_invoice_address != "")
				{
					txtEmailCC.Text = cust.customer_invoice_ccaddress;
				}

				txtSubject.Text = company.name + " Work Order Acceptance: " + this_woprog.OrderNumber;
				memoBody.Text += "Please review the attached work order preview. \r\nPlease don't hesitate to call us at " +
								 company.PhoneNumber + ".\n\n";
			}

			if (this_woprog.Status == OpsWOStatus.Invoiced || is_signoff || this_woprog.Status == OpsWOStatus.WaitingToBeInvoiced ||
				this_woprog.Status == "Waiting For PO")
			{
				((ReportToolbarButton)ReportToolbar1.Items[2]).Enabled = true;
				((ReportToolbarButton)ReportToolbar1.Items[3]).Enabled = true;
				((ReportToolbarButton)ReportToolbar1.Items[14]).Enabled = true;
				((ReportToolbarButton)ReportToolbar1.Items[15]).Enabled = true;
				if (ReportToolbar1.Items.Count > 17)
				{

					((ReportToolbarButton)ReportToolbar1.Items[17]).Enabled = true;
				}
				if (!IsPostBack)
				{

					//     txtEmailAddress.Text = contact.Contact_Email;

					if (!is_signoff && this_woprog.Status == OpsWOStatus.Invoiced)
					{
						txtSubject.Text = string.Format("{0} Work Order: {1} (WO:{2})", company.name, this_woprog.woprog_InvoiceNo, this_woprog.OrderNumber);
						memoBody.Text = "Please review the attached work order preview. \r\n Please don't hesitate to call us at " + company.PhoneNumber + ".\n\n";
					}
					else if (!is_signoff && this_woprog.Status != OpsWOStatus.Invoiced)
					{
						txtSubject.Text = string.Format("{0} Work Order Preview: {1} (WO:{2})", company.name, this_woprog.woprog_InvoiceNo, this_woprog.OrderNumber);
						memoBody.Text = "Please review the attached work order preview. \r\n Please don't hesitate to call us at " + company.PhoneNumber + ".\n\n";
					}
					if (!string.IsNullOrEmpty(this_woprog.woprog_InvoiceNo))
					{
						if (NeBusinessUnit.GetbuCountry(this_woprog.business_unit_id.ToString()) != "USA")
						{
							//HACK MH:HARDCODED ADDRESS
							memoBody.Text += @"
** NOTE -  New Remit to Address  **

Please remit payment to:
" + new NeTaxEntity(new NeBusinessUnit(this_woprog.business_unit_id).tax_entity_id).public_name + @"
1345 Heine Court,
Burlington ON 
L7L 6A7";
						}
						if (!string.IsNullOrEmpty(WorkOrderBU.remit_to_address))
						{
							memoBody.Text += @"\nPlease remit payment to: \n" + WorkOrderBU.remit_to_address.Replace("|", "\n") + "\n\n";
						}
						if (this_woprog.business_unit_id == 45)
						{
							memoBody.Text += @"\n** Please note name change to New Electric Fresno LLC  **\n\n";
						}
					}

				}
				//			report.Name = "Invoice " + this_woprog.woprog_InvoiceNo.ToString();
				//			report_title.Text = "Invoice " + this_woprog.woprog_InvoiceNo.ToString();
			}
			else
			{
				if (currentUser.business_unit_id == 11 || this_woprog.Status == OpsWOStatus.WaitingToBeInvoiced || this_woprog.Status == "Waiting For PO")
				{
					((ReportToolbarButton)ReportToolbar1.Items[2]).Enabled = true;
					((ReportToolbarButton)ReportToolbar1.Items[3]).Enabled = true;
					((ReportToolbarButton)ReportToolbar1.Items[14]).Enabled = true;
					((ReportToolbarButton)ReportToolbar1.Items[15]).Enabled = true;
					((ReportToolbarButton)ReportToolbar1.Items[17]).Enabled = true;
				}
				else
				{
					if (ReportToolbar1.Items.Count > 17)
					{
                        // Enable the email button
						// ((ReportToolbarButton)ReportToolbar1.Items[17]).Enabled = false; 
					}
				}
				//			report.Name = this_woprog.OrderNumber + " Preview";
				//			report_title.Text = this_woprog.OrderNumber + " Preview";
			}
			if (!IsPostBack)
			{
				memoBody.Text += "Best Regards,\n";
				memoBody.Text += currentUser.FullName + "\n";
				memoBody.Text += company.description + "\n";
				memoBody.Text += string.Format("{0}-{1}-{2}\n\n", company.PhoneArea, company.PhoneFirst, company.PhoneLast);
			}
			breakout = string.IsNullOrEmpty(_q["breakout"]) ? this_woprog.default_invoicetype : breakout;
			var report = new WorkOrderPreview(wo_id, breakout);
			InvoicePreviewer.Report = (WorkOrderPreview)fill_report(report);
			InvoicePreviewer.Report.Name = "Work Order Preview for WO " + wo_id;

			InvoicePreviewer.DataBind();
			if (is_html)
			{
				Response.Clear();
				report.ExportOptions.Html.ExportMode = DevExpress.XtraPrinting.HtmlExportMode.SingleFilePageByPage;
				report.ExportOptions.Html.EmbedImagesInHTML = true;
				report.ExportOptions.Html.ExportWatermarks = true;
				var stream = new MemoryStream();
				report.ExportToHtml(stream);
				stream.Position = 0;
				var html = "";


				using (var reader = new StreamReader(stream, Encoding.UTF8))
				{
					html = reader.ReadToEnd();
				}
				var zoom = .9;

				if (Page.Request.Browser.IsMobileDevice)
				{
					zoom = 0.39;
				}

				html = html.Replace("<style type=\"text/css\">", string.Format(@"<style type='text/css'> html{{ height: 100%; }} body {{height:100%; -ms-zoom: {0};-moz-transform:scale({0});-moz-transform-origin:0 0;-o-transform:scale({0});-o-transform-origin:0 0;-webkit-transform	: scale({0});-webkit-transform-origin: 0 0; }} ", zoom));
				Response.Write(html);
				Response.End();

			}
			if (is_pdf)
			{
				company = new NeBusinessUnit(this_woprog.business_unit_id);
				var fileServer = NeTaxEntity.BaseFolder(this_woprog.business_unit_id, false);
				var pdfOptions = report.ExportOptions.Pdf;
				pdfOptions.Compressed = true;
				pdfOptions.ImageQuality = PdfJpegImageQuality.High;
				report.ExportToPdf(string.Format(@"{0}\wos\{1}.pdf", fileServer, this_woprog.woprog_id), pdfOptions);
				Toolbox.QuickReponse(Response, "SUCCESS");
			}
			InvoicePreviewer.ClientSideEvents.PageLoad = "function(s,e){SetIndex(" + breakout + ");}";

		}
		else
		{
			throw new Exception("Invalid Work Order ID");
		}

	}
	protected object fill_report(WorkOrderPreview report)
	{
		var business_unit_id = this_woprog.business_unit_id;
		var customer_id = this_woprog.WOProg_Customer_ID;

		var newTotal = new NeSalesOrder();
		var success = newTotal.getSalesOrderValues(this_woprog.woprog_id.ToString());

		var branch_info = report.FindControl("branch_info", true) as XRLabel;
		var invoice_number = report.FindControl("invoice_number", true) as XRLabel;
		var txtDate = report.FindControl("txtDate", true) as XRLabel;
		var attn_name = report.FindControl("attn_name", true) as XRLabel;
		var txtContact = report.FindControl("txtContact", true) as XRLabel;
		var customer_address = report.FindControl("customer_address", true) as XRLabel;
		var customer_number = report.FindControl("customer_number", true) as XRLabel;
		var wo_number = report.FindControl("wo_number", true) as XRLabel;
		var po_number = report.FindControl("po_number", true) as XRLabel;
		var net_amount = report.FindControl("net_amount", true) as XRLabel;
		var sales_tax = report.FindControl("sales_tax", true) as XRLabel;
		var sales_tax2 = report.FindControl("sales_tax2", true) as XRLabel;
		var total_due = report.FindControl("total_due", true) as XRLabel;
		var footer_text = report.FindControl("footer_text", true) as XRLabel;
		var lbl_sales_tax = report.FindControl("lbl_sales_tax", true) as XRLabel;
		var lbl_sales_tax2 = report.FindControl("lbl_sales_tax2", true) as XRLabel;
		var lbl_total_due = report.FindControl("lbl_total_due", true) as XRLabel;
		var total_material_lbl = report.FindControl("total_material_lbl", true) as XRLabel;
		var total_material = report.FindControl("total_material", true) as XRLabel;
		var service_address = report.FindControl("service_address", true) as XRLabel;
		var txtlocation = report.FindControl("txtlocation", true) as XRLabel;
		var lblremitlabel = report.FindControl("lblremitlabel", true) as XRLabel;
		var lblremit = report.FindControl("lblremit", true) as XRLabel;
		var txtterms = report.FindControl("txtterms", true) as XRLabel;
		var lblsi = report.FindControl("lblsi", true) as XRLabel;
		var inv_header_label = report.FindControl("xrLabel10", true) as XRLabel;
		var lbl_net_amount = report.FindControl("lbl_net_amount", true) as XRLabel;
		var line_amt = report.FindControl("line_amt", true) as XRLabel;
		var overdue_clause = report.FindControl("xrLabel3", true) as XRLabel;
		var lbl_signoff = report.FindControl("lbl_signoff", true) as XRLabel;
		var pb_signature = report.FindControl("xrSignatureBox", true) as XRPictureBox;
		var xrPictureBox1 = report.FindControl("xrPictureBox1", true) as XRPictureBox;
		/* label color change */
		var lblHeader1 = report.FindControl("xrlabel9", true) as XRLabel;
		var lblHeader2 = report.FindControl("xrlabel8", true) as XRLabel;
		var lblHeader3 = report.FindControl("xrlabel7", true) as XRLabel;
        var lbltax_not_included = report.FindControl("lbl_tax_not_included", true) as XRLabel;
        // Check for signature
        var c_signature = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM signature WHERE `table`='woprog' AND table_id = @v0 ", new object[] { this_woprog.woprog_id });
		if (c_signature > 0)
		{
			// Has signature, get latest one
			var signature_id = Toolbox.doSQL_int(@"SELECT id FROM signature WHERE `table`='woprog' AND table_id = @v0  ORDER BY id DESC LIMIT 1", new object[] { this_woprog.woprog_id });
			var s = new shared.signature(signature_id);
			var ms = new MemoryStream(s.graphic, 0, s.graphic.Length);
			ms.Write(s.graphic, 0, s.graphic.Length);
			pb_signature.Image = Image.FromStream(ms, true);
			pb_signature.Visible = true;
		}
		var lblattn = report.FindControl("attn", true) as XRLabel;
		var lbl_work_completed = report.FindControl("lbl_work_completed", true) as XRLabel;
		var lbl_pb_notes = report.FindControl("lbl_pb_notes", true) as XRTable;
		//string _table = "current";
		//if ((this_woprog.Status == OpsWOStatus.Invoiced) || (this_woprog.Status == OpsWOStatus.WaitingToBeInvoiced))
		//    {
		//    _table = "history";
		//    }

		if (newTotal.MaterialPriceSell != 0 && breakout != 4)
		{
			total_material_lbl.Visible = true;
			total_material.Visible = true;
			total_material.Text = newTotal.MaterialPriceSell.ToString("N2");
		}
		if (this_woprog.woprog_InvoiceNo != "" || is_signoff)
		{
			// report.Watermark.Text = ""; Always show watermark
            // lbltax_not_included.Text = "";
		    invoice_number.Text = "Not Applicable";
		}
		else
		{
			inv_header_label.Text = "Work Order Preview";

		}
		var this_customer = new NECustomer(customer_id);
		var this_company = new NeBusinessUnit(business_unit_id);
		customer_number.Text = this_customer.Customer_Number;
		var this_contact = new NEContact(this_woprog.woprog_Contact_ID);
		var parent_company = new NeBusinessUnit(business_unit_id);
		

		/*change header color based on BU */
		lblHeader1.BackColor = System.Drawing.ColorTranslator.FromHtml(this_company.header_color);
		lblHeader2.BackColor = System.Drawing.ColorTranslator.FromHtml(this_company.header_color);
		lblHeader3.BackColor = System.Drawing.ColorTranslator.FromHtml(this_company.header_color);


		lblsi.Text = "";
		// this is by design to only allow special instructions to appear if there is no invoice email address for the customer
		if (string.IsNullOrEmpty(this_customer.customer_invoice_address))
		{
			if (this_customer.Memo != "")
			{
				lblsi.Text = Server.UrlDecode(this_customer.Memo);
			}
		}

		
		var term = new NeAccounting.Terms(this_woprog.term_id);
		txtterms.Text = term.Description;

		if (this_company.country == "USA")
		{
			lblremit.Visible = false;
			lblremitlabel.Visible = false;
		}

		//      lblremit.Visible = false;
		//     lblremitlabel.Visible = false;

		if (!string.IsNullOrEmpty(this_company.remit_to_address))
		{
			lblremit.Visible = true;
			lblremit.Text =  WorkOrderBU.remit_to_address.Replace("|", "\n") + "\n\n";

			lblremitlabel.Visible = true;
		}

		var address = new NEAddress();
		try
		{
			if (this_woprog.woprog_Address_ID != this_customer.Address.id)
			{
				address = new NEAddress(this_woprog.woprog_Address_ID);
				var this_serviceaddress = "Service Address: \n";
				if (address.Addr1.Length > 0)
				{
					this_serviceaddress += address.Addr1;
				}
				if (address.Addr2.Length > 0)
				{
					this_serviceaddress += "\n" + address.Addr2;
				}
				if (address.Addr3.Length > 0)
				{
					this_serviceaddress += "\n" + address.Addr3;
				}
				if (address.Addr4.Length > 0)
				{
					this_serviceaddress += "\n" + address.Addr4;
				}
				service_address.Text = string.Format(@"{0}
{1},{2},{3}",
							this_serviceaddress,
							address.City,
							address.Prov,
							address.Postal
							);
			}
		}
		catch (Exception ee)
		{
			Toolbox.do_errorLog_errorStack(ee);
		}

		txtContact.Text = this_contact.Contact_Name;
		try
		{
			attn_name.Text = "Accounts Payable";
		}
		catch (Exception ee)
		{
			Toolbox.do_errorLog_errorStack(ee);
		}
		if (is_signoff)
		{
			attn_name.Text = "";
			lblattn.Text = "";
			inv_header_label.Text = "Work Order Preview";
			var work_completed = "";
			if (this_woprog.QuoteID != "0")
			{
				var dt = Toolbox.doSQL_dt(@"Call wo_signoffsheet_work_completed(@v0)", new object[] { this_woprog.woprog_id });
				if (dt.Rows.Count > 0)
				{
					work_completed = "Work Completed:" + System.Environment.NewLine;
					var x = 1;
					foreach (DataRow dr in dt.Rows)
					{
						work_completed += x + ". " + dr[0] + System.Environment.NewLine;
						x++;
					}
				}
			}
			if (lbl_work_completed != null)
			{
				lbl_work_completed.Text = work_completed;
			}
		}
		txtDate.Text = this_woprog.woprog_InvoiceNo == "" ? DateTime.Now.ToString("yyyy-MM-dd") : this_woprog.woprog_InvoiceDate.ToString("yyyy-MM-dd");
		wo_number.Text = this_woprog.OrderNumber;
		po_number.Text = this_woprog.PONumber;
        //	net_amount.Text = newTotal.StillToBeBilled.ToString("N2");    
        net_amount.Text = Toolbox.doSQL_string(@"SELECT IF(WOProg_InvoicedNetTotal=0 OR WOProg_InvoicedNetTotal=NULL, WOProg_StillToBeBilled , WOProg_InvoicedNetTotal) FROM woprog WHERE woprog_id= @v0", new object[] { this_woprog.woprog_id });
		if (this_woprog.woprog_StillToBeBilled <= 0)
		{
			inv_header_label.Text = "Credit";
		}
		txtlocation.Text = this_woprog.woprog_Location_in_plant;
        // Display Total due if its an old invoiced wo
        var netsuite_id = Toolbox.doSQL_string(@"SELECT netsuite_invoice_internal_id FROM neintranet.woprog WHERE woprog_id =@v0", new object[] { this_woprog.woprog_id });
        var tax1 = new NeTax(this_woprog.woprog_tax1);
        var tax2 = new NeTax(this_woprog.woprog_tax2);
        var tax3 = new NeTax(this_woprog.woprog_tax3);
        var tax4 = new NeTax(this_woprog.woprog_tax4);
        var sales_tax_label = "";
        if (this_woprog.Status == OpsWOStatus.Invoiced && String.IsNullOrEmpty(netsuite_id))
        {
            bool tax2exists = false;
            if (this_woprog.woprog_tax1 != 0)
            {
                lbl_sales_tax.Visible = true;
                sales_tax.Visible = true;
                //   var thisTax1 = Toolbox.doSQL_double(@"SELECT tax_percentage FROM tax WHERE tax_id = @v0", new object[] { this_woprog.woprog_tax1 });
                //   var thisTotalTax1 = (newTotal.THIS_MATERIALPRICESELL * thisTax1 + newTotal.THIS_LABORPRICESELL * thisTax1)/100;
               
            }
            if (this_woprog.woprog_tax2 != 0)
            {
                tax2exists = true;
                lbl_sales_tax2.Visible = true;
             //   sales_tax2.Visible = true;
               // var thisTax2 = Toolbox.doSQL_double(@"SELECT tax_percentage FROM tax WHERE tax_id = @v0", new object[] { this_woprog.woprog_tax2 });
            //    var thisTotalTax2 = Toolbox.doSQL_double(@"SELECT woprog_invoice_tax FROM woprog WHERE woprog_id = @v0", new object[] { this_woprog.woprog_id });
           //     sales_tax2.Text = thisTotalTax2.ToString("N2");
            }
            
            if (this_company.id == 3)
                	{
                		sales_tax_label = "Sales Tax:";
                	}
                	else
                	{
               //  Is there a tax set up for the address?
                		lbl_sales_tax.Text = tax2exists?  "Total (" + tax1.Name+tax2.Name + "):" : "Total " + tax1.Name + ":" ;
                //  if (tax2exists) { lbl_sales_tax2.Text = "Total " + tax2.Name + ":"; }

                sales_tax.Text = Toolbox.doSQL_string(@"SELECT woprog_invoice_tax FROM woprog WHERE woprog_id = @v0", new object[] { this_woprog.woprog_id });
                        
                		
                	}
            var totaldue= Toolbox.doSQL_string(@"SELECT woprog_invoicebalance FROM woprog WHERE woprog_id = @v0", new object[] { this_woprog.woprog_id });
            total_due.Text = (this_woprog.woprog_invoice_tax + this_woprog.net_total).ToString("C2");
            //  total_due.Text = (newTotal.StillToBeBilled + newTotal.TAX_LABOUR + newTotal.TAX_MATERIAL).ToString("C2").Replace("(", "-").Replace(")", "");
        }
        else
        {
            total_due.Text = "";
        }
        var this_address = "";
        if (!string.IsNullOrEmpty(this_customer.Address.Addr1))
        {
            if (this_customer.Address.Addr1.Length > 0)
            {
                this_address = this_customer.Address.Addr1;
            }
        }
        if (!string.IsNullOrEmpty(this_customer.Address.Addr2))
        {
            if (this_customer.Address.Addr2.Length > 0)
            {
                this_address += "\n" + this_customer.Address.Addr2;
            }
        }
        if (!string.IsNullOrEmpty(this_customer.Address.Addr3))
        {
            if (this_customer.Address.Addr3.Length > 0)
            {
                this_address += "\n" + this_customer.Address.Addr3;
            }
        }
        if (!string.IsNullOrEmpty(this_customer.Address.Addr4))
        {
            if (this_customer.Address.Addr4.Length > 0)
            {
                this_address += "\n" + this_customer.Address.Addr4;
            }
        }
        customer_address.Text = string.Format(@"{0}
{1}
{2}, {3}
{4}",
					this_woprog.CustomerName,
					this_address,
					this_customer.Address.City,
					this_customer.Address.Prov,
					this_customer.Address.Postal
					);
		var tax_number = "";
	
		lbl_pb_notes.Text = "";
		if (this_woprog.woprog_associate_woprog_id != 0)
		{
			var cut_dt = Toolbox.doSQL_string(@"Select woprog_cutdatetime from woprog  where woprog_id =@v0", new object[] { this_woprog.woprog_id });
			// pull up all the progress bill invoices and look for the quote line items on each one...
			var dt = Toolbox.doSQL_dt(@" SELECT a.description, a.qty_committed * a.price_sell AS billed, b.woprog_bvwo wo, a.date_added, IFNULL(b.woprog_invoiceno,'####') invoice FROM wo_detail a LEFT JOIN woprog b ON a.woprog_id = b.WOProg_ID  WHERE a.master_id = 2139 AND a.billtypeid IN (3,11) AND a.date_added <@v0 and b.woprog_id !=@v1  AND b.woprog_associate_woprog_id =@v2  ORDER BY wo ", new object[] { cut_dt, this_woprog.woprog_id, this_woprog.woprog_associate_woprog_id });
			if (dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					lbl_pb_notes.Text += "\n" + dr["description"] + " Invoice: " + dr["invoice"] + " - " + dr["wo"];
				}
			}
			var dt1 = Toolbox.doSQL_dt(@" SELECT a.description, a.qty_committed * a.price_sell AS billed, b.woprog_bvwo wo, a.date_added, IFNULL(b.woprog_invoiceno,'####') invoice FROM wo_detailh a LEFT JOIN woprog b ON a.woprog_id = b.woprog_id  WHERE a.master_id = 2139 AND a.billtypeid IN (9,12) AND a.date_added <@v0 and b.woprog_id !=@v1  AND b.woprog_associate_woprog_id =@v2  ORDER BY wo ", new object[] { cut_dt, this_woprog.woprog_id, this_woprog.woprog_associate_woprog_id });
			if (dt1.Rows.Count > 0)
			{
				foreach (DataRow dr in dt1.Rows)
				{
					var xrtr = new XRTableRow();
					var xrtc = new XRTableCell();
					xrtc.Text = dr["description"] + " Invoice: " + dr["invoice"] + " - " + dr["wo"];
					xrtr.Cells.Add(xrtc);
					lbl_pb_notes.Rows.Add(xrtr);
				}
			}
			if (lbl_pb_notes.Text != "")
			{
				lbl_pb_notes.Text = "Other Progress Billings On This Project:\n" + lbl_pb_notes.Text;
			}
		}

   
		if (this_company.country == "CDN" && this_company.BusinessNumber != null)
		{
			if (this_woprog.woprog_InvoiceDate <= new DateTime(2016, 12, 31) && this_woprog.woprog_InvoiceDate.Year > 1969)
			{
				tax_number = "GST#: 89657 3771 RT0001";
			}
			else
			{
				tax_number = "GST#: " + this_company.BusinessNumber.Replace("RP", "RT");
			}
		}
		xrPictureBox1.ImageUrl =Toolbox.app_setting("Domain") + @"/images/Logos/" + this_company.logo_file;

		var this_TE = new NeTaxEntity(this_company.tax_entity_id);

		branch_info.Text = string.Format(@"{0}
{1}
{2}, {3}
{4}
Tel: {5}
Fax: {6}
Email: ar@{8}
{7}",
					//	this_company.description, // Changed it to Tax Entiy Public Name
					this_TE.public_name,
					this_company.address,
					this_company.city,
					this_company.provstate,
					this_company.postal,
					this_company.PhoneNumber,
					this_company.FaxNumber,
					tax_number,
					this_company.web_domain // change for diffent business unit domain can used for email
                                                      //Toolbox.app_setting("DomainForEmail")
                    );
		if (is_signoff)
		{
			#region signoff
			inv_header_label.Text = "Packing Slip / Acceptance Form";
			report.Watermark.Text = "Work estimate - \r\n not an invoice";
			lbl_total_due.Visible = false;
			lbl_sales_tax.Visible = false;
			total_material_lbl.Visible = false;
			net_amount.Visible = false;
			if (!IsPostBack && false)// Doing this as a temporary stop gap.
			{
				var BrokenOut = ReportToolbar1.Items[18];
				var _t = BrokenOut.GetType();
				if (_t.ToString().Contains("ReportToolbarComboBox"))
				{
					ReportToolbar1.Items.Remove(BrokenOut);
				}
				var default_button = ReportToolbar1.Items[18];
				_t = default_button.GetType();
				if (_t.ToString().Contains("Button"))
				{
					ReportToolbar1.Items.Remove(default_button);
				}
			}
			//ReportToolbarComboBox BrokenOut = ;
			//ReportToolbar1.Items.Remove(BrokenOut);
			total_due.Visible = false;
			total_material.Visible = false;
			sales_tax.Visible = false;
			lbl_net_amount.Visible = false;
			line_amt.Visible = false;
			overdue_clause.Visible = false;
			invoice_number.Text = "Not Applicable";
			var pm = new NeMember();
			if (this_woprog.intProjectManager != 0)
			{
				pm = new NeMember(this_woprog.intProjectManager);
			}
			lblremitlabel.Text = "Your " + this_company.name + " Contacts are: ";
			lblremit.WordWrap = false;
			lblremit.Text = string.Format("Account Manager: {0}\n ( {1} )", this_company.branch_manager.FullName2, this_company.branch_manager.NEEmail);
			if (pm != null && pm.Status == "Active" && pm.id != this_company.branch_manager.id)
			{
				lblremit.Text += string.Format("\n\n{0}:{1}\n ( {2} )", pm.Type, pm.FullName2, pm.NEEmail);
			}
			#endregion signoff
		}
		else
		{
			lbl_signoff.Visible = false;
		}

		var test = report.FindControl("xrPanel1", true) as XRPanel;
		test.Borders = BorderSide.None;

		// If Invoice does not have invoice number, then show watermark
		if (!is_signoff && this_woprog.woprog_InvoiceNo == "")
		{
			invoice_number.Text = "Not Applicable";
			report.Watermark.Text = "Work estimate - \r\n not an invoice";
			report.Visible = true;

		}

		//var test1 = report.FindControl("branch_info", true) as XRLabel;
		//    test1.CanGrow = false;
		//var test2 = report.FindControl("xrPictureBox1", true) as XRPictureBox;
		//    test2.Visible = false;

		var test3 = report.FindControl("xrCrossBandBox1", true) as XRCrossBandBox;
        report.Watermark.Text = "Work estimate - \r\n not an invoice";
        test3.Borders = BorderSide.None;


		return report;
	}

	protected void btnSendEmail_Click(object sender, EventArgs e)
	{
		var this_company = new NeBusinessUnit(this_woprog.business_unit_id);
		if (txtEmailAddress.Text == "")
		{
			return;
		}

		this_woprog = new NeWOProg(wo_id);
		
		/*
        if (this_woprog.woprog_InvoiceNo == "")
	        {
	        is_signoff = true;
	       
        }
        */
		var report = new WorkOrderPreview(wo_id, breakout);
		report = (WorkOrderPreview)fill_report(report);
		var file = new MemoryStream();
		report.CreateDocument(false);
		report.ExportToPdf(file);
		
		file.Seek(0, SeekOrigin.Begin);



		var mail = new NeEMail();
		//    mail.To = "aketelaars@newelectric.com";
		mail.To = txtEmailAddress.Text;
		mail.isHTML = true;
		mail.From = "svc_sparkops_smtp_prd@sparkpower.onmicrosoft.com";
		/*mail.From = "AR@" + this_company.web_domain;*/// change for diffent business unit domain can used for email
		mail.Subject = txtSubject.Text;
		mail.Body = memoBody.Text.Replace("\n", "<br/>\n");

		var _type = "Work Order Preview Emailed";
		if (this_woprog.woprog_InvoiceNo != "" && !is_signoff)
		{
			report.Watermark.Text = "";
			mail.Attachment = new Attachment(file, "workorder: " + this_woprog.woprog_InvoiceNo + ".pdf");

		}
		else if (is_signoff)
		{
			report.Watermark.Text = "Work estimate - \r\n not an invoice";
			mail.Attachment = new Attachment(file, "Work_Order_Acceptance: " + this_woprog.OrderNumber + ".pdf");
			_type = "Work Order Acceptance Emailed";
		}
		else
		{
			report.Watermark.Text = "Work estimate - \r\n not an invoice";
			if (is_signoff)
			{
				mail.Attachment = new Attachment(file, "Work_Order_Acceptance: " + this_woprog.OrderNumber + ".pdf");
				_type = "Work Order Acceptance Emailed";
			}
			else
			{
				mail.Attachment = new Attachment(file, "Work Order Preview: " + this_woprog.OrderNumber + ".pdf");

			}

		}
		if (txtEmailCC.Text.Trim() != "")
		{
			mail.CC = txtEmailCC.Text;
		}

		if (txtmyemail.Text != "")
		{
			if (chksendtome.Checked)
				mail.Bcc = txtmyemail.Text;
		}
		mail.Send();

		Toolbox.doSQL_void(@" INSERT INTO collection_history ( date, woprog_id, member_id, action ) VALUES ( now(), @v1 , @v0 , @v2  )", new object[] { currentUser.id, wo_id, _type });                        // end void


	}
	protected void cb_default_invoice_Callback(object source, DevExpress.Web.CallbackEventArgs e)
	{
		this_woprog.default_invoicetype = Convert.ToInt32(e.Parameter);
		this_woprog.SaveWorkOrder();
		e.Result = "SUCCESS";
	}


}
