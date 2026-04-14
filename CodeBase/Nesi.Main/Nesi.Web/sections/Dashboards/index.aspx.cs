using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_dashboards_index : System.Web.UI.Page
{

	NeMember current_user;
	private int page_id = 152;
	Toolbox _tools;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;
	private string _page_name = "New_Dashboard";
	private bool _company_wide = false;
	private bool _region_wide = false;
	private bool _branch_wide = false;
	private bool _base_level = false;
	protected NameValueCollection _q;
	ASPxDateEdit dte;
	protected void Page_Init()
	{
		_q = Request.QueryString;
		_tools = new Toolbox();
		layout.used_gv = gv_wos;
		current_user = Toolbox.do_handle_authentication(page_id);
		_company_wide = current_user.AuthenticatedForPrivilege(146);
		_region_wide = current_user.AuthenticatedForPrivilege(147);
		_branch_wide = current_user.AuthenticatedForPrivilege(149);
		_base_level = current_user.AuthenticatedForPrivilege(150);

		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);


		layout.__page_name = _page_name;
		layout.__gv_id = "ASPxGridView1";
		layout.used_gv = ASPxGridView1;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		dte = (ASPxDateEdit)rp_main.FindControl("dte");
		h.Set("gridview_id", "ASPxGridView1");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
		_tools.dont_cache_page();
		if (Cache["ds_depend"] == null)
		{
			Cache["ds_depend"] = DateTime.Now;
		}
	    SqlDataSource2.SelectCommand = " Select id,name from business_unit where id in(" +
	                                   new Current_User().visible_business_units + ")";
        SqlDataSource2.DataBind();
	    //	fill_wo_grid(true);


	    }
	protected void Page_Load(object sender, EventArgs e)
	{
		var _q = Request.QueryString;
		if (!IsPostBack && !IsCallback)
		{
			if (_q["drilldown"] == null)
			{
				Session["gv_new_dashboard"] = null;
				Session["gv_new_dashboard_wos"] = null;
			}
			else
			{
				//	if (_q["drilldown"] == "openwos")
				//	{
				//		string mid2 = _q["id"];
				//		i_customers.Attributes.Add("src", "customer_grid.aspx?cid=" + current_user.business_unit_id + "&mid=" + mid2);
				//		i_customers.Attributes.Add("onload", "resizeIframe(this);");
				//		i_quotes.Attributes.Add("src", "quote_grid.aspx?cid=" + current_user.business_unit_id + "&mid=" + mid2);
				//		i_quotes.Attributes.Add("onload", "resizeIframe(this);");
				//	}
			}
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				//		gv_invoices.FilterExpression	= string.Format("[name_branch] = '{0}'", current_user.business_unit.name);
				gl.GridLayout_Layout = ASPxGridView1.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				ASPxGridView1.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}

			bonus_panel1.current_user = current_user;


			dte.Date = System.DateTime.Today;
			dde_filter.Text = gl.GridLayout_Name;
		}
		fill_grid();

	}

	protected void fill_wo_grid(bool bypass_selection)
	{
		
			if (bypass_selection || ASPxGridView1.Selection.Count > 0)
			{
				var member_id =  ASPxGridView1.Selection.Count > 0 ? (int) ASPxGridView1.GetSelectedFieldValues("member_id")[0] : current_user.id;
				if (new NeMemberOffer(NeMemberOffer.get_current_agreement(Convert.ToInt32(member_id))).bonus_type == 3)
				{
					bonus_panel1.Visible = true;
					bonus_panel1.loaded_user = new NeMember(Convert.ToInt32(member_id));
					bonus_panel1.DataBind();
					bonus_panel1.fill_details();
				}
				else
				{
					bonus_panel1.Visible = false;
				}


				Session["gv_new_dashboard_wos"] = _tools.getSQL_datatable(@"
SELECT
	WOProg_ID AS id,
	WOProg_BVWO AS bvwo,
	business_unit_id cid,
	woprog_customername AS customer,
	woprog_description AS description,
	woprog_expected_enddate AS exp_completion_date,
	woprog_expected_sales_value AS exp_sales_value,
	IF(
		(SELECT COUNT(po_details_id) FROM poprog_header, po_details_current WHERE po_details_current.is_gl_account =false and poprog_status IN (1,2,5,3,9) AND po_details_woprog_id = a.woprog_id AND po_details_line_active = 1 AND poprog_id = po_details_poprog_id) > 0, 
			CONCAT(woprog_status, ' - Open PO'),
			WOProg_Status
		) AS status,
	woprog_pm_memberid AS mid,
	woprog_grossmargin AS margin, 
	woprog_stilltobebilled tobebilled,
WOProg_CutDateTime woprog_cutdate  
FROM
woprog a where woprog_pm_memberid = @v0 and 
(
(woprog_status in ('Questions For PM','Waiting PM Approval')) or 
(woprog_status = 'Open' and woprog_expected_enddate < curdate()) OR 
(SELECT COUNT(po_details_id) FROM poprog_header, po_details_current WHERE po_details_current.is_gl_account =false and poprog_status IN (1,2,5,3,9) AND po_details_woprog_id = a.woprog_id AND po_details_line_active = 1 AND poprog_id = po_details_poprog_id) > 0
)", new object[] { member_id });

			}
			gv_wos.DataSource = Session["gv_new_dashboard_wos"];
			gv_wos.DataBind();

	}

	protected void fill_grid()
	{
		var ddl = (ASPxComboBox)rp_main.FindControl("ASPxComboBox2");



		var where = "";

		if (Session["gv_new_dashboard"] != null)
		{


		}
		else
		{// put in limiting logic here for whatever levels they can see.
			var member_list = "0," + _tools.getSQL_string(@"SELECT GROUP_CONCAT(distinct fytarget_employees.member_id) FROM fytarget_employees inner join member on member.Member_ID = fytarget_employees.member_id  WHERE find_in_set(member.member_id,@v0)", new object[] { new Current_User().visible_reporting_users });
            member_list = member_list.TrimEnd(',');
            if (_base_level)
			{
				where = "a.member_id = " + current_user.id + " and ";
				fill_wo_grid(true);
				i_customers.Attributes.Add("src", "customer_grid.aspx?cid=" + current_user.business_unit_id + "&mid=" + current_user.id);
				i_customers.Attributes.Add("onload", "resizeIframe(this);");
				i_quotes.Attributes.Add("src", "quote_grid.aspx?cid=" + current_user.business_unit_id + "&mid=" + current_user.id);
				i_quotes.Attributes.Add("onload", "resizeIframe(this);");

			}
            else
            {
                where = " a.member_id in(" + member_list + ") and ";

            }
	//		if (_branch_wide)
	//		{
	//			where = "c.id = " + current_user.business_unit_id + " and a.member_id in(" + member_list + ") and ";
	//		}
	//		if (_region_wide)
	//		{
	//			member_list = "0," + _tools.getSQL_string(@"SELECT GROUP_CONCAT(distinct fytarget_employees.member_id) FROM fytarget_employees", null);
	//			where = "c.region = '" + new NeBusinessUnit(current_user.business_unit_id).region + "' and a.member_id in(" + member_list + ") and ";
//			}
	//		if (_company_wide)
	//		{
	//			member_list = "0," + _tools.getSQL_string(@"SELECT GROUP_CONCAT(distinct fytarget_employees.member_id) FROM fytarget_employees", null);
	//			where = " a.member_id in(" + member_list + ") and ";
	//		}

			var used_date = Toolbox.MySQL_shortdt(dte.Date);
			var used_date_eom = Toolbox.MySQL_shortdt(new DateTime(dte.Date.Year, dte.Date.Month, 1).AddMonths(1).AddDays(-1));
			Session["gv_new_dashboard"] = _tools.getSQL_datatable(string.Format(@"
SELECT
	a.member_id,mt.membertype_name,
	a.member_fullname pm,
	a.business_unit_id branch,
a.business_unit_id ,
	(SELECT Count(*) from woprog w where w.woprog_pm_memberid = a.Member_ID and w.business_unit_id = c.id and w.WOProg_Status = 'Open' ) openwos,
	(SELECT IFNULL(ROUND(SUM(get_invoiced_of_all_wos (woprog.WOProg_ID)),0),0)		FROM woprog	WHERE woprog_pm_memberid = a.Member_ID and business_unit_id = a.business_unit_id and WOProg_Status = 'Invoiced' and woprog.WOProg_Associate_WOProg_ID = 0 and year(WOProg_InvoiceDate) = year('{0}') AND month(WOProg_InvoiceDate) = month('{0}') ) rev_mtd,
	(SELECT IFNULL(ROUND(SUM(woprog_StillToBeBilled),0),0) FROM woprog	WHERE woprog_pm_memberid = a.Member_ID and woprog_expected_enddate BETWEEN '{0}' AND DATE_ADD('{0}', interval 30 day) and business_unit_id = a.business_unit_id  ) next_thirty_days,
	(SELECT IFNULL(ROUND(SUM(woprog_StillToBeBilled),0),0) FROM woprog WHERE woprog_pm_memberid = a.Member_ID and woprog_expected_enddate BETWEEN '{0}' AND DATE_ADD('{0}', interval 60 day) and business_unit_id = a.business_unit_id  ) next_sixty_days,
	(SELECT IFNULL(ROUND(AVG(get_wo_processing_time(wwww.WOProg_ID)),1),0) from woprog wwww where wwww.business_unit_id = a.business_unit_id and wwww.woprog_pm_memberid = a.Member_ID and year(wwww.WOProg_InvoiceDate) = year('{0}') AND month(wwww.WOProg_InvoiceDate) = month('{0}')) avg_days_to_invoice,
	(SELECT ROUND(SUM(_qm.quoted_price)/SUM(_mt.NumberOfHours),0) FROM membertime _mt INNER JOIN quote_master _qm ON _mt.MemberTime_WorkOrder_ID = Concat(_qm.quote_id,_qm.revision) WHERE _qm.quoted_by = a.member_id and _mt.WOType = 'Quote' and _qm.status_id = 8 and _mt.Date > DATE_SUB('{0}', interval 90 day)) dollars_per_quoted_hour,
	ytd_target_for_member(a.member_id, c.id,'Revenue','{0}') target_rev_fytd, 
	IFNULL((SELECT get_month_target(c.id,a.Member_ID, '{0}','Days to Invoice')),0) avg_days_to_invoice_tar,
	IFNULL((SELECT SUM(get_invoiced_of_all_wos (woprog_t.WOProg_ID)) from woprog woprog_t where  woprog_t.WOProg_Associate_WOProg_ID = 0 and woprog_t.business_unit_id = a.business_unit_id and woprog_t.woprog_pm_memberid = a.Member_ID and woprog_t.WOProg_Status = 'Invoiced' and woprog_t.WOProg_InvoiceDate>=get_fy_start_in_year(woprog_t.business_unit_id,'" + dte.Date.ToString("yyyy-MM-dd") + @"')  and woprog_t.WOProg_InvoiceDate<='" + dte.Date.Date.ToString("yyyy-MM") + @"-31' and (woprog_t.woprog_invoicebalance<=0 || (woprog_t.WOProg_InvoiceDate>curdate() - interval 180 day))),0)  ytd,

	IFNULL((SELECT SUM(get_invoiced_of_all_wos (wmarging.WOProg_ID) - wmarging.WOProg_LaborCost - wmarging.WOProg_MaterialCost)/sum(get_invoiced_of_all_wos (wmarging.WOProg_ID)) FROM woprog wmarging WHERE wmarging.WOProg_Associate_WOProg_ID = 0 and wmarging.woprog_pm_memberid = a.Member_ID and wmarging.business_unit_id = c.id and  month(wmarging.WOProg_InvoiceDate) = month('{0}') and year(wmarging.woprog_invoicedate) = year('{0}')) ,0) margin_mtd,

	IFNULL((SELECT SUM(get_invoiced_of_all_wos (wmargingy.WOProg_ID) - wmargingy.WOProg_LaborCost - wmargingy.WOProg_MaterialCost)/sum(get_invoiced_of_all_wos (wmargingy.WOProg_ID)) FROM woprog wmargingy WHERE wmargingy.WOProg_Associate_WOProg_ID = 0 and wmargingy.business_unit_id = c.id and wmargingy.woprog_pm_memberid = a.Member_ID  AND wmargingy.WOProg_InvoiceDate BETWEEN d.fy AND '{1}' ) ,0) margin_ytd,

	IFNULL((SELECT get_month_target(c.id,a.Member_ID,'{0}','revenue')),0) mtd_target,
	IFNULL((SELECT get_month_target(c.id,a.Member_ID,'{0}','margin')),0)/100 target_mar_mtd,
	ytd_target_for_member(a.member_id, c.id,'Margin','{0}') target_mar_fytd
from 
	(member a,
	business_unit c,
	membertype mt)
LEFT JOIN
		(
		SELECT id, get_fy_start_in_year(id,'{0}') fy from business_unit 
		) d ON c.id = d.id
where 
	{2} 
	mt.membertype_id = a.member_membertype_id and 
	Member_Status = 'Active' and 
	c.id = a.business_unit_id 
order by 
	a.business_unit_id",
			used_date,
			used_date_eom,
			where
			), null);


		}
		ASPxGridView1.DataSource = Session["gv_new_dashboard"];
		ASPxGridView1.DataBind();


	}

	protected void ASPxGridView1_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
	{

	}
	protected void ASPxGridView1_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void ASPxGridView1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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

	protected void ASPxGridView1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		#region rev_ytd
		if (e.DataColumn.FieldName == "ytd")
		{
			if ( Toolbox.ReturnZeroIfNull_double(e.CellValue) < Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "target_rev_fytd")))
			{
				e.Cell.BackColor = System.Drawing.Color.MistyRose;

				e.Cell.ToolTip = "Revenue YTD is less than Target";

			}
		}
		else if (e.DataColumn.FieldName == "target_rev_fytd")
		{
			if (Toolbox.ReturnZeroIfNull_double(e.CellValue) > Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "ytd")))
			{

				e.Cell.ToolTip = "Revenue YTD is less than Target";
			}
		}
		#endregion
		#region rev_mtd
		else if (e.DataColumn.FieldName == "rev_mtd")
		{
			if (Toolbox.ReturnZeroIfNull_double(e.CellValue) < Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "mtd_target")))
			{
				e.Cell.BackColor = System.Drawing.Color.MistyRose;
				e.Cell.ToolTip = "Revenue MTD is less than Target";
			}
		}
		else if (e.DataColumn.FieldName == "mtd_target")
		{
			if (Toolbox.ReturnZeroIfNull_double(e.CellValue) > Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "rev_mtd")))
			{

				e.Cell.ToolTip = "Revenue MTD is less than Target";
			}
		}
		#endregion

		#region mar_mtd
		else if (e.DataColumn.FieldName == "margin_mtd")
		{
			if (Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "rev_mtd")) > 0)
			{
				if (Toolbox.ReturnZeroIfNull_double(e.CellValue) < Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "target_mar_mtd")))
				{
					e.Cell.BackColor = System.Drawing.Color.MistyRose;
					e.Cell.ToolTip = "Margin MTD is less than Target";
				}
			}
		}
		else if (e.DataColumn.FieldName == "target_mar_mtd")
		{
			if (Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "rev_mtd")) > 0)
			{
				if (Toolbox.ReturnZeroIfNull_double(e.CellValue) > Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "margin_mtd")))
				{
					e.Cell.ToolTip = "Margin MTD is less than Target";
				}
			}
		}
		#endregion

		#region mar_ytd

		else if (e.DataColumn.FieldName == "margin_ytd")
		{
			if (Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "ytd")) > 0)
			{
				if (Toolbox.ReturnZeroIfNull_double(e.CellValue) < Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "target_mar_fytd")))
				{

					e.Cell.BackColor = System.Drawing.Color.MistyRose;
					e.Cell.ToolTip = "Margin YTD is less than Target";

				}
			}
		}
		else if (e.DataColumn.FieldName == "target_mar_fytd")
		{
			if (Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "ytd")) > 0)
			{
				if(Toolbox.ReturnZeroIfNull_double(e.CellValue) > Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "margin_ytd")))
				{
					e.Cell.ToolTip = "Margin YTD is less than Target";
				}
			}
		}

		#endregion
		#region avg_days to invoice  avg_days_to_invoice_tar

		else if (e.DataColumn.FieldName == "avg_days_to_invoice")
		{
			if (Toolbox.ReturnZeroIfNull_double(e.CellValue) > Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "avg_days_to_invoice_tar")))
			{
				e.Cell.BackColor = System.Drawing.Color.MistyRose;
				e.Cell.ToolTip = "Average days to invoice is greater than target";
			}
		}
		else if (e.DataColumn.FieldName == "avg_days_to_invoice_tar")
		{
			if (Toolbox.ReturnZeroIfNull_double(e.CellValue) < Convert.ToDouble(ASPxGridView1.GetRowValues(Toolbox.ReturnZeroIfNull_int(e.VisibleIndex), "avg_days_to_invoice")))
			{
				e.Cell.ToolTip = "Average days to invoice is greater than target";
			}
		}
		#endregion

	}

	protected void cb_pm_Callback(object sender, CallbackEventArgsBase e)
	{
		var s = e.Parameter.Split('|');

		if (s.Length == 1)
		{
			var mid = e.Parameter;
			if (mid != "null")
			{
				Session["gv_new_dashboard_wos"] = null;
				var m = new NeMember(Convert.ToInt32(mid));
				fill_wo_grid(false);
				i_customers.Attributes.Add("src", "customer_grid.aspx?cid=" + m.business_unit_id + "&mid=" + mid);
				i_customers.Attributes.Add("onload", "resizeIframe(this);");
				i_quotes.Attributes.Add("src", "quote_grid.aspx?cid=" + m.business_unit_id + "&mid=" + mid);
				i_quotes.Attributes.Add("onload", "resizeIframe(this);");

			}
		}
		else
		{
			var mid = s[1];
			var _type = s[2] == null ? "" : s[2];
			//			i_customers.Attributes.Add("src", "customer_grid.aspx?cid=" + current_user.business_unit_id + "&mid=" + mid);
			//			i_customers.Attributes.Add("onload", "resizeIframe(this);");
			//			i_quotes.Attributes.Add("src", "quote_grid.aspx?cid=" + current_user.business_unit_id + "&mid=" + mid);
			//			i_quotes.Attributes.Add("onload", "resizeIframe(this);");

			if (s[0] == "wos")
			{
				var m = new NeMember(Convert.ToInt32(mid));
				ASPxPopupControl1.HeaderText = "Work Orders Managed By " + new NeMember(Convert.ToInt32(mid)).FullName2 + " for " + dte.Date.ToString("yyyy-MMMM");
				ASPxPopupControl1.ShowOnPageLoad = true;

				var i_drill = (HtmlContainerControl)ASPxPopupControl1.FindControl("i_drill");
				i_drill.Attributes.Add("src", "wo_grid.aspx?cid=" + m.business_unit_id + "&mid=" + mid + "&type=" + _type + "&date=" + dte.Date);
				//		i_drill.Attributes.Add("onload", "resizeIframe(this);");
			}

		}

	}
	protected void ASPxHyperLink1_Init(object sender, EventArgs e)
	{
		var hl = (ASPxHyperLink)sender;
		var container = hl.NamingContainer as GridViewDataItemTemplateContainer;
		hl.ClientSideEvents.Click = string.Format(@"function (s, e) {{
																	cb_pm.PerformCallback('wos|{0}|open');
																	}}", container.KeyValue);



	}
	protected void ASPxTextBox1_Init(object sender, EventArgs e)
	{
		var txtsv = sender as ASPxTextBox;
		var container = txtsv.NamingContainer as GridViewDataItemTemplateContainer;
		txtsv.ClientSideEvents.TextChanged = string.Format(@"function (s, e) {{gv_wos.PerformCallback('{0}|' + s.GetValue() + '|s'); }}", container.KeyValue);
	}
	protected void ASPxDateEdit1_Init(object sender, EventArgs e)
	{
		var dateedit = sender as ASPxDateEdit;
		var container = dateedit.NamingContainer as GridViewDataItemTemplateContainer;
		dateedit.ClientSideEvents.DateChanged = string.Format(@"function (s, e) {{ 
var jsDate = s.GetDate();

var year = jsDate.getFullYear(); 
var month = jsDate.getMonth(); 
var day = jsDate.getDate();   

var myDate = year;
myDate += '/';
myDate += month+1;
myDate += '/';
myDate += day;

gv_wos.PerformCallback('{0}|' + myDate + '|d'); }}", container.KeyValue);
	}


	protected void gv_wos_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters.Length > 0)
		{
			var p = e.Parameters.Split('|');
			if (p.Length > 1)
			{
				if (p[2] == "s")
				{
					try
					{
						_tools.getSQL_void(@"update woprog set woprog.woprog_expected_sales_value =@v0  where woprog.woprog_id =@v1  limit 1",
							new object[] {
Convert.ToDouble(p[1]),p[0]
							});

					}
					catch { }
					Session["gv_new_dashboard_wos"] = null;
					fill_wo_grid(!_branch_wide);
				}
				else if (p[2] == "d")
				{
					_tools.getSQL_void(@"update woprog set woprog.woprog_expected_enddate = @v0 where woprog.woprog_id =@v1 limit 1",
						new object[] {
 p[1], p[0]
						});
					Session["gv_new_dashboard_wos"] = null;
					fill_wo_grid(!_branch_wide);
				}

			}
		}
	}
	protected void ASPxComboBox2_SelectedIndexChanged(object sender, EventArgs e)
	{

	}
	protected void ASPxHyperLink2_Init(object sender, EventArgs e)
	{
		var hl = (ASPxHyperLink)sender;
		var container = hl.NamingContainer as GridViewDataItemTemplateContainer;
		hl.ClientSideEvents.Click = string.Format(@"function (s, e) {{
																	cb_pm.PerformCallback('wos|{0}|ad');
																	}}", container.KeyValue);
	}
	protected void ASPxHyperLink3_Init(object sender, EventArgs e)
	{
		var hl = (ASPxHyperLink)sender;
		var container = hl.NamingContainer as GridViewDataItemTemplateContainer;
		hl.ClientSideEvents.Click = string.Format(@"function (s, e) {{
																	cb_pm.PerformCallback('wos|{0}|mtd');
																	}}", container.KeyValue);
		hl.Text = string.Format("{0:C}", container.Text);
	}
	protected void ASPxHyperLink4_Init(object sender, EventArgs e)
	{
		var hl = (ASPxHyperLink)sender;
		var container = hl.NamingContainer as GridViewDataItemTemplateContainer;
		hl.ClientSideEvents.Click = string.Format(@"function (s, e) {{
																	cb_pm.PerformCallback('wos|{0}|mtd');
																	}}", container.KeyValue);
		hl.Text = string.Format("{0:P}", container.Text);
	}
	protected void ASPxHyperLink5_Init(object sender, EventArgs e)
	{
		var hl = (ASPxHyperLink)sender;
		var container = hl.NamingContainer as GridViewDataItemTemplateContainer;
		hl.ClientSideEvents.Click = string.Format(@"function (s, e) {{
																	cb_pm.PerformCallback('wos|{0}|ytd');
																	}}", container.KeyValue);
		hl.Text = string.Format("{0:C}", container.Text);


	}
	protected void ASPxHyperLink6_Init(object sender, EventArgs e)
	{
		var hl = (ASPxHyperLink)sender;
		var container = hl.NamingContainer as GridViewDataItemTemplateContainer;
		hl.ClientSideEvents.Click = string.Format(@"function (s, e) {{
																	cb_pm.PerformCallback('wos|{0}|ytd');
																	}}", container.KeyValue);
		hl.Text = string.Format("{0:P}", container.Text);
	}
	protected void gv_wos_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
	{

	}
	protected void dte_DateChanged(object sender, EventArgs e)
	{
		Session["gv_new_dashboard"] = null;

		fill_grid();

	}
	protected void gv_wos_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (e.DataColumn.FieldName == "status")
			{
				if (e.CellValue.ToString() == "Questions For PM" || e.CellValue.ToString() == "Waiting PM Approval")
				{
					e.Cell.BackColor = System.Drawing.Color.Red;
					e.Cell.ToolTip = "Status = " + Toolbox.ReturnBlankIfNull_string(e.CellValue);

				}
			}
			else if (e.DataColumn.FieldName == "exp_completion_date")
			{
				//if (e.CellValue != null && e.CellValue.ToString() != "")
                if(Toolbox.ReturnNullDateTime(e.CellValue)!=null)
				{
					if (Convert.ToDateTime(e.CellValue) <= System.DateTime.Today)
					{
						e.Cell.BackColor = System.Drawing.Color.Red;
						e.Cell.ToolTip = "Work order completed date is overdue";
					}
				}

			}

		}
	}
	protected void ASPxGridView1_FocusedRowChanged(object sender, EventArgs e)
	{

	}
	
	}
