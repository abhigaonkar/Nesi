using System;
using nesi.core;

public partial class reports_index : System.Web.UI.Page
{
    NeMember myMember;
    private const int _page_id = 51; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        
		var _tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();

        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);

        
        
    }
    
}
