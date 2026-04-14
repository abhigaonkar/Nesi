using System;
using DevExpress.XtraReports.UI;
using System.Collections.Specialized;
using nesi.core;
using nesi.core.print;

public partial class sections_hr_member_mt_print_off : System.Web.UI.Page
	{
	NeMember current_user;
	NeMemberType mt;
	public string prov;
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools			= new Toolbox();
		current_user					= Toolbox.do_handle_authentication(127);
		var _q	= Request.QueryString;
		var mtid					= string.IsNullOrEmpty(_q["mtid"]) || _q["mtid"] == "0" ? 0 : Convert.ToInt32(_q["mtid"]);
		if(mtid == 0)
			{
			Toolbox.FriendlyException(Response, "Membertype not defined", "/default.aspx");
			}
		prov = _q["prov"];
		mt	= new NeMemberType(Convert.ToInt32(mtid));
		var e_info = new print_membertype();


		rv.Report				= (print_membertype) fill_report(e_info);
		rv.DataBind();
		}
	private object fill_report(print_membertype report)
		{
		var title     = report.FindControl("lbltitle", true) as XRLabel;
		var lbl_hdr = report.FindControl("lbl_hdr", true) as XRLabel;
		var lblreports = report.FindControl("lblreports", true) as XRLabel;
		var lblobjective = report.FindControl("lblobjective", true) as XRLabel;
		var lbldate = report.FindControl("lbldate", true) as XRLabel;
		var xrPictureBox1 = report.FindControl("xrPictureBox1", true) as XRPictureBox;

        lbldate.Text = System.DateTime.Today.ToString("yyyy-MM-dd");
			title.Text = mt.name + " (" +
			             new Toolbox().getSQL_string(@"Select prov_desc from prov  where prov_abbv =@v0", new object[] { prov }) + ")";
			lbl_hdr.Text = "Job Description - " + mt.name + " (" +
			               new Toolbox().getSQL_string(@"Select prov_desc from prov  where prov_abbv =@v0", new object[] { prov }) + ")";
		lblreports.Text = new NeMemberType(mt.reports_to).name;
		lblobjective.Text = mt.objective;
		    xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + current_user.business_unit.logo_file;
        report.Parameters[0].Value = Convert.ToInt32(mt.id);
		report.Parameters[1].Value = prov;
		return report;
		}
	}