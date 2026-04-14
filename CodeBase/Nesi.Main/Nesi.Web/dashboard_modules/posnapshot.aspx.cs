using System;
using System.Data;
using System.Collections.Specialized;
using nesi.core;

public partial class dashboard_modules_posnapshot : System.Web.UI.Page
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
		var _chart_po_ds			= Toolbox.doSQL_dt(@"select b.status_type Arguement, count(*) Value from poprog_header a left join poprog_status b on a.poprog_status = b.poprog_status_id  where business_unit_id =@v0 and poprog_status NOT IN (4,7,8) AND poprog_status is not null group by poprog_status", new object[] { business_unit_id });
	//	Series po_series				= chart_po.Series[0];
	//	po_series.DataSource			= _chart_po_ds;
	//	po_series.ArgumentDataMember	= "Arguement";
	//	po_series.ArgumentScaleType		= ScaleType.Qualitative;
	//	po_series.ValueScaleType		= ScaleType.Numerical;
	///	bm_purchaseorders_title.InnerHtml	= "<a href='/sections/purchaseorder/po_prog_edit.aspx?business_unit_id="+business_unit_id+"'>PO Snapshot</a>";
	//	po_series.ValueDataMembers.AddRange(new string[] { "Value" });

    }
}