using System;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Page.TimeSheet.Vacation;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace NESI.BLL.Pages.Timesheet.Vacation
{
	public class VacationBLL : BLLBase
	{
		private readonly Vacation vacation;

		public VacationBLL(Employee user) : base(user)
		{
			this.vacation = new Vacation(user);
		}
		/// <summary>
		/// Add vacation accrual to user's account
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public string AddVacation(AddVacation model)
		{
			var v = new Vacation(CurrentUser)
			{
				o = model.O,
				all_outstanding = model.Alloutstanding,
				unpaid = model.Unpaid,
				date_start = bllToolbox.MySQL_shortdt(model.Date_start),
				date_end = bllToolbox.MySQL_shortdt(model.Date_end),
				date_return = bllToolbox.MySQL_shortdt(model.Date_return),
				note = model.Note,
				hours = model.Hours
			};
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (model.Hours == 0 && model.Money != 0)
			{
				v.hours = Math.Round(model.Money / v.UserPayRate(), 5);
			}
			return v.save();

		}
		/// <summary>
		/// Returns what is available right now
		/// </summary>
		/// <param name="_type">
		///		<para>1 = Hours, 2 = Dollars</para>
		/// </param>
		/// <returns></returns>
		public double GetCurrentAvailable(int _type)
		{
			var value = bllToolbox.doSQL_doubleOrNull(@"CALL VACATION_AVAILABLE_ACCRUALATDATE(@v0, CURDATE(), @v1)", UserId, _type);
		    return value.HasValue ? value.Value : 0;
		}
		/// <summary>
		/// Returns what is available at supplied
		/// </summary>
		/// <param name="_type">
		///		<para>1 = Hours, 2 = Dollars</para>
		/// </param>
		/// <param name="_date"></param>
		/// <returns></returns>
		public double GetAvailableAtDate(int _type, DateTime _date)
		{
		    if (_date < DateTime.Now)
		    {
		        return 0;
		    }
		   
		    var value = bllToolbox.doSQL_doubleOrNull(@"CALL VACATION_AVAILABLE_ACCRUALATDATE(@v0, @v1, @v2)", UserId, _date, _type);
		    return value.HasValue ? value.Value : 0;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ReviewVacation GetVacationReview(AddVacation model)
		{
			var rv = new ReviewVacation();
			var country_appendage = "";
			if (CurrentUser.BusinessUnit.Country == "CDN")
			{
				country_appendage = " holidays_canada = 1 AND ";
			}
			else if (CurrentUser.BusinessUnit.Country == "USA")
			{
				country_appendage = " holidays_america = 1 AND ";
			}

			rv.date_start = model.Date_start.ToString("D");
			rv.date_end = model.Date_end.ToString("D");
			rv.date_return = model.Date_return.ToString("D");
			rv.type_of_payment = model.Alloutstanding ? "All Outstanding" : model.Unpaid ? "Unpaid" : "Vacation Pay";
			rv.affected_payrolls = GetAffectedPayrolls(model.Date_start, model.Date_end);
			rv.holidays = bllToolbox.doSQL_Array<string>(@"SELECT CONCAT(holidays_name, ' - ', DATE_FORMAT(holidays_date, '%Y-%m-%d')) h FROM holidays WHERE " + country_appendage + @" holidays_date BETWEEN @v0  AND @v1 ",
				model.Date_start, model.Date_end);
			//rv.available_hours = GetCurrentAvailable(1);
			//rv.available_dollars = GetCurrentAvailable(2);
			rv.hours_atdate = GetAvailableAtDate(1, model.Date_start);
			//rv.dollars_atdate = GetAvailableAtDate(2, model.Date_start);

			rv.hours =model.Alloutstanding? rv.hours_atdate :
				(model.Unpaid ? 0 : GetBusinessDays(model.Date_start, model.Date_end) * 8);
			rv.dollars = rv.hours * CurrentUser.GetWage();
			rv.isAlloutstanding = model.Alloutstanding;
			return rv;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="startD"></param>
		/// <param name="endD"></param>
		/// <returns></returns>
		protected static double GetBusinessDays(DateTime startD, DateTime endD)
		{
			startD = DateTime.Parse(startD.ToString("D"));
			endD = DateTime.Parse(endD.ToString("D"));
			var calcBusinessDays =
				1 + ((endD - startD).TotalDays * 5 -
					 (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

			if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
			if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

			return calcBusinessDays;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		protected string[] GetAffectedPayrolls(DateTime start, DateTime end)
		{
			start = DateTime.Parse(start.ToString("D"));
			end = DateTime.Parse(end.ToString("D"));
			var r = bllToolbox.doSQL_Array<string>(
				@"select CONCAT(Date_FORMAT(startdate, '%M %d %Y'),' - ',DATE_FORMAT(enddate, '%M %d %Y')) 
			from payperiods where startdate between @v0  AND @v1  or enddate between @v0  AND @v1 ",
				start, end);

			if (r == null || r.Length == 0)
			{
				r = bllToolbox.doSQL_Array<string>(
					@"SELECT CONCAT(Date_FORMAT(startdate, '%M %d %Y'),' - ', DATE_FORMAT(enddate, '%M %d %Y')) FROM payperiods WHERE startdate <= @v0  AND enddate >= @v1",
					start, end);
			}
			return r;
		}
	}
}