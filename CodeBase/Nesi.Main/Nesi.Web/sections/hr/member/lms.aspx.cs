using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using nesi.core;

public partial class member_lms : Page
	{
	NeMember current_user;
	private const int _page_id = 160; // from Page table in DB
	private const string _page_name = "lms";
	private Toolbox _tools = new Toolbox();

	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
				
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		}
	protected void Page_Load(object sender, EventArgs e)
		{
			if_lms.Attributes.Add("src", "http://lms.nesi.ca");
			
		
		}
	
}