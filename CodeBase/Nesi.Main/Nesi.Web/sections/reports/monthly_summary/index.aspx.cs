using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using nesi.core;
using System.Globalization;
using MySql.Data.MySqlClient;

public partial class sections_reports_monthly_summary_index : System.Web.UI.Page
    {
    public NeMember myMember;

    private const int _page_id = 221; // from Page table in DB
    private const string _page_description = "Monthly Summary";
    static string _page_name = "Monthly_summary";
    ASPxHiddenField h;
    SqlDataSource ds_templates;
    ASPxDropDownEdit dde_filter;
    Panel panel_export;
    ASPxButton btn_export;
    ASPxButton btn_excel;
    Toolbox _tools;
    NeBusinessUnit base_c;
    string company_list = "";

    protected void Page_Init(object sender, EventArgs e)
        {
        _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(_page_id);

        _tools.dont_cache_page();
        }

    protected void Page_Load(object sender, EventArgs e)
        {

       

        _tools.dont_cache_page();
        fill_tax_entities();
        cl_companies.DataBind();
        if (cl_companies.Items.Count > 0)
            {
            if (cl_companies.SelectedIndex < 0)
                {
                cl_companies.Items[cl_companies.Items.IndexOfValue(0)].Selected = true;
            //    cl_companies.SelectedIndex = 0;
                }
          
            }
        if (!IsPostBack)
            {
            NeMenu menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
            divMenu.InnerHtml = menu.MenuHTML;
            divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
            Session["monthly_summary"] = null;
            populate_dates();
            combo_tax_entity.Value = myMember.business_unit.tax_entity_id;
            fill_business_units();
            if (cl_companies.Items.FindByValue(myMember.business_unit_id.ToString()) != null)
                {
                cl_companies.Items[cl_companies.Items.IndexOfValue(myMember.business_unit_id.ToString())].Selected = true;

                }
            else
                {
                cl_companies.Items[cl_companies.Items.IndexOfValue("1")].Selected = true;
                }

            select_enddate.DataBind();
            select_enddate.Items.FindByValue(string.Format("{0}", System.DateTime.Today.Month.ToString("00"))).Selected = true;
            RadioButtonList1.SelectedValue = "0";
            fill_grid();
            }


        }

    private void fill_tax_entities()
        {
        combo_tax_entity.DataSource = _tools.getSQL_datatable(string.Format("Select id,ddl_name from tax_entity where id in ({0})", new Current_User().visible_tax_entities), null);
        combo_tax_entity.DataBind();
        }

    protected void combo_tax_entity_SelectedIndexChanged(object sender, EventArgs e)
        {
        using (var conn = Toolbox.connect())
            {
            fill_business_units();
            var te_id = (int)combo_tax_entity.Value;
            var bu = new NeBusinessUnit(myMember.business_unit_id);
            var bu_id = Convert.ToInt32(myMember.business_unit_id);
            if (te_id == bu.tax_entity_id)
                {
                cl_companies.Items.FindByValue(bu_id.ToString()).Selected = true;
                }
            else
                {
                cl_companies.Items[0].Selected = true;
            }
           
          
            }
        }

    private void fill_business_units()
        {
        var dt_business_units = _tools.getSQL_datatable(string.Format(@" Select id,ddl_name 
from business_unit where active = 'T' and id in ({0}) and tax_entity_id = @v0 ", new Current_User().visible_business_units),
            new object[] { combo_tax_entity.Value });

        cl_companies.DataSource = dt_business_units;
        cl_companies.DataBind();
            
        }

    protected void fill_grid()
        {
        company_list = "";
        foreach (string c_selected in cl_companies.SelectedValues)
            {
            company_list += c_selected + ",";
            }
        company_list = company_list.TrimEnd(',');



        if (Session["monthly_summary"] == null)
            {
            string _month = (select_enddate.SelectedIndex + 1).ToString().PadLeft(2, '0');
            gv1.Visible = true;

            if (RadioButtonList1.SelectedValue == "1")
                {
                DataTable dt = _tools.getSQL_datatable(string.Format(@"
                SELECT

    (Select ddl_name from business_unit where id = a.business_unit_id) Business_Unit,

	round((Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id),0) Rev,
	round((Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id),0) `Rev Last Yr`,
round(((Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id) -
(Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id)),0) `Rev Variance`,

	round(((Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id) -
(Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group in (700) and business_unit_id = a.business_unit_id)),0) Margin,
	round(((Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id) -
(Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group in (700) and business_unit_id = a.business_unit_id)),0) 'Margin Last Yr',

round(((Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id) -
(Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group in (700) and business_unit_id = a.business_unit_id)) -
((Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id)-
(Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group in (700) and business_unit_id = a.business_unit_id)),0) `Margin Variance`,

round(((Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id) -
(Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group not in (750) and business_unit_id = a.business_unit_id)),0) `Net EBITDA`,
round(((Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id) -
(Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group not in (750) and business_unit_id = a.business_unit_id)),0) `Net EBITDA Last Yr`,

round((((Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id) -
(Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group not in (750) and business_unit_id = a.business_unit_id)) -
((Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id = a.business_unit_id)-
(Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group not in (750) and business_unit_id = a.business_unit_id))),0) `Net EBITDA Variance`
FROM
    GL_CHART_OF_ACCOUNTS a
JOIN gl_te b ON a.gl_te_id = b.id
JOIN gl_group_te c ON b.gl_group_id = c.id
WHERE
a.business_unit_id in ({1}) and
    c.type IN ('R', 'X') 
GROUP BY
a.business_unit_id
HAVING
	 `Rev Variance`!=0 OR `Margin Variance` != 0 or `Net EBITDA Variance` !=0
ORDER BY

    (Select ddl_name from business_unit where id = a.business_unit_id)", _month, company_list), null);
                div_results.InnerHtml = "Net Ebitda = Revenue Less all expenses EXCEPT GL Group 750 (Amortization, Depreciation and Taxes)";
             

                Session["monthly_summary"] = dt;
                }

            else if (RadioButtonList1.SelectedValue == "0")
                {
                string _this_last = "last_yr";
                if (select_enddate.SelectedItem.Text.Split(',').GetValue(1).ToString().Trim() == System.DateTime.Today.Year.ToString())
                    {
                    _this_last = "this_yr";
                    }


                DataTable dt = _tools.getSQL_datatable(string.Format(@"
                SELECT 

	(Select ddl_name from business_unit where id = a.business_unit_id) Business_Unit,

	round((Select sum({1}{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id),0) Rev,
	round((Select sum({1}_bgt_{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id),0) `Rev Budget`,
round(((Select sum({1}{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id)-
(Select sum({1}_bgt_{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id)),0) `Rev Variance`,

	round(((Select sum({1}{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id)-
(Select sum({1}{0}) from vw_gl_chart_of_accounts where type IN ('X') and gl_group in (700) and business_unit_id = a.business_unit_id)),0) Margin,
	round(((Select sum({1}_bgt_{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id)-
(Select sum({1}_bgt_{0}) from vw_gl_chart_of_accounts where type IN ('X') and gl_group in (700) and business_unit_id = a.business_unit_id)),0) `Margin Budget`,

round(((Select sum({1}{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id)-
(Select sum({1}{0}) from vw_gl_chart_of_accounts where type IN ('X') and gl_group in (700) and business_unit_id = a.business_unit_id)) -
((Select sum({1}_bgt_{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id)-
(Select sum({1}_bgt_{0}) from vw_gl_chart_of_accounts where type IN ('X') and gl_group in (700) and business_unit_id = a.business_unit_id)),0) `Margin Variance`,

round(((Select sum({1}{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id)-
(Select sum({1}{0}) from vw_gl_chart_of_accounts where type IN ('X') and gl_group not in (750) and business_unit_id = a.business_unit_id)),0) `Net EBITDA`,
round(((Select sum({1}_bgt_{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id)-
(Select sum({1}_bgt_{0}) from vw_gl_chart_of_accounts where type IN ('X') and gl_group not in (750) and business_unit_id = a.business_unit_id)),0) `Net EBITDA Budget`,

round((((Select sum({1}{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id)-
(Select sum({1}{0}) from vw_gl_chart_of_accounts where type IN ('X') and gl_group not in (750) and business_unit_id = a.business_unit_id)) -
((Select sum({1}_bgt_{0}) from vw_gl_chart_of_accounts where type IN ('R') and business_unit_id = a.business_unit_id)-
(Select sum({1}_bgt_{0}) from vw_gl_chart_of_accounts where type IN ('X') and gl_group not in (750) and business_unit_id = a.business_unit_id))),0) `Net EBITDA Variance`


FROM 
	 GL_CHART_OF_ACCOUNTS a
JOIN gl_te b ON a.gl_te_id = b.id
JOIN gl_group_te c ON b.gl_group_id = c.id


WHERE 
a.business_unit_id in ({2}) and
	c.type IN ('R','X') 
GROUP BY
a.business_unit_id
HAVING
	 `Rev Variance`!=0 OR `Margin Variance` != 0 or `Net EBITDA Variance` !=0
ORDER BY 
(Select ddl_name from business_unit where id = a.business_unit_id)", _month, _this_last, company_list),null);


                Session["monthly_summary"] = dt;
                div_results.InnerHtml = "Net Ebitda = Revenue Less all expenses EXCEPT GL Group 750 (Amortization, Depreciation and Taxes)";

                }
            else if (RadioButtonList1.SelectedValue == "2")
                {
                //        gv1.Visible = false;

                get_summary_table(1);

                }
            else if (RadioButtonList1.SelectedValue == "3")
                {
                get_summary_table(2);
                }



            }


        if ((RadioButtonList1.SelectedValue == "0") || (RadioButtonList1.SelectedValue == "1"))
            {
            gv1.DataSource = Session["monthly_summary"];

            gv1.DataBind();
            gv1.TotalSummary.Clear();
            gv1.GroupSummary.Clear();

            foreach (GridViewDataColumn gvc in gv1.Columns)
                {
                if (gvc.Index >= 1)
                    {
                    ASPxSummaryItem asi = new ASPxSummaryItem();
                    ASPxSummaryItem asi2 = new ASPxSummaryItem();
                    asi.FieldName = gvc.FieldName;
                    asi.ValueDisplayFormat = "{0:c0}";
                    asi.DisplayFormat = "{0:c0}";
                    asi.ShowInGroupFooterColumn = gvc.FieldName;
                    asi.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    gv1.GroupSummary.Add(asi);
                    asi2.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    asi2.ShowInColumn = gvc.Caption;
                    asi2.FieldName = gvc.FieldName;
                    asi2.ValueDisplayFormat = "{0:c0}";
                    asi2.DisplayFormat = "{0:c0}";
                    gv1.TotalSummary.Add(asi2);
                    gvc.PropertiesEdit.DisplayFormatString = "{0:c0}";

                    }

                }


            gv1.GroupBy(gv1.Columns["Business_Unit"]);
            //gv1.Columns["Business_Unit"].Visible = false;
            gv1.ExpandAll();

            }



        }


    protected void get_summary_table(int type)
        {
        string _month = (select_enddate.SelectedIndex + 1).ToString().PadLeft(2, '0');
        string _ytd_rev = "";
        string _ytd_bud = "";
        string lst_ytd_rev = "";

        for (int i = 1; i <= select_enddate.SelectedIndex + 1; i++)
            {
            _ytd_rev += "this_yr" + i.ToString().PadLeft(2, '0') + "+";
            _ytd_bud += "this_yr_bgt_" + i.ToString().PadLeft(2, '0') + "+";
            lst_ytd_rev += "last_yr" + i.ToString().PadLeft(2, '0') + "+";
            }
        _ytd_rev = _ytd_rev.TrimEnd('+');
        _ytd_bud = _ytd_bud.TrimEnd('+');
        lst_ytd_rev = lst_ytd_rev.TrimEnd('+');
        DataTable dt = new DataTable();
        dt.Columns.Add("Description");
        dt.Columns.Add("MTD Actual");
        if (type == 1)
            {
            dt.Columns.Add("MTD Budget");
            }
        else
            {
            dt.Columns.Add("MTD Last Yr");
            }
        dt.Columns.Add("MTD Variance");
        dt.Columns.Add("YTD Actual");
        if (type == 1)
            {
            dt.Columns.Add("YTD Budget");
            }
        else
            {
            dt.Columns.Add("YTD Last Yr");
            }
        dt.Columns.Add("YTD Variance");

        if (company_list.Split(',').Length == 1)
            {
            company_list = "0";
            }

       // company_list = "0";

        double rev_act_mtd = _tools.getSQL_double(string.Format("Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id in ({1}) and tax_entity_id = {2}", _month, company_list,combo_tax_entity.Value),null);
        double rev_bud_mtd = (type == 1) ? _tools.getSQL_double(string.Format("Select sum(this_yr_bgt_{0}) from vw_gl_chart_of_accounts where type IN('R') and business_unit_id in ({1}) and tax_entity_id = {2}", _month, company_list, combo_tax_entity.Value), null) :
            _tools.getSQL_double(string.Format("Select sum(last_yr{0}) from vw_gl_chart_of_accounts where  type IN('R') and business_unit_id in ({1}) and tax_entity_id = {2}", _month, company_list, combo_tax_entity.Value), null);
        double rev_act_ytd = _tools.getSQL_double(string.Format("Select sum({0}) from vw_gl_chart_of_accounts where  type IN('R') and business_unit_id in ({1}) and tax_entity_id = {2}", _ytd_rev, company_list, combo_tax_entity.Value), null);
        double rev_bud_ytd = (type == 1) ? _tools.getSQL_double(string.Format("Select sum({0}) from vw_gl_chart_of_accounts where  type IN('R') and business_unit_id in ({1}) and tax_entity_id = {2}", _ytd_bud, company_list, combo_tax_entity.Value), null) :
            _tools.getSQL_double(string.Format("Select sum({0}) from vw_gl_chart_of_accounts where  type IN('R') and business_unit_id in ({1}) and tax_entity_id = {2}", lst_ytd_rev, company_list, combo_tax_entity.Value), null);

        double mar_act_mtd = rev_act_mtd - _tools.getSQL_double(string.Format("Select sum(this_yr{0}) from vw_gl_chart_of_accounts where  type IN('X') and gl_group in (700) and business_unit_id in ({1}) and tax_entity_id = {2}", _month, company_list, combo_tax_entity.Value), null);
        double mar_bud_mtd = (type == 1) ? rev_bud_mtd - _tools.getSQL_double(string.Format("Select sum(this_yr_bgt_{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group in (700) and business_unit_id in ({1}) and tax_entity_id = {2}", _month, company_list, combo_tax_entity.Value), null) :
            rev_bud_mtd - _tools.getSQL_double(string.Format("Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group in (700) and business_unit_id in ({1}) and tax_entity_id = {2}", _month, company_list, combo_tax_entity.Value), null);

        double mar_act_ytd = rev_act_ytd - _tools.getSQL_double(string.Format("Select sum({0}) from vw_gl_chart_of_accounts where  type IN('X') and gl_group in (700) and business_unit_id in ({1}) and tax_entity_id = {2}", _ytd_rev, company_list, combo_tax_entity.Value), null);
        double mar_bud_ytd = (type == 1) ? rev_bud_ytd - _tools.getSQL_double(string.Format("Select sum({0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group in (700) and business_unit_id in ({1}) and tax_entity_id = {2}", _ytd_bud, company_list, combo_tax_entity.Value), null) :
            rev_act_ytd - _tools.getSQL_double(string.Format("Select sum({0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group in (700) and business_unit_id in ({1}) and tax_entity_id = {2}", lst_ytd_rev, company_list, combo_tax_entity.Value), null);


        double ov_act_mtd = _tools.getSQL_double(string.Format("Select ifnull((Select sum(this_yr{0}) from vw_gl_chart_of_accounts where  type IN('X') and gl_group  in (710,720,725,730) and (is_corporate = 0 || is_corporate is null) and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null);
        double ov_bud_mtd = (type == 1) ? _tools.getSQL_double(string.Format("Select ifnull((Select sum(this_yr_bgt_{0}) from vw_gl_chart_of_accounts where  type IN('X') and gl_group in (710,720,725,730) and (is_corporate = 0 || is_corporate is null) and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null) :
            _tools.getSQL_double(string.Format("Select ifnull((Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group  in (710,720,725,730) and (is_corporate = 0 || is_corporate is null) and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null);

        double ov_act_ytd = _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where  type IN('X') and gl_group  in (710,720,725,730) and (is_corporate = 0 || is_corporate is null) and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _ytd_rev, company_list, combo_tax_entity.Value), null);
        double ov_bud_ytd = (type == 1) ? _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where  type IN('X') and gl_group in (710,720,725,730) and (is_corporate = 0 || is_corporate is null) and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _ytd_bud, company_list, combo_tax_entity.Value), null) :
            _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group  in (710,720,725,730) and (is_corporate = 0 || is_corporate is null) and business_unit_id in ({1}) and tax_entity_id = {2}),0)", lst_ytd_rev, company_list, combo_tax_entity.Value), null);

        double ov2_act_mtd = _tools.getSQL_double(string.Format("Select ifnull((Select sum(this_yr{0}) from vw_gl_chart_of_accounts where  type IN('X') and ((gl_group  in (710,720,725,730)  and is_corporate = 1) or gl_group = 740)   and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null);
        double ov2_bud_mtd = (type == 1) ? _tools.getSQL_double(string.Format("Select ifnull((Select sum(this_yr_bgt_{0}) from vw_gl_chart_of_accounts where type IN('X') and ((gl_group  in (710,720,725,730)  and is_corporate = 1) or gl_group = 740)  and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null) :
            _tools.getSQL_double(string.Format("Select ifnull((Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and ((gl_group  in (710,720,725,730)  and is_corporate = 1) or gl_group = 740)  and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null);

        double ov2_act_ytd = _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where type IN('X') and ((gl_group  in (710,720,725,730)  and is_corporate = 1) or gl_group = 740)  and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _ytd_rev, company_list, combo_tax_entity.Value), null);
        double ov2_bud_ytd = (type == 1) ? _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where  type IN('X') and ((gl_group  in (710,720,725,730)  and is_corporate = 1) or gl_group = 740)   and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _ytd_bud, company_list, combo_tax_entity.Value), null) :
            _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where  type IN('X') and ((gl_group  in (710,720,725,730)  and is_corporate = 1) or gl_group = 740)   and business_unit_id in ({1}) and tax_entity_id = {2}),0)", lst_ytd_rev, company_list, combo_tax_entity.Value), null);

        //and gl_id in(89910,89850)

        double dm_act_mtd = mar_act_mtd - _tools.getSQL_double(string.Format("Select ifnull((Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group  in (710,720,725,730,740,760)  and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null);
        double dm_bud_mtd = (type == 1) ? mar_bud_mtd - _tools.getSQL_double(string.Format("Select ifnull((Select sum(this_yr_bgt_{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group in (710,720,725,730,740,760)   and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null) :
            mar_bud_mtd - _tools.getSQL_double(string.Format("Select ifnull((Select sum(last_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group  in (710,720,725,730,740,760) and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null);

        double dm_act_ytd = mar_act_ytd - _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where type IN('X') and gl_group  in (710,720,725,730,740,760)   and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _ytd_rev, company_list, combo_tax_entity.Value), null);
        double dm_bud_ytd = (type == 1) ? mar_bud_ytd - _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where  type IN('X') and gl_group in (710,720,725,730,740,760)   and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _ytd_bud, company_list, combo_tax_entity.Value), null) :
            mar_bud_ytd - _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where  type IN('X') and gl_group  in (710,720,725,730,740,760)  and business_unit_id in ({1}) and tax_entity_id = {2}),0)", lst_ytd_rev, company_list, combo_tax_entity.Value), null);

        double net_act_mtd = rev_act_mtd - _tools.getSQL_double(string.Format("Select ifnull((Select sum(this_yr{0}) from vw_gl_chart_of_accounts where type IN('X') and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null);
        double net_bud_mtd = (type == 1) ? rev_bud_mtd - _tools.getSQL_double(string.Format("Select ifnull((Select sum(this_yr_bgt_{0}) from vw_gl_chart_of_accounts where  type IN('X')  and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null) :
            rev_bud_mtd - _tools.getSQL_double(string.Format("Select ifnull((Select sum(last_yr{0}) from vw_gl_chart_of_accounts where  type IN('X') and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _month, company_list, combo_tax_entity.Value), null);

        double net_act_ytd = rev_act_ytd - _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where type IN('X') and  business_unit_id in ({1}) and tax_entity_id = {2}),0)", _ytd_rev, company_list, combo_tax_entity.Value), null);
        double net_bud_ytd = (type == 1) ? rev_bud_ytd - _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where   type IN('X') and business_unit_id in ({1}) and tax_entity_id = {2}),0)", _ytd_bud, company_list, combo_tax_entity.Value), null) :
            rev_bud_ytd - _tools.getSQL_double(string.Format("Select ifnull((Select sum({0}) from vw_gl_chart_of_accounts where   type IN('X') and business_unit_id in ({1}) and tax_entity_id = {2}),0)", lst_ytd_rev, company_list, combo_tax_entity.Value), null);


        dt.Rows.Add("Revenue", rev_act_mtd.ToString("C2"), rev_bud_mtd.ToString("C2"), (rev_act_mtd - rev_bud_mtd).ToString("C2"), rev_act_ytd.ToString("C2"), rev_bud_ytd.ToString("C2"), (rev_act_ytd - rev_bud_ytd).ToString("C2"));
        dt.Rows.Add("Gross Margin", mar_act_mtd.ToString("C2"), mar_bud_mtd.ToString("C2"), (mar_act_mtd - mar_bud_mtd).ToString("C2"), mar_act_ytd.ToString("C2"), mar_bud_ytd.ToString("C2"), (mar_act_ytd - mar_bud_ytd).ToString("C2"));
        dt.Rows.Add("Gross Margin %", (mar_act_mtd / rev_act_mtd).ToString("P1"), (mar_bud_mtd / rev_bud_mtd).ToString("P1"), ((mar_act_mtd / rev_act_mtd) - (mar_bud_mtd / rev_bud_mtd)).ToString("P1"), (mar_act_ytd / rev_act_ytd).ToString("P1"), (mar_bud_ytd / rev_bud_ytd).ToString("P1"), ((mar_act_ytd / rev_act_ytd) - (mar_bud_ytd / rev_bud_ytd)).ToString("P1"));
        dt.Rows.Add("Overhead", ov_act_mtd.ToString("C2"), ov_bud_mtd.ToString("C2"), (ov_act_mtd - ov_bud_mtd).ToString("C2"), ov_act_ytd.ToString("C2"), ov_bud_ytd.ToString("C2"), (ov_act_ytd - ov_bud_ytd).ToString("C2"));
        dt.Rows.Add("Net Ordinary Income", (mar_act_mtd - ov_act_mtd).ToString("C2"), (mar_bud_mtd - ov_bud_mtd).ToString("C2"), ((mar_act_mtd - ov_act_mtd) - (mar_bud_mtd - ov_bud_mtd)).ToString("C2"), (mar_act_ytd - ov_act_ytd).ToString("C2"), (mar_bud_ytd - ov_bud_ytd).ToString("C2"), ((mar_act_ytd - ov_act_ytd) - (mar_bud_ytd - ov_bud_ytd)).ToString("C2"));
        dt.Rows.Add("Net Ordinary Income %", ((mar_act_mtd - ov_act_mtd) / rev_act_mtd).ToString("P1"), ((mar_bud_mtd - ov_bud_mtd) / rev_bud_mtd).ToString("P1"), (((mar_act_mtd - ov_act_mtd) / rev_act_mtd) - ((mar_bud_mtd - ov_bud_mtd) / rev_bud_mtd)).ToString("P1"), ((mar_act_ytd - ov_act_ytd) / rev_act_ytd).ToString("P1"), ((mar_bud_ytd - ov_bud_ytd) / rev_bud_ytd).ToString("P1"), (((mar_act_ytd - ov_act_ytd) / rev_act_ytd) - ((mar_bud_ytd - ov_bud_ytd) / rev_bud_ytd)).ToString("P1"));
        dt.Rows.Add("OP EBITDA", (mar_act_mtd - ov_act_mtd - ov2_act_mtd).ToString("C2"), (mar_bud_mtd- ov_bud_mtd - ov2_bud_mtd).ToString("C2"), ((mar_act_mtd - ov_act_mtd - ov2_act_mtd) - (mar_bud_mtd - ov_bud_mtd - ov2_bud_mtd)).ToString("C2"), (mar_act_ytd - ov_act_ytd - ov2_act_ytd).ToString("C2"), (mar_bud_ytd - ov_bud_ytd - ov2_bud_ytd).ToString("C2"), ((mar_act_ytd - ov_act_ytd - ov2_act_ytd) - (mar_bud_ytd - ov_bud_ytd - ov2_bud_ytd)).ToString("C2"));
        dt.Rows.Add("OP EBITDA %", ((mar_act_mtd - ov_act_mtd - ov2_act_mtd) / rev_act_mtd).ToString("P1"), ((mar_bud_mtd - ov_bud_mtd - ov2_bud_mtd) / rev_bud_mtd).ToString("P1"), (((mar_act_mtd - ov_act_mtd - ov2_act_mtd) / rev_act_mtd) - ((mar_bud_mtd - ov_bud_mtd - ov2_bud_mtd) / rev_bud_mtd)).ToString("P1"), ((mar_act_ytd - ov_act_ytd - ov2_act_ytd) / rev_act_ytd).ToString("P1"), ((mar_bud_ytd - ov_bud_ytd - ov2_bud_ytd) / rev_bud_ytd).ToString("P1"), (((mar_act_ytd - ov_act_ytd - ov2_act_ytd) / rev_act_ytd) - ((mar_bud_ytd - ov_bud_ytd - ov2_bud_ytd) / rev_bud_ytd)).ToString("P1"));
        dt.Rows.Add("EBITDA", dm_act_mtd.ToString("C2"), dm_bud_mtd.ToString("C2"), (dm_act_mtd - dm_bud_mtd).ToString("C2"), dm_act_ytd.ToString("C2"), dm_bud_ytd.ToString("C2"), (dm_act_ytd - dm_bud_ytd).ToString("C2"));
        dt.Rows.Add("EBITDA %", (dm_act_mtd / rev_act_mtd).ToString("P1"), (dm_bud_mtd / rev_bud_mtd).ToString("P1"), ((dm_act_mtd / rev_act_mtd) - (dm_bud_mtd / rev_bud_mtd)).ToString("P1"), (dm_act_ytd / rev_act_ytd).ToString("P1"), (dm_bud_ytd / rev_bud_ytd).ToString("P1"), ((dm_act_ytd / rev_act_ytd) - (dm_bud_ytd / rev_bud_ytd)).ToString("P1"));
        dt.Rows.Add("Net Income", net_act_mtd.ToString("C2"), net_bud_mtd.ToString("C2"), (net_act_mtd - net_bud_mtd).ToString("C2"), net_act_ytd.ToString("C2"), net_bud_ytd.ToString("C2"), (net_act_ytd - net_bud_ytd).ToString("C2"));
        dt.Rows.Add("Net Income %", (net_act_mtd / rev_act_mtd).ToString("P1"), (net_bud_mtd / rev_bud_mtd).ToString("P1"), ((net_act_mtd / rev_act_mtd) - (net_bud_mtd / rev_bud_mtd)).ToString("P1"), (net_act_ytd / rev_act_ytd).ToString("P1"), (net_bud_ytd / rev_bud_ytd).ToString("P1"), ((net_act_ytd / rev_act_ytd) - (net_bud_ytd / rev_bud_ytd)).ToString("P1"));

        gv1.DataSource = dt;
        gv1.DataBind();

        gv1.Settings.ShowGroupPanel = false;
        gv1.SettingsBehavior.AllowSort = false;

        div_results.InnerHtml = " </br></br>";
        div_results.InnerHtml += "Overhead = Sum of GL Groups 710,720,725,730 for all business units set as 'Non Corporate'</br>";
        div_results.InnerHtml += "Net Ordinary Income = Margin less Overhead</br>";
        div_results.InnerHtml += "OP EBITDA = Net Ordinary Income less expenses from all business units set as 'Corporate'</br>";
        div_results.InnerHtml += "EBITDA = OP EBITDA Less Sum of GL Groups 760 for all business units </br>";
        div_results.InnerHtml += "Net Income = Total Revenue less all expenses (all business units) </br>";
       
        }


    protected void populate_dates()
        {
        var DATE = DateTime.Now;
        #region Fiscal End Date
        // This is for getting the current year's options
        base_c = new NeBusinessUnit(1);
        //	for (int i = NeCompany.FiscalMonthLookup[base_c.fiscal_yearstart_month][DATE.Month]; i >= 1; i--)
        for (var i = 1; i <= 12; i++)
            {
            var working_month = NeBusinessUnit.FiscalMonthList[base_c.fiscal_yearstart_month][i];
            var working_year = base_c.fiscal_current_year;
            var month_name = new DateTime(working_year, working_month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
            var month_end = DateTime.DaysInMonth(working_year, working_month);
            var li = new ListItem(string.Format("{0} {1}, {2}", month_name, month_end, working_year), string.Format("{1:00}", working_year, working_month));
            select_enddate.Items.Add(li);
            }

        // This is for getting the previous year's options


        //	select_enddate.Attributes.Add("onchange", "do_fiscal(this);");
        #endregion Fiscal End Date
        }

    protected void select_dept_SelectedIndexChanged(object sender, EventArgs e)
        {
        if ((select_enddate.SelectedIndex >= 0) && (cl_companies.SelectedItems.Count > 0))
            {
            Session["monthly_summary"] = null;
            fill_grid();
            }
        }

    protected void gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

        }

    protected void gv_customer_assets_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {

        }
    protected void gv_customer_assets_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
        ASPxGridView gv = (ASPxGridView)sender;
        if (e.Parameters != "")
            {
            gv.LoadClientLayout(e.Parameters);
            gv1.ExpandAll();
            }
        else
            {
            gv.FilterExpression = "";
            for (int i = 0; i < gv.Columns.Count; i++)
                {
                if (gv.Columns[i] is GridViewDataColumn)
                    {
                    GridViewDataColumn col = (GridViewDataColumn)gv.Columns[i];
                    if (col.GroupIndex > -1)
                        {
                        gv.UnGroup(col);
                        }
                    col.Visible = true;
                    }
                }
            }
        }
    protected void ASPxButton1_Click(object sender, EventArgs e)
        {
        //todo figure out what TE should be updated
        //is this ever called?
        //NeAccounting.update_gl_chart_into_mysql();
        Session["monthly_summary"] = null;
        fill_grid();
        }

    protected void gv1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
        if (e.VisibleIndex >= 0)
            {
            if ((e.DataColumn.FieldName.Substring(0, 1).Equals("1") || (e.DataColumn.FieldName.Equals("Total"))))
            {
                if (Toolbox.ReturnZeroIfNull_double(e.CellValue) != 0)
                {
                    e.Cell.Text = Convert.ToDouble(e.CellValue).ToString("C2");
                    if (e.DataColumn.FieldName.Equals("Total"))
                    {
                        e.Cell.Font.Bold = true;
                    }
                }
            }
            }
        }


    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
        if ((RadioButtonList1.SelectedIndex >= 0) )
            {
            Session["monthly_summary"] = null;
            fill_grid();
            }
        }

    protected void ASPxButton2_Click(object sender, EventArgs e)
        {
        fill_grid();
        string company_name = "";
        if (cl_companies.SelectedValues.Count == 1)
            {
            company_name = cl_companies.SelectedItems[0].Text;
            }
        if (RadioButtonList1.SelectedValue == "1")
            {
            ASPxGridViewExporter1.FileName = company_name + " LOB Summary :" + select_enddate.Text + " YOY";
            }
        else if (RadioButtonList1.SelectedValue == "0")
            {
            ASPxGridViewExporter1.FileName = company_name + " LOB Summary :" + select_enddate.Text + " - Budget";
            }
        else if (RadioButtonList1.SelectedValue == "2")
            {
            ASPxGridViewExporter1.FileName = company_name + " LOB Summary to Budget ";
            }
        else if (RadioButtonList1.SelectedValue == "3")
            {
            ASPxGridViewExporter1.FileName = company_name + " LOB Summary to Last Year ";
            }
        ASPxGridViewExporter1.WriteXlsToResponse();

        }



    protected void btn_go_Click(object sender, EventArgs e)
        {
        if ((RadioButtonList1.SelectedIndex >= 0) && (select_enddate.SelectedIndex >= 0) && (cl_companies.SelectedItems.Count > 0))
            {
            Session["monthly_summary"] = null;
            fill_grid();
            }
        }
    }
