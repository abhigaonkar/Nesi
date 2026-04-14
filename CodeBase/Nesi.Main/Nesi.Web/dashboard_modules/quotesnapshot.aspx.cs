using System;
using System.Data;
using System.Collections.Specialized;
using nesi.core;

public partial class dashboard_modules_quotesnapshot : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		var _q	= Request.QueryString;
		var business_unit_id						= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		if(business_unit_id == 0)
			{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
			}
		var _chart_quote_ds		= Toolbox.doSQL_dt(@"SELECT b.status Arguement, count(*) Value FROM quote_master a left join quote_status b on a.status_id = b.id  WHERE a.business_unit_id =@v0 AND a.status_id NOT IN (6,5,7,8,9) group by a.status_id", new object[] { business_unit_id });
	//	Series quote_series				= chart_quote.Series[0];
	//	quote_series.DataSource			= _chart_quote_ds;
	//	quote_series.ArgumentDataMember	= "Arguement";
	//	quote_series.ArgumentScaleType	= ScaleType.Qualitative;
	//	quote_series.ValueScaleType		= ScaleType.Numerical;
	//	quote_series.ValueDataMembers.AddRange(new string[] { "Value" });

    }
}