using System;
using System.Data;
using DevExpress.XtraRichEdit.Commands;
using NESI.Common.Models;

namespace nesi.core
{
	/// <summary>
	/// Interfact to the NE Holidays Table
	/// </summary>
	public class NeHolidays
	{
		public int id { get; set; }
		public string name { get; set; }
		public string date { get; set; }
		public int create_id { get; set; }
		public string create_date { get; set; }
		public int audit_id { get; set; }
		public string modified_date { get; set; }
		public int is_canada { get; set; }
		public int is_america { get; set; }
		public int is_active { get; set; }

		public NeHolidays() { }
		public NeHolidays(int _id)
		{
			if (exists(_id))
			{
				init(_id);
			}
		}
		private class HourTypes
			{
			public double RegularTime             { get; set;}
			public double OverTime                { get; set;}
			public double DoubleTime              { get; set;}
			public double RegularTimeShiftPremium { get; set;}
			public double OverTimeShiftPremium    { get; set;}
			public double DoubleTimeShiftPremium  { get; set;}
			public double AutoVacationPayout	  { get; set;}
			public double PaidTimeOff			  { get; set;}
			public double Total => RegularTime+OverTime+DoubleTime+RegularTimeShiftPremium+OverTimeShiftPremium+DoubleTimeShiftPremium+AutoVacationPayout+PaidTimeOff;
			}
		private class HolidayPayValues
			{
			public  HolidayPayValues()
				{
				ThisPayPeriod = new HourTypes();
				LastPayPeriod = new HourTypes();
				TwoPayPeriodsAgo = new HourTypes();
				}
			public HourTypes ThisPayPeriod {get; set;}
			public HourTypes LastPayPeriod {get; set;}
			public HourTypes TwoPayPeriodsAgo {get; set;}
			}
	// MH 2020-11-04 - Commenting this out until it's completed.
	/*
		public static Toolbox.doubleString StatPayValue(NePayPeriod payPeriod, NeMember member)
			{
			var ret = new Toolbox.doubleString();
			if(member.paytype_id == OpsMemberPayType.SalaryHourlyWithoutTimeSheet)
				{
				ret.amount = 8;
				ret.reason = "Salaried employee";
				return ret;
				}
			// MH: 2020-11-02 - Base calculation is as follows: Average of past two pay periods, inclusive of RT,OT,DT,RTsp,OTsp,DTsp,Auto Vacation Payouts
			using var conn = Toolbox.connect();
			var vals = new HolidayPayValues();
			var StartDate = Convert.ToDateTime(member.StartDate);
			var activePayPeriod = payPeriod.id;

			vals.ThisPayPeriod.RegularTime					= payroll.hours.CurrentTotalByHourType(conn, activePayPeriod, member.id, OpsPayType.Regular)*OpsPayType.Multipliers.RegularTime*member.wage;
			vals.ThisPayPeriod.OverTime						= payroll.hours.CurrentTotalByHourType(conn, activePayPeriod, member.id, OpsPayType.OverTime)*OpsPayType.Multipliers.OverTime*member.wage;
			vals.ThisPayPeriod.DoubleTime					= payroll.hours.CurrentTotalByHourType(conn, activePayPeriod, member.id, OpsPayType.DoubleTime)*OpsPayType.Multipliers.DoubleTime*member.wage;
			vals.ThisPayPeriod.RegularTimeShiftPremium		= payroll.hours.CurrentTotalByHourType(conn, activePayPeriod, member.id, OpsPayType.RegularTimeShiftPremium)*OpsPayType.Multipliers.RegularTimeShiftPremium*member.wage;
			vals.ThisPayPeriod.OverTimeShiftPremium			= payroll.hours.CurrentTotalByHourType(conn, activePayPeriod, member.id, OpsPayType.OverTimeShiftPremium)*OpsPayType.Multipliers.OverTimeShiftPremium*member.wage;
			vals.ThisPayPeriod.DoubleTimeShiftPremium		= payroll.hours.CurrentTotalByHourType(conn, activePayPeriod, member.id, OpsPayType.DoubleTimeShiftPremium)*OpsPayType.Multipliers.DoubleTimeShiftPremium*member.wage;
			// As the payout is automatically figured out based on the approved hours, we need to add a base-line value.
			vals.ThisPayPeriod.AutoVacationPayout			= !member.ReceivesAutoVacationPayout 
																? 0 
																: vals.ThisPayPeriod.Total * member.CurrentVacationAmount;
			var CurrentWage										= payroll.get_wage_at_date(conn, member.id, payPeriod.end_date.AddDays(-1));
			if(CurrentWage == 0)
				{
				ret.amount = 0;
				ret.reason = "Error calculating Stat Pay - No Wage Present.";
				}
			vals.ThisPayPeriod.PaidTimeOff					= payroll.DaysOff.ThisPayPeriod(conn, member.id, activePayPeriod, true) * CurrentWage;
			

			// MH: 2020-11-02 - Last Pay Period
			activePayPeriod = payPeriod.id - 1;
			var tempPayPeriod = new NePayPeriod(activePayPeriod);

			vals.LastPayPeriod.RegularTime					= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.RegularTime)*OpsPayType.Multipliers.RegularTime;
			vals.LastPayPeriod.OverTime						= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.OverTime)*OpsPayType.Multipliers.OverTime;
			vals.LastPayPeriod.DoubleTime					= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.DoubleTime)*OpsPayType.Multipliers.DoubleTime;
			vals.LastPayPeriod.RegularTimeShiftPremium		= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.RegularTimeShiftPremium)*OpsPayType.Multipliers.RegularTimeShiftPremium;
			vals.LastPayPeriod.OverTimeShiftPremium			= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.OverTimeShiftPremium)*OpsPayType.Multipliers.OverTimeShiftPremium;
			vals.LastPayPeriod.DoubleTimeShiftPremium		= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.DoubleTimeShiftPremium)*OpsPayType.Multipliers.DoubleTimeShiftPremium;
			vals.LastPayPeriod.AutoVacationPayout			= member.ReceivesAutoVacationPayout 
																? payroll.vacation.GetAutoVacationPayout(activePayPeriod, member.id) 
																: 0;
			var LastWage									= StartDate < tempPayPeriod.start_date ? payroll.get_wage_at_date(conn, member.id, tempPayPeriod.end_date.AddDays(-1)) : 0;
			vals.LastPayPeriod.PaidTimeOff					= payroll.DaysOff.ThisPayPeriod(conn, member.id, activePayPeriod, true) * LastWage;

			// MH: 2020-11-02 -  Two Pay Periods Ago
			activePayPeriod = payPeriod.id - 2;
			tempPayPeriod = new NePayPeriod(activePayPeriod);

			vals.TwoPayPeriodsAgo.RegularTime				= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.RegularTime)*OpsPayType.Multipliers.RegularTime;
			vals.TwoPayPeriodsAgo.OverTime					= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.OverTime)*OpsPayType.Multipliers.OverTime;
			vals.TwoPayPeriodsAgo.DoubleTime				= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.DoubleTime)*OpsPayType.Multipliers.DoubleTime;
			vals.TwoPayPeriodsAgo.RegularTimeShiftPremium	= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.RegularTimeShiftPremium)*OpsPayType.Multipliers.RegularTimeShiftPremium;
			vals.TwoPayPeriodsAgo.OverTimeShiftPremium		= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.OverTimeShiftPremium)*OpsPayType.Multipliers.OverTimeShiftPremium;
			vals.TwoPayPeriodsAgo.DoubleTimeShiftPremium	= payroll.hours.HistoricTotalByHourType(conn, activePayPeriod, member.id, payroll.hours.HourTypes.DoubleTimeShiftPremium)*OpsPayType.Multipliers.DoubleTimeShiftPremium;
			vals.TwoPayPeriodsAgo.AutoVacationPayout		= member.ReceivesAutoVacationPayout 
																? payroll.vacation.GetAutoVacationPayout(activePayPeriod, member.id) 
																: 0;
			var TwoWage										= StartDate < tempPayPeriod.start_date ? payroll.get_wage_at_date(conn, member.id, tempPayPeriod.end_date.AddDays(-1)) : 0;
			vals.TwoPayPeriodsAgo.PaidTimeOff				= payroll.DaysOff.ThisPayPeriod(conn, member.id, activePayPeriod, true) * TwoWage;

			// MH: 2020-11-02 - As these are all extended values based on their pay at date, we need to add them up and divide by their current wage.
			// Using 60 as there are 60 days inclusive and 3 as there are three summations to average.
			var d = 0;
			if(vals.ThisPayPeriod.Total > 0)
				{
				d += 10;
				}
			if(vals.LastPayPeriod.Total > 0)
				{
				d += 10;
				}
			if(vals.TwoPayPeriodsAgo.Total > 0)
				{
				d += 10;
				}
			if(d == 0 || member.wage == 0)
				{
				ret.amount = 0;
				ret.reason = d == 0 
								? "Nothing exists to calculate against?"
								: "Wage is 0?";
				return ret;
				}

			ret.amount = Math.Round((vals.ThisPayPeriod.Total + vals.LastPayPeriod.Total +  vals.TwoPayPeriodsAgo.Total) / d / member.wage, 2);
			try
				{
				if(vals.ThisPayPeriod.Total > 0)
					{ 
				ret.reason = $@"
Current week base total: {vals.ThisPayPeriod.Total:C2} 
  Calculation: (({vals.ThisPayPeriod.RegularTime:C2} RT) + ({vals.ThisPayPeriod.OverTime:C2} OT) + ({vals.ThisPayPeriod.DoubleTime:C2} DT) + ({vals.ThisPayPeriod.RegularTimeShiftPremium:C2} RTsp) + ({vals.ThisPayPeriod.OverTimeShiftPremium:C2} OTsp) + ({vals.ThisPayPeriod.DoubleTimeShiftPremium:C2} DTsp)) + ({vals.ThisPayPeriod.AutoVacationPayout:C2} Vacation Auto Payout)";
					}
				if(vals.LastPayPeriod.Total > 0)
					{
					ret.reason += $@"
Last pay period total: {vals.LastPayPeriod.Total:C2} +
  Calculation: (({vals.LastPayPeriod.RegularTime:C2} RT) + ({vals.LastPayPeriod.OverTime:C2} OT) + ({vals.LastPayPeriod.DoubleTime:C2} DT) + ({vals.LastPayPeriod.RegularTimeShiftPremium:C2} RTsp) + ({vals.LastPayPeriod.OverTimeShiftPremium:C2} OTsp) + ({vals.LastPayPeriod.DoubleTimeShiftPremium:C2} DTsp)) + ({vals.LastPayPeriod.AutoVacationPayout:C2} Vacation Auto Payout)";
					}
				if(vals.TwoPayPeriodsAgo.Total > 0)
					{ 
					ret.reason += $@"
Two pay periods ago: {vals.TwoPayPeriodsAgo.Total:C2} /
  Calculation: (({vals.TwoPayPeriodsAgo.RegularTime:C2} RT) + ({vals.TwoPayPeriodsAgo.OverTime:C2} OT) + ({vals.TwoPayPeriodsAgo.DoubleTime:C2} DT) + ({vals.TwoPayPeriodsAgo.RegularTimeShiftPremium:C2} RTsp) + ({vals.TwoPayPeriodsAgo.OverTimeShiftPremium:C2} OTsp) + ({vals.TwoPayPeriodsAgo.DoubleTimeShiftPremium:C2} DTsp)) + ({vals.TwoPayPeriodsAgo.AutoVacationPayout:C2} Vacation Auto Payout)";
					}
				ret.reason += $@"
Number of working days in pay period(s): {d} /
Current Wage: {member.wage:C2} = 
{ret.amount} Hours";
				}
			catch(Exception ee)
				{
				ret.reason = "Error creating tool tip.";
				Toolbox.do_errorLog(ee, $"Error creating tooltip for {member.FullName} - PayPeriod {payPeriod.id}");
				}
			return ret;
			}
			*/
	public static bool DateIsHoliday(DateTime _inDate, bool _isCanadian)
		{
		var countryField		= _isCanadian ? "holidays_canada" : "holidays_america";
		return Toolbox.doSQL_int(string.Format(@"SELECT COUNT(*) FROM holidays WHERE {0} = 1 AND holidays_date = '{1}'", countryField, _inDate.Date), new object[]{}) == 1;
		}
		private void init(int _id)
		{
			var _dt = Toolbox.doSQL_dt(@" SELECT holidays_id, DATE_FORMAT(holidays_date, '%Y-%m-%d') holidays_date, holidays_name, member_create_id, member_audit_id, create_date, modified_date, holidays_canada, holidays_america, holidays_active FROM holidays WHERE holidays_id = @v0 ", new object[] {  _id } );
			foreach (DataRow _dr in _dt.Rows)
			{
				id = (int)_dr["holidays_id"];
				date = _dr["holidays_date"].ToString();
				name = (string)_dr["holidays_name"];
				create_id = (int)_dr["member_create_id"];
				audit_id = (int)_dr["member_audit_id"];
				create_date = _dr["create_date"].ToString();
				modified_date = _dr["modified_date"].ToString();
				is_canada = (int)_dr["holidays_canada"];
				is_america = (int)_dr["holidays_america"];
				is_active = (int)_dr["holidays_active"];
			}
		}
		private bool exists(int _id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM holidays WHERE holidays_id = @v0", _id) > 0;
		}
		public void save()
		{
			if (id > 0)
			{
				// Update
				Toolbox.doSQL_void(@"
UPDATE
	holidays
SET
	holidays_date = @v0,
	holidays_name = @v1,
	modified_date = NOW(),
	holidays_canada = @v2,
	holidays_america = @v3,
	holidays_active = @v4
WHERE
	holidays_id = @v5
LIMIT 1", new object[] {
					date,
					name,
					is_canada,
					is_america,
					is_active,
					id});
			}
			else
			{
				// Insert
				Toolbox.doSQL_void(@"
INSERT INTO holidays
	(
	holidays_date, 
	holidays_name, 
	member_create_id, 
	create_date, 
	member_audit_id, 
	modified_date, 
	holidays_canada, 
	holidays_america, 
	holidays_active 
	) 
VALUES 	(@v0,@v1,@v2,CURDATE(),@v3,NOW(),@v4,@v5,1)",
	new object[] {
					date,			// {0}
					name, 			// {1}
					create_id, 		// {2}
					audit_id, 		// {3}
					is_canada, 		// {4}
					is_america		// {5}
				});
			}
		}
	}
}