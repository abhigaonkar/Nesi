using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using nesi.core;

public partial class dashboard_modules_revenue : System.Web.UI.Page
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
	            Response.Write(
	                "<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
	            Response.End();
	            }
	        var c = new NeBusinessUnit(business_unit_id);
	        var data = Toolbox.doSQL_dt(
	            @" SELECT * FROM dashboard_thresholds WHERE type = 'rev' AND member_id = @v0  AND business_unit_id = @v1 ",
	            new object[] {user.id, user.business_unit_id});
	        var defaults = new Dictionary<string, int>();
	        defaults.Add("rev_mtd_good", 0);
	        defaults.Add("rev_mtd_bad", 0);

	        defaults.Add("rev_fytd_good", 0);
	        defaults.Add("rev_fytd_bad", 0);

	        defaults.Add("rev_bmtd_good", 0);
	        defaults.Add("rev_bmtd_bad", 0);

	        defaults.Add("rev_bytd_good", 0);
	        defaults.Add("rev_bytd_bad", 0);
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

	        var rev1 = Toolbox.doSQL_dt(
	            @"SELECT rev_mtd, rev_bmtd, rev_ytd, rev_bytd FROM dashboard_snapshot_daily WHERE business_unit_id = @v0   ",
	            new object[] {c.id});
	        if (rev1.Rows.Count > 0)
	            {
	            var rev = rev1.Rows[0];
	            var rev_mtd = Convert.ToDouble(rev["rev_mtd"]);
	            var rev_bmtd = Convert.ToDouble(rev["rev_bmtd"]);
	            var rev_ytd = Convert.ToDouble(rev["rev_ytd"]);
	            var rev_bytd = Convert.ToDouble(rev["rev_bytd"]);

	            #region GC REV MTD

	            gc_rev_mtd.Attributes.Add("data-good", defaults["rev_mtd_good"].ToString());
	            gc_rev_mtd.Attributes.Add("data-bad", defaults["rev_mtd_bad"].ToString());
	            if (rev_mtd <= defaults["rev_mtd_bad"])
	                {
	                is_bad = true;
	                gc_rev_mtd.Attributes["class"] += " b";
	                }
	            else if (rev_mtd >= defaults["rev_mtd_good"])
	                {
	                is_good = true;
	                gc_rev_mtd.Attributes["class"] += " g";
	                }
	            else
	                {
	                gc_rev_mtd.Attributes["class"] += " n";
	                }
	            gc_rev_mtd.InnerHtml = "<b>" + rev_mtd.ToString("C0") + "</b>";

	            #endregion

	            #region GC REV YTD

	            gc_rev_fytd.Attributes.Add("data-good", defaults["rev_fytd_good"].ToString());
	            gc_rev_fytd.Attributes.Add("data-bad", defaults["rev_fytd_bad"].ToString());
	            if (rev_ytd <= defaults["rev_fytd_bad"])
	                {
	                is_bad = true;
	                gc_rev_fytd.Attributes["class"] += " b";
	                }
	            else if (rev_ytd >= defaults["rev_fytd_good"])
	                {
	                is_good = true;
	                gc_rev_fytd.Attributes["class"] += " g";
	                }
	            else
	                {
	                gc_rev_fytd.Attributes["class"] += " n";
	                }
	            gc_rev_fytd.InnerHtml = "<b>" + rev_ytd.ToString("C0") + "</b>";

	            #endregion

	            #region GC REV BMTD

	            gc_rev_bmtd.InnerHtml = "<b>" + rev_bmtd.ToString("C0") + "</b>";

	            #endregion

	            #region GV REV BYTD

	            gc_rev_bytd.InnerHtml = "<b>" + rev_bytd.ToString("C0") + "</b>";

	            #endregion

	            if (is_bad)
	                {
	                bm_tile_revenue.Attributes["class"] += " b";
	                }
	            else if (is_good)
	                {
	                bm_tile_revenue.Attributes["class"] += " g";
	                }
	            else
	                {
	                bm_tile_revenue.Attributes["class"] += " n";
	                }
	            }
	        }
	    }