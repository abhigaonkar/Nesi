using System;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.Data;
using nesi.core;

public partial class sections_reports_ar_report_index : Page
    {
    private static readonly int _page_id = 57;
    private static readonly string _page_name = "ARReport";
    private Toolbox _tools;
    private NeMember current_user;
    private ASPxDropDownEdit dde_filter;
    private SqlDataSource ds_templates;
    public string DSN;
    private ASPxHiddenField h;
    private JavaScriptSerializer jSON = new JavaScriptSerializer();
    private Panel panel_export;
    public string woprogid_notes;

    protected void Page_Init(object sender, EventArgs e)
        {
        _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(_page_id);
        layout.__page_name = _page_name;
        layout.used_gv = Aspxgridview1;
        if (Session["working_business_unit_id"] != null)
            Session.Remove("working_business_unit_id");
        h = (ASPxHiddenField) layout.FindControl("h");
        ds_templates = (SqlDataSource) layout.FindControl("ds_templates");
        dde_filter = (ASPxDropDownEdit) layout.FindControl("dde_filter");
        panel_export = (Panel) layout.FindControl("panel_export");
        panel_export.Visible = true;
        ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
        ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
        }

    protected void Page_Load(object sender, EventArgs e)
        {
        var _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(_page_id);
        //	_tools.dont_cache_page();
        var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
        var lbltemp = (Label) Page.Master.FindControl("lblHeading");
        lbltemp.Text = "Accounts Receivable Report";
        if (!IsPostBack && !IsCallback)
            {
            Session["ar_report_gv"] = null;
            Session["ar_report_bv_notes"] = null;
            Session["ar_report_cust"] = "";
            var dtBusinessUnit = Toolbox.doSQL_dt(@"Select distinct tax_entity.id, tax_entity.ddl_name, tax_entity.dsn 
from tax_entity 
inner join business_unit b on b.tax_entity_id = tax_entity.id and b.id in (" + new Current_User().visible_business_units + ") order by tax_entity.ddl_name", null);
      
            ddlCompany.DataSource = dtBusinessUnit;
            var lic = new ListItem("All Tax Entities", "0");
            ddlCompany.Items.Add(lic);
            ddlCompany.DataBind();
            ddlCompany.SelectedValue = current_user.business_unit.tax_entity_id.ToString();
            dteScope.Date = DateTime.Today.AddDays(14);
            if (current_user.AuthenticatedForPrivilege(49))
                ddlCompany.Enabled = true;
            var gl = new NeGridLayouts(current_user.id, _page_name);
            h.Set("gridview_id", "Aspxgridview1");
            if (gl.GridLayoutID == 0)
                {
                Aspxgridview1.FilterExpression = "";
                gl.GridLayout_Layout = Aspxgridview1.SaveClientLayout();
                gl.member_id = current_user.id;
                gl.GridLayout_Name = "Default";
                gl.GridLayout_Gridid = _page_name;
                gl.SaveGridLayout();
                h.Set("ID", gl.GridLayoutID);
                h.Set("NAME", gl.GridLayout_Name);
                }
            else
                {
                Aspxgridview1.LoadClientLayout(gl.GridLayout_Layout);
                h.Set("ID", gl.GridLayoutID);
                h.Set("NAME", gl.GridLayout_Name);
                }
            dde_filter.Text = gl.GridLayout_Name;
            }
        var comp2 = new NeTaxEntity(ddlCompany.SelectedValue);
        layout.export_filename = comp2.ddl_name + " AR Listing " + dteScope.Date.ToString("yyyy-MM-dd");
        DSN = comp2.DSN;
        populate_grid();
        }

    protected void gv_wo_line_grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
        var gv = (ASPxGridView) sender;
        if (e.Parameters != "")
            {
            gv.LoadClientLayout(e.Parameters);
            }
        else
            {
            gv.FilterExpression = "";
            for (var i = 0; i < gv.Columns.Count; i++)
                if (gv.Columns[i] is GridViewDataColumn)
                    {
                    var col = (GridViewDataColumn) gv.Columns[i];
                    if (col.GroupIndex > -1)
                        gv.UnGroup(col);
                    col.Visible = true;
                    }
            }
        }

    protected void gv_wo_line_grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
        var gv = (ASPxGridView) sender;
        e.Properties["cpExp"] = gv.SaveClientLayout();
        }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
        var comp = new NeTaxEntity(ddlCompany.SelectedValue);
        DSN = comp.DSN;
        Aspxgridview1.CancelEdit();
        Session["ar_report_gv"] = null;
        Session["ar_report_cust"] = "";
        Session["ar_report_bv_notes"] = null;
        populate_grid();
        }

    protected void populate_grid()
        {
//        var dt = new DataTable();
//        if (Session["ar_report_gv"] == null)
//            {
//            var sql = @"
//SELECT 
//	cust, 
//	name, 
//	credit_limit, 
//	SUM(IF(n_days <= 30, balance, 0)) as thirty,  
//	SUM(IF(n_days <= 60 AND n_days > 30, balance, 0)) as sixty,
//	SUM(IF(n_days <= 90 AND n_days > 60, balance, 0)) as ninety,
//	SUM(IF(n_days <= 120 AND n_days > 90, balance, 0)) as onetwenty,
//	SUM(IF(n_days > 120, balance, 0)) as onetwentyplus, 

//	SUM(IF(n_days <= 30, balance, 0))+ 
//	SUM(IF(n_days <= 60 AND n_days > 30, balance, 0))+
//	SUM(IF(n_days <= 90 AND n_days > 60, balance, 0))+
//	SUM(IF(n_days <= 120 AND n_days > 90, balance, 0))+
//	SUM(IF(n_days > 120, balance, 0)) as ebalance,

//	IF(
//		(	(
//			SUM(IF(n_days <= 30,balance,0))+ 
//			SUM(IF(n_days <= 60 AND n_days > 30, balance, 0))+ 
//			SUM(IF(n_days <= 90 AND n_days > 60, balance, 0))+ 
//			SUM(IF(n_days <= 120 AND n_days > 90, balance, 0))+ 
//			SUM(IF(n_days > 120, balance, 0))
//			)-credit_limit
//		)<0,
//		0, 
//		(
//			(
//			SUM(IF(n_days <= 30, balance, 0))+ 
//			SUM(IF(n_days <= 60 AND n_days > 30, balance, 0))+ 
//			SUM(IF(n_days <= 90 AND n_days > 60, balance, 0))+ 
//			SUM(IF(n_days <= 120 AND n_days > 90, balance, 0))+ 
//			SUM(IF(n_days > 120, balance, 0 ))
//			)-credit_limit
//		)) as overage, 
//	'F' as OnHold, 
//	0.00 as wos, 
//	'' as Invoices, 
//	0 as creditdays, 
//	'' as customer_memo, 
//	'' as customer_notes, 
//	'' as customer_salesnotes,
//	'' as bv_cust_notes,
//	'' as status_ , 
//	'' as customer_id,
//	90.0 as nextexpectedpay,
//	0 as avg5, 
//	'' as arnotes , 
//	0.0 as next14, 
//	'' as cust_whyhold, 
//	'' as cust_whohold ,
//	'' as note_date 
//FROM (select 
//	AR.cust as cust, 
//	CU.NAME as name, 
//	CU.CREDIT_LINE as credit_limit , 
//	DATEDIFF(day, TO_DATE(ARF_DATE), CURDATE()) n_days,
//	balance,
//	'F' as OnHold, 
//	0.00 as wos, 
//	'' as Invoices, 
//	0 as creditdays, 
//	'' as customer_memo, 
//	'' as customer_notes, 
//	'' as customer_salesnotes,
//	'' as bv_cust_notes,
//	'' as status_ , 
//	'' as customer_id,
//	90.0 as nextexpectedpay,
//	0 as avg5, 
//	'' as arnotes , 
//	0.0 as next14, 
//	'' as cust_whyhold, 
//	'' as cust_whohold,
//	'' as note_date 
//	from AR_TRANSACTIONS as AR, CUSTOMER AS CU 
//	where AR.CUST = CU.CUS_NO  AND (CODE='I' or CODE='C' or CODE='P' or CODE='D' or CODE='S') AND Balance<>0
//	) asdf
//GROUP BY cust,name,credit_limit";
//            dt = _tools.getSQL_datatable(sql, DSN,null);
//            foreach (DataRow dr in dt.Rows)
//                {
//                var custdt = _tools.getSQL_dataset(@"Select customer_ID,customer_creditdays,customer_notes,customer_salesnotes,customer_memo,customer_collection_status,customer_arnotes,customer_hold,customer_whyhold, ifnull(get_name(customer_whohold),'N/A') as _customer_whohold from customer  where customer_number =@v0", new object[] { dr["cust"].ToString().Trim().TrimStart('0') });
//                var result1 = _tools.getSQL_double(@"Select ifnull(sum(WOProg_StillToBeBilled),0) from woprog inner join business_unit bu on bu.id = woprog.business_unit_id  where woprog_customer_id =@v0 and woprog_status <> 'Invoiced' and woprog_status is not null and tax_entity_id =@v1 ", new object[] { custdt.Tables[0].Rows[0].ItemArray[0],ddlCompany.SelectedValue });
//                var nextpay = _tools.getSQL_string(@"Select DateDiff(min(ifnull(WOProg_expectedcheckrun,WOProg_InvoiceDate+interval(woprog.woprog_dayscredit)DAY)),CurDate()) from woprog inner join business_unit bu on bu.id = woprog.business_unit_id 
//where tax_entity_id =@v0 and woprog_status = 'Invoiced' and woprog_customer_id =@v1 
//and woprog_invoicednettotal >0 and woprog_invoice_paid_date is null and woprog_invoicedate > '2011-01-01'", new object[] { ddlCompany.SelectedValue,custdt.Tables[0].Rows[0].ItemArray[0] });
//                dr["wos"] = result1;
//                var result3 = Convert.ToDouble(dr["ebalance"]) + Convert.ToDouble(dr["wos"]) -
//                              Convert.ToDouble(dr["credit_limit"]);
//                if (result3 < 0)
//                    result3 = 0;
//                dr["overage"] = result3;
//                dr["creditdays"] = custdt.Tables[0].Rows[0].ItemArray[1].ToString();
//                dr["customer_notes"] = _tools.value_from(custdt.Tables[0].Rows[0].ItemArray[2].ToString());
//                dr["customer_memo"] = _tools.value_from(custdt.Tables[0].Rows[0].ItemArray[4].ToString());
//                dr["customer_salesnotes"] = _tools.value_from(custdt.Tables[0].Rows[0].ItemArray[3].ToString());
//                var status = 0;
//                int.TryParse(custdt.Tables[0].Rows[0].ItemArray[5].ToString(), out status);
//                dr["status_"] = status;
//                dr["customer_id"] = custdt.Tables[0].Rows[0].ItemArray[0].ToString();
//                dr["avg5"] = NECustomer.GetAveragePaymentDelay(Convert.ToInt32(dr["customer_id"]));
//                dr["arnotes"] = _tools.value_from(custdt.Tables[0].Rows[0].ItemArray[6].ToString());
//	                dr["next14"] = _tools.getSQL_double(@"select ifnull(sum(ifnull((woprog.WOProg_InvoicedNetTotal+woprog.woprog_invoice_tax),0)),0)
//from woprog inner join business_unit bu on bu.id = woprog.business_unit_id  where tax_entity_id =@v0 and woprog_customer_id = @v1 and Datediff(woprog_ExpectedCheckRun,@v2)<1 and woprog_invoice_paid_date is null and woprog_invoicednettotal >0 ", new object[] {
//		                ddlCompany.SelectedValue,Convert.ToInt32(dr["customer_id"]), dteScope.Date.ToString("yyyy-MM-dd")
//	});


//				dr["OnHold"] = custdt.Tables[0].Rows[0].ItemArray[7].ToString();
//                dr["cust_whyhold"] = _tools.value_from(custdt.Tables[0].Rows[0].ItemArray[8].ToString());
//                dr["cust_whohold"] = custdt.Tables[0].Rows[0].ItemArray[9].ToString();
//                dr["note_date"] = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(DATE_FORMAT(a.ar_notes_ts, '%Y-%m-%d')), '') FROM ar_notes a LEFT JOIN woprog b ON a.ar_notes_woprogid = b.woprog_id WHERE b.woprog_customer_id = @v0  ORDER BY a.ar_notes_id DESC LIMIT 1", new object[] {  dr["customer_id"] } );
//                try
//                    {
//                    dr["nextexpectedpay"] = Convert.ToInt32(nextpay);
//                    }
//                catch
//                    {
//                    }
//                }
//            Session["ar_report_gv"] = dt;
//            }
//        Aspxgridview1.DataSource = Session["ar_report_gv"];
//        Aspxgridview1.DataBind();
        }

    protected void Button1_Click(object sender, EventArgs e)
        {
        }

    protected void btnExportPDF_Click(object sender, EventArgs e)
        {
        exporter.FileName = ddlCompany.Text + " AR List " + dteScope.Date.ToString("yyyy-MM-dd");
        exporter.WritePdfToResponse();
        }

    protected void btnExportXLS_Click(object sender, EventArgs e)
        {
        exporter.FileName = ddlCompany.Text + " AR List " + dteScope.Date.ToString("yyyy-MM-dd");
        exporter.WriteXlsToResponse();
        }

    protected void Aspxgridview1_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
        var gv = (ASPxGridView) sender;
        e.Properties["cpExp"] = gv.SaveClientLayout();
        }

    protected void Aspxgridview1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
        var gv = (ASPxGridView) sender;
        if (e.Parameters != "")
            {
            gv.LoadClientLayout(e.Parameters);
            }
        else
            {
            gv.FilterExpression = "";
            for (var i = 0; i < gv.Columns.Count; i++)
                if (gv.Columns[i] is GridViewDataColumn)
                    {
                    var col = (GridViewDataColumn) gv.Columns[i];
                    if (col.GroupIndex > -1)
                        gv.UnGroup(col);
                    col.Visible = true;
                    }
            }
        }

    protected void Aspxgridview1_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
        pop_invoiceeditform();
        }

    protected void pop_invoiceeditform()
        {
        var comp2 = new NeTaxEntity(ddlCompany.SelectedValue);
        DSN = comp2.DSN;
        //	ASPxGridView gv2 = (ASPxGridView)Aspxgridview1.FindEditFormTemplateControl("gv_invoices");
        var cust = Aspxgridview1.GetRowValues(Aspxgridview1.EditingRowVisibleIndex, "customer_id").ToString();
        if (cust != Session["ar_report_cust"].ToString())
            Session["ar_report_avgage"] = NECustomer.GetAveragePaymentDelay(Convert.ToInt32(cust)).ToString();
        var lbltypical = (ASPxLabel) Aspxgridview1.FindEditFormTemplateControl("lbltypicalPay");
        lbltypical.Text = "Statistical Days Outstanding for this customer: " + Session["ar_report_avgage"] + " days ";
        var frame = (HtmlContainerControl) Aspxgridview1.FindEditFormTemplateControl("gv_invoice_frame");
        frame.Attributes.Add("src",
            "../master_invoices/index.aspx?customer_id=" + cust + "&te_id=" + comp2.id + "&origin=ar_report");
        frame.Attributes.Add("onload", "resizeIframe(this);");
        var frame2 = (HtmlContainerControl) Aspxgridview1.FindEditFormTemplateControl("frame_notes");
        frame2.Attributes.Add("src", "../../customer/customer_notes.aspx?customer_id=" + cust + "&origin=ar_report");
        frame2.Attributes.Add("onload", "resizeIframe(this);");
        Session["ar_report_cust"] = cust;
        }

    protected void text_container_Init(object sender, EventArgs e)
        {
        var l = (ASPxLabel) sender;
        l.Attributes.Add("data-tooltip", l.Text.Replace("\"", ""));
        l.CssClass = "ttip";
        }

    protected void Aspxgridview1_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
        var pc = (ASPxPageControl) Aspxgridview1.FindEditFormTemplateControl("pc");
        var mem1 = (ASPxMemo) pc.FindControl("customer_notes");
        var mem2 = (ASPxMemo) pc.FindControl("customer_memo");
        var mem3 = (ASPxMemo) pc.FindControl("customer_salesnotes");
        var mem4 = (ASPxMemo) pc.FindControl("customer_arnotes");
        try
            {
            var onhold = e.NewValues["OnHold"].ToString() == "T" ? "T" : "F";
            var creditdays = e.NewValues["creditdays"] == null ? 30 : Convert.ToInt32(e.NewValues["creditdays"]);
            var creditlimit = e.NewValues["credit_limit"] == null ? 0 : Convert.ToInt32(e.NewValues["credit_limit"]);
            var customer_notes = mem1.Text == null ? "" : mem1.Text;
            var customer_memo = mem2.Text == null ? "" : mem2.Text;
            var customer_salesnotes = mem3.Text == null ? "" : mem3.Text;
            var customer_arnotes = mem4.Text == null ? "" : mem4.Text;
            var bv_cust_notes = e.NewValues["bv_cust_notes"] == null ? "" : e.NewValues["bv_cust_notes"].ToString();
            var status = e.NewValues["status_"] == null ? 0 : Convert.ToInt32(e.NewValues["status_"]);
            var customer_whyhold = e.NewValues["cust_whyhold"] == null ? "" : e.NewValues["cust_whyhold"].ToString();
            var customer_whohold = e.NewValues["cust_whohold"] == null ? "" : e.NewValues["cust_whohold"].ToString();
            var old_customer_whyhold = e.OldValues["cust_whyhold"] == null
                ? ""
                : e.OldValues["cust_whyhold"].ToString();
            var cust1 = new NECustomer(Convert.ToInt32(e.Keys["cust"]));
            if (onhold == "F")
                {
                customer_whyhold = "";
                customer_whohold = "0";
                }
            if (customer_whyhold != old_customer_whyhold && onhold == "T")
                customer_whohold = current_user.id.ToString();
            if (e.NewValues["OnHold"].ToString() != e.OldValues["OnHold"].ToString() &&
                e.NewValues["OnHold"].ToString() == "T")
                if (customer_whyhold == null || customer_whyhold == "")
                    throw new Exception("You must enter a reason for putting the customer on hold");
            cust1.Hold = onhold;
            cust1.customer_creditdays = creditdays;
            cust1.CreditLimit = creditlimit;
            cust1.Notes = customer_notes;
            cust1.Memo = customer_memo;
            cust1.salesnotes = customer_salesnotes.Replace("'", "");
            cust1.customer_collection_status = status;
            cust1.customer_arnotes = customer_arnotes.Replace("'", "");
            cust1.customer_whyhold = customer_whyhold.Replace("'", "");
            cust1.customer_whohold = char.IsNumber(customer_whohold, 0) ? Convert.ToInt32(customer_whohold) : 0;
            cust1.Member_ID = current_user.id;
            cust1.customer_whyhold = customer_whyhold.Replace("'", "");
            cust1.Save();
            Session["ar_report_gv"] = null;
            populate_grid();
            }
        catch (Exception ee)
            {
            throw ee;
            }
        e.Cancel = true;
        Aspxgridview1.CancelEdit();
        }

    protected void gv_invoicenotes_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
        var gv = (ASPxGridView) sender;
        if (e.NewValues["note"] != e.OldValues["note"])
            _tools.getSQL_void(@"Update ar_notes  set ar_notes_memberid =@v0, ar_notes_note =@v1 ,ar_notes_ts =@v2   where ar_notes_id =@v3", new object[] { current_user.id,_tools.value_to(e.NewValues["note"]),DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),e.Keys[0] });
        Session["ar_report_cust"] = "";
        e.Cancel = true;
        gv.CancelEdit();
        }

    protected void gv_invoicenotes_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
        var gv = (ASPxGridView) sender;
        if (e.NewValues["note"] != null)
            _tools.getSQL_void(@"Insert into ar_notes (ar_notes_memberid,ar_notes_note,ar_notes_woprogid,ar_notes_ts)  values (@v0,@v1,@v2,@v3)",new object[] { current_user.id,_tools.value_to(e.NewValues["note"].ToString()),woprogid_notes,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") } );
        Session["ar_report_cust"] = "";
        e.Cancel = true;
        gv.CancelEdit();
        }

    protected void sql_invnotes_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
        e.Command.Parameters["@woid"].Value = woprogid_notes;
        }

    protected void gv_emails_SelectionChanged(object sender, EventArgs e)
        {
        var pc = (ASPxPageControl) Aspxgridview1.FindEditFormTemplateControl("pc");
        var gv_emails = (ASPxGridView) sender;
        var _html = (ASPxHtmlEditor) pc.FindControl("aremails");
        var id = gv_emails.GetSelectedFieldValues("emaillog_id")[0].ToString();
        _html.Html = _tools.getSQL_string(@"select emaillog_body from emaillog  where emaillog_id =@v0", new object[] { id });
        }

    protected void dteScope_DateChanged(object sender, EventArgs e)
        {
        Session["ar_report_gv"] = null;
        populate_grid();
        }

    protected void btnUpdate_Click(object sender, EventArgs e)
        {
        }

    protected void Aspxgridview1_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
        }
    }