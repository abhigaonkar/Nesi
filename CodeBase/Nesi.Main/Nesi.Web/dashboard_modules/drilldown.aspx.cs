using System;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Data;
using System.Text;
using nesi.core;

public partial class dashboard_modules_drilldown_revenue : System.Web.UI.Page
{
	Toolbox _tools		= new Toolbox();
    protected void Page_Load(object sender, EventArgs e)
		{
		var _q	= Request.QueryString;
		_tools.dont_cache_page();
		var business_unit_id			= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		if(business_unit_id == 0)
			{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
			}
		}

	protected void drilldown_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
       
       
		
		var cbp				= (ASPxCallbackPanel) sender;
		ASPxPanel pan;
		var debit_mat_accts				= "50100,50109,50110,50111,50510,50610,50620,52005,53005,59505,66110,88350";
		var debit_lab_accts				= "51105,51125,51135,51145,51165,51510,51515,51520,51525,51530,51590,51610,51615,51620,51625,51905,51990,76058";
		var debit_accts					= debit_lab_accts+","+debit_mat_accts;
		if(e.Parameter.Contains("|"))
			{
			var param				= e.Parameter.Split('|');
			pan							= (ASPxPanel)cbp.FindControl("p_" + param[0]);
			}
		else
			{
			pan							= (ASPxPanel)cbp.FindControl("p_" + e.Parameter);
			} 
		if(pan == null)
			{
			throw new Exception("Not a valid choice");
			}
		var business_unit_id						= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		if(business_unit_id == 0)
			{
			return;
			}
		var c							= new NeBusinessUnit(business_unit_id);
		var month_table					= "";
		var lastyear_month_table			= "";
		var lastmonth_table				= "";
		var budget_month_table			= "";
		var lastyear_total_tables		= "";
		var total_tables					= "";
		var DATE = DateTime.Now;
		var total_budget_tables			= "";
		var fiscal_year_start			= Toolbox.MySQL_shortdt(c.fiscal_start_current);
		var fiscal_year_end				= Toolbox.MySQL_shortdt(c.fiscal_end_current);
	

		#region Table name(s) assignment loop
		var month_int_fortotals				= DateTime.Now.Month;
		var month_int_formonth				= NeBusinessUnit.FiscalMonthLookup[c.fiscal_yearstart_month][DateTime.Now.Month];

		for(var m = 1; m <= month_int_fortotals; m++)
			{
			var pre				= m < 10 ? "0" : "";
			var month			= pre+m;
			var suff				= m != month_int_fortotals ? "+" : "";	
			total_tables			+= "this_yr"+month+suff;
			total_budget_tables		+= "bdgt_this_yr"+month+suff;
			lastyear_total_tables	+= "last_yr"+month+suff;
			}

		month_table				= "this_yr"+string.Format("{0:00}", month_int_formonth);
		lastmonth_table			= month_int_formonth > 1 ? "this_yr"+string.Format("{0:00}", month_int_formonth - 1) : "last_yr12";
		lastyear_month_table	= "last_yr"+string.Format("{0:00}", month_int_formonth);
		budget_month_table		= "bdgt_this_yr"+string.Format("{0:00}", month_int_formonth);
		#endregion
		var sql		= "";
		var div_addon							= "";
		if(e.Parameter.Contains("|"))
			{
			#region Worst customer drilldown
			var param					= e.Parameter.Split('|');
			var customer_id				= param[1];
			sql								= string.Format(@"
SELECT
	a.woprog_customer_id id,
	a.woprog_id woprog_id,
	a.woprog_bvwo wo,
	REPLACE(a.woprog_description, '\n', ' ') descript,
	DATEDIFF(a.woprog_invoice_paid_date, a.woprog_invoicedate) dso
FROM
	woprog a
LEFT JOIN
	customer b
        ON a.woprog_customer_id = b.customer_id
LEFT JOIN
	customer_sales_properties c 
		ON	b.customer_id = c.customer_id AND
			c.address_id = a.woprog_address_id
WHERE
	a.business_unit_id = {0} AND
	a.woprog_customer_id = {1} AND
	c.status_id NOT IN (4,5,6) AND
	a.woprog_invoice_paid_date IS NOT NULL AND
	a.woprog_invoicedate IS NOT NULL AND
	a.woprog_status = 'Invoiced'
GROUP BY
	woprog_id
ORDER BY
	dso DESC", business_unit_id, customer_id);
			worstcustomers.InnerHtml				= gen_rep(sql, "worst");
			#endregion Worst customer drilldown
			}
		else
			{
				switch (e.Parameter)
				{
					#region Revenue Month to Date
					case "rev_mtd":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({0}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'C' AND 
	gl_group >= 400 
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
					month_table,			// {0}
					lastyear_month_table,	// {1}
					total_tables,           // {2}
					budget_month_table,		// {4}
					total_budget_tables,	// {5}
					lastyear_total_tables,	// {6}
					lastmonth_table			// {7}
					);
						//rev_mtd.InnerHtml = gl_rep(sql);
						break;
					#endregion Revenue Month to Date
					#region Revenue Fiscal Year to Date
					case "rev_fytd":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({2}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'C' AND 
	gl_group >= 400  
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
					month_table,			// {0}
					lastyear_month_table,	// {1}
					total_tables,			// {2}
					business_unit_id,						// {3}
					budget_month_table,		// {4}
					total_budget_tables,	// {5}
					lastyear_total_tables,	// {6}
					lastmonth_table			// {7}
					);
						//rev_fytd.InnerHtml = gl_rep(sql);
						break;
					#endregion Revenue Fiscal Year to Date
					#region Revenue Budget this Month
					case "rev_btm":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({4}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'C' AND 
	gl_group >= 400  AND
	business_unit_id = '{3}'
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
					month_table,			// {0}
					lastyear_month_table,	// {1}
					total_tables,			// {2}
					business_unit_id,						// {3}
					budget_month_table,		// {4}
					total_budget_tables,	// {5}
					lastyear_total_tables,	// {6}
					lastmonth_table			// {7}
					);
						//rev_btm.InnerHtml = gl_rep(sql);
						break;
					#endregion Revenue Budget this Month
					#region Revenue Fiscal this Month
					case "rev_ftm":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({5}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'C' AND 
	gl_group >= 400  AND
	business_unit_id = '{3}'
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
					month_table,			// {0}
					lastyear_month_table,	// {1}
					total_tables,			// {2}
					business_unit_id,						// {3}
					budget_month_table,		// {4}
					total_budget_tables,	// {5}
					lastyear_total_tables,	// {6}
					lastmonth_table			// {7}
					);
						//rev_ftm.InnerHtml = gl_rep(sql);
						break;
					#endregion Revenue Fiscal this Month
					#region Labor Month to Date
					case "lab_mtd":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({0}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'D' AND 
	acct_no IN ({8}) AND
	business_unit_id = '{3}'
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
					month_table,			// {0}
					lastyear_month_table,	// {1}
					total_tables,			// {2}
					business_unit_id,					// {3}
					budget_month_table,		// {4}
					total_budget_tables,	// {5}
					lastyear_total_tables,	// {6}
					lastmonth_table,		// {7}
					debit_lab_accts			// {8}
					);
						//lab_mtd.InnerHtml = gl_rep(sql);
						break;
					#endregion Labor Month to Date
					#region Labor Fiscal Year to Date
					case "lab_fytd":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({2}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'D' AND 
	acct_no IN ({8}) AND
	business_unit_id = '{3}'
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
					month_table,			// {0}
					lastyear_month_table,	// {1}
					total_tables,			// {2}
					business_unit_id,					// {3}
					budget_month_table,		// {4}
					total_budget_tables,	// {5}
					lastyear_total_tables,	// {6}
					lastmonth_table,		// {7}
					debit_lab_accts			// {8}
					);
						//lab_fytd.InnerHtml = gl_rep(sql);
						break;
					#endregion Labor Fiscal Year to Date
					#region Labor Budget this Month
					case "lab_btm":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({5}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'D' AND 
	acct_no IN ({8}) AND
	business_unit_id = '{3}'
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
					month_table,			// {0}
					lastyear_month_table,	// {1}
					total_tables,			// {2}
					business_unit_id,					// {3}
					budget_month_table,		// {4}
					total_budget_tables,	// {5}
					lastyear_total_tables,	// {6}
					lastmonth_table,		// {7}
					debit_lab_accts			// {8}
					);
						//lab_btm.InnerHtml = gl_rep(sql);
						break;
					#endregion Labor Budget this Month
					#region Labor Fiscal this Month
					case "lab_ftm":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({4}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'D' AND 
	acct_no IN ({8}) AND
	business_unit_id = '{3}'
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
					month_table,			// {0}
					lastyear_month_table,	// {1}
					total_tables,			// {2}
					business_unit_id,					// {3}
					budget_month_table,		// {4}
					total_budget_tables,	// {5}
					lastyear_total_tables,	// {6}
					lastmonth_table,		// {7}
					debit_lab_accts			// {8}
					);
						//lab_ftm.InnerHtml = gl_rep(sql);
						break;
					#endregion Labor Fiscal this Month
					#region Material Month to Date
					case "mat_mtd":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({0}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'D' AND 
	acct_no IN ({8}) AND
	business_unit_id = '{3}'
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
						month_table,			// {0}
						lastyear_month_table,	// {1}
						total_tables,			// {2}
						business_unit_id,					// {3}
						budget_month_table,		// {4}
						total_budget_tables,	// {5}
						lastyear_total_tables,	// {6}
						lastmonth_table,		// {7}
						debit_mat_accts			// {8}
						);
						//mat_mtd.InnerHtml = gl_rep(sql);
						break;
					#endregion Material Month to Date
					#region Material Fiscal Year to Date
					case "mat_fytd":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({2}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'D' AND 
	acct_no IN ({8}) AND
	business_unit_id = '{3}'
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
						month_table,			// {0}
						lastyear_month_table,	// {1}
						total_tables,			// {2}
						business_unit_id,					// {3}
						budget_month_table,		// {4}
						total_budget_tables,	// {5}
						lastyear_total_tables,	// {6}
						lastmonth_table,		// {7}
						debit_mat_accts			// {8}
						);
						//mat_fytd.InnerHtml = gl_rep(sql);
						break;
					#endregion Material Fiscal Year to Date
					#region Material Budget this Month
					case "mat_btm":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({4}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'D' AND 
	acct_no IN ({8}) AND
	business_unit_id = '{3}'
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
						month_table,			// {0}
						lastyear_month_table,	// {1}
						total_tables,			// {2}
						business_unit_id,					// {3}
						budget_month_table,		// {4}
						total_budget_tables,	// {5}
						lastyear_total_tables,	// {6}
						lastmonth_table,		// {7}
						debit_mat_accts			// {8}
						);
						//mat_btm.InnerHtml = gl_rep(sql);
						break;
					#endregion Material Budget this Month
					#region Material Fiscal this Month
					case "mat_ftm":
						sql = string.Format(@"
SELECT 
	acct_no,
	UPPER(name) name,
	IFNULL(SUM({5}), 0) amount
FROM 
	GL_CHART_OF_ACCOUNTS 
WHERE 
	dr_cr_desig = 'D' AND 
	acct_no IN ({8}) AND
	business_unit_id = '{3}'
GROUP BY 
	acct_no, name
HAVING 
	amount != 0
ORDER BY name",
						month_table,			// {0}
						lastyear_month_table,	// {1}
						total_tables,			// {2}
						business_unit_id,					// {3}
						budget_month_table,		// {4}
						total_budget_tables,	// {5}
						lastyear_total_tables,	// {6}
						lastmonth_table,		// {7}
						debit_mat_accts			// {8}
						);
						//mat_ftm.InnerHtml = gl_rep(sql);
						break;
					#endregion Material Fiscal this Month
					#region Margin Month to Date
					case "margin_mtd":
						margin_mtd.InnerHtml = "<b></b>";
						break;
					#endregion Margin Month to Date
					#region Margin Fiscal Year to Date
					case "margin_fytd":
						margin_fytd.InnerHtml = "<b></b>";
						break;
					#endregion Margin Fiscal Year to Date
					#region Customers Fiscal Year to Date
					case "cust_fytd":
								sql = string.Format(@"
SELECT 
	a.woprog_customer_id id,
	b.customer_number number,
	b.customer_name name,
	MAX(a.woprog_invoicedate) inv_date
FROM 
	woprog a
LEFT JOIN
	customer b ON a.woprog_customer_id = b.customer_id
LEFT JOIN 
	business_unit c ON a.business_unit_id = c.id
WHERE 
	a.business_unit_id = '{0}' AND 
	
	a.woprog_invoicedate > '{2}' AND
	b.customer_name NOT LIKE '%NEW ELECTRIC%'
GROUP BY
	a.woprog_customer_id
ORDER BY 
	inv_date DESC", c.id, 0, Toolbox.MySQL_shortdt(c.fiscal_start_current));
						cust_fytd.InnerHtml = gen_rep(sql, "customer");
						break;
					#endregion Customers Fiscal Year to Date
					#region Customers Budget Year to Date
					case "cust_bytd":
						cust_bytd.InnerHtml = "<b></b>";
						break;
					#endregion Customers Budget Year to Date
					#region 90plus
					case "90plus":
						
						sql = string.Format(@"
SELECT
	b.customer_id cust_id,
	b.customer_number_int cust_no,
	b.customer_name cust_name,
	a.quote_id,
	a.revision,
	a.quoted_by quoted_by_id,
	MEMBER_NAME(a.quoted_by) quoted_by_name,
	IFNULL(a.quoted_price,0) quoted_price
FROM 
	quote_master a 
LEFT JOIN
	customer b ON a.customer_id = b.customer_id
WHERE 
	a.business_unit_id = {0} AND 
	a.pct_chance >= 90 AND 
	a.status_id NOT IN (1,6,7,8,9) AND 
	completion_date BETWEEN NOW() AND DATE_ADD(NOW(), INTERVAL 1 MONTH)
", business_unit_id);

						d90plus.InnerHtml = gen_rep(sql, "90plus");
						break;
					#endregion 90plus
					#region 60to90
					case "60to90":
					
						sql = string.Format(@"
SELECT
	b.customer_id cust_id,
	b.customer_number_int cust_no,
	b.customer_name cust_name,
	a.quote_id,
	a.revision,
	a.quoted_by quoted_by_id,
	MEMBER_NAME(a.quoted_by) quoted_by_name,
	IFNULL(a.quoted_price,0) quoted_price
FROM 
	quote_master a 
LEFT JOIN
	customer b ON a.customer_id = b.customer_id
WHERE 
	a.business_unit_id = {0}  AND 
	a.pct_chance BETWEEN 60 AND 90 AND 
	a.status_id NOT IN (1,6,7,8,9) AND 
	completion_date BETWEEN NOW() AND DATE_ADD(NOW(), INTERVAL 1 MONTH)
", business_unit_id);
						d60to90.InnerHtml = gen_rep(sql, "60to90");
						break;
					#endregion 60to90
					#region 30to60
					case "30to60":
			
						sql = string.Format(@"
SELECT
	b.customer_id cust_id,
	b.customer_number_int cust_no,
	b.customer_name cust_name,
	a.quote_id,
	a.revision,
	a.quoted_by quoted_by_id,
	MEMBER_NAME(a.quoted_by) quoted_by_name,
	IFNULL(a.quoted_price,0) quoted_price
FROM 
	quote_master a 
LEFT JOIN
	customer b ON a.customer_id = b.customer_id
WHERE 
	a.business_unit_id = {0} AND 
	a.pct_chance BETWEEN 30 AND 60 AND 
	completion_date BETWEEN NOW() AND DATE_ADD(NOW(), INTERVAL 1 MONTH)
ORDER BY
	a.quoted_price DESC
", business_unit_id);
						d30to60.InnerHtml = gen_rep(sql, "30to60");
						break;
					#endregion 30to60
					#region WOThisMonth
					case "WOThisMonth":
						var this_month_start				= Toolbox.MySQL_shortdt(DateTime.Now);
						var this_month_end				= Toolbox.MySQL_shortdt(new DateTime(DATE.Year, DATE.Month, DateTime.DaysInMonth(DATE.Year, DATE.Month)));
					
						sql = string.Format(@"
SELECT
	b.customer_id cust_id,
	b.customer_number_int cust_no,
	b.customer_name cust_name,
	a.woprog_id wo_id,
	a.woprog_bvwo wo_no,
	a.woprog_pm_memberid pm_id,
	MEMBER_NAME(a.woprog_pm_memberid) pm_name,
	IF(a.woprog_quoteid=0,a.woprog_expected_sales_value,a.woprog_stilltobebilled) sales
FROM 
	woprog a
LEFT JOIN
	customer b 
		ON a.woprog_customer_id = b.customer_id
WHERE
	a.business_unit_id = {0} AND
	a.woprog_expected_enddate BETWEEN '{2}' AND '{3}' AND
	a.woprog_bvwo != 'Not Entered' AND 
	a.woprog_iscredit = 0 AND 
	a.woprog_isrebill = 0 AND 
	a.woprog_hold = 0  AND
	a.woprog_status != 'Invoiced'
	
", business_unit_id, 0, this_month_start, this_month_end);
						dWOThisMonth.InnerHtml = gen_rep(sql, "WOThisMonth");
					break;
					#endregion WOThisMonth
					#region WONextMonth
					case "WONextMonth":
						var next_month					= DATE.AddMonths(1);
						var next_month_start				= Toolbox.MySQL_shortdt(new DateTime(next_month.Year, next_month.Month, 1));
						var next_month_end				= Toolbox.MySQL_shortdt(new DateTime(next_month.Year, next_month.Month, DateTime.DaysInMonth(next_month.Year, next_month.Month)));
					
						sql = string.Format(@"
SELECT
	b.customer_id cust_id,
	b.customer_number_int cust_no,
	b.customer_name cust_name,
	a.woprog_id wo_id,
	a.woprog_bvwo wo_no,
	a.woprog_pm_memberid pm_id,
	MEMBER_NAME(a.woprog_pm_memberid) pm_name,
	IF(a.woprog_quoteid=0,a.woprog_expected_sales_value,a.woprog_stilltobebilled) sales
FROM 
	woprog a
LEFT JOIN
	customer b 
		ON a.woprog_customer_id = b.customer_id
WHERE
	a.business_unit_id = {0} AND
	a.woprog_expected_enddate BETWEEN '{2}' AND '{3}' AND
	a.woprog_bvwo != 'Not Entered' AND 
	a.woprog_iscredit = 0 AND 
	a.woprog_isrebill = 0 AND 
	a.woprog_hold = 0  AND
	a.woprog_status != 'Invoiced'
	
", business_unit_id, 0, next_month_start, next_month_end);
						dWONextMonth.InnerHtml = gen_rep(sql, "WONextMonth");
					break;
					#endregion WONextMonth
					#region WOFollowingMonth
					case "WOFollowingMonth":
						var following_month				= DATE.AddMonths(2);
						var following_month_start			= Toolbox.MySQL_shortdt(new DateTime(following_month.Year, following_month.Month, 1));
						var following_month_end				= Toolbox.MySQL_shortdt(new DateTime(following_month.Year, following_month.Month, DateTime.DaysInMonth(following_month.Year, following_month.Month)));
						
						sql = string.Format(@"
SELECT
	b.customer_id cust_id,
	b.customer_number_int cust_no,
	b.customer_name cust_name,
	a.woprog_id wo_id,
	a.woprog_bvwo wo_no,
	a.woprog_pm_memberid pm_id,
	MEMBER_NAME(a.woprog_pm_memberid) pm_name,
	IF(a.woprog_quoteid=0,a.woprog_expected_sales_value,a.woprog_stilltobebilled) sales
FROM 
	woprog a
LEFT JOIN
	customer b 
		ON a.woprog_customer_id = b.customer_id
WHERE
	a.business_unit_id = {0} AND
	a.woprog_expected_enddate BETWEEN '{2}' AND '{3}' AND
	a.woprog_bvwo != 'Not Entered' AND 
	a.woprog_iscredit = 0 AND 
	a.woprog_isrebill = 0 AND 
	a.woprog_hold = 0  AND
	a.woprog_status != 'Invoiced'
	
", business_unit_id, 0, following_month_start, following_month_end);
						dWOFollowingMonth.InnerHtml = gen_rep(sql, "WOFollowingMonth");
					break;
					#endregion WONextMonth
					#region inventory
					case "inventory":
						sql = string.Format(@"
SELECT 
	a.woprog_customer_id id,
	b.customer_number number,
	b.customer_name name,
	MAX(a.woprog_invoicedate) inv_date
FROM 
	woprog a
LEFT JOIN
	customer b ON a.woprog_customer_id = b.customer_id
WHERE 
	a.business_unit_id = {0} AND 
	
	a.woprog_invoicedate > '{2}' AND
	b.customer_name NOT LIKE '%NEW ELECTRIC%'
GROUP BY
	a.woprog_customer_id
ORDER BY 
	inv_date DESC", c.id, 0, Toolbox.MySQL_shortdt(c.fiscal_start_current));

						dinventory.InnerHtml = gen_rep(sql, "inventory");
						break;
					#endregion
				}
			
			}
		pan.ClientVisible				= true;
		}
	protected string gen_rep(string sql, string type)
		{
		var business_unit_id						= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
		var c							= new NeBusinessUnit(business_unit_id);
		var sb					= new StringBuilder();
		var dt						= _tools.getSQL_datatable(sql,null);
		dt									= _tools.recode_datatable(dt, false);
		if(dt.Rows.Count == 0)
			{
			sb.Append("<div align='center' style='padding:5px;'><b>No data available.</b></div>");
			}
		else
			{
			switch(type)
				{
				#region customer
				case "customer":
				sb.Append(@"
<table width='100%' cellspacing='0' cellpadding='5' id='drill'>
	<thead>
		<tr>
			<th width='10%'>Customer #</th>
			<th width='75%'>Name</th>
			<th width='15%'>Latest Invoice Date</th>
		</tr>
	</thead>
	<tbody>");
				break; 
				#endregion
				#region netincome
				case "netincome":
				sb.Append(@"
<table width='100%' cellspacing='0' cellpadding='5' id='drill'>
	<thead>
		<tr>
		<th width='10%'>Customer #</th>
		<th width='90%'>Name</th>
		</tr>
	</thead>
	<tbody>");
				break;

				#endregion
				#region worst
				case "worst":
				sb.Append(@"
<table width='100%' cellspacing='0' cellpadding='5' id='drill'>
	<thead>
		<tr>
		<th width='10%'>Work Order</th>
		<th width='85%'>Description</th>
		<th width='5%'>DSO</th>
		</tr>
	</thead>
	<tbody>");
				break; 
				#endregion
				#region quotepcts
				case "90plus":
				case "60to90":
				case "30to60":
				sb.Append(@"
<table width='100%' cellspacing='0' cellpadding='5' id='drill'>
	<thead>
		<tr>
			<th width='10%'>Customer #</th>
			<th width='60%'>Customer Name</th>
			<th width='10%'>Quote #</th>
			<th width='10%'>Quoted By</th>
			<th width='10%'>Quoted Price</th>
		</tr>
	</thead>
	<tbody>");
				break; 
				#endregion
				#region WO
				case "WOThisMonth":
				case "WONextMonth":
				case "WOFollowingMonth":
				sb.Append(@"
<table width='100%' cellspacing='0' cellpadding='5' id='drill'>
	<thead>
		<tr>
			<th width='10%'>Customer #</th>
			<th width='60%'>Customer Name</th>
			<th width='10%'>WO #</th>
			<th width='10%'>PM</th>
			<th width='10%'>Sales</th>
		</tr>
	</thead>
	<tbody>");
				break; 
				#endregion
				}
			double this_total					= 0;
			foreach(DataRow dr in dt.Rows)
				{
				switch(type)
					{
					case "customer":
						var cust_number		= dr["number"].ToString();
						var cust_id			= dr["id"].ToString();
						var cust_name		= dr["name"].ToString();
						var inv_date			= Convert.ToDateTime(dr["inv_date"]).ToString("d");
						sb.Append(string.Format(@"
		<tr>
			<td align='center'><b>{0}</b></td>
			<td><a href='{2}'>{1}</a></td>
			<td align='center' style='border-right:solid 2px #999;'>{3}</td>
		</tr>
", 
		cust_number, 
		cust_name, 
		cust_id,
		inv_date
		));
					break;
					case "worst":
						var wo				= dr["wo"].ToString();
						var woprog_id		= dr["woprog_id"].ToString();
						var description		= dr["descript"].ToString();
						var dso				= dr["dso"].ToString();
						sb.Append(string.Format(@"
		<tr>
			<td align='center'><b><a href=""javascript:boing('/sections/workorder/index.aspx?woprog_id={1}', 'wo', 1035,800)"">{0}</a></b></td>
			<td>{2}</td>
			<td align='center' style='border-right:solid 2px #999;'>{3}</td>
		</tr>
", 
		wo, 
		woprog_id, 
		description,
		dso
		));
					break;
					case "90plus":
					case "60to90":
					case "30to60":
						var q_cust_id		= dr["cust_id"].ToString();
						var q_cust_no		= dr["cust_no"].ToString();
						var q_cust_name		= dr["cust_name"].ToString();
						var q_quote_id		= dr["quote_id"].ToString();
						var q_quoted_by_id	= dr["quoted_by_id"].ToString();
						var q_quoted_by_name	= dr["quoted_by_name"].ToString();
						var q_quote_rev		= dr["revision"].ToString();
						var q_quoted_amount	= Convert.ToDouble(dr["quoted_price"]);
						this_total				+= q_quoted_amount;
						sb.Append(string.Format(@"
		<tr>
			<td align='center'><b><a href=""javascript:boing('/sections/customer/index.aspx?customer_id={5}&iframe=true', 'wo', 1035,800)"">{0}</a></b></td>
			<td><b><a href=""javascript:boing('/sections/customer/index.aspx?customer_id={5}&iframe=true', 'wo', 1035,800)"">{1}</a></b></td>
			<td align='center'><b><a href=""javascript:boing('/#/opens/65/quotes/{2}/{3}', 'wo', 1035,800)"">Q{2} V{3}</a></b></td>
			<td align='center'>{6}</td>
			<td align='center'>{4:C2}</td>
		</tr>
", 
		q_cust_no,			// {0}
		q_cust_name,		// {1}
		q_quote_id,			// {2}
		q_quote_rev,		// {3}
		q_quoted_amount,	// {4}
		q_cust_id,			// {5}
		q_quoted_by_name	// {6}
		));
					break;
					case "WOThisMonth":
					case "WONextMonth":
					case "WOFollowingMonth":
						var wo_cust_id		= dr["cust_id"].ToString();
						var wo_cust_no		= dr["cust_no"].ToString();
						var wo_cust_name		= dr["cust_name"].ToString();
						var wo_id			= dr["wo_id"].ToString();
						var wo_no			= dr["wo_no"].ToString();
						var wo_pm_id			= dr["pm_id"].ToString();
						var wo_pm_name		= dr["pm_name"].ToString();
						var wo_sales			= Convert.ToDouble(dr["sales"]);
						this_total				+= wo_sales;
						sb.Append(string.Format(@"
		<tr>
			<td align='center'><b><a href=""javascript:boing('/sections/customer/index.aspx?customer_id={5}&iframe=true', 'wo', 1035,800)"">{0}</a></b></td>
			<td><b><a href=""javascript:boing('/sections/customer/index.aspx?customer_id={5}&iframe=true', 'wo', 1035,800)"">{1}</a></b></td>
			<td align='center'><b><a href=""javascript:boing('/sections/workorder/index.aspx?woprog_id={2}', 'wo', 1035,800)"">{3}</a></b></td>
			<td align='center'>{6}</td>
			<td align='center'>{4:C2}</td>
		</tr>
", 
		wo_cust_no,			// {0}
		wo_cust_name,		// {1}
		wo_id,				// {2}
		wo_no,				// {3}
		wo_sales,	// {4}
		wo_cust_id,			// {5}
		wo_pm_name	// {6}
		));
					break;
					}
				}
		if(this_total > 0)
			{
			switch(type)
				{
				case "90plus":
				case "60to90":
				case "30to60":
				case "WOThisMonth":
				case "WONextMonth":
				case "WOFollowingMonth":
					sb.AppendFormat(@"
			<tr>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
				<td align='right'><b>Total:</b></td>
				<td align='center'>{0:C2}</td>
			</tr>", this_total);
				break;
				}
			}
		sb.Append(@"
	</tbody>
<table>");
			}
		return sb.ToString();
		}
//	protected string gl_rep(string sql)
//		{
//		var business_unit_id						= Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
//		var c							= new NeBusinessUnit(business_unit_id);
//		var sb					= new StringBuilder();
//		var dt						= _tools.getSQL_datatable(sql, c.DSN,null);
//		if(dt.Rows.Count == 0)
//			{
//			sb.AppendFormat("<div align='center' title='{0}' style='padding:5px;'><b>No data available.</b></div>", c.DSN);
//			}
//		else
//			{
//			sb.AppendFormat(@"
//<table width='100%' cellspacing='0' cellpadding='5' id='drill' title='{0}'>
//	<thead>
//		<tr>
//		<th width='10%'>GL #</th>
//		<th width='80%'>GL Account Name</th>
//		<th width='10%'>Amount</th>
//		</tr>
//	</thead>
//	<tbody>", c.DSN);
//			foreach(DataRow dr in dt.Rows)
//				{
//				var gl_acct_number		= dr["acct_no"].ToString();
//				var gl_acct_name			= dr["name"].ToString();
//				var gl_amount			= Convert.ToDouble(dr["amount"]);
//				sb.Append(string.Format(@"
//		<tr>
//			<td align='center'><b>{0}</b></td>
//			<td>{1}</td>
//			<td align='center' style='border-right:solid 2px #999;'>{2:C2}</td>
//		</tr>
//", 
//		gl_acct_number, 
//		gl_acct_name, 
//		gl_amount
//		));
//				}
//		sb.Append(@"
//	</tbody>
//<table>");
//			}
//		return sb.ToString();
//		}
}