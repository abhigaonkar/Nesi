<%@ WebHandler Language="C#" Class="export" %>

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Collections.Specialized;
using System.IO;
using NESI.Common.Models;
using nesi.core;

public class export : IHttpHandler, IRequiresSessionState
	{
	HttpRequest req;
	HttpResponse resp;
	NameValueCollection _q;
	NeMember current_user;
	bool is_debug = false;
	DateTime startTime;
	DateTime endTime;
	public void ProcessRequest(HttpContext context)
		{
		startTime = DateTime.Now;
		req = context.Request;
		resp = context.Response;
		_q = req.QueryString;
		is_debug = _q["is_debug"] == OpsGeneralString.True && req.Url.Host == OpsGeneralString.DebugHost;
		if (string.IsNullOrEmpty(_q["a"]) || string.IsNullOrEmpty(_q["adp_company_code"])) return;
		current_user = context.Session["session"] == null
									? Toolbox.do_handle_authentication(OpsPage.PayrollViewer)
									: new NeMember(context.Session["session"].ToString());
		if (!current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewWageAndFileTabs))
			{
			Toolbox.FriendlyException(resp, "You do not have access to this page", "/index.html");
			}

		var action = _q["a"];
		var sb = new StringBuilder();
		var content_type = "";
		var file_name = "";
		// SD: throw this error if there are same adp_company code in different regions
		var company_code = _q["adp_company_code"].ToString();
		var existsOnDifferentRegions = Toolbox.doSQL_int(@"SELECT COUNT(DISTINCT region) FROM tax_entity WHERE adp_company_code =  @v0", new object[] { company_code });
		if (existsOnDifferentRegions > 1)
			{   Toolbox.FriendlyException(resp, "Multiple countries with same company code exists, please cut a ticket", "/index.html"); }
		// SD: The below variable cannot be extracted via object since adp company code can be same for multiple tax entities to group them in the Export. During this task, the grouping is primarily for US tax entities
		var taxEntityRegion = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(region),'N/A') FROM tax_entity WHERE adp_company_code = @v0",new object[] { company_code});
		var baseCountry = Toolbox.doSQL_string(@"SELECT MAX(country) FROM business_unit a LEFT JOIN tax_entity b ON a.tax_entity_id =b.id WHERE b.adp_company_code = @v0",new object[] { company_code});
		int payperiod_id = NePayPeriod.CurrentOpenPayPeriodId();
		using (var conn = Toolbox.connect())
			{
			if (action == "exportbusiness") // MH: Currently easier to debug if the process starts with the general exportbusiness action, and is overridden by the export page
				{
				if (taxEntityRegion == OpsCountry.UnitedStates)
					{
					action = "us_payroll";
					}
				else if (taxEntityRegion == "Canada")
					{
					action = "canada_payroll";
					}
				}
			if (action == "wage")
				{
				// SD: The logic for Wage Export also has been modified as the same drop down list is used for both wage and payroll export. Wage now will also be grouped as per apd company code in the tax entities
				content_type = NeFiles.GetMimeType(".csv");
				file_name = "wage" + company_code + "-" + DateTime.Now.ToString("MMdd_HH_mm_ss") + ".csv";
				payperiod_id = Convert.ToInt32(_q["payperiod_id"]);
				var dt_wages = Toolbox.doSQL_dt(@"
SELECT 
	b.member_payroll_id id, 
	CONCAT(d.adp_company_code,b.Member_Payroll_ID) us_id,
	DATE_FORMAT(a.startdate, '%m%d%Y') effectivedate,
	a.wage 
FROM 
	(member_offers a, payperiods pp)
LEFT JOIN
	member b ON a.memberid = b.member_id
LEFT JOIN 
	business_unit c ON b.business_unit_id = c.id 
LEFT JOIN 
	tax_entity d ON c.tax_entity_id = d.id
WHERE 
	pp.PayperiodID = @v1 AND
	a.startdate BETWEEN pp.StartDate AND pp.EndDate AND
	a.status IN ('Accepted', 'Awaiting Start Date') AND
	d.adp_company_code = @v0 AND 
	b.member_status = 'Active' AND 
	b.paytype_id != @v2", new object[] { company_code, payperiod_id, OpsMemberPayType.Subcontract }); // No sub-contracts
				if (baseCountry == OpsCountry.Canada)
					{
					sb.AppendFormat("Position ID, Change Effective On, Rate Type, Rate Amount, Compensation Change Reason\n");
					foreach (DataRow dr in dt_wages.Rows)
						{
						var id = dr["id"].ToString();
						var effectiveDate = dr["effectiveDate"];
						double wage;
						double.TryParse(dr["wage"].ToString(), out wage);
						if (id == "" || id == "0") continue;
						sb.AppendFormat("{0},{1},H,{2},GINC\n", id, effectiveDate, wage);
						}
					}
				else
					{
					sb.AppendFormat("Position ID, Change Effective On, Compensation Change Reason, Rate Type, Rate 1 Amount\n");
					foreach (DataRow dr in dt_wages.Rows)
						{
						var id = dr["us_id"].ToString();
						var effectiveDate = dr["effectiveDate"];
						double wage;
						double.TryParse(dr["wage"].ToString(), out wage);
						if (id == "" || id == "0") continue;
						sb.AppendFormat("{0},{1},GINC,H,{2}\n", id, effectiveDate, wage);
						}
					}
				}
			else if (action == "us_payroll")
				{
				#region us_payroll
				payperiod_id = Convert.ToInt32(_q["p_id"]); ;
				file_name = "EPI" + company_code + "XX.csv";
				var payperiod = new NePayPeriod(payperiod_id);
				content_type = NeFiles.GetMimeType(".csv");
				sb.Append("ID, Det, detCode, Hours, Amount, rate\n"); // SD: New column headers
				var dt = Toolbox.doSQL_dt(conn, @"CALL payroll_export(@v0 , @v1 )", new object[] { company_code, payperiod_id });
				dt.DefaultView.Sort = "MEMBER_ID ASC, CODE ASC";
				dt = dt.DefaultView.ToTable();
				var vacations_paid = new List<int>();
				var vacation_withdrawn_paid = new List<int>();
				var expenses_paid = new List<int>();
				var oncall_paid = new List<int>();
				var perdiems_paid = new List<int>();
				var bonuses_paid = new List<int>();
				var commissions_paid = new List<int>();
				var misc_paid = new List<int>();
				var stat_paid = new List<int>();
				var sickday_paid = new List<int>();
				var birthday_paid = new List<int>();
				var bereavement_paid = new List<int>();
				var how_many_holidays = payroll.how_many_holidays(OpsCountry.UnitedStates, payperiod.StartDate, payperiod.Enddate);
				foreach (DataRow dr in dt.Rows)
					{
					#region VARIABLE DECLARATION
					var employeeid = dr["EMPLOYEE_ID"].ToString();
					var member_id = Convert.ToInt32(dr["MEMBER_ID"]);
					var memberpaytype_id = payroll.paytype_at_date(conn, payperiod.end_date, member_id);
					var wageAtDate = payroll.get_wage_at_date(conn, member_id, payperiod.end_date);
					var commission = payroll.extra_payments.get(conn, member_id, OpsPayroll.TransactionType.ExtraPayment.Commission, payperiod_id);
					var misc = payroll.extra_payments.get(conn, member_id, OpsPayroll.TransactionType.ExtraPayment.Miscellaneous, payperiod_id);
					var bonus = payroll.extra_payments.get(conn, member_id, OpsPayroll.TransactionType.ExtraPayment.Bonus, payperiod_id);
					var expenses_total = payroll.expense.this_expense(conn, member_id, payperiod_id);
					var on_Call_total = payroll.expense.this_on_call(conn, member_id, payperiod_id);
					var perdiems_total = payroll.expense.this_per_diems(conn, member_id, payperiod_id);
					double vacation_amount = payroll.vacation.this_payperiod(conn, false, member_id, payperiod_id, payperiod.is_complete);
					double vacation_payout = memberpaytype_id == OpsMemberPayType.Hourly ?  payroll.vacation.this_payperiod(conn, true, member_id, payperiod_id, payperiod.is_complete) : 0;
					vacation_amount = vacation_amount == 999999 ? 0 : vacation_amount;
					vacation_payout = vacation_payout == 999999 ? 0 : vacation_payout;
					var earningnumber = Convert.ToInt32(dr["CODE"].ToString());
					var earning_number = earningnumber < 10 ? "0" + earningnumber : earningnumber.ToString();
					var isRegularHr = earning_number == OpsPayroll.PayrollExport.PayrollEntryType.RT;
					var detCode = "";
					var hours = Convert.ToDouble(dr["HOURS"].ToString());
					var paidSickTotal = payroll.DaysOff.ThisPayPeriod(conn, member_id, payperiod_id, OpsPayroll.DaysOffType.SickDayPaid);
					var birthdayTotal = payroll.DaysOff.ThisPayPeriod(conn, member_id, payperiod_id, OpsPayroll.DaysOffType.Birthday);
					var bereavementTotal = payroll.DaysOff.ThisPayPeriod(conn, member_id, payperiod_id, OpsPayroll.DaysOffType.Bereavement);
					#endregion VARIABLE DECLARATION
					#region Hours -> RT, OT, DT , RTSP, OTSP, DTSP
					// SD: Regular hours is capped at 80 for the Employees of type Salary Houely with and without TS 
					// Also, note that there is no Banked pay calculation for US payroll
					hours = (isRegularHr && hours > 80 && Toolbox.Contains(memberpaytype_id, new[] {OpsMemberPayType.SalaryHourlyWithTimeSheet,OpsMemberPayType.SalaryHourlyWithoutTimeSheet}))
										? 80
										: hours;
					if (hours != 0)
						{
						switch(earning_number)
							{
							case OpsPayroll.PayrollExport.PayrollEntryType.RT:
							detCode = OpsPayroll.PayrollExport.ExportDetCodes.REG;
							break;
							case OpsPayroll.PayrollExport.PayrollEntryType.OT:
							detCode = OpsPayroll.PayrollExport.ExportDetCodes.OT;
							break;
							case OpsPayroll.PayrollExport.PayrollEntryType.DT:
							detCode = OpsPayroll.PayrollExport.ExportDetCodes.DT;
							break;
							case OpsPayroll.PayrollExport.PayrollEntryType.RTSP:
							detCode = OpsPayroll.PayrollExport.ExportDetCodes.RTShiftPrem;
							break;
							case OpsPayroll.PayrollExport.PayrollEntryType.DTSP:
							detCode = OpsPayroll.PayrollExport.ExportDetCodes.DTShiftPrem;
							break;
							case OpsPayroll.PayrollExport.PayrollEntryType.OTSP:
							detCode = OpsPayroll.PayrollExport.ExportDetCodes.OTShiftPrem;
							break;
							}
						sb.AppendFormat("{0},E ,{1}, {2}, ,{3} \n", employeeid, detCode, hours , wageAtDate );
						}
					#endregion Hours -> RT , OT, DT , RTSP, OTSP, DTSP

					#region VACATION

					if (vacation_amount > 0  && !vacations_paid.Contains(member_id))
						{

						if (!payroll.vacation.has_been_paid_out(member_id, payperiod_id) && !is_debug)
							{
							payroll.vacation.payout(member_id, payperiod_id);
							}
						//SD: As there is one column for Vacation in Paylocity as opposed to two in Payworks, we are adding the Vacation amount and Vacation Payout. This results as a hour Value                        
						var vac_total = vacation_amount ;
						detCode = OpsPayroll.PayrollExport.ExportDetCodes.VAC;
						sb.AppendFormat("{0},E ,{1},{2},  ,{3} \n", employeeid, detCode, vac_total , wageAtDate );
						vacations_paid.Add(member_id);

						}
					#endregion VACATION

					#region VACATION Withdrawn

					if (vacation_payout > 0 && !vacation_withdrawn_paid.Contains(member_id))
						{

						if (!payroll.vacation.has_been_paid_out(member_id, payperiod_id) && !is_debug)
							{
							payroll.vacation.payout(member_id, payperiod_id);
							}

						var vac_withdrawn = (vacation_payout * wageAtDate);
						detCode = OpsPayroll.PayrollExport.ExportDetCodes.VACWITH;
						sb.AppendFormat("{0},E ,{1}, , {2}, {3} \n", employeeid, detCode, vac_withdrawn , wageAtDate );
						vacation_withdrawn_paid.Add(member_id);
						}
					#endregion VACATION
					#region Expenses Reimbursement
					if (expenses_total > 0 && !expenses_paid.Contains(member_id))
						{

						detCode = OpsPayroll.PayrollExport.ExportDetCodes.ExpenseReimbursement;
						//SD: Previously, there was a logic to have expenses shown as negative amount for membertype f1099, it has been removed
						sb.AppendFormat("{0},E ,{1} , ,{2} ,{3} \n", employeeid, detCode, expenses_total , wageAtDate );
						expenses_paid.Add(member_id);
						}
					#endregion
					#region Oncal
					if (on_Call_total > 0 && !oncall_paid.Contains(member_id))
						{

						detCode = OpsPayroll.PayrollExport.ExportDetCodes.ONCAL;
						sb.AppendFormat("{0},E ,{1} , ,{2} ,{3} \n", employeeid, detCode, on_Call_total , wageAtDate );
						oncall_paid.Add(member_id);
						}
					#endregion
					#region Paid Sick Day
					if (paidSickTotal > 0 && !sickday_paid.Contains(member_id))
						{
						detCode =OpsPayroll.PayrollExport.ExportDetCodes.SICK;
						sb.AppendFormat("{0},E ,{1},{2},  ,{3} \n", employeeid, detCode, paidSickTotal , wageAtDate );
						sickday_paid.Add(member_id);
						}
					#endregion
					#region BONUS
					if (bonus > 0 && !bonuses_paid.Contains(member_id))
						{
						detCode = OpsPayroll.PayrollExport.ExportDetCodes.BONUS;
						sb.AppendFormat("{0},E ,{1} , ,{2} ,{3} \n", employeeid, detCode, bonus , wageAtDate );
						bonuses_paid.Add(member_id);
						}
					#endregion BONUS
					#region COMMISSION
					if (commission > 0 && !commissions_paid.Contains(member_id))
						{
						detCode = OpsPayroll.PayrollExport.ExportDetCodes.Commission;
						sb.AppendFormat("{0},E ,{1} , ,{2} ,{3} \n", employeeid, detCode, commission , wageAtDate );
						commissions_paid.Add(member_id);
						}
					#endregion COMMISSION
					#region Per Diem
					if (perdiems_total > 0 && !perdiems_paid.Contains(member_id))
						{
						detCode = OpsPayroll.PayrollExport.ExportDetCodes.PerDiem;
						sb.AppendFormat("{0},E ,{1} , ,{2} ,{3} \n", employeeid, detCode, perdiems_total , wageAtDate );
						perdiems_paid.Add(member_id);
						}
					#endregion
					#region Birthday
					if (birthdayTotal > 0 && !birthday_paid.Contains(member_id))
						{
						detCode = OpsPayroll.PayrollExport.ExportDetCodes.BDAY;
						sb.AppendFormat("{0},E ,{1} ,{2},  ,{3} \n", employeeid, detCode, birthdayTotal , wageAtDate );
						birthday_paid.Add(member_id);
						}
					#endregion
					#region Bereavement
					if (bereavementTotal > 0 && !bereavement_paid.Contains(member_id))
						{
						detCode = OpsPayroll.PayrollExport.ExportDetCodes.BEREAVE;
						sb.AppendFormat("{0},E ,{1} ,{2},  ,{3} \n", employeeid, detCode, bereavementTotal , wageAtDate );
						bereavement_paid.Add(member_id);
						}
					#endregion
					#region STATPAY
					if (how_many_holidays > 0 && !stat_paid.Contains(member_id))
						{
						var holiday_pay = Convert.ToDouble(dr["stat"]);
						if (holiday_pay > 0)
							{

							detCode = OpsPayroll.PayrollExport.ExportDetCodes.STAT;
							sb.AppendFormat("{0},E ,{1}, {2},  ,{3} \n", employeeid, detCode, holiday_pay , wageAtDate );
							stat_paid.Add(member_id);
							}
						}
					#endregion STATPAY

					}
				}
			#endregion us_payroll

			else if (action == "canada_payroll")
				{
				#region canada_payroll - Payworks
				payperiod_id = Convert.ToInt32(_q["p_id"]);
				var payperiod = new NePayPeriod(payperiod_id);
				content_type = NeFiles.GetMimeType(".csv");
				file_name = "EPI" + company_code + "CP.csv";
				var bankedPay = new payroll.banked_pay();
				sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21}\n",
						OpsPayroll.PayrollExport.Fields.PayWorks.Number,                    // {0}
						OpsPayroll.PayrollExport.Fields.PayWorks.Rate,                      // {1}
						OpsPayroll.PayrollExport.Fields.PayWorks.RTHours,                   // {2}
						OpsPayroll.PayrollExport.Fields.PayWorks.OTHours,                   // {3}
						OpsPayroll.PayrollExport.Fields.PayWorks.DTHours,                   // {4}
						OpsPayroll.PayrollExport.Fields.PayWorks.RTSPHours,                 // {5}
						OpsPayroll.PayrollExport.Fields.PayWorks.OTSPHours,                 // {6}
						OpsPayroll.PayrollExport.Fields.PayWorks.DTSPHours,                 // {7}
						OpsPayroll.PayrollExport.Fields.PayWorks.BankedIn,                  // {8}
						OpsPayroll.PayrollExport.Fields.PayWorks.BankedOut,                 // {9}
						OpsPayroll.PayrollExport.Fields.PayWorks.VacationTaken,             // {10}
						OpsPayroll.PayrollExport.Fields.PayWorks.VacationWithdrawn,         // {11}
						OpsPayroll.PayrollExport.Fields.PayWorks.Reimbursement,             // {12}
						OpsPayroll.PayrollExport.Fields.PayWorks.OnCall,                    // {13}
						OpsPayroll.PayrollExport.Fields.PayWorks.SickPay,                   // {14}
						OpsPayroll.PayrollExport.Fields.PayWorks.SickPayOther,              // {15}
						OpsPayroll.PayrollExport.Fields.PayWorks.Bonus,                     // {16}
						OpsPayroll.PayrollExport.Fields.PayWorks.Commission,                // {17}
						OpsPayroll.PayrollExport.Fields.PayWorks.PerDiem,                   // {18}
						OpsPayroll.PayrollExport.Fields.PayWorks.Birthday,                  // {19}
						OpsPayroll.PayrollExport.Fields.PayWorks.Bereavement,               // {20}
						OpsPayroll.PayrollExport.Fields.PayWorks.StatPay                    // {21}
						);

				var dt = Toolbox.doSQL_dt(conn, @"CALL payroll_export(@v0 , @v1 )", new object[] { company_code, payperiod_id });
				dt.DefaultView.Sort = "MEMBER_ID ASC, CODE ASC";
				dt = dt.DefaultView.ToTable();
				var banked_paid = new List<int>();
				var vacations_paid = new List<int>();
				var expenses_paid = new List<int>();
				var oncall_paid = new List<int>();
				var perdiems_paid = new List<int>();
				var bonuses_paid = new List<int>();
				var commissions_paid = new List<int>();
				var misc_paid = new List<int>();
				var stat_paid = new List<int>();
				var sickday_paid = new List<int>();
				var sickday_other = new List<int>();
				var birthday_paid = new List<int>();
				var bereavement_paid = new List<int>();
				var how_many_holidays = payroll.how_many_holidays("CAN", payperiod.StartDate, payperiod.Enddate);
				var members = new Dictionary<string, PayrollRowsPayworks>();
				foreach(DataRow dr in dt.Rows)
					{
					var employeeid = dr["EMPLOYEE_ID"].ToString();
					if(!members.ContainsKey(employeeid))
						{
						var pr = new PayrollRowsPayworks();
						members.Add(employeeid, pr);
						}
					var empRef = members[employeeid];
					var earningnumber = dr["Code"].ToString();
					var hours = Convert.ToDouble(dr["HOURS"].ToString());
					empRef.MemberID = Convert.ToInt32(dr["member_id"]);
					var paytype_id = payroll.paytype_at_date(conn, payperiod.end_date,empRef.MemberID);
					double vacation_amount = payroll.vacation.this_payperiod(conn, false, empRef.MemberID, payperiod_id, payperiod.is_complete);
					double vacation_payout = paytype_id == OpsMemberPayType.Hourly
														? payroll.vacation.this_payperiod(conn, true, empRef.MemberID, payperiod_id, payperiod.is_complete)
														: 0;
					vacation_payout = vacation_payout == 999999 ? 0 : vacation_payout;
					vacation_amount = vacation_amount == 999999 ? 0 : vacation_amount;
					bankedPay.member_id = empRef.MemberID;
					var deposited = bankedPay.get_hours(conn, OpsPayroll.TransactionType.BankedPay.Deposited, payperiod_id);
					empRef.NumberEmpID = employeeid;
					empRef.Rate = payroll.get_wage_at_date(conn, empRef.MemberID, payperiod.end_date);
					switch(earningnumber)
						{
						case OpsPayroll.PayrollExport.PayrollEntryType.RT:
						//SD: If there is a vacation amount for an employee of type Salay hourly without TS, it is taken off from their Regular hours.! 

						switch(paytype_id)
							{
							case OpsMemberPayType.SalaryHourlyWithoutTimeSheet:
							empRef.RegHrs = (vacation_payout > 0 || hours != 80)
										   ? hours.ToString()
										   : (hours - vacation_amount).ToString();

							break;
							case OpsMemberPayType.Hourly:
							empRef.RegHrs = (hours - deposited).ToString();
							break;
							default:
							empRef.RegHrs = hours.ToString();
							break;
							}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.DT:
						empRef.DoubleTime = hours.ToString();
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.OT:
						empRef.OverTime = hours.ToString();
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.RTSP:
						empRef.PremiumRT = hours.ToString();
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.DTSP:
						empRef.PremiumDT = hours.ToString();
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.OTSP:
						empRef.PremiumOT = hours.ToString();
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.BANK:
						// Banked hours previously were added in the Regular hours. It has been separated and divied up into Banked hours in and banked time paid
						if (!banked_paid.Contains(empRef.MemberID))
							{
							empRef.BankedHoursIn = deposited.ToString();
							var withdrawn = bankedPay.get_hours(conn, OpsPayroll.TransactionType.BankedPay.Withdrawn, payperiod_id);
							var paid_out = bankedPay.get_hours(conn, OpsPayroll.TransactionType.BankedPay.PaidOut, payperiod_id);
							empRef.BankTimePaid = (withdrawn + paid_out).ToString();
							banked_paid.Add(empRef.MemberID);
							}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.VAC:
						var conditionsForHourly = paytype_id == OpsMemberPayType.Hourly && (vacation_amount > 0 || vacation_payout > 0) && !vacations_paid.Contains(empRef.MemberID);
						var conditionsForSalary = vacation_amount > 0;
						if (conditionsForHourly || conditionsForSalary)
							{

							if (!payroll.vacation.has_been_paid_out(empRef.MemberID, payperiod_id) && !is_debug)
								{
								payroll.vacation.payout(empRef.MemberID, payperiod_id);
								}
							empRef.VacTaken = vacation_amount > 0 ? vacation_amount.ToString() : "0";
							empRef.VacWithd = conditionsForHourly && vacation_payout > 0 ? (vacation_payout * empRef.Rate).ToString() : "0";
							vacations_paid.Add(empRef.MemberID);
							}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.EXP:
						var expenses_total = payroll.expense.this_expense(conn, empRef.MemberID, payperiod_id);
						if (expenses_total > 0 && !expenses_paid.Contains(empRef.MemberID))
							{
							empRef.Reimbursement = expenses_total.ToString();
							expenses_paid.Add(empRef.MemberID);
							}

						// On call is being added as an expense reimbursement - category 51 - 'Per Diem Other' which ends up being id_seller 492
						// If this way of entering on call is set in stone, then the following logic should capture on call correctly
						var oncall_total = payroll.expense.this_on_call(conn, empRef.MemberID, payperiod_id);
						if (oncall_total > 0 && !oncall_paid.Contains(empRef.MemberID))
							{
							empRef.OnCall = oncall_total.ToString();
							oncall_paid.Add(empRef.MemberID);
							}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.PAIDSICKDAY:
						if(paytype_id == OpsMemberPayType.Hourly) {
							var paidSickTotal = payroll.DaysOff.ThisPayPeriod(conn, empRef.MemberID, payperiod_id, OpsPayroll.DaysOffType.SickDayPaid);

							if (paidSickTotal > 0 && !sickday_paid.Contains(empRef.MemberID))
								{
								empRef.SickPay = paidSickTotal.ToString();
								sickday_paid.Add(empRef.MemberID);
								}}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.PAIDOTHER:
						if(paytype_id == OpsMemberPayType.Hourly) {
							var paidSickOther = payroll.DaysOff.ThisPayPeriod(conn, empRef.MemberID, payperiod_id, OpsPayroll.DaysOffType.SickDayOther);

							if (paidSickOther > 0 && !sickday_other.Contains(empRef.MemberID))
								{
								empRef.SickOther = paidSickOther.ToString();
								sickday_other.Add(empRef.MemberID);
								}}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.BON:
						var bonus = payroll.extra_payments.get(conn, empRef.MemberID, OpsPayroll.TransactionType.ExtraPayment.Bonus, payperiod_id);
						if (bonus > 0 && !bonuses_paid.Contains(empRef.MemberID))
							{
							empRef.Bonus=bonus.ToString();
							bonuses_paid.Add(empRef.MemberID);
							}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.COMM:
						var commission = payroll.extra_payments.get(conn, empRef.MemberID, OpsPayroll.TransactionType.ExtraPayment.Commission, payperiod_id);
						if (commission > 0 && !commissions_paid.Contains(empRef.MemberID))
							{
							empRef.SalesCommission = commission.ToString();
							commissions_paid.Add(empRef.MemberID);
							}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.PerDiem:
						var perdiems_total = payroll.expense.this_per_diems(conn, empRef.MemberID, payperiod_id);
						if (perdiems_total > 0 && !perdiems_paid.Contains(empRef.MemberID))
							{
							empRef.PerDiem=perdiems_total.ToString();
							perdiems_paid.Add(empRef.MemberID);
							}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.BDAY:
						var birthdayTotal = payroll.DaysOff.ThisPayPeriod(conn, empRef.MemberID, payperiod_id, OpsPayroll.DaysOffType.Birthday);
						if(birthdayTotal > 0 && !birthday_paid.Contains(empRef.MemberID))
							{
							empRef.BDay = birthdayTotal.ToString();
							birthday_paid.Add(empRef.MemberID);
							}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.BEREAVEMENT:
						var bereavementTotal = payroll.DaysOff.ThisPayPeriod(conn, empRef.MemberID, payperiod_id, OpsPayroll.DaysOffType.Bereavement);
						if(bereavementTotal > 0 && !bereavement_paid.Contains(empRef.MemberID))
							{
							empRef.BereavementPay = bereavementTotal.ToString();
							bereavement_paid.Add(empRef.MemberID);
							}
						break;
						case OpsPayroll.PayrollExport.PayrollEntryType.STAT:
						if(!stat_paid.Contains(empRef.MemberID))
							{
							empRef.StatHoliday = dr["stat"].ToString();
							stat_paid.Add(empRef.MemberID);
							}
						break;
						}
					}

				foreach (var m in members)
					{
					var mv = m.Value;
					// 21 columns (0 to 20) in total
					sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21}\n",
						mv.NumberEmpID,                   //{0} Number                                         
						mv.Rate,                          //{1} Rate
						mv.RegHrs,                        //{2} Reg.Hr
						mv.OverTime,                      //{3} Overtime
						mv.DoubleTime,                    //{4} Double Time
						mv.PremiumRT,                     //{5} Premium RT
						mv.PremiumOT,                     //{6} Premium OT
						mv.PremiumDT,                     //{7} Premium DT
						mv.BankedHoursIn,                 //{8} Banked in
						mv.BankTimePaid,                  //{9} Bank time paid 
						mv.VacTaken,                      //{10} Vac taken
						mv.VacWithd,                      //{11} Vac Withdrawal
						mv.Reimbursement,                 //{12} Reimbursement
						mv.OnCall,                        //{13} On call
						mv.SickPay,                       //{14} Sick pay
						mv.SickOther,                     //{15}  Sick Other
						mv.Bonus,                         //{16} Bonus
						mv.SalesCommission,               //{17} Sales Commission 
						mv.PerDiem,                       //{18} Per Diem
						mv.BDay,                          //{19} Bday
						mv.BereavementPay,                //{20} Bereave
						mv.StatHoliday                    //{21} Stat Pay
						);
					}
				#endregion canada_payroll
				}
			}
		if(is_debug)
			{
			endTime = DateTime.Now;
			// Adding time tracking as this is a slow page.
			sb.AppendFormat("\nStart Time:{0}\nEnd Time: {1}", Toolbox.MySQL_longdt(startTime), Toolbox.MySQL_longdt(endTime));
			}
		resp.AddHeader("Content-Length", sb.Length.ToString());
		if (payperiod_id > 0)
			{
			try
				{
				var fileServer = Toolbox.GetRequiredAppSetting("UNC_base_path");
				var path = fileServer + @"/nesi_files/global/payperiods/P" + payperiod_id;
				if (!Directory.Exists(path))
					{
					Directory.CreateDirectory(path);
					}
				path = action == "wage"
														? path + "/" + file_name
														: path + "/" + DateTime.Now.ToString("MMdd_HH_mm_ss-") + file_name;
				using (var sw = new StreamWriter(path, true))
					{
					sw.WriteLine(sb.ToString());
					}
				}
			catch
				{
				throw new Exception("There is an issue saving the exported file... not proceeding");
				}
			}
		if (is_debug)
			{
			//resp.AddHeader("Content-Disposition", "attachment; filename=" + file_name);
			resp.ContentType = "Text/Plain";
			}
		else
			{
			resp.AddHeader("Content-Disposition", "attachment; filename=" + file_name);
			resp.ContentType = content_type;
			}
		resp.Write(sb.ToString());
		}

	private class PayrollRowsPayworks
		{
		private string _numberEmpId     = "";
		private string _regHrs          = "";
		private string _overTime        = "";
		private string _doubleTime      = "";
		private string _premiumRt       = "";
		private string _statHoliday     = "";
		private string _bereavementPay  = "";
		private string _bDay            = "";
		private string _perDiem         = "";
		private string _salesCommission = "";
		private string _bonus           = "";
		private string _sickOther       = "";
		private string _sickPay         = "";
		private string _onCall          = "";
		private string _reimbursement   = "";
		private string _vacWithd        = "";
		private string _vacTaken        = "";
		private string _bankTimePaid    = "";
		private string _bankedHoursIn   = "";
		private string _premiumDt       = "";
		private string _premiumOt       = "";

		public int MemberID { get; set; }

		public string NumberEmpID
			{
			get { return _numberEmpId == null ? "ERR_EMPNUMBER" : _numberEmpId; }
			set { _numberEmpId = value; }
			}

		public double Rate { get; set; } = 0D;

		public string RegHrs
			{
			get { return _regHrs == null ? "ERR_RT" : _regHrs; }
			set { _regHrs = value; }
			}

		public string OverTime
			{
			get { return _overTime == null ? "ERR_OT" : _overTime; }
			set { _overTime = value; }
			}

		public string DoubleTime
			{
			get { return _doubleTime == null ? "ERR_DT" : _doubleTime; }
			set { _doubleTime = value; }
			}

		public string PremiumRT
			{
			get { return _premiumRt == null ? "ERR_PREMRT" : _premiumRt; }
			set { _premiumRt = value; }
			}

		public string PremiumOT
			{
			get { return _premiumOt == null ? "ERR_PREMOT" : _premiumOt; }
			set { _premiumOt = value; }
			}

		public string PremiumDT
			{
			get { return _premiumDt == null ? "ERR_PREMDT" : _premiumDt; }
			set { _premiumDt = value; }
			}

		public string BankedHoursIn
			{
			get { return _bankedHoursIn == null ? "ERR_BANKED" : _bankedHoursIn; }
			set { _bankedHoursIn = value; }
			}

		public string BankTimePaid
			{
			get { return _bankTimePaid == null ? "ERR_BANKPAID" : _bankTimePaid; }
			set { _bankTimePaid = value; }
			}

		public string VacTaken
			{
			get { return _vacTaken == null ? "ERR_VACTAKEN" : _vacTaken; }
			set { _vacTaken = value; }
			}

		public string VacWithd
			{
			get { return _vacWithd == null ? "ERR_VACWITH" : _vacWithd; }
			set { _vacWithd = value; }
			}

		public string Reimbursement
			{
			get { return _reimbursement == null ? "ERR_REIMBURSEMENT" : _reimbursement; }
			set { _reimbursement = value; }
			}

		public string OnCall
			{
			get { return _onCall == null ? "ERR_ONCALL" : _onCall; }
			set { _onCall = value; }
			}

		public string SickPay
			{
			get { return _sickPay == null ? "ERR_SICKPAY" : _sickPay; }
			set { _sickPay = value; }
			}

		public string SickOther
			{
			get { return _sickOther == null ? "ERR_SICKOTHER" : _sickOther; }
			set { _sickOther = value; }
			}

		public string Bonus
			{
			get { return _bonus == null ? "ERR_BONUS" : _bonus; }
			set { _bonus = value; }
			}

		public string SalesCommission
			{
			get { return _salesCommission == null ? "ERR_COMM" : _salesCommission; }
			set { _salesCommission = value; }
			}

		public string PerDiem
			{
			get { return _perDiem == null ? "ERR_PERDIEM" : _perDiem; }
			set { _perDiem = value; }
			}

		public string BDay
			{
			get { return _bDay == null ? "ERR_BDAY" : _bDay; }
			set { _bDay = value; }
			}

		public string BereavementPay
			{
			get { return _bereavementPay == null ? "ERR_BEREAVE" : _bereavementPay; }
			set { _bereavementPay = value; }
			}

		public string StatHoliday
			{
			get { return _statHoliday == null ? "ERR_STAT" : _statHoliday; }
			set { _statHoliday = value; }
			}
		}

	public bool IsReusable
		{
		get
			{
			return false;
			}
		}

	}