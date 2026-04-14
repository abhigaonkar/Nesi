using System;
using System.Data;
using MySql.Data.MySqlClient;
//using nesi.bv;
using ne_xpo.cs;

namespace nesi.core
	{
    /// <summary>
    /// Summary description for NEContact
    /// </summary>
    public class NeGLChart_of_accounts
    {
        public int id { get; set; }
        public DateTime date_modified { get; set; }
        public int business_unit_id { get; set; }
        public int gl_te_id { get; set; }
        public double open_bal {get;set;}
        public double last_yr01 { get; set; }
        public double last_yr02 { get; set; }
        public double last_yr03 { get; set; }
        public double last_yr04 { get; set; }
        public double last_yr05 { get; set; }
        public double last_yr06 { get; set; }
        public double last_yr07 { get; set; }
        public double last_yr08 { get; set; }
        public double last_yr09 { get; set; }
        public double last_yr10 { get; set; }
        public double last_yr11 { get; set; }
        public double last_yr12 { get; set; }
        public double last_yr13 { get; set; }
        public double this_yr01 { get; set; }
        public double this_yr02 { get; set; }
        public double this_yr03 { get; set; }
        public double this_yr04 { get; set; }
        public double this_yr05 { get; set; }
        public double this_yr06 { get; set; }
        public double this_yr07 { get; set; }
        public double this_yr08 { get; set; }
        public double this_yr09 { get; set; }
        public double this_yr10 { get; set; }
        public double this_yr11 { get; set; }
        public double this_yr12 { get; set; }
        public double this_yr13 { get; set; }
        public double next_yr01 { get; set; }
        public double next_yr02 { get; set; }
        public double next_yr03 { get; set; }
        public double next_yr04 { get; set; }
        public double next_yr05 { get; set; }
        public double next_yr06 { get; set; }
        public double next_yr07 { get; set; }
        public double next_yr08 { get; set; }
        public double next_yr09 { get; set; }
        public double next_yr10 { get; set; }
        public double next_yr11 { get; set; }
        public double next_yr12 { get; set; }
        public double next_yr13 { get; set; }
        public double last_yr_bgt_01 { get; set; }
        public double last_yr_bgt_02 { get; set; }
        public double last_yr_bgt_03 { get; set; }
        public double last_yr_bgt_04 { get; set; }
        public double last_yr_bgt_05 { get; set; }
        public double last_yr_bgt_06 { get; set; }
        public double last_yr_bgt_07 { get; set; }
        public double last_yr_bgt_08 { get; set; }
        public double last_yr_bgt_09 { get; set; }
        public double last_yr_bgt_10 { get; set; }
        public double last_yr_bgt_11 { get; set; }
        public double last_yr_bgt_12 { get; set; }
        public double last_yr_bgt_13 { get; set; }
        public double this_yr_bgt_01 { get; set; }
        public double this_yr_bgt_02 { get; set; }
        public double this_yr_bgt_03 { get; set; }
        public double this_yr_bgt_04 { get; set; }
        public double this_yr_bgt_05 { get; set; }
        public double this_yr_bgt_06 { get; set; }
        public double this_yr_bgt_07 { get; set; }
        public double this_yr_bgt_08 { get; set; }
        public double this_yr_bgt_09 { get; set; }
        public double this_yr_bgt_10 { get; set; }
        public double this_yr_bgt_11 { get; set; }
        public double this_yr_bgt_12 { get; set; }
        public double this_yr_bgt_13 { get; set; }
        public double next_yr_bgt_01 { get; set; }
        public double next_yr_bgt_02 { get; set; }
        public double next_yr_bgt_03 { get; set; }
        public double next_yr_bgt_04 { get; set; }
        public double next_yr_bgt_05 { get; set; }
        public double next_yr_bgt_06 { get; set; }
        public double next_yr_bgt_07 { get; set; }
        public double next_yr_bgt_08 { get; set; }
        public double next_yr_bgt_09 { get; set; }
        public double next_yr_bgt_10 { get; set; }
        public double next_yr_bgt_11 { get; set; }
        public double next_yr_bgt_12 { get; set; }
        public double next_yr_bgt_13 { get; set; }
        public string netsuite_gl_id { get; set; }
        public NeGL gl_te { get; set; }
        NeGLGroup gl_group { get; set; }

        public NeGLChart_of_accounts(int business_unit_id, string acct_no)
        {
            var _get_id = Toolbox.doSQL_int(@"SELECT g.id FROM gl_chart_of_accounts g left join gl_te on gl_te.id =g.gl_te_id WHERE g.business_unit_id = @v0  and gl_te.account_no=@v1 ", new object[] {  business_unit_id,acct_no } );
            load(_get_id);
        }

        public NeGLChart_of_accounts(int business_unit_id, int gl_te_id)
        {
       //     var _get_id = Toolbox.doSQL_int(@"Select ifnull((SELECT g.id FROM gl_chart_of_accounts g left join gl_te on gl_te.id =g.gl_te_id WHERE g.business_unit_id = @v0  and gl_te.id=@v1),0) ", new object[] {  business_unit_id, gl_te_id } );
       // if (_get_id == 0)
       //     {
       //     NeGL gl = new NeGL(gl_te_id);
       //     gl.save(false,false);
       //     }
       //_get_id = Toolbox.doSQL_int(@"Select ifnull((SELECT g.id FROM gl_chart_of_accounts g left join gl_te on gl_te.id =g.gl_te_id WHERE g.business_unit_id = @v0  and gl_te.id=@v1),0) ", new object[] { business_unit_id, gl_te_id });

       //     load(_get_id);
        }


        public NeGLChart_of_accounts(string _acct_no, string dsn, string div)
        {
            //load_from_bv(_acct_no, dsn, div);
        }

        //protected void load_from_bv(string _acct_no, string dsn, string div)
        //{
        //    var dt = new Toolbox().getSQL_datatable(@"SELECT * FROM GL_Chart_of_accounts WHERE acct_no =? and division = ? ", dsn , new object[] {  div ,_acct_no} );
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        id = 0;
        //        open_bal = Convert.ToDouble(dr["open_bal"]);
               
        //        last_yr01 = Convert.ToDouble(dr["last_yr01"]);
        //        last_yr02 = Convert.ToDouble(dr["last_yr02"]);
        //        last_yr03 = Convert.ToDouble(dr["last_yr03"]);
        //        last_yr04 = Convert.ToDouble(dr["last_yr04"]);
        //        last_yr05 = Convert.ToDouble(dr["last_yr05"]);
        //        last_yr06 = Convert.ToDouble(dr["last_yr06"]);
        //        last_yr07 = Convert.ToDouble(dr["last_yr07"]);
        //        last_yr08 = Convert.ToDouble(dr["last_yr08"]);
        //        last_yr09 = Convert.ToDouble(dr["last_yr09"]);
        //        last_yr10 = Convert.ToDouble(dr["last_yr10"]);
        //        last_yr11 = Convert.ToDouble(dr["last_yr11"]);
        //        last_yr12 = Convert.ToDouble(dr["last_yr12"]);
        //        last_yr13 = Convert.ToDouble(dr["last_yr13"]);
        //        this_yr01 = Convert.ToDouble(dr["this_yr01"]);
        //        this_yr02 = Convert.ToDouble(dr["this_yr02"]);
        //        this_yr03 = Convert.ToDouble(dr["this_yr03"]);
        //        this_yr04 = Convert.ToDouble(dr["this_yr04"]);
        //        this_yr05 = Convert.ToDouble(dr["this_yr05"]);
        //        this_yr06 = Convert.ToDouble(dr["this_yr06"]);
        //        this_yr07 = Convert.ToDouble(dr["this_yr07"]);
        //        this_yr08 = Convert.ToDouble(dr["this_yr08"]);
        //        this_yr09 = Convert.ToDouble(dr["this_yr09"]);
        //        this_yr10 = Convert.ToDouble(dr["this_yr10"]);
        //        this_yr11 = Convert.ToDouble(dr["this_yr11"]);
        //        this_yr12 = Convert.ToDouble(dr["this_yr12"]);
        //        this_yr13 = Convert.ToDouble(dr["this_yr13"]);
        //        next_yr01 = Convert.ToDouble(dr["next_yr01"]);
        //        next_yr02 = Convert.ToDouble(dr["next_yr02"]);
        //        next_yr03 = Convert.ToDouble(dr["next_yr03"]);
        //        next_yr04 = Convert.ToDouble(dr["next_yr04"]);
        //        next_yr05 = Convert.ToDouble(dr["next_yr05"]);
        //        next_yr06 = Convert.ToDouble(dr["next_yr06"]);
        //        next_yr07 = Convert.ToDouble(dr["next_yr07"]);
        //        next_yr08 = Convert.ToDouble(dr["next_yr08"]);
        //        next_yr09 = Convert.ToDouble(dr["next_yr09"]);
        //        next_yr10 = Convert.ToDouble(dr["next_yr10"]);
        //        next_yr11 = Convert.ToDouble(dr["next_yr11"]);
        //        next_yr12 = Convert.ToDouble(dr["next_yr12"]);
        //        next_yr13 = Convert.ToDouble(dr["next_yr13"]);
        //        last_yr_bgt_01 = Convert.ToDouble(dr["BDGT_LAST_YR01"]);
        //        last_yr_bgt_02 = Convert.ToDouble(dr["BDGT_LAST_YR02"]);
        //        last_yr_bgt_03 = Convert.ToDouble(dr["BDGT_LAST_YR03"]);
        //        last_yr_bgt_04 = Convert.ToDouble(dr["BDGT_LAST_YR04"]);
        //        last_yr_bgt_05 = Convert.ToDouble(dr["BDGT_LAST_YR05"]);
        //        last_yr_bgt_06 = Convert.ToDouble(dr["BDGT_LAST_YR06"]);
        //        last_yr_bgt_07 = Convert.ToDouble(dr["BDGT_LAST_YR07"]);
        //        last_yr_bgt_08 = Convert.ToDouble(dr["BDGT_LAST_YR08"]);
        //        last_yr_bgt_09 = Convert.ToDouble(dr["BDGT_LAST_YR09"]);
        //        last_yr_bgt_10 = Convert.ToDouble(dr["BDGT_LAST_YR10"]);
        //        last_yr_bgt_11 = Convert.ToDouble(dr["BDGT_LAST_YR11"]);
        //        last_yr_bgt_12 = Convert.ToDouble(dr["BDGT_LAST_YR12"]);
        //        last_yr_bgt_13 = Convert.ToDouble(dr["BDGT_LAST_YR13"]);
        //        this_yr_bgt_01 = Convert.ToDouble(dr["BDGT_THIS_YR01"]);
        //        this_yr_bgt_02 = Convert.ToDouble(dr["BDGT_THIS_YR02"]);
        //        this_yr_bgt_03 = Convert.ToDouble(dr["BDGT_THIS_YR03"]);
        //        this_yr_bgt_04 = Convert.ToDouble(dr["BDGT_THIS_YR04"]);
        //        this_yr_bgt_05 = Convert.ToDouble(dr["BDGT_THIS_YR05"]);
        //        this_yr_bgt_06 = Convert.ToDouble(dr["BDGT_THIS_YR06"]);
        //        this_yr_bgt_07 = Convert.ToDouble(dr["BDGT_THIS_YR07"]);
        //        this_yr_bgt_08 = Convert.ToDouble(dr["BDGT_THIS_YR08"]);
        //        this_yr_bgt_09 = Convert.ToDouble(dr["BDGT_THIS_YR09"]);
        //        this_yr_bgt_10 = Convert.ToDouble(dr["BDGT_THIS_YR10"]);
        //        this_yr_bgt_11 = Convert.ToDouble(dr["BDGT_THIS_YR11"]);
        //        this_yr_bgt_12 = Convert.ToDouble(dr["BDGT_THIS_YR12"]);
        //        this_yr_bgt_13 = Convert.ToDouble(dr["BDGT_THIS_YR13"]);
        //        next_yr_bgt_01 = Convert.ToDouble(dr["BDGT_NEXT_YR01"]);
        //        next_yr_bgt_02 = Convert.ToDouble(dr["BDGT_NEXT_YR02"]);
        //        next_yr_bgt_03 = Convert.ToDouble(dr["BDGT_NEXT_YR03"]);
        //        next_yr_bgt_04 = Convert.ToDouble(dr["BDGT_NEXT_YR04"]);
        //        next_yr_bgt_05 = Convert.ToDouble(dr["BDGT_NEXT_YR05"]);
        //        next_yr_bgt_06 = Convert.ToDouble(dr["BDGT_NEXT_YR06"]);
        //        next_yr_bgt_07 = Convert.ToDouble(dr["BDGT_NEXT_YR07"]);
        //        next_yr_bgt_08 = Convert.ToDouble(dr["BDGT_NEXT_YR08"]);
        //        next_yr_bgt_09 = Convert.ToDouble(dr["BDGT_NEXT_YR09"]);
        //        next_yr_bgt_10 = Convert.ToDouble(dr["BDGT_NEXT_YR10"]);
        //        next_yr_bgt_11 = Convert.ToDouble(dr["BDGT_NEXT_YR11"]);
        //        next_yr_bgt_12 = Convert.ToDouble(dr["BDGT_NEXT_YR12"]);
        //        next_yr_bgt_13 = Convert.ToDouble(dr["BDGT_NEXT_YR13"]);

        //    }
        //}

        protected void load(int _id)
        {
            var dt = Toolbox.doSQL_dt(@"SELECT * FROM gl_Chart_of_accounts WHERE id = @v0 ", new object[] {  _id } );
            foreach (DataRow dr in dt.Rows)
            {
                id = (int)dr["id"];
                gl_te = new NeGL(Convert.ToInt32(dr["gl_te_id"]));
                gl_group = gl_te.gl_group;
                open_bal =Convert.ToDouble(dr["open_bal"]);
                business_unit_id = Convert.ToInt32(dr["business_unit_id"]);
                gl_te_id = gl_te.id;
                netsuite_gl_id = dr["Netsuite_Internal_gl_id"] == DBNull.Value ? "" : dr["Netsuite_Internal_gl_id"].ToString();
                last_yr01 =Convert.ToDouble(dr["last_yr01"]);
         last_yr02 =Convert.ToDouble(dr["last_yr02"]);
         last_yr03 =Convert.ToDouble(dr["last_yr03"]);
         last_yr04 =Convert.ToDouble(dr["last_yr04"]);
         last_yr05 =Convert.ToDouble(dr["last_yr05"]);
         last_yr06 =Convert.ToDouble(dr["last_yr06"]);
         last_yr07 =Convert.ToDouble(dr["last_yr07"]);
         last_yr08 =Convert.ToDouble(dr["last_yr08"]);
         last_yr09 =Convert.ToDouble(dr["last_yr09"]);
         last_yr10 =Convert.ToDouble(dr["last_yr10"]);
         last_yr11 =Convert.ToDouble(dr["last_yr11"]);
         last_yr12 =Convert.ToDouble(dr["last_yr12"]);
         last_yr13 =Convert.ToDouble(dr["last_yr13"]);
         this_yr01 =Convert.ToDouble(dr["this_yr01"]);
         this_yr02 =Convert.ToDouble(dr["this_yr02"]);
         this_yr03 =Convert.ToDouble(dr["this_yr03"]);
         this_yr04 =Convert.ToDouble(dr["this_yr04"]);
         this_yr05 =Convert.ToDouble(dr["this_yr05"]);
         this_yr06 =Convert.ToDouble(dr["this_yr06"]);
         this_yr07 =Convert.ToDouble(dr["this_yr07"]);
         this_yr08 =Convert.ToDouble(dr["this_yr08"]);
         this_yr09 =Convert.ToDouble(dr["this_yr09"]);
         this_yr10 =Convert.ToDouble(dr["this_yr10"]);
         this_yr11 =Convert.ToDouble(dr["this_yr11"]);
         this_yr12 =Convert.ToDouble(dr["this_yr12"]);
         this_yr13 =Convert.ToDouble(dr["this_yr13"]);
         next_yr01 =Convert.ToDouble(dr["next_yr01"]);
         next_yr02 =Convert.ToDouble(dr["next_yr02"]);
         next_yr03 =Convert.ToDouble(dr["next_yr03"]);
         next_yr04 =Convert.ToDouble(dr["next_yr04"]);
         next_yr05 =Convert.ToDouble(dr["next_yr05"]);
         next_yr06 =Convert.ToDouble(dr["next_yr06"]);
         next_yr07 =Convert.ToDouble(dr["next_yr07"]);
         next_yr08 =Convert.ToDouble(dr["next_yr08"]);
         next_yr09 =Convert.ToDouble(dr["next_yr09"]);
         next_yr10 =Convert.ToDouble(dr["next_yr10"]);
         next_yr11 =Convert.ToDouble(dr["next_yr11"]);
         next_yr12 =Convert.ToDouble(dr["next_yr12"]);
         next_yr13 =Convert.ToDouble(dr["next_yr13"]);
         last_yr_bgt_01 =Convert.ToDouble(dr["last_yr_bgt_01"]);
         last_yr_bgt_02 =Convert.ToDouble(dr["last_yr_bgt_02"]);
         last_yr_bgt_03 =Convert.ToDouble(dr["last_yr_bgt_03"]);
         last_yr_bgt_04 =Convert.ToDouble(dr["last_yr_bgt_04"]);
         last_yr_bgt_05 =Convert.ToDouble(dr["last_yr_bgt_05"]);
         last_yr_bgt_06 =Convert.ToDouble(dr["last_yr_bgt_06"]);
         last_yr_bgt_07 =Convert.ToDouble(dr["last_yr_bgt_07"]);
         last_yr_bgt_08 =Convert.ToDouble(dr["last_yr_bgt_08"]);
         last_yr_bgt_09 =Convert.ToDouble(dr["last_yr_bgt_09"]);
         last_yr_bgt_10 =Convert.ToDouble(dr["last_yr_bgt_10"]);
         last_yr_bgt_11 =Convert.ToDouble(dr["last_yr_bgt_11"]);
         last_yr_bgt_12 =Convert.ToDouble(dr["last_yr_bgt_12"]);
         last_yr_bgt_13 =Convert.ToDouble(dr["last_yr_bgt_13"]);
         this_yr_bgt_01 =Convert.ToDouble(dr["this_yr_bgt_01"]);
         this_yr_bgt_02 =Convert.ToDouble(dr["this_yr_bgt_02"]);
         this_yr_bgt_03 =Convert.ToDouble(dr["this_yr_bgt_03"]);
         this_yr_bgt_04 =Convert.ToDouble(dr["this_yr_bgt_04"]);
         this_yr_bgt_05 =Convert.ToDouble(dr["this_yr_bgt_05"]);
         this_yr_bgt_06 =Convert.ToDouble(dr["this_yr_bgt_06"]);
         this_yr_bgt_07 =Convert.ToDouble(dr["this_yr_bgt_07"]);
         this_yr_bgt_08 =Convert.ToDouble(dr["this_yr_bgt_08"]);
         this_yr_bgt_09 =Convert.ToDouble(dr["this_yr_bgt_09"]);
         this_yr_bgt_10 =Convert.ToDouble(dr["this_yr_bgt_10"]);
         this_yr_bgt_11 =Convert.ToDouble(dr["this_yr_bgt_11"]);
         this_yr_bgt_12 =Convert.ToDouble(dr["this_yr_bgt_12"]);
         this_yr_bgt_13 =Convert.ToDouble(dr["this_yr_bgt_13"]);
         next_yr_bgt_01 =Convert.ToDouble(dr["next_yr_bgt_01"]);
         next_yr_bgt_02 =Convert.ToDouble(dr["next_yr_bgt_02"]);
         next_yr_bgt_03 =Convert.ToDouble(dr["next_yr_bgt_03"]);
         next_yr_bgt_04 =Convert.ToDouble(dr["next_yr_bgt_04"]);
         next_yr_bgt_05 =Convert.ToDouble(dr["next_yr_bgt_05"]);
         next_yr_bgt_06 =Convert.ToDouble(dr["next_yr_bgt_06"]);
         next_yr_bgt_07 =Convert.ToDouble(dr["next_yr_bgt_07"]);
         next_yr_bgt_08 =Convert.ToDouble(dr["next_yr_bgt_08"]);
         next_yr_bgt_09 =Convert.ToDouble(dr["next_yr_bgt_09"]);
         next_yr_bgt_10 =Convert.ToDouble(dr["next_yr_bgt_10"]);
         next_yr_bgt_11 =Convert.ToDouble(dr["next_yr_bgt_11"]);
         next_yr_bgt_12 =Convert.ToDouble(dr["next_yr_bgt_12"]);
         next_yr_bgt_13 =Convert.ToDouble(dr["next_yr_bgt_13"]);
                
    }

        }

        public NeGLChart_of_accounts(int id)
        {
            load(id);
        }


        //public void add_to_bv(bool save_to_bv, PsqlConnection BVconn, bool save_balances_to_bv)
        //{
        //    gl_te = new NeGL(gl_te_id);
        //    NeTaxEntity tax_entity = new NeTaxEntity(new NeGL(gl_te_id).tax_entity_id);
        //    NeBusinessUnit bu = new NeBusinessUnit(business_unit_id);
        //    if (business_unit_id == 0)
        //    {
        //        bu.gl_div = "000";
        //        bu.name = "Consolidated Company";
        //    }

        //    // verify division exists in BV  if not create it.

        //    if ((save_to_bv) && (BVDB.getSQL_int(BVconn, @"SELECT COUNT(*) FROM gl_divisions WHERE code = ? ", new object[] { bu.gl_div }) == 0))
        //    {
        //        GLDivision gl_div = new GLDivision();
        //        gl_div.Code = bu.gl_div;
        //        gl_div.Name = bu.name;
        //        gl_div.Save(BVconn, "AA", true);
        //    }

        //    if (save_to_bv)
        //    {

        //        if (new Toolbox().getSQL_int(
        //                @"SELECT COUNT(*) FROM gl_chart_of_accounts WHERE acct_no = ?  and division = ? ",
        //                tax_entity.DSN, new object[] { new NeGL(gl_te_id).account_no, bu.gl_div }) == 0)
        //        {
        //            GLAccount gl = new GLAccount
        //            {
        //                AccountNo = gl_te.account_no,
        //                Name = gl_te.gl_chart_name,
        //                GLGroup = gl_te.gl_group.number,
        //                Designation = gl_te.GL_Designation,
        //                SalesAccount = gl_te.is_sales,
        //                BankAccount = gl_te.is_bank,
        //                DivisionCode = bu.gl_div
        //            };
        //            if (gl_te.is_bank)
        //            {
        //                gl.NextChequeNumber = 0;
        //            }
        //            gl.Save(BVconn, "AA", true);

        //        }
        //    }
        //}

        
	

		public NeGLChart_of_accounts()
			{
            id = 0;
			//
			//  
			//
			}
		}
	}