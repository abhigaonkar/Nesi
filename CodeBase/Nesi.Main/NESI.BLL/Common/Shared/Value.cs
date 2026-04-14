using System.Text.RegularExpressions;
using System.Web;

namespace NESI.BLL.Common.Shared
{
	public static class Value
	{
		/// <summary>
		/// This is used when you want to make a value acceptable for HTML
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string value_from(object value)
		{
			return value_from(value, true);
		}

		/// <summary>
		/// This is used when you want to make a value acceptable for HTML or just plainly decode a value
		/// </summary>
		/// <param name="value">Anything</param>
		/// <param name="force">True will return a html encoded string, false will not.</param>
		/// <returns></returns>
		public static string value_from(object value, bool force)
		{
			var temp_string = force ? HttpUtility.HtmlEncode(HttpUtility.UrlDecode(value.ToString())).Replace("%22", "&quot;") : HttpUtility.UrlDecode(value.ToString());
			return temp_string;
		}

		/// <summary>
		/// This is used when you want to make a value acceptable for MySQL
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string value_to(object value)
		{
			var _value = value?.ToString() ?? "";
			var already_encoded = new Regex("%[0-9A-F][0-9A-F]", RegexOptions.IgnoreCase);
			try
			{
				while (already_encoded.Match(_value).Success)
				{
					_value = HttpUtility.UrlDecode(_value);
				}
				_value = HttpUtility.UrlEncode(_value);
				_value = _value.Replace(" ", "%20")
					.Replace("+", "%20")
					.Replace("'", "%27")
					.Replace("/", "%2F")
					.Replace("#", "%23")
					.Replace(".", "%2E")
					.Replace("(", "%28")
					.Replace(")", "%29")
					.Replace("-", "%2D");
			}
			catch
			{
				_value = "";
			}
			return _value;
		}
	}
}