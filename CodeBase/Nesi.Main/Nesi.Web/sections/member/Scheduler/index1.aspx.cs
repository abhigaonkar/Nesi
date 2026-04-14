using System;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using nesi.core;

public partial class sections_member_scheduler_default : System.Web.UI.Page
{
	private int lastInsertedAppointmentId;

	public NeMember mymember;
	public Toolbox _tools;
	private static int _page_id = 108;  //Report Page
	private static Regex _numeric = new Regex(@"^\d+$");
	public string callback_control;

	protected void Page_Init()
	{
		_tools = new Toolbox();
		mymember = Toolbox.do_handle_authentication(_page_id);
		if (mymember.AuthenticatedForPrivilege(96) || _tools.getSQL_int(@"Select ifnull((select count(member_id) from member  where reports_to =@v0 and member_status = 'Active'),0)", new object[] { mymember.id })>0)  // if the person is not allowed to edit
		{
			dayview1.Visible = true;
			single_member_view1.Visible = false;
		}
		else
		{
			dayview1.Visible = false;
			single_member_view1.Visible = true;
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
	protected void Button1_Click(object sender, EventArgs e)
	{
		dayview1.Visible = false;

	}
}
 
