using System;
using DevExpress.XtraReports.UI;
using nesi.core;
using nesi.core.print;

public partial class service_report : System.Web.UI.Page
	{
	NeMember myMember;
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(1);
		_tools.dont_cache_page();
		bindreport();
		}

	private void bindreport()
		{
		var report = new sr_main();
		    var xrPictureBox1 = report.FindControl("xrPictureBox1", true) as XRPictureBox;
        var val				= 0;
		try
			{
			val				= Convert.ToInt32(Request.QueryString["wo"]);
			}
		catch
			{
			Response.Clear();
			Response.Write("Work order not supplied.");
			Response.End();
			}
		val				= Toolbox.doSQL_int(@"SELECT IFNULL(MAX(woprog_id),0) FROM woprog  where cast(woprog_bvwo as unsigned) = @v0", new object[] { val });
		if(val == 0)
			{
			Response.Clear();
			Response.Write("Work order not supplied.");
			Response.End();
			}
		report.Parameters[0].Value = val;
		this.report_viewer.Report = report;
		this.report_viewer.DataBind();
		text_description.Text			= IsPostBack ? text_description.Text : Toolbox.doSQL_string(@"SELECT woprog_servicereportdesc FROM woprog  where woprog_id = @v0  limit 1", new object[] { val });
		    xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + new NeBusinessUnit(new NeWOProg(val).business_unit_id).logo_file;

    }
	protected void b_update_description_Click(object sender, EventArgs e)
		{
		var val			= Convert.ToInt32(Request.QueryString["wo"]);
		val				= Toolbox.doSQL_int(@"SELECT IFNULL(MAX(woprog_id),0) FROM woprog  where cast(woprog_bvwo as unsigned) = @v0", new object[] { val });
		if(val != 0)
			{
			Toolbox.doSQL_void(@"UPDATE woprog SET woprog_servicereportdesc = @v1  where woprog_id = @v0  limit 1", new object[] { val, text_description.Text });
			}
		}
}

