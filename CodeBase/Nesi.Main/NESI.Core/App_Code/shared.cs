using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using DevExpress.Xpo;
using NESI.Common.Models;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeShared
	/// </summary>
	public class shared
		{
        public static string PrintSidePanelHTML(NeMember mem) => "";

        public static string UNC_ticket_attachments() => 
            Toolbox.GetRequiredAppSetting("UNC_ticket_attachments");

		    public static string PrintMenuHTML(NeMember mem, string page_id)
			{
			var myresponse = "\n<table border=0 cellpadding=1 cellspaning=0>";
			foreach (NePage page in mem.Pages)
				{
				string arrow;
				arrow = page_id == Convert.ToString(page.ID) ? "&gt;" : "&nbsp;";
				myresponse += "\n<tr>";
				myresponse += "\n<td>" + arrow + "</td><td><a href=\"" + page.Path + "\">" + page.Name + "</a></td>";
				myresponse += "\n</tr>";
				myresponse += "\n</tr>";
				}
			myresponse += "\n<tr>";
			myresponse += "\n<td><td>&nbsp;</td><a href=\"Default.aspx?sign_out=true\">Sign Out</a></td>";
			myresponse += "\n</tr>";
			myresponse += "\n</table>";

			return myresponse;
			}

		public static string PrintSubMenuHTML(NeMember mem, string page_id)
			{
			// first get the page obj
			var myPage = mem.GetPage(page_id);

			var myresponse = "\n";
			foreach (NePrivilege priv in myPage.Privileges)
				{
				myresponse += string.Format("\n<a href=\"{0}?page_id={1}&priv_id={2}\">{3}</a> |", myPage.Path, myPage.ID, priv.id, priv.name);
				}

			return myresponse;
			}

		    public static NeMember get_nesi_member_based_on_function(string _function)
		        {
		        NeMember m = new NeMember(Toolbox.doSQL_int("select get_nesi_member_id_based_on_function(@v0)",new object[] { _function } ));
		        return m;
		        }

		public static ArrayList GetPages()
			{
			var arlPages = new ArrayList();
			var _pages	= Toolbox.doSQL_dt(@" SELECT a.page_id id, a.page_parent_id parent_id, a.page_name name, a.page_desc desc, a.page_scriptpath url, a.page_order order, a.page_mobile_ready mobile_ready, a.page_mobile_icon mobile_icon, a.page_mobile_action mobile_action, b.privilege_id priv_id, b.privilege_name priv_name, b.privilege_desc priv_desc, a.page_mainmenu_img img, a.page_mainmenu_img_h img_h FROM page a LEFT JOIN privilege b ON a.page_id = b.privilege_page_id"  , null);

			var lastid = 0;
			var tempPage			= new NePage();
			foreach(DataRow _page in _pages.Rows)
				{
				var this_id			= Convert.ToInt32(_page["id"]);
				var this_parent_id		= Convert.ToInt32(_page["parent_id"]);
				var this_name		= _page["name"].ToString();
				var this_desc		= _page["desc"].ToString();
				var this_url			= _page["url"].ToString();
				var this_order			= Convert.ToInt32(_page["order"]);
				var this_priv_id		= _page["priv_id"] == DBNull.Value		? 0  : Convert.ToInt32(_page["priv_id"]);
				var this_priv_name	= _page["priv_name"] == DBNull.Value	? "" : _page["priv_name"].ToString();
				var this_priv_desc	= _page["priv_desc"] == DBNull.Value	? "" : _page["priv_desc"].ToString();
				var this_img			= _page["img"].ToString();
				var this_img_h		= _page["img_h"].ToString();
				var this_mobile_action	= _page["mobile_action"].ToString();
				var this_mobile_icon		= _page["mobile_icon"].ToString();
				var this_mobile_ready	= Convert.ToInt16(_page["mobile_ready"]) == 1;
				if (lastid == 0)
					{
					// make a new page
					tempPage  = new NePage {	ID			= this_id, 
												ParentID    = this_parent_id, 
												Name        = this_name, 
												Description = this_desc, 
												Path        = this_url, 
												Order       = this_order, 
												Img         = this_img, 
												Img_h       = this_img_h,
												mobile_action	= this_mobile_action,
												mobile_ready	= this_mobile_ready,
												mobile_icon		= this_mobile_icon
												};
					}
				else if (lastid != this_id)
					{
					// add the page with all the privileges already added to it
					arlPages.Add(tempPage);
					// make a new page
					tempPage = new NePage {		ID			= this_id, 
											ParentID    = this_parent_id, 
											Name        = this_name, 
											Description = this_desc, 
											Path        = this_url, 
											Order       = this_order, 
											Img         = this_img, 
											Img_h       = this_img_h ,
											mobile_action	= this_mobile_action,
											mobile_ready	= this_mobile_ready,
											mobile_icon		= this_mobile_icon
											};
					}
				if (this_priv_id != 0)
					{
					tempPage.AddPrivilege(new NePrivilege {	id			= Convert.ToInt32(this_priv_id), 
															description = this_priv_desc, 
															name		= this_priv_name, 
															page_id		= Convert.ToInt32(this_id) 
															});
					}
				lastid = this_id;
				}
			if (lastid != 0)
				{
				arlPages.Add(tempPage);
				}
			return arlPages;
			}
		public static void alert_hr(string status_subject, string status_message)
			{
			var e			= new NeEMail 
			     				{ 
			     				To		= EmailID.HR + Toolbox.app_setting("DomainForEmail"),
			     				From	= EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),								
			     				Subject	= status_subject,
			     				Body	= status_message,
			     				isHTML	= true
			     				};
			e.Send();
			}
		public static void alert_payroll(string status_subject, string status_message)
			{
			var e			= new NeEMail 
			     				{ 
			     				To		= EmailID.Payroll + Toolbox.app_setting("DomainForEmail"),
			     				From	= EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),
			     				Subject	= status_subject,
			     				Body	= status_message,
			     				isHTML	= true
			     				};
			e.Send();
			}
		public static void alert_payroll(string status_subject, string status_message, NeMember _employee, payroll.expense _expense)
			{
			var e			= new NeEMail 
			     				{ 
			     				To		= EmailID.Payroll + Toolbox.app_setting("DomainForEmail"),
			     				From	= EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),
			     				Subject	= status_subject,
			     				Body	= status_message,
			     				isHTML	= true,
			     				};
			var fileServer				= NeTaxEntity.BaseFolder(_employee.business_unit_id, false);
			if(_expense.has_file)
				{
				var receipt_scan		= fileServer + @"\expense_receipts\" + _expense.id_expense + "." + _expense.file_ext;
				if(File.Exists(receipt_scan))
					{
					e.Attachment				= new Attachment(receipt_scan, _expense.file_mime);
					}
				}
			e.Send();
			}
		public static void alert_ap(string status_subject, string status_message, bool _ap_canada, string from_address = "administrator@newelectric.com")
		{
			    if (string.IsNullOrEmpty(from_address) || from_address.StartsWith("administrator"))
			    {
			        from_address = EmailID.Administrator + Toolbox.app_setting("DomainForEmail");
			    }

			    var e			= new NeEMail
			     				{ 
			     				To		= _ap_canada ? EmailID.AP + Toolbox.app_setting("DomainForEmail") : EmailID.USAP + Toolbox.app_setting("DomainForEmail"),
			     				From	= from_address,
			     				Subject	= status_subject,
			     				Body	= status_message,
			     				isHTML	= true
			     				};
			e.Send();
			}
		public static void alert_ar(string status_subject, string status_message)
			{
			var e			= new NeEMail
			     				{ 
			     				To		= EmailID.AR + Toolbox.app_setting("DomainForEmail"),
			     				From	= EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),
			     				Subject	= status_subject,
			     				Body	= status_message,
			     				isHTML	= true
			     				};
			e.Send();
			}
		public static void alert_debug(string status_subject, string status_message)
			{
			var e			= new NeEMail 
			     				{ 
			     				To		= EmailID.Debug + Toolbox.app_setting("DomainForEmail"),
			     				From	= EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),
			     				Subject	= status_subject,
			     				Body	= status_message,
			     				isHTML	= true
			     				};
			e.Send();
			}
		public static void alert_it(string status_subject, string status_message)
			{
			var e			= new NeEMail 
			     				{ 
			     				To		= EmailID.Help + Toolbox.app_setting("DomainForEmail"),
			     				From	= EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),
			     				Subject	= status_subject,
			     				Body	= status_message,
			     				isHTML	= true
			     				};
			e.Send();
			}
		public static void alert_invoicing(string status_subject, string status_message)
			{
			var e			= new NeEMail 
			     				{ 
			     				To		= EmailID.Invoicing + Toolbox.app_setting("DomainForEmail"),
			     				From	= EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),
			     				Subject	= status_subject,
			     				Body	= status_message,
			     				isHTML	= true
			     				};
			e.Send();
			}
		public static string TruncateLongString(string str, int maxLength)
			{
			if (string.IsNullOrEmpty(str))
				return str;
			return str.Substring(0, Math.Min(str.Length, maxLength));
			}
		public static void GetParentCats(int cat_id, ref string strNames, ref string strShortDesc)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT Cat_ID,Cat_Parent_ID,Cat_Name,Cat_ShortDesc,Cat_Desc FROM Cat ORDER BY Cat_Order"  , null);
			GetParentCats(cat_id, ref strNames, ref strShortDesc, ref dt);
			}

		private static void GetParentCats(int cat_id, ref string strNames, ref string strShortDesc, ref DataTable dt)
			{
			foreach (DataRow row in dt.Rows)
				{
				if (cat_id == (int) row["Cat_ID"])
					{
					strNames = "> " + row["Cat_Name"] + strNames;
					strShortDesc = row["Cat_ShortDesc"] + " " + strShortDesc;

					if ((int) row["Cat_Parent_ID"] != 0)
						{
						GetParentCats((int) row["Cat_Parent_ID"], ref strNames, ref strShortDesc, ref dt);
						}
					}
				}
			}

		public static DataTable GetBV7DSNs(bool include_masters)
			{
			if (include_masters)
				{
				return Toolbox.doSQL_dt(@"SELECT *, b.dsn consolidated_dsn, 1 is_consol FROM business_unit a inner join tax_entity b on a.tax_entity_id = b.id ORDER BY a.name"  , null);
				}
			else
				{
				return Toolbox.doSQL_dt(@"SELECT *, b.dsn consolidated_dsn, 1 is_consol FROM business_unit a inner join tax_entity b on a.tax_entity_id = b.id  WHERE a.name NOT like 'MASTER%' ORDER BY a.name" , null);
				}
			}
	
		public static DataTable GetAllBVDSNs(bool include_masters)
			{
			if(include_masters)
				{
				return Toolbox.doSQL_dt(@"SELECT
    *,
    b.dsn consolidated_dsn,
    1 is_consol
FROM
    business_unit a
    INNER JOIN tax_entity b
        ON a.tax_entity_id = b.id
WHERE b.is_active = 1 AND dsn != ''
ORDER BY a.name", null);
				}
			else
				{
				return Toolbox.doSQL_dt(@"SELECT *, b.dsn consolidated_dsn, 1 is_consol FROM business_unit a inner join tax_entity b on a.tax_entity_id = b.id  WHERE b.is_active AND a.name NOT like 'MASTER%' ORDER BY a.name", null);
				}
			}
		public static DataTable GetBV7DSNs()
			{
			return GetAllBVDSNs(false);
			}
		public static DataTable GetALLBV7DSNs()
			{
			return Toolbox.doSQL_dt(@"SELECT *, b.dsn consolidated_dsn FROM business_unit a inner join tax_entity b on a.tax_entity_id = b.id  where a.active = 'T' AND b.is_active ORDER BY a.name", null);
			}

		public static DataTable GetBV7DSNs(string _business_unit_id)
			{
			return Toolbox.doSQL_dt(@"SELECT b.dsn FROM business_unit a inner join tax_entity b on a.tax_entity_id = b.id  WHERE a.ID=@v0", new object[] { _business_unit_id });
			}

		public static string GetBV7DSN(string _business_unit_id)
			{
			return Toolbox.doSQL_string(@"SELECT b.dsn FROM business_unit a inner join tax_entity b on a.tax_entity_id = b.id WHERE a.ID=@v0", _business_unit_id);
			}

		public static DataTable GetTestDSNs()
			{
			return Toolbox.doSQL_dt(@"SELECT b.dsn FROM business_unit a inner join tax_entity b on a.tax_entity_id = b.id  WHERE a.IsTest='T'" , null);
			}

		public static DataTable GetProvs()
			{
			return Toolbox.doSQL_dt(@"SELECT * FROM (SELECT * FROM prov  WHERE prov_abbv != 'OTHER' ORDER BY prov_abbv) sub UNION SELECT * FROM prov WHERE prov_abbv = 'OTHER'" , null);
			}

		public static DataTable GetCountries()
			{
			var dt = new DataTable();
			dt.Columns.Add("Prov_Abbv");
			dt.Columns.Add("Prov_Desc");
			dt.Columns.Add("Prov_BVAbbv");

			dt.Rows.Add(new object[]{"CAN", "Canada", "CAN"});
			dt.Rows.Add(new object[]{"USA", "USA", "USA"});
			dt.Rows.Add(new object[]{"OTHER", "OTHER", "OTHER"});

			return dt;
			}


		/// <summary>
		/// Produces the sell price using BV's inventory
		/// </summary>
		/// <param name="CostPrice"></param>
		/// <param name="SellPrice"></param>
		/// <param name="Prod"></param>
		/// <param name="Quantity"></param>
		/// <returns></returns>
		public static double GetSellPrice(double cost, double sell, string qty_flag, double qty, int business_unit_id)
			{
			var is_qty = (qty_flag == "QTY");
			return Toolbox.doSQL_double(@"SELECT GETSELLPRICE(@v0 , @v1 , @v2 , @v3, @v4 )", new object[] {  cost, sell, is_qty, qty, business_unit_id } );
			#region old code

			/*
            Prod = "QTY"; //Treat all parts as QTY Discount
        DataRow dr		 = Toolbox.doSQL_dt(@"SELECT * FROM markup_formula"  , null).Rows[0];
        double markup_qty_threshold = Convert.ToDouble(dr["markup_formula_qty_costxqty_threshold"]);
        double markup_qty_minimum = Convert.ToDouble(dr["markup_formula_qty_minimum_markup_multiplier"]);
        double markup_qty_Kexp = Convert.ToDouble(dr["markup_formula_qty_Kexp"]);
        double markup_threshold = Convert.ToDouble(dr["markup_formula_costxqty_threshold"]);
        double markup_minimum = Convert.ToDouble(dr["markup_formula_minimum_markup_multiplier"]);
        double markup_Kexp = Convert.ToDouble(dr["markup_formula_Kexp"]);
        double dbl_temp=0.0;
        double dbl = 0;
        #region old info
        //double dbl = Math.Exp(CostPrice > 100 ? 100 : CostPrice);
        //double getSellPrice = CostPrice * (1.35 + (6 * (1 / Exp(IIf(CostPrice > 100, 100, CostPrice) * 0.5))))
/*
        Prod = Prod.Trim();
        if (Prod.Contains("QTY"))
        {
            if ((CostPrice * Quantity) > 5000)
            {
                dbl = CostPrice * 2;
            }
            else
            {
                dbl = (CostPrice * 2) + ((SellPrice - (CostPrice * 2)) * (1 / Math.Exp(CostPrice * Quantity * 0.0054)));
            }
        }
        else
        {
            if ((CostPrice * Quantity) > 5000)
            {
                dbl = CostPrice * 1.35;
            }
            else
            {

                dbl = CostPrice * (1.35 + (2.4 * (1 / Math.Exp(CostPrice * 0.14))));
                
            }
            if (CostPrice == 0)
            {
                dbl = SellPrice;
            }
            if (dbl < SellPrice)
            {
                dbl = SellPrice;
            }
        }

        // first check for the base price sell price
            if ((CostPrice) > markup_threshold)
            {
                dbl_temp = CostPrice * markup_minimum;
            }
            else
            {
                dbl_temp = CostPrice * (markup_minimum + (3.0 * (1 / Math.Exp(Math.Abs(CostPrice * markup_Kexp)))));
            }
            if (CostPrice == 0)
            {
                dbl_temp = SellPrice;
            }


		#endregion old_info
        Prod = Prod.Trim();
        if ((Prod.Contains("QTY")) && (Quantity!= 1) && (Quantity !=-1))
        {
            if ((CostPrice * Quantity) > markup_qty_threshold)
            {
                dbl = CostPrice * markup_qty_minimum;
            }
            else
            {
                if (SellPrice < dbl_temp)
                {
                    SellPrice = dbl_temp;
                }
                dbl = (CostPrice * markup_qty_minimum) + ((SellPrice - (CostPrice * markup_qty_minimum)) * (1 / Math.Exp(Math.Abs(CostPrice * Quantity * markup_qty_Kexp))));
                        
            }
        }
        else
        {
            
            if ((CostPrice) > markup_threshold)
            {
                dbl = CostPrice * markup_minimum;
            }
            else
            {
                dbl = CostPrice * (markup_minimum + (3.0 * (1 / Math.Exp(Math.Abs(CostPrice * markup_Kexp)))));
            }
            if (dbl == 0)
            {
                dbl = SellPrice;
            }
            /*if (dbl < SellPrice)
            {
              dbl = SellPrice;
            }
        }

		*/

			#endregion old code
			}

		/// <summary>
		/// Produces the sell price using BV's inventory
		/// </summary>
		/// <param name="cost"></param>
		/// <param name="sell"></param>
		/// <param name="Prod"></param>
		/// <param name="qty"></param>
		/// <returns></returns>
		public static double GetSellPrice(double cost, double sell, bool qty_flag, double qty, int business_unit_id)
			{
			var dbl = qty_flag ? GetSellPrice(cost, sell, "QTY", qty, business_unit_id) : GetSellPrice(cost, sell, "", qty, business_unit_id);
			return dbl;
			}

		public static double GetLabourSell(int labourcode)
			{
			double co=0;
			try
				{
				co = Toolbox.doSQL_double(@"Select getlaboursell(@v0)",new object[] { labourcode } );
				}
			catch { }
			return co;


			}

		public static bool IsNumeric(string StringValue)
			{
			var chrArray = StringValue.ToCharArray();

			for (var i = 0; i < chrArray.Length; i++)
				{
				if (!char.IsNumber(chrArray[i]))
					{
					return false;
					}
				}
			return true;
			}


		public static bool IsAlphabetic(string StringValue)
			{
			var chrArray = StringValue.ToCharArray();

			for (var i = 0; i < chrArray.Length; i++)
				{
				if (!char.IsLetter(chrArray[i]))
					{
					return false;
					}
				}
			return true;
			}

		public static bool IsAlphaNumeric(string StringValue)
			{
			var chrArray = StringValue.ToCharArray();

			for (var i = 0; i < chrArray.Length; i++)
				{
				if (!char.IsNumber(chrArray[i]) & !char.IsLetter(chrArray[i]))
					{
					return false;
					}
				}
			return true;
			}

		public static string FilenameClean(object name)
			{
			var temp_name = name.ToString();
			// first trim the raw string
			var safe = temp_name.Trim();

			// replace spaces with hyphens
			safe = safe.Replace(" ", "_");

			// trim out illegal characters
			safe = Regex.Replace(safe, "[^a-zA-Z0-9\\-_]", "");

			// trim the length
			if (safe.Length > 50)
				safe = safe.Substring(0, 49);

			// clean the beginning and end of the filename
			char[] replace = {'-', '.'};
			safe = safe.TrimStart(replace);
			safe = safe.TrimEnd(replace);

			return safe;
			}

		public static DataTable GetAllCompany()
			{
			return Toolbox.doSQL_dt(@"SELECT * FROM business_unit  WHERE IsTest!='T'" , null);
			}
		public class BusinessHours
			{
			private TimeSpan startingTime;
			private TimeSpan endingTime;
			private DayOfWeek[] excludeDays;
 
			public BusinessHours(TimeSpan? startingTime, TimeSpan? endingTime, DayOfWeek[] excludeDays)
				{
				this.startingTime = startingTime ?? new TimeSpan(8, 0, 0);
				this.endingTime = endingTime ?? new TimeSpan(17, 0, 0);
				this.excludeDays = excludeDays ?? new DayOfWeek[] 
													{ 
													DayOfWeek.Saturday , 
													DayOfWeek.Sunday 
													};
				}
 
			public BusinessHours() : this(null,null,null)
				{
       
				}
 
			public double Calculate(DateTime startDate, DateTime endDate)
				{
 
				var counter = startDate;
				double hours = 0;
 
				while (counter <= endDate)
					{
					var dayStart = counter.Date.Add(startingTime);
					var dayEnd = counter.Date.Add(endingTime);
					var nextDayStart = startDate.Date.Add(startingTime).AddDays(1);
 
					if (counter < dayStart)
						counter = dayStart;
 
					if (excludeDays == null || 
						excludeDays.Contains(counter.DayOfWeek) == false)
						{
						if (endDate < nextDayStart)
							{
							var ticks = Math.Min(endDate.Ticks, dayEnd.Ticks) - counter.Ticks;
							hours = TimeSpan.FromTicks(ticks).TotalHours;
							break;
							}
						else if (counter.Date == startDate.Date)
							{
							if (counter >= dayStart && counter <= dayEnd)
								{
								hours += (dayEnd - counter).TotalHours;
								}
							}
						else if (counter.Date == endDate.Date && 
								startDate.Date != endDate.Date)
							{
							if (counter >= dayStart && counter <= dayEnd)
								{
								hours += (counter - dayStart).TotalHours;
								}
							else if (counter > dayEnd)
								{
								hours += (endingTime - startingTime).TotalHours;
								}
							}
						else
							{
							hours += (endingTime - startingTime).TotalHours;
							}
 
						}
 
					counter = counter.AddDays(1);
					if (counter.Date == endDate.Date)
						counter = endDate;
					}
 
				return hours;
				}
 
			}
		public class properties
			{
			public string name { get; set; }
			public string value { get; set; }
			public DateTime expires { get; set; }
			public properties(){}
			public properties(string _name)
				{
				load(_name);
				}
			private void load(string _name)
				{
				using(var uow = new UnitOfWork())
					{
					if(exists(_name))
						{
						var x		= uow.GetObjectByKey<ne_xpo.cs.properties>(_name);
						name		= x.name;
						value		= x.value1;
						expires		= x.expires;
						}
					}
				}
			public static bool exists(string _name)
				{
				using(var uow = new UnitOfWork())
					{
					var x		= uow.GetObjectByKey<ne_xpo.cs.properties>(_name);
					return x != null;
					}
				}
			public void save()
				{
				if(name != "" && value != "" && expires.Year > 0)
					{
					using(var uow = new UnitOfWork())
						{
						var x		= uow.GetObjectByKey<ne_xpo.cs.properties>(name) ?? new ne_xpo.cs.properties(uow);
						x.name		= name;
						x.value1	= value;
						x.expires	= expires;
						x.Save();
						uow.CommitChanges();
						}
					}
				}
			public static void clean(ref int i)
				{
				using(var uow = new UnitOfWork())
					{
					var props		= from y in new XPQuery<ne_xpo.cs.properties>(uow)
						where y.expires < DateTime.Now
						select y;

					foreach(var p in props)
						{
						p.Delete();
						i++;
						}
					uow.CommitChanges();
					}
				}
			public static void clean()
				{
				var i = 0;
				clean(ref i);
				}
			public static void clean(string _name)
				{
				using (var uow = new UnitOfWork())
					{
					var props = from y in new XPQuery<ne_xpo.cs.properties>(uow)
					            where y.name == _name
					            select y;

					foreach (var p in props)
						{
						p.Delete();
						}
					uow.CommitChanges();
					}
				}
			}
		public class signature
			{
			public int id               { get; set; }
			public DateTime created_dt  { get; set; }
			public int member_id        { get; set; }
			public string table        { get; set; }
			public int table_id        { get; set; }
			public int contact_id        { get; set; }
			public string printed_name  { get; set; }
			public byte[] graphic       { get; set; }
			public string send_to_contact_ids {get;set;}
			public signature(){}
			public signature(int _id, string _table)
				{
				if(exists(_id,_table))
					{
					load(_id,_table);
					}
				}
			public signature(int _id)
				{
				if(exists(_id))
					{
					load(_id);
					}
				}
			private void load(int _id)
				{
				using(var uow = new UnitOfWork())
					{
					var x			= uow.GetObjectByKey<ne_xpo.cs.signature>(_id);
					if(x != null)
						{
						id				= x.id;
						created_dt		= x.created_dt;
						member_id		= x.member.member_id;
						table			= x.table;
						table_id		= x.table_id;
						contact_id		= x.contact_id;
						send_to_contact_ids = x.send_to_contact_ids;
						printed_name	= x.printed_name;
						graphic			= x.graphic;
						}
					}

				}
			private void load(int _id, string _table)
				{
				using(var uow = new UnitOfWork())
					{
					var x			= (from y in new XPQuery<ne_xpo.cs.signature>(uow)
						where y.table_id == _id && y.table == _table
						select y).LastOrDefault();
					if(x != null)
						{
						id				= x.id;
						created_dt		= x.created_dt;
						member_id		= x.member.member_id;
						table			= x.table;
						table_id		= x.table_id;
						send_to_contact_ids = x.send_to_contact_ids;
						contact_id		= x.contact_id;
						printed_name	= x.printed_name;
						graphic			= x.graphic;
						}
					}
				}
			public bool exists(int _id, string _table)
				{
				using(var uow = new UnitOfWork())
					{
					return (from y in new XPQuery<ne_xpo.cs.signature>(uow)
						where y.table_id == _id && y.table == _table
						select y).Any();
					}
				}
			public bool exists(int _id)
				{
				using(var uow = new UnitOfWork())
					{
					var x			= uow.GetObjectByKey<ne_xpo.cs.signature>(_id);
					return x != null;
					}

				}
			public void save()
				{
				using(var uow = new UnitOfWork())
					{
					var x			= id == 0 
						? new ne_xpo.cs.signature(uow)
						: uow.GetObjectByKey<ne_xpo.cs.signature>(id);
					x.member		= uow.GetObjectByKey<ne_xpo.cs.member>(member_id);
					x.created_dt	= created_dt;
					x.table			= table;
					x.send_to_contact_ids = send_to_contact_ids;
					x.contact_id	= contact_id;
					x.table_id		= table_id;
					x.printed_name	= printed_name;
					x.graphic		= graphic;
					x.Save();
					uow.CommitChanges();
					if(id == 0)
						{
						id = x.id;
						}
					}

				}
			public void delete()
				{
				using(var uow = new UnitOfWork())
					{
					var x			= uow.GetObjectByKey<ne_xpo.cs.signature>(id);
					x.Delete();
					uow.CommitChanges();
					}

				}
			}
		public class mobile_subpage : System.Web.UI.UserControl
			{
			public int this_woprog_id		{ get; set; }
			public bool hide_logo			{ get; set; }
			public string from				{ get; set; }
			}
		}
	}