using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using nesi.core;

public partial class dashboard_modules_gross : System.Web.UI.Page
{
	NeMember user;
	private const int _page_id = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
		var _tools						= new Toolbox();
		_tools.dont_cache_page();
		user					= Toolbox.do_handle_authentication(_page_id);
		var _q	= Request.QueryString;
		var business_unit_id			= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		
		if(business_unit_id == 0)
			{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Business unit not set!</div>");
			Response.End();
			}
		var c				= new NeBusinessUnit(business_unit_id);

		var data								= Toolbox.doSQL_dt(@" SELECT * FROM dashboard_thresholds WHERE type = 'gross' AND member_id = @v0  AND business_unit_id = @v1 ", new object[] {  user.id, user.business_unit_id  } );
        
        var defaults				= new Dictionary<string,int>();
		defaults.Add("margin_mtd_good", 0);
		defaults.Add("margin_mtd_bad", 0);
		defaults.Add("margin_fytd_good", 0);
		defaults.Add("margin_fytd_bad", 0);
		if(data.Rows.Count > 0)
			{
			foreach(DataRow datar in data.Rows)
				{
				var _subtype					= datar["subtype"].ToString();
				var good						= Convert.ToInt32(datar["good_threshold"]);
				var bad							= Convert.ToInt32(datar["bad_threshold"]);
				defaults[_subtype+"_good"]			= good;
				defaults[_subtype+"_bad"]			= bad;
				}
			}
		var is_bad									= false;
		var is_good								= false;
		var gross1								= Toolbox.doSQL_dt(@" SELECT gross_mtd_dol, gross_mtd_pct, gross_ytd_dol, gross_ytd_pct FROM dashboard_snapshot_daily WHERE business_unit_id = @v0   ", new object[] {  c.id } );
    if (gross1.Rows.Count > 0)
        {
        var gross = gross1.Rows[0];
        var gross_profit_mtd = Convert.ToDouble(gross["gross_mtd_dol"]);
        var gross_profit_ytd = Convert.ToDouble(gross["gross_ytd_dol"]);
        var mtd_pct = Convert.ToDouble(gross["gross_mtd_pct"]);
        var ytd_pct = Convert.ToDouble(gross["gross_ytd_pct"]);
        gc_gross_mtd.Attributes.Add("data-good", defaults["margin_mtd_good"].ToString());
        gc_gross_mtd.Attributes.Add("data-bad", defaults["margin_mtd_bad"].ToString());
        gc_gross_fytd.Attributes.Add("data-good", defaults["margin_fytd_good"].ToString());
        gc_gross_fytd.Attributes.Add("data-bad", defaults["margin_fytd_bad"].ToString());

        if (Math.Round(mtd_pct * 100) <= defaults["margin_mtd_bad"])
            {
            is_bad = true;
            gc_gross_mtd.Attributes["class"] += " b";
            }
        else if (Math.Round(mtd_pct * 100) >= defaults["margin_mtd_good"])
            {
            is_good = true;
            gc_gross_mtd.Attributes["class"] += " g";
            }
        else
            {
            gc_gross_mtd.Attributes["class"] += " n";
            }

        if (Math.Round(ytd_pct * 100) <= defaults["margin_fytd_bad"])
            {
            is_bad = true;
            gc_gross_fytd.Attributes["class"] += " b";
            }
        else if (Math.Round(ytd_pct * 100) >= defaults["margin_fytd_good"])
            {
            is_good = true;
            gc_gross_fytd.Attributes["class"] += " g";
            }
        else
            {
            gc_gross_fytd.Attributes["class"] += " n";
            }

        gc_gross_mtd.InnerHtml = "<div class='pct'>" + (mtd_pct == 0 ? "" : mtd_pct.ToString("P0")) + "</div><b>" +
                                 gross_profit_mtd.ToString("C0") + "</b>";
        gc_gross_fytd.InnerHtml = "<div class='pct'>" + (ytd_pct == 0 ? "" : ytd_pct.ToString("P0")) + "</div><b>" +
                                  gross_profit_ytd.ToString("C0") + "</b>";

        if (is_bad)
            {
            bm_tile_gross.Attributes["class"] += " b";
            }
        else if (is_good)
            {
            bm_tile_gross.Attributes["class"] += " g";
            }
        else
            {
            bm_tile_gross.Attributes["class"] += " n";
            }
        }
    }
}