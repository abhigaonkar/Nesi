using System;
using System.Web.UI.WebControls;
using nesi.core;

public partial class sections_member_inventory_tocreate : System.Web.UI.Page
	{
	NeMember myMember;
	private const int _page_id = 116;
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		myMember			= Toolbox.do_handle_authentication(_page_id);
		var menu			= new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml	= menu.MenuHTML;
		divSide.InnerHtml	= shared.PrintSidePanelHTML(myMember);
		var lbltemp		= (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text		= "Make This Part";
		ds_tocreate.SelectParameters[0].DefaultValue =  new Current_User().visible_business_units;

		}
	}