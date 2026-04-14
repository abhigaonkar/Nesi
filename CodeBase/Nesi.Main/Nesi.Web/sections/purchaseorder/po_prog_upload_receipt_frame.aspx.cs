using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Script.Serialization;
using System.IO;
using nesi.core;
using NESI.Common.Models;

public partial class sections_member_po_upload_receipt_frame : System.Web.UI.Page
{
	NeMember myMember;
	private const int _page_id = 264; // from Page table in DB
	private const string _page_name = "Branch Packing Slip Upload";
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	Toolbox _tools;
	bool is_admin = false;


	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(1);
		is_admin = myMember.AuthenticatedForPrivilege(213);
		_tools.dont_cache_page();

		if (!myMember.AuthenticatedForPage(264))
		{
			Toolbox.FriendlyException(this.Response, "Sorry, you do not  have access to this po upload page.", @"\sections\purchaseorder\po_prog_edit.aspx");

		}

		if (Session["working_business_unit_id"] == null)
		{
			Session["working_business_unit_id"] = myMember.business_unit_id.ToString();
		}


	}
	protected void Page_Load(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		_tools.dont_cache_page();
		#region Load Summary Gridview
		if (!IsCallback && !IsPostBack)
		{

			var gl = new NeGridLayouts(myMember.id, string.Format("{0}", _page_name));
			var temp_company = new NeBusinessUnit(Session["working_business_unit_id"]);


		}
		#endregion

		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);

	}




}

