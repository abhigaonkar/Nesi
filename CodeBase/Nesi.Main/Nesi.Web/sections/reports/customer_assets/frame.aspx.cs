using System;
using nesi.core;

public partial class customer_assets_frame : System.Web.UI.Page
{
    NeMember myMember;
	static int _page_id = 126;
 

    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(_page_id);
        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		frame.Attributes["src"]			= string.Format("./index.aspx");
		frame.Attributes.Add("onload", "resizeIframe(this);");
    }
}
