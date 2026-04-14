using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using System.Text.RegularExpressions;
using nesi.core;

public partial class sections_hr_member_cellphones : Page
	{
	NeMember current_user;
	private const int _page_id = 164; // from Page table in DB
	private const string _page_name = "Cellphones";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxHiddenField h1;
	SqlDataSource ds_templates1;
	ASPxDropDownEdit dde_filter1;
	Panel panel_export1;
    public string _visible_bu; // = new Current_User().visible_business_units;

    private Toolbox _tools = new Toolbox();

	protected void Page_Init(object sender, EventArgs e)
		{

		var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);

		layout.__page_name = "Cellphones";
		layout.used_gv = gv_cellphones;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		h.Set("gridview_id", "gv_cellphones");
		ds_templates.SelectParameters["@page_name"].DefaultValue = "Cellphones";
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();

//		layout1.__page_name = "CellNumbers";
//		layout1.used_gv = gv_numbers;
//		h1 = (ASPxHiddenField)layout1.FindControl("h");
//		ds_templates1 = (SqlDataSource)layout1.FindControl("ds_templates");
//		dde_filter1 = (ASPxDropDownEdit)layout1.FindControl("dde_filter");
//		panel_export1 = (Panel)layout1.FindControl("panel_export");
//		panel_export1.Visible = true;
//		h1.Set("gridview_id", "gv_numbers");
//		ds_templates1.SelectParameters["@page_name"].DefaultValue = "CellNumbers";
//		ds_templates1.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();

		var lbltemp = (Label)Page.Master.FindControl("lblHeading");

		_tools.dont_cache_page();

		_visible_bu = new Current_User().visible_business_units;

    }
	protected void Page_Load(object sender, EventArgs e)
		{
		if (!IsPostBack)
			{
			var gl = new NeGridLayouts(current_user.id, "Cellphones");
			if (gl.GridLayoutID == 0)
				{
				gl.GridLayout_Layout = gv_cellphones.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = "gv_cellphones";
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_cellphones.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text = gl.GridLayout_Name;

			gl = new NeGridLayouts(current_user.id, "CellNumbers");
			if (gl.GridLayoutID == 0)
				{
				gl.GridLayout_Layout = gv_numbers.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = "gv_numbers";
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_numbers.LoadClientLayout(gl.GridLayout_Layout);
//				h1.Set("ID", gl.GridLayoutID);
//				h1.Set("NAME", gl.GridLayout_Name);
				}
	//		dde_filter1.Text = gl.GridLayout_Name;
			Session["gv_cellphones_grid"] = null;
			Session["gv_cellnumbers_grid"] = null;
			}
		fill_grid();
		}
	protected void fill_grid()
		{
		if (Session["gv_cellphones_grid"] == null)
			{

			var dt = _tools.getSQL_datatable(@" SELECT a.id, a.cellphone_status_id cellphone_status_id, e.status sim_status, 
e.cellphone_carrier_id, a.activedate, ifnull(b.member_id,'--') member_id ,
a.imei, a.business_unit_id, a.OS os, e.simcard, e.number, IFNULL(c.name, 'Inactive') cellphone_status_name,
d.id AS carrier_id, IFNULL(b.member_fullname, '--') AS member_name, 
IFNULL(f.ddl_name, '--') business_unit,
IFNULL(b.Member_Status, '--') member_status,
IFNULL(g.`status`, '--') hr_status, 
IFNULL(b.business_unit_id, '--') business_unit_id, 
a.last_modified, '' history 
FROM cellphone a LEFT JOIN member b ON b.cellphone_id = a.id LEFT JOIN 
cellphone_status c ON a.cellphone_status_id = c.id 
LEFT JOIN cellphone_carrier d ON a.cellphone_carrier_id = d.id
LEFT JOIN cellphone_number e on e.id = b.cellphone_number_id
LEFT JOIN business_unit f ON b.business_unit_id = f.id 
LEFT JOIN member_hrstatus g ON b.member_hrstatus_id = g.id
where ( find_in_set(b.business_unit_id,@v0) OR find_in_set(a.business_unit_id,@v0) OR a.business_unit_id is null) GROUP BY a.id ", new object[] { _visible_bu });
			Session["gv_cellphones_grid"] = dt;
			}
		gv_cellphones.DataSource = Session["gv_cellphones_grid"];
		gv_cellphones.DataBind();

		if (Session["gv_cellnumbers_grid"] == null)
			{
			var dt = _tools.getSQL_datatable(@" SELECT a.id, c.cellphone_status_id cellphone_status_id, 
a.status sim_status, a.cellphone_carrier_id, a.activedate, ifnull(b.member_id,'--') member_id ,
c.imei, a.business_unit_id, c.OS os, a.simcard, a.number,
IFNULL(d.name, 'Inactive') cellphone_status_name, e.id AS carrier_id, 
IFNULL(b.member_fullname, '--') AS member_name, IFNULL(f.ddl_name, '--') business_unit, 
IFNULL(b.Member_Status, '--') member_status, IFNULL(g.`status`, '--') hr_status, 
IFNULL(f.id, '--') business_unit_id, a.last_modified, '' history 
FROM cellphone_number a LEFT JOIN member b ON b.cellphone_number_id = a.id 
LEFT JOIN cellphone c ON c.id = b.cellphone_id 
LEFT JOIN cellphone_status d ON c.cellphone_status_id = d.id 
LEFT JOIN cellphone_carrier e ON a.cellphone_carrier_id = e.id 
LEFT JOIN business_unit f ON f.id = b.business_unit_id 
LEFT JOIN member_hrstatus g ON b.member_hrstatus_id = g.id
where (find_in_set(b.business_unit_id,@v0) OR find_in_set(a.business_unit_id,@v0) OR a.business_unit_id is null) GROUP BY a.id ", new object[] { _visible_bu });
			Session["gv_cellnumbers_grid"] = dt;
			}

		gv_numbers.DataSource = Session["gv_cellnumbers_grid"];
		gv_numbers.DataBind();


		}



	protected void gv_cellphones_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{

		}
	protected void gv_cellphones_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
		{
		if (e.Parameters == "AddEmp")
			{
			gv_cellphones.AddNewRow();

			}
		else if (e.Parameters == "AddApp")
			{


			}
		else
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
		}
	protected void gv_cellphones_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
		{
		if (!gv_cellphones.IsNewRowEditing)
			{

			gv_cellphones.SettingsText.PopupEditFormCaption = "Editing " + gv_cellphones.GetRowValuesByKeyValue(e.EditingKeyValue, "id");
			}
		}



	protected void gv_cellphones_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		}


	protected void gv_cellphones_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var c = new NECellphone(Convert.ToInt32(hdnid.Value));
		c.imei = e.NewValues["imei"].ToString();
		c.os = e.NewValues["os"].ToString();
		c.status_id = Convert.ToInt32(e.NewValues["cellphone_status_id"]);
		c.carrier_id = Convert.ToInt32(e.NewValues["carrier_id"]);
		c.last_update_member_id = Convert.ToInt32(current_user.id);
		c.last_modified = DateTime.Now;
		c.activedate = Convert.ToDateTime(e.NewValues["activedate"]);

		c.save();
		Session["gv_cellphones_grid"] = null;
		fill_grid();
		e.Cancel = true;
		gv_cellphones.CancelEdit();

		}
	protected void gv_cellphones_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{

		var c = new NECellphone();
		c.imei = e.NewValues["imei"].ToString();
		c.os = e.NewValues["os"].ToString();
		c.status_id = Convert.ToInt32(e.NewValues["cellphone_status_id"]);
		c.carrier_id = Convert.ToInt32(e.NewValues["carrier_id"]);
		c.activedate = Convert.ToDateTime(e.NewValues["activedate"]);
		c.last_update_member_id = Convert.ToInt32(current_user.id);
		c.business_unit_id = current_user.business_unit_id;
		c.save();



		Session["gv_cellphones_grid"] = null;
		fill_grid();
		e.Cancel = true;
		gv_cellphones.CancelEdit();
		}
	protected void gv_cellphones_HtmlEditFormCreated1(object sender, ASPxGridViewEditFormEventArgs e)
		{
		if (gv_cellphones.EditingRowVisibleIndex >= 0)
			{
			hdnid.Value = gv_cellphones.GetRowValues(gv_cellphones.EditingRowVisibleIndex, "id").ToString();
			}
		}
	public static string FormatPhone(string num)
		{
		//first we must remove all non numeric characters
		num = num.Replace("(", "").Replace(")", "").Replace("-", "");
		var results = string.Empty;
		var formatPattern = @"(\d{3})(\d{3})(\d{4})";
		results = Regex.Replace(num, formatPattern, "($1) $2-$3");
		//now return the formatted phone number
		return results;
		}

	protected void ASPxLabel1_PreRender(object sender, EventArgs e)
		{
		var l = (ASPxLabel)sender;
		l.Text = FormatPhone(l.Text);
		}

	protected void gv_numbers_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{

		}
	protected void gv_numbers_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
		{
		if (e.Parameters == "AddEmp")
			{
			gv_numbers.AddNewRow();

			}
		else if (e.Parameters == "AddApp")
			{


			}
		else
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
		}
	protected void gv_numbers_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
		{
		if (!gv_numbers.IsNewRowEditing)
			{

			gv_numbers.SettingsText.PopupEditFormCaption = "Editing " + gv_numbers.GetRowValuesByKeyValue(e.EditingKeyValue, "id");
			}
		}

	protected void gv_numbers_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		}

	protected void gv_numbers_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var c = new NECellphone_Number(Convert.ToInt32(hdnnid.Value));
		c.number = e.NewValues["number"].ToString();
		c.simcard = e.NewValues["simcard"].ToString();
		c.status = e.NewValues["sim_status"] == DBNull.Value ? "" : e.NewValues["sim_status"].ToString();
		c.carrier_id = Convert.ToInt32(e.NewValues["carrier_id"]);
		c.active_date = Convert.ToDateTime(e.NewValues["activedate"]);
		c.last_update_member_id = Convert.ToInt32(current_user.id);

		c.save();
		Session["gv_cellnumbers_grid"] = null;
		fill_grid();
		e.Cancel = true;
		gv_numbers.CancelEdit();

		}
	protected void gv_numbers_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{

		var c = new NECellphone_Number();
		if(e.NewValues["number"] == null)
			{
			throw new Exception("Please supply a phone number");
			}
		c.number = e.NewValues["number"].ToString();
		c.simcard = Toolbox.regex_only_numbers().Replace(e.NewValues["simcard"].ToString(), "");
		c.status = e.NewValues["sim_status"] == DBNull.Value ? "" : e.NewValues["sim_status"].ToString();
		c.active_date = Convert.ToDateTime(e.NewValues["activedate"]);
		c.carrier_id = Convert.ToInt32(e.NewValues["carrier_id"]);
		c.last_update_member_id = Convert.ToInt32(current_user.id);
		c.business_unit_id = current_user.business_unit_id;

		c.save();
		Session["gv_cellnumbers_grid"] = null;
		fill_grid();
		e.Cancel = true;
		gv_numbers.CancelEdit();
		}
	protected void gv_numbers_HtmlEditFormCreated1(object sender, ASPxGridViewEditFormEventArgs e)
		{
		if (gv_numbers.EditingRowVisibleIndex >= 0)
			{
			hdnnid.Value = gv_numbers.GetRowValues(gv_numbers.EditingRowVisibleIndex, "id").ToString();

			}
		}

	protected void gv_numbers_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		if (e.Keys[0] != null)
			{
			var id			= 0;
			int.TryParse(e.Keys[0].ToString(), out id);
			if(id > 0)
				{
				NECellphone_Number.delete(id);
				Session["gv_cellnumbers_grid"] = null;
				fill_grid();
				}
			}
		e.Cancel	= true;
		}
	protected void gv_cellphones_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		if (e.Keys[0] != null)
			{
			var id			= 0;
			int.TryParse(e.Keys[0].ToString(), out id);
			if(id > 0)
				{
				NECellphone.delete(id);
				Session["gv_cellphones_grid"] = null;
				fill_grid();
				}
			}
		e.Cancel	= true;
		}
	}