using System;
using nesi.core;

public partial class sections_messaging_videos : System.Web.UI.Page
	{
    NeMember myMember;
    private const int _page_id = 153; // from Page table in DB
	protected void Page_Init(object sender, EventArgs e)
		{
        var _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(_page_id);
        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
		((IntraDefault)this.Master).page_name		= NePage.get_page_name(_page_id);
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		if(!myMember.AuthenticatedForPrivilege(151))
			{
			p.TabPages[2].Visible		= false;
			p.TabPages[1].Visible		= false;
			p.ShowTabs					= false;
			}
		}
	}