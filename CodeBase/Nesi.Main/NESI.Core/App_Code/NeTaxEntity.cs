using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.Data.Linq;
using DevExpress.Xpo;
using ne_xpo.cs;
namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeBusinessUnit
	/// </summary>
	[Serializable]
	public class NeTaxEntity
		{
		#region Method Accessors
	
		#endregion
		#region Shortcut Accessors
	
		#endregion
		#region Deprecated Parameters
		
		#endregion
		public int id											{get;set;}
		public string public_name										{get;set;}
		public string region {get;set; }
        public string ddl_name { get; set; }
        public string nesi_nickname { get; set; }
        public string adp_company_code { get; set; }
        public string tax_id_no { get; set; }
        public bool is_test { get; set; }
        public bool is_active { get; set; }
        public string DSN { get; set; }
        public int fiscal_yearstart_month { get; set; }
        public int fiscal_current_year { get; set; }
        public int yearend_month { get; set; }
        public DateTime fiscal_start_previous { get; set; }
        public DateTime fiscal_end_previous { get; set; }
        public DateTime fiscal_start_current { get; set; }
        public DateTime fiscal_end_current { get; set; }
        public DataTable business_units { get; set; }
        public DataTable parent_tes { get; set; }
		public gl_te gl_default_revenue {get; set;}
		public gl_te gl_default_expense {get; set;}
		public bool sync_customers { get; set; }
		public bool sync_vendors { get; set; }
		public bool is_holdco { get; set; }
		public bool uses_folders { get; set; }	
		public bool allow_interbu_ts { get; set;}
		public double default_mileage_rate { get; set; }
		public string default_onboarding_email {get; set;}
		public string default_fvr_template_ids {get; set;}
		public double  markup_formula_min_markup { get; set; }
        public double markup_formula_top { get; set; }
        public double markup_formula_qty_dim { get; set; }
        public double markup_formula_offset { get; set; }
        public double markup_formula_exp { get; set; }
        public double markup_formula_max { get; set; }
		public string BaseCountry { get; set; } // Extrapolated for now
        public int NetSuite_TE_Internal_Id { get; set; }
        public bool IncludeOwnersInDiscEmails { get; set; }
		public decimal EditTolerance { get; set; }
        public decimal vac_freq_1 { get; set; }
        public decimal vac_freq_2 { get; set; }
        public decimal vac_freq_3 { get; set; }
        public decimal vac_amount_1 { get; set; }
        public decimal vac_amount_2 { get; set; }
        public decimal vac_amount_3 { get; set; }

        public int? controller_id { get; set; }

        public string billing_email { get; set; }



        public NeTaxEntity(){}


		    public static string associated_business_unit_id(int _te_id)
		        {
		        using (var uow = new UnitOfWork())
		            {
		            var ids = (from x in new XPQuery<ne_xpo.cs.business_unit>(uow)
		                where x.tax_entity_id == _te_id
		                select x.id).ToList();
		            return string.Join(",", ids.Select(_i => _i.ToString()).ToArray());
		            }
		        }


        public DataTable LoadActiveTaxEntitiesList()
			{
			var dt = Toolbox.doSQL_dt(@"SELECT id, nesi_nickname FROM tax_entity  WHERE is_active = 1 and dsn is not null order by nesi_nickname" , null);
			
			return dt;
			}

		   // public static bool IsWaitingRollover(int tax_entity_id)
		   //
		   //     {
		   //
		   //     var _tools = new Toolbox();
		   //     var last_date = _tools.getSQL_string(@"Select End_of_period_39 from ctrl_periods  where table_no = 6", new NeTaxEntity(tax_entity_id).DSN, null);
		   //     var end_of_final_year = new DateTime(Convert.ToInt32(last_date.Substring(0, 4)), 1, 1).AddDays(Convert.ToInt32(last_date.Substring(4, 3)) - 1);
		   //
		   //     if ((end_of_final_year - DateTime.Today).Days < 365)
		   //         {
		   //         return true;
		   //         }
		   //     return false;
		   //     }

        public ArrayList LoadTax_EntityList()
			{
			var dt = Toolbox.doSQL_dt(@"SELECT id, nesi_nickname name FROM tax_entity order by nesi_nickname"  , null);
			var list = new ArrayList();
            NeTaxEntity c;
            foreach (DataRow dr in dt.Rows)
            {
                c = new NeTaxEntity();
                c.id = Convert.ToInt32(dr["id"]);
                c.public_name = dr["nesi_nickname"].ToString();
                list.Add(c);
            }
            return list;
        }
		public static ArrayList LoadMyTax_EntityList(string filter)
			{
			var dt = Toolbox.doSQL_dt(string.Format(@"SELECT id, nesi_nickname name FROM tax_entity  where id in ({0}) ",  filter ),null);
			var list = new ArrayList();
            NeTaxEntity c;
            foreach (DataRow dr in dt.Rows)
            {
                c = new NeTaxEntity();
                c.id = Convert.ToInt32(dr["id"]);
                c.public_name = dr["nesi_nickname"].ToString();
                list.Add(c);
            }
            return list;
        }

		public string GetDSN(int te_id)
			{
			return Toolbox.doSQL_string(@"Select dsn FROM tax_entity WHERE id = @v0 ),'')", new object[] {  te_id } );
			}

		public NeTaxEntity(object te_id)
			{
			
			load(Convert.ToInt32(te_id));
			}

		public static int FindFiscalYear(int supplied_year, int supplied_month, int branch_current_year, int branch_startmonth)
			{
			var fy		= 0;
			// I'm breaking these up because it's easier to understand.
			if(supplied_year == branch_current_year && supplied_month >= branch_startmonth)			
				// Example: Trying to look at Dec, 2014 for a fiscal year that starts in June 2014, would return 2014
				// Example: Trying to look at Dec, 2014 for a fiscal year that starts in Jan 2014, would return 2014
				// Example: Trying to look at Jan, 2014 for a fiscal year that starts in Jan 2014, would return 2014
				{
				fy		= supplied_year;
				}
			else if(supplied_year <= branch_current_year && supplied_month < branch_startmonth)
				// Example: Trying to look at Jan, 2015 for a fiscal year that starts in June 2014, would return 2014
				{
				fy		= supplied_year - 1;
				}
			else if(supplied_year > branch_current_year && supplied_month < branch_startmonth)
				// Example: Trying to look at Jan, 2015 for a fiscal year that starts in June 2014, would return 2014
				{
				fy		= supplied_year - 1;
				}
			else if(supplied_year < branch_current_year && supplied_month >= branch_startmonth)
				// Example: Trying to look at Dec, 2013 for a fiscal year that starts in June 2014, would return 2013
				// Example: Trying to look at Dec, 2014 for a fiscal year that starts in Jan 2015, would return 2014
				{
				fy		= supplied_year;
				}
			return fy;
			}
		public static int[][] FiscalMonthLookup	= new[]
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
		public static int[][] FiscalMonthList	= new[]
			                                     	  {
		                                     		// X axis = Fiscal Year Start Month
		                                     		// Y axis = What month 1 actually is.
		                                     		// 
		                                     		//		   00  01  02  03  04  05  06  07  08  09  10  11  12  
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
		public bool is_grouped {get; set; }
		public List<tax_entity_group_link> groups {get; set; }
		public static bool CommonGroup(List<int> _group1, List<int> _group2)
			{
			foreach(var i in _group1)
				{
				foreach(var j in _group2)
					{
					if(i == j) return true;
					}
				}
			return false;
			}
		private void load(int teid)
			{
			using (var uow = new UnitOfWork())
				{
				if (teid != 0)
					{
					var te_obj			= uow.GetObjectByKey<tax_entity>(teid);
					if(te_obj == null) return;
					id					= teid;
					public_name			= te_obj.public_name;
					ddl_name			= te_obj.ddl_name;
					nesi_nickname		= te_obj.nesi_nickname;
					tax_id_no			= te_obj.tax_id_no;
					is_test				= te_obj.is_test;
					DSN					= te_obj.dsn;
					region				= te_obj.region;
					adp_company_code	= te_obj.adp_company_code;
					is_active			= te_obj.is_active;
					yearend_month		= te_obj.yearend_month;
					sync_customers		= te_obj.sync_customers;
					sync_vendors		= te_obj.sync_vendors;
					is_holdco			= te_obj.is_holdco;
					uses_folders		= te_obj.uses_folders;
                    markup_formula_exp = te_obj.markup_formula_exp;
                    markup_formula_min_markup = te_obj.markup_formula_min_markup;
                    markup_formula_top = te_obj.markup_formula_top;
                    markup_formula_qty_dim = te_obj.markup_formula_qty_dim;
                    markup_formula_offset = te_obj.markup_formula_offset;
                    markup_formula_max = te_obj.markup_formula_max; 
                     NetSuite_TE_Internal_Id = !string.IsNullOrEmpty(te_obj.netsuite_te_internal_id) && te_obj.netsuite_te_internal_id != "null" ? Convert.ToInt32(te_obj.netsuite_te_internal_id) : 0;
                    IncludeOwnersInDiscEmails = Convert.ToBoolean(te_obj.include_owners_in_disc_emails);
                    vac_freq_1 = te_obj.vac_freq_1;
                    vac_freq_2 = te_obj.vac_freq_2;
                    vac_freq_3 = te_obj.vac_freq_3;
                    vac_amount_1 = te_obj.vac_amount_1;
                    vac_amount_2 = te_obj.vac_amount_2;
                    vac_amount_3 = te_obj.vac_amount_3;
                    billing_email = te_obj.billing_email;
                    controller_id = te_obj.controller_id;
					EditTolerance = te_obj.edit_tolerance;

                    is_grouped			= (from teg in new XPQuery<tax_entity_group_link>(uow)
										  where teg.tax_entity_id ==uow.GetObjectByKey<ne_xpo.cs.tax_entity> (id) 
										   select teg).Any();
					BaseCountry			= (from bu in new XPQuery<business_unit>(uow)
										   where bu.tax_entity_id == id
										   select bu).FirstOrDefault().country;
					if (is_grouped)
					{
						
						groups= (from teg in new XPQuery<tax_entity_group_link>(uow)
								  where teg.tax_entity_id == uow.GetObjectByKey<ne_xpo.cs.tax_entity>(id)
								  select teg).Distinct().ToList();
										 ;
						
						}
					else
						{
						groups			= new List<tax_entity_group_link>();
						}
				allow_interbu_ts = te_obj.allow_interbu_ts;
					default_fvr_template_ids = te_obj.default_fvr_template_ids; 
					default_onboarding_email = te_obj.default_onboarding_email;
				    default_mileage_rate = te_obj.default_mileage_rate;

					if(te_obj.gl_default_expense != null)
						{
						gl_default_expense	= te_obj.gl_default_expense;
						}
					if(te_obj.gl_default_revenue != null)
						{
						gl_default_revenue		= te_obj.gl_default_revenue;
						}

					// TODO: Move these datatables to lists generated by linq queries using XPO
                    //associated_business_unit_ids = 

					business_units = Toolbox.doSQL_dt(@"Select id,name,gl_div,old_div,old_dsn from business_unit  where tax_entity_id =@v0", new object[] { id });
					parent_tes = Toolbox.doSQL_dt(@"Select tax_entity.id,nesi_nickname from tax_entity inner join tax_entity_hiearchy teh on teh.tax_entity_id = tax_entity.id ", null);

					if (yearend_month > 0)
						{
						var n = DateTime.Now;
						fiscal_yearstart_month = new DateTime(n.Year, yearend_month, 1).AddMonths(1).Month;
						var fiscal_threshold = 13 - fiscal_yearstart_month;
						var current_fiscal_month = FiscalMonthLookup[fiscal_yearstart_month][n.Month];
						var past_fiscal_threshold = current_fiscal_month < 12;
						fiscal_current_year = n.Month >= fiscal_yearstart_month || past_fiscal_threshold && n.Month > yearend_month
							? n.Year
							: n.Year - 1;
						var start_year = fiscal_current_year != n.Year ? n.Year - 1 : n.Year;
						var end_year = yearend_month < 12 ? start_year + 1 : start_year;
						fiscal_start_current = new DateTime(start_year, fiscal_yearstart_month, 1);
						fiscal_end_current = new DateTime(end_year, yearend_month, DateTime.DaysInMonth(end_year, yearend_month));
						fiscal_start_previous = new DateTime(start_year - 1, fiscal_yearstart_month, 1);
						fiscal_end_previous = new DateTime(end_year - 1, yearend_month, DateTime.DaysInMonth(end_year - 1, yearend_month));
						}
					}
				}
			}
		public int get_fiscal_year(DateTime curr, NeTaxEntity c)
			{
			var fy		= 0;
			var current_fiscal_month		= FiscalMonthLookup[c.fiscal_yearstart_month][curr.Month];
			var past_fiscal_threshold		= current_fiscal_month < 12;
			fy			= curr.Month >= c.fiscal_yearstart_month || past_fiscal_threshold && curr.Month > c.yearend_month 
				? curr.Year
				: curr.Year - 1;
			return fy;
			}
		//public static bool is_int_gl (object te_id)
        //{
        //    var dsn = (Toolbox.doSQL_string(@"Select dsn from tax_entity  where id =@v0", new[] { te_id }));
        //    return (new Toolbox().getSQL_string(@"select ifnull((Select top 1 acct_no from gl_chart_of_accounts),0) ", dsn  , null).Trim().Length > 5);
        //}

        public static int TaxEntityFromUnit(int _business_unit_id)
        {

            using (var uow = new UnitOfWork())
            {
                try
                {
                    return uow.GetObjectByKey<ne_xpo.cs.business_unit>(_business_unit_id).tax_entity_id;
                }
                catch
                {
                    return Toolbox.doSQL_int("Select tax_entity_id from business_unit where id = @v0", new object[] { _business_unit_id });
                }
            }
        }
		public static void GetChildren(ref List<int> _finalList, int _teId)
			{
			using (var uow = new UnitOfWork())
				{
				_finalList.Add(_teId);
				var teList = (from d in new XPQuery<ne_xpo.cs.tax_entity_hiearchy>(uow)
								where d.parent_tax_entity_id == _teId
								select d.tax_entity_id).ToList();
				foreach (var child in teList)
					{
					if(_finalList.Contains(child)) continue;
					GetChildren(ref _finalList, child);
					}
				}
			}

        public static List<int> GetOwners(int _teId)
        {
            List<int> _finalList = new List<int>();
            using (var uow = new UnitOfWork())
            {
                var teList = (from d in new XPQuery<ne_xpo.cs.ownership>(uow)
                              where d.tax_entity_id == _teId
                              select d.member_id).ToList();
                foreach (var child in teList)
                {
                    if (_finalList.Contains(child)) continue;
                    GetChildren(ref _finalList, child);
                }
            }
            return _finalList;
        }
        public static string BaseFolder(object businessUnitId, bool externalUse)
        {
            int.TryParse(businessUnitId.ToString(), out var buId);
            if (buId == 0) throw new ArgumentOutOfRangeException(nameof(businessUnitId));

            var teId = TaxEntityFromUnit(buId);
			if(teId == 0) throw new ArgumentOutOfRangeException(nameof(teId));
            CheckEntityFolderStructure(teId);

            var fileServer = Toolbox.GetRequiredAppSetting("UNC_base_path");
            return externalUse?
                $@"/nesi_files/TE/TE{teId}": 
                $@"{fileServer}\nesi_files\TE\TE{teId}";
        }

        public static void CheckEntityFolderStructure(int _teId)
        {
			if(_teId == 0) return;
            var baseFolder = Path.Combine(Toolbox.GetRequiredAppSetting("UNC_base_path"), "nesi_files", "TE");
            var teBaseFolder = $@"{baseFolder}\TE{_teId}";

            var bu_s = Toolbox.doSQL_string("Select GROUP_CONCAT(CONCAT('BU',id)) from business_unit where tax_entity_id = @v0", new object[] { _teId }).Replace(',','|');
			    if (Directory.Exists(baseFolder))
			        {


			        if (!Directory.Exists(teBaseFolder))
			            {
			            Directory.CreateDirectory(teBaseFolder);
			            }

			        var subFolderList = new[]
			            {
			            "_protected|NesiFileManager-InternalPublic|NesiFileManager-Confidential|NesiFileManager-WideOpenPublic",
			            "applicant_files",
			            "asset_pics",
			            "business_unit_files|" + bu_s,
			            "credit_card_receipts",
			            "customer_asset_files",
			            "customer_files",
			            "discipline_files",
			            "ERItemFolders",
			            "ftp",
			            "inventory_files",
			            "member_files",
			            "MessageboardImages",
			            "pos|named_scans|po_files|scanned_items",
			            "ProjectFolders",
			            "quote_store",
			            "quote_worksheet_files",
			            "safety_files",
			            "task_files",
			            "training_documents",
			            "training_files",
			            "TrainingVideos",
			            "vendor",
			            "videos",
			            "wos",
                        "signature_files"
                        };
			        foreach (var subFolder in subFolderList)
			            {
			            var childFolders = subFolder.Contains("|")
			                ? subFolder.Split('|').Skip(1)
			                    .ToArray() // Doing this because we don't want the first folder to be part of the sub set
			                : new string[] { };
			            var topLevelSubFolder = childFolders.Any() ? subFolder.Split('|')[0] : subFolder;
			            var teSubFolder = string.Format(@"{0}\{1}", teBaseFolder, topLevelSubFolder);
			            if (!Directory.Exists(teSubFolder))
			                {
			                Directory.CreateDirectory(teSubFolder);
			                }
			            if (childFolders.Any())
			                {
			                foreach (var childFolder in childFolders)
			                    {
			                    var teSubChildFolder = string.Format(@"{0}\{1}", teSubFolder, childFolder);
			                    if (!Directory.Exists(teSubChildFolder))
			                        {
			                        Directory.CreateDirectory(teSubChildFolder);
			                        }
			                    }
			                }
			            }
			        }
			    }
		}
	}