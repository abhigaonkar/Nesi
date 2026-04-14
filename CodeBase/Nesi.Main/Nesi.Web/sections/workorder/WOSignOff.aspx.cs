using System;
using System.Data;
using DevExpress.XtraReports.UI;
using nesi.core;
using nesi.core.print;

public partial class sections_workorder_WOSignOff : System.Web.UI.Page
{
    string woid;
    Toolbox _Tools = new Toolbox();

    protected void Page_Load(object sender, EventArgs e)
    {
        woid = Request.QueryString["woid"];
        bindreport();
    }

    private void bindreport()
    {
        try
        {
            var report = new WOSOSheet();
            report.Parameters[0].Value = woid;
			var pi =  (XRPageInfo) report.FindControl("xrPageInfo1", true);
			var xrlbl_workdone = (XRLabel)report.FindControl("xrlbl_workdone", true);

			
			var dt = _Tools.getSQL_datatable(@"Call wo_signoffsheet_work_completed(@v0)",new object[] { woid } );
			if (dt.Rows.Count > 0)
			{
				xrlbl_workdone.Text = "Work Completed:" + System.Environment.NewLine;
				var x = 1;
				foreach (DataRow dr in dt.Rows)
				{
					xrlbl_workdone.Text += x + ". " + dr[0] + System.Environment.NewLine;
					x++;
				}
			}
			pi.Format			= "{0:D}";
			pi.LocationFloat	= new DevExpress.Utils.PointFloat(500F, 0F);
			pi.TextAlignment	= DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			pi.SizeF			= new System.Drawing.SizeF(120F, 23F);
            this.RptVwWOSO.Report = report;
            this.RptVwWOSO.DataBind();
        }
        catch (Exception ee)
        {
            _Tools.catch_error(ee);
        }
    }
}
