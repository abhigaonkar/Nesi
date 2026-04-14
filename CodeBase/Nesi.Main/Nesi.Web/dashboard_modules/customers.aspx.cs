using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using nesi.core;

public partial class dashboard_modules_customers : System.Web.UI.Page
{
	NeMember user;
	private const int _page_id = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
		var _tools			= new Toolbox();
		_tools.dont_cache_page();
		user					= Toolbox.do_handle_authentication(_page_id);
		var _q	= Request.QueryString;
		var business_unit_id			= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		if(business_unit_id == 0)
			{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
			}
		var c								= new NeBusinessUnit(business_unit_id);
		var customers_ytd						= Toolbox.doSQL_int(@"
SELECT 
	cust_ytd
FROM 
	dashboard_snapshot_daily 
WHERE 
	business_unit_id = @v0 ", new object[] { c.id});

		var data								= Toolbox.doSQL_dt(@" SELECT * FROM dashboard_thresholds WHERE type = 'customers' AND member_id = @v0  AND business_unit_id = @v1 ", new object[] {  user.id, user.business_unit_id  } );

		var defaults				= new Dictionary<string,int>();
		defaults.Add("cust_fytd_good", 0);
		defaults.Add("cust_fytd_bad", 0);

		if(data.Rows.Count > 0)
			{
			foreach(DataRow datar in data.Rows)
				{
				var _subtype					= datar["subtype"].ToString();
				var good						= Convert.ToInt32(datar["good_threshold"]);
				var bad							= Convert.ToInt32(datar["bad_threshold"]);
				defaults[_subtype+"_good"]		= good;
				defaults[_subtype+"_bad"]		= bad;
				}
			}
		var is_bad								= false;
		var is_good							= false;
		gc_customers_fytd.Attributes.Add("data-good", defaults["cust_fytd_good"].ToString());
		gc_customers_fytd.Attributes.Add("data-bad", defaults["cust_fytd_bad"].ToString());
		if(customers_ytd <= defaults["cust_fytd_bad"])
			{
			is_bad									= true;
			gc_customers_fytd.Attributes["class"]	+= " b";
			}
		else if(customers_ytd >= defaults["cust_fytd_good"])
			{
			is_good									= true;
			gc_customers_fytd.Attributes["class"]	+= " g";
			}
		else
			{
			gc_customers_fytd.Attributes["class"]	+= " n";
			}

		gc_customers_fytd.InnerHtml				= "<b>"+customers_ytd+"</b";
		gc_customers_bytd.InnerHtml				= "0";

		if(is_bad)
			{
			bm_tile_customers.Attributes["class"]		+= " b";
			}
		else if(is_good)
			{
			bm_tile_customers.Attributes["class"]		+= " g";
			}
		else
			{
			bm_tile_customers.Attributes["class"]		+= " n";
			}
    }
}