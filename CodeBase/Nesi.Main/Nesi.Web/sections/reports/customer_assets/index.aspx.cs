using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class sections_reports_customer_assets_index : Page
{
	Toolbox _tools;
	NeMember current_user;
	static int _page_id = 126;
	static string _page_name = "CustomerAssets";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;
	public string woprogid_notes;
	protected NameValueCollection _q;
	string customer_id;
	int address_id = 0;
	string origin = null;
	protected void Page_Init(object sender, EventArgs e)
	{
		_q = Request.QueryString;
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		if ((_q["customer_id"] == null) && (!current_user.isContact))
		{
			customer_id = "0";
		}
		else
		{
			if (_q["customer_id"] != null)
			{
				customer_id = _q["customer_id"];
			}
			if (current_user.isContact)
			{
				customer_id = current_user.customerID.ToString();
			}
		}
		if (!string.IsNullOrEmpty(_q["address_id"]))
		{
			int.TryParse(_q["address_id"], out address_id);
		}
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		layout.used_gv = gv_customer_assets;
		h.Set("gridview_id", "gv_customer_assets");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
		_tools.dont_cache_page();
		if (Cache["ds_depend"] == null)
		{
			Cache["ds_depend"] = DateTime.Now;
		}
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsCallback && !IsPostBack)
		{
			if (origin == null)
			{
				var gl = new NeGridLayouts(current_user.id, _page_name);
				if (gl.GridLayoutID == 0)
				{
					gl.GridLayout_Layout = gv_customer_assets.SaveClientLayout();
					gl.member_id = current_user.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();

					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				else
				{
					gv_customer_assets.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				dde_filter.Text = gl.GridLayout_Name;
			}
			Session["gv_customer_assets"] = null;
			Session["customer_assets_customers"] = null;
			Session["customer_assets_locations"] = null;
			Session["customer_assets_main"] = null;
		}


		fill_grid();
	}
	protected void fill_grid()
	{
		if (Session["customer_assets_main"] == null)
		{
			var appendage = customer_id != "0" || current_user.isContact
												? address_id == 0
													? " WHERE a.customer_id = '" + customer_id + "'"
													: " WHERE a.customer_id = '" + customer_id + "' AND a.address_id = '" + address_id + "' "
												: "";
			Session["customer_assets_main"] = _tools.getSQL_datatable(string.Format(@" SELECT a.id, 
a.name name, b.customer_name customer_name, a.customer_id, a.address_id, a.description description, a.active, c.member_fullname addedby, a.manufacturer, a.model FROM customer_asset a LEFT JOIN customer
b ON a.customer_id = b.customer_id LEFT JOIN member c ON c.member_id = a.addedby {0} ", appendage), null);
		}

		gv_customer_assets.DataSource = Session["customer_assets_main"];
		gv_customer_assets.DataBind();

		var col_customer = (gv_customer_assets.Columns["customer_id"] as GridViewDataComboBoxColumn);
		var col_location = (gv_customer_assets.Columns["address_id"] as GridViewDataComboBoxColumn);

		if (Session["customer_assets_customers"] == null)
		{
			if ((customer_id != "0") || (current_user.isContact))
			{
				Session["customer_assets_customers"] = Toolbox.doSQL_dt(@"SELECT customer_id id,customer_name name FROM customer WHERE customer_id = @v0  ORDER BY customer_name", new object[] { customer_id });
			}
			else
			{
				Session["customer_assets_customers"] = Toolbox.doSQL_dt(@"SELECT customer_id id,customer_name name FROM customer WHERE customer_status NOT IN (4,5,6) ORDER BY customer_name", null);
			}
		}
		if (gv_customer_assets.IsEditing && !gv_customer_assets.IsNewRowEditing)
		{
			var ds_locations = (SqlDataSource)gv_customer_assets.FindEditFormTemplateControl("ds_locations");

		}
		else if (gv_customer_assets.IsNewRowEditing)
		{
			//var test_customer_id		= gv_customer_assets.get
		}


		if (Session["customer_assets_locations"] == null && customer_id != "0")
		{
			Session["customer_assets_locations"] = Toolbox.doSQL_dt(@"SELECT address_id id, CONCAT('[',address_type,'] - ', address_addr1) name FROM address WHERE address_table = 'Customer' AND address_table_id = @v0 ", new object[] { customer_id });
		}
		else if (Session["customer_assets_locations"] == null)
		{
			Session["customer_assets_locations"] = Toolbox.doSQL_dt(@"SELECT address_id id, CONCAT('[',address_type,'] - ', address_addr1) name FROM address  WHERE address_table = 'Customer'", null);
		}

		col_customer.PropertiesComboBox.DataSource = Session["customer_assets_customers"];
		col_customer.PropertiesComboBox.TextField = "name";
		col_customer.PropertiesComboBox.ValueField = "id";
		col_customer.PropertiesComboBox.ValueType = typeof(int);
		if (Session["customer_assets_locations"] != null)
		{
			col_location.PropertiesComboBox.DataSource = Session["customer_assets_locations"];
			col_location.PropertiesComboBox.TextField = "name";
			col_location.PropertiesComboBox.ValueField = "id";
			col_location.PropertiesComboBox.ValueType = typeof(int);
		}
		else
		{
			Session["customer_assets_locations"] = Toolbox.doSQL_dt(@"SELECT address_id id, CONCAT('[',address_type,'] - ', address_addr1) name FROM address WHERE address_table = 'Customer' AND address_table_id = @v0 ", new object[] { customer_id });
			col_location.PropertiesComboBox.DataSource = Session["customer_assets_locations"];
			col_location.PropertiesComboBox.TextField = "name";
			col_location.PropertiesComboBox.ValueField = "id";
			col_location.PropertiesComboBox.ValueType = typeof(int);

		}
		gv_customer_assets.DataBind();
	}
	protected void gv_customer_assets_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_customer_assets_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	protected void gv_customer_assets_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		if (e.NewValues["customer_id"] == null)
		{
			throw new Exception("You must select a customer");
		}
		if (e.NewValues["name"] == null)
		{
			throw new Exception("You must enter a valid name");
		}
		if (e.NewValues["description"] == null)
		{
			throw new Exception("You must select valid description");
		}
		var ca = new customer_asset();
		ca.customer_id = Convert.ToInt32(e.NewValues["customer_id"]);
		ca.address_id = Convert.ToInt32(e.NewValues["address_id"]);
		ca.name = e.NewValues["name"].ToString();
		ca.description = e.NewValues["description"].ToString();
		ca.active = e.NewValues["active"] == null ? true : Convert.ToBoolean(e.NewValues["active"]);
		ca.added_by = current_user.id32;
		ca.model = e.NewValues["model"].ToString();
		ca.manufacturer = e.NewValues["manufacturer"].ToString();
		ca.save();

		e.Cancel = true;
		gv_customer_assets.CancelEdit();
		Session["customer_assets_main"] = null;
		fill_grid();
	}
	protected void gv_customer_assets_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
	{
		if (e.Column.FieldName == "Customer_ID")
		{
			var t = (ASPxComboBox)e.Editor;
			t.DataSource = Session["customer_assets_customers"];
			t.DataBindItems();
			if (customer_id != "0")
			{
				t.Value = Convert.ToInt32(customer_id);
				t.ClientEnabled = false;
			}
		}

	}
	protected void gv_customer_assets_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		var id = Convert.ToInt32(e.Keys[0]);
		var ca = new customer_asset(id);
		ca.delete();
		e.Cancel = true;
		gv_customer_assets.CancelEdit();
		Session["customer_assets_main"] = null;
		fill_grid();
	}
	protected void gv_customer_assets_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		if (e.NewValues["customer_id"] == null)
		{
			throw new Exception("You must select a customer");
		}
		if (e.NewValues["name"] == null)
		{
			throw new Exception("You must enter a valid name");
		}
		if (e.NewValues["description"] == null)
		{
			throw new Exception("You must select valid description");
		}
		var id = Convert.ToInt32(e.Keys[0]);
		var ca = new customer_asset(id);
		ca.customer_id = Convert.ToInt32(e.NewValues["customer_id"]);
		ca.address_id = Convert.ToInt32(e.NewValues["address_id"]);
		ca.name = e.NewValues["name"].ToString();
		ca.description = e.NewValues["description"].ToString();
		ca.active = e.NewValues["active"] == null ? false : Convert.ToBoolean(e.NewValues["active"]);
		ca.added_by = current_user.id32;
		ca.model = e.NewValues["model"].ToString();
		ca.manufacturer = e.NewValues["manufacturer"].ToString();
		ca.save();

		e.Cancel = true;
		gv_customer_assets.CancelEdit();
		Session["customer_assets_main"] = null;
		fill_grid();
	}
	protected void gv_customer_assets_ClientLayout(object sender, ASPxClientLayoutArgs e)
	{
		if (customer_id != "0")
		{
			gv_customer_assets.Settings.ShowFilterBar = GridViewStatusBarMode.Hidden;

		}
	}
	protected void cb_location_Callback(object sender, CallbackEventArgsBase e)
	{
		var cb = (ASPxComboBox)sender;
		cb.DataBind();
	}
	protected void gv_customer_assets_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
	{
		if (customer_id != "" && customer_id != "0")
		{
			e.NewValues["customer_id"] = Convert.ToInt32(customer_id);
		}
		if (address_id != 0)
		{
			e.NewValues["address_id"] = address_id;
		}
	}
	protected void gv_customer_assets_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var cb_customer = (ASPxComboBox)gv_customer_assets.FindEditFormTemplateControl("cb_customer");
		var cb_location = (ASPxComboBox)gv_customer_assets.FindEditFormTemplateControl("cb_location");
		if (customer_id != "" && customer_id != "0")
		{
			cb_customer.ClientEnabled = false;
		}
		if (address_id != 0 && gv_customer_assets.IsNewRowEditing)
		{
			cb_location.ClientEnabled = false;
		}
		if (gv_customer_assets.IsEditing)
		{
			if (gv_customer_assets.EditingRowVisibleIndex >= 0)
			{
				var frm_files = (HtmlContainerControl)gv_customer_assets.FindEditFormTemplateControl("frm_files");
				frm_files.Attributes["src"] = "/filemanager.aspx?parent_page=customer_assets&id=" + gv_customer_assets.GetRowValues(gv_customer_assets.EditingRowVisibleIndex, "id");
			}
		}
	}
}
