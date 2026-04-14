using System;
using System.Web.UI.WebControls;
using nesi.core;

public partial class taskmanager : System.Web.UI.Page
{
	NeMember myMember;
	private const int _page_id = 35; // from Page table in DB
	
	Toolbox _tools;

	protected void Page_Init(object sender, EventArgs e)
	{
		
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
	}

	protected void Page_Load(object sender, EventArgs e)
	{

		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Task Manager";
		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		if (!IsPostBack && !IsCallback)
		{

		}

		//HtmlContainerControl frame = (HtmlContainerControl)I1;
		//frame.Attributes.Add("onload", "resizeIframe(this);");
		
	

	}
}
