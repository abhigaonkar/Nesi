using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.IO;
using System.Drawing;
using System.Linq;
using nesi.core;
using nesi.core.print;
using DevExpress.XtraReports.UI;

public partial class print_employment_record : System.Web.UI.Page
	{
	NeMember _my_member;
	
	NameValueCollection _q;
	Toolbox _tools;
	
	protected void Page_Init(object _sender, EventArgs _e)
		{
		_tools = new Toolbox();
		_q = Request.QueryString;
		_my_member = Toolbox.do_handle_authentication(1);
		_tools.dont_cache_page();
		
		}
    protected void Page_Load(object _sender, EventArgs _e)
    {

        bindreport();
     
    }

	private void bindreport()
		{
        NeMember m = new NeMember(Convert.ToInt32(_q["id"]));
        bool _details = _q["_details"]== null?false: Convert.ToBoolean(_q["_details"]);
        var report = new employment_record(m.id32, _details);

        //		report.Parameters[0].Value = _this_quote_id;
        //		report.Parameters[1].Value = _this_revision;
        //		report.Parameters[2].Value = _this_type;
        //		report.Parameters[3].Value = _this_signoff;

        XRSubreport sub_details = report.FindControl("xrSubreport1", true) as XRSubreport;
        XRPictureBox pb_logo = report.FindControl("pb_logo",true) as XRPictureBox;
        XRLabel lbl_date = report.FindControl("lbl_date", true) as XRLabel;
        XRLabel lbl_body = report.FindControl("lbl_body", true) as XRLabel;
        XRLabel lbl_conclusion = report.FindControl("lbl_conclusion", true) as XRLabel;
        XRLabel lbl_payroll_admin = report.FindControl("lbl_payroll_admin", true) as XRLabel;
        XRLabel lbl_title = report.FindControl("lbl_title", true) as XRLabel;
        XRLabel lbl_email = report.FindControl("lbl_email", true) as XRLabel;
        XRLabel lbl_company = report.FindControl("lbl_company", true) as XRLabel;
        XRLabel lbl_address = report.FindControl("lbl_address", true) as XRLabel;

        
        string str_pt = m.paytype_id == 1 ? "hourly wage" : m.paytype_id == 4 ? "contract rate" : "salary";
        int _salary = 0;
        if (m.paytype_id != 1 && m.paytype_id != 4)
        {
            var wage = new NeWage(m.id32).current_wage*2080;
             _salary = (int)(((double)wage + (0.5 * 5000)) / 5000) * 5000;
        }

        string str_rate = m.paytype_id == 1 ? new NeWage(m.id32).current_wage.ToString("C2") + " per hour" : m.paytype_id == 4 ? new NeWage(m.id32).current_wage.ToString("C2") + " per hour" : _salary.ToString("C0") + " per year";
        lbl_date.Text = System.DateTime.Today.ToShortDateString();
        pb_logo.ImageUrl = @"~\images\Logos\" + m.business_unit.logo_file;
        
        if (Convert.ToDateTime(m.TerminateDate) <= System.DateTime.Now)
        {
            lbl_body.Text = string.Format("This letter is to confirm that {0} was employed in the {1} position of {2} by {3} from {4} to {5} with an ending {6} of {7}.", m.FullName, (m.part_time ? "part time" : "full time"), m.membertype.MemberTypeName, new NeTaxEntity(m.business_unit.tax_entity_id).public_name, m.StartDate,m.TerminateDate,str_pt,  str_rate);
        }
        else
        {
            lbl_body.Text = string.Format("This letter is to confirm that {0} is employed in the {1} position of {2} by {3} starting {4} with a current {5} of {6}.", m.FullName, (m.part_time?"part time":"full time"), m.membertype.MemberTypeName, new NeTaxEntity(m.business_unit.tax_entity_id).public_name, m.StartDate, str_pt, str_rate);
        }

        lbl_address.Text = m.business_unit.address + "   " + m.business_unit.city + ", " + m.business_unit.provstate + ", " + (m.business_unit.country=="CDN"?"Canada":"USA") + "   " + m.business_unit.postal;
        lbl_conclusion.Text = "If you have any questions, please do not hesitate to contact me.";
        lbl_payroll_admin.Text = m.business_unit.branch_manager.FullName;
        lbl_email.Text = m.business_unit.branch_manager.NEEmail;
        lbl_company.Text = new NeTaxEntity(m.business_unit.tax_entity_id).public_name + "("+ m.business_unit.name + ")";
        lbl_title.Text = m.business_unit.branch_manager.membertype.name;

        report.Parameters[0].Value = m.id32;
  
        

        ReportViewer1.Report = report;
		ReportViewer1.DataBind();

		}



}

