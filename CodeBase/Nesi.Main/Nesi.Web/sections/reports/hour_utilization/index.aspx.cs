using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using DevExpress.Web;
using MySql.Data.MySqlClient;
using nesi.core;

public partial class sections_reports_hour_utilization_index : Page
{
	public NeMember current_user;

	private const int page_id = 53; // from Page table in DB
	private const string page_description = "Hour Utilization";
	static string _page_name = "HourUtilization";
	public DataTable comps;
	string _compss = "";
	Toolbox _tools;
	ASPxHiddenField _h;
	SqlDataSource _ds_templates;
	ASPxDropDownEdit _dde_filter;
	Panel _panel_export;

    ASPxHiddenField _h_pro;
    SqlDataSource _ds_templates_pro;
    ASPxDropDownEdit _dde_filter_pro;
    Panel _panel_export_pro;

    protected void Page_Init(object _sender, EventArgs _e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		_tools.dont_cache_page();
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = page_description;

		layout.__page_name = _page_name;
		layout.used_gv = gv_hu;
		_h = (ASPxHiddenField)layout.FindControl("h");
		_ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		_dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		_panel_export = (Panel)layout.FindControl("panel_export");
		_panel_export.Visible = true;
		_h.Set("gridview_id", "gv_hu");
		_ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		_ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();

        _layout.__page_name = _page_name+"_projected";
        _layout.used_gv = gv_hu_projected;
        _h_pro = (ASPxHiddenField)_layout.FindControl("h");
        _ds_templates_pro = (SqlDataSource)_layout.FindControl("ds_templates");
        _dde_filter_pro = (ASPxDropDownEdit)_layout.FindControl("dde_filter");
        _panel_export_pro = (Panel)_layout.FindControl("panel_export");
        _panel_export_pro.Visible = true;
        _h_pro.Set("gridview_id", "gv_hu_projected");
        _ds_templates_pro.SelectParameters["@page_name"].DefaultValue = _page_name + "_projected";
        _ds_templates_pro.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();

    }
	protected void Page_Load(object _sender, EventArgs _e)
	{
		var csv_branches = "";
		using (var conn = Toolbox.connect())
		{
			csv_branches = ddlbymemberbranch.Value == null ? current_user.business_unit_id.ToString() : Toolbox.CommaDelimit(ddlbymemberbranch);
			
			if (!IsCallback && !IsPostBack)
			{
				var gl = new NeGridLayouts(current_user.id, _page_name);
				if (gl.GridLayoutID == 0)
				{
					gl.GridLayout_Layout = gv_hu.SaveClientLayout();
					gl.member_id = current_user.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();
					_h.Set("ID", gl.GridLayoutID);
					_h.Set("NAME", gl.GridLayout_Name);
				}
				else
				{
					gv_hu.LoadClientLayout(gl.GridLayout_Layout);
					_h.Set("ID", gl.GridLayoutID);
					_h.Set("NAME", gl.GridLayout_Name);
				}
				_dde_filter.Text = gl.GridLayout_Name;

                gl = new NeGridLayouts(current_user.id, _page_name + "_projected");
                if (gl.GridLayoutID == 0)
                {
                    gl.GridLayout_Layout = gv_hu_projected.SaveClientLayout();
                    gl.member_id = current_user.id;
                    gl.GridLayout_Name = "Default";
                    gl.GridLayout_Gridid = _page_name + "_projected";
                    gl.SaveGridLayout();
                    _h.Set("ID", gl.GridLayoutID);
                    _h.Set("NAME", gl.GridLayout_Name);
                }
                else
                {
                    gv_hu_projected.LoadClientLayout(gl.GridLayout_Layout);
                    _h.Set("ID", gl.GridLayoutID);
                    _h.Set("NAME", gl.GridLayout_Name);
                }

                _dde_filter_pro.Text = gl.GridLayout_Name;

            }
			if (!IsPostBack)
			{
				Session["hu_projected_temp"] = null;
				Session["hu_projected"] = null;
                Session["gv_hu_member"] = null;

                var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
				divMenu.InnerHtml = menu.MenuHTML;
				divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);

				var c_list2 = new NeBusinessUnit();
				ddlCompany.DataSource = NeBusinessUnit.units_with_timesheet();
				ddlCompany.DataBind();
				ddlCompany.SelectedValue = current_user.business_unit_id.ToString();
				var lic = new ListItem("All Companies", "0");
				ddlCompany.Items.Insert(0, lic);
				dte_end_by_customer.Date = DateTime.Now;
				dt_end_by_member.Date = DateTime.Now;
				var first_day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
				dte_start_by_customer.Date = first_day;
				dte_start_by_member.Date = first_day;
				/* (If you don't want to keep setting the date fields...)
				dte_end_by_customer.Date = new DateTime(2016, 3, 31);
				dt_end_by_member.Date = new DateTime(2016,3,31);
				var first_day = new DateTime(2016, 3, 1);
				dte_start_by_customer.Date = first_day;
				dte_start_by_member.Date = first_day;
				*/
				gv_hu.GroupBy(gv_hu.Columns["Member"]);


				fill_users();
				var lic2 = new ListItem("All Users", "0");
				ddlUsers.Items.Insert(0, lic2);
				ddlbymemberbranch.Value = current_user.business_unit_id;

				if (current_user.AuthenticatedForPrivilege(48))
				{
					ddlCompany.Enabled = true;
				}
			}
			if (ddlbymemberbranch.Value != null)
			{
				comps = Toolbox.doSQL_dt(conn, string.Format(@"Select Internal_CompanyNo_Intranet_CustID from Internal_CompanyNo  where business_unit_id IN ({0}) ", csv_branches), null);
				foreach (DataRow dr in comps.Rows)
				{
					_compss += dr[0] + ",";
				}
			}
		}
        fill_hu_project();
        fill_gv_hu_member();

    }

    protected void fill_gv_hu_member()
    {
        if (Session["gv_hu_member"] == null)
        {
            var csv_branches = ddlbymemberbranch.Value == null ? current_user.business_unit_id.ToString() : Toolbox.CommaDelimit(ddlbymemberbranch);
            using (var conn = Toolbox.connect())
            {
                Session["gv_hu_member"] = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT
	a.Date,
	a.membertime_memberid,
	d.ddl_name business_unit,
	a.MemberTime_Customer_Name,
	a.WOType,
	a.NumberOfHours,
	a.MemberTime_MemberTypeHours_ID,
	e.abbreviation MemberTime_PayTypeHours_ID,
	a.MemberTime_Mileage,
	a.MemberTime_Warranty,
	a.Warrenty,
	a.membertime_shop_type_id,
	b.member_fullname as membername 
FROM
	membertime a 
LEFT JOIN 
	member b on a.membertime_memberid = b.member_id 
LEFT join
	business_unit d ON a.business_unit_id = d.id
LEFT JOIN
	paytypehours e ON a.MemberTime_PayTypeHours_ID = e.PayTypeHours_ID
WHERE 
	a.business_unit_id IN ({0}) and 
	a.date BETWEEN @v0 and @v1
ORDER BY 
	a.business_unit_id,a.date", csv_branches), new object[] { Toolbox.MySQL_shortdt(dte_start_by_member.Date), Toolbox.MySQL_shortdt(dt_end_by_member.Date) });

            }


        }

        gv_hu.DataSource = Session["gv_hu_member"];
        gv_hu.DataBind();

    }

	protected void gv_hu_CustomJSProperties(object _sender, ASPxGridViewClientJSPropertiesEventArgs _e)
	{
		var gv = (ASPxGridView)_sender;
		_e.Properties["cpExp"] = gv.SaveClientLayout();
	}

   

    protected void gv_hu_CustomCallback(object _sender, ASPxGridViewCustomCallbackEventArgs _e)
	{
		var gv = (ASPxGridView)_sender;
		if (_e.Parameters != "")
		{
			gv.LoadClientLayout(_e.Parameters);
		}
		else
		{
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
	protected void bt_submit_Click(object _sender, EventArgs _e)
	{
		using (var conn = Toolbox.connect())
		{
			double dbl_total_hours = 0;
			double dbl_total_quoted = 0;
			double dbl_total_billable = 0;
			double dbl_total_shop_time = 0;
			double dbl_total_invoiced_net = 0;

			double dbl_work_order_total_hours = 0;
			double dbl_work_order_total_quoted = 0;
			double dbl_work_order_total_billable = 0;
			double dbl_work_order_total_shop_time = 0;

			double dbl_customer_total_hours = 0;
			double dbl_customer_total_quoted = 0;
			double dbl_customer_total_billable = 0;
			double dbl_customer_total_shop_time = 0;
			double dbl_customer_total_invoiced_net = 0;

			double dbl_division_total_hours = 0;
			double dbl_division_total_quoted = 0;
			double dbl_division_total_billable = 0;
			double dbl_division_total_shop_time = 0;
			double dbl_division_total_invoiced_net = 0;

			double dbl_company_total_hours = 0;
			double dbl_company_total_quoted = 0;
			double dbl_company_total_billable = 0;
			double dbl_company_total_shop_time = 0;
			double dbl_compamy_total_invoiced_net = 0;

			double removed = 0;
			string str_selected_date;
			string str_ending_date;


			var cust_num_hold = "NOTHING";
			var wo_hold = "NOTHING";
			var prev_wo = 0;
			var str_name_hold = "NOTHING";
			var str_ne_name_hold = "";
			var str_ne_division_hold = "NOTHING";
			var str_ne_division_name = "";
			var company_hold = -1;
			var int_first_run_flag = 0;
			var str_employee_row = new StringBuilder();
			var str_company_row = new StringBuilder();
			var str_work_order_row = new StringBuilder();
			var str_customer_row = new StringBuilder();
			var str_division_row = new StringBuilder();


			str_selected_date = dte_start_by_customer.Text;
			str_ending_date = dte_end_by_customer.Text;

			if (str_ending_date == "")
			{
				str_ending_date = DateTime.Now.ToString("yyyy-MM-dd");
			}

			if (str_selected_date == "")
			{
				str_selected_date = DateTime.Now.ToString("yyyy-MM-dd");

			}

			var sql_comp_addon = "";
			if (ddlCompany.SelectedValue != "0")
			{
				sql_comp_addon = string.Format(" AND id= '{0}' ", ddlCompany.SelectedValue);
			}

			var str_emp_qualifier = "";
			if (ddlUsers.SelectedValue != "0")
			{
				str_emp_qualifier = string.Format(" d.member_id='{0}' AND ", ddlUsers.SelectedValue);
			}
			// This needs to be checked before we launch consol... company_bvno is completely being removed
			var dt_companies = Toolbox.doSQL_dt(conn, (@"SELECT id, name from business_unit WHERE enable_timesheet=1 " + sql_comp_addon), null );

			foreach (DataRow c_dr in dt_companies.Rows)
			{
				var business_unit_id = Convert.ToInt32(c_dr["id"]);
				var name = c_dr["name"].ToString();

			var dt_memberinfo = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT 
	a.membertime_cust_no,
	a.membertime_customer_name,
	a.business_unit_id,
	a.membertime_workorder_id,
	SUM(a.numberofhours) as sumofnumberofhours,
	a.wotype,
	d.business_unit_id,
	d.member_fullname Member_FirstName,
	a.membertime_woprog_id, 
	IF(b.woprog_department IS NULL, d.member_div, b.woprog_department) department,
	c.ddl_name business_unit,
	a.membertime_woprog_id as woid 
FROM 
	membertime a 
LEFT JOIN 
	woprog b ON a.membertime_woprog_id = b.woprog_id 
LEFT JOIN 
	business_unit c ON b.business_unit_id = c.id
LEFT JOIN
	member d ON a.membertime_memberid = d.member_id 
WHERE 
	a.date BETWEEN @v0 AND @v1 AND 
	a.membertime_mileage = 'false' AND 
	a.business_unit_id =@v2 AND 
	d.business_unit_id =@v2 AND 
	{0} 
	d.member_membertype_id NOT IN (SELECT membertype_id FROM membertype where active = 1 and (reports_to = 35 OR membertype_id IN (5,13,9,52))) AND 
	a.membertime_cust_no != 101198 AND 
	(a.membertime_woprog_id != 0 OR a.WOType ='Quote') 
GROUP BY 
	a.business_unit_id, 
	a.membertime_workorder_id, 
	a.membertime_cust_no, 
	department
HAVING 
	a.membertime_cust_no!= ''
ORDER BY 
	department, 
	a.membertime_customer_name, 
	c.id, 
	a.membertime_cust_no,
	a.membertime_workorder_id, 
	a.wotype", str_emp_qualifier),new object[] { str_selected_date, Convert.ToDateTime(str_ending_date), business_unit_id });


				//Select Internal Copmpany No From internal_companyno
				var internal_companies_csv = Toolbox.doSQL_string(conn, @"SELECT GROUP_CONCAT(internal_companyno_bvno) FROM internal_companyno  WHERE internal_companyno_bvno != internal_companyno_intranet_custid AND business_unit_id =@v0", new object[] { business_unit_id });

				var comp_no = internal_companies_csv.Split(',').ToList();

				foreach (DataRow mi_dr in dt_memberinfo.Rows)// loop through timesheets
				{
					var workorder_id = mi_dr["membertime_workorder_id"].ToString();
					var customer_no = mi_dr["membertime_cust_no"].ToString().Trim();
					var department = mi_dr["department"].ToString();
					var hours = Convert.ToDouble(mi_dr["sumofnumberofhours"]);
					var mt_business_unit_id = Convert.ToInt32(mi_dr["business_unit_id"]);
					var wotype = mi_dr["wotype"].ToString();
					var first_name = mi_dr["member_firstname"].ToString();
					var woid = Convert.ToInt32(mi_dr["woid"]);
					var customer_name = mi_dr["membertime_customer_name"].ToString();
				var division_name = mi_dr["business_unit"].ToString();
					//For Work Orders.
					if (wo_hold != workorder_id)
					{

						if (int_first_run_flag != 0)
						{
							//Calculate Values for Previous Work Order
							removed = 0;
							removed = get_invoiced_net(prev_wo, conn);
							dbl_total_invoiced_net += removed;
							dbl_customer_total_invoiced_net += removed;
							dbl_division_total_invoiced_net += removed;
							dbl_compamy_total_invoiced_net += removed;

							str_work_order_row.Append(str_employee_row);
							append_table(wo_hold, dbl_work_order_total_quoted, dbl_work_order_total_shop_time, dbl_work_order_total_billable, dbl_work_order_total_hours, 2, cust_num_hold.Trim() + company_hold.ToString().Trim(), wo_hold.Trim() + company_hold.ToString().Trim(), removed, ref str_work_order_row);
							//Set Counter to 0 For Work Order
							dbl_work_order_total_hours = 0;
							dbl_work_order_total_quoted = 0;
							dbl_work_order_total_billable = 0;
							dbl_work_order_total_shop_time = 0;
							str_employee_row.Clear();
						}
						else
						{

						}
					}

					//For Customer

					if (cust_num_hold != customer_no)
					{
						if (int_first_run_flag != 0)
						{
							str_customer_row.Append(str_work_order_row);
							append_table(str_name_hold, dbl_customer_total_quoted, dbl_customer_total_shop_time, dbl_customer_total_billable, dbl_customer_total_hours, 3, str_ne_division_hold.Trim() + company_hold.ToString().Trim(), cust_num_hold.Trim() + company_hold.ToString().Trim(), dbl_customer_total_invoiced_net, ref str_customer_row);
							//Set Counter to 0 For Customer
							dbl_customer_total_hours = 0;
							dbl_customer_total_quoted = 0;
							dbl_customer_total_billable = 0;
							dbl_customer_total_shop_time = 0;
							dbl_customer_total_invoiced_net = 0;
							str_work_order_row.Clear();
						}
					}

					//For NE Division
					if (str_ne_division_hold != department)
					{

						if (int_first_run_flag != 0)
						{
							str_division_row.Append(str_customer_row);
							append_table(str_ne_division_name, dbl_division_total_quoted, dbl_division_total_shop_time, dbl_division_total_billable, dbl_division_total_hours, 5, company_hold.ToString(), str_ne_division_hold.Trim() + company_hold.ToString().Trim(), dbl_division_total_invoiced_net, ref str_division_row);
							dbl_division_total_hours = 0;
							dbl_division_total_quoted = 0;
							dbl_division_total_billable = 0;
							dbl_division_total_shop_time = 0;
							dbl_division_total_invoiced_net = 0;
							str_customer_row.Clear();
						}

					}

					//For New Electric Company
					if (company_hold != business_unit_id)
					{
						if (int_first_run_flag != 0)
						{
							//Calculate Values for Previous Company
							str_company_row.Append(str_division_row);
							append_table(str_ne_name_hold, dbl_company_total_quoted, dbl_company_total_shop_time, dbl_company_total_billable, dbl_company_total_hours, 4, "0", company_hold.ToString(), dbl_compamy_total_invoiced_net, ref str_company_row);

							//Set Counter to 0 For Comopany
							dbl_company_total_hours = 0;
							dbl_company_total_quoted = 0;
							dbl_company_total_billable = 0;
							dbl_company_total_shop_time = 0;

							str_division_row.Clear();
						}
					}
					int_first_run_flag = 1;

					var int_flag = comp_no.Contains(customer_no) ? 1 : 0;
					dbl_total_hours += hours;
					dbl_customer_total_hours += hours;
					dbl_company_total_hours += hours;
					dbl_work_order_total_hours += hours;
					dbl_division_total_hours += hours;
					int int_company_number;
					int.TryParse(customer_no, out int_company_number);

					if (wotype == "Quote")
					{
						dbl_total_quoted += hours;
						dbl_customer_total_quoted += hours;
						dbl_company_total_quoted += hours;
						dbl_work_order_total_quoted += hours;
						dbl_division_total_quoted += hours;
					}
					else if (wotype == "Shop")
					{
						dbl_total_shop_time += hours;
						dbl_customer_total_shop_time += hours;
						dbl_company_total_shop_time += hours;
						dbl_work_order_total_shop_time += hours;
						dbl_division_total_shop_time += hours;
					}
					else if (int_company_number != 0 && int_flag == 0)
					{
						dbl_total_billable += hours;
						dbl_customer_total_billable += hours;
						dbl_company_total_billable += hours;
						dbl_work_order_total_billable += hours;
						dbl_division_total_billable += hours;
					}
					else
					{
						dbl_total_shop_time += hours;
						dbl_customer_total_shop_time += hours;
						dbl_company_total_shop_time += hours;
						dbl_work_order_total_shop_time += hours;
						dbl_division_total_shop_time += hours;
					}


					append_table(first_name, dbl_total_quoted, dbl_total_shop_time, dbl_total_billable, dbl_total_hours, 1, workorder_id + mt_business_unit_id, "PERSON", 0, ref str_employee_row);


					wo_hold = workorder_id;
					prev_wo = woid;
					cust_num_hold = customer_no;

					company_hold = mt_business_unit_id;
					str_name_hold = customer_name;
					str_ne_division_hold = department;
					str_ne_division_name = division_name;
					str_ne_name_hold = name;

					dbl_total_hours = 0;
					dbl_total_quoted = 0;
					dbl_total_billable = 0;
					dbl_total_shop_time = 0;

				}// end of timesheet entry loop

			}// end of company loop

			str_work_order_row.Append(str_employee_row);
			str_customer_row.Append(str_work_order_row);
			str_division_row.Append(str_customer_row);
			str_company_row.Append(str_division_row);

			//Print Final Lines For Work Order
			append_table(wo_hold, dbl_work_order_total_quoted, dbl_work_order_total_shop_time, dbl_work_order_total_billable, dbl_work_order_total_hours, 2, cust_num_hold.Trim() + company_hold, wo_hold.Trim() + company_hold.ToString().Trim(), 0, ref str_company_row);
			//Set Counter to 0 For Work Order
			dbl_work_order_total_hours = 0;
			dbl_work_order_total_quoted = 0;
			dbl_work_order_total_billable = 0;
			dbl_work_order_total_shop_time = 0;


			//Print Final Line For Customer
			append_table(str_name_hold, dbl_customer_total_quoted, dbl_customer_total_shop_time, dbl_customer_total_billable, dbl_customer_total_hours, 3, str_ne_division_hold.Trim() + company_hold, cust_num_hold.Trim() + company_hold.ToString().Trim(), 0, ref str_company_row);
			//Set Counter to 0 For Customer
			dbl_customer_total_hours = 0;
			dbl_customer_total_quoted = 0;
			dbl_customer_total_billable = 0;
			dbl_customer_total_shop_time = 0;

			//Print Final Line for Department
			append_table(str_ne_division_name, dbl_division_total_quoted, dbl_division_total_shop_time, dbl_division_total_billable, dbl_division_total_hours, 5, company_hold.ToString(), str_ne_division_hold.Trim() + company_hold.ToString().Trim(), dbl_division_total_invoiced_net, ref str_company_row);
			//Set COunter 0 for division
			dbl_division_total_hours = 0;
			dbl_division_total_quoted = 0;
			dbl_division_total_billable = 0;
			dbl_division_total_shop_time = 0;
			dbl_division_total_invoiced_net = 0;

			//Last Company Line
			append_table(str_ne_name_hold, dbl_company_total_quoted, dbl_company_total_shop_time, dbl_company_total_billable, dbl_company_total_hours, 4, "0", company_hold.ToString(), dbl_compamy_total_invoiced_net, ref str_company_row);
			//Set Counter to 0 For Comopany
			dbl_company_total_hours = 0;
			dbl_company_total_quoted = 0;
			dbl_company_total_billable = 0;
			dbl_company_total_shop_time = 0;
			dbl_compamy_total_invoiced_net = 0;


			lblCurrentView.Text = "Currently Viewing from : " + str_selected_date + " to " + str_ending_date;
			var tableheader = "<table style='text-align:left' id='UtilizationTable' cellpadding='5' cellspacing='0' style='border-width: 3px 3px 3px 3px; border-style: solid' width='900px'>";
			var str_table_headings = "<tr><td>Company/Department/Customer/Workorder/Name</td><td>Quoted Time</td><td>Shop Time</td><td>Billed Time</td><td>Removed</td><td>Total Time</td><td>Utilization of Time</td></tr>";
			var str_bottom_row = "<tr><td>Company/Department/Customer/Workorder/Name</td><td>Quoted Time</td><td>Shop Time</td><td>Billed Time</td><td>Removed</td><td>Total Time</td><td>Utilization of Time</td>";
			detail.InnerHtml = tableheader + str_company_row + str_bottom_row + "</table>";
			Td1.InnerHtml = tableheader + str_table_headings + "</tr></table>";

			try
			{

				var bogus_hours = Toolbox.doSQL_double(conn, @"SELECT ifnull(sum(a.NumberOfHours),0) 
FROM membertime a INNER JOIN member b on a.business_unit_id=b.Member_ID  
WHERE a.Date BETWEEN @v0 AND @v1 AND a.Membertime_Mileage = 'false' and a.business_unit_id =@v2 
AND a.business_unit_id = b.Member_ID AND b.member_membertype_id 
NOT IN (SELECT membertype_id FROM membertype where active = 1 and (reports_to = 35 OR membertype_id IN (5,13,9,52)))
AND (a.membertime_woprog_id != 0 or a.WOType ='Quote') and a.business_unit_id != a.Member_ID_Audit", new object[] { str_selected_date, str_ending_date, ddlCompany.SelectedValue });


				tableheader = "<table style='text-align:left' id='DumpedTable' cellpadding='5' cellspacing='0' style='border-width: 3px 3px 3px 3px; border-style: solid' width='500px'>";
				tableheader += "<tr><td>Sum of Hours Entered by Others During This Period:</td><td>" + bogus_hours + "</td><td width:100%></td></tr></table>";
				wos.InnerHtml = tableheader;
			}
			catch { }

			#region figure out net from work order




			#endregion
		}
	}

	private static void append_table(string _str_name, double _dbl_quotes, double _dbl_shop, double _dbl_billable, double _dbl_total, int _int_switch, string _str_id, string _str_click, double _net, ref StringBuilder _table)
	{
		double dbl_hour_utilization = 0;
		switch (_int_switch)
		{
			case 1:
				_table.AppendFormat("<tr class='{0}' style='display:none'>", _str_id);
				break;
			case 2:
				_table.AppendFormat("<tr style='background-color:#FFFFCC; display:none' class='{0}'>", _str_id);
				break;
			case 3:
				_table.AppendFormat("<tr style='background-color:cyan; display:none' class='{0}'>", _str_id);
				break;
			case 5:
				_table.AppendFormat("<tr style='background-color:#99FF30; display:none' class='{0}'>", _str_id);
				break;
			case 4:
				_table.AppendFormat("<tr style='background-color:#99FF33' class='{0}'>", _str_id);
				break;
			default:
				_table.AppendFormat("<tr class='{0}'>", _str_id);
				break;
		}




		if (_dbl_total != 0)
		{
			dbl_hour_utilization = (_dbl_billable - _net) / _dbl_total;
			//Math.Round(dblHourUtilization, 2);
			dbl_hour_utilization = dbl_hour_utilization * 100;
		}
		var str_hour_utilize = dbl_hour_utilization.ToString("#.##");
		if (str_hour_utilize != "")
		{
			str_hour_utilize = str_hour_utilize + "%";
		}
		else
		{
			str_hour_utilize = "0.00%";
		}

		//Math.Round(dblBillable, 2);
		var str_total_bill = _dbl_billable.ToString("#.##");

		//Math.Round(dblTotal, 2);
		var str_total_hours = _dbl_total.ToString("#.##");

		//Math.Round(dblShop, 2);
		var str_total_shop_time = _dbl_shop.ToString("#.##");

		//Math.Round(dblQuotes, 2);
		var str_total_quote_time = _dbl_quotes.ToString("#.##");

		//Math.Round(net, 2);
		var nettime = _net.ToString("#.##");

		if (str_total_shop_time == "")
		{
			str_total_shop_time = "0";
		}
		if (str_total_hours == "")
		{
			str_total_hours = "0";
		}
		if (str_total_bill == "")
		{
			str_total_bill = "0";
		}
		if (str_total_quote_time == "")
		{
			str_total_quote_time = "0";
		}
		if (nettime == "")
		{
			nettime = "0";
		}

		if (_str_click == "PERSON")
		{
			_table.AppendFormat("<td style='border-width: 1px 1px 1px 1px; border-style: solid' nowrap>{0}</td>\n", _str_name);
		}
		else
		{
			_table.AppendFormat("<td style='border-width: 1px 1px 1px 1px; border-style: solid' nowrap> <a href='javascript:void(0);' onclick=\"ToggleRow('{0}')\">{1}</a></td>\n", _str_click, _str_name);
		}
		_table.AppendFormat(@"
		<td style='border-width: 1px 1px 1px 1px; border-style: solid' nowrap>{0}</td>
		<td style='border-width: 1px 1px 1px 1px; border-style: solid' nowrap>{1}</td>
		<td style='border-width: 1px 1px 1px 1px; border-style: solid; background-color:#FFFFCC;' nowrap>{2}</td>
		<td style='border-width: 1px 1px 1px 1px; border-style: solid' nowrap>{3}</td>
		<td style='border-width: 1px 1px 1px 1px; border-style: solid' nowrap>{4}</td>
		<td style='border-width: 1px 1px 1px 1px; border-style: solid' nowrap>{5}</td></tr>", str_total_quote_time, str_total_shop_time, str_total_bill, nettime, str_total_hours, str_hour_utilize);
	}

	private double get_invoiced_net(int _woid, MySqlConnection _conn)
	{
		var removed_ = Toolbox.doSQL_double(_conn, @" SELECT ifnull(SUM(wo_detail_current_qty_committed),0) s FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_billtypeid IN (5) AND wo_detail_current_type = 'L' AND wo_detail_current_origin <> 'Manually Added' and wo_detail_current_origin <> 'Automatically Added' ", new object[] { _woid });
		if (removed_ == 0)
		{
			removed_ = Toolbox.doSQL_double(_conn, @" SELECT ifnull(SUM(wo_detail_history_qty_committed),0) s FROM wo_detail_history WHERE wo_detail_history_woprog_id = @v0  AND wo_detail_history_billtypeid IN (5) AND wo_detail_history_type = 'L' AND wo_detail_history_origin <> 'Manually Added' and wo_detail_history_origin <> 'Automatically Added' ", new object[] { _woid });
		}

		var added_ = Toolbox.doSQL_double(_conn, @" SELECT ifnull(SUM(wo_detail_current_qty_committed),0) s FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_billtypeid IN (0,1) AND wo_detail_current_type = 'L' AND wo_detail_current_origin LIKE '%Manually%' ", new object[] { _woid });
		if (added_ == 0)
		{
			added_ = Toolbox.doSQL_double(_conn, @" SELECT ifnull(SUM(wo_detail_history_qty_committed),0) s FROM wo_detail_history WHERE wo_detail_history_woprog_id = @v0  AND wo_detail_history_billtypeid IN (0,1) AND wo_detail_history_type = 'L' AND wo_detail_history_origin LIKE '%Manually%' ", new object[] { _woid });
		}


		var result = removed_ - added_;
		return result;
	}

	protected void ddlCompany_SelectedIndexChanged(object _sender, EventArgs _e)
	{
		detail.InnerHtml = "";
		lblCurrentView.Text = "";
		fill_users();
		var lic2 = new ListItem("All Users", "0");
		ddlUsers.Items.Insert(0, lic2);
	}
	private void fill_users()
	{
		ddlUsers.DataSource = Toolbox.doSQL_dt(@" SELECT member_id, member_fullname FROM member, membertime WHERE member.business_unit_id = @v0  AND member_id = membertime_memberid AND member_membertype_id NOT IN (SELECT membertype_id FROM membertype where active = 1 and (reports_to = 35 OR membertype_id IN (5,13,9,52))) AND date BETWEEN @v1  AND @v2  GROUP BY membertime_memberid ORDER BY member_nickname,member_lastname ", new object[] { ddlCompany.SelectedValue, Toolbox.MySQL_shortdt(dte_start_by_customer.Date), Toolbox.MySQL_shortdt(dte_end_by_customer.Date) });
		ddlUsers.DataBind();
	}
	protected void Button2_Click(object _sender, EventArgs _e)
	{
		Session["gv_hu_member"] = null;
		fill_gv_hu_member();
	}
	protected void ASPxGridView1_SummaryDisplayText(object _sender, ASPxGridViewSummaryDisplayTextEventArgs _e)
	{
		using (var conn = Toolbox.connect())
		{
			if (!_e.IsGroupSummary)
			{
				return;
			}
			if (_e.Item.FieldName != "membername")
			{
				return;
			}
			var memberid = Convert.ToInt32(gv_hu.GetRowValues(_e.VisibleIndex, "membertime_memberid"));
		var branch = gv_hu.GetRowValues(_e.VisibleIndex, "business_unit").ToString();


			var sum = Toolbox.doSQL_double(conn, @"Select ifnull(sum(numberofhours),0) from membertime  where date >=@v0 and date <=@v1  and membertime_memberid =@v2 ", new object[] { dte_start_by_member.Date.ToString("yyyy-MM-dd"), dt_end_by_member.Date.ToString("yyyy-MM-dd"), memberid });
			var sum_shop = Toolbox.doSQL_double(conn, @"Select ifnull(sum(numberofhours),0) from membertime  where wotype = 'Shop' AND date >=@v0 AND date <=@v1  and membertime_memberid =@v2 ", new object[] { dte_start_by_member.Date.ToString("yyyy-MM-dd"), dt_end_by_member.Date.ToString("yyyy-MM-dd"), memberid });
			var sum_quote = Toolbox.doSQL_double(conn, @"Select ifnull(sum(numberofhours),0) from membertime  where wotype = 'Quote' AND date >=@v0 AND date <=@v1  and membertime_memberid =@v2 ", new object[] { dte_start_by_member.Date.ToString("yyyy-MM-dd"), dt_end_by_member.Date.ToString("yyyy-MM-dd"), memberid });
			var sum_telem = Toolbox.doSQL_double(conn, @"Select ifnull(sum(numberofhours),0) from membertime  where wotype = 'Telem' AND date >=@v0 AND date <=@v1  and membertime_memberid =@v2 ", new object[] { dte_start_by_member.Date.ToString("yyyy-MM-dd"), dt_end_by_member.Date.ToString("yyyy-MM-dd"), memberid });
			double sum_shopwo = 0;
			if (_compss.Length > 0)
			{
				sum_shopwo = Toolbox.doSQL_double(conn, string.Format(@"Select ifnull(sum(numberofhours),0) from membertime  
where wotype = 'WO' AND membertime_customer_id in ({0}) AND date >=@v0  and date <=@v1  and membertime_memberid =@v2 ", _compss.Remove(_compss.Length - 1, 1)),
new object[] { dte_start_by_member.Date.ToString("yyyy-MM-dd"), dt_end_by_member.Date.ToString("yyyy-MM-dd"), memberid });
			}
			if (sum != 0)
			{
			_e.Text = "Business Unit: "+branch+" -- HU:" + Convert.ToDecimal((sum - (sum_shop + sum_quote + sum_shopwo + sum_telem)) / sum).ToString("P1");
			}
		}
	}
	protected void ASPxGridView1_HtmlRowPrepared(object _sender, ASPxGridViewTableRowEventArgs _e)
	{
		if (_e.RowType == GridViewRowType.Group)
		{
			if (_e.VisibleIndex >= 0)
			{
				var s = gv_hu.GetGroupRowSummaryText(_e.VisibleIndex).Replace(")", "").Split(':');
				try
				{
					if (s.Length > 1)
					{
						if (Convert.ToDouble(s[1].Remove(s[1].Length - 1, 1)) < 90)
						{
							_e.Row.BackColor = System.Drawing.Color.Yellow;
						}
					}
				}
				catch
				{
					// ignored
				}
			}
		}
	}
	protected void ASPxPageControl1_ActiveTabChanged(object _source, TabControlEventArgs _e)
	{
		if (ASPxPageControl1.ActiveTabIndex == 2)
		{
			var past_days_in_month = get_business_days(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today);
			var days_in_month = get_business_days(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, 1).AddDays(-1));
			var p_complete = past_days_in_month / days_in_month;
			txt_perc_reported.Text = p_complete.ToString("P1");
			txt_daysinmonth.Text = days_in_month.ToString("N0");
			txt_reported.Text = past_days_in_month.ToString("N0");
		

		}
	}

    private void fill_hu_project()
    {

        if (Session["hu_projected"] == null)
        {
            Session["hu_projected"] = _tools.getSQL_datatable(@"
SELECT 
c.id business_unit_id,
c.ddl_name business_unit,
IFNULL((
SELECT 
	IFNULL(SUM(NumberOfHours),0)
FROM
	membertime sub_mt
INNER JOIN 
	business_unit sub_bu ON sub_mt.business_unit_id = sub_bu.id 
WHERE
	sub_mt.business_unit_id = c.id AND
	sub_mt.WOType = 'WO' AND 
	MONTH(sub_mt.Date)= MONTH(CURDATE()-INTERVAL 1 MONTH) AND 
	YEAR(sub_mt.Date) = YEAR(CURDATE()-INTERVAL 1 MONTH) AND
	sub_mt.MemberTime_Customer_ID NOT IN (SELECT Internal_CompanyNo_Intranet_CustID FROM internal_companyno  WHERE  business_unit_id = sub_mt.business_unit_id)
GROUP BY
	sub_mt.business_unit_id,
	sub_bu.name 
), 0) last_month,
IFNULL((
SELECT 
	IFNULL(SUM(NumberOfHours),0)
FROM
	membertime sub_mt
INNER JOIN 
	business_unit sub_bu ON sub_mt.business_unit_id = sub_bu.id 
WHERE
	sub_mt.business_unit_id = c.id AND
	sub_mt.WOType = 'WO' AND 
	MONTH(sub_mt.Date)= MONTH(CURDATE()) AND 
	YEAR(sub_mt.Date) = YEAR(CURDATE()) AND
	sub_mt.MemberTime_Customer_ID NOT IN (SELECT Internal_CompanyNo_Intranet_CustID FROM internal_companyno  WHERE  business_unit_id = sub_mt.business_unit_id)
GROUP BY
	sub_mt.business_unit_id,
	sub_bu.name 
), 0) this_month,
IFNULL((SELECT SUM(membertime.NumberOfHours)
FROM
membertime
WHERE
membertime.business_unit_id = c.id AND
membertime.WOType = 'WO' AND
MONTH(membertime.Date)= MONTH(CURDATE()-INTERVAL 1 YEAR) AND 
YEAR(membertime.Date) = YEAR(CURDATE()-INTERVAL 1 YEAR) AND
membertime.MemberTime_Customer_ID NOT IN (SELECT internal_companyno.Internal_CompanyNo_Intranet_CustID FROM internal_companyno  WHERE  internal_companyno.business_unit_id = membertime.business_unit_id  )
GROUP BY
membertime.business_unit_id
),0) last_year


FROM business_unit  c WHERE c.enable_timesheet=1
", null);

        }

        gv_hu_projected.DataSource = Session["hu_projected"];
        gv_hu_projected.DataBind();


    }

	private static double get_business_days(DateTime _start_d, DateTime _end_d)
	{
		var calc_business_days =
			1 + ((_end_d - _start_d).TotalDays * 5 -
			(_start_d.DayOfWeek - _end_d.DayOfWeek) * 2) / 7;

		if ((int)_end_d.DayOfWeek == 6) calc_business_days--;
		if ((int)_start_d.DayOfWeek == 0) calc_business_days--;

		return calc_business_days;
	}
	protected void ASPxGridView1_HtmlDataCellPrepared(object _sender, ASPxGridViewTableDataCellEventArgs _e)
	{
		if (_e.VisibleIndex < 0) return;
		var gv = (ASPxGridView)_sender;
		var this_month = Convert.ToDouble(gv.GetRowValues(_e.VisibleIndex, "this_month"));
		var last_month = Convert.ToDouble(gv.GetRowValues(_e.VisibleIndex, "last_month"));
		var last_year = Convert.ToDouble(gv.GetRowValues(_e.VisibleIndex, "last_year"));
		double reported_pct;
		double.TryParse(txt_perc_reported.Text.Replace("%", ""), out reported_pct);
		if (_e.DataColumn.Caption.Equals("Projected"))
		{
			var mult = Convert.ToDouble(100 - reported_pct) * 0.01;
			_e.Cell.Text = (this_month + this_month * mult).ToString("N0");
			Session["hu_projected_temp"] = _e.Cell.Text;
		}
		if (_e.DataColumn.Caption.Equals("M-o-M Change"))
		{
			_e.Cell.Text = ((Convert.ToDouble(Session["hu_projected_temp"]) - last_month) / last_month).ToString("P1");
		}
		if (_e.DataColumn.Caption.Equals("Y-o-Y Change"))
		{
			_e.Cell.Text = ((Convert.ToDouble(Session["hu_projected_temp"]) - last_year) / last_year).ToString("P1");
		}
	}
	protected void ASPxGridView1_FocusedRowChanged(object _sender, EventArgs _e)
	{
		if (gv_hu_projected.FocusedRowIndex >= 0)
		{
			load_org(gv_hu_projected.GetRowValues(gv_hu_projected.FocusedRowIndex, "business_unit_id"));
		}
	}

	private void load_org(object _comapny_id)
	{

		//	NeBusinessUnit c = new NeBusinessUnit(comapny_id);
		var datarows = "";
		//	NeMember ceo_level = new NeMember(Convert.ToInt32(_tools.getSQL_int(@"Select ifnull((select member_id from member  where member_status = 'Active' and reports_to = 0 and substring(member.member_country,1,1) =@v0 limit 1),0) ", new object[] { c.country.Substring(0, 1) });
		//	string img_builder = @"<table width=""100px""><tbody><tr><td style=""white-space:nowrap; text-align:left;"">" + ceo_level.FullName2 + @"</td><td rowspan=""3""><img src=""/_tools/member_photo/index.aspx?member_id=" + ceo_level.id + @""" width=""60"" ></td></tr><tr><td style=""color:red; white-space:nowrap; text-align:left; "">" + ceo_level.membertype.name + @"</td></tr><tr><td></td></tr></tbody></table>";



		var dt = _tools.getSQL_datatable(@"Select Sum(membertime.NumberOfHours),concat(year(membertime.Date),'-',month(membertime.Date)) _Date FROM membertime  WHERE membertime.business_unit_id=@v0 and membertime.date>=curdate()-interval 2 year and membertime.WOType = 'WO' AND membertime.MemberTime_Customer_ID not IN (Select internal_companyno.Internal_CompanyNo_Intranet_CustID from internal_companyno where internal_companyno.business_unit_id = membertime.business_unit_id ) GROUP BY concat(year(membertime.Date),'-',month(membertime.Date)) order by date(concat(year(membertime.Date),'-',month(membertime.Date),'-01')) limit 24", new object[] { _comapny_id });


		for (var x = 0; x < 12; x++)
		{
			var this_year = 0;
			var last_year = 0;
			var date = DateTime.Today.AddYears(-2).AddMonths(x).Year + "-" + DateTime.Today.AddYears(-2).AddMonths(x).Month;
			var filter_lastyear = DateTime.Today.AddYears(-2).AddMonths(x).Year + "-" + DateTime.Today.AddYears(-2).AddMonths(x).Month;
			var filter_thisyear = DateTime.Today.AddYears(-1).AddMonths(x).Year + "-" + DateTime.Today.AddYears(-1).AddMonths(x).Month;
			if (dt.Select("_Date='" + filter_lastyear + "'").Length != 0)
			{
				this_year = Convert.ToInt32(dt.Select("_Date='" + filter_lastyear + "'").CopyToDataTable().Rows[0][0]);
			}
			if (dt.Select("_Date='" + filter_thisyear + "'").Length != 0)
			{
				last_year = Convert.ToInt32(dt.Select("_Date='" + filter_thisyear + "'").CopyToDataTable().Rows[0][0]);
			}


			datarows += "['" + date + "'," + last_year + "," + this_year + "],";

		}

		hist.InnerHtml = @"<html>
  <head>
    <script type='text/javascript' src='https://www.gstatic.com/charts/loader.js'></script>
    <script type='text/javascript'>
      google.charts.load('current',  {packages:['line']});
      google.charts.setOnLoadCallback(drawChart);

      function drawChart() {
        var data = new google.visualization.DataTable();
 data.addColumn('string', 'Date');
        data.addColumn('number', 'This_Year');
        data.addColumn('number', 'Last_Year');
        data.addRows([
          " + datarows.TrimEnd(',') + @"
        ]);

 var options = {
        chart: {
          title: 'Billable Hours past 12 months'
         
        },
        width: 900,
        height: 500,
        axes: {
          x: {
            0: {side: 'bottom'}
          }

        }

      };


var chart = new google.charts.Line(document.getElementById('chart_div'));




        chart.draw(data,options);
      }
    </script>
  </head>

  <body>
    <div id='chart_div'></div>
  </body>
</html>";

	}
}
