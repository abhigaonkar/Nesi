using System.Drawing.Printing;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System;
using System.Data;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using MySql.Data.MySqlClient;
using nesi.core;
using Document = iTextSharp.text.Document;
using Page = System.Web.UI.Page;
using NESI.Common.Models;
using System.Web;
using NESI.BLL.Common.Cache;
using NESI.BLL.Common.Shared;
using System.Collections.Specialized;
using NESI.Common.Templates;
using NESI.BLL.EmbeddedFiles;
using NESI.BLL.Core.User;
using NESI.BLL.EmbeddedResources;
using Nesi.Web.sections.workorder;
using System.Net;

// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable LocalVariableHidesMember
// ReSharper disable PossibleNullReferenceException
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable All

public partial class sections_workorder : Page
{
    NeMember user;
    public string IshowCloseandReassign = "display:none";


    private Dictionary<int, List<DateTime>> AssetUsage = new Dictionary<int, List<DateTime>>();
    private bool CanApproveBM,
                 CanApprovePM,
                 CanApproveDM,
                 CanViewCost,
                 CanViewGross,
                 CanAddWorkOrder,
                 CanPutWoOnHold,
                 CanPMAdjustBDM,
                 CanViewDollarTotalsAllBranches,
                 CanSeeInvoicePreviewButton,
                 CanCreateCreditRebillWO,
                 CanViewCustomers,
                 _same_dept,
                 SkipDateRestrictions,
                 CanEditBDM,
                CanToggleInvoiceIssues,
                CanEditActingBDM
                 ;

    public bool comment_hide = true;
    public bool has_open_childs = true;
    string _strprint = "";
    NeWOProg _wo = new NeWOProg();
    public string order_number;
    double _quote_percent;
    double _quote_flatamount;
    double _totaltobepb;
    bool show_custom_billing_columns;

    string _fromquoteid = "";
    DataTable _part_history_dt = new DataTable { TableName = "part_history" };
    DataTable _max_part_history_dt = new DataTable { TableName = "max_part_history" };

    private static MemoryCacher<object> LoggedWorkOrderList = new MemoryCacher<object>();

    protected void Page_Init(object _sender, EventArgs _e)
    {
        var q = Request.QueryString;
        var workOrder = string.IsNullOrEmpty(q["woprog_id"]) ? "0" : q["woprog_id"];
        if (string.IsNullOrEmpty(q["is_n1"]))
        {
            if (Request.UrlReferrer == null && string.IsNullOrEmpty(q["override_member_id"]))
            {
                Response.Redirect("/#/home/0/" + Server.UrlEncode(string.Format("/sections/workorder/index.aspx?woprog_id={0}&business_unit_id={1}", q["woprog_id"], q["business_unit_id"])));
            }
        }

        user = Toolbox.do_handle_authentication(OpsPage.WorkOrders);

        CanApproveBM = user.AuthenticatedForPrivilege(OpsPrivilege.ApprovalBm);
        CanApproveDM = user.AuthenticatedForPrivilege(OpsPrivilege.ApprovalDm);
        CanApprovePM = user.AuthenticatedForPrivilege(OpsPrivilege.ApprovalPm);
        CanAddWorkOrder = user.AuthenticatedForPrivilege(OpsPrivilege.AddWo);
        CanPMAdjustBDM = user.AuthenticatedForPrivilege(OpsPrivilege.CanPMAdjustBDM);
        CanToggleInvoiceIssues = user.AuthenticatedForPrivilege(OpsPrivilege.ToggleInvoiceIssues);
        CanViewCost = user.AuthenticatedForPrivilege(OpsPrivilege.ViewCostInformationOnWorkOrders);
        CanViewGross = user.AuthenticatedForPrivilege(OpsPrivilege.ViewGrossMarginInformation);
        CanPutWoOnHold = user.AuthenticatedForPrivilege(OpsPrivilege.PutWorkOrdersOnHold);
        CanViewDollarTotalsAllBranches = user.AuthenticatedForPrivilege(OpsPrivilege.ViewDollarTotalsAllBranches);
        CanSeeInvoicePreviewButton = user.AuthenticatedForPrivilege(OpsPrivilege.InvoicePrintPreviewButton);
        CanCreateCreditRebillWO = user.AuthenticatedForPrivilege(OpsPrivilege.CreditAndRebillPrivilege);
        CanViewCustomers = user.AuthenticatedForPage(OpsPage.Customer);
        SkipDateRestrictions = user.AuthenticatedForPrivilege(OpsPrivilege.BackdateEndStartDates);
        CanEditActingBDM = _wo.intProjectManager == user.id && CanPMAdjustBDM;
        CanEditBDM = user.AuthenticatedForPrivilege(OpsPrivilege.ManageSalesReps) || CanEditActingBDM;
        CanEditActingBDM = CanEditBDM || CanEditActingBDM; // MH: Adding this as there is a possibility of CanEditBDM being false though they are the PM on the WO

        populate_ddlCompany(false);
        if (!CanAddWorkOrder)
        {
            row_expected_sales.Style["display"] = "none";
            row_expected_labour.Style["display"] = "none";
            txtSalesValue.ClientVisible = false;
            txtlaborvalue.ClientVisible = false;
        }
        txtwhyCloseReassign.Visible = CanApproveBM;
        btnCloseandReassign.Visible = CanApproveBM;
        if (!shared.properties.exists("remove_close_reassign"))
            IshowCloseandReassign = "display:table-row";
        //this
        gv_parthistory.Columns["wo_detail_current_price_cost"].Visible = CanViewCost;
        gv_parthistory.Columns["wo_detail_current_price_sell"].Visible = CanViewGross;
        ddl_parent_workorder.ClientSideEvents.SelectedIndexChanged = !user.is_backoffice && user.MemberTypeID != OpsMemberTypes.BranchManager
            ? @"function(s,e){
                txtWODescription.SetText('Parent WO: ' + s.GetText());
			  if(s.GetValue() == 1)
                {alert('Only for branch managers and office staff, undoing selection...');s.SetSelectedIndex(0);}}"
            : @"function(s,e) {txtWODescription.SetText('Parent WO: ' + s.GetText());}";
        using (var conn = Toolbox.connect())
        {
            if (sdsAssetUsage.SelectParameters.Count == 0)
            {
                sdsAssetUsage.SelectParameters.Add(new Parameter { Name = "@woprog_id", DefaultValue = workOrder });
                gvAssetUsage.DataBind();
            }
            if (workOrder != "0")
            {
                _wo = new NeWOProg(Convert.ToInt32(workOrder));


                var table = _wo.Status.Contains(OpsWOStatus.Invoiced) ? "wo_detail_history" : "wo_detail_current";
                sdsAssetUsage.SelectCommand = $@"SELECT 
    a.{table}_id id, 
	DATE(a.{table}_date_added) date_added,
    a.{table}_qty_committed qty, 
    a.{table}_price_sell sell, 
    IF(IFNULL(a.asset_unit, 1) = 1, 'Daily', IF(a.asset_unit = 2, 'Weekly', 'Monthly')) as unit, 
    IF(IFNULL(a.asset_unit, 1) = 1, '1', IF(a.asset_unit = 2, '2', '3')) as unit_id,  
    a.{table}_description description,
    bt.wo_lineitem_billtype_name as billtype,
    a.{table}_billtypeid as billtype_id,
    a.{table}_origin as origin,
    a.activity_code,
    a.client_po,
    a.client_wo,
    a.cost_element
FROM 
    {table} a
INNER JOIN 
    wo_detail_lineitem_billtype bt on a.{table}_billtypeid = bt.wo_lineitem_billtypeid  
WHERE 
    a.{table}_woprog_id = @woprog_id AND 
    a.{table}_type = 'A' 
ORDER BY 
    a.{table}_id DESC";

                show_custom_billing_columns = Toolbox.doSQL_bool(@"SELECT show_custom_billing_columns FROM business_unit WHERE id = @v0", new object[] { _wo.business_unit_id });

                if (show_custom_billing_columns)
                {
                    gvAssetUsage.Columns["activity_code"].Visible = true;
                    gvAssetUsage.Columns["client_wo"].Visible = true;
                    gvAssetUsage.Columns["client_po"].Visible = true;
                    gvAssetUsage.Columns["cost_element"].Visible = true;
                }
                if (table == "wo_detail_history")
                {
                    gvAssetUsage.Columns["Action"].Visible = false;
                }
                else
                {
                    sdsAssetUsage.DeleteCommand = $@"DELETE FROM wo_detail_current WHERE wo_detail_current_id = ? LIMIT 1";
                    var dtAssetUsage = Toolbox.doSQL_dt(conn, "SELECT assets_id id, group_concat(b.date_needed) dates_needed FROM assets a LEFT JOIN woprog_asset b ON a.assets_id = b.asset_id LEFT JOIN woprog c ON b.woprog_id = c.woprog_Id WHERE a.business_unit_id = @v0 AND active = 1 GROUP BY a.assets_id", new object[] { _wo.business_unit_id });
                    foreach (DataRow dr in dtAssetUsage.Rows)
                    {
                        var assetId = (int)dr["id"];
                        var strDatesNeeded = Toolbox.ReturnBlankIfNull_string(dr["dates_needed"]);
                        var datesNeeded = strDatesNeeded == ""
                                            ? new List<DateTime>()
                                            : strDatesNeeded.Split(',').Select(DateTime.Parse).ToList();
                        AssetUsage.Add(assetId, datesNeeded);
                    }
                }
            }

        }
        ScriptManager1.RegisterPostBackControl(btn_export);
    }

    private void DataBindForCustomer(ASPxComboBox ddl, MySqlConnection conn, string poprogid, string bu, int customerId)
    {
        var sp = "CALL ds_workorder_customers_withMapping(@v0, @v1, @v2)";
        ddl.DataSource = Toolbox.doSQL_dt(conn, sp, new object[] { poprogid, bu, customerId });
        ddl.DataBind();
        if (customerId > 0)
        {
            ddl.Value = customerId;
        }
    }

    private void DataBindForRevenueLine(string bu, int revenueLineId)
    {
        var query = @"
SELECT DISTINCT
  rl.revenue_line_id id,
  rl.Name name
FROM
  revenue_line_business_unit map
  INNER JOIN revenue_line rl
    ON rl.revenue_line_id = map.revenue_line_id AND rl.Active = 1
WHERE map.business_unit_id = @v0;";

        DataTable dt = Toolbox.doSQL_dt(query, new object[] { bu });

        DataView dv = new DataView(dt);

        dv.RowFilter = "id=" + Toolbox.ReturnZeroIfNull_int(revenueLineId).ToString();   // to check revenueLineId exist in Database  or not

        ddlRevenueLines.DataSource = dt;

        ddlRevenueLines.DataBind();

        ddlRevenueLines.Value = dv.Count == 1 ? revenueLineId : 0;

    }

    protected void Page_Unload(object _sender, EventArgs _e)
    {
    }
    protected void Page_Load(object _sender, EventArgs _e)
    {

        if (!SkipDateRestrictions)
        {

            dteExpEndDate.MinDate = dteStartDate.Date;
        }

        try
        {

            //if (remove_close_reassign != null)
            //this.remove_close_reassign.Visible = !shared.properties.exists("remove_close_reassign");
            using (var conn = Toolbox.connect())
            {
                var customerId = 0;
                var contactId = 0;
                var revenueId = 0;
                var j_son = new JavaScriptSerializer();
                var q = Request.QueryString;
                var f = Request.Form;
                Toolbox.do_add_css(Page, "~/css/workorder.css");
                var lbltemp = (Label)Page.Master.FindControl("lblHeading");
                var woprog_id = string.IsNullOrEmpty(q["woprog_id"]) ? "0" : q["woprog_id"];
                _fromquoteid = string.IsNullOrEmpty(q["fromquoteid"]) ? "" : q["fromquoteid"];
                header.Visible = false;
                var business_unit_id = string.IsNullOrEmpty(q["business_unit_id"]) ? user.business_unit_id.ToString() : q["business_unit_id"];
                if (business_unit_id == "0" && (woprog_id == "9999999" || woprog_id == "9999998" || woprog_id == "9999997"))
                {
                    Response.Redirect("/sections/purchaseorder/NoWOWarning.aspx", true);
                }
                if (business_unit_id == "0" && woprog_id != "0" && string.IsNullOrEmpty(q["a"]) && woprog_id != "")
                {
                    var try_woprog_id = 0;
                    int.TryParse(woprog_id, out try_woprog_id);
                    if (try_woprog_id == 0)
                    {
                        Toolbox.FriendlyException(Response, "This is not a valid work order.", "/default.aspx");
                    }
                    // Check if exists
                    if (try_woprog_id < 20000 || try_woprog_id > 20000 && Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM woprog  WHERE woprog_id =@v0", new object[] { try_woprog_id }) != 1)
                    {
                        Toolbox.FriendlyException(Response, "The requested work order does not exist.", "/default.aspx");
                    }
                    business_unit_id = Toolbox.doSQL_string(conn, @"SELECT business_unit_id FROM woprog  WHERE woprog_id =@v0 limit 1 ", new object[] { woprog_id });
                    Response.Redirect("/sections/workorder/index.aspx?woprog_id=" + woprog_id + "&business_unit_id=" + business_unit_id, true);
                }
                if (hidWOProgID.Value == "")
                {
                    hidWOProgID.Value = woprog_id;
                    lbltemp.Text = "New Work Order";
                }
                if (hidCompanyID.Value == "")
                {
                    hidCompanyID.Value = business_unit_id;
                }
                _wo.business_unit_id = Convert.ToInt32(hidCompanyID.Value);
                _wo.QuoteID = "0";
                lblStatus.Text = "";
                lblWODisplay.Text = "";
                lbl_ToBeInvoiced.Text = "Balance to Invoice: $ 0.00";

                if (_fromquoteid != "")
                {
                    _wo.QuoteID = _fromquoteid;
                }
                if (hidWOProgID.Value != "0")  // if it's not a new work order... a new work order has a wo_prog of 0.
                {
                    header.Visible = true;

                    #region Existing Work Order
                    var this_woprog_id = Convert.ToInt32(hidWOProgID.Value);
                    if (this_woprog_id > 0 && IsCallback && pc_main.ActiveTabIndex == 6)
                    {
                        fill_parthistory(this_woprog_id);
                    }
                    if (pc_main.TabIndex == 4)
                    {
                        fill_parthistory(this_woprog_id);
                    }
                    if (this_woprog_id < 20000 || this_woprog_id > 20000 && Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM woprog  WHERE woprog_id =@v0", new object[] { this_woprog_id }) != 1)
                    {
                        Toolbox.FriendlyException(Response, "The requested work order does not exist.", "/default.aspx");
                    }
                    _wo = new NeWOProg(this_woprog_id);
                    if (!new Current_User().visible_business_units.Split(',').Contains(_wo.business_unit_id.ToString()))
                    {
                        Toolbox.FriendlyException(Response, "The requested work order is not within your visible business units.", "/default.aspx");
                    }
                    customerId = _wo.WOProg_Customer_ID;
                    contactId = _wo.woprog_Contact_ID;
                    revenueId = _wo.revenue_line_id;
                    if (sdsAssetAssignment.SelectParameters.Count == 0)
                    {
                        sdsAssetAssignment.SelectParameters.Add(new Parameter { Name = "@businessUnitId", DefaultValue = _wo.business_unit_id.ToString() });
                    }
                    if (sdsAssetAssignmentGv.SelectParameters.Count == 0)
                    {
                        sdsAssetAssignmentGv.SelectParameters.Add(new Parameter { Name = "@woprog_id", DefaultValue = _wo.woprog_id.ToString() });
                    }
                    else
                    {
                        gv_assetAssignment.DataBind();
                    }
                    if (sdsAssetUsageAsset.SelectParameters.Count == 0)
                    {
                        sdsAssetUsageAsset.SelectParameters.Add(new Parameter { Name = "@woprog_id", DefaultValue = _wo.woprog_id.ToString() });
                    }
                    gvAssetUsage.DataBind();
                    if ((!CanApproveBM && !CanApprovePM) || _wo.Status.Contains(OpsWOStatus.Invoiced) || _wo.Status == OpsWOStatus.ClosedReassigned || _wo.Status == OpsWOStatus.Deleted)
                    {
                        // Lockdown assets tab
                        gvAssetUsage.Enabled = false;
                        gv_assetAssignment.Enabled = false;
                        txtAssetUsageQuantity.Enabled = false;
                        txtAssetUsageSell.Enabled = false;
                        calAssetDate.Enabled = false;
                        btAssetUsageSave.Enabled = false;
                        btSaveAsset.Enabled = false;
                        ddlAssetAssignment.Enabled = false;
                        ddlAssetUsageAsset.Enabled = false;
                        ddlAssetUsageUnit.Enabled = false;
                    }
                    _same_dept = _wo.business_unit_id == user.business_unit_id;
                    has_open_childs = false;
                    if (NeWOProg.get_child_workorders(_wo.woprog_id).Rows.Count > 0)
                    {
                        var drs = NeWOProg.get_child_workorders(_wo.woprog_id).Select("woprog_status<>'Invoiced' and woprog_status<>'Deleted' and woprog_status<>'Waiting To Be Invoiced'");
                        has_open_childs = drs.Any();
                    }
                    pc_main.TabPages[WorkOrderTabPage.Scope].ClientEnabled = true;  // scope tab
                    wo_tasklist1.woprog_id = _wo.woprog_id;
                    wo_tasklist1.DataBind();
                    btnViewScan.Disabled = true;
                    btnViewScan.Attributes["title"] = "Work Order is not Scanned";
                    if (!Toolbox.Contains(_wo.Status, new[] { OpsWOStatus.Open, OpsWOStatus.Deleted }))
                    {
                        pc_main.TabPages[WorkOrderTabPage.Scope].ClientVisible = true;
                        string pdfInternalPath = "";
                        string pdfExternalPath = "";
                        string pdfReason = "";
                        var externalPath = NeTaxEntity.BaseFolder(_wo.business_unit_id, true);
                        var internalPath = NeTaxEntity.BaseFolder(_wo.business_unit_id, false);
                        if ((_wo.woprog_scanned_date.Year > 2000 || _wo.OrderNumber == "") && _wo.Status != OpsWOStatus.Invoiced)
                        {
                            pdfInternalPath = $"{internalPath}/WOs/{_wo.woprog_id}.pdf";
                            pdfExternalPath = $"{externalPath}/WOs/{_wo.woprog_id}.pdf";
                        }
                        else
                        {
                            pdfInternalPath = $"{internalPath}/WOs/Invoiced/{_wo.woprog_id}.pdf";
                            pdfExternalPath = $"{externalPath}/WOs/Invoiced/{_wo.woprog_id}.pdf";
                        }
                        var pdfScanExists = File.Exists(pdfInternalPath);
                        if (pdfScanExists)
                        {
                            btnViewScan.Disabled = false;
                            btnViewScan.Attributes["onclick"] = $"boing('{pdfExternalPath}', 'woscan{_wo.woprog_id}', 600, 845);return false;";
                        }

                        btnViewScan.Attributes["title"] = !btnViewScan.Disabled ? "" : "Scan doesn't exist";
                    }

                    lbltemp.Text = "Work Order: " + _wo.OrderNumber + " - " + _wo.CustomerName + "<br/>" + _wo.Description.Replace("\n", "<br/>");
                    if (user.isContact && _wo.woprog_vis_to_cust == 0)
                    {
                        Response.Redirect("~/default.aspx", true);
                    }
                    set_js_confirm(_wo);
                    ddlCompany.Enabled = false;
                    hidWOStatus.Value = _wo.Status;
                    Session["ERID"] = _wo.woprog_ERID.ToString();
                    if (Session["ERID"].ToString() == "")
                    {
                        Session["ERID"] = "0";
                    }
                    set_tabs();
                    if (_wo.Status != OpsWOStatus.Invoiced && _wo.Status != OpsWOStatus.WaitingToBeInvoiced)
                    {
                        lbCommentPopup.Visible = true;
                        lbCommentPopup.Text = @"<a href='javascript:boing(""../../sections/workorder/wocomments.aspx?woid=" + _wo.woprog_id + @"&hold=0"", ""WorkOrderComments"", 675, 525);'>Comments</a>";
                    }
                    else
                    {
                        lbCommentPopup.Visible = false;
                    }

                    if (_wo.Status != OpsWOStatus.Invoiced && _wo.Status != OpsWOStatus.WaitingToBeInvoiced)
                    {
                        // How many shared comment record have now.
                        var count = Toolbox.doSQL_int(
                            @"SELECT count(1) FROM vwwocomments  where woprog_id =@v0 and member_id = 0",
                            new object[] { _wo.woprog_id });

                        if (count > 1)
                        {
                            lbCommentFixup.Visible = true;
                            lbCommentFixup.Text = @"<a href='javascript:boing(""../../sections/workorder/comments_fixup.aspx?woid=" + _wo.woprog_id + @"&hold=0"", ""WorkOrderCommentsFixup"", 780, 590);'>  Multiple timesheet comment summaries detected, click here to fix.</a>";
                        }
                        else
                        {
                            lbCommentFixup.Visible = false;
                        }
                    }

                    if (_wo.Status != OpsWOStatus.Invoiced && _wo.Status != OpsWOStatus.Open && _wo.Status != OpsWOStatus.WaitingToBeInvoiced)
                    {
                        if (Toolbox.MySQL_shortdt(_wo.woprog_scanned_date) == "2005-12-01" || _wo.woprog_scanned_date == null || Toolbox.MySQL_shortdt(_wo.woprog_scanned_date) == "")
                        {
                            lblDaysSinceScan.Text = "This has not been scanned.";
                        }
                        else
                        {
                            var i = (DateTime.Now.Date - _wo.woprog_scanned_date.Date).Days;
                            lblDaysSinceScan.Text = i + " day(s) since being scanned ";
                            lblDaysSinceScan.Text += i > 5 ? " !!" : i >= 10 ? " !!!" : "";
                        }
                        if (_wo.woprog_iscredit == 1)
                        {
                            lblDaysSinceScan.Text += " **CREDIT **";
                        }
                        else if (_wo.woprog_isrebill == 1)
                        {
                            lblDaysSinceScan.Text += " **REBILL **";
                        }
                    }
                    else
                    {
                        lblDaysSinceScan.Text = "";
                    }



                    #endregion Existing Work Order

                    uc_analysis._wo = _wo;
                    uc_analysis._current_user = user;

                    //
                    // Work order checking when it is associated with a quote.
                    //
                    CheckWorkorder(_wo);

                }
                else  // if its a new work order
                {
                    bt_delete_wo.Visible = false;
                    btnCloseandReassign.Visible = false;
                    txtwhyCloseReassign.Visible = false;
                    btn_refresh.Visible = false;
                    pc_main.TabPages[WorkOrderTabPage.LinkedWorkOrders].ClientVisible = false; // linked wos
                    pc_main.TabPages[WorkOrderTabPage.ProjectFolder].ClientVisible = false; // project folder
                    pc_main.TabPages[WorkOrderTabPage.Scope].ClientVisible = false; // scope tab
                    pc_main.TabPages[WorkOrderTabPage.Assets].ClientVisible = false; // assets tab
                    if (!CanAddWorkOrder)
                    {
                        Toolbox.FriendlyException(Response, "You do not have permission to cut work orders", "/default.aspx");
                    }
                    txtCustPO.Buttons[0].Visible = false;
                    if (IsPostBack && woprog_id == "0" && ddlCustomer.SelectedItem != null && lblError.Text == "Please Select a Customer")
                    {
                        lblError.Text = "";
                    }
                }
                if (hidWOProgID.Value != "0")
                {
                    update_header_totals();
                }
                if (hidWOProgID.Value != "0")
                {
                    var ts_temp = Convert.ToDateTime(Toolbox.doSQL_string(conn, @"SELECT woprog_ts FROM woprog  WHERE woprog_id =@v0", new object[] { hidWOProgID.Value }));
                    hid_ts.Value = ts_temp.Ticks.ToString();
                }
                if (f["action_"] != null)
                {
                    Response.Clear();
                    switch (f["action_"])
                    {
                        case "ltc_proc":
                            var rs = j_son.Deserialize<IList<row_notes>>(f["rs"]);
                            for (var i = 0; i < rs.Count; i++)
                            {
                                var id = rs[i].id;
                                var reason = rs[i].reason;
                                Toolbox.doSQL_void(conn, @"UPDATE wo_detail_current SET wo_detail_current_notes = CONCAT('A_',@v1,' |', IFNULL(wo_detail_current_notes,'')) WHERE wo_detail_current_id = @v0  LIMIT 1", new object[] { id, reason });
                            }
                            Response.Write("SUCCESS");
                            break;
                    }
                    Response.End();
                }
                if (q["a"] != null && woprog_id != "")
                {
                    switch (q["a"])
                    {
                        #region json_update_header
                        case "json_update_header":
                            Toolbox.do_set_plain_header(Response);
                            if (_wo.Description == null)
                            {
                                // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1515
                                // Bug 1515: check the null value for description, otherwise throw exception when selecting progressbill checkbox.
                                _wo.Description = "";
                            }

                            var feed = string.Format(@"{{
""Invoicelbl"":""{0:C2}"",
""GP"":""{1}"",
""Taxlbl"":""0"",
""status"":""{3}"",
""GP_TOTAL"":""{4}"",
""title"":""{5}"",
""description"":""{6}""
}}", lbl_ToBeInvoiced.Text, lbl_topmargin.Text, 0, _wo.Status, lbl_top_whole_job.Text, Server.HtmlEncode("Work Order: " + _wo.OrderNumber + " - " + _wo.CustomerName), (Regex.Replace(_wo.Description, @"\t|\r", "").Replace("\0", string.Empty)).Replace("\n", "<br/>"));
                            Response.Write(feed);
                            break;
                        #endregion json_update_header
                        #region json_update_confirm_variables
                        case "json_update_confirm_variables":
                            Toolbox.do_set_plain_header(Response);

                            var min_hr = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(wo_detail_current_qty_committed),0) FROM wo_detail_current  where wo_detail_current_woprog_id =@v0 and wo_detail_current_type = 'L'", new object[] { _wo.woprog_id });
                            var zer_cp = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current  where wo_detail_current_woprog_id =@v0 and wo_detail_current_qty_committed = 0 and wo_detail_current_origin like 'PO %'", new object[] { _wo.woprog_id });
                            var str_zer_cp = zer_cp > 0 ? Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(wo_detail_current_rec_no) FROM wo_detail_current  where wo_detail_current_woprog_id =@v0 and wo_detail_current_qty_committed = 0 and wo_detail_current_origin like 'PO %'", new object[] { _wo.woprog_id }) : "";
                            #region foot / meter check
                            var this_branch = new NeBusinessUnit(_wo.business_unit_id);
                            var this_emt = "";
                            var this_t90 = "";
                            var ftmetr = false;
                            if (this_branch.country == "CDN")
                            {
                                this_t90 = Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(a.master_id) FROM inventory_item_detail a LEFT JOIN inventory_item_master b ON a.master_id = b.master_id  WHERE a.attribute_value_id = 55 AND b.tag_id = 3"); // 55 equals the attribute_value_id for "t90/thhn" for the attribute "Wire", so I go directly to the source, inventory_item_detail and get all master_id's that have that attribute_value_id, 3 = the tag wire
                                this_emt = Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(a.master_id) FROM inventory_item_detail a LEFT JOIN inventory_item_master b ON a.master_id = b.master_id  WHERE a.attribute_value_id = 54 AND b.tag_id = 5"); // 54 equals the attribute_value_id for "EMT" for the attribute "Conduit", so I go directly to the source, inventory_item_detail and get all master_id's that have that attribute_value_id, 5 = the tag conduit
                                if (this_t90 != "" && this_emt != "")
                                {
                                    ftmetr = Toolbox.doSQL_int(string.Format(@"
SELECT 
	COUNT(*) 
FROM 
	wo_detail_current
WHERE 
	wo_detail_current_woprog_id = @v0 AND 
	(
		(wo_detail_current_master_id IN ({0}) AND wo_detail_current_description NOT LIKE '%\%2Fft%') OR 
		(wo_detail_current_master_id IN ({1}) AND wo_detail_current_description NOT LIKE '%\%2Fm%')
	)", this_emt, this_t90), new object[] { _wo.woprog_id }) > 0;
                                }
                            }
                            else
                            {
                                ftmetr = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_description LIKE '%%2Fm'", new object[] { _wo.woprog_id }) > 0;
                            }
                            #endregion foot / meter check

                            var tot_in = _wo.woprog_StillToBeBilled;
                            string quote_id = quote_id = string.IsNullOrEmpty(_wo.QuoteID.Trim()) || _wo.QuoteID == "0" || _wo.QuoteID.Length < 7 ? "" : _wo.QuoteID.Substring(0, 6);
                            var was_qb = false;
                            var tot_qb = 0.00;
                            try
                            {
                                if (quote_id != "")
                                {
                                    var q_info = Toolbox.doSQL_dt(@"SELECT IF(pricetype_id = 3, true, false) was_qb, quoted_price FROM quote_master  WHERE active_revision = true AND quote_id =@v0", new object[] { quote_id }).Rows[0];
                                    was_qb = Convert.ToBoolean(q_info["was_qb"]);
                                    tot_qb = was_qb ? Convert.ToDouble(q_info["tot_qb"]) : 0;
                                }
                            }
                            catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }

                            var confirm_variables = string.Format(@"{{
""min_hr"":{0},
""zer_cp"":{1},
""was_qb"":{2},
""tot_qb"":{3},
""tot_in"":{4},
""ftmetr"":{5},
""recs"":""{6}""
}}", min_hr,                         // {0}
                                zer_cp,                         // {1}
                                was_qb.ToString().ToLower(),    // {2}
                                tot_qb,                         // {3}
                                tot_in,                         // {4}
                                ftmetr.ToString().ToLower(),    // {5}
                                str_zer_cp //{6}
                                );
                            Response.Write(confirm_variables);
                            break;
                        #endregion json_update_header
                        #region get_ts
                        case "get_ts":
                            Toolbox.do_set_plain_header(Response);
                            if (!string.IsNullOrEmpty(q["woprog_id"]))
                            {
                                var ts = Convert.ToDateTime(Toolbox.doSQL_string(conn, @"SELECT woprog_ts FROM woprog  WHERE woprog_id =@v0", new object[] { q["woprog_id"] }));
                                Response.Write(ts.Ticks + "|" + Toolbox.MySQL_longdt(ts));
                            }
                            else
                            {
                                Response.Write("0");
                            }
                            break;
                        #endregion get_ts
                        #region update_ts
                        case "update_ts":
                            Toolbox.do_set_plain_header(Response);
                            if (!string.IsNullOrEmpty(q["id"]))
                            {
                                var id = 0;
                                int.TryParse(q["id"], out id);
                                var wo = new NeWOProg(id);
                                if (!Toolbox.Contains(wo.Status, new[] { OpsWOStatus.Invoiced, OpsWOStatus.Deleted }))
                                {
                                    Toolbox.doSQL_void(conn, @"SET @disable_triggers = 1;UPDATE woprog SET woprog_ts = NOW() WHERE woprog_id = @v0;SET @disable_triggers = NULL;", new object[] { id });
                                }
                                Response.Write("1");
                            }
                            else
                            {
                                Response.Write("Invalid Request");
                            }
                            break;
                        #endregion update_ts
                        #region less_than_cost
                        case "less_than_cost":
                            var lessthan_dt = Toolbox.doSQL_dt(conn, @" Select wo_detail_current_id id, wo_detail_current_master_id master_id, wo_detail_current_description descr, wo_detail_current_price_cost cost, wo_detail_current_price_sell sell FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_price_sell-wo_detail_current_price_cost < 0 AND IFNULL(wo_detail_current_notes, '') NOT LIKE 'A_%|%' ", new object[] { _wo.woprog_id });
                            if (lessthan_dt.Rows.Count > 0)
                            {
                                var sb = new StringBuilder();
                                sb.Append(@"	
<script type='text/javascript' src='/js/jquery-1.3.2.min.js'></script>
<script type='text/javascript' src='/js/jquery-ui-1.7.1.custom.min.js'></script>
<script type='text/javascript' src='/js/functions.js'></script>


<script>
	function proc_reasons()
		{
		var o					= [];
		$('#reasons .r').each(function()
			{
			var id			= $(this).attr('data-id');
			var reason		= $(this).find('input:text').val().trim();
			var oo			= {id:id,reason:reason};
			o.push(oo);
			});
		var query_string		=	{
									action_:		'ltc_proc', 
									rs:				JSON.stringify(o)
									};
		$.ajax( 
			{
			type:		'POST',
			cached:		false,
			url:		'./index.aspx',
			data:		query_string,
			dataType:	'text',
			beforeSend:	function()
							{
							please_wait('start');
							},
			success:	function(ret)
							{
							if(ret != 'SUCCESS')
								{
								alert(ret);
								}
							else
								{
								alert('Rows Updated, closing window');
								}
							},
			complete:	function()
							{
							please_wait('stop');
							window.opener.location.href = window.opener.location.href;
							window.close();
							}
			});
		}
	function save_reasons(obj)
		{
		var c			= $(obj).attr('data-rows');
		var i			= 0;
		$('#reasons .r').each(function()
			{
			var reason		= $(this).find('input:text').val();
			if(reason.trim() != '')
				{
				i++;
				}
			});
		if(c != i)
			{
			alert('Please fill out all reasons before saving');
			}
		else
			{
			proc_reasons();
			}
		}
</script>
<div style='font-size:12px;font-family:arial;width:730px;'>These parts are being sold at less than cost.<br/>
Please provide a reason explaining why these are less than cost.</div>
<table id='reasons' width='730' cellspacing='0' cellpadding='2' style='font-size:12px;font-family:arial;border:solid 1px #ccc;'>
<thead style='background-color:#000;color:#fff;'>
	<tr>
		<th width='100'>Master ID</th>
		<th width='100'>Cost</th>
		<th width='100'>Sell</th>
		<th>Reason</th>
	<tr>
</thead>
<tbody>");
                                foreach (DataRow dr in lessthan_dt.Rows)
                                {
                                    var id = dr["id"].ToString();
                                    var master_id = dr["master_id"].ToString();
                                    var cost = Convert.ToDouble(dr["cost"]);
                                    var sell = Convert.ToDouble(dr["sell"]);
                                    var descr = Toolbox.do_value_from(dr["descr"], false);
                                    sb.AppendFormat(@"
<tr class='r' data-id='{1}'>
	<td align='center' style='border-right:solid 1px #ccc;'>{0}</td>
	<td align='center' style='border-right:solid 1px #ccc;'>{3:C2}</td>
	<td align='center' style='border-right:solid 1px #ccc;'>{4:C2}</td>
	<td><input type='text' style='width:100%;'/></td>
<tr>
<tr>
	<td style='background-color:#eee;border-bottom:solid 1px #ccc;' colspan='4'><b>Description:</b> {2}</td>
</tr>", master_id, id, descr, cost, sell);
                                }
                                sb.AppendFormat(@"
</table>
<div align='right' style='width:730px'><button type='button' onclick='save_reasons(this)' style='width:100px' data-rows='{0}'><img src='/images/icon/icon[save].gif' align='absmiddle' /> Save</button></div>
", lessthan_dt.Rows.Count);
                                Response.Write(sb.ToString());
                            }
                            else
                            {
                                Toolbox.FriendlyException(Response, "No rows are less than cost", "window.close();");
                            }
                            break;
                            #endregion less_than_cost
                    }
                    Response.End();
                }
                var is_progress = Toolbox.doSQL_int(conn, @"SELECT count(*) FROM wo_detail_current  WHERE (wo_detail_current_billtypeid = 9 or wo_detail_current_billtypeid = 12) AND wo_detail_current_woprog_id =@v0", new object[] { _wo.woprog_id }) > 0;
                if (is_progress)
                {
                    if (_wo.woprog_id != 0)
                    {
                        lbl_pct_to_invoice.Text = "Left to Invoice: " +
                                                  Toolbox.doSQL_string(conn, @"SELECT FORMAT(100-woprog_progressbilled/woprog_quotedamount*100,2) FROM woprog  WHERE woprog_id =@v0", new object[] { _wo.woprog_id }) + "%";
                    }
                }
                #region delete button logic
                if (_wo.woprog_id != 0)
                {
                    var child_wos = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM woprog WHERE woprog_associate_woprog_id = @v0  OR parent_woprog_id = @v0  ", new object[] { _wo.woprog_id });
                    child_wos += Toolbox.doSQL_dt(conn, @"call change_workorders(@v0,0)", new object[] { _wo.woprog_id }).Rows.Count;
                    pc_main.TabPages[WorkOrderTabPage.LinkedWorkOrders].Text = "Linked WOs (" + child_wos + ")";
                }
                else
                {
                    pc_main.TabPages[WorkOrderTabPage.LinkedWorkOrders].ClientVisible = false;
                    pc_main.TabPages[WorkOrderTabPage.QuoteComparison].ClientVisible = false;
                }
                if (_wo.woprog_id != 0)
                {
                    var curr_wo_count = Toolbox.doSQL_dt(conn, @"SELECT CAST((SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0 ) AS UNSIGNED) curr, CAST((SELECT COUNT(*) FROM wo_detail_history WHERE wo_detail_history_woprog_id = @v0 ) AS UNSIGNED) hist", new object[] { _wo.woprog_id }).Rows[0];
                    var curr_wo_line_items = Convert.ToInt32(curr_wo_count["curr"]);
                    var hist_wo_line_items = Convert.ToInt32(curr_wo_count["hist"]);
                    var wo_po_links = Toolbox.doSQL_dt(conn, @" SELECT b.poprog_id, b.poprog_bvpo FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_woprog_id = @v0 and a.is_gl_account = false  AND po_details_part_no != 0 GROUP BY poprog_id, poprog_bvpo", new object[] { _wo.woprog_id });
                    var curr_wo_comm_lines = _wo.IsProgressBill ?
                                                        false
                                                        : curr_wo_line_items != 0 ? Toolbox.doSQL_double(conn, @"SELECT count(wo_detail_current_qty_committed) FROM wo_detail_current  WHERE wo_detail_current_qty_committed != 0 and wo_detail_current_woprog_id =@v0", new object[] { _wo.woprog_id }) != 0 : false;
                    var existing_children = Toolbox.doSQL_dt(conn, @"SELECT woprog_id FROM woprog WHERE parent_woprog_id = @v0", new object[] { _wo.woprog_id });
                    var cant_delete_reason = new List<string>();
                    #region In an elevated status or terminated status
                    if (Toolbox.Contains(_wo.Status, new[] {   OpsWOStatus.Invoiced,
                                                            OpsWOStatus.WaitingToBeInvoiced,
                                                            OpsWOStatus.Deleted,
                                                            OpsWOStatus.ClosedReassigned
                                                            }))
                    {
                        cant_delete_reason.Add("Work orders in the '" + _wo.Status + "' status can't be deleted");
                    }
                    #endregion In an elevated status or terminated status
                    #region Has historic line items
                    if (hist_wo_line_items > 0)
                    {
                        cant_delete_reason.Add("Work orders that have items in the history table can't be deleted");
                    }
                    #endregion Has historic line items
                    #region Has committed work order lines
                    if (curr_wo_comm_lines)
                    {
                        cant_delete_reason.Add("This work order has items committed on it that need to be removed BEFORE it can be deleted");
                    }
                    #endregion Has committed work order lines
                    if (wo_po_links.Rows.Count > 0)
                    {
                        var pos = "";
                        foreach (DataRow dr in wo_po_links.Rows)
                        {
                            var poprog_id = dr["poprog_id"];
                            var poprog_bvpo = dr["poprog_bvpo"];
                            pos += string.Format("<br/><a style='color:#ffc' href='/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}'>PO #:{1}</a>", poprog_id, poprog_bvpo);
                        }
                        cant_delete_reason.Add("This work order has open purchase order(s) with line items linked to it that need to be changed to a different work order or the line on PO needs to be deleted BEFORE this work order can be deleted:" + pos);
                    }

                    if (existing_children.Rows.Count > 0)
                    {
                        var children_wos = "";

                        foreach (DataRow dr in existing_children.Rows)
                        {

                            var wo_id = dr["woprog_id"];
                            children_wos += string.Format("<br/><a style='color:#ffc' href='/sections/workorder/index.aspx?woprog_id={0}&is_n1=true'> Child WO #: {0}</a>", wo_id);
                        }
                        cant_delete_reason.Add("This work order has associated work order(s) thus it cant be deleted: " + children_wos);
                    }
                    // If a WO is of type progress bill, user will be able to delete it if it satisfies the conditions below.
                    if (_wo.IsProgressBill && cant_delete_reason.Count < 0 && q["woprog_id"] != "0" &&
                                        (user.business_unit.is_backoffice
                                        || (_wo.business_unit_id == user.business_unit_id && CanApproveBM))
                                )
                    {
                        bt_delete_wo.CssClass = "";
                        bt_delete_wo.Style.Add("cursor", "pointer");
                        bt_delete_wo.ToolTip = "Click to delete work order";
                        bt_delete_wo.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this work order?')");
                        bt_delete_wo.ImageUrl = "/images/iconsbuttons/32px-Crystal_Clear_action_button_cancel.png";
                    }


                    if (cant_delete_reason.Count > 0 && q["woprog_id"] != "0")
                    {
                        var bt_delete_tooltip = "<ul>";
                        for (var i = 0; i < cant_delete_reason.Count; i++)
                        {
                            bt_delete_tooltip += "<li>" + cant_delete_reason[i] + "</li>";
                        }
                        bt_delete_tooltip += "</ul>";
                        bt_delete_wo.Attributes["data-tooltip"] = bt_delete_tooltip;
                        bt_delete_wo.Attributes["data-width"] = "400";
                        bt_delete_wo.Attributes["data-title"] = "Cannot Delete";
                        bt_delete_wo.Attributes.Add("onclick", "return false;");
                        bt_delete_wo.ImageUrl = "/images/iconsbuttons/32px-Crystal_Clear_action_button_cancel_grey.png";
                    }
                    else if (q["woprog_id"] == "0")
                    {
                        bt_delete_wo.Visible = false;
                    }
                    else
                    {
                        bt_delete_wo.CssClass = "";
                        bt_delete_wo.Style.Add("cursor", "pointer");
                        bt_delete_wo.ToolTip = "Click to delete work order";
                        bt_delete_wo.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this work order?')");
                        bt_delete_wo.ImageUrl = "/images/iconsbuttons/32px-Crystal_Clear_action_button_cancel.png";
                    }

                }
                pc_main.TabPages[WorkOrderTabPage.LinkedWorkOrders].Visible = CanViewCost;
                #endregion delete button logic
                check_open_pos();
                check_project_folder(_wo.business_unit_id);
                if (!IsPostBack)
                {
                    load_buttons(); // Has to be inside the first load of the page
                    Session["workorder_part_history"] = null;

                    pc_main.TabPages[WorkOrderTabPage.Customer].Visible = false; //hidden Customer tab

                    var error = q["error"];

                    if (!string.IsNullOrEmpty(error))
                    {
                        lblError.Text = error;
                        lblError.ClientVisible = true;

                    }

                    if (user.isContact || lblError.ClientVisible)
                    {
                        pc_main.ActiveTabIndex = 0;
                        fill_page_info("General");
                    }
                    else if (_wo.Status == OpsWOStatus.Invoiced && CanViewCost)
                    {
                        pc_main.ActiveTabIndex = 7;

                        fill_page_info("Analysis");
                    }
                    else if (new List<string>(new string[]{ OpsWOStatus.Rework,
                                                    OpsWOStatus.InitialPrep,
                                                    OpsWOStatus.QuestionsForPM,
                                                    OpsWOStatus.WaitingPMApproval,
                                                    OpsWOStatus.WaitingBMApproval,
                                                    OpsWOStatus.WaitingToBeInvoiced
                                                    }).Contains(_wo.Status))
                    {
                        pc_main.ActiveTabIndex = WorkOrderTabPage.LineItems;
                        fill_page_info("Line Items");
                    }
                    else if (!string.IsNullOrEmpty(q["lineitems"]))
                    {
                        pc_main.ActiveTabIndex = 2;
                        fill_page_info("Line Items");
                    }
                    else
                    {
                        pc_main.ActiveTabIndex = 0;
                        fill_page_info("General");

                    }

                    //   update_description_text();  removed by andy because it's causing some problems..
                    //populate_locations(true);
                }
                else  //'if postback
                {
                    if (pc_main.ActiveTabIndex == 4)
                    {
                        fill_page_info("PO");
                    }
                    else if (pc_main.ActiveTabIndex == 16)
                    {
                        fill_page_info("Checklist");
                    }
                    //populate_locations(false);
                    chk_interco();
                }
                load_dollars_header();

                if (ddlCustomer.Value == null)
                {
                    // For first loading, don't touch when customerId=0
                    if (customerId > 0)
                    {
                        ddlCustomer.Value = customerId;
                    }
                }
                else
                {
                    // not first time loading.
                    customerId = Convert.ToInt32(ddlCustomer.Value);
                }

                DataBindForCustomer(ddlCustomer, conn, hidWOProgID.Value, hidCompanyID.Value, customerId);

                if (ddlContact.Value == null)
                {
                    contactId = 0;
                }
                else
                {
                    contactId = Convert.ToInt32(ddlContact.Value);
                }

                populate_ddlContact(customerId, contactId);


                if (ddlRevenueLines.Value == null)
                {
                    // Create a new one or
                    // First time to load one
                }
                else
                {
                    // call back - always get it from control.
                    if ((!string.IsNullOrEmpty(hdnJobCostWO.Value)) && chkProgress.Checked)
                    {
                        NeWOProg JobCostWO = new NeWOProg(Convert.ToInt32(hdnJobCostWO.Value));

                        ddlRevenueLines.Value = JobCostWO.revenue_line_id;
                    }
                    revenueId = Convert.ToInt32(ddlRevenueLines.Value);
                }

                // Set the default value based on quoted value from the page
                revenueId = this.SetDefaultRevenueLine(ddlquote.Value, revenueId);
                DataBindForRevenueLine(hidCompanyID.Value, revenueId);
                // Save logic as customer.
                if (Toolbox.Contains(_wo.Status, new[] {   OpsWOStatus.Invoiced,
                                                            OpsWOStatus.WaitingToBeInvoiced,
                                                            OpsWOStatus.Deleted,
                                                            OpsWOStatus.ClosedReassigned
                                                            }))
                {
                    ddlRevenueLines.ClientEnabled = false;
                    btnCloseandReassign.ClientEnabled = false;
                }
                if (!this.CheckWOs(_wo.woprog_id))
                {
                    btnCloseandReassign.ClientEnabled = false;
                    txtwhyCloseReassign.ClientEnabled = false;
                }
                if (!IsPostBack)
                {
                    DataBindingForCreditWoLink(hidCompanyID.Value, 0);
                }
                else
                {
                    var v = radiotype.Value;
                    DataBindingForCreditWoLink(hidCompanyID.Value, Convert.ToInt32(v));
                }
                if (q["woprog_id"] == "0")
                {
                    bt_delete_wo.Visible = false;
                }
            }
            //      NECustomer nec = new NECustomer(_wo.WOProg_Customer_ID);
            //   maxCreditDays = nec.customer_creditdays > 45 ? nec.customer_creditdays : 45;

            var cust = new NECustomer(Convert.ToInt32(ddlCustomer.Value));
            if (cust.IsNEcompany)
            {
                txtpb_amt.MaskSettings.Mask = "<0..9999999999999>";
            }
            setInvoicePreviewJS();

            if (!IsPostBack)
            {
                this.defaultTM.Value = this.GetConfigSettingByQuery(NESI.BLL.Common.Shared.Configuration.GetRevenueLineTM);
                this.defaultQuoted.Value = this.GetConfigSettingByQuery(NESI.BLL.Common.Shared.Configuration.GetRevenueLineQuoted);
            }

            if (_wo.woprog_id != 0)
            {
                var prevQID = Toolbox.doSQL_int(@"Select ifnull(MAX(quote_id),0) from quote_master where wo = @v0 ", new object[] { _wo.woprog_id });

                if (prevQID != 0)
                {
                    var is_tm = Toolbox.doSQL_int(@"Select ifnull(is_tm,0) from quote_master where quote_id = @v0 AND active_revision = 1", new object[] { prevQID });
                    var version = Toolbox.doSQL_int(@"Select revision from quote_master where quote_id = @v0 AND active_revision = 1", new object[] { prevQID });
                    if (is_tm == 1)
                    {
                        ddlquote.Visible = false;
                        lblInstructons.Visible = false;
                        lblTMquote.Visible = true;
                        lblTMquote.Text = "#" + prevQID + "V" + version;
                    }
                }
            }

        }
        catch (Exception e)
        {
            Toolbox.do_errorLog_errorStack(e);
        }
    }


    private void check_project_folder(int _businessUnitId)
    {
        try
        {
            if (_wo.woprog_id > 0)
            {
                var d = Directory.EnumerateFiles(new NeFiles().GetProjectFolder(_wo.woprog_id, "workorder", "", _businessUnitId), "*.*", SearchOption.AllDirectories).Count();
                pc_main.TabPages[WorkOrderTabPage.ProjectFolder].Text = "Project Folder (" + d + ")";
            }
        }
        catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }

    }
    class row_notes
    {
        private int _id;
        public int id { get { return _id; } set { _id = Convert.ToInt32(value); } }
        private string _reason;
        public string reason { get { return _reason; } set { _reason = value; } }
    }
    private void check_for_blocks(int _this_woprog_id)
    {
        var n_blocks = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND blocks_schedule = 1", new object[] { _this_woprog_id });
        var current_status = Toolbox.doSQL_string(@"SELECT woprog_status FROM woprog WHERE woprog_id = @v0 ", new object[] { _this_woprog_id });

        if (n_blocks > 0 && current_status != "Waiting for Parts") // Add Lock WO Status
        {
            // SET WO status
            Toolbox.doSQL_void(@"UPDATE woprog SET woprog_status = 'Waiting for Parts' WHERE woprog_id = @v0  LIMIT 1", new object[] { _this_woprog_id });
            // ADD history line
            NeWOProg.add_history(_this_woprog_id, user.id, "Waiting for Parts", Toolbox.MySQLNow_long());
        }
        else if (n_blocks == 0 && current_status == "Waiting for Parts") // Release  Lock status
        {
            // Get Last status before most recent lock.
            var previous_status = Toolbox.doSQL_string(@"SELECT woprogstatus_status FROM woprogstatus where woprogstatus_woprog_id = @v0  AND woprogstatus_status != 'Waiting for Parts' order by woprogstatus_datetime DESC LIMIT 1", new object[] { _this_woprog_id });
            // SET WO status
            Toolbox.doSQL_void(@"UPDATE woprog SET woprog_status = @v1  WHERE woprog_id = @v0  LIMIT 1", new object[] { _this_woprog_id, previous_status });
            // ADD history line
            NeWOProg.add_history(_this_woprog_id, user.id, previous_status, Toolbox.MySQLNow_long());
        }
    }
    protected void populate_ddlCompany(bool _is_contact)
    {
        var companies = Toolbox.doSQL_dt(string.Format(@"SELECT ID,ddl_name name FROM business_unit  where id in ({0}) ", new Current_User().visible_business_units), null);

        ddlCompany.DataSource = companies;
        ddlCompany.DataBind();
    }
    protected void populate_parent_workorder(int _child_id)
    {//this
        var imaparentalready = false;

        if ((Toolbox.doSQL_int("Select count(woprog.woprog_id) from woprog where woprog.parent_woprog_id = @v0 and woprog.parent_woprog_id!=0 ", new object[] { _wo.woprog_id }) != 0) || (_wo.parent_woprog_id != 0))
        {
            imaparentalready = true;
        }

        var workorders = Toolbox.doSQL_dt(@" SELECT 0 id, 'Please Select Parent WO' text, '0000' customer_name, 0 woprog_bvwo 
UNION 
SELECT a.woprog_id id, 
CONCAT('(', a.woprog_bvwo, ') [',a.woprog_customername,'] ', REPLACE(a.woprog_description, '\n', ' ')) text, 
a.woprog_customername, 
a.woprog_bvwo 
FROM woprog a 
WHERE (a.business_unit_id = @v0  AND a.woprog_status = 'Open'  and a.parent_woprog_id = 0 and @v2) OR a.woprog_id = @v1
ORDER BY customer_name ", new object[] { _child_id, _wo.parent_woprog_id, !imaparentalready });

        ddl_parent_workorder.DataSource = workorders;
        ddl_parent_workorder.DataBind();
    }
    protected void ddl_parent_workorder_callback(object _sender, CallbackEventArgsBase _e)
    {
        var child_id = 0;
        int.TryParse(_e.Parameter, out child_id);

        populate_parent_workorder(child_id);
        if (ddl_parent_workorder.SelectedIndex >= 0)
        {
            ddl_parent_workorder.Font.Underline = true;
            ddl_parent_workorder.ForeColor = System.Drawing.Color.Blue;
            ddl_parent_workorder.Cursor = "pointer";
        }
    }
    private void set_js_confirm(NeWOProg _wo)
    {
        var min_hr = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(wo_detail_current_qty_committed),0) FROM wo_detail_current  where wo_detail_current_woprog_id =@v0 and wo_detail_current_type = 'L'", new object[] { _wo.woprog_id });
        var zer_cp = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current  where wo_detail_current_woprog_id =@v0 and wo_detail_current_qty_committed = 0 and wo_detail_current_origin like 'PO %'", new object[] { _wo.woprog_id });
        var str_zer_cp = zer_cp > 0 ? Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(wo_detail_current_rec_no) FROM wo_detail_current  where wo_detail_current_woprog_id =@v0 and wo_detail_current_qty_committed = 0 and wo_detail_current_origin like 'PO %'", new object[] { _wo.woprog_id }) : "";
        #region foot / meter check
        var this_branch = new NeBusinessUnit(_wo.business_unit_id);
        var this_emt = "";
        var this_t90 = "";
        var ftmetr = false;
        if (this_branch.country == "CDN")
        {
            this_t90 = Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(a.master_id) FROM inventory_item_detail a LEFT JOIN inventory_item_master b ON a.master_id = b.master_id  WHERE a.attribute_value_id = 55 AND b.tag_id = 3"); // 55 equals the attribute_value_id for "t90/thhn" for the attribute "Wire", so I go directly to the source, inventory_item_detail and get all master_id's that have that attribute_value_id, 3 = the tag wire
            this_emt = Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(a.master_id) FROM inventory_item_detail a LEFT JOIN inventory_item_master b ON a.master_id = b.master_id  WHERE a.attribute_value_id = 54 AND b.tag_id = 5"); // 54 equals the attribute_value_id for "EMT" for the attribute "Conduit", so I go directly to the source, inventory_item_detail and get all master_id's that have that attribute_value_id, 5 = the tag conduit
            if (this_t90 != "" && this_emt != "")
            {
                ftmetr = Toolbox.doSQL_int(string.Format(@"
SELECT 
	COUNT(*) 
FROM 
	wo_detail_current
WHERE 
	wo_detail_current_woprog_id = @v0 AND 
	(
		(wo_detail_current_master_id IN ({0}) AND wo_detail_current_description NOT LIKE '%\%2Fft%') OR 
		(wo_detail_current_master_id IN ({1}) AND wo_detail_current_description NOT LIKE '%\%2Fm%')
	)", this_emt, this_t90), new object[] { _wo.woprog_id }) > 0;
            }
        }
        else
        {
            ftmetr = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_description LIKE '%%2Fm'", new object[] { _wo.woprog_id }) > 0;
        }
        #endregion foot / meter check
        var tot_in = _wo.woprog_StillToBeBilled;
        string quote_id = quote_id = string.IsNullOrEmpty(_wo.QuoteID.Trim()) || _wo.QuoteID == "0" || _wo.QuoteID.Length < 7 ? "" : _wo.QuoteID.Substring(0, 6);
        var was_qb = false;
        var tot_qb = 0.00;
        try
        {
            if (quote_id != "")
            {
                var q_info = Toolbox.doSQL_dt(@"SELECT IF(pricetype_id = 3, true, false) was_qb, quoted_price FROM quote_master  WHERE active_revision = true AND quote_id =@v0", new object[] { quote_id }).Rows[0];
                was_qb = Convert.ToBoolean(q_info["was_qb"]);
                tot_qb = was_qb ? Convert.ToDouble(q_info["quoted_price"]) : 0;
            }
        }
        catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
        #region confirm jscript
        var scr = Regex.Replace(string.Format(@"
			var confirm_v	=	{{
								min_hr: {0},	// Sum of committed hours (Double)
								zer_cp: {1},	// Zero committed parts from PO's (Int)
								was_qb:	{2},	// Was a quoted_budget (Boolean)
								tot_qb:	{3},	// Quoted budget total (Double)
								tot_in: {4},	// Total Invoiced (Double)
								ftmetr:	{5},	// Contains the wrong measurements (Boolean)
								recs:	'{6}',	//
								r:		'',		// table rows holder variable
								title:	'Please confirm the following:', 
								h:		450,	// Width of the popup
								c:		0		// Number of problems it comes across, used for defining the height of the popup.
								}};

            function update_confirm_variables(_callback)
	            {{
	            $.get('./index.aspx',

	                    {{
	                    a: 'json_update_confirm_variables',
	                    woprog_id: {7}
	                    }}, 
	                function(json_string)
	                    {{
                        
	                    var obj = $.parseJSON(json_string);
                        console.log(obj.min_hr);
                        confirm_v.min_hr = obj.min_hr;
                        confirm_v.zer_cp = obj.zer_cp;
                        confirm_v.was_qb = obj.was_qb;
                        confirm_v.tot_qb = obj.tot_qb;
                        confirm_v.tot_in = obj.tot_in;
                        confirm_v.ftmetr = obj.ftmetr;
                        confirm_v.recs = obj.recs;
                        confirm_v.r = '';
                        confirm_v.title = 'Please confirm the following:';
                        confirm_v.h = 450;
                        confirm_v.c = 0;
_callback();
	                    }});
                    
	                }}
            function confirm_check(obj)
				{{                
				$(obj).text('Confirmed').attr('disabled', true);
				var n_confirms		= $('.confirmation').size();
				var n_depressed		= $('.confirmation:disabled').size();
				if(n_depressed == n_confirms)
					{{
					$('#confirmation_yes').removeAttr('disabled');
					}}
				}}
			function filter_by(i)
				{{
				if(pc_main.activeTabIndex != 2)
					{{
					alert('You need to have the line items tab clicked in order to use this');
					}}
				else
					{{
					var iframe = document.getElementById('ctl00_cphMasterBody_pc_main_picklist');
					var innerDoc = iframe.contentWindow;
					innerDoc.setFilter(i);
					}}
				}}
			function confirm_approval(base_url)
				{{
                update_confirm_variables(function(){{
                                        
                    $(""#confirm_asdf12345"").remove();                    
				    if($(""#confirm_asdf12345"").size() == 0)
					    {{
					    if(confirm_v.min_hr < 2)
						    {{
						    confirm_v.r	+= ""\
						    <tr>\
							    <td><b style='color:red'>""+confirm_v.min_hr+""</b> hour(s) are billable on this work order.</td>\
							    <td><button type='button' class='confirmation' style='width:100px' onclick='confirm_check(this)'>Confirm</button></td>\
						    </tr>"";
						    confirm_v.c++;
						    }}
					    if(confirm_v.zer_cp > 0)
						    {{
						    confirm_v.r	+= ""\
						    <tr>\
							    <td>There are <b style='color:red'>""+confirm_v.zer_cp+""</b> part(s) that came from PO's with a qty of zero committed."";
					    var recs			= confirm_v.recs.length > 0 && confirm_v.recs.match(/\,/g) 
											    ? confirm_v.recs.split(',') 
											    : confirm_v.recs.length > 0 
												    ? [confirm_v.recs] 
												    : [];
					    if(recs.length > 0)
						    {{
						    confirm_v.r += ""\
							    <br/>"";
						    for(var i = 0; i < recs.length; i++)
							    {{
							    confirm_v.r	+= ""<button type='button' onclick='filter_by(""+recs[i]+"")' title='Click to filter the line items.'>Rec #""+recs[i]+""</button>"";
							    }}
						    }}
					    confirm_v.r += ""\</td>\
							    <td><button type='button' class='confirmation' style='width:100px' onclick='confirm_check(this)'>Confirm</button></td>\
						    </tr>"";
						    confirm_v.c++;
						    }}
					    if(confirm_v.was_qb)
						    {{
						    confirm_v.r	+= ""\
						    <tr>\
							    <td>The attached quote was a budget.<br/>Invoice Total: <b style='color:red'>$""+confirm_v.tot_in+""</b><br/>Budget Total: <b style='color:red'>$""+confirm_v.tot_qb+""</b></td>\
							    <td><button type='button' class='confirmation' style='width:100px' onclick='confirm_check(this)'>Confirm</button></td>\
						    </tr>"";
						    confirm_v.c++;
						    }}
					    if(confirm_v.ftmetr)
						    {{
						    confirm_v.r	+= ""\
						    <tr>\
							    <td>There are lines in the work sheet that possibly have wrong measurements (ft/m).<br/> Please confirm only after you have double checked that the worksheet is correct.</td>\
							    <td><button type='button' class='confirmation' style='width:100px' onclick='confirm_check(this)'>Confirm</button></td>\
						    </tr>"";
						    confirm_v.c++;
						    }}
					    if(base_url.match(/wait/g))
						    {{
						    please_wait('start');
                            sendto(base_url);
						    }}
					    else
						    {{
						    if(confirm_v.r == '')
							    {{
							    confirm_v.r	= ""\
							    <tr>\
								    <td>Are you sure you want to approve this work order?</td>\
								    <td><button type='button' style='width:48px' id='confirmation_yes' onclick=\""sendto('""+base_url+""');\"">Yes</button><button type='button' style='width:48px' onclick=\""$('#confirm_asdf12345').dialog('destroy')\"">No</button></td>\
							    </tr>"";
							    confirm_v.title	= 'Please confirm:';
							    confirm_v.c++;
							    }}
						    else
							    {{
							    confirm_v.r	+= ""\
							    <tr>\
								    <td>Are you sure you want to approve this work order?</td>\
								    <td><button type='button' style='width:48px' id='confirmation_yes' onclick=\""sendto('""+base_url+""');\"" disabled>Yes</button><button type='button' style='width:48px' onclick=\""$('#confirm_asdf12345').dialog('destroy')\"">No</button></td>\
							    </tr>"";
							    confirm_v.title	= 'Please confirm:';
							    confirm_v.c++;
							    }}
						    confirm_v.h		= 65*confirm_v.c;
						    $('body').append(""\
						    <div id='confirm_asdf12345'>\
						    <table width='100%'>\
							    ""+confirm_v.r+""\
						    </table>\
						    <div>"");
						    }}
					    }}
				    $('#confirm_asdf12345').dialog({{title:confirm_v.title,modal:true, width:700, height:confirm_v.h, resizable:false, draggable:false, close:function(){{$('#confirm_asdf12345').dialog('destroy');  }}}});
                }});

				}}
",
                            min_hr,                         // {0}
                            zer_cp,                         // {1}
                            was_qb.ToString().ToLower(),    // {2}
                            tot_qb,                         // {3}
                            tot_in,                         // {4}
                            ftmetr.ToString().ToLower(),    // {5}
                            str_zer_cp, //{6}
                            _wo.woprog_id // {7}
                            ), "// (.+)", "");
        #endregion confirm jscript
        var inc = new HtmlGenericControl("script");
        inc.Attributes.Add("type", "text/javascript");
        inc.InnerHtml = scr;
        Header.Controls.Add(inc);
    }
    protected void load_dollars_header()
    {
        if (CanViewGross) // can the user see gross margin?
        {
            lbl_topmargin.ClientVisible = true;
            lbl_ToBeInvoiced.ClientVisible = true;
            lbl_top_whole_job.ClientVisible = _wo.QuoteID != "0";

        }
        else
        {
            lbl_topmargin.ClientVisible = false;
            lbl_top_whole_job.ClientVisible = false;
        }
        if (CanViewDollarTotalsAllBranches) // can the user see dollars info?
        {
            lbl_ToBeInvoiced.ClientVisible = true;
        }
        else
        {
            lbl_ToBeInvoiced.ClientVisible = false;
        }
    }
    protected void populate_locations(bool _reset_index)
    {
        var dt = Toolbox.doSQL_dt(@"SELECT 0 id, 'If Applicable Select Location' name UNION SELECT id,name FROM (SELECT id,name FROM inventory_location_master WHERE business_unit_id = @v0  AND type_id = 2 ORDER BY id) locs", new object[] { ddlCompany.Value });
        ddl_default_location.DataSource = dt;
        ddl_default_location.DataBind();
        ddl_default_location.SelectedIndex = _reset_index ? 0 : ddl_default_location.SelectedIndex;
    }

    // Acting BDM renamed from Acting RAM
    protected void populate_ddl_acting_ram(int _acting_ram)
    {
        sds_acct_manager.SelectCommand = "SELECT a.member_id, a.member_fullname FROM member a INNER JOIN membertype b on a.member_membertype_id = b.membertype_id where b.considered_ram = 1 and a.member_status ='Active' union select 0 Member_ID, 'No One' member_fullname  order by member_fullname ";

        ddl_rams.DataBind();
        ddl_rams.SelectedIndex = _acting_ram != 0 && ddl_rams.Items.Count > 0 && ddl_rams.Items.FindByValue(_acting_ram) != null ? ddl_rams.Items.FindByValue(_acting_ram).Index : -1;

    }

    // BDM field: new field
    protected void populate_ddl_customer_ram(int _customer_ram, bool _existing)
    {
        ddl_custram.DataBind();
        if (_customer_ram != 0 && ddl_custram.Items.Count > 0 && ddl_custram.Items.FindByValue(_customer_ram) != null)
        {
            ddl_custram.SelectedIndex = ddl_custram.Items.FindByValue(_customer_ram).Index;
        }
        else
        {
            if (_existing)
            {
                ListEditItem itemHouse = ddl_custram.Items.FindByText("House Account");
                if (itemHouse != null)
                {
                    ddl_custram.SelectedIndex = itemHouse.Index;
                }
                populate_ddl_customer_ram(0, false);
            }
            else
            {
                ddl_custram.Items.Insert(0, new ListEditItem(" ", 0));
                ddl_custram.SelectedIndex = 0;
            }
        }
    }
    protected void populate_ddlCustomer(int _companyid, int _custid)
    {
        if (_custid == 0)
        {
            return;
        }

        var sp = "CALL ds_workorder_customers_withMapping(@v0, @v1, @v2)";
        ddlCustomer.DataSource = Toolbox.doSQL_dt(sp, new object[] { 0, _companyid, _custid });
        ddlCustomer.DataBind();
        ddlCustomer.Value = _custid;
        ddlCustomer.SelectedIndex = _custid != 0 && ddlCustomer.Items.Count > 0 && ddlCustomer.Items.FindByValue(_custid) != null ? ddlCustomer.Items.FindByValue(_custid).Index : -1;

    }

    protected void radiotype_SelectedIndexChanged(object sender, EventArgs e)
    {
        var wo_type = (int)radiotype.Value;
        var isClosed = _wo.Status.Contains(OpsWOStatus.Invoiced) || _wo.Status == OpsWOStatus.Deleted || _wo.Status == OpsWOStatus.ClosedReassigned;
        if (wo_type == 0 && !isClosed && _wo.woprog_id ==0)
        {
            ddl_custram.ClientEnabled = true;
            ddl_custram.Visible = true;
            ddl_custram.ClientVisible = true;
            ddl_rams.ClientEnabled = true;
            ddl_rams.Visible = true;
            ddl_rams.ClientVisible = true;

        }
        else
        {
            // Current User must have 157 privilege for BDM to be enabled.
            // OR 211 && be the PM of WO

            ddl_custram.ClientEnabled = CanEditBDM && !isClosed;
            ddl_custram.Visible = true;
            ddl_custram.ClientVisible = true;
            ddl_rams.ClientEnabled = CanEditActingBDM && !isClosed;
            ddl_rams.Visible = true;
            ddl_rams.ClientVisible = true;


        }

    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        var wo_type = (int)radiotype.Value;
        if (wo_type == 0)
        {
            int customerId = Convert.ToInt32(ddlCustomer.Value);

            int businessUnitId = Convert.ToInt32(ddlCompany.Value);

            ddl_custram.DataSourceID = null;

            var dt = Toolbox.doSQL_dt("CALL sp_get_bdm_for_customer(@v0, @v1, @v2)",
                new object[] { customerId, businessUnitId, "workorder" });

            ddl_custram.DataSource = dt;
            ddl_custram.DataBind();
            if (dt.Rows.Count > 0)
            {
                ddl_custram.Value = dt.Rows[0]["member_id"];
                ddl_custram.SelectedIndex = 0;
            }
        }
    }
    protected void populate_ddlJobCostWO(int _companyid, int _cust_id, int _jobcostwoid)
    {
        //        if (CustID != 0)
        //        {
        var prog = new NeWOProg();
        ddlJobCostWO.DataSource = prog.GetOpenWorkOrders(_companyid, _cust_id, _jobcostwoid != 0);
        ddlJobCostWO.DataBind();
        if (ddlJobCostWO.Items.Count == 0)
        {
            ddlJobCostWO.Text = "No Open WOs";
            ddlJobCostWO.ClientEnabled = false;
        }
        else
        {
            ddlJobCostWO.ClientEnabled = true;
        }
        //        }
        if (_jobcostwoid != 0 && _jobcostwoid != 1)
        {
            ddlJobCostWO.Value = Convert.ToInt32(_jobcostwoid);
        }
    }
    protected void populate_ddlquote(object _quoteid)
    {
        if (_quoteid.ToString() == "0")
        {
            pc_main.TabPages[WorkOrderTabPage.Quote].ClientVisible = false;
        }

        if (_quoteid.ToString() == "0" && (ddlCustomer.Value == null || ddlCustomer.Value.ToString() == "0"))
        {
            sds_quote.SelectCommand = "Select quote_n,urldecode(`quote_stuff`)as Quote_Stuff from vw_openquotes where business_unit_id = " + ddlCompany.Value;
            ddlquote.DataBind();
        }
        else if (_quoteid.ToString() == "0" && ddlCustomer.Value != null && ddlCustomer.Value.ToString() != "0")
        {
            sds_quote.SelectCommand =
                "Select quote_n,urldecode(`quote_stuff`)as Quote_Stuff from vw_openquotes where customer_id = " +
                ddlCustomer.Value + " and business_unit_id = " + ddlCompany.Value;
            ddlquote.DataBind();
        }
        else if (ddlCustomer.Value != null && ddlCustomer.Value.ToString() != "0")
        {
            sds_quote.SelectCommand =
                "Select quote_n,urldecode(`quote_stuff`)as Quote_Stuff from vw_openquotes where customer_id = " +
                ddlCustomer.Value + " and business_unit_id = " + ddlCompany.Value;
            ddlquote.DataBind();

        }



        ddlquote.Value = _quoteid;
        ddlquote.Enabled = !_wo.HasProgressBills;
    }

    protected void ddlquoteSelectIndexChange(object _sender, EventArgs _e)
    {
        if (ddlquote.Value.ToString() == "0")
        {
            pc_main.TabPages[WorkOrderTabPage.Quote].ClientVisible = false;
        }

    }

    protected void populate_ddl_Contact(int _custid, int _contactid)
    {
        ddl_Contact.DataBind();
        if (ddl_Contact.Items.FindByValue(_contactid) != null)
        {
            ddl_Contact.Value = _contactid;
        }
        else
        {
            ddl_Contact.Value = 0;
        }
    }

    protected void populate_ddl_Address(int _custid, int _addressid)
    {
        ddl_Address.DataBind();
        if (ddl_Address.Items.FindByValue(_addressid) != null)
        {
            ddl_Address.Value = _addressid;
        }
        else
        {
            ddl_Address.Value = 0;
        }
    }

    protected void populate_ddlAddress(int _custid, int _addressid)
    {
        ddlAddress.DataBind();
        if (ddlAddress.Items.FindByValue(_addressid) != null)
        {
            ddlAddress.Value = _addressid;
        }
        else
        {
            ddlAddress.Value = 0;
        }
    }
    protected void populate_ddlPM(int _companyid, int _pmid)
    {
        ddlPM.DataBind();
        if (ddlPM.Items.FindByValue(_pmid) == null && _pmid > 0 && hidWOProgID.Value != "0")
        {
            var name = Toolbox.doSQL_string(@"SELECT IFNULL(MIN(member_fullname), 'Does not Exist') FROM member WHERE member_id = @v0  LIMIT 1", new object[] { _pmid });
            ddlPM.Visible = false;
            pm_inactive_name.InnerText = name;
        }
        else if (ddlPM.Items.FindByValue(_pmid) != null && _pmid > 0 && hidWOProgID.Value != "0")
        {
            ddlPM.Value = _pmid;
        }
        else if (_pmid == 0 && ddlPM.Items.FindByValue(user.id) != null)
        {
            ddlPM.Value = user.id;
        }
        else
        {
            ddlPM.SelectedIndex = -1;
        }
    }

    protected void populate_ddlContact(int _cust_id, int _contactid)
    {
        var query = @"SELECT 0 contact_id, 'Please select contact' contact_name UNION ALL (SELECT contact_id, contact_name FROM contact 
WHERE contact_type = 'Customer' AND contact_cust_id= @v0 AND contact_status = 'Active' ORDER BY contact_name)";
        ddlContact.DataSource = Toolbox.doSQL_dt(query, new object[] { _cust_id });

        ddlContact.DataBind();
        //ddlContact.SelectedIndex = Contactid != 0 && ddlContact.Items.Count > 0 ? ddlContact.Items.FindByValue(Contactid).Index : 0;
        if (hid_newcontact_id.Value != "" && hid_newcontact_id.Value != "0")
        {
            ddlContact.Value = Convert.ToInt32(hid_newcontact_id.Value);
        }
        else
        {
            ddlContact.Value = _contactid;
        }
    }
    private bool print_timesheet_in_comments(int _woprog_id)
    {
        return Toolbox.doSQL_int(@"SELECT IFNULL(MAX(printcomments), 0) = 1 FROM wocomment  where woprog_Id =@v0", new object[] { _woprog_id }) == 1;
    }
    protected void btncomments_Click(object _sender, EventArgs _e)
    {
        _strprint = chkPrint.Checked ? "1" : "0";
        var tools = new Toolbox();
        var thiscommentid = "0"; ;
        var timecomment = tools.getSQL_datatable(@"SELECT WOComment_ID, WorkOrder_ID, Comments, CAST(PrintComments AS CHAR) AS PrintComments, Member_ID_Audit, Created_Date, WOComment_Company_ID FROM vwwocomments  where woprog_id =@v0 and member_id = 0", new object[] { _wo.woprog_id });
        if (timecomment.Rows.Count == 1)
        {
            foreach (DataRow dr_time in timecomment.Rows)
            {
                //chkPrint.Enabled = true;
                txtTSComments.Disabled = false;
                thiscommentid = dr_time["WOComment_ID"].ToString();
            }
        }
        if (thiscommentid != "0")
        {
            try
            {
                tools.getSQL_void(@"Update WOCOMMENT  Set Comments =@v0,PrintComments =@v1 ,Modified_Date = NOW()   Where WOCOMMENT_ID =@v2", new object[] { txtTSComments.Value, Convert.ToInt16(_strprint), thiscommentid });
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                lblError.ClientVisible = true;
            }
        }
        else
        {
            try
            {
                tools.getSQL_void(@"INSERT INTO WOCOMMENT (WorkOrder_ID, 
Comments, Personal, PrintComments, Member_ID_Audit,Created_Date, Modified_Date,
wocomment_member_id, WOComment_Company_ID,woprog_id)  VALUES(@v0,@v1,0,@v2,@v3, NOW(), NOW(), 0,@v4,@v5)",
new object[] { _wo.OrderNumber,
    txtTSComments.Value,
    Convert.ToInt16(_strprint),
    user.id,
    int.Parse(hidCompanyID.Value),
    _wo.woprog_id });
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                lblError.ClientVisible = true;
            }
        }

        fill_page_info("Time Entries");
    }
    protected void lbStock_Click(object _sender, EventArgs _e)
    {
        //Set All To Do Not Include For Stock
        var tools = new Toolbox();
        var str_sel_for_change = "UPDATE wo_detail_current SET wo_detail_current_billtypeid = 5 WHERE wo_detail_current_woprog_id=" + hidWOProgID.Value;
        tools.getSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_billtypeid = 5  WHERE wo_detail_current_woprog_id=@v0", new object[] { hidWOProgID.Value });
        var sql = "INSERT INTO event_table (dateoccured,event_text,member_id) VALUES(now(),'Billing Type Set to Do Not Include for " + hidWOProgID.Value + "'," + user.id + ")";
        try
        {
            tools.getSQL_void(@"INSERT INTO event_table (dateoccured,event_text,member_id)  VALUES(now(),@v0,@v1)", new object[] { "Billing Type Set to Do Not Include for " + hidWOProgID.Value, user.id });
        }
        catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
    }
    protected void pc_main_ActiveTabChanged(object _source, TabControlEventArgs _e)
    {
        fill_page_info(_e.Tab.Text);
        load_dollars_header();
    }
    protected void load_buttons()
    {
        var canShowInvoicePreview = Toolbox.Contains(_wo.Status, new[] {   OpsWOStatus.WaitingToBeInvoiced,
                                                                            OpsWOStatus.Invoiced,
                                                                            OpsWOStatus.WaitingPMApproval,
                                                                            OpsWOStatus.WaitingBMApproval
                                                                            });
        var can_see_invoice_preview = CanSeeInvoicePreviewButton && canShowInvoicePreview;
        btnSendToPMApproval.Attributes.Add("Onclick", "sendto('../../wo_prog_edit.aspx?woprog_id=" + hidWOProgID.Value + "&action=waitapproval&business_unit_id=" + hidCompanyID.Value + "&bvwo=" + _wo.OrderNumber + "');");
        btnSendToPMApproval.Style.Add("cursor", "pointer");
        btnSendToRework.Attributes.Add("Onclick", "sendto('../../wo_prog_edit.aspx?woprog_id=" + hidWOProgID.Value + "&action=rework&business_unit_id=" + hidCompanyID.Value + "&bvwo=" + _wo.OrderNumber + "')");
        btnSendToRework.Style.Add("cursor", "pointer");
        //   btnInvoicePreview.OnClientClick = string.Format("boing('/sections/reports/invoice_preview/index.aspx?id={0}', 'invoice_preview', 850, 850);return false;", _wo.woprog_id);
        btnSendPMforQuestions.OnClientClick = "sendto('../../wo_prog_edit.aspx?woprog_id=" + hidWOProgID.Value + "&action=questions&business_unit_id=" + hidCompanyID.Value + "&bvwo=" + _wo.OrderNumber + "')";

        #region UNCUT WORK ORDER
        if (hidWOProgID.Value == "0") // if its an uncut work order
        {
            btnApprovedByPM.Visible = false;
            btnInvoicePreview.Visible = false;
            btnWorkOrderPreview.Visible = false;
            btnSendPMforQuestions.Visible = false;
            btnSendToPMApproval.Visible = false;
            btnSendToRework.Visible = false;
            btnSendtoWaitCustPO.Visible = false;
            btnSendToWaitingToBeInvoiced.Visible = false;
        }
        #endregion
        else
        {
            switch (_wo.Status)
            {
                #region Invoiced
                case OpsWOStatus.Invoiced:
                    btnSaveGeneral.Visible = false;
                    btnApprovedByPM.Visible = false;
                    //  btnInvoicePreview.Visible = false;
                    btnInvoicePreview.Visible = can_see_invoice_preview;
                    btnWorkOrderPreview.Visible = CanSeeInvoicePreviewButton;
                    btnSendPMforQuestions.Visible = false;
                    btnSendToPMApproval.Visible = false;
                    btnSendToRework.Visible = false;
                    btnSendtoWaitCustPO.Visible = false;
                    btnSendToWaitingToBeInvoiced.Visible = false;
                    button_logic(bt_signoff);
                    button_logic(bt_delete_wo);
                    break;
                #endregion Invoiced
                #region Waiting to be Invoiced
                case OpsWOStatus.WaitingToBeInvoiced:
                    btnSaveGeneral.Visible = false;
                    btnApprovedByPM.Visible = false;
                    //    btnInvoicePreview.Visible = false;
                    btnInvoicePreview.Visible = can_see_invoice_preview;
                    btnWorkOrderPreview.Visible = CanSeeInvoicePreviewButton;
                    btnSendPMforQuestions.Visible = false;
                    btnSendToPMApproval.Visible = false;
                    btnSendToRework.Visible = false;
                    button_logic(bt_signoff);
                    button_logic(btnSendtoWaitCustPO);
                    btnSendToWaitingToBeInvoiced.Visible = false;
                    break;
                #endregion Waiting to be Invoiced
                #region Open
                case OpsWOStatus.Open:
                    btnSaveGeneral.Visible = true;
                    btnApprovedByPM.Visible = false;
                    btnInvoicePreview.Visible = false;
                    btnWorkOrderPreview.Visible = CanSeeInvoicePreviewButton;
                    btnSendPMforQuestions.Visible = false;
                    btnSendToPMApproval.Visible = false;
                    btnSendToRework.Visible = false;
                    btnSendtoWaitCustPO.Visible = false;
                    btnSendToWaitingToBeInvoiced.Visible = false;
                    /*
					if (Toolbox.doSQL_double(@"Select woprog_totaltandm from woprog  where woprog_id =@v0", new object[] { wo.woprog_id }) == 0)
						{
						ImgBtnCancelWO.Visible = true;
						}
					*/
                    break;
                #endregion Open
                #region Waiting PM Approval
                case OpsWOStatus.WaitingPMApproval:
                    btnSaveGeneral.Visible = true;
                    // btnInvoicePreview.Visible = false;
                    btnInvoicePreview.Visible = can_see_invoice_preview;
                    btnWorkOrderPreview.Visible = CanSeeInvoicePreviewButton;
                    btnSendPMforQuestions.Visible = false;
                    btnSendToPMApproval.Visible = false;
                    btnSendToRework.Visible = true;
                    button_logic(bt_signoff);
                    btnSendtoWaitCustPO.Visible = false;
                    if (pc_main.TabPages[WorkOrderTabPage.POs].TabStyle.ForeColor.Name != System.Drawing.Color.Red.Name)
                    {
                        button_logic(btnSendToWaitingToBeInvoiced);
                        button_logic(btnSendtoWaitCustPO);
                        button_logic(btnApprovedByPM);
                    }
                    break;
                #endregion Waiting PM Approval
                #region Waiting BM Approval
                case OpsWOStatus.WaitingBMApproval:
                    btnSaveGeneral.Visible = true;
                    btnApprovedByPM.Visible = false;
                    button_logic(bt_signoff);
                    // btnInvoicePreview.Visible = false;
                    btnInvoicePreview.Visible = can_see_invoice_preview;
                    btnWorkOrderPreview.Visible = CanSeeInvoicePreviewButton;
                    btnSendtoWaitCustPO.Visible = false;
                    if (pc_main.TabPages[4].TabStyle.ForeColor.Name != System.Drawing.Color.Red.Name)
                    {
                        btnSendPMforQuestions.Visible = true;
                        button_logic(btnApprovedByPM);
                        button_logic(btnSendtoWaitCustPO);
                        button_logic(btnSendToWaitingToBeInvoiced);
                    }
                    else
                    {
                        btnSendPMforQuestions.Visible = true;
                    }
                    btnSendToRework.Visible = true;
                    break;
                #endregion Waiting BM Approval
                #region Initial Prep
                case OpsWOStatus.InitialPrep:
                    btnSaveGeneral.Visible = true;
                    //  btnInvoicePreview.Visible = false;
                    btnInvoicePreview.Visible = can_see_invoice_preview;
                    btnWorkOrderPreview.Visible = CanSeeInvoicePreviewButton;
                    btnSendtoWaitCustPO.Visible = false;
                    if (pc_main.TabPages[WorkOrderTabPage.POs].TabStyle.ForeColor.Name != System.Drawing.Color.Red.Name)
                    {
                        //		btnSendPMforQuestions.Visible = true;
                        button_logic(btnSendToPMApproval);
                        button_logic(btnSendtoWaitCustPO);
                        button_logic(bt_signoff);
                        button_logic(btnSendToWaitingToBeInvoiced);
                        button_logic(btnApprovedByPM);
                    }
                    btnSendPMforQuestions.Visible = true;
                    btnSendToRework.Visible = true;
                    if (_wo.IsProgressBill && _wo.Status == OpsWOStatus.InitialPrep)
                    {
                        btnApprovedByPM.Visible = user.business_unit.is_backoffice;
                        btnInvoicePreview.Visible = user.business_unit.is_backoffice;
                        btnWorkOrderPreview.Visible = user.business_unit.is_backoffice;
                        btnSendPMforQuestions.Visible = user.business_unit.is_backoffice;
                        btnSendToPMApproval.Visible = user.business_unit.is_backoffice;
                        btnSendToRework.Visible = user.business_unit.is_backoffice;
                        btnSendtoWaitCustPO.Visible = user.business_unit.is_backoffice;
                        btnSendToWaitingToBeInvoiced.Visible = user.business_unit.is_backoffice;
                        bt_signoff.Visible = user.business_unit.is_backoffice;
                    }
                    break;
                #endregion In Progress / Initial Prep
                #region Questions For PM
                case OpsWOStatus.QuestionsForPM:
                    btnSaveGeneral.Visible = true;
                    //  btnInvoicePreview.Visible = false;
                    btnInvoicePreview.Visible = can_see_invoice_preview;
                    btnWorkOrderPreview.Visible = CanSeeInvoicePreviewButton;
                    btnSendPMforQuestions.Visible = false;
                    btnSendToRework.Visible = true;
                    button_logic(bt_signoff);
                    btnSendtoWaitCustPO.Visible = false;
                    if (pc_main.TabPages[WorkOrderTabPage.POs].TabStyle.ForeColor.Name != System.Drawing.Color.Red.Name)
                    {
                        button_logic(btnApprovedByPM);
                        button_logic(btnSendToPMApproval);
                        button_logic(btnSendtoWaitCustPO);
                        button_logic(btnSendToWaitingToBeInvoiced);
                    }
                    break;
                #endregion Questions For PM
                #region Waiting For PO
                case OpsWOStatus.WaitingForPO:
                    btnSaveGeneral.Visible = true;
                    btnApprovedByPM.Visible = false;
                    //   btnInvoicePreview.Visible = false;
                    btnInvoicePreview.Visible = can_see_invoice_preview;
                    btnWorkOrderPreview.Visible = CanSeeInvoicePreviewButton;
                    btnSendPMforQuestions.Visible = false;
                    btnSendToPMApproval.Visible = false;
                    button_logic(bt_signoff);
                    btnSendToRework.Visible = user.is_backoffice;
                    btnSendtoWaitCustPO.Visible = false;
                    if (pc_main.TabPages[WorkOrderTabPage.POs].TabStyle.ForeColor.Name != System.Drawing.Color.Red.Name)
                    {
                        button_logic(btnSendToWaitingToBeInvoiced);
                    }
                    else
                    {
                        btnSendPMforQuestions.Visible = true;
                    }

                    break;
                case OpsWOStatus.WaitingParentBMApproval:
                    btnSaveGeneral.Visible = true;
                    btnApprovedByPM.Visible = false;
                    //   btnInvoicePreview.Visible = false;
                    btnInvoicePreview.Visible = can_see_invoice_preview;
                    btnWorkOrderPreview.Visible = CanSeeInvoicePreviewButton;
                    btnSendPMforQuestions.Visible = false;
                    btnSendToPMApproval.Visible = false;
                    button_logic(bt_signoff);
                    btnSendToRework.Visible = _wo.business_unit_id == Convert.ToInt32(Request.QueryString["business_unit_id"]);
                    btnSendtoWaitCustPO.Visible = false;

                    break;
                #endregion OpsWOStatus.WaitingForPO
                #region Rework
                case OpsWOStatus.Rework:
                    btnSaveGeneral.Visible = true;
                    //  btnInvoicePreview.Visible = false;
                    btnInvoicePreview.Visible = can_see_invoice_preview;
                    btnWorkOrderPreview.Visible = CanSeeInvoicePreviewButton;
                    btnSendToRework.Visible = false;
                    button_logic(bt_signoff);
                    btnSendtoWaitCustPO.Visible = false;
                    if (pc_main.TabPages[WorkOrderTabPage.POs].TabStyle.ForeColor.Name != System.Drawing.Color.Red.Name)
                    {
                        button_logic(btnApprovedByPM);

                        btnSendPMforQuestions.Visible = true;
                        btnSendToPMApproval.Visible = true;
                        button_logic(btnSendToWaitingToBeInvoiced);
                    }
                    else
                    {
                        btnSendPMforQuestions.Visible = true;
                    }
                    break;
                    #endregion Rework
            }
            if (!CanApprovePM && !CanApproveBM && !(CanApproveDM && _same_dept))
            {
                btnApprovedByPM.Visible = false;
                btnInvoicePreview.Visible = !user.isContact && can_see_invoice_preview && _wo.Status != OpsWOStatus.Open;
                btnInvoicePreview.Visible = false;
                btnWorkOrderPreview.Visible = false;
                btnSendPMforQuestions.Visible = false;
                btnSendToPMApproval.Visible = false;
                btnSendToRework.Visible = false;
                btnSendtoWaitCustPO.Visible = false;
                btnSendToWaitingToBeInvoiced.Visible = false;

            }
        }
        if (user.isContact)
        {
            tblbuttons.Visible = false;
        }
    }
    private void setInvoicePreviewJS()
    {
        if (_wo.woprog_id == 0) return;
        var ns_internal_id = Toolbox.doSQL_int(@"SELECT IFNULL(netsuite_invoice_internal_id,0) from woprog where woprog_id=@v0", new object[] { _wo.woprog_id });
        var ns_creditmemo_internal_id = Toolbox.doSQL_int(@"SELECT IFNULL(netsuite_creditmemo_internal_id,0) from woprog where woprog_id=@v0", new object[] { _wo.woprog_id });
        var invoice_url = ns_internal_id > 0 || ns_creditmemo_internal_id > 0 ? "invoice_ns.aspx" : "index.aspx";
        btnInvoicePreview.OnClientClick = "boing('/sections/reports/invoice_preview/" + invoice_url + "?id=" + _wo.woprog_id + "&is_credit=" + _wo.woprog_iscredit + "',700,650)";

        var workorder_url = "index.aspx";
        btnWorkOrderPreview.OnClientClick = "boing('/sections/reports/workorder_preview/" + workorder_url + "?id=" + _wo.woprog_id + "&is_credit=" + _wo.woprog_iscredit + "', 'work order preview',700,650)";
    }

    private void button_logic(Control _b)
    {
        var base_script = string.Format(@"to_approval({0},{1},'{2}'", hidWOProgID.Value, hidCompanyID.Value, _wo.OrderNumber);
        var error_base = "";
        switch (_b.ID)
        {
            case "btnSendToWaitingToBeInvoiced":
                error_base = All_Systems_Go("Invoice");
                if ((CanApproveBM || _wo.Status == OpsWOStatus.WaitingForPO && CanApprovePM || CanApproveDM && _same_dept))
                {
                    btnSendToWaitingToBeInvoiced.Visible = true;
                    btnSendToWaitingToBeInvoiced.Style.Add("cursor", "pointer");
                    btnSendToWaitingToBeInvoiced.Enabled = error_base == "";
                    if (_wo.woprog_creditcard_payment == 0)
                    {
                        btnSendToWaitingToBeInvoiced.ToolTip = !btnSendToWaitingToBeInvoiced.Enabled ? error_base.Replace("\\n", " -- ") : "Final approval, go ahead and invoice";
                        btnSendToWaitingToBeInvoiced.Attributes["onclick"] = btnSendToWaitingToBeInvoiced.Enabled ? base_script + ",'action=approveBranch','','toinvoice');" : "alert('" + error_base + "');";
                    }
                    else
                    {
                        btnSendToWaitingToBeInvoiced.ToolTip = !btnSendToWaitingToBeInvoiced.Enabled ? error_base.Replace("\\n", " -- ") : "Final approval, this will go to cust po to process the credit card";
                        btnSendToWaitingToBeInvoiced.Attributes["onclick"] = btnSendToWaitingToBeInvoiced.Enabled ? base_script + ",'action=waitpo','','waitpo');" : "alert('" + error_base + "');";
                        if (user.is_backoffice)  // if it's a nesi staffer
                        {
                            btnSendToWaitingToBeInvoiced.ToolTip = !btnSendToWaitingToBeInvoiced.Enabled ? error_base.Replace("\\n", " -- ") : "Final Approval, credit card has been processed, send it to waiting to be invoiced..";
                            btnSendToWaitingToBeInvoiced.Attributes["onclick"] = btnSendToWaitingToBeInvoiced.Enabled ? base_script + ",'action=approveBranch','','toinvoice');" : "alert('" + error_base + "');";
                        }
                    }
                    if (error_base != "")
                    {
                        var error_link = error_base.Contains("less than cost") ? "Click <a href=\"javascript:boing('./index.aspx?woprog_id=" + hidWOProgID.Value + "&a=less_than_cost', 'quote', 750,400);\">here</a> to fill out why parts are being sold at less than cost <br/>" : "";
                        lblError.Text = error_link + error_base.Replace("\\n", "<br/>");
                    }
                }
                break;

            case "btnApprovedByPM":
                error_base = All_Systems_Go("");
                if (CanApprovePM)
                {
                    btnApprovedByPM.Visible = true;
                    btnApprovedByPM.Enabled = error_base == "";
                    btnApprovedByPM.Style.Add("cursor", "pointer");
                    btnApprovedByPM.ToolTip = !btnApprovedByPM.Enabled ? error_base.Replace("\\n", " -- ") : "Approve (by PM) to next step!";
                    btnApprovedByPM.Attributes["onclick"] = btnApprovedByPM.Enabled ? base_script + ",'action=approve');" : "alert('There are errors with this work order that need to be addressed.\\n Please go over the issues column in the line items tab.');";
                    if (error_base != "")
                    {
                        var error_link = error_base.Contains("less than cost") ? "Click <a href=\"javascript:boing('./index.aspx?woprog_id=" + hidWOProgID.Value + "&a=less_than_cost', 'quote', 750,400);\">here</a> to fill out why parts are being sold at less than cost <br/>" : "";
                        lblError.Text = error_link + error_base.Replace("\\n", "<br/>");
                    }
                }

                break;
            case "btnSendtoWaitCustPO":
                error_base = All_Systems_Go("");
                btnSendtoWaitCustPO.Visible = user.is_backoffice || user.MemberTypeID == OpsMemberTypes.BranchManager;
                btnSendtoWaitCustPO.Enabled = error_base == "" || error_base.Contains("incomplete PO lines");
                btnSendtoWaitCustPO.Style.Add("cursor", "pointer");
                if (_wo.Status == OpsWOStatus.WaitingToBeInvoiced)
                {
                    btnSendtoWaitCustPO.Attributes["onclick"] = btnSendtoWaitCustPO.Enabled && user.business_unit.is_backoffice ? base_script + ",'action=waitpo','movetocurrent=yes');" : "alert('You cannot Send to Waiting Customer PO Column since the WO is in a Waiting to be Invoiced status.');";
                }
                else
                {
                    btnSendtoWaitCustPO.Attributes["onclick"] = btnSendtoWaitCustPO.Enabled && (user.business_unit.is_backoffice || user.business_unit.branch_manager.id == user.id) ? base_script + ",'action=waitpo','','waitpo');" : "alert('You do not have the required permissions to Send to Waiting Customer PO Column.');";
                }
                if (!btnSendtoWaitCustPO.Enabled && (user.is_backoffice || user.MemberTypeID == OpsMemberTypes.BranchManager))
                {
                    btnSendtoWaitCustPO.ToolTip = error_base.Replace("\\n", " -- ");
                }
                if (error_base != "")
                {
                    lblError.Text = error_base.Replace("\\n", "<br/>");
                }
                break;
            case "btnSendToPMApproval":
                error_base = All_Systems_Go("");
                btnSendToPMApproval.Visible = true;
                btnSendToPMApproval.Enabled = error_base == "";
                btnSendToPMApproval.Style.Add("cursor", "pointer");
                btnSendToPMApproval.ToolTip = !btnSendToPMApproval.Enabled ? error_base.Replace("\\n", " -- ") : "Send to PM for their approval";
                btnSendToPMApproval.Attributes["onclick"] = btnSendToPMApproval.Enabled ? base_script + ",'action=waitapproval');" : "alert('There are errors with this work order that need to be addressed.\\n Please go over the issues column in the line items tab.');";
                if (error_base != "")
                {
                    lblError.Text = error_base.Replace("\\n", "<br/>");
                }
                break;
            case "bt_signoff":
                bt_signoff.Visible = true;
                //bt_signoff.Attributes["onclick"] = "boing('/sections/workorder/wosignoff.aspx?woid=" + _wo.woprog_id + "', 'signoff', 670,960)";
                bt_signoff.Attributes["onclick"] = "boing('../../sections/reports/invoice_preview/index.aspx?id=" + _wo.woprog_id + "&is_signoff=true','sign_off',800,800)";
                bt_signoff.Style.Add("cursor", "pointer");
                bt_signoff.ToolTip = "Print Sign Off Sheet";
                break;
            case "bt_delete_wo":
                bt_delete_wo.Visible = false;
                break;
        }
        if (lblError.Text != "")
        {
            lblError.Visible = true;
            lblError.ClientVisible = true;

        }
    }
    protected void fill_page_info(string _tab_index)
    {
        if (_wo.woprog_iscredit == 1 || _wo.woprog_isrebill == 1)
        {
            pc_main.TabStyle.BackColor = Color.Red;
            pc_main.TabStyle.ForeColor = Color.White;

        }
        var businessUnit = new NeBusinessUnit(_wo.business_unit_id);

        ddlCompany.Value = _wo.business_unit_id;
        if (_tab_index == "General")
        {
            #region General
            var isClosed = _wo.Status.Contains(OpsWOStatus.Invoiced) || _wo.Status == OpsWOStatus.Deleted || _wo.Status == OpsWOStatus.ClosedReassigned;
            chkTax1.ClientVisible = false;
            chkTax2.ClientVisible = false;
            chkTax3.ClientVisible = false;
            chkTax4.ClientVisible = false;
            this.txtwhyCloseReassign.ReadOnly = true;

            if (ddl_parent_workorder.SelectedIndex >= 0)
            {
                ddl_parent_workorder.Font.Underline = true;
                ddl_parent_workorder.ForeColor = Color.Blue;
                ddl_parent_workorder.Cursor = "pointer";
            }
            else
            {
                tr_fixed_labour.Visible = businessUnit.allow_fixed_labour;
                tr_fixed_material.Visible = businessUnit.allow_fixed_markup;
            }
            if (!CanPutWoOnHold)
            {
                txtwhyhold.ClientEnabled = false;
                chkOnHold.ClientEnabled = false;
                if (user.isContact)
                {
                    txtwhyhold.ClientVisible = false;
                }
            }
            // Current User must have 157 privilege for BDM to be enabled.
            // OR 211 && be the PM of WO
            ddl_custram.ClientEnabled = CanEditBDM && !isClosed;
            ddl_custram.Visible = true;
            ddl_custram.ClientVisible = true;
            ddl_rams.ClientEnabled = CanEditActingBDM && !isClosed;
            ddl_rams.Visible = true;
            ddl_rams.ClientVisible = true;

            if (_wo.woprog_id == 0) // if it's a new work order
            {
                ddlCustomer.ClientEnabled = true;
                ddlCustomer.ReadOnly = false;
                #region New Work order
                ddlCompany.Value = _wo.business_unit_id;
                hidWOStatus.Value = "new";
                pc_main.Width = Unit.Pixel(900);
                lbl_tax.Visible = false;
                btnUpdateTaxes.Visible = false;
                chk_warranty.Checked = false;
                chkMobilePartManage.Checked = businessUnit.AllowMobilePartManagement;
                chkEnablePrevailingWages.Enabled = businessUnit.EnablePrevailingWages;
                enablePrevailingWagesTr.Visible = businessUnit.EnablePrevailingWages;
                chkInvoiceFeedbackIssues.Enabled = false;
                populate_ddl_acting_ram(0);
                if (businessUnit.country == "USA")
                {
                    ddl_currency.Value = 1;
                }
                else
                {
                    ddl_currency.Value = 2;
                }
                if (CanCreateCreditRebillWO)
                {
                    div_radiocredit.Visible = true;
                }

                combo_default_terms.Value = 0;
                Clear_all();
                btn_printquote.ClientVisible = false;
                if (!IsPostBack)
                {
                    if (_fromquoteid != "" && _fromquoteid != "0")
                    {
                        ddlquote.DataBind();
                        var selectedIndex = ddlquote.Items.IndexOfValue(Convert.ToInt32(_fromquoteid));
                        ddlquote.Value = Convert.ToInt32(_fromquoteid);
                        ddlquote.SelectedIndex = selectedIndex;
                        fill_from_quote();
                    }
                }
                chk_inspection_yes.Checked = businessUnit.UsesESACheck;
                chk_inspection_no.Checked = !businessUnit.UsesESACheck;
                var wo_type = (int)radiotype.Value;
                if (wo_type == 0)
                {
                    ddl_custram.ClientEnabled = true;
                    ddl_custram.Visible = true;
                    ddl_custram.ClientVisible = true;
                    ddl_rams.ClientEnabled = true;
                    ddl_rams.Visible = true;
                    ddl_rams.ClientVisible = true;
                }
                #endregion
            }
            else
            {
                ddlCustomer.ClientEnabled = false;
                ddlCustomer.ReadOnly = true;
                #region Existing work order
                spn_fixed_labour.Value = _wo.fixed_labour_rate;
                spn_material_markup.Value = _wo.fixed_material_markup;
                chk_fixed_labour.Checked = _wo.use_fixed_labour_rate;
                chk_fixed_markup.Checked = _wo.use_fixed_material_markup;
                chk_sustain.Checked = _wo.sustainability_project;


                chk_verbal.Checked = _wo.woprog_verbal_quote;
                spn_perc_complete.Value = _wo.woprog_timesheet_percentage.ToString();
                chk_inspection_yes.Checked = _wo.inspection_required;
                chk_inspection_no.Checked = !_wo.inspection_required;
                chkEnablePrevailingWages.Enabled = _wo.enable_prevailing_wages || businessUnit.EnablePrevailingWages;
                enablePrevailingWagesTr.Visible = _wo.enable_prevailing_wages || businessUnit.EnablePrevailingWages;
                chkEnablePrevailingWages.Checked = _wo.enable_prevailing_wages;
                chkInvoiceFeedbackIssues.Enabled = _wo.Status == OpsWOStatus.WaitingToBeInvoiced && CanToggleInvoiceIssues;
                tb_inspection_link.Text = _wo.inspection_link;
                chk_labor_only.Checked = _wo.labor_only;
                chk_mat_only.Checked = _wo.mat_only;
                chkInvoiceFeedbackIssues.Checked = _wo.invoice_fb_issues;
                chk_sub_only.Checked = _wo.sub_only;
                chkMobilePartManage.Checked = _wo.AllowMobilePartManagement;
                chkProgress.ClientEnabled = false;
                ddlCompany.ClientEnabled = false;
                ddl_currency.DataBind();
                ddl_currency.Value = _wo.currency_id;
                ddl_jobtag.Value = _wo.woprog_jobtag_id;
                var woCustomer = new NECustomer(Convert.ToInt32(_wo.WOProg_Customer_ID));
                var woBusinessUnit = new NeBusinessUnit(_wo.business_unit_id);
                txtwhyCloseReassign.Text = _wo.close_reassign_reason;
                var internalCustomerBusinessUnit = woCustomer.IsNEcompany ? new NeBusinessUnit(woCustomer.NEbusiness_unit_id) : new NeBusinessUnit();



                if (woCustomer.IsNEcompany)
                {
                    ddlCustomer.ClientEnabled = false;
                    if (_wo.parent_woprog_id != 0)
                    {
                        ddl_parent_workorder.ClientEnabled = false;

                    }

                    populate_parent_workorder(woCustomer.NEbusiness_unit_id);
                    ddl_parent_workorder.Value = _wo.parent_woprog_id;
                }
                if (woCustomer.IsNEcompany && internalCustomerBusinessUnit.id != woBusinessUnit.id)
                {
                    if (_wo.IsProgressBill)
                    {
                        // Task 1459: Parent WO drop down visible on internal Progress Bills
                        // This is a progress bill, so hide the 'parent wo dropdown list'
                        td_parent_info.Style["display"] = "none";
                    }
                    else
                    {
                        td_parent_info.Style["display"] = user.isContact ? "none" : "";
                    }
                }

                ddlCustomer.DataBind();
                populate_ddlCustomer(Convert.ToInt32(hidCompanyID.Value), _wo.WOProg_Customer_ID);


                ddlPM.DataBind();
                populate_ddlPM(Convert.ToInt32(hidCompanyID.Value), Convert.ToInt32(_wo.intProjectManager));

                if (_wo.WOProg_Customer_ID > 0 && ddlCustomer.Value == null)
                {
                    ddlCustomer.Value = _wo.WOProg_Customer_ID;
                }

                combo_default_terms.Value = _wo.term_id;

                populate_ddlquote(_wo.QuoteID);
                lblquoteterms.Text = HttpUtility.HtmlDecode(fill_terms(_wo.QuoteID)).Replace("</div><div>", "\n").Replace("<div>", "").Replace("</div>", "");

                if (_wo.bdm != 0)
                {
                    populate_ddl_customer_ram(_wo.bdm, true);
                }
                else if (woCustomer.s_regional_account_manager != 0)
                {
                    populate_ddl_customer_ram(woCustomer.s_regional_account_manager, true);
                }
                else
                {
                    populate_ddl_customer_ram(0, true);
                }

                populate_ddl_acting_ram(_wo.acting_ram);


                if (Convert.ToInt32(_wo.QuoteID) > 0)
                    sds_address.SelectCommand = "Select  0 as address_id,'Not Applicable' as address union Select address_id,address as address from vw_address where cust_id = @custid";

                populate_ddlAddress(_wo.WOProg_Customer_ID, _wo.woprog_Address_ID);
                populate_ddlContact(_wo.WOProg_Customer_ID, _wo.woprog_Contact_ID);

                combo_default_invoicetype.DataBind();
                if (combo_default_invoicetype.Items.FindByValue(_wo.default_invoicetype) == null)
                {
                    combo_default_invoicetype.Items.FindByValue(1).Selected = true;
                }
                else
                {
                    combo_default_invoicetype.Items.FindByValue(_wo.default_invoicetype).Selected = true;
                }

                chkProgress.Checked = _wo.IsProgressBill;
                tblProgress.Enabled = false;
                cbopb_type.ClientVisible = false;
                Td9.Visible = false;
                chk_warranty.Checked = Convert.ToBoolean(_wo.warranty);


                chk_cc.Checked = Convert.ToBoolean(_wo.woprog_creditcard_payment);
                dte_cutDate.ClientEnabled = false;
                var tempaddress = new NEAddress(_wo.woprog_Address_ID);
                if (woCustomer.Customer_Status == 4)
                {
                    Toolbox.FriendlyException(Response, "Customer has been merged, work order is unavailable.", "");
                }


                if (tempaddress.csp != null && woCustomer.Address.csp != null)
                {
                    if (tempaddress.csp.po_required || woCustomer.Address.csp.po_required)
                    {
                        lblCustPO.Text = "Customer PO (Req'd)";
                        lblCustPO.ForeColor = System.Drawing.Color.Red;
                    }
                }

                spndayscredit.Value = _wo.woprog_dayscredit;
                if (woCustomer.Credit_Type == 3)
                {
                    chk_cc.Checked = true;
                    chk_cc.ClientEnabled = false;
                    spndayscredit.Value = 0;
                    spndayscredit.ClientEnabled = false;
                    spndayscredit.ToolTip = "Customer may only pay by credit card upon or before completion of work.";
                }
                txtWODescription.Text = _wo.Description;
                this.txtwhyCloseReassign.Text = _wo.close_reassign_reason;
                if (_wo.woprog_isdownpayment)
                {
                    spndayscredit.Value = 0;
                    spndayscredit.ClientEnabled = false;
                    chkDownPayment.Checked = true;
                }
                txtAreainPlant.Text = _wo.woprog_Location_in_plant;
                txtSpecialInstruction.Text = _wo.special_instructions;
                chkRD.Checked = _wo.chkRD;
                chkShellWO.Checked = _wo.IsShell;
                ChkServiceCall.Checked = _wo.chkServiceCall;
                txtSalesValue.Text = _wo.woprog_expected_sales_value.ToString();
                txtlaborvalue.Text = _wo.woprog_exp_labor.ToString();
                txtCustPO.Text = _wo.PONumber;
                tbCustomerReference.Text = _wo.CustomerReferenceNumber;

                var custpo_edited_by = _wo.woprog_custpo_member_id == 0 ? new NeMember() : new NeMember(Convert.ToInt32(_wo.woprog_custpo_member_id));
                txtCustPO_notice.ToolTip = _wo.woprog_custpo_member_id == 0 ? "" : $"Last Edited: {custpo_edited_by.FullName} @ {_wo.woprog_custpo_dt:dd MMMM yyyy HH:mm:ss}";
                txtCustPO_notice.Visible = txtCustPO_notice.ToolTip != "";

                if (!Convert.IsDBNull(_wo.woprog_Expected_EndDate))
                    dteExpEndDate.Text = Convert.ToDateTime(_wo.woprog_Expected_EndDate.ToString()).ToString("yyyy-MM-dd");
                if (!Convert.IsDBNull(_wo.woprog_Expected_StartDate))
                    dteStartDate.Text = Convert.ToDateTime(_wo.woprog_Expected_StartDate.ToString()).ToString("yyyy-MM-dd");
                if (!Convert.IsDBNull(_wo.OrderDate))
                    dte_cutDate.Text = Convert.ToDateTime(_wo.OrderDate.ToString()).ToString("yyyy-MM-dd");
                //if (wo.Status == "Hold")
                chkOnHold.Checked = _wo.woprog_hold == 1;
                if (chkOnHold.Checked)
                {
                    txtwhyhold.Text = _wo.woprog_whyhold;
                    chkOnHold.ToolTip = "On Hold By: " + NeMember.GetMemberUserName(_wo.woprog_onholdmemberid);
                    txtwhyhold.ToolTip = "On Hold By: " + NeMember.GetMemberUserName(_wo.woprog_onholdmemberid);
                }
                if (chkProgress.Checked && !user.isContact)
                {
                    tblProgress.ClientVisible = true;
                    populate_ddlJobCostWO(_wo.business_unit_id, Convert.ToInt32(ddlCustomer.Value), _wo.woprog_associate_woprog_id);
                    load_associatedwo_details_fromwo();
                    chk_verbal.ClientEnabled = false;
                }
                else
                {
                    tblProgress.ClientVisible = false;
                }
                if (_wo.QuoteID == "0")
                {
                    btn_printquote.ClientVisible = false;
                }
                else
                {
                    combo_default_invoicetype.ClientEnabled = false;
                }
                //

                if (Toolbox.Contains(_wo.Status, new[]{    OpsWOStatus.Invoiced,
                                                            OpsWOStatus.WaitingToBeInvoiced,
                                                            OpsWOStatus.Deleted,
                                                            OpsWOStatus.ClosedReassigned
                                                            }))
                {
                    chkProgress.ClientEnabled = false;
                    chkRD.Enabled = false;
                    chkDownPayment.ClientEnabled = false;
                    ddlAddress.ClientEnabled = false;
                    ddlCompany.ClientEnabled = false;
                    ddlContact.ClientEnabled = false;
                    ddlCustomer.ClientEnabled = false;
                    ddlPM.ClientEnabled = false;
                    ddlquote.ClientEnabled = false;
                    picklist.Disabled = true;
                    quote_frame.Disabled = true;
                    chkPrint.Visible = false;
                    tbllaser.Visible = false;
                    btn_printdot.ClientVisible = false;
                    bt_delete_wo.Visible = false;
                    chkTax1.ClientEnabled = false;
                    chkTax2.ClientEnabled = false;
                    chkTax3.ClientEnabled = false;
                    chkTax4.ClientEnabled = false;
                    dte_cutDate.ClientEnabled = false;
                    dteExpEndDate.ClientEnabled = false;
                    dteStartDate.ClientEnabled = false;
                    txtlaborvalue.ClientEnabled = false;
                    btnSaveGeneral.ClientEnabled = false;
                    btnCloseandReassign.ClientEnabled = false;
                    //	txt_benchmark_credit0.ClientEnabled = false;
                }
                if (!CheckWOs(_wo.woprog_id))
                {
                    btnCloseandReassign.ClientEnabled = false;
                    txtwhyCloseReassign.ClientEnabled = false;
                }

                Tr5.Visible = false;
                Tr6.Visible = false;
                Tr7.Visible = false;


                #endregion Existing Work Order
            }
            #endregion General
        }
        else if (_tab_index == "Customer")
        {
            cust_frame.Attributes.Add("src", "/sections/customer/index.aspx?from_wo=true&customer_id=" + _wo.WOProg_Customer_ID);
        }
        else if (_tab_index == "Line Items")
        {
            var frameHeight = 0;
            if (_wo.Status != OpsWOStatus.Invoiced && _wo.Status != OpsWOStatus.WaitingToBeInvoiced)
            {
                frameHeight = Toolbox.doSQL_int(@"Select ifnull(count(wo_detail_current_id),0) from wo_detail_current  where wo_detail_current_woprog_id =@v0", new object[] { _wo.woprog_id });
            }
            else
            {
                frameHeight = Toolbox.doSQL_int(@"Select ifnull(count(wo_detail_history_id),0) from wo_detail_history  where wo_detail_history_woprog_id =@v0", new object[] { _wo.woprog_id });
            }

            frameHeight = 650 + frameHeight * 21;
            if (frameHeight < 800)
                frameHeight = 1050;
            if (frameHeight > 2500)
                frameHeight = 2550;
            picklist.Attributes.Add("height", frameHeight.ToString());
            picklist.Attributes.Add("src", "./picklist.aspx?id=" + _wo.woprog_id);
            picklist.Attributes.Add("onload", "autoResize(this)");

        }
        else if (_tab_index == "Quote")
        {
            #region Quote
            if (_wo.QuoteID != "0")
            {
                if (Convert.ToInt32(_wo.QuoteID) != 0 && Convert.ToInt32(_wo.QuoteID) != -1)
                {
                    try
                    {
                        var q = new quote(Convert.ToInt32(_wo.QuoteID));
                        quote_frame.Attributes.Add("src", "/#/opens/65/quotes/" + q.QuoteID + "/" + q.Revision);
                    }
                    catch (Exception ee)
                    {
                        Toolbox.do_errorLog_errorStack(ee);
                        lblError.Text = "Invalid Quote";
                        lblError.ClientVisible = true;
                        return;
                    }
                }
            }
            #endregion
        }
        else if (_tab_index == "Time Entries")
        {
            get_ts_comments(_wo.OrderNumber);
            chkPrint.Checked = print_timesheet_in_comments(_wo.woprog_id);
            btnSaveServiceDateText.Text = _wo.woprog_invoice_text_servicedates;
        }
        else if (_tab_index == "Quote Comparison")
        {
            if (_wo.QuoteID != "0")
            {
                comparison.Attributes.Add("src", "/sections/workorder/WorkSheetComparison.aspx?woid=" + _wo.woprog_id);
            }
        }
        else if (_tab_index.Substring(0, 2) == "PO")
        {
            grid_POs.DataSource = Toolbox.doSQL_dt(@"CALL page_workorder_po_tab(@v0)", new object[] { _wo.woprog_id });
            grid_POs.DataBind();
        }
        else if (_tab_index == "Project Notes")
        {
            mem_chatnewline.Text = "";
            try
            {
                mem_projectNotes.Text = Toolbox.doSQL_string(@"Select ifnull((Select woprog_project_notes_notes from woprog_project_notes  where woprog_project_notes_woprogid =@v0 and woprog_project_notes_type = 'W'),'')", new object[] { _wo.woprog_id });
            }
            catch
            {
                mem_projectNotes.Text = "";
            }
            get_comments(_wo.woprog_id.ToString());
        }
        else if (_tab_index == "Analysis")
        {

            uc_analysis.Visible = true;
            uc_analysis._current_user = user;
            uc_analysis.woprog_id = _wo.woprog_id;
            uc_analysis._wo = _wo;
            uc_analysis.DataBind();
            uc_analysis.load();
            // benchmark totals
            //     string strtable = String.Format("<table><tr><td>Labour:</td><td>{0}</td></tr><tr>Material:</td><td>{1}</td></tr></table>",
        }
        else if (_tab_index.Contains("Project Folder"))
        {

            files_frame.Attributes.Add("src", "/filemanager.aspx?parent_page=workorder&id=" + _wo.woprog_id);
        }
        else if (_tab_index == "R&D")
        {
            mem_Competancy.Text = Toolbox.do_value_from(_wo.WOProg_CoreCompetency, false);
            mem_Advancement.Text = Toolbox.do_value_from(_wo.advancement, false);
            mem_Uncertainty.Text = Toolbox.do_value_from(_wo.uncertainty, false);
        }
        else if (_tab_index.Contains("Linked WOs"))
        {
            if_associated.Attributes.Add("src", "/sections/workorder/if_associated.aspx?woprog_id=" + _wo.woprog_id + "&business_unit_id=" + _wo.business_unit_id);
        }
        else if (_tab_index == "History")
        {
            ProcessDistilledHistory();
            fill_parthistory(_wo.woprog_id);
        }
        else if (_tab_index == "Scope")
        {
            wo_tasklist1.Visible = true;
            wo_tasklist1.LoadPage();
        }
        else if (_tab_index == "Checklist")
        {
            close_checklist.Visible = true;
            close_checklist.woprog_id = _wo.woprog_id;
            close_checklist.member_id = user.id32;
            close_checklist.DataBind();
        }

    }
    private void ProcessDistilledHistory()
    {
        Toolbox.doSQL_void(@"CALL log.COMPARE_LOG_WODETAIL(@v0)", new object[] { _wo.woprog_id });
    }
    protected double Get_margin(double _still_to_be_billed, string _tablename)
    {
        var strsql = string.Format(@"SELECT
ifnull(sum(wo_detail_{0}_qty_committed * wo_detail_{0}_price_cost),0)
FROM
wo_detail_{0}
WHERE
wo_detail_{0}_woprog_id = @v0 and 
wo_detail_{0}_billtypeid NOT IN (3,6,8,9,11,12)", _tablename);
        var true_cost = Toolbox.doSQL_double(strsql, new object[] { _wo.woprog_id });
        var true_sell = _still_to_be_billed;

        return (true_sell - true_cost) / true_sell;
    }
    protected double Get_margin_whole_job(double true_sell, string _tablename)
    {
        var strsql = string.Format(@"SELECT
ifnull(sum(wo_detail_{0}_qty_committed * wo_detail_{0}_price_cost),0)
FROM
wo_detail_{0}
WHERE
wo_detail_{0}_woprog_id = @v0 and 
wo_detail_{0}_billtypeid NOT IN (3,6,8,9,11,12)", _tablename);
        var true_cost = Toolbox.doSQL_double(strsql, new object[] { _wo.woprog_id });

        return (true_sell - true_cost) / true_sell;
    }
    private void UpdateHeaderStatus(string _status)
    {
        lblStatus.Text = _status == OpsWOStatus.WaitingToBeInvoiced
                                ? _wo.invoice_fb_issues
                                    ? $"{_wo.Status} - Invoice Feedback Issues"
                                    : _wo.Status
                                : _wo.Status;
    }
    protected void update_header_totals()
    {
        try
        {
            using (var conn = Toolbox.connect())
            {
                var tablename = "current";
                UpdateHeaderStatus(_wo.Status);
                //		NeWOProg.update_header_totals(wo.woprog_id.ToString(), hidCompanyID.Value, wo.OrderNumber);  // update the woprog_header totals on woprog table
                //		NeSalesOrder newTotal = new NeSalesOrder();  // load sales order class
                //		wo			= new NeWOProg(wo.woprog_id);
                //		newTotal.getSalesOrderValues(wo.woprog_id.ToString());  // grab the totals from the work order for the use here.
                _wo.fast_update_header_totals();
                //NeWOProg.update_header_totals(_wo.woprog_id.ToString(), _wo.business_unit_id, _wo.OrderNumber);
                //	notification.broadcast(string.Format(@"{0} is accessing work order {1}", _current_user.FullName, _wo.OrderNumber), Application);
                //if (_wo.QuoteID != "0")
                //	{
                //	Toolbox.doSQL_void(@"Update woprog  set woprog.woprog_expected_sales_value =@v0  where woprog_id =@v1", new object[] { _wo.woprog_StillToBeBilled,_wo.woprog_id });
                //	}
                lbl_ToBeInvoiced.Text = "Balance to Invoice: " + _wo.woprog_StillToBeBilled.ToString("C2");
                var prev_woprog_id = _wo.woprog_id > 0 ? Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(woprog_id), 0) FROM woprog WHERE woprog_id < @v0  AND business_unit_id = @v1  LIMIT 1", new object[] { _wo.woprog_id, _wo.business_unit_id }) : 0;
                var next_woprog_id = _wo.woprog_id > 0 ? Toolbox.doSQL_int(conn, @"SELECT IFNULL(MIN(woprog_id), 0) FROM woprog WHERE woprog_id > @v0  AND business_unit_id = @v1  LIMIT 1", new object[] { _wo.woprog_id, _wo.business_unit_id }) : 0;
                var prev_wo_link = prev_woprog_id > 0 ? string.Format(@"<button title='Previous Work Order' type='button' style='margin-right:10px;' onclick=""please_wait('start');location.href='./index.aspx?woprog_id={0}';"">&lt;</button>", prev_woprog_id) : "";
                var next_wo_link = next_woprog_id > 0 ? string.Format(@"<button title='Next Work Order' type='button' style='margin-left:10px;' onclick=""please_wait('start');location.href='./index.aspx?woprog_id={0}';"">&gt;</button>", next_woprog_id) : "";
                if (Toolbox.Contains(_wo.Status, new string[] { OpsWOStatus.Rework,
                                                                OpsWOStatus.InitialPrep,
                                                                OpsWOStatus.QuestionsForPM,
                                                                OpsWOStatus.WaitingBMApproval,
                                                                OpsWOStatus.WaitingPMApproval
                                                              }))
                {
                    var scanlink = "<span style='font-size:18px;'>";
                    var endTag = "";
                    //  var buid = new NeBusinessUnit(_wo.business_unit_id);
                    NeBusinessUnit.CheckBUProcessFolderStructure(_wo.business_unit_id);
                    try
                    {
                        var fileServer = NeTaxEntity.BaseFolder(_wo.business_unit_id, false);
                        // the path should be basefolder with TE/TE2 folder structure
                        var dir = new DirectoryInfo(fileServer + "/WOs/");
                        var name = string.Format(@"{0}.pdf", _wo.woprog_id);
                        FileInfo[] found = dir.GetFiles(name);
                        if (found.Length > 0)
                        {
                            var scan = found.FirstOrDefault();
                            scanlink += "<a title ='Click to UNLINK Scan' href='../../wo_prog_rename.aspx?woprog_id=" + _wo.woprog_id + "&scanfile=" + scan + "&showPanName=true'>";
                            endTag = "</a>";
                        }
                        else
                        {
                            scanlink += "<span style='font-size:18px' title='This work order does not have a scan'>";
                            endTag = "";
                        }

                    }
                    catch (Exception ex)
                    {
                        Toolbox.do_errorLog_errorStack(ex);
                    }
                    var endspan = _wo.OrderNumber + endTag + "</span>";

                    lblWODisplay.Text = prev_wo_link + scanlink + endspan + next_wo_link;
                }
                else
                {
                    if (Toolbox.Contains(_wo.Status, new string[]{ OpsWOStatus.WaitingToBeInvoiced,
                                                                  OpsWOStatus.Invoiced
                                                                }))
                    {
                        tablename = "history";
                    }
                    lblWODisplay.Text = prev_wo_link + "<span style='font-size:18px;'>" + _wo.OrderNumber + "</span>" + next_wo_link;
                }
                if (user.isContact) // if member is a contact, or doesn't have bm or pm privys.. make rename link invisible
                {
                    lblWODisplay.Text = "<span style='font-size:18px;'>" + _wo.OrderNumber + "</span>";
                }
                lbl_lastmodified.Text = "Last Modified: " + Toolbox.doSQL_string(conn, @"SELECT IFNULL(woprog_ts, 'N/A') FROM woprog WHERE woprog_id = @v0", new object[] { _wo.woprog_id });
                img_lastmodified.Visible = _wo.woprog_id > 0 && (user.business_unit.is_backoffice || Toolbox.Contains(user.MemberTypeID, new[] { OpsMemberTypes.ProductManager, OpsMemberTypes.Programmer })) && !Toolbox.Contains(_wo.Status, new[] { OpsWOStatus.Invoiced, OpsWOStatus.Deleted });

                if (CanViewGross)
                {
                    //	var margin = Get_margin(_wo.woprog_StillToBeBilled, tablename);

                    //	margin = margin.ToString().Contains("E") || margin.ToString().Contains("Infinity") ? 0 : margin;
                    lbl_topmargin.Text = "Gross Margin This WO: " + _wo.woprog_grossmargin.ToString("P1");
                    if (_wo.QuoteID != "0")
                    {
                        var margin_whole = Get_margin_whole_job(Convert.ToDouble(_wo.QuotedPrice), tablename);
                        margin_whole = margin_whole.ToString().Contains("E") || margin_whole.ToString().Contains("Infinity") ? 0 : margin_whole;
                        lbl_top_whole_job.Text = "Gross Margin Total Job: " + margin_whole.ToString("P1");
                        if (_wo.woprog_grossmargin == margin_whole)
                        {
                            lbl_topmargin.Visible = false;
                        }
                    }

                }
                else
                {
                    lbl_topmargin.Text = "";
                }
            }
        }
        catch (Exception ee)
        {
            Toolbox.do_errorLog_errorStack(ee);
        }
    }
    protected void update_description_text()
    {
        if (_wo.Status == OpsWOStatus.InitialPrep)
        {
            var desc = "";
            if (_wo.QuoteID == "0")  // if it's a T&M Work Order
            {
                if (!_wo.Description.Contains("Contact:"))
                {
                    string daterange;
                    try
                    {
                        var start_date = Toolbox.doSQL_string(@"Select Date from membertime  where membertime_woprog_id =@v0 or membertime_child_workorder_id =@v1  order by Date limit 1", new object[] { _wo.woprog_id, _wo.OrderNumber });
                        var end_date = Toolbox.doSQL_string(@"Select Date from membertime  where membertime_woprog_id =@v0 or membertime_child_workorder_id =@v1  order by Date desc limit 1", new object[] { _wo.woprog_id, _wo.OrderNumber });
                        if (start_date == end_date)
                        {
                            daterange = Convert.ToDateTime(start_date).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            daterange = Convert.ToDateTime(start_date).ToString("yyyy-MM-dd") + " to " + Convert.ToDateTime(end_date).ToString("yyyy-MM-dd");
                        }
                        desc = _wo.Description + Environment.NewLine + "Contact: " + new NEContact(_wo.woprog_Contact_ID).Contact_Name + Environment.NewLine + "Service Dates: " + daterange;
                        Toolbox.doSQL_void(@"Update woprog  set woprog_description =@v0  where woprog_id =@v1", new object[] { desc, _wo.woprog_id });
                        txtWODescription.Text = desc;
                    }
                    catch (Exception ee)
                    {
                        Toolbox.do_errorLog(ee, "Error while trying to update description");
                    }
                }
            }
            else
            {
                if (!_wo.Description.Contains("As per Quote"))
                {
                    var quotestuff = _wo.Description + Environment.NewLine + "As per Quote # Q-" + _wo.QuoteID.Substring(0, 6) + " V" + _wo.QuoteID.Substring(6, 1) + " " + Convert.ToDouble(_wo.QuotedPrice).ToString("C2") + Environment.NewLine + "Contact: " + new NEContact(_wo.woprog_Contact_ID).Contact_Name;
                    desc = quotestuff;
                    Toolbox.doSQL_void(@"Update woprog  set woprog_description =@v0  where woprog_id =@v1", new object[] { desc, _wo.woprog_id });
                }
            }
            if (_wo.woprog_ERID != 0)
            {
                if (!_wo.Description.Contains("As per ER"))
                {
                    var quotestuff = _wo.Description + Environment.NewLine + "As per ER # " + _wo.woprog_ERID + " " + Environment.NewLine + "Contact: " + new NEContact(_wo.woprog_Contact_ID).Contact_Name;
                    desc = quotestuff;
                    Toolbox.doSQL_void(@"Update woprog  set woprog_description =@v0  where woprog_id =@v1", new object[] { desc, _wo.woprog_id });
                }
            }
        }
    }
    protected void btnSaveGeneral_Click(object _sender, EventArgs _e)
    {
        validate_wo();
        if (lblError.Text == "")
        {
            if (hidWOProgID.Value == "0")
            {
                //		if (ex_ddl(ddlCompany).ToString()== "8"|| ex_ddl(ddlCompany).ToString() == "38")
                //		{
                CreateWorkOrder(true);
                //		}
                //		else
                //		{
                //			create_workorder(true);
                //		}
            }
            else
            {
                save_workorder();
            }
        }

        ShowOrHideParentWorkOrderDropDownBasedOnProgressBillCheckbox();
    }
    int _woprog_associated_woprog_id = 0;
    protected void save_workorder()
    {
        using (var conn = Toolbox.connect())
        {
            var taxinfo = new NEAddress();

            #region Company Checking
            var wo_company = new NeBusinessUnit();
            try
            {
                wo_company = new NeBusinessUnit(Convert.ToInt32(ex_ddl(ddlCompany)));
            }
            catch
            {
                lblError.Text = "Company value not defined.";
                lblError.Visible = true;
                lblError.ClientVisible = true;
                ddlCompany.Focus();
                return;
            }
            #endregion Company Checking

            #region Tax Checking
            try
            {
                taxinfo = new NEAddress(Convert.ToInt32(ex_ddl(ddlAddress)));
                if (taxinfo.id == 0)
                {
                    lblError.Text = "Something is wrong with the address on this work order";
                }
                if (hidWOProgID.Value == "0")
                {

                    chkTax1.Checked = wo_company.Tax1 == taxinfo.Tax1_Consol && taxinfo.Tax1_Consol != 0 && string.IsNullOrEmpty(taxinfo.Tax1Exempt.Trim());
                    chkTax2.Checked = wo_company.Tax2 == taxinfo.Tax2_Consol && taxinfo.Tax2_Consol != 0 && string.IsNullOrEmpty(taxinfo.Tax2Exempt.Trim());
                    chkTax3.Checked = wo_company.Tax3 == taxinfo.Tax3_Consol && taxinfo.Tax3_Consol != 0 && string.IsNullOrEmpty(taxinfo.Tax3Exempt.Trim());
                    chkTax4.Checked = wo_company.Tax4 == taxinfo.Tax4_Consol && taxinfo.Tax4_Consol != 0 && string.IsNullOrEmpty(taxinfo.Tax4Exempt.Trim());

                }
                if (taxinfo.Tax1 == taxinfo.Tax2 && taxinfo.Tax1 != 0 || taxinfo.Tax1_Consol == taxinfo.Tax2_Consol && taxinfo.Tax1_Consol != 0)
                {
                    lblError.Text = "Duplicate Tax Settings, Check Customer Settings";
                    lblError.Visible = true;
                    lblError.ClientVisible = true;
                    return;
                }
            }
            catch
            {
                lblError.Text = "Tax value not defined.";
                ddlAddress.Focus();
                return;
            }
            #endregion Tax Checking

            var woObj = new NeWOProg(Convert.ToInt32(hidWOProgID.Value));
            // Setting order_number
            order_number = hidWOProgID.Value.PadLeft(3, '0').ToString();

            #region Checking if work order has been updated since last load.
            if (hid_ts.Value != woObj.woprog_ts.Ticks.ToString())
            {
                lblError.Text = "The information in this work order has changed since last accessed. <br/> You may want to <a href='javascript:location.href = location.href;'>refresh</a> the page so as not to cause a conflict";
                lblError.Visible = true;
                lblError.ClientVisible = true;
                return;
            }
            #endregion Checking if work order has been updated since last load.
            _woprog_associated_woprog_id = woObj.woprog_associate_woprog_id;
            var checkpoints = new List<string>();
            // Capture current quote_id
            var pre_save_quoteid = woObj.QuoteID.Trim();
            var from_customer = new NECustomer(woObj.WOProg_Customer_ID);
            var this_customer = new NECustomer(Convert.ToInt32(ex_ddl(ddlCustomer)));
            var pre_quote_is_set = pre_save_quoteid != "" && pre_save_quoteid != "0";
            var int_pre_save_quoteid = pre_quote_is_set ? Convert.ToInt32(pre_save_quoteid) : 0;

            var quotedamount = 0.0;
            var hoursspentonqote = 0.0;
            var error_flag = false;
            var cc = 0;
            var dsnnew = "";
            dsnnew = wo_company.DSN;
            if (chk_cc.Checked)
            {
                cc = 1;
            }
            lblError.ClientVisible = true;
            var ddl_quote_value = 0;
            try
            {
                if (ex_ddl(ddlquote).ToString().Contains("V"))
                {
                    try
                    {
                        ddl_quote_value = Convert.ToInt32(ddlquote.Value.ToString().Substring(0, 8).Trim().Replace("V", ""));
                    }
                    catch (Exception ee)
                    {
                        ddl_quote_value = 0;
                    }
                }
                else
                {
                    ddl_quote_value = Convert.ToInt32(ex_ddl(ddlquote));
                }
            }
            catch
            {
                lblError.Text = "Quote value not defined.";
                ddlquote.Focus();
                return;
            }

            double prev_qty_billed = 0;
            double prev_sell_billed = 0;
            var c_prev_billed = Toolbox.doSQL_int(conn, @"SELECT COUNT(wo_detail_current_id) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND (wo_detail_current_billtypeid = 3 or wo_detail_current_billtypeid = 11) AND wo_detail_current_master_id = 2139", new object[] { woObj.woprog_id });
            var c_associated = Toolbox.doSQL_int(conn, @"SELECT COUNT(woprog_id) FROM woprog WHERE woprog_associate_woprog_id = @v0 ", new object[] { woObj.woprog_id });
            var c_associated_open = Toolbox.doSQL_int(conn, @"SELECT COUNT(woprog_id) FROM woprog WHERE woprog_associate_woprog_id = @v0  AND woprog_status = 'Open'", new object[] { woObj.woprog_id });
            if (c_prev_billed > 0)
            {
                var dt_prev_billed = Toolbox.doSQL_dt(conn, @"SELECT wo_detail_current_qty_committed qty, wo_detail_current_price_sell sell FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND (wo_detail_current_billtypeid = 3 or wo_detail_current_billtypeid = 11) AND wo_detail_current_master_id = 2139", new object[] { woObj.woprog_id });
                if (dt_prev_billed.Rows.Count > 0)
                {
                    var dr_prev_billed = dt_prev_billed.Rows[0];
                    prev_qty_billed = 1 - Convert.ToDouble(dr_prev_billed["qty"]);
                    prev_sell_billed = Convert.ToDouble(dr_prev_billed["sell"]);
                }
            }
            if (pre_save_quoteid != "0" && (ddl_quote_value == 0 || pre_quote_is_set && ddl_quote_value != int_pre_save_quoteid))
            {
                var dt = Toolbox.doSQL_dt(conn, @"SELECT woprog_id, woprog_bvwo FROM woprog WHERE woprog_associate_woprog_id = @v0 ", new object[] { woObj.woprog_id });
                foreach (DataRow dr in dt.Rows)
                {
                    var woprog_id = Convert.ToInt32(dr["woprog_id"]);
                    #region Add history line for this progress billing to the jobcost.
                    var progbill_statusline = Toolbox.doSQL_string(conn, @" SELECT CAST( CONCAT ( 'Progress Billed WO: ', RIGHT(woprog_bvwo,6), ' Invoice: ', RIGHT(IFNULL(woprog_invoiceno,'N/A'),6), ' $', FORMAT(woprog_invoicednettotal,2), IF(woprog_quotedamount=0, '', CONCAT(' (',ROUND((woprog_invoicednettotal/woprog_quotedamount)*100,3),'%)') ) ) as CHAR) line_text FROM woprog WHERE woprog_id = @v0  ", new object[] { woprog_id });
                    Toolbox.doSQL_void(conn, @" INSERT INTO woprogstatus ( woprogstatus_woprog_id, woprogstatus_member_id, woprogstatus_status, woprogstatus_datetime, note ) VALUES ( @v0 , @v1 , 'Quote Unlinked - Removed Info', NOW(), @v2  )", new object[] { woObj.woprog_id, user.id, progbill_statusline });
                    #endregion Add history line for this progress billing to the jobcost.
                }
                if (ddl_quote_value == 0 && from_customer.id != this_customer.id && woObj.parent_woprog_id > 0) // Moving to a T&M job & is interco
                {
                    // Need to check if there is time allotted to this work order already.
                    var time_entries = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM membertime WHERE membertime_woprog_id = @v0 ", new object[] { woObj.woprog_id });
                    if (time_entries > 0)
                    {
                        shared.alert_invoicing("Unsuccessful customer change on work order", string.Format(@"{0} just tried to change the customer on WO#:{1} from {2} to {3}, please contact them.", user.FullName, woObj.OrderNumber, from_customer.Customer_Name, this_customer.Customer_Name));
                        lblError.Text = string.Format("This work order is currently linked to ({0}) time entry(s), the customer cannot be changed until the timesheet entries have been dealt with. An email has been sent to invoicing", time_entries);
                        return;
                    }
                }
            }
            if (c_prev_billed > 0 && ddl_quote_value != 0 && pre_quote_is_set && ddl_quote_value != int_pre_save_quoteid)
            {
                ddlquote.DataBind();
                //lblError.Text = "You cannot remove a quote link when there have already been progress billings. Please contact a member of NESI to discuss your options.";
                var linked_wos = new List<string>();
                if (c_associated > 0 && c_associated_open == 0)
                {
                    #region All are invoiced, just unlink them from woprog_associate_woprog_id and add them to wo_associated.
                    var dt = Toolbox.doSQL_dt(conn, @"SELECT woprog_id, woprog_bvwo FROM woprog WHERE woprog_associate_woprog_id = @v0 ", new object[] { woObj.woprog_id });
                    foreach (DataRow dr in dt.Rows)
                    {
                        var woprog_id = Convert.ToInt32(dr["woprog_id"]);
                        var woprog_bvwo = dr["woprog_bvwo"].ToString();
                        var wo_assoc = new wo_associated();
                        wo_assoc.woprog_id_a = woObj.woprog_id;
                        wo_assoc.woprog_id_b = woprog_id;
                        //		wo_assoc.save();  commented out by andy
                        var wo_comment = string.Format(@"Quote #{0} was removed by {1}, the progress billing WO: #{2} has been removed", woObj.QuoteID, user.FullName, woprog_bvwo);
                        Toolbox.doSQL_void(conn, @" INSERT INTO woprogcomment ( woprogcomment_woprog_id, woprogcomment_member_id, woprogcomment_text, woprogcomment_x, woprogcomment_y, woprogcomment_datetime ) VALUES ( @v0 , @v1 , @v2 , 0, 0, NOW() )", new object[] { woObj.woprog_id, user.id, wo_comment });
                        linked_wos.Add(woprog_bvwo);
                    }
                    Toolbox.doSQL_void(conn, @"UPDATE woprog SET woprog_associate_woprog_id = 0 WHERE woprog_associate_woprog_id = @v0 ", new object[] { woObj.woprog_id });
                    #endregion
                }
                else if (c_associated > 0 && c_associated_open > 0)
                {
                    #region A little more work, we need to get all the invoiced work orders and run through the procedure above.
                    var dt = Toolbox.doSQL_dt(conn, @"SELECT woprog_id, woprog_bvwo FROM woprog WHERE woprog_associate_woprog_id = @v0  AND woprog_status = 'Invoiced'", new object[] { woObj.woprog_id });
                    foreach (DataRow dr in dt.Rows)
                    {
                        var woprog_id = Convert.ToInt32(dr["woprog_id"]);
                        var woprog_bvwo = dr["woprog_bvwo"].ToString();
                        var wo_assoc = new wo_associated();
                        wo_assoc.woprog_id_a = woObj.woprog_id;
                        wo_assoc.woprog_id_b = woprog_id;
                        //		wo_assoc.save(); commented out by andy

                        var wo_comment = string.Format(@"Quote #{0} was removed by {1}, the progress billing WO #{2}'s link has been removed", woObj.QuoteID, user.FullName, woprog_bvwo);
                        Toolbox.doSQL_void(conn, @" INSERT INTO woprogcomment ( woprogcomment_woprog_id, woprogcomment_member_id, woprogcomment_text, woprogcomment_x, woprogcomment_y, woprogcomment_datetime ) VALUES ( @v0 , @v1 , @v2 , 0, 0, NOW() )", new object[] { woObj.woprog_id, user.id, wo_comment });
                        linked_wos.Add(woprog_bvwo);
                    }
                    // Then we need to run through all the open progress billings and delete them.
                    dt = Toolbox.doSQL_dt(conn, @"SELECT woprog_id, woprog_bvwo FROM woprog WHERE woprog_associate_woprog_id = @v0  AND woprog_status != 'Invoiced'", new object[] { woObj.woprog_id });
                    foreach (DataRow dr in dt.Rows)
                    {
                        var woprog_id = Convert.ToInt32(dr["woprog_id"]);
                        var woprog_bvwo = dr["woprog_bvwo"].ToString();
                        var wo_assoc = new wo_associated();
                        wo_assoc.woprog_id_a = woObj.woprog_id;
                        wo_assoc.woprog_id_b = woprog_id;
                        //			wo_assoc.save(); commented out by andy
                        Toolbox.doSQL_void(conn, @"UPDATE woprog SET woprog_status = 'Deleted' WHERE woprog_id = @v0 ", new object[] { woprog_id });
                        var wo_comment = string.Format(@"Quote #{0} was removed by {1}, the progress billing WO #{2}'s link has been removed", woObj.QuoteID, user.FullName, woprog_bvwo);
                        Toolbox.doSQL_void(conn, @" INSERT INTO woprogcomment ( woprogcomment_woprog_id, woprogcomment_member_id, woprogcomment_text, woprogcomment_x, woprogcomment_y, woprogcomment_datetime ) VALUES ( @v0 , @v1 , @v2 , 0, 0, NOW() )", new object[] { woObj.woprog_id, user.id, wo_comment });
                        linked_wos.Add(woprog_bvwo);
                    }
                    Toolbox.doSQL_void(conn, @"UPDATE woprog SET woprog_associate_woprog_id = 0 WHERE woprog_associate_woprog_id = @v0 ", new object[] { woObj.woprog_id });
                    #endregion
                }
                if (linked_wos.Count > 0)
                {
                    shared.alert_invoicing("Quote was unlinked from job cost while having progress billings.", string.Format(@"
	<table>
		<tr>
			<td><b>Job Cost:</b></td>
			<td>{0}</td>
		</tr>
		<tr>
			<td><b>Previous Quote #:</b></td>
			<td>{1}</td>
		</tr>
		<tr>
			<td><b>Who unlinked?:</b></td>
			<td>{2}</td>
		</tr>
		<tr>
			<td><b>Affected progress billings:</b></td>
			<td>{3}</td>
		</tr>
		<tr>
			<td><b>Open Progress Billings Deleted?:</b></td>
			<td>{4}</td>
		</tr>
	</table>
All progress billings are linked on the 'Linked WOs' tab, inside the job cost work order.<br/>
Credits will (most likely) need to be added.
",
                woObj.OrderNumber,
                woObj.QuoteID,
                user.FullName,
                linked_wos.Aggregate((_a, _x) => _a + "," + _x),
                c_associated_open > 0
                        ));
                }
            }
            quote tempquote2 = new quote();
            quote previous_quote;
            double this_qty_billed = 1;
            if (ddl_quote_value > 0)  // this job is linked to a quote
            {
                tempquote2 = new quote(ddl_quote_value);
                quotedamount = Convert.ToDouble(tempquote2.Price);
                // Check to see if this has any Progress Bills / Down Payments out there
                previous_quote = new quote(Convert.ToInt32(pre_save_quoteid));
                if (c_associated > 0)
                {
                    // If so, we need to do a little footwork here to achieve a correct quantity, otherwise it defaults to 1....
                    var previous_quotevalue = Convert.ToDouble(previous_quote.Price);
                    if (quotedamount != prev_sell_billed)
                    {
                        this_qty_billed = 1 - Math.Round(prev_sell_billed / quotedamount, 2);
                        if (this_qty_billed < 0)
                        {
                            ddlquote.DataBind();
                            lblError.Text = "You cannot link a quote with a lower quoted value than the original quote, when there have already been progress billings.";
                            return;
                        }
                    }
                }
            }


            #region Unreceive Quote
            try
            {
                if (pre_save_quoteid != "0" && pre_save_quoteid != ddl_quote_value.ToString()) // if the quote has changed.. we need to do some work.
                {
                    quote_unattach(conn, pre_save_quoteid, ddl_quote_value.ToString());
                    var tempquote1 = new quote(Convert.ToInt32(pre_save_quoteid));
                    var quote_history_information = "UnReceived from WO:" + order_number + " - (" + pre_save_quoteid + ") was replaced by (" + ddl_quote_value + ")";
                    try
                    {
                        Toolbox.doSQL_void(@"INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event)  VALUES(now(),@v0,@v1,@v2,@v3)", new object[] { user.id, tempquote1.QuoteID, tempquote1.Revision, quote_history_information });
                    }
                    catch (Exception ex)
                    {
                        Toolbox.do_errorLog_errorStack(ex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                        return;

                    }
                    //  Toolbox.doSQL_void(@"delete from woprog_tasks  where woprog_id =@v0", new object[] { _wo.woprog_id });
                }
            }
            catch (Exception ex4)
            {
                log_error("Error UnReceiving Quote for: " + woObj.OrderNumber + " from " + wo_company.DSN + ": ", ex4);
                error_flag = true;
                checkpoints.Add("Couldn't unreceive quote");
            }
            #endregion Unreceive Quote
            #region Receive Quote
            var quote_was_received = false;
            try
            {
                // Get the current count for 2139 lines
                var n_lines_q = Toolbox.doSQL_int(conn, @"SELECT COUNT(wo_detail_current_id) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = 'Q' AND (wo_detail_current_billtypeid = 3 or wo_detail_current_billtypeid = 11) AND wo_detail_current_master_id = 2139", new object[] { woObj.woprog_id });
                if (n_lines_q == 0)
                {
                    if (ddl_quote_value > 0 && pre_save_quoteid != ddl_quote_value.ToString())
                    {
                        quote_was_received = true;
                        var quotesuccess = new quote().ReceiveNeQuotes(ddl_quote_value,
                            woObj.woprog_id,
                                                                            txtCustPO.Text,
                                                                            user.id,
                                                                            Convert.ToInt32(ex_ddl(ddlContact)),
                                                                            wo_company.DSN
                                                                            );
                        tempquote2 = new quote(ddl_quote_value);
                        quotedamount = Convert.ToDouble(tempquote2.Price);
                        var quoteObj = new quote(ddl_quote_value);
                        var is_tm = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(is_tm),0) FROM quote_master WHERE quote_id = @v0 AND active_revision = 1", new object[] { quoteObj.QuoteID }) > 0;

                        if (quotesuccess && !is_tm)
                        {
                            #region Add 2139 - (Price As Quoted) Line
                            Toolbox.doSQL_void(conn, @" UPDATE wo_detail_current SET wo_detail_current_billtypeid = 1 WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_billtypeid = 0", new object[] { woObj.woprog_id });
                            var quote_line = new NeWODetailCurrent();
                            var linecounts = quote_line.GetLineCount(woObj.woprog_id);
                            quote_line.tax1 = wo_company.TaxQuotedJobs == 1 ? woObj.woprog_tax1 : 0;
                            quote_line.tax2 = wo_company.TaxQuotedJobs == 1 ? woObj.woprog_tax2 : 0;
                            quote_line.tax3 = wo_company.TaxQuotedJobs == 1 ? woObj.woprog_tax3 : 0;
                            quote_line.tax4 = wo_company.TaxQuotedJobs == 1 ? woObj.woprog_tax4 : 0;
                            quote_line.description = "Price as Quoted Quote: " + ddl_quote_value;
                            quote_line.master_id = 2139;
                            quote_line.code = "QUOTE";
                            quote_line.business_unit_id = woObj.business_unit_id;
                            quote_line.qty_committed = this_qty_billed;
                            quote_line.qty_invoiced = this_qty_billed;
                            quote_line.type = "Q";
                            quote_line.cost = 0;// quotedamount - (quotedamount * tempquote2.margin);
                            quote_line.sell = quotedamount;
                            quote_line.unit = quotedamount;
                            quote_line.woprog_id = woObj.woprog_id;
                            quote_line.bvwo = Convert.ToInt32(woObj.OrderNumber);
                            quote_line.origin = "Automatically Added";
                            quote_line.billtypeid = 11;
                            quote_line.rec_no = linecounts + 2;
                            quote_line.added_by = user.id;
                            quote_line.save(user, "/sections/workorder/index.aspx.cs - create_workorder #1", false);
                            #endregion Add 2139 Line

                            #region if parent child add quote to parent

                            if (woObj.parent_woprog_id != 0)
                            {
                                // add quote line to the parent WO AS A 9595
                                var quote_lineForParent = new NeWODetailCurrent();
                                var parentWO = new NeWOProg(woObj.parent_woprog_id);
                                //linecounts = quote_lineForParent.GetLineCount(workorder1.parent_woprog_id);
                                quote_lineForParent.tax1 = wo_company.TaxQuotedJobs == 1 ? parentWO.woprog_tax1 : 0;
                                quote_lineForParent.tax2 = wo_company.TaxQuotedJobs == 1 ? parentWO.woprog_tax2 : 0;
                                quote_lineForParent.tax3 = wo_company.TaxQuotedJobs == 1 ? parentWO.woprog_tax3 : 0;
                                quote_lineForParent.tax4 = wo_company.TaxQuotedJobs == 1 ? parentWO.woprog_tax4 : 0;
                                quote_lineForParent.description = "Price as Quoted Quote: " + ddl_quote_value;
                                quote_lineForParent.master_id = OpsSpecialPart.NewChildWO;
                                quote_lineForParent.code = ddl_quote_value.ToString();
                                quote_lineForParent.business_unit_id = parentWO.business_unit_id;
                                quote_lineForParent.qty_committed = this_qty_billed;
                                quote_lineForParent.qty_invoiced = this_qty_billed;
                                quote_lineForParent.type = "M";
                                quote_lineForParent.notes = "Automatically Added from child";
                                quote_lineForParent.cost = quotedamount;
                                var quoteSell = Toolbox.doSQL_double("SELECT GetSellPriceFromWo(@v0,0,1,@v1,@v2)", new object[] { quotedamount, parentWO.business_unit_id, parentWO.woprog_id });
                                quote_lineForParent.sell = quoteSell;
                                quote_lineForParent.unit = quoteSell;
                                quote_lineForParent.woprog_id = parentWO.woprog_id;
                                quote_lineForParent.bvwo = Convert.ToInt32(parentWO.OrderNumber);
                                quote_lineForParent.origin = "Automatically Added from child";
                                quote_lineForParent.billtypeid = 0; //Should be 1 if parent is quoted job
                                                                    //quote_lineForParent.rec_no = linecounts + 2;
                                quote_lineForParent.added_by = user.id;
                                //  quote_lineForParent.save(_current_user, "/sections/workorder/index.aspx.cs - create_workorder #1", false);
                                //  NeWODetailCurrent.reorder_lines(parentWO.woprog_id);


                                //Remove labor lines from child on parent WO
                                //pay type_id and member id

                                var allLaborLinesOnChild = Toolbox.doSQL_dt(
                                    @"SELECT a.wo_detail_current_id,a.wo_detail_current_qty_committed,a.wo_detail_current_qty_ordered,a.wo_detail_current_qty_invoiced, a.memberid, b.paytype_id 
FROM wo_detail_current a
JOIN membertype_chargeout b ON a.wo_detail_current_master_id = b.id
WHERE a.wo_detail_current_woprog_id = @v0 
AND a.wo_detail_current_type = 'L'", new object[] { woObj.woprog_id });

                                foreach (DataRow woDetailObj in allLaborLinesOnChild.Rows)
                                {

                                    //var woDetail = new NeWODetailCurrent( Convert.ToInt32(woDetailObj["wo_detail_current_id"]));



                                    Toolbox.doSQL_void(@"UPDATE wo_detail_current a  
JOIN membertype_chargeout b ON a.wo_detail_current_master_id = b.id
SET a.wo_detail_current_qty_committed = (a.wo_detail_current_qty_committed - @v1),
a.wo_detail_current_qty_ordered = (a.wo_detail_current_qty_ordered - @v2),
a.wo_detail_current_qty_invoiced = (a.wo_detail_current_qty_invoiced - @v3)
WHERE a.wo_detail_current_woprog_id = @v0 
AND a.wo_detail_current_type = 'L' AND a.memberid = @v4 AND b.paytype_id = @v5", new object[]
                                    {
                                        woObj.parent_woprog_id, woDetailObj["wo_detail_current_qty_committed"],
                                        woDetailObj["wo_detail_current_qty_ordered"], woDetailObj["wo_detail_current_qty_invoiced"],
                                        woDetailObj["memberid"], woDetailObj["paytype_id"]
                                    });
                                }

                                woObj.fast_update_header_totals();

                                new NeWOProg(woObj.parent_woprog_id).fast_update_header_totals();
                            }

                            #endregion
                            //All of this code does nothing
                            //                     if (linecounts > 0)
                            //{
                            //	#region 2139 line exists,
                            //	var lineid = Toolbox.doSQL_int(@"SELECT wo_detail_current_id FROM wo_detail_current  WHERE wo_detail_current_woprog_id=@v0 AND wo_detail_current_rec_no =@v1  AND wo_detail_current_master_id = 2139", new object[] { workorder1.woprog_id, quote_line.rec_no });
                            //	var lastlabor = 0;
                            //	var firstpart = 0;
                            //	var lastquote = 0;
                            //	var newinv = new inventory();
                            //	var linetable = Toolbox.doSQL_dt(@"SELECT wo_detail_current_master_id, wo_detail_current_code, wo_detail_current_rec_no FROM wo_detail_current  WHERE wo_detail_current_woprog_id=@v0 ORDER BY wo_detail_current_rec_no", new object[] { workorder1.woprog_id });
                            //	foreach (DataRow row in linetable.Rows)
                            //	{
                            //		var this_rec_no = Convert.ToInt32(row["wo_detail_current_rec_no"]);
                            //		var this_master_id = row["wo_detail_current_master_id"].ToString();
                            //		if (this_rec_no > 99000)
                            //		{
                            //			lastlabor = this_rec_no;
                            //		}
                            //		else if (this_master_id == "2139")
                            //		{
                            //			lastquote = this_rec_no;
                            //		}
                            //		else if (lastlabor != 0)
                            //		{
                            //			firstpart = this_rec_no;
                            //			break;
                            //		}
                            //	}
                            //	if (firstpart == 0)
                            //	{
                            //		//quote_line.SwitchRecNo(linecounts + 2, 2, workorder1.woprog_id, lineid);
                            //	}
                            //	else
                            //	{
                            //		//quote_line.SwitchRecNo(linecounts + 2, firstpart, workorder1.woprog_id, lineid);
                            //		//quote_line.SwitchRecNo(firstpart, 2, workorder1.woprog_id, lineid);
                            //	}
                            //	#endregion 2139 line exists,
                            //}
                        }
                        #region build task list
                        var quote_details = Toolbox.doSQL_dt(@"Select linetext,line_number from quote_extratext  where quote_id =@v0 and revision =@v1  and type =1 order by line_number", new object[] { tempquote2.QuoteID, tempquote2.Revision });
                        foreach (DataRow qd in quote_details.Rows)
                        {
                            Toolbox.doSQL_void(@"Insert into woprog_tasks (woprog_id,task,complete,_order)  Values(@v0,@v1,0,@v2)", new object[] { _wo.woprog_id, qd["linetext"], qd["line_number"] });
                        }
                        #endregion

                        #region Update Quote History
                        var pmmember = new NeMember(Convert.ToInt32(ex_ddl(ddlPM)));
                        var quote_history_information = string.Format(@"Received - WO:{0}  PM:{1} Start Date:{2} End Date:{3}", order_number, pmmember.FullName, dteStartDate.Text, dteExpEndDate.Text);
                        var sql_quote_history = string.Format(@"
INSERT INTO quote_history 
	(
	create_datetime, 
	created_by, 
	quote_id, 
	revision, 
	event
	) 
VALUES
	(
	NOW(),
	{0},
	{1},
	{2},
	""{3}""
	)",
                        user.id,
                        tempquote2.QuoteID,
                        tempquote2.Revision,
                        quote_history_information);
                        try
                        {
                            Toolbox.doSQL_void(@" INSERT INTO quote_history ( create_datetime, created_by, quote_id, revision, event ) VALUES ( NOW(), @v0 , @v1 , @v2 , @v3  )", new object[] { user.id, tempquote2.QuoteID, tempquote2.Revision, quote_history_information });
                        }
                        catch (Exception ex)
                        {
                            error_flag = true;
                            checkpoints.Add("Couldn't add quote_history line item");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                            return;
                        }
                        #endregion Update Quote History
                        #region Send Email
                        if (ex_ddl(ddlCompany).ToString() == "1")
                        {
                            var mail = new NeEMail();
                            if (Toolbox.app_setting("debug_redirect") == "1")
                                mail.To = Toolbox.app_setting("debug_redirect_email");
                            else
                                mail.To = EmailID.OakQuoteUpdate + Toolbox.app_setting("DomainForEmail");
                            mail.Subject = "A Quote Has Been Added to a Work Order";
                            mail.From = "administrator@" + Toolbox.app_setting("DomainForEmail");

                            var body = "Cut - WO:" + order_number + "\n";
                            try
                            {
                                body += "Customer Name: " + Toolbox.doSQL_string(conn, @"SELECT customer_name FROM customer  where Customer_ID =@v0", new object[] { ex_ddl(ddlCustomer) }) + "\n";
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                                return;
                            }
                            body += " Description: " + tempquote2.txtJobDescription + "\n";
                            body += " StartDate:" + dteStartDate.Text + "\n";
                            body += "\n";
                            body += "QUOTE ID: " + tempquote2.QuoteID + "\n";
                            body += "Revision: " + tempquote2.Revision + "\n";
                            body += "Value of Quote: " + tempquote2.Price + "\n";
                            body += "Price Type: " + tempquote2.price_type + "\n";
                            body += "Work Sheet Total T+M: ";
                            double worksheettandm = 0;
                            try
                            {
                                worksheettandm = Toolbox.doSQL_double(conn, @"SELECT
  IFNULL(SUM(a.original_sell * a.qty), 0)
FROM
  quote_worksheet a
  INNER JOIN quote_section b ON a.section_id = b.id
WHERE a.quote_id = 131487
  AND a.revision = 1
  AND a.is_checked
  AND b.is_checked", new object[] { tempquote2.QuoteID, tempquote2.Revision });
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                                return;
                            }
                            body += worksheettandm.ToString("C2") + "\n";
                            body += "Customer Phone: ";
                            var customerphone = "";
                            try
                            {
                                customerphone = Toolbox.doSQL_string(conn, @"SELECT CONCAT('(',Address_PhoneArea,') ',Address_PhoneFirst,'-',Address_PhoneLast,' EXT:',Address_PhoneExt) FROM address  WHERE Address_ID =@v0", new object[] { ex_ddl(ddlAddress) });
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                                return;
                            }
                            body += customerphone + "\n";
                            // body += "Customer On Hold: ";
                            //if (tempcustbvid.Hold == "T")
                            //{
                            //    body += "Yes\n";
                            //}
                            //else
                            //{
                            //    body += "No\n";
                            //}
                            body += " Customer Contact: ";
                            var contactname = "";
                            try
                            {
                                contactname = Toolbox.doSQL_string(conn, @"SELECT Contact_Name FROM contact  WHERE Contact_ID =@v0", new object[] { tempquote2.contact_id });
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                                return;
                            }
                            body += contactname + "\n";
                            body += " PM:" + pmmember.FullName + "\n";
                            body += " Quote Due Date:" + tempquote2.completion_date + "\n";
                            body += " EndDate:" + dteExpEndDate.Text + "\n";
                            mail.Body = body;
                            mail.Send();
                        }
                        #endregion Send Email
                    }
                    else if (ddl_quote_value > 0)
                    {
                        tempquote2 = new quote(ddl_quote_value);
                        quotedamount = Convert.ToDouble(tempquote2.Price);
                    }
                }
            }
            catch (Exception ex5)
            {
                log_error("Error Receiving new Quote for: " + woObj.OrderNumber + " from " + wo_company.DSN + ": ", ex5);
                error_flag = true;
                checkpoints.Add("Couldn't receive quote");
                lblError.Text = "Failure to Receive Quote " + ex5.Message;
            }
            #endregion Receive Quote
            if (error_flag)
            {
                if (quote_was_received)
                {
                    quote_unattach(conn, woObj.QuoteID, "0");
                }
                return;
            }
            woObj.business_unit_id = Convert.ToInt32(ex_ddl(ddlCompany));
            woObj.revenue_line_id = Convert.ToInt32(ddlRevenueLines.Value);
            woObj.Description = txtWODescription.Text;
            if (quotedamount == 0 && ddl_quote_value > 0)
            {
                var temp_quote = new quote(ddl_quote_value);
                double temp_price = temp_quote.Price;
                if (temp_price == 0)
                {
                    error_flag = true;
                    checkpoints.Add("Quoted price was zero");
                    lblError.Text = "The selected quote doesn't have a quoted price set. Cannot proceed until the price is supplied on the quote.";
                    return;
                }
                else
                {
                    quotedamount = temp_price;
                }
            }


            woObj.QuoteID = tempquote2.QuoteID == 0 || (quote_was_received && tempquote2.IsTM) ? "0" : ddl_quote_value.ToString();
            woObj.intProjectManager = Convert.ToUInt16(ex_ddl(ddlPM));
            woObj.woprog_sp_memberid = Convert.ToInt16(ex_ddl(ddlPM));
            if (woObj.PONumber != txtCustPO.Text)
            {
                woObj.woprog_custpo_member_id = user.id;
                woObj.woprog_custpo_dt = DateTime.Now;
            }
            if (ddl_parent_workorder.Value != null)
            {
                woObj.parent_woprog_id = Convert.ToInt32(ddl_parent_workorder.Value);
            }
            woObj.PONumber = txtCustPO.Text;
            if (woObj.WOProg_Customer_ID != Convert.ToInt32(ex_ddl(ddlCustomer)))
            {
                // Need to move the project folder....
                NeWOProg.consolidate_project_folders(woObj.woprog_id, woObj.OrderNumber, this_customer.Customer_Name);
            }
            woObj.CustomerName = this_customer.Customer_Name;
            woObj.WOProg_Customer_ID = Convert.ToInt32(ex_ddl(ddlCustomer));
            if (hdnRD.Contains("memCompetency"))
            {
                woObj.WOProg_CoreCompetency = hdnRD["memCompetency"].ToString();
            }
            if (hdnRD.Contains("memUncertainty"))
            {
                woObj.uncertainty = hdnRD["memUncertainty"].ToString();
            }
            if (hdnRD.Contains("memAdvancement"))
            {
                woObj.advancement = hdnRD["memAdvancement"].ToString();
            }
            woObj.woprog_verbal_quote = chk_verbal.Checked;
            woObj.chkRD = chkRD.Checked;
            woObj.chkServiceCall = ChkServiceCall.Checked;
            woObj.IsShell = chkShellWO.Checked;
            woObj.CustomerReferenceNumber = tbCustomerReference.Text;
            woObj.woprog_Contact_ID = Convert.ToInt32(ex_ddl(ddlContact));
            woObj.woprog_Address_ID = Convert.ToInt32(ex_ddl(ddlAddress));
            woObj.woprog_creditcard_payment = cc;
            woObj.woprog_timesheet_percentage = Convert.ToInt32(spn_perc_complete.Value);
            woObj.warranty = Convert.ToInt16(chk_warranty.Checked);
            if (dteStartDate.Text != "")
            {
                woObj.woprog_Expected_StartDate = Convert.ToDateTime(dteStartDate.Text);
            }
            woObj.woprog_Expected_EndDate = Convert.ToDateTime(dteExpEndDate.Text);
            woObj.special_instructions = txtSpecialInstruction.Text;
            woObj.woprog_Location_in_plant = txtAreainPlant.Text;
            if (chkDownPayment.Checked)
            {
                spndayscredit.Value = 0;
            }
            woObj.woprog_dayscredit = Convert.ToInt32(spndayscredit.Value);
            woObj.QuotedPrice = tempquote2.QuoteID == 0 || (quote_was_received && tempquote2.IsTM) ? "0.00" : quotedamount != 0 ? quotedamount.ToString() : "0.00";
            woObj.woprog_hoursspentquoting = Convert.ToDouble(hoursspentonqote);
            woObj.woprog_exp_labor = Toolbox.ReturnZeroIfNull_double(txtlaborvalue.Text);
            woObj.currency_id = Convert.ToInt32(ddl_currency.Value);
            woObj.inspection_link = tb_inspection_link.Text;
            woObj.inspection_required = chk_inspection_yes.Checked;
            woObj.enable_prevailing_wages = chkEnablePrevailingWages.Checked;
            woObj.labor_only = chk_labor_only.Checked;
            woObj.mat_only = chk_mat_only.Checked;
            woObj.invoice_fb_issues = chkInvoiceFeedbackIssues.Checked;
            woObj.sub_only = chk_sub_only.Checked;
            woObj.sustainability_project = chk_sustain.Checked;
            woObj.use_fixed_labour_rate = chk_fixed_labour.Checked;
            woObj.use_fixed_material_markup = chk_fixed_markup.Checked;
            woObj.fixed_labour_rate = Convert.ToDouble(spn_fixed_labour.Value);
            woObj.fixed_material_markup = Convert.ToDouble(spn_material_markup.Value);
            woObj.bdm = Convert.ToInt32(ddl_custram.Value);
            woObj.acting_ram = Convert.ToInt32(ddl_rams.Value);
            woObj.woprog_jobtag_id = Convert.ToInt32(ddl_jobtag.Value);
            woObj.AllowMobilePartManagement = chkMobilePartManage.Checked;
            if (combo_default_terms.Value.ToString() == "0")
            {
                error_flag = true;
                lblError.ClientVisible = true;
                lblError.Text = "Please make sure select a vaild term.";
                return;
            }

            woObj.term_id = Convert.ToInt32(combo_default_terms.Value);

            woObj.default_invoicetype = Toolbox.ReturnZeroIfNull_int(combo_default_invoicetype.Value);
            double sales_value = 0;
            double.TryParse(txtSalesValue.Text, out sales_value);
            woObj.woprog_expected_sales_value = sales_value;

            if (!wo_company.is_er)
            {
                woObj.woprog_ERID = 0;
            }

            try
            {

                if (chkTax1.Visible)
                {
                    woObj.woprog_tax1 = chkTax1.Checked ? (taxinfo.Tax1_Consol) : 0;
                }
                if (chkTax2.Visible)
                {
                    woObj.woprog_tax2 = chkTax2.Checked ? (taxinfo.Tax2_Consol) : 0;
                }
                if (chkTax3.Visible)
                {
                    woObj.woprog_tax3 = chkTax3.Checked ? (taxinfo.Tax3_Consol) : 0;
                }
                if (chkTax4.Visible)
                {
                    woObj.woprog_tax4 = chkTax4.Checked ? (taxinfo.Tax4_Consol) : 0;
                }
            }
            catch (Exception ee)
            {
                Toolbox.do_errorLog_errorStack(ee);
            }
            try
            {
                if (chkOnHold.Checked)
                {
                    // string UpdateWoProgStatus = "UPDATE WOProg SET WOProg_Status='Hold' WHERE WOProg_ID = " + wo.woprog_id;
                    //string UpdateWoProgStatus = "UPDATE WOProg SET WOProg_Hold= 1 WHERE WOProg_ID = " + wo.woprog_id;
                    //Toolbox.doSQL_void(conn,@UpdateWoProgStatus  , null);
                    if (woObj.woprog_hold == 0)
                    {
                        woObj.woprog_onholdmemberid = user.id32;
                    }
                    woObj.woprog_whyhold = txtwhyhold.Text;
                    woObj.woprog_hold = 1;
                }
                else
                {
                    woObj.woprog_whyhold = "";
                    woObj.woprog_onholdmemberid = 0;
                    woObj.woprog_hold = 0;
                }
            }
            catch (Exception ee)
            {
                Toolbox.do_errorLog_errorStack(ee);
                error_flag = true;
                checkpoints.Add("Couldn't work with onhold value");
            }
            if (!error_flag)
            {
                woObj.SaveWorkOrder();
                woObj = new NeWOProg(woObj.woprog_id);
                hid_ts.Value = woObj.woprog_ts.Ticks.ToString();
            }
            else
            {
                // Rollback quote
                if (quote_was_received)
                {
                    quote_unattach(conn, woObj.QuoteID, "0");
                }

            }

            _wo = woObj;

            if (quote_was_received)
            {
                quote.UpdateWoQuotedHours(woObj.woprog_id);
            }





            fill_page_info("General");
            lblError.Text = "Work order saved";
            update_header_totals();
        }
    }
    private object ex_ddl(ASPxComboBox _ddl)
    {
        try
        {
            if (_ddl.SelectedItem == null && _ddl.Value != null)
            {
                return _ddl.Value;
            }
            else if (_ddl.SelectedItem != null && _ddl.SelectedItem.Value != null)
            {
                return _ddl.SelectedItem.Value;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            lblError.Text = "The value for " + _ddl.ID + " is not defined.";
            _ddl.Focus();
            return null;
        }
    }

    private void validate_wo()
    {
        var ttt = 0;
        var dsn = "";
        var psqldsn = new NeBusinessUnit();
        var business_unit_id = ex_ddl(ddlCompany).ToString();
        var woBusinessUnit = new NeBusinessUnit(business_unit_id);
        //    var woTaxEntity = new NeTaxEntity(woBusinessUnit.tax_entity_id);
        dsn = psqldsn.GetBUDSN(Convert.ToInt32(business_unit_id));
        var progressbill = new NeWOProg();
        lblError.ClientVisible = true;
        lblError.Text = "";
        var wo_type = (int)radiotype.Value;

        try
        {
            if (chkProgress.Checked && hidWOProgID.Value == "0")
            {
                progressbill.Load(Convert.ToInt32(ddlJobCostWO.Value.ToString()));
            }
            if (wo_type != 0)
            {
                if (ddl_creditwolink.Value == null)
                {
                    lblError.Text = "You've must select a work order to credit/rebill if you have selected to credit or rebill.";
                    return;
                }
                else
                {
                    if (wo_type == 2)
                    {
                        var x = 0;
                        x = Toolbox.doSQL_int(@"Select Count(woprog_id) from woprog  where woprog_wocredit =@v0", new object[] { ddl_creditwolink.Value });
                        if (x == 0)
                        {
                            lblError.Text = "The work order you've selected to rebill has never been credited. Please credit it first.";
                            return;
                        }
                    }
                }
            }
            else
            {
                ddl_creditwolink.Value = null;

            }
            try
            {
                if (chk_verbal.Checked)
                {
                    double temp_exp_sales = 0;
                    double.TryParse(txtSalesValue.Text, out temp_exp_sales);
                    if (temp_exp_sales == 0)
                    {
                        lblError.Text = "In order to use the verbal quote checkbox, you must include an expected sales value.";
                        return;
                    }
                }
            }
            catch (Exception ee)
            {
                Toolbox.do_errorLog_errorStack(ee);
                return;
            }
            // validate quote is not already taken 
            try { ttt = Convert.ToInt32(ex_ddl(ddlquote)); }
            catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
            if (ttt > 100000)
            {
                var seed = ex_ddl(ddlquote).ToString().Substring(0, 6);
                var count = Toolbox.doSQL_string(@"SELECT ifnull(MAX(woprog_BVWO),0) FROM WOPROG  WHERE woprog_quoteid LIKE CONCAT(@v0,'%')", new object[] { seed });
                if (count != "0" && count != _wo.OrderNumber)
                {
                    lblError.Text = "This Quote or a version of this quote has already been assigned to work order: " + count;
                    return;
                }
            }
            try
            {

                if (ddlAddress.SelectedIndex == 0 || ddlAddress.Text.Trim() == "Not Applicable")
                {

                    lblError.Text = "Please select a valid address";
                    Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                    ddlAddress.Focus();
                    return;
                }
                var WO_AddressId = (int)ddlAddress.Value;

                if (ddlCustomer.Value == null || ddlCustomer.Text == "Select Customer" || ddlCustomer.Value != null && ddlCustomer.Value.ToString() == "0")
                {
                    lblError.Text = "Please select a customer";
                    Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                    ddlCustomer.Focus();
                    return;
                }
                else
                {
                    var woCustomer = new NECustomer(Convert.ToInt32(ddlCustomer.Value));
                    var woAddress = new NEAddress(Convert.ToInt32(ex_ddl(ddlAddress)));
                    var internalCustomerBusinessUnit = woCustomer.IsNEcompany ? new NeBusinessUnit(woCustomer.NEbusiness_unit_id) : new NeBusinessUnit();
                    //           var internalCustomerTaxEntity = new NeTaxEntity(internalCustomerBusinessUnit.tax_entity_id);
                    if (woCustomer.IsNEcompany && internalCustomerBusinessUnit.id != woBusinessUnit.id)
                    {
                        td_parent_info.Style["display"] = user.isContact ? "none" : "";
                    }
                    if (woCustomer.IsNEcompany)
                    {
                        populate_parent_workorder(woCustomer.NEbusiness_unit_id);
                    }
                    if (woCustomer.Hold == "T")
                    {
                        lblError.Text = "You cannot choose this customer because they are on hold";
                        Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                        ddlCustomer.Focus();
                        return;
                    }

                    if (woAddress.csp.po_required || woCustomer.Address.csp.po_required)
                    {
                        if (txtCustPO.Text == "" && !user.isContact)
                        {
                            lblError.Text = "This customer must have a PO before you can cut or save this work order.";
                            Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                            txtCustPO.Focus();
                            return;
                        }
                        else if (txtCustPO.Text == "" && user.isContact)
                        {
                            lblError.Text = "A PO is required before you can cut or save this work order.";
                            Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                            txtCustPO.Focus();
                            return;
                        }
                    }
                    if (woCustomer.IsNEcompany && ddl_parent_workorder.Value != null && (int)ddl_parent_workorder.Value == 0 && hidWOProgID.Value == "0" && (!chkProgress.Checked))
                    {
                        lblError.Text = "Since you have chosen an Internal Customer, you must supply the parent work order.";
                        Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                        ddl_parent_workorder.Focus();
                        return;
                    }
                    //this
                    else if (woCustomer.IsNEcompany && ddl_parent_workorder.Value != null && (int)ddl_parent_workorder.Value == 1 && !user.is_backoffice && user.MemberTypeID != OpsMemberTypes.BranchManager && hidWOProgID.Value == "0")
                    {
                        lblError.Text = "Not sure how you did it, but you are not allowed to cut an intercompany work order.";
                        Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                        ddl_parent_workorder.Focus();
                        return;
                    }
                    else if (hidWOProgID.Value == "0" && check_for_duplicates())
                    {
                        lblError.Text = "Submitting this would cause an exact duplicate work order.. please do not resubmit, and if you are running into an error, please cut a ticket.";
                        Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                        return;
                    }
                }
            }
            catch (Exception Exce)
            {
                Toolbox.do_errorLog_errorStack(Exce);
                lblError.Text = "Error trying to save work order, please check all required fields.";
                ddlCustomer.Focus();
                return;
            }

            try
            {
                if (!user.isContact && (ex_ddl(ddlPM).ToString() == "0" || ddlPM.Text == "Select Project Manager"))
                {
                    lblError.Text = "Please select a project manager";
                    ddlPM.Focus();
                    return;
                }
            }
            catch (Exception Exce)
            {
                Toolbox.do_errorLog_errorStack(Exce);
                lblError.Text = "Please select a project manager";
                ddlPM.Focus();
                return;
            }

            // Add validation to work order page to verify that a TERM is selected BEFORE sending back the scan popup
            try
            {

                if (ex_ddl(combo_default_terms).ToString() == "0")
                {
                    lblError.Text = "Please select a Payment Terms";
                    Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                    combo_default_terms.Focus();
                    return;
                }
            }
            catch (Exception Exce)
            {
                Toolbox.do_errorLog_errorStack(Exce);
                lblError.Text = "Please select a Payment Terms";
                combo_default_terms.Focus();
                return;
            }



            if (chkProgress.Checked && hidWOProgID.Value == "0")
            {

                if (ddlJobCostWO.Value.ToString() == "0")
                {
                    lblError.Text = "Invalid associated (job cost) work order selected";
                    Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                    ddlJobCostWO.Focus();
                    return;
                }
                if (txtpb_amt.Text == "0")
                {
                    lblError.Text = "You must provide a valid percentage or flat amount value for this progress billing";
                    Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                    txtpb_amt.Focus();
                    return;
                }
                try
                {
                    var associated_woid = ex_ddl(ddlJobCostWO).ToString();
                    var thevalueofquote = Convert.ToDouble(progressbill.QuotedPrice);
                    var todatepb = Toolbox.doSQL_double(@"SELECT IFNull(SUM(WOProg_InvoicedNetTotal),0) FROM woprog  WHERE woprog_associate_woprog_id =@v0", new object[] { associated_woid });

                    if (cbopb_type.Text == "Flat Amount")
                    {
                        _quote_flatamount = Convert.ToDouble(Regex.Replace(txtpb_amt.Text, "[^.0-9]", ""));

                        if (_quote_percent < 0)
                        {
                            lblError.Text = "Value for flat progress bill amount must be above $0";
                            Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                            txtpb_amt.Focus();
                            return;
                        }
                        _totaltobepb = Math.Round(todatepb + _quote_flatamount, 2);
                    }
                    else
                    {
                        _quote_percent = Convert.ToDouble(Regex.Replace(txtpb_amt.Text, "[^.0-9]", "")) / 100;
                        if (_quote_percent > 1)
                        {
                            _quote_percent = _quote_percent * .01;
                        }
                        if (_quote_percent < 0 || _quote_percent > 1)
                        {
                            lblError.Text = "Value for percent of quote outside of acceptable range (0-100%)";
                            Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                            txtpb_amt.Focus();
                            return;
                        }
                        _quote_flatamount = thevalueofquote * _quote_percent;
                        _totaltobepb = Math.Round(todatepb + thevalueofquote * _quote_percent, 2);
                    }
                    if (thevalueofquote < _totaltobepb - 0.1 && chkProgressBillCredit.Checked == false)
                    {
                        lblError.Text = "This progress billing will result in you billing more than the value of the quote!";
                        Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                        return;
                    }
                }
                catch (Exception Exce)
                {
                    Toolbox.do_errorLog_errorStack(Exce);
                    lblError.Text = "Please enter a proper value for percent or flat billing amount of quote";
                    txtpb_amt.Focus();
                    return;
                }


                //Get Total of Associated Billings



            }
            if (chkOnHold.Checked && txtwhyhold.Text.Length < 3)
            {
                lblError.Text = "Please state why the work order is to be put on hold";
                txtwhyhold.Focus();
                return;
            }
            if (ddlContact.Value != null)
            {
                if (ddlContact.Value.ToString() == "0")
                {
                    lblError.Text = "Please Select a Contact";
                    populate_ddlContact(Convert.ToInt32(ex_ddl(ddlCustomer)), 0);
                    ddlContact.Focus();
                    return;
                }
            }
            else
            {
                lblError.Text = "Please Select a Contact";
                ddlContact.Focus();
                populate_ddlContact(Convert.ToInt32(ex_ddl(ddlCustomer)), 0);
                return;
            }
            if (hidWOProgID.Value == "0")
            {
                if (wo_type == 0 && !chkProgress.Checked && (ddl_custram.Value == null ||
                    ddl_custram.Text == "Please Select a BDM" || (ddl_custram.Value != null && ddl_custram.Value.ToString() == "0")))
                {
                    lblError.Text = "Please select a BDM";
                    Toolbox.do_errorLog_errorStack(new Exception(lblError.Text));
                    ddl_custram.Focus();
                    return;
                }
            }
                if (chkRD.Checked && hidWOProgID.Value != "0")
                {
                    if (mem_Advancement.Text == "")
                    {
                        lblError.Text = "R & D advancement Missing, go to R&D tab.";
                        mem_Advancement.Focus();
                        return;
                    }
                    if (mem_Uncertainty.Text == "")
                    {
                        lblError.Text = "R & D Technical Uncertainty Missing, go to R&D tab.";
                        mem_Uncertainty.Focus();
                        return;
                    }
                    if (mem_Competancy.Text == "")
                    {
                        lblError.Text = "R & D Core Competency Missing, go to R&D tab.";
                        mem_Competancy.Focus();
                        return;
                    }
                }
                if (txtWODescription.Text == "")
                {
                    lblError.Text = "Work order description is missing";
                    txtWODescription.Focus();
                    return;
                }
                else if (txtWODescription.Text.Length > 999)
                {
                    lblError.Text = "Work order description can't be longer than 999 characters.";
                    txtWODescription.Focus();
                    return;
                }
                if (txtSpecialInstruction.Text.Length > 999)
                {
                    lblError.Text = "Special Instructions can't be longer than 999 characters.";
                    txtSpecialInstruction.Focus();
                    return;
                }
                if (txtAreainPlant.Text.Length > 200)
                {
                    lblError.Text = "Location in plant text can't be longer than 200 characters.";
                    txtAreainPlant.Focus();
                    return;
                }



                if (chkProgress.Checked == false && !user.isContact && wo_type == 0 && (ddlquote.Value == null || wo_type == 0))
                {
                    if (txtSalesValue.Text == "" || txtSalesValue.Text == "0")
                    {
                        lblError.Text = "You must include an expected sales value.  This will help us predict a sales pipeline";
                        return;
                    }


                }
                else if (user.isContact || (ddlquote.Value != null && ddlquote.Value.ToString() == "0"))
                {
                    txtSalesValue.Text = "0";
                }

                if (ddlquote.Value != null && ddlAddress.Value != null)
                {
                    if (Convert.ToInt32(ddlquote.Value) > 0 && Convert.ToInt32(ddlAddress.Value) > 0)
                    {
                        quote att_quote = new quote(Convert.ToInt32(ddlquote.Value));

                        var address_id = Convert.ToInt32(ddlAddress.Value);
                        if (ddlAddress.Items.Count == 0)
                        {
                            ddlAddress.DataBind();
                            ddlAddress.Value = address_id;
                        }

                        if (Convert.ToInt32(ddlAddress.Value) != att_quote.address_id)
                        {
                            if (ddlAddress.Items.IndexOfValue(att_quote.address_id) == -1)
                            {
                                lblError.Text = "The selected quote has an address linked to it, which does not belong to the customer it is linked to.";
                                ddlquote.Focus();
                                return;
                            }
                        }
                    }
                }

                if (txtSalesValue.Text != "0")
                {
                    try
                    {
                        Convert.ToDouble(txtSalesValue.Text);
                    }
                    catch (Exception Exce)
                    {
                        Toolbox.do_errorLog_errorStack(Exce);
                        lblError.Text = "Please enter a proper numeric value in the expected sales value field";
                        txtSalesValue.Focus();
                        return;
                    }
                }

                double temp_exp_sales1 = 0;
                double.TryParse(txtSalesValue.Text, out temp_exp_sales1);

                if (dteExpEndDate.Text == "")
                {
                    lblError.Text = "Expected end date not supplied.";
                    dteExpEndDate.Focus();
                    return;
                }
                if ((txtlaborvalue.Text == "" || txtlaborvalue.Text == "0") && !user.isContact && !chk_mat_only.Checked)
                {
                    if (!chkProgress.Checked && ddlCompany.Value.ToString() != "11" && (_wo.Status == "" || _wo.OrderDate > Convert.ToDateTime("2012-09-09")))
                    {

                        lblError.Text = "Please indicate the expected amount of hours required for this work.";
                        txtlaborvalue.Focus();
                        return;
                    }
                }


                try { var x = Convert.ToInt32(spndayscredit.Value); }
                catch
                {
                    lblError.Text = "Illegal Days Credit Value.";
                    spndayscredit.Focus();
                    return;
                }

                if (dteStartDate.Text == "")
                {
                    lblError.Text = "You must set a valid Start Date";
                    dteStartDate.Focus();
                    return;
                }

                if (ddlRevenueLines.Value == null || Convert.ToInt32(ddlRevenueLines.Value) == 0)
                {
                    lblError.Text = "Please select revenue line";
                    ddlRevenueLines.Focus();
                    return;
                }
            }
        catch (Exception ex)
        {
            lblError.Text += ex.Message;
            Toolbox.do_errorLog_errorStack(ex);
            return;
        }
        lblError.ClientVisible = false;
    }



    protected bool Detachingquote(NeWOProg WO, NeBusinessUnit wo_company, string pre_save_quoteid, int ddl_quote_value)
    {
        var quote_was_unreceived = false;
        try
        {
            using (var conn = Toolbox.connect())
            {
                if (pre_save_quoteid != "0" && pre_save_quoteid != ddl_quote_value.ToString()) // if the quote has changed.. we need to do some work.
                {
                    quote_unattach(conn, pre_save_quoteid, ddl_quote_value.ToString(), WO);
                    var tempquote1 = new quote(Convert.ToInt32(pre_save_quoteid));
                    var quote_history_information = "UnReceived from WO:" + order_number + " - (" + pre_save_quoteid + ") was replaced by (" + ddl_quote_value + ")";
                    try
                    {
                        Toolbox.doSQL_void(@"INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event)  VALUES(now(),@v0,@v1,@v2,@v3)", new object[] { user.id, tempquote1.QuoteID, tempquote1.Revision, quote_history_information });
                    }
                    catch (Exception ex)
                    {
                        Toolbox.do_errorLog_errorStack(ex);
                        // throw new Exception(ex.ToString());
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);

                    }
                    //   Toolbox.doSQL_void(@"delete from woprog_tasks  where woprog_id =@v0", new object[] { WO.woprog_id });
                    quote_was_unreceived = true;
                }
            }
        }
        catch (Exception ex4)
        {
            log_error("Error UnReceiving Quote for: " + WO.OrderNumber + " from " + wo_company.DSN + ": ", ex4);
        }

        return quote_was_unreceived;

    }

    protected bool Attachequote(NeWOProg WO, NeBusinessUnit wo_company, string pre_save_quoteid, int ddl_quote_value, double this_qty_billed)
    {
        #region Receive Quote
        var quote_was_received = false;
        try
        {
            using (var conn = Toolbox.connect())
            {

                // Get the current count for 2139 lines
                var n_lines_q = Toolbox.doSQL_int(conn, @"SELECT COUNT(wo_detail_current_id) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = 'Q' AND (wo_detail_current_billtypeid = 3 or wo_detail_current_billtypeid = 11) AND wo_detail_current_master_id = 2139", new object[] { WO.woprog_id });
                quote tempquote2;
                var quotedamount = 0.0;

                if (n_lines_q == 0)
                {
                    if (ddl_quote_value > 0 && pre_save_quoteid != ddl_quote_value.ToString())
                    {
                        quote_was_received = true;
                        var quotesuccess = new quote().ReceiveNeQuotes(ddl_quote_value, WO.woprog_id, txtCustPO.Text, user.id, Convert.ToInt32(ex_ddl(ddlContact)), wo_company.DSN);
                        tempquote2 = new quote(ddl_quote_value);
                        quotedamount = Convert.ToDouble(tempquote2.Price);

                        if (quotesuccess)
                        {
                            #region Add 2139 - (Price As Quoted) Line
                            Toolbox.doSQL_void(conn, @" UPDATE wo_detail_current SET wo_detail_current_billtypeid = 1 WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_billtypeid = 0", new object[] { WO.woprog_id });
                            var quote_line = new NeWODetailCurrent();
                            var linecounts = quote_line.GetLineCount(WO.woprog_id);
                            quote_line.tax1 = wo_company.TaxQuotedJobs == 1 ? WO.woprog_tax1 : 0;
                            quote_line.tax2 = wo_company.TaxQuotedJobs == 1 ? WO.woprog_tax2 : 0;
                            quote_line.tax3 = wo_company.TaxQuotedJobs == 1 ? WO.woprog_tax3 : 0;
                            quote_line.tax4 = wo_company.TaxQuotedJobs == 1 ? WO.woprog_tax4 : 0;
                            quote_line.description = "Price as Quoted Quote: " + ddl_quote_value;
                            quote_line.master_id = 2139;
                            quote_line.code = "QUOTE";
                            quote_line.business_unit_id = WO.business_unit_id;
                            quote_line.qty_committed = this_qty_billed;
                            quote_line.qty_invoiced = this_qty_billed;
                            quote_line.type = "Q";
                            quote_line.cost = 0;// quotedamount - (quotedamount * tempquote2.margin);
                            quote_line.sell = quotedamount;
                            quote_line.unit = quotedamount;
                            quote_line.woprog_id = WO.woprog_id;
                            quote_line.bvwo = Convert.ToInt32(WO.OrderNumber);
                            quote_line.origin = "Automatically Added";
                            quote_line.billtypeid = 11;
                            quote_line.rec_no = linecounts + 2;
                            quote_line.added_by = user.id;
                            quote_line.save(user, "/sections/workorder/index.aspx.cs - create_workorder #1", false);
                            #endregion Add 2139 Line

                            #region if parent child add quote to parent

                            if (WO.parent_woprog_id != 0)
                            {
                                // add quote line to the parent WO AS A 9595
                                var quote_lineForParent = new NeWODetailCurrent();
                                var parentWO = new NeWOProg(WO.parent_woprog_id);
                                //linecounts = quote_lineForParent.GetLineCount(workorder1.parent_woprog_id);
                                quote_lineForParent.tax1 = wo_company.TaxQuotedJobs == 1 ? parentWO.woprog_tax1 : 0;
                                quote_lineForParent.tax2 = wo_company.TaxQuotedJobs == 1 ? parentWO.woprog_tax2 : 0;
                                quote_lineForParent.tax3 = wo_company.TaxQuotedJobs == 1 ? parentWO.woprog_tax3 : 0;
                                quote_lineForParent.tax4 = wo_company.TaxQuotedJobs == 1 ? parentWO.woprog_tax4 : 0;
                                quote_lineForParent.description = "Price as Quoted Quote: " + ddl_quote_value;
                                quote_lineForParent.master_id = OpsSpecialPart.NewChildWO;
                                quote_lineForParent.code = ddl_quote_value.ToString();
                                quote_lineForParent.business_unit_id = parentWO.business_unit_id;
                                quote_lineForParent.qty_committed = this_qty_billed;
                                quote_lineForParent.qty_invoiced = this_qty_billed;
                                quote_lineForParent.type = "M";
                                quote_lineForParent.notes = "Automatically Added from child";
                                quote_lineForParent.cost = quotedamount;
                                var quoteSell = Toolbox.doSQL_double("SELECT GetSellPriceFromWo(@v0,0,1,@v1,@v2)", new object[] { quotedamount, parentWO.business_unit_id, parentWO.woprog_id });
                                quote_lineForParent.sell = quoteSell;
                                quote_lineForParent.unit = quoteSell;
                                quote_lineForParent.woprog_id = parentWO.woprog_id;
                                quote_lineForParent.bvwo = Convert.ToInt32(parentWO.OrderNumber);
                                quote_lineForParent.origin = "Automatically Added from child";
                                quote_lineForParent.billtypeid = 0; //Should be 1 if parent is quoted job
                                                                    //quote_lineForParent.rec_no = linecounts + 2;
                                quote_lineForParent.added_by = user.id;
                                //  quote_lineForParent.save(_current_user, "/sections/workorder/index.aspx.cs - create_workorder #1", false);
                                //  NeWODetailCurrent.reorder_lines(parentWO.woprog_id);


                                //Remove labor lines from child on parent WO
                                //pay type_id and member id

                                var allLaborLinesOnChild = Toolbox.doSQL_dt(
                                    @"SELECT a.wo_detail_current_id,a.wo_detail_current_qty_committed,a.wo_detail_current_qty_ordered,a.wo_detail_current_qty_invoiced, a.memberid, b.paytype_id 
FROM wo_detail_current a
JOIN membertype_chargeout b ON a.wo_detail_current_master_id = b.id
WHERE a.wo_detail_current_woprog_id = @v0 
AND a.wo_detail_current_type = 'L'", new object[] { WO.woprog_id });

                                foreach (DataRow woDetailObj in allLaborLinesOnChild.Rows)
                                {

                                    //var woDetail = new NeWODetailCurrent( Convert.ToInt32(woDetailObj["wo_detail_current_id"]));

                                    Toolbox.doSQL_void(@"UPDATE wo_detail_current a  
JOIN membertype_chargeout b ON a.wo_detail_current_master_id = b.id
SET a.wo_detail_current_qty_committed = (a.wo_detail_current_qty_committed - @v1),
a.wo_detail_current_qty_ordered = (a.wo_detail_current_qty_ordered - @v2),
a.wo_detail_current_qty_invoiced = (a.wo_detail_current_qty_invoiced - @v3)
WHERE a.wo_detail_current_woprog_id = @v0 
AND a.wo_detail_current_type = 'L' AND a.memberid = @v4 AND b.paytype_id = @v5", new object[]
                                    {
                                        WO.parent_woprog_id, woDetailObj["wo_detail_current_qty_committed"],
                                        woDetailObj["wo_detail_current_qty_ordered"], woDetailObj["wo_detail_current_qty_invoiced"],
                                        woDetailObj["memberid"], woDetailObj["paytype_id"]
                                    });
                                }

                                WO.fast_update_header_totals();

                                new NeWOProg(WO.parent_woprog_id).fast_update_header_totals();
                            }

                            #endregion

                        }
                        #region build task list
                        var quote_details = Toolbox.doSQL_dt(@"Select linetext,line_number from quote_extratext  where quote_id =@v0 and revision =@v1  and type =1 order by line_number", new object[] { tempquote2.QuoteID, tempquote2.Revision });
                        foreach (DataRow qd in quote_details.Rows)
                        {
                            Toolbox.doSQL_void(@"Insert into woprog_tasks (woprog_id,task,complete,_order)  Values(@v0,@v1,0,@v2)", new object[] { _wo.woprog_id, qd["linetext"], qd["line_number"] });
                        }
                        #endregion

                        #region Update Quote History
                        var pmmember = new NeMember(Convert.ToInt32(ex_ddl(ddlPM)));
                        var quote_history_information = string.Format(@"Received - WO:{0}  PM:{1} Start Date:{2} End Date:{3}", order_number, pmmember.FullName, dteStartDate.Text, dteExpEndDate.Text);
                        var sql_quote_history = string.Format(@"
INSERT INTO quote_history 
	(
	create_datetime, 
	created_by, 
	quote_id, 
	revision, 
	event
	) 
VALUES
	(
	NOW(),
	{0},
	{1},
	{2},
	""{3}""
	)",
                        user.id,
                        tempquote2.QuoteID,
                        tempquote2.Revision,
                        quote_history_information);
                        try
                        {
                            Toolbox.doSQL_void(@" INSERT INTO quote_history ( create_datetime, created_by, quote_id, revision, event ) VALUES ( NOW(), @v0 , @v1 , @v2 , @v3  )", new object[] { user.id, tempquote2.QuoteID, tempquote2.Revision, quote_history_information });
                        }
                        catch (Exception ex)
                        {
                            //error_flag = true;
                            //checkpoints.Add("Couldn't add quote_history line item");
                            //throw new Exception(ex.ToString());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);

                        }
                        #endregion Update Quote History
                        #region Send Email
                        if (ex_ddl(ddlCompany).ToString() == "1")
                        {
                            var mail = new NeEMail();
                            if (Toolbox.app_setting("debug_redirect") == "1")
                                mail.To = Toolbox.app_setting("debug_redirect_email");
                            else
                                mail.To = EmailID.OakQuoteUpdate + Toolbox.app_setting("DomainForEmail");

                            mail.Subject = "A Quote Has Been Added to a Work Order";
                            mail.From = "administrator@" + Toolbox.app_setting("DomainForEmail");

                            var body = "Cut - WO:" + order_number + "\n";
                            try
                            {
                                body += "Customer Name: " + Toolbox.doSQL_string(conn, @"SELECT customer_name FROM customer  where Customer_ID =@v0", new object[] { ex_ddl(ddlCustomer) }) + "\n";
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                            }
                            body += " Description: " + tempquote2.txtJobDescription + "\n";
                            body += " StartDate:" + dteStartDate.Text + "\n";
                            body += "\n";
                            body += "QUOTE ID: " + tempquote2.QuoteID + "\n";
                            body += "Revision: " + tempquote2.Revision + "\n";
                            body += "Value of Quote: " + tempquote2.Price + "\n";
                            body += "Price Type: " + tempquote2.price_type + "\n";
                            body += "Work Sheet Total T+M: ";
                            double worksheettandm = 0;
                            try
                            {
                                worksheettandm = Toolbox.doSQL_double(conn, @"SELECT
  IFNULL(SUM(a.original_sell * a.qty), 0)
FROM
  quote_worksheet a
  INNER JOIN quote_section b ON a.section_id = b.id
WHERE a.quote_id = 131487
  AND a.revision = 1
  AND a.is_checked
  AND b.is_checked", new object[] { tempquote2.QuoteID, tempquote2.Revision });
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);

                            }
                            body += worksheettandm.ToString("C2") + "\n";
                            body += "Customer Phone: ";
                            var customerphone = "";
                            try
                            {
                                customerphone = Toolbox.doSQL_string(conn, @"SELECT CONCAT('(',Address_PhoneArea,') ',Address_PhoneFirst,'-',Address_PhoneLast,' EXT:',Address_PhoneExt) FROM address  WHERE Address_ID =@v0", new object[] { ex_ddl(ddlAddress) });
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);

                            }
                            body += customerphone + "\n";

                            body += " Customer Contact: ";
                            var contactname = "";
                            try
                            {
                                contactname = Toolbox.doSQL_string(conn, @"SELECT Contact_Name FROM contact  WHERE Contact_ID =@v0", new object[] { tempquote2.contact_id });
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                            }
                            body += contactname + "\n";
                            body += " PM:" + pmmember.FullName + "\n";
                            body += " Quote Due Date:" + tempquote2.completion_date + "\n";
                            body += " EndDate:" + dteExpEndDate.Text + "\n";
                            mail.Body = body;
                            mail.Send();
                        }
                        #endregion Send Email

                        quote_was_received = true;
                    }
                    else if (ddl_quote_value > 0)
                    {
                        tempquote2 = new quote(ddl_quote_value);
                        quotedamount = Convert.ToDouble(tempquote2.Price);
                    }
                }
            }
        }
        catch (Exception ex5)
        {
            log_error("Error Receiving new Quote for: " + WO.OrderNumber + " from " + wo_company.DSN + ": ", ex5);
            //error_flag = true;
            //checkpoints.Add("Couldn't receive quote");
            lblError.Text = "Failure to Receive Quote " + ex5.Message;
        }

        return quote_was_received;
        #endregion Receive Quote
    }


    /// <summary>
    /// This is a last ditch safety from being able to create duplicate work orders
    /// </summary>
    private bool check_for_duplicates()
    {

        var c = Toolbox.doSQL_int(@" SELECT COUNT(*) FROM woprog WHERE woprog_customer_id = @v0  AND woprog_address_id = @v1  AND business_unit_id = @v2  AND woprog_description = @v3  AND woprog_expected_sales_value = @v4  AND woprog_pm_memberid = @v5  AND woprog_custpo = @v6  AND woprog_expected_enddate = @v7  AND woprog_status != 'Deleted' ", new object[] { ex_ddl(ddlCustomer).ToString().Trim(), ex_ddl(ddlAddress), ddlCompany.Value, txtWODescription.Text, txtSalesValue.Text.Replace(",", "").Replace("$", ""), ex_ddl(ddlPM), txtCustPO.Text, dteExpEndDate.Date.ToString("yyyy-MM-dd") });
        return c > 0;
    }
    protected void CreateWorkOrder(bool _redirect)
    {
        using (var conn = Toolbox.connect())
        {
            var jobcost_wo = new NeWOProg();

            var qTM = new quote(Convert.ToInt32(ddlquote.Value));
            var is_tm = Toolbox.doSQL_int(conn, @"Select ifnull(MAX(is_tm),0) from quote_master where quote_id = @v0 AND active_revision = 1", new object[] { qTM.QuoteID });

            var print_doc = new PrintDocument();
            var str_expected_sales_value = "";
            var quotedamount = 0.0;
            var currency = Convert.ToInt32(ddl_currency.Value);
            var hoursspentonqote = 0.0;
            var error_flag = false;
            var is_downpayment = chkDownPayment.Checked;
            var rd = chkRD.Checked ? 1 : 0;
            var cc = chk_cc.Checked ? 1 : 0;
            var wo_type = (int)radiotype.Value;
            var business_unit_id = ex_ddl(ddlCompany).ToString();
            var businessUnit = new NeBusinessUnit(business_unit_id);

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var dsn = businessUnit.DSN;

            Session["DSNName"] = dsn;
            if (chkProgress.Checked)
            {
                jobcost_wo.Load(Convert.ToInt32(ddlJobCostWO.Value.ToString()));  // Loads up the original work order.

            }
            str_expected_sales_value = txtSalesValue.Text.Replace(",", "").Replace("$", "");
            double sales_value = 0;
            double.TryParse(str_expected_sales_value, out sales_value);
            var wo_count = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM woprog  WHERE woprog_customer_id =@v0 AND woprog_address_id =@v1 ", new object[] { ex_ddl(ddlCustomer), ex_ddl(ddlAddress) });

            // check if they have a vaild address, if not, copy it over from the intranet db in thier BV

            #region check address and add if its missing
            var thisCustomer = new NECustomer(Convert.ToInt32(ex_ddl(ddlCustomer)));

            var parentWo = 0;
            if (ddl_parent_workorder.Value != null)
            {
                var str = ddl_parent_workorder.Value.ToString();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    int.TryParse(str, out parentWo);
                }
            }

            if (thisCustomer.IsNEcompany && parentWo == 0 && !businessUnit.is_backoffice && (!chkProgress.Checked))
            {
                var customerBusinessUnit = new NeBusinessUnit(thisCustomer.NEbusiness_unit_id);
                if (customerBusinessUnit.tax_entity_id == businessUnit.tax_entity_id)
                {
                    lblError.Text = "Please select the parent workorder.";
                    lblError.ClientVisible = true;
                    return;
                }
            }

            if (combo_default_terms.Value.ToString() == "0")
            {

                //error_flag = true;
                lblError.ClientVisible = true;
                lblError.Text = "Please make sure select a vaild term.";
                return;
            }

            #endregion


            if (chkProgress.Checked)  //if it's a progress bill
            {
                #region check progress bill over billing
                // Check if they are billing OVER the quoted amount
                var q_id = 0;
                var q_rev = 0;
                nesi.core.quote.splice(jobcost_wo.QuoteID, out q_id, out q_rev);
                var quoted_amount = nesi.core.quote.quoted_amount(conn, q_id);
                var total_billed = NeWOProg.total_billed(conn, jobcost_wo.woprog_id);

                //_tools.debug_note(string.Format("({0} + ({1} * {2}) > {2}", total_billed, quote_percent, quoted_amount));
                if ((total_billed >= quoted_amount || total_billed + _quote_flatamount > quoted_amount || total_billed + _quote_percent * quoted_amount > quoted_amount) && !chkProgressBillCredit.Checked)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('You are not allowed to bill more than the quoted amount via a progress bill.');", true);
                    return;
                }
                if (chkProgressBillCredit.Checked && wo_type == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('You cannot select both a credit type of work order AND credit progress billing.');", true);
                    return;
                }


                var quote = new quote(Convert.ToInt32(jobcost_wo.QuoteID));
                if (chkDownPayment.Checked)
                {
                    if (cbopb_type.Text == "Flat Amount")
                    {
                        txtWODescription.Text = string.Format("Down Payment of {0:C2} ({1}) as per Quote-{2}-V{3} - payment is due on receipt prior to start of job.  Original WO: {4}", _quote_flatamount, quote.txtJobDescription, q_id, q_rev, jobcost_wo.OrderNumber);
                        txtSalesValue.Text = _quote_flatamount.ToString();
                    }
                    else
                    {
                        txtWODescription.Text = string.Format("Down Payment of {0:P2} ({1}) as per Quote-{2}-V{3} - payment is due on receipt prior to start of job.  Original WO: {4}", _quote_percent, quote.txtJobDescription, q_id, q_rev, jobcost_wo.OrderNumber);
                        txtSalesValue.Text = (_quote_percent * Convert.ToDouble(quote.Price)).ToString();
                    }
                    spndayscredit.Value = 0;
                }
                else
                {
                    if (cbopb_type.Text == "Flat Amount")
                    {
                        txtWODescription.Text = string.Format("Progress Billing of {0:C2} ({1}) as per Quote-{2}-V{3}", _quote_flatamount, quote.txtJobDescription, q_id, q_rev);
                        txtSalesValue.Text = _quote_flatamount.ToString();
                    }
                    else
                    {
                        txtWODescription.Text = string.Format("Progress Billing of {0:P2} ({1}) as per Quote-{2}-V{3}", _quote_percent, quote.txtJobDescription, q_id, q_rev);
                        txtSalesValue.Text = (_quote_percent * Convert.ToDouble(quote.Price)).ToString();
                    }
                }
                double.TryParse(txtSalesValue.Text, out var checkProgressAmount);
                if (Math.Abs(checkProgressAmount) < 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('You cannot progress bill for less than $1.00');", true);
                    return;
                }
                str_expected_sales_value = txtSalesValue.Text;
                #endregion
            }



            #region cut the mysql work order
            var new_woprog_id = 0;
            if (!error_flag)
            {
                if (ddlquote.Value != null && ddlquote.Value.ToString() != "0" && quotedamount == 0)
                {
                    quotedamount = Convert.ToDouble(str_expected_sales_value);
                }
                var tax1 = 0;
                var tax2 = 0;
                var tax3 = 0;
                var tax4 = 0;
                var is_rebill = wo_type == 2;
                var is_credit = wo_type == 1;
                //int parent_business_unit_id			= (int) ddl_parent_workorder.Value == 0 ? 0 : (int) ddl_parent_company.Value;
                var parent_woprog_id = ddl_parent_workorder.Value != null && thisCustomer.IsNEcompany ? (int)ddl_parent_workorder.Value : 0;
                var parent_wo = parent_woprog_id > 0 ? new NeWOProg(parent_woprog_id) : new NeWOProg();

                var woprog_wocredit = 0;
                if (is_rebill || is_credit)
                {
                    woprog_wocredit = Convert.ToInt32(ddl_creditwolink.Value);
                }
                #region Actual insert query is here!
                // 2014-02-01 - Matt redid the query below so it's actually something we can work with, without much effort.
                var my_comm = new MySqlCommand();
                my_comm.Connection = conn;
                // 2014-02-01 - Everything is parameter based now for this insert query.
                #region CommandText
                my_comm.CommandText = @"
INSERT INTO woprog 
	(
    business_unit_id,
    woprog_bvwo,
    woprog_customername,
    woprog_opendatetime,
    woprog_closedatetime,
    woprog_cutdatetime,
    woprog_description,
    woprog_quoteid,
    woprog_pm_memberid,
    woprog_custpo,
    woprog_invoiceno,
    woprog_invoicedate,
    woprog_customer_id,
    woprog_status,
    woprog_interbranch_woprog_id,
    woprog_uncertainty,
    woprog_advancement,
    woprog_rdflag,
    woprog_servicecall,
    woprog_cutby_memberid,
    woprog_department,
    woprog_contact_id,
    woprog_address_id,
    woprog_location_in_plant,
    woprog_specialinstructions,
    woprog_erid,
    woprog_labourtotalsell,
    woprog_materialtotalsell,
    woprog_jobcost_tracking,
    woprog_billed_as_quoted,
    woprog_quotedhourstotal,
    woprog_quotedamount,
    woprog_timesheet_percentage,
    woprog_invoicednettotal,
    woprog_localtax1,
    woprog_localtax2,
    woprog_localtax3,
    woprog_hoursspentquoting,
    woprog_expected_sales_value,
    woprog_laborcost,
    woprog_materialcost,
    woprog_associate_woprog_id,
    woprog_sp_memberid,
    woprog_quotedprice_type,
    woprog_tax1,
    woprog_tax2,
    woprog_tax3,
    woprog_tax4,
    woprog_progressbilled,
    woprog_stilltobebilled,
    woprog_totaltandm,
    woprog_grossmargin,
    `material sell estimated`,
    `labour sell estimated`,
    woprog_customer_consolidation,
    woprog_expected_startdate,
    woprog_expected_enddate,
    woprog_hold,
    woprog_makeid,
    woprog_modelid,
    woprog_erwarranty,
    woprog_erquotedprice,
    woprog_apply_discount,
    woprog_corecompetency,
    woprog_customersnotes,
    woprog_expectedcheckrun,
    woprog_creditcard_payment,
    woprog_collection_notes,
    woprog_last_email_date,
    woprog_last_faxed_date,
    woprog_invoice_paid_date,
    woprog_dayscredit,
    woprog_whyhold,
    woprog_onholdmemberid,
    woprog_vis_to_cust,
    woprog_invoice_tax,
    woprog_invoice_collection_status,
    woprog_iscredit,
    woprog_isrebill,
    woprog_whycredit,
    woprog_wocredit,
    woprog_invoicebalance,
    woprog_invoicecreditamt,
    woprog_servicereportdesc,
    woprog_custpo_member_id,
    woprog_custpo_dt,
    woprog_verbal_quote,
    woprog_exp_labor,
    woprog_assetid,
    woprog_default_invoicetype,
    woprog_auto_invoice,
    woprog_glposting_instructions,
    woprog_isdownpayment,
    schedule_status_id,
    woprog_notification_sent,
    woprog_survey_sent,
    warranty,
    survey_finished,
	parent_woprog_id,
	currency_id,
    default_labour_sell_gl,
    default_material_sell_gl,
    inspection_link ,
    inspection_required ,
    labor_only,
    mat_only,
    sub_only,
    fixed_labour_rate ,
    fixed_material_markup ,
    sustainability_project ,
    use_fixed_labour_rate ,
    use_fixed_material_markup ,
    bdm,
    acting_ram,
    woprog_jobtag_id,
    revenue_line_id，
    term_id,
    is_shell_wo,
    customer_ref,
	allow_mobile_part_management,
    enable_prevailing_wages,
    invoice_fb_issues
	) 
VALUES
    (
	@business_unit_id,
	@bvwo,
	@customername,
	@opendatetime,
	@closedatetime,
	NOW(),
	@description,
	@quoteid,
	@pm_memberid,
	@custpo,
	@invoiceno,
	@invoicedate,
	@customer_id,
	@status,
	@interbranch_woprog_id,
	@uncertainty,
	@advancement,
	@rdflag,
	@servicecall,
	@cutby_memberid,
	@department,
	@contact_id,
	@address_id,
	@location_in_plant,
	@specialinstructions,
	@erid,
	@labourtotalsell,
	@materialtotalsell,
	@jobcost_tracking,
	@billed_as_quoted,
	@WOProg_QuotedHoursTotal,
	@quotedamount,
	@timesheet_percentage,
	@invoicednettotal,
	@localtax1,
	@localtax2,
	@localtax3,
	@hoursspentquoting,
	@expected_sales_value,
	@laborcost,
	@materialcost,
	@associate_woprog_id,
	@sp_memberid,
	@quotedprice_type,
	@tax1,
	@tax2,
	@tax3,
	@tax4,
	@progressbilled,
	@stilltobebilled,
	@totaltandm,
	@grossmargin,
	@materialsellestimated,
	@laboursellestimated,
	@customer_consolidation,
	@expected_startdate,
	@expected_enddate,
	@hold,
	@makeid,
	@modelid,
	@erwarranty,
	@erquotedprice,
	@apply_discount,
	@corecompetency,
	@customersnotes,
	@expectedcheckrun,
	@creditcard_payment,
	@collection_notes,
	@last_email_date,
	@last_faxed_date,
	@invoice_paid_date,
	@dayscredit,
	@whyhold,
	@onholdmemberid,
	@vis_to_cust,
	@invoice_tax,
	@invoice_collection_status,
	@iscredit,
	@isrebill,
	@whycredit,
	@wocredit,
	@invoicebalance,
	@invoicecreditamt,
	@servicereportdesc,
	@custpo_member_id,
	@custpo_dt,
	@verbal_quote,
	@exp_labor,
	@assetid,
	@default_invoicetype,
	@auto_invoice,
	@glposting_instructions,
	@isdownpayment,
	@schedule_status_id,
	@notification_sent,
	@survey_sent,
	@warranty,
	@survey_finished,
	@parent_woprog_id,
    @currency,
    @default_labour_sell_gl,
    @default_material_sell_gl,
    @inspection_link ,
    @inspection_required ,
    @labor_only,
    @mat_only,
    @sub_only,
    @fixed_labour_rate ,
    @fixed_material_markup ,
    @sustainability_project ,
    @use_fixed_labour_rate ,
    @use_fixed_material_markup,
    @bdm,
    @acting_ram,
    @woprog_jobtag_id,
    @revenue_line_id,
    @term_id,
    @is_shell_wo,
    @customer_ref,
	@allow_mobile_part_management,
    @enable_prevailing_wages,
    @invoice_fb_issues
	)";
                #endregion CommandText
                #region Parameters
                my_comm.Parameters.AddWithValue("@address_id", ex_ddl(ddlAddress));
                my_comm.Parameters.AddWithValue("@advancement", hdnRD.Contains("memAdvancement") ? hdnRD["memAdvancement"] : "");
                my_comm.Parameters.AddWithValue("@apply_discount", false);
                my_comm.Parameters.AddWithValue("@associate_woprog_id", chkProgress.Checked ? ex_ddl(ddlJobCostWO) : 0);
                my_comm.Parameters.AddWithValue("@auto_invoice", 0);
                my_comm.Parameters.AddWithValue("@billed_as_quoted", 0);
                my_comm.Parameters.AddWithValue("@bvwo", order_number);
                my_comm.Parameters.AddWithValue("@closedatetime", null);
                my_comm.Parameters.AddWithValue("@collection_notes", null);
                my_comm.Parameters.AddWithValue("@business_unit_id", ddlCompany.Value);
                my_comm.Parameters.AddWithValue("@contact_id", ex_ddl(ddlContact));
                my_comm.Parameters.AddWithValue("@corecompetency", hdnRD.Contains("memCompetency") ? hdnRD["memCompetency"] : "");
                my_comm.Parameters.AddWithValue("@creditcard_payment", cc);
                my_comm.Parameters.AddWithValue("@customer_consolidation", null);
                my_comm.Parameters.AddWithValue("@customer_id", ex_ddl(ddlCustomer).ToString().Trim());
                my_comm.Parameters.AddWithValue("@customername", ddlCustomer.Text.Replace("'", ""));
                my_comm.Parameters.AddWithValue("@customersnotes", "");
                my_comm.Parameters.AddWithValue("@custpo", txtCustPO.Text.Replace("'", ""));
                my_comm.Parameters.AddWithValue("@custpo_dt", txtCustPO.Text.Trim() != "" ? Toolbox.MySQLNow_long() : null);
                my_comm.Parameters.AddWithValue("@custpo_member_id", txtCustPO.Text.Trim() != "" ? user.id : 0);
                my_comm.Parameters.AddWithValue("@cutby_memberid", user.id);
                my_comm.Parameters.AddWithValue("@cutdatetime", null);
                my_comm.Parameters.AddWithValue("@dayscredit", Convert.ToInt32(spndayscredit.Value));
                my_comm.Parameters.AddWithValue("@default_invoicetype", combo_default_invoicetype.Value);

                my_comm.Parameters.AddWithValue("@description", txtWODescription.Text);
                my_comm.Parameters.AddWithValue("@erquotedprice", 0);
                my_comm.Parameters.AddWithValue("@erwarranty", 0);
                my_comm.Parameters.AddWithValue("@exp_labor", txtlaborvalue.Text != "" ? txtlaborvalue.Text.Trim() : null);
                my_comm.Parameters.AddWithValue("@expected_enddate", Convert.ToDateTime(dteExpEndDate.Text).ToString("yyyy-MM-dd"));
                my_comm.Parameters.AddWithValue("@expected_sales_value", str_expected_sales_value);
                my_comm.Parameters.AddWithValue("@expected_startdate", dteStartDate.Text != "" ? Convert.ToDateTime(dteStartDate.Text).ToString("yyyy-MM-dd") : null);
                my_comm.Parameters.AddWithValue("@expectedcheckrun", null);
                my_comm.Parameters.AddWithValue("@glposting_instructions", null);
                my_comm.Parameters.AddWithValue("@grossmargin", 0);
                my_comm.Parameters.AddWithValue("@hold", chkOnHold.Checked);
                my_comm.Parameters.AddWithValue("@hoursspentquoting", hoursspentonqote);
                my_comm.Parameters.AddWithValue("@interbranch_woprog_id", null);
                my_comm.Parameters.AddWithValue("@invoice_collection_status", null);
                my_comm.Parameters.AddWithValue("@invoice_paid_date", null);
                my_comm.Parameters.AddWithValue("@invoice_tax", null);
                my_comm.Parameters.AddWithValue("@invoicebalance", 0);
                my_comm.Parameters.AddWithValue("@invoicecreditamt", 0);
                my_comm.Parameters.AddWithValue("@invoicedate", null);
                my_comm.Parameters.AddWithValue("@invoicednettotal", 0);
                my_comm.Parameters.AddWithValue("@invoiceno", null);
                my_comm.Parameters.AddWithValue("@iscredit", is_credit);
                my_comm.Parameters.AddWithValue("@isdownpayment", is_downpayment);
                my_comm.Parameters.AddWithValue("@isrebill", is_rebill);
                my_comm.Parameters.AddWithValue("@jobcost_tracking", 0);
                my_comm.Parameters.AddWithValue("@laborcost", 0);
                my_comm.Parameters.AddWithValue("@laboursellestimated", 0);
                my_comm.Parameters.AddWithValue("@labourtotalsell", 0);
                my_comm.Parameters.AddWithValue("@last_email_date", null);
                my_comm.Parameters.AddWithValue("@last_faxed_date", null);
                my_comm.Parameters.AddWithValue("@localtax1", 0);
                my_comm.Parameters.AddWithValue("@localtax2", 0);
                my_comm.Parameters.AddWithValue("@localtax3", 0);
                my_comm.Parameters.AddWithValue("@location_in_plant", txtAreainPlant.Text.Replace("'", "").Replace("\"", "").Replace(",", "."));
                my_comm.Parameters.AddWithValue("@makeid", 0);
                my_comm.Parameters.AddWithValue("@materialcost", 0);
                my_comm.Parameters.AddWithValue("@materialsellestimated", 0);
                my_comm.Parameters.AddWithValue("@materialtotalsell", 0);
                my_comm.Parameters.AddWithValue("@modelid", 0);
                my_comm.Parameters.AddWithValue("@notification_sent", null);
                my_comm.Parameters.AddWithValue("@onholdmemberid", chkOnHold.Checked ? user.id : 0);
                my_comm.Parameters.AddWithValue("@opendatetime", null);
                my_comm.Parameters.AddWithValue("@parent_woprog_id", chkProgress.Checked ? 0 : parent_woprog_id);
                my_comm.Parameters.AddWithValue("@pm_memberid", ex_ddl(ddlPM));
                my_comm.Parameters.AddWithValue("@progressbilled", 0);
                my_comm.Parameters.AddWithValue("@quotedamount", is_tm != 1 ? quotedamount : 0);
                my_comm.Parameters.AddWithValue("@WOProg_QuotedHoursTotal", 0);
                my_comm.Parameters.AddWithValue("@quotedprice_type", null);
                my_comm.Parameters.AddWithValue("@quoteid",is_tm != 1? (object)ddlquote.Value ?? (object)0: (object)0);
                my_comm.Parameters.AddWithValue("@rdflag", rd);
                my_comm.Parameters.AddWithValue("@schedule_status_id", null);
                my_comm.Parameters.AddWithValue("@servicecall", ChkServiceCall.Checked);
                my_comm.Parameters.AddWithValue("@servicereportdesc", null);
                my_comm.Parameters.AddWithValue("@sp_memberid", 0);
                my_comm.Parameters.AddWithValue("@specialinstructions", txtSpecialInstruction.Text.Replace("'", "").Replace("\"", "").Replace(",", "."));
                my_comm.Parameters.AddWithValue("@status", OpsWOStatus.Open);
                my_comm.Parameters.AddWithValue("@stilltobebilled", 0);
                my_comm.Parameters.AddWithValue("@survey_finished", null);
                my_comm.Parameters.AddWithValue("@survey_sent", null);
                my_comm.Parameters.AddWithValue("@tax1", tax1);
                my_comm.Parameters.AddWithValue("@tax2", tax2);
                my_comm.Parameters.AddWithValue("@tax3", tax3);
                my_comm.Parameters.AddWithValue("@tax4", tax4);
                my_comm.Parameters.AddWithValue("@timesheet_percentage", 0);
                my_comm.Parameters.AddWithValue("@totaltandm", 0);
                my_comm.Parameters.AddWithValue("@uncertainty", hdnRD.Contains("memUncertainty") ? hdnRD["memUncertainty"] : "");
                my_comm.Parameters.AddWithValue("@verbal_quote", chk_verbal.Checked);
                my_comm.Parameters.AddWithValue("@vis_to_cust", false);
                my_comm.Parameters.AddWithValue("@warranty", chk_warranty.Checked);
                my_comm.Parameters.AddWithValue("@whycredit", null);
                my_comm.Parameters.AddWithValue("@whyhold", txtwhyhold.Text);
                my_comm.Parameters.AddWithValue("@wocredit", woprog_wocredit);
                my_comm.Parameters.AddWithValue("@currency", parent_woprog_id > 0 ? parent_wo.currency_id : currency);
                my_comm.Parameters.AddWithValue("@default_labour_sell_gl", "");
                my_comm.Parameters.AddWithValue("@default_material_sell_gl", "");

                my_comm.Parameters.AddWithValue("@inspection_link", tb_inspection_link.Text);
                my_comm.Parameters.AddWithValue("@inspection_required", chk_inspection_yes.Checked);
                my_comm.Parameters.AddWithValue("@enable_prevailing_wages", chkEnablePrevailingWages.Checked);
                my_comm.Parameters.AddWithValue("@labor_only", chk_labor_only.Checked);
                my_comm.Parameters.AddWithValue("@mat_only", chk_mat_only.Checked);
                my_comm.Parameters.AddWithValue("@invoice_fb_issues", chkInvoiceFeedbackIssues.Checked);
                my_comm.Parameters.AddWithValue("@sub_only", chk_sub_only.Checked);
                my_comm.Parameters.AddWithValue("@fixed_labour_rate", spn_fixed_labour.Value);
                my_comm.Parameters.AddWithValue("@fixed_material_markup", spn_material_markup.Value);
                my_comm.Parameters.AddWithValue("@sustainability_project", chk_sustain.Checked);
                my_comm.Parameters.AddWithValue("@use_fixed_labour_rate", chk_fixed_labour.Checked);
                my_comm.Parameters.AddWithValue("@use_fixed_material_markup", chk_fixed_markup.Checked);
                my_comm.Parameters.AddWithValue("@bdm", ddl_custram.Value);
                my_comm.Parameters.AddWithValue("@acting_ram", ddl_rams.Value);
                my_comm.Parameters.AddWithValue("@woprog_jobtag_id", ddl_jobtag.Value);
                my_comm.Parameters.AddWithValue("@revenue_line_id", is_tm != 1 ? ddlRevenueLines.Value.ToString() : this.GetConfigSettingByQuery(Configuration.GetRevenueLineTM));
                my_comm.Parameters.AddWithValue("@term_id", Convert.ToInt32(combo_default_terms.Value));
                my_comm.Parameters.AddWithValue("@is_shell_wo", chkShellWO.Checked);
                my_comm.Parameters.AddWithValue("@customer_ref", tbCustomerReference.Text);
                my_comm.Parameters.AddWithValue("@allow_mobile_part_management", chkMobilePartManage.Checked);
                #endregion Parameters

                var cust_id = 0;
                int.TryParse(ex_ddl(ddlCustomer).ToString().Trim(), out cust_id);

                my_comm.ExecuteNonQuery();
                my_comm.CommandText = "SELECT LAST_INSERT_ID()";
                new_woprog_id = Convert.ToInt32(my_comm.ExecuteScalar().ToString());
                #endregion

                _wo = new NeWOProg(new_woprog_id);

                hid_ts.Value = _wo.woprog_ts.Ticks.ToString();
                Toolbox.doSQL_void(conn, @"update woprog set woprog_bvwo = LPAD(woprog_id, 10, 0)  where woprog_id =@v0 limit 1 ", new object[] { _wo.woprog_id });
                _wo = new NeWOProg(new_woprog_id);
                var q = new quote();
                #endregion
                try
                {
                    #region quote
                    if (ex_ddl(ddlquote) != null && ex_ddl(ddlquote).ToString() != "0")
                    {
                        q = new quote(Convert.ToInt32(ddlquote.Value));

                        try
                        {
                            new quote().ReceiveNeQuotes(Convert.ToInt32(ddlquote.Value), _wo.woprog_id, txtCustPO.Text, user.id, Convert.ToInt32(ddlContact.Value), businessUnit.DSN);
                        }
                        catch (Exception ex)
                        {
                            //throw new Exception(ex + " Error opening the Quote");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + " Error opening the Quote');", true);
                            return;
                        }
                        var dt = Toolbox.doSQL_dt(conn, @"Select section from quote_section  where quote_id =@v0 and revision =@v1 ", new object[] { q.QuoteID, q.Revision });
                        foreach (DataRow dr in dt.Rows)
                        {
                            var section = Toolbox.do_value_from(dr["section"], false).Replace(",", "").Trim();
                            if (section != "")
                            {
                                Toolbox.doSQL_void(conn, @" INSERT INTO wocomment ( comments, workorder_id, personal, printcomments, member_id_audit, wocomment_member_id, business_unit_id , created_date, modified_date,woprog_id ) VALUES ( @v0 , @v1 , 0, 0, @v2 , @v2 , @v3 , NOW(), NOW(), @v4  )", new object[] { section, order_number, user.id, ddlCompany.Value, _wo.woprog_id });
                            }
                        }
                        if (q.Price == null)
                        {
                            quotedamount = 0;
                        }
                        else
                        {

                            quotedamount = Convert.ToDouble(q.Price);
                        }
                        hoursspentonqote = Convert.ToDouble(q.HoursSpent);

                        //Update woprog_quotedHoursTotal so that it shows on the master work order grid
                        Toolbox.doSQL_void(conn, @"CALL quoted_hours_for_wo(@v0)", new object[] { _wo.woprog_id });

                        #region Update Quote History
                        var pmmember = new NeMember(Convert.ToInt32(ddlPM.Value));
                        var quote_history_information = "Received - WO:" + new_woprog_id;
                        quote_history_information += " PM:" + pmmember.FullName;
                        quote_history_information += " StartDate:" + dteStartDate.Text;
                        quote_history_information += " EndDate:" + dteExpEndDate.Text;

                        var sql_quote_history = @"INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event) 
VALUES(now()," + user.id + ", " + q.QuoteID + ", " + q.Revision + ", '" + quote_history_information + "')";
                        try
                        {
                            Toolbox.doSQL_void(conn, @"INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event)  VALUES(now(),@v0,@v1,@v2,@v3)", new object[] { user.id, q.QuoteID, q.Revision, quote_history_information });
                        }
                        catch (Exception ex)
                        {
                            //throw new Exception(ex.ToString());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                            return;
                        }

                        #endregion Update Quote History
                    }
                    #endregion quote
                    error_flag = false;
                    #region Change customer status, if applicable.
                    if (_wo.WOProg_Customer_ID != 0)
                    {
                        var csp = new customer_sales_properties(_wo.woprog_Address_ID);
                        if (wo_count == 0) // No work orders cut before, move this customer's status to "Onboarding"  
                        {

                            csp.status_id = 9;
                            csp.save();

                            NECustomer.add_to_customer_history(Convert.ToInt32(_wo.WOProg_Customer_ID),
                                                                user.id,
                                                                DateTime.Now,
                                                                "First Work Order Added - Changing status to 'Onboarding'",
                                                                18,
                                                                1,
                                                                _wo.woprog_Address_ID
                                                                );

                        }
                        if (wo_count == 2) // 2 work orders cut before, move this customer's status to "Customer", or status_id 3 
                        {

                            csp.status_id = 3;
                            csp.save();

                            NECustomer.add_to_customer_history(Convert.ToInt32(_wo.WOProg_Customer_ID),
                                                                user.id,
                                                                DateTime.Now,
                                                                "Third Work Order Added - Changing status to 'Customer'",
                                                                18,
                                                                1,
                                                                _wo.woprog_Address_ID
                                                                );
                        }
                        if (csp.status_id == 0)  // if this address is stale, reset it to customer
                        {
                            csp.status_id = 3;
                            csp.save();
                            NECustomer.add_to_customer_history(Convert.ToInt32(_wo.WOProg_Customer_ID),
                                                                user.id,
                                                                DateTime.Now,
                                                                "New Work Order Added - Changing status from 'stale' back to 'Customer'",
                                                                18,
                                                                1,
                                                                _wo.woprog_Address_ID
                                                                );
                        }
                        else
                        {
                            NECustomer.add_to_customer_history(Convert.ToInt32(_wo.WOProg_Customer_ID),
                                                                user.id,
                                                                DateTime.Now,
                                                                "WO: " + _wo.Description,
                                                                2,
                                                                1,
                                                                _wo.woprog_Address_ID
                                                                );

                        }

                    }
                    #endregion Change status, if applicable.
                    double ProgAmount = 0;
                    double.TryParse(txtpb_amt.Text, out ProgAmount);
                    #region if its a credit
                    if (is_credit)
                    {
                        var to_credit = new NeWOProg(woprog_wocredit);
                        try
                        {
                            // pull in the timesheet comments from the original work order.
                            Toolbox.doSQL_void(conn, @"CALL PROC_DUPLICATE_WOCOMMENT(@v0,@v1)", new object[] { woprog_wocredit, new_woprog_id });
                            NeWOProg.add_history(new_woprog_id, user.id, "Just Scanned", now);
                            NeWOProg.add_history(new_woprog_id, user.id, OpsWOStatus.WaitingPMApproval, now);
                            Toolbox.doSQL_void(conn, @"UPDATE woprog  SET woprog_opendatetime =@v0, woprog_status='Waiting PM Approval', woprog_timesheet_percentage = 100   WHERE WOProg_ID =@v1", new object[] { now, new_woprog_id });
                            create_pdf(to_credit, ddlCompany.Value.ToString(), new_woprog_id, order_number, "Credit");
                            var dtoldwo = Toolbox.doSQL_dt(conn, @" SELECT * FROM wo_detailh WHERE woprog_id = @v0  AND billtypeid IN (0,2,3,10,11,12) ORDER BY rec_no", new object[] { to_credit.woprog_id });
                            var x = 1;
                            foreach (DataRow dr in dtoldwo.Rows)
                            {
                                var wodc = new NeWODetailCurrent();
                                wodc.woprog_id = new_woprog_id;
                                wodc.type = dr["type"].ToString();
                                wodc.track_part = 0;
                                wodc.tax1 = Convert.ToInt16(dr["tax1"]);
                                wodc.tax2 = Convert.ToInt16(dr["tax2"]);
                                wodc.tax3 = Convert.ToInt16(dr["tax3"]);
                                wodc.tax4 = Convert.ToInt16(dr["tax4"]);
                                wodc.rec_no = x;
                                wodc.qty_ordered = Convert.ToDouble(dr["qty_ordered"]) * -1;
                                wodc.qty_committed = Convert.ToDouble(dr["qty_committed"]) * -1;
                                wodc.qty_invoiced = Convert.ToDouble(dr["qty_invoiced"]) * -1;
                                wodc.cost = Convert.ToDouble(dr["price_cost"]);
                                wodc.sell = Convert.ToDouble(dr["price_sell"]);
                                wodc.unit = Convert.ToDouble(dr["price_unit"]);
                                wodc.origin = "Automatically Added";
                                wodc.notes = dr["notes"].ToString();
                                wodc.master_id = Convert.ToInt32(dr["master_id"]);
                                wodc.issues = "";
                                wodc.discount = Convert.ToDouble(dr["discount"]);
                                wodc.description = dr["description"].ToString();
                                wodc.date_required = dr["date_required"] == DBNull.Value
                                                            ? ""
                                                            : Convert.ToDateTime(dr["date_required"]).ToString("yyyy-MM-dd");
                                wodc.date_added = dr["date_added"].ToString();
                                wodc.consignment_id = Toolbox.ReturnZeroIfNull_int(dr["consignment_id"]);
                                wodc.business_unit_id = Convert.ToInt32(dr["business_unit_id"]);
                                wodc.code = dr["code"].ToString();
                                wodc.bvwo = Convert.ToInt32(order_number);
                                wodc.billtypeid = new List<int> { 0, 3, 11 }.Contains(Toolbox.ReturnZeroIfNull_int(dr["billtypeid"]))
                                                            ? 10
                                                            : 0;
                                wodc.added_by = Convert.ToInt32(dr["added_by"]);
                                wodc.save(user, "/sections/workorder/index.aspx.cs - create_workorder #2", false);
                                x++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Toolbox.do_errorLog_errorStack(ex);
                            //throw new Exception(ex.ToString());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                        }
                    }
                    #endregion if its a credit
                    #region if its a rebill
                    if (is_rebill)
                    {
                        var wotocredit = new NeWOProg(woprog_wocredit);
                        try
                        {
                            // pull in the timesheet comments from the original work order.
                            Toolbox.doSQL_void(conn, @"CALL PROC_DUPLICATE_WOCOMMENT(@v0,@v1)", new object[] { woprog_wocredit, new_woprog_id });
                            var dtcomments = Toolbox.doSQL_dt(conn, @"SELECT * from woprogcomment  where WOProgComment_WOProg_ID =@v0", new object[] { woprog_wocredit });
                            foreach (DataRow dr in dtcomments.Rows)
                            {
                                var comment_woprog_id = dr["woprogcomment_woprog_id"];
                                var comment_member_id = dr["woprogcomment_member_id"];
                                var comment_text = dr["woprogcomment_text"];
                                var comment_x = dr["woprogcomment_x"];
                                var comment_y = dr["woprogcomment_y"];
                                var comment_datetime = Toolbox.MySQL_longdt((DateTime)dr["woprogcomment_datetime"]);
                                var comment_deleted = dr["woprogcomment_deleted"];
                                Toolbox.doSQL_void(conn, @" INSERT INTO woprogcomment ( woprogcomment_woprog_id, woprogcomment_member_id, woprogcomment_text, woprogcomment_x, woprogcomment_y, woprogcomment_datetime, woprogcomment_deleted ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6  )", new object[] { new_woprog_id, comment_member_id, comment_text, comment_x, comment_y, comment_datetime, comment_deleted });
                            }
                            NeWOProg.add_history(woprog_wocredit, user.id, "Just Scanned", now);
                            NeWOProg.add_history(woprog_wocredit, user.id, OpsWOStatus.InitialPrep, now);
                            Toolbox.doSQL_void(conn, @"UPDATE woprog SET woprog_opendatetime =@v0 , WOProg_Status='Initial Prep', WOProg_TimeSheet_Percentage = 100 WHERE WOProg_ID =@v1 ", new object[] { now, new_woprog_id });
                            //Toolbox.doSQL_void(@"UPDATE SALES_ORDER_HEADER SET STATUS='3'  WHERE NUMBER=?", dsn, new object[] { order_number });
                            create_pdf(wotocredit, ddlCompany.Value.ToString(), new_woprog_id, order_number, "Rebill");
                            var dtoldwo = Toolbox.doSQL_dt(conn, @"Select * from wo_detail_history  where wo_detail_history_woprog_id=@v0 and wo_detail_history_billtypeid in (0,2,3,10,11,12) order by wo_detail_history_rec_no", new object[] { wotocredit.woprog_id });

                            foreach (DataRow dr in dtoldwo.Rows)
                            {
                                var newrow = new NeWODetailCurrent
                                {
                                    woprog_id = new_woprog_id,
                                    type = dr["wo_detail_history_type"].ToString(),
                                    track_part = 0,
                                    tax1 = Convert.ToInt16(dr["wo_detail_history_tax1"]),
                                    tax2 = Convert.ToInt16(dr["wo_detail_history_tax2"]),
                                    tax3 = Convert.ToInt16(dr["wo_detail_history_tax3"]),
                                    tax4 = Convert.ToInt16(dr["wo_detail_history_tax4"]),
                                    rec_no = Convert.ToInt32(dr["wo_detail_history_rec_no"]),
                                    qty_ordered = Convert.ToDouble(dr["wo_detail_history_qty_ordered"]),
                                    qty_committed = Convert.ToDouble(dr["wo_detail_history_qty_committed"]),
                                    qty_invoiced = Convert.ToDouble(dr["wo_detail_history_qty_invoiced"]),
                                    cost = 0.0,
                                    //cost = Convert.ToDouble(dr["wo_detail_history_price_cost"]),
                                    sell = Convert.ToDouble(dr["wo_detail_history_price_sell"]),
                                    unit = Convert.ToDouble(dr["wo_detail_history_price_unit"]),
                                    origin = "Automatically Added",
                                    notes = dr["wo_detail_history_notes"].ToString(),
                                    master_id = Convert.ToInt32(dr["wo_detail_history_master_id"]),
                                    issues = dr["wo_detail_history_issues"].ToString(),
                                    discount = Convert.ToDouble(dr["wo_detail_history_discount"]),
                                    description = dr["wo_detail_history_description"].ToString(),
                                    date_required = dr["wo_detail_history_date_required"] == DBNull.Value
                                                        ? ""
                                                        : Convert.ToDateTime(dr["wo_detail_history_date_required"]).ToString("yyyy-MM-dd"),
                                    date_added = dr["wo_detail_history_date_added"].ToString(),
                                    consignment_id = Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_consignment_id"]),
                                    business_unit_id = Convert.ToInt32(dr["business_unit_id"]),
                                    code = dr["wo_detail_history_code"].ToString(),
                                    bvwo = Convert.ToInt32(order_number),
                                    billtypeid = Convert.ToInt32(dr["wo_detail_history_billtypeid"]),
                                    added_by = Convert.ToInt32(dr["wo_detail_history_added_by"])
                                };
                                newrow.save(user, "/sections/workorder/index.aspx.cs - create_workorder #3", false);
                            }
                        }
                        catch (Exception ex)
                        {
                            Toolbox.do_errorLog_errorStack(ex);
                            //throw new Exception(ex.ToString());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                        }
                    }
                    #endregion
                    #region If Linked To a Quote

                    if (ddlquote.Value != null && ddlquote.Value.ToString() != "0" && is_tm != 1)  // if it's a quoted job, save the quote line.
                    {

                        #region Insert Quote Line
                        var x = 2;
                        if (businessUnit.is_er)
                        {
                            #region Repair
                            var quote = new quote(Convert.ToInt32(ddlquote.Value));
                            var dt = Toolbox.doSQL_dt(conn, @"SELECT
  *
FROM
  quote_worksheet a
INNER JOIN quote_section b ON a.section_id = b.id
WHERE a.quote_id = @v0
  AND a.revision = @v1
  AND a.is_checked
  AND b.is_checked
  AND consignment_id != 0", new object[] { quote.QuoteID, quote.Revision });
                            if (dt.Rows.Count > 0)
                            {
                                #region Has Consignment
                                foreach (DataRow dr in dt.Rows)
                                {
                                    var wo_quote_line = new NeWODetailCurrent();
                                    wo_quote_line.tax1 = businessUnit.TaxQuotedJobs == 1 ? tax1 : 0;
                                    wo_quote_line.tax2 = businessUnit.TaxQuotedJobs == 1 ? tax2 : 0;
                                    wo_quote_line.tax3 = businessUnit.TaxQuotedJobs == 1 ? tax3 : 0;
                                    wo_quote_line.tax4 = businessUnit.TaxQuotedJobs == 1 ? tax4 : 0;
                                    wo_quote_line.description = dr["description"].ToString();
                                    wo_quote_line.master_id = 2139;
                                    wo_quote_line.code = "QUOTE";
                                    wo_quote_line.business_unit_id = Convert.ToInt32(ddlCompany.Value);
                                    wo_quote_line.qty_ordered = 1;
                                    wo_quote_line.qty_committed = 1;
                                    wo_quote_line.qty_invoiced = 1;
                                    wo_quote_line.cost = .01;
                                    wo_quote_line.sell = Convert.ToDouble(dr["extended_per"]);
                                    wo_quote_line.unit = Convert.ToDouble(dr["extended_per"]);
                                    wo_quote_line.woprog_id = new_woprog_id;
                                    wo_quote_line.bvwo = Convert.ToInt32(order_number);
                                    wo_quote_line.origin = "Automatically Added";
                                    wo_quote_line.billtypeid = 3;
                                    wo_quote_line.rec_no = x;
                                    wo_quote_line.type = "Q";
                                    wo_quote_line.consignment_id = Convert.ToInt32(dr["consignment_id"]);
                                    try
                                    {
                                        wo_quote_line.save(user, "/sections/workorder/index.aspx.cs - create_workorder #4", false);
                                    }
                                    catch (Exception ex)
                                    {
                                        Toolbox.do_errorLog_errorStack(ex);
                                        // throw new Exception(ex.ToString());
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                                    }
                                    var consign = new consignment(Convert.ToInt32(dr["consignment_id"]));
                                    consign.load();
                                    consign.status = consignment.StatusType.InProcess;
                                    consign.save();
                                    x++;
                                }
                                #endregion Has Consignment
                            }
                            else
                            {
                                #region Doesn't Have Consignment
                                var wo_quote_line = new NeWODetailCurrent();
                                wo_quote_line.tax1 = businessUnit.TaxQuotedJobs == 1 ? tax1 : 0;
                                wo_quote_line.tax2 = businessUnit.TaxQuotedJobs == 1 ? tax2 : 0;
                                wo_quote_line.tax3 = businessUnit.TaxQuotedJobs == 1 ? tax3 : 0;
                                wo_quote_line.tax4 = businessUnit.TaxQuotedJobs == 1 ? tax4 : 0;
                                wo_quote_line.description = "Price as Quoted Q: " + ddlquote.Value;
                                wo_quote_line.master_id = 2139;
                                wo_quote_line.code = "QUOTE";
                                wo_quote_line.business_unit_id = Convert.ToInt32(ddlCompany.Value);
                                wo_quote_line.qty_ordered = 1;
                                wo_quote_line.qty_committed = 1;
                                wo_quote_line.qty_invoiced = 1;
                                wo_quote_line.cost = .01;
                                wo_quote_line.sell = quotedamount;
                                wo_quote_line.unit = quotedamount;
                                wo_quote_line.woprog_id = new_woprog_id;
                                wo_quote_line.bvwo = Convert.ToInt32(order_number);
                                wo_quote_line.origin = "Automatically Added";
                                wo_quote_line.billtypeid = 3;
                                wo_quote_line.rec_no = 2;
                                wo_quote_line.type = "Q";
                                try
                                {
                                    wo_quote_line.save(user, "/sections/workorder/index.aspx.cs - create_workorder #5", false);
                                }
                                catch (Exception ex)
                                {
                                    Toolbox.do_errorLog_errorStack(ex);
                                    // throw new Exception(ex.ToString());
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                                }
                                #endregion Doesn't Have Consignment
                            }
                            #endregion Repair
                        }
                        else
                        {
                            var wo_quote_line = new NeWODetailCurrent
                            {
                                tax1 = businessUnit.TaxQuotedJobs == 1 ? tax1 : 0,
                                tax2 = businessUnit.TaxQuotedJobs == 1 ? tax2 : 0,
                                tax3 = businessUnit.TaxQuotedJobs == 1 ? tax3 : 0,
                                tax4 = businessUnit.TaxQuotedJobs == 1 ? tax4 : 0,
                                description = "Price as Quoted Q: " + ddlquote.Value,
                                master_id = 2139,
                                code = "QUOTE",
                                business_unit_id = Convert.ToInt32(ddlCompany.Value),
                                qty_ordered = 1,
                                qty_committed = 1,
                                qty_invoiced = 1,
                                cost = 0,
                                sell = quotedamount,
                                unit = quotedamount,
                                woprog_id = new_woprog_id,
                                bvwo = Convert.ToInt32(order_number),
                                origin = "Automatically Added",
                                billtypeid = 11,
                                rec_no = 2,
                                type = "Q"
                            };
                            try
                            {
                                wo_quote_line.save(user, "/sections/workorder/index.aspx.cs - create_workorder #6", false);
                            }
                            catch (Exception ex)
                            {
                                Toolbox.do_errorLog_errorStack(ex);
                                // throw new Exception(ex.ToString());
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                            }
                        }

                        var tempquote2 = new quote(Convert.ToInt32(ddlquote.Value));
                        #region build task list

                        var quote_details = Toolbox.doSQL_dt(conn, @"Select linetext,line_number from quote_extratext  where quote_id =@v0 and revision =@v1  and type =1 order by line_number", new object[] { tempquote2.QuoteID, tempquote2.Revision });
                        foreach (DataRow qd in quote_details.Rows)
                        {
                            Toolbox.doSQL_void(conn, @"Insert into woprog_tasks (woprog_id,task,complete,_order)  Values(@v0,@v1,0,@v2)", new object[] { new_woprog_id, qd["linetext"], qd["line_number"] });
                        }
                        #endregion

                        #endregion Insert Quote Line
                    }
                    #endregion If Linked To a Quote
                    #region if associated (progress billing)
                    if (chkProgress.Checked)
                    {
                        var ProductDescription = "";
                        var revision = 0;
                        var quote_id = 0;
                        quote.splice(jobcost_wo.QuoteID, out quote_id, out revision);
                        q = new quote(quote_id);
                        if ((string)cbopb_type.Value == "Flat Amount")
                        {
                            if (chkProgressBillCredit.Checked)
                            {
                                ProductDescription = string.Format("Credit for {0:C2} for Quote Q-{1}{2} WO: {3}", ProgAmount, quote_id, revision, jobcost_wo.OrderNumber);
                            }
                            else if (chkDownPayment.Checked)
                            {
                                ProductDescription = string.Format("Down Payment of {0:C2} for Quote Q-{1}{2} WO: {3}", ProgAmount, quote_id, revision, jobcost_wo.OrderNumber);
                            }
                            else
                            {
                                ProductDescription = string.Format("Progress Billing of {0:C2} of Quote Q-{1}{2} WO: {3}", ProgAmount, quote_id, revision, jobcost_wo.OrderNumber);
                            }
                        }
                        else
                        {
                            if (chkProgressBillCredit.Checked)
                            {
                                ProductDescription = string.Format("Credit for {0:P2} ({1:C2}) of Quote Q-{2}{3} WO: {4}", _quote_percent, _quote_flatamount, quote_id, revision, jobcost_wo.OrderNumber);
                            }
                            else if (chkDownPayment.Checked)
                            {
                                ProductDescription = string.Format("Down Payment of {0:P2} ({1:C2}) of Quote Q-{2}{3} WO: {4}", _quote_percent, _quote_flatamount, quote_id, revision, jobcost_wo.OrderNumber);
                            }
                            else
                            {
                                ProductDescription = string.Format("Progress Billing of {0:P2} ({1:C2}) of Quote Q-{2}{3} WO: {4}", _quote_percent, _quote_flatamount, quote_id, revision, jobcost_wo.OrderNumber);
                            }
                        }

                        //Update Status	
                        var pb_margin = 0.5;
                        try
                        {
                            pb_margin = Convert.ToDouble(Regex.Replace(txt_pb_margin.Text, "[^.0-9]", "")) / 100;
                        }
                        catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }

                        var quoted_price = Convert.ToDouble(q.Price);

                        var jobcost_branch = new NeBusinessUnit(jobcost_wo.business_unit_id);
                        var isTypePercentage = cbopb_type.Value == "Percentage";

                        var prog_bill = new NeWODetailCurrent
                        {
                            added_by = user.id,
                            date_added = Toolbox.MySQLNow_long(),
                            date_modified = Toolbox.MySQLNow_long(),
                            tax1 = businessUnit.TaxQuotedJobs == 1 ? jobcost_wo.woprog_tax1 : 0,
                            tax2 = businessUnit.TaxQuotedJobs == 1 ? jobcost_wo.woprog_tax2 : 0,
                            tax3 = businessUnit.TaxQuotedJobs == 1 ? jobcost_wo.woprog_tax3 : 0,
                            tax4 = businessUnit.TaxQuotedJobs == 1 ? jobcost_wo.woprog_tax4 : 0,
                            description = ProductDescription,
                            master_id = 2139,
                            code = "QUOTE",
                            business_unit_id = Convert.ToInt32(ddlCompany.Value),

                            // Added condition for Credit progress bills
                            qty_ordered = chkProgressBillCredit.Checked ? isTypePercentage ? -1 * _quote_percent : -1 : isTypePercentage ? _quote_percent : 1,
                            qty_committed = chkProgressBillCredit.Checked ? isTypePercentage ? -1 * _quote_percent : -1 : isTypePercentage ? _quote_percent : 1,
                            qty_invoiced = chkProgressBillCredit.Checked ? isTypePercentage ? -1 * _quote_percent : -1 : isTypePercentage ? _quote_percent : 1,

                            cost = 0.01, // OLD Price quoted_price - quoted_price * pb_margin,

                            sell = (string)cbopb_type.Value == "Flat Amount" ? ProgAmount : quoted_price,
                            unit = (string)cbopb_type.Value == "Flat Amount" ? ProgAmount : quoted_price,

                            woprog_id = new_woprog_id,
                            bvwo = Convert.ToInt32(order_number),
                            origin = "Automatically Added",
                            billtypeid = 12,
                            rec_no = 2,
                            type = "Q"
                        };
                        try
                        {
                            prog_bill.save(user, "/sections/workorder/index.aspx.cs - create_workorder #7", false);
                        }
                        catch (Exception ex)
                        {
                            Toolbox.do_errorLog_errorStack(ex);
                            // throw new Exception(ex.ToString());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                        }
                        try
                        {
                            NeWOProg.add_history(woprog_wocredit, user.id, "Just Scanned", now);
                            NeWOProg.add_history(woprog_wocredit, user.id, OpsWOStatus.InitialPrep, now);
                            Toolbox.doSQL_void(conn, @"UPDATE woprog  SET WOProg_OpenDateTime =@v0, WOProg_Status='Initial Prep', WOProg_TimeSheet_Percentage = 100   WHERE WOProg_ID =@v1", new object[] { now, new_woprog_id });
                            create_pdf(jobcost_wo, ddlCompany.Value.ToString(), new_woprog_id, order_number, "PB");
                        }
                        catch (Exception ex)
                        {
                            Toolbox.do_errorLog_errorStack(ex);
                            // throw new Exception(ex.ToString());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                        }

                        var progbill_quote_id = jobcost_wo.QuoteID.Substring(0, 6);
                        var progbill_quote_rev = jobcost_wo.QuoteID.Substring(6, 1);
                        if (chkProgressBillCredit.Checked)
                        {
                            ProductDescription = cbopb_type.Text == "Flat Amount"
                                ? string.Format("Credit for {0:C2} of Quote Q-{1}-V{2} WO: {3}", _quote_flatamount, progbill_quote_id, progbill_quote_rev, _wo.OrderNumber)
                                : string.Format("Credit for {0:P2} ({1:C2}) of Quoted Q-{2}-V{3} WO: {4}", _quote_percent, _quote_flatamount, progbill_quote_id, progbill_quote_rev, _wo.OrderNumber);
                        }
                        else if (chkDownPayment.Checked)
                        {
                            ProductDescription = cbopb_type.Text == "Flat Amount"
                                ? string.Format("Down Payment of {0:C2} of Quote Q-{1}-V{2} WO: {3}", _quote_flatamount, progbill_quote_id, progbill_quote_rev, _wo.OrderNumber)
                                : string.Format("Down Payment of {0:P2} ({1:C2}) of Quote Q-{2}-V{3} WO: {4}", _quote_percent, _quote_flatamount, progbill_quote_id, progbill_quote_rev, _wo.OrderNumber);
                        }
                        else
                        {
                            ProductDescription = cbopb_type.Text == "Flat Amount"
                                ? string.Format("Progress Billing of {0:C2} of Quote Q-{1}-V{2} WO: {3}", _quote_flatamount, progbill_quote_id, progbill_quote_rev, _wo.OrderNumber)
                                : string.Format("Progress Billing of {0:P2} ({1:C2}) of Quote Q-{2}-V{3} WO: {4}", _quote_percent, _quote_flatamount, progbill_quote_id, progbill_quote_rev, _wo.OrderNumber);
                        }
                        // Since 2139 lines have been added by now, we need to update timestamp on woprog for it to successfully flow into NetSuite

                        Toolbox.doSQL_affectedrows(@"UPDATE woprog SET woprog_ts = NOW() WHERE woprog_id = @v0", new object[] { jobcost_wo.woprog_id });
                        // Now add credit line to job cost.
                        var job_cost = new NeWODetailCurrent();
                        job_cost.added_by = user.id;
                        job_cost.date_added = Toolbox.MySQLNow_long();
                        job_cost.date_modified = Toolbox.MySQLNow_long();
                        job_cost.tax1 = businessUnit.TaxQuotedJobs == 1 ? jobcost_wo.woprog_tax1 : 0;
                        job_cost.tax2 = businessUnit.TaxQuotedJobs == 1 ? jobcost_wo.woprog_tax2 : 0;
                        job_cost.tax3 = businessUnit.TaxQuotedJobs == 1 ? jobcost_wo.woprog_tax3 : 0;
                        job_cost.tax4 = businessUnit.TaxQuotedJobs == 1 ? jobcost_wo.woprog_tax4 : 0;
                        job_cost.description = ProductDescription;
                        job_cost.added_by = user.id;
                        job_cost.master_id = 2139;
                        job_cost.code = "QUOTE";
                        job_cost.type = "Q";
                        job_cost.business_unit_id = Convert.ToInt32(ddlCompany.Value);
                        job_cost.qty_ordered = prog_bill.qty_ordered * -1;
                        job_cost.qty_committed = prog_bill.qty_committed * -1;
                        job_cost.qty_invoiced = prog_bill.qty_invoiced * -1;
                        job_cost.cost = prog_bill.cost;
                        job_cost.sell = prog_bill.sell;
                        job_cost.unit = prog_bill.unit;
                        job_cost.woprog_id = jobcost_wo.woprog_id;
                        job_cost.bvwo = Convert.ToInt32(jobcost_wo.OrderNumber);
                        job_cost.origin = "Automatically Added";
                        job_cost.billtypeid = 12;
                        job_cost.save(user, "/sections/workorder/index.aspx.cs - create_workorder_consol #8", false);
                        try
                        {
                            // So up to here, we have the new work order created with the progress bill amount as a line item on the work order.  now we need to adjust the origianl work order.
                            // Find out how many dollars have been billed so far on all progress bills.
                            var sofarbill = Toolbox.doSQL_double(conn, @" SELECT IFNULL(SUM(wo_detail_current_qty_committed * wo_detail_current_price_sell),0) FROM wo_detail_current  WHERE wo_detail_current_master_id = 2139 AND (wo_detail_current_billtypeid = 9 or wo_detail_current_billtypeid = 12) AND wo_detail_current_woprog_id =@v0", new object[] { jobcost_wo.woprog_id });

                        }
                        catch (Exception ex)
                        {
                            throw;
                        }


                    }
                    #endregion

                    //	Clear_all();
                    #region update customer status
                    try
                    {
                        // insert new work order being cut here
                        Toolbox.doSQL_void(conn, @"Insert into customer_history (customer_history_memberID,customer_history_Action,customer_history_custID,customer_history_Date,customer_history_notes) 
Values (@v0,2,@v1, NOW(),@v2)", new object[] { user.id, ex_ddl(ddlCustomer).ToString().Trim(), "WO Cut:" + _wo.OrderNumber + " in " + ddlCompany.Text });

                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
                        return;
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    lblError.Visible = true;
                    lblError.Text = ex.ToString();
                    shared.alert_debug("Error cutting workorder", ex.ToString());
                    return;
                }


                try
                {
                    Toolbox.doSQL_void("Delete from wo_checklist_history where woprog_id = @v0", new object[] { new_woprog_id });
                    var dt_cl = Toolbox.doSQL_dt("Select * from wo_checklist where status = 1 order by checklist_order ", null);
                    foreach (DataRow dr_cl in dt_cl.Rows)
                    {
                        Toolbox.doSQL_void("Insert into wo_checklist_history (wo_checklist_id,woprog_id,dt,complete) values(@v0,@v1,now(),0)", new object[] { dr_cl["id"], new_woprog_id });
                    }
                }
                catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }

                #region if we made it this far, send some emails, print the thing etc
                if (!error_flag)
                {
                    _wo = new NeWOProg(new_woprog_id);
                    var mem = new NeMember(Convert.ToInt32(_wo.intProjectManager));

                    if (chk_inspection_yes.Checked && businessUnit.UsesESACheck)
                    {
                        var branchManager = businessUnit.branch_manager;
                        var purchaser = businessUnit.purchaser;
                        var parameters = new Dictionary<string, object>
                            {
                                { "workOrderId", new_woprog_id },
                                { "workOrderLink", $"{Toolbox.app_setting("Domain")}/sections/workorder/index.aspx?woprog_id={new_woprog_id}&business_unit_id={businessUnit.id}" },
                                { "projectManager", _wo.strProjectManager }
                            };

                        var emailTemplate = AssemblyFileLoader.LoadFile<User, EmbeddedBLLResourceMarker>("WORequiresInspection.html");
                        var email = NESI.Common.Templates.TemplateParser.Parse(emailTemplate, parameters, TemplateParameterFormat.DoubleBrace);
                        var subject = $"WO {new_woprog_id} requires inspection";
                        NeBusinessUnit.EmailBranchPurchaser(businessUnit.id, subject, email, true, mem.NEEmail);
                    }

                    if (user.isContact && new_woprog_id != null && user.Email != null)
                    {
                        var message = new MailMessage();
                        message.From = new MailAddress(Toolbox.app_setting("Email_From"), "Work Orders");
                        if (!string.IsNullOrEmpty(mem.NEEmail))
                        {
                            message.From = new MailAddress(mem.NEEmail);
                            message.CC.Add(mem.NEEmail);
                        }
                        if (user.Email != null)
                        {
                            message.To.Add(user.Email);
                        }
                        else
                        {
                            message.To.Add(mem.NEEmail);
                        }
                        message.Subject = "Confirmation of " + new NeBusinessUnit(_wo.business_unit_id).name + " Work Order: " + _wo.OrderNumber + " creation. ";
                        message.Body = "ID: " + _wo.OrderNumber + Environment.NewLine;
                        message.Body += "Description: " + _wo.Description + Environment.NewLine;
                        message.Body += "Requested Start Date: " + _wo.woprog_Expected_StartDate.ToShortDateString() + Environment.NewLine;
                        message.Body += "Project Manager: " + mem.FullName2 + Environment.NewLine;
                        message.Body += Environment.NewLine;
                        message.Body += "Thank you for your business once again!  Either the project manager or service manager will contact you shortly.";
                        message.Body += Environment.NewLine;
                        // var mail_client = new SmtpClient(Toolbox.app_setting("mx_address"), 587);

                        // mail_client.DeliveryMethod = SmtpDeliveryMethod.Network;

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        var smtp = new SmtpClient(Toolbox.app_setting("mx_address"), int.Parse(Toolbox.app_setting("smtp_port")));
                        smtp.EnableSsl = bool.Parse(Toolbox.app_setting("smtp_enable_ssl"));
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(Toolbox.app_setting("smtp_username"), Toolbox.app_setting("smtp_password"));
                        smtp.Timeout = 1000000;
                        //smtp.Send(m);
                        try
                        {
                            smtp.Send(message);
                            Toolbox.do_debug_note(string.Format("Email sent to: {0} - {1}", message.To, message.Subject));

                        }
                        catch (Exception Exce)
                        {
                            Toolbox.do_errorLog_errorStack(Exce);
                            //Need to Remove
                            shared.alert_debug("Error mail sending WO Creation email", "Email:<br/>" + Toolbox.dict_dump(Toolbox.dict_create(message)) + "<br/> Exception:<br/>" + Exce);
                        }
                    }
                }
                #endregion
                hidWOProgID.Value = new_woprog_id.ToString();
                lblStatus.Text = OpsWOStatus.Open;
                try
                {
                    // var NSI = new NetSuite_Integration.Customer();
                    //  NSI.SyncNetSuite(thisCustomer, businessUnit.tax_entity_id);
                }
                catch (Exception ee)
                {
                    //  Toolbox.do_errorLog_errorStack(ee);
                }

                update_header_totals();
                #region send email if credit card only customer
                if (chk_cc.Checked)
                {
                    var e = new NeEMail();
                    try
                    {
                        if (Toolbox.app_setting("debug_redirect") == "1")
                            e.To = Toolbox.app_setting("debug_redirect_email");
                        else
                            e.To = "ar@" + Toolbox.app_setting("DomainForEmail");

                        e.CC = new NeMember(Convert.ToInt32(_wo.intProjectManager)).NEEmail;
                        e.Subject = "Customer Credit Card Verification for WO " + _wo.OrderNumber + " in " + ddlCompany.Text + " branch";
                        e.From = "admin@" + Toolbox.app_setting("DomainForEmail");
                        e.isHTML = true;
                        e.Body = "Customer: " + _wo.CustomerName + " </br>";
                        e.Body += "Project Manager:" + new NeMember(Convert.ToInt32(_wo.intProjectManager)).FullName + "</br>";
                        e.Body += "Please verify that we have a valid credit card imprint or number on file for this customer.  Also, make sure they know we will be charging their credit card upon completion of the work.";
                        e.Send();
                    }
                    catch (Exception ee) { shared.alert_debug("Error sending AR Credit Card Verification email", "Email:<br/>" + Toolbox.dict_dump(Toolbox.dict_create(e)) + "<br/> Exception:<br/>" + ee); }
                }

                #endregion

                if (_redirect)
                {
                    Response.Redirect(string.Format("./index.aspx?woprog_id={0}&business_unit_id={1}", _wo.woprog_id, _wo.business_unit_id), false);
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
        }

    }
    protected void popReassignReason_CallbackPanel_Callback(object _sender, CallbackEventArgsBase _e)
    {
        if (!string.IsNullOrWhiteSpace(this.ddl_Customer.Text))
        {
            var ddl = this.ddl_Customer.Value;
            populate_ddl_Address((int)ddl, 0);
            populate_ddl_Contact((int)ddl, 0);
        }

        // var term_id = Toolbox.doSQL_int(@"Select  0 as address_id,'Not Applicable' as address union Select address_id,address as address from vw_address where cust_id = @v0 AND active = 1", new object[] { cbo_customer.Value });
        //combo_default_terms.Value = term_id;


        //if (!IsCallback)
        //{
        //    var temp_customer_id = Convert.ToInt32(ex_ddl(ddlCustomer));
        //    var temp_customer = new NECustomer((int)temp_customer_id);
        //    spndayscredit.Value = temp_customer.customer_creditdays;
        //    populate_ddlAddress(temp_customer_id, 0);
        //    populate_ddlquote(0);
        //    populate_ddlContact(temp_customer_id, 0);
        //    pc_main.TabPages[1].ClientEnabled = true;
        //    chkProgress.ClientEnabled = true;
        //    ddlJobCostWO.SelectedIndex = 0;
        //    hdnJobCostTotalQuote.Value = "0";
        //    hdnJobCostWO.Value = "0";
        //    lblAlreadyBilled.Text = "0";
        //}
    }



    protected void fill_from_quote()
    {
        try
        {
            var quote = new quote(Convert.ToInt32(ddlquote.Value));

            var nec = new NECustomer(quote.cust_id);
            if (!nec.Is_QCed)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('This customer has not been QCed, cannot start work order');", true);
                return;
            }

            txtWODescription.Text = quote.txtJobDescription + Environment.NewLine + "as per Q" + quote.QuoteID + " V" + quote.Revision;
            txtSalesValue.Text = quote.Price.ToString();
            ddlCustomer.Value = Convert.ToInt32(quote.cust_id);
            PopulateDataBasedOnCustomer(quote);
            populate_ddlquote(ddlquote.Value);
            ddl_currency.Value = quote.currency == null || quote.currency == 0 ? (new NeBusinessUnit(quote.business_unit_id).country == "USA" ? 1 : 2) : quote.currency;
            populate_ddlAddress(Convert.ToInt32(ddlCustomer.Value), quote.address_id);
            populate_ddlPM(Convert.ToInt32(ddlCustomer.Value), Convert.ToInt32(quote.quoted_by));
            populate_ddlContact(Convert.ToInt32(ddlCustomer.Value), Convert.ToInt32(quote.contact_id));
            ddlCompany.Value = quote.business_unit_id;

            var term_id = Toolbox.doSQL_int(@"Select term_id from quote_term where id=@v0 limit 1", new object[] { quote.net_due });
            this.combo_default_terms.Value = Toolbox.ReturnZeroIfNull_int(term_id);

            if (!string.IsNullOrEmpty(quote.date_expected_start))
            {
                DateTime nowPlus2weeks = DateTime.Now.AddDays(14);
                var quoteStart = Convert.ToDateTime(quote.date_expected_start);
                if (quoteStart > nowPlus2weeks)
                {
                    dteStartDate.Date = quoteStart;
                }
                else
                {
                    dteStartDate.Text = "";
                }
            }

            //populate_parent_company();


            if (_wo.woprog_id == 0)
            {
                try
                {
                    dteExpEndDate.Date = Convert.ToDateTime(quote.completion_date);
                    txtlaborvalue.Text = Convert.ToString(Toolbox.doSQL_double(@"SELECT IFNULL((SELECT
  SUM(qty)
FROM
  quote_worksheet a
  INNER JOIN quote_section b ON a.section_id = b.id
WHERE a.quote_id = @v0
  AND a.revision = @v1
  AND a.part_no >= 990000
  AND a.part_no < 1000000
  AND a.is_checked
  AND b.is_checked),0)
", new object[] { quote.QuoteID, quote.Revision }));

                    lblquoteterms.Text = HttpUtility.HtmlDecode(fill_terms(quote.QuoteID)).Replace("</div><div>", "\n").Replace("<div>", "").Replace("</div>", "");
                }
                catch (Exception ee)
                {
                    Toolbox.do_errorLog_errorStack(ee);
                }
            }
            pc_main.TabPages[WorkOrderTabPage.Quote].Enabled = true;
            ddlJobCostWO.SelectedIndex = 0;
            hdnJobCostTotalQuote.Value = "0";
            hdnJobCostWO.Value = "0";
            chkProgress.ClientEnabled = false;
            tblProgress.ClientVisible = false;
            lblAlreadyBilled.Text = "0";
            hdnQuotedPrice.Value = quote.Price == null ? null : quote.Price.ToString();
            combo_default_invoicetype.SelectedIndex = 0;
            combo_default_invoicetype.ClientEnabled = false;
            var woCustomer = new NECustomer(Convert.ToInt32(ddlCustomer.Value));
            var woBusinessUnit = new NeBusinessUnit(quote.business_unit_id);
            //     var woTaxEntity = new NeTaxEntity(woBusinessUnit.tax_entity_id);
            var woAddress = new NEAddress(Convert.ToInt32(ex_ddl(ddlAddress)));
            var internalCustomerBusinessUnit = woCustomer.IsNEcompany ? new NeBusinessUnit(woCustomer.NEbusiness_unit_id) : new NeBusinessUnit();
            //      var internalCustomerTaxEntity = new NeTaxEntity(internalCustomerBusinessUnit.tax_entity_id);

            spndayscredit.Value = woCustomer.customer_creditdays;
            if (woCustomer.IsNEcompany && internalCustomerBusinessUnit.id != woBusinessUnit.id)
            {
                td_parent_info.Style["display"] = user.isContact ? "none" : "";
            }

            if (woCustomer.IsNEcompany)
            {
                populate_parent_workorder(woCustomer.NEbusiness_unit_id);
                ddl_parent_workorder.Value = ddl_parent_workorder.Value == null ? 0 : ddl_parent_workorder.Value;
            }
        }
        catch (Exception ex1)
        {
            lblError.Text = ex1.Message;
            lblError.ClientVisible = true;
            return;
        }
    }
    protected void Clear_all()
    {
        hidWOProgID.Value = "0";
        var tools = new Toolbox();
        _wo = new NeWOProg();
        var business_unit_id = hidCompanyID.Value;
        _wo.business_unit_id = Convert.ToInt32(business_unit_id);
        _wo.QuoteID = "0";
        //ddlCompany.DataSource = Toolbox.doSQL_dt(@"Select * from vw_active_business_units"  , null);
        //ddlCompany.DataBind();
        ddlCompany.Value = Convert.ToInt32(business_unit_id);
        //populate_parent_company();

        populate_ddlCustomer(Convert.ToInt32(business_unit_id), _wo.WOProg_Customer_ID);
        if (user.business_unit_id.ToString() == business_unit_id)
        {
            populate_ddlPM(Convert.ToInt32(business_unit_id), user.id);

        }
        else
        {
            populate_ddlPM(Convert.ToInt32(business_unit_id), 0);

        }

        populate_ddlquote(0);
        populate_ddlJobCostWO(Convert.ToInt32(business_unit_id), Convert.ToInt32(_wo.WOProg_Customer_ID), 0);

        txtWODescription.Text = tools.value_from(_wo.Description, false);
        txtAreainPlant.Text = _wo.woprog_Location_in_plant;
        txtSpecialInstruction.Text = _wo.special_instructions;
        chk_cc.Checked = Convert.ToBoolean(_wo.woprog_creditcard_payment);
        chkRD.Checked = _wo.chkRD;
        ChkServiceCall.Checked = _wo.chkServiceCall;
        //		txtSalesValue.Text = wo.woprog_expected_sales_value.ToString();
        txtCustPO.Text = _wo.PONumber;
        dteExpEndDate.Date = _wo.woprog_Expected_EndDate;

        //dteStartDate.Date = DateTime.Today.AddDays(0);
        // Add min dates except privilege 193
        if (!SkipDateRestrictions)
        {
            dteStartDate.MinDate = DateTime.Today.AddDays(-7);
            dteExpEndDate.MinDate = DateTime.Today.AddDays(0);
        }


        txtwhyhold.Text = _wo.woprog_whyhold;
        chkOnHold.Checked = false;
        var x = 1;
        while (x < pc_main.TabPages.Count)
        {
            pc_main.TabPages[x].Enabled = false;
            x++;
        }

        ddlJobCostWO.Text = "";
        //     ddlJobCostWO.SelectedIndex = 0;
        hdnJobCostTotalQuote.Value = "0";
        hdnJobCostWO.Value = "0";
        chkProgress.Checked = false;
        tblProgress.ClientVisible = false;
        lblAlreadyBilled.Text = "";
        lbljobcost_quote_details.Text = "";
        chkProgressBillCredit.Checked = false;
        chkDownPayment.Checked = false;
        txtpb_amt.Text = "0";
        hidWOStatus.Value = "new";
        dte_cutDate.Enabled = false;
        spndayscredit.Value = 0;
        radiotype.Value = 0;
        ddl_creditwolink.Value = null;
        dte_cutDate.Text = null;
        chk_warranty.Checked = false;
        chk_labor_only.Checked = false;
        chk_mat_only.Checked = false;
        chk_sub_only.Checked = false;
        chkInvoiceFeedbackIssues.Checked = false;

    }
    protected void ProgressAssociated_CheckedChanged(object _sender, EventArgs _e)
    {
        var cb = (ASPxCheckBox)_sender;
        switch (cb.ID)
        {
            case "chkProgress":
                tblProgress.ClientVisible = false;
                if (cb.Checked)
                {
                    pc_main.TabPages[WorkOrderTabPage.Quote].Enabled = false;
                    try
                    {
                        ddlquote.SelectedIndex = -1;
                        ddlquote.ClientEnabled = false;

                    }
                    catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
                    if (ddlCustomer.SelectedIndex > -1)
                    {
                        if (_wo.GetOpenWorkOrders(Convert.ToInt32(ddlCompany.Value), Convert.ToInt32(ddlCustomer.Value), true).Rows.Count == 0)
                        {
                            cb.Checked = false;
                            ddlquote.SelectedIndex = -1;
                            ddlquote.ClientEnabled = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('This customer has no available job cost work orders to progress bill.');", true);
                            return;
                        }
                        if (hidWOProgID.Value != "0" && hidWOProgID.Value != "")
                        {
                            populate_ddlJobCostWO(Convert.ToInt32(ddlCompany.Value), Convert.ToInt32(ddlCustomer.Value), _wo.woprog_associate_woprog_id);
                        }
                        else
                        {
                            populate_ddlJobCostWO(Convert.ToInt32(ddlCompany.Value), Convert.ToInt32(ddlCustomer.Value), 1);
                        }
                        dteExpEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                        dteStartDate.Text = ""; //DateTime.Today.ToString("yyyy-MM-dd");
                        tblProgress.ClientVisible = true;
                        hdnJobCostWO.Value = "";
                        hdnJobCostTotalQuote.Value = "";
                        div_jobcost_quote_link.InnerHtml = "";
                        ddlJobCostWO.SelectedIndex = -1;
                        chkDownPayment.Checked = false;
                        ddlJobCostWO.ClientEnabled = true;
                        txtpb_amt.Text = null;
                        lblAlreadyBilled.Text = "";
                        lbljobcost_quote_details.Text = "";
                        txtWODescription.Text = "";
                    }
                    else
                    {
                        cb.Checked = false;
                        lblError.Text = "Please Select a Customer";
                        lblError.ClientVisible = true;
                    }
                }
                else
                {
                    dteExpEndDate.Text = "";
                    dteStartDate.Text = "";
                    ddlJobCostWO.DataSource = "";
                    ddlJobCostWO.DataBind();
                    hdnJobCostWO.Value = "";
                    hdnJobCostTotalQuote.Value = "";
                    btn_printlaser.ClientEnabled = true;
                    btn_printdot.ClientEnabled = true;
                    ddlquote.ClientEnabled = true;
                    chkDownPayment.Checked = false;
                    chkProgressBillCredit.Checked = false;
                    lblAlreadyBilled.Text = "";
                    lbljobcost_quote_details.Text = "";
                    ddlJobCostWO.SelectedIndex = -1;
                    txtpb_amt.Text = null;
                    txtWODescription.Text = "";
                }

                // Task 1459: Parent WO drop down visible on internal Progress Bills
                ShowOrHideParentWorkOrderDropDownBasedOnProgressBillCheckbox();

                break;
        }
    }

    protected void ddl_creditwolink_SelectedIndexChanged(object _sender, EventArgs _e)
    {
        ASPxComboBox cb = (ASPxComboBox)_sender;
        if (!string.IsNullOrWhiteSpace(cb.Text))
        {
            NeWOProg OriginalWO = new NeWOProg(Convert.ToInt32(cb.Value));

            ddlRevenueLines.Value = OriginalWO.revenue_line_id;
        }

    }

    protected void ddlJobCostWO_SelectedIndexChanged(object _sender, EventArgs _e)
    {
        if (!IsCallback)
        {
            if (ddlJobCostWO.Text != "")
            {
                load_associatedwo_details();

            }
        }

        // Task 1459: Parent WO drop down visible on internal Progress Bills
        ShowOrHideParentWorkOrderDropDownBasedOnProgressBillCheckbox();
    }
    protected void load_associatedwo_details()
    {
        try
        {
            lblError.Text = "";
            var wo = new NeWOProg(Convert.ToInt32(ddlJobCostWO.Value));
            hdnJobCostWO.Value = wo.woprog_id.ToString();
            hdnJobCostTotalQuote.Value = wo.QuotedPrice;

            var RevLine = ddlRevenueLines.Items.FindByText("Services - T&M").Value;

            if (RevLine != null)
                ddlRevenueLines.Value = RevLine;

            //ddlRevenueLines.Value =  3;
            populate_ddlContact(wo.WOProg_Customer_ID, wo.woprog_Contact_ID);

            populate_ddlPM(wo.business_unit_id, wo.intProjectManager);
            var quote = new quote(Convert.ToInt32(wo.QuoteID));
            this.combo_default_terms.Value = Convert.ToInt32(wo.term_id);
            // Check for zero quoted amounts
            double quoted_amount = quote.Price;
            if (quoted_amount == 0)
            {
                lblAlreadyBilled.Text = "";
                lbljobcost_quote_details.Text = "";
                div_jobcost_quote_link.InnerHtml = "";
                ddlJobCostWO.SelectedIndex = -1;
                //  throw new Exception("This WO's associated quote price is zero... cannot proceed. Please check that the correct quote is associated with the job cost WO.");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('This WO's associated quote price is zero... cannot proceed. Please check that the correct quote is associated with the job cost WO.');", true);
                return;
            }

            //	txt_pb_margin.Text = _quote.margin.ToString("p2");
            var sofarbilled = Toolbox.doSQL_double(@"Select ifnull(sum((wo_detail_current_qty_committed)*wo_detail_current_price_sell),0) from wo_detail_current  where wo_detail_current_master_id = 2139 and (wo_detail_current_billtypeid = 9 or wo_detail_current_billtypeid = 12) and wo_detail_current_woprog_id =@v0", new object[] { ddlJobCostWO.Value });
            double percbilled = 0;
            try
            {
                percbilled = sofarbilled / Convert.ToDouble(quote.Price);
            }
            catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
            lblAlreadyBilled.Text = percbilled.ToString("P1") + " or " + sofarbilled.ToString("C2");
            lbljobcost_quote_details.Text = quote.QuoteID + " V" + quote.Revision + Environment.NewLine + "Quoted Price: " + Convert.ToDouble(quote.Price).ToString("C2");

            div_jobcost_quote_link.InnerHtml = @"<a href='javascript:boing(""index.aspx?woprog_id=" + wo.woprog_id + @"&business_unit_id=" + wo.business_unit_id + @""", true, ""JobCost_WO"", 900, 900);'>" + wo.OrderNumber + "- (Q" + wo.QuoteID.Substring(0, 6) + " V" + wo.QuoteID.Substring(6, 1) + ")</a>";

            try
            {
                var lefttobill = Convert.ToDouble(quote.Price) - sofarbilled;
                //		txtSalesValue.Text = Convert.ToString(Math.Round(lefttobill, 2));
                txtpb_amt.Text = "0";
                if (chkDownPayment.Checked)
                {
                    txtWODescription.Text = "Down Payment for Quote # " + quote.QuoteID + " payment is due on receipt prior to start of job.";
                    spndayscredit.Value = 0;
                }
                else
                {
                    txtWODescription.Text = "Progress Billing for Quote # " + quote.QuoteID;
                }
                if (wo.PONumber != "0" && wo.PONumber != "")
                {
                    txtCustPO.Text = wo.PONumber;
                }
                if (wo.currency_id != null)
                {
                    ddl_currency.Value = wo.currency_id;
                }
            }
            catch (Exception ee)
            {
                lblError.Text = ee.Message;
                return;
            }
        }
        catch (Exception ee)
        {
            lblError.Text = ee.Message;
            return;
        }
    }
    protected void load_associatedwo_details_fromwo()
    {
        try
        {
            tblProgress.ClientVisible = true;
            var wo = new NeWOProg(Convert.ToInt32(ddlJobCostWO.Value));
            var quote = new quote(Convert.ToInt32(wo.QuoteID));
            var sofarbilled = Math.Abs(Toolbox.doSQL_double(@"Select ifnull(sum(wo_detail_current_qty_committed*wo_detail_current_price_sell),0) from wo_detail_current  where wo_detail_current_master_id = 2139 and (wo_detail_current_billtypeid = 12 or wo_detail_current_billtypeid=9) and wo_detail_current_woprog_id =@v0", new object[] { ddlJobCostWO.Value }));
            var dollars_bill_on_this_wo = Math.Abs(Toolbox.doSQL_double(@"Select ifnull(sum(wo_detail_current_qty_committed*wo_detail_current_price_sell),0) from wo_detail_current  where wo_detail_current_master_id = 2139 and (wo_detail_current_billtypeid = 3 or wo_detail_current_billtypeid=11) and wo_detail_current_woprog_id =@v0", new object[] { this._wo.woprog_id }));

            double percbilled = 0;
            try
            {
                percbilled = sofarbilled / Convert.ToDouble(quote.Price);
            }
            catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
            //	txt_pb_margin.Text = _quote.margin.ToString("p2");
            lblAlreadyBilled.Text = percbilled.ToString("P2") + " or " + sofarbilled.ToString("C2");
            lbljobcost_quote_details.Text = quote.QuoteID + " V" + " Quoted Price: " + Convert.ToDouble(quote.Price).ToString("C2");

            div_jobcost_quote_link.InnerHtml = @"<a href='javascript:boing(""index.aspx?woprog_id=" + wo.woprog_id + @"&business_unit_id=" + wo.business_unit_id + @""", true, ""JobCost_WO"", 900, 900);'>" + wo.OrderNumber + "- (Q" + wo.QuoteID.Substring(0, 6) + " V" + wo.QuoteID.Substring(6, 1) + ")</a>";
            ddlJobCostWO.ClientVisible = false;

            try
            {
                var lefttobill = Convert.ToDouble(quote.Price) - sofarbilled;
                txtpb_amt.Text = dollars_bill_on_this_wo.ToString("C2") + " or " + (dollars_bill_on_this_wo / Convert.ToDouble(quote.Price)).ToString("P2");


                //			txtSalesValue.Text = Convert.ToString(Math.Round(lefttobill, 2));
            }
            catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
        }
        catch
        {
            tblProgress.ClientVisible = false;
        }
    }
    protected void ddlCompany_SelectedIndexChanged(object _sender, EventArgs _e)
    {
        if (!IsCallback)
        {
            hidCompanyID.Value = ex_ddl(ddlCompany).ToString();
            populate_ddlCustomer(Convert.ToInt32(ddlCompany.Value), 0);
            populate_ddlPM(Convert.ToInt32(ddlCompany.Value), 0);
            //populate_locations(true);
            populate_ddlquote(0);
            ddlAddress.Text = "";
            ddlContact.Text = "";
            ddlContact.SelectedIndex = 0;
            ddlContact.Text = "";
            if (ddlAddress.Items.Count <= 1)
            {
                ddlAddress.SelectedIndex = 0;
                ddlAddress.Text = "";
            }
            else
            {
                ddlAddress.SelectedIndex = -1;
                ddlAddress.Text = "";
            }
            chkProgress.Checked = false;
            tblProgress.ClientVisible = false;
        }
    }
    protected void btnAddNew_Click(object _sender, EventArgs _e)
    {
        Clear_all();
    }
    protected void chkRD_CheckedChanged(object _sender, EventArgs _e)
    {
        pc_main.TabPages[WorkOrderTabPage.ResearchAndDevelopment].Enabled = chkRD.Checked;
        pc_main.TabPages[WorkOrderTabPage.ResearchAndDevelopment].ClientEnabled = chkRD.Checked;
    }

    protected void chkInvoiceFeedbackIssues_CheckedChanged(object _sender, EventArgs _e)
    {
        var q = Request.QueryString;
        using (var conn = Toolbox.connect())
        {
            Toolbox.doSQL_void(conn, @"UPDATE woprog SET invoice_fb_issues = @v0 WHERE woprog_id = @v1  LIMIT 1", new object[] { chkInvoiceFeedbackIssues.Checked, q["woprog_id"] });

        }
    }

    private void printDoc_PrintPage(object _sender, PrintPageEventArgs _e)
    {
        //string customer_id = Context.Items["customer_id"].ToString();
        NeWOProg work_order;
        work_order = new NeWOProg(Convert.ToUInt16(ex_ddl(ddlCompany)), order_number);
        var cust_info = new NECustomer(Convert.ToInt32(work_order.WOProg_Customer_ID));
        //NEAddress CustAddress = new NEAddress(Convert.ToInt32(CustInfo.Customer_ID), "Customer");
        var cust_address = new NEAddress(work_order.woprog_Address_ID);
        // Customer
        var barcodefont = new System.Drawing.Font("Free 3 of 9", 36);
        var print_font = new System.Drawing.Font("Courier New", 10);
        _e.Graphics.DrawString(cust_info.Customer_Name.PadRight(26).Substring(0, 26), print_font, Brushes.Black, 320, 0);
        _e.Graphics.DrawString(cust_address.Addr1.PadRight(26).Substring(0, 26), print_font, Brushes.Black, 320, 15);
        _e.Graphics.DrawString(cust_address.Addr2.PadRight(26).Substring(0, 26), print_font, Brushes.Black, 320, 30);
        _e.Graphics.DrawString(cust_address.Addr3.PadRight(26).Substring(0, 26), print_font, Brushes.Black, 320, 45);
        var tempstring = cust_address.City + ", " + cust_address.Prov;
        _e.Graphics.DrawString(tempstring.PadRight(26).Substring(0, 26), print_font, Brushes.Black, 320, 60);
        tempstring = "(" + cust_address.PhoneArea + ") " + cust_address.Phonefirst + " " + cust_address.PhoneLast;
        _e.Graphics.DrawString(tempstring, print_font, Brushes.Black, 320, 75);
        string[] desclines;
        desclines = work_order.Description.Replace("\r\n", "~").Split('~');
        var x = 0;
        double y = 0;
        var last_row = 190;
        while (x < desclines.Length)
        {
            y = 0;
            while (y <= desclines[x].Length / 40.0)
            {
                _e.Graphics.DrawString(desclines[x].PadRight(500).Substring(Convert.ToInt16(y * 40), 40), print_font, Brushes.Black, 55, last_row);
                last_row += 15;
                y++;
            }
            x++;
        }
        last_row = last_row + 15;
        if (work_order.special_instructions != "")
        {
            string[] instructions;
            instructions = work_order.special_instructions.Replace("\r\n", "~").Split('~');
            x = 0;
            while (x < instructions.Length)
            {
                y = 0;
                while (y < instructions[x].Length / 40.0)
                {
                    _e.Graphics.DrawString(instructions[x].PadRight(500).Substring(Convert.ToInt16(y * 40), 40), print_font, Brushes.Black, 55, last_row);
                    last_row += 15;
                    y++;
                }
                x++;
            }
        }
        last_row = last_row + 15;
        if (work_order.woprog_Location_in_plant != null)
        {
            _e.Graphics.DrawString(work_order.woprog_Location_in_plant.PadRight(250).Substring(0, 100), print_font, Brushes.Black, 55, last_row);
            last_row += 15;
        }
        print_font = new System.Drawing.Font("Courier New", 8);
        _e.Graphics.DrawString("Order No: ", print_font, Brushes.Black, 580, 0);
        _e.Graphics.DrawString("Date: ", print_font, Brushes.Black, 580, 15);
        _e.Graphics.DrawString("Cust PO: ", print_font, Brushes.Black, 580, 30);
        _e.Graphics.DrawString("P Manager: ", print_font, Brushes.Black, 580, 45);
        _e.Graphics.DrawString("Dept: ", print_font, Brushes.Black, 580, 60);
        _e.Graphics.DrawString("Contact: ", print_font, Brushes.Black, 580, 75);
        _e.Graphics.DrawString("Start Date: ", print_font, Brushes.Black, 580, 90);
        _e.Graphics.DrawString("End Date: ", print_font, Brushes.Black, 580, 105);
        _e.Graphics.DrawString("Quote: ", print_font, Brushes.Black, 580, 120);
        _e.Graphics.DrawString("*002-" + work_order.woprog_id + "*", barcodefont, Brushes.Black, 520, 135);
        print_font = new System.Drawing.Font("Courier New", 9);
        _e.Graphics.DrawString(order_number, print_font, Brushes.Black, 680, 0);
        print_font = new System.Drawing.Font("Courier New", 8);
        _e.Graphics.DrawString(DateTime.Now.ToShortDateString(), print_font, Brushes.Black, 680, 15);
        _e.Graphics.DrawString(work_order.PONumber.Trim(), print_font, Brushes.Black, 680, 30);
        _e.Graphics.DrawString(work_order.strProjectManager.PadRight(15).Substring(0, 15), print_font, Brushes.Black, 680, 45);


        var tempcontact = new NEContact(Convert.ToInt32(work_order.woprog_Contact_ID));
        _e.Graphics.DrawString(tempcontact.Contact_Name.PadRight(15).Substring(0, 15), print_font, Brushes.Black, 680, 75);
        last_row = 90;
        if (work_order.woprog_Expected_StartDate != null)
        {
            var str_start_date = "";
            try
            {
                str_start_date = Convert.ToDateTime(work_order.woprog_Expected_StartDate).ToString("yyyy-MM-dd");
            }
            catch
            {
                str_start_date = work_order.woprog_Expected_StartDate.ToString();
            }
            if (work_order.woprog_Expected_StartDate.ToShortDateString() != "")
            {
                _e.Graphics.DrawString(str_start_date.PadRight(60).Substring(0, 60), print_font, Brushes.Black, 680, last_row);
                last_row += 15;
            }
        }
        if (work_order.woprog_Expected_EndDate != null)
        {
            var str_end_date = "";
            try
            {
                str_end_date = Convert.ToDateTime(work_order.woprog_Expected_EndDate).ToString("yyyy-MM-dd");
            }
            catch
            {
                str_end_date = work_order.woprog_Expected_EndDate.ToString();
            }
            if (work_order.woprog_Expected_EndDate.ToShortDateString() != "")
            {
                _e.Graphics.DrawString(str_end_date.PadRight(60).Substring(0, 60), print_font, Brushes.Black, 680, last_row);
                last_row += 15;
            }
        }
        if (work_order.QuoteID != "0")
        {
            var quote = new quote();
            var quoterev = quote.GetQuoteRev(Convert.ToInt32(work_order.QuoteID));
            if (quoterev.Length > 6)
            {
                quoterev = quoterev.Substring(0, 6) + "-" + quoterev.Substring(6, 1);
            }
            _e.Graphics.DrawString("Q-" + quoterev.PadRight(60).Substring(0, 60), print_font, Brushes.Black, 680, last_row + 15);
        }
        print_font = new System.Drawing.Font("Courier New", 10);
        // commented out by andy k    e.Graphics.DrawString(OrderNumber.ToString(), printFont, Brushes.Black, 200, 60);
    }
    protected void create_pdf(NeWOProg _pdf_progress, string _str_comp_id, int _pdf_woid, string _ord_num, string _type)
    {
        var business_unit_id = Convert.ToInt32(_str_comp_id);
        var fileServer = NeTaxEntity.BaseFolder(business_unit_id, false);
        var company = new NeBusinessUnit(business_unit_id);
        var doc = new Document(PageSize.LETTER, 40, 40, 42, 35);
        var strwoid = _pdf_woid.ToString();
        var str_name = company.name;
        var wonum = _pdf_progress.OrderNumber;
        var customername = _pdf_progress.CustomerName;
        var str_date = DateTime.Now.ToString("yyyy-MM-dd");
        var str_head = "";
        var str_body = "";
        if (_type == "PB")
        {

            var quote = new quote();
            var quoterev = quote.GetQuoteRev(Convert.ToInt32(_pdf_progress.QuoteID));
            var str_quote_value = _pdf_progress.QuotedPrice;
            if (chkDownPayment.Checked)
            {
                str_head = str_name + " DOWN PAYMENT WO:" + _ord_num;
            }
            else
            {
                str_head = str_name + " Progress Billing WO:" + _ord_num;
            }
            if (chkDownPayment.Checked)
            {
                str_body = "This is a Down Payment made on Work Order " + wonum + " ";
                str_body += "for " + customername + " for " + txtpb_amt.Text + " of " + str_quote_value + " fromthe Quote Q-" + quoterev + " on " + str_date + ".";
            }
            else
            {
                str_body = "This is a Progress Billing made on Work Order " + wonum + " ";
                str_body += "for " + customername + " for " + txtpb_amt.Text + " of " + str_quote_value + " fromthe Quote Q-" + quoterev + " on " + str_date + ".";
            }
        }
        else
        {
            if (_type == "Credit")
            {
                str_head = str_name + " Credit WO:" + wonum + " on WO: " + _ord_num;
            }
            else
            {
                str_head = str_name + " Rebill WO:" + wonum + " on WO: " + _ord_num;
            }
            if (_type == "Credit")
            {
                str_body = "This is a Credit made for Work Order " + wonum + " ";
                str_body += "for " + customername + " on " + str_date + ".";
                str_body += "This new credit WO number is :" + _ord_num + ".";
            }
            else
            {
                str_body = "This is a Rebill of Work Order " + wonum + " ";
                str_body += "for " + customername + " on " + str_date + ".";
                str_body += "This new Rebill WO number is :" + _ord_num + ".";
            }
        }
        try
        {
            var wri = PdfWriter.GetInstance(doc, new FileStream(fileServer + @"\WOs\" + strwoid + ".pdf", FileMode.Create));
            doc.Open();
            var par_header = new Paragraph(str_head, FontFactory.GetFont(FontFactory.COURIER_BOLD, 20));
            par_header.Alignment = Element.ALIGN_CENTER;
            var par_space = new Paragraph(" ");
            var par_body = new Paragraph(str_body, FontFactory.GetFont(FontFactory.COURIER));
            doc.Add(par_header);
            doc.Add(par_space);
            doc.Add(par_body);
        }
        catch (DocumentException dex)
        {
            //Handle document exception
            Toolbox.do_errorLog_errorStack(dex);
        }
        catch (IOException ioex)
        {
            //Handle IO exception
            Toolbox.do_errorLog_errorStack(ioex);
        }
        catch (Exception ee)
        {
            //Handle Other Exception
            Toolbox.do_errorLog_errorStack(ee);
        }
        finally
        {
            doc.Close(); //Close document
        }
        //  var str_path = company.WOPath;
        //  File.Copy(fileServer + @"\WOs\" + strwoid + ".pdf", str_path + @"WorkPro\" + strwoid + ".pdf");
    }
    protected void chkDownPayment_CheckedChanged(object _sender, EventArgs _e)
    {
        var wo_prog_id = ex_ddl(ddlJobCostWO).ToString();
        if (wo_prog_id != "0")
        {
            var int_woid = Convert.ToInt32(wo_prog_id);
            var prog_dp = new NeWOProg(int_woid);
            //            lblPPInfo.Visible = true;
            //            lblPPInfo.Text = progDP.GetWOInformation(woProgID, chkDownPayment.Checked, txtQtPerc.Text);
            txtWODescription.Text = prog_dp.Description;
            if (prog_dp.PONumber != "0" && prog_dp.PONumber != "")
            {
                txtCustPO.Text = prog_dp.PONumber;
            }
        }
        else
        {
            //            lblPPInfo.Visible = false;
            //            lblPPInfo.Text = "";
            //            txtDescription.Text = "";
        }
        if (chkDownPayment.Checked)
        {
            spndayscredit.Value = 0;
            combo_default_terms.Value = OpsStaticTerms.DueOnReceipt;
        }

        ShowOrHideParentWorkOrderDropDownBasedOnProgressBillCheckbox();
    }
    protected void log_error(string _title, Exception _ex)
    {
        var error_message = _title + _ex.Message + "<br />";
        lblError.Text = error_message;
        try
        {
            UpdatePanel1.Update();
        }
        catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
        lblError.ClientVisible = true;
        Toolbox.doSQL_void(@"INSERT INTO Error (Error_Member_ID,Error_DSN,Error_Desc,Error_DateTime) VALUES (@v0 ,@v1 ,@v2 ,NOW()) ", new object[] { user.id, hidCompanyDSN, error_message });
        Toolbox.do_errorLog(_ex, _title);
        return;
    }


    private void quote_unattach(MySqlConnection conn, string _was_quote, string _is_quote, NeWOProg WO)
    {
        if (_was_quote == "0") return;
        int is_rev, is_quote_id, was_rev, was_quote_id;
        var quote_number = Convert.ToInt32(_was_quote);
        quote.splice(_was_quote, out was_quote_id, out was_rev);
        quote.splice(quote_number, out is_quote_id, out is_rev);
        quote.unattach(is_quote_id, is_rev);

        //Remove Quote Line If Exists
        var quote_lines = Toolbox.doSQL_dt(conn, @"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = 2139 AND wo_detail_current_type = 'Q' AND wo_detail_current_billtypeid IN (3,11)", new object[] { WO.woprog_id });
        if (quote_lines.Rows.Count > 1)
        {
            //  throw new Exception("There are multiple quote lines on this work order, this should never happen.");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('There are multiple quote lines on this work order, this should never happen.');", true);
            return;
        }
        else
        {
            Toolbox.doSQL_void(conn, @"DELETE FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  and wo_detail_current_master_id = 2139 AND wo_detail_current_type = 'Q' AND wo_detail_current_billtypeid IN (3,11)", new object[] { WO.woprog_id });
        }
        //Update quoted hours total on this work order
        Toolbox.doSQL_void(conn, @"Update woprog SET WOProg_QuotedHoursTotal = 0, woprog_ts = woprog_ts WHERE woprog_id = @v0", new object[] { WO.woprog_id });
        //check the parent WO too
        if (WO.parent_woprog_id != 0)
        {

            Toolbox.doSQL_void(conn, @"DELETE FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0 and wo_detail_current_description like '%quote%' and wo_detail_current_code = @v1   and wo_detail_current_master_id IN (55555, 55556, 55557, 55558, 55559) AND wo_detail_current_type = 'M'", new object[] { WO.parent_woprog_id, _was_quote });

            //Need to add the labor back on 

            var allLaborLinesOnChild = Toolbox.doSQL_dt(
                @"SELECT a.wo_detail_current_id,a.wo_detail_current_qty_committed,a.wo_detail_current_qty_ordered,a.wo_detail_current_qty_invoiced, a.memberid, b.paytype_id 
FROM wo_detail_current a
JOIN membertype_chargeout b ON a.wo_detail_current_master_id = b.id
WHERE a.wo_detail_current_woprog_id = @v0 
AND a.wo_detail_current_type = 'L'", new object[] { WO.woprog_id });

            foreach (DataRow woDetailObj in allLaborLinesOnChild.Rows)
            {

                Toolbox.doSQL_void(@"UPDATE wo_detail_current a  
JOIN membertype_chargeout b ON a.wo_detail_current_master_id = b.id
SET a.wo_detail_current_qty_committed = (a.wo_detail_current_qty_committed + @v1),
a.wo_detail_current_qty_ordered = (a.wo_detail_current_qty_ordered + @v2),
a.wo_detail_current_qty_invoiced = (a.wo_detail_current_qty_invoiced + @v3)
WHERE a.wo_detail_current_woprog_id = @v0 
AND a.wo_detail_current_type = 'L' AND a.memberid = @v4 AND b.paytype_id = @v5", new object[] { WO.parent_woprog_id, woDetailObj["wo_detail_current_qty_committed"],
                    woDetailObj["wo_detail_current_qty_ordered"], woDetailObj["wo_detail_current_qty_invoiced"], woDetailObj["memberid"], woDetailObj["paytype_id"] });
            }
            WO.fast_update_header_totals();

            new NeWOProg(WO.parent_woprog_id).fast_update_header_totals();

        }
        // Move all other items that are job cost billtype to regular billtype
        var dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id != 2139", new object[] { WO.woprog_id });
        foreach (DataRow dr in dt.Rows)
        {
            var consignment_id = Convert.ToInt32(dr["wo_detail_current_consignment_id"]);
            var origin = dr["wo_detail_current_origin"].ToString();
            if (consignment_id != 0 && !origin.Contains("Expense Reimbursement") && !origin.Contains("Per Diem Expense") && !origin.Contains("Company Credit Card Expense"))
            {
                var consign = new consignment(consignment_id);
                consign.load();
                consign.status = consignment.StatusType.Quoted;
                consign.save();
            }
            Toolbox.doSQL_void(conn, @"UPDATE wo_detail_current SET wo_detail_current_billtypeid = '0' WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_billtypeid = '1'", new object[] { WO.woprog_id });
        }
        NeWODetailCurrent.reorder_lines(WO.woprog_id);

        //Update Quote History
        var pmmember = new NeMember(Convert.ToInt32(ex_ddl(ddlPM)));
        quote.add_history(conn, user.id32, was_quote_id, was_rev, string.Format("Unreceived - WO:{0} PM:{1} StartDate: {2} EndDate: {3}", order_number, pmmember.FullName, dteStartDate.Text, dteExpEndDate.Text));
    }

    private void quote_unattach(MySqlConnection conn, string _was_quote, string _is_quote)
    {
        if (_was_quote == "0") return;
        int is_rev, is_quote_id, was_rev, was_quote_id;
        var quote_number = Convert.ToInt32(_was_quote);
        quote.splice(_was_quote, out was_quote_id, out was_rev);
        quote.splice(quote_number, out is_quote_id, out is_rev);
        quote.unattach(is_quote_id, is_rev);

        //Remove Quote Line If Exists
        var quote_lines = Toolbox.doSQL_dt(conn, @"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = 2139 AND wo_detail_current_type = 'Q' AND wo_detail_current_billtypeid IN (3,11)", new object[] { _wo.woprog_id });
        if (quote_lines.Rows.Count > 1)
        {
            //throw new Exception("There are multiple quote lines on this work order, this should never happen.");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('There are multiple quote lines on this work order, this should never happen.');", true);
            return;
        }
        else
        {
            Toolbox.doSQL_void(conn, @"DELETE FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  and wo_detail_current_master_id = 2139 AND wo_detail_current_type = 'Q' AND wo_detail_current_billtypeid IN (3,11)", new object[] { _wo.woprog_id });
        }
        //Update quoted hours total on this work order
        Toolbox.doSQL_void(conn, @"Update woprog SET WOProg_QuotedHoursTotal = 0, woprog_ts = woprog_ts WHERE woprog_id = @v0", new object[] { _wo.woprog_id });
        //check the parent WO too
        if (_wo.parent_woprog_id != 0)
        {

            Toolbox.doSQL_void(conn, @"DELETE FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0 and wo_detail_current_description like '%quote%' and wo_detail_current_code = @v1   and wo_detail_current_master_id IN (55555, 55556, 55557, 55558, 55559) AND wo_detail_current_type = 'M'", new object[] { _wo.parent_woprog_id, _was_quote });

            //Need to add the labor back on 

            var allLaborLinesOnChild = Toolbox.doSQL_dt(
                @"SELECT a.wo_detail_current_id,a.wo_detail_current_qty_committed,a.wo_detail_current_qty_ordered,a.wo_detail_current_qty_invoiced, a.memberid, b.paytype_id 
FROM wo_detail_current a
JOIN membertype_chargeout b ON a.wo_detail_current_master_id = b.id
WHERE a.wo_detail_current_woprog_id = @v0 
AND a.wo_detail_current_type = 'L'", new object[] { _wo.woprog_id });

            foreach (DataRow woDetailObj in allLaborLinesOnChild.Rows)
            {

                Toolbox.doSQL_void(@"UPDATE wo_detail_current a  
JOIN membertype_chargeout b ON a.wo_detail_current_master_id = b.id
SET a.wo_detail_current_qty_committed = (a.wo_detail_current_qty_committed + @v1),
a.wo_detail_current_qty_ordered = (a.wo_detail_current_qty_ordered + @v2),
a.wo_detail_current_qty_invoiced = (a.wo_detail_current_qty_invoiced + @v3)
WHERE a.wo_detail_current_woprog_id = @v0 
AND a.wo_detail_current_type = 'L' AND a.memberid = @v4 AND b.paytype_id = @v5", new object[] { _wo.parent_woprog_id, woDetailObj["wo_detail_current_qty_committed"],
                    woDetailObj["wo_detail_current_qty_ordered"], woDetailObj["wo_detail_current_qty_invoiced"], woDetailObj["memberid"], woDetailObj["paytype_id"] });
            }
            _wo.fast_update_header_totals();

            new NeWOProg(_wo.parent_woprog_id).fast_update_header_totals();

        }
        // Move all other items that are job cost billtype to regular billtype
        var dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id != 2139", new object[] { _wo.woprog_id });
        foreach (DataRow dr in dt.Rows)
        {
            var consignment_id = Convert.ToInt32(dr["wo_detail_current_consignment_id"]);
            var origin = dr["wo_detail_current_origin"].ToString();
            if (consignment_id != 0 && !origin.Contains("Expense Reimbursement") && !origin.Contains("Per Diem Expense") && !origin.Contains("Company Credit Card Expense"))
            {
                var consign = new consignment(consignment_id);
                consign.load();
                consign.status = consignment.StatusType.Quoted;
                consign.save();
            }
            Toolbox.doSQL_void(conn, @"UPDATE wo_detail_current SET wo_detail_current_billtypeid = '0' WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_billtypeid = '1'", new object[] { _wo.woprog_id });
        }
        NeWODetailCurrent.reorder_lines(_wo.woprog_id);

        //Update Quote History
        var pmmember = new NeMember(Convert.ToInt32(ex_ddl(ddlPM)));
        quote.add_history(conn, user.id32, was_quote_id, was_rev, string.Format("Unreceived - WO:{0} PM:{1} StartDate: {2} EndDate: {3}", order_number, pmmember.FullName, dteStartDate.Text, dteExpEndDate.Text));
    }


    protected void set_tabs()
    {// all tabs are set to enabled, and client visible false by default.
        /* tab indexes:
        0- General
        1- Customer
        2- Line Items
        3- Quote
        4- POs
        5- Time Entries
        6- History
        7- Analysis
        8- R&D
        9- Project Notes
        10- Pictures

        11- Quote Comparison
        12- Project Folder
        13- ER
         * 14-linked wo
         * 15 t1
         * 16 checklist
         */
        // 17 - Intergration Transaction History 
        // first we set all the visibles..
        if (!user.isContact)  // if its an NE member..
        {
            if (CanViewGross) // can the user see gross margin?
            {
                pc_main.TabPages[WorkOrderTabPage.Analysis].ClientVisible = true;
            }
            if (CanViewDollarTotalsAllBranches) // can the user see dollars info?
            {
                pc_main.TabPages[WorkOrderTabPage.Analysis].ClientVisible = true;   // anaylsis
                pc_main.TabPages[WorkOrderTabPage.POs].ClientVisible = true; // pos
            }
            if (_wo.QuoteID != "0")  // if its a quoted job
            {
                pc_main.TabPages[WorkOrderTabPage.Quote].ClientVisible = true;  // quote
                pc_main.TabPages[WorkOrderTabPage.QuoteComparison].ClientVisible = true;  // quote
            }
            pc_main.TabPages[WorkOrderTabPage.Customer].ClientVisible = CanViewCustomers;
            pc_main.TabPages[WorkOrderTabPage.LineItems].ClientVisible = true;
            pc_main.TabPages[WorkOrderTabPage.TimeEntries].ClientVisible = true;
            pc_main.TabPages[WorkOrderTabPage.ProjectNotes].ClientVisible = true;
            pc_main.TabPages[WorkOrderTabPage.History].ClientVisible = true;
            pc_main.TabPages[WorkOrderTabPage.IntegrationHistory].ClientVisible = true;

            //	pc_main.TabPages[10].ClientVisible = true;
            if (new NeBusinessUnit(_wo.business_unit_id).is_er)
            {
                pc_main.TabPages[WorkOrderTabPage.LinkedWorkOrders].ClientVisible = true;
            }
            if (IsPostBack && chkRD.Checked)
            {
                pc_main.TabPages[WorkOrderTabPage.ResearchAndDevelopment].ClientVisible = true;
            }
        }
    }
    protected void ddlAddress_Callback(object _sender, CallbackEventArgsBase _e)
    {
        populate_ddl_Contact(Convert.ToInt32(_e.Parameter), 0);
    }

    protected void ddl_Address_Callback(object _sender, CallbackEventArgsBase _e)
    {
        populate_ddl_Address(Convert.ToInt32(_e.Parameter), 0);
    }

    protected void ddl_Contact_Callback(object _sender, CallbackEventArgsBase _e)
    {
        populate_ddl_Contact(Convert.ToInt32(_e.Parameter), 0);
    }

    protected void ddl_Customer_CallBack(object _sender, CallbackEventArgsBase _e)
    {
        populate_ddlAddress(Convert.ToInt32(_e.Parameter), 0);
    }



    protected void ddlContact_Callback(object _sender, CallbackEventArgsBase _e)
    {
        if (_e.Parameter.Contains("Add"))
        {


        }
        else
        {
            populate_ddlContact(Convert.ToInt32(_e.Parameter), 0);
        }

    }
    protected void ddlPM_Callback(object _sender, CallbackEventArgsBase _e)
    {
        var q = new quote(Convert.ToInt32(_e.Parameter));
        populate_ddlPM(Convert.ToInt32(ex_ddl(ddlCompany)), 0);
        ddlPM.DataBind();
        if (_e.Parameter != "0")
        {
            try
            {
                ddlPM.JSProperties["cp_selected"] = ddlPM.Items.FindByValue(q.quoted_by).Index;
            }
            catch
            {
                // throw new Exception("Quote's \"Quoted By\" property isn't set or isn't valid");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Quote's \"Quoted By\" property isn't set or isn't valid');", true);
                return;
            }
            ddlPM.JSProperties["cp_description"] = q.txtJobDescription;
            ddlPM.JSProperties["cp_salesvalue"] = q.Price;
            ddlPM.JSProperties["cp_ContactID"] = q.contact_id;
        }
        else
        {
            ddlPM.JSProperties["cp_selected"] = 0;
            ddlPM.JSProperties["cp_description"] = "";
            ddlPM.JSProperties["cp_salesvalue"] = "";
            ddlPM.JSProperties["cp_ContactID"] = 0;
        }
    }


    protected void get_ts_comments(string _str_w_onumber)
    {
        var timecomment = Toolbox.doSQL_dt(@"SELECT WOComment_ID, WorkOrder_ID, Comments, CAST(PrintComments AS CHAR) AS PrintComments, Member_ID_Audit, Created_Date, WOComment_Company_ID FROM vwwocomments  where woprog_Id =@v0 and member_id = 0 ", new object[] { _wo.woprog_id, hidCompanyID.Value });
        if (timecomment.Rows.Count == 1)
        {
            foreach (DataRow dr_time in timecomment.Rows)
            {
                txtTSComments.Disabled = false;
                var wocomments_id = Convert.ToInt32(dr_time["WOComment_ID"]);

                chkPrint.Checked = false;
                txtTSComments.Disabled = false;
                txtTSComments.InnerText = dr_time["Comments"].ToString();
            }
        }
        else
        {
            txtTSComments.Disabled = false;
            if (!IsPostBack)
            {
                txtTSComments.InnerText = "No Timesheet Comments Found";
            }
            chkPrint.Checked = false;
        }
    }
    protected void txtCustPO_ButtonClick(object _source, ButtonEditClickEventArgs _e)
    {
        var prepared_po = txtCustPO.Text.Replace("\"", string.Empty).Replace("'", string.Empty);
        var businessUnit = new NeBusinessUnit(_wo.business_unit_id);

        if (_wo.PONumber != prepared_po)
        {
            Toolbox.doSQL_void(@"UPDATE woprog  SET woprog_CustPO=@v0, woprog_custpo_member_id =@v1 , woprog_custpo_dt = NOW()   WHERE woprog_id =@v2", new object[] { prepared_po, user.id, _wo.woprog_id });
            //if (businessUnit.DSN != "")
            //{
            //    _tools.getSQL_void(@"UPDATE SALES_ORDER_HEADER  SET cust_po_no=?  WHERE number =?", new NeBusinessUnit(_wo.business_unit_id).DSN, new object[] { prepared_po, _wo.OrderNumber });
            //    _tools.getSQL_void(@"UPDATE SALES_HISTORY_HEADER  SET cust_po_no=?  WHERE ord_no =?", new NeBusinessUnit(_wo.business_unit_id).DSN, new object[] { prepared_po, _wo.OrderNumber });
            //}
        }
        hid_ts.Value = Convert.ToDateTime(Toolbox.doSQL_string(@"SELECT woprog_ts FROM woprog  WHERE woprog_id =@v0", new object[] { _wo.woprog_id })).Ticks.ToString();
    }
    protected void imgbnotesupdate_Click(object _sender, ImageClickEventArgs _e)
    {
        var sql = "";
        if (hidNotesID.Value == "0")
        {
            hidNotes.Value = textNotes.Text;
        }
        else
        {
            if (hidOrigin.Value == "purchaseorder")
            {
                sql = string.Format(@"UPDATE po_details_current SET po_details_notes = ""{0}"" WHERE po_details_id = {1}", textNotes.Text, hidNotesID.Value);
            }
            try
            {
                Toolbox.doSQL_void(@"UPDATE po_details_current SET po_details_notes = @v0  WHERE po_details_id = @v1 ", new object[] { textNotes.Text, hidNotesID.Value });
                lblError.Text = "";
            }
            catch
            {
                lblError.Text = "Unable to save note";
                lblError.ClientVisible = true;
            }
        }
        textNotes.Text = "";
        ASPxpuNotes.ShowOnPageLoad = false;
    }
    protected string note_handler(object _container)
    {
        var c = _container as GridViewDataItemTemplateContainer;
        var label_text = "";
        var row_id = c.KeyValue.ToString();
        var _c = 0;
        var n = "";
        if (row_id != "" && row_id != null)
        {
            _c = Toolbox.doSQL_int(@"SELECT ifnull(Length(po_details_notes),0) FROM po_details_current  where po_details_id =@v0", new object[] { row_id });
            if (_c > 0)
                n = Toolbox.doSQL_string(@"SELECT po_details_notes FROM po_details_current  where po_details_id =@v0", new object[] { row_id });
        }
        var div = "";
        if (_c > 0)
        {
            div = string.Format(@"<div class='opt' data-tooltip=""{0}"" data-width='500' data-title='notes'>", n);
        }
        label_text = _c > 0 ? div + "<img src='/images/fullnotes.jpg' border='0'/></div>" : "<img src='/images/emptynotes.jpg' border='0'/>";
        return label_text;
    }
    protected void openpo_click(object _sender, EventArgs _e)
    {
        /*
         * Changed to Boing link directly
		var index = (((LinkButton)_sender).NamingContainer as GridViewDataRowTemplateContainer).VisibleIndex;
		var val = grid_POs.GetRowValues(index, "poprogid");
		var progress = new NePOProg(Convert.ToInt32(val));
		ScriptManager.RegisterStartupScript(this, GetType(), "open_", "boing('../purchaseorder/po_prog_add.aspx?action=show&poprogid=" + progress.poprog_id + "&companyid=" + progress.business_unit_id + "','po',950,800)", true);
	    */
    }
    protected void openvendor_click(object _sender, EventArgs _e)
    {
        var index = (((LinkButton)_sender).NamingContainer as GridViewDataRowTemplateContainer).VisibleIndex;
        var val = grid_POs.GetRowValues(index, "vendorid");
        ScriptManager.RegisterStartupScript(this, GetType(), "open_", "boing('/#/opens/11/vendors/" + val + "','vendor',950,800)", true);
    }
    protected string fill_terms(object _quote_id)
    {
        string result = "";
        if (_quote_id.ToString() != "0")
        {
            div_quoteterms_tr.Style.Add("display", "");
        }
        else
        {
            div_quoteterms_tr.Style.Add("display", "none");
        }
        //div_quoteterms_tr.Visible =  _quote_id.ToString() != "0";
        if (_quote_id.ToString().Length > 6)
        {
            _quote_id = _quote_id.ToString().Substring(0, 6);
        }
        if (_quote_id.ToString() != "0")
        {

            var down = " ";
            var perdr = Toolbox.doSQL_dt(@"SELECT a.percentage_down FROM quote_master a LEFT JOIN quote_term b ON a.net_due = b.id WHERE a.quote_id = @v0  AND active_revision = true", new object[] { _quote_id });
            if (perdr.Rows.Count > 0)
            {
                // Grab quote terms, add to this work order page
                down = perdr.Rows[0]["percentage_down"].ToString().Trim();
                if (down != "")
                    down = string.Format("<div>{0} % Down</div>", down);
            }

            var dr = Toolbox.doSQL_dt(@" SELECT a.custom_term,  Case  When c.Term_Desc Is NULL Then b.description ELSE c.Term_Desc  End AS net_due FROM quote_master a LEFT JOIN  quote_term b ON a.net_due = b.id  left join term as c on b.term_id=c.Term_ID WHERE a.quote_id = @v0  AND active_revision = true", new object[] { _quote_id });
            var net_due = "";
            var custom_term = "";
            if (dr.Rows.Count > 0)
            {
                //var dr_quoteinfor = dr.Rows[0];
                net_due = dr.Rows[0]["net_due"].ToString().Trim();

                if (net_due != "")
                {
                    net_due = string.Format("<div>{0}</div>", net_due);
                }
                custom_term = dr.Rows[0]["custom_term"].ToString().Trim();
                if (custom_term != "")
                {
                    custom_term = string.Format("<div>{0}</div>", custom_term);
                }
            }

            result = string.Format("{2}\n{0}\n{1}", net_due, custom_term, down).Trim().Replace("\"", "&quot;").Replace("\n", "");

            // lblquoteterms.Text= string.Format("{2}\n{0}\n{1}", net_due, custom_term, down);

            // div_quoteterms_td.InnerHtml = string.Format("{2}\n{0}\n{1}", net_due, custom_term, down);
        }
        return result;
    }

    protected void openpart_click(object _sender, EventArgs _e)
    {
        var index = (((LinkButton)_sender).NamingContainer as GridViewDataRowTemplateContainer).VisibleIndex;
        var val = grid_POs.GetRowValues(index, "masterid");
        ScriptManager.RegisterStartupScript(this, GetType(), "open_", "boing('../member/inventory/index.aspx?a=get&tab=G&id=" + val + "','inventory',950,800)", true);
    }
    protected string noteinactive_handler(object _container)
    {
        var c = _container as GridViewDataItemTemplateContainer;
        var label_text = "";
        var row_id = c.KeyValue.ToString();
        var _c = 0;
        if (row_id != "" && row_id != null)
        {
            _c = Toolbox.doSQL_int(@"SELECT po_details_line_active FROM po_details_current  where po_details_id =@v0", new object[] { row_id });
        }
        label_text = _c == 0 ? "<img src='/images/icon/icon[ok_notyet].gif' width='16' height='16' border='0'/>" : "<img src='/images/icon/icon[ok].gif' width='16' height='16' border='0'/>";
        return label_text;
    }
    protected void Build_Tax_Table()
    {

        var tax_data = Toolbox.doSQL_dt(@"SELECT tax_id, tax_percentage, tax_name FROM tax order by tax_bv_s_tax_no", null);
        var address = new NEAddress(_wo.woprog_Address_ID);
        var comp = new NeBusinessUnit(_wo.business_unit_id);
        //		NeWOProg tempwo = new NeWOProg(wo.woprog_id);
        if (hidWOProgID.Value != "0")
        {
            foreach (DataRow row in tax_data.Rows)
            {
                if (comp.Tax1.ToString() == row[0].ToString())
                {
                    chkTax1.ClientVisible = true;
                    chkTax1.Text = row[1] + " " + row[2];
                    chkTax1.Checked = _wo.woprog_tax1 != 0 ? true : false;
                    if (address.Tax1Exempt != "")
                    {
                        chkTax1.Text += " -- Exempt : " + address.Tax1Exempt;
                        chkTax1.ClientEnabled = false;
                    }
                }
                if (comp.Tax2.ToString() == row[0].ToString())
                {
                    chkTax2.ClientVisible = true;
                    chkTax2.Text = row[1] + " " + row[2];
                    chkTax2.Checked = _wo.woprog_tax2 != 0 ? true : false;
                    if (address.Tax1Exempt != "")
                    {
                        chkTax2.Text += " -- Exempt : " + address.Tax2Exempt;
                        chkTax2.ClientEnabled = false;
                    }
                }
                if (comp.Tax3.ToString() == row[0].ToString())
                {
                    chkTax3.ClientVisible = true;
                    chkTax3.Text = row[1] + " " + row[2];
                    chkTax3.Checked = _wo.woprog_tax3 != 0 ? true : false;
                    if (address.Tax3Exempt != "")
                    {
                        chkTax3.Text += " -- Exempt : " + address.Tax3Exempt;
                        chkTax3.ClientEnabled = false;
                    }
                }
                if (comp.Tax4.ToString() == row[0].ToString())
                {
                    chkTax4.ClientVisible = true;
                    chkTax4.Text = row[1] + " " + row[2];
                    chkTax4.Checked = _wo.woprog_tax4 != 0 ? true : false;
                    if (address.Tax1Exempt != "")
                    {
                        chkTax4.Text += " -- Exempt : " + address.Tax4Exempt;
                        chkTax4.ClientEnabled = false;
                    }
                }
            }
        }
    }
    protected void btn_SaveProjectNotes_Click(object _sender, EventArgs _e)
    {
        var commtext = mem_projectNotes.Text.Trim().Replace("'", "''");
        if (commtext != "")
        {
            var note_exists = 0;
            try
            {
                note_exists = Toolbox.doSQL_int(@"Select ifnull(count(woprog_project_notes_id),0) from woprog_project_notes  where woprog_project_notes_woprogid =@v0 and woprog_project_notes_type = 'W'", new object[] { _wo.woprog_id });
            }
            catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
            if (note_exists > 0)
            {
                Toolbox.doSQL_void(@"Update woprog_project_notes  set woprog_project_notes_notes =@v0, woprog_project_notes_memberid =@v1 , woprog_project_notes_datetime = now()   where woprog_project_notes_woprogid =@v2 and woprog_project_notes_type = 'W'", new object[] { commtext, user.id, hidWOProgID.Value });
            }
            else
            {
                Toolbox.doSQL_void(@"INSERT INTO woprog_project_notes (woprog_project_notes_WOProgID,woprog_project_notes_MemberID,woprog_project_notes_notes,woprog_project_notes_type,woprog_project_notes_datetime) VALUES (@v0,@v1,@v2,'W',NOW())", new object[] { hidWOProgID.Value, user.id, commtext });
            }
        }
        fill_page_info("Project Notes");
    }
    protected void btn_AddnewComment_Click(object _sender, EventArgs _e)
    {
        var commtext = mem_chatnewline.Text.Trim().Replace("'", "''");
        if (commtext != "")
        {

            Toolbox.doSQL_void(@"INSERT INTO WOProgComment (WOProgComment_WOProg_ID,WOProgComment_Member_ID,WOProgComment_Text,WOProgComment_X,WOProgComment_Y,WOProgComment_DateTime)  VALUES (@v0,@v1,@v2,5,5,NOW())",
                new object[] { hidWOProgID.Value, user.id, commtext });
            mem_chatnewline.Text = "";
        }
        fill_page_info("Project Notes");
    }
    private void get_comments(string _str_wo_prog_id)
    {
        var cnt = 0;
        var layer = "<div  id=\"content\" style=\"min-height:180px; PADDING-RIGHT: 5px; PADDING-LEFT: 5px; PADDING-BOTTOM: 5px; PADDING-TOP: 5px; \">"
           + ""
           + "";
        var str_comments_sql = "SELECT *,member_fullname AS MEMNAME " +
                 "FROM WOProgComment,Member,WOProg " +
                 "WHERE WOProgComment_WOProg_ID = @v0" +
                 " AND WOProgComment_WOProg_ID=WOProg_ID " +
                 " AND WOProgComment_Member_ID=Member_ID " +
                 " AND WOProgComment_Deleted='F' order by woprogcomment_id desc";
        //        DataTable Comments = Toolbox.do_dt(strCommentsSQL);

        var dt_comments = Toolbox.doSQL_dt(str_comments_sql, new object[] { _str_wo_prog_id });
        foreach (DataRow dr in dt_comments.Rows)

        {
            cnt++;
            layer += "<span class=\"chatcontentname\">" + dr["MEMNAME"] + " - " + dr["WOProgComment_DateTime"] + "</span><br>" + dr["WOProgComment_Text"].ToString().Replace("\n", "<br />") + "<hr>";
        }
        if (cnt == 0)
        {
            layer += "No Comments Entered";
        }
        layer += @"
			</div>
			";
        divComments.InnerHtml = layer;
    }
    protected string All_Systems_Go(string _type)
    {
        var issue = "";

        if (_type == "Invoice")
        {
            fill_page_info("General");
            var so = new NeSalesOrder();
            var bu = new NeBusinessUnit(_wo.business_unit_id);
            var doBv = bu.DSN != "";
            so.DSN = bu.DSN;
            var temp_customer = new NECustomer((int)_wo.WOProg_Customer_ID);
            var temp_address = new NEAddress(_wo.woprog_Address_ID);
            string lockedby;
            //if (doBv && (lockedby = so.CheckOrderLock(_wo.OrderNumber)) != "")
            //{
            //    return "Work order locked in bv by " + lockedby;
            //}
            if ((temp_customer.Address.csp.po_required || temp_address.csp.po_required) && string.IsNullOrEmpty(txtCustPO.Text))
            {
                return "Customer requires a valid purchase order before invoicing";
            }
            if (has_open_childs)
            {
                return "This is a parent work order that has open child work orders that need to be closed first.  Review on the linked work orders tab.";
            }
        }
        #region Active PO Lines Check
        var active_polines = Toolbox.doSQL_int(@"SELECT COUNT(po_details_id) FROM po_details_current WHERE po_details_line_active = 1 AND po_details_woprog_id = @v0 and po_details_current.is_gl_account = false AND po_details_part_no != 0", new object[] { _wo.woprog_id });
        if (active_polines > 0)
        {
            pc_main.ActiveTabIndex = 4;
            fill_page_info("POs");
            return "There are incomplete PO lines related to this work order";
        }
        #endregion Active PO Lines Check
        #region AP Problems Check
        var ap_problems = Toolbox.doSQL_int(@"SELECT COUNT(poprog_id) FROM poprog_header WHERE poprog_status = 10 AND poprog_id IN (SELECT po_details_poprog_id FROM po_details_current WHERE po_details_woprog_id = @v0 and po_details_current.is_gl_account = false )", new object[] { _wo.woprog_id });
        if (ap_problems > 0)
        {
            pc_main.ActiveTabIndex = 4;
            fill_page_info("POs");
            return "There are linked PO's that are currenly in AP Problems";
        }
        #endregion AP Problems Check
        var dt = Toolbox.doSQL_dt(@"Select (wo_detail_current_price_sell-wo_detail_current_price_cost) as spread, wo_detail_current_master_id master_id from wo_detail_current  where wo_detail_current_woprog_id =@v0 AND IFNULL(wo_detail_current_notes,'') NOT LIKE 'A_%|%'", new object[] { _wo.woprog_id });
        foreach (DataRow dr in dt.Rows)
        {
            if (Convert.ToDouble(dr["spread"]) < 0 && _wo.woprog_iscredit == 0)
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
            lblError.Text = issue;
            lblError.ClientVisible = false;
        }
        return issue;
    }

    public int is_directory_full(string _path)
    {
        var a = Directory.Exists(_path)
                    ? Directory.GetFiles(_path, "*.*", SearchOption.AllDirectories).Length
                    : 0;
        return a;
    }

    private void check_open_pos()
    {
        if (_wo.woprog_id == 0) return;
        var no_pos = Toolbox.doSQL_int(@"SELECT COUNT(DISTINCT po_details_poprog_id) FROM po_details_current WHERE po_details_woprog_id = @v0 and is_gl_account = false ", new object[] { _wo.woprog_id });
        var no_lines = Toolbox.doSQL_int(@"SELECT COUNT(po_details_poprog_id) FROM po_details_current WHERE po_details_woprog_id = @v0 and is_gl_account = false ", new object[] { _wo.woprog_id });
        var un_pos = Toolbox.doSQL_int(@"SELECT COUNT(po_details_id) FROM po_details_current WHERE po_details_woprog_id = @v0  AND po_details_line_active = 1 and is_gl_account = false", new object[] { _wo.woprog_id });
        var re_pos = Toolbox.doSQL_int(@"SELECT COUNT(po_details_id) FROM po_details_current WHERE po_details_woprog_id = @v0  AND po_details_line_active = 0 and is_gl_account = false", new object[] { _wo.woprog_id });
        pc_main.TabPages[WorkOrderTabPage.POs].Text = "PO's (" + no_pos + ")";

        var path = @"f:\ProjectFolders\WO" + _wo.woprog_id + "-" + _wo.OrderNumber + "-" + _wo.CustomerName;

        pc_main.TabPages[13].Text = string.Format("Project Folder ({0})", is_directory_full(path));



        if (no_lines > 0)
        {
            pc_main.TabPages[WorkOrderTabPage.POs].ToolTip = "Received Lines: " + re_pos + "/" + no_lines + "\nUnreceived Lines: " + un_pos + "/" + no_lines;
        }
        if (un_pos > 0)
            pc_main.TabPages[WorkOrderTabPage.POs].TabStyle.ForeColor = System.Drawing.Color.Red;
    }


    protected void btnsaverdmemos_Click(object _sender, EventArgs _e)
    {
        try
        {
            Toolbox.doSQL_void(@"Update woprog  set woprog_CoreCompetency =@v0,woprog_Uncertainty=@v1 ,woprog_advancement=@v2   where woprog_id =@v3 limit 1 ", new object[] { mem_Competancy.Text, mem_Uncertainty.Text, mem_Advancement.Text, _wo.woprog_id });
        }
        catch
        {
            errorlabel.Text = "Could not save RD information";
        }
    }
    protected void btn_printbarcode_Click(object _sender, EventArgs _e)
    {
        try
        {
            NeWOProg work_order;
            work_order = new NeWOProg(Convert.ToInt32(hidWOProgID.Value));
            work_order.print_barcode_label(1);
            errorlabel.Text = "";
        }
        catch
        {
            errorlabel.Text = "Error printing bar code";
        }
    }
    private void chk_interco()
    {
        var woCustomer = new NECustomer();
        if (ddlCustomer.Value != null && ddlCustomer.Value.ToString() != "0")
        {
            woCustomer = new NECustomer(Convert.ToInt32(ddlCustomer.Value));
            if (woCustomer.IsNEcompany)
            {
                var internalCustomerBusinessUnit = new NeBusinessUnit(woCustomer.NEbusiness_unit_id);
                var woBusinessUnit = new NeBusinessUnit(ex_ddl(ddlCompany));
                if (woCustomer.NEbusiness_unit_id == (int)ddlCompany.Value) return;

                //   var woTaxEntity = new NeTaxEntity(woBusinessUnit.tax_entity_id);
                var woAddress = new NEAddress(Convert.ToInt32(ex_ddl(ddlAddress)));
                //      var internalCustomerTaxEntity = new NeTaxEntity(internalCustomerBusinessUnit.tax_entity_id);
                if (internalCustomerBusinessUnit.id != woBusinessUnit.id)
                {
                    td_parent_info.Style["display"] = user.isContact ? "none" : "";
                }
                populate_parent_workorder(internalCustomerBusinessUnit.id);
                if (ddl_parent_workorder.Value != null && ddl_parent_workorder.Items.FindByValue(ddl_parent_workorder.Value) != null)
                {
                    ddl_parent_workorder.Value = ddl_parent_workorder.Value ?? 0;
                }
                else
                {
                    ddl_parent_workorder.Value = 0;
                }
            }
        }
    }
    protected void cbp_left_Callback(object _sender, CallbackEventArgsBase _e)
    {
        using (var conn = Toolbox.connect())
        {

            ddlAddress.DataBind();
            var wo_type = (int)radiotype.Value;
            if (_e.Parameter == "ddlcreditwochanged" || wo_type == 1)
            {
                if (ddl_creditwolink.Value != null)
                {
                    if (wo_type != 0)
                    {
                        try
                        {
                            var owo = new NeWOProg(Convert.ToInt32(ddl_creditwolink.Value));
                            populate_ddlCustomer(Convert.ToInt32(ddlCompany.Value), Convert.ToInt32(owo.WOProg_Customer_ID));

                            populate_ddlAddress(Convert.ToInt32(owo.WOProg_Customer_ID), Convert.ToInt32(owo.woprog_Address_ID));
                            populate_ddlContact(Convert.ToInt32(owo.WOProg_Customer_ID), Convert.ToInt32(owo.woprog_Contact_ID));

                            populate_ddlPM(Convert.ToInt32(ddlCompany.Value), Convert.ToInt32(owo.intProjectManager));
                            ddlAddress.DataBind();

                            var active = Toolbox.doSQL_string(conn, @"Select Active from address where Address_ID = @v0", new object[] { owo.woprog_Address_ID });
                            var nextBillingAddressName = Toolbox.doSQL_string(conn, @"Select Address_Addr1 from address where Address_table_id = @v0 AND Address_Type = 'B' AND Active = 1", new object[] { owo.WOProg_Customer_ID });
                            var nextBillingAddress = Toolbox.doSQL_int(conn, @"Select address_id from address where Address_table_id = @v0 AND Address_Type = 'B' AND Active = 1", new object[] { owo.WOProg_Customer_ID });
                            var this_customer = new NECustomer(Convert.ToInt32(ex_ddl(ddlCustomer)));
                            var previousBillingAddressName = Toolbox.doSQL_string(conn, @"Select Address_Addr1 from address where Address_ID = @v0", new object[] { owo.woprog_Address_ID });

                            if (active == "False")
                            {
                                inactive_icon.Visible = true;
                                ddlAddress.Value = nextBillingAddress;

                                var e = new NeEMail();

                                try
                                {
                                    if (Toolbox.app_setting("debug_redirect") == "1")
                                        e.To = Toolbox.app_setting("debug_redirect_email");
                                    else
                                        e.To = user.business_unit.CustomerRequestEmail;

                                    e.CC = user.NEEmail;
                                    e.Subject = "Work Order " + owo.woprog_id + " referencing inactive address in " + ddlCompany.Text + " branch";

                                    e.From = "noreply@" + Toolbox.app_setting("DomainForEmail"); ;
                                    e.isHTML = true;

                                    if (radiotype.Value.ToString() == "1")
                                    {
                                        e.Body += user.FirstName + " " + user.LastName + " is creating a Credit that is linked to an inactive address(" + previousBillingAddressName + "). <br/><br/>The address has been defaulted to the customer's(" + this_customer.Customer_Name + ") active billing address(" + nextBillingAddressName + ").<br/><br/>";
                                    }
                                    if (radiotype.Value.ToString() == "2")
                                    {
                                        e.Body += user.FirstName + " " + user.LastName + " is creating a Rebill that is linked to an inactive address(" + previousBillingAddressName + "). <br/><br/>The address has been defaulted to the customer's(" + this_customer.Customer_Name + ") active billing address(" + nextBillingAddressName + ").<br/><br/>";
                                    }
                                    e.Send();
                                }
                                catch (Exception ee) { shared.alert_debug("Error sending inactive address email", "Email:<br/>" + Toolbox.dict_dump(Toolbox.dict_create(e)) + "<br/> Exception:<br/>" + ee); }


                            }
                            else
                            {
                                ddlAddress.Value = owo.woprog_Address_ID;
                            }

                            ddlContact.DataBind();
                            var tempcust = new NECustomer();
                            spndayscredit.Value = owo.woprog_dayscredit;
                            ddlquote.ClientEnabled = false;
                            if (chkDownPayment.Checked)
                            {
                                spndayscredit.Value = 0;
                                combo_default_terms.Value = OpsStaticTerms.DueOnReceipt;
                            }
                            //dteStartDate.Date = DateTime.Today;

                        }
                        catch
                        {
                            errorlabel.Text = "Error setting wo details";
                            errorlabel.Visible = true;
                            return;
                        }
                    }
                }
            }
            else if (_e.Parameter.StartsWith("downpayment") && _e.Parameter.Contains("|"))
            {
                var splitParas = _e.Parameter.ToLower().Split('|');
                PopulateTermId(true, splitParas[1]);
            }
            else
            {
                PopulateDataBasedOnCustomer(null);
            }

            // Task 1459: Parent WO drop down visible on internal Progress Bills
            ShowOrHideParentWorkOrderDropDownBasedOnProgressBillCheckbox();

            if (!string.IsNullOrWhiteSpace(ddlCustomer.Text) && !_e.Parameter.StartsWith("downpayment"))
            {
                PopulateTermId(false);
            }
        }
    }
    private int DefaultTermId()
    {
        return Toolbox.doSQL_int(@"Select ifnull(customer_term_id,0) as customer_term_id from customer where customer_id=@v0 limit 1", new object[] { ddlCustomer.Value });
    }
    private void PopulateTermId(bool isDownPayment, string downPaymentValue = "false")
    {
        if (isDownPayment)
        {
            combo_default_terms.Value = downPaymentValue == "true" ? OpsStaticTerms.DueOnReceipt : DefaultTermId();
        }
        else
        {
            combo_default_terms.Value = DefaultTermId();
        }
    }
    private void PopulateDataBasedOnCustomer(quote quote)
    {
        if (quote != null)
        {
            ddl_custram.DataBind();
        }

        var temp_business_unit_id = Convert.ToInt32(ddlCompany.Value);
        var bu = new NeBusinessUnit(temp_business_unit_id);
        var tempcust = new NECustomer();
        if (ddlCustomer.Value != null && ddlCustomer.Value.ToString() != "0")
        {
            tempcust = new NECustomer(Convert.ToInt32(ddlCustomer.Value));

            if (ddlAddress.Value == null && ddlAddress.Items.Count == 1)
            {
                ddlAddress.Value = tempcust.Address.id;
            }
            populate_ddlAddress(Convert.ToInt32(ddlCustomer.Value), tempcust.Address.id);
            spndayscredit.Value = tempcust.customer_creditdays;
            ;
            if (chkDownPayment.Checked)
            {
                spndayscredit.Value = 0;
                spndayscredit.ClientEnabled = false;
            }
            //var balance = dsn == "" ? 0 : tempcust.Get_outstanding_balance(45, tempcust.Customer_Number, dsn);
            //var in_breach = dsn != "" && NECustomer.in_breach(tempcust.Customer_ID, Convert.ToInt32(ddlCompany.Value), 0);
            lblCustomerWarning.Text = "";
            if (tempcust.Hold == "T")
            {
                lblCustomerWarning.Text = "On Hold by: " + new NeMember(Convert.ToInt32(tempcust.customer_whohold)).FullName + "   Reason: " + tempcust.customer_whyhold;
            }
            var bm = new NeBusinessUnit(ddlCompany.Value).branch_manager;
            var bm_id = bm.id;
            combo_default_invoicetype.Items.FindByValue(tempcust.default_invoicetype).Selected = true;
            var isBMFromSameBranch = bm.business_unit_id == temp_business_unit_id;
            if (isBMFromSameBranch)
            {
                ddlPM.Value = bm_id;
            }
            if (tempcust.Credit_Type == 3)
            {
                chk_cc.Checked = true;
                chk_cc.ClientEnabled = false;
                spndayscredit.Value = 0;
            }
            if (chkDownPayment.Checked)
            {
                spndayscredit.Value = 0;
                combo_default_terms.Value = OpsStaticTerms.DueOnReceipt;
            }
            if (ddlAddress.SelectedIndex >= 0)
            {
                var tempaddress = new NEAddress(Convert.ToInt32(ddlAddress.Value));
                var tempcsp = new customer_sales_properties(tempaddress.id);
                if (tempcsp.project_mgr_member_id != 0)
                {
                    var pm_id = tempcsp.project_mgr_member_id;
                    if (ddlPM.Items.FindByValue(pm_id) != null && ddlPM.Items.FindByValue(pm_id).Value != null)
                    {
                        ddlPM.Value = pm_id;
                    }
                    else if (isBMFromSameBranch)
                    {
                        ddlPM.Value = bm_id;
                    }
                    else if (ddlPM.Items.Count > 0)
                    {
                        ddlPM.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlPM.SelectedIndex = -1;
                    }
                }
            }

            int bdm = 0;
            if (quote?.bdm > 0)
            {
                bdm = quote.bdm;
            }
            else if (tempcust.s_regional_account_manager != 0 || tempcust.s_regional_account_manager != 999999)
            {
                bdm = tempcust.s_regional_account_manager;
            }

            if (bdm > 0)
            {
                var editItem = ddl_custram.Items.FindByValue(bdm);
                if (editItem != null && editItem.Value != null)
                {
                    ddl_custram.Value = bdm;
                    cbp_left.JSProperties["cp_ddlram"] = ddl_custram.Value.ToString();
                }
            }
            else
            {
                ddl_custram.SelectedIndex = 0;
            }
        }

        if (quote == null)
        {
            ddlContact.Value = null;
            ddlContact.DataBind();
            ddlquote.Value = null;
            ddlquote.DataBind();
        }

        ddlCustomer.DataBind();

        ddlCustomer.Value = tempcust.id != 0 ? tempcust.id : ddlCustomer.Value;
        ddlCompany.Value = temp_business_unit_id;
        if ((int)ddlquote.Items.Count > 1)
        {
            sds_address.SelectCommand = "Select  0 as address_id,'Not Applicable' as address union Select address_id,address as address from vw_address where cust_id = @custid";
            ddlAddress.DataBind();
        }

        chkProgress.ClientEnabled = !bu.is_er;
    }

    private void TrySetActingBDMFromQuote(quote quote)
    {
        if (quote?.acting_bdm > 0)
        {
            var actingBDMItem = ddl_rams.Items.FindByValue(quote.acting_bdm);
            if (actingBDMItem != null && actingBDMItem.Value != null)
            {
                ddl_rams.Value = quote.acting_bdm;
            }
        }
    }

    protected void cb_DataBound(object _sender, EventArgs _e)
    {
        var ddl = (ASPxComboBox)_sender;
        if (ddl.Value != null && Convert.ToString(ddl.ID) != "ddlquote" && Convert.ToString(ddlquote.Value) != "0")
        {
            try
            {
                ddl.SelectedIndex = ddl.Items.Count > 0 && ddl.Value != null && ddl.Items.FindByValue(ddl.Value) != null ? ddl.Items.FindByValue(ddl.Value).Index : -1;
            }
            catch
            {
                ddl.SelectedIndex = -1;
            }
        }
        else if (ddl.ID != "ddlquote" && ddl.ID != "ddlDept")
        {
            if (ddl.Items.Count <= 1)
            {
                ddl.SelectedIndex = 0;
            }
            //else
            //{
            //	//		ddl.SelectedIndex = -1;
            //}
        }

        if (new List<string>(new string[] { "ddlAddress", "ddlContact", "ddlPM", "ddlDept", "ddl_Contact", "ddl_Address" }).Contains(ddl.ID) && (ddl.SelectedIndex == 0 || ddl.SelectedIndex == -1) && ddl.Items.Count > 0)
        {
            if ((ddl.ID == "ddlContact" || ddl.ID == "ddl_Contact") || (ddl.ID == "ddlAddress"))
            {
                if (!user.isContact)
                {
                    ddl.SelectedIndex = 0;
                }
                else
                {
                    ddl.Value = Convert.ToInt32(user.ContactID);
                }
            }
            else
            {
                if (ddl.Items.Count <= 1)
                {
                    ddl.SelectedIndex = 0;
                }
                else
                {
                    //	ddl.SelectedIndex = -1;
                }
            }

            if (ddl.ID == "ddl_Address")
            {
                ddl.Value = _wo.woprog_Address_ID;
            }

            if (ddl.ID == "ddl_Contact")
            {
                ddl.Value = _wo.woprog_Contact_ID;
            }
        }
        if (ddl.ID == "ddlquote")
        {
            var lic2 = new ListEditItem("If Applicable Select Quote", 0);
            ddlquote.Items.Insert(0, lic2);
        }
        if (ddl.ID == "ddlquote" && ddl.Value != null && ddl.Value.ToString() != "0" && ddlquote.Items.Count == 0)
        {
            var quote_stuff = Toolbox.doSQL_string(@" Select cast(concat(quote_master.quote_id,'V',quote_master.revision,' - ',urldecode(left(job_description,20)))as CHAR) from quote_master  where quote_master.quote_id=@v0 and quote_master.revision =@v1 ", new object[] { ddl.Value.ToString().Substring(0, 6), ddl.Value.ToString().Substring(6, 1) });
            ddlquote.Items.Add(quote_stuff, ddl.Value);
        }
        else if (ddl.ID == "ddlquote" && (ddl.Value == null || ddl.Value.ToString() == "0"))
        {
            ddl.SelectedIndex = 0;
        }
        if (ddl.ID == "ddlPM" && (ddl.Value == null || ddl.Value.ToString() == "0"))
        {
            ddl.SelectedIndex = 0;
        }
    }


    protected void popCloseReassigningSave_Click(object _sender, EventArgs _e)
    {

        try
        {
            // if (!((int)this.ddl_Customer.Value==0) && !(this.ddl_Address.SelectedIndex <= 0) && !(this.ddl_Contact.SelectedIndex <= 0) && !string.IsNullOrWhiteSpace(this.txtCloseReassign.Text))
            if ((int)this.ddl_Customer.Value > 0 && (int)this.ddl_Address.Value > 0 && (int)this.ddl_Contact.Value > 0 && !string.IsNullOrWhiteSpace(this.txtCloseReassign.Text))
            {
                if (hidWOProgID.Value != "0")  // if it's not a new work order... a new work order has a wo_prog of 0.
                {
                    if (ddlRevenueLines.Value != null)
                    {
                        var Prev_woprog_id = _wo.woprog_id;
                        var Prev_CustomerName = _wo.CustomerName;

                        CreateWorkOrder(false);
                        CreateNewWO((int)ddlRevenueLines.Value, Prev_woprog_id, Prev_CustomerName);  // New WO

                        string strscript = string.Format("boing('./index.aspx?woprog_id=" + hidWOProgID.Value + "&business_unit_id=" + _wo.business_unit_id + "','_blank',800,800)");

                        UpdateExistingWO((int)ddlRevenueLines.Value, Prev_woprog_id, Prev_CustomerName, Convert.ToInt32(hidWOProgID.Value));   //Old WO


                        ScriptManager.RegisterStartupScript(this, GetType(), "open_", strscript, true);
                        // Thread.Sleep(3500);
                        ScriptManager.RegisterStartupScript(this, GetType(), "refresh", "location.href = location.href;", true);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('" + ex.ToString() + "');", true);
        }
        finally
        {
            popReassignReason.ShowOnPageLoad = false;
        }
        return;

    }


    protected void Emailsending(string Sendto, string CC, string content)
    {
        var e = new NeEMail();
        if (Toolbox.app_setting("debug_redirect") == "1")
            e.To = Toolbox.app_setting("debug_redirect_email");
        else
            e.To = Sendto;
        e.CC = CC;
        e.From = "admin@" + Toolbox.app_setting("DomainForEmail");
        e.Subject = "Closed & Reassigned";
        e.isHTML = true;
        e.Body = content.Replace("\r\n", "<br/>");
        e.Send();
    }

    protected void CreateNewWO(int Revenueline, int Prev_woprog_id, string Prev_CustomerName)
    {

        var EmailContent = " ";
        NeBusinessUnit bu = new NeBusinessUnit(_wo.business_unit_id);
        NeTaxEntity TX = new NeTaxEntity(bu.tax_entity_id);
        switch (Revenueline)
        {
            case 3: //TM
                NeWOProg NewWo = new NeWOProg();

                NewWo = _wo;
                NewWo.business_unit_id = _wo.business_unit_id;

                NewWo.WOProg_Customer_ID = (int)this.ddl_Customer.Value;
                NewWo.CustomerName = (string)this.ddl_Customer.Text;
                NewWo.woprog_Address_ID = (int)this.ddl_Address.Value;
                NewWo.woprog_Contact_ID = (int)this.ddl_Contact.Value;
                // NewWo.woprog_billing_email = Configuration.BillingEmail;


                if (!string.IsNullOrWhiteSpace(this.txt_PONum.Text))
                {
                    NewWo.PONumber = this.txt_PONum.Text;
                }

                //if (bu.tax_entity_id == 2) // Tax Entity = NE
                //    NewWo.woprog_controller_id = 1611;

                string CC = "";

                if (Toolbox.ReturnZeroIfNull_int(TX.controller_id) != 0)
                {
                    int i = Toolbox.ReturnZeroIfNull_int(TX.controller_id);
                    NeMember member = new NeMember(i);
                    CC = member.NEEmail;
                }


                if (Prev_woprog_id != NewWo.woprog_id)
                {
                    NewWo.special_instructions = "Old WO#:" + Prev_woprog_id + "\n";
                    EmailContent = "From Old WO#:" + Prev_woprog_id + "\n";
                    EmailContent += "To New WO#:" + NewWo.woprog_id + "\n";
                }
                if (Prev_CustomerName != NewWo.CustomerName)
                {
                    NewWo.special_instructions += "Old Customer:" + Prev_CustomerName + "\n";
                    EmailContent += " From Old Customer:" + Prev_CustomerName + "\n";
                    EmailContent += " To New Customer:" + NewWo.CustomerName + "\n";
                }

                //  NewWo.close_reassign_reason = this.txtCloseReassign.Text;
                //Send an email to the billing email against the tax entity, CC'ing the Controller email address to notify them of this change (for impact to margin analysis - Make sure to include the Old & New WO#'s)
                if (!string.IsNullOrEmpty(TX.billing_email))
                {
                    Emailsending(TX.billing_email, CC, EmailContent);
                }

                NewWo.Save();

                break;
            case 4: // Quoted

                NeWOProg New_Wo = new NeWOProg();

                New_Wo = _wo;
                New_Wo.business_unit_id = _wo.business_unit_id;

                New_Wo.WOProg_Customer_ID = (int)this.ddl_Customer.Value;
                New_Wo.CustomerName = (string)this.ddl_Customer.Text;
                New_Wo.woprog_Address_ID = (int)this.ddl_Address.Value;
                New_Wo.woprog_Contact_ID = (int)this.ddl_Contact.Value;
                // New_Wo.woprog_billing_email = Configuration.BillingEmail;

                if (!string.IsNullOrWhiteSpace(this.txt_PONum.Text))
                {
                    New_Wo.PONumber = this.txt_PONum.Text;
                }


                //if (bu.tax_entity_id == 2) // Tax Entity = NE
                //TX.
                //New_Wo.woprog_controller_id = 1611;

                if (Prev_woprog_id != New_Wo.woprog_id)
                {
                    New_Wo.special_instructions = "Old WO#:" + Prev_woprog_id + "\n";
                    EmailContent = "From Old WO#:" + Prev_woprog_id + "\n";
                    EmailContent += "To New WO#:" + New_Wo.woprog_id + "\n";
                }
                if (Prev_CustomerName != New_Wo.CustomerName)
                {
                    New_Wo.special_instructions += "Old Customer:" + Prev_CustomerName + "\n";
                    EmailContent += "From Old Customer:" + Prev_CustomerName + "\n";
                    EmailContent += "To New Customer:" + New_Wo.CustomerName + "\n";
                }

                EmailContent = New_Wo.special_instructions;
                //New_Wo.close_reassign_reason = this.txtCloseReassign.Text;
                NeBusinessUnit BU = new NeBusinessUnit(New_Wo.business_unit_id);

                if (New_Wo.QuoteID == "0")
                {
                    var tempquote2 = new quote((int)this.ddlquote.Value);
                    var quotedamount = Convert.ToDouble(tempquote2.Price);
                    var prev_sell_billed = 0.0;

                    using (var conn = Toolbox.connect())
                    {
                        var dt_prev_billed = Toolbox.doSQL_dt(conn, @"SELECT wo_detail_current_qty_committed qty, wo_detail_current_price_sell sell FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_billtypeid = 11 AND wo_detail_current_master_id = 2139", new object[] { New_Wo.woprog_id });
                        if (dt_prev_billed.Rows.Count > 0)
                        {
                            var dr_prev_billed = dt_prev_billed.Rows[0];

                            prev_sell_billed = Convert.ToDouble(dr_prev_billed["sell"]);
                        }
                    }

                    var this_qty_billed = 1 - Math.Round(prev_sell_billed / quotedamount, 2);
                    Attachequote(New_Wo, BU, "0", (int)this.ddlquote.Value, this_qty_billed);
                }

                string CCTO = "";

                if (Toolbox.ReturnZeroIfNull_int(TX.controller_id) != 0)
                {
                    int i = Toolbox.ReturnZeroIfNull_int(TX.controller_id);
                    NeMember member = new NeMember(i);
                    CCTO = member.NEEmail;
                }

                //Send an email to the billing email against the tax entity, CC'ing the Controller email address to notify them of this change (for impact to margin analysis - Make sure to include the Old & New WO#'s)
                if (!string.IsNullOrEmpty(TX.billing_email))
                {
                    Emailsending(TX.billing_email, CCTO, EmailContent);
                }
                New_Wo.Save();
                break;
        }
    }

    protected void UpdateExistingWO(int Revenueline, int Prev_woprog_id, string prev_CustomerName, int New_woprog_id)
    {

        NeWOProg oldProg = new NeWOProg(Prev_woprog_id);
        NeBusinessUnit BU = new NeBusinessUnit(oldProg.business_unit_id);
        switch (Revenueline)
        {
            case 3: //TM

                NeWOProg.move_status(oldProg, user, "", OpsWOStatus.Enums.ClosedReassigned);
                //Close_Child_Wos(Prev_woprog_id, _current_user, this.txtCloseReassign.Text, New_woprog_id);
                oldProg.close_reassign_reason = this.txtCloseReassign.Text;
                oldProg.Save();

                break;
            case 4: // Quoted

                NeWOProg.move_status(oldProg, user, "", OpsWOStatus.Enums.ClosedReassigned);
                Detachingquote(oldProg, BU, "0", (int)this.ddlquote.Value);
                // Close_Child_Wos(Prev_woprog_id, _current_user, this.txtCloseReassign.Text, New_woprog_id);
                oldProg.close_reassign_reason = this.txtCloseReassign.Text;
                oldProg.Save();
                break;

        }
    }

    public bool CheckWOs(int parent_WO_ID)
    {
        bool result = true;
        var intChildCount = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE parent_woprog_id = @v0 AND woprog_status IN ('Invoiced', 'Deleted')", new object[] { parent_WO_ID });
        if (intChildCount > 0)
        {
            return false;
        }

        var intprogressbillsCount = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE woprog_associate_woprog_id = @v0 AND woprog_status IN ('Invoiced', 'Deleted')", new object[] { parent_WO_ID });
        if (intprogressbillsCount > 0)
        {
            return false;
        }

        var intChangeOrderCount_1 = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE woprog_id IN (SELECT woprog_id_b AS woprog_id FROM wo_associated WHERE woprog_id_a=@v0) AND woprog_status  IN ('Invoiced', 'Deleted')", new object[] { parent_WO_ID });
        var intChangeOrderCount_2 = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE woprog_id IN (SELECT woprog_id_a AS woprog_id FROM wo_associated WHERE woprog_id_b=@v0) AND woprog_status  IN ('Invoiced', 'Deleted')", new object[] { parent_WO_ID });
        if (intChangeOrderCount_1 > 0 || intChangeOrderCount_2 > 0)
        {
            return false;
        }

        return result;
    }


    protected void cb_quoteinfo_Callback(object _source, CallbackEventArgs _e)
    {
        if (_e.Parameter != "0")
        {
            var id = _e.Parameter.Substring(0, 6);
            var rev = _e.Parameter.Substring(6, _e.Parameter.Length - 6);
            var dr = Toolbox.doSQL_dt(@" SELECT job_description, quoted_price, contact_id, quoted_by, IFNULL(address_id,0) address_id, completion_date, net_due, IFNULL(currency, IF(b.country = 'USA', 1, 2)) currency, IFNULL(bdm,0) bdm, IFNULL(acting_bdm,0) acting_bdm FROM quote_master a LEFT join business_unit b ON a.business_unit_id = b.id WHERE quote_id = @v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];

            var job_description = Toolbox.do_value_from(dr["job_description"], false);
            var quoted_price = dr["quoted_price"] == DBNull.Value ? 0 : dr["quoted_price"];
            var contact_id = dr["contact_id"];
            var quoted_by = dr["quoted_by"];
            var address_id = dr["address_id"];
            var completion_date = dr["completion_date"];
            var currency = dr["currency"];
            var net_due = dr["net_due"];
            var bdm = dr["bdm"];
            var acting_bdm = dr["acting_bdm"];

            var quoteterms = fill_terms(id);
            DateTime exp_end;
            DateTime.TryParse(completion_date.ToString(), out exp_end);
            completion_date = exp_end == null ? "" : Toolbox.MySQL_shortdt(exp_end);
            double total_labor = 0;
            var term_id = Toolbox.doSQL_int(@"Select term_id from quote_term where id=@v0 limit 1", new object[] { net_due });
            //this.combo_default_terms.Value = Toolbox.ReturnZeroIfNull_int(term_id);

            //sds_address.SelectCommand = "Select  0 as address_id,'Not Applicable' as address union Select address_id,address as address from vw_address where cust_id = @custid ";
            var dt = Toolbox.doSQL_dt(@"SELECT
  part_no,
  qty
FROM
  quote_worksheet a 
  INNER JOIN quote_section b ON a.section_id = b.id
WHERE a.quote_id = @v0
  AND a.revision = @v1
  AND a.is_checked
  AND b.is_checked
  AND part_no >= 990000", new object[] { id, rev });
            foreach (DataRow _dr in dt.Rows)
            {
                var part_no = 0;
                int.TryParse(_dr["part_no"].ToString(), out part_no);
                var qty = Convert.ToDouble(_dr["qty"]);
                if (part_no > 990000 && part_no < 1000000)
                {
                    total_labor += qty;
                }
                else if (part_no >= 2000000) // Kit.... we need to dissect it.
                {
                    var kit = Toolbox.doSQL_dt(@"SELECT inventory_kit_dtl_master_id master_id, inventory_kit_dtl_qty qty FROM inventory_kit_dtl WHERE inventory_kit_dtl_hdr_id = @v0  AND inventory_kit_dtl_active = TRUE", new object[] { part_no });
                    foreach (DataRow dtl in kit.Rows)
                    {
                        var master_id = (int)dtl["master_id"];
                        var kit_qty = Convert.ToDouble(dtl["qty"]);
                        if (master_id >= 990000)
                        {
                            total_labor += qty * kit_qty;
                        }
                    }
                }
            }
            if (job_description.Trim() == "")
            {
                _e.Result = "Quote's job description is blank, please fix.";
                return;
            }
            else if (quoted_price.ToString() == "")
            {
                _e.Result = "Quote's price is blank, please fix.";
                return;
            }
            else if (contact_id.ToString() == "")
            {
                _e.Result = "Quote's contact is blank, please fix.";
                return;
            }
            else if (quoted_by.ToString() == "")
            {
                _e.Result = "Quote's 'Quoted By' is blank, please fix.";
                return;
            }
            else if (address_id.ToString() == "")
            {
                _e.Result = "Quote's address is blank, please fix.";
                return;
            }
            else if (completion_date.ToString() == "")
            {
                _e.Result = "Quote's completion date is blank, please fix.";
                return;
            }
            else if (total_labor.ToString() == "")
            {
                _e.Result = "Quote's total labor is blank, please fix.";
                return;
            }
            else if (currency.ToString() == "")
            {
                _e.Result = "Quote's currency is blank, please fix.";
                return;
            }
            var j_son_result = string.Format(@"
{{
""job_description"":""{0}"",
""quoted_price"":{1},
""contact_id"":{2},
""quoted_by"":{3},
""address_id"":{4},
""expected_end"":""{5}"",
""expected_hours"":{6},
""currency"":{7},
""term_id"":{8},
""quoteterms"":""{9}"",
""bdm"":""{10}"",
""acting_bdm"":""{11}""
}}",
                job_description.Trim().Replace("\"", "&quot;").Replace("\n", ""),   // {0}
                quoted_price,       // {1}
                contact_id,         // {2}
                quoted_by,          // {3}
                address_id,         // {4}
                completion_date,    // {5}
                total_labor,            // {6}
                currency,  //{7}
                term_id,  //{8}
                quoteterms.Trim().Replace("\"", "&quot;").Replace("\n", ""),  // {9}
                bdm, // {10}
                acting_bdm // {11}
                );
            _e.Result = j_son_result;
        }
    }

    protected void b_warranty_Click(object _sender, EventArgs _e)
    {
        Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_billtypeid = 7  WHERE wo_detail_current_woprog_id=@v0", new object[] { _wo.woprog_id });
        fill_page_info("Line Items");
    }
    protected void bt_delete_wo_Click(object _sender, ImageClickEventArgs _e)
    {
        // Initialize the work order object
        _wo = new NeWOProg(Convert.ToInt32(hidWOProgID.Value));
        #region Check if WO is Progress bill and allow deleting of 2139 lines
        // Check if this is a progress bill, if true delete 2139 lines
        if (_wo.IsProgressBill && (!_wo.IsHistoric || !_wo.IsTerminated))
        {
            //clear lines on progress bill
            Toolbox.doSQL_void(@"DELETE FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_billtypeid =@v1 AND wo_detail_current_master_id = @v2", new object[] { _wo.woprog_id, OpsBillType.ProgressBilling, OpsSpecialPart.QuoteLine });
            //clear lines on the jobcost
            Toolbox.doSQL_void(@"DELETE FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_billtypeid =@v1 AND wo_detail_current_description LIKE CONCAT('%',@v2)", new object[] { _wo.woprog_associate_woprog_id, OpsBillType.ProgressBilling, _wo.woprog_id });
        }
        #endregion Check if WO is Progress bill and allow deleting of 2139 lines
        #region Make sure the work order is still allowed to be deleted
        #region Set up variables
        var curr_wo_count = Toolbox.doSQL_dt(@"SELECT CAST((SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0 ) AS UNSIGNED) curr, CAST((SELECT COUNT(*) FROM wo_detail_history WHERE wo_detail_history_woprog_id = @v0 ) AS UNSIGNED) hist", new object[] { _wo.woprog_id }).Rows[0];
        var curr_wo_line_items = Convert.ToInt32(curr_wo_count["curr"]);
        var hist_wo_line_items = Convert.ToInt32(curr_wo_count["hist"]);
        var curr_wo_comm_lines = curr_wo_line_items > 0 ? Toolbox.doSQL_double(@"SELECT SUM(wo_detail_current_qty_committed) FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0", new object[] { _wo.woprog_id }) != 0 : false;
        var existing_progress_bills = Toolbox.doSQL_dt(@"SELECT WOProg_Associate_WOProg_ID FROM woprog WHERE woprog_id = @v0 AND WOProg_Associate_WOProg_ID != 0", new object[] { _wo.woprog_id });
        var existing_children = Toolbox.doSQL_dt(@"SELECT woprog_id FROM woprog WHERE parent_woprog_id = @v0", new object[] { _wo.woprog_id });
        #endregion Set up variables
        #region In an elevated status = Invoiced or Waiting to be Invoiced
        if (_wo.IsHistoric)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('This work order can't be deleted, the status is: " + _wo.Status + "');", true);
            return;
        }
        #endregion In an elevated status  = Invoiced or Waiting to be Invoiced
        #region Has historic line items
        if (hist_wo_line_items > 0)
        {
            //throw new Exception("This work order can't be deleted, it has items in the history table");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('This work order can't be deleted, it has items in the history table.');", true);
            return;
        }
        #endregion Has historic line items
        #region Has committed work order lines
        if (curr_wo_comm_lines)
        {
            // throw new Exception("This work order can't be deleted, it has items committed to it that need to be removed");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('This work order can't be deleted, it has items committed to it that need to be removed.');", true);
            return;
        }
        #endregion Has committed work order lines
        #region Has existing progress bill
        if (existing_progress_bills.Rows.Count > 0 && curr_wo_comm_lines)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('This work order can't be deleted, it has items associated work order(s).');", true);
            return;
            //throw new Exception("This work order can't be deleted, it has items associated work order(s)");
        }

        #endregion Has existing progress bill
        #region Has children wo
        if (existing_children.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('This work order can't be deleted, it has parent work order(s).');", true);
            return;
            //throw new Exception("This work order can't be deleted, it has parent work order(s)");
        }
        #endregion Has children wo
        #endregion Make sure the work order is still allowed to be deleted
        #region Delete line items in NESI
        Toolbox.doSQL_void(@"DELETE FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0", new object[] { _wo.woprog_id });
        #endregion Delete line items in NESI
        #region Change the status of the work order to OpsWOStatus.Deleted
        Toolbox.doSQL_void(@"UPDATE woprog SET woprog_status = 'Deleted', parent_woprog_id = 0  WHERE woprog_id =@v0 limit 1 ", new object[] { _wo.woprog_id });
        #endregion Change the status of the work order to OpsWOStatus.Deleted
        #region Delete Line items in BV if any exist
        var comp = new NeBusinessUnit(_wo.business_unit_id);
        //if (comp.DSN != "")
        //{
        //    var n_lines = _tools.getSQL_int(@"SELECT COUNT(*) FROM sales_order_detail  WHERE number LIKE  ? ", comp.DSN, new object[] { "%" + _wo.OrderNumber + "%" });
        //    if (n_lines > 0)
        //    {
        //        _tools.getSQL_void(@"DELETE FROM sales_order_detail  WHERE number LIKE  ? ", comp.DSN, new object[] { "%" + _wo.OrderNumber + "%" });
        //    }
        //    #region Delete WO line in BV header
        //    _tools.getSQL_void(@"DELETE FROM sales_order_header  WHERE number LIKE  ? ", comp.DSN, new object[] { "%" + _wo.OrderNumber + "%" });
        //    #endregion Delete WO line in BV header
        //}
        #endregion Delete Line items in BV if any exist
        #region Add status change to Woprogstatus table in NESI
        Toolbox.doSQL_void(@" INSERT INTO woprogstatus ( woprogstatus_woprog_id, woprogstatus_member_id, woprogstatus_status, woprogstatus_datetime ) VALUES ( @v0 , @v1 , 'Deleted', NOW() )", new object[] { _wo.woprog_id, user.id });
        #endregion Add status change to Woprogstatus table in NESI
        // Redirect
        Clear_all();
        ScriptManager.RegisterStartupScript(this, GetType(), "home_script", "top.location.href = '/#/home/12/workorder';", true);
    }
    protected void btnUpdateTaxes_Click(object _sender, EventArgs _e)
    {
        var taxinfo = new NEAddress(Convert.ToInt32(ex_ddl(ddlAddress)));
        var comp = new NeBusinessUnit(_wo.business_unit_id);
        var tax1 = chkTax1.Checked && taxinfo.Tax1Exempt == "" ? taxinfo.Tax1 : 0;
        var tax2 = chkTax2.Checked && taxinfo.Tax2Exempt == "" ? taxinfo.Tax2 : 0;
        var tax3 = chkTax3.Checked && taxinfo.Tax3Exempt == "" ? taxinfo.Tax3 : 0;
        var tax4 = chkTax4.Checked && taxinfo.Tax4Exempt == "" ? taxinfo.Tax4 : 0;
        try
        {
            Toolbox.doSQL_void(@"update woprog  set woprog_tax1 =@v0, woprog_tax2 =@v1 , woprog_tax3 =@v2 , woprog_tax4 =@v3   where woprog_id =@v4", new object[] { tax1, tax2, tax3, tax4, _wo.woprog_id });
            // update labour lines.
            if (comp.TaxLabour == 1)
            {
                Toolbox.doSQL_void(@"update wo_detail_current  set wo_detail_current_tax1 =@v0, wo_detail_current_tax2 =@v1 ,wo_detail_current_tax3 =@v2 , wo_detail_current_tax4 =@v3   where wo_detail_current_type = 'L' and wo_detail_current_woprog_id =@v4", new object[] { tax1, tax2, tax3, tax4, _wo.woprog_id });
            }
            if (comp.TaxMaterial == 1)
            {
                Toolbox.doSQL_void(@"update wo_detail_current  set wo_detail_current_tax1 =@v0, wo_detail_current_tax2 =@v1 ,wo_detail_current_tax3 =@v2 , wo_detail_current_tax4 =@v3   where wo_detail_current_type = 'M' and wo_detail_current_woprog_id =@v4", new object[] { tax1, tax2, tax3, tax4, _wo.woprog_id });
            }
            if (comp.TaxQuotedJobs == 1)
            {
                Toolbox.doSQL_void(@"update wo_detail_current  set wo_detail_current_tax1 =@v0, wo_detail_current_tax2 =@v1 ,wo_detail_current_tax3 =@v2 , wo_detail_current_tax4 =@v3   where wo_detail_current_type = 'Q' and wo_detail_current_woprog_id =@v4", new object[] { tax1, tax2, tax3, tax4, _wo.woprog_id });
            }
        }
        catch
        {
            lblError.Text = "Error updating work order with new tax settings.";
            lblError.ClientVisible = true;
            return;
        }
        Build_Tax_Table();
        update_header_totals();
    }
    protected void btnSaveServiceDateText_ButtonClick(object _source, ButtonEditClickEventArgs _e)
    {
        if (_e.ButtonIndex == 0)
        {
            _wo.SaveServiceDatesText(btnSaveServiceDateText.Text, _wo.woprog_id);
        }
        else
        {
            btnSaveServiceDateText.Text = _wo.RefreshServiceDatesText(_wo.woprog_id);
            // the blow code is used to update all the work roders in one shot.
            //		DataTable dt = Toolbox.doSQL_dt(@"select woprog_id from woprog  where woprog_status != 'Invoiced'" , null);
            //		foreach (DataRow dr in dt.Rows)
            //		{
            //			NeWOProg.RefreshAndSaveServiceDatesText(Convert.ToInt32(dr[0]));
            //
            //			}
        }
    }
    protected void cbp_left_CustomJSProperties(object _sender, CustomJSPropertiesEventArgs _e)
    {
        var wo_type = (int)radiotype.Value;
        if (ddl_creditwolink.Value != null)
        {
            var type = "credit";
            if (wo_type == 2)
            {
                type = "rebill";
            }
            if (wo_type == 0)
            {
                ddl_creditwolink.Value = null;
                return;
            }
            var owo = new NeWOProg(Convert.ToInt32(ddl_creditwolink.Value));
            if (owo.woprog_Expected_EndDate != null)
            {
                _e.Properties["cp_expectedenddate"] = owo.woprog_Expected_EndDate.Year + "," + owo.woprog_Expected_EndDate.Month + "," + owo.woprog_Expected_EndDate.Day;

                if (type == "credit")
                {
                    DateTime now = DateTime.Now.AddDays(14);
                    _e.Properties["cp_expectedenddate"] = now.Year + "," + now.Month + "," + now.Day;
                }
            }
            if (owo.woprog_Expected_StartDate != null)
            {
                _e.Properties["cp_expectedstartdate"] = owo.woprog_Expected_StartDate.Year + "," + owo.woprog_Expected_StartDate.Month + "," + owo.woprog_Expected_StartDate.Day;
                if (type == "credit")
                {
                    DateTime now = DateTime.Now;
                    _e.Properties["cp_expectedstartdate"] = now.Year + "," + now.Month + "," + now.Day;
                }
            }
            _e.Properties["cp_custpo"] = owo.PONumber;
            _e.Properties["cp_description"] = "** Original Work Order # " + owo.OrderNumber + " **" + Environment.NewLine;
            if (type == "credit")
            {
                _e.Properties["cp_salesvalue"] = (owo.woprog_StillToBeBilled * -1).ToString();
                _e.Properties["cp_description"] += "Credit Invoice # " + owo.woprog_InvoiceNo + Environment.NewLine;
                _e.Properties["cp_description"] += "Reason for credit:_____" + Environment.NewLine;
            }
            else
            {
                _e.Properties["cp_salesvalue"] = owo.woprog_StillToBeBilled.ToString();
            }
            _e.Properties["cp_description"] += "Original Description: " + owo.Description;
        }
        else
        {
        }
    }

    protected void btn_CloseAndReassign_Click(object _sender, EventArgs _e)
    {

        popReassignReason.ShowOnPageLoad = true;
        using (var conn = Toolbox.connect())
        {
            DataBindForCustomer(ddl_Customer, conn, hidWOProgID.Value, hidCompanyID.Value, Toolbox.ReturnZeroIfNull_int(ddlCustomer.Value));
            txt_PONum.Text = _wo.PONumber;
            //if(Toolbox.ReturnZeroIfNull_int(ddlCustomer.Value)!=0 && ddl_Address.Items.Count==0)
            //DataBindForAddress(ddl_Address, conn, (int)ddlCustomer.Value);
        }



    }

    protected void DataBindForAddress(ASPxComboBox ddl, MySqlConnection conn, int customerId)
    {
        var sp = "Select  0 as address_id,'Not Applicable' as address union Select address_id,address as address from vw_address where cust_id = @v0 AND active = 1";
        ddl.DataSource = Toolbox.doSQL_dt(conn, sp, new object[] { customerId });
        ddl.DataBind();

    }
    protected void btn_printlaser_Click(object _sender, EventArgs _e)
    {
        validate_wo();
        var print_map = chk_printwhat.Items[0].Selected;
        var print_safety = chk_printwhat.Items[1].Selected;
        var print_signoff = chk_printwhat.Items[2].Selected;
        var print_quote = chk_printwhat.Items[3].Selected;
        var print_picklist = chk_printwhat.Items[4].Selected;
        if (lblError.Text == "")
        {
            if (hidWOProgID.Value == "0")
            {
                CreateWorkOrder(false);
                //create_workorder(false);
                var whatprint = string.Format("map={0}&safety={1}&quote={2}&picklist={3}",
                        print_map,
                        print_safety,
                        print_quote,
                        print_picklist
                        );
                ScriptManager.RegisterStartupScript(this, GetType(), "open_", string.Format("boing('../../sections/workorder/print_laser_WO/index.aspx?woprog_id={0}&{1}','laser_wo',800,800); window.location.href = '/wo_prog_frame.aspx?action=show&woprog_id={0}'", _wo.woprog_id, whatprint), true);
                //
                Clear_all();
            }
            else
            {
                save_workorder();
                var whatprint = string.Format("map={0}&safety={1}&quote={2}&picklist={3}", print_map, print_safety, print_quote, print_picklist);
                if (print_signoff)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "open_", string.Format("boing('../../sections/reports/invoice_preview/index.aspx?id={0}&is_signoff=true','sign_of',800,800)", _wo.woprog_id), true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "open_", string.Format("boing('../../sections/workorder/print_laser_WO/index.aspx?woprog_id={0}&{1}','laser_wo',800,800)", _wo.woprog_id, whatprint), true);
                }
            }
            //	Response.Redirect("/sections/workorder/index.aspx?woprog_id=0&business_unit_id=" + hidCompanyID.Value, true);
        }
        //	
    }
    protected void btn_printdot_Click(object _sender, EventArgs _e)
    {
        validate_wo();
        if (lblError.Text == "")
        {
            if (hidWOProgID.Value == "0")
            {
                CreateWorkOrder(false);
                //create_workorder(false);
            }
            else
            {
                save_workorder();
            }
            if (lblError.Text.Contains("saved") || lblError.Text == "")
            {
                var workorder1 = new NeWOProg(Convert.ToInt32(hidWOProgID.Value));
                var print_doc = new PrintDocument();
                try
                {
                    if (chkProgress.Checked == false)
                    {
                        var wo_printer = new NeBusinessUnit();
                        var business_unit_id = Convert.ToInt32(ex_ddl(ddlCompany));
                        print_doc.PrinterSettings.PrinterName = NeBusinessUnit.GetbuWOPrinter(business_unit_id);
                        if (new List<int>(new int[] { 1, 2, 3, 4, 7, 8, 19, 23, 26, 38 }).Contains(business_unit_id))
                        {
                            print_doc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
                            print_doc.Print();
                        }
                    }
                    Clear_all();
                }
                catch (Exception ex5)
                {
                    var error_message = "Error Printing Work Order : " + workorder1.business_unit_id + " from " + new NeBusinessUnit(workorder1.business_unit_id).DSN + ": " + print_doc.PrinterSettings.PrinterName + " " + ex5 + "<br />";
                    lblError.Text = error_message;
                    lblError.ClientVisible = true;
                    throw;
                }
            }
        }
    }
    protected void btn_printquote_Click(object _sender, EventArgs _e)
    {
    }

    protected void cb_deptcheck_Callback(object _source, CallbackEventArgs _e)
    {
        var woprog_id = _e.Parameter.Trim();

    }
    protected void cb_bench_credit_Callback(object _sender, CallbackEventArgsBase _e)
    {

        if (_e.Parameter != null && _e.Parameter != "")
        {

            try
            {
                Toolbox.doSQL_void(@"Update woprog  set benchmark_credit =@v0  where woprog_id =@v1", new object[] { _e.Parameter, _wo.woprog_id });
            }
            catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
        }

    }
    private void fill_parthistory(int _woprog_id)
    {
        if (user.isContact)
        {
            gv_parthistory.DataSource = null;
            gv_parthistory.DataBind();
            gv_parthistory.Visible = false;
            div_part_history.Visible = false;
            return;
        }
        if (Session["workorder_part_history"] == null)
        {
            _max_part_history_dt = Toolbox.doSQL_dt(@"SELECT MAX(a.id) max_id, a.wo_detail_current_id id FROM log.wo_detail_current a LEFT JOIN member b ON a.wo_detail_current_added_by = b.member_id LEFT JOIN tax c ON a.wo_detail_current_tax1 = c.tax_id LEFT JOIN tax d ON a.wo_detail_current_tax2 = d.tax_id LEFT JOIN tax e ON a.wo_detail_current_tax3 = e.tax_id LEFT JOIN tax f ON a.wo_detail_current_tax4 = f.tax_id LEFT JOIN wo_detail_lineitem_billtype g ON a.wo_detail_current_billtypeid = g.wo_lineitem_billtypeid WHERE a.wo_detail_current_woprog_id = @v0  GROUP BY a.wo_detail_current_id", new object[] { _woprog_id });
            _part_history_dt = Toolbox.doSQL_dt(@" select a.*, b.member_fullname added_by, c.tax_name tax1, d.tax_name tax2, e.tax_name tax3,f.tax_name tax4, g.wo_lineitem_billtype_name billtype from log.wo_detail_current a LEFT JOIN member b on a.wo_detail_current_added_by = b.member_id LEFT JOIN tax c ON a.wo_detail_current_tax1 = c.tax_id LEFT JOIN tax d ON a.wo_detail_current_tax2 = d.tax_id LEFT JOIN tax e ON a.wo_detail_current_tax3 = e.tax_id LEFT JOIN tax f ON a.wo_detail_current_tax4 = f.tax_id LEFT JOIN wo_detail_lineitem_billtype g ON a.wo_detail_current_billtypeid = g.wo_lineitem_billtypeid where a.wo_detail_current_woprog_id = @v0 ", new object[] { _woprog_id });
            var id_origins = new List<int>();
            foreach (DataRow dr in _part_history_dt.Rows)
            {
                var _event = (string)dr["event"];
                var is_insert_update = _event == "INSERT" || _event == "UPDATE";
                var old_new = (string)dr["old_new"];
                var id = Convert.ToInt32(dr["wo_detail_current_id"]);
                if (old_new == "New" && is_insert_update && !id_origins.Contains(id))
                {
                    var max_id = Convert.ToInt32(_max_part_history_dt.Select("id = " + id)[0]["max_id"]);
                    var this_id = Convert.ToInt32(dr["id"]);

                    if (this_id == max_id)
                    {
                        id_origins.Add(id);
                        dr["event"] = "CURRENT";
                    }
                }
            }
            _part_history_dt.DefaultView.Sort = "id";
            Session["workorder_part_history"] = _part_history_dt.DefaultView.ToTable();
        }
        gv_parthistory.DataSource = Session["workorder_part_history"];
        gv_parthistory.DataBind();
    }

    protected void gv_parthistory_HtmlRowPrepared(object _sender, ASPxGridViewTableRowEventArgs _e)
    {
        var gv = (ASPxGridView)_sender;
        var dr = gv.GetDataRow(_e.VisibleIndex);
        if (_e.VisibleIndex >= 0)
        {
        }
    }
    protected void gv_parthistory_HtmlDataCellPrepared(object _sender, ASPxGridViewTableDataCellEventArgs _e)
    {
        if (_e.VisibleIndex >= 0)
        {
            var gv = (ASPxGridView)_sender;
            var dr = gv.GetDataRow(_e.VisibleIndex);
            _e.Cell.ToolTip = _e.DataColumn.FieldName == "wo_detail_current_price_cost" || _e.DataColumn.FieldName == "wo_detail_current_price_sell" ? "" : _e.CellValue.ToString();
            if (_e.DataColumn.FieldName == "wo_detail_current_type")
            {
                var type = _e.CellValue.ToString();
                _e.Cell.Text = type == "M" ? "Mat." : type == "Q" ? "Quote" : type == "L" ? "Lab." : type;
            }
            if (_e.DataColumn.FieldName == "wo_detail_current_master_id")
            {
                var master_id = 0;
                int.TryParse(dr["wo_detail_current_master_id"].ToString(), out master_id);
                if (master_id >= 990000 || master_id == 2139)
                {
                    _e.Cell.Text = _e.CellValue.ToString();
                }
            }
            if (_e.DataColumn.FieldName == "wo_detail_current_price_cost")
            {
                if (!CanViewCost)
                {
                    _e.Cell.Text = "";
                }
            }
            if (_e.DataColumn.FieldName == "wo_detail_current_price_sell")
            {
                if (!CanViewGross)
                {
                    _e.Cell.Text = "";
                }
            }
            var _event = (string)dr["event"];
            var old_new = (string)dr["old_new"];

            switch (_event)
            {
                case "INSERT":
                    _e.Cell.BackColor = ColorTranslator.FromHtml("#ccffcc");
                    break;
                case "UPDATE":
                    switch (old_new)
                    {
                        case "Old":
                            _e.Cell.ForeColor = ColorTranslator.FromHtml("#999999");
                            break;
                        case "New":
                            _e.Cell.ForeColor = ColorTranslator.FromHtml("#009900");
                            _e.Cell.Font.Bold = true;
                            break;
                    }
                    break;
                case "DELETE":
                    _e.Cell.BackColor = ColorTranslator.FromHtml("#ffcccc");
                    break;
            }
            if (_event == "CURRENT" && _e.DataColumn.FieldName == "event")
            {
                _e.Cell.BackColor = ColorTranslator.FromHtml("#009900");
                _e.Cell.ForeColor = ColorTranslator.FromHtml("#ffffff");
                _e.Cell.Font.Bold = true;
            }
        }
    }
    protected void gv_parthistory_DataBound(object sender, EventArgs e)
    {
        gv_parthistory.Columns["Tax #1"].Visible = false;
        gv_parthistory.Columns["Tax #2"].Visible = false;
        gv_parthistory.Columns["Tax #3"].Visible = false;
        gv_parthistory.Columns["Tax #4"].Visible = false;
    }
    protected void btn_refresh_Click(object sender, ImageClickEventArgs e)
    {
        fill_page_info(pc_main.TabPages[pc_main.TabIndex].Text);
    }


    protected void cb_contact_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        if (tbnewcontact.Text == "")
        {
            // throw new Exception("You must enter a valid contact name");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('You must enter a valid contact name.');", true);
            return;
        }
        if (ddltitle.SelectedIndex < 0)
        {
            //throw new Exception("You must select a valid title");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('You must select a valid title.');", true);
            return;
        }
        if ((txtcontactemail.Text == "") && (txtcellphone.Text == ""))
        {
            // throw new Exception("You must enter a valid cell phone or email address");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('You must enter a valid cell phone or email address.');", true);
            return;
        }
        var this_contact = new NEContact();
        this_contact.Contact_Cust_ID = Convert.ToInt32(e.Parameter);
        this_contact.Contact_Type = "Customer";
        this_contact.Contact_Status = "Active";
        this_contact.Contact_Name = tbnewcontact.Text.Trim();
        var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM contact WHERE contact_name = @v0  AND contact_cust_id = @v1  AND contact_type = 'Customer'", new object[] { tbnewcontact.Text.Trim(), e.Parameter });
        if (c > 0)
        {
            // throw new Exception("Duplicate Contact Detected");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Duplicate Contact Detected.');", true);
            return;
        }
        if (ddltitle.Text != null)
        {
            this_contact.Contact_Title = ddltitle.Text;
        }
        if (txtcontactemail.Text != null)
        {
            this_contact.Contact_Email = txtcontactemail.Text;
        }
        if (txtcellphone.Text != null)
        {
            this_contact.Contact_CellPhone = txtcellphone.Text;
        }
        try
        {
            var temp_address_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(address_id),0) FROM address where address_table = 'Customer' AND address_table_id = @v0 ", new object[] { e.Parameter });
            this_contact.address_id = temp_address_id;

            this_contact.AddNEContact(this_contact);
            popc.ShowOnPageLoad = false;
            hid_newcontact_id.Value = this_contact.id.ToString();
            populate_ddlContact(Convert.ToInt32(e.Parameter), this_contact.id);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('There was an error saving this contact.');", true);
            return;
            //  throw new Exception("There was an error saving this contact");
        }

    }

    protected void btSaveAsset_Click(object sender, EventArgs e)
    {
        if (ddlAssetAssignment.SelectedIndex == -1)
        {
            lblAssignmentError.Text = "Please select an asset";
            return;
        }
        if (calAssetDate.Date.Date < DateTime.Now.Date)
        {
            lblAssignmentError.Text = "Please select a valid date needed - Dates cannot be in the past";
            return;
        }
        var woAsset = new NeAssets.WorkOrderUsage
        {
            WOProgId = _wo.woprog_id,
            AddedBy = user.id,
            AssetId = (int)ddlAssetAssignment.Value,
            DateNeeded = calAssetDate.Date
        };
        using (var conn = Toolbox.connect())
        {
            woAsset.Save(conn);
        }
        resetAssetAssignment();
        gv_assetAssignment.DataBind();
    }
    private void resetAssetAssignment()
    {
        ddlAssetAssignment.Value = null;
        calAssetDate.Value = null;
        lblAssignmentError.Text = "";
    }
    private void resetAssetUsage()
    {
        ddlAssetUsageAsset.Value = null;
        ddlAssetUsageUnit.SelectedIndex = -1;
        txtAssetUsageQuantity.Text = "";
        txtAssetUsageSell.Text = "";
        txtAssetUsageError.Text = "";
        lblAssetUsageSellExtd.Text = "";
    }

    protected void txtAssetUsageQuantity_TextChanged(object sender, EventArgs e)
    {
        txtAssetUsageError.Text = "";
        btAssetUsageSave.Enabled = true;
        txtAssetUsageSell.Enabled = true;
        ddlAssetUsageUnit.Border.BorderColor = Color.LightGray;
        if (ddlAssetUsageAsset.Value == null) return;
        var asset = (int)ddlAssetUsageAsset.Value;
        var unit = Convert.ToInt32(ddlAssetUsageUnit.Value);
        var textualUnit = unit == 1 ? "daily" : unit == 2 ? "weekly" : "monthly";
        var qty = txtAssetUsageQuantity.Text == "" ? 0 : Convert.ToDouble(txtAssetUsageQuantity.Text);
        if (qty == 0) return;
        using (var conn = Toolbox.connect())
        {
            var usageObj = new NeAssets.WorkOrderUsage(conn, asset);
            var baseRate = Toolbox.doSQL_double(conn, $@"SELECT {textualUnit} FROM assets WHERE assets_id = @v0", new object[] { usageObj.AssetId });
            var customerRate = Toolbox.doSQL_double(conn, $@"SELECT IFNULL(MAX({textualUnit}), 0) FROM asset_customer_rate WHERE asset_id = @v0 AND CURDATE() BETWEEN start_date AND end_date", new object[] { usageObj.AssetId });
            var rate = customerRate > 0 ? customerRate : baseRate;
            if (rate == 0)
            {
                txtAssetUsageError.Text = "Asset does not have this rate set";
                btAssetUsageSave.Enabled = false;
                txtAssetUsageSell.Enabled = false;
                ddlAssetUsageUnit.Border.BorderColor = Color.Red;
                txtAssetUsageQuantity.Text = "";
                txtAssetUsageSell.Text = "";
                lblAssetUsageSellExtd.Text = "";
                return;
            }
            txtAssetUsageSell.Text = rate.ToString();
            lblAssetUsageSellExtd.Text = (qty * rate).ToString("C2");
        }
    }

    protected void btAssetUsageSave_Click(object sender, EventArgs e)
    {
        using (var conn = Toolbox.connect())
        {
            var error = "";
            var asset = 0;
            if (ddlAssetUsageAsset.Value == null)
            {
                error += "- Asset cannot be empty<br/>";
            }
            else
            {
                asset = (int)ddlAssetUsageAsset.Value;
            }

            var unit = Toolbox.ReturnZeroIfNull_int(ddlAssetUsageUnit.Value);
            if (unit == 0)
            {
                error += "- Unit cannot be empty<br/>";
            }
            double.TryParse(txtAssetUsageQuantity.Text, out double qty);
            double.TryParse(txtAssetUsageSell.Text, out double sell);
            if (qty == 0 || sell == 0)
            {
                if (qty == 0)
                {
                    error += "- Quantity cannot be zero<br/>";
                }
                if (sell == 0)
                {
                    error += "- Sell cannot be zero<br/>";
                }
            }
            txtAssetUsageError.Text = error;
            if (error != "")
            {
                return;
            }

            var usageObj = new NeAssets.WorkOrderUsage(conn, asset);
            var baseAsset = new NeAssets(Convert.ToInt16(usageObj.AssetId));
            var workorderObj = new NeAssets.WorkOrderUsage.AssetWOLine
            {
                AssetObj = baseAsset,
                MemberObj = user,
                Qty = qty,
                Sell = sell,
                Unit = ddlAssetUsageUnit.Text,
                UnitId = Convert.ToInt32(ddlAssetUsageUnit.Value),
                UsageObj = usageObj,
                WorkOrderObj = _wo
            };
            NeAssets.WorkOrderUsage.SaveWOLine(workorderObj);
        }
        resetAssetUsage();
        gvAssetUsage.DataBind();
    }

    protected void ddlAssetUsageUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtAssetUsageQuantity.Border.BorderColor = Color.LightGray;
        var qty = txtAssetUsageQuantity.Text != "" ? Convert.ToDouble(txtAssetUsageQuantity.Text) : 0;
        if (qty > 0)
        {
            txtAssetUsageQuantity_TextChanged(txtAssetUsageQuantity, null);
        }
    }

    protected void ddlAssetUsageAsset_SelectedIndexChanged(object sender, EventArgs e)
    {
        var qty = txtAssetUsageQuantity.Text != "" ? Convert.ToDouble(txtAssetUsageQuantity.Text) : 0;
        if (qty > 0)
        {
            txtAssetUsageQuantity_TextChanged(txtAssetUsageQuantity, null);
        }
        else
        {
            txtAssetUsageQuantity.Text = "";
            txtAssetUsageSell.Text = "";
            lblAssetUsageSellExtd.Text = "";
        }
    }

    protected void gvAssetUsage_DataBound(object sender, EventArgs e)
    {

        var column = gvAssetUsage.Columns["unit_id"] as GridViewDataComboBoxColumn;
        column.PropertiesComboBox.ValueField = "unit_id";
        column.PropertiesComboBox.ValueType = typeof(Int32);
        column.PropertiesComboBox.Items.Add(new ListEditItem("Daily", 1));
        column.PropertiesComboBox.Items.Add(new ListEditItem("Weekly", 2));
        column.PropertiesComboBox.Items.Add(new ListEditItem("Monthly", 3));

        var columnbilltype = gvAssetUsage.Columns["billtype_id"] as GridViewDataComboBoxColumn;
        columnbilltype.PropertiesComboBox.ValueField = "billtype_id";
        columnbilltype.PropertiesComboBox.ValueType = typeof(Int32);
        columnbilltype.PropertiesComboBox.TextField = "billtype";
        columnbilltype.PropertiesComboBox.DataSource = MySqlDataSourceBillType;

    }



    protected void gvAssetUsage_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
    {
        var grid = (ASPxGridView)sender;
        var origin = Toolbox.ReturnBlankIfNull_string(grid.GetRowValues(e.VisibleIndex, "origin"));
        var isImportedWIP = string.Equals(origin, "Imported - WIP", StringComparison.OrdinalIgnoreCase);
        var isImportedCOGS = string.Equals(origin, "Imported - COGS", StringComparison.OrdinalIgnoreCase);
        if (e.ButtonType == ColumnCommandButtonType.Delete && (isImportedWIP || isImportedCOGS))
        {
            e.Enabled = false;
            e.Text = "Imported item";
            // e.Visible = false;
        }
        else
        {
            e.Enabled = true;
            // e.Visible = true;
        }

    }

    protected void calAssetDate_CalendarDayCellPrepared(object sender, CalendarDayCellPreparedEventArgs e)
    {
        if (ddlAssetAssignment.Value == null) return;
        var assetValue = (int)ddlAssetAssignment.Value;
        if (AssetUsage.ContainsKey(assetValue) && AssetUsage[assetValue].Count > 0 && AssetUsage[assetValue].Contains(e.Date))
        {
            e.Cell.Attributes["disabled"] = "disabled";
            e.Cell.Attributes["style"] = "pointer-events:none";
            e.Cell.Style["color"] = "#ccc";
        }
    }

    protected void ddlAssetAssignment_SelectedIndexChanged(object sender, EventArgs e)
    {
        calAssetDate.Value = null;
        lblAssignmentError.Text = "";
    }
    private object handleOldNewValues(OrderedDictionary oldDict, OrderedDictionary newDict, string field)
    {
        return oldDict[field] != newDict[field] ? newDict[field] : oldDict[field];
    }
    protected void gvAssetUsage_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_price_sell = @v0, wo_detail_current_qty_ordered = @v1, wo_detail_current_qty_committed = @v1, wo_detail_current_qty_invoiced = @v1, wo_detail_current_description = @v2, asset_unit = @v3, wo_detail_current_billtypeid=@v4 , activity_code = @v5, client_wo = @v6, client_po = @v7, cost_element = @v8 WHERE wo_detail_current_id = @v9 LIMIT 1",
           new object[]{
            handleOldNewValues(e.OldValues, e.NewValues, "sell"),
            handleOldNewValues(e.OldValues, e.NewValues, "qty"),
            handleOldNewValues(e.OldValues, e.NewValues, "description"),
            handleOldNewValues(e.OldValues, e.NewValues, "unit_id"),
            handleOldNewValues(e.OldValues, e.NewValues, "billtype_id"),
            handleOldNewValues(e.OldValues, e.NewValues, "activity_code"),
            handleOldNewValues(e.OldValues, e.NewValues, "client_wo"),
            handleOldNewValues(e.OldValues, e.NewValues, "client_po"),
            handleOldNewValues(e.OldValues, e.NewValues, "cost_element"),
            e.Keys["id"]
           });
        e.Cancel = true;
        gvAssetUsage.CancelEdit();
        gvAssetUsage.DataBind();
    }

    protected void gvAssetUsage_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
        gvAssetUsage.DataBind();
    }

    protected void Grid_TimeEntries_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.Row.Cells.Count < 6) return;
        e.Row.Cells[0].Width = new Unit("5%");
        e.Row.Cells[0].Style["text-align"] = "center";
        e.Row.Cells[1].Width = new Unit("15%");
        e.Row.Cells[2].Width = new Unit("15%");
        e.Row.Cells[3].Width = new Unit("5%");
        e.Row.Cells[3].Style["text-align"] = "center";
        e.Row.Cells[4].Width = new Unit("10%");
        e.Row.Cells[4].Style["text-align"] = "center";
        e.Row.Cells[5].Width = new Unit("50%");
    }

    protected void btn_export_Click(object sender, EventArgs e)
    {
        ASPxGridViewExporter1.FileName = "WO Time Entries for " + hidWOProgID.Value;

        ASPxGridViewExporter1.WriteXlsxToResponse();
    }

    private void DataBindingForCreditWoLink(string bu, int value)
    {
        /*
               var queryOld = @"
       SELECT WOProg_ID AS id, CONCAT(woprog.woprog_customername,' WO-',WOProg_BVWO, ',  INV-', WOProg_InvoiceNo) AS wo 
       FROM woprog 
       WHERE (business_unit_id = @v0) AND (WOProg_InvoiceNo > 0) AND (WOProg_InvoiceDate > DATE_SUB(CURDATE(),INTERVAL 500 DAY))
       ";
       */

        var query =
            $@"SELECT 
a.WOProg_ID AS id, 
CONCAT(a.woprog_customername,' WO-',a.WOProg_BVWO, ',  INV-', a.WOProg_InvoiceNo) AS wo
FROM woprog a
LEFT JOIN  
internal_companyno b ON b.Internal_CompanyNo_Intranet_CustID = a.woprog_customer_id
WHERE 
a.business_unit_id  IN (@v0) AND 
a.WOProg_InvoiceNo > 0 AND
b.business_unit_id IS NULL AND  
a.WOProg_InvoiceDate > '{ OpsDates.FinancialIntegrationImplementation }'";


        if (value == 0)
        {
            bu = " -1";
        }

        var WorkingBusinessUnit = new NeBusinessUnit(int.Parse(bu));

        // Get all BUs from the same tax entity
        var queryToGetBUs = @"SELECT id FROM business_unit WHERE tax_entity_id = @v0";
        var buList = "";
        var tableForBUs = Toolbox.doSQL_dt(queryToGetBUs, new object[] { WorkingBusinessUnit.tax_entity_id });
        if (tableForBUs == null || tableForBUs.Rows == null || tableForBUs.Rows.Count == 0)
        {
            // do nothing;
        }
        else
        {
            foreach (DataRow r in tableForBUs.Rows)
            {
                var id = r["id"].ToString();
                buList = buList == "" ? id : (buList + "," + id);
            }
        }

        ddl_creditwolink.DataSource = Toolbox.doSQL_dt(query, new object[] { bu, buList });

        ddl_creditwolink.DataBind();
    }

    private int SetDefaultRevenueLine(object quotedObject, int currentValue)
    {
        /*
         * If a quote is linked, the revenue line is set to Quoted
           If a quote is unlinked the revenue line is set to T&M
         */
        var tm = this.GetConfigSettingByQuery(NESI.BLL.Common.Shared.Configuration.GetRevenueLineTM);
        var quoted = this.GetConfigSettingByQuery(NESI.BLL.Common.Shared.Configuration.GetRevenueLineQuoted);
        var defaultTM = Convert.ToInt32(tm);
        var defaultQuoted = Convert.ToInt32(quoted);
        quote q_id;
        var is_tm = 0;
        var newValue = 0;

        if (ddlquote.Value != null && ddlquote.Value.ToString() != "0")
        {

            q_id = new quote(Convert.ToInt32(ddlquote.Value));
            is_tm = Toolbox.doSQL_int(@"Select ifnull(is_tm,0) from quote_master where quote_id = @v0 AND active_revision = 1", new object[] { q_id.QuoteID });

        }
        if (quotedObject == null || Convert.ToInt32(quotedObject) == 0 || string.IsNullOrWhiteSpace(quotedObject.ToString()) || is_tm == 1)
        {
            // no quoted things
            if (currentValue == 0 || currentValue != defaultQuoted || is_tm == 1)
            {
                newValue = defaultTM;

            }
            else
            {
                newValue = currentValue;
            }


        }
        else
        {
            // have a quoted wo
            newValue = defaultQuoted;
        }

        return newValue;
    }

    private string GetConfigSettingByQuery(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return "0";
        }

        var revenue_line_id = Toolbox.doSQL_int(@"SELECT revenue_line_id FROM revenue_line WHERE NAME='" + name + "' limit 1");
        // Toolbox.doSQL_int(@"Select customer_term_id from customer where customer_id=@v0 limit 1", new object[] { cboStatus.Value });
        //if ((revenue_line_id==null)
        //{
        //    return "0";
        //}
        return revenue_line_id.ToString();
    }

    private void CheckWorkorder(NeWOProg wo)
    {
        var list = new List<CheckPoint> { };
        var dataInfo = "";
        var summary = "";
        var template = @"
----------------------------------------------------------
Checking Summary for work order: [{0}]
----------------------------------------------------------";

        var lifetime = 60; // mins
        try
        {
            //
            // Checking wo, workorder Id and quote Id.
            //
            if (wo == null || wo.woprog_id == 0 || string.IsNullOrWhiteSpace(wo.QuoteID))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(wo.QuoteID) && wo.QuoteID == "0")
            {
                return;
            }

            if (wo.woprog_id <= 1000000)
            {
                return;
            }

            var logged = CheckWorkorder_internal_IsAlreadyLogging(wo.woprog_id, DateTime.Now);
            if (logged)
            {
                return;
            }

            // Get the data info.
            dataInfo = CheckWorkorder_internal(wo, list);
            StringBuilder sb = new StringBuilder();
            var summaryHead = string.Format(template, wo.woprog_id);
            sb.Append(dataInfo);
            sb.Append(summaryHead);

            foreach (var record in list)
            {
                var result = record.Passed ? " PASS " : " FAILED ";
                var rowInfo = string.Format(record.Content, result);
                sb.Append("\r\n" + rowInfo);
            }

            int count = list.Where(x => x.Passed == false).Count();
            if (count > 0)
            {
                // Log first.
                Toolbox.do_errorLog_go("Workorder & quote checking", "", sb.ToString());

                // Sending email.
                var e = new NeEMail();
                if (Toolbox.app_setting("debug_redirect") == "1")
                    e.To = Toolbox.app_setting("debug_redirect_email");
                else
                    e.To = "debug@" + Toolbox.app_setting("DomainForEmail");

                e.From = "admin@" + Toolbox.app_setting("DomainForEmail");


                e.Subject = "Work Order & Quote Check -- Notification";
                e.isHTML = true;
                e.Body = sb.ToString().Replace("\r\n", "<br/>");
                e.Send();

                CheckWorkorder_internal_AddedToList(wo.woprog_id, lifetime);
            }

        }
        catch (Exception ex)
        {
        }
    }

    private bool CheckWorkorder_internal_IsAlreadyLogging(int woProgID, DateTime now)
    {
        bool alreadyLogged = false;

        var obj = LoggedWorkOrderList.GetValue(woProgID.ToString());
        if (obj != null)
        {
            alreadyLogged = true;
        }

        return alreadyLogged;
    }

    private void CheckWorkorder_internal_AddedToList(int woProgID, int minutes)
    {
        var added = LoggedWorkOrderList.Add(woProgID.ToString(), woProgID, DateTimeOffset.UtcNow.AddMinutes(minutes));
        if (added)
        {
            return;
        }
        else
        {
            //
            // already existing.
            // extending the life time may be applied by delete & re-add
            //
        }
    }

    private string CheckWorkorder_internal(NeWOProg wo, List<CheckPoint> list)
    {
        //
        // Checking wo, workorder Id and quote Id.
        //
        if (wo == null || wo.woprog_id == 0 || string.IsNullOrWhiteSpace(wo.QuoteID))
        {
            // don't care.
            return "";
        }

        int is_rev = 0, is_quote_id = 0;
        var quote_number = 0;

        var qid = string.IsNullOrWhiteSpace(wo.QuoteID) ? "" : wo.QuoteID;
        var quoteDecodeError = new CheckPoint()
        {
            Content = "[{0}] - Quote Number [ " + qid + " ] that from work order is valid.",
            Passed = true
        };

        bool quoteIdOkay = true;
        try
        {
            quote_number = Convert.ToInt32(wo.QuoteID);
        }
        catch (Exception ex)
        {
            quoteIdOkay = false;
            quoteDecodeError.Passed = false;
        }

        list.Add(quoteDecodeError);

        if (!quoteIdOkay)
        {
            return "";
        }

        if (quote_number == 0)
        {
            return "";
        }

        var quoteLength = new CheckPoint()
        {
            Content = "[{0}] - Quote Number [ " + quote_number + " ] Length is great than 6.",
            Passed = true
        };
        string q = quote_number.ToString();

        list.Add(quoteLength);

        if (q.Length <= 6)
        {
            //
            // Log: Quote ID is too short.
            // 
            quoteLength.Passed = false;
            return "";
        }

        bool decodeQuoteIdOkay = true;
        var withRev = new CheckPoint()
        {
            Content = "[{0}] - Quote Number [ " + quote_number + " ] can be decoded into two parts: id & version.",
            Passed = true
        };
        try
        {
            quote.splice(quote_number, out is_quote_id, out is_rev);
        }
        catch (Exception ex)
        {
            decodeQuoteIdOkay = false;
            withRev.Passed = false;
        }

        list.Add(withRev);
        if (!decodeQuoteIdOkay)
        {
            //
            // Log: Cannot get quoteid and rev.
            // 
            return "";
        }

        //
        // Now we have a good wo#, quoteID and quote_rev.
        //
        StringBuilder sb = new StringBuilder();
        var template = @"Recorded at {0}, id = {1}";
        var logInfo = string.Format(template, DateTime.Now.ToString(), Guid.NewGuid().ToString());
        sb.Append(logInfo);
        var woprogInfo = CheckWorkorder_internal_wo(wo, is_quote_id, is_rev, list);
        if (!string.IsNullOrWhiteSpace(woprogInfo))
        {
            sb.Append(woprogInfo);
        }

        var quoteInfo = CheckWorkorder_internal_quote(wo, is_quote_id, is_rev, list);
        if (!string.IsNullOrWhiteSpace(quoteInfo))
        {
            sb.Append(quoteInfo);
        }

        int c1 = 0;
        var quoteLineItems = CheckWorkorder_internal_detail(wo, is_quote_id, is_rev, list, out c1);
        if (!string.IsNullOrWhiteSpace(quoteLineItems))
        {
            sb.Append(quoteLineItems);
        }

        int c2 = 0;
        var quoteLineItemsFromHistory = CheckWorkorder_internal_history(wo, is_quote_id, is_rev, list, out c2);
        if (!string.IsNullOrWhiteSpace(quoteLineItemsFromHistory))
        {
            sb.Append(quoteLineItemsFromHistory);
        }

        if (c1 == 1 && c2 == 0)
        {
            var check = new CheckPoint()
            {
                Content = "[{0}] - wo_detail_history should have zero 2139 (type=11) Items when wo_detail_current has 1.",
                Passed = true
            };
            list.Add(check);
        }

        if (c1 == 0 && c2 == 1)
        {
            var check = new CheckPoint()
            {
                Content = "[{0}] - wo_detail_current should have zero 2139 (type=11)Items when wo_detail_history has 1.",
                Passed = true
            };
            list.Add(check);
        }

        if (c1 == 0 && c2 == 0)
        {
            var check = new CheckPoint()
            {
                Content = "[{0}] - Found 2139 items either in wo_detail_current or wo_detail_history.",
                Passed = false
            };
            list.Add(check);
        }

        if (c1 > 1)
        {
            var check = new CheckPoint()
            {
                Content = "[{0}] - Found only one 2139  (type=11) item in wo_detail_current.",
                Passed = false
            };
            list.Add(check);
        }

        if (c2 > 1)
        {
            var check = new CheckPoint()
            {
                Content = "[{0}] - Found only one 2139 (type=11) item in wo_detail_history.",
                Passed = false
            };
            list.Add(check);
        }
        return sb.ToString();
    }

    private string CheckWorkorder_internal_wo(NeWOProg wo, int quoteId, int rev, List<CheckPoint> list)
    {
        /*
            Workorder Info
            -----------------------------
            workorderId:
            quoteNumber:
            quoteId:
            rev:
            status:
        */
        var template = @"
----------------------------------------------------------
Work Order Information (table: woprog)
----------------------------------------------------------
work order Id: {0}
quoteNumber: {1}
quoteId: {2}
rev: {3}
status: {4}
order date: {5}
";
        var info = string.Format(
            template,
            wo.woprog_id,
            wo.QuoteID,
            quoteId,
            rev,
            wo.Status,
            wo.OrderDate);
        return info;
    }

    private string CheckWorkorder_internal_quote(NeWOProg wo, int quoteId, int rev, List<CheckPoint> list)
    {
        var quote = new quote(quoteId);
        if (quote == null)
        {
            // Log no quote found
            return "";
        }

        var quoteHasWoInfo = new CheckPoint()
        {
            Content = "[{0}] - Quote (id = " + quoteId + " ) has valid work order (not zero).",
            Passed = true
        };
        list.Add(quoteHasWoInfo);

        if (quote.woprogid == 0)
        {
            // No work order.
            quoteHasWoInfo.Passed = false;
            return "";
        }

        quoteHasWoInfo.Content = quoteHasWoInfo.Content + " #" + wo.woprog_id;

        var sameWorkOrder = new CheckPoint()
        {
            Content = "[{0}] - Work order and quote are linked correctly to each other. Details: Work order #" + wo.woprog_id + " is linked to quote #" + quoteId + ", though quote #" + quoteId + " is linked to Work order #" + quote.woprogid + ".",
            Passed = true
        };

        list.Add(sameWorkOrder);

        if (quote.woprogid != wo.woprog_id)
        {
            // work order not match
            sameWorkOrder.Passed = false;
            return "";
        }

        var template = @"
----------------------------------------------------------
Quote Information (view: quote)
----------------------------------------------------------
quote_Id: {0}
revision: {1}
customer_Id: {2}
status_id: {3}
wo: {4}
po: {5}
job description: {6}
";

        var info = string.Format(template, quote.QuoteID, quote.Revision, quote.cust_id, quote.status, quote.woprogid, quote.cust_po, quote.txtJobDescription);
        return info;
    }

    private string CheckWorkorder_internal_detail(NeWOProg wo, int quoteId, int rev, List<CheckPoint> list, out int c)
    {
        var info = "";
        c = 0;
        var template = @"
----------------------------------------------------------
Quote lines Items from wo_detail_current. Found ({0}) Records.
----------------------------------------------------------
";

        if (wo == null || wo.woprog_id == 0)
        {
            return "";
        }

        var quote_lines = Toolbox.doSQL_dt(
            @"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = 2139 AND wo_detail_current_type = 'Q'",
            new object[] { wo.woprog_id });

        if (quote_lines == null || quote_lines.Rows == null || quote_lines.Rows.Count == 0)
        {
            info = string.Format(template, 0);
            return info;
        }

        var count = quote_lines.Rows.Count;

        var template_row = @"Record [{11}] Info:
***************************************
wo_detail_current_id:     {0}
wo_detail_current_rec_no: {1}
wo_detail_current_type:   {2}
wo_detail_current_master_id:     {3}
wo_detail_current_description:   {4}
wo_detail_current_qty_committed: {5}
wo_detail_current_price_sell:    {6}
wo_detail_current_code:   {7}
wo_detail_current_origin: {8}
wo_detail_current_billtypeid:    {9}
wo_detail_current_added_by_module: {10}
";
        int index = 0;
        info = string.Format(template, count);
        StringBuilder sb = new StringBuilder();
        sb.Append(info);
        foreach (DataRow row in quote_lines.Rows)
        {
            var wodc = new NeWODetailCurrent(Convert.ToInt32(row["wo_detail_current_id"]));
            var rowInfo = string.Format(
                template_row,
                wodc.id,
                wodc.rec_no,
                wodc.type,
                wodc.master_id,
                wodc.description,
                wodc.qty_committed,
                wodc.sell,
                wodc.code,
                wodc.origin,
                wodc.billtypeid,
                wodc.added_by_module,
                index);
            index++;
            sb.Append(rowInfo);

            if (wodc.billtypeid == 11)
            {
                c++;
            }
        }

        return sb.ToString();
    }

    private string CheckWorkorder_internal_history(NeWOProg wo, int quoteId, int rev, List<CheckPoint> list, out int c)
    {
        c = 0;
        var template = @"
----------------------------------------------------------
Quote lines Items from wo_detail_history. Found ({0}) Records.
----------------------------------------------------------
";
        var info = "";

        if (wo == null || wo.woprog_id == 0)
        {
            return "";
        }

        var quote_lines = Toolbox.doSQL_dt(
            @"SELECT * FROM wo_detail_history WHERE wo_detail_history_woprog_id = @v0  AND wo_detail_history_master_id = 2139",
            new object[] { wo.woprog_id });

        if (quote_lines == null || quote_lines.Rows == null || quote_lines.Rows.Count == 0)
        {
            info = string.Format(template, 0);
            return info;
        }

        var count = quote_lines.Rows.Count;

        var template_row = @"Record [{11}] Info:
***************************************
wo_detail_history_id:     {0}
wo_detail_history_rec_no: {1}
wo_detail_history_type:   {2}
wo_detail_history_master_id:     {3}
wo_detail_history_description:   {4}
wo_detail_history_qty_committed: {5}
wo_detail_history_price_sell:    {6}
wo_detail_history_code:   {7}
wo_detail_history_origin: {8}
wo_detail_history_billtypeid:    {9}
wo_detail_history_added_by_module: {10}
";
        int index = 0;
        info = string.Format(template, count);
        StringBuilder sb = new StringBuilder();
        sb.Append(info);
        foreach (DataRow row in quote_lines.Rows)
        {
            var wodc = new NeWODetailHistory(Convert.ToInt32(row["wo_detail_history_id"]));
            var rowInfo = string.Format(
                template_row,
                wodc.id,
                wodc.rec_no,
                wodc.type,
                wodc.master_id,
                wodc.description,
                wodc.qty_committed,
                wodc.price_sell,
                wodc.code,
                wodc.origin,
                wodc.billtypeid,
                wodc.added_by_module,
                index);
            index++;
            sb.Append(rowInfo);

            if (wodc.billtypeid == 11)
            {
                c++;
            }
        }

        return sb.ToString();
    }

    private void ShowOrHideParentWorkOrderDropDownBasedOnProgressBillCheckbox()
    {
        // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1459/
        // Task 1459: Parent WO drop down visible on internal Progress Bills
        if (chkProgress == null)
        {
            return;
        }

        if (chkProgress.Checked)
        {
            // When 'Progress Bill' checkbox is selected, hide the 'Parent work order' line.
            td_parent_info.Style["display"] = "none";
        }
        else
        {
            //
            // When 'Progress Bill' checkbox is unselected, show the 'Parent work order' line.
            //

            if (ddlCustomer == null || ddlCustomer.Value == null)
            {
                // No touch
                return;
            }

            var woCustomer = new NECustomer(Convert.ToInt32(ddlCustomer.Value));
            if (woCustomer.IsNEcompany)
            {
                td_parent_info.Style["display"] = "";
            }
            else
            {
                td_parent_info.Style["display"] = "none";
            }
        }
    }

}
