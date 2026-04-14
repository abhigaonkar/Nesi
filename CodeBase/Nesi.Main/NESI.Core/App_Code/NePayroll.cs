using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using DevExpress.Xpo;
using MySql.Data.MySqlClient;
using System.IO;
using System.Web;
using NESI.Common.Models;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NePayroll
	/// </summary>
	public class payroll
	{
		public int manager_id;
		public string next_id = "";
		public string prev_id = "";
		public string first_id = "";
		public string last_id = "";
		public int admin_id;
		public string start_date = "";
		public string end_date = "";

		private string _sql = "";
		public int id { get; set; }
		public int payperiod_id { get; set; }
		public int business_unit_id { get; set; }
		public int member_id { get; set; }
		public bool sent { get; set; }
		public DateTime? sent_date { get; set; }
		public payroll() { }
		public payroll(int _id)
		{
			load(_id);
		}
		public payroll(int _business_unit_id, int _payperiod_id)
		{
			load(_business_unit_id, _payperiod_id);
		}
		public static bool exists(int _id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll WHERE id = @v0", _id) > 0;
		}
		public static bool exists(int _business_unit_id, int _payperiod_id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll WHERE business_unit_id = @v0 AND payperiod_id = @v1", new object[] { _business_unit_id, _payperiod_id }) > 0;
		}
		public static void finalize(int _business_unit_id, int _payperiod_id, int _member_id, bool _do_finalize)
		{
            var p = new payroll(_business_unit_id, _payperiod_id)
            {
                admin_id = _member_id,
                sent = _do_finalize,
                sent_date = DateTime.Now
            };
            p.save();
            if(_do_finalize)
            {
			Toolbox.doSQL_void(@"UPDATE payroll_progress SET sent = 1 WHERE payperiod_id = @v0 AND business_unit_id = @v1 AND member_id = @v2", new object[] {_payperiod_id, _business_unit_id, _member_id});
            }
		}
		private void load(int _id)
		{
			if (!exists(_id)) return;
			var dr = Toolbox.doSQL_dt(@"SELECT * FROM payroll WHERE id = @v0 ", new object[] { _id }).Rows[0];
			id = _id;
			payperiod_id = Convert.ToInt32(dr["payperiod_id"]);
			business_unit_id = Convert.ToInt32(dr["business_unit_id"]);
			admin_id = Convert.ToInt32(dr["member_id"]);
			sent = Convert.ToBoolean(dr["sent"]);
			sent_date = (DateTime) dr["sent_date"];
		}
        public void save()
        {
            if(id == 0)
            {
			Toolbox.doSQL_void(@"INSERT INTO payroll (payperiod_id, business_unit_id, member_id, sent, sent_date) VALUES (@v0,@v1,@v2, @v3, @v4)", new object[] { payperiod_id, business_unit_id, member_id, sent, sent_date });
            }
            else
            {
            Toolbox.doSQL_void(@"UPDATE payroll SET sent = @v0, sent_date = @v1 WHERE id = @v2", new object[] { sent, sent_date, id });
            }
        }
		public void load(int _business_unit_id, int _payperiod_id)
		{
			payperiod_id = _payperiod_id;
			business_unit_id = _business_unit_id;
			if (!exists(_business_unit_id, _payperiod_id)) return;
			var dr = Toolbox.doSQL_dt(@"SELECT * FROM payroll WHERE business_unit_id = @v0  AND payperiod_id = @v1 ", new object[] { _business_unit_id, _payperiod_id }).Rows[0];
			id = Convert.ToInt32(dr["id"]);
			admin_id = Convert.ToInt32(dr["member_id"]);
			sent = Convert.ToBoolean(dr["sent"]);
			sent_date = Toolbox.ReturnBlankDateTimeIfNull(dr["sent_date"]);
		}
		#region string
		public static List<int> get_full_list(int _payperiod_id, int _business_unit_id, int _admin_id, bool _remove_completed_users, int _show_all_type = 2)
		{
			var full_list = new List<int>();
			using (var uow = new UnitOfWork())
			{
				using (var conn = Toolbox.connect())
				{
					var payperiod = new NePayPeriod(_payperiod_id);
					var str_handler_list = Toolbox.doSQL_string(conn, string.Format(@"SELECT GROUP_CONCAT(a.member_id) 
FROM member a WHERE IF({0} = 0, true, (a.payroll_handler = {0} OR a.payroll_handler = 0)) 
AND a.business_unit_id = {1} AND (a.member_termdate > {2} 
OR a.member_termdate BETWEEN '{2}' AND '{3}')", _admin_id, _business_unit_id, payperiod.StartDate, payperiod.Enddate), null);

					var handler_list = str_handler_list == "" ? new List<int>() : str_handler_list.Split(',').Select(int.Parse).ToList();

					var str_reports_to_list = Toolbox.doSQL_string(conn, @"SELECT REPORTS_TO_SANS_STATUS 
(@v0, @v1, @v2, @v3, @v4)", new object[] {
						_admin_id, _business_unit_id, payperiod.StartDate, payperiod.Enddate, _show_all_type});

					var reports_to_list = str_reports_to_list == "" ? new List<int>() : str_reports_to_list.Split(',').Select(int.Parse).ToList();
					if (handler_list.Any())
					{
						foreach (var i in handler_list)
						{
							if (!reports_to_list.Contains(i))
							{
								reports_to_list.Add(i);
							}
						}
					}
					if (reports_to_list.Any())
					{
						foreach (var i in reports_to_list.OrderBy(u => u))
						{
							var been_approved = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM payroll_hours WHERE member_id = @v0 AND payperiod_id = @v1 ", new object[] {
								i, payperiod.payperiodID}) > 0;
							if (been_approved && _remove_completed_users) continue;
							var employee = uow.GetObjectByKey<ne_xpo.cs.member>(i);
							var include = employee.member_status == "Active" // Active users
														|| employee.member_termdate >= payperiod.start_date && employee.member_termdate <= payperiod.end_date // Users that were terminated mid-payroll
														|| employee.member_termdate >= payperiod.start_date && employee.member_termdate <= DateTime.Now.Date // Users that were terminated after payperiod ended, but are still part of the same pay period (Sunday/Monday, or possibly S/M/T on certain holidays)
														;
							if (!include) continue;
							full_list.Add(i);
						}
					}
				}
			}
			return full_list;
		}
		public static int get_user_at(List<int> _full_list, int _payperiod_id, int _business_unit_id, int _admin_id, int _working_user, int _type_id)
		{
			var user_id = 0;
			var length = _full_list.Count - 1; // Zero based
			var working_index = _full_list.IndexOf(_working_user);
			var used_index = 0;
			if (_type_id == 1) // Prev
			{
				used_index = working_index - 1;
				if (used_index < 0)
				{
					used_index = length;
				}
				user_id = _full_list[used_index];
			}
			else if (_type_id == 2) // Next
			{
				used_index = working_index + 1;
				if (used_index > length)
				{
					used_index = 0;
				}
				user_id = _full_list[used_index];
			}
			else if (_type_id == 3) // First
			{
				user_id = _full_list[0];
			}
			else if (_type_id == 4) // Last
			{
				user_id = _full_list[length];
			}
			return Convert.ToInt32(user_id);
		}
		public static int paytype_at_date(MySqlConnection _conn, DateTime _dt_end, int _member_id)
		{
			var paytype_id = Toolbox.doSQL_int(_conn, @"SELECT IFNULL(MAX(paytype_id), (SELECT paytype_id FROM member WHERE member_id = @v1)) 
FROM (SELECT paytype_id FROM log.member where member_id = @v1 and ts < @v0 ORDER BY id DESC LIMIT 1) hist", new object[] {
				Toolbox.MySQL_shortdt(_dt_end), _member_id});
			return paytype_id;
		}

		public string working_member()
		{
			return Toolbox.doSQL_string(@"SELECT current_member FROM payroll WHERE 
member_id = @v0 AND business_unit_id = @v1 
				AND payperiod_id = @v2 ", new object[] { admin_id,business_unit_id
					, payperiod_id
			});
		}
		public string first_member()
		{
			return Toolbox.doSQL_string(@"CALL PAYROLL_MEMBERS(@v0, @v1, @v2, @v3, @v4)", new object[] { payperiod_id, business_unit_id, 0, "F", admin_id });
		}
		public string last_member()
		{
			return Toolbox.doSQL_string(@"CALL PAYROLL_MEMBERS(@v0, @v1, @v2, @v3, @v4)", new object[] { payperiod_id, business_unit_id, 0, "L", admin_id });
		}
		public string previous_member()
		{
			return Toolbox.doSQL_string(@"CALL PAYROLL_MEMBERS(@v0, @v1, @v2, @v3, @v4)", new object[] { payperiod_id, business_unit_id, member_id, "P", admin_id });
		}
		public string next_member()
		{
			return Toolbox.doSQL_string(@"CALL PAYROLL_MEMBERS(@v0, @v1, @v2, @v3, @v4)", new object[] { payperiod_id, business_unit_id, member_id, "N", admin_id });
		}
		public string working_pay_period()
		{
			return Toolbox.doSQL_string("CALL _payperiod");
		}
		#endregion string
		#region void
		public void initialize()
		{
			prev_id = previous_member();
			if (prev_id == "9999999")
			{
				prev_id = last_member();
			}
			next_id = next_member();
			if (next_id == "9999999")
			{
				next_id = first_member();
			}

			first_id = first_member();
			last_id = last_member();
		}

		public static double get_wage_at_date(MySqlConnection _conn, int _member_id, DateTime _dt)
		{
			var wage = Toolbox.doSQL_double(_conn, @"SELECT IFNULL(currentwage, 0) FROM (SELECT 0 currentwage, 2 o_id, '0000-00-00' date UNION SELECT currentwage, 1 o_id, date 
FROM memberwage WHERE memberwage_memberid = @v1 AND date < @v0) _wage ORDER BY o_id ASC, date DESC LIMIT 1", new object[] { _dt, _member_id });
			if (wage > 400)
			{
				wage = wage / 80;
			}
			return wage;
		}
		public static string get_company_code(string _business_number)
		{
			return Toolbox.doSQL_string(@"SELECT adp_company_code FROM tax_entity WHERE id =@v0", _business_number);
		}
		public void clear_working_member()
		{
			Toolbox.doSQL_void(@"UPDATE payroll 
SET current_member = 0 WHERE member_id = @v0 AND business_unit_id = @v1 AND payperiod_id = @v2", new object[] { admin_id, business_unit_id, payperiod_id });
		}
		public void start_header()
		{
			Toolbox.doSQL_void(@"INSERT INTO payroll 
(payperiod_id, business_unit_id, member_id, sent, current_member) 
VALUES (@v0,@v1,@v2,0, 0)", new object[] { payperiod_id, business_unit_id, admin_id });
		}
		public void update_header()
		{
			if (sent)
			{
				Toolbox.doSQL_void(@"UPDATE payroll SET sent = 1, sent_date = now(), member_id = @v0 WHERE payperiod_id = @v1 AND business_unit_id = @v2 ",
					new object[] { admin_id, payperiod_id, business_unit_id });
			}
			else
			{
				Toolbox.doSQL_void(@"UPDATE payroll SET current_member = @v0 WHERE payperiod_id = @v1 AND business_unit_id = @v2 AND member_id = @v3 ",
					new object[] { member_id, payperiod_id, business_unit_id, admin_id });
			}
		}

		#endregion void
		#region bool
		public bool payroll_exists()
		{
			var exist_check = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll WHERE payperiod_id =@v0 AND business_unit_id =@v1 ",
				new object[] {
					payperiod_id,business_unit_id
					}
				 );
			if (exist_check > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public bool user_entry_exists()
		{
			var check = Toolbox.doSQL_int("SELECT COUNT(*) FROM payroll_hours WHERE member_id =@v0 AND payperiod_id =@v1 ", new object[] { member_id, payperiod_id });
			if (check > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public static bool applicable_for_holiday_pay(int _member_id)
		{
			return Toolbox.doSQL_int(@"
SELECT 
	IF(COUNT(*), 1, 0) result 
FROM 
	member 
WHERE 
	member_id = @v0 AND
	receive_stat_pay = 1 AND
	paytype_id in (1, 2) AND 
	DATEDIFF(CURDATE(), member_startdate) >= member_time_to_stat", _member_id) == 1;
		}
		public bool this_branches_payroll_is_complete()
		{
			var exist_check = Toolbox.doSQL_int(@"SELECT COUNT(business_unit_id) COUNT FROM payroll WHERE business_unit_id =@v0 
AND payperiod_id =@v1  AND sent = 1",
				new object[] {
					business_unit_id,payperiod_id
					}
);
			return exist_check > 0;
		}
		public bool this_pay_period_is_complete()
		{
			var exist_check = Toolbox.doSQL_int(@"SELECT count(PayperiodID) FROM payperiods WHERE EndDate < now() AND completed = 0 limit 1");
			return exist_check == 0;
		}
		#endregion bool
		#region integer
		public int available_users()
		{
			return Toolbox.doSQL_int(@"CALL PAYROLL_MEMBERS(@v0, @v1, NULL, @v2, @v3)", new object[] { payperiod_id, business_unit_id, "A", admin_id });
		}
		public static int current_pay_period()
		{
			return Toolbox.doSQL_int("CALL _payperiod()");
		}
		#endregion
		#region double
		public double current_wage()
		{
			return Toolbox.doSQL_double(@"SELECT wage FROM currentwage  WHERE member_id = @v0", new object[] { member_id });
		}
		public double commission()
		{
			return Toolbox.doSQL_double(@"SELECT IFNULL(SUM(amount), 0) amount FROM payroll_extra_payments WHERE member_id = @v0  AND type = 'C' AND requested_by = @v2  AND approved = 1 AND payperiod_id = @v1 ", new object[] { member_id, payperiod_id, admin_id });
		}
		public double bonus()
		{
			return Toolbox.doSQL_double(@"SELECT IFNULL(SUM(amount), 0) amount FROM payroll_extra_payments WHERE member_id = @v0  AND type = 'B' AND requested_by = @v2  AND approved = 1 AND payperiod_id = @v1 ", new object[] { member_id, payperiod_id, admin_id });
		}
		public static double amount_of_holiday_pay(int _member_id, int _payperiod_id)
		{
			return Toolbox.doSQL_double(@" SELECT IFNULL(CAST(IF(SUM(subquery.hours)/20 > 8, 8, round(SUM(subquery.hours)/20*60/15,0) / 4) as decimal(8,2)), 0) hours FROM ( SELECT sub_a.rt+sub_a.ot+sub_a.dt+sub_a.rtsp+sub_a.otsp+sub_a.dtsp+ifnull(sub_b.hours, 0)+ifnull(sub_c.payment_amount, 0) hours FROM payroll_hours sub_a LEFT JOIN bankedpay_ledger sub_b ON sub_a.member_id = sub_b.member_id AND sub_b.payperiod_id = sub_a.payperiod_id AND sub_b.type = 'W' LEFT JOIN vacation_master sub_c ON sub_a.member_id = sub_c.member_id AND sub_c.payperiod_id = sub_a.payperiod_id AND sub_c.payment_method = 2 AND sub_c.status = 3 WHERE sub_a.payperiod_id in (SELECT sub_sub_a.payperiodid FROM payperiods sub_sub_a WHERE sub_sub_a.payperiodid < @v1 ) AND sub_a.member_id = @v0  ORDER BY sub_a.payperiod_id DESC limit 2) subquery", new object[] { _member_id, _payperiod_id });
		}
		#endregion
		public static class extra_payments
		{
			public static double get(MySqlConnection _conn, int _member_id, string _type, int _payperiod_id)
			{
				return Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(amount), 0) amount FROM payroll_extra_payments WHERE member_id = @v0  AND payperiod_id = @v1 AND type = @v2  AND approved = 1", new object[] { _member_id, _payperiod_id, _type });
			}
		}
		public class hours
		{
			public int id { get; set; }
			public int business_unit_id { get; set; }
			public int dept_head_id { get; set; }
			public int payperiod_id { get; set; }
			public int member_id { get; set; }
			public double rt { get; set; }
			public double ot { get; set; }
			public double dt { get; set; }
			public double rtsp { get; set; }
			public double otsp { get; set; }
			public double dtsp { get; set; }
			public double payrate { get; set; }
			public double vac { get; set; }
			public double stat { get; set; }
			public hours(int _id)
			{
				if (exists(_id))
				{
					load(_id);
				}
			}
			public struct HourTypes
				{
				public const string RegularTime = "rt";
				public const string OverTime = "ot";
				public const string DoubleTime = "dt";
				public const string RegularTimeShiftPremium = "rtsp";
				public const string OverTimeShiftPremium = "otsp";
				public const string DoubleTimeShiftPremium = "dtsp";
				}
			private void load(int _id)
			{
				using (var conn = Toolbox.connect())
				{
					var dr = Toolbox.doSQL_dt(conn, @"SELECT * FROM payroll_hours  WHERE id = @v0", new object[] { _id }).Rows[0];
					id = _id;
					business_unit_id = (int)dr["business_unit_id"];
					dept_head_id = Convert.ToInt32(dr["dept_head_id"]);
					payperiod_id = Convert.ToInt32(dr["payperiod_id"]);
					member_id = Convert.ToInt32(dr["member_id"]);
					rt = Toolbox.ReturnZeroIfNull_double(dr["rt"]);
					ot = Toolbox.ReturnZeroIfNull_double(dr["ot"]);
					dt = Toolbox.ReturnZeroIfNull_double(dr["dt"]);
					rtsp = Toolbox.ReturnZeroIfNull_double(dr["rtsp"]);
					otsp = Toolbox.ReturnZeroIfNull_double(dr["otsp"]);
					dtsp = Toolbox.ReturnZeroIfNull_double(dr["dtsp"]);
					payrate = Toolbox.ReturnZeroIfNull_double(dr["payrate"]);
					vac = Toolbox.ReturnZeroIfNull_double(dr["vac"]);
					stat = Toolbox.ReturnZeroIfNull_double(dr["stat"]);
				}
			}
			private bool exists(int _id)
			{
				return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll_hours WHERE id = @v0", _id) > 0;
			}
			public static void clear(object _row_id)
			{
					Toolbox.doSQL_void(@"DELETE FROM payroll_hours WHERE id = @v0 LIMIT 1", new object[] { _row_id });
			}
			public static double HistoricTotalByHourType(MySqlConnection conn, int payPeriodId, int memberId, string field)
				{
				return Toolbox.doSQL_double(conn, $@"SELECT IFNULL(SUM({field}*payrate),0) FROM payroll_hours WHERE payperiod_id = @v0 AND member_id = @v1", new object[]{payPeriodId, memberId });
				}
			public static double CurrentTotalByHourType(MySqlConnection conn, int payPeriodId, int memberId, int hourType)
				{
				return Toolbox.doSQL_double(conn, @"SELECT SUM(numberofhours) FROM membertime WHERE membertime_memberid = @v0 AND membertime_paytypehours_id = @v1 AND payperiod_id = @v2", new object[]{memberId, hourType, payPeriodId});
				}
		}
		public static bool HourEntryExists(int currentEmployeeId, int payPeriodId)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) 
FROM payroll_hours 
WHERE member_id = @v0 AND payperiod_id = @v1", new object[] { currentEmployeeId, payPeriodId }) > 0;
		}
		public static int how_many_holidays(string _country, string _start_date, string _end_date)
		{
			var is_canadian = _country == "CDN" || _country == "CAN";
			return Toolbox.doSQL_int(string.Format(@"SELECT IFNULL(COUNT(holidays_id), 0) c 
FROM holidays WHERE holidays_date >= @v0 AND holidays_date <=@v1 AND holidays_{0} = 1 AND holidays_active = 1", is_canadian ? "canada" : "america"), new object[] { _start_date, _end_date });
		}
		public static void submit_approved_pay(approval_packet _app_packet, hour_packet _hr_packet)
			{
			Toolbox.doSQL_void(@"
INSERT INTO payroll_hours 
	(
	business_unit_id, 
	dept_head_id, 
	timestamp, 
	payperiod_id, 
	member_id, 
	rt, 
	ot, 
	dt, 
	rtsp, 
	otsp, 
	dtsp,
	payrate,
	stat, 

	rt_original, 
	ot_original, 
	dt_original, 
	rtsp_original, 
	otsp_original, 
	dtsp_original
	) 
VALUES  
	(
	@v0,
	@v1,
	NOW(),
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

	@v12,
	@v13,
	@v14,
	@v15,
	@v16,
	@v17
	)",

				new object[] {
								_app_packet.current_employee.business_unit_id,		// 0
								_app_packet.handler.id,								// 1
								_app_packet.payperiod_id,							// 2
								_app_packet.current_employee.id,					// 3
								_hr_packet.RegularTime,								// 4
								_hr_packet.OverTime,								// 5
								_hr_packet.DoubleTime,								// 6
								_hr_packet.RegularTimeShiftPremium,					// 7
								_hr_packet.OverTimeShiftPremium,					// 8
								_hr_packet.DoubleTimeShiftPremium,					// 9
								_app_packet.wage,									// 10
								_hr_packet.StatPay,									// 11

								_hr_packet.rt_original,								// 12
								_hr_packet.ot_original,								// 13
								_hr_packet.dt_original,								// 14
								_hr_packet.rtsp_original,							// 15
								_hr_packet.otsp_original,							// 16
								_hr_packet.dtsp_original							// 17
								});		
			// MH 2020-11-04 - Commenting this out until it's completed.											   
			//if(_app_packet.current_employee.ReceivesAutoVacationPayout)				   
			//	{ 		
				//vacation.AutoVacationPayout(_app_packet, _hr_packet);				   
			//	}	
			}																		   
		public struct hour_packet													   
		{																			   
																					   
			public double RegularTime { get; set; }									   
			public double OverTime { get; set; }									   
			public double DoubleTime { get; set; }									   
			public double RegularTimeShiftPremium { get; set; }						   
			public double OverTimeShiftPremium { get; set; }						   
			public double DoubleTimeShiftPremium { get; set; }						   
			public double UnpaidVacation { get; set; }								   
			public double UnpaidSickDay { get; set; }
			public double PaidSickDay { get; set; }
			public double UnpaidOther { get; set; }
			
			public double rt_original { get; set; }
			public double ot_original { get; set; }
			public double dt_original { get; set; }
			public double rtsp_original { get; set; }
			public double otsp_original { get; set; }
			public double dtsp_original { get; set; }

			public double StatPay { get; set; }
			public double banked_withdrawn { get; set; }
			public double banked_deposited { get; set; }
			public double banked_deducted { get; set; }
			public double banked_paidout { get; set; }
			public double vacation_hours { get; set; }
			public double vacation_dollars { get; set; }
			public double Miscellaneous { get; set; }
			public double Expense { get; set; }
			public double Commission { get; set; }
			public double Bonus { get; set; }
			public double Bereavement { get; set;}
			public double Birthday { get; set;}
            public double PerDiems { get; set; }

        }
		public class paytype
		{
			public int id { get; set; }
			public string name { get; set; }
			public paytype() { }
			public paytype(int _id)
			{
				using (var conn = Toolbox.connect())
				{
					if (exists(conn, _id))
					{
						load(conn, _id);
					}
				}
			}
			public paytype(MySqlConnection _conn, int _id)
			{
				if (exists(_conn, _id))
				{
					load(_conn, _id);
				}
			}
			private bool exists(MySqlConnection _conn, int _id)
			{
				return Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) FROM member_paytype WHERE id = @v0", new object[] { _id }) > 0;
			}
			private void load(MySqlConnection _conn, int _id)
			{
				id = _id;
				name = Toolbox.doSQL_string(_conn, @"SELECT paytype FROM member_paytype WHERE id = @v0", new object[] { _id });
			}
		}
		public class hr_status
		{
			public int id { get; set; }
			public string name { get; set; }
			public string description { get; set; }
			public hr_status() { }
			public hr_status(int _id)
			{
				using (var conn = Toolbox.connect())
				{
					if (exists(conn, _id))
					{
						load(conn, _id);
					}
				}
			}
			public hr_status(MySqlConnection _conn, int _id)
			{
				if (exists(_conn, _id))
				{
					load(_conn, _id);
				}
			}
			private bool exists(MySqlConnection _conn, int _id)
			{
				return Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) FROM member_hrstatus WHERE id = @v0", new object[] { _id }) > 0;
			}
			private void load(MySqlConnection _conn, int _id)
			{
				id = _id;
				var dr = Toolbox.doSQL_dt(_conn, @"SELECT * FROM member_hrstatus WHERE id = @v0 ", new object[] { _id }).Rows[0];
				name = dr["status"].ToString();
				description = dr["description"].ToString();
			}
		}
		public class approval_packet
		{
			public int payperiod_id { get; set; }
			public NeMember handler { get; set; }
			public int business_unit_id { get; set; }
			public List<int> full_branch_list { get; set; }
			public NeMember current_employee { get; set; }
			public List<int> your_to_approve { get; set; }
			public List<int> your_to_approve_full { get; set; }
			public double wage { get; set; }
			public bool is_sent { get; set; }
		}
		public class DaysOff
			{
			public static double ThisPayPeriod(MySqlConnection _conn, int _member_id, int _payperiod, bool isPaid)
				{
				// sql to get hour total for payperiod
				var paid_sick_time = Toolbox.doSQL_double(_conn, @"
SELECT 
		IFNULL(SUM(TIMESTAMPDIFF(MINUTE, date_start, date_end))/60, 0) h
	FROM
		vacation_master a
	INNER JOIN
		vacation_type b ON a.type_id = b.id AND b.paid = @v2
	WHERE
		member_id = @v0 AND
		payperiod_id = @v1 AND
		type_id != 4 AND
		STATUS IN(3, 5)", new object[] { _member_id, _payperiod, isPaid});

				return paid_sick_time;
				}
			public static double ThisPayPeriod(MySqlConnection _conn, int _member_id, int _payperiod, int daysOffType)
				{
				// sql to get hour total for payperiod


				var paid_sick_time = Toolbox.doSQL_double(_conn, @"SELECT 
		IFNULL(SUM(TIMESTAMPDIFF(MINUTE, date_start, date_end)/60), 0) h
	FROM
		vacation_master
	WHERE
		member_id = @v0 AND
		type_id = @v2 AND
		STATUS IN(3, 5) AND
		payperiod_id = @v1", new object[] { _member_id, _payperiod, daysOffType });

				return paid_sick_time;
				}

			}
		public class vacation
		{
			public static double this_payperiod(MySqlConnection _conn, bool _is_payout, int _member_id, int _payperiod_id, bool _is_complete = false, bool forApproval = false)
			{
				// This will return the # of HOURS to pay out.
				using (var uow = new UnitOfWork())
				{
					var employee = uow.GetObjectByKey<ne_xpo.cs.member>(_member_id);
					if (employee == null) return 0;
					if(_is_complete && !_is_payout) // We don't want to compound the vacation... it should only return once
						{
						var ph = (from p in new XPQuery<ne_xpo.cs.payroll_hours>(uow)
								 where p.member_id == _member_id && p.payperiod_id == _payperiod_id
									select p).FirstOrDefault();
						if(ph == null) return 0;
						return ph.vac;
						}
					var availableHoliday = employee.member_holiday;
					var isCanadian = employee.member_country != "USA"; // Doing it this way because there has been a variance between CDN & CAN abbreviations... it's always USA.
					var wage = !isCanadian
													? 0 // This is being used to normalize to an hourly figure, USA is already hourly.
													: NeWage.GetEmployeeWage(_conn, _member_id);
					if (isCanadian)
					{
						availableHoliday = Math.Round(availableHoliday / wage, MidpointRounding.AwayFromZero);
					}

					var year_check = _is_payout ? "<" : ">";
					// Check to see if they had an all outstanding vacation request for the requested payroll
					var outstanding = check_all_outstanding(_conn, _member_id, _payperiod_id); // Denotes there really is one out there
																							   //var outstanding_withdrawal	= outstanding && check_all_outstanding_withdrawal(_conn, _member_id, _payperiod_id); // Denotes that the request is a withdrawal
					var outstanding_vacation = outstanding && check_all_outstanding_vacation(_conn, _member_id, _payperiod_id); // Denotes that the requess is a vacation request
					var tmpOutstanding = 0.0;
					var amount = outstanding
													? 0
													: Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(payment_amount),0) FROM vacation_master WHERE member_id = @v0  AND payperiod_id = @v1  AND STATUS IN (3,5) AND type_id = 4 AND payment_method = 2 AND YEAR(date_start) " + year_check + @"  2000", new object[] { _member_id, _payperiod_id });
					if (outstanding)
					{
						tmpOutstanding = availableHoliday;
					}
					if (tmpOutstanding > availableHoliday) // We need to check to make sure their payment amount (8 hours per day) is within their available hours from their holiday field.
					{
						tmpOutstanding = availableHoliday >= 0    // If it is greater than or equal to zero
											? availableHoliday    // Use the available hours
											: 0;                // Otherwise they get a whopping zero.
					}
					if (_is_payout) // All outstanding vacation requests always take priority over payouts... payouts always happen SECOND
					{
						if (outstanding)
						{
							amount = outstanding_vacation
										? 0
										: tmpOutstanding;
						}
					}
					else if (outstanding) // This is checking if the vacation request is for all outstanding
					{
						amount = outstanding_vacation
									? tmpOutstanding
									: 0;
					}
					if(forApproval)
						{
						return amount;
						}
					return outstanding_vacation             // If it's outstanding...
								? amount                    // Return whatever was in their holiday field
								: amount > availableHoliday   // Otherwise, check if the amount is greater than what is in their holiday field
									? availableHoliday >= 0   // If it is, check to make sure what is in their holiday field is positive
										? availableHoliday    // It is positive, return it
										: 0                 // Otherwise return 0
									: amount;               // This is outside of the available hours check, so it's valid, just return it.
				}
			}

			private static bool check_all_outstanding_vacation(MySqlConnection _conn, int _member_id, int _payperiod_id)
			{
				var c = Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) 
FROM vacation_master WHERE member_id = @v0 AND status in (3,5) AND (payment_method = 3 OR payment_amount = -1) AND payperiod_id = @v1 AND YEAR(date_start) > 2000", new object[] { _member_id, _payperiod_id });
				return c > 0;
			}

			private static bool check_all_outstanding_withdrawal(MySqlConnection _conn, int _member_id, int _payperiod_id)
			{
				var c = Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) 
FROM vacation_master WHERE member_id = @v0 AND status in (3,5) AND (payment_method = 3 OR payment_amount = -1) AND payperiod_id = @v1 AND YEAR(date_start) < 2000", new object[] { _member_id, _payperiod_id });
				return c > 0;
			}
			public static bool check_all_outstanding(MySqlConnection _conn, int _member_id, int _payperiod_id)
			{
				var c = Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) 
FROM vacation_master WHERE member_id = @v0 AND status in (3,5) AND (payment_method = 3 OR payment_amount = -1) AND payperiod_id = @v1", new object[] { _member_id, _payperiod_id });
				return c > 0;
			}
			public static double unpaid_this_payperiod(int _member_id, int _payperiod_id)
			{
				return Toolbox.doSQL_double(@"SELECT IFNULL(SUM(payment_amount),0) FROM vacation_master WHERE member_id = @v0  AND payperiod_id = @v1  AND STATUS IN (3,5) AND type_id = 4 and payment_method = 1", new object[] { _member_id, _payperiod_id });
			}
			public static bool hasUnapprovedVacations(int _member_id, int _payperiod_id)
				{
				return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM vacation_master WHERE member_id = @v0 AND payperiod_id = @v1 AND status = 1") > 0;
				}
			public static bool has_been_paid_out(int _member_id, int _payperiod_id)
			{
				return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll_hours WHERE member_id = @v0 AND payperiod_id = @v1 AND vac > 0", new object[] { _member_id, _payperiod_id }) > 0;
			}
			public static void add_adjustment(MySqlConnection _conn, ref NeMember _user, NeMember _admin, string _action, double _adjustment_amount, bool _override = false)
			{
				var value_old = _user.Holiday;
				var value_new = _override ? _adjustment_amount : value_old + _adjustment_amount;
				if(value_old == value_new) return;

				var delta = value_new - value_old;
				_user.Holiday = value_new;
				_user.save();
				Toolbox.doSQL_void(_conn, @"
INSERT INTO vacation_transactions 
	(
	dt, 
	member_id, 
	action, 
	changed_by_id, 
	value_old, 
	value_new, 
	value_delta
	) 
VALUES 
	(
	NOW(), 
	@v0, 
	@v1, 
	@v2, 
	@v3, 
	@v4, 
	@v5
	)", new object[] {
					_user.id,
					_action,
					_admin.id,
					value_old,
					value_new,
					delta
				});
			}
		// MH 2020-11-04 - Commenting this out until it's completed.
		/*
			public const string PayoutComments = "Automatic Vacation Payout";
			public static bool PayoutExists(int memberId, int payPeriodId)
				{
				return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM vacation_master WHERE member_id = @v0 AND payperiod_id = @v1 AND comments = @v2", 
										new object[]
											{
											memberId,
											payPeriodId,
											PayoutComments
											}) > 0; 
				}
			public static void RemoveExistingPayout(approval_packet approvalPacket)
				{
				var payoutExists = PayoutExists(approvalPacket.current_employee.id, approvalPacket.payperiod_id);
				if(payoutExists)
					{
					Toolbox.doSQL_void(@"DELETE FROM vacation_master WHERE member_id = @v0 AND payperiod_id = @v1 AND comments = @v2",
						new object[]
							{
							approvalPacket.current_employee.id, 
							approvalPacket.payperiod_id, 
							PayoutComments
							});
					}
				}
			public static double GetAutoVacationPayout(int payPeriodId, int memberId)
				{
				using var conn = Toolbox.connect();
				var payPeriod = new NePayPeriod(payPeriodId);
				var amount    = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(payment_amount),0) FROM vacation_master WHERE payperiod_id = @v0 AND member_id = @v1 AND type_id = @v2", 
								new object[]
									{
									payPeriodId, 
									memberId, 
									OpsPayroll.Vacation.Type.AutoVacationPayout 
									}); // MH: 2020-11-02 - Using aggregate on purpose 
				var wage      = get_wage_at_date(conn, memberId, payPeriod.end_date.AddDays(-1));
				return wage == 0 ? wage : amount / wage;
				}
			public static void AutoVacationPayout(approval_packet approvalPacket, hour_packet hourPacket)
				{
				// MH 2020-10-29: Preventing USA employees from automatic payout UNTIL we're able to bring them over to per pay allocation
				if(approvalPacket.current_employee.CurrentVacationAmount > 1)
					return;
				RemoveExistingPayout(approvalPacket);
				// MH 2020-10-29: Create Total Item
				var vacationPayout = 	(hourPacket.RegularTime * OpsPayType.Multipliers.RegularTime + 
										hourPacket.OverTime * OpsPayType.Multipliers.OverTime + 
										hourPacket.DoubleTime * OpsPayType.Multipliers.DoubleTime + 
										hourPacket.RegularTimeShiftPremium * OpsPayType.Multipliers.RegularTimeShiftPremium + 
										hourPacket.OverTimeShiftPremium * OpsPayType.Multipliers.OverTimeShiftPremium + 
										hourPacket.DoubleTimeShiftPremium * OpsPayType.Multipliers.DoubleTimeShiftPremium + 
										hourPacket.StatPay) * approvalPacket.current_employee.CurrentVacationAmount;
				if(vacationPayout <= 0)
					return;
				var vacationItem = new VacationRequest
										{
										business_unit_id = approvalPacket.business_unit_id,
										create_member_id = OpsStaticEmployees.Administrator,
										payperiod_id = approvalPacket.payperiod_id,
										hours_requested = 0,
										member_id = approvalPacket.current_employee.id,
										status_id = OpsPayroll.Vacation.Status.Approved,
										separate_check = false,
										payment_amount = Math.Round(vacationPayout, 2),
										payment_method_id = OpsPayroll.Vacation.PaymentMethod.VacationPay,
										date_start = new DateTime(),
										vacation_id = 0,
										VacationType = OpsPayroll.Vacation.Type.AutoVacationPayout,
										Comments = PayoutComments
										};
				vacationItem.save_record();
				}
			*/
			public static void add_adjustment(MySqlConnection _conn, ref NeMember _user, NeMember _admin, string _action, double _adjustment_amount)
			{
				add_adjustment(_conn, ref _user, _admin, _action, _adjustment_amount, true);
			}

			public static void payout(int _member_id, int _payperiod_id)
			{
				using (var conn = Toolbox.connect())
				{
					//int _counter				= Toolbox.doSQL_int(conn,@"SELECT count(*) FROM bankedpay_ledger WHERE member_id = @v0  AND payperiod_id = @v1  AND type = 'A'", new object[] {  member_id, payperiod_id } );
					var employee = new NeMember(_member_id);
					#region definition queries
					/*
				double all_outstanding		= Toolbox.doSQL_double(conn,@" SELECT ( SELECT IFNULL(SUM(hours), 0) Deposited FROM bankedpay_ledger WHERE member_id = @v0  AND type = 'V' ) - ( SELECT IFNULL(SUM(hours), 0) Withdrawn FROM bankedpay_ledger WHERE member_id = @v0  AND type = 'A' ) hours", new object[] {  member_id } );

			
				 * */
					#endregion
					//if(_counter > 0)
					//	{
					// Check if there are any all outstanding requests
					var this_dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM vacation_master WHERE member_id = @v0  AND payperiod_id = @v1  AND payment_method = 3 AND status = 3 LIMIT 1", new object[] { _member_id, _payperiod_id });
					if (this_dt.Rows.Count > 0)
					{
						#region IF there are: 
						var subCount = 0;
						foreach (DataRow this_dr in this_dt.Rows)
						{
							var vacation_id = this_dr["vacation_id"].ToString();
							var holidayTotal = employee.business_unit.country == "CDN"
													? employee.Holiday / employee.wage
													: employee.Holiday;
							// This will apply the current all outstanding to one of their requests if this was a multi-day vacation request... needs to be hours
							Toolbox.doSQL_void(conn, @"UPDATE vacation_master SET status = 5, payment_amount = @v1 WHERE vacation_id = @v0", new object[] { vacation_id, holidayTotal });
							if (subCount == 0)
							{
								Toolbox.doSQL_void(conn, @"UPDATE payroll_hours SET vac = @v2 WHERE member_id = @v0 AND payperiod_id = @v1 ", new object[] { _member_id, _payperiod_id, holidayTotal });
								//employee.Holiday = 0;
								//employee.save();
							}
							subCount++;
						}
						#endregion
					}
					else
					{
						#region IF there aren't
						double vacation_total = 0;
						this_dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM vacation_master WHERE member_id = @v0  AND payperiod_id = @v1  AND payment_method = 2 AND status = 3", new object[] { _member_id, _payperiod_id });
						if (this_dt.Rows.Count > 0)
						{
							foreach (DataRow this_dr in this_dt.Rows)
							{
								var vacation_id = this_dr["vacation_id"].ToString();
								var payment_amount = Convert.ToDouble(this_dr["payment_amount"]);
								vacation_total += payment_amount;
							}
							Toolbox.doSQL_void(conn, @"UPDATE payroll_hours SET vac = @v2 WHERE member_id = @v0 AND payperiod_id = @v1 ", new object[] { _member_id, _payperiod_id, vacation_total });
							//employee.Holiday = 0;
							//employee.save();
							Toolbox.doSQL_void(conn, @"UPDATE vacation_master SET status = 5 WHERE member_id = @v0 AND payperiod_id = @v1 AND status = 3", new object[] { _member_id, _payperiod_id });
						}
						#endregion
					}
					//	} // End if(counter)
					//else
					//	{
					//	return;
					//	}
				}
			}
		}
		public class expense
		{
			public double amount { get; set; }
            public double pre_tax_amount { get; set; }
            public int approved { get; set; }
			public int customer_id { get; set; }
			public string customer_name { get; set; }
			public DateTime date_purchased { get; set; }
			public DateTime date_requested { get; set; }
			public DateTime date_start { get; set; }
			public DateTime date_end { get; set; }
			public int id_expense { get; set; }
			public int id_member { get; set; }
			public int id_payperiod { get; set; }
			public int id_seller { get; set; }
			public string name_seller { get; set; }
			public string item_text { get; set; }
			public string category { get; set; }
			public string receipt_number { get; set; }
			public int woprog_id { get; set; }
			public int master_id { get; set; }
			public int approved_by { get; set; }
			public bool has_file { get; set; }
			public double distance { get; set; }
			public string unit_distance { get; set; }
			public string attendees { get; set; }
			public string file_ext { get; set; }
			public string file_mime { get; set; }
			public string currency { get; set; }
		    public int? expense_category_id { get; set; }
		    public expense() { }
			public expense(int _id)
			{
				id_expense = _id;
				if (exists(_id))
				{
					load();
				}
				else
				{
					throw new Exception("Expense ID doesn't exists");
				}
			}
			public static double this_payperiod(MySqlConnection _conn, int _member_id, int _payperiod_id)
			{
				return Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(amount), 0) FROM expense_reimbursement e inner join member b on b.member_id = e.id_member WHERE e.id_member = @v0  AND e.id_payperiod = @v1  AND e.approved = 1 and e.id_seller<>309", new object[] { _member_id, _payperiod_id });
			}
            public static double this_per_diems(MySqlConnection _conn, int _member_id, int _payperiod_id)
            {
                return Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(amount), 0) FROM expense_reimbursement e inner join member b on b.member_id = e.id_member WHERE e.id_member = @v0  AND e.id_payperiod = @v1  AND e.approved = 1 and e.id_seller=309", new object[] { _member_id, _payperiod_id });
            }
            // SD:  On call is being added as an expense reimbursement - category 51 - Per Diem Other which ends up being id_seller 492
            // If this way of entering on call is set in stone, then the following logic (this on call and this expense) should capture on call and expenses separately
			// 
			public static double this_on_call(MySqlConnection _conn, int _member_id, int _payperiod_id)
			{
				  return Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(amount), 0) FROM expense_reimbursement e inner join member b on b.member_id = e.id_member WHERE e.id_member = @v0  AND e.id_payperiod = @v1  AND e.approved = 1 and e.id_seller= @v2", new object[] { _member_id, _payperiod_id, ExpenseReimbursement.Seller.OnCall });
			}
			public static double this_expense(MySqlConnection _conn, int _member_id, int _payperiod_id)
			{
				return Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(amount), 0) FROM expense_reimbursement e inner join member b on b.member_id = e.id_member WHERE e.id_member = @v0  AND e.id_payperiod = @v1  AND e.approved = 1 and e.id_seller NOT IN  (@v2 , @v3)", new object[] { _member_id, _payperiod_id ,ExpenseReimbursement.Seller.OnCall ,ExpenseReimbursement.Seller.PerDiem});
			}
            public static int new_seller(string _name, int _member_id)
			{
				return Toolbox.doSQL_return_id(@"INSERT INTO expense_seller (name_seller, added_by, added_dt) VALUES (@v0, @v1, NOW())",
					new object[] { _name, _member_id });
			}
			private void load()
			{
				var e_row = Toolbox.doSQL_dt(@" SELECT a.id_member, IFNULL(a.approved, 0) approved, a.receipt_number, a.date_requested, a.date_purchased, a.date_start, a.date_end, a.id_payperiod, a.id_seller, (SELECT b.name_seller FROM expense_seller b WHERE b.id_seller = a.id_seller) name_seller, URLDECODE(b.description) category, a.customer_id, c.customer_name, a.woprog_id, a.item_text, a.master_id, a.amount,a.pre_tax_amount, a.has_file, a.distance, a.unit_distance, IFNULL(a.attendees, '') attendees, IFNULL(a.file_ext, '') file_ext, IFNULL(a.file_mime, '') file_mime, IFNULL(a.currency, '') currency, a.approved_by, a.expense_category_id FROM expense_reimbursement a LEFT JOIN inventory_description b ON a.master_id = b.master_id LEFT JOIN customer c ON a.customer_id = c.customer_id WHERE  id_expense = @v0  LIMIT 1", new object[] { id_expense }).Rows[0];
				//id_member = (int) e_row["id_member"];
				id_member = int.Parse(e_row["id_member"].ToString());
				if (e_row["approved"] != DBNull.Value)
				{
					approved = Convert.ToInt32(e_row["approved"]);
				}
				receipt_number = e_row["receipt_number"].ToString();
				date_requested = Convert.ToDateTime(e_row["date_requested"]);
				date_purchased = Convert.ToDateTime(e_row["date_purchased"]);
				date_start = e_row["date_start"] == DBNull.Value ? date_purchased : Convert.ToDateTime(e_row["date_start"]);
				date_end = e_row["date_end"] == DBNull.Value ? date_purchased : Convert.ToDateTime(e_row["date_end"]);
				id_payperiod = Convert.ToInt32(e_row["id_payperiod"]);
				id_seller = Convert.ToInt32(e_row["id_seller"]);
				name_seller = e_row["name_seller"].ToString();
				customer_id = e_row["customer_id"] == DBNull.Value ? 0: Convert.ToInt32(e_row["customer_id"]);
				customer_name = e_row["customer_name"].ToString();
				woprog_id = e_row["woprog_id"]==DBNull.Value ? 0: Convert.ToInt32(e_row["woprog_id"]);
				master_id = e_row["master_id"]==DBNull.Value ? 0: Convert.ToInt32(e_row["master_id"]);
				category = e_row["category"].ToString();
				item_text = e_row["item_text"].ToString();
				amount = Convert.ToDouble(e_row["amount"]);
				has_file = Convert.ToBoolean(e_row["has_file"]);
				distance = Convert.ToDouble(e_row["distance"]);
				unit_distance = e_row["unit_distance"].ToString();
				attendees = e_row["attendees"].ToString();
				file_ext = e_row["file_ext"].ToString();
				file_mime = e_row["file_mime"].ToString();
				currency = e_row["currency"].ToString();
				approved_by = Convert.ToInt32(e_row["approved_by"]);
				pre_tax_amount = Convert.ToDouble(e_row["pre_tax_amount"]);
			    expense_category_id = e_row["expense_category_id"] != DBNull.Value ?  Convert.ToInt32(e_row["expense_category_id"]) : (int?) null;
            }
			public bool exists(int _id)
			{
				return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM expense_reimbursement WHERE id_expense = @v0", _id) > 0;
			}
			public void delete()
			{
				// Delete from WO
				// Send email
				if (exists(id_expense) && approved == -1)
				{
					Toolbox.doSQL_void(@"DELETE FROM expense_reimbursement WHERE id_expense = @v0 LIMIT 1", id_expense);
				}
			}
			public void new_perdiem()
			{

			}
			public void save()
			{
				var is_new = id_expense == 0;
				var sql = "";
				if (!is_new)
				{
					
					var oldValues = new expense(id_expense);
					var retainTS = "";
					// Is this edit made in the same accounting period as the one this expense was created in ?
					// If month on requested date is in the past/ does not match current month then retain the timestamp
					var editedinSameAcctPeriod = oldValues.date_requested.Year == DateTime.Today.Year
											? oldValues.date_requested.Month == DateTime.Today.Month
											: false;

					if(oldValues.approved != approved && !editedinSameAcctPeriod)
					{
						//Adding this to prevent re-integration after the accounting period is closed
						retainTS = "update_ts = update_ts,";
					}
					#region edit

					sql = @"
UPDATE 
	expense_reimbursement 
SET 
	id_member = @v0, 
	approved = @v1,
	receipt_number = @v2,
	date_purchased = @v3,
	date_start = @v12,
	date_end = @v13,
	id_payperiod = @v4,
	id_seller = @v5,
	customer_id = @v6,
	woprog_id = @v7,
	item_text = @v9,
	amount = @v10,
	master_id = @v11,
	has_file = @v14,
	distance = @v15,
	unit_distance = @v16,
	attendees = @v17,
	file_ext = @v18,
	file_mime = @v19,
	currency = @v20,
	approved_by = @v21,
	"+retainTS+@"
    expense_category_id = @v22,
    pre_tax_amount = @v23
WHERE
	id_expense = @v8
LIMIT 1
	";
					var paramObjects = new object[]
					{
						id_member,									// {0}
						approved,									// {1}
						receipt_number,							// {2}
						Toolbox.MySQL_shortdt(date_purchased),		// {3}
						id_payperiod,								// {4}
						id_seller,									// {5}
						customer_id,								// {6}
						woprog_id,									// {7}
						id_expense,								// {8}
						item_text,				// {9}
						amount,									// {10}
						master_id,									// {11}
						Toolbox.MySQL_shortdt(date_start),			// {12}
						Toolbox.MySQL_shortdt(date_end),			// {13}	
						has_file,									// {14}
						distance,									// {15}
						unit_distance,								// {16}
						attendees,				// {17}
						file_ext,									// {18}
						file_mime,									// {19}
						currency,									// {20}
						approved_by,                                // {21}
					    expense_category_id ,                        // {22}
                        pre_tax_amount
                    };
					Toolbox.doSQL_void(sql, paramObjects);
					#endregion
				}
				else
				{
					#region insert

					sql = @"
INSERT INTO expense_reimbursement
	(
	id_member, 
	approved,
	receipt_number,
	date_requested,
	date_purchased,
	id_payperiod,
	id_seller,
	customer_id,
	woprog_id,
	item_text,
	amount,
	master_id,
	date_start,
	date_end,
	has_file,
	distance,
	unit_distance,
	attendees,
	file_ext,
	file_mime,
	currency,
	approved_by,
    expense_category_id,
    pre_tax_amount
	)
VALUES
	(
	@v0, 
	-1,
	@v1,
	NOW(),
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
	@v12,
	@v13,
	@v14,
	@v15,
	@v16,
	@v17,
	@v18,
	@v19,
    @v20,
    @v21
	)";
					var paramObjects = new object[]
					{
						id_member,									// {0}
						receipt_number,							// {1}
						Toolbox.MySQL_shortdt(date_purchased),		// {2}
						id_payperiod,								// {3}
						id_seller,									// {4}
						customer_id,								// {5}
						woprog_id,									// {6}
						item_text,				// {7}
						amount,									// {8}
						master_id,									// {9}
						Toolbox.MySQL_shortdt(date_start),			// {10}
						Toolbox.MySQL_shortdt(date_end),			// {11}
						has_file,									// {12}
						distance,									// {13}
						unit_distance,								// {14}
						attendees,				// {15}
						file_ext,									// {16}
						file_mime,									// {17}
						currency,									// {18}
						approved_by,                                 // {19}
					    expense_category_id  ,                       // {22}
                        pre_tax_amount
                    };
					id_expense = Toolbox.doSQL_return_id(sql, paramObjects);
					#endregion
				}
				load();
			}
			public Toolbox.boolstr review(MySqlConnection _conn, bool _approve, NeMember _manager, bool _force)
			{
				var bs = new Toolbox.boolstr();
				var exp_perd = id_seller == 309 ? "per diem" : "expense";
				var to_status = _approve ? "approved" : "denied";
				var pre_status = approved;
				var employee = new NeMember(id_member);
				var employeePayrollApproved = Toolbox.doSQL_int(_conn, @"SELECT COUNT(*) FROM payroll_hours WHERE member_id = @v0 AND payperiod_id = @v1", new object[] { id_member, id_payperiod });
				if (employeePayrollApproved > 0)
				{
					throw new Exception("Payroll has already been approved for this employee, expenses/per diems cannot be changed. If you need to change this request, this employee's payroll needs to be first moved back.");
				}
				
				var is_shop = woprog_id == 0;
				var _wo = new NeWOProg();
				if (!is_shop)
				{
					_wo = new NeWOProg(woprog_id);
				}
				if (approved == -1 || _force) // If the expense has been already dealt with or needs to be denied by force
				{
					approved = _approve ? 1 : 0;
					approved_by = _manager.id;
					save();
					bs.success = true;
					if (!_approve && !is_shop)
					{
						if (_wo.Status == "Waiting to be Invoiced" || _wo.Status == "Invoiced")
						{
							bs.message = string.Format("The {0} has been {1}, though the work order attached to the {0} cannot be edited, it is in the status: {2}", exp_perd, to_status, _wo.Status);
						}
						else
						{
							var wo_lineid = Toolbox.doSQL_int(_conn, @"SELECT IFNULL(MAX(id), 0) FROM wo_detail WHERE memberid = @v0 AND consignment_id = @v1 ",
								new object[] { id_member, id_expense });
							if (wo_lineid == 0)
							{
                                //
                                // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/2012/
                                // 
                                // Why we need to remove this part because it will give us a misunderstanding on the code.
                                //
                                // (1) Think it from angular delete/deny page.
                                // Run case 1: if c is greater than one or equal to ZERO, wo_lineid will be zero. and code wii run into
                                //             'bs.message = string.Format("This {0} has been denied.", exp_perd);'
                                // Run case 2: if c is one, a new wo_lineid will be picked up, and detach function will be called, sounds wrong thing will be happening.
                                //             but go inside deatch function, you will see 'var wo_lineid = Toolbox.doSQL_int(_conn, @"SELECT IFNULL(MAX(id), 0) FROM wo_detail WHERE memberid = @v0 AND consignment_id = @v1", new object[] { _exp.id_member, _exp.id_expense });'
                                //             and the key part is _exp.id_expense, which will block the wrong update.
                                //      
                                // (2) Why this happens, if we look into this bug https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1456 and its changes,
                                //  you will find we changed this behavior by deleting the wo_detail_current record first, then call this function to update the 'expense_reimbursement' table.
                                //
                                // (3) For deny from email passport function, this function will take effect by deleting the wo_detail_current at the last step.
                                //


                                /*
								var c = Toolbox.doSQL_int(_conn, @"SELECT COUNT(id) FROM wo_detail WHERE memberid = @v0 AND 
origin = 'Expense Reimbursement' AND woprog_id = @v1 ", new object[] { id_member, woprog_id });
								if (c == 1)
								{
									wo_lineid = Toolbox.doSQL_int(_conn, @"SELECT id FROM wo_detail WHERE memberid = @v0 AND origin = 'Expense Reimbursement' AND 
woprog_id = @v1 ", new object[] { id_member, woprog_id });
								}
								else if (c > 1)
								{
									shared.alert_debug("Expense reimbursement / Per Diem failure", string.Format("{0} tried denying an expense for {1} (ID: {2}), it is linked to WO ID: {3}, but there were multiple lines matching the memberid, origin & woprog_id???? Shouldn't happen.", _manager.FullName, employee.FullName, id_expense, woprog_id));
								}
                                */
                            }
                            if (wo_lineid != 0)
							{
								try
								{
									var wo_line = new NeWODetailCurrent(wo_lineid);
									bs = detach(_conn, _wo, this, ref wo_line, _manager);
									bs.message = string.Format("This {0} has been denied, work order line has been deleted.", exp_perd);
								}
								catch (Exception ee)
								{
									shared.alert_debug("Expense reimbursement / Per Diem failure", string.Format("{0} tried denying an expense for {1} (ID: {2}), it is linked to WO ID: {3}, but this error occured:<br/>{4}", _manager.FullName, employee.FullName, id_expense, woprog_id, ee));
									bs.message = string.Format("This {0} has been denied, but there was a problem deleting the line from the work order - IT has been emailed with the details.", exp_perd);
								}
							}
							else
							{
								bs.message = string.Format("This {0} has been denied.", exp_perd);
							}
						}
					}
					else
					{
						if (approved == 0 && _approve)
						{
							if(!is_shop && id_seller != 309 && has_file)
								{
								AddToProjectFolder(_wo, this, employee);
								}
							bs.message = !is_shop
								? string.Format("This {0} has been approved, though as it was denied beforehand, please talk to finance", exp_perd)
								: string.Format("This {0} has been approved", exp_perd);
						}
						else
						{
							if(!is_shop && id_seller != 309 && has_file)
								{
								AddToProjectFolder(_wo, this, employee);
								}
							bs.message = string.Format("This {0} has been {1}", exp_perd, to_status);
						}
					}
				}
				else
				{
					bs.success = false;
					bs.message = string.Format("Sorry, this {0} has already been reviewed, and was {1}.", exp_perd, approved == 1 ? "Approved" : "Denied");
				}

				var attendees_appendage = master_id != 73105 ? "" : string.Format(@"
	<tr>
		<td><b>Attendees:</b></td>
		<td>{0}</td>
	</tr>", attendees);
				var woshop = is_shop ? "Shop" : "WO";
				var mileage_appendage = Toolbox.doSQL_int(_conn, @"SELECT count(id) from gl_te where tax_entity_id = @v0 and is_mileage=1 and account_no =@v1", new object[] { employee.business_unit.tax_entity_id, master_id }) > 0 ? string.Format(@"
    < tr>
		<td><b>Distance Traveled:</b></td>
		<td>{0} {1}</td>
	</tr>", distance, unit_distance) : "";
				var wo_appendage = is_shop ? "" : string.Format(@"
	<tr>
		<td><b>WO#:</b></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td><b>Customer:</b></td>
		<td>{1}</td>
	</tr>", _wo.OrderNumber, _wo.CustomerName);
				var category = Toolbox.doSQL_string(_conn, @"SELECT IFNULL(MAX(a.account_no), 'NOT SET') 
FROM gl_te a left join gl_group_te b on a.gl_group_id = b.id where a.account_no = @v0 AND b.type = 'X'", new object[] { master_id });
				var body = string.Format(@"
<b><u>Expense Information</u><b><br/>
<table cellspacing='0' cellpadding='2'>
	<tr>
		<td><b>Employee:</b></td>
		<td>{0}</td>
	</tr>
        <tr>
             <td><b>Business Unit Name:</b></td>
            <td>{12}</td>
        </tr>
	<tr>
		<td><b></b></td>
		<td></td>
	</tr>
	<tr>
		<td><b>Type of Purchase:</b></td>
		<td>{2}</td>
	</tr>
{9}
{10}
	<tr>
		<td><b>WO or Shop?:</b></td>
		<td>{7}</td>
	</tr>
{8}
	<tr>
		<td><b>Purchased From:</b></td>
		<td>{3}</td>
	</tr>
    <tr>
		<td><b>Pre-Tax Amount:</b></td>
		<td>{4:c2}</td>
	</tr>
	<tr>
		<td><b>Amount:</b></td>
		<td>{4:c2}</td>
	</tr>
	<tr>
		<td><b>Purchased Date:</b></td>
		<td>{5}</td>
	</tr>
	<tr>
		<td><b>Description:</b></td>
		<td>{6}</td>
	</tr>
</table>
<br/>
<br/>
",
					employee.FullName,                          // {0}
					0,                              // {1}
					category,                           // {2}
					HttpContext.Current.Server.HtmlEncode(name_seller), // {3}
                    pre_tax_amount,                                    // {4}
					amount,                                 // {5}
					date_purchased.ToShortDateString(),     // {6}
					HttpContext.Current.Server.HtmlEncode(item_text), // {7}  
					woshop,                                     // {8}
					wo_appendage,                               // {9}
					attendees_appendage,                         // {10}
                    mileage_appendage,                           // {11}
                    employee.business_unit.name                     //{12}
                );
				if (_approve)
				{
					shared.alert_payroll(string.Format("Notification that {0} has {1} a/an {2} for {3}", _manager.FullName, bs.success ? "successfully " + to_status : " unsuccessfully " + to_status, exp_perd, employee.FullName),
						bs.success
							? string.Format("Expense Description: {3}<br/><br/>The request was successfully {0}.<br/> The message {1} received was: <b>{2}</b>", to_status, _manager.Nickname, bs.message, body)
							: string.Format("Expense Description: {3}<br/><br/>The request was not successfully {0}, and ran into errors... IT has been notified. <br/>The message {1} received was: <b>{2}</b>", to_status, _manager.Nickname, bs.message, body),
						employee, this);
				}
				return bs;
			}
			public static bool AddToProjectFolder(NeWOProg WorkOrderObj, expense exp, NeMember employee)
				{
				var businessUnit = new NeBusinessUnit(employee.business_unit_id);
				var teFolder = NeTaxEntity.BaseFolder(businessUnit.id, false);
				var folderPath = $@"{teFolder}\expense_receipts\{exp.id_expense}.{exp.file_ext}";
				return AddToProjectFolder(folderPath, WorkOrderObj, exp, employee);
				}
			public static bool AddToProjectFolder(string folderPath, NeWOProg WorkOrderObj, expense exp, NeMember employee)
				{
				if(!File.Exists(folderPath)) return false;
				if(!exp.has_file || exp.woprog_id == 0 || exp.master_id == 309) return false;
				var NeF = new NeFiles();
				var projectFolder = NeF.GetProjectFolder(exp.woprog_id); 
				NeFiles.chk_wo_folders(projectFolder);
				var ConvertedFileName = ProjectFileName(exp, employee, exp.file_ext);
				var ConvertedProjectPath = $@"{projectFolder}\Expense Receipts\{ConvertedFileName}";
				
				if(File.Exists(ConvertedProjectPath)) return false;
				File.Copy(folderPath, ConvertedProjectPath);
				return true;
				}
			public static string ProjectFileName(expense exp, NeMember employee, string fileType)
				{
				if(!fileType.StartsWith("."))
					{
					fileType = $".{fileType}";
					}
				var name = $"[{Toolbox.MySQL_shortdt(exp.date_purchased)}] {exp.id_expense} - {NeFiles.clean_filename(employee.FullName)} - {NeFiles.clean_filename(exp.name_seller)} - ({exp.amount.ToString("C2")}){fileType}";
				if(name.Length > 260)
					{
					name = $"[{Toolbox.MySQL_shortdt(exp.date_purchased)}] {exp.id_expense} - {employee.id} - {NeFiles.clean_filename(shared.TruncateLongString(exp.name_seller, 35))} - ({exp.amount.ToString("C2")}){fileType}";
					}
				return name;
				}
			public static bool workorder_available(expense _exp, NeWOProg _wo)
			{
				// A little explanation as this might change later down the line:
				// Expense: Is only available when the work order is open.
				// Per Diem: Is only available when the work order is not in an "advanced" status
				var is_expense = _exp.id_seller != 309;
				var is_perdiem = _exp.id_seller == 309;
				var is_open = _wo.Status == OpsWOStatus.Open;
				var bad_statuses = new List<string> { "Deleted", OpsWOStatus.WaitingToBeInvoiced, "Invoiced", "Waiting for PO", "Waiting PM Approval", "Waiting BM Approval" };

				return is_expense && is_open ||
						  is_perdiem && !bad_statuses.Contains(_wo.Status);
			}
	
			public static Toolbox.boolstr detach(MySqlConnection _conn, NeWOProg _wo, expense _exp, ref NeWODetailCurrent _wo_line, NeMember _manager)
			{
				var bs = new Toolbox.boolstr();
				var exp_perd = _exp.id_seller == 309 ? "per diem" : "expense";
				var origin = _exp.id_seller == 309 ?OpsWOLineOrigin.PerDiemExpense :OpsWOLineOrigin.ExpenseReimbursement;
				if (!workorder_available(_exp, _wo)) // This really should be done before even getting here...
				{
					bs.message = "Work order is no longer open to expenses.";
					bs.success = false;
					return bs;
				}
				var wo_lineid = _exp.woprog_id == 0 
												? 0
												: Toolbox.doSQL_int(_conn, @"
												SELECT 
													IFNULL(MAX(id), 0)
												FROM
													wo_detail
												WHERE 
													woprog_id = @v3 AND
													memberid = @v0 AND 
													consignment_id = @v1 AND
													origin = @v2 ", 
												new object[] { _exp.id_member, _exp.id_expense, origin, _exp.woprog_id });
				if (wo_lineid == 0)
				{
					bs.message = "Line doesn't exist on work order to be removed";
					bs.success = true;
					return bs;
				}
				// Need to first uncommit the quantities from the line, before deleting.
				var wo_line = new NeWODetailCurrent(wo_lineid)
				{
					qty_ordered = -1,
					qty_committed = -1,
					qty_invoiced = -1
				};
				wo_line.notes += string.Format("\n Request was removed by {0} on {1}", _manager.FullName, Toolbox.MySQLNow_long());
				wo_line.save(_manager, "Expense reimbursement - Detach", false);
				NeWODetailCurrent.delete_workorder_line(wo_lineid, wo_line.master_id, _manager);
				NeWOProg.update_header_totals(_wo.woprog_id.ToString(), _wo.business_unit_id, _wo.OrderNumber);
				bs.message = string.Format("This {0} has been removed, work order line has been deleted.", exp_perd);
				bs.success = true;
				return bs;
			}
			public static Toolbox.boolstr attach(MySqlConnection _conn, NeWOProg _wo, expense _exp, ref NeWODetailCurrent _wo_line, NeMember _current_user)
			{
				var bs = new Toolbox.boolstr();
				if (!workorder_available(_exp, _wo)) // This really should be done before even getting here...
				{
					bs.message = "Work order is no longer open to expenses.";
					bs.success = false;
					return bs;
				}
				//var adjusted_amount = _exp.amount * NECurrency.get_exchange_rate_at_date(_current_user.id, _exp.date_purchased, _exp.currency);
				_wo_line = new NeWODetailCurrent
				{
					added_by = _exp.id_member,
					date_added = Toolbox.MySQLNow_long(),
					date_modified = Toolbox.MySQLNow_long(),
					description = _exp.item_text,
					master_id = _exp.id_seller == 309 ? OpsSpecialPart.PerDiem : OpsSpecialPart.ExpenseReimbursement, // following the logic about 'origin' below
					qty_ordered = _exp.amount < 0 ? -1 : 1,
					qty_committed = _exp.amount < 0 ? -1 : 1,
					qty_invoiced = _exp.amount < 0 ? -1 : 1,
					cost = Math.Abs(_exp.amount),
					billtypeid = _wo.QuoteID == "0" ? 0 : 1,
					tax1 = _wo.woprog_tax1,
					tax2 = _wo.woprog_tax2,
					tax3 = _wo.woprog_tax3,
					tax4 = _wo.woprog_tax4,
					woprog_id = _wo.woprog_id,
					bvwo = Convert.ToInt32(_wo.OrderNumber),
					business_unit_id = _wo.business_unit_id,
					type = "M",
					code = _exp.master_id.ToString(),
					origin = _exp.id_seller == 309 ? "Per Diem Expense" : "Expense Reimbursement",
					issues = "",
					memberid = _exp.id_member,
					paytypeid = 0
				};
				_wo_line.sell = _wo.use_fixed_material_markup ? _wo.fixed_material_markup * Convert.ToDouble(_wo_line.cost) : shared.GetSellPrice(Convert.ToDouble(_wo_line.cost), 0, false, 1, _wo.business_unit_id);
				_wo_line.unit = _wo_line.sell;
				try
				{
					_wo_line.save(_current_user, "Expense reimbursement - Attach", false);
					bs.success = true;
				}
				catch (Exception ee)
				{
					bs.success = false;
					bs.message = "Failed - " + ee;
				}
				return bs;
			}
		}
		public class banked_pay
		{
			public int member_id;
			public int admin_id;
			public int payperiod_id;
			public string date_start;
			public string date_end;
			public string separate_check = "0";

			public bool has_history()
			{
				return Toolbox.doSQL_int(@"SELECT count(*) FROM bankedpay_ledger WHERE member_id =@v0", member_id) > 0;
			}

			public double available_hours()
			{
				using (var conn = Toolbox.connect())
				{
					var payroll_hours = this_payroll_hours(conn);
					var deposited_hours = get_hours(conn, OpsPayroll.TransactionType.BankedPay.Deposited, payperiod_id);
					var withdrawn_hours = get_hours(conn, OpsPayroll.TransactionType.BankedPay.Withdrawn, payperiod_id);
					var available_hours = payroll_hours - (deposited_hours - withdrawn_hours);
					if (available_hours < 0)
					{
						available_hours = 0;
					}
					return available_hours;
				}
			}
			public double this_payroll_hours(MySqlConnection _conn)
			{
				return Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(numberofhours), 0) TOTAL FROM membertime WHERE membertime_memberid = @v0  AND date >= @v1  AND date <= @v2  AND membertime_paytypehours_id = 1", new object[] { member_id, date_start, date_end });
			}
			public double balance()
			{
				using (var conn = Toolbox.connect())
				{
					if (payperiod_id == 0)
					{
						payperiod_id = payroll.current_pay_period();
					}
					var bankedpay = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(hours * payrate), 0) AS banked FROM bankedpay_ledger  WHERE type='D' AND member_id = @v0", new object[] { member_id });
					var withdrawn = Math.Round(get_dollars(conn, OpsPayroll.TransactionType.BankedPay.Withdrawn, payperiod_id + 1, true), 2, MidpointRounding.AwayFromZero);
					var deducted = Math.Round(get_dollars(conn,  OpsPayroll.TransactionType.BankedPay.Deducted, payperiod_id + 1, true), 2, MidpointRounding.AwayFromZero);
					var paidout = Math.Round(get_dollars(conn,   OpsPayroll.TransactionType.BankedPay.PaidOut, payperiod_id + 1, true), 2, MidpointRounding.AwayFromZero);
					var balance = bankedpay - withdrawn - deducted - paidout;
					return balance;
				}
			}
			/// <summary>
			/// Gets the sum of dollars from a bank transaction
			/// </summary>
			/// <param name="_conn"></param>
			/// <param name="_type">
			///	<para>Can be P = Payout, D = Deposit, E = Deducted, W = Withdrawal</para>
			/// </param>
			/// <param name="_payperiod_id"></param>
			/// <param name="_less_than_payperiod"></param>
			/// <returns></returns>
			public double get_dollars(MySqlConnection _conn, string _type, int _payperiod_id, bool _less_than_payperiod = false)
			{
				var less_than = _less_than_payperiod ? '<' : '=';
				return Toolbox.doSQL_double(_conn, string.Format("SELECT IFNULL(SUM(hours * payrate),0) AS banked FROM bankedpay_ledger WHERE type='{2}' AND member_id = {0} AND payperiod_id {3} {1}", member_id, _payperiod_id, _type, less_than), null);
			}
			/// <summary>
			/// Gets the sum of hours from a bank transaction
			/// </summary>
			/// <param name="_conn"></param>
			/// <param name="_type">
			///	<para>Can be P = Payout, D = Deposit, E = Deducted, W = Withdrawal</para>
			/// </param>
			/// <param name="_payperiod_id"></param>
			/// <param name="_less_than_payperiod"></param>
			/// <returns></returns>
			public double get_hours(MySqlConnection _conn, string _type, int _payperiod_id, bool _less_than_payperiod = false)
			{
				var less_than = _less_than_payperiod ? '<' : '=';
				return Toolbox.doSQL_double(_conn, string.Format("SELECT IFNULL(SUM(hours),0) AS banked FROM bankedpay_ledger WHERE type='{2}' AND member_id = {0} AND payperiod_id {3} {1}", member_id, _payperiod_id, _type, less_than), null);
			}
			public double withdrawable_hours()
			{
				using (var conn = Toolbox.connect())
				{
					var bankable_money = get_dollars(conn,	  OpsPayroll.TransactionType.BankedPay.Deposited, payperiod_id, true) -
											get_dollars(conn, OpsPayroll.TransactionType.BankedPay.Withdrawn, payperiod_id, true) -
											get_dollars(conn, OpsPayroll.TransactionType.BankedPay.PaidOut, payperiod_id, true) -
											get_dollars(conn, OpsPayroll.TransactionType.BankedPay.Deducted, payperiod_id, true);
					double balance_hours = 0;
					if (bankable_money > 0)
					{
						balance_hours = Math.Round(Math.Round(bankable_money, 2, MidpointRounding.AwayFromZero) / user_pay_rate(), 2, MidpointRounding.AwayFromZero);
					}
					return balance_hours;
				}
			}
			public void bank_hours(double _hours)
			{
				if (_hours > available_hours())
				{
					throw new Exception("You cannot deposit more hours than you have worked");
				}
				Toolbox.doSQL_void(@"INSERT INTO bankedpay_ledger (date, type, member_id, added_by, hours, payrate, payperiod_id, separate_check)
VALUES (now(), 'D', @v0, @v1, @v2, @v3, @v4, @v5)", new object[] { member_id, admin_id, _hours, user_pay_rate(), payperiod_id, separate_check });
			}
			public void withdraw_hours(double _hours)
			{
				if (_hours > withdrawable_hours())
				{
					throw new Exception("You cannot withdraw more hours than you have deposited");
				}
				Toolbox.doSQL_void(@"INSERT INTO bankedpay_ledger (date, type, member_id, added_by, hours, payrate, payperiod_id) 
VALUES (now(), 'W', @v0, @v1, @v2, @v3, @v4)", new object[] { member_id, admin_id, _hours, user_pay_rate(), payperiod_id });
			}
			public void retract_hours(int _id)
			{
				Toolbox.doSQL_void(@"UPDATE bankedpay_ledger SET type = 'R' WHERE id =@v0 ", _id);
			}
			public int current_pay_period()
			{
				var thisid = 0;
				var payperiod_info = Toolbox.doSQL_dt(@"SELECT PayperiodID, DATE_FORMAT(StartDate, '%Y-%m-%d') StartDate, DATE_FORMAT(EndDate, '%Y-%m-%d') EndDate FROM payperiods  WHERE StartDate < now() AND EndDate > now()", null);
				foreach (DataRow info in payperiod_info.Rows)
				{
					thisid = Convert.ToInt32(info["PayperiodID"].ToString());
					date_start = info["StartDate"].ToString();
					date_end = info["EndDate"].ToString();
				}
				return thisid;
			}
			public double user_pay_rate()
			{
				return Math.Round(Toolbox.doSQL_double(@"SELECT GET_WAGE(@v0 )", new object[] { member_id }), 2);
			}
			public string user_pay_type()
			{
				return Toolbox.doSQL_string(@"SELECT paytype FROM currentwage WHERE member_id =@v0", member_id);
			}
		}
	}
}