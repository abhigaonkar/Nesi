using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using DevExpress.XtraReports.UI;
using nesi.core;

/// <summary>
/// Summary description for Helper
/// </summary>
public class Helper
	{
	public Helper()
		{
		}
	public class WorkOrder
		{
		private static void SendIsError(Exception ee, string subject = "Invoice Error")
			{
			var em = new NeEMail
				         {
				         To      = "debug@" + Toolbox.app_setting("DomainForEmail"),
				         Subject = "Invoice error: " + subject,
				         Body    = ee.ToString(),
				         From    = "noreply@" + Toolbox.app_setting("DomainForEmail")
            };
			em.Send();
			Toolbox.do_errorLog_errorStack(ee);
			}
		public static object FillReport(InvoicePreview report, NeWOProg this_woprog)
			{
			var _tools = new Toolbox();
			var business_unit_id = this_woprog.business_unit_id;
			var customer_id = this_woprog.WOProg_Customer_ID.ToString();
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
			var total_due = report.FindControl("total_due", true) as XRLabel;
			var footer_text = report.FindControl("footer_text", true) as XRLabel;
			var lbl_sales_tax = report.FindControl("lbl_sales_tax", true) as XRLabel;
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
			var lbl_signoff = report.FindControl("lbl_signoff", true) as XRLabel;
			var xrPictureBox1 = report.FindControl("xrPictureBox1", true) as XRPictureBox;
			//string _table = "current";
			//if ((this_woprog.Status == "Invoiced") || (this_woprog.Status == OpsWOStatus.WaitingToBeInvoiced))
			//    {
			//    _table = "history";
			//    }
			if (lbl_signoff != null)
				{
				lbl_signoff.Visible = false;
				}
			if (newTotal.MaterialPriceSell != 0 && this_woprog.default_invoicetype != 4)
				{
				total_material_lbl.Visible = true;
				total_material.Visible = true;
				total_material.Text = newTotal.MaterialPriceSell.ToString("N2");
				}
			report.Watermark.Text = "";
			lblsi.Text = "";
			invoice_number.Text = this_woprog.woprog_InvoiceNo == "" ? "##########" : this_woprog.woprog_InvoiceNo;
			var this_customer = new NECustomer(Convert.ToInt32(customer_id));
			var this_company = new NeBusinessUnit(business_unit_id);
			customer_number.Text = this_customer.Customer_Number;
			var this_contact = new NEContact(this_woprog.woprog_Contact_ID);
			var parent_company = new NeBusinessUnit(business_unit_id);
			if (this_customer.customer_invoice_address == "" || this_customer.customer_invoice_address == null)
				{
				if (this_customer.Memo != "")
					{
					lblsi.Text = _tools.value_from(this_customer.Memo);
					}
				}
			if (this_woprog.woprog_dayscredit > 0)
				{
				txtterms.Text = "Net " + this_woprog.woprog_dayscredit + " days";
				}
			else
				{
				txtterms.Text = "Due Upon Receipt";
				}

			if (this_company.country == "USA")
				{
				lblremit.Visible = false;
				lblremitlabel.Visible = false;
				}

			if (this_company.remit_to_address != "")
				{
				lblremit.Visible = true;
				lblremit.Text = "";
				foreach (string s in this_company.remit_to_address.Split('|'))
					{
					lblremit.Text += s + @"
";
					}

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
				SendIsError(ee);
				}
			txtContact.Text = this_contact.Contact_Name;
			try
				{
				attn_name.Text = "Accounts Payable";
				}
			catch (Exception ee)
				{
				SendIsError(ee);
				}
			txtDate.Text = this_woprog.woprog_InvoiceDate == Convert.ToDateTime("12/01/2005") ? DateTime.Now.ToString("MM/dd/yyyy") : this_woprog.woprog_InvoiceDate.ToString("MM/dd/yyyy");
			wo_number.Text = this_woprog.OrderNumber;
			po_number.Text = _tools.value_from(this_woprog.PONumber);
			net_amount.Text = newTotal.StillToBeBilled.ToString("N2");
			if (newTotal.StillToBeBilled < 0)
				{
				inv_header_label.Text = "Credit";
				}
			txtlocation.Text = _tools.value_from(this_woprog.woprog_Location_in_plant);
			if (this_woprog.woprog_tax1 != 0 || this_woprog.woprog_tax2 != 0 || this_woprog.woprog_tax3 != 0 || this_woprog.woprog_tax4 != 0)
				{
				lbl_sales_tax.Visible = true;
				sales_tax.Visible = true;
				sales_tax.Text = (newTotal.TAX_LABOUR + newTotal.TAX_MATERIAL).ToString("N2");
				}
			total_due.Text = (newTotal.StillToBeBilled + newTotal.TAX_LABOUR + newTotal.TAX_MATERIAL).ToString("C2").Replace("(", "-").Replace(")", "");
			var this_address = "";
			if (this_customer.Address.Addr1.Length > 0)
				{
				this_address = this_customer.Address.Addr1;
				}
			if (this_customer.Address.Addr2.Length > 0)
				{
				this_address += "\n" + this_customer.Address.Addr2;
				}
			if (this_customer.Address.Addr3.Length > 0)
				{
				this_address += "\n" + this_customer.Address.Addr3;
				}
			if (this_customer.Address.Addr4.Length > 0)
				{
				this_address += "\n" + this_customer.Address.Addr4;
				}
			customer_address.Text = string.Format(@"{0}
{1}
{2}, {3}
{4}",
						this_customer.Customer_Name,
						this_address,
						this_customer.Address.City,
						this_customer.Address.Prov,
						this_customer.Address.Postal
						);//
			var tax_number = "";
			var sales_tax_label = "";
			if (!this_company.isregional_branch)
				{
				var parent_business_unit_id = Toolbox.doSQL_int(@"SELECT id from business_unit WHERE region = @v0  AND isregional_branch = 'True'", new object[] { this_company.region });
				parent_company = new NeBusinessUnit(parent_business_unit_id);
				#region oldcode
				//		footer_text.Text = string.Format(@"
				//REMIT PAYMENTS TO: {0}, {1} {2}, {3}
				//Net 30 Days unless otherwise stated -2% per month charged on Overdue Accounts, 24% per annum",
				//parent_company.address.ToUpper(),
				//parent_company.city.ToUpper(),
				//parent_company.provstate.ToUpper(),
				//parent_company.postal.ToUpper());
				#endregion oldcode
				}
			if (this_company.id == 3)
				{
				sales_tax_label = "Sales Tax:";
				}
			else
				{
				// Is there a tax set up for the address?
				var tax_label = "";
				if (address.Tax1 > 0)
					{
					tax_label = Toolbox.doSQL_string(@"SELECT tax_name FROM tax where tax_id = @v0  LIMIT 1", new object[] { address.Tax1 });
					}
				if (tax_label == "")
					{
					var _dt = _tools.getSQL_datatable(@" SELECT b.tax_name from tax_entity a LEFT JOIN tax b ON a.tax1 = b.tax_id WHERE a.id = @v0  AND a.tax1 != 0 UNION SELECT d.tax_name from tax_entity c LEFT JOIN tax d ON c.tax2 = d.tax_id WHERE c.id = @v0  AND c.tax2 != 0 UNION SELECT f.tax_name from tax_entity e LEFT JOIN tax f ON e.tax3 = f.tax_id WHERE e.id = @v0  AND e.tax3 != 0 UNION SELECT h.tax_name from tax_entity g LEFT JOIN tax h ON g.tax4 = h.tax_id WHERE g.id = @v0  AND g.tax4 != 0", new object[] { parent_company.tax_entity_id });
					foreach (DataRow _dr in _dt.Rows)
						{
						var tax_name = _dr["tax_name"].ToString();
						sales_tax_label += tax_name + "/";
						}
					sales_tax_label = sales_tax_label.TrimEnd('/') + ": ";
					}
				else
					{
					sales_tax_label = tax_label + ": ";
					}
				}
			if (this_woprog.woprog_InvoiceDate <= new DateTime(2016, 12, 31))
				{
				tax_number = "GST#: 89657 3771 RT0001";
				}
			else
				{
				tax_number = "GST#: " + this_company.BusinessNumber.Replace("RP", "RT");
				}
			lbl_sales_tax.Text = sales_tax_label;
			xrPictureBox1.ImageUrl =  Toolbox.app_setting("Domain")+ @"/images/Logos/" + this_company.logo_file;

			var this_TE = new NeTaxEntity(this_company.tax_entity_id);
			branch_info.Text = string.Format(@"{0}
{1}
{2}, {3}
{4}
Tel: {5}
Fax: {6}
Email: ar@newelectric.com
{7}",
				this_TE.public_name,
						this_company.address,
						this_company.city,
						this_company.provstate,
						this_company.postal,
						this_company.PhoneNumber,
						this_company.FaxNumber,
						tax_number
						);
			return report;
			}
		}
	}