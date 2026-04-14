using System;
using System.Collections.Specialized;
using nesi.core;

public partial class dashboard_modules_grids : System.Web.UI.Page
{
	protected void Page_Init(object sender, EventArgs e)
		{
		var business_unit_id						= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		var branch					= new NeBusinessUnit(business_unit_id);
		ds_largest.SelectParameters["fiscal_start"].DefaultValue		= Toolbox.MySQL_shortdt(branch.fiscal_start_current);
		if(business_unit_id == 0)
			{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
			}

		}
    protected void Page_Load(object sender, EventArgs e)
    {
    }
}