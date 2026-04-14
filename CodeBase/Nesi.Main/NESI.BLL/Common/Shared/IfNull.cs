using System;

namespace NESI.BLL.Common.Shared
{
    public static class IfNull
    {
        
        public static double ReturnZeroIfNull_double(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(n);
            }
        }
        public static int ReturnZeroIfNull_int(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(n);
            }
        }
        public static string ReturnBlankIfNull_string(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "")
            {
                return "";
            }
            else
            {
                return n.ToString();
            }
        }
        public static DateTime? ReturnNullDateTime(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "" || Convert.ToDateTime(n).Year < 1900)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(n);
            }
        }
        public static DateTime ReturnBlankDateTimeIfNull(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "")
            {
                return new DateTime();
            }
            else
            {
                return Convert.ToDateTime(n);
            }
        }
        public static DateTime? ReturnNullableDateTimeIfNull(object n)
        {
            if (n == DBNull.Value || n == null || n.ToString().Trim() == "")
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(n);
            }
        }
    }
}
