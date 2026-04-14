using System;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using nesi.core;

public partial class sections_member_scheduler_index4 : System.Web.UI.Page
{
	private int lastInsertedAppointmentId;

	public NeMember mymember;
	public Toolbox _tools;
	private static int _page_id = 183;  //Report Page
	private static Regex _numeric = new Regex(@"^\d+$");
	public string callback_control;
	private bool can_edit = false;

	protected void Page_Init()
	{
		_tools = new Toolbox();
		mymember = Toolbox.do_handle_authentication(_page_id);  // allowed to see on call schedule
		can_edit = mymember.AuthenticatedForPrivilege(179);
		if (!Page.IsPostBack)
		{
			
		}
	}
	protected void Page_Load()
	{
		
		if (!IsPostBack)
		{
			var menu = new NeMenu(mymember, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			divSide.InnerHtml = shared.PrintSidePanelHTML(mymember);
			var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		}
		
		
	}
}




