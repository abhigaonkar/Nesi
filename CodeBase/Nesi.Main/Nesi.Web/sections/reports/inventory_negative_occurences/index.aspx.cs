using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nesi.core;

public partial class sections_reports_inventory_negative_occurences_index : System.Web.UI.Page
{	Toolbox _tools;
	NeMember current_user;
	static string _page_id			= "103";
	static string _page_name		= "InventoryCounts";
    protected void Page_Load(object sender, EventArgs e)
    {
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(1);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
			lbltemp.Text = "Inventory Counts Report";
		
			var menu			= new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml	= menu.MenuHTML;
    }
}