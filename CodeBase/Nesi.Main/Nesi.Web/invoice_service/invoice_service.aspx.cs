using System;
using System.Data;
using System.Web.UI;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using nesi.core;
using DevExpress.XtraPrinting.Drawing;
using System.Net.Http.Headers;
using System.Net.Http;


public partial class invoice_service_root_page : Page
{
    private NeMember myMember;
    private const int _page_id = 114; // from Page table in DB
    DateTime start, end;
    bool _in_debug = false;
    bool should_run = false;
    public string _error_step = "";
    private const string URL = "http://www.apilayer.net/api/live";
    private const string urlParameters = "?access_key=b0e416336b53d92d530cbda37536a83d&source=USD&currencies=CAD,EUR&format=1";
	private NameValueCollection q;
	private string ControlPropertyName = "task_scheduler_running";


	protected void Page_Load(object sender, EventArgs e)
    {
	q = Request.QueryString;
	var task = q["task"];
	var qtoken = q["token"];
	var clearToken = q["clearToken"];
	var accessKey = q["access_key"];
	if(string.IsNullOrEmpty(accessKey) || accessKey != access_key.Value)
		{
		Toolbox.FriendlyException(Response, "Not authenticated", "/index.html");
		}
	var isTask = !string.IsNullOrEmpty(task);
	if(shared.properties.exists(ControlPropertyName) && string.IsNullOrEmpty(clearToken))
		{
		var property = new shared.properties(ControlPropertyName);
		if(property.value != qtoken)
			{
			if(property.expires < DateTime.Now)
				{
				CreateNewToken();
				}
			else
				{
				if(!isTask)
					{ 
					Response.Clear();
					Response.Write("Tasks are running elsewhere.<br/><a href='./invoice_service.aspx?clearToken=true&access_key="+accessKey+"'>Force clear</a>");
					Response.End();
					}
				else
					{
					Toolbox.QuickReponse(Response, "DUPLICATE_INSTANCE");
					}
				}
			}
		}
	else if(!string.IsNullOrEmpty(clearToken) && clearToken == "true")
		{
		shared.properties.clean(ControlPropertyName);
		Response.Redirect("./invoice_service.aspx?access_key="+accessKey);
		}
	else
		{ 
		if (!isTask)
			{
			CreateNewToken();
			}
		}
	if (!isTask) return;
	Response.Clear();
	var i = 0;
	switch(task)
		{
		case "move_invoices":
			try
				{ 
				move_invoices(ref i);
				}
			catch(Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				Toolbox.QuickReponse(Response, ee.ToString());
				}
		break;
		case "check_open_payrolls":
		/*	try
				{
				mockInvoices();
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				Toolbox.QuickReponse(Response, ee.ToString());
				}*/
		break;
		case "update_offers":
			try
				{
				update_offers(ref i);
			    }
		    catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				Toolbox.QuickReponse(Response, ee.ToString());
				}
		break;
		case "clean_properties":
			try
				{
				shared.properties.clean(ref i);
				}
		    catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				Toolbox.QuickReponse(Response, ee.ToString());
				}
		break;
		case "customer_rates_warnings":
		/*	try
				{
				var dt = Toolbox.doSQL_dt(@"SELECT id FROM business_unit WHERE istest = 'F' AND active = 'T' ", null);
				foreach (DataRow dr in dt.Rows)
					{
					var business_unit_id = Convert.ToInt32(dr["id"]);
					var bu = new NeBusinessUnit(business_unit_id);
					if (!bu.Active) continue;
					var sent = false;
					send_customer_rates_warning(business_unit_id, ref sent);
					if(!sent) continue;
					i++;
					}
				}
		    catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				Toolbox.QuickReponse(Response, ee.ToString());
				}*/
		break;
		case "send_termination_reminders":
		/*	try
				{
				send_termination_checklists_reminders(ref i);
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				Toolbox.QuickReponse(Response, ee.ToString());
				}*/
		break;
		case "missing_csps":
			try
				{
				fix_missing_csps(ref i);
			    }
		    catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				Toolbox.QuickReponse(Response, ee.ToString());
				}
		break;
		case "ts_watch":
		/*	try
				{
				send_ts_watch_reports(ref i);
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				Toolbox.QuickReponse(Response, ee.ToString());
				}*/
		break;
            case "inactivate_closedpo_lines":
                try
                {
                    inactivate_closedpo_lines(ref i);
                }
                catch (Exception ee)
                {
                    Toolbox.do_errorLog_errorStack(ee);
                    Toolbox.QuickReponse(Response, ee.ToString());
                }
                break;
            default:
			Toolbox.QuickReponse(Response, "Unsupported Type");
		break;
		}
	Toolbox.QuickReponse(Response, "SUCCESS|"+i);
	}

	private void CreateNewToken()
		{
		token.Value = Toolbox.do_RandomString(25);
		var p = new shared.properties
				{
				name = ControlPropertyName,
				expires = DateTime.Today.AddDays(1),
				value = token.Value
				};
		p.save();
		}
	private void mockInvoices()
		{
		foreach(var f in Directory.GetFiles(@"D:\Nesi_data\nesi_files\TE\TE2\wos\Invoiced\"))
			{
			if(f.EndsWith("."))
				{
				continue;
				}
			File.Delete(f);
			}
		var dt = Toolbox.doSQL_dt("SELECT woprog_Id id FROM woprog WHERE business_unit_id = 1 AND woprog_status = 'invoiced' ORDER BY woprog_id DESC LIMIT 50", new object[]{});
		foreach(DataRow dr in dt.Rows)
			{
			var id = dr["id"].ToString();
			var path = @"D:\Nesi_data\nesi_files\TE\TE2\wos\" + id + ".pdf";
			if(!File.Exists(path))
				{ 
				File.Copy(@"C:\temp\test.pdf", path);
				}
			}
		}
	private void fix_missing_csps(ref int i)
		{
		Toolbox.doSQL_void(@"
		INSERT INTO neintranet.customer_sales_properties(
			customer_id,
			address_id,
			status_id
			)
		SELECT
			customer_id,
			b.address_id a_id,
			customer_status
		FROM
			customer a
			LEFT JOIN
			address b ON b.address_table = 'customer' AND b.address_table_id = a.customer_id AND b.Address_Type = 'B' AND b.Active
			WHERE
		b.address_id NOT IN(SELECT address_id FROM customer_sales_properties)", new object[]{});
		}
    private void runTasks()
        {

        // Rest of Invoice Service
        try
        {
            var this_hour = DateTime.Now.Hour;
            var this_minute = DateTime.Now.Minute;

            if (IsPostBack)
            {
                start = DateTime.Now;
                // Runs at first startup in the morning only once
                if (!_in_debug && this_hour > 6)
                {
                        run_auto_reports();
                        update_currency();

                        if (this_minute < 20 && this_hour < 8 && DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
                        {
                            try
                            {
                                send_waiting_cust_po_emails();
                                sendProbationEmail();
                            }
                            catch (Exception ee)
                            {
                                sendIsError(ee);
                            }
                        }
                        if (this_minute < 20 && this_hour < 8 && DateTime.Today.DayOfWeek != DayOfWeek.Monday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
                        {
                            send_daily_collection_notes(1148, 0, 1);
                            send_daily_collection_notes(8, 0, 1);
                            //send_ts_watch_reports();
                        }
                        // if (DateTime.Today.DayOfWeek == DayOfWeek.Friday && this_hour < 8)
                        // {
                        //     if (Toolbox.doSQL_int(@"select count(id) from invoice_service_email_report_history  where dt > curdate() - interval 5 day and report_run = 'customer_account_statements'", null) == 0)
                        //     {
                        //         send_customer_statements();
                        //         // send_closed_ticket_report("#4682B4", "#EBF0FA", new string[] { "1", "30", "31" }, "Nesi.ca updates in the past week");
                        //         //send_closed_ticket_report("#FFE066", "#FFF5CC", new string[] { "2" }, "IT Updates in the past week");
                        //         send_wohistory();
                        //     }
                        // }
                        get_currencies();
                }
                // Runs Every Scan 
                // && ((this_hour == 22 && this_minute < 30) || (this_hour == 6 && this_minute < 30))

                shared.properties.clean();
                //check_open_payrolls();

                //#region timesheet reminders
                //if (DateTime.Today > new DateTime(2014, 6, 30) && DateTime.Now.Hour == 17 && !_in_debug)
                //    {
                //	if (Convert.ToString(Session["run_ts_reminders"]) == "1")
                //	{
                //		if (DateTime.Today.DayOfWeek != DayOfWeek.Monday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
                //		{
                //			send_timesheet_reminders();
                //			Session["run_ts_reminders"] = "0";
                //		}
                //	}
                //}
                //#endregion

                if (!_in_debug)
                {
                   // send_termination_checklists_reminders();
                    task_alert_payroll_handlers();
                }
                // Runs between 6 and 11PM
                //bool should_run = false;
                #region wo_adjust_committed_required
                var dt_wo_adjust_committed_required_start = DateTime.Now;
                if (should_run)
                {
                    try
                    {
                        wo_adjust_committed_required();
                    }
                    catch (Exception ee)
                    {
                        sendIsError(ee);
                    }
                }
                var dt_wo_adjust_committed_required_end = DateTime.Now;
                var span_wo_adjust_committed_required = dt_wo_adjust_committed_required_end.Subtract(dt_wo_adjust_committed_required_start);
                #endregion wo_adjust_committed_required
                #region move_cellphone_db
                var dt_move_cellphone_db_start = DateTime.Now;
                if (should_run)
                {
                    try
                    {
                        move_cellphone_db();
                    }
                    catch (Exception ee)
                    {
                        sendIsError(ee);
                    }
                }
                var dt_move_cellphone_db_end = DateTime.Now;
                var span_move_cellphone_db = dt_move_cellphone_db_end.Subtract(dt_move_cellphone_db_start);
                #endregion move_cellphone_db

            }
        }
        catch (Exception ee)
        {
            sendIsError(ee);
        }
    }
    public void sendIsError(Exception ee)
    {
        sendIsError(ee, "Invoice Service Error");
    }
    private void sendIsError(Exception ee, string subject)
    {
	    var em = new NeEMail
		             {
		             To = "iserror@" + Toolbox.app_setting("DomainForEmail"),
		             Subject = "IS error: " + subject,
		             Body = ee.ToString(),
		             From = "noreply@" + Toolbox.app_setting("DomainForEmail")
		             };
	    em.Send();
        Toolbox.do_errorLog_errorStack(ee);
    }

    /// <summary>
    /// Alerts all the payroll handlers of the upcoming payroll
    /// </summary>
    private void task_alert_payroll_handlers()
    {
        return; // This was pre-consol, make sure to fully audit this before releasing.
        try
        {
            if (DateTime.Today.DayOfWeek != DayOfWeek.Monday || DateTime.Now.Hour < 6 || DateTime.Now.Hour > 7) return;
            var payperiod_id = nesi.core.payroll.current_pay_period();
            var payperiod = new NePayPeriod(payperiod_id);
            var current_date = DateTime.Now;
            var days_till_payperiod_end = payperiod.end_date.Subtract(current_date);
            const int days_max = 1;
            var employee_list = new List<int>();
            if (days_till_payperiod_end.TotalDays > 0 && days_till_payperiod_end.TotalDays < days_max && payperiod.end_date > current_date)
            {
                var payroll_handlers = Toolbox.doSQL_dt(@"SELECT b.member_id id, b.member_nickname name, b.member_neemail email FROM memberpage a INNER JOIN member b ON a.memberpage_member_id = b.member_id  where a.memberpage_page_id = 44 order by id", null);
                foreach (DataRow dr_a in payroll_handlers.Rows)
                {
                    var handler_id = (int)dr_a["id"];
                    if (shared.properties.exists(string.Format("payroll_notification_{0}_{1}", payperiod_id, handler_id))) continue;
                    var handler_name = (string)dr_a["name"];
                    var handler_email = (string)dr_a["email"];
                    var direct_payroll_approval = Toolbox.doSQL_dt(@" SELECT GROUP_CONCAT(a.member_id) ids, b.name FROM member a INNER JOIN business_unit b ON a.business_unit_id = b.id AND b.istest = 'F' WHERE (a.member_status = 'Active' OR member_termdate BETWEEN DATE_SUB(NOW(), INTERVAL 30 DAY) AND NOW()) AND a.payroll_handler = @v0  GROUP BY b.name ORDER BY b.name", new object[] { handler_id });
                    var on_vacation_approval = Toolbox.doSQL_dt(@" SELECT id, name branch_name, REPORTS_TO_SANS_STATUS(@v0 ,id, @v1 , @v2 , 2) ids FROM business_unit WHERE istest = 'F' GROUP BY id HAVING ids != ''", new object[] { handler_id, Toolbox.MySQL_longdt(payperiod.start_date), Toolbox.MySQL_longdt(payperiod.end_date) });
                    if (direct_payroll_approval.Rows.Count <= 0 && on_vacation_approval.Rows.Count <= 0) continue;
                    var sb = new StringBuilder();
                    sb.AppendFormat(@"
<div style='font-family:sans;font-size:14px;width:600px;'>
{0}, <br/>
This is a notification that you will be able to approve payroll for employees in the following branch(es) starting {1:MMMM d, yyy}:
<ul>", handler_name, payperiod.end_date.AddDays(1));
                    foreach (DataRow dr_b in direct_payroll_approval.Rows)
                    {
                        var ids = (string)dr_b["ids"];
                        var list_ids = ids.Split(',').Select(int.Parse).ToList();
                        employee_list.AddRange(list_ids);
                        var branch_name = (string)dr_b["name"];
                        sb.AppendFormat(@"
	<li>{0}", branch_name);
                    }
                    sb.Append(@"
</ul>");
                    if (on_vacation_approval.Rows.Count > 0)
                    {
                        var include_additional = false;
                        const string additional_header = @"
<br/>
<div style='color:#f30;'>
Additionally, these employee(s) (whom report to you) will be on vacation during payroll approval, and you may need to handle payroll approval for their employees:
<ul>";
                        foreach (DataRow dr_c in on_vacation_approval.Rows)
                        {
                            var ids = (string)dr_c["ids"];
                            var branch_name = (string)dr_c["branch_name"];
                            var list_ids = ids.Split(',').Select(int.Parse).ToList();
                            foreach (var id in list_ids)
                            {
                                if (employee_list.Contains(id))
                                {
                                    list_ids.Remove(id);
                                }
                            }
                            if (list_ids.Count <= 0) continue;
                            if (!include_additional)
                            {
                                sb.Append(additional_header);
                                include_additional = true;
                            }
                            var employees = Toolbox.doSQL_dt(string.Format(@"SELECT b.member_fullname handler FROM member a LEFT JOIN member b 
ON a.payroll_handler = b.member_id WHERE a.member_id IN ({0}) GROUP BY handler", string.Join(",", list_ids)), null);
                            sb.AppendFormat(@"
	<li>{0}
	<ul>", branch_name);
                            foreach (DataRow dr_d in employees.Rows)
                            {
                                sb.AppendFormat("<li>{0}", dr_d["handler"]);
                            }
                            sb.Append(@"
	</ul>");
                        }
                        if (include_additional)
                        {
                            sb.Append(@"
		</ul>");
                        }
                        sb.Append(@"
	</ul>
	</div>
	</div>");
                    }
                    else
                    {
                        sb.Append(@"
	<br/> 
	Of the employees that report directly to you, none will be on vacation during payroll approval. <br/> 
	If there were, this notification would also include who is on vacation, and which branch they handle that you may need to approve payroll for.
	</div>");
                    }
                    // Send the handler an email
                    var em = new NeEMail
                    {
                        To = handler_email,
                        From = "noreply@" + Toolbox.app_setting("DomainForEmail"),
                        Subject = "Upcoming payroll notification",
                        Bcc = "mhyde@" + Toolbox.app_setting("DomainForEmail"),
                        isHTML = true,
                        Body = sb.ToString()
                    };
                    em.Send();
                    // So we don't double up on the emails
                    var p = new shared.properties
                    {
                        name = string.Format("payroll_notification_{0}_{1}", payperiod_id, handler_id),
                        expires = current_date.AddDays(3),
                        value = "1"
                    };
                    p.save();
                }
            }
        }
        catch (Exception ee)
        {
            // shared.alert_debug("Invoice services crash - Payroll notification", "This is the error:<br/>" + ee);
            sendIsError(ee, "Payroll notification");
        }
    }
    private void run_auto_reports()
    {
        try
        {
            var reports = Toolbox.doSQL_dt(@"Select * from auto_reports_schedule", null);
            foreach (DataRow dr_reports in reports.Rows)
            {
                #region.. first delete any auto reports if permissions are not set

                if (Toolbox.doSQL_int(@"
SELECT
count(auto_reports.id)
FROM
auto_reports
LEFT JOIN memberpageprivilege ON auto_reports.privilege_id = memberpageprivilege.MemberPagePrivilege_Privilege_ID
left JOIN privilege ON memberpageprivilege.MemberPagePrivilege_Privilege_ID = privilege.Privilege_ID
LEFT JOIN memberpage ON auto_reports.page_id = memberpage.MemberPage_Page_ID
left JOIN page ON page.page_id = memberpage.MemberPage_Page_ID
LEFT JOIN auto_reports_schedule ON auto_reports.id = auto_reports_schedule.auto_reports_id AND auto_reports_schedule.member_id = @v0" +
                                      @"
WHERE
auto_reports.id = @v1" + @" AND
(( memberpageprivilege.MemberPagePrivilege_ID <> 0 AND
Privilege_Enabled = 1 and
memberpageprivilege.MemberPagePrivilege_Member_ID = @v0" + @")
 OR
( memberpage.MemberPage_ID <> 0 AND
page_enabled = 1 and 
memberpage.MemberPage_Member_ID = @v0" + @")) 
", new object[]
                    {
                    dr_reports["member_id"], dr_reports["auto_reports_id"]

                    }) == 0)
                {
                    Toolbox.doSQL_void(@"delete from auto_reports_schedule  where auto_reports_id =@v0 and member_id =@v1 ",
                        new object[] { dr_reports["auto_reports_id"], dr_reports["member_id"] });
                }
            }

            #endregion
            reports = Toolbox.doSQL_dt(@"Select * from auto_reports_schedule", null);
            foreach (DataRow dr_reports in reports.Rows)
            {
                if (Convert.ToInt32(dr_reports["interval"]) != 0 && Toolbox.doSQL_int(@"Select ifnull((Select datediff(curdate(),dt) from auto_report_history  where autoreport_schedule_id =@v0 order by dt desc limit 1),99)", new object[] { dr_reports["id"] }) >= Convert.ToInt32(dr_reports["interval"]))
                {
                    switch (dr_reports["auto_reports_id"].ToString())
                    {
                        case "1":
                            #region income statements
                            //send_income_statement_report(Convert.ToInt32(dr_reports["member_id"]), Convert.ToInt32(dr_reports["id"]), Convert.ToInt32(dr_reports["business_unit_id"]));
                            #endregion
                            break;
                        case "6":
                            #region WO Line Changes
                            #endregion
                            break;
                        case "7":
                            #region trial balances
                            #endregion
                            break;
                        case "2":
                            #region Cash Reports
                            //send_cash_report(Convert.ToInt32(dr_reports["member_id"]), Convert.ToInt32(dr_reports["id"]));
                            #endregion
                            break;
                        case "8":
                            #region WO History
                            #endregion
                            break;
                        case "9":
                            #region Tickets Closed
                            //	send_closed_ticket_report();
                            #endregion
                            break;
                        case "10":
                            #region Account Balances
                            #endregion
                            break;
                        case "11":
                            #region PO Daily Email
                            send_daily_po_report(Convert.ToInt32(dr_reports["member_id"]), Convert.ToInt32(dr_reports["id"]));
                            #endregion
                            break;
                        case "12":
                            #region Collections email
                            #endregion
                            break;
                        case "13":
                            #region Inventory Report
                            #endregion
                            break;
                        case "3":
                            #region Daily Collection Notes
                            send_daily_collection_notes(Convert.ToInt32(dr_reports["member_id"]), Convert.ToInt32(dr_reports["id"]), Convert.ToInt32(dr_reports["interval"]));
                            #endregion
                            break;
                        case "15":
                            #region Payroll Approval Report
                            #endregion
                            break;
                    }
                }
            }
        }
        catch (Exception ee)
        {
            //email_debug("Auto Reports Crash : " + ee.Message + System.Environment.NewLine + ee.InnerException + " " + ee.StackTrace);
            sendIsError(ee, "Auto Reports Crash");
        }
    }
    void check_open_payrolls()
    {
        var prop_name = "IS_open_payroll_daily_email_has_sent";
        if (!shared.properties.exists(prop_name))
        {
	        var check = new shared.properties
		                    {
		                    name = prop_name,
		                    expires = DateTime.Now.Date.AddDays(1)
		                    };
	        var current_payperiod_id = Toolbox.doSQL_int(@"CALL _payperiod()") - 1;
            var p = new NePayPeriod((int)current_payperiod_id);
            if (!p.completed)
            {
                shared.alert_payroll("NOTICE: Previous pay period is open", string.Format("The previous pay period {0} ({1} - {2}), is still open and will keep payroll from being processed.<br/><b>Please fix!</b>", p.payperiodID, p.StartDate, p.Enddate));
                check.value = "sent";
                check.save();
            }
        }
    }
    private void check_PO_APproblems()
    {
        var dt = Toolbox.doSQL_dt(@"SELECT c.poprog_id, MAX(a.poprogstatus_datetime) dt, c.ap_problem_last_notice_sent FROM poprogstatus a LEFT JOIN poprog_status b ON a.poprogstatus_status = b.status_type LEFT JOIN poprog_header c ON a.poprogstatus_poprog_id = c.poprog_id  WHERE c.poprog_status = 10 GROUP BY c.poprog_id", null);
        foreach (DataRow dr in dt.Rows)
        {
            var poprog_id = Convert.ToInt32(dr["poprog_id"]);
            var po = new NePOProg(poprog_id);
            var date_sent = Convert.ToDateTime(dr["dt"]);
            var last_notice_sent = dr["ap_problem_last_notice_sent"] == DBNull.Value
                                            ? new DateTime()
                                            : Convert.ToDateTime(dr["ap_problem_last_notice_sent"]);
            if (DateTime.Now.Subtract(last_notice_sent).Days > 7)
            {
                NeBusinessUnit.EmailBranchPurchaser(po.business_unit_id, string.Format("Reminder: PO #{0} was sent to AP Problems on {1} and hasn't been resolved", po.poprog_bvpo, Toolbox.MySQL_shortdt(date_sent)), string.Format("<b>This PO still needs your immediate attention!</b> <br /><a href='" + Toolbox.app_setting("Domain") + "/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}'>Load PO</a>", po.poprog_id), true);
                Toolbox.doSQL_void(@"UPDATE poprog_header SET ap_problem_last_notice_sent = CURDATE() WHERE poprog_id = @v0  LIMIT 1", new object[] { poprog_id });
            }
        }
    }
    private string email_css()
    {
        var report_css = "";
        var css_path = Server.MapPath("~/css/via_handler/1ER - email_report.css");
        using (var r = new StreamReader(css_path))
        {
            report_css = r.ReadToEnd();
        }
        return report_css;
    }
    private void send_daily_po_report(int mem_id, int auto_report_schedule_id)
    {
        var branch = Toolbox.doSQL_int(@"Select business_unit_id from auto_reports_schedule  where id =@v0", new object[] { auto_report_schedule_id });
        var mem = new NeMember(mem_id);
        var comp = new NeBusinessUnit(branch);
        var dt = Toolbox.doSQL_dt(@"SELECT poprog_header.poprog_bvpo AS PO, vendor.Vendor_Name AS Vendor, poprog_status.status_type AS `Status`, poprog_header.poprog_order_description AS Description, poprog_header.poprog_total_cost AS Total FROM poprog_header INNER JOIN vendor ON poprog_header.poprog_vendor_id = vendor.Vendor_ID INNER JOIN poprog_status ON poprog_header.poprog_status = poprog_status.poprog_status_id  WHERE poprog_header.poprog_cutdate > curdate()-interval 1 week and poprog_header.business_unit_id =@v0", new object[] { branch });
        if (dt.Rows.Count > 0)
        {
            var s = get_email_body_header("Purchase Order Activity for " + comp.name, dt, true, comp);
	        var email = new NeEMail
		                    {
		                    To = mem.NEEmail,
		                    From = "admin@" + Toolbox.app_setting("DomainForEmail"),
		                    Subject = "Purchase Order Activity - Week of " + DateTime.Today.ToString("yyyy-MM-dd") +
		                              "  for " + comp.name,
		                    Body = s,
		                    isHTML = true
		                    };
	        email.Send();
        }
        if (auto_report_schedule_id != 0)
        {
            Toolbox.doSQL_void(@"insert into auto_report_history (dt,autoreport_schedule_id)  values(now(),@v0)", new object[] { auto_report_schedule_id });
        }
    }
    private void send_wohistory()
    {
        return;
        var strtemp = "";
        var dt_wohistory = Toolbox.doSQL_dt(@"Call msaccess_dailyservices_wo_lag", null);
        var dv = new DataView(dt_wohistory);
        dt_wohistory = dv.ToTable(true, "business_unit", "avg_wo_scan_lag", "avg_branch_lag", "avg_office_lag", "number_of_wos");
        var s = get_email_body_header("Work Order Processing Stats", dt_wohistory, false, new NeBusinessUnit(11));
	    var email = new NeEMail
		                {
		                To = "branchmanagers@" + Toolbox.app_setting("DomainForEmail"),
		                From = "admin@" + Toolbox.app_setting("DomainForEmail"),
		                Subject = "Work Order Processing Stats - Week of " + DateTime.Today.ToString("yyyy-MM-dd"),
		                Body = s,
		                isHTML = true
		                };

	    email.Send();
    }
    protected string get_email_body_header(string report_name, DataTable dt, bool auto_format, NeBusinessUnit bu)
    {
        var strtemp = "";
        var holiday_title = "";
        if (DateTime.Today > new DateTime(DateTime.Today.Year, 12, 22) && DateTime.Today < new DateTime(DateTime.Today.Year, 12, 26))
        {
            holiday_title = "</br>Merry Christmas!";
        }
        else if (DateTime.Today > new DateTime(DateTime.Today.Year, 12, 30) && DateTime.Today < new DateTime(DateTime.Today.Year, 1, 2))
        {
            holiday_title = "</br>Happy New Year!";
        }
        strtemp = @"<html><body><span style='font-family: Calibri'>" + holiday_title + @"<br><br><div style='Position: relative; left: 13; top: 5; width: 895; '><font size='1' >Report Time:" + DateTime.Now + @"<br><br><font size='3'>" + report_name + @"<br></div><br><p></p><TABLE cellspacing='0' cellpadding='10' width='1100'><tr>";
        foreach (DataColumn s in dt.Columns)
        {
            strtemp += @"<td align='center' cellpadding='10' style='color: #FFFFFF; vertical-align: top; font-family: Calibri; font-size: 9pt' bgcolor='" + bu.header_color + @"'><b>" + s.ColumnName + "</b></td>";
        }
        strtemp += @"</tr>";
        foreach (DataRow row in dt.Rows)
        {
            strtemp += @"<tr>";
            foreach (var ss in row.ItemArray)
            {


                if ((ss is double || ss is decimal) && auto_format)
                {
                    strtemp += @"<TD  align='right' style='color: #000000; vertical-align: top;font-family: Calibri; font-size: 9pt'>" + (ss == DBNull.Value ? "" : Convert.ToDouble(ss).ToString("c2")) + "</TD>";
                }
                else if (ss is double || ss is decimal)
                {
                    strtemp += @"<TD  align='right' style='color: #000000; vertical-align: top;font-family: Calibri; font-size: 9pt'>" + (ss == DBNull.Value ? "" : Convert.ToDouble(ss).ToString("n2")) + "</TD>";
                }
                else if (ss is int && auto_format)
                {
                    strtemp += @"<TD  align='left' style='color: #000000; vertical-align: top;font-family: Calibri; font-size: 9pt'>" + (ss == DBNull.Value ? "" : ss.ToString()) + "</TD>";
                }

                else if (ss is DateTime)
                {
                    if (ss == DBNull.Value)
                    {
                        strtemp += @"<TD  align='center' style='color: #000000; vertical-align: top;font-family: Calibri; font-size: 9pt'><B></B></TD>";
                    }
                    else
                    {
                        strtemp += @"<TD  align='center' style='color: #000000; vertical-align: top;font-family: Calibri; font-size: 9pt'><B>" + (Convert.ToDateTime(ss).ToString("HHmmss") == "000000" ? Convert.ToDateTime(ss).ToString("yyyy-MM-dd") : Convert.ToDateTime(ss).ToString("yyyy-MM-dd HH:mm:ss")) + "</B></TD>";
                    }

                }
                else
                {
                    if (ss != DBNull.Value)
                    {
                        if (ss.ToString().Length > 200)
                        {
                            strtemp += @"<TD nowrap='nowrap' align='left' style='color: #000000; vertical-align: top;font-family: Calibri; font-size: 9pt'>" + ss + "</TD>";
                        }
                        else
                        {
                            strtemp += @"<TD  align='left' style='color: #000000; vertical-align: top;font-family: Calibri; font-size: 9pt'>" + ss + "</TD>";
                        }
                    }
                    else
                    {
                        strtemp += @"<TD  align='left' style='color: #000000; vertical-align: top;font-family: Calibri; font-size: 9pt'><B></B></TD>";
                    }
                }
            }
            strtemp += @"</tr>";
        }
        strtemp += @"</table></font></span></body></html>";
        return strtemp;
    }

    protected void send_daily_collection_notes(int member_id, int auto_report_schedule_id, int interval)
    {
        //		grab companies and departments
        //		grab companies and departments
        var dt_members = Toolbox.doSQL_dt(@"Select member_id,member_membertype_id from member  where member_membertype_id in (4,5,11,17,25,29,39,47,45,52,53,56,66,67,75) and member_status='Active' and member_id=@v0", new object[] { member_id });
        var dt_notes = Toolbox.doSQL_dt(@"
			SELECT 
woprog.woprog_bvwo wo,
name,
woprog.woprog_customername AS cust_name,
ar_notes.ar_notes_ts AS note_date,
ar_notes.ar_notes_note AS notes,
member.member_fullname AS notes_by,
if(csp.project_mgr_member_id=0,woprog.woprog_pm_memberid,csp.project_mgr_member_id) pm,
(select ifnull((select member_id from member where business_unit_id = business_unit_id and member_membertype_id = 5 and member_status='Active' limit 1),(select reports_to from member where business_unit_id = business_unit_id and member_membertype_id = 5  limit 1))) bm,
csp.account_manager am,
business_unit.ID cid,
woprog.woprog_department did,
ar_notes.ar_notes_sent_to_bm,
woprog.woprog_invoicebalance balance,
(select member_id from member where member.business_unit_id = woprog.business_unit_id  and member.member_membertype_id = 11 and Member_Status = 'Active' limit 1) dm,
(select member_id from member where member.business_unit_id = am_member.business_unit_id and member.member_membertype_id = 5 and Member_Status = 'Active' limit 1) am_bm,
(select member_id from member where member.business_unit_id = pm_member.business_unit_id and member.member_membertype_id = 5 and Member_Status = 'Active' limit 1) pm_bm,
(select reports_to from member where member.business_unit_id = am_member.business_unit_id and member.member_membertype_id = 5 and Member_Status = 'Active' limit 1) am_bm_reports_to,
(select reports_to from member where member.business_unit_id = pm_member.business_unit_id and member.member_membertype_id = 5 and Member_Status = 'Active' limit 1) pm_bm_reports_to
FROM
business_unit
INNER JOIN woprog ON woprog.business_unit_id = business_unit.id
INNER JOIN ar_notes ON ar_notes.ar_notes_woprogid = woprog.WOProg_ID
INNER JOIN member ON ar_notes.ar_notes_memberid = member.Member_ID
LEFT JOIN customer_sales_properties csp ON woprog.WOProg_Address_ID = csp.address_id
left join member am_member on csp.account_manager = am_member.member_id and am_member.Member_Status = 'Active' 
left join member pm_member on csp.project_mgr_member_id = pm_member.member_id and pm_member.Member_Status = 'Active' 
WHERE  ar_notes_ts>=curdate()-interval " + interval + @" day and ar_notes.ar_notes_sent_to_bm = 0 and business_unit.IsTest = 'F'
order by name,wo,note_date ", null);
        if (dt_notes.Rows.Count > 0)
        {
            foreach (DataRow dr_member in dt_members.Rows)
            {
                var selectedColumns = new[] { "name", "cust_name", "wo", "notes", "notes_by", "balance" };
                var filter_expression = "(am=" + dr_member[0] + " or pm=" + dr_member[0] + " or bm=" + dr_member[0] + " or dm=" + dr_member[0] + " or am_bm=" + dr_member[0] + " or pm_bm=" + dr_member[0] + " or am_bm_reports_to=" + dr_member[0] + " or pm_bm_reports_to=" + dr_member[0] + " or " + dr_member[1] + " in (74,73,67,47,45,36,35,30))";
                //		string filter_expression = "(bm=" + dr_member[0] + ")";
                if (dt_notes.Select(filter_expression).Length > 0)
                {
                    var m = new NeMember(Convert.ToInt32(dr_member[0]));
                    if (m.NEEmail != "")
                    {
                        var dt_notes_for_member = dt_notes.Select(filter_expression).CopyToDataTable();
                        var dt = new DataView(dt_notes_for_member).ToTable(false, selectedColumns);
                        var email = new NeEMail();
                        email.isHTML = true;
                        email.Subject = "Collection Notes Report For Day Of: " + DateTime.Today.Date.ToString("yyyy-MM-dd");
                        email.To = m.NEEmail;
                        //email.To = "aketelaars@newelectric.com";
                        email.Body = new NeMember(Convert.ToInt32(dr_member[0])).FullName + get_email_body_header("Collection Notes Report", dt, true, m.business_unit);
                        email.Send();
                    }
                }
            }
        }
        if (auto_report_schedule_id != 0)
        {
            Toolbox.doSQL_void(@"insert into auto_report_history (dt,autoreport_schedule_id)  values(now(),@v0)", new object[] { auto_report_schedule_id });
        }
        //		Toolbox.doSQL_void(@"update ar_notes set ar_notes_sent_to_bm=1  where ar_notes_ts>=curdate()-interval 1 day" , null);
    }
    protected void send_employee_status_change_emails_for_the_week()  // this checks to see if the person has transitioned past their probation and benefits start date.
    {
        /*		DataTable probation_emps = Toolbox.doSQL_dt(@"Select member_id from member  where member_hrstatus_id = 1 and business_unit_id !=8 and member_ order by member_startdate" , null);
				if (this_employee.Status == "Active" && this_employee.hrstatus_id == 6 && (DateTime.Now - this_start_date).TotalDays >= 90)
				{
					this_employee.hrstatus_id = 3;
					this_employee.save();
					NeEMail em = new NeEMail();
					em.Subject = "(" + myMember.FullName + ") has moved (" + this_employee.FullName + ") to the HR status of 'Approved'";
					em.To = this_employee.business_unit.branch_manager.NEEmail;
					if (em.To == "")
					{
						em.To = "hr@thatsnew.com";
						em.Subject += " - No Branch Manager Available";
					}
					else
					{
						em.CC = "hr@thatsnew.com";
					}
					em.From = "hr@thatsnew.com";
					em.Body = "This is an automatic procedure. Once an employee has been with the company for more than 3 months and was on 'Probation', they are moved to 'Approved' when payroll is being processed.";
					em.Bcc = "mhyde@thatsnew.com";
					em.Send();
				}
				try
				{
					DataTable dt = Toolbox.doSQL_dt(@"Select * from member_offers  where status = 'Awaiting Start Date' and startdate<=curdate() and memberid !=0" , null);
					foreach (DataRow dr in dt.Rows)
					{
						NeMemberOffer.go_live(Convert.ToInt32(dr["id"]));
					}
				}
				catch (Exception exxx)
				{
					NeEMail em = new NeEMail();
					em.To = "aketelaars@newelectric.com";
					em.Subject = "something is busted on the auto benefits update";
					em.Body = exxx.ToString();
					em.From = "it@newelectric.com";
					em.Send();
				}
		 */
    }
    /**
     * Update Member offers and push it through go_live process
     */
    protected void update_offers(ref int i)
    {
		var currentPayPeriodId = NePayPeriod.CurrentPayPeriodId();
		var currentPayPeriod = new NePayPeriod(currentPayPeriodId);
		if((currentPayPeriod.end_date - DateTime.Now).TotalDays > 2)
			{
			return;
			}
        var dt = Toolbox.doSQL_dt(@"
		SELECT 
			a.id,
			a.business_unit_id
		FROM 
			member_offers a 
		LEFT JOIN
			business_unit c ON a.business_unit_id = c.id  
		WHERE 
			a.status = 'Awaiting Start Date' AND 
			a.startdate <= CURDATE() AND 
			a.memberid !=0 ", null);
        foreach (DataRow dr in dt.Rows)
			{
			var id	= Convert.ToInt32(dr["id"]);
			var businessUnitId = dr["business_unit_id"];
            var exist_check = Toolbox.doSQL_int(@"
			SELECT 
				COUNT(business_unit_id) COUNT 
			FROM 
				payroll  
			WHERE 
				business_unit_id =@v0 AND 
				payperiod_id = @v1 AND 
				sent = 0", new [] { businessUnitId, NePayPeriod.get_payperiod_id(DateTime.Today) });
            if (exist_check == 0)
				{
                NESI.BLL.Pages.Employees.NeMemberOffer.go_live(id);
				i++;
				}
			}
		}

    protected void clear_invoices(ref StringBuilder sb)
    {
        sb.Append("<ul>");
        var strresult = new StringBuilder();
        strresult.Append("<table cellpadding='0' cellspacing='0' id='processed' width='700px'><thead><tr><th align='center'>Branch</th><th align='center' style='background-color:#E0D8D8'>Waiting To Be Invoiced</th><th align='center'>Processed   </th><th align='center' style='background-color:#E0D8D8'>Invoice   </th><th align='center'>Invoice Date   </th><th align='center'>Success   </th></tr></thead><tbody>");
        var woprog_id = 0;
        var oldname = "";
        var newname = "";
        var invoice = "";
        double invoicedtotal = 0;
        var custpo = "";
        var invdate = "";
        double invoice_tax = 0;
        using (var myConn = Toolbox.connect())
        {
            var dt = Toolbox.doSQL_dt(myConn, @"Select business_unit.id bu_id, business_unit.* from business_unit  where id <>8 and id <>47 and istest = 'F'", null);
            foreach (DataRow dr in dt.Rows)
            {
                var business_unit_id = Convert.ToInt32(dr["bu_id"]);
                var bu = new NeBusinessUnit(business_unit_id);
                if (!bu.Active) continue;
                // if (bu.DSN == "") continue;
                //  using (var bvConn = BVDB.connect(bu.DSN))
                //  {
                var name = dr["name"].ToString();
                sb.AppendFormat("<li>{0}<ul>", name);
                #region Update WOProg Upon Invoice Creation in BV
                var update_woprog_creation_start = DateTime.Now;
                //			update_taxtotal(company_ID);
                //var dt1 = Toolbox.doSQL_dt(myConn, @"Select * from WOprog  where WOProg_Status = 'Waiting To Be Invoiced' and business_unit_id =@v0", new object[] { business_unit_id });
                //foreach (DataRow dr1 in dt1.Rows)
                //{
                //    woprog_id = Convert.ToInt32(dr1["woprog_id"]);
                //    strresult.Append("<tr><td align='center'>" + dr["name"] + "</td><td align='center' style='background-color:#E0D8D8'>" + woprog_id + "</td>");
                //    var wos = BVDB.getSQL_dt(bvConn, @"Select * from SALES_HISTORY_HEADER  WHERE Ord_No =?", new object[] { dr1["WOPROG_BVWO"] });
                //    if (wos.Rows.Count > 0)
                //    {
                //        foreach (DataRow wosdr in wos.Rows)
                //        {
                //            var sh = BVDB.getSQL_dt(bvConn, @"Select * from SALES_ORDER_HEADER  WHERE NUMBER =?", new object[] { dr1["WOPROG_BVWO"] });
                //            if (sh.Rows.Count == 0)
                //            {
                //                invoice = wosdr["NUMBER"].ToString();
                //                invoicedtotal = Convert.ToDouble(wosdr["BVSUBTOTAL"]);
                //                custpo = wosdr["CUST_PO_NO"].ToString();
                //                invoice_tax = Convert.ToDouble(wosdr["BVSALESTAX"]);
                //                invdate = wosdr["IN_Date"].ToString().Substring(0, 4) + "-" + wosdr["IN_Date"].ToString().Substring(4, 2) + "-" + wosdr["IN_Date"].ToString().Substring(6, 2);
                //                var dayscredit = 30;
                //                try
                //                { dayscredit = Convert.ToInt32(dr1["woprog_dayscredit"]); }
                //                catch (Exception ee)
                //                {
                //                    sendIsError(ee, "Update WOProg Upon Invoice Creation in BV 001");
                //                }
                //                var expdt = Convert.ToDateTime(invdate).AddDays(dayscredit);
                //                strresult.Append("<td align='center'>X</td><td align='center' style='background-color:#E0D8D8'>" + invoice + "</td><td align='center'>" + invdate + "</td>");
                //                try
                //                {
                //                    var lag = Toolbox.doSQL_double(myConn, @"SELECT IFNULL(DATEDIFF(a.woprog_opendatetime, MAX(b.date)),0) lag FROM woprog a INNER JOIN membertime b on b.membertime_woprog_id = a.woprog_id  WHERE woprog_id =@v0", new object[] { woprog_id });
                //                    Toolbox.doSQL_void(@"Update WOProg set WOProg_Status ='Invoiced',woprog_invoice_tax=@v0 , WOProg_CloseDateTime = Now(),woprog_InvoiceDate=@v1 ,woprog_InvoiceNo = @v2 , woprog_InvoicedNetTotal=@v3 , woprog_invoicebalance = @v3 , lag = @v5  where woProg_ID = @v4 ", new object[] { invoice_tax, invdate, invoice.Trim(), invoicedtotal, woprog_id, lag });
                //                    Toolbox.doSQL_void(@"Insert INTO woprogstatus (woprogstatus_WOProg_ID,woprogstatus_member_id,woprogstatus_status,woprogstatus_datetime)  values (@v0,9999,'Invoiced',Now())", new object[] { woprog_id });
                //                    strresult.Append("<td align='center'>Success - Changed to invoiced</td>");
                //                }
                //                catch (Exception ee)
                //                {
                //                    sendIsError(ee, "Update WOProg Upon Invoice Creation in BV 002");
                //                    strresult.Append("<td align='center'>CRASH " + ee + "</td>");
                //                }
                //                try
                //                {
                //                    EmailInvoice(woprog_id);
                //                }
                //                catch (Exception ee)
                //                {
                //                    sendIsError(ee, "Update WOProg Upon Invoice Creation in BV email_invoice(" + woprog_id + ") 003");
                //                }
                //            }
                //            else
                //            {
                //                strresult.Append("<td align='center' colspan='4'>Skipping, found lines in sales_order_header</td>");
                //            }
                //        }
                //    }
                //    else
                //    {
                //        strresult.Append("<td align='center' colspan='4'>Skipping, no lines in sales_history_header</td>");
                //    }
                //    strresult.Append("</tr>");
                //}
                //var update_woprog_creation_end = DateTime.Now;
                //var update_woprog_creation_span = update_woprog_creation_end.Subtract(update_woprog_creation_start);
                //sb.AppendFormat("<li>update_woprog_creation - {0} seconds", update_woprog_creation_span.TotalSeconds);
                //#endregion
                //strresult.Append("<tr><td align='center' colspan='6' style='background-color:#ccc;'>Updating balances for " + dr["name"] + "</td></tr>");

                //#region Update woprog upon PAYMENT received
                //var update_woprog_recvd_start = DateTime.Now;
                //var dt2 = Toolbox.doSQL_dt(myConn, @"Select WOProg.* from WOprog  where WOProg_Status = 'Invoiced' and business_unit_id =@v0 and woprog_invoice_paid_date is null and WOProg_InvoicedNetTotal <>0", new object[] { business_unit_id });
                //DataTable dt3;
                //foreach (DataRow dr2 in dt2.Rows)
                //{
                //    // look for the invoice thats been paid in bv and update woprog with the invoice paid date.. if it's finally paid in full
                //    dt3 = BVDB.getSQL_dt(bvConn, @"SELECT Concat(Concat(concat(Concat(Left(bvrvmoddate,4),'-'),Substring(bvrvmoddate,5,2)),'-'),RIGHT(bvrvmoddate,2)) as collecteddate,AR_TRANSACTIONS.BALANCE as balance FROM AR_TRANSACTIONS  WHERE (AR_TRANSACTIONS.CODE='I') and (AR_TRANSACTIONS.REF_NO=?)", new object[] { dr2["woprog_invoiceno"].ToString() });
                //    if (dt3.Rows.Count > 0)
                //    {
                //        if (Convert.ToDouble(dt3.Rows[0].ItemArray[1]) == 0)
                //        {
                //            var curr_inv = Toolbox.doSQL_double(myConn, @"SELECT IFNULL(MAX(woprog_invoicebalance), 0) FROM woprog  WHERE woprog_id =@v0", new object[] { dr2[0] });
                //            if (curr_inv != Convert.ToDouble(dt3.Rows[0].ItemArray[1]))
                //            {
                //                strresult.Append("<tr><td align='center'>" + dr["name"] + "</td><td align='center' style='background-color:#E0D8D8'>" + dr2[0] + "</td><td colspan='4'>Fully paid - Updating balance to " + dt3.Rows[0].ItemArray[1] + "</td></tr>");
                //                try
                //                {
                //                    Toolbox.doSQL_void(myConn, @"Update woprog  set woprog_invoice_paid_date =@v0,woprog_invoicebalance=@v1   where woprog_id =@v2", new object[] { dt3.Rows[0].ItemArray[0].ToString(), Convert.ToDecimal(dt3.Rows[0].ItemArray[1]), dr2[0] });
                //                }
                //                catch (Exception ee)
                //                {
                //                    sendIsError(ee);
                //                }
                //            }
                //        }
                //        else
                //        {
                //            var curr_inv = Toolbox.doSQL_double(myConn, @"SELECT IFNULL(MAX(woprog_invoicebalance), 0) FROM woprog  WHERE woprog_id =@v0", new object[] { dr2[0] });
                //            if (curr_inv != Convert.ToDouble(dt3.Rows[0].ItemArray[1]))
                //            {
                //                strresult.Append("<tr><td align='center'>" + dr["name"] + "</td><td align='center' style='background-color:#E0D8D8'>" + dr2[0] + "</td><td colspan='4'>Partial Update - Updating balance to " + dt3.Rows[0].ItemArray[1] + "</td></tr>");
                //                try
                //                { Toolbox.doSQL_void(myConn, @"update woprog  set woprog_invoicebalance=@v0  where woprog_id =@v1", new object[] { Convert.ToDecimal(dt3.Rows[0].ItemArray[1]), dr2[0] }); }
                //                catch (Exception ee)
                //                {
                //                    sendIsError(ee);
                //                }
                //            }
                //        }
                //    }
                //}
                //var update_woprog_recvd_end = DateTime.Now;
                //var update_woprog_recvd_span = update_woprog_recvd_end.Subtract(update_woprog_recvd_start);
                //sb.AppendFormat("<li>update_woprog_recvd - {0} seconds", update_woprog_recvd_span.TotalSeconds);
                #endregion
                sb.Append("</ul>");
                //   }
            }
            // now run back through the work orders that have actually been invoiced
            strresult.Append("<tr><td align='center' colspan='6' style='background-color:#ccc;'>Moving PDFs</td></tr>");
        }
    }

    /**
     * Move call log from it to neintranet
     */
	private void move_invoices(ref int i)
	{
		using (var myConn = Toolbox.connect())
		{
			var tax_entity_list = Toolbox.doSQL_dt(myConn, @"SELECT id FROM tax_entity  WHERE uses_folders and is_active", null);
			foreach (DataRow dr in tax_entity_list.Rows)
			{
				var te_id = Convert.ToInt32(dr["id"]);
				var te = new NeTaxEntity(te_id);
				if (te.business_units.Rows.Count == 0) continue;
				var buid = te.business_units.Rows[0]["id"];
				var fileServer = NeTaxEntity.BaseFolder(buid, false);
				var woPath = fileServer + @"\wos";

				var files = Directory.GetFiles(woPath);
				foreach (var path in files)
					{
					var f = new FileInfo(path);
					if(f.Extension != ".pdf") continue;
					if(!int.TryParse(Path.GetFileNameWithoutExtension(path), out var intwo)) continue;
					var isinvoiced = Toolbox.doSQL_int(myConn, @"SELECT IFNULL(COUNT(woprog_id),0) FROM woprog  WHERE woprog_id =@v0 AND woprog_status = 'Invoiced'", new object[] { intwo });
					if(isinvoiced <= 0 || intwo <= 10000) continue;
					var oldname = woPath + @"\" + intwo + ".pdf";
					var newname = woPath + @"\Invoiced\" + intwo + ".pdf";
					var temp_newname = woPath + @"\Invoiced\" + intwo + "_tmp.pdf";
					Directory.CreateDirectory(woPath + @"\Invoiced\");
					if (File.Exists(oldname) && !File.Exists(newname))
						{
						File.Move(oldname, newname);
						}
					else if (File.Exists(oldname) && File.Exists(newname))
						{
						if (File.Exists(temp_newname))
							{
							File.Delete(temp_newname);
							}
						File.Move(newname, temp_newname);
						File.Move(oldname, newname);
						}
					}

			}
		}
	}
	protected void move_cellphone_db()
    {
        var _celldb = new Toolbox();
        //_celldb.connection_string = WebConfigurationManager.ConnectionStrings["gps_db"].ConnectionString;
        _celldb.page_author = new NeMember(711);
        var _fromcell = Toolbox.doSQL_dt(@" SELECT calllogid_id id, phonenumber_ds phone_number, callername_ds caller_name_to, devicenumber_ds devicenumber_ds, duration_nb duration, FROM_UNIXTIME(calltime_nb/1000, '%Y-%m-%d %h:%i:%s') this_date, calltime_nb/1000 ts, calltype_fg fg, calltime_nb, imei_ds FROM it.ne_calllog  WHERE imported = FALSE", null);
        // Select the MAX ID, as this will be what we are updating TO
        var max_id = _celldb.getSQL_int(@"SELECT MAX(calllogid_id) FROM it.ne_calllog", null);
        var query = new StringBuilder();
        query.Append(@"INSERT INTO phone_log 
	(
	phone_log_altigen_session_id,
	phone_log_date,
	phone_log_from_number,
	phone_log_to_number,
	phone_log_from_name,
	phone_log_to_name,
	phone_log_duration,
	phone_log_time,
	phone_log_direction,
imei_ds,
calltime_nb,
calltype_fg
	)
VALUES 
");
        var c = 0;
        foreach (DataRow dr in _fromcell.Rows)
        {
            var phone_log = new NePhoneLog().get_NePhonglog(dr["imei_ds"], dr["calltime_nb"]);
            if (phone_log.phone_log_id == 0)
            {
                var id = (int)dr["id"];
                string phone_number_from;
                string phone_number_to;
                if (dr["phone_number"] != null && dr["phone_number"].ToString().Length > 9 && dr["devicenumber_ds"] != null && dr["devicenumber_ds"].ToString().Length > 9)
                {
                    if (dr["fg"].ToString().Equals("2"))  // if the call is outgoing
                    {
                        phone_number_from = dr["devicenumber_ds"].ToString().Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "").Replace(" ", "").Replace("*", "");
                        phone_number_to = dr["phone_number"].ToString().Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "").Replace(" ", "").Replace("*", "");
                    }
                    else
                    {
                        phone_number_to = dr["devicenumber_ds"].ToString().Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "").Replace(" ", "").Replace("*", "");
                        phone_number_from = dr["phone_number"].ToString().Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "").Replace(" ", "").Replace("*", "");
                    }
                    if (phone_number_from.Length == 11)
                    {
                        phone_number_from = phone_number_from.TrimStart('1');
                    }
                    if (phone_number_to.Length == 11)
                    {
                        phone_number_to = phone_number_to.TrimStart('1');
                    }
                    if (phone_number_to.Length == 10 && phone_number_from.Length == 10)
                    {
                        var caller_name_to = "";
                        var caller_name_from = "";
                        if (dr["fg"].ToString().Equals("2"))  // if outgoing
                        {
                            caller_name_to = Toolbox.doSQL_string(@"Select ifnull((Select contact_name from contact  where replace(replace(Contact_CellPhone,'-',''),' ','') =@v0 or replace(replace(contact_directline,'-',''),' ','') =@v1  limit 1),'') ", new object[] { phone_number_to, phone_number_to });
                            caller_name_from = Toolbox.doSQL_string(@"Select ifnull((SELECT member.member_fullname FROM cellphone_number INNER JOIN member ON cellphone_number.id = member.cellphone_number_id  WHERE replace(replace(cellphone_number.number,'-',''),' ','') =@v0 limit 1),'') ", new object[] { phone_number_from });
                        }
                        else
                        {
                            caller_name_to = Toolbox.doSQL_string(@"Select ifnull((SELECT member.member_fullname FROM cellphone_number INNER JOIN member ON cellphone_number.id = member.cellphone_number_id  WHERE replace(replace(cellphone_number.number,'-',''),' ','') =@v0 limit 1),'') ", new object[] { phone_number_to });
                            caller_name_from = Toolbox.doSQL_string(@"Select ifnull((Select contact_name from contact  where replace(replace(Contact_CellPhone,'-',''),' ','') =@v0 or replace(replace(contact_directline,'-',''),' ','') =@v1  limit 1),'') ", new object[] { phone_number_from, phone_number_from });
                        }
                        var duration = (int)dr["duration"];
                        var this_date = dr["this_date"].ToString();
                        var ts = dr["ts"].ToString();
                        var date = Toolbox.MySQL_shortdt(Convert.ToDateTime(this_date));
                        if (phone_number_to.Length == 10)
                        {
                            if (c > 0)
                            {
                                query.Append(",");
                            }
                            query.AppendFormat(@"
	(
	NULL,
	'{0}',
	'{1}',
	'{2}',
	'{9}',
	'{3}',
	{4},
	'{5}',
	{6},
'{7}',
{8},
{10}
	)
",
                    date,                               // {0}
                    phone_number_from,                  // {1}
                    phone_number_to,                    // {2}
                    Toolbox.AddSlashes(caller_name_to), // {3}
                    duration,                           // {4}
                    ts,                                 // {5}
                    dr["fg"].ToString().Equals("2") ? 2 : 1,            //{6}
                    dr["imei_ds"],//{7}
                    dr["calltime_nb"],//{8}
                    Toolbox.AddSlashes(caller_name_from),  // {9}
                    dr["fg"]  //{10}
                    );
                            c++;
                        }
                    }
                }
            }
        }
        if (c > 0)
        {
            _celldb.connection_string = Toolbox.conn_string();
            _celldb.getSQL_void(query.ToString(), null);
            _celldb.getSQL_void(@"UPDATE it.ne_calllog SET imported = TRUE WHERE calllogid_id <= @v0 ", new object[] { max_id });
        }
    }


    protected void send_customer_rates_warning(int business_unit_id, ref bool sent)
    {
            var dt = Toolbox.doSQL_dt(@"
SELECT
	a.id,
	a.customer_id,
	a.base_chargeout_id,
	a.notification_sent,
	b.business_unit_id,
	a.from_date AS from_date,
	a.to_date AS to_date,
	a.chargeout AS cust_rate_charegout,
	b.chargeout AS mt_chargeout,
	d.customer_name,
	c.membertype_name
FROM
	customer_rate a
INNER JOIN 
	membertype_chargeout b ON a.base_chargeout_id = b.id
INNER JOIN 
	membertype c ON b.membertype_id = c.membertype_id
INNER JOIN 
	customer d ON a.customer_id = d.customer_ID
WHERE
	a.notification_sent = 0 AND
	a.to_date > CURDATE() AND
	a.to_date < CURDATE() + INTERVAL 30 DAY AND
	b.paytype_id = 1 AND 
	b.business_unit_id = @v0 
ORDER BY 
	d.customer_name, c.membertype_name
", new object[] { business_unit_id });
	    if(dt.Rows.Count <= 0) return;
		var sb = new StringBuilder();
		sb.Append("<table border='0' cellpadding='1' cellspacing='1' style='width: 800px; font-family:arial,helvetica,sans-serif; font-size:10px;'>");
        sb.Append("<tr bgcolor='black' color='white'><td>Customer</td><td>Member Type</td><td>Rate</td><td>Expires</td><td>Normal ChargeOut</td></tr>");
        foreach (DataRow dr in dt.Rows)
        {
			sb.Append($"<tr bgcolor=\'white\' color=\'black\'><td>{dr["customer_name"]}</td><td>{dr["membertype_name"]}</td><td>{Math.Round(Convert.ToDouble(dr["cust_rate_charegout"]), 2):C2}</td><td>{Convert.ToDateTime(dr["to_date"]):yyyy-MM-dd}</td><td>{Math.Round(Convert.ToDouble(dr["mt_chargeout"]), 2):C2}</td></tr>");
            Toolbox.doSQL_void(@"UPDATE customer_rate SET notification_sent = 1 WHERE id = @v0", new [] { dr["id"] });
        }
	    sb.Append("</table>");
        var comp = new NeBusinessUnit(business_unit_id);
	    var mail = new NeEMail
		                {
		                To = comp.branch_manager.NEEmail,
		                CC = "rates@" + Toolbox.app_setting("DomainForEmail") + ";" +
		                    (comp.branch_manager.reports_to != 0
			                    ? new NeMember(comp.branch_manager.reports_to).NEEmail
			                    : ""),
		                isHTML = true,
		                From = "noreply@" + Toolbox.app_setting("DomainForEmail"),
		                Subject = "Customer Rates Expiring in the Next 30 Days",
		                Body = sb.ToString()
						};
	    mail.Send();
	    sent = true;
	    }
    protected void EmailInvoice(int _woprog_id)
    {
        var this_woprog = new NeWOProg(_woprog_id);
        var report = new InvoicePreview(_woprog_id, this_woprog.default_invoicetype);
        report = (InvoicePreview)Helper.WorkOrder.FillReport(report, this_woprog);
        var file = new MemoryStream();
        report.CreateDocument(false);
        report.ExportToPdf(file);
        file.Seek(0, SeekOrigin.Begin);
        NeWOProg.EmailInvoice(file, this_woprog, myMember);
    }

    /**
     *  Send out approval emails for work orders.
     */

    protected void send_approval_emails()
    {
        //(woprog_notification_sent is null or  woprog_notification_sent ='')  and
        var dt = Toolbox.doSQL_dt(@"Select woprog_id from woprog  where woprog.woprog_notification_sent is null and woprog_status='Waiting PM Approval' and business_unit_id !=8 limit 1 ", null);
        foreach (DataRow dr in dt.Rows)
        {
            try
            {
                var wo = new NeWOProg(Convert.ToInt32(dr["woprog_id"]));
                var pm = new NeMember(wo.intProjectManager);
                var ts_comments = Toolbox.doSQL_dt(@"Select *,get_name(wocomment_member_id) _name from wocomment  where woprog_id =@v0 and wocomment_member_id != 0", new object[] { wo.woprog_id });
                var ts_c = "<table border='0' cellpadding='1' cellspacing='1' style='width: 500px; font-family:arial,helvetica,sans-serif; font-size:11px;'>";
                foreach (DataRow rr in ts_comments.Rows)
                {
                    ts_c += "<tr><td>" + rr["Created_Date"] + "</td><td>" + rr["_name"] + "</td><td>" + rr["comments"] + "</td></tr>";
                }
                ts_c += "</table>";
                var error_base = All_Systems_Go(wo, "");
                var report = new InvoicePreview(wo.woprog_id, wo.default_invoicetype);
                report = (InvoicePreview)Helper.WorkOrder.FillReport(report, wo);
                report.Watermark.Text = "Not An Invoice";
                report.Watermark.ShowBehind = false;
                report.Watermark.ForeColor = System.Drawing.Color.Red;
                report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                report.Watermark.Font = new System.Drawing.Font(report.Watermark.Font.FontFamily, 50);
                report.Watermark.TextTransparency = 175;
                var file = new MemoryStream();
                report.CreateDocument(false);
                report.ExportToPdf(file);
                file.Seek(0, SeekOrigin.Begin);
                var attachment = new Attachment(file, "WO-" + wo.OrderNumber + ".pdf");
                var email = new NeEMail();
                email.To = pm.NEEmail;
                email.From = "noreply@" + Toolbox.app_setting("DomainForEmail");
                email.isHTML = true;
                email.Subject = "WO" + wo.OrderNumber + " for " + wo.CustomerName + " is waiting for your approval for invoicing..";
                email.Attachment = attachment;
                var strmessage = "";
                strmessage += @"<html>
	<head>
		<title></title>
	</head>
	<body>
		<table border='0' cellpadding='1' cellspacing='1' style='width: 700px;'>
			<tbody>
<tr nowrap='nowrap'>
</td>
	</tr>
<tr nowrap='nowrap'>
					<td>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>Work Order:</span></span></td>
					<td style='width: 100%;'>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>" + wo.OrderNumber + @"</span></span></td>
				</tr>
				<tr nowrap='nowrap'>
					<td>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>Customer:</span></span></td>
					<td style='width: 100%;'>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>" + wo.CustomerName + @"</span></span></td>
				</tr>
				<tr nowrap='nowrap'>
					<td>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>Description:</span></span></td>
					<td style='width: 100%;'>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>" + wo.Description + @"</span></span></td>
				</tr>
				<tr nowrap='nowrap'>
					<td nowrap='nowrap'>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>Invoice Total:</span></span></td>
					<td style='width: 100%;'>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>" + wo.net_total.ToString("c2") + @"</span></span></td>
				</tr>
				<tr nowrap='nowrap'>
				<tr >
					<td>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>Margin:</span></span></td>
					<td style='width: 100%;'>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>" + ((wo.net_total - (wo.woprog_materialcost + wo.woprog_labourcost)) / wo.net_total).ToString("p1") + @"</span></span></td>
				</tr>
				<tr>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>Timesheet Comments:</span></span></td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td colspan='2' style='width: 100%;'>
						" + ts_c + @"</td>
				</tr>
				<tr>
					<td>
						<span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>Line Items:</span></span></td>
					<td style='width: 100%;'>
						&nbsp;</td>
				</tr>
				<tr>
					<td colspan='2'>
						<table border='0' cellpadding='1' cellspacing='1' style='width: 500px; font-family:arial,helvetica,sans-serif; font-size:11px;'>
							<tbody>
								<tr style='background-color:#808080;color:#faebd7;'>
									<td>
										Part No</td>
									<td>
										Description</td>
									<td>
										Qty</td>
									<td>
										Billtype</td>
									<td>
										Price Each</td>
									<td>
										Price Ext</td>
								</tr>";
                var woitems = Toolbox.doSQL_dt(@"Select * from wo_detail_current,wo_detail_lineitem_billtype  where wo_detail_current.wo_detail_current_woprog_id =@v0 and wo_detail_current_billtypeid=wo_lineitem_billtypeid order by wo_detail_current_type, wo_detail_current_rec_no", new object[] { wo.woprog_id });
                double total = 0;
                foreach (DataRow r in woitems.Rows)
                {
                    if (Convert.ToDouble(r["wo_detail_current_price_sell"]) == 0 || Convert.ToDouble(r["wo_detail_current_qty_committed"]) == 0)
                    {
                        strmessage += @"<tr style='background-color:#CC0000;color:#ffffff;'>";
                    }
                    else
                    {
                        strmessage += @"<tr style='background-color:#ffffff;color:#000000;'>";
                    }
                    strmessage += @"<td valign='top'>" + r["wo_detail_current_master_id"] + "</td><td valign='top'>" + r["wo_detail_current_description"] + "</td><td valign='top'>" + r["wo_detail_current_qty_committed"] + "</td><td valign='top'>" + r["wo_lineitem_billtype_name"] + "</td><td align='right' valign='top'>" + Convert.ToDouble(r["wo_detail_current_price_sell"]).ToString("c2") + "</td><td align='right' valign='top'>" + Convert.ToDouble(Convert.ToDouble(r["wo_detail_current_qty_committed"]) * Convert.ToDouble(r["wo_detail_current_price_sell"])).ToString("c2") + "</td></tr>";
                    if (Convert.ToInt32(r["wo_detail_current_billtypeid"]) == 0 || Convert.ToInt32(r["wo_detail_current_billtypeid"]) == 2 || Convert.ToInt32(r["wo_detail_current_billtypeid"]) == 3 || Convert.ToInt32(r["wo_detail_current_billtypeid"]) == 10)
                    {
                        total += Convert.ToDouble(r["wo_detail_current_qty_committed"]) * Convert.ToDouble(r["wo_detail_current_price_sell"]);
                    }
                }
                strmessage += "<tr><td></td><td></td><td></td><td></td><td>Total:</td><td>" + total.ToString("c2") + "</td></tr>";
                strmessage += @"</tbody>
						</table>
					</td>
				</tr>
			</tbody>
		</table>
		<p>
			&nbsp;</p>
	</body>
</html>";
                Toolbox.doSQL_void(@"update woprog set woprog_notification_sent = curdate()  where woprog_id =@v0", new object[] { wo.woprog_id });
                wo = new NeWOProg(Convert.ToInt32(dr["woprog_id"]));
                if (error_base == "")
                {
                    email.Body = strmessage;
                    email.URLyes = string.Format("/wo_prog_edit.aspx?woprog_id=" + wo.woprog_id + "&business_unit_id=" + wo.business_unit_id + "&bvwo=" + wo.OrderNumber + "&action=approve&ts=" + wo.woprog_ts.Ticks);
                    email.to_member_id = pm.id;
                    email.file_passport();
                }
                else
                {
                    email.Send();
                }
            }
            catch (Exception ee)
            {
                sendIsError(ee);
            }
        }
    }

    protected void send_timesheet_reminders()
    {
        #region send individual emails

        if (!(DateTime.Today.DayOfWeek == DayOfWeek.Sunday) && !(DateTime.Today.DayOfWeek == DayOfWeek.Monday))
        {
            var dt = Toolbox.doSQL_dt(@"Call get_timesheet_failure_list()", null);
            foreach (DataRow dr in dt.Rows)
            {
                var mem = new NeMember(Convert.ToInt32(dr["Member_ID"]));
                var email_subject =
                    @"mailto:aketelaars@newelectric.com?subject=Andy, you have a problem with your timesheet reminder module&body=Hey Andy, its " +
                    mem.FullName + " from " + mem.business_unit.name +
                    ".  I got this email reminding me that i maybe had missed entering my time.  From what i can see, i did enter it, or had a vacation day booked.  The date was " +
                    DateTime.Today.ToString("yyyy-MM-dd" + ".");
                var body = @"<table style='width: 100%; font-family: Arial; font-size:8pt;'>
			<tr>
				<td>" + dr["Member_nickname"] +
                           @", this is a friendly reminder that you MAY have missed your time entry yesterday. 					
				</td>
			</tr><tr></tr>
			<tr>
				<td><a href='" + email_subject +
                           @"'>If you did enter your time or you booked vacation time yesterday, maybe something is wrong with my programming.. please let me know :)</a>
				</td>
			</tr><tr></tr>
			<tr>
				<td>You can use the mobile site to enter your time via your phone and of course you can enter your time from any web browser in the world!  The reason why we care is we need to get invoicing out quickly and accurately.  When time is entered late we might not get a chance to bill it.  We would like to avoid the extra time to chase these missing hours but we really don't want you to be short any hours that you worked.
				</td>
			</tr>
			<tr>
			<td><img src='https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSVYyNEQKKn9VcYV0_BfpPW0imgyZOZPAq9Lr7_IESuQO3cjM6_'/></td>
			</tr>
			</table>";
                var e = new NeEMail();
                //		e.CC = new NeMember(Convert.ToInt32(mem.reports_to)).NEEmail;
                e.From = "admin@" + Toolbox.app_setting("DomainForEmail");
                e.Subject = DateTime.Today.ToString("yyyy-MM-dd") + "- missing time entry";
                e.Body = body;
                e.isHTML = true;
                //		e.To = "aketelaars@newelectric.com";
                //		e.Send();
                try
                {
                    if (mem.NEEmail != "" && !mem.NEEmail.Contains("nomail"))
                    {
                        e.To = mem.NEEmail;
                        e.Send();
                    }
                    else
                    {
                        if (mem.Email != "" && !mem.Email.Contains("nomail"))
                        {
                            e.To = mem.Email;
                            e.Send();
                        }
                    }
                }
                catch (Exception ee)
                {
                    sendIsError(ee);
                }
            }
        }

        #endregion
        #region send complete list to branch managers
        var dt_branches = Toolbox.doSQL_dt(@"Select id,name from business_unit  where active = 'T' and emailtimesheets = 1 and id not in (8,11,47,48)", null);
        foreach (DataRow dr_branch in dt_branches.Rows)
        {
            try
            {
                var dt = Toolbox.doSQL_dt(@"Call get_timesheet_failure_list_Complete(@v0)", new object[] { dr_branch["id"] });
                var _body = @"<table style='width: 100%; font-family: Arial; font-size:8pt;'><tr><th>These People Have Not Entered Their Time for Yesterday</th></tr>";
                foreach (DataRow dr in dt.Rows)
                {
                    _body += @"<tr><td>" + dr["Member_fullname"] + @"</td></tr>";
                }
                _body += "</table>";
                var _e = new NeEMail();
                _e.To = new NeBusinessUnit().GetBMEmail(Convert.ToInt32(dr_branch["id"]));
                //		_e.To = "aketelaars@newelectric.com";
                _e.From = "admin@" + Toolbox.app_setting("DomainForEmail");
                _e.Subject = DateTime.Today.ToString("yyyy-MM-dd") + "- missing time entries for " + dr_branch["name"];
                _e.Body = _body;
                _e.isHTML = true;
                _e.Send();
            }
            catch (Exception ee)
            {
                sendIsError(ee, "invoice service crash on timesheet report");
                return;
            }
        }
        #endregion
    }


    protected void send_termination_checklists_reminders(ref int i)
    {
        if (Toolbox.doSQL_int(@"select count(id) from invoice_service_email_report_history  where dt > curdate() - interval 30 day and report_run = 'daily_termination_watch'",new object[]{}) == 0)
        {

            #region send termination report to supervisors
            var dt_reports = Toolbox.doSQL_dt(@"		SELECT distinct  member.reports_to
FROM emp_trm_chklist_lnk 
INNER JOIN emp_trm ON emp_trm_chklist_lnk.trm_id = emp_trm.id 
INNER JOIN member ON emp_trm.trm_member_id = member.Member_ID 
INNER JOIN business_unit b on b.id = member.business_unit_id and b.active='T' and istest = 'F'
INNER JOIN emp_trm_chklist_opt ON emp_trm_chklist_lnk.opt_id = emp_trm_chklist_opt.id 
WHERE member.member_hrstatus_id = 4 
AND emp_trm_chklist_lnk.is_checked = 0 
AND type = 'Direct' and reports_to !=0 
order by reports_to
", null);

            foreach (DataRow dr in dt_reports.Rows)
            {

                var dt = Toolbox.doSQL_dt(@"
SELECT 
	DISTINCT   fun_time(member.Member_TermDate) `Termination Date`, 
	member.member_fullname `Employee`,
Concat(@v1,'/#/opens/127/employees/',member.member_id) Link
FROM emp_trm_chklist_lnk 
INNER JOIN emp_trm ON emp_trm_chklist_lnk.trm_id = emp_trm.id 
INNER JOIN member ON emp_trm.trm_member_id = member.Member_ID 
INNER JOIN business_unit b on b.id = member.business_unit_id and b.active='T' and istest = 'F'
INNER JOIN emp_trm_chklist_opt ON emp_trm_chklist_lnk.opt_id = emp_trm_chklist_opt.id 
WHERE member.member_hrstatus_id = 4 
AND emp_trm_chklist_lnk.is_checked = 0 
AND type = 'Direct' and reports_to = @v0
",
                    new object[] { Convert.ToInt32(dr["reports_to"]), Toolbox.app_setting("Domain") });


                var m = new NeMember(Convert.ToInt32(dr["reports_to"]));
                var s = get_email_body_header("The following employees need to be terminated properly in " + Toolbox.app_setting("Domain") + ".  You may have to redirect emails and retrieve company assets etc.  The checklists found behind these links serve as a reminder for you.  These are the smae links that you will find on your home page in your TODO list.  If these steps are done, simply check the boxes.", dt,
                    false, m.business_unit);
                var email = new NeEMail
                {
                    To = m.NEEmail,
                    //   To = "aketelaars@newelectric.com",
                    From = "noreply@" + Toolbox.app_setting("DomainForEmail"),
                    Subject = "Previous Employee Cleanup : " + DateTime.Today.ToShortDateString(),
                    Body = s,
                    isHTML = true
                };
				i++;
                email.Send();
            }
            Toolbox.doSQL_void(@"insert into invoice_service_email_report_history (dt,report_run) values (curdate(),'daily_termination_watch')",new object[] { });
        }
        #endregion

        #region send termination report to nesi staff


        #endregion


    }

    protected void send_ts_watch_reports(ref int i)
	    {
	    if(DateTime.Now.Hour != 8 || DateTime.Today.DayOfWeek == DayOfWeek.Monday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday) return;
	    if(Toolbox.doSQL_int(@"SELECT COUNT(id) FROM invoice_service_email_report_history  WHERE dt > CURDATE() - INTERVAL 22 HOUR AND report_run = 'daily_ts_watch'",new object[] { }) != 0) return;
	    var dt_reports = Toolbox.doSQL_dt(@"
SELECT
	a.reports_to id
FROM
	member a
INNER JOIN 
	business_unit b on b.id = a.business_unit_id
WHERE
	a.member_status = 'Active' AND
	a.member_ts_watch = true AND
	((SELECT COUNT(m.member_id) FROM member m WHERE m.member_id = a.reports_to AND m.member_status='Active')!=0) AND 
	b.Active = 'T' AND 
	b.IsTest = 'F'
GROUP BY
	a.reports_to
", null);

	    foreach (DataRow dr in dt_reports.Rows)
		    {
			var id = (int) dr["id"];
		    var dt = Toolbox.doSQL_dt(@"CALL GET_TS_WATCH(@v0)", new object[] { id });
		    if(dt.Rows.Count <= 0) continue;
		    var m = new NeMember(id);
			if(m.NEEmail == "") continue;
		    var s = get_email_body_header("Time Entered in Past 24 hours : " + DateTime.Today.ToShortDateString(), dt, false, m.business_unit);
		    var email = new NeEMail
			                {
			                To = m.NEEmail,
			                From = "noreply@" + Toolbox.app_setting("DomainForEmail"),
			                Subject = "Timesheet Watch : " + DateTime.Today.ToShortDateString(),
			                Body = s,
			                isHTML = true
			                };
		    email.Send();
		    }
	    Toolbox.doSQL_void(@"INSERT INTO invoice_service_email_report_history (dt,report_run) VALUES (curdate(),'daily_ts_watch')",new object[] { });
	    }


    protected string All_Systems_Go(NeWOProg wo, string type)
    {
        var issue = "";
        if (Toolbox.doSQL_int(@"Select Count(po_details_id) from po_details_current
where po_details_line_active = 1 
and po_details_woprog_id =@v0 
and po_details_current.is_gl_account = false", new object[] { wo.woprog_id }) > 0)
        {
            issue = "There are incomplete PO lines related to this work order";
            return issue;
        }
        var dt = Toolbox.doSQL_dt(@"Select (wo_detail_current_price_sell-wo_detail_current_price_cost) as spread, 
wo_detail_current_master_id master_id 
from wo_detail_current  
where wo_detail_current_woprog_id =@v0 
AND IFNULL(wo_detail_current_notes,'') NOT LIKE 'A_%|%'", new object[] { wo.woprog_id });
        foreach (DataRow dr in dt.Rows)
        {
            if (Convert.ToDouble(dr["spread"]) < 0)
            {
                issue += dr["master_id"] + " is being sold at less than cost\\n";
            }
        }
        if (issue != "")
        {
            issue = "The following parts are being sold at less than cost: \\n" + issue;
            return issue;
        }
        if (issue == "")
        {
        }
        return issue;
    }


    protected void fix_billable_time_in_dashboard()
    {
        var dt = Toolbox.doSQL_dt(@"select id,dt,business_unit_id from dashboard_snapshot_hist dh  where dh.dt>'2014-01-01'", null);
        foreach (DataRow dr in dt.Rows)
        {
            var _date = Convert.ToDateTime(dr["dt"]);
            var d = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(a.numberofhours), 0) FROM membertime a  WHERE a.business_unit_id =@v0 AND a.date BETWEEN @v1 AND @v2 and a.WOType='WO' AND a.membertime_customer_id NOT IN ( SELECT internal_companyno_intranet_custid FROM internal_companyno WHERE business_unit_id = a.business_unit_id)", new object[] { dr["business_unit_id"], new DateTime(_date.Year, _date.Month, 1).ToString("yyyy-MM-dd"), new DateTime(_date.Year, _date.Month, _date.Day).ToString("yyyy-MM-dd") });
            Toolbox.doSQL_void(@"update dashboard_snapshot_hist  set billable_hours=@v0  where id =@v1 limit 1 ", new object[] { d, dr["id"] });
        }
    }

    /**
     * Fetching data from phone_log table and and add to appropriate customer history 
     */
    private void update_customer_history_phone_activity()
    {
        using (var conn = Toolbox.connect())
        {
            var dt = Toolbox.doSQL_dt(@"SELECT phone_log.phone_log_id, phone_log.phone_log_altigen_session_id, phone_log.phone_log_date, phone_log.phone_log_from_number, phone_log.phone_log_to_number, phone_log.phone_log_from_name, phone_log.phone_log_to_name, phone_log.notes, phone_log.phone_log_duration, phone_log.phone_log_time, phone_log.phone_log_direction, phone_log.is_internal, phone_log.calltype_fg, phone_log.numbertype_fg, phone_log.calltime_nb FROM phone_log  WHERE phone_log.phone_log_date > curdate()-interval 3 day and phone_log.updated_to_customer_history = 0 limit 2000", null);
            foreach (DataRow dr1 in dt.Rows)
            {
                var pl = new NePhoneLog(Convert.ToInt32(dr1[0]));
                var from_cell_phone_id = 0;
                var from_phone_numbers_id = 0;
                var from_customer_id = 0;
                var from_address_id = 0;
                var from_contact_id = 0;
                var from_member_id = 0;
                var to_cell_phone_id = 0;
                var to_phone_numbers_id = 0;
                var to_customer_id = 0;
                var to_address_id = 0;
                var to_contact_id = 0;
                var to_member_id = 0;
                try
                {
                    #region update from name
                    // check if its an employee - look at cell phone number table
                    if (pl.phone_log_from_number.Length == 3)
                    {
                        pl.phone_log_from_name = Toolbox.doSQL_string(@"Select ifnull((Select member_fullname from member  where member_phoneextension =@v0 and member_status='Active' limit 1), '') ", new object[] { pl.phone_log_from_number, pl.phone_log_from_name });
                        from_member_id = Toolbox.doSQL_int(conn, @"Select ifnull((Select member_id from member  where member_phoneextension =@v0 and member_status='Active' limit 1),0)", new object[] { pl.phone_log_from_number });
                    }
                    else
                    {
                        from_cell_phone_id = Toolbox.doSQL_int(conn, @"Select ifnull((Select id from cellphone_number  where replace(number,'-','') =@v0 and status ='Active' limit 1),0)", new object[] { pl.phone_log_from_number });
                        if (from_cell_phone_id != 0)
                        {
                            pl.phone_log_from_name = Toolbox.doSQL_string(@"Select ifnull((Select member_fullname from member  where cellphone_number_id =@v0 limit 1),'') ", new object[] { from_cell_phone_id, pl.phone_log_from_name });
                            from_member_id = Toolbox.doSQL_int(@"Select ifnull((Select member_id from member  where cellphone_number_id =@v0 limit 1),0) ", new object[] { from_cell_phone_id });
                        }
                        else
                        {
                            // check if its a contact
                            from_contact_id = Toolbox.doSQL_int(@"Select ifnull((select contact_id from contact  where (replace(contact_cellphone,'-','')=@v0 or replace(contact_directline,'-','')=@v1  ) and contact_status != 4 limit 1),0)", new object[] { pl.phone_log_from_number, pl.phone_log_from_number });
                            if (from_contact_id != 0)
                            {
                                var contact = new NEContact(from_contact_id);
                                pl.phone_log_from_name = contact.Contact_Name.Replace("'", "");
                                if (contact.Contact_Type == "Customer")
                                {
                                    from_customer_id = contact.customer_id;
                                }
                                from_address_id = contact.address_id;
                            }
                            else
                            {
                                // check if its a customer
                                from_phone_numbers_id = Toolbox.doSQL_int(@"Select ifnull((Select phone_numbers_id from phone_numbers  where replace(phone_numbers_number,' ','') =@v0 and phone_numbers_active=1 limit 1),0)", new object[] { pl.phone_log_from_number });
                                if (from_phone_numbers_id != 0)
                                {
                                    var pn = new NePhoneNumbers(from_phone_numbers_id);
                                    if (pn.phone_numbers_type == "Address")
                                    {
                                        var a = new NEAddress(Convert.ToInt32(pn.phone_numbers_table_id));
                                        if (a.Table == "Customer")
                                        {
                                            var cust = new NECustomer(Convert.ToInt32(a.Table_ID));
                                            if (!cust.Customer_Name.Contains("Cash Sale"))
                                            {
                                                pl.phone_log_from_name = cust.Customer_Name.Replace("'", "");
                                                from_customer_id = cust.Customer_ID;
                                            }
                                            else
                                            {
                                                from_address_id = 0;
                                            }
                                        }
                                        else if (a.Table == "Vendor")
                                        {
                                            var vend = new NEVendor(a.Table_ID);
                                            pl.phone_log_from_name = vend.Vendor_Name.Replace("'", "");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region update to name
                    // check if its an employee - look at cell phone number table
                    if (pl.phone_log_to_number.Length == 3)
                    {
                        pl.phone_log_to_name = Toolbox.doSQL_string(@"Select ifnull((Select member_fullname from member  where member_phoneextension =@v0 and member_status='Active' limit 1), '') ", new object[] { pl.phone_log_to_number, pl.phone_log_to_name });
                        to_member_id = Toolbox.doSQL_int(conn, @"Select ifnull((Select member_id from member  where member_phoneextension =@v0 and member_status='Active' limit 1),0)", new object[] { pl.phone_log_to_number });
                    }
                    else
                    {
                        to_cell_phone_id = Toolbox.doSQL_int(@"Select ifnull((Select id from cellphone_number  where replace(number,'-','') =@v0 and status ='Active' limit 1),0)", new object[] { pl.phone_log_to_number });
                        if (to_cell_phone_id != 0)
                        {
                            pl.phone_log_to_name = Toolbox.doSQL_string(@"Select ifnull((Select member_fullname from member  where cellphone_number_id =@v0 limit 1),'')", new object[] { to_cell_phone_id, pl.phone_log_to_name });
                            to_member_id = Toolbox.doSQL_int(@"Select ifnull((Select member_id from member  where cellphone_number_id =@v0 limit 1),0) ", new object[] { to_cell_phone_id });
                        }
                        else
                        {
                            // check if its a contact
                            to_contact_id = Toolbox.doSQL_int(@"Select ifnull((select contact_id from contact  where (replace(contact_cellphone,'-','')=@v0 or replace(contact_directline,'-','')=@v1  ) and contact_status != 4 limit 1),0)", new object[] { pl.phone_log_to_number, pl.phone_log_to_number });
                            if (to_contact_id != 0)
                            {
                                var contact = new NEContact(to_contact_id);
                                pl.phone_log_to_name = contact.Contact_Name.Replace("'", "");
                                if (contact.Contact_Type == "Customer")
                                {
                                    to_customer_id = contact.customer_id;
                                }
                                to_address_id = contact.address_id;
                            }
                            else
                            {
                                // check if its a customer
                                to_phone_numbers_id = Toolbox.doSQL_int(@"Select ifnull((Select phone_numbers_id from phone_numbers  where replace(phone_numbers_number,' ','') =@v0 and phone_numbers_active=1 limit 1),0)", new object[] { pl.phone_log_to_number });
                                if (to_phone_numbers_id != 0)
                                {
                                    var pn = new NePhoneNumbers(to_phone_numbers_id);
                                    if (pn.phone_numbers_type == "Address")
                                    {
                                        var a = new NEAddress(Convert.ToInt32(pn.phone_numbers_table_id));
                                        if (a.Table == "Customer")
                                        {
                                            var cust = new NECustomer(Convert.ToInt32(a.Table_ID));
                                            if (!cust.Customer_Name.Contains("Cash Sale"))
                                            {
                                                to_customer_id = cust.Customer_ID;
                                                pl.phone_log_to_name = cust.Customer_Name.Replace("'", "");
                                            }
                                            else
                                            {
                                                to_address_id = 0;
                                            }
                                        }
                                        else if (a.Table == "Vendor")
                                        {
                                            var vend = new NEVendor(a.Table_ID);
                                            pl.phone_log_to_name = vend.Vendor_Name.Replace("'", "");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ee)
                {

                    Toolbox.do_errorLog_errorStack(ee);
                }
                if (from_address_id != 0 || to_address_id != 0)
                {
                    var a = new NEAddress(from_address_id != 0 ? from_address_id : to_address_id);
                    if (a.Table == "Customer")
                    {
                        NECustomer.add_to_customer_history(a.Table_ID, (to_member_id == 0 ? from_member_id : to_member_id), pl.phone_log_date, pl.phone_log_from_name + " called " + pl.phone_log_to_name, 18, 1, (from_address_id != 0 ? from_address_id : to_address_id));
                    }
                }
                pl.update_after_customer_history();
            }
        }
    }
    /**
     * Update Qty if committed > ordered 
     */

    private void clear_out_exclude_vendor_pricing()
    {

        var dt = Toolbox.doSQL_dt(@"SELECT
Count(inventory_price.id),
inventory_price.master_id,
inventory_price.business_unit_id
FROM
inventory_price
INNER JOIN inventory_item_master ON inventory_price.master_id = inventory_item_master.master_id
INNER JOIN inventory_tag ON inventory_item_master.tag_id = inventory_tag.tag_id AND inventory_tag.is_exclude = 1
GROUP BY
inventory_price.master_id,
inventory_price.business_unit_id
HAVING
Count(inventory_price.id)>50", null);
        foreach (DataRow dr in dt.Rows)
        {
            var count = Convert.ToInt32(dr[0]);
            var dt2 = Toolbox.doSQL_dt(@"SELECT
inventory_price.id
FROM
inventory_price
INNER JOIN inventory_item_master ON inventory_price.master_id = inventory_item_master.master_id
INNER JOIN inventory_tag ON inventory_item_master.tag_id = inventory_tag.tag_id AND inventory_tag.is_exclude = 1
where 
inventory_price.master_id = @v0 and
inventory_price.business_unit_id =@v1
order by edited_dt
limit @v2", new object[] { dr["master_id"], dr["business_unit_id"], (count - 50) });
            foreach (DataRow dr2 in dt2.Rows)
            {

                Toolbox.doSQL_void("Delete from inventory_price where id = @v0 limit 1", new object[] { dr2[0] });

            }

        }


    }

    private void wo_adjust_committed_required()
    {
        try
        {
            Toolbox.doSQL_void(@"UPDATE
  wo_detail_current a
  LEFT JOIN woprog b
    ON a.wo_detail_current_woprog_id = b.woprog_id SET wo_detail_current_qty_ordered = wo_detail_current_qty_committed
WHERE b.woprog_status = 'Open'
  AND a.wo_detail_current_qty_committed > a.wo_detail_current_qty_ordered", new object[] { });
        }
        catch (Exception ee)
        {
            // This doesn't HAVE to run, and if it errors, shouldn't stop invoice services.
            sendIsError(ee);
        }

    }
    protected void send_waiting_cust_po_emails()
    {
        var dt = Toolbox.doSQL_dt(@"SELECT distinct(woprog.woprog_pm_memberid) pmid, b.member_neemail `reports_to_email`, member.member_neemail, member.member_fullname, woprog.business_unit_id FROM woprog INNER JOIN member ON woprog.woprog_pm_memberid = member.Member_ID left join member b on b.member_id = member.reports_to  WHERE woprog.WOProg_Status = 'Waiting For PO' and woprog.business_unit_id<>8 and woprog.business_unit_id<>48 and datediff(curdate(),woprog.WOProg_OpenDateTime)>=1 order by woprog_pm_memberid ", null);
        foreach (DataRow dr in dt.Rows)
        {
            var dt1 = Toolbox.doSQL_dt(@" SELECT woprog.woprog_bvwo WO, woprog.woprog_customername Customer, woprog.woprog_description Description, woprog.WOProg_Status Status, woprog.WOProg_StillToBeBilled `WO Amount`, datediff(curdate(),woprog.WOProg_OpenDateTime) `Days Since Scanned` FROM woprog INNER JOIN member ON woprog.woprog_pm_memberid = member.Member_ID left join member b on b.member_id = member.reports_to  WHERE woprog.woprog_pm_memberid=@v0 and woprog.WOProg_Status = 'Waiting For PO' and woprog.business_unit_id<>8 and woprog.business_unit_id<>48 and datediff(curdate(),woprog.WOProg_OpenDateTime)>=1 order by woprog_pm_memberid ", new object[] { dr["pmid"] });
            if (dt1.Rows.Count > 0)
            {
                var s = get_email_body_header("Work Orders Waiting for POs", dt1, false, new NeBusinessUnit(11));
                s += System.Environment.NewLine;
                s += "<a href='" + Toolbox.app_setting("Domain") + "/wo_prog_edit.aspx?business_unit_id=" + dr["business_unit_id"] + "'>Click here to jump to the work order bucket page</a>";
                var email = new NeEMail();
                email.To = dr["member_neemail"].ToString();
                email.CC = dr["reports_to_email"].ToString();
                //	email.To = "aketelaars@newelectric.com";
                email.From = "admin@" + Toolbox.app_setting("DomainForEmail");
                email.Subject = "Work Orders Waiting for POs - Week of " + DateTime.Today.ToString("yyyy-MM-dd") + "  for " + dr["member_fullname"];
                email.Body = s;
                email.isHTML = true;
                email.Send();
            }
        }
    }
    private static void sendProbationEmail()
    {
        var dt = Toolbox.doSQL_dt(@"
SELECT  
    member_startdate + INTERVAL 90 DAY AS probation_over,
    member_fullname,
    member_startdate, 
    a.reports_to, 
    a.member_id,
    (
        SELECT reports_to FROM member b WHERE b.member_id = a.reports_to ) AS reports_to_to, 
        DATEDIFF(member_startdate + INTERVAL 90 DAY, CURDATE()
    ) days,
    c.name, 
    b.membertype_name
FROM member a
LEFT JOIN membertype b ON 
a.member_membertype_id = b.membertype_id
LEFT JOIN business_unit c ON 
a.business_unit_id = c.id
WHERE `member_hrstatus_id` = 6
    AND DATEDIFF(member_startdate + INTERVAL 90 DAY, CURDATE()) >= 0
    AND DATEDIFF(member_startdate + INTERVAL 90 DAY, CURDATE()) <= 30", null);
        foreach (DataRow dr in dt.Rows)
        {
            var startD = Convert.ToDateTime(dr["member_startdate"]);
            var overD = Convert.ToDateTime(dr["probation_over"]);
            var startDate = startD.ToString("yyyy-MM-dd");
            var overDate = overD.ToString("yyyy-MM-dd");
            var memberType = dr["membertype_name"].ToString();
            var memberName = dr["member_fullname"].ToString();
            var companyName = dr["name"].ToString();
            var daysInt = Convert.ToInt32(dr["days"]);
            string days;
            if (daysInt == 1)
            {
                days = "1 day";
            }
            else
            {
                days = daysInt + " days";
            }
            var reportLine = NeMember.get_reporting_line(Convert.ToInt32(dr["member_id"]));
            var probationMember = new NeMember(Convert.ToInt32(dr["member_id"]));
            if (probationMember.member_probation_email_sent)
            {
                continue;
            }
            var to = new NeMember(Convert.ToInt32(dr["reports_to"]));
            var cc = new NeMember(Convert.ToInt32(dr["reports_to_to"]));
            //brnach name
            var eBody = string.Format(@"
<style type='text/css'>
table.stat td {{ font-family: Arial, Helvetica, sans-serif; font-size:9pt; }}
table.sstat td {{ font-family: Arial, Helvetica, sans-serif; font-size:9pt; }}
</style>
<div style='font-size:12px;font-family:arial;'>
    {7},<br>
    <div>Probation period over for {0} in {6}.</div>
</div>
<table class='stat'>
    <tr>
        <td><b>Probation Details:</b></td>
        <td></td>
    </tr>
    <tr>
        <td><b>Start Date:</b></td>
        <td>{1}</td>
    </tr>
    <tr>
        <td><b>Probation ends:</b></td>
        <td>{2}</td>
    </tr>
    <tr>
        <td><b>Business Unit:</b></td>
        <td>{3}</td>
    </tr>
    <tr>
        <td><b>Reports To:</b></td>
        <td>{4}</td>
    </tr>
    <tr>
        <td><b>Member Type:</b></td>
        <td>{5}</td>
    </tr>
</table>", memberName, startDate, overDate, companyName, reportLine, memberType, days, to.Nickname);
            //to.NEEmail + cc.NEEmail
            var em = new NeEMail();
            em.To = to.NEEmail;
            //CC report to
            em.CC = cc.NEEmail;
            em.From = "admin@" + Toolbox.app_setting("DomainForEmail");
            em.isHTML = true;
            em.Body = eBody; // + to.NEEmail + " " + cc.NEEmail;
            em.Subject = memberName + "'s Probation period is almost up!";
            em.Send();
            probationMember.member_probation_email_sent = true;
            probationMember.save();
        }
    }
    public void get_currencies()
    {
        var past_records = Toolbox.doSQL_dt(@"SELECT date, currency FROM currency_history  WHERE date > DATE_SUB(curdate(), INTERVAL 10 DAY)", null);
        var cursor_date = DateTime.Now.Date.AddDays(-10);
        var past = past_records.AsEnumerable();
        while (cursor_date <= DateTime.Now.Date)
        {
            var this_day = from p in past where (DateTime)p["date"] == cursor_date.Date select p;
            if (this_day.Count() == 0)
            {
                var w = WebRequest.Create("https://openexchangerates.org/api/historical/" + Toolbox.MySQL_shortdt(cursor_date) + ".json?app_id=395f167f51744ac3a90d9e69a895d640");
                using (var r = w.GetResponse())
                {
                    var dataStream = r.GetResponseStream();
                    using (var sr = new StreamReader(dataStream))
                    {
                        var resp = sr.ReadToEnd();
                        var token = JObject.Parse(resp);
                        Toolbox.doSQL_void(@"INSERT INTO currency_history (date, currency, per_usd) VALUES (@v0 , 'USD', @v1 ), (@v0 , 'CAD', @v2 ), (@v0 , 'EUR', @v3 )", new object[] { Toolbox.MySQL_longdt(cursor_date), token["rates"]["USD"], token["rates"]["CAD"], token["rates"]["EUR"] });
                    }
                }
            }
            cursor_date = cursor_date.Date.AddDays(1);
        }
    }

    protected void update_currency()
    {

        var client = new HttpClient();
        client.BaseAddress = new Uri(URL);

        // Add an Accept header for JSON format.
        client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            // List data response.
            var response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                var quotes = JObject.Parse(response.Content.ReadAsStringAsync().Result)["quotes"] as JObject;
                var usdcad = quotes.GetValue("USDCAD").ToString();
                var usdeur = quotes.GetValue("USDEUR").ToString();
                Toolbox.doSQL_void("update currency set per_usd = @v0 where currency = 'CAD'", new object[] { Convert.ToDouble(usdcad) });
                Toolbox.doSQL_void("update currency set per_usd = @v0 where currency = 'EUR'", new object[] { Convert.ToDouble(usdeur) });

            }
        }
        catch (Exception ee)
        {
            sendIsError(ee);
        }

    }
    protected void inactivate_closedpo_lines(ref int i)
    {
        // Task 1575: Inactivate Lines against closed PO 
        // Run this before endhour
        using (var myConn = Toolbox.connect())
        {
            try
            {
                //Disable triggers
                Toolbox.doSQL_void(myConn, @"SET @disable_triggers = 1", new object[] { });

                // Update the inactive lines 
                i = Toolbox.doSQL_affectedrows(myConn,@"UPDATE po_details_current a LEFT JOIN poprog_header b ON b.poprog_id = a.po_details_poprog_id SET a.po_details_line_active = 0 WHERE b.poprog_status = 7 AND a.po_details_line_Active = 1;", new object[] { });


                //set disabled triggers to null
                Toolbox.doSQL_void(myConn,@"SET @disable_triggers = NULL", new object[] { });
            }
            catch (Exception ee)
            {
                sendIsError(ee);
            }
        }
    }



}
