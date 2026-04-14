using System;
using System.Data;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Timesheet.Bank
{
	public class Banked_pay : BLLBase
	{

		public Banked_pay(Employee user) : base(user)
		{
			member_id = CurrentUser.Id;
			admin_id = CurrentUser.Id;
			payperiod_id = CurrentPayPeriod();
			var payp = new Ne2PayPeriod(payperiod_id);
			date_start = payp.StartDate;
			date_end = payp.Enddate;

		}



		public int member_id;
		public int admin_id;
		public int payperiod_id;
		public string date_start;
		public string date_end;
		public string separate_check = "0";

		public DataTable GetLedger(int payperiodId = 0)
		{
			if (payperiodId == 0) payperiodId = this.payperiod_id;

			return bllToolbox.doSQL_dt(@"
							SELECT A.id, A.added_by, CONCAT(B.member_firstname, ' ', B.member_lastname) added_by_name, 
							DATE_FORMAT(A.date, '%M %d, %Y @ %l:%i:%s%p') date,
							A.type, A.hours, A.payrate, (A.hours * A.payrate) total, 
							A.note 
							FROM bankedpay_ledger A 
							LEFT JOIN member B ON A.added_by = B.member_id  
							WHERE A.member_id =@v0 and A.payperiod_id =@v1  
							and A.type in ('W', 'D', 'R', 'E', 'P')", member_id, payperiodId);
		}

		public DTO.ViewModels.Core.LabelValueInt[] GetHistory(int payperiodId = 0)
		{
			if (payperiodId == 0) payperiodId = this.payperiod_id;

			var list = bllToolbox.doSQL_List<DTO.ViewModels.Core.LabelValueInt>(@" select concat('(',ID,') ', START, ' - ', END) Label,
							ID value FROM (
							SELECT distinct(a.payperiod_id) ID, 
							DATE_FORMAT(b.startdate, '%M %d, %Y') START, 
							DATE_FORMAT(b.enddate, '%M %d, %Y') END 
							
							FROM bankedpay_ledger a 
							LEFT JOIN payperiods b 
							ON a.payperiod_id = b.PayPeriodID  
							WHERE a.payperiod_id <= @v1 
							AND a.type in ('W', 'D', 'P', 'E') 
							AND a.member_id = @v0 
							ORDER BY ID DESC) a", member_id, payperiodId);

			list.Insert(0, new LabelValueInt()
			{
				Label = "Please choose a pay period",
				Value = 0
			});
			return list.ToArray();
		}

		public bool has_history()
		{
			return bllToolbox.doSQL_int(@"SELECT count(*) FROM bankedpay_ledger WHERE member_id =@v0", member_id) > 0;
		}

		public double available_hours()
		{
			var payroll_hours = this_payroll_hours();
			var deposited_hours = get_hours('D', payperiod_id);
			var withdrawn_hours = get_hours('W', payperiod_id);
			var available_hours = payroll_hours - (deposited_hours - withdrawn_hours);
			if (available_hours < 0)
			{
				available_hours = 0;
			}
			return available_hours;
		}
		public double this_payroll_hours()
		{
			return bllToolbox.doSQL_double(@"SELECT IFNULL(SUM(numberofhours), 0) TOTAL FROM membertime WHERE membertime_memberid = @v0  AND date >= @v1  AND date <= @v2  AND membertime_paytypehours_id = 1", member_id, date_start, date_end);
		}
		public double balance()
		{
			if (payperiod_id == 0)
			{
				payperiod_id = new BLL.Core.Ne2Payroll().current_pay_period();
			}
			var bankedpay = bllToolbox.doSQL_double(@"SELECT IFNULL(SUM(hours * payrate), 0) AS banked FROM bankedpay_ledger  WHERE type='D' AND member_id = @v0", member_id);
			var withdrawn = Math.Round(get_dollars('W', payperiod_id + 1, true), 2, MidpointRounding.AwayFromZero);
			var deducted = Math.Round(get_dollars('E', payperiod_id + 1, true), 2, MidpointRounding.AwayFromZero);
			var paidout = Math.Round(get_dollars('P', payperiod_id + 1, true), 2, MidpointRounding.AwayFromZero);
			var balance = bankedpay - withdrawn - deducted - paidout;
			return balance;
		}


		public double get_dollars(char _type, int _payperiod_id, bool _less_than_payperiod = false)
		{
			var less_than = _less_than_payperiod ? "<=" : "=";
			return bllToolbox.doSQL_double(
				$"SELECT IFNULL(SUM(hours * payrate),0) AS banked FROM bankedpay_ledger WHERE type='{_type}' AND member_id = {member_id} AND payperiod_id {less_than} {_payperiod_id}");
		}

		public double get_hours(char _type, int _payperiod_id, bool _less_than_payperiod = false)
		{
			var less_than = _less_than_payperiod ? "<=" : "=";
			return bllToolbox.doSQL_double(
				$"SELECT IFNULL(SUM(hours),0) AS banked FROM bankedpay_ledger WHERE type='{_type}' AND member_id = {member_id} AND payperiod_id {less_than} {_payperiod_id}");
		}
		public double withdrawable_hours()
		{
			var bankable_money = get_dollars('D', payperiod_id, true) -
								 get_dollars('W', payperiod_id, true) -
								 get_dollars('P', payperiod_id, true) -
								 get_dollars('E', payperiod_id, true);
			double balance_hours = 0;
			if (bankable_money > 0)
			{
				balance_hours = Math.Round(Math.Round(bankable_money, 2, MidpointRounding.AwayFromZero) / user_pay_rate(), 2, MidpointRounding.AwayFromZero);
			}
			return balance_hours;
		}
		public string bank_hours(double _hours)
		{
			if (_hours > available_hours() || _hours <= 0)
			{
				return "You cannot deposit more hours than you have worked";
			}

            if (IsPayPeriodCompleted())
            {
                return "You cannot deposit more hours due to pay period is completed.";
            }

            try
			{
				bllToolbox.doSQL_void(@"INSERT INTO bankedpay_ledger (date, type, member_id, added_by, hours, payrate, payperiod_id, separate_check)
				VALUES (now(), 'D', @v0, @v1, @v2, @v3, @v4, @v5)", member_id, admin_id, _hours, user_pay_rate(), payperiod_id, separate_check);
				return "Hours have been deposited successfully.";

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return "Updating information failed.";
			}

		}
		public string withdraw_hours(double _hours)
		{
			if (balance() < 0 || _hours > withdrawable_hours() || _hours < 0)
			{
				return "You cannot withdraw more hours than you have deposited";
			}

            if (IsPayPeriodCompleted())
            {
                return "You cannot withdraw more hours due to pay period is completed.";
            }

            try
			{
				bllToolbox.doSQL_void(@"INSERT INTO bankedpay_ledger (date, type, member_id, added_by, hours, payrate, payperiod_id) 
					VALUES (now(), 'W', @v0, @v1, @v2, @v3, @v4)", member_id, admin_id, _hours, user_pay_rate(), payperiod_id);
				return "Hours have been withdrawed successfully.";
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return "Updating information failed.";
			}
		}
		public string retract_hours(int _id)
		{
            if (IsPayPeriodCompleted())
            {
                return "You cannot retract this record due to pay period is completed.";
            }

			bllToolbox.doSQL_void(@"UPDATE bankedpay_ledger SET type = 'R' WHERE id =@v0 ", _id);
			return "Hours have been retracted successfully.";
		}
		public int current_pay_period()
		{
			var thisid = 0;
			var payperiod_info = bllToolbox.doSQL_dt(@"SELECT PayperiodID, DATE_FORMAT(StartDate, '%Y-%m-%d') StartDate, DATE_FORMAT(EndDate, '%Y-%m-%d') EndDate FROM payperiods  WHERE StartDate < now() AND EndDate > now()", null);
			foreach (DataRow info in payperiod_info.Rows)
			{
				thisid = Convert.ToInt32(info["PayperiodID"].ToString());
				date_start = info["StartDate"].ToString();
				date_end = info["EndDate"].ToString();
			}
			return thisid;
		}

		public int CurrentPayPeriod()
		{
			var payperiodid = bllToolbox.doSQL_int("CALL _payperiod()");
			var pp = new Ne2PayPeriod(payperiodid);
			// This should not be in place for banked pay anymore. 
			//if (Convert.ToDateTime(pp.Enddate) < DateTime.Now)
			//{
			//	pp.payperiodID = bllToolbox.doSQL_int(@"SELECT payperiodid FROM payperiods WHERE startdate <= NOW() AND enddate >= NOW()");
			//}
			return Convert.ToInt32(pp.payperiodID);
		}

		public double user_pay_rate()
		{
			return Math.Round(bllToolbox.doSQL_double(@"SELECT GET_WAGE(@v0 )", member_id), 2);
		}
		public string user_pay_type()
		{
			return bllToolbox.doSQL_string(@"SELECT paytype FROM currentwage WHERE member_id =@v0", member_id);
		}

        //
        // Bug 1740: Check Payroll Status when interacting with Banked Pay.
        //
        private bool IsPayPeriodCompleted()
        {
            //
            // This will not take any effect if removing comments.
            //
            // return false;

            //
            // Check the whole payperiod.
            //
            bool completed = false;
            var payp = new Ne2PayPeriod(payperiod_id);
            if (payp.is_complete)
            {
                completed = true;
            }

            //
            // Check this employee's status.
            //
            var employeePayrollApproved = bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll_hours WHERE member_id = @v0 AND payperiod_id = @v1", member_id, payperiod_id);
            if (employeePayrollApproved > 0)
            {
                //throw new Exception("Payroll has already been approved for this employee, expenses/per diems cannot be changed. If you need to change this request, this employee's payroll needs to be first moved back.");
                completed = true;
            }

            return completed;
        }
	}
}

