using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using nesi.core;

public partial class Emailed_Account_Statements_frame : Page
	{

	private NeMember myMember;
	private const int _page_id = 199; // from Page table in DB
	private const string _page_name = "emailed_account_statements";
	Toolbox _tools;
	JavaScriptSerializer jSON = new JavaScriptSerializer();

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		
		if (Session["working_business_unit_id"] != null)
			{
			Session.Remove("working_business_unit_id");
			}
		_tools.dont_cache_page();
		if (Cache["ds_depend"] == null)
			{
			Cache["ds_depend"] = DateTime.Now;
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var _q = Request.QueryString;
		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Emailed Customer Account Statements";

		}
}
