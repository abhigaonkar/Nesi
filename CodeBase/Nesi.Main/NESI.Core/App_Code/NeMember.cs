using System;
using System.Data;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Collections;
using System.Drawing.Printing;
using System.Drawing;
using System.Collections.Generic;
using nesi.core.security;
using System.DirectoryServices;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Text;
using DevExpress.Xpo;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using ne_xpo.cs;
using NESI.Common.Exceptions;
using NESI.Common.Password;
using NESI.Common.Models;

namespace nesi.core
{
	/// <summary>
	/// Summary description for Member
	/// </summary>
	[Serializable]
	public class NeMember
	{
		#region Variables
		private ArrayList _pages = new ArrayList();
		private bool _authenticated = false;
		private bool _tswatch = false;

		private decimal _vacation_amount_1 = 0;
		private decimal _vacation_amount_2 = 0;
		private decimal _vacation_amount_3 = 0;

		private double _holiday = 0;
		private double _holidaytaken = 0;
		private double _late = 0;
		private double _sick = 0;
		private double _unpaid = 0;

		private int _months_with_company = 0;
		private int _years_with_company = 0;
		private int _level = 0;
		private int _receive_stat_pay = 1;
		private int _timetostat = 0;
		private int _vacation_interval_1 = 0;
		private int _vacation_interval_2 = 0;
		private int _vacation_interval_3 = 0;

		private int _business_unit_id = 0;
		private int _membertype_id = 0;
		private int _session_id = 0;

		private string _EmployeeName = "";
		private string _add = "";
		private string _areacode = "";
		private string _birthdate = "";
		private string _hasdependants = "No";
		private string _bvnumber = "";
		private string _change_reason = "";
		private string _city = "";
		private string _company = "";
		private string _country = "";
		private string _dateadded = "";
		private string _driverslicence = "";
		private string _eleclicence = "";
		private string _email = "";
		private string _emergfirstname1 = "";
		private string _emergfirstname2 = "";
		private string _emerglastname1 = "";
		private string _emerglastname2 = "";
		private string _emergphonearea1 = "";
		private string _emergphonearea2 = "";
		private string _emergphonefirst1 = "";
		private string _emergphonefirst2 = "";
		private string _emergphonelast1 = "";
		private string _emergphonelast2 = "";
		private string _emplogon = "";
		private string _employeeID = "";
		private string _empnotes = "";
		private string _firstname = "";
		private string _fullname = "";
		private string _initials = "";
		private string _lastDate = "";
		private string _last_error = "";
		private string _lastname = "";
		private string _lastraise = "";
		private string _necellarea = "";
		private string _necellemail = "";
		private string _necellfirst = "";
		private string _necelllast = "";
		private string _neemail = "";
		private string _nextraise = "";
		private string _pass = "";
		private string _payroll_handler = "";
		private string _payrollid = "";
		private int _paytype_id = 0;
		//	private string _paytype					= "";       // "Hourly", "Salary", Subcontract
		//	private string _paytype_name			= "";       // "Hourly", "Salary", Subcontract
		private string _phoneextension = "";
		private string _phonefirst = "";
		private string _phonelast = "";
		private string _postal = "";
		private string _prov = "";
		private string _sin = null;
		private string _startdate = "";
		private string _status = "Not Active";
		private string _termdate = "2099-12-31";
		private string _truck = "";
		private string _type = "";
		private string _user = "";
		private int _hrstatus_id = 1;

		private bool _is_US_boardmember = false;
		private bool _is_CAN_boardmember = false;
		private NeBusinessUnit _branch;
		public NeMemberType membertype { get { return new NeMemberType(MemberTypeID); } }
		public NEContact contact_profile { get; set; }
		private int _customerID = 0;
		private int _vendorID = 0;
		private bool _isContact;
		private int _contact_id = 0;
		private int _contact_login_enabled = 0;


		private int _apprenticelevel = 0;
		private string _onwo = null;
		private string _lastPasswordDate = "";
		private int _member_default_location = 0;
		private int _member_default_page = 1;
		private int _member_isticket_admin = 0;
		private double _chargeout = 0;
		private int _has_comp;
		private string _comp_details;
		private string _offer_notes;
		private string _member_windows_password;
		private int _cellphone_id;
		private int _cellphone_number_id;

		//	private int _scheduled_by = 0;

		private bool _force_beta;
		private string _bonus_notes;
		private DateTime _ts;
		private bool _part_time;
		private int _vendor_id;
		private bool _include_in_mobile_contactlist = true;
		private bool _member_probation_email_sent = false;
	
		public int reports_to { get; set; }
		public int has_comp { get { return _has_comp; } set { _has_comp = value; } }
		public string comp_details { get { return _comp_details; } set { _comp_details = value; } }
		public string apprentice_contract { get; set; }
		public string offer_notes { get { return _offer_notes; } set { _offer_notes = value; } }
		public int cellphone_id { get { return _cellphone_id; } set { _cellphone_id = value; } }
		public int cellphone_number_id { get { return _cellphone_number_id; } set { _cellphone_number_id = value; } }
		public bool is_US_boardmember { get { return _is_US_boardmember; } set { _is_US_boardmember = value; } }
		public bool is_CAN_boardmember { get { return _is_CAN_boardmember; } set { _is_CAN_boardmember = value; } }
		public bool part_time { get { return _part_time; } set { _part_time = value; } }
		public int vendor_id { get { return _vendor_id; } set { _vendor_id = value; } }

		//added dec 12/14

		//	public int scheduled_by { get { return _scheduled_by; } set { _scheduled_by = value; } }
		public bool force_beta { get { return _force_beta; } set { _force_beta = value; } }
		public string bonus_notes { get { return _bonus_notes; } set { _bonus_notes = value; } }
		public bool gets_vehicle { get; set; }
		public bool gets_phone { get; set; }
		public bool gets_laptop { get; set; }
		public bool gets_barcodescanner { get; set; }
		public bool gets_businesscards { get; set; }
		public bool gets_directdeposit { get; set; }
		public bool gets_neemail { get; set; }
		public bool gets_phoneext { get; set; }
		public string benefits_id { get; set; }
		public string life_insurance_id { get; set; }
		public DateTime? benefits_startdate { get; set; }
		public bool include_in_mobile_contactlist { get { return _include_in_mobile_contactlist; } set { _include_in_mobile_contactlist = value; } }
		public bool member_probation_email_sent { get { return _member_probation_email_sent; } set { _member_probation_email_sent = value; } }
		public int active_ticket_id { get; set; }
		public double wage { get; set; }
		public bool Authenticated
		{
			get { return _authenticated; }
		}
		public int SessionID
		{
			get { return _session_id; }
			set { _session_id = value; }
		}
		public int id
		{
			get;
			set;
		}
		public int id32
		{
			get { return Convert.ToInt32(id); }
			set { id = value; }
		}
		public int ContactID
		{
			get { return _contact_id; }
			set { _contact_id = value; }
		}
		public int MemberTypeID
		{
			get { return _membertype_id; }
			set { _membertype_id = value; }
		}
		public string Type // Membertype Name
		{
			get { return _type; }
		}
		public int timetostat
		{
			get { return _timetostat; }
			set { _timetostat = value; }
		}
		public int vacation_interval_1
		{
			get { return _vacation_interval_1; }
			set { _vacation_interval_1 = value; }
		}
		public int member_default_location
		{
			get { return _member_default_location; }
			set { _member_default_location = value; }
		}
		public int member_default_page
		{
			get { return _member_default_page; }
			set { _member_default_page = value; }
		}
		public int vacation_interval_2
		{
			get { return _vacation_interval_2; }
			set { _vacation_interval_2 = value; }
		}
		public int vacation_interval_3
		{
			get { return _vacation_interval_3; }
			set { _vacation_interval_3 = value; }
		}
		public decimal vacation_amount_1
		{
			get { return _vacation_amount_1; }
			set { _vacation_amount_1 = value; }
		}
		public decimal vacation_amount_2
		{
			get { return _vacation_amount_2; }
			set { _vacation_amount_2 = value; }
		}
		public decimal vacation_amount_3
		{
			get { return _vacation_amount_3; }
			set { _vacation_amount_3 = value; }
		}
		public int years_with_company
		{
			get { return _years_with_company; }
		}
		public int months_with_company
		{
			get { return _months_with_company; }
		}
		public string business_unit_name { get; set; }
		public int business_unit_id
		{
			get { return _business_unit_id; }
			set { _business_unit_id = value; }
		}
		public string FirstName
		{
			get { return _firstname; }
			set { _firstname = value; }
		}
		public int hrstatus_id
		{
			get { return _hrstatus_id; }
			set { _hrstatus_id = value; }
		}
		public string LastName
		{
			get { return _lastname; }
			set { _lastname = value; }
		}
		public string FullName
		{
			get { return Nickname != "" ? Nickname + " " + _lastname : _firstname + " " + _lastname; }
			set { _fullname = value; }
		}
		public string FullName2
		{
			get { return FullName; }
			set { _fullname = value; }
		}
		public string member_windows_password
		{
			get { return _member_windows_password; }
			set { _member_windows_password = value; }
		}
		public string windows_password
		{
			get { return _member_windows_password; }
			set { _member_windows_password = value; }
		}
		public int receive_stat_pay
		{
			get { return _receive_stat_pay; }
			set { _receive_stat_pay = value; }
		}

		public string PayrollNo  // Product Code In BV
		{
			get { return _payrollid; }
			set
			{
				_payrollid = value;
			}
		}
		public string Username
		{
			get { return _user; }
			set
			{
				_user = value;
			}
		}
		public string Password
		{
			get { return _pass; }
			set
			{
				_pass = value;
			}
		}
		public string NEEmail
		{
			get { return _neemail; }
			set
			{
				_neemail = value;
			}
		}
		public string PhoneAreaCode
		{
			get { return _areacode; }
			set
			{
				_areacode = value;

			}
		}
		public string PhoneExtension
		{
			get { return _phoneextension; }
			set
			{
				_phoneextension = value;

			}
		}

		public string PhoneFirst
		{
			get { return _phonefirst; }
			set
			{
				_phonefirst = value;

			}
		}
		public string PhoneLast
		{
			get { return _phonelast; }
			set
			{
				_phonelast = value;

			}
		}

		public int payroll_handler
		{
			get { return Convert.ToInt32(_payroll_handler); }

			set { _payroll_handler = value.ToString(); }
		}

		public string Address
		{
			get { return _add; }
			set
			{
				_add = value;

			}
		}
		public string City
		{
			get { return _city; }
			set
			{
				_city = value;

			}
		}

		public int paytype_id
		{
			get { return _paytype_id; }
			set { _paytype_id = value; }
		}
		public string PostalCode
		{
			get { return _postal; }
			set
			{
				_postal = value;

			}
		}
		public string Prov
		{
			get { return _prov; }
			set
			{
				_prov = value;
			}
		}
		public string Country
		{
			get { return _country; }
			set
			{
				_country = value;
			}
		}
		public string Email
		{
			get { return _email; }
			set
			{
				_email = value;
			}
		}

		public string EmergencyFirstName1
		{
			get { return _emergfirstname1; }
			set
			{
				_emergfirstname1 = value;
			}
		}
		public string EmergencyLastName1
		{
			get { return _emerglastname1; }
			set
			{
				_emerglastname1 = value;
			}
		}
		public string EmergencyPhoneArea1
		{
			get { return _emergphonearea1; }
			set
			{
				_emergphonearea1 = value;
			}
		}
		public string EmergencyPhoneFirst1
		{
			get { return _emergphonefirst1; }
			set
			{
				_emergphonefirst1 = value;
			}

		}
		public string EmergencyPhoneLast1
		{
			get { return _emergphonelast1; }
			set
			{
				_emergphonelast1 = value;
			}
		}
		public string EmergencyFirstName2
		{
			get { return _emergfirstname2; }
			set
			{
				_emergfirstname2 = value;
			}

		}
		public string EmergencyLastName2
		{
			get { return _emerglastname2; }
			set
			{
				_emerglastname2 = value;
			}

		}
		public string EmergencyPhoneArea2
		{
			get { return _emergphonearea2; }
			set
			{ _emergphonearea2 = value; }

		}
		public string EmergencyPhoneFirst2
		{
			get { return _emergphonefirst2; }
			set
			{
				_emergphonefirst2 = value;

			}
		}
		public string EmergencyPhoneLast2
		{
			get { return _emergphonelast2; }
			set
			{
				_emergphonelast2 = value;
			}
		}
		public string NECellEmail
		{
			get { return _necellemail; }
			set
			{
				_necellemail = value;
			}
		}
		public string NECellPhoneNumber
		{
			get { return "(" + _necellarea + ") " + _necellfirst + "-" + _necelllast; }
		}
		public string PersonalPhoneNumber
		{
			get { return "(" + _areacode + ") " + _phonefirst + "-" + _phonelast; }
		}
		public string BirthDate
		{
			get { return _birthdate; }
			set { _birthdate = value; }
		}
		public string StartDate
		{
			get { return _startdate; }
			set { _startdate = value; }
		}
		public string TerminateDate
		{
			get { return _termdate; }
			set { _termdate = value; }
		}
		//	public string PayType
		//		{
		//		get { return _paytype; }
		//		set
		//		{ _paytype = value; }
		//		}
		//	public string PayType_Name
		//		{
		//		get { return _paytype_name; }
		//		}
		public string LastRaise
		{
			get { return _lastraise; }
			set { _lastraise = value; }
		}
		public string NextRaise
		{
			get { return _nextraise; }
			set { _nextraise = value; }
		}
		public double Holiday
		{
			get { return _holiday; }
			set { _holiday = value; }
		}
		public double HolidayTaken
		{
			get { return _holidaytaken; }
			set { _holidaytaken = value; }
		}
		public double Unpaid
		{
			get { return _unpaid; }
			set { _unpaid = value; }
		}
		public double Sick
		{
			get { return _sick; }
			set { _sick = value; }
		}
		public double Late
		{
			get { return _late; }
			set { _late = value; }
		}
		public string ElectricalLicence
		{
			get { return _eleclicence; }
			set
			{
				_eleclicence = value;
			}

		}
		public string DriversLicence
		{
			get { return _driverslicence; }
			set
			{
				_driverslicence = value;

			}
		}
		public string SIN
		{
			get { return _sin; }
			set
			{
				_sin = value;
			}

		}

		public string CreatedDate
		{
			get { return _dateadded; }
			set { _dateadded = value; }
		}
		public string Initials
		{
			get { return _initials; }
		}
		public string HasDependants
		{
			get { return _hasdependants; }
			set { _hasdependants = value; }
		}
		public string MiddleInitial { get; set; }
		public string Nickname { get; set; }
		public string Title { get; set; }
		public string LDAP_user { get; set; }
		public string color { get; set; }
		public ArrayList Pages
		{
			get { if (_pages.Count == 0) { build_pages_and_privs(); } return _pages; }
		}
		public string Status
		{
			get { return _status; }
			set { _status = value; }
		}
		public string NETruck
		{
			get { return _truck; }
			set { _truck = value; }
		}
		public NeBusinessUnit business_unit
		{
			get { return new NeBusinessUnit(business_unit_id); }
		}
		public string emplogon
		{
			get { return _emplogon; }
			set { _emplogon = value; }
		}

		public string empnotes
		{
			get { return _empnotes; }
			set { _empnotes = value; }
		}

		public int contact_login_enabled
		{
			get { return _contact_login_enabled; }
			set { _contact_login_enabled = value; }
		}
		public int member_isticket_admin
		{
			get
			{
				using (var uow = new UnitOfWork())
				{
					var x = (from t in new XPQuery<ne_xpo.cs.ticket_group>(uow)
							 where t.ticket_group_administrator.member_id == id
							 select t).FirstOrDefault(); return x == null ? 0 : 1;
				}
			}
		}
		public double chargeout
		{
			get
			{
				using (var uow = new UnitOfWork())
				{
					var chargeout_query = (from t in new XPQuery<ne_xpo.cs.membertype_chargeout>(uow)
										   where t.membertype_id.membertype_id == MemberTypeID && t.paytype_id == 1 && t.business_unit_id == business_unit_id
										   select t).FirstOrDefault();
					return chargeout_query == null ? 0 : chargeout_query.chargeout;
				}
			}
		}
		public string NECellPhoneArea
		{
			get
			{
				using (var uow = new UnitOfWork())
				{
					var cell = uow.GetObjectByKey<ne_xpo.cs.cellphone_number>(_cellphone_number_id);
					if (cell != null && cell.number.Length == 12 && cell.number.Contains("-"))
					{
						return cell.number.Substring(0, 3);
					}
					else
					{
						return "";
					}
				}
			}
		}
		public string NECellPhoneFirst
		{
			get
			{
				using (var uow = new UnitOfWork())
				{
					var cell = uow.GetObjectByKey<ne_xpo.cs.cellphone_number>(_cellphone_number_id);
					if (cell != null && cell.number.Length == 12 && cell.number.Contains("-"))
					{
						return cell.number.Substring(4, 3);
					}
					else
					{
						return "";
					}
				}
			}
		}
		public string NECellPhoneLast
		{
			get
			{
				using (var uow = new UnitOfWork())
				{
					var cell = uow.GetObjectByKey<ne_xpo.cs.cellphone_number>(_cellphone_number_id);
					if (cell != null && cell.number.Length == 12 && cell.number.Contains("-"))
					{
						return cell.number.Substring(8, 4);
					}
					else
					{
						return "";
					}
				}
			}
		}
		public string changereason
		{
			get { return _change_reason; }
			set { _change_reason = value; }
		}
		public int level
		{
			get { return _level; }
			set { _level = value; }
		}
		public string onwo
		{
			get { return _onwo; }
			set { _onwo = value; }
		}
		public bool tswatch
		{
			get { return _tswatch; }
			set { _tswatch = value; }
		}
		public int apprenticelevel
		{
			get { return _apprenticelevel; }
			set { _apprenticelevel = value; }
		}
		public bool isContact
		{
			get { return _isContact; }
			set { _isContact = value; }
		}
		public string lastPasswordDate
		{
			get { return _lastPasswordDate; }
			set { _lastPasswordDate = value; }
		}
		public double CurrentVacationAmount { get; set; }
		public int customerID
		{
			get { return _customerID; }
			set { _customerID = value; }
		}
		public int vendorID
		{
			get { return _vendorID; }
			set { _vendorID = value; }
		}
		public string pecell_area { get; set; }
		public string pecell_pref { get; set; }
		public string pecell_suff { get; set; }
		public bool disable_ldap_sync { get; set; }
		private List<KeyValuePair<string, int>> _permissions;

		/// <summary>
		/// check if the member is in backoffice, 
		/// now business_unit_id in (11,48,64) is backoffice
		/// </summary>
		public bool is_backoffice
		{
			get
			{
				return Toolbox.doSQL_bool(@"Select is_backoffice from business_unit where id= @v0", new object[] { business_unit_id });
			}
		}

	
		public bool ReceivesAutoVacationPayout	{	get;set;}
		#endregion
		public NeMember()
		{

		}


		public NeMember(string User, bool add_session_id)
		{
			var sql = "";
			DataTable dt;
			if (User.Contains("@"))
			{
				#region contact
				_isContact = true;
				sql = @"
SELECT 
    contact_nesimemberid,
	contact_id,
	contact_type,
	if(contact_login_enabled ='0','Not Active','Active') as Member_Status,
	curdate() as passworddatechange,
	0 member_default_page,
	0 business_unit_id
FROM 
	contact
WHERE 
	contact_email =@v0
LIMIT 1";
				dt = Toolbox.doSQL_dt(sql, new object[] { User });

				#endregion contact
			}
			else
			{
				_isContact = false;
				sql = @"
SELECT 
	member_id,
	member_membertype_id,
	member_inv_bvnumber,
	member_user,
	member_pass,
	member_firstname,
	member_lastname,
	member_status,
	membertype_name,
	passworddatechange,
	business_unit_id,
	member_default_page  
FROM 
	member,
	membertype 
WHERE 
	member_membertype_id = membertype_id AND 
	member_user = @v0
LIMIT 1";
				dt = Toolbox.doSQL_dt(sql, new object[] { User });

			}
			if (dt.Rows.Count > 0)
			{
				var dr = dt.Rows[0];
				Status = dr["member_status"].ToString();
				if (Status == "Active")
				{
					_authenticated = true;
					id = (int)dr[0];
					build_props(id);
					populate_permissions();
					if (add_session_id)
					{
						var session = new ne_session();
						var ip = "";
						if (HttpContext.Current != null)
							ip = HttpContext.Current.Request.UserHostAddress;
						session.ip = ip;
						session.member_id = id;
						session.is_active = true;
						session.is_contact = isContact;
						session.save();
						if (HttpContext.Current != null)
						{
							HttpContext.Current.Session.Add("session", session.id);
							HttpContext.Current.Session.Add("profile", this);
						}
						SessionID = session.id;
					}
				}
				else if (Status != "Active")
				{
					_last_error = "Account status is 'Not Active'";
				}
			}
			else
			{
				_last_error = "Username not found";
			}
		}
		public static void set_global_visibility_vars(System.Web.SessionState.HttpSessionState _s, int _id)
		{
			using (var conn = Toolbox.connect())
			{

				_s["global_visible_tax_entities"] = Toolbox.doSQL_string(conn, @"Call get_visible_tax_entities_group_concat(@v0)", new object[] { _id });
				_s["global_visible_business_units"] = Toolbox.doSQL_string(conn, @"Call get_visible_business_units_group_concat(@v0)", new object[] { _id });
				_s["global_visible_users"] = Toolbox.doSQL_string(conn, @"Call get_visible_users_group_concat(@v0)", new object[] { _id });
				var logo = Toolbox.doSQL_string(conn,
					@"Select logo_file from business_unit inner join member on member.business_unit_id = business_unit.id where member.member_id = @v0",
					new object[] { _id });
				_s["global_logo_icon"] = logo.Replace(".png", "") + @"-ico.png";
				_s["global_logo_full"] = logo;
				_s["global_visible_reporting_users"] = Toolbox.doSQL_string(conn, @"Call get_visible_reporting_users_group_concat(@v0)", new object[] { _id });
				_s["global_visible_reporting_users_all_status"] = Toolbox.doSQL_string(conn, @"Call get_visible_reporting_users_all_status_group_concat(@v0)", new object[] { _id });
			}
		}
		public static void set_global_visibility_vars_contact(System.Web.SessionState.HttpSessionState _s, int _id)
		{
			using (var conn = Toolbox.connect())
			{

				_s["global_visible_tax_entities"] = "";
				_s["global_visible_business_units"] = "";
				_s["global_visible_users"] = "";
				var businessUnit = Toolbox.doSQL_string(conn, @"CALL get_customer_default_business_unit_contact(@v0)", new object[] { _id });
				var logo = Toolbox.doSQL_string(conn, @"SELECT
    logo_file
FROM
    business_unit
WHERE id = @v0", new object[] { businessUnit });
				_s["global_logo_icon"] = logo.Replace(".png", "") + @"-ico.png";
				_s["global_logo_full"] = logo;
				_s["global_visible_reporting_users"] = "";
			}
		}
        /// <summary>
        /// For authentication purposes
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public NeMember([NotNull] string username, [NotNull] string password)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (password == null) throw new ArgumentNullException(nameof(password));
            
            string strSelUser;

            if (username.Contains("@"))
            {
                #region contact

                throw new NesiException("Feature not currently supported");
                _isContact = true;
                _lastPasswordDate = DateTime.Today.ToString(CultureInfo.CurrentCulture);
                strSelUser = @"
SELECT 
    contact_nesimemberid,
	contact_id,
	contact_type,
	if(contact_login_enabled ='0','Not Active','Active') as Member_Status,
	curdate() as passworddatechange,
	0 member_default_page,
	0 business_unit_id
FROM 
	contact
WHERE 
	contact_email = @v0 AND 
	contact_password = @v1
LIMIT 1";
                #endregion contact
            }
            else
            {
                #region member
                _isContact = false;
                strSelUser = @"
SELECT 
	member_id,
	member_membertype_id,
	member_inv_bvnumber,
	member_user,
	member_pass,
	member_firstname,
	member_lastname,
	member_status,
	membertype_name,
	passworddatechange,
	business_unit_id,
	member_default_page
FROM 
	member,
	membertype 
WHERE 
	member_membertype_id = membertype_id AND 
	member_user = @v0 AND 
	member_status != 'Deleted'
LIMIT 1";
                #endregion member
            }

            var dt = Toolbox.doSQL_dt(strSelUser, new object[] {
                    username
				});

            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];

                if (PasswordHasher.VerifyHashedPassword(dr["member_pass"]?.ToString(), password) ==
                    PasswordVerificationResult.Success)
                {
                    Status = dr["member_status"].ToString();
                    if (Status == "Active")
                    {
                        _authenticated = true;
                        id = (int)dr[0];
                        _contact_id = Convert.ToInt32(dr[1]);
                        _branch = new NeBusinessUnit(Convert.ToInt32(dr["business_unit_id"]));
                        contact_profile = new NEContact(Convert.ToInt32(dr[1]));
                        _member_default_page = Convert.ToInt32(dr["member_default_page"]);
                        _lastPasswordDate = dr["passworddatechange"] != DBNull.Value ? dr["passworddatechange"].ToString() : null;
                        build_props(id);
                        populate_permissions();
                    }
                    else if (Status != "Active")
                    {
                        _last_error = "Account status is 'Not Active'";
                    }
                }
                else
                {
                    _last_error = "Username and/or Password not found";
                }
            }
            else
            {
                _last_error = "Username and/or Password not found";
            }
        }


        public NeMember(string ses_id)
		{
			var session = new ne_session(Convert.ToInt32(ses_id));
			if (session.is_active)
			{
				build_props(session.member_id);
				SessionID = session.id;
			}
			else
			{
				try
				{
					if (HttpContext.Current != null)
						HttpContext.Current.Response.Redirect("/default.aspx?sign_out=true");
				}
				catch (Exception ee)
				{
					// This was set as a blind try/catch due to a socket exception when Response object is not available.
					// Unsure if the Response object is null, or if there is another property to use.
					// This was the exception: Response is not available in this context.
				}
			}
		}
		public NeMember(int mem_id)
		{
			build_props(mem_id);
		}
		/// <summary>
		/// Gets the reports to list for the selected supervisor
		/// </summary>
		/// <param name="supervisorId">Supervisor Id</param>
		/// <returns></returns>
        public List<int> Report_To_List(int supervisorId)
        {    
            List<int> Report_To_List = new List<int>();
            var reports_to_str = Toolbox.doSQL_string(@"SELECT REPORTS_TO(@v0)", new object[] { supervisorId });
            if (!string.IsNullOrEmpty(reports_to_str))
            {
                Report_To_List=reports_to_str.Split(',').Select(int.Parse).ToList();
            }
            return Report_To_List;  
        }
        /// <summary>
        /// Offloading the loading of the base properties of the NeMember object to one consolidated method.
        /// </summary>
        /// <param name="m_id">Member ID</param>
        private void build_props(object m_id)
		{
			//		Toolbox.do_debug(string.Format(@"Start (NEMember.build_props) - {0}", m_id));
			var mem_id = Convert.ToInt32(m_id);
			var strSelUser = "";
			if (mem_id > 100000)
			{
				var memtype = Toolbox.doSQL_string(@"SELECT contact_type FROM contact  WHERE contact_nesimemberid =@v0", new object[] { mem_id });
				if (memtype == "Customer")
				{
					#region Customer
					strSelUser = @"
SELECT 
	*
FROM 
	contact a,
	customer c
WHERE 
	a.contact_cust_id = c.customer_id AND 
	a.contact_nesimemberid = @v0
LIMIT 1";
					#endregion Customer
				}
				else if (memtype == "Vendor")
				{
					#region Vendor
					strSelUser = @"
SELECT 
	*
FROM 
	contact a,
	vendor c
WHERE 
	a.contact_cust_id = c.vendor_ID AND 
	a.contact_nesimemberid = @v0
LIMIT 1";
					#endregion Vendor
				}
				var dt = Toolbox.doSQL_dt(strSelUser, new object[] { mem_id });
				if (dt.Rows.Count > 0)
				{
					var dr = dt.Rows[0];
					InitExtMember(dr);
				}
			}
			else
			{
				if (exists(mem_id))
				{
					load(mem_id, false);
				}
			}
		}
		public static bool is_encrypted_password(string _pass)
		{
			if (string.IsNullOrEmpty(_pass))
			{
				return false;
			}
			else
			{
				var reBase64Chk = new Regex("^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$");
				var matchesRegEx = reBase64Chk.Match(_pass).Success;
				var decryptChk = matchesRegEx
										? Toolbox.DecryptString(_pass, Toolbox.GetRequiredAppSetting("encryption_pass"))
										: "";
				return matchesRegEx && !string.IsNullOrEmpty(decryptChk);
			}
		}
		public void build_pages_and_privs()
		{
			var sqlPriv = "";
			object[] objectParams;
			if (_isContact)
			{
				#region true

				sqlPriv = @"
SELECT 
	page_id,
	page_parent_id,
	page_name,
	page_desc,
	page_scriptpath,
	page_order,
	page_mobile_action mobile_action,
	page_mobile_ready mobile_ready,
	page_desktop_ready,
	page_mobile_icon mobile_icon,
	IFNULL(privilege_id, 0) privilege_id,
	IFNULL(privilege_name, '') privilege_name,
	IFNULL(privilege_desc, '') privilege_desc,
	page_class
FROM 
	page,
	contactpage 
LEFT JOIN 
	contactpageprivilege 
		on contactpage_id = contactpageprivilege_contactpage_id 
LEFT JOIN 
	privilege 
		on contactpageprivilege_privilege_id = privilege_id and privilege_enabled = 1 
WHERE 
	contactpage_contact_id = @v0 AND 
	contactpage_page_id = page_id AND 
	page_enabled = 1
ORDER BY 
	page_id,
	page_order,
	privilege_id,
	privilege_order";
				objectParams = new object[] { _contact_id };
				#endregion true
			}
			else
			{
				#region false

				sqlPriv = @"
(
SELECT 
	a.page_id,
	a.page_parent_id,
	a.page_name,
	a.page_desc,
	a.page_scriptpath,
	a.page_order,
	a.page_mobile_action mobile_action,
	a.page_mobile_ready mobile_ready,
	a.page_mobile_icon mobile_icon,
	a.page_desktop_ready,
	IFNULL(d.privilege_id, 0) privilege_id,
	IFNULL(d.privilege_name, '') privilege_name,
	IFNULL(d.privilege_desc, '') privilege_desc,
	a.page_class
FROM 
	page a,
	memberpage b 
LEFT JOIN 
	memberpageprivilege c
		on b.memberpage_id = c.memberpageprivilege_memberpage_id AND c.active = 1
LEFT JOIN 
	privilege d
		on c.memberpageprivilege_privilege_id = d.privilege_id and d.privilege_enabled = 1 
WHERE 
	b.memberpage_member_id = @v0 AND 
	b.memberpage_page_id = page_id AND 
	a.page_enabled = 1 AND
	b.active = 1
)
UNION
(
SELECT
	0 page_id,
	0 page_parent_id,
	'' page_name,
	'' page_desc,
	'' page_scriptpath,
	0 page_order,
	'' mobile_action,
	0 mobile_ready,
	'' mobile_icon,
	0 page_desktop_ready,
	IFNULL(a.global_priv_id, 0) privilege_id,
	IFNULL(b.name, '') privilege_name,
	IFNULL(b.description, '') privilege_desc,
	'' page_class
FROM
	privilege_global_link a
LEFT JOIN
	privilege_global b ON a.global_priv_id = b.id
WHERE 
	type_id = 1 AND
	user_id = @v0
)
ORDER BY 
	page_id,
	page_order,
	privilege_id

";
				objectParams = new object[] { id };

				#endregion false
			}
			var _privs = Toolbox.doSQL_dt(sqlPriv, objectParams);
			var lastid = 0;
			var page = new NePage();
			foreach (DataRow _priv in _privs.Rows)
			{
				var this_page_id = Convert.ToInt32(_priv["page_id"]);
				var this_parent_id = Convert.ToInt32(_priv["page_parent_id"]);
				var this_page_name = (string)_priv["page_name"];
                var this_page_desc = Toolbox.ReturnBlankIfNull_string(_priv["page_desc"]);
                var this_mobile_action = (string)_priv["mobile_action"];
				var this_mobile_icon = (string)_priv["mobile_icon"];
				var this_desktop_ready = Convert.ToInt16(_priv["page_desktop_ready"]) == 1;
				var this_mobile_ready = Convert.ToInt16(_priv["mobile_ready"]) == 1;
                var this_page_path = Toolbox.ReturnBlankIfNull_string(_priv["page_scriptpath"]);
                var this_page_order = Convert.ToInt32(_priv["page_order"]);
				var this_page_class = _priv["page_class"] != DBNull.Value ? _priv["page_class"].ToString() : "";
				var this_priv_id = Convert.ToInt32(_priv["privilege_id"]);
				var this_priv_name = (string)_priv["privilege_name"];
				var this_priv_desc = (string)_priv["privilege_desc"];
				if (lastid == 0)
				{
					// make a new page
					page = new NePage();
					page.ID = this_page_id;
					page.ParentID = this_parent_id;
					page.mobile_ready = this_mobile_ready;
					page.mobile_icon = this_mobile_icon;
					page.mobile_action = this_mobile_action;
					page.desktop_ready = this_desktop_ready;
					page.Name = this_page_name;
					page.Description = this_page_desc;
					page.Path = this_page_path;
					page.Order = this_page_order;
					page.Class = this_page_class;
				}
				else if (lastid != this_page_id)
				{
					// add the page with all the privileges already added to it
					_pages.Add(page);
					// make a new page
					page = new NePage();
					page.ID = this_page_id;
					page.ParentID = this_parent_id;
					page.Name = this_page_name;
					page.Description = this_page_desc;
					page.mobile_ready = this_mobile_ready;
					page.mobile_icon = this_mobile_icon;
					page.mobile_action = this_mobile_action;
					page.desktop_ready = this_desktop_ready;
					page.Path = this_page_path;
					page.Order = this_page_order;
					page.Class = this_page_class;
				}
				// add the priv to the page if not null
				if (this_priv_id != 0)
				{
					var priv = new NePrivilege();
					priv.id = Convert.ToInt32(this_priv_id);
					priv.page_id = Convert.ToInt32(this_page_id);
					priv.name = this_priv_name;
					priv.description = this_priv_desc;
					page.AddPrivilege(priv);
				}
				lastid = this_page_id;
			}
			if (lastid != 0)
			{
				// add the temp page tha didn't get to loop around
				_pages.Add(page);
			}
		}
		public static bool at_jobsite_today(int _member_id, int _woprog_id, int _address_id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM appointments WHERE DATE(startdate) = CURDATE() AND member_id = @v0  AND woprog_id = @v1  AND location = @v2 ", new object[] { _member_id, _woprog_id, _address_id }) > 0;
		}
		private bool exists(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				var x = uow.GetObjectByKey<ne_xpo.cs.member>(_id);
				return x != null;
			}
		}
		/// <summary>
		/// Loads the user - Exists should ALWAYS be ran before it gets to here for NeMembers
		/// </summary>
		/// <param name="_id"></param>
		/// <param name="_is_contact"></param>
		private void load(int _id, bool _is_contact)
		{
			_isContact = _is_contact;
			_authenticated = true;
			using (var uow = new UnitOfWork())
			{
				if (_is_contact)
				{

				}
				else
				{
					var m = uow.GetObjectByKey<ne_xpo.cs.member>(_id);
					id = m.member_id;
					_add = m.member_add;
					_areacode = m.member_areacode;
					_bvnumber = m.member_inv_bvnumber;
					_city = m.member_city;
					business_unit_id = m.business_unit_id.id;
					business_unit_name = m.business_unit_id.name;
					_tswatch = m.member_ts_watch;
					_country = m.member_country;
					_email = m.member_email;
					_emergfirstname1 = m.member_emergfirstname1;
					_emergfirstname2 = m.member_emergfirstname2;
					_emerglastname1 = m.member_emerglastname1;
					_emerglastname2 = m.member_emerglastname2;
					_emergphonearea1 = m.member_emergphonearea1;
					_emergphonearea2 = m.member_emergphonearea2;
					_emergphonefirst1 = m.member_emergphonefirst1;
					_emergphonefirst2 = m.member_emergphonefirst2;
					_emergphonelast1 = m.member_emergphonelast1;
					_emergphonelast2 = m.member_emergphonelast2;
					_firstname = m.member_firstname;
					_lastname = m.member_lastname;
					color = m.color;
					_membertype_id = m.member_membertype_id.membertype_id;
					_months_with_company = Toolbox.MonthDifference(m.member_startdate, DateTime.Now);
					_years_with_company = (int)Math.Round(DateTime.Now.Subtract(m.benefits_startdate).TotalDays / 365.25, 0);
					_necellemail = m.member_necellemail;
					_cellphone_number_id = m.cellphone_number_id == null ? 0 : m.cellphone_number_id.id;
					_cellphone_id = m.cellphone_id;
					_neemail = m.member_neemail;
					_pass = m.member_pass;
					payroll_handler = m.payroll_handler; // Revisit
					_payrollid = m.member_payroll_id;
					_paytype_id = m.paytype_id;
					_phoneextension = m.member_phoneextension;
					_phonefirst = m.member_phonefirst;
					_phonelast = m.member_phonelast;
					_postal = m.member_postal;
					_prov = m.member_prov;
					_status = m.member_status;
					_truck = m.member_truck;
					_EmployeeName = m.member_fullname;
					_type = m.member_membertype_id.membertype_name;
					_user = m.member_user;
					_birthdate = Toolbox.MySQL_shortdt(m.member_birthdate);
					_dateadded = Toolbox.MySQL_shortdt(m.member_dateadded);
					_driverslicence = m.member_driverslicence;
					_eleclicence = m.member_eleclicence;
					_emplogon = m.emplogon;
					_employeeID = m.employeeid;
					_empnotes = m.member_employeenotes;
					_holiday = m.member_holiday;
					_holidaytaken = m.member_holidaytaken;
					_lastDate = Toolbox.MySQL_shortdt(m.member_lastdatetime);
					_lastraise = Toolbox.MySQL_shortdt(m.member_lastraise);
					_late = m.member_lateoccur;
					_nextraise = Toolbox.MySQL_shortdt(m.member_nextraise);
					sent_welcome_email = m.sent_welcome_email;
					_sick = m.member_sickdays;
					_sin = m.member_sin;
					_startdate = Toolbox.MySQL_shortdt(m.member_startdate);
					_receive_stat_pay = m.receive_stat_pay;
					_termdate = Toolbox.MySQL_shortdt(m.member_termdate);
					_timetostat = m.member_time_to_stat;
					_unpaid = m.member_unpaid;
					_apprenticelevel = Convert.ToInt32(m.member_apprentice_level);
					_fullname = m.member_fullname;
					_initials = _firstname.Substring(0, 1) + _lastname.Substring(0, 1);
					_lastPasswordDate = Toolbox.MySQL_shortdt(m.passworddatechange);
					apprentice_contract = m.apprentice_contract;
					Nickname = m.member_nickname;
					MiddleInitial = Toolbox.ReturnBlankIfNull_string(m.member_middleinitial);
					Title = m.member_membertype_id.membertype_name;
					HasDependants = m.member_hasdependants;
					hrstatus_id = m.member_hrstatus_id;
					_member_default_location = m.member_default_location;
					_member_default_page = Convert.ToInt32(m.member_default_page);
					LDAP_user = m.member_ldap_user;

					reports_to = m.reports_to;
					_has_comp = m.has_comp;
					_comp_details = m.comp_details;
					_offer_notes = m.offer_notes;
					_member_windows_password = m.member_windows_password;
					_is_US_boardmember = m.is_us_boardmember == 1;
					_is_CAN_boardmember = m.is_can_boardmember == 1;
					_vacation_interval_1 = m.vacation_interval_1;
					_vacation_interval_2 = m.vacation_interval_2;
					_vacation_interval_3 = m.vacation_interval_3;
					_vacation_amount_1 = (decimal)m.vacation_amount_1;
					_vacation_amount_2 = (decimal)m.vacation_amount_2;
					_vacation_amount_3 = (decimal)m.vacation_amount_3;
					active_ticket_id = m.active_ticket_id;

					var start_date = Convert.ToDateTime(_startdate);
					var month_diff = Math.Abs(12 * (start_date.Year - DateTime.Now.Year) + start_date.Month - DateTime.Now.Month);

					if (month_diff >= _vacation_interval_3)
					{
						CurrentVacationAmount = Convert.ToDouble(_vacation_amount_3);
					}
					else if (month_diff >= _vacation_interval_2)
					{
						CurrentVacationAmount = Convert.ToDouble(_vacation_amount_2);
					}
					else if (month_diff >= _vacation_interval_1)
					{
						CurrentVacationAmount = Convert.ToDouble(_vacation_amount_1);
					}
					else
					{
						CurrentVacationAmount = 0;
					}

					pecell_area = m.member_pecell_area;
					pecell_pref = m.member_pecell_pref;
					pecell_suff = m.member_pecell_suff;

					var cell = new NECellphone_Number(_cellphone_number_id).number;
					if (cell != null)
					{
						if (cell.Length >= 10)
						{
							_necellarea = cell.Substring(0, 3);
							_necellfirst = cell.Substring(4, 3);
							_necelllast = cell.Substring(8, 4);
						}
					}
					disable_ldap_sync = m.disable_ldap_sync;
					business_unit_id = m.business_unit_id.id;
					force_beta = m.force_beta;
					bonus_notes = m.bonus_notes;
					gets_vehicle = m.gets_vehicle == 1;
					gets_phone = m.gets_phone == 1;
					gets_laptop = m.gets_laptop == 1;
					gets_barcodescanner = m.gets_barcodescanner;
					gets_businesscards = m.gets_businesscards;
					gets_directdeposit = m.gets_directdeposit;
					gets_neemail = m.gets_neemail;
					gets_phoneext = m.gets_phoneext;
					benefits_id = m.benefits_id;
					life_insurance_id = m.life_insurance_id;
					benefits_startdate = m.benefits_startdate;
					part_time = m.part_time;
					vendor_id = m.vendor_id;
					_include_in_mobile_contactlist = m.include_in_mobile_contactlist == 1;
					_member_probation_email_sent = m.member_probation_email_sent == 1;
					wage = Toolbox.doSQL_double("SELECT GET_WAGE(@v0)", new object[] { _id });
					ReceivesAutoVacationPayout = m.auto_vacation_payout == 1;
				}
			}
		}

		public bool sent_welcome_email { get; set; }

		private void InitExtMember(DataRow dr)
		{
			// Results
			_authenticated = true;
			_isContact = true;
			_contact_id = Convert.ToInt32(dr["Contact_ID"]);
			if (dr["Contact_Type"].ToString() == "Customer")
			{
				_bvnumber = dr["Customer_Number"].ToString();
				_company = dr["Customer_name"].ToString();
				_customerID = Convert.ToInt32(dr["Customer_ID"]);
				//			NECustomer cust = new NECustomer(Convert.ToInt32(_customerID));
				_business_unit_id = Toolbox.doSQL_int(@"Select ifnull((Select business_unit_id from woprog  where woprog_contact_id =@v0 order by woprog_id desc limit 1),(Select business_unit_id from customer where customer_id = @v1))", new object[] { _contact_id, _customerID });
			}
			else
			{
				_bvnumber = dr["Vendor_Number"].ToString();
				_company = dr["Vendor_name"].ToString();
				_vendorID = Convert.ToInt32(dr["Vendor_ID"]);
				var vendor = new NEVendor(_vendorID);
				_business_unit_id = vendor.business_unit_id;
			}
			_pass = dr["Contact_Password"].ToString();
			_member_default_page = dr["contact_page_id"] == DBNull.Value ? 1 : Convert.ToInt32(dr["contact_page_id"]);
			_email = dr["Contact_Email"].ToString();
			id = Convert.ToInt32(dr["Contact_nesimemberid"]);
			_status = dr["Contact_Status"].ToString();
			_user = dr["Contact_Email"].ToString();

			_emplogon = dr["Contact_Login"].ToString();

			contact_profile = new NEContact(Convert.ToInt32(_contact_id));
			_initials = "XX";
			_fullname = dr["Contact_Name"].ToString();
			Nickname = dr["Contact_Name"].ToString();
			_contact_login_enabled = Convert.ToInt16(dr["contact_login_enabled"]);
		}
		public static int get_current_offer(int _memid, DateTime _dt)
		{

			if (_memid != 0 && _dt != null)
			{
				return Toolbox.doSQL_int(@"Select ifnull((Select id from member_offers  where startdate<=@v0 and enddate >=@v1  and memberid =@v2  and (status = 'Accepted' or status = 'Previous') order by id desc limit 1),0)", new object[] { _dt.ToString("yyyy-MM_dd"), _dt.ToString("yyyy-MM_dd"), _memid });
			}
			return 0;
		}
		public static string get_name(object _id)
		{
			var name = "";
			var this_id = 0;
			int.TryParse(_id.ToString(), out this_id);
			if (this_id > 0)
			{
				name = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(member_fullname), '') FROM member WHERE member_id = @v0  LIMIT 1", new object[] { this_id });
			}
			return name;
		}
		// TODO (LoadActiveMembersWithCompanies) Used on fvr manage page
		public DataTable LoadActiveMembersWithCompanies(string branch_ids)
		{
			return Toolbox.doSQL_dt(string.Format(@" SELECT member_id id, CONCAT(b.name, ' - ',member_name(member_id)) name 
FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE a.member_status = 'Active'
AND b.id IN ({0}) ORDER BY b.name, a.member_fullname", branch_ids), null);
		}
		// TODO (LoadComMemberList) used 2 places messaging, sales_commission
		public ArrayList LoadComMemberList(int _business_unit_id)
		{
			var _dt = Toolbox.doSQL_dt(@" SELECT member_id id, member_name(member_id) user, member_firstname firstname, member_nickname nickname, member_lastname lastname, member_name(member_id) fullname FROM Member WHERE business_unit_id = @v0  AND member_status = 'Active' ORDER BY member_fullname", new object[] { _business_unit_id });
			var list = new ArrayList();
			NeMember item;
			foreach (DataRow _dr in _dt.Rows)
			{
				item = new NeMember();
				item.id = (int)_dr["id"];
				item._user = _dr["user"].ToString();
				item._firstname = _dr["firstname"].ToString();
				item._lastname = _dr["lastname"].ToString();
				item._fullname = _dr["fullname"].ToString();
				item.Nickname = _dr["nickname"].ToString();
				item._onwo = null;
				list.Add(item);
			}
			return list;
		}
		public static string CleanUserPass(string _userpass)
		{
			return Regex.Replace(_userpass, @"[^0-9a-zA-Z]+", "");
		}
		public void save()
		{
			var is_local = HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.IsLocal : true;
			var enable_ldap = Toolbox.app_setting("enable_ldap") == "1";
			var enable_ldap_sync = Toolbox.app_setting("enable_ldap_sync") == "1";
			if (string.IsNullOrWhiteSpace(_email))
			{
				_email = "nomail@" + Toolbox.app_setting("DomainForEmail");
			}
			if (Nickname == "")
			{
				Nickname = FirstName;
			}
			if (FullName == "")
			{
				FullName = Nickname + " " + LastName;
			}
			// Check for looping reports to and prevent the save.
			if (id > 0)
			{
				if (Check_for_circular_org_chart(id, reports_to))
				{
					throw new Exception("Looping reports to detected - this should never happen");
				}				
			}

			var previous_values = new NeMember();
            int memIDgap = 0;

			using (var my_conn = Toolbox.connect())
			{
				var my_comm = new MySqlCommand();
				my_comm.Connection = my_conn;
              
				if (id == 0)
				{
                    // There has been a gap of about 7000 member ids somehow. Below is a surgery to fill this gap while inserting a new member. This might be needed to remove at a later stage
                    #region Member Id Gap
                     memIDgap = Toolbox.doSQL_int("SELECT IFNULL(MAX(member_id),0) INTO @_id FROM member WHERE member_id <9999;SELECT IF(@_id != 0, @_id + 1, 0);");
                   // Making sure the gap didn't get filled out
                    bool checkExistance = Toolbox.doSQL_bool("SELECT count(*) FROM member WHERE member_id = @v0 LIMIT 1", new object[] { memIDgap });                     
                    #endregion
                    #region INSERT command
                    string sql = @"
INSERT INTO member
	(
	member_add,
	apprentice_contract,
	member_apprentice_level,
	member_areacode,
	member_birthdate,
	cellphone_id,
	cellphone_number_id,
	member_city,
	business_unit_id,
	member_country,
	member_dateadded,
	member_default_location,
	
	member_driverslicence,
	member_eleclicence,
	member_email,
	member_emergfirstname1,
	member_emergfirstname2,
	member_emerglastname1,
	member_emerglastname2,
	member_emergphonearea1,
	member_emergphonearea2,
	member_emergphonefirst1,
	member_emergphonefirst2,
	member_emergphonelast1,
	member_emergphonelast2,
	emplogon,
	employeeid,
	member_employeenotes,
	member_firstname,
	member_fullname,
	member_hasdependants,
	member_hrstatus_id,
	is_can_boardmember,
	is_us_boardmember,
	member_lastname,
	member_windows_password,
	member_membertype_id,
	member_middleinitial,
	member_necellemail,
	member_neemail,
	member_nickname,
	member_pass,
	payroll_handler,
	member_payroll_id,
	paytype_id,
	member_pecell_area,
	member_pecell_pref,
	member_pecell_suff,
	member_phoneextension,
	member_phonefirst,
	member_phonelast,
	member_postal,
	member_prov,
	receive_stat_pay,
	reports_to,
	member_sin,
	member_startdate,
	member_status,
	member_termdate,
	member_time_to_stat,
	member_truck,
	member_user,
	vacation_amount_1,
	vacation_amount_2,
	vacation_amount_3,
	vacation_interval_1,
	vacation_interval_2,
	vacation_interval_3,
	scheduled_by,
	member_TS_watch,
	force_beta,
	bonus_notes,
	gets_vehicle,
	gets_phone,
	gets_laptop,
	gets_barcodescanner,
	gets_businesscards,
	gets_directdeposit,
	gets_neemail,
	gets_phoneext,
	benefits_id,
	life_insurance_id,
	benefits_startdate,
	part_time,
	vendor_id,
	color,
	include_in_mobile_contactlist,
	member_probation_email_sent,
	sent_welcome_email,
    auto_vacation_payout
	";
                    string insertVal = @")
VALUES
	(
	@add, 
	@apprentice_contract,
	@apprentice_level,
	@areacode, 
	@birthdate, 
	@cellphone_id,
	@cellphone_number_id,
	@city, 
	@business_unit_id, 
	@country,
	NOW(), 
	@default_location,
	
	@driverslicence, 
	@eleclicence, 
	@email, 
	@emergfirstname1, 
	@emergfirstname2, 
	@emerglastname1, 
	@emerglastname2, 
	@emergphonearea1, 
	@emergphonearea2, 
	@emergphonefirst1,
	@emergphonefirst2, 
	@emergphonelast1, 
	@emergphonelast2, 
	@emplogon, 
	@employeeid, 
	@employeenotes, 
	@firstname, 
	@fullname,
	@hasdependants,
	@hrstatus_id,
	@is_can_boardmember,
	@is_us_boardmember,
	@lastname,
	@windows_password,
	@membertype_id, 
	@middleinitial,
	@necellemail, 
	@neemail, 
	@nickname,
	@pass, 
	@payroll_handler,
	@payroll_id, 
	@paytype_id,
	@pecell_area,
	@pecell_pref,
	@pecell_suff,
	@phoneextension,
	@phonefirst, 
	@phonelast, 
	@postal, 
	@prov, 
	@receive_stat_pay,
	@reports_to,
	@sin,
	@startdate, 
	@status, 
	@termdate, 
	@time_to_stat,
	@truck, 
	@user, 
	@vacation_amount_1,
	@vacation_amount_2,
	@vacation_amount_3,
	@vacation_interval_1,
	@vacation_interval_2,
	@vacation_interval_3,
	@scheduled_by,
	@member_TS_watch,
	@force_beta,
	@bonus_notes,
	@gets_vehicle,
	@gets_phone,
	@gets_laptop,
	@gets_barcodescanner,
	@gets_businesscards,
	@gets_directdeposit,
	@gets_neemail,
	@gets_phoneext,
	@benefits_id,
	@life_insurance_id,
	@benefits_startdate,
	@part_time,
	@vendor_id,
	@color,
    @include_in_mobile_contactlist,
    @member_probation_email_sent,
	@sent_welcome_email,
     @auto_vacation_payout
	";
                    if (!checkExistance)
                    {
                        sql += @",member_id";
                        insertVal += @",@member_id_gap)";
                        my_comm.Parameters.AddWithValue("@member_id_gap", memIDgap);
                    }
                    else
                    {
                        insertVal += @")";
                    }
                    my_comm.CommandText = sql + insertVal;
                    #endregion INSERT command
                }
				else
				{
					#region UPDATE command
					previous_values = new NeMember(id);
					my_comm.CommandText = @"
UPDATE 
	member 
SET 
	member_add				    = @add,
	apprentice_contract         = @apprentice_contract,
	member_apprentice_level     = @apprentice_level,
	member_areacode			    = @areacode,
	member_BirthDate		    = @birthdate,
	cellphone_id                = @cellphone_id,
	cellphone_number_id         = @cellphone_number_id,
	member_city				    = @city,
	member_country			    = @country,
	member_default_location     = @default_location,
	
	member_driverslicence	    = @driverslicence,
	member_eleclicence		    = @eleclicence,
	member_email			    = @email,
	member_emergfirstname1	    = @emergfirstname1,
	member_emergfirstname2	    = @emergfirstname2,
	member_emerglastname1	    = @emerglastname1,
	member_emerglastname2	    = @emerglastname2,
	member_emergphonearea1	    = @emergphonearea1,
	member_emergphonearea2	    = @emergphonearea2,
	member_emergphonefirst1	    = @emergphonefirst1,
	member_emergphonefirst2	    = @emergphonefirst2,
	member_emergphonelast1	    = @emergphonelast1,
	member_emergphonelast2	    = @emergphonelast2,
	emplogon				    = @emplogon,
	employeeid				    = @employeeid,
	member_employeenotes	    = @employeenotes,
	member_firstname		    = @firstname,
	member_fullname			    = @fullname,
	member_hasdependants	    = @hasdependants,
	member_hrstatus_id		    = @hrstatus_id,
	is_can_boardmember          = @is_can_boardmember,
	is_us_boardmember           = @is_us_boardmember,
	member_lastname			    = @lastname, 
	member_ldap_user		    = @ldap_user,
	member_level			    = @level,
	member_membertype_id	    = @membertype_id,
	member_middleinitial	    = @middleinitial,
	member_necellemail		    = @necellemail,
	member_neemail			    = @neemail,
	member_nickname			    = @nickname,
	member_pass				    = @pass,
	payroll_handler			    = @payroll_handler,
	member_payroll_id		    = @payroll_id,
	paytype_id				    = @paytype_id,
	member_pecell_area		    = @pecell_area,
	member_pecell_pref		    = @pecell_pref,
	member_pecell_suff		    = @pecell_suff,
	member_phoneextension       = @phoneextension,
	member_phonefirst		    = @phonefirst,
	member_phonelast		    = @phonelast,
	member_postal			    = @postal,
	member_prov				    = @prov,
	receive_stat_pay		    = @receive_stat_pay,
	reports_to                  = @reports_to,
	member_sin				    = @sin,
	member_startdate		    = @startdate,
	member_status			    = @status,
	member_termdate			    = @termdate,
	member_time_to_stat		    = @time_to_stat,
	member_truck			    = @truck,
	member_user				    = @user,
	vacation_amount_1		    = @vacation_amount_1,
	vacation_amount_2		    = @vacation_amount_2,
	vacation_amount_3		    = @vacation_amount_3,
	vacation_interval_1		    = @vacation_interval_1,
	vacation_interval_2		    = @vacation_interval_2,
	vacation_interval_3		    = @vacation_interval_3,
	member_windows_password		= @windows_password,
	disable_ldap_sync			= @disable_ldap_sync,
	scheduled_by                = @scheduled_by,
	member_TS_watch             = @member_TS_watch,
	force_beta                  = @force_beta,
	bonus_notes                 = @bonus_notes,
	gets_vehicle                = @gets_vehicle,
	gets_phone                  = @gets_phone,
	gets_laptop                 = @gets_laptop,
	gets_barcodescanner         = @gets_barcodescanner,
	gets_businesscards          = @gets_businesscards,
	gets_directdeposit          = @gets_directdeposit,
	gets_neemail                = @gets_neemail,
	gets_phoneext               = @gets_phoneext,
	benefits_id                 = @benefits_id,
	life_insurance_id           = @life_insurance_id,
	benefits_startdate          = @benefits_startdate,
	part_time                   = @part_time,
	vendor_id                   = @vendor_id,
	color                       = @color,
    include_in_mobile_contactlist = @include_in_mobile_contactlist,
    member_probation_email_sent = @member_probation_email_sent,
	member_holiday				= @holiday,
	business_unit_id			= @business_unit_id,
	sent_welcome_email			= @sent_welcome_email,
	auto_vacation_payout		= @auto_vacation_payout
WHERE 
	member_id = @member_id
LIMIT 1";
					#endregion UPDATE command
				}
				#region PARAMETERS
				my_comm.Parameters.AddWithValue("@add", _add); // 
				my_comm.Parameters.AddWithValue("@apprentice_contract", apprentice_contract); // 
				my_comm.Parameters.AddWithValue("@apprentice_level", _apprenticelevel); // 
				my_comm.Parameters.AddWithValue("@areacode", _areacode); // 
				my_comm.Parameters.AddWithValue("@birthdate", _birthdate == "" ? null : Convert.ToDateTime(_birthdate).ToString("yyyy-MM-dd")); // 
				my_comm.Parameters.AddWithValue("@cellphone_id", cellphone_id); // 
				my_comm.Parameters.AddWithValue("@cellphone_number_id", cellphone_number_id == 0 ? (object)DBNull.Value : cellphone_number_id); // 
				my_comm.Parameters.AddWithValue("@city", _city); // 
				my_comm.Parameters.AddWithValue("@country", _country); // 
				my_comm.Parameters.AddWithValue("@default_location", _member_default_location == 0 ? (object)DBNull.Value : _member_default_location); // 

				my_comm.Parameters.AddWithValue("@driverslicence", _driverslicence); // 
				my_comm.Parameters.AddWithValue("@eleclicence", _eleclicence); // 
				my_comm.Parameters.AddWithValue("@email", _email); // 
				my_comm.Parameters.AddWithValue("@emergfirstname1", _emergfirstname1); // 
				my_comm.Parameters.AddWithValue("@emergfirstname2", _emergfirstname2); // 
				my_comm.Parameters.AddWithValue("@emerglastname1", _emerglastname1); // 
				my_comm.Parameters.AddWithValue("@emerglastname2", _emerglastname2); // 
				my_comm.Parameters.AddWithValue("@emergphonearea1", _emergphonearea1); // 
				my_comm.Parameters.AddWithValue("@emergphonearea2", _emergphonearea2); // 
				my_comm.Parameters.AddWithValue("@emergphonefirst1", _emergphonefirst1); // 
				my_comm.Parameters.AddWithValue("@emergphonefirst2", _emergphonefirst2); // 
				my_comm.Parameters.AddWithValue("@emergphonelast1", _emergphonelast1); // 
				my_comm.Parameters.AddWithValue("@emergphonelast2", _emergphonelast2); // 
				my_comm.Parameters.AddWithValue("@emplogon", _emplogon); // 
				my_comm.Parameters.AddWithValue("@employeeid", _employeeID); // 
				my_comm.Parameters.AddWithValue("@employeenotes", _empnotes); // 
				my_comm.Parameters.AddWithValue("@firstname", _firstname); // 
				my_comm.Parameters.AddWithValue("@fullname", FullName); // 
				my_comm.Parameters.AddWithValue("@hasdependants", HasDependants); // 
				my_comm.Parameters.AddWithValue("@holiday", Holiday);
				my_comm.Parameters.AddWithValue("@hrstatus_id", hrstatus_id); // 
				my_comm.Parameters.AddWithValue("@is_can_boardmember", _is_CAN_boardmember); // 
				my_comm.Parameters.AddWithValue("@is_us_boardmember", _is_US_boardmember); // 
				my_comm.Parameters.AddWithValue("@lastname", _lastname); // 
				my_comm.Parameters.AddWithValue("@ldap_user", LDAP_user); // 
				my_comm.Parameters.AddWithValue("@member_id", id); // 
				my_comm.Parameters.AddWithValue("@membertype_id", _membertype_id); // 
				my_comm.Parameters.AddWithValue("@middleinitial", MiddleInitial); // 
				my_comm.Parameters.AddWithValue("@necellemail", _necellemail); // 
				my_comm.Parameters.AddWithValue("@neemail", _neemail); // 
				my_comm.Parameters.AddWithValue("@nickname", Nickname); // 
				my_comm.Parameters.AddWithValue("@pass", Password); // 
				my_comm.Parameters.AddWithValue("@payroll_handler", Toolbox.ReturnZeroIfNull_int(payroll_handler)); // 
				my_comm.Parameters.AddWithValue("@payroll_id", _payrollid); // 
				my_comm.Parameters.AddWithValue("@paytype_id", _paytype_id); //
				my_comm.Parameters.AddWithValue("@pecell_area", pecell_area); // 
				my_comm.Parameters.AddWithValue("@pecell_pref", pecell_pref); // 
				my_comm.Parameters.AddWithValue("@pecell_suff", pecell_suff); //  
				my_comm.Parameters.AddWithValue("@phoneextension", _phoneextension); // 
				my_comm.Parameters.AddWithValue("@phonefirst", _phonefirst); // 
				my_comm.Parameters.AddWithValue("@phonelast", _phonelast); // 
				my_comm.Parameters.AddWithValue("@postal", _postal); // 
				my_comm.Parameters.AddWithValue("@prov", _prov); // 
				my_comm.Parameters.AddWithValue("@receive_stat_pay", _receive_stat_pay); // 
				my_comm.Parameters.AddWithValue("@reports_to", reports_to); // 
				my_comm.Parameters.AddWithValue("@sin", _sin); // 
				my_comm.Parameters.AddWithValue("@startdate", Convert.ToDateTime(_startdate)); // 
				my_comm.Parameters.AddWithValue("@status", _status); // 
                // This will clear out the termination date of the existing user if the status has been changed back to active
                if (_status == "Active")
                {
                    my_comm.Parameters.AddWithValue("@termdate", new DateTime(2099,12,31)); 
                }
                else
                {
                    my_comm.Parameters.AddWithValue("@termdate", Convert.ToDateTime(_termdate));
                }
                my_comm.Parameters.AddWithValue("@time_to_stat", _timetostat); // 
				my_comm.Parameters.AddWithValue("@truck", _truck); // 
				my_comm.Parameters.AddWithValue("@user", Username); // 
				my_comm.Parameters.AddWithValue("@vacation_amount_1", _vacation_amount_1); // 
				my_comm.Parameters.AddWithValue("@vacation_amount_2", _vacation_amount_2); // 
				my_comm.Parameters.AddWithValue("@vacation_amount_3", _vacation_amount_3); // 
				my_comm.Parameters.AddWithValue("@vacation_interval_1", _vacation_interval_1); // 
				my_comm.Parameters.AddWithValue("@vacation_interval_2", _vacation_interval_2); // 
				my_comm.Parameters.AddWithValue("@vacation_interval_3", _vacation_interval_3); // 
				my_comm.Parameters.AddWithValue("@windows_password", _member_windows_password); // 
				my_comm.Parameters.AddWithValue("@disable_ldap_sync", disable_ldap_sync); // 
				my_comm.Parameters.AddWithValue("@scheduled_by", reports_to); // 
				my_comm.Parameters.AddWithValue("@member_TS_watch", _tswatch == true ? 1 : 0); // 
				my_comm.Parameters.AddWithValue("@force_beta", _force_beta); // 
				my_comm.Parameters.AddWithValue("@bonus_notes", _bonus_notes); // 
				my_comm.Parameters.AddWithValue("@gets_vehicle", gets_vehicle); // 
				my_comm.Parameters.AddWithValue("@gets_phone", gets_phone); // 
				my_comm.Parameters.AddWithValue("@gets_laptop", gets_laptop); // 
				my_comm.Parameters.AddWithValue("@gets_barcodescanner", gets_barcodescanner); // 
				my_comm.Parameters.AddWithValue("@gets_businesscards", gets_businesscards); // 
				my_comm.Parameters.AddWithValue("@gets_directdeposit", gets_directdeposit); // 
				my_comm.Parameters.AddWithValue("@gets_neemail", gets_neemail); // 
				my_comm.Parameters.AddWithValue("@gets_phoneext", gets_phoneext); // 
				my_comm.Parameters.AddWithValue("@benefits_id", benefits_id); // 
				my_comm.Parameters.AddWithValue("@life_insurance_id", life_insurance_id); // 
				my_comm.Parameters.AddWithValue("@benefits_startdate", benefits_startdate);
				my_comm.Parameters.AddWithValue("@part_time", part_time);
				my_comm.Parameters.AddWithValue("@vendor_id", vendor_id);
				my_comm.Parameters.AddWithValue("@color", color);
				my_comm.Parameters.AddWithValue("@include_in_mobile_contactlist", _include_in_mobile_contactlist);
				my_comm.Parameters.AddWithValue("@member_probation_email_sent", _member_probation_email_sent);
				my_comm.Parameters.AddWithValue("@sent_welcome_email", sent_welcome_email);
				my_comm.Parameters.AddWithValue("@business_unit_id", _business_unit_id);
				my_comm.Parameters.AddWithValue("@auto_vacation_payout", ReceivesAutoVacationPayout);
				#endregion PARAMETERS

				
				try
				{ 
					my_comm.ExecuteNonQuery();

                    #region Member id gap

                    // Update the member_payroll_id for those members that are added in the memberID gap 
                    if (id == 0 && memIDgap == 0)
					{
						my_comm.CommandText = "SELECT LAST_INSERT_ID()";
						id = Convert.ToInt32(my_comm.ExecuteScalar());
						
					}
                    else if(memIDgap > 0)
                    {
                        id = memIDgap;
                    }
                    if(string.IsNullOrEmpty(_payrollid) || _payrollid.Trim() == "0")
                    {
                     Toolbox.doSQL_void(my_conn, @"
UPDATE 
	member a 
LEFT JOIN 
	business_unit b ON a.business_unit_id = b.id 
LEFT JOIN 
	tax_entity c ON b.tax_entity_id = c.id 
SET 
	a.member_payroll_id = CONCAT(IFNULL(c.adp_company_code, ''),a.member_id) 
WHERE 
	a.member_id = @v0", new object[] { id });
                    }
                    #endregion Member id gap
				   }
                catch (Exception ex)
				{
					Toolbox.do_catch_error(ex,711);
					throw;
				}               
            }

         
			if (!is_local && previous_values.MemberTypeID != 0 && previous_values.MemberTypeID != _membertype_id)
			{
                #region Member Type Switch - Moves stuff around.

                
                var employee = new NeMember(id);
                var mt_old = new NeMemberType(previous_values.MemberTypeID);
				var mt_new = new NeMemberType(_membertype_id);
                var curr_branch = new NeBusinessUnit(employee.business_unit_id);
              //  HasZeroChargeouts(id, mt_old.MemberTypeID, mt_new.MemberTypeID,employee,curr_branch);

                var em_subject = _fullname + " was a(n) " + mt_old.name + " and now is a(n) " + mt_new.name;
				var em_body = "Please adjust their exchange or outlook signature, if required";
				shared.alert_it(em_subject, em_body);


				// Clear old privileges, add new ones based on member type.
				var typepriv = new NEMemberTypePrivilege();
				typepriv.ClearPrivileges(id);
				typepriv.SetMemberPrivileges(id);

				if (LDAP_user != "" && enable_ldap && enable_ldap_sync)
				{
					/*
						var sb = new System.Text.StringBuilder();
						sb.AppendFormat(@"
		<div>Distribution Group Report for moving {0} from a [{1}] to a [{2}]</div> 
		<table>
			<thead>
				<tr>
					<th>Action</th>
					<th>Group Name</th>
					<th>Success/Failed</th>
					<th>Message</th>
				</tr>
			</thead>
			<tbody>", FullName, mt_old.name, mt_new.name);
						var dt_remove = Toolbox.doSQL_dt(@"SELECT b.id, b.name FROM distribution_group_link a LEFT JOIN distribution_group b ON a.distribution_group_id = b.id WHERE a.membertype_id = @v0 ", new object[] { mt_old.id });
						foreach (DataRow dr in dt_remove.Rows)
						{
							var name = dr["name"].ToString();
							var bs_remove = ldap_group_admin("remove", name, LDAP_user);
							sb.AppendFormat("<tr><td>Removed</td><td>{0}</td><td>{1}</td><td>{2}</td></tr>", name, bs_remove.success, bs_remove.message);
						}
						var dt_add = Toolbox.doSQL_dt(@"SELECT b.id, b.name FROM distribution_group_link a LEFT JOIN distribution_group b ON a.distribution_group_id = b.id WHERE a.membertype_id = @v0 ", new object[] { mt_new.id });
						foreach (DataRow dr in dt_add.Rows)
						{
							var name = dr["name"].ToString();
							var bs_add = ldap_group_admin("add", name, LDAP_user);
							sb.AppendFormat("<tr><td>Added</td><td>{0}</td><td>{1}</td><td>{2}</td></tr>", name, bs_add.success, bs_add.message);
						}
						sb.Append(@"
			</tbody>
		</table>");
						shared.alert_it("Distribution list changed by membertype update", sb.ToString());
					*/
				}
				#endregion Member Type Switch - Moves stuff around.
			}

			if (previous_values.id != 0 && previous_values.Password != _pass)
			{
				Toolbox.doSQL_void(@"UPDATE ne_session SET is_active = 0 WHERE member_id = @v0 ", new object[] { id });
			}
			#region Disable credit card if business unit is changed
			if(previous_values.business_unit_id != _business_unit_id)
			{
				var credit_cards_owned = Toolbox.doSQL_dt(@"
										SELECT id, type, number FROM credit_cards 
										WHERE member_id = @v0 
										AND business_unit_id = @v1
										AND STATUS = 'Active'",
										new object[] { id, previous_values.business_unit_id });
				var sb_cc = new StringBuilder();
				if(credit_cards_owned.Rows.Count > 0)
				{
					sb_cc.Append("<ul>");
					foreach(DataRow cc in credit_cards_owned.Rows)
					{
						var cc_type = cc["type"];
						var cc_number = cc["number"];
						Toolbox.doSQL_void(@"UPDATE credit_cards SET STATUS = 'Cancelled' WHERE id = @v0 AND business_unit_id = @v1", new object[] { cc["id"] , previous_values.business_unit_id});
						sb_cc.AppendFormat("<li>Type: {0} - Last four digits: {1}",cc_type,cc_number);
					}
					sb_cc.Append("</ul>");
				}
				if(credit_cards_owned.Rows.Count > 0) 
				{ 
				//Send AP a notification to set up new CC
				var new_bu = new NeBusinessUnit(_business_unit_id);					
				try
					{
						var emailNotification = new NeEMail();
						emailNotification.To = EmailID.AP + Toolbox.app_setting("DomainForEmail");
						emailNotification.From = "noreply@" + Toolbox.app_setting("DomainForEmail");
						emailNotification.Subject = "SparkOps CC Cancellation: " + FullName + " moved to different branch";
						emailNotification.Body = $@"
								 As <b>{FullName}</b> was moved from <b>{previous_values.business_unit_name}</b> to <b>{new_bu.ddl_name}</b>, <br/>
								 the following credit cards in the previous business unit have been cancelled: <br/>
								 {sb_cc.ToString()} <br/> <br/>
								 Please setup new credit card(s) under <b>{new_bu.ddl_name}</b> as soon as possible.<br/>";

						emailNotification.Send();
					}
				catch(Exception ee)
					{
						Toolbox.do_errorLog_errorStack(ee);
					}
				}
            }
			#endregion Disable credit card if business unit is changed
		}

		[Obsolete("Method is deprecated, use the save() method")]
		public void UpdateNeMember() /* Update Member Info in Employee Tab */
		{
			save();
		}
		public void sync_active_directory()
		{
			if (id > 0)
			{
				sync_active_directory(id);
			}
		}
       

     /*   public static void HasZeroChargeouts(int memberid, int prev_employee_MemberTypeID,int membertypeid, NeMember employee, NeBusinessUnit curr_branch)
        {

            var rowsToUpdate = Toolbox.doSQL_dt(@"
SELECT 
	a.wo_detail_current_id id,
	IFNULL(d.id,0) new_chargeoutid,
	IFNULL(d.chargeout, 0.0) new_chargeout
FROM 
	wo_detail_current a 
LEFT JOIN 
	woprog b ON a.wo_detail_current_woprog_id = b.woprog_id 
LEFT JOIN 
	membertype_chargeout c ON c.id = a.wo_detail_current_master_id 
LEFT JOIN
	membertype_chargeout d ON d.business_unit_id = a.business_unit_id AND d.membertype_id = @v2 AND d.paytype_id = a.paytypeid
WHERE 
	b.woprog_status IN ('Open', 'Initial Prep', 'Questions for PM', 'Rework') AND 
	a.wo_detail_current_type = 'L' AND 
	a.memberid = @v0 AND
	c.membertype_id = @v1
	", new object[] { memberid, prev_employee_MemberTypeID, membertypeid });
            var hasZeroChargeouts = false;
            foreach (DataRow dr in rowsToUpdate.Rows)
            {
                var newChargeoutId = Convert.ToInt32(dr["new_chargeoutid"]);
                var newChargeout = Convert.ToDouble(dr["new_chargeout"]);
                // update row to new master id & code to new chargeout id, price sell & price_unit to new chargeout price... don't touch cost
                if (newChargeout == 0 || newChargeoutId == 0)
                {
                    hasZeroChargeouts = true;
                }
            }


            if (hasZeroChargeouts)
            {
                var newMemberType = new NeMemberType(membertypeid);
                var emailNotification = new NeEMail();
                emailNotification.To = curr_branch.branch_manager.NEEmail;
                emailNotification.From = "noreply@" + Toolbox.app_setting("DomainForEmail");
                emailNotification.CC = "hr@" + Toolbox.app_setting("DomainForEmail");
                emailNotification.Bcc = "mhyde@" + Toolbox.app_setting("DomainForEmail");
                emailNotification.Subject = "Blank chargeout(s) detected, in your business unit, for member type '" + newMemberType.name + "'";
                emailNotification.Body = string.Format(@"
            While {0}'s employee profile was being updated via their offer, it was noticed that the chargeouts were not set up fully for their new member type - '{1}' in your business unit ({2}). 
            Please see that these new chargeouts are setup as soon as possible, until then this employee cannot move to their new member type.", employee.FullName, newMemberType.name, curr_branch.ddl_name);
                emailNotification.Send();
                throw new Exception("Not proceeding. Blank chargeout detected while trying to move to the new member type... An email has been sent to " + curr_branch.branch_manager.FullName + " to correct this.");
            }
            //else
            //{
            //    foreach (DataRow dr in rowsToUpdate.Rows)
            //    {
            //        var rowId = Convert.ToInt32(dr["id"]);
            //        var newChargeoutId = Convert.ToInt32(dr["new_chargeoutid"]);
            //        var newChargeout = Convert.ToDouble(dr["new_chargeout"]);
            //        // update row to new master id & code to new chargeout id, price sell & price_unit to new chargeout price... don't touch cost
            //        Toolbox.doSQL_void(@"
            //UPDATE 
            //	wo_detail_current 
            //SET 
            //	wo_detail_current_date_modified = wo_detail_current_date_modified, 
            //	wo_detail_current_master_id = @v0, 
            //	wo_detail_current_code = @v0, 
            //	wo_detail_current_price_sell = @v1, 
            //	wo_detail_current_price_unit = @v1 
            //WHERE 
            //	wo_detail_current_id = @v2 
            //LIMIT 1", new object[] { newChargeoutId, newChargeout, rowId });
            //    }
            //}

        }*/

        public static bool ldap_group_contains_user(string group_name, string username)
		{
			var ip_address = Toolbox.app_setting("ldap_ip");
			var ldap_base = "OU=NE,dc=NewElectric,dc=local";
			var group = new DirectoryEntry(string.Format("{0}CN={1},OU=NEDistributionGroups,{2}", ip_address, group_name, ldap_base), Toolbox.app_setting("ldap_user"), Toolbox.app_setting("ldap_pass"));
			var members = group.Invoke("Members", null);
			var de = new DirectoryEntry(ldap_get_path(username), Toolbox.app_setting("ldap_user"), Toolbox.app_setting("ldap_pass"));
			var is_group_member = false;
			foreach (var member in (IEnumerable)members)
			{
				var x = new DirectoryEntry(member);
				if (x.Name == de.Name)
				{
					is_group_member = true;
					break;
				}
			}
			return is_group_member;
		}
		public static Toolbox.boolstr ldap_group_admin(string action, string group_name, string username)
		{
			var ip_address = Toolbox.app_setting("ldap_ip");
			var ldap_base = "OU=NE,dc=NewElectric,dc=local";
			var ldap_path = string.Format("{0}OU=NEDistributionGroups,{1}", ip_address, ldap_base);
			var group = new DirectoryEntry(string.Format("{0}CN={1},OU=NEDistributionGroups,{2}", ip_address, group_name, ldap_base), Toolbox.app_setting("ldap_user"), Toolbox.app_setting("ldap_pass"));
			var exists = ldap_group_contains_user(group_name, username);
			var bs = new Toolbox.boolstr();
			switch (action)
			{
				case "add":
					if (!exists)
					{
						group.Invoke(action, new object[] { ldap_get_path(username) });
						group.CommitChanges();
						bs.success = true;
						bs.message = "Added to group";
					}
					else
					{
						bs.success = false;
						bs.message = "Already exists in group";
					}
					break;
				case "remove":
					if (exists)
					{
						group.Invoke(action, new object[] { ldap_get_path(username) });
						group.CommitChanges();
						bs.success = true;
						bs.message = "Removed from group";
					}
					else
					{
						bs.success = false;
						bs.message = "Doesn't exist in group";
					}
					break;
			}
			return bs;
		}
		public static string ldap_get_path(string username)
		{
			var ldap_path = Toolbox.app_setting("ldap_path");
			using (var ad_admin = new DirectoryEntry(ldap_path, Toolbox.app_setting("ldap_user"), Toolbox.app_setting("ldap_pass"), AuthenticationTypes.Secure))
			{
				using (var ad_search = new DirectorySearcher(ad_admin, "(objectclass=*"))
				{
					ad_search.Filter = "(SAMAccountName=" + username + ")";
					var ad_user = ad_search.FindOne().GetDirectoryEntry();
					return ad_user != null ? ad_user.Path : "";
				}
			}
		}
		public void sync_active_directory(int _id)
		{
			if (Toolbox.app_setting("enable_ldap") != "1" || Toolbox.app_setting("enable_ldap_sync") != "1")
			{
				return;
			}
			build_props(_id);
			try
			{
				if (LDAP_user != "" && !disable_ldap_sync)
				{
					var ldap_path = "LDAP://192.168.0.197/dc=NewElectric,dc=local";
					using (var ad_admin = new DirectoryEntry(ldap_path, Toolbox.app_setting("ldap_user"), Toolbox.app_setting("ldap_pass"), AuthenticationTypes.Secure))
					{
						using (var ad_search = new DirectorySearcher(ad_admin))
						{
							ad_search.Filter = "(SAMAccountName=" + LDAP_user + ")";
							if (ad_search.FindOne() != null)
							{
								var ad_user = ad_search.FindOne().GetDirectoryEntry();
								if (ad_user != null)
								{
									if (membertype != null)
									{
										set_AD_property(ref ad_user, "title", membertype.name);
									}
									if (cellphone_number_id > 0)
									{
										set_AD_property(ref ad_user, "mobile", new NECellphone_Number(cellphone_number_id).number);
									}
									if (business_unit != null)
									{
										set_AD_property(ref ad_user, "company", business_unit.description.Length > 64 ? business_unit.description.Substring(0, 63) : business_unit.description);
										set_AD_property(ref ad_user, "streetAddress", business_unit.address);
										set_AD_property(ref ad_user, "physicalDeliveryOfficeName", business_unit.PhoneNumber);
										set_AD_property(ref ad_user, "postalCode", business_unit.postal);
										set_AD_property(ref ad_user, "co", business_unit.country == "CDN" ? "Canada" : "United States");
										set_AD_property(ref ad_user, "facsimileTelePhoneNumber", business_unit.FaxNumber);
										set_AD_property(ref ad_user, "l", business_unit.city + ", " + business_unit.provstate);
									}
									if (PhoneExtension != "")
									{
										set_AD_property(ref ad_user, "TelephoneNumber", PhoneExtension);
									}
									ad_user.CommitChanges();
								}
							}
						}
					}
				}
			}
			catch (Exception ee)
			{
				Toolbox.do_catch_error(ee, 711);
			}
		}
		/// <summary>
		/// This will consolidate any wrongly created employee accounts into the correct ones.
		/// </summary>
		/// <param name="_wrong_id"></param>
		/// <param name="_correct_id"></param>
		public static void consolidate_accounts(int _wrong_id, int _correct_id)
		{
			// DO
			// NOT 
			// USE (yet)
			return;
			if (_wrong_id < _correct_id)
			{
				throw new Exception("The correct member id should always be less than the incorrect member id");
			}
			//TENENT:  Always update the timestamp to the same timestamp... Preferably as the first item of the update statement
			using (var conn = Toolbox.connect())
			{
				// Template update statement
				// Toolbox.doSQL_void(conn,@"", new object[] {  _wrong_id, _correct_id } );
				var wrong_membertype_id = 0;
				var wrong_paytype_id = 0;

				var correct_membertype_id = 0;
				var correct_paytype_id = 0;

				if (_wrong_id > 0)
				{
					wrong_membertype_id = Toolbox.doSQL_int(@"SELECT member_membertype_id FROM member WHERE member_id = @v0 ", new object[] { _wrong_id });
				}
				if (_correct_id > 0)
				{
					correct_membertype_id = Toolbox.doSQL_int(@"SELECT member_membertype_id FROM member WHERE member_id = @v0 ", new object[] { _correct_id });
				}
				//TENENT: Update the triggered log statements first
				#region Timesheets - Log
				Toolbox.doSQL_void(conn, @"UPDATE membertime SET ts = ts,business_unit_id = @v1  WHERE business_unit_id = @v0 ", new object[] { _wrong_id, _correct_id });
				Toolbox.doSQL_void(conn, @"UPDATE membertime SET ts = ts,Member_ID_Create = @v1  WHERE Member_ID_Create = @v0 ", new object[] { _wrong_id, _correct_id });
				Toolbox.doSQL_void(conn, @"UPDATE membertime SET ts = ts,Member_ID_Audit = @v1  WHERE Member_ID_Audit = @v0 ", new object[] { _wrong_id, _correct_id });
				#endregion Timesheets - Log
				#region Timesheets - Current
				Toolbox.doSQL_void(conn, @"UPDATE membertime SET ts = ts,business_unit_id = @v1  WHERE business_unit_id = @v0 ", new object[] { _wrong_id, _correct_id });
				Toolbox.doSQL_void(conn, @"UPDATE membertime SET ts = ts,Member_ID_Create = @v1  WHERE Member_ID_Create = @v0 ", new object[] { _wrong_id, _correct_id });
				Toolbox.doSQL_void(conn, @"UPDATE membertime SET ts = ts,Member_ID_Audit = @v1  WHERE Member_ID_Audit = @v0 ", new object[] { _wrong_id, _correct_id });
				#endregion Timesheets - Current
				#region Work Orders
				Toolbox.doSQL_void(conn, @"UPDATE log.wo_detail_current SET ts = ts, wo_detail_current_added_by = @v1  WHERE wo_detail_current_added_by = @v0 ", new object[] { _wrong_id, _correct_id });
				Toolbox.doSQL_void(conn, @"UPDATE log.wo_detail_current SET ts = ts, memberid = @v1  WHERE memberid = @v0 ", new object[] { _wrong_id, _correct_id });
				#endregion Work Orders
				#region Offers
				Toolbox.doSQL_void(conn, @"", new object[] { _wrong_id, _correct_id });

				#endregion Offers
				#region Wage

				#endregion Wage
				#region RFQs

				#endregion RFQs
				#region Expenses

				#endregion Expenses
			}
		}
		private void set_AD_property(ref DirectoryEntry ad_user, string property, string value)
		{
			if (ad_user.Properties.Contains(property))
			{
				ad_user.Properties[property][0] = value;
			}
			else
			{
				ad_user.Properties[property].Add(value);
			}
		}
		public bool ismemberinticketgroup(int groupid)
		{
			try
			{
				if (Toolbox.doSQL_int(@"Select count(ticketmanager_id) from ticketmanager  where ticketmanager_member_id =@v0 and ticketmanager_group_id =@v1 ", new object[] { id, groupid }) > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch { return false; }
		}
		public bool ismemberanassignee()
		{
			try
			{
				if (Toolbox.doSQL_int(@"Select count(ticketmanager_id) from ticketmanager  where ticketmanager_member_id =@v0", new object[] { id }) > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch { return false; }

		}
		public NePage GetPage(string page_id)
		{
			foreach (NePage page in _pages)
			{
				if (page_id == Convert.ToString(page.ID))
				{
					return page;
				}
			}
			return null;
		}
		public bool authenticated_for(string _type, int _id)
		{
			if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["master_permissions"] != null && SessionID == Convert.ToInt32(HttpContext.Current.Session["session"]))
			{
				_permissions = (List<KeyValuePair<string, int>>)HttpContext.Current.Session["master_permissions"];
			}
			else if (_permissions == null)
			{
				var prefix = id < 100000 ? "member" : "contact";
				var used_id = id < 100000 ? id : ContactID;
				var page_dt = Toolbox.doSQL_dt(string.Format(@"SELECT {0}page_page_id id FROM {0}page WHERE {0}page_{0}_id = @v0 AND active = 1 ORDER BY {0}page_page_id", prefix), new object[] { used_id });
				var priv_dt = Toolbox.doSQL_dt(string.Format(@"SELECT {0}pageprivilege_privilege_id id FROM {0}pageprivilege WHERE {0}pageprivilege_{0}_id = @v0 AND active = 1 ORDER BY {0}pageprivilege_privilege_id", prefix), new object[] { used_id });
				_permissions = new List<KeyValuePair<string, int>>();
				//Page Dictionary Entries
				var this_id = 0;
				foreach (DataRow dr in page_dt.Rows)
				{
					this_id = Convert.ToInt32(dr["id"]);
					var i = new KeyValuePair<string, int>("page", this_id);
					_permissions.Add(i);
				}
				this_id = 0;
				if (prefix == "member")
				{
					var global_privs = Toolbox.doSQL_dt(@"SELECT global_priv_id id FROM privilege_global_link where type_id = '1' AND user_id = @v0 ", new object[] { id });
					foreach (DataRow dr in global_privs.Rows)
					{
						this_id = Convert.ToInt32(dr["id"]);
						var i = new KeyValuePair<string, int>("priv", this_id);
						_permissions.Add(i);
					}
				}
				this_id = 0;
				foreach (DataRow dr in priv_dt.Rows)
				{
					this_id = Convert.ToInt32(dr["id"]);
					var i = new KeyValuePair<string, int>("priv", this_id);
					_permissions.Add(i);
				}
			}
			return _permissions.Contains(new KeyValuePair<string, int>(_type, _id));
		}
		public bool AuthenticatedForPage(int page_id)
		{
			return authenticated_for("page", page_id);
		}
		public bool AuthenticatedForPage(string page_id)
		{
			return authenticated_for("page", Convert.ToInt32(page_id));
		}
		public bool AuthenticatedForPrivilege(int priv_id)
		{
			return authenticated_for("priv", priv_id);
		}

		private void populate_permissions()
		{
			var p = new shared.properties("reset_" + id);
			if (HttpContext.Current != null && HttpContext.Current.Session != null && (HttpContext.Current.Session["master_permissions"] == null || p.value != ""))
			{
				var prefix = id < 100000 ? "member" : "contact";
				var used_id = id < 100000 ? id : ContactID;
				var page_dt = Toolbox.doSQL_dt(string.Format(@"SELECT {0}page_page_id id FROM {0}page WHERE {0}page_{0}_id = @v0 AND active = 1 ORDER BY {0}page_page_id", prefix), new object[] { used_id });
				var priv_dt = Toolbox.doSQL_dt(string.Format(@"SELECT {0}pageprivilege_privilege_id id FROM {0}pageprivilege WHERE {0}pageprivilege_{0}_id = @v0 AND active = 1 ORDER BY {0}pageprivilege_privilege_id", prefix), new object[] { used_id });
				var master_permissions = new List<KeyValuePair<string, int>>();
				//Page Dictionary Entries
				var _id = 0;
				foreach (DataRow dr in page_dt.Rows)
				{
					_id = Convert.ToInt32(dr["id"]);
					var i = new KeyValuePair<string, int>("page", _id);
					master_permissions.Add(i);
				}
				_id = 0;
				foreach (DataRow dr in priv_dt.Rows)
				{
					_id = Convert.ToInt32(dr["id"]);
					var i = new KeyValuePair<string, int>("priv", _id);
					master_permissions.Add(i);
				}
				HttpContext.Current.Session["master_permissions"] = master_permissions;
			}
		}
		public static string GetMemberUserName(int MemberID)
		{
			return Toolbox.doSQL_string(@"SELECT IFNULL(MAX(member_user), 'N/A') FROM member WHERE member_id = @v0 ", new object[] { MemberID });
		}
		public void print_barcode_label(int copies)
		{
			try
			{
				var printDoc = new PrintDocument();
				var WOPrinter = new NeBusinessUnit(_business_unit_id);
				var yy = new PaperSize("Custom Paper Size", 220, 99);
				printDoc.DefaultPageSettings.PaperSize = yy;
				printDoc.DefaultPageSettings.Margins.Left = 1;
				printDoc.DefaultPageSettings.PrinterSettings.Copies = (short)copies;
				printDoc.PrinterSettings.PrinterName = WOPrinter.BarCodePrinter;
				printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
				printDoc.Print();
			}
			catch (Exception ex)
			{
				Toolbox.do_catch_error(ex, 711);
				throw new Exception("Printing Barcode Failed");
			}
		}
		private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
		{
			try
			{
				var rightAlign = new StringFormat();
				rightAlign.Alignment = StringAlignment.Far;
				rightAlign.LineAlignment = StringAlignment.Far;
				var leftAlign = new StringFormat();
				leftAlign.Alignment = StringAlignment.Near;
				leftAlign.LineAlignment = StringAlignment.Near;

				var printFont = new System.Drawing.Font("Arial", 9);
				var printFont1 = new System.Drawing.Font("Arial", 8);
				var printFontdesc = new System.Drawing.Font("Arial", 5);
				var barcodefont = new System.Drawing.Font("Free 3 of 9 Extended", 24);
				var rect = new Rectangle(10, 32, 195, 50);
				var br = new SolidBrush(Color.Black);
				e.Graphics.DrawRectangle(Pens.Transparent, rect);
				e.Graphics.DrawString(id.ToString(), printFont, br, 10, 51, leftAlign);
				e.Graphics.DrawString("Employee Bar Code", printFont1, br, 10, 71, leftAlign);
				e.Graphics.DrawString(_fullname, printFontdesc, br, rect);
				e.Graphics.DrawString("*003-" + id + "*", barcodefont, br, 15, 5);

			}
			catch (Exception ex)
			{
				Toolbox.do_catch_error(ex, 711);
			}
		}
		public static List<int> supervisor_list(int _id)
		{
			var managerList = new List<int>();
			var managerDt = get_supervisors(_id);
			if (managerDt.Rows.Count == 0) return managerList;
			foreach (DataRow dr in managerDt.Rows)
			{
				managerList.Add(Convert.ToInt32(dr["memberid"]));
			}
			return managerList;
		}
		public static DataTable get_supervisors(int memberid)
		{
			var dt = new DataTable();
			dt.Columns.Add("memberid");
			dt.Columns.Add("memberfullname");
			dt.Columns.Add("LevelAbove");
			dt.Columns.Add("Membertype");
			dt.Columns.Add("NEEmail");
			dt.Columns.Add("MemberTypeID");
            for (var i = 0; i < 20; i++)
			{
				var reportsto = Toolbox.doSQL_int(@"Select ifnull((Select ifnull(reports_to,0) from member  where member_id =@v0),0) ", new object[] { memberid });
				if (reportsto == 0)
				{
					break;
				}
				else
				{
					var rep_to_mem = new NeMember(reportsto);
					dt.Rows.Add(reportsto, rep_to_mem.FullName, i, rep_to_mem.membertype.MemberTypeName, rep_to_mem.NEEmail, rep_to_mem.MemberTypeID);
					memberid = reportsto;
				}
			}
			return dt;
		}
		public static DataTable get_allreports(int memberid)
		{
			var dt = new DataTable();
			dt.Columns.Add("member_id");
			dt.Columns.Add("member_fullname");
			dt.Columns.Add("LevelBelow");
			dt.Columns.Add("Member_MemberType_ID");
			dt.Columns.Add("member_neemail");
			var emplist = Toolbox.doSQL_dt(@"Select member_id,member_fullname,Member_MemberType_ID,member_neemail from member  where reports_to =@v0 and member_status = 'Active'", new object[] { memberid });
			var i = 1;
			foreach (DataRow dr in emplist.Rows)
			{
				dt.Rows.Add(dr["member_id"], dr["member_fullname"], i, dr["Member_MemberType_ID"], dr["member_neemail"]);
				var dt1 = get_allreports(Convert.ToInt32(dr["member_id"]));
				foreach (DataRow dr1 in dt1.Rows)
				{
					i = 2;
					dt.Rows.Add(dr1["member_id"], dr1["member_fullname"], i, dr1["Member_MemberType_ID"], dr1["member_neemail"]);
				}
			}
			var view = new DataView(dt);
			var distinctvalues = view.ToTable(true, "member_id", "member_fullname", "Member_MemberType_ID", "member_neemail");
			return distinctvalues;
		}

		public static List<int> ReportsToList(int id)
			{
			var ReportsToCSV		= Toolbox.doSQL_string(@"SELECT IFNULL(REPORTS_TO(@v0), '')", new object[]{id});
			return ReportsToCSV == "" ? new List<int>() : ReportsToCSV.Split(',').Select(int.Parse).ToList();
			}

		public static bool is_owner(int memberid, int tax_entity_id)
		{
			return Toolbox.doSQL_bool("SELECT IS_OWNER(@v0,@v1)", new object[] { memberid, tax_entity_id });
		}

		public static bool is_supervisor(int memberid, int supervisorid)
		{
			return Toolbox.doSQL_int(@"SELECT IS_SUPERVISOR(@v0, @v1)", new object[] { memberid, supervisorid }) == 1;
		}
		public static string get_reporting_line(int _id)
			{
			return get_reporting_line(_id, " -> ");
			}
		/// <summary>
		/// Returns a delimited list of employees in employee's reporting line.
		/// </summary>
		/// <param name="_id"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static string get_reporting_line(int _id, string separator)
		{
			var x = new StringBuilder();
			var _last_id = _id;
			using (var uow = new UnitOfWork())
			{
				while (_last_id != 0)
				{
					var this_user = uow.GetObjectByKey<ne_xpo.cs.member>(_last_id);
					if (this_user == null)
					{
						_last_id = 0;
						continue;
					}
					var this_reports_to = uow.GetObjectByKey<ne_xpo.cs.member>(this_user.reports_to);
					if (this_reports_to == null)
					{
						_last_id = 0;
						continue;
					}
					else if (this_reports_to.member_id == _id)
					{
						_last_id = 0;
						Toolbox.do_catch_error(new Exception("Infinite recursion when getting reporting line."), 711);
						continue;
					}
					else
					{
						x.AppendFormat($"{separator}{this_reports_to.member_fullname}");
						_last_id = this_reports_to.member_id;
					}
				}
			}
			return x.ToString();
		}
		/// <summary>
		/// Returns a List ints of employees in employee's reporting line.
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>
		public static List<int> GetReportingLineList(int _id)
		{
			var x = new List<int>();
			var _last_id = _id;
			using (var uow = new UnitOfWork())
			{
				while (_last_id != 0)
				{
					var this_user = uow.GetObjectByKey<ne_xpo.cs.member>(_last_id);
					if (this_user == null)
					{
						_last_id = 0;
						continue;
					}
					var this_reports_to = uow.GetObjectByKey<ne_xpo.cs.member>(this_user.reports_to);
					if (this_reports_to == null)
					{
						_last_id = 0;
						continue;
					}
					else if (this_reports_to.member_id == _id)
					{
						_last_id = 0;
						Toolbox.do_catch_error(new Exception("Infinite recursion when getting reporting line."), 711);
						continue;
					}
					else
					{
						x.Add(this_reports_to.member_id);
						_last_id = this_reports_to.member_id;
					}
				}
			}
			return x;
		}
		public static List<int> GetSubordinatesForBu(NeMember supervisor, int businessUnitId, NePayPeriod payPeriod)
			{
			var reportingLine = supervisor.Report_To_List(supervisor.id);
			var reportingLineCsv = string.Join(",", reportingLine);
			var SubListCsv = Toolbox.doSQL_string(@"
SELECT 
	GROUP_CONCAT(member_id) 
FROM 
	member 
WHERE 
	business_unit_id = @v0 AND 
	FIND_IN_SET(member_id, @v1) AND 
	(member_status = 'Active' OR member_termdate BETWEEN @v2 AND @v3)", new object[]{ businessUnitId, reportingLineCsv, payPeriod.start_date, payPeriod.end_date });
			var SubList = SubListCsv == "" 
							? new List<int>() 
							: SubListCsv.Split(',').Select(int.Parse).ToList();
			return SubList;
			}
		public static DataTable GetEmployees(int _business_unit_id)
		{
			return Toolbox.doSQL_dt(@"SELECT member_id id, member_name(member_id) name FROM member WHERE member_status = 'Active' AND business_unit_id = @v0  ORDER BY member_fullname", new object[] { _business_unit_id });
		}
		public static bool Check_for_circular_org_chart(int memberid, int optional_reports_to = 99999)
		{
			var n_mem = new NeMember(memberid);
			if (optional_reports_to != 99999)
			{
				n_mem.reports_to = optional_reports_to;
			}
			if (n_mem.reports_to == memberid)
			{
				return true;
			}
			else
			{
				var x = n_mem.reports_to;
				var y = 0;
				var exist = new List<int> { x, memberid };
				while (x != 0)
				{
					var x_mem = new NeMember(x);
					x = x_mem.reports_to;
					if (exist.Contains(x))
					{
						return true;
					}
					exist.Add(x);
					y++;
					if (y > 100)
					{
						return true;
					}
				}
			}
			return false;
		}
		
		public class to_do
		{
			public static bool exists(int _member_id, string _type, int _type_id, MySqlConnection _conn)
			{
				var reports_to_list = Toolbox.doSQL_string(_conn, @"SELECT IFNULL(REPORTS_TO(@v0 ), '')", new object[] { _member_id });
				return Toolbox.doSQL_int(_conn, @"SELECT count(id) FROM member_todo_list_helper WHERE (member_id = @v0  OR FIND_IN_SET(member_id, @v1 )) AND type=@v2  and type_id = @v3 ", new object[] { _member_id, reports_to_list, _type, _type_id }) > 0;
			}
			public static void add(int _member_id, DateTime _dt, string _wording, string _hyperlink, string _type, int _type_id, MySqlConnection _conn)
			{
				if (exists(_member_id, _type, _type_id, _conn))
				{
					delete(_member_id, _type, _type_id, _conn);
				}
				Toolbox.doSQL_void(_conn, @"INSERT INTO member_todo_list_helper (member_id, date, wording, hyperlink,type, type_id) VALUES (@v0 ,@v1 ,@v2 ,@v3 ,@v4 ,@v5 )", new object[] { _member_id, Toolbox.MySQL_longdt(_dt), _wording, _hyperlink, _type, _type_id });
			}
			public static void delete(int _member_id, string _type, int _type_id, MySqlConnection _conn)
			{
				Toolbox.doSQL_void(_conn, @"DELETE FROM member_todo_list_helper WHERE member_id = @v0  AND type=@v1  AND type_id = @v2 ", new object[] { _member_id, _type, _type_id });
			}
		}
	}
	public class member_fvr_hdr
	{
		public int id { get; set; }
		public string type { get; set; }
		public int active { get; set; }
		public string business_unit_ids { get; set; }
		public string tabs_needed { get; set; }
		public DateTime release_date { get; set; }
		public DateTime expire_date { get; set; }
		public int req_member_id { get; set; }
		public partial class types
		{
			public static string NEWHIRE { get { return "NEWHIRE"; } }
			public static string EXIT { get { return "EXIT"; } }
			public static string RENEW { get { return "RENEW"; } }
			public static List<string> ids { get { return new List<string>(new string[] { "NEWHIRE", "EXIT", "RENEW" }); } }
		}
		public member_fvr_hdr()
		{
		}
		public static DataTable chk_member(int _id)
		{

			var _session = HttpContext.Current.Session;
			if (_session["is_n1"] != null && (int)_session["is_n1"] == 1)
			{
				return new DataTable();
			}

			var n = Toolbox.doSQL_dt(@" SELECT a.id, a.type, COUNT(b.id) files, a.expire_date due_by FROM member_fvr_hdr a LEFT JOIN member_fvr_dtl b ON a.id = b.member_fvr_hdr_id LEFT JOIN member_fvr_history c ON b.id = c.member_fvr_dtl_id WHERE b.member_id = @v0  AND a.active = 1 AND b.active = 1 AND IFNULL(c.confirmed, 0) = 0 GROUP BY id", new object[] { _id });
			return n;
		}
		public member_fvr_hdr(int _id)
		{
			var dr = Toolbox.doSQL_dt(@"SELECT * FROM member_fvr_hdr  WHERE id =@v0 limit 1 ", new object[] { _id }).Rows[0];
			id = _id;
			type = dr["type"].ToString();
			active = Convert.ToInt32(dr["active"]);
			business_unit_ids = dr["business_unit_ids"].ToString();
			tabs_needed = dr["tabs_needed"].ToString();
			expire_date = Convert.ToDateTime(dr["expire_date"]);
			release_date = dr["release_date"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["release_date"]);
			req_member_id = Convert.ToInt32(dr["req_member_id"]);
		}
		public static DataTable get_fvrs(int _member_id)
		{
			return get_fvrs(_member_id, 0);
		}
		public static DataTable get_fvrs(int _member_id, int _header_id)
		{
			var headerAddon = "";
			if (_header_id > 0)
			{
				headerAddon = " AND a.id = " + _header_id;
			}
			var fvr_dt = Toolbox.doSQL_dt(string.Format(@" 
SELECT 
	a.id header_id, 
	b.id detail_id,
	d.tab_index tab_index_pseudo,
	b.tab_index tab_index_actual,
	d.name,
	a.expire_date,
	IFNULL(c.confirmed, 0) confirmed, 
	b.upload_required
FROM 
	member_fvr_hdr a 
LEFT JOIN 
	member_fvr_dtl b ON a.id = b.member_fvr_hdr_id 
LEFT JOIN 
	member_fvr_history c ON b.id = c.member_fvr_dtl_id 
LEFT JOIN 
	member_fvr_tab d ON b.tab_index = d.id 
WHERE 
	b.member_id = @v0  AND 
	a.active = 1 AND 
	b.active = 1 AND
	IFNULL(c.confirmed, 0) = 0 {0}
GROUP BY b.id", headerAddon), new object[] { _member_id, _header_id });
			return fvr_dt;
		}
		public void save()
		{
			if (string.IsNullOrEmpty(type))
			{
				throw new Exception("Type is empty");
			}
			if (string.IsNullOrEmpty(business_unit_ids))
			{
				throw new Exception("Company ID's are empty");
			}
			if (string.IsNullOrEmpty(tabs_needed))
			{
				throw new Exception("Tabs Needed are empty");
			}
			#region new
			if (id == 0)
			{
				id = Toolbox.doSQL_return_id(@" INSERT INTO member_fvr_hdr ( type, active, business_unit_ids, tabs_needed, req_member_id, expire_date, release_date ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4, @v5, @v6  ) ", new object[] { type, active, business_unit_ids, tabs_needed, req_member_id, expire_date, release_date });
			}
			#endregion new
			#region edit
			else
			{
				Toolbox.doSQL_void(@" UPDATE member_fvr_hdr SET type = @v0 , active = @v1 , business_unit_ids = @v2 , tabs_needed = @v3, expire_date = @v5, release_date = @v6  WHERE id = @v4  LIMIT 1", new object[] { type, active, business_unit_ids, tabs_needed, id, expire_date, release_date });
			}
			#endregion edit
		}
	}
	public class member_fvr_dtl
	{
		public int id { get; set; }
		public DateTime dt_insert { get; set; }
		public int member_fvr_hdr_id { get; set; }
		public int member_id { get; set; }
		public string url { get; set; }
		public int tab_index_pseudo { get; set; }
		public int tab_index_actual { get; set; }
		public int file_id { get; set; }
		public int active { get; set; }
		public int upload_required { get; set; }
		public int uploaded_file_id { get; set; }
		public member_fvr_dtl()
		{
		}
		public member_fvr_dtl(int _id)
		{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM member_fvr_dtl  WHERE id =@v0 limit 1 ", new object[] { _id });
			if (dt.Rows.Count > 0)
			{
				var dr = dt.Rows[0];
				load(dr);
			}
		}
		public member_fvr_dtl(int _hdr_id, int _tab_index, int _member_id)
		{
			var dt = Toolbox.doSQL_dt(@"SELECT a.* FROM member_fvr_dtl a LEFT JOIN member_fvr_tab b ON a.tab_index = b.id  WHERE a.member_fvr_hdr_id =@v0 AND b.tab_index =@v1  AND a.member_id =@v2  limit 1 ", new object[] { _hdr_id, _tab_index, _member_id });
			if (dt.Rows.Count > 0)
			{
				var dr = dt.Rows[0];
				load(dr);
			}
		}
		private void load(DataRow dr)
		{
			id = Convert.ToInt32(dr["id"]);
			member_fvr_hdr_id = Convert.ToInt32(dr["member_fvr_hdr_id"]);
			dt_insert = Convert.ToDateTime(dr["dt_insert"]);
			member_id = Convert.ToInt32(dr["member_id"]);
			tab_index_actual = Convert.ToInt32(dr["tab_index"]);
			tab_index_pseudo = Toolbox.doSQL_int("SELECT tab_index FROM member_fvr_tab WHERE id = @v0", new object[] { tab_index_actual });
			url = dr["url"] == DBNull.Value ? "" : dr["url"].ToString();
			file_id = Convert.ToInt32(dr["file_id"]);
			active = Convert.ToInt32(dr["active"]);
			upload_required = Convert.ToInt32(dr["upload_required"]);
			uploaded_file_id = Convert.ToInt32(dr["uploaded_file_id"]);
		}
		public void mass_insert(ArrayList dtls)
		{
			var sql = new StringBuilder();
			sql.Append(@"
INSERT INTO	member_fvr_dtl
	(
	dt_insert,
	member_fvr_hdr_id,
	member_id,
	tab_index,
	file_id,
	active,
	upload_required,
	uploaded_file_id,
	url
	)
VALUES
	");
			List<object> objectParams = new List<object>();
			int count = 0;
			foreach (member_fvr_dtl i in dtls)
			{
				sql.AppendFormat("(NOW(),@v{0},@v{1},@v{2},@v{3}, 1, @v{4}, @v{5}, @v{6}),",
					count * 7 + 0, // {0}
					count * 7 + 1, // {1}
					count * 7 + 2, // {2}
					count * 7 + 3, // {3}
					count * 7 + 4, // {4}
					count * 7 + 5, // {5}
					count * 7 + 6  // {6}
					);
				count++;
				objectParams.Add(i.member_fvr_hdr_id);         // {0}
				objectParams.Add(i.member_id);                 // {1}
				objectParams.Add(i.tab_index_actual);                 // {2}
				objectParams.Add(i.file_id);                   // {3}
				objectParams.Add(i.upload_required);           // {4}
				objectParams.Add(i.uploaded_file_id);          // {5}
				objectParams.Add(i.url);                       // {6}
			}
			Toolbox.doSQL_void(sql.ToString().TrimEnd(','), objectParams.ToArray());
		}
		public void save()
		{
			if (member_fvr_hdr_id == 0)
			{
				throw new Exception("Header ID not set");
			}
			if (member_id == 0)
			{
				throw new Exception("Member ID not set");
			}
			if (file_id == 0)
			{
				//throw new Exception("File ID not set");
			}
			#region new
			if (id == 0)
			{
				Toolbox.doSQL_void(@" INSERT INTO member_fvr_dtl ( dt_insert, member_fvr_hdr_id, member_id, tab_index, file_id, active, upload_required, uploaded_file_id, url ) VALUES ( NOW(), @v0 , @v1 , @v2 , @v3 , 1, @v4 , @v5 , @v6, @v7  ) ", new object[] { member_fvr_hdr_id, member_id, tab_index_actual, file_id, upload_required, uploaded_file_id, url });
			}
			#endregion new
			#region edit
			else
			{
				Toolbox.doSQL_void(@" UPDATE member_fvr_dtl SET file_id = @v0 , active = @v2 , upload_required = @v3 , uploaded_file_id = @v4 , url = @v5  WHERE id = @v1  LIMIT 1", new object[] { file_id, id, active, upload_required, uploaded_file_id, url });
			}
			#endregion edit
		}
		public bool is_complete()
		{
			var c = Toolbox.doSQL_int(@"SELECT COUNT(a.id) FROM member_fvr_dtl a LEFT JOIN member_fvr_history b ON a.id = b.member_fvr_dtl_id WHERE a.member_fvr_hdr_id = @v0  AND a.member_id = @v1  AND IFNULL(b.confirmed, 0) = 0", new object[] { member_fvr_hdr_id, member_id });
			var complete = c == 0;
			return complete;
		}
		public void do_complete()
		{
			Toolbox.doSQL_void(@"UPDATE member_fvr_dtl SET active = 0 WHERE member_fvr_hdr_id = @v0  AND member_id = @v1 ", new object[] { member_fvr_hdr_id, member_id });
		}
	}
	public class member_fvr_history
	{
		public int id { get; set; }
		public int member_fvr_dtl_id { get; set; }
		public DateTime ts { get; set; }
		public int confirmed { get; set; }
		public int file_uploaded { get; set; }
		public member_fvr_history()
		{
		}
		public member_fvr_history(int _member_fvr_dtl_id)
		{
			var count = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member_fvr_history WHERE member_fvr_dtl_id = @v0  LIMIT 1", new object[] { _member_fvr_dtl_id });
			if (count > 0)
			{
				// Load
				var dr = Toolbox.doSQL_dt(@"SELECT * FROM member_fvr_history WHERE member_fvr_dtl_id = @v0  LIMIT 1", new object[] { _member_fvr_dtl_id }).Rows[0];
				id = Convert.ToInt32(dr["id"]);
				member_fvr_dtl_id = _member_fvr_dtl_id;
				confirmed = Convert.ToInt32(dr["confirmed"]);
				file_uploaded = Convert.ToInt32(dr["file_uploaded"]);
				ts = Convert.ToDateTime(dr["ts"]);
			}
			else
			{
				member_fvr_dtl_id = _member_fvr_dtl_id;
			}
		}
		public DataTable history(int _member_fvr_hdr_id, int _member_id)
		{
			return Toolbox.doSQL_dt(@" SELECT d.tab_index, IFNULL(c.confirmed, 0) confirmed, IFNULL(c.file_uploaded, 0) file_uploaded FROM member_fvr_hdr a LEFT JOIN member_fvr_dtl b ON a.id = b.member_fvr_hdr_id LEFT JOIN member_fvr_history c ON b.id = c.member_fvr_dtl_id LEFT JOIN member_fvr_tab d ON b.tab_index = d.id WHERE a.id = @v0  AND b.member_id = @v1 ", new object[] { _member_fvr_hdr_id, _member_id });
		}
		public void save()
		{
			if (id == 0)
			{
				Toolbox.doSQL_void(@" INSERT INTO member_fvr_history ( member_fvr_dtl_id, ts, confirmed, file_uploaded ) VALUES ( @v0 , NOW(), @v1 , @v2  ) ", new object[] { member_fvr_dtl_id, confirmed, file_uploaded });
			}
			else
			{
				Toolbox.doSQL_void(@" UPDATE member_fvr_history SET confirmed = @v0 , ts = NOW(), file_uploaded = @v2  WHERE id = @v1  LIMIT 1", new object[] { confirmed, id, file_uploaded });
			}
		}
	}
	public class member_fvr_category
	{
		public int id { get; set; }
		public DateTime ts { get; set; }
		public string type { get; set; }
		public int tab_index { get; set; }
		public string name { get; set; }
		public int default_file_id { get; set; }
		public bool upload_required { get; set; }
		public member_fvr_category() { }
		public member_fvr_category(int _id)
		{
			load(_id);
		}
		public member_fvr_category(string _tab, int _typeid)
		{
			var _id = Toolbox.doSQL_int(@"SELECT IFNULL(id, 0) FROM member_fvr_tab WHERE name = @v0  AND type = @v1  LIMIT 1", new object[] { _tab, member_fvr_hdr.types.ids[_typeid] });
			if (_id != 0)
			{
				load(_id);
			}
		}
		private void load(int _id)
		{
			if (_id > 0)
			{
				var _dt = Toolbox.doSQL_dt(@"SELECT * FROM member_fvr_tab WHERE id = @v0  LIMIT 1", new object[] { _id });
				if (_dt.Rows.Count == 0)
				{
					throw new Exception("Record doesn't exist");
				}
				var dr = _dt.Rows[0];
				id = _id;
				ts = Convert.ToDateTime(dr["ts"]);
				type = dr["type"].ToString();
				tab_index = Convert.ToInt32(dr["tab_index"]);
				name = dr["name"].ToString();
				default_file_id = dr["default_file_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["default_file_id"]);
				upload_required = Convert.ToBoolean(dr["upload_required"]);
			}
		}
		public void save()
		{
			var is_new = id == 0;
			if (is_new)
			{
				//insert - Not yet a possibility.
			}
			else
			{
				// update
				var file_id = default_file_id == 0 ? "NULL" : default_file_id.ToString();
				Toolbox.doSQL_void(@" UPDATE member_fvr_tab SET name = @v0 , default_file_id = @v1 , upload_required = @v2  WHERE id = @v3  LIMIT 1", new object[] { name, file_id, upload_required, id });
			}
		}
	}
	public static class member_fvr_template
	{
		public class header
		{
			public int id { get; set; }
			public string name { get; set; }
			public int type_id { get; set; }
			public int created_member_id { get; set; }
			public int days_till_expire { get; set; }
			public DateTime created_dt { get; set; }
			public ArrayList dtls { get; set; }
			public List<int> tabs_needed { get; set; }

			public header() { }
			public header(int _id)
			{
				if (exists(_id))
				{
					dtls = new ArrayList();
					tabs_needed = new List<int>();
					load(_id);
				}
			}
			public bool exists(int _id)
			{
				return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member_fvr_template_header WHERE id = @v0  ", new object[] { _id }) > 0;
			}
			private void load(int _id)
			{
				var dr = Toolbox.doSQL_dt(@"SELECT * FROM member_fvr_template_header WHERE id = @v0  LIMIT 1", new object[] { _id }).Rows[0];
				id = _id;
				name = Toolbox.ReturnBlankIfNull_string(dr["name"]);
				type_id = Toolbox.ReturnZeroIfNull_int(dr["type_id"]);
				created_member_id = Toolbox.ReturnZeroIfNull_int(dr["created_member_id"]);
				created_dt = Toolbox.ReturnBlankDateTimeIfNull(dr["created_dt"]);
				days_till_expire = Toolbox.ReturnZeroIfNull_int(dr["days_till_expire"]);
				var dt = Toolbox.doSQL_dt(@"SELECT * FROM member_fvr_template_detail WHERE hdr_id = @v0 ", new object[] { _id });
				foreach (DataRow dtl_dr in dt.Rows)
				{
					var tabpage_id = Convert.ToInt32(dtl_dr["page_id"]);
					if (!tabs_needed.Contains(tabpage_id))
					{
						tabs_needed.Add(tabpage_id);
					}
					var dtl = new detail();
					dtl.load_fromdr(dtl_dr);
					dtls.Add(dtl);
				}
			}
			public void save()
			{
				if (id == 0)
				{
					// Insert
					id = Toolbox.doSQL_return_id(@" INSERT INTO member_fvr_template_header ( name, type_id, created_member_id, created_dt, days_till_expire ) VALUES ( @v0 , @v1 , @v2 , NOW(), @v3 ) ", new object[] { name, type_id, created_member_id, days_till_expire });
				}
				else
				{
					// Update
					Toolbox.doSQL_void(@" UPDATE member_fvr_template_header SET name = @v0, days_till_expire = @v2  WHERE id = @v1  LIMIT 1 ", new object[] { name, id, days_till_expire });
				}
			}
		}
		public class detail
		{
			public int id { get; set; }
			public int hdr_id { get; set; }
			public int page_id { get; set; }
			public int file_id { get; set; }
			public string url { get; set; }
			public bool upload_required { get; set; }
			public bool is_selected { get; set; }

			public detail() { }
			public detail(int _id)
			{
				if (exists(_id))
				{
					load(_id);
				}
			}
			public bool exists(int _id)
			{
				return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member_fvr_template_detail WHERE id = @v0  ", new object[] { _id }) > 0;
			}
			public void load_fromdr(DataRow dr)
			{
				id = Toolbox.ReturnZeroIfNull_int(dr["id"]);
				hdr_id = Toolbox.ReturnZeroIfNull_int(dr["hdr_id"]);
				page_id = Toolbox.ReturnZeroIfNull_int(dr["page_id"]);
				file_id = Toolbox.ReturnZeroIfNull_int(dr["file_id"]);
				url = Toolbox.ReturnBlankIfNull_string(dr["url"]);
				upload_required = Convert.ToBoolean(dr["upload_required"]);
				is_selected = Convert.ToBoolean(dr["is_selected"]);
			}
			private void load(int _id)
			{
				var dr = Toolbox.doSQL_dt(@"SELECT * FROM member_fvr_template_detail WHERE id = @v0  LIMIT 1", new object[] { _id }).Rows[0];
				load_fromdr(dr);
			}
			public void save()
			{
				if (id == 0)
				{
					// Insert
					id = Toolbox.doSQL_return_id(@" INSERT INTO member_fvr_template_detail ( hdr_id, page_id, file_id, url, upload_required, is_selected ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 , @v5  ) ", new object[] { hdr_id, page_id, file_id, url, upload_required ? 1 : 0, is_selected ? 1 : 0 });
				}
				else
				{
					// Update
					Toolbox.doSQL_void(@" UPDATE member_fvr_template_detail SET file_id = @v0 , url = @v1 , upload_required = @v2 , is_selected = @v3  WHERE id = @v4  LIMIT 1 ", new object[] { file_id, url, upload_required ? 1 : 0, is_selected ? 1 : 0, id });
				}
			}
		}
	}
	public class ne_session
	{
		public int id { get; set; }
		public string ip { get; set; }
		public int member_id { get; set; }
		public bool is_active { get; set; }
		public bool is_contact { get; set; }
		public DateTime dt_init { get; set; }
		public DateTime ts { get; set; }
		public int res_x { get; set; }
		public int res_y { get; set; }
		public static void snap_collection(object page_id, string var_name, int var_size, int dt_rows, int dt_columns)
		{
			Toolbox.doSQL_void(@"INSERT INTO session_collection (page_id, var_name, var_size, dt_rows, dt_columns) VALUES (@v0 , @v1 , @v2 , @v3 , @v4 )", new object[] { page_id, var_name, var_size, dt_rows, dt_columns });
		}
		public static void clear_active_sessions(object member_id)
		{
			Toolbox.doSQL_void(@"UPDATE ne_session SET is_active = FALSE WHERE member_id = @v0 ", new object[] { member_id });
		}
		public ne_session() { }
		public ne_session(int _id)
		{
			init(_id);
		}
		private void init(int _id)
		{
			var dr = Toolbox.doSQL_dt(@"SELECT * FROM ne_session WHERE id = @v0 ", new object[] { _id }).Rows[0];
			id = _id;
			ip = dr["ip"].ToString();
			member_id = Convert.ToInt32(dr["member_id"]);
			is_active = Convert.ToBoolean(dr["is_active"]);
			is_contact = Convert.ToBoolean(dr["is_contact"]);
			dt_init = Convert.ToDateTime(dr["dt_init"]);
			ts = Convert.ToDateTime(dr["ts"]);
			res_x = Toolbox.ReturnZeroIfNull_int(dr["res_x"]);
			res_y = Toolbox.ReturnZeroIfNull_int(dr["res_y"]);
		}
		public void save()
		{
			if (id == 0)
			{
				id = Toolbox.doSQL_return_id(@" INSERT INTO ne_session ( ip, member_id, is_active, is_contact, dt_init, res_x, res_y ) VALUES ( @v0 , @v1 , @v2 , @v3 , NOW(), @v4 , @v5  )", new object[] { ip, member_id, is_active, is_contact, res_x, res_y });
			}
			else
			{
				Toolbox.doSQL_void(@" UPDATE ne_session SET ip = @v0 , is_active = @v1 , is_contact = @v2 , res_x = @v3 , res_y = @v4  WHERE id = @v5  LIMIT 1", new object[] { ip, is_active, is_contact, res_x, res_y, id });
			}
		}
	}
	public class emp_checks
	{
		public bool new_welcome { get; set; }
		public bool new_information { get; set; }
		public bool new_handbook { get; set; }
		public bool new_president { get; set; }
		public bool new_hire_acknowledgement { get; set; }
		public bool new_truck { get; set; }
		public bool new_contact_list { get; set; }
		public bool new_confidentiality { get; set; }
		public bool new_qualifications { get; set; }
		public bool new_training { get; set; }
		public bool new_finish { get; set; }

		public bool renew_welcome { get; set; }
		public bool renew_information { get; set; }
		public bool renew_handbook { get; set; }
		public bool renew_president { get; set; }
		public bool renew_trademarks { get; set; }
		public bool renew_truck { get; set; }
		public bool renew_contact_list { get; set; }
		public bool renew_code { get; set; }
		public bool renew_confidentiality { get; set; }
		public bool renew_safety { get; set; }
		public bool renew_survey { get; set; }
		public bool renew_qualifications { get; set; }
		public bool renew_training { get; set; }
		public bool renew_finish { get; set; }

		public bool exit_interview { get; set; }
		public bool exit_checklist { get; set; }
		public bool exit_forwarding { get; set; }
		public bool exit_notification { get; set; }
		public bool exit_finalreview { get; set; }

		public int active_step { get; set; }
		public void populate_session(Page p, ref emp_checks e)
		{
			if (p.Session["emp_checks"] != null)
			{
				e = (emp_checks)p.Session["emp_checks"];
			}
			else
			{
				p.Session["emp_checks"] = e;
				e = (emp_checks)p.Session["emp_checks"];
			}
		}
	}
}