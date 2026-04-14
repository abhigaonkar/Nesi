using System;
using System.Web.UI.WebControls;
using nesi.core;

public partial class sections_workorder_job_tagging : System.Web.UI.Page
	{
	int _page_id			= 182;
	Toolbox _tools;
	NeMember current_user;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var menu = new NeMenu(current_user, (int) _page_id);
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		hid_member_id.Value		= current_user.id.ToString();
		}
	}