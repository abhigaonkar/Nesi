using System;
using System.Web.UI;
using nesi.core;

public partial class member_review_history_frame : Page
	{
		NeMember current_user;
		bool can_see_all;
		Toolbox _tools;
		int _page_id = 151;
		protected void Page_Load(object sender, EventArgs e)
		{

			_tools = new Toolbox();
			current_user = Toolbox.do_handle_authentication(151);  // ALL PEOPLE HAVE ACCESS TO THE REVIEW HISTORY PAGE
			var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
			can_see_all = current_user.AuthenticatedForPrivilege(144);
			iframe1.Attributes.Add("onload", "resizeIframe(this);");
			
			if (!IsPostBack)
			{
			}
		}

}