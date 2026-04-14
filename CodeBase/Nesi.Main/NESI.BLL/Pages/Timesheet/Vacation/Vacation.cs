using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;

// ReSharper disable MemberInitializerValueIgnored
#pragma warning disable 168

namespace NESI.BLL.Pages.Timesheet.Vacation
{
	public class Vacation : BLLBase
	{

		public string o = "";
		public string member_id = "";
		public string business_unit_id = "";
		public string member_name = "";
		public string vacation_id = "";
		public string payperiod_id = "";
		public string date_start = "";
		public string date_end = "";
		public string date_return = "";
		public string note = "";
		public string payment_method = "";
		public int paytype_id;
		public double hours;
		public DTO.Models.Users.Member employee;

		public bool all_outstanding = false;
		public bool unpaid = false;

		public Vacation(Employee user) : base(user)
		{
			employee = user.EmployeeProfile;
			member_id = UserId.ToString();
			business_unit_id = BusinessUnitId.ToString();
			member_name = employee.member_fullname;
			payperiod_id = CurrentPayPeriod();
			paytype_id = user.EmployeeProfile.paytype_id;

		}

		public double UserPayRate()
		{
			var sql = "SELECT FORMAT(wage,2) AS WAGE FROM currentwage WHERE member_id =" + member_id;
			return Math.Round(bllToolbox.doSQL_double(@"SELECT FORMAT(wage,2) AS WAGE FROM currentwage  WHERE member_id =@v0", member_id), 2,
				MidpointRounding.AwayFromZero);
		}
		public int Last_Anniversary()
		{
			return bllToolbox.doSQL_int(@"SELECT last_anniversary FROM member WHERE member_id =@v0", member_id);
		}
		public string CurrentPayPeriod()
		{
			var payperiodid = bllToolbox.doSQL_int("CALL _payperiod()");
			var pp = new Ne2PayPeriod(payperiodid);
			if (Convert.ToDateTime(pp.Enddate) < DateTime.Now)
			{
				pp.payperiodID = bllToolbox.doSQL_int("SELECT payperiodid FROM payperiods WHERE startdate <= NOW() AND enddate >= NOW()");
			}
			return pp.payperiodID.ToString();
		}

		public double GetCurrentPayPeriodRequestHours()
		{
			return bllToolbox.doSQL_double(@" SELECT ifnull(sum(amount),0) FROM vacation  
			WHERE status_id != 4 AND type_id = 4 AND status IS NOT NULL AND member_id =@v0 and payperiod_id=@v1 ORDER BY date_start DESC", member_id, payperiod_id);
		}

		public double GetCurrentPayPeriodRequestDollars()
		{
			return GetCurrentPayPeriodRequestHours() * UserPayRate();
		}

		public DataTable Payment_Methods()
		{
			const string sql = "SELECT * FROM vacation_payment_method";
			return bllToolbox.doSQL_dt(sql);
		}
		public DataTable View_Past()
		{
			return bllToolbox.doSQL_dt(@" SELECT vacation_id, hours_requested, status, payperiod_id,
			IF((date_insert IS NULL OR YEAR(date_insert) < 2000), '--', date_insert) date_insert,
			IF((date_start IS NULL OR YEAR(date_start) < 2000), '--', DATE_FORMAT(date_start, '%Y-%m-%d')) date_start,
			IF((date_end IS NULL OR YEAR(date_end) < 2000), '--', DATE_FORMAT(date_end, '%Y-%m-%d')) date_end, amount, payment_method FROM vacation  
			WHERE status_id != 4 AND type_id = 4 AND status IS NOT NULL AND member_id =@v0 ORDER BY date_start DESC", member_id);
		}
		public DataTable Past_Requests()
		{
			return bllToolbox.doSQL_dt(@"SELECT * FROM vacation WHERE member_id = @v0  AND type_id = 4 AND status not in ('pending', 'cancelled')", member_id);
		}
		public DataTable Pending_Requests()
		{
			return bllToolbox.doSQL_dt(@"SELECT * FROM VACATION WHERE member_id = @v0  AND type_id = 4 AND status ='PENDING'", member_id);
		}
		public DataTable Past_Notes(int vacationId)
		{
			return bllToolbox.doSQL_dt(@" SELECT CONCAT(b.member_firstname, ' ', b.member_lastname) name_full, DATE_FORMAT(a.date_insert, '%m/%d/%Y @ %I:%i%p') date, URLDECODE(a.note) note FROM vacation_note a LEFT JOIN member b ON a.member_id = b.member_id  WHERE a.vacation_id=@v0 ORDER BY a.date_insert", vacationId);
		}
		private bool Update_Anniversary()
		{
			var _last = bllToolbox.doSQL_int(@"SELECT ifnull(last_anniversary, 0) last FROM member WHERE member_id =@v0", member_id);
			_last = _last == 0 ? bllToolbox.doSQL_int("SELECT YEAR(member_startdate) FROM member WHERE member_id =@v0", member_id) : bllToolbox.doSQL_int("SELECT YEAR(NOW())");
			return bllToolbox.doSQL_bool(@"UPDATE member SET last_anniversary = @v0  WHERE member_id = @v1  LIMIT 1", _last, member_id);
		}

		public bool Add_Vacation()
		{
			double vacation_amount = 0;
			var months_with_company = CurrentUser.months_with_company;
			var vacation_interval_1 = employee.vacation_interval_1.GetValueOrDefault();
			var vacation_interval_2 = employee.vacation_interval_2.GetValueOrDefault();
			var vacation_interval_3 = employee.vacation_interval_3.GetValueOrDefault();
			var vacation_amount_1 = employee.vacation_amount_1.GetValueOrDefault();
			var vacation_amount_2 = employee.vacation_amount_2.GetValueOrDefault();
			var vacation_amount_3 = employee.vacation_amount_3.GetValueOrDefault();


			if (months_with_company >= vacation_interval_1 && months_with_company < vacation_interval_2)
			{
				vacation_amount = vacation_amount_1;
			}
			else if (months_with_company >= vacation_interval_2 && months_with_company < vacation_interval_3)
			{
				vacation_amount = vacation_amount_2;
			}
			else if (months_with_company >= vacation_interval_3)
			{
				vacation_amount = vacation_amount_3;
			}

			if (bllToolbox.doSQL_bool(@"INSERT INTO bankedpay_ledger (date,type, member_id, added_by, hours, payrate, note) 
			VALUES (now(), 'V', @v2 , @v2 , @v0 , @v1 , 'Allotment per anniversary of statutory vacation time')", vacation_amount, UserPayRate(), member_id))
			{
				Update_Anniversary();
			}
			else
			{
				return false;
			}
			return true;
		}
		public string Request_Note_Add(int vacationId, string anote)
		{

			try
			{
				bllToolbox.doSQL_void(@" INSERT INTO vacation_note (vacation_id,member_id,date_insert, note) VALUES ( @v0 , @v1 , now(), @v2  ) ", vacationId, member_id, anote);
			}
			catch
			{
				return "Adingd Note failed.";
			}
			return "Note has been added successfully.";

		}
		public bool All_Outstanding_Available()
		{
			if (payperiod_id == "")
			{
				payperiod_id = CurrentPayPeriod();
			}
			return bllToolbox.doSQL_int(@"SELECT IFNULL(COUNT(*),0) c 
FROM vacation_master WHERE payperiod_id = @v0 AND member_id = @v1 AND status NOT IN (4,2) AND (payment_method = 3 OR payment_amount = -1)", payperiod_id, member_id) == 0;
		}
		public bool Is_Anniversary()
		{
			var country = employee.member_country;
			if (country == "USA")
			{
				var startdate = DateTime.Parse(bllToolbox.doSQL_string(@"SELECT member_startdate FROM member WHERE member_id=@v0", member_id));
				var DoY_Start = bllToolbox.doSQL_int("SELECT DAYOFYEAR(member_startdate) FROM member WHERE member_id =@v0", member_id);
				var DoY_Now = bllToolbox.doSQL_int("SELECT DAYOFYEAR(now())");
				var initial_vac_inc = bllToolbox.doSQL_int("SELECT vacation_interval_1 FROM member WHERE member_id =@v0", member_id);
				var second_vac_inc = bllToolbox.doSQL_int("SELECT vacation_interval_2 FROM member WHERE member_id =@v0", member_id);
				var Year_Last_Anniversary = Last_Anniversary();
				var Days_Differential = bllToolbox.doSQL_int("SELECT DATEDIFF(NOW(),member_startdate) diff FROM member WHERE member_id=@v0", member_id);
				var Month_Start = startdate.Month;
				var Month_Now = DateTime.Now.Month;
				var Day_Start = startdate.Day;
				var Day_Now = DateTime.Now.Day;
				var Year_Start = startdate.Year;
				var Year_Now = DateTime.Now.Year;
				var Leapyear_Start = DateTime.IsLeapYear(Year_Start);
				var Leapyear_Now = DateTime.IsLeapYear(Year_Now);
				if (Year_Now == Year_Start && initial_vac_inc == 0 && Year_Last_Anniversary < Year_Now)
				{
					return true;
				}
				if (Year_Now == Year_Start && initial_vac_inc <= CurrentUser.months_with_company && Year_Last_Anniversary < Year_Now)
				{
					return true;
				}
				if (Year_Now == Year_Start && second_vac_inc <= CurrentUser.months_with_company && Year_Last_Anniversary == Year_Now)
				{
					return true;
				}
				if (Year_Now > Year_Start && initial_vac_inc == 0 && Year_Last_Anniversary < Year_Now && Days_Differential < 365)
				{
					return true;
				}
				if (Year_Now > Year_Start && Year_Last_Anniversary < Year_Now)
				{
					if (Leapyear_Start && Month_Start >= 3)
					{
						DoY_Start--;
					}

					if (Leapyear_Now && Month_Now >= 3)
					{
						DoY_Now--;
					}

					if (DoY_Start <= DoY_Now)
					{
						return true;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		public bool Request_Note_Add(string avacation_id, string anote)
		{
			if (anote.Length == 0)
			{
				return true;
			}
			try
			{
				bllToolbox.doSQL_bool(@" INSERT INTO vacation_note (vacation_id,member_id,date_insert, note) VALUES ( @v0 , @v1 , now(), @v2  ) ", avacation_id, member_id, anote);
			}
			catch
			{
				return false;
			}
			return true;
		}
		public bool Deduct_Vacation(string amount, string avacation_id, string type)
		{
			var this_payperiod_id = bllToolbox.doSQL_string(@"SELECT payperiod_id FROM vacation_master WHERE vacation_id =@v0 ", avacation_id);
			const string sql = @"
INSERT INTO bankedpay_ledger 
(
	date,
	type,
	member_id,
	added_by,
	ref_id,
	hours,
	payrate,
	payperiod_id,
	note
)
VALUES
(
	now(),
	@v6,
	@v2,
	@v2,
	@v3,
	@v0,
	@v4,
	@v5,
	CONCAT('User requested vacation time for vacation_req id ',@v1)
)";
			var paramObjects = new object[] { amount, avacation_id, member_id, avacation_id, UserPayRate(), this_payperiod_id, type };
			try
			{
				bllToolbox.doSQL_void(sql, paramObjects);
			}
			catch (Exception ee)
			{
				// Toolbox.do_catch_error(ee, 711);
				return (false);
			}
			return true;
		}
		public string Request_Cancel(int avacation_id)
		{
			try
			{
				bllToolbox.doSQL_void(@"UPDATE vacation_master SET status = 4 WHERE vacation_id = @v0", avacation_id);
				bllToolbox.doSQL_void(@"UPDATE bankedpay_ledger SET type = 'C' WHERE ref_id= @v0", avacation_id);
			}
			catch (Exception ee)
			{
				// Toolbox.do_catch_error(ee, 711);
				return "Canceling vacation failed.";
			}
			return "Vacation has been canceled successfully.";
		}
		public string save()
		{
			var _request = new VacationRequest();
			if (o == "1" && !unpaid && !all_outstanding)
			{
				payment_method = "2";
			}
			else if (unpaid)
			{
				payment_method = "1";
				hours = 0;
			}
			else if (all_outstanding)
			{
				payment_method = "3";
				hours = 0;
			}
			var vacations_toapprove = new ArrayList();
			double used_hours = 0;
		    var available_hours = 0.0;
            var value = bllToolbox.doSQL_doubleOrNull(@"CALL VACATION_AVAILABLE_ACCRUALATDATE(@v0 , @v1, 1 )", employee.Member_ID,
				(_request.date_start.Year < 2000 ? bllToolbox.MySQLNow_short() : bllToolbox.MySQL_shortdt(_request.date_start)));

		    if (value.HasValue)
		    {
		        available_hours = value.Value;
		    }

		    if (o == "2") // Vacation pay out
			{
				_request = new VacationRequest
				{
					hours_requested = 0,
					payment_amount = all_outstanding ? -1 : hours,
					payment_method_id = 2,
					payperiod_id = Convert.ToInt32(CurrentPayPeriod()),
					status_id = 3,
					create_member_id = employee.Member_ID,
					business_unit_id = Convert.ToInt32(employee.business_unit_id),
					member_id = employee.Member_ID,
					note = note
				};
				_request.save_record();
				used_hours = hours;
			}
			else
			{
				var requested_date_start = Convert.ToDateTime(date_start);
				var requested_date_end = Convert.ToDateTime(date_end);
				var requested_date_return = Convert.ToDateTime(date_return);

				var payperiod_info = bllToolbox.doSQL_dt(@"SELECT payperiodid, startdate, enddate FROM payperiods");
				for (var d = requested_date_start; d <= requested_date_end; d = d.AddDays(1))
				{
					if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
					{
						continue;
					}
					_request = new VacationRequest
					{
						date_start = d.AddHours(8),
						date_end = d.AddHours(17),
						date_return = requested_date_return.AddHours(8),
						hours_requested = 0,
						payment_amount = 8,
						create_member_id = employee.Member_ID,
						business_unit_id = Convert.ToInt32(employee.business_unit_id),
						payment_method_id = Convert.ToInt32(payment_method),
						payperiod_id =
							Convert.ToInt32(
								payperiod_info.Select($"startdate <= '{d:yyyy-MM-dd}' AND  enddate >= '{d:yyyy-MM-dd}'")[0]["payperiodid"]),
						status_id = 1,
						member_id = employee.Member_ID,
						note = note
					};
					_request.save_record();
					vacations_toapprove.Add(_request);
					used_hours += 8;
				}
				EmailBranchManager(vacations_toapprove);
			}
			if (!(available_hours < used_hours)) return "Vacation request was sent successfully.";
			var em = new NeEMail
			{
				From = "noreply@" + Toolbox.app_setting("DomainForEmail"),
				To = CurrentUser.BranchManager.member_neemail,
				CC = "mhyde@" + Toolbox.app_setting("DomainForEmail"),
				Subject = "Alert: Vacation request above available hours"
			};
			var body_appendature = date_start == "" ? "" : " on the requested date (" + date_start + ")";
			em.Body =
				$"{employee.member_fullname} just requested a vacation payout in the amount of {used_hours:N2} hours, when they only have {available_hours:N2} hours available{body_appendature}.<br/> Please follow up with them on this.";
			em.isHTML = true;
			//em.Send();
			return "Vacation request has been sent successfully.";
		}

		public bool EmailBranchManager(ArrayList vacations_toapprove)
		{
			var m = new nesi.core.NeEMail()
			{
				From = "payroll@" + Toolbox.app_setting("DomainForEmail"),
				To = CurrentUser.ReportToManager.member_neemail,
				Subject = "Vacation Request",
				isHTML = true,
				to_member_id = CurrentUser.ReportToManager.Member_ID
			};

            var noteInfo = this.note;
            if (string.IsNullOrEmpty(noteInfo))
            {
                noteInfo = "Note: N/A";
            }
            else
            {
                noteInfo = "Note: " + noteInfo;
            }

            var body = new StringBuilder();
			m.passport_array = new ArrayList();
			body.AppendFormat(
				$@"<div style='font-family:arial; font-size:12px;'>{employee.member_fullname} has requested vacation from {date_start} to {
					date_return
				}.<br/>{noteInfo}</br><br/>Below are a list of days requested off.<br/>");
			var ids = new List<int>();
			foreach (VacationRequest v in vacations_toapprove)
			{
				ids.Add(v.vacation_id);
			}
			var id_list = string.Join(",", ids.ToArray<int>());
			if (vacations_toapprove.Count > 1)
			{
				var pao = new nesi.core.passport.array_object
				{
					url_yes = $"/sections/hr/vacation_admin/index.aspx?a=email-process&vacation_id={id_list}&type_of=3",
					url_no = $"/sections/hr/vacation_admin/index.aspx?a=email-process&vacation_id={id_list}&type_of=2",
					text_yes = "APPROVE ALL",
					text_no = "DENY ALL"
				};
				m.passport_array.Add(pao);
			}
			foreach (VacationRequest v in vacations_toapprove)
			{
				var pao = new nesi.core.passport.array_object()
				{
					url_yes = $"/sections/hr/vacation_admin/index.aspx?a=email-process&vacation_id={v.vacation_id}&type_of=3",
					url_no = $"/sections/hr/vacation_admin/index.aspx?a=email-process&vacation_id={v.vacation_id}&type_of=2",
					text_yes = $"APPROVE {v.date_start:yyyy-MM-dd htt} - {v.date_end:htt}",
					text_no = $"DENY {v.date_start:yyyy-MM-dd htt} - {v.date_end:htt}"
				};


				m.passport_array.Add(pao);
			}

            #region Other people taking vacation during the requested dates
            var _others = bllToolbox.doSQL_dt(@"
SELECT b.member_fullname name, CAST(CONCAT(DATE_FORMAT(a.date_start, '%Y-%m-%d'),' - ', DATE_FORMAT(a.date_return, '%Y-%m-%d')) AS CHAR) timespan , t.type vac_type
FROM vacation_master a 
LEFT JOIN vacation_type t ON t.id = a.type_id
LEFT JOIN member b ON a.member_id = b.member_id WHERE a.member_id != @v0  AND
(DATE(a.date_start) >=@v1 OR DATE(a.date_start) <=@v1) AND a.date_return > @v1 AND DATE(a.date_return) <=@v2 AND b.business_unit_id =@v3  AND a.status = 3",
employee.Member_ID, date_start, date_return, business_unit_id);
			if (_others.Rows.Count > 0)
			{
				body.Append(@"<br/><div><u>Other employees that have time off during this timespan</u><br/>");
				foreach (DataRow _other in _others.Rows)
				{
                    body.AppendFormat(@"{0} - ({1}) - {2}<br/>", _other["name"], _other["timespan"], _other["vac_type"]);

                }
                body.Append(@"</div><br/>");
			}
			else
			{
				body.Append(@"<br/><div>No other employees have vacations during the requested time period(s).</div><br/>");
			}
            #endregion Other people taking vacation during the requested dates
            #region Other days off this year by this employee
            var _days = bllToolbox.doSQL_dt(@"SELECT CAST(CONCAT(DATE_FORMAT(a.date_start, '%Y-%m-%d'),' - ', DATE_FORMAT(a.date_return, '%Y-%m-%d')) AS CHAR) timespan, t.type vac_type  FROM vacation_master a  LEFT JOIN vacation_type t ON t.id = a.type_id WHERE a.member_id = @v0  AND status IN (3,5) AND YEAR(date_start) = YEAR(NOW())", member_id);
            if (_days.Rows.Count > 0)
			{
				body.AppendFormat(@"<div><u>{0} has taken these days off this year:</u><br/>", employee.member_nickname);
				foreach (DataRow _day in _days.Rows)
				{
					var _timespan = _day["timespan"].ToString();
                    var _vac_type = _day["vac_type"];
                    body.AppendFormat(@"{0} - {1}<br/>", _timespan, _vac_type);
                }
				body.Append(@"</div><br/>");
			}
			else
			{
				body.AppendFormat(@"<div>{0} hasn't taken any other days off this year.</div><br/>", employee.member_nickname);
			}
			#endregion Other days off this year by this employee
			body.AppendFormat(@"<div>{1}'s Hire Date: {0}</div><br/></div>", bllToolbox.MySQL_shortdt(Convert.ToDateTime(employee.member_startdate)), employee.member_nickname);
			m.Body = body.ToString();
			try
			{
				m.file_passport();
				foreach (VacationRequest vr in vacations_toapprove)
				{
					bllToolbox.doSQL_void(@"INSERT INTO vacation_log (dt, vacation_id, member_id, action) 
VALUES (NOW(), @v0,@v1, CONCAT('Vacation Request emailed to ',@v2))", vr.vacation_id, employee.Member_ID, m.To);
				}
				return true;
			}
			catch (Exception ee)
			{
				//Toolbox.do_catch_error(ee, 711);
				foreach (VacationRequest vr in vacations_toapprove)
				{
					bllToolbox.doSQL_void(@"INSERT INTO vacation_log (dt, vacation_id, member_id, action) 
VALUES (NOW(), @v0, @v1, CONCAT('Vacation Request not emailed because: ',@v2))", vr.vacation_id, employee.Member_ID, ee.ToString());
				}
				return true;
			}
		}
	}
}