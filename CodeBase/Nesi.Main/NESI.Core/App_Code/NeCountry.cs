using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace nesi.core
{
	public class NeCountry
	{
		public string code { get; set; }
		public string name { get; set; }
		public string cty_currency { get; set; }
		public string unit { get; set; }

		public static List<NeCountry> get_list()
		{
			using (var conn = Toolbox.connect())
			{
				var dt = Toolbox.doSQL_dt(conn,@"SELECT country_code code,country_name name FROM country ORDER BY country_name" ,null);
				return (from DataRow dr in dt.Rows select new NeCountry { code = (string)dr["code"], name = (string)dr["name"] }).ToList();
			}
		}
		public static string get_name(string code)
		{
			return Toolbox.doSQL_string("SELECT IFNULL(MAX(country_name), 'N/A') FROM country WHERE country_code =@v0", new object[] { code });
		}
	}
}