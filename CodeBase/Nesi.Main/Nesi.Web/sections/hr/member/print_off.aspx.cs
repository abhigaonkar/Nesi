using System;
using DevExpress.XtraReports.UI;
using System.Collections.Specialized;
using nesi.core;
using nesi.core.print;

public partial class sections_hr_member_print_off : System.Web.UI.Page
	{
	NeMember current_user;
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools			= new Toolbox();
		current_user					= Toolbox.do_handle_authentication(127);
		var _q	= Request.QueryString;
		var id					= string.IsNullOrEmpty(_q["id"]) || _q["id"] == "0" ? 0 : Convert.ToInt32(_q["id"]);
		if(id == 0)
			{
			Toolbox.FriendlyException(Response, "User not defined", "/default.aspx");
			}
		var employee	= new NeMember(Convert.ToInt32(id));
		if(current_user.business_unit_id != employee.business_unit_id && !current_user.AuthenticatedForPrivilege(119))
			{
			Toolbox.FriendlyException(Response, "This user does not belong to your branch", "/default.aspx");
			}
		var e_info		= new EmployeeInfo((int) id);
		e_info.Name				= "EmployeeInfo-"+current_user.Initials;
		rv.Report				= (EmployeeInfo) fill_report(e_info);
		rv.DataBind();
		}
	private object fill_report(EmployeeInfo report)
		{
		var who_printed     = report.FindControl("who_printed", true) as XRLabel;
		var when_printed    = report.FindControl("when_printed", true) as XRLabel;
		who_printed.Text		= current_user.FullName;
		when_printed.Text		= DateTime.Now.ToString("u");
		return report;
		}
	}