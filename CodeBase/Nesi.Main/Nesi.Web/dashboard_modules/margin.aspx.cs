using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using nesi.core;

public partial class dashboard_modules_margin : System.Web.UI.Page
{
	NeMember user;
	private const int _page_id = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
		var _tools			= new Toolbox();
		_tools.dont_cache_page();
		var _q	= Request.QueryString;
		user					= Toolbox.do_handle_authentication(_page_id);
		var business_unit_id			= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		
		if(business_unit_id == 0)
			{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
			}
		var c							= new NeBusinessUnit(business_unit_id);
		var data								= Toolbox.doSQL_dt(@" SELECT * FROM dashboard_thresholds WHERE type = 'margin' AND member_id = @v0  AND business_unit_id = @v1 ", new object[] {  user.id, user.business_unit_id  } );

		var defaults				= new Dictionary<string,int>();
		defaults.Add("margin_total_good", 0);
		defaults.Add("margin_total_bad", 0);

		defaults.Add("margin_tm_good", 0);
		defaults.Add("margin_tm_bad", 0);

		defaults.Add("margin_quoted_good", 0);
		defaults.Add("margin_quoted_bad", 0);


		if(data.Rows.Count > 0)
			{
			foreach(DataRow datar in data.Rows)
				{
				var _subtype						= datar["subtype"].ToString();
				var good							= Convert.ToInt32(datar["good_threshold"]);
				var bad								= Convert.ToInt32(datar["bad_threshold"]);
				defaults[_subtype+"_good"]			= good;
				defaults[_subtype+"_bad"]			= bad;
				}
			}
		var is_bad									= false;
		var is_good								= false;
		
		gc_total_margin.Attributes.Add("data-good",		defaults["margin_total_good"].ToString());
		gc_total_margin.Attributes.Add("data-bad",		defaults["margin_total_bad"].ToString());
		
		gc_total_TM.Attributes.Add("data-good",			defaults["margin_tm_good"].ToString());
		gc_total_TM.Attributes.Add("data-bad",			defaults["margin_tm_bad"].ToString());
		
		gc_total_quoted.Attributes.Add("data-good",		defaults["margin_quoted_good"].ToString());
		gc_total_quoted.Attributes.Add("data-bad",		defaults["margin_quoted_bad"].ToString());
	
		var total_margin						= Toolbox.doSQL_double(string.Format(@" SELECT ROUND(IFNULL(((SUM(woprog_stilltobebilled)) - SUM(woprog_laborcost)-SUM(woprog_materialcost)),0), 2) 
tmmargin FROM woprog WHERE business_unit_id = @v0  AND woprog_closedatetime IS NULL AND woprog_bvwo != 'Not Entered' AND  woprog_status 
IN ('Initial Prep','Open','Questions For PM','Rework','Waiting BM Approval','Waiting For PO','Waiting PM Approval','Open Vendor POs')"), new object[] {  c.id  } );

		var tm_margin						= Toolbox.doSQL_double(string.Format(@" SELECT ROUND(IFNULL(((SUM(woprog_stilltobebilled)) - SUM(woprog_laborcost)-SUM(woprog_materialcost)),0), 2) tmmargin 
FROM woprog WHERE business_unit_id = @v0  AND  woprog_quoteid = 0 AND woprog_closedatetime IS NULL AND woprog_bvwo != 'Not Entered' AND woprog_status 
IN ('Initial Prep','Open','Questions For PM','Rework','Waiting BM Approval','Waiting For PO','Waiting PM Approval','Open Vendor POs')"), new object[] {  c.id } );
		var quoted_margin					= total_margin - tm_margin;


			#region Total
			if(total_margin <= defaults["margin_total_bad"])
				{
				is_bad										= true;
				gc_total_margin.Attributes["class"]			+= " b";
				}
			else if(total_margin >= defaults["margin_total_good"])
				{
				is_good										= true;
				gc_total_margin.Attributes["class"]			+= " g";
				}
			else
				{
				gc_total_margin.Attributes["class"]			+= " n";
				}
			gc_total_margin.InnerHtml		= "<b>"+total_margin.ToString("C0")+"</b>";
			#endregion Total
			#region TM
			if(tm_margin <= defaults["margin_tm_bad"])
				{
				is_bad										= true;
				gc_total_TM.Attributes["class"]				+= " b";
				}
			else if(tm_margin >= defaults["margin_tm_good"])
				{
				is_good										= true;
				gc_total_TM.Attributes["class"]				+= " g";
				}
			else
				{
				gc_total_TM.Attributes["class"]				+= " n";
				}
			gc_total_TM.InnerHtml							= "<b>"+tm_margin.ToString("C0")+"</b>";			
			#endregion TM
			#region Quoted Margin
			if(quoted_margin <= defaults["margin_quoted_bad"])
				{
				is_bad										= true;
				gc_total_quoted.Attributes["class"]			+= " b";
				}
			else if(quoted_margin >= defaults["margin_quoted_good"])
				{
				is_good										= true;
				gc_total_quoted.Attributes["class"]			+= " g";
				}
			else
				{
				gc_total_quoted.Attributes["class"]			+= " n";
				}
			gc_total_quoted.InnerHtml						= "<b>"+quoted_margin.ToString("C0")+"</b>";			
			#endregion  Quoted Margin
			if(is_bad)
				{
				bm_tile_margin.Attributes["class"]		+= " b";
				}
			else if(is_good)
				{
				bm_tile_margin.Attributes["class"]		+= " g";
				}
			else
				{
				bm_tile_margin.Attributes["class"]		+= " n";
				}
    }
}