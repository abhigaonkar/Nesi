using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using System.Web.Script.Serialization;
using nesi.core;

public partial class sections_reports_phone_comments_index : System.Web.UI.Page
{

    Toolbox _tools;
    NeMember current_user;
    JavaScriptSerializer jSON = new JavaScriptSerializer();
    static int _page_id = 203;
    static string _page_name = "Master Phone call Grid";

    public string woprogid_notes;
    public string DSN;

    protected void Page_Init(object sender, EventArgs e)
    {
        _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(_page_id);


    }


    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(_page_id);
        //	_tools.dont_cache_page();
        var menu = new NeMenu(current_user, _page_id);
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
        var lbltemp = (Label)Page.Master.FindControl("lblHeading");
        lbltemp.Text = _page_name;


    }
}