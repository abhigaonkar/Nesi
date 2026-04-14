using System;
using System.Data;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using nesi.core;
using NESI.Common.Models;

public partial class wo_prog_edit : System.Web.UI.Page
{
    private NeMember _current_user;
    private const int page_id = 12; // from Page table in DB
    private NameValueCollection _q;
    private Toolbox _tools;
    private NeBusinessUnit _working_business_unit;
    private int _used_pm_id;
    private string _business_unit_id;
    private string _dept;
    private string _where_company = "";

    protected void Page_Init(object _sender, EventArgs _e)
    {
        _tools = new Toolbox();
        _q = Request.QueryString;
        _current_user = Toolbox.do_handle_authentication(page_id);
        _business_unit_id = _q["business_unit_id"];


        if (_current_user.business_unit_id == 11 || _current_user.business_unit_id == 48)
        {
            _dept = "%";
        }
        if (!string.IsNullOrEmpty(_business_unit_id) && _business_unit_id != "0")
        {
            _where_company = "and a.business_unit_id = " + _business_unit_id + " ";
        }

        _working_business_unit = _business_unit_id != "0" ? new NeBusinessUnit(Convert.ToInt32(_business_unit_id)) : new NeBusinessUnit();
        Title = "WO Buckets - " + _working_business_unit.name;
        ddlpm.DataSource = Toolbox.doSQL_dt(string.Format(@"
SELECT 
	0 id,
	'All Project Managers' name
UNION ALL
	(
SELECT 
	b.member_id id,
	b.member_fullname name
FROM 
	woprog a
LEFT JOIN 
	member b on a.woprog_pm_memberid = b.member_id
WHERE 
	woprog_status NOT IN ('', 'Invoiced', 'Open', 'Deleted') AND 
	woprog_pm_memberid IS NOT NULL {0}
GROUP BY 
	b.member_id
ORDER BY 
	name
	)", _where_company), null);
        ddlpm.DataBind();
        if (IsPostBack) return;
        if (_current_user.MemberTypeID != 5 && _current_user.MemberTypeID != 34 && _current_user.MemberTypeID != 11 && _current_user.MemberTypeID != 12 && _current_user.MemberTypeID != 29 && _current_user.MemberTypeID != 79)
        {
            ddlpm.SelectedValue = _current_user.id.ToString();
        }
        else
        {
            ddlpm.SelectedIndex = 0;
        }
    }
    protected void Page_Load(object _sender, EventArgs _e)
    {
        using (var conn = Toolbox.connect())
        {
            //Debug.WriteLine("-------------------- ");
            //	Debug.WriteLine("start page load - "+DateTime.Now.ToLongTimeString());
            _used_pm_id = Toolbox.ReturnZeroIfNull_int(ddlpm.SelectedValue);
            if (!string.IsNullOrEmpty(_q["a"]))
            {
                Response.Clear();
                switch (_q["a"])
                {
                    #region xml_get_notes
                    case "xml_get_notes":
                        var pos = Toolbox.doSQL_string(conn, @"SELECT GROUP_CONCAT(DISTINCT po_details_poprog_id) 
FROM po_details_current WHERE po_details_woprog_id =@v0 and is_gl_account=false ", new object[] {
                        _q["woprog_id"]});
                        var po_notes = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT 
	c.member_fullname who, 
	a.poprogcomment_text note, 
	a.poprogcomment_datetime dt, 
	b.poprog_bvpo bvpo,
	d.status_type
FROM 
	poprogcomment a 
LEFT JOIN 
	poprog_header b ON a.poprogcomment_poprog_id = b.poprog_id 
LEFT JOIN
	member c ON a.poprogcomment_member_id = c.member_id
LEFT JOIN
	poprog_status d ON b.poprog_status = d.poprog_status_id
WHERE 
	a.poprogcomment_poprog_id IN ({0}) AND
	b.poprog_status < 4", pos), null);
                        var poxml = new StringBuilder();
                        _tools.set_XML_header();
                        poxml.Append(@"
<notes>");
                        foreach (DataRow dr in po_notes.Rows)
                        {
                            var who = (string)dr["who"];
                            var note = Server.HtmlEncode((string)dr["note"]);
                            var bvpo = (string)dr["bvpo"];
                            var dt = Convert.ToDateTime(dr["dt"]).ToString("U");
                            poxml.Append(@"
	<note>
		<type>PO</type>
		<number>" + bvpo + @"</number>
		<who>" + who + @"</who>
		<dt>" + dt + @"</dt>
		<note_text>" + note + @"</note_text>
	</note>");
                        }
                        poxml.Append(@"
</notes>");
                        Response.Write(poxml.ToString());
                        break;
                        #endregion xml_get_notes
                }
                Response.End();
            }
            var master_page = (IntraDefault)Master;
            if (master_page != null) master_page.page_name = NePage.get_page_name(page_id);
            var menu = new NeMenu(_current_user, Convert.ToInt32(page_id));
            divMenu.InnerHtml = menu.MenuHTML;
            divSide.InnerHtml = shared.PrintSidePanelHTML(_current_user);
            _tools.add_css("/css/wo_prog.css");

            if (string.IsNullOrEmpty(_q["business_unit_id"]))
            {
                Toolbox.FriendlyException(Response, "Business Unit not provided.", "");
            }

            //if (!IsPostBack)  // first time to set up the page
            //	{


            if (_business_unit_id == "0")
            {

                divCoName.InnerHtml = string.Format(@"<div style=''>{0}</div>", "All Branches");
                div_add_new_button.InnerHtml = @"<div style='font-size:14px;font-weight:bold;padding-left:10px'><a href='/sections/workorder/index.aspx?business_unit_id=0&woprog_id=0'> Add  </a></div>";
                div_refresh_button.InnerHtml = @"<div style='font-size:14px;font-weight:bold;'><a href=""javascript:setTimeout('__doPostBack(\'\',\'\')', 0)"">Refresh  </a>";
            }
            else
            {
                divCoName.InnerHtml = string.Format(@"<div style=''>{0}</div>", _working_business_unit.name);
                div_add_new_button.InnerHtml = string.Format(@"<div style='font-size:14px;font-weight:bold;padding-left:10px'><a href='/sections/workorder/index.aspx?business_unit_id={0}&woprog_id=0'> Add  </a></div>", _business_unit_id);
                div_refresh_button.InnerHtml = @"<div style='font-size:14px;font-weight:bold;'><a href=""javascript:setTimeout('__doPostBack(\'\',\'\')', 0)"">Refresh  </a>";

            }

            // submitsA
            if (_q["woprog_id"] != null)
            {
                if (_q["action"] != null && _q["action"] != string.Empty && !IsPostBack)
                {
                    var wo = new NeWOProg(Convert.ToInt32(_q["woprog_id"]));
                    var woBusinessUnit = new NeBusinessUnit(wo.business_unit_id);
                    var doBv = woBusinessUnit.DSN != "";
                    var cust = new NECustomer((int)wo.WOProg_Customer_ID);
                    var n_hist = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM wo_detail_history WHERE wo_detail_history_woprog_id =@v0", new object[] { wo.woprog_id });
                    //if (doBv)
                    //{
                    //    cust.sync_bv(new NeBusinessUnit(wo.business_unit_id).DSN, _current_user);
                    //}
                    if (string.IsNullOrEmpty(_q["ts"]))
                    {
                        Toolbox.FriendlyException(Response, "Work order timestamp not present.", "/wo_prog_frame.aspx?action=show&woprog_id=" + wo.woprog_id + "&business_unit_id=" + wo.business_unit_id);
                    }
                    var ts = Convert.ToInt64(_q["ts"]);
                    if (wo.woprog_ts.Ticks != ts && ts != 711)
                    {
                        Toolbox.FriendlyException(Response, "This work order has been edited since you last loaded it.", "/wo_prog_frame.aspx?action=show&woprog_id=" + wo.woprog_id + "&business_unit_id=" + wo.business_unit_id);
                    }
                    // check for AP Problems1
                    var c_ap = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_woprog_id = @v0 and a.is_gl_account=false AND b.poprog_status = 10", new object[] {
                        wo.woprog_id});
                    switch (_q["action"])
                    {
                        #region wait approval
                        case "waitapproval": // Waiting PM Approval
                            if (n_hist > 0)
                            {
                                throw new Exception("You cannot move a work order that is either in 'Waiting to be Invoiced', or 'Invoiced' to Waiting . Please reload the work order and try again");
                            }
                            if (wo.Status == "Invoiced" || wo.Status == "Waiting to be Invoiced")
                            {
                                Toolbox.FriendlyException(Response, "This work order has been moved since you last loaded this page, and cannot be approved.", "/wo_prog_frame.aspx?action=show&woprog_id=" + wo.woprog_id + "&business_unit_id=" + wo.business_unit_id);
                            }
                            if (c_ap > 0)
                            {
                                Toolbox.FriendlyException(Response, "Cannot move this work order a linked purchase order is in AP Problems.", "/wo_prog_frame.aspx?action=show&woprog_id=" + wo.woprog_id + "&business_unit_id=" + wo.business_unit_id);
                            }
                            NeWOProg.check_before_move(wo, OpsWOStatus.WaitingPMApproval);
                            NeWOProg.move_status(wo, _current_user, "", OpsWOStatus.Enums.WaitingPMApproval);
                            break;
                        #endregion wait approval
                        #region waitpo
                        case "waitpo":
                            NeWOProg.move_status(wo, _current_user, "yes", OpsWOStatus.Enums.WaitingForPO);

                            break;
                        #endregion waitpo
                        #region rework
                        case "rework":
                            try
                            {
                                NeWOProg.check_before_move(wo, OpsWOStatus.Rework);
                            }
                            catch (Exception check_move_error)
                            {
                                Toolbox.FriendlyException(this.Response, check_move_error.Message, this.Page.Request.Url.AbsoluteUri);
                                return;
                            }
                            if (n_hist > 0)
                            {
                                throw new Exception("You cannot move a work order that is either in 'Waiting to be Invoiced', or 'Invoiced' to Reworks. Please reload the work order and try again");
                            }
                            wo.woprog_custpo_dt = DateTime.Now;
                            NeWOProg.move_status(wo, _current_user, "", OpsWOStatus.Enums.Rework);
                            break;
                        #endregion rework
                        #region approve
                        case "approve": // PM Approval
                            try
                            {
                                NeWOProg.check_before_move(wo, OpsWOStatus.WaitingBMApproval);
                            }
                            catch (Exception check_move_error)
                            {
                                Toolbox.FriendlyException(Response, check_move_error.Message, string.Format("/sections/workorder/index.aspx?woprog_id={0}&is_n1=true&business_unit_id={1}", wo.woprog_id, wo.business_unit_id));
                                return;
                            }
                            if (n_hist > 0)
                            {
                                throw new Exception("You cannot move a work order that is either in 'Waiting to be Invoiced', or 'Invoiced' to BM Approval. Please reload the work order and try again");
                            }
                            wo.woprog_custpo_dt = DateTime.Now;
                            NeWOProg.move_status(wo, _current_user, "", OpsWOStatus.Enums.WaitingBMApproval);
                            if (Convert.ToString(Session["allowpopup"]) == "true" && Page.Request.Browser.IsMobileDevice)
                            {
                                Toolbox.FriendlyPopup(Response, wo.OrderNumber + " has been approved from the email notification", "/default.aspx", "Work Order Has Been Approved");
                            }
                            break;
                        #endregion approve
                        #region approvebranch
                        case "approveBranch": // BM/BuM approval
                            try
                            {
                                NeWOProg.check_before_move(wo, OpsWOStatus.WaitingParentBMApproval);
                            }
                            catch (Exception check_move_error)
                            {
                                Toolbox.FriendlyException(this.Response, check_move_error.Message, this.Page.Request.Url.AbsoluteUri);
                                return;
                            }
                            if (n_hist > 0)
                            {
                                throw new Exception("You cannot move a work order that is either in 'Waiting to be Invoiced', or 'Invoiced' to 'Waiting to be Invoiced'. Please reload the work order and try again");
                            }
                            wo.woprog_custpo_dt = DateTime.Now;
                            var parent_wo = wo.parent_woprog_id > 1 ? new NeWOProg(wo.parent_woprog_id) : new NeWOProg();
                            if (wo.parent_woprog_id > 1 && wo.approved_by_parent_member_id == 0 && parent_wo.business_unit_id != wo.business_unit_id)
                            {
                                NeWOProg.move_status(wo, _current_user, "", OpsWOStatus.Enums.WaitingParentBMApproval);
                            }
                            else
                            {
                                try 
                                { 
                                    NeWOProg.move_status(wo, _current_user, "", OpsWOStatus.Enums.WaitingToBeInvoiced);
                                } 
                                catch(Exception e)
                                {
                                    Response.Redirect($"/sections/workorder/index.aspx?woprog_id={wo.woprog_id}&business_unit_id={_business_unit_id}&error={e.Message}");
                                }
                            }

                            break;
                        #endregion approvebranch
                        #region questions
                        case "questions":
                            wo.woprog_custpo_dt = DateTime.Now;
                            try
                            {
                                NeWOProg.check_before_move(wo, OpsWOStatus.QuestionsForPM);
                            }
                            catch (Exception check_move_error)
                            {
                                Toolbox.FriendlyException(this.Response, check_move_error.Message, this.Page.Request.Url.AbsoluteUri);
                                return;
                            }
                            NeWOProg.move_status(wo, _current_user, "", OpsWOStatus.Enums.QuestionsForPM);
                            break;
                        #endregion questions
                        #region invoice
                        case "invoice":
                            try
                            {
                                if (wo.Status == "Invoiced")
                                {
                                    throw new Exception("This work order has been moved since you last loaded this page, and cannot be approved.");
                                }
                                if (c_ap > 0)
                                {
                                    throw new Exception("Cannot move this work order a linked purchase order is in AP Problems");
                                }
                                // move local 
                                var fileServer = NeTaxEntity.BaseFolder(wo.business_unit_id, false);
                                var str_w_os_path = fileServer + @"\WOs\";
                                var str_from = string.Format("{0}{1}.pdf", str_w_os_path, _q["woprog_id"]);
                                var str_to = string.Format("{0}Invoiced\\{1}.pdf", str_w_os_path, _q["woprog_id"]);
                                File.Move(str_from, str_to);
                                // Move remote scan folder 
                                //File.Copy(@"C:\Inetpub\neintranet\WOs\Invoiced\" + Request.QueryString["woprog_id"] + ".pdf", drCompany["Company_WOPath"].ToString() + "Invoiced\\" + strNewName + ".pdf");
                            }
                            catch (Exception ex)
                            {
                                Toolbox.do_errorLog_errorStack(ex);
                                spanMSG.InnerHtml = string.Format("Error Invoicing ID: {0}<br>{1}", _q["woprog_id"], ex.Message);
                            }
                            //insert db
                            NeWOProg.add_history(wo.woprog_id, _current_user.id, "Invoiced", Toolbox.MySQLNow_long());
                            Toolbox.doSQL_void(conn, @"UPDATE WOProg SET WOProg_CloseDateTime=NOW(), WOProg_Status = 'Invoiced' WHERE  WOProg_ID =@v0", new object[] { _q["woprog_id"] });
                            Toolbox.doSQL_void(conn, string.Format(@"UPDATE passport SET active = false, valid = true WHERE url_yes LIKE ""%woprog_id={0}&%""", wo.woprog_id), null);

                            break;
                            #endregion invoice
                    }
                    Response.Redirect("/#/home/12/workorder/buckets/" + _business_unit_id);
                }
            }
            //just scanned ////////////////////////////////////////////////////////////////////
            var sb_just_scanned = new StringBuilder();
            NeBusinessUnit.CheckBUProcessFolderStructure(_working_business_unit.id);
            try
            {
                var dir_just = new DirectoryInfo(_working_business_unit.WOPath);
                var arr_files = dir_just.GetFiles();
                if (!_current_user.isContact)
                {
                    foreach (var t in arr_files)
                    {
                        if (!t.Extension.Contains("pdf") && !t.Extension.Contains("PDF")) continue;
                        var filename = Server.UrlEncode(t.Name);
                        var size = t.Length / 1000;
                        var str_created = "<img src='/images/ext/pdf.png' width='14' height='14' align='absmiddle' /> " + t.CreationTime.ToString("MM-dd HH:mm:ss");
                        var url = string.Format("wo_prog_frame.aspx?action=justscan&scanfile={0}&business_unit_id={1}&navpanes=0", filename, _business_unit_id);
                        sb_just_scanned.AppendFormat(@"<div class='opt' data-tooltip=""Filename: {2}<br/>Size: {3:N0} KB<br/>Created: {4:f}<br/>Last Accessed: {5:f}"" data-url=""{1}"" data-title=""File information"">{0}</div>", str_created, url, filename, size, t.CreationTime, t.LastAccessTime);
                    }
                }
                div_list_just_scanned.InnerHtml = sb_just_scanned.ToString();
            }
            catch (Exception e)
            {

                div_list_just_scanned.InnerHtml = "Unable to connect to " + _working_business_unit.WOPath;
            }



            update_page();
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object _sender, EventArgs _e)
    {
        update_page();
    }
    protected void ddlpm_SelectedIndexChanged(object _sender, EventArgs _e)
    {
        update_page();
    }
    protected void update_page()
    {
        using (var conn = Toolbox.connect())
        {
            var q = Request.QueryString;
            double var_total_pm_approval = 0;
            double var_total_bm_approval = 0;
            double var_total_initial_prep = 0;
            double var_total_cust_po = 0;
            double var_total_rework = 0;
            double var_total_questions = 0;
            double var_total_waiting_invoice = 0;
            double var_total_waiting_other_branch = 0;
            double var_open_pos = 0;

            var custid = "%";
            if (_current_user.isContact)
            {
                custid = _current_user.customerID.ToString();
            }
            double headertotal;
            var can_viewtotaltime = _current_user.AuthenticatedForPrivilege(60);
            var business_unit_id = "";
            if (q["business_unit_id"] != null)
            {
                business_unit_id = q["business_unit_id"];
            }

            if (business_unit_id == "")
            {
                throw new Exception("Company ID not defined");
            }
            var sb_initial_prep = new StringBuilder();
            var sb_rework = new StringBuilder();
            var sb_questions = new StringBuilder();
            var sb_pm_approval = new StringBuilder();
            var sb_bm_approval = new StringBuilder();
            var sb_waiting_invoice = new StringBuilder();
            var sb_cust_po = new StringBuilder();
            var sb_open_pos = new StringBuilder();
            var sb_waiting_other_branch = new StringBuilder();

            div_list_initial_prep.InnerHtml = "";
            div_list_cust_po.InnerHtml = "";
            div_list_rework.InnerHtml = "";
            div_list_pm_approval.InnerHtml = "";
            div_list_bm_approval.InnerHtml = "";
            div_list_waiting_invoice.InnerHtml = "";
            div_list_open_pos.InnerHtml = "";
            div_list_questions.InnerHtml = "";
            div_list_waiting_other_branch.InnerHtml = "";

            // get all  OPEN WOs            /////////////////////////////////////////////////////////////////////
            var orderby = DropDownList1.SelectedItem.Value;
            var dt_bmapproval = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT 
	a.*, 
	MEMBER_NAME(woprog_pm_memberid) pm_name,
	IFNULL(DATEDIFF(CURDATE(),woprog_opendatetime), 0) days_since,
	c.ddl_name 
FROM 
	woprog a 
INNER JOIN 
	business_unit c ON a.business_unit_id = c.id 
WHERE 
	a.business_unit_id ={0}  and woprog_closedatetime IS NULL AND woprog_status = 'Waiting BM Approval' and woprog_customer_id like '{2}' 

union
SELECT
	wo_child.*, 
	MEMBER_NAME (wo_child.woprog_pm_memberid) pm_name,
	IFNULL(DATEDIFF(CURDATE(),wo_child.woprog_opendatetime),0) days_since,
	c.ddl_name 
FROM
	woprog wo_child 
inner join 
	woprog wo_parent on wo_child.parent_woprog_id = wo_parent.WOProg_ID 
INNER JOIN 
	business_unit c ON wo_child.business_unit_id = c.id 
WHERE
	wo_parent.business_unit_id ={0} and wo_child.woprog_status = 'Waiting Parent BM Approval'
	
UNION
	SELECT
	wo_child.*, 
	MEMBER_NAME (wo_child.woprog_pm_memberid) pm_name,
	IFNULL(DATEDIFF(CURDATE(),wo_child.woprog_opendatetime),0) days_since,
	c.ddl_name
FROM
	woprog wo_child 
LEFT JOIN
	internal_companyno int_co ON wo_child.woprog_customer_id = int_co.internal_companyno_intranet_custid 
LEFT JOIN 
	business_unit c ON wo_child.business_unit_id = c.id
WHERE 
	wo_child.parent_woprog_id = 1 AND
	int_co.business_unit_id = {0} AND 
	wo_child.woprog_status = 'Questions For PM'
ORDER BY {1}, WOProg_id", business_unit_id, orderby, custid, _dept), null);



            // fill the bm approval with all work orders...
            var last_id = 0;
            foreach (DataRow dr in dt_bmapproval.Rows)
            {
                #region loop

                var wo = new wo_vars
                {
                    id = Convert.ToInt32(dr["woprog_id"]),

                    branch = dr["ddl_name"].ToString(),
                    associate_woprog_id = Convert.ToInt32(dr["woprog_associate_woprog_id"]),
                    pm_name = dr["pm_name"].ToString(),
                    status = (string)dr["woprog_status"],
                    bvwo = (string)dr["woprog_bvwo"],
                    cust_po = (string)dr["woprog_custpo"],
                    customer_name = (string)dr["woprog_customername"],
                    description = (string)dr["woprog_description"],
                    to_be_billed = Convert.ToDouble(dr["woprog_stilltobebilled"]),
                    total_tandm = Convert.ToDouble(dr["woprog_totaltandm"]),
                    hold = Convert.ToBoolean(dr["woprog_hold"]),
                    is_creditcard = Convert.ToBoolean(dr["woprog_creditcard_payment"]),
                    quote_id = dr["woprog_quoteid"].ToString() == "" ? 0 : Convert.ToInt32(dr["woprog_quoteid"]),
                    days_since = Convert.ToInt32(dr["days_since"]),
                    parent_woprog_id = Convert.ToInt32(dr["parent_woprog_id"]),
                    business_unit_id = Convert.ToInt32(dr["business_unit_id"]),
                    pm_id = Convert.ToInt32(dr["WOProg_PM_MemberID"])
                };
                if (last_id != wo.id)
                {
                    headertotal = Toolbox.ReturnZeroIfNull_double(wo.to_be_billed);
                    var fore_color = "color:#000";
                    string bg_color;



                    if (wo.hold)
                    {
                        bg_color = "background-color:#fcc";
                    }
                    else if (wo.parent_woprog_id > 1)
                    {

                        bg_color = wo.business_unit_id.ToString() == business_unit_id ? "background-color:yellow" : "background-color:orange";
                    }
                    else if (wo.associate_woprog_id != 0)
                    {
                        bg_color = "background-color:#fec";
                    }
                    else if (wo.quote_id != 0)
                    {
                        bg_color = "background-color:#0ff";
                    }
                    else
                    {
                        bg_color = "background-color:#fff";
                    }
                    var fontbold = wo.is_creditcard ? "font-weight:bold;" : "";
                    var laststatus_str = "";
                    try
                    {
                        var temp_dt = Toolbox.doSQL_dt(conn, @"
SELECT 
	datediff(curdate(),woprogstatus_datetime) dt, 
	member_name(woprogstatus_member_id) name 
FROM 
	woprogstatus 
WHERE 
	woprogstatus_woprog_id = @v0 AND 
	woprogstatus_status = (SELECT woprog_status FROM woprog WHERE woprog_id = @v0) ORDER BY woprogstatus_datetime DESC LIMIT 1", new object[] { dr["woprog_id"] });
                        if (temp_dt.Rows.Count > 0)
                        {
                            var laststatus = temp_dt.Rows[0];
                            var lastupdated_by = laststatus["name"].ToString();
                            var lastupdated_dt = laststatus["dt"].ToString();
                            laststatus_str = string.Format(@"
<tr>
	<td><strong>Last Status Change:</strong></td>
	<td>{0}</td>
</tr>
<tr>
	<td><strong>Last Status Changed by:</strong></td>
	<td>{1}</td>
</tr>", lastupdated_dt, lastupdated_by);
                        }
                    }
                    catch
                    {
                        laststatus_str = "";
                    }

                    var tooltip = string.Format(@"
<table width='100%'>
	<tr>
		<td width='150'><strong>Status</strong></td>
		<td>{10}</td>
	</tr>
	<tr>
		<td width='150'><strong>Business Unit</strong></td>
		<td>{11}</td>
	</tr>
	<tr>
		<td width='150'><strong>Customer Name</strong></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td><strong>Days Since Scanned</strong></td>
		<td>{2}</td>
	</tr>
{6}
	<tr>
		<td><strong>Still to be Billed</strong></td>
		<td>{7:C}</td>
	</tr>
	<tr>
		<td><strong>PO:</strong></td>
		<td>{9}</td>
	</tr>
	<tr>
		<td><strong>Project Manager</strong></td>
		<td>{4}</td>
	</tr>
	<tr>
		<td><strong>Department</strong></td>
		<td>{5}</td>
	</tr>
	<tr>
		<td><strong>On Hold</strong></td>
		<td>{8}</td>
	</tr>
{6}
	<tr>
		<td colspan='3' valign='top'><br/><br/><strong>WO Description</strong></td>
	</tr>
	<tr>
		<td colspan='3' style='background-color:#000;padding:5px;'>{1}</td>
	</tr>
</table>",
        Server.HtmlEncode(wo.customer_name),                                    // {0}
        Server.HtmlEncode(Toolbox.do_value_from(wo.description, false)),        // {1}
        wo.days_since,                                                              // {2}
        can_viewtotaltime ? wo.total_tandm : 0,                                     // {3}
        wo.pm_name,                                                                 // {4}
        "",                                                                 // {5}
        laststatus_str,                                                         // {6}
        can_viewtotaltime ? wo.to_be_billed : 0,                                    // {7}
        wo.hold,                                                                    // {8}
        wo.cust_po,                                                                 // {9}
        wo.status,                                                                  // {10}
        wo.branch                                                                   // {11}
        ).Replace(Environment.NewLine, string.Empty);
                    if (_used_pm_id == 0 || _used_pm_id == wo.pm_id)
                    {
                        var url = string.Format("./wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}", wo.id, business_unit_id);
                        if (wo.parent_woprog_id > 1)
                        {
                            if (wo.business_unit_id.ToString() == business_unit_id)
                            {
                                sb_bm_approval.AppendFormat(@"<div class='opt' data-tooltip=""{2}"" data-url=""{5}"" style=""{3};{4};{6}"">{0} - {1}</div>", dr["WOProg_BVWO"], wo.customer_name, tooltip, fore_color, bg_color, url, fontbold);
                            }
                            else
                            {
                                url = string.Format("javascript:boing('/sections/reports/invoice_preview/frame.aspx?id={0}&is_child_approval=true&is_signoff=false','WO_Approval',1200,800);", wo.id);
                                sb_bm_approval.AppendFormat(@"<div class='opt' data-tooltip=""{2}"" data-url=""{5}"" style=""{3};{4};{6}"">{0} - {1}</div>", dr["WOProg_BVWO"], wo.customer_name, tooltip, fore_color, bg_color, url, fontbold);

                            }
                        }
                        else
                        {
                            sb_bm_approval.AppendFormat(@"<div class='opt' data-tooltip=""{2}"" data-url=""{5}"" style=""{3};{4};{6}"">{0} - {1}</div>", dr["WOProg_BVWO"], wo.customer_name, tooltip, fore_color, bg_color, url, fontbold);
                        }
                        var_total_bm_approval += headertotal;
                        last_id = wo.id;
                    }
                }
                #endregion loop
            } // End Foreach
            DataTable dt_open;
            if (_used_pm_id > 0)
            {
                dt_open = Toolbox.doSQL_dt(conn, string.Format("SELECT *, MEMBER_NAME(woprog_pm_memberid) pm_name, IFNULL(DATEDIFF(CURDATE(),woprog_opendatetime), 0) days_since, c.name branch FROM WOProg a left join business_unit c on a.business_unit_id = c.id WHERE business_unit_id ={0} AND WOProg_PM_MemberID = {1} AND WOProg_CloseDateTime IS NULL AND WOProg_Status != 'Open' and woprog_customer_id like '{3}' ORDER BY {2},WOProg_id DESC", business_unit_id, _used_pm_id, orderby, custid), null);
            }
            else
            {
                dt_open = Toolbox.doSQL_dt(conn, string.Format(@"
				SELECT
woprog_id,woprog_associate_woprog_id,woprog_pm_memberid,
woprog_status,woprog_bvwo,woprog_bvwo,woprog_custpo,woprog_customername,woprog_description,
woprog_stilltobebilled,woprog_totaltandm,woprog_hold,woprog_creditcard_payment,woprog_quoteid,
parent_woprog_id,a.business_unit_id,
c.ddl_name,
 member_fullname as pm_name,
	IFNULL(
		DATEDIFF(
			CURDATE(),
			woprog_opendatetime
		),
		0
	) days_since
FROM
	WOProg a inner join member on member_id = woprog_pm_memberid INNER JOIN business_unit c ON a.business_unit_id = c.id
WHERE
	a.business_unit_id ={0}  
AND WOProg_CloseDateTime IS NULL
AND WOProg_Status != 'Open' and WOProg_Status != 'Deleted' and woprog_customer_id like '{2}' 
ORDER BY {1},WOProg_id DESC", business_unit_id, orderby, custid), null);

            }

            foreach (DataRow dr in dt_open.Rows)
            {
                #region open PO's

                var wo = new wo_vars
                {
                    id = Convert.ToInt32(dr["woprog_id"]),

                    associate_woprog_id = Convert.ToInt32(dr["woprog_associate_woprog_id"]),
                    pm_name = dr["pm_name"].ToString(),
                    status = (string)dr["woprog_status"],
                    bvwo = (string)dr["woprog_bvwo"],
                    branch = dr["ddl_name"].ToString(),
                    cust_po = (string)dr["woprog_custpo"],
                    customer_name = (string)dr["woprog_customername"],
                    description = (string)dr["woprog_description"],
                    to_be_billed = Convert.ToDouble(dr["woprog_stilltobebilled"]),
                    total_tandm = Convert.ToDouble(dr["woprog_totaltandm"]),
                    hold = Convert.ToBoolean(dr["woprog_hold"]),
                    is_creditcard = Convert.ToBoolean(dr["woprog_creditcard_payment"]),
                    quote_id = dr["woprog_quoteid"].ToString() == "" ? 0 : Convert.ToInt32(dr["woprog_quoteid"]),
                    days_since = Convert.ToInt32(dr["days_since"]),
                    parent_woprog_id = Convert.ToInt32(dr["parent_woprog_id"]),
                    business_unit_id = Convert.ToInt32(dr["business_unit_id"])
                };

                if (last_id != wo.id)
                {
                    headertotal = Toolbox.ReturnZeroIfNull_double(wo.to_be_billed);

                    var dt_hold_pos = Toolbox.doSQL_dt(conn, @"
SELECT 
	b.poprog_id id, 
	b.poprog_bvpo bvpo ,
	c.vendor_name 
FROM
	po_details_current a
LEFT JOIN 
	poprog_header b 
		ON a.po_details_poprog_id = b.poprog_id
LEFT JOIN
	vendor c
		ON b.poprog_vendor_id = c.vendor_id
WHERE 
	b.poprog_status IN (1,2,5,3,9) AND 
	a.po_details_woprog_id =@v0 AND 
	a.po_details_line_active = 1 and
    a.is_gl_account=false
GROUP BY
	id
", new object[] { wo.id });
                    var laststatus_str = "";
                    try
                    {
                        var c = Toolbox.doSQL_int(conn, @"
SELECT 
	COUNT(woprogstatus_id)  
FROM 
	woprogstatus 
WHERE 
	woprogstatus_woprog_id = @v0 AND 
	woprogstatus_status = (SELECT woprog_status FROM woprog WHERE woprog_id = @v0)", new object[] {
                            wo.id});
                        if (c > 0)
                        {
                            var laststatus = Toolbox.doSQL_dt(conn, @"
SELECT 
	datediff(curdate(),woprogstatus_datetime) dt, 
	member_name(woprogstatus_member_id) name 
FROM 
	woprogstatus 
WHERE 
	woprogstatus_woprog_id = @v0 AND 
	woprogstatus_status = (SELECT woprog_status FROM woprog WHERE woprog_id =@v0) ORDER BY woprogstatus_datetime DESC LIMIT 1", new object[] { wo.id }).Rows[0];
                            var lastupdated_by = laststatus["name"].ToString();
                            var lastupdated_dt = laststatus["dt"].ToString();
                            laststatus_str = string.Format(@"
<tr>
	<td><strong>Days in this bucket:</strong></td>
	<td>{0}</td>
</tr>
<tr>
	<td><strong>Last Status Changed by:</strong></td>
	<td>{1}</td>
</tr>", lastupdated_dt, lastupdated_by);
                        }
                    }
                    catch
                    {
                        laststatus_str = "";
                    }
                    string po_list;
                    if (dt_hold_pos.Rows.Count > 0)
                    {
                        var open_pos_sb = new StringBuilder();
                        open_pos_sb.Append(@"
	<tr>
		<td colspan='2' valign='top'><br/><strong>Open Purchase Orders</strong></td>
	</tr>
	<tr>
		<td colspan='2' style='background-color:#047;padding:5px;'>");
                        foreach (DataRow dr_hold_po in dt_hold_pos.Rows)
                        {
                            var id = dr_hold_po["id"];
                            var bvpo = "0" + dr_hold_po["bvpo"].ToString().TrimStart('0');
                            var vend = dr_hold_po["vendor_name"].ToString();
                            if (vend.Length > 25)
                            {
                                vend = vend.Substring(0, 24) + "...";
                            }
                            open_pos_sb.AppendFormat(@"
					<div><a style='color:#fff;' href='javascript:load_po({0});'>{1} - {2}</a></div>", id, bvpo, vend);
                        }
                        open_pos_sb.Append(@"
			</td> </tr>");
                        po_list = open_pos_sb.ToString();
                    }
                    else
                    {
                        po_list = "";
                    }

                    var tooltip = string.Format(@"
<table cellspacing='0' cellpadding='1' width='100%'>
	<tr>
		<td width='150'><strong>Status</strong></td>
		<td>{10}</td>
	</tr>
	<tr>
		<td width='150'><strong>Business Unit</strong></td>
		<td>{12}</td>
	</tr>
	<tr>
		<td width='150'><strong>Customer Name</strong></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td><strong>Days Since Scanned</strong></td>
		<td>{2}</td>
	</tr>
{6}
	<tr>
		<td><strong>Still to be Billed</strong></td>
		<td>{7:C}</td>
	</tr>
	<tr>
		<td><strong>PO:</strong></td>
		<td>{9}</td>
	</tr>
	<tr>
		<td><strong>Project Manager</strong></td>
		<td>{4}</td>
	</tr>
	<tr>
		<td><strong></strong></td>
		<td></td>
	</tr>
   <tr>
		<td><strong>On Hold</strong></td>
		<td>{8}</td>
	</tr>
	<tr>
		<td colspan='2' valign='top'><br/><strong>Work Order Description</strong></td>
	</tr>
	<tr>
		<td colspan='2' style='background-color:#000;padding:5px;'>{1}</td>
	</tr>
{11}
</table>",
        Server.HtmlEncode(wo.customer_name),                                // {0}
        Server.HtmlEncode(wo.description).Replace("\n", "<br/>"),   // {1}
        wo.days_since,                                                          // {2}
        wo.total_tandm,                                                         // {3}
        wo.pm_name,                                                             // {4}
        "",                                                             // {5}
        laststatus_str,                                                     // {6}
        can_viewtotaltime ? wo.to_be_billed : 0,                                // {7}
        wo.hold,                                                                // {8}
        wo.cust_po,                                                             // {9}
        wo.status,                                                              // {10}
        po_list,                                                                // {11}
        wo.branch                                                               // {12}
        ).Replace(Environment.NewLine, string.Empty).Replace("\t", "");
                    var fore_color = "";
                    var bg_color = "";
                    var url = string.Format("./wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}", wo.id, business_unit_id);
                    /*		switch (wo.department)
								{
								case 2:
									fore_color		= "color:#000";
								break;
								case 3:
									fore_color		= "color:#f00";
								break;
								case 5:
									fore_color		= "color:#00c";
								break;
								default:
									fore_color		= "color:#999";
								break;
								}
							*/
                    var fontbold = wo.is_creditcard ? "font-weight:bold;" : "";
                    var used_wo = "0" + wo.bvwo.TrimStart('0');
                    if (wo.hold)
                    {
                        bg_color = "background-color:pink";
                    }
                    else if (wo.parent_woprog_id > 1)
                    {
                        bg_color = wo.business_unit_id.ToString() == business_unit_id ? "background-color:yellow" : "background-color:orange";
                    }
                    else if (wo.associate_woprog_id != 0)
                    {
                        bg_color = "background-color:#fec";
                    }
                    else if (wo.quote_id != 0)
                    {
                        bg_color = "background-color:#0ff";
                    }
                    else
                    {
                        bg_color = "background-color:#fff";
                    }
                    if (dt_hold_pos.Rows.Count > 0)
                    {
                        #region Open POs
                        sb_open_pos.AppendFormat(@"
					<div class='opt' data-tooltip=""{4}"" data-url=""{5}"" data-show_notes='true' data-notes_id='{7}'  style='{2};{3};{6}'>{0} - {1}</div>", used_wo, wo.customer_name, fore_color, bg_color, tooltip, url, fontbold, wo.id);
                        var_open_pos += headertotal;
                        #endregion Open POs
                    }
                    if (wo.status == "Initial Prep")
                    {
                        #region Initial Prep
                        var_total_initial_prep += headertotal;
                        sb_initial_prep.AppendFormat(@"
					<div class='opt' data-tooltip=""{4}"" data-url=""{5}"" style='{2};{3};{6}'>{0} - {1}</div>", used_wo, wo.customer_name, fore_color, bg_color, tooltip, url, fontbold);
                        #endregion Initial Prep
                    }
                    else if (wo.status == "Waiting Approval" || wo.status == "Waiting PM Approval")
                    {
                        #region Waiting Approval
                        var_total_pm_approval += headertotal;
                        sb_pm_approval.AppendFormat(@"
					<div class='opt' data-tooltip=""{4}"" data-url=""{5}"" style='{2};{3};{6}'>{0} - {1}</div>", used_wo, wo.customer_name, fore_color, bg_color, tooltip, url, fontbold);
                        #endregion Waiting Approval
                    }
                    else if (wo.status == "Rework")
                    {
                        #region Rework
                        var_total_rework += headertotal;
                        sb_rework.AppendFormat(@"
					<div class='opt' data-tooltip=""{4}"" data-url=""{5}"" style='{2};{3};{6}'>{0} - {1}</div>", used_wo, wo.customer_name, fore_color, bg_color, tooltip, url, fontbold);
                        #endregion Rework
                    }
                    else if (wo.status == "Waiting Parent BM Approval" && wo.parent_woprog_id > 1)
                    {
                        #region Waiting for parent bm
                        var_total_waiting_other_branch += headertotal;
                        sb_waiting_other_branch.AppendFormat(@"
					<div class='opt' data-tooltip=""{4}"" data-url=""{5}"" style='{2};{3};{6}'>{0} - {1}</div>", used_wo, wo.customer_name, fore_color, bg_color, tooltip, url, fontbold);
                        #endregion Waiting for PO
                    }
                    else if (wo.status == "Waiting For PO")
                    {
                        #region Waiting for PO
                        var_total_cust_po += headertotal;
                        sb_cust_po.AppendFormat(@"
					<div class='opt' data-tooltip=""{4}"" data-url=""{5}"" style='{2};{3};{6}'>{0} - {1}</div>", used_wo, wo.customer_name, fore_color, bg_color, tooltip, url, fontbold);
                        #endregion Waiting for PO
                    }
                    else if (wo.status == OpsWOStatus.WaitingToBeInvoiced)
                    {
                        #region Waiting to be Invoiced
                        var_total_waiting_invoice += headertotal;
                        sb_waiting_invoice.AppendFormat(@"<div class='opt' data-tooltip=""{4}"" data-url=""{5}"" style='{2};{3};{6}'>{0} - {1}</div>", used_wo, wo.customer_name, fore_color, bg_color, tooltip, url, fontbold);
                        #endregion Waiting to be Invoiced
                    }
                    else if (wo.status == "Questions For PM")
                    {
                        #region Questions for PM
                        var_total_questions += headertotal;
                        sb_questions.AppendFormat(@"
					<div class='opt' data-tooltip=""{4}"" data-url=""{5}"" style='{2};{3};{6}'>{0} - {1}</div>", used_wo, wo.customer_name, fore_color, bg_color, tooltip, url, fontbold);
                        #endregion Questions for PM
                    }
                }
                last_id = wo.id;
                #endregion Open PO's
            }
            Debug.WriteLine("end loop 2 - " + DateTime.Now.ToLongTimeString());
            if (_current_user.AuthenticatedForPrivilege(16) || _current_user.AuthenticatedForPrivilege(60))
            {
                div_total_pm_approval.InnerHtml = var_total_pm_approval.ToString("C2");
                div_total_bm_approval.InnerHtml = var_total_bm_approval.ToString("C2");
                div_total_cust_po.InnerHtml = var_total_cust_po.ToString("C2");
                div_total_rework.InnerHtml = var_total_rework.ToString("C2");
                div_total_questions.InnerHtml = var_total_questions.ToString("C2");
                div_total_waiting_invoice.InnerHtml = var_total_waiting_invoice.ToString("C2");
                div_total_initial.InnerHtml = var_total_initial_prep.ToString("C2");
                div_total_waiting_other_branch.InnerHtml = var_total_waiting_other_branch.ToString("C2");
                spanHoldPOTotal.InnerHtml = var_open_pos.ToString("C2");
            }
            else
            {
                div_total_pm_approval.InnerHtml = "";
                div_total_bm_approval.InnerHtml = "";
                spanHoldPOTotal.InnerHtml = "";
                div_total_cust_po.InnerHtml = "";
                div_total_rework.InnerHtml = "";
                div_total_waiting_invoice.InnerHtml = "";
                div_total_initial.InnerHtml = "";
                div_total_waiting_other_branch.InnerHtml = "";
                spanHoldPOTotal.InnerHtml = "";

            }
            div_list_open_pos.InnerHtml = sb_open_pos.ToString();
            div_list_initial_prep.InnerHtml = sb_initial_prep.ToString();
            div_list_rework.InnerHtml = sb_rework.ToString();
            div_list_questions.InnerHtml = sb_questions.ToString();
            div_list_pm_approval.InnerHtml = sb_pm_approval.ToString();
            div_list_bm_approval.InnerHtml = sb_bm_approval.ToString();
            div_list_waiting_invoice.InnerHtml = sb_waiting_invoice.ToString();
            div_list_cust_po.InnerHtml = sb_cust_po.ToString();
            div_list_waiting_other_branch.InnerHtml = sb_waiting_other_branch.ToString();
        }
    }

    private struct wo_vars
    {
        public int id;

        public int associate_woprog_id;
        public int days_since;
        public string branch;
        public string pm_name;
        public string status;
        public string bvwo;
        public string customer_name;
        public string description;
        public string cust_po;
        public double to_be_billed;
        public double total_tandm;
        public bool hold;
        public bool is_creditcard;
        public int quote_id;
        public int parent_woprog_id;
        public int business_unit_id;
        public int pm_id;

    }


}