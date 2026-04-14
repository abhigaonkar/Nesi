using System;
using System.Data;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeBusinessUnit
	/// </summary>
	public class NECurrency
		{

		/// <summary>
		/// Provides the Company ID
		/// </summary>
		public NECurrency(){}
		public NECurrency(int id)
			{
			var dt = Toolbox.doSQL_dt(@"Select * from currency  where id = @v0", new object[] { id });
			if (dt.Rows.Count > 0)
				{
				currency = dt.Rows[0]["currency"].ToString();
				rate_to_usd = Convert.ToDouble(dt.Rows[0]["per_usd"]);
				}
			}

		public string currency;
		public double rate_to_usd;
	
/*	public static double convert_cad_to_usd (double cad)
	{
		return(cad/(Toolbox.doSQL_double(@"Select per_usd from currency  where currency ='CAD'")) ,null);
	}
	public static double convert_eur_to_usd(double eur)
	{
		return (eur / (Toolbox.doSQL_double(@"Select per_usd from currency  where currency ='EUR'")) ,null);
	}
	public static double convert_usd_to_cad(double usd)
	{
		return (usd * (Toolbox.doSQL_double(@"Select per_usd from currency  where currency ='CAD'")) ,null);
	}
	public static double convert_usd_to_EUR(double eur)
	{
		return (eur * (Toolbox.doSQL_double(@"Select per_usd from currency  where currency ='EUR'")) ,null);
	}
 */

		public static double get_exchange_rate_at_date(int member_id, DateTime dt, string _currency)
			{
			var _tools = new Toolbox();
			var _home_currency ="";
			double _exchange_rate =1;
			var format_date ="";	
			format_date = dt.ToString("yyyy-MM-dd");
			_home_currency= Toolbox.doSQL_string("SELECT member.member_country FROM  member  WHERE member_id =@v0", member_id);
			if (_currency != _home_currency)
				{
				switch (_currency)
					{
						case "USA":
							switch (_home_currency)
								{
									case "CAN":
										_exchange_rate = _tools.getSQL_double(@"Select per_usd from currency_history  where currency = 'CAD' and currency_history.date<=@v0  order by currency_history.date desc limit 1", new object[] { format_date });
										break;
									case "EUR":
										_exchange_rate = _tools.getSQL_double(@"Select per_usd from currency_history  where currency = 'EUR' and currency_history.date<=@v0  order by currency_history.date desc limit 1", new object[] { format_date });
										break;
								}
							break;
						case "CAN":
							switch (_home_currency)
								{
									case "USA":
										_exchange_rate = _tools.getSQL_double(@"Select 1/per_usd from currency_history  where currency = 'CAD' and currency_history.date<=@v0  order by currency_history.date desc limit 1", new object[] { format_date });
										break;
									case "EUR":
										_exchange_rate = _tools.getSQL_double(@"1/((Select per_usd from currency_history  where currency = 'CAD' and currency_history.date<=@v0 order by currency_history.date desc limit 1)/(Select per_usd from currency_history where currency = 'EUR' and currency_history.date<=@v1 order by currency_history.date desc limit 1))", new object[] { format_date,format_date });
										break;
								}
							break;
						case "EUR":
							switch (_home_currency)
								{
									case "USA":
										_exchange_rate = _tools.getSQL_double(@"Select 1/per_usd from currency_history  where currency = 'EUR' and currency_history.date<=@v0  order by currency_history.date desc limit 1", new object[] { format_date });
										break;
									case "CAN":
										_exchange_rate = _tools.getSQL_double(@"Select per_usd/(Select per_usd from currency_history  where currency = 'EUR' and currency_history.date<=@v0 order by currency_history.date desc limit 1) from currency_history where currency = 'CAD' and currency_history.date<=@v1 order by currency_history.date desc limit 1", new object[] { format_date,format_date });
										break;
								}
							break;
					}

				}
      
           
         
			return _exchange_rate;
	

			}

		}
	}