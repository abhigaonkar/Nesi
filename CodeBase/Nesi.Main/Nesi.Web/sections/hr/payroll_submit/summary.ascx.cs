using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using nesi.core;
using DevExpress.Xpo;
using NESI.Common.Models;

public partial class sections_hr_payroll_submit_summary : System.Web.UI.UserControl
{
    public payroll.approval_packet packet { get; set; }
    public bool do_final_payroll_approval { get; set; }
    public payroll payroll { get; set; }
    private int unapproved_bonuses { get; set; }
    private bool can_see_wage { get; set; }
    private bool has_submitted = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Visible) return;
        load();
    }
    private void load()
    {
        using (var conn = Toolbox.connect())
        {
            if (packet != null && packet.payperiod_id > 0)
            {
                can_see_wage = packet.handler.AuthenticatedForPrivilege(OpsPrivilege.ViewLabourCostsOnWorkOrders);
                if (!can_see_wage)
                {
                    div_wage_priv.Visible = true;
                }
                var payperiod = new NePayPeriod(packet.payperiod_id);
                payroll = packet.current_employee != null
                                                ? new payroll { start_date = payperiod.StartDate, end_date = payperiod.Enddate, member_id = packet.current_employee.id32 }
                                                : new payroll { start_date = payperiod.StartDate, end_date = payperiod.Enddate };
                has_submitted = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM payroll_progress WHERE business_unit_id = @v0 AND member_id = @v1 AND payperiod_id = @v2 AND sent = 1", new object[] { packet.business_unit_id, packet.handler.id, packet.payperiod_id }) > 0;
                var hasEmployees = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM payroll_hours WHERE payperiod_id = @v2 AND business_unit_id = @v0 AND dept_head_id = @v1", new object[] { packet.business_unit_id, packet.handler.id, packet.payperiod_id }) > 0;
                bind(conn);
                unapproved_bonuses = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM payroll_extra_payments a LEFT JOIN member b ON a.member_id = b.member_id where b.business_unit_id = @v0 AND a.payperiod_Id = @v1 AND a.approved = -1", new object[] { packet.business_unit_id, packet.payperiod_id });
                do_final_payroll_approval = packet.handler.AuthenticatedForPrivilege(OpsPrivilege.FinalBranchSubmit);
                gv_notapproved.DataSource = packet.full_branch_list.Count > 0
                                                ? Toolbox.doSQL_dt(conn, string.Format(@"SELECT member_fullname name FROM member WHERE member_id in ({0}) ORDER BY member_fullname", string.Join(",", packet.full_branch_list)), null)
                                                : Toolbox.doSQL_dt(conn, "SELECT member_fullname name FROM member WHERE member_id = -1", null);
                gv_notapproved.DataBind();
                var unapproved_vacations = Toolbox.doSQL_dt(conn, @"
SELECT 
	DISTINCT IFNULL(b.member_fullname, a.member_id) name,
    IFNULL(c.member_fullname, b.payroll_handler) payroll_handler
FROM 
	vacation_master a 
INNER JOIN 
	member b ON a.member_id = b.member_id AND b.business_unit_id = @v0 
INNER JOIN 
	member c ON b.payroll_handler = c.member_id 
WHERE 
	a.payperiod_id = @v1 AND 
    a.status = 1", new object[] { packet.business_unit_id, packet.payperiod_id });
                div_unapproved_vacations.Visible = unapproved_vacations.Rows.Count > 0;
				if(div_unapproved_vacations.Visible)
					{
                    div_unapproved_employees.InnerHtml = "<u>Outstanding Employees:</u>";
                    foreach(DataRow dr in unapproved_vacations.Rows)
						{
                        div_unapproved_employees.InnerHtml += $"<div>{dr["name"]} - Handler: {dr["payroll_handler"]}</div>";
						}
					}
                gv_withouthours.DataSource = Toolbox.doSQL_dt(conn, @"SELECT member_fullname name FROM member WHERE business_unit_id = @v0  AND member_status = 'Active' AND paytype_id != 3 AND member_id NOT IN (SELECT membertime_memberid FROM membertime WHERE date BETWEEN @v1  and @v2 ) ORDER BY member_fullname", new object[] { packet.business_unit_id, payroll.start_date, payroll.end_date });
                gv_withouthours.DataBind();
                bt_finalsubmit.Text = !do_final_payroll_approval
                                        ? bt_finalsubmit.Text.Replace("Complete Payroll", "Complete Your Employee's Payroll")
                                        : bt_finalsubmit.Text.Replace("Complete Payroll", "Complete Business Unit's Payroll");
                var waitingOnOtherHandlers = Toolbox.doSQL_string(conn, @"
SELECT 
	IFNULL(GROUP_CONCAT(DISTINCT a.dept_head_id), '') 
FROM 
	payroll_hours a 
LEFT JOIN 
	payroll_progress b ON b.payperiod_id = @v0 AND b.business_unit_id = a.business_unit_id AND a.dept_head_id = b.member_id
WHERE 
	a.payperiod_id = @v0 AND 
	a.business_unit_id = @v1 AND 
	a.dept_head_id != @v2 AND
	b.sent = FALSE", new object[] { packet.payperiod_id, packet.business_unit_id, packet.handler.id });
                
                if (!has_submitted &&
                        (packet.full_branch_list.Count == 0 && do_final_payroll_approval && unapproved_bonuses == 0 && unapproved_vacations.Rows.Count == 0 // If this is a person with final approval and the full branch list is 0
                        ||
                        !do_final_payroll_approval && hasEmployees && packet.your_to_approve.Count == 0 && unapproved_vacations.Rows.Count == 0 && unapproved_bonuses == 0)) // This person can't do final approval and they have employees, and their unapproved employees is zero, and all vacations & bonuses are submitted
                {
                    bt_finalsubmit.ClientVisible = true;
                    bt_finalsubmit.Enabled = true;
                }
                else if (do_final_payroll_approval && packet.your_to_approve.Count == 0 && waitingOnOtherHandlers != "")
                {
                    bt_finalsubmit.ClientVisible = true;
                    bt_finalsubmit.Enabled = false;
                    bt_finalsubmit.ToolTip = packet.full_branch_list.Count == 0
                                                ? "There are still handlers that haven't submitted their payrolls"
                                                : "There are employees still to be handled in this business unit";
                    if(packet.full_branch_list.Count == 0)
                    { 
                    FillOtherApprovers(conn);
                    }
                }
                else if(do_final_payroll_approval)
                {
                    bt_finalsubmit.ClientVisible = true;
                }
                div_unapproved_bonuses.Visible = unapproved_bonuses > 0;
            }
        }
    }
    private void bind(MySqlConnection _conn)
    {
        var reports_to_str = Toolbox.doSQL_string(_conn, @"SELECT REPORTS_TO(@v0);", new object[] { packet.handler.id });
        var payroll_handler_str = Toolbox.doSQL_string(_conn, @"SELECT GROUP_CONCAT(member_id) FROM member WHERE business_unit_id = @v1 AND payroll_handler = @v0", new object[] { packet.handler.id, packet.business_unit_id });
        if (reports_to_str == "") return;
        var reports_to_list = reports_to_str.Split(',').Select(int.Parse).ToList();
        var payroll_handler_list = string.IsNullOrEmpty(payroll_handler_str) ? new List<int>() : payroll_handler_str.Split(',').Select(int.Parse).ToList();

        var dt = Toolbox.doSQL_dt(_conn, @"CALL ds_payroll_approval_summary(@v0 , @v1 , @v2 )", new object[] { packet.payperiod_id, packet.business_unit_id, packet.handler.id });
        if (!can_see_wage)
        {
            foreach (DataRow dr in dt.Rows)
            {
                var member_id = Convert.ToInt32(dr["member_id"]);
                if (!reports_to_list.Contains(member_id))
                {
                    dr.Delete();
                }
                else
                {
                    dr["wage"] = 0;
                    dr["total"] = 0;
					dr["expense_total"] = 0;
					dr["commission"] = 0;
					dr["bonus"] = 0;
					dr["banked_w"] = Toolbox.ReturnZeroIfNull_double(dr["banked_w_hours"]);
                    dr["banked_d"] = Toolbox.ReturnZeroIfNull_double(dr["banked_d_hours"]);
                    dr["banked_p"] = Toolbox.ReturnZeroIfNull_double(dr["banked_p_hours"]);
                }
            }
        }
        else
        {
            foreach (DataRow dr in dt.Rows)
            {
                var member_id = Convert.ToInt32(dr["member_id"]);
                if (!reports_to_list.Contains(member_id) && !payroll_handler_list.Contains(member_id))
                {
                    dr.Delete();
                }
                else
                {
                    dr["banked_w"] = Toolbox.ReturnZeroIfNull_double(dr["banked_w"]);
                    dr["banked_d"] = Toolbox.ReturnZeroIfNull_double(dr["banked_d"]);
                    dr["banked_p"] = Toolbox.ReturnZeroIfNull_double(dr["banked_p"]);
                }
            }
        }
        gv_summary.DataSource = dt;
        gv_summary.Columns["Edit"].Visible = !packet.is_sent && !has_submitted;
        gv_summary.DataBind();
    }
    protected void gv_summary_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
    {
        var col_name = e.DataColumn.Name;
        if (e.VisibleIndex > -1 && e.CellValue != null && e.CellValue.ToString() == "0")
        {
            e.Cell.ForeColor = Color.LightGray;
        }
    }
    protected void bt_edit_Init(object sender, EventArgs e)
    {
        var bt = (ASPxButton)sender;
        var cell = (GridViewDataItemTemplateContainer)bt.NamingContainer;
        var grid = cell.Grid;
        var dr = grid.GetDataRow(cell.VisibleIndex);
        if (dr == null) return;
        var id = dr["id"];
        var member_id = dr["member_id"];
        bt.ClientSideEvents.Click = string.Format("function(s,e){{if(confirm('Are you sure you wish to clear out this entry?')){{bt_finalsubmit.SetVisible(false);pc.SetActiveTabIndex(0);approval.clear(s,e,{0},{1});}}}}", id, member_id);
    }
    private void FillOtherApprovers(MySqlConnection _conn)
    {
        var payperiod   = new NePayPeriod(packet.payperiod_id);
            var handlers = Toolbox.doSQL_dt(_conn, @"
SELECT 
    DISTINCT payroll_handler 
FROM 
    member 
WHERE 
    business_unit_id = @v0 AND 
    payroll_handler != @v1 AND 
    (member_status = 'Active' OR member_termdate > @v2) AND 
    payroll_handler NOT IN (SELECT member_id FROM payroll_progress WHERE business_unit_id = @v0 AND payperiod_id = @v3 AND sent = 1)", new object[]{packet.business_unit_id, packet.handler.id, payperiod.Enddate, payperiod.id });  

            if(handlers.Rows.Count > 0)
            {
            div_other_approvers.InnerHtml = "This unit's payroll is currently waiting on payroll to be submitted from the following handler(s):<br/><br/>";
           foreach(DataRow h in handlers.Rows)
            {
            var handler = new NeMember((int) h["payroll_handler"]);
            div_other_approvers.InnerHtml   += handler.FullName;
            }
            div_other_approvers.InnerHtml +="<br/>";
            }
            div_other_approvers.Visible = handlers.Rows.Count > 0;
        
    }
    protected void cbp_summary_Callback(object sender, CallbackEventArgsBase e)
    {
        using (var conn = Toolbox.connect())
        {

            if (e.Parameter == "submit")
            {
                var alertText = "";
                var branch = new NeBusinessUnit(packet.business_unit_id);
                var hasThisUserSentPayroll = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM payroll_progress WHERE payperiod_id = @v0 AND business_unit_id = @v1 AND member_id = @v2", new object[] { packet.payperiod_id, packet.business_unit_id, packet.handler.id }) > 0;
                if (hasThisUserSentPayroll)
                {
                    Toolbox.doSQL_void(@"UPDATE payroll_progress SET sent = 1 WHERE payperiod_id = @v0 AND business_unit_id = @v1 AND member_id = @v2", new object[] { packet.payperiod_id, packet.business_unit_id, packet.handler.id });
                }
                else
                {
                    Toolbox.doSQL_void(@"INSERT INTO payroll_progress (payperiod_id, business_unit_id, member_id, sent) VALUES (@v0, @v1, @v2, 1)", new object[] { packet.payperiod_id, packet.business_unit_id, packet.handler.id });
                }

                if (packet.full_branch_list.Count == 0)
                {
                    if (do_final_payroll_approval)
                    {
                        // Are there any other approvers for this BU?
                        var dtOtherApprovers = Toolbox.doSQL_dt(conn, @"SELECT DISTINCT(dept_head_id) handler_id FROM payroll_hours WHERE payperiod_id = @v0 AND business_unit_id = @v1 AND dept_head_id != @v2", new object[] { packet.payperiod_id, packet.business_unit_id, packet.handler.id });
                        var canPushPayroll = true;
                        var waitingOn = new List<string>();
                        nesi.core.payroll.finalize(packet.business_unit_id, packet.payperiod_id, packet.handler.id32, false);
                        if (dtOtherApprovers.Rows.Count > 0)
                        {
                            foreach (DataRow drHandlers in dtOtherApprovers.Rows)
                            {
                                // Only allow payroll to process if every other handler has sent their payroll
                                var thisHandlerId = Convert.ToInt32(drHandlers["handler_id"]);
                                var thisHandler = new NeMember(thisHandlerId);
                                var hasSentPayroll = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM payroll_progress WHERE payperiod_id = @v0 AND business_unit_id = @v1 AND member_id = @v2 AND sent = 1", new object[] { packet.payperiod_id, packet.business_unit_id, thisHandlerId });
                                if (hasSentPayroll == 0)
                                {
                                    canPushPayroll = false;
                                    waitingOn.Add(thisHandler.FullName);
                                    var handlerEMail = new NeEMail
                                    {
                                        To = thisHandler.NEEmail,
                                        From = "noreply@" + Toolbox.app_setting("DomainForEmail"),
                                        Subject = string.Format("The payroll for {0} is waiting for you to finalize your payroll.", branch.name),
                                        Body = string.Format("{0} just submitted their payroll for {1}, and in order to fully process this business unit's payroll, you will need to login and finalize your employees payroll.", packet.handler.FullName, branch.name)
                                    };
                                    handlerEMail.Send();
                                }
                            }
                        }
                        if (canPushPayroll)
                        {
                            nesi.core.payroll.finalize(packet.business_unit_id, packet.payperiod_id, packet.handler.id32, true);
                            shared.alert_payroll(string.Format("{0} has submitted payroll for {1}", packet.handler.FullName, branch.name), "Please login to check their payroll.");
                            bt_finalsubmit.ClientVisible = false;
                            gv_summary.Columns["Edit"].Visible = false;
                            alertText = "Successfully performed final approval for business unit... refreshing.";
                        }
                        else
                        {
                            alertText = "Cannot finalize payroll, still waiting on the following handler(s):" + string.Join("\n- ", waitingOn);
                        }
                    }
                    else
                    {
                        // Need to alert the BM that their payroll is available to submit.
                        var branch_em = new NeEMail
                        {
                            To = branch.branch_manager.NEEmail,
                            From = "noreply@" + Toolbox.app_setting("DomainForEmail"),
                            Subject = string.Format("The payroll for {0} is ready for final approval.", branch.name),
                            Body = "Please log in and go over the payroll data for this branch.",
                            Bcc = "mhyde@" + Toolbox.app_setting("DomainForEmail")
                        };
                        branch_em.Send();
                        if (branch.branch_manager.id != packet.handler.id)
                        {
                            alertText = "Successfully performed your approval for your employees... An email has been sent to " + branch.branch_manager.FullName + " for final approval.";
                        }
                        else
                        {
                            alertText = "Successfully performed your approval for your employees... It appears that you are the business unit manager, and should have the privilege for final approval, though don't currently... please contact support regarding this.";
                        }
                    }
                }
                else
                {
                    shared.alert_payroll(string.Format("{0} has submitted payroll for {1} (There are more people to be approved)", packet.handler.FullName, branch.name), "The final payroll hasn't been submitted, this is just to notify you that this person is done with their payroll.");
                    alertText = "Successfully performed your approval for your employees... Though there are more employees that need to be approved before final approval can occur.";
                }
                cbp_summary.JSProperties["cpDoJS"] = "alert(\"" + alertText + "\");please_wait('start', 'Refreshing');location.href = location.href;";
            }
            else if (e.Parameter == "refresh")
            {
                load();
            }
        }
    }
}