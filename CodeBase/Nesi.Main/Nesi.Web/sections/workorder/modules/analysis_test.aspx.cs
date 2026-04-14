using System;
using nesi.core;

public partial class sections_workorder_modules_analysis_test : System.Web.UI.Page
{
	protected void Page_Init(object sender, EventArgs e)
	{
		if (Toolbox.Contains(Request.Url.Host, new[] { "", "www", "beta", "is", "devbeta", "print" }))
		{
			Response.End();
		}
		analysisq._wo = new NeWOProg(1031001);
		analysisq.woprog_id = analysisq._wo.woprog_id;
		analysisq._current_user = new NeMember(711);

		analysist._wo           = new NeWOProg(1030996);
		analysist.woprog_id     = analysist._wo.woprog_id;
		analysist._current_user = new NeMember(711);
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		analysisq.DataBind();
		analysisq.load();
	analysist.DataBind();
	analysist.load();
	}
}