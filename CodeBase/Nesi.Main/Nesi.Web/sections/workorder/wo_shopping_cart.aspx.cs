using System;
using nesi.core.print;

public partial class sections_workorder_wo_shopping_cart : System.Web.UI.Page
	{
	string woid	= "";
	int unfulfilled = 0;
	protected void Page_Load(object sender, EventArgs e)
		{
		woid = Request.QueryString["woid"];
		unfulfilled = string.IsNullOrEmpty(Request.QueryString["unf"]) ? 0 : 1;
		bindreport();
		}

	private void bindreport()
		{
		var report = new woshoppingcart(Convert.ToInt32(woid));
		report.Parameters[0].Value = woid;
		report.Parameters[1].Value = unfulfilled;
		this.RptVwWOSO.Report = report;
		this.RptVwWOSO.DataBind();
		}
	}
