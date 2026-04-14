using System;
using System.Data;
using System.Text;
using DevExpress.Xpo;
using System.Linq;
using Microsoft.Ajax.Utilities;
//using nesi.bv;
using nesi.core;

namespace nesi.core
	{
    /// <summary>
    /// Summary description for NeAccounting
    /// </summary>
    public class NeAccounting
		{
		public NeAccounting() { }


		    public static void sync_gl_accounts(object te_id)
		    {

		        var test = "";

                NeTaxEntity te = new NeTaxEntity(te_id);
				if(te.DSN == "") return;
//            DataTable dt_bv = new Toolbox().getSQL_datatable(
//                   @" Select * from gl_chart_of_accounts left outer join gl_groups on gl_groups.gl_group = gl_chart_of_accounts.gl_group  
//where Division = '000' ", te.DSN, null);

            /*   DataTable dt_bv1     = new Toolbox().getSQL_datatable(
                       @" Select * from gl_chart_of_accounts left outer join gl_groups on gl_groups.gl_group = gl_chart_of_accounts.gl_group  
   where Division = '000' and (acct_no = '14310' or acct_no = '39401' or acct_no = '35500' or acct_no = '35005' or 
   open_bal <> 0 or this_yr01 <>0 or this_yr02 <>0 or this_yr03 <>0 or this_yr04 <>0 or this_yr05 <>0 or this_yr06 <>0 or this_yr07 <>0 or this_yr08 <>0 or this_yr09 <>0 or this_yr10 <>0 
   or this_yr11 <>0 or this_yr12 <>0 or this_yr13 <>0 or next_yr01 <>0 or next_yr02 <>0 or next_yr03 <>0 or next_yr04 <>0 or next_yr05 <>0 or next_yr06 <>0 or next_yr07 <>0 or next_yr08 <>0 
   or next_yr09 <>0 or next_yr10 <>0 or next_yr11 <>0 or next_yr12 <>0 or next_yr13 <>0 or last_yr01 <>0 or last_yr02 <>0 or last_yr03 <>0 or last_yr04 <>0 or last_yr05 <>0 or last_yr06 <>0 
   or last_yr07 <>0 or last_yr08 <>0 or last_yr09 <>0 or last_yr10 <>0 or last_yr11 <>0 or last_yr12 <>0 or last_yr13 <>0 or bdgt_last_yr01 <>0 or bdgt_last_yr02 <>0 or bdgt_last_yr03 <>0 
   or bdgt_last_yr04 <>0 or bdgt_last_yr05 <>0 or bdgt_last_yr06 <>0 or bdgt_last_yr07 <>0 or bdgt_last_yr08 <>0 or bdgt_last_yr09 <>0 or bdgt_last_yr10 <>0 or bdgt_last_yr11 <>0 
   or bdgt_last_yr12 <>0 or bdgt_last_yr13 <>0 or bdgt_this_yr01 <>0 or bdgt_this_yr02 <>0 or bdgt_this_yr03 <>0 or bdgt_this_yr04 <>0 or bdgt_this_yr05 <>0 or bdgt_this_yr06 <>0 
   or bdgt_this_yr07 <>0 or bdgt_this_yr08 <>0 or bdgt_this_yr09 <>0 or bdgt_this_yr10 <>0 or bdgt_this_yr11 <>0 or bdgt_this_yr12 <>0 or bdgt_this_yr13 <>0 or bdgt_next_yr01 <>0 
   or bdgt_next_yr02 <>0 or bdgt_next_yr03 <>0 or bdgt_next_yr04 <>0 or bdgt_next_yr05 <>0 or bdgt_next_yr06 <>0 or bdgt_next_yr07 <>0 
   or bdgt_next_yr08 <>0 or bdgt_next_yr09 <>0 or bdgt_next_yr10 <>0 or bdgt_next_yr11 <>0 or bdgt_next_yr12 <>0 or bdgt_next_yr13 <>0) or gl_chart_of_accounts.BVRVADDDATE > 
   " + DateTime.Today.AddMonths(-6).ToString("yyyyMMdd") + @" or 
   (Select count(*) 
   from gl_transactions 
   where gl_transactions.division = gl_chart_of_accounts.division 
   and gl_transactions.acct_no = gl_chart_of_accounts.acct_no)>0", te.DSN, null);*/

           


            //foreach (DataRow _bu in te.business_units.Rows)
		          //  {
		          //  using (var BVconn = BVDB.connect(te.DSN))
		          //      {
		          //      NeBusinessUnit bu = new NeBusinessUnit(_bu["id"]);
		          //      if (bu.gl_div.Length != 3)
		          //          {
		          //          throw new Exception("Invalid GL Div setting in this business unit.");
		          //          }
            //               // loop through all the BV GL accounts and make sure they exist in MYSQL
		          //      foreach (DataRow dr in dt_bv.Rows) // loop nthrough each GL
		          //          {
		          //          // check and populate groups into gl_group_te fom BV
		          //          if (new Toolbox().getSQL_int(@"Select count(id) from gl_group_te  where tax_entity_id =@v0 and number=@v1 ", new object[] { te_id, dr["gl_group"] }) == 0)
		          //              {
		          //              NeGLGroup newgroup = new NeGLGroup();
		          //              newgroup.id = 0;
		          //              newgroup.desc = dr["default_desc"].ToString().Trim();
		          //              newgroup.number = dr["gl_group"].ToString();
		          //              newgroup.tax_entity_id = Convert.ToInt32(te_id);
		          //              newgroup.type = dr["acct_type"].ToString();
		          //              newgroup.line_advance = dr["line_advance"].ToString();
		          //              newgroup.total = Convert.ToInt32(dr["total"]);
		          //              newgroup.save(false);
		          //              }
		          //          // check if GL  exists, if it does, skip it.  else write it into the gl_te
		          //          NeGL gl = new NeGL();
		          //          if (new Toolbox().getSQL_int(@"Select count(id) from gl_te  where tax_entity_id =@v0 and account_no=@v1 ", new object[] { te_id, dr["acct_no"].ToString().TrimStart('0').Substring(0, 5) }) == 0)
		          //              {
		          //              gl.id = 0;
		          //              gl.account_no = dr["acct_no"].ToString().TrimStart('0').Substring(0, 5);
		          //              gl.tax_entity_id = Convert.ToInt32(te_id);
		          //              gl.gl_chart_name = dr["name"].ToString();
		          //              gl.gl_comments = "";
		          //              gl.GL_Designation = dr["DR_CR_DESIG"].ToString();
		          //              gl.gl_group_id = new NeGLGroup(Convert.ToInt32(te_id), dr["gl_group"].ToString()).id;
		          //              gl.InitCheque = dr["NEXT_CHEQUE_NO"].ToString();
		          //              gl.is_active = true;
		          //              gl.currency_id = dr["CHART_CURRENCY"].ToString() == "CAD" || dr["CHART_CURRENCY"].ToString() == "CDN" ? 2 : dr["CHART_CURRENCY"].ToString().Trim() == "" ? 2 : new Toolbox().getSQL_int(@"select ifnull((Select id from currency  where currency =@v0), 2)", new object[] { dr["CHART_CURRENCY"] });
		          //              gl.is_sales = dr["sales_acct"].ToString() == "1";
		          //              gl.is_bank = dr["bank_acct"].ToString() == "1";
		          //              gl.save(false, false);
		          //              }

		                   

		          //          }


            //        // loop through all MYSQL gl accounts and save them to BV
            //        DataTable dt = new NeGL().get_all_gls_for_te(te.id);
            //        // get all GL accounts 
		          //          var dtBVAccounts = BVDB.getSQL_dt(BVconn,
            //                    "select acct_no,division from GL_CHART_OF_ACCOUNTS", new object[] {bu.gl_div});

            //            try
		          //          {
                            
		          //          foreach (DataRow dr in dt.Rows) // loop nthrough each GL
		          //          {


            //                    var foundAccountMatch2 = dtBVAccounts.Select("acct_no like '%" + dr["account_no"] + "%' and division = '000' ");
            //                var foundAccountMatch = dtBVAccounts.Select("acct_no like '%" + dr["account_no"] + "%' and division = '" + bu.gl_div + "' ");
            //                // check and populate groups into gl_group_te fom BV
            //                if (Toolbox.doSQL_int(
            //                            @"Select count(id) from gl_chart_of_accounts  where business_unit_id =@v0 and gl_te_id=@v1 ",
            //                            new object[] { bu.id32, Convert.ToInt32(dr["id"]) }) == 0)
		          //                      {
		          //                      NeGLChart_of_accounts new_coa = new NeGLChart_of_accounts();
		          //                      new_coa.id = 0;
		          //                      new_coa.gl_te_id = Convert.ToInt32(dr["id"]);
		          //                      new_coa.business_unit_id = bu.id32;
		          //                      new_coa.save(true, BVconn,false); // 
		          //                      }
            //                    else if (foundAccountMatch.Length == 0 )
		          //                      {
		          //                          // Check if that Account with division exists in BV?
            //                                // Create account in BV
            //                                NeGLChart_of_accounts new_coa2 = new NeGLChart_of_accounts();
		          //                          new_coa2.gl_te_id = Convert.ToInt32(dr["id"]);
		          //                          new_coa2.business_unit_id = bu.id32;
		          //                          new_coa2.add_to_bv(true, BVconn, false); // 
                                
            //                            }
            //                else if (  foundAccountMatch2.Length == 0)
            //                {

            //                    NeGLChart_of_accounts new_coa2 = new NeGLChart_of_accounts();
            //                    new_coa2.gl_te_id = Convert.ToInt32(dr["id"]);
            //                    new_coa2.business_unit_id = 0;
            //                    new_coa2.add_to_bv(true, BVconn, false); // 

            //                }
            //            }

            //        }
		          //      catch (Exception e)
		          //          {
		          //          throw new Exception(
		          //              "Some sort of BV Connection Problem, verify the tax_entity DSN setting is correct");

		          //          }

		          //      }
		          //  }

		        }

        public static void clear_mysql_gl_to_zero(object tax_entity_id)
        {
           
            Toolbox.doSQL_void("update gl_chart_of_accounts inner join gl_te on gl_te.id = gl_chart_of_accounts.gl_te_id set" +
                         " open_bal=0," +
                         " last_yr01=0," +
                         " last_yr02=0," +
                         " last_yr03=0," +
                         " last_yr04=0," +
                         " last_yr05=0," +
                         " last_yr06=0," +
                         " last_yr07=0," +
                         " last_yr08=0," +
                         " last_yr09=0," +
                         " last_yr10=0," +
                         " last_yr11=0," +
                         " last_yr12=0," +
                         " last_yr13=0," +
                         " this_yr01=0," +
                         " this_yr02=0," +
                         " this_yr03=0," +
                         " this_yr04=0," +
                         " this_yr05=0," +
                         " this_yr06=0," +
                         " this_yr07=0," +
                         " this_yr08=0," +
                         " this_yr09=0," +
                         " this_yr10=0," +
                         " this_yr11=0," +
                         " this_yr12=0," +
                         " this_yr13=0," +
                         " next_yr01=0," +
                         " next_yr02=0," +
                         " next_yr03=0," +
                         " next_yr04=0," +
                         " next_yr05=0," +
                         " next_yr06=0," +
                         " next_yr07=0," +
                         " next_yr08=0," +
                         " next_yr09=0," +
                         " next_yr10=0," +
                         " next_yr11=0," +
                         " next_yr12=0," +
                         " next_yr13=0," +
                         " last_yr_bgt_01=0," +
                         " last_yr_bgt_02=0," +
                         " last_yr_bgt_03=0," +
                         " last_yr_bgt_04=0," +
                         " last_yr_bgt_05=0," +
                         " last_yr_bgt_06=0," +
                         " last_yr_bgt_07=0," +
                         " last_yr_bgt_08=0," +
                         " last_yr_bgt_09=0," +
                         " last_yr_bgt_10=0," +
                         " last_yr_bgt_11=0," +
                         " last_yr_bgt_12=0," +
                         " last_yr_bgt_13=0," +
                         " this_yr_bgt_01=0," +
                         " this_yr_bgt_02=0," +
                         " this_yr_bgt_03=0," +
                         " this_yr_bgt_04=0," +
                         " this_yr_bgt_05=0," +
                         " this_yr_bgt_06=0," +
                         " this_yr_bgt_07=0," +
                         " this_yr_bgt_08=0," +
                         " this_yr_bgt_09=0," +
                         " this_yr_bgt_10=0," +
                         " this_yr_bgt_11=0," +
                         " this_yr_bgt_12=0," +
                         " this_yr_bgt_13=0," +
                         " next_yr_bgt_01=0," +
                         " next_yr_bgt_02=0," +
                         " next_yr_bgt_03=0," +
                         " next_yr_bgt_04=0," +
                         " next_yr_bgt_05=0," +
                         " next_yr_bgt_06=0," +
                         " next_yr_bgt_07=0," +
                         " next_yr_bgt_08=0," +
                         " next_yr_bgt_09=0," +
                         " next_yr_bgt_10=0," +
                         " next_yr_bgt_11=0," +
                         " next_yr_bgt_12=0," +
                         " next_yr_bgt_13=0" +
                         " where gl_te.tax_entity_id=@v0" , new object[] { tax_entity_id });

        }

        public static string get_bus_with_gl_data(int tax_entity_id)
        {
            return (Toolbox.doSQL_string(@"select ifnull((SELECT
group_concat(distinct gl_chart_of_accounts.business_unit_id) ids
FROM
gl_chart_of_accounts
INNER JOIN gl_te ON gl_chart_of_accounts.gl_te_id = gl_te.id AND gl_te.tax_entity_id = @v0
WHERE
gl_chart_of_accounts.business_unit_id !=0 and (
gl_chart_of_accounts.open_bal <> 0 OR
gl_chart_of_accounts.last_yr01 <> 0 OR
gl_chart_of_accounts.last_yr02 <> 0 OR
gl_chart_of_accounts.last_yr03 <> 0 OR
gl_chart_of_accounts.last_yr04 <> 0 OR
gl_chart_of_accounts.last_yr05 <> 0 OR
gl_chart_of_accounts.last_yr06 <> 0 OR
gl_chart_of_accounts.last_yr07 <> 0 OR
gl_chart_of_accounts.last_yr08 <> 0 OR
gl_chart_of_accounts.last_yr09 <> 0 OR
gl_chart_of_accounts.last_yr10 <> 0 OR
gl_chart_of_accounts.last_yr11 <> 0 OR
gl_chart_of_accounts.last_yr12 <> 0 OR
gl_chart_of_accounts.last_yr13 <> 0 OR

gl_chart_of_accounts.this_yr01 <> 0 OR
gl_chart_of_accounts.this_yr02 <> 0 OR
gl_chart_of_accounts.this_yr03 <> 0 OR
gl_chart_of_accounts.this_yr04 <> 0 OR
gl_chart_of_accounts.this_yr05 <> 0 OR
gl_chart_of_accounts.this_yr06 <> 0 OR
gl_chart_of_accounts.this_yr07 <> 0 OR
gl_chart_of_accounts.this_yr08 <> 0 OR
gl_chart_of_accounts.this_yr09 <> 0 OR
gl_chart_of_accounts.this_yr10 <> 0 OR
gl_chart_of_accounts.this_yr11 <> 0 OR
gl_chart_of_accounts.this_yr12 <> 0 OR
gl_chart_of_accounts.this_yr13 <> 0 OR

gl_chart_of_accounts.next_yr01 <> 0 OR
gl_chart_of_accounts.next_yr02 <> 0 OR
gl_chart_of_accounts.next_yr03 <> 0 OR
gl_chart_of_accounts.next_yr04 <> 0 OR
gl_chart_of_accounts.next_yr05 <> 0 OR
gl_chart_of_accounts.next_yr06 <> 0 OR
gl_chart_of_accounts.next_yr07 <> 0 OR
gl_chart_of_accounts.next_yr08 <> 0 OR
gl_chart_of_accounts.next_yr09 <> 0 OR
gl_chart_of_accounts.next_yr10 <> 0 OR
gl_chart_of_accounts.next_yr11 <> 0 OR
gl_chart_of_accounts.next_yr12 <> 0 OR
gl_chart_of_accounts.next_yr13 <> 0 OR

gl_chart_of_accounts.last_yr_bgt_01 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_02 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_03 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_04 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_05 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_06 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_07 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_08 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_09 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_10 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_11 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_12 <> 0 OR
gl_chart_of_accounts.last_yr_bgt_13 <> 0 OR

gl_chart_of_accounts.this_yr_bgt_01 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_02 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_03 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_04 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_05 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_06 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_07 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_08 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_09 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_10 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_11 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_12 <> 0 OR
gl_chart_of_accounts.this_yr_bgt_13 <> 0 OR

gl_chart_of_accounts.next_yr_bgt_01 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_02 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_03 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_04 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_05 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_06 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_07 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_08 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_09 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_10 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_11 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_12 <> 0 OR
gl_chart_of_accounts.next_yr_bgt_13 <> 0)),'')", new object[] { tax_entity_id }));
        }

		    public static void update_gl_chart_into_mysql(int tax_entity_id)
		        {
		        var _tools = new Toolbox();
		        //NeTaxEntity te = new NeTaxEntity(tax_entity_id);
		        string DSN = _tools.getSQL_string("select ifnull((Select dsn from tax_entity where id = " + tax_entity_id + "),'')",
		            null);

            // clear GL in NESI
            clear_mysql_gl_to_zero(tax_entity_id);


//                using (var BVconn = BVDB.connect(DSN))
//		            {


//		            // pull all the accounts from BV 
//		            DataTable dt;
//                // old method.. sucks dt = Toolbox.doSQL_dt(
//                //                   @" Select * from gl_chart_of_accounts left outer join gl_groups on gl_groups.gl_group = gl_chart_of_accounts.gl_group  where (acct_no = '39401' or acct_no = '35500' or acct_no = '35005' or open_bal <> 0 or this_yr01 <>0 or this_yr02 <>0 or this_yr03 <>0 or this_yr04 <>0 or this_yr05 <>0 or this_yr06 <>0 or this_yr07 <>0 or this_yr08 <>0 or this_yr09 <>0 or this_yr10 <>0 or this_yr11 <>0 or this_yr12 <>0 or this_yr13 <>0 or next_yr01 <>0 or next_yr02 <>0 or next_yr03 <>0 or next_yr04 <>0 or next_yr05 <>0 or next_yr06 <>0 or next_yr07 <>0 or next_yr08 <>0 or next_yr09 <>0 or next_yr10 <>0 or next_yr11 <>0 or next_yr12 <>0 or next_yr13 <>0 or last_yr01 <>0 or last_yr02 <>0 or last_yr03 <>0 or last_yr04 <>0 or last_yr05 <>0 or last_yr06 <>0 or last_yr07 <>0 or last_yr08 <>0 or last_yr09 <>0 or last_yr10 <>0 or last_yr11 <>0 or last_yr12 <>0 or last_yr13 <>0 or bdgt_last_yr01 <>0 or bdgt_last_yr02 <>0 or bdgt_last_yr03 <>0 or bdgt_last_yr04 <>0 or bdgt_last_yr05 <>0 or bdgt_last_yr06 <>0 or bdgt_last_yr07 <>0 or bdgt_last_yr08 <>0 or bdgt_last_yr09 <>0 or bdgt_last_yr10 <>0 or bdgt_last_yr11 <>0 or bdgt_last_yr12 <>0 or bdgt_last_yr13 <>0 or bdgt_this_yr01 <>0 or bdgt_this_yr02 <>0 or bdgt_this_yr03 <>0 or bdgt_this_yr04 <>0 or bdgt_this_yr05 <>0 or bdgt_this_yr06 <>0 or bdgt_this_yr07 <>0 or bdgt_this_yr08 <>0 or bdgt_this_yr09 <>0 or bdgt_this_yr10 <>0 or bdgt_this_yr11 <>0 or bdgt_this_yr12 <>0 or bdgt_this_yr13 <>0 or bdgt_next_yr01 <>0 or bdgt_next_yr02 <>0 or bdgt_next_yr03 <>0 or bdgt_next_yr04 <>0 or bdgt_next_yr05 <>0 or bdgt_next_yr06 <>0 or bdgt_next_yr07 <>0 or bdgt_next_yr08 <>0 or bdgt_next_yr09 <>0 or bdgt_next_yr10 <>0 or bdgt_next_yr11 <>0 or bdgt_next_yr12 <>0 or bdgt_next_yr13 <>0) order by acct_no",
//                //	                DSN, null);

//                string current_earnings = Toolbox.doSQL_string(@"SELECT IFNULL((SELECT
//gl_te.account_no
//FROM
//gl_te
//INNER JOIN gl_special_account_te_link ON gl_special_account_te_link.gl_te_id = gl_te.id
//WHERE
//gl_special_account_te_link.special_accounts_id = 40 AND
//gl_te.tax_entity_id = @v0), '')", new object[] { tax_entity_id });

//                string retained_earnings = Toolbox.doSQL_string(@"SELECT IFNULL((SELECT
//gl_te.account_no
//FROM
//gl_te
//INNER JOIN gl_special_account_te_link ON gl_special_account_te_link.gl_te_id = gl_te.id
//WHERE
//gl_special_account_te_link.special_accounts_id = 21 AND
//gl_te.tax_entity_id = @v0), '')", new object[] { tax_entity_id });

//		                if (current_earnings.IsNullOrWhiteSpace() || retained_earnings.IsNullOrWhiteSpace())
//		                    return;

//                dt = Toolbox.doSQL_dt(
//                                      @" Select * from gl_chart_of_accounts 
//left outer join gl_groups on gl_groups.gl_group = gl_chart_of_accounts.gl_group 
//where ((acct_no = '" + current_earnings + @"' or acct_no = '35500' or acct_no = '" + retained_earnings + @"' or open_bal <> 0 or this_yr01 <>0 or this_yr02 <>0 or this_yr03 <>0 or this_yr04 <>0 or this_yr05 <>0 or this_yr06 <>0 or this_yr07 <>0 or this_yr08 <>0 or this_yr09 <>0 or this_yr10 <>0 or this_yr11 <>0 or this_yr12 <>0 or this_yr13 <>0 or next_yr01 <>0 or next_yr02 <>0 or next_yr03 <>0 or next_yr04 <>0 or next_yr05 <>0 or next_yr06 <>0 or next_yr07 <>0 or next_yr08 <>0 or next_yr09 <>0 or next_yr10 <>0 or next_yr11 <>0 or next_yr12 <>0 or next_yr13 <>0 or last_yr01 <>0 or last_yr02 <>0 or last_yr03 <>0 or last_yr04 <>0 or last_yr05 <>0 or last_yr06 <>0 or last_yr07 <>0 or last_yr08 <>0 or last_yr09 <>0 or last_yr10 <>0 or last_yr11 <>0 or last_yr12 <>0 or last_yr13 <>0 or bdgt_last_yr01 <>0 or bdgt_last_yr02 <>0 or bdgt_last_yr03 <>0 or bdgt_last_yr04 <>0 or bdgt_last_yr05 <>0 or bdgt_last_yr06 <>0 or bdgt_last_yr07 <>0 or bdgt_last_yr08 <>0 or bdgt_last_yr09 <>0 or bdgt_last_yr10 <>0 or bdgt_last_yr11 <>0 or bdgt_last_yr12 <>0 or bdgt_last_yr13 <>0 or bdgt_this_yr01 <>0 or bdgt_this_yr02 <>0 or bdgt_this_yr03 <>0 or bdgt_this_yr04 <>0 or bdgt_this_yr05 <>0 or bdgt_this_yr06 <>0 or bdgt_this_yr07 <>0 or bdgt_this_yr08 <>0 or bdgt_this_yr09 <>0 or bdgt_this_yr10 <>0 or bdgt_this_yr11 <>0 or bdgt_this_yr12 <>0 or bdgt_this_yr13 <>0 or bdgt_next_yr01 <>0 or bdgt_next_yr02 <>0 or bdgt_next_yr03 <>0 or bdgt_next_yr04 <>0 or bdgt_next_yr05 <>0 or bdgt_next_yr06 <>0 or bdgt_next_yr07 <>0 or bdgt_next_yr08 <>0 or bdgt_next_yr09 <>0 or bdgt_next_yr10 <>0 or bdgt_next_yr11 <>0 or bdgt_next_yr12 <>0 or bdgt_next_yr13 <>0) 
//or 
//(Select count(*) 
//from gl_transactions 
//where gl_transactions.division = gl_chart_of_accounts.division 
//and gl_transactions.acct_no = gl_chart_of_accounts.acct_no)>0 )
//order by acct_no",
//		            	                DSN, null);

//                foreach (DataRow dr in dt.Rows)
//		                {
//		                // does the gro int gl_id = _tools.getSQL_int("Select id from gl_te where account_no= @v0 and tax_entity_id = @v1 and ")up exist for thsi tax_entity?
//		                int bu_id = dr["division"].ToString()=="000"? 0:
//                    _tools.getSQL_int(
//		                    "select ifnull((Select id from business_unit where tax_entity_id = @v0 and gl_div = @v1 limit 1),9999)",
//		                    new object[] {tax_entity_id, dr["division"].ToString()});
//		                		if(bu_id == 9999) continue;
//		                int gl_group_id =
//		                    _tools.getSQL_int(
//		                        "Select ifnull((Select id from gl_group_te where number= @v0 and tax_entity_id = @v1 limit 1),0)",
//		                        new object[] {dr["gl_group"], tax_entity_id});
//		                if (gl_group_id == 0)
//		                    {
//		                    NeGLGroup new_group = new NeGLGroup();
//		                    new_group.id = 0;
//		                    new_group.tax_entity_id = tax_entity_id;
//		                    new_group.desc = dr["default_desc"].ToString();
//		                    new_group.line_advance = dr["line_advance"].ToString();
//		                    new_group.number = dr["gl_group"].ToString();
//		                    new_group.total = Convert.ToInt32(dr["total"]);
//		                    new_group.type = dr["acct_type"].ToString();
//		                    new_group.save(false); // save to mysql.. but dont save it back to BV
//		                    gl_group_id = _tools.getSQL_int(
//		                        "Select id from gl_group_te where tax_entity_id = @v0 order by id desc limit 1",
//		                        new object[] {tax_entity_id});
//		                    }

//		                // does the account exist for this tax entity?
//		                int gl_id = _tools.getSQL_int(
//		                    "select ifnull((Select id from gl_te where account_no= @v0 and tax_entity_id = @v1 limit 1),0) ",
//		                    new object[] {dr["acct_no"], tax_entity_id});
//		                if (gl_id == 0)
//		                    {
//		                    NeGL gl = new NeGL();
//		                    gl.id = 0;
//		                    gl.account_no = dr["acct_no"].ToString().TrimStart('0').Substring(0, 5);
//		                    gl.tax_entity_id = tax_entity_id;
//		                    gl.gl_chart_name = dr["name"].ToString();
//		                    gl.gl_comments = "";
//		                    gl.GL_Designation = dr["DR_CR_DESIG"].ToString();
//		                    gl.gl_group_id = new NeGLGroup(tax_entity_id, dr["gl_group"].ToString()).id;
//		                    gl.InitCheque = dr["NEXT_CHEQUE_NO"].ToString();
//		                    gl.is_active = true;
//		                    gl.currency_id = dr["CHART_CURRENCY"].ToString() == "CAD" ||
//		                                     dr["CHART_CURRENCY"].ToString() == "CDN"
//		                        ? 2
//		                        : dr["CHART_CURRENCY"].ToString().Trim() == ""
//		                            ? 2
//		                            : _tools.getSQL_int(@"select ifnull((Select id from currency  where currency =@v0), 2)",
//		                                new object[] {dr["CHART_CURRENCY"]});
//		                    gl.is_sales = dr["sales_acct"].ToString() == "1";
//		                    gl.is_bank = dr["bank_acct"].ToString() == "1";
//		                    gl.save(false,false); // save to mysql.. but dont save it back to BV
//		                    gl_id = gl.id;
//		                    }
//		                // at this point, the group and te should exist for this tax entity and all its departments.
//		                NeGLChart_of_accounts coa =
//		                    new NeGLChart_of_accounts(bu_id, gl_id);
//		                coa.last_yr01 = Convert.ToDouble(dr["last_yr01"]);
//		                coa.last_yr02 = Convert.ToDouble(dr["last_yr02"]);
//		                coa.last_yr03 = Convert.ToDouble(dr["last_yr03"]);
//		                coa.last_yr04 = Convert.ToDouble(dr["last_yr04"]);
//		                coa.last_yr05 = Convert.ToDouble(dr["last_yr05"]);
//		                coa.last_yr06 = Convert.ToDouble(dr["last_yr06"]);
//		                coa.last_yr07 = Convert.ToDouble(dr["last_yr07"]);
//		                coa.last_yr08 = Convert.ToDouble(dr["last_yr08"]);
//		                coa.last_yr09 = Convert.ToDouble(dr["last_yr09"]);
//		                coa.last_yr10 = Convert.ToDouble(dr["last_yr10"]);
//		                coa.last_yr11 = Convert.ToDouble(dr["last_yr11"]);
//		                coa.last_yr12 = Convert.ToDouble(dr["last_yr12"]);
//		                coa.this_yr01 = Convert.ToDouble(dr["this_yr01"]);
//		                coa.this_yr02 = Convert.ToDouble(dr["this_yr02"]);
//		                coa.this_yr03 = Convert.ToDouble(dr["this_yr03"]);
//		                coa.this_yr04 = Convert.ToDouble(dr["this_yr04"]);
//		                coa.this_yr05 = Convert.ToDouble(dr["this_yr05"]);
//		                coa.this_yr06 = Convert.ToDouble(dr["this_yr06"]);
//		                coa.this_yr07 = Convert.ToDouble(dr["this_yr07"]);
//		                coa.this_yr08 = Convert.ToDouble(dr["this_yr08"]);
//		                coa.this_yr09 = Convert.ToDouble(dr["this_yr09"]);
//		                coa.this_yr10 = Convert.ToDouble(dr["this_yr10"]);
//		                coa.this_yr11 = Convert.ToDouble(dr["this_yr11"]);
//		                coa.this_yr12 = Convert.ToDouble(dr["this_yr12"]);
//		                coa.next_yr01 = Convert.ToDouble(dr["next_yr01"]);
//		                coa.next_yr02 = Convert.ToDouble(dr["next_yr02"]);
//		                coa.next_yr03 = Convert.ToDouble(dr["next_yr03"]);
//		                coa.next_yr04 = Convert.ToDouble(dr["next_yr04"]);
//		                coa.next_yr05 = Convert.ToDouble(dr["next_yr05"]);
//		                coa.next_yr06 = Convert.ToDouble(dr["next_yr06"]);
//		                coa.next_yr07 = Convert.ToDouble(dr["next_yr07"]);
//		                coa.next_yr08 = Convert.ToDouble(dr["next_yr08"]);
//		                coa.next_yr09 = Convert.ToDouble(dr["next_yr09"]);
//		                coa.next_yr10 = Convert.ToDouble(dr["next_yr10"]);
//		                coa.next_yr11 = Convert.ToDouble(dr["next_yr11"]);
//		                coa.next_yr12 = Convert.ToDouble(dr["next_yr12"]);

//		                coa.last_yr_bgt_01 = Convert.ToDouble(dr["bdgt_last_yr01"]);
//		                coa.last_yr_bgt_02 = Convert.ToDouble(dr["bdgt_last_yr02"]);
//		                coa.last_yr_bgt_03 = Convert.ToDouble(dr["bdgt_last_yr03"]);
//		                coa.last_yr_bgt_04 = Convert.ToDouble(dr["bdgt_last_yr04"]);
//		                coa.last_yr_bgt_05 = Convert.ToDouble(dr["bdgt_last_yr05"]);
//		                coa.last_yr_bgt_06 = Convert.ToDouble(dr["bdgt_last_yr06"]);
//		                coa.last_yr_bgt_07 = Convert.ToDouble(dr["bdgt_last_yr07"]);
//		                coa.last_yr_bgt_08 = Convert.ToDouble(dr["bdgt_last_yr08"]);
//		                coa.last_yr_bgt_09 = Convert.ToDouble(dr["bdgt_last_yr09"]);
//		                coa.last_yr_bgt_10 = Convert.ToDouble(dr["bdgt_last_yr10"]);
//		                coa.last_yr_bgt_11 = Convert.ToDouble(dr["bdgt_last_yr11"]);
//		                coa.last_yr_bgt_12 = Convert.ToDouble(dr["bdgt_last_yr12"]);
//		                coa.this_yr_bgt_01 = Convert.ToDouble(dr["bdgt_this_yr01"]);
//		                coa.this_yr_bgt_02 = Convert.ToDouble(dr["bdgt_this_yr02"]);
//		                coa.this_yr_bgt_03 = Convert.ToDouble(dr["bdgt_this_yr03"]);
//		                coa.this_yr_bgt_04 = Convert.ToDouble(dr["bdgt_this_yr04"]);
//		                coa.this_yr_bgt_05 = Convert.ToDouble(dr["bdgt_this_yr05"]);
//		                coa.this_yr_bgt_06 = Convert.ToDouble(dr["bdgt_this_yr06"]);
//		                coa.this_yr_bgt_07 = Convert.ToDouble(dr["bdgt_this_yr07"]);
//		                coa.this_yr_bgt_08 = Convert.ToDouble(dr["bdgt_this_yr08"]);
//		                coa.this_yr_bgt_09 = Convert.ToDouble(dr["bdgt_this_yr09"]);
//		                coa.this_yr_bgt_10 = Convert.ToDouble(dr["bdgt_this_yr10"]);
//		                coa.this_yr_bgt_11 = Convert.ToDouble(dr["bdgt_this_yr11"]);
//		                coa.this_yr_bgt_12 = Convert.ToDouble(dr["bdgt_this_yr12"]);
//		                coa.next_yr_bgt_01 = Convert.ToDouble(dr["bdgt_next_yr01"]);
//		                coa.next_yr_bgt_02 = Convert.ToDouble(dr["bdgt_next_yr02"]);
//		                coa.next_yr_bgt_03 = Convert.ToDouble(dr["bdgt_next_yr03"]);
//		                coa.next_yr_bgt_04 = Convert.ToDouble(dr["bdgt_next_yr04"]);
//		                coa.next_yr_bgt_05 = Convert.ToDouble(dr["bdgt_next_yr05"]);
//		                coa.next_yr_bgt_06 = Convert.ToDouble(dr["bdgt_next_yr06"]);
//		                coa.next_yr_bgt_07 = Convert.ToDouble(dr["bdgt_next_yr07"]);
//		                coa.next_yr_bgt_08 = Convert.ToDouble(dr["bdgt_next_yr08"]);
//		                coa.next_yr_bgt_09 = Convert.ToDouble(dr["bdgt_next_yr09"]);
//		                coa.next_yr_bgt_10 = Convert.ToDouble(dr["bdgt_next_yr10"]);
//		                coa.next_yr_bgt_11 = Convert.ToDouble(dr["bdgt_next_yr11"]);
//		                coa.next_yr_bgt_12 = Convert.ToDouble(dr["bdgt_next_yr12"]);
//		                coa.open_bal = Convert.ToDouble(dr["open_bal"]);
//		                coa.date_modified = System.DateTime.Now;
//		                coa.save(false, BVconn,false); // save to mysql.. but dont save it back to BV

//		                }

//		            // loop through all gl_s that didnt get updated and clear them 

//		     //       dt = Toolbox.doSQL_dt(
//		      //          @" Select gl_chart_of_accounts.* from gl_chart_of_accounts inner join gl_te on gl_te.id = gl_chart_of_accounts.gl_te_id   where gl_te.tax_entity_id = @v0 and  ( date_modified < now()-interval 2 minute ) ",
//		       //         new object[] {tax_entity_id});
//		        //    foreach (DataRow dr in dt.Rows)
//		         //       {
//		                //         NeGLChart_of_accounts.clear_consol_acct(Convert.ToInt32(dr["id"]),true);
//		          //      }
//		            }

		        }

		    public static class gl
			{
			public class group
				{
				public int id              { get; set; }
				public string number       { get; set; }
				public string desc		   { get; set; }
				public char type           { get; set; }

				public group(){}
				public group(int _id)
					{
					load(_id);
					}
				private void load(int _id)
					{
					id			= _id;
					if(exists(_id))
						{

						}
					}
				public static bool exists(int _id)
					{
					return true;
					}
				public void save()
					{
					if(id == 0)
						{
						#region New
						#endregion
						}
					else
						{
						#region Edit
						#endregion
						}
					}
				}
			public class subgroup
				{

				}
			}
		public class Terms
			{
			public int Id { get; set; }
			public string Code  { get; set; }
			public string Description  { get; set; }
			public short DaysDiscount  { get; set; }
			public double DiscountRate { get; set; }
			public short DaysBeforeDue { get; set; }
			public Terms() {}
			public Terms(int _id)
				{
				Load(_id);
				}
			private void Load(int _id)
				{
				if(_id == 0) return;
				using (var uow = new UnitOfWork())
					{
					var termObj	= uow.GetObjectByKey<ne_xpo.cs.term>(_id);
					Id = _id;
					Code = termObj.term_code;
					DaysBeforeDue = termObj.term_daysbeforedue;
					Description = termObj.term_desc;
					DaysDiscount = termObj.term_daysdiscount;
					DiscountRate = termObj.term_discountrate;
					}
				}
			public void Save()
				{
				using (var uow = new UnitOfWork())
					{
					var taxObj		= Id == 0 
										? new ne_xpo.cs.term(uow) 
										: uow.GetObjectByKey<ne_xpo.cs.term>(Id);
					taxObj.term_code = Code;
					taxObj.term_daysbeforedue = DaysBeforeDue;
					taxObj.term_desc = Description;
					taxObj.term_daysdiscount = DaysDiscount;
					taxObj.term_discountrate = DiscountRate;
					taxObj.Save();
					uow.CommitChanges();
					Id = taxObj.term_id;
					}
				}
//			public static void sync_bv(PsqlConnection _bvConn)
//				{
//				using (var uow = new UnitOfWork())
//					{
//					var terms = from t in new XPQuery<ne_xpo.cs.term>(uow)
//								select t;
//					foreach(var t in terms)
//						{
//						var dateNow = DateTime.Now.ToString("yyyyMMdd");
//						var timeNow = DateTime.Now.ToString("Hmmssff");
//						var count = BVDB.getSQL_int(_bvConn, "SELECT COUNT(*) FROM terms WHERE bvtermsinfocode = ?", new object[] {t.term_code});
//						if(count == 0)
//							{
//							BVDB.doSQL_void(_bvConn, @"
//INSERT INTO terms
//	(
//bvtermsinfocode,
//bvtermsinfodesc,
//bvdoubleterms,
//bvdayofmonth01,
//bvdaysbeforedue01,
//bvmindays01,
//bvdiscdayofmonth01,
//bvdiscdaysallowed01,
//bvdiscmindays01,
//bvdiscmethod01,
//bvdiscrate01,
//bvdayofmonth02,
//bvdaysbeforedue02,
//bvmindays02,
//bvdiscdayofmonth02,
//bvdiscdaysallowed02,
//bvdiscmindays02,
//bvdiscmethod02,
//bvdiscrate02,
//notepad,
//bvrvversion,
//bvrvmoddate,
//bvrvmodtime,
//bvrvuserinit,
//bvrvadddate,
//bvrvaddtime,
//bvrvadduserinit,
//bvexcharkey1,
//bvexcharkey2,
//bvexcharfield1,
//bvexcharfield2,
//bvexamtkey1,
//bvexamtkey2,
//bvexamtfield1,
//bvexamtfield2,
//bvexpricekey1,
//bvexpricekey2,
//bvexpricefield1,
//bvexpricefield2,
//bvexcharflagkey1,
//bvexcharflagkey2,
//bvexcharflag1,
//bvexcharflag2,
//bvexcharflag3,
//bvexcharflag4,
//bvexcharflag5,
//bvexcharflag6,
//bvexnumflagkey1,
//bvexnumflagkey2,
//bvexnumflag1,
//bvexnumflag2,
//bvexnumflag3,
//bvexnumflag4,
//bvexnumflag5,
//bvexnumflag6
//	)
//VALUES
//	(
//	?,
//	?,
//	'0',
//	'0',
//	?,
//	'0',
//	'0',
//	?,
//	'0',
//	'3',
//	?,
//	'0',
//	'0',
//	'0',
//	'0',
//	'0',
//	'0',
//	'3',
//	'0.00',
//	'',
//	'7.00',
//	?,
//	?,
//	'BV*',
//	?,
//	?,
//	'BV*',
//	'',
//	'',
//	'',
//	'',
//	'0.00',
//	'0.00',
//	'0.00',
//	'0.00',
//	'0.00000',
//	'0.00000',
//	'0.00000',
//	'0.00000',
//	'',
//	'',
//	'',
//	'',
//	'',
//	'',
//	'',
//	'',
//	'0',
//	'0',
//	'0',
//	'0',
//	'0',
//	'0',
//	'0',
//	'0'
//	)", new object[] {t.term_code, t.term_desc, t.term_daysbeforedue, t.term_daysdiscount, t.term_discountrate, dateNow, timeNow, dateNow, timeNow});
//							}
//						else
//							{
//							BVDB.doSQL_void(_bvConn, @"
//UPDATE 
//	terms
//SET 
//	bvtermsinfodesc = ?,
//	bvdaysbeforedue01 = ?,
//	bvdiscdaysallowed01 = ?,
//	bvdiscrate01 = ?,
//	bvrvmoddate = ?,
//	bvrvmodtime = ?
//WHERE 
//	bvtermsinfocode = ?
//", new object[]
//								{
//								t.term_desc,
//								t.term_daysbeforedue,
//								t.term_daysdiscount,
//								t.term_discountrate,
//								dateNow,
//								timeNow,
//								t.term_code
//								});
//							}
//						}
//					}
//				}
			}
		}
	}
