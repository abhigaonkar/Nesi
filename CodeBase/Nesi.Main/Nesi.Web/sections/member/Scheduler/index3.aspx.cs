using System.Text.RegularExpressions;
using NESI.Common.Models;
using nesi.core;

public partial class sections_member_scheduler_default3 : System.Web.UI.Page
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
		if(!mymember.AuthenticatedForPrivilege(OpsPrivilege.WOPlanner))
			{
			Toolbox.FriendlyException(Response, "You do not have access to this page", "");
			}
		if (!Page.IsPostBack)
		{
			
		}
	}
	protected void Page_Load()
	{
		
		if (!IsPostBack)
		{
		
		}
		
		
	}
}




