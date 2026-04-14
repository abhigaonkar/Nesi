using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Globalization;
using nesi.core;

public partial class sections_reports_manpower_index : Page
	{
	Toolbox _tools;
	NeMember current_user;
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	static int _page_id = 142;
	static string _page_name = "Manpower";
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Manpower";
		var init_month				= 0;

		    cbl_branch.DataSource = Toolbox.doSQL_dt(string.Format(@"SELECT id, ddl_name name FROM business_unit  WHERE id in ({0}) and id != 11 ORDER BY name", new Current_User().visible_business_units ),null);
		    cbl_branch.DataBind();

        if (!IsCallback && !IsPostBack)
			{
			var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			}
		if (!IsCallback && !IsPostBack)
			{
			cbl_branch.DataBind();
			var selected_branch		= current_user.business_unit_id == 11 ? 1 : current_user.business_unit_id;
			cbl_branch.Value			= Convert.ToInt32(selected_branch);
			cbl_branch.Enabled			= current_user.AuthenticatedForPrivilege(134);
			//cbl_dept.Enabled			= current_user.AuthenticatedForPrivilege(134);
			//cbl_dept.DataBind();
			//cbl_dept.Items.FindByValue(selected_division).Selected		= true;
			init_month					= Toolbox.doSQL_int(@"SELECT COUNT(date_due) n 
FROM quote_master where active_revision = true and status_id NOT IN (6,8,9) and completion_date is 
not null and business_unit_id = @v0  order by quote_id desc LIMIT 1", new object[] { selected_branch }) > 0 ?
Toolbox.doSQL_int(@"SELECT DISTINCT(DATE_FORMAT(date_due, '%Y%m')) n
FROM quote_master where active_revision = true and status_id NOT IN (6,8,9) and completion_date is not null and business_unit_id = @v0 order by quote_id desc LIMIT 1", 
  new object[] { selected_branch } ) : Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));

			fill_months();
			if(ddl_month.Items.Count > 0 && ddl_month.Items.FindByValue(init_month) != null)
				{
				ddl_month.Value			= init_month;
				}
			else
				{
				ddl_month.SelectedIndex	= 0;	
				}
			fill_report();
			}
		}
	private void fill_months()
		{
		ddl_month.DataSource				= Toolbox.doSQL_dt(string.Format(@"CALL REPORT_MANPOWER_MONTHS('{0}', '')", get_selected_branches()), null);
        ddl_month.DataBind();
		}
	private string get_selected_branches()
		{
		var cs				= new List<string>();
		foreach(ListEditItem li in cbl_branch.SelectedItems)
			{
			cs.Add(li.Value.ToString());
			}
		return string.Join(",", cs);
		}
	private void clear_report()
		{
		lb_month1_header.Text					= "";
		lb_month2_header.Text					= "";
		lb_month3_header.Text					= "";
		lb_expquoterev_month1.Text				= "";
		lb_expquoterev_month2.Text				= "";
		lb_expquoterev_month3.Text				= "";
		lb_tmrev_month1.Text					= "";
		lb_tmrev_month2.Text					= "";
		lb_tmrev_month3.Text					= "";
		lb_billablehours_month1.Text			= "";
		lb_billablehours_month2.Text			= "";
		lb_billablehours_month3.Text			= "";
		lb_utilization_month1.Text				= "";
		lb_utilization_month2.Text				= "";
		lb_utilization_month3.Text				= "";
		lb_totalheadcount_month1.Text			= "";
		lb_totalheadcount_month2.Text			= "";
		lb_totalheadcount_month3.Text			= "";
		lb_actualheadcount_month1.Text			= "";
		lb_actualheadcount_month2.Text			= "";
		lb_actualheadcount_month3.Text			= "";
		lb_surpdeficit_month1.Text				= "";
		lb_surpdeficit_month2.Text				= "";
		lb_surpdeficit_month3.Text				= "";

		}
	private void fill_report()
		{
		if (ddl_month.Value == null) return;
		var dt							= Toolbox.doSQL_dt(@"CALL REPORT_MANPOWER(@v0 , @v1 , '')", new object[] {  ddl_month.Value, get_selected_branches() } );
		var start_month						= ddl_month.Value.ToString();
		var month								= Convert.ToInt32(start_month.Substring(4, 2));
		var year								= Convert.ToInt32(start_month.Substring(0,4));
		int[] months							= {0,0,0};
		months[0]								= month;
		months[1]								= month + 1 > 12 ? 1 : month + 1;
		months[2]								= month == 12 ? 2 : month == 11 ? 1 : month + 2;
		string[] months_str						= {"", "", ""};
		months_str[0]							= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(months[0]);
		months_str[1]							= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(months[1]);
		months_str[2]							= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(months[2]);
		DateTime[] months_dt					= {
													DateTime.Parse(string.Format("{0}-{1}-{2}", year, months[0], 1)), 
													DateTime.Parse(string.Format("{0}-{1}-{2}", year, months[1], 1)), 
													DateTime.Parse(string.Format("{0}-{1}-{2}", year, months[2], 1))
												  };
		lb_month1_header.Text					= months_str[0];
		lb_month2_header.Text					= months_str[1];
		lb_month3_header.Text					= months_str[2];

		var quote_rev					= new List<double>();
		quote_rev.Add(Convert.ToDouble(dt.Rows[0]["dv_month_1"]));
		quote_rev.Add(Convert.ToDouble(dt.Rows[0]["dv_month_2"]));
		quote_rev.Add(Convert.ToDouble(dt.Rows[0]["dv_month_3"]));
		lb_expquoterev_month1.Text				= quote_rev[0].ToString("C0");
		lb_expquoterev_month2.Text				= quote_rev[1].ToString("C0");
		lb_expquoterev_month3.Text				= quote_rev[2].ToString("C0");
		
		
		var dt1_invoice_start				= DateTime.Now.AddMonths(-3);
		var dt1_invoice_end				= DateTime.Now;
		var str1_invoice_start				= Toolbox.MySQL_shortdt(dt1_invoice_start);
		var str1_invoice_end					= Toolbox.MySQL_shortdt(dt1_invoice_end);
		var month1_dt						= DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, 1));
		var month2_dt						= DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, 1));
		var month3_dt						= DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, 1));
		var wo_avg							= Toolbox.doSQL_double(@"SELECT round((IFNULL(SUM(woprog_invoicednettotal),0) / 3),0) FROM woprog WHERE FIND_IN_SET(business_unit_id, @v2 ) AND woprog_quoteid IN ('', '0') and woprog_associate_woprog_id = 0 AND woprog_invoicedate BETWEEN @v0  AND @v1 ", new object[] {  str1_invoice_start, str1_invoice_end, get_selected_branches() } );
		lb_tmrev_month1.Text					= wo_avg.ToString("C0");
		lb_tmrev_month2.Text					= wo_avg.ToString("C0");
		lb_tmrev_month3.Text					= wo_avg.ToString("C0");

		var exp_bill1						= Convert.ToDouble(dt.Rows[0]["bh_month_1"]);
		var exp_bill2						= Convert.ToDouble(dt.Rows[0]["bh_month_2"]);
		var exp_bill3						= Convert.ToDouble(dt.Rows[0]["bh_month_3"]);

	//	double exp_bill1 = (quote_rev[0] + wo_avg) / 2 / 85;
	//	double exp_bill2= (quote_rev[1] + wo_avg) / 2 / 85;
	//	double exp_bill3 = (quote_rev[2] + wo_avg) / 2 / 85;

		var exp_bill_from_TM = Toolbox.doSQL_double(@"Select ifnull((SELECT Sum(wo_detail_current.wo_detail_current_qty_committed) FROM wo_detail_current INNER JOIN woprog ON wo_detail_current.wo_detail_current_woprog_id = woprog.WOProg_ID WHERE FIND_IN_SET(woprog.business_unit_id, @v2 ) AND wo_detail_current.wo_detail_current_master_id >= 900000 AND wo_detail_current.wo_detail_current_date_added > curdate() - interval 3 month AND woprog.woprog_quoteid IN ('', '0') AND woprog.WOProg_Associate_WOProg_ID = 0),0)", new object[] {  str1_invoice_start, str1_invoice_end, get_selected_branches() } );

		exp_bill_from_TM += Toolbox.doSQL_double(@"Select ifnull((SELECT Sum(wo_detail_history.wo_detail_history_qty_committed) FROM wo_detail_history INNER JOIN woprog ON wo_detail_history.wo_detail_history_woprog_id = woprog.WOProg_ID WHERE FIND_IN_SET(woprog.business_unit_id, @v2 ) AND wo_detail_history.wo_detail_history_master_id >= 900000 AND wo_detail_history.wo_detail_history_date_added > curdate() - interval 3 month AND woprog.woprog_quoteid IN ('', '0') AND woprog.WOProg_Associate_WOProg_ID = 0),0)", new object[] {  str1_invoice_start, str1_invoice_end, get_selected_branches() } );

		exp_bill1 += (exp_bill_from_TM/3);
		exp_bill2 += (exp_bill_from_TM/3);
		exp_bill3 += (exp_bill_from_TM/3);

		lb_billablehours_month1.Text			= exp_bill1.ToString("N0");
		lb_billablehours_month2.Text			= exp_bill2.ToString("N0");
		lb_billablehours_month3.Text			= exp_bill3.ToString("N0");

		lb_utilization_month1.Text				= "90%";
		lb_utilization_month2.Text				= "90%";
		lb_utilization_month3.Text				= "90%";

		var totalheadcount_month1			= exp_bill1 * (1/.9) / 8 / 22;
		var totalheadcount_month2			= exp_bill2 * (1/.9) / 8 / 22;
		var totalheadcount_month3			= exp_bill3 * (1/.9) / 8 / 22;

		lb_totalheadcount_month1.Text			= totalheadcount_month1.ToString("N2");
		lb_totalheadcount_month2.Text			= totalheadcount_month2.ToString("N2");
		lb_totalheadcount_month3.Text			= totalheadcount_month3.ToString("N2");

		var n_totalemployees					= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_status = 'Active' and member_membertype_id not in(5,52,39,66,29) AND FIND_IN_SET(business_unit_id, @v0 )", new object[] {  get_selected_branches() } ) ;
		lb_actualheadcount_month1.Text			= n_totalemployees.ToString("N");
		lb_actualheadcount_month2.Text			= n_totalemployees.ToString("N");
		lb_actualheadcount_month3.Text			= n_totalemployees.ToString("N");

		lb_surpdeficit_month1.Text = (n_totalemployees-totalheadcount_month1 ).ToString("N2");
		lb_surpdeficit_month2.Text = (n_totalemployees-totalheadcount_month2 ).ToString("N2");
		lb_surpdeficit_month3.Text = (n_totalemployees-totalheadcount_month3 ).ToString("N2");

		var reference_month1					= Toolbox.doSQL_double(@"SELECT IFNULL(SUM(IFNULL(HOUR(TIMEDIFF(enddate, startdate)), 0)),0) FROM appointments WHERE DATE(startdate) >= @v0  and DATE(enddate) < @v1  AND FIND_IN_SET(business_unit_id, @v2 )", new object[] {  Toolbox.MySQL_shortdt(months_dt[0]), Toolbox.MySQL_shortdt(months_dt[0].AddMonths(1).AddDays(-1)), get_selected_branches() } );
		var reference_month2					= Toolbox.doSQL_double(@"SELECT IFNULL(SUM(IFNULL(HOUR(TIMEDIFF(enddate, startdate)), 0)),0) FROM appointments WHERE DATE(startdate) >= @v0  and DATE(enddate) < @v1  AND FIND_IN_SET(business_unit_id, @v2 )", new object[] {  Toolbox.MySQL_shortdt(months_dt[1]), Toolbox.MySQL_shortdt(months_dt[1].AddMonths(1).AddDays(-1)), get_selected_branches() } );
		var reference_month3					= Toolbox.doSQL_double(@"SELECT IFNULL(SUM(IFNULL(HOUR(TIMEDIFF(enddate, startdate)), 0)),0) FROM appointments WHERE DATE(startdate) >= @v0  and DATE(enddate) < @v1  AND FIND_IN_SET(business_unit_id, @v2 )", new object[] {  Toolbox.MySQL_shortdt(months_dt[2]), Toolbox.MySQL_shortdt(months_dt[2].AddMonths(1).AddDays(-1)), get_selected_branches() } );
					
		lb_reference_month1.Text				= (reference_month1 * (1/.9) / 8 / 22).ToString("N2");
		lb_reference_month2.Text				= (reference_month2 * (1/.9) / 8 / 22).ToString("N2");
		lb_reference_month3.Text				= (reference_month3 * (1/.9) / 8 / 22).ToString("N2");

		
	/*	lb_expquoterev_formula.Text				= @"<img src='/images/icon/icon[help].png' width='16' class='opt1' data-tooltip='This figure is the sum of all quoted jobs quoted prices in this branch/department' data-title='Formula' height='16' />";
		lb_tmrev_formula.Text					= @"<img src='/images/icon/icon[help].png' width='16' class='opt1' data-tooltip='This figure represents the average of jobs invoiced in the past 3 months for T+M jobs in this branch/department' data-title='Formula' height='16' />";
		lb_billablehours_formula.Text			= @"<img src='/images/icon/icon[help].png' width='16' class='opt1' data-tooltip='This figure is the sum of all quote line items for labor in this branch/department' data-title='Formula' height='16' />";
		lb_utilization_formula.Text				= @"<img src='/images/icon/icon[help].png' width='16' class='opt1' data-tooltip='This figure is always 90%' data-title='Formula' height='16' data-title='Formula' height='16' />";
		lb_totalheadcount_formula.Text			= @"<img src='/images/icon/icon[help].png' width='16' class='opt1' data-tooltip='Expected billable hours (b) * utilization (u) / 8 hrs per day / 22 workdays in a month [ b * u / 8 / 22 ]' data-title='Formula' height='16' />";

		lb_actualheadcount_formula.Text			= @"<img src='/images/icon/icon[help].png' width='16' class='opt1' data-tooltip='Total number of employees in the branch minus the branch manager & purchaser' data-title='Formula' height='16' />";
		lb_surpdeficit_formula.Text				= @"<img src='/images/icon/icon[help].png' width='16' class='opt1' data-tooltip='This figure represents Total Headcount minus Actual Headcount' data-title='Formula' height='16' />";
		lb_reference_formula.Text				= @"<img src='/images/icon/icon[help].png' width='16' class='opt1' data-tooltip='This figure represents the totals scheduled hours for the month (s) * utilization (u) / 8 hrs per day / 22 workdays in a mont [ s * u / 8 / 22 ]' data-title='Formula' height='16' />";
	*/
	  //1333.3333 * (1/0.9)/8/22 = 8.415 = Total Head count								   
		//Referenced - Get total # of hours scheduled - Drill it down to headcount
		}
	/*
	protected void gv_manpower_DataBound(object sender, EventArgs e)
		{
		// Start Prep
		ASPxGridView gv							= (ASPxGridView) sender;
		string start_month						= ddl_month.SelectedValue;
		if(start_month != "")
			{
			int month								= Convert.ToInt32(start_month.Substring(4, 2));
			int year								= Convert.ToInt32(start_month.Substring(0,4));
			int[] months							= {0,0,0};
			months[0]								= month;
			months[1]								= month + 1 > 12 ? 1 : month + 1;
			months[2]								= month == 12 ? 2 : month == 11 ? 1 : month + 2;
			string[] months_str						= {"", "", ""};
			months_str[0]							= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(months[0]);
			months_str[1]							= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(months[1]);
			months_str[2]							= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(months[2]);
			// T & M
			DateTime dt1_invoice_start				= DateTime.Now.AddMonths(-3);
			DateTime dt1_invoice_end				= DateTime.Now;
			string str1_invoice_start				= Toolbox.MySQL_shortdt(dt1_invoice_start);
			string str1_invoice_end					= Toolbox.MySQL_shortdt(dt1_invoice_end);
			// End Prep
			lb_month_hdr1.Text						= months_str[0];
			lb_month_hdr2.Text						= months_str[1];
			lb_month_hdr3.Text						= months_str[2];
			lb_month_hdr4.Text						= lb_month_hdr1.Text;
			lb_month_hdr5.Text						= lb_month_hdr2.Text;
			lb_month_hdr6.Text						= lb_month_hdr3.Text;
			double total_current_mp					= 0;
			double tm_avg							= 0;
			double hours_average					= 0;
			if(ddl_branch.SelectedValue == "0")
				{
				total_current_mp					= Toolbox.doSQL_double(@"SELECT COUNT(member_id) FROM member  WHERE business_unit_id != '8' AND member_status = 'Active'" , null);
				tm_avg								= Toolbox.doSQL_double(@"SELECT SUM(woprog_invoicednettotal) / 3 FROM woprog WHERE business_unit_id NOT IN (8, 11) AND woprog_quoteid IN ('', '0') AND woprog_invoicedate BETWEEN @v0  AND @v1 ", new object[] {  str1_invoice_start, str1_invoice_end } );
				hours_average						= Toolbox.doSQL_double(@"SELECT SUM(numberofhours) / 3 FROM membertime WHERE membertime_woprog_id IN (SELECT woprog_id FROM woprog WHERE business_unit_id NOT IN (8, 11) AND woprog_quoteid IN ('', '0') AND woprog_invoicedate BETWEEN @v0  AND @v1 )", new object[] {  str1_invoice_start, str1_invoice_end } );
				}
			else
				{
				total_current_mp					= Toolbox.doSQL_double(@"SELECT COUNT(member_id) FROM member WHERE business_unit_id = @v0  AND member_status = 'Active'", new object[] {  ddl_branch.SelectedValue } );
				tm_avg								= Toolbox.doSQL_double(@"SELECT SUM(woprog_invoicednettotal) / 3 FROM woprog WHERE business_unit_id = @v2  AND woprog_quoteid IN ('', '0') AND woprog_invoicedate BETWEEN @v0  AND @v1 ", new object[] {  str1_invoice_start, str1_invoice_end, ddl_branch.SelectedValue } );
				hours_average						= Toolbox.doSQL_double(@"SELECT SUM(numberofhours) / 3 FROM membertime WHERE membertime_woprog_id IN (SELECT woprog_id FROM woprog WHERE business_unit_id = @v2  AND woprog_quoteid IN ('', '0') AND woprog_invoicedate BETWEEN @v0  AND @v1 )", new object[] {  str1_invoice_start, str1_invoice_end, ddl_branch.SelectedValue } );
				}
			double mth1_qv							= Convert.ToDouble(gv_manpower.GetTotalSummaryValue(gv_manpower.TotalSummary[0]));
			double mth2_qv							= Convert.ToDouble(gv_manpower.GetTotalSummaryValue(gv_manpower.TotalSummary[1]));
			double mth3_qv							= Convert.ToDouble(gv_manpower.GetTotalSummaryValue(gv_manpower.TotalSummary[2]));
			double mth1_mp							= Convert.ToDouble(gv_manpower.GetTotalSummaryValue(gv_manpower.TotalSummary[3]));
			double mth2_mp							= Convert.ToDouble(gv_manpower.GetTotalSummaryValue(gv_manpower.TotalSummary[4]));
			double mth3_mp							= Convert.ToDouble(gv_manpower.GetTotalSummaryValue(gv_manpower.TotalSummary[5]));
			lb_month_exp1.Text						= (mth1_qv + tm_avg).ToString("C2");
			lb_month_exp1.ToolTip					= string.Format("{2} Est. Quote Value: {0:C2} + Avg of T&M jobs in the past 3 months: {1:C2}", mth1_qv, tm_avg, months_str[0]);
			lb_month_exp2.Text						= (mth2_qv + tm_avg).ToString("C2");
			lb_month_exp2.ToolTip					= string.Format("{2} Est. Quote Value: {0:C2} + Avg of T&M jobs in the past 3 months: {1:C2}", mth2_qv, tm_avg, months_str[1]);
			lb_month_exp3.Text						= (mth3_qv + tm_avg).ToString("C2");
			lb_month_exp3.ToolTip					= string.Format("{2} Est. Quote Value: {0:C2} + Avg of T&M jobs in the past 3 months: {1:C2}", mth3_qv, tm_avg, months_str[2]);
			lb_month_expmp1.Text					= (mth1_mp + hours_average).ToString("N2");
			lb_month_expmp1.ToolTip					= string.Format("{2} Est. Manpower Needed: {0:N2} + Avg hours of T&M jobs in the past 3 months: {1:N2}", mth1_mp, hours_average, months_str[0]);
			lb_month_expmp2.Text					= (mth2_mp + hours_average).ToString("N2");
			lb_month_expmp2.ToolTip					= string.Format("{2} Est. Manpower Needed: {0:N2} + Avg hours of T&M jobs in the past 3 months: {1:N2}", mth2_mp, hours_average, months_str[1]);
			lb_month_expmp3.Text					= (mth3_mp + hours_average).ToString("N2");
			lb_month_expmp3.ToolTip					= string.Format("{2} Est. Manpower Needed: {0:N2} + Avg hours of T&M jobs in the past 3 months: {1:N2}", mth3_mp, hours_average, months_str[2]);
			lb_month_tm1.Text						= tm_avg.ToString("C2");
			lb_month_tmmp1.Text						= hours_average.ToString("N2");
			lb_month_mp1.Text						= (total_current_mp*160).ToString("N2");
			lb_month_mp1.ToolTip					= "Total of employees: "+total_current_mp+" * 160";
			gv.Columns["quote_value_1"].Caption		= "Exp. Quote Value <br/> <b>"+months_str[0]+"</b>";
			gv.Columns["quote_value_2"].Caption		= "Exp. Quote Value <br/> <b>"+months_str[1]+"</b>";
			gv.Columns["quote_value_3"].Caption		= "Exp. Quote Value <br/> <b>"+months_str[2]+"</b>";
			gv.Columns["mp_month_1"].Caption		= "Exp. Manpower Needed <br/> <b>"+months_str[0]+"</b>";
			gv.Columns["mp_month_2"].Caption		= "Exp. Manpower Needed <br/> <b>"+months_str[1]+"</b>";
			gv.Columns["mp_month_3"].Caption		= "Exp. Manpower Needed <br/> <b>"+months_str[2]+"</b>";
			}
		}
	 */
	protected void ddl_month_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	protected void ddl_branch_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	protected void ddl_branch_DataBound(object sender, EventArgs e)
		{
		var ddl		= (DropDownList) sender;
		ddl.Items.Insert(0, new ListItem("All Branches", "0"));
		}
	protected void cbp_Callback(object sender, CallbackEventArgsBase e)
		{
		fill_months();
		if(cbl_branch.SelectedItems.Count > 0)
			{
			fill_report();
			}
		else
			{
			clear_report();
			}
		}
}
