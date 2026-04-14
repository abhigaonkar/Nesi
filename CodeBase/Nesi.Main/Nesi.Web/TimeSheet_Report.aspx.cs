using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using DevExpress.Web;
using System.Collections;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using nesi.core;
using NESI.BLL.Pages.Timesheet.BreakTime;
using NESI.Common.Models;
using System.Collections.Generic;

public partial class TimeSheet_Report : System.Web.UI.Page
	{
	NeMember current_user;
	NeBusinessUnit businessUnit;
	private const int _page_id = 30; // from Page table in DB
	private const string _page_description = "Timesheet Reports for a Selected Period";
	static string _page_name = "TimesheetReport";
	Toolbox _tools;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	private bool TimeSheetReport_CanViewBranch, TimeSheetReport_CanViewTaxEntity, TimeSheetReport_CanViewEntireCompany, ViewReportsTo ;
	List<int> ReportsToList;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		TimeSheetReport_CanViewBranch = current_user.AuthenticatedForPrivilege(OpsPrivilege.TimeSheetReportCanViewBranch);
		TimeSheetReport_CanViewTaxEntity = current_user.AuthenticatedForPrivilege(OpsPrivilege.TimeSheetReportCanViewTaxEntity);
		TimeSheetReport_CanViewEntireCompany = current_user.AuthenticatedForPrivilege(OpsPrivilege.TimeSheetReportCanViewEntireCompany);
		ReportsToList = NeMember.ReportsToList(current_user.id32);
		ViewReportsTo = (ReportsToList.Any() && !current_user.membertype.name.Contains("Project Manager"));				
		bind_payperiods();
		
		if (!IsPostBack)
			{
			
			chkVacations.Checked = true;
			}
		layout.__page_name = _page_name;
		layout.used_gv = gv_results;
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		h = (ASPxHiddenField)layout.FindControl("h");
		h.Set("gridview_id", "gv_jobcost");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		businessUnit = new NeBusinessUnit(current_user.business_unit_id);
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = _page_description;
		if(!IsPostBack && !IsCallback)
			{
			Session["gv_ts_report"] = null;
			
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
				{
				gv_results.FilterExpression = "";
				gl.GridLayout_Layout = gv_results.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_results.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text = gl.GridLayout_Name;
			}
        bind_gv_results();
    }
	protected void btnSearch_Click(object sender, ImageClickEventArgs e)
		{
		}
	protected void gv_results_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		}
	protected void gv_results_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	private void bind_gv_results()
		{

			if (Session["gv_ts_report"] == null)
			{
			    var filter = "";
				var wotype = "";
				var sb_vac = new StringBuilder();
				sb_vac.Append(@"
SELECT
		c.id,
		c.name,
        d.public_name,
		b.member_id member_id,
		b.member_fullname,
        b.member_status,
		a.date_start,
		a.date_return,
		0 completed,
c.id business_unit_id
FROM 
	vacation_master a
LEFT JOIN
	member b ON a.member_id = b.member_id
LEFT join
	business_unit c
		ON b.business_unit_id = c.id
LEFT JOIN tax_entity d ON c.tax_entity_id = d.id
WHERE 
	
");
				
				if(TimeSheetReport_CanViewEntireCompany)
                {
                    var bu_group = Toolbox.doSQL_string(@" SELECT IFNULL(GET_VISIBLE_BUSINESS_UNITS_GROUP_CONCAT(@v0),'')",new object[] {current_user.id32});
                    filter = $" FIND_IN_SET(e.id, '{bu_group}')";
                    sb_vac.AppendFormat("  FIND_IN_SET(c.id, '{0}')",bu_group);
				}
				else if (TimeSheetReport_CanViewTaxEntity)				
				{
				
				    filter = $" i.id = {businessUnit.tax_entity_id}";
                    sb_vac.Append(" d.id = "+ businessUnit.tax_entity_id);
				}
				else if(TimeSheetReport_CanViewBranch)
                {
                    var bu_group = Toolbox.doSQL_string(@"SELECT id FROM business_unit WHERE acting_manager =@v0 OR id = @v1", new object[] {current_user.id32,current_user.business_unit_id});
				    filter = $" e.id IN {bu_group}";
                    sb_vac.AppendFormat("  FIND_IN_SET(c.id, '{0}')",bu_group);
				}				
				else if(ViewReportsTo)
                {
                    var reportsToList = string.Join(",", ReportsToList.ToArray());
				    filter = $" FIND_IN_SET(a.membertime_memberid,'{reportsToList}')";
                    sb_vac.AppendFormat(" FIND_IN_SET(a.member_id, '{0}') ", reportsToList );
				}
				else
				{
				    filter = $" a.membertime_memberid= {current_user.id32}";
					sb_vac.Append(" a.member_id = "+ current_user.id32);
                }
				

				if (ddl_payperiod.Value != null && (int)ddl_payperiod.Value != 0)
				{
					var getdates = new NePayPeriod(Convert.ToInt32(ddl_payperiod.Value));
					de_start.Date = Convert.ToDateTime(getdates.StartDate);
					de_end.Date = Convert.ToDateTime(getdates.Enddate);
				}
				if (de_end.Date.Year < 1900 && de_start.Date.Year > 1900)
				{
					de_end.Date = de_start.Date;
				}
				else if (de_start.Date.Year < 1900 && de_end.Date.Year > 1900)
				{
					de_start.Date = de_end.Date;
				}
			
				var sb_sql = Toolbox.doSQL_dt(@"CALL report_TimeSheet(@v0,@v1,@v2,@v3)", new object[] {filter,current_user.id32, Toolbox.MySQL_shortdt(de_start.Date), Toolbox.MySQL_shortdt(de_end.Date)});
			
			
				if (de_start.Text != "" && de_end.Text != "")
				{
					sb_vac.AppendFormat(" AND a.date_start BETWEEN '{0}' AND '{1}' AND status IN ('3', '5') ", Toolbox.MySQL_shortdt(de_start.Date), Toolbox.MySQL_shortdt(de_end.Date));
				}
				
			
				var dt_main = sb_sql;
				if (chkVacations.Checked && dt_main.Rows.Count > 0 )
				{
					var sql_vac = sb_vac.ToString();
					var dt_vac = Toolbox.doSQL_dt(sb_vac.ToString(),null);
					if (dt_vac.Rows.Count > 0)
					{
						foreach (DataRow dr in dt_vac.Rows)
						{
							//			if(dt_main.Rows.Count > 1000)
							//				{
							//				continue;
							//				}
							var this_business_unit_id = Convert.ToInt32(dr["business_unit_id"]);
							var this_member_id = Convert.ToInt32(dr["member_id"]);
							var this_name = dr["name"].ToString();
							var this_member_name = dr["member_fullname"].ToString();
							var dt_start = dr["date_start"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(dr["date_start"].ToString());
							var dt_end = dr["date_return"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(dr["date_return"].ToString());
							var diff = dt_end.Subtract(dt_start).Days;
                            var this_member_status = dr["member_status"].ToString();
                            var this_te = dr["public_name"].ToString();

							//for (var d = 0; d < diff; d++)
							//{
							//	var this_dt = dt_start.AddDays(d);
							//	if (this_dt.DayOfWeek == DayOfWeek.Saturday || this_dt.DayOfWeek == DayOfWeek.Sunday)
							//	{
							//		continue;
							//	}
								var new_dr = dt_main.NewRow();
								new_dr["date"] = Toolbox.MySQL_shortdt(dt_start);
								new_dr["wotype"] = "VAC";
								new_dr["workorder"] = DBNull.Value;
								new_dr["custno"] = DBNull.Value;
								new_dr["customername"] = "Vacation Day";
								new_dr["hours"] = DBNull.Value;
								new_dr["employee"] = this_member_name;
								new_dr["hourtype"] = DBNull.Value;
								new_dr["childworkorder"] = DBNull.Value;
								new_dr["name"] = this_name;
                                new_dr["member_status"] = this_member_status;
                                new_dr["public_name"] = this_te;
								dt_main.Rows.Add(new_dr);
								dt_main.AcceptChanges();
							//}
						}
					}
				}
				dt_main.DefaultView.Sort = "name ASC, employee ASC, date ASC";

				Session["gv_ts_report"] = dt_main;
			}
		gv_results.DataSource = Session["gv_ts_report"];
   
        layout.export_filename ="TS_Report_" +DateTime.Now.ToString(@"dd-MM-yy")+ ".xlsx";
        gv_results.DataBind();
		
		}

	  /*  private void setColumnVisible(int businessId)
	    {
	        IBreakTimeService srv = new BreakTimeService();
	        BranchBreakTimeRequirementInputParameter input = new BranchBreakTimeRequirementInputParameter() { business_unit_id = businessId };
	        var setting = srv.GetBranchBreakTimeRequirement(input);

            bool visible = setting.monitor_breaktime;
	        gv_results.Columns[16].Visible = visible;
	        gv_results.Columns[17].Visible = visible;
	        gv_results.Columns[18].Visible = visible;
	        gv_results.Columns[19].Visible = visible;
	        gv_results.Columns[20].Visible = visible;
	        gv_results.Columns[21].Visible = visible;
	        gv_results.Columns[22].Visible = visible;
        }*/

	    protected void drpPayPeriod_SelectedIndexChanged(object sender, EventArgs e)
		{
		if (ddl_payperiod.Value != null && (int) ddl_payperiod.Value != 0)
			{
			var getdates = new NePayPeriod(Convert.ToInt32(ddl_payperiod.Value));
			de_start.Date = Convert.ToDateTime(getdates.StartDate);
			de_end.Date = Convert.ToDateTime(getdates.Enddate);
			}
		}

	
	
	private void bind_payperiods()
		{
		var pList = new NePayPeriod();
		var payperiods		= pList.LoadPayPeriodList();
		// Need to include current payperiod
		var current_payperiod_id	= Toolbox.doSQL_int("CALL _payperiod()"  );
		var include_current		= true;
		foreach(DataRow ne_p in payperiods.Rows)
			{
			var pp_id = Convert.ToInt32(ne_p["payperiodid"]);
			if(pp_id == current_payperiod_id)
				{
				include_current	= false;
				}
			}
		ddl_payperiod.DataSource = payperiods;
		ddl_payperiod.DataBind();
		if(include_current)
			{
			pList			= new NePayPeriod(current_payperiod_id);
			ddl_payperiod.Items.Insert(0, new ListEditItem { Value= current_payperiod_id, Text= pList.StartDate + " - " + pList.Enddate});
			}
		ddl_payperiod.Items.Insert(0, new ListEditItem { Value = 0, Text = "Select Pay Period " });
		if(!IsPostBack && !IsCallback)
			{
			ddl_payperiod.SelectedIndex		= 1;
			}
		}
	
	
	protected void bt_submit_Click(object sender, EventArgs e)
		{
			Session["gv_ts_report"] = null;
		bind_gv_results();
		}
	protected void ASPxButton1_Click(object sender, EventArgs e)
	{
	}
}
// Started with 538 rows