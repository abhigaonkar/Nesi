using nesi.core;
using NESI.Common.Models;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;

namespace Nesi.Web.sections.reports.invoice_preview
	{
	public partial class invoice_ns : System.Web.UI.Page
    {
        int wo_id =0;
        NeMember currentUser;
        NeWOProg this_woprog;
        NeBusinessUnit WorkOrderBU;
        NameValueCollection _q;     
        bool is_credit;    
      
        protected void Page_Init(object sender, EventArgs e)
        {
            _q = Request.QueryString;
           wo_id = _q["id"] == "null" ? 0 : Toolbox.ReturnZeroIfNull_int(_q["id"]);
            is_credit = !string.IsNullOrEmpty(_q["is_credit"]) && _q["is_credit"] == "1";
            
                currentUser = Toolbox.do_handle_authentication(OpsPage.WorkOrders);
         
            if (!is_credit)
            {
                currentUser = Toolbox.do_handle_authentication(OpsPage.WorkOrders);
                if (!currentUser.AuthenticatedForPrivilege(OpsPrivilege.InvoicePrintPreviewButton))
                {
                    Toolbox.FriendlyException(Response, "Access Denied, you need permission to Invoice Print Preview Button", "");
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this_woprog = new NeWOProg(wo_id);
            WorkOrderBU = new NeBusinessUnit(this_woprog.business_unit_id);
            if (Toolbox.Contains(this_woprog.Status, new [] {	OpsWOStatus.Invoiced, 
																OpsWOStatus.WaitingToBeInvoiced, 
																OpsWOStatus.WaitingForPO
																}) || currentUser.business_unit.is_backoffice)
            {
                email_invoice.Enabled = true;
            }
            if (wo_id == 0)
            {
                Toolbox.FriendlyException(Response, "No work order supplied", "/");
            }
            if (!NeWOProg.CheckForPdf(wo_id))
            {
                Toolbox.FriendlyException(Response, "PDF does not exist for this Work Order", "/");
            }
            if (wo_id > 0)
            {            
                iframe_invoice.Attributes["src"] = string.Format("/sections/reports/invoice_preview/invoice.ashx?id={0}&is_credit={1}", wo_id, is_credit);              
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
                   
                        txtEmailAddress.Text = currentUser.NEEmail;
                        txtmyemail.Text = currentUser.NEEmail;
                        Session["emailfrom"] = currentUser.NEEmail;
                    
                }
                txtEmailAddress.Enabled = true;           
                if (cust.customer_invoice_address != "")
                {
                    txtEmailCC.Text = cust.customer_invoice_ccaddress;
                }

                txtSubject.Text = company.name + " Work Order Acceptance: " + this_woprog.OrderNumber;
                memoBody.Text += "Please review the attached acceptance form. Please don't hesitate to call us at " +
                                 company.PhoneNumber + ".\n\n";
                if (!IsPostBack)
                {
                    memoBody.Text += "Best Regards,\n";
                    memoBody.Text += currentUser.FullName + "\n";
                    memoBody.Text += company.description + "\n";
                    memoBody.Text += string.Format("{0}-{1}-{2}\n\n", company.PhoneArea, company.PhoneFirst, company.PhoneLast);
                }
            }
        }
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            if (txtEmailAddress.Text == "")
            {
                return;
            }
            this_woprog = new NeWOProg(wo_id);
            var mail = new NeEMail
            {
                To = txtEmailAddress.Text,
                isHTML = true,
                From = currentUser.NEEmail,
                Subject = txtSubject.Text,
                Body = memoBody.Text.Replace("\n", "<br/>\n")
            };

            var _type = "Invoice Emailed";
            var pdf = NeWOProg.GeneratePDF(wo_id, is_credit);
            if(pdf == null)
            {
                return;
            }
            var file = new MemoryStream(pdf);
            
            if (this_woprog.woprog_InvoiceNo != "")
            {
              mail.Attachment = new Attachment(file, "Invoice: " + this_woprog.woprog_InvoiceNo + ".pdf");
            }
            else
            {
             mail.Attachment = new Attachment(file, "Preview Invoice: " + this_woprog.woprog_InvoiceNo + ".pdf");                       
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
    }
}