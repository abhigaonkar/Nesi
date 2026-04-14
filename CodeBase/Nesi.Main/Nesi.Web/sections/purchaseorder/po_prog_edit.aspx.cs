using System;
using System.Data;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using nesi.core;
using DevExpress.Xpo;
using MySql.Data.MySqlClient;
using NESI.Common.Models;

public partial class sections_purchaseorder_po_prog_edit : Page
{
    NeMember current_user;
    private const int _page_id = 92; // from Page table in DB
    NameValueCollection _q;
    int business_unit_id;
    MySqlConnection conn;

    protected void Page_Init(object sender, EventArgs e)
    {
        current_user = Toolbox.do_handle_authentication(_page_id);
        _q = Request.QueryString;

        sqlDataSourceOpenPO.FilterExpression = current_user.business_unit_id == 11 ? "" : "nesi_cut_po = false";
        int.TryParse(_q["business_unit_id"], out business_unit_id);

        if (business_unit_id == 0)
        {
            Toolbox.FriendlyException(Response, "Business unit not defined", "");
        }
        conn = Toolbox.connect();
    }
    protected void Page_Unload(object _sender, EventArgs _e)
    {
        if (conn == null || conn.State != ConnectionState.Open) return;
        conn.Close();
        conn.Dispose();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        var menu = new NeMenu(current_user, _page_id);
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
        var company = new NeBusinessUnit(business_unit_id);
        sqlDataSourceOpenPO.SelectCommand = @"SELECT

    poprog_id,
	CONCAT(
        lpad(a.business_unit_id, 3, '0'),
        '-',
        trim(LEADING '0' FROM poprog_bvpo),
        ' - ',
        Vendor_Name
    ) AS poprog_bv,
    poprog_order_description,
    nesi_cut_po,
a.poprog_cutdate
FROM

    poprog_header a,
    vendor b,
	business_unit c
WHERE
    poprog_status NOT IN (8)
AND vendor_id = poprog_vendor_id
and a.business_unit_id = c.id " +
(current_user.business_unit.is_backoffice ? " AND c.country = '" + company.country + @"'" : " and c.id = " + business_unit_id) + @"
ORDER BY

    poprog_id DESC
LIMIT 10000";


        if (Session["POSortOrder"] == null)
        {
            Session["POSortOrder"] = "poprog_bvpo";
        }
        var hold = Session["POSortOrder"].ToString();


        if (!IsPostBack)
        {
            Session["POFilter"] = null;
            Session["POPTFilter"] = null;
            Session["POWOFilter"] = null;
            Session["POPartFilter"] = null;


            lblCompanyIdentifier.Text = company.name;

            DropDownList1.SelectedValue = Session["POSortOrder"].ToString();
            ddlMemberList.DataSource = Toolbox.doSQL_dt(conn, @"SELECT member_id, member_fullname AS member_name FROM member a, poprog_header b WHERE member_id = poprog_cutby_member_id AND a.business_unit_id = @v0  group by member_id", new object[] { business_unit_id });
            ddlMemberList.DataBind();
            var lic = new ListItem("Select Member", "0");
            ddlMemberList.Items.Insert(0, lic);

            DDLPaymentType.DataSource = Toolbox.doSQL_dt(conn, @"SELECT term_id AS payment_typeid, term_desc AS payment_type FROM term order by term_id", null);
            DDLPaymentType.DataBind();
            var lic2 = new ListItem("Select Payment Type", "0");
            DDLPaymentType.Items.Insert(0, lic2);

            ddlWorkOrdersOnPO.DataSource = Toolbox.doSQL_dt(conn, @"SELECT DISTINCT(po_details_woprog_id) AS WOID, woprog_bvwo AS BVWO FROM po_details_current a, woprog b, poprog_header c WHERE po_details_woprog_id = woprog_id AND b.business_unit_id = @v0  AND poprog_id = po_details_poprog_id AND po_details_line_active = 1 AND po_details_woprog_id NOT IN(SELECT gl_te.id FROM gl_te  where tax_entity_id = (select tax_entity_id from business_unit where id = @v0)) AND po_details_woprog_id != 9999999 AND po_details_woprog_id != 9999998 AND po_details_woprog_id != 9999997 and is_gl_account = false GROUP BY po_details_woprog_id", new object[] { business_unit_id });
            ddlWorkOrdersOnPO.DataBind();
            var lic3 = new ListItem("Select Work Order", "0");
            ddlWorkOrdersOnPO.Items.Insert(0, lic3);


            ddlPartsOnPO.DataSource = Toolbox.doSQL_dt(conn, @"SELECT DISTINCT(po_details_part_no) AS partid, po_details_part_no AS partno FROM po_details_current a, poprog_header b WHERE b.business_unit_id = @v0  AND poprog_id = po_details_poprog_id AND po_details_line_active = 1 GROUP BY po_details_part_no", new object[] { business_unit_id });
            ddlPartsOnPO.DataBind();
            var lic4 = new ListItem("Select Part", "0");
            ddlPartsOnPO.Items.Insert(0, lic4);
            if (Session["POCheckNesi"] == null || Session["POCheckNesi"] == "false")
            {
                chk_showNesi.Checked = false;
                Session["POCheckNesi"] = "false";
            }
            FillListBoxes();



        }
    }

    /// <summary>
    /// Get the sum of the supplied status
    /// </summary>
    /// <param name="po_type"></param>
    /// <returns></returns>
    protected string get_sum(int status_id)
    {
        var cut_by_filter = "";
        if (ddlMemberList.SelectedValue != "" && ddlMemberList.SelectedValue != "0")
        {
            cut_by_filter = "poprog_cutby_member_id = '" + ddlMemberList.SelectedValue + "' AND";
        }
        var _total = Toolbox.doSQL_double(conn, @" SELECT IFNULL(SUM(poprog_total_cost), 0) FROM poprog_header WHERE poprog_status = @v0  AND @v2  business_unit_id = @v1 ", new object[] { status_id, business_unit_id, cut_by_filter });
        return _total.ToString("C2");
    }

    protected void FillListBoxes()
    {
        NeBusinessUnit business_unit = new NeBusinessUnit(business_unit_id);
        lsbJustCut.InnerHtml = "";
        lsbQuestions.InnerHtml = "";
        lsbWaitingPackingSlip.InnerHtml = "";
        //   lsbWaitingInvoice.InnerHtml = "";
        lsbWaitBMApproval.InnerHtml = "";
        //    lsbWaitCheque.InnerHtml = "";
        lsbWaitingApp.InnerHtml = "";
        lsbAPProblems.InnerHtml = "";



        var sb_justcut = new StringBuilder();
        var sb_waitingapp = new StringBuilder();
        var sb_questions = new StringBuilder();
        var sb_approved = new StringBuilder();
        var sb_waitingparts = new StringBuilder();
        var sb_holdup = new StringBuilder();
        var sb_approblems = new StringBuilder();
        var sb_waiting_confirmation = new StringBuilder();
        var sb_waiting_shipping_confirmation = new StringBuilder();

        var sb_waiting_invoice = new StringBuilder();

        lsb_scans2.Items.Clear();
        lsbScans.Items.Clear();

        var PMFilterstring = "";
        if (Session["POFilter"] != null)
            PMFilterstring = "AND poprog_cutby_member_id = " + Session["POFilter"];
        var PayTypeFilterstring = "";
        if (Session["POPTFilter"] != null)
            PayTypeFilterstring = "AND poprog_poprog_payment_method_id = " + Session["POPTFilter"];
        var POWOFilterstring = "";
        if (Session["POWOFilter"] != null)
            POWOFilterstring = "AND c.is_gl_account=false and c.po_details_woprog_id = " + Session["POWOFilter"];
        var POPartFilter = "";
        if (Session["POPartFilter"] != null)
            POPartFilter = "AND  c.po_details_part_no = " + Session["POPartFilter"];
        var remove_nesi_cut_po = "";
        if (Session["POCheckNesi"] == null || Session["POCheckNesi"].ToString() == "false")
        {

            remove_nesi_cut_po = " AND a.nesi_cut_po = false ";
        }
        else
        {

            remove_nesi_cut_po = " ";
        }

        var sql1 = string.Format(@"
SELECT  
	a.poprog_id,
	a.poprog_cutdate po_date,
	concat(lpad(a.business_unit_id,3,'0'),'-',trim(leading '0' from a.poprog_bvpo)) poprog_bvpo,
	a.poprog_status,
	a.poprog_order_description po_description,
	b.vendor_name,
	b.vendor_number,
	f.poprog_payment_method_method payment_method,
	IFNULL(SUM(c.po_details_qty_received), 0) qtyflag,
    IFNULL(SUM(c.po_details_qty_ordered), 0) ordflag,
	IFNULL(SUM(c.po_details_qty_ordered * c.po_details_cost), 0) po_value,
	(SELECT COUNT(*) FROM po_details_current WHERE po_details_line_active = 1 AND po_details_poprog_id = a.poprog_id) incomplete,
	(SELECT COUNT(*) FROM po_details_current WHERE po_details_line_active = 0 AND po_details_poprog_id = a.poprog_id) complete,
	IFNULL((Select poprog_scans_type from poprog_scans where poprog_scans_poprog_id = a.poprog_id and poprog_scans_type =1 limit 1),0) psscan, 
	IFNULL((Select poprog_scans_type from poprog_scans where poprog_scans_poprog_id = a.poprog_id and poprog_scans_type =2 limit 1),0) invscan, 
    a.poprog_ack_req,
    IFNULL(c.po_details_line_active, 1) po_details_line_active, 
    a.poprog_ack_req_rec, 
    a.poprog_ship_note_req, 
    a.poprog_ship_note_req_rec,
	a.nesi_cut_po,
	a.poprog_apstatus,
a.poprog_hasproblem_notes,
	g.apstatus_name,
(SELECT GROUP_CONCAT(POProgComment_Text SEPARATOR '\n<br/>--<br/>') FROM poprogcomment WHERE POProgComment_POProg_ID = a.poprog_id) po_comments ,
	h.status_type status 
FROM 
	poprog_header a 
LEFT JOIN vendor b 
	ON b.Vendor_ID = a.poprog_vendor_id 
LEFT JOIN 
	po_details_current c 
	ON a.poprog_id = c.po_details_poprog_id 
LEFT JOIN 
	poprog_payment_method f 
	ON a.poprog_poprog_payment_method_id = f.poprog_payment_method_id
LEFT JOIN 
	apstatus g 
	ON a.poprog_apstatus = g.apstatus_id	
LEFT JOIN
	poprog_status h ON a.poprog_status = h.poprog_status_id
WHERE 
	a.poprog_status != 7 AND 

	a.business_unit_id = {0} {6} {2} {3} {4} {5}
GROUP BY a.poprog_id  
ORDER BY {1}",
        business_unit_id,
        Session["POSortOrder"],
        PMFilterstring,
        PayTypeFilterstring,
        POWOFilterstring,
        POPartFilter,
        remove_nesi_cut_po
        );
        var POTable = Toolbox.doSQL_dt(conn, sql1, null);

        double summary_just_cut1 = 0;
        double summary_waiting_app1 = 0;
        double summary_packing_slips1 = 0;
        double summary_invoice1 = 0;
        double summary_order_app1 = 0;
        double summary_closed1 = 0;
        double summary_questions1 = 0;
        double summary_has_problems1 = 0;
        double summary_waiting_confirmation = 0;
        double summary_waiting_shipping_confirmation = 0;
        double holding_up = 0;

        foreach (DataRow PORows in POTable.Rows)
        {
            var background = "";
            var foreground = "";
            var po_id = PORows["poprog_id"].ToString();
            var po_number = PORows["poprog_bvpo"].ToString();
            var status = PORows["status"].ToString();
            var po_date = PORows["po_date"].ToString();
            var po_description = PORows["po_description"].ToString();
            var po_value = Convert.ToDouble(PORows["po_value"]);
            var vendor_number = PORows["vendor_number"].ToString();
            var po_status = Convert.ToInt32(PORows["poprog_status"]);
            var incomplete = Convert.ToInt32(PORows["incomplete"]);
            var complete = Convert.ToInt32(PORows["complete"]);
            var recqty = Convert.ToDouble(PORows["qtyflag"]);
            var ordqty = Convert.ToDouble(PORows["ordflag"]);
            var psqty = Convert.ToDouble(PORows["psscan"]);
            var insqty = Convert.ToDouble(PORows["invscan"]);
            var is_active = Convert.ToBoolean(PORows["po_details_line_active"]);
            var li_text = string.Format("<b>{0}</b> - {1}", po_number, PORows["Vendor_name"]);
            var li_value = PORows["poprog_id"].ToString();
            //			string link				= String.Format("/sections/purchaseorder/po_prog_frame.aspx?action=show&poprogid={0}", po_id);
            var link = string.Format("/redir.aspx?url=%2Fsections%2Fpurchaseorder%2Fpo_prog_add.aspx%3Faction%3Dshow%2526poprogid%3D{0}", po_id);
            //li					= new ListItem(li_text, li_value);
            //li.Attributes.Add("title", li_text + " Payment Method: " + PORows["Payment_Method"]);
            var ackreq = PORows["poprog_ack_req"].ToString();
            var ackrec = PORows["poprog_ack_req_rec"].ToString();
            var shipreq = PORows["poprog_ship_note_req"].ToString();
            var shiprec = PORows["poprog_ship_note_req_rec"].ToString();
            var apstatus = PORows["apstatus_name"].ToString();
            var po_comments = Toolbox.do_value_from(PORows["po_comments"]);
            var hasproblem_notes = PORows["poprog_hasproblem_notes"].ToString();
            var nesi_cut_po = Convert.ToBoolean(PORows["nesi_cut_po"]);
            var _tooltip = string.Format(@"
<table cellspacing='0' cellpadding='2' width='100%'>
	<tr>
		<td width='150'><strong>Number</strong></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td valign='top'><strong>Date</strong></td>
		<td>{1}</td>
	</tr>
	<tr>
		<td valign='top'><strong>Status</strong></td>
		<td>{7}</td>
	</tr>
	<tr>
		<td><strong>Vendor Number</strong></td>
		<td>{2}</td>
	</tr>
	<tr>
		<td><strong>Value</strong></td>
		<td>{3:C}</td>
	</tr>
	<tr>
		<td height='50' valign='top'><strong>Description</strong></td>
		<td valign='top'>{4}</td>
	</tr>
	{5}
	{6}
</table>",
    po_number,                                                          // {0}
    po_date,                                                            // {1}
    vendor_number,                                                      // {2}
    po_value,                                                           // {3}
    HttpUtility.HtmlEncode(po_description),                             // {4}
            po_comments == ""
                ? ""
                : po_status == 10
                    ? "<tr style='background-color:#fff;color:#000;'><td valign='top' colspan='2' align='left'><strong>AP Problems</strong></td></tr><tr style='background-color:#fff;color:#000;'><td colspan='2'><div style='max-height:300px;overflow-y:scroll;'>" + po_comments + "</div></td></tr>"
                    : "<tr style='background-color:#fff;color:#000;'><td valign='top' colspan='2' align='left'><strong>Comments</strong></td></tr><tr style='background-color:#fff;color:#000;'><td colspan='2'><div style='max-height:300px;overflow-y:scroll;'>" + po_comments + "</div></td></tr>",                                // {5}
            hasproblem_notes.Length > 0 ? "<tr><td><strong>AP Notes</strong></td><td>" + hasproblem_notes + "</td></tr>" : "", //{6}
            status
    ).Replace(System.Environment.NewLine, string.Empty);
            //drOpen["WOProg_BVWO"].ToString() + "-" + strCoName, "wo_prog_frame.aspx?action=show&woprog_id=" + strID + "&business_unit_id=" + business_unit_id
            switch (po_status)
            {
                case 3: // Waiting for Packing Slips
                    if (incomplete == 0 && psqty != 0 && insqty != 0) // All Items Completed, Packing Slips and Invoices Scanned
                    {
                        background = "background-color:DarkRed";
                        foreground = "color:white";
                    }
                    else if (incomplete == 0 && psqty != 0 && !is_active) // All Items Completed, Packing Slips Scanned
                    {
                        background = "background-color:yellow";
                        foreground = "color:black";
                    }
                    else if (incomplete == 0 && insqty != 0) // All Items Completed, Invoice Slip Scanned
                    {
                        background = "background-color:green";
                        foreground = "color:white";
                    }
                    else if (incomplete == 0) // All Items Completed
                    {
                        background = "background-color:blue";
                        foreground = "color:white";
                    }
                    else if (incomplete > 0 && complete > 0 && psqty != 0 && recqty != 0) // Partial Items Completed, Packing Slip Scanned
                    {
                        background = "background-color:DarkOrange";
                        foreground = "color:black";
                    }
                    else if (incomplete > 0 && complete > 0 && psqty == 0 && recqty != 0) // Partial Items Completed
                    {
                        background = "background-color:lightgray";
                        foreground = "color:black";
                    }
                    else if (recqty == 0 && psqty == 0 && insqty == 0) // no activity
                    {
                        background = "background-color:transparent";
                        foreground = "color:black";
                    }
                    else // No activity
                    {
                        background = "background-color:transparent";
                        foreground = "color:black";
                    }
                    if (ackrec != ackreq) // Check For Notice of PO Received or Items Shipped
                    {
                        //	foreground = "color:Red";
                    }
                    if (shipreq == "1" && shiprec == "0") //Check For Notice of PO Received or Items Shipped
                    {
                        //	foreground = "color:Red";
                    }
                    break;
                /*case 4:	// Waiting for Invoice
					if (recqty != 0 && psqty != 0 && insqty != 0)
						{
						background = "background-color:red";
						foreground = "color:white";
						}
					else if (recqty != 0 && psqty != 0 && insqty == 0)
						{
						background = "background-color:white";
						foreground = "color:black";
						}
					else if (recqty != 0 && psqty == 0 && insqty != 0)
						{
						background = "background-color:black";
						foreground = "color:white";
						}
				break;*/
                default: //All Items Completed, Invoice Slip Attached, No Packing Slip
                    background = "background-color:transparent";
                    foreground = "color:#000";
                    break;
            }
            //var icon_src = nesi_cut_po ? "'/images/icon/icon[flag].gif' title='Denotes that this is a NESI cut PO' alt='Denotes that this is a NESI cut PO'" : "'/images/pixel.gif'";
            var totalOrdered = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_ordered),0) FROM po_details_current WHERE po_details_poprog_id = @v0;", new object[] { po_id });
            var totalReceived = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_received),0) FROM po_details_current WHERE po_details_poprog_id = @v0;", new object[] { po_id });
            var pctReceived = Math.Round((totalReceived == 0 ? 0 : totalReceived / totalOrdered) * 100, 2);
            pctReceived = pctReceived > 100 ? 100 : pctReceived;
            var div = string.Format(@"
<div class='opt' data-value=""{6}"" data-tooltip=""{1}"" data-title=""{10} - {11}"" data-url=""{4}"">
	<div class='code' style='{3}'>&nbsp;</div>
	<div class='details'>
		<div class='po'>{0}</div>
		<div class='progress'>
			<span class='plbl'>Total Qty Rec/Ord: {8} / {7}</span>
			<div class='tbar'>
				<div class='pbar' style='width: {9}%;'></div>
			</div>
		</div>
	</div>
</div>",
        li_text,
        _tooltip,
        foreground,
        background,
        link,
        "",
        po_id,
        totalOrdered,
        totalReceived,
        pctReceived,
        po_number,
        PORows["Vendor_name"]
        );
            switch (po_status)
            {


                case 1: // Just Cut / Not Issued
                    sb_justcut.Append(div);
                    summary_just_cut1 += po_value;
                    break;
                case 2: //Waiting Approval
                    sb_waitingapp.Append(div);
                    summary_waiting_app1 += po_value;
                    break;
                case 3: // Waiting for Packing Slips
                    sb_waitingparts.Append(div);
                    summary_packing_slips1 += po_value;
                    break;
                case 4: // Waiting for invoice
                    sb_waiting_invoice.Append(div);
                    summary_invoice1 += po_value;
                    break;
                case 5: // Waiting BM Approval (Order Approved)
                    sb_approved.Append(div);
                    summary_order_app1 += po_value;
                    break;
                case 6: // Waiting BM Approval (Order Approved)

                    break;
                case 9: // Questions
                    sb_questions.Append(div);
                    summary_questions1 += po_value;
                    break;
                case 10:    // Problems
                    sb_approblems.Append(div);
                    summary_has_problems1 += po_value;
                    break;
            }
        } // End Foreach

        summary_just_cut.InnerText = summary_just_cut1.ToString("c2");
        summary_waiting_app.InnerText = summary_waiting_app1.ToString("c2");
        summary_packing_slips.InnerText = summary_packing_slips1.ToString("c2");
        //  summary_invoice.Text = summary_invoice1.ToString("c2");
        summary_order_app.InnerText = summary_order_app1.ToString("c2");
        //  summary_closed.Text = summary_closed1.ToString("c2");
        summary_questions.InnerText = summary_questions1.ToString("c2");
        summary_waiting_invoice.InnerText = summary_invoice1.ToString("C2");

        summary_problems.InnerText = summary_has_problems1.ToString("C2");


        var posholdingupwos = Toolbox.doSQL_dt(conn, @" 
SELECT 
	c.poprog_id po_id, 
	concat(lpad(c.business_unit_id,3,'0'),'-',trim(leading '0' from c.poprog_bvpo)) po_n, 
	c.poprog_cutdate po_dt, 
	c.poprog_order_description po_desc, 
	d.vendor_number vendor_n, 
	d.vendor_name, 
	GROUP_CONCAT(DISTINCT CONCAT(b.woprog_id, '|', b.woprog_bvwo)) wos, 
	IFNULL(SUM(a.po_details_qty_ordered * a.po_details_cost), 0) po_value,
	e.apstatus_name,
c.poprog_cutdate,
 c.poprog_bvpo,
c.poprog_invoice_slip_scan_date
FROM 
	po_details_current a 
LEFT JOIN 
	woprog b ON b.woprog_id > 10000 AND a.po_details_woprog_id = b.woprog_id and a.is_gl_account = false 
LEFT JOIN 
	poprog_header c ON a.po_details_poprog_id = c.poprog_id 
LEFT JOIN 
	vendor d ON c.poprog_vendor_id = d.vendor_id 
LEFT JOIN 
	apstatus e ON c.poprog_apstatus = e.apstatus_id 
WHERE 
	c.business_unit_id = @v0 and 
	a.is_gl_account = false AND 
	(a.po_details_line_active = true AND a.po_details_qty_received < a.po_details_qty_ordered OR a.po_details_line_active = true OR c.poprog_status = 10)  AND 
	b.woprog_status NOT IN ('Invoiced', 'Open') AND 
	b.woprog_opendatetime IS NOT NULL 
GROUP BY 
	c.poprog_id 
ORDER BY
 " + Session["POSortOrder"], new object[] { business_unit_id, Session["POSortOrder"] });
        foreach (DataRow po_r in posholdingupwos.Rows)
        {
            var po_id = po_r["po_id"];
            var po_n = po_r["po_n"];
            var po_dt = po_r["po_dt"];
            var po_value = po_r["po_value"];
            var po_desc = po_r["po_desc"];
            var vendor_n = po_r["vendor_n"];
            var vendor_name = po_r["vendor_name"];
            var ap_status = po_r["apstatus_name"];
            var wos = po_r["wos"].ToString();
            var wo_list = "";
            holding_up += Convert.ToDouble(po_value);
            if (wos.Contains(","))
            {
                var _wos = wos.Split(',');
                foreach (var wo_i in _wos)
                {
                    var __wos = wo_i.Split('|');
                    var wo_id = __wos[0];
                    var wo_n = __wos[1];
                    wo_list += string.Format("<div><a href='/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}' style='color:#fff;'>{2}</a></div>", wo_id, business_unit_id, wo_n);
                }
            }
            else
            {
                var _wos = wos.Split('|');
                var wo_id = _wos[0];
                var wo_n = _wos[1];
                wo_list += string.Format("<div><a href='/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}' style='color:#fff;'>{2}</a></div>", wo_id, business_unit_id, wo_n);
            }

            var link = string.Format("/redir.aspx?url=%2Fsections%2Fpurchaseorder%2Fpo_prog_add.aspx%3Faction%3Dshow%2526poprogid%3D{0}", po_id);
            //  var link = string.Format("/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}", po_id);
            var li_text = string.Format("<b>{0}</b> - {1}", po_n, vendor_name);

            var _tooltip = string.Format(@"
<table>
	<tr>
		<td width='150'><strong>Number</strong></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td valign='top'><strong>Date</strong></td>
		<td>{1}</td>
	</tr>
	<tr>
		<td><strong>Vendor Number</strong></td>
		<td>{2}</td>
	</tr>
	<tr>
		<td><strong>Value</strong></td>
		<td>{3:C}</td>
	</tr>
	<tr>
		<td height='50'  valign='top'><strong>Description</strong></td>
		<td valign='top'>{4}</td>
	</tr>
	<tr>
		<td><strong>Work Orders Affected:</strong></td>
		<td>{6}</td>
	</tr>
</table>",
po_n,                                   // {0}
po_dt,                                  // {1}
vendor_n,                               // {2}
po_value,                               // {3}
HttpUtility.HtmlEncode(po_desc),        // {4}
ap_status.ToString() == ""
    ? ""
    : "<tr><td colspan='2' align='left'><strong>AP Problem/Comments</strong></td></tr><tr><td colspan='2'>" + ap_status + "</td></tr>",                              // {5}
wo_list                                 // {6}
).Replace(System.Environment.NewLine, string.Empty);
            var totalOrdered = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_ordered),0) FROM po_details_current WHERE po_details_poprog_id = @v0;", new object[] { po_id });
            var totalReceived = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(po_details_qty_received),0) FROM po_details_current WHERE po_details_poprog_id = @v0;", new object[] { po_id });
            var pctReceived = Math.Round((totalReceived == 0 ? 0 : totalReceived / totalOrdered) * 100, 2);

            pctReceived = pctReceived > 100 ? 100 : pctReceived;
            var div = string.Format(@"
<div class='opt' data-tooltip=""{1}"" data-url=""{2}"" data-title=""{7} - {8}"" data-value=""{3}"">
	<div class='code' style='background-color:#fcc;color:#600;'>&nbsp;</div>
	<div class='details'>
		<div class='po'>{0}</div>
		<div class='progress'>
			<span class='plbl'>Total Qty Rec/Ord: {5} / {4}</span>
			<div class='tbar'>
				<div class='pbar' style='width: {6}%;'></div>
			</div>
		</div>
	</div>
</div>", li_text, _tooltip, link, po_id, totalOrdered, totalReceived, pctReceived, po_n, vendor_name);
            sb_holdup.Append(div);
        }
        summary_wo_holdup.InnerText = holding_up.ToString("C2");

        lsbJustCut.InnerHtml = sb_justcut.ToString();
        lsbQuestions.InnerHtml = sb_questions.ToString();
        lsbWaitBMApproval.InnerHtml = sb_approved.ToString();
        lsbWaitingPackingSlip.InnerHtml = sb_waitingparts.ToString();
        lsbWaitingApp.InnerHtml = sb_waitingapp.ToString();
        lsbWOHoldup.InnerHtml = sb_holdup.ToString();
        lsbAPProblems.InnerHtml = sb_approblems.ToString();


        lsbwaiting_for_invoice.InnerHtml = sb_waiting_invoice.ToString();


        #region just scanned list and POs Needed
        div_allscans.Visible = current_user.business_unit.is_backoffice;
        if (current_user.business_unit.is_backoffice)
        {
            lbl_final_column.InnerHtml = business_unit.country == "USA" ? "All Scans<br/> USA" : "All Scans<br/> Canada";
        }

        DirectoryInfo dirJust = null;
        DirectoryInfo dirALL = null;

        NeBusinessUnit.CheckBUProcessFolderStructure(business_unit_id);
        var popath = Toolbox.doSQL_string(conn, @"SELECT POPath from business_unit  WHERE ID =@v0", new object[] { business_unit_id });
        //ddlCompanyList.SelectedValue);
        //dirJust = new DirectoryInfo("f:\\ProjectFiles\\REGIS\\sections\\purchaseorder\\scanned_items\\");

        try
        {
            dirJust = new DirectoryInfo(popath);
            FileInfo[] arrFiles;
            // For catching my test environment, otherwise I can't load this page due to not being able to access this scan path. -- Matt
            var from_localhost = HttpContext.Current.Request.Url.Port == 80 &&
                                 HttpContext.Current.Request.Url.Host == "localhost";

            dirALL = new DirectoryInfo(business_unit.country == "USA"
                            ? Toolbox.app_setting("po_all_scans_usa")
                            : Toolbox.app_setting("po_all_scans_can"));

            #region scanned list for the business unit directly
            try
            {
                arrFiles = dirJust.GetFiles();
                for (var i = 0; i < arrFiles.Length; i++)
                {
                    var filename = HttpUtility.UrlEncode(arrFiles[i].Name);
                    if (!arrFiles[i].Extension.Contains("pdf") && !arrFiles[i].Extension.Contains("PDF") && !filename.Contains("BPSU_264")) continue;

                    var strCreated = arrFiles[i].CreationTime.ToString("MM-dd HH:mm:ss");
                    if (filename.Contains("PO_NEEDED"))
                    {
                        //var li_text = string.Format(strCreated);
                        //var div = string.Format(@"<div align='left'><a style='text-decoration:none; color:black;' href=""{1}"">{0}</a></div>", li_text, "javascript: open_scan('" + filename + "');");
                        //lsbAPProblems.InnerHtml = div + lsbAPProblems.InnerHtml;
                        var li_text = string.Format(strCreated);
                        var div = string.Format(@"
<div align='left' class='opt' data-dotip='false' onclick=""open_scan('{1}');"">
	<div class='code'>&nbsp;</div>
	<div class='details'>
		<div class='po'>{0}</div>
		<div class='progress'>{2}</div>
	</div>
</div>", Server.UrlDecode(filename), filename, strCreated);
                        var associatedBU = NePOProg.Scan.GetAssociatedBusinessUnit(arrFiles[i].FullName);
                        if (associatedBU == 0)
                        {
                            var ij = NePOProg.Scan.GetXMLData(arrFiles[i].FullName, current_user.id);
                        }
                        else if (associatedBU == business_unit_id)
                        {
                            lsbAPProblems.InnerHtml = div + lsbAPProblems.InnerHtml;
                        }
                    }
                    else
                    {
                        strCreated = filename + " - " + strCreated;
                        var li2 = new ListItem(strCreated, filename);
                        lsbScans.Items.Add(li2);
                        lsbScans.Enabled = true;
                    }
                }
            }
            catch (Exception ee)
            {
                Toolbox.do_errorLog_errorStack(ee);
                if (!Request.Url.Host.Contains("localhost"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error",
                        "alert('Cannot Access Branches PO Scans Folder.');", true);
                    return;
                }
            }
            #endregion
            #region fill the ALL list
            try
            {
                arrFiles = dirALL.GetFiles();
                for (var i = 0; i < arrFiles.Length; i++)
                {
                    if (!arrFiles[i].Extension.Contains("pdf") && !arrFiles[i].Extension.Contains("PDF")) continue;
                    var filename = HttpUtility.UrlEncode(arrFiles[i].Name);
                    var strCreated = arrFiles[i].CreationTime.ToString("MM-dd HH:mm:ss");
                    strCreated = filename + " - " + strCreated;

                    var li2 = new ListItem(strCreated, filename);
                    lsb_scans2.Items.Add(li2);
                    lsb_scans2.Enabled = true;

                }
            }
            catch (Exception ee)
            {
                Toolbox.do_errorLog_errorStack(ee);
                if (!Request.Url.Host.Contains("localhost"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error",
                        "alert('Cannot Access Branches PO Scans Folder.');", true);
                    return;
                }
            }


            #endregion

        }
        catch (Exception e)
        {
            Toolbox.do_errorLog_errorStack(e);
            var li2 = new ListItem("Unable to Connect to " + popath, "0");
            lsbScans.Items.Add(li2);
            lsbScans.Enabled = false;

        }

        #endregion

    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["POSortOrder"] = DropDownList1.SelectedValue;
        DropDownList1.SelectedValue = Session["POSortOrder"].ToString();
        lsbJustCut.InnerHtml = "";
        lsbQuestions.InnerHtml = "";
        lsbWaitingPackingSlip.InnerHtml = "";
        //lsbWaitingInvoice.InnerHtml		= "";
        lsbWaitBMApproval.InnerHtml = "";
        lsbWOHoldup.InnerHtml = "";
        //lsbWaitCheque.InnerHtml			= "";
        lsbScans.Items.Clear();
        lsb_scans2.Items.Clear();
        FillListBoxes();
    }
    protected void ddlpm_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void lsbScans_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_scanned_image(HttpUtility.UrlDecode(lsbScans.SelectedValue));

    }
    protected void lsbScans2_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_scanned_image2(HttpUtility.UrlDecode(lsb_scans2.SelectedValue));

    }
    private void LoadScanComments(string _path)
    {
        var dt = NePOProg.Scan.GetXMLData(_path, current_user.id, business_unit_id);
        scan_comments.InnerHtml = "";
        using (var uow = new UnitOfWork())
        {
            foreach (DataRow dr in dt.Rows)
            {
                var member_id = 0;
                int.TryParse(dr["member"].ToString(), out member_id);
                var employee = uow.GetObjectByKey<ne_xpo.cs.member>(member_id);
                scan_comments.InnerHtml += string.Format("<br/><div class='comment'><div class='whowhen'><strong>{0}</strong> @ {2}</div>{1}</div>", employee.member_fullname, dr["data"], dr["date"]);
            }
        }
    }
    protected void load_scanned_image(string _filename)
    {
        // This function will be called from three buckets: 'AP Problems', 'All Scans Canada', and 'This Business Unit Scans'.
        // From 'AP Problems', we are using regular Divs to make a list;
        // From others, we are using real List or List box.
        // And the control lsbScans is only for 'This Business Unit Scans'. When picking an item from 'This Business Unit Scans', it has value.
        // But no matter in which case we will have a _filename.

        var httpFile = HttpUtility.UrlEncode(_filename);
        
        mem_notes.Text = "";
        var fileServerInternal = NeTaxEntity.BaseFolder(business_unit_id, false);
        var fileServerExternal = NeTaxEntity.BaseFolder(business_unit_id, true);
        hidFileName.Value = _filename;
        var popath = Toolbox.doSQL_string(conn, @"SELECT POPath from business_unit  WHERE ID =@v0", new object[] { business_unit_id });
        var inPath = popath + "\\" + _filename;
        var extPath = fileServerInternal + @"\POs\scanned_items\" + _filename;
        NePOProg.Scan.Rename(inPath, extPath, false);
        if (_filename.Contains("PO_NEEDED"))
        {
            btnpo_needed.Text = "Return Scan to AP";
            btn_savenotes.Visible = true;
            div_scan_comments.Visible = true;
            LoadScanComments(inPath);
        }
        else
        {
            btnpo_needed.Text = "PO STILL NEEDED!";
            btn_savenotes.Visible = false;
            div_scan_comments.Visible = false;
        }

        scanshow.Attributes.Add("src", string.Format("{1}/POs/scanned_items/{0}", httpFile, fileServerExternal));
        scan_pop.ShowOnPageLoad = true;
        gv_open_pos.SearchPanelFilter = "";

    }

    protected void load_scanned_image2(object filename)
    {


        var fileServerInternal = NeTaxEntity.BaseFolder(business_unit_id, false);
        var fileServerExternal = NeTaxEntity.BaseFolder(business_unit_id, true);
        hidFileName.Value = filename.ToString();

        NeBusinessUnit business_unit = new NeBusinessUnit(Convert.ToInt32(business_unit_id));
        string popath_override = business_unit.country == "USA" ? Toolbox.app_setting("po_all_scans_usa") : Toolbox.app_setting("po_all_scans_can");
        btn_savenotes.Visible = true;
        div_scan_comments.Visible = true;
        LoadScanComments(popath_override + "\\" + filename);
        //Moving files to \POs\scanned_items\ folder
        if (!File.Exists(fileServerInternal + @"\POs\scanned_items\" + filename))
        {
            NePOProg.Scan.Rename(popath_override + "\\" + filename, fileServerInternal + @"\POs\scanned_items\" + filename, false);
        }
        var pdfscan = string.Format("<embed src='{1}/pos/scanned_items/{0}/' width='100%' height='100%'>\n", filename, fileServerExternal);
        scanshow.Attributes.Add("src", string.Format("{1}/POs/scanned_items/{0}", filename, fileServerExternal));
        scan_pop.ShowOnPageLoad = true;
        gv_open_pos.SearchPanelFilter = "";

    }

    protected void lbAddNewPO_Click(object sender, EventArgs e)
    {
        Response.Redirect("po_prog_add.aspx?action=add&poprogid=0&business_unit_id=" + business_unit_id);
    }

    protected void ClearSelections()
    {
        //lsbJustCut.SelectedIndex = -1;
        //lsbJustCut.ClearSelection();
        //lsbQuestions.SelectedIndex = -1;
        //lsbQuestions.ClearSelection();
        //lsbWaitingPackingSlip.SelectedIndex = -1;
        //lsbWaitingPackingSlip.ClearSelection();
        //lsbWaitingInvoice.SelectedIndex = -1;
        //lsbWaitingInvoice.ClearSelection();
        //lsbWaitBMApproval.SelectedIndex = -1;
        //lsbWaitBMApproval.ClearSelection();
        //lsbWaitCheque.SelectedIndex = -1;
        //lsbWaitCheque.ClearSelection();

    }

    protected void btnPONeeded_Click(object sender, EventArgs e)
    {
        var popath = Toolbox.doSQL_string(conn, @"SELECT POPath from business_unit  WHERE ID =@v0", new object[] { business_unit_id });
        var fromPath = "";
        var toPath = "";
        var businessUnit = new NeBusinessUnit(business_unit_id);
        if (hidFileName.Value.Contains("PO_NEEDED"))
        {
            if (mem_notes.Text.Trim() == "")
            {
                throw new Exception("You must enter a reason why you are returning this scan back to AP.");
            }
            else
            {
                string override_path = businessUnit.country == "USA"
                                        ? Toolbox.app_setting("po_all_scans_usa")
                                        : Toolbox.app_setting("po_all_scans_can");
                fromPath = popath + hidFileName.Value;
                toPath = override_path + hidFileName.Value.Replace("PO_NEEDED-", "");
                NePOProg.Scan.SaveXMLData(fromPath, current_user.id, mem_notes.Text);
                NePOProg.Scan.Rename(fromPath, toPath, true);
                shared.alert_ap(string.Format("Scan {0} returned to regional scans by {1}", hidFileName.Value, current_user.FullName), string.Format("The scan {0} was returned back to regional scans by {1} with the following reason<br/>**************<br/><br/>{2}", hidFileName.Value, current_user.FullName, mem_notes.Text), businessUnit.country == "CDN", current_user.NEEmail);
                gv_open_pos.Selection.UnselectAll();
                lsbScans.ClearSelection();
            }
        }
        else
        {
            if (lsb_scans2.SelectedIndex >= 0)
            {
                // Move files from global location to TE structure!
                string override_path = businessUnit.country == "USA"
                                        ? Toolbox.app_setting("po_all_scans_usa")
                                        : Toolbox.app_setting("po_all_scans_can");
                fromPath = override_path + hidFileName.Value;
                toPath = popath + "PO_NEEDED-" + hidFileName.Value;
                NePOProg.Scan.Rename(fromPath, toPath, true);
                NePOProg.Scan.SaveXMLData(toPath, current_user.id, mem_notes.Text, business_unit_id);
                gv_open_pos.Selection.UnselectAll();
                lsb_scans2.ClearSelection();
            }
            else
            {
                fromPath = popath + hidFileName.Value;
                toPath = popath + "PO_NEEDED-" + hidFileName.Value;
                NePOProg.Scan.Rename(fromPath, toPath, true);
                if (mem_notes.Text.Trim() != "")
                {
                    NePOProg.Scan.SaveXMLData(toPath, current_user.id, mem_notes.Text, business_unit_id);
                }
                else
                {
                    NePOProg.Scan.SaveXMLData(toPath, current_user.id, "Automated message: This scan has been flagged as needing a PO associated with it", business_unit_id);
                }
                gv_open_pos.Selection.UnselectAll();
                lsbScans.ClearSelection();
            }
        }
        mem_notes.Text = "";
        scan_pop.ShowOnPageLoad = false;
        FillListBoxes();
    }

    protected void btnUseScan_Click(object sender, EventArgs e)
    {
        if (gv_open_pos.Selection.Count > 0)
        {
            try
            {
                string override_path = new NeBusinessUnit(Convert.ToInt32(business_unit_id)).country == "USA"
                                            ? Toolbox.app_setting("po_all_scans_usa")
                                            : Toolbox.app_setting("po_all_scans_can");
                if (lsb_scans2.SelectedIndex < 0)
                {
                    override_path = "";
                }
                var poprog_id = 0;
                int.TryParse(gv_open_pos.GetSelectedFieldValues("poprog_id")[0].ToString(), out poprog_id);
                ;
                NePOProg.use_scan(poprog_id, business_unit_id, hidFileName.Value, Convert.ToInt32(rbl_slip_type.SelectedValue), current_user, mem_notes.Text, override_path);
            }
            catch (Exception scan_move_ex)
            {
                Toolbox.do_errorLog_errorStack(scan_move_ex);
                throw new Exception("There Was an Error Attempting to Rename This Scan: " + scan_move_ex);
            }
            finally
            {
                gv_open_pos.Selection.UnselectAll();
                lsbScans.ClearSelection();
                scan_pop.ShowOnPageLoad = false;
                FillListBoxes();
            }

        }
        else
        {
            throw new Exception("Please select a PO to attach to");
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        var fileServer = NeTaxEntity.BaseFolder(current_user.business_unit_id, false);
        try
        {
            File.Delete(fileServer + "\\POs\\scanned_items\\" + hidFileName.Value);
        }
        catch
        {


        }

        scan_pop.ShowOnPageLoad = false;
        FillListBoxes();
    }
    protected void btnDeleteScan_Click(object sender, EventArgs e)
    {
        var fileServer = NeTaxEntity.BaseFolder(current_user.business_unit_id, false);
        string override_path = new NeBusinessUnit(business_unit_id).country == "USA" ? Toolbox.app_setting("po_all_scans_usa") : Toolbox.app_setting("po_all_scans_can");
        if (lsb_scans2.SelectedIndex < 0)
        {
            override_path = "";
        }

        try
        {
            File.Delete(fileServer + "\\POs\\scanned_items\\" + hidFileName.Value);
        }
        catch
        {

        }
        var popath = Toolbox.doSQL_string(conn, @"SELECT POPath from business_unit  WHERE ID =@v0", new object[] { business_unit_id });  //+ ddlCompanyList.SelectedValue);
        try
        {
            if (override_path == "")
            {
                File.Delete(popath + "\\" + hidFileName.Value);
            }
            else
            {
                File.Delete(override_path + "\\" + hidFileName.Value);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('There was an error deleting this scan from this PO.  Please refresh the page using SHIFT + F5 and try it again.');", true);
            return;
        }
        scan_pop.ShowOnPageLoad = false;
        FillListBoxes();
    }
    protected void ddlMemberList_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["POFilter"] = ddlMemberList.SelectedValue;
        ddlMemberList.SelectedValue = Session["POFilter"].ToString();
        if (ddlMemberList.SelectedValue == "0")
            Session["POFilter"] = null;
        lsbJustCut.InnerHtml = "";
        lsbQuestions.InnerHtml = "";
        lsbWaitingPackingSlip.InnerHtml = "";
        //lsbWaitingInvoice.InnerHtml		= "";
        lsbWaitBMApproval.InnerHtml = "";
        //lsbWaitCheque.InnerHtml			= "";
        lsbScans.Items.Clear();
        FillListBoxes();
    }
    protected void DDLPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["POPTFilter"] = DDLPaymentType.SelectedValue;
        DDLPaymentType.SelectedValue = Session["POPTFilter"].ToString();
        if (DDLPaymentType.SelectedValue == "0")
            Session["POPTFilter"] = null;
        lsbJustCut.InnerHtml = "";
        lsbQuestions.InnerHtml = "";
        lsbWaitingPackingSlip.InnerHtml = "";
        //lsbWaitingInvoice.InnerHtml		= "";
        lsbWaitBMApproval.InnerHtml = "";
        //lsbWaitCheque.InnerHtml			= "";
        lsbScans.Items.Clear();
        FillListBoxes();
    }
    protected void ddlWorkOrdersOnPO_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["POWOFilter"] = ddlWorkOrdersOnPO.SelectedValue;
        ddlWorkOrdersOnPO.SelectedValue = Session["POWOFilter"].ToString();
        if (ddlWorkOrdersOnPO.SelectedValue == "0")
            Session["POWOFilter"] = null;
        lsbJustCut.InnerHtml = "";
        lsbQuestions.InnerHtml = "";
        lsbWaitingPackingSlip.InnerHtml = "";
        //lsbWaitingInvoice.InnerHtml		= "";
        lsbWaitBMApproval.InnerHtml = "";
        //lsbWaitCheque.InnerHtml			= "";
        lsbScans.Items.Clear();
        FillListBoxes();
    }
    protected void ddlPartsOnPO_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["POPartFilter"] = ddlPartsOnPO.SelectedValue;
        ddlPartsOnPO.SelectedValue = Session["POPartFilter"].ToString();
        if (ddlPartsOnPO.SelectedValue == "0")
            Session["POPartFilter"] = null;
        lsbJustCut.InnerHtml = "";
        lsbQuestions.InnerHtml = "";
        lsbWaitingPackingSlip.InnerHtml = "";
        //lsbWaitingInvoice.InnerHtml		= "";
        lsbWaitBMApproval.InnerHtml = "";
        //lsbWaitCheque.InnerHtml			= "";
        lsbScans.Items.Clear();
        FillListBoxes();
    }

    protected void chk_showNesi_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_showNesi.Checked)
        {
            Session["POCheckNesi"] = "true";
        }
        else
            Session["POCheckNesi"] = "false";
        FillListBoxes();
    }
    protected void gv_open_pos_PreRender(object sender, EventArgs e)
    {
        gv_open_pos.FocusedRowIndex = -1;
    }

    protected void ASPxpcScanDisplay_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
    {
        var filename = e.Parameter;
        load_scanned_image(Server.UrlDecode(filename));

    }

    protected void btn_savenotes_Click(object sender, EventArgs e)
    {
        var override_path = "";
        if (lsb_scans2.SelectedIndex >= 0)
        {
            // Move files from global location to TE structure!
            override_path = new NeBusinessUnit(Convert.ToInt32(business_unit_id)).country == "USA"
                                    ? Toolbox.app_setting("po_all_scans_usa")
                                    : Toolbox.app_setting("po_all_scans_can");
        }
        var popath = Toolbox.doSQL_string(conn, @"SELECT POPath from business_unit  WHERE ID =@v0", new object[] { business_unit_id });
        var toPath = (override_path == "" ? popath : override_path) + hidFileName.Value;
        if (mem_notes.Text.Trim() != "")
        {
            NePOProg.Scan.SaveXMLData(toPath, current_user.id, mem_notes.Text);
            var dt = NePOProg.Scan.GetXMLData(toPath, current_user.id, business_unit_id, true);
        }
        else
        {
            throw new Exception("Please enter a note");
        }
        if (override_path == "")
        {
            load_scanned_image(hidFileName.Value);
        }
        else
        {
            load_scanned_image2(hidFileName.Value);
        }
        mem_notes.Text = "";
    }
}