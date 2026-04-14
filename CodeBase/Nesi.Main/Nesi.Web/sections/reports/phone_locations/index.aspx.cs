using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using nesi.core;

public partial class phone_locations : System.Web.UI.Page
	{
	NeMember myMember;
	private const int _page_id = 133; // from Page table in DB
	private const string _page_name = "phonelocations";
	Toolbox _tools = new Toolbox();
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;

    protected void Page_Init(object sender, EventArgs e)
    {
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
	//	myMember = Toolbox.do_handle_authentication("12");
		layout.__page_name					= _page_name;
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		layout.used_gv			= gv_gps;

		h.Set("gridview_id", "gv_gps");
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= myMember.id.ToString();
		_tools.dont_cache_page();
    }

	protected void Page_Load(object sender, EventArgs e)
		{
		_tools.dont_cache_page();
		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Phone Locations Report";

		if (!IsPostBack)
			{
			var gl	= new NeGridLayouts(myMember.id, _page_name);
			if(gl.GridLayoutID == 0)
				{
				gl.GridLayout_Layout		= "";
				gl.member_id	= myMember.id;
				gl.GridLayout_Name			= "Default";
				gl.GridLayout_Gridid		= _page_name;
				gl.SaveGridLayout();
				
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_gps.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text					= gl.GridLayout_Name;
			Session["master_gps_gv"] = null;
			}
		fill_grid();
		
		}


	protected void fill_grid()
	{
		if (Session["master_gps_gv"] == null)
		{
		


		}
		
		gv_gps.DataBind();

	}

	protected void Button2_Click(object sender, EventArgs e)
		{
		}

	protected void Button1_Click(object sender, EventArgs e)
		{
		gv_gps.CancelEdit();
		}


	protected void Aspxgridview1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		
		e.Cancel = true;

		gv_gps.CancelEdit();
		Session["master_gps_gv"] = null;
		fill_grid();
		}

	
	
	

	protected void LinkButton1_Click(object sender, EventArgs e)
		{
		var index = (((LinkButton) sender).NamingContainer as GridViewDataRowTemplateContainer).VisibleIndex;
//		object val = gv_MasterContacts.GetRowValues(index, "Contact_ID");
//		NECustomer progress = new NECustomer(Convert.ToInt32(val));
//		ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('../../customer/frame.aspx?Customer_id=" + progress.Customer_ID.ToString() + "&business_unit_id=" + progress.business_unit_id.ToString() + "','wo',950,800)", true);
		}

	protected void gv_MasterContacts_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void gv_MasterContacts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		if(e.Parameters != "")
			{
			gv.LoadClientLayout(e.Parameters);
			}
		else
			{
			gv.FilterExpression		= "";
			for(var i = 0; i < gv.Columns.Count; i++)
				{
				if (gv.Columns[i] is GridViewDataColumn)
					{
					var col = (GridViewDataColumn) gv.Columns[i];
					if (col.GroupIndex > -1)
						{
						gv.UnGroup(col);
						}
					col.Visible = true;
					}
				}
			}
		}


	protected void gv_MasterContacts_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex > 0)
		{
			
		}
	}
	protected void gv_gps_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{

		var _int = gv_gps.GetRowValues(gv_gps.EditingRowVisibleIndex,"longitude_db").ToString();
		var _lat = gv_gps.GetRowValues(gv_gps.EditingRowVisibleIndex, "latitude_db").ToString();
		var _title = "Location for " + gv_gps.GetRowValues(gv_gps.EditingRowVisibleIndex, "phone") + " at " + gv_gps.GetRowValues(gv_gps.EditingRowVisibleIndex, "timestamp_dt");
		var lb = (ASPxLabel)gv_gps.FindEditFormTemplateControl("_title");
		lb.Text = _title;
		var frame = (HtmlContainerControl)gv_gps.FindEditFormTemplateControl("editframe");
		frame.Attributes.Add("src", "//maps.google.ca/maps?q=" + _lat + "," + _int + "&hl=en&ll=" + Convert.ToString(Convert.ToDouble(_lat)-0.001) + "," + Convert.ToString(Convert.ToDouble(_int)-0.001) + "&spn=0.026148,0.038581&sll=49.303974,-84.738438&sspn=24.093163,39.506836&t=m&z=17&iwloc=near&output=embed");
//	http://maps.google.ca/maps?q=43.43102346,-79.78233923&hl=en&ll=43.431014,-79.782329&spn=0.026148,0.038581&sll=49.303974,-84.738438&sspn=24.093163,39.506836&t=m&z=15&iwloc=near				
//	http://maps.google.ca/maps?q=43.43102346,-79.78233923&hl=en&ll=43.430983,-79.782372&spn=0.10459,0.154324&sll=49.303974,-84.738438&sspn=24.093163,39.506836&t=m&z=13&iwloc=near
		
		gv_gps.SettingsText.PopupEditFormCaption = _title;
	}
}
