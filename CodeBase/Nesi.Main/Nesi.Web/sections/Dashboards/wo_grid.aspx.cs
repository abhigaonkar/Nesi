using System;
using System.Collections.Specialized;
using DevExpress.Web;
using NESI.Common.Models;
using nesi.core;

public partial class sections_dashboards_wo_grid : System.Web.UI.Page
	{
		NeMember current_user;
		private int page_id = 32;
		Toolbox _tools;
	
	private NeBusinessUnit c;
	private bool _company_wide = false;
	private bool _region_wide = false;
	private bool _department_wide = false;
	private bool _branch_wide = false;
	private bool _base_level = false;
	protected NameValueCollection _q;

	protected void Page_Init()
	{
		_q = Request.QueryString;
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		_company_wide = current_user.AuthenticatedForPrivilege(146);
		_region_wide = current_user.AuthenticatedForPrivilege(147);
		_department_wide = current_user.AuthenticatedForPrivilege(148);
		_branch_wide = current_user.AuthenticatedForPrivilege(149);
		_base_level = current_user.AuthenticatedForPrivilege(150);
	    _tools.dont_cache_page();
		if (Cache["ds_depend"] == null)
		{
			Cache["ds_depend"] = DateTime.Now;
		}

	}
		protected void Page_Load(object sender, EventArgs e)
		{
			var _q = Request.QueryString;
			c = new NeBusinessUnit(_q["cid"]);

			if ((!IsPostBack)&&(!IsCallback))
			{
				Session["gv_new_dashboard_drill"] = null;
			}
			fill_wo_grid();
		}


		

		
		protected void ASPxGridView1_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
			var gv = (ASPxGridView)sender;
			e.Properties["cpExp"] = gv.SaveClientLayout();
		}
	

		protected void ASPxGridView1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			
		}
		protected void ASPxGridView1_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{
			if (e.VisibleIndex >= 0)
			{
			
			}
		}
		protected void ASPxGridView1_CustomCallback1(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
							_tools.getSQL_void(@"update woprog set woprog.woprog_expected_sales_value = @v0 where woprog.woprog_id = @v1 limit 1",
								new object[] { 
									Convert.ToDouble(p[1]), p[0]
									});
							
						}
						catch { }
						Session["gv_new_dashboard_drill"] = null;
						fill_wo_grid();
					}
					else if (p[2] == "d")
					{
						_tools.getSQL_void(@"update woprog set woprog.woprog_expected_enddate =@v0  where woprog.woprog_id =@v1 limit 1",
							new object[] { 
								p[1], p[0]
								});
						Session["gv_new_dashboard_drill"] = null;
						fill_wo_grid();
					}
					
				}
			}
		}
		protected void ASPxDateEdit1_Init(object sender, EventArgs e)
		{
			var dateedit = sender as ASPxDateEdit;

			if (_q["type"] == "open")
			{
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

ASPxGridView1.PerformCallback('{0}|' + myDate + '|d'); }}", container.KeyValue);
			}
			else
			{
				dateedit.ClientEnabled = false;
			}
		}
		protected void ASPxTextBox1_Init(object sender, EventArgs e)
		{
			var txtsv = sender as ASPxTextBox;
			if (_q["type"] == OpsWOStatus.Open)
			{
			var container = txtsv.NamingContainer as GridViewDataItemTemplateContainer;
			txtsv.ClientSideEvents.TextChanged = string.Format(@"function (s, e) {{ASPxGridView1.PerformCallback('{0}|' + s.GetValue() + '|s'); }}", container.KeyValue);
			}
			else
			{
				txtsv.ClientEnabled = false;
			}
		}
		protected void fill_wo_grid()
		{
			var dt = Convert.ToDateTime(_q["date"]);
			var yo = new NeMember(Convert.ToInt32(_q["mid"]));
				if (Session["gv_new_dashboard_drill"] != null)
				{

				}
				else
				{
					if (_q["type"] == "open")
					{
						Session["gv_new_dashboard_drill"] = _tools.getSQL_datatable(@"SELECT
woprog.WOProg_ID AS woprog_id,
woprog.business_unit_id cid,
woprog.WOProg_BVWO AS bvwo,
woprog.woprog_customername AS Customer,
woprog.woprog_description AS description,
woprog.woprog_expected_enddate AS exp_completion_date,
woprog.woprog_expected_sales_value AS exp_sales_value,
woprog.WOProg_Status AS `status`,
woprog.woprog_pm_memberid AS mid,
woprog.woprog_grossmargin AS margin, woprog_stilltobebilled tobebilled,
0 days_to_process, '' as inv_date,
(woprog_stilltobebilled-(woprog.WOProg_MaterialCost+woprog.WOProg_LaborCost)) dollars_margin

FROM
woprog where woprog.woprog_pm_memberid = " + _q["mid"] + " and business_unit_id = " + yo.business_unit_id + " and woprog_status = 'Open' order by woprog_id",null);
					}
					else if (_q["type"] == "ad")
					{
						Session["gv_new_dashboard_drill"] = _tools.getSQL_datatable(@"SELECT
woprog.WOProg_ID AS woprog_id,
woprog.business_unit_id cid,
woprog.WOProg_BVWO AS bvwo,
woprog.woprog_customername AS Customer,
woprog.woprog_description AS description,
woprog.woprog_expected_enddate AS exp_completion_date,
woprog.woprog_expected_sales_value AS exp_sales_value,
woprog.WOProg_Status AS `status`,
woprog.woprog_pm_memberid AS mid,
get_margin_of_all_wos(woprog.WOProg_ID) AS margin, get_invoiced_of_all_wos (woprog.WOProg_ID) tobebilled,
ifnull(get_wo_processing_time(WOProg_ID),0) days_to_process, woprog_invoicedate as inv_date  ,
get_margin_dollars_of_all_wos(woprog.WOProg_ID) dollars_margin
FROM
woprog where
woprog_pm_memberid = " + _q["mid"] + @" and business_unit_id = " + yo.business_unit_id + "  and year(WOProg_InvoiceDate) = year('" + dt.Date.ToString("yyyy-MM-dd") + "') AND month(WOProg_InvoiceDate) = month('" + dt.Date.ToString("yyyy-MM-dd") + "') order by ifnull(round(get_wo_processing_time(WOProg_ID),0),0) desc"
,null);

					}
					else if (_q["type"] == "mtd")
					{
						Session["gv_new_dashboard_drill"] = _tools.getSQL_datatable(@"SELECT
woprog.WOProg_ID AS woprog_id,
woprog.business_unit_id cid,
woprog.WOProg_BVWO AS bvwo,
woprog.woprog_customername AS Customer,
woprog.woprog_description AS description,
woprog.woprog_expected_enddate AS exp_completion_date,
woprog.woprog_expected_sales_value AS exp_sales_value,
woprog.WOProg_Status AS `status`,
woprog.woprog_pm_memberid AS mid,
get_margin_of_all_wos(woprog.WOProg_ID) AS margin, get_invoiced_of_all_wos (woprog.WOProg_ID) tobebilled,
ifnull(round(get_wo_processing_time(WOProg_ID),0),0) days_to_process, woprog_invoicedate as inv_date,
 get_margin_dollars_of_all_wos(woprog.WOProg_ID)  dollars_margin
FROM
woprog where woprog.woprog_associate_woprog_id=0 and woprog_pm_memberid = " + _q["mid"] + " and business_unit_id = " + yo.business_unit_id + @"
and year(WOProg_InvoiceDate) = year('" + dt.Date.ToString("yyyy-MM-dd") + "') and month(WOProg_InvoiceDate)= month('" + dt.Date.ToString("yyyy-MM-dd") + "') order by WOProg_InvoiceDate",null);
					}
					else if (_q["type"] == "ytd")
					{
						Session["gv_new_dashboard_drill"] = _tools.getSQL_datatable(@"SELECT
woprog.WOProg_ID AS woprog_id,
woprog.business_unit_id cid,
woprog.WOProg_BVWO AS bvwo,
woprog.woprog_customername AS Customer,
woprog.woprog_description AS description,
woprog.woprog_expected_enddate AS exp_completion_date,
woprog.woprog_expected_sales_value AS exp_sales_value,
woprog.WOProg_Status AS `status`,
woprog.woprog_pm_memberid AS mid,
get_margin_of_all_wos(woprog.WOProg_ID) AS margin, 
get_invoiced_of_all_wos (woprog.WOProg_ID) tobebilled,
ifnull(round(get_wo_processing_time(WOProg_ID),0),0) days_to_process, woprog_invoicedate as inv_date ,
 get_margin_dollars_of_all_wos (woprog.WOProg_ID)  dollars_margin
FROM
woprog where woprog.woprog_associate_woprog_id=0 and woprog_pm_memberid = " + _q["mid"] + @"
and business_unit_id = " + yo.business_unit_id + @" and 
WOProg_InvoiceDate>get_fy_start_in_year(woprog.business_unit_id,'" + dt.Date.ToString("yyyy-MM-dd") + @"')
and woprog.WOProg_InvoiceDate<='" + dt.Date.ToString("yyyy-MM") + @"-31'
 and (woprog.woprog_invoicebalance<=0 || (woprog.WOProg_InvoiceDate>curdate() - interval 180 day))
order by WOProg_InvoiceDate",null);
					}


				}
				ASPxGridView1.DataSource = Session["gv_new_dashboard_drill"];

				ASPxGridView1.DataBind();
				if (_q["type"] == "open")
				{
					ASPxGridView1.Columns["days_to_process"].Visible = false;
				}
				if ((_q["type"] == "ad") || (_q["type"] == "mtd") || (_q["type"] == "ytd"))
				{
					ASPxGridView1.Columns["tobebilled"].Caption = "Invoiced Total";
					ASPxGridView1.TotalSummary["tobebilled"].ShowInColumn = "Invoiced Total";

				}

			
		}
		protected void ASPxGridView1_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
		{
			if (e.IsTotalSummary)
			{
				var dt = Convert.ToDateTime(_q["date"]);
				if (e.Item.FieldName == "margin")
				{
					try
					{
						if (_q["type"] == "open")
						{
							var margin = _tools.getSQL_double(@"Select ifnull((SELECT (sum(woprog_stilltobebilled) - sum(woprog_laborcost + woprog_materialcost))/ sum(woprog_stilltobebilled) as margin 
FROM woprog   where woprog.woprog_pm_memberid =@v0 and woprog_status = 'Open'),0)", new object[] { _q["mid"] });

							e.Text = margin.ToString("P1");
						}
						else if ((_q["type"] == "ad") || (_q["type"] == "mtd"))
						{
							var margin = _tools.getSQL_double(@"Select ifnull((SELECT
(sum(get_invoiced_of_all_wos (woprog.WOProg_ID)) - sum(woprog_laborcost + woprog_materialcost))/ sum(get_invoiced_of_all_wos (woprog.WOProg_ID)) as margin
FROM woprog where woprog.woprog_associate_woprog_id=0 and woprog_pm_memberid = @v0  
   and year(WOProg_InvoiceDate) = year(@v1) and month(WOProg_InvoiceDate) = month(@v2)),0)" , new object[] {_q["mid"], dt.Date.ToString("yyyy-MM-dd"), dt.Date.ToString("yyyy-MM-dd")
						});

							e.Text = margin.ToString("P1");
						}
						else if (_q["type"] == "ytd")
						{
							var margin = _tools.getSQL_double(@"Select ifnull((SELECT
(sum(get_invoiced_of_all_wos (woprog.WOProg_ID)) - sum(woprog_laborcost + woprog_materialcost))/ sum(get_invoiced_of_all_wos (woprog.WOProg_ID)) as margin
FROM
woprog where woprog.woprog_associate_woprog_id=0 and woprog_pm_memberid =@v0
 and WOProg_InvoiceDate>get_fiscal_year_start_date(woprog.business_unit_id) and woprog.WOProg_InvoiceDate<=CONCAT(@v1,'-31') order by WOProg_InvoiceDate),0)",
								new object[] { _q["mid"],dt.Date.ToString("yyyy-MM")});

							e.Text = margin.ToString("P1");
						}

					}
					catch { e.Text = "ERR"; }

					

				}
			}
		}
		protected void ASPxGridView1_HtmlDataCellPrepared1(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			if (_q["type"] == "open")
			{
				if (e.DataColumn.FieldName == "exp_completion_date")
				{
					try
					{
                    if (Toolbox.ReturnNullDateTime(e.CellValue) != null)
                    {
                        if (Convert.ToDateTime(e.CellValue) < System.DateTime.Today)
                        {
                            e.Cell.BackColor = System.Drawing.Color.Red;

                        }
                    }
					}
					catch { }


				}
			}
			else
			{
				if (e.DataColumn.FieldName == "days_to_process")
				{
					try
					{
						if (Toolbox.ReturnZeroIfNull_int(e.CellValue)>5)
						{
			//				e.Cell.BackColor = System.Drawing.Color.Red;

						}
					}
					catch { }
				}
			}
		}
}
