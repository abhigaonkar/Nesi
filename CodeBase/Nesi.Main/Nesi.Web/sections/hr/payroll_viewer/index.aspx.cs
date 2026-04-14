using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web.Services;
using nesi.core;
using System.IO;
using DevExpress.Web;
using MySql.Data.MySqlClient;
using NESI.Common.Models;
using System.Collections.Generic;
using System.Linq;

public partial class payroll_viewer : Page
	{
	public NeMember current_user;
	private const string page_description = "Payroll Viewer";
	private bool _can_see_wage, _can_export_payroll, _can_toggle_sent;
	private int q_business_unit_id, q_payperiod_id;
	private const string _page_name = "payroll_viewer_summary";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	private string VisibleBusinessUnits { get; set; }

	protected void Page_Init(object _sender, EventArgs _e)
		{

		current_user = Toolbox.do_handle_authentication(OpsPage.PayrollViewer);
		_can_see_wage = current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewWageAndFileTabs);  //View Wage and File Tabs
		_can_export_payroll = current_user.AuthenticatedForPrivilege(OpsPrivilege.TogglePayPeriodsComplete);
		_can_toggle_sent = current_user.AuthenticatedForPrivilege(OpsPrivilege.ToggleSentStatuses);
		VisibleBusinessUnits = new Current_User().visible_business_units;

		layout.used_gv = gv_summary;
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		h.Set("gridview_id", "gv_summary");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
		}
	protected void Page_Load(object _sender, EventArgs _e)
		{
		using (var conn = Toolbox.connect())
			{

			var q = Request.QueryString;
			if (Page.Master != null)
				{
				var lbltemp = (Label)Page.Master.FindControl("lblHeading");
				lbltemp.Text = page_description;
				}
			var max_payperiod_id = Toolbox.doSQL_string(conn, "CALL _payperiod()", null);
			var start_date = "";
			var end_date = "";
			var date_start = "";
			var date_end = "";
			var payperiod_complete = false;

			if (!string.IsNullOrEmpty(q["C"]))
				{
				int.TryParse(q["C"], out q_business_unit_id);
				}
			if (!string.IsNullOrEmpty(q["P"]))
				{
				int.TryParse(q["P"], out q_payperiod_id);
				}
			if (q_payperiod_id == 0 || !_can_export_payroll)
				{
				pc.TabPages[1].Enabled = false;
				pc.TabPages[2].Enabled = false;
				}
			else if (q_payperiod_id > 0)
				{
				var fileServer = Toolbox.GetRequiredAppSetting("UNC_base_path");
				var path = $@"{fileServer}/nesi_files/global/payperiods/P{q_payperiod_id}";

				if (!Directory.Exists(path))
					{
					Directory.CreateDirectory(path);
					}
				fm.Settings.RootFolder = path;
				}
			if (!IsPostBack)
				{
				var gl = new NeGridLayouts(current_user.id, _page_name);
				if (gl.GridLayoutID == 0)
					{
					gl.GridLayout_Layout = gv_summary.SaveClientLayout();
					gl.member_id = current_user.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();

					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
					}
				else
					{
					gv_summary.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
					}
				dde_filter.Text = gl.GridLayout_Name;
				}
			var pay_period = new NePayPeriod(q_payperiod_id);
			var action = string.IsNullOrEmpty(q["A"])
												? ""
												: q["A"];
			var state = string.IsNullOrEmpty(q["STATE"])
												? ""
												: q["STATE"];
			var where_clause = q_payperiod_id == 0 ? "enddate >= NOW() AND startdate < NOW()" : "payperiodid = " + q_payperiod_id;
			var p_id = Toolbox.doSQL_int(conn, string.Format(@"
SELECT  
	IFNULL(MAX(payperiodid),0)
FROM 
	payperiods 
WHERE 
	{0}
	", where_clause), null);
			var payperiod = new NePayPeriod(p_id);
			q_payperiod_id = payperiod.id;
			start_date = payperiod.start_date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
			date_start = payperiod.start_date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			end_date = payperiod.end_date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
			date_end = payperiod.end_date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			payperiod_complete = payperiod.completed;

			if (action == "RESET" && q_business_unit_id > 0 && q_payperiod_id > 0)
				{
				Toolbox.doSQL_void(conn, @"UPDATE payroll SET sent = 0 WHERE business_unit_id = @v0 AND payperiod_id = @v1", new object[] { q_business_unit_id, q_payperiod_id });
				Response.Redirect("/sections/hr/payroll_viewer/index.aspx?P=" + q_payperiod_id);
				}
			if (action == "CHANGECOMPLETE" && state != null && q_payperiod_id > 0 && _can_export_payroll)
				{
				var _state = Convert.ToInt32(state);
				Toolbox.doSQL_void(conn, @"UPDATE payperiods SET completed = @v0 WHERE payperiodid = @v1", new object[] { state, q_payperiod_id });
				shared.alert_payroll("Pay Period " + q_payperiod_id + "'s completed status has been moved to " + Convert.ToBoolean(_state), "This has been done by: " + current_user.FullName + "<br/>Method used: Manually clicking completed button.");
				if (q["c_id"] != null)
					{
					Response.Redirect("/sections/hr/payroll_viewer/index.aspx?P=" + q_payperiod_id + "&C=" + q["c_id"]);
					}
				else
					{
					Response.Redirect("/sections/hr/payroll_viewer/index.aspx?P=" + q_payperiod_id);
					}
				}

			if (!string.IsNullOrEmpty(q["P"]))
				{
				var status_html = new StringBuilder();
				var dt = Toolbox.doSQL_dt(conn, @" 
SELECT 
	b.ddl_name cname, 
	b.id cid, 
	a.sent sent, 
	tax_entity_id, 
	ddl_name 
FROM 
	payroll a 
INNER join 
	business_unit b ON a.business_unit_id = b.id 
WHERE 
	a.payperiod_id = @v0  AND 
	a.sent = 1 AND 
	b.istest = 'F' and 
	b.uses_payroll = 1 AND 
	FIND_IN_SET(b.id, @v1)
UNION
SELECT 
	c.ddl_name, 
	c.id, 
	0, 
	tax_entity_id, 
	ddl_name 
FROM 
	business_unit c 
WHERE 
	FIND_IN_SET(c.id, @v1) and
	c.id NOT IN (SELECT business_unit_id FROM payroll WHERE payperiod_id = @v0  AND sent = 1) AND 
	c.enable_timesheet = 1 AND 
	c.active = 'T' AND 
	c.istest = 'F' AND 
	c.uses_payroll = 1 
ORDER BY 
	tax_entity_id,ddl_name", new object[] { q_payperiod_id, VisibleBusinessUnits });

				var visibleBusinessUnitsArray = VisibleBusinessUnits.Split(',');
				var reportingBusinessUnitsList = new List<int>();
				if (visibleBusinessUnitsArray.Length > 0)
					{
					foreach (DataRow dr in dt.Rows)
						{
						var this_branch_name = dr["cname"].ToString();
						var business_unit_id = Convert.ToInt32(dr["cid"]);
						var isInVisibleBusinessUnits = Array.IndexOf(visibleBusinessUnitsArray, business_unit_id) > 0;

						var outstanding_dt = Toolbox.doSQL_dt(conn, $@"
SELECT
	b.member_fullname name,
	CAST(SUM(numberofhours) AS CHAR) hours,
	c.member_fullname handler
FROM
	membertime a
LEFT JOIN
	member b ON a.membertime_memberid = b.member_id
LEFT JOIN
	member c ON b.payroll_handler = c.member_id
WHERE
	a.date > '{pay_period.StartDate}' AND
	a.date < '{pay_period.Enddate}' AND
	b.business_unit_id = '{business_unit_id}' AND
	a.membertime_memberid IN (SELECT c.member_id FROM member c WHERE c.business_unit_id = '{business_unit_id}' ) AND
	a.membertime_memberid NOT IN (SELECT d.member_id FROM payroll_hours d WHERE d.payperiod_id = '{q_payperiod_id}') AND
	b.paytype_id IN ({OpsMemberPayType.Hourly},
					{OpsMemberPayType.SalaryHourlyWithTimeSheet},
					{OpsMemberPayType.SalaryHourlyWithoutTimeSheet},
					{OpsMemberPayType.Subcontract},
					{OpsMemberPayType.f1099}
					)
GROUP BY 
	b.member_id
UNION
SELECT 
	a.member_fullname name,
	'Salary' hours, 
	b.member_fullname handler
FROM 
	member a
LEFT JOIN
	member b ON a.payroll_handler = b.member_id
WHERE 
	a.paytype_id = {OpsMemberPayType.SalaryHourlyWithoutTimeSheet} AND
	a.member_startdate <= '{pay_period.Enddate}' AND
	a.member_termdate >= '{pay_period.Enddate}' AND 
	a.business_unit_id = {business_unit_id} AND
	a.member_id NOT IN (SELECT d.member_id FROM payroll_hours d WHERE d.payperiod_id = '{q_payperiod_id}')
	", null);
						var is_sent = Convert.ToBoolean(dr["sent"]);
						var subordinatesInBusinessUnit = NeMember.GetSubordinatesForBu(current_user, business_unit_id, pay_period);
						if (isInVisibleBusinessUnits || _can_export_payroll || subordinatesInBusinessUnit.Any())
							{
							reportingBusinessUnitsList.Add(business_unit_id);
							var this_event = $@" onclick=""location.href='./index.aspx?C={business_unit_id}&P={q_payperiod_id}';""";
							var this_sub_class = "active";
							var this_title = " title='Click for details'";
							if (q_business_unit_id > 0)
								{
								if (q_business_unit_id == business_unit_id)
									{
									this_branch_name = "&raquo; " + this_branch_name;
									}
								}
							var icon_class = "";
							var icon_title = "";
							var is_checked = is_sent ? " checked" : "";
							var in_progress = Toolbox.doSQL_int(conn, @"SELECT COUNT(id) FROM payroll_hours WHERE business_unit_id = @v0 AND payperiod_id = @v1", new object[] { business_unit_id, q_payperiod_id }) > 0;
							if (!is_sent)
								{
								if (in_progress)
									{
									icon_class = "inprogress";
									icon_title = "Payroll approval has started, but is not sent.";
									}
								else if (outstanding_dt.Rows.Count > 0)
									{
									icon_class = "alert";
									icon_title = "Not all users have been approved";
									}
								else
									{
									icon_class = "notstarted";
									icon_title = "Payroll approval has not started, approval hasn't been started.";
									}
								}
							else if (is_sent)
								{
								icon_class = "sent";
								icon_title = "Payroll approval has finished";
								is_checked = "checked";
								}
							var chkbox = _can_toggle_sent && (in_progress || is_sent)
														? $"<input type='checkbox' data-pp_id='{q_payperiod_id}' data-c_id='{business_unit_id}' onchange='payroll_viewer.toggle_sent.run(this);' {is_checked}/>"
														: "";
							status_html.AppendFormat(@"
		<div class='company {6}'>
			<div class='toggle'>{5}</div>
			<div class='name' {3} {1}>{0}</div>
			<div class='icon {2}' title=""{4}""></div>
		</div>",
							this_branch_name,
							this_event,
							icon_class,
							this_title,
							icon_title,
							chkbox,
							this_sub_class
							);
							}
						}
					}

				if (q_business_unit_id > 0 && !reportingBusinessUnitsList.Contains(q_business_unit_id))
					{
					Toolbox.FriendlyException(Response, "You have tried to access a branch that is not in your reporting line.", "/index.html");
					}

				if (_can_export_payroll)
					{
					var js_function = q_business_unit_id > 0
						? string.Format("payroll_viewer.toggle_complete({2}, {0}, {1})", q_payperiod_id, q["C"], payperiod_complete ? 0 : 1)
						: string.Format("payroll_viewer.toggle_complete({1}, {0})", q_payperiod_id, payperiod_complete ? 0 : 1);
					if (payperiod_complete)
						{
						status_html.AppendFormat(@"
				<div style='margin-top:5px;'>
					<button id='incomplete' type='button' style='width:100%;' onclick='{0}'><img src='/images/icon/icon[incomplete].gif' align='absmiddle' width='16' height='16' /> Open Payperiod?</button>
				</div>", js_function);
						}
					else
						{
						status_html.AppendFormat(@"
				<div style='margin-top:5px;'>
					<button id='complete' type='button' style='width:100%;' onclick='{0}'><img src='/images/icon/icon[ok].gif' align='absmiddle' width='16' height='16' /> Complete Payperiod</button>
				</div>", js_function);
						}
					}

				if (_can_export_payroll)
					{
					status_html.Append(@"
			<div id='export_business' style='margin-top:5px;'>");

					var reporting_TE = Toolbox.doSQL_string(conn, @"CALL get_visible_tax_entities_group_concat(@v0)", new object[] { current_user.id });

					dt = Toolbox.doSQL_dt(conn, @"select * from tax_entity where find_in_set(id, '" + reporting_TE + "') AND id IN (SELECT DISTINCT(b.tax_entity_id) FROM payroll_hours a  LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE a.payperiod_id = @v0)", new object[] { q_payperiod_id });
					status_html.Append(@"
			<select id='adp_company_code' style='width:100%;margin-top:15px;'>
				<option value='0'>Select Tax Entity</option>");
					var groupedUS = new List<string>();
					foreach (DataRow dr in dt.Rows)
						{
						var region = dr["region"].ToString();
						var code = dr["adp_company_code"].ToString();
						
						if(region == "USA" && !groupedUS.Contains(code))
						{
							//SD: if there are multiple tax entities with same adp_company_code and region, this logic with group them as one option in the DDL. This change is specific to USA region
							groupedUS.Add(code);
							var export_name = !string.IsNullOrEmpty(dr["export_name"].ToString()) ? dr["export_name"].ToString() : "Export";	
							
							var friendlyName = export_name + " - " + code;
							status_html.AppendFormat(@"<option value='{0}'>{1}</option>", code, friendlyName);
						}
						else if(region == "Canada")
						{ 
						var business_name = dr["ddl_name"].ToString();						
						status_html.AppendFormat(@"<option value='{0}'>{1}</option>", code, business_name);}
						}


					status_html.Append(@"
			</select><br/>");
					if (_can_see_wage)
						{
						status_html.Append(@"
				<button type='button' style='width:49%;margin-top:5px;' onclick='payroll_viewer.do_export.wage(this)'>Export Wage</button>
				");
						}
					status_html.AppendFormat(@"
			<button type='button' style='width:{0}%;margin-top:5px;' onclick='payroll_viewer.do_export.payroll(this)'>Export Payroll</button>
			</div>", _can_see_wage ? "49" : "100");
					}



				status_html.AppendFormat(@"<input type='hidden' id='payperiod_id' value='{0}' />", q_payperiod_id);
				branch_selector.InnerHtml = status_html.ToString();
				}
			else
				{
				status.Visible = false;
				}

			if (q_business_unit_id == 0 && q_payperiod_id == 0 || q_business_unit_id == 0 && q_payperiod_id > 0)
				{
				viewer_home.Visible = true;
				var PayPeriodSelector = new StringBuilder();
				PayPeriodSelector.AppendFormat(@"
<table cellpadding='0' cellspacing='0' height='400'>
	<tr>
		<td valign='middle' align='center'>
			<b style='color:#4C6FB7'><b style='color:#888;'>FOR THE PAYPERIOD OF</b><br />{0} to {1}</b><br />", start_date, end_date);

				PayPeriodSelector.Append(@"
			<select id='past_payperiods' onchange='location.href = ""./index.aspx?P=""+this.value' multiple>");
				var dt_pp = Toolbox.doSQL_dt(conn, @" 
SELECT 
	a.payperiodid AS ID, 
	DATE_FORMAT(a.startdate, '%b %d,%Y') AS START, 
	DATE_FORMAT(a.enddate, '%b %d, %Y') AS END, 
	a.completed 
FROM 
	payperiods a 
WHERE 
	a.payperiodid <= @v0
GROUP BY
	a.payperiodid
ORDER BY 
	enddate DESC", new object[] { max_payperiod_id });
				if (dt_pp.Rows.Count > 0)
					{
					foreach (DataRow dr in dt_pp.Rows)
						{
						var id = Convert.ToInt32(dr["ID"]);
						var start = dr["START"].ToString();
						var end = dr["END"].ToString();
						var selected = id == q_payperiod_id ? " selected" : "";
						var completed = dr["completed"].ToString() == "1";
						var is_completed = completed ? "&#10003; - " : "";
						PayPeriodSelector.AppendFormat(@"
				<option value='{2}' {3}>{4}{0} - {1}</option>", start, end, id, selected, is_completed);
						}
					}
				PayPeriodSelector.Append(@"
			</select>
		</td>
	</tr>
</table>");
				viewer_home.InnerHtml = PayPeriodSelector.ToString();
				}
			else if (q_business_unit_id > 0 && q_payperiod_id > 0)
				{
				viewer_table.Visible = true;
				var business_unit = new NeBusinessUnit(q_business_unit_id);
				var sb_branch_details = new StringBuilder();
				name.InnerText = business_unit.name;
				finalapproval.InnerText = "Final approval is done by either " + business_unit.branch_manager.FullName + " or their manager, " + new NeMember(business_unit.branch_manager.reports_to).FullName;
				payperiod_of.InnerText = $"For the payperiod of: {start_date} - {end_date}";


				var dt_viewer = Toolbox.doSQL_dt(conn, @"CALL payroll_viewer_info(@v0 , @v1 )", new object[] { q_payperiod_id, business_unit.id });
				var total = new payroll.hour_packet();
				var total_dollar = new payroll.hour_packet();
				var total_sub = new payroll.hour_packet();
				var total_1099 = new payroll.hour_packet();
				var sb_viewer_body = new StringBuilder();
				var sb_viewer_foot = new StringBuilder();
				if (dt_viewer.Rows.Count > 0)
					{
					var status_html = new StringBuilder();
					status_html.AppendFormat(@"
			<button type='button' style='width:{0}%;margin-top:5px;' id='btnex'>Export to Excel</button>
			</div>", "100");
					branch_selector.InnerHtml += status_html.ToString();

					foreach (DataRow dr in dt_viewer.Rows)
						{
						var member_id = Convert.ToInt32(dr["MEMBERID"]);
						var this_member = new NeMember((int)member_id);
						var member_name_first = this_member.FirstName;
						var member_name_last = this_member.LastName;
						var canSeeWage = _can_export_payroll || NeMember.is_supervisor(member_id, current_user.id) || current_user.id == member_id;
						var hr_status = Toolbox.doSQL_string(conn, @"Select status from member_hrstatus where id =@v0", new object[] { this_member.hrstatus_id });

						var member_paytype_id = nesi.core.payroll.paytype_at_date(conn, payperiod.end_date, this_member.id);
						var member_pre_paytype = Toolbox.doSQL_string(conn, @"SELECT paytype FROM member_paytype WHERE id=@v0", new object[] { member_paytype_id }).Replace("Timesheet", "TS").Replace(" with ", " w/ ");
						var member_paytype = Toolbox.doSQL_string(conn, @"SELECT paytype FROM member_paytype WHERE id=@v0", new object[] { this_member.paytype_id }).Replace("Timesheet", "TS").Replace(" with ", " w/ ");
						var used_paytype_string = member_paytype_id != this_member.paytype_id
															? member_pre_paytype + "<br/><div style='font-size:10px;color:#f00;'>(Currently: " + member_paytype + ")</div>"
															: member_paytype;
						var per_user_dollar_total = 0.0;

						var DepartmentHeadId = Convert.ToInt32(dr["DEPTHEADID"]);
						if (DepartmentHeadId == 0)
							{
							DepartmentHeadId = this_member.business_unit.branch_manager.id;
							}
						var DepartmentHead = DepartmentHeadId != 0 ? new NeMember(DepartmentHeadId) : new NeMember();
						var DepartmentHeadFirstName = DepartmentHeadId != 0 ? DepartmentHead.Nickname : "--";
						var DepartmentHeadLastName = DepartmentHeadId != 0 ? DepartmentHead.LastName.Substring(0, 1) + "." : "--";

						var bankedPay = new payroll.banked_pay
							{
							payperiod_id = q_payperiod_id,
							member_id = member_id
							};

						var payroll = new payroll
							{
							member_id = this_member.id,
							payperiod_id = bankedPay.payperiod_id
							};
						var dashdash = "<span style='opacity:0.25;'>--</span>";
						var wage = payroll.get_wage_at_date(conn, member_id, Convert.ToDateTime(date_end));
						var hour_obj = new payroll.hour_packet
							{
							vacation_hours = payroll.vacation.this_payperiod(conn, false, this_member.id, q_payperiod_id, payperiod_complete)
							};
						var alloutstanding_chk = payroll.vacation.check_all_outstanding(conn, this_member.id, q_payperiod_id);
						var vacation_string = hour_obj.vacation_hours == 0
																	? "0"
																	: alloutstanding_chk
																		? "<div style='font-size:13px;'>ALL</div><div style='font-size:10px;'>outstanding</div>"
																		: hour_obj.vacation_hours.ToString("N2");
						hour_obj.vacation_hours = hour_obj.vacation_hours == 999999
																	? 0
																	: hour_obj.vacation_hours;
						hour_obj.vacation_dollars = payroll.vacation.this_payperiod(conn, true, this_member.id, q_payperiod_id, payperiod_complete);
						
						// MH 2020-11-04 - Commenting this out until it's completed.
						// var payoutExist = payroll.vacation.PayoutExists(this_member.id, q_payperiod_id);
						var payout_addition = !alloutstanding_chk && hour_obj.vacation_dollars > 0
							? "<div style='font-size:13px;'>" + hour_obj.vacation_dollars.ToString("N2") + "</div><div style='font-size:10px;'>Withdrawn</div>"
							: "";
						var alloutstanding_addition = alloutstanding_chk && !vacation_string.Contains("ALL")
																	? "<div style='font-size:13px;'>ALL</div><div style='font-size:10px;'>outstanding</div>"
																	: "";
						if (hour_obj.vacation_hours == 0 && !alloutstanding_chk && payout_addition == "")
							{
							vacation_string = dashdash;
							}
						else if ((hour_obj.vacation_hours >= 0 || payout_addition != "") && !alloutstanding_chk)
							{
							vacation_string = "<div style='font-size:13px;'>" + hour_obj.vacation_hours.ToString("N2") + "</div><div style='font-size:10px;margin-bottom:10px;'>Hours</div>";
							}
						if (hour_obj.vacation_hours == 999999 || hour_obj.vacation_dollars == 999999) // 999999 is used to temporarily denote all outstanding
							{
							hour_obj.vacation_hours = 0;
							hour_obj.vacation_dollars = 0;
							}
						var member_payroll_id = this_member.PayrollNo;
						var deposited_double = bankedPay.get_hours(conn, OpsPayroll.TransactionType.BankedPay.Deposited, q_payperiod_id);
						var withdrawn_double = bankedPay.get_hours(conn, OpsPayroll.TransactionType.BankedPay.Withdrawn, q_payperiod_id);
						var paidout_double = bankedPay.get_hours(conn, OpsPayroll.TransactionType.BankedPay.PaidOut, q_payperiod_id);
						var deducted_double = bankedPay.get_hours(conn, OpsPayroll.TransactionType.BankedPay.Deducted, q_payperiod_id);
						var expense_double = payroll.expense.this_payperiod(conn, member_id, q_payperiod_id);
						var perdiem_double = payroll.expense.this_per_diems(conn, member_id, q_payperiod_id);
						var commission_double = payroll.extra_payments.get(conn, member_id, OpsPayroll.TransactionType.ExtraPayment.Commission, q_payperiod_id);
						var misc_double = payroll.extra_payments.get(conn, member_id, OpsPayroll.TransactionType.ExtraPayment.Miscellaneous, q_payperiod_id);
						var bonus_double = payroll.extra_payments.get(conn, member_id, OpsPayroll.TransactionType.ExtraPayment.Bonus, q_payperiod_id);

						var unpaid_vacation_double = Toolbox.doSQL_double(conn, @"
    SELECT 
		COUNT(*)*8 h 
	FROM 
		vacation_master 
	WHERE 
		member_id = @v0 AND 
		type_id = 4 AND 
		payment_method = 1 AND 
		STATUS IN (3,5) AND 
		payperiod_id = @v1", new object[] { member_id, q_payperiod_id });

						var unpaid_sick_double = payroll.DaysOff.ThisPayPeriod(conn, member_id, q_payperiod_id, OpsPayroll.DaysOffType.SickDayUnpaid);
						var paid_sick_day = payroll.DaysOff.ThisPayPeriod(conn, member_id, q_payperiod_id, OpsPayroll.DaysOffType.SickDayPaid);
						var paid_sick_other = payroll.DaysOff.ThisPayPeriod(conn, member_id, q_payperiod_id, OpsPayroll.DaysOffType.SickDayOther);
						var paid_sick_double = paid_sick_day + paid_sick_other;
						var birthday_double = payroll.DaysOff.ThisPayPeriod(conn, member_id, q_payperiod_id, OpsPayroll.DaysOffType.Birthday);
						var bereavement_double = payroll.DaysOff.ThisPayPeriod(conn, member_id, q_payperiod_id, OpsPayroll.DaysOffType.Bereavement);
						var unpaid_dayoff_other_double = Toolbox.doSQL_double(conn, @"
	SELECT 
		IFNULL(SUM(TIMESTAMPDIFF(HOUR, date_start, date_end)),0) h 
	FROM 
		vacation_master 
	WHERE 
		member_id = @v0 AND 
		type_id NOT IN (2,4,10,11,12,13) AND 
		STATUS IN (3,5) AND 
		payperiod_id = @v1 ", new object[] { member_id, q_payperiod_id });

						var deposited_string = deposited_double == 0 ? dashdash : deposited_double.ToString("N2");
						var withdrawn_string = withdrawn_double == 0 ? dashdash : withdrawn_double.ToString("N2");
						var paidout_string = paidout_double == 0 ? dashdash : paidout_double.ToString("N2");
						var deducted_string = deducted_double == 0 ? dashdash : deducted_double.ToString("N2");
						var expense_string = expense_double == 0 ? dashdash : expense_double.ToString("C2");
						var perdiems_string = perdiem_double == 0 ? dashdash : perdiem_double.ToString("C2");
						var commission_string = commission_double == 0 ? dashdash : commission_double.ToString("C2");
						var misc_string = misc_double == 0 ? dashdash : misc_double.ToString("C2");
						var bonus_string = bonus_double == 0 ? dashdash : bonus_double.ToString("C2");


						hour_obj.RegularTime = Convert.ToDouble(dr["rt"]);
						hour_obj.OverTime = Convert.ToDouble(dr["ot"]);
						hour_obj.DoubleTime = Convert.ToDouble(dr["dt"]);
						hour_obj.RegularTimeShiftPremium = Convert.ToDouble(dr["rtsp"]);
						hour_obj.OverTimeShiftPremium = Convert.ToDouble(dr["otsp"]);
						hour_obj.DoubleTimeShiftPremium = Convert.ToDouble(dr["dtsp"]);
						var over_80_check = "";
						if (member_paytype_id != OpsMemberPayType.SalaryHourlyWithTimeSheet && member_paytype_id != OpsMemberPayType.SalaryHourlyWithoutTimeSheet) //Pay Type is neither Salary-With nor Salary-Without Timesheet
							{
							hour_obj.StatPay = member_paytype_id != OpsMemberPayType.CoOp ? Convert.ToDouble(dr["stat"]) : 0;
							}
						else if (member_paytype_id == OpsMemberPayType.SalaryHourlyWithTimeSheet || member_paytype_id == OpsMemberPayType.SalaryHourlyWithoutTimeSheet) // Salary w/ Timesheet
							{
							hour_obj.StatPay = Convert.ToDouble(dr["stat"]);
							if (hour_obj.RegularTime > 80)
								{
								hour_obj.RegularTime = 80;
								}

							if (member_paytype_id == OpsMemberPayType.SalaryHourlyWithoutTimeSheet)
								{
								var vacation_rt_chk = payroll.vacation.this_payperiod(conn, false, member_id, q_payperiod_id, payperiod_complete);
								if (hour_obj.RegularTime == 80 && vacation_rt_chk > 0)
									{
									over_80_check = "RT hours are 80 and vacation is greater than 0... this should not happen";
									}
								}
							}
						
						total.banked_deposited += deposited_double;
						total.banked_withdrawn += withdrawn_double;
						total.banked_paidout += paidout_double;
						total.banked_deducted += deducted_double;
						total.vacation_dollars += hour_obj.vacation_hours + hour_obj.vacation_dollars;
						total.Birthday += birthday_double;
						total.Bereavement += bereavement_double;
						total.UnpaidVacation += unpaid_vacation_double;
						total.UnpaidSickDay += unpaid_sick_double;
						total.PaidSickDay += paid_sick_double;
						total.UnpaidOther += unpaid_dayoff_other_double;

						if (member_paytype_id == OpsMemberPayType.Subcontract)
							{
							total_sub.StatPay += hour_obj.StatPay;
							}
						else
							{
							total.StatPay += hour_obj.StatPay;
							}
						total.Expense += expense_double;
						total.PerDiems += perdiem_double;
						total.Bonus += bonus_double;
						total.Commission += commission_double;
						total.Miscellaneous += misc_double;
						total_dollar.Bereavement += bereavement_double * wage;
						total_dollar.Birthday += birthday_double * wage;
						total_dollar.banked_deposited += deposited_double * wage;
						total_dollar.banked_withdrawn += withdrawn_double * wage;
						total_dollar.banked_paidout += paidout_double * wage;
						total_dollar.banked_deducted += deducted_double * wage;
						total_dollar.vacation_dollars += hour_obj.vacation_hours * wage + hour_obj.vacation_dollars * wage;
						total_dollar.StatPay += hour_obj.StatPay * wage;

						var holiday_string = hour_obj.StatPay == 0 ? dashdash : hour_obj.StatPay.ToString("N2");
						var rt_string = hour_obj.RegularTime == 0 ? dashdash : hour_obj.RegularTime.ToString("N2");
						var ot_string = hour_obj.OverTime == 0 ? dashdash : hour_obj.OverTime.ToString("N2");
						var dt_string = hour_obj.DoubleTime == 0 ? dashdash : hour_obj.DoubleTime.ToString("N2");
						var rtsp_string = hour_obj.RegularTimeShiftPremium == 0 ? dashdash : hour_obj.RegularTimeShiftPremium.ToString("N2");
						var otsp_string = hour_obj.OverTimeShiftPremium == 0 ? dashdash : hour_obj.OverTimeShiftPremium.ToString("N2");
						var dtsp_string = hour_obj.DoubleTimeShiftPremium == 0 ? dashdash : hour_obj.DoubleTimeShiftPremium.ToString("N2");
						var unpaid_sick_string = unpaid_sick_double == 0 ? dashdash : unpaid_sick_double.ToString("N2");
						var unpaid_dayoff_other_string = unpaid_dayoff_other_double == 0 ? dashdash : unpaid_dayoff_other_double.ToString("N2");
						var paid_sick_string = paid_sick_double == 0 ? dashdash : paid_sick_double.ToString("N2");
						var unpaid_vacation_string = unpaid_vacation_double == 0 ? dashdash : unpaid_vacation_double.ToString("N2");
						var BereavementString = bereavement_double == 0 ? dashdash : bereavement_double.ToString("N2");
						var BirthdayString = birthday_double == 0 ? dashdash : birthday_double.ToString("N2");
						var bgcolor_over80 = over_80_check != "" ? "background-color:#f00;" : "";
						var tooltip_over80 = over_80_check;



						// Per user Dollar Totals for employees that aren't SubContract, 1099 or Co-Op	
						if (!Toolbox.Contains(member_paytype_id, new[] { OpsMemberPayType.Subcontract, OpsMemberPayType.f1099, OpsMemberPayType.CoOp }))
							{
							per_user_dollar_total += hour_obj.RegularTime * wage;
							per_user_dollar_total += hour_obj.OverTime * wage * 1.5;
							per_user_dollar_total += hour_obj.DoubleTime * wage * 2;
							per_user_dollar_total += hour_obj.RegularTimeShiftPremium * wage * 1.1;
							per_user_dollar_total += hour_obj.OverTimeShiftPremium * wage * 1.65;
							per_user_dollar_total += hour_obj.DoubleTimeShiftPremium * wage * 2.2;
							}
						if (!string.IsNullOrEmpty(holiday_string) && holiday_string != dashdash)
							{
							per_user_dollar_total += Convert.ToDouble(hour_obj.StatPay * wage);
							}
						if (!string.IsNullOrEmpty(vacation_string) && vacation_string != dashdash ||
							!string.IsNullOrEmpty(alloutstanding_addition) && alloutstanding_addition != dashdash ||
							!string.IsNullOrEmpty(payout_addition) && payout_addition != dashdash)
							{
							per_user_dollar_total += Convert.ToDouble(hour_obj.vacation_hours * wage + hour_obj.vacation_dollars * wage);
							}
						if (expense_double != 0)
							{
							per_user_dollar_total += expense_double;
							}
						if (commission_double != 0)
							{
							per_user_dollar_total += commission_double;
							}
						if (misc_double != 0)
							{
							per_user_dollar_total += misc_double;
							}
						if (bonus_double != 0)
							{
							per_user_dollar_total += bonus_double;
							}
						if (withdrawn_double != 0)
							{
							per_user_dollar_total += withdrawn_double * wage;
							}
						if (paidout_double != 0)
							{
							per_user_dollar_total += paidout_double * wage;
							}
						if (deducted_double != 0)
							{
							per_user_dollar_total -= deducted_double * wage;
							}
						if (deposited_double != 0)
							{
							per_user_dollar_total -= deposited_double * wage;
							}
						if(birthday_double != 0)
							{
							per_user_dollar_total += birthday_double * wage;
							}
						if(bereavement_double != 0)
							{
							per_user_dollar_total += bereavement_double * wage;
							}

						#region Add Row to final Table
					var hourRow = $@"
				<tr class='member'>
                    <td data-coln='1'	class='paytype'>{member_payroll_id}</td>
					<td data-coln='2'	class='name' title=""{this_member.FullName} (Member ID: {this_member.id})""><a href='javascript:void(0);' onclick=""boing('/#/opens/127/employees/{this_member.id}', 'member{this_member.id}', 1124,768);"">{member_name_first}</a></td>
					<td data-coln='3'	class='name' title=""{this_member.FullName} (Member ID: {this_member.id})""><div style='white-space: nowrap; width:45px; overflow: hidden; text-overflow: clip;'><a href='javascript:void(0);' onclick=""boing('/#/opens/127/employees/{this_member.id}', 'member{this_member.id}', 1124,768);"">{member_name_last}</a></div></td>
					<td data-coln='4'	class='paytype'>{hr_status}</td>
					<td data-coln='5'	class='paytype'>{used_paytype_string}</td>
					<td data-coln='6'	class='name'>{DepartmentHeadFirstName} {DepartmentHeadLastName}</td>
					<td data-coln='7'	class='hours'>{(canSeeWage ? wage.ToString("C2") : dashdash)}</td>
					<td data-coln='8'	class='hours' title='{tooltip_over80}' style='{bgcolor_over80}'>{rt_string}</td>
					<td data-coln='9'	class='hours'>{ot_string}</td>
					<td data-coln='10'	class='hours'>{dt_string}</td>
					<td data-coln='11'	class='hours'>{rtsp_string}</td>
					<td data-coln='12'	class='hours'>{otsp_string}</td>
					<td data-coln='13'	class='hours'>{dtsp_string}</td>
					<td data-coln='14'	class='hours'>{unpaid_vacation_string}</td>
					<td data-coln='15'	class='hours'>{unpaid_sick_string}</td>
					<td data-coln='16'	class='hours'>{paid_sick_string}</td>
					<td data-coln='17'	class='hours opt1' data-title='Unpaid Other Detail' data-tooltip=""{(unpaid_dayoff_other_double > 0 ? UnpaidOtherTip(conn, member_id, q_payperiod_id) : "")}"" data-dotip='{(unpaid_dayoff_other_double > 0 ? "true" : "false")}'>{unpaid_dayoff_other_string}</td>
					<td data-coln='18'	class='hours'>{(canSeeWage ? BirthdayString : dashdash)}</td>
					<td data-coln='19'	class='hours'>{(canSeeWage ? BereavementString : dashdash)}</td>
					<td data-coln='20'	class='hours'>{(canSeeWage ? holiday_string : dashdash)}</td>
					<td data-coln='21'	class='hours'>{(canSeeWage ? vacation_string : dashdash)}{alloutstanding_addition}{payout_addition}</td>
					<td data-coln='22'	class='hours'>{(canSeeWage ? expense_string : dashdash)}</td>
                    <td data-coln='23'	class='hours'>{(canSeeWage ? perdiems_string : dashdash)}</td>
					<td data-coln='24'	class='hours'>{(canSeeWage ? commission_string : dashdash)}</td>
					<td data-coln='25'	class='hours'>{(canSeeWage ? misc_string : dashdash)}</td>
					<td data-coln='26'	class='hours'>{(canSeeWage ? bonus_string : dashdash)}</td>
					<td data-coln='27'	class='hours'>{(canSeeWage ? deposited_string : dashdash)}</td>
					<td data-coln='28'	class='hours'>{(canSeeWage ? withdrawn_string : dashdash)}</td>
					<td data-coln='29'	class='hours'>{(canSeeWage ? paidout_string : dashdash)}</td>
					<td data-coln='30'	class='hours'>{(canSeeWage ? deducted_string : dashdash)}</td>
                    <td data-coln='31'	class='hours'>{(canSeeWage ? per_user_dollar_total.ToString("C2") : dashdash)}</td>
				</tr>";
						sb_viewer_body.Append(hourRow);
						#endregion

						//Hour Totals for SubContract, 1099, Co-Op
						if (member_paytype_id == OpsMemberPayType.Subcontract)
							{
							total_sub.RegularTime += hour_obj.RegularTime;
							total_sub.OverTime += hour_obj.OverTime;
							total_sub.DoubleTime += hour_obj.DoubleTime;
							total_sub.RegularTimeShiftPremium += hour_obj.RegularTimeShiftPremium;
							total_sub.OverTimeShiftPremium += hour_obj.OverTimeShiftPremium;
							total_sub.DoubleTimeShiftPremium += hour_obj.DoubleTimeShiftPremium;
							}
						else if (member_paytype_id == OpsMemberPayType.f1099)
							{
							total_1099.RegularTime += hour_obj.RegularTime * wage;
							total_1099.OverTime += hour_obj.OverTime * wage * 1.5;
							total_1099.DoubleTime += hour_obj.DoubleTime * wage * 2;
							total_1099.RegularTimeShiftPremium += hour_obj.RegularTimeShiftPremium * wage * 1.1;
							total_1099.OverTimeShiftPremium += hour_obj.OverTimeShiftPremium * wage * 1.65;
							total_1099.DoubleTimeShiftPremium += hour_obj.DoubleTimeShiftPremium * wage * 2.2;
							}
						else if (member_paytype_id != OpsMemberPayType.CoOp)
							{
							total.RegularTime += hour_obj.RegularTime;
							total.OverTime += hour_obj.OverTime;
							total.DoubleTime += hour_obj.DoubleTime;
							total.RegularTimeShiftPremium += hour_obj.RegularTimeShiftPremium;
							total.OverTimeShiftPremium += hour_obj.OverTimeShiftPremium;
							total.DoubleTimeShiftPremium += hour_obj.DoubleTimeShiftPremium;
							}
						// Dollar Totals for employees that aren't SubContract, 1099 or Co-Op	
						if (!Toolbox.Contains(member_paytype_id, new[] {    OpsMemberPayType.Subcontract,
																			OpsMemberPayType.f1099,
																			OpsMemberPayType.CoOp
																			}))
							{
							total_dollar.RegularTime += hour_obj.RegularTime * wage;
							total_dollar.OverTime += hour_obj.OverTime * wage * 1.5;
							total_dollar.DoubleTime += hour_obj.DoubleTime * wage * 2;
							total_dollar.RegularTimeShiftPremium += hour_obj.RegularTimeShiftPremium * wage * 1.1;
							total_dollar.OverTimeShiftPremium += hour_obj.OverTimeShiftPremium * wage * 1.65;
							total_dollar.DoubleTimeShiftPremium += hour_obj.DoubleTimeShiftPremium * wage * 2.2;
							}
						}
					}
				viewer_body.InnerHtml = sb_viewer_body.ToString();
				var grand_total = total_dollar.RegularTime +
																total_dollar.RegularTimeShiftPremium +
																total_dollar.OverTime +
																total_dollar.OverTimeShiftPremium +
																total_dollar.DoubleTime +
																total_dollar.DoubleTimeShiftPremium +
																total_dollar.StatPay +
																total_dollar.vacation_dollars +
																total.Expense +
																total.PerDiems +
																total.Commission +
																total.Miscellaneous +
																total.Bonus +
																total.Bereavement+
																total.Birthday+
																total_dollar.banked_withdrawn +
																total_dollar.banked_paidout -
																total_dollar.banked_deposited;

				if (_can_export_payroll)
					{
					if (total_sub.RegularTime > 0 ||
						total_sub.OverTime > 0 ||
						total_sub.DoubleTime > 0 ||
						total_sub.RegularTimeShiftPremium > 0 ||
						total_sub.OverTimeShiftPremium > 0 ||
						total_sub.DoubleTimeShiftPremium > 0)
						{
						var rowSubcontractTotal = $@"
			<tr id='totals' class='totals'>
				<td data-coln='1-7' class='total_text' colspan='7'>SUBCONTRACT TOTALS - </td>
				<td data-coln='8' class='total_hours'>{blankcheck(total_sub.RegularTime)}</td>
				<td data-coln='9' class='total_hours'>{blankcheck(total_sub.OverTime)}</td>
				<td data-coln='10' class='total_hours'>{blankcheck(total_sub.DoubleTime)}</td>
				<td data-coln='11' class='total_hours'>{blankcheck(total_sub.RegularTimeShiftPremium)}</td>
				<td data-coln='12' class='total_hours'>{blankcheck(total_sub.OverTimeShiftPremium)}</td>
				<td data-coln='13' class='total_hours'>{blankcheck(total_sub.DoubleTimeShiftPremium)}</td>
				<td data-coln='14' class='total_hours'>&nbsp;</td>
				<td data-coln='15' class='total_hours'>&nbsp;</td>
				<td data-coln='16' class='total_hours'>&nbsp;</td>
				<td data-coln='17' class='total_hours'>&nbsp;</td>
				<td data-coln='18' class='total_hours'>&nbsp;</td>
				<td data-coln='19' class='total_hours'>&nbsp;</td>
				<td data-coln='20' class='total_hours'>{blankcheck(total_sub.StatPay)}</td>
				<td data-coln='21' class='total_hours'>&nbsp;</td>
				<td data-coln='22' class='total_hours'>&nbsp;</td>
				<td data-coln='23' class='total_hours'>&nbsp;</td>
				<td data-coln='24' class='total_hours'>&nbsp;</td>
				<td data-coln='25' class='total_hours'>&nbsp;</td>
				<td data-coln='26' class='total_hours'>&nbsp;</td>
				<td data-coln='27' class='total_hours'>&nbsp;</td>
				<td data-coln='28' class='total_hours'>&nbsp;</td>
				<td data-coln='29' class='total_hours'>&nbsp;</td>
                <td data-coln='30' class='total_hours'>&nbsp;</td>
                <td data-coln='31' class='total_hours'>&nbsp;</td>
			</tr>";
						sb_viewer_foot.Append(rowSubcontractTotal);
						}
					}

				if (_can_export_payroll)
					{
					if (total_1099.RegularTime > 0 ||
						total_1099.OverTime > 0 ||
						total_1099.DoubleTime > 0 ||
						total_1099.RegularTimeShiftPremium > 0 ||
						total_1099.OverTimeShiftPremium > 0 ||
						total_1099.DoubleTimeShiftPremium > 0)
						{
						
						var row1099Total = $@"
			<tr id='totals' class='totals'>
				<td data-coln='1-7' class='total_text' colspan='7'>1099 TOTALS - </td>
				<td data-coln='8' class='total_hours'>{blankcheck(total_1099.RegularTime)}</td>
				<td data-coln='9' class='total_hours'>{blankcheck(total_1099.OverTime)}</td>
				<td data-coln='10' class='total_hours'>{blankcheck(total_1099.DoubleTime)}</td>
				<td data-coln='11' class='total_hours'>{blankcheck(total_1099.RegularTimeShiftPremium)}</td>
				<td data-coln='12' class='total_hours'>{blankcheck(total_1099.OverTimeShiftPremium)}</td>
				<td data-coln='13' class='total_hours'>{blankcheck(total_1099.DoubleTimeShiftPremium)}</td>
				<td data-coln='14' class='total_hours'>&nbsp;</td>
				<td data-coln='15' class='total_hours'>&nbsp;</td>
				<td data-coln='16' class='total_hours'>&nbsp;</td>
				<td data-coln='17' class='total_hours'>&nbsp;</td>
				<td data-coln='18' class='total_hours'>&nbsp;</td>
				<td data-coln='19' class='total_hours'>&nbsp;</td>
				<td data-coln='20' class='total_hours'>&nbsp;</td>
				<td data-coln='21' class='total_hours'>&nbsp;</td>
				<td data-coln='22' class='total_hours'>&nbsp;</td>
				<td data-coln='23' class='total_hours'>&nbsp;</td>
				<td data-coln='24' class='total_hours'>&nbsp;</td>
				<td data-coln='25' class='total_hours'>&nbsp;</td>
				<td data-coln='26' class='total_hours'>&nbsp;</td>
				<td data-coln='27' class='total_hours'>&nbsp;</td>
				<td data-coln='28' class='total_hours'>&nbsp;</td>
				<td data-coln='29' class='total_hours'>&nbsp;</td>
				<td data-coln='24' class='total_hours'>&nbsp;</td>
                <td data-coln='30' class='total_hours'>&nbsp;</td>
                <td data-coln='31' class='total_hours'>&nbsp;</td>
			</tr>";      
						sb_viewer_foot.Append(row1099Total);
						}
					}
				if (_can_export_payroll)
					{
					var rowTotal = $@"
			<tr id='totals' class='totals'>
				<td data-coln='1-7' class='total_text' colspan='7'>HOUR TOTALS - </td>
				<td data-coln='8' class='total_hours'>{blankcheck(total.RegularTime)}</td>
				<td data-coln='9' class='total_hours'>{blankcheck(total.OverTime)}</td>
				<td data-coln='10' class='total_hours'>{blankcheck(total.DoubleTime)}</td>
				<td data-coln='11' class='total_hours'>{blankcheck(total.RegularTimeShiftPremium)}</td>
				<td data-coln='12' class='total_hours'>{blankcheck(total.OverTimeShiftPremium)}</td>
				<td data-coln='13' class='total_hours'>{blankcheck(total.DoubleTimeShiftPremium)}</td>
				<td data-coln='14' class='total_hours'>{blankcheck(total.UnpaidVacation)}</td>
				<td data-coln='15' class='total_hours'>{blankcheck(total.UnpaidSickDay)}</td>
				<td data-coln='16' class='total_hours'>{blankcheck(total.PaidSickDay)}</td>
				<td data-coln='17' class='total_hours'>{blankcheck(total.UnpaidOther)}</td>
				<td data-coln='18' class='total_hours'>{blankcheck(total.Birthday)}</td>
				<td data-coln='19' class='total_hours'>{blankcheck(total.Bereavement)}</td>
				<td data-coln='20' class='total_hours'>{blankcheck(total.StatPay)}</td>
				<td data-coln='21' class='total_hours'>{blankcheck(total.vacation_dollars)}</td>
                <td data-coln='22' class='total_hours'>&nbsp;</td>
				<td data-coln='23' class='total_hours'>&nbsp;</td>
				<td data-coln='24' class='total_hours'>&nbsp;</td>
				<td data-coln='25' class='total_hours'>&nbsp;</td>
				<td data-coln='26' class='total_hours'>&nbsp;</td>
				<td data-coln='27' class='total_hours'>{blankcheck(total.banked_deposited)}</td>
				<td data-coln='28' class='total_hours'>{blankcheck(total.banked_withdrawn)}</td>
				<td data-coln='29' class='total_hours'>{blankcheck(total.banked_paidout)}</td>
				<td data-coln='30' class='total_hours'>{blankcheck(total.banked_deducted)}</td>
                <td data-coln='31' class='total_hours'>&nbsp;</td>
			</tr>";
					sb_viewer_foot.Append(rowTotal);
					}


				if (_can_export_payroll)
					{
					var dollarTotalRow = $@"
			<tr id='totals' class='totals'>
				<td data-coln='1-7' class='total_text' colspan='7'>DOLLAR TOTALS - </td>
				<td data-coln='8' class='total_hours'>{dollarize(total_dollar.RegularTime)}</td>
				<td data-coln='9' class='total_hours'>{dollarize(total_dollar.OverTime)}</td>
				<td data-coln='10' class='total_hours'>{dollarize(total_dollar.DoubleTime)}</td>
				<td data-coln='11' class='total_hours'>{dollarize(total_dollar.RegularTimeShiftPremium)}</td>
				<td data-coln='12' class='total_hours'>{dollarize(total_dollar.OverTimeShiftPremium)}</td>
				<td data-coln='13' class='total_hours'>{dollarize(total_dollar.DoubleTimeShiftPremium)}</td>
				<td data-coln='14' class='total_hours'>&nbsp;</td>
				<td data-coln='15' class='total_hours'>&nbsp;</td>
				<td data-coln='16' class='total_hours'>{dollarize(total_dollar.PaidSickDay)}</td>
				<td data-coln='17' class='total_hours'>&nbsp;</td>
				<td data-coln='18' class='total_hours'>{dollarize(total_dollar.Birthday)}</td>
				<td data-coln='19' class='total_hours'>{dollarize(total_dollar.Bereavement)}</td>
				<td data-coln='20' class='total_hours'>{dollarize(total_dollar.StatPay)}</td>
				<td data-coln='21' class='total_hours'>{dollarize(total_dollar.vacation_dollars)}</td>
				<td data-coln='22' class='total_hours'>{dollarize(total.Expense)}</td>
                <td data-coln='23' class='total_hours'>{dollarize(total.PerDiems)}</td>
				<td data-coln='24' class='total_hours'>{dollarize(total.Commission)}</td>
				<td data-coln='25' class='total_hours'>{dollarize(total.Miscellaneous)}</td>
				<td data-coln='26' class='total_hours'>{dollarize(total.Bonus)}</td>
				<td data-coln='27' class='total_hours'>{dollarize(total_dollar.banked_deposited)}</td>
				<td data-coln='28' class='total_hours'>{dollarize(total_dollar.banked_withdrawn)}</td>
				<td data-coln='29' class='total_hours'>{dollarize(total_dollar.banked_paidout)}</td>
	 			<td data-coln='30' class='total_hours'>{dollarize(total_dollar.banked_deducted)}</td>
				<td data-coln='31' class='total_hours'>&nbsp;</td>
			</tr>";
					sb_viewer_foot.Append(dollarTotalRow);
					}

				if (_can_export_payroll)
					{
					sb_viewer_foot.AppendFormat(@"
			<tr>
				<td class='grand_total' colspan='31'>GRAND TOTAL: ${0}<div class='equation'>(RT + OT + DT + RTsp + OTsp + DTsp + HOL + VAC + EXP + COMM + BON + REF + BANK WITH) - BANK DEP</div></td>
			</tr>", grand_total.ToString("N2"));
					}
				viewer_foot.InnerHtml = sb_viewer_foot.ToString();
				var sb_outstanding = new StringBuilder();
				var outstanding = Toolbox.doSQL_dt(conn, $@"
SELECT
	   b.member_fullname name,
	   CAST(SUM(numberofhours) AS CHAR) hours,
	   c.member_fullname handler
FROM
	   membertime a
LEFT JOIN
	   member b ON a.membertime_memberid = b.member_id
LEFT JOIN
		member c ON b.payroll_handler = c.member_id
WHERE
	   a.date > '{pay_period.StartDate}' AND
	   a.date < '{pay_period.Enddate}' AND
	   b.business_unit_id = '{business_unit.id}' AND
	   a.membertime_memberid IN (select c.member_id from member c where c.business_unit_id = '{business_unit.id}' ) AND
	   a.membertime_memberid NOT IN (select d.member_id from payroll_hours d where d.payperiod_id = '{q_payperiod_id}') AND
	   b.paytype_id IN ({OpsMemberPayType.Hourly},
						{OpsMemberPayType.SalaryHourlyWithTimeSheet},
						{OpsMemberPayType.Subcontract},
						{OpsMemberPayType.f1099}
						)
GROUP BY 
	b.member_id
UNION
SELECT 
	a.member_fullname name,
	'Salary' hours, 
	b.member_fullname handler
FROM 
	member a 
LEFT JOIN
	member b ON a.payroll_handler = b.member_id
WHERE 
	a.paytype_id = {OpsMemberPayType.SalaryHourlyWithoutTimeSheet} AND
	a.member_startdate <= '{pay_period.Enddate}' AND
	a.member_termdate >= '{pay_period.Enddate}' AND 
	a.business_unit_id = {business_unit.id} AND
	a.member_id NOT IN (SELECT d.member_id FROM payroll_hours d WHERE d.payperiod_id = '{q_payperiod_id}')
	", null);
				if (outstanding.Rows.Count > 0)
					{
					var temp_table = "<b>Employees left to be approved</br>(with timesheet entries)</b><br/><table width='400' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;border:solid 1px #000;background-color:#fff;'><thead style='background-color:#000;color:#fff;'><tr><th>Employee</th><th>Hours</th><th>Handler</th></thead><tbody>";
					foreach (DataRow dr in outstanding.Rows)
						{
						var _name = dr["name"].ToString();
						var hours = dr["hours"].ToString();
						var handler = dr["handler"].ToString();
						temp_table += "<tr><td>" + _name + "</td><td>" + hours + "</td><td>" + handler + "</td></tr>";
						}
					temp_table += "</tbody></table><br/><br/>";
					sb_outstanding.Append(temp_table);
					}
				var dt_expenses = Toolbox.doSQL_dt(conn, @"SELECT b.member_fullname name FROM expense_reimbursement a LEFT JOIN member b ON a.id_member = b.member_id WHERE a.approved = -1 AND a.id_payperiod = @v1  AND b.business_unit_id = @v0 ", new object[] { business_unit.id, q_payperiod_id });
				if (dt_expenses.Rows.Count > 0)
					{
					foreach (DataRow dr_exp in dt_expenses.Rows)
						{
						sb_outstanding.AppendFormat(@"<div style='font-weight:bold;margin:2px;'>Expense not dealt with: {0}</div>", dr_exp["name"]);
						}
					}
				var dt_extra = Toolbox.doSQL_dt(conn, @"SELECT a.type, b.member_fullname name FROM payroll_extra_payments a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.approved = -1 AND a.payperiod_id = @v1  AND b.business_unit_id = @v0 ", new object[] { business_unit.id, q_payperiod_id });
				if (dt_extra.Rows.Count > 0)
					{
					foreach (DataRow dr_extra in dt_extra.Rows)
						{
						var extra_type = dr_extra["type"].ToString();
						var this_type = extra_type == "B" ? "Bonus" : extra_type == "C" ? "Commission" : "Misc. Payment";
						sb_outstanding.AppendFormat(@"<div style='font-weight:bold;margin:2px;'>{1} not dealt with: {0}</div>", dr_extra["name"], this_type);
						}
					}
				var dt_vacations = Toolbox.doSQL_dt(conn, @"SELECT MEMBER_NAME(member_id) name, COUNT(*) how_many FROM vacation_master WHERE member_id in (SELECT member_id FROM member WHERE business_unit_id = @v0) AND payperiod_id = @v1 AND status = 1 GROUP BY member_id", new object[] { business_unit.id, q_payperiod_id });
				if (dt_vacations.Rows.Count > 0)
					{
					foreach (DataRow dr_vac in dt_vacations.Rows)
						{
						sb_outstanding.AppendFormat(@"<div style='font-weight:bold;margin:2px;'>Vacation(s) not dealt with: {0} - {1} request(s)</div>", dr_vac["name"], dr_vac["how_many"]);
						}
					}
				var dt_missingadp = Toolbox.doSQL_dt(conn, $"SELECT member_id id, member_fullname name FROM member WHERE business_unit_id = @v0 AND (TRIM(member_payroll_id) = '' || member_payroll_id = '0') AND member_startdate < @v1 AND member_status = 'Active' AND paytype_id NOT IN ({OpsMemberPayType.Subcontract},{OpsMemberPayType.f1099},{OpsMemberPayType.Owner})", new object[] { business_unit.id, payperiod.Enddate });
				if (dt_missingadp.Rows.Count > 0)
					{
					foreach (DataRow dr_missingadp in dt_missingadp.Rows)
						{
						sb_outstanding.AppendFormat(@"<div style='font-weight:bold;margin:2px;'>Missing ADP ID: <a href='javascript:void(0);' onclick=""boing('/#/opens/127/employees/{1}', 'member{1}', 1024,768);"">{0}</a></div>", dr_missingadp["name"], dr_missingadp["id"]);
						}
					}
				div_outstanding.InnerHtml = sb_outstanding.ToString();
				}
			}
		}
	public string UnpaidOtherTip(MySqlConnection conn, int _memberId, int _payperiodId)
		{
		var unpaid_dt = Toolbox.doSQL_dt(conn, @"
	SELECT 
        b.type,
		IFNULL(SUM(TIMESTAMPDIFF(HOUR, a.date_start, a.date_end)),0) h
	FROM 
		vacation_master a
    LEFT JOIN
        vacation_type b ON a.type_id = b.id
	WHERE 
		a.member_id = @v0 AND 
		a.type_id NOT IN (2,4,10) AND 
		a.STATUS IN (3,5) AND 
		a.payperiod_id = @v1
    GROUP BY
        a.type_id", new object[] { _memberId, _payperiodId });
		string tip = "";
		foreach (DataRow dr in unpaid_dt.Rows)
			{
			var DOType = dr["type"].ToString();
			var DOTotal = Convert.ToDouble(dr["h"]);
			tip += $"<div>{DOType}: {DOTotal} hours</div>";
			}
		return tip;
		}
	private static string dollarize(double _figure)
		{
		return _figure > 0 ? _figure.ToString("C2") : "&nbsp;";
		}
	private static string blankcheck(double _figure)
		{
		return _figure > 0 ? _figure.ToString("N2") : "&nbsp;";
		}
	[WebMethod]
	public static string toggle_sent(int _pp_id, int _c_id, bool _is_sent)
		{
		using (var conn = Toolbox.connect())
			{
			try
				{
				var v = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM vacation_master WHERE member_id in 
(SELECT member_id FROM member WHERE business_unit_id = @v0) AND payperiod_id = @v1 AND status = 1", new object[] { _c_id, _pp_id });
				var ep = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM payroll_extra_payments
WHERE member_id in (SELECT member_id FROM member WHERE business_unit_id = @v0) AND payperiod_id = @v1 AND approved = -1", new object[] { _c_id, _pp_id });
				var e = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM expense_reimbursement A  LEFT JOIN expense_seller B ON a.id_seller = b.id_seller 
WHERE a.id_member IN (SELECT member_id FROM member WHERE business_unit_id = @v0) AND a.id_payperiod = @v1 AND a.approved = -1", new object[] { _c_id, _pp_id });
				var p = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM payroll
WHERE payperiod_id = @v0 AND business_unit_id = @v1", new object[] { _pp_id, _c_id });
				if (p == 0)
					{
					Toolbox.doSQL_void(conn, @"INSERT INTO payroll (payperiod_id, business_unit_id, member_id, sent, sent_date, current_member) 
VALUES (@v0, @v1, 1, 0, NULL, 9999999)", new object[] { _pp_id, _c_id });
					}
				if (v == 0 && ep == 0 && e == 0 || !_is_sent)
					{
					Toolbox.doSQL_void(conn, @"UPDATE payroll SET sent = @v0 WHERE payperiod_id = @v1 AND business_unit_id = @v2", new object[] { _is_sent, _pp_id, _c_id });
					Toolbox.doSQL_void(conn, @"UPDATE payroll_progress SET sent = @v0 WHERE payperiod_id = @v1 AND business_unit_id = @v2", new object[] { _is_sent, _pp_id, _c_id });
					return "SUCCESS";
					}
				var this_types = "";
				var c = 0;
				if (v > 0)
					{
					this_types += "Vacation(s),";
					c++;
					}
				if (e > 0)
					{
					this_types += "Expense(s),";
					c++;
					}
				if (ep > 0)
					{
					this_types += "Commission(s)/Bonus(es),";
					c++;
					}
				this_types = this_types.TrimEnd(',');
				return "There are " + this_types + " that need addressing before this branches payroll can be toggled";
				}
			catch (Exception ee)
				{
				return ee.ToString();
				}
			}
		}

	protected void gv_summary_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		}

	protected void gv_summary_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
				if (gv.Columns[i] is GridViewDataColumn col)
					{
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
