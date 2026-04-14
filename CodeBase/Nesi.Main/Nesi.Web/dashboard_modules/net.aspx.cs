using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using nesi.core;

public partial class dashboard_modules_net : System.Web.UI.Page
{
	NeMember user;
	private const int _page_id = 1;
	protected void Page_Load(object sender, EventArgs e)
	{
		var _q = Request.QueryString;
		var business_unit_id = Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		if (business_unit_id == 0)
		{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
		}
		var _tools = new Toolbox();
		_tools.dont_cache_page();
		user = Toolbox.do_handle_authentication(_page_id);
		var c = new NeBusinessUnit(business_unit_id);
		var DATE = DateTime.Now;
		
		var dt = _tools.getSQL_datatable(@"Select * from daily_finance  where business_unit_id =@v0 and type = 'IS' order by date desc limit 1", new object[] { business_unit_id });

	    if (dt.Rows.Count>0)
	        {
	        var data = Toolbox.doSQL_dt(
	            @" SELECT * FROM dashboard_thresholds WHERE type = 'net' AND member_id = @v0  AND business_unit_id = @v1 ",
	            new object[] {user.id, user.business_unit_id});
	        var defaults = new Dictionary<string, int>();
	        defaults.Add("net_mtd_good", 0);
	        defaults.Add("net_mtd_bad", 0);
	        defaults.Add("net_btm_good", 0);
	        defaults.Add("net_btm_bad", 0);
	        defaults.Add("net_fytd_good", 0);
	        defaults.Add("net_fytd_bad", 0);
	        defaults.Add("net_ftm_good", 0);
	        defaults.Add("net_ftm_bad", 0);
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

	        // Seriously? Did Shawn pay us a visit?
	        // Make a DataRow, and use column name indexing.

	        var mtd = Convert.ToDouble(dt.Rows[0][4]);
	        var mtd_pct = Convert.ToDouble(dt.Rows[0][5]);
	        var btm = Convert.ToDouble(dt.Rows[0][8]);
	        var btm_pct = Convert.ToDouble(dt.Rows[0][9]);
	        var fytd = Convert.ToDouble(dt.Rows[0][6]);
	        var fytd_pct = Convert.ToDouble(dt.Rows[0][7]);
	        var ftm = Convert.ToDouble(dt.Rows[0][10]);
	        var ftm_pct = Convert.ToDouble(dt.Rows[0][11]);

	        if (Math.Round(mtd_pct * 100) <= defaults["net_mtd_bad"])
	            {
	            is_bad = true;
	            gc_net_mtd.Attributes["class"] += " b";
	            }
	        else if (Math.Round(mtd_pct * 100) >= defaults["net_mtd_good"])
	            {
	            is_good = true;
	            gc_net_mtd.Attributes["class"] += " g";
	            }
	        else
	            {
	            gc_net_mtd.Attributes["class"] += " n";
	            }
	        gc_net_mtd.InnerHtml = "<div class='pct'>" + (mtd_pct == 0 ? "" : mtd_pct.ToString("P0")) + "</div><b>" +
	                               mtd.ToString("C0") + "</b>";


	        #region Budget MTD

	        gc_net_btm.InnerHtml = "<div class='pct'>" + (btm_pct == 0 ? "" : btm_pct.ToString("P0")) + "</div><b>" +
	                               btm.ToString("C0") + "</b>";

	        #endregion Budget MTD

	        #region Fiscal YTD

	        gc_net_fytd.Attributes.Add("data-good", defaults["net_fytd_good"].ToString());
	        gc_net_fytd.Attributes.Add("data-bad", defaults["net_fytd_bad"].ToString());
	        if (Math.Round(fytd_pct * 100) <= defaults["net_fytd_bad"])
	            {
	            is_bad = true;
	            gc_net_fytd.Attributes["class"] += " b";
	            }
	        else if (Math.Round(fytd_pct * 100) >= defaults["net_fytd_good"])
	            {
	            is_good = true;
	            gc_net_fytd.Attributes["class"] += " g";
	            }
	        else
	            {
	            gc_net_fytd.Attributes["class"] += " n";
	            }
	        gc_net_fytd.InnerHtml = "<div class='pct'>" + (fytd_pct == 0 ? "" : fytd_pct.ToString("P0")) + "</div><b>" +
	                                fytd.ToString("C0") + "</b>";

	        #endregion Fiscal YTD

	        #region Budget Fiscal this Month

	        gc_net_ftm.InnerHtml = "<div class='pct'>" + (ftm_pct == 0 ? "" : ftm_pct.ToString("P0")) + "</div><b>" +
	                               ftm.ToString("C0") + "</b>";

	        #endregion Budget Fiscal this Month

	        if (is_bad)
	            {
	            bm_tile_netincome.Attributes["class"] += " b";
	            }
	        else if (is_good)
	            {
	            bm_tile_netincome.Attributes["class"] += " g";
	            }
	        else
	            {
	            bm_tile_netincome.Attributes["class"] += " n";
	            }
	        }
	    }



//	protected void old_page_load(object sender, EventArgs e)
//	{
//		var _q = Request.QueryString;
//		var business_unit_id = Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
//		if (business_unit_id == 0)
//		{
//			Response.Clear();
//			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
//			Response.End();
//		}
//		var _tools = new Toolbox();
//		_tools.dont_cache_page();
//		user = Toolbox.do_handle_authentication(_page_id);
//		var c = new NeBusinessUnit(business_unit_id);
//		var DATE = DateTime.Now;
//		var month_table = "";
//		var lastyear_month_table = "";
//		var lastmonth_table = "";
//		var budget_month_table = "";
//		var lastyear_total_tables = "";
//		var total_tables = "";
//		var total_budget_tables = "";

//		#region Table name(s) assignment loop
//		var month_int = DATE.Month;
//		if (c.country == "CDN")
//		{
//			month_int = month_int < 6
//										? month_int + 7
//										: month_int - 5;
//		}
//		for (var m = 1; m <= month_int; m++)
//		{
//			var pre = "";
//			var suff = "";
//			var month = "";
//			if (m < 10)
//			{
//				pre = "0";
//			}
//			else
//			{
//				pre = "";
//			}
//			month = pre + m;
//			if (m != month_int)
//			{
//				suff = "+";
//			}
//			else
//			{
//				suff = "";
//			}
//			total_tables += "this_yr" + month + suff;
//			total_budget_tables += "bdgt_this_yr" + month + suff;
//			lastyear_total_tables += "last_yr" + month + suff;
//		}
//		switch (c.country)
//		{
//			case "CDN":
//				var temp_month_int = 0;
//				var temp_last_month_int = 0;
//				if (DATE.Month < 6) // Fiscal year for Canada starts June 1st. So all months need to be shifted to accomodate
//				{
//					temp_month_int = DATE.Month + 7;
//					temp_last_month_int = DATE.Month + 6;
//				}
//				else
//				{
//					temp_month_int = DATE.Month - 5;
//					temp_last_month_int = DATE.Month - 6;
//				}
//				month_table = "this_yr" + string.Format("{0:00}", temp_month_int);
//				lastmonth_table = temp_last_month_int > 0 ? "this_yr" + string.Format("{0:00}", temp_last_month_int) : "last_yr12";
//				lastyear_month_table = "last_yr" + string.Format("{0:00}", temp_month_int);
//				budget_month_table = "bdgt_this_yr" + string.Format("{0:00}", temp_month_int);
//				break;
//			case "USA":
//				month_table = "this_yr" + string.Format("{0:00}", month_int);
//				lastmonth_table = month_int > 1 ? "this_yr" + string.Format("{0:00}", month_int - 1) : "last_yr12";
//				lastyear_month_table = "last_yr" + string.Format("{0:00}", month_int);
//				budget_month_table = "bdgt_this_yr" + string.Format("{0:00}", month_int);
//				break;
//		}
//		#endregion
//		#region totals
//		var debit_mat_accts = "50100,50109,50110,50111,50510,50610,50620,52005,53005,59505,66110,88350";
//		var debit_lab_accts = "51105,51125,51135,51145,51165,51510,51515,51520,51525,51530,51590,51610,51615,51620,51625,51905,51990,76058";
//		var debit_accts = debit_lab_accts + "," + debit_mat_accts;
//		var credit_accts = "41001,41005,41205,41405,41505,41605,41705,42005,42990,43005,43999,44000,44100,44500,47500,48315,49115,49198,49125,49405,49505,49515,48305,48315,49105";

//		var _totals = new DataTable();
//		try
//		{
//			_totals = _tools.getSQL_datatable(string.Format(@"
//SELECT 
//(SELECT IFNULL(SUM({1}), 0) FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'C' AND division = '{0}' AND acct_no IN ({8})) c_mtd,
//(SELECT IFNULL(SUM({7}), 0) FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'C' AND division = '{0}' AND acct_no IN ({8})) c_lmtd,
//(SELECT IFNULL(SUM({2}), 0) FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'C' AND division = '{0}' AND acct_no IN ({8})) c_ytd,
//(SELECT IFNULL(SUM({3}), 0) FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'C' AND division = '{0}' AND acct_no IN ({8})) c_lytd,
//(SELECT IFNULL(SUM({4}), 0) FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'C' AND division = '{0}' AND acct_no IN ({8})) c_lymtd,
//(SELECT IFNULL(SUM(({1}) - ({5})), 0) vytd FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'C' AND division = '{0}' AND acct_no IN ({8})) c_vmtd,
//(SELECT IFNULL(SUM(({2}) - ({6})), 0) vytd FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'C' AND division = '{0}' AND acct_no IN ({8})) c_vytd,
//(SELECT IFNULL(SUM({5}), 0) bmtd FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'C' AND division = '{0}' AND acct_no IN ({8})) c_bmtd,
//(SELECT IFNULL(SUM({6}), 0) bytd FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'C' AND division = '{0}' AND acct_no IN ({8})) c_bytd,
//(SELECT SUM({1}) FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'D' AND acct_no IN ({9}) AND division = '{0}') d_mtd,
//(SELECT SUM({7}) FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'D' AND acct_no IN ({9}) AND division = '{0}') d_lmtd,
//(SELECT SUM({2}) FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'D' AND acct_no IN ({9}) AND division = '{0}') d_ytd,
//(SELECT SUM({3}) FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'D' AND acct_no IN ({9}) AND division = '{0}') d_lytd,
//(SELECT SUM({4}) FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'D' AND acct_no IN ({9}) AND division = '{0}') d_lymtd,
//(SELECT IFNULL(SUM(({1}) - ({5})), 0) vmtd FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'D' AND acct_no IN ({9}) AND division = '{0}') d_vmtd,
//(SELECT IFNULL(SUM(({2}) - ({6})), 0) vytd FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'D' AND acct_no IN ({9}) AND division = '{0}') d_vytd,
//(SELECT IFNULL(SUM({5}), 0) bytd FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'D'  AND acct_no IN ({9}) AND division = '{0}') d_bmtd,
//(SELECT IFNULL(SUM({6}), 0) bytd FROM GL_CHART_OF_ACCOUNTS WHERE dr_cr_desig = 'D'  AND acct_no IN ({9}) AND division = '{0}') d_bytd",
//				"000",                          // 0
//				month_table,                    // 1
//				total_tables,                   // 2
//				lastyear_total_tables,          // 3
//				lastyear_month_table,           // 4
//				budget_month_table,             // 5
//				total_budget_tables,            // 6
//				lastmonth_table,                // 7
//				credit_accts,                   // 8
//				debit_accts                     // 9
//			), c.DSN,null);
//		}
//		catch (Exception ee)
//		{
//			_tools.catch_error(ee);
//		}
//		var total = _totals.Rows[0];
//		var total_cytd = Convert.ToDouble(total["c_ytd"]);
//		var total_cmtd = Convert.ToDouble(total["c_mtd"]);
//		var total_cbytd = Convert.ToDouble(total["c_bytd"]);
//		var total_cbmtd = Convert.ToDouble(total["c_bmtd"]);
//		#endregion
//		var data = Toolbox.doSQL_dt(@" SELECT * FROM dashboard_thresholds WHERE type = 'net' AND member_id = @v0  AND business_unit_id = @v1 ", new object[] { user.id, user.business_unit_id });
//		var defaults = new Dictionary<string, int>();
//		defaults.Add("net_mtd_good", 0);
//		defaults.Add("net_mtd_bad", 0);
//		defaults.Add("net_btm_good", 0);
//		defaults.Add("net_btm_bad", 0);
//		defaults.Add("net_fytd_good", 0);
//		defaults.Add("net_fytd_bad", 0);
//		defaults.Add("net_ftm_good", 0);
//		defaults.Add("net_ftm_bad", 0);
//		if (data.Rows.Count > 0)
//		{
//			foreach (DataRow datar in data.Rows)
//			{
//				var _subtype = datar["subtype"].ToString();
//				var good = Convert.ToInt32(datar["good_threshold"]);
//				var bad = Convert.ToInt32(datar["bad_threshold"]);
//				defaults[_subtype + "_good"] = good;
//				defaults[_subtype + "_bad"] = bad;
//			}
//		}
//		var is_bad = false;
//		var is_good = false;
//		#region MTD
//		gc_net_mtd.Attributes.Add("data-good", defaults["net_mtd_good"].ToString());
//		gc_net_mtd.Attributes.Add("data-bad", defaults["net_mtd_bad"].ToString());
//		var _totals2 = _tools.getSQL_datatable(string.Format(@"
//SELECT 
//(SELECT SUM({0}) total_credit_mtd FROM GL_CHART_OF_ACCOUNTS WHERE division = '{1}' AND gl_group >= 400 AND dr_cr_desig = 'C') total_credit_mtd,
//(SELECT SUM({0}) total_credit_mtd FROM GL_CHART_OF_ACCOUNTS WHERE division = '{1}' AND gl_group >= 400 AND dr_cr_desig = 'D') total_debit_mtd, 
//(SELECT SUM({2}) total_credit_mtd FROM GL_CHART_OF_ACCOUNTS WHERE division = '{1}' AND gl_group >= 400 AND dr_cr_desig = 'C') total_credit_bmtd,
//(SELECT SUM({2}) total_credit_mtd FROM GL_CHART_OF_ACCOUNTS WHERE division = '{1}' AND gl_group >= 400 AND dr_cr_desig = 'D') total_debit_bmtd,
//(SELECT SUM({3}) total_credit_mtd FROM GL_CHART_OF_ACCOUNTS WHERE division = '{1}' AND gl_group >= 400 AND dr_cr_desig = 'C') total_credit_fytd,
//(SELECT SUM({3}) total_credit_mtd FROM GL_CHART_OF_ACCOUNTS WHERE division = '{1}' AND gl_group >= 400 AND dr_cr_desig = 'D') total_debit_fytd,
//(SELECT SUM({4}) total_credit_mtd FROM GL_CHART_OF_ACCOUNTS WHERE division = '{1}' AND gl_group >= 400 AND dr_cr_desig = 'C') total_credit_ftm,
//(SELECT SUM({4}) total_credit_mtd FROM GL_CHART_OF_ACCOUNTS WHERE division = '{1}' AND gl_group >= 400 AND dr_cr_desig = 'D') total_debit_ftm
//",
//			month_table,
//			"000",
//			budget_month_table,
//			total_tables,
//			total_budget_tables
//		), c.DSN,null);
//		var dr = _totals2.Rows[0];
//		var total_credit_bmtd = Toolbox.ReturnZeroIfNull_double(dr["total_credit_bmtd"]);
//		var total_debit_bmtd = Toolbox.ReturnZeroIfNull_double(dr["total_debit_bmtd"]);
//		var total_credit_mtd = Toolbox.ReturnZeroIfNull_double(dr["total_credit_mtd"]);
//		var total_debit_mtd = Toolbox.ReturnZeroIfNull_double(dr["total_debit_mtd"]);
//		var total_credit_fytd = Toolbox.ReturnZeroIfNull_double(dr["total_credit_fytd"]);
//		var total_debit_fytd = Toolbox.ReturnZeroIfNull_double(dr["total_debit_fytd"]);
//		var total_credit_ftm = Toolbox.ReturnZeroIfNull_double(dr["total_credit_ftm"]);
//		var total_debit_ftm = Toolbox.ReturnZeroIfNull_double(dr["total_debit_ftm"]);


//		var mtd = total_credit_mtd - total_debit_mtd;
//		var mtd_pct = mtd == 0 && total_cmtd == 0 ? 0 : mtd / total_cmtd;
//		var btm = total_credit_bmtd - total_debit_bmtd;
//		var btm_pct = btm == 0 && total_cbmtd == 0 ? 0 : btm / total_cbmtd;
//		var fytd = total_credit_fytd - total_debit_fytd;
//		var fytd_pct = fytd == 0 && total_cytd == 0 ? 0 : fytd / total_cytd;
//		var ftm = total_credit_ftm - total_debit_ftm;
//		var ftm_pct = ftm == 0 && total_cbytd == 0 ? 0 : ftm / total_cbytd;

//		if (Math.Round(mtd_pct * 100) <= defaults["net_mtd_bad"])
//		{
//			is_bad = true;
//			gc_net_mtd.Attributes["class"] += " b";
//		}
//		else if (Math.Round(mtd_pct * 100) >= defaults["net_mtd_good"])
//		{
//			is_good = true;
//			gc_net_mtd.Attributes["class"] += " g";
//		}
//		else
//		{
//			gc_net_mtd.Attributes["class"] += " n";
//		}
//		gc_net_mtd.InnerHtml = "<div class='pct'>" + (mtd_pct == 0 ? "" : mtd_pct.ToString("P0")) + "</div><b>" + mtd.ToString("C0") + "</b>";
//		#endregion MTD

//		#region Budget MTD
//		gc_net_btm.InnerHtml = "<div class='pct'>" + (btm_pct == 0 ? "" : btm_pct.ToString("P0")) + "</div><b>" + btm.ToString("C0") + "</b>";
//		#endregion Budget MTD

//		#region Fiscal YTD
//		gc_net_fytd.Attributes.Add("data-good", defaults["net_fytd_good"].ToString());
//		gc_net_fytd.Attributes.Add("data-bad", defaults["net_fytd_bad"].ToString());
//		if (Math.Round(fytd_pct * 100) <= defaults["net_fytd_bad"])
//		{
//			is_bad = true;
//			gc_net_fytd.Attributes["class"] += " b";
//		}
//		else if (Math.Round(fytd_pct * 100) >= defaults["net_fytd_good"])
//		{
//			is_good = true;
//			gc_net_fytd.Attributes["class"] += " g";
//		}
//		else
//		{
//			gc_net_fytd.Attributes["class"] += " n";
//		}
//		gc_net_fytd.InnerHtml = "<div class='pct'>" + (fytd_pct == 0 ? "" : fytd_pct.ToString("P0")) + "</div><b>" + fytd.ToString("C0") + "</b>";
//		#endregion Fiscal YTD

//		#region Budget Fiscal this Month
//		gc_net_ftm.InnerHtml = "<div class='pct'>" + (ftm_pct == 0 ? "" : ftm_pct.ToString("P0")) + "</div><b>" + ftm.ToString("C0") + "</b>";
//		#endregion Budget Fiscal this Month

//		if (is_bad)
//		{
//			bm_tile_netincome.Attributes["class"] += " b";
//		}
//		else if (is_good)
//		{
//			bm_tile_netincome.Attributes["class"] += " g";
//		}
//		else
//		{
//			bm_tile_netincome.Attributes["class"] += " n";
//		}
//		//	bm_tile_netincome.Attributes["class"]			= "tile b clickable";
//	}
}