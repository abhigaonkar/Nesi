using System;
using System.Web.UI;
using nesi.core;

public partial class this_workOrder_lineitems_page : Page
	{
	private const int _page_id = 1; // from Page table in DB

	protected void Page_Load(object sender, EventArgs e)
		{
		var member = Toolbox.do_handle_authentication(_page_id);
		var _q = Request.QueryString;
		Toolbox.do_set_XML_header(Response);

		try
			{
			if (string.IsNullOrEmpty(_q["id"]) || string.IsNullOrEmpty(_q["newdate"])) return;

			var resultdate = (Convert.ToDateTime(_q["newdate"]) - DateTime.Today).Days < 7
				? DateTime.Today.AddDays(7)
				: Convert.ToDateTime(_q["newdate"]);

			var x = Toolbox.doSQL_affectedrows(@"UPDATE wo_detail_current  SET wo_detail_current_date_required =@v0  WHERE wo_detail_current_qty_ordered>wo_detail_current_qty_committed AND wo_detail_current_type = 'M' AND wo_detail_current_woprog_id =@v1", new object[] { string.Format("{0:yyyy-MM-dd}", resultdate), _q["id"] });

			Toolbox.doSQL_void(@"UPDATE woprog  SET woprog_Expected_Startdate =@v0  where woprog_id=@v1", new object[] { string.Format("{0:yyyy-MM-dd 00:00:00}", Convert.ToDateTime(_q["newdate"])), _q["id"] });

			Response.Write(string.Format("<response><result>{0}</result><resultdate>{1:yyyy-MM-dd}</resultdate></response>", x, resultdate));
			Response.End();
			}
		catch (Exception ee)
			{
			Toolbox.do_errorLog_errorStack(ee);
			}
		}
	}

