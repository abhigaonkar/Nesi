using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Data;
using System.Linq;
using DevExpress.Web;
using System.Web.Services;
using System.Web;
using nesi.core;

public partial class sections_member_inventory_marketing_catalog : Page
	{
	Toolbox _tools;
	NeMember current_user;
	static int _page_id = 205;

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		var menu = new NeMenu(current_user, _page_id);
		divMenu.InnerHtml = menu.MenuHTML;
		frame.Attributes.Add("src", "https://newelectric.goepower.com/");
	//	frame.Attributes.Add("onload", "resizeIframe(this);");
		}
	protected void Page_Load(object sender, EventArgs e)
		{
			
		}
}
