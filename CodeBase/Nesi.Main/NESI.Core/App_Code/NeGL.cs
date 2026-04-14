using System;
using System.Data;
using DevExpress.Xpo;
using MySql.Data.MySqlClient;
//using nesi.bv;

namespace nesi.core
	{
    /// <summary>
    /// Summary description for NEContact
    /// </summary>
    public class NeGL
    {
        public int id { get; set; }
        public string account_no { get; set; }
        public string gl_chart_name { get; set; }
       
        public int tax_entity_id { get; set; }
        public int currency_id { get; set; }
        public string gl_comments { get; set; }
        public string InitCheque { get; set; }
        public string GL_Designation { get; set; }
        public bool is_bank { get; set; }
        
        public bool is_active { get; set; }
        
        public int gl_group_id { get; set; }
        public bool is_sales { get; set; }

        public bool is_mileage { get; set; }
    public bool see_on_po_gl_list { get; set; }
    public string netsuite_gl_id { get; set; }

        public NeGLGroup gl_group { get; set; }
        public NeGL(int tax_entity_id, string acct_no)
        {
            var _get_id = Toolbox.doSQL_int(@"SELECT id FROM gl_te WHERE tax_entity_id = @v0  and account_no=@v1 ", new object[] {  tax_entity_id,acct_no } );
            load(_get_id);
        }

        protected void load(int gl_id)
        {
            var dt = Toolbox.doSQL_dt(@"SELECT * FROM gl_te WHERE id = @v0 ", new object[] {  gl_id } );
            foreach (DataRow dr in dt.Rows)
            {
                id = (int)dr["id"];
                account_no = dr["account_no"].ToString();
                gl_chart_name = dr["gl_chart_name"].ToString();
               
                tax_entity_id = (int)dr["tax_entity_id"];
                currency_id = (int)dr["currency_id"];
                gl_comments = dr["gl_comments"].ToString();
                InitCheque = dr["InitCheque"].ToString();
                GL_Designation = dr["GL_Designation"].ToString();
                is_bank = (bool)dr["is_bank"];
                
                is_active = (bool)dr["is_active"];
                is_mileage = (bool) dr["is_mileage"];

                gl_group_id = Convert.ToInt32(dr["gl_group_id"]);
                is_sales = (bool)dr["is_sales"];
                gl_group = new NeGLGroup(gl_group_id);
            see_on_po_gl_list = (bool)dr["see_on_po_gl_list"];
                netsuite_gl_id = dr["netsuite_gl_id"] == DBNull.Value ? "" : dr["netsuite_gl_id"].ToString();
            }

        }

        public NeGL(int id)
        {
            load(id);
        }

//        public static bool has_history (int gl_id)
//        {
//            Toolbox _tools = new Toolbox();
//            NeGL gl = new NeGL(gl_id);
//            NeTaxEntity te = new NeTaxEntity(gl.tax_entity_id);
//            if (NeTaxEntity.is_int_gl(gl.tax_entity_id))
//            {
//                if (_tools.getSQL_string(@"Select ifnull((Select top 1 TRANS_NO from gl_transactions  where substring(acct_no,7,5)=? and division <> ''),'')", te.DSN, new object[] { gl.account_no })  !=  "")
//                {
//                    return true;
//                }
//                if (_tools.getSQL_string(@" Select ifnull((Select ACCT_NO from gl_chart_of_accounts  where (open_bal <> 0 or THIS_YR01 <> 0 or THIS_YR02 <> 0 or THIS_YR03 <> 0 or THIS_YR04 <> 0 or THIS_YR05 <> 0 or THIS_YR06 <> 0 or THIS_YR07 <> 0 or THIS_YR08 <> 0 or THIS_YR09 <> 0 or THIS_YR10 <> 0 or THIS_YR11 <> 0 or THIS_YR12 <> 0 or THIS_YR13 <> 0 or LAST_YR01 <> 0 or LAST_YR02 <> 0 or LAST_YR03 <> 0 or LAST_YR04 <> 0 or LAST_YR05 <> 0 or LAST_YR06 <> 0 or LAST_YR07 <> 0 or LAST_YR08 <> 0 or LAST_YR09 <> 0 or LAST_YR10 <> 0 or LAST_YR11 <> 0 or LAST_YR12 <> 0 or LAST_YR13 <> 0 or NEXT_YR01 <> 0 or NEXT_YR02 <> 0 or NEXT_YR03 <> 0 or NEXT_YR04 <> 0 or NEXT_YR05 <> 0 or NEXT_YR06 <> 0 or NEXT_YR07 <> 0 or NEXT_YR08 <> 0 or NEXT_YR09 <> 0 or NEXT_YR10 <> 0 or NEXT_YR11 <> 0 or NEXT_YR12 <> 0 or NEXT_YR13 <> 0) and substring(acct_no,7,5) =? and division <> ''),'')", te.DSN, new object[] { gl.account_no })  !=  "")
//                {
//                    return true;
//                }

//            }
//            else
//            {
//                if (_tools.getSQL_string(@"Select ifnull((Select top 1 TRANS_NO from gl_transactions  where rtrim(acct_no)=? and division <> ''),'')", te.DSN, new object[] { gl.account_no })  !=  "")
//                {
//                    return true;
//                }
//                if (_tools.getSQL_string(@" Select ifnull((Select ACCT_NO from gl_chart_of_accounts  
//where (open_bal <> 0 or THIS_YR01 <> 0 or THIS_YR02 <> 0 or THIS_YR03 <> 0 or THIS_YR04 <> 0 or THIS_YR05 <> 0 or
//THIS_YR06 <> 0 or THIS_YR07 <> 0 or THIS_YR08 <> 0 or THIS_YR09 <> 0 or THIS_YR10 <> 0 or THIS_YR11 <> 0 or THIS_YR12 <> 0 or 
//THIS_YR13 <> 0 or LAST_YR01 <> 0 or LAST_YR02 <> 0 or LAST_YR03 <> 0 or LAST_YR04 <> 0 or LAST_YR05 <> 0 or LAST_YR06 <> 0 or LAST_YR07 <> 0 or
//LAST_YR08 <> 0 or LAST_YR09 <> 0 or LAST_YR10 <> 0 or LAST_YR11 <> 0 or LAST_YR12 <> 0 or LAST_YR13 <> 0 or NEXT_YR01 <> 0 or NEXT_YR02 <> 0 or NEXT_YR03 <> 0 or
//NEXT_YR04 <> 0 or NEXT_YR05 <> 0 or NEXT_YR06 <> 0 or NEXT_YR07 <> 0 or NEXT_YR08 <> 0 or NEXT_YR09 <> 0 or NEXT_YR10 <> 0 or NEXT_YR11 <> 0 or NEXT_YR12 <> 0 or
//NEXT_YR13 <> 0) and acct_no =? and division <> ''),'')", te.DSN, new object[] { gl.account_no }) !=  "")
//                {
//                    return true;
//                }
//            }
               
         
//            return false;   
//        }


        protected void validate()
        {
            if (account_no == null || account_no==""  || account_no.Length!=5)
            {
                throw new Exception("Invalid GL Account Number Supplied");
            }

            if (gl_chart_name == null || gl_chart_name =="" || gl_chart_name.Length > 60)
            {
                throw new Exception("Invalid GL Account Name Supplied.  Must be less than 60 characters");
            }
            if (tax_entity_id == 0)
            {
                throw new Exception("Invalid Tax Entity Supplied");
            }
            if ((currency_id == 0) || (Toolbox.doSQL_int(@"Select count(id) from currency  where id =@v0", new object[] { currency_id}) == 0 ))
            {
                throw new Exception("Invalid Currency Supplied");
            }
            if (gl_comments == null)
            {
                gl_comments = "";
            }
            if ( gl_comments.Length > 255)
            {
                throw new Exception("Invalid GL Comments Supplied.  Must be less than 255 characters");
            }
            if (GL_Designation==null|(GL_Designation != "C" && GL_Designation != "D" && GL_Designation!=" "))
            {
                throw new Exception("Invalid GL Designation.  Must be either D or C");
            }
            if ((gl_group_id == 0)|| (Toolbox.doSQL_int(@"Select count(id) from gl_group_te  where id =@v0", new object[] { gl_group_id }) == 0))
            {
                throw new Exception("Invalid GL Group ID");
            }
           
            

          
        }

    public DataTable get_all_gls_for_te(object te_id)
        {
        return (new Toolbox().getSQL_datatable(@" SELECT
*
FROM  gl_te INNER JOIN gl_group_te ON gl_te.gl_group_id = gl_group_te.id
WHERE gl_group_te.tax_entity_id =@v0 and gl_te.is_active = 1", new object[] { te_id }));

        }

//        public void save(bool save_to_bv, bool save_balances_to_bv)
//        {
//            try
//            {
//                validate();
//            }
//            catch (Exception ee)
//            {
//                throw ee;
//            }

//            if (tax_entity_id != 0)
//            {
//                NeTaxEntity te = new NeTaxEntity(tax_entity_id);
//            using (var BVconn = BVDB.connect(te.DSN))
//                {
//                gl_group = new NeGLGroup(gl_group_id);

//                if (id == 0)
//                    {
//                    using (var conn = Toolbox.connect())
//                        {
//                        var comm = new MySqlCommand(@"
//                    Insert into
//	                gl_te 
//                 (account_no ,
//                gl_chart_name ,
               
//                tax_entity_id,
//                currency_id ,
//                gl_comments ,
//                InitCheque ,
//                GL_Designation ,
//                is_bank ,
            
//                is_active ,
                
//                gl_group_id ,
//                is_sales,
//is_mileage,
//see_on_po_gl_list,
//netsuite_gl_id
//) 
//values
//(
// ?account_no,
// ?gl_chart_name,
 
// ?tax_entity_id,
// ?currency_id,
// ?gl_comments,
// ?InitCheque,
// ?GL_Designation,
// ?is_bank,
 
// ?is_active,

// ?gl_group_id,
//?is_sales,
//?is_mileage,
//?see_on_po_gl_list,
//?netsuite_gl_id)", conn);
//                        comm.Parameters.AddWithValue("?account_no", account_no);
//                        comm.Parameters.AddWithValue("?gl_chart_name", gl_chart_name);

//                        comm.Parameters.AddWithValue("?tax_entity_id", tax_entity_id);
//                        comm.Parameters.AddWithValue("?currency_id", currency_id);
//                        comm.Parameters.AddWithValue("?gl_comments", gl_comments);
//                        comm.Parameters.AddWithValue("?InitCheque", InitCheque);
//                        comm.Parameters.AddWithValue("?GL_Designation", GL_Designation);
//                        comm.Parameters.AddWithValue("?is_bank", is_bank);

//                        comm.Parameters.AddWithValue("?is_active", is_active);

//                        comm.Parameters.AddWithValue("?gl_group_id", gl_group_id);
//                        comm.Parameters.AddWithValue("?is_sales", is_sales);
//                        comm.Parameters.AddWithValue("?is_mileage", is_sales);
//                        comm.Parameters.AddWithValue("?see_on_po_gl_list", see_on_po_gl_list);
//                            comm.Parameters.AddWithValue("?netsuite_gl_id", netsuite_gl_id);
//                                comm.ExecuteNonQuery();
//                        id = Toolbox.doSQL_int(
//                            @"Select id from gl_te  where gl_te.tax_entity_id =@v0 and account_no=@v1 order by id desc limit 1 ",
//                            new object[] {tax_entity_id, account_no});

//                        try
//                            {

//                            if ((Toolbox.doSQL_int(
//                                     @"Select count(id) from gl_chart_of_accounts  where business_unit_id = 0 and gl_te_id=@v0",
//                                     new object[] {id}) == 0))
//                                {
//                                NeGLChart_of_accounts new_coa = new NeGLChart_of_accounts();
//                                new_coa.id = 0;
//                                new_coa.gl_te_id = id;
//                                new_coa.business_unit_id = 0;
//                                    new_coa.netsuite_gl_id = netsuite_gl_id;
//                                new_coa.save(save_to_bv,BVconn, save_balances_to_bv);
//                                }

//                            foreach (DataRow bu in te.business_units.Rows) // loop through business units
//                                {
//                                // make sure the group exists if not create it

//                                // make sure the dvision exists, if not create it

//                                if (Toolbox.doSQL_int(
//                                        @"Select count(id) from gl_chart_of_accounts  where business_unit_id =@v0 and gl_te_id=@v1 ",
//                                        new object[] {bu["id"], id}) == 0)
//                                    {
//                                    NeGLChart_of_accounts new_coa = new NeGLChart_of_accounts();
//                                    new_coa.id = 0;
//                                    new_coa.gl_te_id = id;
//                                        new_coa.netsuite_gl_id = netsuite_gl_id;
//                                        new_coa.business_unit_id = Convert.ToInt32(bu["id"]);
//                                    new_coa.save(save_to_bv,BVconn, save_balances_to_bv);
//                                    }

//                                }
//                            // next check if the consolidated gl level exists '000' in the GL _chart of accounts

//                            }
//                        catch (Exception ee)
//                            {
//                            throw ee;
//                            }

//                        }
//                    }
//                else
//                    {
//                    using (var conn = Toolbox.connect())
//                        {
//                        var comm = new MySqlCommand(@"
//UPDATE 
//	gl_te 
//SET 
//	            account_no = ?account_no,
//                gl_chart_name = ?gl_chart_name,
                
//                tax_entity_id = ?tax_entity_id,
//                currency_id = ?currency_id,
//                gl_comments = ?gl_comments,
//                InitCheque = ?InitCheque,
//                GL_Designation = ?GL_Designation,
//                is_bank = ?is_bank,
//                is_active =?is_active,
//                gl_group_id = ?gl_group_id, 
//is_sales = ?is_sales,
//is_mileage = ?is_mileage,
//see_on_po_gl_list = ?see_on_po_gl_list,
//netsuite_gl_id = ?netsuite_gl_id
//                WHERE 
//	id = ?id", conn);
//                        comm.Parameters.AddWithValue("?account_no", account_no);
//                        comm.Parameters.AddWithValue("?gl_chart_name", gl_chart_name);

//                        comm.Parameters.AddWithValue("?tax_entity_id", tax_entity_id);
//                        comm.Parameters.AddWithValue("?currency_id", currency_id);
//                        comm.Parameters.AddWithValue("?gl_comments", gl_comments);
//                        comm.Parameters.AddWithValue("?InitCheque", InitCheque);
//                        comm.Parameters.AddWithValue("?GL_Designation", GL_Designation);
//                        comm.Parameters.AddWithValue("?is_bank", is_bank);

//                        comm.Parameters.AddWithValue("?is_active", is_active);

//                        comm.Parameters.AddWithValue("?gl_group_id", gl_group_id);
//                        comm.Parameters.AddWithValue("?is_sales", is_sales);
//                        comm.Parameters.AddWithValue("?is_mileage", is_mileage);
//                        comm.Parameters.AddWithValue("?see_on_po_gl_list", see_on_po_gl_list);
//                            comm.Parameters.AddWithValue("?netsuite_gl_id", netsuite_gl_id);
//                            comm.Parameters.AddWithValue("?id", id);
//                        comm.ExecuteNonQuery();

//                        // update all bv and all departments

//                        if (Toolbox.doSQL_int(
//                                @"Select count(id) from gl_chart_of_accounts  where business_unit_id =0 and gl_te_id=@v0 ",
//                                new object[] {id}) == 0)
//                            {
//                            NeGLChart_of_accounts new_coa1 = new NeGLChart_of_accounts();
//                            new_coa1.id = 0;
//                            new_coa1.gl_te_id = id;
//                            new_coa1.business_unit_id = 0;
//                            new_coa1.save(save_to_bv,BVconn,false);
//                            }

//                        NeGLChart_of_accounts new_coa = new NeGLChart_of_accounts(0, id);
//                        new_coa.date_modified = System.DateTime.Now;
//                        new_coa.save(save_to_bv,BVconn, save_balances_to_bv);

//                        foreach (DataRow bu in te.business_units.Rows) // loop through business units
//                            {
//                            // make sure the group exists if not create it

//                            // make sure the dvision exists, if not create it


//                            if (Toolbox.doSQL_int(
//                                    @"Select count(id) from gl_chart_of_accounts  where business_unit_id =@v0 and gl_te_id=@v1 ",
//                                    new object[] {bu["id"], id}) == 0)
//                                {
//                                new_coa.id = 0;
//                                new_coa.gl_te_id = id;
//                                new_coa.business_unit_id = Convert.ToInt32(bu["id"]);
//                                new_coa.save(save_to_bv,BVconn,false);
//                                }

//                            new_coa = new NeGLChart_of_accounts(Convert.ToInt32(bu["id"]), id);
//                            new_coa.date_modified = System.DateTime.Now;
//                            new_coa.save(save_to_bv,BVconn, save_balances_to_bv);

//                            }

//                        }
//                    }
//                }
//            }
//        }


        //public int delete()
        //{
        //    string dsn = new NeTaxEntity(tax_entity_id).DSN;
        //    // validate the deletion first
        //    if (!has_history(id))
        //    {
        //        // delete the gl_chart_of_accounts first
        //        if (NeTaxEntity.is_int_gl(tax_entity_id))
        //        {
        //           new Toolbox().getSQL_void(@"Delete from gl_chart_of_accounts  where substring(acct_no,7,5) =?" , dsn, new object[] { account_no });
        //           new Toolbox().getSQL_void(@"Delete from gl_segments  where code =?" , dsn, new object[] { account_no });

        //        }
        //        else
        //        {
        //           new Toolbox().getSQL_void(@"Delete from gl_chart_of_accounts  where acct_no =?" , dsn, new object[] { account_no });
        //          new Toolbox().getSQL_void(@"Delete from gl_segments  where code =?" , dsn, new object[] { account_no });
        //        }

        //        // Delete from the gl_te table
        //        new Toolbox().getSQL_void(@"Delete from gl_chart_of_accounts  where gl_te_id =@v0", new object[] { id });
        //        new Toolbox().getSQL_void(@"Delete from gl_te  where gl_te.id =@v0", new object[] { id });

        //        return 1;
        //    }



        //    return 0;




        //}

        public NeGL()
			{
            id = 0;
			//
			//  
			//
			}
		public class Transaction
			{
			public int id { get; set; }
			public int gl_id { get; set; }
			public string bv_trans_no { get; set; }
			public string where_from { get; set; }
			public int business_unit_id { get; set; }
			public DateTime post_date { get; set; }
			public DateTime tran_date { get; set; }
			public DateTime rd_date { get; set; }
			public string memo { get; set; }
			public double debit { get; set; }
			public double credit { get; set; }
			public string source_dept { get; set; }
			public string reconcile_flag { get; set; }
			public int customer_id { get; set; }
			public int vendor_id { get; set; }
			public int woprog_id { get; set; }
			public int poprog_id { get; set; }
			public string acct_no { get; set; }
			public string div { get; set; }
			public int tax_entity_id { get; set; }
			public Transaction()
				{

				}
			public static void AttachToNetSuiteJE(MySqlConnection _conn, int _tax_entity_id, string _transaction_number, int _netsuite_id)
				{
				Toolbox.doSQL_void(_conn, @"UPDATE gltrans SET gltrans_ts = gltrans_ts, in_netsuite = 1, netsuite_id = @v2 WHERE tax_entity_id = @v0 AND gltrans_bv_trans_no = @v1", new object[] { _tax_entity_id, _transaction_number, _netsuite_id });
				}

			public void Save()
				{
				using(var uow = new UnitOfWork())
					{
					var line = id == 0 
									? new ne_xpo.cs.gltrans(uow) 
									: uow.GetObjectByKey<ne_xpo.cs.gltrans>(id);
					line.tax_entity_id = tax_entity_id;
					line.gltrans_acct_no = acct_no;
					line.gltrans_business_unit_id = business_unit_id;
					line.gltrans_gl_id = gl_id;
					line.gltrans_bv_trans_no = bv_trans_no;
					line.gltrans_where_from = where_from;
					line.gltrans_post_date = post_date;
					line.gltrans_tran_date = tran_date;
					line.gltrans_rd_date = rd_date;
					line.gltrans_memo = memo;
					line.gltrans_debit = debit;
					line.gltrans_credit = credit;
					line.gltrans_source_dept = source_dept;
					line.gltrans_reconcile_flag = reconcile_flag;
					line.gltrans_customer_id = customer_id;
					line.gltrans_vendor_id = vendor_id;
					line.gltrans_woprog_id = woprog_id;
					line.gltrans_poprog_id = poprog_id;
					line.gltrans_div = div;
					line.Save();
					uow.CommitChanges();
					id = line.gltrans_id;
					}
				}
			}
		}
	}