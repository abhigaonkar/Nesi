using System;
using System.Data;
using DevExpress.Web;
//using nesi.bv;

namespace nesi.core
    {
    /// <summary>
    /// Summary description for NEAddress
    /// </summary>
//
    public class email_reports
        {
        //public static DataTable get_cash_report(int _member_id)
        //    {
        //    using (var conn = Toolbox.connect())
        //        {
        //        var dt = new DataTable();
        //        dt.Columns.Add("Tax Entity");
        //        dt.Columns.Add("Holdco");
                
        //        dt.Columns.Add("10105 TD CAN", typeof(double));
        //        dt.Columns.Add("10110 TD USD", typeof(double));
        //        dt.Columns.Add("10205 USD", typeof(double));
        //        dt.Columns.Add("10250 ING CAD", typeof(double));
        //        dt.Columns.Add("10260 ING USD", typeof(double));
        //        dt.Columns.Add("11122 AK TD", typeof(double));
        //        dt.Columns.Add("10012 ARK TD", typeof(double));
        //        dt.Columns.Add("10014 ARK USD TD", typeof(double));
        //        dt.Columns.Add("10150 EURO", typeof(double));
        //        dt.Columns.Add("11120 USD", typeof(double));
        //        dt.Columns.Add("10210 USD", typeof(double));
        //        dt.Columns.Add("10200 Nesi Dep CAN", typeof(double));
        //        dt.Columns.Add("10190 Nesi Cash CAN", typeof(double));
        //        dt.Columns.Add("10170 Nesi US Dep USD", typeof(double));
        //        dt.Columns.Add("10160 Nesi US Cash USD", typeof(double));
        //        dt.Columns.Add("10130 TD Spark Cash CAD", typeof(double));
        //        dt.Columns.Add("10140 TD Spark Cash USD", typeof(double));
        //        dt.Columns.Add("Total", typeof(double));
        //        dt.Columns.Add("last_updated", typeof(DateTime));
        //        var m = new NeMember(_member_id);
        //        var visible_tax_entities = new Current_User().visible_tax_entities;
        //        var visible_business_units = new Current_User().visible_business_units;
        //        var rstsettings = Toolbox.doSQL_dt(conn,@"SELECT * FROM tax_entity WHERE  is_active = 1 AND dsn != '' and id in (0" + (visible_tax_entities == "" ? "" : "," + visible_tax_entities) + ")",null);
               
        //        foreach (DataRow dr_businessUnit in rstsettings.Rows)
        //            {
        //            var dr = dt.NewRow();
        //            var _adder = "";

        //            var ye = Convert.ToInt32(dr_businessUnit["YearEnd_Month"]);

        //            var ye_pointer = 1;
        //            //			if (NeBusinessUnit.IsWaitingRollover(dr_company["id"].ToString()))
        //            if (NeBusinessUnit.IsWaitingRollover(Convert.ToInt32(dr_businessUnit["id"]))
        //            ) // this has to be fixed
        //                {
        //                dr["Tax Entity"] = dr_businessUnit["ddl_name"];
                      
        //                dr["Holdco"] = dr_businessUnit["is_active"].ToString().Equals("1") &&
        //                               dr_businessUnit["is_test"].ToString().Equals("0");
        //                while (ye_pointer <= 12)
        //                    {
        //                    _adder += "+ifNull(this_yr" + ye_pointer.ToString().PadLeft(2, '0') + ",0)";
        //                    ye_pointer++;
        //                    }
        //                ye_pointer = 1;
        //                while (ye_pointer <= 12)
        //                    {
        //                    _adder += "+ifNull(next_yr" + ye_pointer.ToString().PadLeft(2, '0') + ",0)";
        //                    ye_pointer++;
        //                    }

        //                }
        //            else
        //                {
        //                if (ye == 12)
        //                    {
        //                    while (ye_pointer <= DateTime.Today.Month)
        //                        {
        //                        _adder += "+ifNull(this_yr" + ye_pointer.ToString().PadLeft(2, '0') + ",0)";
        //                        ye_pointer++;
        //                        }
        //                    }
        //                else
        //                    {
        //                    ye_pointer = 1;
        //                    if (DateTime.Today.Month > ye
        //                    ) // if we are between june and december, build 1-whatever the month is.
        //                        {
        //                        while (ye_pointer <= DateTime.Today.Month - ye)
        //                            {
        //                            _adder += "+ifNull(this_yr" + ye_pointer.ToString().PadLeft(2, '0') + ",0)";
        //                            ye_pointer++;
        //                            }
        //                        }
        //                    else
        //                        {
        //                        while (ye_pointer <= DateTime.Today.Month + (12 - ye))
        //                            {
        //                            _adder += "+ifNull(this_yr" + ye_pointer.ToString().PadLeft(2, '0') + ",0)";
        //                            ye_pointer++;
        //                            }
        //                        }
        //                    }
        //                dr["Tax Entity"] = dr_businessUnit["ddl_name"];
        //                dr["Holdco"] = dr_businessUnit["is_active"].ToString().Equals("1") &&
        //                               dr_businessUnit["is_test"].ToString().Equals("0");
        //                }
        //            double total = 0;
        //            var gl_account = "";
        //            foreach (DataColumn dc in dt.Columns)
        //                {
        //                if (dc.ColumnName.Substring(0, 1) == "1")
        //                    {
        //                    gl_account = dc.ColumnName.Substring(0, 5);
        //                    var rsttb = Toolbox.doSQL_dt(conn,
        //                        "SELECT ifNull(GL_Designation, '') DR_CR_DESIG,round(ifNull(open_bal,0)+ifNull(last_yr01,0)+ifNull(last_yr02,0)+ifNull(last_yr03,0)+ifNull(last_yr04,0)+ifNull(last_yr05,0)+ifNull(last_yr06,0)+ifNull(last_yr07,0)+ifNull(last_yr08,0)+ifNull(last_yr09,0)+ifNull(last_yr10,0)+ifNull(last_yr11,0)+ifNull(last_yr12,0)" +
        //                        _adder +
        //                        ",2) as totalopen, ifNull(type, '') type, ifNull(account_no, 000) Acct_no,ifNull(date_modified, '1900-01-01') date_modified FROM GL_CHART_OF_ACCOUNTS left join gl_te on gl_te.id = gl_chart_of_accounts.gl_te_id left join gl_group_te on gl_te.gl_group_id = gl_group_te.id where (type = 'L' or type = 'A') and account_no in('" +
        //                        gl_account + "') and business_unit_id = 0 and gl_te.tax_entity_id=" + dr_businessUnit["id"].ToString(), null);
        //                    dr[dc.ColumnName] = rsttb.Rows.Count > 0 ? Convert.ToDouble(rsttb.Rows[0]["totalopen"]) : 0;
        //                    total += rsttb.Rows.Count > 0 ? Convert.ToDouble(rsttb.Rows[0]["totalopen"]) : 0;
        //                    if (rsttb.Rows.Count > 0)
        //                        {
        //                        dr["last_updated"] = Convert.ToDateTime(rsttb.Rows[0]["date_modified"]);
        //                        }
        //                    }
        //                if (dc.ColumnName.Equals("Total"))
        //                    {
        //                    dr["Total"] = total;
        //                    total = 0;
        //                    }
        //                }
        //            dt.Rows.Add(dr);
        //            }
        //        return dt;
        //        }
        //    }

//        public static DataTable get_income_statement(int _year, int _month, string buids, int tax_entity_id)
//            {
//            var _tools = new Toolbox();
//            var te = new NeTaxEntity(tax_entity_id);
//            var waiting_rollover = NeTaxEntity.IsWaitingRollover(tax_entity_id);

//            if (buids.Length < 1)
//                {
//                return new DataTable();
//                }
//            buids = buids.TrimEnd(',');
//            var business_units = " and (";

//            if ((buids.ToString()=="0")||(!buids.Contains(",")&& (new NeBusinessUnit(Convert.ToInt32(buids)).gl_div == "000")))
//                {
                
//                    business_units += " a.business_unit_id = 0 and b.tax_entity_id = " + tax_entity_id +
//                                      ") ";
//                }
//            else
//                {
//                foreach (var c_selected in buids.Split(','))
//                    {
//                    business_units += " a.business_unit_id = " + c_selected + " or ";
//                    }
//                if (business_units != " and (")
//                    {
//                    business_units = business_units.Remove(business_units.Length - 4, 4) + ")";
//                    }
//                else
//                    {
//                    business_units += "1=1)";
//                    }
//                }

//            var DATE = new DateTime(_year,_month,1);
//            var _month_table = waiting_rollover?  "next_yr" + string.Format("{0:MM}", DATE) : "this_yr" + string.Format("{0:MM}", DATE);
//            var _total_tables = "";
//            var _budget_total_tables = "";
//            var month_int = DATE.Month;
//            var year_int = DATE.Year;
//            var base_c = new NeBusinessUnit(Convert.ToInt32(te.business_units.Rows[0]["id"]));
            
//            var fiscal_month = NeBusinessUnit.FiscalMonthLookup[base_c.fiscal_yearstart_month][month_int];
//            var _prefix = year_int == base_c.fiscal_current_year && fiscal_month >= base_c.fiscal_yearstart_month
//               ? "this_yr"
//               : "last_yr";

//            if (waiting_rollover)
//            {
//                _prefix = year_int == base_c.fiscal_current_year && fiscal_month >= base_c.fiscal_yearstart_month
//               ? "next_yr": year_int == base_c.fiscal_current_year-1 && fiscal_month >= base_c.fiscal_yearstart_month? 
//                "this_yr": "last_yr";
//            }

          

//                for (var m = 1; m <= fiscal_month; m++)
//                {
//                var pre = m < 10 ? "0" : ""; // Zero padding
//                var suff = m != fiscal_month ? "+" : "";
//                var month = pre + m;
//                _total_tables += _prefix + month + suff;
//                _budget_total_tables += _prefix + "_bgt_" + month + suff;
//                }

//            _month_table = string.Format("{0}{1:00}", _prefix, fiscal_month);

//            var _total_ytd =
//                _tools.getSQL_double(string.Format(
//                    "SELECT IFNULL(SUM({0}), 0) YTD FROM gl_chart_of_accounts a INNER JOIN gl_te b ON a.gl_te_id = b.id INNER JOIN gl_group_te c ON b.gl_group_id = c.id WHERE c.number >= 400 AND c.type = 'R' {1}",
//                    _total_tables, business_units),null);
//            var total_debit_ytd = _tools.getSQL_double(
//                string.Format(
//                    "SELECT IFNULL(SUM({0}), 0) YTD FROM gl_chart_of_accounts a INNER JOIN gl_te b ON a.gl_te_id = b.id INNER JOIN gl_group_te c ON b.gl_group_id = c.id WHERE c.number >= 400 AND c.type = 'X' {1}",
//                    _total_tables, business_units), null);
//			var _total_mtd =
//                _tools.getSQL_double(string.Format(
//                    "SELECT IFNULL(SUM({0}), 0) total_credit_mtd FROM gl_chart_of_accounts a INNER JOIN gl_te b ON a.gl_te_id = b.id INNER JOIN gl_group_te c ON b.gl_group_id = c.id WHERE c.number >= 400 AND c.type = 'R' {1}",
//                    _month_table, business_units), null);
//			var total_debit_mtd = _tools.getSQL_double(
//                string.Format(
//                    "SELECT IFNULL(SUM({0}), 0) total_credit_mtd FROM gl_chart_of_accounts a INNER JOIN gl_te b ON a.gl_te_id = b.id INNER JOIN gl_group_te c ON b.gl_group_id = c.id WHERE c.number >= 400 AND c.type = 'X' {1}",
//                    _month_table, business_units), null);

//			var groupfootercounter = 0;
//            var month_ind = _month_table.Substring(_month_table.Length - 2, 2);

//	            if (business_units == " and (1=1)")
//                {
//                business_units =
//                    " AND a.business_unit_id = 0 AND a.gl_te_id IN (SELECT id FROM gl_te WHERE tax_entity_id = " +
//                    tax_entity_id + ") ";
//                }
//            var d = string.Format(@"
//SELECT 
//	b.account_no ACCT_NO,
//	IFNULL(SUM({1}), 0) YTD,
//	concat(b.account_no,'-',UPPER(b.gl_chart_name)) ACCOUNT,
//	sum({0}) MTD,
//	sum({5}_bgt_{2}) BMTD,
//	IFNULL(SUM({3}), 0) BYTD,
//	(IFNULL(SUM({1}), 0)-IFNULL(SUM({3}), 0)) BYTDDIFF,
//	(sum({0})-sum({5}_bgt_{2})) BMTDDIFF, 
//	CONCAT(CONCAT(c.number,' '),c.desc) gl_group_alias,
//	b.gl_designation drcr, 
//	if(c.type='R','Revenue','Expense') type,
//	IF(c.type='R' or c.number='700','Gross','Net') pos,
//	date_modified 
//FROM 
//	gl_chart_of_accounts a
//INNER JOIN
//	gl_te b ON a.gl_te_id = b.id 
//INNER JOIN
//	gl_group_te c ON b.gl_group_id = c.id
//WHERE 
//	c.type IN ('R','X') 
//	{4}
//GROUP BY
//	acct_no,
//	CONCAT(CONCAT(c.number,' '),c.desc),
	
//c.type,
//b.gl_designation,
//	c.number 
//HAVING
//	YTD != 0 OR BYTD != 0 OR MTD != 0
//ORDER BY 
//	c.type desc ,c.number", _month_table, _total_tables, month_ind, _budget_total_tables, business_units, _prefix);
//            var _dt = Toolbox.doSQL_dt(d,null);

//            double this_mtd = 0;
//            double this_mtd_rev = 0;
//            double this_ytd = 0;
//            double this_ytd_rev = 0;


//            var this_acct_no = "";
//            var YTD_SALES = new DataColumn("YTD_SALES");
//            YTD_SALES.DataType = this_ytd.GetType();
//            _dt.Columns.Add(YTD_SALES);
//            var MTD_SALES = new DataColumn("MTD_SALES");
//            MTD_SALES.DataType = this_mtd.GetType();
//            _dt.Columns.Add(MTD_SALES);
//            var x = 0;

//            foreach (DataRow _dr in _dt.Rows)
//                {
//                x++;

//                this_acct_no = _dr["ACCT_NO"].ToString().Trim();
//                _dr["ACCT_NO"] = this_acct_no;
//                this_ytd = Convert.ToDouble(_dr["YTD"]);
//                this_mtd = Convert.ToDouble(_dr["MTD"]);

//                if (_dr["type"].ToString() == "Revenue" && _dr["drcr"].ToString() == "D")
//                    {
//                    _dr["YTD"] = Math.Abs(this_ytd)*-1;
//                    _dr["MTD"] = Math.Abs(this_mtd)*-1;
//                    _dr["BYTDDIFF"] = Math.Abs(Convert.ToDouble(_dr["BYTDDIFF"]))*-1;
//                    _dr["BMTDDIFF"] = Math.Abs(Convert.ToDouble(_dr["BMTDDIFF"]))*-1;
//                    _dr["BYTD"] = Math.Abs(Convert.ToDouble(_dr["BYTD"]))*-1;
//                    _dr["BMTD"] = Math.Abs(Convert.ToDouble(_dr["BMTD"]))*-1;
//                }


//                if (this_acct_no == "50111" && this_acct_no == "400")
//                    {
//                    _dr["YTD"] = Math.Abs(this_ytd);
//                    _dr["MTD"] = Math.Abs(this_mtd);
//                    _dr["BYTDDIFF"] = Math.Abs(Convert.ToDouble(_dr["BYTDDIFF"]));
//                    _dr["BMTDDIFF"] = Math.Abs(Convert.ToDouble(_dr["BMTDDIFF"]));
//                    _dr["BYTD"] = Math.Abs(Convert.ToDouble(_dr["BYTD"]));
//                    _dr["BMTD"] = Math.Abs(Convert.ToDouble(_dr["BMTD"]));
//                    _dr["type"] = "Revenue";
//                    }
//                else
//                    {
//                    this_ytd_rev = this_ytd / _total_ytd;
//                    this_mtd_rev = this_mtd / _total_mtd;
//                    _dr["YTD_SALES"] = double.IsInfinity(this_ytd_rev) || double.IsNaN(this_ytd_rev) ? 0 : this_ytd_rev;
//                    _dr["MTD_SALES"] = double.IsInfinity(this_mtd_rev) || double.IsNaN(this_mtd_rev) ? 0 : this_mtd_rev;
//                    }
//                _dr.AcceptChanges();
//                }
//            return _dt;

//            }

      
        }
    }