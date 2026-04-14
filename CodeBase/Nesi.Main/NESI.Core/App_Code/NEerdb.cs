using System;
using System.Data;
using System.Collections;
using System.Data.OleDb;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NeQuotes
	/// </summary>
	public class NeERdb
	{

		private int _ERJobID;
		private string _txtMake;
		private string _txtModel;
		private string _price;
		private string _broughtinDate;
		private int _erjob_id;
		private int _erjob_product_id;
		private int _erjob_customer_id;
		private int _erjob_woprog_id;
		private DateTime _erjob_PODate;
		private string _erjob_SerialNumber;
		private double _erjob_QuotePrice;
		private int _erjob_Currency;
		private double _erjob_ESPrice;
		private double _erjob_NewPrice;
		private DateTime _erjob_DateReceived;
		private DateTime _erjob_DateQuoted;
		private string _erjob_WorkPerformed;
		private DateTime _erjob_DateShipped;
		private string _erjob_ShippedVia;
		private DateTime _erjob_DateEntered;
		private string _erjob_JobNotes;
		private string _erjob_status;
		private string _FinalResult;
		private int _erjob_ContactID;
		private int _erjob_Warranty;
		private int _erjob_address_id;
		private int _erjob_quoterev;


		public int ERJobID
		{
			get { return _ERJobID; }

		}
		public string txtMake
		{
			get { return _txtMake; }
		}

		public string txtModel
		{
			get { return _txtModel; }
		}
		public string price
		{
			get { return _price; }
			set { _price = value; }
		}
		public string broughtinDate
		{
			get { return _broughtinDate; }
			set { _broughtinDate = value; }
		}

		public int erjob_id
		{
			get { return _erjob_id; }
			set { _erjob_id = value; }
		}
		public int erjob_product_id
		{
			get { return _erjob_product_id; }
			set { _erjob_product_id = value; }
		}
		public int erjob_customer_id
		{
			get { return _erjob_customer_id; }
			set { _erjob_customer_id = value; }
		}
		public int erjob_woprog_id
		{
			get { return _erjob_woprog_id; }
			set { _erjob_woprog_id = value; }
		}
		public DateTime erjob_PODate
		{
			get { return _erjob_PODate; }
			set { _erjob_PODate = value; }
		}
		public string erjob_SerialNumber
		{
			get { return _erjob_SerialNumber; }
			set { _erjob_SerialNumber = value; }
		}
		public double erjob_QuotePrice
		{
			get { return _erjob_QuotePrice; }
			set { _erjob_QuotePrice = value; }
		}

		public int erjob_Currency
		{
			get { return _erjob_Currency; }
			set { _erjob_Currency = value; }
		}

		public double erjob_ESPrice
		{
			get { return _erjob_ESPrice; }
			set { _erjob_ESPrice = value; }
		}
		public double erjob_NewPrice
		{
			get { return _erjob_NewPrice; }
			set { _erjob_NewPrice = value; }
		}
		public DateTime erjob_DateReceived
		{
			get { return _erjob_DateReceived; }
			set { _erjob_DateReceived = value; }
		}
		public DateTime erjob_DateQuoted
		{
			get { return _erjob_DateQuoted; }
			set { _erjob_DateQuoted = value; }
		}
		public string erjob_WorkPerformed
		{
			get { return _erjob_WorkPerformed; }
			set { _erjob_WorkPerformed = value; }
		}
		public DateTime erjob_DateShipped
		{
			get { return _erjob_DateShipped; }
			set { _erjob_DateShipped = value; }
		}
		public DateTime erjob_DateEntered
		{
			get { return _erjob_DateEntered; }
			set { _erjob_DateEntered = value; }
		}
		public string erjob_ShippedVia
		{
			get { return _erjob_ShippedVia; }
			set { _erjob_ShippedVia = value; }
		}
		public string erjob_status
		{
			get { return _erjob_status; }
			set { _erjob_status = value; }
		}
		public string FinalResult
		{
			get { return _FinalResult; }
			set { _FinalResult = value; }
		}
		public int erjob_ContactID
		{
			get { return _erjob_ContactID; }
			set { _erjob_ContactID = value; }
		}
		public int erjob_Warranty
		{
			get { return _erjob_Warranty; }
			set { _erjob_Warranty = value; }
		}
		public int erjob_address_id
		{
			get { return _erjob_address_id; }
			set { _erjob_address_id = value; }
		}
		public string erjob_JobNotes
		{
			get { return _erjob_JobNotes; }
			set { _erjob_JobNotes = value; }
		}
		public int erjob_quoterev
		{
			get { return _erjob_quoterev; }
			set { _erjob_quoterev = value; }
		}


		public NeERdb()
		{
			//
			//  
			//
		}

		public DataSet GetERJobsforThisCustomer(string CustBVNo)
		{
			//        OdbcConnection conn = NeDB.getQuotesAccessCon();

			var conn = NeDB.getConAccess(@"\\ne-vserver-04\branches\neoakville\new electric software\ERLC\ElabData.mdb");
			var sqljobs = "SELECT tbljobs.JobID, tbljobs.WorkOrder, tbljobs.QuotedPrice, tbljobs.Div, tbljobs.BVNOCAN, tbljobs.BVNOUSA, tbljobs.BVNONES, tbljobs.DateReceived";
			sqljobs += " FROM tbljobs WHERE ((tbljobs.chkComplete=False) AND (tbljobs.DateShipped Is Null) AND ((tbljobs.WorkOrder = '0') or (tbljobs.WorkOrder Is Null) or (tbljobs.WorkOrder = '')) AND (tbljobs.Dead=False) and ((tbljobs.bvnocan = '" + CustBVNo + "') or (tbljobs.bvnousa = '" + CustBVNo + "') or (tbljobs.bvnones='" + CustBVNo + "')) )";


			var dsERJobs = new DataSet();

			//        OdbcDataAdapter adptCustomers = new OdbcDataAdapter(strCustomers, conn);
			var adptERJobs = new OleDbDataAdapter(sqljobs, conn);
			try
			{
				adptERJobs.Fill(dsERJobs);
			}
			catch (OleDbException)
			{
				throw;
			}

			conn.Close();
			return dsERJobs;


		}


		public NeERdb(int ERJobID)
		{
			//        OdbcConnection conn = NeDB.getQuotesAccessCon();
			if (ERJobID > 100000)
			{
				var conn = NeDB.getConAccess(@"\\ne-vserver-04\branches\neoakville\new electric software\ERLC\ElabData.mdb");
				var strWO = "SELECT tbljobs.JobID, tbljobs.WorkOrder, tbljobs.JobID, tblProductManufacturers.ProductManufacturer, tblProducts.ProductNumber, tbljobs.QuotedPrice, tbljobs.DateReceived ";
				strWO += "FROM (tbljobs INNER JOIN tblProducts ON tbljobs.ProductID = tblProducts.ProductID) INNER JOIN tblProductManufacturers ON tblProducts.ProductManufacturerID = tblProductManufacturers.ProductManufacturerID ";
				strWO += "WHERE (((tbljobs.JobID)=" + ERJobID + "))";
				//        OdbcCommand comWODetailList = new OdbcCommand(strWO, conn);
				var comWODetailList = new OleDbCommand(strWO, conn);
				//        OdbcDataReader drWODetailList = comWODetailList.ExecuteReader();
				var drWODetailList = comWODetailList.ExecuteReader();
				var list = new ArrayList();

				while (drWODetailList.Read())
				{

					_ERJobID = Convert.ToInt32(drWODetailList.GetValue(0).ToString());
					_txtMake = drWODetailList.GetValue(1).ToString();
					_txtModel = drWODetailList.GetValue(2).ToString();
					_price = drWODetailList.GetValue(3).ToString();
					_broughtinDate = drWODetailList.GetValue(4).ToString();

				}
				conn.Close();
			}
			else
			{
				var _tools = new Toolbox();
				var dt = Toolbox.doSQL_dt(@"Select * from erjob  where erjob_id = @v0", new object[] { ERJobID });

				foreach (DataRow dr in dt.Rows)
				{
					_erjob_id = Convert.ToInt32(dr["erjob_id"]);
					_erjob_ContactID = Convert.ToInt32(dr["erjob_ContactID"]);
					_erjob_Currency = Convert.ToInt32(dr["erjob_Currency"]);
					_erjob_customer_id = Convert.ToInt32(dr["erjob_customer_id"]);
					_erjob_DateEntered = Convert.ToDateTime(dr["erjob_DateEntered"]);
					_erjob_DateQuoted = Convert.ToDateTime(dr["erjob_DateQuoted"]);
					_erjob_DateReceived = Convert.ToDateTime(dr["erjob_DateReceived"]);
					_erjob_DateShipped = Convert.ToDateTime(dr["erjob_DateShipped"]);
					_erjob_ESPrice = Convert.ToDouble(dr["erjob_ESPrice"]);
					_erjob_JobNotes = dr["erjob_JobNotes"].ToString();
					_erjob_NewPrice = Convert.ToDouble(dr["erjob_NewPrice"]);
					_erjob_PODate = Convert.ToDateTime(dr["erjob_PODate"]);
					_erjob_product_id = Convert.ToInt32(dr["erjob_item_id"]);
					_erjob_QuotePrice = Convert.ToDouble(dr["erjob_quotedprice"]);
					_erjob_SerialNumber = dr["erjob_SerialNumber"].ToString();
					_erjob_ShippedVia = dr["erjob_ShippedVia"].ToString();
					_erjob_status = dr["erjob_status"].ToString();
					_erjob_Warranty = Convert.ToInt32(dr["erjob_Warranty"]);
					_erjob_woprog_id = Convert.ToInt32(dr["erjob_woprog_id"]);
					_erjob_WorkPerformed = dr["erjob_WorkPerformed"].ToString();
					_erjob_address_id = Convert.ToInt32(dr["erjob_address_id"].ToString());
					_erjob_quoterev = Convert.ToInt32(dr["erjob_quoterev"]);
				}


			}


		}
		public bool ReceiveERJobfromWO(int JobID, int WO, string txtpo, int member_id, int contactid, string div)
		{
			// update the quote table
			var membertemp = new NeMember(member_id);
			var tempcontact = new NEContact(Convert.ToInt32(contactid));

			if (JobID > 100000)
			{
				var conn = NeDB.getConAccess(@"\\ne-vserver-04\branches\neoakville\new electric software\ERLC\ElabData.mdb");
				var strWO = "Update tblJobs set tblJobs.Workorder = '" + WO.ToString().PadLeft(10, '0') + "', tblJobs.CustomerPurchaseOrderNumber = '" + txtpo + "', tbljobs.ContactID = " + tempcontact.Contact_QuoteMDB_ID + ",tbljobs.Div = '" + div + "' where tbljobs.jobid = " + JobID;
				var comWODetailList = new OleDbCommand(strWO, conn);
				comWODetailList.ExecuteNonQuery();


				var strSQL = "INSERT INTO tblJobHistory (JobID,[Action],[User]) Select " + Convert.ToInt32(JobID) + " as Expr1,'Work Order: " + WO + " assigned to job' as Expr2,'" + membertemp.emplogon + "' as Expr3";

				comWODetailList = new OleDbCommand(strSQL, conn);
				comWODetailList.ExecuteNonQuery();



				// update the contact history table

				conn.Close();
				return true;
			}
			else
			{
				var _tools = new Toolbox();
				try
				{
					_tools.getSQL_void(@"Update erjob_status = 'Converted', erjob_PODate = curdate(),erjob_woprog_id =@v0 where erjob_id =@v1", new object[] { WO, JobID });
				}
				catch (Exception ee)
				{
					throw new Exception(ee.Message);
				}
				return true;


			}

		}

		public bool UnReceiveERJobfromWO(int JobID, int WO, string txtpo, int member_id, int contactid, string div)
		{
			// update the quote table
			if (JobID > 100000)
			{

				var membertemp = new NeMember(member_id);
				var comptemp = new NeBusinessUnit(membertemp.business_unit_id);
				var tempcontact = new NEContact(Convert.ToInt32(contactid));
				var conn = NeDB.getConAccess(@"\\ne-vserver-04\branches\neoakville\new electric software\ERLC\ElabData.mdb");
				var strWO = "Update tblJobs set tblJobs.Workorder = '0', tblJobs.CustomerPurchaseOrderNumber = '', tbljobs.ContactID = 0,tbljobs.Div = '" + div + "' where tbljobs.jobid = " + JobID;
				var comWODetailList = new OleDbCommand(strWO, conn);
				comWODetailList.ExecuteNonQuery();

				var strSQL = "INSERT INTO tblJobHistory (JobID,[Action],[User]) Select " + Convert.ToInt32(JobID) + " as Expr1,'Work Order: " + WO + " Removed from WO ' as Expr2,'" + membertemp.emplogon + "' as Expr3";

				comWODetailList = new OleDbCommand(strSQL, conn);
				comWODetailList.ExecuteNonQuery();



				// update the contact history table

				conn.Close();
				return true;
			}

			else
			{
				var _tools = new Toolbox();
				try
				{
					_tools.getSQL_void(@"Update erjob_status = '', erjob_PODate = null,erjob_woprog_id = 0 where erjob_id = @v0", new object[] { JobID });
				}
				catch (Exception ee)
				{
					throw new Exception(ee.Message);
				}
				return true;


			}
		}
		public string Save_job()
		{
			var _tools = new Toolbox();
			var id = erjob_id.ToString();
			if (_erjob_id == 0)
			{
				try
				{

					var sql = @"Insert into erjob
(erjob_ContactID,erjob_address_id,erjob_Currency,erjob_customer_id,erjob_DateEntered,
erjob_DateQuoted,erjob_DateReceived,erjob_DateShipped,erjob_ESPrice,
erjob_JobNotes,erjob_NewPrice,erjob_PODate,erjob_item_id,
erjob_QuotedPrice,erjob_SerialNumber,erjob_ShippedVia,erjob_status,
erjob_Warranty,erjob_woprog_id,erjob_WorkPerformed)
Values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19)";

					var paramObjects =
					new object[]
					{
						_erjob_ContactID, //0
						_erjob_address_id, //1
						_erjob_Currency, //2
						_erjob_customer_id, //3
						_erjob_DateEntered.ToString("yyyy-MM-dd"), //4
						_erjob_DateQuoted.ToString("yyyy-MM-dd"), //5
						_erjob_DateReceived.ToString("yyyy-MM-dd"), //6
						_erjob_DateShipped.ToString("yyyy-MM-dd"), //7
						_erjob_ESPrice, //8
						_tools.value_to(_erjob_JobNotes), //9
						_erjob_NewPrice, //10
						_erjob_PODate.ToString("yyyy-MM-dd"), //11
						_erjob_product_id, //12
						_erjob_QuotePrice, //13
						_tools.value_to(_erjob_SerialNumber), //14
						_erjob_ShippedVia, //15
						"Waiting to be Quoted", //16
						_erjob_Warranty, //17
						_erjob_woprog_id, //18
						_tools.value_to(_erjob_WorkPerformed) //19
					};
					id = _tools.returnSQL_id(sql, paramObjects);


				}
				catch (Exception ee)
				{
					throw new Exception(ee.Message);
				}
				return id;
			}
			else
			{
				try
				{

					var sql = @"Update erjob set
erjob_ContactID = @v0,
erjob_address_id =@v1,
erjob_Currency=@v2,
erjob_customer_id=@v3,
erjob_DateEntered=@v4,
erjob_DateQuoted=@v5,
erjob_DateReceived=@v6,
erjob_DateShipped=@v7,
erjob_ESPrice=@v8,
erjob_JobNotes=@v9,
erjob_NewPrice=@v10,
erjob_PODate=@v11,
erjob_item_id=@v12,
erjob_QuotedPrice=@v13,
erjob_SerialNumber=@v14,
erjob_ShippedVia=@v15,
erjob_status=@v16,
erjob_Warranty=@v17,
erjob_woprog_id=@v18,
erjob_WorkPerformed=@v19 
where erjob_id =@v20";

					var paramObjects = new object[]
					{
						_erjob_ContactID, //0
						_erjob_address_id, //1
						_erjob_Currency, //2
						_erjob_customer_id, //3
						_erjob_DateEntered.ToString("yyyy-MM-dd"), //4
						_erjob_DateQuoted.ToString("yyyy-MM-dd"), //5
						_erjob_DateReceived.ToString("yyyy-MM-dd"), //6 
						_erjob_DateShipped.ToString("yyyy-MM-dd"), //7
						_erjob_ESPrice, //8
						_tools.value_to(_erjob_JobNotes), //9
						_erjob_NewPrice, //10
						_erjob_PODate.ToString("yyyy-MM-dd"), //11
						_erjob_product_id, //12
						_erjob_QuotePrice, //13
						_tools.value_to(_erjob_SerialNumber), //14
						_erjob_ShippedVia, //15
						_erjob_status, //16
						_erjob_Warranty, //17
						_erjob_woprog_id, //18
						_tools.value_to(_erjob_WorkPerformed), //19
						erjob_id //20
					};
					_tools.getSQL_void(sql, paramObjects);
				}
				catch (Exception ee1)
				{
					throw new Exception(ee1.Message);
				}
				return id;
			}

		}


	}
}