using System;
using System.Collections.Specialized;
using nesi.core;

public partial class dashboard_modules_10worstcustomers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		var _tools			= new Toolbox();
		_tools.dont_cache_page();
		var _q	= Request.QueryString;
		var business_unit_id			= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		if(business_unit_id == 0)
			{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
			}
    }
}