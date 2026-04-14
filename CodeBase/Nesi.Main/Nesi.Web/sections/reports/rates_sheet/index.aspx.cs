using System;
using DevExpress.XtraReports.UI;
using System.Collections.Specialized;
using System.Net.Mail;
using System.IO;
using nesi.core;
using nesi.core.print;

public partial class sections_reports_rates_sheet_index : System.Web.UI.Page
{
    NeMember myMember;
    Toolbox _tools = new Toolbox();
    int business_unit_id = 0;
    int customerid = 0;
    int memid = 0;

    protected void Page_Init(object sender, EventArgs e)
    {
        _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(1);
        _tools.dont_cache_page();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        memid = myMember.id32;


     ASPxComboBox1.DataSource = Toolbox.doSQL_dt(@"CALL get_visible_business_units(@v0)",new object[] {  memid});
        ASPxComboBox1.DataBind();


        var _q = Request.QueryString;

    if (!IsPostBack && !IsCallback)
        {
            if (_q["business_unit_id"] != null) {
                business_unit_id = Convert.ToInt32(_q["business_unit_id"]);
                ASPxComboBox1.Value = business_unit_id;
            }
        if (business_unit_id == 0)
            {
            business_unit_id = myMember.business_unit_id;
            ASPxComboBox1.Value = business_unit_id;
            }
        }
    else
        {
        if (_q["customerid"] != null)
            {
                customerid = Convert.ToInt32(_q["customerid"]);
            }

        if (Page.Request.Params.Get("__EVENTTARGET") == null)
            {
                ASPxComboBox1.Value = business_unit_id;
            }
        else
            {
                business_unit_id = Convert.ToInt32(ASPxComboBox1.Value);
            }
        }

    var report = new rpt_rates_sheet(Convert.ToInt32(business_unit_id), customerid);
        RatesSheetPreviewer.Report = (rpt_rates_sheet)fill_report(report);
        RatesSheetPreviewer.DataBind();
    }



    protected object fill_report(rpt_rates_sheet report)
    {
        var _tools = new Toolbox();
        var branch_info = report.FindControl("branch_info", true) as XRLabel;
        var branch_phone = report.FindControl("xrLabel4", true) as XRLabel;
        var this_company = new NeBusinessUnit(business_unit_id);

        branch_info.Text = string.Format(@"{0}
{1}
{2}, {3}
{4}
Tel: {5}
Fax: {6}
",
                this_company.description,
                this_company.address,
                this_company.city,
                this_company.provstate,
                this_company.postal,
                this_company.PhoneNumber,
                this_company.FaxNumber
                );
        branch_phone.Text =  "1-833-775-7697";
        return report;
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        var this_company = new NeBusinessUnit(Convert.ToInt32(business_unit_id));
        txtSubject.Text = this_company.name + " rates on " + System.DateTime.Today.ToString("yyyy-MM-dd");
        if (txtEmailAddress.Text == "")
        {
            return;
        }
        var _tools = new Toolbox();

        var report = new rpt_rates_sheet(Convert.ToInt32(business_unit_id), customerid);

        report = (rpt_rates_sheet)fill_report(report);
        var file = new MemoryStream();
        report.CreateDocument(false);
        report.ExportToPdf(file);
        file.Seek(0, SeekOrigin.Begin);



        var mail = new NeEMail();
        mail.To = txtEmailAddress.Text;
        mail.From = myMember.NEEmail;
        mail.Subject = txtSubject.Text;
        mail.Body = memoBody.Text;


        mail.Attachment = new Attachment(file, this_company.name + " Rate Sheet - " + System.DateTime.Today.ToString("yyyy-MM-dd") + ".pdf");
        if (txtEmailCC.Text != "")
        {
            mail.CC = txtEmailCC.Text;
        }

        if (txtmyemail.Text != "")
        {
            if (chksendtome.Checked)
                mail.Bcc = txtmyemail.Text;
        }
        mail.Send();

    }

    protected void popupemail_ClientLayout(object sender, DevExpress.Web.ASPxClientLayoutArgs e)
    {
        var this_company = new NeBusinessUnit(business_unit_id);
        txtSubject.Text = this_company.name + " rates on " + System.DateTime.Today.ToString("yyyy-MM-dd");

    }
    protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        business_unit_id = Convert.ToInt32(ASPxComboBox1.Value);

        var report = new rpt_rates_sheet(Convert.ToInt32(business_unit_id), customerid);
        RatesSheetPreviewer.Report = (rpt_rates_sheet)fill_report(report);
        RatesSheetPreviewer.DataBind();
    }

}
