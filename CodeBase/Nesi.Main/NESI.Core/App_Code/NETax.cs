using System;
using System.Data;
//using nesi.bv;
namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeTax
	/// </summary>
	public class NeTax
		{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double Percentage { get; set; }
		public double dblPercentage {get; set;} 
		public int tax_bv_s_tax_no { get; set; }
		public string GLAccount { get; set; }
		public bool Active { get; set; }
		
		public NeTax() {}
		public NeTax(int _id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM tax where tax_id = @v0 ", new object[] { _id });
			if (dt.Rows.Count == 1)
				{
				var dr = dt.Rows[0];
				Id = _id;
				Name = dr["tax_name"].ToString();
				Percentage = Convert.ToDouble(dr["tax_percentage"]);
				tax_bv_s_tax_no = Convert.ToInt32(dr["tax_bv_s_tax_no"]);
				Description = dr["tax_description"].ToString();
				GLAccount = dr["tax_glaccount"].ToString();
				//	_tax_bv_s_tax_no = Convert.ToInt32(dr["tax_bv_s_tax_no"]);
				Active = Convert.ToBoolean(dr["is_active"]);
				dblPercentage = Percentage > 1
					? Percentage / 100
					: Percentage;
				}
			}
		public void Save()
			{
			if(Id == 0)
				{
				Id = Toolbox.doSQL_return_id(@"
INSERT INTO tax
	(
	tax_name,
	tax_percentage,
	tax_description,
	tax_GLaccount,
	tax_bv_s_tax_no,
	is_active
	)
VALUES
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5
	)
", new object[]{Name, Percentage, Description, GLAccount, tax_bv_s_tax_no, Active});
				}
			else
				{
				Toolbox.doSQL_void(@"
UPDATE 
	tax
SET
	tax_name = @v0,
	tax_percentage = @v1,
	tax_description = @v2,
	tax_GLaccount = @v3,
	tax_bv_s_tax_no = @v4,
	is_active = @v5
WHERE 
	tax_id = @v6
", new object[]{Name, Percentage, Description, GLAccount, tax_bv_s_tax_no, Active, Id});
				}
//			Save_to_bv();
			}

//        public bool Save_to_bv()
//			{
//			var comp = new NeTaxEntity();
//			DataTable comps = new NeTaxEntity().LoadActiveTaxEntitiesList();
//			var error = "";
//			foreach (DataRow dr in comps.Rows)
//				{
//				comp = new NeTaxEntity(dr["id"]);
//				var dsn = comp.DSN;
//				var is_consol = true;
//				try
//					{
//					using (var bvConn = BVDB.connect(dsn))
//						{
//						if (Active)
//							{
//							var dt = BVDB.getSQL_dt(bvConn, @"Select * from sales_tax  where s_tax_no =?", new object[] { Id });
//							if (dt.Rows.Count > 0)
//								{
//								BVDB.doSQL_void(bvConn, @"
//UPDATE 
//	sales_tax 
//SET 
//	name = ?,
//	rate = ?,
//	gl_account = ?,
//	tax_inc_pricing = 0,
//	short_name = ?, 
//	compound_tax = 0, 
//	supply_and_install = 0,
//	base_tax_no = 0,
//	cap_amount = 0.00 
//WHERE 
//	s_tax_no = ? ", new object[] { Name, Percentage, GLAccount, Id, Id.ToString().PadLeft(4, '0') });
//								}
//							else
//								{
//								BVDB.doSQL_void(bvConn, @"
//INSERT INTO sales_tax 
//	(
//	s_tax_no,
//	name,
//	rate,
//	gl_account,
//	tax_inc_pricing,
//	short_name,
//	compound_tax,
//	supply_and_install,
//	base_tax_no,
//	cap_amount
//	) 
//VALUES 
//	(
//	?,
//	?,
//	?,
//	?,
//	0,
//	?,
//	0,
//	0,
//	0,
//	0.00
//	)", new object[] { Id.ToString().PadLeft(4, '0'), Name, Percentage, GLAccount, Name });
//								}
//							}
//						}
//					}
//				catch (Exception ee)
//					{
//					error += " couldnt save to " + comp.ddl_name + "\n";
//					}
//				}
//			if (error != "")
//				{
//				throw new Exception(error);
//				}
//			return true;
//			}

//        public bool Save_to_bv_from_MySQL(string dsn)
//        {
            
//               var is_consol = true;
                
//                    using (var bvConn = BVDB.connect(dsn))
//                    {
//                        if (Active)
//                        {
//                            var dt = BVDB.getSQL_dt(bvConn, @"Select * from sales_tax  where s_tax_no =?", new object[] { Id });
//                            if (dt.Rows.Count > 0)
//                            {
//                                BVDB.doSQL_void(bvConn, @"
//UPDATE 
//	sales_tax 
//SET 
//	name = ?,
//	rate = ?,
//	gl_account = ?,
//	tax_inc_pricing = 0,
//	short_name = ?, 
//	compound_tax = 0, 
//	supply_and_install = 0,
//	base_tax_no = 0,
//	cap_amount = 0.00 
//WHERE 
//	s_tax_no = ? ", new object[] { Name, Percentage, GLAccount, Id, Id.ToString().PadLeft(4, '0') });
//                            }
//                            else
//                            {
//                                BVDB.doSQL_void(bvConn, @"
//INSERT INTO sales_tax 
//	(
//	s_tax_no,
//	name,
//	rate,
//	gl_account,
//	tax_inc_pricing,
//	short_name,
//	compound_tax,
//	supply_and_install,
//	base_tax_no,
//	cap_amount
//	) 
//VALUES 
//	(
//	?,
//	?,
//	?,
//	?,
//	0,
//	?,
//	0,
//	0,
//	0,
//	0.00
//	)", new object[] { Id.ToString().PadLeft(4, '0'), Name, Percentage, GLAccount, Name });
//                            }
//                        }
//                    }
                
                       
//            return true;
//        }


    }
}