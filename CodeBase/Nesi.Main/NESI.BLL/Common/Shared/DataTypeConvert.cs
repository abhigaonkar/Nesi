using System;

namespace NESI.BLL.Common.Shared
{
	public static class DataTypeConvert
	{
		public static string MySQL_shortdt(DateTime dt)
		{
			return dt.Year < 1900 ? "" : dt.ToString("yyyy-MM-dd");
		}
		public static string MySQL_longdt(DateTime dt)
		{
			return dt.Year < 1900 ? "" : dt.ToString("yyyy-MM-dd HH:mm:ss");
		}
		public static string MySQL_longdt(DateTime? _dt)
		{
			if (_dt == null)
			{
				return "";
			}
			else
			{
				var dt = Convert.ToDateTime(_dt);
				return dt.ToString("yyyy-MM-dd HH:mm:ss");
			}
		}
		public static bool is_valid_date(DateTime? dt)
		{
			return dt != null && is_valid_date((DateTime)dt);
		}
		public static bool is_valid_date(DateTime dt)
		{
			return dt.Year > 2000;
		}
	}
}