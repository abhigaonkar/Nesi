using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.IO;
using DevExpress.Web;
using System.Linq;
using MySql.Data.MySqlClient;
using NESI.Common.Models;
using nesi.core;
using System.Data;

public partial class sections_hr_expense_index : System.Web.UI.Page
	{
	NeMember _current_user;
	private const int page_id = OpsPage.ExpenseReport; // from Page table in DB
	private const string page_description = "Expense Reimbursement Report";
	ASPxDropDownEdit _dde_filter;
	Panel _panel_export;
	static string _default_filter = "";
	SqlDataSource _ds_templates;
	ASPxHiddenField _h;
	string _page_name = "expense_report";
	int _current_payperiod_id;
	private int _c_id;
	private string _companies;
	private NameValueCollection _q;
	private List<int> _reports_to_me = new List<int>();
	bool CanViewAll;
	protected void Page_Init(object _sender, EventArgs _e)
		{
		_q = Request.QueryString;
		layout.__page_name = _page_name;
		layout.used_gv = gv_expensereport;
		_current_user = Toolbox.do_handle_authentication(page_id);
		_current_payperiod_id = Toolbox.doSQL_int(@"CALL _payperiod()");
		_h = (ASPxHiddenField)layout.FindControl("h");
		_ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		_dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		_panel_export = (Panel)layout.FindControl("panel_export");
		_panel_export.Visible = true;
		CanViewAll	= _current_user.AuthenticatedForPrivilege(OpsPrivilege.ExpenseReportCanViewAll);
		var reporting_list	= Toolbox.doSQL_string(string.Format("SELECT REPORTS_TO({0})", _current_user.id));
		if(reporting_list.Contains(","))
			{
			_reports_to_me	= reporting_list.Split(',').Select(int.Parse).ToList();
			}
		else if(reporting_list != "")
			{
			_reports_to_me.Add(int.Parse(reporting_list));
			}
		_h.Set("gridview_id", "gv_expensereport");
		_ds_templates.SelectParameters["@page_name"].DefaultValue = string.Format("{0}", _page_name);
		_ds_templates.SelectParameters["@member_id"].DefaultValue = _current_user.id.ToString();
		  
        if (!IsPostBack)
			{
			dte_start.Date = DateTime.Today.AddDays(-30);
			dte_end.Date = DateTime.Today;
			rdo_date.Checked = true;
			}
		}
	protected void Page_Load(object _sender, EventArgs _e)
		{
		var menu = new NeMenu(_current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(_current_user);
		 


        var lbltemp = (Label)Master.FindControl("lblHeading");
		lbltemp.Text = page_description;

		    using (var conn = Toolbox.connect())
		        {
                    combo_tax_entity.DataBind();
		        if (!IsPostBack)
		            {

		            fill_tax_entities(conn);
		            combo_tax_entity.Value = _current_user.business_unit.tax_entity_id;
		            var bu = new NeBusinessUnit(_current_user.business_unit_id);
		            combo_tax_entity.Value = bu.tax_entity_id;
					Session["tax_entity_id"] = bu.tax_entity_id;

				_c_id = _current_user.business_unit_id;
		            ddl_payperiods.Value = _current_payperiod_id;
		            //gv_expensereport.GroupBy(gv_expensereport.Columns["Dept"]);
		            gv_expensereport.GroupBy(gv_expensereport.Columns["Type"]);
		            gv_expensereport.ExpandAll();
		            }
		        //		exporter.FileName					= ddl_payperiods.Value + " Expense Report";

		        # region Load Summary Gridview

		        fill_business_units(conn);
		        if (!IsCallback && !IsPostBack)
		            {
		            var gl = new NeGridLayouts(Convert.ToInt32(_current_user.id), string.Format("{0}", _page_name));
		            if (gl.GridLayoutID != 0)
		                {
		                gv_expensereport.LoadClientLayout(gl.GridLayout_Layout);
		                _h.Set("ID", gl.GridLayoutID);
		                _h.Set("NAME", gl.GridLayout_Name);
		                _dde_filter.Text = gl.GridLayout_Name;
		                }
		            else
		                {
		                gv_expensereport.LoadClientLayout(_default_filter);
		                gl.GridLayout_Layout = _default_filter;
		                gl.member_id = _current_user.id;
		                gl.GridLayout_Name = "Default";
		                gl.GridLayout_Gridid = string.Format("{0}", _page_name);
		                gl.SaveGridLayout();
		                _h.Set("ID", gl.GridLayoutID);
		                _h.Set("NAME", gl.GridLayout_Name);
		                _dde_filter.Text = gl.GridLayout_Name;
		                }
                Session["gv_expense_report"] = null;

                    }

		        #endregion

		        #region Switch Company Privilege

		        //	cl_companies.Enabled = _current_user.AuthenticatedForPrivilege(175);

		        if (!IsPostBack && cl_business_units.Items.FindByValue(_c_id.ToString()) != null)
		            {
		            cl_business_units.Items[cl_business_units.Items.IndexOfValue(_c_id.ToString())].Selected = true;
		            }
		        _companies = "";
		        foreach (string c_selected in cl_business_units.SelectedValues)
		            {
		            _c_id = 99;
		            _companies += "" + c_selected + ",";
		            }
		        if (_companies != "")
		            {
		            _companies = _companies.Remove(_companies.Length - 1, 1) + "";
		            }
		        else
		            {
		            _companies += "";
		            }
		        }

		    #endregion
		fill_grid();
		}

	private void fill_grid()
		{
        if (Session["gv_expense_report"] == null)
        {
            if (rdo_date.Checked)
            {
                Session["gv_expense_report"] = Toolbox.doSQL_dt(@"CALL ds_expensereport_by_company2(@v0 , @v1  ,@v2, @v3 , @v4)", new object[] { _companies, dte_start.Text, dte_end.Text, _current_user.id, CanViewAll });
            }
            else
            {
                var payperiod = new NePayPeriod(Convert.ToInt32(ddl_payperiods.Value));
                Session["gv_expense_report"] = Toolbox.doSQL_dt(@"CALL ds_expensereport_by_company2(@v0 , @v1  ,@v2 , @v3, @v4)", new object[] { _companies, Toolbox.MySQL_longdt(payperiod.start_date), Toolbox.MySQL_longdt(payperiod.end_date), _current_user.id, CanViewAll });
            }
        }
        gv_expensereport.DataSource = Session["gv_expense_report"];

        gv_expensereport.DataBind();
		}
	protected void ASPxButton1_Click(object _sender, EventArgs _e)
		{
        Session["gv_expense_report"] = null;
        fill_grid();
		}

	protected void gv_expensereport_HtmlDataCellPrepared(object _sender, ASPxGridViewTableDataCellEventArgs _e)
		{
		var row_id          = gv_expensereport.GetRowValues(_e.VisibleIndex, "id") == null ? 0 : Convert.ToInt32(gv_expensereport.GetRowValues(_e.VisibleIndex, "id"));
		if (row_id <= 0) return;
		var status			= (string)gv_expensereport.GetRowValues(_e.VisibleIndex, "STATUS");
		var payperiod_id			= (int)gv_expensereport.GetDataRow(_e.VisibleIndex)["payperiod_id"];
		var member_id				= (int)gv_expensereport.GetDataRow(_e.VisibleIndex)["member_id"];
		var status_n		= status == "Approved" ? 1 : status == "Denied" ? 0 : -1;
		var status_icon		= status == "Approved" ? "1" : status == "Denied" ? "0" : "";
		var reports_to_me	= _reports_to_me.Contains(member_id);
		switch (_e.DataColumn.Caption)
			{
			case "Description":
				_e.Cell.ToolTip =Toolbox.ReturnBlankIfNull_string(_e.CellValue).ToString();
			break;
			case "Receipt":
				var exp = new payroll.expense(row_id);
				// Check if it has a file
				if (exp.has_file)
				{
					// retrive business unit of the user who created record
					var memberCreatedRecord = new NeMember(exp.id_member);
					var tax_entity_Id = Convert.ToInt32(Session["tax_entity_id"]);
					if (tax_entity_Id == 46)
					{
						// Get all business units with tax_entity_id = 43 using the stored procedure
						var business_units_by_tax_entity = Toolbox.doSQL_dt(@"SELECT id, ddl_name  FROM business_unit WHERE tax_entity_id IN(43,46,2,45) AND active = 'T'");
						foreach (DataRow row in business_units_by_tax_entity.Rows)
						{
							int businessUnitId = Convert.ToInt32(row["id"]);
							var fileServerOld = NeTaxEntity.BaseFolder(businessUnitId, false);
							var fileServerExtOld = NeTaxEntity.BaseFolder(businessUnitId, true);
							var pathOld = fileServerOld + $"/expense_receipts/{exp.id_expense}.{exp.file_ext}";
							var pathExtOld = fileServerExtOld + $"/expense_receipts/{exp.id_expense}.{exp.file_ext}";
							if (File.Exists(pathOld))
							{
								_e.Cell.Text = string.Format(@"<a href='javascript:void(0)' onmousedown=""get_attachment(event, {0}, '{1}');""><img src='/images/icon/icon[attachment].gif' alt='View Attachment' /></a>", exp.id_expense, pathExtOld);
								return;
							}
						}
					}
					else
					{
						// Check if file exists
						var fileServer = NeTaxEntity.BaseFolder(memberCreatedRecord.business_unit_id, false);
						var fileServerExt = NeTaxEntity.BaseFolder(memberCreatedRecord.business_unit_id, true);
						var path = fileServer + @"/expense_receipts/" + exp.id_expense + "." + exp.file_ext;
						var pathExt = fileServerExt + @"/expense_receipts/" + exp.id_expense + "." + exp.file_ext;

						_e.Cell.Text = File.Exists(path) ? string.Format(@"<a href='javascript:void(0)' onmousedown=""get_attachment(event, {0}, '{1}');""><img src='/images/icon/icon[attachment].gif' alt='View Attachment' /></a>", exp.id_expense, pathExt) : "";
					}
				}
			break;
			case "Approve/Deny":
				var button_app = (ASPxButton) gv_expensereport.FindRowCellTemplateControl(_e.VisibleIndex, _e.DataColumn, "app_exp");
				var button_del = (ASPxButton) gv_expensereport.FindRowCellTemplateControl(_e.VisibleIndex, _e.DataColumn, "del_exp");
				if (button_app != null && button_del != null)
					{
					if(payperiod_id >= _current_payperiod_id)
						{
						if(status_n != -1)
							{
							button_app.ClientEnabled			= status_n != 1 && status_n != 0 || CanViewAll && status_n != 1;
							button_del.ClientEnabled			= status_n != 0;
							}
						if(button_app.ClientEnabled)
							{
							button_app.ClientSideEvents.Click	=  string.Format(@"
				function(s,e)
					{{
					if(confirm('Are you sure you want to approve this request?'))
						{{
						gv_expensereport.PerformCallback('app|{0}');
						}}
					}}", row_id);
							}
						if(button_del.ClientEnabled)
							{
							button_del.ClientSideEvents.Click	=  string.Format(@"
				function(s,e)
					{{
					if(confirm('Are you sure you want to deny this request?'))
						{{
						gv_expensereport.PerformCallback('del|{0}');
						}}
					}}", row_id);
							}
						}
					else
						{
						button_app.ClientEnabled			= false;
						button_del.ClientEnabled			= false;
						}
					
					if(!reports_to_me && !CanViewAll)
						{
						button_app.ClientEnabled			= false;
						button_del.ClientEnabled			= false;
						}
					if(!button_app.ClientEnabled)
						{
						button_app.Style["opacity"]				= "0.25";
						button_app.ToolTip						= "Expense cannot be approved/edited";
						}
					else
						{
						button_app.Style["opacity"]				= "1";
						button_app.ToolTip						= "Approve Request";
						}
					if(!button_del.ClientEnabled)
						{
						button_del.Style["opacity"]				= "0.25";
						button_del.ToolTip						= "Expense cannot be denied/edited";
						}
					else
						{
						button_del.Style["opacity"]				= "1";
						button_del.ToolTip						= "Deny Request";
						}
					}
			break;
			}
		}
	protected void gv_expensereport_CustomCallback(object _sender, ASPxGridViewCustomCallbackEventArgs _e)
		{
		using(var conn = Toolbox.connect())
			{
			var gv = (ASPxGridView)_sender;
			gv.JSProperties["cpMessage"]	= "";
			if (_e.Parameters.Contains("|"))
				{
				var paras  = _e.Parameters.Split('|');
				var action = paras[0];
				var id     = Convert.ToInt32(paras[1]);
				var ex     = new payroll.expense(id);
				var bs		= new Toolbox.boolstr();
				switch (action)
					{
					case "del":
						bs	= ex.review(conn, false, _current_user, true);
					break;
					case "app":
						bs	= ex.review(conn, true, _current_user, true);
					break;
					}
				if(!bs.success)
					{
					throw new Exception(bs.message);
					}
				gv.JSProperties["cpMessage"] = bs.message;
				fill_grid();
				}
			else
				{
				if (_e.Parameters != "")
					{
					gv.LoadClientLayout(_e.Parameters);
					}
				else
					{
					gv.FilterExpression = "";
					for (var i = 0; i < gv.Columns.Count; i++)
						{
						var column = gv.Columns[i] as GridViewDataColumn;
						if (column != null)
							{
							var col = column;
							if (col.GroupIndex > -1)
								{
							//	gv.UnGroup(col);
								}
							col.Visible = true;
							}
						}
					}
				}
			}
		}
	[System.Web.Services.WebMethod]
	public static void set_currency(string _date, string _currency, string _rate)
		{
		using(var conn = Toolbox.connect())
			{
			if (Toolbox.doSQL_int(conn, @"SELECT COUNT(id) FROM currency_history WHERE date=@v0 AND currency=@v1", new object[] { _date, _currency}) == 0)
				{
				Toolbox.doSQL_void(conn,@"INSERT INTO currency_history (date,currency,per_usd) VALUES (@v0,@v1,@v2)", new object[] { _date, _currency, _rate});
				}
			}
		}
	protected void gv_expensereport_CustomJSProperties(object _sender, ASPxGridViewClientJSPropertiesEventArgs _e)
		{
		var gv = (ASPxGridView)_sender;
		_e.Properties["cpExp"] = gv.SaveClientLayout();
		}
	protected void ddl_payperiods_SelectedIndexChanged(object _sender, EventArgs _e)
		{
		var pp = new NePayPeriod(Convert.ToInt32(ddl_payperiods.Value));
		dte_start.Date = pp.start_date;
		dte_end.Date = pp.end_date;
		fill_grid();
		}

	    protected void combo_tax_entity_SelectedIndexChanged(object sender, EventArgs e)
	        {
	        using (var conn = Toolbox.connect())
	            {
	            try
	                {

	                fill_business_units(conn);
                var te_id = (int)combo_tax_entity.Value;
				Session["tax_entity_id"] = (int)combo_tax_entity.Value;
				var bu = new NeBusinessUnit(_current_user.business_unit_id);
	                var bu_id = Convert.ToInt32(_current_user.business_unit_id);
	                if (te_id == bu.tax_entity_id && cl_business_units.Items.FindByValue(bu_id) != null)
	                    {
	                    cl_business_units.Items.FindByValue(bu_id).Selected = true;
	                    }
	               
	             
	                }
	            catch (Exception ee)
	                {

	                Toolbox.do_errorLog(ee);throw;
	                }

	            }
	        }

	    private void fill_tax_entities(MySqlConnection _conn)
	        {
	        combo_tax_entity.DataSource = Toolbox.doSQL_dt(_conn, string.Format("Select id,ddl_name from tax_entity where id in ({0})", new Current_User().visible_tax_entities), null);
	        combo_tax_entity.DataBind();
	        }

    private void fill_business_units(MySqlConnection _conn)
	        {
	        var dt_business_units = Toolbox.doSQL_dt(_conn, string.Format(@"Select id,ddl_name name
from business_unit where active = 'T' and id in ({0}) and tax_entity_id = @v0 ", new Current_User().visible_business_units),
	            new object[] { combo_tax_entity.Value });
            var dv_companies = dt_business_units.DefaultView;
	            dv_companies.Sort = "name";
	            cl_business_units.DataSource = dv_companies;
	            cl_business_units.DataBind();
	        }

}