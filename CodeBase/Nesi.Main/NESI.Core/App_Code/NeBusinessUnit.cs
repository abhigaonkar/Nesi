using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using DevExpress.Xpo;
using MySql.Data.MySqlClient;
using DateTime = System.DateTime;
using NESI.Common.Models;

//using nesi.bv;

namespace nesi.core.security
{
    public class ImpersonateUser : IDisposable
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        private IntPtr userHandle = IntPtr.Zero;
        private WindowsImpersonationContext impersonationContext;

        public ImpersonateUser(string user, string domain, string password)
        {
            if (!string.IsNullOrEmpty(user))
            {
                // Call LogonUser to get a token for the user
                var loggedOn = LogonUser(user, domain, password,
                        9 /*(int)LogonType.LOGON32_LOGON_NEW_CREDENTIALS*/,
                        3 /*(int)LogonProvider.LOGON32_PROVIDER_WINNT50*/,
                        out userHandle);
                if (!loggedOn)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                // Begin impersonating the user
                impersonationContext = WindowsIdentity.Impersonate(userHandle);
            }
        }

        public void Dispose()
        {
            if (userHandle != IntPtr.Zero)
                CloseHandle(userHandle);
            if (impersonationContext != null)
                impersonationContext.Undo();
        }
    }
}

namespace nesi.core
{
    /// <summary>
    /// Summary description for NeBusinessUnit
    /// </summary>
    [Serializable]
    public class NeBusinessUnit
    {
        #region Method Accessors
        public NeMember regional_manager
        {
            get
            {
                try
                {
                    return new NeMember(Toolbox.doSQL_int(@"SELECT member_id FROM member WHERE business_unit_id = (SELECT id FROM business_unit WHERE region = @v0  AND isregional_branch = 'True') AND member_membertype_id = 29 AND member_status = 'Active' LIMIT 1", new object[] { region }));
                }
                catch
                {
                    return new NeMember();
                }

            }
        }
        public NeMember branch_manager
        {
            get
            {
                try
                {
                    return new NeMember(Toolbox.doSQL_int(@"select get_bm(@v0 )", new object[] { id }));
                }
                catch
                {
                    return new NeMember();
                }

            }
        }
        public NeMember purchaser
        {
            get
            {
                {


                    var m_id = 0;
                    if (id > 0)
                    {
                        //acting purchaser
                        m_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(acting_purchaser), 0) FROM business_unit WHERE id= @v0 ", new object[] { id });
                    }
                    if (m_id == 0)
                    {
                        //acting manager
                        m_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(acting_manager), 0) FROM business_unit WHERE id= @v0 ", new object[] { id });
                    }
                    if (m_id == 0)
                    {
                        //branch manager
                        m_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(member_id), 0) FROM member WHERE business_unit_id = @v0  AND member_status = 'Active' AND member_membertype_id = 5", new object[] { warehouse_bu_id });

                    }
                    if (m_id == 0)
                    {
                        //purchaser
                        m_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(member_id), @v1) FROM member WHERE business_unit_id = @v0  AND member_status = 'Active' AND member_membertype_id = 9", new object[] { warehouse_bu_id, branch_manager.id });
                    }
                    return new NeMember(m_id);
                }
            }
        }
        #endregion
        #region Shortcut Accessors


        public int id32 { get { return Convert.ToInt32(id); } }
        public string labour_labor
        {
            get { return country.Equals("USA") ? "labor" : "labour"; }
        }
        /// <summary>
        /// Provides the Company's Country (USA / CDN)
        /// </summary>
        public string country
        {
            get; set;
        }
        public string city
        {
            get { return City; }
        }
        public string postal
        {
            get;
            set;
        }
        public string provstate
        {
            get { return Prov; }
            set { Prov = value; }
        }
        public string address
        {
            get { return Add; }
        }

        #endregion

        public int id { get; set; }
        public string name { get; set; }
        public int TaxLabour { get; set; }
        public int TaxMaterial { get; set; }
        public int TaxQuotedJobs { get; set; }
        public int tax_material_only_wos { get; set; }
        public int selfassess_tax { get; set; }
        public string description { get; set; }
        public string Add { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
        public string Prov { get; set; }

        public string State { get; set; }

        public List<int> internal_cust_nos { get; set; }
        public string IsTest { get; set; }
        public string DSN { get; set; }



        public string PhoneNumber { get; set; }
        public string PhoneArea { get; set; }
        public string PhoneFirst { get; set; }
        public string PhoneLast { get; set; }
        public string PhoneAltArea { get; set; }
        public string PhoneAltFirst { get; set; }
        public string POpath { get; set; }
        public string PhoneAltLast { get; set; }
        public string FaxArea { get; set; }
        public string FaxFirst { get; set; }
        public string FaxLast { get; set; }
        public string FaxNumber { get; set; }
        public string member_id { get; set; }
        public DateTime _DateTime { get; set; }
        public string Ord { get; set; }
        public string WOPath { get; set; }
        public string PathTimesheet { get; set; }

        public int Tax1 { get; set; }
        public int Tax2 { get; set; }
        public int Tax3 { get; set; }
        public int Tax4 { get; set; }
        public string revenue { get; set; }



        public string WorkOrderPrinter { get; set; }
        public string LaserPrinter { get; set; }
        public string BarCodePrinter { get; set; }
        public bool Active { get; set; }
        public string BusinessNumber { get; set; }
        public string UIA { get; set; }
        public bool isregional_branch { get; set; }
        public string region { get; set; }
        public int fiscal_yearstart_month { get; set; }
        public int fiscal_current_year { get; set; }
        public int yearend_month { get; set; }
        public DateTime fiscal_start_previous { get; set; }
        public DateTime fiscal_end_previous { get; set; }
        public DateTime fiscal_start_current { get; set; }
        public DateTime fiscal_end_current { get; set; }
        public bool fvr_lockout { get; set; }
        public double perdiem_rate { get; set; }
        public int uses_quote_process { get; set; }
        public double quote_level_2_start { get; set; }
        public double quote_level_3_start { get; set; }
        public string default_material_sell_gl { get; set; }
        public string default_labour_sell_gl { get; set; }
        public bool allow_open_scheduling { get; set; }
        public int default_currency { get; set; }
        public int tax_entity_id { get; set; }
        public int warehouse_bu_id { get; set; }
        public string ddl_name { get; set; }
        public string gl_div { get; set; }
        public string old_dsn { get; set; }
        public string old_div { get; set; }
        public int old_company_id { get; set; }
        public bool is_er { get; set; }
        public bool is_panelshop { get; set; }
        public bool is_corporate { get; set; }
        public string logo_file { get; set; }
        public bool allow_unlinked_timesheet { get; set; }
        public string default_onboarding_email { get; set; }
        public string default_fvr_template_ids { get; set; }
        public int acting_manager { get; set; }
        public int acting_purchaser { get; set; }
        public int acting_right_hand { get; set; }
        public int acting_safety_officer { get; set; }
        public bool is_backoffice { get; set; }
        public string web_domain { get; set; }
        public string header_color { get; set; }
        public string contractor_license { get; set; }
        public string masters_license { get; set; }
        public string ESA_id { get; set; }
        public string TSSA_id { get; set; }
        public string CSA_id { get; set; }
        public bool allow_fixed_labour { get; set; }
        public bool allow_fixed_markup { get; set; }
        public bool allow_mixed_chargeouts { get; set; }
        public bool allow_cell_edit { get; set; }
        public bool allow_email_edit { get; set; }
        public string remit_to_address { get; set; }
        public string NetSuiteInternalId { get; set; }
	    public string CustomerRequestEmail { get; set; }
	    public string VendorRequestEmail { get; set; }
        public bool ShowCustomBillingColumns { get; set; }
        public bool summarize_labor { get; set; }
        public string po_billing_address { get; set; }
        public string RebrandMemo { get; set; }
		public decimal EditTolerance { get; set; }
        public string legal_name { get; set; }
		public bool AllowMobilePartManagement { get; set;}
		public bool AllowPartialBilling { get; set;}
        public bool allow_bankedpay { get; set; }
        public bool allow_vac_withd { get; set; }
        public bool EnablePrevailingWages { get; set; }
        public bool UsesESACheck { get; set; }
        public NeBusinessUnit() { }

    public static string GetbuWOPrinter(int buid)
        {
            return Toolbox.doSQL_string(@"SELECT workorderprinter FROM business_unit WHERE id = @v0 ", new object[] { buid });
        }
        public static ArrayList units_with_timesheet()
        {
            var dt = Toolbox.doSQL_dt(string.Format(@"SELECT distinct id id, ddl_name name FROM business_unit inner join 
membertime on membertime.business_unit_id = business_unit.id  where id in  ({0}) ", new Current_User().visible_business_units), null);
            var list = new ArrayList();
            NeBusinessUnit c;
            foreach (DataRow dr in dt.Rows)
            {
                c = new NeBusinessUnit
                {
                    id = Convert.ToInt32(dr["id"]),
                    name = dr["name"].ToString()
                };
                list.Add(c);
            }
            return list;
        }

        public static void CheckBUProcessFolderStructure(int _buId)
        {
            var BU = new NeBusinessUnit(_buId);

            if (!string.IsNullOrEmpty(BU.WOPath) && !string.IsNullOrEmpty(BU.POpath))
            {
                //  if(BU.WOPath == "" || BU.POpath == "")
                //{
                //   // Should probably add a to-do item here... not sure to whom though
                //   return;
                //}
                // Check Base path
                var baseServer = new Uri(BU.WOPath).Host;

                if (!string.IsNullOrEmpty(baseServer) || BU.WOPath != null && BU.WOPath.Length > 2 && BU.WOPath[1] == ':')
                {
                    try
                    {
                        if (CheckURLPath(baseServer) || BU.WOPath != null && BU.WOPath.Length > 2 && BU.WOPath[1] == ':')
                        {
                            if (!NeFiles.DirectoryAccessible(BU.WOPath)) // Work Orders
                            {
                                Directory.CreateDirectory(BU.WOPath);
                            }
                       //     if (!NeFiles.DirectoryAccessible(BU.WOPath + @"\WorkPro\"))
                        //    {
                        //        Directory.CreateDirectory(BU.WOPath + @"\WorkPro\");
                        //    }
                            if (!NeFiles.DirectoryAccessible(BU.POpath)) // Packing Slips
                            {
                                Directory.CreateDirectory(BU.POpath);
                            }
                            if (!NeFiles.DirectoryAccessible(BU.POpath + @"\LinkedPackingSlips\")) // LinkedPackingSlips
                            {
                                Directory.CreateDirectory(BU.POpath + @"\LinkedPackingSlips\");
                            }
                            NeWOProg.ValidateTEStructureInvoiceFolder(_buId);
                        }

                    }
                    catch(Exception ee)
                    {
						Toolbox.do_errorLog(ee, "Error checking BU folder structure");
                        return;
                    }

                }
            }

            return;
        }


        public static bool CheckURLPath(string Server)
        {
			if(Server == "") return true; // MH: Adding contigency for local development
            var pinger = new Ping();
            try
            {
                if (pinger.Send(Server, 1000).Status != IPStatus.Success)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }



        public static ArrayList units_filtered(string filter)
        {
            if (filter == "")
            {
                throw new Exception("There is an error with the visible business units - none were returned for your account");
            }
            var dt = Toolbox.doSQL_dt(string.Format(@"SELECT id id, ddl_name name FROM business_unit  where id in  ({0}) order by ddl_name ", filter), null);
            var list = new ArrayList();
            NeBusinessUnit c;
            foreach (DataRow dr in dt.Rows)
            {
                c = new NeBusinessUnit();
                c.id = (int)dr["id"];
                c.name = dr["name"].ToString();
                list.Add(c);
            }
            return list;
        }
        /// <summary>
        /// Returns a CSV list that allows you to FIND_IN_SET in MySQL
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static string associated_business_unit_ids(int _id)
        {
            using (var uow = new UnitOfWork())
            {
                var oldCompanyId = (from o in new XPQuery<ne_xpo.cs.business_unit>(uow) where o.id == _id select o.old_company_id).FirstOrDefault();
                if (oldCompanyId == 0)
                {
                    return "";
                }
                var ids = (from x in new XPQuery<ne_xpo.cs.business_unit>(uow)
                           where x.old_company_id == oldCompanyId
                           select x.id).ToList();
                return string.Join(",", ids.Select(_i => _i.ToString()).ToArray());
            }
        }
        /// <summary>
        /// Retrive list of BUs in CSV format that are associated with warehouse BUs.
        /// </summary>

        public static string warehouse_associated_business_unit_ids(int _warehouse_business_unit_id)
        {
            using (var uow = new UnitOfWork())
            {
                var wid = (from o in new XPQuery<ne_xpo.cs.business_unit>(uow) where o.warehouse_bu_id == _warehouse_business_unit_id select o.warehouse_bu_id).FirstOrDefault();
                if (wid == 0)
                {
                    return "";
                }
                var ids = (from x in new XPQuery<ne_xpo.cs.business_unit>(uow)
                           where x.warehouse_bu_id == wid
                           select x.id).ToList();
                return string.Join(",", ids.Select(_i => _i.ToString()).ToArray());
            }
        }

        public static ArrayList units_active()
        {
            var dt = Toolbox.doSQL_dt(@"SELECT id id, ddl_name name FROM business_unit  WHERE active = 'T'", null);
            var list = new ArrayList();
            NeBusinessUnit c;
            foreach (DataRow dr in dt.Rows)
            {
                c = new NeBusinessUnit();
                c.id = (int)dr["id"];
                c.name = dr["name"].ToString();
                list.Add(c);
            }
            return list;
        }

        public string GetBUDSN(int buid)
        {
            return Toolbox.doSQL_string(@"Select ifnull((Select dsn FROM tax_entity a inner join business_unit b on b.tax_entity_id = a.id WHERE b.id = @v0 ),'')", new object[] { buid });
        }

        public static bool SubsidiaryExists(MySqlConnection conn, int subsidiaryId)
            {
            return Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM business_unit WHERE netsuite_bu_internal_id = @v0 LIMIT 1", new object[] { subsidiaryId }) > 0;
            }
        public static Dictionary<int, bool> SubsidiaryList(MySqlConnection conn)
            {
            var subList = new Dictionary<int, bool>();
            var dtSubs = Toolbox.doSQL_dt(conn, @"SELECT netsuite_bu_internal_id, IFNULL(active, 'F') active FROM business_unit WHERE netsuite_bu_internal_id IS NOT NULL", new object[]{ });
            foreach(DataRow dr in dtSubs.Rows)
                {
                var subId = Convert.ToInt32(dr["netsuite_bu_internal_id"]);
                var active = (string) dr["active"] == "T";
                subList.Add(subId, active);
                }
            return subList;
            }

    public static bool SubsidiaryActive(MySqlConnection conn, int subsidiaryId)
            {
            return Toolbox.doSQL_int(conn, @"SELECT IF(active = 'T', 1, 0) FROM business_unit WHERE netsuite_bu_internal_id = @v0 LIMIT 1", new object[] { subsidiaryId }) == 1;
            }


        //public static bool IsWaitingRollover(int tax_entity_id)
		//
        //{
		//
        //    return (NeTaxEntity.IsWaitingRollover(tax_entity_id));
        //}
        public string GetBMEmail(int buid)
        {
            var c = new NeBusinessUnit(buid);
            return c.branch_manager.NEEmail;
        }
        public NeBusinessUnit(long buid)
        {
            Load(Convert.ToInt32(buid));
        }
        public NeBusinessUnit(object buid)
        {
            Load(Convert.ToInt32(buid));
        }
        public NeBusinessUnit(string buid)
        {
            Load(Convert.ToInt32(buid));
        }
        public NeBusinessUnit(int buid)
        {
            Load(buid);
        }
        public static int FindFiscalYear(int supplied_year, int supplied_month, int branch_current_year, int branch_startmonth)
        {
            var fy = 0;
            // I'm breaking these up because it's easier to understand.
            if (supplied_year == branch_current_year && supplied_month >= branch_startmonth)
            // Example: Trying to look at Dec, 2014 for a fiscal year that starts in June 2014, would return 2014
            // Example: Trying to look at Dec, 2014 for a fiscal year that starts in Jan 2014, would return 2014
            // Example: Trying to look at Jan, 2014 for a fiscal year that starts in Jan 2014, would return 2014
            {
                fy = supplied_year;
            }
            else if (supplied_year <= branch_current_year && supplied_month < branch_startmonth)
            // Example: Trying to look at Jan, 2015 for a fiscal year that starts in June 2014, would return 2014
            {
                fy = supplied_year - 1;
            }
            else if (supplied_year > branch_current_year && supplied_month < branch_startmonth)
            // Example: Trying to look at Jan, 2015 for a fiscal year that starts in June 2014, would return 2014
            {
                fy = supplied_year - 1;
            }
            else if (supplied_year < branch_current_year && supplied_month >= branch_startmonth)
            // Example: Trying to look at Dec, 2013 for a fiscal year that starts in June 2014, would return 2013
            // Example: Trying to look at Dec, 2014 for a fiscal year that starts in Jan 2015, would return 2014
            {
                fy = supplied_year;
            }
            return fy;
        }
        public static int[][] FiscalMonthLookup = new[]
            {
		                                       		// X axis = Fiscal Year Start Month
		                                       		// Y axis = Actual Month
		                                       		// So if looking up what December translates to a fiscal year that starts in June (X=6, Y=12), the answer is 7
		                                       		//	   00  01  02  03  04  05  06  07  08  09  10  11  12  
		                                       		new[] {00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00}, // 00
		                                       		new[] {00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12}, // 01
		                                       		new[] {00, 12, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11}, // 02
		                                       		new[] {00, 11, 12, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10}, // 03 
		                                       		new[] {00, 10, 11, 12, 01, 02, 03, 04, 05, 06, 07, 08, 09}, // 04 
		                                       		new[] {00, 09, 10, 11, 12, 01, 02, 03, 04, 05, 06, 07, 08}, // 05
		                                       		new[] {00, 08, 09, 10, 11, 12, 01, 02, 03, 04, 05, 06, 07}, // 06
		                                       		new[] {00, 07, 08, 09, 10, 11, 12, 01, 02, 03, 04, 05, 06}, // 07
		                                       		new[] {00, 06, 07, 08, 09, 10, 11, 12, 01, 02, 03, 04, 05}, // 08
		                                       		new[] {00, 05, 06, 07, 08, 09, 10, 11, 12, 01, 02, 03, 04}, // 09
		                                       		new[] {00, 04, 05, 06, 07, 08, 09, 10, 11, 12, 01, 02, 03}, // 10
		                                       		new[] {00, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 01, 02}, // 11
		                                       		new[] {00, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 01}	// 12
		                                       		};
        /// <summary>
        /// For retrieving a zero based array of months that match [fiscal year start][iterator]
        /// <example>FiscalMonthList[6][1] would return 6, FiscalMonthList[6][12] would return 5</example>
        /// </summary>
        public static int[][] FiscalMonthList = new[]
            {
		                                     		// X axis = Fiscal Year Start Month
		                                     		// Y axis = What month 1 actually is.
		                                     		// 
		                                     		//	   00  01  02  03  04  05  06  07  08  09  10  11  12  
		                                     		new[] {00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00}, // 00
		                                     		new[] {00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12}, // 01
		                                     		new[] {00, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 01},	// 02
		                                     		new[] {00, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 01, 02}, // 03
		                                     		new[] {00, 04, 05, 06, 07, 08, 09, 10, 11, 12, 01, 02, 03}, // 04
		                                     		new[] {00, 05, 06, 07, 08, 09, 10, 11, 12, 01, 02, 03, 04}, // 05
		                                     		new[] {00, 06, 07, 08, 09, 10, 11, 12, 01, 02, 03, 04, 05}, // 06
		                                     		new[] {00, 07, 08, 09, 10, 11, 12, 01, 02, 03, 04, 05, 06}, // 07
		                                     		new[] {00, 08, 09, 10, 11, 12, 01, 02, 03, 04, 05, 06, 07}, // 08
		                                     		new[] {00, 09, 10, 11, 12, 01, 02, 03, 04, 05, 06, 07, 08}, // 09
		                                     		new[] {00, 10, 11, 12, 01, 02, 03, 04, 05, 06, 07, 08, 09}, // 10
		                                     		new[] {00, 11, 12, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10}, // 11
		                                     		new[] {00, 12, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11}  // 12

		                                     		};
        private DataTable GetRow(int _id)
            {
            return Toolbox.doSQL_dt(@"
SELECT 
	*,
   (SELECT  IFNULL(GROUP_CONCAT(internal_companyno_bvno), '') FROM internal_companyno WHERE business_unit_id = a.id) internal_company_nos 
FROM 
	business_unit a 
INNER JOIN 
	tax_entity b on b.id = a.tax_entity_id 
WHERE 
	a.id = @v0 ", new object[] { _id });
            }

        private void Load(int _id)
        {
        if(_id == 0) return;
        DataTable _dt;
        var StopBUCaching = shared.properties.exists("stop_bu_caching");
        // MH: HttpContext.Current is null when working from any other type of project other than a Web Page/Application
        if(HttpContext.Current != null && !StopBUCaching)
            {
            // MH: Adding a caching layer to streamline objects that do not change often.
            // If there is ever trouble with this, just add a stop_bu_caching to the properties table.
            var AppState = HttpContext.Current.Application;
            if(AppState["BU"] == null)
                {
                AppState["BU"] = new Dictionary<int, Tuple<DateTime, DataTable>>();
                }
            if(!((Dictionary<int, Tuple<DateTime, DataTable>>)AppState["BU"]).ContainsKey(_id))
                {
                _dt = GetRow(_id);
                var t = new Tuple<DateTime, DataTable>(DateTime.Now, _dt);
                ((Dictionary<int, Tuple<DateTime, DataTable>>)AppState["BU"]).Add(_id, t);
                }
            else
                {
                var t =  ((Dictionary<int, Tuple<DateTime, DataTable>>)AppState["BU"])[_id];
                if(DateTime.Now.Subtract(t.Item1).TotalMinutes > 5)
                    {
                    ((Dictionary<int, Tuple<DateTime, DataTable>>)AppState["BU"]).Remove(_id);
                    _dt = GetRow(_id);
                    ((Dictionary<int, Tuple<DateTime, DataTable>>)AppState["BU"]).Add(_id, new Tuple<DateTime, DataTable>(DateTime.Now, _dt));
                    }
                else
                    {
                    _dt = t.Item2;
                    }
                }
            }
        else
            {
            _dt = GetRow(_id);
            }

        if(_dt.Rows.Count <= 0) return;
        foreach (DataRow _dr in _dt.Rows)
            {
            id             = (int)_dr["id"];
            name           = _dr["name"].ToString();
            ddl_name       = _dr["ddl_name"].ToString();
            description    = _dr["description"].ToString();
            DSN            = _dr["DSN"].ToString(); // from tax_entity table
            BusinessNumber = _dr["tax_id_no"].ToString();
            IsTest         = _dr["IsTest"].ToString();
            internal_cust_nos = _dr["internal_company_nos"].ToString() == "" 
                ? new List<int>() 
                : _dr["internal_company_nos"].ToString().Split(',').Select(int.Parse).ToList();
						
            Add                      = _dr["address"].ToString();
            country                  = _dr["Country"].ToString();
            State                    = _dr["state"].ToString();
            City                     = _dr["City"].ToString();
            Prov                     = country == "CDN" ? _dr["Prov"].ToString() : _dr["state"].ToString();
            postal                   = _dr["postal"].ToString();
            PhoneArea                = _dr["PhoneArea"].ToString();
            PhoneFirst               = _dr["PhoneFirst"].ToString();
            PhoneLast                = _dr["PhoneLast"].ToString();
            PhoneNumber              = $"({PhoneArea}) {PhoneFirst}-{PhoneLast}";
            PhoneAltArea             = _dr["PhoneAltArea"].ToString();
            PhoneAltFirst            = _dr["PhoneAltFirst"].ToString();
            PhoneAltLast             = _dr["PhoneAltLast"].ToString();
            FaxArea                  = _dr["FaxArea"].ToString();
            FaxFirst                 = _dr["FaxFirst"].ToString();
            FaxLast                  = _dr["FaxLast"].ToString();
            FaxNumber                = $"({FaxArea}) {FaxFirst}-{FaxLast}";
            member_id                = _dr["member_id"].ToString();
            _DateTime                = _dr["datetime"] != null && _dr["datetime"] != DBNull.Value ? Convert.ToDateTime(_dr["datetime"]) : DateTime.Now;
            Ord                      = _dr["ord"].ToString();
            WOPath                   = _dr["wopath"].ToString();
            POpath                   = _dr["popath"].ToString();
            PathTimesheet            = _dr["pathtimesheet"].ToString();
            warehouse_bu_id          = Toolbox.ReturnZeroIfNull_int(_dr["warehouse_bu_id"]);
            Tax1                     = Toolbox.ReturnZeroIfNull_int(_dr["tax1"]);
            Tax2                     = Toolbox.ReturnZeroIfNull_int(_dr["tax2"]);
            Tax3                     = Toolbox.ReturnZeroIfNull_int(_dr["tax3"]);
            Tax4                     = Toolbox.ReturnZeroIfNull_int(_dr["tax4"]);
            revenue                  = _dr["revenue"].ToString();
            default_onboarding_email = Toolbox.ReturnBlankIfNull_string(_dr["default_onboarding_email"]).Trim();
            default_fvr_template_ids = Toolbox.ReturnBlankIfNull_string(_dr["default_fvr_template_ids"]);
            WorkOrderPrinter         = _dr["workorderprinter"].ToString();
            LaserPrinter             = _dr["laserprinter"].ToString();
            BarCodePrinter           = _dr["barcodeprinter"].ToString();
            yearend_month            = _dr["YearEnd_Month"] == DBNull.Value ? 12 : Convert.ToInt32(_dr["YearEnd_Month"]); // from tax_entity table
            Active                   = _dr["active"].ToString() == "T";
            isregional_branch        = _dr["isregional_branch"] != DBNull.Value && Convert.ToBoolean(_dr["isregional_branch"]);
            region                   = _dr["region"].ToString();
            TaxLabour                = Convert.ToInt16(_dr["taxlabour"]);
            TaxQuotedJobs            = Convert.ToInt16(_dr["taxquotedjobs"]);
            selfassess_tax           = Convert.ToInt16(_dr["selfassess_tax"]);
            tax_material_only_wos    = Convert.ToInt16(_dr["tax_material_only_wos"]);
            TaxMaterial              = Convert.ToInt16(_dr["taxmaterial"]);
            fvr_lockout              = _dr["fvr_lockout"] != DBNull.Value && (bool)_dr["fvr_lockout"];
            uses_quote_process       = Convert.ToInt32(_dr["uses_quote_process"]);
            allow_open_scheduling    = _dr["allow_open_scheduling"] != DBNull.Value && Convert.ToBoolean(_dr["allow_open_scheduling"]);
            quote_level_2_start      = Convert.ToDouble(_dr["quote_level_2_start"]);
            quote_level_3_start      = Convert.ToDouble(_dr["quote_level_3_start"]);
            default_labour_sell_gl   = _dr["default_labour_sell_gl"].ToString();
            default_material_sell_gl = _dr["default_material_sell_gl"].ToString();
            ShowCustomBillingColumns = (bool) _dr["show_custom_billing_columns"];
            var n = DateTime.Now;

            fiscal_yearstart_month = new DateTime(n.Year, yearend_month, 1).AddMonths(1).Month;
            var fiscal_threshold      = 13 - fiscal_yearstart_month;
            var current_fiscal_month  = FiscalMonthLookup[fiscal_yearstart_month][n.Month];
            var past_fiscal_threshold = current_fiscal_month < 12;
            fiscal_current_year = fiscal_yearstart_month == 1 || n.Month < fiscal_yearstart_month
                ? n.Year
                : n.Year + 1;
            var start_year = fiscal_yearstart_month == 1 ? n.Year : n.Month >= 1 ? n.Year - 1 : n.Year;
            var end_year   = yearend_month < 12 ? n.Month >= fiscal_yearstart_month ? n.Year + 1 : n.Year : start_year;
            fiscal_start_current = new DateTime(start_year, fiscal_yearstart_month, 1);
            fiscal_end_current   = new DateTime(end_year, yearend_month, DateTime.DaysInMonth(end_year, yearend_month));

            fiscal_start_previous    = new DateTime(start_year - 1, fiscal_yearstart_month, 1);
            fiscal_end_previous      = new DateTime(end_year - 1, yearend_month, DateTime.DaysInMonth(end_year - 1, yearend_month));
            perdiem_rate             = Convert.ToDouble(_dr["perdiem_rate"]);
            default_currency         = Toolbox.ReturnDefaultIfNull_int(_dr["default_currency"], 2); //RG changed to default CAD
            tax_entity_id            = Convert.ToInt32(_dr["tax_entity_id"]);
            gl_div                   = Toolbox.ReturnBlankIfNull_string(_dr["gl_div"]);
            old_dsn                  = Toolbox.ReturnBlankIfNull_string(_dr["old_dsn"]);
            old_div                  = Toolbox.ReturnBlankIfNull_string(_dr["old_div"]);
            old_company_id           = Toolbox.ReturnZeroIfNull_int(_dr["old_company_id"]);
            is_er                    = _dr["is_er"] != DBNull.Value && Convert.ToBoolean(_dr["is_er"]);
            is_panelshop             = _dr["is_panelshop"] != DBNull.Value && Convert.ToBoolean(_dr["is_panelshop"]);
            logo_file                = Toolbox.ReturnBlankIfNull_string(_dr["logo_file"]);
            allow_unlinked_timesheet = _dr["allow_unlinked_timesheet"] != DBNull.Value && Convert.ToBoolean(_dr["allow_unlinked_timesheet"]);
            acting_manager           = Toolbox.ReturnZeroIfNull_int(_dr["acting_manager"]);
            acting_right_hand        = Toolbox.ReturnZeroIfNull_int(_dr["acting_right_hand"]);
            acting_purchaser         = Toolbox.ReturnZeroIfNull_int(_dr["acting_purchaser"]);
            acting_safety_officer    = Toolbox.ReturnZeroIfNull_int(_dr["acting_safety_officer"]);
            is_corporate             = _dr["is_corporate"] != DBNull.Value && Convert.ToBoolean(_dr["is_corporate"]);
            is_backoffice            = _dr["is_backoffice"] != DBNull.Value && Convert.ToBoolean(_dr["is_backoffice"]);
            web_domain               = Toolbox.ReturnBlankIfNull_string(_dr["web_domain"]);
                                               if (web_domain == "")
                                                    {
                                                        web_domain = Toolbox.app_setting("DomainForEmail");
                                                    }
            header_color              = Toolbox.ReturnBlankIfNull_string(_dr["header_color"]);
            contractor_license        = Toolbox.ReturnBlankIfNull_string(_dr["contractor_license"]);
            masters_license           = Toolbox.ReturnBlankIfNull_string(_dr["masters_license"]);
            ESA_id                    = Toolbox.ReturnBlankIfNull_string(_dr["ESA_id"]);
            TSSA_id                   = Toolbox.ReturnBlankIfNull_string(_dr["TSSA_id"]);
            CSA_id                    = Toolbox.ReturnBlankIfNull_string(_dr["CSA_id"]);
            allow_fixed_labour        = _dr["allow_fixed_labour"] != DBNull.Value && Convert.ToBoolean(_dr["allow_fixed_labour"]);
            allow_fixed_markup        = _dr["allow_fixed_markup"] != DBNull.Value && Convert.ToBoolean(_dr["allow_fixed_markup"]);
            allow_mixed_chargeouts    = _dr["allow_mixed_chargeouts"] != DBNull.Value && Convert.ToBoolean(_dr["allow_mixed_chargeouts"]);
            allow_email_edit          = _dr["allow_email_edit"] != DBNull.Value && Convert.ToBoolean(_dr["allow_email_edit"]);
            allow_cell_edit           = _dr["allow_cell_edit"] != DBNull.Value && Convert.ToBoolean(_dr["allow_cell_edit"]);
            remit_to_address          = Toolbox.ReturnBlankIfNull_string(_dr["remit_to_address"]);
            NetSuiteInternalId        = Toolbox.ReturnBlankIfNull_string(_dr["netsuite_bu_internal_id"]);
            VendorRequestEmail        = Toolbox.ReturnBlankIfNull_string(_dr["vendor_request_email"]);
            CustomerRequestEmail      = Toolbox.ReturnBlankIfNull_string(_dr["customer_request_email"]);
            summarize_labor           = _dr["summarize_labor"] != DBNull.Value && Convert.ToBoolean(_dr["summarize_labor"]);
            po_billing_address        = Toolbox.ReturnBlankIfNull_string(_dr["po_billing_address"]);
            RebrandMemo               = Toolbox.ReturnBlankIfNull_string(_dr["rebrand_memo"]);
            EditTolerance             = (decimal) _dr["edit_tolerance"];
            legal_name                = Toolbox.ReturnBlankIfNull_string(_dr["legal_name"]);
            AllowMobilePartManagement = Toolbox.ReturnZeroIfNull_int(_dr["allow_mobile_part_management"]) == 1;
            AllowPartialBilling         = Toolbox.ReturnZeroIfNull_int(_dr["allow_partial_billing"]) == 1;
            allow_bankedpay             = Convert.ToBoolean(_dr["allow_bankedpay"]);
            allow_vac_withd             = Convert.ToBoolean(_dr["allow_vac_withd"]);
            EnablePrevailingWages = _dr["enable_prevailing_wages"] != DBNull.Value && Convert.ToBoolean(_dr["enable_prevailing_wages"]);
            UsesESACheck          = Convert.ToBoolean(_dr["uses_esa_check"]);
            }
        }


        public int get_fiscal_year(DateTime curr, NeBusinessUnit c)
        {
            var fy = 0;
            var current_fiscal_month = FiscalMonthLookup[c.fiscal_yearstart_month][curr.Month];
            var past_fiscal_threshold = current_fiscal_month < 12;
            fy = curr.Month >= c.fiscal_yearstart_month || past_fiscal_threshold && curr.Month > c.yearend_month
                ? curr.Year
                : curr.Year - 1;
            return fy;
        }
        public static DataTable get_org_chart(int buid)
        {
            var c = new NeBusinessUnit(buid);
            DataTable dt = null;
            var ceo = Toolbox.doSQL_int(@"Select ifnull((Select member_id from member,business_unit  where member.reports_to = 0 and business_unit.country =@v0 limit 1),0) ", new object[] { c.country });
            if (ceo == 0)
            {
                return null;
            }
            else
            {
                var a = Toolbox.doSQL_dt(@"Select ifnull((Select member_id from member  where member.reports_to =@v0),0) ", new object[] { ceo });
            }
            return dt;
        }

        public static string GetbuCountry(string buid)
        {
            return Toolbox.doSQL_string(@"SELECT country FROM business_unit WHERE id = @v0 ", new object[] { buid });
        }
        public static void EmailBranchPurchaser(int bu_id, string subject, string body, bool include_bm, string pmEmail = null)
        {
            var c = new NeBusinessUnit(bu_id);
            var p = c.purchaser;
            if (p.id > 0)
            {
                var cc = include_bm
                                ? c.branch_manager.NEEmail
                                : "";

                if (!string.IsNullOrWhiteSpace(pmEmail))
                {
                    cc += $";{pmEmail}";
                }
              

                cc += AppConstants.puchasingEmailID;

                var e = new NeEMail();
						
                if (Toolbox.app_setting("debug_redirect") == "1")
                    e.To = Toolbox.app_setting("debug_redirect_email");
                else
                    e.To = c.purchaser.NEEmail;

                    e.From = "noreply@" + Toolbox.app_setting("DomainForEmail");

                e.Subject = subject;
                e.Body = body;
                e.isHTML = true;
                e.CC = cc;

                e.Send();
            }
            else
            {
                throw new Exception("This business unit does not have a purchaser");
            }
        }
    }
}