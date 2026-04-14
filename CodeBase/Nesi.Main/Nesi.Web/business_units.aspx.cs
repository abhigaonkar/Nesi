using DevExpress.Web;
using nesi.core;
using System;
using System.Web.UI.WebControls;
using NESI.Common.Models;

public partial class business_units : System.Web.UI.Page
	{
	private NeMember currentUser;

	static string _page_name = "Business_Units";
	private ASPxHiddenField h;
	private SqlDataSource ds_templates;
	private ASPxDropDownEdit dde_filter;
	private Panel panel_export;
	private bool canEditBusinessUnit;
	private bool canEditPOApprovalSettings;
	private bool canEditChargeouts;
	private bool canEditTargets;
	private bool canClickBusinessUnitLink;

	protected void Page_Init(object sender, EventArgs e)
		{
		currentUser = Toolbox.do_handle_authentication(OpsPage.BusinessUnits);
		canEditBusinessUnit = currentUser.AuthenticatedForPrivilege(OpsPrivilege.EditBranch);
		canEditPOApprovalSettings = currentUser.AuthenticatedForPrivilege(OpsPrivilege.POApprovalSettingsTab);
		canEditChargeouts = currentUser.AuthenticatedForPrivilege(OpsPrivilege.AllowVisibilityToResetCopyChargeouts);
		canEditTargets = currentUser.AuthenticatedForPrivilege(OpsPrivilege.EditTargets);
		canClickBusinessUnitLink = canEditBusinessUnit || canEditPOApprovalSettings || canEditChargeouts || canEditTargets;

		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;

		layout.used_gv = gv;
		h.Set("gridview_id", "gv");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = currentUser.id.ToString();
		sqlGrid.SelectParameters["@member_id"].DefaultValue = currentUser.id.ToString();
		}


	protected void Page_Load(object sender, EventArgs e)
		{
		var masterPage = Master;
		if (masterPage != null) ((IntraDefault)masterPage).page_name = NePage.get_page_name(OpsPage.BusinessUnits);

		if(IsCallback || IsPostBack) return;
		var gl = new NeGridLayouts(currentUser.id, _page_name);
		if (gl.GridLayoutID == 0)
			{
			gv.FilterExpression  = "";
			gl.GridLayout_Layout = gv.SaveClientLayout();
			gl.member_id         = currentUser.id;
			gl.GridLayout_Name   = "Default";
			gl.GridLayout_Gridid = _page_name;
			gl.SaveGridLayout();

			h.Set("ID", gl.GridLayoutID);
			h.Set("NAME", gl.GridLayout_Name);
			}
		else
			{
			gv.LoadClientLayout(gl.GridLayout_Layout);
			h.Set("ID", gl.GridLayoutID);
			h.Set("NAME", gl.GridLayout_Name);
			}
		dde_filter.Text = gl.GridLayout_Name;
		}
		
	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		}

	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		if (e.Parameters != "")
			{
			gv.LoadClientLayout(e.Parameters);
			}
		else
			{
			gv.FilterExpression = "";
			for (var i = 0; i < gv.Columns.Count; i++)
				{
				if (gv.Columns[i] is GridViewDataColumn)
					{
					var col = (GridViewDataColumn)gv.Columns[i];
					if (col.GroupIndex > -1)
						{
						gv.UnGroup(col);
						}
					col.Visible = true;
					}
				}
			}
		}

	protected void gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		}

	protected void ASPxHyperLink1_OnDataBound(object _sender, EventArgs _e)
		{
		var link = (ASPxHyperLink) _sender;
		link.Enabled = canClickBusinessUnitLink;
		}
	}





