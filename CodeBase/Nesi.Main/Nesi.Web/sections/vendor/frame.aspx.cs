using System;
using System.Web.UI.WebControls;
using nesi.core;

public partial class vendor_frame : System.Web.UI.Page
{
    NeMember myMember;
    private const int _page_id = 11; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        myMember = Toolbox.do_handle_authentication(_page_id);
        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		if(!string.IsNullOrEmpty(Request.QueryString["vendor_id"]) && !string.IsNullOrEmpty(Request.QueryString["first_tab"]))
			{
			frame.Attributes["src"]			= string.Format("./index.aspx?vendor_id={0}&first_tab={1}", Request.QueryString["vendor_id"], Request.QueryString["first_tab"]);
			}
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Vendor";
    }
}
