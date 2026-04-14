using System;
using System.Web.UI.WebControls;
using nesi.core;

public partial class cust : System.Web.UI.Page
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
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Customer";
		if(Request.QueryString["customer_id"] != string.Empty)
			{
			frame.Attributes["src"]			= string.Format("./index.aspx?customer_id={0}", Request.QueryString["customer_id"]);
			}
    }
}
