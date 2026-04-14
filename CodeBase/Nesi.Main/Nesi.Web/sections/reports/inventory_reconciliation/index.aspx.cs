using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class sections_reports_inventory_reconciliation_index : Page
	{
	Toolbox _tools;
	NeMember current_user;
	static int _page_id			= 131;
	static string _page_name = "inventory_reconciliation";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	bool can_see_cost;
	
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		current_user						= Toolbox.do_handle_authentication(_page_id);
		can_see_cost						= current_user.AuthenticatedForPrivilege(58);
		layout.__page_name					= _page_name;
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		h.Set("gridview_id", "gv_counts");
		layout.used_gv						= gv_counts;
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= current_user.id.ToString();
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Inventory Quantity Reconciliation";
        var dt = Toolbox.doSQL_dt(@"Call get_visible_business_units(@v0 )", new object[] {  current_user.id } );
        dt.DefaultView.Sort = "[ddl_name] ASC";
        ddlCompany.DataSource = dt;
        ddlCompany.DataBind();
		if(!IsCallback && !IsPostBack)
			{
            ddlCompany.Value = current_user.business_unit_id;
			dte_end.Date = System.DateTime.Today;
			dte_start.Date = System.DateTime.Today.AddMonths(-1);
			}
		}
	protected void Page_Load(object sender, EventArgs e)
	    {
        if (!IsCallback && !IsPostBack)
			{
			var menu			= new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml	= menu.MenuHTML;
			var gl	= new NeGridLayouts(current_user.id, _page_name);
			if(gl.GridLayoutID == 0)
			    {
                gl.GridLayout_Layout		= gv_counts.SaveClientLayout();
				gl.member_id	= current_user.id;
				gl.GridLayout_Name			= "Default";
				gl.GridLayout_Gridid		= _page_name;
				gl.SaveGridLayout();
				
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_counts.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text					= gl.GridLayout_Name;

			}
		load_grid();
		}

	protected void gv_counts_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void gv_counts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		if(e.Parameters != "")
			{
			gv.LoadClientLayout(e.Parameters);
			}
		else
			{
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

    private void load_grid()
		{
		//Toolbox.do_debug(@"load_grid - Start");
		gv_counts.DataSource = Toolbox.doSQL_dt(@"CALL get_inv_atdate(@v0 , @v1 ,@v2 , 0)", new object[] {  ddlCompany.Value, Toolbox.MySQL_shortdt(dte_start.Date), Toolbox.MySQL_shortdt(dte_end.Date) } );
		gv_counts.DataBind();
		//Toolbox.do_debug(@"load_grid - End");
		}


	protected void ASPxButton2_Click(object sender, EventArgs e)
		{
		}
	protected void gv_counts_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		if (gv_counts.EditingRowVisibleIndex >= 0)
			{
			var x = gv_counts.EditingRowVisibleIndex;
			var frame = (HtmlContainerControl)gv_counts.FindEditFormTemplateControl("I1");

            var working_businesss_unit = new NeBusinessUnit(ddlCompany.Value);
            var warehouse_business_unit = new NeBusinessUnit(working_businesss_unit.warehouse_bu_id);

			frame.Attributes.Add("src", "../../member/inventory/history.aspx?master_id=" + gv_counts.GetRowValues(x, "x") + "&business_unit_id=" + warehouse_business_unit.id);
			}
		}
	protected void gv_counts_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
		{
		gv_counts.SettingsText.PopupEditFormCaption = "Transactions for: " + e.EditingKeyValue;
		}
	protected void pc_Callback(object sender, CallbackEventArgsBase e)
		{

		}
	protected void gv_counts_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if(e.DataColumn.FieldName == "qty_diff_percent" && e.VisibleIndex >= 0)
			{
			double st	= 0;
			double en	= 0;
			double cv	= 0;
			double.TryParse(gv_counts.GetDataRow(e.VisibleIndex)["_start"].ToString(), out st);
			double.TryParse(gv_counts.GetDataRow(e.VisibleIndex)["_end"].ToString(), out en);
			double.TryParse(e.CellValue.ToString(), out cv);
			if(st == -4)
				{
				var xx = st;
				}
			if(cv == 0 && st == 0 && en != 0) // we have an issue
				{
				cv = en;
				}
			if(st > en)
				{
				e.Cell.Text = "("+Math.Abs(cv).ToString("N2")+"%)";
				}
			else if(st < en)
				{
				e.Cell.Text = Math.Abs(cv).ToString("N2")+"%";
				}
			else if(st == en)
				{
				e.Cell.Text = "0.00%";
				}
			else
				{
				e.Cell.Text = cv+"%";
				}
			}
		}
}
