using DevExpress.Web;
//using nesi.bv;
using nesi.core;
using System;
using System.Collections.Specialized;
using System.Data;

public partial class gl_structure : System.Web.UI.Page
{
//    NeMember myMember;
//    public Toolbox _tools = new Toolbox();
//    private const int _page_id = 9; // from Page table in DB
//    NameValueCollection _q;
//    DataTable dt_glhistory;
  
//    protected void Page_Init(object sender, EventArgs e)
//    {
//        _q = Request.QueryString;
//        hdn_tax_ent_it.Value = _q["te_id"];
//        if (IsPostBack)
//        {
//            fill_history();
//        }
//    }

//    protected void Page_Load(object sender, EventArgs e)
//    {

//        myMember = Toolbox.do_handle_authentication(_page_id);
//        if (!IsPostBack)
//        {
//            Session["dt_glhistory"] = null;
//            Session["dt_glspecial_accounts"] = null;

//        }
//        fill_history();
//    if (pc.ActiveTabIndex == 2)
//        {
//        fill_special_accounts();

//        }
//    }


   


//    protected void fill_history()
//    {
//        if (Session["dt_glhistory"] == null)
//        {
//            NeTaxEntity te = new NeTaxEntity(hdn_tax_ent_it.Value);
//			if(te.DSN == "") return;
//            if (NeTaxEntity.is_int_gl(hdn_tax_ent_it.Value))
//                dt_glhistory = _tools.getSQL_datatable(@"Select distinct substring(acct_no,7,5) acct_no from gl_transactions  where division <>''", te.DSN , null);
//            else
//                dt_glhistory = _tools.getSQL_datatable(@"Select distinct acct_no from gl_transactions  where division <>''", te.DSN , null);
//            Session["dt_glhistory"] = dt_glhistory;
//        }
//        dt_glhistory = (DataTable)Session["dt_glhistory"];


//    }






//    protected void btn_clear_unused_Click(object sender, EventArgs e)
//    {
//        // grab all GLs current visible on the screen
//        for (int i = 0; i < gv.VisibleRowCount; i++)
//        {
//            int id = (int)gv.GetRowValues(i, "id");
//            if (dt_glhistory.Select("acct_no = '" + gv.GetRowValues(i, "account_no") + "'").Length == 0)
//            {
//                NeGL _gl = new NeGL(id);

//                // commented out by andy                     _gl.delete();
//            }

//        }
//        gv.DataBind();
//        Session["dt_glhistory"] = null;
//        fill_history();
//    }

//    protected DataTable get_all_bv_gls(string dsn, bool include_empty_gls)
//    {
//        DataTable dt;
//        if (include_empty_gls)
//        {
//            dt = _tools.getSQL_datatable(@" Select * from gl_chart_of_accounts left outer join gl_groups on gl_groups.gl_group = gl_chart_of_accounts.gl_group  where Division = '000' order by acct_no", dsn, null);
//        }
//        else
//        {
//            dt = _tools.getSQL_datatable(@" Select * from gl_chart_of_accounts left outer join gl_groups on gl_groups.gl_group = gl_chart_of_accounts.gl_group  where Division = '000' and (acct_no = '39401' or acct_no = '35500' or acct_no = '35005' or open_bal <> 0 or this_yr01 <>0 or this_yr02 <>0 or this_yr03 <>0 or this_yr04 <>0 or this_yr05 <>0 or this_yr06 <>0 or this_yr07 <>0 or this_yr08 <>0 or this_yr09 <>0 or this_yr10 <>0 or this_yr11 <>0 or this_yr12 <>0 or this_yr13 <>0 or next_yr01 <>0 or next_yr02 <>0 or next_yr03 <>0 or next_yr04 <>0 or next_yr05 <>0 or next_yr06 <>0 or next_yr07 <>0 or next_yr08 <>0 or next_yr09 <>0 or next_yr10 <>0 or next_yr11 <>0 or next_yr12 <>0 or next_yr13 <>0 or last_yr01 <>0 or last_yr02 <>0 or last_yr03 <>0 or last_yr04 <>0 or last_yr05 <>0 or last_yr06 <>0 or last_yr07 <>0 or last_yr08 <>0 or last_yr09 <>0 or last_yr10 <>0 or last_yr11 <>0 or last_yr12 <>0 or last_yr13 <>0 or bdgt_last_yr01 <>0 or bdgt_last_yr02 <>0 or bdgt_last_yr03 <>0 or bdgt_last_yr04 <>0 or bdgt_last_yr05 <>0 or bdgt_last_yr06 <>0 or bdgt_last_yr07 <>0 or bdgt_last_yr08 <>0 or bdgt_last_yr09 <>0 or bdgt_last_yr10 <>0 or bdgt_last_yr11 <>0 or bdgt_last_yr12 <>0 or bdgt_last_yr13 <>0 or bdgt_this_yr01 <>0 or bdgt_this_yr02 <>0 or bdgt_this_yr03 <>0 or bdgt_this_yr04 <>0 or bdgt_this_yr05 <>0 or bdgt_this_yr06 <>0 or bdgt_this_yr07 <>0 or bdgt_this_yr08 <>0 or bdgt_this_yr09 <>0 or bdgt_this_yr10 <>0 or bdgt_this_yr11 <>0 or bdgt_this_yr12 <>0 or bdgt_this_yr13 <>0 or bdgt_next_yr01 <>0 or bdgt_next_yr02 <>0 or bdgt_next_yr03 <>0 or bdgt_next_yr04 <>0 or bdgt_next_yr05 <>0 or bdgt_next_yr06 <>0 or bdgt_next_yr07 <>0 or bdgt_next_yr08 <>0 or bdgt_next_yr09 <>0 or bdgt_next_yr10 <>0 or bdgt_next_yr11 <>0 or bdgt_next_yr12 <>0 or bdgt_next_yr13 <>0) order by acct_no", dsn, null);
//        }
      

//        return dt;
//    }

//    protected DataTable get_bv_balance_sheet_gls(int te_id)
//    {
//        DataTable dt;

//        dt = _tools.getSQL_datatable(@" SELECT gl_chart_of_accounts.id, gl_te.account_no acct_no, gl_te.gl_chart_name name, gl_te.id gl_id FROM gl_chart_of_accounts INNER JOIN gl_te ON gl_te.id = gl_chart_of_accounts.gl_te_id INNER JOIN gl_group_te ON gl_te.gl_group_id = gl_group_te.id  WHERE gl_chart_of_accounts.business_unit_id = 0 and (gl_group_te.type = 'A' OR gl_group_te.type = 'L' or (gl_te.account_no=39401 or gl_te.account_no=35005)) and (gl_te.account_no=39401 or gl_te.account_no=35005) AND gl_group_te.tax_entity_id =@v0 ", new object[] { te_id });

    

//        return dt;
//    }

//    protected DataTable get_bv_income_statement_gls(string dsn)
//    {
//        DataTable dt;

//        dt = _tools.getSQL_datatable(@" Select * from gl_chart_of_accounts left outer join gl_groups on gl_groups.gl_group = gl_chart_of_accounts.gl_group  where Division = '000' and (gl_groups.ACCT_TYPE<>'A' and gl_groups.ACCT_TYPE<>'L') and (open_bal <> 0 or this_yr01 <>0 or this_yr02 <>0 or this_yr03 <>0 or this_yr04 <>0 or this_yr05 <>0 or this_yr06 <>0 or this_yr07 <>0 or this_yr08 <>0 or this_yr09 <>0 or this_yr10 <>0 or this_yr11 <>0 or this_yr12 <>0 or this_yr13 <>0 or next_yr01 <>0 or next_yr02 <>0 or next_yr03 <>0 or next_yr04 <>0 or next_yr05 <>0 or next_yr06 <>0 or next_yr07 <>0 or next_yr08 <>0 or next_yr09 <>0 or next_yr10 <>0 or next_yr11 <>0 or next_yr12 <>0 or next_yr13 <>0 or last_yr01 <>0 or last_yr02 <>0 or last_yr03 <>0 or last_yr04 <>0 or last_yr05 <>0 or last_yr06 <>0 or last_yr07 <>0 or last_yr08 <>0 or last_yr09 <>0 or last_yr10 <>0 or last_yr11 <>0 or last_yr12 <>0 or last_yr13 <>0 or bdgt_last_yr01 <>0 or bdgt_last_yr02 <>0 or bdgt_last_yr03 <>0 or bdgt_last_yr04 <>0 or bdgt_last_yr05 <>0 or bdgt_last_yr06 <>0 or bdgt_last_yr07 <>0 or bdgt_last_yr08 <>0 or bdgt_last_yr09 <>0 or bdgt_last_yr10 <>0 or bdgt_last_yr11 <>0 or bdgt_last_yr12 <>0 or bdgt_last_yr13 <>0 or bdgt_this_yr01 <>0 or bdgt_this_yr02 <>0 or bdgt_this_yr03 <>0 or bdgt_this_yr04 <>0 or bdgt_this_yr05 <>0 or bdgt_this_yr06 <>0 or bdgt_this_yr07 <>0 or bdgt_this_yr08 <>0 or bdgt_this_yr09 <>0 or bdgt_this_yr10 <>0 or bdgt_this_yr11 <>0 or bdgt_this_yr12 <>0 or bdgt_this_yr13 <>0 or bdgt_next_yr01 <>0 or bdgt_next_yr02 <>0 or bdgt_next_yr03 <>0 or bdgt_next_yr04 <>0 or bdgt_next_yr05 <>0 or bdgt_next_yr06 <>0 or bdgt_next_yr07 <>0 or bdgt_next_yr08 <>0 or bdgt_next_yr09 <>0 or bdgt_next_yr10 <>0 or bdgt_next_yr11 <>0 or bdgt_next_yr12 <>0 or bdgt_next_yr13 <>0 ) order by acct_no", dsn , null);
//        return dt;
//    }

//    protected void btn_gomerge_frombv_Click(object sender, EventArgs e)
//    {
//        div_inbv.InnerHtml = "";
//        if (ddl_structure_from_legacy.Text != null && ddl_structure_from_legacy.Text != "")
//        {
//            string dsn = ddl_structure_from_legacy.Text;
//            ddl_data_from_legacy.Value = "";

//            go_merge_structure_from_dsn(dsn);
//            ddl_structure_from_legacy.Value = null;
//        }
//        else if (ddl_data_from_legacy.Value != null && ddl_data_from_legacy.Value != "")
//        {
//            ddl_structure_from_legacy.Value = "";
//            if (_tools.getSQL_int(@"Select count(*) from gl_divisions", ddl_data_from_legacy.Value.ToString()  , null) > 1)
//            {
//                //TODO this is a nono. we cant be using division-y
//                foreach (DataRow dr in _tools.getSQL_datatable(@"Select * from `division-y`  where division_id in (2,3,5,6,7,9,10) order by division_id" , null).Rows)
//                {
//                    go_merge_data_from_dsn(ddl_data_from_legacy.Text, dr["division_bvdept"].ToString());
//                }
//            }
//            else
//            {
//                go_merge_data_from_dsn(ddl_data_from_legacy.Text, "000");
//            }
//            ddl_data_from_legacy.Value = null;
//        }
//        else
//        {
//            ddl_structure_from_legacy.Text = "";
//            ddl_data_from_legacy.Value = "";
//            go_merge_bs_data_from_dsn();

//        }
//        gv.DataBind();
//        gv_groups.DataBind();
//        pop_frombv.ShowOnPageLoad = true;
//    }

//    // this function grabs OLD dsns and merges them INTO one set of GL accounts in MYSQL only
//    protected void go_merge_bs_data_from_dsn()
//        {
//        NeTaxEntity new_te = new NeTaxEntity(hdn_tax_ent_it.Value);
//        int tax_entity_id = Convert.ToInt32(hdn_tax_ent_it.Value);

//        using (var BVconn = BVDB.connect(new_te.DSN))
//            {
//            DataTable dt = get_bv_balance_sheet_gls(Convert.ToInt32(hdn_tax_ent_it.Value));

//            DataTable dt_old_dsns =
//                _tools.getSQL_datatable(@"Select distinct old_dsn from business_unit  where tax_entity_id =@v0",
//                    new object[] {new_te.id});
//            foreach (DataRow dr_old_dsns in dt_old_dsns.Rows)
//                {
//                Session["old_dsn_" + dr_old_dsns["old_dsn"]] =
//                    _tools.getSQL_datatable(@"Select * from gl_chart_of_accounts", dr_old_dsns["old_dsn"].ToString(),
//                        null);
//                }

//            foreach (DataRow dr in dt.Rows) // loop through GL accounts 
//                {
//                string acct_no = dr["acct_no"].ToString();
//                int gl_id = Convert.ToInt32(dr["gl_id"]);
//                NeGL gl = new NeGL(gl_id);
              
//                #region set div  000
//                NeGLChart_of_accounts new_dsn_acct = new NeGLChart_of_accounts(0, gl_id);
//                NeGLChart_of_accounts.clear_consol_acct(new_dsn_acct.id, true, BVconn);
//                new_dsn_acct = new NeGLChart_of_accounts(0, gl_id);

//                foreach (DataRow dr_old_dsns in dt_old_dsns.Rows)
//                    {
//                    DataTable dt_old_accts = (DataTable) Session["old_dsn_" + dr_old_dsns["old_dsn"]];
//                    if (dt_old_accts.Select("Division='000' and acct_no='" + acct_no + "'").Length > 0)
//                        {
//                        DataRow dr_old_acct = dt_old_accts.Select("Division='000' and acct_no='" + acct_no + "'")
//                            .CopyToDataTable().Rows[0];
//                        new_dsn_acct.next_yr01 += Convert.ToDouble(dr_old_acct["next_yr01"]);
//                        new_dsn_acct.next_yr02 += Convert.ToDouble(dr_old_acct["next_yr02"]);
//                        new_dsn_acct.next_yr03 += Convert.ToDouble(dr_old_acct["next_yr03"]);
//                        new_dsn_acct.next_yr04 += Convert.ToDouble(dr_old_acct["next_yr04"]);
//                        new_dsn_acct.next_yr05 += Convert.ToDouble(dr_old_acct["next_yr05"]);
//                        new_dsn_acct.next_yr06 += Convert.ToDouble(dr_old_acct["next_yr06"]);
//                        new_dsn_acct.next_yr07 += Convert.ToDouble(dr_old_acct["next_yr07"]);
//                        new_dsn_acct.next_yr08 += Convert.ToDouble(dr_old_acct["next_yr08"]);
//                        new_dsn_acct.next_yr09 += Convert.ToDouble(dr_old_acct["next_yr09"]);
//                        new_dsn_acct.next_yr10 += Convert.ToDouble(dr_old_acct["next_yr10"]);
//                        new_dsn_acct.next_yr11 += Convert.ToDouble(dr_old_acct["next_yr11"]);
//                        new_dsn_acct.next_yr12 += Convert.ToDouble(dr_old_acct["next_yr12"]);
//                        new_dsn_acct.next_yr13 += Convert.ToDouble(dr_old_acct["next_yr13"]);

//                        new_dsn_acct.last_yr01 += Convert.ToDouble(dr_old_acct["last_yr01"]);
//                        new_dsn_acct.last_yr02 += Convert.ToDouble(dr_old_acct["last_yr02"]);
//                        new_dsn_acct.last_yr03 += Convert.ToDouble(dr_old_acct["last_yr03"]);
//                        new_dsn_acct.last_yr04 += Convert.ToDouble(dr_old_acct["last_yr04"]);
//                        new_dsn_acct.last_yr05 += Convert.ToDouble(dr_old_acct["last_yr05"]);
//                        new_dsn_acct.last_yr06 += Convert.ToDouble(dr_old_acct["last_yr06"]);
//                        new_dsn_acct.last_yr07 += Convert.ToDouble(dr_old_acct["last_yr07"]);
//                        new_dsn_acct.last_yr08 += Convert.ToDouble(dr_old_acct["last_yr08"]);
//                        new_dsn_acct.last_yr09 += Convert.ToDouble(dr_old_acct["last_yr09"]);
//                        new_dsn_acct.last_yr10 += Convert.ToDouble(dr_old_acct["last_yr10"]);
//                        new_dsn_acct.last_yr11 += Convert.ToDouble(dr_old_acct["last_yr11"]);
//                        new_dsn_acct.last_yr12 += Convert.ToDouble(dr_old_acct["last_yr12"]);
//                        new_dsn_acct.last_yr13 += Convert.ToDouble(dr_old_acct["last_yr13"]);

//                        new_dsn_acct.next_yr_bgt_01 += Convert.ToDouble(dr_old_acct["bdgt_next_yr01"]);
//                        new_dsn_acct.next_yr_bgt_02 += Convert.ToDouble(dr_old_acct["bdgt_next_yr02"]);
//                        new_dsn_acct.next_yr_bgt_03 += Convert.ToDouble(dr_old_acct["bdgt_next_yr03"]);
//                        new_dsn_acct.next_yr_bgt_04 += Convert.ToDouble(dr_old_acct["bdgt_next_yr04"]);
//                        new_dsn_acct.next_yr_bgt_05 += Convert.ToDouble(dr_old_acct["bdgt_next_yr05"]);
//                        new_dsn_acct.next_yr_bgt_06 += Convert.ToDouble(dr_old_acct["bdgt_next_yr06"]);
//                        new_dsn_acct.next_yr_bgt_07 += Convert.ToDouble(dr_old_acct["bdgt_next_yr07"]);
//                        new_dsn_acct.next_yr_bgt_08 += Convert.ToDouble(dr_old_acct["bdgt_next_yr08"]);
//                        new_dsn_acct.next_yr_bgt_09 += Convert.ToDouble(dr_old_acct["bdgt_next_yr09"]);
//                        new_dsn_acct.next_yr_bgt_10 += Convert.ToDouble(dr_old_acct["bdgt_next_yr10"]);
//                        new_dsn_acct.next_yr_bgt_11 += Convert.ToDouble(dr_old_acct["bdgt_next_yr11"]);
//                        new_dsn_acct.next_yr_bgt_12 += Convert.ToDouble(dr_old_acct["bdgt_next_yr12"]);
//                        new_dsn_acct.next_yr_bgt_13 += Convert.ToDouble(dr_old_acct["bdgt_next_yr13"]);

//                        new_dsn_acct.last_yr_bgt_01 += Convert.ToDouble(dr_old_acct["bdgt_last_yr01"]);
//                        new_dsn_acct.last_yr_bgt_02 += Convert.ToDouble(dr_old_acct["bdgt_last_yr02"]);
//                        new_dsn_acct.last_yr_bgt_03 += Convert.ToDouble(dr_old_acct["bdgt_last_yr03"]);
//                        new_dsn_acct.last_yr_bgt_04 += Convert.ToDouble(dr_old_acct["bdgt_last_yr04"]);
//                        new_dsn_acct.last_yr_bgt_05 += Convert.ToDouble(dr_old_acct["bdgt_last_yr05"]);
//                        new_dsn_acct.last_yr_bgt_06 += Convert.ToDouble(dr_old_acct["bdgt_last_yr06"]);
//                        new_dsn_acct.last_yr_bgt_07 += Convert.ToDouble(dr_old_acct["bdgt_last_yr07"]);
//                        new_dsn_acct.last_yr_bgt_08 += Convert.ToDouble(dr_old_acct["bdgt_last_yr08"]);
//                        new_dsn_acct.last_yr_bgt_09 += Convert.ToDouble(dr_old_acct["bdgt_last_yr09"]);
//                        new_dsn_acct.last_yr_bgt_10 += Convert.ToDouble(dr_old_acct["bdgt_last_yr10"]);
//                        new_dsn_acct.last_yr_bgt_11 += Convert.ToDouble(dr_old_acct["bdgt_last_yr11"]);
//                        new_dsn_acct.last_yr_bgt_12 += Convert.ToDouble(dr_old_acct["bdgt_last_yr12"]);
//                        new_dsn_acct.last_yr_bgt_13 += Convert.ToDouble(dr_old_acct["bdgt_last_yr13"]);

//                        new_dsn_acct.this_yr01 += Convert.ToDouble(dr_old_acct["this_yr01"]);
//                        new_dsn_acct.this_yr02 += Convert.ToDouble(dr_old_acct["this_yr02"]);
//                        new_dsn_acct.this_yr03 += Convert.ToDouble(dr_old_acct["this_yr03"]);
//                        new_dsn_acct.this_yr04 += Convert.ToDouble(dr_old_acct["this_yr04"]);
//                        new_dsn_acct.this_yr05 += Convert.ToDouble(dr_old_acct["this_yr05"]);
//                        new_dsn_acct.this_yr06 += Convert.ToDouble(dr_old_acct["this_yr06"]);
//                        new_dsn_acct.this_yr07 += Convert.ToDouble(dr_old_acct["this_yr07"]);
//                        new_dsn_acct.this_yr08 += Convert.ToDouble(dr_old_acct["this_yr08"]);
//                        new_dsn_acct.this_yr09 += Convert.ToDouble(dr_old_acct["this_yr09"]);
//                        new_dsn_acct.this_yr10 += Convert.ToDouble(dr_old_acct["this_yr10"]);
//                        new_dsn_acct.this_yr11 += Convert.ToDouble(dr_old_acct["this_yr11"]);
//                        new_dsn_acct.this_yr12 += Convert.ToDouble(dr_old_acct["this_yr12"]);
//                        new_dsn_acct.this_yr13 += Convert.ToDouble(dr_old_acct["this_yr13"]);

//                        new_dsn_acct.this_yr_bgt_01 += Convert.ToDouble(dr_old_acct["bdgt_this_yr01"]);
//                        new_dsn_acct.this_yr_bgt_02 += Convert.ToDouble(dr_old_acct["bdgt_this_yr02"]);
//                        new_dsn_acct.this_yr_bgt_03 += Convert.ToDouble(dr_old_acct["bdgt_this_yr03"]);
//                        new_dsn_acct.this_yr_bgt_04 += Convert.ToDouble(dr_old_acct["bdgt_this_yr04"]);
//                        new_dsn_acct.this_yr_bgt_05 += Convert.ToDouble(dr_old_acct["bdgt_this_yr05"]);
//                        new_dsn_acct.this_yr_bgt_06 += Convert.ToDouble(dr_old_acct["bdgt_this_yr06"]);
//                        new_dsn_acct.this_yr_bgt_07 += Convert.ToDouble(dr_old_acct["bdgt_this_yr07"]);
//                        new_dsn_acct.this_yr_bgt_08 += Convert.ToDouble(dr_old_acct["bdgt_this_yr08"]);
//                        new_dsn_acct.this_yr_bgt_09 += Convert.ToDouble(dr_old_acct["bdgt_this_yr09"]);
//                        new_dsn_acct.this_yr_bgt_10 += Convert.ToDouble(dr_old_acct["bdgt_this_yr10"]);
//                        new_dsn_acct.this_yr_bgt_11 += Convert.ToDouble(dr_old_acct["bdgt_this_yr11"]);
//                        new_dsn_acct.this_yr_bgt_12 += Convert.ToDouble(dr_old_acct["bdgt_this_yr12"]);
//                        new_dsn_acct.this_yr_bgt_13 += Convert.ToDouble(dr_old_acct["bdgt_this_yr13"]);
//                        new_dsn_acct.open_bal += Convert.ToDouble(dr_old_acct["open_bal"]);
//                        new_dsn_acct.date_modified = DateTime.Now;
//                        }
//                    }

//                new_dsn_acct.save(false, BVconn,true);

//                #endregion

                
//                foreach (DataRow bu_ in new_te.business_units.Rows)
//                    {
//                    NeBusinessUnit bu_temp = new NeBusinessUnit(bu_["id"]);
//                    if (bu_temp.id != 0)
//                        {
//                        new_dsn_acct = new NeGLChart_of_accounts(Convert.ToInt32(bu_["id"]), gl_id);
//                        NeGLChart_of_accounts.clear_consol_acct(new_dsn_acct.id, true, BVconn);
//                        new_dsn_acct = new NeGLChart_of_accounts(Convert.ToInt32(bu_["id"]), gl_id);
//                        DataTable dt_old_accts = (DataTable) Session["old_dsn_" + bu_temp.old_dsn];

//                        if (dt_old_accts.Select("acct_no='" + acct_no + "' and Division='" +
//                                                _tools.getSQL_string(
//                                                    "Select division_bvdept from `division-y` where division_id = " +
//                                                    bu_temp.old_div, null) +
//                                                "'").Length > 0)
//                            {

//                            DataRow dr_old_acct = dt_old_accts
//                                .Select("acct_no='" + acct_no + "' and Division='" +
//                                        _tools.getSQL_string(
//                                            "Select division_bvdept from `division-y` where division_id = " + bu_temp.old_div,
//                                            null) +
//                                        "'")[0];
                                

//                            new_dsn_acct.next_yr01 += Convert.ToDouble(dr_old_acct["next_yr01"]);
//                            new_dsn_acct.next_yr02 += Convert.ToDouble(dr_old_acct["next_yr02"]);
//                            new_dsn_acct.next_yr03 += Convert.ToDouble(dr_old_acct["next_yr03"]);
//                            new_dsn_acct.next_yr04 += Convert.ToDouble(dr_old_acct["next_yr04"]);
//                            new_dsn_acct.next_yr05 += Convert.ToDouble(dr_old_acct["next_yr05"]);
//                            new_dsn_acct.next_yr06 += Convert.ToDouble(dr_old_acct["next_yr06"]);
//                            new_dsn_acct.next_yr07 += Convert.ToDouble(dr_old_acct["next_yr07"]);
//                            new_dsn_acct.next_yr08 += Convert.ToDouble(dr_old_acct["next_yr08"]);
//                            new_dsn_acct.next_yr09 += Convert.ToDouble(dr_old_acct["next_yr09"]);
//                            new_dsn_acct.next_yr10 += Convert.ToDouble(dr_old_acct["next_yr10"]);
//                            new_dsn_acct.next_yr11 += Convert.ToDouble(dr_old_acct["next_yr11"]);
//                            new_dsn_acct.next_yr12 += Convert.ToDouble(dr_old_acct["next_yr12"]);
//                            new_dsn_acct.next_yr13 += Convert.ToDouble(dr_old_acct["next_yr13"]);

//                            new_dsn_acct.last_yr01 += Convert.ToDouble(dr_old_acct["last_yr01"]);
//                            new_dsn_acct.last_yr02 += Convert.ToDouble(dr_old_acct["last_yr02"]);
//                            new_dsn_acct.last_yr03 += Convert.ToDouble(dr_old_acct["last_yr03"]);
//                            new_dsn_acct.last_yr04 += Convert.ToDouble(dr_old_acct["last_yr04"]);
//                            new_dsn_acct.last_yr05 += Convert.ToDouble(dr_old_acct["last_yr05"]);
//                            new_dsn_acct.last_yr06 += Convert.ToDouble(dr_old_acct["last_yr06"]);
//                            new_dsn_acct.last_yr07 += Convert.ToDouble(dr_old_acct["last_yr07"]);
//                            new_dsn_acct.last_yr08 += Convert.ToDouble(dr_old_acct["last_yr08"]);
//                            new_dsn_acct.last_yr09 += Convert.ToDouble(dr_old_acct["last_yr09"]);
//                            new_dsn_acct.last_yr10 += Convert.ToDouble(dr_old_acct["last_yr10"]);
//                            new_dsn_acct.last_yr11 += Convert.ToDouble(dr_old_acct["last_yr11"]);
//                            new_dsn_acct.last_yr12 += Convert.ToDouble(dr_old_acct["last_yr12"]);
//                            new_dsn_acct.last_yr13 += Convert.ToDouble(dr_old_acct["last_yr13"]);

//                            new_dsn_acct.next_yr_bgt_01 += Convert.ToDouble(dr_old_acct["bdgt_next_yr01"]);
//                            new_dsn_acct.next_yr_bgt_02 += Convert.ToDouble(dr_old_acct["bdgt_next_yr02"]);
//                            new_dsn_acct.next_yr_bgt_03 += Convert.ToDouble(dr_old_acct["bdgt_next_yr03"]);
//                            new_dsn_acct.next_yr_bgt_04 += Convert.ToDouble(dr_old_acct["bdgt_next_yr04"]);
//                            new_dsn_acct.next_yr_bgt_05 += Convert.ToDouble(dr_old_acct["bdgt_next_yr05"]);
//                            new_dsn_acct.next_yr_bgt_06 += Convert.ToDouble(dr_old_acct["bdgt_next_yr06"]);
//                            new_dsn_acct.next_yr_bgt_07 += Convert.ToDouble(dr_old_acct["bdgt_next_yr07"]);
//                            new_dsn_acct.next_yr_bgt_08 += Convert.ToDouble(dr_old_acct["bdgt_next_yr08"]);
//                            new_dsn_acct.next_yr_bgt_09 += Convert.ToDouble(dr_old_acct["bdgt_next_yr09"]);
//                            new_dsn_acct.next_yr_bgt_10 += Convert.ToDouble(dr_old_acct["bdgt_next_yr10"]);
//                            new_dsn_acct.next_yr_bgt_11 += Convert.ToDouble(dr_old_acct["bdgt_next_yr11"]);
//                            new_dsn_acct.next_yr_bgt_12 += Convert.ToDouble(dr_old_acct["bdgt_next_yr12"]);
//                            new_dsn_acct.next_yr_bgt_13 += Convert.ToDouble(dr_old_acct["bdgt_next_yr13"]);

//                            new_dsn_acct.last_yr_bgt_01 += Convert.ToDouble(dr_old_acct["bdgt_last_yr01"]);
//                            new_dsn_acct.last_yr_bgt_02 += Convert.ToDouble(dr_old_acct["bdgt_last_yr02"]);
//                            new_dsn_acct.last_yr_bgt_03 += Convert.ToDouble(dr_old_acct["bdgt_last_yr03"]);
//                            new_dsn_acct.last_yr_bgt_04 += Convert.ToDouble(dr_old_acct["bdgt_last_yr04"]);
//                            new_dsn_acct.last_yr_bgt_05 += Convert.ToDouble(dr_old_acct["bdgt_last_yr05"]);
//                            new_dsn_acct.last_yr_bgt_06 += Convert.ToDouble(dr_old_acct["bdgt_last_yr06"]);
//                            new_dsn_acct.last_yr_bgt_07 += Convert.ToDouble(dr_old_acct["bdgt_last_yr07"]);
//                            new_dsn_acct.last_yr_bgt_08 += Convert.ToDouble(dr_old_acct["bdgt_last_yr08"]);
//                            new_dsn_acct.last_yr_bgt_09 += Convert.ToDouble(dr_old_acct["bdgt_last_yr09"]);
//                            new_dsn_acct.last_yr_bgt_10 += Convert.ToDouble(dr_old_acct["bdgt_last_yr10"]);
//                            new_dsn_acct.last_yr_bgt_11 += Convert.ToDouble(dr_old_acct["bdgt_last_yr11"]);
//                            new_dsn_acct.last_yr_bgt_12 += Convert.ToDouble(dr_old_acct["bdgt_last_yr12"]);
//                            new_dsn_acct.last_yr_bgt_13 += Convert.ToDouble(dr_old_acct["bdgt_last_yr13"]);

//                            new_dsn_acct.this_yr01 += Convert.ToDouble(dr_old_acct["this_yr01"]);
//                            new_dsn_acct.this_yr02 += Convert.ToDouble(dr_old_acct["this_yr02"]);
//                            new_dsn_acct.this_yr03 += Convert.ToDouble(dr_old_acct["this_yr03"]);
//                            new_dsn_acct.this_yr04 += Convert.ToDouble(dr_old_acct["this_yr04"]);
//                            new_dsn_acct.this_yr05 += Convert.ToDouble(dr_old_acct["this_yr05"]);
//                            new_dsn_acct.this_yr06 += Convert.ToDouble(dr_old_acct["this_yr06"]);
//                            new_dsn_acct.this_yr07 += Convert.ToDouble(dr_old_acct["this_yr07"]);
//                            new_dsn_acct.this_yr08 += Convert.ToDouble(dr_old_acct["this_yr08"]);
//                            new_dsn_acct.this_yr09 += Convert.ToDouble(dr_old_acct["this_yr09"]);
//                            new_dsn_acct.this_yr10 += Convert.ToDouble(dr_old_acct["this_yr10"]);
//                            new_dsn_acct.this_yr11 += Convert.ToDouble(dr_old_acct["this_yr11"]);
//                            new_dsn_acct.this_yr12 += Convert.ToDouble(dr_old_acct["this_yr12"]);
//                            new_dsn_acct.this_yr13 += Convert.ToDouble(dr_old_acct["this_yr13"]);

//                            new_dsn_acct.this_yr_bgt_01 += Convert.ToDouble(dr_old_acct["bdgt_this_yr01"]);
//                            new_dsn_acct.this_yr_bgt_02 += Convert.ToDouble(dr_old_acct["bdgt_this_yr02"]);
//                            new_dsn_acct.this_yr_bgt_03 += Convert.ToDouble(dr_old_acct["bdgt_this_yr03"]);
//                            new_dsn_acct.this_yr_bgt_04 += Convert.ToDouble(dr_old_acct["bdgt_this_yr04"]);
//                            new_dsn_acct.this_yr_bgt_05 += Convert.ToDouble(dr_old_acct["bdgt_this_yr05"]);
//                            new_dsn_acct.this_yr_bgt_06 += Convert.ToDouble(dr_old_acct["bdgt_this_yr06"]);
//                            new_dsn_acct.this_yr_bgt_07 += Convert.ToDouble(dr_old_acct["bdgt_this_yr07"]);
//                            new_dsn_acct.this_yr_bgt_08 += Convert.ToDouble(dr_old_acct["bdgt_this_yr08"]);
//                            new_dsn_acct.this_yr_bgt_09 += Convert.ToDouble(dr_old_acct["bdgt_this_yr09"]);
//                            new_dsn_acct.this_yr_bgt_10 += Convert.ToDouble(dr_old_acct["bdgt_this_yr10"]);
//                            new_dsn_acct.this_yr_bgt_11 += Convert.ToDouble(dr_old_acct["bdgt_this_yr11"]);
//                            new_dsn_acct.this_yr_bgt_12 += Convert.ToDouble(dr_old_acct["bdgt_this_yr12"]);
//                            new_dsn_acct.this_yr_bgt_13 += Convert.ToDouble(dr_old_acct["bdgt_this_yr13"]);
//                            new_dsn_acct.open_bal += Convert.ToDouble(dr_old_acct["open_bal"]);
//                            new_dsn_acct.date_modified = DateTime.Now;
//                            new_dsn_acct.save(false, BVconn,false);
//                        }

//                        }
//                    }
//                }
//            foreach (DataRow dr_old_dsns in dt_old_dsns.Rows)
//                {
//                Session["old_dsn_" + dr_old_dsns["old_dsn"]] = null;
//                }
//            }
//        }


//    /* THis function is for bring DATA from a group of DSNs into ONE new BV.  
//     * Currently, all the save_to_bv flags in this function are set to false, so its only going to create a merged structure in mysql
//     * Also, it uses the OLD division-y table, which could be replaced by the BV division in the business unit table
//     */
//    protected void go_merge_data_from_dsn(string old_dsn, string old_div)
//    {
//        NeTaxEntity new_te = new NeTaxEntity(hdn_tax_ent_it.Value);
//    using (var BVconn = BVDB.connect(new_te.DSN))
//        {
//        int bu_id = 0;
//        //TODO We cant be using division-y
//        int div = _tools.getSQL_int(@"Select division_id from `division-y`  where division_bvdept=@v0 limit 1 ",
//            new object[] {old_div});
//        bool add = false;
//        bu_id = _tools.getSQL_int(
//            @"Select ifnull((select id from business_unit  where old_dsn =@v0 and old_div =@v1  limit 1),0) ",
//            new object[] {old_dsn, div});
//        if (bu_id == 0)
//            {
//            add = true;
//            bu_id = _tools.getSQL_int(
//                @"Select ifnull((select id from business_unit  where tax_entity_id =@v0 and old_div =@v1  order by id desc limit 1),0)",
//                new object[] {hdn_tax_ent_it.Value, div});
//            }
//        if (bu_id == 0)
//            {
//            add = true;
//            bu_id = _tools.getSQL_int(
//                @"Select ifnull((select id from business_unit  where tax_entity_id =@v0 and old_div = 2 order by id desc limit 1),0)",
//                new object[] {hdn_tax_ent_it.Value});
//            }
//        if (bu_id == 0)
//            {
//            add = true;
//            bu_id = _tools.getSQL_int(
//                @"Select ifnull((select id from business_unit  where tax_entity_id =@v0 order by id desc limit 1),0)",
//                new object[] {hdn_tax_ent_it.Value});
//            }

//        if (bu_id == 0)
//            {
//            throw new Exception("No business ID found");
//            }

//        NeBusinessUnit bu = new NeBusinessUnit(bu_id);

//        // loop through account list gridview

//        Session["old_dsn_" + old_dsn] =
//            _tools.getSQL_datatable(@"Select * from gl_chart_of_accounts  where division =?", old_dsn,
//                new object[] {old_div});
//        DataTable dt_old_accts = (DataTable) Session["old_dsn_" + old_dsn];
//        if (dt_old_accts != null && dt_old_accts.Rows.Count > 0)
//            {
//            for (int i = 0; i < gv.VisibleRowCount; i++) // loop through every account
//                {
//                string acct_no = gv.GetRowValues(i, "account_no").ToString();
               
//                    int gl_id = Convert.ToInt32(gv.GetRowValues(i, "id"));
//                    if (dt_old_accts.Select("acct_no='" + acct_no + "'").Length > 0)
//                        {
//                        DataRow dr_old_acct = dt_old_accts.Select("acct_no='" + acct_no + "'").CopyToDataTable()
//                            .Rows[0];

//                        // for each account, find values for each div and populate consol bv while totally up for the 000
//                        NeGLChart_of_accounts new_dsn_acct = new NeGLChart_of_accounts(bu.id32, gl_id);

//                        if (add)
//                            {
//                            new_dsn_acct.next_yr01 += Convert.ToDouble(dr_old_acct["next_yr01"]);
//                            new_dsn_acct.next_yr02 += Convert.ToDouble(dr_old_acct["next_yr02"]);
//                            new_dsn_acct.next_yr03 += Convert.ToDouble(dr_old_acct["next_yr03"]);
//                            new_dsn_acct.next_yr04 += Convert.ToDouble(dr_old_acct["next_yr04"]);
//                            new_dsn_acct.next_yr05 += Convert.ToDouble(dr_old_acct["next_yr05"]);
//                            new_dsn_acct.next_yr06 += Convert.ToDouble(dr_old_acct["next_yr06"]);
//                            new_dsn_acct.next_yr07 += Convert.ToDouble(dr_old_acct["next_yr07"]);
//                            new_dsn_acct.next_yr08 += Convert.ToDouble(dr_old_acct["next_yr08"]);
//                            new_dsn_acct.next_yr09 += Convert.ToDouble(dr_old_acct["next_yr09"]);
//                            new_dsn_acct.next_yr10 += Convert.ToDouble(dr_old_acct["next_yr10"]);
//                            new_dsn_acct.next_yr11 += Convert.ToDouble(dr_old_acct["next_yr11"]);
//                            new_dsn_acct.next_yr12 += Convert.ToDouble(dr_old_acct["next_yr12"]);
//                            new_dsn_acct.next_yr13 += Convert.ToDouble(dr_old_acct["next_yr13"]);

//                            new_dsn_acct.last_yr01 += Convert.ToDouble(dr_old_acct["last_yr01"]);
//                            new_dsn_acct.last_yr02 += Convert.ToDouble(dr_old_acct["last_yr02"]);
//                            new_dsn_acct.last_yr03 += Convert.ToDouble(dr_old_acct["last_yr03"]);
//                            new_dsn_acct.last_yr04 += Convert.ToDouble(dr_old_acct["last_yr04"]);
//                            new_dsn_acct.last_yr05 += Convert.ToDouble(dr_old_acct["last_yr05"]);
//                            new_dsn_acct.last_yr06 += Convert.ToDouble(dr_old_acct["last_yr06"]);
//                            new_dsn_acct.last_yr07 += Convert.ToDouble(dr_old_acct["last_yr07"]);
//                            new_dsn_acct.last_yr08 += Convert.ToDouble(dr_old_acct["last_yr08"]);
//                            new_dsn_acct.last_yr09 += Convert.ToDouble(dr_old_acct["last_yr09"]);
//                            new_dsn_acct.last_yr10 += Convert.ToDouble(dr_old_acct["last_yr10"]);
//                            new_dsn_acct.last_yr11 += Convert.ToDouble(dr_old_acct["last_yr11"]);
//                            new_dsn_acct.last_yr12 += Convert.ToDouble(dr_old_acct["last_yr12"]);
//                            new_dsn_acct.last_yr13 += Convert.ToDouble(dr_old_acct["last_yr13"]);

//                            new_dsn_acct.next_yr_bgt_01 += Convert.ToDouble(dr_old_acct["bdgt_next_yr01"]);
//                            new_dsn_acct.next_yr_bgt_02 += Convert.ToDouble(dr_old_acct["bdgt_next_yr02"]);
//                            new_dsn_acct.next_yr_bgt_03 += Convert.ToDouble(dr_old_acct["bdgt_next_yr03"]);
//                            new_dsn_acct.next_yr_bgt_04 += Convert.ToDouble(dr_old_acct["bdgt_next_yr04"]);
//                            new_dsn_acct.next_yr_bgt_05 += Convert.ToDouble(dr_old_acct["bdgt_next_yr05"]);
//                            new_dsn_acct.next_yr_bgt_06 += Convert.ToDouble(dr_old_acct["bdgt_next_yr06"]);
//                            new_dsn_acct.next_yr_bgt_07 += Convert.ToDouble(dr_old_acct["bdgt_next_yr07"]);
//                            new_dsn_acct.next_yr_bgt_08 += Convert.ToDouble(dr_old_acct["bdgt_next_yr08"]);
//                            new_dsn_acct.next_yr_bgt_09 += Convert.ToDouble(dr_old_acct["bdgt_next_yr09"]);
//                            new_dsn_acct.next_yr_bgt_10 += Convert.ToDouble(dr_old_acct["bdgt_next_yr10"]);
//                            new_dsn_acct.next_yr_bgt_11 += Convert.ToDouble(dr_old_acct["bdgt_next_yr11"]);
//                            new_dsn_acct.next_yr_bgt_12 += Convert.ToDouble(dr_old_acct["bdgt_next_yr12"]);
//                            new_dsn_acct.next_yr_bgt_13 += Convert.ToDouble(dr_old_acct["bdgt_next_yr13"]);

//                            new_dsn_acct.last_yr_bgt_01 += Convert.ToDouble(dr_old_acct["bdgt_last_yr01"]);
//                            new_dsn_acct.last_yr_bgt_02 += Convert.ToDouble(dr_old_acct["bdgt_last_yr02"]);
//                            new_dsn_acct.last_yr_bgt_03 += Convert.ToDouble(dr_old_acct["bdgt_last_yr03"]);
//                            new_dsn_acct.last_yr_bgt_04 += Convert.ToDouble(dr_old_acct["bdgt_last_yr04"]);
//                            new_dsn_acct.last_yr_bgt_05 += Convert.ToDouble(dr_old_acct["bdgt_last_yr05"]);
//                            new_dsn_acct.last_yr_bgt_06 += Convert.ToDouble(dr_old_acct["bdgt_last_yr06"]);
//                            new_dsn_acct.last_yr_bgt_07 += Convert.ToDouble(dr_old_acct["bdgt_last_yr07"]);
//                            new_dsn_acct.last_yr_bgt_08 += Convert.ToDouble(dr_old_acct["bdgt_last_yr08"]);
//                            new_dsn_acct.last_yr_bgt_09 += Convert.ToDouble(dr_old_acct["bdgt_last_yr09"]);
//                            new_dsn_acct.last_yr_bgt_10 += Convert.ToDouble(dr_old_acct["bdgt_last_yr10"]);
//                            new_dsn_acct.last_yr_bgt_11 += Convert.ToDouble(dr_old_acct["bdgt_last_yr11"]);
//                            new_dsn_acct.last_yr_bgt_12 += Convert.ToDouble(dr_old_acct["bdgt_last_yr12"]);
//                            new_dsn_acct.last_yr_bgt_13 += Convert.ToDouble(dr_old_acct["bdgt_last_yr13"]);

//                            new_dsn_acct.this_yr01 += Convert.ToDouble(dr_old_acct["this_yr01"]);
//                            new_dsn_acct.this_yr02 += Convert.ToDouble(dr_old_acct["this_yr02"]);
//                            new_dsn_acct.this_yr03 += Convert.ToDouble(dr_old_acct["this_yr03"]);
//                            new_dsn_acct.this_yr04 += Convert.ToDouble(dr_old_acct["this_yr04"]);
//                            new_dsn_acct.this_yr05 += Convert.ToDouble(dr_old_acct["this_yr05"]);
//                            new_dsn_acct.this_yr06 += Convert.ToDouble(dr_old_acct["this_yr06"]);
//                            new_dsn_acct.this_yr07 += Convert.ToDouble(dr_old_acct["this_yr07"]);
//                            new_dsn_acct.this_yr08 += Convert.ToDouble(dr_old_acct["this_yr08"]);
//                            new_dsn_acct.this_yr09 += Convert.ToDouble(dr_old_acct["this_yr09"]);
//                            new_dsn_acct.this_yr10 += Convert.ToDouble(dr_old_acct["this_yr10"]);
//                            new_dsn_acct.this_yr11 += Convert.ToDouble(dr_old_acct["this_yr11"]);
//                            new_dsn_acct.this_yr12 += Convert.ToDouble(dr_old_acct["this_yr12"]);
//                            new_dsn_acct.this_yr13 += Convert.ToDouble(dr_old_acct["this_yr13"]);

//                            new_dsn_acct.this_yr_bgt_01 += Convert.ToDouble(dr_old_acct["bdgt_this_yr01"]);
//                            new_dsn_acct.this_yr_bgt_02 += Convert.ToDouble(dr_old_acct["bdgt_this_yr02"]);
//                            new_dsn_acct.this_yr_bgt_03 += Convert.ToDouble(dr_old_acct["bdgt_this_yr03"]);
//                            new_dsn_acct.this_yr_bgt_04 += Convert.ToDouble(dr_old_acct["bdgt_this_yr04"]);
//                            new_dsn_acct.this_yr_bgt_05 += Convert.ToDouble(dr_old_acct["bdgt_this_yr05"]);
//                            new_dsn_acct.this_yr_bgt_06 += Convert.ToDouble(dr_old_acct["bdgt_this_yr06"]);
//                            new_dsn_acct.this_yr_bgt_07 += Convert.ToDouble(dr_old_acct["bdgt_this_yr07"]);
//                            new_dsn_acct.this_yr_bgt_08 += Convert.ToDouble(dr_old_acct["bdgt_this_yr08"]);
//                            new_dsn_acct.this_yr_bgt_09 += Convert.ToDouble(dr_old_acct["bdgt_this_yr09"]);
//                            new_dsn_acct.this_yr_bgt_10 += Convert.ToDouble(dr_old_acct["bdgt_this_yr10"]);
//                            new_dsn_acct.this_yr_bgt_11 += Convert.ToDouble(dr_old_acct["bdgt_this_yr11"]);
//                            new_dsn_acct.this_yr_bgt_12 += Convert.ToDouble(dr_old_acct["bdgt_this_yr12"]);
//                            new_dsn_acct.this_yr_bgt_13 += Convert.ToDouble(dr_old_acct["bdgt_this_yr13"]);
//                            new_dsn_acct.open_bal += Convert.ToDouble(dr_old_acct["open_bal"]);
//                            new_dsn_acct.date_modified = DateTime.Now;
//                            }
//                        else
//                            {
//                            new_dsn_acct.next_yr01 = Convert.ToDouble(dr_old_acct["next_yr01"]);
//                            new_dsn_acct.next_yr02 = Convert.ToDouble(dr_old_acct["next_yr02"]);
//                            new_dsn_acct.next_yr03 = Convert.ToDouble(dr_old_acct["next_yr03"]);
//                            new_dsn_acct.next_yr04 = Convert.ToDouble(dr_old_acct["next_yr04"]);
//                            new_dsn_acct.next_yr05 = Convert.ToDouble(dr_old_acct["next_yr05"]);
//                            new_dsn_acct.next_yr06 = Convert.ToDouble(dr_old_acct["next_yr06"]);
//                            new_dsn_acct.next_yr07 = Convert.ToDouble(dr_old_acct["next_yr07"]);
//                            new_dsn_acct.next_yr08 = Convert.ToDouble(dr_old_acct["next_yr08"]);
//                            new_dsn_acct.next_yr09 = Convert.ToDouble(dr_old_acct["next_yr09"]);
//                            new_dsn_acct.next_yr10 = Convert.ToDouble(dr_old_acct["next_yr10"]);
//                            new_dsn_acct.next_yr11 = Convert.ToDouble(dr_old_acct["next_yr11"]);
//                            new_dsn_acct.next_yr12 = Convert.ToDouble(dr_old_acct["next_yr12"]);
//                            new_dsn_acct.next_yr13 = Convert.ToDouble(dr_old_acct["next_yr13"]);

//                            new_dsn_acct.last_yr01 = Convert.ToDouble(dr_old_acct["last_yr01"]);
//                            new_dsn_acct.last_yr02 = Convert.ToDouble(dr_old_acct["last_yr02"]);
//                            new_dsn_acct.last_yr03 = Convert.ToDouble(dr_old_acct["last_yr03"]);
//                            new_dsn_acct.last_yr04 = Convert.ToDouble(dr_old_acct["last_yr04"]);
//                            new_dsn_acct.last_yr05 = Convert.ToDouble(dr_old_acct["last_yr05"]);
//                            new_dsn_acct.last_yr06 = Convert.ToDouble(dr_old_acct["last_yr06"]);
//                            new_dsn_acct.last_yr07 = Convert.ToDouble(dr_old_acct["last_yr07"]);
//                            new_dsn_acct.last_yr08 = Convert.ToDouble(dr_old_acct["last_yr08"]);
//                            new_dsn_acct.last_yr09 = Convert.ToDouble(dr_old_acct["last_yr09"]);
//                            new_dsn_acct.last_yr10 = Convert.ToDouble(dr_old_acct["last_yr10"]);
//                            new_dsn_acct.last_yr11 = Convert.ToDouble(dr_old_acct["last_yr11"]);
//                            new_dsn_acct.last_yr12 = Convert.ToDouble(dr_old_acct["last_yr12"]);
//                            new_dsn_acct.last_yr13 = Convert.ToDouble(dr_old_acct["last_yr13"]);

//                            new_dsn_acct.next_yr_bgt_01 = Convert.ToDouble(dr_old_acct["bdgt_next_yr01"]);
//                            new_dsn_acct.next_yr_bgt_02 = Convert.ToDouble(dr_old_acct["bdgt_next_yr02"]);
//                            new_dsn_acct.next_yr_bgt_03 = Convert.ToDouble(dr_old_acct["bdgt_next_yr03"]);
//                            new_dsn_acct.next_yr_bgt_04 = Convert.ToDouble(dr_old_acct["bdgt_next_yr04"]);
//                            new_dsn_acct.next_yr_bgt_05 = Convert.ToDouble(dr_old_acct["bdgt_next_yr05"]);
//                            new_dsn_acct.next_yr_bgt_06 = Convert.ToDouble(dr_old_acct["bdgt_next_yr06"]);
//                            new_dsn_acct.next_yr_bgt_07 = Convert.ToDouble(dr_old_acct["bdgt_next_yr07"]);
//                            new_dsn_acct.next_yr_bgt_08 = Convert.ToDouble(dr_old_acct["bdgt_next_yr08"]);
//                            new_dsn_acct.next_yr_bgt_09 = Convert.ToDouble(dr_old_acct["bdgt_next_yr09"]);
//                            new_dsn_acct.next_yr_bgt_10 = Convert.ToDouble(dr_old_acct["bdgt_next_yr10"]);
//                            new_dsn_acct.next_yr_bgt_11 = Convert.ToDouble(dr_old_acct["bdgt_next_yr11"]);
//                            new_dsn_acct.next_yr_bgt_12 = Convert.ToDouble(dr_old_acct["bdgt_next_yr12"]);
//                            new_dsn_acct.next_yr_bgt_13 = Convert.ToDouble(dr_old_acct["bdgt_next_yr13"]);

//                            new_dsn_acct.last_yr_bgt_01 = Convert.ToDouble(dr_old_acct["bdgt_last_yr01"]);
//                            new_dsn_acct.last_yr_bgt_02 = Convert.ToDouble(dr_old_acct["bdgt_last_yr02"]);
//                            new_dsn_acct.last_yr_bgt_03 = Convert.ToDouble(dr_old_acct["bdgt_last_yr03"]);
//                            new_dsn_acct.last_yr_bgt_04 = Convert.ToDouble(dr_old_acct["bdgt_last_yr04"]);
//                            new_dsn_acct.last_yr_bgt_05 = Convert.ToDouble(dr_old_acct["bdgt_last_yr05"]);
//                            new_dsn_acct.last_yr_bgt_06 = Convert.ToDouble(dr_old_acct["bdgt_last_yr06"]);
//                            new_dsn_acct.last_yr_bgt_07 = Convert.ToDouble(dr_old_acct["bdgt_last_yr07"]);
//                            new_dsn_acct.last_yr_bgt_08 = Convert.ToDouble(dr_old_acct["bdgt_last_yr08"]);
//                            new_dsn_acct.last_yr_bgt_09 = Convert.ToDouble(dr_old_acct["bdgt_last_yr09"]);
//                            new_dsn_acct.last_yr_bgt_10 = Convert.ToDouble(dr_old_acct["bdgt_last_yr10"]);
//                            new_dsn_acct.last_yr_bgt_11 = Convert.ToDouble(dr_old_acct["bdgt_last_yr11"]);
//                            new_dsn_acct.last_yr_bgt_12 = Convert.ToDouble(dr_old_acct["bdgt_last_yr12"]);
//                            new_dsn_acct.last_yr_bgt_13 = Convert.ToDouble(dr_old_acct["bdgt_last_yr13"]);

//                            new_dsn_acct.this_yr01 = Convert.ToDouble(dr_old_acct["this_yr01"]);
//                            new_dsn_acct.this_yr02 = Convert.ToDouble(dr_old_acct["this_yr02"]);
//                            new_dsn_acct.this_yr03 = Convert.ToDouble(dr_old_acct["this_yr03"]);
//                            new_dsn_acct.this_yr04 = Convert.ToDouble(dr_old_acct["this_yr04"]);
//                            new_dsn_acct.this_yr05 = Convert.ToDouble(dr_old_acct["this_yr05"]);
//                            new_dsn_acct.this_yr06 = Convert.ToDouble(dr_old_acct["this_yr06"]);
//                            new_dsn_acct.this_yr07 = Convert.ToDouble(dr_old_acct["this_yr07"]);
//                            new_dsn_acct.this_yr08 = Convert.ToDouble(dr_old_acct["this_yr08"]);
//                            new_dsn_acct.this_yr09 = Convert.ToDouble(dr_old_acct["this_yr09"]);
//                            new_dsn_acct.this_yr10 = Convert.ToDouble(dr_old_acct["this_yr10"]);
//                            new_dsn_acct.this_yr11 = Convert.ToDouble(dr_old_acct["this_yr11"]);
//                            new_dsn_acct.this_yr12 = Convert.ToDouble(dr_old_acct["this_yr12"]);
//                            new_dsn_acct.this_yr13 = Convert.ToDouble(dr_old_acct["this_yr13"]);

//                            new_dsn_acct.this_yr_bgt_01 = Convert.ToDouble(dr_old_acct["bdgt_this_yr01"]);
//                            new_dsn_acct.this_yr_bgt_02 = Convert.ToDouble(dr_old_acct["bdgt_this_yr02"]);
//                            new_dsn_acct.this_yr_bgt_03 = Convert.ToDouble(dr_old_acct["bdgt_this_yr03"]);
//                            new_dsn_acct.this_yr_bgt_04 = Convert.ToDouble(dr_old_acct["bdgt_this_yr04"]);
//                            new_dsn_acct.this_yr_bgt_05 = Convert.ToDouble(dr_old_acct["bdgt_this_yr05"]);
//                            new_dsn_acct.this_yr_bgt_06 = Convert.ToDouble(dr_old_acct["bdgt_this_yr06"]);
//                            new_dsn_acct.this_yr_bgt_07 = Convert.ToDouble(dr_old_acct["bdgt_this_yr07"]);
//                            new_dsn_acct.this_yr_bgt_08 = Convert.ToDouble(dr_old_acct["bdgt_this_yr08"]);
//                            new_dsn_acct.this_yr_bgt_09 = Convert.ToDouble(dr_old_acct["bdgt_this_yr09"]);
//                            new_dsn_acct.this_yr_bgt_10 = Convert.ToDouble(dr_old_acct["bdgt_this_yr10"]);
//                            new_dsn_acct.this_yr_bgt_11 = Convert.ToDouble(dr_old_acct["bdgt_this_yr11"]);
//                            new_dsn_acct.this_yr_bgt_12 = Convert.ToDouble(dr_old_acct["bdgt_this_yr12"]);
//                            new_dsn_acct.this_yr_bgt_13 = Convert.ToDouble(dr_old_acct["bdgt_this_yr13"]);
//                            new_dsn_acct.open_bal = Convert.ToDouble(dr_old_acct["open_bal"]);
//                            new_dsn_acct.date_modified = DateTime.Now;
//                            }
//                        new_dsn_acct.save(false, BVconn,false);

//                        }
//                    NeGLChart_of_accounts.update_consol_div(gl_id, BVconn);


                 
//                }
//            }

//        }

//    }




//    protected void go_merge_structure_from_dsn(string dsn)
//    {
//        NeTaxEntity new_tax_entity = new NeTaxEntity(hdn_tax_ent_it.Value);
//        DataTable dt = get_all_bv_gls(dsn,false);
//        foreach (DataRow dr in dt.Rows) // loop nthrough each GL
//        {
//            // check and populate groups into gl_group_te fom BV
//            if (_tools.getSQL_int(@"Select count(id) from gl_group_te  where tax_entity_id =@v0 and number=@v1 ", new object[] { hdn_tax_ent_it.Value,dr["gl_group"] }) == 0)
//            {
//                NeGLGroup newgroup = new NeGLGroup();
//                newgroup.id = 0;
//                newgroup.desc = dr["default_desc"].ToString().Trim();
//                newgroup.number = dr["gl_group"].ToString();
//                newgroup.tax_entity_id = Convert.ToInt32(hdn_tax_ent_it.Value);
//                newgroup.type = dr["acct_type"].ToString();
//                newgroup.line_advance = dr["line_advance"].ToString();
//                newgroup.total = Convert.ToInt32(dr["total"]);
//                newgroup.save(true);
//            }
//            // check if GL  exists, if it does, skip it.  else write it into the gl_te
//            NeGL gl = new NeGL();
//            if (_tools.getSQL_int(@"Select count(id) from gl_te  where tax_entity_id =@v0 and account_no=@v1 ", new object[] { hdn_tax_ent_it.Value,dr["acct_no"].ToString().TrimStart('0').Substring(0, 5) }) == 0)
//            {
//                gl.id = 0;
//                gl.account_no = dr["acct_no"].ToString().TrimStart('0').Substring(0, 5);
//                gl.tax_entity_id = Convert.ToInt32(hdn_tax_ent_it.Value);
//                gl.gl_chart_name = dr["name"].ToString();
//                gl.gl_comments = "";
//                gl.GL_Designation = dr["DR_CR_DESIG"].ToString();
//                gl.gl_group_id = new NeGLGroup(Convert.ToInt32(hdn_tax_ent_it.Value), dr["gl_group"].ToString()).id;
//                gl.InitCheque = dr["NEXT_CHEQUE_NO"].ToString();
//                gl.is_active = true;
//                gl.currency_id = dr["CHART_CURRENCY"].ToString() == "CAD" || dr["CHART_CURRENCY"].ToString() == "CDN" ? 2 : dr["CHART_CURRENCY"].ToString().Trim() == "" ? 2 : _tools.getSQL_int(@"select ifnull((Select id from currency  where currency =@v0), 2)", new object[] { dr["CHART_CURRENCY"] });
//                gl.is_sales = dr["sales_acct"].ToString() == "1";
//                gl.is_bank = dr["bank_acct"].ToString() == "1";
//            gl.is_mileage = false;
//                gl.netsuite_gl_id = "";
//                gl.save(true,true);
//            }

//            // loop through all the GL's through all divisions and make sure all the chart of accounts are in bv

//        }
//        foreach (DataRow dr_mysql_gl_list in _tools.getSQL_datatable(@"Select id from gl_te  where tax_entity_id =@v0", new object[] { hdn_tax_ent_it.Value }).Rows)
//        {
//        using (var BVconn = BVDB.connect(new_tax_entity.DSN))
//            {
//            if (Toolbox.doSQL_int(
//                    @"Select count(id) from gl_chart_of_accounts  where business_unit_id =0 and gl_te_id=@v0 ",
//                    new object[] {dr_mysql_gl_list["id"]}) == 0)
//                {
//                NeGLChart_of_accounts new_coa = new NeGLChart_of_accounts();
//                new_coa.id = 0;
//                new_coa.gl_te_id = Convert.ToInt32(dr_mysql_gl_list["id"]);
//                new_coa.business_unit_id = 0;
//                new_coa.save(false,BVconn,true);  // set to NOT write to BV.. because we aren't merging stuff anymore
//                }
//            foreach (DataRow dr_bu in new NeTaxEntity(hdn_tax_ent_it.Value).business_units.Rows)
//                {

//                if (Toolbox.doSQL_int(
//                        @"Select count(id) from gl_chart_of_accounts  where business_unit_id =@v0 and gl_te_id=@v1 ",
//                        new object[] {dr_bu["id"], dr_mysql_gl_list["id"]}) == 0)
//                    {
//                    NeGLChart_of_accounts new_coa = new NeGLChart_of_accounts();
//                    new_coa.id = 0;
//                    new_coa.gl_te_id = Convert.ToInt32(dr_mysql_gl_list["id"]);
//                    new_coa.business_unit_id = Convert.ToInt32(dr_bu["id"]);
//                    new_coa.save(false, BVconn,true);// set to NOT write to BV.. because we aren't merging stuff anymore
//                    }
//                }
//            }
//        }

//    }

//    protected void btn_gomerge_frombv_groupsClick(object sender, EventArgs e)
//    {

//    }

//    protected void gv_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
//    {
//        if (e.VisibleIndex >= 0)

//        {
//            if (e.ButtonType == ColumnCommandButtonType.Delete)
//            {
//                if (dt_glhistory == null || dt_glhistory.Select("acct_no = '" + gv.GetRowValues(e.VisibleIndex, "account_no") + "'").Length > 0)
//                {
//                    e.Enabled = false;
//                }
//            }
//        }
//    }





//    protected void gv_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
//        {
//        if (!gv.IsNewRowEditing)
//            {
//            bool has_history = dt_glhistory.Select("acct_no = '" + gv.GetRowValues(e.VisibleIndex, "account_no") + "'")
//                                   .Length > 0;
//            if (has_history)
//                {
//                if (e.Column.FieldName == "GL_Designation")
//                    {
//                    e.Column.ReadOnly = true;
//                    }
//                else if (e.Column.FieldName == "account_no")
//                    {
//                    e.Column.ReadOnly = true;
//                    }
//                }
//            if ((e.Column.FieldName == "see_on_po_gl_list")||(e.Column.FieldName == "is_mileage"))
//                {
//                if (Convert.ToBoolean(gv.GetRowValues(e.VisibleIndex, "type") == "X"))
//                    {
//                    e.Column.ReadOnly = false;
                    
//                }
//                else
//                    {
                    
//                    e.Column.ReadOnly = true;
//                    }
//                }

//            }
       
//        }

//    protected void gv_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
//    {
//        ASPxComboBox ddl = (ASPxComboBox)gv.FindEditRowCellTemplateControl((GridViewDataColumn)gv.Columns["gl_group_id"], "ddl_group_edit");
//        if (!gv.IsNewRowEditing)
//        {
//            var gl_type = _tools.getSQL_string(@"Select type from gl_group_te  where id =@v0"  , new object[] { gv.GetRowValues(gv.EditingRowVisibleIndex,"gl_group_id") });
//            ddl.DataSource = _tools.getSQL_datatable(@"Select id, concat(number,'-',`desc`) name from gl_group_te  where type =@v0 and tax_entity_id =@v1  and is_active=1 order by number,`desc`", new object[] { gl_type,hdn_tax_ent_it.Value });
//            ddl.Value = gv.GetRowValues(gv.EditingRowVisibleIndex, "gl_group_id");
//        }
//        else
//        {
//            ddl.DataSource = _tools.getSQL_datatable(@"Select id, concat(number,'-',`desc`) name from gl_group_te  where tax_entity_id =@v0 and is_active=1 order by type,number,`desc`", new object[] { hdn_tax_ent_it.Value });
//        }
//        ddl.DataBind();
//    }

    

//    protected void gv_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
//    {
//    ASPxComboBox ddl = (ASPxComboBox)gv.FindEditRowCellTemplateControl((GridViewDataColumn)gv.Columns["gl_group_id"], "ddl_group_edit");
 
//        // Save to mysql
//        NeGL _gl = new NeGL(Convert.ToInt32(e.Keys["id"]));
//    _gl.GL_Designation = e.NewValues["GL_Designation"].ToString();
//    _gl.InitCheque = e.NewValues["InitCheque"] == null ? "0" : e.NewValues["InitCheque"].ToString();
//    _gl.currency_id = Convert.ToInt32(e.NewValues["currency_id"]);
//    _gl.gl_chart_name = e.NewValues["gl_chart_name"].ToString();
//    _gl.gl_comments = e.NewValues["gl_comments"]==null?"": e.NewValues["gl_comments"].ToString();
//    _gl.gl_group_id = Convert.ToInt32(ddl.Value);
//    _gl.is_active = Convert.ToBoolean(e.NewValues["is_active"]);
//    _gl.is_bank = Convert.ToBoolean(e.NewValues["is_bank"]);
//    _gl.is_sales = Convert.ToBoolean(e.NewValues["is_sales"]);
//    _gl.is_mileage = Convert.ToBoolean(e.NewValues["is_mileage"]);
//    _gl.see_on_po_gl_list = Convert.ToBoolean(e.NewValues["see_on_po_gl_list"]);
//        _gl.netsuite_gl_id = e.NewValues["netsuite_gl_id"] == null ? "" : e.NewValues["netsuite_gl_id"].ToString();
//        _gl.save(true,false);    // setting to true saves to bv


//        //if bv failed, undo the mysql save and puke an error

//        e.Cancel = true;
//        gv.CancelEdit();
//        gv.DataBind();
//    }

//    protected void gv_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
//    {
//    ASPxComboBox ddl = (ASPxComboBox)gv.FindEditRowCellTemplateControl((GridViewDataColumn)gv.Columns["gl_group_id"], "ddl_group_edit");

//        NeGL _gl = new NeGL(0);
//    _gl.tax_entity_id = Convert.ToInt32(hdn_tax_ent_it.Value);
//    _gl.GL_Designation = e.NewValues["GL_Designation"].ToString();
//    _gl.InitCheque = e.NewValues["InitCheque"]==null?"0":e.NewValues["InitCheque"].ToString();
//    _gl.account_no = e.NewValues["account_no"].ToString();
//    _gl.currency_id = Convert.ToInt32(e.NewValues["currency_id"]);
//    _gl.gl_chart_name = e.NewValues["gl_chart_name"].ToString();
//    _gl.gl_comments = e.NewValues["gl_comments"] == null ? "" : e.NewValues["gl_comments"].ToString();
//    _gl.gl_group_id = Convert.ToInt32(ddl.Value);
//    _gl.is_active = Convert.ToBoolean(e.NewValues["is_active"]);
//    _gl.is_bank = Convert.ToBoolean(e.NewValues["is_bank"]);
//    _gl.is_sales = Convert.ToBoolean(e.NewValues["is_sales"]);
//    _gl.is_mileage = Convert.ToBoolean(e.NewValues["is_mileage"]);
//    _gl.see_on_po_gl_list = Convert.ToBoolean(e.NewValues["see_on_po_gl_list"]);
//       _gl.netsuite_gl_id = e.NewValues["netsuite_gl_id"] == null ? "" : e.NewValues["netsuite_gl_id"].ToString();
//        _gl.save(true,false);
//        e.Cancel = true;
//        gv.CancelEdit();
//        gv.DataBind();
//    }

//    protected void gv_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
//    {
//// commented out by ANDY until we know what we are doing.. so probably forever
//       NeGL _gl = new NeGL(Convert.ToInt32(e.Keys["id"]));
//        _gl.delete();


//        e.Cancel = true;
//        gv.CancelEdit();
//        gv.DataBind();
//    }

//    protected void btn_clear__Click(object sender, EventArgs e)
//    {
//     //    commented out by Andy because this will wipe out the BV 
//            NeTaxEntity te = new NeTaxEntity(Convert.ToInt32(hdn_tax_ent_it.Value));
//                   new Toolbox().getSQL_void(@"Delete from gl_chart_of_accounts", te.DSN  , null); // this will wipe out BV do not enable
//                 new Toolbox().getSQL_void(@"Delete from gl_transactions", te.DSN  , null);//this will wipe out BV do not enable
//                 new Toolbox().getSQL_void(@"Delete from gl_segments", te.DSN  , null);//this will wipe out BV do not enable

//            new Toolbox().getSQL_void(@"Delete from gltrans  where gltrans_gl_id in (Select id from gl_te where tax_entity_id=@v0) ", new object[] { te.id });

//            new Toolbox().getSQL_void(@"Delete gl_chart_of_accounts.* from gl_chart_of_accounts 
//    inner join gl_te on gl_te.id = gl_chart_of_accounts.gl_te_id 
//    where gl_te.tax_entity_id =@v0 ", new object[] { te.id });


//            new Toolbox().getSQL_void(@"Delete from gl_te  where gl_te.tax_entity_id =@v0", new object[] { te.id });

//            gv.DataBind();
//            Session["dt_glhistory"] = null;
//            fill_history();

//            // ANother version will just CLEAR the accounts, without wiping them out

//                 _tools.getSQL_void("update gl_chart_of_accounts set " +
//        "open_bal=0," +
//            "last_yr01=0," +
//            "last_yr02=0," +
//            "last_yr03=0," +
//            "last_yr04=0," +
//            "last_yr05=0," +
//            "last_yr06=0," +
//            "last_yr07=0," +
//            "last_yr08=0," +
//            "last_yr09=0," +
//            "last_yr10=0," +
//            "last_yr11=0," +
//            "last_yr12=0," +
//            "last_yr13=0," +
//            "this_yr01=0," +
//            "this_yr02=0," +
//            "this_yr03=0," +
//            "this_yr04=0," +
//            "this_yr05=0," +
//            "this_yr06=0," +
//            "this_yr07=0," +
//            "this_yr08=0," +
//            "this_yr09=0," +
//            "this_yr10=0," +
//            "this_yr11=0," +
//            "this_yr12=0," +
//            "this_yr13=0", te.DSN, null); 
//      //   loop through and clear out mysql chart of accounts 

//     //   */
//        _tools.getSQL_void("update gl_chart_of_accounts inner join gl_te on gl_te.id = gl_chart_of_accounts.gl_te_id set" +
//                           " open_bal=0," +
//                           " last_yr01=0," +
//                           " last_yr02=0," +
//                           " last_yr03=0," +
//                           " last_yr04=0," +
//                           " last_yr05=0," +
//                           " last_yr06=0," + 
//                           " last_yr07=0," +
//                           " last_yr08=0," +
//                           " last_yr09=0," +
//                           " last_yr10=0," +
//                           " last_yr11=0," +
//                           " last_yr12=0," +
//                           " last_yr13=0," +
//                           " this_yr01=0," +
//                           " this_yr02=0," +
//                           " this_yr03=0," +
//                           " this_yr04=0," +
//                           " this_yr05=0," +
//                           " this_yr06=0," +
//                           " this_yr07=0," +
//                           " this_yr08=0," +
//                           " this_yr09=0," +
//                           " this_yr10=0," +
//                           " this_yr11=0," +
//                           " this_yr12=0," +
//                           " this_yr13=0  where gl_te.tax_entity_id=" + hdn_tax_ent_it.Value);

//        // loop through and clear out bv accoutns
//    }




//    protected void btn_merge_from_legacy_bv_Click(object sender, EventArgs e)
//    {
//        div_inbv.InnerHtml = "";
//        string dsn = ddl_structure_from_legacy.Text ;
//        DataTable dt = get_all_bv_gls(dsn,false);
//        foreach (DataRow dr in dt.Rows)
//        {
//            div_inbv.InnerHtml += dr["acct_no"] + " - " + dr["name"] + "</br>";
//        }
//        pop_frombv.ShowOnPageLoad = true;
//    }

//    protected void btn_copy_data_from_legacy_Click(object sender, EventArgs e)
//    {
//        div_inbv.InnerHtml = "";
//        pop_frombv.ShowOnPageLoad = true;
//    }



//    protected void btn_copy_bs_data_from_bv_Click(object sender, EventArgs e)
//    {
//        //       NeGLChart_of_accounts.update_consol_div(2010);
//        //      return;
//        div_inbv.InnerHtml = "";

//        DataTable dt = get_bv_balance_sheet_gls(Convert.ToInt32(hdn_tax_ent_it.Value));
//        div_inbv.InnerHtml = "<table><th><td>Acct No</td><td>Name</td><td>GL_ID</td></th>";
//        foreach (DataRow dr in dt.Rows)
//        {
//            div_inbv.InnerHtml += string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",
//                dr["acct_no"], dr["name"], dr["gl_id"]);
//        }
//        div_inbv.InnerHtml += "</table>";
//        pop_frombv.ShowOnPageLoad = true;

//    }

//    protected void btn_copy_groups_from_bv_Click(object sender, EventArgs e)
//        {
//        string dsn = new NeTaxEntity(hdn_tax_ent_it.Value).DSN;
//        DataTable dt = _tools.getSQL_datatable("Select * from gl_groups", dsn,null);
//        foreach (DataRow dr in dt.Rows)
//            {
//            if (_tools.getSQL_int("Select count(id) from gl_group_te where tax_entity_id = @v0 and number = @v1",
//                    new object[] {hdn_tax_ent_it.Value, dr["gl_group"].ToString()}) == 0)
//                {
//                NeGLGroup n = new NeGLGroup();
//                n.tax_entity_id = Convert.ToInt32(hdn_tax_ent_it.Value);
//                n.desc = dr["default_desc"].ToString();
//                n.line_advance = dr["line_advance"].ToString();
//                n.number = dr["gl_group"].ToString();
//                n.total = Convert.ToInt32(dr["total"]);
//                n.type = dr["acct_type"].ToString();
//                n.save(false);

//                }
//            }
//        gv_groups.DataBind();
//        }

//    protected void btn_clear_unused_groups_Click(object sender, EventArgs e)
//    {
//        // grab all GLs current visible on the screen
//        string dsn = new NeTaxEntity(hdn_tax_ent_it.Value).DSN;
//        for (int i = 0; i < gv_groups.VisibleRowCount; i++)
//        {
//            //checked if used in any accounts.

//            if (_tools.getSQL_int(@"Select count(*) from gl_chart_of_accounts  where gl_group =?" , dsn, new object[] { gv_groups.GetRowValues(i, "number") }) == 0)
//            {
//                _tools.getSQL_void(@"Delete from gl_group_te  where tax_entity_id =@v0 and number =@v1  limit 1 ", new object[] { hdn_tax_ent_it.Value,gv_groups.GetRowValues(i, "number") });
// /* Commented out by Andy               _tools.getSQL_void(@"Delete from gl_groups  where gl_group =?" , dsn, new object[] { gv_groups.GetRowValues(i, "number") });
//        */    }

//        }
//        gv_groups.DataBind();


//    }

//    protected void btn_sync_Click(object sender, EventArgs e)
//    {
//        NeAccounting.sync_gl_accounts(hdn_tax_ent_it.Value);
//        gv.DataBind();
//        gv_groups.DataBind();
        
//    }

//    protected void fill_special_accounts()
//        {
//        if (Session["dt_glspecial_accounts"] == null)
//            {

//            Session["dt_glspecial_accounts"] = _tools.getSQL_datatable(@"call get_special_accounts(@v0)", new object[] { hdn_tax_ent_it.Value });
//            }
//        gv_sa.DataSource = Session["dt_glspecial_accounts"];
//        gv_sa.DataBind();
//        }

//    protected void ddl_gl_Init(object sender, EventArgs e)
//    {
//    var ddl = (ASPxComboBox)sender;
//    var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
//    var _type = gv_sa.GetRowValues(container.VisibleIndex, "_type").ToString();
//   if ((int)container.KeyValue == 11)
//        {
//        if ((int) gv_sa.GetRowValues(container.VisibleIndex, "tax1") == 0)
//            {
//            ddl.Enabled = false;
//            }
//        }
//    else if ((int)container.KeyValue == 12)
//        {
//        if ((int)gv_sa.GetRowValues(container.VisibleIndex, "tax2") == 0)
//            {
//            ddl.Enabled = false;
//            }
//        }
//    else if ((int)container.KeyValue == 13)
//        {
//        if ((int)gv_sa.GetRowValues(container.VisibleIndex, "tax3") == 0)
//            {
//            ddl.Enabled = false;
//            }
//        }
//    else if ((int)container.KeyValue == 14)
//        {
//        if ((int)gv_sa.GetRowValues(container.VisibleIndex, "tax4") == 0)
//            {
//            ddl.Enabled = false;
//            }
//        }
//    if ((int)container.KeyValue == 37)
//        {
//        if (Convert.ToInt32(gv_sa.GetRowValues(container.VisibleIndex, "bu")) <= 1)
//            {
//            ddl.Enabled = false;
//            }
//        }
//        ddl.DataSourceID = "sql_gl_" + _type;
//    ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function(s,e){{gv_sa.PerformCallback('{0}|' + s.GetValue() + '|n'); }}", container.KeyValue);

//    }

//    protected void pc_ActiveTabChanged(object source, TabControlEventArgs e)
//    {
//    if (pc.ActiveTabIndex == 2)
//        {
//        fill_special_accounts();

//        }
//    }

//    protected void gv_sa_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
//        {
//        var id = e.Parameters.Split('|').GetValue(0);
//        var sa_value = e.Parameters.Split('|').GetValue(1);
//        var link_id = gv_sa.GetRowValuesByKeyValue(id, "link_id");

//        if (sa_value != "")
//            {
//            if (sa_value == "0")
//            {
//                _tools.getSQL_void("Delete from gl_special_account_te_link where id=" + link_id);
//            }
//            if (link_id == DBNull.Value || link_id == "")
//                {
//                _tools.getSQL_void("Insert into gl_special_account_te_link (gl_te_id,special_accounts_id) values(" +
//                                   sa_value + "," + id + ")");
//                }
//            else
//                {
//                _tools.getSQL_void(
//                    "update gl_special_account_te_link set gl_te_id=" + sa_value + " where id=" + link_id);
//                }
//            Session["dt_glspecial_accounts"] = null;
          
//            }
//  fill_special_accounts();
//        }
}