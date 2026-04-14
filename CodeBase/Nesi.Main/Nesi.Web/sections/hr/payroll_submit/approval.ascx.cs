using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;
using NESI.Common.Models;

public partial class sections_hr_payroll_submit_approval : System.Web.UI.UserControl
	{
	public payroll.approval_packet packet {get; set;}
	public payroll payroll {get; set;}
	public NePayPeriod payperiod { get; set;}
	private bool can_see_wage {get;set;}
	double total_bonus				= 0;
	double total_commission			= 0;
	double total_misc				= 0;
	double total_holiday			= 0;
	double total_vacation			= 0;
	double total_vacation_unpaid	= 0;
	bool unpaidHasBeenOverrode		= false;
	double TotalPaidTimeOff			= 0.0D;
	double total_vacation_payout	= 0;
	double total_banked_w			= 0;
	double total_banked_d			= 0;
	double total_banked_p			= 0;
	double total_banked_w_hours		= 0;
	double total_banked_d_hours		= 0;
	double total_banked_p_hours		= 0;
	double total_expenses			= 0;
	double total_perdiems			= 0;
	bool allOutStandingVacation		= false;
	DataTable dt;
	
	protected void Page_Load(object sender, EventArgs e)
		{
		CheckVisible();
		}
	public void load()
		{
		payperiod					= new NePayPeriod(packet.payperiod_id);
		payroll						= new payroll { start_date = payperiod.StartDate, end_date = payperiod.Enddate, member_id = packet.current_employee.id32};
		can_see_wage				= packet.handler.AuthenticatedForPrivilege(93);
		if(!can_see_wage)
			{
			div_wage_priv.Visible	= true;
			}
		using(var conn = Toolbox.connect())
			{
			var paytype					= new payroll.paytype(conn, packet.current_employee.paytype_id);
			var hr_status				= new payroll.hr_status(conn, packet.current_employee.hrstatus_id);
			if(packet.current_employee != null && packet.your_to_approve.Count > 0)
				{
				packet.wage				= payroll.get_wage_at_date(conn, packet.current_employee.id, payperiod.end_date);
				var n_employees			= packet.your_to_approve.Count;
				var handler				= packet.current_employee.payroll_handler == 0 
											? new NeMember { FullName = "Not set" } 
											: new NeMember(Convert.ToInt32(packet.current_employee.payroll_handler));
				var str_employees		= n_employees > 1 
											? n_employees+" employee(s)" 
											: "This is the last employee ";
				lb_name.Text			= string.Format("{0} <i style='font-weight:normal;font-size:12px;'>({1} left in this branch)</i>", packet.current_employee.FullName, str_employees);
				lb_paytype.Text			= @"<b style='display:inline-block;width:100px;'>Pay Type: </b>"+paytype.name;
				lb_membertype.Text		= @"<b style='display:inline-block;width:100px;'>Member Type: </b>"+packet.current_employee.membertype.name;
				lb_hrstatus.Text		= @"<b style='display:inline-block;width:100px;'>HR Status: </b>"+hr_status.name;
				lb_handler.Text			= @"<b style='display:inline-block;width:100px;'>Payroll Handler: </b>"+handler.FullName;
				dt						= Toolbox.doSQL_dt(conn,@"CALL PAYROLLHOURS(@v0 ,@v1 ,@v2 , @v3 )", new object[] {  payroll.start_date, payroll.end_date, packet.business_unit_id, packet.current_employee.id } );
				if(!can_see_wage)
					{
					foreach(DataRow dr in dt.Rows)
						{
						dr["total"]					= 0;
						}
					}
				gv_hours.DataSource											= dt;
				do_databind();
				lb_utilization.Text											= @"<b style='display:inline-block;width:100px;'>Utilization: </b>"+get_utilization_for_payperiod().ToString("P2");
				gv_hours.GroupBy(gv_hours.Columns["Week Of"]);
				gv_hours.ExpandAll();
				gv_hours.ClientVisible					= true;
				var emp_prev							= new NeMember(payroll.get_user_at(packet.your_to_approve, packet.payperiod_id, packet.business_unit_id, packet.handler.id32, packet.current_employee.id32, 1));
				var emp_next							= new NeMember(payroll.get_user_at(packet.your_to_approve, packet.payperiod_id, packet.business_unit_id, packet.handler.id32, packet.current_employee.id32, 2));
				if(emp_prev.id == packet.current_employee.id)// || emp_prev.id == emp_next.id && emp_prev.id > packet.current_employee.id)
					{
					bt_prev.ClientVisible									= false;
					}
				else
					{
					bt_prev.ClientVisible									= true;
					var prev_name											= emp_prev.FullName.Length > 12 ? emp_prev.FullName.Substring(0,12)+"..." : emp_prev.FullName;
					bt_prev.ClientSideEvents.Click							= string.Format("function(s,e){{cbp_approval.PerformCallback('prev|{0}');}}", packet.current_employee.id);
					bt_prev.Text											= @"prev <div style='font-size:10px;max-width:100px;width:100px;'>"+prev_name+"</div>";
					bt_prev.ToolTip											= "(prev) "+emp_prev.FullName;
					}

				if(emp_next.id == packet.current_employee.id)// || emp_prev.id == emp_next.id && emp_next.id < packet.current_employee.id)
					{
					bt_next.ClientVisible									= false;
					}
				else
					{
					bt_next.ClientVisible									= true;
					var next_name											= emp_next.FullName.Length > 12 ? emp_next.FullName.Substring(0,12)+"..." : emp_next.FullName;
					bt_next.ClientSideEvents.Click							= string.Format("function(s,e){{cbp_approval.PerformCallback('next|{0}');}}", packet.current_employee.id);
					bt_next.Text											= @"next <div style='font-size:10px;max-width:100px;width:100px;'>"+next_name+"</div>";
					bt_next.ToolTip											= "(next) "+emp_next.FullName;
					}
				bind_lb_members();
				bt_submit.ClientSideEvents.Click							= string.Format("function(s,e){{ approval.send_hours(s,e,{0}); }}", packet.current_employee.id);
				total_bonus			                                        = payroll.extra_payments.get(conn, packet.current_employee.id, OpsPayroll.TransactionType.ExtraPayment.Bonus, packet.payperiod_id);
				total_commission	                                        = payroll.extra_payments.get(conn, packet.current_employee.id, OpsPayroll.TransactionType.ExtraPayment.Commission, packet.payperiod_id);
				total_misc			                                        = payroll.extra_payments.get(conn, packet.current_employee.id, OpsPayroll.TransactionType.ExtraPayment.Miscellaneous, packet.payperiod_id);
				total_holiday												= Convert.ToDouble(get_summary_text(false, false, "holiday_base"));
				total_vacation												= payroll.vacation.this_payperiod(conn, false, packet.current_employee.id, packet.payperiod_id, false, true);
				var totalVacationToBePaidOut								= payroll.vacation.this_payperiod(conn, false, packet.current_employee.id, packet.payperiod_id);
				allOutStandingVacation										= payroll.vacation.check_all_outstanding(conn, packet.current_employee.id, packet.payperiod_id);
				total_vacation_unpaid										= payroll.vacation.unpaid_this_payperiod(packet.current_employee.id, packet.payperiod_id);
				if(totalVacationToBePaidOut != total_vacation)
					{
					var vacationRemainder	= total_vacation - totalVacationToBePaidOut;
					total_vacation_unpaid += vacationRemainder;
					unpaidHasBeenOverrode = true;
					}
				total_vacation_payout										= can_see_wage ? payroll.vacation.this_payperiod(conn, true, packet.current_employee.id, packet.payperiod_id) : 0;
				total_expenses												= can_see_wage ? payroll.expense.this_payperiod(conn, packet.current_employee.id, packet.payperiod_id) : 0;
				total_perdiems												= can_see_wage ? payroll.expense.this_per_diems(conn, packet.current_employee.id, packet.payperiod_id) : 0;
				var bank													= new payroll.banked_pay
																				{
																				payperiod_id = packet.payperiod_id,
																				member_id = packet.current_employee.id
																				};
				total_banked_w		                                        = can_see_wage ? bank.get_dollars(conn, OpsPayroll.TransactionType.BankedPay.Withdrawn, packet.payperiod_id) : 0;
				total_banked_d		                                        = can_see_wage ? bank.get_dollars(conn, OpsPayroll.TransactionType.BankedPay.Deposited, packet.payperiod_id) : 0;
				total_banked_p		                                        = can_see_wage ? bank.get_dollars(conn, OpsPayroll.TransactionType.BankedPay.PaidOut, packet.payperiod_id) : 0;
				total_banked_w_hours                                        = bank.get_hours(conn, OpsPayroll.TransactionType.BankedPay.Withdrawn, packet.payperiod_id);
				total_banked_d_hours                                        = bank.get_hours(conn, OpsPayroll.TransactionType.BankedPay.Deposited, packet.payperiod_id);
				total_banked_p_hours                                        = bank.get_hours(conn, OpsPayroll.TransactionType.BankedPay.PaidOut, packet.payperiod_id);
				TotalPaidTimeOff											= payroll.DaysOff.ThisPayPeriod(conn, packet.current_employee.id, packet.payperiod_id, true);
				fill_footer_values();
				}
			else
				{
				clear();//pc.SetTabEnabled(0, false);
				cbp_approval.JSProperties["cpDoJS"]			= "pc.SetActiveTabIndex(1);bt_finalsubmit.SetVisible(true);";
				}
			}
		}
	private void clear()
		{
		lb_name.Text												= "";
		lb_paytype.Text												= "";
		lb_membertype.Text											= "";
		lb_hrstatus.Text											= "";
		gv_hours.ClientVisible										= false;

		}
	private void CheckVisible()
		{
		var hasEmployees	= packet.your_to_approve.Any();
		lb_members.ClientVisible = hasEmployees;
		gv_hours.ClientVisible = hasEmployees;
		lb_handler.ClientVisible = hasEmployees;
		lb_hrstatus.ClientVisible = hasEmployees;
		lb_membertype.ClientVisible = hasEmployees;
		lb_paytype.ClientVisible = hasEmployees;
		lb_utilization.ClientVisible = hasEmployees;
		lb_name.ClientVisible = hasEmployees;
		bt_submit.ClientVisible = hasEmployees;
		div_no_employees.Visible = !hasEmployees;
		bt_next.ClientVisible = hasEmployees;
		bt_prev.ClientVisible = hasEmployees;
		}
	private void bind_lb_members()
		{
		lb_members.DataSource				= Toolbox.doSQL_dt(string.Format(@"
SELECT 
	a.member_id id,
	IF(a.member_id = @v0 , CONCAT('>> ',a.member_fullname), a.member_fullname) name, 
	IF(b.id IS null, 0, 1) approved 
FROM 
	member a 
LEFT JOIN 
	payroll_hours b 
		ON a.member_id = b.member_id AND b.payperiod_id = @v1 
WHERE 
	a.member_id IN ({0}) 
ORDER BY 
	a.member_fullname", string.Join(",", packet.your_to_approve)),
new object[] { packet.current_employee.id, packet.payperiod_id } );

		lb_members.DataBind();
		lb_members.SelectedIndex			= -1;
		}
	private void fill_footer_values()
		{
		var col_date				= gv_hours.Columns["date"];
		var col_customer_name		= gv_hours.Columns["customer_name"];
		var col_type				= gv_hours.Columns["type"];
		var col_unpaidTimeOff		= gv_hours.Columns["unpaid_time_off"];
		var col_paidTimeOff			= gv_hours.Columns["paid_time_off"];
		var col_rt					= gv_hours.Columns["rt"];
		var col_ot					= gv_hours.Columns["ot"];
		var col_dt					= gv_hours.Columns["dt"];
		var col_rtsp				= gv_hours.Columns["rtsp"];
		var col_otsp				= gv_hours.Columns["otsp"];
		var col_dtsp				= gv_hours.Columns["dtsp"];
		var col_total				= gv_hours.Columns["total"];
		/* rows
		 
		lb_hour_total
		lb_vacation_total
		lb_approval_total
		lb_verify_hours
		lb_bonuses_total
		lb_commissions_total
		lb_bankedpay_w
		lb_bankedpay_d
		lb_finalpayout
		 */
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_bonuses_total")).Text	                         = get_summary_text(false, false, "bonuses");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_commissions_total")).Text	                     = get_summary_text(false, false, "commissions");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_verify_total")).Text						         = can_see_wage ? get_summary_text(true, true, "total") : "$0.00";
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_expenses_total")).Text						     = get_summary_text(false, false, "expenses");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_misc_total")).Text						         = get_summary_text(false, false, "misc");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_bankedpay_w")).Text	                             = get_summary_text(false, false, "bankedpay_w");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_bankedpay_d")).Text	                             = get_summary_text(false, false, "bankedpay_d");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_bankedpay_p")).Text	                             = get_summary_text(false, false, "bankedpay_p");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_finalpayout")).Text	                             = can_see_wage ? get_summary_text(false, false, "finaltotal") : "$0.00";


		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_rt, "lb_vacation_total")).Text						         = get_summary_text(false, false, "vacation");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_vacation_total")).Text	                         = can_see_wage 
																																	? (Convert.ToDouble(get_summary_text(false, false, "vacation"))*packet.wage).ToString("C2") 
																																	: "$0.00";

		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_unpaidTimeOff, "lb_vacation_total")).Text					 = get_summary_text(false, false, "vacation_unpaid");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_unpaidTimeOff, "lb_timeoff_unpaid_total")).Text						 = get_summary_text(true, false, "unpaid_time_off");

		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_paidTimeOff, "lb_timeoff_paid_total")).Text					 = get_summary_text(true, false, "paid_time_off");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_total, "lb_timeoff_paid_total")).Text						 = can_see_wage 
																																	? (Convert.ToDouble(get_summary_text(true, false, "paid_time_off"))*packet.wage).ToString("C2") 
																																	: "$0.00";

		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_rt, "lb_hour_total")).Text									 = get_summary_text(true, true, "rt");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_ot, "lb_hour_total")).Text									 = get_summary_text(true, false, "ot");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_dt, "lb_hour_total")).Text									 = get_summary_text(true, false, "dt");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_rtsp, "lb_hour_total")).Text						         = get_summary_text(true, false, "rtsp");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_otsp, "lb_hour_total")).Text						         = get_summary_text(true, false, "otsp");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_dtsp, "lb_hour_total")).Text						         = get_summary_text(true, false, "dtsp");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_rt, "lb_bankedpay_d")).Text							         = get_summary_text(false, false, "bankedpay_d_hours");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_rt, "lb_bankedpay_w")).Text							         = get_summary_text(false, false, "bankedpay_w_hours");
		((ASPxLabel) gv_hours.FindFooterCellTemplateControl(col_rt, "lb_bankedpay_p")).Text							         = get_summary_text(false, false, "bankedpay_p_hours");

		((ASPxTextBox) gv_hours.FindFooterCellTemplateControl(col_rt, "tb_rt")).Text								         = get_summary_text(true, true, "rt");
		((ASPxTextBox) gv_hours.FindFooterCellTemplateControl(col_ot, "tb_ot")).Text								         = get_summary_text(true, false, "ot");
		((ASPxTextBox) gv_hours.FindFooterCellTemplateControl(col_dt, "tb_dt")).Text								         = get_summary_text(true, false, "dt");
		((ASPxTextBox) gv_hours.FindFooterCellTemplateControl(col_rtsp, "tb_rtsp")).Text							         = get_summary_text(true, false, "rtsp");
		((ASPxTextBox) gv_hours.FindFooterCellTemplateControl(col_otsp, "tb_otsp")).Text							         = get_summary_text(true, false, "otsp");
		((ASPxTextBox) gv_hours.FindFooterCellTemplateControl(col_dtsp, "tb_dtsp")).Text							         = get_summary_text(true, false, "dtsp");


		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_wage")).Value							         = can_see_wage ? get_wage().Replace(",", "") : "0";
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_is_salary")).Value								 = (packet.current_employee.paytype_id == 3 || packet.current_employee.paytype_id == 2).ToString();
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_total_hours")).Value					         = get_summary_text(true, true, "totalhoursminusholiday").Replace(",", "");
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_total_basepay")).Value	                         = can_see_wage ?get_summary_text(false, false, "totalbasepay").Replace(",", "") : "0";
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_total_vacation")).Value	                     = get_summary_text(false, false, "vacation").Replace(",", "");
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_all_outstanding")).Value	                     = allOutStandingVacation ? "1" : "0";
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_total_holiday")).Value					         = get_summary_text(false, false, "holiday").Replace(",", "");

		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_rt_original")).Value					         = get_summary_text(true, true, "rt");
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_ot_original")).Value					         = get_summary_text(true, true, "ot");
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_dt_original")).Value					         = get_summary_text(true, true, "dt");
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_rtsp_original")).Value					         = get_summary_text(true, true, "rtsp");
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_otsp_original")).Value					         = get_summary_text(true, true, "otsp");
		((HiddenField) gv_hours.FindFooterCellTemplateControl(col_type, "hid_dtsp_original")).Value					         = get_summary_text(true, true, "dtsp");
		}
	protected void gv_hours_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		var numeric_columns				= new []{"unpaid_time_off", "rt", "ot", "dt", "rtsp", "otsp", "dtsp", "paid_time_off"};
		var dr							= gv_hours.GetDataRow(e.VisibleIndex);
		if(dr == null) return;
		var type = dr["wotype"].ToString();
		if(Toolbox.Contains(e.DataColumn.FieldName, numeric_columns))
			{
			double.TryParse(Toolbox.ReturnZeroIfNull_double(e.CellValue).ToString(), out var hours);
			if(hours == 0)
				{
				e.Cell.ForeColor			= Color.LightGray;
				}
			if(type == "Expense" || type == "Per Diem")
				{
				e.Cell.BackColor			= Color.LightPink;
				}
			}
		else
			{
			switch(type)
				{
				case "Shop":
					e.Cell.ForeColor			= Color.Black;
				break;
				case "WO":
				break;
				case "Quote":
					e.Cell.ForeColor			= Color.SteelBlue;
				break;
				case "Per Diem":
				case "Expense":
					e.Cell.ForeColor			= Color.DarkRed;
					e.Cell.BackColor			= Color.LightPink;
				break;
				case "Vacation":
					e.Cell.ForeColor			= Color.DarkGreen;
				break;
				}
			}
		}
	protected void gv_hours_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{
		if (e.RowType != GridViewRowType.Data) return;
		var dr = gv_hours.GetDataRow(e.VisibleIndex);
		if (dr == null) return;
		var type = Toolbox.ReturnBlankIfNull_string(dr["wotype"]);
		switch(type)
			{
				case "Shop":
					e.Row.BackColor			= Color.FromArgb(183,207,227);
					break;
				case "WO":
					break;
				case "Quote":
					e.Row.BackColor			= Color.FromArgb(218,226,239);
					break;
				case "Per Diem":
				case "Expense":
					e.Row.BackColor			= Color.LightSalmon;
					break;
				case "Paid Time Off":
					e.Row.BackColor			= Color.LightGreen;
					break;
				case "Unpaid Time Off":
					e.Row.BackColor			= Color.FromArgb(211,249,134);
					break;
				case "Vacation":
					e.Row.BackColor			= Color.LightGreen;
					break;
			}
		}
	protected string get_wage()
		{
		return packet == null ? "0" : packet.wage.ToString();
		}
	private double get_utilization_for_payperiod()
		{
		var total_rt			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["rt"]));
		var total_ot			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["ot"]));
		var total_dt			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["dt"]));
		var total_rtsp			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["rtsp"]));
		var total_otsp			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["otsp"]));
		var total_dtsp			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["dtsp"]));
		var total_unpaid_timeoff		= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["unpaid_time_off"]));
		var total_paid_timeoff	= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["paid_time_off"]));
		var total				= total_rt+total_ot+total_dt+total_rtsp+total_otsp+total_dtsp+total_unpaid_timeoff;
		double shop				= 0;
		double quote			= 0;
		foreach(DataRow row in dt.Rows)
			{
			var t				= row["wotype"].ToString().ToLower();
			var rt				= Convert.ToDouble(row["rt"]);
			var unpaid			= Convert.ToDouble(row["unpaid_time_off"]);
			if(t == "shop" || t == "telem" || t == "salary" || t == "paid time off" || t == "unpaid time off" || t == "vacation")
				{
				shop			+= unpaid > 0 ? unpaid : rt;
				}
			else if(t == "quote")
				{
				quote			+= rt;
				}
			}
		return double.IsInfinity((total - shop - quote) / total) || double.IsNaN((total - shop - quote) / total) ? 0 : (total - shop - quote) / total;
		}
	protected string get_summary_text(bool _is_column_total, bool _remove_vacation, string field)
		{
		if(packet == null) return "";
		if(_is_column_total)
			{
			var si = gv_hours.TotalSummary[field];
			double total;
			var total_rt			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["rt"]));
			var total_ot			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["ot"]));
			var total_dt			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["dt"]));
			var total_rtsp			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["rtsp"]));
			var total_otsp			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["otsp"]));
			var total_dtsp			= Convert.ToDouble(gv_hours.GetTotalSummaryValue(gv_hours.TotalSummary["dtsp"]));
			var wage				= packet.wage;
			if(_remove_vacation)
				{
				total_rt	= allOutStandingVacation 
								? total_rt
								: total_rt - total_vacation;
				}
			if(field == "totalhours")
				{
				total		= total_rt+total_ot+total_dt+total_rtsp+total_otsp+total_dtsp;
				}
			else if(field == "totalhoursminusholiday")
				{
				// MH 2020-11-04 - Commenting this out until it's completed.
				// total = total_rt+total_ot+total_dt+total_rtsp+total_otsp+total_dtsp;

				total		= total_rt+total_ot+total_dt+total_rtsp+total_otsp+total_dtsp-total_holiday;
				}
			else if(field == "total")
				{
				total		=	total_rt*wage	* OpsPayType.Multipliers.RegularTime+
								total_ot*wage	* OpsPayType.Multipliers.OverTime+
								total_dt*wage	* OpsPayType.Multipliers.DoubleTime+
								total_rtsp*wage	* OpsPayType.Multipliers.RegularTimeShiftPremium+
								total_otsp*wage	* OpsPayType.Multipliers.OverTimeShiftPremium+
								total_dtsp*wage	* OpsPayType.Multipliers.DoubleTimeShiftPremium;
				}
			else
				{
				if (si == null)
					{
					return "0";
					}
				if(field == "rt")
					{
					return Toolbox.Contains(packet.current_employee.paytype_id, new []
																					{
																					OpsMemberPayType.SalaryHourlyWithTimeSheet,
																					OpsMemberPayType.SalaryHourlyWithoutTimeSheet
																					}) 
							? total_rt > 80 
								? "80" 
								: total_rt.ToString("N2") 
							: total_rt.ToString("N2");
					}
				total = Convert.ToDouble(gv_hours.GetTotalSummaryValue(si));
				if(field == "unpaid_time_off")
					{
					total = total == 0 && unpaidHasBeenOverrode ? total + total_vacation_unpaid : total;
					}
				}
			var is_dollar	 = si != null && si.FieldName == "total";
			return is_dollar 
							? total.ToString("C2") 
							: total.ToString("N2");
			}
		else
			{
			var si					= gv_hours.TotalSummary["total"];
			var total				= Convert.ToDouble(gv_hours.GetTotalSummaryValue(si));
			var total_final			= total - total_banked_d + total_banked_w + total_bonus + total_commission;
			if (si == null)
				{
				return "0";
				}
			switch(field)
				{
				case "bonuses":
					return total_bonus.ToString("C2");
				case "misc":
					return total_misc.ToString("C2");
				case "commissions":
					return total_commission.ToString("C2");
				case "holiday":
					return total_holiday.ToString("N2");
				case "holiday_base":
					var holiday_base_total_rows		= dt.Select("WOTYPE = 'HOLIDAY'");
					double holiday_base_total		= 0;
					if(holiday_base_total_rows.Length > 0)
						{
						foreach(var dr_h in holiday_base_total_rows)
							{
							// MH 2020-11-04 - Commenting this out until it's completed.
							// holiday_base_total		+= NeHolidays.StatPayValue(payperiod, packet.current_employee).amount;
							holiday_base_total += Convert.ToDouble(dr_h["RT"]);
							}
						}
					return holiday_base_total.ToString("N2");
				case "expenses":
					return (total_perdiems+total_expenses).ToString("C2");
				case "bankedpay_w":
					return total_banked_w.ToString("C2");
				case "bankedpay_d":
					return total_banked_d.ToString("C2");
				case "bankedpay_p":
					return total_banked_p.ToString("C2");
				case "bankedpay_w_hours":
					return total_banked_w_hours.ToString("N2");
				case "bankedpay_d_hours":
					return total_banked_d_hours.ToString("N2");
				case "bankedpay_p_hours":
					return total_banked_p_hours.ToString("N2");
				case "finaltotal":
					return total_final.ToString("C2");
				case "vacation":
					return total_vacation.ToString("N2");
				case "vacation_unpaid":
					return total_vacation_unpaid.ToString("N2");
				case "paid_time_off":
					return TotalPaidTimeOff.ToString("N2");
				case "totalbasepay":
					return (
					total_vacation*packet.wage+
					total_banked_p+
					total_banked_w-
					total_banked_d+
					total_expenses+
					total_bonus+
					total_commission+
					TotalPaidTimeOff*packet.wage+
					total_misc
					).ToString("N2");
				default:
					return "0";
				}
			}
		}
	protected bool show_row(string field)
		{
		switch(field)
			{
			case "bonuses":
				return total_bonus != 0;
			case "commissions":
				return total_commission != 0;
			case "bankedpay_w":
				return total_banked_w != 0;
			case "bankedpay_d":
				return total_banked_d != 0;
			case "vacation":
				return total_vacation != 0;
			default:
				return true;
			}
		}
	private List<string> list_week			= new List<string>();
	private void do_databind()
		{
		gv_hours.DataBind();
		}
	protected void rt_DataBound(object sender, EventArgs e)
		{
		var tb				= (ASPxTextBox) sender;
		var cell			= (GridViewDataItemTemplateContainer) tb.NamingContainer;
		var dr				= cell?.Grid.GetDataRow(cell.VisibleIndex);
		if(dr == null) return;
		var hour_type		= (string) dr["wotype"];
		tb.ClientEnabled	= hour_type.ToLower() == "holiday" || hour_type.ToLower() == "salary";
		tb.CssClass			+= " "+hour_type.ToLower();
		var week			= (string) dr["w"];
		if(!list_week.Contains(week))
			{
			list_week.Add(week);
			}
		tb.CssClass			+= " w"+(list_week.FindIndex(x => x == week)+1);
		// MH 2020-11-04 - Commenting this out until it's completed.
		//if(hour_type.ToLower() == "holiday")
		//	{
		//	var total = NeHolidays.StatPayValue(payperiod, packet.current_employee); 
		//	tb.Value = total.amount;
		//	tb.ToolTip = total.reason;
		//	}
		}
	protected void gv_hours_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
		{
		if(e.IsGroupSummary && e.Item.ShowInGroupFooterColumn == "wotype")
			{
			e.Text	= "Week Totals: ";
			}
		else
			{
			e.Text	= e.Text;
			}
		}
	protected void gv_hours_CustomColumnGroup(object sender, CustomColumnSortEventArgs e)
		{

		}
	protected void cbp_approval_Callback(object sender, CallbackEventArgsBase e)
		{
		if(!e.Parameter.Contains('|')) return;
		var paras			= e.Parameter.Split('|');
		if(paras.Length < 2) return;
		var id			= Convert.ToInt32(paras[1]);
		switch(paras[0])
			{
			case "get_user":
				packet.current_employee		= new NeMember(id);
			break;
			case "prev":
				packet.current_employee		= new NeMember(payroll.get_user_at(packet.your_to_approve, packet.payperiod_id, packet.business_unit_id, packet.handler.id32, id, 1));
			break;
			case "next":
				packet.current_employee		= new NeMember(payroll.get_user_at(packet.your_to_approve, packet.payperiod_id, packet.business_unit_id, packet.handler.id32, id, 2));
			break;
			case "save":
				var p						= new JavaScriptSerializer().Deserialize<payroll.hour_packet>(paras[2]);
				var checkVacations			= payroll.vacation.hasUnapprovedVacations(packet.current_employee.id, packet.payperiod_id);
				if(checkVacations)
					{
					throw new Exception(@"This employee has vacations that need to be approved/denied before their payroll can be submitted - Please review the vacation admin page");
					}
				if(!payroll.HourEntryExists(id, packet.payperiod_id))
					{
					packet.current_employee		= new NeMember(id);
					payroll.submit_approved_pay(packet, p);
					packet.your_to_approve.Remove(Convert.ToInt32(id));
					packet.current_employee		= packet.your_to_approve.Any() 
													? new NeMember(payroll.get_user_at(packet.your_to_approve, packet.payperiod_id, packet.business_unit_id, packet.handler.id32, id, 2))
													: new NeMember();
					if(packet.current_employee.id == 0)
						{
						lb_members.Items.Clear();
						cbp_approval.JSProperties["cpDoJS"]			= "pc.SetTabEnabled(0, false);pc.SetActiveTabIndex(1);";
						}
					CheckVisible();
					}
				else
					{
					throw new Exception("User entry already exists - Please refresh your page");
					}
			break;
			case "expense":
				using(var conn = Toolbox.connect())
					{
					var exp		= new payroll.expense(id);
					var origin = exp.id_seller == 309 ?OpsWOLineOrigin.PerDiemExpense :OpsWOLineOrigin.ExpenseReimbursement;
					exp.approved		= Convert.ToInt32(paras[2]);
					exp.approved_by		= packet.handler.id32;
					exp.save();
					var wo_lineid		= exp.woprog_id == 0 
												? 0
												: Toolbox.doSQL_int(conn, @"
												SELECT 
													IFNULL(MAX(id), 0)
												FROM
													wo_detail
												WHERE 
													woprog_id = @v3 AND
													memberid = @v0 AND 
													consignment_id = @v1 AND
													origin = @v2 ", 
											new object[] { exp.id_member, exp.id_expense,origin, exp.woprog_id});
					var wo					= exp.woprog_id > 0 ? new NeWOProg(exp.woprog_id) : new NeWOProg();
					var wo_line				= wo_lineid > 0 ? new NeWODetailCurrent(wo_lineid) : new NeWODetailCurrent();
					var wo_available		= payroll.expense.workorder_available(exp, wo);
					if(exp.woprog_id > 0 && wo_available)
						{
						if(exp.approved == 1 && wo_lineid == 0)
							{
							payroll.expense.attach(conn, wo, exp, ref wo_line, packet.handler);
							}
						else if(exp.approved == 0 && wo_lineid > 0)
							{
							payroll.expense.detach(conn, wo, exp, ref wo_line, packet.handler);
							}	
						}
					else if(!wo_available && exp.woprog_id > 0)
						{
						shared.alert_payroll(exp.approved == 1 
													? string.Format("(Payroll Approval Notice) Expense cannot be added to work order {0}", wo.OrderNumber) 
													: string.Format("(Payroll Approval Notice) Expense cannot be removed from work order {0}", wo.OrderNumber), 
													string.Format("The work order {0} is currently in the status {1}, and cannot be edited.", wo.OrderNumber, wo.Status));
						}
					packet.current_employee		= new NeMember(exp.id_member);
					do_databind();
					}
			break;
			case "vacation":
				var v = new VacationRequest(id);
				v.status_id = Convert.ToInt32(paras[2]) == 1 ? 3 : 2;
				v.save_record();
				//Toolbox.doSQL_void(string.Format(@"UPDATE passport SET active = 0 WHERE url_yes LIKE '%vacation_id={0}%'", id), new object[] { });
				packet.current_employee		= new NeMember(v.member_id);
				do_databind();
			break;
			case "clear":
				payroll.hours.clear(id);
				packet.current_employee		= new NeMember(Convert.ToInt32(paras[2]));
				// MH 2020-11-04 - Commenting this out until it's completed.
				// payroll.vacation.RemoveExistingPayout(packet);
				packet.your_to_approve.Add(Convert.ToInt32(paras[2]));
				packet.your_to_approve.Sort(); // Doing this because we want the user to be brought back to the same user, with the same prev/next users (if applicable)
				cbp_approval.JSProperties["cpDoJS"]			= "pc.SetTabEnabled(0, true);";
				CheckVisible();
			break;
			}
		load();
		}
	protected void lb_description_DataBound(object sender, EventArgs e)
		{
		var lb				= (ASPxLabel) sender;
		var cell			= (GridViewDataItemTemplateContainer) lb.NamingContainer;
		if (cell == null) return;
		var dr				= cell.Grid.GetDataRow(cell.VisibleIndex);
		if(dr == null) return;
		var hour_type		= dr["wotype"].ToString().ToLower();
		var status			= dr["item_status"].ToString();
		var id				= dr["item_id"].ToString();
		if(hour_type == "expense" || hour_type == "per diem")
			{
			if(status == "-1")
				{
				lb.Text			= string.Format(@" 
				<span class='unapproved' style='color:#f00;'>
					<button type='button' onclick='approval.expense.do(this, {0}, false);'>
						<img src='/images/icon/icon[deny].gif' width='16' height='16' title='Deny'/>
					</button>
					<button type='button' onclick='approval.expense.do(this, {0}, true);'>
						<img src='/images/icon/icon[approve].gif' width='16' height='16' title='Approve'/>
					</button>
				</span>
				{1}", id, lb.Text);
				}
			else
				{
				lb.Text			= string.Format("{0} ({1} Approved)", lb.Text, hour_type);
				}
			}
		else if(hour_type == "wo")
			{
			lb.Text				= string.Format(@"<a href='javascript:void(0)' onclick=""boing('/sections/workorder/index.aspx?woprog_id={0}', 'workorder', 1280,960);"">{1}</a>", id, lb.Text);
			}
		else if(hour_type == "quote")
			{
			int quote_id, revision;
			if(id == "") return;

			quote.splice(id, out quote_id, out revision);
			lb.Text				= string.Format(@"<a href='javascript:void(0)' onclick=""boing('/#/opens/65/quotes/{0}/{1}','quote_count{0}',1060,920);"">{2}</a>", quote_id, revision, lb.Text);
			}
		else if(hour_type == "vacation")
			{
			if(status == "1")
				{
				lb.Text			= string.Format(@" 
				<span class='unapproved' style='color:#f00;'>
					<button type='button' onclick='approval.vacation.do(this, {0}, false);'>
						<img src='/images/icon/icon[deny].gif' width='16' height='16' title='Deny'/>
					</button>
					<button type='button' onclick='approval.vacation.do(this, {0}, true);'>
						<img src='/images/icon/icon[approve].gif' width='16' height='16' title='Approve'/>
					</button>
				</span>
				{1}", id, lb.Text);
				}
			else
				{
				lb.Text			= string.Format("{0} (Approved)", lb.Text);
				}
			}
		}
}