using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_reports_nesi_error_log_index : System.Web.UI.Page
	{
		NeMember current_user;
		private const int _page_id = 195; // from Page table in DB
		private const string _page_name = "Nesi Error Log";
		Toolbox _tools = new Toolbox();
		ASPxHiddenField h;
		SqlDataSource ds_templates;
		ASPxDropDownEdit dde_filter;
		Panel panel_export;
		bool can_view_dollar_totals = false;
		public int ddl_selected;
		NameValueCollection _q;
		bool can_edit_reps = false;

		protected void Page_Init(object sender, EventArgs e)
		{
			_tools = new Toolbox();
			//	myMember = Toolbox.do_handle_authentication(_page_id);
			current_user = Toolbox.do_handle_authentication(_page_id);
			_q = Request.QueryString;





			
			var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
			var lbltemp = (Label)Page.Master.FindControl("lblHeading");
			lbltemp.Text = "Nesi Error log Report";

            crash_log.isMessageBoard = false;
		
		}
	



}
