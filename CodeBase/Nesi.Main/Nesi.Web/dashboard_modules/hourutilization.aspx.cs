using System;
using System.Collections.Generic;
using System.Data;
using System.Collections.Specialized;
using nesi.core;

public partial class dashboard_modules_hourutilization : System.Web.UI.Page
{
	NeMember user;
	private const int _page_id = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
		var _q	= Request.QueryString;
		var business_unit_id			= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		
		var _tools			= new Toolbox();
		_tools.dont_cache_page();
		user					= Toolbox.do_handle_authentication(_page_id);
		if(business_unit_id == 0)
			{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
			}
		
		var c				= new NeBusinessUnit(business_unit_id);
		
		var hu1									= Toolbox.doSQL_dt(@" SELECT hu_mtd, hu_ytd, hu_bytd FROM dashboard_snapshot_daily WHERE business_unit_id = @v0 ", new object[] {  c.id } );
    if (hu1.Rows.Count > 0)
        {
        var hu = hu1.Rows[0];
        var hu_TYMHU = Convert.ToDouble(hu["hu_mtd"]);
        var hu_LYHU = Convert.ToDouble(hu["hu_ytd"]);
        var hu_TYHUBudget = Convert.ToDouble(hu["hu_bytd"]);

        var data = Toolbox.doSQL_dt(
            @" SELECT * FROM dashboard_thresholds WHERE type = 'hourutilization' AND member_id = @v0  AND business_unit_id = @v1 ",
            new object[] {user.id, user.business_unit_id});

        var defaults = new Dictionary<string, int>();
        defaults.Add("hour_bytd_good", 0);
        defaults.Add("hour_bytd_bad", 0);

        defaults.Add("hour_fytd_good", 0);
        defaults.Add("hour_fytd_bad", 0);

        defaults.Add("hour_mtd_good", 0);
        defaults.Add("hour_mtd_bad", 0);
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

        #region BYTD

        var bytd = Math.Round(hu_TYHUBudget * 100, 2);
        gc_hour_bytd.InnerHtml = hu_TYHUBudget.ToString() == "NaN" ? "0%" : "<b>" + bytd + "%</b>";

        #endregion BYTD

        #region FYTD

        gc_hour_fytd.Attributes.Add("data-good", defaults["hour_fytd_good"].ToString());
        gc_hour_fytd.Attributes.Add("data-bad", defaults["hour_fytd_bad"].ToString());
        var fytd = Math.Round(hu_LYHU * 100, 2);
        if (fytd <= defaults["hour_fytd_bad"])
            {
            is_bad = true;
            gc_hour_fytd.Attributes["class"] += " b";
            }
        else if (fytd >= defaults["hour_fytd_good"])
            {
            is_good = true;
            gc_hour_fytd.Attributes["class"] += " g";
            }
        else
            {
            gc_hour_fytd.Attributes["class"] += " n";
            }
        gc_hour_fytd.InnerHtml = hu_LYHU.ToString() == "NaN" ? "0%" : "<b>" + fytd + "%</b>";

        #endregion FYTD

        #region MTD

        gc_hour_mtd.Attributes.Add("data-good", defaults["hour_mtd_good"].ToString());
        gc_hour_mtd.Attributes.Add("data-bad", defaults["hour_mtd_bad"].ToString());
        var mtd = Math.Round(hu_TYMHU * 100, 2);
        if (mtd <= defaults["hour_mtd_bad"])
            {
            is_bad = true;
            gc_hour_mtd.Attributes["class"] += " b";
            }
        else if (mtd >= defaults["hour_mtd_good"])
            {
            is_good = true;
            gc_hour_mtd.Attributes["class"] += " g";
            }
        else
            {
            gc_hour_mtd.Attributes["class"] += " n";
            }
        gc_hour_mtd.InnerHtml = hu_TYMHU.ToString() == "NaN" ? "0%" : "<b>" + mtd + "%</b>";

        #endregion MTD

        if (is_bad)
            {
            bm_tile_hourutilization.Attributes["class"] += " b";
            }
        else if (is_good)
            {
            bm_tile_hourutilization.Attributes["class"] += " g";
            }
        else
            {
            bm_tile_hourutilization.Attributes["class"] += " n";
            }
        }
    }
}