using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using nesi.core;

public partial class dashboard_modules_labor : System.Web.UI.Page
{
	NeMember user;
	private const int _page_id = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
		var _tools			= new Toolbox();
		var _q	= Request.QueryString;
		user					= Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();
		var business_unit_id			= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		if(business_unit_id == 0)
			{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
			}
		var c							= new NeBusinessUnit(business_unit_id);
		var data								= Toolbox.doSQL_dt(@" SELECT * FROM dashboard_thresholds WHERE type = 'labor' AND member_id = @v0  AND business_unit_id = @v1 ", new object[] {  user.id, user.business_unit_id  } );

		var defaults				= new Dictionary<string,int>();
		defaults.Add("lab_mtd_good", 0);
		defaults.Add("lab_mtd_bad", 0);
		defaults.Add("lab_fytd_good", 0);
		defaults.Add("lab_fytd_bad", 0);
		defaults.Add("lab_btm_good", 0);
		defaults.Add("lab_btm_bad", 0);
		defaults.Add("lab_ftm_good", 0);
		defaults.Add("lab_ftm_bad", 0);

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
		var labor								= Toolbox.doSQL_dt(@" SELECT lab_mtd_dol, lab_mtd_pct, lab_bmtd_dol, lab_ytd_dol, lab_ytd_pct, lab_bytd_dol, lab_bmtd_pct, lab_bytd_pct FROM dashboard_snapshot_daily WHERE business_unit_id = @v0  ORDER BY dt DESC ", new object[] {  c.id } );
		if(labor.Rows.Count > 0)
			{
			var dr					= labor.Rows[0];
			#region MTD
			var mtd					= Convert.ToDouble(dr["lab_mtd_dol"]);
			var mtd_pct				= Convert.ToDouble(dr["lab_mtd_pct"]);
			var ytd					= Convert.ToDouble(dr["lab_ytd_dol"]);
			var fytd_pct				= Convert.ToDouble(dr["lab_ytd_pct"]);
			var btm					= Convert.ToDouble(dr["lab_bmtd_dol"]);
			var btm_pct				= Convert.ToDouble(dr["lab_bmtd_pct"]);
			var ftm					= Convert.ToDouble(dr["lab_bytd_dol"]);
			var ftm_pct				= Convert.ToDouble(dr["lab_bytd_pct"]);

			gc_labor_mtd.Attributes.Add("data-good", defaults["lab_mtd_good"].ToString());
			gc_labor_mtd.Attributes.Add("data-bad", defaults["lab_mtd_bad"].ToString());
			if(Math.Round(mtd_pct*100) <= defaults["lab_mtd_bad"])
				{
				is_bad									= true;
				gc_labor_mtd.Attributes["class"]		+= " b";
				}
			else if(Math.Round(mtd_pct*100) >= defaults["lab_mtd_good"])
				{
				is_good									= true;
				gc_labor_mtd.Attributes["class"]		+= " g";
				}
			else
				{
				gc_labor_mtd.Attributes["class"]		+= " n";
				}
			gc_labor_mtd.InnerHtml		= "<div class='pct'>"+(mtd_pct == 0 ? "" : mtd_pct.ToString("P0"))+"</div><b>"+mtd.ToString("C0")+"</b>";
			#endregion MTD
			#region YTD
			gc_labor_fytd.Attributes.Add("data-good", defaults["lab_fytd_good"].ToString());
			gc_labor_fytd.Attributes.Add("data-bad", defaults["lab_fytd_bad"].ToString());
			if(Math.Round(fytd_pct*100) <= defaults["lab_fytd_bad"])
				{
				is_bad									= true;
				gc_labor_fytd.Attributes["class"]		+= " b";
				}
			else if(Math.Round(fytd_pct*100) >= defaults["lab_fytd_good"])
				{
				is_good									= true;
				gc_labor_fytd.Attributes["class"]		+= " g";
				}
			else
				{
				gc_labor_fytd.Attributes["class"]		+= " n";
				}
			gc_labor_fytd.InnerHtml		= "<div class='pct'>"+(fytd_pct == 0 ? "" : fytd_pct.ToString("P0"))+"</div><b>"+ytd.ToString("C0")+"</b>";
			#endregion YTD
			#region BTM
			gc_labor_btm.InnerHtml		= "<div class='pct'>"+(btm_pct == 0 ? "" : btm_pct.ToString("P0"))+"</div><b>"+btm.ToString("C0")+"</b>";
			#endregion BTM
			#region FTM
			gc_labor_ftm.InnerHtml		= "<div class='pct'>"+(ftm_pct == 0 ? "" : ftm_pct.ToString("P0"))+"</div><b>"+ftm.ToString("C0")+"</b>";
			#endregion FTM
			if(is_bad)
				{
				bm_tile_labor.Attributes["class"]		+= " b";
				}
			else if(is_good)
				{
				bm_tile_labor.Attributes["class"]		+= " g";
				}
			else
				{
				bm_tile_labor.Attributes["class"]		+= " n";
				}
			}
    }
}