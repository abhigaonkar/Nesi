using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using System.IO;

namespace nesi.core
{
    /// <summary> 
    /// Summary description for NeQuotes 
    /// </summary> 
    public class quote
    {
        //private string _drpid; 
        private int _pct_chance;
        public int QuoteID { get; private set; }
        public string txtCustomerName { get; private set; }
        public int txtContact { get; private set; }
        public string txtJobDescription { get; set; }
        public string All { get; private set; }
        public DateTime? PrintedDate { get; private set; }
        public DateTime? LastFaxedDate { get; private set; }
        public double Price { get; private set; }
        public double HoursSpent { get; private set; }
        public int Revision { get; private set; }
        public int price_type { get; private set; }
        public int quoted_by { get; set; }
        public int cust_id { get; set; }
        public int contact_id { get; set; }
        public int address_id { get; set; }
        public string completion_date { get; set; }
        public int apply_discount { get; set; }
        public int status { get; set; }
        public string cust_spec_doc { get; set; }
        public int woprogid { get; set; }
        public string cust_po { get; set; }
        public string date_due { get; set; }
        public string date_expected_start { get; set; }
        public string notes { get; set; }
        public int business_unit_id { get; set; }
        public bool IsTM { get; set; }
        public int revenue_line_id { get; set; }
		public int ValidPeriod { get; set; }
        public int cost_level { get; set; }
        public DateTime opendate { get; set; }
        public DateTime? killed_date { get; set; }
        public bool allowed_to_quote { get; set; }
        public int pct_chance { get { return _pct_chance; } set { _pct_chance = value; } }
        public bool allowed_to_quote_2 { get; set; }
        public double expected_value { get; set; }
        public int killed_by { get; set; }
        public int currency { get; set; }
        public string why_killed { get; set; }
        public string quote_kill_stage { get; set; }
        public double margin { get; set; }
        public bool parallel_bid { get; set; }
        public int net_due { get; set; }
		public bool IncludeTitle { get; set; }
        public int bdm { get;  set; }
        public int acting_bdm { get; private set; }

        #region WorkSheet Information 
        public int worksheet_id { get; set; }
        public int worksheetquote_id { get; set; }
        public int worksheet_rev { get; set; }
        public int worksheet_section_id { get; set; }
        public int worksheet_member_id { get; set; }
        public string worksheet_partno { get; set; }
        public string worksheet_code { get; set; }
        public string worksheet_description { get; set; }
        public double worksheet_sell { get; set; }
        public double worksheet_cost { get; set; }
        public double worksheet_extendedper { get; set; }
        public double worksheet_originalsell { get; set; }
        public double worksheet_qty { get; set; }
        public DateTime worksheet_ts { get; set; }
        public int quote_worksheet_part_requested { get; set; }
        public double quote_worksheet_discount { get; set; }
        public int worksheet_consignment_id { get; set; }
        #endregion
        public quote()
        {
            quote_worksheet_discount = 0;
            quote_worksheet_part_requested = 0;
            apply_discount = 0;
            worksheet_consignment_id = 0;
        }
        public static DataTable GetCustomersAndDetails(int _business_unit_id)
        {
            return Toolbox.doSQL_dt(@" SELECT DISTINCT a.customer_name txtCustomerName, a.customer_id ID FROM customer a, quote_master b WHERE a.customer_id = b.customer_id AND b.status_id NOT IN (6,7,8,9) AND b.business_unit_id = @v0  ORDER BY a.customer_name",
            new object[] { _business_unit_id });
        }
        public ArrayList LoadCustWOLIST(string custname)
        {
            var list = new ArrayList();
            var dt = Toolbox.doSQL_dt(@" SELECT a.quote_n, b.customer_name, a.job_description, a.quoted_price, revision, a.business_unit_id,  
a.quote_id FROM quote a, customer b WHERE a.customer_id = b.customer_id and a.status_id NOT IN (6,7,8,9) AND b.customer_name = @v0  ORDER BY quote_id desc",
            new object[] { custname });
            foreach (DataRow dr in dt.Rows)
            {
                var item = new quote
                {
                    business_unit_id = Convert.ToInt32(dr["business_unit_id"]),
                    QuoteID = Convert.ToInt32(dr["quote_n"]),
                    All = dr["quote_id"] + " -R" + dr["revision"] + " - " +
                dr["job_description"] + " - $ " + dr["quoted_price"],
                    HoursSpent = NeMemberTime.HoursSpent_onQuote(Convert.ToInt16(QuoteID))
                };
                list.Add(item);
            }
            return list;
        }
        public static DataTable quote_list_for_timesheet(string _customer_number)
        {
            using (var conn = Toolbox.connect())
            {
                return Toolbox.doSQL_dt(conn, @" SELECT a.quote_n value, CONCAT('(',c.name,') ',a.quote_id, ' - R', a.revision, ' - ', a.job_description, ' - $', a.quoted_price) text FROM quote a LEFT JOIN customer b ON a.customer_id = b.customer_id LEFT JOIN business_unit c ON a.business_unit_id = c.id  WHERE a.status_id NOT IN (6,7,9,8) and b.customer_number =@v0 ORDER BY c.name, a.quote_id desc", new object[] { _customer_number });
            }
        }

        public void load_worksheet_row(int worksheet_id)
        {
            var sql = "Select * from quote_worksheet where id = " + worksheet_id;
            var tools = new Toolbox();
            var lineitem = tools.getSQL_datatable(@"Select * from quote_worksheet  where id =@v0", new object[] { worksheet_id });
            foreach (DataRow row in lineitem.Rows)
            {
                worksheet_qty = Convert.ToInt32(row["qty"]);
                worksheet_partno = row["part_no"].ToString();
                worksheet_sell = Convert.ToDouble(row["sell"]);
                worksheet_cost = Convert.ToDouble(row["cost"]);
                worksheet_consignment_id = Convert.ToInt32(row["consignment_id"]);
            }
        }
        public static void copy_worksheet_file_to_new_rev(string quote_id, string old_rev, string newrev)
        {
            var _tools = new Toolbox();
            var dt = Toolbox.doSQL_dt(@"Select id,part_no,description from quote_worksheet  where has_file = 1 and quote_id =@v0 and revision =@v1 ", new object[] { quote_id, old_rev });
            if (dt.Rows.Count > 0)
            {
                var quoteObj = new quote(Convert.ToInt32(quote_id));
                var fileServer = NeTaxEntity.BaseFolder(quoteObj.business_unit_id, false);
                foreach (DataRow dr in dt.Rows)
                {
                    // find old file(s) 
                    try
                    {
                        var old_path = Path.Combine(fileServer + @"\quote_store\quote_worksheet_lines", string.Format("L{0}", dr["id"].ToString()));
                        var di = new DirectoryInfo(old_path);
                        var rgFiles = di.GetFiles();
                        // find new ID number based on description and masterid 
                        var newlineid = _tools.getSQL_int(@"select ifnull((Select id from quote_worksheet  where has_file=1 and quote_id =@v0 and revision =@v1  and part_no =@v2  and description =@v3  limit 1),0)", new object[] { quote_id, newrev, dr["part_no"], dr["description"] });
                        // create new folder if new one doesnt exist 
                        var new_path = Path.Combine(fileServer + @"\quote_store\quote_worksheet_lines", string.Format("L{0}", newlineid));
                        Directory.CreateDirectory(new_path);
                        var di_new = new DirectoryInfo(new_path);
                        // copy file over with new line item. 
                        var f = new NeFiles();
                        f.copy_all(di, di_new, false);
                    }
                    catch { }
                }
            }
        }
        public quote(int quoteId, int revision)
        {
            load(quoteId, revision);
        }

        public quote(int QuoteNo)
        {
            if (QuoteNo == 0) return;
            var strQuote = QuoteNo.ToString();
            
            if (strQuote.Length == 6)
            {
                load(QuoteNo, GetActiveRevision(QuoteNo));
            }
            else
            {
                splice(QuoteNo, out var quoteId, out var revision);
                load(quoteId, revision);
            }
        }
        public static int GetActiveRevision(int quoteId)
			{
			return Toolbox.doSQL_int(@"SELECT revision from quote_master WHERE quote_id = @v0 AND active_revision = 1 LIMIT 1", new object[] { quoteId });
			}
		public static int GetStatus(int quoteId, int revision)
			{
			return Toolbox.doSQL_int(@"SELECT status_id FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quoteId, revision });
			}
		/// <summary>
        /// Update the work order header level snapshot of the quoted hours
        /// </summary>
        /// <param name="woProgId"></param>
		public static void UpdateWoQuotedHours(int woProgId)
			{
			Toolbox.doSQL_void(@"CALL QUOTED_HOURS_FOR_WO(@v0)", new object[] { woProgId });
			}
        private void load(int quoteId, int revision)
        {

            using (var conn = Toolbox.connect())
            {
                quote_worksheet_discount = 0;
                quote_worksheet_part_requested = 0;
                apply_discount = 0;
                worksheet_consignment_id = 0;
                var dt = Toolbox.doSQL_dt(conn, @" 
SELECT  
	a.*, 
	b.customer_name customer_name 
FROM  
	quote_master a 
LEFT JOIN 
	customer b ON a.customer_id = b.customer_id 
WHERE  
	a.quote_id = @v0 AND 
	a.revision = @v1", new object[] { quoteId, revision });
                QuoteID = 0;
                foreach (DataRow drSearch in dt.Rows)
                {
                    QuoteID = Convert.ToInt32(drSearch["quote_id"]);
                    txtCustomerName = drSearch["customer_name"].ToString();
                    txtJobDescription = drSearch["job_description"].ToString();
                    Price = Toolbox.ReturnZeroIfNull_double(drSearch["quoted_price"]);
                    Revision = Convert.ToInt32(drSearch["revision"]);
                    PrintedDate = Toolbox.ReturnNullableDateTimeIfNull(drSearch["last_print_date"]);
                    LastFaxedDate = Toolbox.ReturnNullableDateTimeIfNull(drSearch["last_fax_date"]);
                    business_unit_id = Convert.ToInt32(drSearch["business_unit_id"]);
                    txtContact = Toolbox.ReturnZeroIfNull_int(drSearch["contact_id"]);
                    price_type = Toolbox.ReturnZeroIfNull_int(drSearch["pricetype_id"]);
                    quoted_by = Toolbox.ReturnZeroIfNull_int(drSearch["quoted_by"]);
                    completion_date = drSearch["completion_date"].ToString();
                    cust_id = Toolbox.ReturnZeroIfNull_int(drSearch["customer_id"]);
                    contact_id = Toolbox.ReturnZeroIfNull_int(drSearch["contact_id"]);
                    HoursSpent = Toolbox.doSQL_double(conn, @"SELECT GET_QUOTEDHOURS(@v0,@v1)", new object[] { QuoteID, Revision });
                    worksheet_extendedper = Toolbox.doSQL_double(conn, @"SELECT GET_QUOTEWORKSHEET_EXTENDEDPER(@v0,@v1)", new object[] { QuoteID, Revision });
                    worksheet_originalsell = Toolbox.doSQL_double(conn, @"SELECT GET_QUOTEWORKSHEET_ORIGINALSELL(@v0,@v1)", new object[] { QuoteID, Revision });
                    margin = Toolbox.doSQL_double(conn, @"SELECT GET_QUOTEWORKSHEET_MARGIN(@v0,@v1,@2)", new object[] { QuoteID, Revision, Price});
                    status = Convert.ToInt32(drSearch["status_id"]);
                    opendate = Convert.ToDateTime(drSearch["open_date"]);
                    allowed_to_quote = Toolbox.ReturnZeroIfNull_int(drSearch["allowed_to_quote"]) == 1;
                    allowed_to_quote_2 = Toolbox.ReturnZeroIfNull_int(drSearch["allowed_to_quote_2"]) == 1;
                    IsTM = Toolbox.ReturnZeroIfNull_int(drSearch["is_tm"]) == 1;
					ValidPeriod = Toolbox.ReturnZeroIfNull_int(drSearch["period_valid"]);
                    address_id = Toolbox.ReturnZeroIfNull_int(drSearch["address_id"]);
					IncludeTitle = Toolbox.ReturnZeroIfNull_int(drSearch["include_title"]) == 1;
                    currency = Toolbox.ReturnZeroIfNull_int(drSearch["currency"]);
                    parallel_bid = Convert.ToBoolean(drSearch["parallel_bid"]);
                    if (address_id == 0 && cust_id > 0)
                    {
                        address_id = Toolbox.doSQL_int(conn, @"SELECT MAX(IFNULL(address_id, 0)) FROM address WHERE address_table = 'Customer' AND address_table_id = @v0 AND address_type = 'B'", new object[] { cust_id });
                    }
                    expected_value = Convert.ToDouble(drSearch["expected_value"]);
                    quote_kill_stage = Toolbox.ReturnBlankIfNull_string(drSearch["killed_at_stage"]);
                    killed_by = Toolbox.ReturnZeroIfNull_int(drSearch["killed_by"]);
                    why_killed = Toolbox.ReturnBlankIfNull_string(drSearch["why_killed"]);
                    killed_date = Toolbox.ReturnBlankDateTimeIfNull(drSearch["killed_date"]);
                    cust_spec_doc = Toolbox.ReturnBlankIfNull_string(drSearch["cust_spec_doc"]);
                    woprogid = Toolbox.ReturnZeroIfNull_int(drSearch["wo"]);
                    cust_po = Toolbox.ReturnBlankIfNull_string(drSearch["po"]);
                    date_due = Toolbox.ReturnBlankIfNull_string(drSearch["date_due"]);
                    date_expected_start = Toolbox.ReturnBlankIfNull_string(drSearch["date_expected_start"]);
                    _pct_chance = Toolbox.ReturnZeroIfNull_int(drSearch["pct_chance"]);
                    net_due = Toolbox.ReturnZeroIfNull_int(drSearch["net_due"]);                    
                    var c = Toolbox.doSQL_int(conn, @"SELECT COUNT(woprog_project_notes_id) FROM woprog_project_notes WHERE woprog_project_notes_type = 'Q' and woprog_project_notes_woprogid = @v0", new object[] { QuoteID });
                    if (c > 0)
                    {
                        notes = Toolbox.doSQL_string(conn, @"Select urldecode(IFNULL(woprog_project_notes_notes, '')) from woprog_project_notes  where woprog_project_notes_type = 'Q' and woprog_project_notes_woprogid =@v0", new object[] { QuoteID });
                    }
                    bdm = Toolbox.ReturnZeroIfNull_int(drSearch["bdm"]);
                    acting_bdm = Toolbox.ReturnZeroIfNull_int(drSearch["acting_bdm"]);
                }
            }
        }

        public void save()
        {

            var insert = false;
            var strsql = "";
            object[] paramObjects = null;
            if (QuoteID != 0)
            {
                strsql = @"update quote_master set 
 job_description = @v0, 
             Contact_ID = @v1, 
             quoted_by = @v2, 
             customer_id = @v3, 
 cust_spec_doc = @v4, 
 po = @v5, 
 date_due = @v6, 
 date_expected_start = @v15, 
completion_date = @v7, 
 business_unit_id = @v8, 
 address_id = @v9, 
 expected_value = @v10, 
 currency = @v11, 
pct_chance = @v12, 
parallel_bid=@v14, 
revenue_line_id=@16 
 
 where quote_master.quote_id = @v13 limit 1";
                paramObjects = new object[]
                {
txtJobDescription, //0 
Convert.ToInt32(contact_id),
Convert.ToInt32(quoted_by),
Convert.ToInt32(cust_id),//3 
HttpUtility.UrlEncode(cust_spec_doc),
HttpUtility.UrlEncode(cust_po),
date_due,
completion_date,
business_unit_id,
address_id,
expected_value, //10 
currency,
_pct_chance,
QuoteID,
parallel_bid, //14 
                        date_expected_start, //15 
                        revenue_line_id
                                    };
            }
            else
            {
                insert = true;
            }

            if (insert)
            {
                strsql = @"insert into quote_master( 
job_description, 
contact_id, 
business_unit_id, 
quoted_by, 
customer_id, 
cust_spec_doc, 
po, 
date_due, 
date_expected_start, 
status_id, 
revision, 
active_revision, 
completion_date, 
address_id, 
pct_chance, parallel_bid,revenue_line_id)  
 Values(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16)";
                paramObjects = new object[]
                {

txtJobDescription,//0 
Convert.ToInt32(contact_id)  ,
business_unit_id  ,
  Convert.ToInt32(quoted_by)  ,
Convert.ToInt32(cust_id)  ,
  HttpUtility.UrlEncode(cust_spec_doc)  ,//5 
HttpUtility.UrlEncode(cust_po)  ,
date_due  ,
date_expected_start, //8 
                      status  ,
1,//10 
1,
completion_date  ,
address_id  ,
_pct_chance,
parallel_bid,//15 
                    revenue_line_id
                                };
            }
            try
            {
                var _tools = new Toolbox();
                _tools.getSQL_void(strsql, paramObjects);
                if (insert)
                {
                    QuoteID = _tools.getSQL_int(@"Select quote_id from quote_master order by quote_id desc limit 1", null);
                    Revision = 1;
                }
            }
            catch
            {
            }
        }
        public DataTable get_quote_todo_list(int member_id)
        {
            var _tools = new Toolbox();
            var dt_final = new DataTable();
            dt_final.Columns.Add(new DataColumn("link"));
            dt_final.Columns.Add(new DataColumn("description"));
            dt_final.Columns.Add(new DataColumn("overdue"));
            dt_final.Columns.Add(new DataColumn("star"));
            dt_final.Columns.Add(new DataColumn("link2"));
            dt_final.Columns.Add(new DataColumn("quote_id"));
            dt_final.Columns.Add(new DataColumn("is_level_2"));
            dt_final.Columns.Add(new DataColumn("is_level_3"));
            dt_final.Columns.Add(new DataColumn("row_message"));
            dt_final.Columns.Add(new DataColumn("customer_name"));
            dt_final.Columns.Add(new DataColumn("this_date_open"));
            dt_final.Columns.Add(new DataColumn("this_fullname"));
            dt_final.Columns.Add(new DataColumn("this_description"));
            dt_final.Columns.Add(new DataColumn("revision"));
            dt_final.Columns.Add(new DataColumn("page_id"));
            dt_final.Columns.Add(new DataColumn("n2_params"));


            var dt = Toolbox.doSQL_dt(@" SELECT a.quote_id, a.revision, a.pricetype_id, b.type pricetype,  
a.price_to, a.open_date open_date, a.customer_id, a.quoted_price, a.job_description,  
ifnull(a.allowed_to_quote,0) allowed_to_quote, 
ifnull(a.allowed_to_quote_2,0) allowed_to_quote_2, 
ifnull(a.expected_value,0) expected_value,  
ifnull(c.uses_quote_process,0) uses_quote_process, 
ifnull(c.quote_level_2_start,0) quote_level_2_start, 
ifnull(c.quote_level_3_start,0) quote_level_3_start,  
d.status, c.id business_unit_id, a.quoted_by, a.status_id, 
e.customer_name, GET_BM(a.business_unit_id) bm, 
f.member_fullname fullname, a.currency 
FROM quote_master a 
LEFT JOIN quote_pricetype b ON a.pricetype_id = b.id  
INNER JOIN business_unit c on a.business_unit_id = c.id  
INNER JOIN quote_status d on a.status_id = d.id  
LEFT JOIN quote_schedule q on a.quote_id = q.quoteid  
LEFT JOIN customer e ON a.customer_id = e.customer_id 
LEFT JOIN member f ON a.quoted_by = f.member_id  
WHERE a.active_revision = true  
AND (a.status_id = 10 or ((a.status_id = 10) or (a.status_id in(11,12,13,4) and (q.id!=0))))  
and ( q.estimator=@v1  or q.pointperson=@v1  
or q.stage1screening_mid=@v1  or q.schedule_produced_mid=@v1   
or q.manpower_information_collected_mid=@v1   
or q.finance_info_collected_mid=@v1   
or q.customer_info_collected_mid=@v1   
or q.market_info_collected_mid=@v1   
or q.recon_report_created_mid=@v1  
or q.quote_delivery_strategy_mid=@v1   
or q.project_estimated_mid=@v1   
or q.worksheet_review_mid=@v1  
or q.stage6_final_review_mid=@v1   
or q.worksheet_review_mid=@v1   
or q.quote_delivered_mid=@v1   
or q.followup1_mid=@v1   
or q.followup2_mid=@v1   
or q.convert_or_kill_mid=@v1  
or q.post_mortem_complete_mid=@v1  
or q.rt1 = @v1  or q.rt2 = @v1   
or q.rt3 = @v1  or q.rt4 = @v1 ) 
and ifnull(c.uses_quote_process,0)=1 and a.quoted_price>ifnull(c.quote_level_2_start,0) 
ORDER BY a.open_date ",
            new object[] { 0, member_id });

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow _dr in dt.Rows)
                {
                    var this_id = _dr["quote_id"].ToString();
                    var this_date_open = Convert.ToDateTime(_dr["open_date"]);
                    var this_quoted_price = _dr["quoted_price"].ToString();
                    var this_custname = "";
                    var this_price_to = _dr["price_to"].ToString();
                    var this_pricetype_id = _dr["pricetype_id"].ToString();
                    var this_pricetype = _dr["pricetype"].ToString();
                    var this_allowed_to_quote = Toolbox.ReturnZeroIfNull_double(_dr["allowed_to_quote"]);
                    var this_allowed_to_quote_2 = Toolbox.ReturnZeroIfNull_double(_dr["allowed_to_quote_2"]);
                    var this_expected_value = Convert.ToDouble(_dr["expected_value"]);
                    var this_uses_quote_process = _dr["uses_quote_process"].ToString();
                    var this_quote_level_2_start = Toolbox.ReturnZeroIfNull_double(_dr["quote_level_2_start"]);
                    var this_quote_level_3_start = Toolbox.ReturnZeroIfNull_double(_dr["quote_level_3_start"]);
                    var this_company = _dr["business_unit_id"].ToString();
                    var status_id = Toolbox.ReturnZeroIfNull_int(_dr["status_id"]);
                    var status = _dr["status"].ToString();
                    var this_quotedby = _dr["quoted_by"].ToString();
                    var this_revision = _dr["revision"].ToString();
                    var customer_id = _dr["customer_id"].ToString();
                    var this_description = _dr["job_description"].ToString();
                    var row_message = "";
                    var fullname = _dr["fullname"].ToString();
                    var currency = Toolbox.ReturnZeroIfNull_int(_dr["currency"]);
                    this_quoted_price = this_pricetype_id == "6"
                    ? _tools.Monetize(this_quoted_price) + " to " + _tools.Monetize(this_price_to)
                    : _tools.Monetize(this_quoted_price);
                    this_custname = _dr["customer_name"].ToString();
                    var bm = Convert.ToInt32(_dr["bm"]);
                    var qs = new NeQuoteSchedule(this_id);
                    var is_supervisor = NeMember.is_supervisor(bm, member_id);
                    var is_bm = member_id == bm;
                    var is_level_2 = (this_expected_value >= this_quote_level_2_start) && (this_expected_value < this_quote_level_3_start);
                    var is_level_3 = (this_expected_value >= this_quote_level_3_start);

                    var _date = DateTime.Now;
                    if (qs.id != 0)
                    {
                        var is_scheduled_producer = member_id == qs.schedule_produced_mid;
                        var blank_schedule_date = qs.schedule_produced_date_completed == null;
                        var is_manpower_info_collector = member_id == qs.manpower_information_collected_mid;
                        var blank_manpower_date = qs.manpower_information_collected_date_completed == null;
                        var is_market_info_collector = member_id == qs.market_info_collected_mid;
                        var blank_market_date = qs.market_info_collected_date_completed == null;
                        var is_customer_info_collector = member_id == qs.customer_info_collected_mid;
                        var blank_customer_info_date = qs.customer_info_collected_date_completed == null;
                        var is_finance_info_collector = member_id == qs.finance_info_collected_mid;
                        var blank_finance_info_date = qs.finance_info_collected_date_completed == null;
                        var is_recon_report_creator = member_id == qs.recon_report_created_mid;
                        var blank_recon_report_date = qs.recon_report_created_date_completed == null;
                        var is_followup1_creator = member_id == qs.followup1_mid;
                        var followup1_date = (qs.followup1_date - DateTime.Now).TotalDays <= 30;
                        var is_followup2_creator = member_id == qs.followup2_mid;
                        var followup2_date = (qs.followup2_date - DateTime.Now).TotalDays <= 30;

                        if ((is_level_2 && status_id == 10 && is_bm) ||  // if its a level 2 and its a branch manager 
                        (is_level_3 && status_id == 10 && is_supervisor))
                        {
                            row_message = "Waiting for stage 1 approval from you.";
                            _date = Convert.ToDateTime(this_date_open);
                        }
                        else if (status_id == 11)
                        {
                            if (is_scheduled_producer && blank_schedule_date)
                            {
                                row_message = "Waiting for you to produce and distribute the schedule.";
                                _date = qs.schedule_produced_date;
                            }
                            else if (is_manpower_info_collector && blank_manpower_date)
                            {
                                row_message = "Waiting for you to complete the manpower info recon";
                                _date = qs.manpower_information_collected_date;
                            }
                            else if (is_market_info_collector && blank_market_date)
                            {
                                row_message = "Waiting for you to complete the market info recon";
                                _date = qs.market_info_collected_date;
                            }
                            else if (is_customer_info_collector && blank_customer_info_date)
                            {
                                row_message = "Waiting for you to complete the customer recon";
                                _date = qs.customer_info_collected_date;
                            }
                            else if (is_finance_info_collector && blank_finance_info_date)
                            {
                                row_message = "Waiting for you to complete the finance recon";
                                _date = qs.finance_info_collected_date;
                            }
                            else if (is_recon_report_creator && blank_recon_report_date)
                            {
                                row_message = "Waiting for you to review and approve all the recon stuff";
                                _date = qs.recon_report_created_date;
                            }
                            else if (is_followup1_creator && followup1_date)
                            {
                                row_message = "Follow Up 1 date coming up";
                                _date = qs.followup1_date;
                            }
                            else if (is_followup2_creator && followup2_date)
                            {
                                row_message = "Follow Up 2 date coming up";
                                _date = qs.followup2_date;
                            }
                            else if (!blank_recon_report_date)
                            {
                                if (((member_id == qs.rt1) && (qs.rt1_s4_approved == null)) || (member_id == qs.rt2 && qs.rt2_s4_approved == null) || ((member_id == qs.rt3) && (qs.rt3_s4_approved == null)) || ((member_id == qs.rt4) && (qs.rt4_s4_approved == null)))
                                {
                                    row_message = "Waiting for you to review the recon info and approve it through stage 4";
                                    _date = qs.stage4_gono_date;
                                }
                            }
                        }
                        else if (status_id == 12)
                        {
                            if ((member_id == qs.quote_delivery_strategy_mid) && (qs.quote_delivery_strategy_date_completed == null))
                            {
                                row_message = "Waiting for you to finish the sell strategy";
                                _date = qs.quote_delivery_strategy_date;
                            }
                            else if ((member_id == qs.project_estimated_mid) && (qs.project_estimated_date_completed == null))
                            {
                                row_message = "Waiting for you to estimate the project";
                                _date = qs.project_estimated_date;
                            }
                            else if ((member_id == qs.worksheet_review_mid) && (qs.project_estimated_date_completed != null) && (qs.worksheet_review_date_completed == null))
                            {
                                row_message = "Waiting for you to complete the worksheet review";
                                _date = qs.worksheet_review_date;
                            }
                            else if (qs.worksheet_review_date_completed != null)
                            {
                                if (is_level_3)
                                {
                                    if (((member_id == qs.rt1) && (qs.rt1_f_approved == null)) || ((member_id == qs.rt2) && (qs.rt2_f_approved == null)) || ((member_id == qs.rt3) && (qs.rt3_f_approved == null)) || ((member_id == qs.rt4) && (qs.rt4_f_approved == null)))
                                    {
                                        row_message = "Waiting for you to give the final review";
                                        _date = qs.stage6_final_review_date;
                                    }
                                }
                                else if (is_level_2)
                                {
                                    if (is_bm)
                                    {
                                        if (((member_id == qs.rt1) && (qs.rt1_f_approved == null)) || ((member_id == qs.rt2) && (qs.rt2_f_approved == null)) || ((member_id == qs.rt3) && (qs.rt3_f_approved == null)) || ((member_id == qs.rt4) && (qs.rt4_f_approved == null)))
                                        {
                                            row_message = "Waiting for you to give the final review";
                                            _date = qs.stage6_final_review_date;
                                        }
                                    }
                                }
                            }
                        }
                        else if (status_id == 4)
                        {
                            if ((member_id == qs.quote_delivered_mid) && (qs.quote_delivered_date_completed == null))
                            {
                                row_message = "Waiting for you to deliver the quote";
                                _date = qs.quote_delivered_date;
                            }
                            else if ((member_id == qs.convert_or_kill_mid) && (qs.convert_or_kill_date_completed == null))
                            {
                                row_message = "Waiting for you to covert or kill the quote";
                                _date = qs.convert_or_kill_date;
                            }
                        }
                        else if (status_id == 13)
                        {
                            if ((member_id == qs.post_mortem_complete_mid) && (qs.post_mortem_complete_date_completed == null))
                            {
                                row_message = "Waiting for you to complete the post mortem";
                                _date = qs.post_mortem_complete_date;
                            }
                        }
                    }
                    else
                    {
                        if ((is_level_2 && (status_id == 10) && is_bm) ||  // if its a level 2 and its a branch manager 
                        (is_level_3 && (status_id == 10) && is_supervisor))
                        {
                            row_message = "Waiting for your initial approval to allow the quote to begin";
                            _date = qs.stage1screening_date;
                        }
                    }

                    if (row_message != "")
                    {
                        /* 
                            dt_final.Columns.Add(new DataColumn("link")); 
                        dt_final.Columns.Add(new DataColumn("description")); 
                        dt_final.Columns.Add(new DataColumn("overdue")); 
                        dt_final.Columns.Add(new DataColumn("star")); 
                        dt_final.Columns.Add(new DataColumn("link2")); 
                        dt_final.Columns.Add(new DataColumn("quote_id")); 
                        dt_final.Columns.Add(new DataColumn("is_level_2")); 
                        dt_final.Columns.Add(new DataColumn("is_level_3")); 
                        dt_final.Columns.Add(new DataColumn("row_message")); 
                        dt_final.Columns.Add(new DataColumn("customer_name")); 
                        dt_final.Columns.Add(new DataColumn("this_date_open")); 
                        dt_final.Columns.Add(new DataColumn("this_fullname")); 
                         dt_final.Columns.Add(new DataColumn("this_description")); 

                         */
                        dt_final.Rows.Add("/#/opens/65/quotes/" + this_id + "/" + this_revision,
                        row_message,
                        _tools.getSQL_string("Select fun_time(@v0)", new object[] { _date.ToString("yyyy-MM-dd HH:mm:ss") }),
                        "",
                        "Quote for " + this_custname + "- " + this_description,
                        this_id,
                        is_level_2,
                        is_level_3,
                        row_message,
                        this_custname,
                        this_date_open,
                        fullname,
                        this_description,
                        this_revision,
                        "65/quote",
                        $@"{this_id},{this_revision}"
                        );
                    }
                }
            }
            return dt_final;
        }
        public int Get_Quote_Price_Code(string pricecode_string)
        {
            switch (pricecode_string)
            {
                case "Budget Price":
                    return 1;
                case "Price Each":
                    return 2;
                case "Budget Price Each":
                    return 3;
                case "Price / hr":
                    return 4;
                case "Not To Exceed Price":
                    return 5;
                case "Price From - To":
                    return 6;
                default:
                    return 0;
            }
        }
        public static double quoted_amount(MySqlConnection _conn, int _id)
        {
            return Toolbox.doSQL_double(_conn, @"SELECT IFNULL(MAX(quoted_price), 0) FROM quote_master WHERE quote_id = @v0  AND active_revision = 1", new object[] { _id }); ;
        }
        public bool ReceiveNeQuotes(int QuoteNo, int WO, string txtpo, int member_id, int contactid, string div)
        {
            // update the quote table 
            var messagebody = "";
            var membertemp = new NeMember(member_id);
            var wo = new NeWOProg(WO);
            if (QuoteNo > 100000)
            {
                var _tools = new Toolbox();
                // update the quote table to show the quote is alive again 
                var rows = _tools.getSQL_affectedrows(@"UPDATE quote  SET status_id = 8, prev_status_id = 4, wo =@v0, killed_by = NULL, po =@v1 , pct_chance = 100   where quote_n =@v2", new object[] { WO, txtpo, QuoteNo });
                if (WO != 0)
                {
                    try
                    {
                        messagebody = WO + " was run and " + rows + " were updated";
                        var quote = new quote(QuoteNo);
                        _tools.getSQL_void(@"INSERT INTO event_table (dateoccured,event_text,member_id) VALUES(now(),@v0,@v1)",
                        new object[] { messagebody, member_id });
                        // Move quote folder over to work order 
                        new NeFiles().CreateFolder(WO, "workorder", wo.business_unit_id);
                    }
                    catch (Exception ex)
                    {
                        _tools.catch_error(ex);
                        //throw new Exception(ex.ToString()); 
                    }
                }
            }
            return true;
        }
        /// <summary> 
        /// The idea here is we have keyed, in a lot of places, that the quote # is quote_id+rev... so 1000009 for quote_id 100000 and rev 9 <br/> 
        /// We need to take the _quoteno, and splice it into two variables _quote_id, and _rev 
        /// </summary> 
        /// <param name="_quoteno"></param> 
        /// <param name="_quote_id"></param> 
        /// <param name="_rev"></param> 
        public static void splice(object _quoteno, out int _quote_id, out int _rev)
        {
            var q = _quoteno is string ? (string)_quoteno : _quoteno.ToString(); // No matter what, turn it into a string 
            var l = q.Length; // We need this because a 6, or 7, or 8 character _quoteno could be provided... only can work with 7 or 8. 
            if (l <= 6) throw new ArgumentOutOfRangeException("_quoteno", "The quote number length is too short, cannot proceed");
            int.TryParse(q.Substring(0, 6), out _quote_id);
            if (l == 7) // Covers all revisions from 1 to 9 
            {
                int.TryParse(q.Substring(6, 1), out _rev);
            }
            else // Covers revisions 10 - 99 -- If it ever gets higher than 99, I will eat my hat. 
            {
                int.TryParse(q.Substring(6, 2), out _rev);
            }
        }
        public static void unattach(int _quote_id, int _rev)
        {
            Toolbox.doSQL_void(@" 
UPDATE  
quote_master  
SET  
status_id = 4,  
prev_status_id = 8,  
wo = NULL,  
killed_by = NULL,  
po = '',  
pct_chance = 30 
WHERE  
quote_id = @v0 AND  
revision = @v1 AND 
active_revision = true", new object[] {
_quote_id, _rev});
        }
        public static void add_history(MySqlConnection _conn, int _created_by, int _quote_id, int _rev, string _event)
        {
            Toolbox.doSQL_void(_conn, @" 
INSERT INTO quote_history  
( 
create_datetime,  
created_by,  
quote_id,  
revision,  
event 
)  
VALUES 
( 
NOW(), 
@v0,  
@v1,  
@v2,  
@v3 
)", new object[] {
_created_by, _quote_id, _rev, _event});
        }
        public string GetQuoteRev(int QuoteNo)
        {
            var quoterev = "0";
            if (QuoteNo < 100000)
            {
                var conn = NeDB.getConAccess(@"\\ne-vserver-04\branches\neoakville\new electric software\quote\quotes_be.mdb");
                var strWO = "select quoterev,Revision  from tblQuotesNew where tblQuotesNew.Quote = " + QuoteNo + " Order by tblQuotesnew.Quote Desc";
                var comWODetailList = new OleDbCommand(strWO, conn);
                var drWODetailList = comWODetailList.ExecuteReader();
                while (drWODetailList.Read())
                {
                    quoterev = drWODetailList.GetValue(0).ToString() + " " + drWODetailList.GetValue(1);
                }
                conn.Close();
            }
            else
            {
                quoterev = QuoteNo.ToString();
            }
            return quoterev;
        }
        public void AddNewWorkSheetLine()
        {
            Toolbox.doSQL_void(@" 
INSERT INTO quote_worksheet  
( 
quote_id,  
revision,  
section_id,  
part_no,  
code,  
description,  
sell,  
cost,  
extended_per,  
original_sell,  
qty,member_id,  
ts,  
quote_worksheet_discount, 
consignment_id, 
cost_level 
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
@v9,   
@v10,   
@v11,    
NOW(),   
@v12,  
@v13, 
@v14 
)", new object[] {

worksheetquote_id,
worksheet_rev,
worksheet_section_id,
worksheet_partno,
Toolbox.do_value_from(worksheet_code, false),
worksheet_description,
worksheet_sell,
worksheet_cost,
worksheet_extendedper,
worksheet_originalsell,
worksheet_qty,
worksheet_member_id,
quote_worksheet_discount,
worksheet_consignment_id,
cost_level
});
        }
        public void UpdateWorkSheetLine()
        {
            var tools = new Toolbox();
            var sql = @"UPDATE quote_worksheet 
SET section_id=@v0, 
part_no=@v1, 
description=@v2, 
code=@v3, 
sell=@v4, 
cost=@v5, 
extended_per=@v6, 
original_sell=@v7, 
qty=@v8, 
member_id=@v9, 
quote_worksheet_part_requested=@v10, 
quote_worksheet_discount=@v11  
WHERE id=@v12";

            var paramObjects = new object[]
            {
worksheet_section_id,
Toolbox.do_value_from(worksheet_partno,false),
Toolbox.do_value_from(worksheet_description,false),
Toolbox.do_value_from(worksheet_code,false),
worksheet_sell,
worksheet_cost,
worksheet_extendedper,
worksheet_originalsell,
worksheet_qty,
worksheet_member_id,
quote_worksheet_part_requested,
quote_worksheet_discount,
worksheet_id
            };
            tools.getSQL_void(sql, paramObjects);
        }
    }
}