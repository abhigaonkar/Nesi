using System;
using System.Collections.Specialized;
using nesi.core;

public partial class dashboard_modules_inventory : System.Web.UI.Page
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
		var c				= new NeBusinessUnit(business_unit_id);
		var inventory_balance = _tools.getSQL_double(@"Select sum(ROUND(a.dollar_balance,2)) from inventory_branch a LEFT JOIN inventory_item_master b using (master_id) LEFT JOIN inventory_tag c USING (tag_id)  where b.active = true AND c.is_exclude = false AND a.business_unit_id =@v0", new object[] { c.warehouse_bu_id });

		gc_inventory.InnerHtml = "<b>" + inventory_balance.ToString("C2") + "</b";
		gc_inventory_b.InnerHtml = "0";
	

    }
}