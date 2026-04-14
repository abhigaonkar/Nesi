using System;

namespace NESI.BLL.Common.Shared
{
	public static class DateTimeTools
	{
		public static int MonthDifference(DateTime lValue, DateTime rValue)
		{
			return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
		}
		public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
		{
			// The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
			var daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
			return start.AddDays(daysToAdd);
		}

		public static string MySQLNow_long()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}
		public static string MySQLNow_short()
		{
			return DateTime.Now.ToString("yyyy-MM-dd");
		}
	}
}