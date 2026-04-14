using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
//using nesi.bv;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Web.Configuration;
using ne_xpo.cs;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NEVendor
	/// </summary>
	public class NEVendor
		{
		#region private accessors
		private int _Vendor_ID;
		private string _vendor_number = "";
		private int _Vendor_Number_Int;
		private string _Vendor_Name = "";
		private int _Vendor_GL_ID;
		private int _Vendor_GL_ID_Consol;
		private int _Vendor_Credit_Type;
		private double _Vendor_Credit_Limit	= 0;
		private string _Vendor_Hold = "T";
		private string _Vendor_CPRS = "T";
		private string _Vendor_IDType = "E";
		private string _Vendor_IDNumber = "";
		private string _Vendor_Account = "";
		private string _Vendor_Notes = "";
		private string _Vendor_Buyer = "";
		private int _Vendor_Term_ID;
		private DateTime _Vendor_CreatedDateTime = DateTime.Now;
		private int _Vendor_InitMember_ID;
		private DateTime _Vendor_LastDateTime = DateTime.Now;
		private int _Vendor_Member_ID;
		private int _Vendor_QC_Member_ID;
		private DateTime _Vendor_QC_DateTime = DateTime.Now;
		private int _Vendor_PO_Exempt;
		private int _Vendor_Active;
		private Toolbox _tools = new Toolbox();
		private NEAddress _Address = new NEAddress();
		private NEAddress[] _ShippingAddresses;
		private bool _is_partner;
        private int _nesi_member_id;


        #endregion private accessors
        #region public accessors
        public int Vendor_ID { set { _Vendor_ID = value; } get { return _Vendor_ID; } }
		public string vendor_number { set { _vendor_number = value; } get { return _vendor_number; } }
		public int Vendor_Number_Int { set { _Vendor_Number_Int = value; } get { return _Vendor_Number_Int; } }
		public string Vendor_Name { set { _Vendor_Name = value; } get { return _Vendor_Name; } }
		public int Vendor_GL_ID { set { _Vendor_GL_ID = value; } get { return _Vendor_GL_ID; } }
		public int Vendor_GL_ID_Consol { set { _Vendor_GL_ID_Consol = value; } get { return _Vendor_GL_ID_Consol; } }
		public int Vendor_Credit_Type { set { _Vendor_Credit_Type = value; } get { return _Vendor_Credit_Type; } }
		public double Vendor_Credit_Limit { set { _Vendor_Credit_Limit = value; } get { return _Vendor_Credit_Limit; } }
		public string Vendor_Hold { set { _Vendor_Hold = value; } get { return _Vendor_Hold; } }
		public string Vendor_CPRS { set { _Vendor_CPRS = value; } get { return _Vendor_CPRS; } }
		public string Vendor_IDType { set { _Vendor_IDType = value; } get { return _Vendor_IDType; } }
		public string Vendor_IDNumber { set { _Vendor_IDNumber = value; } get { return _Vendor_IDNumber; } }
		public string Vendor_Account { set { _Vendor_Account = value; } get { return _Vendor_Account; } }
		public string Vendor_Notes { set { _Vendor_Notes = value; } get { return _Vendor_Notes; } }
		public string Vendor_Buyer { set { _Vendor_Buyer = value; } get { return _Vendor_Buyer; } }
		public int Vendor_Term_ID { set { _Vendor_Term_ID = value; } get { return _Vendor_Term_ID; } }
		public DateTime Vendor_CreatedDateTime { set { _Vendor_CreatedDateTime = value; } get { return _Vendor_CreatedDateTime; } }
		public int Vendor_InitMember_ID { set { _Vendor_InitMember_ID = value; } get { return _Vendor_InitMember_ID; } }
		public DateTime Vendor_LastDateTime { set { _Vendor_LastDateTime = value; } get { return _Vendor_LastDateTime; } }
		public int Vendor_Member_ID { set { _Vendor_Member_ID = value; } get { return _Vendor_Member_ID; } }
		public int Vendor_QC_Member_ID { set { _Vendor_QC_Member_ID = value; } get { return _Vendor_QC_Member_ID; } }
		public DateTime Vendor_QC_DateTime { set { _Vendor_QC_DateTime = value; } get { return _Vendor_QC_DateTime; } }
		public int Vendor_PO_Exempt { set { _Vendor_PO_Exempt = value; } get { return _Vendor_PO_Exempt; } }
		public int Vendor_Active { set { _Vendor_Active = value; } get { return _Vendor_Active; } }
		public NEAddress Address { get { return _Address; } set { _Address = value; } }
	
		public int id { set { _Vendor_ID = value; } get { return _Vendor_ID; } }
		public int ID { set { _Vendor_ID = value; } get { return _Vendor_ID; } }
		public string Number { set { _vendor_number = value; } get { return _vendor_number; } }
		public int Number_Int { set { _Vendor_Number_Int = value; } get { return _Vendor_Number_Int; } }
		public string Name { set { _Vendor_Name = value; } get { return _Vendor_Name; } }
		public int business_unit_id {get; set; }
		//public int GL_ID { set { _Vendor_GL_ID = value; } get { return _Vendor_GL_ID; } }
		//public int GL_ID_Consol { set { _Vendor_GL_ID_Consol = value; } get { return _Vendor_GL_ID_Consol; } }
		public int Credit_Type { set { _Vendor_Credit_Type = value; } get { return _Vendor_Credit_Type; } }
		public double Credit_Limit { set { _Vendor_Credit_Limit = value; } get { return _Vendor_Credit_Limit; } }
		public bool Hold { set { _Vendor_Hold = value.ToString().Substring(0,1); } get { return (_Vendor_Hold == "T"); } }
		public bool CPRS { set { _Vendor_CPRS = value.ToString().Substring(0,1); } get { return (_Vendor_CPRS == "T"); } }
		public string IDType { set { _Vendor_IDType = value; } get { return _Vendor_IDType; } }
		public string IDNumber { set { _Vendor_IDNumber = value; } get { return _Vendor_IDNumber; } }
		public string Account { set { _Vendor_Account = value; } get { return _Vendor_Account; } }
		public string Notes { set { _Vendor_Notes = value; } get { return _Vendor_Notes; } }
		public string Buyer { set { _Vendor_Buyer = value; } get { return _Vendor_Buyer; } }
		public int Term_ID { set { _Vendor_Term_ID = value; } get { return _Vendor_Term_ID; } }
		public DateTime CreatedDateTime { set { _Vendor_CreatedDateTime = value; } get { return _Vendor_CreatedDateTime; } }
		public int InitMember_ID { set { _Vendor_InitMember_ID = value; } get { return _Vendor_InitMember_ID; } }
		public DateTime LastDateTime { set { _Vendor_LastDateTime = value; } get { return _Vendor_LastDateTime; } }
		public int Member_ID { set { _Vendor_Member_ID = value; } get { return _Vendor_Member_ID; } }
		public int QC_Member_ID { set { _Vendor_QC_Member_ID = value; } get { return _Vendor_QC_Member_ID; } }
		public DateTime QC_DateTime { set { _Vendor_QC_DateTime = value; } get { return _Vendor_QC_DateTime; } }
		public bool PO_Exempt  { set { _Vendor_PO_Exempt = Convert.ToInt32(value); } get { return Convert.ToBoolean(_Vendor_PO_Exempt); } }
		public bool Active  { set { _Vendor_Active = Convert.ToInt32(value); } get { return Convert.ToBoolean(_Vendor_Active); } }
		public NEAddress[] ShippingAddresses { get { return _ShippingAddresses; } set { _ShippingAddresses = value; } }
		public DateTime ts { get; set;}
		public bool is_partner { get { return _is_partner; } set { _is_partner = value; } }
        public int nesi_member_id { get { return _nesi_member_id; } set { _nesi_member_id = value; } }

        #endregion public accessors

        public NEVendor(){}
		public NEVendor(int ID)
			{
			using (var myConn = Toolbox.connect())
				{
				Load(myConn, ID);
				}
			}
		public NEVendor(MySqlConnection _myConn, int ID)
			{
			Load(_myConn, ID);
			}
		//public void sync_bvs(NeMember _member)
		//	{
		//
		//	//		ThreadPool.QueueUserWorkItem(delegate { background_sync(_member); });
		//	try
		//		{
		//		background_sync(_member);
		//		}
		//	catch (Exception ee)
		//		{
		//
		//		throw ee;
		//		//	_tools.debug_note(String.Format("({0}) tried to sync ({1}) for this BV ({2}) but failed with this error: {3}", _user.FullName, ID, _dsn, ee));
		//
		//		}
		//	
		//	}
		//private void background_sync(NeMember _user)
		//	{
		//	using (var uow = new UnitOfWork())
		//		{
		//		var taxEntities = (from te in new XPQuery<ne_xpo.cs.tax_entity>(uow)
		//			where te.is_active // && te.sync_vendors
		//			select te).ToList();
		//
		//		Parallel.ForEach(taxEntities, new ParallelOptions { MaxDegreeOfParallelism = 5 }, _dr =>
		//			{
		//			    if (_dr.sync_vendors)
		//			        {
		//			        using (var bv_conn = BVDB.connect(_dr.dsn))
		//			            {
		//			            using (var conn = Toolbox.connect())
		//			                {
		//			                var te = new NeTaxEntity(_dr.id);
		//			                sync_bv(bv_conn, conn, te, _user, true);
		//			                }
		//			            }
        //                }					    
		//			 });
		//		}
		//	}

        //public static void sync_bv_from_mysql(string _dsn, NeMember _member)
        //{
        //    using (var uow = new UnitOfWork())
        //    {
        //        var vendorsList = (from vend in new XPQuery<vendor>(uow)
        //                           select vend.vendor_id).OrderBy(x => x)
        //            .ToList();
		//
        //        var te_id = Toolbox.doSQL_int("SELECT IFNULL((SELECT id FROM tax_entity WHERE dsn = @v0  LIMIT 1),0)", new object[] { _dsn });
        //        var te = new NeTaxEntity(te_id);
		//
        //        Parallel.ForEach(vendorsList, new ParallelOptions { MaxDegreeOfParallelism = 8 }, _id =>
        //        {
        //            var bv_conn = new PsqlConnection(BVDB.ConStr +
        //                                                    ";Min Pool Size=0;Max Pool Size=100;Pooling=true;dbq=" +
        //                                                    _dsn);
		//
        //            bv_conn.Open();
        //            var my_conn = new MySqlConnection(Toolbox.str_connection_string +
        //                                    "pooling=true;Min Pool Size=0;Max Pool Size=100;");
		//
        //            my_conn.Open();
        //           
        //            try
        //            {
        //                var v = new NEVendor(_id);
        //                v.sync_bv(bv_conn, my_conn, te, _member, true);
        //            }
        //            catch (Exception ee)
        //            {
        //                Toolbox.do_errorLog(ee);
        //            }
        //            finally
        //            {
        //                my_conn.Close();
        //                bv_conn.Close();                        
        //            }
		//
		//
        //        });
        //    }
        //}

		//public void sync_bv(PsqlConnection _bvConn, MySqlConnection _myConn, NeTaxEntity _taxEntity, NeMember _user, bool is_consol)
		//	{
		//	if (ID == 0)
		//		{
		//		throw new Exception("Vendor not loaded");
		//		}
		//	try
		//		{			
		//		var c = BVDB.getSQL_int(_bvConn, @"SELECT COUNT(*) FROM VENDOR WHERE VEN_NO = ? ", new object[] {  Number } );
		//		var isNew = (c == 0);
		//		var objVendor = new Vendor
		//								{
		//								VendorNumber = Number,
		//								VendorName = Name.Trim(),
		//								CreditType = Credit_Type,
		//								CreditLimit = Credit_Limit > 0 ? Credit_Limit : 0,
		//								Hold = Hold,
		//								CPRS = CPRS,
		//								IDType = IDType,
		//								IDNumber = IDNumber.Trim(),
		//								VendorAddress = {Account = is_consol ? Address.RVAccount_Consol : Address.RVAccount},
		//								TermsCode = Toolbox.doSQL_string(_myConn, @"select ifnull((SELECT CAST(IFNULL(MAX(term_code), '') AS CHAR) FROM term WHERE term_id =@v0 ),'')", new object[] {  Term_ID } ),
		//								Notes = Notes.Trim(),
		//								Buyer = Buyer.Trim(),
		//								APAccount = _taxEntity.gl_default_expense == null ? "" : _taxEntity.gl_default_expense.account_no,
		//								AccountNumber = Account // NOT THE GL ACCOUNT #
		//								};
		//		// account number from vendor not gl
		//		objVendor.VendorAddress.Address1 = Address.Addr1;
		//		objVendor.VendorAddress.Address2 = Address.Addr2;
		//		objVendor.VendorAddress.Address3 = Address.Addr3;
		//		objVendor.VendorAddress.Address4 = Address.Addr4;
		//		objVendor.VendorAddress.Account	= objVendor.APAccount;
		//		objVendor.VendorAddress.City = Address.City;
		//		objVendor.VendorAddress.Province = Address.Prov;
		//		objVendor.VendorAddress.PostalCode = Address.Postal;
		//		objVendor.VendorAddress.CountryCode = Address.Country;
		//		objVendor.VendorAddress.PhoneNumber = Address.PhoneArea + Address.Phonefirst + Address.PhoneLast + Address.PhoneExt;
		//		objVendor.VendorAddress.FaxNumber = Address.FaxArea + Address.FaxFirst + Address.FaxLast;
		//		objVendor.VendorAddress.EmailAddress = Address.Email;
		//		objVendor.VendorAddress.Website = Address.Web;
		//		objVendor.VendorAddress.Contact1Name = string.IsNullOrEmpty(Address.BVContact1.Name) ? "" : Address.BVContact1.Name.Trim();
		//		objVendor.VendorAddress.Contact1PhoneNumber = string.IsNullOrEmpty(Address.BVContact1.Phone_Area) ? "" : Address.BVContact1.Phone_Area + Address.BVContact1.Phone_First + Address.BVContact1.Phone_Area + //Address.BVContact1.Phone_Ext;
		//		objVendor.VendorAddress.Contact1FaxNumber = string.IsNullOrEmpty(Address.BVContact1.Fax_Area) ? "" : Address.BVContact1.Fax_Area + Address.BVContact1.Fax_First + Address.BVContact1.Fax_Area;
		//		objVendor.VendorAddress.Contact1EmailAddress = string.IsNullOrEmpty(Address.BVContact1.Email) ? "" : Address.BVContact1.Email;
		//		objVendor.VendorAddress.Contact2Name = string.IsNullOrEmpty(Address.BVContact2.Name) ? "" : Address.BVContact2.Name.Trim();
		//		objVendor.VendorAddress.Contact2PhoneNumber = string.IsNullOrEmpty(Address.BVContact2.Phone_Area) ? "" : Address.BVContact2.Phone_Area + Address.BVContact2.Phone_First + Address.BVContact2.Phone_Area + //Address.BVContact2.Phone_Ext;
		//		objVendor.VendorAddress.Contact2FaxNumber = string.IsNullOrEmpty(Address.BVContact2.Fax_Area) ? "" : Address.BVContact2.Fax_Area + Address.BVContact2.Fax_First + Address.BVContact2.Fax_Area;
		//		objVendor.VendorAddress.Contact2EmailAddress = string.IsNullOrEmpty(Address.BVContact2.Email) ? "" : Address.BVContact2.Email;
		//		objVendor.VendorAddress.Contact3Name = string.IsNullOrEmpty(Address.BVContact3.Name) ? "" : Address.BVContact3.Name.Trim();
		//		objVendor.VendorAddress.Contact3PhoneNumber = string.IsNullOrEmpty(Address.BVContact3.Phone_Area) ? "" : Address.BVContact3.Phone_Area + Address.BVContact3.Phone_First + Address.BVContact3.Phone_Area + //Address.BVContact3.Phone_Ext;
		//		objVendor.VendorAddress.Contact3FaxNumber = string.IsNullOrEmpty(Address.BVContact3.Fax_Area) ? "" : Address.BVContact3.Fax_Area + Address.BVContact3.Fax_First + Address.BVContact3.Fax_Area;
		//		objVendor.VendorAddress.Contact3EmailAddress = string.IsNullOrEmpty(Address.BVContact3.Email) ? "" : Address.BVContact3.Email;
		//		objVendor.Save(_bvConn, isNew, _user.Initials);
		//		}
		//	catch (Exception ee)
		//		{
		//		var message = ee.Message;
		//		if(message.Contains("Account Not Found"))
		//			{
		//			message = message + " the account ("+Account+") may need to be created in this BV.";
		//			}
		//		throw new Exception(string.Format("[{0}] Failed with this error: {1}", _bvConn.Database, message));
		//		}
		//	}
		private void Load(int ID)
			{
			using (var myConn = Toolbox.connect())
				{
				Load(myConn, ID);
				}
			}
		private void Load(MySqlConnection myConn, int ID)
			{
				if (ID > 0)
					{
					var vr = Toolbox.doSQL_dt(myConn, @" SELECT vendor_id, vendor_number, vendor_number_int, vendor_name, business_unit_id, IFNULL(vendor_gl_id, 0) vendor_gl_id, nesi_member_id, vendor_credittype, vendor_creditlimit, vendor_hold, vendor_cprs, vendor_idtype, vendor_idnumber, vendor_account, vendor_notes, vendor_buyer, vendor_term_id, vendor_createddatetime, vendor_initmember_id, vendor_lastdatetime, vendor_member_id, IFNULL(vendor_qc_member_id, 0) vendor_qc_member_id, vendor_qc_datetime, vendor_po_exempt, vendor_active, vendor_ts, is_partner, vendor_gl_id_consol, (SELECT IFNULL(MIN(address_id), 0) FROM address WHERE Active=true and address_table = 'Vendor' AND address_table_id = a.vendor_id AND address_type = 'B') address_id FROM vendor a WHERE vendor_id = @v0 ", new object[] { ID }).Rows[0];
					Vendor_ID = Convert.ToInt32(vr["Vendor_ID"]);
					Vendor_Number_Int = Convert.ToInt32(vr["Vendor_Number_Int"]);
					business_unit_id = (int)vr["business_unit_id"];
					Vendor_GL_ID = Convert.ToInt32(vr["Vendor_GL_ID"]);
					Vendor_GL_ID_Consol = Convert.ToInt32(vr["Vendor_GL_ID_Consol"]);
					Vendor_Credit_Type = Convert.ToInt32(vr["Vendor_CreditType"]);
					Vendor_Term_ID = Convert.ToInt32(vr["Vendor_Term_ID"]);
					Vendor_InitMember_ID = Convert.ToInt32(vr["Vendor_InitMember_ID"]);
					Vendor_Member_ID = Convert.ToInt32(vr["Vendor_Member_ID"]);
					Vendor_QC_Member_ID = Convert.ToInt32(vr["Vendor_QC_Member_ID"]);
					Vendor_PO_Exempt = Convert.ToInt32(vr["Vendor_PO_Exempt"]);
					vendor_number = vr["vendor_number"].ToString();
					Vendor_Name = vr["Vendor_Name"].ToString();
					Vendor_IDNumber = vr["Vendor_IDNumber"].ToString();
					Vendor_Account = vr["Vendor_Account"].ToString();
					Vendor_Notes = vr["Vendor_Notes"].ToString();
					Vendor_Buyer = vr["Vendor_Buyer"].ToString();
					Vendor_Hold = vr["Vendor_Hold"].ToString();
					Vendor_CPRS = vr["Vendor_CPRS"].ToString();
					Vendor_IDType = vr["Vendor_IDType"].ToString();
					Vendor_Credit_Limit = Convert.ToDouble(vr["Vendor_CreditLimit"]);
					Vendor_CreatedDateTime = Convert.ToDateTime(vr["Vendor_CreatedDateTime"]);
					Vendor_QC_DateTime = vr["Vendor_QC_DateTime"] != DBNull.Value ? Convert.ToDateTime(vr["Vendor_QC_DateTime"]) : new DateTime();
					Vendor_Active = Convert.ToInt32(vr["Vendor_Active"]);
                    nesi_member_id = Convert.ToInt32(vr["nesi_member_id"]);

                    var address_id = Convert.ToInt32(vr["address_id"]);
					if (address_id > 0)
						{
						_Address = new NEAddress(address_id);
						}
					ts = Toolbox.ReturnBlankDateTimeIfNull(vr["vendor_ts"]);
					_is_partner = Convert.ToBoolean(vr["is_partner"]);
					}
			}
		public int Save()
			{
			var isNew					= (_Vendor_ID == 0);
			if(isNew)
				{
				var new_id			= _tools.returnSQL_id(@"
INSERT INTO vendor
	(
	vendor_number_int,
	vendor_name,
	business_unit_id,
	vendor_credittype,
	vendor_creditlimit,
	vendor_hold,
	vendor_cprs,
	vendor_idtype,
	vendor_idnumber,
	vendor_account,
	vendor_notes,
	vendor_buyer,
	vendor_term_id,
	vendor_createddatetime,
	vendor_initmember_id,
	vendor_lastdatetime,
	vendor_member_id,
	vendor_po_exempt,
	vendor_active,
	is_partner,
nesi_member_id
	)
VALUES	(0,@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,NOW(),@v12,NOW(),@v13,@v14,TRUE,@v15,@v16)",
new object[] {
					Name, 								// {0}
					business_unit_id, 					// {1}
					Credit_Type, 						// {2}
					Credit_Limit, 						// {3}
					Hold.ToString().Substring(0,1), 	// {4}
					CPRS.ToString().Substring(0,1), 	// {5}
					IDType, 							// {6}
					IDNumber, 							// {7}
					Account, 							// {8}
					Notes, 								// {9}
					Buyer, 								// {10}
					Term_ID, 							// {13}
					InitMember_ID, 						// {12}
					InitMember_ID, 						// {13}
					PO_Exempt, // 14
					_is_partner, // 15
                    _nesi_member_id  //16
                    });			
				ID			= Convert.ToInt32(new_id);
				}
			else
				{
				_tools.getSQL_void(@"
UPDATE vendor
SET
	vendor_name			= @v0,
	business_unit_id	= @v1,
	vendor_credittype	= @v2,
	vendor_creditlimit	= @v3,
	vendor_hold			= @v4,
	vendor_cprs			= @v5,
	vendor_idtype		= @v6,
	vendor_idnumber		= @v7,
	vendor_account		= @v8,
	vendor_notes		= @v9,
	vendor_buyer		= @v10,
	vendor_term_id		= @v11,
	vendor_lastdatetime = NOW(),
	vendor_member_id	= @v12,
	vendor_po_exempt	= @v13,
	vendor_active		= @v14,
	is_partner			= @v16,
nesi_member_id = @v17
WHERE
	vendor_id = @v15
", 
new object[] {
Name, business_unit_id, Credit_Type, Credit_Limit, Hold.ToString().Substring(0, 1), CPRS.ToString().Substring(0, 1), IDType, IDNumber, Account, Notes, Buyer, Term_ID, Member_ID, PO_Exempt, Active, ID, _is_partner,_nesi_member_id});
				}
			return ID;
			}
		public DataTable GetCompanyVendorList(int companyid)
			{
			return Toolbox.doSQL_dt(@"SELECT Vendor_ID, Vendor_Name FROM vendor WHERE business_unit_id = @v0  ORDER BY Vendor_Name", new object[] {  companyid } );
			}

		public string GetVendorBVNO(object vendid)
			{
			return _tools.getSQL_string(@"SELECT vendor_number FROM vendor WHERE Vendor_ID = @v0 ", new object[] {  vendid } );
			}

		public void GetVendorTaxInfo()
			{
			}
		public DataTable ActiveVendorList()
			{
			return Toolbox.doSQL_dt(@"SELECT vendor_id, URLDECODE(vendor_name) vendor_name FROM vendor  WHERE URLDECODE(vendor_name) not like '%DO NOT%' AND vendor_active = true ORDER BY URLDECODE(vendor_name)" , null);
			}
		}
	public class VendorRFQ
		{
		private Toolbox _tools			= new Toolbox();
		private int _id					= 0;
		private string _name;
		private int _business_unit_id;
		private string _status;
		private int _member_id;
		private DateTime _date_created;
		private DateTime _date_open;
		private DateTime _date_close;
		private string _notes;
		private List<int> _vendor_ids	= new List<int>();
		public int id { set { _id = value; } get{ return _id; } }
		public string name { set { _name = value; } get{ return _name; } }
		public int business_unit_id { set { _business_unit_id = value; } get{ return _business_unit_id; } }
		public string status { set { _status = value; } get{ return _status; } }
		public string notes { set { _notes = value; } get{ return _notes; } }
		public int member_id { set { _member_id = value; } get{ return _member_id; } }
		public DateTime date_created { set { _date_created = value; } get{ return _date_created; } }
		public DateTime date_open { set { _date_open = value; } get{ return _date_open; } }
		public DateTime date_close { set { _date_close = value; } get{ return _date_close; } }
		public List<int> vendor_ids { set { _vendor_ids = value; } get { return _vendor_ids; } } 
		public VendorRFQ(){}
		public VendorRFQ(int _row_id)
			{
			_id			= _row_id;
			if(exists())
				{
				get();
				}
			else
				{
				throw new Exception("RFQ ID does not exist");
				}
			}
		private void get()
			{
			var rfq			= Toolbox.doSQL_dt(@"SELECT * FROM rfq_header  WHERE id =@v0 LIMIT 1", new object[] { _id }).Rows[0];
			_name				= rfq["name"].ToString();
			_business_unit_id			= (int) rfq["business_unit_id"];
			_member_id			= Convert.ToInt32(rfq["member_id"]);
			_status				= rfq["status"].ToString();
			_date_created		= Convert.ToDateTime(rfq["date_created"]);
			_date_open			= Convert.ToDateTime(rfq["date_open"]);
			_date_close			= Convert.ToDateTime(rfq["date_close"]);
			_notes				= rfq["notes"].ToString();
			}
		private bool exists()
			{
			return (_tools.getSQL_int(@"SELECT COUNT(*) FROM rfq_header  WHERE id =@v0", new object[] { _id}) > 0);
			}
		public void save()
			{
			if(_id == 0)
				{
				#region New Record
				id				= Toolbox.doSQL_return_id(@"
INSERT INTO rfq_header
	(
	name,
	business_unit_id,
	status,
	member_id,
	date_created,
	date_open,
	date_close,
	notes
	)
VALUES
	(
	@v0,
	@v1,
	'Building',
	@v2,
	NOW(),
	@v3,
	@v4,
	@v5
	)
", new object[] {

					_name,					// {0}
					_business_unit_id,							// {1}
					_member_id,								// {2}
					Toolbox.MySQL_longdt(_date_open),		// {3}
					Toolbox.MySQL_longdt(_date_close),		// {4}
					_notes					// {5}
				});
				#endregion New Record
				}
			else
				{
				#region Edit Record
				_tools.getSQL_void(@"
UPDATE rfq_header 
SET
	name			= @v0,
	status			= @v1,
	date_open		= @v2,
	date_close		= @v3,
	notes			= @v4
WHERE
	id = @v5
LIMIT 1
", new object[] {
					_name,					// {0}
					_status,								// {1}
					Toolbox.MySQL_longdt(_date_open),		// {2}
					Toolbox.MySQL_longdt(_date_close),		// {3}
					_notes,				// {4}
					_id										// {5}
				});
				#endregion Edit Record
				}
			}
		}
	public class rfq_part_list
		{
		private int? _id;
		public int? id				{get { return _id; }set { _id = value; }}
		private int _rfq_header_id;
		public int rfq_header_id	{get { return _rfq_header_id; } set { _rfq_header_id = value;}}
		private int _master_id;
		public int master_id		{get { return _master_id; } set { _master_id = value; }}
		private DateTime? _required_date;
		public DateTime? required_date {get {return _required_date; } set { _required_date = value; }}
		public bool does_exist		{ get; set; }
		private double _qty;
		public double qty			{get { return _qty; } set { _qty = value; }}
		public string note {get;set;}
		public rfq_part_list()
			{
			// Constructor
			}
		public rfq_part_list(int line_id)
			{
			id					= line_id;
			load();
			}
		public rfq_part_list(int r_id, int m_id)
			{
			rfq_header_id		= r_id;
			master_id			= m_id;
			load();
			}
		private void load()
			{
			if(exists())
				{
				DataRow dr;
				if (id != null)
					{
					dr = Toolbox.doSQL_dt(@"SELECT * FROM rfq_part_list WHERE id = @v0 ", new object[] {  id } ).Rows[0];
					}
				else
					{
					dr = Toolbox.doSQL_dt(@"SELECT * FROM rfq_part_list WHERE rfq_header_id = @v0  AND master_id = @v1  ", new object[] {  rfq_header_id, master_id } ).Rows[0];
					}

				id				= Convert.ToInt32(dr["id"]);
				master_id		= Convert.ToInt32(dr["master_id"]);
				rfq_header_id	= Convert.ToInt32(dr["rfq_header_id"]);
				required_date	= dr["required_date"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dr["required_date"]);
				qty				= Convert.ToDouble(dr["qty"]);
				note			= dr["note"].ToString();
				}
			else
				{
				throw new Exception("Doesn't Exist");
				}
			}
		public bool exists()
			{
			does_exist			= false;
			if(id != null)
				{
				does_exist		= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM rfq_part_list WHERE id = @v0 ", new object[] { id}) > 0;
				}
			else 
				{
				does_exist		= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM rfq_part_list WHERE rfq_header_id = @v0 AND master_id = @v1 ", new object[] { rfq_header_id, master_id}) > 0;
				}
			return does_exist;
			}
		public void delete()
			{
			// delete -- only via id
			Toolbox.doSQL_void(@"DELETE FROM rfq_part_list WHERE id = @v0 LIMIT 1", new object[] { id});
			id = null;
			}
		public void save()
			{
			var rq_date		= "";
			if(required_date == null || required_date < DateTime.Now)
				{
				rq_date	= "null";
				}
			else
				{
				rq_date	= "'"+Toolbox.MySQL_shortdt((DateTime) required_date)+"'";
				}
			if(exists())
				{
				// update -- only allow qty to be updated.
				Toolbox.doSQL_void(@"UPDATE rfq_part_list SET qty =@v0, required_date =@v2 WHERE id = @v1 LIMIT 1", new object[] { qty, id, rq_date});
				}
			else
				{
				// insert
				id	= Toolbox.doSQL_return_id(@"INSERT INTO rfq_part_list (rfq_header_id, master_id, qty, note, required_date) VALUES (@v0,@v1,@v2,@v3,@v4)", new object[] {
				rfq_header_id, master_id, qty, note, rq_date});
				}
			}
		}
	public class rfq_lineitem
		{
		/*
id
rfq_header_id
master_id
vendor_id
vendor_code
vendor_price
qty
lead_time
vendor_note
note
			 */
		private int? _id;
		public int? id				{get { return _id; }set { _id = value; }}
		private int _rfq_header_id;
		public int rfq_header_id	{get { return _rfq_header_id; } set { _rfq_header_id = value;}}
		private int _master_id;
		public int master_id		{get { return _master_id; } set { _master_id = value; }}
		private int _vendor_id;
		public int vendor_id		{get { return _vendor_id;}set { _vendor_id = value;}}
		private string _vendor_code;
		public string vendor_code	{get { return _vendor_code;}set { _vendor_code = value;}}
		private double _vendor_price;
		public double vendor_price	{get { return _vendor_price;}set { _vendor_price = value;}}
		private double _qty;
		public double qty			{get { return _qty; } set { _qty = value; }}
		private DateTime? _required_date;
		public DateTime? required_date {get {return _required_date; } set { _required_date = value; }}
		private int _lead_time;
		public int lead_time		{get { return _lead_time;}set { _lead_time = value;}}
		private string _vendor_note;
		public string vendor_note	{get { return _vendor_note;}set { _vendor_note = value;}}
		private string _note;
		public string note			{get { return _note;}set { _note = value;}}
		public rfq_lineitem()
			{
			// Constructor
			}
		public rfq_lineitem(int line_id)
			{
			id					= line_id;
			load();
			}
		public rfq_lineitem(int r_id, int m_id)
			{
			rfq_header_id		= r_id;
			master_id			= m_id;
			load();
			}
		public rfq_lineitem(int r_id, int m_id, int v_id)
			{
			rfq_header_id		= r_id;
			master_id			= m_id;
			vendor_id			= v_id;
			load();
			}
		private void load()
			{
			if(exists())
				{
				DataRow dr;
				if(id != null)
					{
					dr			= Toolbox.doSQL_dt(@"SELECT * FROM rfq_lineitem WHERE id = @v0 ", new object[] {  id } ).Rows[0];
					}
				else 
					{
					try
						{
						dr			= Toolbox.doSQL_dt(@"SELECT * FROM rfq_lineitem WHERE rfq_header_id = @v0  AND master_id = @v1  AND vendor_id = @v2  ", new object[] {  rfq_header_id, master_id, vendor_id } ).Rows[0];
						}
					catch
						{
						throw new Exception(string.Format("SELECT * FROM rfq_lineitem WHERE rfq_header_id = '{0}' AND master_id = '{1}' AND vendor_id = '{2}' ", rfq_header_id, master_id, vendor_id));
						}
					}

				id				= Convert.ToInt32(dr["id"]);
				master_id		= Convert.ToInt32(dr["master_id"]);
				rfq_header_id	= Convert.ToInt32(dr["rfq_header_id"]);
				qty				= Convert.ToDouble(dr["qty"]);
				vendor_price	= Convert.ToDouble(dr["vendor_price"]);
				vendor_id		= Convert.ToInt32(dr["vendor_id"]);
				vendor_code		= dr["vendor_code"].ToString();
				lead_time		= Convert.ToInt32(dr["lead_time"]);
				required_date	= dr["required_date"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dr["required_date"]);
				vendor_note		= dr["vendor_note"].ToString();
				note			= dr["note"].ToString();
				}
			}
		private bool exists()
			{
			var _isexist		= false;
			if(id != null)
				{
				_isexist		= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM rfq_lineitem WHERE id = @v0 ", new object[] { id}) > 0;
				}
			else
				{
				_isexist		= Toolbox.doSQL_int(@"SELECT COUNT(*)
FROM rfq_lineitem WHERE rfq_header_id = @v0 AND master_id = @v1 AND vendor_id = @v2 ",
new object[] { rfq_header_id, master_id, vendor_id}) > 0;
				}
			return _isexist;
			}
		public void set_merged()
			{
			Toolbox.doSQL_void(@"UPDATE rfq_lineitem SET merge_dt = NOW() WHERE id = @v0 LIMIT 1", new object[] { id});
			}
		public void save()
			{
			var rq_date		= "";
			if(required_date == null)
				{
				rq_date	= "null";
				}
			else
				{
				rq_date	= "'"+Toolbox.MySQL_shortdt((DateTime) required_date)+"'";
				}
			if(exists())
				{
				// update -- only allow qty to be updated.
				Toolbox.doSQL_void(@"
UPDATE 
	rfq_lineitem 
SET 
	qty				= @v0,
	vendor_code		= @v1,
	vendor_price	= @v2,
	qty				= @v3,
	lead_time		= @v4,
	vendor_note		= @v5,
	note			= @v6,
	required_date	= @v8
WHERE 
	id = @v7 LIMIT 1", new object[] {
					qty, 
					vendor_code,
					vendor_price,
					qty,
					lead_time,
					vendor_note,
					note,
					id,
					rq_date
				});
				}
			else
				{
				// insert
				id	= Toolbox.doSQL_return_id(@"
INSERT INTO rfq_lineitem 
	(
	rfq_header_id,
	master_id,
	qty,
	vendor_id,
	vendor_code,
	vendor_price,
	lead_time,
	vendor_note,
	note,
	required_date
	) 
VALUES 
	(
	@v0, 
	@v1, 
	@v2,
	@v3,
	@v4,
	@v5,
	@v6,
	@v7,
	@v8,
	@v9
	)", new object[] {
					rfq_header_id, 
					master_id, 
					qty,
					vendor_id,
					vendor_code,
					0,
					lead_time,
					vendor_note,
					note,
					rq_date
				});
				}
			}
			
		}
	public class rfq_vendor
		{
		private int _id;
		private int _rfq_header_id;
		private int _vendor_id;
		private int _contact_id;
		private string _keycode;
		private DateTime? _date_created;
		private DateTime? _date_opened;
		private DateTime? _date_sent;
		public int id {get {return _id;} set {_id = value;}}
		public int rfq_header_id {get {return _rfq_header_id;} set {_rfq_header_id = value;}}
		public int vendor_id {get {return _vendor_id;} set {_vendor_id = value;}}
		public int contact_id {get {return _contact_id;} set {_contact_id = value;}}
		public string keycode { get {return _keycode;} set {_keycode = value;}}
		public bool does_exist { get; set;}
		public DateTime? date_created { get {return _date_created;} set {_date_created = value;}}
		public DateTime? date_opened { get {return _date_opened;} set {_date_opened= value;}}
		public DateTime? date_sent { get {return _date_sent;} set {_date_sent= value;}}
		public DateTime? date_verified { get {return _date_sent;} set {_date_sent= value;}}
		public string note { get; set;}
		public rfq_vendor()
			{
			// Constructor
			}
		/// <summary>
		/// Default way of loading, provide the line id
		/// </summary>
		/// <param name="r_id"></param>
		public rfq_vendor(int r_id)
			{
			id				= r_id;
			load();
			}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="v_id">Vendor ID</param>
		/// <param name="r_id">RFQ Header ID</param>
		public rfq_vendor(int v_id, int r_id)
			{
			vendor_id			= v_id;
			rfq_header_id		= r_id;
			if(exists())
				{
				load();
				}
			}
		public void save()
			{
			if(id == 0)
				{
				id			= Toolbox.doSQL_return_id(@"
INSERT INTO rfq_vendor_list 
	(
	rfq_header_id,
	vendor_id,
	contact_id,
	keycode,
	date_created
	)
VALUES
	(
	@v0,
	@v1,
	@v2,
	@v3,
	NOW()
	)", new object[] {
					rfq_header_id, 
					vendor_id, 
					contact_id,
					keycode
				});
				}
			}
		public void save_note(string _note)
			{

			Toolbox.doSQL_void(@"UPDATE rfq_vendor_list SET note = @v0 WHERE id = @v1 LIMIT 1", new object[] { Toolbox.MySQL_safe(_note), _id});
			}
		public bool validate(string code, out int rfq_id, out int v_id)
			{
			if(key_code_exists(code))
				{
				var dr		= Toolbox.doSQL_dt(@"SELECT * FROM rfq_vendor_list WHERE keycode = @v0  LIMIT 1", new object[] {  code } ).Rows[0];
				rfq_id			= Convert.ToInt32(dr["rfq_header_id"]);
				v_id			= Convert.ToInt32(dr["vendor_id"]);
				return true;
				}
			else
				{
				rfq_id			= 0;
				v_id			= 0;
				return false;
				}
			}
		public void set_vendor_opened()
			{
			Toolbox.doSQL_void(@"UPDATE rfq_vendor_list SET date_opened = NOW() WHERE id = @v0 LIMIT 1", id);
			}
		public void set_vendor_verified()
			{
			Toolbox.doSQL_void(@"UPDATE rfq_vendor_list SET date_verified = NOW() WHERE id = @v0 LIMIT 1", id);
			}
		public void set_vendor_sent()
			{
			Toolbox.doSQL_void(@"UPDATE rfq_vendor_list SET date_sent = NOW() WHERE id = @v0 LIMIT 1", id);
			}
		public bool key_code_exists(string code)
			{
			var c			= Toolbox.doSQL_int(@"SELECT COUNT(id) FROM rfq_vendor_list WHERE keycode = @v0", code);
			return (c > 0);
			}
		public void delete(int v_id, int r_id)
			{
			Toolbox.doSQL_void(@"DELETE FROM rfq_vendor_list WHERE vendor_id = @v0 AND rfq_header_id = @v1 LIMIT 1", new object[] { v_id, r_id});
			}
		public void delete()
			{
			Toolbox.doSQL_void(@"DELETE FROM rfq_vendor_list WHERE id = @v0 LIMIT 1", _id);
			}
		public bool exists()
			{
			var c			= Toolbox.doSQL_int(@"SELECT COUNT(id) FROM rfq_vendor_list WHERE vendor_id = @v0 AND rfq_header_id = @v1", new object[] { _vendor_id, _rfq_header_id});
			does_exist		= c > 0;
			return (c > 0);
			}
		public void load()
			{
			DataRow dr;
			if(id > 0 )
				{
				dr					= Toolbox.doSQL_dt(@"SELECT * FROM rfq_vendor_list  WHERE id =@v0", new object[] { _id }).Rows[0];
				}
			else
				{
				dr					= Toolbox.doSQL_dt(@"SELECT * FROM rfq_vendor_list  WHERE rfq_header_id =@v0 AND vendor_id =@v1 ", new object[] { _rfq_header_id,_vendor_id }).Rows[0];
				id					= Convert.ToInt32(dr["id"]);
				}
			keycode					= dr["keycode"].ToString();
			date_created			= Convert.ToDateTime(dr["date_created"]);
			contact_id				= Convert.ToInt32(dr["contact_id"]);
			if(dr["date_opened"] != DBNull.Value)
				{
				date_opened				= Convert.ToDateTime(dr["date_opened"]);
				}
			else
				{
				date_opened				= null;
				}
			if(dr["date_sent"] != DBNull.Value)
				{
				date_sent				= Convert.ToDateTime(dr["date_sent"]);
				}
			else
				{
				date_sent				= null;
				}
			if(dr["date_verified"] != DBNull.Value)
				{
				date_verified			= Convert.ToDateTime(dr["date_verified"]);
				}
			else
				{
				date_verified				= null;
				}
			if(dr["note"] != DBNull.Value)
				{
				note						= dr["note"].ToString();
				}
			else
				{
				note						= null;
				}
			}
		}
	}