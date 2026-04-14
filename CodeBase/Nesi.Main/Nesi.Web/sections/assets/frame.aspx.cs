using System;
using nesi.core;

public partial class sections_assets_frame : System.Web.UI.Page
{
    NeMember myMember;
    private const int _page_id = 10; // from Page table in DB
    private const string _add_id = "9"; // from Privilege table in DB - Add Customer
    private const string _edit_id = "10"; // from Privilege table in DB - Edit Customer

    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(_page_id);
        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
	//frame_assets.Attributes["src"]=	 String.Format("./index.aspx");
//			.Attributes["src"]			=
			
    }
}
