using System;
using System.Web.UI.WebControls;
using nesi.core;

public partial class sections_admin_androidReleaseVersion_index : System.Web.UI.Page
{

    Toolbox _tools;
    private NeMember current_user;
    private const int _page_id = 201;

    protected void Page_Load(object sender, EventArgs e)
    {
        _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(_page_id);

        var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
        var lbltemp = (Label)Page.Master.FindControl("lblHeading");
        lbltemp.Text = "Nesi App Release Versions";
    }

}