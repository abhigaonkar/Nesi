using System;
using nesi.core;
using nesi.core.print;

public partial class sections_reports_print_quote_worksheet_index : System.Web.UI.Page
	{
	NeMember myMember;
	protected void Page_Load(object sender, EventArgs e)
		{

		myMember = Toolbox.do_handle_authentication(65);
        
		bindreport();

		}

	private void bindreport()
		{
		var report = new WorkSheetDisp();
		report.Parameters[0].Value = Convert.ToInt32(Request.QueryString["quoteid"]);
		report.Parameters[1].Value = Convert.ToInt32(Request.QueryString["revision"]);
		report.Parameters[2].Value = myMember.id;
		report.stuff = Convert.ToString(Request.QueryString["stuff"]);
		if (report.stuff == "85")
			{
			report.Adjusted_sell = Convert.ToString(Request.QueryString["Q"]);
			report.TM_Sell = Convert.ToString(Request.QueryString["T"]);
			}
        

		this.ReportViewer1.Report = report;
		this.ReportViewer1.DataBind();
        this.Title = "Quote " + report.Parameters[0].Value + " Worksheet Printout";
		}
	}
