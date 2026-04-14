using System;
using System.Data;
using System.Globalization;
using System.Linq;
using DevExpress.Web;
using MySql.Data.MySqlClient;
using NESI.Common.Models;

namespace nesi.core
{
    /// <summary>
    /// Summary description for NeMemberTime
    /// </summary>
    public class NeMemberTime
    {
        /*
  MemberTime_ID int(11)   No  auto_increment		       
  membertime_memberid int(11)   No				 
  Date date   No				 
  MemberTime_WorkOrder_ID varchar(15) latin1_swedish_ci  No				 
  MemberTime_WorkOrder_Name varchar(500) latin1_swedish_ci  No				 
  NumberofHours float   No				 
  MemberTime_MemberTypeHours_ID int(11)   Yes NULL				
  MemberTime_PayTypeHours_ID int(11)   No				 
  MemberTime_WoComment_ID varchar(15) latin1_swedish_ci  Yes NULL				
  MemberTime_WoComment varchar(500) latin1_swedish_ci  No				 
  Member_ID_Audit int(11)   No				 
  Created_Date datetime   Yes NULL				
  Modified_Date */

        private string _Date = "";
        private int _MemberTime_MemberTypeHours_ID;
        private int _MemberTime_WoComment_Child_ID = 0;
        private int _MemberTime_Child_CompanyID = 0;

        public double Miles { get; set; }

        public string MemberTime_SRED { get; set; }

        public string MemberTime_Mileage { get; set; }

        public string MemberTime_Warranty { get; set; }

        public string MemberTime_Premium { get; set; }

        public int ID { get; set; }

        public int MemberTimeID { get { return ID; } set { ID = value; } }
        public int member_id { get; set; }

        public int membertime_memberid { get; set; }
        public int business_unit_id { get; set; }
        public int membertime_shop_type_id { get; set; }
        public int internal_project_id { get; set; }
        public int MemberTime_ChildCompanyID { get { return _MemberTime_Child_CompanyID; } set { _MemberTime_Child_CompanyID = value; } }
        public int PayTypeHoursID { get; set; }

        public int MemberTimePayTypeHoursID { get { return PayTypeHoursID; } set { PayTypeHoursID = value; } }
        public string Date { get { return _Date; } set { _Date = value; } }
        public string WorkOrderID { get; set; }

        public string MemberTimeWorkOrderID { get { return WorkOrderID; } set { WorkOrderID = value; } }
        public string ChildWorkOrderID { get; set; }

        public string MemberTimeChildWorkOrderID { get { return ChildWorkOrderID; } set { ChildWorkOrderID = value; } }
        public string CustomerName { get; set; }

        public string MemberTimeCustomerName { get { return CustomerName; } set { CustomerName = value; } }
        public string ChildCustomerName { get; set; }

        public string MemberTimeChildCustomerName { get { return ChildCustomerName; } set { ChildCustomerName = value; } }
        public double NumberOfHours { get; set; }

        public int WoCommentID { get; set; }

        public int MemberTimeWoCommentID { get { return WoCommentID; } set { WoCommentID = value; } }
        public int MemberTimeWoCommentChildID { get { return _MemberTime_WoComment_Child_ID; } set { _MemberTime_WoComment_Child_ID = value; } }
        public string WoComment { get; set; }

        public string MemberTimeWoComment { get { return WoComment; } set { WoComment = value; } }
        public int MemberIDAudit { get; set; }

        public int MemberIDCreate { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }

        public string Cust_No { get; set; }

        public string MemberTime_Cust_No { get { return Cust_No; } set { Cust_No = value; } }
        public string Child_Cust_No { get; set; }

        public string MemberTime_Child_Cust_No { get { return Child_Cust_No; } set { Child_Cust_No = value; } }
        public string WOType { get; set; }
        public double MemberTime_CostPrice { get; set; }
        public double MemberTime_SellPrice { get; set; }

        public string Memo { get; set; }

        public string ProductCode { get; set; }

        public double wo_percent_complete { get; set; }

        public int woprog_id { get; set; }

        public int membertime_woprog_id { get { return woprog_id; } set { woprog_id = value; } }
        public int child_woprog_id { get; set; }
        public int membertime_child_woprog_id { get { return child_woprog_id; } set { child_woprog_id = value; } }
        public int customer_id { get; set; }
        public int child_customer_id { get; set; }

        public bool child_shoptime { get; set; }

        public int membertype_id { get; set; }
        public int membertype_chargeout_id { get; set; }
        public int rating { get; set; }
        public int tsLitePayType { get; set; }
        public int? scope_id { get; set; }
        public int? prov_id { get; set; }
        public decimal? mileage_value { get; set; }
        public decimal old_mileage_value { get; set; }
        public string mileage_unit { get; set; }
        public bool is_prevailing_wage { get; set; }

        public NeMemberTime()
        {
            child_shoptime = false;
            customer_id = 0;
            Child_Cust_No = null;
            ChildCustomerName = null;
            ChildWorkOrderID = null;
        }

        public NeMemberTime(string dayoff_type, int dayoff_id)
        {
            child_shoptime = false;
            customer_id = 0;
            Child_Cust_No = null;
            ChildCustomerName = null;
            ChildWorkOrderID = null;
            var mt_id = 0;
            var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM membertime WHERE wotype = @v0  AND membertime_workorder_id = @v1 ", new object[] { dayoff_type, dayoff_id });
            if (c == 1)
            {
                mt_id = Toolbox.doSQL_int(@"SELECT membertime_id FROM membertime WHERE wotype = @v0  AND membertime_workorder_id = @v1 ", new object[] { dayoff_type, dayoff_id });
                init(mt_id);
            }
            else if (c > 0)
            {
                throw new Exception("Multiple entries exist for this 'day off' entry, this should not happen.");
            }
        }
        public NeMemberTime(int membertime_id, MySqlConnection connection = null)
        {
            child_shoptime = false;
            customer_id = 0;
            Child_Cust_No = null;
            ChildCustomerName = null;
            ChildWorkOrderID = null;
            init(Convert.ToInt32(membertime_id), connection);
        }

        public static int wo_line_id(object _member_id, object _paytype_id, object _woprog_id)
        {
            return Toolbox.doSQL_int(@"SELECT IFNULL(MAX(wo_detail_current_id), 0) FROM wo_detail_current WHERE memberid = @v0  AND paytypeid = @v1  AND wo_detail_current_woprog_id = @v2  ", new object[] { _member_id, _paytype_id, _woprog_id });
        }
        private void init(int _id, MySqlConnection connection = null)
        {
            var _tools = new Toolbox();
            DataTable dt;
            if (connection == null)
                dt = Toolbox.doSQL_dt(@"SELECT * FROM membertime WHERE membertime_id = @v0 ", new object[] { _id });
            else
                dt = Toolbox.doSQL_dt(connection, @"SELECT * FROM membertime WHERE membertime_id = @v0 ", new object[] { _id });
            foreach (DataRow dr in dt.Rows)
            {
                var hold = "";
                ID = _id;
                member_id = Convert.ToInt32(dr["membertime_memberid"]);
                _Date = dr["date"].ToString();
                WorkOrderID = dr["membertime_workorder_id"].ToString();
                Cust_No = dr["membertime_cust_no"].ToString();
                membertime_shop_type_id = Toolbox.ReturnZeroIfNull_int(dr["membertime_shop_type_id"]);
                NumberOfHours = Convert.ToDouble(dr["numberofhours"]);
                _MemberTime_MemberTypeHours_ID = Convert.ToInt32(dr["membertime_membertypehours_id"]);
                PayTypeHoursID = Convert.ToInt32(dr["membertime_paytypehours_id"]);
                WoCommentID = Convert.ToInt32(dr["membertime_wocomment_id"]);
                CustomerName = dr["membertime_customer_name"].ToString().Replace("''", "'");
                MemberTime_Mileage = Convert.ToInt32(dr["membertime_paytypehours_id"]) == OpsPayType.Mileage ? "true" : "false";
                CreatedDate = dr["created_date"].ToString();
                WOType = dr["wotype"].ToString();
                MemberTime_Premium = dr["membertime_premium"].ToString();
                business_unit_id = (int)dr["business_unit_id"];
                ChildWorkOrderID = dr["membertime_child_workorder_id"].ToString();
                Child_Cust_No = dr["membertime_child_cust_no"].ToString();
                woprog_id = Toolbox.ReturnZeroIfNull_int(dr["membertime_woprog_id"]);
                child_woprog_id = Toolbox.ReturnZeroIfNull_int(dr["membertime_child_woprog_id"]);
                wo_percent_complete = Toolbox.ReturnZeroIfNull_double(dr["wo_percent_complete"]);
                membertype_id = Convert.ToInt32(dr["membertype_id"]);
                membertype_chargeout_id = Convert.ToInt32(dr["membertype_chargeout_id"]);
                child_shoptime = Convert.ToBoolean(dr["child_shoptime"]);
                customer_id = Toolbox.ReturnZeroIfNull_int(dr["membertime_customer_id"]);
                ChildCustomerName = dr["membertime_child_customer_name"].ToString().Replace("''", "'");
                MemberTime_CostPrice = Convert.ToDouble(dr["MemberTime_CostPrice"]);
                MemberTime_SellPrice = Convert.ToDouble(dr["MemberTime_SellPrice"]);
                rating = Convert.ToInt16(dr["rating"]);
                mileage_value = Toolbox.ReturnZeroIfNull_decimal(dr["mileage_value"], 0);
                mileage_unit = Toolbox.ReturnBlankIfNull_string(dr["mileage_unit"]);
                old_mileage_value = Toolbox.ReturnZeroIfNull_decimal(dr["mileage_value"], 0);

                int.TryParse(Toolbox.ReturnBlankIfNull_string(dr["membertime_child_companyid"]), out _MemberTime_Child_CompanyID);
                int.TryParse(Toolbox.ReturnBlankIfNull_string(dr["membertime_wocomment_child_id"]), out _MemberTime_WoComment_Child_ID);
            }
        }
		public static double TotalPayTypeHours(int woprogId, int memberId, int paytypeId)
			{
			return Toolbox.doSQL_double(@"
				SELECT 
					IFNULL(SUM(numberofhours),0) 
				FROM 
					membertime 
				WHERE 
					membertime_woprog_id = @v0 AND 
					membertime_memberid = @v1 AND 
					membertime_paytypehours_id = @v2", new object[] { woprogId, memberId, paytypeId });
			}
        public static double get_entered_time(int _member_id, DateTime _dt)
        {
            return Toolbox.doSQL_double(@"SELECT IFNULL(SUM(numberofhours),0) FROM membertime WHERE membertime_memberid = @v0  AND date = @v1 ", new object[] { _member_id, Toolbox.MySQL_shortdt(_dt) });
        }
        public static double HoursSpent_onQuote(int QuoteID)
        {
            return Toolbox.doSQL_double(@"SELECT IFNULL(Sum(NumberofHours), 0) AS SumOfNumberofHours FROM membertime GROUP BY WOType,Membertime_WorkOrder_ID HAVING WOType='Quote' and Membertime_WorkOrder_ID =@v0", new object[] { QuoteID });
        }
        public struct time_entry_vars
        {
            // WO (1) / Quote (2)
            public int entrytype { get; set; }
            public int membertime_id { get; set; }
            public string woprog_bvwo { get; set; }
            public string parent_woprog_bvwo { get; set; }
            public NeMember loaded_user { get; set; }
            public NeMember entering_user { get; set; }
            public string comment { get; set; }
            public double hours { get; set; }
            public int paytype_id { get; set; }
            public bool is_child_wo { get; set; }
            public int parent_business_unit_id { get; set; }
            public int selected_business_unit_id { get; set; }
            public string date { get; set; }
            public double old_hours { get; set; }
            public int old_comment_id { get; set; }
            public int selected_comment_id { get; set; }
            public string selected_comment_text { get; set; }
            public string todays_date { get; set; }
            public string customer_name { get; set; }
            public string parent_customer_name { get; set; }
            public bool is_shop_time { get; set; }
            public string parent_customer_number { get; set; }
            public string customer_number { get; set; }
            public double percent_done { get; set; }
            public NeWOProg wo { get; set; }
            public NeWOProg parent_wo { get; set; }
            public int woprog_id { get; set; }
            public int parent_woprog_id { get; set; }
            public double cost_price { get; set; }
            public double sell_price { get; set; }
            public int master_id { get; set; }
            public int rating { get; set; }

            public bool do_advancement { get; set; }
            public bool do_uncertainty { get; set; }
            public string advancement { get; set; }
            public string uncertainty { get; set; }

            public string bizdev_calls { get; set; }
            public string bizdev_faxes { get; set; }
            public string bizdev_emails { get; set; }
            public string bizdev_mailers { get; set; }
            public string bizdev_meetings { get; set; }
            public string bizdev_quoteopps { get; set; }
            public TokenCollection jobTags { get; set; }
            public int tsLitePayType { get; set; }
            public int? internal_project_id { get; set; }

            public int membertype_id { get; set; }
            public int? scope_id { get; set; }
            public int? prov_id { get; set; }

            public decimal? mileage_value { get; set; }
            public decimal oldmileage_value { get; set; }
            
            public string mileage_unit { get; set; }

            // For transfer used only.
            public bool IsTransfer { get; set; }
            public string transferInfo { get; set; }
            public int originalChargeoutId { get; set; }
            public double OriginalPrice { get; set; }
            public double OriginalCost { get; set; }    
            public int originalMemberTypeid { get; set; }
            public int IdForTransfer { get; set; }
            public int orginalChargeoutIdForParentWorkOrderLaborLIine { get; set; }
            public bool orginal_branch_can_see_jobtype { get; set; }
            public bool is_prevailing_wage { get; set; }
        }
        private static void handleTags(MySqlConnection _conn, time_entry_vars _tev, MySqlTransaction transaction = null)
        {
            #region Tokens
            var businessUnit = new NeBusinessUnit(_tev.selected_business_unit_id);
            if (_tev.jobTags != null)
            {
                var myTI = new CultureInfo("en-US", false).TextInfo;
                foreach (var t in _tev.jobTags)
                {
                    if (Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) FROM membertime_tags WHERE tag = @v0 AND tax_entity_id = @v1", new object[] { myTI.ToTitleCase(t.Trim()), businessUnit.tax_entity_id }) == 0)
                    {
                        if (transaction == null)
                            Toolbox.doSQL_void(_conn, @"INSERT INTO membertime_tags (type, tag,tax_entity_id,created_by, created_when) VALUES (@v0, @v1, @v2, @v3, NOW())", new object[] { _tev.entrytype, t, businessUnit.tax_entity_id, _tev.entering_user.id });
                        else
                            Toolbox.doSQL_void(_conn, @"INSERT INTO membertime_tags (type, tag,tax_entity_id,created_by, created_when) VALUES (@v0, @v1, @v2, @v3, NOW())", new object[] { _tev.entrytype, t, businessUnit.tax_entity_id, _tev.entering_user.id }, transaction);
                    }
                }
            }
            #endregion
        }
        public static void time_entry_create(ref time_entry_vars tev)
        {
            using (var conn = Toolbox.connect())
            {
                // Entrytypes - WO = 1, Quote = 2, BizDev = 3, Shop = 4
                var mt_obj = new NeMemberTime();
                mt_obj.tsLitePayType = tev.tsLitePayType;
                mt_obj.scope_id = tev.scope_id;
                mt_obj.prov_id = tev.prov_id;
                mt_obj.is_prevailing_wage = tev.is_prevailing_wage;
                var wo_comments_obj = new NeWOComments();
                var commentid = 0;
                var commentchildid = 0;
                //var businessUnit = new NeBusinessUnit(tev.selected_business_unit_id);
                using (var transaction = conn.BeginTransaction())
                {
                    handleTags(conn, tev, transaction);
                    if (tev.entrytype == 1 || tev.entrytype == 2)
                    {

                        #region WO / Quotes
                        #region comments
                        wo_comments_obj.WorkOrderID = tev.woprog_bvwo;
                        wo_comments_obj.WoComment_Member_ID = tev.loaded_user.id;
                        wo_comments_obj.Comments = tev.comment;
                        if (tev.paytype_id == OpsPayType.Mileage)
                        {
                            mt_obj.mileage_value = tev.mileage_value;
                            mt_obj.mileage_unit = tev.mileage_unit;
                        }
                        mt_obj.WOType = "WO";
                        if (tev.entrytype == 2)
                        {
                            wo_comments_obj.Comments = "Quoted Job.  See Quote Manager for details.";
                            mt_obj.WOType = "Quote";
                        }
                        wo_comments_obj.HoursWorked = tev.hours.ToString();
                        if (tev.is_child_wo)
                        {
                            wo_comments_obj.WorkOrderChildID = tev.parent_wo.OrderNumber;
                            wo_comments_obj.company_child_id = tev.parent_wo.business_unit_id;
                        }
                        wo_comments_obj.Personal = 0;
                        wo_comments_obj.PrintComments = 0;
                        wo_comments_obj.business_unit_id = tev.selected_business_unit_id;
                        wo_comments_obj.CreatedDate = tev.date;
                        wo_comments_obj.EntryDate = tev.date;

                        wo_comments_obj.woprog_id = tev.entrytype == 1 ? tev.wo.woprog_id : 0;

                        if (tev.selected_comment_id == 0 || tev.comment != "")
                        {
                            try
                            {
                                wo_comments_obj.AddWOComment(tev.loaded_user.id, 0, out commentid, out commentchildid, false, wo_comments_obj.woprog_id, conn, transaction);
                            }
                            catch (Exception ex)
                            {
                                new Toolbox().catch_error(ex);
                                transaction.Rollback();
                                throw new Exception("Error adding comment to DB");
                            }
                        }
                        else
                        {
                            mt_obj.MemberTimeWoCommentID = tev.selected_comment_id;
                            try
                            {
                                wo_comments_obj.Comments = tev.selected_comment_text;
                                try
                                {
                                    wo_comments_obj.woprog_id = tev.entrytype == 1 ? tev.wo.woprog_id : 0;
                                }
                                catch
                                {
                                    wo_comments_obj.woprog_id = 0;
                                }
                                wo_comments_obj.AddWOComment(tev.loaded_user.id, 0, out commentid, out commentchildid, true, wo_comments_obj.woprog_id, conn, transaction);
                                wo_comments_obj.AddWOComment(tev.loaded_user.id, 0, out commentid, out commentchildid, false, wo_comments_obj.woprog_id, conn, transaction);
                            }
                            catch (Exception ee)
                            {
                                new Toolbox().catch_error(ee);
                                transaction.Rollback();
                                throw new Exception("Error adding comment to DB");
                            }
                        }
                        #endregion comments

                        #region membertime prep
                        mt_obj.WoCommentID = commentid;
                        mt_obj.MemberTime_Premium = "False";
                        mt_obj.Date = tev.todays_date;
                        mt_obj.MemberTimeCustomerName = tev.customer_name;
                        mt_obj.MemberTimeWorkOrderID = tev.woprog_bvwo;
                        mt_obj.MemberTime_CostPrice = tev.paytype_id == OpsPayType.Mileage ? 0 : tev.cost_price;
                        mt_obj.MemberTime_SellPrice = tev.sell_price;
                        mt_obj.rating = tev.rating;

                        if (tev.is_child_wo)
                        {
                            // I realize it is confusing to have parent & child mixed here... but it flows with out current phrasing.
                            mt_obj.MemberTimeChildWorkOrderID = tev.parent_wo.OrderNumber;
                            mt_obj.MemberTimeChildCustomerName = tev.parent_wo.CustomerName;
                        }
                        if (tev.entrytype == 2)
                        {
                            mt_obj.MemberTime_Cust_No = tev.customer_number;
                            mt_obj.CustomerName = tev.customer_name;
                            mt_obj.WOType = "Quote";
                            mt_obj.membertime_woprog_id = 0;
                        }
                        else if (tev.is_child_wo && tev.is_shop_time)
                        {
                            mt_obj.MemberTime_Child_Cust_No = tev.parent_wo.CustomerNumber;
                            mt_obj.membertime_child_woprog_id = 0;
                            mt_obj.child_shoptime = true;
                            mt_obj.MemberTime_Cust_No = "0";
                            mt_obj.membertime_woprog_id = tev.wo.woprog_id;
                        }
                        else
                        {
                            try
                            {
                                mt_obj.membertime_woprog_id = tev.wo.woprog_id;
                            }
                            catch
                            {
                                mt_obj.membertime_woprog_id = 0;
                            }
                            mt_obj.MemberTime_Cust_No = tev.customer_number;
                            if (tev.is_child_wo)
                            {
                                mt_obj.MemberTime_Child_Cust_No = tev.parent_wo.CustomerNumber;
                                try
                                {
                                    mt_obj.membertime_child_woprog_id = tev.parent_wo.woprog_id;
                                }
                                catch (Exception ee)
                                {
                                    new Toolbox().catch_error(ee);
                                    transaction.Rollback();
                                    throw;
                                }
                            }
                            mt_obj.WOType = "WO";
                        }
                        mt_obj.wo_percent_complete = tev.percent_done;
                        mt_obj.NumberOfHours = tev.hours;
                        mt_obj.MemberTimePayTypeHoursID = tev.paytype_id;
                        mt_obj.membertime_memberid = tev.loaded_user.id;
                        mt_obj.MemberIDAudit = tev.entering_user.id;
                        mt_obj.CreatedDate = tev.date;
                        mt_obj.MemberIDCreate = tev.entering_user.id;
                        mt_obj.business_unit_id = tev.selected_business_unit_id;
                        if (tev.is_child_wo)
                        {
                            mt_obj.MemberTime_ChildCompanyID = tev.parent_wo.business_unit_id;
                        }

                        mt_obj.MemberTime_Mileage = tev.paytype_id == OpsPayType.Mileage ? "True" : "false";
                        mt_obj.MemberTime_Premium = "false";
                        mt_obj.membertype_id = tev.membertype_id;
                        mt_obj.membertype_chargeout_id = tev.master_id;
                        mt_obj.MemberTime_SRED = "false";
                        mt_obj.MemberTime_Warranty = "false";
                        mt_obj.MemberIDCreate = tev.entering_user.id;
                        mt_obj.customer_id = tev.wo.WOProg_Customer_ID;
                        mt_obj.rating = tev.rating;
                        mt_obj.member_id = tev.loaded_user.id32;

                        #endregion membertime prep

                        mt_obj.AddMemberTime(tev.loaded_user.id, conn, transaction);
                        NeWOProg.RefreshAndSaveServiceDatesText(mt_obj.membertime_woprog_id, conn, transaction);

                        #region R & D

                        if (tev.do_advancement && tev.do_uncertainty)
                        {
                            try
                            {
                                var rd = new NERandD();
                                if (tev.wo.chkRD)
                                {
                                    rd.RD_MemberTime_ID = mt_obj.MemberTimeID;
                                    rd.RD_Member_ID = mt_obj.membertime_memberid;
                                    rd.RD_ProjectAdvancement = tev.advancement;
                                    rd.RD_ProjectUncertainty = tev.uncertainty;
                                    rd.RD_WOProgID = mt_obj.membertime_woprog_id;
                                    rd.InsertNewEntry(conn, transaction);
                                }
                                if (tev.parent_wo.chkRD)
                                {
                                    rd.RD_MemberTime_ID = mt_obj.MemberTimeID;
                                    rd.RD_Member_ID = mt_obj.membertime_memberid;
                                    rd.RD_ProjectAdvancement = tev.advancement;
                                    rd.RD_ProjectUncertainty = tev.uncertainty;
                                    rd.RD_WOProgID = tev.parent_wo.woprog_id;
                                    rd.InsertNewEntry(conn, transaction);
                                }
                            }
                            catch (Exception ex)
                            {
                                new Toolbox().catch_error(ex);
                                transaction.Rollback();
                                throw;
                            }
                        }

                        #endregion R & D

                        #region update quote hours spent


                        if (tev.entrytype == 2)
                        {
                            Toolbox.doSQL_void(conn,
                                @"UPDATE quote_master LEFT JOIN vw_membertime_quote ON LEFT (quote_master.quote_id, 6) = vw_membertime_quote.MemberTime_WorkOrder_ID SET quote_master.hours_spent = ifnull(vw_membertime_quote.NumberOfHours,0)  WHERE quote_master.quote_id =@v0",
                                new object[] { tev.woprog_bvwo }, transaction);
                        }

                        #endregion


                        var charge_out_rate = tev.loaded_user.business_unit.allow_unlinked_timesheet ? 0 : tev.master_id != 1000000
                                ? Toolbox.doSQL_double(conn, @"CALL CUSTOMER_CHARGEOUT(@v0 ,@v1 )",
                                    new object[] { tev.wo.WOProg_Customer_ID, tev.master_id })
                                : 0;
                        var Paytype = Toolbox.doSQL_string(conn,
                            @"SELECT description FROM paytypehours WHERE paytypehours_id = @v0 ",
                            new object[] { tev.paytype_id });
                        if (tev.entrytype == 1 && !tev.loaded_user.business_unit.allow_unlinked_timesheet)
                        {
                            #region variable definition

                            var detail_obj = new NeWODetailCurrent();
                            var tax_labor = tev.loaded_user.business_unit.TaxLabour == 1;
                            var strLabor = tev.loaded_user.business_unit.country == "CDN" ? "Labour" : "Labor";


                            detail_obj = tev.paytype_id == OpsPayType.Mileage ? detail_obj : new NeWODetailCurrent(mt_obj.membertime_memberid, mt_obj.MemberTimePayTypeHoursID, mt_obj.membertime_woprog_id, tev.master_id);
                            detail_obj.woprog_id = mt_obj.membertime_woprog_id;
                            detail_obj.description = tev.paytype_id == OpsPayType.Mileage
                                                    ? string.Format("({1}) {0} traveled: {2} {3}", tev.loaded_user.FullName, Paytype, tev.mileage_value, tev.mileage_unit)
                                                    : string.Format("{0} Hours {1}: {2}", tev.loaded_user.FullName, strLabor, Paytype);

                            //
                            // Code change by https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1983/
                            // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/2007/
                            //

                            var setting = WoLineOnlyJobType(tev.selected_business_unit_id);
                            if (setting)
                            {
                                var jobType = JobTypeName(tev.membertype_id);
                                detail_obj.description = tev.paytype_id == OpsPayType.Mileage ?
                                                                  string.Format("({1}) {0} - Distance traveled: {2} {3}", tev.loaded_user.FullName, Paytype, tev.mileage_value, tev.mileage_unit)
                                                                : string.Format("{0} Hours {1}: {2}", jobType, strLabor, Paytype);
                            }

                            if (tev.paytype_id == OpsPayType.Mileage)
                            {
                                detail_obj.seg1_id = mt_obj.ID;
                            }
                            var already_exists = tev.paytype_id == OpsPayType.Mileage ? Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM wo_detail_current WHERE seg1_id=@v0 and wo_detail_current_type = 'K' ", new object[] { mt_obj.ID }) : Toolbox.doSQL_int(conn,
                                @"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1  AND wo_detail_current_description = @v2 ",
                                new object[] { mt_obj.membertime_woprog_id, tev.master_id, detail_obj.description });
                            var current_cost = tev.paytype_id == OpsPayType.Mileage ? 0 :
                                detail_obj.FindTrueLabourCost(tev.loaded_user.id, tev.paytype_id, tev.selected_business_unit_id);
                            var current_sell = already_exists > 0
                                ? Toolbox.doSQL_double(conn,
                                    @"SELECT IFNULL(MAX(wo_detail_current_price_sell), 0) sell FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ",
                                    new object[] { mt_obj.membertime_woprog_id, tev.master_id })
                                : 0;
                            var current_unit = already_exists > 0
                                ? Toolbox.doSQL_double(conn,
                                    @"SELECT IFNULL(MAX(wo_detail_current_price_unit), 0) unit FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ",
                                    new object[] { mt_obj.membertime_woprog_id, tev.master_id })
                                : 0;
                            #endregion variable definition
                            var line_exists = detail_obj.id > 0;
                            if (!line_exists)
                            {
                                detail_obj.added_by = tev.entering_user.id;
                                detail_obj.master_id = tev.master_id;
                                detail_obj.business_unit_id = tev.selected_business_unit_id;
                                detail_obj.cost = tev.paytype_id == OpsPayType.Mileage ? 0 : detail_obj.FindTrueLabourCost(detail_obj.memberid, detail_obj.paytypeid, detail_obj.business_unit_id);
                                detail_obj.sell = tev.wo.use_fixed_labour_rate ? tev.wo.fixed_labour_rate : already_exists > 0 ? current_sell : charge_out_rate;
                                detail_obj.unit = tev.wo.use_fixed_labour_rate ? tev.wo.fixed_labour_rate : already_exists > 0 ? current_unit : charge_out_rate;
                                detail_obj.tax1 = tax_labor ? tev.wo.woprog_tax1 : 0;
                                detail_obj.tax2 = tax_labor ? tev.wo.woprog_tax2 : 0;
                                detail_obj.tax3 = tax_labor ? tev.wo.woprog_tax3 : 0;
                                detail_obj.tax4 = tax_labor ? tev.wo.woprog_tax4 : 0;
                                detail_obj.bvwo = Convert.ToInt32(tev.woprog_bvwo);
                                detail_obj.discount = 0;
                                if (tev.paytype_id != OpsPayType.Mileage)
                                {
                                    detail_obj.type = "L";
                                }
                                else
                                {
                                    detail_obj.type = "K";
                                }


                                detail_obj.code = tev.master_id.ToString();
                                detail_obj.origin = "Entered From Timesheet";
                                detail_obj.issues = "";
                                var current_billtypeid = already_exists > 0
                                    ? Toolbox.doSQL_int(conn,
                                        @"SELECT IFNULL(MAX(wo_detail_current_billtypeid), 0) id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1  AND wo_detail_current_description = @v2 ",
                                        new object[] { mt_obj.membertime_woprog_id, tev.master_id, detail_obj.description })
                                    : tev.wo.QuoteID == "0"
                                        ? 0
                                        : 1;

                                if (tev.wo.QuoteID == "0")
                                {
                                    detail_obj.billtypeid = already_exists > 0 ? current_billtypeid : 0;
                                }
                                else
                                {
                                    detail_obj.billtypeid = already_exists > 0 ? current_billtypeid : 1;
                                    detail_obj.tax1 = 0;
                                    detail_obj.tax2 = 0;
                                    detail_obj.tax3 = 0;
                                    detail_obj.tax4 = 0;
                                }
                            }

                            detail_obj.paytypeid = tev.paytype_id;
                            detail_obj.memberid = mt_obj.membertime_memberid;
                            detail_obj.qty_ordered = tev.paytype_id == OpsPayType.Mileage ? Convert.ToDouble(tev.mileage_value ?? 0) : tev.hours;
                            detail_obj.qty_committed = tev.paytype_id == OpsPayType.Mileage ? Convert.ToDouble(tev.mileage_value ?? 0) : tev.hours;
                            detail_obj.qty_invoiced = detail_obj.qty_committed;

                            #region Tries to update wo_detail_current, returns rows affected to test if an insert is needed

                            // Get a copy of timesheet record after inserted.
                            var postRecord = new NeMemberTime(mt_obj.MemberTimeID);
                            var previous = new NeWODetailCurrent(detail_obj.id);

                            //
                            // Transfer information checking.
                            //
                            if (tev.IsTransfer == true)
                            {
                                tev.IdForTransfer = mt_obj.ID;
                                if (tev.originalChargeoutId == tev.master_id)
                                {
                                    if (tev.transferInfo == "N")
                                    {
                                        // For negative record, go with existing labor line
                                        // For positive one, using the old one.
                                        detail_obj.cost = tev.OriginalCost;
                                        postRecord.MemberTime_CostPrice = tev.OriginalCost; // for job cost..., and the cost part should be based on the orignal one.

                                    }

                                    if (tev.transferInfo == "P")
                                    {
                                        // For positive one, using the old one.
                                        detail_obj.cost = tev.OriginalCost;
                                        postRecord.MemberTime_CostPrice = tev.OriginalCost; // for job cost..., and the cost part should be based on the orignal one.
                                    }

                                }
                                else
                                {
                                    //
                                    // if do diferent job, no changes.
                                    //  

                                    if (tev.transferInfo == "N")
                                    {
                                        // For negative one, using the old one.
                                        detail_obj.cost = tev.OriginalCost;
                                        postRecord.MemberTime_CostPrice = tev.OriginalCost; // for job cost..., and the cost part should be based on the orignal one.
                                    }

                                    if (tev.transferInfo == "P")
                                    {
                                        // For positive record: no changes.
                                        if (tev.orginal_branch_can_see_jobtype)
                                        {
                                            // The branch can see the jobtype selector, so use what ever from angular side.
                                        }
                                        else
                                        {
                                            detail_obj.cost = tev.OriginalCost;
                                            postRecord.MemberTime_CostPrice = tev.OriginalCost; // for job cost...
                                        }
                                    }
                                }
                            }
                            //
                            // End of transfer
                            //

                            // Any committed change to timesheet record may trigger an update on lablor line item.
                            // First two parameters describes this changes.
                            // Previous states the labor line before change happens.
                            // detail_obj states the 'To be changed value'
                            // null state that not call for parent work order.
                            WOCostVisibility(null, postRecord, previous, detail_obj, null);

                            detail_obj.save(tev.entering_user, "NEMembertime - time_entry_create #1", false, conn, transaction);

                            var target_rec_no = 2;
                            if (!line_exists)
                            {
                                // We need to move the labor up to the top
                                var c_total_records = detail_obj.GetLineCount(tev.wo.woprog_id);
                                var c_total_labor_lines = detail_obj.GetTypedLineCount(tev.wo.woprog_id, "L");
                                // Any other labor exists?
                                if (c_total_labor_lines > 0)
                                {
                                    // Get the max rec # for labor lines
                                    var max_rec_no = Toolbox.doSQL_int(conn,
                                        @"SELECT IFNULL(MAX(wo_detail_current_rec_no), 1) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = 'L' AND wo_detail_current_id != @v1 ",
                                        new object[] { detail_obj.woprog_id, detail_obj.id });
                                    // Add it to the bottom
                                    target_rec_no = max_rec_no + 1;
                                }
                                //detail_obj.SwitchRecNo(detail_obj.rec_no, target_rec_no, MySQLworkorder.woprog_id, detail_obj.id);
                            }
                            // The user's charge out for this branch... if there isn't one... use the sell.
                            if (tev.is_child_wo)
                            {
                                #region is_child_wo

                                // Check to see if a row already exists on the parent work order
                                var temp_chargeout_id = Toolbox.doSQL_int(conn,
                                    @"SELECT IFNULL(MAX(id), 0) FROM membertype_chargeout where membertype_id = @v0  and paytype_id = @v1  and business_unit_id = @v2 ",
                                    new object[] { tev.membertype_id, tev.paytype_id, tev.parent_wo.business_unit_id });


                                //
                                // For transfer we need to check which chargeout Id we need to use.
                                //
                                if (tev.IsTransfer == true)
                                {
                                    if (tev.orginalChargeoutIdForParentWorkOrderLaborLIine == temp_chargeout_id)
                                    {
                                        //
                                        // do the same job type work.
                                        //

                                        if (tev.transferInfo == "N")
                                        {
                                            // For negative record, keep same thing

                                        }

                                        if (tev.transferInfo == "P")
                                        {
                                            // For positive record: no changes.
                                        }

                                    }
                                    else
                                    {
                                        //
                                        // if do diferent job, no changes.
                                        //  

                                        if (tev.transferInfo == "N")
                                        {
                                            // For negative record
                                            temp_chargeout_id = tev.orginalChargeoutIdForParentWorkOrderLaborLIine;
                                        }

                                        if (tev.transferInfo == "P")
                                        {
                                            // For positive record: no changes.
                                            if (tev.orginal_branch_can_see_jobtype)
                                            {
                                                // The branch can see the jobtype selector, so use what ever from angular side.
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                }
                                //
                                // End of transfer
                                //

                                var wodc_parent = new NeWODetailCurrent(tev.loaded_user.id,
                                    mt_obj.MemberTimePayTypeHoursID, tev.parent_wo.woprog_id, temp_chargeout_id);
                                var division_xfer_ids =
                                    Toolbox.doSQL_string(conn,
                                        @"SELECT GROUP_CONCAT(customer_id) FROM customer  where customer_name like 'Division Transfer%'",
                                        null);
                                var division_xfer_list = division_xfer_ids.Split(',').Select(int.Parse).ToList();
                                if (tev.wo.QuoteID == "0" &&
                                    !division_xfer_list.Contains(tev.wo.WOProg_Customer_ID)
                                ) // You are never to save labor to parent work orders if it's a quoted job.... OR DIV 2 DIV
                                {
                                    line_exists = wodc_parent.id > 0;
                                    if (!line_exists) // You only need to prep these variables if it's a new row.
                                    {
                                        var temp_chargeout = temp_chargeout_id > 0
                                            ? Toolbox.doSQL_double(conn, @"CALL CUSTOMER_CHARGEOUT(@v0 ,@v1 )",
                                                new object[] { tev.parent_wo.WOProg_Customer_ID, temp_chargeout_id })
                                            : detail_obj.sell;
                                        var childcompany = new NeBusinessUnit(tev.parent_wo.business_unit_id);
                                        var tax_labor_child = childcompany.TaxLabour;
                                        var billtype = detail_obj.billtypeid;
                                        wodc_parent.added_by = tev.entering_user.id;
                                        wodc_parent.master_id = temp_chargeout_id;
                                        wodc_parent.memberid = mt_obj.membertime_memberid;
                                        wodc_parent.billtypeid = tev.parent_wo.QuoteID == "0" ? 0 : 1; // Change billtype
                                        wodc_parent.tax1 = tax_labor_child == 1 && billtype != 1 ? tev.parent_wo.woprog_tax1 : 0;
                                        wodc_parent.tax2 = tax_labor_child == 1 && billtype != 1 ? tev.parent_wo.woprog_tax2 : 0;
                                        wodc_parent.tax3 = tax_labor_child == 1 && billtype != 1 ? tev.parent_wo.woprog_tax3 : 0;
                                        wodc_parent.tax4 = tax_labor_child == 1 && billtype != 1 ? tev.parent_wo.woprog_tax4 : 0;
                                        wodc_parent.woprog_id = tev.parent_wo.woprog_id;
                                        wodc_parent.business_unit_id = tev.parent_wo.business_unit_id;
                                        wodc_parent.cost = tev.paytype_id == OpsPayType.Mileage ? 0 : detail_obj.sell;

                                        if (tev.paytype_id != OpsPayType.Mileage)
                                        {
                                            wodc_parent.type = "L";
                                        }
                                        else
                                        {
                                            wodc_parent.type = "K";
                                        }
                                        wodc_parent.sell = tev.parent_wo.use_fixed_labour_rate ? tev.parent_wo.fixed_labour_rate : temp_chargeout;
                                        wodc_parent.unit = tev.parent_wo.use_fixed_labour_rate ? tev.parent_wo.fixed_labour_rate : temp_chargeout;
                                        wodc_parent.bvwo = Convert.ToInt32(tev.parent_wo.OrderNumber);
                                        wodc_parent.code = wodc_parent.master_id.ToString();
                                        wodc_parent.origin = "Entered From Timesheet";
                                    }
                                    // This info is allowed to shift when a line is edited.
                                    wodc_parent.description = tev.paytype_id == OpsPayType.Mileage ? string.Format("({1}) {0} traveled: {2} {3}", tev.loaded_user.FullName, Paytype, tev.mileage_value, tev.mileage_unit) : string.Format("{0} Hours {1}: {2}", tev.loaded_user.FullName,
                                        strLabor, Paytype);

                                    //
                                    // Code change by https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1983/
                                    //

                                    //code change https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/2007/

                                    var checksetting = WoLineOnlyJobType(tev.parent_wo.business_unit_id);

                                    if (checksetting)
                                    {
                                        var jobType = JobTypeName(tev.membertype_id);
                                        detail_obj.description = tev.paytype_id == OpsPayType.Mileage ?
                                                                          string.Format("({1}) {0} - Distance traveled: {2} {3}", jobType, Paytype, tev.mileage_value, tev.mileage_unit)
                                                                        : string.Format("{0} Hours {1}: {2}", jobType, strLabor, Paytype);
                                    }
                                    wodc_parent.qty_ordered = tev.paytype_id == OpsPayType.Mileage ? (double)tev.mileage_value : tev.hours;
                                    wodc_parent.qty_committed = tev.paytype_id == OpsPayType.Mileage ? (double)tev.mileage_value : tev.hours;
                                    wodc_parent.qty_invoiced = tev.paytype_id == OpsPayType.Mileage ? (double)tev.mileage_value : tev.hours;
                                    wodc_parent.paytypeid = Convert.ToInt32(tev.paytype_id);

                                    if (tev.paytype_id == OpsPayType.Mileage)
                                    {
                                        wodc_parent.seg1_id = mt_obj.ID;
                                    }

                                    var previousParent = new NeWODetailCurrent(wodc_parent.id);
                                    WOCostVisibility(null, postRecord, previousParent, wodc_parent, detail_obj);

                                    wodc_parent.save(tev.entering_user, "NEMembertime - time_entry_create #2", false, conn, transaction);
                                    if (!line_exists)
                                    {
                                        // We need to move the labor up to the top
                                        var c_total_records = detail_obj.GetLineCount(tev.parent_wo.woprog_id);
                                        var c_total_labor_lines = detail_obj.GetTypedLineCount(tev.parent_wo.woprog_id, "L");
                                        // Any other labor exists?
                                        if (c_total_labor_lines > 0)
                                        {
                                            // Get the max rec # for labor lines
                                            var max_rec_no = Toolbox.doSQL_int(conn,
                                                @"SELECT IFNULL(MAX(wo_detail_current_rec_no), 1) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = 'L' AND wo_detail_current_id != @v1 ",
                                                new object[] { tev.parent_wo.woprog_id, wodc_parent.id });
                                            // Add it to the bottom
                                            target_rec_no = max_rec_no + 1;
                                        }
                                        //detail_obj.SwitchRecNo(wodc_parent.rec_no, target_rec_no, wo_parent.woprog_id, wodc_parent.id);
                                    }
                                }

                                #endregion is_child_wo
                            }

                            #endregion Didn't update child

                            //
                            // Bug 1887: Sell Price on time record is 0. (https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1887/)
                            // 
                            // When creating a wo timesheet record, we go through two steps:
                            // (1) Create a wo timesheet reord with some required information.
                            // (2) Update the cost & sell price.
                            //
                            //  Issue:
                            //  In step (2), we try to get the sell price from labor line. But for the first time to create timesheet record,
                            //  no records from wo_detail_current table. Then the current_sell is zero, that's the reason why the first record always has zero $chargeout.
                            //  But the labor line code is smart enough to pick the $ correctly. so the fix will be following the labor line code.
                            //
                            mt_obj.MemberTime_SellPrice = tev.wo.use_fixed_labour_rate ? tev.wo.fixed_labour_rate : (already_exists > 0 ? current_sell : charge_out_rate);
                            mt_obj.MemberTime_CostPrice = current_cost;

                            //
                            // Transfer information checking.
                            //
                            if (tev.IsTransfer == true)
                            {
                                tev.IdForTransfer = mt_obj.ID;
                                if (tev.originalChargeoutId == tev.master_id)
                                {
                                    //
                                    // do the same job type work, but salary may be up.
                                    //

                                    if (tev.transferInfo == "N")
                                    {
                                        // For negative record, keep same thing
                                        mt_obj.MemberTime_CostPrice = tev.OriginalCost;
                                        mt_obj.MemberTime_SellPrice = tev.OriginalPrice;
                                    }

                                    if (tev.transferInfo == "P")
                                    {
                                        // For positive record: no changes.
                                        mt_obj.MemberTime_CostPrice = tev.OriginalCost;
                                        mt_obj.MemberTime_SellPrice = tev.OriginalPrice;
                                    }

                                }
                                else
                                {
                                    //
                                    // if do diferent job, no changes.
                                    //  

                                    if (tev.transferInfo == "N")
                                    {
                                        // For negative record
                                    }

                                    if (tev.transferInfo == "P")
                                    {
                                        // For positive record: no changes.
                                        if (tev.orginal_branch_can_see_jobtype)
                                        {
                                            // The branch can see the jobtype selector, so use what ever from angular side.
                                        }
                                        else
                                        {
                                            mt_obj.MemberTime_CostPrice = tev.OriginalCost;
                                            mt_obj.MemberTime_SellPrice = tev.OriginalPrice;
                                        }

                                        //
                                        // Bug 1887: Sell Price on time record is 0. (https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1887/)
                                        //
                                        //if (mt_obj.MemberTime_SellPrice == 0)
                                        //{
                                        //    //
                                        //    // Now we have the 'master id', 'customer id', so we can query a value on this.
                                        //    //
                                        //    var chargeoutForTransfer = Toolbox.doSQL_double(conn, @"CALL CUSTOMER_CHARGEOUT(@v0 ,@v1 )",
                                        //        new object[] { tev.wo.WOProg_Customer_ID, tev.master_id });

                                        //    mt_obj.MemberTime_SellPrice = chargeoutForTransfer;
                                        //}
                                    }
                                }

                                // end of if
                            }
                            //
                            // End of transfer
                            //

                            mt_obj.UpdateMemberTime(conn, transaction);

                            #region Update WO header Totals 

                            NeWOProg.update_header_totals(mt_obj.membertime_woprog_id.ToString(),
                                tev.selected_business_unit_id, tev.woprog_bvwo, conn, transaction);
                            if (tev.is_child_wo && !tev.is_shop_time)
                            {
                                NeWOProg.update_header_totals(tev.parent_wo.woprog_id.ToString(), tev.parent_wo.business_unit_id,
                                    tev.parent_wo.OrderNumber, conn, transaction);
                            }
                            if ((mt_obj.wo_percent_complete != 0) && (tev.loaded_user.membertype.considered_pm))
                            {
                                Toolbox.doSQL_void(conn,
                                    @"update woprog  set WOProg_TimeSheet_Percentage=@v0  where woprog_id =@v1 limit 1 ",
                                    new object[] { mt_obj.wo_percent_complete, mt_obj.membertime_woprog_id }, transaction);
                            }

                            #endregion Update WO header Totals
                        }

                        transaction.Commit();
                        #endregion WO / Quotes
                    }
                    else if (tev.entrytype == 3)
                    {
                        #region Business Development
                        wo_comments_obj.WorkOrderID = "0";
                        wo_comments_obj.WoComment_Member_ID = tev.loaded_user.id;
                        wo_comments_obj.Comments = string.Format("CALLS: {0}  MEETINGS: {1}  DropOffs: {2}  EMAILS: {3}  VMs: {4}  QuoteOpps: {5}", tev.bizdev_calls, tev.bizdev_meetings, tev.bizdev_faxes, tev.bizdev_emails, tev.bizdev_mailers, tev.bizdev_quoteopps);
                        wo_comments_obj.HoursWorked = tev.hours.ToString();
                        wo_comments_obj.PrintComments = 0;
                        wo_comments_obj.business_unit_id = tev.selected_business_unit_id;
                        wo_comments_obj.CreatedDate = tev.date;
                        wo_comments_obj.AddBusinessDevelopmentWOComment(tev.loaded_user.id, 0, out commentid, conn, transaction);
                        wo_comments_obj.EntryDate = tev.date;
                        mt_obj.membertype_id = Convert.ToInt32(tev.loaded_user.MemberTypeID);
                        mt_obj.MemberTimeWoCommentID = commentid;
                        mt_obj.MemberTime_Cust_No = tev.customer_number;
                        mt_obj.MemberTimeWoComment = string.Format("CALLS: {0}  MEETINGS: {1}  DropOffs: {2}  EMAILS: {3}  VMs: {4}  QuoteOpps: {5}", tev.bizdev_calls, tev.bizdev_meetings, tev.bizdev_faxes, tev.bizdev_emails, tev.bizdev_mailers, tev.bizdev_quoteopps);
                        mt_obj.WOType = "telem";
                        mt_obj.MemberTime_Premium = "False";
                        mt_obj.Date = tev.date;
                        mt_obj.MemberTimeCustomerName = tev.customer_name;
                        mt_obj.MemberTimeWorkOrderID = "0";
                        mt_obj.membertime_woprog_id = 0;
                        mt_obj.wo_percent_complete = 100;
                        mt_obj.NumberOfHours = tev.hours;
                        mt_obj.MemberTimePayTypeHoursID = tev.paytype_id;
                        mt_obj.membertime_memberid = tev.loaded_user.id;
                        mt_obj.MemberIDAudit = tev.entering_user.id;
                        mt_obj.CreatedDate = Toolbox.MySQLNow_long();
                        mt_obj.MemberIDCreate = tev.entering_user.id;
                        mt_obj.business_unit_id = tev.selected_business_unit_id;
                        mt_obj.MemberTime_Mileage = "false";
                        mt_obj.MemberTime_Premium = "false";
                        mt_obj.MemberTime_SRED = "false";
                        mt_obj.MemberTime_Warranty = "false";
                        mt_obj.MemberIDCreate = tev.entering_user.id;
                        mt_obj.MemberTime_CostPrice = tev.cost_price;
                        mt_obj.MemberTime_SellPrice = tev.sell_price;
                        mt_obj.rating = tev.rating;
                        try
                        {
                            mt_obj.AddMemberTime(tev.loaded_user.id, conn, transaction);
                            transaction.Commit();
                        }
                        catch (Exception ex1)
                        {
                            new Toolbox().catch_error(ex1);
                            transaction.Rollback();
                            throw;
                        }
                        #endregion Business Development
                    }
                    else if (tev.entrytype == 4)
                    {
                        #region Shop Time
                        wo_comments_obj.WorkOrderID = tev.woprog_bvwo;
                        wo_comments_obj.WoComment_Member_ID = tev.loaded_user.id;
                        wo_comments_obj.Comments = tev.selected_comment_id > 0 ? tev.selected_comment_text : tev.comment;
                        wo_comments_obj.HoursWorked = tev.hours.ToString();
                        wo_comments_obj.MemberIDAudit = tev.entering_user.id;
                        wo_comments_obj.business_unit_id = tev.selected_business_unit_id;
                        wo_comments_obj.CreatedDate = tev.date;
                        wo_comments_obj.woprog_id = 0;
                        wo_comments_obj.AddWOComment(tev.loaded_user.id, 0, out commentid, out commentchildid, false, 0, conn, transaction);
                        wo_comments_obj.EntryDate = tev.date;
                        mt_obj.MemberTimeWoCommentID = commentid;
                        mt_obj.MemberTime_Cust_No = tev.customer_number;
                        mt_obj.MemberTimeCustomerName = Toolbox.doSQL_string(@"SELECT type from membertime_shop_type where id=@v0",
                            tev.customer_number);
                        mt_obj.WOType = "Shop";
                        mt_obj.MemberTimeWorkOrderID = tev.customer_number;
                        mt_obj.membertime_woprog_id = Convert.ToInt32(tev.customer_number);
                        mt_obj.wo_percent_complete = 100;
                        mt_obj.NumberOfHours = tev.hours;
                        mt_obj.MemberTimePayTypeHoursID = tev.paytype_id;
                        mt_obj.membertime_memberid = tev.loaded_user.id;
                        mt_obj.MemberIDAudit = tev.entering_user.id;
                        mt_obj.Date = tev.date;
                        mt_obj.CreatedDate = Toolbox.MySQLNow_short();
                        mt_obj.MemberIDCreate = tev.entering_user.id;
                        mt_obj.business_unit_id = tev.selected_business_unit_id;
                        mt_obj.membertime_shop_type_id = Toolbox.ReturnZeroIfNull_int(tev.customer_number);
                        mt_obj.membertype_id = Convert.ToInt32(tev.loaded_user.MemberTypeID);
                        mt_obj.MemberTime_Mileage = "false";
                        mt_obj.MemberTime_Premium = "false";
                        mt_obj.MemberTime_SRED = "false";
                        mt_obj.MemberTime_Warranty = "false";
                        mt_obj.MemberIDCreate = tev.entering_user.id;
                        mt_obj.MemberTime_CostPrice = tev.cost_price;
                        mt_obj.MemberTime_SellPrice = tev.sell_price;
                        mt_obj.rating = tev.rating;
                        mt_obj.internal_project_id = Toolbox.ReturnZeroIfNull_int(tev.internal_project_id);

                        try
                        {
                            if (tev.IsTransfer == true)
                            {
                                mt_obj.MemberTime_CostPrice = tev.OriginalCost;
                                mt_obj.membertype_id = tev.originalMemberTypeid;
                            }


                            mt_obj.AddMemberTime(tev.loaded_user.id, conn, transaction);

                            //
                            // Transfer information checking.
                            //
                            if (tev.IsTransfer == true)
                            {
                                tev.IdForTransfer = mt_obj.ID;
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            new Toolbox().catch_error(ex);
                            transaction.Rollback();
                            throw;
                        }
                    }
                    #endregion Shop Time
                }
            }
        }
        public static void time_entry_edit(time_entry_vars tev)
        {
            using (var conn = Toolbox.connect())
            {
                var parent_wo = tev.parent_wo;
                var mt_obj = new NeMemberTime();
                var old_mt_obj = new NeMemberTime(tev.membertime_id);
                var wo_comment_obj = new NeWOComments();
                var commentid = 0;
                var commentchildid = 0;
                mt_obj.is_prevailing_wage = tev.is_prevailing_wage;

                using (var transaction = conn.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    handleTags(conn, tev, transaction);
                    if (tev.entrytype == 1 || tev.entrytype == 2)
                    {
                        #region WO / Quotes
                        #region Variable Declaration
                        wo_comment_obj.WorkOrderID = tev.woprog_bvwo;
                        wo_comment_obj.WoComment_Member_ID = tev.loaded_user.id;
                        wo_comment_obj.business_unit_id = tev.selected_business_unit_id;
                        wo_comment_obj.Comments = tev.comment;
                        wo_comment_obj.HoursWorked = tev.hours.ToString();
                        wo_comment_obj.CreatedDate = tev.date;
                        wo_comment_obj.EntryDate = tev.date;
                        if (tev.is_child_wo)
                        {
                            wo_comment_obj.company_child_id = tev.parent_wo.business_unit_id;
                            wo_comment_obj.WorkOrderChildID = tev.parent_wo.OrderNumber;
                        }
                        mt_obj.WOType = "WO";
                        if (tev.entrytype == 2)
                        {
                            wo_comment_obj.Comments = "Quoted Job.  See Quote Manager for details.";
                            mt_obj.WOType = "Quote";
                        }
                        wo_comment_obj.Personal = 0;
                        wo_comment_obj.PrintComments = 0;
                        mt_obj.MemberTime_Premium = "false";
                        if (tev.selected_comment_id == 0 || tev.comment != "")
                        {
                            #region if the comment box is not blank or the list box is not 0, there's a comment here.
                            #region if there was a comment already
                            if (tev.selected_comment_id > 0)
                            {
                                wo_comment_obj.UpdateWOComment(tev.loaded_user.id, tev.selected_comment_id, conn, transaction);
                                commentid = tev.selected_comment_id;
                                if (tev.is_child_wo)
                                {
                                    wo_comment_obj.UpdateWOComment(tev.loaded_user.id, tev.old_comment_id, conn, transaction);
                                }
                            }
                            else
                            {
                                wo_comment_obj.woprog_id = tev.entrytype == 1 ? tev.wo.woprog_id : 0;
                                wo_comment_obj.AddWOComment(tev.loaded_user.id, 1, out commentid, out commentchildid, false, wo_comment_obj.woprog_id, conn, transaction);
                            }
                            #endregion
                            #endregion
                        }
                        else
                        {
                            #region use the selected comment
                            try
                            {
                                wo_comment_obj.woprog_id = tev.entrytype == 1 ? tev.wo.woprog_id : 0;
                            }
                            catch
                            {
                                wo_comment_obj.woprog_id = 0;
                            }
                            wo_comment_obj.Comments = string.Format("THIS IS AN EDIT {0}", tev.selected_comment_text);
                            wo_comment_obj.AddWOComment(tev.loaded_user.id, 1, out commentid, out commentchildid, true, wo_comment_obj.woprog_id, conn, transaction);
                            mt_obj.MemberTimeWoCommentID = tev.selected_comment_id;
                            if (tev.is_child_wo)
                            {
                                mt_obj.MemberTimeWoCommentChildID = tev.old_comment_id;
                            }
                            #endregion use the selected comment
                        }
                        #region MemberTime Object Setup
                        mt_obj.WoCommentID = commentid > 0 ? commentid : mt_obj.WoCommentID;
                        mt_obj.MemberTimeID = tev.membertime_id;
                        mt_obj.member_id = tev.loaded_user.id32;
                        mt_obj.Date = tev.date;
                        mt_obj.scope_id = tev.scope_id;
                        mt_obj.prov_id = tev.prov_id;


                    if (tev.paytype_id == OpsPayType.Mileage && tev.entrytype == 1)
                    {
                        mt_obj.mileage_value = tev.mileage_value;
                        mt_obj.mileage_unit = tev.mileage_unit;
                    }

                    if (tev.entrytype == 2)
                    {
                        mt_obj.MemberTime_Cust_No = tev.customer_number;
                        mt_obj.WOType = "Quote";
                        if (tev.is_child_wo)
                        {
                            mt_obj.MemberTime_Child_Cust_No = tev.parent_wo.CustomerNumber;
                        }
                        mt_obj.membertime_woprog_id = 0;
                    }
                    else
                    {
                        mt_obj.membertime_woprog_id = tev.wo.woprog_id;
                        mt_obj.MemberTime_Cust_No = tev.customer_number;
                        if (tev.is_child_wo)
                        {
                            mt_obj.MemberTime_Child_Cust_No = tev.parent_wo.CustomerNumber;
                        }
                        mt_obj.WOType = "WO";
                    }
                    if (tev.percent_done > 0)
                    {
                        try
                        {
                            mt_obj.wo_percent_complete = tev.percent_done;
                        }
                        catch
                        {
                            mt_obj.wo_percent_complete = old_mt_obj.wo_percent_complete;
                        }
                    }
                    else
                    {
                        mt_obj.wo_percent_complete = 0;
                    }
                    mt_obj.business_unit_id = tev.selected_business_unit_id;
                    mt_obj.MemberTimeCustomerName = tev.customer_name;
                    mt_obj.MemberTimeWorkOrderID = tev.woprog_bvwo;
                    if (tev.is_child_wo)
                    {
                        mt_obj.MemberTimeChildWorkOrderID = tev.parent_wo.OrderNumber;
                        mt_obj.MemberTimeChildCustomerName = tev.parent_wo.CustomerName;
                        if (old_mt_obj.child_shoptime)
                        {
                            mt_obj.child_shoptime = true;
                            mt_obj.membertime_child_woprog_id = Convert.ToInt32(tev.parent_wo.OrderNumber);
                        }
                        else
                        {
                            try
                            {
                                mt_obj.membertime_child_woprog_id = tev.parent_wo.woprog_id;
                            }
                            catch
                            {
                                mt_obj.membertime_child_woprog_id = 0;
                            }
                        }
                    }
                    mt_obj.MemberTimeCustomerName = tev.customer_name;
                    mt_obj.NumberOfHours = tev.hours;
                    mt_obj.MemberTimePayTypeHoursID = tev.paytype_id;
                    mt_obj.membertime_memberid = tev.loaded_user.id;
                    mt_obj.MemberIDAudit = tev.entering_user.id;
                    mt_obj.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd");
                    mt_obj.ModifiedDate = DateTime.Now.ToString("yyyy-MM-dd");
                    mt_obj.MemberTime_Mileage = tev.paytype_id == OpsPayType.Mileage ? "true" : "false";
                    mt_obj.MemberTime_Premium = "false";
                    mt_obj.MemberTime_CostPrice = tev.paytype_id == OpsPayType.Mileage ? 0 : tev.cost_price;
                    mt_obj.MemberTime_SellPrice = tev.sell_price;
                    mt_obj.rating = tev.rating;

                    #endregion MemberTime Object Setup
                    #endregion Variable Declaration
                    try
                    {
                        // Get a copy of timesheet record before updated.
                        var previousRecord = new NeMemberTime(mt_obj.MemberTimeID);

                        //
                        // Bug 1887: Sell Price on time record is 0
                        // 
                        // This is the code to update the wo record. Inside UpdateMemberTime, it will update the sell price according to
                        // currently loading value, but unluckly it is always zero due to we don't pass it correrclty.
                        //
                        // So we need to fix it firstly by setting the original value. (zero'ed problem)
                        //

                        // This line fixed the zero'ed problem even you have a non-zero sell price.
                        mt_obj.MemberTime_SellPrice = previousRecord.MemberTime_SellPrice;

                        // This line try to correct an zero one.
                        if (mt_obj.MemberTime_SellPrice == 0)
                        {
                            //
                            // Work for Bug 1887: Sell Price on time record is 0.
                            // 
                            // Requery the chargeout $. We do a simple way! (we may query wo_detail_current, then chargeout later,.... but go with simple way)
                            // 
                            var charge_out_rate = tev.loaded_user.business_unit.allow_unlinked_timesheet ? 0 : tev.master_id != 1000000
                                    ? Toolbox.doSQL_double(conn, @"CALL CUSTOMER_CHARGEOUT(@v0 ,@v1 )",
                                        new object[] { tev.wo.WOProg_Customer_ID, tev.master_id })
                                    : 0;

                            mt_obj.MemberTime_SellPrice = charge_out_rate;
                        }

                        mt_obj.UpdateMemberTime(conn, transaction);
                        #region R & D
                        if (tev.do_advancement && tev.do_uncertainty)
                        {
                            var rd = new NERandD();
                            if (tev.wo.chkRD)
                            {
                                rd.RD_MemberTime_ID = mt_obj.MemberTimeID;
                                rd.RD_Member_ID = mt_obj.membertime_memberid;
                                rd.RD_ProjectAdvancement = tev.advancement;
                                rd.RD_ProjectUncertainty = tev.uncertainty;
                                rd.RD_WOProgID = mt_obj.membertime_woprog_id;
                                rd.InsertNewEntry(conn, transaction);
                            }
                            if (parent_wo.chkRD)
                            {
                                rd.RD_MemberTime_ID = mt_obj.MemberTimeID;
                                rd.RD_Member_ID = mt_obj.membertime_memberid;
                                rd.RD_ProjectAdvancement = tev.advancement;
                                rd.RD_ProjectUncertainty = tev.uncertainty;
                                rd.RD_WOProgID = parent_wo.woprog_id;
                                rd.InsertNewEntry(conn, transaction);
                            }
                        }
                        #endregion R & D

                        if (!tev.loaded_user.business_unit.allow_unlinked_timesheet)
                        {
                            var charge_out_rate = tev.master_id != 1000000
                                ? Toolbox.doSQL_double(conn, @"CALL customer_chargeout(@v0 ,@v1 )",
                                    new object[] { mt_obj.customer_id, tev.master_id })
                                : 0;
                            var Paytype = Toolbox.doSQL_string(conn,
                                @"SELECT Description FROM paytypehours WHERE PayTypeHours_ID = @v0 ", new object[] { tev.paytype_id });
                            //Update or Add Detail Line to wo_detail_current if not a quote
                            if (tev.entrytype != 2)
                            {
                                #region Try updating main

                                var master_id = tev.master_id;
                                var detail_obj = new NeWODetailCurrent();
                                var changedhours = mt_obj.MemberTimePayTypeHoursID == OpsPayType.Mileage ? (double)tev.mileage_value - (double)tev.oldmileage_value : tev.hours - tev.old_hours;
                                var strLabor = tev.entering_user.business_unit.country == "CDN" ? "Labour" : "Labor";


                                detail_obj = mt_obj.MemberTimePayTypeHoursID == OpsPayType.Mileage
                                    ? new NeWODetailCurrent(mt_obj, tev.master_id)
                                    : new NeWODetailCurrent(mt_obj.member_id, mt_obj.MemberTimePayTypeHoursID, mt_obj.membertime_woprog_id, tev.master_id);

                                detail_obj.type = mt_obj.MemberTimePayTypeHoursID == OpsPayType.Mileage ? OpsWOLineType.Mileage : OpsWOLineType.Labor;
                                //
                                // Code change by https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1983/
                                // There is no reason to update the description when editing a labor line.
                                //
                                detail_obj.description = tev.paytype_id == OpsPayType.Mileage ? string.Format("({1}) {0} traveled: {2} {3}", tev.loaded_user.FullName, Paytype, tev.mileage_value, tev.mileage_unit) : detail_obj.description;
                                detail_obj.qty_ordered = changedhours;
                                detail_obj.qty_committed = changedhours;
                                detail_obj.qty_invoiced = changedhours;
                                detail_obj.paytypeid = Convert.ToInt32(mt_obj.MemberTimePayTypeHoursID);


                                if (mt_obj.MemberTimePayTypeHoursID == OpsPayType.Mileage)
                                {
                                    detail_obj.seg1_id = mt_obj.ID;
                                }
                                // Get a copy of timesheet record after updated.
                                var postRecord = new NeMemberTime(mt_obj.MemberTimeID, conn);
                                var previous = new NeWODetailCurrent(detail_obj.id, conn);
                                WOCostVisibility(previousRecord, postRecord, previous, detail_obj, null);

                                detail_obj.save(tev.entering_user, "NEMemberTime - time_entry_edit #1", false, conn, transaction);

                                #endregion Try updating

                                var division_xfer_ids = Toolbox.doSQL_string(conn,
                                    @"SELECT GROUP_CONCAT(customer_id) FROM customer  where customer_name like 'Division Transfer%'",
                                    null);
                                var division_xfer_list = division_xfer_ids.Split(',').Select(int.Parse).ToList();

                                if (tev.is_child_wo && !mt_obj.child_shoptime && tev.wo.QuoteID == "0" &&
                                    !division_xfer_list.Contains(tev.wo.WOProg_Customer_ID))
                                {
                                    #region Has Child, try updating
                                    var temp_chargeout_id = Toolbox.doSQL_int(conn,
                                        @"SELECT IFNULL(MAX(id), 0) FROM membertype_chargeout where membertype_id = @v0  and paytype_id = @v1  and business_unit_id = @v2 ",
                                        new object[] { tev.membertype_id, tev.paytype_id, tev.parent_wo.business_unit_id });
                                    var parent_obj = mt_obj.MemberTimePayTypeHoursID == OpsPayType.Mileage
                                                          ? new NeWODetailCurrent(mt_obj, tev.master_id, true)
                                                           : new NeWODetailCurrent(mt_obj.member_id, mt_obj.MemberTimePayTypeHoursID, mt_obj.membertime_child_woprog_id, temp_chargeout_id);

                                    parent_obj.type = mt_obj.MemberTimePayTypeHoursID == OpsPayType.Mileage ? "K" : "L";
                                    parent_obj.qty_ordered = changedhours;
                                    parent_obj.qty_committed = changedhours;
                                    parent_obj.qty_invoiced = changedhours;
                                    parent_obj.seg1_id = mt_obj.MemberTimePayTypeHoursID == OpsPayType.Mileage ? mt_obj.ID : 0;
                                    parent_obj.description = mt_obj.MemberTimePayTypeHoursID == OpsPayType.Mileage
                                                            ? string.Format("({1}) {0} traveled: {2} {3}", tev.loaded_user.FullName, Paytype, tev.mileage_value, tev.mileage_unit)
                                                            : string.Format("{0} Hours {1}: {2}", tev.loaded_user.FullName, strLabor, Paytype);

                                    var previousParent = new NeWODetailCurrent(parent_obj.id);
                                    WOCostVisibility(previousRecord, postRecord, previousParent, parent_obj, detail_obj);
                                    parent_obj.save(tev.entering_user, "NEMemberTime - time_entry_edit #2", false, conn, transaction);

                                    #endregion Has Child, try updating
                                }

                                #region Update Labour Total

                                NeWOProg.update_header_totals(mt_obj.membertime_woprog_id.ToString(), old_mt_obj.business_unit_id,
                                    tev.woprog_bvwo, conn, transaction);
                                if (tev.is_child_wo && !mt_obj.child_shoptime)
                                {
                                    NeWOProg.update_header_totals(parent_wo.woprog_id.ToString(), tev.parent_wo.business_unit_id,
                                        tev.parent_wo.OrderNumber, conn, transaction);
                                }

                                #endregion Update Labour Total

                                #region update quote hours spent

                                if (tev.entrytype == 2)
                                {
                                    Toolbox.doSQL_void(conn,
                                        @"UPDATE quote_master LEFT JOIN vw_membertime_quote ON LEFT (quote_master.quote_id, 6) = vw_membertime_quote.MemberTime_WorkOrder_ID SET quote_master.hours_spent = ifnull(vw_membertime_quote.NumberOfHours,0)  WHERE quote_master.quote_id =@v0",
                                        new object[] { tev.woprog_bvwo }, transaction);
                                }

                                #endregion
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ee)
                    {
                        new Toolbox().catch_error(ee);
                        transaction.Rollback();
                        throw;
                    }
                    #endregion WO / Quotes
                }
                else if (tev.entrytype == 3)
                {
                    #region Update Business Development Only
                    wo_comment_obj.WorkOrderID = "0";
                    wo_comment_obj.WoComment_Member_ID = tev.loaded_user.id;
                    wo_comment_obj.Comments = string.Format("CALLS: {0}  MEETINGS: {1}  DropOffs: {2}  EMAILS: {3}  VMs: {4}  QuoteOpps: {5}", tev.bizdev_calls, tev.bizdev_meetings, tev.bizdev_faxes, tev.bizdev_emails, tev.bizdev_mailers, tev.bizdev_quoteopps);
                    wo_comment_obj.HoursWorked = tev.hours.ToString();
                    wo_comment_obj.EntryDate = tev.date;
                    wo_comment_obj.PrintComments = 0;
                    wo_comment_obj.business_unit_id = tev.selected_business_unit_id;
                    wo_comment_obj.CreatedDate = tev.date;
                    wo_comment_obj.AddBusinessDevelopmentWOComment(tev.loaded_user.id, 0, out commentid, conn, transaction);
                    mt_obj.MemberTimeWoCommentID = commentid;
                    mt_obj.MemberTimeID = tev.membertime_id;
                    mt_obj.MemberTime_Cust_No = tev.customer_number.ToString();
                    mt_obj.MemberTimeWoComment = string.Format("CALLS: {0}  MEETINGS: {1}  DropOffs: {2}  EMAILS: {3}  VMs: {4}  QuoteOpps: {5}", tev.bizdev_calls, tev.bizdev_meetings, tev.bizdev_faxes, tev.bizdev_emails, tev.bizdev_mailers, tev.bizdev_quoteopps);
                    mt_obj.WOType = "telem";
                    mt_obj.MemberTime_Premium = "False";
                    mt_obj.Date = tev.todays_date;
                    mt_obj.MemberTimeCustomerName = tev.customer_name;
                    mt_obj.MemberTimeWorkOrderID = "0";
                    mt_obj.membertime_woprog_id = 0;
                    mt_obj.wo_percent_complete = 100;
                    mt_obj.NumberOfHours = tev.hours;
                    mt_obj.MemberTimePayTypeHoursID = tev.paytype_id;
                    mt_obj.membertime_memberid = tev.loaded_user.id;
                    mt_obj.MemberIDAudit = tev.entering_user.id;
                    mt_obj.CreatedDate = Toolbox.MySQLNow_short();
                    mt_obj.ModifiedDate = Toolbox.MySQLNow_short();
                    mt_obj.business_unit_id = tev.selected_business_unit_id;
                    mt_obj.MemberTime_Mileage = "false";
                    mt_obj.MemberTime_Premium = "false";
                    mt_obj.MemberIDCreate = tev.entering_user.id;
                    mt_obj.member_id = tev.loaded_user.id32;
                    mt_obj.rating = tev.rating;

                    try
                    {
                        mt_obj.UpdateMemberTime(conn, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ee)
                    {
                        new Toolbox().catch_error(ee);
                        transaction.Rollback();
                        throw;
                    }
                    #endregion Update Business Development Only
                }
                else if (tev.entrytype == 4)
                {
                    #region Shop Time
                    wo_comment_obj.WorkOrderID = old_mt_obj.MemberTimeWorkOrderID;
                    wo_comment_obj.WoCommentID = old_mt_obj.MemberTimeWoCommentID;
                    wo_comment_obj.WoComment_Member_ID = tev.entering_user.id;
                    wo_comment_obj.Comments = tev.comment == "" ? tev.selected_comment_text : tev.comment;
                    wo_comment_obj.HoursWorked = tev.hours.ToString();
                    wo_comment_obj.MemberIDAudit = tev.entering_user.id;
                    wo_comment_obj.business_unit_id = tev.selected_business_unit_id;
                    wo_comment_obj.CreatedDate = tev.date;
                    wo_comment_obj.UpdateWOComment(tev.loaded_user.id, old_mt_obj.MemberTimeWoCommentID, conn, transaction);
                    wo_comment_obj.EntryDate = tev.date;
                    mt_obj.MemberTime_Cust_No = tev.customer_number;
                    mt_obj.MemberTimeCustomerName = Toolbox.doSQL_string(@"SELECT type from membertime_shop_type where id=@v0",
                        tev.customer_number);
                    mt_obj.WOType = "Shop";
                    mt_obj.MemberTimeWorkOrderID = tev.customer_number;
                    mt_obj.membertime_woprog_id = Convert.ToInt32(tev.customer_number);
                    mt_obj.wo_percent_complete = 100;
                    mt_obj.NumberOfHours = tev.hours;
                    mt_obj.MemberTimePayTypeHoursID = tev.paytype_id;
                    mt_obj.membertime_memberid = tev.loaded_user.id;
                    mt_obj.ModifiedDate = Toolbox.MySQLNow_long();
                    mt_obj.MemberIDAudit = tev.entering_user.id;
                    mt_obj.Date = tev.date;
                    mt_obj.CreatedDate = old_mt_obj.CreatedDate;
                    mt_obj.MemberIDCreate = tev.entering_user.id;
                    mt_obj.business_unit_id = tev.selected_business_unit_id;
                    mt_obj.membertime_shop_type_id = Toolbox.ReturnZeroIfNull_int(tev.customer_number);

                    mt_obj.MemberTime_Mileage = "false";
                    mt_obj.MemberTimeID = old_mt_obj.MemberTimeID;
                    mt_obj.MemberTimeWoCommentID = old_mt_obj.MemberTimeWoCommentID;
                    mt_obj.MemberTime_Premium = "false";
                    mt_obj.MemberTime_SRED = "false";
                    mt_obj.MemberTime_Warranty = "false";
                    mt_obj.MemberIDCreate = tev.entering_user.id;
                    mt_obj.member_id = tev.loaded_user.id32;
                    mt_obj.rating = tev.rating;
                    try
                    {
                        mt_obj.UpdateMemberTime(conn, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ee)
                    {
                        new Toolbox().catch_error(ee);
                        transaction.Rollback();
                        throw;
                    }
                    #endregion Shop Time
                }
            }
            }
        }
        public static void time_entry_delete(time_entry_vars tev)
        {

        }
        public DataTable CurrentMemberTimeSummary(int mem_id, string searchdate)
        {
            return Toolbox.doSQL_dt(@" Select MT.membertime_id, MT.membertime_memberid , MT.date , MT.membertime_workorder_id, MT.membertime_cust_no, URLDECODE(MT.membertime_customer_name) MemberTime_Customer_Name, IF(membertime_mileage = 'false', MT.numberofhours, 0 ) as Hours, IF(membertime_mileage = 'true', MT.numberofhours, 0) as Miles, MT.membertime_wocomment_id, MT.membertime_paytypehours_id, MT.created_date, Hour.description as HourType, M.member_user as MTBy, MT.membertime_premium, MT.wotype, WOC.comments as Comments from membertime MT Inner Join wocomment WOC ON MT.MemberTime_WoComment_ID = WOC.WoComment_ID INNER JOIN paytypehours hour ON MT.MemberTime_PayTypeHours_ID = hour.PayTypeHours_ID INNER JOIN member M ON MT.membertime_memberid = M.Member_ID WHERE MT.Date = @v0  AND MT.membertime_memberid =@v1 ", new object[] { Toolbox.MySQL_shortdt(Convert.ToDateTime(searchdate)), mem_id });
        }
        public bool AddMemberTime(int mem_id, MySqlConnection conn = null, MySqlTransaction transaction = null)
        {
            var _tools = new Toolbox();
            var temp_child_name = ChildCustomerName;
            var temp_name = CustomerName;
            ChildCustomerName = temp_child_name != null && !temp_child_name.Contains("''") && temp_child_name.Contains("'") ? temp_child_name.Replace("'", "''") : temp_child_name;
            CustomerName = temp_name != null && !temp_name.Contains("''") && temp_name.Contains("'") ? temp_name.Replace("'", "''") : temp_name;

            if (Cust_No.Length >= 25)
            {
                Cust_No = Cust_No.Substring(0, 24);
            }

            var returned_id = _tools.returnSQL_id(@" INSERT INTO membertime 
( membertime_memberid, date, membertime_workorder_id, membertime_customer_name, membertime_cust_no, 
membertime_customer_id, numberofhours, membertime_membertypehours_id, membertime_paytypehours_id, 
membertime_wocomment_id, member_id_audit, membertime_premium, created_date, wotype, member_id_create, 
membertime_sred, membertime_warranty, membertime_mileage, business_unit_id, membertime_child_companyid,
membertime_child_workorder_id, membertime_child_cust_no, membertime_child_customer_name, 
membertime_wocomment_child_id, wo_percent_complete, membertime_woprog_id, membertime_child_woprog_id, 
child_shoptime, membertype_id, membertype_chargeout_id, membertime_costprice, membertime_sellprice,membertime_shop_type_id, ts_lite_paytype_id, internal_project_id,scope_id,prov_id,mileage_value,mileage_unit, is_prevailing_wage) 
VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6 , @v7 , @v8 , @v9 , @v10 , @v11 , now(), @v12 , 
@v13 , @v14 , @v15 , @v16 , @v17 , @v18 , @v19 , @v20 , @v21 , @v22 , @v23 , @v24 , @v25 , @v26 ,
@v27 , @v28 , @v29 , @v30 ,@v31,@v32,@v33,@v34,@v35,@v36,@v37, @v38)", new object[] {  mem_id, _Date, WorkOrderID, CustomerName, Cust_No,
                customer_id, NumberOfHours, _MemberTime_MemberTypeHours_ID,
                PayTypeHoursID, WoCommentID, MemberIDAudit, MemberTime_Premium,
                WOType, MemberIDCreate, MemberTime_SRED, MemberTime_Warranty,
                MemberTime_Mileage, business_unit_id, _MemberTime_Child_CompanyID, ChildWorkOrderID,
                Child_Cust_No, ChildCustomerName, _MemberTime_WoComment_Child_ID, wo_percent_complete,
                woprog_id, child_woprog_id, child_shoptime, membertype_id, membertype_chargeout_id,
                MemberTime_CostPrice, MemberTime_SellPrice,membertime_shop_type_id, tsLitePayType,internal_project_id,scope_id,prov_id,mileage_value,mileage_unit, is_prevailing_wage}, conn, transaction);
            ID = Convert.ToInt32(returned_id);
            return true;
        }
        public void UpdateMemberTime(MySqlConnection conn = null, MySqlTransaction transaction = null)
        {
            var _tools = new Toolbox();
            var temp_child_name = ChildCustomerName;
            var temp_name = CustomerName;
            ChildCustomerName = temp_child_name != null && !temp_child_name.Contains("''") && temp_child_name.Contains("'") ? temp_child_name.Replace("'", "''") : temp_child_name;
            CustomerName = temp_name != null && !temp_name.Contains("''") && temp_name.Contains("'") ? temp_name.Replace("'", "''") : temp_name;
            _tools.getSQL_void(@" UPDATE membertime SET membertime_memberid = @v0 , 
date = @v1 , membertime_workorder_id = @v2 , membertime_cust_no = @v3 ,
membertime_customer_name = @v4 , numberofhours = @v5 , membertime_membertypehours_id = @v6 , 
membertime_paytypehours_id = @v7 , membertime_wocomment_id = @v8 , member_id_audit =@v9 ,
membertime_premium = @v10 , modified_date = now(), membertime_child_workorder_id = @v12 , 
membertime_child_cust_no = @v13 , membertime_child_customer_name = @v14 , 
membertime_wocomment_child_id = @v15 , membertime_woprog_id = @v16 ,
membertime_child_woprog_id = @v17 , wo_percent_complete = @v18 , 
child_shoptime = @v20 , membertime_costprice = @v21 ,
membertime_sellprice = @v22 , rating = @v23 ,membertime_shop_type_id=@v24, internal_project_id=@v25, scope_id=@v26,prov_id=@v27,mileage_value=@v28,mileage_unit=@v29, is_prevailing_wage=@v30
WHERE membertime_id = @v19 ",
new object[] {  member_id, _Date,
    WorkOrderID, MemberTime_Cust_No, CustomerName,
    NumberOfHours, _MemberTime_MemberTypeHours_ID,
    PayTypeHoursID, WoCommentID, MemberIDAudit,
    MemberTime_Premium, ModifiedDate, ChildWorkOrderID,
    Child_Cust_No, ChildCustomerName,
    _MemberTime_WoComment_Child_ID, woprog_id, child_woprog_id,
    wo_percent_complete, ID, child_shoptime, MemberTime_CostPrice,
    MemberTime_SellPrice, rating ,membertime_shop_type_id,internal_project_id,scope_id,prov_id,mileage_value,mileage_unit, is_prevailing_wage}, conn, transaction);
        }
        public void DeleteMemberTimeDet(MySqlConnection conn = null, MySqlTransaction transaction = null)
        {
            Toolbox.doSQL_void(conn, @"DELETE FROM MemberTime WHERE MemberTime_ID = @v0 ", new object[] { ID }, transaction);
        }
        public DataTable GetHourUtilization(int _business_unit_id)
        {
            var _tools = new Toolbox();
            var branch = new NeBusinessUnit(_business_unit_id);
            var retTable = new DataTable();
            retTable.Columns.Add("LYHU", typeof(double));
            retTable.Columns.Add("LYHUProjected", typeof(double));
            retTable.Columns.Add("LYHUVariance", typeof(double));
            retTable.Columns.Add("LYMHU", typeof(double));
            retTable.Columns.Add("LYMHUBudget", typeof(double));
            retTable.Columns.Add("LYMHUVariance", typeof(double));
            retTable.Columns.Add("TYHU", typeof(double));
            retTable.Columns.Add("TYHUBudget", typeof(double));
            retTable.Columns.Add("TYHUVariance", typeof(double));
            retTable.Columns.Add("TYMHU", typeof(double));
            retTable.Columns.Add("TYMHUBudget", typeof(double));
            retTable.Columns.Add("TYMHUVariance", typeof(double));
            retTable.Columns.Add("TWHU", typeof(double));
            retTable.Columns.Add("TSMHU", typeof(double));

            #region Get Date Ranges To Work With

            var TodayDate = DateTime.Now;
            var WorkDate = new DateTime();
            var _dt = new DataTable();
            var todayyear = TodayDate.Year;
            var todaymonth = TodayDate.Month;
            var lastyear = 0;
            var LastYearDateRangeStart = "";
            var LastYearDateRangeEnd = TodayDate.ToString("yyyy-MM-dd");
            var ThisYearDateRangeStart = "";
            var ThisYearDateRangeEnd = TodayDate.ToString("yyyy-MM-dd");

            //WorkDate = TodayDate.AddMonths(-1).AddDays(1).AddYears(-1);
            WorkDate = new DateTime(TodayDate.Year, TodayDate.Month, 1);
            WorkDate = WorkDate.AddYears(-1);
            var LastYearMonthDateRangeStart = WorkDate.ToString("yyyy-MM-dd");
            //WorkDate = WorkDate.AddMonths(1).AddDays(-1);
            WorkDate = new DateTime(TodayDate.Year, TodayDate.Month, 1);
            WorkDate = WorkDate.AddMonths(1).AddDays(-1).AddYears(-1);
            var LastYearMonthDateRangeEnd = WorkDate.ToString("yyyy-MM-dd");

            //WorkDate = TodayDate.AddMonths(-1).AddDays(1);
            WorkDate = new DateTime(TodayDate.Year, TodayDate.Month, 1);
            var ThisYearMonthDateRangeStart = WorkDate.ToString("yyyy-MM-dd");
            var ThisYearMonthDateRangeEnd = TodayDate.ToString("yyyy-MM-dd");

            LastYearDateRangeStart = Toolbox.MySQL_shortdt(branch.fiscal_start_previous);
            ThisYearDateRangeStart = Toolbox.MySQL_shortdt(branch.fiscal_start_current);

            #endregion

            #region set up variables

            var total_quote_last_year = 0.0;
            var totalhours_last_year = 0.0;
            var totalshop_last_year = 0.0;
            var totalnewshop_last_year = 0.0;

            var total_quote_last_month = 0.0;
            var totalhours_last_month = 0.0;
            var totalshop_last_month = 0.0;
            var totalnewshop_last_month = 0.0;

            var total_quote_this_year = 0.0;
            var totalhours_this_year = 0.0;
            var totalshop_this_year = 0.0;
            var totalnewshop_this_year = 0.0;

            var total_quote_this_month = 0.0;
            var totalhours_this_month = 0.0;
            var totalshop_this_month = 0.0;
            var totalnewshop_this_month = 0.0;

            var total_quote_this_week = 0.0;
            var totalhours_this_week = 0.0;
            var totalshop_this_week = 0.0;
            var totalnewshop_this_week = 0.0;

            var total_quote_six_month = 0.0;
            var totalhours_six_month = 0.0;
            var totalshop_six_month = 0.0;
            var totalnewshop_six_month = 0.0;

            #endregion

            var sql = string.Format(@"CALL HourUtilization('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                _business_unit_id, // {0}
                LastYearDateRangeStart, // {1}
                LastYearDateRangeEnd, // {2}
                LastYearMonthDateRangeStart, // {3}
                LastYearMonthDateRangeEnd, // {4}
                ThisYearMonthDateRangeStart, // {5}
                ThisYearMonthDateRangeEnd, // {6}
                ThisYearDateRangeStart, // {7}
                ThisYearDateRangeEnd // {8}
            );
            _dt = _tools.getSQL_datatable(@"CALL HourUtilization(@v0 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6 , @v7 , @v8 )", new object[] { _business_unit_id, LastYearDateRangeStart, LastYearDateRangeEnd, LastYearMonthDateRangeStart, LastYearMonthDateRangeEnd, ThisYearMonthDateRangeStart, ThisYearMonthDateRangeEnd, ThisYearDateRangeStart, ThisYearDateRangeEnd });
            //_tools.getSQL_void(@"INSERT INTO matt (jumble)  VALUES (@v0)",new object[] { _tools.value_to(sql) } );
            foreach (DataRow _dr in _dt.Rows)
            {
                total_quote_last_year = Convert.ToDouble(_dr["total_quote_last_year"]);
                totalhours_last_year = Convert.ToDouble(_dr["totalhours_last_year"]);
                totalshop_last_year = Convert.ToDouble(_dr["totalshop_last_year"]) + Convert.ToDouble(_dr["totalnewshop_last_year"]);
                totalnewshop_last_year = Convert.ToDouble(_dr["totalnewshop_last_year"]);

                total_quote_last_month = Convert.ToDouble(_dr["total_quote_last_month"]);
                totalhours_last_month = Convert.ToDouble(_dr["totalhours_last_month"]);
                totalshop_last_month = Convert.ToDouble(_dr["totalshop_last_month"]) + Convert.ToDouble(_dr["totalnewshop_last_month"]);
                totalnewshop_last_month = Convert.ToDouble(_dr["totalnewshop_last_month"]);

                total_quote_this_year = Convert.ToDouble(_dr["total_quote_this_year"]);
                totalhours_this_year = Convert.ToDouble(_dr["totalhours_this_year"]);
                totalshop_this_year = Convert.ToDouble(_dr["totalshop_this_year"]) + Convert.ToDouble(_dr["totalnewshop_this_year"]);
                totalnewshop_this_year = Convert.ToDouble(_dr["totalnewshop_this_year"]);

                total_quote_this_month = Convert.ToDouble(_dr["total_quote_this_month"]);
                totalhours_this_month = Convert.ToDouble(_dr["totalhours_this_month"]);
                totalshop_this_month = Convert.ToDouble(_dr["totalshop_this_month"]) + Convert.ToDouble(_dr["totalnewshop_this_month"]);
                totalnewshop_this_month = Convert.ToDouble(_dr["totalnewshop_this_month"]);

                total_quote_this_week = Convert.ToDouble(_dr["totalquote_this_week"]);
                totalhours_this_week = Convert.ToDouble(_dr["totalhours_this_month"]);
                totalshop_this_week = Convert.ToDouble(_dr["totalshop_this_week"]) + Convert.ToDouble(_dr["totalnewshop_this_week"]);
                totalnewshop_this_week = Convert.ToDouble(_dr["totalnewshop_this_week"]);

                total_quote_six_month = Convert.ToDouble(_dr["totalquote_six_month"]);
                totalhours_six_month = Convert.ToDouble(_dr["totalhours_six_month"]);
                totalshop_six_month = Convert.ToDouble(_dr["totalshop_six_month"]) + Convert.ToDouble(_dr["totalnewshop_six_month"]);
                totalnewshop_six_month = Convert.ToDouble(_dr["totalnewshop_six_month"]);
            }

            #region calculate utilization

            #region LastYear

            var lastYearUtilization = totalhours_last_year != 0 ? (totalhours_last_year - total_quote_last_year - totalshop_last_year) / totalhours_last_year : 0;
            fZero(ref lastYearUtilization);
            var lastYearUtilizationVariance = totalhours_last_year != 0 ? lastYearUtilization - .9 : 0;
            fZero(ref lastYearUtilizationVariance);

            #endregion LastYear

            #region LastYearMonth

            var lastMonthUtilization = totalhours_last_month != 0 ? (totalhours_last_month - total_quote_last_month - totalshop_last_month) / totalhours_last_month : 0;
            fZero(ref lastMonthUtilization);
            var lastMonthUtilizationVariance = totalhours_last_month != 0 ? lastMonthUtilization - .9 : 0;
            fZero(ref lastMonthUtilizationVariance);

            #endregion LastMonth

            #region ThisYear

            var ThisYearUtilization = totalhours_last_year != 0 ? (totalhours_this_year - total_quote_this_year - totalshop_this_year) / totalhours_this_year : 0;
            fZero(ref ThisYearUtilization);
            var ThisYearUtilizationVariance = totalhours_last_year != 0 ? ThisYearUtilization - .9 : 0;
            fZero(ref ThisYearUtilizationVariance);

            #endregion ThisYear

            #region ThisMonth

            var thisMonthUtilization = (totalhours_this_month - total_quote_this_month - totalshop_this_month) / totalhours_this_month;
            fZero(ref thisMonthUtilization);
            var thisMonthUtilizationVariance = thisMonthUtilization - .9;
            fZero(ref thisMonthUtilizationVariance);

            #endregion ThisMonth
            #region ThisWeek

            var thisWeekUtilization = totalhours_this_week != 0 ? (totalhours_this_week - total_quote_this_week - totalshop_this_week) / totalhours_this_week : 0;
            fZero(ref thisWeekUtilization);


            #endregion ThisWeek
            #region Six Month

            var thisSixMonthUtilization = totalhours_six_month != 0 ? (totalhours_six_month - total_quote_six_month - totalshop_six_month) / totalhours_six_month : 0;
            fZero(ref thisSixMonthUtilization);

            #endregion  Six Month

            #endregion calculate utilization		

            retTable.Rows.Add(Math.Round(lastYearUtilization, 3), // LYHU
                .9, // LYHUProjected
                Math.Round(lastYearUtilizationVariance, 3), // LYHUVariance
                Math.Round(lastMonthUtilization, 3), // LYMHU
                .9, // LYMHUBudget
                Math.Round(lastMonthUtilizationVariance, 3), // LYMHUVariance
                Math.Round(ThisYearUtilization, 3), // TYHU
                .9, // TYHUBudget
                Math.Round(ThisYearUtilizationVariance, 3), // TYHUVariance
                Math.Round(thisMonthUtilization, 3), // TYMHU
                .9, // TYMHUBudget
                Math.Round(thisMonthUtilizationVariance, 3), // TYMHUVariance
                Math.Round(thisWeekUtilization, 3),
                Math.Round(thisSixMonthUtilization, 3)
            );

            return retTable;
        }
        /// <summary>
        /// Forces zero if the figure is NaN or Infinity, I just shortened the name
        /// </summary>
        /// <param name="f"></param>
        /// <returns>Zero if bad</returns>
        private void fZero(ref double f)
        {
            if (double.IsInfinity(f) || double.IsNaN(f))
            {
                f = 0;
            }
        }

        public static bool WoLineOnlyJobType(int businessUnitID)
        {
            if (businessUnitID == 0)
            {
                return false;
            }

            var settingValue = Toolbox.doSQL_bool(@"SELECT woline_only_jobtype FROM business_unit WHERE ID = " + businessUnitID.ToString(), new object[] { });
            return settingValue;
        }

        public static string JobTypeName(int jobTypeId)
        {
            if (jobTypeId == 0)
            {
                return "";
            }

            var name = Toolbox.doSQL_string(@"SELECT membertype_name FROM membertype WHERE membertype_id = " + jobTypeId, new object[] { });
            return name;
        }


        #region WO Cost Visibility
        //
        // Related 2 tasks:
        // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1554
        // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1555
        // 
        //  Summary:
        //          Any update to a timesheet record may trigger an update to the related labor line item record (only limit to update the field: wo_detail_current_price_cost).
        //

        public static void WOCostVisibility(NeMemberTime previousRecord, NeMemberTime postRecord, NeWODetailCurrent matchedLaborLine, NeWODetailCurrent toBeAddedOrUpdatedRecord, NeWODetailCurrent childMatchedLaborLine)
        {
            //
            // In terms of operations on timesheet records, finally found the common patterns: Do Add/Update/Delete operations on timesheet table first, 
            // then do Add/Update operation on wo_detail_current table.
            //

            //
            // So be able to have below settings during the time window of between 'after operation on timesheet table is done' and 'before any operation on wo_detail_current staring':
            // (1) When added a new timesheet record, previousRecord = NULL, and postRecord is committed record.
            // (2) When editted an existing timesheet record, previousRecord is the status before updating, postRecord is committed record.
            // (3) When deleting an exising timesheet record, privousRecord is the status before deleting, postRecord is NULL.
            //

            //
            // Then before adding or updating the work order line item, do a search based on workorderID, MasterID and MemberID (This is done by outside of code, but keep the logic here).
            // Say matchedLaborLine is to be updated labor line item (from wo_detail_current), it can be either NULL (First time to add this line item) or one and only one record associated if found.
            // The input matchedLaborLine will hold the value before it gets updated. The new value will be in [toBeAddedOrUpdatedRecord].
            // 

            //
            //Outdated !!!-calledForParentWorkOrder: When it is true, that will check the parent' work order's line item. 
            //Updated: childMatchedLaborLine: when not null,that will check the parent' work order's line item and take the data from the childMatchedLaborLine into acount.

            //
            // Supposed the current code will be working perfectly that when adding/editing/deleting one timesheet record, the related line item will be updated correctly 
            // except the wo_detail_current_price_cost field. This is the problem would be fixed in this function.
            //

            //
            // Again the below code will fix this problem based on idea from the design:
            // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1554
            //

            //
            // To make simple and easy to understand, handle different cases seperately: Add/Edit/Delete timesheet record.
            //

            //
            // Testing:
            // (1) From angular timesheet page, do ADD/EDIT/DELETE operations.
            // (2) From scheduler page, do ADD operation only.
            //

            try
            {
                if (previousRecord == null && postRecord != null)
                {
                    WOCostVisibility_AfterAdding(postRecord, matchedLaborLine, toBeAddedOrUpdatedRecord, childMatchedLaborLine);
                }

                if (previousRecord != null && postRecord != null)
                {
                    WOCostVisibility_AfterEditting(previousRecord, postRecord, matchedLaborLine, toBeAddedOrUpdatedRecord, childMatchedLaborLine);
                }

                if (previousRecord != null && postRecord == null)
                {
                    WOCostVisibility_AfterDeleting(previousRecord, matchedLaborLine, toBeAddedOrUpdatedRecord, childMatchedLaborLine);
                }
            }
            catch (Exception ex)
            {
                //
                // This should not happen but ...
                // The code should not block the creation/updation for wo_detail_current.
                // Log this info to further check.
                //
                Log(previousRecord, postRecord, matchedLaborLine, toBeAddedOrUpdatedRecord, childMatchedLaborLine != null);
            }
        }

        #region add part
        private static void WOCostVisibility_AfterAdding( NeMemberTime timesheetRecord, NeWODetailCurrent laborRecord, NeWODetailCurrent toBeAddedOrUpdatedRecord, NeWODetailCurrent childMatchedLaborLine)
        {
            //
            // No lablor line item found in wo_detail_current, return.
            // When first time to create a timesheet record, the related lablor line item is not created yet.
            //
            if (laborRecord.id <= 0)
            {
                return;
            }

            if (childMatchedLaborLine != null)
            {
                WOCostVisibility_AfterAdding_CallForParentWorkorder(timesheetRecord, laborRecord, toBeAddedOrUpdatedRecord);
            }
            else
            {
                WOCostVisibility_AfterAdding_CallForCurrentWorkorder(timesheetRecord, laborRecord, toBeAddedOrUpdatedRecord);
            }
        }

        private static void WOCostVisibility_AfterAdding_CallForCurrentWorkorder(NeMemberTime timesheetRecord, NeWODetailCurrent laborRecord, NeWODetailCurrent toBeUpdatedRecord)
        {
            //
            // If costPrice from timesheet recrod is same as the current labor costPrice, do nothing.
            //
            if (IsPriceCostSame_CallForCurrrentWorkOrder(timesheetRecord, laborRecord))
            {
                return;
            }

            /*
                Say Total$ = laborRecord.wo_detail_current_price_cost * laborRecord.wo_detail_current_qty_committed + timesheetRecord.hours * timesheetRecord.cost_price;
                Say TotalHour = laborRecord.wo_detail_current_qty_committed  + timesheetRecord.hours;

                Then 
                    wo_detail_current_price_cost = Total$ / TotalHour.
             */

            var total = laborRecord.cost * laborRecord.qty_committed + timesheetRecord.NumberOfHours * timesheetRecord.MemberTime_CostPrice;
            var hours = laborRecord.qty_committed + timesheetRecord.NumberOfHours;

            var average = 0.0;
            if (hours != 0)
            {
                average = total / hours;
            }

            if (double.IsInfinity(average) || double.IsNaN(average))
            {
                return;
            }

            WOCostVisibility_UpdateCost(toBeUpdatedRecord, average, hours);
        }

        private static void WOCostVisibility_AfterAdding_CallForParentWorkorder(NeMemberTime timesheetRecord, NeWODetailCurrent laborRecord, NeWODetailCurrent toBeUpdatedRecord)
        {
            // Validation checking: child-parent relationship.
            if (timesheetRecord.child_woprog_id != laborRecord.woprog_id)
            {
                return;
            }

            // Validation checking: same person
            if (timesheetRecord.member_id != laborRecord.memberid)
            {
                return;
            }

            //
            // If this wants to adjust the hours on previous existing record, better edit other than add a new time sheet record.
            // The problem here is the chargeout$ may be updated, that means the current valude may not be the exacly same as the
            // original value when added this labor line.
            //
            // Again:
            // You can edit existing child record hours.
            // or create a new child workorder and add a new timesheet in this new workorder.
            //

            //
            // !!!! After talk with Matt, This is the corner case and not handle it and leave it as it is. !!!!
            //
        }
        
        #endregion

        #region edit part
        private static void WOCostVisibility_AfterEditting(NeMemberTime previousRecord, NeMemberTime postRecord, NeWODetailCurrent matchedLaborLine, NeWODetailCurrent toBeUpdatedRecord, NeWODetailCurrent childMatchedLaborLine)
        {
            if (childMatchedLaborLine!= null)
            {
                WOCostVisibility_AfterEditting_CallForParentWorkorder(previousRecord, postRecord, matchedLaborLine, toBeUpdatedRecord, childMatchedLaborLine);
            }
            else
            {
                WOCostVisibility_AfterEditting_CallForCurrentWorkorder(previousRecord, postRecord, matchedLaborLine, toBeUpdatedRecord);
            }
        }

        private static void WOCostVisibility_AfterEditting_CallForCurrentWorkorder(NeMemberTime previousRecord, NeMemberTime postRecord, NeWODetailCurrent matchedLaborLine, NeWODetailCurrent toBeUpdatedRecord)
        {
            /*
                Say OldTotal$ = matchedRecord.wo_detail_current_price_cost * matchedRecord.wo_detail_current_qty_committed;
                Say Reduced$ = previousRecord.MemberTime_CostPrice * previousRecord.NumberOfHours;
                Say Added$ = postRecord.MemberTime_CostPrice * postRecord.NumberOfHours;
                Say newTotal$ =  OldTotal$ - Reduced$ + Added$;
                Say TotalHour =  toBeUpdatedRecord.qty_committed.

                Then 
                    wo_detail_current_price_cost = newTotal$ / TotalHour.
             */

            var OldTotal = matchedLaborLine.cost * matchedLaborLine.qty_committed;
            var Reduced = previousRecord.MemberTime_CostPrice * previousRecord.NumberOfHours;
            var Added = postRecord.MemberTime_CostPrice * postRecord.NumberOfHours;
            var newTotal = OldTotal - Reduced + Added;

            // follow the save you will find this logic about adding both together to get the true value. search: true_qty = old_qty + new_qty;
            var TotalHour = matchedLaborLine.qty_committed +  toBeUpdatedRecord.qty_committed; 

            var average = 0.0;
            if (TotalHour != 0)
            {
                average = newTotal / TotalHour;
            }

            if (double.IsInfinity(average) || double.IsNaN(average))
            {
                return;
            }

            WOCostVisibility_UpdateCost(toBeUpdatedRecord, average, TotalHour);
        }

        private static void WOCostVisibility_AfterEditting_CallForParentWorkorder(NeMemberTime previousRecord, NeMemberTime postRecord, NeWODetailCurrent matchedLaborLine, NeWODetailCurrent toBeUpdatedRecord, NeWODetailCurrent childMatchedLaborLine)
        {
            // Validation checking: child-parent relationship.
            if (postRecord.child_woprog_id != matchedLaborLine.woprog_id)
            {
                return;
            }

            // Validation checking: same person
            if (postRecord.member_id != matchedLaborLine.memberid)
            {
                return;
            }

            //
            // Get the chargeout$
            // 
            var charge_out_rate = WOCostVisibility_GetChargeoutValue(postRecord.customer_id, postRecord.membertype_chargeout_id, matchedLaborLine.id, childMatchedLaborLine.id);

            // Do job
            var OldTotal = matchedLaborLine.cost * matchedLaborLine.qty_committed;
            var Reduced = charge_out_rate * previousRecord.NumberOfHours;
            var Added = charge_out_rate * postRecord.NumberOfHours;
            var newTotal = OldTotal - Reduced + Added;

            var TotalHour = matchedLaborLine.qty_committed - previousRecord.NumberOfHours + postRecord.NumberOfHours;

            var average = 0.0;
            if (TotalHour != 0)
            {
                average = newTotal / TotalHour;
            }

            if (double.IsInfinity(average) || double.IsNaN(average))
            {
                return;
            }

            WOCostVisibility_UpdateCost(toBeUpdatedRecord, average, TotalHour);
        }

        #endregion

        #region delete part
        private static void WOCostVisibility_AfterDeleting(NeMemberTime previousRecord, NeWODetailCurrent matchedLaborLine, NeWODetailCurrent toBeAddedOrUpdatedRecord, NeWODetailCurrent childMatchedLaborLine)
        {
            if (childMatchedLaborLine != null)
            {
                WOCostVisibility_AfterDeleting_CallForParentWorkorde(previousRecord, matchedLaborLine, toBeAddedOrUpdatedRecord, childMatchedLaborLine);
            }
            else
            {
                WOCostVisibility_AfterDeleting_CallForCurrentWorkorder(previousRecord, matchedLaborLine, toBeAddedOrUpdatedRecord);
            }
        }

        private static void WOCostVisibility_AfterDeleting_CallForCurrentWorkorder(NeMemberTime previousRecord, NeWODetailCurrent matchedLaborLine, NeWODetailCurrent toBeAddedOrUpdatedRecord)
        {
            /*
               Say OldTotal$ = matchedRecord.wo_detail_current_price_cost * matchedRecord.wo_detail_current_qty_committed;
               Say Reduced$ = previousRecord.MemberTime_CostPrice * previousRecord.NumberOfHours;
               Say Added$ = 0
               Say newTotal$ =  OldTotal$ - Reduced$ + Added$;
               Say TotalHour =  toBeUpdatedRecord.qty_committed.

               Then 
                   wo_detail_current_price_cost = newTotal$ / TotalHour.
            */

            var OldTotal = matchedLaborLine.cost * matchedLaborLine.qty_committed;
            var Reduced = previousRecord.MemberTime_CostPrice * previousRecord.NumberOfHours;
            var newTotal = OldTotal - Reduced;

            // follow the save you will find this logic about adding both together to get the true value. search: true_qty = old_qty + new_qty;
            var TotalHour = matchedLaborLine.qty_committed + toBeAddedOrUpdatedRecord.qty_committed;

            var average = 0.0;
            if (TotalHour != 0)
            {
                average = newTotal / TotalHour;
            }

            if (double.IsInfinity(average) || double.IsNaN(average))
            {
                return;
            }

            WOCostVisibility_UpdateCost(toBeAddedOrUpdatedRecord, average, TotalHour);
        }

        private static void WOCostVisibility_AfterDeleting_CallForParentWorkorde(NeMemberTime previousRecord, NeWODetailCurrent matchedLaborLine, NeWODetailCurrent toBeAddedOrUpdatedRecord, NeWODetailCurrent childMatchedLaborLine)
        {
            // Validation checking: child-parent relationship.
            if (previousRecord.child_woprog_id != matchedLaborLine.woprog_id)
            {
                return;
            }

            // Validation checking: same person
            if (previousRecord.member_id != matchedLaborLine.memberid)
            {
                return;
            }

            //
            // Get the chargeout$
            // 
            var charge_out_rate = WOCostVisibility_GetChargeoutValue(previousRecord.customer_id, previousRecord.membertype_chargeout_id, matchedLaborLine.id, childMatchedLaborLine.id);

            // Do job
            var OldTotal = matchedLaborLine.cost * matchedLaborLine.qty_committed;
            var Reduced = charge_out_rate * previousRecord.NumberOfHours;
            var newTotal = OldTotal - Reduced;

            var TotalHour = matchedLaborLine.qty_committed - previousRecord.NumberOfHours;

            var average = 0.0;
            if (TotalHour != 0)
            {
                average = newTotal / TotalHour;
            }

            if (double.IsInfinity(average) || double.IsNaN(average))
            {
                return;
            }

            WOCostVisibility_UpdateCost(toBeAddedOrUpdatedRecord, average, TotalHour);
        }
        #endregion

        private static bool IsPriceCostSame_CallForCurrrentWorkOrder(NeMemberTime timesheetRecord, NeWODetailCurrent laborRecord)
        {
            bool same = false;

            var diff = Math.Abs(timesheetRecord.MemberTime_CostPrice - laborRecord.cost);

            if (timesheetRecord.MemberTime_CostPrice == laborRecord.cost || diff <= 0.01) // After talk to Matt: Not care about 0.01 thing.
            {
                same = true;
            }

            return same;
        }

        private static void WOCostVisibility_UpdateCost(NeWODetailCurrent laborRecord, double average, double totalHour)
        {
            if (laborRecord == null)
            {
                return;
            }

            if (totalHour == 0)
            {
                // In this case, the total hour is zero, so average will be kept. 
                return;
            }

            laborRecord.cost = average;
        }

        private static double WOCostVisibility_GetChargeoutValue(int customerID, int chargeoutID, int detail_id, int? childDetail_id)
        {
            //
            // Get the chargeout$:
            // The chargeout$ is not stable from parent labor line (wo_detail_current table) if averaging on cost happened before. 
            //
            // Then how to get the original $?
            //
            // Solution from Matt: 
            // First can call: SELECT* FROM log.wo_detail_current WHERE wo_detail_current_id = 1269309 AND EVENT = "insert";
            // If no valid $ returned, try to call CUSTOMER_CHARGEOUT.
            //

            var charge_out_rate = 0.0;
            var template = @"SELECT IFNULL(MAX(wo_detail_current_price_cost), 0) FROM log.wo_detail_current WHERE wo_detail_current_id = {0} AND EVENT = 'insert'";
            var sql = string.Format(template, detail_id);

            charge_out_rate = Toolbox.doSQL_double(sql, new object[] { });

            if (charge_out_rate <= 0)
            {
                charge_out_rate = Toolbox.doSQL_double(
                    @"CALL CUSTOMER_CHARGEOUT(@v0 ,@v1 )",
                    new object[] { customerID, chargeoutID });
            }
            else if(childDetail_id.HasValue)
            {
                //Check if the sell price has been changed manully 
                //IFNULL(MAX(RIGHT(the_change, INSTR(REVERSE(the_change), '$') - 1)), -1) -gets the right part of the strings similar to 
                //"Some One - Manually changed the sell price from $90.00 to $180.00' after the last '$' or -1 if there is no data for manual change
                var query = @"SELECT IFNULL(MAX(RIGHT(the_change, INSTR(REVERSE(the_change), '$') - 1)), -1) new_value
                             FROM (SELECT the_change 
                                   FROM woprogchanges_snapshot
                                   WHERE wo_detail_current_id = @v0 AND the_change LIKE '%Manually changed the sell price from%'
                                   ORDER BY id desc LIMIT 1) AS temp";
                var manuallyChangedPrice = Toolbox.doSQL_double(query, new object[] { childDetail_id.Value });
                if(manuallyChangedPrice > 0)
                {
                    charge_out_rate = manuallyChangedPrice;
                }
            }
            return charge_out_rate;
        }

        private static void Log(NeMemberTime previousRecord, NeMemberTime postRecord, NeWODetailCurrent matchedLaborLine, NeWODetailCurrent toBeAddedOrUpdatedRecord, bool calledForParentWorkOrder)
        {
            string info = "";
            string infoTemp = @"Operation on timesheet ( previousId = {0}  postId = {1} )
Before update laborline record (wo_detail_current)
cost = {2}
sell = {3}
masterId = {4}
hours = {5}
";
            int wo_detail_id = 0;
            int previousId = 0;
            int postId = 0;

            double cost = 0;
            double sell = 0;
            int masterId = 0;
            double hours = 0;

            try
            {
                if (matchedLaborLine != null)
                {
                    wo_detail_id = matchedLaborLine.id;
                    cost = matchedLaborLine.cost;
                    sell = matchedLaborLine.sell;
                    masterId = matchedLaborLine.master_id;
                    hours = matchedLaborLine.qty_committed;
                }

                if (previousRecord != null)
                {
                    previousId = previousRecord.MemberTimeID;
                }

                if (postRecord != null)
                {
                    postId = postRecord.MemberTimeID;
                }

                info = string.Format(infoTemp,
                    previousId,
                    postId,
                    cost,
                    sell,
                    masterId,
                    hours);
                
                var titleTemplate = "Error when preparing the averaging cost on labor line (wo_detail_id = {0})";
                var title = string.Format(titleTemplate, wo_detail_id);

                info = title + "\r\n" + info;
                Toolbox.do_errorLog_go(title, "", info);

                // Sending email.
                var e = new NeEMail();
                e.To = "debug@" + Toolbox.app_setting("DomainForEmail");
                e.From = "admin@" + Toolbox.app_setting("DomainForEmail");
                e.Subject = "Error when preparing the averaging cost";
                e.isHTML = true;
                e.Body = info.Replace("\r\n", "<br/>");
                e.Send();
            }
            catch (Exception ex)
            {
            }
        }

        // 
        // 
        // There is no timesheet record for current workorder labor line if it has a child workorder (when creating an timesheet record against child workorder, only timesheet record is created for child workorder).
        // But we can calculate one.
        //
        //private static int CalculateParentShadowTimesheetRecord(curreentLaborLine)
        //{
        //    In order to get the shadow timesheet record from child work order's timesheet contritbution.
        //    First to get the hours from labor line curreentLaborLine: say total, 25
        //    Then to get all accumulated hours from all related time sheet records (from current work order's view), say HoursContributedByTimesheetRecord = 16
        //    Then the shadowHour = 25 - 16 = 9
        //    
        //    Then get the chargeout$ by using child work order info. How? kind of WOCostVisibility_GetChargeoutValue.
        //    
        //    Finally have:
        //      Shadow timesheet record (shadowHour, chargeout$)
        //      
        //    
        //    return 0;
        //}

        #endregion
    }

    public class Chargeout
        {
        public int id { get; set;}
        public string type { get; set;}
        public int membertype_id { get; set;}
        public int paytype_id { get; set;}
        public double chargeout { get; set;}
        public int business_unit_id { get; set;}


        public Chargeout(int _chargeOutId)
            {
            Load(_chargeOutId);
            }
        public Chargeout(string typeChar, int businessUnitId, int memberTypeId, int payTypeId)
            {
            var chargeoutId = Toolbox.doSQL_int("SELECT IFNULL(MAX(id), 0) FROM chargeout WHERE type = @v0 AND business_unit_id = @v1 AND membertype_id = @v2 AND paytype_id = @v3", new object[]{typeChar, businessUnitId, memberTypeId, payTypeId});
            if(chargeoutId == 0)
                {
                var bu = new NeBusinessUnit(businessUnitId);
                var body = $"A chargeout just tried to be retrieved for the business unit {bu.ddl_name}, though it doesn't exist... this most likely means that there is a one sided entry out there, either a parent/child labor entry or a time sheet record that isn't on the work order - This message is also being copied to the debugging crew";
                shared.alert_ar("Spark Ops Alerts - Chargeout warning!", body);
                shared.alert_debug("Spark Ops Alerts - Chargeout warning!", body);
                throw new Exception($"Chargeout doesn't exist for {bu.ddl_name}");
                }
            Load(chargeoutId);
            }
        private void Load(int _id)
            {
            var chargeoutValues = Toolbox.doSQL_dt("SELECT * FROM chargeout WHERE id = @v0", new object[]{ _id });
            foreach(DataRow dr in chargeoutValues.Rows)
                {
                id = _id;
                type = Toolbox.ReturnBlankIfNull_string(dr["type"]);
                membertype_id = Toolbox.ReturnZeroIfNull_int(dr["membertype_id"]);
                paytype_id = Toolbox.ReturnZeroIfNull_int(dr["paytype_id"]);
                chargeout = Toolbox.ReturnZeroIfNull_double(dr["chargeout"]);
                business_unit_id = Toolbox.ReturnZeroIfNull_int(dr["business_unit_id"]);
                }
            }
        }
}