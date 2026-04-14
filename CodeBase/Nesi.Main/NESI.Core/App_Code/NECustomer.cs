using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using NESI.Common.Models;


namespace nesi.core
{
	public class NECustomer
	{
		#region variable declaration

		private ArrayList _ShippingAddresses = new ArrayList();
		private string _Memo = "";
		private string _considerations = "";
		private string _salesnotes = "";
		public int id { get; set; }
		public int Customer_ID { get { return id; } set { id = value; } }
		public int Account_Manager { get; set; }
		public int Project_Manager { get { return Account_Manager; } set { Account_Manager = value; } }
		public bool Is_QCed { get; set; }
		public int Customer_Number_Int { get; set; }
		public string Customer_Number { get; set; }
		public string Customer_Name { get; set; }
		public int business_unit_id { get; set; }
		public int Credit_Type { get; set; }
		public double CreditLimit { get; set; }
		public double Discount { get; set; }
		public double customer_budget_threshold { get; set; }
		public string Notes { get; set; }
		public int StatementCode { get; set; }
		public string ServiceChargeCode { get; set; }
		public string TaxPrompt { get; set; }
		public string Hold { get; set; }
		public int default_invoicetype { get; set; }
		public string customer_whyhold { get; set; }
		public int customer_whohold { get; set; }
		public int customer_decision_maker { get; set; }
		public string customer_nextdate { get; set; }
		public int customer_yearend { get; set; }
		public string customer_followupnotes { get; set; }
		public int customer_frequency_days { get; set; }
		public int customer_auto_invoice { get; set; }
		public bool is_partner { get; set; }
        public bool active { get; set; }
		public bool? key_account { get; set; }

		public int? customer_term_id { get; set; } 

		public List<NEContact> Contacts
		{
			get
			{
				var lc = new List<NEContact>();
				var _contact_ds = NEContact.LoadContactList(id, "Customer");
				var _contacts = _contact_ds.Tables.Count > 0 ? _contact_ds.Tables[0] : new DataTable();
				if (_contacts.Rows.Count > 0)
				{
					foreach (DataRow _c in _contacts.Rows)
					{
						var contact_id = Convert.ToInt32(_c["contact_id"]);
						var con = new NEContact
						{
							id = contact_id,
							nesi_member_id = Toolbox.ReturnZeroIfNull_int(_c["contact_nesimemberid"]),
							customer_id = Toolbox.ReturnZeroIfNull_int(_c["contact_cust_id"]),
							quotemdb_id = Toolbox.ReturnZeroIfNull_int(_c["contact_quotemdb_id"]),
							status_id = Toolbox.ReturnZeroIfNull_int(_c["contact_status_id"]),
							login_enabled = Toolbox.ReturnZeroIfNull_int(_c["contact_login_enabled"]),
							contact_password_set = Toolbox.ReturnZeroIfNull_int(_c["contact_password_set"]) == 1,
							bvno = Toolbox.ReturnBlankIfNull_string(_c["contact_bvno"]),
							name = Toolbox.ReturnBlankIfNull_string(_c["contact_name"]),
							email = Toolbox.ReturnBlankIfNull_string(_c["contact_email"]),
							title = Toolbox.ReturnBlankIfNull_string(_c["contact_title"]),
							cellphone = Toolbox.ReturnBlankIfNull_string(_c["contact_cellphone"]),
							status = Toolbox.ReturnBlankIfNull_string(_c["contact_status"]),
							birthday = Toolbox.ReturnBlankDateTimeIfNull(_c["contact_birthday"]),
							extension = Toolbox.ReturnBlankIfNull_string(_c["contact_extension"]),
							login = Toolbox.ReturnBlankIfNull_string(_c["contact_login"]),
							password = Toolbox.ReturnBlankIfNull_string(_c["contact_password"]),
							type = Toolbox.ReturnBlankIfNull_string(_c["contact_type"]),
							direct_line = Toolbox.ReturnBlankIfNull_string(_c["contact_directline"])
						};
						lc.Add(con);
					}
				}
				return lc;
			}
		}
		/// <summary>
		/// isr in customer table
		/// </summary>
		public int s_inside_sales_rep { get; set; }
		/// <summary>
		/// osr in customer table
		/// </summary>
		public int s_outside_sales_rep { get; set; }
		/// <summary>
		/// ram in customer table
		/// </summary>
		public int s_regional_account_manager { get; set; }
		/// <summary>
		/// mam in customer table
		/// </summary>
		public int s_major_account_manager { get; set; }
		/// <summary>
		/// cisr in customer table
		/// </summary>
		public int s_campaign_inside_sales_rep { get; set; }
		/// <summary>
		/// T or F
		/// </summary>
		public string ApplyFinanceCharges { get; set; }

		public string PriceCode { get; set; }
		public string StatementType { get; set; }
		public string InvoiceType { get; set; }
		public string PORequired { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public int InitMember_ID { get; set; }
		public int ControlsGuy_MemberID { get; set; }
		//public string InitMember_Name { get { return _init_member_name; } }
		public DateTime ts { get; set; }
		public DateTime LastDateTime { get; set; }
		public int Member_ID { get; set; }
		public string Error { get; set; }
		public int QC_Member_ID { get; set; }
		public int QC2_Member_ID { get; set; }
		public DateTime QC_DateTime { get; set; }
		public DateTime QC2_DateTime { get; set; }
		public NEAddress Address { get; set; }

		public ArrayList ShippingAddresses
		{
			get
			{
				if (_ShippingAddresses.Count == 0)
				{
					_ShippingAddresses = Address.ServiceAddresses(id);
				}
				return _ShippingAddresses;
			}
			set
			{
				_ShippingAddresses = value;
			}
		}
		public int Customer_Status { get; set; }
		public bool IsNEcompany { get; set; }
		public int NEbusiness_unit_id { get; set; }
		public string status { get; set; }
		public int customer_autostatements { get; set; }
		public string customer_invoice_address { get; set; }
		public string customer_invoice_ccaddress { get; set; }
		public string customer_autostatement_address { get; set; }
		public string customer_autostatement_ccaddress { get; set; }
		public int customer_creditdays { get; set; }
		public int customer_collection_status { get; set; }
		public string customer_arnotes { get; set; }
		public int confirmed_po { get; set; }
		public int confirmed_taxexempt { get; set; }
		public bool requires_wo_copy { get; set; }
		public string WorkOrder_Memo { get; set; }

       

		public string Memo
		{
			get { return _Memo; }
			set
			{
				if (_Memo == value)
				{
					return;
				}
				_Memo = value;
			}
		}
		public string considerations
		{
			get { return _considerations; }
			set
			{
				if (_considerations == value)
				{
					return;
				}
				_considerations = value;
			}
		}
		public string salesnotes
		{
			get { return _salesnotes; }
			set
			{
				if (_salesnotes == value)
				{
					return;
				}
				_salesnotes = value;
			}
		}
		public int customer_lastorigin { get; set; }
		public string account_code { get; set; }
		#endregion variable declaration
		public static int GetAveragePaymentDelay(int custid)
		{
			var dt = 0;
			try
			{
				dt = Convert.ToInt32(Toolbox.doSQL_double(@"select IFNULL(avg(datediff(woprog_invoice_paid_date, woprog_invoicedate)), 0) from (select * from woprog  WHERE woprog_customer_id =@v0 order by woprog_invoice_paid_date desc limit 10) as avgdays", new object[] { custid }));
			}
			catch
			{
				dt = 0;
			}
			return dt;
		}
		public static int Get_Default_Billing_Address_Id(int c_id)
		{
			return Toolbox.doSQL_int(@"SELECT address_id FROM address WHERE address_table = 'Customer' AND address_table_id = @v0  AND address_type = 'B'", new object[] { c_id });
		}
		private string base_select_query = @"
SELECT 
	*, 
	(SELECT IFNULL(MAX(business_unit_id),0) FROM internal_companyno WHERE internal_companyno_intranet_custid = a.customer_id) internal_custid, 
	(SELECT IFNULL(MAX(customer_or_contact_status), 'Unknown') FROM customer_or_contact_status WHERE customer_or_contact_status_id = a.customer_status) status 
FROM 
	customer a 
WHERE 
";
		public NECustomer(int customer_id)
			{
			if (customer_id == 0) return;
			var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM customer WHERE customer_id = @v0 ", new object[] { customer_id });
			if (c == 0)
			{
				Toolbox.do_errorLog($"Customer isn't in database - {customer_id}.");
				throw new Exception("Customer isn't in database.");
			}
			var dr = Toolbox.doSQL_dt(string.Format(@"{0}  a.customer_id = @v0  LIMIT 1", base_select_query), new object[] {  customer_id }).Rows[0];
			set_vars(dr);
		}
		private void set_vars(DataRow dr)
		{
			var st = DateTime.Now;
			Credit_Type = Convert.ToInt16(dr["customer_credittype"].ToString());
			StatementCode = Convert.ToInt16(dr["customer_statementcode"].ToString());
			Customer_Status = Convert.ToInt16(dr["customer_status"]);
			id = Convert.ToInt32(dr["customer_id"]);
			Customer_Number_Int = Convert.ToInt32(dr["customer_number_int"]);
           // business_unit_id = Convert.ToInt32(dr["business_unit_id"]);
            InitMember_ID = Convert.ToInt32(dr["customer_initmember_id"]);
			//_init_member_name		          = _tools.getSQL_string(@"SELECT SQL_NO_CACHE IFNULL(MAX(member_fullname), 'N/A') FROM member WHERE member_id = @v0 ", new object[] {  _InitMember_ID } );
			Member_ID = Toolbox.ReturnZeroIfNull_int(dr["customer_member_id"]);
			QC_Member_ID = dr["customer_qc_member_id"] != DBNull.Value ? Convert.ToInt32(dr["customer_qc_member_id"]) : QC_Member_ID;
			QC2_Member_ID = dr["customer_qc2_member_id"] != DBNull.Value ? Convert.ToInt32(dr["customer_qc2_member_id"]) : QC2_Member_ID;
			Account_Manager = dr["customer_account_manager"] != DBNull.Value ? Convert.ToInt32(dr["customer_account_manager"]) : 0;
			ControlsGuy_MemberID = dr["customer_controlsguy"] != DBNull.Value ? Convert.ToInt32(dr["customer_controlsguy"]) : 0;
			CreditLimit = Convert.ToDouble(dr["customer_creditlimit"].ToString());
			Discount = Convert.ToDouble(dr["customer_discount"].ToString());
			CreatedDateTime = Convert.ToDateTime(dr["customer_createddatetime"].ToString());
			LastDateTime = Convert.ToDateTime(dr["customer_lastdatetime"].ToString());
			QC_DateTime = dr["customer_qc_datetime"] != DBNull.Value ? Convert.ToDateTime(dr["customer_qc_datetime"]) : QC_DateTime;
			QC2_DateTime = dr["customer_qc2_datetime"] != DBNull.Value ? Convert.ToDateTime(dr["customer_qc2_datetime"]) : QC2_DateTime;
			Customer_Number = dr["customer_number"].ToString();
			Customer_Name = dr["customer_name"].ToString();
			Notes = dr["customer_notes"].ToString();
			ServiceChargeCode = dr["customer_servicechargecode"].ToString();
			TaxPrompt = dr["customer_taxprompt"].ToString();
			Hold = dr["customer_hold"].ToString();
			PriceCode = dr["customer_pricecode"].ToString();
			StatementType = dr["customer_statementtype"].ToString();
			InvoiceType = dr["customer_invoicetype"].ToString();
			PORequired = dr["customer_porequired"].ToString();
			_Memo = dr["customer_memo"].ToString();
			WorkOrder_Memo = dr["customer_workorder_memo"].ToString();
			_considerations = dr["customer_considerations"].ToString();
			is_partner = Convert.ToBoolean(dr["is_partner"]);
			ApplyFinanceCharges = dr["customer_apply_finance"] == DBNull.Value ? "F" : "T";
			if (Customer_Status != 4)
			{
				Address = new NEAddress(Customer_ID, "Customer");
			}
			Is_QCed = dr["customer_qc_member_id"] != DBNull.Value && dr["customer_qc_datetime"] != DBNull.Value;
			customer_autostatements = Convert.ToInt16(dr["customer_autostatements"]);
			customer_invoice_address = dr["customer_invoice_address"].ToString();
			customer_invoice_ccaddress = dr["customer_invoice_ccaddress"].ToString();
			customer_autostatement_address = dr["customer_autostatement_address"].ToString();
			customer_autostatement_ccaddress = dr["customer_autostatement_ccaddress"].ToString();
			customer_creditdays = Convert.ToInt32(dr["customer_creditdays"]);
            customer_term_id = dr["customer_term_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["customer_term_id"]);
            _salesnotes = dr["customer_salesnotes"].ToString();
			customer_collection_status = dr["customer_collection_status"] == DBNull.Value ? 0 : Convert.ToInt16(dr["customer_collection_status"]);
			customer_arnotes = dr["customer_arnotes"].ToString();
			customer_whohold = Convert.ToInt32(dr["customer_whohold"]);
			customer_whyhold = dr["customer_whyhold"].ToString();
			customer_budget_threshold = dr["customer_budget_threshold"] == DBNull.Value ? 0 : Convert.ToDouble(dr["customer_budget_threshold"]);
			customer_decision_maker = dr["customer_decision_maker"] == DBNull.Value ? 0 : Convert.ToInt32(dr["customer_decision_maker"]);
			customer_nextdate = dr["customer_nextdate"].ToString();
			customer_yearend = dr["customer_yearend"] == DBNull.Value ? 0 : Convert.ToInt32(dr["customer_yearend"]);
			customer_followupnotes = dr["customer_followupnotes"].ToString();
			customer_frequency_days = dr["customer_frequency_days"] == DBNull.Value ? 0 : Convert.ToInt32(dr["customer_frequency_days"]);
			ts = Toolbox.ReturnBlankDateTimeIfNull(dr["customer_ts"]);
			NEbusiness_unit_id = Convert.ToInt32(dr["internal_custid"]);

			requires_wo_copy = Convert.ToBoolean(dr["requires_wo_copy"]);

			IsNEcompany = NEbusiness_unit_id > 0;
            business_unit_id = NEbusiness_unit_id; // IsNEcompany? NEbusiness_unit_id:Convert.ToInt32(dr["business_unit_id"]);  
            default_invoicetype = Convert.ToInt32(dr["customer_default_invoicetype"]);
			customer_auto_invoice = Convert.ToInt32(dr["customer_auto_invoice"]);
			confirmed_po = Convert.ToInt32(dr["confirmed_po"]);
			confirmed_taxexempt = Convert.ToInt32(dr["confirmed_taxexempt"]);
			customer_lastorigin = Convert.ToInt32(dr["customer_lastorigin"]);
			account_code = dr["customer_accountcode"] == DBNull.Value ? "" : dr["customer_accountcode"].ToString();
			s_inside_sales_rep = dr["isr"] == DBNull.Value ? 0 : Convert.ToInt32(dr["isr"]);
			s_outside_sales_rep = dr["osr"] == DBNull.Value ? 0 : Convert.ToInt32(dr["osr"]);
			s_regional_account_manager = dr["ram"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ram"]);
			s_major_account_manager = dr["mam"] == DBNull.Value ? 0 : Convert.ToInt32(dr["mam"]);
			s_campaign_inside_sales_rep = dr["cisr"] == DBNull.Value ? 0 : Convert.ToInt32(dr["cisr"]);
			status = dr["status"].ToString();
            active = Convert.ToBoolean(dr["active"]);
			key_account = dr["key_account"] == DBNull.Value ? false : Convert.ToBoolean(dr["key_account"]);
		}
		public static string GetName(object _id)
		{
			return Toolbox.doSQL_string(@"SELECT customer_name FROM customer  WHERE customer_id =@v0 limit 1 ", new[] { _id });
		}

		public void add_history(string _memo, NeMember _user, object _business_unit_id)
		{
		var _item = new historycustitem
						{
						historycust_customer_id      = id,
						historycust_business_unit_id = Convert.ToInt32(_business_unit_id),
						historycust_memo             = _memo
						};
		_item.save();
		}
		public void add_history(string _memo, NeMember _user, object _business_unit_id, string _invoice, string _wo_number)
		{
		var _item = new historycustitem
						{
						historycust_customer_id      = id,
						historycust_business_unit_id = Convert.ToInt32(_business_unit_id),
						historycust_memo             = _memo,
						historycust_wo               = _wo_number,
						historycust_invoice          = _invoice
						};
		_item.save();
		}


	public NECustomer()
		{
			//
			//  
			//
		}
		/// <summary>
		/// Saves the currently loaded NeCustomer, don't need to reinitiate the Class.
		/// </summary>
		/// <returns></returns>
		public int Save()
		{
			using (var conn = Toolbox.connect())
			{
				var cust_id = 0;
				var is_new = id == 0;
				var exists = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM customer WHERE customer_id = @v0 ", new object[] { id });
				var sql = "";
				if (Toolbox.Contains(Credit_Type, new[] { 0, 1, 3 })) // If the credit type is No Credit (0), Unlimited (1), or Credit Card Only (2) - set their credit to zero
				{
					CreditLimit = 0;
				}

				if (exists == 0)
				{
					try
					{
						cust_id = Convert.ToInt32(Toolbox.doSQL_return_id(conn, @" INSERT INTO Customer ( customer_name, business_unit_id, customer_number, customer_number_int, 
customer_credittype, customer_creditlimit, customer_discount, customer_pricecode, customer_notes, customer_servicechargecode, customer_taxprompt, customer_hold, customer_whyhold, 
customer_statementtype, customer_invoicetype, customer_porequired, customer_createddatetime, customer_lastdatetime, customer_member_id, customer_initmember_id, customer_status, 
customer_apply_finance, customer_account_manager, customer_budget_threshold, customer_yearend, customer_default_invoicetype, customer_auto_invoice, customer_lastorigin, customer_accountcode, isr, osr, ram, cisr, is_partner )
VALUES ( @v0 , @v1 , NULL, NULL, @v2 , @v3 , @v4 , @v5 , @v6 , 'F', @v7 , @v8 , @v26 , @v9 , @v10 , @v11 , NOW(), NOW(), @v12 , @v13 , 1, @v14 , @v15 , @v16 , @v17 , @v18 , @v19 , @v20 , @v21 , @v22 , @v23 , @v24 , @v25 , @v27  )", 
new object[] { Customer_Name, business_unit_id, Credit_Type, CreditLimit, Discount, PriceCode, Notes, TaxPrompt, Hold, StatementType, InvoiceType, PORequired, Member_ID, InitMember_ID, ApplyFinanceCharges, Account_Manager, customer_budget_threshold, customer_yearend, default_invoicetype, customer_auto_invoice, customer_lastorigin, account_code, s_inside_sales_rep == 0 ? "NULL" : s_inside_sales_rep.ToString(), s_outside_sales_rep == 0 ? "NULL" : s_outside_sales_rep.ToString(), s_regional_account_manager == 0 ? "NULL" : s_regional_account_manager.ToString(), s_campaign_inside_sales_rep == 0 ? "NULL" : s_campaign_inside_sales_rep.ToString(), customer_whyhold, is_partner }));
						Toolbox.doSQL_void(conn, @"UPDATE customer SET customer_number = @v0 , customer_number_int = @v0  WHERE customer_id = @v0  LIMIT 1", new object[] { cust_id });
						add_history("Customer created", new NeMember(Member_ID), business_unit_id);
					}
					catch (Exception ee)
					{
						Toolbox.do_errorLog_errorStack(ee);
						throw new Exception("Customer could not be saved; There was an issue creating the customer entry in the customer table.");
					}
					id = cust_id;
					Customer_Number = cust_id.ToString();
					Customer_Number_Int = cust_id;

				}
				else
				{

					try
					{
						Toolbox.doSQL_void(conn, @" UPDATE customer SET customer_name = @v0 , 
customer_number = @v1 , customer_number_int = @v2 , business_unit_id = @v3 , customer_credittype = @v4 , 
customer_creditlimit = @v5 , customer_discount = @v6 , customer_notes = @v7 ,
customer_statementcode = @v8 , customer_servicechargecode = @v9 , customer_taxprompt = @v10 ,
customer_hold = @v11 , customer_pricecode = @v12 , customer_statementtype = @v13 , customer_invoicetype = @v14 ,
customer_porequired = @v15 , customer_lastdatetime = Now(), customer_account_manager = @v20 , customer_member_id = @v17 , 
customer_status = @v18 , customer_memo = @v21 , customer_apply_finance = @v22 , customer_considerations = @v23 , customer_controlsguy = @v24 ,
customer_autostatements = @v25 , customer_invoice_address = @v26 , customer_invoice_ccaddress = @v27 , customer_autostatement_address = @v28 ,
customer_autostatement_ccaddress = @v29 , customer_salesnotes = @v30 , customer_collection_status = @v31 , customer_creditdays = @v32 , 
customer_arnotes = @v33 , customer_whyhold = @v34 , customer_whohold = @v35 , customer_budget_threshold = @v36 , customer_decision_maker = @v37 ,
customer_nextdate = @v38 , customer_yearend = @v39 , customer_followupnotes = @v40 , customer_frequency_days = @v41 , customer_default_invoicetype = @v42 ,
customer_auto_invoice = @v43 , confirmed_po = @v44 , confirmed_taxexempt = @v45 , customer_lastorigin = @v46 , customer_accountcode = @v47 , isr = @v48 ,
osr = @v49 , ram = @v50 , mam = @v51 , cisr = @v52 , requires_wo_copy = @v53 , is_partner = @v54, customer_workorder_memo=@v55,customer_term_id=@v56  WHERE customer_id = @v19  LIMIT 1", 
							new object[] { Customer_Name, Customer_Number,
								Customer_Number_Int, business_unit_id, Credit_Type, CreditLimit, Discount,
								Notes, StatementCode, ServiceChargeCode, TaxPrompt, Hold,
								PriceCode, StatementType, InvoiceType, PORequired, 0, Member_ID, Customer_Status, id, Account_Manager,
							_Memo, ApplyFinanceCharges, _considerations,
							ControlsGuy_MemberID, customer_autostatements, customer_invoice_address,
							customer_invoice_ccaddress, customer_autostatement_address, customer_autostatement_ccaddress,
							_salesnotes, customer_collection_status, customer_creditdays,
							customer_arnotes, customer_whyhold, customer_whohold, customer_budget_threshold,
							customer_decision_maker, customer_nextdate, customer_yearend, customer_followupnotes,
							customer_frequency_days, default_invoicetype, customer_auto_invoice, confirmed_po, confirmed_taxexempt,
							customer_lastorigin, account_code, s_inside_sales_rep == 0 ? "NULL" : s_inside_sales_rep.ToString(), s_outside_sales_rep == 0 ? 
							"NULL" : s_outside_sales_rep.ToString(), s_regional_account_manager == 0 ? 
							"NULL" : s_regional_account_manager.ToString(), s_major_account_manager == 0 ? "NULL" : 
							s_major_account_manager.ToString(), s_campaign_inside_sales_rep == 0 ? "NULL" : s_campaign_inside_sales_rep.ToString(),
							requires_wo_copy, is_partner, WorkOrder_Memo,customer_term_id });

						add_history("Customer updated", new NeMember(Member_ID), business_unit_id);
					}
					catch
					{
						throw new Exception("Could not save customer");
					}
					cust_id = id;
				}
                

				return cust_id;
			}
		}
    
        public static bool NSCustomerExists(MySqlConnection conn, int customerInternalId)
            {
            return Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM customer WHERE netsuite_internal_id = @v0 LIMIT 1", new object[] { customerInternalId }) > 0;
            }
        public static Dictionary<int, bool> NSCustomerList(MySqlConnection conn)
            {
            var customerList = new Dictionary<int, bool>();
            var dtCustomers  = Toolbox.doSQL_dt(conn, @"SELECT netsuite_internal_id, IFNULL(active, 0) active FROM customer WHERE IFNULL(netsuite_internal_id, 0) != 0", new object[]{ });
            foreach(DataRow dr in dtCustomers.Rows)
                {
                var custId  = Convert.ToInt32(dr["netsuite_internal_id"]);
                var active = Convert.ToInt32(dr["active"]) == 1;
				if(custId != 0 && !customerList.ContainsKey(custId))
					{ 
					customerList.Add(custId, active);
					}
                }
            return customerList;
            }
    
        public static bool NSCustomerActive(MySqlConnection conn, int customerInternalId)
            {
            return Toolbox.doSQL_int(conn, @"SELECT active FROM customer WHERE netsuite_internal_id = @v0 LIMIT 1", new object[] { customerInternalId }) == 1;
            }
		public static void add_to_customer_history(int custid, int memberid, DateTime date, string notes, int actionid, int origin_id, int address_id)
		{
			Toolbox.doSQL_void(@" INSERT INTO customer_history ( customer_history_memberid, customer_history_date, customer_history_action, customer_history_notes, customer_history_custid, origin, address_id ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6  )", new object[] { memberid, Toolbox.MySQL_longdt(date), actionid,
				notes, custid, origin_id, address_id });
		}
	}
	public class customer_asset
	{
		public int id { get; set; }
		public string name { get; set; }
		public int customer_id { get; set; }
		public int address_id { get; set; }
		public string description { get; set; }
		public bool active { get; set; }
		public int added_by { get; set; }
		public string manufacturer { get; set; }
		public string model { get; set; }
		public void save()
		{
			if (id == 0)
			{
				// INSERT
				Toolbox.doSQL_void(@" INSERT INTO customer_asset ( name, customer_id, address_id, description, active, addedby, manufacturer, model ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6 , @v7  ) ", 
					new object[] { name, customer_id, address_id, description, active, added_by, manufacturer, model });
			}
			else
			{
				// UPDATE
				Toolbox.doSQL_void(@" UPDATE customer_asset SET name = @v0 , customer_id = @v1 , address_id = @v2 , description = @v3 , active = @v4 , addedby = @v5 , manufacturer = @v6 , model = @v7  WHERE id = @v8  LIMIT 1 ",
					new object[] { name, customer_id, address_id, description, active, added_by, manufacturer, model, id });
			}
		}
		public customer_asset() { }
		public customer_asset(int _id)
		{
			id = _id;
			init();
		}
		private void init()
		{
			if (exists(id))
			{
				var dr = Toolbox.doSQL_dt(@"SELECT * FROM customer_asset WHERE id = @v0 ", new object[] { id }).Rows[0];
				name = dr["name"].ToString();
				customer_id = (int)dr["customer_id"];
				address_id = (int)dr["address_id"];
				description = dr["description"].ToString();
				active = Convert.ToBoolean(dr["active"]);
				added_by = (int)dr["addedby"];
				manufacturer = dr["manufacturer"].ToString();
				model = dr["model"].ToString();
			}
		}
		public bool exists(int _id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM customer_asset WHERE id = @v0 ", new object[] { _id }) > 0;
		}
		public void delete()
		{
			if (exists(id))
			{
				if (is_linked(id, 0))
				{
					throw new Exception(@"This asset is currently linked to work orders and cannot be deleted");
				}
				else
				{
					Toolbox.doSQL_void(@"DELETE FROM customer_asset WHERE id = @v0  LIMIT 1", new object[] { id });
				}
			}
		}
		/// <summary>
		/// Checks is this asset is used anywhere
		/// <para>0 = Work Orders</para>
		/// </summary>
		/// <param name="_id"></param>
		/// <param name="type_id"></param>
		/// <returns></returns>
		public bool is_linked(int _id, int type_id)
		{
			var existence_count = 0;
			switch (type_id)
			{
				case 0:
					existence_count = Toolbox.doSQL_int(@"SELECT COUNT(woprog_id) FROM woprog WHERE woprog_assetid = @v0 ", new object[] { _id });
					break;
			}
			return existence_count > 0;
		}
	}
	public class customer_chargeout
	{
		public int id { get; set; }
		public int customer_id { get; set; }
		public int base_chargeout_id { get; set; }
		public double chargeout { get; set; }
		public int member_id { get; set; }
		public int business_unit_id { get; set; }
		public DateTime from_date { get; set; }
		public DateTime to_date { get; set; }
		public DateTime last_updated { get; set; }
		public int address_id { get; set; }

		public customer_chargeout()
		{

		}
		public customer_chargeout(int _id)
		{
			var dr = Toolbox.doSQL_dt(@"SELECT * FROM customer_rate WHERE id = @v0  LIMIT 1", new object[] { _id }).Rows[0];
			id = _id;
			customer_id = Convert.ToInt32(dr["customer_id"]);
			base_chargeout_id = Convert.ToInt32(dr["base_chargeout_id"]);
			chargeout = Convert.ToDouble(dr["chargeout"]);
			from_date = dr["from_date"] == DBNull.Value ? Convert.ToDateTime("0001-01-01") : Convert.ToDateTime(dr["from_date"]);
			to_date = dr["from_date"] == DBNull.Value ? Convert.ToDateTime("0001-01-01") : Convert.ToDateTime(dr["from_date"]);
			last_updated = dr["from_date"] == DBNull.Value ? Convert.ToDateTime("0001-01-01") : Convert.ToDateTime(dr["from_date"]);
			address_id = Convert.ToInt32(dr["address_id"]);

		}
		public bool exists(int _id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM customer_rate  WHERE id =@v0", new object[] { _id }) > 0;
		}
		public void save()
		{
			var is_new = id == 0;
			if (is_new)
			{
				// Insert
				var insert_id = Toolbox.doSQL_return_id(@" INSERT INTO customer_rate ( customer_id, base_chargeout_id, chargeout, from_date,to_date,last_updated,notification_sent,address_id ) 
VALUES ( @v0 , @v1 , @v2 ,@v3 ,@v4 ,@v5 ,0,@v6 )",
					new object[] { customer_id, base_chargeout_id, chargeout, from_date.ToString("yyyy-MM-dd"),
						to_date.ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd"),
						address_id
					});
				#region Save Log Entry

				var log = new NELog
					{
					section_id = OpsLog.Section.CustomerRates,
					action_id = OpsLog.Action.AdjustedChargeOutRate,
					table = OpsLog.Table.CustomerRate,
					table_id = insert_id,
					value_old = 0,
					value_new = chargeout,
					is_manual = true,
					member_id = member_id,
					business_unit_id = business_unit_id
					};
				log.save();
				#endregion Save Log Entry
			}
			else
			{
				// Update
				var temp = new customer_chargeout(id);
				Toolbox.doSQL_void(@" UPDATE customer_rate SET chargeout = @v0 ,
last_updated=@v2 ,from_date=@v3 ,to_date=@v4 ,notification_sent = 0 
WHERE id = @v1  LIMIT 1", new object[] { chargeout, id, DateTime.Today.ToString("yyyy-MM-dd"), from_date.ToString("yyyy-MM-dd"), to_date.ToString("yyyy-MM-dd") });
				#region Save Log Entry
				if (temp.chargeout != chargeout)
				{
					var log = new NELog
						{
						section_id = OpsLog.Section.CustomerRates,
						action_id = OpsLog.Action.AdjustedChargeOutRate,
						table = OpsLog.Table.CustomerRate,
						table_id = id,
						value_old = temp.chargeout,
						value_new = chargeout,
						is_manual = true,
						member_id = member_id,
						business_unit_id = business_unit_id
						};
					log.save();
				}
				#endregion Save Log Entry
			}

		}
	}
	public class customer_history
	{
		//customer_history_id = id
		//customer_history_memberid = member_id
		//customer_history_action = action_id
		//customer_history_custid = customer_id
		//customer_history_date = date
		//customer_history_notes = notes
		public int id { get; set; }
		public int member_id { get; set; }
		public int action_id { get; set; }
		public int customer_id { get; set; }
		public DateTime date { get; set; }
		public string notes { get; set; }
		public int origin { get; set; }
		public int address_id { get; set; }

		public customer_history() { }
		public customer_history(int _id)
		{
			if (exists(_id))
			{
				init(_id);
			}
		}
		private void init(int _id)
		{
			id = _id;
			var dr = Toolbox.doSQL_dt(@"SELECT * FROM customer_history WHERE customer_history_id = @v0  LIMIT 1", new object[] { _id }).Rows[0];
			member_id = Convert.ToInt32(dr["customer_history_memberid"]);
			action_id = Convert.ToInt32(dr["customer_history_action"]);
			customer_id = Convert.ToInt32(dr["customer_history_custid"]);
			date = Toolbox.ReturnBlankDateTimeIfNull(dr["customer_history_date"]);
			notes = dr["customer_history_notes"].ToString();
			origin = Convert.ToInt32(dr["origin"]);
			address_id = Convert.ToInt32(dr["address_id"]);
		}
		public void save()
		{
			if (id == 0)
			{
				// Insert
				Toolbox.doSQL_void(@" INSERT INTO customer_history ( customer_history_memberid, customer_history_action, customer_history_custid, customer_history_date, customer_history_notes, origin, address_id ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6  )", new object[] { member_id, action_id, customer_id, Toolbox.MySQL_shortdt(date),
					notes, origin == 0 ? 1 : origin, address_id });
			}
			else
			{
				// Update
				Toolbox.doSQL_void(@" UPDATE customer_history SET customer_history_memberid = @v0 , customer_history_action = @v1 , customer_history_custid = @v2 , customer_history_date = @v3 , customer_history_notes = @v4 , origin = @v5  WHERE customer_history_id = @v6  LIMIT 1", new object[] { member_id, action_id, customer_id, Toolbox.MySQL_shortdt(date),
					notes, origin == 0 ? 1 : origin, id });
			}
		}
		public void delete(int _id)
		{
			Toolbox.doSQL_void(@"DELETE FROM customer_history WHERE customer_history_id = @v0  LIMIT 1", new object[] { _id });
		}
		public bool exists(int _id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(customer_history_id) FROM customer_history WHERE customer_history_id = @v0 ", new object[] { _id }) > 0;
		}
	}
	public class historycustitem
	{
		private Toolbox _tools = new Toolbox();

		public int historycust_id { get; set; }
		public DateTime historycust_date { get; set; }
		public int historycust_member_id { get; set; }
		public int historycust_business_unit_id { get; set; }
		public int historycust_customer_id { get; set; }
		public string historycust_memo { get; set; }
		public string historycust_wo { get; set; }
		public string historycust_invoice { get; set; }

		public void save()
		{
			var is_new = historycust_id == 0;
			try
			{
				if (is_new)
				{
					_tools.getSQL_void(@" INSERT INTO historycust ( historycust_date, historycust_member_id,business_unit_id,historycust_company_id, historycust_customer_id, historycust_memo, historycust_wo, historycust_invoice ) 
VALUES ( NOW(), @v0 , @v1 ,@v1, @v3 , @v2 , @v4 , @v5  ) ", new object[] { historycust_member_id, historycust_business_unit_id,
						historycust_memo, historycust_customer_id,
					(historycust_wo==null ? "" : historycust_wo), 
					(historycust_invoice==null ? "" : historycust_invoice),
					});
				}
				else
				{
					throw new Exception("Updating not built in yet");
				}
			}
			catch (Exception ee)
			{
				_tools.debug_note(ee);
				throw ee;
			}
		}
	}
}
