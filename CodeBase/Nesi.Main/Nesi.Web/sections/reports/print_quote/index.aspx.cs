using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.IO;
using System.Drawing;
using System.Linq;
using nesi.core;
using nesi.core.print;
using DevExpress.XtraReports.Web;
using NESI.Common.Models;

public partial class print_quote : System.Web.UI.Page
	{
	NeMember _my_member;
	quote _quote;
	NameValueCollection _q;
	int _this_quoteid;
	int _this_quote_id;
	int _this_revision;
	string _this_type = "";
	int _this_signoff;
	protected void Page_Init(object _sender, EventArgs _e)
		{
		_q = Request.QueryString;
		_my_member = Toolbox.do_handle_authentication(OpsPage.Home);
		if (_q["quoteid"] == null || _q["quoteid"].Length < 7)
			{
			Toolbox.FriendlyException(Response, "Invalid Quote # provided", "");
			}
		var quote_id = string.IsNullOrEmpty(_q["quoteid"]) ? "" : _q["quoteid"].Substring(0, 6);
		_this_quoteid = Convert.ToInt32(_q["quoteid"]);
		var canExportRTF = _my_member.AuthenticatedForPrivilege(OpsPrivilege.RtfExporting);
		_this_quote_id = Convert.ToInt32(quote_id);
		_this_revision = string.IsNullOrEmpty(_q["quoteid"]) ? 0 : Convert.ToInt32(_q["quoteid"].Substring(6, _q["quoteid"].Length - 6));
		_this_type = string.IsNullOrEmpty(_q["type"]) ? "" : _q["type"].ToLower();
		int.TryParse(_q["signoff"], out _this_signoff);
		ReportToolbarComboBox saveFormat = new ReportToolbarComboBox(ReportToolbarItemKind.SaveFormat);
		if (canExportRTF)
			{
			saveFormat.Elements.Add(new ListElement("pdf"));
			saveFormat.Elements.Add(new ListElement("rtf"));
			saveFormat.Index = 0;
			}
		else
			{
			saveFormat.Elements.Add(new ListElement("pdf"));
			saveFormat.Index = 0;
			}
		ReportToolbar1.Items.Add(saveFormat);
		Title = $"Quote Print Off - Q#{quote_id}v{_this_revision}";
		}
	protected void Page_Load(object _sender, EventArgs _e)
		{
		var active_rev = quote.GetActiveRevision(_this_quote_id);
		if (active_rev != _this_revision)
			{
			Toolbox.FriendlyException(Response, string.Format("<b>Revision #{1}</b> is not the active revision... Only the active revision (Revision #{0} currently) can be printed. <br/><br/> You can toggle the active revision by clicking the check box next to the revision number in the upper-righthand corner of the quote.", active_rev, _this_revision), "");
			}
		_quote = new quote(_this_quote_id, _this_revision);
		if (_my_member.isContact && _my_member.customerID != _quote.cust_id)
			{
			Toolbox.FriendlyException(Response, "I'm sorry this quote is not cut for your company.", "window.close()");
			}
		if (string.IsNullOrEmpty(_this_type))
			{
			Toolbox.FriendlyException(Response, "Type of quote print off not provided", "");
			}
		bindreport();
		if (!IsPostBack)
			{

			var member = new NeMember(Convert.ToInt32(_quote.quoted_by));
			var csp = new customer_sales_properties((int)_quote.address_id);
			// Is this their first quote being printed and they are suspect?
			// How many quotes have been printed for this customer?
			var n_print = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM quote_master WHERE customer_id = @v0  AND last_print_date < curdate()", new object[] { _quote.cust_id });
			if (csp.status_id == 1 && n_print == 0)
				{
				// First quote printed, and they are suspect.
				csp.status_id = 2;
				csp.save();
				}
			txtEmailAddress.Text = _my_member.NEEmail;
			txtEmailAddress.Enabled = true;
			txtSubject.Text = new NeBusinessUnit(new quote(_this_quote_id).business_unit_id).name + " Quote: " + _this_quote_id + "V" + _this_revision + "  " + _quote.txtJobDescription;
			Session["emailfrom"] = member.NEEmail;
			txtmyemail.Text = _my_member.NEEmail;
			memoBody.Text += "Please review the attached quotation at your earliest convenience and please call without hesitation\n\n";
			}
		}

	private void bindreport()
		{
		var report = new quotemaster();
		report.Name = $"Quote - {_this_quote_id}{_this_revision}";
		report.Parameters[0].Value = _this_quote_id;
		report.Parameters[1].Value = _this_revision;
		report.Parameters[2].Value = _this_type;
		report.Parameters[3].Value = _this_signoff;
		report.remitToAddress.Value = getRemitAddress(_quote.business_unit_id);
		var status_id = quote.GetStatus(_this_quote_id, _this_revision);
		if (_this_type == "draft" && (status_id < 3 || status_id == 6))
			{
			report.Watermark.Text = "DRAFT";
			report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 80);
			report.Watermark.ForeColor = Color.Red;
			report.Watermark.TextTransparency = 200;
			}
		report.PaperKind = System.Drawing.Printing.PaperKind.Letter;
		report.Margins = new System.Drawing.Printing.Margins(25, 25, 25, 25);
		ReportViewer1.Report = report;
		ReportViewer1.DataBind();
		SqlDataSource1.FilterExpression = string.Format("[quote_id] = " + _this_quote_id + " and [revision] = " + _this_revision);
		}


	protected void btnSendEmail_Click(object _sender, EventArgs _e)
		{
		if (txtEmailAddress.Text == "")
			{
			return;
			}
		var report = new quotemaster();
		report.Parameters[0].Value = _this_quote_id;
		report.Parameters[1].Value = _this_revision;
		report.Parameters[2].Value = _this_type;
		report.Parameters[3].Value = _this_signoff;
		report.remitToAddress.Value = this.getRemitAddress(this._quote.business_unit_id);

		var status_id = quote.GetStatus(_this_quote_id, _this_revision);
		if (_this_type == "draft" && (status_id < 3 || status_id == 6))
			{
			report.Watermark.Text = "DRAFT";
			report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 80);
			report.Watermark.ForeColor = Color.Red;
			report.Watermark.TextTransparency = 200;
			}
		var file = new MemoryStream();
		report.CreateDocument(false);
		report.ExportToPdf(file);
		file.Seek(0, SeekOrigin.Begin);
		var attachment = new Attachment(file, "Quote-" + _this_quoteid + ".pdf");

		var email_addresses = txtEmailAddress.Text.Split(',');
		var invalid_emails = "The following email addresses are invalid: ";
		var was_error = false;
		foreach (var add in email_addresses.Where(_add => !Toolbox.CheckEmail(_add)))
			{
			invalid_emails += add + ", ";
			was_error = true;
			}
		if (was_error)
			{
			Page.ClientScript.RegisterStartupScript(GetType(), "invalidEmail", "alert('" + invalid_emails + "');", true);
			//ScriptHandler.Register()
			return;
			}

		if (_this_type != "draft")
			{
			var current_status_id = quote.GetStatus(_this_quote_id, _this_revision);
			if (current_status_id > 2)
				Toolbox.doSQL_void(@"UPDATE quote_master SET last_fax_date = NOW() WHERE quote_id = '{0}' AND status_id NOT IN (8,6)", _this_quote_id);
			else
				Toolbox.doSQL_void(@"UPDATE quote_master SET last_fax_date = NOW(), status_id = 3 WHERE quote_id = @v0  AND revision = @v1 ", new object[] { _this_quote_id, _this_revision });
			}
		var history_text = _this_type == "draft" ? "Draft Emailed" : "Quote Emailed";
		Toolbox.doSQL_void(@" INSERT INTO quote_history ( create_datetime, created_by, quote_id, revision, event ) VALUES ( now(), @v0 , @v1 , @v2 , @v3  )", new object[] { _my_member.id, _this_quote_id, _this_revision, history_text });

		var mail = new NeEMail
			{
			To = txtEmailAddress.Text,
			fromQuote = txtmyemail.Text,
			isHTML = true,
			Subject = txtSubject.Text,
			Body = memoBody.Text,
			Attachment = attachment
			};

		if (txtEmailCC.Text != "")
			{
			mail.CC = txtEmailCC.Text;
			}

		if (txtmyemail.Text != "")
			{
			if (chksendtome.Checked)
				{
				mail.Bcc = txtmyemail.Text;
				}
			}
		mail.Send_Background();

		Page.ClientScript.RegisterStartupScript(GetType(), "invalidEmail", "window.top.location.reload();", true);
		}

	private string getRemitAddress(int businessUnitId)
		{
		return quotemaster.getRemitAddress(businessUnitId);
		}
	}

