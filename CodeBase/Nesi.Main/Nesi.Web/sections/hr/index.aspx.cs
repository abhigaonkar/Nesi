using System;
using nesi.core;

public partial class hr_index : System.Web.UI.Page
{
    NeMember myMember;
    private const int _page_id = 47; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        
        myMember = Toolbox.do_handle_authentication(_page_id);

        var menu = new NeMenu(myMember, _page_id);
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);

        
        
    }
    
}
