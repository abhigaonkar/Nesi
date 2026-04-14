using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using nesi.core;

public partial class root_page : System.Web.UI.Page
	{
    NeMember current_user;

	private static Regex _numeric	= new Regex(@"^\d+$");
    protected void Page_Load(object sender, EventArgs e)
		{
		var _q			= Request.QueryString;
		var _page_id					= 0;
		if(_q["page_id"] != null && _numeric.Match(_q["page_id"]).Success)
			{
			int.TryParse(_q["page_id"], out _page_id);
			current_user				= Toolbox.do_handle_authentication(_page_id);
			
			var menu						= new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml				= menu.MenuHTML;
			divSide.InnerHtml				= shared.PrintSidePanelHTML(current_user);
			var lbltemp					= (Label)Page.Master.FindControl("lblHeading");
			lbltemp.Text				= Toolbox.doSQL_string(@"SELECT Page_Desc FROM page  WHERE Page_Id =@v0", new object[] { _page_id });
			}
		else
			{
			Response.Redirect("/default.aspx");
			}
		
		}        
	}
