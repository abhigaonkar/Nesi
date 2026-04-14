using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using nesi.core;

public partial class dashboard_modules_sales_pipline : System.Web.UI.Page
{
	NeMember user;
	private const int _page_id = 1;
	protected void Page_Load(object sender, EventArgs e)
	{
		var _tools = new Toolbox();
		_tools.dont_cache_page();
		var _q = Request.QueryString;
		user = Toolbox.do_handle_authentication(_page_id);
		var business_unit_id = Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
	
		if (business_unit_id == 0)
		{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
		}
		var c = new NeBusinessUnit(business_unit_id);
		var DATE = DateTime.Now;
		var data = Toolbox.doSQL_dt(@" SELECT * FROM dashboard_thresholds WHERE type = 'salespipeline' AND member_id = @v0  AND business_unit_id = @v1 ", new object[] { user.id, user.business_unit_id });
		var defaults = new Dictionary<string, int>();
		defaults.Add("90plus_good", 0);
		defaults.Add("90plus_bad", 0);

		defaults.Add("60to90_good", 0);
		defaults.Add("60to90_bad", 0);

		defaults.Add("30to60_good", 0);
		defaults.Add("30to60_bad", 0);

		defaults.Add("WOThisMonth_good", 0);
		defaults.Add("WOThisMonth_bad", 0);

		defaults.Add("WONextMonth_good", 0);
		defaults.Add("WONextMonth_bad", 0);

		defaults.Add("WOFollowingMonth_good", 0);
		defaults.Add("WOFollowingMonth_bad", 0);

		if (data.Rows.Count > 0)
		{
			foreach (DataRow datar in data.Rows)
			{
				var _subtype = datar["subtype"].ToString();
				var good = Convert.ToInt32(datar["good_threshold"]);
				var bad = Convert.ToInt32(datar["bad_threshold"]);
				defaults[_subtype + "_good"] = good;
				defaults[_subtype + "_bad"] = bad;
			}
		}
		var is_bad = false;
		var is_good = false;
		
		var v90plus = Toolbox.doSQL_double(string.Format("SELECT IFNULL(SUM(quoted_price),0) FROM quote_master WHERE business_unit_id = '{0}' AND pct_chance >= 90 AND status_id NOT IN (1,6,7,8,9) AND completion_date BETWEEN NOW() AND DATE_ADD(NOW(), INTERVAL 1 MONTH)", business_unit_id), null);
		var v60to90 = Toolbox.doSQL_double(string.Format("SELECT IFNULL(SUM(quoted_price),0) FROM quote_master WHERE business_unit_id = '{0}' AND pct_chance BETWEEN 60 AND 90 AND status_id NOT IN (1,6,7,8,9) AND completion_date BETWEEN NOW() AND DATE_ADD(NOW(), INTERVAL 1 MONTH)", business_unit_id), null);
		var v30to60 = Toolbox.doSQL_double(string.Format("SELECT IFNULL(SUM(quoted_price),0) FROM quote_master WHERE business_unit_id = '{0}' AND pct_chance BETWEEN 30 AND 60 AND status_id NOT IN (1,6,7,8,9) AND completion_date BETWEEN NOW() AND DATE_ADD(NOW(), INTERVAL 1 MONTH)", business_unit_id), null);
		#region gc_90plus
		gc_90plus.Attributes.Add("data-good", defaults["90plus_good"].ToString());
		gc_90plus.Attributes.Add("data-bad", defaults["90plus_bad"].ToString());
		if (v90plus <= defaults["90plus_bad"])
		{
			is_bad = true;
			gc_90plus.Attributes["class"] += " b";
		}
		else if (v90plus >= defaults["90plus_good"])
		{
			is_good = true;
			gc_90plus.Attributes["class"] += " g";
		}
		else
		{
			gc_90plus.Attributes["class"] += " n";
		}
		gc_90plus.InnerHtml = "<b>" + v90plus.ToString("C0") + "</b>";
		#endregion
		#region 60to90
		gc_60to90.Attributes.Add("data-good", defaults["60to90_good"].ToString());
		gc_60to90.Attributes.Add("data-bad", defaults["60to90_bad"].ToString());
		if (v60to90 <= defaults["60to90_bad"])
		{
			is_bad = true;
			gc_60to90.Attributes["class"] += " b";
		}
		else if (v60to90 >= defaults["60to90_good"])
		{
			is_good = true;
			gc_60to90.Attributes["class"] += " g";
		}
		else
		{
			gc_60to90.Attributes["class"] += " n";
		}
		gc_60to90.InnerHtml = "<b>" + v60to90.ToString("C0") + "</b>";
		#endregion
		#region v30to60
		gc_30to60.Attributes.Add("data-good", defaults["30to60_good"].ToString());
		gc_30to60.Attributes.Add("data-bad", defaults["30to60_bad"].ToString());
		if (v30to60 <= defaults["30to60_bad"])
		{
			is_bad = true;
			gc_30to60.Attributes["class"] += " b";
		}
		else if (v30to60 >= defaults["30to60_good"])
		{
			is_good = true;
			gc_30to60.Attributes["class"] += " g";
		}
		else
		{
			gc_30to60.Attributes["class"] += " n";
		}
		gc_30to60.InnerHtml = "<b>" + v30to60.ToString("C0") + "</b>";
		#endregion
		#region work orders this_month
		
		var this_month_start = Toolbox.MySQL_shortdt(DateTime.Now);
		var this_month_end = Toolbox.MySQL_shortdt(new DateTime(DATE.Year, DATE.Month, DateTime.DaysInMonth(DATE.Year, DATE.Month)));
		var wos_this_month = Toolbox.doSQL_double(string.Format(@"SELECT IFNULL(SUM(IF(woprog_quoteid=0,woprog_expected_sales_value,woprog_stilltobebilled)),0)
sales FROM woprog WHERE business_unit_id = @v0  AND woprog_expected_enddate BETWEEN @v1  AND @v2  AND woprog_bvwo != 'Not Entered' AND woprog_iscredit = 0 AND woprog_isrebill = 0
AND woprog_hold = 0 AND woprog_status != 'Invoiced'  "), new object[] { business_unit_id,  this_month_start, this_month_end });

		gc_wos_thismonth.Attributes.Add("data-good", defaults["WOThisMonth_good"].ToString());
		gc_wos_thismonth.Attributes.Add("data-bad", defaults["WOThisMonth_bad"].ToString());
		if (wos_this_month <= defaults["WOThisMonth_bad"])
		{
			is_bad = true;
			gc_wos_thismonth.Attributes["class"] += " b";
		}
		else if (wos_this_month >= defaults["WOThisMonth_good"])
		{
			is_good = true;
			gc_wos_thismonth.Attributes["class"] += " g";
		}
		else
		{
			gc_wos_thismonth.Attributes["class"] += " n";
		}

		gc_wos_thismonth.InnerHtml = "<b>" + wos_this_month.ToString("C") + "</b>";
		#endregion work orders this_month
		#region work orders next_month
		var next_month = DATE.AddMonths(1);
		var next_month_start = Toolbox.MySQL_shortdt(new DateTime(next_month.Year, next_month.Month, 1));
		var next_month_end = Toolbox.MySQL_shortdt(new DateTime(next_month.Year, next_month.Month, DateTime.DaysInMonth(next_month.Year, next_month.Month)));
		var wos_next_month = Toolbox.doSQL_double(string.Format(@" SELECT IFNULL(SUM(IF(woprog_quoteid=0,woprog_expected_sales_value,woprog_stilltobebilled)),0) sales FROM woprog WHERE business_unit_id = @v0 
AND woprog_expected_enddate BETWEEN @v1  AND @v2  AND woprog_bvwo != 'Not Entered' AND woprog_iscredit = 0 AND woprog_isrebill = 0
AND woprog_hold = 0 AND woprog_status != 'Invoiced'  "), new object[] { business_unit_id,  next_month_start, next_month_end });
		gc_wos_nextmonth.Attributes.Add("data-good", defaults["WONextMonth_good"].ToString());
		gc_wos_nextmonth.Attributes.Add("data-bad", defaults["WONextMonth_bad"].ToString());
		if (wos_next_month <= defaults["WONextMonth_bad"])
		{
			is_bad = true;
			gc_wos_nextmonth.Attributes["class"] += " b";
		}
		else if (wos_next_month >= defaults["WONextMonth_good"])
		{
			is_good = true;
			gc_wos_nextmonth.Attributes["class"] += " g";
		}
		else
		{
			gc_wos_nextmonth.Attributes["class"] += " n";
		}

		gc_wos_nextmonth.InnerHtml = "<b>" + wos_next_month.ToString("C") + "</b>";
		#endregion work orders next_month
		#region work orders following_month
		var following_month = DATE.AddMonths(2);
		var following_month_start = Toolbox.MySQL_shortdt(new DateTime(following_month.Year, following_month.Month, 1));
		var following_month_end = Toolbox.MySQL_shortdt(new DateTime(following_month.Year, following_month.Month, DateTime.DaysInMonth(following_month.Year, following_month.Month)));
		var wos_following_month = Toolbox.doSQL_double(string.Format(@" SELECT IFNULL(SUM(IF(woprog_quoteid=0,woprog_expected_sales_value,woprog_stilltobebilled)),0) sales
FROM woprog WHERE business_unit_id = @v0  AND woprog_expected_enddate BETWEEN @v1  AND @v2  AND woprog_bvwo != 'Not Entered' AND woprog_iscredit = 0 
AND woprog_isrebill = 0 AND woprog_hold = 0 AND woprog_status != 'Invoiced' "), new object[] { business_unit_id,  following_month_start, following_month_end });

		gc_wos_followingmonth.Attributes.Add("data-good", defaults["WOFollowingMonth_good"].ToString());
		gc_wos_followingmonth.Attributes.Add("data-bad", defaults["WOFollowingMonth_bad"].ToString());
		if (wos_following_month <= defaults["WOFollowingMonth_bad"])
		{
			is_bad = true;
			gc_wos_followingmonth.Attributes["class"] += " b";
		}
		else if (wos_following_month >= defaults["WOFollowingMonth_good"])
		{
			is_good = true;
			gc_wos_followingmonth.Attributes["class"] += " g";
		}
		else
		{
			gc_wos_followingmonth.Attributes["class"] += " n";
		}
		gc_wos_followingmonth.InnerHtml = "<b>" + wos_following_month.ToString("C") + "</b>";
		#endregion work orders following_month


		if (is_bad)
		{
			bm_tile_salespipeline.Attributes["class"] += " b";
		}
		else if (is_good)
		{
			bm_tile_salespipeline.Attributes["class"] += " g";
		}
		else
		{
			bm_tile_salespipeline.Attributes["class"] += " n";
		}
	}
}