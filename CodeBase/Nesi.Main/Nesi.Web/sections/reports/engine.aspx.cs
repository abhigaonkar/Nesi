using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DevExpress.Xpo;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using MenuItem = DevExpress.Web.MenuItem;
using System.Text;
using DevExpress.Data;
using nesi.core;
using NESI.Common.Models;
using System.Web.UI.HtmlControls;

public partial class sections_reports_engine : System.Web.UI.Page
	{
	NeMember _current_user;
	int _page_id = OpsPage.ReportEngine;
	ASPxHiddenField _h;
	SqlDataSource _ds_templates;
	ASPxDropDownEdit _dde_filter;
	Panel _panel_export;
	readonly Unit ControlWidth = Unit.Pixel(150);
	protected void Page_Init(object _sender, EventArgs _e)
		{
		_current_user = Toolbox.do_handle_authentication(_page_id);
		bind_grid();
		}
	protected void Page_Load(object _sender, EventArgs _e)
		{
		var menu = new NeMenu(_current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		Page.Title = "Report Engine";
		build_menu(report_menu);
		var currentReportId = hid_report_id.Value;
		var currentParameters = hid_paras.Value;
		if (report_menu.Items.Count == 0)
			{
			// No items, only show the no reports div
			gv.Visible = false;
			report_menu.Visible = false;
			div_no_reports.Visible = true;
			}
		else
			{
			if (!IsPostBack)
				{
				Session["engine_ds"] = null;
				gv.ClientVisible = false;
				}
			bind_grid(); 
		    if(!string.IsNullOrEmpty(currentReportId))
			 	{
			 	//Load_report("filter", currentReportId, currentParameters);
				}
			}
		}
	private void SetCurrentHeaderValues(string controlId, string value)
		{
		var currentIdControl = gv.FindTitleTemplateControl(controlId) as HtmlInputHidden;
		if(currentIdControl != null)
			{
			currentIdControl.Value = value;
			}
		}

	private void build_menu(ASPxMenuBase _menu)
		{
		var dt = Toolbox.doSQL_dt(@" SELECT a.name category, a.id category_id, b.name report, b.id report_id FROM r_engine_category a Inner join r_engine_report b on b.r_engine_category = a.id INNER JOIN r_engine_permission c ON c.r_engine_report = b.id AND c.member = @v0 ORDER BY a.name", new object[] { _current_user.id });
		var categories = dt.DefaultView.ToTable(true, "category", "category_id");
		foreach (DataRow cat in categories.Rows)
			{
			var m = create_menu_item(cat, dt);
			_menu.Items.Add(m);
			}
		}
	private MenuItem create_menu_item(DataRow _row, DataTable _dt)
		{
		var category_id = (int)_row["category_id"];
		var category = (string)_row["category"];
		var m = new MenuItem(category, category_id.ToString());
		var items = _dt.Select("category_id = " + category_id)
               .OrderByDescending(row => row["report_id"])
               .ToList();
		
		foreach (var i in items)
			{
			var report_id = (int)i["report_id"];
			var report = (string)i["report"];
			var child_i = new MenuItem(report, report_id.ToString());
			m.Items.Add(child_i);
			}
		return m;
		}
	private void bind_grid()
		{
		layout_control.used_gv = gv;
		_h = (ASPxHiddenField)layout_control.FindControl("h");
		_ds_templates = (SqlDataSource)layout_control.FindControl("ds_templates");
		_dde_filter = (ASPxDropDownEdit)layout_control.FindControl("dde_filter");
		_panel_export = (Panel)layout_control.FindControl("panel_export");
		_h.Set("gridview_id", "gv");

		if (IsPostBack)
			{
			if (Session["engine_ds"] != null)
				{
				gv.DataSource = (DataTable)Session["engine_ds"];
				gv.DataBind();
				}

			var currentReportId = hid_report_id.Value;
			if (!string.IsNullOrEmpty(currentReportId))
				{
				if (int.TryParse(currentReportId, out int reportId))
					{
					var report = new report_engine.report(reportId);
					if (!string.IsNullOrEmpty(report.name))
						{
						// Normal Case - Grid Load 
						_panel_export.Visible = true;
						string stripped_name = "ReportEngine_" + Regex.Replace(report.name, @"[^A-Za-z0-9\w_]", "");
						layout_control.__page_name = stripped_name; // i.e.: ReportEngine_InternalCustomers
						_ds_templates.SelectParameters["@page_name"].DefaultValue = stripped_name;
						HandleGridLayout(report, stripped_name);
						reportName.Text = report.name;
						}
					else // Edge Case: This would mean the person that CREATED the report didn't name it, can happen though during debug/initial creation.
						{
						Toolbox.do_errorLog($"Report with ID {reportId} does not have a name.");

						// Set default __page_name
						string defaultName = $"UnnamedReport_{reportId}";
						reportName.Text = report.name;
						_panel_export.Visible = true;
						string stripped_name = "ReportEngine_" + Regex.Replace(defaultName, @"[^A-Za-z0-9\w_]", "");
						layout_control.__page_name = stripped_name;
						_ds_templates.SelectParameters["@page_name"].DefaultValue = stripped_name;
						HandleGridLayout(report, stripped_name);
						}
					fill_permitted_users(report.id);
					}
				else // Edge Case: This is stating that the value in hid_report_id wasn't parse-able
					{
					Toolbox.do_errorLog($"Invalid report ID: {currentReportId}");

					// Set default __page_name for invalid report ID
					string defaultNameForInvalidId = "InvalidReportID";
					reportName.Text = defaultNameForInvalidId;
					SetDefaultPageName(defaultNameForInvalidId);
					}
				}

			//permitted_users.Visible = true;
			_dde_filter.DataBind();
			}
		else
			{
			_panel_export.Visible = false;
			//permitted_users.Visible = false;
			}
		}
	private void SetDefaultPageName(string defaultName)
		{
		gv.SettingsText.Title = defaultName;
		_panel_export.Visible = true;
		string stripped_name = "ReportEngine_" + Regex.Replace(defaultName, @"[^A-Za-z0-9\w_]", "");
		if(layout_control.GridviewID == stripped_name)
			{
			_ds_templates.DataBind();
			return;
			}
		layout_control.__page_name = stripped_name;
		layout_control.GridviewID = stripped_name;
		_ds_templates.SelectParameters["@page_name"].DefaultValue = stripped_name;
		_ds_templates.DataBind();
		HandleGridLayout(null, stripped_name); // Assuming HandleGridLayout can handle null report
		}
	private void HandleGridLayout(report_engine.report report, string stripped_name)
		{
		var gl = new NeGridLayouts(Convert.ToInt32(_current_user.id), stripped_name);

		if (!gv.ClientVisible)
			{
			if (gl.GridLayoutID == 0)
				{
				gl.GridLayout_Layout = gv.SaveClientLayout();
				gl.member_id = _current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = stripped_name;
				gl.SaveGridLayout();
				_h.Set("ID", gl.GridLayoutID);
				_h.Set("NAME", gl.GridLayout_Name);
				}
			else if (gv.FilterExpression == "")
				{
				hid_layout.Value = gl.GridLayout_Layout;
				gv.LoadClientLayout(gl.GridLayout_Layout);
				_h.Set("ID", gl.GridLayoutID);
				_h.Set("NAME", gl.GridLayout_Name);
				}
			}
		else
			{
			_h.Set("ID", gl.GridLayoutID);
			_h.Set("NAME", gl.GridLayout_Name);
			}

		_dde_filter.Text = gl.GridLayout_Name;
		}
	private void HandleBusinessUnit(string action, string parameter, string[] payloadList, int para_i)
		{
		var ctrl = new ASPxComboBox
			{
			DataSource = Toolbox.doSQL_dt(@"SELECT * FROM vw_active_business_units", null),
			TextField = "name",
			ValueField = "id",
			ValueType = typeof(int),
			Caption = "Business Unit",
			ID = parameter.Replace("?", "").Trim(),
			ClientInstanceName = parameter.Replace("?", "").Trim(),
			NullText = "Select Business Unit"
			};
		ctrl.Style["margin-bottom"] = "10px";
		ctrl.CaptionCellStyle.Width = ControlWidth;
		ctrl.ValidationSettings.RequiredField.IsRequired = true;
		ctrl.ValidationSettings.RequiredField.ErrorText = "Field is required";
		ctrl.ValidationSettings.Display = Display.Dynamic;
		ctrl.ValidationSettings.ValidationGroup = "dynamicCtrls";
		ctrl.DataBind();
		if (action == "filter")
			{
			ctrl.Value = Convert.ToInt32(payloadList[para_i]);
			}
		div_filters.Controls.Add(ctrl);
		}
	private void HandlePayPeriod(string action, string parameter, string[] payloadList, int para_i)
		{
		var ctrl = new ASPxComboBox
			{
			DataSource = Toolbox.doSQL_dt(@"SELECT payperiodid id, CONCAT('(',payperiodid,') ', DATE(startdate), ' - ', DATE(enddate)) text FROM payperiods  WHERE startdate <= CURDATE() ORDER BY payperiodid DESC", null),
			TextField = "text",
			ValueField = "id",
			ValueType = typeof(int),
			Caption = "Pay Period",
			ID = parameter.Replace("?", "").Trim(),
			ClientInstanceName = parameter.Replace("?", "").Trim(),
			NullText = "Select Pay Period"
			};
		ctrl.Style["margin-bottom"] = "10px";
		ctrl.CaptionCellStyle.Width = ControlWidth;
		ctrl.ValidationSettings.RequiredField.IsRequired = true;
		ctrl.ValidationSettings.RequiredField.ErrorText = "Field is required";
		ctrl.ValidationSettings.Display = Display.Dynamic;
		ctrl.ValidationSettings.ValidationGroup = "dynamicCtrls";
		ctrl.DataBind();
		if (action == "filter")
			{
			ctrl.Value = Convert.ToInt32(payloadList[para_i]);
			}
		div_filters.Controls.Add(ctrl);
		}
	private void HandleTaxEntity(string action, string parameter, string[] payloadList, int para_i)
		{
		var ctrl = new ASPxComboBox
			{
			DataSource = Toolbox.doSQL_dt(@"SELECT ddl_name name, id FROM tax_entity WHERE is_active = 1 AND FIND_IN_SET(id, @v0)", new Object[] { new Current_User().visible_tax_entities }),
			TextField = "name",
			ValueField = "id",
			ValueType = typeof(int),
			Caption = "Tax Entity",
			ID = parameter.Replace("?", "").Trim(),
			ClientInstanceName = parameter.Replace("?", "").Trim(),
			NullText = "Select Tax Entity"
			};
		ctrl.Style["margin-bottom"] = "10px";
		ctrl.CaptionCellStyle.Width = ControlWidth;
		ctrl.ValidationSettings.RequiredField.IsRequired = true;
		ctrl.ValidationSettings.RequiredField.ErrorText = "Field is required";
		ctrl.ValidationSettings.Display = Display.Dynamic;
		ctrl.ValidationSettings.ValidationGroup = "dynamicCtrls";
		ctrl.DataBind();
		if (action == "filter")
			{
			ctrl.Value = Convert.ToInt32(payloadList[para_i]);
			}
		div_filters.Controls.Add(ctrl);
		}
	private void HandleDate(string action, string parameter, string[] payloadList, int para_i)
		{
		var ti = new CultureInfo("en-US", false).TextInfo;
		var ctrl = new ASPxDateEdit
			{
			Caption = ti.ToTitleCase(parameter.Replace("?", "").Replace("_", " ")),
			ID = parameter.Replace("?", "").Trim(),
			ClientInstanceName = parameter.Replace("?", "").Trim(),
			NullText = "Select Date"
			};
		ctrl.CaptionCellStyle.Width = ControlWidth;
		ctrl.Style["margin-bottom"] = "7px";
		ctrl.ValidationSettings.RequiredField.IsRequired = true;
		ctrl.ValidationSettings.RequiredField.ErrorText = "Field is required";
		ctrl.ValidationSettings.Display = Display.Dynamic;
		ctrl.ValidationSettings.ValidationGroup = "dynamicCtrls";

		ctrl.DataBind();
		div_filters.Controls.Add(ctrl);
	
		if (action == "filter" && !payloadList[para_i].Contains("?"))
			{
			ctrl.Value = Convert.ToDateTime(payloadList[para_i]);
			}
		}
	private void HandleEmployee(string action, string parameter, string[] payloadList, int para_i)
		{
		var ctrl = new ASPxComboBox
			{
			DataSource = Toolbox.doSQL_dt(@"SELECT b.member_id id, CONCAT(c.name, ' - ', b.member_fullname) name FROM active_members a INNER JOIN member b ON a.member_id = b.member_id INNER join business_unit c ON a.company = c.id ORDER BY c.name, b.member_fullname, b.member_lastname", null),
			TextField = "name",
			ValueField = "id",
			ValueType = typeof(int),
			Caption = "Employee",
			ID = parameter.Replace("?", "").Trim(),
			ClientInstanceName = parameter.Replace("?", "").Trim(),
			NullText = "Select Employee"
			};
		ctrl.Style["margin-bottom"] = "10px";
		ctrl.CaptionCellStyle.Width = ControlWidth;
		ctrl.DataBind();
		if (action == "filter")
			{
			ctrl.Value = Convert.ToInt32(payloadList[para_i]);
			}
		div_filters.Controls.Add(ctrl);
		}
	private void Load_report(string action, string value, string payload)
		{
		var report					= new report_engine.report(Convert.ToInt32(value));
		hid_report_id.Value			= value;
		Session["engine_ds"]		= null;
		gv.DataSource				= null;
		gv.Columns.Clear();
		gv.ClientVisible			= false;
		var hasParameters = !string.IsNullOrEmpty(report.parameters);
		if(hasParameters)
			{
			var report_paras			= report.parameters.Split(',');
			hid_paras.Value				= report.parameters;
			var para_i					= 0;
			var payload_list			= payload.Split(',');
			#region Control Creation
			foreach(var p in report_paras)
				{
				if(p.Contains("_id")) // Int based parameters
					{
					if(p == "?business_unit_id")
						{
						HandleBusinessUnit(action, p, payload_list, para_i);
						}
					if (p == "?tax_entity_id")
					    {
					    HandleTaxEntity(action, p, payload_list, para_i);
						}
                    if (p == "?payperiod_id")
						{
						HandlePayPeriod(action, p, payload_list, para_i);
						}
					else if(p == "?member_id")
						{
						HandleEmployee(action, p, payload_list, para_i);
						}
					}
				else if(p.Contains("date_")) // Date based parameters
					{
					HandleDate(action, p, payload_list, para_i);
					}
				para_i++;
				}
				var b_submit = new ASPxButton
									{
									Text = "Submit",
									AutoPostBack = false
									};
				b_submit.ClientSideEvents.Click		= "engine_client.filter_submit";
				div_filters.Controls.Add(b_submit);
			#endregion Control Creation
			if(action == "filter")
				{
				para_i = 0;
				object[] paramObjects = new object[report_paras.Length];

				// Preparing parameters for the query
				foreach (var para in report_paras)
				{
					// Assuming each 'para' corresponds to a placeholder in 'sql'
					// and 'payload_list' contains the values for these parameters.
					paramObjects[para_i] = payload_list[para_i];
					report.query		 = report.query.Replace(report_paras[para_i], "@v"+para_i);
					para_i++;
				}

				// Execute the SQL query with parameters
				Session["engine_ds"] = Toolbox.doSQL_dt(report.query, paramObjects);

				}
			}
		else
			{
			gv.DataSource			= new DataTable();
			Session["engine_ds"]	= Toolbox.doSQL_dt(report.query  , null);
			}
		bind_grid();
		gv.SettingsText.Title		= report.name;
		reportName.Text = report.name;
		fill_permitted_users(report.id);
		SetDefaultPageName(report.name);
		reportName.ClientVisible = action == "filter" || !hasParameters;
		gv.ClientVisible = action == "filter" || !hasParameters;
		layout_control.Visible = gv.ClientVisible;
		}
	private void fill_permitted_users(int _report_id)
		{
		var dt			= Toolbox.doSQL_dt(@"SELECT b.member_fullname name, b.member_status FROM r_engine_permission a LEFT JOIN member b ON a.member = b.member_id WHERE r_engine_report = @v0  ORDER BY b.member_lastname, b.member_fullname", new object[] {  _report_id } );
		var sb			= new StringBuilder();
		sb.Append("<table><thead><tr><th>Employee</th><th>Status</th></thead><tbody>");
		foreach(DataRow dr in dt.Rows)
			{
			sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", dr["name"], dr["member_status"]);
			}
		sb.Append("</tbody></table>");
		permitted_users.Attributes["data-tooltip"]			= sb.ToString();
		}
	protected void cbp_grid_Callback(object sender, CallbackEventArgsBase e)
		{
		if(e.Parameter.Contains("|"))
			{
			// Examples:
			// e.Parameter = "load|271|" = Load report 271
			var paras		= e.Parameter.Split('|'); 
			var action		= paras[0];
			var value		= paras[1];
			var payload		= paras[2];

			switch(action)
				{
				case "filter":
					Load_report(action, value, payload);
				break;
				case "load":
					gv.FilterExpression	= "";
					Load_report(action, value, payload);
					
				break;
				}
			}
		}
    protected void gv_DataBound(object sender, EventArgs e)
		{
		var grid = (ASPxGridView)sender;
		foreach (GridViewDataColumn column in grid.Columns)
			{
			var row_values	= grid.GetRowValues(0, column.FieldName);
			if(row_values != null)
				{
				if(grid.GroupSummary.Count == 0)
					{
					grid.GroupSummary.Add(new ASPxSummaryItem { SummaryType = SummaryItemType.Count });
					}
				var column_type = row_values.GetType();
				var type_code = Type.GetTypeCode(column_type);
				var is_numeric	= type_code == TypeCode.Decimal || type_code == TypeCode.Double;
				if (is_numeric && column.FieldName.Contains("[sum]"))
					{
					var si		= new ASPxSummaryItem(column.FieldName, DevExpress.Data.SummaryItemType.Sum);
					si.Tag		= column.FieldName;
					si.DisplayFormat	= "Sum: {0}";
					if(grid.TotalSummary.Count == 0)
						{
						grid.TotalSummary.Add(si);
						}
					else 
						{
						var matched		= 0;
						foreach(ASPxSummaryItem s in grid.TotalSummary)
							{
							if(s.Tag == column.FieldName)
								{
								matched		= 1;
								}
							}
						if(matched == 0)
							{
							grid.TotalSummary.Add(si);
							}
						}
					column.Caption = column.FieldName.Replace("[sum]", "");
					}
				else if (is_numeric && column.FieldName.Contains("[avg]"))
					{
					var si		= new ASPxSummaryItem(column.FieldName, DevExpress.Data.SummaryItemType.Average);
					si.Tag		= column.FieldName;
					si.DisplayFormat	= "Average: {0}";
					if(grid.TotalSummary.Count == 0)
						{
						grid.TotalSummary.Add(si);
						}
					else 
						{
						var matched		= 0;
						foreach(ASPxSummaryItem s in grid.TotalSummary)
							{
							if(s.Tag == column.FieldName)
								{
								matched		= 1;
								}
							}
						if(matched == 0)
							{
							grid.TotalSummary.Add(si);
							}
						}
					column.Caption = column.FieldName.Replace("[avg]", "");
					}
				if(column.FieldName.Contains("[width"))
					{
					var reg		= @"\[width:(\d+)\]";
					var re		= new Regex(reg);
					if(re.Match(column.FieldName).Success)
						{
						var width		= re.Match(column.FieldName).Groups[1].Value;
						int w;
						int.TryParse(width, out w);
						column.Width	= Unit.Pixel(w);
						column.Caption	= Regex.Replace(column.FieldName, reg, "");
						}
					}
				if(type_code == TypeCode.DateTime)
					{
					var grid_view_data_date_column = column as GridViewDataDateColumn;
					if (grid_view_data_date_column != null) grid_view_data_date_column.PropertiesDateEdit.DisplayFormatString	= "yyyy-MM-dd HH:mm:ss";
					}
				}
			}

		}
	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		}

	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	}
