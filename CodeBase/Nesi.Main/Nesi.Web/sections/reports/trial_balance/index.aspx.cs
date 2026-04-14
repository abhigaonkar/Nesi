using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
//using nesi.bv;
using nesi.core;
using MySql.Data.MySqlClient;

public partial class trial_balance : Page
	{
	public NeMember myMember;
	double totalsum;
	private const int _page_id = 184; // from Page table in DB
	private const string _page_description = "Reports / Trial Balance";
	private int groupfootercounter;
	
	int cid;
	string companies;
	int did;
	string DSN;
	string Company_Name;
	NeBusinessUnit c;
	NeBusinessUnit base_c = new NeBusinessUnit();
	string month_field;
	string total_fields;
	string current_year_fields;
	string total_tables_1;
	string total_tables_2;
	string total_tables_3;
	string total_tables_4;
	string total_tables_LY;
	string prefix_month = "this_yr";
	Toolbox _tools = new Toolbox();
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	
	bool is_debug			= false;
	    private string acct_prefix = "";
    protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		layout.__page_name = "trial_balance";
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		layout.used_gv = gv_trial_balance;
		h.Set("gridview_id", "gv_trial_balance");
		ds_templates.SelectParameters["@page_name"].DefaultValue = "trial_balance";
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
	
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		using (var conn = Toolbox.connect())
			{
			if (!IsPostBack)
				{
				var gl = new NeGridLayouts(myMember.id32, layout.__page_name);
				if (gl.GridLayoutID == 0)
					{
					gl.GridLayout_Layout = "";
					gl.member_id = myMember.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = layout.__page_name;
					gl.SaveGridLayout();
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
					}
				else
					{
					gv_trial_balance.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
					}
				dde_filter.Text = gl.GridLayout_Name;
				}
			var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
			var lbltemp = (Label)Page.Master.FindControl("lblHeading");
			lbltemp.Text = _page_description;
			var DATE = DateTime.Now;

            

            DataTable _dt; ;


			    if (!IsPostBack)
			    {
			        cid = Convert.ToInt32(myMember.business_unit_id);
			    }

			    if (select_type.SelectedValue != "Actuals")
			    {
			        acct_prefix = "_BGT_";
			    }

 combo_tax_entity.DataBind();

			    if (!IsPostBack)
			    {  combo_tax_entity.Value = myMember.business_unit.tax_entity_id;
			        fill_enddate_selector();
			        fill_tax_entities();
                  
                   
			        
			    }
			   base_c = new NeBusinessUnit(Toolbox.doSQL_string("Select id from business_unit where tax_entity_id = @v0 limit 1", new object[] { combo_tax_entity.Value }));


           fill_business_units(conn);

			    if (myMember.business_unit_id == 11)
			        {
			        ASPxButton3.ClientVisible = true;
			        }

            if (cl_companies.SelectedValues.Count > 0)
			    {
			        companies = "(";
			        foreach (string c_selected in cl_companies.SelectedValues)
			        {
			            companies += " business_unit_id = " + c_selected + " or ";
			        }
			        companies = companies.Remove(companies.Length - 4, 4) + ")";
			        month_field = "this_yr" + string.Format("{0:MM}", DATE);
			        total_fields = "";
			        total_tables_1 = "";
			        total_tables_2 = "";
			        total_tables_3 = "";
			        total_tables_4 = "";
			        total_tables_LY = "";
			        var year_int = DATE.Year;
			        var month_int = DATE.Month;
 var waiting_rollover = false;//NeBusinessUnit.IsWaitingRollover(Convert.ToInt32(combo_tax_entity.Value));
			        //			if(!string.IsNullOrEmpty(fisc_end) && select_enddate.Items.FindByValue(fisc_end) != null)
			        //				{
			        year_int = waiting_rollover? Convert.ToInt32(select_enddate.Text.Split('_')[0])+1 : Convert.ToInt32(select_enddate.Text.Split('_')[0]);
			        month_int = Convert.ToInt32(select_enddate.Text.Split('_')[1]); // This is the month that is selected
                                                                                    //	select_enddate.Items.FindByValue(fisc_end).Selected		= true;
                                                                                    //				}
               

                var fiscal_month =
			            NeBusinessUnit.FiscalMonthLookup[
			                base_c.fiscal_yearstart_month][
			                month_int]; // this si the fiscal month base ont eh fiscal start of the year.
              
                    prefix_month = year_int == base_c.fiscal_current_year && month_int >= base_c.fiscal_yearstart_month
			            ? "this_yr"
			            : "last_yr"; // whether you are selecting into last year or not.
			        month_field = string.Format("{0}{2}{1:00}", prefix_month, fiscal_month, acct_prefix);
			        if (prefix_month == "this_yr")
			        {
			            for (var m = 1; m <= 12; m++)
			            {
			                var pre = m < 10 ? "0" : ""; // Zero padding
			                total_fields += "last_yr" + acct_prefix + pre + m + "+";
			            }
			            gv_trial_balance.Columns["last_yr_total"].Visible = true;
			            gv_trial_balance.Columns["last_yr_total"].Caption = "Dec " + (year_int - 1);
			        }
			        else
			        {
			            gv_trial_balance.Columns["last_yr_total"].Visible = false;
			        }
			        for (var m = 1; m <= fiscal_month; m++)
			        {
			            var pre = m < 10 ? "0" : ""; // Zero padding
			            var suff = m != fiscal_month ? "+" : "";
			            var month = pre + m;
			            total_fields += prefix_month + acct_prefix + month + suff;
			        }
			        for (var m = 1; m <= fiscal_month; m++)
			        {
			            var pre = m < 10 ? "0" : ""; // Zero padding
			            var suff = m != fiscal_month ? "+" : "";
			            var month = pre + m;
			            current_year_fields += prefix_month + acct_prefix + month + suff;
			        }
			        total_fields = "open_bal + " + total_fields;

			        #region set captions

			        foreach (GridViewDataColumn gvc in gv_trial_balance.Columns)
			        {
			            gvc.Caption = get_column_caption(gvc);
			        }

			        #endregion

			        fill_gv(conn);
			    }
			}
		}




	    private void fill_tax_entities()
	    {
	        combo_tax_entity.DataSource = _tools.getSQL_datatable(@"Select id,ddl_name from tax_entity  where FIND_IN_SET(id,@v0) order by ddl_name ", new object[] { new Current_User().visible_tax_entities });
	        combo_tax_entity.DataBind();

	    }

	    private void fill_business_units(MySqlConnection _conn)
	    {
	    try
	        {
            //TODO: LL SQL Security FIND_IN_SET
            var has_data_bus = NeAccounting.get_bus_with_gl_data(Convert.ToInt32(combo_tax_entity.Value));

            var dt_business_units = new DataTable();

            if (myMember.is_US_boardmember || myMember.is_CAN_boardmember)
            {
                dt_business_units = Toolbox.doSQL_dt(_conn, @"Select distinct 0 id, 'Consolidated' name union Select id,ddl_name name from business_unit  where tax_entity_id = @v0 and  (active = 'T' OR find_in_set(id,@v2))", new object[] { combo_tax_entity.Value.ToString(), new Current_User().visible_reporting_users, has_data_bus });
            }
            else if (myMember.business_unit.is_backoffice || myMember.business_unit.is_corporate)
            {
                dt_business_units = Toolbox.doSQL_dt(_conn, @"Select distinct 0 id, 'Consolidated' name union Select id,ddl_name name from business_unit  where tax_entity_id = @v0  and ( active = 'T' OR find_in_set(id,@v1)) ", new object[] { combo_tax_entity.Value.ToString(), has_data_bus });
            }
            else
            {
                dt_business_units = Toolbox.doSQL_dt(_conn, @"Select distinct id,ddl_name name from business_unit inner join member m on m.business_unit_id = business_unit.id and find_in_set(m.member_id,@v1) and m.member_status='Active' where tax_entity_id = @v0 and (active = 'T' OR find_in_set(id,@v2))", new object[] { combo_tax_entity.Value.ToString(), new Current_User().visible_reporting_users, has_data_bus });
            }
           

	        var waiting_rollover_i = 0;
	        foreach (DataRow dr in dt_business_units.Rows)
	            {
	            var bu_id = Convert.ToInt32(dr["id"]);
	            var waiting_rollover = false;//NeBusinessUnit.IsWaitingRollover(Convert.ToInt32(combo_tax_entity.Value));
	            var bu_name = dr["name"].ToString();
	            if (waiting_rollover)
	                {
	                bu_name = bu_name + "*";
	                waiting_rollover_i++;
	                }
	            dr["name"] = bu_name;
	            dr.AcceptChanges();
	            }
	        div_waiting_rollover.Visible = waiting_rollover_i > 0;
	        var dv_companies = dt_business_units.DefaultView;
	        dv_companies.Sort = "name";
	        cl_companies.DataSource = dv_companies;
	        cl_companies.DataBind();
	        }
	    catch (Exception ee)
	        {
	        throw new Exception("Error Occurred while connecting to BV database.");
	        }
	    }


    protected void fill_enddate_selector()
		{

        var DATE = DateTime.Now;
        #region Fiscal End Date
        select_enddate.Items.Clear();
        // This is for getting the current year's options
        base_c = new NeBusinessUnit(Toolbox.doSQL_string("Select id from business_unit where tax_entity_id = @v0 limit 1", new object[] { combo_tax_entity.Value }));
        //for (int i = NeBusinessUnit.FiscalMonthLookup[base_c.fiscal_yearstart_month][DATE.Month]; i >= 1; i--)
        for (var i = 12; i >= 1; i--)
        {
            var working_month = Convert.ToInt32(combo_tax_entity.Value) != 99 ? NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i] : NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i];
            var working_year = base_c.fiscal_yearstart_month == 1 || working_month < base_c.fiscal_yearstart_month
                                        ? DATE.Year
                                            : DATE.Year - 1;
            var month_name = new DateTime(working_year, working_month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
            var month_end = DateTime.DaysInMonth(working_year, working_month);
            var li = new ListItem(string.Format("{0} {1}, {2}", month_name, month_end, working_year), string.Format("{0}_{1}", working_year, working_month));
            select_enddate.Items.Add(li);
        }
        select_enddate.Items.Add("-----------------------------");
        // This is for getting the previous year's options
        for (var i = 12; i >= 1; i--)
        {
            var working_month = NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i];
            var working_year = base_c.fiscal_yearstart_month == 1 || working_month < base_c.fiscal_yearstart_month
                                        ? DATE.Year - 1
                                            : DATE.Year - 2;
            var month_name = new DateTime(working_year, working_month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
            var month_end = DateTime.DaysInMonth(working_year, working_month);
            var li = new ListItem(string.Format("{0} {1}, {2}", month_name, month_end, working_year), string.Format("{0}_{1}", working_year, working_month));
            select_enddate.Items.Add(li);
        }
        select_enddate.Items.Add("-----------------------------");

        if (false)//NeTaxEntity.IsWaitingRollover(Convert.ToInt32(combo_tax_entity.Value)))
        {
            for (var i = 12; i >= 1; i--)
            {
                var working_month = NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i];
                var working_year = base_c.fiscal_yearstart_month == 1 || working_month < base_c.fiscal_yearstart_month
                    ? DATE.Year - 2
                    : DATE.Year - 3;
                var month_name =
                    new DateTime(working_year, working_month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
                var month_end = DateTime.DaysInMonth(working_year, working_month);
                var li = new ListItem(string.Format("{0} {1}, {2}", month_name, month_end, working_year),
                    string.Format("{0}_{1:00}", working_year, working_month));
                select_enddate.Items.Add(li);
            }
        }
        //	select_enddate.Attributes.Add("onchange", "do_fiscal(this);");
        #endregion Fiscal End Date

      /*  var DATE = DateTime.Now;
        #region fill Fiscal End Date
        var waiting_rollover = NeBusinessUnit.IsWaitingRollover(Convert.ToInt32(combo_tax_entity.Value));
        var working_year1 = 2018;
        select_enddate.Items.Clear();
        // This is for getting the current year's options
        for (var i = NeBusinessUnit.FiscalMonthLookup[base_c.fiscal_yearstart_month][12]; i >= 1; i--)
			{
			var working_month = NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i];
			working_year1 = waiting_rollover? base_c.fiscal_current_year-1: base_c.fiscal_current_year;
			var month_name = new DateTime(working_year1, working_month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
			var month_end = DateTime.DaysInMonth(working_year1, working_month);
			var li = new ListItem(string.Format("{0} {1}, {2}", month_name, month_end, working_year1), string.Format("{0}_{1}", working_year1, working_month));
			select_enddate.Items.Add(li);
			}
		select_enddate.Items.Add("-----------------------------");
		// This is for getting the previous year's options
		for (var i = 12; i >= 1; i--)
			{
			var working_month = NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i];
			var working_year = base_c.fiscal_yearstart_month == 1 || working_month < base_c.fiscal_yearstart_month
										? working_year1 - 1
											: working_year1 - 2;
			var month_name = new DateTime(working_year, working_month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
			var month_end = DateTime.DaysInMonth(working_year, working_month);
			var li = new ListItem(string.Format("{0} {1}, {2}", month_name, month_end, working_year), string.Format("{0}_{1}", working_year, working_month));
			select_enddate.Items.Add(li);
			}
		#endregion Fiscal End Date */
		}
	protected string get_column_caption(GridViewDataColumn gvc)
		{
		if (gvc.FieldName.Contains("this_")|| (gvc.FieldName.Contains("last_") && !gvc.FieldName.Contains("last_yr_")))
			{
            int offset = base_c.fiscal_yearstart_month -1;
            string caption_base = gvc.FieldName.Split('_').GetValue(1).ToString().Replace("yr", "");

            var month_number = offset==0? Convert.ToInt32(caption_base) : Convert.ToInt32(caption_base)>(12-(offset))? offset-(12- Convert.ToInt32(caption_base)): offset  + Convert.ToInt32(caption_base);
            if (gvc.FieldName.Contains("this_"))
            {
                gvc.Caption = new System.DateTime(Convert.ToInt32(select_enddate.Text.Split('_').GetValue(0)), Convert.ToInt32(month_number), 1).ToString("MMM yyyy");
            }
            else
            {
                gvc.Caption = new System.DateTime(Convert.ToInt32(select_enddate.Text.Split('_').GetValue(0)), Convert.ToInt32(month_number), 1).AddYears(-1).ToString("MMM yyyy");
            }
        }
		return gvc.Caption;
		}
	
    private void fill_gv(MySqlConnection _conn)
    {
        groupfootercounter = 0;
        var month_ind = month_field.Substring(month_field.Length - 2, 2);
        string d;


        string current_earnings = Toolbox.doSQL_string(@"SELECT
gl_te.account_no
FROM
gl_te
INNER JOIN gl_special_account_te_link ON gl_special_account_te_link.gl_te_id = gl_te.id
WHERE
gl_special_account_te_link.special_accounts_id = 40 AND
gl_te.tax_entity_id = @v0", new object[] { combo_tax_entity.Value });

        string retained_earnings = Toolbox.doSQL_string(@"SELECT
gl_te.account_no
FROM
gl_te
INNER JOIN gl_special_account_te_link ON gl_special_account_te_link.gl_te_id = gl_te.id
WHERE
gl_special_account_te_link.special_accounts_id = 21 AND
gl_te.tax_entity_id = @v0", new object[] { combo_tax_entity.Value });


        string      used_months = @"IFNULL(open_bal,0) + IFNULL(last_yr01,0)+
IFNULL(last_yr02,0)+
IFNULL(last_yr03,0)+
IFNULL(last_yr04,0)+
IFNULL(last_yr05,0)+
IFNULL(last_yr06,0)+
IFNULL(last_yr07,0)+
IFNULL(last_yr08,0)+
IFNULL(last_yr09,0)+
IFNULL(last_yr10,0)+
IFNULL(last_yr11,0)+
IFNULL(last_yr12,0)";
        var total_credit_ytd = Toolbox.doSQL_double(_conn, string.Format(@"Select ifnull((SELECT SUM({0}) YTD FROM GL_CHART_OF_ACCOUNTS a inner join gl_te on gl_te.id = a.gl_te_id inner join gl_group_te on gl_group_te.id = gl_te.gl_group_id WHERE  gl_group_te.number >= '400' AND gl_te.gl_designation= 'C' and gl_te.tax_entity_id = {2} and {1}),0)", used_months, companies, combo_tax_entity.Value), null);
        var total_debit_ytd = Toolbox.doSQL_double(_conn, string.Format(@"Select ifnull((SELECT SUM({0}) YTD FROM GL_CHART_OF_ACCOUNTS a inner join gl_te on gl_te.id = a.gl_te_id inner join gl_group_te on gl_group_te.id = gl_te.gl_group_id WHERE  gl_group_te.number >= '400' AND gl_te.gl_designation= 'D' and gl_te.tax_entity_id = {2} and {1}),0)", used_months, companies, combo_tax_entity.Value), null);
        var diff = Convert.ToDecimal(total_credit_ytd - total_debit_ytd);
        


        DataTable dt;

        d = string.Format(@"
SELECT 
	
	UPPER(gl_chart_name) ACCOUNT,
	g.desc gl_group_alias,
	gl_te.GL_Designation drcr, 
	g.type type,
	if((g.type='A'or g.type='L'),  IFNULL(SUM({1}),0)   ,    IFNULL(SUM({6}),0)    ) YTD,
	IFNULL(SUM({0}), 0) MTD,
if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01) , 0),sum({8}{9}01)) last_yr01,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02) , 0),sum({8}{9}02)) last_yr02,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03) , 0),sum({8}{9}03)) last_yr03,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04),0),sum({8}{9}04)) last_yr04,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05) , 0),sum({8}{9}05)) last_yr05,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06),0),sum({8}{9}06)) last_yr06,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07) , 0),sum({8}{9}07)) last_yr07,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08) , 0),sum({8}{9}08)) last_yr08,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09) , 0)	,sum({8}{9}09)) last_yr09,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10) , 0)	,sum({8}{9}10)) last_yr10,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11) , 0)	,sum({8}{9}11)) last_yr11,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12) , 0)	,sum({8}{9}12)) last_yr12,


	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12 +this_yr{9}01) , sum(open_bal + last_yr{9}01)),sum({8}{9}01)) this_yr01,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02) , sum(open_bal + last_yr{9}01 + last_yr{9}02)),sum({8}{9}02)) this_yr02,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02+ this_yr{9}03) , sum(open_bal + last_yr{9}01 + last_yr{9}02 + last_yr{9}03)),sum({8}{9}03)) this_yr03,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02+ this_yr{9}03+ this_yr{9}04),sum(open_bal + last_yr{9}01 + last_yr{9}02 + last_yr{9}03 + last_yr{9}04)),sum({8}{9}04)) this_yr04,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02+ this_yr{9}03+ this_yr{9}04 +this_yr{9}05) , sum(open_bal + last_yr{9}01 + last_yr{9}02 + last_yr{9}03 + last_yr{9}04  + last_yr{9}05)),sum({8}{9}05)) this_yr05,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02+ this_yr{9}03+ this_yr{9}04 +this_yr{9}05+this_yr{9}06),sum(open_bal + last_yr{9}01 + last_yr{9}02 + last_yr{9}03 + last_yr{9}04  + last_yr{9}05 + last_yr{9}06)),sum({8}{9}06)) this_yr06,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02+ this_yr{9}03+ this_yr{9}04 +this_yr{9}05+this_yr{9}06+this_yr{9}07) , sum(open_bal + last_yr{9}01 + last_yr{9}02 + last_yr{9}03 + last_yr{9}04  + last_yr{9}05 + last_yr{9}06 + last_yr{9}07)),sum({8}{9}07)) this_yr07,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02+ this_yr{9}03+ this_yr{9}04 +this_yr{9}05+this_yr{9}06+this_yr{9}07+this_yr{9}08) , sum(open_bal + last_yr{9}01 + last_yr{9}02 + last_yr{9}03 + last_yr{9}04  + last_yr{9}05 + last_yr{9}06 + last_yr{9}07 + last_yr{9}08)),sum({8}{9}08)) this_yr08,
	if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02+ this_yr{9}03+ this_yr{9}04 +this_yr{9}05+this_yr{9}06+this_yr{9}07+this_yr{9}08+this_yr{9}09) , sum(open_bal + last_yr{9}01 + last_yr{9}02 + last_yr{9}03 + last_yr{9}04  + last_yr{9}05 + last_yr{9}06 + last_yr{9}07 + last_yr{9}08 + last_yr{9}09))	,sum({8}{9}09)) this_yr09,
if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02+ this_yr{9}03+ this_yr{9}04+this_yr{9}05+this_yr{9}06+this_yr{9}07+this_yr{9}08+this_yr{9}09+this_yr{9}10) , sum(open_bal + last_yr{9}01 + last_yr{9}02 + last_yr{9}03 + last_yr{9}04  + last_yr{9}05 + last_yr{9}06 + last_yr{9}07 + last_yr{9}08 + last_yr{9}09 + last_yr{9}10))	,sum({8}{9}10)) this_yr10,
if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02+ this_yr{9}03+ this_yr{9}04+this_yr{9}05+this_yr{9}06+this_yr{9}07+this_yr{9}08+this_yr{9}09+this_yr{9}10+this_yr{9}11) , sum(open_bal + last_yr{9}01 + last_yr{9}02 + last_yr{9}03 + last_yr{9}04  + last_yr{9}05 + last_yr{9}06 + last_yr{9}07 + last_yr{9}08 + last_yr{9}09 + last_yr{9}10 + last_yr{9}11))	,sum({8}{9}11)) this_yr11,
if((g.type='A'or g.type='L'),if('{8}'='this_yr', sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12+this_yr{9}01+this_yr{9}02+ this_yr{9}03+ this_yr{9}04+this_yr{9}05+this_yr{9}06+this_yr{9}07+this_yr{9}08+this_yr{9}09+this_yr{9}10+this_yr{9}11+this_yr{9}12) , sum(open_bal + last_yr{9}01 + last_yr{9}02 + last_yr{9}03 + last_yr{9}04  + last_yr{9}05 + last_yr{9}06 + last_yr{9}07 + last_yr{9}08 + last_yr{9}09 + last_yr{9}10 + last_yr{9}11 + last_yr{9}12))	,sum({8}{9}12)) this_yr12,
sum(open_bal) open_bal,
if((g.type='A'or g.type='L'),sum(open_bal+last_yr{9}01+last_yr{9}02+last_yr{9}03+last_yr{9}04+last_yr{9}05+last_yr{9}06+last_yr{9}07+last_yr{9}08+last_yr{9}09+last_yr{9}10+last_yr{9}11+last_yr{9}12),sum(last_yr{9}12)) last_yr_total,
	ifnull(sum(" + month_field.Replace("this", "last") + @"),0) mtdlastyear,
date_modified,

gl_te.account_no
FROM 
	GL_CHART_OF_ACCOUNTS a inner join gl_te on gl_te.id = a.gl_te_id inner join gl_group_te g on g.id = gl_te.gl_group_id
WHERE 
	{7} and gl_te.tax_entity_id = {10} 
GROUP BY
a.gl_te_id,
gl_chart_name,
	CONCAT(CONCAT(g.number,' '),g.desc),
	gl_te.GL_Designation,			 
	{0},
	g.type,
	g.number 
ORDER BY 
	gl_te.account_no,gl_te.GL_Designation,g.number", month_field, total_fields, 0, month_ind, 0, 0, current_year_fields, companies, prefix_month, acct_prefix,combo_tax_entity.Value);
        dt = Toolbox.doSQL_dt(_conn, d,null);
        if (dt.Rows.Count > 0)
        {
            var date = Toolbox.ReturnNullDateTime(dt.Rows[0]["date_modified"]);
            if (date != null)
            {
               string date2 = date==null?"unknown": Convert.ToDateTime(dt.Rows[0]["date_modified"]).ToString("yyyy-MM-dd HH:mm:ss");


                foreach (DataRow dr in dt.Rows)
                {
                    DateTime? checkDate = Toolbox.ReturnNullDateTime(dr["date_modified"]);
                    if (checkDate != null && checkDate > date)
                    {
                        date = (DateTime)checkDate;
                    }
                }

                name_branch.Text = "All Business Units Last Updated: " + Toolbox.doSQL_string(_conn, @"Select fun_time(@v0)", new object[] { date2 });
            }
        }
       
        foreach (DataRow dr in dt.Rows)
        {
            var drcr = dr["drcr"].ToString();
        var type = dr["type"].ToString();
        dr["mtd"] = post_process(drcr, Convert.ToDecimal(dr["mtd"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
            dr["ytd"] = post_process(drcr, Convert.ToDecimal(dr["ytd"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
            dr["this_yr01"] = post_process(drcr, Convert.ToDecimal(dr["this_yr01"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
            dr["this_yr02"] = post_process(drcr, Convert.ToDecimal(dr["this_yr02"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
        dr["this_yr03"] = post_process(drcr, Convert.ToDecimal(dr["this_yr03"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
        dr["this_yr04"] = post_process(drcr, Convert.ToDecimal(dr["this_yr04"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
        dr["this_yr05"] = post_process(drcr, Convert.ToDecimal(dr["this_yr05"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
        dr["this_yr06"] = post_process(drcr, Convert.ToDecimal(dr["this_yr06"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
        dr["this_yr07"] = post_process(drcr, Convert.ToDecimal(dr["this_yr07"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
        dr["this_yr08"] = post_process(drcr, Convert.ToDecimal(dr["this_yr08"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
        dr["this_yr09"] = post_process(drcr, Convert.ToDecimal(dr["this_yr09"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
        dr["this_yr10"] = post_process(drcr, Convert.ToDecimal(dr["this_yr10"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
        dr["this_yr11"] = post_process(drcr, Convert.ToDecimal(dr["this_yr11"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
        dr["this_yr12"] = post_process(drcr, Convert.ToDecimal(dr["this_yr12"]) +
                                             ((dr["account_no"].ToString() == retained_earnings) ? diff : 0), type);
            if (dr["account_no"].ToString() == retained_earnings)
            {
           
            }
            dt.AcceptChanges();
        }
        gv_trial_balance.DataSource = dt;
        double this_mtd = 0;
        double this_mtd_rev = 0;
        double this_ytd = 0;
        double this_ytd_rev = 0;
        var this_acct_no = "";
        var x = 0;

        gv_trial_balance.DataBind();
        gv_trial_balance.ExpandAll();
    }
   
	// This is being added a temporary post processor for BV data, so that the figures follow these accounting rules (Per Pat Grobe, phone call on 2017-02-09):
	// If Debit and value is positive or negative, leave it
	// If Credit and value is positive, make the value negative
	// If Credit and value is negative, make the value positive
	private decimal post_process(string _drcr, decimal _value, string _type)
		{
		   
		if((_drcr == "D"))
			{
			return _value;
			}
		return _value * -1;
		}
	private int group_counter = 0;
	protected void gv_trial_balance_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		}

	    protected void combo_tax_entity_SelectedIndexChanged(object sender, EventArgs e)
	    {
	        using (var conn = Toolbox.connect())
	        {
	            fill_business_units(conn);
	            var te_id = (int)combo_tax_entity.Value;
	            var bu = new NeBusinessUnit(myMember.business_unit_id);
	            var bu_id = Convert.ToInt32(myMember.business_unit_id);
	            if (te_id == bu.tax_entity_id)
	            {

                cl_companies.Items.FindByValue(bu_id).Selected = true;
	            }
            //    fill_gv(conn);
            fill_enddate_selector();


            }
	    }


    protected void gv_trial_balance_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	    protected void ASPxButton3_Click(object sender, EventArgs e)
	        {
	        using (var conn = Toolbox.connect())
	            {
	            NeAccounting.update_gl_chart_into_mysql(Convert.ToInt32(combo_tax_entity.Value));

	     //       fill_gv(conn);
	            }
	        }
}
