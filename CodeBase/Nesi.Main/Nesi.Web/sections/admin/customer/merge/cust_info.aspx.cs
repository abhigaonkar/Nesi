using System;
using System.Data;
using System.Text;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;
//using nesi.bv;
using nesi.core;
using Pervasive.Data.SqlClient;

public partial class sections_admin_customer_merge_cust_info : System.Web.UI.Page
	{
//	Toolbox _tools;
//	NeMember current_user;
//	int _page_id = 88;
//	string _merge_from_id = "";
//	int _merge_privilege = 80;
//	StringBuilder mailed_report_sb;
//	NameValueCollection _q;
//	protected void Page_Init(object sender, EventArgs e)
//		{
//		_tools					= new Toolbox();
//		current_user			= Toolbox.do_handle_authentication(_page_id);
//		mailed_report_sb		= new StringBuilder();
//		_q						= Request.QueryString;
//		}
//	protected void Page_Load(object sender, EventArgs e)
//		{
//		var MergeCustID      = string.IsNullOrEmpty(_q["MergeCustomer"]) ? "" : _q["MergeCustomer"];
//		var custid           = string.IsNullOrEmpty(_q["custid"]) ? "" : _q["custid"];
//		var phonenumber      = "";
//		var name             = "";
//		var TypeofMerge      = string.IsNullOrEmpty(_q["MergeType"]) ? "" : _q["MergeType"];
//		var sb		= new StringBuilder();

//		if (MergeCustID == "")
//			{
//			var custinfo = new DataTable();
//			if(!string.IsNullOrEmpty(_q["custid"]))
//				{
//				custid = _q["custid"];
//				phonenumber = getPhoneNumberForMatch(custid);
//				custinfo = getAllCustID(phonenumber);
//				TypeofMerge = "PhoneNumber";
//				}
//			else if(!string.IsNullOrEmpty(_q["namecustid"]))
//				{
//				custid = _q["namecustid"];
//				name = getNameForMatch(custid);
//				Session["NameToCheck"] = name;
//				custinfo = getAllCustIDName(custid);
//				TypeofMerge = "CustomerName";
//				}
//			/*
//			if (custid == "")
//				{
//				try
//					{
//					}
//				catch { }
//				}
//			if (custid == "")
//				{

//				try
//					{
//					custid = _q["addresscustid"].ToString();
//					name = getAddressForMatch(custid);
//					Session["AddressToCheck"] = name;
//					custinfo = getAllCustIDAddress(Session["AddressToCheck"].ToString());
//					TypeofMerge = "CustomerAddress";
//					}
//				catch (Exception eex) { throw new Exception(eex.ToString()); }
//				}
//			*/
//			sb.Append("<table border='1'>");
//			foreach (DataRow row in custinfo.Rows)
//				{
//				generate_information(row, TypeofMerge, ref sb);
//				}
//			sb.Append("</table>");
//			var asdf = sb.ToString();
//			report.InnerHtml = sb.ToString();
//			}
//		else
//			{
//			if (!current_user.AuthenticatedForPrivilege(_merge_privilege))
//				{
//				throw new Exception("You Do Not have Permission to Merge Customer");
//				}
//			if(!string.IsNullOrEmpty(_q["force_merge"]) && !string.IsNullOrEmpty(_q["to_id"]) && !string.IsNullOrEmpty(_q["from_id"]))
//				{
//				_merge_from_id		= _q["from_id"];
//				merge_customer(_q["to_id"], "");
//				}
//			else
//				{
//				merge_customer(MergeCustID, TypeofMerge);
//				}
//			}
//		}
//	protected string getPhoneNumberForMatch(string custid)
//		{
//		return Toolbox.doSQL_string(@"
//SELECT 
//	CONCAT(a.address_phonefull,A.address_type) AS PhoneA 
//FROM 
//	address as A, Customer as C 
//WHERE 
//	A.address_table_id = C.customer_id AND 
//	(A.address_table = 'Customer' or A.address_table='Worksite') AND 
//	C.customer_id = @v0", custid);
//		}
//	protected string getNameForMatch(string custid)
//		{
//		return Toolbox.doSQL_string(@"SELECT customer_name FROM Customer WHERE Customer_ID = @v0", custid);
//		}
//	protected string getAddressForMatch(string custid)
//		{
//		var sqlAddressFind = "SELECT A.Address_Addr1 ";
//		sqlAddressFind += "FROM address as A, Customer as C ";
//		sqlAddressFind += "WHERE ";
//		sqlAddressFind += "A.Address_Table_ID = C.Customer_ID ";
//		sqlAddressFind += "AND (A.Address_Table = 'Customer' or A.Address_Table = 'Worksite') ";
//		sqlAddressFind += "AND C.Customer_ID = @v0";
//		var Addr1 = _tools.getSQL_string(sqlAddressFind,new object[] { custid} );
//		return Addr1;
//		}
//	protected DataTable getAllCustID(string phonenumber)
//		{
//		return Toolbox.doSQL_dt(@" SELECT c.customer_number, c.customer_name, c.customer_id, c.customer_hold, a.address_addr1, a.address_city, a.address_prov, a.address_phonefull FROM address a, customer c, customer_sales_properties d WHERE CONCAT(a.address_phonefull, a.address_type) = @v0  AND a.address_table = 'Customer' AND a.address_type = 'B' AND a.address_table_id = c.customer_id AND d.address_id = a.address_id AND IFNULL(d.status_id,0) != 4", new object[] {  phonenumber } );
//		}
//	protected DataTable getAllCustIDName(string cust_id)
//		{
//		return Toolbox.doSQL_dt(@"CALL REPORT_QC_CUSTOMERNAME(1, @v0 )", new object[] {  cust_id } );
//		}
//	protected DataTable getAllCustIDAddress(string address)
//		{

//		var VendorSQL = "SELECT Customer_number, customer_name, ";
//		VendorSQL += "Address_Addr1,Address_City, Address_Prov, ";
//		VendorSQL += "Address_PhoneArea, Address_PhoneFirst, Address_PhoneLast, Customer_ID, Customer_Hold ";
//		VendorSQL += "FROM address as A, Customer as C ";
//		VendorSQL += "WHERE Address_Addr1 = '" + address + "' ";
//		VendorSQL += "AND (A.Address_Table = 'Customer' or A.Address_Table = 'Worksite') ";
//		VendorSQL += "AND A.Address_Table_ID = C.Customer_ID ";
//		VendorSQL += "AND C.Customer_Status != 4";


//		var table = _tools.getSQL_datatable(VendorSQL,null);

//		return table;
//		}

//	private void generate_information(DataRow _row, string _merge_type, ref StringBuilder _sb)
//		{
//		var customer_name			= _row["customer_name"].ToString();
//		var addr1					= _row["address_addr1"].ToString();
//		var city						= _row["address_city"].ToString();
//		var prov_state				= _row["address_prov"].ToString();
//		var customer_id				= _row["customer_id"].ToString();
//		var customer_number			= _row["customer_number"].ToString();
//		var hold						= _row["customer_hold"].ToString();
//		hold							= hold == "T" ? " - ON HOLD" : "";
//		var phone_full				= _row["address_phonefull"].ToString();
//		var wo_count					= Toolbox.doSQL_int(@"SELECT COUNT(woprog_id) FROM woprog WHERE woprog_customer_id = @v0 ", customer_id);
//		var quote_count					= Toolbox.doSQL_int(@"SELECT COUNT(quote_id) FROM quote_master WHERE customer_id = @v0 ", customer_id);
//		var contact_count				= Toolbox.doSQL_int(@"SELECT COUNT(contact_id) FROM contact WHERE contact_type='Customer' AND contact_cust_id = @v0 ", customer_id);
//		var shipping_count				= Toolbox.doSQL_int(@"SELECT COUNT(address_id) FROM address WHERE address_table_id = @v0 AND (address_table = 'customer' or address_table='Worksite') AND address_type = 'S'", customer_id);
//		var timeslice_count				= Toolbox.doSQL_int(@"SELECT COUNT(membertime_id) FROM membertime WHERE membertime_customer_id = @v0 ", customer_id);
//		var bv_info					= UsedInBV(customer_number, false);
//		_sb.AppendFormat(@"
//<tr>
//	<td>MySQL ID: {0}</td>
//	<td>{1} {2}</td>
//	<td>BV Cust Num: {3}</td>
//	<td>{4}</td>
//	<td>{5}</td>
//	<td>{6}</td>
//	<td>{7}</td>
//</tr>
//<tr>
//	<td>Used in Work Orders this many Times:</td>
//	<td colspan='5'>{8}</td>
//</tr>
//<tr>
//	<td>Used in Quotes this many Times:</td>
//	<td colspan='5'>{9}</td>
//</tr>
//	<tr><td>Related Time Slices:</td>
//	<td colspan='5'>{10}</td>
//</tr>
//<tr>
//	<td>Number of Contacts:</td>
//	<td colspan='5'>{11}</td>
//</tr>
//<tr>
//	<td>Number of Shipping Addresses:</td>
//	<td colspan='5'>{12}</td>
//</tr>
//<tr>
//	<td colspan='6'>{13}</td>
//	<td>Make <a href='cust_info.aspx?MergeCustomer={0}&MergeType={14}'>{3}</a> The True Customer</td>
//</tr>",
//		customer_id, 		  // {0}
//		customer_name, 		  // {1}
//		hold, 				  // {2}
//		customer_number,	  // {3}
//		phone_full,			  // {4}
//		addr1,				  // {5}
//		city,				  // {6}
//		prov_state,			  // {7}
//		wo_count,			  // {8}
//		quote_count,		  // {9}
//		timeslice_count,	  // {10}
//		contact_count,		  // {11}
//		shipping_count,		  // {12}
//		bv_info,			  // {13}
//		_merge_type			  // {14}
//		);
//		}
//	protected string UsedInBV(string custnum, bool for_email)
//		{
//		var bvsb		= new StringBuilder();
//		bvsb.Append(@"
//<table>");
//        //TODO This should check if TE uses BV
//		var dsn_table = Toolbox.doSQL_dt(@"SELECT dsn, name name from business_unit inner join tax_entity on tax_entity.id = business_unit.tax_entity_id where tax_entity.is_active=1 AND tax_entity.dsn != '' ", null);
//		foreach (DataRow row in dsn_table.Rows)
//			{
//			var dsn			= row["dsn"].ToString();
//			var name			= row["name"].ToString();
//			var bv_info	= _tools.getSQL_datatable(@"SELECT name, last_date FROM CUSTOMER WHERE CUS_NO = ? ", dsn , new object[] {  custnum } );
//			var exists			= bv_info.Rows.Count > 0;
//			var bv_name		= exists ? bv_info.Rows[0]["name"].ToString() : "Not Found";
//			var last_date	= exists 
//									? bv_info.Rows[0]["last_date"].ToString().Trim() == ""
//										? "Last Date: No Last Date" 
//										: "Last Date: " +bv_info.Rows[0]["last_date"] 
//									: "No Last Date";
//			if(!for_email)
//				{
//				bvsb.AppendFormat(@"
//	<tr>
//		<td>{0}</td>
//		<td>{1}</td>
//		<td>{2} -- {3}</td>
//	</tr>", 
//				dsn,
//				name,
//				bv_name,
//				last_date
//				);
//				}
//			else
//				{
//				mailed_report_sb.AppendFormat("{0}\t{1}\t\t{2}{3}", dsn, name, bv_name, last_date);
//				}
//			}
//			if(!for_email)
//				{
//				bvsb.Append("\n</table>");
//				}
//			else
//				{
//				mailed_report_sb.Append("\n\n\n");
//				}
//		return bvsb.ToString();
//		}
//	private void do_nesi_merge(MySqlConnection conn, ref StringBuilder message, string query)
//		{
//		var result		= "";
//		try
//			{
//			Toolbox.doSQL_void(conn, query,null);
//			result			= "<div style='font-style:oblique'><b>QUERY:</b> "+query+"</div>";
//			}
//		catch (Exception ee)
//			{
//			result			= "<div style='color:#f00'><b>ERROR:</b>"+ee.Message+"</div><div style='font-style:oblique'><b>ASSOCIATED QUERY:</b>"+query+"</div>";
//			}
//		message.Append(result);
//		}
//	private void do_bv_merge(PsqlConnection conn, ref StringBuilder message, string query, string dsn)
//		{
//		var result		= "";
//		try
//			{
//			BVDB.doSQL_void(conn, query,null);
//			result			= "<div style='font-style:oblique'><b>QUERY:</b> "+query+"</div>";
//			}
//		catch (Exception ee)
//			{
//			result			= "<div style='color:#f00'><b>ERROR:</b>"+ee.Message+"</div><div style='font-style:oblique'><b>ASSOCIATED QUERY:</b>"+query+"</div>";
//			}
//		message.Append(result);
//		}

//	private void merge_customer(string _to_customer_id, string _mergetype)
//		{
//		using(var conn = Toolbox.connect())
//			{
//		var companyIDS = Toolbox.doSQL_dt(conn, @"SELECT
//    dsn,
//    NAME
//FROM
//    business_unit a
//    INNER JOIN tax_entity b
//        ON b.id = a.tax_entity_id
//WHERE b.sync_customers", null);
//		var to_customer = new NECustomer(Convert.ToInt32(_to_customer_id));
//	//	UsedInBV(to_customer.Customer_Number, true);
//		var phonematch = !string.IsNullOrEmpty(_q["force_merge"]) ? "" : getPhoneNumberForMatch(_to_customer_id);
//		//string namematch = getNameForMatch(vendorid);
//		var emailmessage = new StringBuilder();
//		var shortmessage = "";

//		shortmessage = "Customer " + to_customer.Customer_Number + " has been made the true customer \n";

//		var CustomerTable = new DataTable();
//		switch (_mergetype)
//			{
//			case "PhoneNumber":
//				CustomerTable				= getAllCustID(phonematch);
//			break;
//			case "CustomerName":
//				CustomerTable				= getAllCustIDName(_to_customer_id);
//			break;
//			default:
//				CustomerTable				= Toolbox.doSQL_dt(conn,@"SELECT customer_id, customer_number, customer_name FROM customer WHERE customer_id = @v0  LIMIT 1", new object[] {  _merge_from_id } );
//			break;
//			}
		
//		emailmessage.Append("<div style='color:#000;font-size:20px;font-weight:bold;'><u>NESI</u></div><div style='padding-left:10px;'>");
//		foreach (DataRow row in CustomerTable.Rows)
//			{
//			var from_customer_id = row["customer_id"].ToString();
//			var from_customer_number = row["customer_number"].ToString();
//			var from_customer_name = row["customer_name"].ToString();
//			if (from_customer_id != _to_customer_id)
//				{
//				UsedInBV(from_customer_number, true);
//				shortmessage += string.Format("{0} - {1} has been merged.\n", from_customer_number, from_customer_name);
//				#region NESI - Work Orders
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE woprog SET woprog_customer_id = '{0}', woprog_customername = ""{2}"" WHERE woprog_customer_id = '{1}'", _to_customer_id, from_customer_id, to_customer.Customer_Name));
//				#endregion NESI - Work Orders
//				#region NESI - Quotes
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE quote_master SET customer_id = '{0}' WHERE customer_id = '{1}'", _to_customer_id, from_customer_id));
//				#endregion NESI - Quotes
//				#region NESI - Timesheet
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE membertime SET membertime_customer_id = '{0}', membertime_cust_no = '{2}', membertime_customer_name = ""{3}"" WHERE membertime_customer_id = '{1}'", _to_customer_id, from_customer_id, to_customer.Customer_Number, to_customer.Customer_Name));
//				#endregion NESI - Timesheet
//				#region NESI - Customer Status
//					if(_mergetype == "") // Direct merge via querystring.
//						{
//						do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE customer SET customer_status = '3', customer_hold = 'F' WHERE customer_id = '{0}'", _to_customer_id));
//						}
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE customer SET customer_status = '4', customer_hold = 'T' WHERE customer_id = '{0}'", from_customer_id));
//				#endregion NESI - Customer Status
//				#region NESI - Contacts
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE contact SET contact_cust_id = '{0}', contact_bvno = '{1}' WHERE contact_cust_id = '{2}' AND contact_type = 'Customer'", _to_customer_id, to_customer.Customer_Number, from_customer_id, from_customer_number));
//				#endregion NESI - Contacts
//				#region NESI - address - address_table_id
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"call do_merge_address({1},{0})", _to_customer_id, from_customer_id));
//				#endregion NESI - address
//				#region NESI - customer_asset - customer_id
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE customer_asset SET customer_id = '{0}' WHERE customer_id = '{1}'", _to_customer_id, from_customer_id));
//				#endregion NESI - customer_asset
//				#region NESI - poprog_header - poprog_customer_id
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE poprog_header SET poprog_customer_id = '{0}' WHERE poprog_customer_id = '{1}'", _to_customer_id, from_customer_id));
//				#endregion NESI - poprog_header
//				#region NESI - customer_rate - customer_id
//				// Need to check if the to_customer has rates, if they do, delete the from_customer_rates, if they don't, change the customer_ids
//					var custrate_c	= Toolbox.doSQL_int(@"SELECT COUNT(id) FROM customer_rate WHERE customer_id = @v0", _to_customer_id);
//					if(custrate_c == 0)
//						{
//						do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE customer_rate SET customer_id = '{0}' WHERE customer_id = '{1}'", _to_customer_id, from_customer_id));				
//						}
//					else
//						{
//						// Delete the rates on the from_customer.
//						// MATT: Granted I realize this isn't a "MERGE", but I want to at least capture the query used.
//						do_nesi_merge(conn, ref emailmessage, string.Format(@"DELETE FROM customer_rate WHERE customer_id = '{0}'", from_customer_id));
//						}
//				#endregion NESI - customer_rate
//				#region NESI - customer_sales_properties - customer_id
//					// MATT: On second thought, this probably shouldn't happen... the to_customer already has properties.. if they need to be referenced after the fact, this should stay.
//					//do_nesi_merge(ref emailmessage, string.Format(@"UPDATE customer_sales_properties SET customer_id = '{0}' WHERE customer_id = '{1}'", to_customer_id, from_customer_id));
//				#endregion NESI - customer_sales_properties
//				#region NESI - erjob - erjob_customer_id
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE erjob SET erjob_customer_id = '{0}' WHERE erjob_customer_id = '{1}'", _to_customer_id, from_customer_id));
//				#endregion NESI - erjob
//				#region NESI - expense_reimbursement - customer_id
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE expense_reimbursement SET customer_id = '{0}' WHERE customer_id = '{1}' ", _to_customer_id, from_customer_id));
//				#endregion NESI - expense_reimbursement
//				#region NESI - historycust - historycust_customer_id
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE historycust SET historycust_customer_id = '{0}' WHERE historycust_customer_id = '{1}'", _to_customer_id, from_customer_id));
//				#endregion NESI - historycust
//				#region NESI - inventory_consignment - customer_id
//					do_nesi_merge(conn, ref emailmessage, string.Format(@"UPDATE inventory_consignment SET customer_id = '{0}' WHERE customer_id = '{1}'", _to_customer_id, from_customer_id));
//				#endregion NESI - inventory_consignment
//				}
//			}


//		emailmessage.Append("</div><hr/><div style='color:#000;font-size:20px;font-weight:bold;'><u>BV</u></div><div style='padding-left:10px;'>");
//		foreach (DataRow row in CustomerTable.Rows)
//			{
//			var from_customer_id = row["customer_id"].ToString();
//			var from_customer_number = row["customer_number"].ToString();
//			emailmessage.Append("<div style='color:#000;font-size:14px;font-weight:bold;'><u>Working On: "+from_customer_number+"</u></div><div style='padding-left:10px;'>");
//			if (from_customer_id != _to_customer_id)
//				{
//				foreach (DataRow CompanyRow in companyIDS.Rows)
//					{
//					var dsn			= CompanyRow["dsn"].ToString();
//					using(var bv_conn = BVDB.connect(dsn))
//						{
//					var exists_in_bv	= _tools.getSQL_int(@"SELECT COUNT(cus_no) FROM customer WHERE cus_no = ? ", dsn , new object[] {  from_customer_number } ) > 0;
//					if(exists_in_bv)
//						{
//						emailmessage.Append("<div style='color:#090;'><u>"+from_customer_number+" exists in "+dsn+"!</u></div>");
//						#region BV - Sales Orders
//						do_bv_merge(bv_conn, ref emailmessage, string.Format(@"UPDATE SALES_ORDER_HEADER SET CUST_NO = '{0}', CUST_NAME = (SELECT NAME FROM CUSTOMER WHERE CUS_NO = '{0}') WHERE CUST_NO = '{1}'", to_customer.Customer_Number, from_customer_number), dsn);
//						#endregion BV - Sales Orders
//						#region BV - FROM customer on hold, add notes stating it was merged
//						var current_notes			= BVDB.getSQL_string(bv_conn,@"SELECT notes FROM customer  WHERE cus_no =?", new object[] { from_customer_number });
//						do_bv_merge(bv_conn, ref emailmessage, string.Format(@"UPDATE CUSTOMER SET HOLD = 1, NOTES = '{0} - MERGED WITH {1}' WHERE CUS_NO = '{2}'",current_notes.Replace("'", "''").Trim(),to_customer.Customer_Number, from_customer_number) , dsn);
//						#endregion BV - FROM customer on hold, add notes stating it was merged					
//						#region BV - Address update, move to new customer
//						// Update ADDRESS - RECORD_TYPE = 'CUST', ID is CEV_NO
//                        //JA 2017-06-08 commented this out. Next time we use the customer it will get synced anyways.
//						//do_bv_merge(bv_conn, ref emailmessage, string.Format(@"UPDATE ADDRESS SET CEV_NO = '{1}', ADDR_TYPE = 'S' WHERE RECORD_TYPE = 'CUST' AND CEV_NO = '{0}'", from_customer_number, to_customer.Customer_Number), dsn);
//						#endregion BV - Address update, move to new customer
//						#region BV - Update AR_TRANSACTIONS - ID is CUST
//						do_bv_merge(bv_conn, ref emailmessage, string.Format(@"UPDATE AR_TRANSACTIONS SET CUST = '{1}' WHERE CUST = '{0}'", from_customer_number, to_customer.Customer_Number), dsn);
//						#endregion BV - Update AR_TRANSACTIONS - ID is CUST
//						#region BV - Update GL_TRANSACTIONS - MF_WHO = 'Cust.', MF_KEY = customer_number
//						do_bv_merge(bv_conn, ref emailmessage, string.Format(@"UPDATE GL_TRANSACTIONS SET MF_KEY = '{1}' WHERE MF_WHO = 'Cust.' AND MF_KEY = '{0}'", from_customer_number, to_customer.Customer_Number), dsn);
//						#endregion BV - Update GL_TRANSACTIONS - MF_WHO = 'Cust.', MF_KEY = customer_number
//						#region BV - Update SALES_HISTORY_HEADER - ID = CUST_NO
//						do_bv_merge(bv_conn, ref emailmessage, string.Format(@"UPDATE SALES_HISTORY_HEADER SET CUST_NO = '{1}' WHERE CUST_NO = '{0}'", from_customer_number, to_customer.Customer_Number), dsn);
//						#endregion BV - Update SALES_HISTORY_HEADER - ID = CUST_NO
//						}
//					else
//						{
//						emailmessage.Append("<div style='color:#f00;'><b>ERROR:</b>"+from_customer_number+" doesn't exist in "+dsn+"!</div>");
//						}
//						}
//					}
//				}
//				emailmessage.Append("</div>");
//			}
//		emailmessage.Append("</div>");
//		//to_customer.sync_bvs(current_user);
//		var mail = new NeEMail();
//		mail.To =  "ar@newelectric.com";
//		//mail.MailTo = "hagens@newelectric.ca";
//		mail.CC = "debug@newelectric.com";
//		mail.isHTML = true;
//		mail.Subject = "Customers Have Been Merged";
//		mail.From = "noreply@newelectric.com";
//		mail.Body = shortmessage + "\n\n\n" + mailed_report_sb + "\n\n\nDebugging Information\n" + emailmessage;
//		mail.Send();

//		report.InnerHtml = emailmessage.ToString();
//			}
//		}
	}
