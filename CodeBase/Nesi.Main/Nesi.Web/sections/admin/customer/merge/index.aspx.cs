using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_admin_customer_merge_index : System.Web.UI.Page
	{
	Toolbox _tools;
	public NeMember current_user;
	int _page_id = 88;
	// http://localhost:61114/sections/admin/customer/merge/index.aspx
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		current_user						= Toolbox.do_handle_authentication(_page_id);
		if(!IsPostBack && !IsCallback)
			{
			Cache["ds_depend"]				= Toolbox.do_RandomString(10);
			}
		}

	protected void Page_Load(object sender, EventArgs e)
		{
		var menu							= new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(current_user);
		var lbltemp						= (Label)Page.Master.FindControl("lblHeading");
		}

	protected void gv_duplicatephones_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		var IDs = new object[gv.VisibleRowCount];
		for (var i = 0; i < gv.VisibleRowCount; i++)
			{
			IDs[i] = gv.GetRowValues(i, "ACID");
			}
		e.Properties["cpids"] = IDs;
		}
	protected void gv_duplicatenames_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		var IDs = new object[gv.VisibleRowCount];
		for (var i = 0; i < gv.VisibleRowCount; i++)
			{
			IDs[i] = gv.GetRowValues(i, "customer_id");
			}
		e.Properties["cpids"] = IDs;
		}
	}
