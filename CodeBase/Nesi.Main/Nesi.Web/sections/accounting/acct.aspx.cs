using System;
using nesi.core;

public partial class acct : System.Web.UI.Page
{
    NeMember myMember;
	Toolbox _tools;
    private const int _page_id = 4; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        
		_tools		= new Toolbox();
   		myMember	= Toolbox.do_handle_authentication(_page_id);
		((IntraDefault)this.Master).page_name		= NePage.get_page_name(_page_id);
        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
        
        
    }
    
}
