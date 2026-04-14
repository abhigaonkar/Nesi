using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using nesi.core;

public partial class sections_dashboards_frame : System.Web.UI.Page
{
	
	NeMember current_user;
	private int page_id = 152;
	Toolbox _tools;
	protected NameValueCollection _q;
	protected void Page_Init()
	{
		_q = Request.QueryString;
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));
		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var _q = Request.QueryString;

		if (ASPxCheckBox1.Checked)
		{
			frm.Attributes.Add("src", "index2.aspx");
			frm.Attributes.Add("onload", "resizeIframe(this);");
		}
		else
		{
//			frm.Attributes.Add("src", "index.aspx");
//			frm.Attributes.Add("onload", "resizeIframe(this);");
			Response.Redirect("index.aspx");
		}
	}

}
