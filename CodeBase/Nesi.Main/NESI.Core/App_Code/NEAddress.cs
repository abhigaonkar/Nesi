using System;
using System.Data;
using System.Collections;
using System.Threading.Tasks;
using DevExpress.Xpo;
using System.Linq;
//using nesi.bv;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NEAddress
	/// </summary>
	public class NEAddress
	{
		#region privates
		private int _ID;
		private string _Table;
		private int _Table_ID;
		private string _Type;
		private string _Desc;
		private string _Addr1;
		private string _Addr2;
		private string _Addr3;
		private string _Addr4;
		private string _City;
		private string _Prov;
		private string _Postal;
		private string _Country;
		private string _PhoneArea = "NA";
		private string _PhoneNumber;
		private string _PhoneFirst;
		private string _PhoneLast;
		private string _PhoneExt;
		private string _FaxNumber;
		private string _FaxArea = "NA";
		private string _FaxFirst;
		private string _FaxLast;
		private string _Email;
		private string _Web;
		private string _SellPrice;
		private string _Terr;
		private string _SalesPerson;
		private string _Ship;
		private BVContact _bv_contact1 = new BVContact();
		private BVContact _bv_contact2 = new BVContact();
		private BVContact _bv_contact3 = new BVContact();
		private int _Tax1_Consol;
		private int _Tax2_Consol;
		private int _Tax3_Consol;
		private int _Tax4_Consol;
		private int _Tax1;
		private int _Tax2;
		private int _Tax3;
		private int _Tax4;
		private string _Tax1Exempt;
		private string _Tax2Exempt;
		private string _Tax3Exempt;
		private string _Tax4Exempt;
		private string _GPS_Coords;
		private string _RVAccount;
		private string _RVAccount_Consol;
		private DataSet _phonenumbers = new DataSet();
		private DataSet _faxnumbers;
        private bool _Active;
        private customer_sales_properties _csp;
		private void populate_phonenumbers()
		{
			var phonenumber = "";
			var faxnumber = "";
			var p = new NePhoneNumbers();
			//_phonenumbers		= new NePhoneNumbers().GetNePhoneNumbers(Convert.ToInt16(_ID), "Address", "%", "LandLine");
			phonenumber = p.GetDefaultPhoneNumbers(id, "Address", "1", "LandLine");
			//_faxnumbers			= new NePhoneNumbers().GetNePhoneNumbers(Convert.ToInt16(_ID), "Address", "%", "Fax");
			faxnumber = p.GetDefaultPhoneNumbers(id, "Address", "1", "Fax");
			//_PhoneExt			= ""; // This should be populated...
			phonenumber = phonenumber.Replace(" ", "");
			faxnumber = faxnumber.Replace(" ", "");
			_PhoneArea = phonenumber.Length < 10 ? "NA" : phonenumber.Substring(0, 3);
			_PhoneFirst = phonenumber.Length < 10 ? "NA" : phonenumber.Substring(3, 3);
			_PhoneLast = phonenumber.Length < 10 ? "NA" : phonenumber.Substring(6, 4);
			_PhoneNumber = phonenumber.Length < 10 ? "NA" : string.Format("({0}) {1}-{2}", _PhoneArea, _PhoneFirst, _PhoneLast);
			_FaxArea = faxnumber.Length < 10 ? "NA" : faxnumber.Substring(0, 3);
			_FaxFirst = faxnumber.Length < 10 ? "NA" : faxnumber.Substring(3, 3);
			_FaxLast = faxnumber.Length < 10 ? "NA" : faxnumber.Substring(6, 4);
			_FaxNumber = faxnumber.Length < 10 ? "NA" : string.Format("({0}) {1}-{2}", _FaxArea, _FaxFirst, _FaxLast);
		}
		#endregion privates
		#region publics
		public customer_sales_properties csp { get { return _csp; } set { _csp = value; } }
		public int id { get; set; }
		public string Table { get { return _Table; } set { _Table = value; } }
		public int Table_ID { get { return _Table_ID; } set { _Table_ID = value; } }
		public string Type { get { return _Type; } set { _Type = value; } }
		public BVContact BVContact1 { get { return _bv_contact1; } set { _bv_contact1 = value; } }
		public BVContact BVContact2 { get { return _bv_contact2; } set { _bv_contact2 = value; } }
		public BVContact BVContact3 { get { return _bv_contact3; } set { _bv_contact3 = value; } }
		public string Desc { get { return _Desc; } set { _Desc = value; } }
		public string Addr1 { get { return _Addr1; } set { _Addr1 = value; } }
		public string Addr2 { get { return _Addr2; } set { _Addr2 = value; } }
		public string Addr3 { get { return _Addr3; } set { _Addr3 = value; } }
		public string Addr4 { get { return _Addr4; } set { _Addr4 = value; } }
		public string City { get { return _City; } set { _City = value; } }
		public string Prov { get { return _Prov; } set { _Prov = value; } }
		public string Postal { get { return _Postal; } set { _Postal = value; } }
		public string Country { get { return _Country; } set { _Country = value; } }
		public string facebook { get; set; }
		public string twitter { get; set; }
		public string linkedin { get; set; }
		/// <summary>
		/// <para>Provides a pre-concatenated phone number.</para>
		/// <para>Resulting phone mask (###) ###-####</para>
		/// </summary>
		public string PhoneNumber { get { return _PhoneNumber; } set { _PhoneNumber = value; } }
		public string PhoneArea { get { return _PhoneArea; } set { _PhoneArea = value; } }
		public string Phonefirst { get { return _PhoneFirst; } set { _PhoneFirst = value; } }
		public string PhoneLast { get { return _PhoneLast; } set { _PhoneLast = value; } }
		public string PhoneExt { get { return _PhoneExt; } set { _PhoneExt = value; } }
		/// <summary>
		/// <para>Provides a pre-concatenated fax number.</para>
		/// <para>Resulting fax # mask (###) ###-####</para>
		/// </summary>
		public string FaxNumber { get { return _FaxNumber; } set { _FaxNumber = value; } }
		public string FaxArea { get { return _FaxArea; } set { _FaxArea = value; } }
		public string FaxFirst { get { return _FaxFirst; } set { _FaxFirst = value; } }
		public string FaxLast { get { return _FaxLast; } set { _FaxLast = value; } }
		public string Email { get { return _Email; } set { _Email = value; } }
		public string Web { get { return _Web; } set { _Web = value; } }
		public string SellPrice { get { return _SellPrice; } set { _SellPrice = value; } }
		public string Terr { get { return _Terr; } set { _Terr = value; } }
		public string SalesPerson { get { return _SalesPerson; } set { _SalesPerson = value; } }
		public string Ship { get { return _Ship; } set { _Ship = value; } }
		public int Tax1 { get { return _Tax1; } set { _Tax1 = value; } }
		public int Tax2 { get { return _Tax2; } set { _Tax2 = value; } }
		public int Tax3 { get { return _Tax3; } set { _Tax3 = value; } }
		public int Tax4 { get { return _Tax4; } set { _Tax4 = value; } }
		public int Tax1_Consol { get { return _Tax1_Consol; } set { _Tax1_Consol = value; } }
		public int Tax2_Consol { get { return _Tax2_Consol; } set { _Tax2_Consol = value; } }
		public int Tax3_Consol { get { return _Tax3_Consol; } set { _Tax3_Consol = value; } }
		public int Tax4_Consol { get { return _Tax4_Consol; } set { _Tax4_Consol = value; } }
		public string Tax1Exempt { get { return _Tax1Exempt; } set { _Tax1Exempt = value; } }
		public string Tax2Exempt { get { return _Tax2Exempt; } set { _Tax2Exempt = value; } }
		public string Tax3Exempt { get { return _Tax3Exempt; } set { _Tax3Exempt = value; } }
		public string Tax4Exempt { get { return _Tax4Exempt; } set { _Tax4Exempt = value; } }
		public string GPS_Coords { get { return _GPS_Coords; } set { _GPS_Coords = value; } }
		public string RVAccount { get { return _RVAccount; } set { _RVAccount = value; } }
		public string RVAccount_Consol { get { return _RVAccount_Consol; } set { _RVAccount_Consol = value; } }

        public bool Active { get { return _Active; } set { _Active = value; } }
        public bool exists(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				return uow.GetObjectByKey<ne_xpo.cs.address>(_id) != null;
			}
		}
		public NEAddress(int _id)
		{
			load(_id);
		}
		private void load(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				if (exists(_id))
				{
					var a = uow.GetObjectByKey<ne_xpo.cs.address>(_id);
					id = _id;
					_Table = a.address_table;
					_Table_ID = a.address_table_id;
					_Type = a.address_type.ToString();
					_Desc = a.address_desc ?? "";
					_Addr1 = a.address_addr1;
					_Addr2 = a.address_addr2;
					_Addr3 = a.address_addr3;
					_Addr4 = a.address_addr4;
					_City = a.address_city;
					_Prov = a.address_prov;
					_Postal = a.address_postal;
					_Country = a.address_country;
					_Email = a.address_email;
					_Web = a.address_web;
					_SellPrice = a.address_sellprice;
					_Terr = a.address_terr;
					_SalesPerson = a.address_salesperson;
					_Ship = a.address_ship;

					_Tax1 = a.address_tax1;
					_Tax2 = a.address_tax2;
					_Tax3 = a.address_tax3;
					_Tax4 = a.address_tax4;

					_Tax1_Consol = a.address_tax1_consol;
					_Tax2_Consol = a.address_tax2_consol;
					_Tax3_Consol = a.address_tax3_consol;
					_Tax4_Consol = a.address_tax4_consol;

					_Tax1Exempt = a.address_taxex1;
					_Tax2Exempt = a.address_taxex2;
					_Tax3Exempt = a.address_taxex3;
					_Tax4Exempt = a.address_taxex4;

					_RVAccount = a.address_rvaccountno;
					_RVAccount_Consol = a.address_rvaccountno_consol;
					_GPS_Coords = a.address_gps_coordinates;

					_bv_contact1.Name = a.address_contactname1;
					_bv_contact1.Phone_Area = a.address_contactphonearea1;
					_bv_contact1.Phone_First = a.address_contactphonefirst1;
					_bv_contact1.Phone_Last = a.address_contactphonelast1;
					_bv_contact1.Phone_Ext = a.address_contactphoneext1;
					_bv_contact1.Fax_Area = a.address_contactfaxarea1;
					_bv_contact1.Fax_First = a.address_contactfaxfirst1;
					_bv_contact1.Fax_Last = a.address_contactfaxlast1;
					_bv_contact1.Email = a.address_contactemail1;

					_bv_contact2.Name = a.address_contactname2;
					_bv_contact2.Phone_Area = a.address_contactphonearea2;
					_bv_contact2.Phone_First = a.address_contactphonefirst2;
					_bv_contact2.Phone_Last = a.address_contactphonelast2;
					_bv_contact2.Phone_Ext = a.address_contactphoneext2;
					_bv_contact2.Fax_Area = a.address_contactfaxarea2;
					_bv_contact2.Fax_First = a.address_contactfaxfirst2;
					_bv_contact2.Fax_Last = a.address_contactfaxlast2;
					_bv_contact2.Email = a.address_contactemail2;

					_bv_contact3.Name = a.address_contactname3;
					_bv_contact3.Phone_Area = a.address_contactphonearea3;
					_bv_contact3.Phone_First = a.address_contactphonefirst3;
					_bv_contact3.Phone_Last = a.address_contactphonelast3;
					_bv_contact3.Phone_Ext = a.address_contactphoneext3;
					_bv_contact3.Fax_Area = a.address_contactfaxarea3;
					_bv_contact3.Fax_First = a.address_contactfaxfirst3;
					_bv_contact3.Fax_Last = a.address_contactfaxlast3;
					_bv_contact3.Email = a.address_contactemail3;


					_csp = new customer_sales_properties(_id);
					populate_phonenumbers();

					facebook = a.facebook;
					twitter = a.twitter;
					linkedin = a.linkedin;
                    _Active = a.Active;

					_PhoneArea = a.address_phonearea;
					_PhoneFirst = a.address_phonefirst;
					_PhoneLast = a.address_phonelast;

					_FaxArea = a.address_faxarea;
					_FaxFirst = a.address_faxfirst;
					_FaxLast = a.address_faxlast;
				}
			}
		}
		public ArrayList ServiceAddresses(int _customer_id)
		{
			var a = new ArrayList();
			var _shipping_addresses = Toolbox.doSQL_dt(@" SELECT a.address_id, a.address_table, a.address_table_id, a.address_type, a.address_desc, a.address_addr1, a.address_addr2, a.address_addr3, a.address_addr4, a.address_city, a.address_prov, a.address_postal, a.address_country, a.address_phonearea, a.address_phonefirst, a.address_phonelast, a.address_phoneext, a.address_phonefull, a.address_faxarea, a.address_faxfirst, a.address_faxlast, a.address_email, a.address_web, a.address_sellprice, a.address_terr, a.address_salesperson, a.address_ship, a.address_tax1, a.address_tax2, a.address_tax3, a.address_tax4, a.address_taxex1, a.address_taxex2, a.address_taxex3, a.address_taxex4, a.address_contactname1, a.address_contactphonearea1, a.address_contactphonefirst1, a.address_contactphonelast1, a.address_contactphoneext1, a.address_contactfaxarea1, a.address_contactfaxfirst1, a.address_contactfaxlast1, a.address_contactemail1, a.address_contactname2, a.address_contactphonearea2, a.address_contactphonefirst2, a.address_contactphonelast2, a.address_contactphoneext2, a.address_contactfaxarea2, a.address_contactfaxfirst2, a.address_contactfaxlast2, a.address_contactemail2, a.address_contactname3, a.address_contactphonearea3, a.address_contactphonefirst3, a.address_contactphonelast3, a.address_contactphoneext3, a.address_contactfaxarea3, a.address_contactfaxfirst3, a.address_contactfaxlast3, a.address_contactemail3, a.address_gps_coordinates, a.address_rvaccountno, a.facebook, a.twitter, a.linkedin, a.address_rvaccountno_consol, b.status_id csp_status_id, b.industry csp_industry, b.sector csp_sector, b.naics_code csp_naics_code, b.employee_size csp_employee_size, b.do_at_location csp_do_at_location, b.affiliated_companies csp_affiliated_companies, b.known_suppliers csp_known_suppliers, b.known_competitors csp_known_competitors, b.why_choose csp_why_choose, b.origin csp_origin, b.call_cycle csp_call_cycle, b.year_end csp_year_end, b.project_mgr_member_id csp_project_mgr_member_id, b.controls_mgr_member_id csp_controls_mgr_member_id, b.discount_pct csp_discount_pct, b.decision_maker csp_decision_maker, b.account_code csp_account_code, b.next_followup_date csp_next_followup_date, b.next_followup_notes csp_next_followup_notes, b.job_budget_threshold csp_job_budget_threshold, b.po_required csp_po_required, b.confirmed_po_required csp_confirmed_po_required, b.confirmed_tax_exempt csp_confirmed_tax_exempt, b.isr_member_id csp_isr_member_id, b.osr_member_id csp_osr_member_id, b.ram_member_id csp_ram_member_id, b.mam_member_id csp_mam_member_id, b.cisr_member_id csp_cisr_member_id, b.notes_sales csp_notes_sales, b.notes_public csp_notes_public, b.account_manager csp_account_manager, IFNULL(c.phone_numbers_number, '') phone_phonenumber, IFNULL(d.phone_numbers_number, '') phone_faxnumber, a.address_tax1_consol, a.address_tax2_consol, a.address_tax3_consol, a.address_tax4_consol FROM address a LEFT JOIN customer_sales_properties b ON a.address_id = b.address_id LEFT JOIN phone_numbers c ON c.phone_numbers_active = '1' AND c.phone_numbers_type = 'Address' AND c.phone_numbers_table_id = a.address_id AND c.phone_numbers_default = 1 AND c.phone_numbers_comm_type = 'LandLine' LEFT JOIN phone_numbers d ON d.phone_numbers_active = '1' AND d.phone_numbers_type = 'Address' AND d.phone_numbers_table_id = a.address_id AND d.phone_numbers_default = 1 AND d.phone_numbers_comm_type = 'LandLine' WHERE a.address_table_id = @v0  AND (a.address_table = 'Customer' or a.address_table = 'Worksite') AND a.address_type='S' and a.Active=true ", new object[] { _customer_id });
			if (_shipping_addresses.Rows.Count > 0)
			{
				foreach (DataRow _dr in _shipping_addresses.Rows)
				{
					var addr = new NEAddress();
					addr.csp = new customer_sales_properties();
					addr.id = (int)_dr["address_id"];
					addr.Table = _dr["address_table"].ToString();
					addr.Table_ID = Convert.ToInt32(_dr["address_table_id"]);
					addr.Type = _dr["address_type"].ToString();
					addr.Desc = _dr["address_desc"].ToString().Trim();
					addr.Addr1 = _dr["address_addr1"].ToString();
					addr.Addr2 = _dr["address_addr2"].ToString();
					addr.Addr3 = _dr["address_addr3"].ToString();
					addr.Addr4 = _dr["address_addr4"].ToString();
					addr.City = _dr["address_city"].ToString();
					addr.Prov = _dr["address_prov"].ToString();
					addr.Postal = _dr["address_postal"].ToString();
					addr.Country = _dr["address_country"].ToString();
					addr.Email = _dr["address_email"].ToString();
					addr.Web = _dr["address_web"].ToString();
					addr.SellPrice = _dr["address_sellprice"].ToString();
					addr.Terr = _dr["address_terr"].ToString();
					addr.SalesPerson = _dr["address_salesperson"].ToString();
					addr.Ship = _dr["address_ship"].ToString();
					addr.Tax1 = Convert.ToInt32(_dr["address_tax1"]);
					addr.Tax2 = Convert.ToInt32(_dr["address_tax2"]);
					addr.Tax3 = Convert.ToInt32(_dr["address_tax3"]);
					addr.Tax4 = Convert.ToInt32(_dr["address_tax4"]);
					addr.Tax1_Consol = Toolbox.ReturnZeroIfNull_int(_dr["address_tax1_consol"]);
					addr.Tax2_Consol = Toolbox.ReturnZeroIfNull_int(_dr["address_tax2_consol"]);
					addr.Tax3_Consol = Toolbox.ReturnZeroIfNull_int(_dr["address_tax3_consol"]);
					addr.Tax4_Consol = Toolbox.ReturnZeroIfNull_int(_dr["address_tax4_consol"]);
					addr.PhoneExt = _dr["address_phoneext"].ToString();
					addr.Tax1Exempt = _dr["address_taxex1"].ToString();
					addr.Tax2Exempt = _dr["address_taxex2"].ToString();
					addr.Tax3Exempt = _dr["address_taxex3"].ToString();
					addr.Tax4Exempt = _dr["address_taxex4"].ToString();
					addr.RVAccount = _dr["address_rvaccountno"].ToString();
					addr.RVAccount_Consol = _dr["Address_RVAccountNo_consol"].ToString();
					addr.GPS_Coords = _dr["address_gps_coordinates"].ToString();
					addr.BVContact1.Name = _dr["address_contactname1"].ToString();
					addr.BVContact1.Phone_Area = _dr["address_contactphonearea1"].ToString();
					addr.BVContact1.Phone_First = _dr["address_contactphonefirst1"].ToString();
					addr.BVContact1.Phone_Last = _dr["address_contactphonelast1"].ToString();
					addr.BVContact1.Phone_Ext = _dr["address_contactphoneext1"].ToString();
					addr.BVContact1.Fax_Area = _dr["address_contactfaxarea1"].ToString();
					addr.BVContact1.Fax_First = _dr["address_contactfaxfirst1"].ToString();
					addr.BVContact1.Fax_Last = _dr["address_contactfaxlast1"].ToString();
					addr.BVContact1.Email = _dr["address_contactemail1"].ToString();

					addr.BVContact2.Name = _dr["address_contactname2"].ToString();
					addr.BVContact2.Phone_Area = _dr["address_contactphonearea2"].ToString();
					addr.BVContact2.Phone_First = _dr["address_contactphonefirst2"].ToString();
					addr.BVContact2.Phone_Last = _dr["address_contactphonelast2"].ToString();
					addr.BVContact2.Phone_Ext = _dr["address_contactphoneext2"].ToString();
					addr.BVContact2.Fax_Area = _dr["address_contactfaxarea2"].ToString();
					addr.BVContact2.Fax_First = _dr["address_contactfaxfirst2"].ToString();
					addr.BVContact2.Fax_Last = _dr["address_contactfaxlast2"].ToString();
					addr.BVContact2.Email = _dr["address_contactemail2"].ToString();

					addr.BVContact3.Name = _dr["address_contactname3"].ToString();
					addr.BVContact3.Phone_Area = _dr["address_contactphonearea3"].ToString();
					addr.BVContact3.Phone_First = _dr["address_contactphonefirst3"].ToString();
					addr.BVContact3.Phone_Last = _dr["address_contactphonelast3"].ToString();
					addr.BVContact3.Phone_Ext = _dr["address_contactphoneext3"].ToString();
					addr.BVContact3.Fax_Area = _dr["address_contactfaxarea3"].ToString();
					addr.BVContact3.Fax_First = _dr["address_contactfaxfirst3"].ToString();
					addr.BVContact3.Fax_Last = _dr["address_contactfaxlast3"].ToString();
					addr.BVContact3.Email = _dr["address_contactemail3"].ToString();

					addr.facebook = _dr["facebook"].ToString();
					addr.twitter = _dr["twitter"].ToString();
					addr.linkedin = _dr["linkedin"].ToString();

					addr.csp.sector = _dr["csp_sector"].ToString();
					addr.csp.industry = Toolbox.ReturnZeroIfNull_int(_dr["csp_industry"]);
					addr.csp.naics_code = _dr["csp_naics_code"].ToString();
					addr.csp.employee_size = Toolbox.ReturnZeroIfNull_int(_dr["csp_employee_size"]);
					addr.csp.do_at_location = _dr["csp_do_at_location"].ToString();
					addr.csp.affiliated_companies = _dr["csp_affiliated_companies"].ToString();
					addr.csp.known_suppliers = _dr["csp_known_suppliers"].ToString();
					addr.csp.known_competitors = _dr["csp_known_competitors"].ToString();
					addr.csp.why_choose = _dr["csp_why_choose"].ToString();
					addr.csp.origin = _dr["csp_origin"] == DBNull.Value ? 1 : (int)_dr["csp_origin"];
					addr.csp.status_id = Toolbox.ReturnZeroIfNull_int(_dr["csp_status_id"]);
					addr.csp.call_cycle = Toolbox.ReturnZeroIfNull_int(_dr["csp_call_cycle"]);
					addr.csp.year_end = Toolbox.ReturnZeroIfNull_int(_dr["csp_year_end"]);
					addr.csp.project_mgr_member_id = Toolbox.ReturnZeroIfNull_int(_dr["csp_project_mgr_member_id"]);
					addr.csp.controls_mgr_member_id = Toolbox.ReturnZeroIfNull_int(_dr["csp_controls_mgr_member_id"]);
					addr.csp.discount_pct = Toolbox.ReturnZeroIfNull_int(_dr["csp_discount_pct"]);
					addr.csp.decision_maker = Toolbox.ReturnZeroIfNull_int(_dr["csp_decision_maker"]);
					addr.csp.account_code = _dr["csp_account_code"].ToString();
					addr.csp.next_followup_date = Toolbox.ReturnBlankDateTimeIfNull(_dr["csp_next_followup_date"]);
					addr.csp.next_followup_notes = _dr["csp_next_followup_notes"].ToString();
					addr.csp.job_budget_threshold = Toolbox.ReturnZeroIfNull_double(_dr["csp_job_budget_threshold"]);
					addr.csp.po_required = _dr["csp_po_required"] == DBNull.Value ? false : (bool)_dr["csp_po_required"];
					addr.csp.confirmed_po_required = _dr["csp_confirmed_po_required"] == DBNull.Value ? false : (bool)_dr["csp_confirmed_po_required"];
					addr.csp.confirmed_tax_exempt = _dr["csp_confirmed_tax_exempt"] == DBNull.Value ? false : (bool)_dr["csp_confirmed_tax_exempt"];
					addr.csp.customer_id = addr.Table_ID;
					addr.csp.address_id = addr.id;
					addr.csp.isr_member_id = Toolbox.ReturnZeroIfNull_int(_dr["csp_isr_member_id"]);
					addr.csp.osr_member_id = Toolbox.ReturnZeroIfNull_int(_dr["csp_osr_member_id"]);
					addr.csp.ram_member_id = Toolbox.ReturnZeroIfNull_int(_dr["csp_ram_member_id"]);
					addr.csp.mam_member_id = Toolbox.ReturnZeroIfNull_int(_dr["csp_mam_member_id"]);
					addr.csp.cisr_member_id = Toolbox.ReturnZeroIfNull_int(_dr["csp_cisr_member_id"]);
					addr.csp.am_member_id = Toolbox.ReturnZeroIfNull_int(_dr["csp_account_manager"]);

					addr.csp.notes_sales = _dr["csp_notes_sales"].ToString();
					addr.csp.notes_public = _dr["csp_notes_public"].ToString();
					addr.csp.previous_values = new customer_sales_properties
					{
						sector = addr.csp.sector,
						industry = addr.csp.industry,
						naics_code = addr.csp.naics_code,
						employee_size = addr.csp.employee_size,
						do_at_location = addr.csp.do_at_location,
						affiliated_companies = addr.csp.affiliated_companies,
						known_suppliers = addr.csp.known_suppliers,
						known_competitors = addr.csp.known_competitors,
						why_choose = addr.csp.why_choose,
						origin = addr.csp.origin,
						status_id = addr.csp.status_id,
						call_cycle = addr.csp.call_cycle,
						year_end = addr.csp.year_end,
						project_mgr_member_id = addr.csp.project_mgr_member_id,
						controls_mgr_member_id = addr.csp.controls_mgr_member_id,
						discount_pct = addr.csp.discount_pct,
						decision_maker = addr.csp.decision_maker,
						account_code = addr.csp.account_code,
						next_followup_date = addr.csp.next_followup_date,
						next_followup_notes = addr.csp.next_followup_notes,
						job_budget_threshold = addr.csp.job_budget_threshold,
						po_required = addr.csp.po_required,
						confirmed_po_required = addr.csp.confirmed_po_required,
						confirmed_tax_exempt = addr.csp.confirmed_tax_exempt,
						customer_id = addr.csp.customer_id,
						isr_member_id = addr.csp.isr_member_id,
						osr_member_id = addr.csp.osr_member_id,
						ram_member_id = addr.csp.ram_member_id,
						mam_member_id = addr.csp.mam_member_id,
						cisr_member_id = addr.csp.cisr_member_id,
						am_member_id = addr.csp.am_member_id,
						notes_sales = addr.csp.notes_sales,
						notes_public = addr.csp.notes_public
					};

					var phonenumber = _dr["phone_phonenumber"].ToString().Replace(" ", "");
					var faxnumber = _dr["phone_faxnumber"].ToString().Replace(" ", "");
					addr.PhoneArea = phonenumber.Length < 10 ? "NA" : phonenumber.Substring(0, 3);
					addr.Phonefirst = phonenumber.Length < 10 ? "NA" : phonenumber.Substring(3, 3);
					addr.PhoneLast = phonenumber.Length < 10 ? "NA" : phonenumber.Substring(6, 4);
					addr.PhoneNumber = phonenumber.Length < 10 ? "NA" : string.Format("({0}) {1}-{2}", addr.PhoneArea, addr.Phonefirst, addr.PhoneLast);
					addr.FaxArea = faxnumber.Length < 10 ? "NA" : faxnumber.Substring(0, 3);
					addr.FaxFirst = faxnumber.Length < 10 ? "NA" : faxnumber.Substring(3, 3);
					addr.FaxLast = faxnumber.Length < 10 ? "NA" : faxnumber.Substring(6, 4);
					addr.FaxNumber = faxnumber.Length < 10 ? "NA" : string.Format("({0}) {1}-{2}", addr.FaxArea, addr.FaxFirst, addr.FaxLast);

					if (addr.PhoneArea == "NA")
					{
						addr.PhoneArea = _dr["address_phonearea"].ToString();
						addr.Phonefirst = _dr["address_phonefirst"].ToString();
						addr.PhoneLast = _dr["address_phonelast"].ToString();
					}
					if (_FaxArea == "NA")
					{
						addr.FaxArea = _dr["address_faxarea"].ToString();
						addr.FaxFirst = _dr["address_faxfirst"].ToString();
						addr.FaxLast = _dr["address_faxlast"].ToString();
					}
					a.Add(addr);
				}
			}
			return a;
		}
		public NEAddress(int CustomerID, string TableType)
		{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM address WHERE address_table_id = @v0  AND address_table = @v1  and Active=true order by address_type limit 1", new object[] { CustomerID, TableType });
			if (dt.Rows.Count > 0)
			{
				var _dr = dt.Rows[0];
				id = Convert.ToInt32(_dr["Address_ID"]);
				_Table = _dr["Address_Table"].ToString();
				_Table_ID = Convert.ToInt32(_dr["Address_Table_ID"]);
				_Type = _dr["Address_Type"].ToString();
				_Desc = _dr["Address_Desc"].ToString().Trim();
				_Addr1 = _dr["address_addr1"].ToString();
				_Addr2 = _dr["address_addr2"].ToString();
				_Addr3 = _dr["address_addr3"].ToString();
				_Addr4 = _dr["address_addr4"].ToString();
				_City = _dr["Address_City"].ToString();
				_Prov = _dr["Address_Prov"].ToString();
				_Postal = _dr["Address_Postal"].ToString();
				_Country = _dr["Address_Country"].ToString();
				_Email = _dr["Address_Email"].ToString();
				_Web = _dr["Address_Web"].ToString();
				_SellPrice = _dr["Address_SellPrice"].ToString();
				_Terr = _dr["Address_Terr"].ToString();
				_SalesPerson = _dr["Address_SalesPerson"].ToString();
				_Ship = _dr["Address_Ship"].ToString();
				_Tax1 = Convert.ToInt32(_dr["Address_Tax1"]);
				_Tax2 = Convert.ToInt32(_dr["Address_Tax2"]);
				_Tax3 = Convert.ToInt32(_dr["Address_Tax3"]);
				_Tax4 = Convert.ToInt32(_dr["Address_Tax4"]);
				_Tax1_Consol = Convert.ToInt32(_dr["Address_Tax1_Consol"]);
				_Tax2_Consol = Convert.ToInt32(_dr["Address_Tax2_Consol"]);
				_Tax3_Consol = Convert.ToInt32(_dr["Address_Tax3_Consol"]);
				_Tax4_Consol = Convert.ToInt32(_dr["Address_Tax4_Consol"]);
				_PhoneExt = _dr["address_phoneext"].ToString();
				_Tax1Exempt = _dr["Address_TaxEx1"].ToString();
				_Tax2Exempt = _dr["Address_TaxEx2"].ToString();
				_Tax3Exempt = _dr["Address_TaxEx3"].ToString();
				_Tax4Exempt = _dr["Address_TaxEx4"].ToString();
				_RVAccount = _dr["Address_RVAccountNo"].ToString();
				_RVAccount_Consol = _dr["Address_RVAccountNo_Consol"].ToString();
				_GPS_Coords = _dr["Address_GPS_Coordinates"].ToString();
				_bv_contact1.Name = _dr["Address_ContactName1"].ToString();
				_bv_contact1.Phone_Area = _dr["Address_ContactPhoneArea1"].ToString();
				_bv_contact1.Phone_First = _dr["Address_ContactPhoneFirst1"].ToString();
				_bv_contact1.Phone_Last = _dr["Address_ContactPhoneLast1"].ToString();
				_bv_contact1.Phone_Ext = _dr["Address_ContactPhoneExt1"].ToString();
				_bv_contact1.Fax_Area = _dr["Address_ContactFaxArea1"].ToString();
				_bv_contact1.Fax_First = _dr["Address_ContactFaxFirst1"].ToString();
				_bv_contact1.Fax_Last = _dr["Address_ContactFaxLast1"].ToString();
				_bv_contact1.Email = _dr["Address_ContactEmail1"].ToString();

				_bv_contact2.Name = _dr["Address_ContactName2"].ToString();
				_bv_contact2.Phone_Area = _dr["Address_ContactPhoneArea2"].ToString();
				_bv_contact2.Phone_First = _dr["Address_ContactPhoneFirst2"].ToString();
				_bv_contact2.Phone_Last = _dr["Address_ContactPhoneLast2"].ToString();
				_bv_contact2.Phone_Ext = _dr["Address_ContactPhoneExt2"].ToString();
				_bv_contact2.Fax_Area = _dr["Address_ContactFaxArea2"].ToString();
				_bv_contact2.Fax_First = _dr["Address_ContactFaxFirst2"].ToString();
				_bv_contact2.Fax_Last = _dr["Address_ContactFaxLast2"].ToString();
				_bv_contact2.Email = _dr["Address_ContactEmail2"].ToString();

				_bv_contact3.Name = _dr["Address_ContactName3"].ToString();
				_bv_contact3.Phone_Area = _dr["Address_ContactPhoneArea3"].ToString();
				_bv_contact3.Phone_First = _dr["Address_ContactPhoneFirst3"].ToString();
				_bv_contact3.Phone_Last = _dr["Address_ContactPhoneLast3"].ToString();
				_bv_contact3.Phone_Ext = _dr["Address_ContactPhoneExt3"].ToString();
				_bv_contact3.Fax_Area = _dr["Address_ContactFaxArea3"].ToString();
				_bv_contact3.Fax_First = _dr["Address_ContactFaxFirst3"].ToString();
				_bv_contact3.Fax_Last = _dr["Address_ContactFaxLast3"].ToString();
				_bv_contact3.Email = _dr["Address_ContactEmail3"].ToString();

				facebook = _dr["facebook"].ToString();
				twitter = _dr["twitter"].ToString();
				linkedin = _dr["linkedin"].ToString();

				_csp = new customer_sales_properties(id);

				populate_phonenumbers();
				if (_PhoneArea == "NA")
				{
					_PhoneArea = _dr["Address_PhoneArea"].ToString();
					_PhoneFirst = _dr["Address_PhoneFirst"].ToString();
					_PhoneLast = _dr["Address_PhoneLast"].ToString();
				}
				if (_FaxArea == "NA")
				{
					_FaxArea = _dr["Address_FaxArea"].ToString();
					_FaxFirst = _dr["Address_FaxFirst"].ToString();
					_FaxLast = _dr["Address_FaxLast"].ToString();
				}
			}
		}
		public NEAddress()
		{

			//
			//  
			//
		}
		public int Save()
		{
			var address_id = 0;
			var count = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM address WHERE address_id = @v0 AND Active = True", new object[] { id });
            if (count == 0)
            {
                var countInactive = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM address WHERE address_id = @v0", new object[] { id });
                count = countInactive == 1 ? 1 : 0;
               
            }
          
           
			var sql = "";
            
          	if (count == 0)             // if there is no other address with that id number... 
                    {
                #region OLD INSERT STATEMENT 
                /*
                        sql = @"
        INSERT INTO address  
            (
            address_table,				
            address_table_id,			
            address_type,   
            address_desc,   
            address_addr1,   
            address_addr2,   
            address_addr3,   
            address_addr4,   
            address_city,   
            address_prov,   
            address_postal,   
            address_country,   
            address_phonearea,   
            address_phonefirst,   
            address_phonelast,
            address_phoneext,
            address_faxarea,   
            address_faxfirst,   
            address_faxlast,   
            address_email,   
            address_web,   
            address_sellprice,   
            address_terr,   
            address_salesperson,   
            address_ship,   
            address_tax1,   
            address_tax2,   
            address_tax3,   
            address_tax4,   
            address_taxex1,   
            address_taxex2,   
            address_taxex3,   
            address_taxex4,   
            address_gps_coordinates,   
            address_rvaccountno,
            facebook,
            twitter,
            linkedin,
            address_phonefull,
        Address_RVAccountNo_consol,
        address_tax1_consol,   
            address_tax2_consol,   
            address_tax3_consol,   
            address_tax4_consol,
            Active
            ) 
        VALUES 	(
            @v34, 
            @v0, 
            @v33,
            @v1,
            @v2,
            @v3,
            @v4, 
            @v32,
            @v5, 
            @v6, 
            @v7, 
            @v8, 
            @v9,  
            @v10,  
            @v11,
            @v31, 
            @v12,  
            @v13,  
            @v14,  
            @v15,  
            @v16,  
            @v17,  
            @v18,  
            @v19,  
            @v20,  
            @v21,  
            @v22,  
            @v23,  
            @v24,  
            @v25,  
            @v26,  
            @v27,  
            @v28,  
            @v29, 
            @v30,
            @v35,
            @v36,
            @v37,
            CONCAT(@v9, ' ', @v10, ' ', @v11),
        @v38,
        @v39,
        @v40,
        @v41,
        @v42,
            true)";
                        var paramObjects = new object[] {
                            _Table_ID,					// {0}
                            _Desc, 						// {1}
                            _Addr1, 	// {2}
                            _Addr2, 	// {3}
                            _Addr3, 	// {4}
                            _City, 		// {5}
                            _Prov, 						// {6}
                            _Postal, 					// {7}
                            _Country, 					// {8}
                            _PhoneArea, 				// {9}
                            _PhoneFirst, 				// {10}
                            _PhoneLast, 				// {11}
                            _FaxArea, 					// {12}
                            _FaxFirst, 					// {13}
                            _FaxLast, 					// {14}
                            _Email, 					// {15}
                            _Web, 						// {16}
                            _SellPrice, 				// {17}
                            _Terr, 						// {18}
                            _SalesPerson, 				// {19}
                            _Ship, 						// {20}
                            _Tax1, 						// {21}
                            _Tax2, 						// {22}
                            _Tax3, 						// {23}
                            _Tax4, 						// {24}
                            _Tax1Exempt, 				// {25}
                            _Tax2Exempt,				// {26}
                            _Tax3Exempt,				// {27}
                            _Tax4Exempt, 				// {28}
                            _GPS_Coords, 				// {29}
                            _RVAccount,					// {30}
                            _PhoneExt,					// {31}
                            _Addr4,	// {32}
                            _Type,						// {33}
                            _Table,						// {34}
                            facebook,		// {35}
                            twitter,		// {36}
                            linkedin,		// {37}
                            _RVAccount_Consol,					// {38}
                            _Tax1_Consol,//{39}
                            _Tax2_Consol,//{40}
                            _Tax3_Consol,//{41}
                            _Tax4_Consol //{42}
                        };

                        try
                        {
                            address_id = Toolbox.doSQL_return_id(sql, paramObjects);
                        }
                        catch (Exception ee)
                        {
                            Toolbox.do_errorLog_errorStack(ee);
                            throw ee;
                        } */
                #endregion OLD INSERT STATEMENT
                #region CUSTOMER SALES PROPERTIES
                /*     if (_Table == "Customer")
                             {
                                 // Need to initially create the customer_Sales_properties object.
                                 var this_csp = new customer_sales_properties(id);
                                 this_csp.customer_id = Table_ID;
                                 this_csp.save();
                             }*/
                #endregion CUSTOMER SALES PROPERTIES           

                #region BILLING PHONE NUMBER
                /*         var b_phone_id = Toolbox.doSQL_int(@"
                 Select ifnull((SELECT MAX(phone_numbers_id) 
                 FROM 
                     phone_numbers
                 WHERE 
                     phone_numbers_type = 'Address' AND 
                     phone_numbers_comm_type = 'LandLine' AND
                     phone_numbers_active = true AND
                     phone_numbers_table_id = @v0 LIMIT 1),0)", address_id);
                         var billingphone = b_phone_id == 0 ? new NePhoneNumbers() : new NePhoneNumbers(b_phone_id);
                         billingphone.phone_numbers_active = true;
                         billingphone.phone_numbers_added_date = DateTime.Today.Date.ToShortDateString();
                         billingphone.phone_numbers_comm_type = "LandLine";
                         billingphone.phone_numbers_default = true;
                         billingphone.phone_numbers_type = "Address";
                         billingphone.phone_numbers_table_id = Convert.ToInt32(address_id);
                         billingphone.phone_numbers_number = string.Format("{0} {1} {2}", _PhoneArea, _PhoneFirst, _PhoneLast);
                         billingphone.Save();*/

                #endregion BILLING PHONE NUMBER

                #region BILLING FAX DETAILS
                //        if ((_FaxArea.Length == 3) && (_FaxFirst.Length == 3) && (_FaxLast.Length == 4))
                //        {
                //            var b_fax_id = Toolbox.doSQL_int(@"
                //Select ifnull((SELECT MAX(phone_numbers_id) 
                //FROM 
                //    phone_numbers
                //WHERE 
                //    phone_numbers_type = 'Address' AND 
                //    phone_numbers_comm_type = 'Fax' AND
                //    phone_numbers_active = true AND
                //    phone_numbers_table_id =@v0 LIMIT 1),0)", address_id);
                //            var billingFax = b_fax_id == 0 ? new NePhoneNumbers() : new NePhoneNumbers(b_fax_id);
                //            billingFax.phone_numbers_active = true;
                //            billingFax.phone_numbers_added_date = DateTime.Today.Date.ToShortDateString();
                //            billingFax.phone_numbers_comm_type = "Fax";
                //            billingFax.phone_numbers_default = true;
                //            billingFax.phone_numbers_type = "Address";
                //            billingFax.phone_numbers_table_id = Convert.ToInt32(address_id);
                //            billingFax.phone_numbers_number = string.Format("{0} {1} {2}", _FaxArea, _FaxFirst, _FaxLast);
                //            billingFax.Save();
                //}
                #endregion BILLING FAX DETAILS

                return 0;
            }
			else  // if the id already exists... just update it.
			{
				sql = @"
UPDATE address SET   
	address_addr1			= @v0,
	address_addr2			= @v1,
	address_addr3			= @v2,
	address_addr4			= @v3,   
	address_desc			= @v4,
	address_city			= @v5,
	address_prov			= @v6,   
	address_postal			= @v7,
	address_country			= @v8,   
	address_phonearea		= @v9,
	address_phonefirst		= @v10,
	address_phonelast		= @v11,
	address_phoneext		= @v12,   
	address_faxarea			= @v13,
	address_faxfirst		= @v14,
	address_faxlast			= @v15,   
	address_email			= @v16,
	address_web				= @v17,
	address_gps_coordinates	= @v18,   
	address_sellprice		= @v19,   
	address_tax1			= @v20,
	address_tax2			= @v21,
	address_tax3			= @v22,
	address_tax4			= @v23,   
	address_taxex1			= @v24,
	address_taxex2			= @v25,
	address_taxex3			= @v26,
	address_taxex4			= @v27,  
	address_rvaccountno		= @v28,
	facebook				= @v30,
	twitter					= @v31,
	linkedin				= @v32,
	address_table			= @v33,
	address_phonefull		= CONCAT(@v9, ' ', @v10, ' ', @v11),
	Address_RVAccountNo_consol =  @v34,
    Active                     =  @39
 address_tax1_consol			= @v35,
	address_tax2_consol			= @v36,
	address_tax3_consol			= @v37,
	address_tax4_consol			= @v38   
WHERE 
	address_id				= @v29 
LIMIT 1";
				var paramObjects = new object[] {
					_Addr1, // {0}
					_Addr2,     // {1}
					_Addr3,     // {2}
					_Addr4,     // {3}
					_Desc,      // {4}
					_City,      // {5}
					_Prov,                      // {6}
					_Postal,                    // {7}
					_Country,                   // {8}
					_PhoneArea,                 // {9}
					_PhoneFirst,                // {10}
					_PhoneLast,                 // {11}
					_PhoneExt,                  // {12}
					_FaxArea,                   // {13}
					_FaxFirst,                  // {14}
					_FaxLast,                   // {15}
					_Email,                     // {16}
					_Web,                       // {17}
					_GPS_Coords,                // {18}
					_SellPrice,                 // {19}
					_Tax1,                      // {20}
					_Tax2,                      // {21}
					_Tax3,                      // {22}
					_Tax4,                      // {23}
					_Tax1Exempt,                // {24}
					_Tax2Exempt,                // {25}
					_Tax3Exempt,                // {26}
					_Tax4Exempt,                // {27}
					_RVAccount,                 // {28}
					id,                     // {29}
					facebook,   // {30}
					twitter,    // {31}
					linkedin,   // {32}
					_Table, // {{33}
					_RVAccount_Consol,              // {34}
					_Tax1_Consol,                       // {35}
					_Tax2_Consol,                       // {36}
					_Tax3_Consol,                       // {37}
					_Tax4_Consol,                        // {38}
                    _Active                             //{39}
				};
				Toolbox.doSQL_void(sql, paramObjects);
				address_id = id;
			}
			/*			int b_phone_id						= Toolbox.doSQL_int(@" SELECT IFNULL(MAX(phone_numbers_id), 0) FROM phone_numbers  WHERE phone_numbers_type = 'Address' AND phone_numbers_comm_type = 'LandLine' AND phone_numbers_active = true AND phone_numbers_table_id = @v0  LIMIT 1", new object[] { address_id });
						NePhoneNumbers billingphone				= b_phone_id == 0 ? new NePhoneNumbers() : new NePhoneNumbers(b_phone_id);
						billingphone.phone_numbers_active		= true;
						billingphone.phone_numbers_added_date	= DateTime.Today.Date.ToShortDateString();
						billingphone.phone_numbers_comm_type	= "LandLine";
						billingphone.phone_numbers_default		= true;
						billingphone.phone_numbers_type			= "Address";
						billingphone.phone_numbers_table_id		= address_id;
						billingphone.phone_numbers_number		= String.Format("{0} {1} {2}", _PhoneArea, _PhoneFirst, _PhoneLast);
						billingphone.Save();

						int b_fax_id							= Toolbox.doSQL_int(@" SELECT IFNULL(MAX(phone_numbers_id), 0) FROM phone_numbers  WHERE phone_numbers_type = 'Address' AND phone_numbers_comm_type = 'Fax' AND phone_numbers_active = true AND phone_numbers_table_id = @v0  LIMIT 1", new object[] { address_id });
						NePhoneNumbers billingFax				= b_fax_id == 0 ? new NePhoneNumbers() : new NePhoneNumbers(b_fax_id);
						billingFax.phone_numbers_active			= true;
						billingFax.phone_numbers_added_date		= DateTime.Today.Date.ToShortDateString();
						billingFax.phone_numbers_comm_type		= "Fax";
						billingFax.phone_numbers_default		= true;
						billingFax.phone_numbers_type			= "Address";
						billingFax.phone_numbers_table_id		= address_id;
						billingFax.phone_numbers_number			= String.Format("{0} {1} {2}", _FaxArea, _FaxFirst, _FaxLast);
						billingFax.Save();
			 */
			//	sync_bvs(this, new NeMember(1));
			id = address_id;
			return address_id;
		}
		/// <summary>
		/// ONLY use this for customers currently.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="_member"></param>
		//public static void sync_bvs(NEAddress a, NeMember _member)
		//{
		//	//var active_branches = shared.GetAllBVDSNs(true);
		//	//var cev_no = a.Table == "Customer" || a.Table == "Worksite"
		//	//	? new NECustomer(a.Table_ID).Customer_Number
		//	//	: new NEVendor(a.Table_ID).Number;

		//	//var _addr = Toolbox.doSQL_dt(@"SELECT * FROM Address WHERE Address_Table_ID = @v0 ", new object[] {  a.Table_ID } );
		//	//Parallel.ForEach(active_branches.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = 12 }, _dr =>
		//	//	 {
		//	//		 var dsn = _dr["consolidated_dsn"].ToString();
		//	//		 var is_consol = Convert.ToInt32(_dr["is_consol"]) == 1;
		//	//		 using (var conn = BVDB.connect(dsn))
		//	//		 {
		//	//			 var cache = BVControlDA.build_cache(conn, cev_no);
		//	//			 var record_type = a.Table == "Customer" || a.Table == "Worksite" ? "CUST" : "SUPP";
		//	//			 BV_thread_sync(conn, a, record_type, cev_no, _addr.Rows[0], _dr, cache, is_consol, _member, dsn);
		//	//		 }
		//	//	 });

		//	var cev_no = a.Table == "Customer" || a.Table == "Worksite"
		//		? new NECustomer(a.Table_ID).Customer_Number
		//		: new NEVendor(a.Table_ID).Number;

		//	using (var uow = new UnitOfWork())
		//	{
		//		var taxEntities = (from te in new XPQuery<ne_xpo.cs.tax_entity>(uow)
		//						   where te.is_active // && ( te.sync_customers || te.sync_vendors)
		//											  //where te.id == 26
		//						   select te).ToList();
		//		Parallel.ForEach(taxEntities, new ParallelOptions { MaxDegreeOfParallelism = 5 }, _dr =>
		//		{
		//			var _dsn = _dr.dsn;

		//			if (_dr.sync_customers && _dr.sync_vendors)
		//			{
		//				using (var conn = BVDB.connect(_dsn))
		//				{
		//					var cache = BVControlDA.build_cache(conn, cev_no);
		//					var record_type = a.Table == "Customer" || a.Table == "Worksite" ? "CUST" : "SUPP";

		//					BV_thread_sync(conn, a, record_type, cev_no, cache, _member, _dsn);
		//				}
		//			}
		//			//sync_bv(_dsn, _member);
		//		});
		//	}

		//}

		//public static void sync_S_adds_bv_from_mysql(string _dsn, NeMember _member)
		//{

		//	// Iterate through all the addresses
		//	using (var uow = new UnitOfWork())
		//	{
		//		var addressList = (from add in new XPQuery<ne_xpo.cs.address>(uow)
		//						   where ((add.address_table == "Customer" || add.address_table == "Worksite") && add.address_type.ToString() == "S")
		//						   select add.address_id).ToList();
		//		var total = addressList.Count;

		//		Parallel.ForEach(addressList, _id =>
		//		{
		//			try
		//			{
		//				var a = new NEAddress(_id);
		//				var cev_no = a.Table == "Customer" || a.Table == "Worksite"
		//						? new NECustomer(a.Table_ID).Customer_Number
		//						: new NEVendor(a.Table_ID).Number;

		//				using (var conn = BVDB.connect(_dsn))
		//				{
		//					var cache = BVControlDA.build_cache(conn, cev_no);
		//					var record_type = a.Table == "Customer" || a.Table == "Worksite" ? "CUST" : "SUPP";
		//					BV_thread_sync(conn, a, record_type, cev_no, cache, _member, _dsn);
		//				}

		//			}
		//			catch (Exception ee)
		//			{
		//				Toolbox.do_errorLog(ee);
		//			}
		//			finally
		//			{

		//			}
		//		});
		//	}

		//}

		//public static void BV_thread_sync(PsqlConnection conn, NEAddress addr, string record_type, string cev_no, DataSet cache, NeMember _user, string dsn)
		//{
		//	var count = addr.Type == "B"
		//		? cache.Tables["Address"].Select("id = '" + addr.id + "' OR addr_type = 'B'").Length
		//		: cache.Tables["Address"].Select("id = '" + addr.id + "'").Length;
		//	var is_new_addr = count == 0;
		//	var addr_table = addr.Table == "Vendor"
		//		? "SUPP"
		//		: "CUST";
		//	var bvaddr = is_new_addr
		//		? new Address()
		//		: new Address(cache.Tables["Address"], addr_table, cev_no, addr.Type, addr.id);
		//	bvaddr.cache = cache;

		//	// TODO : Change Account # Rev#41001 & Expense#50105
		//	// bvaddr.Account = is_consol ? "41000" : "41005";
		//	var tax_entity_id = Toolbox.doSQL_int(@"SELECT IFNULL((SELECT id FROM tax_entity WHERE dsn = @v0  LIMIT 1),0)", new object[] { dsn });

		//	var dtTaxes = Toolbox.doSQL_dt(@"SELECT * FROM tax_entity WHERE dsn = @v0", new object[] { dsn });

		//	var taxes = dtTaxes.Rows[0];
		//	var taxEntity = new NeTaxEntity(tax_entity_id);

		//	bvaddr.Account = taxEntity.gl_default_revenue == null ? "" : record_type == "CUST" ? "00000" : taxEntity.gl_default_expense.account_no;
		//	bvaddr.Name = addr.Desc.Trim();
		//	bvaddr.ID = addr.Type == "B" ? "" : addr.id.ToString();
		//	bvaddr.CEVNumber = cev_no;
		//	bvaddr.Address1 = addr.Addr1;
		//	bvaddr.Address2 = addr.Addr2;
		//	bvaddr.Address3 = addr.Addr3;
		//	bvaddr.Address4 = addr.Addr4;
		//	bvaddr.City = addr.City;
		//	bvaddr.CountryCode = addr.Country;
		//	bvaddr.EmailAddress = addr.Email;
		//	bvaddr.PhoneNumber = addr.PhoneArea + addr.Phonefirst + addr.PhoneLast;
		//	bvaddr.FaxNumber = addr.FaxArea + addr.FaxFirst + addr.FaxLast;
		//	bvaddr.RecordType = addr_table;
		//	bvaddr.AddressType = addr.Type;
		//	bvaddr.PostalCode = addr.Postal;
		//	bvaddr.Province = addr.Prov;
		//	var tax_1 = Convert.ToInt32(taxes["tax1"]);
		//	var tax_2 = Convert.ToInt32(taxes["tax2"]);
		//	var tax_3 = Convert.ToInt32(taxes["tax3"]);
		//	var tax_4 = Convert.ToInt32(taxes["tax4"]);

		//	bvaddr.Tax1 = addr.Tax1 == tax_1 ? addr.Tax1 : 0;
		//	bvaddr.Tax2 = addr.Tax2 == tax_2 ? addr.Tax2 : 0;
		//	bvaddr.Tax3 = addr.Tax3 == tax_3 ? addr.Tax3 : 0;
		//	bvaddr.Tax4 = addr.Tax4 == tax_4 ? addr.Tax4 : 0;
		//	bvaddr.Tax1Exempt = addr.Tax1Exempt;
		//	bvaddr.Tax2Exempt = addr.Tax2Exempt;
		//	bvaddr.Tax3Exempt = addr.Tax3Exempt;
		//	bvaddr.Tax4Exempt = addr.Tax4Exempt;
		//	bvaddr.Website = addr.Web;
		//	bvaddr.SellPriceLevel = string.IsNullOrEmpty(addr.SellPrice) ? 1 : Convert.ToInt32(addr.SellPrice);
		//	bvaddr.Save(conn, _user.Initials, is_new_addr);
		//}
	
		#endregion publics
	}
	public class BVContact
	{
		private string _name;
		private string _phone_area;
		private string _phone_first;
		private string _phone_last;
		private string _phone_ext;
		private string _fax_area;
		private string _fax_first;
		private string _fax_last;
		private string _email;
		private object _address_id;
		private object _contact_n;
		public BVContact()
		{
		}
		public BVContact(object address_id, object contact_number)
		{
			_address_id = address_id;
			_contact_n = contact_number;
		}

		public BVContact(object address_id, object contact_number, bool load_contact)
		{
			_address_id = address_id;
			_contact_n = contact_number;
			if (load_contact)
			{
				Load();
			}
		}

		public string Name { get { return _name; } set { _name = value; } }
		public object Address_ID { get { return _address_id; } set { _address_id = value; } }
		public string Phone_Area { get { return _phone_area; } set { _phone_area = value; } }
		public string Phone_First { get { return _phone_first; } set { _phone_first = value; } }
		public string Phone_Last { get { return _phone_last; } set { _phone_last = value; } }
		public string Phone_Ext { get { return _phone_ext; } set { _phone_ext = value; } }
		public string Fax_Area { get { return _fax_area; } set { _fax_area = value; } }
		public string Fax_First { get { return _fax_first; } set { _fax_first = value; } }
		public string Fax_Last { get { return _fax_last; } set { _fax_last = value; } }
		public string Email { get { return _email; } set { _email = value; } }
		public object Contact_n { get { return _contact_n; } set { _contact_n = value; } }
		public void Load()
		{
			var c = Toolbox.doSQL_dt(@"SELECT * FROM address WHERE address_id = @v0 and Active=true  LIMIT 1", new object[] { Address_ID }).Rows[0];
			Name = c[string.Format("Address_ContactName{0}", _contact_n)].ToString();
			Phone_Area = c[string.Format("Address_ContactPhoneArea{0}", _contact_n)].ToString();
			Phone_First = c[string.Format("Address_ContactPhoneFirst{0}", _contact_n)].ToString();
			Phone_Last = c[string.Format("Address_ContactPhoneLast{0}", _contact_n)].ToString();
			Phone_Ext = c[string.Format("Address_ContactPhoneExt{0}", _contact_n)].ToString();
			Fax_Area = c[string.Format("Address_ContactFaxArea{0}", _contact_n)].ToString();
			Fax_First = c[string.Format("Address_ContactFaxFirst{0}", _contact_n)].ToString();
			Fax_Last = c[string.Format("Address_ContactFaxLast{0}", _contact_n)].ToString();
			Email = c[string.Format("Address_ContactEmail{0}", _contact_n)].ToString();
		}
		public void Save()
		{
			var _n = _contact_n.ToString();
			if (_address_id.ToString() != "" && (_n == "1" || _n == "2" || _n == "3"))
			{
				Toolbox.doSQL_void(string.Format(@"
UPDATE address 
	SET
		Address_ContactName{0}			= @v0,
		Address_ContactPhoneArea{0}		= @v1,
		Address_ContactPhoneFirst{0}	= @v2,
		Address_ContactPhoneLast{0}		= @v3,
		Address_ContactPhoneExt{0}		= @v4,
		Address_ContactFaxArea{0}		= @v5,
		Address_ContactFaxFirst{0}		= @v6,
		Address_ContactFaxLast{0}		= @v7,
		Address_ContactEmail{0}			= @v8
WHERE
	address_id = @v9
LIMIT 1", _n), new object[] {

					_name,
					_phone_area,
					_phone_first,
					_phone_last,
					_phone_ext,
					_fax_area,
					_fax_first,
					_fax_last,
					_email,
					_address_id});
			}
			else
			{
				throw new Exception("Contact Information Not Set!");
			}
		}
	}
}