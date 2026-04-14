using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NESI.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidBase64EncodedString(this string value)
        {
            value = value.Trim();
            return (value.Length % 4 == 0) && Regex.IsMatch(value, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}
